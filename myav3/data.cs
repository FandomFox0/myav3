using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace myav3
{
    internal class data
    {
        public static string connect = "host=localhost;uid=root;pwd=root;database=myav3";
        //public static string connect = "host=localhost;uid=root;pwd=;database=myav3";
        //public static string connect = "host=localhost;uid=root;pwd=root";

        public static string login;
        public static string surname;
        public static string name;
        public static string patronymic;
        public static string role;

        public static string path = AppDomain.CurrentDomain.BaseDirectory;

        public static string CreateMD5(string input)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(input)).Select(x => x.ToString("X2")));
            }
        }
    }
}
