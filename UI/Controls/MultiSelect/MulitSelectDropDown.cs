﻿// ******************************************************************************************
//     Assembly:                Badger
//     Author:                  Terry D. Eppler
//     Created:                 08-01-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-01-2024
// ******************************************************************************************
// <copyright file="MulitSelectDropDown.cs" company="Terry D. Eppler">
//    Badger is data analysis and reporting tool for EPA Analysts
//    based on WPF, NET6.0, and written in C-Sharp.
// 
//    Copyright ©  2024  Terry D. Eppler
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
//    You can contact me at: terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   MulitSelectDropDown.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using Syncfusion.UI.Xaml.Grid;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Syncfusion.UI.Xaml.Grid.SfMultiColumnDropDownControl" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    public class MulitSelectDropDown : SfMultiColumnDropDownControl
    {
        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Badger.MetroMultiSelect" /> class.
        /// </summary>
        public MulitSelectDropDown( )
        {
            // Control Properties
            SetResourceReference( StyleProperty,
                typeof( SfMultiColumnDropDownControl ) );

            FontFamily = new FontFamily( "Roboto" );
            FontSize = 12;
            PopupBorderThickness = new Thickness( 1 );
            HorizontalAlignment = _theme.HorizontalAlignment;
            VerticalAlignment = _theme.VerticalAlignment;
        }
    }
}