﻿// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 01-16-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        01-16-2025
// ******************************************************************************************
// <copyright file="SentimentAnalyzer.cs" company="Terry D. Eppler">
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
//   SentimentAnalyzer.cs
// </summary>
// ******************************************************************************************

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ML;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bubba.ISentimentAnalyzer" />
    public class SentimentAnalyzer : ISentimentAnalyzer
    {
        /// <summary>
        /// The predictor
        /// </summary>
        private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SentimentAnalyzer"/> class.
        /// </summary>
        public SentimentAnalyzer( )
        {
            var _mlContext = new MLContext( );
            var Trainer =
                _mlContext.BinaryClassification.Trainers.SdcaLogisticRegression( nameof( SentimentData.Sentiment ) );

            var _data = _mlContext.Data.LoadFromEnumerable( new List<SentimentData>( ) );
            var _pipeline = _mlContext.Transforms.Text
                .FeaturizeText( "Features", nameof( SentimentData.Text ) ).Append( Trainer );

            var _model = _pipeline.Fit( _data );
            _predictor =
                _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>( _model );
        }

        /// <summary>
        /// Analyzes the sentiment.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public bool AnalyzeSentiment( string text )
        {
            var _prediction = _predictor.Predict( new SentimentData
            {
                Text = text
            } );

            return _prediction.Prediction;
        }
    }
}