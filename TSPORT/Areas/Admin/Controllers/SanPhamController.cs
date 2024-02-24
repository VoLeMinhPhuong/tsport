using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using TSPORT.Models;

namespace TSPORT.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        dbDataContext db = new dbDataContext();
        // GET: Admin/Sach
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 7;
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MaSanPham).ToPagedList(iPageNum, iPageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaLoai = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SANPHAM sp, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            //Đưa dữ liệu vào DropDown
            ViewBag.MaLoai = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            if (fFileUpload == null)
            {
                //Nội dung thông báo yêu cầu chọn ảnh bìa
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                //Lưu thông tin để khi load lại trang do yêu cầu chọn ảnh bìa sẽ hiển thị các thông tin này lên trang 
                ViewBag.TenSanPham = f["sTenSanPham"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaLoai = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", int.Parse(f["MaLoai"]));
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/File/img"), sFileName);
                    //Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    // Luu Sach vao CSDL
                    sp.TenSanPham = f["sTenSanPham"];
                    sp.MoTa = f["sMoTa"];
                    sp.AnhBia = sFileName;
                    sp.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                    sp.SoLuongBan = int.Parse(f["iSoLuong"]);
                    sp.GiaBan = decimal.Parse(f["mGiaBan"]);
                    sp.MaLoai = int.Parse(f["MaLoai"]);
                    db.SANPHAMs.InsertOnSubmit(sp);
                    db.SubmitChanges();
                    // Về lại trang Quản lý sách 
                    return RedirectToAction("Index");
                }
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            if (sp == null)
            {
                Response.StatusCode = 404; return null;
            }
            return View(sp);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(sp);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSanPham == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sản phẩm này đang có trong bảng Chi tiết đặt hàng <br>" + "Nếu muốn xóa thì phải xóa hết mã sách này trong bảng Chi tiết đặt hàng";
                return View(sp);
            } //Xóa hết thông tin của cuốn sách trong table VietSach trước khi xóa sách này
            var vietsach = db.VIETSANPHAMs.Where(vs => vs.MaSanPham == id).ToList();
            if (vietsach != null)
            {
                db.VIETSANPHAMs.DeleteAllOnSubmit(vietsach);
                db.SubmitChanges();
            }
            db.SANPHAMs.DeleteOnSubmit(sp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            if (sp == null)
            {
                Response.StatusCode = 404; return null;
            }
            ViewBag.MaLoai = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sp.MaLoai);
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            var sp = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == int.Parse(f["iMaSanPham"]));
            ViewBag.MaLoai = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoai),
            " MaLoai", "TenLoai", sp.MaLoai);
            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName); var path = Path.Combine(Server.MapPath("~/File/img"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sp.AnhBia = sFileName;
                }
                sp.TenSanPham = f["sTenSanPham"]; sp.MoTa = f["sMoTa"];
                sp.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]); sp.SoLuongBan = int.Parse(f["iSoLuong"]); sp.GiaBan = decimal.Parse(f["mGiaBan"]);
                sp.MaLoai = int.Parse(f["MaLoai"]);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(sp);
        }
    }
}