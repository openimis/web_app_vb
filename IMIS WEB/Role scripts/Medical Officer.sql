							--Medical Officer--
DECLARE @RoleID as INT
DECLARE @Rights AS TABLE(RightID INT)
INSERT INTO @Rights VALUES(111001)--ClaimSearch
INSERT INTO @Rights VALUES(111008)--ClaimReview
INSERT INTO @Rights VALUES(111009)--ClaimFeedback
INSERT INTO @Rights VALUES(111010)--ClaimUpdate
INSERT INTO @Rights VALUES(111011)--ClaimProcess
INSERT INTO @Rights VALUES(131223)--ReportsClaimHistoryReport

SELECT @RoleID = RoleID from tblRole WHERE Rolename ='Medical Officer'
DELETE FROM tblRoleRight WHERE RoleID = @RoleID AND RightID NOT IN (SELECT RightID FROM @Rights)


	
INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom) 
SELECT @RoleID,Ro.RightID,GETDATE() FROM @Rights Ro 
		LEFT OUTER JOIN tblRoleRight Rr ON Rr.RoleID =@RoleID 
		AND Rr.RightID = Ro.RightID 
		AND Rr.ValidityTo IS NULL 
		WHERE Rr.RoleRightID IS NULL
					--END Medical Officer