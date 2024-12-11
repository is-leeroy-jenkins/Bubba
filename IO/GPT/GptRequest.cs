// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-25-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-25-2024
// ******************************************************************************************
// <copyright file="GptRequest.cs" company="Terry D. Eppler">
//    Bubba is a small windows (wpf) application for interacting with
//    Chat GPT that's developed in C-Sharp under the MIT license
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

    /// <inheritdoc />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeProtected.Global" ) ]
    public class GptRequest : GptBase
    {
        /// <summary>
        /// The messages
        /// </summary>
        private protected IList<IGptMessage> _messages;

        /// <summary>
        /// The body
        /// </summary>
        private protected GptBody _body;

        /// <summary>
        /// The header
        /// </summary>
        private protected GptHeader _header;

        /// <summary>
        /// The user prompt
        /// </summary>
        private protected string _userPrompt;

        /// <summary>
        /// The assistant prompt
        /// </summary>
        private protected string _systemPrompt;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptRequest" /> class.
        /// </summary>
        public GptRequest( )
        {
            _entry = new object( );
            _header = new GptHeader( );
            _apiKey = _header.ApiKey;
            _presence = 0.0;
            _frequency = 0.0;
            _topPercent = 1.0;
            _temperature = 1.0;
            _maximumCompletionTokens = 157;
            _model = "gpt-4o";
            _endPoint = "https://api.openai.com/v1/chat/completions";
            _responseFormat = "text";
            _number = 1;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptRequest" /> class.
        /// </summary>
        /// <param name = "user" > </param>
        /// <param name = "system" > </param>
        /// <param name = "format" > </param>
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
            string format = "text", bool store = false, bool stream = false,
            int number = 1, double frequency = 0.0, double presence = 0.0,
            double topPercent = 1.0, double temperature = 1.0, int completionTokens = 157 )
        {
            _header = new GptHeader( );
            _endPoint = new GptEndPoint( ).TextGeneration;
            _systemPrompt = system;
            _userPrompt = user;
            _model = model;
            _responseFormat = format;
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
            _responseFormat = gptRequest.ResponseFormat;
            _model = gptRequest.Model;
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
        /// <param name = "model" > </param>
        /// <param name = "format" > </param>
        /// <param name="number">The user identifier.</param>
        /// <param name="frequency">The frequency.</param>
        /// <param name="presence">The presence.</param>
        /// <param name="temperature">The temperature.</param>
        /// <param name = "topPercent" > </param>
        /// <param name="tokens">The maximum tokens.</param>
        /// <param name = "user" > </param>
        /// <param name = "store" > </param>
        /// <param name = "stream" > </param>
        /// <param name = "system" > </param>
        public void Deconstruct( out GptHeader header, out string endPoint, out string system,
            out string user, out bool store, out bool stream,
            out string model, out string format, out int number,
            out double presence, out double frequency, out double temperature,
            out double topPercent, out int tokens )
        {
            header = _header;
            endPoint = _endPoint;
            system = _systemPrompt;
            user = _userPrompt;
            store = _store;
            stream = _stream;
            model = _model;
            format = _responseFormat;
            number = _number;
            presence = _presence;
            frequency = _frequency;
            temperature = _temperature;
            topPercent = _topPercent;
            tokens = _maximumCompletionTokens;
        }

        /// <summary>
        /// Gets or sets the system prompt.
        /// </summary>
        /// <value>
        /// The system prompt.
        /// </value>
        public string SystemPrompt
        {
            get
            {
                return _systemPrompt;
            }
            set
            {
                if( _systemPrompt != value )
                {
                    _systemPrompt = value;
                    OnPropertyChanged( nameof( SystemPrompt ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the user prompt.
        /// </summary>
        /// <value>
        /// The user prompt.
        /// </value>
        public string UserPrompt
        {
            get
            {
                return _userPrompt;
            }
            set
            {
                if( _userPrompt != value )
                {
                    _userPrompt = value;
                    OnPropertyChanged( nameof( UserPrompt ) );
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
        public virtual string EndPoint
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
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
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
        /// An string specifying the format that the model must output.
        /// Important: when using JSON mode, you must also instruct the model
        /// to produce JSON yourself via a system or user message. Without this,
        /// the model may generate an unending stream of whitespace until the
        /// generation reaches the token limit, resulting in a
        /// long-running and seemingly "stuck" request.  
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
        public virtual string ResponseFormat
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
        /// THe number 'n' of responses returned by the API.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public virtual int Number
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

        /// <summary>
        /// Gets the maximum tokens.
        /// </summary>
        /// <value>
        /// The maximum tokens.
        /// </value>
        public virtual int MaximumCompletionTokens
        {
            get
            {
                return _maximumCompletionTokens;
            }
            set
            {
                if( _maximumCompletionTokens != value )
                {
                    _maximumCompletionTokens = value;
                    OnPropertyChanged( nameof( MaximumCompletionTokens ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="GptRequest"/> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Store
        {
            get
            {
                return _store;
            }
            set
            {
                if( _store != value )
                {
                    _store = value;
                    OnPropertyChanged( nameof( Store ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="Stream"/> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
        public virtual bool Stream
        {
            get
            {
                return _stream;
            }
            set
            {
                if( _stream != value )
                {
                    _stream = value;
                    OnPropertyChanged( nameof( Stream ) );
                }
            }
        }

        /// <summary>
        /// A number between -2.0 and 2.0 Positive value decrease the
        /// model's likelihood to repeat the same line verbatim.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        public virtual double Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                if( _temperature != value )
                {
                    _temperature = value;
                    OnPropertyChanged( nameof( Temperature ) );
                }
            }
        }

        /// <summary>
        /// An alternative to sampling with temperature,
        /// called nucleus sampling, where the model considers
        /// the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability
        /// mass are considered. We generally recommend altering this
        /// or temperature but not both.
        /// </summary>
        /// <value>
        /// The top percent.
        /// </value>
        public virtual double TopPercent
        {
            get
            {
                return _topPercent;
            }
            set
            {
                if( _topPercent != value )
                {
                    _topPercent = value;
                    OnPropertyChanged( nameof( TopPercent ) );
                }
            }
        }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public virtual double Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                if( _frequency != value )
                {
                    _frequency = value;
                    OnPropertyChanged( nameof( Frequency ) );
                }
            }
        }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <value>
        /// The presence.
        /// </value>
        public virtual double Presence
        {
            get
            {
                return _presence;
            }
            set
            {
                if( _presence != value )
                {
                    _presence = value;
                    OnPropertyChanged( nameof( Presence ) );
                }
            }
        }

        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        public virtual GptBody Body
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