﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-12-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-12-2025
// ******************************************************************************************
// <copyright file="ChatPayload.cs" company="Terry D. Eppler">
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
//   ChatPayload.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Newtonsoft.Json;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class ChatPayload : TextPayload
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ChatPayload"/> class.
        /// </summary>
        /// <inheritdoc />
        public ChatPayload( )
            : base( )
        {
            _model = "gpt-4o-mini";
            _endPoint = GptEndPoint.Completions;
            _store = false;
            _stream = true;
            _number = 1;
            _temperature = 0.18;
            _topPercent = 0.11;
            _frequencyPenalty = 0.00;
            _presencePenalty = 0.00;
            _maximumTokens = 2048;
            _responseFormat = "text";
        }

        /// <inheritdoc />
        /// <summary>
        /// THe number 'n' of responses generatred.
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
        /// Gets or sets a value indicating whether this
        /// <see cref="T:Bubba.ParameterBase" /> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        [ JsonProperty( "store" ) ]
        public override bool Store
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:Bubba.ParameterBase" /> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
        [ JsonProperty( "stream" ) ]
        public override bool Stream
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

        /// <inheritdoc />
        /// <summary>
        /// A number between 0.0 and 2.0   between 0 and 2.
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        [ JsonProperty( "temperature" ) ]
        public override double Temperature
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

        /// <inheritdoc />
        /// <summary>
        /// A number between -2.0 and 2.0. Positive values penalize new
        /// tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        [ JsonProperty( "frequency_penalty" ) ]
        public override double FrequencyPenalty
        {
            get
            {
                return _frequencyPenalty;
            }
            set
            {
                if( _frequencyPenalty != value )
                {
                    _frequencyPenalty = value;
                    OnPropertyChanged( nameof( FrequencyPenalty ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <value>
        /// The presence.
        /// </value>
        [ JsonProperty( "presence_penalty" ) ]
        public override double PresencePenalty
        {
            get
            {
                return _presencePenalty;
            }
            set
            {
                if( _presencePenalty != value )
                {
                    _presencePenalty = value;
                    OnPropertyChanged( nameof( PresencePenalty ) );
                }
            }
        }

        /// <inheritdoc />
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
        [ JsonProperty( "top_p" ) ]
        public override double TopPercent
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the response format.
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
        [ JsonProperty( "response_format" ) ]
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

        /// <inheritdoc />
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <returns></returns>
        public override IDictionary<string, object> GetData( )
        {
            try
            {
                _data.Add( "n", _number );
                _data.Add( "max_completion_tokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
                _stop.Add( "#" );
                _stop.Add( ";" );
                _data.Add( "stop", _stop );
                _data.Add( "modalities", _modalities );
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
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" /> that represents this instance.
        /// </returns>
        public override string ToString( )
        {
            try
            {
                return _data?.Any( ) == true
                    ? _data.ToJson( )
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