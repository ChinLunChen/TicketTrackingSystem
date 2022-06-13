using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TicketTrackingSystem.Models;

namespace TicketTrackingSystem.Controllers
{
    public class HomeController : Controller
    {

        NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=postgres;UserId=postgres;Password=729650");
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly IMemoryCache memoryCache;

        public HomeController(IMemoryCache _memoryCache)
        {
            this.memoryCache = _memoryCache;
        }

        public IActionResult Index()
        {
            UserPermissionViewModel Model = new UserPermissionViewModel();
            List<BugListViewModel> BugModelList = new List<BugListViewModel>();
            try
            {
                //0.controller 1.action 2.user
                string sUser = Request.QueryString.ToString().Split("=")[1];
                Response.Cookies.Append("User", sUser);
                Model.sUser = sUser;
                switch (sUser)
                {
                    case "RD":
                        Model.sPermissionLevel = "0";
                        break;
                    case "QA":
                        Model.sPermissionLevel = "1";
                        break;
                    case "PM":
                        Model.sPermissionLevel = "2";
                        break;
                    case "Admin":
                        Model.sPermissionLevel = "3";
                        break;
                }

                //預設記錄存在一小時
                var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(3600))
            .SetSize(1024);

                memoryCache.Set(MemoryCacheKey.sUserPermission, Model, cacheEntryOptions);
                memoryCache.Set(MemoryCacheKey.sTest, sUser, cacheEntryOptions);
                //var obj = memoryCache.Get(MemoryCacheKey.sTest);
                ViewData["User"] = memoryCache.Get(MemoryCacheKey.sTest);
                conn.Open();

                NpgsqlCommand cmd =
                    new NpgsqlCommand("select * from BugList", conn);
                //NpgsqlDataReader reader = command.ExecuteReader();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataSet _ds = new DataSet();
                DataTable _dt = new DataTable();

                da.Fill(_ds);
                _dt = _ds.Tables[0];

                for (int i = 0; i < _dt.Rows.Count; i++)
                {
                    BugListViewModel BugListModel = new BugListViewModel();
                    BugListModel.ID = _dt.Rows[i]["ID"].ToString();
                    BugListModel.Summary = _dt.Rows[i]["Summary"].ToString();
                    BugListModel.Description = _dt.Rows[i]["Description"].ToString();
                    BugListModel.Type = _dt.Rows[i]["Type"].ToString();
                    BugListModel.Priority = _dt.Rows[i]["Priority"].ToString();
                    BugListModel.Status = _dt.Rows[i]["Status"].ToString();
                    BugListModel.CreateUser = _dt.Rows[i]["CreateUser"].ToString();
                    BugListModel.CreateDate = _dt.Rows[i]["CreateDate"].ToString();
                    BugListModel.EndUser = _dt.Rows[i]["EndUser"].ToString();
                    BugListModel.EndDate = _dt.Rows[i]["EndDate"].ToString();
                    BugModelList.Add(BugListModel);
                }

            }
            catch (Exception ex)
            {
            }
            return View(BugModelList);
        }

        public IActionResult Privacy()
        {

            return View();
        }

        public IActionResult Permission()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string sUser)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static class MemoryCacheKey
        {
            public static string sUserPermission = "UserPermission";
            public static string sTest = "Test";
        }
    }
}
