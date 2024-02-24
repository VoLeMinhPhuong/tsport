using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSPORT.Models;

namespace TSPORT.Controllers
{
    public class SearchController : Controller
    {
        dbDataContext data = new dbDataContext();
        // GET: Search
        public ActionResult TimKiem(string Search)
        {
            ViewBag.Search = Search;

            if (!string.IsNullOrEmpty(Search))
            {
                var kq = data.SANPHAMs.Where(s =>
                s.TenSanPham.Contains(Search) ||                
                s.LOAISP.TenLoai.Contains(Search) ||
                s.MoTa.Contains(Search))                    
                .OrderByDescending(s => s.SoLuongBan)            
                .ThenByDescending(s => s.NgayCapNhat);          
                return View(kq.ToList());
            }
            return View();
        }
    }
}