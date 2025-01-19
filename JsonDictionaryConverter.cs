// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-19-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-19-2025
// ******************************************************************************************
// <copyright file="DictionaryJsonConverter.cs" company="Terry D. Eppler">
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
//   DictionaryJsonConverter.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class DictionaryJsonConverter : JsonConverter<IDictionary<string, object>>
    {
        /// <inheritdoc />
        /// <summary>
        /// Reads and converts the JSON to a Dictionary(string, object).
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>
        /// The converted value.
        /// </returns>
        /// <exception cref="T:Newtonsoft.Json.JsonException">
        /// Expected StartObject token
        /// or
        /// Expected PropertyName token
        /// or
        /// Unexpected token {reader.TokenType}
        /// or
        /// Expected EndObject token
        /// </exception>
        public override IDictionary<string, object> Read( ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options )
        {
            if( reader.TokenType != JsonTokenType.StartObject )
            {
                throw new JsonException( "Expected StartObject token" );
            }

            var _dictionary = new Dictionary<string, object>( );
            while( reader.Read( ) )
            {
                if( reader.TokenType == JsonTokenType.EndObject )
                {
                    return _dictionary;
                }

                if( reader.TokenType != JsonTokenType.PropertyName )
                {
                    throw new JsonException( "Expected PropertyName token" );
                }

                var _propertyName = reader.GetString( );
                reader.Read( );
                object _value;
                switch( reader.TokenType )
                {
                    case JsonTokenType.String:
                    {
                        _value = reader.GetString( );
                        break;
                    }
                    case JsonTokenType.Number:
                    {
                        if( reader.TryGetInt32( out var _intValue ) )
                        {
                            _value = _intValue;
                        }
                        else
                        {
                            _value = reader.GetDouble( );
                        }

                        break;
                    }
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                    {
                        _value = reader.GetBoolean( );
                        break;
                    }
                    case JsonTokenType.StartObject:
                    {
                        _value = JsonSerializer.Deserialize<Dictionary<string, object>>( ref reader,
                            options );

                        break;
                    }
                    case JsonTokenType.StartArray:
                    {
                        _value = JsonSerializer.Deserialize<List<object>>( ref reader, options );
                        break;
                    }
                    default:
                    {
                        throw new JsonException( $"Unexpected token {reader.TokenType}" );
                    }
                }

                if( !string.IsNullOrEmpty( _propertyName ) )
                {
                    _dictionary[ _propertyName ] = _value;
                }
            }

            throw new JsonException( "Expected EndObject token" );
        }

        /// <inheritdoc />
        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write( Utf8JsonWriter writer, IDictionary<string, object> value,
            JsonSerializerOptions options )
        {
            writer.WriteStartObject( );
            foreach( var _kvp in value )
            {
                writer.WritePropertyName( _kvp.Key );
                switch( _kvp.Value )
                {
                    case string _stringValue:
                    {
                        writer.WriteStringValue( _stringValue );
                        break;
                    }
                    case int _intValue:
                    {
                        writer.WriteNumberValue( _intValue );
                        break;
                    }
                    case bool _boolValue:
                    {
                        writer.WriteBooleanValue( _boolValue );
                        break;
                    }
                    default:
                    {
                        JsonSerializer.Serialize( writer, _kvp.Value, options );
                        break;
                    }
                }
            }

            writer.WriteEndObject( );
        }
    }
}