using LabDemoWebASPMVC.Data;
using LabDemoWebASPMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabDemoWebASPMVC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {

        private readonly ApplicationDBContext _db;
        public UsersController(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string sortOrder,
    string currentFilter,
    string searchString,
    int? pageNumber)
        {
            
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var students = from s in _db.Users
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString));
            }
            int pageSize = 3;
            return View(await PaginatedList<Users>.CreateAsync(students, pageNumber ?? 1, pageSize));
        }

        // Get
        public IActionResult AddUser()
        {
            TempData["Case"] = "add";
            return RedirectToAction("Confirm", new { Id = 0 });
        }

        // Get
        public IActionResult EditUser(Users user)
        {
            IEnumerable<Users> lst = _db.Users.Where(x => x.Email.CompareTo(user.Email) == 0 && x.Id != user.Id).ToList();
            // Case change email
            if (lst != null && lst.Count() > 0)
            {
                TempData["edit"] = "Email đã được sử dụng";
                return RedirectToAction("DetailUser", new { Id = user.Id });
            }
            TempData["Case"] = "edit";
            TempData["EditName"] = user.Name;
            TempData["EditEmail"] = user.Email;
            TempData["EditTel"] = user.Tel;
            return RedirectToAction("Confirm", new { Id = user.Id });
        }
        public IActionResult DeleteUser(int id)
        {
            TempData["Case"] = "delete";
            return RedirectToAction("Confirm", new { Id = id });
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUserPost(Users user)
        {
            var checkDuplicateEmail = _db.Users.SingleOrDefault(b => b.Email.CompareTo(user.Email) == 0);
            if (checkDuplicateEmail != null)
            {
                TempData["AddError"] = "Email đã được sử dụng";
                TempData["AddName"] = user.Name;
                TempData["AddEmail"] = user.Email;
                TempData["AddTel"] = user.Tel;
                return RedirectToAction("AddUser");
            }
            try 
            {
                var result = _db.Users.Add(user);
                _db.SaveChanges();
                TempData["Result"] = "Đăng kí thành công";
            }
            catch (Exception e)
            {
                TempData["Result"] = "Đăng kí không thành công, hãy nhập lại thông tin nhân viên";
            }
            return RedirectToAction("Result");
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUserPost(Users user)
        {   
            try
            {
                var result = _db.Users.Update(user);
                _db.SaveChanges();
                TempData["Result"] = "Updat thông tin thành công";
            }
            catch (Exception e)
            {
                TempData["Result"] = "Update không thành công";
            }
            return RedirectToAction("Result");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUserPost(Users user)
        {
            try
            {
                _db.Users.Remove(user);
                _db.SaveChanges();
                TempData["Result"] = "Delete thông tin thành công";
            }
            catch (Exception e)
            {
                TempData["Result"] = "Delete không thành công";
            }
            return RedirectToAction("Result");
        }

        // Get
        public IActionResult DetailUser(int id)
        {
            if (TempData["edit"] != null)
            {
                ViewBag.Message = TempData["edit"].ToString();
                TempData.Remove("edit");
            }
            var result = _db.Users.SingleOrDefault(b => b.Id == id);
            return View(result);
        }

        public IActionResult Result()
        {
            if (TempData["Result"] != null)
            {
                ViewBag.Result = TempData["Result"].ToString();
                TempData.Remove("Result");
            }
            return View();
        }

        public IActionResult Confirm(int id)
        {
            if (TempData["Case"] != null)
            {
                if (TempData["Case"].ToString() == "add")
                {
                    if (TempData["AddError"] != null)
                    {
                        ViewBag.AddError = TempData["AddError"].ToString();
                        TempData.Remove("AddError");
                    }
                    else
                    {
                        ViewBag.AddError = "";
                    }
                    // Save input fields in case add user fail 
                    if (TempData["AddName"] != null)
                    {
                        ViewBag.AddName = TempData["AddName"].ToString();
                        TempData.Remove("AddName");
                    }
                    else
                    {
                        ViewBag.AddName = "";
                    }
                    if (TempData["AddEmail"] != null)
                    {
                        ViewBag.AddEmail = TempData["AddEmail"].ToString();
                        TempData.Remove("AddEmail");
                    }
                    else
                    {
                        ViewBag.AddEmail = "";
                    }
                    if (TempData["AddTel"] != null)
                    {
                        ViewBag.AddTel = TempData["AddTel"].ToString();
                        TempData.Remove("AddTel");
                    }
                    else
                    {
                        ViewBag.AddTel = "";
                    }
                }
                ViewBag.Case = TempData["Case"].ToString();
                TempData.Remove("Case");
            }
            
            var result = _db.Users.SingleOrDefault(b => b.Id == id);
            if (result != null)
            {
                if (TempData["EditName"] != null)
                {
                    result.Name = TempData["EditName"].ToString();
                    TempData.Remove("EditName");
                }
                if (TempData["EditEmail"] != null)
                {
                    result.Email = TempData["EditEmail"].ToString();
                    TempData.Remove("EditEmail");
                }
                if (TempData["EditTel"] != null)
                {
                    result.Tel = TempData["EditTel"].ToString();
                    TempData.Remove("EditTel");
                }
                return View(result);
            }
            else
                return View(new Users());
        }

        public IActionResult Back(int id)
        {
            return RedirectToAction("DetailUser", new { Id = id });
        }

        
    }
}
