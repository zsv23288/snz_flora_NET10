using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
// using System.IO.Compression.FileSystem;
using System.Linq;
using System.Text;
using System.Text.Json;  // Добавьте это для работы с JSON
using System.Net.Http;   // Добавьте это для HTTP запросов
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
            
            if (ps.Count > 0 )
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
        {         }
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
                            var value1 = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex ].Value;
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
                setMethods.ProtocolT($"Архив сохранен: {archivePath}", $"Размер архива: {archiveInfo.Length / 1024.0 / 1024.0:F2} MB","Фотографии растений");
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

        private async void дополнеиередакторСсылкИзИнтеретаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Отключаем пункт меню на время выполнения
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem != null) menuItem.Enabled = false;

            try
            {
                string inputFilePath = "D:\\ALLZSV\\myHomeland\\floraZSV\\DeepSeek\\query\\2026.05.09-06.05.17.txt";
                string outputFilePath = "D:\\ALLZSV\\myHomeland\\floraZSV\\DeepSeek\\result\\result.txt";

                // 1. Проверяем существование файла с запросом
                if (!File.Exists(inputFilePath))
                {
                    MessageBox.Show($"Файл запроса не найден: {inputFilePath}",
                                  "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 2. Читаем запрос из файла
                string userPrompt = File.ReadAllText(inputFilePath, Encoding.UTF8);

                if (string.IsNullOrWhiteSpace(userPrompt))
                {
                    MessageBox.Show("Файл запроса пуст!", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Показываем пользователю, что процесс начался
                MessageBox.Show($"Запрос прочитан ({userPrompt.Length} символов).\nОтправляю в Google AI...",
                               "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 3. Получаем API ключ (лучше из переменных окружения или конфигурации)
                string? apiKey = GetApiKey();

                if (string.IsNullOrEmpty(apiKey))
                {
                    MessageBox.Show("API ключ Google не найден!\n" +
                                  "Установите переменную окружения GOOGLE_API_KEY или укажите ключ в коде.",
                                  "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 4. Отправляем запрос в Google Gemini AI
                string aiResponse = await SendToGoogleGeminiAsync(userPrompt, apiKey);

                // 5. Записываем результат в файл
                File.WriteAllText(outputFilePath, aiResponse ?? "Ответ не получен", Encoding.UTF8);

                // 6. Уведомляем пользователя об успехе
                MessageBox.Show($"Результат сохранен в файл: {outputFilePath}\n" +
                               $"Длина ответа: {(aiResponse?.Length ?? 0)} символов",
                               "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\n{ex.StackTrace}",
                               "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Включаем пункт меню обратно
                if (menuItem != null) menuItem.Enabled = true;
            }
        }

        // Метод для получения API ключа
        private string? GetApiKey()
        {
            // Приоритет 1: переменная окружения
            string? apiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY");

            if (!string.IsNullOrEmpty(apiKey))
                return apiKey;

            // Приоритет 2: из файла конфигурации (если есть)
            try
            {
                if (File.Exists("appsettings.txt"))
                {
                    string config = File.ReadAllText("appsettings.txt");
                    var lines = config.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("GOOGLE_API_KEY="))
                        {
                            return line.Substring("GOOGLE_API_KEY=".Length);
                        }
                    }
                }
            }
            catch { /* Игнорируем ошибки чтения конфига */ }

            // Приоритет 3: вернуть строку для ручной вставки (ЗАМЕНИТЕ НА ВАШ КЛЮЧ!)
            // return "ВАШ_API_КЛЮЧ_ЗДЕСЬ";

            return null;
        }

        // Метод для отправки запроса в Google Gemini
        private async Task<string> SendToGoogleGeminiAsync(string prompt, string apiKey)
        {
            // Используем HttpClient напрямую (без сторонних библиотек)
            using var httpClient = new HttpClient();

            // Формируем запрос к Gemini API
            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        },
                generationConfig = new
                {
                    temperature = 0.7,
                    maxOutputTokens = 2048
                }
            };

            string jsonBody = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // URL для Gemini API (используем модель flash для быстроты)
            string url = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.0-flash:generateContent?key={apiKey}";

            // Отправляем запрос
            var response = await httpClient.PostAsync(url, content);
            string responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API ошибка ({response.StatusCode}): {responseBody}");
            }

            // Парсим ответ
            using var doc = System.Text.Json.JsonDocument.Parse(responseBody);
            var root = doc.RootElement;

            // Извлекаем текст ответа
            if (root.TryGetProperty("candidates", out var candidates) &&
                candidates.GetArrayLength() > 0 &&
                candidates[0].TryGetProperty("content", out var content07) &&
                content07.TryGetProperty("parts", out var parts) &&
                parts.GetArrayLength() > 0 &&
                parts[0].TryGetProperty("text", out var text))
            {
                return text.GetString();
            }

            return null;
        }
    }
}
