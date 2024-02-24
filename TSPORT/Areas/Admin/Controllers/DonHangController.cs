using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TSPORT.Models;

namespace TSPORT.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        dbDataContext db = new dbDataContext();
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 5;
            return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(iPageNum, iPageSize));
        }
    }
}