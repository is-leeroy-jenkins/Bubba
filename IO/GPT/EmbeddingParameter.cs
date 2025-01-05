

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class EmbeddingParameter : GptParam
    {
        /// <summary>
        /// Whether or not to store the responses
        /// </summary>
        private protected bool _store;

        /// <summary>
        /// The stream
        /// </summary>
        private protected bool _stream;

        /// <summary>
        /// An alternative to sampling with temperature,
        /// called nucleus sampling, where the model considers
        /// the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability
        /// mass are considered. We generally recommend altering this
        /// or temperature but not both.
        /// </summary>
        private protected double _topPercent;

        /// <summary>
        /// A number between 0.0 and 2.0   between 0 and 2.
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// </summary>
        private protected double _temperature;

        /// <summary>
        /// A number between -2.0 and 2.0. Positive values penalize new
        /// tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        private protected double _frequencyPenalty;

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        private protected double _presencePenalty;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.EmbeddingParameter" /> class.
        /// </summary>
        public EmbeddingParameter( ) 
        {
            _model = "text-embedding-ada-002";
            _store = false;
            _stream = false;
            _number = 1;
            _temperature = 1.0;
            _topPercent = 1.0;
            _frequencyPenalty = 0.0;
            _presencePenalty = 0.0;
            _responseFormat = "text";
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="GptParam"/> is store.
        /// </summary>
        /// <value>
        ///   <c>true</c> if store; otherwise, <c>false</c>.
        /// </value>
        public bool Store
        {
            get
            {
                return _store;
            }
            set
            {
                if(_store != value)
                {
                    _store = value;
                    OnPropertyChanged(nameof(Store));
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="GptParam"/> is stream.
        /// </summary>
        /// <value>
        ///   <c>true</c> if stream; otherwise, <c>false</c>.
        /// </value>
        public bool Stream
        {
            get
            {
                return _stream;
            }
            set
            {
                if(_stream != value)
                {
                    _stream = value;
                    OnPropertyChanged(nameof(Stream));
                }
            }
        }

        /// <summary>
        /// A number between 0.0 and 2.0   between 0 and 2.
        /// Higher values like 0.8 will make the output more random,
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// </summary>
        /// <value>
        /// The temperature.
        /// </value>
        public new double Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                if(_temperature != value)
                {
                    _temperature = value;
                    OnPropertyChanged(nameof(Temperature));
                }
            }
        }

        /// <summary>
        /// A number between -2.0 and 2.0. Positive values penalize new
        /// tokens based on their existing frequency in the text so far,
        /// decreasing the model's likelihood to repeat the same line verbatim.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public double FrequencyPenalty
        {
            get
            {
                return _frequencyPenalty;
            }
            set
            {
                if(_frequencyPenalty != value)
                {
                    _frequencyPenalty = value;
                    OnPropertyChanged(nameof(FrequencyPenalty));
                }
            }
        }

        /// <summary>
        /// Number between -2.0 and 2.0. Positive values penalize new tokens
        /// based on whether they appear in the text so far,
        /// ncreasing the model's likelihood to talk about new topics.
        /// </summary>
        /// <value>
        /// The presence.
        /// </value>
        public double PresencePenalty
        {
            get
            {
                return _presencePenalty;
            }
            set
            {
                if(_presencePenalty != value)
                {
                    _presencePenalty = value;
                    OnPropertyChanged(nameof(PresencePenalty));
                }
            }
        }

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
        public double TopPercent
        {
            get
            {
                return _topPercent;
            }
            set
            {
                if(_topPercent != value)
                {
                    _topPercent = value;
                    OnPropertyChanged(nameof(TopPercent));
                }
            }
        }

        /// <summary>
        /// Gets or sets the response format.
        /// </summary>
        /// <value>
        /// The response format.
        /// </value>
        public string ResponseFormat
        {
            get
            {
                return _responseFormat;
            }
            set
            {
                if(_responseFormat != value)
                {
                    _responseFormat = value;
                    OnPropertyChanged(nameof(ResponseFormat));
                }
            }
        }
    }
}
