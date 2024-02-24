using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSPORT.Models
{
    public class GioHang
    {
        dbDataContext db = new dbDataContext();
        public int iMaSanPham { get; set; }
        public string sTenSanPham { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        
        public double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        public string HinhThucThanhToan { get; set; }
        public string MaTaiKhoanMomo { get; set; }
        

        public GioHang(int ms)
        {
            iMaSanPham = ms;
            SANPHAM s = db.SANPHAMs.Single(n => n.MaSanPham == iMaSanPham);
            sTenSanPham = s.TenSanPham;
            sAnhBia = s.AnhBia;
            dDonGia = double.Parse(s.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}