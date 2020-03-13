using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LabelCreator.Model
{
    public class DbTextModel : Label
    {
        public static readonly DependencyProperty IdPoleProperty = DependencyProperty.Register("Id_Pole", typeof(int?), typeof(DbTextModel));

        public int? Id_Pole
        {
            get { return (int)GetValue(IdPoleProperty); }
            set { SetValue(IdPoleProperty, value); }
        }

        public DbTextModel() : base()
        {

        }

    }
}
