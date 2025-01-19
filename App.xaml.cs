// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-19-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-19-2025
// ******************************************************************************************
// <copyright file="App.xaml.cs" company="Terry D. Eppler">
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
//   App.xaml.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using OfficeOpenXml;
    using RestoreWindowPlace;
    using Syncfusion.Licensing;
    using Syncfusion.SfSkinManager;
    using Syncfusion.Themes.FluentDark.WPF;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "RedundantJumpStatement" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "ExpressionIsAlwaysNull" ) ]
    public partial class App : Application
    {
        /// <summary>
        /// The window place
        /// </summary>
        private protected WindowPlace _windowPlace;

        /// <summary>
        /// The active windows
        /// </summary>
        public static IDictionary<string, Window> ActiveWindows { get; private set; }

        /// <summary>
        /// The controls
        /// </summary>
        public static string[ ] Controls =
        {
            "ComboBoxAdv",
            "MetroComboBox",
            "MetroDatagrid",
            "SfDataGrid",
            "ToolBarAdv",
            "ToolStrip",
            "MetroCalendar",
            "CalendarEdit",
            "PivotGridControl",
            "MetroPivotGrid",
            "SfChart",
            "SfChart3D",
            "SfHeatMap",
            "SfMap",
            "MetroMap",
            "EditControl",
            "CheckListBox",
            "MetroEditor",
            "DropDownButtonAdv",
            "SfTextBoxExt",
            "SfCircularProgressBar",
            "SfLinearProgressBar",
            "GridControl",
            "MetroGridControl",
            "TabControlExt",
            "MetroTabControl",
            "SfTextInputLayout",
            "MetroTextInput",
            "SfSpreadsheet",
            "SfSpreadsheetRibbon",
            "MenuItemAdv",
            "ButtonAdv",
            "Carousel",
            "ColorEdit",
            "SfCalculator",
            "SfMultiColumnDropDownControl",
            "SfImageEditor"
        };

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.App" /> class.
        /// </summary>
        public App( )
        {
            InitializeDelegates( );
            var _key = ConfigurationManager.AppSettings[ "UI" ];
            SyncfusionLicenseProvider.RegisterLicense( _key );
            OpenAiKey = Environment.GetEnvironmentVariable( "OPENAI_API_KEY" );
            GoogleKey = Environment.GetEnvironmentVariable( "GOOGLE_API_KEY" );
            Instructions = OpenAI.BubbaPrompt;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            RegisterTheme( );
            ActiveWindows = new Dictionary<string, Window>( );
        }

        /// <summary>
        /// The system instructions
        /// </summary>
        public static string Instructions;

        /// <summary>
        /// The open ai API key
        /// </summary>
        public static string OpenAiKey;

        /// <summary>
        /// The google API key
        /// </summary>
        public static string GoogleKey;

        /// <summary>
        /// Registers the _theme.
        /// </summary>
        private void RegisterTheme( )
        {
            var _theme = new FluentDarkThemeSettings
            {
                PrimaryBackground = new SolidColorBrush( Color.FromRgb( 20, 20, 20 ) ),
                PrimaryColorForeground = new SolidColorBrush( Color.FromRgb( 0, 120, 212 ) ),
                PrimaryForeground = new SolidColorBrush( Color.FromRgb( 222, 222, 222 ) ),
                BodyFontSize = 12,
                HeaderFontSize = 16,
                SubHeaderFontSize = 14,
                TitleFontSize = 16,
                SubTitleFontSize = 14,
                BodyAltFontSize = 11,
                FontFamily = new FontFamily( "Roboto" )
            };

            SfSkinManager.RegisterThemeSettings( "FluentDark", _theme );
            SfSkinManager.ApplyStylesOnApplication = true;
        }

        /// <summary>
        /// Initializes the delegates.
        /// </summary>
        private void InitializeDelegates( )
        {
            try
            {
                DispatcherUnhandledException += ( s, args ) => HandleException( args.Exception );
                TaskScheduler.UnobservedTaskException += ( s, args ) =>
                    HandleException( args.Exception?.InnerException );

                AppDomain.CurrentDomain.UnhandledException += ( s, args ) =>
                    HandleException( args.ExceptionObject as Exception );
            }
            catch( Exception e )
            {
                Fail( e );
            }
        }

        /// <summary>
        /// Setups the restore window place.
        /// </summary>
        /// <param name="mainWindow">
        /// The main window.
        /// </param>
        public void SetupRestoreWindowPlace( ChatWindow mainWindow )
        {
            var _config = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Bubba.config" );
            _windowPlace = new WindowPlace( _config );
            _windowPlace.Register( mainWindow );
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void HandleException( Exception ex )
        {
            if( ex == null )
            {
                var _msg = $"The argument {ex} is null!";
                throw new ArgumentNullException( _msg );
            }
            else
            {
                Fail( ex );
                Environment.Exit( 1 );
            }
        }

        /// <summary>
        /// Opens the GPT file dialog asynchronous.
        /// </summary>
        public static async Task OpenGptFileDialogAsync( )
        {
            try
            {
                await Task.Run( ( ) => OpenGptFileDialog( ) );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the GPT file dialog.
        /// </summary>
        public static void OpenGptFileDialog( )
        {
            try
            {
                var _gptFileDialog = new GptFileDialog
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                _gptFileDialog.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the folder browser asynchronous.
        /// </summary>
        public static async Task OpenFolderBrowserAsync( )
        {
            try
            {
                await Task.Run( ( ) => OpenFolderBrowser( ) );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the folder browser.
        /// </summary>
        public static void OpenFolderBrowser( )
        {
            try
            {
                var _folderBrowser = new FolderBrowser( )
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                _folderBrowser.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the file browser asynchronous.
        /// </summary>
        public static async Task OpenFileBrowserAsync( )
        {
            try
            {
                await Task.Run( ( ) => OpenFileBrowser( ) );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the file browser.
        /// </summary>
        public static void OpenFileBrowser( )
        {
            try
            {
                var _fileBrowser = new FileBrowser( )
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                _fileBrowser.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the search dialog asynchronous.
        /// </summary>
        public static async Task OpenSearchDialogAsync( )
        {
            try
            {
                await Task.Run( ( ) => OpenSearchDialog( ) );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the search dialog.
        /// </summary>
        public static void OpenSearchDialog( )
        {
            try
            {
                var _searchDialog = new SearchDialog
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                _searchDialog.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the image dialog asynchronous.
        /// </summary>
        public static async Task OpenImageDialogAsync( )
        {
            try
            {
                await Task.Run( ( ) => OpenImageDialog( ) );
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
        public static void OpenImageDialog( )
        {
            try
            {
                var _web = new GptImageDialog
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                _web.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the WebBrowser asynchronous.
        /// </summary>
        public static async Task OpenWebBrowserAsync( )
        {
            try
            {
                await Task.Run( ( ) => OpenWebBrowser( ) );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the WebBrowser.
        /// </summary>
        public static void OpenWebBrowser( )
        {
            try
            {
                var _web = new WebBrowser( )
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                ActiveWindows.Add( "WebBrowser", _web );
                _web.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the system dialog asynchronous.
        /// </summary>
        public static async Task OpenSystemDialogAsync( )
        {
            try
            {
                await Task.Run( ( ) => OpenSystemDialog( ) );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Opens the prompt dialog.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public static void OpenSystemDialog( )
        {
            try
            {
                var _systemDialog = new SystemDialog( )
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                _systemDialog.Show( );
                _systemDialog.SystemDialogTextBox.Text = Instructions;
                _systemDialog.SystemDialogTextBox.Focus( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A
        /// <see cref="T:System.Windows.StartupEventArgs" />
        /// that contains the event data.
        /// </param>
        protected override void OnStartup( StartupEventArgs e )
        {
            try
            {
                base.OnStartup( e );
            }
            catch( Exception ex )
            {
                Fail( ex );
                Environment.Exit( 1 );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" />
        /// that contains the event data.</param>
        protected override void OnExit( ExitEventArgs e )
        {
            try
            {
                base.OnExit( e );
                Environment.Exit( 0 );
            }
            catch( Exception )
            {
                // Do Nothing
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private static void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}