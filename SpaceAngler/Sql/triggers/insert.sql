CREATE TRIGGER InsertNodeTrigger
    ON [Node]
    AFTER INSERT
AS
	ALTER TABLE [Node] NOCHECK CONSTRAINT all
	DECLARE @id INT = 0
	DECLARE @parentId INT = 0
	DECLARE db_cursor CURSOR FOR 
	SELECT Id, Parent_Id FROM inserted
	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @id, @parentId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@parentId) IS NOT NULL
		BEGIN
			DECLARE @pR INT = (SELECT R FROM [Node] WHERE [Id] = @parentId)
			UPDATE [Node] SET L = L + 2 WHERE L > @pR
			UPDATE [Node] SET R = R + 2 WHERE R >= @pR
			UPDATE [Node] SET L = @pR, R = @pr+1 WHERE Id = @id
		END 
		ELSE 
		BEGIN
			DECLARE @maxR INT = (SELECT ISNULL(Max(R),0) FROM [Node] WHERE Parent_Id IS NULL)
			UPDATE [Node] SET L = @maxR+1, R = @maxR+2 WHERE Id = @id
		END



		FETCH NEXT FROM db_cursor INTO @id, @parentId
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor
	ALTER TABLE [Node] WITH CHECK CHECK CONSTRAINT all

GO
