﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-06-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-06-2025
// ******************************************************************************************
// <copyright file="FineTuningResponse.cs" company="Terry D. Eppler">
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
//   FineTuningResponse.cs
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
    public class FineTuningResponse : GptResponse
    {
        /// <summary>
        /// The file path
        /// </summary>
        private protected string _generatedFineTuning;

        /// <summary>
        /// The raw response
        /// </summary>
        private protected string _rawResponse;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="FineTuningResponse"/> class.
        /// </summary>
        /// <inheritdoc />
        public FineTuningResponse( )
            : base( )
        {
            _created = DateTime.Now;
        }

        /// <summary>
        /// Gets or sets the raw response.
        /// </summary>
        /// <value>
        /// The raw response.
        /// </value>
        public string RawResponse
        {
            get
            {
                return _rawResponse;
            }
            set
            {
                if( _rawResponse != value )
                {
                    _rawResponse = value;
                    OnPropertyChanged( nameof( RawResponse ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the generated fine tuning.
        /// </summary>
        /// <value>
        /// The generated fine tuning.
        /// </value>
        public string GeneratedFineTuning
        {
            get
            {
                return _generatedFineTuning;
            }
            set
            {
                if( _generatedFineTuning != value )
                {
                    _generatedFineTuning = value;
                    OnPropertyChanged( nameof( GeneratedFineTuning ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public override string Id
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        public override string Object
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public override DateTime Created
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public override string Model
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
        /// Gets or sets the choices.
        /// </summary>
        /// <value>
        /// The choices.
        /// </value>
        public override IList<GptChoice> Choices
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

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the usage.
        /// </summary>
        /// <value>
        /// The usage.
        /// </value>
        public override GptUsage Usage
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