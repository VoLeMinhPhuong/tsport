using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSPORT.Models;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;

namespace TSPORT.Controllers
{
    public class HomeController : Controller
    {
        dbDataContext data = new dbDataContext();
        private List<SANPHAM> layspmoi(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            var danhSachSP = data.SANPHAMs.ToList();
            int pageSize = 8;
            int pageNum = (page ?? 1);
            var spMoi = layspmoi(23);
            return View(spMoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Shop(int ? page)
        {
            var danhSachSP = data.SANPHAMs.ToList();
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var spMoi = layspmoi(23);
            return View(spMoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Detail(int ? page)
        {
            var danhSachSP = data.SANPHAMs.ToList();
            int pageSize = 4;
            int pageNum = (page ?? 1);
            var spMoi = layspmoi(23);
            return View(spMoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult ShoppingCart()
        {
            return View();
        }
        public ActionResult CheckOut()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult DanhMucSanPhamPartial()
        {
            var listSanPham = from sp in data.LOAISPs select sp;
            return PartialView(listSanPham);
        }
        public ActionResult SanPham(int id, int? page)
        {
            // Lấy sách theo mã chủ đề
            var spTheoLoai = data.SANPHAMs
                                  .Where(s => s.MaLoai == id)
                                  .ToList();
            int pageNumber = (page ?? 1); // Trang mặc định nếu không có page được chỉ định
            int pageSize = 8; // Số lượng mục trên mỗi trang, bạn có thể điều chỉnh theo mong muốn
            // Sử dụng ToPagedList để phân trang danh sách sách
            IPagedList<SANPHAM> pagedSPTheoLoai = spTheoLoai.ToPagedList(pageNumber, pageSize);
            return View(pagedSPTheoLoai);
        }
        public ActionResult ChiTietSanPham(int id)
        {
            var sp = from s in data.SANPHAMs
                     where s.MaSanPham == id
                     select s;
            return View(sp.Single());
        }
        public ActionResult TimKiem(int ? page,string search = "")
        {
            var result = from b in data.SANPHAMs select b;
            int pageSize = 6;
            int pageNum = (page ?? 1);
            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(x => x.TenSanPham.Contains(search));
            }
            return View(result.ToPagedList(pageNum, pageSize));
        }
        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogoutPartial");
        }
    }
}