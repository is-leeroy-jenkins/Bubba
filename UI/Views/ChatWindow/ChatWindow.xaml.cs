// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-25-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-25-2025
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
    using Syncfusion.SfSkinManager;
    using Syncfusion.Windows.Edit;
    using ToastNotifications;
    using ToastNotifications.Lifetime;
    using ToastNotifications.Messages;
    using ToastNotifications.Position;
    using Action = System.Action;
    using Exception = System.Exception;
    using Properties;

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
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "UnusedMethodReturnValue.Global" ) ]
    [ SuppressMessage( "ReSharper", "ConvertIfStatementToReturnStatement" ) ]
    public partial class ChatWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The entry
        /// </summary>
        private protected object _entry = new object( );

        /// <summary>
        /// The system prompt
        /// </summary>
        private string _systemPrompt;

        /// <summary>
        /// The interface update
        /// </summary>
        private protected Action _window;

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
        private protected GptRequestTypes _requestType;

        /// <summary>
        /// The role
        /// </summary>
        private protected string _voice;

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
        private protected string _size;

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
        /// increasing the model's likelihood to talk about new topics.
        /// </summary>
        private protected double _presence;

        /// <summary>
        /// The models
        /// </summary>
        private protected IList<string> _models;

        /// <summary>
        /// The image sizes
        /// </summary>
        private protected IList<string> _imageSizes;

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
            Opacity = 0;

            // Window Initialization
            InitializeComponent( );
            RegisterCallbacks( );
            InitializeDelegates( );
            InitializeToolStrip( );

            // GPT Parameters
            _store = false;
            _stream = true;
            TemperatureSlider.Value = 0.18;
            _temperature = TemperatureSlider.Value;
            TopPercentSlider.Value = 0.11;
            _topPercent = TopPercentSlider.Value;
            PresenceSlider.Value = 0.00;
            _presence = PresenceSlider.Value;
            FrequencySlider.Value = 0.00;
            _frequency = FrequencySlider.Value;
            MaxTokenSlider.Value = 2048;
            _maximumTokens = (int)MaxTokenSlider.Value;
            _imageSizes = new List<string>( );

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
                HeaderLabel.Visibility = Visibility.Visible;
                HeaderLabel.Content = "Select Generation Type";
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
        /// Initializes the interface asynchronous.
        /// </summary>
        public void InitializeInterface( )
        {
            try
            {
                App.LoadWebBrowser( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sets the end point.
        /// </summary>
        private void SetGeneration( )
        {
            try
            {
                switch( _requestType )
                {
                    case GptRequestTypes.Assistants:
                    {
                        PopulateCompletionModels( );
                        _endpoint = GptEndPoint.Assistants;
                        HeaderLabel.Content = "GPT Assistant ...";
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.ChatCompletion:
                    {
                        PopulateCompletionModels( );
                        _endpoint = GptEndPoint.Completions;
                        HeaderLabel.Content = "GPT Completions...";
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.TextGeneration:
                    {
                        PopulateTextModels( );
                        _endpoint = GptEndPoint.TextGeneration;
                        HeaderLabel.Content = "Text Generations...";
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.ImageGeneration:
                    {
                        PopulateImageModels( );
                        _endpoint = GptEndPoint.ImageGeneration;
                        HeaderLabel.Content = "Image Generations...";
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.Translations:
                    {
                        PopulateTranslationModels( );
                        PopulateOpenAiVoices( );
                        HeaderLabel.Content = "Translations...";
                        _endpoint = GptEndPoint.Translations;
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.Embeddings:
                    {
                        PopulateEmbeddingModels( );
                        HeaderLabel.Content = "Vector Embeddings...";
                        _endpoint = GptEndPoint.Embeddings;
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.Transcriptions:
                    {
                        PopulateTranscriptionModels( );
                        PopulateOpenAiVoices( );
                        HeaderLabel.Content = "Transcriptions (Speech To Text)...";
                        _endpoint = GptEndPoint.Transcriptions;
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.VectorStores:
                    {
                        PopulateVectorStoreModels( );
                        HeaderLabel.Content = "Vector Stores...";
                        _endpoint = GptEndPoint.VectorStores;
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.SpeechGeneration:
                    {
                        PopulateSpeechModels( );
                        PopulateOpenAiVoices( );
                        HeaderLabel.Content = "Speech Generations (Text To Speech)...";
                        _endpoint = GptEndPoint.SpeechGeneration;
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.FineTuning:
                    {
                        PopulateFineTuningModels( );
                        HeaderLabel.Content = "Fine-Tunings...";
                        _endpoint = GptEndPoint.FineTuning;
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.Files:
                    {
                        PopulateFileApiModels( );
                        HeaderLabel.Content = "Files API...";
                        _endpoint = GptEndPoint.Files;
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.Uploads:
                    {
                        PopulateUploadApiModels( );
                        HeaderLabel.Content = "Uploads API...";
                        _endpoint = GptEndPoint.Uploads;
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    case GptRequestTypes.Projects:
                    {
                        PopulateTextModels( );
                        HeaderLabel.Content = "Projects API...";
                        _endpoint = GptEndPoint.Projects;
                        TabControl.SelectedIndex = 1;
                        break;
                    }
                    default:
                    {
                        PopulateModelsAsync( );
                        HeaderLabel.Content = "GPT Completion...";
                        _endpoint = GptEndPoint.Completions;
                        TabControl.SelectedIndex = 1;
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
        /// Begins the initialize.
        /// </summary>
        private protected void Busy( )
        {
            try
            {
                ProgressBar.Visibility = Visibility.Visible;
                ProgressBar.IsIndeterminate = true;
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
        /// Shows the progress.
        /// </summary>
        private protected void CheckProgress( )
        {
            try
            {
                lock( _entry )
                {
                    if( _busy )
                    {
                        ProgressBar.Visibility = Visibility.Visible;
                        ProgressBar.IsIndeterminate = true;
                    }
                    else
                    {
                        ProgressBar.Visibility = Visibility.Hidden;
                        ProgressBar.IsIndeterminate = false;
                    }
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
        private protected void Chill( )
        {
            try
            {
                ProgressBar.Visibility = Visibility.Hidden;
                ProgressBar.IsIndeterminate = false;
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
                StoreCheckBox.Checked += OnStoreCheckBoxChecked;
                GenerationComboBox.SelectionChanged += OnSelectedRequestTypeChanged;
                GenerationComboBox.SelectionChanged += OnSelectedRequestTypeChanged;
                ImageSizeComboBox.SelectionChanged += OnSelectedImageSizeChanged;
                RefreshButton.Click += OnRefreshButtonClick;
                LookupButton.Click += OnGoButtonClicked;
                GptFileButton.Click += OnFileApiButtonClick;
                ImageSizeComboBox.SelectionChanged += OnSelectedImageSizeChanged;
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
                DeleteButton.Click -= OnDeleteButtonClick;
                ClearButton.Click -= OnClearButtonClick;
                SendButton.Click -= OnSendButtonClick;
                LanguageListBox.SelectionChanged -= OnSelectedLanguageChanged;
                ListenCheckBox.Checked -= OnListenCheckedChanged;
                MuteCheckBox.Checked -= OnMuteCheckedBoxChanged;
                StoreCheckBox.Checked -= OnStoreCheckBoxChecked;
                GenerationComboBox.SelectionChanged -= OnSelectedRequestTypeChanged;
                ImageSizeComboBox.SelectionChanged -= OnSelectedImageSizeChanged;
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
                _stream = true;
                _model = "";
                _size = "";
                _endpoint = "";
                _number = 1;
                _maximumTokens = 2048;
                _temperature = 0.18;
                _topPercent = 0.11;
                _frequency = 0.00;
                _presence = 0.00;
                _language = "";
                _voice = "";
            }
            catch( Exception ex )
            {
                Fail( ex );
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
                StreamCheckBox.IsChecked = true;
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
                PresenceSlider.Value = 0.00;
                FrequencySlider.Value = 0.00;
                TemperatureSlider.Value = 0.18;
                TopPercentSlider.Value = 0.11;
                MaxTokenSlider.Value = 2048;
                NumberSlider.Value = 1;
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
                PopulateLanguageListBox( );
                LanguageListBox.SelectedIndex = -1;
            }
            catch( Exception ex )
            {
                Fail( ex );
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
        private void ClearSelections( )
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
        private string GetApplicationDirectory( string name )
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
                var _splashMessage = new SplashMessage( message )
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

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
                _store = StoreCheckBox.IsChecked ?? false;
                _stream = StreamCheckBox.IsChecked ?? true;
                _presence = double.Parse( PresenceSlider.Value.ToString( "N2" ) );
                _temperature = double.Parse( TemperatureSlider.Value.ToString( "N2" ) );
                _topPercent = double.Parse( TopPercentSlider.Value.ToString( "N2" ) );
                _frequency = double.Parse( FrequencySlider.Value.ToString( "N2" ) );
                _number = int.Parse( NumberTextBox.Text );
                _maximumTokens = Convert.ToInt32( MaxTokenTextBox.Text );
                _userPrompt = _language == "Text"
                    ? ChatEditor.Text
                    : "";
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
                            TabControl.SelectedIndex = 0;
                            ChatEditor.DocumentLanguage = Languages.Text;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "C#":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\CSharp.txt";
                            TabControl.SelectedIndex = 0;
                            ChatEditor.DocumentLanguage = Languages.CSharp;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "Python":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\Python.txt";
                            TabControl.SelectedIndex = 0;
                            ChatEditor.DocumentLanguage = Languages.Text;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "SQL":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\SQL.txt";
                            TabControl.SelectedIndex = 0;
                            ChatEditor.DocumentLanguage = Languages.SQL;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "JavaScript":
                        {
                            var _path = _prefix
                                + @"Resources\Documents\Editor\Stubs\JavaScript.txt";

                            TabControl.SelectedIndex = 0;
                            ChatEditor.DocumentLanguage = Languages.JScript;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "C/C++":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\C.txt";
                            TabControl.SelectedIndex = 0;
                            ChatEditor.DocumentLanguage = Languages.C;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        case "VB/VBA":
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\VBA.txt";
                            TabControl.SelectedIndex = 0;
                            ChatEditor.DocumentLanguage = Languages.VisualBasic;
                            ChatEditor.DocumentSource = _path;
                            break;
                        }
                        default:
                        {
                            var _path = _prefix + @"Resources\Documents\Editor\Stubs\Text.txt";
                            TabControl.SelectedIndex = 0;
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
            _request.Headers.Add( "Authorization", "Bearer " + App.OpenAiKey );
            var _maxTokens = int.Parse( MaxTokenTextBox.Text );// 2048

            // 0.5
            var _temp = double.Parse( TemperatureTextBox.Text );
            if( ( _temp < 0d ) | ( _temp > 1d ) )
            {
                var _msg = "Randomness has to be between 0 and 2"
                    + "with higher values resulting in more random text; Default 1 ";

                SendMessage( _msg );
                return "";
            }

            var _userId = UserLabel.Content;
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
        /// Opens the search dialog.
        /// </summary>
        private protected void OpenSearchDialog( )
        {
            try
            {
                if( App.ActiveWindows.Keys.Contains( "SearchDialog" ) )
                {
                    var _window = ( SearchDialog )App.ActiveWindows[ "SearchDialog" ];
                    _window.Show( );
                }
                else
                {
                    var _searchDialog = new SearchDialog
                    {
                        Topmost = true,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    _searchDialog.Show( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the chat window.
        /// </summary>
        private protected void OpenWebBrowser( )
        {
            try
            {
                if( App.ActiveWindows?.ContainsKey( "WebBrowser" ) == true )
                {
                    var _browser = ( WebBrowser )App.ActiveWindows[ "WebBrowser" ];
                    _browser.Show( );
                }
                else
                {
                    var _webBrowser = new WebBrowser( );
                    App.ActiveWindows?.Add( "WebBrowser", _webBrowser );
                    _webBrowser.Show( );
                }

                Hide( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the file browser.
        /// </summary>
        private protected void OpenFileBrowser( )
        {
            try
            {
                if( App.ActiveWindows.Keys.Contains( "FileBrowser" ) )
                {
                    var _window = ( FileBrowser )App.ActiveWindows[ "FileBrowser" ];
                    _window.Show( );
                }
                else
                {
                    var _fileBrowser = new FileBrowser( )
                    {
                        Topmost = true,
                        Owner = this,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    _fileBrowser.Show( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the file browser.
        /// </summary>
        private protected void OpenGptFileDialog( )
        {
            try
            {
                if( App.ActiveWindows.Keys.Contains( "GptFileDialog" ) )
                {
                    var _window = ( GptFileDialog )App.ActiveWindows[ "GptFileDialog" ];
                    _window.Show( );
                }
                else
                {
                    var _fileDialog = new GptFileDialog( )
                    {
                        Topmost = true,
                        Owner = this,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    _fileDialog.Show( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the folder browser.
        /// </summary>
        private protected void OpenFolderBrowser( )
        {
            try
            {
                if( App.ActiveWindows.Keys.Contains( "FolderBrowser" ) )
                {
                    var _window = ( FolderBrowser )App.ActiveWindows[ "FolderBrowser" ];
                    _window.Show( );
                }
                else
                {
                    var _folderBrowser = new FolderBrowser( )
                    {
                        Topmost = true,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    _folderBrowser.Show( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the image dialog asynchronous.
        /// </summary>
        /// <returns></returns>
        private protected void OpenImageDialog( )
        {
            try
            {
                if( App.ActiveWindows.Keys.Contains( "GptImageDialog" ) )
                {
                    var _window = ( GptImageDialog )App.ActiveWindows[ "GptImageDialog" ];
                    _window.Show( );
                }
                else
                {
                    var _gptImageDialog = new GptImageDialog
                    {
                        Topmost = true,
                        Owner = this,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    _gptImageDialog.Show( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the prompt dialog.
        /// </summary>
        private protected void OpenSystemDialog( )
        {
            try
            {
                if( App.ActiveWindows.Keys.Contains( "SystemDialog" ) )
                {
                    var _window = ( SystemDialog )App.ActiveWindows[ "SystemDialog" ];
                    _window.Show( );
                }
                else
                {
                    var _systemDialog = new SystemDialog
                    {
                        Topmost = true,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    _systemDialog.Show( );
                }
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
                Busy( );
                var _url = "https://api.openai.com/v1/models";
                _httpClient = new HttpClient( );
                _httpClient.Timeout = new TimeSpan( 0, 0, 3 );
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue( "Bearer", App.OpenAiKey );

                var _async = await _httpClient.GetAsync( _url );
                _async.EnsureSuccessStatusCode( );
                var _content = await _async.Content.ReadAsStringAsync( );
                var _models = new List<string>( );
                using var _document = JsonDocument.Parse( _content );
                var _root = _document.RootElement;
                if( _root.TryGetProperty( "data", out var _data )
                    && _data.ValueKind == JsonValueKind.Array )
                {
                    foreach( var _item in _data.EnumerateArray( ) )
                    {
                        if( _item.TryGetProperty( "id", out var _element ) )
                        {
                            _models.Add( _element.GetString( ) );
                        }
                    }
                }

                _models.Sort( );
                ModelComboBox.Items.Clear( );
                Dispatcher.BeginInvoke( ( ) =>
                {
                    foreach( var _lm in _models )
                    {
                        if( !_lm.StartsWith( "ft" ) )
                        {
                            ModelComboBox.Items.Add( _lm );
                        }
                    }
                } );

                Chill( );
                ModelComboBox.SelectedIndex = -1;
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
        /// Populates the image sizes.
        /// </summary>
        private void PopulateImageSizes( )
        {
            try
            {
                ImageSizeComboBox.Items?.Clear( );
                if( string.IsNullOrEmpty( _model ) )
                {
                    ImageSizeComboBox.Items.Add( "256 X 256" );
                    ImageSizeComboBox.Items.Add( "512 X 512" );
                    ImageSizeComboBox.Items.Add( "1024 X 1024" );
                    ImageSizeComboBox.Items.Add( "1792 X 1024" );
                    ImageSizeComboBox.Items.Add( "1024 X 1792" );
                    ImageSizeComboBox.SelectedIndex = -1;
                }
                else if( !string.IsNullOrEmpty( _model ) )
                {
                    switch( _model )
                    {
                        case "dall-e-2":
                        {
                            ImageSizeComboBox.Items.Add( "256 X 256" );
                            ImageSizeComboBox.Items.Add( "512 X 512" );
                            ImageSizeComboBox.Items.Add( "1024 X 1024" );
                            ImageSizeComboBox.SelectedIndex = -1;
                            break;
                        }
                        case "dall-e-3":
                        {
                            ImageSizeComboBox.Items.Add( "1024 X 1024" );
                            ImageSizeComboBox.Items.Add( "1792 X 1024" );
                            ImageSizeComboBox.Items.Add( "1024 X 1792" );
                            ImageSizeComboBox.SelectedIndex = -1;
                            break;
                        }
                        default:
                        {
                            ImageSizeComboBox.Items.Add( "256 X 256" );
                            ImageSizeComboBox.Items.Add( "512 X 512" );
                            ImageSizeComboBox.Items.Add( "1024 X 1024" );
                            ImageSizeComboBox.Items.Add( "1792 X 1024" );
                            ImageSizeComboBox.Items.Add( "1024 X 1792" );
                            ImageSizeComboBox.SelectedIndex = -1;
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
        /// Populates the text generation models.
        /// </summary>
        private void PopulateTextModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "gpt-4-0613" );
                ModelComboBox.Items.Add( "gpt-4-0314" );
                ModelComboBox.Items.Add( "gpt-4-turbo-2024-04-09" );
                ModelComboBox.Items.Add( "gpt-4o-2024-08-06" );
                ModelComboBox.Items.Add( "gpt-4o-2024-11-20" );
                ModelComboBox.Items.Add( "gpt-4o-2024-05-13" );
                ModelComboBox.Items.Add( "gpt-4o-mini-2024-07-18" );
                ModelComboBox.Items.Add( "o1-2024-12-17" );
                ModelComboBox.Items.Add( "o1-mini-2024-09-12" );
                ModelComboBox.Items.Add( "text-davinci-003" );
                ModelComboBox.Items.Add( "text-curie-001" );
                ModelComboBox.SelectedIndex = -1;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the completion models.
        /// </summary>
        private void PopulateCompletionModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "gpt-4-0613" );
                ModelComboBox.Items.Add( "gpt-4-0314" );
                ModelComboBox.Items.Add( "gpt-4-turbo-2024-04-09" );
                ModelComboBox.Items.Add( "gpt-4o-2024-08-06" );
                ModelComboBox.Items.Add( "gpt-4o-2024-11-20" );
                ModelComboBox.Items.Add( "gpt-4o-2024-05-13" );
                ModelComboBox.Items.Add( "gpt-4o-mini-2024-07-18" );
                ModelComboBox.Items.Add( "o1-2024-12-17" );
                ModelComboBox.Items.Add( "o1-mini-2024-09-12" );
                ModelComboBox.SelectedIndex = -1;
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
                ModelComboBox.Items.Add( "dall-e-2" );
                ModelComboBox.Items.Add( "dall-e-3" );
                ModelComboBox.Items.Add( "gpt-4-0613" );
                ModelComboBox.Items.Add( "gpt-4-0314" );
                ModelComboBox.Items.Add( "gpt-4o-mini-2024-07-18" );
                ModelComboBox.SelectedIndex = -1;
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
                ModelComboBox.Items.Add( "gpt-4-0613" );
                ModelComboBox.Items.Add( "gpt-4-0314" );
                ModelComboBox.Items.Add( "gpt-4-turbo-2024-04-09" );
                ModelComboBox.Items.Add( "text-davinci-003" );
                ModelComboBox.SelectedIndex = -1;
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
                ModelComboBox.SelectedIndex = -1;
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
                ModelComboBox.SelectedIndex = -1;
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
                ModelComboBox.Items.Add( "gpt-4-0613" );
                ModelComboBox.Items.Add( "gpt-4-0314" );
                ModelComboBox.Items.Add( "gpt-4-turbo-2024-04-09" );
                ModelComboBox.Items.Add( "gpt-4o-mini-2024-07-18" );
                ModelComboBox.Items.Add( "gpt-4o-2024-08-06" );
                ModelComboBox.Items.Add( "gpt-4o-2024-11-20" );
                ModelComboBox.Items.Add( "gpt-4o-2024-05-13" );
                ModelComboBox.SelectedIndex = -1;
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
                ModelComboBox.Items.Add( "gpt-4o-audio-preview-2024-12-17" );
                ModelComboBox.Items.Add( "gpt-4o-audio-preview-2024-10-01" );
                ModelComboBox.Items.Add( "gpt-4o-mini-audio-preview-2024-12-17" );
                ModelComboBox.SelectedIndex = -1;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the upload API models.
        /// </summary>
        private void PopulateUploadApiModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "gpt-4-0613" );
                ModelComboBox.Items.Add( "gpt-4-0314" );
                ModelComboBox.Items.Add( "gpt-4-turbo-2024-04-09" );
                ModelComboBox.Items.Add( "gpt-4o-mini-2024-07-18" );
                ModelComboBox.Items.Add( "gpt-4o-2024-08-06" );
                ModelComboBox.Items.Add( "gpt-4o-2024-11-20" );
                ModelComboBox.Items.Add( "gpt-4o-2024-05-13" );
                ModelComboBox.Items.Add( "o1-2024-12-17" );
                ModelComboBox.Items.Add( "o1-mini-2024-09-12" );
                ModelComboBox.SelectedIndex = -1;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the vector store models.
        /// </summary>
        private void PopulateVectorStoreModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "gpt-4-0613" );
                ModelComboBox.Items.Add( "gpt-4-0314" );
                ModelComboBox.Items.Add( "gpt-4-turbo-2024-04-09" );
                ModelComboBox.Items.Add( "gpt-4o-mini-2024-07-18" );
                ModelComboBox.Items.Add( "gpt-4o-2024-08-06" );
                ModelComboBox.Items.Add( "gpt-4o-2024-11-20" );
                ModelComboBox.Items.Add( "gpt-4o-2024-05-13" );
                ModelComboBox.Items.Add( "o1-2024-12-17" );
                ModelComboBox.Items.Add( "o1-mini-2024-09-12" );
                ModelComboBox.SelectedIndex = -1;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the file API models.
        /// </summary>
        private void PopulateFileApiModels( )
        {
            try
            {
                ModelComboBox.Items?.Clear( );
                ModelComboBox.Items.Add( "gpt-4-0613" );
                ModelComboBox.Items.Add( "gpt-4-0314" );
                ModelComboBox.Items.Add( "gpt-4-turbo-2024-04-09" );
                ModelComboBox.Items.Add( "gpt-4o-mini-2024-07-18" );
                ModelComboBox.Items.Add( "gpt-4o-2024-08-06" );
                ModelComboBox.Items.Add( "gpt-4o-2024-11-20" );
                ModelComboBox.Items.Add( "gpt-4o-2024-05-13" );
                ModelComboBox.Items.Add( "o1-2024-12-17" );
                ModelComboBox.Items.Add( "o1-mini-2024-09-12" );
                ModelComboBox.SelectedIndex = -1;
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
        private void PopulateInstalledVoices( )
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
        /// Populates the open ai voices.
        /// </summary>
        private void PopulateOpenAiVoices( )
        {
            try
            {
                VoiceComboBox.Items?.Clear( );
                VoiceComboBox.Items.Add( "alloy" );
                VoiceComboBox.Items.Add( "ash" );
                VoiceComboBox.Items.Add( "coral" );
                VoiceComboBox.Items.Add( "echo" );
                VoiceComboBox.Items.Add( "onyx" );
                VoiceComboBox.Items.Add( "fable" );
                VoiceComboBox.Items.Add( "nova" );
                VoiceComboBox.Items.Add( "sage" );
                VoiceComboBox.Items.Add( "shimmer" );
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
                Opacity = 0;
                InitializeTimer( );
                InitializeToolStrip( );
                InitializeLabel( );
                PopulateModelsAsync( );
                PopulateLanguageListBox( );
                PopulateInstalledVoices( );
                PopulateImageSizes( );
                ClearChatControls( );
                InitializeChatEditor( );
                _systemPrompt = OpenAI.BubbaPrompt;
                StreamCheckBox.Checked += OnStreamCheckBoxChecked;
                ModelComboBox.SelectionChanged += OnSelectedModelChanged;
                UserLabel.Content = $@"User ID : {Environment.UserName}";
                TabControl.SelectedIndex = 1;
                App.ActiveWindows.Add( "ChatWindow", this );
                InitializeInterface( );
                Opacity = 1;
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
        /// Called when [tab changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnTabChanged( object sender, RoutedEventArgs e )
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
        private void OnStoreCheckBoxChecked( object sender, EventArgs e )
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
        private void OnStreamCheckBoxChecked( object sender, EventArgs e )
        {
            try
            {
                _stream = StreamCheckBox.IsChecked == true;
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
        private protected void OnToolStripTextBoxTextChanged( object sender,
            TextChangedEventArgs e )
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
                _statusUpdate += null;
                _window += null;
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
                Busy( );
                OpenFileBrowser( );
                Chill( );
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
                Busy( );
                var _fileBrowser = new FileBrowser
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Owner = this,
                    Topmost = true
                };

                Chill( );
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
        /// Called when [file upload button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnFileApiButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                Busy( );
                OpenGptFileDialog( );
                Chill( );
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
        /// Called when [system prompt button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnPromptOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                Busy( );
                OpenSystemDialog( );
                Chill( );
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
                ClearParameters( );
                PopulateModelsAsync( );
                PopulateInstalledVoices( );
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
                Busy( );
                OpenWebBrowser( );
                Chill( );
                Hide( );
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
                HeaderLabel.Content = "";
                HeaderLabel.Visibility = Visibility.Visible;
                SpeechToText( );
            }
            else
            {
                _engine.RecognizeAsyncStop( );
                HeaderLabel.Visibility = Visibility.Hidden;
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
            HeaderLabel.Content = "";
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
            HeaderLabel.Content = _text;
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
                ClearParameters( );
                PopulateModelsAsync( );
                PopulateInstalledVoices( );
                PopulateImageSizes( );
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
                    PopulateImageSizes( );
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
        private void OnSelectedRequestTypeChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( GenerationComboBox.SelectedIndex != -1 )
                {
                    var _item = GenerationComboBox.SelectedItem;
                    var _request = ( ( ComboBoxItem )_item ).Tag.ToString( );
                    _requestType =
                        ( GptRequestTypes )Enum.Parse( typeof( GptRequestTypes ), _request );

                    SetGeneration( );
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
                            case "FrequencyPenalty":
                            case "PresencePenalty":
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
                    var _image = ImageSizeComboBox.SelectedValue.ToString( );
                    _size = _image?.Replace( " ", "" );
                    var _message = "Image Size Select - " + _size;
                    SendNotification( _message );
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
        private protected void Fail( Exception ex )
        {
            using var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}