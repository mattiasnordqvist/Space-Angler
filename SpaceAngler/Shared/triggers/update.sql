CREATE TRIGGER [Update%_Node_%Trigger]
    ON [%_Node_%]
    AFTER UPDATE
AS
	IF UPDATE ([%_Parent_Id_%])
	BEGIN
		ALTER TABLE [%_Node_%] NOCHECK CONSTRAINT all
	DECLARE @Id INT = 0
	DECLARE @NewParentId INT = 0
	DECLARE db_cursor CURSOR FOR 
	SELECT [%_Id_%], [%_Parent_Id_%] FROM inserted
	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @Id, @NewParentId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @l INT = (SELECT L FROM [%_Node_%] WHERE [%_Id_%] = @Id)
		DECLARE @r INT = (SELECT R FROM [%_Node_%] WHERE [%_Id_%] = @Id)
		DECLARE @w INT = (@r-@l+1)

		DECLARE @barnbarn TABLE(
			[%_Id_%] INT NOT NULL
		)

		-- hitta alla barn o barnbarn
		INSERT INTO @barnbarn SELECT [%_Id_%] FROM [%_Node_%] WHERE L > @l AND R < @R 
		--reducera på höger sida om uppdaterad nod
		UPDATE [%_Node_%] SET L = L - (@w) WHERE L > @r
		UPDATE [%_Node_%] SET R = R - (@w) WHERE R > @r

		DECLARE @newL INT
		IF (@NewParentId) IS NOT NULL
		BEGIN
			DECLARE @pR INT = (SELECT R FROM [%_Node_%] WHERE [%_Id_%] = @NewParentId)
			--uppdatera flyttad nod som om den inte hade några barn
			DECLARE @pNewR INT = @pR+@w
			UPDATE [%_Node_%] SET R = @pNewR WHERE [%_Id_%] = @NewParentId
			SET @newL = @pR
			UPDATE [%_Node_%] SET L = @newL, R = @pNewR-1 WHERE [%_Id_%] = @Id
		
			--uppdatera alla barn o barnbarn till flyttad nod
			UPDATE [%_Node_%] SET L = L + (@newL - @l), R = R + (@newL - @l) WHERE [%_Id_%] IN (SELECT [%_Id_%] FROM @barnbarn)

			--Uppdatera alla noder till höger om flyttad nod
			UPDATE [%_Node_%] SET L = L + @w WHERE L > @pR AND [%_Id_%] NOT IN (SELECT Id FROM @barnbarn) AND [%_Id_%] <> @NewParentId AND [%_Id_%] <> @Id
			UPDATE [%_Node_%] SET R = R + @w WHERE R >= @pR AND [%_Id_%] NOT IN (SELECT Id FROM @barnbarn) AND [%_Id_%] <> @NewParentId AND [%_Id_%] <> @Id
		END
		ELSE
		BEGIN
			DECLARE @maxR INT = (SELECT ISNULL(Max(R),0) FROM [%_Node_%] WHERE [%_Parent_Id_%] IS NULL)
			DECLARE @newR INT = @maxR+@w
			SET @newL = @maxR+1
			UPDATE [%_Node_%] SET L = @newL, R = @newR WHERE [%_Id_%] = @Id

			--uppdatera alla barn o barnbarn till flyttad nod
			UPDATE [%_Node_%] SET L = L + (@newL - @l), R = R + (@newL - @l) WHERE [%_Id_%] IN (SELECT [%_Id_%] FROM @barnbarn)

			--Uppdatera alla noder till höger om flyttad nod
			UPDATE [%_Node_%] SET L = L + @w WHERE L > @maxR AND [%_Id_%] NOT IN (SELECT [%_Id_%] FROM @barnbarn) AND [%_Id_%] <> @NewParentId AND [%_Id_%] <> @Id
			UPDATE [%_Node_%] SET R = R + @w WHERE R >= @maxR AND [%_Id_%] NOT IN (SELECT [%_Id_%] FROM @barnbarn) AND [%_Id_%] <> @NewParentId AND [%_Id_%] <> @Id
		END
		FETCH NEXT FROM db_cursor INTO @Id, @NewParentId
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor
	ALTER TABLE [%_Node_%] WITH CHECK CHECK CONSTRAINT all
	END
GO
