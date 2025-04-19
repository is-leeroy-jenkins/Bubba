// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 04-18-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        04-18-2025
// ******************************************************************************************
// <copyright file="UserLocation.cs" company="Terry D. Eppler">
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
//   UserLocation.cs
// </summary>
// ******************************************************************************************
namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Bubba.PropertyChangedBase" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class UserLocation : PropertyChangedBase
    {
        /// <summary>
        /// The city
        /// </summary>
        private protected string _city;

        /// <summary>
        /// The region
        /// </summary>
        private protected string _region;

        /// <summary>
        /// The country
        /// </summary>
        private protected string _country;

        /// <summary>
        /// The state
        /// </summary>
        private protected string _state;

        /// <summary>
        /// The zip code
        /// </summary>
        private protected string _zipCode;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="UserLocation"/> class.
        /// </summary>
        public UserLocation( )
        {
        }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                if( _city != value )
                {
                    _city = value;
                    OnPropertyChanged( nameof( City ) );
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public string Region
        {
            get
            {
                return _region;
            }
            set
            {
                if( _region != value )
                {
                    _region = value;
                    OnPropertyChanged( nameof( UserLocation.Region ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                if( _state != value )
                {
                    _state = value;
                    OnPropertyChanged( nameof( UserLocation.State ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        /// <value>
        /// The zip code.
        /// </value>
        public string ZipCode
        {
            get
            {
                return _zipCode;
            }
            set
            {
                if( _zipCode != value )
                {
                    _zipCode = value;
                    OnPropertyChanged( nameof( UserLocation.ZipCode ) );
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