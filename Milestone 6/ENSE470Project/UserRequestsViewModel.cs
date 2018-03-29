using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ENSE470Project
{
    class UserRequestsViewModel: INotifyPropertyChanged
    {

        public UserRequestsViewModel()
        {
            _userRequest = new UserRequest("Unknown", "Unknown", "Unknown", "Unknown", "Unknown", "Unknown", "Unknown");


        }



        UserRequest _userRequest;

        public UserRequest UserRequest
        {

            get
            {
                return _userRequest;
            }

            set
            {
                _userRequest = value;
            }
        }


        public string UserName
        {
            get { return UserRequest.UserName; }
            set
            {
                if (UserRequest.UserName != value)
                {
                    UserRequest.UserName = value;
                    RaisePropertyChanged("UserName");
                }
            }



        }


        public string UserEmail
        {
            get { return UserRequest.UserEmail; }
            set
            {
                if (UserRequest.UserEmail != value)
                {
                    UserRequest.UserEmail = value;
                    RaisePropertyChanged("UserEmail");
                }
            }



        }


        public string Company
        {
            get { return UserRequest.Company; }
            set
            {
                if (UserRequest.Company != value)
                {
                    UserRequest.Company = value;
                    RaisePropertyChanged("Company");
                }
            }



        }


        public string Location
        {
            get { return UserRequest.Location; }
            set
            {
                if (UserRequest.Location != value)
                {
                    UserRequest.Location = value;
                    RaisePropertyChanged("Location");
                }
            }



        }

        public string RequestedSoftware
        {
            get { return UserRequest.RequestedSoftware; }
            set
            {
                if (UserRequest.RequestedSoftware != value)
                {
                    UserRequest.RequestedSoftware = value;
                    RaisePropertyChanged("RequestedSoftware");
                }
            }



        }




        public string ReasonForRequest
        {
            get { return UserRequest.ReasonForRequest; }
            set
            {
                if (UserRequest.ReasonForRequest != value)
                {
                    UserRequest.ReasonForRequest = value;
                    RaisePropertyChanged("ReasonForRequest");
                }
            }



        }


        public string TimeFrame
        {
            get { return UserRequest.TimeFrame; }
            set
            {
                if (UserRequest.TimeFrame != value)
                {
                    UserRequest.TimeFrame = value;
                    RaisePropertyChanged("TimeFrame");
                }
            }



        }

        public string Stage
        {
            get { return UserRequest.Stage; }
            set
            {
                if (UserRequest.Stage != value)
                {
                    UserRequest.Stage = value;
                    RaisePropertyChanged("Stage");
                }
            }



        }


        public string DecisionReason
        {
            get { return UserRequest.DecisionReason; }
            set
            {
                if (UserRequest.DecisionReason != value)
                {
                    UserRequest.DecisionReason = value;
                    RaisePropertyChanged("DecisionReason");
                }
            }



        }




        //Various getters and setters for each variable here



        public event PropertyChangedEventHandler PropertyChanged;


        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }


        }

    }
}
