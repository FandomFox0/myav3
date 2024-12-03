using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myav3
{
    public partial class import : Form
    {
        public import()
        {
            InitializeComponent();
            LoadTableNames();
        }

        private void comboBoxTables_Changed(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void LoadTableNames()
        {
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();
                var cmd = new MySqlCommand("SHOW TABLES;", con);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comboBoxTables.Items.Add(reader[0].ToString());
                    }
                }
            }
        }

        private void ImportCsvToDatabase(string filePath, string tableName)
        {
            int importedRecordsCount = 0;

            try
            {
                using (MySqlConnection con = new MySqlConnection(data.connect))
                {
                    con.Open();

                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        reader.ReadLine();

                        int columnCount = GetColumnCount(con, tableName);

                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (values.Length != columnCount)
                            {
                                MessageBox.Show($"Parameter count mismatch: expected {columnCount}, but got {values.Length}.");
                                return;
                            }

                            var insertCommand = new MySqlCommand($"INSERT INTO {tableName} VALUES ({string.Join(",", values.Select(v => $"'{v}'"))});", con);
                            insertCommand.ExecuteNonQuery();
                            importedRecordsCount++;
                        }
                    }
                }

                MessageBox.Show($"Successfully imported {importedRecordsCount} records.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error importing data: {ex.Message}");
            }
        }
        private int GetColumnCount(MySqlConnection con, string tableName)
        {
            var getColumnCountCommand = new MySqlCommand($"DESCRIBE {tableName}", con);
            int columnCount = 0;
            using (var reader = getColumnCountCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    columnCount++;
                }
            }
            return columnCount;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void import_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show("Вы действительно хотите вернуться в меню?", "Импорт", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            e.Cancel = !(res == DialogResult.Yes);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(data.connect))
            {
                con.Open();
                try
                {
                    string createSchemaScript = @"

                CREATE TABLE `check` (
                    `id_check` int NOT NULL AUTO_INCREMENT,
                    `client_id_client` int NOT NULL,
                    `employee_id` int NOT NULL,
                    `date` varchar(45) NOT NULL,
                    `total_cost` int NOT NULL,
                    `service_id_service1` int NOT NULL,
                    PRIMARY KEY (`id_check`),
                    KEY `fk_check_client1_idx` (`client_id_client`),
                    KEY `fk_check_employee1_idx` (`employee_id`),
                    CONSTRAINT `fk_check_client1` FOREIGN KEY (`client_id_client`) REFERENCES `client` (`id_client`),
                    CONSTRAINT `fk_check_employee1` FOREIGN KEY (`employee_id`) REFERENCES `employee` (`id_employee`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `client` (
                    `id_client` int NOT NULL AUTO_INCREMENT,
                    `tariff_id` int NOT NULL,
                    `surname` varchar(255) NOT NULL,
                    `name` varchar(255) NOT NULL,
                    `patronymic` varchar(255) DEFAULT NULL,
                    `age` int NOT NULL,
                    `phone_number` varchar(45) NOT NULL,
                    `series_and_number_passport` varchar(255) NOT NULL,
                    PRIMARY KEY (`id_client`),
                    KEY `fk_client_tariff1_idx` (`tariff_id`),
                    CONSTRAINT `fk_client_tariff1` FOREIGN KEY (`tariff_id`) REFERENCES `tariff` (`id_tariff`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `compound` (
                    `id_compound` int NOT NULL AUTO_INCREMENT,
                    `check_id` int NOT NULL,
                    `product_id` int NOT NULL,
                    `quantity` int NOT NULL,
                    PRIMARY KEY (`id_compound`),
                    KEY `fk_compound_product1_idx` (`product_id`),
                    KEY `fk_compound_check1_idx` (`check_id`),
                    CONSTRAINT `fk_compound_check1` FOREIGN KEY (`check_id`) REFERENCES `check` (`id_check`),
                    CONSTRAINT `fk_compound_product1` FOREIGN KEY (`product_id`) REFERENCES `product` (`id_product`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `department` (
                    `id_department` int NOT NULL AUTO_INCREMENT,
                    `region` varchar(255) NOT NULL,
                    `city` varchar(255) NOT NULL,
                    `street` varchar(255) NOT NULL,
                    `house` varchar(255) NOT NULL,
                    PRIMARY KEY (`id_department`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `employee` (
                    `id_employee` int NOT NULL AUTO_INCREMENT,
                    `login` varchar(255) NOT NULL,
                    `password` varchar(255) NOT NULL,
                    `role_id` int NOT NULL,
                    `department_id` int NOT NULL,
                    `surname` varchar(255) NOT NULL,
                    `name` varchar(255) NOT NULL,
                    `patronymic` varchar(255) DEFAULT NULL,
                    `age` int NOT NULL,
                    `phone_number` bigint NOT NULL,
                    `status` varchar(255) NOT NULL,
                    PRIMARY KEY (`id_employee`),
                    KEY `fk_employee_role1_idx` (`role_id`),
                    KEY `fk_employee_department1_idx` (`department_id`),
                    CONSTRAINT `fk_employee_department1` FOREIGN KEY (`department_id`) REFERENCES `department` (`id_department`),
                    CONSTRAINT `fk_employee_role1` FOREIGN KEY (`role_id`) REFERENCES `role` (`id_role`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `producer` (
                    `id_producer` int NOT NULL AUTO_INCREMENT,
                    `name` varchar(255) NOT NULL,
                    `country` varchar(255) NOT NULL,
                    PRIMARY KEY (`id_producer`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `product` (
                    `id_product` int NOT NULL AUTO_INCREMENT,
                    `name` text NOT NULL,
                    `cost` int NOT NULL,
                    `discount` int NOT NULL,
                    `producer_id` int NOT NULL,
                    `description` text NOT NULL,
                    `photo` varchar(255) NOT NULL,
                    `balance` int NOT NULL,
                    PRIMARY KEY (`id_product`),
                    KEY `fk_product_producer1_idx` (`producer_id`),
                    CONSTRAINT `fk_product_producer1` FOREIGN KEY (`producer_id`) REFERENCES `producer` (`id_producer`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `role` (
                    `id_role` int NOT NULL AUTO_INCREMENT,
                    `name` varchar(255) NOT NULL,
                    PRIMARY KEY (`id_role`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

                CREATE TABLE `tariff` (
                    `id_tariff` int NOT NULL AUTO_INCREMENT,
                    `name` varchar(255) NOT NULL,
                    `monthly_payment` int NOT NULL,
                    `description` text NOT NULL,
                    `relevance` varchar(8) NOT NULL,
                    PRIMARY KEY (`id_tariff`)
                ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";

                    using (MySqlCommand command = new MySqlCommand(createSchemaScript, con))
                    {
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Структура базы данных восстановлена успешно!");
                    LoadTableNames();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка восстановления структуры базы данных: {ex.Message}");
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBoxTables.SelectedItem == null)
            {
                MessageBox.Show("Выберите таблицу из списка");
                return;
            }

            string selectedTable = comboBoxTables.SelectedItem.ToString();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    ImportCsvToDatabase(filePath, selectedTable);
                }
            }
        }
    }
}
