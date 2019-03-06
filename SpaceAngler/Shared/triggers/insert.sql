CREATE TRIGGER [Insert%_Node_%Trigger]
    ON [%_Node_%]
    AFTER INSERT
AS
	ALTER TABLE [%_Node_%] NOCHECK CONSTRAINT all
	DECLARE @id INT = 0
	DECLARE @parentId INT = 0
	DECLARE db_cursor CURSOR FOR 
	SELECT [%_Id_%], [%_Parent_Id_%] FROM inserted
	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @id, @parentId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (@parentId) IS NOT NULL
		BEGIN
			DECLARE @pR INT = (SELECT R FROM [%_Node_%] WHERE [%_Id_%] = @parentId)
			UPDATE [%_Node_%] SET L = L + 2 WHERE L > @pR
			UPDATE [%_Node_%] SET R = R + 2 WHERE R >= @pR
			UPDATE [%_Node_%] SET L = @pR, R = @pr+1 WHERE [%_Id_%] = @id
		END 
		ELSE 
		BEGIN
			DECLARE @maxR INT = (SELECT ISNULL(Max(R),0) FROM [%_Node_%] WHERE [%_Parent_Id_%] IS NULL)
			UPDATE [%_Node_%] SET L = @maxR+1, R = @maxR+2 WHERE [%_Id_%] = @id
		END



		FETCH NEXT FROM db_cursor INTO @id, @parentId
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor
	ALTER TABLE [%_Node_%] WITH CHECK CHECK CONSTRAINT all

GO
