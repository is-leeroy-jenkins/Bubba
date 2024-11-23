// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-22-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-22-2024
// ******************************************************************************************
// <copyright file="GptClientFactory.cs" company="Terry D. Eppler">
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
//   GptClientFactory.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System.Diagnostics.CodeAnalysis;
    using OpenAI;
    using OpenAI.Assistants;
    using OpenAI.Audio;
    using OpenAI.Chat;
    using OpenAI.Files;
    using OpenAI.FineTuning;
    using OpenAI.Images;
    using OpenAI.VectorStores;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Bubba.GptBase" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MergeConditionalExpression" ) ]
    public class GptClientFactory : GptBase
    {
        private const string KEY = "sk-proj-qW9o_PoT2CleBXOErbGxe2UlOeHtgJ9K-"
            + "rVFooUImScUvXn44e4R9ivYZtbYh5OIObWepnxCGET3BlbkFJykj4Dt9MDZT2GQg"
            + "NarXOifdSxGwmodYtevUniudDGt8vkUNmxurKO9DkULeAUVz3rdY9g_-OsA";

        /// <summary>
        /// The client
        /// </summary>
        private OpenAIClient _client;

        /// <summary>
        /// The user prompt
        /// </summary>
        private string _userPrompt;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptClient" /> class.
        /// </summary>
        public GptClientFactory( )
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
        public GptClientFactory( string model )
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
                return _vectorClient != null
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
                return _imageClient != null
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
                return _fileClient != null
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
                return _audioClient != null
                    ? _audioClient
                    : default( AudioClient );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( AudioClient );
            }
        }
    }
}