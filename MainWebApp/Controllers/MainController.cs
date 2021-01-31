using MainWebApp.Repositoies;
using System;
using System.Web.Mvc;

namespace MainWebApp.Controllers
{
    public class MainController : Controller
    {
        private readonly ReleSettingsRepository _releSettingsRepository;

        public MainController()
        {
            _releSettingsRepository = new ReleSettingsRepository();
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
            _releSettingsRepository.Update(usbReleSettings);
            return true;
        }
    }
}