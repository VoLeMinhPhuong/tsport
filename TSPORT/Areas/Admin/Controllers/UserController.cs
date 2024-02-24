using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSPORT.Models;

namespace TSPORT.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        dbDataContext data = new dbDataContext();
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap(string returnUrl)
        {
            ViewBag.Url = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection, string returnUrl)
        {
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["Matkhau"];

            if (String.IsNullOrEmpty(sTenDN) || String.IsNullOrEmpty(sMatKhau))
            {
                ViewBag.ThongBao = "Tên đăng nhập và mật khẩu không được để trống";
                return View();
            }
            // string hashedPassword = HashPassword(sMatKhau);
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);

            if (kh != null)
            {
                ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                Session["TaiKhoan"] = kh;
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    // Quay lại trang trước đó sau khi đăng nhập thành công
                    return Redirect(returnUrl);
                }
                else
                {
                    // Chuyển hướng người dùng đến trang đặt hàng
                    return Redirect("~/GioHang/GioHang");
                }

            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
        }
        public ActionResult LogOut(string username, string password)
        {
            Session["TaiKhoan"] = null; // Xóa phiên đăng nhập
            return RedirectToAction("Index", "Home"); // Chuyển hướng về trang chính hoặc trang khác mà bạn muốn.
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection f, KHACHHANG kh)
        {
            var hoten = f["HoTen"];
            var tendn = f["TenDN"];
            var matkhau = f["Matkhau"];
            var matkhaunl = f["MatkhauNL"];
            var diachi = f["DiaChi"];
            var email = f["Email"];
            var dienthoai = f["DienThoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", f["NgaySinh"]);


            ////
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["err1"] = "Họ tên không để trống!";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["err2"] = "Tên Đăng Nhập không để trống!";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["err3"] = "Mật khẩu không để trống!";
            }
            else if (String.IsNullOrEmpty(matkhaunl))
            {
                ViewData["err4"] = "Mật Khẩu nhập lại không để trống!";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["err5"] = "Email không để trống!";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["err6"] = "SĐT không để trống!";
            }
            else if (data.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendn) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (data.KHACHHANGs.SingleOrDefault(n => n.Email == email) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DiaChi = diachi;
                kh.DienThoai = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }
        private bool IsValidToken(string token)
        {
            return true; // Replace this with your actual validation logic
        }
        [HttpGet]
        public ActionResult ResetPassword(string token)
        {
            if (IsValidToken(token))
            {
                var model = new ForgotPassword
                {
                    Token = token
                };
                return View(model);
            }
            else
            {
                // Redirect to an error page or handle it accordingly
                return RedirectToAction("PasswordResetError");
            }
        }

        [HttpPost]
        public ActionResult ResetPassword(ForgotPassword model)
        {
            return RedirectToAction("ResetPassword");
        }
    }
}