﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-09-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-09-2025
// ******************************************************************************************
// <copyright file="FileParameter.cs" company="Terry D. Eppler">
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
//   FileParameter.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Properties;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.GptParameter" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class FileParameter : GptParameter
    {
        /// <summary>
        /// The file identifier, which can be referenced in the API endpoints.
        /// </summary>
        private protected int _id;

        /// <summary>
        /// The intended purpose of the file.
        /// Supported values are assistants, assistants_output,
        /// batch, batch_output, fine-tune, fine-tune-results and vision.
        /// </summary>
        private protected string _purpose;

        /// <summary>
        /// A limit on the number of objects to be returned.
        /// Limit can range between 1 and 10,000,
        /// and the default is 10,000.
        /// </summary>
        private protected int _limit;

        /// <summary>
        /// Sort order by the created_at timestamp of the objects.
        /// asc for ascending order and desc for descending order
        /// </summary>
        private protected string _order;

        /// <summary>
        /// A cursor for use in pagination. after is an object ID
        /// defines your place in the list. For instance, if you
        /// make a list request and receive 100 objects, ending with
        /// obj_foo, your subsequent call can include after=obj_foo
        /// in order to fetch the next page of the list
        /// </summary>
        private protected string _after;

        /// <summary>
        /// The Unix timestamp (in seconds) for when the file was created.
        /// </summary>
        private protected int _createdAt;

        /// <summary>
        /// The size of the file, in bytes.
        /// </summary>
        private protected int _bytes;

        /// <summary>
        /// The file name
        /// </summary>
        private protected string _fileName;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="FileParameter"/> class.
        /// </summary>
        /// <inheritdoc />
        public FileParameter( )
            : base( )
        {
            _model = "gpt-4o-mini";
            _endPoint = GptEndPoint.Files;
            _order = "desc";
            _limit = 10000;
        }

        /// <summary>
        /// The file identifier, which can be referenced in the API endpoints.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id
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
        /// The intended purpose of the file.
        /// Supported values are assistants, assistants_output,
        /// batch, batch_output, fine-tune, fine-tune-results and vision.
        /// </summary>
        /// <value>
        /// The purpose.
        /// </value>
        public string Purpose
        {
            get
            {
                return _purpose;
            }
            set
            {
                if( _purpose != value )
                {
                    _purpose = value;
                    OnPropertyChanged( nameof( Purpose ) );
                }
            }
        }

        /// <summary>
        /// A limit on the number of objects to be returned.
        /// Limit can range between 1 and 10,000,
        /// and the default is 10,000.
        /// </summary>
        /// <value>
        /// The limit.
        /// </value>
        public int Limit
        {
            get
            {
                return _limit;
            }
            set
            {
                if( _limit != value )
                {
                    _limit = value;
                    OnPropertyChanged( nameof( Limit ) );
                }
            }
        }

        /// <summary>
        /// Sort order by the created_at timestamp of the objects.
        /// asc for ascending order and desc for descending order
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public string Order
        {
            get
            {
                return _order;
            }
            set
            {
                if( _order != value )
                {
                    _order = value;
                    OnPropertyChanged( nameof( Order ) );
                }
            }
        }

        /// <summary>
        /// A cursor for use in pagination. after is an object ID
        /// defines your place in the list. For instance, if you
        /// make a list request and receive 100 objects, ending with
        /// obj_foo, your subsequent call can include after=obj_foo
        /// in order to fetch the next page of the list
        /// </summary>
        /// <value>
        /// The after.
        /// </value>
        public string After
        {
            get
            {
                return _after;
            }
            set
            {
                if( _after != value )
                {
                    _after = value;
                    OnPropertyChanged( nameof( After ) );
                }
            }
        }

        /// <summary>
        /// The Unix timestamp (in seconds) for when the file was created.
        /// </summary>
        /// <value>
        /// The created at.
        /// </value>
        public int CreatedAt
        {
            get
            {
                return _createdAt;
            }
            set
            {
                if( _createdAt != value )
                {
                    _createdAt = value;
                    OnPropertyChanged( nameof( CreatedAt ) );
                }
            }
        }

        /// <summary>
        /// The size of the file, in bytes.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        public int Bytes
        {
            get
            {
                return _bytes;
            }
            set
            {
                if( _bytes != value )
                {
                    _bytes = value;
                    OnPropertyChanged( nameof( Bytes ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                if( _fileName != value )
                {
                    _fileName = value;
                    OnPropertyChanged( nameof( FileName ) );
                }
            }
        }
    }
}