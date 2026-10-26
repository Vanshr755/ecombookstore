using Microsoft.AspNetCore.Mvc;
using NationalParkWebApp_1033.Models;
using NationalParkWebApp_1033.Models.ViewModels;
using NationalParkWebApp_1033.Repository.IRepository;
using System.Diagnostics;

namespace NationalParkWebApp_1033.Controllers
{
    public class HomeController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly ITrailRepository _trailRepository;
        public HomeController(INationalParkRepository nationalParkRepository,ITrailRepository trailRepository)
        {
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
        }
        public async Task<IActionResult> Index()
        {
            IndexVM indexVM = new IndexVM()
            {
                NationalParkList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath),
                TrailList = await _trailRepository.GetAllAsync(SD.TrailAPIPath)
            };
            return View(indexVM);
        }
        //public async Task<IActionResult> Index()
        //{
        //    IndexVM indexVM = new IndexVM()
        //    {
        //        NationalParkList = await _nationalParkRepository.GetAllAsync(SD.NationalParkAPIPath) ?? new List<NationalPark>(),
        //        TrailList = await _trailRepository.GetAllAsync(SD.TrailAPIPath) ?? new List<Trail>()
        //    };
        //    return View(indexVM);
        //}
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
