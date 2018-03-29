using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace ENSE470Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    
    public partial class MainWindow : Window
    {


        UserRequestsViewModel _userReqestViewModel = new UserRequestsViewModel();


        UserViewModel _userViewModel = new UserViewModel();

        ObservableCollection<string> availableSoftwareList;
        List<string> timeFrameList;
        ObservableCollection<UserRequest> userRequestList;     //This list will be different for approvers/analysts (Analysts get them all, approvers only get those they are responsible for.
        UserRequest currentlySelectedUserRequest;
         

        bool[] verificationBools = new bool[8];
        bool[] verificationBoolsRequest = new bool[3];


        MySqlConnection conn;



        public MainWindow()
        {
            
            InitializeComponent();
            base.DataContext = _userReqestViewModel;
            //this.DataContext = this;





timeFrameList = new List<string>();
            timeFrameList.Add("One day");
            timeFrameList.Add("One month");
            timeFrameList.Add("One year");
            timeFrameList.Add("Forever");


            //Contact server here and populate the list



            






            show_SignIn();
            hide_UserSignUpPage();
            hide_UserRequestPage();
            hide_UserRequests();
            hide_UserRequestsDetailed();
            hide_UserRequestsApproved();
            hide_UserRequestsDetailed_Approver();

            availableSoftwareList = new ObservableCollection<string>();
            userRequestList = new ObservableCollection<UserRequest>();

            UserRequestSoftware_ComboBox.ItemsSource = availableSoftwareList;
            UserRequestTimeframe_ComboBox.ItemsSource = timeFrameList;
            UserRequests_ListView.ItemsSource = userRequestList;

            //UserRequest testUserRequest = new UserRequest("Bob", "JoeBob@gmail.com", "JOE'S JOEY", "Mexico City, Mexico", "WUT", "Cause I need's it", "3 months");
            //userRequestList.Add(testUserRequest);
            //userRequestList.Add(testUserRequest);
            //userRequestList.Add(testUserRequest);
            //userRequestList.Add(testUserRequest);
            //userRequestList.Add(testUserRequest);
            //userRequestList.Add(testUserRequest);

            for (int i = 0; i < 8; i++)
                verificationBools[i] = false; //Initialize the Error verification bools


            //currentlySelectedUserRequest = new UserRequest("Timothy", "TIM@gmail.com", "TIMMY", "Mexico City, Mexico", "WUT", "Cause I need's it", "5 months");




            string sqlQuery = "SELECT * FROM ense470.userRequests";

            //string mySqlConn = "server=ense471.cqqhatcgtts4.ca-central-1.rds.amazonaws.com;user=pham;database=ense470;port=3306;password=12345678;"

             conn = new MySqlConnection();
             conn.ConnectionString =
                "Server = ense471.cqqhatcgtts4.ca-central-1.rds.amazonaws.com; Port = 3306; Database = ense470; Uid = pham; Pwd = 12345678;";

           

            MySqlCommand command = new MySqlCommand(sqlQuery, conn);


            conn.Open();


            MySqlDataReader reader = command.ExecuteReader();
            UserRequest tempUserRequest;

            while (reader.Read())
            {
                tempUserRequest = new UserRequest();

                tempUserRequest.UserName = reader["UserName"].ToString();
                tempUserRequest.UserEmail = reader["UserEmail"].ToString();
                tempUserRequest.Company = reader["Company"].ToString();
                tempUserRequest.Location = reader["Location"].ToString();
                tempUserRequest.RequestedSoftware = reader["RequestedSoftware"].ToString();
                tempUserRequest.ReasonForRequest = reader["ReasonForRequest"].ToString();
                tempUserRequest.TimeFrame = reader["TimeFrame"].ToString();
                tempUserRequest.Stage = reader["Stage"].ToString();
                tempUserRequest.DecisionReason = reader["DecisionReason"].ToString();



                userRequestList.Add(tempUserRequest);
            }

            reader.Close();



            sqlQuery = "SELECT * FROM ense470.availableSoftware";

            command = new MySqlCommand(sqlQuery, conn);




            reader = command.ExecuteReader();
            string availableSoftware = "";

            while (reader.Read())
            {
                availableSoftware = reader["SoftwareName"].ToString();
                availableSoftwareList.Add(availableSoftware);
            }

            reader.Close();






        }

        private void button_Click(object sender, RoutedEventArgs e)
        {


            string sqlQuery = "SELECT COUNT(*) from ense470.users where Useremail like '" +
                Username_TextBox.Text + 
                "' AND Passcode like '" +
                SignIn_TextBox.Text +
                "'";


            MySqlCommand command = new MySqlCommand(sqlQuery, conn);


            int userCount = Convert.ToInt32(command.ExecuteScalar());

            if (userCount == 1)
            {
                SignUpError_Label.Content = "";
                SignUpError_Label.Visibility = Visibility.Hidden;

                sqlQuery = "SELECT * from ense470.users where Useremail like '" +
                 Username_TextBox.Text +
                 "' AND Passcode like '" +
                 SignIn_TextBox.Text +
                 "'";

                command = new MySqlCommand(sqlQuery, conn);

                MySqlDataReader reader = command.ExecuteReader();

                reader.Read();

               
                _userViewModel.Location = reader["City"].ToString() + ", " + reader["Province"].ToString() + ", " + reader["Country"].ToString();
                _userViewModel.Company = reader["Company"].ToString();
                _userViewModel.UserEmail = reader["Useremail"].ToString();
                _userViewModel.UserName = reader["Username"].ToString();
                 _userViewModel.Privilege = reader["Privilege"].ToString();

                reader.Close();
            }
            else
            {
                SignUpError_Label.Content = "Username or password Incorrect";
                SignUpError_Label.Visibility = Visibility.Visible;
                return;


            }


            if(_userViewModel.Privilege == "Analyst" || _userViewModel.Privilege == "Approver")
            {
                show_UserRequests();
            
            }
            else
            {
                show_UserRequestPage();
            }


            hide_SignIn();


                //if everything is good so far, check the privilege level of the user

            //if USER Then go to user request page

            //If Approver or analyst then go to UserRequestList








            TypeofAccount_Label.Content = "Type of Account: " + _userViewModel.Privilege;



            

        }



        public void show_SignIn()
        {
            SignIn_Page.Visibility = Visibility.Visible;
            SignUp_Button.Visibility = Visibility.Visible;

        }

        public void hide_SignIn()
        {

            SignIn_Page.Visibility = Visibility.Hidden;
            SignUp_Button.Visibility = Visibility.Hidden;

        }
       


        public void show_UserRequests()
        {
            UserRequests_List.Visibility = Visibility.Visible;
            TypeofAccount_Label.Visibility = Visibility.Visible;

        }

        public void hide_UserRequests()
        {
            UserRequests_List.Visibility = Visibility.Hidden;

        }


        public void show_UserRequestsDetailed()
        {
            UserRequest_Detailed.Visibility = Visibility.Visible;

            Resubmission_Button.Visibility = Visibility.Visible;
            Waitlist_Button.Visibility = Visibility.Visible;
            Reason_Analyst_Label.Visibility = Visibility.Visible;
            Reason_Analyst_TextBox.Visibility = Visibility.Visible;
            Approver_Label_Analyst.Visibility = Visibility.Visible;
            ApproverData_Label_Analyst.Visibility = Visibility.Visible;
            Forward_Button.Visibility = Visibility.Visible;


        }

        public void hide_UserRequestsDetailed()
        {
            UserRequest_Detailed.Visibility = Visibility.Hidden;
            Resubmission_Button.Visibility = Visibility.Hidden;
            Waitlist_Button.Visibility = Visibility.Hidden;
            Reason_Analyst_Label.Visibility = Visibility.Hidden;
            Reason_Analyst_TextBox.Visibility = Visibility.Hidden;
            Approver_Label_Analyst.Visibility = Visibility.Hidden;
            ApproverData_Label_Analyst.Visibility = Visibility.Hidden;
            Forward_Button.Visibility = Visibility.Hidden;
            Approve_Button.Visibility = Visibility.Hidden;
            Reason_TextBox.Visibility = Visibility.Hidden;
            Deny_Button.Visibility = Visibility.Hidden;


        }

        public void show_UserRequestsDetailed_Approver()
        {
            UserRequest_Detailed.Visibility = Visibility.Visible;
            Approve_Button.Visibility = Visibility.Visible;
            Deny_Button.Visibility = Visibility.Visible;
            Reason_TextBox.Visibility = Visibility.Visible;
            Reason_Label.Visibility = Visibility.Visible;
            Required_Label.Visibility = Visibility.Visible;
        }

        public void hide_UserRequestsDetailed_Approver()
        {
            UserRequest_Detailed.Visibility = Visibility.Hidden;
            Approve_Button.Visibility = Visibility.Hidden;
            Deny_Button.Visibility = Visibility.Hidden;
            Reason_TextBox.Visibility = Visibility.Hidden;
            Reason_Label.Visibility = Visibility.Hidden;
            Required_Label.Visibility = Visibility.Hidden;
        }

        public void show_UserRequestsApproved()
        {
            UserRequest_Detailed.Visibility = Visibility.Visible;
            ApprovalResult_Label.Visibility = Visibility.Visible;
            ForwardUser_Button.Visibility = Visibility.Visible;

        }

        public void hide_UserRequestsApproved()
        {
            UserRequest_Detailed.Visibility = Visibility.Hidden;
            ApprovalResult_Label.Visibility = Visibility.Hidden;
            ForwardUser_Button.Visibility = Visibility.Hidden;
            Resubmission_Button.Visibility = Visibility.Hidden;
            Waitlist_Button.Visibility = Visibility.Hidden;
            Reason_Analyst_Label.Visibility = Visibility.Hidden;
            Reason_Analyst_TextBox.Visibility = Visibility.Hidden;
        }

        public void show_UserSignUpPage()
        {
            UserSignUpPage.Visibility = Visibility.Visible;
            TypeofAccount_Label.Visibility = Visibility.Visible;

        }


        public void hide_UserSignUpPage()
        {
            UserSignUpPage.Visibility = Visibility.Hidden;


        }

        public void show_UserRequestPage()
        {
            UserRequestPage.Visibility = Visibility.Visible;
            TypeofAccount_Label.Visibility = Visibility.Visible;

        }


        public void hide_UserRequestPage()
        {
            UserRequestPage.Visibility = Visibility.Hidden;


        }



        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SignIn_Page.Visibility == Visibility.Visible)
            {
                hide_SignIn();
                show_UserSignUpPage();


            }

            else if (UserSignUpPage.Visibility == Visibility.Visible)
            {
                hide_UserSignUpPage();
                show_UserRequestPage();

            }

            else if (UserRequestPage.Visibility == Visibility.Visible)
            {
                hide_UserRequestPage();
                show_UserRequests();


            }
            else if (UserRequests_List.Visibility == Visibility.Visible)
            {
                hide_UserRequests();
                show_UserRequestsDetailed();

            }

            else if (Resubmission_Button.Visibility == Visibility.Visible)
            {
                hide_UserRequestsDetailed();
                show_UserRequestsDetailed_Approver();

            }

            else if (Reason_TextBox.Visibility == Visibility.Visible)
            {
                hide_UserRequestsDetailed_Approver();
                show_UserRequestsApproved();


            }

            else if (ApprovalResult_Label.Visibility == Visibility.Visible)
            {
                hide_UserRequestsApproved();
                show_SignIn();

            }
            

        }

        private void UserRequests_ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            

            currentlySelectedUserRequest = (UserRequest)UserRequests_ListView.SelectedItem;

            _userReqestViewModel.UserName = currentlySelectedUserRequest.UserName;


            _userReqestViewModel.UserEmail = currentlySelectedUserRequest.UserEmail;
            _userReqestViewModel.Company = currentlySelectedUserRequest.Company;
            _userReqestViewModel.Location = currentlySelectedUserRequest.Location;
            _userReqestViewModel.RequestedSoftware = currentlySelectedUserRequest.RequestedSoftware;
            _userReqestViewModel.ReasonForRequest = currentlySelectedUserRequest.ReasonForRequest;
            _userReqestViewModel.TimeFrame = currentlySelectedUserRequest.TimeFrame;
            _userReqestViewModel.Stage = currentlySelectedUserRequest.Stage;



            //else if User is an analyst and selected userrequest is approved
            if (_userViewModel.Privilege == "Analyst" && (_userReqestViewModel.Stage == "Approved" || _userReqestViewModel.Stage == "Denied"))
            {
                show_UserRequestsApproved();
                hide_UserRequests();
                hide_UserRequestsDetailed();
                Approve_Button.Visibility = Visibility.Hidden;

                hide_UserRequestsDetailed_Approver();
                show_UserRequestsApproved();
                return;
            }
            //If User is an analyst and selected user request is not yet approved
            if (_userViewModel.Privilege == "Analyst")
            {
                show_UserRequestsDetailed();
                hide_UserRequests();
                return;
            }

            
            //Else if User is an approver

            if (_userViewModel.Privilege == "Approver")
            {
                show_UserRequestsDetailed_Approver();
                hide_UserRequests();
                return;
            }

           
        }

        private void SignUp_Button_Click(object sender, RoutedEventArgs e)
        {
            hide_SignIn();
            show_UserSignUpPage();
        }

        private void CreateAccount_Button_Click(object sender, RoutedEventArgs e)
        {
            string sqlQuery = "SELECT COUNT(*) from ense470.users where Useremail like '" +
                Email_TextBox.Text +
                "'";


            MySqlCommand command = new MySqlCommand(sqlQuery, conn);


            int userCount = Convert.ToInt32(command.ExecuteScalar());

            if (userCount > 0 )
            {
                NameError_Label.Content = "That email is taken.";
                NameError_Label.BorderBrush = Brushes.Red;
                NameError_Label.Visibility = Visibility.Visible;
                return;

            }
            else
            {
                NameError_Label.Content = "";
                NameError_Label.BorderBrush = Brushes.Black;
                NameError_Label.Visibility = Visibility.Hidden;


                sqlQuery = "INSERT INTO ense470.users (`Username`,`Useremail`,`Company`,`City`,`Province`,`Country`,`Passcode`,`Privilege`) VALUES ('" +
                    Name_TextBox.Text + "', '" +
Email_TextBox.Text + "', '" +
Company_TextBox.Text + "', '" +
City_TextBox.Text + "', '" +
Province_TextBox.Text + "', '" +
Country_TextBox.Text + "', '" +
Password_TextBox.Text + "' ,'User');";

                command = new MySqlCommand(sqlQuery, conn);

                command.ExecuteNonQuery();

            }

            show_SignIn();
            hide_UserSignUpPage();

        }

        private void ReturnToUserRequestList_Button_Click(object sender, RoutedEventArgs e)
        {
            Reason_TextBox.Text = "";

            show_UserRequests();
            hide_UserRequestsApproved();
            hide_UserRequestsDetailed();
            hide_UserRequestsDetailed_Approver();

            string sqlQuery = "SELECT * FROM ense470.userRequests";

            MySqlCommand command = new MySqlCommand(sqlQuery, conn);

            MySqlDataReader reader = command.ExecuteReader();
            UserRequest tempUserRequest;

            userRequestList.Clear();

            while (reader.Read())
            {
                tempUserRequest = new UserRequest();

                tempUserRequest.UserName = reader["UserName"].ToString();
                tempUserRequest.UserEmail = reader["UserEmail"].ToString();
                tempUserRequest.Company = reader["Company"].ToString();
                tempUserRequest.Location = reader["Location"].ToString();
                tempUserRequest.RequestedSoftware = reader["RequestedSoftware"].ToString();
                tempUserRequest.ReasonForRequest = reader["ReasonForRequest"].ToString();
                tempUserRequest.TimeFrame = reader["TimeFrame"].ToString();
                tempUserRequest.Stage = reader["Stage"].ToString();
                tempUserRequest.DecisionReason = reader["DecisionReason"].ToString();



                userRequestList.Add(tempUserRequest);
            }

            reader.Close();

        }

        private void Forward_Button_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private void Resubmission_Button_Click(object sender, RoutedEventArgs e)
        {
            EmailHandler emailer = new EmailHandler();

            string message = "";

            message = "Hello User " + _userReqestViewModel.UserName + ",  /n Your software request for " + _userReqestViewModel.RequestedSoftware + " for a time period of " + _userReqestViewModel.TimeFrame + " must be resubmitted because " + Reason_Analyst_TextBox.Text + " /n /n Cordially yours, /n The HELL IT Team";


            emailer.sendEmail(_userReqestViewModel.UserEmail, message);        //Send email to relevant user


        }

        private void Waitlist_Button_Click(object sender, RoutedEventArgs e)
        {
            string sqlQuery = "UPDATE userRequests SET Stage = 'Waitlisted' WHERE Username like '" +
                _userReqestViewModel.UserName +
                "'";

            MySqlCommand command = new MySqlCommand(sqlQuery, conn);

            command.ExecuteNonQuery();
        }

        private void Approve_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Reason_TextBox.Text == "")
            {
                Required_Label.Visibility = Visibility.Visible;

                return;
            }
            
            string sqlQuery = "UPDATE userRequests SET Stage = 'Approved' WHERE Username like '" +
                _userReqestViewModel.UserName +
                "'";

            MySqlCommand command = new MySqlCommand(sqlQuery, conn);

            command.ExecuteNonQuery();

            Required_Label.Visibility = Visibility.Hidden;
            

        }

        private void Deny_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Reason_TextBox.Text == "")
            {
                Required_Label.Visibility = Visibility.Visible;

                return;
            }

            string sqlQuery = "UPDATE userRequests SET Stage = 'Denied' WHERE Username like '" +
                _userReqestViewModel.UserName +
                "'";

            MySqlCommand command = new MySqlCommand(sqlQuery, conn);

            command.ExecuteNonQuery();

            Required_Label.Visibility = Visibility.Hidden;
            
        }

        private void ForwardUser_Button_Click(object sender, RoutedEventArgs e)
        {
            EmailHandler emailer = new EmailHandler();

            string message = "";

            if (_userReqestViewModel.Stage == "Approved")
            {
                message = "Hello User " + _userReqestViewModel.UserName + ",  /n Your software request for " + _userReqestViewModel.RequestedSoftware + " for a time period of " + _userReqestViewModel.TimeFrame + " has been approved. /n /n Cordially yours, /n The HELL IT Team";



            }
            else if (_userReqestViewModel.Stage == "Denied")
            {

                message = "Hello User " + _userReqestViewModel.UserName + ",  /n Your software request for " + _userReqestViewModel.RequestedSoftware + " for a time period of " + _userReqestViewModel.TimeFrame + " has been denied. /n /n " + _userReqestViewModel.DecisionReason + "/n /n We apologize for any inconvenience this may cause. /n Cordially yours, /n The HELL IT Team";


            }

            //emailer.sendEmail("uyskriek@gmail.com");

            emailer.sendEmail(_userReqestViewModel.UserEmail, message);        //Send email to relevant user



        }

        private void UserRequest_Button_Click(object sender, RoutedEventArgs e)
        {

            //Add new User request to SQL server
            string sqlQuery = "INSERT INTO ense470.userRequests(`Username`,`Useremail`,`Company`,`Location`,`RequestedSoftware`,`ReasonForRequest`,`TimeFrame`,`Stage`,`DecisionReason`) VALUES( '" +
_userViewModel.UserName +" ' , '" +
_userViewModel.UserEmail + " ' , '" +
 _userViewModel.Company + " ' , '" +
  _userViewModel.Location + " ' , '" +
    UserRequestSoftware_ComboBox.SelectedItem + " ' , '" +
     UserRequestReason_TextBox.Text + " ' , '" +
      UserRequestTimeframe_ComboBox.SelectedItem + " ' , '" +
       "Created" + " ' , ' ');";

            MySqlCommand command = new MySqlCommand(sqlQuery, conn);

            command.ExecuteNonQuery();


            Success_Label.Visibility = Visibility.Visible;


            UserRequestSoftware_ComboBox.SelectedIndex = -1;
            UserRequestReason_TextBox.Text = "";
            UserRequestTimeframe_ComboBox.SelectedIndex = -1;
        }

      
        private void Name_TextBox_LostFocus_1(object sender, RoutedEventArgs e)
        {


            if (Name_TextBox.Text == "")
            {
                NameError_Label.Content = "Field cannot be empty.";
                NameError_Label.BorderBrush = Brushes.Red;
                NameError_Label.Visibility = Visibility.Visible;
                CreateAccount_Button.IsEnabled = false;
                return;

            }
            else
            {
                NameError_Label.Content = "";
                NameError_Label.BorderBrush = Brushes.Black;
                NameError_Label.Visibility = Visibility.Hidden;
                verificationBools[0] = true;
                for (int i = 0; i < 8; i++)
                    if (verificationBools[i] == false)           //If any of the textboxes has an error
                        return;

                CreateAccount_Button.IsEnabled = true;
            }
        }
        private void Email_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Email_TextBox.Text == "")
            {
                EmailError_Label.Content = "Field cannot be empty.";
                EmailError_Label.BorderBrush = Brushes.Red;
                EmailError_Label.Visibility = Visibility.Visible;
                CreateAccount_Button.IsEnabled = false;
                return;

            }
            else
            {
                EmailError_Label.Content = "";
                EmailError_Label.BorderBrush = Brushes.Black;
                EmailError_Label.Visibility = Visibility.Hidden;
                verificationBools[1] = true;
                for (int i = 0; i < 8; i++)
                    if (verificationBools[i] == false)           //If any of the textboxes has an error
                        return;

                CreateAccount_Button.IsEnabled = true;
            }
        }

        private void Company_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Company_TextBox.Text == "")
            {
                CompanyError_Label.Content = "Field cannot be empty.";
                CompanyError_Label.BorderBrush = Brushes.Red;
                CompanyError_Label.Visibility = Visibility.Visible;
                CreateAccount_Button.IsEnabled = false;
                return;

            }
            else
            {
                CompanyError_Label.Content = "";
                CompanyError_Label.BorderBrush = Brushes.Black;
                CompanyError_Label.Visibility = Visibility.Hidden;
                verificationBools[2] = true;
                for (int i = 0; i < 8; i++)
                    if (verificationBools[i] == false)           //If any of the textboxes has an error
                        return;

                CreateAccount_Button.IsEnabled = true;

            }
        }

        private void City_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (City_TextBox.Text == "")
            {
                CityError_Label.Content = "Field cannot be empty.";
                CityError_Label.BorderBrush = Brushes.Red;
                CityError_Label.Visibility = Visibility.Visible;
                CreateAccount_Button.IsEnabled = false;
                return;

            }
            else
            {
                CityError_Label.Content = "";
                CityError_Label.BorderBrush = Brushes.Black;
                CityError_Label.Visibility = Visibility.Hidden;
                verificationBools[3] = true;
                for (int i = 0; i < 8; i++)
                    if (verificationBools[i] == false)           //If any of the textboxes has an error
                        return;

                CreateAccount_Button.IsEnabled = true;
            }
        }

        private void Province_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Province_TextBox.Text == "")
            {
                ProvinceError_Label.Content = "Field cannot be empty.";
                ProvinceError_Label.BorderBrush = Brushes.Red;
                ProvinceError_Label.Visibility = Visibility.Visible;
                CreateAccount_Button.IsEnabled = false;
                return;

            }
            else
            {
                ProvinceError_Label.Content = "";
                ProvinceError_Label.BorderBrush = Brushes.Black;
                ProvinceError_Label.Visibility = Visibility.Hidden;
                verificationBools[4] = true;
                for (int i = 0; i < 8; i++)
                    if (verificationBools[i] == false)           //If any of the textboxes has an error
                        return;

                CreateAccount_Button.IsEnabled = true;
            }
        }

        private void Country_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Country_TextBox.Text == "")
            {
                CountryError_Label.Content = "Field cannot be empty.";
                CountryError_Label.BorderBrush = Brushes.Red;
                CountryError_Label.Visibility = Visibility.Visible;
                CreateAccount_Button.IsEnabled = false;
                return;

            }
            else
            {
                CountryError_Label.Content = "";
                CountryError_Label.BorderBrush = Brushes.Black;
                CountryError_Label.Visibility = Visibility.Hidden;
                verificationBools[5] = true;
                for (int i = 0; i < 8; i++)
                    if (verificationBools[i] == false)           //If any of the textboxes has an error
                        return;

                CreateAccount_Button.IsEnabled = true;
            }
        }

        private void Password_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Password_TextBox.Text == "")
            {
                Password1Error_Label.Content = "Field cannot be empty.";
                Password1Error_Label.BorderBrush = Brushes.Red;
                Password1Error_Label.Visibility = Visibility.Visible;
                CreateAccount_Button.IsEnabled = false;
                return;

            }
            else if (PasswordReentry_TextBox.Text != Password_TextBox.Text)
            {
                Password1Error_Label.Content = "Password is not the same";
                Password1Error_Label.BorderBrush = Brushes.Red;
                Password1Error_Label.Visibility = Visibility.Visible;
                CreateAccount_Button.IsEnabled = false;
                return;

            }
            else
            {

                Password1Error_Label.Content = "";
                Password1Error_Label.BorderBrush = Brushes.Black;
                Password1Error_Label.Visibility = Visibility.Hidden;
                Password2Error_Label.Content = "";
                Password2Error_Label.BorderBrush = Brushes.Black;
                Password2Error_Label.Visibility = Visibility.Hidden;
                verificationBools[6] = true;
                verificationBools[7] = true;
                for (int i = 0; i < 8; i++)
                    if (verificationBools[i] == false)           //If any of the textboxes has an error
                        return;

                CreateAccount_Button.IsEnabled = true;

            }
        }

        private void PasswordReentry_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordReentry_TextBox.Text == "")
            {
                Password2Error_Label.Content = "Field cannot be empty.";
                Password2Error_Label.BorderBrush = Brushes.Red;
                Password2Error_Label.Visibility = Visibility.Visible;
                CreateAccount_Button.IsEnabled = false;
                return;

            }
            else if (PasswordReentry_TextBox.Text != Password_TextBox.Text)
            {
                Password2Error_Label.Content = "Password is not the same";
                Password2Error_Label.BorderBrush = Brushes.Red;
                Password2Error_Label.Visibility = Visibility.Visible;
                CreateAccount_Button.IsEnabled = false;
                return;

            }
            else
            {
                
                               


                Password1Error_Label.Content = "";
                Password1Error_Label.BorderBrush = Brushes.Black;
                Password1Error_Label.Visibility = Visibility.Hidden;
                Password2Error_Label.Content = "";
                Password2Error_Label.BorderBrush = Brushes.Black;
                Password2Error_Label.Visibility = Visibility.Hidden;


                verificationBools[6] = true;
                verificationBools[7] = true;
                for (int i = 0; i < 8; i++)
                    if(verificationBools[i] == false)           //If any of the textboxes has an error
                        return;

                CreateAccount_Button.IsEnabled = true;
            }
        }

        private void UserRequestSoftware_ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Success_Label.Visibility = Visibility.Hidden;

            if (UserRequestSoftware_ComboBox.SelectedIndex < 0)
            {
                Request_Error_Label.Content = "Must select a software.";
                Request_Error_Label.BorderBrush = Brushes.Red;
                Request_Error_Label.Visibility = Visibility.Visible;
                UserRequest_Button.IsEnabled = false;
                return;

            }
            else
            {
                Request_Error_Label.Content = "";
                Request_Error_Label.BorderBrush = Brushes.Black;
                Request_Error_Label.Visibility = Visibility.Hidden;
                verificationBoolsRequest[0] = true;
                for (int i = 0; i < 3; i++)
                    if (verificationBoolsRequest[i] == false)           //If any of the textboxes has an error
                        return;

                UserRequest_Button.IsEnabled = true;
            }
        }


        private void UserRequestReason_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Success_Label.Visibility = Visibility.Hidden;

            if (UserRequestReason_TextBox.Text == "")
            {
                Reason_Error_Label.Content = "Field cannot be empty.";
                Reason_Error_Label.BorderBrush = Brushes.Red;
                Reason_Error_Label.Visibility = Visibility.Visible;
                UserRequest_Button.IsEnabled = false;
                return;

            }
            else
            {
                Reason_Error_Label.Content = "";
                Reason_Error_Label.BorderBrush = Brushes.Black;
                Reason_Error_Label.Visibility = Visibility.Hidden;
                verificationBoolsRequest[1] = true;
                for (int i = 0; i < 3; i++)
                    if (verificationBoolsRequest[i] == false)           //If any of the textboxes has an error
                        return;

                UserRequest_Button.IsEnabled = true;
            }
        }
    

        private void UserRequestTimeframe_ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Success_Label.Visibility = Visibility.Hidden;

            if (UserRequestTimeframe_ComboBox.SelectedIndex < 0)
            {
                Timeframe_Error_Label.Content = "Must select a TimeFrame.";
                Timeframe_Error_Label.BorderBrush = Brushes.Red;
                Timeframe_Error_Label.Visibility = Visibility.Visible;
                UserRequest_Button.IsEnabled = false;
                return;

            }
            else
            {
                Timeframe_Error_Label.Content = "";
                Timeframe_Error_Label.BorderBrush = Brushes.Black;
                Timeframe_Error_Label.Visibility = Visibility.Hidden;
                verificationBoolsRequest[2] = true;
                for (int i = 0; i < 3; i++)
                    if (verificationBoolsRequest[i] == false)           //If any of the textboxes has an error
                        return;

                UserRequest_Button.IsEnabled = true;
            }
        }
    }


}
