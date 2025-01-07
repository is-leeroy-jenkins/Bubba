// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-23-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-23-2024
// ******************************************************************************************
// <copyright file="SystemMessage.cs" company="Terry D. Eppler">
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
//   SystemMessage.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptMessage" />
    /// <seealso cref="T:Bubba.IGptMessage" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassNeverInstantiated.Global" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    public class SystemMessage : GptMessage, IGptMessage
    {
        /// <summary>
        /// The system prompt
        /// </summary>
        private protected string _systemPrompt = OpenAI.BubbaPrompt;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SystemMessage"/> class.
        /// </summary>
        public SystemMessage( )
        {
            _role = "system";
            _messages = new Dictionary<string, object>( );
            _messages.Add( "role", _role );
            _content = _systemPrompt;
            _messages.Add( "content", _content );
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.SystemMessage" /> class.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        public SystemMessage( string prompt )
        {
            _role = "system";
            _messages = new Dictionary<string, object>( );
            _messages.Add( "role", _role );
            _content = prompt;
            _messages.Add( "content", _content );
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SystemMessage"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SystemMessage( SystemMessage message )
        {
            _role = message.Role;
            _content = message.Content;
            _messages = message.Messages;
        }

        /// <summary>
        /// Deconstructs the specified role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="content">The content.</param>
        /// <param name = "messages" > </param>
        public void Deconstruct( out string role, out string content, 
            out IDictionary<string, object> messages )
        {
            role = _role;
            content = _content;
            messages = _messages;
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public override string Role
        {
            get
            {
                return _role;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public override string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public override IDictionary<string, object> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
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
            return _messages.ToJson( );
        }
    }
}