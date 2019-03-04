/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM	[Node]


  WHILE EXISTS (SELECT child.[Id] FROM [Node] child
				LEFT JOIN [Node] parent ON parent.[Id] = child.Parent_Id
				WHERE (parent.L IS NOT NULL AND child.L IS NULL) OR (child.Parent_Id IS NULL AND child.L IS NULL))
BEGIN
	DECLARE @Id INT = 0
	
	DECLARE db_cursor CURSOR FOR 
	SELECT child.Id FROM [Node] child
	LEFT JOIN [Node] parent ON parent.[Id] = child.Parent_Id
	WHERE (parent.L IS NOT NULL AND child.L IS NULL) OR child.Parent_Id IS NULL  
	OPEN db_cursor

	FETCH NEXT FROM db_cursor INTO @Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (SELECT Parent_Id FROM [Node] WHERE [Id] = @Id) IS NOT NULL
		BEGIN
			DECLARE @pR INT = (SELECT R FROM [Node] WHERE [Id] = (SELECT Parent_Id FROM [Node] WHERE [Id] = @Id))
			UPDATE [Node] SET L = L + 2 WHERE L > @pR
			UPDATE [Node] SET R = R + 2 WHERE R >= @pR
			UPDATE [Node] SET L = @pR, R = @pr+1 WHERE Id = @Id
		END 
		ELSE 
		BEGIN
			DECLARE @maxR INT = (SELECT ISNULL(Max(R),0) FROM [Node] WHERE Parent_Id IS NULL)
			UPDATE [Node] SET L = @maxR+1, R = @maxR+2 WHERE Id = @Id
		END

		FETCH NEXT FROM db_cursor INTO @Id
	END
	CLOSE db_cursor
	DEALLOCATE db_cursor
END
