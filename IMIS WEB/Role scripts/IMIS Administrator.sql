							
--Declare variable
DECLARE @RoleID as INT
	
DECLARE @Rights AS TABLE(RightID INT) 
INSERT INTO @Rights VALUES(101105)--InsureeEnquire
INSERT INTO @Rights VALUES(121701)--UsersSearch
INSERT INTO @Rights VALUES(121702)--UsersAdd
INSERT INTO @Rights VALUES(121703)--UsersEdit
INSERT INTO @Rights VALUES(121704)--UsersDelete 

--RFC 108 22-03-2019 IMIS Administrator gets rights to the register of locations  
INSERT INTO @Rights VALUES(121901)--LocationsSearch   
INSERT INTO @Rights VALUES(121902)--LocationsAdd      
INSERT INTO @Rights VALUES(121903)--LocationsEdit     
INSERT INTO @Rights VALUES(121904)--LocationsDelete  
INSERT INTO @Rights VALUES(121905)--LocationsMove  
--end rfc 108
 
 --RFC 108 (IMIS Administrator loses all rights to the register of user profile previous has rights

--RFC 108 22-03-2019 IMIS Administrator gets rights to the register of locations  upload / download
INSERT INTO @Rights VALUES(131005)--LocationsUpload          
INSERT INTO @Rights VALUES(131006)--LocationsDownload   
--end rfc 108

INSERT INTO @Rights VALUES(131207)--ReportsUserActivity
INSERT INTO @Rights VALUES(131301)--Backup
INSERT INTO @Rights VALUES(131302)--Restore
INSERT INTO @Rights VALUES(131303)--ExecuteScript
INSERT INTO @Rights VALUES(131304)--EmailSetting

--Setting value
SELECT @RoleID = RoleID from tblRole WHERE Rolename ='IMIS Administrator' AND ValidityTo IS NULL	
--Uncheck
DELETE FROM tblRoleRight WHERE RoleID = @RoleID AND RightID NOT IN (SELECT RightID FROM @Rights)
 
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom) 
SELECT @RoleID,Ro.RightID,GETDATE() FROM @Rights Ro 
		LEFT OUTER JOIN tblRoleRight Rr ON Rr.RoleID =@RoleID 
		AND Rr.RightID = Ro.RightID 
		AND Rr.ValidityTo IS NULL 
		WHERE Rr.RoleRightID IS NULL 


