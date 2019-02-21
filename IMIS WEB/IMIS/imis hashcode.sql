IF  COL_LENGTH('tblUsers','StoredPassword') IS NULL
ALTER TABLE tblUsers ADD StoredPassword nvarchar(256) NULL
GO
IF  COL_LENGTH('tblUsers','PrivateKey') IS NULL
ALTER TABLE tblUsers ADD PrivateKey nvarchar(256) NULL
GO
IF  COL_LENGTH('tblUsers','PasswordValidity') IS NULL
ALTER TABLE tblUsers ADD PasswordValidity DateTime NULL
GO
OPEN SYMMETRIC KEY EncryptionKey DECRYPTION BY Certificate EncryptData;
UPDATE tblUsers 
SET Privatekey =
CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST(LoginName AS VARCHAR(MAX))),2),
StoredPassword =
CONVERT(varchar(max),HASHBYTES('SHA2_256',
	CONCAT
		(
		CAST(CONVERT(NVARCHAR(25), DECRYPTBYKEY(Password)) COLLATE LATIN1_GENERAL_CS_AS AS VARCHAR(MAX))
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST(LoginName AS VARCHAR(MAX))),2)
		)
	),2)
FROM tblusers  
WHERE ValidityTo is null  
			  

 CLOSE SYMMETRIC KEY EncryptionKey


