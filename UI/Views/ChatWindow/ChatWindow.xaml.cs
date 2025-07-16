
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
    using System.Windows.Input;
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
        /// The last search
        /// </summary>
        private string _lastSearch = "";

        /// <summary>
        /// The old window state
        /// </summary>
        private protected WindowState _oldWindowState;

        /// <summary>
        /// The system prompt fro the GPT
        /// </summary>
        private protected string _instructions;

        /// <summary>
        /// The user prompt for the GPT
        /// </summary>
        private protected string _userPrompt;

        /// <summary>
        /// The assembly
        /// </summary>
        public static Assembly Assembly;

        /// <summary>
        /// The messages
        /// </summary>
        private protected ChatLog _messages;

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
        /// The chat model
        /// </summary>
        private protected string _model;

        /// <summary>
        /// The time
        /// </summary>
        private protected int _time;

        /// <summary>
        /// The number
        /// </summary>
        private protected int _number;

        /// <summary>
        /// The document
        /// </summary>
        private protected string _document;

        /// <summary>
        /// The user prompt for the GPT
        /// </summary>
        private protected string _inputText;

        /// <summary>
        /// The output format
        /// </summary>
        private protected string _outputFormat;

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
        /// The image compression
        /// </summary>
        private protected string _imageCompression;

        /// <summary>
        /// An upper bound for the number of tokens
        /// that can be generated for a completion
        /// </summary>
        private protected int _maximumTokens;

        /// <summary>
        /// The output tokens
        /// </summary>
        private protected int _outputTokens;

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
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// The timer callback
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <summary>
        /// The engine
        /// </summary>
        private protected SpeechRecognitionEngine _engine;

        /// <summary>
        /// The synthesizer
        /// </summary>
        private protected SpeechSynthesizer _synthesizer;

        /// <summary>
        /// The search dialog
        /// </summary>
        private protected SearchDialog _searchDialog;

        /// <summary>
        /// The status update
        /// </summary>
        private protected Action _statusUpdate;

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

            // Control Properties
            TemperatureSlider.Value = 0.8;
            TopPercentSlider.Value = 0.9;
            PresenceSlider.Value = 0.00;
            FrequencySlider.Value = 0.00;

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
            _instructions = Prompts.BudgetAnalyst;

            // Event Wiring
            Loaded += OnLoad;
            Closing += OnClosing;
        }

        /// <summary>
        /// Gets or sets the input text.
        /// </summary>
        /// <value>
        /// The input text.
        /// </value>
        public string InputText
        {
            get
            {
                return _inputText;
            }
            set
            {
                if( _inputText != value )
                {
                    _inputText = value;
                    OnPropertyChanged( nameof(InputText ) );
                }
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
                ToolStripMenuButton.Visibility = Visibility.Visible;
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
        /// Registers the callbacks.
        /// </summary>
        private protected void RegisterCallbacks( )
        {
            try
            {
                GenerationListBox.SelectionChanged += OnRequestDropDownSelectionChanged;
                ModelDropDown.SelectionChanged += OnModelDropDownSelectionChanged;
                ToolStripTextBox.TextChanged += OnToolStripTextBoxTextChanged;
                ToolStripTextBox.MouseDoubleClick += OnUrlTextBoxDoubleClick;
                ToolStripMenuButton.Click += OnToolStripToggleButtonClick;
                ToolStripResetButton.Click += OnToolStripResetButtonClick;
                ToolStripSaveButton.Click += OnToolStripSaveButtonClick;
                ToolStripCancelButton.Click += OnToolStripCancelButtonClick;
                ToolStripSendButton.Click += OnToolStripSendButtonClick;
                ToolStripEraseButton.Click += OnToolStripEraseButtonClick;
                ListenCheckBox.Checked += OnListenCheckedChanged;
                MuteCheckBox.Checked += OnMuteCheckedBoxChanged;
                StoreCheckBox.Checked += OnStoreCheckBoxChecked;
                StreamCheckBox.Checked += OnStreamCheckBoxChecked;
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
                SpeechRateDropDown.SelectionChanged += OnSpeechRateSelectionChanged;
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
                    _busy = true;
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
                _timerCallback = null;
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
                PopulateTextDocuments( );
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
        /// Enables the reset button.
        /// </summary>
        /// <param name="canReset">
        /// if set to <c>true</c> [can go back].</param>
        private void EnableResetButton( bool canReset )
        {
            InvokeIf( ( ) =>
            {
                if( canReset )
                {
                    ToolStripResetButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolStripResetButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the send button.
        /// </summary>
        /// <param name="canSend">
        /// if set to <c>true</c> [can send].</param>
        private void EnableSendButton( bool canSend )
        {
            InvokeIf( ( ) =>
            {
                if( canSend )
                {
                    ToolStripSendButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolStripSendButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the erase button.
        /// </summary>
        /// <param name="canErase">
        /// if set to <c>true</c> [can erase].</param>
        private void EnableEraseButton( bool canErase )
        {
            InvokeIf( ( ) =>
            {
                if( canErase )
                {
                    ToolStripEraseButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolStripEraseButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the cancel button.
        /// </summary>
        /// <param name="canCancel">
        /// if set to <c>true</c> [can cancel].</param>
        private void EnableCancelButton( bool canCancel )
        {
            InvokeIf( ( ) =>
            {
                if( canCancel )
                {
                    ToolStripCancelButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolStripCancelButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the delete button.
        /// </summary>
        /// <param name="canDelete">
        /// if set to <c>true</c> [can delete].</param>
        private void EnableDeleteButton( bool canDelete )
        {
            InvokeIf( ( ) =>
            {
                if( canDelete )
                {
                    ToolStripDeleteButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolStripDeleteButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the save button.
        /// </summary>
        /// <param name="canSave">
        /// if set to <c>true</c> [can go back].</param>
        private void EnableSaveButton( bool canSave )
        {
            InvokeIf( ( ) =>
            {
                if( canSave )
                {
                    ToolStripSaveButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolStripSaveButton.Visibility = Visibility.Hidden;
                }
            } );
        }

        /// <summary>
        /// Enables the upload button.
        /// </summary>
        /// <param name="canUpload">
        /// if set to <c>true</c> [can upload].</param>
        private void EnableUploadButton( bool canUpload )
        {
            InvokeIf( ( ) =>
            {
                if( canUpload )
                {
                    ToolStripUploadButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolStripUploadButton.Visibility = Visibility.Hidden;
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
        /// Shows the search dialog.
        /// </summary>
        private protected virtual void OpenSearchDialog( double x, double y )
        {
            try
            {
                _searchDialog = (SearchDialog)App.ActiveWindows[ "SearchDialog" ];
                _searchDialog.Owner = this;
                _searchDialog.Topmost = true;
                _searchDialog.Left = x;
                _searchDialog.Top = y + 100;
                _searchDialog.Show( );
                _searchDialog.SearchPanelTextBox.Focus(  );
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
                _httpClient.Timeout = new TimeSpan( 0, 0, 5 );
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
        /// Sets the user document language.
        /// </summary>
        private protected void PopulateDocuments( )
        {
            try
            {
                var _prefix = @"C:\Users\terry\source\repos\Bubba\Resources\Documents\Editor\";
                Editor.Text = "";
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
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the prompt drop down.
        /// </summary>
        private protected void PopulatePromptListBox( )
        {
            try
            {
                DocumentListBox.Items?.Clear( );
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

                    DocumentListBox.Items.Add( _item );
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
                    ToolStripResetButton.Visibility = Visibility.Visible;
                    ToolStripCancelButton.Visibility = Visibility.Visible;
                    ToolStripUploadButton.Visibility = Visibility.Visible;
                    ToolStripSaveButton.Visibility = Visibility.Visible;
                    ToolStripEraseButton.Visibility = Visibility.Visible;
                    ToolStripDeleteButton.Visibility = Visibility.Visible;
                }
                else
                {
                    ToolStripTextBox.Visibility = Visibility.Hidden;
                    ToolStripSendButton.Visibility = Visibility.Hidden;
                    ToolStripResetButton.Visibility = Visibility.Hidden;
                    ToolStripCancelButton.Visibility = Visibility.Hidden;
                    ToolStripSaveButton.Visibility = Visibility.Hidden;
                    ToolStripUploadButton.Visibility = Visibility.Hidden;
                    ToolStripEraseButton.Visibility = Visibility.Hidden;
                    ToolStripDeleteButton.Visibility = Visibility.Hidden;
                }
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
        /// Called when [load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.
        /// </param>
        private protected void OnLoad( object sender, RoutedEventArgs e )
        {
            PopulateModelsAsync( );
            PopulateRequestTypes( );
            PopulateVoices( );
            PopulateTextDocuments( );
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
                    ?.Content
                    ?.ToString( );

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
                    var _filepath = @"C:\Users\terry\source\repos\Bubba\Properties\Prompts.resx";
                    var _path = Locations.PathPrefix + @"Resources\Documents\Prompts\";
                    var _names = GetResourceNames( _filepath );
                    _document = ((MetroListBoxItem)DocumentListBox.SelectedItem ).Tag.ToString( );
                    Editor.LoadFile( _document );
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
                WebMinion.RunEdge( );
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
                _responseFormat = ResponseFormatDropDown
                    ?.SelectedValue
                    ?.ToString( );

                var _message = "ResponseFormat = " + _responseFormat;
                SendNotification( _message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [speech rate selection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnSpeechRateSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                var _rate = ( ( MetroDropDownItem )SpeechRateDropDown.SelectedItem )
                    ?.Content
                    ?.ToString( );

                _speed = int.Parse( _rate );

                var _message = "Speech Rate = " + _speed;
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnStatusUpdated( object sender, RoutedEventArgs e )
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
            try
            {
                if( Editor.Text != "" )
                {
                    Editor.Text += "\n";
                }

                var _text = e.Result.Text;
                Editor.Text += _text;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
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
            try
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
                var _answer = SendHttpMessage( _userInput ) + "";
                Editor.AppendText( "Bubba: " + _answer.Replace( "\n", "\r\n" ).Trim( ) );
                TextToSpeech( _answer );
            }
            catch( Exception ex )
            {
                Editor.AppendText( "Error: " + ex.Message );
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
        private void OnToolStripToggleButtonClick( object sender, RoutedEventArgs e )
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
        private void OnToolStripResetButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [URL text box clicked].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs" />
        /// instance containing the event data.</param>
        private protected void OnUrlTextBoxDoubleClick( object sender, MouseEventArgs e )
        {
            try
            {
                var _psn = e.GetPosition( this );
                var _x = _psn.X + 100.0;
                var _y = _psn.Y + 100.0;
                OpenSearchDialog( _x, _y );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [tool strip cancel button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnToolStripCancelButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [tool strip erase button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnToolStripEraseButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [tool strip upload button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnToolStripUploadButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [tool strip save button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnToolStripSaveButtonClick( object sender, RoutedEventArgs e )
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
        /// Called when [tool strip send button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnToolStripSendButtonClick( object sender, RoutedEventArgs e )
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
                    ?.Content
                    ?.ToString( );

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
                    ?.Content
                    ?.ToString( );

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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnImageBackgroundSelectionChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _imageBackground = ( ( MetroDropDownItem )ImageBackgroundDropDown.SelectedItem )
                    ?.Content
                    ?.ToString( );

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
                        ?.Content
                        ?.ToString( );

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
                        ?.Content
                        ?.ToString( );

                    var _message = "Image Style = " + _imageFormat;
                    SendNotification( _message );
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
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