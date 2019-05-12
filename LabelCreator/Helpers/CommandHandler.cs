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
        public static readonly RoutedUICommand NewCanvas = new RoutedUICommand
            (
                "GenerateCodes",
                "GenerateCodes",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F1)
                }
            );
    }
}
