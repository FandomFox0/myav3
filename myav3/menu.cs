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
    public partial class menu : Form
    {
        private Timer inactivityTimer;
        private int inactivityThreshold = 30; // Секунды
        public menu()
        {
            InitializeComponent();
            InitializeInactivityTimer();
        }
        private void InitializeInactivityTimer()
        {
            inactivityTimer = new Timer();
            inactivityTimer.Interval = 1000; // Проверяем каждую секунду
            inactivityTimer.Tick += InactivityTimer_Tick;
            inactivityTimer.Start();
        }
        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            inactivityThreshold--;
            if (inactivityThreshold <= 0)
            {
                inactivityTimer.Stop();
                ShowLoginForm(); // Показать форму авторизации
            }
        }
        private void ShowLoginForm()
        {
            this.Hide(); // Скрыть текущую форму
            autho loginForm = new autho(); // Предположим, ваша форма авторизации называется LoginForm
            loginForm.ShowDialog(); // Показать форму авторизации
            this.Close(); // Закрыть текущую форму после выхода из формы авторизации
        }
        private void ResetInactivityTimer()
        {
            inactivityThreshold = 30; // Сбросить таймер на 30 секунд
        }

        string search = "";
        string sorting = "ASC";
        int productBalance;
        int dgRow; int dgRow2; int dgROWS;
        int sum;
        public int minScren = 0;
        public int maxScren = 10;
        private int currentPage = 1;
        private int pageSize = 20;
        private int totalRecords = 0;


        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menu_Load(object sender, EventArgs e)
        {
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView2.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            see();
        }

        private void see()
        {
            dataGridView1.Rows.Clear();
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();

                MySqlCommand countCmd = new MySqlCommand($"SELECT COUNT(*) FROM product WHERE product.`name` like '%{search}%';", con);
                totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());
                int offset = (currentPage - 1) * pageSize;
                MySqlCommand cmd = new MySqlCommand($"SELECT product.`name`, cost, discount, producer.`name`, photo, balance FROM product " +
                                                    $"INNER JOIN producer ON producer_id = producer.id_producer " +
                                                    $"WHERE product.`name` like '%{search}%' LIMIT {pageSize} OFFSET {offset};", con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j != 4)
                        {
                            dataGridView1.Rows[i].Cells[j].Value = dt.Rows[i][j].ToString();
                            if (j == 2)
                            {
                                int number = int.Parse(dt.Rows[i][j].ToString());
                                if (number == 0) { dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red; }
                                else if (number > 0 && number <= 10) { dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Yellow; }
                                else { dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.LightGreen; }
                            }
                            else if (j == 5)
                            {
                                int number = int.Parse(dt.Rows[i][j].ToString());
                                if (number == 0) { dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Red; }
                                else if (number > 0 && number <= 5) { dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.Yellow; }
                                else { dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.LightGreen; }
                            }

                        }
                        else
                        {
                            string photo = dt.Rows[i][j].ToString();
                            if (photo == "") { photo = "null.png"; }
                            Image sketch = new Bitmap($@"{data.path}\products\{photo}");
                            dataGridView1.Rows[i].Cells[j].Value = sketch;
                        }
                    }
                }
                label4.Text = $"{dt.Rows.Count} из {totalRecords}";
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ActiveControl = null;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetInactivityTimer();
            dgRow = dataGridView1.CurrentCell.RowIndex;
            if (Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[5].Value) != 0)
            {
                if (dataGridView2.Rows.Count > 0)
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        if (dataGridView2.Rows[i].Cells[0].Value.ToString() == dataGridView1.Rows[dgRow].Cells[0].Value.ToString())
                        {
                            if (Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[5].Value) > Convert.ToInt32(dataGridView2.Rows[i].Cells[1].Value))
                            {
                                dataGridView2.Rows[i].Cells[1].Value = Convert.ToInt32(dataGridView2.Rows[i].Cells[1].Value) + 1;
                                dgROWS = i + 1;
                                goto skip;
                            }
                            else { MessageBox.Show("Нельзя добавить товаров больше, чем есть на складе!", "Товары", MessageBoxButtons.OK, MessageBoxIcon.Information); goto skip; }
                        }
                    }
                    dataGridView2.Rows.Add();
                    dgROWS = dataGridView2.Rows.Count;
                    dataGridView2.Rows[dgROWS - 1].Cells[0].Value = dataGridView1.Rows[dgRow].Cells[0].Value.ToString();
                    dataGridView2.Rows[dgROWS - 1].Cells[1].Value = "1";

                skip:
                    if (Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[2].Value) != 0)
                    {
                        sum = Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[1].Value) - (Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[1].Value) / 100 * Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[2].Value));
                    }
                    else { sum = Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[1].Value); }

                    dataGridView2.Rows[dgROWS - 1].Cells[2].Value = sum * Convert.ToInt32(dataGridView2.Rows[dgROWS - 1].Cells[1].Value);
                    sumUpdate();
                }
                else
                {
                    dataGridView2.Rows.Add();
                    dgROWS = dataGridView2.Rows.Count;
                    dataGridView2.Rows[dgROWS - 1].Cells[0].Value = dataGridView1.Rows[dgRow].Cells[0].Value.ToString();
                    dataGridView2.Rows[dgROWS - 1].Cells[1].Value = "1";

                    if (Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[2].Value) != 0)
                    {
                        sum = Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[1].Value) - (Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[1].Value) / 100 * Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[2].Value));
                    }
                    else { sum = Convert.ToInt32(dataGridView1.Rows[dgRow].Cells[1].Value); }

                    dataGridView2.Rows[dgROWS - 1].Cells[2].Value = sum * Convert.ToInt32(dataGridView2.Rows[dgROWS - 1].Cells[1].Value);
                    sumUpdate();
                }


            }
            else { MessageBox.Show("Товар отсутствует на складе!", "Товары", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetInactivityTimer();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[dgRow2].DefaultCellStyle.BackColor = Color.White;
            }

            dgRow2 = dataGridView2.CurrentCell.RowIndex;
            dataGridView2.Rows[dgRow2].DefaultCellStyle.BackColor = Color.Pink;

            label1.Enabled = true; label2.Enabled = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            ResetInactivityTimer();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView2.Rows[dgRow2].Cells[0].Value.ToString() == dataGridView1.Rows[i].Cells[0].Value.ToString())
                {
                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value) != 0)
                    {
                        sum = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value) - (Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value) / 100 * Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value));
                    }
                    else { sum = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value); }

                    productBalance = Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
                    dataGridView2.Rows[dgRow2].Cells[2].Value = sum * (Convert.ToInt32(dataGridView2.Rows[dgRow2].Cells[1].Value) + 1);
                    sumUpdate();
                    break;
                }
            }

            if (Convert.ToUInt32(dataGridView2.Rows[dgRow2].Cells[1].Value) < productBalance)
            {
                dataGridView2.Rows[dgRow2].Cells[1].Value = Convert.ToInt32(dataGridView2.Rows[dgRow2].Cells[1].Value) + 1;
            }
            else { MessageBox.Show("Нельзя добавить товаров больше, чем есть на складе!", "Товары", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            ResetInactivityTimer();
            if (Convert.ToUInt32(dataGridView2.Rows[dgRow2].Cells[1].Value) > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView2.Rows[dgRow2].Cells[0].Value.ToString() == dataGridView1.Rows[i].Cells[0].Value.ToString())
                    {
                        if (Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value) != 0)
                        {
                            sum = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value) - (Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value) / 100 * Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value));
                        }
                        else { sum = Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value); }
                    }
                }
                dataGridView2.Rows[dgRow2].Cells[1].Value = Convert.ToInt32(dataGridView2.Rows[dgRow2].Cells[1].Value) - 1;
                dataGridView2.Rows[dgRow2].Cells[2].Value = sum * (Convert.ToInt32(dataGridView2.Rows[dgRow2].Cells[1].Value));
                if (Convert.ToUInt32(dataGridView2.Rows[dgRow2].Cells[1].Value) == 0)
                {
                    dataGridView2.Rows.RemoveAt(dgRow2);
                }
                sumUpdate();
            }
            
            if (dataGridView2.Rows.Count == 0) { label1.Enabled = false; label2.Enabled = false; }
        }

        private void sumUpdate()
        {
            ResetInactivityTimer();
            int sum = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                sum += Convert.ToInt32(dataGridView2.Rows[i].Cells[2].Value);
            }
            label3.Text = sum.ToString();
        }

        private void GoToPage(int page)
        {
            ResetInactivityTimer();
            if (page < 1 || page > (totalRecords + pageSize - 1) / pageSize) return;
            currentPage = page;
            see(); // Обновляем данные
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ResetInactivityTimer();
            GoToPage(currentPage + 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ResetInactivityTimer();
            GoToPage(currentPage - 1);
        }

        private void label7_Click(object sender, EventArgs e)
        {
            ResetInactivityTimer();
            GoToPage(1);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            ResetInactivityTimer();
            GoToPage(2);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            ResetInactivityTimer();
            GoToPage(3);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            ResetInactivityTimer();
            GoToPage(4);
        }
    }
}
