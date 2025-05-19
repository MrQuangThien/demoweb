-- Xóa database nếu tồn tại
USE master;
GO
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = 'QLCHOTO')
BEGIN
    ALTER DATABASE QLCHOTO SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLCHOTO;
END
GO

-- Tạo database
CREATE DATABASE QLCHOTO;
GO

-- Sử dụng database
USE QLCHOTO;
GO
ALTER DATABASE QLCHOTO SET MULTI_USER;
GO

-- Bảng sản phẩm
CREATE TABLE SANPHAM (
    IDSP INT PRIMARY KEY,
    TenSP NVARCHAR(100) NOT NULL,
    NgaySanXuat DATETIME NULL,
    LoaiXe NVARCHAR(100) NULL,
    HangXe NVARCHAR(20) NULL,
    GiaBan DECIMAL(18,2) NULL,
    SoLuong INT NULL,
    HinhAnh VARBINARY(MAX) NULL
);
GO
DECLARE @id INT;
SET @id = 1; -- Hoặc giá trị cần thiết

-- Cập nhật cột HINHANH bằng chuỗi văn bản chuyển sang kiểu dữ liệu varbinary
UPDATE SANPHAM
SET HINHANH = CONVERT(varbinary(MAX), N'Mô tả sản phẩm')
WHERE IDSP = @id;

-- Tạo bảng THONGTIN với IDKH tự tăng
CREATE TABLE THONGTIN (
    IDKH INT IDENTITY(1,1) PRIMARY KEY, -- tự tăng bắt đầu từ 1, bước 1
    HoTen NVARCHAR(100) NOT NULL,
    NgaySinh DATETIME NULL,
    GioiTinh CHAR(3) CONSTRAINT TT_GTinh_CK CHECK (GioiTinh IN (N'Nam', N'Nu')),
    SDT NVARCHAR(20) NULL,
    DiaChi NVARCHAR(100) NULL,
    Gmail NVARCHAR(100) NULL
);
GO

-- Xóa ràng buộc khóa ngoại từ TAIKHOAN


-- Bảng nhân viên
CREATE TABLE NHANVIEN (
    IDNV INT PRIMARY KEY,
    HoTen NVARCHAR(100) NOT NULL,
    SDT NVARCHAR(10) NULL,
    Gmail NVARCHAR(100) NULL,
    ChucVu NVARCHAR(1000) NULL
);
GO

-- Bảng hóa đơn
CREATE TABLE HOADON (
    IDHD INT PRIMARY KEY,
    IDKH INT NOT NULL,
    IDNV INT NOT NULL,
    NgayMua DATETIME NULL,
    ThanhTien DECIMAL(18,2) NULL,
    SoLuong INT NULL,
    FOREIGN KEY (IDKH) REFERENCES THONGTIN(IDKH),
    FOREIGN KEY (IDNV) REFERENCES NHANVIEN(IDNV)
);
GO

-- Chi tiết đơn hàng
CREATE TABLE CHITIETDONHANG (
    IDCTDH INT PRIMARY KEY,
    IDHD INT NOT NULL,
    IDSP INT NOT NULL,
    DonGia DECIMAL(18,2) NULL,
    ThanhTien DECIMAL(18,2) NULL,
    SoLuong INT NULL,
    FOREIGN KEY (IDHD) REFERENCES HOADON(IDHD),
    FOREIGN KEY (IDSP) REFERENCES SANPHAM(IDSP)
);
GO

-- Tài khoản
CREATE TABLE TAIKHOAN (
   IDTK INT IDENTITY(1,1) PRIMARY KEY,
    IDKH INT NOT NULL,
    TenTK NVARCHAR(100) NOT NULL,
    MatKhau NVARCHAR(100) NOT NULL,
    Gmail NVARCHAR(100) NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT N'user',
    FOREIGN KEY (IDKH) REFERENCES THONGTIN(IDKH)
);
GO

-- Đánh giá
CREATE TABLE DANHGIA (
    IDDG INT PRIMARY KEY,
    IDKH INT NOT NULL,
    IDSP INT NOT NULL,
    ThangDiem INT NULL,
    FeedBack NVARCHAR(1000) NULL,
    NgayFB DATETIME NULL,
    FOREIGN KEY (IDKH) REFERENCES THONGTIN(IDKH),
    FOREIGN KEY (IDSP) REFERENCES SANPHAM(IDSP)
);
GO

-- Bảo hành
CREATE TABLE BAOHANH (
    IDBH INT PRIMARY KEY,
    IDSP INT NOT NULL,
    NgayMua DATETIME NULL,
    NgayHHBH DATETIME NULL,
    NoiDung NVARCHAR(1000) NULL,
    FOREIGN KEY (IDSP) REFERENCES SANPHAM(IDSP)
);
GO

-- Thông báo
CREATE TABLE ThongBao (
    IDTB INT PRIMARY KEY,
    TenTB CHAR(10) NULL,
    NoiDungTB NVARCHAR(1000) NULL,
    NgayDang DATETIME NULL,
    TinhTrang NVARCHAR(100) NULL,
    IDNV INT NULL,
    IDKH INT NULL,
    FOREIGN KEY (IDNV) REFERENCES NHANVIEN(IDNV),
    FOREIGN KEY (IDKH) REFERENCES THONGTIN(IDKH)
);
GO

-- Lịch sử đăng nhập
CREATE TABLE LOG_DANGNHAP (
    IDLog INT IDENTITY(1,1) PRIMARY KEY, -- thêm IDENTITY tự tăng
    IDTK INT NOT NULL,
    ThoiGian DATETIME2 NULL,
    IP VARCHAR(50) NULL,
    ThanhCong BIT NULL,
    FOREIGN KEY (IDTK) REFERENCES TAIKHOAN(IDTK)
);


-- Thống kê số lượng bán
CREATE TABLE THONGKE_SLBAN (
    IDSP INT NOT NULL,
    SoLuongBan INT NULL,
    ThoiGian DATETIME NOT NULL,
    PRIMARY KEY (IDSP, ThoiGian),
    FOREIGN KEY (IDSP) REFERENCES SANPHAM(IDSP)
);
GO
-- dang nhappp
CREATE TABLE Dangnhap (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL
);
ALTER TABLE Dangnhap
ADD CONSTRAINT UQ_Dangnhap_Username UNIQUE (Username);
INSERT INTO Dangnhap(Username, Password)
VALUES (N'admin', N'admin123'); -- Có thể mã hóa mật khẩu nếu muốn bảo mật




   
-- Khách hàng
--INSERT INTO THONGTIN ( HoTen, NgaySinh, GioiTinh, SDT, DiaChi, Gmail) VALUES
--( N'Nguyễn Văn Quyết', '1990-05-10', N'Nam', '0857540568', N'Hà Nội', 'Quyet991@gmail.com'),
--( N'Lê Thị Hà', '1988-09-15', N'Nu', '01278101108', N'Hải Phòng', 'ha213@gmail.com'),
--( N'Trần Văn Lễ', '1995-01-20', N'Nam', '0385050875', N'HCM', 'Vanle912@gmail.com'),
--(N'Phạm Thị Duyên', '1992-03-03', N'Nu', '01247578875', N'Đà Nẵng', 'Duyen23@gmail.com'),
--( N'Hoàng Văn Quyết', '1985-07-07', N'Nam', '0905056568', N'Cần Thơ', 'hoangQuyet123@gmail.com'),
--( N'Lê Văn Luyện', '1993-08-22', N'Nam', '0856677889', N'Bình Dương', 'VLuyena12@gmail.com'),
--( N'Doãn Văn Hoàng', '1987-12-12', N'Nam', '0858899001', N'Huế', 'hoangem12@gmail.com'),
----( N'Vũ Thị Yến', '1996-06-18', N'Nu', '0989900112', N'Biên Hòa', 'Yen1231@gmail.com'),
--SSS( N'Bùi Văn Quyết', '1994-02-28', N'Nam', '0380011223', N'Thanh Hóa', 'buiquyet011@gmail.com');


-- Tài khoản
--INSERT INTO TAIKHOAN (IDTK, IDKH, TenTK, MatKhau, Gmail, Role) VALUES
--(2, 2, N'nguyenvanquyet', N'vanquyet911@', 'Quyet991@gmail.com', N'user'),
--(3, 3, N'lethiha', N'1212@12ha', 'ha213@gmail.com', N'user'),
--(4, 4, N'tranvanle', N'tranle@12', 'Vanle912@gmail.com', N'user'),
--(5, 5, N'phamthiduyen', N'duyen991@1', 'Duyen23@gmail.com', N'user'),
--(6, 6, N'hoangvanquyet', N'ha001@112', 'hoangQuyet123@gmail.com', N'user'),
--(7, 7, N'levanluyen', N'1adbc@123', 'VLuyena12@gmail.com', N'user'),
--(8, 8, N'ngothigiang', N'giang@122', 'giang911@gmail.com', N'user'),
--(9, 9, N'doanvanhoang', N'honag999@', 'hoangem12@gmail.com', N'user'),
--(10, 10, N'vuthiyen', N'passs@11', 'Yen1231@gmail.com', N'user'),
--(11, 11, N'buivanquyet', N'quyet2@112', 'buiquyet011@gmail.com', N'user');
/* 1. Thêm bản ghi vào THONGTIN – IDKH sẽ tự tăng */


/* 2. Lấy IDKH vừa sinh ra */
DECLARE @AdminID int = SCOPE_IDENTITY();

/* 3. Thêm tài khoản quản trị */
INSERT INTO TAIKHOAN (IDKH, TenTK, MatKhau, Gmail, Role)
VALUES (@AdminID, N'admin', N'admin123', N'admin@example.com', N'admin');



-- Lịch sử đăng nhập
INSERT INTO LOG_DANGNHAP (IDLog, IDTK, ThoiGian, IP, ThanhCong) VALUES
(2, 2, '2024-05-02 09:00:00', '192.168.1.2', 1),
(3, 3, '2024-05-03 10:15:00', '192.168.1.3', 0),
(4, 4, '2024-05-04 11:00:00', '192.168.1.4', 1),
(5, 5, '2024-05-05 12:45:00', '192.168.1.5', 1),
(6, 6, '2024-05-06 13:30:00', '192.168.1.6', 0),
(7, 7, '2024-05-07 14:20:00', '192.168.1.7', 1),
(8, 8, '2024-05-08 15:10:00', '192.168.1.8', 1),
(9, 9, '2024-05-09 16:00:00', '192.168.1.9', 1),
(10, 10, '2024-05-10 16:55:00', '192.168.1.10', 1),
(11, 11, '2024-05-11 17:40:00', '192.168.1.11', 0);


-- Nhân viên
INSERT INTO NHANVIEN (IDNV, HoTen, SDT, Gmail, ChucVu) VALUES
(1, N'Nguyễn Văn Quản', '0381286437', 'vanquan991@gmail.com', N'Quản lý'),
(2, N'Trần Thị Bán', '0382344938', 'tranthinban@gmail.com', N'Nhân viên bán hàng'),
(3, N'Lê Văn Toán', '0853439789', 'ketoan@gmail.com', N'Nhân viên bán hàng'),
(4, N'Phạm Thị Kỹ', '0855655901', 'phamthiky77@gmial.com', N'Nhân viên kỹ thuật'),
(5, N'Hoàng Văn Bánh', '0945678901', 'hoangvanbanh991@gmail.com', N'Nhân viên bán hàng'),
(6, N'Đỗ Thị Linh', '0386789012', 'dolin188@gmail.com', N'Trưởng phòng'),
(7, N'Bùi Văn Luyện', '0857890163', 'vanluyenbui123@gmail.com',  N'Nhân viên bán hàng'),
(8, N'Ngô Thị Ánh', '0388901241', 'ngoanhthi12@gmail.com', N'Nhân viên chăm sóc KH'),
(9, N'Vũ Văn Long', '0989012821', 'vuvanlong91@gmail.com', N'Nhân viên kỹ thuật'),
(10, N'Đặng Thị Toán', '0999831521', 'dangtian2@gmail.com', N'Kế toán');


-- Sản phẩm

INSERT INTO SANPHAM (IDSP, TenSP, NgaySanXuat, LoaiXe, HangXe, GiaBan, SoLuong, HinhAnh) VALUES
(1, N'Ford Ranger Raptor', '2023-01-15', N'Bán tải', N'Ford', 360000000, 5, NULL),
(2, N'Ford Everest', '2022-12-10', N'SUV', N'Ford', 350000000, 3, NULL),
(3, N'Lamborghini Aventador', '2023-05-20', N'Supercar', N'Lamborghini', 50400000000, 1, NULL),
(4, N'Lamborghini Urus', '2023-03-10', N'SUV', N'Lamborghini', 50522000000, 2, NULL),
(5, N'Ferrari 488 GTB', '2023-06-01', N'Supercar', N'Ferrari', 40238000000, 1, NULL),
(6, N'Ferrari Roma', '2023-07-12', N'Sport', N'Ferrari', 50527000000, 1, NULL),
(7, N'Toyota Camry', '2022-10-05', N'Sedan', N'Toyota', 440000000, 10, NULL),
(8, N'Toyota Corolla Altis', '2022-08-18', N'Sedan', N'Toyota', 523000000, 8, NULL),
(9, N'Toyota Hilux', '2023-02-22', N'Bán tải', N'Toyota', 390000000, 6, NULL),
(10, N'Toyota Fortuner', '2023-04-15', N'SUV', N'Toyota', 1126000000, 4, NULL),
(11, N'Hyundai Tucson', '2023-06-01', N'SUV', N'Hyundai', 950000000, 3, NULL),
(12, N'Kia Seltos', '2023-07-10', N'Crossover', N'Kia', 780000000, 5, NULL),
(13, N'Mazda CX-5', '2023-08-15', N'SUV', N'Mazda', 900000000, 4, NULL);


-- Hóa đơn
INSERT INTO HOADON (IDHD, IDKH, IDNV, NgayMua, ThanhTien, SoLuong) VALUES
(3, 2, 1, '2024-05-02', 360000000, 1),
(4, 3, 2, '2024-05-03', 350000000, 1),
(5, 4, 1, '2024-05-04', 400000000, 1),
(6, 5, 2, '2024-05-05', 522000000, 1),
(7, 6, 1, '2024-05-06', 476000000, 2),
(8, 7, 2, '2024-05-07', 527000000, 1),
(9, 8, 1, '2024-05-08', 880000000, 2),
(10, 9, 2, '2024-05-09', 523000000, 1),
(11, 10, 1, '2024-05-10', 390000000, 1),
(12, 11, 2, '2024-05-11', 2252000000, 2);


-- Chi tiết hóa đơn

-- CHITIETDONHANG: Sửa cho khớp với HOADON
INSERT INTO CHITIETDONHANG (IDCTDH, IDHD, IDSP, DonGia, SoLuong, ThanhTien) VALUES
(5, 3, 4, 360000000, 1, 360000000),
(6, 4, 5, 350000000, 1, 350000000),
(7, 5, 6, 400000000, 1, 400000000),
(8, 6, 7, 522000000, 1, 522000000),
(9, 7, 8, 238000000, 2, 476000000),
(10, 8, 9, 527000000, 1, 527000000),
(11, 9, 10, 440000000, 2, 880000000),
(12, 10, 11, 523000000, 1, 523000000),
(13, 11, 12, 390000000, 1, 390000000),
(14, 12, 13, 1126000000, 2, 2252000000);

-- Đánh giá
INSERT INTO DANHGIA (IDDG, IDKH, IDSP, ThangDiem, FeedBack, NgayFB) VALUES
(3, 2, 4, 5, N'Rất hài lòng với xe', '2024-05-02'),
(4, 3, 5, 4, N'Giá hợp lý', '2024-05-03'),
(5, 4, 6, 3, N'Tạm ổn', '2024-05-04'),
(6, 5, 7, 5, N'Xe điện chạy êm', '2024-05-05'),
(7, 6, 8, 4, N'Ngoại hình đẹp', '2024-05-06'),
(8, 7, 9, 2, N'Không hài lòng lắm', '2024-05-07'),
(9, 8, 10, 5, N'Máy mạnh, êm', '2024-05-08'),
(10, 9, 11, 4, N'Đáng tiền', '2024-05-09'),
(11, 10, 12, 5, N'Rất tốt', '2024-05-10'),
(12, 11, 13, 3, N'Ổn định, nên cải tiến thêm', '2024-05-11');


-- Bảo hành
INSERT INTO BAOHANH (IDBH, IDSP, NgayMua, NgayHHBH, NoiDung) VALUES
(3, 4, '2024-05-02', '2025-05-02', N'Bảo hành động cơ'),
(4, 5, '2024-05-03', '2025-05-03', N'Bảo hành điện'),
(5, 6, '2024-05-04', '2025-05-04', N'Bảo hành khung xe'),
(6, 7, '2024-05-05', '2025-05-05', N'Bảo hành pin'),
(7, 8, '2024-05-06', '2025-05-06', N'Bảo hành hộp số'),
(8, 9, '2024-05-07', '2025-05-07', N'Bảo hành bánh xe'),
(9, 10, '2024-05-08', '2025-05-08', N'Bảo hành đèn xe'),
(10, 11, '2024-05-09', '2025-05-09', N'Bảo hành phanh'),
(11, 12, '2024-05-10', '2025-05-10', N'Bảo hành toàn bộ'),
(12, 13, '2024-05-11', '2025-05-11', N'Bảo hành tay lái');

-- Thông báo
INSERT INTO ThongBao (IDTB, TenTB, NoiDungTB, NgayDang, TinhTrang, IDNV, IDKH) VALUES
(3, N'KM1', N'Giảm 10% cho xe ford', '2024-05-02', N'Đã đọc', 2, 2),
(4, N'BHV', N'Xe Ford được bảo hành', '2024-05-03', N'Chưa đọc', 2, 2),
(5, N'KM2', N'Giảm 5% dịp lễ', '2024-05-04', N'Đã đọc', 5, 3),
(6, N'TBSC', N'Sửa chữa xe xong', '2024-05-05', N'Chưa đọc', 2, 4),
(8, N'TBGG', N'Giảm giá xe số', '2024-05-07', N'Chưa đọc', 5, 5),
(9, N'KM4', N'Tặng quà khi mua xe', '2024-05-08', N'Đã đọc', 7, 6),
(10, N'TBSC2', N'Sửa phanh hoàn tất', '2024-05-09', N'Chưa đọc', 2, 3),
(11, N'TBST', N'Thanh toán thành công', '2024-05-10', N'Đã đọc', 7, 7),
(12, N'TBKM', N'Khuyến mãi lớn 5/2024', '2024-05-11', N'Chưa đọc', 5, 8);

-- Thống kê bán
INSERT INTO THONGKE_SLBAN (IDSP, SoLuongBan, ThoiGian) VALUES
(4, 1, '2024-05-02'),
(5, 1, '2024-05-03'),
(6, 1, '2024-05-04'),
(7, 1, '2024-05-05'),
(8, 2, '2024-05-06'),
(9, 1, '2024-05-07'),
(10, 2, '2024-05-08'),
(11, 1, '2024-05-09'),
(12, 1, '2024-05-10'),
(13, 2, '2024-05-11');



-- Kiểm tra dữ liệu
SELECT * FROM THONGTIN;
SELECT * FROM TAIKHOAN;
SELECT * FROM SANPHAM;
SELECT * FROM HOADON;
SELECT * FROM CHITIETDONHANG;
SELECT * FROM NHANVIEN;
SELECT * FROM DANHGIA;
SELECT * FROM BAOHANH;
SELECT * FROM ThongBao;
SELECT * FROM LOG_DANGNHAP;
SELECT * FROM THONGKE_SLBAN;
GO
sp_help THONGTIN;
