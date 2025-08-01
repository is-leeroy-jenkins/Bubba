﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 07-11-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        07-11-2025
// ******************************************************************************************
// <copyright file="GptFunction.cs" company="Terry D. Eppler">
//     Badger is a budget execution & data analysis tool for EPA analysts
//     based on WPF, Net 6, and written in C Sharp.
// 
//     Copyright �  2022 Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the �Software�),
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
//    THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
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
//   GptFunction.cs
// </summary>
// ******************************************************************************************
namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Bubba.PropertyChangedBase" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class GptFunction : PropertyChangedBase
    {
        /// <summary>
        /// The tool choice
        /// </summary>
        private protected string _toolChoice;

        /// <summary>
        /// The strict
        /// </summary>
        private protected bool _strict;

        /// <summary>
        /// The type
        /// </summary>
        private protected string _type;

        /// <summary>
        /// The name
        /// </summary>
        private protected string _name;

        /// <summary>
        /// The description
        /// </summary>
        private protected string _description;

        /// <summary>
        /// The parameters
        /// </summary>
        private protected IList<string> _parameters;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptFunction"/> class.
        /// </summary>
        public GptFunction( )
            : base( )
        {
            _toolChoice = "none";
            _strict = false;
            _type = "function";
        }

        /// <summary>
        /// Gets or sets the tool choice.
        /// </summary>
        /// <value>
        /// The tool choice.
        /// </value>
        [ JsonPropertyName( "tool_choice" ) ]
        public string ToolChoice
        {
            get
            {
                return _toolChoice;
            }
            set
            {
                if( _toolChoice != value )
                {
                    _toolChoice = value;
                    OnPropertyChanged( nameof( ToolChoice ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [ JsonPropertyName( "description" ) ]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if( _description != value )
                {
                    _description = value;
                    OnPropertyChanged( nameof( Description ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [ JsonPropertyName( "name" ) ]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if( _name != value )
                {
                    _name = value;
                    OnPropertyChanged( nameof( Name ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="GptFunction"/> is strict.
        /// </summary>
        /// <value>
        ///   <c>true</c> if strict; otherwise, <c>false</c>.
        /// </value>
        [ JsonPropertyName( "strict" ) ]
        public bool Strict
        {
            get
            {
                return _strict;
            }
            set
            {
                if( _strict != value )
                {
                    _strict = value;
                    OnPropertyChanged( nameof( Strict ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [ JsonPropertyName( "type" ) ]
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if( _type != value )
                {
                    _type = value;
                    OnPropertyChanged( nameof( Type ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        [ JsonPropertyName( "parameters" ) ]
        public IList<string> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                if( _parameters.Equals( value ) )
                {
                    _parameters = value;
                    OnPropertyChanged( nameof( Parameters ) );
                }
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// 
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected void Fail( Exception ex )
        {
            using var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}