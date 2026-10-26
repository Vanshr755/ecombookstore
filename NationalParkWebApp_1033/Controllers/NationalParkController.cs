using Microsoft.AspNetCore.Mvc;
using NationalParkWebApp_1033.Models;
using NationalParkWebApp_1033.Repository.IRepository;

namespace NationalParkWebApp_1033.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        public NationalParkController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark nationalPark = new NationalPark();
            if (id == null) return View(nationalPark);
            nationalPark = await _nationalParkRepository.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault());
            if (nationalPark == null) return NotFound();
            return View(nationalPark);
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if (nationalPark == null) return NotFound();
            if (!ModelState.IsValid) return View(nationalPark);
            //Image
            var files = HttpContext.Request.Form.Files;
            if (files.Count() > 0)
            {
                byte[] p1 = null;
                using (var fs = files[0].OpenReadStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);
                        p1 = ms.ToArray();
                    }
                    ;
                }
                ;
                nationalPark.Picture = p1;
            }
            else
            {
                var nationalParkInDb = await _nationalParkRepository.GetAsync
                    (SD.NationalParkAPIPath, nationalPark.Id);
                nationalPark.Picture = nationalParkInDb.Picture;
            }
            //**
            if (nationalPark.Id == 0)
                await _nationalParkRepository.CreateAsync(SD.NationalParkAPIPath, nationalPark);
            else
                await _nationalParkRepository.UpdateAsync(SD.NationalParkAPIPath, nationalPark);
            return RedirectToAction(nameof(Index));
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new
            {
                data = await _nationalParkRepository.GetAllAsync
                (SD.NationalParkAPIPath)
            });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _nationalParkRepository.DeleteAsync
                (SD.NationalParkAPIPath, id);
            if (status == false)
                return Json(new { success = false, message = "Unable to delete data!!" });
                return Json(new { success = true, message = "Data deleted successfully!!" });
        }
        #endregion

    }
}
