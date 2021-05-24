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
    /// <summary>
    ///   Controller hiển thị mà hình quản lí nhân viên, hiển thị thông tin, thêm, sửa xóa nhân viên.
    /// </summary>
    /// <Modified>
    /// Name Date Comments
    /// khanhnn 5/24/2021 created
    /// </Modified>
    [Authorize]
    public class UsersController : Controller
    {

        /// <summary>Dung để thao tác với database</summary>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
        private readonly ApplicationDBContext _db;
        public UsersController(ApplicationDBContext db)
        {
            _db = db;
        }

        /// <summary>Hiển thị tất cả các user có trong database</summary>
        /// <param name="sortOrder"></param>
        /// <param name="currentFilter"></param>
        /// <param name="searchString">Tìm kiếm nhân viên</param>
        /// <param name="pageNumber">Chỉ số trang</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
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
        /// <summary>Chọn nút thêm nhân viên</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
        public IActionResult AddUser()
        {
            TempData["Case"] = "add";
            return RedirectToAction("Confirm", new { Id = 0 });
        }

        // Get
        /// <summary>Chọn nút sửa thông tin nhân viên</summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
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
        /// <summary>Chọn nút xóa nhân viên</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
        public IActionResult DeleteUser(int id)
        {
            TempData["Case"] = "delete";
            return RedirectToAction("Confirm", new { Id = id });
        }

        // Post
        /// <summary>Gửi đi hành động thêm nhân viên</summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
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
        /// <summary>Gửi đi hành động sửa nhân viên</summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
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

        /// <summary>Gửi đi hành động xóa nhân viên</summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
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
        /// <summary>Hiển thị thông tin nhân viên</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
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

        /// <summary>Xác nhận kết quả cho việc thêm, sửa xóa nhân viên</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
        public IActionResult Result()
        {
            if (TempData["Result"] != null)
            {
                ViewBag.Result = TempData["Result"].ToString();
                TempData.Remove("Result");
            }
            return View();
        }

        /// <summary>Confirms the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   Xác nhận thông tin thêm, sửa, xóa, cảnh báo lỗi khi thông tin không hợp lệ
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
        public IActionResult Confirm(int id)
        {
            if (TempData["Case"] != null)
            {
                // Lưu thông tin trường hợp sửa thông tin nhân viên không hợp lệ
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
                // Lưu thông tin nhập vào trường hợp thông tin user thêm vào không hợp lệ
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

        /// <summary>Quay trở lại màn hình hiển thị thông tin nhân viên</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <Modified>
        /// Name Date Comments
        /// khanhnn 5/24/2021 created
        /// </Modified>
        public IActionResult Back(int id)
        {
            return RedirectToAction("DetailUser", new { Id = id });
        }


    }
}
