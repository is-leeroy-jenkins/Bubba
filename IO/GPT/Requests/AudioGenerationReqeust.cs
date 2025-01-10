// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-09-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-09-2025
// ******************************************************************************************
// <copyright file="AudioGenerationReqeust.cs" company="Terry D. Eppler">
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
//   AudioGenerationReqeust.cs
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
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class AudioGenerationReqeust : GptRequest
    {
        /// <summary>
        /// The file
        /// </summary>
        private protected string _file;

        /// <summary>
        /// The voice
        /// </summary>
        private protected string _voice;

        /// <summary>
        /// The modalities
        /// </summary>
        private protected IList<string> _modalities;

        /// <summary>
        /// The response format
        /// </summary>
        private protected string _responseFormat;

        /// <summary>
        /// The language
        /// </summary>
        private protected string _language;

        /// <summary>
        /// The speed
        /// </summary>
        private protected int _speed;

        /// <summary>
        /// The input
        /// </summary>
        private protected string _input;

        /// <summary>
        /// The audio data
        /// </summary>
        private protected byte[ ] _audioData;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AudioGenerationReqeust"/> class.
        /// </summary>
        /// <inheritdoc />
        public AudioGenerationReqeust( )
            : base( )
        {
            _entry = new object( );
            _httpClient = new HttpClient( );
            _modalities = new List<string>( );
            _model = "gpt-4o-mini";
            _endPoint = GptEndPoint.SpeechGeneration;
            _speed = 1;
            _language = "en";
            _responseFormat = "mp3";
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

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [ JsonProperty( "language" ) ]
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                if( _language != value )
                {
                    _language = value;
                    OnPropertyChanged( nameof( Language ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        [ JsonProperty( "input" ) ]
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

        /// <summary>
        /// Gets or sets the voice.
        /// </summary>
        /// <value>
        /// The voice.
        /// </value>
        [ JsonProperty( "voice" ) ]
        public string Voice
        {
            get
            {
                return _voice;
            }
            set
            {
                if( _voice != value )
                {
                    _voice = value;
                    OnPropertyChanged( nameof( Voice ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>
        /// The speed.
        /// </value>
        [ JsonProperty( "speed" ) ]
        public int Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                if( _speed != value )
                {
                    _speed = value;
                    OnPropertyChanged( nameof( Speed ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        [ JsonProperty( "file" ) ]
        public string File
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
        /// Gets or sets the audio data.
        /// </summary>
        /// <value>
        /// The audio data.
        /// </value>
        [ JsonProperty( "audio_data" ) ]
        public byte[ ] AudioData
        {
            get
            {
                return _audioData;
            }
            set
            {
                if( _audioData != value )
                {
                    _audioData = value;
                    OnPropertyChanged( nameof( AudioData ) );
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
                _data.Add( "number", _number );
                _data.Add( "max_completion_tokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
                _data.Add( "response_format", _responseFormat );
                _data.Add( "endpoint", _endPoint );
                _modalities.Add( "text" );
                _modalities.Add( "audio" );
                _data.Add( "modalities", _modalities );
                if( !string.IsNullOrEmpty( _file ) )
                {
                    _data.Add( "file", _file );
                }

                if( !string.IsNullOrEmpty( _language ) )
                {
                    _data.Add( "language", _language );
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

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            try
            {
                return _data?.Any() == true
                    ? _data.ToJson()
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