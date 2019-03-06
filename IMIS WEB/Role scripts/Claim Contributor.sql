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