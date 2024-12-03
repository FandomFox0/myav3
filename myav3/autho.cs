using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace myav3
{
    public partial class autho : Form
    {
        public autho()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 5 || textBox2.Text.Length < 5)
            {
                button1.Enabled = false;
            }
            else { button1.Enabled = true;}
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 5 || textBox2.Text.Length < 5)
            {
                button1.Enabled = false; 
            }
            else { button1.Enabled = true;}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void autho_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("Вы действительно хотите закрыть программу?", "Авторизация", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = !(res == DialogResult.Yes);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string login = textBox1.Text;
                string password = textBox2.Text;

                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT login, `password`, role_id, surname, `name`, patronymic, status FROM employee WHERE login = '{login}'", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count != 0)
                    {
                        if (dt.Rows[0][6].ToString() == "Работает")
                        {
                            password = data.CreateMD5(password);
                            if (dt.Rows[0][1].ToString() == password)
                            {
                                data.login = login;
                                if (dt.Rows[0][2].ToString() == "1")
                                {
                                    data.role = "Администратор";
                                }
                                else { data.role = "Менеджер"; }
                                data.surname = dt.Rows[0][3].ToString();
                                data.name = dt.Rows[0][4].ToString();
                                data.patronymic = dt.Rows[0][5].ToString();

                                MessageBox.Show($"Добро пожаловать, {data.name}!", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                this.Hide();
                                if (data.role == "Администратор")
                                {
                                    admin forma = new admin();
                                    forma.ShowDialog();
                                }
                                else
                                {
                                    menu forma = new menu();
                                    forma.ShowDialog();
                                }
                                this.Show();

                                textBox1.Clear(); textBox2.Clear();
                            }
                            else { MessageBox.Show("Неверный пароль", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                        }
                        else { MessageBox.Show("ВЫ УВОЛЕНЫ!\nДоступ запрещён", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    }
                    else { MessageBox.Show("Сотрудник с данным логином не найден", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                }
            }
            catch { MessageBox.Show("Неизвестная ошибка", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
