using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LabelCreator
{
    public class CanvasForLabel : Canvas
    {
        public CanvasForLabel() : base()
        {

        }

        // POTRZEBNE DO BINDOWANIA DANYCH
        public static readonly DependencyProperty IdGrupaProperty = DependencyProperty.Register("Id_Grupa", typeof(int?), typeof(CanvasForLabel));
        
        public int? Id_Grupa 
        {
            get { return (int)GetValue(IdGrupaProperty); }
            set { SetValue(IdGrupaProperty, value); }
        }
    }
}
