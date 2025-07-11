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
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Bubba.GptMessage" />
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    public class ToolMessage : GptMessage
    {
        /// <summary>
        /// The identifier
        /// </summary>
        private protected string _toolCallId;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ToolMessage"/> class.
        /// </summary>
        public ToolMessage( )
        {
            Role = "tool";
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        /// <inheritdoc />
        [ JsonPropertyName( "name" ) ]
        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if( _name != value )
                {
                    _name = value;
                    OnPropertyChanged( nameof( Name ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the tool call identifier.
        /// </summary>
        /// <value>
        /// The tool call identifier.
        /// </value>
        [ JsonPropertyName( "tool_call_id" ) ]
        public string ToolCallId
        {
            get
            {
                return _toolCallId;
            }
            set
            {
                if( _toolCallId != value )
                {
                    _toolCallId = value;
                    OnPropertyChanged( nameof( ToolCallId ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        [JsonPropertyName( "role" ) ]
        public override string Role
        {
            get
            {
                return _role;
            }
            set
            {
                if( _role != value )
                {
                    _role = value;
                    OnPropertyChanged( Role );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [JsonPropertyName( "content" )]
        public override IDictionary<string, string> Content
        {
            get
            {
                return _content;
            }
            set
            {
                if( _content != value )
                {
                    _content = value;
                    OnPropertyChanged( nameof( Content ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns></returns>
        public override IDictionary<string, object> GetData( )
        {
            try
            {
                _data.Add( "role", _role );
                _data.Add( "content", _content );
                _data.Add( "type", _type );
                return _data?.Any( ) == true
                    ? _data
                    : default( IDictionary<string, object> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IDictionary<string, object> );
            }
        }

        /// <summary>
        /// Dumps this instance.
        /// </summary>
        /// <returns></returns>
        public string Dump( )
        {
            try
            {
                if( _data?.Any( ) == false )
                {
                    var _message = "Data is null or has no items";
                    throw new Exception( _message );
                }
                else
                {
                    var _newln = Environment.NewLine;
                    var _payload = "Data Dump:" + _newln;
                    var _space = " ";
                    var _indent = "    ";
                    foreach( var _kvp in _data )
                    {
                        _payload += _indent + $"{_kvp.Key}: {_kvp.Value}" + _newln;
                    }

                    return _payload;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Serializes the specified prompt.
        /// </summary>
        /// <returns>
        /// </returns>
        public override string Serialize( )
        {
            try
            {
                var _options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = false,
                    WriteIndented = true,
                    AllowTrailingCommas = false,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
                    PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                    DefaultIgnoreCondition = JsonIgnoreCondition.Always,
                    IncludeFields = false
                };

                var _format = JsonSerializer.Serialize( this, _options );
                return !string.IsNullOrEmpty( _format )
                    ? _format
                    : string.Empty;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }
    }
}
