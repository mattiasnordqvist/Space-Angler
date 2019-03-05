IF OBJECT_ID('dbo.[Node]', 'U') IS NOT NULL 
  DROP TABLE dbo.[Node]
GO

CREATE TABLE [dbo].[Node](
	[Id] [int] NOT NULL,
	[Parent_Id] [int] NULL)