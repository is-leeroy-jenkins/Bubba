// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 02-04-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        02-04-2025
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
    using CefSharp;
    using CefSharp.Wpf;
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
    [ SuppressMessage( "ReSharper", "CanSimplifyDictionaryLookupWithTryGetValue" ) ]
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
            "TabItemExt",
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
            "SfImageEditor",
            "SfBusyIndicator"
        };

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.App" /> class.
        /// </summary>
        public App( )
        {
            var _key = ConfigurationManager.AppSettings[ "UI" ];
            SyncfusionLicenseProvider.RegisterLicense( _key );
            OpenAiKey = Environment.GetEnvironmentVariable( "OPENAI_API_KEY" );
            GoogleKey = Environment.GetEnvironmentVariable( "GOOGLE_API_KEY" );
            ExcelPackage.License.SetNonCommercialPersonal( "Terry Eppler" );
            Instructions = OpenAI.BubbaPrompt;
            RegisterTheme( );
            ActiveWindows = new Dictionary<string, Window>( );

            // Wire Events
            Startup += OnStartup;
            Exit += OnExit;
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
                PrimaryForeground = new SolidColorBrush( Color.FromRgb( 160, 189, 252 ) ),
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
        /// Gets the application directory.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static string GetApplicationDirectory( string name )
        {
            try
            {
                ThrowIf.Empty( name, nameof( name ) );
                var _directory = @"C:\Documents and Settings\All Users\Application Data\";
                return Directory.Exists( _directory )
                    ? _directory + Locations.Branding + @"\" + name + @"\"
                    : @"C:\ProgramData\" + Locations.Branding + @"\" + name + @"\";
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
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

        /// <inheritdoc />
        /// <summary>
        /// Raises the
        /// <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name = "sender" > </param>
        /// <param name="e">
        /// that contains the event data.
        /// </param>
        public void OnStartup( object sender, StartupEventArgs e )
        {
            try
            {
                DispatcherUnhandledException += ( s, args ) => HandleException( args.Exception );
                TaskScheduler.UnobservedTaskException += ( s, args ) =>
                    HandleException( args.Exception?.InnerException );

                AppDomain.CurrentDomain.UnhandledException += ( s, args ) =>
                    HandleException( args.ExceptionObject as Exception );

                var _cefSettings = new CefSettings( );
                _cefSettings.RegisterScheme( new CefCustomScheme
                {
                    SchemeName = Locations.Internal,
                    SchemeHandlerFactory = new SchemaCallbackFactory( )
                } );

                _cefSettings.UserAgent = Locations.UserAgent;
                _cefSettings.AcceptLanguageList = Locations.AcceptLanguage;
                _cefSettings.IgnoreCertificateErrors = true;
                _cefSettings.CachePath = GetApplicationDirectory( "Cache" );
                if( bool.Parse( Locations.Proxy ) )
                {
                    CefSharpSettings.Proxy = new ProxyOptions( Locations.ProxyIP,
                        Locations.ProxyPort, Locations.ProxyUsername, Locations.ProxyPassword,
                        Locations.ProxyBypassList );
                }

                Cef.Initialize( _cefSettings );
            }
            catch( Exception ex )
            {
                Cef.Shutdown( );
                Fail( ex );
                Environment.Exit( 1 );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name = "sender" > </param>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" />
        /// that contains the event data.</param>
        public virtual void OnExit( object sender, ExitEventArgs e )
        {
            try
            {
                Cef.Shutdown( );
                base.OnExit( e );
                ActiveWindows?.Clear( );
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