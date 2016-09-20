using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models
{
    public class Token
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }

        public string UserId { get; set; }

        public Guid TokenString { get; set; }

        public int TokenType { get; set; }

        public bool IsUsed { get; set; }

        public DateTime DateUsed { get; set; }
    }
}