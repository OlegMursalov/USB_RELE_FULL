using System;

namespace MainWebApp.DTO
{
    /// <summary>
    /// Настройка открытия/закрытия порта USB RELE
    /// </summary>
    public class UsbRelePortSettingsDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Дата установки настроек
        /// </summary>
        public DateTime InstallDate { get; set; }

        /// <summary>
        /// Через сколько дней повторять открытие порта
        /// </summary>
        public int RecurrencyDay { get; set; }

        /// <summary>
        /// Кол-во секунд, когда порт открыт
        /// </summary>
        public int OpenSecAmount { get; set; }

        /// <summary>
        /// Массив времен, по наступлению которых открывается порт
        /// </summary>
        public DateTime[] Times { get; set; }
    }
}