using System.Windows.Interactivity;
using System;
using System.Windows;
using System.Windows.Input;

namespace BCI.PLCSimPP.PresentationControls.MVVM
{
    /// <summary>
    /// Input arg type
    /// </summary>
    public enum PassArgsType
    {
        /// <summary>
        /// Original args only
        /// </summary>
        OnlyOriginalArgs,

        /// <summary>
        /// Command parameter only
        /// </summary>
        OnlyCommandParameter,

        /// <summary>
        /// Both original parameter and command parameter
        /// </summary>
        All
    }

    /// <summary>
    /// Attach properties to bind events and parameters 
    /// </summary>
    public class EventToCommand : TriggerAction<DependencyObject>
    {
        /// <summary>
        /// command
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null));

        /// <summary>
        /// command parameter
        /// </summary>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommand), new PropertyMetadata(null));

        /// <summary>
        /// tag 
        /// </summary>
        public object Tag
        {
            get { return (object)GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Tag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TagProperty =
            DependencyProperty.Register("Tag", typeof(object), typeof(EventToCommand), new PropertyMetadata(null));

        /// <summary>
        /// Input arg type
        /// </summary>
        public PassArgsType PassEventArgsToCommand { get; set; } = PassArgsType.OnlyOriginalArgs;

        /// <summary>
        /// Invoke command
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (Command == null)
            {
                return;
            }

            //Parameter is converted to an array type before the delegate execution
            object param = null;
            switch (PassEventArgsToCommand)
            {
                case PassArgsType.OnlyOriginalArgs:
                    param = parameter;
                    break;
                case PassArgsType.OnlyCommandParameter:
                    param = CommandParameter;
                    break;
                case PassArgsType.All:
                    param = new[] { parameter, Tag, CommandParameter };
                    break;
                default:
                    param = new[] { parameter, Tag, CommandParameter };
                    break;
            }

            try
            {
                if (Command.CanExecute(param))
                {
                    Command.Execute(param);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
