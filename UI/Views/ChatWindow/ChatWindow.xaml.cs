// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 03-17-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        03-17-2025
// ******************************************************************************************
// <copyright file="ChatWindow.xaml.cs" company="Terry D. Eppler">
//     Badger is a budget execution & data analysis tool for EPA analysts
//     based on WPF, Net 6, and written in C Sharp.
// 
//     Copyright �  2022 Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy

//    of this software and associated documentation files (the �Software�),
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
//    THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Speech.Recognition;
    using System.Speech.Synthesis;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Input;
    using CefSharp;
    using CefSharp.Wpf;
    using MahApps.Metro.Controls;
    using Properties;
    using Syncfusion.SfSkinManager;
    using ToastNotifications;
    using ToastNotifications.Lifetime;
    using ToastNotifications.Messages;
    using ToastNotifications.Position;
    using Application = System.Windows.Forms.Application;
    using MessageBox = System.Windows.MessageBox;
    using MouseEventArgs = System.Windows.Input.MouseEventArgs;
    using Timer = System.Threading.Timer;

    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "LocalVariableHidesMember" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "UseNullPropagation" ) ]
    [ SuppressMessage( "ReSharper", "SuggestBaseTypeForParameter" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    public partial class ChatWindow : Window, INotifyPropertyChanged, IDisposable
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
        /// The current clean URL
        /// </summary>
        private protected string _finalUrl;

        /// <summary>
        /// The last search
        /// </summary>
        private string _lastSearch = "";

        /// <summary>
        /// The old window state
        /// </summary>
        private protected WindowState _oldWindowState;

        /// <summary>
        /// The current full URL
        /// </summary>
        private protected string _originalUrl;

        /// <summary>
        /// The search open
        /// </summary>
        private protected bool _isSearchOpen;

        /// <summary>
        /// The system prompt fro the GPT
        /// </summary>
        private protected string _instructions;

        /// <summary>
        /// The user prompt for the GPT
        /// </summary>
        private protected string _userPrompt;

        /// <summary>
        /// The tab pages
        /// </summary>
        private protected BrowserTabCollection _tabPages;

        /// <summary>
        /// The download strip
        /// </summary>
        private protected BrowserTabItem _downloadStrip;

        /// <summary>
        /// The add tab item
        /// </summary>
        private protected BrowserTabItem _addTabItem;

        /// <summary>
        /// The new tab strip
        /// </summary>
        private protected BrowserTabItem _newTabItem;

        /// <summary>
        /// The new tab strip
        /// </summary>
        private protected BrowserTabItem _currentTab;

        /// <summary>
        /// The assembly
        /// </summary>
        public static Assembly Assembly;

        /// <summary>
        /// The messages
        /// </summary>
        private protected ChatLog _messages;

        /// <summary>
        /// The key words for the custome search engine
        /// </summary>
        private protected string _keyWords;

        /// <summary>
        /// The role
        /// </summary>
        private protected string _role;

        /// <summary>
        /// The role
        /// </summary>
        private protected API _requestType;

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
        /// The selected document
        /// </summary>
        private protected string _document;

        /// <summary>
        /// The selected request
        /// </summary>
        private protected string _request;

        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// The HTTP client
        /// </summary>
        private protected HttpClient _httpClient;

        /// <summary>
        /// The options
        /// </summary>
        private protected GptOptions _options;

        /// <summary>
        /// The region
        /// </summary>
        private Region _region;

        /// <summary>
        /// A number between 0 and 2.
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// </summary>
        private protected double _temperature;

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
        /// The listen
        /// </summary>
        private protected bool _listen;

        /// <summary>
        /// The mute
        /// </summary>
        private protected bool _mute;

        /// <summary>
        /// Whether to return log probabilities of the output tokens or not.
        /// If true, returns the log probabilities of each output token
        /// returned in the content of message.
        /// </summary>
        private protected bool _logprobs;

        /// <summary>
        /// The speed
        /// </summary>
        private protected int _speed;

        /// <summary>
        /// The response format
        /// </summary>
        private protected string _responseFormat;

        /// <summary>
        /// The reasoning effort
        /// </summary>
        private protected string _reasoningEffort;

        /// <summary>
        /// The audio format
        /// </summary>
        private protected string _audioFormat;

        /// <summary>
        /// The image size
        /// </summary>
        private protected string _imageSize;

        /// <summary>
        /// The image format
        /// </summary>
        private protected string _imageFormat;

        /// <summary>
        /// The quality
        /// </summary>
        private protected string _imageQuality;

        /// <summary>
        /// The detail
        /// </summary>
        private protected string _imageDetail;

        /// <summary>
        /// The background
        /// </summary>
        private protected string _imageBackground;

        /// <summary>
        /// The image style
        /// </summary>
        private protected string _imageStyle;

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
        private protected double _frequencyPenalty;

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// increasing the model's likelihood to talk about new topics.
        /// </summary>
        private protected double _presencePenalty;

        /// <summary>
        /// An integer between 0 and 20 specifying the number of most likely tokens
        /// to return at each token position, each with an associated log probability.
        /// logprobs must be set to true if this parameter is used.
        /// </summary>
        private protected double _topLogProbs;

        /// <summary>
        /// The models
        /// </summary>
        private protected IList<string> _modelOptions;

        /// <summary>
        /// The image sizes
        /// </summary>
        private protected IList<string> _sizeOptions;

        /// <summary>
        /// The image sizes
        /// </summary>
        private protected IDictionary<string, string> _voiceOptions;

        /// <summary>
        /// The speed options
        /// </summary>
        private protected IList<string> _speedOptions;

        /// <summary>
        /// The reasoning options
        /// </summary>
        private protected IList<string> _reasoningOptions;

        /// <summary>
        /// The quality options
        /// </summary>
        private protected IList<string> _qualityOptions;

        /// <summary>
        /// The detail options
        /// </summary>
        private protected IList<string> _detailOptions;

        /// <summary>
        /// The response format options
        /// </summary>
        private protected IList<string> _responseFormatOptions;

        /// <summary>
        /// The audio format options
        /// </summary>
        private protected IList<string> _audioFormatOptions;

        /// <summary>
        /// The chat model
        /// </summary>
        private protected string _model;

        /// <summary>
        /// The browser tabs
        /// </summary>
        private ObservableCollection<BrowserTabViewModel> _browserTabs;

        /// <summary>
        /// The context menu handler
        /// </summary>
        private protected ContextMenuCallback _contextMenuCallback;

        /// <summary>
        /// The current title
        /// </summary>
        private string _currentTitle;

        /// <summary>
        /// The search engine identifier
        /// </summary>
        private protected string _searchEngineId;

        /// <summary>
        /// The search engine project identifier
        /// </summary>
        private protected string _searchEngineProjectId;

        /// <summary>
        /// The search engine name
        /// </summary>
        private protected string _searchEngineName;

        /// <summary>
        /// The search engine project number
        /// </summary>
        private protected string _searchEngineProjectNumber;

        /// <summary>
        /// The search engine key
        /// </summary>
        private protected string _searchEngineKey;

        /// <summary>
        /// The search engine URL
        /// </summary>
        private protected string _searchEngineUrl;

        /// <summary>
        /// The speech recognition engine
        /// </summary>
        private protected SpeechRecognitionEngine _engine;

        /// <summary>
        /// The speech synthesizer
        /// </summary>
        private protected SpeechSynthesizer _synthesizer;

        /// <summary>
        /// The download names
        /// </summary>
        private protected Dictionary<int, string> _downloadNames;

        /// <summary>
        /// The downloads
        /// </summary>
        private protected Dictionary<int, DownloadItem> _downloadItems;

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
        /// The download cancel requests
        /// </summary>
        private protected List<int> _cancelRequests;

        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// The life span handler
        /// </summary>
        private protected LifeSpanCallback _lifeSpanCallback;

        /// <summary>
        /// 
        /// The keyboard handler
        /// </summary>
        private protected KeyboardCallback _keyboardCallback;

        /// <summary>
        /// The download handler
        /// </summary>
        private protected DownloadCallback _downloadCallback;

        /// <summary>
        /// The request handler
        /// </summary>
        private protected RequestCallback _requestCallback;

        /// <summary>
        /// The host
        /// </summary>
        private protected HostCallback _hostCallback;

        /// <summary>
        /// The timer callback
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <summary>
        /// The browser action
        /// </summary>
        private protected Func<ChromiumWebBrowser> _browserCallback;

        /// <summary>
        /// The status update
        /// </summary>
        private protected Action _statusUpdate;

        /// <summary>
        /// The x
        /// </summary>
        private protected double[ ] _x =
        {
            1,
            2,
            3,
            4,
            5
        };

        /// <summary>
        /// The y
        /// </summary>
        private protected double[ ] _y =
        {
            1,
            4,
            9,
            16,
            25
        };

        /// <inheritdoc />
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ChatWindow"/> class.
        /// </summary>
        public ChatWindow( )
        {
            // Theme Properties
            SfSkinManager.ApplyStylesOnApplication = true;

            // Window Initialization
            InitializeComponent( );
            RegisterCallbacks( );
            InitializeDelegates( );
            InitializeToolStrip( );

            // Control Properties
            TemperatureSlider.Value = 0.8;
            TopPercentSlider.Value = 0.9;
            PresenceSlider.Value = 0.00;
            FrequencySlider.Value = 0.00;
            SpeechRateSlider.Value = 1.0;

            // GPT Parameters
            _store = true;
            _stream = true;
            _temperature = TemperatureSlider.Value;
            _topPercent = TopPercentSlider.Value;
            _presencePenalty = PresenceSlider.Value;
            _frequencyPenalty = FrequencySlider.Value;
            _maximumTokens = int.Parse( MaxTokenTextBox.Value.ToString(  ) );

            // GPT Parameter Options
            _sizeOptions = new List<string>( );
            _speedOptions = new List<string>( );
            _qualityOptions = new List<string>( );
            _reasoningOptions = new List<string>( );
            _responseFormatOptions = new List<string>( );
            _audioFormatOptions = new List<string>( );
            _voiceOptions = new Dictionary<string, string>( );
            _browserTabs = new ObservableCollection<BrowserTabViewModel>( );
            _instructions = Prompts.BudgetAnalyst;
            _searchEngineUrl = Locations.SearchUrl;
            _originalUrl = Locations.Google;

            // Event Wiring
            Loaded += OnLoad;
            Closing += OnClosing;
        }

        /// <summary>
        /// Gets the duration of the current tab loading.
        /// </summary>
        /// <value>
        /// The duration of the current tab loading.
        /// </value>
        public int LoadingDuration
        {
            get
            {
                if( TabControl.SelectedItem != null
                    && TabControl.SelectedItem != null )
                {
                    var _loadTime =
                        ( int )( DateTime.Now - _currentTab.DateCreated ).TotalMilliseconds;

                    return _loadTime;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets the search engine URL.
        /// </summary>
        /// <value>
        /// The search engine URL.
        /// </value>
        public string SearchEngineUrl
        {
            get
            {
                return _searchEngineUrl;
            }
            set
            {
                if( _searchEngineUrl != value )
                {
                    _searchEngineUrl = value;
                    OnPropertyChanged( nameof( SearchEngineUrl ) );
                }
            }
        }

        /// <summary>
        /// Gets the last index.
        /// </summary>
        /// <value>
        /// The last index.
        /// </value>
        private int LastIndex
        {
            get
            {
                return TabControl.Items.Count - 2;
            }
        }

        /// <summary>
        /// Gets or sets the browser tabs.
        /// </summary>
        /// <value>
        /// The browser tabs.
        /// </value>
        public ObservableCollection<BrowserTabViewModel> BrowserTabs
        {
            get
            {
                return _browserTabs;
            }
            set
            {
                if( _browserTabs != value )
                {
                    _browserTabs = value;
                    OnPropertyChanged( nameof( BrowserTabs ) );
                }
            }
        }

        /// <summary>
        /// The host
        /// </summary>
        public HostCallback HostCallback
        {
            get
            {
                return _hostCallback;
            }
            set
            {
                _hostCallback = value;
            }
        }

        /// <summary>
        /// The download names
        /// </summary>
        public Dictionary<int, string> DownloadNames
        {
            get
            {
                return _downloadNames;
            }
            set
            {
                _downloadNames = value;
            }
        }

        /// <summary>
        /// Gets or sets the add tab item.
        /// </summary>
        /// <value>
        /// The add tab item.
        /// </value>
        public BrowserTabItem AddTabItem
        {
            get
            {
                return _addTabItem;
            }
            set
            {
                _addTabItem = value;
            }
        }

        /// <summary>
        /// Creates new tabitem.
        /// </summary>
        /// <value>
        /// The new tab item.
        /// </value>
        public BrowserTabItem NewTabItem
        {
            get
            {
                return _newTabItem;
            }
            set
            {
                _newTabItem = value;
            }
        }

        /// <summary>
        /// Gets the cancel requests.
        /// </summary>
        /// <value>
        /// The cancel requests.
        /// </value>
        public List<int> CancelRequests
        {
            get
            {
                return _cancelRequests;
            }
            set
            {
                _cancelRequests = value;
            }
        }

        /// <summary>
        /// Initializes the plotter.
        /// </summary>
        private protected void InitializePlotter( )
        {
            try
            {
                Plotter.Plot.Clear( );
                var  _x = new[ ]
                {
                    1,
                    2,
                    3,
                    4,
                    5
                };

                var _y = new[ ]
                {
                    1,
                    4,
                    9,
                    16,
                    25
                };

                Plotter.Plot.Add.Scatter( _x, _y );
                Plotter.Refresh( );
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
                SetToolbarVisibility( true );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the user system text box.
        /// </summary>
        private void InitializeEditor( )
        {
            try
            {
                Editor.EnableOutlining = true;
                Editor.EnableIntellisense = true;
                Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.Text;
                Editor.IsMultiLine = true;
                Editor.IsUndoEnabled = true;
                Editor.IsRedoEnabled = true;
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
                LoadFileBrowser( );
                LoadSearchDialog( );
                LoadGptFileDialog( );
                LoadCalculator( );
                LoadSystemDialog( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Speeches to text.
        /// </summary>
        private void InitializeSpeechEngine( )
        {
            try
            {
                if( _engine != null )
                {
                    _engine.RecognizeAsync( RecognizeMode.Multiple );
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
        /// we must store download metadata in a list,
        /// since CefSharp does not
        /// </summary>
        private void InitializeDownloads( )
        {
            _downloadItems = new Dictionary<int, DownloadItem>( );
            _downloadNames = new Dictionary<int, string>( );
            _cancelRequests = new List<int>( );
        }

        /// <summary>
        /// these hot keys work when the user
        /// is focused on the .NET form and its controls,
        /// AND when the user is focused on the browser
        /// (CefSharp portion)
        /// </summary>
        private void InitializeHotkeys( )
        {
            try
            {
                // browser hot keys
                KeyboardCallback.AddHotKey( this, CloseActiveTab, Keys.W, true );
                KeyboardCallback.AddHotKey( this, CloseActiveTab, Keys.Escape, true );
                KeyboardCallback.AddHotKey( this, RefreshActiveTab, Keys.F5 );
                KeyboardCallback.AddHotKey( this, OpenDeveloperTools, Keys.F12 );

                // search hot keys
                KeyboardCallback.AddHotKey( this, OpenSearch, Keys.F, true );
                KeyboardCallback.AddHotKey( this, CloseSearch, Keys.Escape );
                KeyboardCallback.AddHotKey( this, StopActiveTab, Keys.Escape );
                KeyboardCallback.AddHotKey( this, ToggleFullscreen, Keys.F11 );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sets the hyper parameters.
        /// </summary>
        private protected void InitializeParameters( )
        {
            try
            {
                _store = StoreCheckBox.IsChecked ?? true;
                _stream = StreamCheckBox.IsChecked ?? true;
                _listen = ListenCheckBox.IsChecked ?? false;
                _mute = MuteCheckBox.IsChecked ?? true;
                _presencePenalty = double.Parse( PresenceSlider.Value.ToString( "N2" ) );
                _temperature = double.Parse( TemperatureSlider.Value.ToString( "N2" ) );
                _topPercent = double.Parse( TopPercentSlider.Value.ToString( "N2" ) );
                _frequencyPenalty = double.Parse( FrequencySlider.Value.ToString( "N2" ) );
                _number = int.Parse( MaxTokenTextBox.Text );
                _maximumTokens = Convert.ToInt32( MaxTokenTextBox.Text );
                _model = ModelDropDown.SelectedItem.ToString( ) ?? "gpt-4o-mini";
                _speed = int.Parse( SpeechRateSlider.Value.ToString( "N0"  ) );
                _userPrompt = _language == "Text"
                    ? Editor.Text
                    : "";
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the command bindings.
        /// </summary>
        private void InitializeCommands( )
        {
            try
            {
                CommandBindings.Add( new CommandBinding( ApplicationCommands.New, OpenNewTab ) );
                CommandBindings.Add( new CommandBinding( ApplicationCommands.Close, CloseTab ) );
                CommandBindings.Add( new CommandBinding( CefSharpCommands.Exit, CloseTab ) );
                CommandBindings.Add( new CommandBinding( CefSharpCommands.OpenTabCommand, OpenTabCommandBinding ) );
                CommandBindings.Add( new CommandBinding( CefSharpCommands.PrintTabToPdfCommand, PrintToPdfCommandBinding ) );
                CommandBindings.Add( new CommandBinding( CefSharpCommands.CustomCommand, CustomCommandBinding ) );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// this is done just once,
        /// to globally initialize CefSharp/CEF
        /// </summary>
        private void InitializeBrowser( )
        {
            Browser.LoadingStateChanged += OnWebBrowserLoadingStateChanged;
            Browser.AddressChanged += OnWebBrowserAddressChanged;
            Browser.TitleChanged += OnWebBrowserTitleChanged;
            _searchEngineUrl = Locations.SearchUrl;
            _hostCallback = new HostCallback( this );
            _downloadCallback = new DownloadCallback( this );
            _lifeSpanCallback = new LifeSpanCallback( this );
            _contextMenuCallback = new ContextMenuCallback( this );
            _keyboardCallback = new KeyboardCallback( this );
            _requestCallback = new RequestCallback( this );
            InitializeDownloads( );
            ConfigureBrowser( Browser );
        }

        /// <summary>
        /// Registers the callbacks.
        /// </summary>
        private protected void RegisterCallbacks( )
        {
            try
            {
                GenerationListBox.SelectionChanged += OnRequestDropDownSelectionChanged;
                ModelDropDown.SelectionChanged += OnModelDropDownSelectionChanged;
                ToolStripTextBox.TextChanged += OnToolStripTextBoxTextChanged;
                ToolStripMenuButton.Click += OnToggleButtonClick;
                ToolStripRefreshButton.Click += OnToolStripRefreshButtonClick;
                ToolStripSendButton.Click += OnGoButtonClicked;
                ToolStripFileButton.Click += OnFileApiButtonClick;
                UrlCancelButton.Click += OnUrlCancelButtonClick;
                UrlHomeButton.Click += OnUrlHomeButtonClick;
                UrlRefreshButton.Click += OnUrlRefreshButtonClick;
                UrlBackButton.Click += OnUrlBackButtonClick;  
                UrlForwardButton.Click += OnUrlForwardButtonClick;
                UrlPanelTextBox.GotMouseCapture += OnUrlTextBoxMouseEnter;
                ListenCheckBox.Checked += OnListenCheckedChanged;
                MuteCheckBox.Checked += OnMuteCheckedBoxChanged;
                StoreCheckBox.Checked += OnStoreCheckBoxChecked;
                StreamCheckBox.Checked += OnStreamCheckBoxChecked;
                LanguageDropDown.SelectionChanged += OnLanguageDropDownSelectionChanged;
                DocumentListBox.SelectionChanged += OnDocumentListBoxSelectionChanged;
                ResponseFormatDropDown.SelectionChanged += OnResponseFormatSelectionChanged;
                ImageSizeDropDown.SelectionChanged += OnImageSizeSelectionChanged;
                ImageFormatDropDown.SelectionChanged += OnImageFormatSelectionChanged;
                ImageQualityDropDown.SelectionChanged += OnImageQualitySelectionChanged;
                ImageDetailDropDown.SelectionChanged += OnImageDetailSelectionChanged;
                ImageStyleDropDown.SelectionChanged += OnImageStyleSelectionChanged;
                ImageBackgroundDropDown.SelectionChanged += OnImageBackgroundSelectionChanged;
                EffortDropDown.SelectionChanged += OnEffortSelectionChanged;
                AudioFormatDropDown.SelectionChanged += OnAudioFormatSelectionChanged;
                VoicesDropDown.SelectionChanged += OnVoiceSelectionChanged;
                PromptDropDown.SelectionChanged += OnPromptDropDownSelectionChanged;
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
        /// Determines whether the specified URL is blank.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// <c>true</c>
        /// if the specified URL is blank;
        /// otherwise,
        /// <c>false</c>.
        /// </returns>
        private bool IsBlank( string url )
        {
            try
            {
                ThrowIf.Null( url, nameof( url ) );
                return url == "" || url == "about:blank";
            }
            catch( Exception ex )
            {
                Fail( ex );
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is blank or system] [the specified URL].
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// <c>true</c> if [is blank or system] [the specified URL];
        /// otherwise,
        /// <c>false</c>.
        /// </returns>
        private bool IsBlankOrSystem( string url )
        {
            try
            {
                ThrowIf.Null( url, nameof( url ) );
                return url == "" || url.BeginsWith( "about:" ) || url.BeginsWith( "chrome:" )
                    || url.BeginsWith( Locations.Internal + ":" );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is on first tab].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is on first tab]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsFirstTab( )
        {
            return TabControl.SelectedItem == TabControl.Items[ 0 ];
        }

        /// <summary>
        /// Determines whether [is on last tab].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is on last tab]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsLastTab( )
        {
            return TabControl.SelectedItem == TabControl.Items[ ^2 ];
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
                        ProgressBar.IsIndeterminate = true;
                        ProgressBar.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ProgressBar.IsIndeterminate = false;
                        ProgressBar.Visibility = Visibility.Hidden;
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
                _store = true;
                _stream = true;
                _model = "";
                _imageSize = "";
                _endpoint = "";
                _number = 1;
                _maximumTokens = 10000;
                _temperature = 0.8;
                _topPercent = 0.9;
                _frequencyPenalty = 0.00;
                _presencePenalty = 0.00;
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
                PresenceSlider.Value = 0.00;
                FrequencySlider.Value = 0.00;
                TemperatureSlider.Value = 0.8;
                TopPercentSlider.Value = 0.9;
                SpeechRateSlider.Value = 1;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Clears the labels.
        /// </summary>
        private void ClearLabels( )
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
        /// Clears the combo boxes.
        /// </summary>
        private void ClearComboBoxes( )
        {
            try
            {
                GenerationListBox.SelectedIndex = -1;
                ModelDropDown.SelectedIndex = -1;
                VoicesDropDown.SelectedIndex = -1;
                ImageSizeDropDown.SelectedIndex = -1;
                LanguageDropDown.SelectedIndex = -1;
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
                PopulateDocumentListBox( );
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
                Editor.Text = "";
                ChatTextBox.Text = "";
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
        /// this is done every time a new tab is opened
        /// </summary>
        /// <param name="browser">The browser.</param>
        private void ConfigureBrowser( ChromiumWebBrowser browser )
        {
            try
            {
                ThrowIf.Null( browser, nameof( browser ) );
                var _config = new BrowserSettings( );
                var _flag = Locations.WebGL;
                _config.WebGl = bool.Parse( _flag ).ToCefState( );
                browser.BrowserSettings = _config;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Closes the tab.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void CloseTab( object sender, ExecutedRoutedEventArgs e )
        {
            if( BrowserTabs.Count > 0 )
            {
                //Obtain the original source element for this event
                var originalSource = ( FrameworkElement )e.OriginalSource;
                BrowserTabViewModel browserViewModel;
                if( originalSource is ChatWindow )
                {
                    browserViewModel = BrowserTabs[ TabControl.SelectedIndex ];
                    BrowserTabs.RemoveAt( TabControl.SelectedIndex );
                }
                else
                {
                    //Remove the matching DataContext from the BrowserTabs collection
                    browserViewModel = ( BrowserTabViewModel )originalSource.DataContext;
                    BrowserTabs.Remove( browserViewModel );
                }

                browserViewModel.WebBrowser.Dispose( );
            }
        }

        /// <summary>
        /// Closes the active tab.
        /// </summary>
        public void CloseActiveTab( )
        {
            var _index = TabControl.Items.IndexOf( TabControl.SelectedItem );
            TabControl.Items.RemoveAt( _index );
            if( ( TabControl.Items.Count - 1 ) > _index )
            {
                TabControl.SelectedItem = TabControl.Items[ _index ];
            }
        }

        /// <summary>
        /// Closes the search.
        /// </summary>
        private void CloseSearch( )
        {
            if( _isSearchOpen )
            {
                _isSearchOpen = false;
                InvokeIf( ( ) =>
                {
                   Browser.GetBrowser( )
                       ?.StopFinding( true );
                } );
            }
        }

        /// <summary>
        /// Closes the other tabs.
        /// </summary>
        private void CloseOtherTabs( )
        {
            try
            {
                var _listToClose = new List<BrowserTabItem>( );
                foreach( BrowserTabItem _tab in TabControl.Items )
                {
                    if( _tab != _currentTab
                        && _tab != TabControl.SelectedItem )
                    {
                        _listToClose.Add( _tab );
                    }
                }

                foreach( var _tab in _listToClose )
                {
                    TabControl.Items.Remove( _tab );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Creates the new tab.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="showSideBar">
        /// if set to <c>true</c> [show side bar].
        /// </param>
        /// <param name="legacyBindingEnabled">
        /// if set to <c>true</c> [legacy binding enabled].
        /// </param>
        private void CreateNewTab( string url, bool showSideBar = false, 
                                   bool legacyBindingEnabled = false )
        {
            BrowserTabs.Add( new BrowserTabViewModel( url )
            {
                ShowSidebar = showSideBar, 
                LegacyBindingEnabled = legacyBindingEnabled
            } );
        }

        /// <summary>
        /// Customs the command binding.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private void CustomCommandBinding( object sender, ExecutedRoutedEventArgs e )
        {
            var param = e.Parameter.ToString( );
            if( BrowserTabs.Count > 0 )
            {
                var originalSource = ( FrameworkElement )e.OriginalSource;

                //TODO: Remove duplicate code
                BrowserTabViewModel browserViewModel;

                if( originalSource is ChatWindow )
                {
                    browserViewModel = BrowserTabs[ TabControl.SelectedIndex ];
                }
                else
                {
                    browserViewModel = ( BrowserTabViewModel )originalSource.DataContext;
                }

                switch( param )
                {
                    case "CustomRequest":
                        browserViewModel.LoadCustomRequestExample( );
                        break;

                    case "OpenDevTools":
                        browserViewModel.WebBrowser.ShowDevTools( );
                        break;

                    case "ZoomIn":
                    {
                        var cmd = browserViewModel.WebBrowser.ZoomInCommand;
                        cmd.Execute( null );
                        break;
                    }

                    case "ZoomOut":
                    {
                        var cmd = browserViewModel.WebBrowser.ZoomOutCommand;
                        cmd.Execute( null );
                        break;
                    }

                    case "ZoomReset":
                    {
                        var cmd = browserViewModel.WebBrowser.ZoomResetCommand;
                        cmd.Execute( null );
                        break;
                    }

                    case "ToggleAudioMute":
                    {
                        var cmd = browserViewModel.WebBrowser.ToggleAudioMuteCommand;
                        cmd.Execute( null );
                        break;
                    }

                    case "ClearHttpAuthCredentials":
                    {
                        var browserHost = browserViewModel.WebBrowser.GetBrowserHost( );
                        if( browserHost != null 
                            && !browserHost.IsDisposed )
                        {
                            var requestContext = browserHost.RequestContext;
                            requestContext.ClearHttpAuthCredentials( );
                            requestContext.ClearHttpAuthCredentialsAsync( ).ContinueWith( x =>
                            {
                                Console.WriteLine( "RequestContext.ClearHttpAuthCredentials returned " + x.Result );
                            } );
                        }

                        break;
                    }

                    case "ToggleSidebar":
                        browserViewModel.ShowSidebar = !browserViewModel.ShowSidebar;
                        break;

                    case "ToggleDownloadInfo":
                        browserViewModel.ShowDownloadInfo = !browserViewModel.ShowDownloadInfo;
                        break;

                    case "ResizeHackTests":
                        ReproduceWasResizedCrashAsync( );
                        break;

                    case "AsyncJsbTaskTests":
                        CefSharpSettings.ConcurrentTaskExecution = true;
                        CreateNewTab( _searchEngineUrl, true );
                        TabControl.SelectedIndex = TabControl.Items.Count - 1;
                        break;

                    case "LegacyBindingTest":
                        CreateNewTab( _searchEngineUrl, true, true );
                        TabControl.SelectedIndex = TabControl.Items.Count - 1;
                        break;
                }
            }
        }

        /// <summary>
        /// DownloadItems the in progress.
        /// </summary>
        /// <returns>
        /// </returns>
        public bool DownloadsInProgress( )
        {
            if( _downloadItems?.Values?.Count > 0 )
            {
                Busy( );
                foreach( var _item in _downloadItems.Values )
                {
                    if( _item.IsInProgress )
                    {
                        return true;
                    }
                }

                Chill( );
            }

            return false;
        }

        /// <summary>
        /// Enables the URL home button.
        /// </summary>
        /// <param name="canGoHome">
        /// if set to <c>true</c> [can go forward].
        /// </param>
        private void EnableUrlHomeButton( bool canGoHome )
        {
            InvokeIf( ( ) =>
            {
                if( canGoHome )
                {
                    UrlHomeButton.Visibility = Visibility.Visible;
                }
                else
                {
                    UrlHomeButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the URL cancel button.
        /// </summary>
        /// <param name="canCancel">
        /// if set to <c>true</c> [can go forward].
        /// </param>
        private void EnableUrlCancelButton( bool canCancel )
        {
            InvokeIf( ( ) =>
            {
                if( canCancel )
                {
                    UrlCancelButton.Visibility = Visibility.Visible;
                }
                else
                {
                    UrlCancelButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the URL refresh button.
        /// </summary>
        /// <param name="canRefresh">
        /// if set to <c>true</c> [can refresh].
        /// </param>
        private void EnableUrlRefreshButton( bool canRefresh )
        {
            InvokeIf( ( ) =>
            {
                if( canRefresh )
                {
                    UrlRefreshButton.Visibility = Visibility.Visible;
                }
                else
                {
                    UrlRefreshButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the back button.
        /// </summary>
        /// <param name="canGoBack">if set to
        /// <c>true</c> [can go back].</param>
        private void EnableUrlBackButton( bool canGoBack )
        {
            InvokeIf( ( ) =>
            {
                if( canGoBack )
                {
                    UrlBackButton.Visibility = Visibility.Visible;
                }
                else
                {
                    UrlBackButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the forward button.
        /// </summary>
        /// <param name="canGoForward">
        /// if set to <c>true</c> [can go forward].
        /// </param>
        private void EnableUrlForwardButton( bool canGoForward )
        {
            InvokeIf( ( ) =>
            {
                if( canGoForward )
                {
                    UrlForwardButton.Visibility = Visibility.Visible;
                }
                else
                {
                    UrlForwardButton.Visibility = Visibility.Hidden;
                }
            } );
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
        /// Finds the text on page.
        /// </summary>
        /// <param name="next">if set to
        /// <c>true</c> [next].</param>
        private void FindTextOnPage( bool next = true )
        {
            Busy( );
            var _first = _lastSearch != UrlPanelTextBox.Text;
            _lastSearch = UrlPanelTextBox.Text;
            if( _lastSearch.IsNull( ) )
            {
                Browser.GetBrowser( )?.Find( _lastSearch, true, false, !_first );
            }
            else
            {
                Browser.GetBrowser( )?.StopFinding( true );
            }

            UrlPanelTextBox.Focus( );
            Chill( );
        }

        /// <summary>
        /// Calculates the download path.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public string GetDownloadPath( DownloadItem item )
        {
            return item.SuggestedFileName;
        }

        /// <summary>
        /// Gets the resource stream.
        /// </summary>
        /// <param name="fileName">The fileName.</param>
        /// <param name="nameSpace">if set to
        /// <c>true</c> [with nameSpace].</param>
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
        /// Gets the browser asynchronous.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="focused">if set to <c>true</c> [focused].</param>
        /// <returns></returns>
        private protected Task<ChromiumWebBrowser> GetBrowserAsync( string url, bool focused )
        {
            var _tcs = new TaskCompletionSource<ChromiumWebBrowser>( );
            try
            {
                ThrowIf.Null( url, nameof( url ) );
                ThrowIf.Null( focused, nameof( focused ) );
                var _browser = AddNewBrowserTab( url, focused );
                _tcs.SetResult( _browser );
                return _tcs.Task;
            }
            catch( Exception ex )
            {
                _tcs.SetException( ex );
                Fail( ex );
                return default( Task<ChromiumWebBrowser> );
            }
        }

        /// <summary>
        /// Gets the resource names.
        /// </summary>
        /// <param name="resxPath">The RESX file path.</param>
        /// <returns></returns>
        private protected List<string> GetResourceNames( string resxPath )
        {
            try
            {
                ThrowIf.Null( resxPath, nameof( resxPath ) );
                var _names = new List<string>( );
                using var reader = new ResXResourceReader( resxPath );
                foreach( DictionaryEntry entry in reader )
                {
                    _names.Add( entry.Key.ToString( ) );
                }

                return _names?.Any( ) == true
                    ? _names
                    : default( List<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( List<string> );
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
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls;

            // text-davinci-002, text-davinci-003
            var _model = ModelDropDown.SelectedItem.ToString( );
            var _url = "https://api.openai.com/v1/completions";
            if( _model?.IndexOf( "gpt-3.5-turbo" ) != -1 )
            {
                //Chat GTP 4 https://openai.com/research/gpt-4
                _url = "https://api.openai.com/v1/chat/completions";
            }

            var _request = WebRequest.Create( _url );
            _request.Method = "POST";
            _request.ContentType = "application/json";
            _request.Headers.Add( "Authorization", "Bearer " + App.OpenAiKey );
            var _maxTokens = int.Parse( MaxTokenTextBox.Text );// 10000

            // 0.5
            var _temp = double.Parse( TemperatureSlider.Value.ToString( ) );
            if( ( _temp < 0d ) | ( _temp > 1d ) )
            {
                var _msg = "Randomness has to be between 0 and 2"
                    + "with higher values resulting in more random text; Default 1 ";

                SendNotification( _msg );
                return "";
            }

            var _data = "";
            if( _model?.IndexOf( "gpt-3.5-turbo" ) != -1 )
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
            if( _model?.IndexOf( "gpt-3.5-turbo" ) != -1 )
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
        /// Opens the GPT file dialog.
        /// </summary>
        private protected void LoadGptFileDialog( )
        {
            try
            {
                var _gptFileDialog = new GptFileDialog
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                App.ActiveWindows.Add( "GptFileDialog", _gptFileDialog );
                _gptFileDialog.Hide( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the file browser.
        /// </summary>
        private protected void LoadFileBrowser( )
        {
            try
            {
                var _fileBrowser = new FileBrowser( )
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                App.ActiveWindows.Add( "FileBrowser", _fileBrowser );
                _fileBrowser.Hide( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the search dialog.
        /// </summary>
        private protected void LoadSearchDialog( )
        {
            try
            {
                var _searchDialog = new SearchDialog( );
                _searchDialog.Hide( );
                App.ActiveWindows.Add( "SearchDialog", _searchDialog );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the prompt dialog.
        /// </summary>
        private protected void LoadSystemDialog( )
        {
            try
            {
                var _default = App.Instructions;
                var _systemDialog = new SystemDialog( )
                {
                    Topmost = true
                };

                _systemDialog.SystemDialogTextBox.Text = _default;
                App.ActiveWindows.Add( "SystemDialog", _systemDialog );
                _systemDialog.Hide( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the calculator window.
        /// </summary>
        private protected void LoadCalculator( )
        {
            try
            {
                var _calculator = new CalculatorWindow( )
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                App.ActiveWindows.Add( "CalculatorWindow", _calculator );
                _calculator.Hide( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Loads the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        private void LoadUrl( string url )
        {
            try
            {
                ThrowIf.Null( url, nameof( url ) );
                Busy( );
                var _newUrl = url;
                var _urlLower = url.Trim( )?.ToLower( );
                SetTabText( Browser, "Loading..." );
                if( _urlLower == "localhost" )
                {
                    _newUrl = "http://localhost/";
                }
                else if( url.IsFilePath( ) )
                {
                    _newUrl = url.PathToUrl( );
                }
                else
                {
                    Uri.TryCreate( url, UriKind.Absolute, out var _outUri );
                    if( !( _urlLower.StartsWith( "http" )
                        || _urlLower.StartsWith( Locations.Internal ) ) )
                    {
                        if( _outUri == null
                            || _outUri.Scheme != Uri.UriSchemeFile )
                        {
                            _newUrl = "https://" + url;
                        }
                    }

                    if( _urlLower.StartsWith( Locations.Internal + ":" )
                        || ( Uri.TryCreate( _newUrl, UriKind.Absolute, out _outUri )
                            && ( ( ( _outUri.Scheme == Uri.UriSchemeHttp
                                        || _outUri.Scheme == Uri.UriSchemeHttps )
                                    && _newUrl?.Contains( "." ) == true )
                                || _outUri.Scheme == Uri.UriSchemeFile ) ) )
                    {
                    }
                    else
                    {
                        _newUrl = Locations.Google + HttpUtility.UrlEncode( url );
                    }
                }

                Browser.Load( _newUrl );
                SetUrl( _newUrl );
                EnableUrlBackButton( true );
                EnableUrlForwardButton( false );
                Chill( );
            }
            catch( Exception e )
            {
                Fail( e );
            }
        }

        /// <summary>
        /// Nexts the tab.
        /// </summary>
        private void NextTab( )
        {
            if( IsLastTab( ) )
            {
                var _msg = "At The End";
                SendMessage( _msg );
            }
            else
            {
                var _next = TabControl.SelectedIndex + 1;
                TabControl.SelectedItem = TabControl.Items[ _next ];
            }
        }

        /// <summary>
        /// Previous tab.
        /// </summary>
        private void PreviousTab( )
        {
            if( IsFirstTab( ) )
            {
                var _msg = "At The Beginning!";
                SendMessage( _msg );
            }
            else
            {
                var _next = TabControl.SelectedIndex - 1;
                TabControl.SelectedItem = TabControl.Items[ _next ];
            }
        }

        /// <summary>
        /// Opens the file browser.
        /// </summary>
        private protected void OpenFileBrowser( )
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
        /// Opens the prompt dialog.
        /// </summary>
        private protected void OpenSystemDialog( )
        {
            try
            {
                if( App.ActiveWindows.Keys.Contains( "SystemDialog" ) )
                {
                    var _window = ( SystemDialog )App.ActiveWindows[ "SystemDialog" ];
                    _window.Owner = this;
                    _window.Show( );
                }
                else
                {
                    var _systemDialog = new SystemDialog
                    {
                        Topmost = true,
                        Owner = this,
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
        /// Opens the calculator.
        /// </summary>
        private protected void OpenCalculator( )
        {
            try
            {
                if( App.ActiveWindows.Keys.Contains( "CalculatorWindow" ) )
                {
                    var _window = ( CalculatorWindow )App.ActiveWindows[ "CalculatorWindow" ];
                    _window.Topmost = true;
                    _window.Show( );
                }
                else
                {
                    var _calculator = new CalculatorWindow
                    {
                        Topmost = true,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    _calculator.Show( );
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
                Busy( );
                if( App.ActiveWindows.Keys.Contains( "GptFileDialog" ) )
                {
                    var _window = ( GptFileDialog )App.ActiveWindows[ "GptFileDialog" ];
                    _window.Owner = this;
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

                Chill( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the search.
        /// </summary>
        private void OpenSearch( )
        {
            try
            {
                if( !_isSearchOpen )
                {
                    _isSearchOpen = true;
                    InvokeIf( ( ) =>
                    {
                        UrlPanelTextBox.Text = _lastSearch;
                        UrlPanelTextBox.Focus( );
                        UrlPanelTextBox.SelectAll( );
                    } );
                }
                else
                {
                    InvokeIf( ( ) =>
                    {
                        UrlPanelTextBox.Focus( );
                        UrlPanelTextBox.SelectAll( );
                    } );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Shows the search dialog.
        /// </summary>
        private protected virtual void OpenSearchDialog( double x, double y )
        {
            try
            {
                var _searchDialog = (SearchDialog)App.ActiveWindows[ "SearchDialog" ];
                _searchDialog.Owner = this;
                _searchDialog.Topmost = true;
                _searchDialog.Left = x;
                _searchDialog.Top = y + 100;
                _searchDialog.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the developer tools.
        /// </summary>
        private void OpenDeveloperTools( )
        {
            try
            {
                if( Browser == null )
                {
                    var _message = "CurrentBrowser is null!";
                    SendNotification( _message );
                }
                else
                {
                    Browser.ShowDevTools( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the downloads tab.
        /// </summary>
        public void OpenDownloadsTab( )
        {
            if( _downloadStrip != null )
            {
                TabControl.SelectedItem = _downloadStrip;
            }
            else
            {
                var _brw = AddNewBrowserTab( Locations.Downloads );
                _downloadStrip = ( BrowserTabItem )_brw.Parent;
            }
        }

        /// <summary>
        /// Opens the new tab.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OpenNewTab( object sender, ExecutedRoutedEventArgs e )
        {
            CreateNewTab( _searchEngineUrl );
            TabControl.SelectedIndex = TabControl.Items.Count - 1;
        }

        /// <summary>
        /// Opens the tab command binding.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/>
        /// instance containing the event data.</param>
        /// <exception cref="Bubba.ErrorWindow.Exception">
        /// Please provide a valid command parameter for binding</exception>
        private void OpenTabCommandBinding( object sender, ExecutedRoutedEventArgs e )
        {
            var url = e.Parameter.ToString( );
            if( string.IsNullOrEmpty( url ) )
            {
                var _message = "Please provide a valid command parameter for binding"; 
                throw new Exception( _message );
            }

            CreateNewTab( url, true );
            TabControl.SelectedIndex = TabControl.Items.Count - 1;
        }

        /// <summary>
        /// Prints to PDF command binding.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="ExecutedRoutedEventArgs"/>
        /// instance containing the event data.
        /// </param>
        private async void PrintToPdfCommandBinding( object sender, ExecutedRoutedEventArgs e )
        {
            if( BrowserTabs.Count > 0 )
            {
                var originalSource = ( FrameworkElement )e.OriginalSource;
                BrowserTabViewModel browserViewModel;
                if( originalSource is ChatWindow )
                {
                    browserViewModel = BrowserTabs[ TabControl.SelectedIndex ];
                }
                else
                {
                    browserViewModel = ( BrowserTabViewModel )originalSource.DataContext;
                }

                var dialog = new SaveFileDialog
                {
                    DefaultExt = ".pdf",
                    Filter = "Pdf documents (.pdf)|*.pdf"
                };

                var success = await browserViewModel.WebBrowser.PrintToPdfAsync( dialog.FileName, new PdfPrintSettings
                {
                    MarginType = CefPdfPrintMarginType.Custom,
                    MarginBottom = 0.01,
                    MarginTop = 0.01,
                    MarginLeft = 0.01,
                    MarginRight = 0.01,
                } );

                if( success )
                {
                    MessageBox.Show( "Pdf was saved to " + dialog.FileName );
                }
                else
                {
                    MessageBox.Show( "Unable to save Pdf, check you have write permissions to " + dialog.FileName );
                }
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
            try
            {
                ThrowIf.Empty( input, nameof( input ) );
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
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets the models.
        /// </summary>
        /// 
        private async void PopulateModelsAsync( )
        {
            try
            {
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
                ModelDropDown.Items.Clear( );
                Dispatcher.BeginInvoke( ( ) =>
                {
                    foreach( var _lm in _models )
                    {
                        if( !_lm.StartsWith( "ft" ) )
                        {
                            ModelDropDown.AddItem( _lm );
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
        /// Populates the image sizes.
        /// </summary>
        private void PopulateImageSizes( )
        {
            try
            {
                ImageSizeDropDown.Items?.Clear( );
                if( string.IsNullOrEmpty( _model ) )
                {
                    ImageSizeDropDown.AddItem( "256 X 256" );
                    ImageSizeDropDown.AddItem( "512 X 512" );
                    ImageSizeDropDown.AddItem( "1024 X 1024" );
                    ImageSizeDropDown.AddItem( "1792 X 1024" );
                    ImageSizeDropDown.AddItem( "1024 X 1792" );
                }
                else if( !string.IsNullOrEmpty( _model ) )
                {
                    switch( _model )
                    {
                        case "dall-e-2":
                        {
                            ImageSizeDropDown.AddItem( "256 X 256" );
                            ImageSizeDropDown.AddItem( "512 X 512" );
                            ImageSizeDropDown.AddItem( "1024 X 1024" );
                            break;
                        }
                        case "dall-e-3":
                        {
                            ImageSizeDropDown.AddItem( "1024 X 1024" );
                            ImageSizeDropDown.AddItem( "1792 X 1024" );
                            ImageSizeDropDown.AddItem( "1024 X 1792" );
                            break;
                        }
                        default:
                        { 
                            ImageSizeDropDown.AddItem( "512 X 512" );
                            ImageSizeDropDown.AddItem( "1024 X 1024" );
                            ImageSizeDropDown.AddItem( "1792 X 1024" );
                            ImageSizeDropDown.AddItem( "1024 X 1792" );

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
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "gpt-4-0613" );
                ModelDropDown.AddItem( "gpt-4-0314" );
                ModelDropDown.AddItem( "gpt-4-turbo-2024-04-09" );
                ModelDropDown.AddItem( "gpt-4o-2024-08-06" );
                ModelDropDown.AddItem( "gpt-4o-2024-11-20" );
                ModelDropDown.AddItem( "gpt-4o-2024-05-13" );
                ModelDropDown.AddItem( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.AddItem( "o1-2024-12-17" );
                ModelDropDown.AddItem( "o1-pro-2025-03-19" );
                ModelDropDown.AddItem( "o1-mini-2024-09-12" );
                ModelDropDown.AddItem( "o3-mini-2025-01-31"  );
                ModelDropDown.AddItem( "text-davinci-003" );
                ModelDropDown.AddItem( "text-curie-001" );
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
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "gpt-4-0613" );
                ModelDropDown.AddItem( "gpt-4-0314" );
                ModelDropDown.AddItem( "gpt-4-turbo-2024-04-09" );
                ModelDropDown.AddItem( "gpt-4o-2024-08-06" );
                ModelDropDown.AddItem( "gpt-4o-2024-11-20" );
                ModelDropDown.AddItem( "gpt-4o-2024-05-13" );
                ModelDropDown.AddItem( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.AddItem( "o1-2024-12-17" );
                ModelDropDown.AddItem( "o1-pro-2025-03-19" );
                ModelDropDown.AddItem( "o1-mini-2024-09-12" );
                ModelDropDown.AddItem( "o3-mini-2025-01-31" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the assistant models.
        /// </summary>
        private void PopulateAssistantModels( )
        {
            try
            {
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "gpt-4-0613" );
                ModelDropDown.AddItem( "gpt-4-0314" );
                ModelDropDown.AddItem( "gpt-4-turbo-2024-04-09" );
                ModelDropDown.AddItem( "gpt-4o-2024-08-06" );
                ModelDropDown.AddItem( "gpt-4o-2024-11-20" );
                ModelDropDown.AddItem( "gpt-4o-2024-05-13" );
                ModelDropDown.AddItem( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.AddItem( "o1-2024-12-17" );
                ModelDropDown.AddItem( "o1-pro-2025-03-19" );
                ModelDropDown.AddItem( "o3-mini-2025-01-31" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the image generation models.
        /// </summary>
        private void PopulateImageEditingModels( )
        {
            try
            {
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "gpt-image-1" );
                ModelDropDown.AddItem( "dall-e-2" );
                ModelDropDown.AddItem( "dall-e-3" );
                ModelDropDown.AddItem( "gpt-4-0613" );
                ModelDropDown.AddItem( "gpt-4-0314" );
                ModelDropDown.AddItem( "gpt-4o-2024-11-20" );
                ModelDropDown.AddItem( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.AddItem( "gpt-4.1-2025-04-14" );
                ModelDropDown.AddItem( "gpt-4.1-mini-2025-04-14" );
                ModelDropDown.AddItem( "gpt-4.1-nano-2025-04-14" );
                ModelDropDown.AddItem( "o1-2024-12-17" );
                ModelDropDown.AddItem( "o3-2025-04-16" );
                ModelDropDown.AddItem( "o3-mini-2025-01-31" );
                ModelDropDown.AddItem( "o4-mini-2025-04-16" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the image generation models.
        /// </summary>
        private void PopulateImageGenerationModels( )
        {
            try
            {
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "gpt-image-1" );
                ModelDropDown.AddItem( "dall-e-2" );
                ModelDropDown.AddItem( "dall-e-3" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the reasoning models.
        /// </summary>
        private void PopulateReasoningModels( )
        {
            try
            {
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "gpt-4o-2024-08-06" );
                ModelDropDown.AddItem( "gpt-4o-2024-11-20" );
                ModelDropDown.AddItem( "gpt-4o-2024-05-13" );
                ModelDropDown.AddItem( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.AddItem( "o1-2024-12-17" );
                ModelDropDown.AddItem( "o1-pro-2025-03-19" );
                ModelDropDown.AddItem( "o3-2025-04-16" );
                ModelDropDown.AddItem( "o3-mini-2025-01-31" );
                ModelDropDown.AddItem( "o4-mini-2025-04-16" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the translation models.
        /// </summary>
        private void PopulateResponseModels( )
        {
            try
            {
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "gpt-4-0613" );
                ModelDropDown.AddItem( "gpt-4-0314" );
                ModelDropDown.AddItem( "gpt-4-turbo-2024-04-09" );
                ModelDropDown.AddItem( "gpt-4o-2024-08-06" );
                ModelDropDown.AddItem( "gpt-4o-2024-11-20" );
                ModelDropDown.AddItem( "gpt-4o-2024-05-13" );
                ModelDropDown.AddItem( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.AddItem( "o1-2024-12-17" );
                ModelDropDown.AddItem( "o1-pro-2025-03-19" );
                ModelDropDown.AddItem( "o1-mini-2024-09-12" );
                ModelDropDown.AddItem( "o3-mini-2025-01-31" );
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
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "whisper-1" );
                ModelDropDown.AddItem( "gpt-4o-mini-transcribe" );
                ModelDropDown.AddItem( "gpt-4o-transcribe" );
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
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "whisper-1" );
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
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "text-embedding-3-small" );
                ModelDropDown.AddItem( "text-embedding-3-large" );
                ModelDropDown.AddItem( "text-embedding-ada-002" );
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
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "gpt-3.5-turbo-0125" );
                ModelDropDown.AddItem( "gpt-3.5-turbo-1106" );
                ModelDropDown.AddItem( "gpt-3.5-turbo-0613" );
                ModelDropDown.AddItem( "gpt-4-0613" );
                ModelDropDown.AddItem( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.AddItem( "gpt-4o-2024-08-06" );
                ModelDropDown.AddItem( "gpt-4o-2024-11-20" );
                ModelDropDown.AddItem( "gpt-4o-2024-05-13" );
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
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "tts-1" );
                ModelDropDown.AddItem( "tts-1-hd" );
                ModelDropDown.AddItem( "gpt-4o-mini-tts"  );
                ModelDropDown.AddItem( "gpt-4o-audio-preview-2024-12-17" );
                ModelDropDown.AddItem( "gpt-4o-audio-preview-2024-10-01" );
                ModelDropDown.AddItem( "gpt-4o-mini-audio-preview-2024-12-17" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the text to speech models.
        /// </summary>
        private void PopulateTextToSpeechModels( )
        {
            try
            {
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "tts-1" );
                ModelDropDown.AddItem( "tts-1-hd" );
                ModelDropDown.AddItem( "gpt-4o-mini-tts" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the upload API models.
        /// </summary>
        private void PopulateUploadModels( )
        {
            try
            {
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "gpt-4-0613" );
                ModelDropDown.AddItem( "gpt-4-0314" );
                ModelDropDown.AddItem( "gpt-4-turbo-2024-04-09" );
                ModelDropDown.AddItem( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.AddItem( "gpt-4o-2024-08-06" );
                ModelDropDown.AddItem( "gpt-4o-2024-11-20" );
                ModelDropDown.AddItem( "gpt-4o-2024-05-13" );
                ModelDropDown.AddItem( "o1-2024-12-17" );
                ModelDropDown.AddItem( "o1-mini-2024-09-12" );
                ModelDropDown.AddItem( "o3-mini-2025-01-31" );
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
                ModelDropDown.Items?.Clear( );
                ModelDropDown.AddItem( "gpt-4-0613" );
                ModelDropDown.AddItem( "gpt-4-0314" );
                ModelDropDown.AddItem( "gpt-4-turbo-2024-04-09" );
                ModelDropDown.AddItem( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.AddItem( "gpt-4o-2024-08-06" );
                ModelDropDown.AddItem( "gpt-4o-2024-11-20" );
                ModelDropDown.AddItem( "gpt-4o-2024-05-13" );
                ModelDropDown.AddItem( "o1-2024-12-17" );
                ModelDropDown.AddItem( "o1-mini-2024-09-12" );
                ModelDropDown.AddItem( "o3-mini-2025-01-31" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the file API models.
        /// </summary>
        private void PopulateFileModels( )
        {
            try
            {
                ModelDropDown.Items?.Clear( );
                ModelDropDown.Items.Add( "gpt-4-0613" );
                ModelDropDown.Items.Add( "gpt-4-0314" );
                ModelDropDown.Items.Add( "gpt-4-turbo-2024-04-09" );
                ModelDropDown.Items.Add( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.Items.Add( "gpt-4o-2024-08-06" );
                ModelDropDown.Items.Add( "gpt-4o-2024-11-20" );
                ModelDropDown.Items.Add( "gpt-4o-2024-05-13" );
                ModelDropDown.Items.Add( "o1-2024-12-17" );
                ModelDropDown.Items.Add( "o1-mini-2024-09-12" );
                ModelDropDown.Items.Add( "o3-mini-2025-01-31" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the agent models.
        /// </summary>
        private void PopulateAgentModels( )
        {
            try
            {
                ModelDropDown.Items?.Clear( );
                ModelDropDown.Items.Add( "o1-2024-12-17" );
                ModelDropDown.Items.Add( "o1-mini-2024-09-12" );
                ModelDropDown.Items.Add( "o3-mini-2025-01-31" );
                ModelDropDown.Items.Add( "gpt-4-0613" );
                ModelDropDown.Items.Add( "gpt-4o-mini-2024-07-18" );
                ModelDropDown.Items.Add( "gpt-4o-2024-11-20" );
                ModelDropDown.Items.Add( "gpt-4.1-2025-04-14" );
                ModelDropDown.Items.Add( "gpt-4.1-nano-2025-04-14" );
                ModelDropDown.Items.Add( "gpt-4.1-mini-2025-04-14" );
                ModelDropDown.Items.Add( "gpt-4.5-preview-2025-02-27" );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the language ListBox.
        /// </summary>
        private void PopulateDocumentListBox( )
        {
            try
            {
                if( !string.IsNullOrEmpty( _language ) )
                {
                    switch( _language )
                    {
                        case "TXT":
                        {
                            PopulateTextDocuments( );
                            break;
                        }
                        case "CS":
                        {
                            PopulateCSharpDocuments( );
                            break;
                        }
                        case "PY":
                        {
                            PopulatePythonDocuments( );
                            break;
                        }
                        case "SQL":
                        {
                            PopulateSqlDocuments( );
                            break;
                        }
                        case "JS":
                        {
                            PopulateJavaScriptDocuments( );
                            break;
                        }
                        case "CPP":
                        {
                            PopulateCPlusDocuments( );
                            break;
                        }
                        case "VBA":
                        {
                            PopulateVisualBasicDocuments( );
                            break;
                        }
                        default:
                        {
                            PopulateTextDocuments( );
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
        /// Populates the visual basic documents.
        /// </summary>
        private void PopulateVisualBasicDocuments( )
        {
            var _path = Locations.PathPrefix + @"Resources\Documents\Editor\VBA\";
            TabControl.SelectedIndex = 0;
            Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.VisualBasic;
            DocumentListBox.Items?.Clear( );
            var _documents = Directory.GetFiles( _path );
            foreach( var _file in _documents )
            {
                var _item = new MetroListBoxItem(  )
                {
                    Tag = Path.GetFullPath( _file ),
                    Content = Path.GetFileNameWithoutExtension( _file )
                };

                DocumentListBox.Items?.Add( _item );
            }
        }

        /// <summary>
        /// Populates the c plus documents.
        /// </summary>
        private void PopulateCPlusDocuments( )
        {
            try
            {
                var _path = Locations.PathPrefix + @"Resources\Documents\Editor\CPP\";
                TabControl.SelectedIndex = 0;
                Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.C;
                Editor.DocumentSource = _path;
                DocumentListBox.Items?.Clear( );
                var _documents = Directory.GetFiles( _path );
                foreach( var _file in _documents )
                {
                    var _item = new MetroListBoxItem(  )
                    {
                        Tag = Path.GetFullPath( _file ),
                        Content = Path.GetFileNameWithoutExtension( _file )
                    };

                    DocumentListBox.Items?.Add( _item );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the java script documents.
        /// </summary>
        private void PopulateJavaScriptDocuments( )
        {
            try
            {
                var _path = Locations.PathPrefix + @"Resources\Documents\Editor\JS\";
                TabControl.SelectedIndex = 0;
                Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.JScript;
                DocumentListBox.Items?.Clear( );
                var _documents = Directory.GetFiles( _path );
                foreach( var _file in _documents )
                {
                    var _item = new MetroListBoxItem(  )

                    {
                        Tag = Path.GetFullPath( _file ),
                        Content = Path.GetFileNameWithoutExtension( _file )
                    };

                    DocumentListBox.Items?.Add( _item );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the SQL documents.
        /// </summary>
        private void PopulateSqlDocuments( )
        {
            try
            {
                var _path = @"C:\Users\terry\source\repos\Bubba\Resources\Documents\Editor\SQL\";
                Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.SQL;
                DocumentListBox.Items?.Clear( );
                var _documents = Directory.GetFiles( _path );
                foreach( var _file in _documents )
                {
                    var _item = new MetroListBoxItem(  )
                    {
                        Tag = Path.GetFullPath( _file ),
                        Content = Path.GetFileNameWithoutExtension( _file )
                    };

                    DocumentListBox.Items?.Add( _item );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the python documents.
        /// </summary>
        private void PopulatePythonDocuments( )
        {
            try
            {
                var _path = Locations.PathPrefix + @"Resources\Documents\Editor\PY\";
                Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.Text;
                DocumentListBox.Items?.Clear( );
                var _documents = Directory.GetFiles( _path );
                foreach( var _file in _documents )
                {
                    var _item = new MetroListBoxItem( )
                    {
                        Tag = Path.GetFullPath( _file ),
                        Content = Path.GetFileNameWithoutExtension( _file )
                    };

                    DocumentListBox.Items?.Add( _item );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the c sharp documents.
        /// </summary>
        private void PopulateCSharpDocuments( )
        {
            try
            {
                var _path = Locations.PathPrefix + @"Resources\Documents\Editor\CS\";
                Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.CSharp;
                DocumentListBox.Items?.Clear( );
                var _documents = Directory.GetFiles( _path );
                foreach( var _file in _documents )
                {
                    var _item = new MetroListBoxItem( )
                    {
                        Tag = Path.GetFullPath( _file ),
                        Content = Path.GetFileNameWithoutExtension( _file )
                    };

                    DocumentListBox.Items?.Add( _item );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the text documents.
        /// </summary>
        private void PopulateTextDocuments( )
        {
            try
            {
                var _path = Locations.PathPrefix + @"Resources\Documents\Prompts\";
                Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.Text;
                DocumentListBox.Items?.Clear( );
                var _documents = Directory.GetFiles( _path );
                foreach( var _file in _documents )
                {
                    var _item = new  MetroListBoxItem( )
                    {
                        Tag = Path.GetFullPath( _file ),
                        Content = Path.GetFileNameWithoutExtension( _file )
                    };

                    DocumentListBox.Items?.Add( _item );
                }
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
                VoicesDropDown.Items?.Clear( );
                _voiceOptions.Clear(  );
                foreach( var _voice in _synth.GetInstalledVoices( ) )
                {
                    var _info = _voice.VoiceInfo;
                    var _start = "Microsoft".ToCharArray(  );
                    var _end = "Desktop".ToCharArray( );
                    var _first = _info.Name.TrimStart( _start );
                    var _last = _first.TrimEnd( _end );
                    var _lower = _last.ToLower(  );
                    _voiceOptions.Add( _info.Name, _lower );
                    var _item = new MetroDropDownItem( )
                    {
                        Tag = _info.Name,
                        Content = _lower
                    };

                    VoicesDropDown.Items.Add( _item );
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
                var _aiVoices = new Dictionary<string, string>( );
                _aiVoices.Add( "alloy", "alloy" );
                _aiVoices.Add( "ash", "ash" );
                _aiVoices.Add( "coral", "coral" );
                _aiVoices.Add( "echo", "echo" );
                _aiVoices.Add( "onyx", "onyx" );
                _aiVoices.Add( "fable", "fable" );
                _aiVoices.Add( "nova", "nova" );
                _aiVoices.Add( "sage", "sage" );
                _aiVoices.Add( "shimmer", "shimer" );
                VoicesDropDown.Items.Clear(  );
                foreach( var _voice in _aiVoices )
                {
                    var _item = new MetroDropDownItem( )
                    {
                        Tag = _voice.Key,
                        Content = _voice.Value
                    };

                    VoicesDropDown.Items.Add( _item );
                }
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
                PopulateInstalledVoices(  );
                var _aiVoices = new Dictionary<string, string>( );
                _aiVoices.Add( "alloy", "Alloy" );
                _aiVoices.Add( "ash", "Ash" );
                _aiVoices.Add( "coral", "Coral" );
                _aiVoices.Add( "echo", "Echo" );
                _aiVoices.Add( "onyx", "Onyx" );
                _aiVoices.Add( "fable", "Fable" );
                _aiVoices.Add( "nova", "Nova" );
                _aiVoices.Add( "sage", "Sage" );
                _aiVoices.Add( "shimmer", "Shimer" );
                foreach( var _voice in _aiVoices )
                {
                    var _item = new MetroDropDownItem
                    {
                        Tag = _voice.Key,
                        Content = _voice.Value
                    };

                    VoicesDropDown.Items.Add( _item );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the request types.
        /// </summary>
        private void PopulateRequestTypes( )
        {
            try
            {
                GenerationListBox.Items?.Clear( );
                var _names = Enum.GetNames( typeof( API ) );
                foreach( var _request in _names )
                {
                    var _item = new MetroDropDownItem( )
                    {
                        Height = 35,
                        Tag = _request,
                        Content = _request.SplitPascal( )
                    };

                    GenerationListBox.Items.Add( _item );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the language ListBox.
        /// </summary>
        private void PopulateLanguageDropDown( )
        {
            try
            {
                LanguageDropDown.Items?.Clear( );
                var _names = Enum.GetNames( typeof( Languages ) );
                foreach( var _request in _names )
                {
                    var _item = new MetroDropDownItem( )
                    {
                        Height = 35,
                        Tag = _request,
                        Content = _request
                    };

                    LanguageDropDown.Items.Add( _item );
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
        private protected void PopulateDocuments( )
        {
            try
            {
                var _prefix = @"C:\Users\terry\source\repos\Bubba\Resources\Documents\Editor\";
                if( !string.IsNullOrEmpty( _language ) )
                {
                    Editor.Text = "";
                    switch( _language )
                    {
                        case "TXT":
                        {
                            var _pre = @"C:\Users\terry\source\repos\Bubba\Resources\Documents\";
                            var _path = _pre + @"Prompts\";
                            Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.Text;
                            DocumentListBox.Items?.Clear( );
                            var _documents = Directory.GetFiles( _path );
                            foreach( var _file in _documents )
                            {
                                var _item = new MetroListBoxItem( );
                                _item.Tag = Path.GetFullPath( _file );
                                _item.Content = Path.GetFileNameWithoutExtension( _file );
                                DocumentListBox.Items.Add( _item );
                            }

                            break;
                        }
                        case "CS":
                        {
                            var _path = _prefix + @"CS\";
                            Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.CSharp;
                            DocumentListBox.Items?.Clear( );
                            var _documents = Directory.GetFiles( _path );
                            foreach( var _file in _documents )
                            {
                                var _item = new MetroListBoxItem( );
                                _item.Tag = Path.GetFullPath( _file );
                                _item.Content = Path.GetFileNameWithoutExtension( _file );
                                DocumentListBox.Items.Add( _item );
                            }

                            break;
                        }
                        case "PY":
                        {
                            var _path = _prefix + @"PY\";
                            Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.Text;
                            DocumentListBox.Items?.Clear( );
                            var _documents = Directory.GetFiles( _path );
                            foreach( var _file in _documents )
                            {
                                var _item = new MetroListBoxItem( );
                                _item.Tag = Path.GetFullPath( _file );
                                _item.Content = Path.GetFileNameWithoutExtension( _file );
                                DocumentListBox.Items.Add( _item );
                            }

                            break;
                        }
                        case "SQL":
                        {
                            var _path = _prefix + @"SQL\";
                            Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.SQL;
                            DocumentListBox.Items?.Clear( );
                            var _documents = Directory.GetFiles( _path );
                            foreach( var _file in _documents )
                            {
                                var _item = new MetroListBoxItem( );
                                _item.Tag = Path.GetFullPath( _file );
                                _item.Content = Path.GetFileNameWithoutExtension( _file );
                                DocumentListBox.Items.Add( _item );
                            }

                            break;
                        }
                        case "JS":
                        {
                            var _path = _prefix + @"JS\";
                            Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.JScript;
                            DocumentListBox.Items?.Clear( );
                            var _documents = Directory.GetFiles( _path );
                            foreach( var _file in _documents )
                            {
                                var _item = new MetroListBoxItem( );
                                _item.Tag = Path.GetFullPath( _file );
                                _item.Content = Path.GetFileNameWithoutExtension( _file );
                                DocumentListBox.Items.Add( _item );
                            }

                            break;
                        }
                        case "CPP":
                        {
                            var _path = _prefix + @"CPP\";
                            Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.C;
                            Editor.DocumentSource = _path;
                            DocumentListBox.Items?.Clear( );
                            var _documents = Directory.GetFiles( _path );
                            foreach( var _file in _documents )
                            {
                                var _item = new MetroListBoxItem( );
                                _item.Tag = Path.GetFullPath( _file );
                                _item.Content = Path.GetFileNameWithoutExtension( _file );
                                DocumentListBox.Items.Add( _item );
                            }

                            break;
                        }
                        case "VBA":
                        {
                            var _path = _prefix + @"VBA\";
                            Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.VisualBasic;
                            DocumentListBox.Items?.Clear( );
                            var _documents = Directory.GetFiles( _path );
                            foreach( var _file in _documents )
                            {
                                var _item = new MetroListBoxItem( );
                                _item.Tag = Path.GetFullPath( _file );
                                _item.Content = Path.GetFileNameWithoutExtension( _file );
                                DocumentListBox.Items.Add( _item );
                            }

                            break;
                        }
                        default:
                        {
                            var _pre = @"C:\Users\terry\source\repos\Bubba\Resources\Documents\";
                            var _path = _pre + @"Prompts\";
                            Editor.DocumentLanguage = Syncfusion.Windows.Edit.Languages.Text;
                            DocumentListBox.Items?.Clear( );
                            var _documents = Directory.GetFiles( _path );
                            foreach( var _file in _documents )
                            {
                                var _item = new MetroListBoxItem( );
                                _item.Tag = Path.GetFullPath( _file );
                                _item.Content = Path.GetFileNameWithoutExtension( _file );
                                DocumentListBox.Items.Add( _item );
                            }

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
        /// Populates the prompt drop down.
        /// </summary>
        private protected void PopulatePromptDropDown( )
        {
            try
            {
                PromptDropDown.Items?.Clear( );
                var _filepath = @"C:\Users\terry\source\repos\Bubba\Properties\Prompts.resx";
                var _path = Locations.PathPrefix + @"Resources\Documents\Prompts\";
                var _names = GetResourceNames( _filepath );
                foreach( var _file in _names )
                {
                    var _item = new MetroDropDownItem( )
                    {
                        Tag = _file,
                        Content = _file
                    };

                    PromptDropDown.Items.Add( _item );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Activates the chat tab.
        /// </summary>
        private protected void ActivateChatTab( )
        {
            try
            {
                var _tabPages = new Dictionary<string, MetroTabItem>( );
                SetToolbarVisibility( true );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Activates the chat tab.
        /// </summary>
        private protected void ActivateBrowserTab( )
        {
            try
            {
                InitializeBrowser( );
                SetToolbarVisibility( true );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Activates the editor tab.
        /// </summary>
        private protected void ActivateEditorTab( )
        {
            try
            {
                PopulateLanguageDropDown(  );
                PopulateDocumentListBox( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Activates the image tab.
        /// </summary>
        private protected void ActivateImageTab( )
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
        /// Activates the parameter tab.
        /// </summary>
        private protected void ActivateParameterTab( )
        {
            try
            {
                SetToolbarVisibility( true );
                PopulateRequestTypes( );
                PopulateModelsAsync( );
                PopulateVoices( );
                PopulateImageSizes( );
                ClearChatControls( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Activates the chart tab.
        /// </summary>
        private protected void ActivateChartTab( )
        {
            try
            {
                SetToolbarVisibility( true );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Activates the data tab.
        /// </summary>
        private protected void ActivateDataTab( )
        {
            try
            {
                SetToolbarVisibility( true );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Activates the graph tab.
        /// </summary>
        private protected void ActivateGraphTab( )
        {
            try
            {
                SetToolbarVisibility( true );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Activates the busy tab.
        /// </summary>
        private protected void ActivateBusyTab( )
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
        /// Adds the blank tab.
        /// </summary>
        public void AddBlankTab( )
        {
            AddNewBrowserTab( "" );
            Dispatcher.BeginInvoke( ( ) =>
            {
                UrlPanelTextBox.Focus( );
            } );
        }

        /// <summary>
        /// Adds the blank window.
        /// </summary>
        public void AddBlankWindow( )
        {
            var _info = new ProcessStartInfo( Application.ExecutablePath, "" )
            {
                LoadUserProfile = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };

            Process.Start( _info );
        }

        /// <summary>
        /// Adds the new browser tab.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="focus">if set to
        /// <c>true</c> [focus new tab].</param>
        /// <param name="referringUrl">The referring URL.</param>
        /// <returns>
        /// </returns>
        public ChromiumWebBrowser AddNewBrowserTab( string url, bool focus = true,
            string referringUrl = null )
        {
            return Dispatcher.Invoke( delegate
            {
                foreach( BrowserTabItem _item in TabControl.Items )
                {
                    var _tab2 = ( BrowserTabItem )_item.Tag;
                    if( _tab2 != null
                        && _tab2.CurrentUrl == url )
                    {
                        TabControl.SelectedItem = _item;
                        return _tab2.Browser;
                    }
                }

                var _tab = new BrowserTabItem
                {
                    Title = "New Tab"
                };

                TabControl.Items.Insert( TabControl.Items.Count - 1, _tab );
                _newTabItem = _tab;
                var _newTab = AddNewBrowser( _newTabItem, url );
                _newTab.ReferringUrl = referringUrl;
                if( focus )
                {
                    _newTab.BringIntoView( );
                }

                return _newTab.Browser;
            } );
        }

        /// <summary>
        /// Adds the new browser.
        /// </summary>
        /// <param name="tabItem">The tab strip.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private BrowserTabItem AddNewBrowser( BrowserTabItem tabItem, string url )
        {
            if( url == "" )
            {
                url = Locations.NewTab;
            }

            var _newBrowser = new ChromiumWebBrowser( url );
            ConfigureBrowser( _newBrowser );
            _newBrowser.BringIntoView( );
            tabItem.Content = _newBrowser;
            _newBrowser.StatusMessage += OnStatusUpdated;
            _newBrowser.LoadingStateChanged += OnWebBrowserLoadingStateChanged;
            _newBrowser.TitleChanged += OnWebBrowserTitleChanged;
            _newBrowser.LoadError += OnWebBrowserLoadError;
            _newBrowser.AddressChanged += OnWebBrowserAddressChanged;
            _newBrowser.DownloadHandler = _downloadCallback;
            _newBrowser.MenuHandler = _contextMenuCallback;
            _newBrowser.LifeSpanHandler = _lifeSpanCallback;
            _newBrowser.KeyboardHandler = _keyboardCallback;
            _newBrowser.RequestHandler = _requestCallback;
            var _tabItem = new BrowserTabItem
            {
                IsOpen = true,
                Browser = _newBrowser,
                Tab = tabItem,
                OriginalUrl = url,
                CurrentUrl = url,
                Title = "New Tab",
                DateCreated = DateTime.Now
            };

            tabItem.Tag = _tabItem;
            if( !string.IsNullOrEmpty( url )
                && url.StartsWith( Locations.Internal + ":" ) )
            {
                _newBrowser.JavascriptObjectRepository.Register( "host", _hostCallback,
                    BindingOptions.DefaultBinder );
            }

            return _tabItem;
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
        /// Updates the download item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void UpdateDownloadItem( DownloadItem item )
        {
            lock( _downloadItems )
            {
                // SuggestedFileName comes full only
                // in the first attempt so keep it somewhere
                if( item.SuggestedFileName != "" )
                {
                    _downloadNames[ item.Id ] = item.SuggestedFileName;
                }

                // Set it back if it is empty
                if( item.SuggestedFileName == ""
                    && _downloadNames.TryGetValue( item.Id, out var _name ) )
                {
                    item.SuggestedFileName = _name;
                }

                _downloadItems[ item.Id ] = item;

                //UpdateSnipProgress();
            }
        }

        /// <summary>
        /// Refreshes the active tab.
        /// </summary>
        public void RefreshActiveTab( )
        {
            Busy( );
            var _address = Browser.Address;
            Browser.Load( _address );
            Chill( );
        }

        /// <summary>
        /// Reproduces the was resized crash asynchronous.
        /// </summary>
        private void ReproduceWasResizedCrashAsync( )
        {
            CreateNewTab( _searchEngineUrl );
            CreateNewTab( _searchEngineUrl );
            WindowState = WindowState.Normal;
            Task.Run( ( ) =>
            {
                try
                {
                    var random = new Random( );
                    for( var i = 0; i < 20; i++ )
                    {
                        for( var j = 0; j < 150; j++ )
                        {
                            Dispatcher.Invoke( new Action( ( ) =>
                            {
                                var newWidth = Width + ( i % 2 == 0 ? -5 : 5 );
                                var newHeight = Height + ( i % 2 == 0 ? -5 : 5 );
                                if( newWidth < 500 
                                    || newWidth > 1500 )
                                {
                                    newWidth = 1000;
                                }

                                if( newHeight < 500 
                                    || newHeight > 1500 )
                                {
                                    newHeight = 1000;
                                }

                                Width = newWidth;
                                Height = newHeight;
                                var indexes = new List<int>( );
                                for( var k = 0; k < TabControl.Items.Count; k++ )
                                {
                                    if( TabControl.SelectedIndex != k )
                                    {
                                        indexes.Add( k );
                                    }
                                }

                                // Select a random unselected tab
                                TabControl.SelectedIndex = indexes[ random.Next( 0, indexes.Count ) ];
                                if( random.Next( 0, 5 ) == 0 )
                                {
                                    // Don't close the first tab
                                    CloseOtherTabs( ); 
                                    CreateNewTab( _searchEngineUrl );
                                }
                            } ) );

                            // Sleep random amount of time
                            Thread.Sleep( random.Next( 1, 11 ) );
                        }
                    }
                }
                catch( TaskCanceledException ) { } // So it doesn't break on VS stop
            } );
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
        /// Sets the end point.
        /// </summary>
        private void SetRequestType( )
        {
            try
            {
                switch( _requestType )
                {
                    case API.Assistants:
                    {
                        PopulateAssistantModels( );
                        _endpoint = GptEndPoint.Assistants;
                        break;
                    }
                    case API.ChatCompletion:
                    {
                        PopulateCompletionModels( );
                        _endpoint = GptEndPoint.Completions;
                        break;
                    }
                    case API.TextGeneration:
                    {
                        PopulateTextModels( );
                        _endpoint = GptEndPoint.TextGeneration;
                        break;
                    }
                    case API.Agents:
                    {
                        PopulateAgentModels(  );
                        _endpoint = GptEndPoint.Responses;
                        break;
                    }
                    case API.ImageGeneration:
                    {
                        PopulateImageGenerationModels( );
                        _endpoint = GptEndPoint.ImageGeneration;
                        break;
                    }
                    case API.Embeddings:
                    {
                        PopulateEmbeddingModels( );
                        _endpoint = GptEndPoint.Embeddings;
                        break;
                    }
                    case API.ImageEditing:
                    {
                        PopulateImageEditingModels(  );
                        _endpoint = GptEndPoint.ImageEditing;
                        break;
                    }
                    case API.VectorStores:
                    {
                        PopulateVectorStoreModels( );
                        _endpoint = GptEndPoint.VectorStores;
                        break;
                    }
                    case API.SpeechGeneration:
                    {
                        PopulateSpeechModels( );
                        PopulateOpenAiVoices( );
                        _endpoint = GptEndPoint.SpeechGeneration;
                        break;
                    }
                    case API.TextToSpeech:
                    {
                        PopulateTextToSpeechModels( );
                        PopulateOpenAiVoices( );
                        _endpoint = GptEndPoint.SpeechGeneration;
                        break;
                    }
                    case API.Translations:
                    {
                        PopulateTranslationModels( );
                        PopulateOpenAiVoices( );
                        _endpoint = GptEndPoint.Translations;
                        break;
                    }
                    case API.Transcriptions:
                    {
                        PopulateTranscriptionModels( );
                        PopulateOpenAiVoices( );
                        _endpoint = GptEndPoint.Transcriptions;
                        break;
                    }
                    case API.FineTuning:
                    {
                        PopulateFineTuningModels( );
                        _endpoint = GptEndPoint.FineTuning;
                        break;
                    }
                    case API.Responses:
                    {
                        PopulateResponseModels( );
                        _endpoint = GptEndPoint.Responses;
                        break;
                    }
                    case API.Files:
                    {
                        PopulateFileModels( );
                        _endpoint = GptEndPoint.Files;
                        break;
                    }
                    case API.Uploads:
                    {
                        PopulateUploadModels( );
                        _endpoint = GptEndPoint.Uploads;
                        break;
                    }
                    case API.Projects:
                    {
                        PopulateTextModels( );
                        _endpoint = GptEndPoint.Projects;
                        break;
                    }
                    default:
                    {
                        PopulateModelsAsync( );
                        _endpoint = GptEndPoint.Completions;
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
        /// Sets the user mode.
        /// </summary>
        private protected void SetTabControl( )
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
        /// Sets the GPT options.
        /// </summary>
        private protected void SetGptOptions( )
        {
            try
            {
                _options = _requestType switch
                {
                    API.ChatCompletion => new ChatOptions( ),
                    API.Responses => new ChatOptions( ),
                    API.Assistants => new AssistantOptions(  ),
                    API.Agents => new AssistantOptions( ),
                    API.TextGeneration => new TextOptions(  ),
                    API.ImageGeneration => new ImageOptions(  ),
                    API.Transcriptions => new TranscriptionOptions( ),
                    API.Translations => new TranslationOptions( ),
                    API.ImageEditing => new ImageOptions(  ),
                    API.Files => new FileOptions( ),
                    API.Embeddings => new EmbeddingOptions(  ),
                    API.FineTuning => new FineTuningOptions(  ),
                    API.VectorStores => new VectorOptions( ),
                    API.SpeechGeneration => new SpeechOptions( ),
                    var _ => new TextOptions( )
                };
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Stops the active tab.
        /// </summary>
        private void StopActiveTab( )
        {
            Browser.Stop( );
        }

        /// <summary>
        /// Called by LoadURL to set the form URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        private void SetUrl( string url )
        {
            try
            {
                ThrowIf.Null( url, nameof( url ) );
                _originalUrl = url;
                _finalUrl = CleanUrl( url );
                UrlPanelTextBox.Text = _finalUrl;
                Browser.LoadUrl( _finalUrl );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sets the tab title.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <param name="text">The text.</param>
        private void SetTabText( ChromiumWebBrowser browser, string text )
        {
            try
            {
                ThrowIf.Null( browser, nameof( browser ) );
                ThrowIf.Null( text, nameof( text ) );
                text = text.Trim( );
                if( IsBlank( text ) )
                {
                    text = "New Tab";
                }

                browser.Tag = text;
                if( text.Length > 20 )
                {
                    var _title = text.Substring( 0, 20 ) + "...";
                    var _tab = ( BrowserTabItem )browser.Parent;
                    _tab.Title = _title;
                }
                else 
                {
                    var _tab = ( BrowserTabItem )browser.Parent;
                    _tab.Title = text;
                }

                // if current tab
                if( browser == Browser )
                {
                    SetTitleText( text );
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
                    ToolStripTextBox.Visibility = Visibility.Visible;
                    ToolStripSendButton.Visibility = Visibility.Visible;
                    ToolStripRefreshButton.Visibility = Visibility.Visible;
                    ToolStripTextBox.Visibility = Visibility.Visible;
                    ToolStripCancelButton.Visibility = Visibility.Visible;
                    ToolStripFileButton.Visibility = Visibility.Visible;
                    ToolStripDeleteButton.Visibility = Visibility.Visible;
                    ToolStripRemoveButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolStripTextBox.Visibility = Visibility.Hidden;
                    ToolStripSendButton.Visibility = Visibility.Hidden;
                    ToolStripRefreshButton.Visibility = Visibility.Hidden;
                    ToolStripCancelButton.Visibility = Visibility.Hidden;
                    ToolStripFileButton.Visibility = Visibility.Hidden;
                    ToolStripDeleteButton.Visibility = Visibility.Hidden;
                    ToolStripRemoveButton.Visibility = Visibility.Hidden;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sets the URL panel visibility.
        /// </summary>
        /// <param name="visible">
        /// if set to <c>true</c> [show].
        /// </param>
        private void SetUrlPanelVisibility( bool visible = true )
        {
            try
            {
                if( visible )
                {
                    UrlForwardButton.Visibility = Visibility.Visible;
                    UrlBackButton.Visibility = Visibility.Visible;
                    UrlCancelButton.Visibility = Visibility.Visible;
                    UrlHomeButton.Visibility = Visibility.Hidden;
                    UrlRefreshButton.Visibility = Visibility.Visible;
                }
                else
                {
                    UrlForwardButton.Visibility = Visibility.Hidden;
                    UrlBackButton.Visibility = Visibility.Hidden;
                    UrlCancelButton.Visibility = Visibility.Hidden;
                    UrlHomeButton.Visibility = Visibility.Visible;
                    UrlRefreshButton.Visibility = Visibility.Hidden;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sets the form title.
        /// </summary>
        /// <param name="title">
        /// Name of the tab.
        /// </param>
        private void SetTitleText( string title )
        {
            InvokeIf( ( ) =>
            {
                if( title.IsNull( ) )
                {
                    Title = Locations.Branding + " - " + title;
                    _currentTitle = title;
                }
                else
                {
                    Title = Locations.Branding;
                    _currentTitle = "New Tab";
                }
            } );
        }

        /// <summary>
        /// Toggles the fullscreen.
        /// </summary>
        private void ToggleFullscreen( )
        {
            if( !_fullScreen )
            {
                _oldWindowState = WindowState;
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = WindowState.Maximized;
                _fullScreen = true;
            }
            else
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
                WindowState = _oldWindowState;
                _fullScreen = false;
            }
        }

        /// <summary>
        /// Speeches to text.
        /// </summary>
        /// <param name="input">The input.</param>
        public void TextToSpeech( string input )
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

                if( VoicesDropDown.SelectedItem.ToString( ) != "" )
                {
                    _synthesizer.SelectVoice( VoicesDropDown.SelectedItem.ToString( ) );
                }

                _synthesizer.Speak( input );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Waits for browser to initialize.
        /// </summary>
        /// <param name="browser">The browser.</param>
        public void WaitForBrowserToInitialize( ChromiumWebBrowser browser )
        {
            while( !browser.IsBrowserInitialized )
            {
                Thread.Sleep( 100 );
            }
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.
        /// </param>
        private protected void OnLoad( object sender, RoutedEventArgs e )
        {
            PopulateModelsAsync( );
            PopulatePromptDropDown( );
            PopulateRequestTypes( );
            PopulateVoices( );
            InitializeCommands( );
            InitializePlotter( );
            InitializeHotkeys( );
            InitializeTimer( );
            InitializeToolStrip( );
            InitializeEditor( );
            App.ActiveWindows.Add( "ChatWindow", this );
            InitializeInterface( );
        }

        /// <summary>
        /// Called when [audio format selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnAudioFormatSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _audioFormat = ( ( MetroDropDownItem )AudioFormatDropDown.SelectedItem )
                        ?.Content?.ToString( );

                var _message = "AudioFormat = " + _audioFormat;
                SendNotification( _message );
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
                // ask user if they are sure
                if( DownloadsInProgress( ) )
                {
                    var _msg = "DownloadItems are in progress.";
                    SendMessage( _msg );
                }

                _timer?.Dispose( );
                _statusUpdate += null;
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
                Busy( );
                var _calculator = new CalculatorWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Owner = this,
                    Topmost = true
                };

                _calculator.Show( );
                Chill( );
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
                Busy( );
                WebMinion.RunChrome( );
                Chill( );
            }
            catch( Exception ex )
            {
                Fail( ex );
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
                Busy( );
                ClearChatControls( );
                ClearParameters( );
                ClearLabels( );
                PopulateModelsAsync( );
                PopulateInstalledVoices( );
                PopulateImageSizes( );
                Chill( );
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
                SendNotification( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [select
        /// ed document changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnDocumentListBoxSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( DocumentListBox.SelectedIndex != -1 )
                {
                    Busy( );
                    var _filepath = @"C:\Users\terry\source\repos\Bubba\Properties\Prompts.resx";
                    var _path = Locations.PathPrefix + @"Resources\Documents\Prompts\";
                    var _names = GetResourceNames( _filepath );
                    _document = ((MetroListBoxItem)DocumentListBox.SelectedItem).Tag.ToString( );
                    Editor.LoadFile( _document );
                    Chill( );
                    var _message = "Document = " + _document;
                    SendNotification( _message );
                }
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
                SendNotification( _message );
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
                Busy( );
                WebMinion.RunEdge( );
                Chill( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [reasoning effort selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnEffortSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _reasoningEffort = ((MetroDropDownItem)EffortDropDown.SelectedItem )
                    ?.Content?.ToString( );

                var _message = "ReasoningEffort = " + _reasoningEffort;
                SendNotification( _message );
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
                OpenGptFileDialog( );
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
                OpenGptFileDialog( );
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
                Busy( );
                WebMinion.RunFirefox( );
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
                SendNotification( _message );
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
        private protected void OnGoButtonClicked( object sender, RoutedEventArgs e )
        {
            try
            {
                _keyWords = ToolStripTextBox.InputText;
                if( !string.IsNullOrEmpty( _keyWords ) )
                {
                    var _message = "The search keywords are empty!";
                    SendNotification( _message );
                }
                else
                {
                    Busy( );
                    var _keywords = ToolStripTextBox.Text;
                    if( !string.IsNullOrEmpty( _keywords ) )
                    {
                        var _search = SearchEngineUrl + _keywords;
                        Browser.Load( _search );
                    }

                    Chill( );
                }
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
                SendNotification( _message );
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
                InitializeSpeechEngine( );
                var _msg = "The Speech Engine has been activated!";
                SendNotification( _msg );
            }
            else
            {
                _engine.RecognizeAsyncStop( );
            }
        }

        /// <summary>
        /// Called when [document selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnLanguageDropDownSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _language = ( ( MetroDropDownItem )LanguageDropDown.SelectedItem )
                    ?.Content?.ToString( );

                PopulateDocuments( );
                var _message = "Language = " + _language;
                SendNotification( _message );
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
                SendNotification( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [mouse move].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnMouseMove( object sender, MouseEventArgs e )
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
                SendNotification( _message );
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
        private void OnModelDropDownSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( ModelDropDown.SelectedIndex != -1 )
                {
                    _model = ( ( MetroDropDownItem )ModelDropDown.SelectedItem )
                        ?.Tag.ToString(  );

                    PopulateImageSizes( );
                    var _message = "Model = " + _model;
                    SendNotification( _message );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
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
                var _msg = "The GPT Audio Client has been activated!";
                SendNotification( _msg );
            }
        }

        /// <summary>
        /// Called when [next button click].
        /// </summary>
        /// 
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnNextButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _message = "NOT YET IMPLEMENTED!";
                SendNotification( _message );
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
                            {
                                var _temp = _textBox.Text;
                                var _value = double.Parse( _temp );
                                _textBox.Text = _value.ToString( "N2" );
                                break;
                            }
                            case "TopLogProbs":
                            case "Temperature":
                            case "TopPercent":
                            {
                                var _temp = _textBox.Text;
                                var _top = double.TryParse( _temp, out var _value );
                                if( _top )
                                {
                                    _textBox.Text = _value.ToString( "P2" );
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
        /// Called when [system prompt button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnPromptMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _name = ( (MetroListBoxItem)PromptDropDown.SelectedItem )
                    ?.Tag?.ToString( );

                var _message = "Prompt = " + _name;
                SendNotification( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [prompt drop down selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnPromptDropDownSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                var _name = ( ( MetroDropDownItem )PromptDropDown.SelectedItem )
                    ?.Tag?.ToString( );

                var _message = "Selected Prompt = " + _name;
                SendNotification( _message );
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
                SendNotification( _message );
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
            try
            {
                PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
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
        private void OnRequestDropDownSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( GenerationListBox.SelectedIndex != -1 )
                {
                    var _item = ( ( MetroDropDownItem )GenerationListBox.SelectedItem )
                        ?.Content?.ToString( );

                    _requestType = ( API )Enum.Parse( typeof( API ), _item?.Replace( " ", "" ) );
                    SetRequestType( );

                    var _message = "Request Type = " + _requestType;
                    SendNotification( _message );
                }
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
                ClearLabels( );
                PopulateModelsAsync( );
                PopulateInstalledVoices( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [response format selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnResponseFormatSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                var _image = ResponseFormatDropDown.SelectedValue.ToString( );
                var _message = "ResponseFormat = " + _responseFormat;
                SendNotification( _message );
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
                SendNotification( _message );
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
                if( StoreCheckBox.IsChecked == true )
                {
                    var _msg = "The Chats are now being stored!";
                    SendNotification( _msg );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
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
            if( Editor.Text != "" )
            {
                Editor.Text += "\n";
            }

            var _text = e.Result.Text;
            Editor.Text += _text;
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
                var _userInput = Editor.Text;
                if( string.IsNullOrEmpty( _userInput ) )
                {
                    MessageBox.Show( "Type in your question!" );
                    Editor.Focus( );
                    return;
                }

                if( Editor.Text != "" )
                {
                    Editor.AppendText( "\r\n" );
                }

                Editor.AppendText( _userInput + "\r\n" );
                Editor.Text = "";
                try
                {
                    var _answer = SendHttpMessage( _userInput ) + "";
                    Editor.AppendText( "Bubba: " + _answer.Replace( "\n", "\r\n" ).Trim( ) );
                    TextToSpeech( _answer );
                }
                catch( Exception ex )
                {
                    Editor.AppendText( "Error: " + ex.Message );
                }
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
                SendNotification( _message );
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
                if( StreamCheckBox.IsChecked == true )
                {
                    var _msg = "The Responses are now being streamed!";
                    SendNotification( _msg );
                }
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
                SendNotification( _message );
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
                SendNotification( _message );
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
                SetToolbarVisibility( !ToolStripTextBox.IsVisible );
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
                SendNotification( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [tool strip refresh button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnToolStripRefreshButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                ClearChatControls( );
                ClearParameters( );
                ClearLabels( );
                PopulateModelsAsync( );
                PopulateInstalledVoices( );
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
                var _textBox = sender as ToolStripTextBox;
                var _text = _textBox?.InputText;
                if( !string.IsNullOrEmpty( _text ) )
                {
                    ChatTextBox.Text = _text;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [tab control index changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnTabControlSelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            try
            {
                if( sender is MetroTabControl tabControl )
                {
                    var _index = tabControl.SelectedItem.Tag?.ToString( );
                    switch( _index )
                    {
                        case "Chat":
                        {
                            ActivateChatTab( );
                            break;
                        }
                        case "GPT":
                        {
                            ActivateParameterTab( );
                            break;
                        }
                        case "Web":
                        {
                            ActivateBrowserTab( );
                            break;
                        }
                        case "Editor":
                        {
                            ActivateEditorTab( );
                            break;
                        }
                        case "Image":
                        {
                            break;
                        }
                        case "Data":
                        {
                            ActivateDataTab( );
                            break;
                        }
                        case "Chart":
                        {
                            ActivateChartTab( );
                            break;
                        }
                        case "Graph":
                        {
                            ActivateGraphTab( );
                            break;
                        }
                        case "Busy":
                        {
                            ActivateBusyTab( );
                            break;
                        }
                        default:
                        {
                            ActivateChatTab( );
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
        /// Called when [URL text box clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs" />
        /// instance containing the event data.</param>
        private protected void OnUrlTextBoxMouseEnter( object sender, MouseEventArgs e )
        {
            try
            {
                var _psn = e.GetPosition( this );
                var _x = _psn.X - 100.0;
                var _y = _psn.Y - 100.0;
                OpenSearchDialog( _x, _y );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [URL cancel button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnUrlCancelButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                if( Browser.IsLoading )
                {
                    Browser.Stop( );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [home button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// instance containing the event data.</param>
        private void OnUrlHomeButtonClick( object sender, EventArgs e )
        {
            Busy( );
            _originalUrl = Browser.Address;
            SetUrl( Locations.HomePage );
            Chill( );
        }

        /// <summary>
        /// Called when [home button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// instance containing the event data.</param>
        private void OnUrlRefreshButtonClick( object sender, EventArgs e )
        {
            Busy( );
            RefreshActiveTab( );
            Chill( );
        }

        /// <summary>
        /// Called when [URL back button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnUrlBackButtonClick( object sender, EventArgs e )
        {
            Busy( );
            Browser.Back( );
            Chill( );
        }

        /// <summary>
        /// Called when [URL forward button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnUrlForwardButtonClick( object sender, EventArgs e )
        {
            Busy( );
            Browser.Forward( );
            Chill( );
        }

        /// <summary>
        /// Called when [voice selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnVoiceSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _voice = ( ( MetroDropDownItem )VoicesDropDown.SelectedItem )
                    ?.Content
                    ?.ToString( );

                var _message = "Voice = " + _voice;
                SendNotification( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [guidance option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnImageQualitySelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _imageQuality = ( ( MetroDropDownItem )ImageQualityDropDown.SelectedItem )
                    ?.Content
                    ?.ToString( );

                var _message = "ImageQuality = " + _imageQuality;
                SendNotification( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [image size selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnImageSizeSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _imageSize = ( ( MetroDropDownItem )ImageSizeDropDown.SelectedItem )
                    ?.Content?.ToString( );

                var _message = "ImageSize = " + _imageSize;
                SendNotification( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [image detail selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnImageDetailSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _imageDetail = ( ( MetroDropDownItem )ImageDetailDropDown.SelectedItem)
                    ?.Content?.ToString( );

                var _message = "ImageDetail = " + _imageDetail;
                SendNotification( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [image background selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private protected void OnImageBackgroundSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _imageBackground = ( ( MetroDropDownItem )ImageBackgroundDropDown.SelectedItem )
                    ?.Content?.ToString( );

                var _message = "ImageBackground = " + _imageBackground;
                SendNotification( _message );
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
        private protected void OnImageFormatSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( ImageFormatDropDown.SelectedIndex != -1 )
                {
                    _imageFormat = ( ( MetroDropDownItem )ImageFormatDropDown.SelectedItem )
                        ?.Content?.ToString( );

                    var _message = "Image Format = " + _imageFormat;
                    SendNotification( _message );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [image style selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnImageStyleSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( ImageStyleDropDown.SelectedIndex != -1 )
                {
                    _imageStyle = ( ( MetroDropDownItem )ImageStyleDropDown.SelectedItem )
                        ?.Content?.ToString( );

                    var _message = "Image Style = " + _imageFormat;
                    SendNotification( _message );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [loading state changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnWebBrowserLoadingStateChanged( object sender, LoadingStateChangedEventArgs e )
        {
            if( e.IsLoading )
            {
                Busy( );
            }
            else if( !e.IsLoading )
            {
                Chill( );
            }
        }

        /// <summary>
        /// Called when [WebBrowser frame load end].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CefSharp.FrameLoadEndEventArgs"/>
        /// instance containing the event data.</param>
        private void OnWebBrowserFrameLoadEnd( object sender, FrameLoadEndEventArgs e )
        {
            try
            {
                // Do something after page loads
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// 
        /// Called when [load error].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LoadErrorEventArgs" />
        /// instance containing the event data.</param>
        private void OnWebBrowserLoadError( object sender, LoadErrorEventArgs e )
        {
            // ("Load Error:" + e.ErrorCode + ";" + e.ErrorText);
        }

        /// <summary>
        /// Called when [title changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TitleChangedEventArgs" />
        /// instance containing the event data.</param>
        private void OnWebBrowserTitleChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            InvokeIf( ( ) =>
            {
                var _browser = ( ChromiumWebBrowser )sender;
                SetTabText( _browser, _browser.Title );
            } );
        }

        /// <summary>
        /// Called when [URL changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="AddressChangedEventArgs" />
        /// instance containing the event data.</param>
        private void OnWebBrowserAddressChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            Busy( );
            _originalUrl = Browser.Address;
            InvokeIf( ( ) =>
            {
                // if current tab
                if( sender == Browser
                    && e.Property.Name.Equals( "Address" ) )
                {
                    if( !NetUtility.IsFocused( UrlPanelTextBox ) )
                    {
                        _finalUrl = e.NewValue.ToString( );
                        SetUrl( _finalUrl );
                        SetUrlPanelVisibility( Browser.IsLoading );
                        SetTabText( ( ChromiumWebBrowser )sender, "Loading..." );
                        _currentTab.DateCreated = DateTime.Now;
                    }
                }
            } );

            Chill( );
        }

        /// <summary>
        /// Called when [browser mouse left button down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/>
        /// instance containing the event data.</param>
        private void OnBrowserMouseLeftButtonDown( object sender, MouseButtonEventArgs e )
        {
            var point = e.GetPosition( Browser );
            if( _region.IsVisible( ( float )point.X, ( float )point.Y ) )
            {
                var window = Window.GetWindow( this );
                window.DragMove( );
                e.Handled = true;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and
        /// - optionally - managed resources.
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
        /// 
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