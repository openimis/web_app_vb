
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