USE [master]
GO
/****** Object:  Database [WanagerDB]    Script Date: 17/10/2018 10:30:14 ******/
CREATE DATABASE [WanagerDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WanagerDB', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS2012\MSSQL\DATA\WanagerDB.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'WanagerDB_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS2012\MSSQL\DATA\WanagerDB_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [WanagerDB] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WanagerDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WanagerDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WanagerDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WanagerDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WanagerDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WanagerDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [WanagerDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WanagerDB] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [WanagerDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WanagerDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WanagerDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WanagerDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WanagerDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WanagerDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WanagerDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WanagerDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WanagerDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WanagerDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WanagerDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WanagerDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WanagerDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WanagerDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WanagerDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WanagerDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WanagerDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [WanagerDB] SET  MULTI_USER 
GO
ALTER DATABASE [WanagerDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WanagerDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WanagerDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WanagerDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [WanagerDB]
GO
/****** Object:  User [IIS APPPOOL\Wanager]    Script Date: 17/10/2018 10:30:14 ******/
CREATE USER [IIS APPPOOL\Wanager] FOR LOGIN [IIS APPPOOL\Wanager] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [IIS APPPOOL\Wanager]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCompanies]    Script Date: 17/10/2018 10:30:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCompanies]	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Companies.Active,Companies.Address,Companies.GUID,Companies.ID,Companies.Name
	FROM Companies
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetEmployees]    Script Date: 17/10/2018 10:30:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetEmployees]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Employees.Active,Employees.BirthDate,Employees.CompanyID,Employees.Email,Employees.FirstName,Employees.GUID,Employees.ID,Employees.LastName,Employees.Password,Employees.Username,
	Companies.Name CompanyName
	FROM Employees
	inner join Companies on Companies.GUID = Employees.CompanyID
END

GO
/****** Object:  StoredProcedure [dbo].[sp_GetScales]    Script Date: 17/10/2018 10:30:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetScales]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Scales.Active,Scales.CompanyID,Scales.GUID,Scales.MAC,Scales.Status,Scales.Weight,Scales.WeightDate,Scales.Name,
	Companies.Name CompanyName
	FROM Scales
	inner join Companies on Companies.GUID = Scales.CompanyID
END

GO
/****** Object:  StoredProcedure [dbo].[sp_SaveCompany]    Script Date: 17/10/2018 10:30:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SaveCompany]
@GUID	bigint	,
@Name	nvarchar(MAX)	,
@Address	nvarchar(MAX)	,
@ID	nvarchar(50)	,
@Active	bit	
		
		
AS
BEGIN
	SET NOCOUNT ON;

	if @GUID = 0		
			insert into Companies(Active,Address,ID,Name)
			OUTPUT INSERTED.GUID
			values(@Active,@Address,@ID,@Name)
	else	
		update Companies 
		Set
		Active = @Active,
		Address = @Address,
		ID = @ID,
		Name = @Name
		OUTPUT INSERTED.GUID
		where GUID = @GUID

END

GO
/****** Object:  StoredProcedure [dbo].[sp_SaveEmployee]    Script Date: 17/10/2018 10:30:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SaveEmployee]
@GUID	bigint	,
@FirstName	nvarchar(MAX)	,
@LastName	nvarchar(MAX)	,
@ID	nvarchar(50)	,
@BirthDate	datetime	,
@Username	nvarchar(MAX)	,
@Password	nvarchar(MAX)	,
@CompanyID	bigint	,
@Active	bit	,
@Email	nvarchar(MAX)	
		
AS
BEGIN
	SET NOCOUNT ON;

	if @GUID = 0		
			insert into Employees(Active,BirthDate,CompanyID,Email,FirstName,ID,LastName,Password,Username)
			OUTPUT INSERTED.GUID
			values(@Active,@BirthDate,@CompanyID,@Email,@FirstName,@ID,@LastName,@Password,@Username)
	else	
		update Employees 
		Set
		Active = @Active,
		BirthDate = @BirthDate,
		CompanyID = @CompanyID,
		Email = @Email,
		FirstName = @FirstName,
		ID = @ID,
		LastName = @LastName,
		Password = @Password,
		Username = @Username
		OUTPUT INSERTED.GUID
		where GUID = @GUID

END

GO
/****** Object:  StoredProcedure [dbo].[sp_SaveScale]    Script Date: 17/10/2018 10:30:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SaveScale]
@GUID	bigint	,
@MAC	nvarchar(MAX)	,
@CompanyID	bigint	,
@Status	bit	,
@Weight	nvarchar(50)	,
@WeightDate	datetime	,
@Active	bit	,
@Name	nvarchar(MAX)	
AS
BEGIN
	SET NOCOUNT ON;

	if @GUID = 0		
			insert into Scales(Active,CompanyID,MAC,Status,Weight,WeightDate,Name)
			OUTPUT INSERTED.GUID
			values(@Active,@CompanyID,@MAC,@Status,@Weight,@WeightDate,@Name)
	else	
		update Scales 
		Set
		Active = @Active,
		CompanyID = @CompanyID,
		MAC = @MAC,
		Status = @Status,
		Weight = @Weight,
		WeightDate = @WeightDate,
		Name = @Name
		OUTPUT INSERTED.GUID
		where GUID = @GUID

END

GO
/****** Object:  Table [dbo].[Companies]    Script Date: 17/10/2018 10:30:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[GUID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[ID] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Companies_Active]  DEFAULT ((0)),
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Employees]    Script Date: 17/10/2018 10:30:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[GUID] [bigint] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[ID] [nvarchar](50) NULL,
	[BirthDate] [datetime] NULL,
	[Username] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[CompanyID] [bigint] NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Employees_Active]  DEFAULT ((0)),
	[Email] [nvarchar](max) NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Scales]    Script Date: 17/10/2018 10:30:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Scales](
	[GUID] [bigint] IDENTITY(1,1) NOT NULL,
	[MAC] [nvarchar](max) NULL,
	[CompanyID] [bigint] NULL,
	[Status] [bit] NULL,
	[Weight] [nvarchar](50) NULL,
	[WeightDate] [datetime] NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Scales_Active]  DEFAULT ((0)),
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Scales] PRIMARY KEY CLUSTERED 
(
	[GUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [WanagerDB] SET  READ_WRITE 
GO
