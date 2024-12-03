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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace myav3
{
    public partial class clients : Form
    {
        public clients()
        {
            InitializeComponent();
        }

        string phoneNumber;

        private void button3_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT id_tariff FROM tariff WHERE name = '{comboBox2.Text}'", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    int tariff_id = int.Parse(dt.Rows[0][0].ToString());

                    cmd = new MySqlCommand($"INSERT INTO myav3.`client` (`tariff_id`, `surname`, `name`, `patronymic`, `age`, `phone_number`, `series_and_number_passport`) " +
                                           $"VALUES ('{tariff_id}', '{textBox3.Text}', '{textBox4.Text}', '{textBox5.Text}', '{textBox6.Text}', '{helper("num", maskedTextBox1.Text)}', '{maskedTextBox2.Text}" + $"{maskedTextBox3.Text}');", con);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Вы успешно добавили нового клиента!", "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    maskedTextBox1.Clear(); maskedTextBox2.Clear(); maskedTextBox3.Clear();
                    textBox3.Clear(); textBox4.Clear(); textBox5.Clear(); textBox6.Clear();
                    comboBoxPhoneUpdate1(); comboBoxPhoneUpdate2();
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            }
        }

        private void clients_Load(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT name FROM tariff;", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        comboBox2.Items.Add(row["name"]);
                    }
                    comboBox2.SelectedIndex = 0;
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }


            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT name FROM tariff;", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        comboBox1.Items.Add(row["name"]);
                    }
                    comboBox1.SelectedIndex = 0;
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }

            comboBoxPhoneUpdate1();
            comboBoxPhoneUpdate2();
        }

        private void comboBoxPhoneUpdate1()
        {
            comboBox3.Items.Clear();
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT phone_number FROM `client`;", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        comboBox3.Items.Add(row["phone_number"]);
                    }
                    comboBox3.SelectedIndex = 0;
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void comboBoxPhoneUpdate2()
        {
            comboBox4.Items.Clear();
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT phone_number FROM `client`;", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        comboBox4.Items.Add(row["phone_number"]);
                    }
                    comboBox4.SelectedIndex = 0;
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private string helper(string processing, string delay)
        {
            switch (processing)
            {
                case "num":
                    delay = delay.Replace(" ", "");
                    delay = delay.Replace(")", "");
                    delay = delay.Replace("(", "");
                    delay = delay.Replace("-", "");
                    break;
            }

            return delay;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted && maskedTextBox2.MaskCompleted && maskedTextBox3.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted && maskedTextBox2.MaskCompleted && maskedTextBox3.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted && maskedTextBox2.MaskCompleted && maskedTextBox3.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted && maskedTextBox2.MaskCompleted && maskedTextBox3.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted && maskedTextBox2.MaskCompleted && maskedTextBox3.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void maskedTextBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted && maskedTextBox2.MaskCompleted && maskedTextBox3.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != 8)
            { e.Handled = true; }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != 8)
            { e.Handled = true; }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != 8)
            { e.Handled = true; }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT id_tariff FROM tariff WHERE name = '{comboBox1.Text}'", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    int tariff_id = int.Parse(dt.Rows[0][0].ToString());

                    cmd = new MySqlCommand($"UPDATE `client` SET `tariff_id` = '{tariff_id}', `surname` = '{textBox8.Text}', `name` = '{textBox7.Text}', `patronymic` = '{textBox2.Text}', " +
                                            $"`age` = '{textBox1.Text}', `phone_number` = '{comboBox3.Text}', `series_and_number_passport` = '{maskedTextBox5.Text}" + $"{maskedTextBox4.Text}' " +
                                             $"WHERE (`phone_number` = '{phoneNumber}');", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Вы успешно обновили информацию о клиенте!", "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    comboBoxPhoneUpdate1();
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            phoneNumber = comboBox3.Text;

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT tariff_id, surname, `name`, patronymic, age, series_and_number_passport FROM `client` WHERE phone_number = '{helper("num", comboBox3.Text)}';", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    int tariff_id = int.Parse(dt.Rows[0][0].ToString());

                    textBox8.Text = dt.Rows[0][1].ToString();
                    textBox7.Text = dt.Rows[0][2].ToString();
                    textBox2.Text = dt.Rows[0][3].ToString();
                    textBox1.Text = dt.Rows[0][4].ToString();
                    maskedTextBox5.Text = dt.Rows[0][5].ToString().Substring(0, dt.Rows[0][5].ToString().Length - 6);
                    maskedTextBox4.Text = dt.Rows[0][5].ToString().Substring(4);

                    cmd = new MySqlCommand($"SELECT `name` FROM tariff WHERE id_tariff = '{tariff_id}';", con);
                    da = new MySqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    comboBox1.SelectedItem = dt.Rows[0][0].ToString();
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void maskedTextBox5_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if (textBox8.Text != "" && textBox7.Text != "" && textBox1.Text != "" && maskedTextBox5.MaskCompleted && maskedTextBox4.MaskCompleted && comboBox3.Text != "") { button4.Enabled = true; }
            else { button4.Enabled = false; }
        }

        private void maskedTextBox4_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if (textBox8.Text != "" && textBox7.Text != "" && textBox1.Text != "" && maskedTextBox5.MaskCompleted && maskedTextBox4.MaskCompleted && comboBox3.Text != "") { button4.Enabled = true; }
            else { button4.Enabled = false; }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "" && textBox7.Text != "" && textBox1.Text != "" && maskedTextBox5.MaskCompleted && maskedTextBox4.MaskCompleted && comboBox3.Text != "") { button4.Enabled = true; }
            else { button4.Enabled = false; }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "" && textBox7.Text != "" && textBox1.Text != "" && maskedTextBox5.MaskCompleted && maskedTextBox4.MaskCompleted && comboBox3.Text != "") { button4.Enabled = true; }
            else { button4.Enabled = false; }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "" && textBox7.Text != "" && textBox1.Text != "" && maskedTextBox5.MaskCompleted && maskedTextBox4.MaskCompleted && comboBox3.Text != "") { button4.Enabled = true; }
            else { button4.Enabled = false; }
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "" && textBox7.Text != "" && textBox1.Text != "" && maskedTextBox5.MaskCompleted && maskedTextBox4.MaskCompleted && comboBox3.Text != "") { button4.Enabled = true; }
            else { button4.Enabled = false; }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != 8)
            { e.Handled = true; }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != 8)
            { e.Handled = true; }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != 8)
            { e.Handled = true; }
        }

        private void textBox6_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void maskedTextBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "" && textBox7.Text != "" && textBox1.Text != "" && maskedTextBox5.MaskCompleted && maskedTextBox4.MaskCompleted && comboBox3.Text != "") { button4.Enabled = true; }
            else { button4.Enabled = false; }
        }

        private void maskedTextBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "" && textBox7.Text != "" && textBox1.Text != "" && maskedTextBox5.MaskCompleted && maskedTextBox4.MaskCompleted && comboBox3.Text != "") { button4.Enabled = true; }
            else { button4.Enabled = false; }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            phoneNumber = comboBox4.Text;

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT tariff_id, surname, `name`, patronymic, age, series_and_number_passport FROM `client` WHERE phone_number = '{helper("num", comboBox4.Text)}';", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    int tariff_id = int.Parse(dt.Rows[0][0].ToString());

                    label22.Text = dt.Rows[0][1].ToString() + " " + dt.Rows[0][2].ToString() + "\n" + dt.Rows[0][3].ToString();
                    label17.Text = "Возраст: " + dt.Rows[0][4].ToString();
                    label16.Text = "Серия и номер паспорта:\n" + dt.Rows[0][5].ToString();

                    cmd = new MySqlCommand($"SELECT `name` FROM tariff WHERE id_tariff = '{tariff_id}';", con);
                    da = new MySqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    label19.Text = "Тариф: " + dt.Rows[0][0].ToString();
                }
                catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Невозможно удалить клиента", "Клиенты", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clients_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("Вы действительно хотите вернуться в меню?", "Клиенты", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = !(res == DialogResult.Yes);
        }
    }
}
