using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace ENSE470Project
{
    class EmailHandler
    {


        public string sendEmail(string recieverEmail, string emailMessage)
        {

            //ﬁnal String username = ”sseuofr@gmail.com”;
            //ﬁnal String password = ”Z9KqzY+J”;


            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("sseuofr@gmail.com");
            msg.To.Add(recieverEmail);



            msg.Subject = "Hello!";
            msg.Body = emailMessage;




            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("sseuofr@gmail.com", "Z9KqzY+J");
            client.Timeout = 20000;
            try
            {
                client.Send(msg);
                return "Mail has been successfully sent!";
            }
            catch (Exception ex)
            {
                return "Fail Has error" + ex.Message;
            }
            finally
            {
                msg.Dispose();
            }
        }







    }
}
