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

namespace Menu_14
{

    public partial class questionnaireEdit : Form
    {
        private string connectionString = "Server=localhost;Database=snz_flora;Uid=root;Pwd=root;";
        private string currentNameLat = string.Empty;

        public questionnaireEdit()
       {
          InitializeComponent();
        }

        // Метод для установки текста в richTextBoxEdit
        public void SetQuestionnaireText(string text)
        {
            if (richTextBoxEdit != null)
                richTextBoxEdit.Text = text;
        }

        // Метод для сохранения name_lat
        public void SetNameLat(string nameLat)
        {
            currentNameLat = nameLat;
        }

        // Кнопка "Отказ"
        private void buttonRejection_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Кнопка "Сохранить"
        private void buttonSave_Click(object sender, EventArgs e)
        {

        }
        private void buttonRejection_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private bool UpdateQuestionnaireInDB(string nameLat, string questionnaireText)
        {
            string query = "UPDATE plants SET questionnaire = @questionnaire WHERE name_lat = @name_lat";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@questionnaire", questionnaireText);
                cmd.Parameters.AddWithValue("@name_lat", nameLat);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка обновления БД: " + ex.Message);
                    return false;
                }
            }
        }

        private void buttonSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentNameLat))
            {
                MessageBox.Show("Ошибка: name_lat не указан.");
                return;
            }

            string updatedText = richTextBoxEdit.Text.Trim();
            bool success = UpdateQuestionnaireInDB(currentNameLat, updatedText);

            if (success)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при сохранении данных в базу.");
            }
        }
    }
}
