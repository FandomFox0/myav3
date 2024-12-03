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

namespace myav3
{
    public partial class employees : Form
    {
        public employees()
        {
            InitializeComponent();
        }

        string status = "";
        string loginEmp = "";

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void employees_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("Вы действительно хотите вернуться в меню?", "Сотрудники", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = !(res == DialogResult.Yes);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted) { button3.Enabled = true;  }
            else { button3.Enabled = false; }
        }

        private void employees_Load(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 1;

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT region, city, street, house FROM department;", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    comboBox1.Items.Add(row["region"] + " обл., " + row["city"] + ", ул." + row["street"] + ", дом " + row["house"]);
                }
            }

            comboBox1.SelectedIndex = 0;
            comboBoxUpdate56();
            comboBox5.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Pass = "";
            string simvols = "1234567890qwertyuiopasdfghjklzxcvbnm";
            Random rnd = new Random();
            for (int i = 0; i < rnd.Next(8, 30); i = i + 1)
            {
                Pass = Pass + simvols[rnd.Next(0, simvols.Length)];
            }
            textBox2.Text = Pass;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT login FROM employee WHERE login = '{textBox1.Text}';", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    if (dt.Rows.Count != 1) 
                    {
                        cmd = new MySqlCommand($"INSERT INTO `employee` (`login`, `password`, `role_id`, `department_id`, `surname`, `name`, `patronymic`, `age`, `phone_number`) " +
                                                $"VALUES ('{textBox1.Text}', '{data.CreateMD5(textBox2.Text)}', '{helper("role")}', '{helper("department")}', '{textBox3.Text}', '{textBox4.Text}', '{textBox5.Text}', '{textBox6.Text}', '{helper("num")}');", con);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Вы успешно добавили нового сотрудника!", "Сотрудники", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        textBox1.Clear(); textBox2.Clear(); textBox3.Clear(); textBox4.Clear(); textBox5.Clear(); textBox6.Clear(); maskedTextBox1.Clear();
                        comboBoxUpdate56();
                    }
                    else
                    {
                        MessageBox.Show("Сотрудник с данным логином уже существует!", "Сотрудники", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Произошла неизвестная ошибка\n\n" + ex.Message, "Сотрудники", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private string helper (string delay)
        {
            switch (delay)
            {
                case "num":
                    delay = maskedTextBox1.Text;
                    delay = delay.Replace(" ", "");
                    delay = delay.Replace(")", "");
                    delay = delay.Replace("(", "");
                    delay = delay.Replace("-", "");
                    break;

                case "role":
                    delay = Convert.ToString(comboBox2.SelectedIndex + 1);
                    break;

                case "department":
                    string adress = comboBox1.Text.Replace(" обл., ", " ");
                    adress = adress.Replace(", ул.", " ");
                    adress = adress.Replace(", дом ", " ");
                    string region = adress.Split()[0];
                    string city = adress.Split()[1];
                    string street = adress.Split()[2];
                    string house = adress.Split()[3];

                    using (MySqlConnection con = new MySqlConnection(data.connect))
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand($"SELECT id_department FROM department " +
                                                            $"WHERE region = '{region}' " +
                                                            $"AND city = '{city}' " +
                                                            $"AND street = '{street}' " +
                                                            $"AND house = '{house}';",con);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0) { delay = dt.Rows[0][0].ToString(); }
                        else { MessageBox.Show("Неизвестная ошибка.\nДанное отделение не найдено.", "Сотрудники", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }

                        break;
            }

            return delay;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "" && maskedTextBox1.MaskCompleted) { button3.Enabled = true; }
            else { button3.Enabled = false; }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string number = maskedTextBox2.Text;
                number = number.Replace(" ", "");
                number = number.Replace(")", "");
                number = number.Replace("(", "");
                number = number.Replace("-", "");

                string adress = comboBox3.Text.Replace(" обл., ", " ");
                adress = adress.Replace(", ул.", " ");
                adress = adress.Replace(", дом ", " ");
                string region = adress.Split()[0];
                string city = adress.Split()[1];
                string street = adress.Split()[2];
                string house = adress.Split()[3];

                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"SELECT id_department FROM department " +
                                                        $"WHERE region = '{region}' " +
                                                        $"AND city = '{city}' " +
                                                        $"AND street = '{street}' " +
                                                        $"AND house = '{house}';", con);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0) { adress = dt.Rows[0][0].ToString(); }
                    else { MessageBox.Show("Неизвестная ошибка.\nДанное отделение не найдено.", "Сотрудники", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }

                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand($"UPDATE employee SET login = '{comboBox5.Text}', password = '{data.CreateMD5(textBox11.Text)}', role_id = '{comboBox4.SelectedIndex + 1}', " +
                                                        $"department_id = '{adress}', surname = '{textBox10.Text}', `name` = '{textBox9.Text}', patronymic = '{textBox8.Text}', " +
                                                        $"age = '{textBox7.Text}', phone_number = '{number}' WHERE (login = '{loginEmp}');", con);
                    cmd.ExecuteNonQuery();
                    comboBoxUpdate56();
                    textBox11.Clear();
                    loginEmp = comboBox5.Text;
                    MessageBox.Show("Вы успешно отредактировали нового сотрудника!", "Сотрудники", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) { MessageBox.Show("Произошла ошибка :(\n\n" + ex.Message, "Сотрудники", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void comboBoxUpdate56()
        {
            comboBox5.Items.Clear();
            comboBox6.Items.Clear();

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT login FROM employee;", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows) 
                { 
                    comboBox5.Items.Add(row["login"]); 
                    comboBox6.Items.Add(row["login"]); 
                }
            }
            comboBox6.SelectedIndex = 0;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            string department_id = "";           

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT role_id, surname, `name`, patronymic, age, phone_number, department_id, status FROM employee WHERE login = '{comboBox6.Text}';", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows[0][0].ToString() == "1") { label27.Text = "Администратор"; }
                else { label27.Text = "Менеджер"; }
                label23.Text = dt.Rows[0][1].ToString() + " " + dt.Rows[0][2].ToString() + "\n" + dt.Rows[0][3].ToString();
                label26.Text = dt.Rows[0][5].ToString();
                label25.Text = dt.Rows[0][4].ToString();
                label21.Text = dt.Rows[0][7].ToString();
                updateColor();
                department_id = dt.Rows[0][6].ToString();
            }

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT region, city, street, house FROM department WHERE id_department = '{department_id}';", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    label24.Text = "Отделение: " + (row["region"] + " обл., " + row["city"] + ", ул." + row["street"] + ", дом " + row["house"]);
                }
            }

            button8.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBox6.Text != data.login)
            {
                if (label21.Text == "Работает")
                {
                    var res = MessageBox.Show($"Вы действительно хотите уволить сотрудника {comboBox6.Text}?", "Сотрудники", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes) { status = "Уволен"; goto yes; }
                    else { goto no; }
                }
                else
                {
                    var res = MessageBox.Show($"Вы действительно хотите вернуть в штат сотрудника {comboBox6.Text}?", "Сотрудники", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes) { status = "Работает"; goto yes; }
                    else { goto no; }
                }

            yes:
                try
                {
                    string id_emp;
                    using (MySqlConnection con = new MySqlConnection(data.connect))
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand($"SELECT id_employee FROM employee WHERE login = '{comboBox6.Text}';", con);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        id_emp = dt.Rows[0][0].ToString();
                    }
                        using (MySqlConnection con = new MySqlConnection(data.connect))
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand($"UPDATE employee SET `status` = '{status}' WHERE (id_employee = '{id_emp}');", con);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Успешный успех!", "Сотрудники", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        label21.Text = status;
                        updateColor();
                    }
                }
                catch (Exception ex) { MessageBox.Show($"Кажется, произошла ошибка...\n{ex}", "Сотрудники", MessageBoxButtons.OK, MessageBoxIcon.Error); }

            no:;             
            }
            else { MessageBox.Show("Вы не можете уволить самого себя!", "Сотрудники", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void updateColor()
        {
            if (label21.Text == "Работает")
            {
                label21.ForeColor = Color.Green;
                button8.Text = "Уволить";
                button8.ForeColor = Color.Red;
            }
            else
            {
                label21.ForeColor = Color.Red;
                button8.Text = "Вернуть";
                button8.ForeColor = Color.Green;
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox11.Clear();
            loginEmp = comboBox5.Text;

            string department_id = "";

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT role_id, surname, `name`, patronymic, age, phone_number, department_id FROM employee WHERE login = '{loginEmp}';", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows[0][0].ToString() == "1") { comboBox4.SelectedIndex = 0; }
                else { comboBox4.SelectedIndex = 1; }
                textBox10.Text = dt.Rows[0][1].ToString();
                textBox9.Text = dt.Rows[0][2].ToString();
                textBox8.Text = dt.Rows[0][3].ToString();
                textBox7.Text = dt.Rows[0][4].ToString();
                maskedTextBox2.Text = dt.Rows[0][5].ToString();
                department_id = dt.Rows[0][6].ToString();
            }

            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                comboBox3.Items.Clear();

                con.Open();
                MySqlCommand cmd = new MySqlCommand($"SELECT region, city, street, house FROM department;", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    comboBox3.Items.Add(row["region"] + " обл., " + row["city"] + ", ул." + row["street"] + ", дом " + row["house"]);
                }

                comboBox3.SelectedIndex = Convert.ToInt32(department_id) - 1;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string Pass = "";
            string simvols = "1234567890qwertyuiopasdfghjklzxcvbnm";
            Random rnd = new Random();
            for (int i = 0; i < rnd.Next(8, 30); i = i + 1)
            {
                Pass = Pass + simvols[rnd.Next(0, simvols.Length)];
            }
            textBox11.Text = Pass;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != "" && textBox10.Text != "" && textBox9.Text != "" && maskedTextBox2.MaskCompleted && textBox7.Text != "" && comboBox5.Text != "") { button5.Enabled = true; }
            else { button5.Enabled = false; }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != "" && textBox10.Text != "" && textBox9.Text != "" && maskedTextBox2.MaskCompleted && textBox7.Text != "" && comboBox5.Text != "") { button5.Enabled = true; }
            else { button5.Enabled = false; }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != "" && textBox10.Text != "" && textBox9.Text != "" && maskedTextBox2.MaskCompleted && textBox7.Text != "" && comboBox5.Text != "") { button5.Enabled = true; }
            else { button5.Enabled = false; }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != "" && textBox10.Text != "" && textBox9.Text != "" && maskedTextBox2.MaskCompleted && textBox7.Text != "" && comboBox5.Text != "") { button5.Enabled = true; }
            else { button5.Enabled = false; }
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if (textBox11.Text != "" && textBox10.Text != "" && textBox9.Text != "" && maskedTextBox2.MaskCompleted && textBox7.Text != "" && comboBox5.Text != "") { button5.Enabled = true; }
            else { button5.Enabled = false; }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != "" && textBox10.Text != "" && textBox9.Text != "" && maskedTextBox2.MaskCompleted && textBox7.Text != "" && comboBox5.Text != "") { button5.Enabled = true; }
            else { button5.Enabled = false; }
        }

        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != "" && textBox10.Text != "" && textBox9.Text != "" && maskedTextBox2.MaskCompleted && textBox7.Text != "" && comboBox5.Text != "") { button5.Enabled = true; }
            else { button5.Enabled = false; }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'A' || l > 'z') && l != 8 && (l < '0' || l > '9'))
            { e.Handled = true; }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'A' || l > 'z') && l != 8 && (l < '0' || l > '9'))
            { e.Handled = true; }
        }

        private void comboBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'A' || l > 'z') && l != 8 && (l < '0' || l > '9'))
            { e.Handled = true; }
        }

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'A' || l > 'z') && l != 8 && (l < '0' || l > '9'))
            { e.Handled = true; }
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

        private void textBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != 8)
            { e.Handled = true; }
        }

        private void textBox9_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != 8)
            { e.Handled = true; }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            char l = e.KeyChar;
            if ((l < 'А' || l > 'я') && l != 8)
            { e.Handled = true; }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
