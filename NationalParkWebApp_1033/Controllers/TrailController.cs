using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NationalParkWebApp_1033.Models.ViewModels;
using NationalParkWebApp_1033.Repository.IRepository;

namespace NationalParkWebApp_1033.Controllers
{
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;
        public TrailController(ITrailRepository trailRepository,INationalParkRepository nationalParkRepository)
        {
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalparkList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
            TrailVM trailVM = new TrailVM()
            {
                NationalParkList = nationalparkList.Select(np => new SelectListItem()
                {
                    Text = np.Name,
                    Value = np.Id.ToString()
                }),
                Trail = new Models.Trail()
            };

            if (id == null) return View(trailVM);
            trailVM.Trail = await _trailRepository.GetAsync(SD.TrailAPIPath, id.Value);
            if (trailVM.Trail == null) return NotFound();
            return View(trailVM);
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(TrailVM trailVM)
        {
            if (trailVM == null) return BadRequest();
            if (!ModelState.IsValid) return View(trailVM);
            if (trailVM.Trail.Id == 0)
                await _trailRepository.CreateAsync(SD.TrailAPIPath, trailVM.Trail);
            else
                await _trailRepository.UpdateAsync(SD.TrailAPIPath, trailVM.Trail);
            return RedirectToAction("Index");
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(SD.TrailAPIPath) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (!status)
                return Json(new { success = true, message = "Delete Successful" });
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    } 
    #endregion
}

