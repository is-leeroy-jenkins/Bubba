// ******************************************************************************************
//     Assembly:                Baby
//     Author:                  Terry D. Eppler
//     Created:                 09-09-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        09-09-2024
// ******************************************************************************************
// <copyright file="Domain.cs" company="Terry D. Eppler">
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
//   Domain.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    public enum Domains
    {
        /// <summary>
        /// "https://www.google.com/search?q=site:fda.gov "
        /// </summary>
        FDA,

        /// <summary>
        /// "https://www.google.com/search?q=site:cdc.gov "
        /// </summary>
        CDC,

        /// <summary>
        /// "https://www.google.com/search?q=site:nasa.gov "
        /// </summary>
        NASA,

        /// <summary>
        /// "https://www.google.com/search?q=site:noaa.gov "
        /// </summary>
        NOAA,

        /// <summary>
        /// "https://www.google.com/search?q=site:epa.gov "
        /// </summary>
        EPA,

        /// <summary>
        /// "https://www.google.com/search?q=site:epa.gov "
        /// </summary>
        DATA,

        /// <summary>
        /// "https://www.google.com/search?q=site:gpo.gov "
        /// </summary>
        GPO,

        /// <summary>
        /// "https://www.google.com/search?q=site:govinfo.gov "
        /// </summary>
        USGI,

        /// <summary>
        /// "https://www.google.com/search?q=site:crsreports.congress.gov "
        /// </summary>
        CRS,

        /// <summary>
        /// "https://www.google.com/search?q=site:loc.gov "
        /// </summary>
        LOC,

        /// <summary>
        /// "https://www.google.com/search?q=site:whitehouse.gov "
        /// </summary>
        OMB,

        /// <summary>
        /// "https://www.google.com/search?q=site:fiscal.treasury.gov "
        /// </summary>
        UST,

        /// <summary>
        /// "https://www.google.com/search?q=site:doi.gov "
        /// </summary>
        DOI,

        /// <summary>
        /// "https://www.google.com/search?q=site:nps.gov "
        /// </summary>
        NPS,

        /// <summary>
        /// "https://www.google.com/search?q=site:gsa.gov "
        /// </summary>
        GSA,

        /// <summary>
        /// The National Archives
        /// </summary>
        NARA,

        /// <summary>
        /// "https://www.google.com/search?q=site:commerce.gov "
        /// </summary>
        DOC,

        /// <summary>
        /// The HHS
        /// </summary>
        HHS,

        /// <summary>
        /// The NRC
        /// </summary>
        NRC,

        /// <summary>
        /// The doe
        /// </summary>
        DOE,

        /// <summary>
        /// The NSF
        /// </summary>
        NSF,

        /// <summary>
        /// The usda
        /// </summary>
        USDA,

        /// <summary>
        /// "https://www.google.com/search?q=site:csb.gov "
        /// </summary>
        CSB,

        /// <summary>
        /// "https://www.google.com/search?q=site:irs.gov "
        /// </summary>
        IRS,

        /// <summary>
        /// "https://www.google.com/search?q=site:usace.army.mil "
        /// </summary>
        ACE,

        /// <summary>
        /// "https://www.google.com/search?q=site:dhs.gov "
        /// </summary>
        DHS,

        /// <summary>
        /// "https://www.google.com/search?q=site:defense.gov "
        /// </summary>
        DOD,

        /// <summary>
        /// "https://www.google.com/search?q=site:usno.navy.mil "
        /// </summary>
        USNO,

        /// <summary>
        /// "https://www.google.com/search?q=site:weather.gov "
        /// </summary>
        NWS
    }
}