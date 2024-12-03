using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace myav3
{
    public partial class products : Form
    {
        public products()
        {
            InitializeComponent();
        }

        string photoName = "null.png";
        string photoName2 = "null.png";
        string productName = "";

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void products_Load(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT name FROM producer;", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        comboBox1.Items.Add(row["name"]);
                        comboBox2.Items.Add(row["name"]);
                    }
                    comboBox1.SelectedIndex = 0;
                    comboBox2.SelectedIndex = 0;
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Товары", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }

            comboBoxUpdate1();
        }

        private void comboBoxUpdate1()
        {
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT name FROM product;", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        comboBox3.Items.Add(row["name"]);
                        comboBox4.Items.Add(row["name"]);
                    }
                    comboBox3.SelectedIndex = 0;
                    comboBox4.SelectedIndex = 0;
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Товары", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT id_producer FROM producer WHERE name = '{comboBox1.Text}';", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmd = new MySqlCommand($"INSERT INTO `product` (`name`, `cost`, `discount`, `producer_id`, `description`, `photo`, `balance`) " +
                                            $"VALUES ('{textBox1.Text}', '{textBox4.Text}', '{textBox3.Text}', '{dt.Rows[0][0]}', '{textBox2.Text}', " +
                                             $"'{photoName}', '{textBox5.Text}');", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Вы успешно добавили новый товар!", "Товары", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox4.Clear(); textBox5.Clear();
                    pictureBox2.ImageLocation = photoName = data.path + @"products\\null.png";
                    comboBoxUpdate1();
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Товары", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "") { button2.Enabled = true; }
            else { button2.Enabled = false; }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "") { button2.Enabled = true; }
            else { button2.Enabled = false; }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "") { button2.Enabled = true; }
            else { button2.Enabled = false; }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "") { button2.Enabled = true; }
            else { button2.Enabled = false; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap imageFile = null;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = data.path + "products";
            openFileDialog1.Filter = "jpg фото (*.jpg)|*.jpg|png фото (*.png)|*.png|всё подряд (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageFile = new Bitmap(openFileDialog1.FileName);
                string[] words = openFileDialog1.FileName.Split('\\');
                if (words[words.Length - 1] != "null.png") { photoName = words[words.Length - 1]; }
                else { photoName = "null.png"; }
                pictureBox2.Image = imageFile;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {          
            productName = comboBox3.Text;

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT cost, discount, producer_id, description, photo, balance FROM product WHERE `name` = '{productName}';", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    textBox7.Text = dt.Rows[0][0].ToString();
                    textBox8.Text = dt.Rows[0][1].ToString();
                    textBox9.Text = dt.Rows[0][3].ToString();
                    pictureBox3.ImageLocation = data.path + $@"products\\{dt.Rows[0][4]}";
                    photoName2 = dt.Rows[0][4].ToString();
                    textBox6.Text = dt.Rows[0][5].ToString();

                    cmd = new MySqlCommand($"SELECT name FROM producer WHERE id_producer = '{dt.Rows[0][2]}'", con);
                    da = new MySqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    comboBox2.SelectedItem = dt.Rows[0][0].ToString();
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Товары", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && textBox7.Text != "" && textBox8.Text != "" && textBox9.Text != "" && comboBox3.Text != "") { button6.Enabled = true; }
            else { button6.Enabled = false; }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && textBox7.Text != "" && textBox8.Text != "" && textBox9.Text != "" && comboBox3.Text != "") { button6.Enabled = true; }
            else { button6.Enabled = false; }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && textBox7.Text != "" && textBox8.Text != "" && textBox9.Text != "" && comboBox3.Text != "") { button6.Enabled = true; }
            else { button6.Enabled = false; }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && textBox7.Text != "" && textBox8.Text != "" && textBox9.Text != "" && comboBox3.Text != "") { button6.Enabled = true; }
            else { button6.Enabled = false; }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox6.Text != "" && textBox7.Text != "" && textBox8.Text != "" && textBox9.Text != "" && comboBox3.Text != "") { button6.Enabled = true; }
            else { button6.Enabled = false; }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT id_producer FROM producer WHERE name = '{comboBox2.Text}'", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    int producer_id = int.Parse(dt.Rows[0][0].ToString());

                    cmd = new MySqlCommand($"UPDATE `product` SET `name` = '{comboBox3.Text}', `cost` = '{textBox7.Text}', `discount` = '{textBox8.Text}', " +
                                            $"`producer_id` = '{producer_id}', `description` = '{textBox8.Text}', `photo` = '{photoName2}', `balance` = '{textBox6.Text}' " +
                                             $"WHERE (`name` = '{productName}');", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Вы успешно обновили информацию о товаре!", "Товары", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    comboBoxUpdate1();
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Товары", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap imageFile = null;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = data.path + "products";
            openFileDialog1.Filter = "jpg фото (*.jpg)|*.jpg|png фото (*.png)|*.png|всё подряд (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imageFile = new Bitmap(openFileDialog1.FileName);
                string[] words = openFileDialog1.FileName.Split('\\');
                if (words[words.Length - 1] != "null.png") { photoName2 = words[words.Length - 1]; }
                else { photoName2 = "null.png"; }
                pictureBox3.Image = imageFile;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT cost, discount, producer_id, description, photo, balance FROM product WHERE `name` = '{comboBox4.Text}';", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    textBox11.Text = dt.Rows[0][0].ToString();
                    textBox12.Text = dt.Rows[0][1].ToString();
                    textBox13.Text = dt.Rows[0][3].ToString();
                    pictureBox4.ImageLocation = data.path + $@"products\\{dt.Rows[0][4]}";
                    textBox10.Text = dt.Rows[0][5].ToString();

                    cmd = new MySqlCommand($"SELECT name FROM producer WHERE id_producer = '{dt.Rows[0][2]}'", con);
                    da = new MySqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    textBox14.Text = dt.Rows[0][0].ToString();
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Товары", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Невозможно удалить товар", "Товары", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void products_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("Вы действительно хотите вернуться в меню?", "Товары", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = !(res == DialogResult.Yes);
        }
    }
}
