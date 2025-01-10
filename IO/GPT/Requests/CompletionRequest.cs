// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-09-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-09-2025
// ******************************************************************************************
// <copyright file="CompletionRequest.cs" company="Terry D. Eppler">
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
//   CompletionRequest.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class CompletionRequest : GptRequest
    {
        /// <summary>
        /// Developer-defined tags and values used for filtering completions
        /// </summary>
        private protected IDictionary<string, object> _metaData;

        /// <summary>
        /// The response format
        /// </summary>
        private protected string _responseFormat;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.CompletionRequest" /> class.
        /// </summary>
        public CompletionRequest( )
            : base( )
        {
            _header = new GptHeader( );
            _entry = new object( );
            _httpClient = new HttpClient( );
            _endPoint = GptEndPoint.TextGeneration;
            _model = "gpt-4o-mini";
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.CompletionRequest" /> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public CompletionRequest( GptParameter config )
        {
            _header = new GptHeader( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.CompletionRequest" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="system">The system.</param>
        /// <param name="model">The model.</param>
        /// <param name="format">The format.</param>
        /// <param name="store">if set to <c>true</c> [store].</param>
        /// <param name="stream">if set to <c>true</c> [stream].</param>
        /// <param name="number">The number.</param>
        /// <param name = "frequencyPenalty" > </param>
        /// <param name = "presencePenalty" > </param>
        /// <param name="topPercent">The top percent.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="tokens">The completion tokens.</param>
        public CompletionRequest( string user, string system, string model = "gpt-4o-mini",
            string format = "text", bool store = false, bool stream = false,
            int number = 1, double frequencyPenalty = 0.00, double presencePenalty = 0.00,
            double topPercent = 0.11, double temperature = 0.18, int tokens = 2048 )
        {
            _header = new GptHeader( );
            _endPoint = new GptEndpoints( ).TextGeneration;
            _store = store;
            _stream = stream;
            _number = number;
            _presencePenalty = presencePenalty;
            _frequencyPenalty = frequencyPenalty;
            _temperature = temperature;
            _topPercent = topPercent;
            _maximumTokens = tokens;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.CompletionRequest" /> class.
        /// </summary>
        /// <param name="completionRequest">The completionRequest.</param>
        public CompletionRequest( CompletionRequest completionRequest )
        {
            _header = new GptHeader( );
            _endPoint = completionRequest.EndPoint;
        }

        /// <summary>
        /// Decontructs the specified header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="store">if set to <c>true</c> [store].</param>
        /// <param name="stream">if set to <c>true</c> [stream].</param>
        /// <param name="model">The model.</param>
        /// <param name="number">The number.</param>
        /// <param name="presence">The presence.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="topPercent">The top percent.</param>
        /// <param name="tokens">The tokens.</param>
        public void Decontruct( out GptHeader header, out string endPoint, out bool store,
            out bool stream, out string model, out int number,
            out double presence, out double frequency, out double temperature,
            out double topPercent, out int tokens )
        {
            header = _header;
            endPoint = _endPoint;
            store = _store;
            stream = _stream;
            model = _model;
            number = _number;
            presence = _presencePenalty;
            frequency = _frequencyPenalty;
            temperature = _temperature;
            topPercent = _topPercent;
            tokens = _maximumTokens;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        [ JsonProperty( "messages" ) ]
        public IList<IGptMessage> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                if( _messages != value )
                {
                    _messages = value;
                    OnPropertyChanged( nameof( Messages ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        public override string EndPoint
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
        
        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        /// <inheritdoc />
        [ JsonProperty( "model" ) ]
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

        /// <inheritdoc />
        /// <summary>
        /// THe number 'n' of responses returned by the API.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [ JsonProperty( "n" ) ]
        public override int Number
        {
            get
            {
                return _number;
            }
            set
            {
                if( _number != value )
                {
                    _number = value;
                    OnPropertyChanged( nameof( Number ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the maximum tokens.
        /// </summary>
        /// <value>
        /// The maximum tokens.
        /// </value>
        [ JsonProperty( "max_completion_tokens" ) ]
        public override int MaximumTokens
        {
            get
            {
                return _maximumTokens;
            }
            set
            {
                if( _maximumTokens != value )
                {
                    _maximumTokens = value;
                    OnPropertyChanged( nameof( MaximumTokens ) );
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
        [ JsonProperty( "response_format" ) ]
        public string ResponseFormat
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

        /// <inheritdoc />
        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        public override GptBody Body
        {
            get
            {
                return _body;
            }
            set
            {
                if( _body != value )
                {
                    _body = value;
                    OnPropertyChanged( nameof( Body ) );
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
                _data.Add( "endpoint", _endPoint );
                _data.Add( "number", _number );
                _data.Add( "max_completion_tokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
                _data.Add( "response_format", _responseFormat );
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
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString( )
        {
            try
            {
                return _data?.Any( ) == true
                    ? _data.ToJson( )
                    : string.Empty;
            }
            catch(Exception ex)
            {
                Fail(ex);
                return string.Empty;
            }
        }
    }
}