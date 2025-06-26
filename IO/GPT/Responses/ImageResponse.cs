﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 06-26-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        06-26-2025
// ******************************************************************************************
// <copyright file="ImageResponse.cs" company="Terry D. Eppler">
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
//   ImageResponse.cs
// </summary>
// ******************************************************************************************
namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "PossibleUnintendedReferenceComparison" ) ]
    [ SuppressMessage( "ReSharper", "FunctionRecursiveOnAllPaths" ) ]
    public class ImageResponse : GptResponse
    {
        /// <summary>
        /// The resource locations
        /// </summary>
        private protected IList<string> _resourceLocations;

        /// <summary>
        /// The transcribed text
        /// </summary>
        private protected string _transcribedText;

        /// <summary>
        /// The raw response
        /// </summary>
        private protected string _rawResponse;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ImageResponse"/> class.
        /// </summary>
        /// <inheritdoc />
        public ImageResponse( )
            : base( )
        {
            _created = DateTime.Now;
            _object = "chat.response";
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.ImageResponse" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public ImageResponse( ImageGenerationRequest request )
            : base( request )
        {
            _model = request.Model;
        }

        /// <summary>
        /// Gets or sets the image urls.
        /// </summary>
        /// <value>
        /// The image urls.
        /// </value>
        public IList<string> ResourceLocations
        {
            get
            {
                return _resourceLocations;
            }
            set
            {
                if( _resourceLocations != value )
                {
                    _resourceLocations = value;
                    OnPropertyChanged( nameof( ImageResponse.ResourceLocations ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the transcribed text.
        /// </summary>
        /// <value>
        /// The transcribed text.
        /// </value>
        public string TranscribedText
        {
            get
            {
                return _transcribedText;
            }
            set
            {
                if( _transcribedText != value )
                {
                    _transcribedText = value;
                    OnPropertyChanged( nameof( ImageResponse.TranscribedText ) );
                }
            }
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
                    OnPropertyChanged( nameof( ImageResponse.RawResponse ) );
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
        [ JsonPropertyName( "id" ) ]
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
                    OnPropertyChanged( nameof( ImageResponse.Id ) );
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
        [ JsonPropertyName( "object" ) ]
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
                    OnPropertyChanged( nameof( ImageResponse.Object ) );
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
        [ JsonPropertyName( "created" ) ]
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
                    OnPropertyChanged( nameof( ImageResponse.Created ) );
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
        [ JsonPropertyName( "model" ) ]
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
                    OnPropertyChanged( nameof( ImageResponse.Model ) );
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
                    OnPropertyChanged( nameof( ImageResponse.Choices ) );
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
        [ JsonPropertyName( "usage" ) ]
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
                    OnPropertyChanged( nameof( ImageResponse.Usage ) );
                }
            }
        }

        /// <summary>
        /// Extracts the image.
        /// </summary>
        /// <param name="jsonResponse">The json response.</param>
        /// <returns></returns>
        private string ExtractImage( string jsonResponse )
        {
            try
            {
                ThrowIf.Empty( jsonResponse, nameof( jsonResponse ) );
                using var _document = JsonDocument.Parse( jsonResponse );
                return _document.RootElement
                    .GetProperty( "choices" )[ 0 ]
                    .GetProperty( "text" )
                    .GetString( );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }

        /// <summary>
        /// Extracts the message from response.
        /// </summary>
        /// <param name="jsonResponse">The json response.</param>
        /// <param name="chatModel">The chat model.</param>
        /// <returns></returns>
        private string ExtractResponseData( string jsonResponse, string chatModel )
        {
            try
            {
                ThrowIf.Empty( jsonResponse, nameof( jsonResponse ) );
                ThrowIf.Empty( chatModel, nameof( chatModel ) );
                using var _document = JsonDocument.Parse( jsonResponse );
                var _root = _document.RootElement;
                if( chatModel.Contains( "gpt-3.5-turbo" ) )
                {
                    var _element = _root.GetProperty( "choices" );
                    if( _element.ValueKind == JsonValueKind.Array
                        && _element.GetArrayLength( ) > 0 )
                    {
                        var _messageElement = _element[ 0 ].GetProperty( "message" );
                        return _messageElement.GetProperty( "content" ).GetString( );
                    }
                }
                else
                {
                    return _root.GetProperty( "choices" )[ 0 ].GetProperty( "text" ).GetString( );
                }

                return string.Empty;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return string.Empty;
            }
        }
    }
}