

--DROP TABLE TBLROLERIGHT
--DROP TABLE TBLUSERROLE
--DROP TABLE TBLROLE
 
IF  COL_LENGTH('tblOfficer','HasLogin') IS NULL
	ALTER TABLE tblOfficer Add HasLogin BIT NULL

IF  COL_LENGTH('tblClaimAdmin','HasLogin') IS NULL
	ALTER TABLE tblClaimAdmin Add HasLogin BIT NULL

IF COL_LENGTH('tblUsers','IsAssociated') IS NULL
	ALTER TABLE tblUsers Add IsAssociated BIT NULL

IF  COL_LENGTH('tblReporting','OfficerID') IS NULL
	ALTER TABLE tblReporting Add OfficerID INT NULL

IF  COL_LENGTH('tblReporting','ReportType') IS NULL
	ALTER TABLE tblReporting Add ReportType INT NULL

IF  COL_LENGTH('tblPremium','ReportingCommisionID') IS NULL
	ALTER TABLE tblPremium Add ReportingCommisionID INT NULL

GO

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
GO


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

				-- START Accountant--
DECLARE @ID INT
SELECT @ID = RoleID from tblRole WHERE Rolename ='Accountant'
If @ID >0
BEGIN
--Family
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101001,GETDATE())  --FindFamily 

--Insuree
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101101,GETDATE()) --FindInsuree 

--Policy
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101201 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101201,GETDATE()) --FindPolicy

--Contribution
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101301 AND ROLEID = @ID) IS NULL 
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101301,GETDATE()) --FindContribution
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101302 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101302,GETDATE()) --AddContribution
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101303 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101303,GETDATE()) --EditContribution
--Payment
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101401 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101401,GETDATE()) --FindPayment

--LoadClaim
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111005 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111005,GETDATE()) --LoadClaim

--ValuateClaim
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111101,GETDATE()) --ValuateClaim

--ValuateClaim
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111102 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111102,GETDATE()) --BatchProcess

--BatchFilter
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111103,GETDATE()) --BatchFilter

--BatchPreview
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111104 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111104,GETDATE()) --BatchPreview 

--Contribution Collection
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160004 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160004,GETDATE()) --ContributionCollection

--Product Sales
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160005 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160005,GETDATE()) --ProductSales

--Contribution Distribution
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160006 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160006,GETDATE()) --ContributionDistribution

--Payment Category Overview
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160010 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160010,GETDATE()) --InsureeWithoutPhotos
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160011 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160011,GETDATE()) --PaymentCategoryOverView
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160012 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom) 
VALUES (@ID,160012,GETDATE()) --MatchingFunds
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160013 AND ROLEID = @ID) IS NULL 
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160013,GETDATE()) --ClaimOverViewReport
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160014 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160014,GETDATE()) --PercentageReferrals
-- 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160015 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160015,GETDATE()) --FamiliesInsureesOverview
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160016 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160016,GETDATE()) --PendingInsurees

--Renewals
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160017 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160017,GETDATE()) --Renewals 

--Capitation Payment
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160018 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160018,GETDATE()) --CapitationPayment

--Rejected Photo
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160019 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160019,GETDATE()) --RejectedPhoto

--ContributionPayment
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160020 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160020,GETDATE()) --FindContributionPayment

--ControlNumberAssignment
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160021 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160021,GETDATE()) --FindControlNumberAssignment

--Fund
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 181001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,181001,GETDATE()) --AddFund
END 
					--END Accountant--
					

				-- START Administrator-- 
SELECT @ID = RoleID from tblRole WHERE Rolename ='Administrator'
--PrintClaim 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111006 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111006,GETDATE()) --PrintClaim

--ReviewClaim 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111008 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111008,GETDATE()) --ReviewClaim
 
--BatchFilter 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111102 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111102,GETDATE()) --BatchFilter

--BatchPreview 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111103,GETDATE()) --BatchPreview

--Health Facilities
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121101,GETDATE()) --FindHealthFacilities
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121102 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121102,GETDATE()) --AddHealthFacilities
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121103,GETDATE()) --EditHealthFacilities
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121104 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121104,GETDATE()) --DeleteHealthFacilities

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

--Officer
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
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 151601 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,151601,GETDATE()) --IMISExtracts   
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160009 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160009,GETDATE()) --StatusOfRegister 
-- 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170002,GETDATE()) --DatabaseBackup 
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170003,GETDATE()) --DatabaseRestore 
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170004 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170004,GETDATE()) --ExecuteScripts 
					--END Administrator--



					--START Manager--
					 
SELECT @ID = RoleID from tblRole WHERE Rolename ='Manager' 
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160001,GETDATE()) --Primary Operational Indicators-policies 
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160002,GETDATE()) --Primary Operational Indicators Claims 
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160003,GETDATE()) --Derived Operational Indicators
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160008 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160008,GETDATE()) --Enrolment Performance Indicators 
					--END Manager--


					--START Receptionist--

SELECT @ID = RoleID from tblRole WHERE Rolename ='Receptionist'
--Family
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101001,GETDATE()) --FindFamily 

--Insuree
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101101,GETDATE()) --FindInsuree 

--Policy
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101201 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101201,GETDATE()) --FindPolicy 
					--END Receptionist--


					--START Claim Administrator--

SELECT @ID = RoleID from tblRole WHERE Rolename ='Claim Administrator'
--Claim
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

------
----IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111014 AND ROLEID = @ID) IS NULL
----INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
----VALUES (@ID,111014,GETDATE()) --ClaimsBatchClosure

------Claim overview Report
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160013 AND ROLEID = @ID) IS NULL 
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160013,GETDATE()) --ClaimOverViewReport

------Percentage Referrals
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160014 AND ROLEID = @ID) IS NULL 
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160014,GETDATE()) --PercentageReferrals

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
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 151201 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,151201,GETDATE()) --OfflineExtractCreate

--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 151601 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,151601,GETDATE()) --IMISExtracts  
 
--Database
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170001,GETDATE()) --DatabaseBackup
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170002,GETDATE()) --DatabaseRestore
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170003,GETDATE()) --ExecuteScripts
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

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 151201 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,151201,GETDATE()) --OfflineExtractCreate

--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 151601 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,151601,GETDATE()) --IMISExtracts  
 
--Database
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170001,GETDATE()) --DatabaseBackup
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170002,GETDATE()) --DatabaseRestore
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170003,GETDATE()) --ExecuteScripts
					--END Offline Administrator--


					--START Clerk--

SELECT @ID = RoleID from tblRole WHERE Rolename ='Clerk'
--Family
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

--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --EnquireInsuree

--Policy
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101201 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101201,GETDATE()) --FindPolicy
--
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
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101205 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101205,GETDATE()) --RenewPolicy
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101302 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101302,GETDATE()) --AddContribution
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101303 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101303,GETDATE()) --EditContribution
--Claim
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111002,GETDATE()) --EnterClaim
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111005 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111005,GETDATE()) --LoadClaim 

--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111015 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111015,GETDATE()) --ClaimsBatchClosure 

--Policy
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101205 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101205,GETDATE()) --RenewPolicy

--FindContribution
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101301 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101301,GETDATE()) --FindContribution
--DeleteContribution
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101304 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101304,GETDATE()) --DeleteContribution

--Payment
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101401 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101401,GETDATE()) --FindPayment

--Claim Xml Upload
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 151301 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,151301,GETDATE()) --ClaimXmlUpload 

--ContributionPayment
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160020 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160020,GETDATE()) --FindContributionPayment

--ControlNumberAssignment
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160021 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160021,GETDATE()) --FindControlNumberAssignment

					--END Clerk--



					--START Medical Officer--

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

--Prompt
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 191001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,191001,GETDATE()) --Feedbackprompt
					--END Medical Officer


					--START IMIS Administrator--

SELECT @ID = RoleID from tblRole WHERE Rolename ='IMIS Administrator'

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
VALUES (@ID,122005,GETDATE()) --DuplicateUserProfile
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
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 160007 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,160007,GETDATE()) --UserActivity
-- 
--Database
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170001,GETDATE()) --DatabaseBackup
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170002,GETDATE()) --DatabaseRestore
--
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170003,GETDATE()) --ExecuteScripts

--Email Settings
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 170004 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,170004,GETDATE()) --EmailSettings

--UploadICD
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 191002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,191002,GETDATE()) --UploadICD

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
--Claim
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111005 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111005,GETDATE()) --LoadClaim

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111001,GETDATE()) --FindClaim


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

-- ********* < CONVERGE MULTIPLE SCRIPTS INTO ONE SCRIPT 15 -02 -2019 > ***************
GO

-- [ UPLOAD LOCATIONS XML ]--
IF NOT OBJECT_ID('uspUploadLocationsXML') IS NULL
DROP PROCEDURE uspUploadLocationsXML

GO
CREATE  PROCEDURE [dbo].[uspUploadLocationsXML]
(
		--@File NVARCHAR(500),
		@XML XML,
		@StrategyId INT,	--1	: Insert Only,	2: Update Only	3: Insert & Update	7: Insert, Update & Delete
		@DryRun BIT,
		@AuditUserId INT,
		@SentRegion INT =0 OUTPUT,  
		@SentDistrict INT =0  OUTPUT, 
		@SentWard INT =0  OUTPUT, 
		@SentVillage INT =0  OUTPUT, 
		@InsertRegion INT =0  OUTPUT, 
		@InsertDistrict INT =0  OUTPUT, 
		@InsertWard INT =0  OUTPUT, 
		@InsertVillage INT =0 OUTPUT, 
		@UpdateRegion INT =0  OUTPUT, 
		@UpdateDistrict INT =0  OUTPUT, 
		@UpdateWard INT =0  OUTPUT, 
		@UpdateVillage INT =0  OUTPUT
)
AS 
	BEGIN

		/* Result type in @tblResults
		-------------------------------
			E	:	Error
			C	:	Conflict
			FE	:	Fatal Error

		Return Values
		------------------------------
			0	:	All Okay
			-1	:	Fatal error
		*/

		DECLARE @InsertOnly INT = 1,
				@UpdateOnly INT = 2,
				@Delete INT= 4

		SET @SentRegion = 0
		SET @SentDistrict = 0
		SET @SentWard = 0
		SET @SentVillage = 0
		SET @InsertRegion = 0
		SET @InsertDistrict = 0
		SET @InsertWard = 0
		SET @InsertVillage = 0
		SET @UpdateRegion = 0
		SET @UpdateDistrict = 0
		SET @UpdateWard = 0
		SET @UpdateVillage = 0

		DECLARE @Query NVARCHAR(500)
		--DECLARE @XML XML
		DECLARE @tblResult TABLE(Result NVARCHAR(Max), ResultType NVARCHAR(2))
		DECLARE @tempRegion TABLE(RegionCode NVARCHAR(100), RegionName NVARCHAR(100), IsValid BIT )
		DECLARE @tempLocation TABLE(LocationCode NVARCHAR(100))
		DECLARE @tempDistricts TABLE(RegionCode NVARCHAR(100),DistrictCode NVARCHAR(100),DistrictName NVARCHAR(100), IsValid BIT )
		DECLARE @tempWards TABLE(DistrictCode NVARCHAR(100),WardCode NVARCHAR(100),WardName NVARCHAR(100), IsValid BIT )
		DECLARE @tempVillages TABLE(WardCode NVARCHAR(100),VillageCode NVARCHAR(100), VillageName NVARCHAR(100),MalePopulation INT,FemalePopulation INT, OtherPopulation INT, Families INT, IsValid BIT )

		BEGIN TRY
	
			--SET @Query = (N'SELECT @XML = CAST(X as XML) FROM OPENROWSET(BULK  '''+ @File +''' ,SINGLE_BLOB) AS T(X)')

			--EXECUTE SP_EXECUTESQL @Query,N'@XML XML OUTPUT',@XML OUTPUT
			
			
			IF ( @XML.exist('(Locations/Regions/Region)')=1 AND  @XML.exist('(Locations/Districts/District)')=1 AND  @XML.exist('(Locations/Municipalities/Municipality)')=1 AND  @XML.exist('(Locations/Villages/Village)')=1)
				BEGIN
					--GET ALL THE REGIONS FROM THE XML
					INSERT INTO @tempRegion(RegionCode,RegionName,IsValid)
					SELECT 
					NULLIF(T.R.value('(RegionCode)[1]','NVARCHAR(100)'),''),
					NULLIF(T.R.value('(RegionName)[1]','NVARCHAR(100)'),''),
					1
					FROM @XML.nodes('Locations/Regions/Region') AS T(R)
		
					SELECT @SentRegion=@@ROWCOUNT

					--GET ALL THE DISTRICTS FROM THE XML
					INSERT INTO @tempDistricts(RegionCode, DistrictCode, DistrictName,IsValid)
					SELECT 
					NULLIF(T.R.value('(RegionCode)[1]','NVARCHAR(100)'),''),
					NULLIF(T.R.value('(DistrictCode)[1]','NVARCHAR(100)'),''),
					NULLIF(T.R.value('(DistrictName)[1]','NVARCHAR(100)'),''),
					1
					FROM @XML.nodes('Locations/Districts/District') AS T(R)

					SELECT @SentDistrict=@@ROWCOUNT

					--GET ALL THE WARDS FROM THE XML
					INSERT INTO @tempWards(DistrictCode,WardCode, WardName,IsValid)
					SELECT 
					NULLIF(T.R.value('(DistrictCode)[1]','NVARCHAR(100)'),''),
					NULLIF(T.R.value('(MunicipalityCode)[1]','NVARCHAR(100)'),''),
					NULLIF(T.R.value('(MunicipalityName)[1]','NVARCHAR(100)'),''),
					1
					FROM @XML.nodes('Locations/Municipalities/Municipality') AS T(R)
		
					SELECT @SentWard = @@ROWCOUNT

					--GET ALL THE VILLAGES FROM THE XML
					INSERT INTO @tempVillages(WardCode, VillageCode, VillageName, MalePopulation, FemalePopulation, OtherPopulation, Families, IsValid)
					SELECT 
					NULLIF(T.R.value('(MunicipalityCode)[1]','NVARCHAR(100)'),''),
					NULLIF(T.R.value('(VillageCode)[1]','NVARCHAR(100)'),''),
					NULLIF(T.R.value('(VillageName)[1]','NVARCHAR(100)'),''),
					NULLIF(T.R.value('(MalePopulation)[1]','INT'),0),
					NULLIF(T.R.value('(FemalePopulation)[1]','INT'),0),
					NULLIF(T.R.value('(OtherPopulation)[1]','INT'),0),
					NULLIF(T.R.value('(Families)[1]','INT'),0),
					1
					FROM @XML.nodes('Locations/Villages/Village') AS T(R)
		
					SELECT @SentVillage=@@ROWCOUNT
				END
			ELSE
				BEGIN
					RAISERROR (N'-200', 16, 1);
				END


			--SELECT * INTO tempRegion from @tempRegion
			--SELECT * INTO tempDistricts from @tempDistricts
			--SELECT * INTO tempWards from @tempWards
			--SELECT * INTO tempVillages from @tempVillages

			--RETURN

			/*========================================================================================================
			VALIDATION STARTS
			========================================================================================================*/	
			/********************************CHECK THE DUPLICATE LOCATION CODE******************************/
				INSERT INTO @tempLocation(LocationCode)
				SELECT RegionCode FROM @tempRegion
				INSERT INTO @tempLocation(LocationCode)
				SELECT DistrictCode FROM @tempDistricts
				INSERT INTO @tempLocation(LocationCode)
				SELECT WardCode FROM @tempWards
				INSERT INTO @tempLocation(LocationCode)
				SELECT VillageCode FROM @tempVillages
			
				INSERT INTO @tblResult(Result, ResultType)
				SELECT N'Location Code ' + QUOTENAME(LocationCode) + '  has already being used in a file ', N'C' FROM @tempLocation GROUP BY LocationCode HAVING COUNT(LocationCode)>1

				UPDATE @tempRegion  SET IsValid=0 WHERE RegionCode IN (SELECT LocationCode FROM @tempLocation GROUP BY LocationCode HAVING COUNT(LocationCode)>1)
				UPDATE @tempDistricts  SET IsValid=0 WHERE DistrictCode IN (SELECT LocationCode FROM @tempLocation GROUP BY LocationCode HAVING COUNT(LocationCode)>1)
				UPDATE @tempWards  SET IsValid=0 WHERE WardCode IN (SELECT LocationCode FROM @tempLocation GROUP BY LocationCode HAVING COUNT(LocationCode)>1)
				UPDATE @tempVillages  SET IsValid=0 WHERE VillageCode IN (SELECT LocationCode FROM @tempLocation GROUP BY LocationCode HAVING COUNT(LocationCode)>1)


			/********************************REGION STARTS******************************/
			--check if the regioncode is null 
			IF EXISTS(
			SELECT 1 FROM @tempRegion WHERE  LEN(ISNULL(RegionCode,''))=0 
			)
			INSERT INTO @tblResult(Result, ResultType)
			SELECT  CONVERT(NVARCHAR(3), COUNT(1)) + N' Region(s) have empty code', N'E' FROM @tempRegion WHERE  LEN(ISNULL(RegionCode,''))=0 
		
			UPDATE @tempRegion SET IsValid=0  WHERE  LEN(ISNULL(RegionCode,''))=0 
		
			--check if the regionname is null 
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'Region Code ' + QUOTENAME(RegionCode) + N' has empty name', N'E' FROM @tempRegion WHERE  LEN(ISNULL(RegionName,''))=0 
		
			UPDATE @tempRegion SET IsValid=0  WHERE RegionName  IS NULL OR LEN(ISNULL(RegionName,''))=0 

			--Check for Duplicates in file
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'Region Code ' + QUOTENAME(RegionCode) + ' found  ' + CONVERT(NVARCHAR(3), COUNT(RegionCode)) + ' times in the file', N'C'  FROM @tempRegion GROUP BY RegionCode HAVING COUNT(RegionCode) >1 
		
			UPDATE R SET IsValid = 0 FROM @tempRegion R
			WHERE RegionCode in (SELECT RegionCode from @tempRegion GROUP BY RegionCode HAVING COUNT(RegionCode) >1)
		
			--check the length of the regionCode
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'length of the Region Code ' + QUOTENAME(RegionCode) + N' is greater than 50', N'E' FROM @tempRegion WHERE  LEN(ISNULL(RegionCode,''))>50
		
			UPDATE @tempRegion SET IsValid=0  WHERE LEN(ISNULL(RegionCode,''))>50

			--check the length of the regionname
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'length of the Region Name ' + QUOTENAME(RegionCode) + N' is greater than 50', N'E' FROM @tempRegion WHERE  LEN(ISNULL(RegionName,''))>50
		
			UPDATE @tempRegion SET IsValid=0  WHERE LEN(ISNULL(RegionName,''))>50
		
		

			/********************************REGION ENDS******************************/

			/********************************DISTRICT STARTS******************************/
			--check if the district has regioncode
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'District Code ' + QUOTENAME(DistrictCode) + N' has empty Region Code', N'E' FROM @tempDistricts WHERE  LEN(ISNULL(RegionCode,''))=0 
		
			UPDATE @tempDistricts SET IsValid=0  WHERE  LEN(ISNULL(RegionCode,''))=0 

			--check if the district has valid regioncode
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'District Code ' + QUOTENAME(DistrictCode) + N' has invalid Region Code', N'E' FROM @tempDistricts TD
			LEFT OUTER JOIN @tempRegion TR ON TR.RegionCode=TD.RegionCode
			LEFT OUTER JOIN tblLocations L ON L.LocationCode=TD.RegionCode AND L.LocationType='R' 
			WHERE L.ValidityTo IS NULL
			AND L.LocationCode IS NULL
			AND TR.RegionCode IS NULL
			AND LEN(TD.RegionCode)>0

			UPDATE TD SET TD.IsValid=0 FROM @tempDistricts TD
			LEFT OUTER JOIN @tempRegion TR ON TR.RegionCode=TD.RegionCode
			LEFT OUTER JOIN tblLocations L ON L.LocationCode=TD.RegionCode AND L.LocationType='R' 
			WHERE L.ValidityTo IS NULL
			AND L.LocationCode IS NULL
			AND TR.RegionCode IS NULL
			AND LEN(TD.RegionCode)>0

			--check if the districtcode is null 
			IF EXISTS(
			SELECT  1 FROM @tempDistricts WHERE  LEN(ISNULL(DistrictCode,''))=0 
			)
			INSERT INTO @tblResult(Result, ResultType)
			SELECT  CONVERT(NVARCHAR(3), COUNT(1)) + N' District(s) have empty District code', N'E' FROM @tempDistricts WHERE  LEN(ISNULL(DistrictCode,''))=0 
		
			UPDATE @tempDistricts SET IsValid=0  WHERE  LEN(ISNULL(DistrictCode,''))=0 
		
			--check if the districtname is null 
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'District Code ' + QUOTENAME(DistrictCode) + N' has empty name', N'E' FROM @tempDistricts WHERE  LEN(ISNULL(DistrictName,''))=0 
		
			UPDATE @tempDistricts SET IsValid=0  WHERE  LEN(ISNULL(DistrictName,''))=0 
		
			--Check for Duplicates in file
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'District Code ' + QUOTENAME(DistrictCode) + ' found  ' + CONVERT(NVARCHAR(3), COUNT(DistrictCode)) + ' times in the file', N'C'  FROM @tempDistricts GROUP BY DistrictCode HAVING COUNT(DistrictCode) >1 
		
			UPDATE D SET IsValid = 0 FROM @tempDistricts D
			WHERE DistrictCode in (SELECT DistrictCode from @tempDistricts GROUP BY DistrictCode HAVING COUNT(DistrictCode) >1)

			--check the length of the DistrictCode
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'length of the District Code ' + QUOTENAME(DistrictCode) + N' is greater than 50', N'E' FROM @tempDistricts WHERE  LEN(ISNULL(DistrictCode,''))>50
		
			UPDATE @tempDistricts SET IsValid=0  WHERE LEN(ISNULL(DistrictCode,''))>50

			--check the length of the regionname
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'length of the District Name ' + QUOTENAME(DistrictName) + N' is greater than 50', N'E' FROM @tempDistricts WHERE  LEN(ISNULL(DistrictName,''))>50
		
			UPDATE @tempDistricts SET IsValid=0  WHERE LEN(ISNULL(DistrictName,''))>50

			--Validate Parent Location
			IF (@StrategyId & @UpdateOnly) > 0
				BEGIN
					INSERT INTO @tblResult(Result, ResultType)
					SELECT N'Region Code ' + QUOTENAME(TD.RegionCode) + ' is a duplicate in District Code ' + QUOTENAME(TD.DistrictCode) + ' and therefore it could not be updated', N'FD'
					FROM @tempDistricts TD
					INNER JOIN tblDistricts D ON TD.DistrictCode = D.DistrictCode
					LEFT OUTER JOIN tblRegions R ON TD.RegionCode = R.RegionCode
					WHERE D.ValidityTo IS NULL
					AND R.ValidityTo IS NULL
					AND D.Region != R.RegionId;

					UPDATE TD SET IsValid = 0
					FROM @tempDistricts TD
					INNER JOIN tblDistricts D ON TD.DistrictCode = D.DistrictCode
					LEFT OUTER JOIN tblRegions R ON TD.RegionCode = R.RegionCode
					WHERE D.ValidityTo IS NULL
					AND R.ValidityTo IS NULL
					AND D.Region != R.RegionId;

				END
		
			/********************************DISTRICT ENDS******************************/

			/********************************WARDS STARTS******************************/
			--check if the ward has districtcode
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'Municipality Code ' + QUOTENAME(WardCode) + N' has empty District Code', N'E' FROM @tempWards WHERE  LEN(ISNULL(DistrictCode,''))=0 
		
			UPDATE @tempWards SET IsValid=0  WHERE  LEN(ISNULL(DistrictCode,''))=0 

			--check if the ward has valid districtCode
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'Municipality Code ' + QUOTENAME(WardCode) + N' has invalid District Code', N'E' 
			FROM @tempWards TW
			LEFT OUTER JOIN @tempDistricts TD ON  TD.DistrictCode=TW.DistrictCode
			LEFT OUTER JOIN tblLocations L ON L.LocationCode=TW.DistrictCode AND L.LocationType='D' 
			WHERE L.ValidityTo IS NULL
			AND L.LocationCode IS NULL
			AND TD.DistrictCode IS NULL
			AND LEN(TW.DistrictCode)>0

			UPDATE TW SET TW.IsValid=0 FROM @tempWards TW
			LEFT OUTER JOIN @tempDistricts TD ON  TD.DistrictCode=TW.DistrictCode
			LEFT OUTER JOIN tblLocations L ON L.LocationCode=TW.DistrictCode AND L.LocationType='D' 
			WHERE L.ValidityTo IS NULL
			AND L.LocationCode IS NULL
			AND TD.DistrictCode IS NULL
			AND LEN(TW.DistrictCode)>0

			--check if the wardcode is null 
			IF EXISTS(
			SELECT  1 FROM @tempWards WHERE  LEN(ISNULL(WardCode,''))=0 
			)
			INSERT INTO @tblResult(Result, ResultType)
			SELECT  CONVERT(NVARCHAR(3), COUNT(1)) + N' Ward(s) have empty Municipality Code', N'E' FROM @tempWards WHERE  LEN(ISNULL(WardCode,''))=0 
		
			UPDATE @tempWards SET IsValid=0  WHERE  LEN(ISNULL(WardCode,''))=0 
		
			--check if the wardname is null 
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'Municipality Code ' + QUOTENAME(WardCode) + N' has empty name', N'E' FROM @tempWards WHERE  LEN(ISNULL(WardName,''))=0 
		
			UPDATE @tempWards SET IsValid=0  WHERE  LEN(ISNULL(WardName,''))=0 
		
			--Check for Duplicates in file
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'Municipality Code ' + QUOTENAME(WardCode) + ' found  ' + CONVERT(NVARCHAR(3), COUNT(WardCode)) + ' times in the file', N'C'  FROM @tempWards GROUP BY WardCode HAVING COUNT(WardCode) >1 
		
			UPDATE W SET IsValid = 0 FROM @tempWards W
			WHERE WardCode in (SELECT WardCode from @tempWards GROUP BY WardCode HAVING COUNT(WardCode) >1)

			--check the length of the wardcode
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'length of the Municipality Code ' + QUOTENAME(WardCode) + N' is greater than 50', N'E' FROM @tempWards WHERE  LEN(ISNULL(WardCode,''))>50
		
			UPDATE @tempWards SET IsValid=0  WHERE LEN(ISNULL(WardCode,''))>50

			--check the length of the wardname
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'length of the Municipality Name ' + QUOTENAME(WardName) + N' is greater than 50', N'E' FROM @tempWards WHERE  LEN(ISNULL(WardName,''))>50
		
			UPDATE @tempWards SET IsValid=0  WHERE LEN(ISNULL(WardName,''))>50;

			--Validate the parent location
			IF (@StrategyId & @UpdateOnly) > 0
				BEGIN
					INSERT INTO @tblResult(Result, ResultType)
					SELECT N'District Code ' + QUOTENAME(TW.DistrictCode) + ' is a duplicate in Municipality Code ' + QUOTENAME(TW.WardCode) + ' and therefore it could not be updated', N'FM'
					FROM @tempWards TW
					INNER JOIN tblWards W ON TW.WardCode = W.WardCode
					LEFT OUTER JOIN tblDistricts D ON TW.DistrictCode = D.DistrictCode
					WHERE W.ValidityTo IS NULL
					AND D.ValidityTo IS NULL
					AND W.DistrictId != D.DistrictId;

					UPDATE TW SET IsValid = 0
					FROM @tempWards TW
					INNER JOIN tblWards W ON TW.WardCode = W.WardCode
					LEFT OUTER JOIN tblDistricts D ON TW.DistrictCode = D.DistrictCode
					WHERE W.ValidityTo IS NULL
					AND D.ValidityTo IS NULL
					AND W.DistrictId != D.DistrictId;

				END

		
			/********************************WARDS ENDS******************************/

			/********************************VILLAGE STARTS******************************/
			--check if the village has Wardcoce
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'Village Code ' + QUOTENAME(VillageCode) + N' has empty Municipality Code', N'E' FROM @tempVillages WHERE  LEN(ISNULL(WardCode,''))=0 
		
			UPDATE @tempVillages SET IsValid=0  WHERE  LEN(ISNULL(WardCode,''))=0 

			--check if the village has valid wardcode

			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'Village Code ' + QUOTENAME(VillageCode) + N' has invalid Municipality Code', N'E' 
			FROM @tempVillages TV
			LEFT OUTER JOIN @tempWards TW ON TV.WardCode = TW.WardCode
			LEFT OUTER JOIN tblWards W ON TV.WardCode = W.WardCode
			WHERE W.ValidityTo IS NULL
			AND TW.WardCode IS NULL 
			AND W.WardCode IS NULL
			AND LEN(TV.WardCode)>0
			AND LEN(TV.VillageCode) >0;

			UPDATE TV SET TV.IsValid=0 
			FROM @tempVillages TV
			LEFT OUTER JOIN @tempWards TW ON TV.WardCode = TW.WardCode
			LEFT OUTER JOIN tblWards W ON TV.WardCode = W.WardCode
			WHERE W.ValidityTo IS NULL
			AND TW.WardCode IS NULL 
			AND W.WardCode IS NULL
			AND LEN(TV.WardCode)>0
			AND LEN(TV.VillageCode) >0;

			--check if the villagecode is null 
			IF EXISTS(
			SELECT  1 FROM @tempVillages WHERE  LEN(ISNULL(VillageCode,''))=0 
			)
			INSERT INTO @tblResult(Result, ResultType)
			SELECT  CONVERT(NVARCHAR(3), COUNT(1)) + N' Village(s) have empty Village code', N'E' FROM @tempVillages WHERE  LEN(ISNULL(VillageCode,''))=0 
		
			UPDATE @tempVillages SET IsValid=0  WHERE  LEN(ISNULL(VillageCode,''))=0 
		
			--check if the villageName is null 
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'Village Code ' + QUOTENAME(VillageCode) + N' has empty name', N'E' FROM @tempVillages WHERE  LEN(ISNULL(VillageName,''))=0 
		
			UPDATE @tempVillages SET IsValid=0  WHERE  LEN(ISNULL(VillageName,''))=0 
		
			--Check for Duplicates in file
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'Village Code ' + QUOTENAME(VillageCode) + ' found  ' + CONVERT(NVARCHAR(3), COUNT(VillageCode)) + ' times in the file', N'C'  FROM @tempVillages GROUP BY VillageCode HAVING COUNT(VillageCode) >1 
		
			UPDATE V SET IsValid = 0 FROM @tempVillages V
			WHERE VillageCode in (SELECT VillageCode from @tempVillages GROUP BY VillageCode HAVING COUNT(VillageCode) >1)

			--check the length of the VillageCode
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'length of the Village Code ' + QUOTENAME(VillageCode) + N' is greater than 50', N'E' FROM @tempVillages WHERE  LEN(ISNULL(VillageCode,''))>50
		
			UPDATE @tempVillages SET IsValid=0  WHERE LEN(ISNULL(VillageCode,''))>50

			--check the length of the VillageName
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'length of the Village Name ' + QUOTENAME(VillageName) + N' is greater than 50', N'E' FROM @tempVillages WHERE  LEN(ISNULL(VillageName,''))>50
		
			UPDATE @tempVillages SET IsValid=0  WHERE LEN(ISNULL(VillageName,''))>50

			--check the validity of the malepopulation
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'The Village Code' + QUOTENAME(VillageCode) + N' has invalid Male polulation', N'E' FROM @tempVillages WHERE  MalePopulation<0
		
			UPDATE @tempVillages SET IsValid=0  WHERE MalePopulation<0

			--check the validity of the female population
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'The Village Code' + QUOTENAME(VillageCode) + N' has invalid Female polulation', N'E' FROM @tempVillages WHERE  FemalePopulation<0
		
			UPDATE @tempVillages SET IsValid=0  WHERE FemalePopulation<0

			--check the validity of the OtherPopulation
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'The Village Code' + QUOTENAME(VillageCode) + N' has invalid Others polulation', N'E' FROM @tempVillages WHERE  OtherPopulation<0
		
			UPDATE @tempVillages SET IsValid=0  WHERE OtherPopulation<0

			--check the validity of the number of families
			INSERT INTO @tblResult(Result, ResultType)
			SELECT N'The Village Code' + QUOTENAME(VillageCode) + N' has invalid Number of  Families', N'E' FROM @tempVillages WHERE  Families<0
		
			UPDATE @tempVillages SET IsValid=0  WHERE Families < 0;

			--Validate the parent location
			IF (@StrategyId & @UpdateOnly) > 0
				BEGIN
					INSERT INTO @tblResult(Result, ResultType)
					SELECT N'Village Code ' + QUOTENAME(TV.VillageCode) + N' is a duplicate in Municipality Code ' + QUOTENAME(TV.WardCode) +  ' and therefore it could not be updated', N'FV'
					FROM @tempVillages TV
					INNER JOIN tblVillages V ON TV.VillageCode = V.VillageCode
					LEFT OUTER JOIN tblWards W ON TV.WardCode = W.WardCode
					WHERE V.ValidityTo IS NULL
					AND W.ValidityTo IS NULL
					AND V.WardId != W.WardId;

					UPDATE TV SET IsValid = 0
					FROM @tempVillages TV
					INNER JOIN tblVillages V ON TV.VillageCode = V.VillageCode
					LEFT OUTER JOIN tblWards W ON TV.WardCode = W.WardCode
					WHERE V.ValidityTo IS NULL
					AND W.ValidityTo IS NULL
					AND V.WardId != W.WardId;

				END

		
			/********************************VILLAGE ENDS******************************/
			/*========================================================================================================
			VALIDATION ENDS
			========================================================================================================*/	
	
			/*========================================================================================================
			COUNTS START
			========================================================================================================*/	
					--updates counts	
					IF (@StrategyId & @UpdateOnly) > 0
					BEGIN
							--Failed Locations
							IF (@StrategyId = @UpdateOnly)
							BEGIN
								--Failed Regions
								INSERT INTO @tblResult(Result, ResultType)
								SELECT 'Region Code ' + QUOTENAME(TR.RegionCode) + ' does not exists in database', N'FR'
								FROM @tempRegion TR
								LEFT OUTER JOIN tblRegions R ON TR.RegionCode = R.RegionCode
								WHERE R.ValidityTo IS NULL 
								--AND TR.IsValid=1
								AND R.RegionCode IS NULL;

								--Failed District
								INSERT INTO @tblResult(Result, ResultType)
								SELECT 'District Code ' + QUOTENAME(TD.DistrictCode) + ' does not exists in database', N'FD'
								FROM @tempDistricts TD
								LEFT OUTER JOIN tblDistricts D ON TD.DistrictCode = D.DistrictCode
								WHERE D.ValidityTo IS NULL 
								--AND TD.IsValid=1
								AND D.DistrictCode IS NULL;

								--Failed Municipality
								INSERT INTO @tblResult(Result, ResultType)
								SELECT 'Municipality Code ' + QUOTENAME(TM.WardCode) + ' does not exists in database', N'FM'
								FROM @tempWards TM
								LEFT OUTER JOIN tblWards W ON TM.WardCode= W.WardCode
								WHERE W.ValidityTo IS NULL 
								--AND TM.IsValid=1
								AND W.WardCode IS NULL;

								--Failed Villages
								INSERT INTO @tblResult(Result, ResultType)
								SELECT 'Village Code ' + QUOTENAME(TV.VillageCode) + ' does not exists in database', N'FV'
								FROM @tempVillages TV
								LEFT OUTER JOIN tblVillages V ON TV.VillageCode=V.VillageCode
								WHERE V.ValidityTo IS NULL 
								--AND TV.IsValid=1
								AND V.VillageCode IS NULL;


							END
						--Regions updates
							SELECT @UpdateRegion=COUNT(1) FROM @tempRegion TR 
							INNER JOIN tblLocations L ON L.LocationCode=TR.RegionCode AND L.LocationType='R'
							WHERE
							TR.IsValid=1
							AND L.ValidityTo IS NULL
							
						--Districts updates
							SELECT @UpdateDistrict=COUNT(1) FROM @tempDistricts TD 
							INNER JOIN tblLocations L ON L.LocationCode=TD.DistrictCode AND L.LocationType='D'
							WHERE
							TD.IsValid=1
							AND L.ValidityTo IS NULL

						--Wards updates
							SELECT @UpdateWard=COUNT(1) FROM @tempWards TW 
							INNER JOIN tblLocations L ON L.LocationCode=TW.WardCode AND L.LocationType='W'
							WHERE
							TW.IsValid=1
							AND L.ValidityTo IS NULL

						--Villages updates
							SELECT @UpdateVillage=COUNT(1) FROM @tempVillages TV 
							INNER JOIN tblLocations L ON L.LocationCode=TV.VillageCode AND L.LocationType='V'
							WHERE
							TV.IsValid=1
							AND L.ValidityTo IS NULL
					END

					--To be inserted
					IF (@StrategyId & @InsertOnly) > 0
						BEGIN
							
							--Failed Region
							IF (@StrategyId = @InsertOnly)
							BEGIN
								INSERT INTO @tblResult(Result, ResultType)
								SELECT 'Region Code' + QUOTENAME(TR.RegionCode) + ' already exists in database', N'FR'
								FROM @tempRegion TR
								INNER JOIN tblLocations L ON TR.RegionCode = L.LocationCode
								WHERE L.ValidityTo IS NULL 
								--AND TR.IsValid=1;
							END
							--Regions insert
							SELECT @InsertRegion=COUNT(1) FROM @tempRegion TR 
							LEFT OUTER JOIN tblLocations L ON L.LocationCode=TR.RegionCode AND L.LocationType='R'
							WHERE
							TR.IsValid=1 AND
							L.ValidityTo IS NULL
							AND L.LocationCode IS NULL

							--Failed Districts
							IF (@StrategyId = @InsertOnly)
							BEGIN
								INSERT INTO @tblResult(Result, ResultType)
								SELECT 'District Code' + QUOTENAME(TD.DistrictCode) + ' already exists in database', N'FD'
								FROM @tempDistricts TD
								INNER JOIN tblLocations L ON TD.DistrictCode = L.LocationCode
								WHERE L.ValidityTo IS NULL 
								--AND TD.IsValid=1;
							END
							--Districts insert
							SELECT @InsertDistrict=COUNT(1) FROM @tempDistricts TD 
							LEFT OUTER JOIN tblLocations L ON L.LocationCode=TD.DistrictCode AND L.LocationType='D'
							LEFT  OUTER JOIN tblRegions R ON TD.RegionCode = R.RegionCode AND R.ValidityTo IS NULL
							LEFT OUTER JOIN @tempRegion TR ON TD.RegionCode = TR.RegionCode
							WHERE
							TD.IsValid=1
							AND TR.IsValid = 1
							AND L.ValidityTo IS NULL
							AND L.LocationCode IS NULL
							
							--Failed Municipalities
							IF (@StrategyId = @InsertOnly)
							BEGIN
								INSERT INTO @tblResult(Result, ResultType)
								SELECT 'Municipality Code' + QUOTENAME(TW.WardCode) + ' already exists in database', N'FM'
								FROM @tempWards TW
								INNER JOIN tblLocations L ON TW.WardCode = L.LocationCode
								WHERE L.ValidityTo IS NULL 
								--AND TW.IsValid=1;
							END
							--Wards insert
							SELECT @InsertWard=COUNT(1) FROM @tempWards TW 
							LEFT OUTER JOIN tblLocations L ON L.LocationCode=TW.WardCode AND L.LocationType='W'
							LEFT  OUTER JOIN tblDistricts D ON TW.DistrictCode = D.DistrictCode AND D.ValidityTo IS NULL
							LEFT OUTER JOIN @tempDistricts TD ON TD.DistrictCode = TW.DistrictCode
							WHERE
							TW.IsValid=1
							AND TD.IsValid = 1
							AND L.ValidityTo IS NULL
							AND L.LocationCode IS NULL

							--Failed Village
							IF (@StrategyId = @InsertOnly)
							BEGIN
								INSERT INTO @tblResult(Result, ResultType)
								SELECT 'Village Code' + QUOTENAME(TV.VillageCode) + ' already exists in database', N'FV'
								FROM @tempVillages TV
								INNER JOIN tblLocations L ON TV.VillageCode= L.LocationCode
								WHERE L.ValidityTo IS NULL 
								--AND TV.IsValid=1;
							END
							--Villages insert
							SELECT @InsertVillage=COUNT(1) FROM @tempVillages TV 
							LEFT OUTER JOIN tblLocations L ON L.LocationCode=TV.VillageCode AND L.LocationType='V'
							LEFT  OUTER JOIN tblWards W ON TV.WardCode = W.WardCode AND W.ValidityTo IS NULL
							LEFT OUTER JOIN @tempWards TW ON TV.WardCode = TW.WardCode
							WHERE
							TV.IsValid=1
							AND TW.IsValid = 1
							AND L.ValidityTo IS NULL
							AND L.LocationCode IS NULL
						END
			


			/*========================================================================================================
			COUNTS ENDS
			========================================================================================================*/	
		
			
				IF @DryRun =0
					BEGIN
						BEGIN TRAN UPLOAD

						
			/*========================================================================================================
			UPDATE STARTS
			========================================================================================================*/	
					IF (@StrategyId & @UpdateOnly) > 0
							BEGIN
							/********************************REGIONS******************************/
								--insert historocal record(s)
									INSERT INTO [tblLocations]
										([LocationCode],[LocationName],[ParentLocationId],[LocationType],[ValidityFrom],[ValidityTo] ,[LegacyId],[AuditUserId],[MalePopulation] ,[FemalePopulation],[OtherPopulation],[Families])
									SELECT L.LocationCode, L.LocationName,L.ParentLocationId,L.LocationType, L.ValidityFrom,GETDATE(),L.LocationId,@AuditUserId AuditUserId, L.MalePopulation, L.FemalePopulation, L.OtherPopulation,L.Families 
									FROM @tempRegion TR 
									INNER JOIN tblLocations L ON L.LocationCode=TR.RegionCode AND L.LocationType='R'
									WHERE TR.IsValid=1 AND L.ValidityTo IS NULL

								--update
									UPDATE L SET  L.LocationName=TR.RegionName, ValidityFrom=GETDATE(),L.AuditUserId=@AuditUserId
									OUTPUT QUOTENAME(deleted.LocationCode), N'UR' INTO @tblResult
									FROM @tempRegion TR 
									INNER JOIN tblLocations L ON L.LocationCode=TR.RegionCode AND L.LocationType='R'
									WHERE TR.IsValid=1 AND L.ValidityTo IS NULL;

									SELECT @UpdateRegion = @@ROWCOUNT;

									/********************************DISTRICTS******************************/
								--Insert historical records
									INSERT INTO [tblLocations]
										([LocationCode],[LocationName],[ParentLocationId],[LocationType],[ValidityFrom],[ValidityTo] ,[LegacyId],[AuditUserId],[MalePopulation] ,[FemalePopulation],[OtherPopulation],[Families])
										SELECT L.LocationCode, L.LocationName,L.ParentLocationId,L.LocationType, L.ValidityFrom,GETDATE(),L.LocationId,@AuditUserId AuditUserId, L.MalePopulation, L.FemalePopulation, L.OtherPopulation,L.Families 
										FROM @tempDistricts TD 
										INNER JOIN tblLocations L ON L.LocationCode=TD.DistrictCode AND L.LocationType='D'
										WHERE TD.IsValid=1 AND L.ValidityTo IS NULL

									--update
										UPDATE L SET L.LocationName=TD.DistrictName, ValidityFrom=GETDATE(),L.AuditUserId=@AuditUserId
										OUTPUT QUOTENAME(deleted.LocationCode), N'UD' INTO @tblResult
										FROM @tempDistricts TD 
										INNER JOIN tblLocations L ON L.LocationCode=TD.DistrictCode AND L.LocationType='D'
										WHERE TD.IsValid=1 AND L.ValidityTo IS NULL;

										SELECT @UpdateDistrict = @@ROWCOUNT;

										/********************************WARD******************************/
								--Insert historical records
									INSERT INTO [tblLocations]
										([LocationCode],[LocationName],[ParentLocationId],[LocationType],[ValidityFrom],[ValidityTo] ,[LegacyId],[AuditUserId],[MalePopulation] ,[FemalePopulation],[OtherPopulation],[Families])
										SELECT L.LocationCode, L.LocationName,L.ParentLocationId,L.LocationType, L.ValidityFrom,GETDATE(),L.LocationId,@AuditUserId AuditUserId, L.MalePopulation, L.FemalePopulation, L.OtherPopulation,L.Families 
										FROM @tempWards TW 
										INNER JOIN tblLocations L ON L.LocationCode=TW.WardCode AND L.LocationType='W'
										WHERE TW.IsValid=1 AND L.ValidityTo IS NULL

								--Update
									UPDATE L SET L.LocationName=TW.WardName, ValidityFrom=GETDATE(),L.AuditUserId=@AuditUserId
										OUTPUT QUOTENAME(deleted.LocationCode), N'UM' INTO @tblResult
										FROM @tempWards TW 
										INNER JOIN tblLocations L ON L.LocationCode=TW.WardCode AND L.LocationType='W'
										WHERE TW.IsValid=1 AND L.ValidityTo IS NULL;

										SELECT @UpdateWard = @@ROWCOUNT;
									  
										/********************************VILLAGES******************************/
								--Insert historical records
									INSERT INTO [tblLocations]
										([LocationCode],[LocationName],[ParentLocationId],[LocationType],[ValidityFrom],[ValidityTo] ,[LegacyId],[AuditUserId],[MalePopulation] ,[FemalePopulation],[OtherPopulation],[Families])
										SELECT L.LocationCode, L.LocationName,L.ParentLocationId,L.LocationType, L.ValidityFrom,GETDATE(),L.LocationId,@AuditUserId AuditUserId, L.MalePopulation, L.FemalePopulation, L.OtherPopulation,L.Families 
										FROM @tempVillages TV 
										INNER JOIN tblLocations L ON L.LocationCode=TV.VillageCode AND L.LocationType='V'
										WHERE TV.IsValid=1 AND L.ValidityTo IS NULL

								--Update
									UPDATE L  SET L.LocationName=TV.VillageName, L.MalePopulation=TV.MalePopulation, L.FemalePopulation=TV.FemalePopulation, L.OtherPopulation=TV.OtherPopulation, L.Families=TV.Families, ValidityFrom=GETDATE(),L.AuditUserId=@AuditUserId
										OUTPUT QUOTENAME(deleted.LocationCode), N'UV' INTO @tblResult
										FROM @tempVillages TV 
										INNER JOIN tblLocations L ON L.LocationCode=TV.VillageCode AND L.LocationType='V'
										WHERE TV.IsValid=1 AND L.ValidityTo IS NULL;

										SELECT @UpdateVillage = @@ROWCOUNT;

							END
			/*========================================================================================================
			UPDATE ENDS
			========================================================================================================*/	

			/*========================================================================================================
			INSERT STARTS
			========================================================================================================*/	
					IF (@StrategyId & @InsertOnly) > 0
							BEGIN
								--insert Region(s)
									INSERT INTO [tblLocations]
										([LocationCode],[LocationName],[LocationType],[ValidityFrom],[AuditUserId])
									OUTPUT QUOTENAME(inserted.LocationCode), N'IR' INTO @tblResult
									SELECT TR.RegionCode, TR.RegionName,'R',GETDATE(), @AuditUserId AuditUserId 
									FROM @tempRegion TR 
									LEFT OUTER JOIN tblLocations L ON L.LocationCode=TR.RegionCode AND L.LocationType='R'
									WHERE
									TR.IsValid=1
									AND L.ValidityTo IS NULL
									AND L.LocationCode IS NULL;

									SELECT @InsertRegion = @@ROWCOUNT;


								--Insert District(s)
									INSERT INTO [tblLocations]
										([LocationCode],[LocationName],[ParentLocationId],[LocationType],[ValidityFrom],[AuditUserId])
									OUTPUT QUOTENAME(inserted.LocationCode), N'ID' INTO @tblResult
									SELECT TD.DistrictCode, TD.DistrictName, R.RegionId, 'D', GETDATE(), @AuditUserId AuditUserId 
									FROM @tempDistricts TD
									INNER JOIN tblRegions R ON TD.RegionCode = R.RegionCode
									LEFT OUTER JOIN tblDistricts D ON TD.DistrictCode = D.DistrictCode
									WHERE R.ValidityTo IS NULL
									AND D.ValidityTo IS NULL 
									AND D.DistrictId IS NULL;

									SELECT @InsertDistrict = @@ROWCOUNT;
									
								--Insert Wards
								INSERT INTO [tblLocations]
									([LocationCode],[LocationName],[ParentLocationId],[LocationType],[ValidityFrom],[AuditUserId])
								OUTPUT QUOTENAME(inserted.LocationCode), N'IM' INTO @tblResult
								SELECT TW.WardCode, TW.WardName, D.DistrictId, 'W',GETDATE(), @AuditUserId AuditUserId 
								FROM @tempWards TW
								INNER JOIN tblDistricts D ON TW.DistrictCode = D.DistrictCode
								LEFT OUTER JOIN tblWards W ON TW.WardCode = W.WardCode
								WHERE D.ValidityTo IS NULL
								AND W.ValidityTo IS NULL 
								AND W.WardId IS NULL;

									SELECT @InsertWard = @@ROWCOUNT;
									

							--insert  villages
								INSERT INTO [tblLocations]
									([LocationCode],[LocationName],[ParentLocationId],[LocationType], [MalePopulation],[FemalePopulation],[OtherPopulation],[Families], [ValidityFrom],[AuditUserId])
								OUTPUT QUOTENAME(inserted.LocationCode), N'IV' INTO @tblResult
								SELECT TV.VillageCode,TV.VillageName,W.WardId,'V',TV.MalePopulation,TV.FemalePopulation,TV.OtherPopulation,TV.Families,GETDATE(), @AuditUserId AuditUserId
								FROM @tempVillages TV
								INNER JOIN tblWards W ON TV.WardCode = W.WardCode
								LEFT OUTER JOIN tblVillages V ON TV.VillageCode = V.VillageCode
								WHERE W.ValidityTo IS NULL
								AND V.ValidityTo IS NULL 
								AND V.VillageId IS NULL;

									SELECT @InsertVillage = @@ROWCOUNT;

							END
			/*========================================================================================================
			INSERT ENDS
			========================================================================================================*/	
							

						COMMIT TRAN UPLOAD
					END
		
			
		
		END TRY
		BEGIN CATCH
			DECLARE @InvalidXML NVARCHAR(100)
			IF ERROR_NUMBER()=245 
				BEGIN
					SET @InvalidXML='Invalid input in either MalePopulation, FemalePopulation, OtherPopulation or Number of Families '
					INSERT INTO @tblResult(Result, ResultType)
					SELECT @InvalidXML, N'FE';
				END
			ELSE  IF ERROR_NUMBER()=9436 
				BEGIN
					SET @InvalidXML='Invalid XML file, end tag does not match start tag'
					INSERT INTO @tblResult(Result, ResultType)
					SELECT @InvalidXML, N'FE';
				END
			ELSE IF  ERROR_MESSAGE()=N'-200'
				BEGIN
					INSERT INTO @tblResult(Result, ResultType)
				SELECT'Invalid Locations XML file', N'FE';
			END
			ELSE
				INSERT INTO @tblResult(Result, ResultType)
				SELECT'Invalid XML file', N'FE';

			IF @@TRANCOUNT > 0 ROLLBACK TRAN UPLOAD;
			SELECT * FROM @tblResult
			RETURN -1;
				
		END CATCH
		SELECT * FROM @tblResult
		RETURN 0;
	END
-- [ End UPLOAD LOCATIONS XML ]
GO


-- [ SSRS OVERVIEW OF COMMISSIONS TEST ]--
IF NOT OBJECT_ID('uspSSRSOverviewOfCommissionsTEST') IS NULL
DROP PROCEDURE uspSSRSOverviewOfCommissionsTEST

GO
CREATE PROCEDURE uspSSRSOverviewOfCommissionsTEST
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

	
				

				UPDATE tblPremium SET ReportingCommisionID = @ReportingId
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
				left join tblReporting ON PR.ReportingCommisionID =tblReporting.ReportingId AND tblReporting.ReportType=2

				WHERE Pr.ValidityTo IS NULL 
				AND PL.ValidityTo IS NULL
				AND Prod.ValidityTo IS NULL
				AND F.ValidityTo IS NULL
				AND D.ValidityTo IS NULL
				AND W.ValidityTo IS NULL
				AND V.ValidityTo IS NULL
				AND Payer.ValidityTo IS NULL
				
				AND D.DistrictID = 33
				AND PayDate BETWEEN '2012-01-01' AND '2019-02-12'
				AND Prod.ProdID = 6
				AND (ISNULL(O.OfficerID,0) = ISNULL(1,0) OR 1 IS NULL)
				AND (ISNULL(Payer.PayerID,0) = ISNULL(1,0) OR 1 IS NULL)
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
	       
		
				--IF @Mode =0
				--BEGIN
		        
				--	SELECT Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName Product,  PL.PolicyID, F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName,O.Code + ' ' + O.LastName Officer,
				--	Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount,Pr.Amount as TotlaPrescribedContribution, Payer.PayerName,PY.PaymentDate
				--	FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID 
				--	INNER JOIN tblPaymentDetails PD ON PD.PremiumID = Pr.PremiumId
				--	INNER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID
				--	INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
				--	INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
				--	INNER JOIN tblVillages V ON V.VillageId = F.LocationId
				--	INNER JOIN tblWards W ON W.WardId = V.WardId
				--	INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
				--	INNER JOIN tblOfficer O ON O.LocationId = D.DistrictId
				--	INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
				--	LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
				--	WHERE Pr.PayDate BETWEEN @FirstDay AND @LastDay
				--	AND PL.PolicyStatus=1 OR PL.PolicyStatus=2
				--	--WHERE   (SELECT TOP 1 Amount FROM tblPremium) =

				--	GROUP BY Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName , PL.PolicyID ,  F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames ,O.Code + ' ' + O.LastName ,
				--	Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,Pr.Amount,Pr.Amount, Payer.PayerName,PY.PaymentDate

				--END
				--IF @Mode = 1

				--BEGIN
				--	SELECT TOP 1 Amount FROM tblPremium WHERE PolicyID = 1 ORDER BY PremiumId DESC
				--	SELECT Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName Product,  PL.PolicyID, F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName,O.Code + ' ' + O.LastName Officer,
				--	Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount,Pr.Amount as TotlaPrescribedContribution, Payer.PayerName,PY.PaymentDate
				--	FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID 
				--	INNER JOIN tblPaymentDetails PD ON PD.PremiumID = Pr.PremiumId
				--	INNER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID
				--	INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
				--	INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
				--	INNER JOIN tblVillages V ON V.VillageId = F.LocationId
				--	INNER JOIN tblWards W ON W.WardId = V.WardId
				--	INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
				--	INNER JOIN tblOfficer O ON O.LocationId = D.DistrictId
				--	INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
				--	LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
				--	WHERE PD.Amount >=  (SELECT TOP 1 Amount FROM tblPaymentDetails)
				--	AND Pr.PayDate BETWEEN @FirstDay AND @LastDay
				--	AND PL.PolicyStatus=1 OR PL.PolicyStatus=2
				--	GROUP BY Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName , PL.PolicyID ,  F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames ,O.Code + ' ' + O.LastName ,
				--	Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,Pr.Amount,Pr.Amount, Payer.PayerName,PY.PaymentDate
				--END
			    
				 
				--IF @Mode = NULL

				--BEGIN
				
					SELECT Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName Product,  PL.PolicyID, F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName,O.Code + ' ' + O.LastName Officer,
					Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount,Pr.Amount as TotlaPrescribedContribution, Payer.PayerName,PY.PaymentDate
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
			        WHERE Pr.ReportingCommisionID = @ReportingId
	                
					GROUP BY Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName , PL.PolicyID ,  F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames ,O.Code + ' ' + O.LastName ,
					Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,Pr.Amount,Pr.Amount, Payer.PayerName,PY.PaymentDate
					--ORDER BY PremiumId DESC, IsHead DESC;
				--END

			    
           
     RETURN 0
END
-- [ End SSRS OVERVIEW OF COMMISSIONS TEST ]
GO


-- [ SSRS GET MATCHING FUNDS ]--
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
				left join tblReporting ON PR.ReportingId =tblReporting.ReportingId AND tblReporting.ReportType=1
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
 -- [ End SSRS GET MATCHING FUNDS ]
 GO


 -- [ SSRS GET HISTORY REPORT ]--
IF NOT OBJECT_ID('uspSSRSGetClaimHistoryReport') IS NULL 
DROP PROCEDURE uspSSRSGetClaimHistoryReport
GO
CREATE PROCEDURE [dbo].[uspSSRSGetClaimHistoryReport]
(
	@HFID INT,
	@LocationId INT,
	@ProdId INT, 
	@StartDate DATE, 
	@EndDate DATE,
	@ClaimStatus INT = NULL,
	@InsuranceNumber NVARCHAR(12),
	@Scope INT= NULL
	
)
AS
BEGIN
     /*
	RESPONSE CODES
		1 - Insurance number not found
		0 - Success 
		-1 Unknown Error
	*/


	-- Check Insurance number if exsists
	
	    IF NOT EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID=@InsuranceNumber AND ValidityTo IS NULL) 
	     
		RETURN 1 
		
		IF @Scope =2 OR @Scope = -1
		BEGIN
		;WITH TotalForItems AS
			(
				SELECT C.ClaimId, SUM(CI.PriceAsked * CI.QtyProvided)Claimed,
				SUM(ISNULL(CI.PriceApproved, CI.PriceAsked) * ISNULL(CI.QtyApproved, CI.QtyProvided)) Approved,
				SUM(CI.PriceValuated)Adjusted,
				SUM(CI.RemuneratedAmount)Remunerated
				FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
				WHERE C.ValidityTo IS NULL
				AND CI.ValidityTo IS NULL
				GROUP BY C.ClaimID
			), TotalForServices AS
			(
				SELECT C.ClaimId, SUM(CS.PriceAsked * CS.QtyProvided)Claimed,
				SUM(ISNULL(CS.PriceApproved, CS.PriceAsked) * ISNULL(CS.QtyApproved, CS.QtyProvided)) Approved,
				SUM(CS.PriceValuated)Adjusted,
				SUM(CS.RemuneratedAmount)Remunerated
				FROM tblClaim C 
				LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
				WHERE C.ValidityTo IS NULL
				AND CS.ValidityTo IS NULL
				GROUP BY C.ClaimID
			)

			SELECT R.RegionId,R.RegionName,D.DistrictId,D.DistrictName, C.DateClaimed, PROD.ProductCode +' ' + PROD.ProductName Product, C.ClaimID, I.ItemId, S.ServiceID, HF.HFCode, HF.HFName, C.ClaimCode, C.DateClaimed, CA.LastName + ' ' + CA.OtherNames ClaimAdminName,
			C.DateFrom, C.DateTo, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsureeName,
			CASE C.ClaimStatus WHEN 1 THEN N'Rejected' WHEN 2 THEN N'Entered' WHEN 4 THEN N'Checked' WHEN 8 THEN N'Processed' WHEN 16 THEN N'Valuated' END ClaimStatus,
			C.RejectionReason, COALESCE(TFI.Claimed + TFS.Claimed, TFI.Claimed, TFS.Claimed) Claimed, 
			COALESCE(TFI.Approved + TFS.Approved, TFI.Approved, TFS.Approved) Approved,
			COALESCE(TFI.Adjusted + TFS.Adjusted, TFI.Adjusted, TFS.Adjusted) Adjusted,
			COALESCE(TFI.Remunerated + TFS.Remunerated, TFI.Remunerated, TFS.Remunerated)Paid,
			CASE WHEN CI.RejectionReason <> 0 THEN I.ItemCode ELSE NULL END RejectedItem, CI.RejectionReason ItemRejectionCode,
			CASE WHEN CS.RejectionReason > 0 THEN S.ServCode ELSE NULL END RejectedService, CS.RejectionReason ServiceRejectionCode,
			CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN I.ItemCode ELSE NULL END AdjustedItem,
			CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN CI.QtyProvided ELSE NULL END OrgQtyItem,
			CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved ,CI.QtyProvided)THEN CI.QtyApproved ELSE NULL END AdjQtyItem,
			CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided) THEN S.ServCode ELSE NULL END AdjustedService,
			CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided) THEN CS.QtyProvided ELSE NULL END OrgQtyService,
			CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved ,CS.QtyProvided)THEN CS.QtyApproved ELSE NULL END AdjQtyService,
			C.Explanation


			FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
			LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
			LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID
			LEFT OUTER JOIN tblProduct PROD ON PROD.ProdID = CI.ProdID
			LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID
			INNER JOIN tblHF HF ON C.HFID = HF.HfID
			LEFT OUTER JOIN tblDistricts D ON D.DistrictId =HF.LocationId
			LEFT OUTER JOIN tblRegions R ON R.RegionId = D.Region
			LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
			INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
			LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
			LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId

			WHERE C.ValidityTo IS NULL
			AND ISNULL(C.DateTo,C.DateFrom) BETWEEN @StartDate AND @EndDate
			AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
			AND HF.LocationId = @LocationId OR @LocationId = 0
			AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) 
			--OR  (CI.ProdID IS NOT NULL OR CS.ProdID IS NOT NULL)	-- --Added by Rogers
			AND (Ins.CHFID = @InsuranceNumber)
			AND HF.HFID = @HFID

		IF @Scope =0
		 BEGIN
			;WITH TotalForItems AS
			(
				SELECT C.ClaimId, SUM(CI.PriceAsked * CI.QtyProvided)Claimed,
				SUM(ISNULL(CI.PriceApproved, CI.PriceAsked) * ISNULL(CI.QtyApproved, CI.QtyProvided)) Approved,
				SUM(CI.PriceValuated)Adjusted,
				SUM(CI.RemuneratedAmount)Remunerated
				FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
				WHERE C.ValidityTo IS NULL
				AND CI.ValidityTo IS NULL
				GROUP BY C.ClaimID
			), TotalForServices AS
			(
				SELECT C.ClaimId, SUM(CS.PriceAsked * CS.QtyProvided)Claimed,
				SUM(ISNULL(CS.PriceApproved, CS.PriceAsked) * ISNULL(CS.QtyApproved, CS.QtyProvided)) Approved,
				SUM(CS.PriceValuated)Adjusted,
				SUM(CS.RemuneratedAmount)Remunerated
				FROM tblClaim C 
				LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
				WHERE C.ValidityTo IS NULL
				AND CS.ValidityTo IS NULL
				GROUP BY C.ClaimID
			)

			SELECT R.RegionId,R.RegionName,D.DistrictId,D.DistrictName, C.DateClaimed,PROD.ProductCode +' ' + PROD.ProductName Product, C.ClaimID, I.ItemId, S.ServiceID, HF.HFCode, HF.HFName, C.ClaimCode, C.DateClaimed, CA.LastName + ' ' + CA.OtherNames ClaimAdminName,
			C.DateFrom, C.DateTo, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsureeName,
			CASE C.ClaimStatus WHEN 1 THEN N'Rejected' WHEN 2 THEN N'Entered' WHEN 4 THEN N'Checked' WHEN 8 THEN N'Processed' WHEN 16 THEN N'Valuated' END ClaimStatus,
			COALESCE(TFI.Claimed + TFS.Claimed, TFI.Claimed, TFS.Claimed) Claimed, 
			COALESCE(TFI.Approved + TFS.Approved, TFI.Approved, TFS.Approved) Approved,
			COALESCE(TFI.Adjusted + TFS.Adjusted, TFI.Adjusted, TFS.Adjusted) Adjusted,
			COALESCE(TFI.Remunerated + TFS.Remunerated, TFI.Remunerated, TFS.Remunerated)Paid


			FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
			LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
			LEFT OUTER JOIN tblProduct PROD ON PROD.ProdID =@ProdId
			LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID
			LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID
			INNER JOIN tblHF HF ON C.HFID = HF.HfID
			LEFT OUTER JOIN tblDistricts D ON D.DistrictId = HF.LocationId
			LEFT OUTER JOIN tblRegions R ON R.RegionId = D.Region
			LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
			INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
			LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
			LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId

			
			WHERE C.ValidityTo IS NULL
			AND ISNULL(C.DateTo,C.DateFrom) BETWEEN @StartDate AND @EndDate
			AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
			AND HF.LocationId = @LocationId OR @LocationId = 0
			AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) 
			--OR  (CI.ProdID IS NOT NULL OR CS.ProdID IS NOT NULL)	-- --Added by Rogers
			AND (Ins.CHFID = @InsuranceNumber)
			--AND C.ClaimStatus=(CASE @Scope WHEN 1 THEN 4 END)
			AND HF.HFID = @HFID
		END
		IF @Scope =1
		BEGIN
		;WITH TotalForItems AS
			(
				SELECT C.ClaimId, SUM(CI.PriceAsked * CI.QtyProvided)Claimed,
				SUM(ISNULL(CI.PriceApproved, CI.PriceAsked) * ISNULL(CI.QtyApproved, CI.QtyProvided)) Approved,
				SUM(CI.PriceValuated)Adjusted,
				SUM(CI.RemuneratedAmount)Remunerated
				FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
				WHERE C.ValidityTo IS NULL
				AND CI.ValidityTo IS NULL
				GROUP BY C.ClaimID
			), TotalForServices AS
			(
				SELECT C.ClaimId, SUM(CS.PriceAsked * CS.QtyProvided)Claimed,
				SUM(ISNULL(CS.PriceApproved, CS.PriceAsked) * ISNULL(CS.QtyApproved, CS.QtyProvided)) Approved,
				SUM(CS.PriceValuated)Adjusted,
				SUM(CS.RemuneratedAmount)Remunerated
				FROM tblClaim C 
				LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
				WHERE C.ValidityTo IS NULL
				AND CS.ValidityTo IS NULL
				GROUP BY C.ClaimID
			)

			SELECT R.RegionId,R.RegionName,D.DistrictId,D.DistrictName, C.DateClaimed,PROD.ProductCode +' ' + PROD.ProductName Product, C.ClaimID, I.ItemId, S.ServiceID, HF.HFCode, HF.HFName, C.ClaimCode, C.DateClaimed, CA.LastName + ' ' + CA.OtherNames ClaimAdminName,
			C.DateFrom, C.DateTo, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsureeName,
			CASE C.ClaimStatus WHEN 1 THEN N'Rejected' WHEN 2 THEN N'Entered' WHEN 4 THEN N'Checked' WHEN 8 THEN N'Processed' WHEN 16 THEN N'Valuated' END ClaimStatus,
			C.RejectionReason, COALESCE(TFI.Claimed + TFS.Claimed, TFI.Claimed, TFS.Claimed) Claimed, 
			COALESCE(TFI.Approved + TFS.Approved, TFI.Approved, TFS.Approved) Approved,
			COALESCE(TFI.Adjusted + TFS.Adjusted, TFI.Adjusted, TFS.Adjusted) Adjusted,
			COALESCE(TFI.Remunerated + TFS.Remunerated, TFI.Remunerated, TFS.Remunerated)Paid,
			CASE WHEN CI.RejectionReason <> 0 THEN I.ItemCode ELSE NULL END RejectedItem, CI.RejectionReason ItemRejectionCode,
			CASE WHEN CS.RejectionReason > 0 THEN S.ServCode ELSE NULL END RejectedService, CS.RejectionReason ServiceRejectionCode


			FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
			LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
			LEFT OUTER JOIN tblProduct PROD ON PROD.ProdID =CI.ProdID
			LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID
			LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID
			INNER JOIN tblHF HF ON C.HFID = HF.HfID
		    
			LEFT OUTER JOIN tblDistricts D ON D.DistrictId = HF.LocationId
			LEFT OUTER JOIN tblRegions R ON R.RegionId = D.Region
			LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
			INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
			LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
			LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId
		
			WHERE C.ValidityTo IS NULL
			AND ISNULL(C.DateTo,C.DateFrom) BETWEEN @StartDate AND @EndDate
			AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
			AND HF.LocationId = @LocationId OR @LocationId = 0
			AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) 
			--OR  (CI.ProdID IS NOT NULL OR CS.ProdID IS NOT NULL)	-- --Added by Rogers
			AND (Ins.CHFID = @InsuranceNumber)
			AND HF.HFID = @HFID
		END

		

		END

		

END
-- [ End SSRS GET HISTORY REPORT ]
GO

-- [ Policy Value ]--
IF NOT OBJECT_ID('uspPolicyValue') IS NULL
DROP PROCEDURE [uspPolicyValue]
GO
CREATE PROCEDURE [dbo].[uspPolicyValue]
(
	@FamilyId INT =0,			--Provide if policy is not saved
	@ProdId INT =0,				--Provide if policy is not saved
	@PolicyId INT = 0,			--Provide if policy id is known
	@PolicyStage CHAR(1),		--Provide N if new policy, R if renewal
	@EnrollDate DATE = NULL,	--Enrollment date of the policy
	@PreviousPolicyId INT = 0,	--To determine the Expiry Date (For Renewal)
	@ErrorCode INT = 0 OUTPUT
)
AS

/*
********ERROR CODE***********
-1	:	Policy does not exists at the time of enrolment
-2	:	Policy was deleted at the time of enrolment

*/

BEGIN

	SET @ErrorCode = 0;

	DECLARE @LumpSum DECIMAL(18,2) = 0,
			@PremiumAdult DECIMAL(18,2) = 0,
			@PremiumChild DECIMAL(18,2) = 0,
			@RegistrationLumpSum DECIMAL(18,2) = 0,
			@RegistrationFee DECIMAL(18,2) = 0,
			@GeneralAssemblyLumpSum DECIMAL(18,2) = 0,
			@GeneralAssemblyFee DECIMAL(18,2) = 0,
			@Threshold SMALLINT = 0,
			@MemberCount INT = 0,
			@AdultMembers INT =0,
			@ChildMembers INT = 0,
			@OAdultMembers INT =0,
			@OChildMembers INT = 0,
			@Registration DECIMAL(18,2) = 0,
			@GeneralAssembly DECIMAL(18,2) = 0,
			@Contribution DECIMAL(18,2) = 0,
			@PolicyValue DECIMAL(18,2) = 0,
			@ExtraAdult INT = 0,
			@ExtraChild INT = 0,
			@AddonAdult DECIMAL(18,2) = 0,
			@AddonChild DECIMAL(18,2) = 0,
			@DiscountPeriodR INT = 0,
			@DiscountPercentR DECIMAL(18,2) =0,
			@DiscountPeriodN INT = 0,
			@DiscountPercentN DECIMAL(18,2) =0,
			@ExpiryDate DATE
		

		IF @EnrollDate IS NULL 
			SET @EnrollDate = GETDATE();



	--This means you are calculating existing policy
		IF @PolicyId > 0
		BEGIN
			SELECT TOP 1 @FamilyId = FamilyId, @ProdId = ProdId,@PolicyStage = PolicyStage,@EnrollDate = EnrollDate, @ExpiryDate = ExpiryDate FROM tblPolicy WHERE PolicyID = @PolicyId
		END

		DECLARE @ValidityTo DATE = NULL,
				@LegacyId INT = NULL

	/*--Get all the required fiedls from product (Valide product at the enrollment time)--*/
		SELECT TOP 1 @LumpSum = ISNULL(LumpSum,0),@PremiumAdult = ISNULL(PremiumAdult,0),@PremiumChild = ISNULL(PremiumChild,0),@RegistrationLumpSum = ISNULL(RegistrationLumpSum,0),
		@RegistrationFee = ISNULL(RegistrationFee,0),@GeneralAssemblyLumpSum = ISNULL(GeneralAssemblyLumpSum,0), @GeneralAssemblyFee = ISNULL(GeneralAssemblyFee,0), 
		@Threshold = ISNULL(Threshold ,0),@MemberCount = ISNULL(MemberCount,0), @ValidityTo = ValidityTo, @LegacyId = LegacyID, @DiscountPeriodR = ISNULL(RenewalDiscountPeriod, 0), @DiscountPercentR = ISNULL(RenewalDiscountPerc,0)
		, @DiscountPeriodN = ISNULL(EnrolmentDiscountPeriod, 0), @DiscountPercentN = ISNULL(EnrolmentDiscountPerc,0)
		FROM tblProduct 
		WHERE (ProdID = @ProdId OR LegacyID = @ProdId)
		AND CONVERT(DATE,ValidityFrom,103) <= @EnrollDate
		ORDER BY ValidityFrom Desc

		IF @@ROWCOUNT = 0	--No policy found
			SET @ErrorCode = -1
		IF NOT @ValidityTo IS NULL AND @LegacyId IS NULL	--Policy is deleted by the time of enrollment
			SET @ErrorCode = -2
			

	/*
		Relationships to be excluded from the normal family Count
		7: Others
	*/

	--Get only valid insurees according to the maximum members of the product from the family

	IF NOT OBJECT_ID('tempdb..#tblInsuree') IS NULL DROP TABLE #tblInsuree
	SELECT * INTO #tblInsuree FROM tblInsuree WHERE FamilyID = @FamilyId AND ValidityTo IS NULL;

	;WITH TempIns AS
	(
	SELECT ROW_NUMBER() OVER(ORDER BY ValidityFrom) Number, * FROM #tblInsuree
	)DELETE I FROM #tblInsuree I INNER JOIN TempIns T ON I.InsureeId = T.InsureeId
	 WHERE Number > @MemberCount;


	--Get the number of adults, Children, OtherAdult and Other Children from the family
		SET @AdultMembers = (SELECT COUNT(InsureeId) FROM #tblInsuree WHERE DATEDIFF(YEAR,DOB,GETDATE()) >= 18 AND ISNULL(Relationship,0) <> 7 AND ValidityTo IS NULL AND FamilyID = @FamilyId) 
		SET @ChildMembers = (SELECT COUNT(InsureeId) FROM #tblInsuree WHERE DATEDIFF(YEAR,DOB,GETDATE()) < 18 AND ISNULL(Relationship,0) <> 7  AND ValidityTo IS NULL AND FamilyID = @FamilyId)
		SET @OAdultMembers = (SELECT COUNT(InsureeId) FROM #tblInsuree WHERE DATEDIFF(YEAR,DOB,GETDATE()) >= 18 AND ISNULL(Relationship,0) = 7 AND ValidityTo IS NULL AND FamilyID = @FamilyId) 
		SET @OChildMembers = (SELECT COUNT(InsureeId) FROM #tblInsuree WHERE DATEDIFF(YEAR,DOB,GETDATE()) < 18 AND ISNULL(Relationship,0) = 7 AND ValidityTo IS NULL AND FamilyID = @FamilyId)


	--Get extra members in family
		IF @Threshold > 0 AND @AdultMembers > @Threshold
			SET @ExtraAdult = @AdultMembers - @Threshold
		IF @Threshold > 0 AND @ChildMembers > (@Threshold - @AdultMembers + @ExtraAdult )
					SET @ExtraChild = @ChildMembers - ((@Threshold - @AdultMembers + @ExtraAdult))

--Added by Salumu Start 31-01-2019
	   IF @Threshold = 0 AND @AdultMembers > @Threshold
		    SET @ExtraAdult = @AdultMembers
	   IF @Threshold = 0 AND @ChildMembers > @Threshold
		     SET @ExtraChild = @ChildMembers
--Added by Salumu End

--Get the Contribution



-- Changed by Salumu 31-01-2019
--OLD
	
		--IF @LumpSum > 0
		-- to 
		--	SET @Contribution = @LumpSum
		---ELSE
		--	SET @Contribution = (@AdultMembers * @PremiumAdult) + (@ChildMembers * @PremiumChild)
--NEW
			SET @Contribution = @LumpSum
-- Changed by Salumu 31-01-2019	

	--Get the Assembly
		IF @GeneralAssemblyLumpSum > 0
			SET @GeneralAssembly = @GeneralAssemblyLumpSum
		ELSE
			SET @GeneralAssembly = (@AdultMembers + @ChildMembers + @OAdultMembers + @OChildMembers) * @GeneralAssemblyFee;

	--Get the Registration
		IF @PolicyStage = N'N'	--Don't calculate if it's renewal
		BEGIN
			IF @RegistrationLumpSum > 0
				SET @Registration = @RegistrationLumpSum
			ELSE
				SET @Registration = (@AdultMembers + @ChildMembers  + @OAdultMembers + @OChildMembers) * @RegistrationFee;
		END

	/* Any member above the maximum member count  or with excluded relationship calculate the extra addon amount */
	
		SET @AddonAdult = (@ExtraAdult + @OAdultMembers) * @PremiumAdult;
		SET @AddonChild = (@ExtraChild + @OChildMembers) * @PremiumChild;

		SET @Contribution += @AddonAdult + @AddonChild;
	

		--Line below was a mistake, All adults and children are already included in GeneralAssembly and Registration
		--SET @GeneralAssembly += (@OAdultMembers + @OChildMembers + @ExtraAdult + @ExtraChild) * @GeneralAssemblyFee;
		
		--IF @PolicyStage = N'N'
		--	SET @Registration += (@OAdultMembers + @OChildMembers + @ExtraAdult + @ExtraChild) * @RegistrationFee;


	SET @PolicyValue = @Contribution + @GeneralAssembly + @Registration;


	--The total policy value is calculated, So if the enroldate is earlier than the discount period then apply discount
	DECLARE @HasCycle BIT
	DECLARE @tblPeriod TABLE(StartDate DATE, ExpiryDate DATE, HasCycle BIT)
	INSERT INTO @tblPeriod(StartDate, ExpiryDate, HasCycle)
	EXEC uspGetPolicyPeriod @ProdId, @EnrollDate, @HasCycle OUTPUT, @PolicyStage;

	DECLARE @StartDate DATE =(SELECT StartDate FROM @tblPeriod);


	DECLARE @MinDiscountDateR DATE,
			@MinDiscountDateN DATE

	IF @PolicyStage = N'N'
	BEGIN
		SET @MinDiscountDateN = DATEADD(MONTH,-(@DiscountPeriodN),@StartDate);
		IF @EnrollDate <= @MinDiscountDateN AND @HasCycle = 1
			SET @PolicyValue -=  (@PolicyValue * 0.01 * @DiscountPercentN);
	END
	ELSE IF @PolicyStage  = N'R'
	BEGIN
		DECLARE @PreviousExpiryDate DATE = NULL

		IF @PreviousPolicyId > 0
		BEGIN
			SELECT @PreviousExpiryDate = DATEADD(DAY, 1, ExpiryDate) FROM tblPolicy WHERE ValidityTo IS NULL AND PolicyId = @PreviousPolicyId;	
		END
		ELSE
		BEGIN
			SET @PreviousExpiryDate = @StartDate;
		END

		SET @MinDiscountDateR = DATEADD(MONTH,-(@DiscountPeriodR),@PreviousExpiryDate);
		IF @EnrollDate <= @MinDiscountDateR
			SET @PolicyValue -=  (@PolicyValue * 0.01 * @DiscountPercentR);
	END

	SELECT @PolicyValue PolicyValue;
	RETURN @PolicyValue;

END
-- [ End POLICY VALUE ]
GO

-- [ RFC 98 ADJUSTIBILITIES ] --
   IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'CHFID')
	   UPDATE tblControls SET FieldName='CHFID',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'CHFID'
	ELSE
	  INSERT INTO tblControls(FieldName,Adjustibility,Usage)
	  VALUES('CHFID','M','Search Insurance Number/Enquiry')

    IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'LastName')
	   UPDATE tblControls SET FieldName='LastName',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'LastName'
	ELSE
	  INSERT INTO tblControls(FieldName,Adjustibility,Usage)
	  VALUES('LastName','M','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'OtherNames')
	   UPDATE tblControls SET FieldName='OtherNames',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'OtherNames'
	ELSE
	  INSERT INTO tblControls(FieldName,Adjustibility,Usage)
	  VALUES('OtherNames','M','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Age')
	   UPDATE tblControls SET FieldName='Age',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Age'
	ELSE
	  INSERT INTO tblControls(FieldName,Adjustibility,Usage)
	  VALUES('Age','M','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'DOB')
		UPDATE tblControls SET FieldName='DOB',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'DOB' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('DOB','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Gender')
		UPDATE tblControls SET FieldName='Gender',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Gender' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('Gender','O','Search Insurance Number/Enquiry')

	
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'RegionOfFSP')
		UPDATE tblControls SET FieldName='RegionOfFSP',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'RegionOfFSP' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('RegionOfFSP','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'DistrictOfFSP')
		UPDATE tblControls SET FieldName='DistrictOfFSP',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'DistrictOfFSP' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('DistrictOfFSP','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'HFLevel')
		UPDATE tblControls SET FieldName='HFLevel',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'HFLevel' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('HFLevel','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'FirstServicePoint')
		UPDATE tblControls SET FieldName='FirstServicePoint',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'FirstServicePoint' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('FirstServicePoint','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'ProductCode')
		UPDATE tblControls SET FieldName='ProductCode',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'ProductCode' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('ProductCode','M','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'ExpiryDate')
		UPDATE tblControls SET FieldName='ExpiryDate',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'ExpiryDate' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('ExpiryDate','M','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'PolicyStatus')
		UPDATE tblControls SET FieldName='PolicyStatus',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'PolicyStatus' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('PolicyStatus','M','Search Insurance Number/Enquiry')

		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Ded1')
		UPDATE tblControls SET FieldName='Ded1',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Ded1' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('Ded1','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Ded2')
		UPDATE tblControls SET FieldName='Ded2',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Ded2' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('Ded2','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Ceiling1')
		UPDATE tblControls SET FieldName='Ceiling1',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Ceiling1' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('Ceiling1','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Ceiling2')
		UPDATE tblControls SET FieldName='Ceiling2',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Ceiling2' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('Ceiling2','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalAdmissionsLeft')
		UPDATE tblControls SET FieldName='TotalAdmissionsLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalAdmissionsLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalAdmissionsLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalVisitsLeft')
		UPDATE tblControls SET FieldName='TotalVisitsLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalVisitsLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalVisitsLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalConsultationsLeft')
		UPDATE tblControls SET FieldName='TotalConsultationsLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalConsultationsLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalConsultationsLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalSurgeriesLeft')
		UPDATE tblControls SET FieldName='TotalSurgeriesLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalSurgeriesLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalSurgeriesLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalDelivieriesLeft')
		UPDATE tblControls SET FieldName='TotalDelivieriesLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalDelivieriesLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalDelivieriesLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalAntenatalLeft')
		UPDATE tblControls SET FieldName='TotalAntenatalLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalAntenatalLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalAntenatalLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'HospitalizationAmountLeft')
		UPDATE tblControls SET FieldName='HospitalizationAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'HospitalizationAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('HospitalizationAmountLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'ConsultationAmountLeft')
		UPDATE tblControls SET FieldName='ConsultationAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'ConsultationAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('ConsultationAmountLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'SurgeryAmountLeft')
		UPDATE tblControls SET FieldName='SurgeryAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'SurgeryAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('SurgeryAmountLeft','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'AntenatalAmountLeft')
		UPDATE tblControls SET FieldName='AntenatalAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'AntenatalAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('AntenatalAmountLeft','O','Search Insurance Number/Enquiry')

		IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'DeliveryAmountLeft')
		UPDATE tblControls SET FieldName='DeliveryAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'DeliveryAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('DeliveryAmountLeft','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'AntenatalAmountLeft')
		UPDATE tblControls SET FieldName='AntenatalAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'AntenatalAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('AntenatalAmountLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblItemCodeL')
		UPDATE tblControls SET FieldName='lblItemCodeL',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblItemCodeL' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblItemCodeL','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblItemCode')
		UPDATE tblControls SET FieldName='lblItemCode',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblItemCode' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblItemCode','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblItemLeftL')
		UPDATE tblControls SET FieldName='lblItemLeftL',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblItemLeftL' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblItemLeftL','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblServiceMinDate')
		UPDATE tblControls SET FieldName='lblServiceMinDate',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblServiceMinDate' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblServiceMinDate','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblItemMinDate')
		UPDATE tblControls SET FieldName='lblItemMinDate',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblItemMinDate' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblItemMinDate','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblServiceLeft')
		UPDATE tblControls SET FieldName='lblServiceLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblServiceLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblServiceLeft','O','Search Insurance Number/Enquiry')

GO


-- [ IMIS HASHCODE ]
IF  COL_LENGTH('tblUsers','StoredPassword') IS NULL
ALTER TABLE tblUsers ADD StoredPassword nvarchar(256) NULL
GO
IF  COL_LENGTH('tblUsers','PrivateKey') IS NULL
ALTER TABLE tblUsers ADD PrivateKey nvarchar(256) NULL
GO
IF  COL_LENGTH('tblUsers','PasswordValidity') IS NULL
ALTER TABLE tblUsers ADD PasswordValidity DateTime NULL
GO

-- [ COMBINED SCRIPTS ]--

--Capitation Payment
IF OBJECT_ID('uspSSRSCapitationPayment') IS NOT NULL
DROP PROCEDURE [dbo].[uspSSRSCapitationPayment]
GO

CREATE PROCEDURE [dbo].[uspSSRSCapitationPayment]

(
	@RegionId INT = NULL,
	@DistrictId INT = NULL,
	@ProdId INT,
	@Year INT,
	@Month INT,
	@HFLevel xAttributeV READONLY
)
AS
BEGIN
	
	DECLARE @Level1 CHAR(1) = NULL,
			@Sublevel1 CHAR(1) = NULL,
			@Level2 CHAR(1) = NULL,
			@Sublevel2 CHAR(1) = NULL,
			@Level3 CHAR(1) = NULL,
			@Sublevel3 CHAR(1) = NULL,
			@Level4 CHAR(1) = NULL,
			@Sublevel4 CHAR(1) = NULL,
			@ShareContribution DECIMAL(5, 2),
			@WeightPopulation DECIMAL(5, 2),
			@WeightNumberFamilies DECIMAL(5, 2),
			@WeightInsuredPopulation DECIMAL(5, 2),
			@WeightNumberInsuredFamilies DECIMAL(5, 2),
			@WeightNumberVisits DECIMAL(5, 2),
			@WeightAdjustedAmount DECIMAL(5, 2)

	DECLARE @FirstDay DATE = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01'; 
	DECLARE @LastDay DATE = EOMONTH(CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01', 0)
	DECLARE @DaysInMonth INT = DATEDIFF(DAY,@FirstDay,DATEADD(MONTH,1,@FirstDay));

	SELECT @Level1 = Level1, @Sublevel1 = Sublevel1, @Level2 = Level2, @Sublevel2 = Sublevel2, @Level3 = Level3, @Sublevel3 = Sublevel3, 
	@Level4 = Level4, @Sublevel4 = Sublevel4, @ShareContribution = ISNULL(ShareContribution, 0), @WeightPopulation = ISNULL(WeightPopulation, 0), 
	@WeightNumberFamilies = ISNULL(WeightNumberFamilies, 0), @WeightInsuredPopulation = ISNULL(WeightInsuredPopulation, 0), @WeightNumberInsuredFamilies = ISNULL(WeightNumberInsuredFamilies, 0), 
	@WeightNumberVisits = ISNULL(WeightNumberVisits, 0), @WeightAdjustedAmount = ISNULL(WeightAdjustedAmount, 0)
	FROM tblProduct Prod 
	WHERE ProdId = @ProdId;


	PRINT @ShareContribution
	PRINT @WeightPopulation
	PRINT @WeightNumberFamilies 
	PRINT @WeightInsuredPopulation 
	PRINT @WeightNumberInsuredFamilies 
	PRINT @WeightNumberVisits 
	PRINT @WeightAdjustedAmount


	;WITH TotalPopFam AS
	(
		SELECT C.HFID  ,
		CASE WHEN ISNULL(@DistrictId, @RegionId) IN (R.RegionId, D.DistrictId) THEN 1 ELSE 0 END * SUM((ISNULL(L.MalePopulation, 0) + ISNULL(L.FemalePopulation, 0) + ISNULL(L.OtherPopulation, 0)) *(0.01* Catchment))[Population], 
		CASE WHEN ISNULL(@DistrictId, @RegionId) IN (R.RegionId, D.DistrictId) THEN 1 ELSE 0 END * SUM(ISNULL(((L.Families)*(0.01* Catchment)), 0))TotalFamilies
		FROM tblHFCatchment C
		LEFT JOIN tblLocations L ON L.LocationId = C.LocationId
		INNER JOIN tblHF HF ON C.HFID = HF.HfID
		INNER JOIN tblDistricts D ON HF.LocationId = D.DistrictId
		INNER JOIN tblRegions R ON D.Region = R.RegionId
		WHERE C.ValidityTo IS NULL
		AND L.ValidityTo IS NULL
		AND HF.ValidityTo IS NULL
		GROUP BY C.HFID, D.DistrictId, R.RegionId
	), InsuredInsuree AS
	(
		SELECT HC.HFID, @ProdId ProdId, COUNT(DISTINCT IP.InsureeId)*(0.01 * Catchment) TotalInsuredInsuree
		FROM tblInsureePolicy IP
		INNER JOIN tblInsuree I ON I.InsureeId = IP.InsureeId
		INNER JOIN tblFamilies F ON F.FamilyId = I.FamilyId
		INNER JOIN tblHFCatchment HC ON HC.LocationId = F.LocationId
		--INNER JOIN uvwLocations L ON L.LocationId = HC.LocationId
		INNER JOIN tblPolicy PL ON PL.PolicyID = IP.PolicyId
		WHERE HC.ValidityTo IS NULL 
		AND I.ValidityTo IS NULL
		AND IP.ValidityTo IS NULL
		AND F.ValidityTo IS NULL
		AND PL.ValidityTo IS NULL
		AND IP.EffectiveDate <= @LastDay 
		AND IP.ExpiryDate > @LastDay
		AND PL.ProdID = @ProdId
		GROUP BY HC.HFID, Catchment--, L.LocationId
	), InsuredFamilies AS
	(
		SELECT HC.HFID, COUNT(DISTINCT F.FamilyID)*(0.01 * Catchment) TotalInsuredFamilies
		FROM tblInsureePolicy IP
		INNER JOIN tblInsuree I ON I.InsureeId = IP.InsureeId
		INNER JOIN tblFamilies F ON F.InsureeID = I.InsureeID
		INNER JOIN tblHFCatchment HC ON HC.LocationId = F.LocationId
		--INNER JOIN uvwLocations L ON L.LocationId = HC.LocationId
		INNER JOIN tblPolicy PL ON PL.PolicyID = IP.PolicyId
		WHERE HC.ValidityTo IS NULL 
		AND I.ValidityTo IS NULL
		AND IP.ValidityTo IS NULL
		AND F.ValidityTo IS NULL
		AND PL.ValidityTo IS NULL
		AND IP.EffectiveDate <= @LastDay 
		AND IP.ExpiryDate > @LastDay
		AND PL.ProdID = @ProdId
		GROUP BY HC.HFID, Catchment--, L.LocationId
	), Claims AS
	(
		SELECT C.HFID,  COUNT(C.ClaimId)TotalClaims
		FROM tblClaim C
		INNER JOIN (
			SELECT ClaimId FROM tblClaimItems WHERE ProdId = @ProdId AND ValidityTo IS NULL
			UNION
			SELECT ClaimId FROM tblClaimServices WHERE ProdId = @ProdId AND ValidityTo IS NULL
			) CProd ON CProd.ClaimID = C.ClaimID
		WHERE C.ValidityTo IS NULL
		AND C.ClaimStatus >= 8
		AND YEAR(C.DateProcessed) = @Year
		AND MONTH(C.DateProcessed) = @Month
		GROUP BY C.HFID
	), ClaimValues AS
	(
		SELECT HFID, @ProdId ProdId, SUM(PriceValuated)TotalAdjusted
		FROM(
		SELECT C.HFID, CValue.PriceValuated
		FROM tblClaim C
		INNER JOIN (
			SELECT ClaimId, PriceValuated FROM tblClaimItems WHERE ValidityTo IS NULL AND ProdId = @ProdId
			UNION ALL
			SELECT ClaimId, PriceValuated FROM tblClaimServices WHERE ValidityTo IS NULL AND ProdId = @ProdId
			) CValue ON CValue.ClaimID = C.ClaimID
		WHERE C.ValidityTo IS NULL
		AND C.ClaimStatus >= 8
		AND YEAR(C.DateProcessed) = @Year
		AND MONTH(C.DateProcessed) = @Month
		)CValue
		GROUP BY HFID
	),Locations AS
	(
		SELECT 0 LocationId, N'National' LocationName, NULL ParentLocationId
		UNION
		SELECT LocationId,LocationName, ISNULL(ParentLocationId, 0) FROM tblLocations WHERE ValidityTo IS NULL AND LocationId = ISNULL(@DistrictId, @RegionId)
		UNION ALL
		SELECT L.LocationId, L.LocationName, L.ParentLocationId 
		FROM tblLocations L 
		INNER JOIN Locations ON Locations.LocationId = L.ParentLocationId
		WHERE L.validityTo IS NULL
		AND L.LocationType IN ('R', 'D')
	), Allocation AS
	(
		SELECT ProdId, CAST(SUM(ISNULL(Allocated, 0)) AS DECIMAL(18, 6))Allocated
		FROM
		(SELECT PL.ProdID,
		CASE 
		WHEN MONTH(DATEADD(D,-1,PL.ExpiryDate)) = @Month AND YEAR(DATEADD(D,-1,PL.ExpiryDate)) = @Year AND (DAY(PL.ExpiryDate)) > 1
			THEN CASE WHEN DATEDIFF(D,CASE WHEN PR.PayDate < @FirstDay THEN @FirstDay ELSE PR.PayDate END,PL.ExpiryDate) = 0 THEN 1 ELSE DATEDIFF(D,CASE WHEN PR.PayDate < @FirstDay THEN @FirstDay ELSE PR.PayDate END,PL.ExpiryDate) END  * ((SUM(PR.Amount))/(CASE WHEN (DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate)) <= 0 THEN 1 ELSE DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) END))
		WHEN MONTH(CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END) = @Month AND YEAR(CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END) = @Year
			THEN ((@DaysInMonth + 1 - DAY(CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END)) * ((SUM(PR.Amount))/CASE WHEN DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) <= 0 THEN 1 ELSE DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) END)) 
		WHEN PL.EffectiveDate < @FirstDay AND PL.ExpiryDate > @LastDay AND PR.PayDate < @FirstDay
			THEN @DaysInMonth * (SUM(PR.Amount)/CASE WHEN (DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,DATEADD(D,-1,PL.ExpiryDate))) <= 0 THEN 1 ELSE DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) END)
		END Allocated
		FROM tblPremium PR 
		INNER JOIN tblPolicy PL ON PR.PolicyID = PL.PolicyID
		INNER JOIN tblProduct Prod ON Prod.ProdId = PL.ProdID
		INNER JOIN Locations L ON ISNULL(Prod.LocationId, 0) = L.LocationId
		WHERE PR.ValidityTo IS NULL
		AND PL.ValidityTo IS NULL
		AND PL.ProdID = @ProdId
		AND PL.PolicyStatus <> 1
		AND PR.PayDate <= PL.ExpiryDate
		GROUP BY PL.ProdID, PL.ExpiryDate, PR.PayDate,PL.EffectiveDate)Alc
		GROUP BY ProdId
	)



	,ReportData AS
	(
		SELECT L.RegionCode, L.RegionName, L.DistrictCode, L.DistrictName, HF.HFCode, HF.HFName, Hf.AccCode, HL.Name HFLevel, SL.HFSublevelDesc HFSublevel,
		PF.[Population] [Population], PF.TotalFamilies TotalFamilies, II.TotalInsuredInsuree, IFam.TotalInsuredFamilies, C.TotalClaims, CV.TotalAdjusted
		,(
			  ISNULL(ISNULL(PF.[Population], 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightPopulation)) /  NULLIF(SUM(PF.[Population])OVER(),0),0)  
			+ ISNULL(ISNULL(PF.TotalFamilies, 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightNumberFamilies)) /NULLIF(SUM(PF.[TotalFamilies])OVER(),0),0) 
			+ ISNULL(ISNULL(II.TotalInsuredInsuree, 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightInsuredPopulation)) /NULLIF(SUM(II.TotalInsuredInsuree)OVER(),0),0) 
			+ ISNULL(ISNULL(IFam.TotalInsuredFamilies, 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightNumberInsuredFamilies)) /NULLIF(SUM(IFam.TotalInsuredFamilies)OVER(),0),0) 
			+ ISNULL(ISNULL(C.TotalClaims, 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightNumberVisits)) /NULLIF(SUM(C.TotalClaims)OVER() ,0),0) 
			+ ISNULL(ISNULL(CV.TotalAdjusted, 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightAdjustedAmount)) /NULLIF(SUM(CV.TotalAdjusted)OVER(),0),0)

		) PaymentCathment

		, Allocation.Allocated * (0.01 * @WeightPopulation) * (0.01 * @ShareContribution) AlcContriPopulation
		, Allocation.Allocated * (0.01 * @WeightNumberFamilies) * (0.01 * @ShareContribution) AlcContriNumFamilies
		, Allocation.Allocated * (0.01 * @WeightInsuredPopulation) * (0.01 * @ShareContribution) AlcContriInsPopulation
		, Allocation.Allocated * (0.01 * @WeightNumberInsuredFamilies) * (0.01 * @ShareContribution) AlcContriInsFamilies
		, Allocation.Allocated * (0.01 * @WeightNumberVisits) * (0.01 * @ShareContribution) AlcContriVisits
		, Allocation.Allocated * (0.01 * @WeightAdjustedAmount) * (0.01 * @ShareContribution) AlcContriAdjustedAmount

		,  ISNULL((Allocation.Allocated * (0.01 * @WeightPopulation) * (0.01 * @ShareContribution))/ NULLIF(SUM(PF.[Population]) OVER(),0),0) UPPopulation
		,  ISNULL((Allocation.Allocated * (0.01 * @WeightNumberFamilies) * (0.01 * @ShareContribution))/NULLIF(SUM(PF.TotalFamilies) OVER(),0),0) UPNumFamilies
		,  ISNULL((Allocation.Allocated * (0.01 * @WeightInsuredPopulation) * (0.01 * @ShareContribution))/NULLIF(SUM(II.TotalInsuredInsuree) OVER(),0),0) UPInsPopulation
		,  ISNULL((Allocation.Allocated * (0.01 * @WeightNumberInsuredFamilies) * (0.01 * @ShareContribution))/ NULLIF(SUM(IFam.TotalInsuredFamilies) OVER(),0),0) UPInsFamilies
		,  ISNULL((Allocation.Allocated * (0.01 * @WeightNumberVisits) * (0.01 * @ShareContribution)) / NULLIF(SUM(C.TotalClaims) OVER(),0),0) UPVisits
		,  ISNULL((Allocation.Allocated * (0.01 * @WeightAdjustedAmount) * (0.01 * @ShareContribution))/ NULLIF(SUM(CV.TotalAdjusted) OVER(),0),0) UPAdjustedAmount




		FROM tblHF HF
		INNER JOIN @HFLevel HL ON HL.Code = HF.HFLevel
		LEFT OUTER JOIN tblHFSublevel SL ON SL.HFSublevel = HF.HFSublevel
		LEFT JOIN uvwLocations L ON L.LocationId = HF.LocationId
		LEFT OUTER JOIN TotalPopFam PF ON PF.HFID = HF.HfID
		LEFT OUTER JOIN InsuredInsuree II ON II.HFID = HF.HfID
		LEFT OUTER JOIN InsuredFamilies IFam ON IFam.HFID = HF.HfID
		LEFT OUTER JOIN Claims C ON C.HFID = HF.HfID
		LEFT OUTER JOIN ClaimValues CV ON CV.HFID = HF.HfID
		LEFT OUTER JOIN Allocation ON Allocation.ProdID = @ProdId

		WHERE HF.ValidityTo IS NULL
		AND (((L.RegionId = @RegionId OR @RegionId IS NULL) AND (L.DistrictId = @DistrictId OR @DistrictId IS NULL)) OR CV.ProdID IS NOT NULL OR II.ProdId IS NOT NULL)
		AND (HF.HFLevel IN (@Level1, @Level2, @Level3, @Level4) OR (@Level1 IS NULL AND @Level2 IS NULL AND @Level3 IS NULL AND @Level4 IS NULL))
		AND(
			((HF.HFLevel = @Level1 OR @Level1 IS NULL) AND (HF.HFSublevel = @Sublevel1 OR @Sublevel1 IS NULL))
			OR ((HF.HFLevel = @Level2 ) AND (HF.HFSublevel = @Sublevel2 OR @Sublevel2 IS NULL))
			OR ((HF.HFLevel = @Level3) AND (HF.HFSublevel = @Sublevel3 OR @Sublevel3 IS NULL))
			OR ((HF.HFLevel = @Level4) AND (HF.HFSublevel = @Sublevel4 OR @Sublevel4 IS NULL))
		  )

	)



	SELECT  MAX (RegionCode)RegionCode, 
			MAX(RegionName)RegionName,
			MAX(DistrictCode)DistrictCode,
			MAX(DistrictName)DistrictName,
			HFCode, 
			MAX(HFName)HFName,
			MAX(AccCode)AccCode, 
			MAX(HFLevel)HFLevel, 
			MAX(HFSublevel)HFSublevel,
			ISNULL(SUM([Population]),0)[Population],
			ISNULL(SUM(TotalFamilies),0)TotalFamilies,
			ISNULL(SUM(TotalInsuredInsuree),0)TotalInsuredInsuree,
			ISNULL(SUM(TotalInsuredFamilies),0)TotalInsuredFamilies,
			ISNULL(MAX(TotalClaims), 0)TotalClaims,
			ISNULL(SUM(AlcContriPopulation),0)AlcContriPopulation,
			ISNULL(SUM(AlcContriNumFamilies),0)AlcContriNumFamilies,
			ISNULL(SUM(AlcContriInsPopulation),0)AlcContriInsPopulation,
			ISNULL(SUM(AlcContriInsFamilies),0)AlcContriInsFamilies,
			ISNULL(SUM(AlcContriVisits),0)AlcContriVisits,
			ISNULL(SUM(AlcContriAdjustedAmount),0)AlcContriAdjustedAmount,
			ISNULL(SUM(UPPopulation),0)UPPopulation,
			ISNULL(SUM(UPNumFamilies),0)UPNumFamilies,
			ISNULL(SUM(UPInsPopulation),0)UPInsPopulation,
			ISNULL(SUM(UPInsFamilies),0)UPInsFamilies,
			ISNULL(SUM(UPVisits),0)UPVisits,
			ISNULL(SUM(UPAdjustedAmount),0)UPAdjustedAmount,
			ISNULL(SUM(PaymentCathment),0)PaymentCathment,
			ISNULL(SUM(TotalAdjusted),0)TotalAdjusted
	
	 FROM ReportData

	 GROUP BY HFCode



END
GO

--Number of Insured House holds
ALTER VIEW [dw].[uvwNumberOfInsuredHouseholds]
AS
	WITH RowData AS
	(
		SELECT F.FamilyID, DATEADD(MONTH,MonthCount.Numbers, EOMONTH(PL.EffectiveDate, 0)) ActiveDate, 
		R.RegionName Region, D.DistrictName, W.WardName, V.VillageName
		FROM tblPolicy PL 
		INNER JOIN tblFamilies F ON PL.FamilyId = F.FamilyID
		INNER JOIN tblVillages V ON V.VillageId = F.LocationId
		INNER JOIN tblWards W ON W.WardId = V.WardId
		INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
		INNER JOIN tblRegions R ON D.Region = R.RegionId 
		CROSS APPLY(VALUES (0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11))MonthCount(Numbers)
		WHERE PL.ValidityTo IS NULL
		AND F.ValidityTo IS NULL
		AND R.ValidityTo IS NULL
		AND D.ValidityTo IS NULL
		AND W.ValidityTo IS NULL
		AND V.ValidityTo IS NULL
		AND PL.EffectiveDate IS NOT NULL
		GROUP BY F.FamilyID,R.RegionName,D.DistrictName,W.WardName,v.VillageName,DATEADD(MONTH,MonthCount.Numbers, EOMONTH(PL.EffectiveDate, 0)) 
	), RowData2 AS
	(
		SELECT FamilyId, ActiveDate, Region, DistrictName, WardName, VillageName
		FROM RowData
		GROUP BY FamilyId, ActiveDate, Region, DistrictName, WardName, VillageName
	)
	SELECT COUNT(FamilyId) InsuredHouseholds, MONTH(ActiveDate)MonthTime, DATENAME(Q, ActiveDate)QuarterTime, YEAR(ActiveDate)YearTime, Region, DistrictName, WardName, VillageName
	FROM RowData2
	GROUP BY ActiveDate, Region, DistrictName, WardName, VillageName

GO

--Get Claim Overview
IF NOT OBJECT_ID('uspSSRSGetClaimOverView') IS NULL
DROP PROCEDURE uspSSRSGetClaimOverView

GO

CREATE PROCEDURE [dbo].[uspSSRSGetClaimOverView]
(
	@HFID INT,
	@LocationId INT,
	@ProdId INT, 
	@StartDate DATE, 
	@EndDate DATE,
	@ClaimStatus INT = NULL
)
AS
BEGIN
	;WITH TotalForItems AS
	(
		SELECT C.ClaimId, SUM(CI.PriceAsked * CI.QtyProvided)Claimed,
		SUM(ISNULL(CI.PriceApproved, CI.PriceAsked) * ISNULL(CI.QtyApproved, CI.QtyProvided)) Approved,
		SUM(CI.PriceValuated)Adjusted,
		SUM(CI.RemuneratedAmount)Remunerated
		FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
		WHERE C.ValidityTo IS NULL
		AND CI.ValidityTo IS NULL
		GROUP BY C.ClaimID
	), TotalForServices AS
	(
		SELECT C.ClaimId, SUM(CS.PriceAsked * CS.QtyProvided)Claimed,
		SUM(ISNULL(CS.PriceApproved, CS.PriceAsked) * ISNULL(CS.QtyApproved, CS.QtyProvided)) Approved,
		SUM(CS.PriceValuated)Adjusted,
		SUM(CS.RemuneratedAmount)Remunerated
		FROM tblClaim C 
		LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
		WHERE C.ValidityTo IS NULL
		AND CS.ValidityTo IS NULL
		GROUP BY C.ClaimID
	)

	SELECT C.DateClaimed, C.ClaimID, I.ItemId, S.ServiceID, HF.HFCode, HF.HFName, C.ClaimCode, C.DateClaimed, CA.LastName + ' ' + CA.OtherNames ClaimAdminName,
	C.DateFrom, C.DateTo, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsureeName,
	CASE C.ClaimStatus WHEN 1 THEN N'Rejected' WHEN 2 THEN N'Entered' WHEN 4 THEN N'Checked' WHEN 8 THEN N'Processed' WHEN 16 THEN N'Valuated' END ClaimStatus,
	C.RejectionReason, COALESCE(TFI.Claimed + TFS.Claimed, TFI.Claimed, TFS.Claimed) Claimed, 
	COALESCE(TFI.Approved + TFS.Approved, TFI.Approved, TFS.Approved) Approved,
	COALESCE(TFI.Adjusted + TFS.Adjusted, TFI.Adjusted, TFS.Adjusted) Adjusted,
	COALESCE(TFI.Remunerated + TFS.Remunerated, TFI.Remunerated, TFS.Remunerated)Paid,
	CASE WHEN CI.RejectionReason <> 0 THEN I.ItemCode ELSE NULL END RejectedItem, CI.RejectionReason ItemRejectionCode,
	CASE WHEN CS.RejectionReason > 0 THEN S.ServCode ELSE NULL END RejectedService, CS.RejectionReason ServiceRejectionCode,
	CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN I.ItemCode ELSE NULL END AdjustedItem,
	CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN CI.QtyProvided ELSE NULL END OrgQtyItem,
	CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved ,CI.QtyProvided)  THEN CI.QtyApproved ELSE NULL END AdjQtyItem,
	CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)  THEN S.ServCode ELSE NULL END AdjustedService,
	CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)   THEN CS.QtyProvided ELSE NULL END OrgQtyService,
	CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved ,CS.QtyProvided)   THEN CS.QtyApproved ELSE NULL END AdjQtyService,
	C.Explanation


	FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
	LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
	LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID
	LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID
	INNER JOIN tblHF HF ON C.HFID = HF.HfID
	LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
	INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
	LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
	LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId

	WHERE C.ValidityTo IS NULL
	AND ISNULL(C.DateTo,C.DateFrom) BETWEEN @StartDate AND @EndDate
	AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
	AND HF.LocationId = @LocationId OR @LocationId = 0
	AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) 
	--OR  (CI.ProdID IS NOT NULL OR CS.ProdID IS NOT NULL)	-- --Added by Rogers
	AND HF.HFID = @HFID
END

GO

--Clean tables new
IF NOT OBJECT_ID('uspCleanTablesNew') IS NULL
DROP PROCEDURE uspCleanTablesNew
GO

CREATE PROCEDURE [dbo].[uspCleanTablesNew]
	@OffLine as int = 0		--0: For Online, 1: HF Offline, 2: CHF Offline
AS
BEGIN
	
	DECLARE @LocationId INT
	DECLARE @ParentLocationId INT

	--SELECT @ParentLocationId = ParentLocationId, @LocationId =LocationId  FROM tblLocations WHERE LocationName='Dummy' AND ValidityTo IS NULL AND LocationType = N'D'
	 
	--Phase 2
	DELETE FROM tblFeedbackPrompt;
	EXEC [UspS_ReseedTable] 'tblFeedbackPrompt';
	DELETE FROM tblReporting;
	EXEC [UspS_ReseedTable] 'tblReporting';
	DELETE FROM tblSubmittedPhotos;
	EXEC [UspS_ReseedTable] 'tblSubmittedPhotos';
	DELETE FROM dbo.tblFeedback;
	EXEC [UspS_ReseedTable] 'tblFeedback';
	DELETE FROM dbo.tblClaimServices;
	EXEC [UspS_ReseedTable] 'tblClaimServices';
	DELETE FROM dbo.tblClaimItems ;
	EXEC [UspS_ReseedTable] 'tblClaimItems';
	DELETE FROM dbo.tblClaimDedRem;
	EXEC [UspS_ReseedTable] 'tblClaimDedRem';
	DELETE FROM dbo.tblClaim;
	EXEC [UspS_ReseedTable] 'tblClaim';
	DELETE FROM dbo.tblClaimAdmin
	EXEC [USPS_ReseedTable] 'tblClaimAdmin'
	DELETE FROM dbo.tblICDCodes;
	EXEC [UspS_ReseedTable] 'tblICDCodes'
	
	
	DELETE FROM dbo.tblRelDistr;
	EXEC [UspS_ReseedTable] 'tblRelDistr';
	DELETE FROM dbo.tblRelIndex ;
	EXEC [UspS_ReseedTable] 'tblRelIndex';
	DELETE FROM dbo.tblBatchRun;
	EXEC [UspS_ReseedTable] 'tblBatchRun';
	DELETE FROM dbo.tblExtracts;
	EXEC [UspS_ReseedTable] 'tblExtracts';
	TRUNCATE TABLE tblPremium;
	
	--Phase 1
	EXEC [UspS_ReseedTable] 'tblPremium';
	DELETE FROM tblPayer;
	EXEC [UspS_ReseedTable] 'tblPayer';

	DELETE FROM dbo.tblPolicyRenewalDetails;
	EXEC [UspS_ReseedTable] 'tblPolicyRenewalDetails';
	DELETE FROM dbo.tblPolicyRenewals;
	EXEC [UspS_ReseedTable] 'tblPolicyRenewals';


	DELETE FROM tblInsureePolicy;
	EXEC [UspS_ReseedTable] 'tblInsureePolicy';
	DELETE FROM tblPolicy;
	EXEC [UspS_ReseedTable] 'tblPolicy';
	DELETE FROM tblProductItems;
	EXEC [UspS_ReseedTable] 'tblProductItems';
	DELETE FROM tblProductServices;
	EXEC [UspS_ReseedTable] 'tblProductServices';

	DELETE FROM dbo.tblRelDistr;
	EXEC [UspS_ReseedTable] 'tblRelDistr';


	DELETE FROM tblProduct;
	EXEC [UspS_ReseedTable] 'tblProduct';
	UPDATE tblInsuree set PhotoID = NULL ;
	DELETE FROM tblPhotos;
	EXEC [UspS_ReseedTable] 'tblPhotos';
	DELETE FROM tblInsuree;
	EXEC [UspS_ReseedTable] 'tblInsuree';
	DELETE FROM tblGender;
	DELETE FROM tblFamilies;
	EXEC [UspS_ReseedTable] 'tblFamilies';
	DELETE FROM tblOfficerVillages;
	EXEC [UspS_ReseedTable] 'tblOfficerVillages';
	DELETE FROM dbo.tblOfficer;
	EXEC [UspS_ReseedTable] 'tblOfficer';
	DELETE FROM dbo.tblHFCatchment;
	EXEC [UspS_ReseedTable] 'tblHFCatchment';
	DELETE FROM dbo.tblHF;
	EXEC [UspS_ReseedTable] 'tblHF';
	DELETe FROM dbo.tblPLItemsDetail;
	EXEC [UspS_ReseedTable] 'tblPLItemsDetail';
	DELETE FROM dbo.tblPLItems;
	EXEC [UspS_ReseedTable] 'tblPLItems';
	DELETE FROM dbo.tblItems;
	EXEC [UspS_ReseedTable] 'tblItems';
	DELETE FROM dbo.tblPLServicesDetail;
	EXEC [UspS_ReseedTable] 'tblPLServicesDetail';
	DELETE FROM dbo.tblPLServices;
	EXEC [UspS_ReseedTable] 'tblPLServices';
	DELETE FROM dbo.tblServices;
	EXEC [UspS_ReseedTable] 'tblServices';
	DELETE FROM dbo.tblUsersDistricts;
	EXEC [UspS_ReseedTable] 'tblUsersDistricts';


	DELETE FROM tblLocations;
	EXEC [UspS_ReseedTable] 'tblLocations';
	DELETE FROM dbo.tblLogins ;
	EXEC [UspS_ReseedTable] 'tblLogins';

	DELETE FROM dbo.tblUsers;
	EXEC [UspS_ReseedTable] 'tblUsers';

	TRUNCATE TABLE tblFromPhone;
	EXEC [UspS_ReseedTable] 'tblFromPhone';

	TRUNCATE TABLE tblEmailSettings;

	DBCC SHRINKDATABASE (0);
	
	--Drop the encryption set
	IF EXISTS(SELECT * FROM sys.symmetric_keys WHERE name = N'EncryptionKey')
	DROP SYMMETRIC KEY EncryptionKey;
		
	IF EXISTS(SELECT * FROM sys.certificates WHERE name = N'EncryptData')
	DROP CERTIFICATE EncryptData;
	
	IF EXISTS(SELECT * FROM sys.symmetric_keys WHERE symmetric_key_id = 101)
	DROP MASTER KEY;
	
	
	
	
	
	--insert new user Admin-Admin
	IF @OffLine = 2  --CHF offline
	BEGIN
		
        INSERT INTO tblUsers ([LastName],[OtherNames],[Phone],[LoginName],[RoleID],[LanguageID],[HFID],[AuditUserID],StoredPassword,PrivateKey)
        VALUES('Admin', 'Admin', '', 'Admin', 1048576,'en',0,0
		--storedPassword
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CONCAT(CAST('Admin' AS VARCHAR(MAX)),CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2))),2)
		 -- PrivateKey
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2)
		)
       
        UPDATE tblIMISDefaults SET OffLineHF = 0,OfflineCHF = 0, FTPHost = '',FTPUser = '', FTPPassword = '',FTPPort = 0,FTPClaimFolder = '',FtpFeedbackFolder = '',FTPPolicyRenewalFolder = '',FTPPhoneExtractFolder = '',FTPOfflineExtractFolder = '',AppVersionEnquire = 0,AppVersionEnroll = 0,AppVersionRenewal = 0,AppVersionFeedback = 0,AppVersionClaim = 0, AppVersionImis = 0, DatabaseBackupFolder = ''
        
	END
	
	
	IF @OffLine = 1 --HF offline
	BEGIN
		
        INSERT INTO tblUsers ([LastName],[OtherNames],[Phone],[LoginName],[RoleID],[LanguageID],[HFID],[AuditUserID],StoredPassword,PrivateKey)
        VALUES('Admin', 'Admin', '', 'Admin', 524288,'en',0,0
		--storedPassword
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CONCAT(CAST('Admin' AS VARCHAR(MAX)),CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2))),2)
		 -- PrivateKey
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2)
		)
        
        UPDATE tblIMISDefaults SET OffLineHF = 0,OfflineCHF = 0,FTPHost = '',FTPUser = '', FTPPassword = '',FTPPort = 0,FTPClaimFolder = '',FtpFeedbackFolder = '',FTPPolicyRenewalFolder = '',FTPPhoneExtractFolder = '',FTPOfflineExtractFolder = '',AppVersionEnquire = 0,AppVersionEnroll = 0,AppVersionRenewal = 0,AppVersionFeedback = 0,AppVersionClaim = 0, AppVersionImis = 0, DatabaseBackupFolder = ''
        
	END
	IF @OffLine = 0 --ONLINE CREATION NEW COUNTRY NO DEFAULTS KEPT
	BEGIN
		
        INSERT INTO tblUsers ([LastName],[OtherNames],[Phone],[LoginName],[RoleID],[LanguageID],[HFID],[AuditUserID],StoredPassword,PrivateKey)
        VALUES('Admin', 'Admin', '', 'Admin',  1023,'en',0,0
		--storedPassword
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CONCAT(CAST('Admin' AS VARCHAR(MAX)),CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2))),2)
		 -- PrivateKey
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2)
		)
		
       	UPDATE tblIMISDefaults SET OffLineHF = 0,OfflineCHF = 0,FTPHost = '',FTPUser = '', FTPPassword = '',FTPPort = 0,FTPClaimFolder = '',FtpFeedbackFolder = '',FTPPolicyRenewalFolder = '',FTPPhoneExtractFolder = '',FTPOfflineExtractFolder = '',AppVersionEnquire = 0,AppVersionEnroll = 0,AppVersionRenewal = 0,AppVersionFeedback = 0,AppVersionClaim = 0, AppVersionImis = 0,				DatabaseBackupFolder = ''
    END
	
	IF @OffLine = -1 --ONLINE CREATION WITH DEFAULTS KEPT AS PREVIOUS CONTENTS
	BEGIN
		
        INSERT INTO tblUsers ([LastName],[OtherNames],[Phone],[LoginName],[RoleID],[LanguageID],[HFID],[AuditUserID],StoredPassword,PrivateKey)
        VALUES('Admin', 'Admin', '', 'Admin', 1023,'en',0,0
		--storedPassword
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CONCAT(CAST('Admin' AS VARCHAR(MAX)),CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2))),2)
		 -- PrivateKey
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2)
		)
        UPDATE tblIMISDefaults SET OffLineHF = 0,OfflineCHF = 0
    END


	SET IDENTITY_INSERT tblLocations ON
	INSERT INTO tblLocations(LocationId, LocationCode, Locationname, LocationType, AuditUserId, ParentLocationId) VALUES
	(1, N'R0001', N'Region', N'R', -1, NULL),
	(2, N'D0001', N'Dummy', N'D', -1, 1)
	SET IDENTITY_INSERT tblLocations OFF
		
	INSERT INTO tblUsersDistricts ([UserID],[LocationId],[AuditUserID]) VALUES (1,2,-1)
END
GO


-- ********* < END CONVERGE MULTIPLE SCRIPTS INTO ONE SCRIPT 15 -02 -2019 > ***************