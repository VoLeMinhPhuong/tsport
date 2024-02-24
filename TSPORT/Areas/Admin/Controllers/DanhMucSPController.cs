using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSPORT.Models;

namespace TSPORT.Areas.Admin.Controllers
{
    public class DanhMucSPController : Controller
    {
        dbDataContext db = new dbDataContext();
        // GET: Admin/DanhMucSP
        public ActionResult Index()
        {
            return View(db.LOAISPs);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(LOAISP model)
        {
            if (ModelState.IsValid)
            {
                // Tạo một đối tượng LOAISP mới từ dữ liệu trong model
                var newLoaiSP = new LOAISP
                {
                    TenLoai = model.TenLoai // Giả sử TenLoai là một thuộc tính trong YourModel
                };

                // Thêm đối tượng mới vào DataContext
                db.LOAISPs.InsertOnSubmit(newLoaiSP);

                // Lưu thay đổi
                db.SubmitChanges();

                // Chuyển hướng đến một trang khác sau khi thêm thành công
                return RedirectToAction("Index"); // Thay "Index" bằng tên action mà bạn muốn chuyển hướng đến
            }

            // Nếu ModelState không hợp lệ, quay lại view hiện tại với thông báo lỗi
            return View(model);
        }
        public ActionResult Detail(int id)
        {
            var lsp = db.LOAISPs.SingleOrDefault(n => n.MaLoai == id);
            if (lsp == null)
            {
                Response.StatusCode = 404; return null;
            }
            return View(lsp);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var lsp = db.LOAISPs.SingleOrDefault(n => n.MaLoai == id);
            if (lsp == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(lsp);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id)
        {
            var lsp = db.LOAISPs.SingleOrDefault(n => n.MaLoai == id);
            if (lsp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            var sp = db.SANPHAMs.Where(s => s.MaLoai == id).ToList();

            // Xoá các sách liên quan đến chủ đề
            foreach (var s in sp)
            {
                // Kiểm tra và xóa chi tiết đặt hàng liên quan
                var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSanPham == s.MaSanPham).ToList();
                if (ctdh.Count > 0)
                {
                    ViewBag.ThongBao = "Chủ đề này đang có sách trong bảng Chi tiết đặt hàng. Hãy xóa hết sách liên quan trước.";
                    return View(lsp);
                }

                // Xóa thông tin sách trong bảng VIETSACH
                var vietsach = db.VIETSANPHAMs.Where(vs => vs.MaSanPham == s.MaSanPham).ToList();
                if (vietsach.Count > 0)
                {
                    db.VIETSANPHAMs.DeleteAllOnSubmit(vietsach);
                    db.SubmitChanges();
                }

                // Xóa sách
                db.SANPHAMs.DeleteOnSubmit(s);
            }

            // Xóa chủ đề
            db.LOAISPs.DeleteOnSubmit(lsp);
            db.SubmitChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var lsp = db.LOAISPs.SingleOrDefault(n => n.MaLoai == id);
            if (lsp == null)
            {
                Response.StatusCode = 404; return null;
            }
            return View(lsp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            var lsp = db.LOAISPs.SingleOrDefault(n => n.MaLoai == int.Parse(f["iMaLoai"]));
            if (ModelState.IsValid)
            {
                lsp.TenLoai = f["sTenLoai"];
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return View(lsp);
        }
    }
}