using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests
{
    public class PasswordResetRequest
    {
        [Required]
        public Guid TokenString { get; set; }

        [Required]
        public string Password { get; set; }
    }
}