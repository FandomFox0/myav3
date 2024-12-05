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
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Threading;

namespace myav3
{
    public partial class autho : Form
    {
        public autho()
        {
            InitializeComponent();
        }
        private string captchaText;
        string conString = data.connect;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 5 || textBox2.Text.Length < 5 || button4.Enabled == true)
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

                string adminUsername = "local";
                string adminPassword = "local";
                if (login == adminUsername && password == adminPassword)
                {
                    this.Hide();
                    import importForm = new import();
                    importForm.ShowDialog();
                    this.Show();
                    return;
                }
                else
                {
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
                                else {
                                    MessageBox.Show("Неверный пароль", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Captha();
                                }
                            }
                            else { MessageBox.Show("ВЫ УВОЛЕНЫ!\nДоступ запрещён", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                        }
                        else { MessageBox.Show("Сотрудник с данным логином не найден", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                    }
                }
            }
            catch { MessageBox.Show("Неизвестная ошибка", "Авторизация", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
        private void Captha() //Создание капчи
        {
            button3.Enabled = true;
            button4.Enabled = true;
            textBox3.Enabled = true;
            CaptchaToImage();
            button1.Enabled = false;
            textBox1.Text = null;
            textBox2.Text = null;
            this.Width = 700;
        }
        private void CaptchaToImage() //Отрисовка капчи
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            captchaText = ""; for (int i = 0; i < 5; i++)
            {
                captchaText += chars[random.Next(chars.Length)];
            }
            Bitmap bmp = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.SmoothingMode = SmoothingMode.AntiAlias; graphics.Clear(Color.Silver);
            Font font = new Font("Arial", 25, FontStyle.Bold);
            for (int i = 0; i < 5; i++)
            {
                PointF point = new PointF(i * 20, 0);
                graphics.TranslateTransform(100, 50);
                graphics.RotateTransform(random.Next(-10, 10));
                graphics.DrawString(captchaText[i].ToString(), font, Brushes.LightCoral, point);
                graphics.ResetTransform();
            }
            for (int i = 0; i < 10; i++)
            {
                Pen pen = new Pen(Color.LightCoral, random.Next(2, 5));
                int x1 = random.Next(pictureBox2.Width);
                int y1 = random.Next(pictureBox2.Height);
                int x2 = random.Next(pictureBox2.Width);
                int y2 = random.Next(pictureBox2.Height); graphics.DrawLine(pen, x1, y1, x2, y2);
            }
            pictureBox2.Image = bmp;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == captchaText)
            {
                button1_Click(sender, e);
            }
            else //Блокировка системы на 10 секунд посленеудачного ввода
            {
                MessageBox.Show("Неверный ввод, блокировка системы на 10 секунд");
                button4.Enabled = false;
                button3.Enabled = false;
                Thread.Sleep(10000);
                button4.Enabled = true;
                button3.Enabled = true;
                Captha();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Captha();
        }
    }
}
