using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace myav3
{
    public partial class producer : Form
    {
        public producer()
        {
            InitializeComponent();
        }

        string nameProd;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "") { button1.Enabled = true; }
            else { button1.Enabled = false; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void producer_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("Вы действительно хотите вернуться в меню?", "Производители", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = !(res == DialogResult.Yes);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show($"Вы действительно хотите удалить производителя {comboBox1.Text}?", "Тарифы", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(data.connect))
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand($"DELETE FROM producer WHERE (`name` = '{comboBox1.Text}');", con);
                        cmd.ExecuteNonQuery();
                    }

                    comboBoxUpdate();
                    MessageBox.Show("Производитель успешно удалён!", "Тарифы", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show($"Кажется, произошла ошибка...\n{ex}", "Производители", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void producer_Load(object sender, EventArgs e)
        {
            comboBoxUpdate();
            comboBoxUpdate2();
        }

        private void comboBoxUpdate()
        {
            comboBox1.Items.Clear();

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT name FROM producer", con);
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
                MySqlCommand cmd = new MySqlCommand($"SELECT name FROM producer", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBox2.Items.Add(dt.Rows[i][0].ToString());
                }

                comboBox2.SelectedIndex = 0;
                nameProd = comboBox2.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"INSERT INTO producer (`name`, `country`) VALUES ('{textBox1.Text}', '{textBox2.Text}');", con);
                    cmd.ExecuteNonQuery();
                }

                textBox1.Clear(); textBox2.Clear();
                MessageBox.Show("Производитель успешно добавлен!", "Производители", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show($"Кажется, произошла ошибка...\n{ex}", "Производители", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxUpdate();
            comboBoxUpdate2();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"UPDATE producer SET name = '{comboBox2.Text}', `country` = '{textBox3.Text}' WHERE (`name` = '{nameProd}');", con);
                    cmd.ExecuteNonQuery();
                    comboBoxUpdate2();
                    MessageBox.Show("Производитель успешно изменён", "Редактирвание производителя", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) { MessageBox.Show("Неизвестная ошибка\n\n" + ex.Message, "Редактирвание производителя", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            nameProd = comboBox2.Text;

            try
            {
                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT country FROM producer WHERE `name` = '{nameProd}';", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    textBox3.Text = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex) { MessageBox.Show("Неизвестная ошибка\n\n" + ex.Message, "Редактирвание производителя", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "" && textBox3.Text != "") { button6.Enabled = true; }
            else {  button6.Enabled = false; }
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "" && textBox3.Text != "") { button6.Enabled = true; }
            else { button6.Enabled = false; }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "") { button1.Enabled = true; }
            else { button1.Enabled = false; }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if (l != 8 && (l < 'A' || l > 'z') && l != ' ')
            { e.Handled = true; }
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if (l != 8 && (l < 'A' || l > 'z') && l != ' ')
            { e.Handled = true; }
        }
    }
}
