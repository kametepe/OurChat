using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace OurChat.Utilities
{
    public static class PasswordHelper
    {
        public static string HashPassword(string salt, string password)
        {
            string data = string.Concat(password, salt);
            var sha1Provider = System.Security.Cryptography.SHA1.Create();
            var binHash = sha1Provider.ComputeHash(Encoding.ASCII.GetBytes(data));
            return (BitConverter.ToString(binHash)).Replace("-", "");
        }
    }
}