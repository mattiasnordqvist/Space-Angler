IF OBJECT_ID('dbo.[%_Node_%]', 'U') IS NOT NULL 
  DROP TABLE dbo.[%_Node_%]
GO

CREATE TABLE [dbo].[%_Node_%](
	[Id] [int] NOT NULL,
	[Parent_Id] [int] NULL)