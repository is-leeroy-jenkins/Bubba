// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-27-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-27-2024
// ******************************************************************************************
// <copyright file="BubbaWindow.xaml.cs" company="Terry D. Eppler">
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
//   BubbaWindow.xaml.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Web;
    using System.Reflection;
    using System.Windows;
    using System.Threading.Tasks;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.CompilerServices;
    using System.Speech.Recognition;
    using System.Speech.Synthesis;
    using System.Text.Json;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Input;
    using CefSharp;
    using CefSharp.Wpf;
    using Syncfusion.SfSkinManager;
    using Syncfusion.Windows.Controls.Input;
    using Syncfusion.Windows.Tools.Controls;
    using ToastNotifications;
    using ToastNotifications.Lifetime;
    using ToastNotifications.Position;
    using static System.Configuration.ConfigurationManager;
    using Application = System.Windows.Forms.Application;
    using BrowserSettings = CefSharp.Core.BrowserSettings;
    using Button = System.Windows.Controls.Button;
    using Cef = CefSharp.Core.Cef;
    using Clipboard = System.Windows.Clipboard;
    using KeyEventArgs = System.Windows.Input.KeyEventArgs;
    using MessageBox = System.Windows.MessageBox;
    using MouseEventArgs = System.Windows.Input.MouseEventArgs;
    using PrintDialog = System.Windows.Controls.PrintDialog;
    using TextDataFormat = System.Windows.TextDataFormat;
    using Timer = System.Threading.Timer;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Window" />
    /// <seealso cref="T:System.Windows.Markup.IComponentConnector" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    public partial class BubbaWindow : Window, INotifyPropertyChanged, IDisposable
    {
        private const string KEY = "sk-proj-eTIELWTlG8lKT3hpqgq7a3vmB6lBVKo"
            + "GBlkoHhu0KqWqsnxyRq9bpEz0N1xZKcOxlyDbLJFCu"
            + "YT3BlbkFJiQGzglbEgyZB7O9FsBi4bJTO0WEg-"
            + "xddgKbywZr1o4bbn0HtNQlSU3OALS0pfMuifvMcy2XPAA";

        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The entry
        /// </summary>
        private protected object _entry = new object( );

        /// <summary>
        /// The provider
        /// </summary>
        private protected Provider _provider;

        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// The HTTP client
        /// </summary>
        private protected HttpClient _httpClient;

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

        /// <summary>
        /// The cancel requests
        /// </summary>
        private protected List<int> _cancelRequests;

        /// <summary>
        /// The current title
        /// </summary>
        private string _currentTitle;

        /// <summary>
        /// The final URL
        /// </summary>
        private protected string _finalUrl;

        /// <summary>
        /// The full screen
        /// </summary>
        private protected bool _fullScreen;

        /// <summary>
        /// The last search
        /// </summary>
        private string _lastSearch = "";

        /// <summary>
        /// The old window state
        /// </summary>
        private protected WindowState _oldWindowState;

        /// <summary>
        /// The original URL
        /// </summary>
        private protected string _originalUrl;

        /// <summary>
        /// The path
        /// </summary>
        private string _path = Path.GetDirectoryName( Application.ExecutablePath ) + @"\";

        /// <summary>
        /// The search engine URL
        /// </summary>
        private protected string _searchEngineUrl;

        /// <summary>
        /// The is search open
        /// </summary>
        private protected bool _isSearchOpen;

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
        /// The browser callback
        /// </summary>
        private protected Func<ChromiumWebBrowser> _browserCallback;

        /// <summary>
        /// The context menu callback
        /// </summary>
        private protected ContextMenuCallback _contextMenuCallback;

        /// <summary>
        /// The life span callback
        /// </summary>
        private protected LifeSpanCallback _lifeSpanCallback;

        /// <summary>
        /// The keyboard callback
        /// </summary>
        private protected KeyboardCallback _keyboardCallback;

        /// <summary>
        /// The download callback
        /// </summary>
        private protected DownloadCallback _downloadCallback;

        /// <summary>
        /// The request callback
        /// </summary>
        private protected RequestCallback _requestCallback;

        /// <summary>
        /// The timer callback
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <summary>
        /// The status update
        /// </summary>
        private protected Action _statusUpdate;

        /// <summary>
        /// The host callback
        /// </summary>
        private protected HostCallback _hostCallback;

        /// <summary>
        /// The download names
        /// </summary>
        private protected Dictionary<int, string> _downloadNames;

        /// <summary>
        /// The download items
        /// </summary>
        private protected Dictionary<int, DownloadItem> _downloadItems;

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
        /// The new tab item
        /// </summary>
        private protected BrowserTabItem _newTabItem;

        /// <summary>
        /// The current tab
        /// </summary>
        private protected BrowserTabItem _currentTab;

        /// <summary>
        /// The current browser
        /// </summary>
        private protected ChromiumWebBrowser _currentBrowser;

        /// <summary>
        /// The time label
        /// </summary>
        public MetroLabel TimeLabel;

        /// <summary>
        /// The tool strip forward button
        /// </summary>
        public ToolStripButton ToolStripForwardButton;

        /// <summary>
        /// The instance
        /// </summary>
        public static BubbaWindow Instance;

        /// <summary>
        /// The assembly
        /// </summary>
        public static Assembly Assembly;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.BubbaWindow" /> class.
        /// </summary>
        public BubbaWindow( )
        {
            // Theme Properties
            SfSkinManager.SetTheme(this, new Theme("FluentDark", App.Controls));

            // Window Initialization
            Instance = this;
            InitializeComponent( );
            RegisterCallbacks( );
            InitializeBrowser( );
            InitializeDelegates( );
            InitializeToolStrip( );
            InitializeTabControl( );

            // GPT Hyperparameters
            _temperature = 1.0;
            _maximumTokens = 2048;

            // Event Wiring
            Loaded += OnLoad;
            Closing += OnClosing;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="MainWindow"/> is store.
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
        /// <see cref="MainWindow"/> is stream.
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
        /// Gets or sets the host callback.
        /// </summary>
        /// <value>
        /// The host callback.
        /// </value>
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
        /// Gets or sets the download names.
        /// </summary>
        /// <value>
        /// The download names.
        /// </value>
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
        /// Gets or sets the cancel requests.
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
        /// Gets or sets the download items.
        /// </summary>
        /// <value>
        /// The download items.
        /// </value>
        public Dictionary<int, DownloadItem> DownloadItems
        {
            get
            {
                return _downloadItems;
            }
            set
            {
                _downloadItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the search engine URL.
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
                _searchEngineUrl = value;
            }
        }

        /// <summary>
        /// Gets the duration of the loading.
        /// </summary>
        /// <value>
        /// The duration of the loading.
        /// </value>
        public int LoadingDuration
        {
            get
            {
                if( TabControl.SelectedItem != null
                    && ( ( BrowserTabItem )TabControl.SelectedItem ).Tag != null )
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
        /// Gets or sets the current tab.
        /// </summary>
        /// <value>
        /// The current tab.
        /// </value>
        public BrowserTabItem CurrentTab
        {
            get
            {
                return TabControl.SelectedItem != null
                    && ( ( BrowserTabItem )TabControl.SelectedItem ).Tag != null
                        ? ( BrowserTabItem )TabControl.SelectedItem
                        : default( BrowserTabItem );
            }
            set
            {
                _currentTab = value;
            }
        }

        /// <summary>
        /// Gets or sets the current browser.
        /// </summary>
        /// <value>
        /// The current browser.
        /// </value>
        public ChromiumWebBrowser CurrentBrowser
        {
            get
            {
                return TabControl.SelectedItem != null
                    && ( ( BrowserTabItem )TabControl.SelectedItem ).Tag != null
                        ? ( ( BrowserTabItem )TabControl.SelectedItem ).Browser
                        : default( ChromiumWebBrowser );
            }
            set
            {
                _currentBrowser = value;
            }
        }

        /// <summary>
        /// Initializes the browser.
        /// </summary>
        private void InitializeBrowser( )
        {
            _hostCallback = new HostCallback( this );
            ConfigureBrowser( Browser );
            _currentTab = BrowserTab;
            _currentBrowser = Browser;
            _searchEngineUrl = Browser.Address;
        }

        /// <summary>
        /// Initializes the downloads.
        /// </summary>
        private void InitializeDownloads( )
        {
            _downloadItems = new Dictionary<int, DownloadItem>( );
            _downloadNames = new Dictionary<int, string>( );
            _cancelRequests = new List<int>( );
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
        /// Initializes the tab control.
        /// </summary>
        private void InitializeTabControl( )
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
        /// Initializes the PictureBox.
        /// </summary>
        private void InitializePictureBox( )
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
        /// Initializes the buttons.
        /// </summary>
        private void InitializeButtons( )
        {
            try
            {
                SearchPanelForwardButton.Visibility = Visibility.Hidden;
                SearchPanelBackButton.Visibility = Visibility.Hidden;
                SearchPanelCancelButton.Visibility = Visibility.Hidden;
            }
            catch( Exception ex )
            {
                Fail( ex );
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
        /// Initializes the hotkeys.
        /// </summary>
        private void InitializeHotkeys( )
        {
            try
            {
                // browser hot keys
                KeyboardCallback.AddHotKey( this, CloseActiveTab, Keys.W, true );
                KeyboardCallback.AddHotKey( this, CloseActiveTab, Keys.Escape, true );
                KeyboardCallback.AddHotKey( this, AddBlankWindow, Keys.N, true );
                KeyboardCallback.AddHotKey( this, AddBlankTab, Keys.T, true );
                KeyboardCallback.AddHotKey( this, RefreshActiveTab, Keys.F5 );
                KeyboardCallback.AddHotKey( this, OpenDeveloperTools, Keys.F12 );
                KeyboardCallback.AddHotKey( this, NextTab, Keys.Tab, true );
                KeyboardCallback.AddHotKey( this, PreviousTab, Keys.Tab, true,
                    true );

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
        /// Registers the callbacks.
        /// </summary>
        private protected void RegisterCallbacks( )
        {
            try
            {
                SearchPanelCancelButton.MouseLeftButtonDown += OnCloseButtonClick;
                UrlTextBox.GotMouseCapture += OnUrlTextBoxClick;
                ToolStripTextBox.GotMouseCapture += OnToolStripTextBoxClick;
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
        /// Clears the callbacks.
        /// </summary>
        private protected virtual void ClearCallbacks( )
        {
            try
            {
                _lifeSpanCallback = null;
                _keyboardCallback = null;
                _downloadCallback = null;
                _requestCallback = null;
                _timerCallback = null;
                _hostCallback = null;
                _timerCallback = null;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Configures the browser.
        /// </summary>
        /// <param name="browser">The browser.</param>
        private void ConfigureBrowser( ChromiumWebBrowser browser )
        {
            try
            {
                ThrowIf.Null( browser, nameof( browser ) );
                var _config = new BrowserSettings( );
                _config.WebGl = bool.Parse( AppSettings[ "WebGL" ] ).ToCefState( );
                browser.BrowserSettings = _config;
            }
            catch( Exception ex )
            {
                Fail( ex );
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
        /// Sets the tab text.
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
                if( browser == _currentBrowser )
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
        /// Sets the title text.
        /// </summary>
        /// <param name="title">The title.</param>
        private void SetTitleText( string title )
        {
            InvokeIf( ( ) =>
            {
                if( title.CheckIfValid( ) )
                {
                    Title = AppSettings[ "Branding" ] + " - " + title;
                    _currentTitle = title;
                }
                else
                {
                    Title = AppSettings[ "Branding" ];
                    _currentTitle = "New Tab";
                }
            } );
        }

        /// <summary>
        /// Sets the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        private void SetUrl( string url )
        {
            try
            {
                ThrowIf.Null( url, nameof( url ) );
                _originalUrl = url;
                _finalUrl = CleanUrl( url );
                UrlTextBox.Text = _finalUrl;
                _currentTab.CurrentUrl = _originalUrl;
                CloseSearch( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Creates the notifier.
        /// </summary>
        /// <returns></returns>
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
                    cfg.Dispatcher = CurrentBrowser.Dispatcher;
                } );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( Notifier );
            }
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
        private void SendNotification( )
        {
            try
            {
                var _message = "THIS IS NOT YET IMPLEMENTED!";
                var _notify = new Notification( _message );
                _notify.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Finds the text on page.
        /// </summary>
        /// <param name="next">if set to <c>true</c> [next].</param>
        private void FindTextOnPage( bool next = true )
        {
            var _first = _lastSearch != UrlTextBox.Text;
            _lastSearch = UrlTextBox.Text;
            if( _lastSearch.CheckIfValid( ) )
            {
                _currentBrowser.GetBrowser( )?.Find( _lastSearch, true, false, !_first );
            }
            else
            {
                _currentBrowser.GetBrowser( )?.StopFinding( true );
            }

            UrlTextBox.Focus( );
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
        /// Gets the tabs.
        /// </summary>
        /// <returns></returns>
        public List<BrowserTabItem> GetTabs( )
        {
            var _tabs = new List<BrowserTabItem>( );
            foreach( BrowserTabItem _tabPage in TabControl.Items )
            {
                if( _tabPage.Tag != null )
                {
                    _tabs.Add( ( BrowserTabItem )_tabPage.Tag );
                }
            }

            return _tabs;
        }

        /// <summary>
        /// Gets the tab by browser.
        /// </summary>
        /// <param name="browser">The browser.</param>
        /// <returns></returns>
        public BrowserTabItem GetTabByBrowser( IWebBrowser browser )
        {
            foreach( BrowserTabItem _item in TabControl.Items )
            {
                var _tab = ( BrowserTabItem )_item.Tag;
                if( _tab != null
                    && _tab.Browser == browser )
                {
                    return _tab;
                }
            }

            return null;
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
                    ? _winXpDir + AppSettings[ "Branding" ] + @"\" + name + @"\"
                    : @"C:\ProgramData\" + AppSettings[ "Branding" ] + @"\" + name + @"\";
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
        /// Gets the browser asynchronous.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="focused">if set to <c>true</c> [focused].</param>
        /// <returns></returns>
        private Task<ChromiumWebBrowser> GetBrowserAsync( string url, bool focused )
        {
            var _tcs = new TaskCompletionSource<ChromiumWebBrowser>( );
            try
            {
                ThrowIf.Null( url, nameof( url ) );
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
        /// Loads the URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        private void LoadUrl( string url )
        {
            try
            {
                ThrowIf.Null( url, nameof( url ) );
                var _newUrl = url;
                var _urlLower = url.Trim( ).ToLower( );
                SetTabText( _currentBrowser, "Loading..." );
                if( _urlLower == "localhost" )
                {
                    _newUrl = "http://localhost/";
                }
                else if( url.CheckIfFilePath( )
                    || url.CheckIfFilePath2( ) )
                {
                    _newUrl = url.PathToUrl( );
                }
                else
                {
                    Uri.TryCreate( url, UriKind.Absolute, out var _outUri );
                    if( !( _urlLower.StartsWith( "http" )
                        || _urlLower.StartsWith( AppSettings[ "Internal" ] ) ) )
                    {
                        if( _outUri == null
                            || _outUri.Scheme != Uri.UriSchemeFile )
                        {
                            _newUrl = "https://" + url;
                        }
                    }

                    if( _urlLower.StartsWith( AppSettings[ "Internal" ] + ":" )
                        || ( Uri.TryCreate( _newUrl, UriKind.Absolute, out _outUri )
                            && ( ( ( _outUri.Scheme == Uri.UriSchemeHttp
                                        || _outUri.Scheme == Uri.UriSchemeHttps )
                                    && _newUrl.Contains( "." ) )
                                || _outUri.Scheme == Uri.UriSchemeFile ) ) )
                    {
                    }
                    else
                    {
                        _newUrl = AppSettings[ "Google" ] + HttpUtility.UrlEncode( url );
                    }
                }

                _currentBrowser.Load( _newUrl );
                SetUrl( _newUrl );
                EnableBackButton( true );
                EnableForwardButton( false );
            }
            catch( Exception e )
            {
                Fail( e );
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
        /// Enables the back button.
        /// </summary>
        /// <param name="canGoBack">if set to <c>true</c> [can go back].</param>
        private void EnableBackButton( bool canGoBack )
        {
            InvokeIf( ( ) =>
            {
                if( canGoBack )
                {
                    PreviousButton.Visibility = Visibility.Visible;
                    SearchPanelBackButton.Visibility = Visibility.Visible;
                }
                else
                {
                    PreviousButton.Visibility = Visibility.Hidden;
                    SearchPanelBackButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the forward button.
        /// </summary>
        /// <param name="canGoForward">if set to <c>true</c> [can go forward].</param>
        private void EnableForwardButton( bool canGoForward )
        {
            InvokeIf( ( ) =>
            {
                if( canGoForward )
                {
                    NextButton.Visibility = Visibility.Visible;
                    SearchPanelForwardButton.Visibility = Visibility.Visible;
                }
                else
                {
                    NextButton.Visibility = Visibility.Hidden;
                    SearchPanelForwardButton.Visibility = Visibility.Hidden;
                }
            } );
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
        /// Determines whether the specified URL is blank.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        ///   <c>true</c> if the specified URL is blank; otherwise, <c>false</c>.
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
        ///   <c>true</c> if [is blank or system] [the specified URL]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsBlankOrSystem( string url )
        {
            try
            {
                ThrowIf.Null( url, nameof( url ) );
                return url == "" || url.BeginsWith( "about:" ) || url.BeginsWith( "chrome:" )
                    || url.BeginsWith( AppSettings[ "Internal" ] + ":" );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is first tab].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is first tab]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsFirstTab( )
        {
            return TabControl.SelectedItem == TabControl.Items[ 0 ];
        }

        /// <summary>
        /// Determines whether [is last tab].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is last tab]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsLastTab( )
        {
            return TabControl.SelectedItem == TabControl.Items[ ^2 ];
        }

        /// <summary>
        /// Stops the active tab.
        /// </summary>
        private void StopActiveTab( )
        {
            _currentBrowser.Stop( );
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
        /// Previouses the tab.
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
        /// Adds the blank window.
        /// </summary>
        public void AddBlankWindow( )
        {
            // open a new instance of the browser
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
        /// Adds the blank tab.
        /// </summary>
        public void AddBlankTab( )
        {
            AddNewBrowserTab( "" );
            Dispatcher.BeginInvoke( ( ) =>
            {
                UrlTextBox.Focus( );
            } );
        }

        /// <summary>
        /// Adds the new browser tab.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="focus">if set to <c>true</c> [focus].</param>
        /// <param name="referringUrl">The referring URL.</param>
        /// <returns></returns>
        public ChromiumWebBrowser AddNewBrowserTab( string url, bool focus = true,
            string referringUrl = null )
        {
            return Dispatcher.Invoke( delegate
            {
                // check if already exists
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

                var _tab = new BrowserTabItem( );
                _tab.Title = "New Tab";
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
        /// <param name="tabItem">The tab item.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private BrowserTabItem AddNewBrowser( BrowserTabItem tabItem, string url )
        {
            if( url == "" )
            {
                url = AppSettings[ "NewTab" ];
            }

            var _newBrowser = new ChromiumWebBrowser( url );
            ConfigureBrowser( _newBrowser );
            TabControl.Items.Add( _newBrowser );
            _newBrowser.BringIntoView( );
            tabItem.Content = _newBrowser;
            _newBrowser.StatusMessage += OnStatusUpdated;
            _newBrowser.LoadingStateChanged += OnLoadingStateChanged;
            _newBrowser.TitleChanged += OnTitleChanged;
            _newBrowser.LoadError += OnLoadError;
            _newBrowser.AddressChanged += OnAddressChanged;
            _newBrowser.DownloadHandler = _downloadCallback;
            _newBrowser.MenuHandler = _contextMenuCallback;
            _newBrowser.LifeSpanHandler = _lifeSpanCallback;
            _newBrowser.KeyboardHandler = _keyboardCallback;
            _newBrowser.RequestHandler = _requestCallback;
            var _tab = new BrowserTabItem
            {
                IsOpen = true,
                Browser = _newBrowser,
                Tab = tabItem,
                OriginalUrl = url,
                CurrentUrl = url,
                Title = "New Tab",
                DateCreated = DateTime.Now
            };

            tabItem.Tag = _tab;
            if( url.StartsWith( AppSettings[ "Internal" ] + ":" ) )
            {
                _newBrowser.JavascriptObjectRepository.Register( "host", HostCallback,
                    BindingOptions.DefaultBinder );
            }

            return _tab;
        }

        /// <summary>
        /// Searches the government domains.
        /// </summary>
        /// <param name="keyWords">The key words.</param>
        private void SearchGovernmentDomains( string keyWords )
        {
            if( !string.IsNullOrEmpty( keyWords ) )
            {
                try
                {
                    var _google = AppSettings[ "Google" ] + keyWords;
                    _currentBrowser.LoadUrl( _google );
                    var _epa = AppSettings[ "EPA" ] + keyWords;
                    AddNewBrowserTab( _epa, false );
                    var _data = AppSettings[ "DATA" ] + keyWords;
                    AddNewBrowserTab( _data, false );
                    var _crs = AppSettings[ "CRS" ] + keyWords;
                    AddNewBrowserTab( _crs, false );
                    var _loc = AppSettings[ "LOC" ] + keyWords;
                    AddNewBrowserTab( _loc, false );
                    var _gpo = AppSettings[ "GPO" ] + keyWords;
                    AddNewBrowserTab( _gpo, false );
                    var _usgi = AppSettings[ "USGI" ] + keyWords;
                    AddNewBrowserTab( _usgi, false );
                    var _omb = AppSettings[ "OMB" ] + keyWords;
                    AddNewBrowserTab( _omb, false );
                    var _ust = AppSettings[ "UST" ] + keyWords;
                    AddNewBrowserTab( _ust, false );
                    var _nara = AppSettings[ "NARA" ] + keyWords;
                    AddNewBrowserTab( _nara, false );
                    var _nasa = AppSettings[ "NASA" ] + keyWords;
                    AddNewBrowserTab( _nasa, false );
                    var _noaa = AppSettings[ "NOAA" ] + keyWords;
                    AddNewBrowserTab( _noaa, false );
                    var _doi = AppSettings[ "DOI" ] + keyWords;
                    AddNewBrowserTab( _doi, false );
                    var _nps = AppSettings[ "NPS" ] + keyWords;
                    AddNewBrowserTab( _nps, false );
                    var _gsa = AppSettings[ "GSA" ] + keyWords;
                    AddNewBrowserTab( _gsa, false );
                    var _doc = AppSettings[ "DOC" ] + keyWords;
                    AddNewBrowserTab( _doc, false );
                    var _hhs = AppSettings[ "HHS" ] + keyWords;
                    AddNewBrowserTab( _hhs, false );
                    var _nrc = AppSettings[ "NRC" ] + keyWords;
                    AddNewBrowserTab( _nrc, false );
                    var _doe = AppSettings[ "DOE" ] + keyWords;
                    AddNewBrowserTab( _doe, false );
                    var _nsf = AppSettings[ "NSF" ] + keyWords;
                    AddNewBrowserTab( _nsf, false );
                    var _usda = AppSettings[ "USDA" ] + keyWords;
                    AddNewBrowserTab( _usda, false );
                    var _csb = AppSettings[ "CSB" ] + keyWords;
                    AddNewBrowserTab( _csb, false );
                    var _irs = AppSettings[ "IRS" ] + keyWords;
                    AddNewBrowserTab( _irs, false );
                    var _fda = AppSettings[ "FDA" ] + keyWords;
                    AddNewBrowserTab( _fda, false );
                    var _cdc = AppSettings[ "CDC" ] + keyWords;
                    AddNewBrowserTab( _cdc, false );
                    var _ace = AppSettings[ "ACE" ] + keyWords;
                    AddNewBrowserTab( _ace, false );
                    var _dhs = AppSettings[ "DHS" ] + keyWords;
                    AddNewBrowserTab( _dhs, false );
                    var _dod = AppSettings[ "DOD" ] + keyWords;
                    AddNewBrowserTab( _dod, false );
                    var _usno = AppSettings[ "USNO" ] + keyWords;
                    AddNewBrowserTab( _usno, false );
                    var _nws = AppSettings[ "NWS" ] + keyWords;
                    AddNewBrowserTab( _nws, false );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
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
        /// Downloadses the in progress.
        /// </summary>
        /// <returns></returns>
        public bool DownloadsInProgress( )
        {
            if( _downloadItems?.Values?.Count > 0 )
            {
                foreach( var _item in _downloadItems.Values )
                {
                    if( _item.IsInProgress )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Refreshes the active tab.
        /// </summary>
        public void RefreshActiveTab( )
        {
            var _address = _currentBrowser.Address;
            _currentBrowser.Load( _address );
        }

        /// <summary>
        /// Closes the active tab.
        /// </summary>
        public void CloseActiveTab( )
        {
            if( _currentTab != null/* && TabPages.Items.Count > 2*/ )
            {
                var _index = TabControl.Items.IndexOf( TabControl.SelectedItem );
                TabControl.Items.RemoveAt( _index );
                if( TabControl.Items.Count - 1 > _index )
                {
                    TabControl.SelectedItem = TabControl.Items[ _index ];
                }
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
                var _brw = AddNewBrowserTab( AppSettings[ "Downloads" ] );
                _downloadStrip = ( BrowserTabItem )_brw.Parent;
            }
        }

        /// <summary>
        /// Opens the developer tools.
        /// </summary>
        private void OpenDeveloperTools( )
        {
            if( _currentBrowser == null )
            {
                var _message = "CurrentBrowser is null!";
                SendMessage( _message );
            }
            else
            {
                _currentBrowser.ShowDevTools( );
            }
        }

        /// <summary>
        /// Opens the chrome browser.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private void OpenChromeBrowser( string args )
        {
            try
            {
                var _url = AppSettings[ "Google" ] + args;
                var _info = new ProcessStartInfo
                {
                    FileName = AppSettings[ "Chrome" ],
                    LoadUserProfile = true,
                    UseShellExecute = true
                };

                _info.ArgumentList.Add( _url );
                Process.Start( _info );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the chrome browser.
        /// </summary>
        private void OpenChromeBrowser( )
        {
            try
            {
                var _url = AppSettings[ "HomePage" ];
                var _info = new ProcessStartInfo
                {
                    FileName = AppSettings[ "Chrome" ],
                    LoadUserProfile = true,
                    UseShellExecute = true
                };

                _info.ArgumentList.Add( _url );
                Process.Start( _info );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the edge browser.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private void OpenEdgeBrowser( string args )
        {
            try
            {
                var _url = AppSettings[ "Google" ] + args;
                var _info = new ProcessStartInfo
                {
                    FileName = AppSettings[ "Edge" ],
                    LoadUserProfile = true,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                _info.ArgumentList.Add( _url );
                Process.Start( _info );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the edge browser.
        /// </summary>
        private void OpenEdgeBrowser( )
        {
            try
            {
                var _url = AppSettings[ "HomePage" ];
                var _info = new ProcessStartInfo
                {
                    FileName = AppSettings[ "Edge" ],
                    LoadUserProfile = true,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                _info.ArgumentList.Add( _url );
                Process.Start( _info );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the fire fox browser.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private void OpenFireFoxBrowser( string args )
        {
            try
            {
                var _url = AppSettings[ "Google" ] + args;
                var _info = new ProcessStartInfo
                {
                    FileName = AppSettings[ "FireFox" ],
                    LoadUserProfile = true,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                _info.ArgumentList.Add( _url );
                Process.Start( _info );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the fire fox browser.
        /// </summary>
        private void OpenFireFoxBrowser( )
        {
            try
            {
                var _url = AppSettings[ "HomePage" ];
                var _info = new ProcessStartInfo
                {
                    FileName = AppSettings[ "FireFox" ],
                    LoadUserProfile = true,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                _info.ArgumentList.Add( _url );
                Process.Start( _info );
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
            if( !_isSearchOpen )
            {
                _isSearchOpen = true;
                InvokeIf( ( ) =>
                {
                    UrlTextBox.Text = _lastSearch;
                    UrlTextBox.Focus( );
                    UrlTextBox.SelectAll( );
                } );
            }
            else
            {
                InvokeIf( ( ) =>
                {
                    UrlTextBox.Focus( );
                    UrlTextBox.SelectAll( );
                } );
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
                    //SearchPanel.Visible = false;
                    _currentBrowser.GetBrowser( )?.StopFinding( true );
                } );
            }
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
        /// Clears the controls.
        /// </summary>
        private void ClearChatControls( )
        {
            try
            {
                UserTextBox.Text = "1";
                PresenceSlider.Value = 0.0;
                TemperatureSlider.Value = 1.0;
                FrequencySlider.Value = 0.0;
                PercentSlider.Value = 0.0;
                MaxTokensTextBox.Text = "2048";
                SystemTextBox.Text = "";
                UserTextBox.Text = "";
                ToolStripTextBox.Text = "";
            }
            catch(Exception ex)
            {
                Fail(ex);
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
                var _searchDialog = new SearchDialog( );
                _searchDialog.Owner = this;
                _searchDialog.Left = x;
                _searchDialog.Top = y;
                _searchDialog.Show( );
                _searchDialog.SearchPanelTextBox.Focus( );
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
                _httpClient.Timeout = new TimeSpan( 0, 0, 3 );
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

                _aiModels.Sort( );
                ModelComboBox.Items.Clear( );
                Dispatcher.BeginInvoke( ( ) =>
                {
                    foreach( var _model in _aiModels )
                    {
                        ModelComboBox.Items.Add( _model );
                    }
                } );

                ModelComboBox.SelectedIndex = 0;
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

        /// <summary>
        /// Sets the hyper parameters.
        /// </summary>
        private protected void SetHyperParameters( )
        {
            try
            {
                _store = StoreCheckBox.IsChecked ?? false;
                _stream = StreamCheckBox.IsChecked ?? false;
                _presence = PresenceSlider.Value;
                _temperature = TemperatureSlider.Value;
                _topPercent = PercentSlider.Value;
                _frequency = FrequencySlider.Value;
                _number = int.Parse( NumberTextBox.Text );
                _maximumTokens = int.Parse( MaxTokensTextBox.Text );
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
                StatusLabel.Content = DateTime.Now.ToLongTimeString();
                DateLabel.Content = DateTime.Now.ToShortDateString();
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
        /// Sets the search panel visibility.
        /// </summary>
        /// <param name="visible">
        /// if set to <c>true</c> [visible].</param>
        private void SetSearchPanelVisibility( bool visible = true )
        {
            try
            {
                if( visible )
                {
                    UrlTextBox.Visibility = Visibility.Visible;
                    SearchPanelForwardButton.Visibility = Visibility.Visible;
                    SearchPanelBackButton.Visibility = Visibility.Visible;
                    SearchPanelCancelButton.Visibility = Visibility.Visible;
                }
                else
                {
                    UrlTextBox.Visibility = Visibility.Hidden;
                    SearchPanelForwardButton.Visibility = Visibility.Hidden;
                    SearchPanelBackButton.Visibility = Visibility.Hidden;
                    SearchPanelCancelButton.Visibility = Visibility.Hidden;
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
                InitializeTimer( );
                InitializePictureBox( );
                PopulateDomainDropDowns( );
                InitializeHotkeys( );
                InitializeButtons( );
                InitializeTitle( );
                InitializeToolStrip( );
                _searchEngineUrl = AppSettings[ "Google" ];
                SetSearchPanelVisibility( false );
                TabControl.SelectedIndex = 0;
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
        /// Called when [frame loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="FrameLoadEndEventArgs"/>
        /// instance containing the event data.</param>
        private void OnFrameLoaded( object sender, FrameLoadEndEventArgs e )
        {
            // Do something after the page loads
        }

        /// <summary>
        /// Called when [address changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnAddressChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            InvokeIf( ( ) =>
            {
                // if current tab
                if( sender == _currentBrowser
                    && e.Property.Name.Equals( "Address" ) )
                {
                    if( !WebUtils.IsFocused( UrlTextBox ) )
                    {
                        var _url = e.NewValue.ToString( );
                        SetUrl( _url );
                    }

                    EnableBackButton( _currentBrowser.CanGoBack );
                    EnableForwardButton( _currentBrowser.CanGoForward );
                    SetTabText( ( ChromiumWebBrowser )sender, "Loading..." );
                    RefreshButton.Visibility = Visibility.Hidden;
                    SearchPanelCancelButton.Visibility = Visibility.Visible;
                    _currentTab.DateCreated = DateTime.Now;
                }
            } );
        }

        /// <summary>
        /// Called when [load error].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LoadErrorEventArgs"/>
        /// instance containing the event data.</param>
        private void OnLoadError( object sender, LoadErrorEventArgs e )
        {
            // ("Load Error:" + e.ErrorCode + ";" + e.ErrorText);
        }

        /// <summary>
        /// Called when [title changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnTitleChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            InvokeIf( ( ) =>
            {
                var _browser = ( ChromiumWebBrowser )sender;
                SetTabText( _browser, _browser.Title );
            } );
        }

        /// <summary>
        /// Called when [developer tools button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnDeveloperToolsButtonClick( object sender, RoutedEventArgs e )
        {
            OpenDeveloperTools( );
        }

        /// <summary>
        /// Called when [loading state changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LoadingStateChangedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnLoadingStateChanged( object sender, LoadingStateChangedEventArgs e )
        {
            if( sender == _currentBrowser )
            {
                EnableBackButton( e.CanGoBack );
                EnableForwardButton( e.CanGoForward );
                if( e.IsLoading )
                {
                    // set title
                    SetTitleText( "Loading..." );
                }
                else
                {
                    // after loaded / stopped
                    InvokeIf( ( ) =>
                    {
                        RefreshButton.Visibility = Visibility.Visible;
                        SearchPanelCancelButton.Visibility = Visibility.Hidden;
                    } );
                }
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
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [tab closing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TabClosingEventArgs"/>
        /// instance containing the event data.</param>
        private void OnTabClosing( object sender, TabClosingEventArgs e )
        {
            // exit if invalid tab
            if( _currentTab == null )
            {
                e.Cancel = true;
                return;
            }

            // add a blank tab if the very last tab is closed!
            if( TabControl.Items.Count <= 2 )
            {
                AddBlankTab( );

                //e.Cancel = true;
            }
        }

        /// <summary>
        /// Called when [tabs changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="BrowserTabChangedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnTabsChanged( object sender, BrowserTabChangedEventArgs e )
        {
            ChromiumWebBrowser _browser = null;
            try
            {
                _browser = e.Item.Browser;
            }
            catch( Exception )
            {
                // ignore 
            }

            if( e.ChangeType == ChangeType.SelectionChanged
                && sender is BrowserTabItem _tabItem )
            {
                if( _tabItem.Browser == _currentBrowser )
                {
                    AddBlankTab( );
                }
                else
                {
                    _browser = _currentBrowser;
                    SetUrl( _browser.Address );
                    SetTitleText( _browser.Tag.ToString( ) ?? "New Tab" );
                    EnableBackButton( _browser.CanGoBack );
                    EnableForwardButton( _browser.CanGoForward );
                }
            }

            if( e.ChangeType == ChangeType.Removed )
            {
                if( e.Item == _downloadStrip )
                {
                    _downloadStrip = null;
                }

                _browser?.Dispose( );
            }

            if( e.ChangeType == ChangeType.Changed )
            {
                if( _browser != null )
                {
                    if( _originalUrl != "about:blank" )
                    {
                        _browser.Focus( );
                    }
                }
            }
        }

        /// <summary>
        /// Called when [selected domain changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSelectedDomainChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( sender is ToolStripComboBox _comboBox )
                {
                    var _index = _comboBox.SelectedIndex;
                    _searchEngineUrl = _index switch
                    {
                        0 => AppSettings[ "EPA" ],
                        1 => AppSettings[ "DATA" ],
                        2 => AppSettings[ "GPO" ],
                        3 => AppSettings[ "USGI" ],
                        4 => AppSettings[ "CRS" ],
                        5 => AppSettings[ "LOC" ],
                        6 => AppSettings[ "OMB" ],
                        7 => AppSettings[ "UST" ],
                        8 => AppSettings[ "NASA" ],
                        9 => AppSettings[ "NOAA" ],
                        10 => AppSettings[ "DOI" ],
                        11 => AppSettings[ "NPS" ],
                        12 => AppSettings[ "GSA" ],
                        13 => AppSettings[ "NARA" ],
                        14 => AppSettings[ "DOC" ],
                        15 => AppSettings[ "HHS" ],
                        16 => AppSettings[ "NRC" ],
                        17 => AppSettings[ "DOE" ],
                        18 => AppSettings[ "NSF" ],
                        19 => AppSettings[ "USDA" ],
                        20 => AppSettings[ "CSB" ],
                        21 => AppSettings[ "IRS" ],
                        22 => AppSettings[ "FDA" ],
                        23 => AppSettings[ "CDC" ],
                        24 => AppSettings[ "ACE" ],
                        25 => AppSettings[ "DHS" ],
                        26 => AppSettings[ "DOD" ],
                        27 => AppSettings[ "USNO" ],
                        28 => AppSettings[ "NWS" ],
                        var _ => AppSettings[ "Google" ]
                    };
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [menu close click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnMenuCloseClick( object sender, RoutedEventArgs e )
        {
            CloseActiveTab( );
        }

        /// <summary>
        /// Called when [close other tabs click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnCloseOtherTabsClick( object sender, RoutedEventArgs e )
        {
            CloseOtherTabs( );
        }

        /// <summary>
        /// Called when [back button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnBackButtonClick( object sender, RoutedEventArgs e )
        {
            _currentBrowser.Back( );
        }

        /// <summary>
        /// Called when [forward button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnForwardButtonClick( object sender, RoutedEventArgs e )
        {
            _currentBrowser.Forward( );
        }

        /// <summary>
        /// Called when [downloads button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnDownloadsButtonClick( object sender, RoutedEventArgs e )
        {
            AddNewBrowserTab( AppSettings[ "Downloads" ] );
        }

        /// <summary>
        /// Called when [refresh button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnRefreshButtonClick( object sender, RoutedEventArgs e )
        {
            RefreshActiveTab( );
        }

        /// <summary>
        /// Called when [stop button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnStopButtonClick( object sender, RoutedEventArgs e )
        {
            StopActiveTab( );
        }

        /// <summary>
        /// Called when [search button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSearchButtonClick( object sender, MouseButtonEventArgs e )
        {
            try
            {
                var _search = new SearchDialog( );
                var _width = Width / 3 + ( int )( _search.Width * .66 );
                var _heigth = Height / 15;
                _search.Owner = this;
                var _pos = e.GetPosition( this );
                _search.PointToScreen( new Point( _pos.X, _pos.Y ) );
                _search.Show( );
                _search.SearchPanelTextBox.Focus( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [search dialog closing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnSearchDialogClosing( object sender, EventArgs e )
        {
            if( sender is SearchDialog _dialog )
            {
                try
                {
                    var _search = _dialog.Results;
                    _currentBrowser.Load( _search );
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
                finally
                {
                    _dialog.SearchPanelTextBox.Name = string.Empty;
                }
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
                var _searchDialog = new SearchDialog( );
                _searchDialog.Owner = this;
                _searchDialog.Left = _psn.X - 100;
                _searchDialog.Top = _psn.Y + 150;
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
        private protected void OnToolStripTextBoxClick( object sender, MouseEventArgs e )
        {
            try
            {
                var _psn = e.GetPosition( this );
                var _searchDialog = new SearchDialog( );
                _searchDialog.Owner = this;
                _searchDialog.Left = _psn.X;
                _searchDialog.Top = _psn.Y - 150;
                _searchDialog.Show( );
                _searchDialog.SearchPanelTextBox.Focus( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [tab pages click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/>
        /// instance containing the event data.</param>
        private void OnTabPagesClick( object sender, MouseButtonEventArgs e )
        {
            if( e.LeftButton == MouseButtonState.Pressed )
            {
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
            // ask user if they are sure
            if( DownloadsInProgress( ) )
            {
                var _msg = "DownloadItems are in progress.";
                SendMessage( _msg );
            }

            try
            {
                SfSkinManager.Dispose( this );
                foreach( BrowserTabItem _tab in TabControl.Items )
                {
                    _tab.Dispose( );
                }
            }
            catch( Exception )
            {
                // ignore exception
            }
        }

        /// <summary>
        /// Called when [search panel clear button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSearchPanelClearButtonClick( object sender, RoutedEventArgs e )
        {
            CloseSearch( );
        }

        /// <summary>
        /// Called when [search previous button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSearchPreviousButtonClick( object sender, RoutedEventArgs e )
        {
            FindTextOnPage( false );
        }

        /// <summary>
        /// Called when [search forward button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSearchForwardButtonClick( object sender, RoutedEventArgs e )
        {
            FindTextOnPage( true );
        }

        /// <summary>
        /// Called when [search panel text changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSearchPanelTextChanged( object sender, RoutedEventArgs e )
        {
            FindTextOnPage( true );
        }

        /// <summary>
        /// Called when [search key down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSearchKeyDown( object sender, KeyEventArgs e )
        {
            if( e.IsDown
                && sender is MetroTextBox )
            {
                FindTextOnPage( true );
            }
        }

        /// <summary>
        /// Called when [home button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnHomeButtonClick( object sender, RoutedEventArgs e )
        {
            _currentBrowser.Load( AppSettings[ "HomePage" ] );
        }

        /// <summary>
        /// Called when [go button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnGoButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _keywords = ToolStripTextBox.Text;
                if( !string.IsNullOrEmpty( _keywords )
                    && ToolStripComboBox.SelectedIndex == -1 )
                {
                    SearchGovernmentDomains( _keywords );
                }
                else if( !string.IsNullOrEmpty( _keywords )
                    && ToolStripComboBox.SelectedIndex > -1 )
                {
                    var _search = SearchEngineUrl + _keywords;
                    _currentBrowser.Load( _search );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
            finally
            {
                ToolStripTextBox.Text = string.Empty;
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
                Environment.Exit( 1 );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [fire fox button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnFireFoxButtonClick( object sender, RoutedEventArgs e )
        {
            ToolStripTextBox.SelectAll( );
            var _args = ToolStripTextBox.Text;
            if( !string.IsNullOrEmpty( _args ) )
            {
                OpenFireFoxBrowser( _args );
                ToolStripTextBox.Clear( );
                ToolStripComboBox.SelectedIndex = -1;
            }
            else
            {
                OpenFireFoxBrowser( );
            }
        }

        /// <summary>
        /// Called when [edge button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnEdgeButtonClick( object sender, RoutedEventArgs e )
        {
            ToolStripTextBox.SelectAll( );
            var _args = ToolStripTextBox.Text;
            if( !string.IsNullOrEmpty( _args ) )
            {
                OpenEdgeBrowser( _args );
                ToolStripTextBox.Clear( );
                ToolStripComboBox.SelectedIndex = -1;
            }
            else
            {
                OpenEdgeBrowser( );
            }
        }

        /// <summary>
        /// Called when [chrome button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnChromeButtonClick( object sender, RoutedEventArgs e )
        {
            ToolStripTextBox.SelectAll( );
            var _args = ToolStripTextBox.Text;
            if( !string.IsNullOrEmpty( _args ) )
            {
                OpenChromeBrowser( _args );
                ToolStripTextBox.Clear( );
                ToolStripComboBox.SelectedIndex = -1;
            }
            else
            {
                OpenChromeBrowser( );
            }
        }

        /// <summary>
        /// Called when [sharepoint button clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnSharepointButtonClicked( object sender, RoutedEventArgs e )
        {
            try
            {
                var _message = "THIS HAS NOT BEEN IMPLEMENTED";
                SendMessage( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [form closing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnFormClosing( object sender, RoutedEventArgs e )
        {
            try
            {
                _timer?.Dispose( );
                Opacity = 1;
                FadeOutAsync( this );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [activated].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnActivated( object sender, RoutedEventArgs e )
        {
            try
            {
                Opacity = 0;
                FadeInAsync( this );
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
                Application.Exit( );
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
                var _message = "NOT YET IMPLEMENTED!";
                SendMessage( _message );
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
            if( MuteCheckBox.IsChecked == true )
            {
                VoiceLabel.Visibility = Visibility.Hidden;
                VoiceComboBox.Visibility = Visibility.Hidden;
            }
            else
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
        /// Called when [text box input changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnTextBoxInputChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if(sender is MetroTextBox _textBox)
                {
                    var _tag = _textBox?.Tag.ToString();
                    if(!string.IsNullOrEmpty(_tag))
                    {
                        switch(_tag)
                        {
                            case "Frequency":
                            case "Presence":
                            case "Temperature":
                            {
                                var _temp = _textBox.Text;
                                var _value = double.Parse(_temp);
                                _textBox.Text = _value.ToString("N2");
                                break;
                            }
                            case "TopPercent":
                            {
                                var _temp = _textBox.Text;
                                var _top = double.TryParse(_temp, out var _value);
                                if(_top)
                                {
                                    _textBox.Text = _value.ToString("P1");
                                }

                                break;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Fail(ex);
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