﻿using MainWebApp.DTO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace MainWebApp.Repositoies
{
    public class ReleSettingsMySqlRepository
    {
        private readonly string _connectionString;

        public ReleSettingsMySqlRepository()
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
                                    Id = reader.GetInt32(0),
                                    OpenSecAmount = !reader.IsDBNull(1) ? reader.GetInt32(1) : 0,
                                    InstallDate = !reader.IsDBNull(2) ? reader.GetDateTime(2) : DateTime.Now,
                                    RecurrencyDay = !reader.IsDBNull(3) ? reader.GetInt32(3) : 0,
                                    Times = (!reader.IsDBNull(4) && !string.IsNullOrWhiteSpace(reader.GetString(4))) 
                                        ? JsonConvert.DeserializeObject<DateTime[]>(reader.GetString(4)) : new DateTime[0]
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

        public void Update(UsbRelePortSettingsDto usbRelePortSettingsDto, out string errMessage)
        {
            errMessage = string.Empty;

            try
            {
                using (var myConnection = new MySqlConnection(_connectionString))
                {
                    myConnection.Open();
                    var sb = new StringBuilder();
                    sb.AppendLine("UPDATE `usbreleportsettings`");
                    var timesJson = usbRelePortSettingsDto.Times != null && usbRelePortSettingsDto.Times.Length > 0
                        ? JsonConvert.SerializeObject(usbRelePortSettingsDto.Times) : null;
                    var installDate = usbRelePortSettingsDto.InstallDate;
                    var recurrencyDay = usbRelePortSettingsDto.RecurrencyDay;
                    sb.AppendLine($"SET `OpenSecAmount`={usbRelePortSettingsDto.OpenSecAmount},`Times`='{timesJson}',`RecurrencyDay`={recurrencyDay},`InstallDate`='{usbRelePortSettingsDto.InstallDate.ToString("yyyy-MM-dd 00:00:00")}'");
                    sb.AppendLine($"WHERE `Id` = {usbRelePortSettingsDto.Id}");
                    var cmdTxt = GetUTF8String(sb.ToString());
                    var myCommand = new MySqlCommand(cmdTxt, myConnection);
                    myCommand.ExecuteNonQuery();
                    myConnection.Close();
                }
            }
            catch (Exception ex)
            {
                errMessage = GetInfoByException(ex);
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