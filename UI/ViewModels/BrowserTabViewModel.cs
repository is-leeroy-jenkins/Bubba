// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 07-01-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        07-01-2025
// ******************************************************************************************
// <copyright file="BrowserTabViewModel.cs" company="Terry D. Eppler">
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
//   BrowserTabViewModel.cs
// </summary>
// ******************************************************************************************
namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using CefSharp;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ConvertToAutoProperty" ) ]
    public class BrowserTabViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The address
        /// </summary>
        private string _address;

        /// <summary>
        /// The address editable
        /// </summary>
        private string _addressEditable;

        /// <summary>
        /// The output message
        /// </summary>
        private string _outputMessage;

        /// <summary>
        /// The status message
        /// </summary>
        private string _statusMessage;

        /// <summary>
        /// The title
        /// </summary>
        private string _title;

        /// <summary>
        /// The show download information
        /// </summary>
        private bool _showDownloadInfo;

        /// <summary>
        /// The web browser
        /// </summary>
        private IWpfWebBrowser _webBrowser;

        /// <summary>
        /// The evaluate java script result
        /// </summary>
        private object _evaluateJavaScriptResult;

        /// <summary>
        /// The show sidebar
        /// </summary>
        private bool _showSidebar;

        /// <summary>
        /// The legacy binding enabled
        /// </summary>
        private bool _legacyBindingEnabled;

        /// <summary>
        /// The last download action
        /// </summary>
        private string _lastDownloadAction;

        /// <summary>
        /// The download item
        /// </summary>
        private DownloadItem _downloadItem;

        /// <summary>
        /// The go command
        /// </summary>
        private ICommand _goCommand;

        /// <summary>
        /// The home command
        /// </summary>
        private ICommand _homeCommand;

        /// <summary>
        /// The execute java script command
        /// </summary>
        private ICommand _executeJavaScriptCommand;

        /// <summary>
        /// The evaluate java script command
        /// </summary>
        private ICommand _evaluateJavaScriptCommand;

        /// <summary>
        /// The show dev tools command
        /// </summary>
        private ICommand _showDevToolsCommand;

        /// <summary>
        /// The close dev tools command
        /// </summary>
        private ICommand _closeDevToolsCommand;

        /// <summary>
        /// The javascript binding stress test
        /// </summary>
        private ICommand _javascriptBindingStressTest;

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                Set( ref _address, value );
            }
        }

        /// <summary>
        /// Gets or sets the address editable.
        /// </summary>
        /// <value>
        /// The address editable.
        /// </value>
        public string AddressEditable
        {
            get
            {
                return _addressEditable;
            }
            set
            {
                Set( ref _addressEditable, value );
            }
        }

        /// <summary>
        /// Gets or sets the output message.
        /// </summary>
        /// <value>
        /// The output message.
        /// </value>
        public string OutputMessage
        {
            get
            {
                return _outputMessage;
            }
            set
            {
                Set( ref _outputMessage, value );
            }
        }

        /// <summary>
        /// Gets or sets the status message.
        /// </summary>
        /// <value>
        /// The status message.
        /// </value>
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                Set( ref _statusMessage, value );
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set( ref _title, value );
            }
        }

        /// <summary>
        /// Gets or sets the WebBrowser.
        /// </summary>
        /// <value>
        /// The WebBrowser.
        /// </value>
        public IWpfWebBrowser WebBrowser
        {
            get
            {
                return _webBrowser;
            }
            set
            {
                Set( ref _webBrowser, value );
            }
        }

        /// <summary>
        /// Gets or sets the evaluate java script result.
        /// </summary>
        /// <value>
        /// The evaluate java script result.
        /// </value>
        public object EvaluateJavaScriptResult
        {
            get
            {
                return _evaluateJavaScriptResult;
            }
            set
            {
                Set( ref _evaluateJavaScriptResult, value );
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show sidebar].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show sidebar]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowSidebar
        {
            get
            {
                return _showSidebar;
            }
            set
            {
                Set( ref _showSidebar, value );
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [show download information].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show download information]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowDownloadInfo
        {
            get
            {
                return _showDownloadInfo;
            }
            set
            {
                Set( ref _showDownloadInfo, value );
            }
        }

        /// <summary>
        /// Gets or sets the last download action.
        /// </summary>
        /// <value>
        /// The last download action.
        /// </value>
        public string LastDownloadAction
        {
            get
            {
                return _lastDownloadAction;
            }
            set
            {
                Set( ref _lastDownloadAction, value );
            }
        }

        /// <summary>
        /// Gets or sets the download item.
        /// </summary>
        /// <value>
        /// The download item.
        /// </value>
        public DownloadItem DownloadItem
        {
            get
            {
                return _downloadItem;
            }
            set
            {
                Set( ref _downloadItem, value );
            }
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether [legacy binding enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [legacy binding enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool LegacyBindingEnabled
        {
            get
            {
                return _legacyBindingEnabled;
            }
            set
            {
                Set( ref _legacyBindingEnabled, value );
            }
        }

        /// <summary>
        /// Gets the go command.
        /// </summary>
        /// <value>
        /// The go command.
        /// </value>
        public ICommand GoCommand
        {
            get
            {
                return _goCommand;
            }
            private set
            {
                _goCommand = value;
            }
        }

        /// <summary>
        /// Gets the home command.
        /// </summary>
        /// <value>
        /// The home command.
        /// </value>
        public ICommand HomeCommand
        {
            get
            {
                return _homeCommand;
            }
            private set
            {
                _homeCommand = value;
            }
        }

        /// <summary>
        /// Gets the execute java script command.
        /// </summary>
        /// <value>
        /// The execute java script command.
        /// </value>
        public ICommand ExecuteJavaScriptCommand
        {
            get
            {
                return _executeJavaScriptCommand;
            }
            private set
            {
                _executeJavaScriptCommand = value;
            }
        }

        /// <summary>
        /// Gets the evaluate java script command.
        /// </summary>
        /// <value>
        /// The evaluate java script command.
        /// </value>
        public ICommand EvaluateJavaScriptCommand
        {
            get
            {
                return _evaluateJavaScriptCommand;
            }
            private set
            {
                _evaluateJavaScriptCommand = value;
            }
        }

        /// <summary>
        /// Gets the show dev tools command.
        /// </summary>
        /// <value>
        /// The show dev tools command.
        /// </value>
        public ICommand ShowDevToolsCommand
        {
            get
            {
                return _showDevToolsCommand;
            }
            private set
            {
                _showDevToolsCommand = value;
            }
        }

        /// <summary>
        /// Gets the close dev tools command.
        /// </summary>
        /// <value>
        /// The close dev tools command.
        /// </value>
        public ICommand CloseDevToolsCommand
        {
            get
            {
                return _closeDevToolsCommand;
            }
            private set
            {
                _closeDevToolsCommand = value;
            }
        }

        /// <summary>
        /// Gets the javascript binding stress test.
        /// </summary>
        /// <value>
        /// The javascript binding stress test.
        /// </value>
        public ICommand JavascriptBindingStressTest
        {
            get
            {
                return _javascriptBindingStressTest;
            }
            private set
            {
                _javascriptBindingStressTest = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserTabViewModel"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        public BrowserTabViewModel( string address )
        {
            Address = address;
            AddressEditable = Address;

            GoCommand = new RelayCommand( Go, ( ) => !string.IsNullOrWhiteSpace( Address ) );
            HomeCommand = new RelayCommand( ( ) => AddressEditable = Address  );
            ExecuteJavaScriptCommand = new RelayCommand<string>( ExecuteJavaScript,
                s => !string.IsNullOrWhiteSpace( s ) );

            EvaluateJavaScriptCommand = new RelayCommand<string>( EvaluateJavaScript,
                s => !string.IsNullOrWhiteSpace( s ) );

            ShowDevToolsCommand = new RelayCommand( ( ) => WebBrowser.ShowDevTools( ) );
            CloseDevToolsCommand = new RelayCommand( ( ) => WebBrowser.CloseDevTools( ) );
            JavascriptBindingStressTest = new RelayCommand( ( ) =>
            {
                WebBrowser.Load( Address );
                WebBrowser.LoadingStateChanged += ( e, args ) =>
                {
                    if( args.IsLoading == false )
                    {
                        Task.Delay( 10000 ).ContinueWith( t =>
                        {
                            WebBrowser?.Reload( );
                        } );
                    }
                };
            } );
        }

        /// <summary>
        /// Evaluates the java script.
        /// </summary>
        /// <param name="s">The s.</param>
        private async void EvaluateJavaScript( string s )
        {
            try
            {
                var response = await _webBrowser.EvaluateScriptAsync( s );
                if ( response.Success
                    && response.Result is IJavascriptCallback )
                {
                    response =
                        await ( ( IJavascriptCallback )response.Result )
                            .ExecuteAsync( "Callback from EvaluateJavaScript" );
                }

                EvaluateJavaScriptResult = response.Success
                    ? response.Result ?? "null"
                    : response.Message;
            }
            catch ( Exception e )
            {
                MessageBox.Show( "Error while evaluating Javascript: " + e.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error );
            }
        }

        /// <summary>
        /// Executes the java script.
        /// </summary>
        /// <param name="s">The s.</param>
        private void ExecuteJavaScript( string s )
        {
            try
            {
                _webBrowser.ExecuteScriptAsync( s );
            }
            catch ( Exception e )
            {
                MessageBox.Show( "Error while executing Javascript: " + e.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error );
            }
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            switch ( e.PropertyName )
            {
                case "Address":
                    AddressEditable = Address;
                    break;

                case "WebBrowser":
                    if ( WebBrowser != null )
                    {
                        WebBrowser.ConsoleMessage += OnWebBrowserConsoleMessage;
                        WebBrowser.StatusMessage += OnWebBrowserStatusMessage;
                    }

                    break;
            }
        }

        /// <summary>
        /// Called when [WebBrowser console message].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ConsoleMessageEventArgs"/> instance containing the event data.</param>
        private void OnWebBrowserConsoleMessage( object sender, ConsoleMessageEventArgs e )
        {
            OutputMessage = e.Message;
        }

        /// <summary>
        /// Called when [WebBrowser status message].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StatusMessageEventArgs"/> instance containing the event data.</param>
        private void OnWebBrowserStatusMessage( object sender, StatusMessageEventArgs e )
        {
            StatusMessage = e.Value;
        }

        /// <summary>
        /// Goes this instance.
        /// </summary>
        private void Go( )
        {
            Address = AddressEditable;

            Keyboard.Focus( WebBrowser );
        }

        /// <summary>
        /// Loads the custom request example.
        /// </summary>
        public void LoadCustomRequestExample( )
        {
            var postData = Encoding.Default.GetBytes( "test=123&data=456" );

            WebBrowser.LoadUrlWithPostData( "https://cefsharp.com/PostDataTest.html", postData );
        }

        /// <summary>
        /// Sets the specified field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        protected void Set<T>( ref T field, T value,
                               [ CallerMemberName ]
                               string propertyName = null )
        {
            field = value;
            if ( PropertyChanged != null )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// 
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