﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-15-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-15-2025
// ******************************************************************************************
// <copyright file="GptFileDialog.xaml.cs" company="Terry D. Eppler">
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
//   GptFileDialog.xaml.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.CompilerServices;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using Syncfusion.SfSkinManager;
    using HorizontalAlignment = System.Windows.HorizontalAlignment;
    using RadioButton = System.Windows.Controls.RadioButton;
    using Timer = System.Threading.Timer;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for GptFileDialog.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    public partial class GptFileDialog : Window, IDisposable, INotifyPropertyChanged
    {
        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The count
        /// </summary>
        private protected int _count;

        /// <summary>
        /// The data
        /// </summary>
        private protected string _data;

        /// <summary>
        /// The duration
        /// </summary>
        private protected double _duration;

        /// <summary>
        /// The extension
        /// </summary>
        private protected EXT _extension;

        /// <summary>
        /// The file extension
        /// </summary>
        private protected string _fileExtension;

        /// <summary>
        /// The file paths
        /// </summary>
        private protected IList<string> _filePaths;

        /// <summary>
        /// The image
        /// </summary>
        private protected BitmapImage _image;

        /// <summary>
        /// The initial directory
        /// </summary>
        private protected string _initialDirectory;

        /// <summary>
        /// The initial dir paths
        /// </summary>
        private protected IList<string> _initialPaths;

        /// <summary>
        /// The locked object
        /// </summary>
        private protected object _entry = new object( );

        /// <summary>
        /// The radio buttons
        /// </summary>
        private protected IList<RadioButton> _radioButtons;

        /// <summary>
        /// The seconds
        /// </summary>
        private protected int _seconds;

        /// <summary>
        /// The selected file
        /// </summary>
        private protected string _selectedFile;

        /// <summary>
        /// The selected path
        /// </summary>
        private protected string _selectedPath;

        /// <summary>
        /// The status update
        /// </summary>
        private protected Action _statusUpdate;

        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// The time
        /// </summary>
        private protected int _time;

        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// The timer
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <inheritdoc />
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The HTTP client
        /// </summary>
        private protected HttpClient _httpClient;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptFileDialog" /> class.
        /// </summary>
        public GptFileDialog( )
        {
            // Theme Properties
            SfSkinManager.SetTheme( this, new Theme( "FluentDark", App.Controls ) );

            // Wwindow Initialization
            InitializeComponent( );
            InitializeDelegates( );
            RegisterCallbacks( );

            // Basic Properties
            Width = 700;
            Height = 480;
            ResizeMode = _theme.SizeMode;
            FontFamily = _theme.FontFamily;
            FontSize = _theme.FontSize;
            WindowStyle = _theme.WindowStyle;
            Padding = _theme.Padding;
            Margin = new Thickness( 1 );
            BorderThickness = _theme.BorderThickness;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            WindowStyle = WindowStyle.None;
            Background = _theme.ControlBackground;
            Foreground = _theme.LightBlueBrush;
            BorderBrush = _theme.BlueBorderBrush;
            _filePaths = new List<string>( );

            // Timer Properties
            _time = 0;
            _seconds = 5;

            // Event Wiring
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Gets or sets the selected path.
        /// </summary>
        /// <value>
        /// The selected path.
        /// </value>
        public string SelectedPath
        {
            get
            {
                return _selectedPath;
            }
            private protected set
            {
                _selectedPath = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected file.
        /// </summary>
        /// <value>
        /// The selected file.
        /// </value>
        public string SelectedFile
        {
            get
            {
                return _selectedFile;
            }
            private protected set
            {
                _selectedFile = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value>
        /// <c> true </c>
        /// if this instance is busy; otherwise,
        /// <c> false </c>
        /// </value>
        public bool IsBusy
        {
            get
            {
                lock( _entry )
                {
                    return _busy;
                }
            }
        }

        /// <summary>
        /// Initializes the callbacks.
        /// </summary>
        private void RegisterCallbacks( )
        {
            try
            {
                CloseButton.Click += OnCloseButtonClick;
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
        /// Initializes the labels.
        /// </summary>
        private void InitializeLabels( )
        {
            try
            {
                CountLabel.Content = "Files:";
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
        /// Initializes the buttons.
        /// </summary>
        private void InitializeButtons( )
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
        /// Clears the callbacks.
        /// </summary>
        private void ClearCallbacks( )
        {
            try
            {
                CloseButton.Click -= OnCloseButtonClick;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Clears the delegates.
        /// </summary>
        private void ClearDelegates( )
        {
            try
            {
                _timerCallback += UpdateStatus;
            }
            catch( Exception ex )
            {
                Fail( ex );
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
                var _notify = new Notification( message )
                {
                    Owner = this
                };

                _notify.Show( );
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
        /// Updates the status.
        /// </summary>
        private void UpdateStatus( )
        {
            try
            {
                TimeLabel.Content = DateTime.Now.ToShortTimeString( );
                DateLabel.Content = DateTime.Now.ToShortDateString( );
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
                Dispatcher.BeginInvoke( _statusUpdate );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sets the RadioButton events.
        /// </summary>
        private protected void RegisterRadioButtonEvents( )
        {
            try
            {
                foreach( var _checkBox in _radioButtons )
                {
                    _checkBox.Click += OnRadioButtonSelected;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Gets the radio buttons.
        /// </summary>
        /// <returns>
        /// List( MetroRadioButton )
        /// </returns>
        private protected virtual IList<RadioButton> GetRadioButtons( )
        {
            try
            {
                var _list = new List<RadioButton>
                {
                    PdfRadioButton,
                    PyRadioButton,
                    CRadioButton,
                    CppRadioButton,
                    HtmlRadioButton,
                    CssRadioButton,
                    DocRadioButton,
                    DocxRadioButton,
                    MdRadioButton,
                    JsRadioButton,
                    JsonRadioButton
                };

                return _list?.Any( ) == true
                    ? _list
                    : default( IList<RadioButton> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<RadioButton> );
            }
        }

        /// <summary>
        /// Populates the ListBox.
        /// </summary>
        private protected void PopulateListBox( )
        {
            try
            {
                ListBox.Items?.Clear( );
                if( _filePaths?.Any( ) == true )
                {
                    foreach( var _item in _filePaths )
                    {
                        ListBox.Items.Add( _item );
                    }
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Populates the ListBox.
        /// </summary>
        /// <param name="filePaths">The file paths.</param>
        private protected void PopulateListBox( IEnumerable<string> filePaths )
        {
            try
            {
                ThrowIf.Null( filePaths, nameof( filePaths ) );
                ListBox.Items?.Clear( );
                var _paths = filePaths.ToArray( );
                for( var _i = 0; _i < _paths.Length; _i++ )
                {
                    var _item = _paths[ _i ];
                    if( !string.IsNullOrEmpty( _item ) )
                    {
                        ListBox?.Items?.Add( _item );
                    }
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Sets the models.
        /// </summary>
        private async void PopulateFilesAsync( )
        {
            try
            {
                Busy( );
                var _watch = new Stopwatch( );
                _watch.Start( );
                var _url = "https://api.openai.com/v1/files";
                _httpClient = new HttpClient( );
                _httpClient.Timeout = new TimeSpan( 0, 0, 3 );
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue( "Bearer", App.OpenAiKey );

                var _responseMessage = await _httpClient.GetAsync( _url );
                _responseMessage.EnsureSuccessStatusCode( );
                var _body = await _responseMessage.Content.ReadAsStringAsync( );
                using var _document = JsonDocument.Parse( _body );
                var _root = _document.RootElement;
                _filePaths.Clear( );
                if( _root.TryGetProperty( "data", out var _files )
                    && _files.ValueKind == JsonValueKind.Array )
                {
                    foreach( var _item in _files.EnumerateArray( ) )
                    {
                        if( _item.TryGetProperty( "purpose", out var _purposeElement ) )
                        {
                            var _purpose = _purposeElement.ToString( );
                            if( _purpose.Equals( "assistants" )
                                && _item.TryGetProperty( "filename", out var _name ) )
                            {
                                _filePaths.Add( _name.GetString( ) );
                            }
                        }
                    }
                }

                Chill( );
                _watch.Stop( );
                _count = _filePaths.Count;
                FileCountLabel.Content = $"{_count:N0}";
                _duration = _watch.Elapsed.TotalMilliseconds;
                DurationLabel.Content = $"{_duration:N0} ms";
                Dispatcher.BeginInvoke( ( ) =>
                {
                    ListBox.Items?.Clear( );
                    for( var _index = 0; _index < _filePaths.Count; _index++ )
                    {
                        var _fileName = _filePaths[_index];
                        var _file = _fileName.Split( "." )[ 0 ];
                        ListBox.Items?.Add( _file );
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
        /// Gets the ListView paths.
        /// </summary>
        /// <returns></returns>
        private protected void CreateListViewFilePaths( )
        {
            try
            {
                Busy( );
                var _watch = new Stopwatch( );
                _filePaths?.Clear( );
                var _pattern = "*." + _fileExtension;
                for( var _i = 0; _i < _initialPaths.Count; _i++ )
                {
                    var _dirPath = _initialPaths[ _i ];
                    var _parent = Directory.CreateDirectory( _dirPath );
                    var _folders = _parent.GetDirectories( )
                        ?.Where( s => s.Name.Contains( "My" ) == false )?.Select( s => s.FullName )
                        ?.ToList( );

                    var _topLevelFiles = _parent.GetFiles( _pattern, SearchOption.TopDirectoryOnly )
                        ?.Select( f => f.FullName )?.ToArray( );

                    _filePaths.AddRange( _topLevelFiles );
                    for( var _k = 0; _k < _folders.Count; _k++ )
                    {
                        var _folder = Directory.CreateDirectory( _folders[ _k ] );
                        var _lowerLevelFiles = _folder
                            .GetFiles( _pattern, SearchOption.AllDirectories )
                            ?.Select( s => s.FullName )?.ToArray( );

                        _filePaths.AddRange( _lowerLevelFiles );
                    }
                }

                Chill( );
                _watch.Stop( );
                _count = _filePaths?.Count ?? 0;
                CountLabel.Content = $"Files:";
                FileCountLabel.Content = $"{_count:N0}";
                _duration = _watch.Elapsed.TotalMilliseconds;
                DurationLabel.Content = $"{_duration:N0} ms";
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Gets the file paths.
        /// </summary>
        /// <returns></returns>
        private protected IList<string> GetFilePaths( )
        {
            try
            {
                Busy( );
                var _watch = new Stopwatch( );
                _watch.Start( );
                var _list = new List<string>( );
                var _pattern = "*." + _fileExtension;
                for( var _e = 0; _e < _initialPaths.Count; _e++ )
                {
                    var _dirPath = _initialPaths[ _e ];
                    var _parent = Directory.CreateDirectory( _dirPath );
                    var _folders = _parent.GetDirectories( )
                        ?.Where( s => s.Name.StartsWith( "My" ) == false )
                        ?.Select( s => s.FullName )
                        ?.ToList( );

                    var _topLevelFiles = _parent.GetFiles( _pattern, SearchOption.TopDirectoryOnly )
                        ?.Select( f => f.FullName )
                        ?.ToArray( );

                    _list.AddRange( _topLevelFiles );
                    for( var _k = 0; _k < _folders.Count; _k++ )
                    {
                        var _folder = Directory.CreateDirectory( _folders[ _k ] );
                        var _lowerLevelFiles = _folder
                            .GetFiles( _pattern, SearchOption.AllDirectories )
                            ?.Select( s => s.FullName )
                            ?.ToArray( );

                        _list.AddRange( _lowerLevelFiles );
                    }
                }

                Chill( );
                _watch.Stop( );
                _count = _list.Count;
                CountLabel.Content = $"{_count:N0}";
                _duration = _watch.Elapsed.TotalMilliseconds;
                DurationLabel.Content = $"{_duration:N0}";
                return _list;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the file paths asynchronous.
        /// </summary>
        /// <returns></returns>
        private protected async Task<IList<string>> GetFilesAsync( )
        {
            try
            {
                Busy( );
                var _url = GptEndPoint.Files;
                _httpClient = new HttpClient( );
                _httpClient.Timeout = new TimeSpan( 0, 0, 3 );
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue( "Bearer", App.OpenAiKey );

                var _list = new List<string>( );
                var _responseMessage = await _httpClient.GetAsync( _url );
                _responseMessage.EnsureSuccessStatusCode( );
                var _body = await _responseMessage.Content.ReadAsStringAsync( );
                using var _document = JsonDocument.Parse( _body );
                var _root = _document.RootElement;
                if( _root.TryGetProperty( "data", out var _files )
                    && _files.ValueKind == JsonValueKind.Array )
                {
                    foreach( var _item in _files.EnumerateArray( ) )
                    {
                        if( _item.TryGetProperty( "purpose", out var _purposeElement ) )
                        {
                            var _purpose = _purposeElement.ToString( );
                            if( _purpose.Equals( "assistants" )
                                && _item.TryGetProperty( "filename", out var _name ) )
                            {
                                _list.Add( _name.GetString( ) );
                            }
                        }
                    }

                    FileCountLabel.Content = $"{_list.Count:N0}";
                }

                Chill( );
                return _list?.Any( ) == true
                    ? _list
                    : default( IList<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the initial dir paths.
        /// </summary>
        /// <returns>
        /// IList(string)
        /// </returns>
        private protected IList<string> CreateInitialDirectoryPaths( )
        {
            try
            {
                var _current = Environment.CurrentDirectory;
                var _list = new List<string>
                {
                    Environment.GetFolderPath( Environment.SpecialFolder.DesktopDirectory ),
                    Environment.GetFolderPath( Environment.SpecialFolder.Personal ),
                    Environment.GetFolderPath( Environment.SpecialFolder.Recent ),
                    Environment.CurrentDirectory,
                    _current
                };

                return _list?.Any( ) == true
                    ? _list
                    : default( IList<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Called when [load].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnLoaded( object sender, EventArgs e )
        {
            try
            {
                PdfRadioButton.IsChecked = true;
                _radioButtons = GetRadioButtons( );
                InitializeTimer( );
                PopulateFilesAsync( );
                InitializeLabels( );
                InitializeButtons( );
                RegisterRadioButtonEvents( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [RadioButton selected].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name = "e" > </param>
        private protected void OnRadioButtonSelected( object sender, EventArgs e )
        {
            try
            {
                Busy( );
                var _watch = new Stopwatch( );
                _watch.Start( );
                var _radioButton = sender as MetroRadioButton;
                _fileExtension = _radioButton?.Tag?.ToString( );
                if( !string.IsNullOrEmpty( _fileExtension ) )
                {
                    _extension = ( EXT )Enum.Parse( typeof( EXT ), _fileExtension.ToUpper( ) );
                }

                _filePaths = GetFilePaths( );
                _count = _filePaths.Count;
                _duration = _watch.ElapsedMilliseconds;
                CountLabel.Content = $"{_count:N0}";
                DurationLabel.Content = $"{_duration:N0}";
                PopulateListBox( _filePaths );
                Chill( );
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
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnClosing( object sender, CancelEventArgs e )
        {
            try
            {
                SfSkinManager.Dispose( this );
                ClearDelegates( );
                _timer?.Dispose( );
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
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Hide();
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
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
            }
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