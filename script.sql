USE [master]
GO
/****** Object:  Database [ManageNet]    Script Date: 08/19/2019 1:33:54 PM ******/
CREATE DATABASE [ManageNet]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ManageNet', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.KENTFIRE\MSSQL\DATA\ManageNet.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ManageNet_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.KENTFIRE\MSSQL\DATA\ManageNet_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ManageNet] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ManageNet].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ManageNet] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ManageNet] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ManageNet] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ManageNet] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ManageNet] SET ARITHABORT OFF 
GO
ALTER DATABASE [ManageNet] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ManageNet] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ManageNet] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ManageNet] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ManageNet] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ManageNet] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ManageNet] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ManageNet] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ManageNet] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ManageNet] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ManageNet] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ManageNet] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ManageNet] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ManageNet] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ManageNet] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ManageNet] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ManageNet] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ManageNet] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ManageNet] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ManageNet] SET  MULTI_USER 
GO
ALTER DATABASE [ManageNet] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ManageNet] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ManageNet] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ManageNet] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [ManageNet]
GO
/****** Object:  Table [dbo].[Administrators]    Script Date: 08/19/2019 1:33:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Administrators](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NULL,
	[Password] [nchar](10) NULL,
 CONSTRAINT [PK_Administrators] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Cinema]    Script Date: 08/19/2019 1:33:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cinema](
	[IDMovie] [int] IDENTITY(1,1) NOT NULL,
	[NameMovie] [nvarchar](50) NULL,
	[Director] [nvarchar](50) NULL,
	[Ticket] [int] NULL,
 CONSTRAINT [PK_Cinema] PRIMARY KEY CLUSTERED 
(
	[IDMovie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Member]    Script Date: 08/19/2019 1:33:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NULL,
	[Password] [nchar](10) NULL,
	[Email] [nvarchar](50) NULL,
	[Money] [int] NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
USE [master]
GO
ALTER DATABASE [ManageNet] SET  READ_WRITE 
GO
