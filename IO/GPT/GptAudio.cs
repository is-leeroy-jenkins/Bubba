// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 ${CurrentDate.Month}-${CurrentDate.Day}-${CurrentDate.Year}
//
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        ${CurrentDate.Month}-${CurrentDate.Day}-${CurrentDate.Year}
// ******************************************************************************************
// <copyright file="${File.FileName}" company="Terry D. Eppler">
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
//   ${File.FileName}
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
    /// <seealso cref="Bubba.PropertyChangedBase" />
    [ SuppressMessage( "ReSharper", "PreferConcreteValueOverDefault" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class GptAudio : PropertyChangedBase
    {
        /// <summary>
        /// The format
        /// </summary>
        private protected string _format;

        /// <summary>
        /// The voice
        /// </summary>
        private protected string _voice;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptAudio"/> class.
        /// </summary>
        public GptAudio( )
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptAudio"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="voice">The voice.</param>
        public GptAudio( string format, string voice )
        {
            _format = format;
            _voice = voice;
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public string Format
        {
            get
            {
                return _format;
            }
            set
            {
                if( value != _format )
                {
                    _format = value;
                    OnPropertyChanged( );
                }
            }
        }

        /// <summary>
        /// Gets or sets the voice.
        /// </summary>
        /// <value>
        /// The voice.
        /// </value>
        public string Voice
        {
            get
            {
                return _voice;
            }
            set
            {
                if( value != _voice )
                {
                    _voice = value;
                    OnPropertyChanged( );
                }
            }
        }

        /// <summary>
        /// Gets the voice options.
        /// </summary>
        /// <returns>
        /// </returns>
        public IList<string> GetFormatOptions( )
        {
            try
            {
                var _formats = new List<string>( );
                _formats.Add( "mp3" );
                _formats.Add( "wav" );
                _formats.Add( "ogg" );
                _formats.Add( "flac" );
                _formats.Add( "aac" );
                _formats.Add( "pcm16"  );
                _formats.Add( "opus" );
                return _formats;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the voice options.
        /// </summary>
        /// <returns>
        /// </returns>
        public IList<string> GetVoiceOptions( )
        {
            try
            {
                var _voices = new List<string>( );
                _voices.Add( "alloy" );
                _voices.Add( "amethyst" );
                _voices.Add( "ash" );
                _voices.Add( "ballad" );
                _voices.Add( "coral" );
                _voices.Add( "echo" );
                _voices.Add( "fable" );
                _voices.Add( "nova" );
                _voices.Add( "onyx" );
                _voices.Add( "sage" );
                _voices.Add( "shimmer" );
                _voices.Add( "verse" );
                return _voices;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<string> );
            }
        }

        /// <summary>
        /// Gets the speed options.
        /// </summary>
        /// <returns></returns>
        public IList<double> GetSpeedOptions( )
        {
            try
            {
                var _formats = new List<double>( );
                _formats.Add( 0.25 );
                _formats.Add( 4.0 );
                _formats.Add( 1.0 );
                return _formats;
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( IList<double> );
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
                _formats.Add( "tts-1" );
                _formats.Add( "tts-1-hd" );
                _formats.Add( "gpt-4o-mini-tts" );
                return _formats;
            }
            catch( Exception ex )
            {
                Fail( ex );
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
