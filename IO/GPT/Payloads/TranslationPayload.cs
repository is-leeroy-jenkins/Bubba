﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-21-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-21-2025
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
    using System.Text.Json;
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
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    public class TranslationPayload : GptPayload
    {
        /// <summary>
        /// The file
        /// </summary>
        private protected object _audioFile;

        /// <summary>
        /// The respose format
        /// </summary>
        private protected string _resposeFormat;

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
            _store = false;
            _stream = true;
            _number = 1;
            _temperature = 0.18;
            _topPercent = 0.11;
            _frequencyPenalty = 0.00;
            _presencePenalty = 0.00;
            _maximumTokens = 2048;
            _resposeFormat = "json";
            _audioFile = "";
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
        public TranslationPayload( string userPrompt, double frequency = 0.00,
            double presence = 0.00, double temperature = 0.18, double topPercent = 0.11,
            int maxTokens = 2048, bool store = false, bool stream = true )
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
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        [ JsonPropertyName( "file" ) ]
        public object AudioFile
        {
            get
            {
                return _audioFile;
            }
            set
            {
                if( _audioFile != value )
                {
                    _audioFile = value;
                    OnPropertyChanged( nameof( AudioFile ) );
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
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// </returns>
        public string Parse( )
        {
            try
            {
                var _json = "{";
                _json += " \"model\":\"" + _model + "\",";
                _json += " \"n\": \"" + _number + "\", ";
                _json += " \"response_format\": \"" + _resposeFormat + "\", ";
                _json += " \"presence_penalty\": " + _presencePenalty + ", ";
                _json += " \"frequency_penalty\": " + _frequencyPenalty + ", ";
                _json += " \"temperature\": " + _temperature + ", ";
                _json += " \"top_p\": " + _topPercent + ", ";
                _json += " \"store\": \"" + _store + "\", ";
                _json += " \"stream\": \"" + _stream + "\", ";
                _json += " \"max_completion_tokens\": " + _maximumTokens + ",";
                _json += " \"response_format\": " + _resposeFormat + ",";
                _json += " \"stop\": [\"#\", \";\"]" + "\",";
                _json += "}";
                return _json;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String" />
        /// that represents this instance.
        /// </returns>
        public override string ToString( )
        {
            try
            {
                var _text = JsonSerializer.Serialize( this );
                return !string.IsNullOrEmpty( _text )
                    ? _text
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