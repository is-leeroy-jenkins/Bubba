﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-19-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-19-2024
// ******************************************************************************************
// <copyright file="GptEndPoint.cs" company="Terry D. Eppler">
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
//   GptEndPoint.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    public class GptEndPoint : PropertyChangedBase
    {
        /// <summary>
        /// The locations
        /// </summary>
        private protected IList<string> _all;

        /// <summary>
        /// The Completions API
        /// </summary>
        private protected string _completions;

        /// <summary>
        /// The chat completions
        /// </summary>
        private protected string _chatCompletions;

        /// <summary>
        /// The assistant
        /// </summary>
        private protected string _assistant;

        /// <summary>
        /// The speech
        /// </summary>
        private protected string _speech;

        /// <summary>
        /// The translations
        /// </summary>
        private protected string _translations;

        /// <summary>
        /// The jobs
        /// </summary>
        private protected string _jobs;

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
        private protected string _imageGenerations;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptEndPoint"/> class.
        /// </summary>
        public GptEndPoint( )
        {
            _completions = "https://api.openai.com/v1/completions";
            _chatCompletions = "https://api.openai.com/v1/chat/completions";
            _assistant = "https://api.openai.com/v1/assistants";
            _speech = "https://api.openai.com/v1/audio/speech"; 
            _translations = "https://api.openai.com/v1/audio/translations";
            _jobs = "https://api.openai.com/v1/fine_tuning/jobs";
            _files = "https://api.openai.com/v1/files";
            _uploads = "https://api.openai.com/v1/uploads";
            _vectorStores = "https://api.openai.com/v1/vector_stores";
            _projects = "https://api.openai.com/v1/organization/projects";
            _imageGenerations = "https://api.openai.com/v1/images/generations";
            _all = GetLocations( );
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
        /// Gets the completions.
        /// </summary>
        /// <value>
        /// The completions.
        /// </value>
        public string Completions
        {
            get
            {
                return _completions;
            }
        }

        /// <summary>
        /// Gets the chat completions.
        /// </summary>
        /// <value>
        /// The chat completions.
        /// </value>
        public string ChatCompletions
        {
            get
            {
                return _chatCompletions;
            }
        }

        /// <summary>
        /// Gets the assistant.
        /// </summary>
        /// <value>
        /// The assistant.
        /// </value>
        public string Assistant
        {
            get
            {
                return _assistant;
            }
        }

        /// <summary>
        /// Gets the speech.
        /// </summary>
        /// <value>
        /// The speech.
        /// </value>
        public string Speech
        {
            get
            {
                return _assistant;
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
        /// Gets the jobs.
        /// </summary>
        /// <value>
        /// The jobs.
        /// </value>
        public string Jobs
        {
            get
            {
                return _jobs;
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
        public string ImageGenerations
        {
            get
            {
                return _imageGenerations;
            }
        }

        /// <summary>
        /// Gets the end points.
        /// </summary>
        /// <returns></returns>
        private protected IList<string> GetLocations( )
        {
            _all.Add( "https://api.openai.com/v1/chat/completions" );
            _all.Add( "https://api.openai.com/v1/completions" );
            _all.Add( "https://api.openai.com/v1/assistants" );
            _all.Add( "https://api.openai.com/v1/audio/speech" );
            _all.Add( "https://api.openai.com/v1/audio/translations" );
            _all.Add( "https://api.openai.com/v1/fine_tuning/jobs" );
            _all.Add( "https://api.openai.com/v1/files" );
            _all.Add( "https://api.openai.com/v1/uploads" );
            _all.Add( "https://api.openai.com/v1/images/generations" );
            _all.Add( "https://api.openai.com/v1/images/variations" );
            _all.Add( "https://api.openai.com/v1/threads" );
            _all.Add( "https://api.openai.com/v1/threads/runs" );
            _all.Add( "https://api.openai.com/v1/vector_stores" );
            _all.Add( "https://api.openai.com/v1/organization/invites" );
            _all.Add( "https://api.openai.com/v1/organization/users" );
            _all.Add( "https://api.openai.com/v1/organization/projects" );
            return _all?.Any( ) == true
                ? _all
                : default( IList<string> );
        }
    }
}