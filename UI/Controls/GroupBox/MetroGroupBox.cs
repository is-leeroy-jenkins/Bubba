

namespace Bubba
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    /// <inheritdoc />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    public class MetroGroupBox : GroupBox
    {
        /// <summary>
        /// The theme
        /// </summary>
        private protected DarkMode _theme = new DarkMode();

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Bubba.MetroGroupBox" /> class.
        /// </summary>
        public MetroGroupBox( )
        {
            // Basic Settings
            FontFamily = _theme.FontFamily;
            FontSize = _theme.FontSize;
            Background = _theme.GroupBoxBackground;
            Foreground = _theme.FormForeground;
            BorderBrush = _theme.MutedBorderBrush;
        }

        /// <summary>
        /// Fails the specified _ex.
        /// </summary>
        /// <param name="_ex">The _ex.</param>
        private protected void Fail(Exception _ex)
        {
            var _error = new ErrorWindow(_ex);
            _error?.SetText();
            _error?.ShowDialog();
        }
    }
}
