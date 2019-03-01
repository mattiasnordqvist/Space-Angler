CREATE TRIGGER afterUpdateTrigger
    ON [Node]
    AFTER UPDATE
AS
	IF UPDATE (Parent_Id)
	BEGIN
		ALTER TABLE [Node] NOCHECK CONSTRAINT all
	DECLARE @Id INT = 0
	DECLARE @NewParent_Id INT = 0
	DECLARE db_cursor CURSOR FOR 
	SELECT Id, Parent_Id FROM inserted
	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @Id, @NewParent_Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @l INT = (SELECT L FROM [Node] WHERE Id = @Id)
		DECLARE @r INT = (SELECT R FROM [Node] WHERE Id = @Id)
		DECLARE @w INT = (@r-@l+1)

		DECLARE @barnbarn TABLE(
			Id INT NOT NULL
		)

		-- hitta alla barn o barnbarn
		INSERT INTO @barnbarn SELECT Id FROM [Node] WHERE L > @l AND R < @R 

		UPDATE [Node] SET L = L - (@w) WHERE L > @r
		UPDATE [Node] SET R = R - (@w) WHERE R > @r

		DECLARE @pR INT = (SELECT R FROM [Node] WHERE Id = @NewParent_Id)
		--uppdatera flyttad nod som om den inte hade några barn
		DECLARE @pNewR INT = @pR+@w
		UPDATE [Node] SET R = @pNewR WHERE Id = @NewParent_Id
		DECLARE @newL INT = @pR
		UPDATE [Node] SET L = @newL, R = @pNewR-1 WHERE Id = @Id
		
		--uppdatera alla barn o barnbarn till flyttad nod
		UPDATE [Node] SET L = L + (@newL - @l), R = R + (@newL - @l) WHERE Id IN (SELECT Id FROM @barnbarn)

		--Uppdatera alla noder till höger om flyttad nod
		UPDATE [Node] SET L = L + @w WHERE L > @pR AND Id NOT IN (SELECT Id FROM @barnbarn) AND Id <> @NewParent_Id AND Id <> @Id

		UPDATE [Node] SET R = R + @w WHERE R >= @pR AND Id NOT IN (SELECT Id FROM @barnbarn) AND Id <> @NewParent_Id AND Id <> @Id

		FETCH NEXT FROM db_cursor INTO @Id, @NewParent_Id
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor
	ALTER TABLE [Node] WITH CHECK CHECK CONSTRAINT all
	END
GO
