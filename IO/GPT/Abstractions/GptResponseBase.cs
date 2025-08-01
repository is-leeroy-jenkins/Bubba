﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 02-06-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        02-06-2025
// ******************************************************************************************
// <copyright file="GptResponseBase.cs" company="Terry D. Eppler">
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
//   GptResponseBase.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json.Serialization;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptBase" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public abstract class GptResponseBase : GptBase
    {
        /// <summary>
        /// The identifier
        /// </summary>
        private protected string _id;

        /// <summary>
        /// The identifier
        /// </summary>
        private protected string _previousId;

        /// <summary>
        /// The input text
        /// </summary>
        private protected string _outputText;

        /// <summary>
        /// The object
        /// </summary>
        private protected string _object;

        /// <summary>
        /// The created
        /// </summary>
        private protected DateTime _created;

        /// <summary>
        /// The background
        /// </summary>
        private protected bool _background;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptResponseBase" /> class.
        /// </summary>
        protected GptResponseBase( )
            : base( )
        {
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [ JsonPropertyName( "id" ) ]
        public virtual string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if( _id != value )
                {
                    _id = value;
                    OnPropertyChanged( nameof( Id ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [ JsonPropertyName( "output_text" ) ]
        public virtual string OutputText
        {
            get
            {
                return _outputText;
            }
            set
            {
                if( _outputText != value )
                {
                    _outputText = value;
                    OnPropertyChanged( nameof( OutputText ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the previous identifier.
        /// </summary>
        /// <value>
        /// The previous identifier.
        /// </value>
        [ JsonPropertyName( "previous_id" ) ]
        public virtual string PreviousId
        {
            get
            {
                return _previousId;
            }
            set
            {
                if( _previousId != value )
                {
                    _previousId = value;
                    OnPropertyChanged( nameof( PreviousId ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        [ JsonPropertyName( "object" ) ]
        public virtual string Object
        {
            get
            {
                return _object;
            }
            set
            {
                if( _object != value )
                {
                    _object = value;
                    OnPropertyChanged( nameof( Object ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to run the model response
        /// <see cref="GptResponseBase"/> in the background.
        /// </summary>
        /// <value>
        ///   <c>true</c> if background; otherwise, <c>false</c>.
        /// </value>
        [ JsonPropertyName( "background" ) ]
        public virtual bool Background
        {
            get
            {
                return _background;
            }
            set
            {
                if( _background != value )
                {
                    _background = value;
                    OnPropertyChanged( nameof( Background ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        [ JsonPropertyName( "created" ) ]
        public virtual DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                if( _created != value )
                {
                    _created = value;
                    OnPropertyChanged( nameof( Created ) );
                }
            }
        }
    }
}