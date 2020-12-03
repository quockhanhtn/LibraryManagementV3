USE master
GO
DROP DATABASE IF EXISTS [LibraryManagement]
GO
CREATE DATABASE [LibraryManagement]
GO
USE [LibraryManagement]
GO

DROP TABLE IF EXISTS [dbo].[User]
CREATE TABLE [dbo].[User](
	[UserId] BIGINT IDENTITY(1, 1),
	[FisrtName] NVARCHAR(10) NULL,
	[LastName] NVARCHAR(30) NULL,
	[Gender] NVARCHAR(10) NULL,
	[DateOfBirth] DATE NULL,
	[Ssn] VARCHAR(12) NULL,

	[Address] NVARCHAR(100) NULL,
	[PhoneNumber] VARCHAR(15) UNIQUE NOT NULL,
	[Email] VARCHAR(50) NULL,

	[Username] VARCHAR(16) UNIQUE NOT NULL,
	[Password] VARCHAR(32) NOT NULL,
	[UserType] VARCHAR(1) NOT NULL DEFAULT 'M',		-- loại tài khoản admin = A, librarian = L, member = M
	[UserStatus] BIT NOT NULL DEFAULT 1,
	[ImagePath] VARCHAR(500) NULL,
)
GO
ALTER TABLE [dbo].[User] ADD CONSTRAINT [PK_User] PRIMARY KEY([UserId])
GO

---- tài khoản quản trị viên
--DROP TABLE IF EXISTS [dbo].[Admin]
--CREATE TABLE [dbo].[Admin] (
--	[UserId] BIGINT NOT NULL,
--)
--GO
--ALTER TABLE [dbo].[Admin] ADD CONSTRAINT [pk_admin] PRIMARY KEY ([UserId])
--ALTER TABLE [dbo].[Admin] ADD CONSTRAINT [fk_user_admin] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId])
--GO

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
	[AuthorStatus] BIT DEFAULT 1 NULL,
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
	[Email] VARCHAR(30) NULL,
	[Website] VARCHAR(40) NULL,
	[PublisherStatus] BIT DEFAULT 1 NULL,
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
	[Limitdays] INT NULL,						-- số ngày cho mượn
	[BookCategoryStatus] BIT DEFAULT 1 NULL
)
GO
ALTER TABLE [dbo].[BookCategory] ADD CONSTRAINT [pk_book_category] PRIMARY KEY([BookCategoryId])
GO


-- đầu sách
DROP TABLE IF EXISTS [dbo].[BookInfo]
CREATE TABLE [dbo].[BookInfo]
(
	[BookInfoId] BIGINT IDENTITY(1, 1),
	[Title] NVARCHAR(100) NULL,				-- tựa sách

	[BookCategoryId] BIGINT NULL,			-- mã chuyên mục
	[PublisherId] BIGINT NOT NULL,			-- mã nxb

	[YearPublished] INT NULL,				-- năm xb
	[PageNumber] INT NULL,					-- số trang
	[Size] VARCHAR(11) NULL,				-- kích thước
	[Price] DECIMAL(19, 0) NULL,			-- giá tiền
	[BookInfoStatus] BIT DEFAULT 1 NULL,
)
GO 
ALTER TABLE [dbo].[BookInfo] ADD CONSTRAINT [pk_book] PRIMARY KEY ([BookInfoId])
ALTER TABLE [dbo].[BookInfo] ADD CONSTRAINT [fk_book_category] FOREIGN KEY ([BookCategoryId]) REFERENCES [dbo].[BookCategory]([BookCategoryId])
ALTER TABLE [dbo].[BookInfo] ADD CONSTRAINT [fk_book_publisher]  FOREIGN KEY([PublisherId]) REFERENCES [dbo].[Publisher]([PublisherId])
GO

-- cuốn sách
DROP TABLE IF EXISTS [dbo].[BookItem]
CREATE TABLE [dbo].[BookItem] (
	[BookItemId]VARCHAR(12) NOT NULL,
	[BookInfoId] BIGINT NOT NULL,
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
	[BookItemId]VARCHAR(12) NOT NULL,					-- mã cuốn sách

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

------------------------------------------

-- Admin user
-- username: admin
-- password: admin
INSERT INTO dbo.[User] ( PhoneNumber, Username, Password, UserType, UserStatus)
VALUES ( '00', 'admin', 'db69fc039dcbd2962cb4d28f5891aae1', 'A', 1)

INSERT INTO dbo.[User] (FisrtName, LastName, PhoneNumber, Username, Password, UserType, UserStatus)
VALUES ( N'Vy',N'Huỳnh','001', 'librarian', 'db69fc039dcbd2962cb4d28f5891aae1', 'L', 1)

INSERT INTO dbo.[User] (FisrtName, LastName, PhoneNumber, Username, Password, UserType, UserStatus)
VALUES ( N'Mỹ',N'Phan','002', 'member', 'db69fc039dcbd2962cb4d28f5891aae1', 'M', 1)