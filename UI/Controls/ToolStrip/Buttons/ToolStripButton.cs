﻿// ******************************************************************************************
//     Assembly:                Badger
//     Author:                  Terry D. Eppler
//     Created:                 09-07-2020
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        09-07-2024
// ******************************************************************************************
// <copyright file="ToolStripButton.cs" company="Terry D. Eppler">
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
//   ToolStripButton.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Input;
    using Syncfusion.Windows.Controls.Notification;
    using Syncfusion.Windows.Tools.Controls;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <seealso cref="T:Badger.MetroButton" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "PublicConstructorInAbstractClass" ) ]
    public class ToolStripButton : MetroTile
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolStripButton"/> class.
        /// </summary>
        /// <inheritdoc />
        public ToolStripButton( )
            : base( )
        {
            SetResourceReference( StyleProperty, typeof( SfHubTile ) );

            // Basic Properties
            Width = 40;
            Height = 30;
            Header = "";
            Title = "";
            Padding = new Thickness( 1 );
            Margin = new Thickness( 5, 1, 5, 1 );
            Background = _theme.FormBackground;
            BorderBrush = _theme.FormBackground;
            Foreground = _theme.LightBlueBrush;

            // Wire Events
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }

        /// <inheritdoc />
        /// <summary> Called when [mouse enter]. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e">
        /// The
        /// <see cref="T:System.EventArgs" />
        /// instance containing the event data.
        /// </param>
        private protected override void OnMouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                Background = _theme.SteelBlueBrush;
                BorderBrush = _theme.LightBlueBrush;
                Foreground = _theme.WhiteForeground;
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <inheritdoc />
        /// <summary> Called when [mouse leave]. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e">
        /// The
        /// <see cref="T:System.EventArgs" />
        /// instance containing the event data.
        /// </param>
        private protected override void OnMouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                Background = _theme.FormBackground;
                BorderBrush = _theme.FormBackground;
                Foreground = _theme.LightBlueBrush;
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }
    }
}