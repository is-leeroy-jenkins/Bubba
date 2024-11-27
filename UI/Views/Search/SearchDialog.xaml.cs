namespace Bubba
{
    using System.Configuration;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for SearchDialog.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "ConvertToAutoPropertyWhenPossible" ) ]
    [ SuppressMessage( "ReSharper", "UnusedParameter.Global" ) ]
    public partial class SearchDialog : Window
    {
        /// <summary>
        /// The domain
        /// </summary>
        private string _queryPrefix;

        /// <summary>
        /// The results
        /// </summary>
        private string _results;

        /// <summary>
        /// The input
        /// </summary>
        private string _keywordInput;

        /// <summary>
        /// The keyword prefix
        /// </summary>
        private string _keywordLabelPrefix;

        /// <summary>
        /// The domain prefix
        /// </summary>
        private string _domainLabelPrefix;

        /// <summary>
        /// The stop watch
        /// </summary>
        private Stopwatch _stopWatch = new Stopwatch( );

        /// <summary>
        /// The selected domains
        /// </summary>
        private protected IList<string> _selectedDomains;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.SearchDialog" /> class.
        /// </summary>
        public SearchDialog( )
        {
            InitializeComponent( );
            RegisterCallbacks( );

            // Form Properterties
            _keywordInput = string.Empty;
            _results = string.Empty;
            _domainLabelPrefix = "Domain:";
            _keywordLabelPrefix = "Key Words:";
            _queryPrefix = ConfigurationManager.AppSettings[ "Google" ];

            // Control Properties
            DomainLabel.Content = _domainLabelPrefix + " " + "Google";
            QueryLabel.Content = _keywordLabelPrefix + " " + "0";

            //Event Wiring
            Loaded += OnLoad;
        }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>
        /// The results.
        /// </value>
        public string Results
        {
            get
            {
                return _results;
            }
            private protected set
            {
                _results = value;
            }
        }

        /// <summary>
        /// Registers the callbacks.
        /// </summary>
        private void RegisterCallbacks( )
        {
            try
            {
                LookupButton.Click += OnLookupButtonClick;
                CancelButton.Click += OnCloseButtonClick;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the buttons.
        /// </summary>
        private void InitializeTextBox( )
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
        /// Initializes the labels.
        /// </summary>
        private void InitializeLabels( )
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
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Initializes the ComboBox.
        /// </summary>
        private void InitializeComboBox( )
        {
            try
            {
                //ComboBox.ForeColor = Color.White;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Fades the in asynchronous.
        /// </summary>
        /// <param name="form">The o.</param>
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
        /// <param name="form">The o.</param>
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
        /// Sets the tool strip properties.
        /// </summary>
        private void PopulateDomainDropDowns( ) 
        {
            try
            {
                DomainDropDown.Items?.Clear( );
                var _domains = Enum.GetNames( typeof( Domains ) );
                for( var _i = 0; _i < _domains.Length; _i++ )
                {
                    var _domain = _domains[ _i ];
                    if( !string.IsNullOrEmpty( _domain ) )
                    {
                        DomainDropDown.Items.Add( _domain );
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
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// instance containing the event data.
        /// </param>
        private void OnLoad( object sender, RoutedEventArgs e )
        {
            try
            {
                InitializeTextBox( );
                InitializeButtons( );
                InitializeComboBox( );
                InitializeLabels( );
                PopulateDomainDropDowns( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [close button clicked].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The
        /// <see cref="EventArgs"/>
        /// instance containing the event data.
        /// </param>
        public virtual void OnCloseButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                _results = string.Empty;
                _queryPrefix = string.Empty;
                _domainLabelPrefix = string.Empty;
                Close( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [okay button click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        public virtual void OnLookupButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                if( !string.IsNullOrEmpty( _keywordInput )
                    && !string.IsNullOrEmpty( _queryPrefix ) )
                {
                    _results = _queryPrefix + _keywordInput;;
                    Close( );
                }
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
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        public virtual void OnClearButtonClick( object sender, RoutedEventArgs e )
        {
            try
            {
                _keywordInput = string.Empty;
                _results = string.Empty;
                _queryPrefix = ConfigurationManager.AppSettings[ "Google" ];
                DomainLabel.Content = _domainLabelPrefix;
                QueryLabel.Content = _keywordLabelPrefix;
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [input text changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        public void OnInputTextChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                _keywordInput = SearchPanelTextBox.Name;
                if( _keywordInput.Contains( " " ) )
                {
                    var _split = _keywordInput.Split( " " );
                    QueryLabel.Content = _keywordLabelPrefix + " " + _split.Length;
                    _results = _queryPrefix + _keywordInput;
                }
                else
                {
                    QueryLabel.Content = _keywordLabelPrefix + " " + 0;
                    _results = _queryPrefix + _keywordInput;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [search engine selected].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnSelectedDomainChanged( object sender, RoutedEventArgs e )
        {
            try
            {
                if( sender is MetroComboBox _comboBox )
                {
                    var _index = _comboBox.SelectedIndex;
                    var _tag = _comboBox.SelectedItem;
                    _comboBox.Tag = _tag;
                    _queryPrefix = _index switch
                    {
                        0 => ConfigurationManager.AppSettings[ "EPA" ],
                        1 => ConfigurationManager.AppSettings[ "DATA" ],
                        2 => ConfigurationManager.AppSettings[ "GPO" ],
                        3 => ConfigurationManager.AppSettings[ "USGI" ],
                        4 => ConfigurationManager.AppSettings[ "CRS" ],
                        5 => ConfigurationManager.AppSettings[ "LOC" ],
                        6 => ConfigurationManager.AppSettings[ "OMB" ],
                        7 => ConfigurationManager.AppSettings[ "UST" ],
                        8 => ConfigurationManager.AppSettings[ "NASA" ],
                        9 => ConfigurationManager.AppSettings[ "NOAA" ],
                        10 => ConfigurationManager.AppSettings[ "DOI" ],
                        11 => ConfigurationManager.AppSettings[ "NPS" ],
                        12 => ConfigurationManager.AppSettings[ "GSA" ],
                        13 => ConfigurationManager.AppSettings[ "NARA" ],
                        14 => ConfigurationManager.AppSettings[ "DOC" ],
                        15 => ConfigurationManager.AppSettings[ "HHS" ],
                        16 => ConfigurationManager.AppSettings[ "NRC" ],
                        17 => ConfigurationManager.AppSettings[ "DOE" ],
                        18 => ConfigurationManager.AppSettings[ "NSF" ],
                        19 => ConfigurationManager.AppSettings[ "USDA" ],
                        20 => ConfigurationManager.AppSettings[ "CSB" ],
                        21 => ConfigurationManager.AppSettings[ "IRS" ],
                        22 => ConfigurationManager.AppSettings[ "FDA" ],
                        23 => ConfigurationManager.AppSettings[ "CDC" ],
                        24 => ConfigurationManager.AppSettings[ "ACE" ],
                        25 => ConfigurationManager.AppSettings[ "DHS" ],
                        26 => ConfigurationManager.AppSettings[ "DOD" ],
                        27 => ConfigurationManager.AppSettings[ "USNO" ],
                        28 => ConfigurationManager.AppSettings[ "NWS" ],
                        _ => ConfigurationManager.AppSettings[ "Google" ]
                    };

                    var _selection = _tag.ToString( );
                    if( !string.IsNullOrEmpty( _selection ) )
                    {
                        //DomainLabel.Text = _domainLabelPrefix + " " + _selection;
                    }
                    else
                    {
                        //DomainLabel.Text = _domainLabelPrefix + " " + "Google";
                    }
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Get ErrorDialog Dialog.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        private void Fail( Exception ex )
        {
            using var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}
