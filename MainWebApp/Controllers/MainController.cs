using MainWebApp.Repositoies;
using System;
using System.Web.Mvc;

namespace MainWebApp.Controllers
{
    public class MainController : Controller
    {
        private readonly ReleSettingsMySqlRepository _releSettingsRepository;

        public MainController()
        {
            _releSettingsRepository = new ReleSettingsMySqlRepository();
        }

        public ActionResult Index()
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

        [HttpPost]
        public bool Update(DTO.UsbRelePortSettingsDto usbReleSettings)
        {
            var errMessage = string.Empty;
            _releSettingsRepository.Update(usbReleSettings, out errMessage);
            return true;
        }
    }
}