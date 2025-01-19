// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-11-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-11-2025
// ******************************************************************************************
// <copyright file="TranslationPayload.cs" company="Terry D. Eppler">
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
//   TranslationPayload.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.Json.Serialization;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptPayload" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class TranslationPayload : GptPayload
    {
        /// <summary>
        /// The file
        /// </summary>
        private protected object _file;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TranslationPayload"/> class.
        /// </summary>
        /// <inheritdoc />
        public TranslationPayload( )
            : base( )
        {
            _model = "whisper-1";
            _endPoint = GptEndPoint.Translations;
            _responseFormat = "text";
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="userPrompt"></param>
        /// <param name="frequency"></param>
        /// <param name="presence"></param>
        /// <param name="temperature"></param>
        /// <param name="topPercent"></param>
        /// <param name="maxTokens"></param>
        /// <param name="store"></param>
        /// <param name="stream"></param>
        public TranslationPayload( string userPrompt, double frequency = 0.00, double presence = 0.00,
            double temperature = 0.18, double topPercent = 0.11, int maxTokens = 2048,
            bool store = false, bool stream = true ) 
            : this( )
        {
            _prompt = userPrompt;
            _temperature = temperature;
            _maximumTokens = maxTokens;
            _frequencyPenalty = frequency;
            _presencePenalty = presence;
            _store = store;
            _stream = stream;
            _topPercent = topPercent;
            _stop = new List<string>( );
            _messages = new List<IGptMessage>( );
            _data = new Dictionary<string, object>( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bubba.TranslationPayload" /> class.
        /// </summary>
        /// <param name="userPrompt">The user prompt.</param>
        /// <param name="config">The configuration.</param>
        public TranslationPayload( string userPrompt, GptParameter config ) 
            : this( )
        {
            _prompt = userPrompt;
            _temperature = config.Temperature;
            _maximumTokens = config.MaximumTokens;
            _frequencyPenalty = config.FrequencyPenalty;
            _presencePenalty = config.PresencePenalty;
            _store = config.Store;
            _stream = config.Stream;
            _topPercent = config.TopPercent;
            _stop = config.Stop;
            _messages = new List<IGptMessage>( );
            _data = new Dictionary<string, object>( );
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        [ JsonPropertyName( "file" ) ]
        public object File
        {
            get
            {
                return _file;
            }
            set
            {
                if( _file != value )
                {
                    _file = value;
                    OnPropertyChanged( nameof( File ) );
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
                _data.Add( "model", _model );
                _data.Add( "endpoint", _endPoint );
                _data.Add( "max_completionTokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
                _data.Add( "response_format", _responseFormat );
                _data.Add( "modalities", _modalities );
                _stop.Add( "#" );
                _stop.Add( ";" );
                _data.Add( "stop", _stop );
                if( _messages?.Any( ) == true )
                {
                    _data.Add( "messages", _messages );
                }

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