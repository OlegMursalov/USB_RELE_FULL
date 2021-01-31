using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsbReleMainApp
{
    public class Program
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            _logger.Info("Запуск консольки");
        }
    }
}