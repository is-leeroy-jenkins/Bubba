// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-30-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-30-2025
// ******************************************************************************************
// <copyright file="GptResponse.cs" company="Terry D. Eppler">
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
//   GptResponse.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "VirtualMemberNeverOverridden.Global" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    public class GptResponse : GptResponseBase, IDisposable
    {
        /// <summary>
        /// The choices
        /// </summary>
        private protected IList<GptChoice> _choices;

        /// <summary>
        /// The usage
        /// </summary>
        private protected GptUsage _usage;

        /// <summary>
        /// The error
        /// </summary>
        private protected GptError _error;

        /// <summary>
        /// The status
        /// </summary>
        private protected string _status;

        /// <summary>
        /// The meta data
        /// </summary>
        private protected IDictionary<string, string> _metaData;

        /// <summary>
        /// The maximum tool calls
        /// </summary>
        private protected int _maxToolCalls;

        /// <summary>
        /// The output
        /// </summary>
        private protected IList<string> _output;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptResponse" /> class.
        /// </summary>
        public GptResponse( )
            : base( )
        {
            _object = "response";
            _metaData = new Dictionary<string, string>( );
            _output = new List<string>( );
            _choices = new List<GptChoice>( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bubba.GptResponse" /> class.
        /// </summary>
        /// <param name="response">The request.</param>
        public GptResponse( GptResponse response )
            : this( )
        {
            _model = response.Model;
            _object = response.Object;
            _maxToolCalls = response.MaxToolCalls;
            _choices = response.Choices;
            _output = response.Output;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [ JsonPropertyName( "id" ) ]
        public override string Id
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
        /// Gets or sets the maximum output tokens.
        /// </summary>
        /// <value>
        /// The maximum output tokens.
        /// </value>
        [ JsonPropertyName( "max_output_tokens" ) ]
        public virtual int MaxOutputTokens
        {
            get
            {
                return _maxOutputTokens;
            }
            set
            {
                if( _maxOutputTokens != value )
                {
                    _maxOutputTokens = value;
                    OnPropertyChanged( nameof( MaxOutputTokens ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
        [ JsonPropertyName( "metadata" ) ]
        public virtual IDictionary<string, string> MetaData
        {
            get
            {
                return _metaData;
            }
            set
            {
                if( _metaData != value )
                {
                    _metaData = value;
                    OnPropertyChanged( nameof( MetaData ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum tool calls.
        /// </summary>
        /// <value>
        /// The maximum tool calls.
        /// </value>
        [ JsonPropertyName( "max_tool_calls" ) ]
        public int MaxToolCalls
        {
            get
            {
                return _maxToolCalls;
            }
            set
            {
                if( _maxToolCalls != value )
                {
                    _maxToolCalls = value;
                    OnPropertyChanged( nameof( MaxToolCalls ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        [ JsonPropertyName( "object" ) ]
        public override string Object
        {
            get
            {
                return _object;
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [ JsonPropertyName( "status" ) ] 
        public virtual string Status
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
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        [ JsonPropertyName( "created_at" ) ]
        public override DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                if( _created != value )
                {
                    _created = value;
                    OnPropertyChanged( nameof( Created ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        [ JsonPropertyName( "model" ) ]
        public virtual string Model
        {
            get
            {
                return _model;
            }
            set
            {
                if( _model != value )
                {
                    _model = value;
                    OnPropertyChanged( nameof( Model ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the choices.
        /// </summary>
        /// <value>
        /// The choices.
        /// </value>
        [ JsonPropertyName( "choices" ) ]
        public virtual IList<GptChoice> Choices
        {
            get
            {
                return _choices;
            }
            set
            {
                if( _choices != value )
                {
                    _choices = value;
                    OnPropertyChanged( nameof( Choices ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the usage.
        /// </summary>
        /// <value>
        /// The usage.
        /// </value>
        [ JsonPropertyName( "usage" ) ]
        public virtual GptUsage Usage
        {
            get
            {
                return _usage;
            }
            set
            {
                if( _usage != value )
                {
                    _usage = value;
                    OnPropertyChanged( nameof( Usage ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public virtual IList<string> Output
        {
            get
            {
                return _output;
            }
            set
            {
                if( _output != value )
                {
                    _output = value;
                    OnPropertyChanged( nameof( Output ) );
                }
            }
        }

        /// <summary>
        /// Extracts the message from response.
        /// </summary>
        /// <param name="response">The json response.</param>
        /// <returns></returns>
        private protected virtual string ExtractContent( string response )
        {
            try
            {
                ThrowIf.Empty( response, nameof( response ) );
                using var _jsonDocument = JsonDocument.Parse( response );
                var _root = _jsonDocument.RootElement;
                if( _model.Contains( "gpt-3.5-turbo" ) )
                {
                    var _property = _root.GetProperty( "choices" );
                    if( _property.ValueKind == JsonValueKind.Array
                        && _property.GetArrayLength( ) > 0 )
                    {
                        var _msg = _property[ 0 ].GetProperty( "message" );
                        var _cnt = _msg.GetProperty( "content" );
                        var _txt = _cnt.GetString( );
                        return _txt;
                    }
                    else
                    {
                        var _message = _property[ 0 ].GetProperty( "message" );
                        var _text = _message.GetString( );
                        return _text;
                    }
                }
                else
                {
                    var _choice = _root.GetProperty( "choices" )[ 0 ];
                    var _property = _choice.GetProperty( "text" );
                    var _text = _property.GetString( );
                    return _text;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, object> GetData( )
        {
            try
            {
                _data.Add( "model", $"{_model}" );
                _data.Add( "id", $"{_id}" );
                _data.Add( "max_output_tokens", $"{_maxOutputTokens}" );
                _data.Add( "status", $"{_status}" );
                _data.Add( "created", $"{_created}" );
                _data.Add( "object",  $"{_object}" );
                _data.Add( "usage",  $"{_usage}" );
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

        /// <inheritdoc />
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c>
        /// to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        private protected void Dispose( bool disposing )
        {
            if( disposing )
            {
                Dispose( );
            }
        }
    }
}