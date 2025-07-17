// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 02-23-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        02-23-2025
// ******************************************************************************************
// <copyright file="SystemMessage.cs" company="Terry D. Eppler">
//    Bubba is a small and simple windows (wpf) application for interacting with the OpenAI API
//    that's developed in C-Sharp under the MIT license.C#.
// 
//    Copyright ©  2020-2024 Terry D. Eppler
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
//   SystemMessage.cs
// </summary>
// ******************************************************************************************
namespace Bubba
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Resources;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Properties;
    using Exception = System.Exception;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptMessage" />
    /// <seealso cref="T:Bubba.IGptMessage" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassNeverInstantiated.Global" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "CanSimplifyDictionaryLookupWithTryGetValue" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    public class SystemMessage : GptMessage, IGptMessage
    {
        /// <summary>
        /// The instructions
        /// </summary>
        private protected string _instructions;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SystemMessage"/> class.
        /// </summary>
        public SystemMessage( )
        {
            _role = "system";
            _type = "text";
            _instructions = Prompts.BudgetAnalyst;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemMessage"/> class.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        public SystemMessage( string prompt )
        {
            _role = "system";
            _type = "text";
            _instructions = prompt;
            _content.Add( _type, prompt );
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SystemMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SystemMessage( SystemMessage message )
        {
            _role = message.Role;
            _type = message.Type;
            _content = message.Content;
        }

        /// <summary>
        /// Deconstructs the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="content">The content.</param>
        public void Deconstruct( out string role, out IDictionary<string, string> content )
        {
            role = _role;
            content = _content;
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

        /// <inheritdoc />
        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        [ JsonPropertyName( "role" ) ]
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
        [ JsonPropertyName( "content" ) ]
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

        /// <summary>
        /// Gets the resource names.
        /// </summary>
        /// <param name="resxPath">The RESX file path.</param>
        /// <returns></returns>
        private protected List<string> GetResourceNames( string resxPath )
        {
            try
            {
                ThrowIf.Null( resxPath, nameof( resxPath ) );
                var _names = new List<string>( );
                using var reader = new ResXResourceReader( resxPath );
                foreach( DictionaryEntry entry in reader )
                {
                    _names.Add( entry.Key.ToString( ) );
                }

                return _names?.Any( ) == true
                    ? _names
                    : default( List<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( List<string> );
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
                _data.Add( "instructions", _instructions );
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
                if( _data?.Any(  ) == false )
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