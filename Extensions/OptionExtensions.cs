// ******************************************************************************************
//     Assembly:                Bocifus
//     Author:                  Terry D. Eppler
//     Created:                 10-31-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-31-2024
// ******************************************************************************************
// <copyright file="OptionExtensions.cs" company="Terry D. Eppler">
//   Bocifus is an open source windows (wpf) application that interacts with OpenAI GPT-3.5 Turbo API
//   based on NET6 and written in C-Sharp.
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
//   OptionExtensions.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public static class OptionExtensions
    {
        /// <summary>
        /// Firsts or none.
        /// </summary>
        /// <typeparam name="_"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static Option<_t> FirstOrNone<_t>( this IEnumerable<_t> enumerable )
        {
            try
            {
                return enumerable.Select( x => ( Option<_t> )new Some<_t>( x ) ).FirstOrDefault( );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( Option<_t> );
            }
        }

        /// <summary>
        /// First or none.
        /// </summary>
        /// <typeparam name="_"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static Option<_t> FirstOrNone<_t>( this IEnumerable<_t> enumerable,
            Func<_t, bool> predicate )
        {
            try
            {
                return enumerable.Where( predicate ).FirstOrNone( );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( Option<_t> );
            }
        }

        /// <summary>
        /// Selects the optional.
        /// </summary>
        /// <typeparam name="_t"></typeparam>
        /// <typeparam name="_result">The type of the result.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="map">The map.</param>
        /// <returns></returns>
        public static IEnumerable<_tResult> SelectOptional<_t, _tResult>(
            this IEnumerable<_t> enumerable, Func<_t, Option<_tResult>> map )
        {
            try
            {
                return ( IEnumerable<_tResult> )enumerable.Select( map ).OfType<Some<_tResult>>( )
                    .Select( s => s.IsSome );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IEnumerable<_tResult> );
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private static void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}