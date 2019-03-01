CREATE TRIGGER insteadOfInsertTrigger
    ON [Node]
    INSTEAD OF INSERT
AS
	ALTER TABLE [Node] NOCHECK CONSTRAINT all
	DECLARE @parentId INT = 0
	DECLARE @value INT = 0
	DECLARE db_cursor CURSOR FOR 
	SELECT Parent_Id, [Value] FROM inserted
	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @parentId, @value
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @pR INT = (SELECT R FROM [Node] WHERE [Id] = @parentId)
		
		UPDATE [Node] SET L = L + 2 WHERE L > @pR
		UPDATE [Node] SET R = R + 2 WHERE R >= @pR
		
		INSERT INTO [Node] ([Value], [Parent_Id], L, R) VALUES (@value, @parentId, @pR, @pR+1);
		FETCH NEXT FROM db_cursor INTO @parentId, @value
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor
	ALTER TABLE [Node] WITH CHECK CHECK CONSTRAINT all

GO
