using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using TSPORT.Models;

namespace TSPORT.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        dbDataContext db = new dbDataContext();

        // GET: Admin/KhachHang
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 5;
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(iPageNum, iPageSize));
        }
        public ActionResult Details(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 400;
                return null;
            }
            return View(kh);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);

            if (kh == null)
            {
                // Xử lý khi không tìm thấy khách hàng với MaKH tương ứng
                // Ví dụ: Chuyển hướng đến một trang lỗi hoặc hiển thị thông báo lỗi
                ViewBag.ErrorMessage = "Không tìm thấy khách hàng với MaKH tương ứng.";
                return View("Error"); // Chuyển hướng đến trang lỗi hoặc thay đổi tùy ý
            }

            return View(kh);
        }

        [HttpPost]
        public ActionResult Edit(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                // Tìm khách hàng dựa trên MaKH
                var existingKh = db.KHACHHANGs.FirstOrDefault(n => n.MaKH == kh.MaKH);

                if (existingKh != null)
                {
                    // Cập nhật thông tin khách hàng từ model
                    existingKh.HoTen = kh.HoTen;
                    existingKh.TaiKhoan = kh.TaiKhoan;
                    existingKh.MatKhau = kh.MatKhau;
                    existingKh.Email = kh.Email;
                    existingKh.NgaySinh = kh.NgaySinh;
                    existingKh.DienThoai = kh.DienThoai;
                    existingKh.DiaChi = kh.DiaChi;

                    try
                    {
                        // Lưu thay đổi vào cơ sở dữ liệu
                        db.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        // Xử lý lỗi cơ sở dữ liệu
                        ViewBag.ErrorMessage = "Lỗi cơ sở dữ liệu: " + ex.Message;
                        return View("Error");
                    }
                }
            }

            // Xử lý khi dữ liệu không hợp lệ hoặc khi không tìm thấy khách hàng
            ViewBag.ErrorMessage = "Dữ liệu không hợp lệ hoặc không tìm thấy khách hàng với MaKH tương ứng.";
            return View("Error");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(KHACHHANG kh)
        {
            if (ModelState.IsValid)
            {
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }

            return View(kh);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                return HttpNotFound();
            }
            return View(kh);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                return HttpNotFound(); // Return a 404 status if the customer is not found
            }

            db.KHACHHANGs.DeleteOnSubmit(kh);
            db.SubmitChanges();

            return RedirectToAction("Index");
        }
    }
}