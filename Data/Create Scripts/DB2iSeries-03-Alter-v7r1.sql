ALTER TABLE InheritanceParent
  ADD COLUMN Name NVARCHAR(50) Default NULL
GO

ALTER TABLE InheritanceChild
  ADD COLUMN Name NVARCHAR(50) Default NULL
GO

ALTER TABLE Person
  ADD COLUMN FirstName  NVARCHAR(50) Default NOT NULL
  ADD COLUMN LastName   NVARCHAR(50) Default NOT NULL
  ADD COLUMN MiddleName NVARCHAR(50) 
  ADD COLUMN Gender     NCHAR(1)     Default NOT NULL
GO

ALTER TABLE TestMerge1 
  ADD COLUMN FieldNString NVARCHAR(20)
  ADD COLUMN FieldNChar   NCHAR(1)
GO

ALTER TABLE TestMerge2 
  ADD COLUMN FieldNString NVARCHAR(20)
  ADD COLUMN FieldNChar   NCHAR(1)
GO

ALTER TABLE AllTypes
  ADD COLUMN decfloat16DataType decfloat(16) Default NULL
  ADD COLUMN decfloat34DataType decfloat(34) Default NULL
  ADD COLUMN xmlDataType        xml          Default NULL
GO

ALTER TABLE AllTypes2
  ADD COLUMN decfloat16DataType decfloat(16) Default NULL
  ADD COLUMN decfloat34DataType decfloat(34) Default NULL
GO
