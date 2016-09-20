using Sabio.Web.Models;
using Sabio.Web.Models.ViewModels;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sabio.Web.Controllers
{
    [RoutePrefix("password")]
    public class PasswordController : BaseController                        
    {
        // GET: Password
        public ActionResult Index()                                         //Enter email address to change pw.
        {
            return View();
        }

        // Reset page:
        [Route("reset/{passwordGuid:guid}")]                                //Create a new route to reset (localhost/password/reset/guid.
        public ActionResult Reset(Guid passwordGuid)                        //view controller named 'Reset'.
        {
            ItemViewModel<Guid> vm = new ItemViewModel<Guid>();             //Using a view model called 'ItemViewModel' and wrapping the 'Guid' to pass it back to the user.
            vm.Item = passwordGuid;

            Token resetNow = TokenService.GetByToken(passwordGuid);         //Calling the GetByToken function from the Token Service.

            if (resetNow == null || resetNow.IsUsed == true)                //If the GUID doesnt match the user or link expires(user already resetted pw), it was take the user to an error page telling the user to request a new reset pw.
            {
                return View("Error", vm);
            }
            else                                                            //Else, it'll bring the user to the reset page to reset pw.
            { 
                return View(vm);
            }           
        }
    }
}