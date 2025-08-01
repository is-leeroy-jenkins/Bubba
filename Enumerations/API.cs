﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 12-12-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        12-12-2024
// ******************************************************************************************
// <copyright file="API.cs" company="Terry D. Eppler">
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
//   API.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public enum API
    {
        /// <summary>
        /// The chat completion
        /// </summary>
        ChatCompletion,

        /// <summary>
        /// The agents
        /// </summary>
        Agents,

        /// <summary>
        /// The text generation
        /// </summary>
        TextGeneration,

        /// <summary>
        /// The image generation
        /// </summary>
        ImageGeneration,

        /// <summary>
        /// The speech generation
        /// </summary>
        SpeechGeneration,

        /// <summary>
        /// The speech translation
        /// </summary>
        Translations,

        /// <summary>
        /// The text to speech
        /// </summary>
        TextToSpeech,

        /// <summary>
        /// The sound transcribing
        /// </summary>
        Transcriptions,

        /// <summary>
        /// The image editing
        /// </summary>
        ImageEditing,

        /// <summary>
        /// The assistants
        /// </summary>
        Assistants,

        /// <summary>
        /// The responses
        /// </summary>
        Responses,

        /// <summary>
        /// The files
        /// </summary>
        Files,

        /// <summary>
        /// The uploads
        /// </summary>
        Uploads,

        /// <summary>
        /// The fine tuning
        /// </summary>
        FineTuning,

        /// <summary>
        /// The vector embeddings
        /// </summary>
        Embeddings,

        /// <summary>
        /// The vector stores
        /// </summary>
        VectorStores,

        /// <summary>
        /// The projects
        /// </summary>
        Projects
    }
}