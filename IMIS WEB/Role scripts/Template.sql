							
		
DECLARE @Rights AS TABLE(RightID INT)
INSERT INTO @Rights VALUES(101001)--FamilySearch
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
INSERT INTO @Rights VALUES(101401)--PaymentSearch
INSERT INTO @Rights VALUES(101402)--PaymentAdd
INSERT INTO @Rights VALUES(101403)--PaymentEdit
INSERT INTO @Rights VALUES(101404)--PaymentDelete
INSERT INTO @Rights VALUES(111005)--LoadClaim
INSERT INTO @Rights VALUES(111006)--PrintClaim
INSERT INTO @Rights VALUES(111008)--ReviewClaim
INSERT INTO @Rights VALUES(111101)--BatchProcess
INSERT INTO @Rights VALUES(111102)--BatchFilter
INSERT INTO @Rights VALUES(111103)--BatchPreview
INSERT INTO @Rights VALUES(131201)--ReportsPrimaryOperationalIndicators-policies
INSERT INTO @Rights VALUES(131202)--ReportsPrimaryOperationalIndicatorsClaims
INSERT INTO @Rights VALUES(131203)--ReportsDerivedOperationalIndicators
INSERT INTO @Rights VALUES(131204)--ReportsContributionCollection
INSERT INTO @Rights VALUES(131205)--ReportsProductSales
INSERT INTO @Rights VALUES(131206)--ReportsContributionDistribution
INSERT INTO @Rights VALUES(131207)--ReportsUserActivity
INSERT INTO @Rights VALUES(131208)--ReportsEnrolmentPerformanceIndicators
INSERT INTO @Rights VALUES(131209)--ReportsStatusOfRegister
INSERT INTO @Rights VALUES(131210)--ReportsInsureeWithoutPhotos
INSERT INTO @Rights VALUES(131211)--ReportsPaymentCategoryOverview
INSERT INTO @Rights VALUES(131212)--ReportsMatchingFunds
INSERT INTO @Rights VALUES(131213)--ReportsClaimOverviewReport 
INSERT INTO @Rights VALUES(131214)--ReportsPercentageReferrals
INSERT INTO @Rights VALUES(131215)--ReportsFamiliesInsureesOverview
INSERT INTO @Rights VALUES(131216)--ReportsPendingInsurees
INSERT INTO @Rights VALUES(131217)--ReportsRenewals
INSERT INTO @Rights VALUES(131216)--ReportsCapitationPayment
INSERT INTO @Rights VALUES(131219)--ReportRsejectedPhoto
INSERT INTO @Rights VALUES(131220)--ReportsContributionPayment
INSERT INTO @Rights VALUES(131221)--ReportsControlNumberAssignment
INSERT INTO @Rights VALUES(131222)--ReportsOverviewOfCommissions
INSERT INTO @Rights VALUES(131223)--ReportsClaimHistoryReport
INSERT INTO @Rights VALUES(131401)--AddFund
--DECLARE @RoleID INT
--SELECT @RoleID = RoleID from tblRole WHERE Rolename ='Accountant'
--DELETE FROM tblRoleRight WHERE RoleID = @RoleID AND RightID NOT IN (SELECT RightID FROM @Rights)
--INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom) 
--SELECT @RoleID,R.RightID,GETDATE() FROM @Rights R 
--LEFT JOIN tblRoleRight RR ON RR.RoleID =@RoleID AND RR.RightID = R.RightID AND RR.ValidityTo IS NULL 
--WHERE RR.RoleRightID IS NULL
--					