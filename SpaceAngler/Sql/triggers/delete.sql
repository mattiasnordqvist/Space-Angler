CREATE TRIGGER insteadOfDeleteTrigger
    ON [Node]
    INSTEAD OF DELETE
AS
	ALTER TABLE [Node] NOCHECK CONSTRAINT all
	DECLARE @Id INT = 0
	DECLARE db_cursor CURSOR FOR 
	SELECT [Id] FROM deleted
	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @l INT = (SELECT L FROM [Node] WHERE [Id] = @Id)
		DECLARE @r INT = (SELECT R FROM [Node] WHERE [Id] = @Id)
		DECLARE @w INT = (@r-@l+1)
		DELETE FROM [Node] WHERE L BETWEEN @l AND @r
		DELETE FROM [Node] WHERE Id = @Id
		UPDATE [Node] SET L = L - (@w) WHERE L > @r
		UPDATE [Node] SET R = R - (@w) WHERE R > @r
		
		FETCH NEXT FROM db_cursor INTO @Id 
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor
	ALTER TABLE [Node] WITH CHECK CHECK CONSTRAINT all

GO