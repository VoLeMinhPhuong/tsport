using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSPORT.Models;

namespace TSPORT.Areas.Admin.Controllers
{
    
    public class HomeAdminController : Controller
    {
        dbDataContext db = new dbDataContext();
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];
            //Gán giá trị cho đối tượng được tạo mới (ad)
            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau
            == sMatKhau);
            if (ad != null)
            {

                Session["Admin"] = sTenDN;
                Session["Hoten"] = ad.HoTen;
                return RedirectToAction("Index", "HomeAdmin");

                /*Session["Admin"] = ad;
                return RedirectToAction("Index", "Home");*/
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}