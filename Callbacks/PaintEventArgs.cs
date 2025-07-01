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
    using CefSharp.Wpf;

    /// <summary>
    /// Event arguments for the Paint event handler.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class PaintEventArgs : EventArgs
    {
        /// <summary>
        /// Is the OnPaint call for a Popup or the Main View
        /// </summary>
        public bool IsPopup { get; private set; }

        /// <summary>
        /// contains the set of rectangles in pixel coordinates that need to be repainted
        /// </summary>
        public Rect DirtyRect { get; private set; }

        /// <summary>
        /// Pointer to the unmanaged buffer that holds the bitmap.
        /// The buffer shouldn't be accessed outside the scope of
        /// <see cref="ChromiumWebBrowser.Paint"/> event.
        /// A copy should be taken as the buffer is reused
        /// internally and may potentialy be freed. 
        /// </summary>
        /// <remarks>The bitmap will be width * height *
        /// 4 bytes in size and represents a BGRA image
        /// with an upper-left origin</remarks>
        public IntPtr Buffer { get; private set; }

        /// <summary>
        /// Width
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Height
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the event is handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintEventArgs"/> class.
        /// </summary>
        /// <param name="isPopup">is popup</param>
        /// <param name="dirtyRect">direct rectangle</param>
        /// <param name="buffer">buffer</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public PaintEventArgs( bool isPopup, Rect dirtyRect, IntPtr buffer, int width, int height )
        {
            IsPopup = isPopup;
            DirtyRect = dirtyRect;
            Buffer = buffer;
            Width = width;
            Height = height;
        }
    }
}
