DROP TABLE [Node]
GO

CREATE TABLE [dbo].[Node](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Parent_Id] [int] NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_Period] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Node]  WITH CHECK ADD  CONSTRAINT [FK_Period_Period] FOREIGN KEY([Parent_Id])
REFERENCES [dbo].[Node] ([Id])
GO

ALTER TABLE [dbo].[Node] CHECK CONSTRAINT [FK_Period_Period]
GO

--DELETE FROM [Node]
insert into [Node] (Parent_Id, [Value]) values (NULL, 0)
insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 1), a.[Value]+1    from [Node] a
insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 2), a.[Value]+2    from [Node] a
insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 4), a.[Value]+4    from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 8), a.[Value]+8    from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 16), a.[Value]+16   from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 1), a.[Value]+32   from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 2), a.[Value]+64   from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 4), a.[Value]+128  from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 8), a.[Value]+256  from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 16), a.[Value]+512  from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 32), a.[Value]+1024 from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 1), a.[Value]+2048 from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 2), a.[Value]+4096 from [Node] a
--insert into [Node] select (SELECT TOP 1 p.Id FROM [Node] p WHERE p.[Value] = a.[Value] % 4), a.[Value]+8192 from [Node] a

-- Add left right

ALTER TABLE [Node] ADD L INT NULL
ALTER TABLE [Node] ADD R INT NULL
GO


--- Leaf nodes?
ALTER TABLE [Node] ADD IsLeaf AS CASE WHEN L + 1 = R THEN 1 ELSE 0 END persisted
