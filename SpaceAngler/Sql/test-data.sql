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