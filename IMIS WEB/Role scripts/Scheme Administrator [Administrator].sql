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
					--END Administrator-- 