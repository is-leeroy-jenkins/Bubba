

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class InputData : PropertyChangedBase
    {
        /// <summary>
        /// The text
        /// </summary>
        private protected string _text;

        /// <summary>
        /// The number
        /// </summary>
        private protected float _number;

        /// <summary>
        /// The category
        /// </summary>
        private protected string _category;

        /// <summary>
        /// The label
        /// </summary>
        private protected int _label;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputData"/> class.
        /// </summary>
        public InputData( )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputData"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="number">The number.</param>
        /// <param name="category">The category.</param>
        /// <param name="label">The label.</param>
        public InputData( string text, float number, string category, int label )
        {
            Text = text;
            Number = number;
            Category = category;
            Label = label;
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
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public float Number
        {
            get
            {
                return _number;
            }
            set
            {
                if( _number != value )
                {
                    _number = value;
                    OnPropertyChanged( nameof( Number ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                if( _category != value )
                {
                    _category = value;
                    OnPropertyChanged( nameof( Category ) );
                }
            }
        }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public int Label
        {
            get
            {
                return _label;
            }
            set
            {
                if( _label != value )
                {
                    _label = value;
                    OnPropertyChanged( nameof( Label ) );
                }
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
