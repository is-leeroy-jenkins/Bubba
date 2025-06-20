// ******************************************************************************************
//     Assembly:                Badger
//     Author:                  Terry D. Eppler
//     Created:                 09-07-2020
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        09-07-2024
// ******************************************************************************************
// <copyright file="MetroListBoxItem.cs" company="Terry D. Eppler">
//    Badger is data analysis and reporting tool for EPA Analysts
//    that is based on WPF, NET6.0, and written in C-Sharp.
// 
//     Copyright ©  2020, 2022, 2204 Terry D. Eppler
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
//   MetroListBoxItem.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Controls.ListBoxItem" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    public class MetroListBoxItem : ListBoxItem
    {
        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// Gets or sets an arbitrary object value that can be used
        /// to store custom information about this element.
        /// </summary>
        public new object Tag { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.MetroListBoxItem" /> class.
        /// </summary>
        public MetroListBoxItem( )
            : base( )
        {
            // Control Properties
            Height = 40;
            FontFamily = new FontFamily( "Roboto" );
            FontSize = 12;
        }

        /// <summary>
        /// Called when [item mouse enter].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnMouseEnter( object sender, MouseEventArgs e )
        {
            try
            {
                if( sender is MetroListBoxItem _item )
                {
                    _item.Foreground = _theme.FormForeground;
                    _item.Background = _theme.SteelBlueBrush;
                    _item.BorderBrush = _theme.SteelBlueBrush;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [item mouse leave].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnMouseLeave( object sender, MouseEventArgs e )
        {
            try
            {
                if( sender is MetroListBoxItem _item )
                {
                    _item.Foreground = _theme.FormForeground;
                    _item.Background = _theme.ControlInterior;
                    _item.BorderBrush = _theme.ControlInterior;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [item selected].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnItemSelected( object sender, RoutedEventArgs e )
        {
            try
            {
                if( sender is MetroListBoxItem _item 
                    && _item.IsSelected )
                {
                    _item.Foreground = _theme.FormForeground;
                    _item.Background = _theme.DarkGreenBrush;
                    _item.BorderBrush = _theme.GreenBrush;
                }
            }
            catch(Exception ex)
            {
                Fail(ex);
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