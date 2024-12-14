// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 12-10-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        12-10-2024
// ******************************************************************************************
// <copyright file="GptChoice.cs" company="Terry D. Eppler">
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
//   GptChoice.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "ClassNeverInstantiated.Global" ) ]
    public class GptChoice : PropertyChangedBase
    {
        /// <summary>
        /// The text
        /// </summary>
        private protected string _text;

        /// <summary>
        /// The index
        /// </summary>
        private protected int _index;

        /// <summary>
        /// The logprobs
        /// </summary>
        private protected string _logprobs;

        /// <summary>
        /// The finish reason
        /// </summary>
        private protected string _finishReason;

        /// <summary>
        /// The data
        /// </summary>
        private protected IDictionary<string, object> _data;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="GptChoice"/> class.
        /// </summary>
        public GptChoice( )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GptChoice"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="index">The index.</param>
        /// <param name="logprobs">The logprobs.</param>
        /// <param name="finishReason">The finish reason.</param>
        public GptChoice( int index, string text, string logprobs,
            string finishReason )
        {
            _index = index;
            _text = text;
            _logprobs = logprobs;
            _finishReason = finishReason;
        }

        /// <summary>
        /// Deconstructs the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="text">The text.</param>
        /// <param name="logprobs">The logprobs.</param>
        /// <param name="finishReason">The finish reason.</param>
        public void Deconstruct( out int index, out string text, out string logprobs,
            out string finishReason )
        {
            index = _index;
            text = _text;
            logprobs = _logprobs;
            finishReason = _finishReason;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if( _text != value )
                {
                    _text = value;
                    OnPropertyChanged( nameof( Text ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                if( _index != value )
                {
                    _index = value;
                    OnPropertyChanged( nameof( Index ) );
                }
            }
        }

        /// <summary>
        /// This might be a more complex object in reality
        /// </summary>
        /// <value>
        /// The logprobs.
        /// </value>
        public string Logprobs
        {
            get
            {
                return _logprobs;
            }
            set
            {
                if( _logprobs != value )
                {
                    _logprobs = value;
                    OnPropertyChanged( nameof( Logprobs ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the finish reason.
        /// </summary>
        /// <value>
        /// The finish reason.
        /// </value>
        public string FinishReason
        {
            get
            {
                return _finishReason;
            }
            set
            {
                if( _finishReason != value )
                {
                    _finishReason = value;
                    OnPropertyChanged( nameof( FinishReason ) );
                }
            }
        }
    }
}