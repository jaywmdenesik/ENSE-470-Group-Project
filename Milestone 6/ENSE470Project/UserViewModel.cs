using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ENSE470Project
{
    class UserViewModel : INotifyPropertyChanged
    {
        public UserViewModel()
        {
            _user = new User("Unknown", "Unknown", "Unknown", "Unknown");


        }



        User _user;


        public User User
        {

            get
            {
                return _user;
            }

            set
            {
                _user = value;
            }
        }


        public string UserName
        {
            get { return User.UserName; }
            set
            {
                if (User.UserName != value)
                {
                    User.UserName = value;
                    RaisePropertyChanged("UserName");
                }
            }



        }


        public string UserEmail
        {
            get { return User.UserEmail; }
            set
            {
                if (User.UserEmail != value)
                {
                    User.UserEmail = value;
                    RaisePropertyChanged("UserEmail");
                }
            }



        }


        public string Company
        {
            get { return User.Company; }
            set
            {
                if (User.Company != value)
                {
                    User.Company = value;
                    RaisePropertyChanged("Company");
                }
            }



        }


        public string Location
        {
            get { return User.Location; }
            set
            {
                if (User.Location != value)
                {
                    User.Location = value;
                    RaisePropertyChanged("Location");
                }
            }



        }



        public string Privilege
        {
            get { return User.Privilege; }
            set
            {
                if (User.Privilege != value)
                {
                    User.Privilege = value;
                    RaisePropertyChanged("Privilege");
                }
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
