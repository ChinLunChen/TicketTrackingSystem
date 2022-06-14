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
                memoryCache.Set(MemoryCacheKey.sUser, sUser, cacheEntryOptions);
                ViewData["User"] = memoryCache.Get(MemoryCacheKey.sUser);

                conn.Open();
                BugModelList = GetBugLists();
                conn.Close();
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
            BugListViewModel Model = new BugListViewModel();
            try
            {
                ViewData["bugID"] = Request.Path.Value.Split('/')[3];
                var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(3600))
            .SetSize(1024);
                memoryCache.Set(MemoryCacheKey.sBugID, ViewData["bugID"].ToString(), cacheEntryOptions);

                
                NpgsqlCommand cmd = new NpgsqlCommand($"select * from BugList where id='{Request.Path.Value.Split('/')[3]}'", conn);

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataSet _ds = new DataSet();
                DataTable _dt = new DataTable();

                da.Fill(_ds);
                _dt = _ds.Tables[0];

                
                Model.Summary = _dt.Rows[0]["summary"].ToString();
                Model.Description = _dt.Rows[0]["description"].ToString();
               
            }
            catch (Exception ex)
            {

            }
            return View(Model);
        }

        [HttpPost]
        public IActionResult Edit(string Summary, string Description)
        {
            List<BugListViewModel> BugModelList = new List<BugListViewModel>();
            try
            {
                ViewData["User"] = memoryCache.Get(MemoryCacheKey.sUser);


                conn.Open();
                string sBugID = memoryCache.Get(MemoryCacheKey.sBugID).ToString();
                string sSQL = $"Update buglist set summary='{Summary}',description='{Description}' where id='{sBugID}'";
                NpgsqlCommand cmd = new NpgsqlCommand(sSQL, conn);
                cmd.ExecuteNonQuery();

                BugModelList = GetBugLists();
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return View("Index", BugModelList);
        }


        public IActionResult Resolve()
        {
            List<BugListViewModel> BugModelList = new List<BugListViewModel>();
            try
            {
                ViewData["User"] = memoryCache.Get(MemoryCacheKey.sUser);
                string sBugID = Request.Path.Value.Split('/')[3];

                conn.Open();
                string sSQL = $"Update buglist set status='Resolved' where id='{sBugID}'";
                NpgsqlCommand cmd = new NpgsqlCommand(sSQL, conn);
                cmd.ExecuteNonQuery();

                BugModelList = GetBugLists();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return View("Index", BugModelList);
        }

        public IActionResult Delete()
        {
            List<BugListViewModel> BugModelList = new List<BugListViewModel>();
            try
            {
                ViewData["User"] = memoryCache.Get(MemoryCacheKey.sUser);
                string sBugID = Request.Path.Value.Split('/')[3];

                conn.Open();
                string sSQL = $"delete from buglist where id='{sBugID}'";
                NpgsqlCommand cmd = new NpgsqlCommand(sSQL, conn);
                cmd.ExecuteNonQuery();

                BugModelList = GetBugLists();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return View("Index", BugModelList);
        }

        public IActionResult Create_Bug()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create_Bug(string Summary, string Description, string Type, string Priority, string Status)
        {
            List<BugListViewModel> BugModelList = new List<BugListViewModel>();
            try
            {
                ViewData["User"] = memoryCache.Get(MemoryCacheKey.sUser);
                string sUser = memoryCache.Get(MemoryCacheKey.sUser).ToString();

                conn.Open();
                string sSQL = $"Insert into buglist (id, summary, description, type, priority, status, create_user, create_date) ";
                sSQL += $"VALUES ('{DateTime.Now.ToString("yyyyMMddHHmmss")}', '{Summary}', '{Description}', '{Type}', '{Priority}', '{Status}','{sUser}','{DateTime.Now.ToString("yyyyMMdd")}')";
                NpgsqlCommand cmd = new NpgsqlCommand(sSQL, conn);
                cmd.ExecuteNonQuery();

                BugModelList = GetBugLists();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return View("Index", BugModelList);
        }

        public IActionResult Create_FeatureRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create_FeatureRequest(string Summary, string Description, string Type, string Priority, string Status)
        {
            List<BugListViewModel> BugModelList = new List<BugListViewModel>();
            try
            {
                ViewData["User"] = memoryCache.Get(MemoryCacheKey.sUser);
                string sUser = memoryCache.Get(MemoryCacheKey.sUser).ToString();

                conn.Open();
                string sSQL = $"Insert into buglist (id, summary, description, type, priority, status, create_user, create_date) ";
                sSQL += $"VALUES ('{DateTime.Now.ToString("yyyyMMddHHmmss")}', '{Summary}', '{Description}', '{Type}', '{Priority}', '{Status}','{sUser}','{DateTime.Now.ToString("yyyyMMdd")}')";
                NpgsqlCommand cmd = new NpgsqlCommand(sSQL, conn);
                cmd.ExecuteNonQuery();

                BugModelList = GetBugLists();
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return View("Index", BugModelList);
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
            public static string sUser = "User";
            public static string sBugID = "bugID";
        }

        public List<BugListViewModel> GetBugLists()
        {
            List<BugListViewModel> BugModelList = new List<BugListViewModel>();

            NpgsqlCommand cmd = new NpgsqlCommand("select * from BugList", conn);

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
            DataSet _ds = new DataSet();
            DataTable _dt = new DataTable();

            da.Fill(_ds);
            _dt = _ds.Tables[0];

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                BugListViewModel BugListModel = new BugListViewModel();
                BugListModel.ID = _dt.Rows[i]["id"].ToString();
                BugListModel.Summary = _dt.Rows[i]["summary"].ToString();
                BugListModel.Description = _dt.Rows[i]["description"].ToString();
                BugListModel.Type = _dt.Rows[i]["type"].ToString();
                BugListModel.Priority = _dt.Rows[i]["priority"].ToString();
                BugListModel.Status = _dt.Rows[i]["status"].ToString();
                BugListModel.CreateUser = _dt.Rows[i]["create_user"].ToString();
                BugListModel.CreateDate = _dt.Rows[i]["create_date"].ToString();
                BugListModel.EndUser = _dt.Rows[i]["end_user"].ToString();
                BugListModel.EndDate = _dt.Rows[i]["end_date"].ToString();
                BugModelList.Add(BugListModel);
            }

            return BugModelList;
        }
    }
}
