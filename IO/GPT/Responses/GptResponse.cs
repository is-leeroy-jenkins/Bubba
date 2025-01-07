// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-07-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-07-2025
// ******************************************************************************************
// <copyright file="GptResponse.cs" company="Terry D. Eppler">
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
//   GptResponse.cs
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
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class GptResponse : GptResponseBase
    {
        /// <summary>
        /// The choices
        /// </summary>
        private protected IList<GptChoice> _choices;

        /// <summary>
        /// The usage
        /// </summary>
        private protected GptUsage _usage;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.GptResponse" /> class.
        /// </summary>
        public GptResponse( )
            : base( )
        {
            _entry = new object( );
            _presencePenalty = 0.0;
            _frequencyPenalty = 0.0;
            _topPercent = 0.11;
            _temperature = 0.18;
            _maximumTokens = 2048;
            _model = "gpt-4o";
            _endPoint = "https://api.openai.com/v1/chat/completions";
            _number = 1;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
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
        /// Gets or sets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
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
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
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

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public virtual string Model
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
        /// Gets or sets the choices.
        /// </summary>
        /// <value>
        /// The choices.
        /// </value>
        public virtual IList<GptChoice> Choices
        {
            get
            {
                return _choices;
            }
            set
            {
                if( _choices != value )
                {
                    _choices = value;
                    OnPropertyChanged( nameof( Choices ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the usage.
        /// </summary>
        /// <value>
        /// The usage.
        /// </value>
        public virtual GptUsage Usage
        {
            get
            {
                return _usage;
            }
            set
            {
                if( _usage != value )
                {
                    _usage = value;
                    OnPropertyChanged( nameof( Usage ) );
                }
            }
        }
    }
}