using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myav3
{
    public static class UserInfo
    {
        public static int id_employee;
        public static string login;
        public static string password;
        public static int role_id;
        public static string department_id;
        public static int surname;
        public static int name;
        public static int patronymic;
        public static int age;
        public static int phone_number;
        public static int status;
    }
    public static class LocalAdminAccount
    {
        public static readonly string Username = "ladmin";
        public static readonly string Password = "ladmin";
    }
}
