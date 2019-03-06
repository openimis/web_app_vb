
DROP TABLE TBLROLERIGHT
DROP TABLE TBLUSERROLE
DROP TABLE TBLROLE


IF  COL_LENGTH('tblOfficer','HasLogin') IS NULL
	ALTER TABLE tblOfficer Add HasLogin BIT NULL
IF  COL_LENGTH('tblClaimAdmin','HasLogin') IS NULL
	ALTER TABLE tblClaimAdmin Add HasLogin BIT NULL
IF COL_LENGTH('tblUsers','IsAssociated') IS NULL
	ALTER TABLE tblUsers Add IsAssociated BIT NULL

IF OBJECT_ID('tblRole') IS NULL
	BEGIN	
		CREATE TABLE [dbo].[tblRole](
			[RoleID] [int] IDENTITY(1,1) NOT NULL,
			[RoleName] [nvarchar](50) NOT NULL,
			[IsSystem] [int] NOT NULL,
			[IsBlocked] [bit] NOT NULL,
			[ValidityFrom] [datetime] NOT NULL,
			[ValidityTo] [datetime] NULL,
			[AuditUserID] [int] NULL,
			[LegacyID] [int] NULL
		 CONSTRAINT [PK_tblRole] PRIMARY KEY CLUSTERED 
		(
			[RoleID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	END
GO
IF OBJECT_ID('tblRoleRight') IS NULL
	BEGIN
		CREATE TABLE [dbo].[tblRoleRight](
			[RoleRightID] [int] IDENTITY(1,1) NOT NULL,
			[RoleID] [int] NOT NULL,
			[RightID] [int] NOT NULL,
			[ValidityFrom] [datetime] NOT NULL,
			[ValidityTo] [datetime] NULL,
			[AuditUserId] [int] NULL,
			[LegacyID] [int] NULL,
		 CONSTRAINT [PK_tblRoleRight] PRIMARY KEY CLUSTERED 
		(
			[RoleRightID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	END
GO
IF  Object_id('FK_tblRoleRight_tblRole') is null
	ALTER TABLE [dbo].[tblRoleRight]  WITH CHECK ADD  CONSTRAINT [FK_tblRoleRight_tblRole] FOREIGN KEY([RoleID])
	REFERENCES [dbo].[tblRole] ([RoleID])



ALTER TABLE [dbo].[tblRoleRight] CHECK CONSTRAINT [FK_tblRoleRight_tblRole]
GO
IF OBJECT_ID('tblUserRole') IS NULL

CREATE TABLE tblUserRole
(	UserRoleID INT not null IDENTITY(1,1),
	UserID INT NOT NULL,
	RoleID int NOT null,
	ValidityFrom datetime NOT NULL,
	ValidityTo datetime NULL,
	AudituserID INT NULL,
	LegacyID INT NULL
	CONSTRAINT PK_tblUserRole PRIMARY KEY (UserRoleID),
	CONSTRAINT FK_tblUserRole_tblUsers FOREIGN KEY (UserID) REFERENCES tblUsers(UserID) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT FK_tblUserRole_tblRole FOREIGN KEY (RoleID) REFERENCES tblRole (RoleID) ON DELETE CASCADE ON UPDATE CASCADE
)
GO
IF (SELECT 1 FROM tblRole WHERE RoleName = 'Enrolement Officer') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('Enrolement Officer',1,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'Manager') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('Manager',2,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'Accountant') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('Accountant',4,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'Clerk') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('Clerk',8,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'Medical Officer') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('Medical Officer',16,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'Administrator') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('Administrator',32,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'IMIS Administrator') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('IMIS Administrator',64,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'Receptionist') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('Receptionist',128,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'Claim Administrator') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('Claim Administrator',256,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'Claim Contributor') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('Claim Contributor',512,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'HF Administrator') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('HF Administrator',524288,GETDATE(),0)
IF (SELECT 1 FROM tblRole WHERE RoleName = 'Offline Administrator') IS NULL
	INSERT INTO tblRole
	(RoleName,IsSystem,ValidityFrom,IsBlocked)
	VALUES('Offline Administrator',1048576,GETDATE(),0)

GO
-- Insert into Roles

				
DECLARE @ID INT
							-- Accountant--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Accountant'
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101101,GETDATE()) --InsureeSearch

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101102 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101102,GETDATE()) --InsureeAdd

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101103,GETDATE()) --InsureeEdit

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101104 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101104,GETDATE()) --InsureeDelete

--Insuree Enquire
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --InsureeEnquire

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101201 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101201,GETDATE()) --PolicySearch

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101202 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101202,GETDATE()) --PolicyAdd

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101203 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101203,GETDATE()) --PolicyEdit

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101204 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101204,GETDATE()) --PolicyDelete

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101205 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101205,GETDATE()) --PolicyRenew

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101301 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101301,GETDATE()) --ContributionSearch

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101302 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101302,GETDATE()) --ContributionAdd

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101303 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101303,GETDATE()) --ContributionEdit 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101401 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101401,GETDATE()) --FindPayment

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101402 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101402,GETDATE()) --AddPayment

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101403 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101403,GETDATE()) --EditPayment

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101404 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101404,GETDATE()) --DeletePayment

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111005 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111005,GETDATE()) --LoadClaim
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111006 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111006,GETDATE()) --PrintClaim
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111008 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111008,GETDATE()) --ReviewClaim
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131201 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131201,GETDATE()) --PrimaryOperationalIndicators-policies
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131207 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131207,GETDATE()) --UserActivityReport 

/* Previous rights*/
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101001,GETDATE())  --FindFamily  

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131401 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131401,GETDATE()) --AddFund
/*previous rights*/
					--END Accountant--
					

				-- START Administrator-- 
SELECT @ID = RoleID from tblRole WHERE Rolename ='Administrator'
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --EnquireInsuree
 
--Primary Operational Indicators-policies
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131201 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131201,GETDATE()) --PrimaryOperationalIndicatorspolicies

--Insurees without Photos
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131210 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131210,GETDATE()) --InsureeswithoutPhotos 

--Backup
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131301 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131301,GETDATE()) --Backup

--Restore
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131302 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131302,GETDATE()) --Restore

--Execute Script
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131303 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131303,GETDATE()) --ExecuteScript 

--Email Settings
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131304 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131304,GETDATE()) --EmailSettings 

/* Previous rights */ 
--Payment
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121401 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121401,GETDATE()) --FindPayment
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121402 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121402,GETDATE()) --AddPayment
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121403 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121403,GETDATE()) --EditPayment
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121404 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121404,GETDATE()) --DeletePayment 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121501 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121501,GETDATE()) --FindOfficer
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121502 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121502,GETDATE()) --AddOfficer
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121503 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121503,GETDATE()) --EditOfficer
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121504 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121504,GETDATE()) --DeleteOfficer

--Claim Administrator
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121601 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121601,GETDATE()) --FindClaimAdministrator
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121602 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121602,GETDATE()) --AddClaimAdministrator
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121603 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121603,GETDATE()) --EditClaimAdministrator
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121604 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121604,GETDATE()) --DeleteClaimAdministrator

--Product
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121001,GETDATE()) --FindProduct
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121002,GETDATE()) --AddProduct
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121003,GETDATE()) --EditProduct
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121004 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121004,GETDATE()) --DeleteProduct 
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121005 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121005,GETDATE()) --DuplicateProduct

--Pricelist-Medical Services
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121201 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121201,GETDATE()) --FindPriceListMedicalServices
-- 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121202 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121202,GETDATE()) --AddPriceListMedicalServices
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121203 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121203,GETDATE()) --EditPriceListMedicalServices
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121204 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121204,GETDATE()) --DeletePriceListMedicalServices
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121205 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121205,GETDATE()) --DuplicatePriceListMedicalServices

--Pricelist Medical Items 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121301 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121301,GETDATE()) --FindPriceListMedicalItems
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121302 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121302,GETDATE()) --AddPriceListMedicalItems
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121303 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121303,GETDATE()) --EditPriceListMedicalItems
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121304 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121304,GETDATE()) --DeletePriceListMedicalItems
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121305 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121305,GETDATE()) --DuplicatePriceListMedicalItems

--Payer
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121801 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121801,GETDATE()) --FindPayer
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121802 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121802,GETDATE()) --AddPayer
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121803 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121803,GETDATE()) --EditPayer
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121804 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121804,GETDATE()) --DeletePayer 

--Health Facility
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121101,GETDATE()) --FindHealthFacility
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121102 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121102,GETDATE()) --AddHealthFacility
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121103,GETDATE()) --EditHealthFacility
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121104 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121104,GETDATE()) --DeleteHealthFacility

--Medical Services
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121401 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121401,GETDATE()) --FindMedicalServices
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121402 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121402,GETDATE()) --AddMedicalServices
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121403 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121403,GETDATE()) --EditMedicalServices
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121404 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121404,GETDATE()) --DeleteMedicalServices

--Medical Item
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122101,GETDATE()) --FindMedicalItem
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122102 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122102,GETDATE()) --AddMedicalItem
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122103,GETDATE()) --EditMedicalItem
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122104 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122104,GETDATE()) --DeleteMedicalItem 

--Locations
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121901 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121901,GETDATE()) --FindLocations 
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121902 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121902,GETDATE()) --AddLocations 
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121903 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121903,GETDATE()) --EditLocations 
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121904 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121904,GETDATE()) --DeleteLocations
/*Previous rights*/
					--END Administrator-- 

					--START Manager--					 
SELECT @ID = RoleID from tblRole WHERE Rolename ='Manager'
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --EnquireInsurees 
 
--Status of Registers
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131209 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131209,GETDATE()) --StatusofRegisters 
					--END Manager--


					--START Receptionist--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Receptionist'
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --InsureeEnquire  
					--END Receptionist--


					--START Claim Administrator--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Claim Administrator' 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111006 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111006,GETDATE()) --PrintClaim
/* Previous rights */
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111001,GETDATE()) --FindClaim
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111002,GETDATE()) --EnterClaim
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111004 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111004,GETDATE()) --DeleteClaim 
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111005 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111005,GETDATE()) --LoadClaim

 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131214 AND ROLEID = @ID) IS NULL 
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131214,GETDATE()) --PercentageReferrals
/*Previous rights*/
					--END Claim Administrator--


					--START HF Administrator--
SELECT @ID = RoleID from tblRole WHERE Rolename ='HF Administrator'
--User
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121701 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121701,GETDATE()) --FindUser
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121702 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121702,GETDATE()) --AddUser
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121703 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121703,GETDATE()) --EditUser
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121704 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121704,GETDATE()) --DeleteUser

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131101,GETDATE()) --Extract 

--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131103,GETDATE()) --OfflineExtractCreate
 
--Backup
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131301 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131301,GETDATE()) --Backup 

--Restore
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131302 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131302,GETDATE()) --Restore

--Execute Script
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131303 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131303,GETDATE()) --ExecuteScript

--Email Settings
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131304 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131304,GETDATE()) --EmailSettings
					--END HF Administrator--



					--START Offline Administrator--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Offline Administrator'
--User
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121701 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121701,GETDATE()) --FindUser
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121702 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121702,GETDATE()) --AddUser
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121703 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121703,GETDATE()) --EditUser

--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121704 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121704,GETDATE()) --DeleteUser

--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131101,GETDATE()) --Extract 

--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131103,GETDATE()) --OfflineExtractCreate

--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131301 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131301,GETDATE()) --Backup

--Restore
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131302 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131302,GETDATE()) --Restore
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131303 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131303,GETDATE()) --ExecuteScript
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131304 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131304,GETDATE()) --EmailSettings
					--END Offline Administrator--


							--Clerk--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Clerk' 
--Insuree
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101101,GETDATE()) --FindInsuree  

--Policy
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101201 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101201,GETDATE()) --FindPolicy 
 
--Payment
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101401 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101401,GETDATE()) --FindPayment
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131104 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131104,GETDATE()) --ClaimUpload 

/*Previous rights */
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101001,GETDATE()) --FindFamily
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101002,GETDATE()) --AddFamily
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101003,GETDATE()) --EditFamily
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101004 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101004,GETDATE()) --DeleteFamily

--Insuree
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101101,GETDATE()) --FindInsuree
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101102 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101102,GETDATE()) --AddInsuree
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101103,GETDATE()) --EditInsuree
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101104 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101104,GETDATE()) --DeleteInsuree 
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101202 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101202,GETDATE()) --AddPolicy
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101203 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101203,GETDATE()) --EditPolicy
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101204 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101204,GETDATE()) --DeletePolicy 

--Product
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121001,GETDATE()) --ViewProduct

--Payer
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121801 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121801,GETDATE()) --ViewPayer

--Policy
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101205 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101205,GETDATE()) --RenewPolicy 

--Contribution
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101301 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101301,GETDATE()) --FindContribution 
/* Previous rights */ 
							--END Clerk--



							--Medical Officer--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Medical Officer'
--ReviewClaim 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111008 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111008,GETDATE()) --ReviewClaim
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111009 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111009,GETDATE()) --EnterFeedback
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111010 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111010,GETDATE()) --UpdateClaims
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111011 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111011,GETDATE()) --ProcessClaims 

----ClaimOverview
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111015 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111015,GETDATE()) --ClaimOverview

--Claim History Report
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131223 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131223,GETDATE()) --ClaimHistoreReport 
					--END Medical Officer


					--START IMIS Administrator--
SELECT @ID = RoleID from tblRole WHERE Rolename ='IMIS Administrator' 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --EnquireInsuree

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131207 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131207,GETDATE()) --UserActivityReport  

/*Previous rights */
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121701 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121701,GETDATE()) --FindUser
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121702 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121702,GETDATE()) --AddUser
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121703 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121703,GETDATE()) --EditUser
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121704 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121704,GETDATE()) --DeleteUser

--User profile
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122001,GETDATE()) --FindUserProfile
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122002,GETDATE()) --Add Userprofile
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122003,GETDATE()) --EditUserProfile
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122004 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122004,GETDATE()) --DeleteUserProfile
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122005 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122005,GETDATE()) --duuplicateUserProfile

--Database
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131301 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131301,GETDATE()) --DatabaseBackup
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131302 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131302,GETDATE()) --DatabaseRestore
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131303 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131303,GETDATE()) --ExecuteScripts 
--Email Settings
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131304 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131304,GETDATE()) --EmailSettings 
							/* Previous rights */
 					--END IMIS Administrator--



					--START Claim Contributor--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Claim Contributor'
--Claim
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111001,GETDATE()) --FindClaim
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111002,GETDATE()) --EnterClaim
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111005 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111005,GETDATE()) --LoadClaim
					--END Claim Contributor-- 



					--START Enrolement Officer--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Enrolement Officer' 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101002,GETDATE()) --FamilyAdd 
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101003,GETDATE()) --FamilyEdit  
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101004 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101004,GETDATE()) --FamilyDelete  

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101102 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101102,GETDATE()) --InsureeAdd  

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101103,GETDATE()) --InsureeEdit  

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101104 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101104,GETDATE()) --InsureeDelete 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --InsureeEnquire 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101202 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101202,GETDATE()) --PolicyAdd 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101203 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101203,GETDATE()) --PolicyEdit 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101204 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101204,GETDATE()) --PolicyDelete 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101205 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101205,GETDATE()) --PolicyRenewal 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101302 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101302,GETDATE()) --ContributionAdd 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101303 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101303,GETDATE()) --ContributionEdit 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101304 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101304,GETDATE()) --ContributionDelete 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111009 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111009,GETDATE()) --ClaimFeedback 
							--End Enrolment Officer--


 --EnrolementOfficer = 1
 --       CHFManager = 2
 --       CHFAccountant = 4
 --       CHFClerk = 8
 --       CHFMedicalOfficer = 16
 --       CHFAdministrator = 32
 --       IMISAdministrator = 64
 --       Receptionist = 128
 --       ClaimAdministrator = 256
 --       ClaimContributor = 512

 --       HFAdministrator = 524288
 --       OfflineCHFAdministrator = 1048576
 DECLARE @AuditUserID INT = 0
 DECLARE @LegacyRoleID INT
 DECLARE @UserID INT
 DECLARE @NewRoleID INT

 SELECT @AuditUserID = UserID FROM tblUsers WHERE loginName = 'Admin'

 DECLARE User_Cursor CURSOR FOR
 SELECT UserID,RoleID FROM tblUsers where validityto is null

OPEN User_Cursor
FETCH NEXT FROM User_Cursor INTO @UserID,@LegacyRoleID

WHILE @@FETCH_STATUS = 0
BEGIN
IF @LegacyRoleID & 1 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='Enrolement Officer'
	IF @NewRoleID > 0
		BEGIN
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
		END
END
IF @LegacyRoleID & 2 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='Manager'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END
IF @LegacyRoleID & 4 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='Accountant'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END
IF @LegacyRoleID & 8 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='Clerk'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END
IF @LegacyRoleID & 16 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='Medical Officer'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END
IF @LegacyRoleID & 32 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='Administrator'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END
IF @LegacyRoleID & 64 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='IMIS Administrator'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END
IF @LegacyRoleID & 128 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='Receptionist'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END
IF @LegacyRoleID & 256 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='Claim Administrator'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END
IF @LegacyRoleID & 512 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='Claim Contributer'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END

IF @LegacyRoleID & 524288 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='HF Administrator'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END
IF @LegacyRoleID & 1048576 > 0 
BEGIN
	SELECT @NewRoleID = RoleID from tblRole WHERE Rolename ='Offline Administrator'
	IF @NewRoleID > 0
		BEGIN
			
			IF (SELECT userID FROM tblUserRole WHERE Userid = @UserID AND RoleID = @NewRoleID AND ValidityTo IS NULL) IS NULL
				
				INSERT INTO tblUserRole (USERID,RoleID,ValidityFrom,AudituserID)
				VALUES(@UserID,@NewRoleID,GETDATE(),@AuditUserID)
			
		END
END
FETCH NEXT FROM User_Cursor INTO @UserID,@LegacyRoleID
END
CLOSE User_Cursor
DEALLOCATE User_Cursor
GO
 

--//2nd Emmanuel
IF COL_LENGTH('tblReporting', 'OfficerID') IS NULL 
ALTER TABLE tblReporting ADD OfficerID INT NULL 
 
IF COL_LENGTH('tblReporting','ReportType') IS NULL 
ALTER TABLE tblReporting ADD ReportType INT NULL
 
IF COL_LENGTH('tblPremium','ReportingCommisionID') IS NULL 
ALTER TABLE tblPremium DROP COLUMN ReportingCommisionID 
ELSE
ALTER TABLE tblPremium ADD ReportingCommissionID INT NULL
GO 

-- [ SSRS get matching funds ]
GO
 IF NOT OBJECT_ID('uspSSRSGetMatchingFunds') IS NULL
 DROP PROCEDURE uspSSRSGetMatchingFunds
 GO
CREATE PROCEDURE [dbo].[uspSSRSGetMatchingFunds]
(
	@LocationId INT = NULL, 
	@ProdId INT = NULL,
	@PayerId INT = NULL,
	@StartDate DATE = NULL,
	@EndDate DATE = NULL,
	@ReportingId INT = NULL,
	@ErrorMessage NVARCHAR(200) = N'' OUTPUT
)
AS
BEGIN

/*=======Return Values
	0	= All OK
   -1	= Error Occured
	1	= District is missing
	2	= Product is missing
	3	= StartDate is missing
	4	= End Date is missing
	
*/

/*=======ReportType

	1	= Matching Fund Report
	2	= Overview Of Commissions Report

*/

	DECLARE @RecordFound INT = 0

	--Create new entries only if reportingId is not provided

	IF @ReportingId IS NULL
	BEGIN

		IF @LocationId IS NULL RETURN 1;
		IF @ProdId IS NULL RETURN 2;
		IF @StartDate IS NULL RETURN 3;
		IF @EndDate IS NULL RETURN 4;
		
		BEGIN TRY
			BEGIN TRAN
				--Insert the entry into the reporting table
				INSERT INTO tblReporting(ReportingDate,LocationId, ProdId, PayerId, StartDate, EndDate, RecordFound,OfficerID,ReportType)
				SELECT GETDATE(),@LocationId, @ProdId, @PayerId, @StartDate, @EndDate, 0,null,1;

				--Get the last inserted reporting Id
				SELECT @ReportingId =  SCOPE_IDENTITY();

	
				--Update the premium table with the new reportingid

				UPDATE tblPremium SET ReportingId = @ReportingId
				WHERE PremiumId IN (
				SELECT Pr.PremiumId--,Prod.ProductCode, Prod.ProductName, D.DistrictName, W.WardName, V.VillageName, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName, 
				--Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount, Payer.PayerName
				FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID
				INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
				INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
				INNER JOIN tblVillages V ON V.VillageId = F.LocationId
				INNER JOIN tblWards W ON W.WardId = V.WardId
				INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
				LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
				left join tblReporting ON PR.ReportingId = tblReporting.ReportingId AND tblReporting.ReportType=1
				WHERE Pr.ValidityTo IS NULL 
				AND PL.ValidityTo IS NULL
				AND Prod.ValidityTo IS NULL
				AND F.ValidityTo IS NULL
				AND D.ValidityTo IS NULL
				AND W.ValidityTo IS NULL
				AND V.ValidityTo IS NULL
				AND Payer.ValidityTo IS NULL

				AND D.DistrictID = @LocationId
				AND PayDate BETWEEN @StartDate AND @EndDate
				AND Prod.ProdID = @ProdId
				AND (ISNULL(Payer.PayerID,0) = ISNULL(@PayerId,0) OR @PayerId IS NULL)
				AND Pr.ReportingId IS NULL
				AND PR.PayType <> N'F'
				)

				SELECT @RecordFound = @@ROWCOUNT;

				UPDATE tblReporting SET RecordFound = @RecordFound WHERE ReportingId = @ReportingId;

			COMMIT TRAN;
		END TRY
		BEGIN CATCH
			SELECT @ErrorMessage = ERROR_MESSAGE();
			ROLLBACK;
			RETURN -1
		END CATCH
	END
	
	SELECT Pr.PremiumId,Prod.ProductCode, Prod.ProductName,F.FamilyID, D.DistrictName, W.WardName, V.VillageName, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName, 
	Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount, Payer.PayerName
	FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID
	INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
	INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
	INNER JOIN tblVillages V ON V.VillageId = F.LocationId
	INNER JOIN tblWards W ON W.WardId = V.WardId
	INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
	INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
	LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
	WHERE Pr.ReportingId = @ReportingId
	ORDER BY PremiumId DESC, IsHead DESC;

	SET @ErrorMessage = N''

	RETURN 0;

END
 GO

 --//3rd
GO
IF NOT OBJECT_ID('uspSSRSOverviewOfCommissions') IS NULL
DROP PROCEDURE uspSSRSOverviewOfCommissions

GO
CREATE PROCEDURE [dbo].[uspSSRSOverviewOfCommissions]
(
    @Month INT,
    @Year INT, 
	@Mode INT=NULL,
	@OfficerId INT =NULL,
    @LocationId INT, 
	@ProdId INT = NULL,
	@PayerId INT = NULL,
	@ReportingId INT = NULL,
	@CommissionRate DECIMAL(18,2) = NULL,
	@ErrorMessage NVARCHAR(200) = N'' OUTPUT
)
AS
BEGIN

   --   ReportType
	  --1 = OverviewCommissions report
	  --2 = MatchingFund report
	  


      DECLARE @RecordFound INT = 0

	--Create new entries only if reportingId is not provided

	  IF @ReportingId IS NULL

        BEGIN

	    IF @LocationId IS NULL RETURN 1;
		IF @ProdId IS NULL RETURN 2;
		--IF @StartDate IS NULL RETURN 3;
		--IF @EndDate IS NULL RETURN 4;
	
		
		BEGIN TRY
				BEGIN TRAN

			  
				DECLARE @FirstDay DATE = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01'; 
	            DECLARE @LastDay DATE = EOMONTH(CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01', 0)
			    INSERT INTO tblReporting(ReportingDate,LocationId, ProdId, PayerId, StartDate, EndDate, RecordFound,OfficerID,ReportType)
			
				SELECT GETDATE(),@LocationId, @ProdId, @PayerId, @FirstDay, @LastDay, 0,@OfficerId,2; 
				--Get the last inserted reporting Id
				SELECT @ReportingId =  SCOPE_IDENTITY();

	
				

				UPDATE tblPremium SET ReportingCommissionID = @ReportingId
				WHERE PremiumId IN (
                SELECT Pr.PremiumId
				FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID
				INNER JOIN tblOfficer O ON O.OfficerID = PL.OfficerID
				INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
				INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
				INNER JOIN tblVillages V ON V.VillageId = F.LocationId
				INNER JOIN tblWards W ON W.WardId = V.WardId
				INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
				LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
				left join tblReporting ON PR.ReportingCommissionID = tblReporting.ReportingId AND tblReporting.ReportType=2

				WHERE Pr.ValidityTo IS NULL 
				AND PL.ValidityTo IS NULL
				AND Prod.ValidityTo IS NULL
				AND F.ValidityTo IS NULL
				AND D.ValidityTo IS NULL
				AND W.ValidityTo IS NULL
				AND V.ValidityTo IS NULL
				AND Payer.ValidityTo IS NULL
				
				AND D.DistrictID = @LocationId
				AND PayDate BETWEEN @FirstDay AND @LastDay
				AND Prod.ProdID = @ProdId
			    AND (ISNULL(O.OfficerID,0) = ISNULL(@OfficerId,0) OR @OfficerId IS NULL)
				AND (ISNULL(Payer.PayerID,0) = ISNULL(@PayerId,0) OR @PayerId IS NULL)
				AND tblReporting.ReportingId IS NULL
				AND PR.PayType <> N'F'
				)

				SELECT @RecordFound = @@ROWCOUNT;

				UPDATE tblReporting SET RecordFound = @RecordFound WHERE ReportingId = @ReportingId;

			COMMIT TRAN;
		END TRY
		BEGIN CATCH
			SELECT @ErrorMessage = ERROR_MESSAGE();
			ROLLBACK;
			RETURN -1
		END CATCH
	  END
	       
		
				IF @Mode =0
				BEGIN
		        
					SELECT Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName Product,  PL.PolicyID, F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName,O.Code + ' ' + O.LastName Officer,
					Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount,Pr.Amount as TotlaPrescribedContribution,  PD.Amount TotlActualPayment,Payer.PayerName,PY.PaymentDate,(@CommissionRate / 100) AS CommissionRate,PY.ExpectedAmount PaymentAmount
					FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID AND PL.PolicyStatus=1 OR PL.PolicyStatus=2
					INNER JOIN tblPaymentDetails PD ON PD.PremiumID = Pr.PremiumId
					INNER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID
					INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
					INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
					INNER JOIN tblVillages V ON V.VillageId = F.LocationId
					INNER JOIN tblWards W ON W.WardId = V.WardId
					INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
					INNER JOIN tblOfficer O ON O.LocationId = D.DistrictId
					INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
					LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
					WHERE Pr.ReportingCommissionID = @ReportingId
					--AND Pr.PayDate BETWEEN @FirstDay AND @LastDay
					GROUP BY Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName , PL.PolicyID ,  F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames ,O.Code + ' ' + O.LastName ,
					Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,Pr.Amount,Pr.Amount, PD.Amount , Payer.PayerName,PY.PaymentDate, PY.ExpectedAmount 

				END
				IF @Mode = 1

				BEGIN
					SELECT TOP 1 Amount FROM tblPremium WHERE PolicyID = 1 ORDER BY PremiumId DESC
					SELECT Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName Product,  PL.PolicyID, F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName,O.Code + ' ' + O.LastName Officer,
					Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount,Pr.Amount as TotlaPrescribedContribution, PD.Amount TotlActualPayment, Payer.PayerName,PY.PaymentDate,(@CommissionRate / 100) AS CommissionRate,PY.ExpectedAmount PaymentAmount
					FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID AND PL.PolicyStatus=1 OR PL.PolicyStatus=2
					INNER JOIN tblPaymentDetails PD ON PD.PremiumID = Pr.PremiumId
					INNER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID
					INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
					INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
					INNER JOIN tblVillages V ON V.VillageId = F.LocationId
					INNER JOIN tblWards W ON W.WardId = V.WardId
					INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
					INNER JOIN tblOfficer O ON O.LocationId = D.DistrictId
					INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
					LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
					WHERE PD.Amount >=  (SELECT TOP 1 Amount FROM tblPaymentDetails)
					AND Pr.ReportingCommissionID = @ReportingId
					--AND Pr.PayDate BETWEEN @FirstDay AND @LastDay
					
					GROUP BY Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName , PL.PolicyID ,  F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames ,O.Code + ' ' + O.LastName ,
					Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,Pr.Amount,Pr.Amount,  PD.Amount ,Payer.PayerName,PY.PaymentDate,PY.ExpectedAmount 
				END
			    
				 
				IF @Mode = -1

				BEGIN
				
					SELECT Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName Product,  PL.PolicyID, F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName,O.Code + ' ' + O.LastName Officer,
					Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount,Pr.Amount as TotlaPrescribedContribution, PD.Amount TotlActualPayment, Payer.PayerName,PY.PaymentDate, (@CommissionRate / 100) AS CommissionRate,PY.ExpectedAmount PaymentAmount
					FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID AND PL.PolicyStatus=1 OR PL.PolicyStatus=2
					LEFT OUTER JOIN tblPaymentDetails PD ON PD.PremiumID = Pr.PremiumId
					LEFT OUTER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID
					INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
					INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
					INNER JOIN tblVillages V ON V.VillageId = F.LocationId
					INNER JOIN tblWards W ON W.WardId = V.WardId
					INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
					INNER JOIN tblOfficer O ON O.LocationId = D.DistrictId
					INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
					LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
			        WHERE Pr.ReportingCommissionID = @ReportingId
	                --AND Pr.ReportingCommissionID = @ReportingId
					GROUP BY Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName , PL.PolicyID ,  F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames ,O.Code + ' ' + O.LastName ,
					Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,Pr.Amount,Pr.Amount, PD.Amount , Payer.PayerName,PY.PaymentDate, PY.ExpectedAmount
					--ORDER BY PremiumId DESC, IsHead DESC;
				END

			    
           
     RETURN 0
END
GO 

--//4th
GO

--01/03/2019
IF NOT OBJECT_ID('uspSSRSPolicyStatus') IS NULL
DROP PROCEDURE uspSSRSPolicyStatus
GO
CREATE PROCEDURE [dbo].[uspSSRSPolicyStatus]
	@RangeFrom datetime, --= getdate ,
	@RangeTo datetime, --= getdate ,
	@OfficerID int = 0,
	@RegionId INT = 0,
	@DistrictID as int = 0,
	@VillageID as int = 0, 
	@WardID as int = 0 ,
	@PolicyStatus as int = 0 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	DECLARE @RenewalID int
	DECLARE @PolicyID as int 
	DECLARE @FamilyID as int 
	DECLARE @RenewalDate as date
	DECLARE @InsureeID as int
	DECLARE @ProductID as int 
	DECLARE @ProductCode as nvarchar(8)
	DECLARE @ProductName as nvarchar(100)
	DECLARE @ProductFromDate as date 
	DECLARE @ProductToDate as date
	DECLARE @RegionName as nvarchar(50)
	DECLARE @DistrictName as nvarchar(50)
	DECLARE @VillageName as nvarchar(50) 
	DECLARE @WardName as nvarchar(50)  
	DECLARE @CHFID as nvarchar(12)
	DECLARE @InsLastName as nvarchar(100)
	DECLARE @InsOtherNames as nvarchar(100)
	DECLARE @InsDOB as date
	DECLARE @ConvProdID as int    
	DECLARE @OffCode as nvarchar(15)
	DECLARE @OffLastName as nvarchar(50)
	DECLARE @OffOtherNames as nvarchar(50)
	DECLARE @OffPhone as nvarchar(50)
	DECLARE @OffSubstID as int 
	DECLARE @OffWorkTo as date 
	DECLARE @PolicyValue DECIMAL(18,4) = 0
	DECLARE @OfficerId1 INT


	DECLARE @SMSStatus as tinyint 
	DECLARE @iCount as int 
	DECLARE @tblResult TABLE(PolicyId INT, 
							FamilyId INT,
							RenewalDate DATE,
							PolicyValue DECIMAL(18,4),
							InsureeId INT,
							ProdId INT,
							ProductCode NVARCHAR(8),
							ProductName NVARCHAR(100),
							DateFrom DATE,
							DateTo DATE,
							RegionName NVARCHAR(50),
							DistrictName NVARCHAR(50),
							VillageName NVARCHAR(50),
							WardName NVARCHAR(50),
							CHFID NVARCHAR(12),
							LastName NVARCHAR(100),
							OtherNames NVARCHAR(100),
							DOB DATE,
							ConversionProdId INT,
							OfficerId INT,
							Code NVARCHAR(15),
							OffLastName NVARCHAR(50),
							OffOtherNames NVARCHAR(50),
							Phone NVARCHAR(50),
							OfficerIdSubst INT,
							WorksTo DATE)
	DECLARE LOOP1 CURSOR LOCAL FORWARD_ONLY FOR
	SELECT PL.PolicyID, PL.FamilyID, DATEADD(DAY, 1, PL.ExpiryDate) AS RenewalDate, 
			F.InsureeID, Prod.ProdID, Prod.ProductCode, Prod.ProductName,
			Prod.DateFrom, Prod.DateTo,R.RegionName, D.DistrictName, V.VillageName, W.WardName, I.CHFID, I.LastName, I.OtherNames, I.DOB, Prod.ConversionProdID, 
			O.OfficerID, O.Code, O.LastName OffLastName, O.OtherNames OffOtherNames, O.Phone, O.OfficerIDSubst, O.WorksTo,
			PL.PolicyValue

			FROM tblPolicy PL INNER JOIN tblFamilies F ON PL.FamilyId = F.FamilyID
			INNER JOIN tblInsuree I ON F.InsureeId = I.InsureeID
			INNER JOIN tblProduct Prod ON PL.ProdId = Prod.ProdID
			INNER JOIN tblVillages V ON V.VillageId = F.LocationId
			INNER JOIN tblWards W ON W.WardId = V.WardId
			INNER JOIN tblDistricts D ON D.DistrictID = W.DistrictID
			INNER JOIN tblRegions R ON R.RegionId = D.Region
			INNER JOIN tblOfficer O ON PL.OfficerId = O.OfficerID
			AND PL.ExpiryDate BETWEEN @RangeFrom AND @RangeTo
			WHERE PL.ValidityTo IS NULL
			AND F.ValidityTo IS NULL
			AND R.ValidityTo IS NULL
			AND D.ValidityTo IS NULL
			AND V.ValidityTo IS NULL
			AND W.ValidityTo IS NULL
			AND I.ValidityTo IS NULL
			AND O.ValidityTo IS NULL
			AND PL.ExpiryDate BETWEEN @RangeFrom AND @RangeTo 
			AND (R.RegionId = @RegionId OR @RegionId = 0)
			AND (D.DistrictID = @DistrictID OR @DistrictID = 0)
			AND (V.VillageId = @VillageId  OR @VillageId = 0)
			AND (W.WardId = @WardId OR @WardId = 0)
			AND (PL.PolicyStatus = @PolicyStatus OR @PolicyStatus = 0)
			AND (PL.PolicyStatus > 1)	--Do not renew Idle policies
		ORDER BY RenewalDate DESC  --Added by Rogers


		OPEN LOOP1
		FETCH NEXT FROM LOOP1 INTO @PolicyID,@FamilyID,@RenewalDate,@InsureeID,@ProductID, @ProductCode,@ProductName,@ProductFromDate,@ProductToDate,@RegionName,@DistrictName,@VillageName,@WardName,
								  @CHFID,@InsLastName,@InsOtherNames,@InsDOB,@ConvProdID,@OfficerID1, @OffCode,@OffLastName,@OffOtherNames,@OffPhone,@OffSubstID,@OffWorkTo,
								  @PolicyValue
	
		WHILE @@FETCH_STATUS = 0 
		BEGIN
			
			--GET ProductCode or the substitution
			IF ISNULL(@ConvProdID,0) > 0 
			BEGIN
				SET @iCount = 0 
				WHILE @ConvProdID <> 0 AND @iCount < 20   --this to prevent a recursive loop by wrong datra entries 
				BEGIN
					--get new product info 
					SET @ProductID = @ConvProdID
					SELECT @ConvProdID = ConversionProdID FROM tblProduct WHERE ProdID = @ProductID
					IF ISNULL(@ConvProdID,0) = 0 
					BEGIN
						SELECT @ProductCode = ProductCode from tblProduct WHERE ProdID = @ProductID
						SELECT @ProductName = ProductName  from tblProduct WHERE ProdID = @ProductID
						SELECT @ProductFromDate = DateFrom from tblProduct WHERE ProdID = @ProductID
						SELECT @ProductToDate = DateTo  from tblProduct WHERE ProdID = @ProductID		
					END
					SET @iCount = @iCount + 1
				END
			END 
		
			IF ISNULL(@OfficerID1 ,0) > 0 
			BEGIN
				--GET OfficerCode or the substitution
				IF ISNULL(@OffSubstID,0) > 0 
				BEGIN
					SET @iCount = 0 
					WHILE @OffSubstID <> 0 AND @iCount < 20 AND @OffWorkTo < @RenewalDate  --this to prevent a recursive loop by wrong datra entries 
					BEGIN
						--get new product info 
						SET @OfficerID1 = @OffSubstID
						SELECT @OffSubstID = OfficerIDSubst FROM tblOfficer  WHERE OfficerID  = @OfficerID1
						IF ISNULL(@OffSubstID,0) = 0 
						BEGIN
							SELECT @OffCode = Code from tblOfficer  WHERE OfficerID  = @OfficerID1
							SELECT @OffLastName = LastName  from tblOfficer  WHERE OfficerID  = @OfficerID1
							SELECT @OffOtherNames = OtherNames  from tblOfficer  WHERE OfficerID  = @OfficerID1
							SELECT @OffPhone = Phone  from tblOfficer  WHERE OfficerID  = @OfficerID1
							SELECT @OffWorkTo = WorksTo  from tblOfficer  WHERE OfficerID  = @OfficerID1
						END
						SET @iCount = @iCount + 1
					END
				END 
			END 
		--Code added by Hiren to check if the policy has another following policy
			IF EXISTS(SELECT 1 FROM tblPolicy 
								WHERE FamilyId = @FamilyId 
								AND (ProdId = @ProductID OR ProdId = @ConvProdID) 
								AND StartDate >= @RenewalDate
								AND ValidityTo IS NULL
								)
					GOTO NextPolicy;
		--Added by Rogers to check if the policy is alread in a family
		IF EXISTS(SELECT 1 FROM @tblResult WHERE FamilyId = @FamilyID AND ProdId = @ProductID OR ProdId = @ConvProdID)
		GOTO NextPolicy;

		
		EXEC @PolicyValue = uspPolicyValue
							@FamilyId = @FamilyID,
							@ProdId = @ProductID,
							@EnrollDate = @RenewalDate,
							@PreviousPolicyId = @PolicyID,
							@PolicyStage = 'R';


		
		INSERT INTO @tblResult(PolicyId, FamilyId, RenewalDate, Policyvalue, InsureeId, ProdId,
		ProductCode, ProductName, DateFrom, DateTo,RegionName, DistrictName, VillageName,
		WardName, CHFID, LastName, OtherNames, DOB, ConversionProdId,OfficerId,
		Code, OffLastName, OffOtherNames, Phone, OfficerIdSubst, WorksTo)
		SELECT @PolicyID PolicyId, @FamilyId FamilyId, @RenewalDate RenewalDate, @PolicyValue PolicyValue, @InsureeID InsureeId, @ProductID ProdId,
		@ProductCode ProductCode, @ProductName ProductName, @ProductFromDate DateFrom, @ProductToDate DateTo,@RegionName RegionName, @DistrictName DistrictName, @VillageName VillageName,
		@WardName WardName, @CHFID CHFID, @InsLastName LastName, @InsOtherNames OtherNames, @InsDOB DOB, @ConvProdID ConversionProdId, @OfficerID1 OfficerId,
		@OffCode Code, @OffLastName OffLastName, @OffOtherNames OffOtherNames, @OffPhone Phone, @OffSubstID OfficerIdSubst, @OffWorkTo WorksTo
	
	NextPolicy:
			FETCH NEXT FROM LOOP1 INTO @PolicyID,@FamilyID,@RenewalDate,@InsureeID,@ProductID, @ProductCode,@ProductName,@ProductFromDate,@ProductToDate,@RegionName, @DistrictName,@VillageName,@WardName,
								  @CHFID,@InsLastName,@InsOtherNames,@InsDOB,@ConvProdID,@OfficerID1,@OffCode,@OffLastName,@OffOtherNames,@OffPhone,@OffSubstID,@OffWorkTo,
								  @PolicyValue
	
		END
		CLOSE LOOP1
		DEALLOCATE LOOP1

		SELECT PolicyId, FamilyId, RenewalDate, PolicyValue, InsureeId, ProdId, ProductCode, ProductName, DateFrom, DateTo, RegionName,DistrictName,
		VillageName, WardName, CHFID, LastName, OtherNames, DOB, ConversionProdId, OfficerId, Code, OffLastName, OffOtherNames, Phone, OfficerIdSubst, WorksTo
		FROM @tblResult
		WHERE (OfficerId = @OfficerId OR @OfficerId = 0);
END
GO

