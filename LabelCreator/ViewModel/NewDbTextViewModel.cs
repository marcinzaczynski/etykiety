using LabelCreator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelCreator.ViewModel
{
    public class NewDbTextViewModel : INotifyPropertyChanged
    {
        private int idGrupa;

        private string firma;

        private t3 selectedElem;
        private List<t3> groupElements;

        public NewDbTextViewModel()
        {

        }


        public int IdGrupa
        {
            get { return idGrupa; }
            set { idGrupa = value; OnPropertyChanged("IdGrupa"); LoadElements(); }
        }

        public string Firma
        {
            get { return firma; }
            set { firma = value; OnPropertyChanged("Firma"); }
        }

        public List<t3> GroupElements
        {
            get { return groupElements; }
            set { groupElements = value; OnPropertyChanged("GroupElements"); }
        }

        

        public t3 SelectedElem
        {
            get { return selectedElem; }
            set { selectedElem = value; OnPropertyChanged("SelectedElem"); }
        }


        private void LoadElements()
        {
            Firma = DbHandler.T1GetGroups().Where(r => r.id_grupa == IdGrupa).Select(r => r.nazwa).FirstOrDefault();

            GroupElements = DbHandler.T2GetGroupElements(IdGrupa);
        }

        #region PropertyChange

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
