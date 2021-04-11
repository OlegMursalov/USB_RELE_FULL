using MainWebApp.DTO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace MainWebApp.Repositoies
{
    public class ReleSettingsRepository
    {
        private readonly string _connectionString;

        public ReleSettingsRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MySqlConnectionStr"].ConnectionString;
        }

        public UsbRelePortSettingsDto[] GetAll(out string errMessage)
        {
            errMessage = string.Empty;
            var result = new List<UsbRelePortSettingsDto>();

            try
            {
                using (var myConnection = new MySqlConnection(_connectionString))
                {
                    myConnection.Open();
                    var sb = new StringBuilder();
                    sb.AppendLine("SELECT `Id`, `OpenSecAmount`, `InstallDate`, `RecurrencyDay`, `Times`");
                    sb.AppendLine("FROM `usbreleportsettings`");
                    var myCommand = new MySqlCommand(GetUTF8String(sb.ToString()), myConnection);
                    using (var reader = myCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result.Add(new UsbRelePortSettingsDto
                                {
                                    Id = reader.GetInt32(1),
                                    OpenSecAmount = !reader.IsDBNull(1) ? reader.GetInt32(1) : 0,
                                    InstallDate = !reader.IsDBNull(2) ? reader.GetDateTime(2) : DateTime.Now,
                                    RecurrencyDay = !reader.IsDBNull(3) ? reader.GetInt32(3) : 0,
                                    Times = !reader.IsDBNull(4) ? JsonConvert.DeserializeObject<DateTime[]>(reader.GetString(4)) : new DateTime[0]
                                });
                            }
                        }
                    }
                    myConnection.Close();
                }
            }

            catch (Exception ex)
            {
                errMessage = GetInfoByException(ex);
            }

            return result.ToArray();
        }

        public UsbRelePortSettingsDto[] GetAllFake()
        {
            var result = new List<UsbRelePortSettingsDto>();

            result.Add(new UsbRelePortSettingsDto
            {
                Id = 1,
                OpenSecAmount = 60,
                RecurrencyDay = 2,
                InstallDate = new DateTime(2021, 1, 21),
                Times = new DateTime[]
                {
                    new DateTime(2021, 1, 1),
                    new DateTime(2021, 1, 2),
                    new DateTime(2021, 1, 3),
                    new DateTime(2021, 1, 4),
                    new DateTime(2021, 1, 5),
                    new DateTime(2021, 1, 5)
                }
            });

            result.Add(new UsbRelePortSettingsDto
            {
                Id = 2,
                RecurrencyDay = 1,
                OpenSecAmount = 120,
                InstallDate = new DateTime(2021, 1, 21),
                Times = new DateTime[]
                {
                    new DateTime(2021, 2, 1),
                    new DateTime(2021, 2, 5),
                    new DateTime(2021, 2, 5)
                }
            });

            return result.ToArray();
        }

        public void Update(UsbRelePortSettingsDto usbRelePortSettingsDto)
        {
            using (var myConnection = new MySqlConnection(_connectionString))
            {
                myConnection.Open();
                var sb = new StringBuilder();
                sb.AppendLine("UPDATE `usbreleportsettings`");
                var timesJson = JsonConvert.SerializeObject(usbRelePortSettingsDto.Times);
                var installDate = usbRelePortSettingsDto.InstallDate;
                var recurrencyDay = usbRelePortSettingsDto.RecurrencyDay;
                sb.AppendLine($"SET `OpenSecAmount`={usbRelePortSettingsDto.OpenSecAmount},`Times`=`{timesJson}`,`InstallDate`=`{installDate}`,`RecurrencyDay`=`{recurrencyDay}`");
                sb.AppendLine($"WHERE `Id` = {usbRelePortSettingsDto.Id}");
                var myCommand = new MySqlCommand(GetUTF8String(sb.ToString()), myConnection);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }

        private string GetInfoByException(Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            return sb.ToString();
        }

        private string GetUTF8String(string myString)
        {
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(myString);
            return UTF8Encoding.UTF8.GetString(bytes);
        }
    }
}