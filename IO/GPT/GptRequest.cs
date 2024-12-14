// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 12-11-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        12-11-2024
// ******************************************************************************************
// <copyright file="GptRequest.cs" company="Terry D. Eppler">
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
//   GptRequest.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;

    /// <inheritdoc />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" ) ]
    public class GptRequest : GptRequestBase
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptRequest" /> class.
        /// </summary>
        public GptRequest( )
        {
            _entry = new object( );
            _presence = 0.0;
            _frequency = 0.0;
            _topPercent = 1.0;
            _temperature = 1.0;
            _maximumCompletionTokens = 2048;
            _model = "gpt-4o";
            _endPoint = "https://api.openai.com/v1/chat/completions";
            _number = 1;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptRequest" /> class.
        /// </summary>
        /// <param name = "user" > </param>
        /// <param name = "system" > </param>
        /// <param name = "model" > </param>
        /// <param name = "store" > </param>
        /// <param name = "stream" > </param>
        /// <param name="number">The identifier.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="presence">The presence.</param>
        /// <param name = "topPercent" > </param>
        /// <param name="temperature">The temperature.</param>
        /// <param name="completionTokens"</param>
        public GptRequest( string user, string system, string model = "gpt-4o",
            bool store = false, bool stream = false,
            int number = 1, double frequency = 0.0, double presence = 0.0,
            double topPercent = 1.0, double temperature = 1.0, int completionTokens = 2048 )
        {
            _header = new GptHeader( );
            _endPoint = new GptEndPoint( ).TextGeneration;
            _systemPrompt = system;
            _userPrompt = user;
            _model = model;
            _store = store;
            _stream = stream;
            _number = number;
            _presence = presence;
            _frequency = frequency;
            _temperature = temperature;
            _topPercent = topPercent;
            _maximumCompletionTokens = completionTokens;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptRequest" /> class.
        /// </summary>
        /// <param name="gptRequest">The GPT request.</param>
        public GptRequest( GptRequest gptRequest )
        {
            _header = new GptHeader( );
            _endPoint = gptRequest.EndPoint;
            _systemPrompt = gptRequest.SystemPrompt;
            _userPrompt = gptRequest.UserPrompt;
            _store = gptRequest.Store;
            _stream = gptRequest.Stream;
            _number = gptRequest.Number;
            _presence = gptRequest.Presence;
            _frequency = gptRequest.Frequency;
            _temperature = gptRequest.Temperature;
            _topPercent = gptRequest.TopPercent;
            _maximumCompletionTokens = gptRequest.MaximumCompletionTokens;
        }

        /// <summary>
        /// Deconstructs the specified user identifier.
        /// </summary>
        /// <param name = "header" > </param>
        /// <param name = "endPoint" > </param>
        /// <param name = "system" > </param>
        /// <param name = "user" > </param>
        /// <param name = "store" > </param>
        /// <param name = "stream" > </param>
        /// <param name = "model" > </param>
        /// <param name="number">The user identifier.</param>
        /// <param name="presence">The presence.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name = "topPercent" > </param>
        /// <param name="tokens">The maximum tokens.</param>
        public void Deconstruct( out GptHeader header, out string endPoint, out string system,
            out string user, out bool store, out bool stream,
            out string model, out int number, out double presence, 
            out double frequency, out double temperature,
            out double topPercent, out int tokens )
        {
            header = _header;
            endPoint = _endPoint;
            system = _systemPrompt;
            user = _userPrompt;
            store = _store;
            stream = _stream;
            model = _model;
            number = _number;
            presence = _presence;
            frequency = _frequency;
            temperature = _temperature;
            topPercent = _topPercent;
            tokens = _maximumCompletionTokens;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="P:Bubba.GptRequest.HttpClient" /> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        public override HttpClient HttpClient
        {
            get
            {
                return _httpClient;
            }
            set
            {
                if( _httpClient != value )
                {
                    _httpClient = value;
                    OnPropertyChanged( nameof( HttpClient ) );
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
    }
}