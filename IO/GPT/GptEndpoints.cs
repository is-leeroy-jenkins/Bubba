// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-06-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-06-2025
// ******************************************************************************************
// <copyright file="GptEndpoints.cs" company="Terry D. Eppler">
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
//   GptEndpoints.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.PropertyChangedBase" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberInitializerValueIgnored" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class GptEndpoints : PropertyChangedBase
    {
        /// <summary>
        /// The domain
        /// </summary>
        private protected string _baseUrl;

        /// <summary>
        /// The locations
        /// </summary>
        private protected IList<string> _all = new List<string>( );

        /// <summary>
        /// The Completions API
        /// </summary>
        private protected string _textGeneration;

        /// <summary>
        /// The assistant
        /// </summary>
        private protected string _assistants;

        /// <summary>
        /// The speech
        /// </summary>
        private protected string _speechGeneration;

        /// <summary>
        /// The translations
        /// </summary>
        private protected string _translations;

        /// <summary>
        /// The transcriptions
        /// </summary>
        private protected string _transcriptions;

        /// <summary>
        /// The jobs
        /// </summary>
        private protected string _fineTuning;

        /// <summary>
        /// The files
        /// </summary>
        private protected string _files;

        /// <summary>
        /// The uploads
        /// </summary>
        private protected string _uploads;

        /// <summary>
        /// The vector stores
        /// </summary>
        private protected string _vectorStores;

        /// <summary>
        /// The projects
        /// </summary>
        private protected string _projects;

        /// <summary>
        /// The generations
        /// </summary>
        private protected string _imageGeneration;

        /// <summary>
        /// The vector embedding
        /// </summary>
        private protected string _vectorEmbeddings;

        /// <summary>
        /// The data
        /// </summary>
        private protected IDictionary<string, string> _data;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptEndpoints"/> class.
        /// </summary>
        public GptEndpoints( )
        {
            _baseUrl = @"https://api.openai.com/";
            _textGeneration = "https://api.openai.com/v1/chat/completions";
            _assistants = "https://api.openai.com/v1/assistants";
            _speechGeneration = "https://api.openai.com/v1/audio/speech";
            _translations = "https://api.openai.com/v1/audio/translations";
            _transcriptions = "https://api.openai.com/v1/audio/transcriptions";
            _fineTuning = "https://api.openai.com/v1/fine_tuning/jobs";
            _files = "https://api.openai.com/v1/files";
            _uploads = "https://api.openai.com/v1/uploads";
            _vectorEmbeddings = "https://api.openai.com/v1/embeddings";
            _vectorStores = "https://api.openai.com/v1/vector_stores";
            _projects = "https://api.openai.com/v1/organization/projects";
            _imageGeneration = "https://api.openai.com/v1/images/generations";
            _data = CreateData( );
            _all = GetAll( );
        }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        public string BaseUrl
        {
            get
            {
                return _baseUrl;
            }
        }

        /// <summary>
        /// Gets the locations.
        /// </summary>
        /// <value>
        /// The locations.
        /// </value>
        public IList<string> All
        {
            get
            {
                return _all;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IDictionary<string, string> Data
        {
            get
            {
                return _data;
            }
        }

        /// <summary>
        /// Gets the completions.
        /// </summary>
        /// <value>
        /// The completions.
        /// </value>
        public string TextGeneration
        {
            get
            {
                return _textGeneration;
            }
        }

        /// <summary>
        /// Gets the assistant.
        /// </summary>
        /// <value>
        /// The assistant.
        /// </value>
        public string Assistants
        {
            get
            {
                return _assistants;
            }
        }

        /// <summary>
        /// Gets the speech.
        /// </summary>
        /// <value>
        /// The speech.
        /// </value>
        public string SpeechGeneration
        {
            get
            {
                return _speechGeneration;
            }
        }

        /// <summary>
        /// Gets the translations.
        /// </summary>
        /// <value>
        /// The translations.
        /// </value>
        public string Translations
        {
            get
            {
                return _translations;
            }
        }

        /// <summary>
        /// Gets the translations.
        /// </summary>
        /// <value>
        /// The translations.
        /// </value>
        public string Transcriptions
        {
            get
            {
                return _transcriptions;
            }
        }

        /// <summary>
        /// Gets the jobs.
        /// </summary>
        /// <value>
        /// The jobs.
        /// </value>
        public string FineTuning
        {
            get
            {
                return _fineTuning;
            }
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <value>
        /// The files.
        /// </value>
        public string Files
        {
            get
            {
                return _files;
            }
        }

        /// <summary>
        /// Gets the uploads.
        /// </summary>
        /// <value>
        /// The uploads.
        /// </value>
        public string Uploads
        {
            get
            {
                return _uploads;
            }
        }

        /// <summary>
        /// Gets the vector stores.
        /// </summary>
        /// <value>
        /// The vector stores.
        /// </value>
        public string VectorStores
        {
            get
            {
                return _vectorStores;
            }
        }

        /// <summary>
        /// Gets the vector embedding.
        /// </summary>
        /// <value>
        /// The vector embedding.
        /// </value>
        public string VectorEmbeddings
        {
            get
            {
                return _vectorEmbeddings;
            }
        }

        /// <summary>
        /// Gets the projects.
        /// </summary>
        /// <value>
        /// The projects.
        /// </value>
        public string Projects
        {
            get
            {
                return _projects;
            }
        }

        /// <summary>
        /// Gets the image generations.
        /// </summary>
        /// <value>
        /// The image generations.
        /// </value>
        public string ImageGeneration
        {
            get
            {
                return _imageGeneration;
            }
        }

        /// <summary>
        /// Creates the data.
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> CreateData( )
        {
            try
            {
                var _urls = new Dictionary<string, string>( );
                _urls.Add( "Text Generation", _textGeneration );
                _urls.Add( "Translations", _translations );
                _urls.Add( "Image Generation", _imageGeneration );
                _urls.Add( "Vector Embeddings", _vectorEmbeddings );
                _urls.Add( "Transcriptions", _transcriptions );
                _urls.Add( "Vector Stores", _vectorStores );
                _urls.Add( "Speech Generation", _speechGeneration );
                _urls.Add( "Fine Tuning", _fineTuning );
                _urls.Add( "Files", _files );
                _urls.Add( "Uploads", _uploads );
                _urls.Add( "Projects", _projects );
                return _urls;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IDictionary<string, string> );
            }
        }

        /// <summary>
        /// Gets the end points.
        /// </summary>
        /// <returns></returns>
        private protected IList<string> GetAll( )
        {
            _all.Add( "v1/chat/completions" );
            _all.Add( "v1/completions" );
            _all.Add( "v1/assistants" );
            _all.Add( "v1/audio/speech" );
            _all.Add( "v1/audio/translations" );
            _all.Add( "v1/fine_tuning/jobs" );
            _all.Add( "v1/files" );
            _all.Add( "v1/uploads" );
            _all.Add( "v1/images/generations" );
            _all.Add( "v1/images/variations" );
            _all.Add( "v1/threads" );
            _all.Add( "v1/threads/runs" );
            _all.Add( "v1/vector_stores" );
            _all.Add( "v1/organization/invites" );
            _all.Add( "v1/organization/users" );
            _all.Add( "v1/organization/projects" );
            return _all?.Any( ) == true
                ? _all
                : default( IList<string> );
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}