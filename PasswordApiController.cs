using Sabio.Web.Models;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Responses;
using Sabio.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sabio.Web.Controllers.Api
{
    [RoutePrefix("api/password")]
    public class PasswordApiController : ApiController
    {
        //POST:
        [Route, HttpPost]                                                    
        public HttpResponseMessage post(EmailInsertRequest model)
        {                                                                    
            if (!ModelState.IsValid)                                                    //if model request is invalid, it will return a bad request.
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (UserService.IsUser(model.Email))                                        //a function that checks if this person enters in their email is an actual user (actual email when signed up).
            {
                ApplicationUser currentUser = UserService.GetUser(model.Email);         //a function that gets the userId through email.

                Guid tokenId = TokenService.Post(currentUser.Id);                       //generate a unique token for the user pw reset request.                

                ContactEmailService.AdminResetEmailPassword(model, tokenId);            //calling the function from the ContactEmailService class that includes the 'emailInsertRequest model'.

                ItemResponse<Guid> response = new ItemResponse<Guid>();                 //Wrap up the 'Guid' in an envelope(which is the item response).
                response.Item = tokenId;                                                //'ItemReponse' gives structure to the json which gets passed back to the user.                
                return Request.CreateResponse(HttpStatusCode.OK, response);             //Return the tokenId to the user.      
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No account exist");
            }
        }

        //UPDATE:
        [Route, HttpPut]
        public HttpResponseMessage Put(PasswordResetRequest model)
        {
            if (model == null)                                                                  //if model request is null or invalid, it will return a bad request.
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Error is null");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            Token resetNow = TokenService.GetByToken(model.TokenString);                        //To update new password for the user who requested the pw change.            

            bool result = UserService.ChangePassWord(resetNow.UserId, model.Password);

            if (result == true)                                                                 //If password update successful, mark the GUID as used.
            {
                TokenService.MarkedIsUsed(resetNow);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            else 
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Password is not strong enough.");
            }

        }
    }
}