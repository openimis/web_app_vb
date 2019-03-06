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