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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Bubba.PropertyChangedBase" />
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class GptUpload : PropertyChangedBase
    {
        /// <summary>
        /// The file name
        /// </summary>
        private protected string _fileName;

        /// <summary>
        /// The bytes
        /// </summary>
        private protected int _bytes;

        /// <summary>
        /// The data
        /// </summary>
        private protected IDictionary<string, object> _data; 

        /// <summary>
        /// The identifier
        /// </summary>
        private protected string _id;

        /// <summary>
        /// The file
        /// </summary>
        private protected GptFile _file;

        /// <summary>
        /// The object
        /// </summary>
        private protected string _object;

        /// <summary>
        /// The purpose
        /// </summary>
        private protected string _purpose;

        /// <summary>
        /// The status
        /// </summary>
        private protected string _status;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptUpload"/> class.
        /// </summary>
        public GptUpload( )
        {
            _object = "Upload";
            _purpose = "assistant";
            _data = new Dictionary<string, object>( );
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                if( value != _fileName )
                {
                    _fileName = value;
                    OnPropertyChanged( );
                }
            }
        }

        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        public int Bytes
        {
            get
            {
                return _bytes;
            }
            set
            {
                if( value != _bytes )
                {
                    _bytes = value;
                    OnPropertyChanged( );
                }
            }
        }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        public GptFile File
        {
            get
            {
                return _file;
            }
            set
            {
                if( value != _file )
                {
                    _file = value;
                    OnPropertyChanged( );
                }
            }
        }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        public string Object
        {
            get
            {
                return _object;
            }
            set
            {
                if( value != _object )
                {
                    _object = value;
                    OnPropertyChanged( );
                }
            }
        }

        /// <summary>
        /// Gets or sets the purpose.
        /// </summary>
        /// <value>
        /// The purpose.
        /// </value>
        public string Purpose
        {
            get
            {
                return _purpose;
            }
            set
            {
                if( value != _purpose )
                {
                    _purpose = value;
                    OnPropertyChanged( );
                }
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if( value != _status )
                {
                    _status = value;
                    OnPropertyChanged( );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// </returns>
        public IDictionary<string, object> GetData( )
        {
            try
            {
                return default( IDictionary<string, object> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IDictionary<string, object> );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" />
        /// that represents this instance.
        /// </returns>
        public override string ToString( )
        {
            try
            {
                return string.Empty;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
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
