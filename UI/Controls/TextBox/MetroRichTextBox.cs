﻿namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using Syncfusion.Windows.Controls.Input;
    using Syncfusion.Windows.Controls.RichTextBoxAdv;
    using Syncfusion.Windows.Forms.Tools;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Syncfusion.Windows.Controls.RichTextBoxAdv.SfRichTextBoxAdv" />
    [SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassNeverInstantiated.Global" ) ]
    public class MetroRichTextBox : SfRichTextBoxAdv
    {
        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.MetroRichTextBox" /> class.
        /// </summary>
        public MetroRichTextBox( )
            : base( )
        {
            SetResourceReference( StyleProperty, typeof( SfRichTextBoxAdv ) );
            FontFamily = _theme.FontFamily;
            FontSize = _theme.FontSize;
            Width = 200;
            Height = 100;
            Padding = new Thickness( 1 );
            BorderThickness = _theme.BorderThickness;
            Padding = _theme.Padding;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            // Event Wiring
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }

        /// <summary>
        /// Called when [mouse enter].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnMouseEnter( object sender, RoutedEventArgs e )
        {
            try
            {
                if( sender is MetroRichTextBox TextBox )
                {
                    TextBox.Background = _theme.DarkBlueBrush;
                    TextBox.BorderBrush = _theme.LightBlueBrush;
                    TextBox.Foreground = _theme.WhiteForeground;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [mouse leave].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnMouseLeave( object sender, RoutedEventArgs e )
        {
            try
            {
                if( sender is MetroRichTextBox TextBox )
                {
                    TextBox.Background = _theme.ControlInterior;
                    TextBox.BorderBrush = _theme.SteelBlueBrush;
                    TextBox.Foreground = _theme.LightBlueBrush;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="_ex">The ex.</param>
        private protected void Fail( Exception _ex )
        {
            var _error = new ErrorWindow( _ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}