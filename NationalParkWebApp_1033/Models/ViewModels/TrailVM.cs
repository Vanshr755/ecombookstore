using Microsoft.AspNetCore.Mvc.Rendering;

namespace NationalParkWebApp_1033.Models.ViewModels
{
    public class TrailVM
    {
        public IEnumerable<SelectListItem> NationalParkList { get; set; }
        public Trail Trail { get; set; }
    }
}
