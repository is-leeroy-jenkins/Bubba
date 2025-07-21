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
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class GptInput : IGptInput
    {
        /// <summary>
        /// The first identifier
        /// </summary>
        private protected string _firstId;

        /// <summary>
        /// The last identifier
        /// </summary>
        private protected string _lastId;

        /// <summary>
        /// The has more
        /// </summary>
        private protected bool _hasMore;

        /// <summary>
        /// The object
        /// </summary>
        private protected readonly string _object;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptInput"/> class.
        /// </summary>
        public GptInput( )
        {
            _object = "list";
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptInput"/> class.
        /// </summary>
        /// <param name="firstId">The first identifier.</param>
        /// <param name="lastId">The last identifier.</param>
        /// <param name="hasMore">if set to <c>true</c> [has more].</param>
        public GptInput( string firstId, string lastId, bool hasMore = false ) 
            : this( )
        {
            _firstId = firstId;
            _lastId = lastId;
            _hasMore = hasMore;
        }

        /// <summary>
        /// Gets or sets the first identifier.
        /// </summary>
        /// <value>
        /// The first identifier.
        /// </value>
        [ JsonPropertyName( "first_id" ) ]
        public string FirstId
        {
            get
            {
                return _firstId;
            }
            set
            {
                _firstId = value;
            }
        }

        /// <summary>
        /// Gets or sets the last identifier.
        /// </summary>
        /// <value>
        /// The last identifier.
        /// </value>
        [ JsonPropertyName( "last_id" ) ]
        public string LastId
        {
            get
            {
                return _lastId;
            }
            set
            {
                _lastId = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has more.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has more; otherwise, <c>false</c>.
        /// </value>
        [ JsonPropertyName( "has_more" ) ]
        public bool HasMore
        {
            get
            {
                return _hasMore;
            }
            set
            {
                _hasMore = value;
            }
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        [ JsonPropertyName( "object" ) ]
        public string Object
        {
            get
            {
                return _object;
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
