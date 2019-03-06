				-- START Administrator-- 
SELECT @ID = RoleID from tblRole WHERE Rolename ='Administrator'
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --EnquireInsuree

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
				/*Previous rights*/
 
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
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121701 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121701,GETDATE()) --FindUser

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121702 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121702,GETDATE()) --AddUser

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121703 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121703,GETDATE()) --EditUser

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 121704 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,121704,GETDATE()) --DeleteUser

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122001 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122001,GETDATE()) --FindUserProfile

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122002,GETDATE()) --AddUserProfile

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122003,GETDATE()) --EditUserProfile

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122004 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122004,GETDATE()) --DeleteUserProfile

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 122005 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,122005,GETDATE()) --DuplicateUserProfile

					--END Administrator-- 