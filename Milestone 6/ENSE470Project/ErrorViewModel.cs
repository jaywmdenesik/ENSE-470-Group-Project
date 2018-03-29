using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;



namespace ENSE470Project
{
    class ErrorViewModel : INotifyPropertyChanged
    {

        private string errorName;
        public string ErrorName
        {
            get { return errorName; }
            set
            {
                errorName = value;
                if (value == "")
                {
                    ToolTipNameEnable = false;
                    BordertbName = Brushes.Gray;
                }
                else
                {
                    ToolTipNameEnable = true;
                    BordertbName = Brushes.Red;
                }
                this.RaisePropertyChanged("ErrorName");
            }
        }
        private Brush bordertbName;
        public Brush BordertbName
        {
            get { return bordertbName; }
            set
            {
                bordertbName = value;
                this.RaisePropertyChanged("BordertbName");
            }
        }
        private bool toolTipNameEnable;
        public bool ToolTipNameEnable
        {
            get { return toolTipNameEnable; }
            set
            {
                toolTipNameEnable = value;
                this.RaisePropertyChanged("ToolTipNameEnable");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;


        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }


        }

    }
}
