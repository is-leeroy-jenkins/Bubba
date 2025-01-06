// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 11-20-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-20-2024
// ******************************************************************************************
// <copyright file="IGptClient.cs" company="Terry D. Eppler">
//    Bubba is a small windows (wpf) application for interacting with
//    Chat GPT that's developed in C-Sharp under the MIT license
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
//   IGptClient.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IGptClient
    {
        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        double Frequency { get; }

        /// <summary>
        /// A number between 0 and 2.
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// </summary>
        double Temperature { get; }

        /// <summary>
        /// a number between -2.0 and 2.0.
        /// Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// increasing the model's likelihood to talk about new topics.
        /// </summary>
        double Presence { get; }

        /// <summary>
        /// Gets the maximum number of tokens.
        /// </summary>
        /// <value>
        /// The maximum tokens.
        /// </value>
        int MaxTokens { get; }

        /// <summary>
        /// Gets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        string EndPoint { get; }

        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        string Model { get; }

        /// <summary>
        /// Gets the prompt.
        /// </summary>
        /// <value>
        /// The prompt.
        /// </value>
        string Prompt { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is busy.
        /// </summary>
        /// <value>
        /// <c> true </c>
        /// if this instance is busy; otherwise,
        /// <c> false </c>
        /// </value>
        bool IsBusy { get; }

        /// <summary>
        /// Sends a request to the Chat (Assistant) API.
        /// </summary>
        Task<string> GetResponseAsync( List<dynamic> messages, string model = "gpt-4o",
            int maxTokens = 2048, double temperature = 0.7 );

        /// <summary>
        /// Handles POST requests and response parsing.
        /// </summary>
        Task<string> SendRequestAsync( string url, object payload );

        /// <summary>
        /// Sends the HTTP message.
        /// </summary>
        /// <param name="prompt">The question.</param>
        /// <returns></returns>
        string SendCompletionRequest( string prompt );

        /// <summary>
        /// Builds the request data.
        /// </summary>
        /// <param name="prompt">The question.</param>
        /// <returns></returns>
        string CreatePayload( string prompt );

        /// <summary>
        /// Sends the HTTP message asynchronous.
        /// </summary>
        /// <param name="userPrompt">The prompt.</param>
        /// <returns></returns>
        Task<string> SendHttpMessageAsync( string userPrompt );

        /// <inheritdoc />
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        void OnPropertyChanged( [ CallerMemberName ] string propertyName = null );
    }
}