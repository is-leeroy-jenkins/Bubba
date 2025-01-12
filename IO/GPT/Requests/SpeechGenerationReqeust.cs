// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-12-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-12-2025
// ******************************************************************************************
// <copyright file="SpeechGenerationReqeust.cs" company="Terry D. Eppler">
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
//   SpeechGenerationReqeust.cs
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
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Properties;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "UseAwaitUsing" ) ]
    [ SuppressMessage( "ReSharper", "RedundantBlankLines" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    public class SpeechGenerationReqeust : GptRequest
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
        /// The language
        /// </summary>
        private protected string _language;

        /// <summary>
        /// The speed
        /// </summary>
        private protected int _speed;

        /// <summary>
        /// The seed
        /// </summary>
        private protected int _seed;

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
        /// <see cref="SpeechGenerationReqeust"/> class.
        /// </summary>
        /// <inheritdoc />
        public SpeechGenerationReqeust( )
            : base( )
        {
            _entry = new object( );
            _httpClient = new HttpClient( );
            _model = "tts-1-hd";
            _endPoint = GptEndPoint.SpeechGeneration;
            _speed = 1;
            _language = "en";
            _responseFormat = "mp3";
            _modalities = "['text', 'audio']";
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
        /// Gets or sets the seed.
        /// </summary>
        /// <value>
        /// The seed.
        /// </value>
        [ JsonProperty( "seed" ) ]
        public int Seed
        {
            get
            {
                return _seed;
            }
            set
            {
                if( _seed != value )
                {
                    _seed = value;
                    OnPropertyChanged( nameof( Seed ) );
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
        /// Gets the data.
        /// </summary>
        /// <returns>
        /// </returns>
        public override IDictionary<string, object> GetData( )
        {
            try
            {
                _data.Add( "model", _model );
                _data.Add( "n", _number );
                _data.Add( "max_completion_tokens", _maximumTokens );
                _data.Add( "store", _store );
                _data.Add( "stream", _stream );
                _data.Add( "temperature", _temperature );
                _data.Add( "frequency_penalty", _frequencyPenalty );
                _data.Add( "presence_penalty", _presencePenalty );
                _data.Add( "top_p", _topPercent );
                _data.Add( "response_format", _responseFormat );
                _data.Add( "endpoint", _endPoint );
                _stop.Add( "#" );
                _stop.Add( ";" );
                _data.Add( "stop", _stop );
                _data.Add( "modalities", _modalities );
                if( _file != null )
                {
                    _data.Add( "file", _file );
                }

                if( !string.IsNullOrEmpty( _input ) )
                {
                    _data.Add( "input", _input );
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
        /// Generates the speech asynchronous.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        /// Task
        /// </returns>
        public async Task<string> GenerateSpeechAsync( string text )
        {
            using var _client = new HttpClient( );
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue( "Bearer", OpenAI.BubbaKey );

            var _payload = new SpeechPayload
            {
                Model = _model,
                Language = _language,
                Input = text
            };

            var _serialize = JsonSerializer.Serialize( _payload );
            var _content = new StringContent( _serialize, Encoding.UTF8, "application/json" );
            var _response = await _client.PostAsync( _endPoint, _content );
            _response.EnsureSuccessStatusCode( );
            var _responseContent = await _response.Content.ReadAsStringAsync( );
            return ExtractSpeech( _responseContent );
        }

        /// <summary>
        /// Parses the generated speech.
        /// </summary>
        /// <param name="jsonResponse">The json response.</param>
        /// <returns>
        /// string
        /// </returns>
        private string ExtractSpeech( string jsonResponse )
        {
            try
            {
                ThrowIf.Empty( jsonResponse, nameof( jsonResponse ) );
                using var _document = JsonDocument.Parse( jsonResponse );
                var _text = _document.RootElement.GetProperty( "text" ).GetString( );
                return _text ?? "Speech generation failed.";
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Saves the speech asynchronous.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="filePath">The file path.</param>
        public async Task SaveSpeechAsync( string text, string filePath )
        {
            try
            {
                ThrowIf.Empty( text, nameof( text ) );
                ThrowIf.Empty( filePath, nameof( filePath ) );
                using var _client = new HttpClient( );
                _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue( "Bearer", OpenAI.BubbaKey );

                var _payload = new SpeechPayload
                {
                    Model = _model,
                    Input = text,
                    Speed = _speed
                };

                var _serialize = JsonSerializer.Serialize( _payload );
                var _content = new StringContent( _serialize, Encoding.UTF8, "application/json" );
                var _response = await _client.PostAsync( _endPoint, _content );
                _response.EnsureSuccessStatusCode( );
                using var _responseStream = await _response.Content.ReadAsStreamAsync( );
                using var _fileStream =
                    new FileStream( filePath, FileMode.Create, FileAccess.Write );

                await _responseStream.CopyToAsync( _fileStream );
            }
            catch( Exception ex )
            {
                Fail( ex );
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