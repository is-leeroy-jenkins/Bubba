// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-12-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-12-2025
// ******************************************************************************************
// <copyright file="TranscriptionRequest.cs" company="Terry D. Eppler">
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
//   TranscriptionRequest.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "UseAwaitUsing" ) ]
    public class TranscriptionRequest : GptRequest
    {
        /// <summary>
        /// The language
        /// </summary>
        private protected string _language;

        /// <summary>
        /// The file path
        /// </summary>
        private protected string _file;

        /// <summary>
        /// The audio data
        /// </summary>
        private protected byte[ ] _audioData;

        /// <summary>
        /// The speed
        /// </summary>
        private protected int _speed;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TranscriptionRequest"/> class.
        /// </summary>
        /// <inheritdoc />
        public TranscriptionRequest( )
            : base( )
        {
            _entry = new object( );
            _httpClient = new HttpClient( );
            _language = "en";
            _model = "whisper-1";
            _endPoint = GptEndPoint.Transcriptions;
            _responseFormat = "text";
            _temperature = 0.18;
            _number = 1;
            _speed = 1;
            _modalities = "['text','audio']";
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

        /// <summary>
        /// The path to the audio file object (not file name) to transcribe,
        /// in one of these formats: flac, mp3, mp4, mpeg, mpga, m4a, ogg, wav, or webm.
        /// </summary>
        /// <value>
        /// The file.
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
        /// Gets or sets the audio data.
        /// </summary>
        /// <value>
        /// The audio data.
        /// </value>
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

        /// <summary>
        /// The language of the input audio.
        /// Supplying the input language in ISO-639-1
        /// format will improve accuracy and latency.
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

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        [ JsonProperty( "prompt" ) ]
        public string Prompt
        {
            get
            {
                return _prompt;
            }
            set
            {
                if( _prompt != value )
                {
                    _prompt = value;
                    OnPropertyChanged( nameof( Prompt ) );
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

        /// <inheritdoc />
        /// <summary>
        /// The sampling temperature, between 0 and 1.
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// If set to 0, the model will use log probability to automatically increase
        /// the temperature until certain thresholds are hit.
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

        /// <summary>
        /// Transcribes the audio asynchronous.
        /// </summary>
        /// <param name="filePath">
        /// The file path.</param>
        /// <returns></returns>
        public async Task<string> TranscribeAudioAsync( string filePath )
        {
            using var _client = new HttpClient( );
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue( "Bearer", OpenAI.BubbaKey );

            using var _fileStream = new FileStream( filePath, FileMode.Open, FileAccess.Read );
            var _fileContent = new StreamContent( _fileStream );
            _fileContent.Headers.ContentType = new MediaTypeHeaderValue( "audio/mpeg" );
            var _formData = new MultipartFormDataContent
            {
                {
                    _fileContent, "file", Path.GetFileName( filePath )
                },
                {
                    new StringContent( "whisper-1" ), "model"
                }
            };

            var _response = await _client.PostAsync( _endPoint, _formData );
            _response.EnsureSuccessStatusCode( );
            var _responseContent = await _response.Content.ReadAsStringAsync( );
            return ParseTranscription( _responseContent );
        }

        /// <summary>
        /// Parses the transcription.
        /// </summary>
        /// <param name="jsonResponse">The json response.</param>
        /// <returns></returns>
        private string ParseTranscription( string jsonResponse )
        {
            using var _document = JsonDocument.Parse( jsonResponse );
            return _document.RootElement.GetProperty( "text" ).GetString( );
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
                _data.Add( "endpoint", _endPoint );
                _data.Add( "speed", _speed );
                _data.Add( "language", _language );
                _data.Add( "modalities", _modalities );
                if( !string.IsNullOrEmpty( _file ) )
                {
                    _data.Add( "filepath", _file );
                }

                if( !string.IsNullOrEmpty( _language ) )
                {
                    _data.Add( "language", _language );
                }

                if( _audioData?.Any( ) == true )
                {
                    _data.Add( "audio_data", _audioData );
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