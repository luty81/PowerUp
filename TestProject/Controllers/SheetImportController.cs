using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TestProject.Controllers
{
    public class SheetImportController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            using(var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var filepath = Path.Combine("c:\\temp", file.FileName);
                var fileContent = await streamReader.ReadToEndAsync();
                System.IO.File.WriteAllText(filepath, fileContent);
            }

            return RedirectToAction("Index");
        }
    }
}
