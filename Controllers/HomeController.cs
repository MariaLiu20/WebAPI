using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Test()
        {
            return Json(new { message = "Hello World!", status = "MVC project is running successfully!" });
        }

        public IActionResult GetDepartmentTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("DepID");
            dt.Columns.Add("DepName");
            dt.Rows.Add(1, "IT");
            dt.Rows.Add(2, "Support");

            var serializableRows = dt.AsEnumerable().Select(row =>
            dt.Columns.Cast<DataColumn>().ToDictionary(
            col => col.ColumnName,
            col => row[col] == DBNull.Value ? null : row[col]
    )
);

            return Ok(serializableRows);
        }
    }
}
