							
--Declare variable
DECLARE @RoleID as INT
	
DECLARE @Rights AS TABLE(RightID INT) 
INSERT INTO @Rights VALUES(101105)--InsureeEnquire
INSERT INTO @Rights VALUES(121701)--UsersSearch
INSERT INTO @Rights VALUES(121702)--UsersAdd
INSERT INTO @Rights VALUES(121703)--UsersEdit
INSERT INTO @Rights VALUES(121704)--UsersDelete 
INSERT INTO @Rights VALUES(122001)--UserProfilesSearch
INSERT INTO @Rights VALUES(122002)--UserProfilesAdd
INSERT INTO @Rights VALUES(122003)--UserProfilesEdit
INSERT INTO @Rights VALUES(122004)--UserProfilesDelete 
INSERT INTO @Rights VALUES(122005)--UserProfilesDuplicate
INSERT INTO @Rights VALUES(131207)--ReportsUserActivity
INSERT INTO @Rights VALUES(131301)--Backup
INSERT INTO @Rights VALUES(131302)--Restore
INSERT INTO @Rights VALUES(131303)--ExecuteScript
INSERT INTO @Rights VALUES(131304)--EmailSetting

SELECT @RoleID = RoleID from tblRole WHERE Rolename ='IMIS Administrator'	
--Uncheck
DELETE FROM tblRoleRight WHERE RoleID = @RoleID AND RightID NOT IN (SELECT RightID FROM @Rights)

 --Setting value

INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom) 
SELECT @RoleID,Ro.RightID,GETDATE() FROM @Rights Ro 
		LEFT OUTER JOIN tblRoleRight Rr ON Rr.RoleID =@RoleID 
		AND Rr.RightID = Ro.RightID 
		AND Rr.ValidityTo IS NULL 
		WHERE Rr.RoleRightID IS NULL
