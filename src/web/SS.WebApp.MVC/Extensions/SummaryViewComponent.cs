using System;
using Microsoft.AspNetCore.Mvc;

namespace SS.WebApp.MVC.Extensions
{
	public class SummaryViewComponent : ViewComponent
	{
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}

