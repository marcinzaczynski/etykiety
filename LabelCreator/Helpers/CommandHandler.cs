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
                "Zapisz",
                "FileSave",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.S, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand FileOpen = new RoutedUICommand
            (
                "Otwórz",
                "FileOpen",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.O, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Print = new RoutedUICommand
            (
                "Drukuj",
                "Print",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.P, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand NewCanvas = new RoutedUICommand
            (
                "Nowa etykieta",
                "NewCanvas",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.N, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand NewText = new RoutedUICommand
            (
                "Tekst",
                "NewText",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D1, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand NewImage = new RoutedUICommand
            (
                "Obraz",
                "NewImage",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D2, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand NewBarcode = new RoutedUICommand
            (
                "Kod",
                "NewBarcode",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.D3, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand Exit = new RoutedUICommand
            (
                "Wyjście",
                "NewCanvas",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                }
            );

        public static readonly RoutedUICommand EditComponent = new RoutedUICommand
            (
                "Edytuj",
                "EditComponent",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.E, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand DeleteComponent = new RoutedUICommand
            (
                "Usuń",
                "DeleteComponent",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Delete)
                }
            );

        public static readonly RoutedUICommand Ok = new RoutedUICommand
            (
                "OK",
                "Ok",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F1)
                }
            );

        public static readonly RoutedUICommand Cancel = new RoutedUICommand
            (
                "Anuluj",
                "Cancel",
                typeof(CommandHandler),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.Escape)
                }
            );
    }
}
