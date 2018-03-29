using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ENSE470Project
{
    public class UserRequest 
    {

        public UserRequest()
        {


        }

       

        private string _userName;
        public string UserName { get { return _userName; } set { _userName = value; } }


        private string _userEmail;
        public string UserEmail { get { return _userEmail; } set { _userEmail = value; } }


        private string _company;
        public string Company { get { return _company; } set { _company = value;  } }


        private string _location;
        public string Location { get { return _location; } set { _location = value;  } }


        private string _requestedSoftware;
        public string RequestedSoftware { get { return _requestedSoftware; } set { _requestedSoftware = value;  } }


        private string _reasonForRequest;
        public string ReasonForRequest { get { return _reasonForRequest; } set { _reasonForRequest = value;  } }


        private string _timeFrame;
        public string TimeFrame { get { return _timeFrame; } set { _timeFrame = value;  } }


        private string _stage;
        public string Stage { get { return _stage; } set { _stage = value; } }

        private string _decisionReason;
        public string DecisionReason { get { return _decisionReason; } set { _decisionReason = value; } }

        public UserRequest(string userName, string userEmail, string company, string location, string requestedSoftware, string reasonForRequest, string timeFrame)
        {
            this.UserName = userName;
            this.UserEmail = userEmail;
            this.Company = company;
            this.Location = location;
            this.RequestedSoftware = requestedSoftware;
            this.ReasonForRequest = reasonForRequest;
            this.TimeFrame = timeFrame;
            this.Stage = "Created";
            this.DecisionReason = "";
        }


        

    }
}
