using Microsoft.AspNetCore.Mvc;

namespace MVC_PublicModule.ViewComponents
{
    public class RowNumber : ViewComponent
    {
        public IViewComponentResult Invoke(int number)
        {
            return View(number);
        }
    }
}
