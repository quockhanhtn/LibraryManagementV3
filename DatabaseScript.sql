USE master
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'LibraryManagement')
BEGIN
	ALTER DATABASE [LibraryManagement] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
	DROP DATABASE IF EXISTS [LibraryManagement]
END
GO

CREATE DATABASE [LibraryManagement]
GO
USE [LibraryManagement]
GO


-----------------------------------------------------------
-----------Create table------------------------------------
-----------------------------------------------------------
DROP TABLE IF EXISTS [dbo].[User]
CREATE TABLE [dbo].[User](
	[UserId] BIGINT IDENTITY(1, 1),
	[FirstName] NVARCHAR(10) NULL,
	[LastName] NVARCHAR(30) NULL,
	[Gender] VARCHAR(1) NULL,						-- M = male, F = female, O = orther
	[DateOfBirth] DATE NOT NULL,
	[Ssn] VARCHAR(12) NULL,

	[Address] NVARCHAR(100) NULL,
	[PhoneNumber] VARCHAR(15) NULL,
	[Email] VARCHAR(50) NULL,

	[Username] VARCHAR(16) UNIQUE NOT NULL,
	[Password] VARCHAR(32) NOT NULL,
	[UserType] VARCHAR(15) NOT NULL DEFAULT 'MEMBER',	-- loại tài khoản admin = 'ADMIN', thủ thư = 'LIBRARIAN', độc giả = 'MEMBER', chưa xác thực thông tin = 'UN_VERIFIED'
	[UserStatus] BIT DEFAULT 1 NOT NULL,
	[Image] TEXT NULL
)
GO
ALTER TABLE [dbo].[User] ADD CONSTRAINT [PK_User] PRIMARY KEY([UserId])
GO


-- Set nullable unique for column email
CREATE UNIQUE NONCLUSTERED INDEX IDX_User_Email_Unique_Nullable ON [dbo].[User]([Email]) WHERE [Email] IS NOT NULL
CREATE UNIQUE NONCLUSTERED INDEX IDX_User_PhoneNumber_Unique_Nullable ON [dbo].[User]([PhoneNumber]) WHERE [PhoneNumber] IS NOT NULL
GO


-- tài khoản thủ thư
DROP TABLE IF EXISTS [dbo].[Librarian]
CREATE TABLE [dbo].[Librarian] (
	[UserId] BIGINT NOT NULL,

	[StartDate] DATE DEFAULT GETDATE() NOT NULL,
	[Salary] DECIMAL(19, 4) NULL,
)
GO
ALTER TABLE [dbo].[Librarian] ADD CONSTRAINT [pk_librarian] PRIMARY KEY ([UserId])
ALTER TABLE [dbo].[Librarian] ADD CONSTRAINT [fk_user_librarian] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId])
GO

-- tài khoản độc giả
DROP TABLE IF EXISTS [dbo].[Member]
CREATE TABLE [dbo].[Member] (
	[UserId] BIGINT NOT NULL,

	[RegisterDate] DATE DEFAULT GETDATE() NOT NULL,		-- ngày đăng ký
	[ExpDate] DATE DEFAULT GETDATE() NOT NULL,			-- ngày hết hạn
)
GO
ALTER TABLE [dbo].[Member] ADD CONSTRAINT [pk_member] PRIMARY KEY ([UserId])
ALTER TABLE [dbo].[Member] ADD CONSTRAINT [fk_user_member] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId])
GO

--tác giả của sách
DROP TABLE IF EXISTS [dbo].[Author]
CREATE TABLE [dbo].[Author]
(
	[AuthorId] BIGINT IDENTITY(1,1),
	[NickName] NVARCHAR(40) NOT NULL,	-- bút danh
	[RealName] NVARCHAR(50) NULL,		-- tên thật
	[AuthorStatus] BIT DEFAULT 1 NOT NULL,
)
GO
ALTER TABLE [dbo].[Author] ADD CONSTRAINT [pk_author] PRIMARY KEY ([AuthorId])
GO

-- nhà xuất bản
DROP TABLE IF EXISTS [dbo].[Publisher]
CREATE TABLE [dbo].[Publisher]
(
	[PublisherId] BIGINT IDENTITY(1, 1),
	[PublisherName] NVARCHAR(100) NOT NULL,

	[PhoneNumber] VARCHAR(15) NULL,
	[Address] NVARCHAR(100) NULL,
	[Email] VARCHAR(50) NULL,
	[Website] VARCHAR(40) NULL,

	[PublisherStatus] BIT DEFAULT 1 NOT NULL,
)
GO
ALTER TABLE [dbo].[Publisher] ADD CONSTRAINT [pk_publisher] PRIMARY KEY ([PublisherId])
GO

-- danh mục sách
DROP TABLE IF EXISTS [dbo].[BookCategory]
CREATE TABLE [dbo].[BookCategory]
(
	[BookCategoryId] BIGINT IDENTITY(1, 1),		
	[BookCategoryName] NVARCHAR(50) NULL,		-- tên chuyên mục
	[LimitDays] INT NULL,						-- số ngày cho mượn
	[BookCategoryStatus] BIT DEFAULT 1 NOT NULL
)
GO
ALTER TABLE [dbo].[BookCategory] ADD CONSTRAINT [pk_book_category] PRIMARY KEY([BookCategoryId])
GO


-- đầu sách
DROP TABLE IF EXISTS [dbo].[BookInfo]
CREATE TABLE [dbo].[BookInfo]
(
	[BookInfoId] BIGINT IDENTITY(1, 1),
	[Title] NVARCHAR(100) NULL,					-- tựa sách

	[BookCategoryId] BIGINT NULL,				-- mã chuyên mục
	[PublisherId] BIGINT NOT NULL,				-- mã nxb

	[YearPublished] INT NULL,					-- năm xb
	[PageNumber] INT NULL,						-- số trang
	[Size] VARCHAR(11) NULL,					-- kích thước
	[Price] DECIMAL(19, 0) NULL,				-- giá tiền

	[Count] INT NOT NULL CHECK([Count] > 0),	-- số lượng cuốn sách
	[BookInfoStatus] BIT DEFAULT 1 NULL,
	[Image] TEXT NULL
)
GO 
ALTER TABLE [dbo].[BookInfo] ADD CONSTRAINT [PK_BookInfo] PRIMARY KEY ([BookInfoId])
ALTER TABLE [dbo].[BookInfo] ADD CONSTRAINT [PK_BookInfo_BookCategory] FOREIGN KEY ([BookCategoryId]) REFERENCES [dbo].[BookCategory]([BookCategoryId])
ALTER TABLE [dbo].[BookInfo] ADD CONSTRAINT [PK_BookInfo_Publisher]  FOREIGN KEY([PublisherId]) REFERENCES [dbo].[Publisher]([PublisherId])
ALTER TABLE [dbo].[BookInfo] ADD CONSTRAINT [CK_BookInfo_Count] CHECK ([Count] > 0)
GO

-- cuốn sách
DROP TABLE IF EXISTS [dbo].[BookItem]
CREATE TABLE [dbo].[BookItem] (
	[BookItemId] VARCHAR(20) NOT NULL,
	[BookInfoId] BIGINT NOT NULL
)
GO
ALTER TABLE [dbo].[BookItem] ADD CONSTRAINT [pk_book_item] PRIMARY KEY ([BookItemId])
ALTER TABLE [dbo].[BookItem] ADD CONSTRAINT [fk_book_book_item] FOREIGN KEY ([BookInfoId]) REFERENCES [dbo].[BookInfo]([BookInfoId])
GO

-- quan hệ nhiều nhiều book - author
DROP TABLE IF EXISTS [dbo].[BookAuthor]
CREATE TABLE [dbo].[BookAuthor] (
	[BookInfoId] BIGINT NOT NULL,
	[AuthorId] BIGINT NOT NULL,
)
GO
ALTER TABLE [dbo].[BookAuthor] ADD CONSTRAINT [pk_book_author] PRIMARY KEY ([BookInfoId], [AuthorId])
ALTER TABLE [dbo].[BookAuthor] ADD CONSTRAINT [fk_book_author_book] FOREIGN KEY ([BookInfoId]) REFERENCES [dbo].[BookInfo]([BookInfoId])
ALTER TABLE [dbo].[BookAuthor] ADD CONSTRAINT [fk_book_author_author] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[Author]([AuthorId])
GO


-- mượn sách
DROP TABLE IF EXISTS [dbo].[Borrow]
CREATE TABLE [dbo].[Borrow]
(
	[BorrowId] BIGINT IDENTITY(1,1),
	[BookItemId] VARCHAR(20) NOT NULL,					-- mã cuốn sách

	[MemberId] BIGINT NOT NULL,							-- mã đọc giả
	[LibrarianId] BIGINT NOT NULL,						-- mã thủ thư cho mượn sách
	
	[BorrowDate] DATE NOT NULL DEFAULT GETDATE(),		-- ngày mượn
	[Status] BIT DEFAULT 1 NOT NULL						-- trạng thái 1 = đang mượn, 0 = đã trả
)
GO
ALTER TABLE [dbo].[Borrow] ADD CONSTRAINT [pk_borrow] PRIMARY KEY([BorrowId])
ALTER TABLE [dbo].[Borrow] ADD CONSTRAINT [pk_borrow_book_item] FOREIGN KEY ([BookItemId]) REFERENCES [dbo].[BookItem]([BookItemId])
ALTER TABLE [dbo].[Borrow] ADD CONSTRAINT [fk_borrow_member] FOREIGN KEY([MemberId]) REFERENCES [dbo].[Member]([UserId])
ALTER TABLE [dbo].[Borrow] ADD CONSTRAINT [fk_borrow_librarian] FOREIGN KEY([LibrarianId]) REFERENCES [dbo].[Librarian]([UserId])
GO


-- trả sách
DROP TABLE IF EXISTS [dbo].[Return]
CREATE TABLE [dbo].[Return] (
	[BorrowId] BIGINT NOT NULL,
	[ReturnDate] DATE NOT NULL DEFAULT GETDATE(),		-- ngày trả
	[LibrarianId] BIGINT NOT NULL,						-- mã thủ thư nhận sách
)
GO
ALTER TABLE [dbo].[Return] ADD CONSTRAINT [pk_return] PRIMARY KEY([BorrowId], [ReturnDate])
ALTER TABLE [dbo].[Return] ADD CONSTRAINT [fk_return_borrow] FOREIGN KEY ([BorrowId]) REFERENCES [dbo].[Borrow]([BorrowId])
ALTER TABLE [dbo].[Return] ADD CONSTRAINT [fk_return_librarian] FOREIGN KEY([LibrarianId]) REFERENCES [dbo].[Librarian]([UserId])
GO

-----------------------------------------------------------
-----------FUNCTION----------------------------------------
-----------------------------------------------------------
CREATE OR ALTER FUNCTION [UFN_CheckPhoneNumber] (@PHONE_NUMBER_TO_CHECK NVARCHAR(15))
RETURNS BIT AS 
BEGIN
	DECLARE @C INT
	SELECT @C = COUNT(*) FROM [dbo].[User] AS U WHERE @PHONE_NUMBER_TO_CHECK = [U].[PhoneNumber] 
	RETURN (CASE WHEN @C > 0 THEN 0 ELSE 1 END)
END
GO

CREATE OR ALTER FUNCTION [UFN_CheckEmail] (@EMAIL_TO_CHECK NVARCHAR(50))
RETURNS BIT AS 
BEGIN
	DECLARE @C INT
	SELECT @C = COUNT(*) FROM [dbo].[User] AS U WHERE @EMAIL_TO_CHECK = [U].[Email]
	RETURN (CASE WHEN @C > 0 THEN 0 ELSE 1 END)
END
GO


-----------------------------------------------------------
-----------PROCEDURE---------------------------------------
-----------------------------------------------------------
CREATE OR ALTER PROCEDURE [USP_InsertBookItem]
	@BOOK_INFO_ID BIGINT
AS BEGIN
	DECLARE @BOOK_COUNT INT, @i INT = 1, @BOOK_ITEM_ID NVARCHAR(20)
	SELECT @BOOK_COUNT = [Count] FROM [dbo].[BookInfo] WHERE [BookInfoId] = @BOOK_INFO_ID

	WHILE @i <= @BOOK_COUNT
	BEGIN
		SET @BOOK_ITEM_ID = CONCAT('B', FORMAT(@BOOK_INFO_ID, 'D13'), '_', FORMAT(@i, 'D5'))
		INSERT INTO	[dbo].[BookItem] ([BookItemId], [BookInfoId]) VALUES (@BOOK_ITEM_ID, @BOOK_INFO_ID)
		SET @i = @i + 1
	END
END
GO



-----------------------------------------------------------
-----------Trigger-----------------------------------------
-----------------------------------------------------------

CREATE OR ALTER TRIGGER [TG_AUTO_INSERT_BOOK_ITEM] ON [dbo].[BookInfo] AFTER INSERT, UPDATE
AS BEGIN
	DECLARE 
		@NO_OF_INSERT_RECORD INT, 
		@LAST_INSERTED_ID BIGINT, 
		@j INT = 0

	SELECT @NO_OF_INSERT_RECORD = COUNT(*) FROM [Inserted]
	SELECT @LAST_INSERTED_ID = MAX(BookInfoId) FROM [dbo].[BookInfo]

	WHILE @j < @NO_OF_INSERT_RECORD
	BEGIN
		DECLARE @THIS_BOOK_INFO_ID BIGINT
		SET @THIS_BOOK_INFO_ID = @LAST_INSERTED_ID - @NO_OF_INSERT_RECORD + @j + 1
		EXEC [dbo].[USP_InsertBookItem] @BOOK_INFO_ID = @THIS_BOOK_INFO_ID
		SET @j = @j + 1
	END
END
GO

-----------------------------------------------------------
-----------Insert sample data------------------------------
-----------------------------------------------------------


-----------------------------
-- Admin account ------------
-- username: admin ----------
-- password: 12 -------------
-----------------------------
INSERT INTO [dbo].[User] ( [FirstName], [LastName], [Gender], [DateOfBirth], [Ssn], [Address], [PhoneNumber], [Email], [Username], [Password], [UserType], [UserStatus])
VALUES 
	(N'Khánh',  N'Lâm', 'F', '20000520', '123456789', N'', '0000000000', NULL, 'admin', '7e7175c2e20d590551e9fb500bc38c8c', 'ADMIN', 1)					-- UID = 1
GO


----------------------------------------------------------
-- Librarian account -------------------------------------
-- username: librarian -----------------------------------
-- password: 12 ------------------------------------------
-- password (orther): 000000 -----------------------------
----------------------------------------------------------
INSERT INTO [dbo].[User] ( [FirstName], [LastName], [Gender], [DateOfBirth], [Ssn], [Address], [PhoneNumber], [Email], [Username], [Password], [UserType], [UserStatus])
VALUES 
	(N'Bích',  N'Nguyễn Ngọc', 'F', '20000331', '786522653964', N'Quận 7, Hồ Chí Minh', '0000000001', NULL, 'librarian', '7e7175c2e20d590551e9fb500bc38c8c', 'LIBRARIAN', 1),											-- UID = 2
	(N'Giang',  N'Lê Trường', 'M', '19951203', '596522653964', N'26 Võ Văn Ngân, Quận Thủ Đức, Hồ Chí Minh', '0965632521', 'giangle1995@gmail.com', 'letruonggiang', 'e73adf9842e38aab89b6a8b9c8824051', 'LIBRARIAN', 0),	-- UID = 3
	(N'Minh',  N'Mai Sỹ', 'O', '19940228', '496229526', N'30C Lê Văn Chí, Quận Thủ Đức, Hồ Chí Minh', '0339566263', 'msm1994@yahoo.com', 'msm1994', 'e73adf9842e38aab89b6a8b9c8824051', 'LIBRARIAN', 1),					-- UID = 4
	(N'Thu',  N'Lê Thị', 'O', '19980214', '261626546455', N'8 Tân Lập, Quận 9, Hồ Chí Minh', '0368465655', 'thult@gmail.com', 'thult', 'e73adf9842e38aab89b6a8b9c8824051', 'LIBRARIAN', 1),									-- UID = 5
	(N'Toàn',  N'Cao Văn', 'M', '19950831', '344643356', N'24 Hồ Văn Tư, Quận Thủ Đức, Hồ Chí Minh', '0945641535', 'toancv@outlook.com', 'toancv', 'e73adf9842e38aab89b6a8b9c8824051', 'LIBRARIAN', 0),						-- UID = 6
	(N'Cúc',  N'Nguyễn Thị Thu', 'F', '19941002', '463786434', N'224 Lê Văn Việt, Quận 9, Hồ Chí Minh', '0914846315', 'cuntt@hotmail.com', 'cucntt', 'e73adf9842e38aab89b6a8b9c8824051', 'LIBRARIAN', 0),					-- UID = 7
	(N'Việt',  N'Trần Quốc', 'M', '19970209', '243624766483', N'27 Thống Nhất, Dĩ An, Bình Dương', '0901316265', 'viettqq@yahoo.com', 'viettq', 'e73adf9842e38aab89b6a8b9c8824051', 'LIBRARIAN', 1),						-- UID = 8
	(N'Sơn',  N'Huỳnh Văn', 'M', '19930402', '796289529', N'18A, đường DT 743B , Thuận An, Bình Dương', '0943325968', 'sonhv@gmail.com', 'sonhv', 'e73adf9842e38aab89b6a8b9c8824051', 'LIBRARIAN', 1),						-- UID = 9
	(N'Mi',  N'Lê Thị Ngọc', 'F', '19951218', '232355657', N'14/2 Xô Viết Nghệ Tĩnh, Bình Thạnh, Hồ Chí Minh', '0968454664', 'miltn@gmail.com', 'miltn', 'e73adf9842e38aab89b6a8b9c8824051', 'LIBRARIAN', 1),				-- UID = 10
	(N'Hương',  N'Võ Thu', 'F', '19920112', '786345586521', N'20 Hồ Tùng Mậu, Quận 1, HCM', '0927893532', 'huongvt@gmail.com', 'huongvo', 'e73adf9842e38aab89b6a8b9c8824051', 'LIBRARIAN', 1)								-- UID = 11
GO
INSERT INTO [dbo].[Librarian] ( [UserId], [StartDate], [Salary] )
VALUES
	(2, '20181025', 8000000),
	(3, '20181102', 6500000),
	(4, '20190103', 5500000),
	(5, '20190605', 3500000),
	(6, '20190924', 4500000),
	(7, '20191031', 7500000),
	(8, '20191104', 8500000),
	(9, '20200301', 6800000),
	(10, '20200414', 4200000),
	(11, '20200512', 5400000)
GO

----------------------------------------------------------
-- member account ----------------------------------------
-- username: member --------------------------------------
-- password: 12 ------------------------------------------
-- password (orther): 000000 -----------------------------
----------------------------------------------------------
INSERT INTO [dbo].[User] ( [FirstName], [LastName], [Gender], [DateOfBirth], [Ssn], [Address], [PhoneNumber], [Email], [Username], [Password], [UserStatus])
VALUES 
	(N'Ngọc',  N'Nguyễn Ngọc', 'F', '20000331', '563178953', N'Quận 7, Hồ Chí Minh', '0000000002', NULL, 'member', '7e7175c2e20d590551e9fb500bc38c8c', 1),														-- UID = 12
	(N'Khải',  N'Lê Trường', 'M', '19951203', '522532456133', N'26 Võ Văn Ngân, Quận Thủ Đức, Hồ Chí Minh', '0965632000', 'khailt@gmail.com', '522532456133', 'e73adf9842e38aab89b6a8b9c8824051', 0),		-- UID = 13
	(N'Hoàng',  N'Phan Văn', 'O', '19940228', '003259526', N'30C Lê Văn Chí, Quận Thủ Đức, Hồ Chí Minh', '0367566263', 'hangpv@yahoo.com', '003259526', 'e73adf9842e38aab89b6a8b9c8824051', 1),					-- UID = 14
	(N'Nhi',  N'Võ Thị Yến', 'O', '19980214', '260022546455', N'8 Tân Lập, Quận 9, Hồ Chí Minh', '0396565655', 'nhivty@gmail.com', '260022546455', 'e73adf9842e38aab89b6a8b9c8824051', 1),									-- UID = 15
	(N'Thắng',  N'Đinh Công', 'M', '19950831', '086343356', N'24 Hồ Văn Tư, Quận Thủ Đức, Hồ Chí Minh', '0900041535', 'thangdc@outlook.com', '086343356', 'e73adf9842e38aab89b6a8b9c8824051', 0),					-- UID = 16
	(N'Hường',  N'Trần Bích', 'F', '19941002', '463786059', N'224 Lê Văn Việt, Quận 9, Hồ Chí Minh', '0914846012', 'huongtb@hotmail.com', '463786059', 'e73adf9842e38aab89b6a8b9c8824051', 0),					-- UID = 17
	(N'Lâm',  N'Lê Sơn', 'M', '19970209', '243624023653', N'27 Thống Nhất, Dĩ An, Bình Dương', '0901316289', 'lamls@yahoo.com', '243624023653', 'e73adf9842e38aab89b6a8b9c8824051', 1),							-- UID = 18
	(N'Đạt',  N'Hồ Văn', 'M', '19930402', '796200529', N'18A, đường DT 743B , Thuận An, Bình Dương', '0943325911', 'dathv@gmail.com', '796200529', 'e73adf9842e38aab89b6a8b9c8824051', 1),						-- UID = 19
	(N'Cẩm',  N'Đinh Thị', 'F', '19951218', '230153689', N'14/2 Xô Viết Nghệ Tĩnh, Bình Thạnh, Hồ Chí Minh', '0963534664', 'camdt@gmail.com', '230153689', 'e73adf9842e38aab89b6a8b9c8824051', 1),				-- UID = 20
	(N'Lụa',  N'Dương Thị', 'F', '19920112', '786860586521', N'20 Hồ Tùng Mậu, Quận 1, HCM', '0927003532', 'luadt@gmail.com', '786860586521', 'e73adf9842e38aab89b6a8b9c8824051', 1)								-- UID = 21
GO
INSERT INTO [dbo].[Member] ([UserId], [RegisterDate], [ExpDate] )
VALUES
	(12, '20181025', '20210101'),
	(13, '20181102', '20210101'),
	(14, '20190103', '20210101'),
	(15, '20190605', '20210101'),
	(16, '20190924', '20210101'),
	(17, '20191031', '20210101'),
	(18, '20191104', '20210101'),
	(19, '20200301', '20210101'),
	(20, '20200414', '20210101'),
	(21, '20200512', '20210101')
GO


-- Chuyên mục sách 
INSERT INTO [dbo].[BookCategory] ([BookCategoryName], [Limitdays], [BookCategoryStatus]) 
VALUES 
	(N'Truyện ngắn - tản văn', 20, 1),		--1
	(N'Kỹ năng sống', 25, 1),				--2
	(N'Tiểu thuyết', 30, 1),				--3
	(N'Luyện thi ĐH-CĐ', 50, 1),			--4
	(N'Khoa học - công nghệ', 30, 1),		--5
	(N'Tiếng Anh', 30, 1),					--6
	(N'Tiếng Pháp', 25, 0),					--7
	(N'Tiếng Đức', 14, 0)					--8
GO


-- Nhà xuất bản
INSERT INTO [dbo].[Publisher] ([PublisherName], [PhoneNumber], [Address], [Email], [Website]) 
VALUES 
	(N'Phụ Nữ Việt Nam', '02439710717', N'39 Hàng Chuối, Hà Nội', 'truyenthongvaprnxbpn@gmail.com', 'http://nxbphunu.com.vn/'),
	(N'Trẻ', '02839316289', N'161B Lý Chính Thắng, Phường 7, Quận 3, Hồ Chí Minh', 'info@ybook.vn', 'https://www.nxbtre.com.vn/'),
	(N'Văn học', '02437161518', N'18 Nguyễn Trường Tộ - Ba Đình - Hà Nội', 'info@nxbvanhoc.com.vn', 'https://nxbvanhoc.com.vn/'),
	(N'Đại Học Quốc Gia Hà Nội', '02439714896', N'16 Hàng Chuối, Phạm Đình Hổ, Hai Bà Trưng, Hà Nội', 'nhaxuatbandhqghanoi@gmail.com', 'https://press.vnu.edu.vn/'),
	(N'Đà Nẵng', '02363812964', N'108 Bạch Đằng, Hải Châu 1, Hải Châu, Đà Nẵng', 'xuatban@nxbdanang.vn', 'https://nxbdanang.vn/'),
	(N'Thế Giới', '02838220102', N'7 Nguyễn Thị Minh Khai, Bến Nghé, Quận 1, Hồ Chí Minh', 'thegioi@hn.vnn.vn', 'http://www.thegioipublishers.vn/'),
	(N'Tổng Hợp TPHCM', '02838256804', N'62 Nguyễn Thị Minh Khai, Đa Kao, Quận 1, Hồ Chí Minh', 'tonghop@nxbhcm.com.vn', 'https://www.nxbhcm.com.vn/'),
	(N'Thanh Niên', '0462631724', N'64 Bà Triệu, Hoàn Kiếm, Hà Nội', 'chinhanhnxbthanhnien@gmail.com', 'https://www.nhaxuatbanthanhnien.vn/')
GO


-- Tác giả
INSERT INTO [dbo].[Author] ([NickName]) 
VALUES 
	(N'Ở Đây Zui Nè'),			--1
	(N'Tony Buổi Sáng'),		--2
	(N'Paulo Coelho'),			--3
	(N'Jorge Amado'),			--4
	(N'Ngọc Giao'),				--5
	(N'Lê Đình Thanh'),			--6
	(N'Nguyễn Việt Anh'),		--7
	(N'Võ Quốc Bá Cẩn'),		--8
	(N'Trần Quốc Anh'),			--9
	(N'Trần Phương'),			--10
	(N'Mai Lan Hương'),			--11
	(N'Hà Thanh Uyên'),			--12
	(N'Mai Thị Tường Vân'),		--13
	(N'Kiên Trần'),				--14
	(N'Nguyễn Thanh Loan'),		--15
	(N'Stacey Riches'),			--16
	(N'Claire Luong'),			--17
	(N'Trí'),					--18
	(N'Trác Nhã'),				--19
	(N'Dale Carnegie')			--20
GO


-- Đầu sách
INSERT INTO [dbo].[BookInfo] ([Title], [PublisherId], [YearPublished], [BookCategoryId], [PageNumber], [Size], [Price], [Count])
VALUES 
	(N'Vui Vẻ Không Quạu Nha', 1, 2020, 1, 280, '10 x 12', 53820, 20),												--1
	(N'Cà Phê Cùng Tony', 2, 2017, 1, 268, '13 x 20', 63000, 20),													--2
	(N'Trên Đường Băng', 2, 2017, 2, 308, '13 x 20', 64000, 15),													--3
	(N'Nhà Giả Kim', 3, 2017, 3, 224, '13 x 20.5', 55200, 10),														--4
	(N'Hảo Hán Nơi Trảng Cát', 3, 2017, 3, 380, '14.5 x 20.5', 75000, 10), 											--5
	(N'Quán Gió', 3, 2017, 3, 180, '14 x 20.5', 52800, 10),															--6
	(N'GT Phát Triển Ứng Dụng Web', 4, 2019, 5, 340, '16 x 24', 168000, 10),										--7
	(N'Sử Dụng AM - GM Để Chứng Minh Bất Đẳng Thức', 4, 2019, 4, 256, '16 x 24', 60000, 10),						--8
	(N'VẺ ĐẸP BẤT ĐẲNG THỨC TRONG CÁC KÌ THI OLYMPIC TOÁN HỌC', 4, 2016, 4, 492, '16 x 24', 95000, 10),				--9
	(N'Giải Thích Ngữ Pháp Tiếng Anh (Bài Tập & Đáp Án)', 5, 2019, 6, 200, '16 x 24', 112500, 15),					--10
	(N'Giải Mã Trí Nhớ)', 5, 2019, 5, 102, '14.5 x 21', 98500, 10),													--11
	(N'Cẩm Nang Tự Học Ielts)', 6, 2019, 6, 188, '16 x 24', 65000, 10),												--12
	(N'Ngữ Pháp Tiếng Anh', 5, 2019, 6, 280, '13.5 x 20', 60000, 15),												--13
	(N'Little Stories – To Push You Forward', 5, 2018, 6, 192, '11.3 x 17.6', 50050, 5),							--14
	(N'Little Stories - To Make You A Good Person', 5, 2018, 6, 192, '11.3 x 17.6', 50050, 5),						--15
	(N'Little Stories - The Best Book For Your Leisure Time', 5, 2018, 6, 192, '11.3 x 17.6', 50050, 5),			--16
	(N'Little Stories - To Get More Knowledge', 5, 2018, 6, 192, '11.3 x 17.6', 50050, 5),							--17
	(N'Little Stories - To Have A Nice Day', 5, 2018, 6, 192, '11.3 x 17.6', 50050, 5),								--18
	(N'Little Stories - To Share With Your Friends', 5, 2018, 6, 192, '11.3 x 17.6', 50050, 5),						--19
	(N'Little Stories - The Book For Peaceful Nights', 5, 2018, 6, 192, '11.3 x 17.6', 50050, 5),					--20
	(N'Tự Thương Mình Sau Những Tháng Năm Thương Người', 3, 2019, 1, 248, '13 x 20.5', 58500, 10),					--21
	(N'Mình Buồn Đủ Rồi, Mình Hạnh Phúc Thôi!', 3, 2020, 1, 224, '13 x 20.5', 71200, 10),							--22
	(N'Khéo Ăn Nói Sẽ Có Được Thiên Hạ', 3, 2018, 2, 406, '14.5 x 20.5', 82500, 10),								--23
	(N'Đắc Nhân Tâm', 7, 2018, 2, 320, '14.5 x 20.5', 73500, 15),													--24
	(N'Đá Cuội Hay Kim Cương - Cùng Dale Carnegie Tiến Tới Thành Công', 8, 2018, 2, 248, '14.5 x 20.5', 73500, 15)	--25
GO


-- Quan hệ tác giả - sách
INSERT INTO [dbo].[BookAuthor] ([BookInfoId], [AuthorId]) 
VALUES 
	(1, 1),
	(2, 2),
	(3, 2),
	(4, 3),
	(5, 4),
	(6, 5),
	(7, 6), (7, 7),
	(8, 8), (8, 9),
	(9, 8), (9, 9), (9, 10),
	(10, 11), (10, 12),
	(11, 13),
	(12, 14),
	(13, 11), (13, 15),
	(14, 16),
	(15, 16),
	(16, 17),
	(17, 17),
	(18, 16),
	(19, 16),
	(20, 17),
	(21, 18),
	(22, 18),
	(23, 19),
	(24, 20),
	(25, 20)
GO