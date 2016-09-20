using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Sabio.Web.Models.Requests;
using System.Net.Mail;
using SendGrid;
using Sabio.Web.Models;
using Sabio.Web.Controllers;
using System.Web.Mvc;
using System.Web;
using Sabio.Web.Models.ViewModels;

namespace Sabio.Web.Services
{
    internal class ContactEmailService : BaseService
    {      
        public static void AdminResetEmailPassword(EmailInsertRequest model, Guid tokenId)
        {

            SendGridMessage resetUserPassword = new SendGridMessage();                                         //An object is created with datatype 'SendGridMessage' which is class that enable us to send out the pw reset email to user.
            resetUserPassword.AddTo(model.Email);                                                              //User email.
            resetUserPassword.From = new MailAddress(ConfigService.AdminEmail);                                //Send from the admin (right now it is under Jimmy).
            resetUserPassword.Subject = "Reset Your PW";                                                       //Title of the email.
            resetUserPassword.Text = "Forgot Your Password? Follow this cool link to reset your pw: " + ConfigService.BaseUrlLink + "/password/reset/" + tokenId;   //Content (link) in email.

           // Create a Web transport, using API Key
           var transportWeb = new SendGrid.Web(ConfigService.SendGridApiKey);                          

            // Send the email.
            transportWeb.DeliverAsync(resetUserPassword);
        }
    }
}
