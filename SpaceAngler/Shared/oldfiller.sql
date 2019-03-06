WHILE EXISTS (SELECT child.[%_Id_%] FROM [%_Node_%] child
			  LEFT JOIN [%_Node_%] parent ON parent.[%_Id_%] = child.[%_Parent_Id_%]
			  WHERE (parent.L IS NOT NULL AND child.L IS NULL) OR (child.[%_Parent_Id_%] IS NULL AND child.L IS NULL))
BEGIN
	DECLARE @Id INT = 0
	
	DECLARE db_cursor CURSOR FOR 
	SELECT child.Id FROM [%_Node_%] child
	LEFT JOIN [%_Node_%] parent ON parent.[%_Id_%] = child.[%_Parent_Id_%]
	WHERE (parent.L IS NOT NULL AND child.L IS NULL) OR (child.[%_Parent_Id_%] IS NULL AND child.L IS NULL)
	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (SELECT [%_Parent_Id_%] FROM [%_Node_%] WHERE [%_Id_%] = @Id) IS NOT NULL
		BEGIN
			DECLARE @pR INT = (SELECT R FROM [%_Node_%] WHERE [%_Id_%] = (SELECT [%_Parent_Id_%] FROM [%_Node_%] WHERE [%_Id_%] = @Id))
			UPDATE [%_Node_%] SET L = L + 2 WHERE L > @pR
			UPDATE [%_Node_%] SET R = R + 2 WHERE R >= @pR
			UPDATE [%_Node_%] SET L = @pR, R = @pr + 1 WHERE Id = @Id
		END 
		ELSE 
		BEGIN
			DECLARE @maxR INT = (SELECT ISNULL(Max(R),0) FROM [%_Node_%] WHERE [%_Parent_Id_%] IS NULL)
			UPDATE [%_Node_%] SET L = @maxR + 1, R = @maxR + 2 WHERE [%_Id_%] = @Id
		END

		FETCH NEXT FROM db_cursor INTO @Id
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor
END
