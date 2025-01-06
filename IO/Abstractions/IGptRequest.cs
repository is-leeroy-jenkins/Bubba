// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 12-18-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        12-18-2024
// ******************************************************************************************
// <copyright file="IGptRequest.cs" company="Terry D. Eppler">
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
//   IGptRequest.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System.Collections.Generic;
    using System.Net.Http;

    public interface IGptRequest
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="P:Bubba.GptRequest.HttpClient" /> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        HttpClient HttpClient { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Gets the chat model.
        /// </summary>
        /// <value>
        /// The chat model.
        /// </value>
        GptBody Body { get; set; }

        /// <summary>
        /// Gets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        string ApiKey { get; }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        IDictionary<string, object> Data { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Gets the end point.
        /// </summary>
        /// <value>
        /// The end point.
        /// </value>
        string EndPoint { get; set; }

        /// <summary>
        /// THe number 'n' of responses returned by the API.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        int Number { get; set; }

        /// <summary>
        /// Gets the maximum tokens.
        /// </summary>
        /// <value>
        /// The maximum tokens.
        /// </value>
        int MaximumTokens { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="GptRequest"/> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        bool Store { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="Stream"/> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
        bool Stream { get; set; }

        /// <summary>
        /// A number between -2.0 and 2.0 Positive value decrease the
        /// model's likelihood to repeat the same line verbatim.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        double Temperature { get; set; }

        /// <summary>
        /// An alternative to sampling with temperature,
        /// called nucleus sampling, where the model considers
        /// the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability
        /// mass are considered. We generally recommend altering this
        /// or temperature but not both.
        /// </summary>
        /// <value>
        /// The top percent.
        /// </value>
        double TopPercent { get; set; }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        double Frequency { get; set; }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <value>
        /// The presence.
        /// </value>
        double Presence { get; set; }
    }
}