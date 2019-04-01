--Declare variable
DECLARE @RoleID as INT
DECLARE @Rights AS TABLE(RightID INT) 
INSERT INTO @Rights VALUES(101001)--FamilySearch
INSERT INTO @Rights VALUES(101002)--FamilyAdd
INSERT INTO @Rights VALUES(101003)--FamilyEdit
INSERT INTO @Rights VALUES(101004)--FamilyDelete 
INSERT INTO @Rights VALUES(101101)--InsureeSearch
INSERT INTO @Rights VALUES(101102)--InsureeAdd
INSERT INTO @Rights VALUES(101103)--InsureeEdit
INSERT INTO @Rights VALUES(101104)--InsureeDelete
INSERT INTO @Rights VALUES(101105)--InsureeEnquire
INSERT INTO @Rights VALUES(101201)--PolicySearch 
INSERT INTO @Rights VALUES(101202)--PolicyAdd
INSERT INTO @Rights VALUES(101203)--PolicyEdit
INSERT INTO @Rights VALUES(101204)--PolicyDelete
INSERT INTO @Rights VALUES(101205)--PolicyRenew
INSERT INTO @Rights VALUES(101301)--ContributionSearch   
INSERT INTO @Rights VALUES(101302)--ContributionAdd
INSERT INTO @Rights VALUES(101303)--ContributionEdit
INSERT INTO @Rights VALUES(101304)--ContributionDelete
INSERT INTO @Rights VALUES(111001)--ClaimSearch  
INSERT INTO @Rights VALUES(111009)--ClaimFeedback 
SELECT @RoleID = RoleID from tblRole WHERE Rolename ='Enrolement Officer' AND ValidityTo IS NULL	
--Uncheck
DELETE FROM tblRoleRight WHERE RoleID = @RoleID AND RightID NOT IN (SELECT RightID FROM @Rights)

--Setting value	

INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom) 
SELECT @RoleID,Ro.RightID,GETDATE() FROM @Rights Ro 
		LEFT OUTER JOIN tblRoleRight Rr ON Rr.RoleID =@RoleID 
		AND Rr.RightID = Ro.RightID 
		AND Rr.ValidityTo IS NULL 
		WHERE Rr.RoleRightID IS NULL 