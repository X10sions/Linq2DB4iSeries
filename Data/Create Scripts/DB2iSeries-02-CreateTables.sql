CREATE TABLE InheritanceParent(
  InheritanceParentId INTEGER       PRIMARY KEY NOT NULL
, TypeDiscriminator   INTEGER                   Default NULL
--, Name                NVARCHAR(50)               Default NULL
)
GO

CREATE TABLE InheritanceChild(
  InheritanceChildId  INTEGER      PRIMARY KEY NOT NULL
, InheritanceParentId INTEGER                  NOT NULL
, TypeDiscriminator   INTEGER                  Default NULL
--, Name                NVARCHAR(50)              Default NULL
)
GO

CREATE TABLE Person( 
  PersonID   INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL
--, FirstName  NVARCHAR(50)                                      NOT NULL
--, LastName   NVARCHAR(50)                                      NOT NULL
--, MiddleName NVARCHAR(50)             
--, Gender     NCHAR(1)                                          NOT NULL
)
GO

CREATE TABLE Doctor(
  PersonID INTEGER     PRIMARY KEY NOT NULL
, Taxonomy VARCHAR(50)             NOT NULL
, FOREIGN KEY FK_Doctor_Person(PersonID) REFERENCES Person
)
GO

CREATE TABLE MasterTable(
  ID1 INTEGER NOT NULL
, ID2 INTEGER NOT NULL
, PRIMARY KEY (ID1,ID2)
)
GO

CREATE TABLE SlaveTable(
  ID1    INTEGER NOT NULL
, ID2222222222222222222222 INTEGER NOT NULL
, ID2222222222222222           INTEGER NOT NULL
, FOREIGN KEY FK_SlaveTable_MasterTable (ID2222222222222222222222, ID1) REFERENCES MasterTable
)
GO

CREATE TABLE Patient(
  PersonID  INTEGER      PRIMARY KEY NOT NULL
, Diagnosis VARCHAR(256) NOT NULL
, FOREIGN KEY FK_Patient_Person (PersonID) REFERENCES Person
)
GO

CREATE TABLE Parent      (ParentID int, Value1 int)
GO

CREATE TABLE Child       (ParentID int, ChildID int)
GO

CREATE TABLE GrandChild  (ParentID int, ChildID int, GrandChildID int)
GO

CREATE TABLE LinqDataTypes(
  ID             int
, MoneyValue     decimal(10, 4)
, DateTimeValue  timestamp
, DateTimeValue2 timestamp  Default NULL
, BoolValue      smallint
, GuidValue      char(16) for bit DATA
, BinaryValue    blob(5000) Default NULL
, SmallIntValue  smallint
, IntValue       int        Default NULL
, BigIntValue    bigint     Default NULL
, StringValue    VARCHAR(50) Default NULL
)
GO

CREATE TABLE TestMerge1 (
  Id              INTEGER PRIMARY KEY  not null
, Field1          INTEGER                      
, Field2          INTEGER                      
, Field3          INTEGER                      
, Field4          INTEGER                      
, Field5          INTEGER                      

, FieldInt64      BIGINT                       
, FieldBoolean    SMALLINT                     
, FieldString     VARCHAR(20)                  
--, FieldNString    NVARCHAR(20)                  
, FieldChar       CHAR(1)                      
--, FieldNChar      NCHAR(1)                      
, FieldFloat      REAL                         
, FieldDouble     DOUBLE                       
, FieldDateTime   TIMESTAMP                    

, FieldBinary     VARCHAR(20)  FOR BIT DATA    
, FieldGuid       CHAR(16)     FOR BIT DATA    

, FieldDecimal    DECIMAL(24, 10)              
, FieldDate       DATE                         
, FieldTime       TIME                         
, FieldEnumString VARCHAR(20)                  
, FieldEnumNumber INT                    
)
GO

CREATE TABLE  TestMerge2 (
  Id        INTEGER            PRIMARY KEY not null
, Field1    INTEGER                            
, Field2    INTEGER                            
, Field3    INTEGER                            
, Field4    INTEGER                            
, Field5    INTEGER                            

, FieldInt64       BIGINT                      
, FieldBoolean     SMALLINT                    
, FieldString      VARCHAR(20)                 
--, FieldNString     NVARCHAR(20)                 
, FieldChar        CHAR(1)                     
--, FieldNChar       NCHAR(1)                               
, FieldFloat       REAL                        
, FieldDouble      DOUBLE                      
, FieldDateTime    TIMESTAMP                   
, FieldBinary      VARCHAR(20)  FOR BIT DATA   
, FieldGuid        CHAR(16)     FOR BIT DATA   
, FieldDecimal     DECIMAL(24, 10)             
, FieldDate        DATE                        
, FieldTime        TIME                        
, FieldEnumString  VARCHAR(20)                 
,  FieldEnumNumber  INT                      
)
GO

CREATE TABLE TestIdentity (
  ID   INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL
)
GO

CREATE TABLE AllTypes(
  ID INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL
, bigintDataType        bigint                     Default NULL
, binaryDataType        binary(20)                 Default NULL
, blobDataType          blob                       Default NULL
, charDataType          char(1)                    Default NULL
, char20DataType        char(20)    CCSID 1208     Default NULL
, CharForBitDataType    char(5) for bit data       Default NULL
, clobDataType          clob        CCSID 1208     Default NULL
, dataLinkDataType      dataLink                   Default NULL
, dateDataType          date                       Default NULL
, dbclobDataType        dbclob(100)                Default NULL
--, decfloat16DataType    decfloat(16)               Default NULL 
--, decfloat34DataType    decfloat(34)               Default NULL 
, decimalDataType       decimal(30)                Default NULL
, doubleDataType        double                     Default NULL
, graphicDataType       graphic(10) ccsid 13488    Default NULL
, intDataType           int                        Default NULL
, numericDataType       numeric                    Default NULL
, realDataType          real                       Default NULL
, rowIdDataType         rowId                              
, smallintDataType      smallint                   Default NULL
, timeDataType          time                       Default NULL
, timestampDataType     timestamp                  Default NULL
, varbinaryDataType     varbinary(20)              Default NULL
, varcharDataType       varchar(20)                Default NULL
, varCharForBitDataType varchar(5) for bit data    Default NULL
, varGraphicDataType    vargraphic(10) ccsid 13488 Default NULL
--, xmlDataType         xml              Default NULL 
)
GO

CREATE TABLE KeepIdentityTest (
	ID    INTEGER  GENERATED ALWAYS AS IDENTITY PRIMARY KEY not null,
	intDataType INTEGER  
)
GO

CREATE TABLE AllTypes2(
  ID INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY NOT NULL
, bigintDataType           bigint                  Default NULL
, binaryDataType           binary(20)              Default NULL
, charDataType             char(1)                 Default NULL
, char20DataType           char(20)     CCSID 1208 Default NULL
, CharForBitDataType       char(5) for bit data    Default NULL
, dataLinkDataType         dataLink                Default NULL
, dateDataType             date                    Default NULL
--, decfloat16DataType       decfloat(16)            Default NULL
--, decfloat34DataType       decfloat(34)            Default NULL
, decimalDataType          decimal(30)             Default NULL
, doubleDataType           double                  Default NULL
, graphicDataType          graphic(10) CCSID 13488 Default NULL
, intDataType              int                     Default NULL
, numericDataType          numeric                 Default NULL
, realDataType             real                    Default NULL
, rowIdDataType            rowId                              
, smallintDataType         smallint                Default NULL
, timeDataType             time                    Default NULL
, timestampDataType        timestamp               Default NULL
, varbinaryDataType        varbinary(20)           Default NULL
, varcharDataType          varchar(20)             Default NULL
, varCharForBitDataType    varchar(5) for bit data Default NULL
, varGraphicDataType       vargraphic(10) CCSID 13488 Default NULL
)
GO
