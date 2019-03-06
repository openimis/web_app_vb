				--START Manager--					 
SELECT @ID = RoleID from tblRole WHERE Rolename ='Manager'
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --EnquireInsurees 
 
--Status of Registers
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 131209 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,131209,GETDATE()) --StatusofRegisters 
					--END Manager--