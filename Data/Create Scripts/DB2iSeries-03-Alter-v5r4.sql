ALTER TABLE InheritanceParent
  ADD COLUMN Name VARCHAR(50) Default NULL
GO

ALTER TABLE InheritanceChild
  ADD COLUMN Name VARCHAR(50) Default NULL
GO

ALTER TABLE Person
  ADD COLUMN FirstName  VARCHAR(50) Default NOT NULL
  ADD COLUMN LastName   VARCHAR(50) Default NOT NULL
  ADD COLUMN MiddleName VARCHAR(50) 
  ADD COLUMN Gender     CHAR(1)     Default NOT NULL
GO

ALTER TABLE TestMerge1 
  ADD COLUMN FieldNString VARCHAR(20)
  ADD COLUMN FieldNChar   CHAR(1)
GO

ALTER TABLE TestMerge2 
  ADD COLUMN FieldNString VARCHAR(20)
  ADD COLUMN FieldNChar   CHAR(1)
GO

ALTER TABLE AllTypes
  ADD COLUMN decfloat16DataType float(16)     Default NULL
  ADD COLUMN decfloat34DataType float(34)     Default NULL
  ADD COLUMN xmlDataType        varchar(9999) Default NULL
GO

ALTER TABLE AllTypes2
  ADD COLUMN decfloat16DataType float(16) Default NULL
  ADD COLUMN decfloat34DataType float(34) Default NULL
GO
