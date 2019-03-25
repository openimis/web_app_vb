--Declare variable
DECLARE @RoleID as INT	
DECLARE @Rights AS TABLE(RightID INT)  

INSERT INTO @Rights VALUES(111001)--ClaimSearch
INSERT INTO @Rights VALUES(111002)--ClaimAdd
INSERT INTO @Rights VALUES(111004)--ClaimDelete
INSERT INTO @Rights VALUES(111005)--ClaimLoad
INSERT INTO @Rights VALUES(111006)--ClaimPrint  
INSERT INTO @Rights VALUES(111007)--ClaimSubmit
SELECT @RoleID = RoleID from tblRole WHERE Rolename ='Claim Administrator'	
--Uncheck
DELETE FROM tblRoleRight WHERE RoleID = @RoleID AND RightID NOT IN (SELECT RightID FROM @Rights)

 --Setting value

INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom) 
SELECT @RoleID,Ro.RightID,GETDATE() FROM @Rights Ro 
		LEFT OUTER JOIN tblRoleRight Rr ON Rr.RoleID =@RoleID 
		AND Rr.RightID = Ro.RightID 
		AND Rr.ValidityTo IS NULL 
		WHERE Rr.RoleRightID IS NULL
