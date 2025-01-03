﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 12-11-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        12-11-2024
// ******************************************************************************************
// <copyright file="ChatWindow.xaml.cs" company="Terry D. Eppler">
//    Bubba is a small and simple windows (wpf) application for interacting with the OpenAI API
//    that's developed in C-Sharp under the MIT license.C#.
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
//   ChatWindow.xaml.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Speech.Recognition;
    using System.Speech.Synthesis;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using CefSharp;
    using Microsoft.Office.Interop.Outlook;
    using OpenTK.Platform.Windows;
    using Syncfusion.PMML;
    using Syncfusion.SfSkinManager;
    using Syncfusion.Windows.Edit;
    using ToastNotifications;
    using ToastNotifications.Lifetime;
    using ToastNotifications.Messages;
    using ToastNotifications.Position;
    using Action = System.Action;
    using Exception = System.Exception;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "LoopCanBePartlyConvertedToQuery" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "AssignNullToNotNullAttribute" ) ]
    [ SuppressMessage( "ReSharper", "UnusedParameter.Global" ) ]
    [ SuppressMessage( "ReSharper", "LocalVariableHidesMember" ) ]
    [ SuppressMessage( "ReSharper", "CanSimplifyDictionaryLookupWithTryGetValue" ) ]
    public partial class ChatWindow : Window, INotifyPropertyChanged
    {
        private const string KEY = "sk-proj-eTIELWTlG8lKT3hpqgq7a3vmB6lBVKo"
            + "GBlkoHhu0KqWqsnxyRq9bpEz0N1xZKcOxlyDbLJFCu"
            + "YT3BlbkFJiQGzglbEgyZB7O9FsBi4bJTO0WEg-"
            + "xddgKbywZr1o4bbn0HtNQlSU3OALS0pfMuifvMcy2XPAA";

        /// <summary>
        /// The system prompt
        /// </summary>
        private string _systemPrompt;

        /// <summary>
        /// The user prompt
        /// </summary>
        private string _userPrompt;

        /// <summary>
        /// The role
        /// </summary>
        private protected string _role;

        /// <summary>
        /// The role
        /// </summary>
        private protected string _generation;

        /// <summary>
        /// The role
        /// </summary>
        private protected string _voice;

        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The entry
        /// </summary>
        private protected object _entry = new object( );

        /// <summary>
        /// The endpoint
        /// </summary>
        private protected string _endpoint;

        /// <summary>
        /// The provider
        /// </summary>
        private protected Provider _provider;

        /// <summary>
        /// The language
        /// </summary>
        private protected string _language;

        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// The HTTP client
        /// </summary>
        private protected HttpClient _httpClient;

        /// <summary>
        /// The assembly
        /// </summary>
        public static Assembly Assembly;

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
        /// The store
        /// </summary>
        private protected bool _store;

        /// <summary>
        /// The stream
        /// </summary>
        private protected bool _stream;

        /// <summary>
        /// The image size
        /// </summary>
        private protected string _imageSize;

        /// <summary>
        /// A number between 0 and 2.0
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// Default=1
        /// </summary>
        private protected double _temperature;

        /// <summary>
        /// An upper bound for the number of tokens
        /// that can be generated for a completion
        /// </summary>
        private protected int _maxCompletionTokens;

        /// <summary>
        /// A number between -2.0 and 2.0. Positive values penalize new
        /// tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected double _frequency;

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// increasing the model's likelihood to talk about new topics.
        /// </summary>
        private protected double _presence;

        /// <summary>
        /// The models
        /// </summary>
        private protected IList<string> _models;

        /// <summary>
        /// The chat model
        /// </summary>
        private protected string _model;

        /// <summary>
        /// The speech recognition engine
        /// </summary>
        private protected SpeechRecognitionEngine _engine;

        /// <summary>
        /// The speech synthesizer
        /// </summary>
        private protected SpeechSynthesizer _synthesizer;

        /// <summary>
        /// The full screen
        /// </summary>
        private protected bool _fullScreen;

        /// <summary>
        /// The time
        /// </summary>
        private protected int _time;

        /// <summary>
        /// The number
        /// </summary>
        private protected int _number;

        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// The timer callback
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <summary>
        /// The status update
        /// </summary>
        private protected Action _statusUpdate;

        /// <summary>
        /// The time label
        /// </summary>
        public MetroLabel TimeLabel;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.ChatWindow" /> class.
        /// </summary>
        public ChatWindow( )
        {
            // Theme Properties
            SfSkinManager.SetTheme( this, new Theme( "FluentDark", App.Controls ) );

            // Window Initialization
            InitializeComponent( );
            RegisterCallbacks( );
            InitializeDelegates( );
            InitializeToolStrip( );

            // GPT Parameters
            _store = false;
            _stream = false;
            _temperature = 1;
            _topPercent = 1.0D;
            _presence = 0.0D;
            _frequency = 0.0D;
            _maxCompletionTokens = 2048;

            // Event Wiring
            Loaded += OnLoad;
            Closing += OnClosing;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="Store"/> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        public bool Store
        {
            get
            {
                return _store;
            }
            set
            {
                if( _store != value )
                {
                    _store = value;
                    OnPropertyChanged( nameof( Store ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="Stream"/> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
        public bool Stream
        {
            get
            {
                return _stream;
            }
            set
            {
                if( _stream != value )
                {
                    _stream = value;
                    OnPropertyChanged( nameof( Stream ) );
                }
            }
        }

        /// <summary>
        /// An upper bound for the number of tokens
        /// that can be generated for a completion
        /// </summary>
        /// <value>
        /// The maximum tokens.
        /// </value>
        public int MaxCompletionTokens
        {
            get
            {
                return _maxCompletionTokens;
            }
            set
            {
                if( _maxCompletionTokens != value )
                {
                    _maxCompletionTokens = value;
                    OnPropertyChanged( nameof( MaxCompletionTokens ) );
                }
            }
        }

        /// <summary>
        /// A number between 0 and 2.0
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// Default=1
        /// </summary>
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
        /// Gets or sets the endpoint.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        public string Endpoint
        {
            get
            {
                return _endpoint;
            }
            set
            {
                if( _endpoint != value )
                {
                    _endpoint = value;
                    OnPropertyChanged( nameof( EndPoint ) );
                }
            }
        }

        /// <summary>
        /// Initializes the title.
        /// </summary>
        private void InitializeTitle( )
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
        /// Initializes the label.
        /// </summary>
        private void InitializeLabel( )
        {
            try
            {
                SpeechLabel.Visibility = Visibility.Hidden;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the delegates.
        /// </summary>
        private void InitializeDelegates( )
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
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the user system text box.
        /// </summary>
        private void InitializeChatEditor( )
        {
            try
            {
                ChatEditor.EnableOutlining = true;
                ChatEditor.EnableIntellisense = false;
                ChatEditor.DocumentLanguage = Languages.Text;
                ChatEditor.IsMultiLine = true;
                ChatEditor.IsUndoEnabled = true;
                ChatEditor.IsRedoEnabled = true;
                ChatEditor.SelectionBackground = _theme.SteelBlueBrush;
                ChatEditor.SelectionForeground = _theme.WhiteForeground;
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
        public void InvokeIf( Action action )
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
        public void InvokeIf( Action<object> action )
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
        /// Registers the callbacks.
        /// </summary>
        private protected void RegisterCallbacks( )
        {
            try
            {
                ToolStripTextBox.TextChanged += OnToolStripTextBoxTextChanged;
                FirstButton.Click += OnFirstButtonClick;
                PreviousButton.Click += OnPreviousButtonClick;
                NextButton.Click += OnNextButtonClick;
                LastButton.Click += OnLastButtonClick;
                RefreshButton.Click += OnRefreshButtonClick;
                MenuButton.Click += OnToggleButtonClick;
                BrowserButton.Click += OnWebBrowserButtonClick;
                TemperatureTextBox.TextChanged += OnParameterTextBoxChanged;
                PresenceTextBox.TextChanged += OnParameterTextBoxChanged;
                FrequencyTextBox.TextChanged += OnParameterTextBoxChanged;
                TopPercentTextBox.TextChanged += OnParameterTextBoxChanged;
                DeleteButton.Click += OnDeleteButtonClick;
                ClearButton.Click += OnClearButtonClick;
                SendButton.Click += OnSendButtonClick;
                LanguageListBox.SelectionChanged += OnSelectedLanguageChanged;
                ListenCheckBox.Checked += OnListenCheckedChanged;
                MuteCheckBox.Checked += OnMuteCheckedBoxChanged;
                StoreCheckBox.Checked += OnStoreBoxChecked;
                StreamCheckBox.Checked += OnStreamBoxChecked;
                GenerationComboBox.SelectionChanged += OnSelectedGenerationChanged;
                ModelComboBox.SelectionChanged += OnSelectedModelChanged;
                GenerationComboBox.SelectionChanged += OnSelectedGenerationChanged;
                ImageSizeComboBox.SelectionChanged += OnSelectedImageSizeChanged;
                RefreshButton.Click += OnRefreshButtonClick;
                LookupButton.Click += OnGoButtonClicked;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Clears the callbacks.
        /// </summary>
        private protected virtual void ClearCallbacks( )
        {
            try
            {
                FirstButton.Click -= OnFirstButtonClick;
                PreviousButton.Click -= OnPreviousButtonClick;
                NextButton.Click -= OnNextButtonClick;
                LastButton.Click -= OnLastButtonClick;
                RefreshButton.Click -= OnRefreshButtonClick;
                ModelComboBox.SelectionChanged -= OnSelectedModelChanged;
                MenuButton.Click -= OnToggleButtonClick;
                TemperatureTextBox.TextChanged -= OnParameterTextBoxChanged;
                PresenceTextBox.TextChanged -= OnParameterTextBoxChanged;
                FrequencyTextBox.TextChanged -= OnParameterTextBoxChanged;
                TopPercentTextBox.TextChanged -= OnParameterTextBoxChanged;
                BrowserButton.Click -= OnWebBrowserButtonClick;
                LookupButton.Click -= OnGoButtonClicked;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Clears the callbacks.
        /// </summary>
        private protected virtual void ClearDelegates( )
        {
            try
            {
                _timerCallback = null;
                _statusUpdate = null;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Clears the controls.
        /// </summary>
        private void ClearChatControls( )
        {
            try
            {
                ClearTextBoxes( );
                ClearSliders( );
                ClearComboBoxes( );
                ClearCheckBoxes( );
                ClearListBoxes( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Clears the parameters.
        /// </summary>
        private void ClearParameters( )
        {
            try
            {
                _store = false;
                _stream = false;
                _model = "";
                _imageSize = "";
                _endpoint = "";
                _number = 1;
                _maxCompletionTokens = 2048;
                _temperature = 1.0;
                _topPercent = 1.0;
                _frequency = 0.0;
                _presence = 0.0;
                _language = "";
                _voice = "";
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Cleans the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private string CleanUrl( string url )
        {
            try
            {
                ThrowIf.Null( url, nameof( url ) );
                if( url.BeginsWith( "about:" ) )
                {
                    return "";
                }

                url = url.RemovePrefix( "http://" );
                url = url.RemovePrefix( "https://" );
                url = url.RemovePrefix( "file://" );
                url = url.RemovePrefix( "/" );
                return url.DecodeUrlForFilePath( );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Clears the filter.
        /// </summary>
        private void ClearCheckBoxes( )
        {
            try
            {
                MuteCheckBox.IsChecked = false;
                ListenCheckBox.IsChecked = false;
                StoreCheckBox.IsChecked = false;
                StreamCheckBox.IsChecked = false;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Clears the list boxes.
        /// </summary>
        private void ClearSliders( )
        {
            try
            {
                PresenceSlider.Value = 0D;
                FrequencySlider.Value = 0D;
                TemperatureSlider.Value = 1D;
                TopPercentSlider.Value = 1D;
                MaxTokenSlider.Value = 2048;
                NumberSlider.Value = 1D;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Clears the combo boxes.
        /// </summary>
        private void ClearComboBoxes( )
        {
            try
            {
                GenerationComboBox.SelectedIndex = -1;
                ModelComboBox.SelectedIndex = -1;
                VoiceComboBox.SelectedIndex = -1;
                ImageSizeComboBox.SelectedIndex = -1;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Clears the list boxes.
        /// </summary>
        private void ClearListBoxes( )
        {
            try
            {
                LanguageListBox.SelectedValue = "Text";
            }
            catch( Exception ex )
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Clears the text boxes.
        /// </summary>
        private void ClearTextBoxes( )
        {
            try
            {
                ChatEditor.Text = _userPrompt;
                ToolStripTextBox.Text = "";
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Clears the selections.
        /// </summary>
        private void ClearSelections()
        {
            try
            {
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Clears the collections.
        /// </summary>
        private void ClearCollections( )
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
        /// Creates the notifier.
        /// </summary>
        /// <returns>
        /// </returns>
        private Notifier CreateNotifier( )
        {
            try
            {
                var _position = new PrimaryScreenPositionProvider( Corner.BottomRight, 10, 10 );
                var _count = MaximumNotificationCount.UnlimitedNotifications( );
                var _lifeTime = 
                    new TimeAndCountBasedLifetimeSupervisor( TimeSpan.FromSeconds( 5 ), _count );

                return new Notifier( cfg =>
                {
                    cfg.LifetimeSupervisor = _lifeTime;
                    cfg.PositionProvider = _position;
                } );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( Notifier );
            }
        }

        /// <summary>
        /// Fades the in asynchronous.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="interval">The interval.</param>
        private async void FadeInAsync( Window form, int interval = 80 )
        {
            try
            {
                ThrowIf.Null( form, nameof( form ) );
                while( form.Opacity < 1.0 )
                {
                    await Task.Delay( interval );
                    form.Opacity += 0.05;
                }

                form.Opacity = 1;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Fades the out asynchronous.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="interval">The interval.</param>
        private async void FadeOutAsync( Window form, int interval = 80 )
        {
            try
            {
                ThrowIf.Null( form, nameof( form ) );
                while( form.Opacity > 0.0 )
                {
                    await Task.Delay( interval );
                    form.Opacity -= 0.05;
                }

                form.Opacity = 0;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Gets the download path.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public string GetDownloadPath( DownloadItem item )
        {
            return item.SuggestedFileName;
        }

        /// <summary>
        /// Gets the application directory.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static string GetApplicationDirectory( string name )
        {
            try
            {
                ThrowIf.Null( name, nameof( name ) );
                var _winXpDir = @"C:\Documents and Settings\All Users\Application Data\";
                return Directory.Exists( _winXpDir )
                    ? _winXpDir + ConfigurationManager.AppSettings[ "Branding" ] + @"\" + name
                    + @"\"
                    : @"C:\ProgramData\" + ConfigurationManager.AppSettings[ "Branding" ] + @"\"
                    + name + @"\";
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the resource stream.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="nameSpace">if set to <c>true</c> [name space].</param>
        /// <returns></returns>
        public Stream GetResourceStream( string fileName, bool nameSpace = true )
        {
            try
            {
                ThrowIf.Null( fileName, nameof( fileName ) );
                var _prefix = "Properties.Resources.";
                return Assembly.GetManifestResourceStream( fileName );
            }
            catch( Exception )
            {
                //ignore exception
            }

            return null;
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void SendMessage( string message )
        {
            try
            {
                ThrowIf.Null( message, nameof( message ) );
                var _splashMessage = new SplashMessage( message );
                _splashMessage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _splashMessage.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sends the notification.
        /// </summary>
        private void SendNotification( string message )
        {
            try
            {
                ThrowIf.Null( message, nameof( message ) );
                var _notify = CreateNotifier( );
                _notify.ShowInformation( message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sends the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void SendErrorMessage( string message )
        {
            try
            {
                ThrowIf.Null( message, nameof( message ) );
                var _notify = CreateNotifier( );
                _notify.ShowError( message );
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
            try
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
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Speeches to text.
        /// </summary>
        /// <param name="input">The input.</param>
        public void SpeechToText( string input )
        {
            try
            {
                ThrowIf.Null( input, nameof( input ) );
                if( MuteCheckBox.IsChecked == true )
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
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sets the hyper parameters.
        /// </summary>
        private protected void SetGptParameters( )
        {
            try
            {
                var _input = ChatEditor.Text;
                if( !string.IsNullOrEmpty( _generation ) )
                {
                    var _msg = "Select a GPT Task!";
                    SendErrorMessage( _msg );
                }
                else
                {
                    _store = StoreCheckBox.IsChecked ?? false;
                    _stream = StreamCheckBox.IsChecked ?? false;
                    _presence = PresenceSlider.Value;
                    _temperature = TemperatureSlider.Value;
                    _topPercent = TopPercentSlider.Value;
                    _frequency = FrequencySlider.Value;
                    _number = int.Parse( NumberTextBox.Text );
                    _maxCompletionTokens = Convert.ToInt32( MaxTokenTextBox.Value );
                    _userPrompt = ( _language == "Text" )
                        ? ChatEditor.Text
                        : "";
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sets the end point.
        /// </summary>
        private void SetEndpoints( )
        {
            try
            {
                var _endpoints = new GptEndPoint( );
                var _domain = _endpoints.BaseUrl;
                switch( _generation )
                {
                    case "Text Generation":
                    {
                        PopulateTextModels( );
                        _endpoint = _endpoints.TextGeneration;
                        TemperatureSlider.Value = 0.6;
                        TopPercentSlider.Value = 0.95;
                        MaxTokenSlider.Value = 200;
                        FrequencySlider.Value = 0.4;
                        PresenceSlider.Value = 0.1;
                        NumberSlider.Value = 1;
                        break;
                    }
                    case "Translations":
                    {
                        PopulateTranslationModels( );
                        _endpoint = _endpoints.Translations;
                        TemperatureSlider.Value = 0.7;
                        MaxTokenSlider.Value = 150;
                        NumberSlider.Value = 1;
                        break;
                    }
                    case "Image Generation":
                    {
                        PopulateImageModels( );
                        _endpoint = _endpoints.ImageGeneration;
                        TopPercentSlider.Value = 1.0;
                        TemperatureSlider.Value = 1.0;
                        NumberSlider.Value = 1;
                        break;
                    }
                    case "Vector Embedding":
                    {
                        PopulateEmbeddingModels( );
                        _endpoint = _endpoints.VectorEmbeddings;
                        NumberSlider.Value = 1;
                        break;
                    }
                    case "Transcriptions":
                    {
                        PopulateTranscriptionModels( );
                        _endpoint = _endpoints.Transcriptions;
                        TemperatureSlider.Value = 0.7;
                        MaxTokenSlider.Value = 150;
                        NumberSlider.Value = 1;
                        break;
                    }
                    case "Vector Stores":
                    {
                        PopulateEmbeddingModels( );
                        _endpoint = _endpoints.VectorStores;
                        break;
                    }
                    case "Speech Generation":
                    {
                        PopulateSpeechModels( );
                        _endpoint = _endpoints.SpeechGeneration;
                        TemperatureSlider.Value = 0.6;
                        MaxTokenSlider.Value = 200;
                        break;
                    }
                    case "Fine-Tuning":
                    {
                        PopulateFineTuningModels( );
                        _endpoint = _endpoints.FineTuning;
                        break;
                    }
                    case "Files":
                    {
                        PopulateTextModels( );
                        _endpoint = _endpoints.Files;
                        break;
                    }
                    case "Uploads":
                    {
                        PopulateTextModels( );
                        _endpoint = _endpoints.Uploads;
                        break;
                    }
                    default:
                    {
                        PopulateTextModels( );
                        _endpoint = _endpoints.TextGeneration;
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
        /// Sets the user document language.
        /// </summary>
        private protected void SetUserDocumentLanguage( )
        {
            try
            {
                var _prefix = @"C:\Users\terry\source\repos\Bubba\";
                if( !string.IsNullOrEmpty( _language ) )
                {
                    ChatEditor.Text = "";
                    switch( _language )
                    {
                        case "Text":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\Text.txt";
                            ChatEditor.DocumentLanguage = Languages.Text;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "C#":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\CSharp.txt";
                            ChatEditor.DocumentLanguage = Languages.CSharp;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "Python":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\Python.txt";
                            ChatEditor.DocumentLanguage = Languages.Text;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "SQL":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\SQL.txt";
                            ChatEditor.DocumentLanguage = Languages.SQL;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "JavaScript":
                        {
                            var _path = _prefix
                                + @"Resources\Documents\Editor\Stubs\JavaScript.txt";

                            ChatEditor.DocumentLanguage = Languages.JScript;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "C/C++":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\C.txt";
                            ChatEditor.DocumentLanguage = Languages.C;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "VB/VBA":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\VBA.txt";
                            ChatEditor.DocumentLanguage = Languages.VisualBasic;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        default:
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\Text.txt";
                            ChatEditor.DocumentLanguage = Languages.Text;
                            ChatEditor.DocumentSource = _path;
                            break;
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
                    ToolStripTextBox.Visibility = Visibility.Visible;
                    ToolStripComboBox.Visibility = Visibility.Visible;
                    CancelButton.Visibility = Visibility.Visible;
                    DeleteButton.Visibility = Visibility.Visible;
                    BrowserButton.Visibility = Visibility.Visible;
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
                    ToolStripTextBox.Visibility = Visibility.Hidden;
                    ToolStripComboBox.Visibility = Visibility.Hidden;
                    CancelButton.Visibility = Visibility.Hidden;
                    DeleteButton.Visibility = Visibility.Hidden;
                    BrowserButton.Visibility = Visibility.Hidden;
                }
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
            var _maxTokens = int.Parse( MaxTokenTextBox.Text ); // 2048

            // 0.5
            var _temp = double.Parse( TemperatureTextBox.Text );
            if( ( _temp < 0d ) | ( _temp > 1d ) )
            {
                var _msg = "Randomness has to be between 0 and 2"
                    + "with higher values resulting in more random text; Default 1 ";

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
                _data += " \"max_completion_tokens\": " + _maxTokens + ",";
                _data += " \"user\": \"" + _userId + "\", ";
                _data += " \"temperature\": " + _temp + ", ";
                _data += " \"frequency_penalty\": 0.0" + ", ";
                _data += " \"presence_penalty\": 0.0" + ", ";
                _data += " \"stop\": [\"#\", \";\"]";
                _data += "}";
            }

            using var _streamWriter = new StreamWriter( _request.GetRequestStream( ) );
            _streamWriter.Write( _data );
            _streamWriter.Flush( );
            _streamWriter.Close( );
            var _json = "";
            using( var _response = _request.GetResponse( ) )
            {
                using var _responseStream = _response.GetResponseStream( );
                using var _reader = new StreamReader( _responseStream );
                _json = _reader.ReadToEnd( );
            }

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
        /// Busies this instance.
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
        /// Chills this instance.
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
        /// Opens the search dialog.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private protected virtual void OpenSearchDialog( double x, double y )
        {
            try
            {
                ThrowIf.Negative( x, nameof( x ) );
                ThrowIf.Negative( x, nameof( x ) );
                var _searchDialog = new SearchDialog
                {
                    Owner = this,
                    Left = x,
                    Top = y
                };

                _searchDialog.Show( );
                _searchDialog.SearchPanelTextBox.Focus( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the chat window.
        /// </summary>
        private void OpenWebBrowser( )
        {
            try
            {
                if( App.ActiveWindows?.ContainsKey( "WebBrowser" ) == true )
                {
                    var _form = ( WebBrowser )App.ActiveWindows[ "WebBrowser" ];
                    _form.Show( );
                }
                else
                {
                    var _web = new WebBrowser
                    {
                        Owner = this,
                        Topmost = true
                    };

                    _web.Show( );
                }

                Hide( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Pads the quotes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// string
        /// </returns>
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
                _httpClient.Timeout = new TimeSpan( 0, 0, 3 );
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue( "Bearer", KEY );

                var _responseMessage = await _httpClient.GetAsync( _url );
                _responseMessage.EnsureSuccessStatusCode( );
                var _body = await _responseMessage.Content.ReadAsStringAsync( );
                var _llms = new List<string>( );
                using var _document = JsonDocument.Parse( _body );
                var _root = _document.RootElement;
                if( _root.TryGetProperty( "data", out var _data )
                    && _data.ValueKind == JsonValueKind.Array )
                {
                    foreach( var _item in _data.EnumerateArray( ) )
                    {
                        if( _item.TryGetProperty( "id", out var _element ) )
                        {
                            _llms.Add( _element.GetString( ) );
                        }
                    }
                }

                _llms.Sort( );
                ModelComboBox.Items.Clear( );
                Dispatcher.BeginInvoke( ( ) =>
                {
                    foreach( var _lm in _llms )
                    {
                        if( !_lm.StartsWith( "ft" ) )
                        {
                            ModelComboBox.Items.Add( _lm );
                        }
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
        /// Populates the domain drop downs.
        /// </summary>
        private void PopulateDomainDropDowns( )
        {
            try
            {
                ToolStripComboBox.Items?.Clear( );
                var _domains = Enum.GetNames( typeof( Domains ) );
                for( var _i = 0; _i < _domains.Length; _i++ )
                {
                    var _dom = _domains[ _i ];
                    if( !string.IsNullOrEmpty( _dom ) )
                    {
                        ToolStripComboBox.Items.Add( _dom );
                    }
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the GPT tasks.
        /// </summary>
        private void PopulateGenerations( )
        {
            try
            {
                var _ept = new GptEndPoint( );
                var _urls = _ept.Data.Keys.ToList( );
                GenerationComboBox.Items.Clear( );
                foreach( var _path in _urls )
                {
                    GenerationComboBox.Items.Add( _path );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the text generation models.
        /// </summary>
        private void PopulateTextModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "gpt-4-turbo" );
                ModelComboBox.Items.Add( "gpt-4-turbo-2024-04-09" );
                ModelComboBox.Items.Add( "gpt-4-turbo-preview" );
                ModelComboBox.Items.Add( "gpt-4-0125-preview" );
                ModelComboBox.Items.Add( "gpt-4-1106-preview" );
                ModelComboBox.Items.Add( "gpt-4" );
                ModelComboBox.Items.Add( "gpt-4-0613" );
                ModelComboBox.Items.Add( "gpt-4-0314" );
                ModelComboBox.Items.Add( "gpt-4-turbo" );
                ModelComboBox.Items.Add( "gpt-4-turbo-2024-04-09" );
                ModelComboBox.Items.Add( "gpt-4-turbo-preview" );
                ModelComboBox.Items.Add( "gpt-4-0125-preview" );
                ModelComboBox.Items.Add( "gpt-4-1106-preview" );
                ModelComboBox.Items.Add( "gpt-4o" );
                ModelComboBox.Items.Add( "gpt-4o-mini" );
                ModelComboBox.Items.Add( "o1-preview" );
                ModelComboBox.Items.Add( "o1-mini" );
                ModelComboBox.Items.Add( "gpt-3.5-turbo" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the image generation models.
        /// </summary>
        private void PopulateImageModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "dall-e-3" );
                ModelComboBox.Items.Add( "dall-e-2" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the translation models.
        /// </summary>
        private void PopulateTranslationModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "whisper-1" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the transcription models.
        /// </summary>
        private void PopulateTranscriptionModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "whisper-1" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the embedding models.
        /// </summary>
        private void PopulateEmbeddingModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "text-embedding-3-small" );
                ModelComboBox.Items.Add( "text-embedding-3-large" );
                ModelComboBox.Items.Add( "text-embedding-ada-002" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the fine tuning models.
        /// </summary>
        private void PopulateFineTuningModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "gpt-4o" );
                ModelComboBox.Items.Add( "gpt-4o-mini" );
                ModelComboBox.Items.Add( "gpt-4" );
                ModelComboBox.Items.Add( "gpt-3.5-turbo" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the speech models.
        /// </summary>
        private void PopulateSpeechModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "tts-1" );
                ModelComboBox.Items.Add( "tts-1-hd" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the language ListBox.
        /// </summary>
        private void PopulateLanguageListBox( )
        {
            try
            {
                LanguageListBox.Items?.Clear( );
                LanguageListBox.Items.Add( "Text" );
                LanguageListBox.Items.Add( "C#" );
                LanguageListBox.Items.Add( "Python" );
                LanguageListBox.Items.Add( "SQL" );
                LanguageListBox.Items.Add( "C/C++" );
                LanguageListBox.Items.Add( "JavaScript" );
                LanguageListBox.Items.Add( "VBA" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the voices.
        /// </summary>
        private void PopulateVoices( )
        {
            try
            {
                var _synth = new SpeechSynthesizer( );
                VoiceComboBox.Items?.Clear( );
                foreach( var _voice in _synth.GetInstalledVoices( ) )
                {
                    var _info = _voice.VoiceInfo;
                    VoiceComboBox.Items.Add( _info.Name );
                }
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
                InitializeTimer( );
                PopulateDomainDropDowns( );
                InitializeToolStrip( );
                InitializeLabel( );
                PopulateModelsAsync( );
                PopulateGenerations( );
                PopulateLanguageListBox( );
                PopulateVoices( );
                SetGptParameters( );
                ClearChatControls( );
                InitializeChatEditor( );
                App.ActiveWindows.Add( "ChatWindow", this );
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
        /// Called when [tab closed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnTabClosed( object sender, RoutedEventArgs e )
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
        /// Called when [status updated].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusMessageEventArgs"/>
        /// instance containing the event data.</param>
        private void OnStatusUpdated( object sender, StatusMessageEventArgs e )
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
        /// Called when [source button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnSourceButtonClick( object sender, EventArgs e )
        {
            try
            {
                var _message = "NOT YET IMPLEMENTED!";
                var _notifier = CreateNotifier( );
                _notifier.ShowInformation( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [store box checked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnStoreBoxChecked( object sender, EventArgs e )
        {
            try
            {
                _store = StoreCheckBox.IsChecked == true;
                var _message = $"The '{nameof( _store )}' data member is {_store.ToString( )} ";
                var _notifier = CreateNotifier( );
                _notifier.ShowInformation( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [stream box checked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnStreamBoxChecked( object sender, EventArgs e )
        {
            try
            {
                _stream = StreamCheckBox.IsChecked == true;
                var _message = $"The '{nameof( _stream )}' data member is {_stream.ToString( )} ";
                var _notifier = CreateNotifier( );
                _notifier.ShowInformation( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [URL text box click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnUrlTextBoxClick( object sender, MouseEventArgs e )
        {
            try
            {
                var _psn = e.GetPosition( this );
                var _searchDialog = new SearchDialog
                {
                    Owner = this,
                    Left = _psn.X - 100,
                    Top = _psn.Y + 50
                };

                _searchDialog.Show( );
                _searchDialog.SearchPanelTextBox.Focus( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [tool strip text box click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnToolStripTextBoxTextChanged( object sender, TextChangedEventArgs e )
        {
            try
            {
                var _text = ToolStripTextBox.Text;
                if( !string.IsNullOrEmpty( _text ) )
                {
                    ChatEditor.Text = _text;
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
        private void OnClosing( object sender, CancelEventArgs e )
        {
            try
            {
                _timer?.Dispose( );
                SfSkinManager.Dispose( this );
                App.ActiveWindows.Clear( );
                Environment.Exit( 0 );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [close button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnCloseButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                Close( );
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnCalculatorMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _calculator = new CalculatorWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnFileMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _fileBrowser = new FileBrowser
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnFolderMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _fileBrowser = new FileBrowser
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnCloseOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                Close( );
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
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
        /// Called when [first button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnFirstButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [previous button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnPreviousButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [next button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnNextButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [last button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnLastButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [edit button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnEditButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [refresh button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnRefreshButtonClick( object sender, EventArgs e )
        {
            try
            {
                ClearChatControls( );
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
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
        /// Called when [WebBrowser button click].
        /// </summary>
        /// 
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnWebBrowserButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                OpenWebBrowser( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [delete button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnDeleteButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [save button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSaveButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [menu button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnMenuButtonClick( object sender, RoutedEventArgs e )
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
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
        /// Called when [tool strip text box mouse enter].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnToolStripTextBoxMouseEnter( object sender, RoutedEventArgs e )
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
        /// Called when [listen checked changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnListenCheckedChanged( object sender, RoutedEventArgs e )
        {
            if( ListenCheckBox.IsChecked == true )
            {
                SpeechLabel.Content = "";
                SpeechLabel.Visibility = Visibility.Visible;
                SpeechToText( );
            }
            else
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
            if( MuteCheckBox.IsChecked == true )
            {
                VoiceLabel.Visibility = Visibility.Visible;
                VoiceComboBox.Visibility = Visibility.Visible;
                var _msg = "The GPT Audio Client has been activated!";
                SendNotification( _msg );
            }
            else
            {
                VoiceLabel.Visibility = Visibility.Hidden;
                VoiceComboBox.Visibility = Visibility.Hidden;
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
            if( ChatEditor.Text != "" )
            {
                ChatEditor.Text += "\n";
            }

            var _text = e.Result.Text;
            ChatEditor.Text += _text;
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
                var _question = ChatEditor.Text;
                if( string.IsNullOrEmpty( _question ) )
                {
                    MessageBox.Show( "Type in your question!" );
                    ChatEditor.Focus( );
                    return;
                }

                if( ChatEditor.Text != "" )
                {
                    ChatEditor.AppendText( "\r\n" );
                }

                ChatEditor.AppendText( "User: " + _question + "\r\n" );
                ChatEditor.Text = "";
                try
                {
                    var _answer = SendHttpMessage( _question ) + "";
                    ChatEditor.AppendText( "Bubba GPT: "
                        + _answer.Replace( "\n", "\r\n" ).Trim( ) );

                    SpeechToText( _answer );
                }
                catch( Exception ex )
                {
                    ChatEditor.AppendText( "Error: " + ex.Message );
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
                ClearChatControls( );
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
        private void OnSelectedModelChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( ModelComboBox.SelectedIndex != -1 )
                {
                    _model = ModelComboBox.SelectedValue.ToString( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [language selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSelectedLanguageChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( LanguageListBox.SelectedIndex != -1 )
                {
                    _language = LanguageListBox.SelectedItem.ToString( );
                    SetUserDocumentLanguage( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [task selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSelectedGenerationChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( GenerationComboBox.SelectedIndex != -1 )
                {
                    _generation = GenerationComboBox.SelectedItem.ToString( );
                    SetEndpoints( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [text box input changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnParameterTextBoxChanged( object sender, RoutedEventArgs e )
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
                                _textBox.Text = _value.ToString( "N1" );
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
        /// Called when [selected image size changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSelectedImageSizeChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( ImageSizeComboBox.SelectedIndex != -1 )
                {
                    _imageSize = ImageSizeComboBox.SelectedValue.ToString( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [go button clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnGoButtonClicked( object sender, RoutedEventArgs e )
        {
            try
            {
                SendMessage( ToolStripTextBox.Text );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c>
        /// to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose( bool disposing )
        {
            if( disposing )
            {
                _timer?.Dispose( );
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected static void Fail( Exception ex )
        {
            using var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}