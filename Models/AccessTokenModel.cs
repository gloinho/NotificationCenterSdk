using System;

namespace RaroNotifications.Models
{
    internal class AccessTokenModel
    {
        public AccessTokenModel(string value, DateTime validTo)
        {
            Value = value;
            ValidTo = validTo;
        }
        public string Value { get; private set; }
        public DateTime ValidTo { get; private set; }
    }
}
