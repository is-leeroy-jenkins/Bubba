

namespace Bubba
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using OfficeOpenXml;
    using RestoreWindowPlace;
    using Syncfusion.Licensing;
    using Syncfusion.SfSkinManager;
    using Syncfusion.Themes.FluentDark.WPF;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The window place
        /// </summary>
        private WindowPlace _windowPlace;

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
            "SfMultiColumnDropDownControl"
        };

        public static string KEY = ConfigurationManager.AppSettings[ "OpenAi" ];

        /// <summary>
        /// Registers the theme.
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
        /// Initializes a new instance of the
        /// <see cref="App"/> class.
        /// </summary>
        public App( )
        {
            var _key = ConfigurationManager.AppSettings[ "UI" ];
            SyncfusionLicenseProvider.RegisterLicense( _key );
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            RegisterTheme( );
        }

        /// <summary>
        /// Setups the restore window place.
        /// </summary>
        /// <param name="mainWindow">
        /// The main window.
        /// </param>
        private void SetupRestoreWindowPlace( MainWindow mainWindow )
        {
            var _config = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bobo.config");
            _windowPlace = new WindowPlace(_config);
            _windowPlace.Register(mainWindow);

            // This logic works but I don't like the window being maximized
            //if (!File.Exists(windowPlaceConfigFilePath))
            //{
            //    // For the first time, maximize the window, so it won't go off the screen on laptop
            //    // WindowPlacement will take care of future runs
            //    mainWindow.WindowState = WindowState.Maximized;
            //}
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
                DispatcherUnhandledException += ( s, args ) => HandleException( args.Exception );
                TaskScheduler.UnobservedTaskException += ( s, args ) =>
                    HandleException( args.Exception?.InnerException );

                AppDomain.CurrentDomain.UnhandledException += ( s, args ) =>
                    HandleException( args.ExceptionObject as Exception );

                // TODO 1: Get your OpenAI API key: https://platform.openai.com/account/api-keys
                string _openaiApiKey;
                if(e.Args?.Length > 0
                    && e.Args[ 0 ].StartsWith( '/' ) )
                {
                    // OpenAI API key from command line parameter
                    // such as "/sk-Ih...WPd" after removing '/'
                    _openaiApiKey = e.Args[ 0 ].Remove( 0, 1 );
                }
                else
                {
                    // Put your key from above here instead of
                    // using a command line parameter in the 'if' block
                    _openaiApiKey = ConfigurationManager.AppSettings[ "Bubba" ];
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
                Current?.Shutdown( );
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
                _windowPlace?.Save( );
                Environment.Exit( 0 );
            }
            catch( Exception )
            {
                // Do Nothing
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="e">The e.</param>
        private void HandleException( Exception e )
        {
            if( e == null )
            {
            }
            else
            {
                Fail( e );
                Environment.Exit( 1 );
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
