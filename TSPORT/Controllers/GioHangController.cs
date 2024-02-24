using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TSPORT.Models;

namespace TSPORT.Controllers
{
    public class GioHangController : Controller
    {
        dbDataContext db = new dbDataContext();
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FailureView()
        {
            return View();
        }
        public ActionResult SuccessView()
        {
            return View();
        }
        //
        private double TongtienUSD()
        {
            double iTongtien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongtien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return Math.Round(iTongtien / 24000, 2);
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            //double totalAmount = TongtienUSD();
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            /*foreach (var item in lstGioHang)
            {
                itemList.items.Add(new Item()
                {
                    name = item.sTenSanPham,
                    currency = "USD",
                    price = Math.Round(item.dDonGia / 24000, 2).ToString(),
                    quantity = item.iSoLuong.ToString(),
                    sku = item.iMaSanPham.ToString()
                });
            }*/
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name come here",
                currency = "USD",
                price = "1",
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "3",// Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
            
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //
        public ActionResult ThemGioHang(int ms, string url = "/")
        {
            // Lấy giỏ hàng hiện tại
            List<GioHang> lstGioHang = LayGioHang();
            // Kiểm tra nếu sản phẩm chưa có trong giỏ thì thêm vào, nếu đã có thì thêm số lượng
            GioHang sp = lstGioHang.Find(n => n.iMaSanPham == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }
        // Tính tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        // Tính tổng tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        // Action trả về view GioHang
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult CapNhatGioHang(int iMaSanPham, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSanPham == iMaSanPham);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult XoaSanPhamGioHang(int id)
        {
            List<GioHang> lstGioHang = LayGioHang();
            for (int i = 0; i < lstGioHang.Count; i++)
            {
                if (lstGioHang[i].iMaSanPham == id)
                {
                    lstGioHang.RemoveAt(i);
                    break;
                }
            }
            Session["GioHang"] = lstGioHang;
            if (lstGioHang.Count > 0)
            {
                return RedirectToAction("GioHang", "GioHang");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập và truyền returnUrl
                return RedirectToAction("DangNhap", "User");
            }

            // Kiểm tra có hàng trong giỏ chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Nếu đã đăng nhập và có hàng trong giỏ, hiển thị trang Đặt Hàng
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang); // Trả về view "DatHang" và truyền danh sách giỏ hàng làm model
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(NgayGiao);
            ddh.TinhTrangGiaoHang = 1;
            ddh.DaThanhToan = false;
            db.DONDATHANGs.InsertOnSubmit(ddh);
            db.SubmitChanges();
            Session["GioHang"] = null;

            string hinhThucThanhToan = f["HinhThucThanhToan"];

            if (hinhThucThanhToan == "ThanhToanKhiNhanHang")
            {
                string httt = "Khi nhận hàng";
                // Thực hiện thanh toán khi nhận hàng
                // ...
                // Gửi email thông tin đơn hàng đến địa chỉ email của khách hàng
                string emailNguoiNhan = kh.Email; // Lấy địa chỉ email của khách hàng từ đối tượng KHACHHANG
                GuiEmailDonHang(ddh, emailNguoiNhan, lstGioHang, httt);
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            else if (hinhThucThanhToan == "ThanhToanOnline")
            {
                string httt = "Online";

                string emailNguoiNhan = kh.Email; // Lấy địa chỉ email của khách hàng từ đối tượng KHACHHANG
                GuiEmailDonHang(ddh, emailNguoiNhan, lstGioHang, httt);
                // Sau khi thanh toán thành công, chuyển đến trang xác nhận đơn hàng
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            else
            {
                // Lỗi: Hình thức thanh toán không hợp lệ
                return RedirectToAction("Index", "GioHang");
            }
        }
        private void GuiEmailDonHang(DONDATHANG donHang, string emailNguoiNhan, List<GioHang> gioHangItems, string hinhThucThanhToan)
        {
            // Tạo nội dung email dựa trên thông tin đơn hàng và sản phẩm
            string subject = "Thông tin đơn hàng #" + donHang.MaDonHang;
            string body = $"<h2>Thông tin đơn hàng #{donHang.MaDonHang}</h2>";
            body += $"<p>Mã đơn hàng: {donHang.MaDonHang}</p>";
            body += $"<p>Ngày đặt: {donHang.NgayDat}</p>";
            body += $"<p>Tình trạng giao hàng: {donHang.TinhTrangGiaoHang}</p>";
            // Thêm thông tin hình thức thanh toán
            body += $"<p>Hình thức thanh toán: {hinhThucThanhToan}</p>";
            body += "<h3>Danh sách sản phẩm:</h3>";
            body += "<table border='1'><tr><th>Tên sách</th><th>Đơn giá</th><th>Số lượng</th><th>Thành tiền</th></tr>";

            foreach (var item in gioHangItems)
            {
                body += "<tr>";
                body += $"<td>{item.sTenSanPham}</td>";
                body += $"<td>{item.dDonGia}</td>";
                body += $"<td>{item.iSoLuong}</td>";
                body += $"<td>{item.dThanhTien}</td>";
                body += "</tr>";
            }

            body += "</table>";
            // Tính tổng tiền
            double tongTien = gioHangItems.Sum(item => item.dThanhTien);
            body += $"<p>Tổng tiền: {tongTien}</p>";
            // Gửi email
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("2124802010356@student.tdmu.edu.vn", "ctsp jqch shla nbna"),
                    EnableSsl = true,
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("2124802010356@student.tdmu.edu.vn"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true, // Sử dụng HTML cho nội dung email
                };

                mailMessage.To.Add(emailNguoiNhan);

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi gửi email
                throw ex;
            }
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}