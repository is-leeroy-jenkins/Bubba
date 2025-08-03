// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 07-13-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        07-13-2025
// ******************************************************************************************
// <copyright file="GptImage.cs" company="Terry D. Eppler">
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
//   GptImage.cs
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
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class GptImage : PropertyChangedBase
    {
        /// <summary>
        /// The model
        /// </summary>
        private protected string _model;

        /// <summary>
        /// The background
        /// </summary>
        private protected string _background;

        /// <summary>
        /// The number
        /// </summary>
        private protected string _number;

        /// <summary>
        /// The URL
        /// </summary>
        private protected string _url;

        /// <summary>
        /// The prompt
        /// </summary>
        private protected string _prompt;

        /// <summary>
        /// The quality
        /// </summary>
        private protected string _quality;

        /// <summary>
        /// The response format
        /// </summary>
        private protected string _responseFormat;

        /// <summary>
        /// The response format
        /// </summary>
        private protected string _outputFormat;

        /// <summary>
        /// The user
        /// </summary>
        private protected string _user;

        /// <summary>
        /// The size
        /// </summary>
        private protected string _size;

        /// <summary>
        /// The style
        /// </summary>
        private protected string _style;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptImage"/> class.
        /// </summary>
        public GptImage( )
        {
        }

        /// <summary>
        /// Gets or sets the size of the image.
        /// </summary>
        /// <value>
        /// The size of the image.
        /// </value>
        [ JsonPropertyName( "size" ) ]
        public string Size
        {
            get
            {
                return _size;
            }
            set
            {
                if( _size != value )
                {
                    _size = value;
                    OnPropertyChanged( nameof( Size ) );
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the prompt.
        /// </summary>
        /// <value>
        /// The prompt.
        /// </value>
        [ JsonPropertyName( "prompt" ) ]
        public string Prompt
        {
            get
            {
                return _prompt;
            }
            set
            {
                if( _prompt != value )
                {
                    _prompt = value;
                    OnPropertyChanged( nameof( Prompt ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        /// <value>
        /// The quality.
        /// </value>
        [ JsonPropertyName( "quality" ) ]
        public string Quality
        {
            get
            {
                return _quality;
            }
            set
            {
                if( _quality != value )
                {
                    _quality = value;
                    OnPropertyChanged( nameof( Quality ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>
        /// The style.
        /// </value>
        [ JsonPropertyName( "style" ) ]
        public string Style
        {
            get
            {
                return _style;
            }
            set
            {
                if( _style != value )
                {
                    _style = value;
                    OnPropertyChanged( nameof( Style ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>
        /// The background.
        /// </value>
        [ JsonPropertyName( "background" ) ]
        public string Background
        {
            get
            {
                return _background;
            }
            set
            {
                if( _background != value )
                {
                    _background = value;
                    OnPropertyChanged( nameof( Background ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the output format.
        /// </summary>
        /// <value>
        /// The output format.
        /// </value>
        [JsonPropertyName( "output_format" )]
        public string OutputFormat
        {
            get
            {
                return _outputFormat;
            }
            set
            {
                if( _outputFormat != value )
                {
                    _outputFormat = value;
                    OnPropertyChanged( nameof( OutputFormat ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the output format.
        /// </summary>
        /// <value>
        /// The output format.
        /// </value>
        [JsonPropertyName( "response_format" ) ]
        public string ResponseFormat
        {
            get
            {
                return _responseFormat;
            }
            set
            {
                if( _responseFormat != value )
                {
                    _responseFormat = value;
                    OnPropertyChanged( nameof( ResponseFormat ) );
                }
            }
        }

        /// <summary>
        /// Gets the model options.
        /// </summary>
        /// <returns>
        /// </returns>
        public IList<string> GetModelOptions( )
        {
            try
            {
                var _formats = new List<string>( );
                _formats.Add( "dall-e-2" );
                _formats.Add( "dall-e-3" );
                _formats.Add( "gpt-image-1" );
                return _formats;
            }
            catch( Exception _ex )
            {
                Fail( _ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the qualiy options.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetQualiyOptions( )
        {
            try
            {
                var _formats = new List<string>( );
                _formats.Add( "auto" );
                _formats.Add( "high" );
                _formats.Add( "medium" );
                _formats.Add( "low" );
                return _formats;
            }
            catch( Exception _ex )
            {
                Fail( _ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the output format options.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetOutputFormatOptions( )
        {
            try
            {
                var _formats = new List<string>( );
                _formats.Add( "png" );
                _formats.Add( "jpeg" );
                _formats.Add( "webp" );
                return _formats;
            }
            catch( Exception _ex )
            {
                Fail( _ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the format options.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetResponseFormatOptions( )
        {
            try
            {
                var _formats = new List<string>( );
                _formats.Add( "url" );
                _formats.Add( "b64_json" );
                return _formats;
            }
            catch( Exception _ex )
            {
                Fail( _ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the format options.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetStyleOptions( )
        {
            try
            {
                var _formats = new List<string>( );
                _formats.Add( "natural" );
                _formats.Add( "vivid" );
                return _formats;
            }
            catch( Exception _ex )
            {
                Fail( _ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the background options.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetBackgroundOptions( )
        {
            try
            {
                var _formats = new List<string>( );
                _formats.Add( "auto" );
                _formats.Add( "opaque" );
                _formats.Add( "transparent" );
                return _formats;
            }
            catch( Exception _ex )
            {
                Fail( _ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the size options.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetSizeOptions( )
        {
            try
            {
                var _formats = new List<string>( );
                _formats.Add( "256x256" );
                _formats.Add( "512x512" );
                _formats.Add( "1024x1024" );
                _formats.Add( "1024x1536" );
                _formats.Add( "1792x1024" );
                _formats.Add( "1024x1792" );
                return _formats;
            }
            catch( Exception _ex )
            {
                Fail( _ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Fails the specified ex.
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