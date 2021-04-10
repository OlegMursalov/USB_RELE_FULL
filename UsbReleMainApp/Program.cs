using NLog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbReleMainApp.Structure;

namespace UsbReleMainApp
{
    public class Program
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void Main()
        {
            _logger.Info("Запуск консольки");

            SerialPort port = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
            try
            {
                port.Open();
                port.Write("RELx.ON"); /* You can pass any command as per your documentation in port.Write */
                System.Threading.Thread.Sleep(3000);
                port.Write("RELx.OFF");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                if (ex is System.IO.IOException)
                {
                    Console.WriteLine("Port Exception: " + ex.ToString());
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("General Exception: " + ex.ToString());
                    Console.ReadLine();
                }
            }
        }
    }
}