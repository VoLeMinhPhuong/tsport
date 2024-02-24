USE MASTER
DROP DATABASE TSPORT

CREATE DATABASE TSPORT
GO
USE TSPORT
GO

CREATE TABLE LOAISP
(
	MaLoai INT IDENTITY(1,1),
	TenLoai NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_Loai PRIMARY KEY(MaLoai)
)
GO

CREATE TABLE SANPHAM
(
	MaSanPham INT IDENTITY(1,1),
	TenSanPham NVARCHAR(100) NOT NULL,
	MoTa NTEXT,
	AnhBia VARCHAR(50),
	NgayCapNhat SMALLDATETIME,
	SoLuongBan INT CHECK(SoLuongBan>0),
	GiaBan MONEY CHECK (GiaBan>=0),
	MaLoai INT,
	CONSTRAINT PK_SP PRIMARY KEY(MaSanPham)
)
GO

CREATE TABLE KHACHHANG
(
	MaKH INT IDENTITY(1,1),
	HoTen NVARCHAR(50) NOT NULL,
	TaiKhoan VARCHAR(15) UNIQUE,
	MatKhau VARCHAR(15) NOT NULL,
	Email VARCHAR(50) UNIQUE,
	DiaChi NVARCHAR(50),
	DienThoai VARCHAR(10),
	NgaySinh SMALLDATETIME,
	CONSTRAINT PK_Kh PRIMARY KEY(MaKH)
)
GO

CREATE TABLE DONDATHANG
(
	MaDonHang INT IDENTITY(1,1),
	DaThanhToan BIT DEFAULT(0),
	TinhTrangGiaoHang INT DEFAULT(1),
	NgayDat SMALLDATETIME,
	NgayGiao SMALLDATETIME,
	MaKH INT,
	CONSTRAINT PK_DDH PRIMARY KEY(MaDonHang)
)
GO

CREATE TABLE CHITIETDATHANG
(
	MaDonHang INT,
	MaSanPham INT,
	SoLuong INT CHECK(SoLuong>0),
	DonGia DECIMAL(9,2) CHECK(DonGia>=0),
	CONSTRAINT PK_CTDH PRIMARY KEY(MaDonHang,MaSanPham)
)
GO

CREATE TABLE NHACUNGCAP
(
	MaNCC INT IDENTITY(1,1),
	TenNCC NVARCHAR(50) NOT NULL,
	DiaChi NVARCHAR(100),
	TieuSu NTEXT,
	DienThoai VARCHAR(15),
	CONSTRAINT PK_NCC PRIMARY KEY(MaNCC)
)
GO

CREATE TABLE VIETSANPHAM
(
	MaNCC INT,
	MaSanPham INT,
	VaiTro NVARCHAR(30),
	ViTri NVARCHAR(30),
	CONSTRAINT PK_VS PRIMARY KEY(MaNCC,MaSanPham)
)
GO

CREATE TABLE TRANGTIN
(
	MaTT INT IDENTITY(1,1) PRIMARY KEY,
	TenTrang NVARCHAR(100) NOT NULL,
	NoiDung NTEXT,
	NgayTao SMALLDATETIME,
	MetaTitle NVARCHAR(100)
	
)
GO

CREATE TABLE MENU
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	MenuName NVARCHAR(50),
	MenuLink NVARCHAR(100),
	ParentId INT,
	OrderNumber INT
)
GO

ALTER TABLE SANPHAM ADD CONSTRAINT FK_SP FOREIGN KEY (MaLoai) REFERENCES LOAISP(MaLoai)
ALTER TABLE DONDATHANG ADD CONSTRAINT FK_DDH FOREIGN KEY (MaKH) REFERENCES KHACHHANG(MaKH)
ALTER TABLE CHITIETDATHANG ADD CONSTRAINT FK_CTDH_DDH FOREIGN KEY (MaDonHang) REFERENCES DONDATHANG(MaDonHang)
ALTER TABLE CHITIETDATHANG ADD CONSTRAINT FK_CTDH_SP FOREIGN KEY (MaSanPham) REFERENCES SANPHAM(MaSanPham)
ALTER TABLE VIETSANPHAM ADD CONSTRAINT FK_VSP_NCC FOREIGN KEY (MaNCC) REFERENCES NHACUNGCAP(MaNCC)
ALTER TABLE VIETSANPHAM ADD CONSTRAINT FK_VSP_SP FOREIGN KEY (MaSanPham) REFERENCES SANPHAM(MaSanPham)

SET IDENTITY_INSERT [dbo].[LOAISP] ON
INSERT [dbo].[LOAISP] ([MaLoai], [TenLoai]) VALUES (1, N'Máy Chạy Bộ')
INSERT [dbo].[LOAISP] ([MaLoai], [TenLoai]) VALUES (2, N'Tạ Đơn')
INSERT [dbo].[LOAISP] ([MaLoai], [TenLoai]) VALUES (3, N'Xà Đơn, Xà Kép')
INSERT [dbo].[LOAISP] ([MaLoai], [TenLoai]) VALUES (4, N'Dụng Cụ Hít Đất')
INSERT [dbo].[LOAISP] ([MaLoai], [TenLoai]) VALUES (5, N'Dây Kháng Lực')
INSERT [dbo].[LOAISP] ([MaLoai], [TenLoai]) VALUES (6, N'Dây Nhảy')
INSERT [dbo].[LOAISP] ([MaLoai], [TenLoai]) VALUES (7, N'Thảm')
INSERT [dbo].[LOAISP] ([MaLoai], [TenLoai]) VALUES (8, N'Parallettes')
INSERT [dbo].[LOAISP] ([MaLoai], [TenLoai]) VALUES (9, N'Máy Tập Đa Năng')
SET IDENTITY_INSERT [dbo].[LOAISP] OFF

SET IDENTITY_INSERT [dbo].[SANPHAM] ON
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (1, N'Máy chạy bộ AGURI AGT-108L TSPORT', 19990000, N'Máy chạy bộ đa năng AGT-108L đầm chắc, công suất lớn, trang bị chế độ nâng dốc tự động tạo độ khó trong quá trình tập luyện, giúp người dùng tiêu hao calo triệt để:
Thông số kỹ thuật
Motor: DC 4.0HP
Màn hình: LCD 7.0″ hiển thị thời gian, quãng đường, vận tốc, độ nghiêng, nhịp tim
Tốc độ: 1.0km/h – 22km/h
Độ nghiêng: 0 – 20%
Trọng lượng người dùng tối đa: 160kg
Diện tích băng tải: 540mm*1500mm
Diện tích đặt máy: 2000mm*860mm*1400mm
N.W/G.W: 102kg/117kg
Xuất xứ: Lắp ráp tại Trung Quốc
Xin trân trọng giới thiệu cùng các bạn.', N'mcb_01.png', 1, '04/12/2021', 35)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (2, N'Máy chạy bộ AGURI AGT-108L TSPORT', 19990000, N'Máy chạy bộ đa năng AGT-108L đầm chắc, công suất lớn, trang bị chế độ nâng dốc tự động tạo độ khó trong quá trình tập luyện, giúp người dùng tiêu hao calo triệt để:
Thông số kỹ thuật
Motor: DC 4.0HP
Màn hình: LCD 7.0″ hiển thị thời gian, quãng đường, vận tốc, độ nghiêng, nhịp tim
Tốc độ: 1.0km/h – 22km/h
Độ nghiêng: 0 – 20%
Trọng lượng người dùng tối đa: 160kg
Diện tích băng tải: 540mm*1500mm
Diện tích đặt máy: 2000mm*860mm*1400mm
N.W/G.W: 102kg/117kg
Xuất xứ: Lắp ráp tại Trung Quốc
Xin trân trọng giới thiệu cùng các bạn.', N'mcb_02.png', 1, '04/12/2021', 45)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (3, N'MÁY CHẠY BỘ AGT-117LE', 11590000, N'Máy chạy bộ đa năng AGT-117LE thiết kế trẻ trung, hiện đại, màu sắc cá tính bao gồm thông số sau:
Motor: DC 3.0 HP
Màn hình: LED 6 cửa sổ hiển thị: thời gian, quãng đường, vận tốc, nhịp tim, calories, chương trình mặc định
Tốc độ: 0.8km/h – 15km/h
Chương trình mặc định: P1-P12
Kích thước đặt máy: 1640*710*1250mm (dài-rộng-cao)
Kích thước đóng thùng: 1690*750*320mm
Diện tích băng tải:	430*1300mm
Trọng lượng người dùng tối đa: 130 kgs
N.W/G.W: 61/70kg
Gấp gọn: Máy có piston thủy lực nâng đỡ, có thể gấp gọn máy khi không sử dụng
Bánh xe phụ: Máy có bánh xe di chuyển rất dễ dàng
Đầu massage: Máy được trang bị 1 đầu massage, đánh mỡ bụng; bộ phận gập bụng
Hệ thống loa: Có hệ thống giảm xóc khi hoạt động, kết nối Bluetooth, hệ thống 2 loa stereo
Body fat: Có chức năng đo chỉ số cơ thể (Body fat)
Xuất xứ: Lắp ráp tại Trung Quốc
Xin trân trọng giới thiệu cùng các bạn.', N'mcb_03.jpg', 1, '04/11/2023', 23)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (4, N'Máy chạy bộ Makano DVCB-00003', 14220000, N'"Máy chạy bộ Makano DVCB-00003 sở hữu nhiều tính năng, tích hợp đa năng thông minh, hiện đại vượt trội:
MODEL: DVCB-00003
Loại sản phẩm: Đa năng
Điện áp: 220V-50Hz
Kích thước đóng thùng: 1690*760*325MM
Kích thước sản phẩm: 1660*735*1300MM
Có thể xếp gọn với kích thước xếp gọn: 1180*735*1320mm
Màn hình điều khiển: LCD lớn 5 inch, hiển thị sắc nét
Màn hình hiển thị rõ các chỉ số: Quãng đường, tốc độ, chỉ số Calo, thời gian tập luyện, đo nhịp tim
Xin trân trọng giới thiệu cùng các bạn.', N'mcb_04.png', 1, '5/12/2022', 40)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (5, N'Máy Chạy Bộ KLC Aguri KLA 838', 21000000, N'Máy chạy bộ KLC Aguri KLA 838 là dòng máy mới ra mắt của thương hiệu KLC trong năm 2021:
Model: KLC AL838
Màn hình: LCD 5.0″ hiển thị thời gian, quãng đường, vận tốc, độ nghiêng, nhịp tim
Tốc độ: 0.8km/h-16km/h
Độ dốc:	0-15%
Tải trọng tối đa: 130kg
Chương trình mặc định: P1-P12
Chức năng: MP3, đo nhịp tim, calo, quãng đường, thời gian.
Thời gian bảo hành: 03 năm
Trân trọng giới thiệu.', N'mcb_05.png', 1, '04/10/2021', 16)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (6, N'Máy chạy bộ Poongsan TMP-900', 11100000,  N'Máy tập chạy bộ Poongsan TMP-900 – Thiết bị chăm sóc sức khỏe cho mọi nhà
Thông số kỹ thuật
Model: TMP-900-Athena-03
Điện nguồn: 240V
Công suất: 1500W
Tần số: 50Hz
Thành phần: Sản phẩm sử dụng điện
Trọng lượng tịnh: 75,2 kg
Trọng lượng tối đa: 120 kg
Kích thước: 166,8 x 71,5 x 128 cm
Thời gian bảo hành: 1 năm
Xin trân trọng giới thiệu cùng các bạn.', N'mcb_06.png', 1, '10/12/2020', 65)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (7, N'Bộ tạ tay đa năng điều chỉnh', 600000, N'Bộ tạ tay đa năng điều chỉnh thiết kế có thể tháo lắp dễ dàng thành tạ tay hoặc tạ đòn
THÔNG SỐ KỸ THUẬT
Mã sản phẩm: BTDNDC
Màu sắc: Đen - Vàng
Chất liệu: Bê tông, nhựa ABS
Loại tạ: Tháo lắp
Trọng lượng: 20, 30, 40kg
Xin trân trọng giới thiệu cùng các bạn.', N'sp1.png', 2, CAST(0x95450000 AS SmallDateTime), 51)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (8, N'Tạ Tay Điều Chỉnh Bowflex 1090', 61000, N'Tạ tay điều chỉnh Bowflex 1090 hay còn gọi là Bowflex Selecttech 1090 là một sự đột phát trong công nghệ thiết kế, vì nó giúp cho  người dùng dễ dàng thay đổi trọng lượng tạ tập bằng cách điều chỉnh đến mức tạ cần chọn ở 2 đầu tạ.
Thông số kỹ thuật tạ tay điều chỉnh Bowflex 1090:
Thương hiệu: Bowflex.
Công nghệ: USA.
Xuất xứ: Trung Quốc.
Màu sắc: đen.
Kích thước mỗi quả: 40 x 20 x 23 cm (dài x rộng x cao).
Kích thước cả khay tạ : 45 x 25 x 25 cm
Trọng lượng 1 bên tạ : 40,8kg
Xin trân trọng giới thiệu. ', N'shop6.jpg', 2, '01/10/2021', 15)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (9, N'Tạ tay liền đòn ', 525000, N'Tạ được làm từ sắt có bọc cao su chống trơn trượt và tăng độ an toàn khi tập luyện.
Thông số kỹ thuật:
Trọng lượng tạ: 10kg, 12kg, 14kg, 16kg, 18kg, 20kg, 22kg
Màu sắc: Đen
Sản xuất tại: Trung Quốc
Xuất xứ thương hiệu: Đài Loan
Xin trân trọng giới thiệu.', N'sp8.jpg', 2, '04/12/2018', 19)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (10, N'Tạ Tập Gym Hình Lục Giác', 98000, N'TẠ TAY TẬP THỂ HÌNH màu đen thiết kế hình đa giác chống trơn,trượt, thiết kế bằng nhựa cao cấp đảm bảo chất lượng siêu bền và đẹp, hàng loại 1 đẹp duy nhất trên thị trường, hàng độc quyền mẫu mới nhấtCó 2 loại:1 - Vỏ tạ chưa nhồi: giá thành rẻ hơn,Tức là vỏ kèm theo nắp đạy, mọi người mua về tự nhồi nguyên vật liệu như đổ bê tông xi măng, sỏi cát, sắt vào để đủ cân tập nhéLưu ý: với những vỏ 7,8,9,10 kích thước chỉ nhích hơn nhau chút, gần như bằng nhau sản xuất từ nhà máy như vậy để tay cầm quả tạ dễ cầm thuận tiện cho người tập, nên vỏ 7kg, 8kg thì cho vật liệu:nhào, trộn nước, cát đen, xi măng,sỏi vào là xong, nhưng riêng với vỏ 9,10 thì dùng vật liệu: nhào trộn nước, cát vàng, xi măng,sỏi, thêm 1 thanh sắt đặc vào giữa tay cầm giúp chiếc tạ đủ cân và đảm bảo độ bền chắc chán cho tạ tránh rủi ro rơi từ trên cao bị gãy.2 - Loại Tạ đã được nhồi sẵn là đã có đủ nguyên vật liệu đủ cân rồi chỉ việc tậpTạ Màu Đen từ 1kg, 1,5kg -10kg thiết kế hình đa giác chống trơn,trượt, thiết kế bằng nhựa cao cấp đảm bảo chất lượng siêu bền và đẹp, hàng loại 1 đẹp duy nhất trên thị trường:các bạn mua vỏ về nhồi xi măng, và cát vào để im 1-2 ngày là có thể sử dụng đượcGia công chắc chắn, bền, đẹp.Kèm thêm nút đạy.
Xin trân trọng giới thiệu cùng bạn. ', N'tadon1.png', 2, CAST(0x93F90000 AS SmallDateTime), 8)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (11, N'Xà đơn gắn cửa', 161000, N'Xà đơn gắn cửa trên thị trường có rất khá nhiều phiên bản với cấu tạo và giá tiền khác nhau một chút. Sau đây T SPORT sẽ giới thiệu các mẫu xà đơn gắn cửa tốt nhất và bán chạy nhất hiện nay.
Thông tin chi tiết xà đơn gắn cửa:
Chất liệu: Inox có trục thép hổ trợ bên trong
Màu sắc: Đen, xanh, nâu và ghi
Đệm giữ bằng silicon siêu bám
Đệm mút phủ toàn thân
Kích thước: có thể điều chỉnh
Chịu tải tối đa: 130kg', N'sp7.jpg', 3,'02/12/2020', 24)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (12, N'Xà Đơn Đa Năng Treo Tường Kết Hợp Xà Kép Di Động Tải Trọng 300kg – Wall Pull Up Bar Tập GYM Tại Nhà', 521000, N'Xà đơn xà kép treo tường 8 IN 1 là mẫu dụng cụ thể hình đa năng hỗ trợ nhiều bài tập như hít xà đơn, tập xà kép, tập chống đẩy, tập cơ bụng…. Ưu điểm của bộ xà đơn xà kép này là tính năng tháo rời khi không sử dụng, giúp không gian nhà bạn không bị chiếm dụng gây vướng víu. Bộ xà đơn xà kép này được treo lên 4 móc sắt chịu lực cao khoan vào tường.
THÔNG TIN SẢN PHẨM XÀ ĐƠN TREO TƯỜNG ĐA NĂNG:
Trọng lượng xà đơn treo tường đa năng: ~ 11kg.
Kích thước lắp đặt: Dài x rộng x cao: ~ 95cm x 77cm x 47cm.
Màu sắc: Màu đen.
Chất liệu: Thép chống gỉ.
Tải trọng: ~300kg
Thương hiệu: Welike.', N'detail2.jpg', 3,'04/02/2021', 19)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (13, N'Xà Kép Tiêu Chuẩn', 1850000, N'Sản phẩm chuyên dùng để tập các nhóm cơ chính gồm cơ ngực dưới, sau là cơ ngực trên, ngực giữa, cơ tam đầu bắp tay sau, cơ vai và cơ lưng trên.
THÔNG TIN SẢN PHẨM
Xuất xứ: Việt Nam.
Xà kép ngoài trời được làm từ chất liệu thép cao cấp, cực dày và sơn tĩnh điện chất lượng cao.
Phần đế của xà kép sử dụng thép U120, D60, D48 và sơn bằng công nghệ sơn epoxy. Phần tay xà sử dụng ống D42 mạ kẽm dài 3 m.
Kích thước xà kép khi lắp đặt là 1,5 x 3 m. Sản phẩm có thể thay đổi chiều cao tay xà từ 1,45 – 1,75 m và thay đổi chiều rộng tay xà từ 36 – 46 cm nên phù hợp với mọi đối tượng sử dụng.
Xà kép dùng để tập luyện ngoài trời, giúp phát triển các nhóm cơ vai, tay, hông, bụng..
Bạn có thể tháo rời để vận chuyển sản phẩm rất dễ dàng.
Xà kép ngoài trời thích hợp sử dụng cho cơ quan, trường học, công viên…
Bảo hành: 12 tháng.', N'shop8.jpg', 3, '02/10/2016', 17)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (14, N'XÀ ĐƠN XÀ KÉP MP-320', 1221000, N'Xà đơn xà kép MP-320 là thiết bị tập gym được sử dụng hỗ trợ nhiều bài tập như tập cơ bụng, tập cơ ngực,… Được dùng cho phòng tập thể hình hoặc gia đình. Sản phẩm làm bằng ống thép to khỏe, chắc chắn. Chân đế vững chãi, ổn định khi tập luyện. Đảm bảo an toàn cho người dùng. 
Thông tin sản phẩm xà đơn xà kép MP-320
Xuất xứ: Việt Nam
Khung làm bằng thép hộp chịu lực 50x100mm
Lớp đệm lưng, tay bằng mút dày 50mm, bọc da PU.
Tập luyện các nhóm cơ ngực, tay , xô…
Sơn tĩnh điện chống gỉ
Kích thước: 115 x 117 x 162 cm
Sản phẩm chuyên dụng cho các phòng tập', N'xadonxakep1.png', 3, '04/12/2019', 28)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (15, N'Xà kép ngoài trời HQ-623', 1223000, N'Thông tin xà kép ngoài trời HQ-623.
Mã sản phẩm: HQ-623.
Đơn vị nhập khẩu: Thiên Trường Sport.
Xuất xứ: Trung Quốc.
Xà kép ngoài trời HQ-623 được thiết kế chắc chắn, hỗ trợ các bài tập xà kép hiệu quả và chuyên dùng lắp đặt cho công viên ngoài trời.
Trụ chính của xà làm từ thép tròn phi 114, dày 3.00mm và được sơn chống rỉ.
Ống phụ làm từ thép tròn phi 60 dày 2.75mm và được sơn chống rỉ.
Màu sắc: xanh.
Trọng lượng: kg.
Diện tích lắp đặt: 240 x 59 x 135 cm.
Cách tập: Nắm chắc hai tay vào thanh ngang, nhấc cả thân người lên rồi hạ xuống. Lặp lại động tác để tiếp tục bài tập.
Bảo hành: 1 năm.
Trân trọng giới thiệu.', N'xadonxakep2.png', 3, '04/29/2020', 24)
INSERT [dbo].[SANPHAM] ([MaSanPham], [TenSanPham], [GiaBan], [Mota], [AnhBia], [MaLoai], [Ngaycapnhat], [SoLuongBan]) VALUES (16, N'Xà kép Mini đa năng HM2612, xà kép đa năng tập gym tại nhà giá rẻ', 180000, N'Sản phẩm xà kép Mini đa năng HM2612 được làm nhỏ gọn, chắc chắn và phù hợp sử dụng để tập thể dục, tập Gym tại nhà.
Xuất xứ: Trung Quốc.
Thiết kế chi tiết của xà kép Mini gồm:
Khung xà được làm từ thép ống dày và sơn tĩnh điện chống rỉ sét, bong tróc.
Chân đế xà được bọc nhựa giúp tăng độ ma sát và vị trí tay cầm tập bọc mút cực êm.
Xà kép có chức năng tháo rời và lắp lại dễ dàng.
Màu sắc: ghi xám hoặc vàng.
Kích thước lắp 1 bên sản phẩm: 60 x 38 x 77 cm (dài x rộng x cao).
Kích thước đóng thùng: 74 x 62 x 10 cm.
Trọng lượng xà kép Mini: 10 kg.
Trọng lượng đóng thùng: 11 kg.
Trọng lượng người tập tối đa: 120 kg.
Xà kép Mini đa năng HM2612 hỗ trợ tập Gym, tập thể lực tại nhà hiệu quả với các bài tập cơ bản gồm: Tập xà kép, ke bụng, tập cơ tay, tập chống đẩy.
Trân trọng giới thiệu. ', N'TH001.jpg', 3, '07/29/2020', 8)

SET IDENTITY_INSERT [dbo].[SANPHAM] OFF

SET IDENTITY_INSERT [dbo].[KHACHHANG] ON
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (1, N'Võ Lê Minh Phương', N'Phú Hòa', N'0378256319', N'vlmphuong', N'12345', '2003/05/19', N'2124802010356@student.tdmu.edu.vn')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (2, N'Huỳnh Nhựt Linh', N'Phú Hòa', N'0383404293', N'linhdz', N'12345', '2003/07/24', N'2124802010182@student.tdmu.edu.vn')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (3, N'Cao Hữu Cường', N'Phú Hòa', N'0344567795', N'caocuong', N'12345', '03/03/2003', N'caocuong@gmail.com')
SET IDENTITY_INSERT [dbo].[KHACHHANG] OFF

SET IDENTITY_INSERT [dbo].[DONDATHANG] ON
INSERT [dbo].[DONDATHANG] ([MaDonHang], [MaKH], [TinhTrangGiaoHang], [Ngaydat], [Ngaygiao]) VALUES (1, 1, 4, '05/10/2021','05/12/2021')
INSERT [dbo].[DONDATHANG] ([MaDonHang], [MaKH], [TinhTrangGiaoHang], [Ngaydat], [Ngaygiao]) VALUES (3, 2, 2, '05/07/2021','05/11/2021')
SET IDENTITY_INSERT [dbo].[DONDATHANG] OFF

INSERT [dbo].[CHITIETDATHANG] ([MaDonHang], [MaSanPham], [SoLuong], [DonGia]) VALUES (1, 9, 1, 525000)
INSERT [dbo].[CHITIETDATHANG] ([MaDonHang], [MaSanPham], [SoLuong], [DonGia]) VALUES (1, 8, 2, 61000)
INSERT [dbo].[CHITIETDATHANG] ([MaDonHang], [MaSanPham], [SoLuong], [DonGia]) VALUES (3, 12, 1, 521000)
INSERT [dbo].[CHITIETDATHANG] ([MaDonHang], [MaSanPham], [SoLuong], [DonGia]) VALUES (3, 11, 3, 161000)

SET IDENTITY_INSERT [dbo].[NHACUNGCAP] ON
INSERT [dbo].[NHACUNGCAP] ([MaNCC], [TenNCC], [Diachi], [Dienthoai]) VALUES (1, N'Võ Lê Minh Phương', N'424/77 Lê Hồng Phong', N'0378256319')
INSERT [dbo].[NHACUNGCAP] ([MaNCC], [TenNCC], [Diachi], [Dienthoai]) VALUES (2, N'Huỳnh Nhựt Linh', N'', N'')
INSERT [dbo].[NHACUNGCAP] ([MaNCC], [TenNCC], [Diachi], [Dienthoai]) VALUES (3, N'Ngô Gia Lâm', N'', N'')
INSERT [dbo].[NHACUNGCAP] ([MaNCC], [TenNCC], [Diachi], [Dienthoai]) VALUES (4, N'Cao Hữu Cường', NULL, NULL)
INSERT [dbo].[NHACUNGCAP] ([MaNCC], [TenNCC], [Diachi], [Dienthoai]) VALUES (5, N'Nguyễn Huỳnh Huy Hoàng', NULL, NULL)
SET IDENTITY_INSERT [dbo].[NHACUNGCAP] OFF

INSERT [dbo].[VIETSANPHAM] ([MaSanPham], [MaNCC], [Vaitro], [ViTri]) VALUES (1, 1, N'Khách Hàng', N'')
INSERT [dbo].[VIETSANPHAM] ([MaSanPham], [MaNCC], [Vaitro], [ViTri]) VALUES (2, 2, N'Khách Hàng',N'')
INSERT [dbo].[VIETSANPHAM] ([MaSanPham], [MaNCC], [Vaitro], [ViTri]) VALUES (3, 3, N'Khách Hàng',N'')
INSERT [dbo].[VIETSANPHAM] ([MaSanPham], [MaNCC], [Vaitro], [ViTri]) VALUES (4, 4, N'Khách Hàng',N'')
INSERT [dbo].[VIETSANPHAM] ([MaSanPham], [MaNCC], [Vaitro], [ViTri]) VALUES (5, 5, N'Khách Hàng',N'')

CREATE TABLE ADMIN
(
	MaAd INT IDENTITY(1,1),
	HoTen NVARCHAR(50),
	DienThoai VARCHAR(10),
	TenDN VARCHAR(15),
	MatKhau VARCHAR(15),
	Quyen Int Default 1,
	CONSTRAINT PK_AM PRIMARY KEY(MaAd)
)
GO

SET IDENTITY_INSERT [dbo].[ADMIN] ON
INSERT [dbo].[ADMIN] ([MaAd], [HoTen], [DienThoai], [TenDN], [MatKhau], [Quyen]) VALUES (1, N'Võ Lê Minh Phương', N'0378256319', N'vlmphuong', N'12345', 1)
INSERT [dbo].[ADMIN] ([MaAd], [HoTen], [DienThoai], [TenDN], [MatKhau], [Quyen]) VALUES (2, N'Huỳnh Nhựt Linh', N'0383404293', N'linhdz', N'12345', 1)
INSERT [dbo].[ADMIN] ([MaAd], [HoTen], [DienThoai], [TenDN], [MatKhau], [Quyen]) VALUES (3, N'Cao Hữu Cường', N'0344567795', N'caocuong', N'12345', 2)
SET IDENTITY_INSERT [dbo].[ADMIN] OFF

SET IDENTITY_INSERT [dbo].[TRANGTIN] ON
INSERT [dbo].[TRANGTIN] ([MaTT], [TenTrang], [NoiDung], [NgayTao], [MetaTitle]) VALUES (1, N'GIỚI THIỆU', N'Đây là nội dung trang giới thiệu', '04/12/2021', N'gioi-thieu')
INSERT [dbo].[TRANGTIN] ([MaTT], [TenTrang], [NoiDung], [NgayTao], [MetaTitle]) VALUES (2, N'LIÊN HỆ', N'Đây là nội dung trang liên hệ', '04/10/2022', N'lien-he')
SET IDENTITY_INSERT [dbo].[TRANGTIN] OFF

SET IDENTITY_INSERT [dbo].MENU ON
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (1, N'TRANG CHỦ', N'SachOnline/Index', NULL, 1)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (2, N'GIỚI THIỆU', N'gioi-thieu', NULL, 2)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (11, N'LIÊN HỆ', N'lien-he', NULL, 5)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (3, N'TIN TỨC', N'tin-tuc', NULL, 3)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (4, N'DANH MỤC', N'danh-muc', NULL, 4)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (5, N'Tin tức 1', N'danh-muc-1', 3, 1)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (6, N'Tin tức 2', N'danh-muc-2', 3, 2)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (7, N'Tin tức 3', N'danh-muc-3', 3, 3)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (9, N'Tin tức 2-1', N'tin-tuc-2-1', 6, 1)
INSERT [dbo].[MENU] ([Id], [MenuName], [MenuLink], [ParentId], [OrderNumber]) VALUES (10, N'Tin tức 2-2', N'tin-tuc-2-2', 6, 2)
SET IDENTITY_INSERT [dbo].MENU OFF