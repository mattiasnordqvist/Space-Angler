CREATE TRIGGER [Delete%_Node_%Trigger]
    ON [%_Node_%]
    INSTEAD OF DELETE
AS
	ALTER TABLE [%_Node_%] NOCHECK CONSTRAINT all
	DECLARE @Id INT = 0
	DECLARE db_cursor CURSOR FOR 
	SELECT [%_Id_%] FROM deleted
	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @l INT = (SELECT L FROM [%_Node_%] WHERE [%_Id_%] = @Id)
		DECLARE @r INT = (SELECT R FROM [%_Node_%] WHERE [%_Id_%] = @Id)
		DECLARE @w INT = (@r-@l+1)
		DELETE FROM [%_Node_%] WHERE L BETWEEN @l AND @r
		DELETE FROM [%_Node_%] WHERE [%_Id_%] = @Id
		UPDATE [%_Node_%] SET L = L - (@w) WHERE L > @r
		UPDATE [%_Node_%] SET R = R - (@w) WHERE R > @r
		
		FETCH NEXT FROM db_cursor INTO @Id 
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor
	ALTER TABLE [%_Node_%] WITH CHECK CHECK CONSTRAINT all

GO