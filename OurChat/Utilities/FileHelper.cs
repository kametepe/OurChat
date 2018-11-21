using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurChat.Utilities
{
    public static class FileHelper
    {
        public static string GetFileName(string extension)
        {
            string filename = string.Concat(Guid.NewGuid().ToString(), ".", extension);
            return filename;
        }
    }
}
