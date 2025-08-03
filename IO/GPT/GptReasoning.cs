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
    using System.Text.Json.Serialization;

    /// <inheritdoc />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class GptReasoning : PropertyChangedBase
    {
        /// <summary>
        /// The effort
        /// </summary>
        private protected string _effort;

        /// <summary>
        /// The summary
        /// </summary>
        private protected string _summary;

        /// <summary>
        /// The identifier
        /// </summary>
        private protected string _id;

        /// <summary>
        /// The status
        /// </summary>
        private protected string _status;

        /// <summary>
        /// The encrypted content
        /// </summary>
        private protected string _encryptedContent;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptReasoning"/> class.
        /// </summary>
        public GptReasoning( )
        {
            _effort = "medium";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GptReasoning"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="effort">The effort.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="encryptedContent">Content of the encrypted.</param>
        /// <param name="status">The status.</param>
        public GptReasoning( string id, string effort, string summary, 
                             string encryptedContent, string status )
        {
            _id = id;
            _effort = effort;
            _summary = summary;
            _encryptedContent = encryptedContent;
            _status = status;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [ JsonPropertyName( "id" ) ]
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if( _id != value )
                {
                    _id = value;
                    OnPropertyChanged( nameof( Id ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [ JsonPropertyName( "effort" ) ]
        public string Effort
        {
            get
            {
                return _effort;
            }
            set
            {
                if( _effort != value )
                {
                    _effort = value;
                    OnPropertyChanged( nameof( Effort ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [ JsonPropertyName( "summary" ) ]
        public string Summary
        {
            get
            {
                return _summary;
            }
            set
            {
                if( _summary != value )
                {
                    _summary = value;
                    OnPropertyChanged( nameof( Summary ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the content of the encrypted.
        /// </summary>
        /// <value>
        /// The content of the encrypted.
        /// </value>
        [ JsonPropertyName( "encrypted_content" ) ]
        public string EncryptedContent
        {
            get
            {
                return _encryptedContent;
            }
            set
            {
                if( _encryptedContent != value )
                {
                    _encryptedContent = value;
                    OnPropertyChanged( nameof( EncryptedContent ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [ JsonPropertyName( "status" ) ]
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if( _status != value )
                {
                    _status = value;
                    OnPropertyChanged( nameof( Status ) );
                }
            }
        }

        /// <summary>
        /// Gets the effort options.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetEffortOptions( )
        {
            var _options = new List<string>( );
            _options.Add( "low" );  
            _options.Add( "medium" );
            _options.Add( "high" );
            return _options;
        }

        /// <summary>
        /// Wraps error
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
