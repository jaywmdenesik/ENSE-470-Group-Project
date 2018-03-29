using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ENSE470Project
{
    class User
    { 
        
        public User(string userName, string userEmail, string company, string location)
        {
            this.UserName = userName;
            this.UserEmail = userEmail;
            this.Company = company;
            this.Location = location;
            this.Privilege = "User";        //Default privilege is user


        }



    private string _userName;
    public string UserName { get { return _userName; } set { _userName = value; } }


    private string _userEmail;
    public string UserEmail { get { return _userEmail; } set { _userEmail = value; } }


    private string _company;
    public string Company { get { return _company; } set { _company = value; } }


    private string _location;
    public string Location { get { return _location; } set { _location = value; } }

    private string _privilege;
    public string Privilege { get { return _privilege; } set { _privilege = value; } }


      

    }
}
