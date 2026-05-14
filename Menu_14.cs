//using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
// using System.IO.Compression.FileSystem;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Menu_14
{
    [DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
    public partial class Menu_14 : Form
    {
        public Menu_14()
        {
            InitializeComponent();
            dataGridView1.CellClick += DataGridView1_CellClick; //  добавлено событие, нажатие на поле таблицы БД
                                                                //        button1_Click(null, null);
            button1_Click(this, EventArgs.Empty);
        }

        private void оснвныеДанныеИзBDMySQLToolStripMenuItem_Click(object sender, EventArgs e)   // выбор архива из папки
        {
            //    MessageBox.Show("Здесь будт Город Сад !!","И солнце встаёт над рекой...");
            setMethods.choiceFile("catExport");  // выбор архива из папки

        }
        // для фиксации изменений 26.03
        private void записьПротоколаToolStripMenuItem_Click(object sender, EventArgs e) // тест метода. проверка метода Protocol-a
        {
            setMethods.ProtocolT("Первая запись! ", " Кнопка в Меню", "test_XX");
        }

        private void bDПастенийВMySQLToolStripMenuItem_Click(object sender, EventArgs e) // очистка таблицы plants в MySQL
        {
            setMethods.cleanTable("plants");
        }

        private void папкиСФотографиямиToolStripMenuItem_Click(object sender, EventArgs e) // очиста папаки с фотогрвфиями
        {
            string? targetDir = ConfigurationManager.AppSettings["catSubCutFotos"];
            // Проверяем, что директория указана
            if (string.IsNullOrEmpty(targetDir))
            {
                Console.WriteLine("Ошибка: настройка 'catSubCutFotos' не найдена в конфигурации");
                return;
            }

            // Проверяем, что директория существует
            if (!Directory.Exists(targetDir))
            {
                Console.WriteLine($"Ошибка: директория '{targetDir}' не существует");
                return;
            }

            // Удалить все файлы
            foreach (string file in Directory.GetFiles(targetDir))
            {
                try
                {
                    File.Delete(file);
                    Console.WriteLine($"Удален: {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка удаления {file}: {ex.Message}");
                }
            }
            //// Удалить все файлы
            //foreach (string file in Directory.GetFiles(targetDir))
            //{
            //    File.Delete(file);
            //}

            //// Удалить все подпапки (рекурсивно)
            //foreach (string directory in Directory.GetDirectories(targetDir))
            //{
            //    Directory.Delete(directory, true);
            //}
        }

        private void namelatListToolStripMenuItem_Click(object sender, EventArgs e)  // проверка наличие названия name_lat в базе MySQL
        {
            var ps = setMethods.findName_latPlants("Acer negundo");

            if (ps.Count > 0)
            {
                DateTime dt = ps[0].time_last;
                MessageBox.Show(Convert.ToString(ps.Count), "однако...");
            }
        }

        private void проверкаНаличияПодкаталогаToolStripMenuItem_Click(object sender, EventArgs e) //  тест метода. 
        {
            setMethods.subfolderAndFiles("2024-08-12T06_14_59Z", "Solidago virgaurea");
        }

        private void вывестиИзMySQLВТаблицуToolStripMenuItem_Click(object sender, EventArgs e) // кнопка в главном подменю. что это будет? 
        { }
        private void button1_Click(object sender, EventArgs e) // вывод данных из БД в таблицу dataGridView1
        {
            string connectionString = "Server=localhost;Database=snz_flora;Uid=root;Pwd=root;";
            string query = "SELECT name_ru, name_lat, time_first, time_last, questionnaire  FROM plants ORDER BY name_ru";
            // FROM plants ORDER BY name_ru
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    // Привязка данных к DataGridView
                    dataGridView1.DataSource = dataTable;

                    // Настройка заголовков столбцов (опционально)
                    dataGridView1.Columns["name_ru"]?.HeaderText = "Русское название";
                    dataGridView1.Columns["name_lat"]?.HeaderText = "Латинское название";
                    dataGridView1.Columns["time_first"]?.HeaderText = "Первая фиксация";
                    dataGridView1.Columns["time_last"]?.HeaderText = "Последняя фиксация";
                    dataGridView1.Columns["questionnaire"]?.HeaderText = "Типа, листовка";
                    // Выделяем цветом поле name_lat для понимания, что оно кликабельное
                    dataGridView1.Columns["name_lat"]?.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    dataGridView1.Columns["name_lat"]?.DefaultCellStyle.Font =
                        new System.Drawing.Font(dataGridView1.Font, System.Drawing.FontStyle.Underline);
                    dataGridView1.Columns["questionnaire"]?.DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                    dataGridView1.Columns["questionnaire"]?.DefaultCellStyle.Font =
                        new System.Drawing.Font(dataGridView1.Font, System.Drawing.FontStyle.Underline);

                    // Автоматическая подстройка ширины столбцов
                    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DataGridView1_CellClick(object? sender, DataGridViewCellEventArgs e) // Обработчик события клика мышкой по ячейке
        {
            // Проверяем, что клик не по заголовку
            if (e.RowIndex < 0) return;
            {
                // Действия для другой колонки,  Проверка на валидные индексы
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    int aimColumn = e.ColumnIndex;
                    switch (aimColumn)
                    {
                        case 0:
                            var value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value;
                            string Value = value?.ToString() ?? "";
                            OpenInFastStone(Value);
                            break;
                        case 4:
                            var value1 = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                            string listXX = value1?.ToString() ?? "";
                            var value3 = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 3].Value;
                            string nameLat = value3?.ToString() ?? "";
                            var value4 = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 4].Value;
                            string nameRu = value4?.ToString() ?? "";
                            FormLinks.workFormLinks(nameRu, nameLat, listXX);   // вызов метода в FormLinks.cs
                            break;
                        default:
                            MessageBox.Show($"Клик ячейке - не определена: Row={e.RowIndex}, Column={e.ColumnIndex}");
                            break;
                    }
                }
            }

        }
        private void OpenInFastStone(string latinName) // Метод для открытия изображения в FastStone Image Viewer
        {
            try
            {
                string? parentFolder = ConfigurationManager.AppSettings["catSubCutFotos"];
                // Путь к FastStone Image Viewer (обычно в Program Files)
                string? fastStonePath = ConfigurationManager.AppSettings["FastStone"];

                if (!System.IO.File.Exists(fastStonePath))
                {
                    MessageBox.Show("FastStone Image Viewer не найден... Проверьте путь к программе.",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Формируем путь к изображению
                // Предполагаем, что изображения хранятся в папке Images с названиями {latinName}.jpg
                string imagePath = parentFolder + latinName;

                // Запускаем FastStone с параметром (путь к изображению)
                Process.Start(fastStonePath, $"\"{imagePath}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске FastStone: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void изСмартфонаВБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setMethods.choiceFile("catExport");  // выбор архива из папки
            // setMethods.choiceFile();
            //  public static void choiceFile(string folderPath) // поиск первого архива
        }

        private void фотографииРастенийВыгрузкаАрхивToolStripMenuItem_Click(object sender, EventArgs e) // выгрузка из БД всех растений
        {
            string? sourceDirectory = ConfigurationManager.AppSettings["catSubCutFotos"];
            string? sevenZipPath = ConfigurationManager.AppSettings["UnZip"];

            // Диалог выбора места сохранения
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "7-Zip архив (*.7z)|*.7z|ZIP архив (*.zip)|*.zip",
                Title = "Сохранить архив как",
                FileName = $"{Path.GetFileName(sourceDirectory)}_{DateTime.Now:yyyyMMdd_HHmmss}"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string archivePath = saveFileDialog.FileName;
                string archiveType = Path.GetExtension(archivePath).ToLower() == ".zip" ? "zip" : "7z";

                string arguments = $"a -t{archiveType} \"{archivePath}\" \"{sourceDirectory}\" -mx5";

                Process.Start(new ProcessStartInfo
                {
                    FileName = sevenZipPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true
                })?.WaitForExit();
                FileInfo archiveInfo = new FileInfo(archivePath);
                setMethods.ProtocolT($"Архив сохранен: {archivePath}", $"Размер архива: {archiveInfo.Length / 1024.0 / 1024.0:F2} MB", "Фотографии растений");
                Console.WriteLine($"Архив создан: {archivePath}");
            }
        }

        private string GetDebuggerDisplay()
        {
            return ToString();
        }

        private void протоколИзMySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setMethods.cleanTable("protocol");
        }

        private void таблицаСсылокВыгрузкаИзBDВФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setMethods.ExportDatabase();
        }

        private void инструкцияПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string? filePath = ConfigurationManager.AppSettings["instrFloraSNZ"];

            // System.Diagnostics.Process.Start(filePath);
            if (!string.IsNullOrEmpty(filePath))
            {
                //  System.Diagnostics.Process.Start(filePath);
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            else
            {
                // Обработка случая, когда путь не задан
                MessageBox.Show("Путь к файлу не указан");
            }
        }

        private void дополнеиередакторСсылкИзИнтеретаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // начнём с утра приделывать код от Виталия
        }

        private void заменадополнеиеФотографииРастенийToolStripMenuItem_Click(object sender, EventArgs e) // добавим фото в БД из ПК
        {
            // сегодня 14.05.26 попробуем ещё раз
            // 1) Выбор файлов в указанном каталоге
            string? catSubCutFotos = ConfigurationManager.AppSettings["catSubCutFotos"];

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = catSubCutFotos;
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Выберите файлы фотографий растений";
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            // 2) Создание массива AddFotos
            List<string[]> AddFotos = new List<string[]>();

            foreach (string filePath in openFileDialog.FileNames)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                string fileName = Path.GetFileName(filePath);
                DateTime creationDate = fileInfo.CreationTime;

                string[] fotoRecord = new string[5];
                fotoRecord[0] = fileName;           // Название файла
                fotoRecord[1] = filePath;            // Полный путь файла
                fotoRecord[2] = fileInfo.Length.ToString(); // Размер файла
                fotoRecord[3] = creationDate.ToString("yyyy-MM-dd HH:mm:ss"); // Дата создания
                fotoRecord[4] = "не обработано";     // Для указаний

                AddFotos.Add(fotoRecord);
            }

            // Строка подключения к БД
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
            string? catSubCutFotosPath = ConfigurationManager.AppSettings["catSubCutFotos"];

            // 3) Цикл обработки
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                foreach (string[] foto in AddFotos)
                {
                    string fileName = foto[0];
                    string filePath = foto[1];
                    string creationDateStr = foto[3];
                    DateTime fotoFromArray = DateTime.Parse(creationDateStr);

                    // Поиск совпадения в таблице plants по полю name_ru
                    string query = "SELECT id, name_lat, time_last FROM plants WHERE name_ru = @name_ru";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name_ru", Path.GetFileNameWithoutExtension(fileName));

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int plantId = reader.GetInt32("id");
                            string nameLat = reader.GetString("name_lat");
                            DateTime? timeLast = reader.IsDBNull(reader.GetOrdinal("time_last"))
                                ? (DateTime?)null
                                : reader.GetDateTime("time_last");
                            reader.Close();

                            // Составление пути подкаталога
                            //        string targetDirectory = Path.Combine(catSubCutFotosPath, nameLat);
                            string targetDirectory = Path.Combine(catSubCutFotosPath ?? string.Empty, nameLat ?? string.Empty);
                            // Поиск самой новой фотографии в подкаталоге
                            DateTime? fotoFromBD = null;
                            if (Directory.Exists(targetDirectory))
                            {
                                var imageFiles = Directory.GetFiles(targetDirectory, "*.*")
                                    .Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") ||
                                               f.EndsWith(".png") || f.EndsWith(".bmp") || f.EndsWith(".gif"));

                                foreach (string existingPhoto in imageFiles)
                                {
                                    DateTime? takenDate = GetDateTakenFromImage(existingPhoto);
                                    if (takenDate.HasValue)
                                    {
                                        if (!fotoFromBD.HasValue || takenDate.Value > fotoFromBD.Value)
                                            fotoFromBD = takenDate.Value;
                                    }
                                    else
                                    {
                                        // Если нет даты съёмки, используем дату создания файла
                                        DateTime fileCreationDate = File.GetCreationTime(existingPhoto);
                                        if (!fotoFromBD.HasValue || fileCreationDate > fotoFromBD.Value)
                                            fotoFromBD = fileCreationDate;
                                    }
                                }
                            }

                            // 3.1) Если фотография из массива новее
                            if (!fotoFromBD.HasValue || fotoFromArray > fotoFromBD.Value)
                            {
                                // Создание подкаталога если не существует
                                if (!Directory.Exists(targetDirectory))
                                    Directory.CreateDirectory(targetDirectory);

                                // Копирование файла
                                string targetPath = Path.Combine(targetDirectory, fileName);
                                File.Copy(filePath, targetPath, true);

                                // Обновление массива
                                foto[4] = "скопированно";

                                // Обновление поля time_last в таблице
                                string updateQuery = "UPDATE plants SET time_last = @time_last WHERE id = @id";
                                MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                                updateCommand.Parameters.AddWithValue("@time_last", fotoFromArray);
                                updateCommand.Parameters.AddWithValue("@id", plantId);
                                updateCommand.ExecuteNonQuery();
                            }
                            // 3.2) Иначе
                            else
                            {
                                foto[4] = "не подходит";
                            }
                        }
                        else
                        {
                            reader.Close();
                            foto[4] = "растение не найдено";
                        }
                    }
                }
            }

            // 4) Запись массива в файл
            string outputFilePath = @"D:\ALLZSV\myHomeland\floraZSV\addFotos.txt";
            using (StreamWriter writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            {
                // Заголовки
                writer.WriteLine("Название файла\tПолный путь\tРазмер файла\tДата создания\tДля указаний");

                // Данные
                foreach (string[] foto in AddFotos)
                {
                    writer.WriteLine(string.Join("\t", foto));
                }
            }

            MessageBox.Show($"Обработка завершена. Результат сохранен в файл:\n{outputFilePath}",
                            "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // Вспомогательный метод для получения даты съёмки из EXIF
        private DateTime? GetDateTakenFromImage(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (Image image = Image.FromStream(fs, false, false))
                {
                    PropertyItem? propItem = image.GetPropertyItem(0x9003);
                    if (propItem != null && propItem.Value != null)
                    {
                        byte[] bytes = propItem.Value;
                        string dateTaken = Encoding.ASCII.GetString(bytes).TrimEnd('\0');
                        if (!string.IsNullOrEmpty(dateTaken))
                        {
                            return DateTime.ParseExact(dateTaken, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
            catch
            {
                // Если не удалось прочитать EXIF, возвращаем null
                return null;
            }
            return null;
        }
    }
    
}
