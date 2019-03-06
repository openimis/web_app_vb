							-- Accountant--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Accountant'
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101101 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --InsureeSearch

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

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101102 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101102,GETDATE()) --InsureeAdd

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101103,GETDATE()) --InsureeEdit

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131401 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131401,GETDATE()) --AddFund
/*previous rights*/
					--END Accountant--