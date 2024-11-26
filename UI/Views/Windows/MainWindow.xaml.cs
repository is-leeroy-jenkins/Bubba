// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-23-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-23-2024
// ******************************************************************************************
// <copyright file="MainWindow.xaml.cs" company="Terry D. Eppler">
//    Bubba is a small windows (wpf) application for interacting with
//    Chat GPT that's developed in C-Sharp under the MIT license
// 
//    Copyright ©  2020-2024 Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the “Software”),
//    to deal in the Software without restriction,
//    including without limitation the rights to use,
//    copy, modify, merge, publish, distribute, sublicense,
//    and/or sell copies of the Software,
//    and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
// 
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
// 
//    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT.
//    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//    DEALINGS IN THE SOFTWARE.
// 
//    You can contact me at:  terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   MainWindow.xaml.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;
    using System.Windows;
    using System.Threading;
    using ToastNotifications;
    using ToastNotifications.Lifetime;
    using ToastNotifications.Messages;
    using ToastNotifications.Position;
    using System.Speech.Recognition;
    using System.Speech.Synthesis;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Syncfusion.SfSkinManager;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "AssignNullToNotNullAttribute" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    public partial class MainWindow : Window, INotifyPropertyChanged, IDisposable
    {
        private const string KEY = "sk-proj-eTIELWTlG8lKT3hpqgq7a3vmB6lBVKo"
            + "GBlkoHhu0KqWqsnxyRq9bpEz0N1xZKcOxlyDbLJFCu"
            + "YT3BlbkFJiQGzglbEgyZB7O9FsBi4bJTO0WEg-"
            + "xddgKbywZr1o4bbn0HtNQlSU3OALS0pfMuifvMcy2XPAA";

        /// <summary>
        /// The provider
        /// </summary>
        private protected Provider _provider;

        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The path
        /// </summary>
        private protected object _entry = new object( );

        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// The seconds
        /// </summary>
        private protected int _seconds;

        /// <summary>
        /// The update status
        /// </summary>
        private protected Action _statusUpdate;

        /// <summary>
        /// The timer callback
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// The HTTP client
        /// </summary>
        private protected HttpClient _httpClient;

        /// <summary>
        /// The time
        /// </summary>
        private protected int _time;

        /// <summary>
        /// The openai API key
        /// </summary>
        private protected string OPENAI_API_KEY = KEY;

        /// <summary>
        /// An alternative to sampling with temperature,
        /// called nucleus sampling, where the model considers
        /// the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability
        /// mass are considered. We generally recommend altering this
        /// or temperature but not both.
        /// </summary>
        private protected double _topPercent;

        /// <summary>
        /// A number between -2.0 and 2.0  Positive value decrease the
        /// model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected double _temperature;

        /// <summary>
        /// An upper bound for the number of tokens
        /// that can be generated for a completion
        /// </summary>
        private protected int _maximumTokens;

        /// <summary>
        /// A number between -2.0 and 2.0. Positive values penalize new
        /// tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected double _frequency;

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        private protected double _presence;

        /// <summary>
        /// The models
        /// </summary>
        private protected IList<string> _models;

        /// <summary>
        /// The chat model
        /// </summary>
        private protected string _chatModel;

        /// <summary>
        /// The speech recognition engine
        /// </summary>
        private protected SpeechRecognitionEngine _engine;

        /// <summary>
        /// The speech synthesizer
        /// </summary>
        private protected SpeechSynthesizer _synthesizer;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.MainWindow" /> class.
        /// </summary>
        public MainWindow( )
        {
            // Theme Properties
            SfSkinManager.SetTheme( this, new Theme( "FluentDark" ) );

            // Window Settings
            InitializeComponent( );
            RegisterCallbacks( );
            InitializeDelegates( );
            _temperature = 0.7;
            _maximumTokens = 2048;

            // Event Wiring
            Loaded += OnLoad;
            Closed += OnClosed;
        }

        /// <summary>
        /// An upper bound for the number of tokens
        /// that can be generated for a completion
        /// </summary>
        /// <value>
        /// The maximum tokens.
        /// </value>
        public int MaximumTokens
        {
            get
            {
                return _maximumTokens;
            }
            set
            {
                if( _maximumTokens != value )
                {
                    _maximumTokens = value;
                    OnPropertyChanged( nameof( MaximumTokens ) );
                }
            }
        }

        /// <summary>
        /// A number between -2.0 and 2.0  Positive value decrease the
        /// model's likelihood to repeat the same line verbatim.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        public double Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                if( _temperature != value )
                {
                    _temperature = value;
                    OnPropertyChanged( nameof( Temperature ) );
                }
            }
        }

        /// <summary>
        /// A number between -2.0 and 2.0. Positive values penalize new
        /// tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public double Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                if( _frequency != value )
                {
                    _frequency = value;
                    OnPropertyChanged( nameof( Frequency ) );
                }
            }
        }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <value>
        /// The presence.
        /// </value>
        public double Presence
        {
            get
            {
                return _presence;
            }
            set
            {
                if( _presence != value )
                {
                    _presence = value;
                    OnPropertyChanged( nameof( Presence ) );
                }
            }
        }

        /// <summary>
        /// Registers the callbacks.
        /// </summary>
        private protected void RegisterCallbacks( )
        {
            try
            {
                FirstButton.Click += OnFirstButtonClick;
                PreviousButton.Click += OnPreviousButtonClick;
                NextButton.Click += OnNextButtonClick;
                LastButton.Click += OnLastButtonClick;
                LookupButton.Click += OnLookupButtonClick;
                RefreshButton.Click += OnRefreshButtonClick;
                ModelComboBox.SelectionChanged += OnModelSelectionChanged;
                MenuButton.Click += OnToggleButtonClick;
                TemperatureTextBox.TextChanged += OnTextBoxInputChanged;
                PresenceTextBox.TextChanged += OnTextBoxInputChanged;
                FrequencyTextBox.TextChanged += OnTextBoxInputChanged;
                TopPercentTextBox.TextChanged += OnTextBoxInputChanged;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the delegates.
        /// </summary>
        private protected void InitializeDelegates( )
        {
            try
            {
                _statusUpdate += UpdateStatus;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the timer.
        /// </summary>
        private void InitializeTimer( )
        {
            try
            {
                _timerCallback += UpdateStatus;
                _timer = new Timer( _timerCallback, null, 0, 260 );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the tool strip.
        /// </summary>
        private void InitializeToolStrip( )
        {
            try
            {
                SetToolbarVisibility( false );
            }
            catch( Exception ex )
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Invokes if needed.
        /// </summary>
        /// <param name="action">The action.</param>
        private void InvokeIf( Action action )
        {
            try
            {
                ThrowIf.Null( action, nameof( action ) );
                if( Dispatcher.CheckAccess( ) )
                {
                    action?.Invoke( );
                }
                else
                {
                    Dispatcher.BeginInvoke( action );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Invokes if.
        /// </summary>
        /// <param name="action">The action.</param>
        private void InvokeIf( Action<object> action )
        {
            try
            {
                ThrowIf.Null( action, nameof( action ) );
                if( Dispatcher.CheckAccess( ) )
                {
                    action?.Invoke( null );
                }
                else
                {
                    Dispatcher.BeginInvoke( action );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Begins the initialize.
        /// </summary>
        private void Busy( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = true;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Ends the initialize.
        /// </summary>
        private void Chill( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = false;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Creates a notifier.
        /// </summary>
        /// <returns>
        /// Notifier
        /// </returns>
        private Notifier CreateNotifier( )
        {
            try
            {
                var _position = new PrimaryScreenPositionProvider( Corner.BottomRight, 10, 10 );
                var _lifeTime = new TimeAndCountBasedLifetimeSupervisor( TimeSpan.FromSeconds( 5 ),
                    MaximumNotificationCount.UnlimitedNotifications( ) );

                return new Notifier( cfg =>
                {
                    cfg.LifetimeSupervisor = _lifeTime;
                    cfg.PositionProvider = _position;
                    cfg.Dispatcher = Application.Current.Dispatcher;
                } );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( Notifier );
            }
        }

        /// <summary>
        /// Clears the controls.
        /// </summary>
        private void ClearControls( )
        {
            try
            {
                UserTextBox.Text = "1";
                PresenceTextBox.Text = "0.0";
                TemperatureTextBox.Text = "0.0";
                FrequencyTextBox.Text = "0.0";
                MaxTokensTextBox.Text = "2048";
                SystemTextBox.Text = "";
                UserTextBox.Text = "";
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Gets the hyper parameter.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private protected HyperParameter GetHyperParameter(string input)
        {
            try
            {
                ThrowIf.Empty(input, nameof(input));
                var _names = Enum.GetNames(typeof(HyperParameter));
                if(_names.Contains(input))
                {
                    return (HyperParameter)Enum.Parse(typeof(HyperParameter), input);
                }
                else
                {
                    return default(HyperParameter);
                }
            }
            catch(Exception ex)
            {
                Fail(ex);
                return default(HyperParameter);
            }
        }

        /// <summary>
        /// Sends the notification.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void SendNotification( string message )
        {
            try
            {
                ThrowIf.Null( message, nameof( message ) );
                var _notification = CreateNotifier( );
                _notification.ShowInformation( message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Shows the splash message.
        /// </summary>
        private void SendMessage( string message )
        {
            try
            {
                ThrowIf.Null( message, nameof( message ) );
                var _splash = new SplashMessage( message )
                {
                    Owner = this
                };

                _splash.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sends the HTTP message.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <returns></returns>
        public string SendHttpMessage( string question )
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            // text-davinci-002, text-davinci-003
            var _model = ModelComboBox.Text;
            var _url = "https://api.openai.com/v1/completions";
            if( _model.IndexOf( "gpt-3.5-turbo" ) != -1 )
            {
                //Chat GTP 4 https://openai.com/research/gpt-4
                _url = "https://api.openai.com/v1/chat/completions";
            }

            var _request = WebRequest.Create( _url );
            _request.Method = "POST";
            _request.ContentType = "application/json";
            _request.Headers.Add( "Authorization", "Bearer " + KEY );
            var _maxTokens = int.Parse( MaxTokensTextBox.Text );// 2048
            var _temp = double.Parse( TemperatureTextBox.Text );// 0.5
            if( ( _temp < 0d ) | ( _temp > 1d ) )
            {
                var _msg = "Randomness has to be between 0 and 1 "
                    + "with higher values resulting in more random text";

                SendMessage( _msg );
                return "";
            }

            var _userId = UserLabel.Content;// 1        
            var _data = "";
            if( _model.IndexOf( "gpt-3.5-turbo" ) != -1 )
            {
                _data = "{";
                _data += " \"model\":\"" + _model + "\",";
                _data += " \"messages\": [{\"role\": \"user\", \"content\": \""
                    + PadQuotes( question ) + "\"}]";

                _data += "}";
            }
            else
            {
                _data = "{";
                _data += " \"model\":\"" + _model + "\",";
                _data += " \"prompt\": \"" + PadQuotes( question ) + "\",";
                _data += " \"max_tokens\": " + _maxTokens + ",";
                _data += " \"user\": \"" + _userId + "\", ";
                _data += " \"temperature\": " + _temp + ", ";

                // Number between -2.0 and 2.0  Positive value decrease the
                // model's likelihood to repeat the same line verbatim.
                _data += " \"frequency_penalty\": 0.0" + ", ";

                // Number between -2.0 and 2.0. Positive values increase the model's
                // likelihood to talk about new topics.
                _data += " \"presence_penalty\": 0.0" + ", ";

                // Up to 4 sequences where the API will stop generating further tokens.
                // The returned text will not contain the stop sequence.
                _data += " \"stop\": [\"#\", \";\"]";
                _data += "}";
            }

            using var _streamWriter = new StreamWriter( _request.GetRequestStream( ) );
            _streamWriter.Write( _data );
            _streamWriter.Flush( );
            _streamWriter.Close( );
            var _response = _request.GetResponse( );
            var _stream = _response.GetResponseStream( );
            var _reader = new StreamReader( _stream );
            var _json = _reader.ReadToEnd( );
            var _script = _json.SerializeToJavaScript( );
            var _objects = new Dictionary<string, object>( );
            var _choices = _objects.Keys.ToList( );
            var _choice = _choices[ 0 ];
            var _message = "";
            if( _model.IndexOf( "gpt-3.5-turbo" ) != -1 )
            {
                var _key = _objects[ "message" ];
                var _kvp = new Dictionary<string, object>( );
            }
            else
            {
                _message = ( string )_objects[ "text" ];
            }

            return _message;
        }

        /// <summary>
        /// Pads the quotes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private string PadQuotes( string input )
        {
            if( input.IndexOf( "\\" ) != -1 )
            {
                input = input.Replace( "\\", @"\\" );
            }

            if( input.IndexOf( "\n\r" ) != -1 )
            {
                input = input.Replace( "\n\r", @"\n" );
            }

            if( input.IndexOf( "\r" ) != -1 )
            {
                input = input.Replace( "\r", @"\r" );
            }

            if( input.IndexOf( "\n" ) != -1 )
            {
                input = input.Replace( "\n", @"\n" );
            }

            if( input.IndexOf( "\t" ) != -1 )
            {
                input = input.Replace( "\t", @"\t" );
            }

            if( input.IndexOf( "\"" ) != -1 )
            {
                return input.Replace( "\"", @"""" );
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// Sets the models.
        /// </summary>
        private async void PopulateModelsAsync( )
        {
            try
            {
                var _url = "https://api.openai.com/v1/models";
                _httpClient = new HttpClient( );
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue( "Bearer", KEY );

                var _responseMessage = await _httpClient.GetAsync( _url );
                _responseMessage.EnsureSuccessStatusCode( );
                var _body = await _responseMessage.Content.ReadAsStringAsync( );
                var _aiModels = new List<string>( );
                using var _document = JsonDocument.Parse( _body );
                var _root = _document.RootElement;
                if( _root.TryGetProperty( "data", out var _data )
                    && _data.ValueKind == JsonValueKind.Array )
                {
                    foreach( var _item in _data.EnumerateArray( ) )
                    {
                        if( _item.TryGetProperty( "id", out var _element ) )
                        {
                            _aiModels.Add( _element.GetString( ) );
                        }
                    }
                }

                _aiModels.Sort();
                ModelComboBox.Items.Clear( );
                Application.Current.Dispatcher.BeginInvoke( ( ) =>
                {
                    foreach( var _model in _aiModels )
                    {
                        ModelComboBox.Items.Add( _model );
                    }
                } );
            }
            catch( HttpRequestException ex )
            {
                Fail( ex );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Speeches to text.
        /// </summary>
        private void SpeechToText( )
        {
            if( _engine != null )
            {
                _engine.RecognizeAsync( RecognizeMode.Multiple );
                return;
            }

            _engine = new SpeechRecognitionEngine( new CultureInfo( "en-US" ) );
            _engine.LoadGrammar( new DictationGrammar( ) );
            _engine.SpeechRecognized += OnSpeechRecognized;
            _engine.SpeechHypothesized += OnSpeechHypothesized;
            _engine.SetInputToDefaultAudioDevice( );
            _engine.RecognizeAsync( RecognizeMode.Multiple );
        }

        /// <summary>
        /// Speeches to text.
        /// </summary>
        /// <param name="input">The input.</param>
        public void SpeechToText( string input )
        {
            //if( MuteCheckBox.IsChecked == true )
            {
                return;
            }

            if( _synthesizer == null )
            {
                _synthesizer = new SpeechSynthesizer( );
                _synthesizer.SetOutputToDefaultAudioDevice( );
            }

            if( VoiceComboBox.Text != "" )
            {
                _synthesizer.SelectVoice( VoiceComboBox.Text );
            }

            _synthesizer.Speak( input );
        } 

        private protected void SetHyperParameters( )
        {
            try
            {
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        private void UpdateStatus( )
        {
            try
            {
                StatusLabel.Content = DateTime.Now.ToLongTimeString( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        private void UpdateStatus( object state )
        {
            try
            {
                InvokeIf( _statusUpdate );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Moves to first.
        /// </summary>
        private protected void MoveToFirst( )
        {
            try
            {
                var _message = "NOT YET IMPLEMENTED!";
                SendMessage( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Moves to previous.
        /// </summary>
        private protected void MoveToPrevious( )
        {
            try
            {
                var _message = "NOT YET IMPLEMENTED!";
                SendMessage( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Moves to next.
        /// </summary>
        private protected void MoveToNext( )
        {
            try
            {
                var _message = "NOT YET IMPLEMENTED!";
                SendMessage( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Moves to last.
        /// </summary>
        private protected void MoveToLast( )
        {
            try
            {
                var _message = "NOT YET IMPLEMENTED!";
                SendMessage( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// 
        /// Shows the items.
        /// </summary>
        private void SetToolbarVisibility( bool visible = true )
        {
            try
            {
                if( visible )
                {
                    FirstButton.Visibility = Visibility.Visible;
                    PreviousButton.Visibility = Visibility.Visible;
                    NextButton.Visibility = Visibility.Visible;
                    LastButton.Visibility = Visibility.Visible;
                    ToolStripTextBox.Visibility = Visibility.Visible;
                    LookupButton.Visibility = Visibility.Visible;
                    RefreshButton.Visibility = Visibility.Visible;
                    DeleteButton.Visibility = Visibility.Visible;
                    ToolStripTextBox.Visibility = Visibility.Visible;
                    ToolStripComboBox.Visibility = Visibility.Visible;
                }
                else
                {
                    FirstButton.Visibility = Visibility.Hidden;
                    PreviousButton.Visibility = Visibility.Hidden;
                    NextButton.Visibility = Visibility.Hidden;
                    LastButton.Visibility = Visibility.Hidden;
                    ToolStripTextBox.Visibility = Visibility.Hidden;
                    LookupButton.Visibility = Visibility.Hidden;
                    RefreshButton.Visibility = Visibility.Hidden;
                    DeleteButton.Visibility = Visibility.Hidden;
                    ToolStripTextBox.Visibility = Visibility.Hidden;
                    ToolStripComboBox.Visibility = Visibility.Hidden;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Runs the client application.
        /// </summary>
        private void RunClient( )
        {
            try
            {
                switch( _provider )
                {
                    case Provider.Access:
                    {
                        DataMinion.RunAccess( );
                        break;
                    }
                    case Provider.SqlCe:
                    {
                        DataMinion.RunSqlCe( );
                        break;
                    }
                    case Provider.SQLite:
                    {
                        DataMinion.RunSQLite( );
                        break;
                    }
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnLoad( object sender, RoutedEventArgs e )
        {
            try
            {
                var _apiKey = KEY;
                if( _apiKey == "" )
                {
                    var _msg = "Please enter your OpenAI API key file.";
                    SendMessage( _msg );
                    Application.Current.Shutdown( );
                }
                else
                {
                    OPENAI_API_KEY = _apiKey;
                }

                PopulateModelsAsync( );
                VoiceComboBox.Items.Clear( );
                var _synth = new SpeechSynthesizer( );
                var _voices = _synth.GetInstalledVoices( );
                for( var _index = 0; _index < _voices.Count; _index++ )
                {
                    var _voice = _voices[ _index ];
                    VoiceComboBox.Items.Add( _voice.VoiceInfo.Name );
                }


                InitializeToolStrip( );
                InitializeTimer( );
                ProgressBar.Visibility = Visibility.Hidden;
                SpeechLabel.Visibility = Visibility.Hidden;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        public void OnPropertyChanged( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        /// <summary>
        /// Called when [text box input changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnTextBoxInputChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( sender is MetroTextBox _textBox )
                {
                    var _tag = _textBox?.Tag.ToString( ); 
                    if( !string.IsNullOrEmpty( _tag ) )
                    {
                        switch( _tag )
                        {
                            case "Frequency":
                            case "Presence":
                            case "Temperature":
                            {
                                var _temp = _textBox.Text;
                                var _value = double.Parse( _temp );
                                _textBox.Text = _value.ToString( "N2" );
                                break;
                            }
                            case "TopPercent":
                            {
                                var _temp = _textBox.Text;
                                var _top = double.TryParse( _temp, out var _value );
                                if( _top )
                                {
                                    _textBox.Text = _value.ToString( "P1" );
                                }

                                break;
                            }
                        }
                    }
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [closing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CancelEventArgs"/>
        /// instance containing the event data.</param>
        private void OnClosed( object sender, EventArgs e )
        {
            try
            {
                _timer?.Dispose( );
                Environment.Exit( 0 );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [calculator menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnCalculatorMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _calculator = new CalculatorWindow
                {
                    Owner = this,
                    Topmost = true
                };

                _calculator.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [file menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnFileMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _fileBrowser = new FileBrowser
                {
                    Owner = this,
                    Topmost = true
                };

                _fileBrowser.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [folder menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnFolderMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _fileBrowser = new FileBrowser
                {
                    Owner = this,
                    Topmost = true
                };

                _fileBrowser.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [control panel option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnControlPanelOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WinMinion.LaunchControlPanel( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [task manager option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnTaskManagerOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WinMinion.LaunchTaskManager( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [close option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnCloseOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                Application.Current.Shutdown( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [chrome option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// containing the event data.</param>
        private void OnChromeOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunChrome( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [edge option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnEdgeOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunEdge( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [firefox option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// containing the event data.</param>
        private void OnFirefoxOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunFirefox( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [listen checked changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnListenCheckedChanged( object sender, RoutedEventArgs e )
        {
           // if( ListenCheckBox.IsChecked == true )
            {
                SpeechLabel.Content = "";
                SpeechLabel.Visibility = Visibility.Visible;
                SpeechToText( );
            }
            //else
            {
                _engine.RecognizeAsyncStop( );
                SpeechLabel.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Called when [mute checked box changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnMuteCheckedBoxChanged( object sender, RoutedEventArgs e )
        {
            //if( MuteCheckBox.IsChecked == true )
            {
                VoiceLabel.Visibility = Visibility.Hidden;
                VoiceComboBox.Visibility = Visibility.Hidden;
            }
            //else
            {
                VoiceLabel.Visibility = Visibility.Visible;
                VoiceComboBox.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Called when [speech recognized].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeechRecognizedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSpeechRecognized( object sender, SpeechRecognizedEventArgs e )
        {
            // Reset Hypothesized text
            SpeechLabel.Content = "";
            if( UserTextBox.Text != "" )
            {
                UserTextBox.Text += "\n";
            }

            var _text = e.Result.Text;
            UserTextBox.Text += _text;
        }

        /// <summary>
        /// Called when [speech hypothesized].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeechHypothesizedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSpeechHypothesized( object sender, SpeechHypothesizedEventArgs e )
        {
            var _text = e.Result.Text;
            SpeechLabel.Content = _text;
        }

        /// <summary>
        /// Called when [send button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSendButtonClick( object sender, RoutedEventArgs e )
        {
            {
                var _question = UserTextBox.Text;
                if( string.IsNullOrEmpty( _question ) )
                {
                    MessageBox.Show( "Type in your question!" );
                    UserTextBox.Focus( );
                    return;
                }

                if( SystemTextBox.Text != "" )
                {
                    SystemTextBox.AppendText( "\r\n" );
                }

                SystemTextBox.AppendText( "User: " + _question + "\r\n" );
                UserTextBox.Text = "";
                try
                {
                    var _answer = SendHttpMessage( _question ) + "";
                    SystemTextBox.AppendText( "Bubba GPT: "
                        + _answer.Replace( "\n", "\r\n" ).Trim( ) );

                    SpeechToText( _answer );
                }
                catch( Exception ex )
                {
                    SystemTextBox.AppendText( "Error: " + ex.Message );
                }
            }
        }

        /// <summary>
        /// Called when [clear button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnClearButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                ClearControls( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [model ComboBox selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnModelSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _chatModel = ModelComboBox.SelectedValue.ToString( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [first button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnFirstButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                MoveToFirst( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [previous button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnPreviousButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                MoveToPrevious( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [next button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnNextButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                MoveToNext( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [last button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnLastButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                MoveToLast( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [lookup button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnLookupButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _message = "NOT YET IMPLEMENTED!";
                SendMessage( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [toggle button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnToggleButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                SetToolbarVisibility( !FirstButton.IsVisible );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [refresh button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnRefreshButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                ClearControls( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c>
        /// to release both managed
        /// and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose( bool disposing )
        {
            if( disposing )
            {
                _timer?.Dispose( );
                _httpClient?.Dispose( );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks
        /// associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}