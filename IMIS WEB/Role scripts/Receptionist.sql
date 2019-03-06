				--START Receptionist--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Receptionist'
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --InsureeEnquire  
					--END Receptionist--