							
--Declare variable
DECLARE @RoleID as INT		
DECLARE @Rights AS TABLE(RightID INT)  
INSERT INTO @Rights VALUES(101001)--FamilySearch  
INSERT INTO @Rights VALUES(101101)--InsureeSearch 
INSERT INTO @Rights VALUES(101105)--InsureeEnquire 
INSERT INTO @Rights VALUES(101201)--PolicySearch
SELECT @RoleID = RoleID from tblRole WHERE Rolename ='Receptionist'
--Uncheck
DELETE FROM tblRoleRight WHERE RoleID = @RoleID AND RightID NOT IN (SELECT RightID FROM @Rights)

 --Setting value

INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom) 
SELECT @RoleID,Ro.RightID,GETDATE() FROM @Rights Ro 
		LEFT OUTER JOIN tblRoleRight Rr ON Rr.RoleID =@RoleID 
		AND Rr.RightID = Ro.RightID 
		AND Rr.ValidityTo IS NULL 
		WHERE Rr.RoleRightID IS NULL
