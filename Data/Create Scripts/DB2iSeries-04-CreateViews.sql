CREATE VIEW PersonView AS SELECT * FROM Person
GO

CREATE Procedure Person_SelectByKey(in ID integer)
  RESULT SETS 1
  LANGUAGE SQL
  BEGIN
    DECLARE C1 CURSOR FOR
        SELECT * FROM Person WHERE PersonID = ID;

    OPEN C1;
  END
GO
