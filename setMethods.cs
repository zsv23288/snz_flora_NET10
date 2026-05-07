using Google.Protobuf;
// horn, horn ...
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
// using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace Menu_14
{

    /// <summary>
    /// Основная линия Добавки новых растений в БД
    /// 1. Перенос из смартфона на локальный диск ПК
    /// 2. Выбор файла архива и распаковка
    /// 3. Проверка растения наличия в БД
    /// 4. Добавка к приложению запись растения и фотографий
    /// 5. Если растение уже есть в БД, то можно добавить новые фотографии
    /// </summary>
    static class setMethods
    {
        public static void ProtocolT(string name, string description, string comment) // фиксация события в коде
        {
            string initialBD = ConfigurationManager.AppSettings["floraBD"];
            string connectionString = "server=localhost;database=" + initialBD + ";uid=root;password=root";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO protocol (date, name, description, comment) VALUES (@date, @name, @description, @comment)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@date", DateTime.Now);         // Текущая дата и время
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@comment", comment);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"Запись добавлена. Затронуто строк: {rowsAffected}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
            // Ваша реализация метода
            Console.WriteLine("ProtocolT called");
        }

        public static void cleanTable(string nameT) // очистить указанную таблицу MySQL
        {
            // string initialBD = nameT;
            string connectionString = "server=localhost;database=snz_flora;uid=root;password=root";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // TRUNCATE удаляет данные и сбрасывает автоинкремент [3, 13]
                    string sql = $"TRUNCATE TABLE {nameT}";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Таблица успешно очищена.");
                        MessageBox.Show("Таблица успешно очищена: " + nameT);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }
        public static void choiceFile(string folderPath) // поиск первого архива
        {
            folderPath = ConfigurationManager.AppSettings["catExport"];
            //    string folderPath = @"C:\Your\Folder\Path"; // Путь к каталогу
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = folderPath;
                openFileDialog.Filter = "Выбор архива (*.zip)|*.zip|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    //        MessageBox.Show("Good...", "Yes !");
                    ProcessFile(selectedFilePath);
                }
                else
                {
                    MessageBox.Show("что-то не так...", "не нашли кондидата");
                }
            }
        }
        public static void ProcessFile(string fileZIP) // архив выбран, распаковка
        {
            try
            {
                string zipPath = ConfigurationManager.AppSettings["floraBD"] + fileZIP;
                string extractPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExtractedData");

                // Проверяем существование ZIP-файла
                if (!File.Exists(zipPath))
                {
                    Console.WriteLine($"Файл не найден: {zipPath}");
                    return;
                }

                // Создаем директорию для распаковки
                Directory.CreateDirectory(extractPath);

                // Распаковываем с перезаписью существующих файлов
                ZipFile.ExtractToDirectory(zipPath, extractPath, true);

                Console.WriteLine($"✅ Файл успешно распакован в: {extractPath}");

                // Выводим список распакованных файлов
                var files = Directory.GetFiles(extractPath, "*", SearchOption.AllDirectories);
                Console.WriteLine($"\nРаспаковано файлов: {files.Length}");
                foreach (var file in files)
                {
                    Console.WriteLine($"  - {Path.GetFileName(file)}");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Нет доступа: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при распаковке: {ex.Message}");
            }

        }
        static void CreatingArrayFromCSV(string WorkDirectoryFromCSV) // построить, проверить массив из CSV файла
        {
            // Укажите путь к вашему каталогу
            string directoryPath = WorkDirectoryFromCSV;

            // Поиск всех CSV файлов
            string[] csvFiles = Directory.GetFiles(directoryPath, "*.csv");

            if (csvFiles.Length == 0)
            {
                Console.WriteLine("CSV файлы не найдены.");
                return;
            }

            string filePath = csvFiles[0];
            Console.WriteLine($"Обработка файла: {filePath}");

            try
            {
                // Чтение всех строк
                string[] lines = File.ReadAllLines(filePath);
                Console.WriteLine($"Всего строк: {lines.Length}");

                if (lines.Length == 0)
                {
                    Console.WriteLine("Файл пуст.");
                    return;
                }

                // Определяем количество столбцов по первой строке
                string[] firstLine = lines[0].Split(',');
                int columnsCount = firstLine.Length;
                Console.WriteLine($"Количество столбцов: {columnsCount}");

                // Создаем массив
                string[,] data = new string[lines.Length, columnsCount];

                // Заполняем массив
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] columns = lines[i].Split(',');

                    for (int j = 0; j < columnsCount; j++)
                    {
                        // Если в строке меньше столбцов, заполняем пустой строкой
                        data[i, j] = j < columns.Length ? columns[j] : "";
                    }
                }

                // Вывод результата
                Console.WriteLine($"\nСоздан массив: {data.GetLength(0)} строк × {data.GetLength(1)} столбцов");
                // попровляем поля из смартфона
                for (int i = 1; i < lines.Length; i++)
                {
                    if ((data[i, 2].Length > 0) & (data[i, 3].Length == 2))
                    {
                        data[i, 3] = data[i, 2];
                        setMethods.ProtocolT("добавляем ", data[i, 2], "оба c научным именем");
                    }
                    if ((data[i, 2].Length == 2) & (data[i, 3].Length > 0))
                    {
                        data[i, 2] = data[i, 2].Substring(0, 1) + "растение не определено! " + data[i, 2].Substring(1, 1);
                        setMethods.ProtocolT("генерируем засаду ", data[i, 1], "оч. плохо...");
                    }
                }
                for (int i = 0; i < lines.Length; i++)
                {
                    for (int j = 0; j < columnsCount; j++)
                    {
                        data[i, j] = data[i, j].TrimEnd('"');
                        data[i, j] = data[i, j].TrimStart('"');
                    }
                }
                // сортируем массив по дате
                string format = "yyyy-MM-ddTHH_mm_ssZ";
                string temp;
                int sumRow = data.GetLength(0); // кол-во строк
                int sumColumn = data.GetLength(1); // кол-во столбцов
                for (int iStr = 1; iStr < sumColumn - 1; iStr++)
                {
                    for (int iStr2 = 1; iStr2 < sumColumn - 1; iStr2++)
                    {
                        DateTime result = DateTime.ParseExact(data[iStr2, 1], format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                        DateTime result2 = DateTime.ParseExact(data[iStr2 + 1, 1], format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                        if (result > result2)
                        {
                            for (int j = 0; j < columnsCount; j++)
                            {
                                temp = data[iStr, j];
                                data[iStr, j] = data[iStr2, j];
                                data[iStr2, j] = temp;
                            }
                        }
                    }
                }
                Console.WriteLine($"\n  Создан массив: {data.GetLength(0)} строк × {data.GetLength(1)} столбцов");
                addRecordBDplants(data);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        static void addRecordBDplants(string[,] clearArray) // операции с БД MySQL plants
        {
            int rowCount = clearArray.GetLength(0); // количество строк
            for (int i_Arx = 1; i_Arx < rowCount; i_Arx++)
            {
                string name_latArx = clearArray[i_Arx, 2];
                Console.WriteLine(clearArray[i_Arx, 2]);
                var conporisionName = setMethods.findName_latPlants(name_latArx); // проверка наличие названия name_lat в базе MySQL
                if (conporisionName.Count > 0) //  есть совпадение
                {
                    setMethods.weAddFotos(clearArray[i_Arx, 1], clearArray[i_Arx, 2]);
                }
                else // записыаем новую строку
                {
                    setMethods.InsertRecordTableBD(clearArray, i_Arx);
                    setMethods.subfolderAndFiles(clearArray[i_Arx, 1], clearArray[i_Arx, 2]);
                }
            }
        }
        public static void InsertRecordTableBD(string[,] arrayXX, int nr) // запись новой строки в БД 
        {
            string connectionString = ConfigurationManager.AppSettings["floraBD"];
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO plants (name_lat, name_ru, time_first, time_last, score, latitude, longitude, accuracy, notes, tags, preducations, questionnaire) " +
                    "VALUES (@name_lat, @name_ru, @time_first, @time_last, @score, @latitude, @longitude, @accuracy, @notes, @tags, @preducations, @questionnaire)";

                MySqlCommand command = new MySqlCommand(query, connection);
                string name_lat = arrayXX[nr, 2];
                command.Parameters.AddWithValue("@name_lat", name_lat);              //  научное название
                string name_ru = arrayXX[nr, 3];
                command.Parameters.AddWithValue("@name_ru", name_ru);                //  известное русское название
                string input = arrayXX[nr, 1];
                string normalized = input.Replace('_', ':');
                DateTime dateTime = DateTime.ParseExact(
                    normalized,
                    "yyyy-MM-ddTHH:mm:ssZ",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal
                );
                command.Parameters.AddWithValue("@time_first", dateTime);           // первоя фиксация ростерия по времени
                command.Parameters.AddWithValue("@time_last", dateTime);            // последняя фиксация ростерия по времени
                string scoreStr = arrayXX[nr, 4];
                scoreStr = scoreStr.Replace('.', ',');
                decimal score = Convert.ToDecimal(scoreStr);
                command.Parameters.AddWithValue("@score", score);                   // вероятность (всегда есть в экспорте)
                string latiStr = arrayXX[nr, 5];
                if (latiStr.Length > 0)
                {
                    latiStr = latiStr.Replace('.', ',');
                    decimal latitude = Convert.ToDecimal(latiStr);
                    command.Parameters.AddWithValue("@latitude", latitude);         // широта
                }
                else
                {
                    command.Parameters.AddWithValue("@latitude", 0.0);
                }
                string longiStr = arrayXX[nr, 6];
                if (longiStr.Length > 0)
                {
                    longiStr = longiStr.Replace(".", ",");
                    decimal longitude = Convert.ToDecimal(longiStr);
                    command.Parameters.AddWithValue("@longitude", longitude);
                }
                else
                {
                    command.Parameters.AddWithValue("@longitude", 0.0);         //   долгота
                }
                string altiStr = arrayXX[nr, 7];
                if (altiStr.Length > 0)
                {
                    altiStr = altiStr.Replace(".", ",");
                    decimal altitude = Convert.ToDecimal(altiStr);
                    command.Parameters.AddWithValue("@altitude", altitude);         //   высота
                }
                else
                {
                    command.Parameters.AddWithValue("@altitude", 0.0);
                }
                string accurStr = arrayXX[nr, 8];
                if (accurStr.Length > 0)
                {
                    accurStr = accurStr.Replace(".", ",");
                    decimal accur = Convert.ToDecimal(accurStr);
                    command.Parameters.AddWithValue("@accuracy", accur);             // точность
                }
                else
                {
                    command.Parameters.AddWithValue("@accuracy", 0.0);
                }
                command.Parameters.AddWithValue("@notes", arrayXX[nr, 9]);            // заметки
                command.Parameters.AddWithValue("@tags", arrayXX[nr, 10]);           // ярлыки
                command.Parameters.AddWithValue("@preducations", arrayXX[nr, 11]);   // обявления
                command.Parameters.AddWithValue("@questionnaire", arrayXX[nr, 12]);  // анкета
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"Запись добавлена. Затронуто строк: {rowsAffected}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
            // Ваша реализация метода
            Console.WriteLine("Record in BD plants, Ok!");
        }

        public static List<(int id, string name_lat, DateTime time_last)> findName_latPlants(string searchName) // проверка наличие названия name_lat в базе MySQL
        {
            var results = new List<(int id, string name_lat, DateTime time_last)>();

            // Строка подключения к базе данных (замените на свои параметры)
            string connectionString = "Server=localhost;Database=snz_flora;User ID=root;Password=root;";

            // SQL запрос с параметром для защиты от SQL-инъекций
            string query = "SELECT id, name_lat, time_last FROM plants WHERE name_lat LIKE @searchName";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Добавляем параметр со знаком % для поиска частичного совпадения
                        command.Parameters.AddWithValue("@searchName", "%" + searchName + "%");

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                string name_lat = reader.GetString("name_lat");
                                DateTime time_last = reader.GetDateTime("time_last");

                                results.Add((id, name_lat, time_last));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске в базе данных: {ex.Message}");
                // Здесь можно добавить логирование ошибки
            }

            return results;
        }

        public static void subfolderAndFiles(string subFolderZip, string destiSubFoto) // создаём подпапки и добавляем фотографиями
        {
            string parentFolder = ConfigurationManager.AppSettings["catSubCutFotos"];
            string fullPath = Path.Combine(parentFolder, destiSubFoto);  // куда будем копировать фото
            string sourceDir = ConfigurationManager.AppSettings["catUnZip"];
            sourceDir = sourceDir + subFolderZip + "\\";

            if (Directory.Exists(fullPath))
            {
                Console.WriteLine("Подкаталог существует - что делать");
            }
            else
            {
                Console.WriteLine("Будем создавать подкаталог!");
                string fullPathCreate = Path.Combine(parentFolder, destiSubFoto);
                Directory.CreateDirectory(fullPathCreate);
                parentFolder = parentFolder + destiSubFoto + "\\";           // путь теперь нацелен в подкаталог
                // Получаем все файлы из исходного каталога
                string sourceDirZip = ConfigurationManager.AppSettings["catSubCutFotos"] + destiSubFoto; ;
                string[] files = Directory.GetFiles(sourceDir);
                setMethods.ProtocolT(parentFolder, destiSubFoto, "");
                // Копируем каждый файл
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(parentFolder, fileName);

                    // Копируем файл (третий параметр - разрешить перезапись)
                    File.Copy(file, destFile, true);
                    setMethods.ProtocolT(sourceDir, destFile, "");
                }
            }
        }
        public static void weAddFotos(string subFolderZip, string destiSubFoto) // в существующие подпапки добавляем фото, меняем поле time_last in MySQL
        {
            string parentFolder = ConfigurationManager.AppSettings["catSubCutFotos"];
            string fullPath = Path.Combine(parentFolder, destiSubFoto);  // куда будем копировать фото
            string sourceDir = ConfigurationManager.AppSettings["catUnZip"];
            sourceDir = sourceDir + subFolderZip + "\\";

            string normalized = subFolderZip.Replace('_', ':');
            DateTime dateTimeZip = DateTime.ParseExact(
                normalized, "yyyy-MM-ddTHH:mm:ssZ",
                CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal
            );
            string connectionString = "server=localhost;database=snz_flora;uid=root;pwd=root;";
            string searchName = destiSubFoto;
            DateTime? timeLast = null; // Объявляем nullable переменную
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT time_last FROM plants WHERE name_lat = @nameLat LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nameLat", searchName);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            timeLast = Convert.ToDateTime(result);
                            Console.WriteLine($"Найдено значение time_last: {timeLast}");
                        }
                        else
                        {
                            Console.WriteLine("Совпадение не найдено");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            if (dateTimeZip > timeLast) // в экспорте ZIP сортировка не та ...
            {
                string sourceDirZip = ConfigurationManager.AppSettings["catSubCutFotos"] + destiSubFoto; ;
                string[] files = Directory.GetFiles(sourceDir);

                // Копируем каждый файл
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = sourceDirZip + "\\" + fileName;

                    // Копируем файл (третий параметр - разрешить перезапись)
                    File.Copy(file, destFile, true);
                    setMethods.ProtocolT("add foto", file, destFile);
                }
                // изменить в поле time_last таблицs plants 
                setMethods.UpdateTimeLastByName(destiSubFoto, dateTimeZip);
                setMethods.ProtocolT("время другое:", subFolderZip, "");
            }
            else
            {
                MessageBox.Show("Probably...");
            }
        }


        public static void UpdateTimeLastByName(string searchName, DateTime newTimeValue)  // замена в поле time_last
        {
            string connectionString = "server=localhost;database=snz_flora;uid=root;pwd=root;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE plants SET time_last = @newTime WHERE name_lat = @searchName";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Добавляем параметры для защиты от SQL-инъекций
                        command.Parameters.AddWithValue("@searchName", searchName);
                        command.Parameters.AddWithValue("@newTime", newTimeValue);

                        // Выполняем запрос и получаем количество обновленных строк
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Успешно обновлено! Найдено и обновлено записей: {rowsAffected}");
                            Console.WriteLine($"Для растения '{searchName}' установлено time_last = {newTimeValue}");
                        }
                        else
                        {
                            Console.WriteLine($"Растение с именем '{searchName}' не найдено в базе данных");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Ошибка MySQL: {ex.Message}");
                Console.WriteLine($"Код ошибки: {ex.Number}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Общая ошибка: {ex.Message}");
            }
        }
        public static void ExportDatabase()  // выгрузка БД из MySQL
        {
            // Строка подключения указывает только на сервер и БД
     //       string connectionString = "server=localhost;user=root;pwd=root;database=snz_flora;charset=utf8;convertzerodatetime=true;";
            //string sevenZipPath = ConfigurationManager.AppSettings["catExport"];
            //string backupPath = sevenZipPath + "db_backup.sql";

            try
            {
                //using (MySqlConnection conn = new MySqlConnection(connectionString))
                //using (MySqlCommand cmd = conn.CreateCommand())
                //using (MySqlBackup backup = new MySqlBackup(cmd))
                //{
                //    conn.Open();

                //    // --- ГЛАВНОЕ: Здесь вы указываете список нужных таблиц ---
                //    // Раскомментируйте строки ниже и укажите имена ваших таблиц
                //    backup.ExportInfo.TablesToBeExportedList.Clear();
                //    backup.ExportInfo.TablesToBeExportedList.Add("plants");       // Пример: таблица "users"
                //    backup.ExportInfo.TablesToBeExportedList.Add("plant_links");    // Пример: таблица "products"
                //                                                                 // -----------------------------------------------------
                //    backup.ExportToFile(backupPath);
                //    //   Console.WriteLine($"✅ Бэкап успешно создан: {backupPath}");
                //    MessageBox.Show($"✅ Бэкап успешно создан: {backupPath}");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
            }
        }
    }
}