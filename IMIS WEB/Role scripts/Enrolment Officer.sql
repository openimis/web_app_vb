					--START Enrolement Officer--
SELECT @ID = RoleID from tblRole WHERE Rolename ='Enrolement Officer' 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101002 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101002,GETDATE()) --FamilyAdd 
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101003 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101003,GETDATE()) --FamilyEdit  
 
IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101004 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101004,GETDATE()) --FamilyDelete  

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101102 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101102,GETDATE()) --InsureeAdd  

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101103 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101103,GETDATE()) --InsureeEdit  

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101104 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101104,GETDATE()) --InsureeDelete 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101105 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101105,GETDATE()) --InsureeEnquire 

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
VALUES (@ID,101205,GETDATE()) --PolicyRenewal 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101302 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101302,GETDATE()) --ContributionAdd 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101303 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101303,GETDATE()) --ContributionEdit 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 101304 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,101304,GETDATE()) --ContributionDelete 

IF (SELECT 1 FROM tblRoleRight WHERE RightID = 111009 AND ROLEID = @ID) IS NULL
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom)
VALUES (@ID,111009,GETDATE()) --ClaimFeedback 
							--End Enrolment Officer--