using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LabelCreator.Helpers
{
    public static class CommandHandler
    {
        public static readonly RoutedUICommand FileSave = new RoutedUICommand
            (
                "FileSave",
                "FileSave",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.S ,ModifierKeys.Control)
                }
            );
        public static readonly RoutedUICommand FileOpen = new RoutedUICommand
            (
                "FileOpen",
                "FileOpen",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.O ,ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand NewCanvas = new RoutedUICommand
            (
                "NewCanvas",
                "NewCanvas",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F1)
                }
            );
    }
}
