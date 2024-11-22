// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-21-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-21-2024
// ******************************************************************************************
// <copyright file="GptClient.cs" company="Terry D. Eppler">
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
//   GptClient.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using OpenAI;
    using OpenAI.Chat;
    using OpenAI.Files;
    using OpenAI.Images;
    using OpenAI.Assistants;
    using OpenAI.Audio;
    using OpenAI.FineTuning;
    using OpenAI.VectorStores;
    using Exception = System.Exception;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MergeConditionalExpression" ) ]
    public class GptClient : GptBase
    {
        private OpenAIClient _client;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptClient" /> class.
        /// </summary>
        public GptClient( )
            : base( )
        {
            _entry = new object( );
            _apiKey = KEY;
            _model = "gpt-3.5-turbo";
            _client = new OpenAIClient( _apiKey );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptClient" /> class.
        /// </summary>
        /// <param name="model">The chat model.</param>
        public GptClient( string model )
        {
            _entry = new object( );
            _apiKey = KEY;
            _model = model;
            _client = new OpenAIClient( _apiKey );
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        public string Model
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
        /// Gets the prompt.
        /// </summary>
        /// <value>
        /// The prompt.
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

        /// <summary>
        /// Creates the chat client.
        /// </summary>
        /// <returns></returns>
        public ChatClient CreateChatClient( )
        {
            try
            {
                return new ChatClient( "gpt-4o", _apiKey );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( ChatClient );
            }
        }

        /// <summary>
        /// Creates the chat stream asynchronous.
        /// </summary>
        public async Task CreateChatStreamAsync( )
        {
            var _streamClient = new ChatClient( _model, _apiKey );
            var _updates = _streamClient.CompleteChatStreamingAsync( "Say 'this is a test.'" );
            await foreach( var _update in _updates )
            {
                if( _update.ContentUpdate.Count > 0 )
                {
                    Console.Write( _update.ContentUpdate[ 0 ].Text );
                }
            }
        }

        /// <summary>
        /// Creates the assistant client.
        /// </summary>
        /// <returns></returns>
        [ Experimental( "OPENAI001" ) ]
        public AssistantClient CreateAssistantClient( )
        {
            try
            {
                var _assistClient = new OpenAIClient( _apiKey );
                var _assistant = _assistClient.GetAssistantClient( );
                return _assistant;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( AssistantClient );
            }
        }

        /// <summary>
        /// Creates the fine tuning client.
        /// </summary>
        /// <returns></returns>
        [ Experimental( "OPENAI001" ) ]
        public FineTuningClient CreateFineTuningClient( )
        {
            try
            {
                var _fineTuner = new OpenAIClient( _apiKey );
                var _fineClient = _fineTuner.GetFineTuningClient( );
                return _fineClient;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( FineTuningClient );
            }
        }

        /// <summary>
        /// Creates the vector store client.
        /// </summary>
        /// <returns></returns>
        [ Experimental( "OPENAI001" ) ]
        public VectorStoreClient CreateVectorStoreClient( )
        {
            try
            {
                var _vectorStore = new OpenAIClient( _apiKey );
                var _vectorClient = _vectorStore.GetVectorStoreClient( );
                return ( _vectorClient != null )
                    ? _vectorClient
                    : default( VectorStoreClient );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( VectorStoreClient );
            }
        }

        /// <summary>
        /// Creates the image client.
        /// </summary>
        /// <returns></returns>
        public ImageClient CreateImageClient( )
        {
            try
            {
                var _imageClient = new ImageClient( "dall-e-3", _apiKey );
                return ( _imageClient != null )
                    ? _imageClient
                    : default( ImageClient );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( ImageClient );
            }
        }

        /// <summary>
        /// Creates the file client.
        /// </summary>
        /// <returns></returns>
        public OpenAIFileClient CreateFileClient( )
        {
            try
            {
                var _fileClient = new OpenAIFileClient( _apiKey );
                return ( _fileClient != null )
                    ? _fileClient
                    : default( OpenAIFileClient );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( OpenAIFileClient );
            }
        }

        /// <summary>
        /// Creates the audio client.
        /// </summary>
        /// <returns></returns>
        public AudioClient CreateAudioClient( )
        {
            try
            {
                var _audioClient = new AudioClient( "tts-1", _apiKey );
                return ( _audioClient != null )
                    ? _audioClient
                    : default( AudioClient );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( AudioClient );
            }
        }

        /// <summary>
        /// Sends the HTTP message.
        /// </summary>
        /// <param name="userPrompt">The userPrompt.</param>
        /// <returns></returns>
        public string SendHttpMessage( string userPrompt )
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
                | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            // text-davinci-002, text-davinci-003
            var _url = "https://api.openai.com/v1/completions";
            if( _model.IndexOf( "gpt-3.5-turbo" ) != -1 )
            {
                //Chat GTP 4 https://openai.com/research/gpt-4
                _url = "https://api.openai.com/v1/chat/completions";
            }

            var _request = WebRequest.Create( _url );
            _request.Method = "POST";
            _request.ContentType = "application/json";
            _request.Headers.Add( "Authorization", "Bearer " + App.KEY );
            var _maxTokens = 2048;// 2048
            var _temp = 0.5;      // 0.5
            var _userId = 1;
            var _data = "";
            if( _model.IndexOf( "gpt-3.5-turbo" ) != -1 )
            {
                _data = "{";
                _data += " \"model\":\"" + _model + "\",";
                _data += " \"messages\": [{\"role\": \"user\", \"content\": \""
                    + PadQuotes( userPrompt ) + "\"}]";

                _data += "}";
            }
            else
            {
                _data = "{";
                _data += " \"model\":\"" + _model + "\",";
                _data += " \"prompt\": \"" + PadQuotes( userPrompt ) + "\",";
                _data += " \"max_tokens\": " + _maxTokens + ",";
                _data += " \"user\": \"" + _userId + "\", ";
                _data += " \"temperature\": " + _temp + ", ";

                // Number between -2.0 and 2.0  Positive value decrease the
                // model's likelihood to repeat the same line verbatim.
                _data += " \"frequency_penalty\": 0.0" + ", ";

                // Number between -2.0 and 2.0. Positive values increase the model's
                // likelihood to talk about new topics.
                _data += " \"presence_penalty\": 0.0" + ", ";

                // Up to 4 sequences where the API will stop generating further tokens.
                // The returned text will not contain the stop sequence.
                _data += " \"stop\": [\"#\", \";\"]";
                _data += "}";
            }

            using var _streamWriter = new StreamWriter( _request.GetRequestStream( ) );
            _streamWriter.Write( _data );
            _streamWriter.Flush( );
            _streamWriter.Close( );
            var _response = _request.GetResponse( );
            var _stream = _response.GetResponseStream( );
            if( _stream != null )
            {
                var _reader = new StreamReader( _stream );
                var _json = _reader.ReadToEnd( );
            }

            var _objects = new Dictionary<string, object>( );
            var _choices = _objects.Keys.ToList( );
            var _choice = _choices[ 0 ];
            var _message = "";
            if( _model.IndexOf( "gpt-3.5-turbo" ) != -1 )
            {
                var _key = _objects[ "message" ];
                var _kvp = new Dictionary<string, object>( );
            }
            else
            {
                _message = ( string )_objects[ "text" ];
            }

            return _message;
        }
    }
}