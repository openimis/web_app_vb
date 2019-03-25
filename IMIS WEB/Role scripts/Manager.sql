							
--Declare variable
DECLARE @RoleID as INT
DECLARE @Rights AS TABLE(RightID INT)  
INSERT INTO @Rights VALUES(131201)--ReportsPrimaryOperationalIndicators-policies
INSERT INTO @Rights VALUES(131202)--ReportsPrimaryOperationalIndicatorsClaims
INSERT INTO @Rights VALUES(131203)--ReportsDerivedOperationalIndicators
INSERT INTO @Rights VALUES(131208)--ReportsEnrolmentPerformanceIndicators
INSERT INTO @Rights VALUES(101105)--InsureeEnquire
SELECT @RoleID = RoleID from tblRole WHERE Rolename ='Manager'	
	--Uncheck
DELETE FROM tblRoleRight WHERE RoleID = @RoleID AND RightID NOT IN (SELECT RightID FROM @Rights)

 --Setting value
	
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom) 
SELECT @RoleID,Ro.RightID,GETDATE() FROM @Rights Ro 
		LEFT OUTER JOIN tblRoleRight Rr ON Rr.RoleID =@RoleID 
		AND Rr.RightID = Ro.RightID 
		AND Rr.ValidityTo IS NULL 
		WHERE Rr.RoleRightID IS NULL
