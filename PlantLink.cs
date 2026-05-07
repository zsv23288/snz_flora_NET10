using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Menu_14
{

    public class PlantLink
    {
        public string Head { get; set; }
        public string Link { get; set; }
    }

    public class PlantDataService
    {
        private string connectionString;

        public PlantDataService()
        {
            connectionString = ConfigurationManager.AppSettings["floraBD"];
            // connectionString = $"Server={server};Database={database};Uid={userId};Pwd={password};";
        }

        /// <summary>
        /// Получает массив объектов PlantLink из таблицы plant_links
        /// </summary>
        /// <param name="nameLat">Название на латыни для фильтрации</param>
        /// <returns>Массив объектов с полями Head и Link</returns>
        public PlantLink[] GetPlantLinksArray(string nameLat)
        {
            List<PlantLink> linksList = new List<PlantLink>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT head, link FROM plant_links WHERE name_lat = @nameLat";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nameLat", nameLat);

                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PlantLink plantLink = new PlantLink
                                {
                                    Head = reader["head"] != DBNull.Value ? reader["head"].ToString() : string.Empty,
                                    Link = reader["link"] != DBNull.Value ? reader["link"].ToString() : string.Empty
                                };
                                linksList.Add(plantLink);
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Ошибка базы данных: {ex.Message}");
                        throw;
                    }
                }
            }

            return linksList.ToArray();
        }

        /// <summary>
        /// Получает все уникальные значения name_lat из таблицы plant_links
        /// </summary>
        /// <returns>Массив уникальных латинских названий</returns>
        public string[] GetAllNameLatValues()
        {
            List<string> nameLatList = new List<string>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT name_lat FROM plant_links ORDER BY name_lat";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nameLat = reader["name_lat"] != DBNull.Value ? reader["name_lat"].ToString() : string.Empty;
                                if (!string.IsNullOrEmpty(nameLat))
                                {
                                    nameLatList.Add(nameLat);
                                }
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Ошибка базы данных: {ex.Message}");
                        throw;
                    }
                }
            }

            return nameLatList.ToArray();
        }

        /// <summary>
        /// Получает словарь, где ключ - name_lat, значение - массив ссылок для этого названия
        /// </summary>
        /// <returns>Словарь с латинскими названиями и соответствующими массивами ссылок</returns>
        public Dictionary<string, PlantLink[]> GetAllPlantsLinks()
        {
            Dictionary<string, List<PlantLink>> linksDictionary = new Dictionary<string, List<PlantLink>>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT name_lat, head, link FROM plant_links ORDER BY name_lat";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nameLat = reader["name_lat"] != DBNull.Value ? reader["name_lat"].ToString() : string.Empty;

                                if (!string.IsNullOrEmpty(nameLat))
                                {
                                    if (!linksDictionary.ContainsKey(nameLat))
                                    {
                                        linksDictionary[nameLat] = new List<PlantLink>();
                                    }

                                    PlantLink plantLink = new PlantLink
                                    {
                                        Head = reader["head"] != DBNull.Value ? reader["head"].ToString() : string.Empty,
                                        Link = reader["link"] != DBNull.Value ? reader["link"].ToString() : string.Empty
                                    };

                                    linksDictionary[nameLat].Add(plantLink);
                                }
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Ошибка базы данных: {ex.Message}");
                        throw;
                    }
                }
            }

            // Конвертируем List<PlantLink> в PlantLink[]
            Dictionary<string, PlantLink[]> result = new Dictionary<string, PlantLink[]>();
            foreach (var kvp in linksDictionary)
            {
                result[kvp.Key] = kvp.Value.ToArray();
            }

            return result;
        }
    }

}


