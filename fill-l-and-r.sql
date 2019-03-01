-- Fill Left and Right

WHILE EXISTS (SELECT child.Id FROM [Node] child
				LEFT JOIN [Node] parent ON parent.Id = child.Parent_Id
				WHERE (parent.L IS NOT NULL AND child.L IS NULL) OR (child.Parent_Id IS NULL AND child.L IS NULL))
BEGIN
	DECLARE @Id INT = 0
	
	DECLARE db_cursor CURSOR FOR 
	SELECT child.Id FROM [Node] child
	LEFT JOIN [Node] parent ON parent.Id = child.Parent_Id
	WHERE (parent.L IS NOT NULL AND child.L IS NULL) OR child.Parent_Id IS NULL  

	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE  @parentLeft INT = (SELECT L from [Node] WHERE Id = (SELECT Parent_Id FROM [Node] WHERE Id = @Id))
		DECLARE  @parentRight INT = (SELECT R from [Node] WHERE Id = (SELECT Parent_Id FROM [Node] WHERE Id = @Id))
		UPDATE [Node] SET R = R + 2 WHERE R > @parentLeft AND Id != @Id
		UPDATE [Node] SET L = L + 2 WHERE L > @parentLeft AND Id != @Id
		UPDATE [Node] SET L = ISNULL(@parentLeft,0) + 1, R = ISNULL(@parentLeft,0) + 2 WHERE Id = @Id
		FETCH NEXT FROM db_cursor INTO @Id ;
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor
END
