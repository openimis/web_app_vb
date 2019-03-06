
					--START Claim Administrator--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Claim Administrator' 
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

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111006 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111006,GETDATE()) --PrintClaim
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131214 AND ROLEID = @ID) IS NULL 
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131214,GETDATE()) --PercentageReferrals
					--END Claim Administrator--