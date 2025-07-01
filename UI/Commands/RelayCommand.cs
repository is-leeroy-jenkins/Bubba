// ******************************************************************************************
//     Assembly:                Bubba
//     Author:                  Terry D. Eppler
//     Created:                 07-01-2025
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        07-01-2025
// ******************************************************************************************
// <copyright file="RelayCommand.cs" company="Terry D. Eppler">
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
//   RelayCommand.cs
// </summary>
// ******************************************************************************************
namespace Bubba
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// The command handler
        /// </summary>
        private readonly Action<object> _commandHandler;

        /// <summary>
        /// The can execute handler
        /// </summary>
        private readonly Func<object, bool> _canExecuteHandler;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <param name="canExecuteHandler">The can execute handler.</param>
        public RelayCommand( Action<object> commandHandler,
                             Func<object, bool> canExecuteHandler = null )
        {
            _commandHandler = commandHandler;
            _canExecuteHandler = canExecuteHandler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <param name="canExecuteHandler">The can execute handler.</param>
        public RelayCommand( Action commandHandler, Func<bool> canExecuteHandler = null )
            : this( _ => commandHandler( ), canExecuteHandler == null
                ? null
                : new Func<object, bool>( _ => canExecuteHandler( ) ) )
        {
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.
        /// If the command does not require data to be passed,
        /// this object can be set to <see langword="null" />.</param>
        public void Execute( object parameter )
        {
            _commandHandler( parameter );
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.
        /// If the command does not require data to be passed,
        /// this object can be set to <see langword="null" />.</param>
        /// <returns>
        ///   <see langword="true" /> if this command can be executed;
        /// otherwise, <see langword="false" />.
        /// </returns>
        public bool CanExecute( object parameter )
        {
            return
                _canExecuteHandler == null ||
                _canExecuteHandler( parameter );
        }

        /// <summary>
        /// Raises the can execute changed.
        /// </summary>
        public void RaiseCanExecuteChanged( )
        {
            if( CanExecuteChanged != null )
            {
                CanExecuteChanged( this, EventArgs.Empty );
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class RelayCommand<T> : RelayCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <param name="canExecuteHandler">The can execute handler.</param>
        public RelayCommand( Action<T> commandHandler, Func<T, bool> canExecuteHandler = null )
            : base( o => commandHandler( o is T t
                ? t
                : default( T ) ), canExecuteHandler == null
                ? null
                : new Func<object, bool>( o => canExecuteHandler( o is T t
                    ? t
                    : default( T ) ) ) )
        {
        }
    }
}