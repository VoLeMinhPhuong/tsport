using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSPORT.Models
{
    public class ForgotPassword
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}