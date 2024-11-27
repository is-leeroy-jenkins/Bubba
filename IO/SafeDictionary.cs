// ******************************************************************************************
//     Assembly:                Baby
//     Author:                  Terry D. Eppler
//     Created:                 09-09-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        09-09-2024
// ******************************************************************************************
// <copyright file="SafeDictionary.cs" company="Terry D. Eppler">
//     Baby is a light-weight, full-featured, web-browser built with .NET 6 and is written
//     in C#.  The baby browser is designed for budget execution and data analysis.
//     A tool for EPA analysts and a component that can be used for general browsing.
// 
//     Copyright ©  2020 Terry D. Eppler
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
//   SafeDictionary.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "CanSimplifyDictionaryLookupWithTryAdd" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class SafeDictionary<TKey, TValue>
    {
        /// <summary>
        /// The dictionary
        /// </summary>
        private readonly Dictionary<TKey, TValue> _dictionary;

        /// <summary>
        /// The padlock
        /// </summary>
        private readonly object _padlock = new object( );

        /// <summary>
        /// Gets or sets the <see cref="TValue"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="TValue"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TValue this[ TKey key ]
        {
            get
            {
                lock( _padlock )
                {
                    return _dictionary[ key ];
                }
            }
            set
            {
                lock( _padlock )
                {
                    _dictionary[ key ] = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SafeDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public SafeDictionary( int capacity )
        {
            _dictionary = new Dictionary<TKey, TValue>( capacity );
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SafeDictionary{TKey, TValue}"/> class.
        /// </summary>
        public SafeDictionary( )
        {
            _dictionary = new Dictionary<TKey, TValue>( );
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool TryGetValue( TKey key, out TValue value )
        {
            lock( _padlock )
            {
                return _dictionary.TryGetValue( key, out value );
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add( TKey key, TValue value )
        {
            lock( _padlock )
            {
                if( _dictionary.ContainsKey( key ) == false )
                {
                    _dictionary.Add( key, value );
                }
            }
        }
    }
}