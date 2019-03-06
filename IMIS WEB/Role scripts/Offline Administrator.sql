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