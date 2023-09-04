using System;
using System.Collections.Generic;
using System.Text;

namespace RaroNotifications.Models.Response
{
    public class ErrorResponse
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string? Stack { get; set; }
    }
}
