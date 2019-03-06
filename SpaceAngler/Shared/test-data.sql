insert into [%_Node_%] (ParentId, [Value]) values (NULL, 0)
insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 1), a.[Value]+1    from [%_Node_%] a
insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 2), a.[Value]+2    from [%_Node_%] a
insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 4), a.[Value]+4    from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 8), a.[Value]+8    from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 16), a.[Value]+16   from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 1), a.[Value]+32   from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 2), a.[Value]+64   from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 4), a.[Value]+128  from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 8), a.[Value]+256  from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 16), a.[Value]+512  from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 32), a.[Value]+1024 from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 1), a.[Value]+2048 from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 2), a.[Value]+4096 from [%_Node_%] a
--insert into [%_Node_%] select (SELECT TOP 1 p.Id FROM [%_Node_%] p WHERE p.[Value] = a.[Value] % 4), a.[Value]+8192 from [%_Node_%] a
