using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myav3
{
    class ConnectionString
    {
        public static string connectionString()
        {
            string dConnectionString = $"Server=127.0.0.1; Database=myav31; Uid=root; Pwd=root";

            return dConnectionString;

        }
        public static string path = AppDomain.CurrentDomain.BaseDirectory + @"Photo\";
    }
}
