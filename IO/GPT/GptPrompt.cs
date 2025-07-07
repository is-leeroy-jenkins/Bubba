// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 06-19-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        06-19-2025
// ******************************************************************************************
// <copyright file="GptPrompt.cs" company="Terry D. Eppler">
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
//   GptPrompt.cs
// </summary>
// ******************************************************************************************
namespace Bubba
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Resources;
    using Properties;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Bubba.PropertyChangedBase" />
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    public class GptPrompt : PropertyChangedBase
    {
        /// <summary>
        /// The identifier
        /// </summary>
        private protected string _id;

        /// <summary>
        /// The version
        /// </summary>
        private protected string _version;

        /// <summary>
        /// The instructions
        /// </summary>
        private protected string _instructions;

        /// <summary>
        /// The variables
        /// </summary>
        private protected IDictionary<string, string> _variables;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptPrompt"/> class.
        /// </summary>
        public GptPrompt( )
        {
            _instructions = Prompts.BudgetAnalyst;
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
        /// Gets or sets the instructions.
        /// </summary>
        /// <value>
        /// The instructions.
        /// </value>
        public virtual string Instructions
        {
            get
            {
                return _instructions;
            }
            set
            {
                if( _instructions != value )
                {
                    _instructions = value;
                    OnPropertyChanged( nameof( Instructions ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual string Version
        {
            get
            {
                return _version;
            }
            set
            {
                if( _version != value )
                {
                    _version = value;
                    OnPropertyChanged( nameof( Version ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the previous identifier.
        /// </summary>
        /// <value>
        /// The previous identifier.
        /// </value>
        public virtual IDictionary<string, string> Variables
        {
            get
            {
                return _variables;
            }
            set
            {
                if( _variables != value )
                {
                    _variables = value;
                    OnPropertyChanged( nameof( Variables ) );
                }
            }
        }

        /// <summary>
        /// Gets the resource names.
        /// </summary>
        /// <param name = "resxPath" > </param>
        /// <returns></returns>
        private protected virtual IList<string> GetResourceNames( string resxPath )
        {
            try
            {
                ThrowIf.Null( resxPath, nameof( resxPath ) );
                var _names = new List<string>( );
                using var reader = new ResXResourceReader( resxPath );
                foreach( DictionaryEntry entry in reader )
                {
                    _names.Add( entry.Key.ToString( ) );
                }

                return _names?.Any( ) == true
                    ? _names
                    : default( List<string> );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( List<string> );
            }
        }

        /// <summary>
        /// Wraps error
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