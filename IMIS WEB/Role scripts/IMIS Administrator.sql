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