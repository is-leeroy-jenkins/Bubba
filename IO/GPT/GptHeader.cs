// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-20-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-20-2024
// ******************************************************************************************
// <copyright file="GptHeader.cs" company="Terry D. Eppler">
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
//   GptHeader.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class GptHeader : PropertyChangedBase
    {
        /// <summary>
        /// </summary>
        private const string KEY = "sk-proj-qW9o_PoT2CleBXOErbGxe2UlOeHtgJ9K-"
            + "rVFooUImScUvXn44e4R9ivYZtbYh5OIObWepnxCGET3BlbkFJykj4Dt9MDZT2GQg"
            + "NarXOifdSxGwmodYtevUniudDGt8vkUNmxurKO9DkULeAUVz3rdY9g_-OsA";

        /// <summary>
        /// The content type
        /// </summary>
        private protected string _contentType;

        /// <summary>
        /// The authorization
        /// </summary>
        private protected string _authorization;

        /// <summary>
        /// The API key
        /// </summary>
        private protected string _apiKey;

        /// <summary>
        /// The data
        /// </summary>
        private protected IDictionary<string, object> _data = new Dictionary<string, object>( );

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptHeader" /> class.
        /// </summary>
        public GptHeader( )
        {
            _apiKey = KEY;
            _contentType = "application/json";
            _authorization = "Bearer " + KEY;
            _data.Add( "content-type", "application/json" );
            _data.Add( "authorization", _authorization );
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptHeader"/> class.
        /// </summary>
        /// <param name="header">The header.</param>
        public GptHeader( GptHeader header )
        {
            _apiKey = KEY;
            _contentType = "application/json";
            _authorization = "Bearer " + KEY;
            _data = header.Data;
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
        /// Gets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType
        {
            get
            {
                return _contentType;
            }
        }

        /// <summary>
        /// Gets or sets the authorization.
        /// </summary>
        /// <value>
        /// The authorization.
        /// </value>
        public string Authorization
        {
            get
            {
                return _authorization;
            }
            set
            {
                _authorization = value;
            }
        }

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
                _data = value;
            }
        }

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
    }
}