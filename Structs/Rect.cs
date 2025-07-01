// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 ${CurrentDate.Month}-${CurrentDate.Day}-${CurrentDate.Year}
//
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        ${CurrentDate.Month}-${CurrentDate.Day}-${CurrentDate.Year}
// ******************************************************************************************
// <copyright file="${File.FileName}" company="Terry D. Eppler">
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
//   ${File.FileName}
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public struct Rect
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// Y coordinate
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Rect
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public Rect( int x, int y, int width, int height )
            : this( )
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Returns a new Rect with Scaled values
        /// </summary>
        /// <param name="dpi">Dpi to scale by</param>
        /// <returns>New rect with scaled values</returns>
        public Rect ScaleByDpi( float dpi )
        {
            var x = ( int )Math.Ceiling( X / dpi );
            var y = ( int )Math.Ceiling( Y / dpi );
            var width = ( int )Math.Ceiling( Width / dpi );
            var height = ( int )Math.Ceiling( Height / dpi );

            return new Rect( x, y, width, height );
        }
    }
}
