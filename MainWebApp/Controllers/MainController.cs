using MainWebApp.Repositoies;
using System;
using System.Web.Mvc;

namespace MainWebApp.Controllers
{
    public class MainController : Controller
    {
        private const string KEY = "00000000-0000-0000-0000-000000000000";

        private readonly ReleSettingsMySqlRepository _releSettingsRepository;

        public MainController()
        {
            _releSettingsRepository = new ReleSettingsMySqlRepository();
        }

        [HttpGet]
        public ActionResult Index(string key)
        {
            if (key == KEY)
            {
                var errMessage = string.Empty;
                var result = _releSettingsRepository.GetAll(out errMessage);
                if (string.IsNullOrEmpty(errMessage))
                {
                    return View(result);
                }
                else
                {
                    return View("Error", model: errMessage);
                }
            }
            else
            {
                return View("Error", model: "Введите корректный ключ в строку браузера для доступа к настройкам USB RELE портов");
            }
        }

        [HttpPost]
        public bool Update(DTO.UsbRelePortSettingsDto usbReleSettings)
        {
            var errMessage = string.Empty;
            _releSettingsRepository.Update(usbReleSettings, out errMessage);
            return true;
        }
    }
}