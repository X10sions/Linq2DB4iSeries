INSERT INTO Person (FirstName, LastName, Gender) VALUES ('John',   'Pupkin',    'M')
GO
INSERT INTO Person (FirstName, LastName, Gender) VALUES ('Tester', 'Testerson', 'M')
GO
INSERT INTO Person (FirstName, LastName, Gender) VALUES ('Jane',   'Doe',       'F')
GO
INSERT INTO Person (FirstName, LastName, MiddleName, Gender) VALUES ('Jürgen', 'König', 'Ko', 'M')
GO

INSERT INTO Doctor (PersonID, Taxonomy) VALUES (1, 'Psychiatry')
GO

INSERT INTO Patient(PersonID, Diagnosis) VALUES (2, 'Hallucination with Paranoid Bugs'' Delirium of Persecution')
GO

INSERT INTO AllTypes (bigintDataType) VALUES (NULL)
GO

INSERT INTO AllTypes(
  bigintDataType           
, binaryDataType           
, blobDataType             
, charDataType             
, CharForBitDataType       
, clobDataType             
, dataLinkDataType         
, dateDataType             
, dbclobDataType           
, decfloat16DataType       
, decfloat34DataType       
, decimalDataType          
, doubleDataType           
, graphicDataType          
, intDataType              
, numericDataType          
, realDataType             
, rowIdDataType            
, smallintDataType         
, timeDataType             
, timestampDataType        
, varbinaryDataType        
, varcharDataType          
, varCharForBitDataType    
, varGraphicDataType       
, xmlDataType              
) VALUES (
  1000000                    --bigIntDataType         
, Cast('123' as binary)      --binaryDataType
, Cast('234' as blob)        --blobDataType             
, 'Y'                        --charDataType             
, '123'                      --CharForBitDataType       
, Cast('567' as clob)        --clobDataType             
, DEFAULT                    --dataLinkDataType         
, '2012-12-12'               --dateDataType             
, Cast('890' as dbclob)      --dbclobDataType           
, 888.456                    --decfloat16DataType       
, 777.987                    --decfloat34DataType       
, 666.987                    --decimalDataType          
, 555.987                    --doubleDataType           
, 'graphic'                  -- DEFAULT --Cast('graphic' as graphic) --graphicDataType          gets error when casting the data
, 444444                     --intDataType              
, 333.987                    --numericDataType          
, 222.987                    --realDataType             
, DEFAULT                    --rowIdDataType            
, 100                        --smallintDataType         
, '12:12:12'                 --timeDataType             
, '2012-12-12 12:12:12'      --timestampDataType        
, Cast('456' as binary)      --varbinaryDataType        
, 'var-char'                 --varcharDataType          
, 'vcfb'                     --varCharForBitDataType    
, 'vargraphic'               --varGraphicDataType
, '<root><element strattr="strvalue" intattr="12345"/></root>' --xmlDataType  
)
GO
