CREATE PROCEDURE [%_Node_%_SetLR]
(
	@Id INT,
	@L INT,
	@Next INT OUTPUT
)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @R INT = 0
	DECLARE @ChildId INT

	UPDATE [%_Node_%] SET L = @L WHERE [%_Id_%] = @Id


	DECLARE db_cursor CURSOR LOCAL FOR 
	SELECT [%_Id_%] FROM [%_Node_%] WHERE [%_Parent_Id_%] = @Id
	OPEN db_cursor
	DECLARE @NextL INT = (@L + 1);
	SET @R = @NextL 

	FETCH NEXT FROM db_cursor INTO @ChildId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC [%_Node_%_SetLR] @ChildId, @NextL, @Next = @R OUT
		SET @NextL = @R
		FETCH NEXT FROM db_cursor INTO @ChildId
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor

	UPDATE [%_Node_%] SET R = @R WHERE [%_Id_%] = @Id

	SET @Next = @R + 1

END
GO



DECLARE @R INT = 0
DECLARE @L INT = 1
DECLARE @Id INT
DECLARE db_cursor CURSOR LOCAL FOR 
SELECT [%_Id_%] FROM [%_Node_%] WHERE [%_Parent_Id_%] IS NULL AND [L] IS NULL
OPEN db_cursor
	
FETCH NEXT FROM db_cursor INTO @Id
WHILE @@FETCH_STATUS = 0
BEGIN
	EXEC [%_Node_%_SetLR] @Id, @L, @Next = @R OUT
		
	SET @L = @R
	FETCH NEXT FROM db_cursor INTO @Id
END
CLOSE db_cursor
DEALLOCATE db_cursor

GO

DROP PROCEDURE [%_Node_%_SetLR]
GO
