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
    public partial class tariffs : Form
    {
        public tariffs()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tariffs_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("Вы действительно хотите вернуться в меню?", "Тарифы", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = !(res == DialogResult.Yes);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "") { button1.Enabled = true; }
        }

        private void tariffs_Load(object sender, EventArgs e)
        {
            comboBoxUpdate();
            comboBoxUpdate2();

            comboBox3.SelectedIndex = 0;
        }

        private void comboBoxUpdate()
        {
            comboBox1.Items.Clear();

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT name FROM tariff", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBox1.Items.Add(dt.Rows[i][0].ToString());
                }

                comboBox1.SelectedIndex = 0;
            }
        }

        private void comboBoxUpdate2()
        {
            comboBox2.Items.Clear();

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT name FROM tariff", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBox2.Items.Add(dt.Rows[i][0].ToString());
                }

                comboBox2.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"INSERT INTO tariff (`name`, monthly_payment, `description`, relevance) VALUES ('{textBox1.Text}', '{textBox2.Text}', '{textBox3.Text}', 'Актуален');", con);
                    cmd.ExecuteNonQuery();
                }
                
                textBox1.Clear(); textBox2.Clear(); textBox3.Clear();
                MessageBox.Show("Тариф успешно добавлен!", "Тарифы", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show($"Кажется, произошла ошибка...\n{ex}", "Тарифы", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "") { button1.Enabled = true; }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "") { button1.Enabled = true; }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show($"Вы действительно хотите удалить тариф {comboBox1.Text}?", "Тарифы", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(data.connect))
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand($"DELETE FROM tariff WHERE (`name` = '{comboBox1.Text}');", con);
                        cmd.ExecuteNonQuery();
                    }

                    comboBoxUpdate();
                    MessageBox.Show("Тариф успешно удалён!", "Тарифы", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show($"Кажется, произошла ошибка...\n{ex}", "Тарифы", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxUpdate();
            comboBoxUpdate2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT `name`, monthly_payment, `description`, relevance FROM tariff WHERE `name` = '{comboBox2.Text}'", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    textBox5.Text = dt.Rows[0][1].ToString();
                    textBox4.Text = dt.Rows[0][2].ToString().Replace("rn", "\n");
                    if (dt.Rows[0][3].ToString() == "Актуален") { comboBox3.SelectedIndex = 0; }
                    else { comboBox3.SelectedIndex = 1; }

                    button6.Enabled = false;
                }
            }
            catch (Exception ex) { MessageBox.Show("Неизвестная ошибка\n\n" + ex.Message, "Редактирвание тарифа", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"UPDATE tariff SET monthly_payment = '{textBox5.Text}', `description` = '{textBox4.Text}', relevance = '{comboBox3.Text}' WHERE (`name` = '{comboBox2.Text}');", con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Тариф успешно изменён", "Редактирование тарифа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) { MessageBox.Show("Неизвестная ошибка\n\n" + ex.Message, "Редактирование тарифа", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            button6.Enabled = true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text != "" && textBox4.Text != "") { button6.Enabled = true; }
            else { button6.Enabled = false; }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox5.Text != "" && textBox4.Text != "") { button6.Enabled = true; }
            else { button6.Enabled = false; }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
