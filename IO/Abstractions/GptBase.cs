// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-18-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-18-2024
// ******************************************************************************************
// <copyright file="GptBase.cs" company="Terry D. Eppler">
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
//   GptBase.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;

    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "VirtualMemberNeverOverridden.Global" ) ]
    public abstract class GptBase : PropertyChangedBase, IDisposable
    {
        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The path
        /// </summary>
        private protected object _entry;

        /// <summary>
        /// The HTTP client
        /// </summary>
        private protected HttpClient _httpClient;

        /// <summary>
        /// The user
        /// </summary>
        private protected string _user;

        /// <summary>
        /// The presence
        /// </summary>
        private protected double _presence;

        /// <summary>
        /// The frequency
        /// </summary>
        private protected double _frequency;

        /// <summary>
        /// The API key
        /// </summary>
        private protected string _apiKey;

        /// <summary>
        /// The base URL
        /// </summary>
        private protected string _endPoint;

        /// <summary>
        /// The chat model
        /// </summary>
        private protected string _model;

        /// <summary>
        /// The urls
        /// </summary>
        private protected IList<string> _endPoints;

        /// <summary>
        /// The models
        /// </summary>
        private protected IList<string> _models;

        /// <summary>
        /// A number between -2.0 and 2.0  Positive value decrease the
        /// model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected double _temperature;

        /// <summary>
        /// The maximum tokens
        /// </summary>
        private protected int _maximumTokens;

        /// <summary>
        /// The role
        /// </summary>
        private protected string _role;

        /// <summary>
        /// The prompt
        /// </summary>
        private protected string _prompt;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptBase"/> class.
        /// </summary>
        protected GptBase( )
        {
        }

        /// <summary>
        /// Gets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        public string ApiKey
        {
            get
            {
                return _apiKey;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value>
        /// <c> true </c>
        /// if this instance is busy; otherwise,
        /// <c> false </c>
        /// </value>
        public bool IsBusy
        {
            get
            {
                lock( _entry )
                {
                    return _busy;
                }
            }
        }

        /// <summary>
        /// Begins the initialize.
        /// </summary>
        private protected void Busy( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = true;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Ends the initialize.
        /// </summary>
        private protected void Chill( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = false;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Pads the quotes.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        private protected string ProcessQuotes( string input )
        {
            if( input.IndexOf( "\\" ) != -1 )
            {
                input = input.Replace( "\\", @"\\" );
            }

            if( input.IndexOf( "\n\r" ) != -1 )
            {
                input = input.Replace( "\n\r", @"\n" );
            }

            if( input.IndexOf( "\r" ) != -1 )
            {
                input = input.Replace( "\r", @"\r" );
            }

            if( input.IndexOf( "\n" ) != -1 )
            {
                input = input.Replace( "\n", @"\n" );
            }

            if( input.IndexOf( "\t" ) != -1 )
            {
                input = input.Replace( "\t", @"\t" );
            }

            if( input.IndexOf( "\"" ) != -1 )
            {
                return input.Replace( "\"", @"""" );
            }
            else
            {
                return input;
            }
        }

        /// <summary>
        /// Gets the available models.
        /// </summary>
        /// <returns>
        /// A list of strings representing OpenAI models
        /// </returns>
        private protected IList<string> GetModels( )
        {
            try
            {
                _models.Add( "gpt-3.5-turbo" );
                _models.Add( "gpt-3.5-turbo-16k" );
                _models.Add( "gpt-4" );
                _models.Add( "gpt-4-32k" );
                _models.Add( "gpt-4o" );
                _models.Add( "gpt-4o-mini" );
                _models.Add( "o1-mini" );
                return _models?.Any( ) == true
                    ? _models
                    : default( IList<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the end points.
        /// </summary>
        /// <returns></returns>
        private protected IList<string> GetEndPoints( )
        {
            try
            {
                _endPoints.Add( "https://api.openai.com/v1/chat/completions" );
                _endPoints.Add( "https://api.openai.com/v1/completions" );
                _endPoints.Add( "https://api.openai.com/v1/assistants" );
                _endPoints.Add( "https://api.openai.com/v1/audio/speech" );
                _endPoints.Add( "https://api.openai.com/v1/audio/translations" );
                _endPoints.Add( "https://api.openai.com/v1/fine_tuning/jobs" );
                _endPoints.Add( "https://api.openai.com/v1/files" );
                _endPoints.Add( "https://api.openai.com/v1/uploads" );
                _endPoints.Add( "https://api.openai.com/v1/images/generations" );
                _endPoints.Add( "https://api.openai.com/v1/images/variations" );
                _endPoints.Add( "https://api.openai.com/v1/threads" );
                _endPoints.Add( "https://api.openai.com/v1/threads/runs" );
                _endPoints.Add( "https://api.openai.com/v1/vector_stores" );
                _endPoints.Add( "https://api.openai.com/v1/organization/invites" );
                _endPoints.Add( "https://api.openai.com/v1/organization/users" );
                _endPoints.Add( "https://api.openai.com/v1/organization/projects" );
                return _endPoints?.Any( ) == true
                    ? _endPoints
                    : default( IList<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c>
        /// to release both managed
        /// and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        public virtual void Dispose( bool disposing )
        {
            if( disposing )
            {
                _httpClient?.Dispose( );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks
        /// associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
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