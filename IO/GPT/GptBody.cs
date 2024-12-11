// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 12-09-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        12-09-2024
// ******************************************************************************************
// <copyright file="GptBody.cs" company="Terry D. Eppler">
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
//   GptBody.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.IGptBody" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassNeverInstantiated.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class GptBody : PropertyChangedBase, IGptBody
    {
        /// <summary>
        /// The response format
        /// </summary>
        private protected string _responseFormat;

        /// <summary>
        /// The model
        /// </summary>
        private protected string _model;

        /// <summary>
        /// The messages
        /// </summary>
        private protected IList<IGptMessage> _messages;

        /// <summary>
        /// The message
        /// </summary>
        private protected UserMessage _userMessage;

        /// <summary>
        /// The system message
        /// </summary>
        private protected SystemMessage _systemMessage;

        /// <summary>
        /// The data
        /// </summary>
        private protected IDictionary<string, object> _data;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptBody"/> class.
        /// </summary>
        public GptBody( )
        {
            _responseFormat = "text";
            _systemMessage = new SystemMessage( );
            _userMessage = new UserMessage( );
            _messages = new List<IGptMessage>( );
            _data = new Dictionary<string, object>( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptBody" /> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public GptBody( string model )
            : this( )
        {
            _model = model;
            _data.Add( "response_format", _responseFormat );
            _data.Add( "model", model );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptBody" /> class.
        /// </summary>
        /// <param name="gptBody">The GPT body.</param>
        public GptBody( GptBody gptBody )
            : this( )
        {
            _model = gptBody.Model;
            _responseFormat = gptBody.ResponseFormat;
            _data.Add( "model", gptBody.Model );
        }

        /// <summary>
        /// Deconstructs the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name = "responseFormat" >
        /// The format of the response
        /// </param>
        /// <param name="system">The system.</param>
        /// <param name="user">The user.</param>
        public void Deconstruct( out string model, out string responseFormat,
            out SystemMessage system, out UserMessage user )
        {
            model = _model;
            responseFormat = _responseFormat;
            system = _systemMessage;
            user = _userMessage;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
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

        /// <summary>
        /// Gets or sets the response format.
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
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
        /// Gets or sets the system message.
        /// </summary>
        /// <value>
        /// The system message.
        /// </value>
        public SystemMessage SystemMessage
        {
            get
            {
                return _systemMessage;
            }
            set
            {
                if( _systemMessage != value )
                {
                    _systemMessage = value;
                    OnPropertyChanged( nameof( SystemMessage ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the user message.
        /// </summary>
        /// <value>
        /// The user message.
        /// </value>
        public UserMessage UserMessage
        {
            get
            {
                return _userMessage;
            }
            set
            {
                if( _userMessage != value )
                {
                    _userMessage = value;
                    OnPropertyChanged( nameof( UserMessage ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public IDictionary<string, object> Data
        {
            get
            {
                return _data;
            }
            set
            {
                if( _data != value )
                {
                    _data = value;
                    OnPropertyChanged( nameof( Data ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString( )
        {
            return _data.ToJson( );
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