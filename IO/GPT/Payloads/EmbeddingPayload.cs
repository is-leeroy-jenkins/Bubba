// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-11-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-11-2025
// ******************************************************************************************
// <copyright file="EmbeddingPayload.cs" company="Terry D. Eppler">
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
//   EmbeddingPayload.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class EmbeddingPayload : GptPayload
    {
        /// <summary>
        /// The file path
        /// </summary>
        private protected string _filePath;

        /// <summary>
        /// The input
        /// </summary>
        private protected string _input;

        /// <summary>
        /// The encoding format
        /// </summary>
        private protected string _encodingFormat;

        /// <summary>
        /// The dimensions
        /// </summary>
        private protected int _dimensions;

        /// <summary>
        /// The end point
        /// </summary>
        private protected string _endPoint;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EmbeddingPayload"/> class.
        /// </summary>
        /// <inheritdoc />
        public EmbeddingPayload( )
            : base( )
        {
            _model = "text-embedding-3";
            _endPoint = GptEndPoint.Embeddings;
            _responseFormat = "text";
            _encodingFormat = "float";
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        [ JsonPropertyName( "filepath" ) ]
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                if( _filePath != value )
                {
                    _filePath = value;
                    OnPropertyChanged( nameof( FilePath ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        public string EndPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                if( _endPoint != value )
                {
                    _endPoint = value;
                    OnPropertyChanged( nameof( EndPoint ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the response format.
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
        [ JsonPropertyName( "response_format" ) ]
        public override string ResponseFormat
        {
            get
            {
                return _responseFormat;
            }
            set
            {
                if( _responseFormat != value )
                {
                    _responseFormat = value;
                    OnPropertyChanged( nameof( ResponseFormat ) );
                }
            }
        }

        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        /// <inheritdoc />
        [ JsonPropertyName( "model" ) ]
        public override string Model
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
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        [ JsonPropertyName( "input" ) ]
        public string Input
        {
            get
            {
                return _input;
            }
            set
            {
                if( _input != value )
                {
                    _input = value;
                    OnPropertyChanged( nameof( Input ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// </returns>
        public override IDictionary<string, object> GetData( )
        {
            try
            {
                _data.Add( "model", _model );
                _data.Add( "number", _number );
                _data.Add( "max_completionTokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", Temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", TopPercent );
                _data.Add( "response_format", _responseFormat );
                _stop.Add( "#" );
                _stop.Add( ";" );
                _data.Add( "stop", _stop );
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
    }
}