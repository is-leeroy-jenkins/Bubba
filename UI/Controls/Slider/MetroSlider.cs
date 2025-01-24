

namespace Bubba
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Syncfusion.Windows.Controls.Input;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Syncfusion.Windows.Controls.Input.SfRangeSlider" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class MetroSlider : SfRangeSlider
    {
        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode();

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bubba.MetroSlider" /> class.
        /// </summary>
        public MetroSlider( )
            : base( )
        {
            MinHeight = 45;
            MinWidth = 120;
            Background = _theme.TransparentBrush;
            BorderBrush = _theme.TransparentBrush;
            Foreground = _theme.LightBlueBrush;
            ActiveMinorTickStroke = _theme.LightBlueBrush;
            ActiveTickStroke = _theme.GreenBrush;
            MinorTickStroke = _theme.LightBlueBrush;
            TickStroke = _theme.WhiteForeground;
            ClipToBounds = true;
            Orientation = Orientation.Horizontal;
            SnapsToDevicePixels = true;
            FontFamily = new FontFamily( "Roboto" );
            FontSize = 12;
            LabelOrientation = Orientation.Horizontal;
            SnapsTo = SliderSnapsTo.Ticks;
            TickFrequency = 5;
            TickPlacement = TickPlacement.Inline;
            TickLength = 10;
            MinorTickLength = 4;
            MinorTickStrokeThickness = 1;
            MoveToPoint = MovePoint.MoveToTapPosition;
        }
    }
}
