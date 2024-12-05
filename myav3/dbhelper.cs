using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;
using System.IO;

namespace myav3
{
    public class dbhelper
    {
        public static int maxTC = 0;
        private static string connectionString = "server=localhost;user=root;database=myav3;password=root;";
        static public void LoadDataToDGV(DataGridView dataGridView, string query)
        {
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = connectionString;
                con.Open();

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dataGridView.DataSource = dt;
                con.Close();
            }
        }



        static public int CheckUserRole(string login, string password)
        {
            try
            {
                int role = -1;

                MySqlConnection con = new MySqlConnection(connectionString);
                con.Open();

                MySqlCommand cmd = new MySqlCommand($"Select * From employee Where login = '{login}'", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (password == dt.Rows[0].ItemArray.GetValue(3).ToString())
                {
                    if (Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(4)) == 2)
                    {
                        role = 2;
                    }
                    if (Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(4)) == 1)
                    {
                        role = 1;
                    }

                    UserInfo.id_employee = Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(0));
                    UserInfo.login = dt.Rows[0].ItemArray.GetValue(1).ToString();
                    UserInfo.department_id = dt.Rows[0].ItemArray.GetValue(2).ToString();
                    UserInfo.password = dt.Rows[0].ItemArray.GetValue(3).ToString();
                    UserInfo.role_id = Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(4));
                    UserInfo.surname = Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(4));
                    UserInfo.name = Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(4));
                    UserInfo.patronymic = Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(4));
                    UserInfo.age = Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(4));
                    UserInfo.phone_number = Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(4));
                    UserInfo.status = Convert.ToInt32(dt.Rows[0].ItemArray.GetValue(4));
                }
                else return 0;
                con.Close();
                return role;
            }
            catch (IndexOutOfRangeException) { return 0; }
        }

        static public void LoadDataToDt(DataTable dt, string query)
        {
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = connectionString;
                con.Open();

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                da.Fill(dt);

                con.Close();
            }
        }

        static public long InsertDataOnDb(string query)
        {
            using (MySqlConnection con = new MySqlConnection())
            {
                con.ConnectionString = connectionString;
                con.Open();

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                con.Close();
                return cmd.LastInsertedId;
            }
        }

        static public int AddProductRowToDGV(DataGridView dataGridView, string query, int quantity)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.ExecuteNonQuery();

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                // Получаем значение из первого столбца
                var newValue = dt.Rows[0].ItemArray[0];

                // Проверяем, существует ли уже строка с таким же значением в первом столбце
                bool rowExists = false;
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells[0].Value.ToString() == newValue.ToString())
                    {
                        return 0;
                    }
                }
                // Если строки с таким значением нет, добавляем новую
                if (!rowExists)
                {
                    dataGridView.Rows.Add(dt.Rows[0].ItemArray.GetValue(0), dt.Rows[0].ItemArray.GetValue(1), dt.Rows[0].ItemArray.GetValue(2), dt.Rows[0].ItemArray.GetValue(3), quantity);

                }

                con.Close();
                return 1;
            }
        }

        public static void ImportData(string filePath, string tablename)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();

                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Предполагается, что данные разделены запятыми
                        var values = line.Split(',');

                        string query = string.Empty;

                        switch (tablename)
                        {
                            case "check":
                                query = $"INSERT INTO '{tablename}' (id_check, client_id_client, employee_id, date, total_cost, service_id_service1) VALUES ({values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}, {values[5]})";
                                break;
                            case "client":
                                query = $"INSERT INTO '{tablename}' (id_client, tariff_id, surname, name, patronymic, age, phone_number, series_and_number_passport) VALUES ({values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}, {values[5]}, {values[6]}, {values[7]})";
                                break;
                            case "products":
                                query = $"INSERT INTO '{tablename}' (product_id, product_name, category_id, supplier_id, price, stock) VALUES ({values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]}, {values[5]})";
                                break;
                            case "roles":
                                query = $"INSERT INTO '{tablename}' (id, role_name) VALUES ({values[0]}, {values[1]})";
                                break;
                            case "sales":
                                query = $"INSERT INTO '{tablename}' (sale_id, user_id, sale_date, total_amount, check_check_id) VALUES ({values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]})";
                                break;
                            case "suppliers":
                                query = $"INSERT INTO '{tablename}' (supplier_id, supplier_name, contact_email, phone) VALUES ({values[0]}, {values[1]}, {values[2]}, {values[3]})";
                                break;
                            case "users":
                                query = $"INSERT INTO '{tablename}' (user_id, username, email, password, role) VALUES ({values[0]}, {values[1]}, {values[2]}, {values[3]}, {values[4]})";
                                break;

                        }


                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("Данные успешно импортированы.");
            }
        }
    }
}
