// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 06-20-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        06-20-2025
// ******************************************************************************************
// <copyright file="TextBoxBindingUpdateOnEnterBehaviour.cs" company="Terry D. Eppler">
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
//   TextBoxBindingUpdateOnEnterBehaviour.cs
// </summary>
// ******************************************************************************************
namespace Bubba
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Microsoft.Xaml.Behaviors;

    /// <summary>
    /// 
    /// </summary>
    public class TextBoxBindingUpdateOnEnterBehaviour : Behavior<TextBox>
    {
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected override void OnAttached( )
        {
            AssociatedObject.KeyDown += OnTextBoxKeyDown;
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject,
        /// but before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected override void OnDetaching( )
        {
            AssociatedObject.KeyDown -= OnTextBoxKeyDown;
        }

        /// <summary>
        /// Called when [text box key down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/>
        /// instance containing the event data.</param>
        private void OnTextBoxKeyDown( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Enter )
            {
                var txtBox = sender as TextBox;
                txtBox.GetBindingExpression( TextBox.TextProperty ).UpdateSource( );
            }
        }
    }
}