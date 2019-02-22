DROP TYPE [dbo].[xPayementStatus]
GO

CREATE TYPE [dbo].[xPayementStatus] AS TABLE(
	[StatusID] [int] NULL,
	[PaymenyStatusName] [nvarchar](40) NULL
)
GO

DROP FUNCTION [dbo].[udfAPIisValidMaritalStatus]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
	
CREATE FUNCTION [dbo].[udfAPIisValidMaritalStatus](
	@MaritalStatusCode NVARCHAR(1)
)

RETURNS BIT
AS
BEGIN
		DECLARE @tblMaritalStatus TABLE(MaritalStatusCode NVARCHAR(1))
		DECLARE @isValid BIT
		INSERT INTO @tblMaritalStatus(MaritalStatusCode) 
		VALUES ('N'),('W'),('S'),('D'),('M'),(NULL)

		IF EXISTS(SELECT 1 FROM @tblMaritalStatus WHERE MaritalStatusCode = @MaritalStatusCode)
			SET @isValid = 1
		ELSE 
			SET @isValid = 0

      RETURN(@isValid)
END
GO

IF  COL_LENGTH('tblIMISDefaults','APIKey') IS NOT NULL
ALTER TABLE tblIMISDefaults DROP APIKey
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblPaymentDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblPaymentDetails](
	[PaymentDetailsID] [bigint] IDENTITY(1,1) NOT NULL,
	[PaymentID] [bigint] NOT NULL,
	[ProductCode] [nvarchar](8) NULL,
	[InsuranceNumber] [nvarchar](12) NULL,
	[PolicyStage] [nvarchar](1) NULL,
	[Amount] [decimal](18, 2) NULL,
	[LegacyID] [bigint] NULL,
	[ValidityFrom] [datetime] NULL,
	[ValidityTo] [datetime] NULL,
	[RowID] [timestamp] NULL,
	[PremiumID] [int] NULL,
	[AuditedUserId] [int] NULL,
	[enrollmentDate] [date] NULL,
	[ExpectedAmount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tblPaymentDetails] PRIMARY KEY CLUSTERED 
(
	[PaymentDetailsID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblPayment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblPayment](
	[PaymentID] [bigint] IDENTITY(1,1) NOT NULL,
	[ExpectedAmount] [decimal](18, 2) NULL,
	[ReceivedAmount] [decimal](18, 2) NULL,
	[OfficerCode] [nvarchar](50) NULL,
	[PhoneNumber] [nvarchar](12) NULL,
	[RequestDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[PaymentStatus] [int] NULL,
	[LegacyID] [bigint] NULL,
	[ValidityFrom] [datetime] NULL,
	[ValidityTo] [datetime] NULL,
	[RowID] [timestamp] NOT NULL,
	[AuditedUSerID] [int] NULL,
	[TransactionNo] [nvarchar](50) NULL,
	[PaymentOrigin] [nvarchar](50) NULL,
	[MatchedDate] [datetime] NULL,
	[ReceiptNo] [nvarchar](100) NULL,
	[PaymentDate] [datetime] NULL,
	[RejectedReason] [nvarchar](255) NULL,
	[DateLastSMS] [datetime] NULL,
 CONSTRAINT [PK_tblPayment] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblControlNumber]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tblControlNumber](
	[ControlNumberID] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestedDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[RequestOrigin] [nvarchar](50) NULL,
	[ResponseOrigin] [nvarchar](50) NULL,
	[Status] [int] NULL,
	[LegacyID] [bigint] NULL,
	[ValidityFrom] [datetime] NULL,
	[ValidityTo] [datetime] NULL,
	[AuditedUserID] [int] NULL,
	[PaymentID] [bigint] NULL,
	[ControlNumber] [nvarchar](50) NULL,
	[IssuedDate] [datetime] NULL,
	[Comment] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblControlNumber] PRIMARY KEY CLUSTERED 
(
	[ControlNumberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspConsumeEnrollments]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspConsumeEnrollments]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspPolicyValueProxyFamily]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspPolicyValueProxyFamily]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspRequestGetControlNumber]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspRequestGetControlNumber]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspSentRequestGetControlNumber]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspSentRequestGetControlNumber]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAcknowledgeControlNumberRequest]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAcknowledgeControlNumberRequest]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAPIEnterPolicy]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAPIEnterPolicy]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAPIEnterContribution]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAPIEnterContribution]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAPIEditFamily]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAPIEditFamily]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAddInsureePolicyOffline]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAddInsureePolicyOffline]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAPIEnterFamily]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAPIEnterFamily]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAPIEnterMemberFamily]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAPIEnterMemberFamily]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAPIEditMemberFamily]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAPIEditMemberFamily]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAPIRenewPolicy]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAPIRenewPolicy]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspInsertPaymentIntent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspInsertPaymentIntent]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAddInsureePolicy]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAddInsureePolicy]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspReceiveControlNumber]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspReceiveControlNumber]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspProcessSingleClaimStep2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspProcessSingleClaimStep2]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspProcessSingleClaimStep1]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspProcessSingleClaimStep1]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspReceivePayment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspReceivePayment]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspMatchPayment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspMatchPayment]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAPIDeleteMemberFamily]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAPIDeleteMemberFamily]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[uspAPIGetCoverage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[uspAPIGetCoverage]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[uspAPIGetCoverage]
(
	
	@InsureeNumber NVARCHAR(12),
	@MinDateService DATE=NULL  OUTPUT,
	@MinDateItem DATE=NULL OUTPUT,
	@ServiceLeft INT=0 OUTPUT,
	@ItemLeft INT =0 OUTPUT,
	@isItemOK BIT =0 OUTPUT,
	@isServiceOK BIT=0 OUTPUT
)

AS

BEGIN



	/*
	RESPONSE CODE
		1-Wrong format or missing insurance number of head
		2-Insurance number of head not found
		
	*/


	/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	--1- Wrong format or missing insurance number of head
	IF LEN(ISNULL(@InsureeNumber,'')) = 0
		RETURN 3

	--2 - Insurance number of member not found
	IF NOT EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID = @InsureeNumber AND ValidityTo IS NULL)
		RETURN 4



	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/
	DECLARE @LocationId int =  0
	IF NOT OBJECT_ID('tempdb..#tempBase') IS NULL DROP TABLE #tempBase

		SELECT PL.PolicyValue,PL.EffectiveDate, PR.ProdID,PL.PolicyID,I.CHFID,P.PhotoFolder + case when RIGHT(P.PhotoFolder,1) = '\' then '' else '\' end + P.PhotoFileName PhotoPath,I.LastName, I.OtherNames,
		CONVERT(VARCHAR,DOB,103) DOB, CASE WHEN I.Gender = 'M' THEN 'Male' ELSE 'Female' END Gender,PR.ProductCode,PR.ProductName,
		CONVERT(VARCHAR(12),IP.ExpiryDate,103) ExpiryDate, 
		CASE WHEN IP.EffectiveDate IS NULL OR CAST(GETDATE() AS DATE) < IP.EffectiveDate  THEN 'I' WHEN CAST(GETDATE() AS DATE) NOT BETWEEN IP.EffectiveDate AND IP.ExpiryDate THEN 'E' ELSE 
		CASE PL.PolicyStatus WHEN 1 THEN 'I' WHEN 2 THEN 'A' WHEN 4 THEN 'S' ELSE 'E' END
		END  AS [Status]
		INTO #tempBase
		FROM tblInsuree I LEFT OUTER JOIN tblPhotos P ON I.PhotoID = P.PhotoID
		INNER JOIN tblFamilies F ON I.FamilyId = F.FamilyId 
		INNER JOIN tblVillages V ON V.VillageId = F.LocationId
		INNER JOIN tblWards W ON W.WardId = V.WardId
		INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
		LEFT OUTER JOIN tblPolicy PL ON I.FamilyID = PL.FamilyID
		LEFT OUTER JOIN tblProduct PR ON PL.ProdID = PR.ProdID
		LEFT OUTER JOIN tblInsureePolicy IP ON IP.InsureeId = I.InsureeId AND IP.PolicyId = PL.PolicyID
		WHERE I.ValidityTo IS NULL AND PL.ValidityTo IS NULL AND P.ValidityTo IS NULL AND PR.ValidityTo IS NULL AND IP.ValidityTo IS NULL AND F.ValidityTo IS NULL
		AND (I.CHFID = @InsureeNumber OR @InsureeNumber = '')
		AND (D.DistrictID = @LocationId or @LocationId= 0)


	DECLARE @Members INT = (SELECT COUNT(1) FROM tblInsuree WHERE FamilyID = (SELECT TOP 1 FamilyId FROM tblInsuree WHERE CHFID = @InsureeNumber AND ValidityTo IS NULL) AND ValidityTo IS NULL); 		
	DECLARE @InsureeId INT = (SELECT InsureeId FROM tblInsuree WHERE CHFID = @InsureeNumber AND ValidityTo IS NULL)
	DECLARE @FamilyId INT = (SELECT FamilyId FROM tblInsuree WHERE ValidityTO IS NULL AND CHFID = @InsureeNumber);

		
	IF NOT OBJECT_ID('tempdb..#tempDedRem')IS NULL DROP TABLE #tempDedRem
	CREATE TABLE #tempDedRem (PolicyId INT,ProdID INT,DedInsuree DECIMAL(18,2),DedOPInsuree DECIMAL(18,2),DedIPInsuree DECIMAL(18,2),MaxInsuree DECIMAL(18,2),MaxOPInsuree DECIMAL(18,2),MaxIPInsuree DECIMAL(18,2),DedTreatment DECIMAL(18,2),DedOPTreatment DECIMAL(18,2),DedIPTreatment DECIMAL(18,2),MaxTreatment DECIMAL(18,2),MaxOPTreatment DECIMAL(18,2),MaxIPTreatment DECIMAL(18,2),DedPolicy DECIMAL(18,2),DedOPPolicy DECIMAL(18,2),DedIPPolicy DECIMAL(18,2),MaxPolicy DECIMAL(18,2),MaxOPPolicy DECIMAL(18,2),MaxIPPolicy DECIMAL(18,2))

	INSERT INTO #tempDedRem(PolicyId, ProdID ,DedInsuree ,DedOPInsuree ,DedIPInsuree ,MaxInsuree ,MaxOPInsuree ,MaxIPInsuree ,DedTreatment ,DedOPTreatment ,DedIPTreatment ,MaxTreatment ,MaxOPTreatment ,MaxIPTreatment ,DedPolicy ,DedOPPolicy ,DedIPPolicy ,MaxPolicy ,MaxOPPolicy ,MaxIPPolicy)
					SELECT #tempBase.PolicyId, #tempBase.ProdID,
					DedInsuree ,DedOPInsuree ,DedIPInsuree ,
					MaxInsuree,MaxOPInsuree,MaxIPInsuree ,
					DedTreatment ,DedOPTreatment ,DedIPTreatment,
					MaxTreatment ,MaxOPTreatment ,MaxIPTreatment,
					DedPolicy ,DedOPPolicy ,DedIPPolicy , 
					CASE WHEN ISNULL(NULLIF(SIGN(((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMember, 0))),-1),0) * ((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMember, 0)) + MaxPolicy > MaxCeilingPolicy THEN MaxCeilingPolicy ELSE ISNULL(NULLIF(SIGN(((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMember, 0))),-1),0) * ((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMember, 0)) + MaxPolicy END MaxPolicy ,
					CASE WHEN ISNULL(NULLIF(SIGN(((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMemberOP, 0))),-1),0) * ((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMemberOP, 0)) + MaxOPPolicy > MaxCeilingPolicyOP THEN MaxCeilingPolicyOP ELSE ISNULL(NULLIF(SIGN(((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMemberOP, 0))),-1),0) * ((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMemberOP, 0)) + MaxOPPolicy END MaxOPPolicy ,
					CASE WHEN ISNULL(NULLIF(SIGN(((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMemberIP, 0))),-1),0) * ((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMemberIP, 0)) + MaxIPPolicy > MaxCeilingPolicyIP THEN MaxCeilingPolicyIP ELSE ISNULL(NULLIF(SIGN(((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMemberIP, 0))),-1),0) * ((CASE WHEN MemberCount - @Members < 0 THEN MemberCount ELSE @Members END - Threshold) * ISNULL(MaxPolicyExtraMemberIP, 0)) + MaxIPPolicy END MaxIPPolicy
					FROM tblProduct INNER JOIN #tempBase ON tblProduct.ProdID = #tempBase.ProdID
					WHERE ValidityTo IS NULL



IF EXISTS(SELECT 1 FROM tblClaimDedRem WHERE InsureeID = @InsureeId AND ValidityTo IS NULL)
BEGIN			
	UPDATE #tempDedRem
	SET 
	DedInsuree = (SELECT DedInsuree - ISNULL(SUM(DedG),0) 
			FROM tblProduct INNER JOIN tblPolicy ON tblProduct.ProdID = tblPolicy.ProdID
			LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID
			WHERE tblProduct.ValidityTo IS NULL 
			AND tblProduct.ProdID = #tempDedRem.ProdID
			AND tblClaimDedRem.PolicyID = #tempDedRem.PolicyId
			AND InsureeId = @InsureeId
			GROUP BY DedInsuree),
	DedOPInsuree = (select DedOPInsuree - ISNULL(SUM(DedOP),0) 
			FROM tblProduct INNER JOIN tblPolicy ON tblProduct.ProdID = tblPolicy.ProdID
			LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID
			WHERE tblProduct.ValidityTo IS NULL 
			AND tblProduct.ProdID = #tempDedRem.ProdID
			AND tblClaimDedRem.PolicyId = #tempDedRem.PolicyId
			AND InsureeId = @InsureeId
			GROUP BY DedOPInsuree),
	DedIPInsuree = (SELECT DedIPInsuree - ISNULL(SUM(DedIP),0)
			FROM tblProduct INNER JOIN tblPolicy ON tblProduct.ProdID = tblPolicy.ProdID
			LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID
			WHERE tblProduct.ValidityTo IS NULL 
			AND tblProduct.ProdID = #tempDedRem.ProdID
			AND tblClaimDedRem.PolicyId = #tempDedRem.PolicyId
			AND InsureeId = @InsureeId
			GROUP BY DedIPInsuree) ,
	MaxInsuree = (SELECT MaxInsuree - ISNULL(SUM(RemG),0)
			FROM tblProduct INNER JOIN tblPolicy ON tblProduct.ProdID = tblPolicy.ProdID
			LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID
			WHERE tblProduct.ValidityTo IS NULL 
			AND tblProduct.ProdID = #tempDedRem.ProdID
			AND tblClaimDedRem.PolicyId = #tempDedRem.PolicyId
			AND InsureeId = @InsureeId
			GROUP BY MaxInsuree ),
	MaxOPInsuree = (SELECT MaxOPInsuree - ISNULL(SUM(RemOP),0)
			FROM tblProduct INNER JOIN tblPolicy ON tblProduct.ProdID = tblPolicy.ProdID
			LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID
			WHERE tblProduct.ValidityTo IS NULL 
			AND tblProduct.ProdID = #tempDedRem.ProdID
			AND tblClaimDedRem.PolicyId = #tempDedRem.PolicyId
			AND InsureeId = @InsureeId
			GROUP BY MaxOPInsuree ) ,
	MaxIPInsuree = (SELECT MaxIPInsuree - ISNULL(SUM(RemIP),0)
			FROM tblProduct INNER JOIN tblPolicy ON tblProduct.ProdID = tblPolicy.ProdID
			LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID
			WHERE tblProduct.ValidityTo IS NULL 
			AND tblProduct.ProdID = #tempDedRem.ProdID
			AND tblClaimDedRem.PolicyId = #tempDedRem.PolicyId
			AND InsureeId = @InsureeId
			GROUP BY MaxIPInsuree),
	DedTreatment = (SELECT DedTreatment FROM tblProduct WHERE tblProduct.ValidityTo IS NULL AND tblProduct.ProdID = #tempDedRem.ProdID ) ,
	DedOPTreatment = (SELECT DedOPTreatment FROM tblProduct WHERE tblProduct.ValidityTo IS NULL AND tblProduct.ProdID = #tempDedRem.ProdID) ,
	DedIPTreatment = (SELECT DedIPTreatment FROM tblProduct WHERE tblProduct.ValidityTo IS NULL AND tblProduct.ProdID = #tempDedRem.ProdID) ,
	MaxTreatment = (SELECT MaxTreatment FROM tblProduct WHERE tblProduct.ValidityTo IS NULL AND tblProduct.ProdID = #tempDedRem.ProdID) ,
	MaxOPTreatment = (SELECT MaxOPTreatment FROM tblProduct WHERE tblProduct.ValidityTo IS NULL AND tblProduct.ProdID = #tempDedRem.ProdID) ,
	MaxIPTreatment = (SELECT MaxIPTreatment FROM tblProduct WHERE tblProduct.ValidityTo IS NULL AND tblProduct.ProdID = #tempDedRem.ProdID) 
	
END



IF EXISTS(SELECT 1
			FROM tblInsuree I INNER JOIN tblClaimDedRem DR ON I.InsureeId = DR.InsureeId
			WHERE I.ValidityTo IS NULL
			AND DR.ValidityTO IS NULL
			AND I.FamilyId = @FamilyId)			
BEGIN
	UPDATE #tempDedRem SET
	DedPolicy = (SELECT DedPolicy - ISNULL(SUM(DedG),0)
			FROM tblProduct INNER JOIN tblPolicy ON tblProduct.ProdID = tblPolicy.ProdID
			LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID
			WHERE tblProduct.ValidityTo IS NULL 
			AND tblProduct.ProdID = #tempDedRem.ProdID
			AND tblClaimDedRem.PolicyId = #tempDedRem.PolicyId
			AND FamilyId = @FamilyId
			GROUP BY DedPolicy),
	DedOPPolicy = (SELECT DedOPPolicy - ISNULL(SUM(DedOP),0)
			FROM tblProduct INNER JOIN tblPolicy ON tblProduct.ProdID = tblPolicy.ProdID
			LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID
			WHERE tblProduct.ValidityTo IS NULL 
			AND tblProduct.ProdID = #tempDedRem.ProdID
			AND tblClaimDedRem.PolicyId = #tempDedRem.PolicyId
			AND FamilyId = @FamilyId
			GROUP BY DedOPPolicy),
	DedIPPolicy = (SELECT DedIPPolicy - ISNULL(SUM(DedIP),0)
			FROM tblProduct INNER JOIN tblPolicy ON tblProduct.ProdID = tblPolicy.ProdID
			LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID
			WHERE tblProduct.ValidityTo IS NULL 
			AND tblProduct.ProdID = #tempDedRem.ProdID
			AND tblClaimDedRem.PolicyId = #tempDedRem.PolicyId
			AND FamilyId = @FamilyId
			GROUP BY DedIPPolicy)


	UPDATE t SET MaxPolicy = MaxPolicyLeft, MaxOPPolicy = MaxOPLeft, MaxIPPolicy = MaxIPLeft
	FROM #tempDedRem t LEFT OUTER JOIN
	(SELECT t.PolicyId, t.ProdId, t.MaxPolicy - ISNULL(SUM(RemG),0)MaxPolicyLeft
	FROM #tempDedRem t INNER JOIN tblPolicy ON t.ProdID = tblPolicy.ProdID --AND tblPolicy.PolicyStatus = 2 
	LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID AND tblClaimDedRem.PolicyId = t.PolicyId
	WHERE FamilyId = @FamilyId
	
	--AND Prod.ValidityTo IS NULL AND Prod.ProdID = t.ProdID
	GROUP BY t.ProdId, t.MaxPolicy, t.PolicyId)MP ON t.ProdID = MP.ProdID AND t.PolicyId = MP.PolicyId
	LEFT OUTER JOIN
	--UPDATE t SET MaxOPPolicy = MaxOPLeft
	--FROM #tempDedRem t LEFT OUTER JOIN
	(SELECT t.PolicyId, t.ProdId, MaxOPPolicy - ISNULL(SUM(RemOP),0) MaxOPLeft
	FROM #tempDedRem t INNER JOIN tblPolicy ON t.ProdID = tblPolicy.ProdID  --AND tblPolicy.PolicyStatus = 2
	LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID AND tblClaimDedRem.PolicyId = t.PolicyId
	WHERE FamilyId = @FamilyId
	
	--WHERE tblProduct.ValidityTo IS NULL AND tblProduct.ProdID = #tempDedRem.ProdID
	GROUP BY t.ProdId, MaxOPPolicy, t.PolicyId)MOP ON t.ProdId = MOP.ProdID AND t.PolicyId = MOP.PolicyId
	LEFT OUTER JOIN
	(SELECT t.PolicyId, t.ProdId, MaxIPPolicy - ISNULL(SUM(RemIP),0) MaxIPLeft
	FROM #tempDedRem t INNER JOIN tblPolicy ON t.ProdID = tblPolicy.ProdID  --AND tblPolicy.PolicyStatus = 2
	LEFT OUTER JOIN tblClaimDedRem ON tblPolicy.PolicyID = tblClaimDedRem.PolicyID AND tblClaimDedRem.PolicyId = t.PolicyId
	WHERE FamilyId = @FamilyId
	
	--WHERE tblProduct.ValidityTo IS NULL AND tblProduct.ProdID = #tempDedRem.ProdID
	GROUP BY t.ProdId, MaxIPPolicy, t.PolicyId)MIP ON t.ProdId = MIP.ProdID AND t.PolicyId = MIP.PolicyId	
END
 
 BEGIN


	-- @InsureeId  = (SELECT InsureeId FROM tblInsuree WHERE CHFID = @InsureeNumber AND ValidityTo IS NULL)
	DECLARE @Age INT = (SELECT DATEDIFF(YEAR,DOB,GETDATE()) FROM tblInsuree WHERE InsureeID = @InsureeId)
	
	DECLARE @ServiceCode NVARCHAR(6) = N''
	DECLARE @ItemCode NVARCHAR(6) = N''

	SET NOCOUNT ON

	--Service Information
	
	IF LEN(@ServiceCode) > 0
	BEGIN
		DECLARE @ServiceId INT = (SELECT ServiceId FROM tblServices WHERE ServCode = @ServiceCode AND ValidityTo IS NULL)
		DECLARE @ServiceCategory CHAR(1) = (SELECT ServCategory FROM tblServices WHERE ServiceID = @ServiceId)
		
		DECLARE @tblService TABLE(EffectiveDate DATE,ProdId INT,MinDate DATE,ServiceLeft INT)
		
		INSERT INTO @tblService
		SELECT IP.EffectiveDate, PL.ProdID,
		DATEADD(MONTH,CASE WHEN @Age >= 18 THEN  PS.WaitingPeriodAdult ELSE PS.WaitingPeriodChild END,IP.EffectiveDate) MinDate,
		(CASE WHEN @Age >= 18 THEN NULLIF(PS.LimitNoAdult,0) ELSE NULLIF(PS.LimitNoChild,0) END) - COUNT(CS.ServiceID) ServicesLeft
		FROM tblInsureePolicy IP INNER JOIN tblPolicy PL ON IP.PolicyId = PL.PolicyID
		INNER JOIN tblProductServices PS ON PL.ProdID = PS.ProdID
		LEFT OUTER JOIN tblClaim C ON IP.InsureeId = C.InsureeID
		LEFT JOIN tblClaimServices CS ON C.ClaimID = CS.ClaimID
		WHERE IP.ValidityTo IS NULL AND PL.ValidityTo IS NULL AND PS.ValidityTo IS NULL AND C.ValidityTo IS NULL AND CS.ValidityTo IS NULL
		AND IP.InsureeId = @InsureeId
		AND PS.ServiceID = @ServiceId
		AND (C.ClaimStatus > 2 OR C.ClaimStatus IS NULL)
		AND (CS.ClaimServiceStatus = 1 OR CS.ClaimServiceStatus IS NULL)
		AND PL.PolicyStatus = 2
		GROUP BY IP.EffectiveDate, PL.ProdID,PS.WaitingPeriodAdult,PS.WaitingPeriodChild,PS.LimitNoAdult,PS.LimitNoChild


		IF EXISTS(SELECT 1 FROM @tblService WHERE MinDate <= GETDATE())
			SET @MinDateService = (SELECT MIN(MinDate) FROM @tblService WHERE MinDate <= GETDATE())
		ELSE
			SET @MinDateService = (SELECT MIN(MinDate) FROM @tblService)
			
		IF EXISTS(SELECT 1 FROM @tblService WHERE MinDate <= GETDATE() AND ServiceLeft IS NULL)
			SET @ServiceLeft = NULL
		ELSE
			SET @ServiceLeft = (SELECT MAX(ServiceLeft) FROM @tblService WHERE ISNULL(MinDate, GETDATE()) <= GETDATE())
	END
	--

	--Item Information
	
	
	IF LEN(@ItemCode) > 0
	BEGIN
		DECLARE @ItemId INT = (SELECT ItemId FROM tblItems WHERE ItemCode = @ItemCode AND ValidityTo IS NULL)
		
		DECLARE @tblItem TABLE(EffectiveDate DATE,ProdId INT,MinDate DATE,ItemsLeft INT)

		INSERT INTO @tblItem
		SELECT IP.EffectiveDate, PL.ProdID,
		DATEADD(MONTH,CASE WHEN @Age >= 18 THEN  PItem.WaitingPeriodAdult ELSE PItem.WaitingPeriodChild END,IP.EffectiveDate) MinDate,
		(CASE WHEN @Age >= 18 THEN NULLIF(PItem.LimitNoAdult,0) ELSE NULLIF(PItem.LimitNoChild,0) END) - COUNT(CI.ItemID) ItemsLeft
		FROM tblInsureePolicy IP INNER JOIN tblPolicy PL ON IP.PolicyId = PL.PolicyID
		INNER JOIN tblProductItems PItem ON PL.ProdID = PItem.ProdID
		LEFT OUTER JOIN tblClaim C ON IP.InsureeId = C.InsureeID
		LEFT OUTER JOIN tblClaimItems CI ON C.ClaimID = CI.ClaimID
		WHERE IP.ValidityTo IS NULL AND PL.ValidityTo IS NULL AND PItem.ValidityTo IS NULL AND C.ValidityTo IS NULL AND CI.ValidityTo IS NULL
		AND IP.InsureeId = @InsureeId
		AND PItem.ItemID = @ItemId
		AND (C.ClaimStatus > 2  OR C.ClaimStatus IS NULL)
		AND (CI.ClaimItemStatus = 1 OR CI.ClaimItemStatus IS NULL)
		AND PL.PolicyStatus = 2
		GROUP BY IP.EffectiveDate, PL.ProdID,PItem.WaitingPeriodAdult,PItem.WaitingPeriodChild,PItem.LimitNoAdult,PItem.LimitNoChild


		IF EXISTS(SELECT 1 FROM @tblItem WHERE MinDate <= GETDATE())
			SET @MinDateItem = (SELECT MIN(MinDate) FROM @tblItem WHERE MinDate <= GETDATE())
		ELSE
			SET @MinDateItem = (SELECT MIN(MinDate) FROM @tblItem)
			
		IF EXISTS(SELECT 1 FROM @tblItem WHERE MinDate <= GETDATE() AND ItemsLeft IS NULL)
			SET @ItemLeft = NULL
		ELSE
			SET @ItemLeft = (SELECT MAX(ItemsLeft) FROM @tblItem WHERE ISNULL(MinDate, GETDATE()) <= GETDATE())
	END
	
	--

	DECLARE @Result TABLE(ProdId INT, TotalAdmissionsLeft INT, TotalVisitsLeft INT, TotalConsultationsLeft INT, TotalSurgeriesLeft INT, TotalDelivieriesLeft INT, TotalAntenatalLeft INT,
					ConsultationAmountLeft DECIMAL(18,2),SurgeryAmountLeft DECIMAL(18,2),DeliveryAmountLeft DECIMAL(18,2),HospitalizationAmountLeft DECIMAL(18,2), AntenatalAmountLeft DECIMAL(18,2))

	INSERT INTO @Result
	SELECT  Prod.ProdId,
	Prod.MaxNoHospitalizaion - ISNULL(TotalAdmissions,0)TotalAdmissionsLeft,
	Prod.MaxNoVisits - ISNULL(TotalVisits,0)TotalVisitsLeft,
	Prod.MaxNoConsultation - ISNULL(TotalConsultations,0)TotalConsultationsLeft,
	Prod.MaxNoSurgery - ISNULL(TotalSurgeries,0)TotalSurgeriesLeft,
	Prod.MaxNoDelivery - ISNULL(TotalDelivieries,0)TotalDelivieriesLeft,
	Prod.MaxNoAntenatal - ISNULL(TotalAntenatal, 0)TotalAntenatalLeft,
	--Changes by Rogers Start
	Prod.MaxAmountConsultation ConsultationAmountLeft, --- SUM(ISNULL(Rem.RemConsult,0)) ConsultationAmountLeft,
	Prod.MaxAmountSurgery SurgeryAmountLeft ,--- SUM(ISNULL(Rem.RemSurgery,0)) SurgeryAmountLeft ,
	Prod.MaxAmountDelivery DeliveryAmountLeft,--- SUM(ISNULL(Rem.RemDelivery,0)) DeliveryAmountLeft,By Rogers (Amount must Remain Constant)
	Prod.MaxAmountHospitalization HospitalizationAmountLeft, -- SUM(ISNULL(Rem.RemHospitalization,0)) HospitalizationAmountLeft, By Rogers (Amount must Remain Constant)
	Prod.MaxAmountAntenatal AntenatalAmountLeft -- - SUM(ISNULL(Rem.RemAntenatal, 0)) AntenatalAmountLeft By Rogers (Amount must Remain Constant)
	--Changes by Rogers End
	FROM tblInsureePolicy IP INNER JOIN tblPolicy PL ON IP.PolicyId = PL.PolicyID
	INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
	LEFT OUTER JOIN tblClaimDedRem Rem ON PL.PolicyID = Rem.PolicyID AND Rem.InsureeID = IP.InsureeId

	LEFT OUTER JOIN
		(SELECT COUNT(C.ClaimID)TotalAdmissions,CS.ProdID
		FROM tblClaim C INNER JOIN tblClaimServices CS ON C.ClaimID = CS.ClaimID
		INNER JOIN tblInsureePolicy IP ON C.InsureeID = IP.InsureeID
		WHERE C.ValidityTo IS NULL AND CS.ValidityTo IS NULL AND IP.ValidityTo IS NULL
		AND C.ClaimStatus > 2
		AND CS.RejectionReason = 0
		AND C.InsureeID = @InsureeId
		AND C.ClaimCategory = 'H'
		AND (ISNULL(C.DateTo,C.DateFrom) BETWEEN IP.EffectiveDate AND IP.ExpiryDate)
		GROUP BY CS.ProdID)TotalAdmissions ON TotalAdmissions.ProdID = Prod.ProdId
		
		LEFT OUTER JOIN
		(SELECT COUNT(C.ClaimID)TotalVisits,CS.ProdID
		FROM tblClaim C INNER JOIN tblClaimServices CS ON C.ClaimID = CS.ClaimID
		WHERE C.ValidityTo IS NULL AND CS.ValidityTo IS NULL 
		AND C.ClaimStatus > 2
		AND CS.RejectionReason = 0
		AND C.InsureeID = @InsureeId
		AND C.ClaimCategory = 'V'
		GROUP BY CS.ProdID)TotalVisits ON Prod.ProdID = TotalVisits.ProdID
		LEFT OUTER JOIN
		
		(SELECT COUNT(C.ClaimID) TotalConsultations,CS.ProdID
		FROM tblClaim C 
		INNER JOIN (SELECT ClaimId, ProdId FROM tblClaimServices WHERE ValidityTo IS NULL AND RejectionReason = 0 GROUP BY ClaimId, ProdID) CS ON C.ClaimID = CS.ClaimID
		WHERE C.ValidityTo IS NULL 
		AND C.ClaimStatus > 2
		AND C.InsureeID = @InsureeId
		AND C.ClaimCategory = 'C'
		GROUP BY CS.ProdID) TotalConsultations ON Prod.ProdID = TotalConsultations.ProdID
		LEFT OUTER JOIN
		
		(SELECT COUNT(C.ClaimID) TotalSurgeries,CS.ProdID
		FROM tblClaim C 
		INNER JOIN (SELECT ClaimId, ProdId FROM tblClaimServices WHERE ValidityTo IS NULL AND RejectionReason = 0 GROUP BY ClaimId, ProdID) CS ON C.ClaimID = CS.ClaimID
		WHERE C.ValidityTo IS NULL 
		AND C.ClaimStatus > 2
		AND C.InsureeID = @InsureeId
		AND C.ClaimCategory = 'S'
		GROUP BY CS.ProdID)TotalSurgeries ON Prod.ProdID = TotalSurgeries.ProdID
		LEFT OUTER JOIN
		
		(SELECT COUNT(C.ClaimID) TotalDelivieries,CS.ProdID
		FROM tblClaim C 
		INNER JOIN (SELECT ClaimId, ProdId FROM tblClaimServices WHERE ValidityTo IS NULL AND RejectionReason = 0 GROUP BY ClaimId, ProdID) CS ON C.ClaimID = CS.ClaimID
		WHERE C.ValidityTo IS NULL 
		AND C.ClaimStatus > 2
		AND C.InsureeID = @InsureeId
		AND C.ClaimCategory = 'D'
		GROUP BY CS.ProdID)TotalDelivieries ON Prod.ProdID = TotalDelivieries.ProdID
		LEFT OUTER JOIN
		
		(SELECT COUNT(C.ClaimID) TotalAntenatal,CS.ProdID
		FROM tblClaim C 
		INNER JOIN (SELECT ClaimId, ProdId FROM tblClaimServices WHERE ValidityTo IS NULL AND RejectionReason = 0 GROUP BY ClaimId, ProdID) CS ON C.ClaimID = CS.ClaimID
		WHERE C.ValidityTo IS NULL 
		AND C.ClaimStatus > 2
		AND C.InsureeID = @InsureeId
		AND C.ClaimCategory = 'A'
		GROUP BY CS.ProdID)TotalAntenatal ON Prod.ProdID = TotalAntenatal.ProdID
		
	WHERE IP.ValidityTo IS NULL AND PL.ValidityTo IS NULL AND Prod.ValidityTo IS NULL AND Rem.ValidityTo IS NULL
	AND IP.InsureeId = @InsureeId

	GROUP BY Prod.ProdID,Prod.MaxNoHospitalizaion,TotalAdmissions, Prod.MaxNoVisits, TotalVisits, Prod.MaxNoConsultation, 
	TotalConsultations, Prod.MaxNoSurgery, TotalSurgeries, Prod.MaxNoDelivery, Prod.MaxNoAntenatal, TotalDelivieries, TotalAntenatal,Prod.MaxAmountConsultation,
	Prod.MaxAmountSurgery, Prod.MaxAmountDelivery, Prod.MaxAmountHospitalization, Prod.MaxAmountAntenatal
	
	Update @Result set TotalAdmissionsLeft=0 where TotalAdmissionsLeft<0;
	Update @Result set TotalVisitsLeft=0 where TotalVisitsLeft<0;
	Update @Result set TotalConsultationsLeft=0 where TotalConsultationsLeft<0;
	Update @Result set TotalSurgeriesLeft=0 where TotalSurgeriesLeft<0;
	Update @Result set TotalDelivieriesLeft=0 where TotalDelivieriesLeft<0;
	Update @Result set TotalAntenatalLeft=0 where TotalAntenatalLeft<0;

	DECLARE @MaxNoSurgery INT,
			@MaxNoConsultation INT,
			@MaxNoDeliveries INT,
			@TotalAmountSurgery DECIMAL(18,2),
			@TotalAmountConsultant DECIMAL(18,2),
			@TotalAmountDelivery DECIMAL(18,2)
			
	SELECT TOP 1 @MaxNoSurgery = TotalSurgeriesLeft, @MaxNoConsultation = TotalConsultationsLeft, @MaxNoDeliveries = TotalDelivieriesLeft,
	@TotalAmountSurgery = SurgeryAmountLeft, @TotalAmountConsultant = ConsultationAmountLeft, @TotalAmountDelivery = DeliveryAmountLeft 
	FROM @Result 


	 

	IF @ServiceCategory = N'S'
		BEGIN
			IF @MaxNoSurgery = 0 OR @ServiceLeft = 0 OR @MinDateService > GETDATE() OR @TotalAmountSurgery <= 0
				SET @isServiceOK = 0
			ELSE
				SET @isServiceOK = 1
		END
	ELSE IF @ServiceCategory = N'C'
		BEGIN
			IF @MaxNoConsultation = 0 OR @ServiceLeft = 0 OR @MinDateService > GETDATE() OR @TotalAmountConsultant <= 0
				SET @isServiceOK = 0
			ELSE
				SET @isServiceOK = 1
		END
	ELSE IF @ServiceCategory = N'D'
		BEGIN
			IF @MaxNoDeliveries = 0 OR @ServiceLeft = 0 OR @MinDateService > GETDATE() OR @TotalAmountDelivery  <= 0
				SET @isServiceOK = 0
			ELSE
				SET @isServiceOK = 1
		END
	ELSE IF @ServiceCategory = N'O'
		BEGIN
			IF  @ServiceLeft = 0 OR @MinDateService > GETDATE() 
				SET @isServiceOK = 0
			ELSE
				SET @isServiceOK = 1
		END
	ELSE 
		BEGIN
			IF  @ServiceLeft = 0 OR @MinDateService > GETDATE() 
				SET @isServiceOK = 0
			ELSE
				SET @isServiceOK = 1
		END

     

	IF @ItemLeft = 0 OR @MinDateItem > GETDATE() 
		SET @isItemOK = 0
	ELSE
		SET @isItemOK = 1


END


	ALTER TABLE #tempBase ADD DedType FLOAT NULL
	ALTER TABLE #tempBase ADD Ded1 DECIMAL(18,2) NULL
	ALTER TABLE #tempBase ADD Ded2 DECIMAL(18,2) NULL
	ALTER TABLE #tempBase ADD Ceiling1 DECIMAL(18,2) NULL
	ALTER TABLE #tempBase ADD Ceiling2 DECIMAL(18,2) NULL
			
	DECLARE @ProdID INT
	DECLARE @DedType FLOAT = NULL
	DECLARE @Ded1 DECIMAL(18,2) = NULL
	DECLARE @Ded2 DECIMAL(18,2) = NULL
	DECLARE @Ceiling1 DECIMAL(18,2) = NULL
	DECLARE @Ceiling2 DECIMAL(18,2) = NULL
	DECLARE @PolicyID INT


	DECLARE @InsuranceNumber NVARCHAR(12)
	DECLARE @OtherNames NVARCHAR(100)
	DECLARE @LastName NVARCHAR(100)
	DECLARE @BirthDate DATE
	DECLARE @Gender NVARCHAR(1)
	DECLARE @ProductCode NVARCHAR(8) = NULL
	DECLARE @ProductName NVARCHAR(50)
	DECLARE @PolicyValue DECIMAL
	DECLARE @EffectiveDate DATE=NULL
	DECLARE @ExpiryDate DATE=NULL
	DECLARE @PolicyStatus BIT =0
	DECLARE @DeductionType INT
	DECLARE @DedNonHospital DECIMAL(18,2)
	DECLARE @DedHospital DECIMAL(18,2)
	DECLARE @CeilingHospital DECIMAL(18,2)
	DECLARE @CeilingNonHospital DECIMAL(18,2)
	DECLARE @AdmissionLeft NVARCHAR
	DECLARE @PhotoPath NVARCHAR
	DECLARE @VisitLeft NVARCHAR(200)=NULL
	DECLARE @ConsultationLeft NVARCHAR(50)=NULL
	DECLARE @SurgeriesLeft NVARCHAR
	DECLARE @DeliveriesLeft NVARCHAR(50)=NULL
	DECLARE @Anc_CareLeft NVARCHAR(100)=NULL
	DECLARE @IdentificationNumber NVARCHAR(25)=NULL
	DECLARE @ConsultationAmount DECIMAL(18,2)
	DECLARE @SurgriesAmount DECIMAL(18,2)
	DECLARE @DeliveriesAmount DECIMAL(18,2)


	DECLARE Cur CURSOR FOR SELECT DISTINCT ProdId, PolicyId FROM #tempDedRem
	OPEN Cur
	FETCH NEXT FROM Cur INTO @ProdID, @PolicyId

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @Ded1 = NULL
		SET @Ded2 = NULL
		SET @Ceiling1 = NULL
		SET @Ceiling2 = NULL
		
		SELECT @Ded1 =  CASE WHEN NOT DedInsuree IS NULL THEN DedInsuree WHEN NOT DedTreatment IS NULL THEN DedTreatment WHEN NOT DedPolicy IS NULL THEN DedPolicy ELSE NULL END  FROM #tempDedRem WHERE ProdID = @ProdID AND PolicyId = @PolicyId
		IF NOT @Ded1 IS NULL SET @DedType = 1
		
		IF @Ded1 IS NULL
		BEGIN
			SELECT @Ded1 = CASE WHEN NOT DedIPInsuree IS NULL THEN DedIPInsuree WHEN NOT DedIPTreatment IS NULL THEN DedIPTreatment WHEN NOT DedIPPolicy IS NULL THEN DedIPPolicy ELSE NULL END FROM #tempDedRem WHERE ProdID = @ProdID AND PolicyId = @PolicyId
			SELECT @Ded2 = CASE WHEN NOT DedOPInsuree IS NULL THEN DedOPInsuree WHEN NOT DedOPTreatment IS NULL THEN DedOPTreatment WHEN NOT DedOPPolicy IS NULL THEN DedOPPolicy ELSE NULL END FROM #tempDedRem WHERE ProdID = @ProdID AND PolicyId = @PolicyId
			IF NOT @Ded1 IS NULL OR NOT @Ded2 IS NULL SET @DedType = 1.1
		END
		
		SELECT @Ceiling1 =  CASE WHEN NOT MaxInsuree IS NULL THEN MaxInsuree WHEN NOT MaxTreatment IS NULL THEN MaxTreatment WHEN NOT MaxPolicy IS NULL THEN MaxPolicy ELSE NULL END  FROM #tempDedRem WHERE ProdID = @ProdID AND PolicyId = @PolicyId
		IF NOT @Ceiling1 IS NULL SET @DedType = 1
		
		IF @Ceiling1 IS NULL
		BEGIN
			SELECT @Ceiling1 = CASE WHEN NOT MaxIPInsuree IS NULL THEN MaxIPInsuree WHEN NOT MaxIPTreatment IS NULL THEN MaxIPTreatment WHEN NOT MaxIPPolicy IS NULL THEN MaxIPPolicy ELSE NULL END FROM #tempDedRem WHERE ProdID = @ProdID AND PolicyId = @PolicyId
			SELECT @Ceiling2 = CASE WHEN NOT MaxOPInsuree IS NULL THEN MaxOPInsuree WHEN NOT MaxOPTreatment IS NULL THEN MaxOPTreatment WHEN NOT MaxOPPolicy IS NULL THEN MaxOPPolicy ELSE NULL END FROM #tempDedRem WHERE ProdID = @ProdID AND PolicyId = @PolicyId
			IF NOT @Ceiling1 IS NULL OR NOT @Ceiling2 IS NULL SET @DedType = 1.1
		END
		
			UPDATE #tempBase SET DedType = @DedType, Ded1 = @Ded1, Ded2 = CASE WHEN @DedType = 1 THEN @Ded1 ELSE @Ded2 END,Ceiling1 = @Ceiling1,Ceiling2 = CASE WHEN @DedType = 1 THEN @Ceiling1 ELSE @Ceiling2 END
		WHERE ProdID = @ProdID
		 AND PolicyId = @PolicyId
		
	FETCH NEXT FROM Cur INTO @ProdID, @PolicyId
	END

	CLOSE Cur
	DEALLOCATE Cur

	--DECLARE @LASTRESULT TABLE(PolicyValue DECIMAL(18,2) NULL,EffectiveDate DATE NULL, LastName NVARCHAR(100) NULL, OtherNames NVARCHAR(100) NULL,CHFID NVARCHAR(12), PhotoPath  NVARCHAR(100) NULL,  DOB DATE NULL ,Gender NVARCHAR(1) NULL,ProductCode NVARCHAR(8) NULL,ProductName NVARCHAR(50) NULL, ExpiryDate DATE NULL, [Status] NVARCHAR(1) NULL,DedType FLOAT NULL, Ded1 DECIMAL(18,2)NULL,  Ded2 DECIMAL(18,2)NULL, Ceiling1 DECIMAL(18,2)NULL, Ceiling2 DECIMAL(18,2)NULL)
  IF (SELECT COUNT(*) FROM #tempBase WHERE [Status] = 'A') > 0
 SELECT R.AntenatalAmountLeft,R.ConsultationAmountLeft,R.DeliveryAmountLeft, R.HospitalizationAmountLeft,R.SurgeryAmountLeft,R.TotalAdmissionsLeft,R.TotalAntenatalLeft, R.TotalConsultationsLeft,  r.TotalDelivieriesLeft, R.TotalSurgeriesLeft ,r.TotalVisitsLeft, PolicyValue, EffectiveDate, LastName, OtherNames,CHFID, PhotoPath,  DOB,Gender,ProductCode ,ProductName, ExpiryDate, [Status],DedType, Ded1,  Ded2, CASE WHEN Ceiling1 < 0 THEN 0 ELSE  Ceiling1 END Ceiling1 , CASE WHEN Ceiling2< 0 THEN 0 ELSE Ceiling2 END Ceiling2   from #tempBase T LEFT OUTER JOIN @Result R ON R.ProdId = T.ProdID WHERE [Status] = 'A';
		
	ELSE 
		IF (SELECT COUNT(1) FROM #tempBase WHERE (YEAR(GETDATE()) - YEAR(CONVERT(DATETIME,ExpiryDate,103))) <= 2) > 1
	  SELECT R.AntenatalAmountLeft,R.ConsultationAmountLeft,R.DeliveryAmountLeft, R.HospitalizationAmountLeft,R.SurgeryAmountLeft,R.TotalAdmissionsLeft,R.TotalAntenatalLeft, R.TotalConsultationsLeft,  r.TotalDelivieriesLeft, R.TotalSurgeriesLeft ,r.TotalVisitsLeft,  PolicyValue,EffectiveDate, LastName, OtherNames,CHFID, PhotoPath,  DOB,Gender,ProductCode,ProductName,ExpiryDate,[Status],DedType,Ded1,Ded2,CASE WHEN Ceiling1<0 THEN 0 ELSE Ceiling1 END Ceiling1,CASE WHEN Ceiling2<0 THEN 0 ELSE Ceiling2 END Ceiling2  from #tempBase T LEFT OUTER JOIN @Result R ON R.ProdId = T.ProdID WHERE (YEAR(GETDATE()) - YEAR(CONVERT(DATETIME,ExpiryDate,103))) <= 2;
		ELSE
	
			 SELECT R.AntenatalAmountLeft,R.ConsultationAmountLeft,R.DeliveryAmountLeft, R.HospitalizationAmountLeft,R.SurgeryAmountLeft,R.TotalAdmissionsLeft,R.TotalAntenatalLeft, R.TotalConsultationsLeft, r.TotalDelivieriesLeft, R.TotalSurgeriesLeft ,r.TotalVisitsLeft, PolicyValue,EffectiveDate, LastName, OtherNames, CHFID, PhotoPath,  DOB,Gender,ProductCode,ProductName,ExpiryDate,[Status],DedType,Ded1,Ded2,CASE WHEN Ceiling1<0 THEN 0 ELSE Ceiling1 END Ceiling1,CASE WHEN Ceiling2<0 THEN 0 ELSE Ceiling2 END Ceiling2  from #tempBase T LEFT OUTER JOIN @Result R  ON R.ProdId = T.ProdID
END
  



GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[uspAPIDeleteMemberFamily]
(
	@AuditUserID INT = -3,
	@InsuranceNumber NVARCHAR(12)
)

AS
BEGIN
	/*
	RESPONSE CODE
		1-Wrong format or missing insurance number  of member
		2-Insurance number of member not found
		3- Member is head of family
		0 - Success (0 OK), 
		-1 -Unknown  Error 
	*/


	/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	--1-Wrong format or missing insurance number of head
	IF LEN(ISNULL(@InsuranceNumber,'')) = 0
		RETURN 1

	--2-Insurance number of member not found
	IF NOT EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID = @InsuranceNumber AND ValidityTo IS NULL)
		RETURN 2

	--3- Member is head of family
	IF  EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID = @InsuranceNumber AND ValidityTo IS NULL AND IsHead = 1)
		RETURN 3

	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/

	BEGIN TRY
			BEGIN TRANSACTION DELETEMEMBERFAMILY
			
				DECLARE @InsureeId INT


				SELECT @InsureeID = InsureeID FROM tblInsuree WHERE CHFID = @InsuranceNumber AND ValidityTo IS NULL
				
				INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,[ValidityTo],legacyId,TypeOfId, HFID, CurrentAddress, CurrentVillage,GeoLocation ) 
				SELECT	[FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,getdate(),@insureeId ,TypeOfId, HFID, CurrentAddress, CurrentVillage, GeoLocation 
				FROM tblInsuree WHERE InsureeID = @InsureeID AND ValidityTo IS NULL
				UPDATE [tblInsuree] SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate(),[AuditUserID] = @AuditUserID 
				WHERE InsureeId = @InsureeID AND ValidityTo IS NULL

       

			COMMIT TRANSACTION DELETEMEMBERFAMILY
			RETURN 0
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION DELETEMEMBERFAMILY
			SELECT ERROR_MESSAGE()
			RETURN -1
		END CATCH


END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[uspMatchPayment]
	@PaymentID INT = NULL,
	@AuditUserId INT = NULL
AS
BEGIN

BEGIN TRY
---CHECK IF PAYMENTID EXISTS
	IF NOT @PaymentID IS NULL
	BEGIN
	     IF NOT EXISTS ( SELECT PaymentID FROM tblPayment WHERE PaymentID = @PaymentID AND ValidityTo IS NULL)
			RETURN 1
	END

	SET @AuditUserId =ISNULL(@AuditUserId, -1)

	DECLARE @InTopIsolation as bit 
	SET @InTopIsolation = -1 
	IF @@TRANCOUNT = 0 	
		SET @InTopIsolation =0
	ELSE
		SET @InTopIsolation =1
	IF @InTopIsolation = 0
	BEGIN
		BEGIN TRANSACTION MATCHPAYMENT
	END
	
	

	DECLARE @tblHeader TABLE(PaymentID BIGINT, officerCode nvarchar(12),PhoneNumber nvarchar(12),paymentDate DATE,ReceivedAmount DECIMAL(18,2),TotalPolicyValue DECIMAL(18,2), isValid BIT, TransactionNo NVARCHAR(50))
	DECLARE @tblDetail TABLE(PaymentDetailsID BIGINT, PaymentID BIGINT, InsuranceNumber nvarchar(12),productCode nvarchar(8),  enrollmentDate DATE,PolicyStage CHAR(1), MatchedDate DATE, PolicyValue DECIMAL(18,2),DistributedValue DECIMAL(18,2), policyID INT, RenewalpolicyID INT, PremiumID INT)
	DECLARE @tblResult TABLE(policyID INT, PremiumId INT)
	DECLARE @tblFeedback TABLE(fdMsg NVARCHAR(MAX), fdType NVARCHAR(1),paymentID INT,InsuranceNumber nvarchar(12),PhoneNumber nvarchar(12),productCode nvarchar(8), Balance DECIMAL(18,2), isActivated BIT, PaymentFound INT, PaymentMatched INT, APIKey NVARCHAR(100))
	DECLARE @tblPaidPolicies TABLE(PolicyID INT, Amount DECIMAL(18,2), PolicyValue DECIMAL(18,2))
	DECLARE @tblPeriod TABLE(startDate DATE, expiryDate DATE, HasCycle  BIT)
	DECLARE @paymentFound INT
	DECLARE @paymentMatched INT


	--GET ALL UNMATCHED RECEIVED PAYMENTS
	INSERT INTO @tblDetail(PaymentDetailsID, PaymentID, InsuranceNumber, ProductCode, enrollmentDate, policyID, PolicyStage, PolicyValue, PremiumID)
	SELECT PaymentDetailsID, PaymentID, InsuranceNumber, ProductCode, EnrollDate,  PolicyID, PolicyStage, PolicyValue, PremiumId FROM(
	SELECT ROW_NUMBER() OVER(PARTITION BY PR.ProductCode,I.CHFID ORDER BY PL.EnrollDate DESC) RN, PD.PaymentDetailsID, PY.PaymentID,PD.InsuranceNumber, PD.ProductCode,PL.EnrollDate,  PL.PolicyID, PD.PolicyStage, PL.PolicyValue, PRM.PremiumId FROM tblPaymentDetails PD 
	LEFT OUTER JOIN tblInsuree I ON I.CHFID = PD.InsuranceNumber
	LEFT OUTER JOIN tblFamilies F ON F.FamilyID = I.FamilyID
	LEFT OUTER JOIN tblProduct PR ON PR.ProductCode = PD.ProductCode
	LEFT OUTER JOIN (SELECT  PolicyID, EnrollDate, PolicyValue,FamilyID, ProdID,PolicyStatus FROM tblPolicy WHERE ProdID = ProdID AND FamilyID = FamilyID AND ValidityTo IS NULL AND PolicyStatus NOT IN (4,8)  ) PL ON PL.ProdID = PR.ProdID  AND PL.FamilyID = I.FamilyID
	LEFT OUTER JOIN ( SELECT MAX(PremiumId) PremiumId , PolicyID FROM  tblPremium WHERE ValidityTo IS NULL GROUP BY PolicyID ) PRM ON PRM.PolicyID = PL.PolicyID
	INNER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID
	WHERE PD.PremiumID IS NULL 
	AND PD.ValidityTo IS NULL
	AND I.ValidityTo IS NULL
	AND PR.ValidityTo IS NULL
	AND F.ValidityTo IS NULL
	AND PY.ValidityTo IS NULL
	AND PY.PaymentStatus = 4 --Received Payment
	AND PD.PaymentID = ISNULL(@PaymentID,PD.PaymentID)
	)XX --WHERE RN =1
	
	INSERT INTO @tblHeader(PaymentID, ReceivedAmount, PhoneNumber, TotalPolicyValue, TransactionNo, officerCode)
	SELECT P.PaymentID, P.ReceivedAmount, P.PhoneNumber, D.TotalPolicyValue, P.TransactionNo, P.OfficerCode FROM tblPayment P
	INNER JOIN (SELECT PaymentID, SUM(PolicyValue) TotalPolicyValue FROM @tblDetail GROUP BY PaymentID)  D ON P.PaymentID = D.PaymentID
	WHERE P.ValidityTo IS NULL AND P.PaymentStatus = 4

	IF EXISTS(SELECT COUNT(1) FROM @tblHeader PH )
		BEGIN
			SET @paymentFound= (SELECT COUNT(1)  FROM @tblHeader PH )
			INSERT INTO @tblFeedback(fdMsg, fdType )
			SELECT CONVERT(NVARCHAR(4), ISNULL(@paymentFound,0))  +' Unmatched Payment(s) found ', 'I' 
		END


	/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	
	--1. Insurance number is missing on tblPaymentDetails
	IF EXISTS(SELECT 1 FROM @tblDetail WHERE LEN(ISNULL(InsuranceNumber,'')) = 0)
		BEGIN
			INSERT INTO @tblFeedback(fdMsg, fdType, paymentID )
			SELECT CONVERT(NVARCHAR(4), COUNT(1) OVER(PARTITION BY InsuranceNumber)) +' Insurance number(s) missing in tblPaymentDetails ', 'E', PaymentID FROM @tblDetail WHERE LEN(ISNULL(InsuranceNumber,'')) = 0
		END

		--2. Product code is missing on tblPaymentDetails
		INSERT INTO @tblFeedback(fdMsg, fdType, paymentID )
		SELECT 'Family with Insurance Number ' + QUOTENAME(PD.InsuranceNumber) + ' is missing product code ', 'E', PD.PaymentID FROM @tblDetail PD WHERE LEN(ISNULL(productCode,'')) = 0

	--2. Insurance number is missing in tblinsuree
		INSERT INTO @tblFeedback(fdMsg, fdType, paymentID )
		SELECT 'Family with Insurance Number' + QUOTENAME(PD.InsuranceNumber) + ' does not exists', 'E', PD.PaymentID FROM @tblDetail PD 
		LEFT OUTER JOIN tblInsuree I ON I.CHFID = PD.InsuranceNumber
		WHERE I.ValidityTo  IS NULL
		AND I.CHFID IS NULL
		
	--1. Policy/Prevous Policy not found
		INSERT INTO @tblFeedback(fdMsg, fdType, paymentID )
		SELECT 'Family with Insurance Number ' + QUOTENAME(PD.InsuranceNumber) + ' does not have Policy or Previous Policy for the product '+QUOTENAME(PD.productCode), 'E', PD.PaymentID FROM @tblDetail PD 
		WHERE policyID IS NULL
		AND ISNULL(LEN(PD.productCode),'') > 0
		AND ISNULL(LEN(PD.InsuranceNumber),'') > 0

	 --3. Invalid Product
		INSERT INTO @tblFeedback(fdMsg, fdType, paymentID)
		SELECT  'Family with insurance number '+ QUOTENAME(PD.InsuranceNumber) +' can not enroll to product '+ QUOTENAME(PD.productCode),'E',PD.PaymentID FROM @tblDetail PD 
		INNER JOIN tblInsuree I ON I.CHFID = PD.InsuranceNumber
		INNER JOIN tblFamilies F ON F.InsureeID = I.InsureeID
		INNER JOIN tblVillages V ON V.VillageId = F.LocationId
		INNER JOIN tblWards W ON W.WardId = V.WardId
		INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
		INNER JOIN tblRegions R ON R.RegionId =  D.Region
		LEFT OUTER JOIN tblProduct PR ON (PR.LocationId IS NULL OR PR.LocationId = D.Region OR PR.LocationId = D.DistrictId) AND (GETDATE()  BETWEEN PR.DateFrom AND PR.DateTo) AND PR.ProductCode = PD.ProductCode
		WHERE 
		I.ValidityTo IS NULL
		AND F.ValidityTo IS NULL
		AND PR.ValidityTo IS NULL
		AND PR.ProdID IS NULL
		AND ISNULL(LEN(PD.productCode),'') > 0
		AND ISNULL(LEN(PD.InsuranceNumber),'') > 0


	--4. Invalid Officer
		INSERT INTO @tblFeedback(fdMsg, fdType, paymentID)
		SELECT 'Enrollment officer '+ QUOTENAME(PY.officerCode) +' does not exists ' ,'E',PD.PaymentID  FROM @tblDetail PD
		INNER JOIN @tblHeader PY ON PY.PaymentID = PD.PaymentID
		LEFT OUTER JOIN tblOfficer O ON O.Code = PY.OfficerCode
		WHERE
		O.ValidityTo IS NULL
		AND PY.OfficerCode IS NOT NULL
		AND O.Code IS NULL


	--4. Invalid Officer/Product Match
		INSERT INTO @tblFeedback(fdMsg, fdType, paymentID)
		SELECT 'Enrollment officer '+ QUOTENAME(PY.officerCode) +' can not sell the product '+ QUOTENAME(PD.productCode),'E',PD.PaymentID  FROM @tblDetail PD
		INNER JOIN @tblHeader PY ON PY.PaymentID = PD.PaymentID
		LEFT OUTER JOIN tblOfficer O ON O.Code = PY.OfficerCode
		INNER JOIN tblDistricts D ON D.DistrictId = O.LocationId
		LEFT JOIN tblProduct PR ON PR.ProductCode = PD.ProductCode AND (PR.LocationId IS NULL OR PR.LocationID = D.Region OR PR.LocationId = D.DistrictId)
		WHERE
		O.ValidityTo IS NULL
		AND PY.OfficerCode IS NOT NULL
		AND PR.ValidityTo IS NULL
		AND D.ValidityTo IS NULL
		AND PR.ProdID IS NULL
		
	--5. Premiums not available
	INSERT INTO @tblFeedback(fdMsg, fdType, paymentID)
	SELECT 'Premium from Enrollment officer '+ QUOTENAME(PY.officerCode) +' is not yet available ','E',PD.PaymentID FROM @tblDetail PD 
	INNER JOIN @tblHeader PY ON PY.PaymentID = PD.PaymentID
	WHERE
	PD.PremiumID IS NULL
	AND PY.officerCode IS NOT NULL




	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/
	

	---DELETE ALL INVALID PAYMENTS
		DELETE PY FROM @tblHeader PY
		INNER JOIN @tblFeedback F ON F.paymentID = PY.PaymentID
		WHERE F.fdType ='E'


		DELETE PD FROM @tblDetail PD
		INNER JOIN @tblFeedback F ON F.paymentID = PD.PaymentID
		WHERE F.fdType ='E'

		IF NOT EXISTS(SELECT 1 FROM @tblHeader)
			INSERT INTO @tblFeedback(fdMsg, fdType )
			SELECT 'No Payment matched  ', 'I' FROM @tblHeader P

		--DISTRIBUTE PAYMENTS EVENLY
		UPDATE PD SET PD.DistributedValue = PH.ReceivedAmount*( PD.PolicyValue/PH.TotalPolicyValue) FROM @tblDetail PD
		INNER JOIN @tblHeader PH ON PH.PaymentID = PD.PaymentID

		--INSERT ONLY RENEWALS
		DECLARE @DistributedValue DECIMAL(18, 2)
		DECLARE @InsuranceNumber NVARCHAR(12)
		DECLARE @productCode NVARCHAR(8)
		DECLARE @PhoneNumber NVARCHAR(12)
		DECLARE @PaymentDetailsID INT

		--loop below only for SELF PAYER
		DECLARE @PreviousPolicyID INT
		IF EXISTS(SELECT 1 FROM @tblDetail PD INNER JOIN @tblHeader P ON PD.PaymentID = P.PaymentID WHERE PD.PolicyStage ='R' AND P.PhoneNumber IS NOT NULL AND P.officerCode IS NULL AND PD.policyID IS NOT NULL)
			BEGIN
			DECLARE CurPolicies CURSOR FOR SELECT PaymentDetailsID, InsuranceNumber, productCode, PhoneNumber, DistributedValue, policyID FROM @tblDetail PD INNER JOIN @tblHeader P ON PD.PaymentID = P.PaymentID WHERE PD.PolicyStage ='R' AND P.PhoneNumber IS NOT NULL AND P.officerCode IS NULL 
			OPEN CurPolicies;
			FETCH NEXT FROM CurPolicies INTO @PaymentDetailsID,  @InsuranceNumber, @productCode, @PhoneNumber, @DistributedValue, @PreviousPolicyID
			WHILE @@FETCH_STATUS = 0
			BEGIN			
						DECLARE @ProdId INT
						DECLARE @FamilyId INT
						DECLARE @OfficerID INT
						DECLARE @PolicyId INT
						DECLARE @PremiumId INT
						DECLARE @StartDate DATE
						DECLARE @ExpiryDate DATE
						DECLARE @EffectiveDate DATE
						DECLARE @EnrollmentDate DATE = GETDATE()
						DECLARE @PolicyStatus TINYINT=1
						DECLARE @PreviousPolicyStatus TINYINT=1
						DECLARE @PolicyValue DECIMAL(18, 2)
						DECLARE @PaidAmount DECIMAL(18, 2)
						DECLARE @Balance DECIMAL(18, 2)
						DECLARE @ErrorCode INT
						DECLARE @HasCycle BIT
						DECLARE @isActivated BIT = 0
						DECLARE @TransactionNo NVARCHAR(50)
						SELECT @ProdId = ProdID, @FamilyId = FamilyID, @OfficerID = OfficerID, @PreviousPolicyStatus = PolicyStatus  FROM tblPolicy WHERE PolicyID = @PreviousPolicyID AND ValidityTo IS NULL
							EXEC @PolicyValue = uspPolicyValue @FamilyId, @ProdId, 0, 'R', @enrollmentDate, @PreviousPolicyID, @ErrorCode OUTPUT;
							DELETE FROM @tblPeriod
							
							SET @TransactionNo = (SELECT ISNULL(PY.TransactionNo,'') FROM @tblHeader PY INNER JOIN @tblDetail PD ON PD.PaymentID = PY.PaymentID AND PD.policyID = @PreviousPolicyID)
							
							
							IF @PreviousPolicyStatus = 1 
								BEGIN
									--Get the previous paid amount for only Iddle policy
									SELECT @PaidAmount =  ISNULL(SUM(Amount),0) FROM tblPremium  PR 
									LEFT OUTER JOIN tblPolicy PL ON PR.PolicyID = PL.PolicyID  
									WHERE PR.PolicyID = @PreviousPolicyID 
									AND PR.ValidityTo IS NULL 
									AND PL.ValidityTo IS NULL
									AND PL.PolicyStatus = 1
									
									SELECT @PolicyValue = ISNULL(PolicyValue,0) FROM tblPolicy WHERE PolicyID = @PreviousPolicyID AND ValidityTo IS NULL

									IF (ISNULL(@DistributedValue,0) + ISNULL(@PaidAmount,0)) - ISNULL(@PolicyValue,0) >= 0
										BEGIN
											SET @PolicyStatus=2
											SET @EffectiveDate = (SELECT StartDate FROM tblPolicy WHERE PolicyID = @PreviousPolicyID AND ValidityTo IS NULL)
											SET @isActivated = 1
											SET @Balance = 0

											INSERT INTO tblPolicy(FamilyID,EnrollDate,StartDate,EffectiveDate,ExpiryDate,PolicyStatus,PolicyValue,ProdID,OfficerID,PolicyStage,ValidityFrom, ValidityTo, LegacyID,  AuditUserID, isOffline)
														 SELECT FamilyID,EnrollDate,StartDate,EffectiveDate,ExpiryDate,PolicyStatus,PolicyValue,ProdID,OfficerID,PolicyStage,ValidityFrom,GETDATE(), PolicyID, AuditUserID, isOffline FROM tblPolicy WHERE PolicyID = @PreviousPolicyID

									
											INSERT INTO tblInsureePolicy
											(InsureeId,PolicyId,EnrollmentDate,StartDate,EffectiveDate,ExpiryDate,ValidityFrom,ValidityTo,LegacyId,AuditUserId,isOffline)
											SELECT InsureeId, PolicyId, EnrollmentDate, StartDate, EffectiveDate,ExpiryDate,ValidityFrom,GETDATE(),PolicyId,AuditUserId,isOffline FROM tblInsureePolicy 
											WHERE PolicyID = @PreviousPolicyID AND ValidityTo IS NULL

											UPDATE tblPolicy SET PolicyStatus = @PolicyStatus,  EffectiveDate  = @EffectiveDate, ExpiryDate = @ExpiryDate, ValidityFrom = GETDATE(), AuditUserID = @AuditUserId WHERE PolicyID = @PreviousPolicyID

											UPDATE tblInsureePolicy SET EffectiveDate = @EffectiveDate, ValidityFrom = GETDATE(), AuditUserID = @AuditUserId WHERE ValidityTo IS NULL AND PolicyId = @PreviousPolicyID  AND EffectiveDate IS NULL
											SET @PolicyId = @PreviousPolicyID

										END
									ELSE
										BEGIN
											SET @Balance = ISNULL(@PolicyValue,0) - (ISNULL(@DistributedValue,0) + ISNULL(@PaidAmount,0))
											SET @isActivated = 0
											SET @PolicyId = @PreviousPolicyID
										END
								END
							ELSE
								BEGIN --insert new Renewals if the policy is not Iddle
								DECLARE @StartCycle NVARCHAR(5)
									SELECT @StartCycle= ISNULL(StartCycle1, ISNULL(StartCycle2,ISNULL(StartCycle3,StartCycle4))) FROM tblProduct WHERE ProdID = @PreviousPolicyID
									IF @StartCycle IS NOT NULL
									SET @HasCycle = 1
									ELSE
									SET @HasCycle = 0
									SET @EnrollmentDate = (SELECT DATEADD(DAY,1,expiryDate) FROM tblPolicy WHERE PolicyID = @PreviousPolicyID  AND ValidityTo IS NULL)
									INSERT INTO @tblPeriod(StartDate, ExpiryDate, HasCycle)
									EXEC uspGetPolicyPeriod @ProdId, @EnrollmentDate, @HasCycle;
									SET @StartDate = (SELECT startDate FROM @tblPeriod)
									SET @ExpiryDate =(SELECT expiryDate FROM @tblPeriod)
									IF ISNULL(@DistributedValue,0) - ISNULL(@PolicyValue,0) >= 0
										BEGIN
											SET @PolicyStatus=2
											SET @EffectiveDate = @StartDate
											SET @isActivated = 1
											SET @Balance = 0
										END
										ELSE
										BEGIN
											SET @Balance = ISNULL(@PolicyValue,0) - (ISNULL(@DistributedValue,0))
											SET @isActivated = 0
											SET @PolicyStatus=1

										END

									INSERT INTO tblPolicy(FamilyID,EnrollDate,StartDate,EffectiveDate,ExpiryDate,PolicyStatus,PolicyValue,ProdID,OfficerID,PolicyStage,ValidityFrom,AuditUserID, isOffline)
									SELECT	@FamilyId, GETDATE(),@StartDate,@EffectiveDate,@ExpiryDate,@PolicyStatus,@PolicyValue,@ProdID,@OfficerID,'R',GETDATE(),@AuditUserId, 0 isOffline 
									SELECT @PolicyId = SCOPE_IDENTITY()

									UPDATE @tblDetail SET policyID  = @PolicyId WHERE policyID = @PreviousPolicyID

									DECLARE @InsureeId INT
									DECLARE CurNewPolicy CURSOR FOR SELECT I.InsureeID FROM tblInsuree I WHERE I.FamilyID = @FamilyId AND I.ValidityTo IS NULL
									OPEN CurNewPolicy;
									FETCH NEXT FROM CurNewPolicy INTO @InsureeId;
									WHILE @@FETCH_STATUS = 0
									BEGIN
										EXEC uspAddInsureePolicy @InsureeId;
										FETCH NEXT FROM CurNewPolicy INTO @InsureeId;
									END
									CLOSE CurNewPolicy;
									DEALLOCATE CurNewPolicy; 

									
								END	
				
				--INSERT PREMIUMS FOR INDIVIDUAL RENEWALS ONLY
				INSERT INTO tblPremium(PolicyID, Amount, PayType, Receipt, PayDate, ValidityFrom, AuditUserID)
				SELECT @PolicyId, @DistributedValue, 'C',@TransactionNo, GETDATE() PayDate, GETDATE() ValidityFrom, @AuditUserId AuditUserID 
				SELECT @PremiumId = SCOPE_IDENTITY()

				UPDATE @tblDetail SET PremiumID = @PremiumId  WHERE PaymentDetailsID = @PaymentDetailsID

				INSERT INTO @tblFeedback(InsuranceNumber, productCode, PhoneNumber, isActivated ,Balance, fdType)
				SELECT @InsuranceNumber, @productCode, @PhoneNumber, @isActivated,@Balance, 'A'

			FETCH NEXT FROM CurPolicies INTO @PaymentDetailsID,  @InsuranceNumber, @productCode, @PhoneNumber, @DistributedValue, @PreviousPolicyID;
			END
			CLOSE CurPolicies;
			DEALLOCATE CurPolicies; 
			END
			
			-- ABOVE LOOP SELF PAYER ONLY

		--Update the actual tblpayment & tblPaymentDetails
			UPDATE PD SET PD.PremiumID = TPD.PremiumId, PD.Amount = TPD.DistributedValue,  ValidityFrom =GETDATE(), AuditedUserId = @AuditUserId 
			FROM @tblDetail TPD
			INNER JOIN tblPaymentDetails PD ON PD.PaymentDetailsID = TPD.PaymentDetailsID 
			

			UPDATE P SET P.PaymentStatus = 5, P.MatchedDate = GETDATE(),  ValidityFrom = GETDATE(), AuditedUSerID = @AuditUserId FROM tblPayment P
			INNER JOIN @tblDetail PD ON PD.PaymentID = P.PaymentID

			IF EXISTS(SELECT COUNT(1) FROM @tblHeader PH )
			BEGIN
				SET @paymentMatched= (SELECT COUNT(1)  FROM @tblHeader PH )
				INSERT INTO @tblFeedback(fdMsg, fdType )
				SELECT CONVERT(NVARCHAR(4), ISNULL(@paymentMatched,0))  +' Payment(s) matched ', 'I' 
			END

			UPDATE @tblFeedback SET PaymentFound =ISNULL(@paymentFound,0), PaymentMatched = ISNULL(@paymentMatched,0),APIKey = (SELECT APIKey FROM tblIMISDefaults)

		IF @InTopIsolation = 0 
			COMMIT TRANSACTION MATCHPAYMENT
		
		SELECT fdMsg, fdType, productCode, InsuranceNumber, PhoneNumber, isActivated, Balance,PaymentFound, PaymentMatched, APIKey FROM @tblFeedback
		RETURN 0
		END TRY
		BEGIN CATCH
			IF @InTopIsolation = 0
				ROLLBACK TRANSACTION MATCHPAYMENT
			SELECT fdMsg, fdType FROM @tblFeedback
			SELECT ERROR_MESSAGE ()
			RETURN -1
		END CATCH
	

END







GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[uspReceivePayment]
(
	
	@XML XML,
	@Payment_ID BIGINT =NULL OUTPUT
)
AS
BEGIN

	DECLARE
	@PaymentID BIGINT =NULL,
	@ControlNumberID BIGINT=NULL,
	@PaymentDate DATE,
    @ControlNumber NVARCHAR(50),
    @TransactionNo NVARCHAR(50),
    @ReceiptNo NVARCHAR(100),
    @PaymentOrigin NVARCHAR(50),
    @PhoneNumber NVARCHAR(50),
    @OfficerCode NVARCHAR(50),
    @InsuranceNumber NVARCHAR(12),
    @productCode NVARCHAR(8),
    @Amount  DECIMAL(18,2),
    @isRenewal  BIT,
	@ExpectedAmount DECIMAL(18,2),
	@ErrorMsg NVARCHAR(50)
	

		

	
				
	BEGIN TRY

		DECLARE @tblDetail TABLE(InsuranceNumber nvarchar(12),productCode nvarchar(8), isRenewal BIT)

		SELECT @PaymentID = NULLIF(T.H.value('(PaymentID)[1]','INT'),''),
			   @PaymentDate = NULLIF(T.H.value('(PaymentDate)[1]','DATE'),''),
			   @ControlNumber = NULLIF(T.H.value('(ControlNumber)[1]','NVARCHAR(50)'),''),
			   @ReceiptNo = NULLIF(T.H.value('(ReceiptNo)[1]','NVARCHAR(100)'),''),
			   @TransactionNo = NULLIF(T.H.value('(TransactionNo)[1]','NVARCHAR(50)'),''),
			   @PaymentOrigin = NULLIF(T.H.value('(PaymentOrigin)[1]','NVARCHAR(50)'),''),
			   @PhoneNumber = NULLIF(T.H.value('(PhoneNumber)[1]','NVARCHAR(25)'),''),
			   @OfficerCode = NULLIF(T.H.value('(OfficerCode)[1]','NVARCHAR(50)'),''),
			   @Amount = T.H.value('(Amount)[1]','DECIMAL(18,2)')
		FROM @XML.nodes('PaymentData') AS T(H)
	

		DECLARE @MyAmount Decimal(18,2)
		DECLARE @MyControlNumber nvarchar(50)
		DECLARE @MyReceivedAmount decimal(18,2) = 0 
		DECLARE @MyStatus INT = 0 
		DECLARE @ResultCode AS INT = 0 

		INSERT INTO @tblDetail(InsuranceNumber, productCode, isRenewal)
		SELECT 
		LEFT(NULLIF(T.D.value('(InsureeNumber)[1]','NVARCHAR(12)'),''),12),
		LEFT(NULLIF(T.D.value('(ProductCode)[1]','NVARCHAR(8)'),''),8),
		T.D.value('(IsRenewal)[1]','BIT')
		FROM @XML.nodes('PaymentData/Detail') AS T(D)
	
		--VERIFICATION START
		IF ISNULL(@PaymentID,'') <> ''
		BEGIN
			--lets see if all element are matching

			SELECT @MyControlNumber = ControlNumber , @MyAmount = P.ExpectedAmount, @MyReceivedAmount = ISNULL(ReceivedAmount,0) , @MyStatus = PaymentStatus  from tblPayment P left outer join tblControlNumber CN ON P.PaymentID = CN.PaymentID  WHERE CN.ValidityTo IS NULL and P.ValidityTo IS NULL AND P.PaymentID = @PaymentID 


			--CONTROl NUMBER CHECK
			IF ISNULL(@MyControlNumber,'') <> ISNULL(@ControlNumber,'') 
			BEGIN
				--Control Nr mismatch
				SET @ErrorMsg = 'Wrong Control Number'
				SET @ResultCode = 3
			END 

			--AMOUNT VALE CHECK
			IF ISNULL(@MyAmount,0) <> ISNULL(@Amount ,0) 
			BEGIN
				--Amount mismatch
				SET @ErrorMsg = 'Wrong Payment Amount'
				SET @ResultCode = 4
			END 

			--DUPLICATION OF PAYMENT
			IF @MyReceivedAmount = @Amount 
			BEGIN
				SET @ErrorMsg = 'Duplicated Payment'
				SET @ResultCode = 5
			END

		END
		--VERIFICATION END

		IF @ResultCode <> 0
		BEGIN
			--ERROR OCCURRED
	
			INSERT INTO [dbo].[tblPayment]
			(PaymentDate, ReceivedDate, ReceivedAmount, ReceiptNo, TransactionNo, PaymentOrigin, PhoneNumber, PaymentStatus, OfficerCode, ValidityFrom, AuditedUSerID, RejectedReason) 
			VALUES (@PaymentDate, GETDATE(),  @Amount, @ReceiptNo, @TransactionNo, @PaymentOrigin, @PhoneNumber, -3, @OfficerCode,  GETDATE(), -1,@ErrorMsg)
			SET @PaymentID= SCOPE_IDENTITY();

			INSERT INTO [dbo].[tblPaymentDetails]
				([PaymentID],[ProductCode],[InsuranceNumber],[PolicyStage],[ValidityFrom],[AuditedUserId]) SELECT
				@PaymentID, productCode, InsuranceNumber,  CASE isRenewal WHEN 0 THEN 'N' ELSE 'R' END, GETDATE(), -1
				FROM @tblDetail D

			SET @Payment_ID = @PaymentID
			SELECT @Payment_ID
			RETURN @ResultCode

		END
		ELSE
		BEGIN
			--ALL WENT OK SO FAR
		
		
			IF ISNULL(@PaymentID ,0) <> 0 
			BEGIN
				--REQUEST/INTEND WAS FOUND
				UPDATE tblPayment SET ReceivedAmount = @Amount, PaymentDate = @PaymentDate, ReceivedDate = GETDATE(), PaymentStatus = 4, TransactionNo = @TransactionNo, ReceiptNo= @ReceiptNo, PaymentOrigin = @PaymentOrigin, ValidityFrom = GETDATE(),AuditedUserID =-1 WHERE PaymentID = @PaymentID  AND ValidityTo IS NULL AND PaymentStatus = 3
				SET @Payment_ID = @PaymentID
				RETURN 0 
			END
			ELSE
			BEGIN
				--PAYMENT WITHOUT INTEND TP PAY
				INSERT INTO [dbo].[tblPayment]
					(PaymentDate, ReceivedDate, ReceivedAmount, ReceiptNo, TransactionNo, PaymentOrigin, PhoneNumber, PaymentStatus, OfficerCode, ValidityFrom, AuditedUSerID) 
					VALUES (@PaymentDate, GETDATE(),  @Amount, @ReceiptNo, @TransactionNo, @PaymentOrigin, @PhoneNumber, 4, @OfficerCode,  GETDATE(), -1)
				SET @PaymentID= SCOPE_IDENTITY();
							
				INSERT INTO [dbo].[tblPaymentDetails]
				([PaymentID],[ProductCode],[InsuranceNumber],[PolicyStage],[ValidityFrom],[AuditedUserId]) SELECT
				@PaymentID, productCode, InsuranceNumber,  CASE isRenewal WHEN 0 THEN 'N' ELSE 'R' END, GETDATE(), -1
				FROM @tblDetail D
				SET @Payment_ID = @PaymentID
				RETURN 0 

			END
		 
		
			
		END

		RETURN 0

	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE()
		RETURN -1
	END CATCH
	

	
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[uspProcessSingleClaimStep1]
	
	@AuditUser as int = 0,
	@ClaimID as int,
	@InsureeID as int, 
	@HFCareType as char(1),
	@RowID as int = 0,
	@AdultChild as nvarchar(1),
	@RtnStatus as int = 0 OUTPUT
	
		
	/*
	Rejection reasons:
	0 = NOT REJECTED
	1 = Item/Service not in Registers
	2 = Item/Service not in HF Pricelist 
	3 = Item/Service not in Covering Product
	4 = Item/Service Limitation Fail
	5 = Item/Service Frequency Fail
	6 = Item/Service DUPLICATED
	7 = CHFID Not valid / Family Not Valid 
	8 = ICD Code not in current ICD list 
	9 = Target date provision invalid
	10= Care type not consistant with Facility 
	11=
	12=
	*/
	
AS
BEGIN
	DECLARE @RtnItemsPassed as int 
	DECLARE @RtnServicesPassed as int 
	DECLARE @RtnItemsRejected as int 
	DECLARE @RtnServicesRejected as int 

	DECLARE @oReturnValue as int 
	SET @oReturnValue = 0 
	SET @RtnStatus = 0  
	DECLARE @HFID as int  
	DECLARE @FamilyID as int  
	DECLARE @TargetDate as Date 
	DECLARE @ClaimItemID as int 
	DECLARE @ClaimServiceID as int 
	DECLARE @ItemID as int
	DECLARE @ServiceID as int
	DECLARE @ProdItemID as int
	DECLARE @ProdServiceID as int
	DECLARE @ItemPatCat as int 
	DECLARE @ItemPrice as decimal(18,2)
	DECLARE @ServicePrice as decimal(18,2)
	DECLARE @ServicePatCat as int 
	DECLARE @Gender as nvarchar(1)
	DECLARE @Adult as bit
	DECLARE @DOB as date
	DECLARE @PatientMask as int
	DECLARE @CareType as Char
	DECLARE @PriceAsked as decimal(18,2)
	DECLARE @PriceApproved as decimal(18,2)
	DECLARE @PriceAdjusted as decimal(18,2)
	DECLARE @PriceValuated as decimal(18,2)
	DECLARE @PriceOrigin as Char
	DECLARE @ClaimPrice as Decimal(18,2)
	DECLARE @ProductID as int   
	DECLARE @PolicyID as int 
	DECLARE @ProdItemID_C as int 
	DECLARE @ProdItemID_F as int 
	DECLARE @ProdServiceID_C as int 
	DECLARE @ProdServiceID_F as int 
	DECLARE @CoSharingPerc as decimal(18,2)
	DECLARE @FixedLimit as decimal(18,2)
	DECLARE @ProdAmountOwnF as decimal(18,2)
	DECLARE @ProdAmountOwnC as decimal(18,2)
	DECLARE @ProdCareType as Char
		
		
	DECLARE @LimitationType as Char(1)
	DECLARE @LimitationValue as decimal(18,2)	
	
	DECLARE @VisitType as CHAR(1)

	SELECT @VisitType = ISNULL(VisitType,'O') from tblClaim where ClaimId = @ClaimID and ValidityTo IS NULL

	BEGIN TRY
	
	--***** PREPARE PHASE *****
	
	SELECT @FamilyID = tblFamilies.FamilyID FROM tblFamilies INNER JOIN tblInsuree ON tblFamilies.FamilyID = tblInsuree.FamilyID  WHERE tblFamilies.ValidityTo IS NULL AND tblInsuree.InsureeID = @InsureeID AND tblInsuree.ValidityTo IS NULL 

	IF ISNULL(@FamilyID,0)=0 
	BEGIN
		UPDATE tblClaimServices SET tblClaimServices.RejectionReason = 7 WHERE ClaimID = @ClaimID AND tblClaimServices.RejectionReason = 0 
		UPDATE tblClaimItems SET tblClaimItems.RejectionReason = 7 WHERE ClaimID = @ClaimID AND tblClaimItems.RejectionReason = 0 
		GOTO UPDATECLAIMDETAILS 
	END	
	
	SELECT @TargetDate = ISNULL(TblClaim.DateTo,TblClaim.DateFrom) FROM TblClaim WHERE ClaimID = @ClaimID 
	IF @TargetDate IS NULL 
	BEGIN
		UPDATE tblClaimServices SET tblClaimServices.RejectionReason = 9 WHERE ClaimID = @ClaimID AND tblClaimServices.RejectionReason = 0 
		UPDATE tblClaimItems SET tblClaimItems.RejectionReason = 9 WHERE ClaimID = @ClaimID  AND tblClaimItems.RejectionReason = 0 
		GOTO UPDATECLAIMDETAILS 
	END	
		
		  
	SET @PatientMask = 0 
	SELECT @Gender = Gender FROm tblInsuree WHERE InsureeID = @InsureeID 
	IF @Gender = 'M' OR @Gender = 'O'
		SET @PatientMask = @PatientMask + 1 
	ELSE
		SET @PatientMask = @PatientMask + 2 
	
	SELECT @DOB = DOB FROM tblInsuree WHERE InsureeID = @InsureeID 
	IF @AdultChild = 'A' 
		SET @PatientMask = @PatientMask + 4 
	ELSE
		SET @PatientMask = @PatientMask + 8 
		
	/*PREPARE HISTORIC TABLE WITh RELEVANT ITEMS AND SERVICES*/

	DECLARE  @DTBL_ITEMS TABLE (
							[ItemID] [int] NOT NULL,
							[ItemCode] [nvarchar](6) NOT NULL,
							[ItemType] [char](1) NOT NULL,
							[ItemPrice] [decimal](18, 2) NOT NULL,
							[ItemCareType] [char](1) NOT NULL,
							[ItemFrequency] [smallint] NULL,
							[ItemPatCat] [tinyint] NOT NULL
							)

	INSERT INTO @DTBL_ITEMS (ItemID , ItemCode, ItemType , ItemPrice, ItemCaretype ,ItemFrequency, ItemPatCat) 
	SELECT ItemID , ItemCode, ItemType , ItemPrice, ItemCaretype ,ItemFrequency, ItemPatCat FROM 
	(SELECT  ROW_NUMBER() OVER(PARTITION BY ItemId ORDER BY ValidityFrom DESC)RNo,AllItems.* FROM
	(
	SELECT Sub1.* FROM
	(
	SELECT ItemID , ItemCode, ItemType , ItemPrice, ItemCaretype ,ItemFrequency, ItemPatCat , ValidityFrom, ValidityTo, LegacyID from tblitems Where (ValidityTo IS NULL) OR ((NOT ValidityTo IS NULL) AND (LegacyID IS NULL))
	UNION ALL
	SELECT  LegacyID as ItemID , ItemCode, ItemType , ItemPrice, ItemCaretype ,ItemFrequency, ItemPatCat , ValidityFrom,ValidityTo, LegacyID  FROM tblItems Where  (NOT ValidityTo IS NULL) AND (NOT LegacyID IS NULL)
	
	) Sub1
	INNER JOIN 
	(
	SELECT        tblClaimItems.ItemID
	FROM            tblClaimItems 
	WHERE        (tblClaimItems.ValidityTo IS NULL) AND tblClaimItems.ClaimID = @ClaimID
	) Sub2 ON Sub1.ItemID = Sub2.ItemID 
	)  AllItems 
	WHERE CONVERT(date,ValidityFrom,103) <= @TargetDate 
	)Result
	WHERE Rno = 1 AND ((ValidityTo IS NULL) OR (NOT ValidityTo IS NULL AND NOT LegacyID IS NULL ))  	

	DECLARE  @DTBL_SERVICES TABLE (
							[ServiceID] [int] NOT NULL,
							[ServCode] [nvarchar](6) NOT NULL,
							[ServType] [char](1) NOT NULL,
							[ServLevel] [char](1) NOT NULL,
							[ServPrice] [decimal](18, 2) NOT NULL,
							[ServCareType] [char](1) NOT NULL,
							[ServFrequency] [smallint] NULL,
							[ServPatCat] [tinyint] NOT NULL,
							[ServCategory] [char](1) NULL
							)

	INSERT INTO @DTBL_SERVICES (ServiceID , ServCode, ServType , ServLevel, ServPrice, ServCaretype ,ServFrequency, ServPatCat, ServCategory ) 
	SELECT ServiceID , ServCode, ServType , ServLevel ,ServPrice, ServCaretype ,ServFrequency, ServPatCat,ServCategory FROM 
	(SELECT  ROW_NUMBER() OVER(PARTITION BY ServiceId ORDER BY ValidityFrom DESC)RNo,AllServices.* FROM
	(
	SELECT Sub1.* FROM
	(
	SELECT ServiceID , ServCode, ServType , ServLevel  ,ServPrice, ServCaretype ,ServFrequency, ServPatCat , ServCategory ,ValidityFrom, ValidityTo, LegacyID from tblServices WHere (ValidityTo IS NULL) OR ((NOT ValidityTo IS NULL) AND (LegacyID IS NULL))
	UNION ALL
	SELECT  LegacyID as ServiceID , ServCode, ServType , ServLevel  ,ServPrice, ServCaretype ,ServFrequency, ServPatCat , ServCategory , ValidityFrom, ValidityTo, LegacyID FROM tblServices Where  (NOT ValidityTo IS NULL) AND (NOT LegacyID IS NULL)
	) Sub1
	INNER JOIN 
	(
	SELECT        tblClaimServices.ServiceID 
	FROM            tblClaim INNER JOIN
							 tblClaimServices ON tblClaim.ClaimID = tblClaimServices.ClaimID
	WHERE        (tblClaimServices.ValidityTo IS NULL) AND tblClaim.ClaimID = @ClaimID
	) Sub2 ON Sub1.ServiceID = Sub2.ServiceID 
	)  AllServices 
	WHERE CONVERT(date,ValidityFrom,103) <= @TargetDate
	)Result
	WHERE Rno = 1 AND ((ValidityTo IS NULL) OR (NOT ValidityTo IS NULL AND NOT LegacyID IS NULL ))   

	--***** CHECK 1 ***** --> UPDATE to REJECTED for Items/Services not in registers   REJECTION REASON = 1
	
	UPDATE tblClaimItems SET tblClaimItems.RejectionReason = 1     
	FROM         tblClaim INNER JOIN
                      tblClaimItems ON tblClaim.ClaimID = tblClaimItems.ClaimID 
                      WHERE tblClaim.ClaimID = @ClaimID AND tblClaimItems.ValidityTo IS NULL AND tblClaimItems.RejectionReason = 0 AND tblClaimItems.ItemID NOT IN 
                      (
                      SELECT     ItemID FROM @DTBL_ITEMS
                      )
                      
	UPDATE tblClaimServices SET tblClaimServices.RejectionReason = 1     
	FROM         tblClaim INNER JOIN
                      tblClaimServices ON tblClaim.ClaimID = tblClaimServices.ClaimID 
                      WHERE tblClaim.ClaimID = @ClaimID AND tblClaimServices.ValidityTo IS NULL AND tblClaimServices.RejectionReason = 0  AND tblClaimServices.ServiceID  NOT IN 
                      (
                      SELECT     ServiceID FROM @DTBL_SERVICES  
                      )
	
	--***** CHECK 2 ***** --> UPDATE to REJECTED for Items/Services not in Pricelists  REJECTION REASON = 2
	SELECT @HFID = HFID from tblClaim WHERE ClaimID = @ClaimID 
	
	UPDATE tblClaimItems SET tblClaimItems.RejectionReason = 2
	FROM dbo.tblClaimItems 
	LEFT OUTER JOIN 
	(SELECT     tblPLItemsDetail.ItemID
	FROM         tblHF INNER JOIN
						  tblPLItems ON tblHF.PLItemID = tblPLItems.PLItemID INNER JOIN
						  tblPLItemsDetail ON tblPLItems.PLItemID = tblPLItemsDetail.PLItemID
	WHERE     (tblHF.HfID = @HFID) AND (tblPLItems.ValidityTo IS NULL) AND (tblPLItemsDetail.ValidityTo IS NULL)) PLItems 
	ON tblClaimItems.ItemID = PLItems.ItemID 
	WHERE tblClaimItems.ClaimID = @ClaimID AND tblClaimItems.RejectionReason = 0 AND tblClaimItems.ValidityTo IS NULL AND PLItems.ItemID IS NULL
	
	UPDATE tblClaimServices SET tblClaimServices.RejectionReason = 2 
	FROM dbo.tblClaimServices 
	LEFT OUTER JOIN 
	(SELECT     tblPLServicesDetail.ServiceID 
	FROM         tblHF INNER JOIN
						  tblPLServicesDetail ON tblHF.PLServiceID = tblPLServicesDetail.PLServiceID
	WHERE     (tblHF.HfID = @HFID) AND (tblPLServicesDetail.ValidityTo IS NULL) AND (tblPLServicesDetail.ValidityTo IS NULL)) PLServices 
	ON tblClaimServices.ServiceID = PLServices.ServiceID  
	WHERE tblClaimServices.ClaimID = @ClaimID AND  tblClaimServices.RejectionReason = 0  AND tblClaimServices.ValidityTo IS NULL AND PLServices.ServiceID  IS NULL
	
	
	-- ** !!!!! ITEMS LOOPING !!!!! ** 
	
	--now loop through all (remaining) items and determine what is the matching product within valid policies using the rule least cost sharing for Insuree 
	-- at this stage we only check if any valid product itemline is found --> will not yet assign the line. 
	
	DECLARE CLAIMITEMLOOP CURSOR LOCAL FORWARD_ONLY FOR SELECT     tblClaimItems.ClaimItemID, tblClaimItems.PriceAsked, PriceApproved, Items.ItemPrice, Items.ItemCareType, Items.ItemPatCat, Items.ItemID
														FROM         tblClaimItems INNER JOIN
																			  @DTBL_ITEMS Items ON tblClaimItems.ItemID = Items.ItemID 
														WHERE     (tblClaimItems.ClaimID = @ClaimID) AND (tblClaimItems.ValidityTo IS NULL) AND (tblClaimItems.RejectionReason = 0) ORDER BY tblClaimItems.ClaimItemID ASC
	OPEN CLAIMITEMLOOP
	FETCH NEXT FROM CLAIMITEMLOOP INTO @ClaimItemID, @PriceAsked, @PriceApproved, @ItemPrice ,@CareType, @ItemPatCat,@ItemID
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		SET @ProdItemID_C = 0 
		SET @ProdItemID_F = 0 
		
		IF ISNULL(@PriceAsked,0) > ISNULL(@PriceApproved,0)
			SET @ClaimPrice = @PriceAsked
		ELSE
			SET @ClaimPrice = @PriceApproved
		
		-- **** START CHECK 4 --> Item/Service Limitation Fail (4)*****
		IF (@ItemPatCat  & @PatientMask) <> @PatientMask 	
		BEGIN
			--inconsistant patient type check 
			UPDATE tblClaimItems SET RejectionReason = 4 WHERE ClaimItemID   = @ClaimItemID 
			GOTO NextItem
		END
		-- **** END CHECK 4 *****	
		
		---- **** START CHECK 10 --> Item Care type / HF caretype Fail (10)*****
		--IF (@CareType = 'I' AND @HFCareType = 'O') OR (@CareType = 'O' AND @HFCareType = 'I')	
		--BEGIN
		--	--inconsistant patient type check 
		--	UPDATE tblClaimItems SET RejectionReason = 10 WHERE ClaimItemID   = @ClaimItemID 
		--	GOTO NextItem
		--END
		---- **** END CHECK 10 *****	
		
		-- **** START ASSIGNING PROD ID to ClaimITEMS *****	
		IF @AdultChild = 'A'
		BEGIN
			--Try to find co-sharing product with the least co-sharing --> better for insuree
			
			IF @VisitType = 'O' 
			BEGIN
				SELECT TOP 1 @ProdItemID_C = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationType = 'C'
									  ORDER BY LimitAdult DESC

				SELECT TOP 1 @ProdItemID_F = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationType = 'F'
									  ORDER BY (CASE LimitAdult WHEN 0 THEN 1000000000000 ELSE LimitAdult END) DESC
			END

			IF @VisitType = 'E' 
			BEGIN
				SELECT TOP 1 @ProdItemID_C = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeE = 'C'
									  ORDER BY LimitAdultE DESC
			
				SELECT TOP 1 @ProdItemID_F = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeE = 'F'
									  ORDER BY (CASE LimitAdultE WHEN 0 THEN 1000000000000 ELSE LimitAdultE END) DESC
			END


			IF @VisitType = 'R' 
			BEGIN
				SELECT TOP 1 @ProdItemID_C = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeR = 'C'
									  ORDER BY LimitAdultR DESC
				
				SELECT TOP 1 @ProdItemID_F = tblProductItems.ProdItemID
					FROM         tblFamilies INNER JOIN
										  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
										  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
										  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
					WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
										  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
										  AND LimitationTypeR = 'F'
										  ORDER BY (CASE LimitAdultR WHEN 0 THEN 1000000000000 ELSE LimitAdultR END) DESC
			END

		END
		ELSE
		BEGIN
			IF @VisitType = 'O' 
			BEGIN
				SELECT TOP 1 @ProdItemID_C = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationType = 'C'
									  ORDER BY LimitChild DESC
			
				SELECT TOP 1 @ProdItemID_F = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationType = 'F'
									  ORDER BY (CASE LimitChild WHEN 0 THEN 1000000000000 ELSE LimitChild END) DESC		
			END
			IF @VisitType = 'E' 
			BEGIN
				SELECT TOP 1 @ProdItemID_C = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeE = 'C'
									  ORDER BY LimitChildE DESC
			
				SELECT TOP 1 @ProdItemID_F = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeE = 'F'
									  ORDER BY (CASE LimitChildE WHEN 0 THEN 1000000000000 ELSE LimitChildE END) DESC	
			END

			IF @VisitType = 'R' 
			BEGIN
				SELECT TOP 1 @ProdItemID_C = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeR = 'C'
									  ORDER BY LimitChildR DESC
			
				SELECT TOP 1 @ProdItemID_F = tblProductItems.ProdItemID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ItemID = @ItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeR = 'F'
									  ORDER BY (CASE LimitChildR WHEN 0 THEN 1000000000000 ELSE LimitChildR END) DESC	
			END

		END



		IF ISNULL(@ProdItemID_C,0) = 0 AND ISNULL(@ProdItemID_F,0) = 0 
		BEGIN
			-- No suitable product is found for this specific claim item 
			UPDATE tblClaimItems SET RejectionReason = 3 WHERE ClaimItemID = @ClaimItemID
			GOTO NextItem
		END
		ELSE
		BEGIN
			IF ISNULL(@ProdItemID_F,0) <> 0
			BEGIN
				IF @VisitType = 'O'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @FixedLimit = ISNULL(LimitAdult,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_F  
					ELSE
						SELECT @FixedLimit = ISNULL(LimitChild,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_F  
				END
				IF @VisitType = 'E'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @FixedLimit = ISNULL(LimitAdultE,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_F  
					ELSE
						SELECT @FixedLimit = ISNULL(LimitChildE,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_F  
				END
				IF @VisitType = 'R'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @FixedLimit = ISNULL(LimitAdultR,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_F  
					ELSE
						SELECT @FixedLimit = ISNULL(LimitChildR,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_F  
				END
				
			END	
			IF ISNULL(@ProdItemID_C,0) <> 0
			BEGIN

				IF @VisitType = 'O'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @CoSharingPerc = ISNULL(LimitAdult,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_C  
					ELSE
						SELECT @CoSharingPerc = ISNULL(LimitChild,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_C  
				END
				IF @VisitType = 'E'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @CoSharingPerc = ISNULL(LimitAdultE,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_C  
					ELSE
						SELECT @CoSharingPerc = ISNULL(LimitChildE,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_C  
				END
				IF @VisitType = 'R'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @CoSharingPerc = ISNULL(LimitAdultR,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_C  
					ELSE
						SELECT @CoSharingPerc = ISNULL(LimitChildR,0) FROM tblProductItems WHERE ProdItemID  = @ProdItemID_C  
				END

				
			END
		END
		
		IF ISNULL(@ProdItemID_C,0) <> 0 AND ISNULL(@ProdItemID_F,0) <> 0 
		BEGIN
			--Need to check which product would be the best to choose CO-sharing or FIXED
			IF @FixedLimit = 0 OR @FixedLimit > @ClaimPrice 
			BEGIN --no limit or higher than claimed amount
				SET @ProdItemID = @ProdItemID_F
				SET @ProdItemID_C = 0 
			END
			ELSE  
			BEGIN
				SET @ProdAmountOwnF =  @ClaimPrice - @FixedLimit
				IF (100 - @CoSharingPerc) > 0 
				BEGIN
					--Insuree pays own part on co-sharing 
					SET @ProdAmountOwnF =  @ClaimPrice - @FixedLimit
					SET @ProdAmountOwnC = ((100 - @CoSharingPerc)/100) * @ClaimPrice 
					IF @ProdAmountOwnC > @ProdAmountOwnF 
					BEGIN
						SET @ProdItemID = @ProdItemID_F  
						SET @ProdItemID_C = 0 
					END
					ELSE
					BEGIN 
						SET @ProdItemID = @ProdItemID_C  	
						SET @ProdItemID_F = 0
					END
				END
				ELSE
				BEGIN
					SET @ProdItemID = @ProdItemID_C  
					SET @ProdItemID_F = 0
				END
			END
		END
		ELSE
		BEGIN
			IF ISNULL(@ProdItemID_C,0) <> 0
			BEGIN
				-- Only Co-sharing 
				SET @ProdItemID = @ProdItemID_C
				SET @ProdItemID_F = 0 
			END
			ELSE
			BEGIN
				-- Only Fixed
				SET @ProdItemID = @ProdItemID_F 
				SET @ProdItemID_C = 0
			END 
		END
		
		
		SELECT @ProductID = tblProduct.ProdID FROM tblProduct INNER JOIN tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID WHERE tblProduct.ValidityTo IS NULL AND tblProductItems.ProdItemID = @ProdItemID 
		SELECT TOP 1 @PolicyID = tblPolicy.PolicyID 
			FROM         tblFamilies INNER JOIN
								  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
								  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
								  tblProductItems ON tblProduct.ProdID = tblProductItems.ProdID
			WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL) AND 
								  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductItems.ProdItemID = @ProdItemID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
								  
		-- **** END ASSIGNING PROD ID to CLAIM *****	
		
		-- **** START DETERMINE PRICE ITEM **** 
		SELECT @PriceOrigin = PriceOrigin FROM tblProductItems WHERE ProdItemID = @ProdItemID 
		
		IF @ProdItemID_C <> 0 
		BEGIN
			SET @LimitationType = 'C'
			SET @LimitationValue = @CoSharingPerc 		
		END
		ELSE
		BEGIN
			--FIXED LIMIT
			SET @LimitationType = 'F'
			SET @LimitationValue =@FixedLimit 
		END
		
		UPDATE tblClaimItems SET ProdID = @ProductID, PolicyID = @PolicyID , PriceAdjusted = @PriceAdjusted , PriceOrigin = @PriceOrigin, Limitation = @LimitationType , LimitationValue = @LimitationValue  WHERE ClaimItemID = @ClaimItemID 
		
		NextItem:
		FETCH NEXT FROM CLAIMITEMLOOP INTO @ClaimItemID, @PriceAsked, @PriceApproved, @ItemPrice ,@CareType, @ItemPatCat,@ItemID
	END
	CLOSE CLAIMITEMLOOP
	DEALLOCATE CLAIMITEMLOOP
	
	-- ** !!!!! ITEMS LOOPING !!!!! ** 
	
	--now loop through all (remaining) Services and determine what is the matching product within valid policies using the rule least cost sharing for Insuree 
	-- at this stage we only check if any valid product Serviceline is found --> will not yet assign the line. 
	
	DECLARE CLAIMSERVICELOOP CURSOR LOCAL FORWARD_ONLY FOR SELECT     tblClaimServices.ClaimServiceID, tblClaimServices.PriceAsked, PriceApproved, Serv.ServPrice, Serv.ServCareType, Serv.ServPatCat, Serv.ServiceID
														FROM         tblClaimServices INNER JOIN
																			  @DTBL_SERVICES Serv
																			   ON tblClaimServices.ServiceID = Serv.ServiceID
														WHERE     (tblClaimServices.ClaimID = @ClaimID) AND (tblClaimServices.ValidityTo IS NULL) AND (tblClaimServices.RejectionReason = 0) ORDER BY tblClaimServices.ClaimServiceID ASC
	OPEN CLAIMSERVICELOOP
	FETCH NEXT FROM CLAIMSERVICELOOP INTO @ClaimServiceID, @PriceAsked, @PriceApproved, @ServicePrice ,@CareType, @ServicePatCat,@ServiceID
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		SET @ProdServiceID_C = 0 
		SET @ProdServiceID_F = 0 
		
		IF ISNULL(@PriceAsked,0) > ISNULL(@PriceApproved,0)
			SET @ClaimPrice = @PriceAsked
		ELSE
			SET @ClaimPrice = @PriceApproved
		
		-- **** START CHECK 4 --> Service/Service Limitation Fail (4)*****
		IF (@ServicePatCat  & @PatientMask) <> @PatientMask 	
		BEGIN
			--inconsistant patient type check 
			UPDATE tblClaimServices SET RejectionReason = 4 WHERE ClaimServiceID   = @ClaimServiceID 
			GOTO NextService
		END
		-- **** END CHECK 4 *****	
		
		-- **** START CHECK 10 --> Service Care type / HF caretype Fail (10)*****
		--IF (@CareType = 'I' AND @HFCareType = 'O') OR (@CareType = 'O' AND @HFCareType = 'I')	
		--BEGIN
		--	--inconsistant patient type check 
		--	UPDATE tblClaimServices SET RejectionReason = 10 WHERE ClaimServiceID   = @ClaimServiceID 
		--	GOTO NextService
		--END
		-- **** END CHECK 10 *****	
		
		-- **** START ASSIGNING PROD ID to ClaimServiceS *****	
		IF @AdultChild = 'A'
		BEGIN
			--Try to find co-sharing product with the least co-sharing --> better for insuree
			
			IF @VisitType = 'O'
			BEGIN
				SELECT TOP 1 @ProdServiceID_C = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationType = 'C'
									  ORDER BY LimitAdult DESC
			
				SELECT TOP 1 @ProdServiceID_F = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationType = 'F'
									  ORDER BY (CASE LimitAdult WHEN 0 THEN 1000000000000 ELSE LimitAdult END) DESC
			END

			IF @VisitType = 'E'
			BEGIN
				SELECT TOP 1 @ProdServiceID_C = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeE = 'C'
									  ORDER BY LimitAdultE DESC
			
				SELECT TOP 1 @ProdServiceID_F = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeE = 'F'
									  ORDER BY (CASE LimitAdultE WHEN 0 THEN 1000000000000 ELSE LimitAdultE END) DESC
			END
			
			
			IF @VisitType = 'R'
			BEGIN
				SELECT TOP 1 @ProdServiceID_C = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeR = 'C'
									  ORDER BY LimitAdultR DESC
			
				SELECT TOP 1 @ProdServiceID_F = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeR = 'F'
									  ORDER BY (CASE LimitAdultR WHEN 0 THEN 1000000000000 ELSE LimitAdultR END) DESC
			END
			
		END
		ELSE
		BEGIN
			
			IF @VisitType = 'O'
			BEGIN
				SELECT TOP 1 @ProdServiceID_C = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationType = 'C'
									  ORDER BY LimitChild DESC
			
				SELECT TOP 1 @ProdServiceID_F = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationType = 'F'
									  ORDER BY (CASE LimitChild WHEN 0 THEN 1000000000000 ELSE LimitChild END) DESC		
			END
			IF @VisitType = 'E'
			BEGIN
				SELECT TOP 1 @ProdServiceID_C = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeE = 'C'
									  ORDER BY LimitChildE DESC
			
				SELECT TOP 1 @ProdServiceID_F = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeE = 'F'
									  ORDER BY (CASE LimitChildE WHEN 0 THEN 1000000000000 ELSE LimitChildE END) DESC		
			END


			IF @VisitType = 'R'
			BEGIN
				SELECT TOP 1 @ProdServiceID_C = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeR = 'C'
									  ORDER BY LimitChildR DESC
			
				SELECT TOP 1 @ProdServiceID_F = tblProductServices.ProdServiceID
				FROM         tblFamilies INNER JOIN
									  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
									  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
									  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
				WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
									  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ServiceID = @ServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
									  AND LimitationTypeR = 'F'
									  ORDER BY (CASE LimitChildR WHEN 0 THEN 1000000000000 ELSE LimitChildR END) DESC		
			END

		END
		
		
		
		IF ISNULL(@ProdServiceID_C,0) = 0 AND ISNULL(@ProdServiceID_F,0) = 0 
		BEGIN
			-- No suitable product is found for this specific claim Service 
			UPDATE tblClaimServices SET RejectionReason = 3 WHERE ClaimServiceID = @ClaimServiceID
			GOTO NextService
		END
		ELSE
		BEGIN
			IF ISNULL(@ProdServiceID_F,0) <> 0
			BEGIN
				IF @VisitType = 'O'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @FixedLimit = ISNULL(LimitAdult,0) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_F  
					ELSE
						SELECT @FixedLimit = ISNULL(LimitChild,0) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_F   
				END 
				IF @VisitType = 'E'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @FixedLimit = ISNULL(LimitAdultE,0) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_F  
					ELSE
						SELECT @FixedLimit = ISNULL(LimitChildE,0) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_F   
				END
				IF @VisitType = 'R'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @FixedLimit = ISNULL(LimitAdultR,0) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_F  
					ELSE
						SELECT @FixedLimit = ISNULL(LimitChildR,0) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_F   
				END
			END	
			IF ISNULL(@ProdServiceID_C,0) <> 0
			BEGIN
				IF @Visittype = 'O'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @CoSharingPerc = ISNULL(LimitAdult,100) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_C  
					ELSE
						SELECT @CoSharingPerc = ISNULL(LimitChild,100) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_C 
				END
				IF @Visittype = 'E'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @CoSharingPerc = ISNULL(LimitAdultE,100) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_C  
					ELSE
						SELECT @CoSharingPerc = ISNULL(LimitChildE,100) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_C 
				END 
				IF @Visittype = 'R'
				BEGIN
					IF @AdultChild = 'A'
						SELECT @CoSharingPerc = ISNULL(LimitAdultR,100) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_C  
					ELSE
						SELECT @CoSharingPerc = ISNULL(LimitChildR,100) FROM tblProductServices WHERE ProdServiceID  = @ProdServiceID_C 
				END
			END

		END
		
		IF ISNULL(@ProdServiceID_C,0) <> 0 AND ISNULL(@ProdServiceID_F,0) <> 0 
		BEGIN
			--Need to check which product would be the best to choose CO-sharing or FIXED
			IF @FixedLimit = 0 OR @FixedLimit > @ClaimPrice 
			BEGIN --no limit or higher than claimed amount
				SET @ProdServiceID = @ProdServiceID_F
				SET @ProdServiceID_C = 0 
			END
			ELSE
			BEGIN
				SET @ProdAmountOwnF =  @ClaimPrice - ISNULL(@FixedLimit,0)
				IF (100 - @CoSharingPerc) > 0 
				BEGIN
					--Insuree pays own part on co-sharing 
					SET @ProdAmountOwnF =  @ClaimPrice - @FixedLimit
					SET @ProdAmountOwnC = ((100 - @CoSharingPerc)/100) * @ClaimPrice 
					IF @ProdAmountOwnC > @ProdAmountOwnF 
					BEGIN
						SET @ProdServiceID = @ProdServiceID_F  
						SET @ProdServiceID_C = 0 
					END
					ELSE
					BEGIN 
						SET @ProdServiceID = @ProdServiceID_C  	
						SET @ProdServiceID_F = 0
					END
				END
				ELSE
				BEGIN
					SET @ProdServiceID = @ProdServiceID_C  
					SET @ProdServiceID_F = 0
				END
			END
		END
		ELSE
		BEGIN
			IF ISNULL(@ProdServiceID_C,0) <> 0
			BEGIN
				-- Only Co-sharing 
				SET @ProdServiceID = @ProdServiceID_C
				SET @ProdServiceID_F = 0 
			END
			ELSE
			BEGIN
				-- Only Fixed
				SET @ProdServiceID = @ProdServiceID_F 
				SET @ProdServiceID_C = 0
			END 
		END
		
		SELECT @ProductID = tblProduct.ProdID FROM tblProduct INNER JOIN tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID WHERE tblProduct.ValidityTo IS NULL AND tblProductServices.ProdServiceID = @ProdServiceID 
		SELECT TOP 1 @PolicyID = tblPolicy.PolicyID 
			FROM         tblFamilies INNER JOIN
								  tblPolicy ON tblFamilies.FamilyID = tblPolicy.FamilyID INNER JOIN
								  tblProduct ON tblPolicy.ProdID = tblProduct.ProdID INNER JOIN
								  tblProductServices ON tblProduct.ProdID = tblProductServices.ProdID
			WHERE     (tblPolicy.EffectiveDate <= @TargetDate) AND (tblPolicy.ExpiryDate >= @TargetDate) AND (tblPolicy.ValidityTo IS NULL) AND (tblProductServices.ValidityTo IS NULL) AND 
								  (tblPolicy.PolicyStatus = 2 OR tblPolicy.PolicyStatus = 8) AND (tblProductServices.ProdServiceID = @ProdServiceID) AND (tblFamilies.FamilyID = @FamilyID) AND (tblProduct.ValidityTo IS NULL)
								  
		-- **** END ASSIGNING PROD ID to CLAIM *****	
		
		-- **** START DETERMINE PRICE Service **** 
		SELECT @PriceOrigin = PriceOrigin FROM tblProductServices WHERE ProdServiceID = @ProdServiceID 
		
		IF @ProdServiceID_C <> 0 
		BEGIN
			SET @LimitationType = 'C'
			SET @LimitationValue = @CoSharingPerc 		
		END
		ELSE
		BEGIN
			--FIXED LIMIT
			SET @LimitationType = 'F'
			SET @LimitationValue =@FixedLimit 
		END
		
		UPDATE tblClaimServices SET ProdID = @ProductID, PolicyID = @PolicyID, PriceOrigin = @PriceOrigin, Limitation = @LimitationType , LimitationValue = @LimitationValue WHERE ClaimServiceID = @ClaimServiceID 
		
		NextService:
		FETCH NEXT FROM CLAIMSERVICELOOP INTO @ClaimServiceID, @PriceAsked, @PriceApproved, @ServicePrice ,@CareType, @ServicePatCat,@ServiceID
	END
	CLOSE CLAIMSERVICELOOP
	DEALLOCATE CLAIMSERVICELOOP
	
	
	
	
	
UPDATECLAIMDETAILS:
	UPDATE tblClaimItems SET ClaimItemStatus = 2 WHERE ClaimID = @ClaimID AND RejectionReason <> 0 
	UPDATE tblClaimServices SET ClaimServiceStatus = 2 WHERE ClaimID = @ClaimID AND RejectionReason <> 0 
	
	SELECT @RtnItemsPassed = ISNULL(COUNT(ClaimItemID),0) FROM dbo.tblClaimItems WHERE ClaimID = @ClaimID AND ClaimItemStatus = 1 AND ValidityTo IS NULL
	SELECT @RtnServicesPassed  = ISNULL(COUNT(ClaimServiceID),0) FROM dbo.tblClaimServices  WHERE ClaimID = @ClaimID AND ClaimServiceStatus = 1  AND ValidityTo IS NULL
	
	IF @RtnItemsPassed <> 0  OR @RtnServicesPassed <> 0  --UPDATE CLAIM TO PASSED !! (default is not yet passed before checking procedure 
	BEGIN
		SET @RtnStatus = 1 
	END
	ELSE
	BEGIN
		UPDATE tblClaim SET ClaimStatus = 1 WHERE ClaimID = @ClaimID --> set rejected as all items ands services did not pass ! 
		SET @RtnStatus = 2 
	END
	
	
	
FINISH:
	RETURN @oReturnValue
	
	END TRY
	
	BEGIN CATCH
		SELECT 'Unexpected error encountered'
		SET @oReturnValue = 1 
		RETURN @oReturnValue
		
	END CATCH
END









GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[uspProcessSingleClaimStep2]
	
	@AuditUser as int = 0,
	@ClaimID as int,
	@InsureeID as int,
	@HFLevel as Char(1),   --check later with Jiri --> will not be used anymore
	@RowID as int = 0,
	@AdultChild as Char(1),
	@Hospitalization as BIT,
	@IsProcess as BIT = 1,
	@RtnStatus as int = 0 OUTPUT
	
		
	/*
	Rejection reasons:
	0 = NOT REJECTED
	1 = Item/Service not in Registers
	2 = Item/Service not in HF Pricelist 
	3 = Item/Service not in Covering Product
	4 = Item/Service Limitation Fail
	5 = Item/Service Frequency Fail
	6 = Item/Service DUPLICATD
	7 = CHFID Not valid / Family Not Valid 
	8 = ICD Code not in current ICD list 
	9 = Target date provision invalid
	10= Care type not consistant with Facility 
	11=
	12=
	*/
	
AS
BEGIN
	
	DECLARE @oReturnValue as int
	SET @oReturnValue = 0 
		
	DECLARE @ProductID as int   
	DECLARE @PolicyID as int 
	DECLARE @Ceiling as decimal(18,2)
	DECLARE @Deductable as decimal(18,2)
	DECLARE @PrevDeducted as Decimal(18,2)
	DECLARE @Deducted as decimal(18,2)
	DECLARE @PrevRemunerated as decimal(18,2)
	DECLARE @Remunerated as decimal(18,2)
	
	DECLARE @DeductableType as Char(1)
	DECLARE @CeilingType as Char(1)
	
	DECLARE @ClaimItemID as int 
	DECLARE @ClaimServiceID as int
	DECLARE @PriceAsked as decimal(18,2)
	DECLARE @PriceApproved as decimal(18,2)
	DECLARE @PriceAdjusted as decimal(18,2)
	DECLARE @PLPrice as decimal(18,2)
	DECLARE @PriceOrigin as Char(1)
	DECLARE @Limitation as Char(1)
	DECLARE @Limitationvalue as Decimal(18,2)
	DECLARE @ItemQty as decimal(18,2)
	DECLARE @ServiceQty as decimal(18,2)
	DECLARE @QtyProvided as decimal(18,2) 
	DECLARE @QtyApproved as decimal(18,2)
	DECLARE @SetPriceValuated as decimal(18,2)
	DECLARE @SetPriceAdjusted as decimal(18,2)
	DECLARE @SetPriceRemunerated as decimal(18,2)
	DECLARE @SetPriceDeducted as decimal(18,2)	
	DECLARE @ExceedCeilingAmount as decimal(18,2)
	
	DECLARE @ExceedCeilingAmountCategory as decimal(18,2)
	

	DECLARE @WorkValue as decimal(18,2)
	--declare all ceilings and deductables from the cursor on product
	DECLARE @DedInsuree as decimal(18,2) 
	DECLARE @DedOPInsuree as decimal(18,2) 
	DECLARE @DedIPInsuree as decimal(18,2) 
	DECLARE @MaxInsuree as decimal(18,2)  
	DECLARE @MaxOPInsuree as decimal(18,2) 
	DECLARE @MaxIPInsuree as decimal(18,2) 
	DECLARE @DedTreatment as decimal(18,2)  
	DECLARE @DedOPTreatment as decimal(18,2)  
	DECLARE @DedIPTreatment as decimal(18,2)  
	DECLARE @MaxIPTreatment as decimal(18,2) 
	DECLARE @MaxTreatment as decimal(18,2) 
	DECLARE @MaxOPTreatment as decimal(18,2) 
	DECLARE @DedPolicy as decimal(18,2) 
	DECLARE @DedOPPolicy as decimal(18,2) 
	DECLARE @DedIPPolicy as decimal(18,2) 
	DECLARE @MaxPolicy as decimal(18,2) 
	DECLARE @MaxOPPolicy as decimal(18,2) 
	DECLARE @MaxIPPolicy as decimal(18,2) 
	
	DECLARE @CeilingConsult as Decimal(18,2) = 0 
	DECLARE @CeilingSurgery as Decimal(18,2) = 0 
	DECLARE @CeilingHospitalization as Decimal(18,2) = 0 
	DECLARE @CeilingDelivery as Decimal(18,2) = 0 
	DECLARE @CeilingAntenatal as decimal(18,2) =0 

	DECLARE @PrevRemuneratedConsult as decimal(18,2) = 0 
	DECLARE @PrevRemuneratedSurgery as decimal(18,2) = 0 
	DECLARE @PrevRemuneratedHospitalization as decimal(18,2) = 0 
	DECLARE @PrevRemuneratedDelivery as decimal(18,2) = 0 
	DECLARE @PrevRemuneratedAntenatal as decimal(18,2) = 0 

	DECLARE @RemuneratedConsult as decimal(18,2) = 0 
	DECLARE @RemuneratedSurgery as decimal(18,2) = 0 
	DECLARE @RemuneratedHospitalization as decimal(18,2) = 0 
	DECLARE @RemuneratedDelivery as decimal(18,2) = 0 
	DECLARE @RemuneratedAntenatal as decimal(18,2) = 0

	DECLARE @Treshold as INT
	DECLARE @MaxPolicyExtraMember decimal(18,2) = 0 
	DECLARE @MaxPolicyExtraMemberIP decimal(18,2) = 0 
	DECLARE @MaxPolicyExtraMemberOP decimal(18,2) = 0 
	DECLARE @MaxCeilingPolicy decimal (18,2) = 0 
	DECLARE @MaxCeilingPolicyIP decimal (18,2) = 0 
	DECLARE @MaxCeilingPolicyOP decimal (18,2) = 0 
	
	DECLARE @ServCategory as CHAR
	DECLARE @ClaimDateFrom as datetime
	DECLARE @ClaimDateTo as datetime
	

	DECLARE @RelativePrices as int = 0 
	DECLARE @PolicyMembers as int = 0 
	
	DECLARE @BaseCategory as CHAR(1)  = 'V'
	DECLARE @CeilingInterpretation as Char

	BEGIN TRY 
	
	--check first if this is a hospital claim falling under the hospitalization category
	--check first if this is a hospital claim falling under the hospitalization category
	
	-- S = Surgery
	-- D = Delivery
	-- A = Antenatal care
	-- H = Hospitalization
	-- C = Consultation
	-- O = Other
	-- V = Visit 

	SELECT @ClaimDateFrom = DateFrom,  @ClaimDateTo = DateTo FROM tblClaim Where ClaimID = @ClaimID 

	IF  EXISTS (SELECT tblClaimServices.ClaimServiceID FROM tblClaim INNER JOIN tblClaimServices ON tblClaim.ClaimID = tblClaimServices.ClaimID INNER JOIN tblServices ON tblClaimServices.ServiceID = tblServices.ServiceID
		WHERE        (tblClaim.ClaimID = @ClaimID) AND (tblClaim.ValidityTo IS NULL) AND (tblClaimServices.ValidityTo IS NULL) AND (tblServices.ServCategory = 'S') AND 
							 (tblServices.ValidityTo IS NULL))
	BEGIN
		SET @BaseCategory = 'S'
	END
	ELSE
	BEGIN
		IF  EXISTS (SELECT tblClaimServices.ClaimServiceID FROM tblClaim INNER JOIN tblClaimServices ON tblClaim.ClaimID = tblClaimServices.ClaimID INNER JOIN tblServices ON tblClaimServices.ServiceID = tblServices.ServiceID
		WHERE        (tblClaim.ClaimID = @ClaimID) AND (tblClaim.ValidityTo IS NULL) AND (tblClaimServices.ValidityTo IS NULL) AND (tblServices.ServCategory = 'D') AND 
							 (tblServices.ValidityTo IS NULL))
		BEGIN
			SET @BaseCategory = 'D'
		END
		ELSE
		BEGIN
			IF  EXISTS (SELECT tblClaimServices.ClaimServiceID FROM tblClaim INNER JOIN tblClaimServices ON tblClaim.ClaimID = tblClaimServices.ClaimID INNER JOIN tblServices ON tblClaimServices.ServiceID = tblServices.ServiceID
			WHERE        (tblClaim.ClaimID = @ClaimID) AND (tblClaim.ValidityTo IS NULL) AND (tblClaimServices.ValidityTo IS NULL) AND (tblServices.ServCategory = 'A') AND 
								 (tblServices.ValidityTo IS NULL))
			BEGIN
				SET @BaseCategory = 'A'
			END
			ELSE
			BEGIN
				
				
				IF ISNULL(@ClaimDateTo,@ClaimDateFrom) <> @ClaimDateFrom 
				BEGIN
					SET @BaseCategory = 'H'
				END
				ELSE
				BEGIN
					IF  EXISTS (SELECT tblClaimServices.ClaimServiceID FROM tblClaim INNER JOIN tblClaimServices ON tblClaim.ClaimID = tblClaimServices.ClaimID INNER JOIN tblServices ON tblClaimServices.ServiceID = tblServices.ServiceID
					WHERE        (tblClaim.ClaimID = @ClaimID) AND (tblClaim.ValidityTo IS NULL) AND (tblClaimServices.ValidityTo IS NULL) AND (tblServices.ServCategory = 'C') AND 
										 (tblServices.ValidityTo IS NULL))
					BEGIN
						SET @BaseCategory = 'C'
					END
					ELSE
					BEGIN
						SET @BaseCategory = 'V'
					END
				END
			END
		END
	END

	/*PREPARE HISTORIC TABLE WITh RELEVANT ITEMS AND SERVICES*/

	DECLARE @TargetDate as Date

	
	SELECT @TargetDate = ISNULL(TblClaim.DateTo,TblClaim.DateFrom) FROM TblClaim WHERE ClaimID = @ClaimID 

	DECLARE @FamilyID INT 
	SELECT @FamilyID = FamilyID from tblInsuree where InsureeID = @InsureeID 
	


	DECLARE  @DTBL_ITEMS TABLE (
							[ItemID] [int] NOT NULL,
							[ItemCode] [nvarchar](6) NOT NULL,
							[ItemType] [char](1) NOT NULL,
							[ItemPrice] [decimal](18, 2) NOT NULL,
							[ItemCareType] [char](1) NOT NULL,
							[ItemFrequency] [smallint] NULL,
							[ItemPatCat] [tinyint] NOT NULL
							)

	INSERT INTO @DTBL_ITEMS (ItemID , ItemCode, ItemType , ItemPrice, ItemCaretype ,ItemFrequency, ItemPatCat) 
	SELECT ItemID , ItemCode, ItemType , ItemPrice, ItemCaretype ,ItemFrequency, ItemPatCat FROM 
	(SELECT  ROW_NUMBER() OVER(PARTITION BY ItemId ORDER BY ValidityFrom DESC)RNo,AllItems.* FROM
	(
	SELECT Sub1.* FROM
	(
	SELECT ItemID , ItemCode, ItemType , ItemPrice, ItemCaretype ,ItemFrequency, ItemPatCat , ValidityFrom, ValidityTo, LegacyID from tblitems Where (ValidityTo IS NULL) OR ((NOT ValidityTo IS NULL) AND (LegacyID IS NULL))
	UNION ALL
	SELECT  LegacyID as ItemID , ItemCode, ItemType , ItemPrice, ItemCaretype ,ItemFrequency, ItemPatCat , ValidityFrom,ValidityTo, LegacyID  FROM tblItems Where  (NOT ValidityTo IS NULL) AND (NOT LegacyID IS NULL)
	
	) Sub1
	INNER JOIN 
	(
	SELECT        tblClaimItems.ItemID
	FROM            tblClaimItems 
	WHERE        (tblClaimItems.ValidityTo IS NULL) AND tblClaimItems.ClaimID = @ClaimID
	) Sub2 ON Sub1.ItemID = Sub2.ItemID 
	)  AllItems 
	WHERE CONVERT(date,ValidityFrom,103) <= @TargetDate 
	)Result
	WHERE Rno = 1 AND ((ValidityTo IS NULL) OR (NOT ValidityTo IS NULL AND NOT LegacyID IS NULL ))  	



	DECLARE  @DTBL_SERVICES TABLE (
							[ServiceID] [int] NOT NULL,
							[ServCode] [nvarchar](6) NOT NULL,
							[ServType] [char](1) NOT NULL,
							[ServLevel] [char](1) NOT NULL,
							[ServPrice] [decimal](18, 2) NOT NULL,
							[ServCareType] [char](1) NOT NULL,
							[ServFrequency] [smallint] NULL,
							[ServPatCat] [tinyint] NOT NULL,
							[ServCategory] [char](1) NULL
							)

	INSERT INTO @DTBL_SERVICES (ServiceID , ServCode, ServType , ServLevel, ServPrice, ServCaretype ,ServFrequency, ServPatCat, ServCategory ) 
	SELECT ServiceID , ServCode, ServType , ServLevel ,ServPrice, ServCaretype ,ServFrequency, ServPatCat,ServCategory FROM 
	(SELECT  ROW_NUMBER() OVER(PARTITION BY ServiceId ORDER BY ValidityFrom DESC)RNo,AllServices.* FROM
	(
	SELECT Sub1.* FROM
	(
	SELECT ServiceID , ServCode, ServType , ServLevel  ,ServPrice, ServCaretype ,ServFrequency, ServPatCat , ServCategory ,ValidityFrom, ValidityTo, LegacyID from tblServices WHere (ValidityTo IS NULL) OR ((NOT ValidityTo IS NULL) AND (LegacyID IS NULL))
	UNION ALL
	SELECT  LegacyID as ServiceID , ServCode, ServType , ServLevel  ,ServPrice, ServCaretype ,ServFrequency, ServPatCat , ServCategory , ValidityFrom, ValidityTo, LegacyID FROM tblServices Where  (NOT ValidityTo IS NULL) AND (NOT LegacyID IS NULL)
	) Sub1
	INNER JOIN 
	(
	SELECT        tblClaimServices.ServiceID 
	FROM            tblClaim INNER JOIN
							 tblClaimServices ON tblClaim.ClaimID = tblClaimServices.ClaimID
	WHERE        (tblClaimServices.ValidityTo IS NULL) AND tblClaim.ClaimID = @ClaimID
	) Sub2 ON Sub1.ServiceID = Sub2.ServiceID 
	)  AllServices 
	WHERE CONVERT(date,ValidityFrom,103) <= @TargetDate
	)Result
	WHERE Rno = 1 AND ((ValidityTo IS NULL) OR (NOT ValidityTo IS NULL AND NOT LegacyID IS NULL ))  
	
	DECLARE PRODUCTLOOP CURSOR LOCAL FORWARD_ONLY FOR	
													SELECT Policies.ProdID, Policies.PolicyID,	ISNULL(DedInsuree,0), ISNULL(DedOPInsuree,0), ISNULL(DedIPInsuree,0), ISNULL(MaxInsuree,0), ISNULL(MaxOPInsuree,0), 
																								ISNULL(MaxIPInsuree,0), ISNULL(DedTreatment,0), ISNULL(DedOPTreatment,0), ISNULL(DedIPTreatment,0), ISNULL(MaxIPTreatment,0), 
																								ISNULL(MaxTreatment,0), ISNULL(MaxOPTreatment,0), ISNULL(DedPolicy,0), ISNULL(DedOPPolicy,0), ISNULL(DedIPPolicy,0), 
																								ISNULL(MaxPolicy,0), ISNULL(MaxOPPolicy,0) , ISNULL(MaxIPPolicy,0),ISNULL(MaxAmountConsultation ,0),ISNULL(MaxAmountSurgery,0),ISNULL(MaxAmountHospitalization ,0),ISNULL(MaxAmountDelivery ,0), ISNULL(MaxAmountAntenatal  ,0),
																								ISNULL(Threshold,0), ISNULL(MaxPolicyExtraMember,0),ISNULL(MaxPolicyExtraMemberIP,0),ISNULL(MaxPolicyExtraMemberOP,0),ISNULL(MaxCeilingPolicy,0),ISNULL(MaxCeilingPolicyIP,0),ISNULL(MaxCeilingPolicyOP,0), ISNULL(CeilingInterpretation,'I')
																		  FROM 
													(
													SELECT     tblClaimItems.ProdID, tblClaimItems.PolicyID
													FROM         tblClaimItems INNER JOIN
																		  @DTBL_ITEMS Items ON tblClaimItems.ItemID = Items.ItemID
													WHERE     (tblClaimItems.ClaimID = @ClaimID) AND (tblClaimItems.ValidityTo IS NULL) AND (tblClaimItems.RejectionReason = 0)
																										
													UNION 
													SELECT     tblClaimServices.ProdID, tblClaimServices.PolicyID
													FROM         tblClaimServices INNER JOIN
																		  @DTBL_SERVICES Serv ON tblClaimServices.ServiceID = Serv.ServiceID
													WHERE     (tblClaimServices.ClaimID = @ClaimID) AND (tblClaimServices.ValidityTo IS NULL) AND (tblClaimServices.RejectionReason = 0)
													) Policies 
													INNER JOIN 
													(
													SELECT     ProdID, DedInsuree, DedOPInsuree, DedIPInsuree, MaxInsuree, MaxOPInsuree, MaxIPInsuree, DedTreatment, DedOPTreatment, DedIPTreatment, MaxIPTreatment, 
																MaxTreatment, MaxOPTreatment, DedPolicy, DedOPPolicy, DedIPPolicy, MaxPolicy, MaxOPPolicy, MaxIPPolicy, MaxAmountConsultation ,MaxAmountSurgery ,MaxAmountHospitalization ,MaxAmountDelivery , MaxAmountAntenatal,
																Threshold, MaxPolicyExtraMember , MaxPolicyExtraMemberIP , MaxPolicyExtraMemberOP, MaxCeilingPolicy, MaxCeilingPolicyIP ,MaxCeilingPolicyOP ,ValidityTo, CeilingInterpretation  FROM tblProduct
													WHERE     (ValidityTo IS NULL)
													) Product ON Product.ProdID = Policies.ProdID
													
	OPEN PRODUCTLOOP
	FETCH NEXT FROM PRODUCTLOOP INTO	@ProductID, @PolicyID,@DedInsuree,@DedOPInsuree,@DedIPInsuree,@MaxInsuree,@MaxOPInsuree,@MaxIPInsuree,@DedTreatment,@DedOPTreatment,@DedIPTreatment,
										@MaxIPTreatment,@MaxTreatment,@MaxOPTreatment,@DedPolicy,@DedOPPolicy,@DedIPPolicy,@MaxPolicy,@MaxOPPolicy,@MaxIPPolicy,@CeilingConsult,@CeilingSurgery,@CeilingHospitalization,@CeilingDelivery,@CeilingAntenatal,
										@Treshold, @MaxPolicyExtraMember,@MaxPolicyExtraMemberIP,@MaxPolicyExtraMemberOP,@MaxCeilingPolicy,@MaxCeilingPolicyIP,@MaxCeilingPolicyOP,@CeilingInterpretation
	
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		--FIRST CHECK GENERAL 
		
		--DECLARE @PrevDeducted as Decimal(18,2)
		--DECLARE @PrevRemunerated as decimal(18,2)
		--DECLARE @Deducted as decimal(18,2)
		
		SET @Ceiling = 0 
		SET @Deductable = 0 
		SET @Deducted = 0  --reset to zero 
		SET @Remunerated = 0 
		SET @RemuneratedConsult = 0 
		SET @RemuneratedDelivery = 0 
		SET @RemuneratedHospitalization = 0 
		SET @RemuneratedSurgery = 0 
		SET @RemuneratedAntenatal  = 0 

		SELECT @PolicyMembers =  COUNT(InsureeID) FROM tblInsureePolicy WHERE tblInsureePolicy.PolicyId = @PolicyID  AND  (NOT (EffectiveDate IS NULL)) AND  ( @ClaimDateTo BETWEEN EffectiveDate And ExpiryDate  )   AND   (ValidityTo IS NULL)

		IF ISNULL(@CeilingConsult,0) > 0 
		BEGIN
			SELECT @PrevRemuneratedConsult = 0 --SUM(RemConsult) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
		END
		IF ISNULL(@CeilingSurgery,0) > 0 
		BEGIN
			SELECT @PrevRemuneratedSurgery  = 0 -- SUM(RemSurgery ) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
		END
		IF ISNULL(@CeilingHospitalization,0)  > 0 
		BEGIN
			--check first if this is a hospital claim falling under the hospitalization category
			IF @Hospitalization = 1 

			--SELECT @ClaimDateFrom = DateFrom,  @ClaimDateTo = DateTo FROM tblClaim Where ClaimID = @ClaimID 
			--IF ISNULL(@ClaimDateTo,@ClaimDateFrom) <> @ClaimDateFrom 
			BEGIN
				--SET @Hospitalization = 1 
				SELECT @PrevRemuneratedHospitalization = 0 -- SUM(RemHospitalization) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
			END
		END

		IF ISNULL(@CeilingDelivery,0)  > 0 
		BEGIN
			SELECT @PrevRemuneratedDelivery  = 0 -- SUM(RemDelivery ) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
		END

		IF ISNULL(@PrevRemuneratedAntenatal ,0)  > 0 
		BEGIN
			SELECT @PrevRemuneratedAntenatal  = 0 --  SUM(RemAntenatal ) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
		END


		IF ISNULL(@DedTreatment,0) <> 0 
		BEGIN
			SET @Deductable = @DedTreatment
			SET @DeductableType = 'G'
			SET @PrevDeducted = 0 
		END
		
		IF ISNULL(@DedInsuree,0) <> 0
		BEGIN
			SET @Deductable = @DedInsuree
			SET @DeductableType = 'G'
			SELECT @PrevDeducted = SUM(DedG) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
		END
		
		IF ISNULL(@DedPolicy,0) <> 0
		BEGIN
			SET @Deductable = @DedPolicy
			SET @DeductableType = 'G'
			SELECT @PrevDeducted = SUM(DedG) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID And ClaimID <> @ClaimID 
		END
		
		IF ISNULL(@MaxTreatment,0) <> 0
		BEGIN
			SET @Ceiling = @MaxTreatment
			SET @CeilingType  = 'G'
			SET @PrevRemunerated = 0 
		END
		
		IF ISNULL(@MaxInsuree,0) <> 0
		BEGIN
			SET @Ceiling = @MaxInsuree
			SET @CeilingType  = 'G'
			SELECT @PrevRemunerated = SUM(RemG) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
		END
		IF ISNULL(@MaxPolicy,0) <> 0
		BEGIN
		    --check with the amount of members if we go over the treshold --> if so lets calculate 
			IF @PolicyMembers > @Treshold
			BEGIN
				SET @Ceiling = @MaxPolicy + ((@PolicyMembers - @Treshold) * @MaxPolicyExtraMember) 
				IF @Ceiling > @MaxCeilingPolicy
					SET @Ceiling = ISNULL(NULLIF(@MaxCeilingPolicy, 0), @Ceiling)
			END
			ELSE
			BEGIN
				SET @Ceiling = @MaxPolicy
			END

			SET @CeilingType  = 'G'
			SELECT @PrevRemunerated = SUM(RemG) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID And ClaimID <> @ClaimID  
		END
				
		--NOW CHECK FOR IP DEDUCTABLES --> if hospital
		IF @Deductable = 0 
		BEGIN 
			IF (@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' ) --@HFLevel = 'H' This was a claim with a hospital stay 
			BEGIN
				--Hospital IP
				IF @DedIPTreatment <> 0 
				BEGIN
					SET @Deductable = @DedIPTreatment
					SET @DeductableType = 'I'
					SET @PrevDeducted = 0 
				END
				
				IF @DedIPInsuree  <> 0
				BEGIN
					SET @Deductable = @DedIPInsuree
					SET @DeductableType = 'I'
					SELECT @PrevDeducted = SUM(DedIP) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
					
				END
				
				IF @DedIPPolicy <> 0
				BEGIN
					SET @Deductable = @DedIPPolicy
					SET @DeductableType = 'I'
					SELECT @PrevDeducted = SUM(DedIP) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID And ClaimID <> @ClaimID 
				END	
			END
			ELSE
			BEGIN
				--Non hospital OP
				--Hospital IP
				IF @DedOPTreatment <> 0 
				BEGIN
					SET @Deductable = @DedOPTreatment
					SET @DeductableType = 'O'
					SET @PrevDeducted = 0 
				END
				
				IF @DedIPInsuree  <> 0
				BEGIN
					SET @Deductable = @DedOPInsuree
					SET @DeductableType = 'O'
					SELECT @PrevDeducted = SUM(DedOP) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
					
				END
				
				IF @DedIPPolicy <> 0
				BEGIN
					SET @Deductable = @DedOPPolicy
					SET @DeductableType = 'O'
					SELECT @PrevDeducted = SUM(DedOP) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID And ClaimID <> @ClaimID 
				END	
			END
		END
		
		--NOW CHECK FOR IP CEILINGS --> if hospital
		IF @Ceiling = 0  
		BEGIN
		--- HANS HERE CHANGE DEPENDING ON NEW FIELD IN PRODUCT
			IF (@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' )
			BEGIN
				--Hospital IP
				IF @MaxIPTreatment <> 0 
				BEGIN
					SET @Ceiling  = @MaxIPTreatment
					SET @CeilingType = 'I'
					SET @PrevRemunerated = 0 
				END
				
				IF @MaxIPInsuree  <> 0
				BEGIN
					SET @Ceiling  = @MaxIPInsuree 
					SET @CeilingType = 'I'
					SELECT @PrevRemunerated = SUM(RemIP) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
					
				END
				
				IF @MaxIPPolicy <> 0
				BEGIN
					
					IF @PolicyMembers > @Treshold
					BEGIN
						SET @Ceiling = @MaxIPPolicy + ((@PolicyMembers - @Treshold) * @MaxPolicyExtraMemberIP ) 
						IF @Ceiling > @MaxCeilingPolicyIP 
							SET @Ceiling = ISNULL(NULLIF(@MaxCeilingPolicyIP, 0), @Ceiling)
					END
					ELSE
					BEGIN
						SET @Ceiling = @MaxIPPolicy 
					END
					SET @CeilingType = 'I'
					SELECT @PrevRemunerated = SUM(RemIP) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID And ClaimID <> @ClaimID 
				END	
			END
			ELSE
			BEGIN
				--Non hospital OP
				IF @MaxOPTreatment <> 0 
				BEGIN
					SET @Ceiling  = @MaxOPTreatment
					SET @CeilingType = 'O'
					SET @PrevRemunerated = 0 
				END
				
				IF @MaxOPInsuree  <> 0
				BEGIN
					SET @Ceiling  = @MaxOPInsuree 
					SET @CeilingType = 'O'
					SELECT @PrevRemunerated = SUM(RemOP) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID AND InsureeID = @InsureeID And ClaimID <> @ClaimID 
					
				END
				
				IF @MaxOPPolicy <> 0
				BEGIN
					IF @PolicyMembers > @Treshold
					BEGIN
						SET @Ceiling = @MaxOPPolicy + ((@PolicyMembers - @Treshold) * @MaxPolicyExtraMemberOP ) 
						IF @Ceiling > @MaxCeilingPolicyOP 
							SET @Ceiling = ISNULL(NULLIF(@MaxCeilingPolicyOP, 0), @Ceiling)
					END
					ELSE
					BEGIN
						SET @Ceiling = @MaxOPPolicy 
					END
					 
					SET @CeilingType = 'O'
					SELECT @PrevRemunerated = SUM(RemOP) FROM dbo.tblClaimDedRem WHERE PolicyID = @PolicyID And ClaimID <> @ClaimID 
				END	
			END
		END
		
		--Make sure that we have zero in case of NULL
		SET @PrevRemunerated = ISNULL(@PrevRemunerated,0)
		SET @PrevDeducted = ISNULL(@PrevDeducted,0)
		SET @PrevRemuneratedConsult = ISNULL(@PrevRemuneratedConsult,0)
		SET @PrevRemuneratedSurgery  = ISNULL(@PrevRemuneratedSurgery ,0)
		SET @PrevRemuneratedHospitalization  = ISNULL(@PrevRemuneratedHospitalization ,0)
		SET @PrevRemuneratedDelivery  = ISNULL(@PrevRemuneratedDelivery ,0)
		SET @PrevRemuneratedantenatal   = ISNULL(@PrevRemuneratedantenatal ,0)

		
		DECLARE @CeilingExclusionAdult NVARCHAR(1)
		DECLARE @CeilingExclusionChild NVARCHAR(1)
		

		--FIRST GET all items 
		DECLARE CLAIMITEMLOOP CURSOR LOCAL FORWARD_ONLY FOR 
															SELECT     tblClaimItems.ClaimItemID, tblClaimItems.QtyProvided, tblClaimItems.QtyApproved, tblClaimItems.PriceAsked, tblClaimItems.PriceApproved,  
																		ISNULL(tblPLItemsDetail.PriceOverule,Items.ItemPrice) as PLPrice, tblClaimItems.PriceOrigin, tblClaimItems.Limitation, tblClaimItems.LimitationValue, tblProductItems.CeilingExclusionAdult, tblProductItems.CeilingExclusionChild 
															FROM         tblPLItemsDetail INNER JOIN
																		  @DTBL_ITEMS Items ON tblPLItemsDetail.ItemID = Items.ItemID INNER JOIN
																		  tblClaimItems INNER JOIN
																		  tblClaim ON tblClaimItems.ClaimID = tblClaim.ClaimID INNER JOIN
																		  tblHF ON tblClaim.HFID = tblHF.HfID INNER JOIN
																		  tblPLItems ON tblHF.PLItemID = tblPLItems.PLItemID ON tblPLItemsDetail.PLItemID = tblPLItems.PLItemID AND Items.ItemID = tblClaimItems.ItemID
																		  INNER JOIN tblProductItems ON tblClaimItems.ItemID = tblProductItems.ItemID AND tblProductItems.ProdID = tblClaimItems.ProdID 
															WHERE     (tblClaimItems.ClaimID = @ClaimID) AND (tblClaimItems.ValidityTo IS NULL) AND (tblClaimItems.ClaimItemStatus = 1) AND (tblClaimItems.ProdID = @ProductID) AND 
																		  (tblClaimItems.PolicyID = @PolicyID) AND (tblPLItems.ValidityTo IS NULL) AND (tblPLItemsDetail.ValidityTo IS NULL) AND (tblProductItems.ValidityTo IS NULL)
															ORDER BY tblClaimItems.ClaimItemID
		OPEN CLAIMITEMLOOP
		FETCH NEXT FROM CLAIMITEMLOOP INTO @ClaimItemId, @QtyProvided, @QtyApproved ,@PriceAsked, @PriceApproved, @PLPrice, @PriceOrigin, @Limitation, @Limitationvalue,@CeilingExclusionAdult,@CeilingExclusionChild
		WHILE @@FETCH_STATUS = 0 
		BEGIN
			--SET @Deductable = @DedOPTreatment
			--SET @DeductableType = 'O'
			--SET @PrevDeducted = 0 
			
			--DeductableAmount
			--RemuneratedAmount
			--ExceedCeilingAmount
			--ProcessingStatus
			
			--CHECK first if any amount is still to be deducted 
			--SELECT @ClaimExclusionAdult = CeilingEx FROM tblProductItems WHERE ProdID = @ProductID AND ItemID = @ItemID AND ValidityTo IS NULL

			
			SET @ItemQty = ISNULL(@QtyApproved,@QtyProvided) 
			SET @WorkValue = 0 
			SET @SetPriceDeducted = 0 
			SET @ExceedCeilingAmount = 0 
			SET @ExceedCeilingAmountCategory = 0 

			IF @PriceOrigin = 'O' 
				SET @SetPriceAdjusted = ISNULL(@PriceApproved,@PriceAsked)
			ELSE
				--HVH check if this is the case
				SET @SetPriceAdjusted = ISNULL(@PriceApproved,@PLPrice)
			
			SET @WorkValue = (@ItemQty * @SetPriceAdjusted)
			
			IF @Limitation = 'F' AND ((@ItemQty * @Limitationvalue) < @WorkValue)
				SET @WorkValue =(@ItemQty * @Limitationvalue)


			IF @Deductable - @PrevDeducted - @Deducted > 0 
			BEGIN
				IF (@Deductable - @PrevDeducted - @Deducted) >= ( @WorkValue)
				BEGIN
					SET @SetPriceDeducted = (@WorkValue)
					SET @Deducted = @Deducted + ( @WorkValue)
					SET @Remunerated = @Remunerated + 0 
					SET @SetPriceValuated = 0 
					SET @SetPriceRemunerated = 0 
					GOTO NextItem
				END
				ELSE
				BEGIN
					--partial coverage 
					SET @SetPriceDeducted = (@Deductable - @PrevDeducted - @Deducted)
					SET @WorkValue = (@WorkValue) - @SetPriceDeducted
					SET @Deducted = @Deducted + (@Deductable - @PrevDeducted - @Deducted)
					
					--go next stage --> valuation considering the ceilings 
				END
			END
			
			--DEDUCTABLES ARE ALREADY TAKEN OUT OF VALUE AND STORED IN VARS
			
			--IF @Limitation = 'F' AND ((@ItemQty * @Limitationvalue) < @WorkValue)
				--SET @WorkValue =(@ItemQty * @Limitationvalue)
			
			IF @Limitation = 'C' 
				SET @WorkValue = (@Limitationvalue/100) * @WorkValue  
				
			
			IF @BaseCategory <> 'V'
			BEGIN
				IF (ISNULL(@CeilingSurgery  ,0) > 0) AND @BaseCategory = 'S'  --  Ceiling check for Surgery
				BEGIN
					IF @WorkValue + @PrevRemuneratedSurgery  + @RemuneratedSurgery   <= @CeilingSurgery  
					BEGIN
						--we are still under the ceiling for hospitalization and can be fully covered 
						SET @RemuneratedSurgery   =  @RemuneratedSurgery   + @WorkValue
					END
					ELSE
					BEGIN
						IF @PrevRemuneratedSurgery  + @RemuneratedSurgery  >= @CeilingSurgery 
						BEGIN
							--Nothing can be covered already reached ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue
							SET @RemuneratedSurgery  = @RemuneratedSurgery    + 0
							SET @WorkValue = 0 
						END
						ELSE
						BEGIN
							--claim service can partially be covered , we are over the ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue + @PrevRemuneratedSurgery   + @RemuneratedSurgery    - @CeilingSurgery   
							SET @WorkValue = @WorkValue - @ExceedCeilingAmountCategory
							SET @RemuneratedSurgery    =  @RemuneratedSurgery    + @WorkValue   -- we only add the value that could be covered up to the ceiling
						END
					END
				END

				IF (ISNULL(@CeilingDelivery  ,0) > 0) AND @BaseCategory = 'D'  --  Ceiling check for Delivery
				BEGIN
					IF @WorkValue + @PrevRemuneratedDelivery  + @RemuneratedDelivery   <= @CeilingDelivery  
					BEGIN
						--we are still under the ceiling for hospitalization and can be fully covered 
						SET @RemuneratedDelivery   =  @RemuneratedDelivery   + @WorkValue
					END
					ELSE
					BEGIN
						IF @PrevRemuneratedDelivery  + @RemuneratedDelivery  >= @CeilingDelivery 
						BEGIN
							--Nothing can be covered already reached ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue
							SET @RemuneratedDelivery  = @RemuneratedDelivery    + 0
							SET @WorkValue = 0 
						END
						ELSE
						BEGIN
							--claim service can partially be covered , we are over the ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue + @PrevRemuneratedDelivery   + @RemuneratedDelivery    - @CeilingDelivery   
							SET @WorkValue = @WorkValue - @ExceedCeilingAmountCategory
							SET @RemuneratedDelivery    =  @RemuneratedDelivery    + @WorkValue   -- we only add the value that could be covered up to the ceiling
						END
					END
				END
				
				IF (ISNULL(@CeilingAntenatal  ,0) > 0) AND @BaseCategory = 'A'  --  Ceiling check for Antenatal
				BEGIN
					IF @WorkValue + @PrevRemuneratedAntenatal  + @RemuneratedAntenatal   <= @CeilingAntenatal  
					BEGIN
						--we are still under the ceiling for hospitalization and can be fully covered 
						SET @RemuneratedAntenatal   =  @RemuneratedAntenatal   + @WorkValue
					END
					ELSE
					BEGIN
						IF @PrevRemuneratedAntenatal  + @RemuneratedAntenatal  >= @CeilingAntenatal 
						BEGIN
							--Nothing can be covered already reached ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue
							SET @RemuneratedAntenatal  = @RemuneratedAntenatal    + 0
							SET @WorkValue = 0 
						END
						ELSE
						BEGIN
							--claim service can partially be covered , we are over the ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue + @PrevRemuneratedAntenatal   + @RemuneratedAntenatal    - @CeilingAntenatal   
							SET @WorkValue = @WorkValue - @ExceedCeilingAmountCategory
							SET @RemuneratedAntenatal    =  @RemuneratedAntenatal    + @WorkValue   -- we only add the value that could be covered up to the ceiling
						END
					END
				END

				IF (ISNULL(@CeilingHospitalization ,0) > 0) AND @BaseCategory = 'H'  --  Ceiling check for Hospital
				BEGIN
					IF @WorkValue + @PrevRemuneratedHospitalization + @RemuneratedHospitalization  <= @CeilingHospitalization 
					BEGIN
						--we are still under the ceiling for hospitalization and can be fully covered 
						SET @RemuneratedHospitalization  =  @RemuneratedHospitalization  + @WorkValue
					END
					ELSE
					BEGIN
						IF @PrevRemuneratedHospitalization  + @RemuneratedHospitalization  >= @CeilingHospitalization 
						BEGIN
							--Nothing can be covered already reached ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue
							SET @RemuneratedHospitalization  = @RemuneratedHospitalization    + 0
							SET @WorkValue = 0 
						END
						ELSE
						BEGIN
							--claim service can partially be covered , we are over the ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue + @PrevRemuneratedHospitalization   + @RemuneratedHospitalization    - @CeilingHospitalization   
							SET @WorkValue = @WorkValue - @ExceedCeilingAmountCategory
							SET @RemuneratedHospitalization    =  @RemuneratedHospitalization    + @WorkValue   -- we only add the value that could be covered up to the ceiling
						END
					END
				END

				IF (ISNULL(@CeilingConsult   ,0) > 0) AND @BaseCategory = 'C'  --  Ceiling check for Consult
				BEGIN
					IF @WorkValue + @PrevRemuneratedConsult  + @RemuneratedConsult   <= @CeilingConsult  
					BEGIN
						--we are still under the ceiling for hospitalization and can be fully covered 
						SET @RemuneratedConsult   =  @RemuneratedConsult   + @WorkValue
					END
					ELSE
					BEGIN
						IF @PrevRemuneratedConsult  + @RemuneratedConsult  >= @CeilingConsult 
						BEGIN
							--Nothing can be covered already reached ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue
							SET @RemuneratedConsult  = @RemuneratedConsult    + 0
							SET @WorkValue = 0 
						END
						ELSE
						BEGIN
							--claim service can partially be covered , we are over the ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue + @PrevRemuneratedConsult   + @RemuneratedConsult    - @CeilingConsult   
							SET @WorkValue = @WorkValue - @ExceedCeilingAmountCategory
							SET @RemuneratedConsult    =  @RemuneratedConsult    + @WorkValue   -- we only add the value that could be covered up to the ceiling
						END
					END
				END

			END 

		
			IF (@AdultChild = 'A' AND (((@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' ))) AND (@CeilingExclusionAdult = 'B' OR @CeilingExclusionAdult = 'H'))  OR
			   (@AdultChild = 'A' AND (NOT ((@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' ))) AND (@CeilingExclusionAdult = 'B' OR @CeilingExclusionAdult = 'N')) OR
			   (@AdultChild = 'C' AND (((@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' ))) AND (@CeilingExclusionChild = 'B' OR @CeilingExclusionChild  = 'H')) OR
			   (@AdultChild = 'C' AND (NOT ((@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' ))) AND (@CeilingExclusionChild = 'B' OR @CeilingExclusionChild  = 'N')) 
			BEGIN
				--NO CEILING WILL BE AFFECTED
				SET @ExceedCeilingAmount = 0
				SET @Remunerated = @Remunerated + 0 --here in this case we do notr add the amount to be added to the ceiling --> so exclude from the actual value to be entered against the insert into tblClaimDedRem in the end of the prod loop 
				SET @SetPriceValuated = @WorkValue
				SET @SetPriceRemunerated = @WorkValue
				GOTO NextItem
			END
			ELSE
			BEGIN
				IF @Ceiling > 0 --CEILING HAS BEEN DEFINED 
				BEGIN	
					IF (@Ceiling - @PrevRemunerated  - @Remunerated)  > 0
					BEGIN
						--we have not reached the ceiling
						IF (@Ceiling - @PrevRemunerated  - @Remunerated) >= @WorkValue
						BEGIN
							--full amount of workvalue can be paid out as it under the limit
							SET @ExceedCeilingAmount = 0
							SET @SetPriceValuated = @WorkValue
							SET @SetPriceRemunerated = @WorkValue
							SET @Remunerated = @Remunerated + @WorkValue
							GOTO NextItem
						END
						ELSE
						BEGIN
							SET @ExceedCeilingAmount = @WorkValue - (@Ceiling - @PrevRemunerated  - @Remunerated)			
							SET @SetPriceValuated = (@Ceiling - @PrevRemunerated  - @Remunerated)
							SET @SetPriceRemunerated = (@Ceiling - @PrevRemunerated  - @Remunerated)
							SET @Remunerated = @Remunerated + (@Ceiling - @PrevRemunerated  - @Remunerated)			
							GOTO NextItem
						END
					
					END
					ELSE
					BEGIN
						SET @ExceedCeilingAmount = @WorkValue
						SET @Remunerated = @Remunerated + 0
						SET @SetPriceValuated = 0
						SET @SetPriceRemunerated = 0
						GOTO NextItem
					END
				END
				ELSE
				BEGIN
					-->
					SET @ExceedCeilingAmount = 0
					SET @Remunerated = @Remunerated + @WorkValue
					SET @SetPriceValuated = @WorkValue
					SET @SetPriceRemunerated = @WorkValue
					GOTO NextItem
				END

			END
	
			
NextItem:
			IF @IsProcess = 1 
			BEGIN
				IF @PriceOrigin = 'R'
				BEGIN
					UPDATE tblClaimItems SET PriceAdjusted = @SetPriceAdjusted , PriceValuated = @SetPriceValuated , DeductableAmount = @SetPriceDeducted , ExceedCeilingAmount = @ExceedCeilingAmount , @ExceedCeilingAmountCategory  = @ExceedCeilingAmountCategory WHERE ClaimItemID = @ClaimItemID 
					SET @RelativePrices = 1 
				END
				ELSE
				BEGIN
					UPDATE tblClaimItems SET PriceAdjusted = @SetPriceAdjusted , PriceValuated = @SetPriceValuated , DeductableAmount = @SetPriceDeducted ,ExceedCeilingAmount = @ExceedCeilingAmount,  @ExceedCeilingAmountCategory  = @ExceedCeilingAmountCategory, RemuneratedAmount = @SetPriceRemunerated WHERE ClaimItemID = @ClaimItemID 
				END
			END
			
			FETCH NEXT FROM CLAIMITEMLOOP INTO @ClaimItemId, @QtyProvided, @QtyApproved ,@PriceAsked, @PriceApproved, @PLPrice, @PriceOrigin, @Limitation, @Limitationvalue,@CeilingExclusionAdult,@CeilingExclusionChild
		END
		CLOSE CLAIMITEMLOOP
		DEALLOCATE CLAIMITEMLOOP 
			
		-- !!!!!! SECONDLY GET all SERVICES !!!!!!!
			
		DECLARE CLAIMSERVICELOOP CURSOR LOCAL FORWARD_ONLY FOR 
															SELECT     tblClaimServices.ClaimServiceID, tblClaimServices.QtyProvided, tblClaimServices.QtyApproved, tblClaimServices.PriceAsked, tblClaimServices.PriceApproved,  
																		ISNULL(tblPLServicesDetail.PriceOverule,Serv.ServPrice) as PLPrice, tblClaimServices.PriceOrigin, tblClaimServices.Limitation, tblClaimServices.LimitationValue, Serv.ServCategory , tblProductServices.CeilingExclusionAdult, tblProductServices.CeilingExclusionChild 
															FROM         tblPLServicesDetail INNER JOIN
																		  @DTBL_Services Serv ON tblPLServicesDetail.ServiceID = Serv.ServiceID INNER JOIN
																		  tblClaimServices INNER JOIN
																		  tblClaim ON tblClaimServices.ClaimID = tblClaim.ClaimID INNER JOIN
																		  tblHF ON tblClaim.HFID = tblHF.HfID INNER JOIN
																		  tblPLServices ON tblHF.PLServiceID = tblPLServices.PLServiceID ON tblPLServicesDetail.PLServiceID = tblPLServices.PLServiceID AND Serv.ServiceID = tblClaimServices.ServiceID
																		  INNER JOIN tblProductServices ON tblClaimServices.ServiceID  = tblProductServices.ServiceID  AND tblProductServices.ProdID = tblClaimServices.ProdID 
															WHERE     (tblClaimServices.ClaimID = @ClaimID) AND (tblClaimServices.ValidityTo IS NULL) AND (tblClaimServices.ClaimServiceStatus = 1) AND (tblClaimServices.ProdID = @ProductID) AND 
																		  (tblClaimServices.PolicyID = @PolicyID) AND (tblPLServices.ValidityTo IS NULL) AND (tblPLServicesDetail.ValidityTo IS NULL)  AND (tblProductServices.ValidityTo IS NULL)
															ORDER BY tblClaimServices.ClaimServiceID
		OPEN CLAIMSERVICELOOP
		FETCH NEXT FROM CLAIMSERVICELOOP INTO @ClaimServiceId, @QtyProvided, @QtyApproved ,@PriceAsked, @PriceApproved, @PLPrice, @PriceOrigin, @Limitation, @Limitationvalue,@ServCategory,@CeilingExclusionAdult,@CeilingExclusionChild
		WHILE @@FETCH_STATUS = 0 
		BEGIN
			--SET @Deductable = @DedOPTreatment
			--SET @DeductableType = 'O'
			--SET @PrevDeducted = 0 
			
			--DeductableAmount
			--RemuneratedAmount
			--ExceedCeilingAmount
			--ProcessingStatus
			
			--CHECK first if any amount is still to be deducted 
			SET @ServiceQty = ISNULL(@QtyApproved,@QtyProvided) 
			SET @WorkValue = 0 
			SET @SetPriceDeducted = 0 
			SET @ExceedCeilingAmount = 0 
			SET @ExceedCeilingAmountCategory = 0 
			


			IF @PriceOrigin = 'O' 
				SET @SetPriceAdjusted = ISNULL(@PriceApproved,@PriceAsked)
			ELSE
				--HVH check if this is the case
				SET @SetPriceAdjusted = ISNULL(@PriceApproved,@PLPrice)
			
			--FIRST GET THE NORMAL PRICING 
			SET @WorkValue = (@ServiceQty * @SetPriceAdjusted)
			

			IF @Limitation = 'F' AND ((@ServiceQty * @Limitationvalue) < @WorkValue)
				SET @WorkValue =(@ServiceQty * @Limitationvalue)

           

			IF @Deductable - @PrevDeducted - @Deducted > 0 
			BEGIN
				IF (@Deductable - @PrevDeducted - @Deducted) >= (@WorkValue)
				BEGIN
					SET @SetPriceDeducted = ( @WorkValue)
					SET @Deducted = @Deducted + ( @WorkValue)
					SET @Remunerated = @Remunerated + 0 
					SET @SetPriceValuated = 0 
					SET @SetPriceRemunerated = 0 
					GOTO NextService
				END
				ELSE
				BEGIN
					--partial coverage 
					SET @SetPriceDeducted = (@Deductable - @PrevDeducted - @Deducted)
					SET @WorkValue = (@WorkValue) - @SetPriceDeducted
					SET @Deducted = @Deducted + (@Deductable - @PrevDeducted - @Deducted)
					
					--go next stage --> valuation considering the ceilings 
				END
			END
			
			--DEDUCTABLES ARE ALREADY TAKEN OUT OF VALUE AND STORED IN VARS
			
			--IF @Limitation = 'F' AND ((@ServiceQty * @Limitationvalue) < @WorkValue)
				--SET @WorkValue =(@ServiceQty * @Limitationvalue)
			
			IF @Limitation = 'C' 
				SET @WorkValue = (@Limitationvalue/100) * @WorkValue  
				
			
			--now capping in case of category constraints
			
			IF @BaseCategory <> 'V'
			BEGIN
				IF @BaseCategory = 'S' AND (ISNULL(@CeilingSurgery ,0) > 0)  --  Ceiling check for category Surgery
				BEGIN
					IF @WorkValue + @PrevRemuneratedSurgery + @RemuneratedSurgery   <= @CeilingSurgery
					BEGIN
						--we are still under the ceiling for surgery and can be fully covered 
						SET @RemuneratedSurgery =  @RemuneratedSurgery + @WorkValue
					END
					ELSE
					BEGIN
						IF @PrevRemuneratedSurgery + @RemuneratedSurgery >= @CeilingSurgery 
						BEGIN
							--Nothing can be covered already reached ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue
							SET @RemuneratedSurgery  = @RemuneratedSurgery  + 0
							SET @WorkValue = 0 
						END
						ELSE
						BEGIN
							--claim service can partially be covered , we are over the ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue + @PrevRemuneratedSurgery  + @RemuneratedSurgery  - @CeilingSurgery 
							SET @WorkValue = @WorkValue - @ExceedCeilingAmountCategory
							SET @RemuneratedSurgery  =  @RemuneratedSurgery  + @WorkValue   -- we only add the value that could be covered up to the ceiling
						END
					END
				END

				IF @BaseCategory = 'D' AND (ISNULL(@CeilingDelivery ,0) > 0)  --  Ceiling check for category Deliveries 
				BEGIN
					IF @WorkValue + @PrevRemuneratedDelivery  + @RemuneratedDelivery    <= @CeilingDelivery 
					BEGIN
						--we are still under the ceiling for Delivery and can be fully covered 
						SET @RemuneratedDelivery  =  @RemuneratedDelivery  + @WorkValue
					END
					ELSE
					BEGIN
						IF @PrevRemuneratedDelivery  + @RemuneratedDelivery  >= @CeilingDelivery 
						BEGIN
							--Nothing can be covered already reached ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue
							SET @RemuneratedDelivery  = @RemuneratedDelivery   + 0
							SET @WorkValue = 0 
						END
						ELSE
						BEGIN
							--claim service can partially be covered , we are over the ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue + @PrevRemuneratedDelivery   + @RemuneratedDelivery   - @CeilingDelivery  
							SET @WorkValue = @WorkValue - @ExceedCeilingAmountCategory
							SET @RemuneratedDelivery   =  @RemuneratedDelivery   + @WorkValue   -- we only add the value that could be covered up to the ceiling
						END
					END
				END
				
				IF @BaseCategory = 'A' AND (ISNULL(@CeilingAntenatal  ,0) > 0)  --  Ceiling check for category Antenatal 
				BEGIN
					IF @WorkValue + @PrevRemuneratedAntenatal  + @RemuneratedAntenatal    <= @CeilingAntenatal 
					BEGIN
						--we are still under the ceiling for Antenatal and can be fully covered 
						SET @RemuneratedAntenatal  =  @RemuneratedAntenatal  + @WorkValue
					END
					ELSE
					BEGIN
						IF @PrevRemuneratedAntenatal  + @RemuneratedAntenatal  >= @CeilingAntenatal 
						BEGIN
							--Nothing can be covered already reached ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue
							SET @RemuneratedAntenatal  = @RemuneratedAntenatal   + 0
							SET @WorkValue = 0 
						END
						ELSE
						BEGIN
							--claim service can partially be covered , we are over the ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue + @PrevRemuneratedAntenatal   + @RemuneratedAntenatal   - @CeilingAntenatal  
							SET @WorkValue = @WorkValue - @ExceedCeilingAmountCategory
							SET @RemuneratedAntenatal   =  @RemuneratedAntenatal   + @WorkValue   -- we only add the value that could be covered up to the ceiling
						END
					END
				END

				IF  @BaseCategory  = 'H' AND (ISNULL(@CeilingHospitalization ,0) > 0)   --  Ceiling check for category Hospitalization 
				BEGIN
					IF @WorkValue + @PrevRemuneratedHospitalization + @RemuneratedHospitalization  <= @CeilingHospitalization 
					BEGIN
						--we are still under the ceiling for hospitalization and can be fully covered 
						SET @RemuneratedHospitalization  =  @RemuneratedHospitalization  + @WorkValue
					END
					ELSE
					BEGIN
						IF @PrevRemuneratedHospitalization  + @RemuneratedHospitalization  >= @CeilingHospitalization 
						BEGIN
							--Nothing can be covered already reached ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue
							SET @RemuneratedHospitalization  = @RemuneratedHospitalization    + 0
							SET @WorkValue = 0 
						END
						ELSE
						BEGIN
							--claim service can partially be covered , we are over the ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue + @PrevRemuneratedHospitalization   + @RemuneratedHospitalization    - @CeilingHospitalization   
							SET @WorkValue = @WorkValue - @ExceedCeilingAmountCategory
							SET @RemuneratedHospitalization    =  @RemuneratedHospitalization    + @WorkValue   -- we only add the value that could be covered up to the ceiling
						END
					END
				END

				IF @BaseCategory  = 'C' AND (ISNULL(@CeilingConsult,0) > 0)  --  Ceiling check for category Consult 
				BEGIN
					IF @WorkValue + @PrevRemuneratedConsult + @RemuneratedConsult  <= @CeilingConsult 
					BEGIN
						--we are still under the ceiling for consult and can be fully covered 
						SET @RemuneratedConsult =  @RemuneratedConsult + @WorkValue
					END
					ELSE
					BEGIN
						IF @PrevRemuneratedConsult + @RemuneratedConsult >= @CeilingConsult
						BEGIN
							--Nothing can be covered already reached ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue
							SET @RemuneratedConsult  = @RemuneratedConsult + 0
							SET @WorkValue = 0 
						END
						ELSE
						BEGIN
							--claim service can partially be covered , we are over the ceiling
							SET @ExceedCeilingAmountCategory = @WorkValue + @PrevRemuneratedConsult + @RemuneratedConsult - @CeilingConsult
							SET @WorkValue = @WorkValue - @ExceedCeilingAmountCategory
							SET @RemuneratedConsult =  @RemuneratedConsult + @WorkValue   -- we only add the value that could be covered up to the ceiling
						END
					END
 				END


			END

			IF (@AdultChild = 'A' AND (((@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' ))) AND (@CeilingExclusionAdult = 'B' OR @CeilingExclusionAdult = 'H'))  OR
			   (@AdultChild = 'A' AND (NOT ((@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' ))) AND (@CeilingExclusionAdult = 'B' OR @CeilingExclusionAdult = 'N')) OR
			   (@AdultChild = 'C' AND (((@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' ))) AND (@CeilingExclusionChild = 'B' OR @CeilingExclusionChild  = 'H')) OR
			   (@AdultChild = 'C' AND (NOT ((@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' ))) AND (@CeilingExclusionChild = 'B' OR @CeilingExclusionChild  = 'N')) 
			BEGIN
				--NO CEILING WILL BE AFFECTED
				SET @ExceedCeilingAmount = 0
				SET @Remunerated = @Remunerated + 0  --(we do not add any value to the running sum for renumerated values as we do not coulnt this service for any ceiling calculation 
				SET @SetPriceValuated = @WorkValue
				SET @SetPriceRemunerated = @WorkValue
				GOTO NextService
				
			END
			ELSE
			BEGIN
				IF @Ceiling > 0 --CEILING HAS BEEN DEFINED 
				BEGIN	
					IF (@Ceiling - @PrevRemunerated  - @Remunerated)  > 0
					BEGIN
						--we have not reached the ceiling
						IF (@Ceiling - @PrevRemunerated  - @Remunerated) >= @WorkValue
						BEGIN
							--full amount of workvalue can be paid out as it under the limit
							SET @ExceedCeilingAmount = 0
							SET @SetPriceValuated = @WorkValue
							SET @SetPriceRemunerated = @WorkValue
							SET @Remunerated = @Remunerated + @WorkValue
							GOTO NextService
						END
						ELSE
						BEGIN
							SET @ExceedCeilingAmount = @WorkValue - (@Ceiling - @PrevRemunerated  - @Remunerated)			
							SET @SetPriceValuated = (@Ceiling - @PrevRemunerated  - @Remunerated)
							SET @SetPriceRemunerated = (@Ceiling - @PrevRemunerated  - @Remunerated)
							SET @Remunerated = @Remunerated + (@Ceiling - @PrevRemunerated  - @Remunerated)			
							GOTO NextService
						END
					
					END
					ELSE
					BEGIN
						SET @ExceedCeilingAmount = @WorkValue
						SET @Remunerated = @Remunerated + 0
						SET @SetPriceValuated = 0
						SET @SetPriceRemunerated = 0
						GOTO NextService
					END
				END
				ELSE
				BEGIN
					-->
					SET @ExceedCeilingAmount = 0
					SET @Remunerated = @Remunerated + @WorkValue
					SET @SetPriceValuated = @WorkValue
					SET @SetPriceRemunerated = @WorkValue
					GOTO NextService
				END

			END

NextService:
			IF @IsProcess = 1 
			BEGIN
				IF @PriceOrigin = 'R'
				BEGIN
					UPDATE tblClaimServices SET PriceAdjusted = @SetPriceAdjusted , PriceValuated = @SetPriceValuated , DeductableAmount = @SetPriceDeducted , ExceedCeilingAmount = @ExceedCeilingAmount , @ExceedCeilingAmountCategory  = @ExceedCeilingAmountCategory  WHERE ClaimServiceID = @ClaimServiceID 
					SET @RelativePrices = 1 
				END
				ELSE
				BEGIN
					UPDATE tblClaimServices SET PriceAdjusted = @SetPriceAdjusted , PriceValuated = @SetPriceValuated , DeductableAmount = @SetPriceDeducted ,ExceedCeilingAmount = @ExceedCeilingAmount, @ExceedCeilingAmountCategory  = @ExceedCeilingAmountCategory, RemuneratedAmount = @SetPriceRemunerated WHERE ClaimServiceID = @ClaimServiceID 
				END
			END
			
			FETCH NEXT FROM CLAIMSERVICELOOP INTO @ClaimServiceId, @QtyProvided, @QtyApproved ,@PriceAsked, @PriceApproved, @PLPrice, @PriceOrigin, @Limitation, @Limitationvalue,@ServCategory,@CeilingExclusionAdult,@CeilingExclusionChild
		END
		CLOSE CLAIMSERVICELOOP
		DEALLOCATE CLAIMSERVICELOOP 
		
		
		FETCH NEXT FROM PRODUCTLOOP INTO	@ProductID, @PolicyID,@DedInsuree,@DedOPInsuree,@DedIPInsuree,@MaxInsuree,@MaxOPInsuree,@MaxIPInsuree,@DedTreatment,@DedOPTreatment,@DedIPTreatment,
											@MaxIPTreatment,@MaxTreatment,@MaxOPTreatment,@DedPolicy,@DedOPPolicy,@DedIPPolicy,@MaxPolicy,@MaxOPPolicy,@MaxIPPolicy,@CeilingConsult,@CeilingSurgery,@CeilingHospitalization,@CeilingDelivery,@CeilingAntenatal,
											@Treshold, @MaxPolicyExtraMember,@MaxPolicyExtraMemberIP,@MaxPolicyExtraMemberOP,@MaxCeilingPolicy,@MaxCeilingPolicyIP,@MaxCeilingPolicyOP,@CeilingInterpretation
	
	END
	CLOSE PRODUCTLOOP
	DEALLOCATE PRODUCTLOOP 
	
	--Now insert the total renumerations and deductions on this claim 
	
	If @IsProcess = 1 
	BEGIN
		--delete first the policy entry in the table tblClaimDedRem as it was a temporary booking
		DELETE FROM tblClaimDedRem WHERE ClaimID = @ClaimID -- AND PolicyID = @PolicyID AND InsureeID = @InsureeID 
	END

	IF (@CeilingInterpretation = 'I' AND  @Hospitalization = 1) OR (@CeilingInterpretation = 'H' AND @HFLevel = 'H' )
	BEGIN 
		INSERT INTO tblClaimDedRem ([PolicyID],[InsureeID],[ClaimID],[DedG],[RemG],[DedIP],[RemIP],[RemConsult],[RemSurgery] ,[RemHospitalization] ,[RemDelivery] , [RemAntenatal] , [AuditUserID]) VALUES (@PolicyID,@InsureeID , @ClaimID , @Deducted ,@Remunerated ,@Deducted ,@Remunerated , @RemuneratedConsult  , @RemuneratedSurgery  ,@RemuneratedHospitalization , @RemuneratedDelivery  , @RemuneratedAntenatal,@AuditUser) 
	END
	ELSE
	BEGIN 
		INSERT INTO tblClaimDedRem ([PolicyID],[InsureeID],[ClaimID],[DedG],[RemG],[DedOP],[RemOP], [RemConsult],[RemSurgery] ,[RemHospitalization] ,[RemDelivery], [RemAntenatal] ,  [AuditUserID]) VALUES (@PolicyID,@InsureeID , @ClaimID , @Deducted ,@Remunerated ,@Deducted ,@Remunerated , @RemuneratedConsult  , @RemuneratedSurgery  ,@RemuneratedHospitalization , @RemuneratedDelivery , @RemuneratedAntenatal ,@AuditUser) 
	END
	
	If @IsProcess = 1 
	BEGIN
		IF @RelativePrices = 0
		BEGIN
			--update claim in total and set to Valuated
			UPDATE tblClaim SET ClaimStatus = 16, AuditUserIDProcess = @AuditUser, ProcessStamp = GETDATE(), DateProcessed = GETDATE() WHERE ClaimID = @ClaimID 
			SET @RtnStatus = 4
		END
		ELSE
		BEGIN
			--update claim in total and set to Processed --> awaiting one or more Services for relative prices
			UPDATE tblClaim SET ClaimStatus = 8, AuditUserIDProcess = @AuditUser, ProcessStamp = GETDATE(), DateProcessed = GETDATE() WHERE ClaimID = @ClaimID 
			SET @RtnStatus = 3
		END  
	
		UPDATE tblClaim SET FeedbackStatus = 16 WHERE ClaimID = @ClaimID AND FeedbackStatus = 4 
		UPDATE tblClaim SET ReviewStatus = 16 WHERE ClaimID = @ClaimID AND ReviewStatus = 4 
	END


	
FINISH:
	RETURN @oReturnValue
	
	END TRY
	
	BEGIN CATCH
		SELECT 'Unexpected error encountered'
		SET @oReturnValue = 1 
		RETURN @oReturnValue
		
	END CATCH
END










GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[uspReceiveControlNumber]
(
	@PaymentID INT,
	@ControlNumber NVARCHAR(50),
	@ResponseOrigin NVARCHAR(50) = NULL,
	@Failed BIT = 0
)
AS
	BEGIN
		BEGIN TRY
			IF EXISTS(SELECT 1 FROM tblControlNumber  WHERE PaymentID = @PaymentID AND ValidityTo IS NULL )
			BEGIN
				IF @Failed = 0
				BEGIN
					UPDATE tblPayment SET PaymentStatus = 3 WHERE PaymentID = @PaymentID AND ValidityTo IS NULL
					UPDATE tblControlNumber SET ReceivedDate = GETDATE(), ResponseOrigin = @ResponseOrigin,  ValidityFrom = GETDATE() ,AuditedUserID =-1,ControlNumber = @ControlNumber  WHERE PaymentID = @PaymentID AND ValidityTo IS NULL
					RETURN 0 
				END
				ELSE
				BEGIN
					UPDATE tblPayment SET PaymentStatus = -3, RejectedReason ='8: Duplicated control number assigned' WHERE PaymentID = @PaymentID AND ValidityTo IS NULL
					RETURN 2
				END
			END
			ELSE
			BEGIN
				RETURN 1
			END


				
		END TRY
		BEGIN CATCH
			RETURN -1
		END CATCH

	
	END




GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[uspAddInsureePolicy]
(
	@InsureeId INT,
	@Activate BIT = 0
)
AS
BEGIN
	DECLARE @FamilyId INT,
			@PolicyId INT,
			@NewPolicyValue DECIMAL(18,2),
			@EffectiveDate DATE,
			@PolicyValue DECIMAL(18,2),
			@PolicyStage NVARCHAR(1),
			@ProdId INT,
			@AuditUserId INT,
			@isOffline BIT,
			@ErrorCode INT,
			@TotalInsurees INT,
			@MaxMember INT,
			@ThresholdMember INT

	SELECT @FamilyId = FamilyID,@AuditUserId = AuditUserID FROM tblInsuree WHERE InsureeID = @InsureeId
	SELECT @TotalInsurees = COUNT(InsureeId) FROM tblInsuree WHERE FamilyId = @FamilyId AND ValidityTo IS NULL 
	SELECT @isOffline = ISNULL(OfflineCHF,0)  FROM tblIMISDefaults
	
	DECLARE @Premium decimal(18,2) = 0
	
	DECLARE Cur CURSOR FOR SELECT PolicyId,PolicyValue,EffectiveDate,PolicyStage,ProdID FROM tblPolicy WHERE FamilyID  = @FamilyId AND ValidityTo IS NULL
	OPEN Cur
	FETCH NEXT FROM Cur INTO @PolicyId,@PolicyValue,@EffectiveDate,@PolicyStage,@ProdId
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT @MaxMember = MemberCount FROM tblProduct WHERE ProdId = @ProdId;
		--amani 07/12
		SELECT @ThresholdMember = Threshold FROM tblProduct WHERE ProdId = @ProdId;

		IF @MaxMember < @TotalInsurees
			GOTO NEXT_POLICY;

		EXEC @NewPolicyValue = uspPolicyValue @PolicyId = @PolicyId, @PolicyStage = @PolicyStage, @ErrorCode = @ErrorCode OUTPUT;
		--If new policy value is changed then the current insuree will not be insured
		IF @NewPolicyValue <> @PolicyValue OR @ErrorCode <> 0
		BEGIN
			IF @Activate = 0
			BEGIN
				
				SET @Premium=ISNULL((SELECT SUM(Amount) Amount FROM tblPremium WHERE PolicyID=@PolicyId AND ValidityTo IS NULL and isPhotoFee = 0 ),0) 
				IF @Premium < @NewPolicyValue 
					SET @EffectiveDate = NULL
			END
		END
				
		INSERT INTO tblInsureePolicy(InsureeId,PolicyId,EnrollmentDate,StartDate,EffectiveDate,ExpiryDate,AuditUserId,isOffline)
		SELECT @InsureeId, @PolicyId,EnrollDate,P.StartDate,@EffectiveDate,P.ExpiryDate,@AuditUserId,@isOffline
		FROM tblPolicy P 
		WHERE P.PolicyID = @PolicyId
			
NEXT_POLICY:
		FETCH NEXT FROM Cur INTO @PolicyId,@PolicyValue,@EffectiveDate,@PolicyStage,@ProdId
	END
	CLOSE Cur
	DEALLOCATE Cur
END

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[uspInsertPaymentIntent]
(
	@XML XML,
	@ExpectedAmount DECIMAL(18,2) = 0 OUT, 
	@PaymentID INT = 0 OUT,
	@ErrorNumber INT = 0,
	@ErrorMsg NVARCHAR(255)=NULL,
	@ProvidedAmount decimal(18,2) = 0,
	@PriorEnrollment BIT = 0 
	
)

AS
BEGIN
	DECLARE @tblHeader TABLE(officerCode nvarchar(12),requestDate DATE, phoneNumber NVARCHAR(50), AuditUSerID INT)
	DECLARE @tblDetail TABLE(InsuranceNumber nvarchar(12),productCode nvarchar(8), PolicyStage NVARCHAR(1),  isRenewal BIT, PolicyValue DECIMAL(18,2), isExisting BIT)
	DECLARE @OfficerLocationID INT
	DECLARE @OfficerParentLocationID INT
	DECLARE @AdultMembers INT 
	DECLARE @ChildMembers INT 
	DECLARE @oAdultMembers INT 
	DECLARE @oChildMembers INT 


	DECLARE @isEO BIT
		INSERT INTO @tblHeader(officerCode, requestDate, phoneNumber, AuditUSerID)
		SELECT 
		LEFT(NULLIF(T.H.value('(OfficerCode)[1]','NVARCHAR(50)'),''),12),
		NULLIF(T.H.value('(RequestDate)[1]','NVARCHAR(50)'),''),
		LEFT(NULLIF(T.H.value('(PhoneNumber)[1]','NVARCHAR(50)'),''),50),
		NULLIF(T.H.value('(AuditUserId)[1]','INT'),'')
		FROM @XML.nodes('PaymentIntent/Header') AS T(H)

		INSERT INTO @tblDetail(InsuranceNumber, productCode, isRenewal)
		SELECT 
		LEFT(NULLIF(T.D.value('(InsuranceNumber)[1]','NVARCHAR(12)'),''),12),
		LEFT(NULLIF(T.D.value('(ProductCode)[1]','NVARCHAR(8)'),''),8),
		T.D.value('(IsRenewal)[1]','BIT')
		FROM @XML.nodes('PaymentIntent/Details/Detail') AS T(D)
		
		IF @ErrorNumber != 0
		BEGIN
			GOTO Start_Transaction;
		END

		SELECT @AdultMembers =T.P.value('(AdultMembers)[1]','INT'), @ChildMembers = T.P.value('(ChildMembers)[1]','INT') , @oAdultMembers =T.P.value('(oAdultMembers)[1]','INT') , @oChildMembers = T.P.value('(oChildMembers)[1]','INT') FROM @XML.nodes('PaymentIntent/ProxySettings') AS T(P)
		
		SELECT @AdultMembers= ISNULL(@AdultMembers,0), @ChildMembers= ISNULL(@ChildMembers,0), @oAdultMembers= ISNULL(@oAdultMembers,0), @oChildMembers= ISNULL(@oChildMembers,0)
		

		UPDATE D SET D.isExisting = 1 FROM @tblDetail D 
		INNER JOIN  tblInsuree I ON D.InsuranceNumber = I.CHFID 
		WHERE I.IsHead = 1 AND I.ValidityTo IS NULL
	
		UPDATE  @tblDetail SET isExisting = 0 WHERE isExisting IS NULL

		IF EXISTS(SELECT 1 FROM @tblHeader WHERE officerCode IS NOT NULL)
			SET @isEO = 1
	/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	/*Error Codes
	2- Not valid insurance or missing product code
	3- Not valid enrolment officer code
	4 –Enrolment officer code and insurance product code are not compatible
	5- Beneficiary has no policy of specified insurance product for renewal
	6- Can not issue a control number as default indicated prior enrollment and Insuree has not been enrolled yet 

	7 - 'Insuree not enrolled and prior enrolement mandatory'


	-1. Unexpected error
	0. Success
	*/

	--2. Insurance number missing
		IF EXISTS(SELECT 1 FROM @tblDetail WHERE LEN(ISNULL(InsuranceNumber,'')) =0)
		BEGIN
			SET @ErrorNumber = 2
			SET @ErrorMsg ='Not valid insurance or missing product code'
			GOTO Start_Transaction;
		END

		--4. Missing product or Product does not exists
		IF EXISTS(SELECT 1 FROM @tblDetail D 
				LEFT OUTER JOIN tblProduct P ON P.ProductCode = D.productCode
				WHERE 
				(P.ValidityTo IS NULL AND P.ProductCode IS NULL) 
				OR D.productCode IS NULL
				)
		BEGIN
			SET @ErrorNumber = 4
			SET @ErrorMsg ='Not valid insurance or missing product code'
			GOTO Start_Transaction;
		END


	--3. Invalid Officer Code
		IF EXISTS(SELECT 1 FROM @tblHeader H
				LEFT JOIN tblOfficer O ON H.officerCode = O.Code
				WHERE O.ValidityTo IS NULL
				AND H.officerCode IS NOT NULL
				AND O.Code IS NULL
		)
		BEGIN
			SET @ErrorNumber = 3
			SET @ErrorMsg ='Not valid enrolment officer code'
			GOTO Start_Transaction;
		END

		
		--4. Wrong match of Enrollment Officer agaists Product
		SELECT @OfficerLocationID= L.LocationId, @OfficerParentLocationID = L.ParentLocationId FROM tblLocations L 
		INNER JOIN tblOfficer O ON O.LocationId = L.LocationId AND O.Code = (SELECT officerCode FROM @tblHeader WHERE officerCode IS NOT NULL)
		WHERE 
		L.ValidityTo IS NULL
		AND O.ValidityTo IS NULL


		IF EXISTS(SELECT D.productCode, P.ProductCode FROM @tblDetail D
			LEFT OUTER JOIN tblProduct P ON P.ProductCode = D.productCode AND (P.LocationId IS NULL OR P.LocationId = @OfficerLocationID OR P.LocationId = @OfficerParentLocationID)
			WHERE
			P.ValidityTo IS NULL
			AND P.ProductCode IS NULL
			) AND EXISTS(SELECT 1 FROM @tblHeader WHERE officerCode IS NOT NULL)
		BEGIN
			SET @ErrorNumber = 4
			SET @ErrorMsg ='Enrolment officer code and insurance product code are not compatible'
			GOTO Start_Transaction;
		END
		
		
		--The family does't contain this product for renewal
		IF EXISTS(SELECT 1 FROM @tblDetail D
				LEFT OUTER JOIN tblProduct PR ON PR.ProductCode = D.productCode
				LEFT OUTER JOIN tblInsuree I ON I.CHFID = D.InsuranceNumber
				LEFT OUTER JOIN tblPolicy PL ON PL.FamilyID = I.FamilyID  AND PL.ProdID = PR.ProdID
				WHERE PR.ValidityTo IS NULL
				AND D.isRenewal = 1 AND D.isExisting = 1
				AND I.ValidityTo IS NULL
				AND PL.ValidityTo IS NULL AND PL.PolicyID IS NULL)
		BEGIN
			SET @ErrorNumber = 5
			SET @ErrorMsg ='Beneficiary has no policy of specified insurance product for renewal'
			GOTO Start_Transaction;
		END

		

		--5. Proxy family can not renew
		IF EXISTS(SELECT 1 FROM @tblDetail WHERE isExisting =0 AND isRenewal= 1)
		BEGIN
			SET @ErrorNumber = 5
			SET @ErrorMsg ='Beneficiary has no policy of specified insurance product for renewal'
			GOTO Start_Transaction;
		END


		--7. Insurance number not existing in system 
		IF @PriorEnrollment = 1 AND EXISTS(SELECT 1 FROM @tblDetail D
				LEFT OUTER JOIN tblProduct PR ON PR.ProductCode = D.productCode
				LEFT OUTER JOIN tblInsuree I ON I.CHFID = D.InsuranceNumber
				LEFT OUTER JOIN tblPolicy PL ON PL.FamilyID = I.FamilyID  AND PL.ProdID = PR.ProdID
				WHERE PR.ValidityTo IS NULL
				AND I.ValidityTo IS NULL
				AND PL.ValidityTo IS NULL AND PL.PolicyID IS NULL)
		BEGIN
			SET @ErrorNumber = 7
			SET @ErrorMsg ='Insuree not enrolled and prior enrollment mandatory'
			GOTO Start_Transaction;
		END




	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/
	/**********************************************************************************************************************
			CALCULATIONS STARTS
	*********************************************************************************************************************/
	
	DECLARE @FamilyId INT, @ProductId INT, @PrevPolicyID INT, @PolicyID INT, @PremiumID INT
	DECLARE @PolicyStatus TINYINT=1
	DECLARE @PolicyValue DECIMAL(18,2), @PolicyStage NVARCHAR(1), @InsuranceNumber nvarchar(12), @productCode nvarchar(8), @enrollmentDate DATE, @isRenewal BIT, @isExisting BIT, @ErrorCode NVARCHAR(50)

	IF @ProvidedAmount = 0 
	BEGIN
		--only calcuylate if we want the system to provide a value to settle
		DECLARE CurFamily CURSOR FOR SELECT InsuranceNumber, productCode, isRenewal, isExisting FROM @tblDetail
		OPEN CurFamily
		FETCH NEXT FROM CurFamily INTO @InsuranceNumber, @productCode, @isRenewal, @isExisting;
		WHILE @@FETCH_STATUS = 0
		BEGIN
									
			SET @PolicyStatus=1
			SET @FamilyId = NULL
			SET @ProductId = NULL
			SET @PolicyID = NULL


			IF @isRenewal = 1
				SET @PolicyStage = 'R'
			ELSE 
				SET @PolicyStage ='N'

			IF @isExisting = 1
			BEGIN
											
				SELECT @FamilyId = FamilyId FROM tblInsuree I WHERE IsHead = 1 AND CHFID = @InsuranceNumber  AND ValidityTo IS NULL
				SELECT @ProductId = ProdID FROM tblProduct WHERE ProductCode = @productCode  AND ValidityTo IS NULL
				SELECT TOP 1 @PolicyID =  PolicyID FROM tblPolicy WHERE FamilyID = @FamilyId  AND ProdID = @ProductId AND PolicyStage = @PolicyStage AND ValidityTo IS NULL ORDER BY EnrollDate DESC
											
				IF @isEO = 1
					IF EXISTS(SELECT 1 FROM tblPremium WHERE PolicyID = @PolicyID AND ValidityTo IS NULL)
					BEGIN
						SELECT @PolicyValue =  ISNULL(PR.Amount - ISNULL(MatchedAmount,0),0) FROM tblPremium PR
														INNER JOIN tblPolicy PL ON PL.PolicyID = PR.PolicyID
														LEFT OUTER JOIN (SELECT PremiumID, SUM (Amount) MatchedAmount from tblPaymentDetails WHERE ValidityTo IS NULL GROUP BY PremiumID ) PD ON PD.PremiumID = PR.PremiumId
														WHERE
														PR.ValidityTo IS NULL
														AND PL.ValidityTo IS NULL AND PL.PolicyID = @PolicyID
														IF @PolicyValue < 0
														SET @PolicyValue = 0.00
					END
					ELSE
					BEGIN
						EXEC @PolicyValue = uspPolicyValue @FamilyId, @ProductId, 0, @PolicyStage, NULL, 0;
					END
												
				ELSE IF @PolicyStage ='N'
				BEGIN
					EXEC @PolicyValue = uspPolicyValue @FamilyId, @ProductId, 0, 'N', NULL, 0;
				END
				ELSE
				BEGIN
					SELECT TOP 1 @PrevPolicyID = PolicyID, @PolicyStatus = PolicyStatus FROM tblPolicy  WHERE ProdID = @ProductId AND FamilyID = @FamilyId AND ValidityTo IS NULL AND PolicyStatus != 4 ORDER BY EnrollDate DESC
					IF @PolicyStatus = 1
					BEGIN
						SELECT @PolicyValue =  (ISNULL(SUM(PL.PolicyValue),0) - ISNULL(SUM(Amount),0)) FROM tblPolicy PL 
						LEFT OUTER JOIN tblPremium PR ON PR.PolicyID = PL.PolicyID
						WHERE PL.ValidityTo IS NULL
						AND PR.ValidityTo IS NULL
						AND PL.PolicyID = @PrevPolicyID
						IF @PolicyValue < 0
							SET @PolicyValue =0
					END
					ELSE
					BEGIN 
						EXEC @PolicyValue = uspPolicyValue @FamilyId, @ProductId, 0, 'R', @enrollmentDate, @PrevPolicyID, @ErrorCode OUTPUT;
					END
				END
			END
			ELSE
			BEGIN
				EXEC @PolicyValue = uspPolicyValueProxyFamily @productCode, @AdultMembers, @ChildMembers,@oAdultMembers,@oChildMembers
			END
			UPDATE @tblDetail SET PolicyValue = ISNULL(@PolicyValue,0), PolicyStage = @PolicyStage  WHERE InsuranceNumber = @InsuranceNumber AND productCode = @productCode AND isRenewal = @isRenewal
			FETCH NEXT FROM CurFamily INTO @InsuranceNumber, @productCode, @isRenewal, @isExisting;
		END
		CLOSE CurFamily
		DEALLOCATE CurFamily;

	

	END

	
	
		
	--IF IT REACHES UP TO THIS POINT THEN THERE IS NO ERROR
	SET @ErrorNumber = 0
	SET @ErrorMsg = NULL


	/**********************************************************************************************************************
			CALCULATIONS ENDS
	 *********************************************************************************************************************/

	 /**********************************************************************************************************************
			INSERTION STARTS
	 *********************************************************************************************************************/
		Start_Transaction:
		BEGIN TRY
			BEGIN TRANSACTION INSERTPAYMENTINTENT
				
				IF @ProvidedAmount > 0 
					SET @ExpectedAmount = @ProvidedAmount 
				ELSE
					SELECT @ExpectedAmount = SUM(ISNULL(PolicyValue,0)) FROM @tblDetail
				
				SET @ErrorMsg = ISNULL(CONVERT(NVARCHAR(5),@ErrorNumber)+': '+ @ErrorMsg,NULL)
				--Inserting Payment
				INSERT INTO [dbo].[tblPayment]
				 ([ExpectedAmount],[OfficerCode],[PhoneNumber],[RequestDate],[PaymentStatus],[ValidityFrom],[AuditedUSerID],[RejectedReason]) 
				 SELECT
				 @ExpectedAmount, officerCode, phoneNumber, GETDATE(),CASE @ErrorNumber WHEN 0 THEN 0 ELSE -1 END, GETDATE(), AuditUSerID, @ErrorMsg
				 FROM @tblHeader
				 SELECT @PaymentID= SCOPE_IDENTITY();

				 --Inserting Payment Details
				 DECLARE @AuditedUSerID INT
				 SELECT @AuditedUSerID = AuditUSerID FROM @tblHeader
				INSERT INTO [dbo].[tblPaymentDetails]
			   ([PaymentID],[ProductCode],[InsuranceNumber],[PolicyStage],[ValidityFrom],[AuditedUserId], ExpectedAmount) SELECT
				@PaymentID, productCode, InsuranceNumber,  CASE isRenewal WHEN 0 THEN 'N' ELSE 'R' END, GETDATE(), @AuditedUSerID, PolicyValue
				FROM @tblDetail D
				


			COMMIT TRANSACTION INSERTPAYMENTINTENT
			RETURN @ErrorNumber
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION INSERTPAYMENTINTENT
			SELECT ERROR_MESSAGE()
			RETURN -1
		END CATCH

	 /**********************************************************************************************************************
			INSERTION ENDS
	 *********************************************************************************************************************/

END









GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[uspAPIRenewPolicy]
(	@AuditUserID INT = -3,
	@InsuranceNumber NVARCHAR(12),
	@RenewalDate DATE,
	@ProductCode NVARCHAR(8),
	@EnrollmentOfficerCode NVARCHAR(8)
)

AS
BEGIN
	/*
	RESPONSE CODES
		1-Wrong format or missing insurance number 
		2-Insurance number of not found
		3- Wrong or missing product code (not existing or not applicable to the family/group)
		4- Wrong or missing renewal date
		5- Wrong or missing enrolment officer code (not existing or not applicable to the family/group)
		0 - all ok
		-1 Unknown Error

	*/


/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	--1-Wrong format or missing insurance number 
	IF LEN(ISNULL(@InsuranceNumber,'')) = 0
		RETURN 1
	
	--2-Insurance number of not found
	IF NOT EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID = @InsuranceNumber AND ValidityTo IS NULL)
		RETURN 2

	--3- Wrong or missing product code (not existing or not applicable to the family/group)
	IF LEN(ISNULL(@ProductCode,'')) = 0
		RETURN 3

	IF NOT EXISTS(SELECT F.LocationId, V.LocationName, V.LocationType, D.ParentLocationId, PR.ProductCode FROM tblInsuree I
		INNER JOIN tblFamilies F ON F.FamilyID = I.FamilyID
		INNER JOIN tblLocations V ON V.LocationId = F.LocationId
		INNER JOIN tblLocations M ON M.LocationId = V.ParentLocationId
		INNER JOIN tblLocations D ON D.LocationId = M.ParentLocationId
		INNER JOIN tblProduct PR ON (PR.LocationId = D.LocationId) OR PR.LocationId =  D.ParentLocationId OR PR.LocationId IS NULL 
		WHERE
		F.ValidityTo IS NULL
		AND V.ValidityTo IS NULL
		AND PR.ValidityTo IS NULL AND PR.ProductCode =@ProductCode
		AND I.CHFID = @InsuranceNumber AND I.ValidityTo IS NULL AND I.IsHead = 1)
		RETURN 3


	--Validating Conversional product
		DECLARE @ProdId INT,
				@ConvertionalProdId INT,
				@DateTo DATE
		
		SELECT @DateTo = DateTo, @ConvertionalProdId = ConversionProdID, @ProdId = ProdID FROM tblProduct WHERE ProductCode = @ProductCode  AND ValidityTo IS NULL
			
		IF GETDATE() > = @DateTo 
			BEGIN
				IF @ConvertionalProdId IS NOT NULL
						SET @ProdId = @ConvertionalProdId
					ELSE
						RETURN 3
			END
				
		--4- Wrong or missing renewal date
		IF NULLIF(@RenewalDate,'') IS NULL
			RETURN 4

		--5- Wrong or missing enrolment officer code (not existing or not applicable to the family/group)
		IF LEN(ISNULL(@EnrollmentOfficerCode,'')) = 0
			RETURN 5
	
		IF NOT EXISTS(SELECT 1 FROM tblInsuree I 
						INNER JOIN tblFamilies F ON F.FamilyID = I.FamilyID
						INNER JOIN tblLocations V ON V.LocationId = F.LocationId
						INNER JOIN tblLocations M ON M.LocationId = V.ParentLocationId
						INNER JOIN tblLocations D ON D.LocationId = M.ParentLocationId
						INNER JOIN tblOfficer O ON O.LocationId = D.LocationId
						WHERE 
						I.CHFID = @InsuranceNumber AND O.Code= @EnrollmentOfficerCode
						AND I.ValidityTo IS NULL
						AND F.ValidityTo IS NULL
						AND V.ValidityTo IS NULL
						AND O.ValidityTo IS NULL)
		 RETURN 5

	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/

		/****************************************BEGIN TRANSACTION *************************/
		BEGIN TRY
			BEGIN TRANSACTION RENEWPOLICY
			
				DECLARE @tblPeriod TABLE(startDate DATE, expiryDate DATE, HasCycle  BIT)
				DECLARE @FamilyId INT = 0,
				@PolicyValue DECIMAL(18, 4),
				@PolicyStage CHAR(1)='R',
				@StartDate DATE,
				@ExpiryDate DATE,
				@EffectiveDate DATE,
				@ErrorCode INT,
				@PolicyStatus INT,
				@PolicyId INT,
				@Active TINYINT=2,
				@Idle TINYINT=1,
				@OfficerID INT,
				@HasCycle BIT

				SELECT @FamilyId = FamilyID FROM tblInsuree WHERE CHFID = @InsuranceNumber  AND ValidityTo IS NULL
				INSERT INTO @tblPeriod(StartDate, ExpiryDate, HasCycle)
				EXEC uspGetPolicyPeriod @ProdId, @RenewalDate, @HasCycle OUTPUT, @PolicyStage;
				EXEC @PolicyValue = uspPolicyValue @FamilyId, @ProdId, 0, @PolicyStage, @RenewalDate, 0, @ErrorCode OUTPUT;
				SELECT @StartDate = startDate FROM @tblPeriod
				SELECT @ExpiryDate = expiryDate FROM @tblPeriod
				SELECT @OfficerID = OfficerID FROM tblOfficer WHERE Code = @EnrollmentOfficerCode AND ValidityTo IS NULL

					INSERT INTO tblPolicy(FamilyID,EnrollDate,StartDate,EffectiveDate,ExpiryDate,PolicyStatus,PolicyValue,ProdID,OfficerID,PolicyStage,ValidityFrom,AuditUserID, isOffline)
					SELECT	 @FamilyId,@RenewalDate,@StartDate,@EffectiveDate,@ExpiryDate,@Idle,@PolicyValue,@ProdId,@OfficerID,@PolicyStage,GETDATE(),@AuditUserId, 0 isOffline 
					SET @PolicyId = SCOPE_IDENTITY()

	

							DECLARE @InsureeId INT
							DECLARE CurNewPolicy CURSOR FOR SELECT I.InsureeID FROM tblInsuree I 
							INNER JOIN tblFamilies F ON I.FamilyID = F.FamilyID 
							INNER JOIN tblPolicy P ON P.FamilyID = F.FamilyID 
							WHERE P.PolicyId = @PolicyId 
							AND I.ValidityTo IS NULL 
							AND F.ValidityTo IS NULL
							AND P.ValidityTo IS NULL
							OPEN CurNewPolicy;
							FETCH NEXT FROM CurNewPolicy INTO @InsureeId;
							WHILE @@FETCH_STATUS = 0
							BEGIN
								EXEC uspAddInsureePolicy @InsureeId;
								FETCH NEXT FROM CurNewPolicy INTO @InsureeId;
							END
							CLOSE CurNewPolicy;
							DEALLOCATE CurNewPolicy; 

			COMMIT TRANSACTION RENEWPOLICY
			RETURN 0
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION RENEWPOLICY
			SELECT ERROR_MESSAGE()
			RETURN -1
		END CATCH
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[uspAPIEditMemberFamily]
(
	@AuditUserID INT = -3,
	@InsureeNumber NVARCHAR(12),
	@OtherNames NVARCHAR(100) = NULL,
	@LastName NVARCHAR(100) = NULL,
	@BirthDate DATE = NULL,
	@Gender NVARCHAR(1) = NULL,
	@Relationship NVARCHAR(50) = NULL,
	@MaritalStatus NVARCHAR(1) = NULL,
	@BeneficiaryCard BIT = NULL,
	@VillageCode NVARCHAR(8) = NULL,
	@CurrentAddress NVARCHAR(200) = NULL,
	@Proffesion NVARCHAR(50) = NULL,
	@Education NVARCHAR(50) = NULL,
	@PhoneNumber NVARCHAR(50) = NULL,
	@Email NVARCHAR(100) = NULL,
	@IdentificationType NVARCHAR(1) = NULL,
	@IdentificationNumber NVARCHAR(25) = NULL,
	@FSPCode NVARCHAR(8) = NULL
)

AS
BEGIN
	/*
	RESPONSE CODE
		1-Wrong format or missing insurance number of a member
		2-Insurance number of head not found
		3- Wrong format or missing insurance number of member
		4-Insurance number of member not found
		5-Wrong current village code
		6-Wrong gender
		7-Wrong marital status
		8-Wrong education
		9 - Wrong profession
		10 - FSP code not found
		11 - Wrong identification type
		12 - Wrong Relation
		-1 - Unexpected error
	*/


	/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	--3- Wrong format or missing insurance number of member
	IF LEN(ISNULL(@InsureeNumber,'')) = 0
		RETURN 3

	--4 - Insurance number of member not found
	IF NOT EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID = @InsureeNumber AND ValidityTo IS NULL)
		RETURN 4

	--5-Wrong current village code
	IF NOT EXISTS(SELECT 1 FROM tblLocations  WHERE LocationCode = @VillageCode AND ValidityTo IS NULL AND LocationType ='V') AND LEN(ISNULL(@VillageCode,'')) > 0
		RETURN 5

	--6-Wrong gender
	IF NOT EXISTS(SELECT 1 FROM tblGender WHERE Code = @Gender) AND LEN(ISNULL(@Gender,'')) > 0
		RETURN 6

	--7-Wrong marital status
	IF dbo.udfAPIisValidMaritalStatus(@MaritalStatus) = 0 AND LEN(ISNULL(@MaritalStatus,'')) > 0
		RETURN 7

	--8-Wrong education
	IF NOT EXISTS(SELECT  1 FROM tblEducations WHERE Education = @Education) AND LEN(ISNULL(@Education,'')) > 0
		RETURN 8

	--9 - Wrong profession
	IF NOT EXISTS(SELECT  1 FROM tblProfessions WHERE Profession = @Proffesion) AND LEN(ISNULL(@Proffesion,'')) > 0
		RETURN 9

	--10 - FSP code not found
	IF NOT EXISTS(SELECT  1 FROM tblHF WHERE HFCode = @FSPCode AND ValidityTo IS NULL) AND LEN(ISNULL(@FSPCode,'')) > 0
		RETURN 10
	--11 - Wrong identification type
	IF NOT EXISTS(SELECT 1 FROM tblIdentificationTypes WHERE  IdentificationCode  = @IdentificationType ) AND LEN(ISNULL(@IdentificationType,'')) > 0
		RETURN 11

	--12 - Wrong Relation
	IF NOT EXISTS(SELECT  1 FROM tblRelations WHERE Relation = @Relationship) AND LEN(ISNULL(@Relationship,'')) > 0
		RETURN 12

	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/

	BEGIN TRY
			BEGIN TRANSACTION EDITMEMBERFAMILY
			
				DECLARE @FamilyID INT,
				
						@ProfessionId INT,
						@RelationId INT,
						@EducationId INT,
						@LocationId INT,
						@HfID INT,
						@InsureeId INT,
						@AssociatedPhotoFolder NVARCHAR(255),
						@DBOtherNames NVARCHAR(100) = NULL,
						@DBLastName NVARCHAR(100) = NULL,
						@DBBirthDate DATE = NULL,
						@DBGender NVARCHAR(1) = NULL,
						@DBRelationshipID NVARCHAR(50) = NULL,
						@DBMaritalStatus NVARCHAR(1) = NULL,
						@DBBeneficiaryCard BIT = NULL,
						@DBVillageID INT = NULL,
						@DBCurrentAddress NVARCHAR(200) = NULL,
						@DBProffesionID INT = NULL,
						@DBEducationID INT = NULL,
						@DBPhoneNumber NVARCHAR(50) = NULL,
						@DBEmail NVARCHAR(100) = NULL,
						@DBIdentificationNumber NVARCHAR(25) = NULL,
						@DBIdentificationType NVARCHAR(1) = NULL,
						@DBFSPCode NVARCHAR(8) = NULL


				SET @AssociatedPhotoFolder=(SELECT AssociatedPhotoFolder FROM tblIMISDefaults)
				SELECT @HfID = HfID FROM tblHF WHERE HFCode = @FSPCode AND ValidityTo IS NULL
				SELECT @ProfessionId = ProfessionId FROM tblProfessions WHERE Profession = @Proffesion 
				SELECT @EducationId = EducationId FROM tblEducations WHERE Education = @Education
				SELECT @RelationId = RelationId FROM tblRelations WHERE Relation = @Relationship
				SELECT @LocationId = LocationId FROM tblLocations WHERE LocationCode = @VillageCode AND ValidityTo IS NULL
				SELECT @InsureeId = InsureeID, @DBOtherNames = OtherNames, @DBLastName= LastName, @DBBirthDate = DOB, @DBGender = Gender, @DBMaritalStatus= Marital, @DBBeneficiaryCard = CardIssued, 
				@DBVillageID = CurrentVillage, @DBCurrentAddress = CurrentAddress, @DBProffesionID = Profession, @DBEducationID =Education, @DBPhoneNumber = Phone, @DBEmail = Email, 
				@DBIdentificationNumber = passport, @DBFSPCode = HFID, @DBIdentificationType = TypeOfId, @DBRelationshipID=Relationship 
				FROM tblInsuree WHERE CHFID = @InsureeNumber AND ValidityTo IS NULL

					SET	@OtherNames = ISNULL(@OtherNames, @DBOtherNames)
					SET	@LastName = ISNULL(@LastName, @DBLastName)
					SET	@BirthDate = ISNULL(@BirthDate, @DBBirthDate)
					SET	@Gender = ISNULL(@Gender, @DBGender)
					SET	@RelationId = ISNULL(@RelationId, @DBRelationshipID)
					SET	@MaritalStatus = ISNULL(@MaritalStatus, @DBMaritalStatus)
					SET	@BeneficiaryCard = ISNULL(@BeneficiaryCard, @DBBeneficiaryCard)
					SET	@LocationId = ISNULL(@LocationId, @DBVillageID)
					SET	@CurrentAddress = ISNULL(@CurrentAddress, @DBCurrentAddress)
					SET	@ProfessionId = ISNULL(@ProfessionId, @DBProffesionID)
					SET	@EducationId = ISNULL(@EducationId, @DBEducationID)
					SET	@PhoneNumber = ISNULL(@PhoneNumber, @DBPhoneNumber)
					SET	@Email = ISNULL(@Email, @DBEmail)
					SET @IdentificationType = ISNULL(@IdentificationType,@DBIdentificationType )
					SET	@IdentificationNumber = ISNULL(@IdentificationNumber, @DBIdentificationNumber)

					SET	@FSPCode = ISNULL(@FSPCode, @DBFSPCode)

				--Insert Insuree History
					INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,[ValidityTo],legacyId,[Relationship],[Profession],[Education],[Email],[TypeOfId],[HFID], [CurrentAddress], [GeoLocation], [CurrentVillage]) 
					SELECT	I.[FamilyID],I.[CHFID],I.[LastName],I.[OtherNames],I.[DOB],I.[Gender],I.[Marital],I.[IsHead],I.[passport],I.[Phone],I.[PhotoID],I.[PhotoDate],I.[CardIssued],I.isOffline,I.[AuditUserID],I.[ValidityFrom] ,GETDATE() ValidityTo,I.InsureeID,I.[Relationship],I.[Profession],I.[Education],I.[Email]  ,I.[TypeOfId],I.[HFID], I.[CurrentAddress], I.[GeoLocation], [CurrentVillage] FROM tblInsuree I
					WHERE I.InsureeID = @InsureeId AND  I.ValidityTo IS NULL
					
					UPDATE tblInsuree  SET [LastName] = @LastName, [OtherNames] = @OtherNames,[DOB] = @BirthDate, [Gender] = @Gender,[Marital] = @MaritalStatus, [TypeOfId]  = @IdentificationType ,[passport] = @IdentificationNumber,[Phone] = @PhoneNumber,[CardIssued] = ISNULL(@BeneficiaryCard,0),[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID ,[Relationship] = @RelationId, [Profession] = @ProfessionId, [Education] = @EducationId,[Email] = @Email ,HFID = @HFID, CurrentAddress = @CurrentAddress, CurrentVillage = @LocationId, GeoLocation = @LocationId 
					WHERE InsureeID = @InsureeId AND  ValidityTo IS NULL 
				
					
								


			COMMIT TRANSACTION EDITMEMBERFAMILY
			RETURN 0
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION EDITMEMBERFAMILY
			SELECT ERROR_MESSAGE()
			RETURN -1
		END CATCH
END




GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[uspAPIEnterMemberFamily]
(
	@AuditUserID INT = -3,
	@InsureeNumberOfHead NVARCHAR(12),
	@InsureeNumber NVARCHAR(12),
	@OtherNames NVARCHAR(100),
	@LastName NVARCHAR(100),
	@BirthDate DATE,
	@Gender NVARCHAR(1),
	@Relationship NVARCHAR(50) = NULL,
	@MaritalStatus NVARCHAR(1) = NULL,
	@BeneficiaryCard BIT = 0,
	@VillageCode NVARCHAR(8)= NULL,
	@CurrentAddress NVARCHAR(200) = '',
	@Proffesion NVARCHAR(50)= NULL,
	@Education NVARCHAR(50)= NULL,
	@PhoneNumber NVARCHAR(50) = '',
	@Email NVARCHAR(100)= '',
	@IdentificationType NVARCHAR(1) = NULL,
	@IdentificationNumber NVARCHAR(25) = '',
	@FSPCode NVARCHAR(8) = NULL
)

AS
BEGIN
	/*
	RESPONSE CODE
		1-Wrong format or missing insurance number of head
		2-Insurance number of head not found
		3- Wrong format or missing insurance number of member
		4-Wrong or missing  gender
		5-Wrong format or missing birth date
		6-Missing last name
		7-Missing other name
		8- Insurance number of member duplicated
		9- Wrong current village code
		10-Wrong marital status
		11-Wrong education
		12-Wrong profession
		13-Wrong RelationShip
		14-FSP code not found 
		15 - wrong identification type 
		0 - Success (0 OK), 
		-1 -Unknown  Error 
	*/


	/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	--1-Wrong format or missing insurance number of head
	IF LEN(ISNULL(@InsureeNumberOfHead,'')) = 0
		RETURN 1

	--2-Insurance number of head not found
	IF NOT EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID = @InsureeNumberOfHead AND ValidityTo IS NULL)
		RETURN 2

	--3- Wrong format or missing insurance number of member
	IF LEN(ISNULL(@InsureeNumber,'')) = 0
		RETURN 3
	--4-Wrong or missing  gender
	IF LEN(ISNULL(@Gender,'')) = 0
		RETURN 4

	IF NOT EXISTS(SELECT 1 FROM tblGender WHERE Code = @Gender)
		RETURN 4

	--5-Wrong format or missing birth date
	IF NULLIF(@BirthDate,'') IS NULL
		RETURN 5

	--6-Missing last name
	IF LEN(ISNULL(@LastName,'')) = 0 
			RETURN 6
	
	--7-Missing other name
	IF LEN(ISNULL(@OtherNames,'')) = 0 
		RETURN 7

	--8- Insurance number of member duplicated
	IF EXISTS(SELECT 1 FROM tblInsuree WHERE ValidityTo IS NULL AND CHFID = @InsureeNumber)
		RETURN 8

	--9- Wrong current village code
	IF NOT EXISTS(SELECT 1 FROM tblLocations  WHERE LocationCode = @VillageCode AND ValidityTo IS NULL AND LocationType ='V') AND LEN(ISNULL(@VillageCode,'')) > 0
		RETURN 9

	--10-Wrong marital status
	IF dbo.udfAPIisValidMaritalStatus(@MaritalStatus) = 0 AND LEN(ISNULL(@MaritalStatus,'')) > 0
		RETURN 10

	--11-Wrong education
	IF NOT EXISTS(SELECT  1 FROM tblEducations WHERE Education = @Education) AND LEN(ISNULL(@Education,'')) > 0
		RETURN 11

	--12 - Wrong profession
	IF NOT EXISTS(SELECT  1 FROM tblProfessions WHERE Profession = @Proffesion) AND LEN(ISNULL(@Proffesion,'')) > 0
		RETURN 12

	--13 - FSP code not found
	IF NOT EXISTS(SELECT  1 FROM tblHF WHERE HFCode = @FSPCode AND ValidityTo IS NULL) AND LEN(ISNULL(@FSPCode,'')) > 0
		RETURN 13

	--14 - Wrong Relation
	IF NOT EXISTS(SELECT  1 FROM tblRelations WHERE Relation = @Relationship) AND LEN(ISNULL(@Relationship,'')) > 0
		RETURN 14



	--15 - Wrong identification type
	IF NOT EXISTS(SELECT 1 FROM tblIdentificationTypes WHERE  IdentificationCode  = @IdentificationType ) AND LEN(ISNULL(@IdentificationType,'')) > 0
		RETURN 15
	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/

	BEGIN TRY
			BEGIN TRANSACTION ENROLMEMBERFAMILY
			
				DECLARE @FamilyID INT,
						@ProfessionId INT,
						@RelationId INT,
						@EducationId INT,
						@LocationId INT,
						@HfID INT,
						@InsureeId INT



				SET @FamilyID = (SELECT TOP 1 FamilyID FROM tblInsuree WHERE CHFID = @InsureeNumberOfHead AND ValidityTo IS NULL ORDER BY FamilyID DESC)
				SELECT @HfID = HfID FROM tblHF WHERE HFCode = @FSPCode AND ValidityTo IS NULL
				SELECT @ProfessionId = ProfessionId FROM tblProfessions WHERE Profession = @Proffesion 
				SELECT @EducationId = EducationId FROM tblEducations WHERE Education = @Education
				SELECT @RelationId = RelationId FROM tblRelations WHERE Relation = @Relationship
				SELECT @LocationId = LocationId FROM tblLocations WHERE LocationCode = @VillageCode AND ValidityTo IS NULL


				INSERT INTO dbo.tblInsuree
					(FamilyID,CHFID,LastName,OtherNames,DOB,Gender,Marital,IsHead,Phone, CardIssued, passport,TypeOfId , ValidityFrom,AuditUserID,Profession,Education, Relationship, Email,isOffline,HFID,CurrentAddress,CurrentVillage)
					SELECT @FamilyID FamilyID, @InsureeNumber CHFID, @LastName LastName, @OtherNames OtherNames, @BirthDate BirthDate, @Gender Gender, @MaritalStatus Marital, 0  IsHead, @PhoneNumber Phone, @BeneficiaryCard BeneficiaryCard, @IdentificationNumber PassPort, @IdentificationType , GETDATE() ValidityFrom,@AuditUserID AuditUserID, @ProfessionId Profession, @EducationId Education, @RelationId Relation, @Email Email, 0 IsOffline, @HfID, @CurrentAddress CurrentAddress, @LocationId CurrentVillage
							SET @InsureeId = SCOPE_IDENTITY()

							INSERT INTO tblPhotos(InsureeID,CHFID,PhotoFolder,PhotoFileName,OfficerID,PhotoDate,ValidityFrom,AuditUserID)
					SELECT InsureeID,CHFID,'','',0,GETDATE(),ValidityFrom,AuditUserID from tblInsuree WHERE InsureeID = @InsureeID; 
					UPDATE tblInsuree SET PhotoID = (SELECT IDENT_CURRENT('tblPhotos')),PhotoDate=GETDATE() WHERE InsureeID = @InsureeID;

							EXEC uspAddInsureePolicy @InsureeId;
								
				

			COMMIT TRANSACTION ENROLMEMBERFAMILY
			RETURN 0
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION ENROLMEMBERFAMILY
			SELECT ERROR_MESSAGE()
			RETURN -1
		END CATCH

END




GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[uspAPIEnterFamily]
(
	@AuditUserID INT = -3,
	@PermanentVillageCode NVARCHAR(8),
	@InsuranceNumber NVARCHAR(12),
	@OtherNames NVARCHAR(100),
	@LastName NVARCHAR(100),
	@BirthDate DATE,
	@Gender NVARCHAR(1),
	@PovertyStatus BIT = NULL,
	@ConfirmationNo nvarchar(12) = '' ,
	@ConfirmationType NVARCHAR(1) = NULL,
	@PermanentAddress NVARCHAR(200) = '',
	@MaritalStatus NVARCHAR(1) = NULL,
	@BeneficiaryCard BIT = 0 ,
	@CurrentVillageCode NVARCHAR(8) = NULL ,
	@CurrentAddress NVARCHAR(200) = '',
	@Proffesion NVARCHAR(50) = NULL,
	@Education NVARCHAR(50) = NULL,
	@PhoneNumber NVARCHAR(50) = '',
	@Email NVARCHAR(100) = '',
	@IdentificationType NVARCHAR(1) = NULL,
	@IdentificationNumber NVARCHAR(25) = '',
	@FSPCode NVARCHAR(8) = NULL,
	@GroupType NVARCHAR(2)= NULL
)
AS
BEGIN

	/*
	RESPONSE CODES
		1 - Wrong format or missing insurance number of head
		2 - Duplicated insurance number of head
		3 - Wrong or missing permanent village code
		4 - Wrong current village code
		5 - Wrong or missing  gender
		6 - Wrong format or missing birth date
		7 - Missing last name
		8 - Missing other name
		9 - Wrong confirmation type
		10 - Wrong group type
		11 - Wrong marital status
		12 - Wrong education
		13 - Wrong profession
		14 - FSP code not found
		15 - wrong identification type 
		0 - Success 
		-1 Unknown Error

	*/



	/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	--1 - Wrong format or missing insurance number of head
	IF LEN(ISNULL(@InsuranceNumber,'')) = 0
		RETURN 1
	
	--2 - Duplicated insurance number of head
	IF EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID = @InsuranceNumber AND ValidityTo IS NULL)
		RETURN 2

	--3 - Wrong or missing permanent village code
	IF LEN(ISNULL(@PermanentVillageCode,'')) = 0
		RETURN 3

	IF NOT EXISTS(SELECT 1 FROM tblLocations  WHERE LocationCode = @PermanentVillageCode AND ValidityTo IS NULL AND LocationType ='V')
		RETURN 3

	--4 - Wrong current village code
	IF LEN(ISNULL(@CurrentVillageCode,'')) <> 0
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM tblLocations  WHERE LocationCode = @CurrentVillageCode AND ValidityTo IS NULL AND LocationType ='V')
		RETURN 4
	END

	--5 - Wrong or missing  gender
	IF LEN(ISNULL(@Gender,'')) = 0
		RETURN 5

	IF NOT EXISTS(SELECT 1 FROM tblGender WHERE Code = @Gender)
		RETURN 5
	
	--6 - Wrong format or missing birth date
	IF NULLIF(@BirthDate,'') IS NULL
		RETURN 6
	
	--7 - Missing last name
	IF LEN(ISNULL(@LastName,'')) = 0 
		RETURN 7
	
	--8 - Missing other name
	IF LEN(ISNULL(@OtherNames,'')) = 0 
		RETURN 8

	--9 - Wrong confirmation type
	IF NOT EXISTS(SELECT 1 FROM tblConfirmationTypes WHERE ConfirmationTypeCode = @ConfirmationType) AND LEN(ISNULL(@ConfirmationType,'')) > 0
		RETURN 9
	
	--10 - Wrong group type
	IF NOT EXISTS(SELECT  1 FROM tblFamilyTypes WHERE FamilyTypeCode = @GroupType) AND LEN(ISNULL(@GroupType,'')) > 0
		RETURN 10

	--11 - Wrong marital status
	IF dbo.udfAPIisValidMaritalStatus(@MaritalStatus) = 0 AND LEN(ISNULL(@MaritalStatus,'')) > 0
		RETURN 11

	--12 - Wrong education
	IF NOT EXISTS(SELECT  1 FROM tblEducations WHERE Education = @Education) AND LEN(ISNULL(@Education,'')) > 0
		RETURN 12

	--13 - Wrong profession
	IF NOT EXISTS(SELECT  1 FROM tblProfessions WHERE Profession = @Proffesion) AND LEN(ISNULL(@Proffesion,'')) > 0
		RETURN 13

	--14 - FSP code not found
	IF NOT EXISTS(SELECT  1 FROM tblHF WHERE HFCode = @FSPCode AND ValidityTo IS NULL) AND LEN(ISNULL(@FSPCode,'')) > 0
		RETURN 14

	--15 - Wrong identification type
	IF NOT EXISTS(SELECT 1 FROM tblIdentificationTypes WHERE  IdentificationCode  = @IdentificationType ) AND LEN(ISNULL(@IdentificationType,'')) > 0
		RETURN 15


	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/

		/****************************************BEGIN TRANSACTION *************************/
		BEGIN TRY
			BEGIN TRANSACTION ENROLFAMILY
			
				DECLARE @FamilyID INT,
						@InsureeID INT,
			
						@ProfessionId INT,
						@LocationId INT,
						@CurrentLocationId INT=0,
						@EducationId INT,
						@HfID INT

						SELECT @HfID = HfID FROM tblHF WHERE HFCode = @FSPCode AND ValidityTo IS NULL
						SELECT @ProfessionId = ProfessionId FROM tblProfessions WHERE Profession = @Proffesion 
						SELECT @EducationId = EducationId FROM tblEducations WHERE Education = @Education
						SELECT @HfID = HfID FROM tblHF WHERE HFCode = @FSPCode AND ValidityTo IS NULL
						SELECT @ProfessionId = ProfessionId FROM tblProfessions WHERE Profession = @Proffesion 
						SELECT @EducationId = EducationId FROM tblEducations WHERE Education = @Education
						SELECT @CurrentLocationId = LocationId FROM tblLocations WHERE LocationCode = @CurrentVillageCode AND ValidityTo IS NULL
						SELECT @LocationId = LocationId FROM tblLocations WHERE LocationCode = @PermanentVillageCode AND ValidityTo IS NULL


					INSERT INTO dbo.tblFamilies
						   (InsureeID,LocationId,Poverty,ValidityFrom,AuditUserID,FamilyType,FamilyAddress,isOffline,ConfirmationType,ConfirmationNo )
					SELECT 0 InsureeID, @LocationId LocationId, @PovertyStatus Poverty, GETDATE() ValidityFrom, @AuditUserID AuditUserID, @GroupType FamilyType, @PermanentAddress FamilyAddress, 0 isOffline, @ConfirmationType ConfirmationType, @ConfirmationNo ConfirmationNo
					SET @FamilyID = SCOPE_IDENTITY()

	

				INSERT INTO dbo.tblInsuree
					(FamilyID,CHFID,LastName,OtherNames,DOB,Gender,Marital,IsHead,Phone, CardIssued, passport,TypeOfId , ValidityFrom,AuditUserID,Profession,Education,Email,isOffline,HFID,CurrentAddress,CurrentVillage)
					SELECT @FamilyID FamilyID, @InsuranceNumber CHFID, @LastName LastName, @OtherNames OtherNames, @BirthDate BirthDate, @Gender Gender, @MaritalStatus Marital, 1 IsHead, @PhoneNumber Phone, isnull(@BeneficiaryCard,0) BeneficiaryCard, @IdentificationNumber PassPort, @IdentificationType  ,GETDATE() ValidityFrom,@AuditUserID AuditUserID, @ProfessionId Profession, @EducationId Education, @Email Email, 0 IsOffline, @HfID, @CurrentAddress CurrentAddress, @CurrentLocationId CurrentVillage
					SET @InsureeID = SCOPE_IDENTITY()


					INSERT INTO tblPhotos(InsureeID,CHFID,PhotoFolder,PhotoFileName,OfficerID,PhotoDate,ValidityFrom,AuditUserID)
					SELECT InsureeID,CHFID,'','',0,GETDATE(),ValidityFrom,AuditUserID from tblInsuree WHERE InsureeID = @InsureeID; 
					UPDATE tblInsuree SET PhotoID = (SELECT IDENT_CURRENT('tblPhotos')), PhotoDate=GETDATE() WHERE InsureeID = @InsureeID ;

					UPDATE tblFamilies SET InsureeID = @InsureeID WHERE FamilyID = @FamilyID

			COMMIT TRANSACTION ENROLFAMILY
			RETURN 0
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION ENROLFAMILY
			SELECT ERROR_MESSAGE()
			RETURN -1
		END CATCH


END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[uspAddInsureePolicyOffline]
(
	--@InsureeId INT,
	@PolicyId INT,
	@Activate BIT = 0
)
AS
BEGIN

	DECLARE @FamilyId INT,			
			@NewPolicyValue DECIMAL(18,2),
			@EffectiveDate DATE,
			@PolicyValue DECIMAL(18,2),
			@PolicyStage NVARCHAR(1),
			@ProdId INT,
			@AuditUserId INT,
			@isOffline BIT,
			@ErrorCode INT,
			@TotalInsurees INT,
			@MaxMember INT,
			@ThresholdMember INT,
			@Premium DECIMAL(18,2),
			@NewFamilyId INT,
			@NewPolicyId INT,
			@NewInsureeId INT
	DECLARE @Result TABLE(ErrorMessage NVARCHAR(500))
	DECLARE @tblInsureePolicy TABLE(
	InsureeId int NULL,
	PolicyId int NULL,
	EnrollmentDate date NULL,
	StartDate date NULL,
	EffectiveDate date NULL,
	ExpiryDate date NULL,
	ValidityFrom datetime NULL ,
	ValidityTo datetime NULL,
	LegacyId int NULL,
	AuditUserId int NULL,
	isOffline bit NULL,
	RowId timestamp NULL
)

----BY AMANI 19/12/2017
	--SELECT @FamilyId = FamilyID,@AuditUserId = AuditUserID FROM tblInsuree WHERE InsureeID = @InsureeId
	SELECT @FamilyId = F.FamilyID,@AuditUserId = F.AuditUserID FROM tblFamilies F
	INNER JOIN tblPolicy P ON P.FamilyID=F.FamilyID AND P.PolicyID=@PolicyId  AND F.ValidityTo IS NULL  AND P.ValidityTo IS NULL
	SELECT @isOffline = ISNULL(OfflineCHF,0)  FROM tblIMISDefaults
	SELECT @ProdId=ProdID FROM tblPolicy WHERE PolicyID=@PolicyId
	SET    @Premium=ISNULL((SELECT SUM(Amount) Amount FROM tblPremium WHERE PolicyID=@PolicyId AND ValidityTo IS NULL and isPhotoFee = 0 ),0) 
	SELECT @MaxMember = ISNULL(MemberCount,0) FROM tblProduct WHERE ProdId = @ProdId;		
	SELECT @ThresholdMember = Threshold FROM tblProduct WHERE ProdId = @ProdId;

	SELECT @PolicyStage = PolicyStage FROM tblPolicy WHERE PolicyID=@PolicyId
				
BEGIN TRY
	SAVE TRANSACTION TRYSUB	---BEGIN SAVE POINT

	--INSERT TEMPORARY FAMILY
	INSERT INTO tblFamilies(InsureeID, LocationId, Poverty, ValidityFrom, ValidityTo, LegacyID, AuditUserID, FamilyType, FamilyAddress, isOffline, Ethnicity, ConfirmationNo, ConfirmationType)
	SELECT					InsureeID, LocationId, Poverty, ValidityFrom, ValidityTo, LegacyID, AuditUserID, FamilyType, FamilyAddress, isOffline, Ethnicity, ConfirmationNo, ConfirmationType 
	FROM tblFamilies WHERE FamilyID=@FamilyId  AND ValidityTo IS NULL 
	SET @NewFamilyId = (SELECT SCOPE_IDENTITY());

	EXEC @NewPolicyValue = uspPolicyValue @FamilyId=@NewFamilyId, @PolicyStage=@PolicyStage, @ErrorCode = @ErrorCode OUTPUT;

	--INSERT TEMP POLICY
	INSERT INTO dbo.tblPolicy
           (FamilyID,EnrollDate,StartDate,EffectiveDate,ExpiryDate,PolicyStatus,PolicyValue,ProdID,OfficerID,PolicyStage,ValidityFrom,ValidityTo,LegacyID,AuditUserID,isOffline)
 SELECT		@NewFamilyId,EnrollDate,StartDate,EffectiveDate,ExpiryDate,PolicyStatus,@NewPolicyValue,ProdID,OfficerID,@PolicyStage,ValidityFrom,ValidityTo,LegacyID,AuditUserID,isOffline
  FROM dbo.tblPolicy WHERE PolicyID=@PolicyId
	SET @NewPolicyId = (SELECT SCOPE_IDENTITY());


		--SELECT InsureeID FROM tblInsuree WHERE FamilyID =@FamilyId AND ValidityTo IS NULL 	ORDER BY InsureeID ASC

		DECLARE @NewCurrentInsureeId INT =0
	
		DECLARE CurTempInsuree CURSOR FOR 
		SELECT InsureeID FROM tblInsuree WHERE FamilyID =@FamilyId AND ValidityTo IS NULL 	ORDER BY InsureeID ASC
		OPEN CurTempInsuree
		FETCH NEXT FROM CurTempInsuree INTO @NewCurrentInsureeId
		WHILE @@FETCH_STATUS = 0
		BEGIN
				INSERT INTO dbo.tblInsuree
		  (FamilyID,CHFID,LastName,OtherNames,DOB,Gender,Marital,IsHead,passport,Phone,PhotoID,PhotoDate,CardIssued,ValidityFrom,ValidityTo,LegacyID,AuditUserID,Relationship,Profession,Education,Email,isOffline,TypeOfId,HFID,CurrentAddress ,GeoLocation,CurrentVillage)
  
		SELECT   
		   @NewFamilyId,CHFID,LastName,OtherNames,DOB,Gender,Marital,IsHead,passport,Phone,PhotoID,PhotoDate,CardIssued,ValidityFrom,ValidityTo,LegacyID,AuditUserID,Relationship,Profession,Education,Email,isOffline,TypeOfId,HFID,CurrentAddress,GeoLocation,CurrentVillage
		  FROM dbo.tblInsuree WHERE InsureeID=@NewCurrentInsureeId
		  SET @NewInsureeId= (SELECT SCOPE_IDENTITY());
			SELECT @TotalInsurees = COUNT(InsureeId) FROM tblInsuree WHERE FamilyId = @NewFamilyId AND ValidityTo IS NULL 
				IF  @TotalInsurees > @MaxMember 
				GOTO CLOSECURSOR;
		
	SELECT @EffectiveDate= EffectiveDate, @PolicyValue=ISNULL(PolicyValue,0) FROM tblPolicy  WHERE PolicyID =@NewPolicyId AND ValidityTo IS NULL 
			EXEC @NewPolicyValue = uspPolicyValue @PolicyId = @NewPolicyId, @PolicyStage = @PolicyStage, @ErrorCode = @ErrorCode OUTPUT;
			--If new policy value is changed then the current insuree will not be insured
		IF @NewPolicyValue <> @PolicyValue OR @ErrorCode <> 0
		BEGIN
	UPDATE tblPolicy SET PolicyValue=@NewPolicyValue WHERE PolicyID=@NewPolicyId
		IF @Activate = 0 
			IF  @Premium < @NewPolicyValue
			BEGIN
				SET @EffectiveDate = NULL
			END
		END

		--INSERT TEMP INSUREEPOLICY
	
		INSERT INTO @tblInsureePolicy(InsureeId,PolicyId,EnrollmentDate,StartDate,EffectiveDate,ExpiryDate,ValidityFrom,AuditUserId,isOffline)
			SELECT @NewCurrentInsureeId, @PolicyId,EnrollDate,P.StartDate,@EffectiveDate,P.ExpiryDate,GETDATE(),@AuditUserId,@isOffline
			FROM tblPolicy P 
			WHERE P.PolicyID = @NewPolicyId
		

		CLOSECURSOR:
		FETCH NEXT FROM CurTempInsuree INTO @NewCurrentInsureeId
		END														
		CLOSE CurTempInsuree
		
	
		ROLLBACK TRANSACTION  TRYSUB --ROLLBACK SAVE POINT			
		SELECT * FROM @tblInsureePolicy

		--BEGIN TRY	

		--MERGE TO THE REAL TABLE


		MERGE INTO tblInsureePolicy  AS TARGET
			USING @tblInsureePolicy AS SOURCE
				ON TARGET.InsureeId = SOURCE.InsureeId
				AND TARGET.PolicyId = SOURCE.PolicyId
				AND TARGET.ValidityTo IS NULL
			WHEN MATCHED THEN 
				UPDATE SET TARGET.EffectiveDate = SOURCE.EffectiveDate
			WHEN NOT MATCHED BY TARGET THEN
				INSERT (InsureeId,PolicyId,EnrollmentDate,StartDate,EffectiveDate,ExpiryDate,ValidityFrom,AuditUserId,isOffline)
				VALUES (SOURCE.InsureeId,
						SOURCE.PolicyId, 
						SOURCE.EnrollmentDate, 
						SOURCE.StartDate, 
						SOURCE.EffectiveDate, 
						SOURCE.ExpiryDate, 
						SOURCE.ValidityFrom, 
						SOURCE.AuditUserId, 
						SOURCE.isOffline);
		--END TRY
		--BEGIN CATCH
		--	SELECT ERROR_MESSAGE();
		--	ROLLBACK TRANSACTION  TRYSUB;	
		--END CATCH
	

END TRY
BEGIN CATCH
		ROLLBACK TRANSACTION  TRYSUB;	
		SELECT @ErrorCode;
		INSERT INTO @Result(ErrorMessage) VALUES(ERROR_MESSAGE())
		SELECT * INTO TempError FROM @Result
END CATCH
	
END




GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[uspAPIEditFamily]
(
	@AuditUserID INT = -3,
	@InsuranceNumberOfHead NVARCHAR(12),
	@VillageCode NVARCHAR(8)= NULL,
	@OtherNames NVARCHAR(100) = NULL,
	@LastName NVARCHAR(100) = NULL,
	@BirthDate DATE = NULL,
	@Gender NVARCHAR(1) = NULL,
	@PovertyStatus BIT = NULL,
	@ConfirmationType NVARCHAR(1) = NULL,
	@GroupType NVARCHAR(2) = NULL,
	@ConfirmationNumber NVARCHAR(12) = NULL,
	@PermanentAddress NVARCHAR(200) = NULL,
	@MaritalStatus NVARCHAR(1) = NULL,
	@BeneficiaryCard BIT = NULL,
	@CurrentVillageCode NVARCHAR(8) = NULL,
	@CurrentAddress NVARCHAR(200) = NULL,
	@Proffesion NVARCHAR(50) = NULL,
	@Education NVARCHAR(50) = NULL,
	@PhoneNumber NVARCHAR(50) = NULL,
	@Email NVARCHAR(100) = NULL,
	@IdentificationType NVARCHAR(1) = NULL,
	@IdentificationNumber NVARCHAR(25) = NULL,
	@FSPCode NVARCHAR(8) = NULL
)

AS
BEGIN
	/*
	RESPONSE CODES
		1 - Wrong format or missing insurance number of head
		2 - Insurance number of head not found
		3 - Wrong or missing permanent village code
		4 - Wrong current village code
		5 - Wrong  gender
		6 - Wrong confirmation type
		7 - Wrong group type
		8 - Wrong marital status
		9 - Wrong education
		10 - Wrong profession
		11 - FSP code not found
		12 - Wrong identification type
		0 - Success 
		-1 Unknown Error

	*/
	

	/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	--1 - Wrong format or missing insurance number of head
	IF LEN(ISNULL(@InsuranceNumberOfHead,'')) = 0
		RETURN 1
	
	--2 - Insurance number of head not found
	IF NOT EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID = @InsuranceNumberOfHead AND ValidityTo IS NULL AND IsHead = 1)
		RETURN 2

	--3 - Wrong missing permanent village code
	IF NOT EXISTS(SELECT 1 FROM tblLocations  WHERE LocationCode = @VillageCode AND ValidityTo IS NULL AND LocationType ='V') AND  LEN(ISNULL(@VillageCode,'')) > 0
		RETURN 3

	--4 - Wrong current village code
	IF NOT EXISTS(SELECT 1 FROM tblLocations  WHERE LocationCode = @CurrentVillageCode AND ValidityTo IS NULL AND LocationType ='V') AND  LEN(ISNULL(@CurrentVillageCode,'')) > 0
		RETURN 4
	
	--5 - Wrong   gender
	IF NOT EXISTS(SELECT 1 FROM tblGender WHERE Code = @Gender) AND LEN(ISNULL(@Gender,'')) > 0
		RETURN 5
	
	--6 - Wrong confirmation type
	IF NOT EXISTS(SELECT 1 FROM tblConfirmationTypes WHERE ConfirmationTypeCode = @ConfirmationType) AND LEN(ISNULL(@ConfirmationType,'')) > 0
		RETURN 6
	
	--7 - Wrong group type
	IF NOT EXISTS(SELECT  1 FROM tblFamilyTypes WHERE FamilyTypeCode = @GroupType) AND LEN(ISNULL(@GroupType,'')) > 0
		RETURN 7

	--8 - Wrong marital status
	IF dbo.udfAPIisValidMaritalStatus(@MaritalStatus) = 0 AND LEN(ISNULL(@MaritalStatus,'')) > 0
		RETURN 8

	--9 - Wrong education
	IF NOT EXISTS(SELECT  1 FROM tblEducations WHERE Education = @Education) AND LEN(ISNULL(@Education,'')) > 0
		RETURN 9

	--10 - Wrong profession
	IF NOT EXISTS(SELECT  1 FROM tblProfessions WHERE Profession = @Proffesion) AND LEN(ISNULL(@Proffesion,'')) > 0
		RETURN 10

	--11 - FSP code not found
	IF NOT EXISTS(SELECT  1 FROM tblHF WHERE HFCode = @FSPCode AND ValidityTo IS NULL) AND LEN(ISNULL(@FSPCode,'')) > 0
		RETURN 11

	--12 - Wrong identification type
	IF NOT EXISTS(SELECT 1 FROM tblIdentificationTypes WHERE  IdentificationCode  = @IdentificationType ) AND LEN(ISNULL(@IdentificationType,'')) > 0
		RETURN 12


	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/

		/****************************************BEGIN TRANSACTION *************************/
		BEGIN TRY
			BEGIN TRANSACTION EDITROLFAMILY
			
				DECLARE @FamilyID INT,
						@InsureeID INT,
						@ProfessionId INT,
						@EducationId INT,
						@RelationId INT,
						@LocationId INT,
						@CurrentLocationId INT,
						@HfID INT,
						@DBLocationID INT = NULL,
						@DBOtherNames NVARCHAR(100) = NULL,
						@DBLastName NVARCHAR(100) = NULL,
						@DBBirthDate DATE = NULL,
						@DBGender NVARCHAR(1) = NULL,
						@DBMaritalStatus NVARCHAR(1) = NULL,
						@DBBeneficiaryCard BIT = NULL,
						@DBVillageID INT = NULL,
						@DBCurrentAddress NVARCHAR(200) = NULL,
						@DBProffesionID INT = NULL,
						@DBEducationID INT = NULL,
						@DBPhoneNumber NVARCHAR(50) = NULL,
						@DBEmail NVARCHAR(100) = NULL,
						@DBConfirmationType NVARCHAR(25) = NULL,
						@DBIdentificationNumber NVARCHAR(25) = NULL,
						@DBIdentificationType NVARCHAR(1) = NULL,
						@DBGroupType nvarchar(2) = NULL,
						@DBHFID INT = NULL,
						@DBCurrentLocationId INT=NULL
						

						SELECT @HfID = HfID FROM tblHF WHERE HFCode = @FSPCode AND ValidityTo IS NULL
						SELECT @FamilyID = FamilyID FROM tblInsuree WHERE CHFID = @InsuranceNumberOfHead AND IsHead = 1 
						SELECT @ProfessionId = ProfessionId FROM tblProfessions WHERE Profession = @Proffesion 
						SELECT @EducationId = EducationId FROM tblEducations WHERE Education = @Education
						SELECT @CurrentLocationId = LocationId FROM tblLocations WHERE LocationCode = @CurrentVillageCode AND ValidityTo IS NULL
						SELECT @LocationId = LocationId FROM tblLocations WHERE LocationCode = @VillageCode AND ValidityTo IS NULL
						SELECT @InsureeId = I.InsureeID, @DBOtherNames = OtherNames, @DBLastName= LastName, @DBBirthDate = DOB, @DBGender = Gender, @DBMaritalStatus= Marital, 
						@DBBeneficiaryCard = CardIssued, @DBCurrentLocationId = CurrentVillage, @DBCurrentAddress = CurrentAddress, @DBProffesionID = Profession, @DBEducationID =Education, 
						@DBPhoneNumber = Phone, @DBEmail = Email, @DBIdentificationNumber = passport, @DBHFID = HFID, @DBLocationID = F.LocationId, @DBConfirmationType = F.ConfirmationType,
						@DBIdentificationType = [TypeOfId], @DBGroupType = FamilyType
						FROM tblInsuree I INNER JOIN tblFamilies  F ON F.FamilyID = I.FamilyID  WHERE CHFID = @InsuranceNumberOfHead AND I.ValidityTo IS NULL AND F.ValidityTo IS NULL

						SET	@LocationId = ISNULL(@LocationId, @DBLocationID)
						SET	@OtherNames = ISNULL(@OtherNames, @DBOtherNames)
						SET	@LastName = ISNULL(@LastName, @DBLastName)
						SET	@BirthDate = ISNULL(@BirthDate, @DBBirthDate)
						SET	@Gender = ISNULL(@Gender, @DBGender)
						SET	@MaritalStatus = ISNULL(@MaritalStatus, @DBMaritalStatus)
						SET	@BeneficiaryCard = ISNULL(@BeneficiaryCard, @DBBeneficiaryCard)
						SET	@CurrentAddress = ISNULL(@CurrentAddress, @DBCurrentAddress)
						SET	@ProfessionId = ISNULL(@ProfessionId, @DBProffesionID)
						SET	@EducationId = ISNULL(@EducationId, @DBEducationID)
						SET	@PhoneNumber = ISNULL(@PhoneNumber, @DBPhoneNumber)
						SET	@Email = ISNULL(@Email, @DBEmail)
						SET	@ConfirmationType = ISNULL(@ConfirmationType, @DBConfirmationType)
						SET @IdentificationType = ISNULL(@IdentificationType,@DBIdentificationType )
						SET	@IdentificationNumber = ISNULL(@IdentificationNumber, @DBIdentificationNumber)
						SET	@HfID = ISNULL(@HfID, @DBHFID )
						SET @GroupType = ISNULL(@GroupType, @DBGroupType)
						SET @CurrentLocationId = ISNULL(@CurrentLocationId,@DBCurrentLocationId)

						INSERT INTO tblFamilies ([insureeid],[Poverty],[ConfirmationType],isOffline,[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID],FamilyType, FamilyAddress,Ethnicity,ConfirmationNo, LocationId) 
						SELECT [insureeid],[Poverty],[ConfirmationType],isOffline,[ValidityFrom],getdate() ValidityTo,FamilyID, @AuditUserID,FamilyType, FamilyAddress,Ethnicity,ConfirmationNo,LocationId FROM tblFamilies
						WHERE FamilyID = @FamilyID 
								AND ValidityTo IS NULL
						

						UPDATE tblFamilies SET LocationId = @LocationId, Poverty = @PovertyStatus, ValidityFrom = GETDATE(),AuditUserID = @AuditUserID,FamilyType = @GroupType,FamilyAddress = @PermanentAddress,ConfirmationType =@ConfirmationType,
							  ConfirmationNo = @ConfirmationNumber WHERE FamilyID = @FamilyID AND ValidityTo IS NULL

						--Insert Insuree History
						INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,[ValidityTo],legacyId,[Relationship],[Profession],[Education],[Email],[TypeOfId],[HFID], [CurrentAddress], [GeoLocation], [CurrentVillage]) 
						SELECT	I.[FamilyID],I.[CHFID],I.[LastName],I.[OtherNames],I.[DOB],I.[Gender],I.[Marital],I.[IsHead],I.[passport],I.[Phone],I.[PhotoID],I.[PhotoDate],I.[CardIssued],I.isOffline,I.[AuditUserID],I.[ValidityFrom] ,GETDATE() ValidityTo,I.InsureeID,I.[Relationship],I.[Profession],I.[Education],I.[Email] ,I.[TypeOfId],I.[HFID], I.[CurrentAddress], I.[GeoLocation], [CurrentVillage] FROM tblInsuree I
						WHERE I.CHFID = @InsuranceNumberOfHead AND  I.ValidityTo IS NULL
					
						UPDATE tblInsuree  SET [LastName] = @LastName, [OtherNames] = @OtherNames,[DOB] = @BirthDate, [Gender] = @Gender,[Marital] = @MaritalStatus, [TypeOfId]  = @IdentificationType , [passport] = @IdentificationNumber,[Phone] = @PhoneNumber,[CardIssued] = ISNULL(@BeneficiaryCard,0),[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID , [Profession] = @ProfessionId, [Education] = @EducationId,[Email] = @Email ,HFID = @HFID, CurrentAddress = @CurrentAddress, CurrentVillage = @CurrentLocationId
						WHERE InsureeID = @InsureeId AND  ValidityTo IS NULL 


			COMMIT TRANSACTION EDITROLFAMILY
			RETURN 0
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION EDITROLFAMILY
			SELECT ERROR_MESSAGE()
			RETURN -1
		END CATCH
END




GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROCEDURE [dbo].[uspAPIEnterContribution]
(
	@AuditUserID INT = -3,
	@InsuranceNumber NVARCHAR(12),
	@Payer NVARCHAR(100),
	@PaymentDate DATE,
	@ProductCode NVARCHAR(8),
	@ContributionAmount DECIMAL(18,2),
	@ReceiptNo NVARCHAR(50),
	@PaymentType CHAR(1),
	@ContributionCategory CHAR(1) = NULL,
	@ReactionType BIT
	
)

AS
BEGIN
	/*
	RESPONSE CODES
		1-Wrong format or missing insurance number 
		2-Insurance number of not found
		3- Wrong or missing  product code (policy of the product code not assigned to the family/group)
		4- Wrong or missing payment date
		5- Wrong contribution category
		6-Wrong or missing payment type
		7-Wrong or missing payer
		8-Missing receipt no.
		9-Duplicated receipt no.
		0 - all ok
		-1 Unknown Error

	*/


	/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	DECLARE @tblPaymentType TABLE(PaymentTypeCode NVARCHAR(1))
		DECLARE @isValid BIT
		INSERT INTO @tblPaymentType(PaymentTypeCode) 
		VALUES ('B'),('C'),('F'),('M')
	--1-Wrong format or missing insurance number 
	IF LEN(ISNULL(@InsuranceNumber,'')) = 0
		RETURN 1
	
	--2-Insurance number of not found
	IF NOT EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID = @InsuranceNumber AND ValidityTo IS NULL)
		RETURN 2

	--3- Wrong or missing product code (not existing or not applicable to the family/group)
	IF LEN(ISNULL(@ProductCode,'')) = 0
		RETURN 3

	IF NOT EXISTS(SELECT F.LocationId, V.LocationName, V.LocationType, D.ParentLocationId, PR.ProductCode FROM tblInsuree I
		INNER JOIN tblFamilies F ON F.FamilyID = I.FamilyID
		INNER JOIN tblLocations V ON V.LocationId = F.LocationId
		INNER JOIN tblLocations M ON M.LocationId = V.ParentLocationId
		INNER JOIN tblLocations D ON D.LocationId = M.ParentLocationId
		INNER JOIN tblProduct PR ON (PR.LocationId = D.LocationId) OR PR.LocationId =  D.ParentLocationId OR PR.LocationId IS NULL 
		WHERE
		F.ValidityTo IS NULL
		AND V.ValidityTo IS NULL
		AND PR.ValidityTo IS NULL AND PR.ProductCode =@ProductCode
		AND I.CHFID = @InsuranceNumber AND I.ValidityTo IS NULL AND I.IsHead = 1)
		RETURN 3

	--4- Wrong or missing payment date
	IF NULLIF(@PaymentDate,'') IS NULL
		RETURN 4

	--5- Wrong contribution category
	IF LEN(ISNULL(@ContributionCategory,'')) > 0
	IF NOT (@ContributionCategory = 'C' OR @ContributionCategory  = 'P') 
		 RETURN 5

		 --6-Wrong or missing payment type
	IF NOT EXISTS(SELECT 1 FROM @tblPaymentType WHERE PaymentTypeCode = @PaymentType) 
		 RETURN 6

	--7-Wrong or missing payer
	IF NOT EXISTS(SELECT 1 FROM tblPayer WHERE PayerName = @Payer AND ValidityTo IS NULL)
		RETURN 7
	
	--8-Missing receipt no.
	IF NULLIF(@ReceiptNo,'') IS NULL
		 RETURN 8

	--9-Duplicated receipt no.
	IF EXISTS(SELECT 1 FROM tblPolicy PL 
				INNER JOIN tblPremium PR ON PL.PolicyID = PR.PolicyID 
				INNER JOIN tblProduct Prod ON PL.ProdId = Prod.ProdId
				WHERE PL.ValidityTo IS NULL AND PR.ValidityTo IS NULL
				AND PR.Amount = @ContributionAmount AND PR.Receipt = @ReceiptNo AND Prod.ProductCode = @ProductCode)
		RETURN 9


	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/

		/****************************************BEGIN TRANSACTION *************************/
		BEGIN TRY
			BEGIN TRANSACTION ENTERCONTRIBUTION
			
				DECLARE @tblPeriod TABLE(startDate DATE, expiryDate DATE, HasCycle  BIT)
				DECLARE @FamilyId INT = 0,
				@PolicyValue DECIMAL(18, 4),
				@PaidAmount DECIMAL(18, 4),
				@PayerID INT,
				@PolicyStage CHAR(1),
				@EnrollmentDate DATE,
				@EffectiveDate DATE,
				@ErrorCode INT,
				@PolicyStatus INT,
				@PolicyId INT,
				@ProdId INT,
				@Active TINYINT=2,
				@Idle TINYINT=1,
				@OfficerID INT,
				@isPhotoFee BIT,
				@Installment INT, 
				@MaxInstallments INT,
				@LastDate DATE,
				@PremiumPayCount as INT 
				

				SELECT @ProdId = ProdID , @MaxInstallments = MaxInstallments  FROM tblProduct  WHERE ValidityTo IS NULL AND ProductCode = @ProductCode
				--find the right policy and family
				select @FamilyId = FamilyID  from tblInsuree where CHFID = @InsuranceNumber and ValidityTo IS NULL
				SELECT TOP 1 @PolicyId = PL.PolicyID, @PolicyStatus = PolicyStatus, @EnrollmentDate = PL.EnrollDate, @PolicyStage = PL.PolicyStage  FROM tblPolicy PL   WHERE FamilyID = @FamilyId AND PL.ValidityTo IS NULL AND PL.ProdID = @ProdId AND PolicyStatus = 1 ORDER BY PolicyStatus ASC,PolicyID DESC  
				
				DECLARE  @MaxDate TABLE (LastDate  Date) 
				INSERT @MaxDate (LastDate)
				EXECUTE  [dbo].[uspLastDateForPayment] 
				   @PolicyId
				
				SELECT @Installment = COUNT(PremiumID) from tblPremium WHERE PolicyID = @PolicyID and ValidityTo IS NULL
				SET @Installment = ISNULL(@Installment,0) + 1 

				SELECT @LastDate = LastDate FROM @MaxDate  
				
				SELECT @PayerID = PayerID FROM tblPayer WHERE ValidityTo IS NULL AND PayerName = @Payer
				
				IF ISNULL(@ContributionCategory,'') = 'P'
					SET @isPhotoFee = 1
				ELSE
					SET @isPhotoFee = 0

				INSERT INTO tblPremium(PolicyID,PayerID,Amount,Receipt,PayDate,PayType,ValidityFrom,AuditUserID,isPhotoFee)
				SELECT @PolicyId,@PayerID,ISNULL(@ContributionAmount,0),@ReceiptNo, @PaymentDate, @PaymentType,GETDATE(),@AuditUserId,@isPhotoFee

				EXEC @PolicyValue = uspPolicyValue @FamilyId, @ProdId, 0, @PolicyStage, @EnrollmentDate, 0, @ErrorCode OUTPUT;
					
				
				SELECT @PaidAmount = ISNULL(SUM(Amount),0) FROM tblPremium WHERE PolicyId = @PolicyId and ValidityTo IS NULL AND isPhotoFee = 0 
				IF ((@PaidAmount >= @PolicyValue AND ( @Installment <= @MaxInstallments) AND (@PaymentDate <= @LastDate ) ) OR @ReactionType = 1) 
				BEGIN
					IF @PolicyStatus = 1
					BEGIN
						-- only activate if the policy was not yet activated (do not change anything on already suspended or expired policies 
						SET @PolicyStatus = @Active
						SET @EffectiveDate = @PaymentDate
						
						UPDATE tblInsureePolicy SET EffectiveDate = @EffectiveDate WHERE PolicyID = @PolicyId
						UPDATE tblPolicy SET PolicyStatus = @PolicyStatus, EffectiveDate = @EffectiveDate WHERE PolicyID = @PolicyId	
					END

				END
				--ELSE 
				--BEGIN
				--	--now check if we have problems in installments OR GracePeriod 
				--	SET @PolicyStatus = @Idle
				--	SET @EffectiveDate = NULL
				--END
			
			COMMIT TRANSACTION ENTERCONTRIBUTION
			RETURN 0
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION ENTERCONTRIBUTION
			SELECT ERROR_MESSAGE()
			RETURN -1
		END CATCH
END



GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROCEDURE [dbo].[uspAPIEnterPolicy]
(
	@AuditUserID INT = -3,
	@InsuranceNumber NVARCHAR(12),
	@EnrollmentDate DATE,
	@ProductCode NVARCHAR(8),
	@EnrollmentOfficerCode NVARCHAR(8)
)

AS
BEGIN
	/*
	RESPONSE CODES
		1-Wrong format or missing insurance number 
		2-Insurance number of not found
		3- Wrong or missing product code (not existing or not applicable to the family/group)
		4- Wrong or missing enrolment date
		5- Wrong or missing enrolment officer code (not existing or not applicable to the family/group)
		0 - all ok
		-1 Unknown Error

	*/


/**********************************************************************************************************************
			VALIDATION STARTS
	*********************************************************************************************************************/
	--1-Wrong format or missing insurance number 
	IF LEN(ISNULL(@InsuranceNumber,'')) = 0
		RETURN 1
	
	--2-Insurance number of not found
	IF NOT EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID = @InsuranceNumber AND ValidityTo IS NULL)
		RETURN 2

	--3- Wrong or missing product code (not existing or not applicable to the family/group)
	IF LEN(ISNULL(@ProductCode,'')) = 0
		RETURN 3

	IF NOT EXISTS(SELECT F.LocationId, V.LocationName, V.LocationType, D.ParentLocationId, PR.ProductCode FROM tblInsuree I
		INNER JOIN tblFamilies F ON F.FamilyID = I.FamilyID
		INNER JOIN tblLocations V ON V.LocationId = F.LocationId
		INNER JOIN tblLocations M ON M.LocationId = V.ParentLocationId
		INNER JOIN tblLocations D ON D.LocationId = M.ParentLocationId
		INNER JOIN tblProduct PR ON (PR.LocationId = D.LocationId) OR PR.LocationId =  D.ParentLocationId OR PR.LocationId IS NULL 
		WHERE
		F.ValidityTo IS NULL
		AND V.ValidityTo IS NULL
		AND PR.ValidityTo IS NULL AND PR.ProductCode =@ProductCode AND PR.DateTo >= GETDATE()
		AND I.CHFID = @InsuranceNumber AND I.ValidityTo IS NULL AND I.IsHead = 1)
		RETURN 3

	--4- Wrong or missing enrolment date
	IF NULLIF(@EnrollmentDate,'') IS NULL
		RETURN 4

	--5- Wrong or missing enrolment officer code (not existing or not applicable to the family/group)
	IF LEN(ISNULL(@EnrollmentOfficerCode,'')) = 0
		RETURN 5
	
		IF NOT EXISTS(SELECT 1 FROM tblInsuree I 
						INNER JOIN tblFamilies F ON F.FamilyID = I.FamilyID
						INNER JOIN tblLocations V ON V.LocationId = F.LocationId
						INNER JOIN tblLocations M ON M.LocationId = V.ParentLocationId
						INNER JOIN tblLocations D ON D.LocationId = M.ParentLocationId
						INNER JOIN tblOfficer O ON O.LocationId = D.LocationId
						WHERE 
						I.CHFID = @InsuranceNumber AND O.Code= @EnrollmentOfficerCode
						AND I.ValidityTo IS NULL
						AND F.ValidityTo IS NULL
						AND V.ValidityTo IS NULL
						AND O.ValidityTo IS NULL)
		 RETURN 5

	/**********************************************************************************************************************
			VALIDATION ENDS
	*********************************************************************************************************************/

		/****************************************BEGIN TRANSACTION *************************/
		BEGIN TRY
			BEGIN TRANSACTION ENTERPOLICY
			
				DECLARE @tblPeriod TABLE(startDate DATE, expiryDate DATE, HasCycle  BIT)
				DECLARE @FamilyId INT = 0,
				@PolicyValue DECIMAL(18, 4),
				@ProdId INT,
				@PolicyStage CHAR(1) = N'N',
				@StartDate DATE,
				@ExpiryDate DATE,
				@EffectiveDate DATE,
				@ErrorCode INT,
				@PolicyStatus INT,
				@PolicyId INT,
				@Active TINYINT=2,
				@Idle TINYINT=1,
				@OfficerID INT,
				@HasCycle BIT

				SELECT @FamilyId = FamilyID FROM tblInsuree WHERE CHFID = @InsuranceNumber  AND ValidityTo IS NULL
				SELECT @ProdId = ProdID FROM tblProduct WHERE ProductCode = @ProductCode  AND ValidityTo IS NULL
				INSERT INTO @tblPeriod(StartDate, ExpiryDate, HasCycle)
				EXEC uspGetPolicyPeriod @ProdId, @EnrollmentDate, @HasCycle OUTPUT, @PolicyStage;
				EXEC @PolicyValue = uspPolicyValue @FamilyId, @ProdId, 0, @PolicyStage, @EnrollmentDate, 0, @ErrorCode OUTPUT;
				SELECT @StartDate = startDate FROM @tblPeriod
				SELECT @ExpiryDate = expiryDate FROM @tblPeriod
				SELECT @OfficerID = OfficerID FROM tblOfficer WHERE Code = @EnrollmentOfficerCode AND ValidityTo IS NULL

					INSERT INTO tblPolicy(FamilyID,EnrollDate,StartDate,EffectiveDate,ExpiryDate,PolicyStatus,PolicyValue,ProdID,OfficerID,PolicyStage,ValidityFrom,AuditUserID, isOffline)
					SELECT	 @FamilyId,@EnrollmentDate,@StartDate,@EffectiveDate,@ExpiryDate,@Idle,@PolicyValue,@ProdId,@OfficerID,@PolicyStage,GETDATE(),@AuditUserId, 0 isOffline 
					SET @PolicyId = SCOPE_IDENTITY()

	

							DECLARE @InsureeId INT
							DECLARE CurNewPolicy CURSOR FOR SELECT I.InsureeID FROM tblInsuree I 
							INNER JOIN tblFamilies F ON I.FamilyID = F.FamilyID 
							INNER JOIN tblPolicy P ON P.FamilyID = F.FamilyID 
							WHERE P.PolicyId = @PolicyId 
							AND I.ValidityTo IS NULL 
							AND F.ValidityTo IS NULL
							AND P.ValidityTo IS NULL
							OPEN CurNewPolicy;
							FETCH NEXT FROM CurNewPolicy INTO @InsureeId;
							WHILE @@FETCH_STATUS = 0
							BEGIN
								EXEC uspAddInsureePolicy @InsureeId;
								FETCH NEXT FROM CurNewPolicy INTO @InsureeId;
							END
							CLOSE CurNewPolicy;
							DEALLOCATE CurNewPolicy; 

			COMMIT TRANSACTION ENTERPOLICY
			RETURN 0
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION ENTERPOLICY
			SELECT ERROR_MESSAGE()
			RETURN -1
		END CATCH
END


GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[uspAcknowledgeControlNumberRequest]
(
	
	@XML XML
)
AS
BEGIN

	DECLARE
	@PaymentID INT,
    @Success BIT,
    @Comment  NVARCHAR(MAX)
	SELECT @PaymentID = NULLIF(T.H.value('(PaymentID)[1]','INT'),''),
		   @Success = NULLIF(T.H.value('(Success)[1]','BIT'),''),
		   @Comment = NULLIF(T.H.value('(Comment)[1]','NVARCHAR(MAX)'),'')
	FROM @XML.nodes('ControlNumberAcknowledge') AS T(H)

				BEGIN TRY

				UPDATE tblPayment SET PaymentStatus =  CASE @Success WHEN 1 THEN 2 ELSE-3 END, RejectedReason = CASE @Success WHEN 0 THEN  @Comment ELSE NULL END,  ValidityFrom = GETDATE(),AuditedUserID =-1 WHERE PaymentID = @PaymentID  AND ValidityTo IS NULL 

				RETURN 0
			END TRY
			BEGIN CATCH
				ROLLBACK TRAN GETCONTROLNUMBER
				SELECT ERROR_MESSAGE()
				RETURN -1
			END CATCH
	

	
END






GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[uspSentRequestGetControlNumber]
(
	
	@PaymentID INT= 0
)
AS
BEGIN

			BEGIN TRY
				
	UPDATE tblControlNumber SET RequestedDate = GETDATE(), ValidityFrom = GETDATE(),AuditedUserID =-1 WHERE PaymentID = @PaymentID  AND ValidityTo IS NULL 
	UPDATE tblPayment SET PaymentStatus = 1, ValidityFrom = GETDATE(),AuditedUserID =-1 WHERE PaymentID = @PaymentID  AND ValidityTo IS NULL 

	RETURN 0
			END TRY
			BEGIN CATCH
				ROLLBACK TRAN GETCONTROLNUMBER
				SELECT ERROR_MESSAGE()
				RETURN -1
			END CATCH
	

	
END





GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[uspRequestGetControlNumber]
(
	
	@PaymentID INT= 0,
	@RequestOrigin NVARCHAR(50) = NULL,
	@Failed BIT = 0
)
AS
BEGIN

		IF NOT EXISTS(SELECT 1 FROM tblPayment WHERE PaymentID = @PaymentID)
		RETURN 1 --Payment Does not exists
	
	IF EXISTS(SELECT 1 FROM tblControlNumber  WHERE PaymentID = @PaymentID  AND [Status] = 0 AND ValidityTo IS NULL)
		RETURN 2 --Request Already exists

			BEGIN TRY
				BEGIN TRAN GETCONTROLNUMBER
						INSERT INTO [dbo].[tblControlNumber]
						 ([RequestedDate],[RequestOrigin],[Status],[ValidityFrom],[AuditedUserID],[PaymentID])
							 SELECT GETDATE(), @RequestOrigin,0, GETDATE(), -1, @PaymentID
							 IF @Failed = 0
							 UPDATE tblPayment SET PaymentStatus =1 WHERE PaymentID = @PaymentID
				COMMIT TRAN GETCONTROLNUMBER
				RETURN 0
			END TRY
			BEGIN CATCH
				ROLLBACK TRAN GETCONTROLNUMBER
				SELECT ERROR_MESSAGE()
				RETURN -1
			END CATCH
	

	
END



GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[uspPolicyValueProxyFamily]
(
	@ProductCode NVARCHAR(8),			
	@AdultMembers INT ,
	@ChildMembers INT ,
	@OAdultMembers INT =0,
	@OChildMembers INT = 0
)
AS

/*
********ERROR CODE***********
-1	:	Policy does not exists at the time of enrolment
-2	:	Policy was deleted at the time of enrolment

*/

BEGIN

	DECLARE @LumpSum DECIMAL(18,2) = 0,
			@PremiumAdult DECIMAL(18,2) = 0,
			@PremiumChild DECIMAL(18,2) = 0,
			@RegistrationLumpSum DECIMAL(18,2) = 0,
			@RegistrationFee DECIMAL(18,2) = 0,
			@GeneralAssemblyLumpSum DECIMAL(18,2) = 0,
			@GeneralAssemblyFee DECIMAL(18,2) = 0,
			@Threshold SMALLINT = 0,
			@MemberCount INT = 0,
			@Registration DECIMAL(18,2) = 0,
			@GeneralAssembly DECIMAL(18,2) = 0,
			@Contribution DECIMAL(18,2) = 0,
			@PolicyValue DECIMAL(18,2) = 0,
			@ExtraAdult INT = 0,
			@ExtraChild INT = 0,
			@AddonAdult DECIMAL(18,2) = 0,
			@AddonChild DECIMAL(18,2) = 0,
			@DiscountPeriodR INT = 0,
			@DiscountPercentR DECIMAL(18,2) =0,
			@DiscountPeriodN INT = 0,
			@DiscountPercentN DECIMAL(18,2) =0,
			@ExpiryDate DATE,
			@ProdId INT =0,
			@PolicyStage NVARCHAR(1) ='N',
			@ErrorCode INT=0,
			@ValidityTo DATE = NULL,
			@LegacyId INT = NULL,
			@EnrollDate DATE = GETDATE();


			SET @ProdId = ( SELECT ProdID FROM tblProduct WHERE ProductCode = @ProductCode AND ValidityTo IS NULL	)
	


	/*--Get all the required fiedls from product (Valide product at the enrollment time)--*/
		SELECT TOP 1 @LumpSum = ISNULL(LumpSum,0),@PremiumAdult = ISNULL(PremiumAdult,0),@PremiumChild = ISNULL(PremiumChild,0),@RegistrationLumpSum = ISNULL(RegistrationLumpSum,0),
		@RegistrationFee = ISNULL(RegistrationFee,0),@GeneralAssemblyLumpSum = ISNULL(GeneralAssemblyLumpSum,0), @GeneralAssemblyFee = ISNULL(GeneralAssemblyFee,0), 
		@Threshold = ISNULL(Threshold ,0),@MemberCount = ISNULL(MemberCount,0), @ValidityTo = ValidityTo, @LegacyId = LegacyID, @DiscountPeriodR = ISNULL(RenewalDiscountPeriod, 0), @DiscountPercentR = ISNULL(RenewalDiscountPerc,0)
		, @DiscountPeriodN = ISNULL(EnrolmentDiscountPeriod, 0), @DiscountPercentN = ISNULL(EnrolmentDiscountPerc,0)
		FROM tblProduct 
		WHERE (ProdID = @ProdId OR LegacyID = @ProdId)
		AND CONVERT(DATE,ValidityFrom,103) <= @EnrollDate
		ORDER BY ValidityFrom Desc

		IF @@ROWCOUNT = 0	--No policy found
			SET @ErrorCode = -1
		IF NOT @ValidityTo IS NULL AND @LegacyId IS NULL	--Policy is deleted by the time of enrollment
			SET @ErrorCode = -2
			

	

	--Get extra members in family
		IF @Threshold > 0 AND @AdultMembers > @Threshold
			SET @ExtraAdult = @AdultMembers - @Threshold
		IF @Threshold > 0 AND @ChildMembers > (@Threshold - @AdultMembers + @ExtraAdult )
					SET @ExtraChild = @ChildMembers - ((@Threshold - @AdultMembers + @ExtraAdult))
			

	--Get the Contribution
		IF @LumpSum > 0
			SET @Contribution = @LumpSum
		ELSE
			SET @Contribution = (@AdultMembers * @PremiumAdult) + (@ChildMembers * @PremiumChild)

	--Get the Assembly
		IF @GeneralAssemblyLumpSum > 0
			SET @GeneralAssembly = @GeneralAssemblyLumpSum
		ELSE
			SET @GeneralAssembly = (@AdultMembers + @ChildMembers + @OAdultMembers + @OChildMembers) * @GeneralAssemblyFee;

	--Get the Registration
		IF @PolicyStage = N'N'	--Don't calculate if it's renewal
		BEGIN
			IF @RegistrationLumpSum > 0
				SET @Registration = @RegistrationLumpSum
			ELSE
				SET @Registration = (@AdultMembers + @ChildMembers  + @OAdultMembers + @OChildMembers) * @RegistrationFee;
		END

	/* Any member above the maximum member count  or with excluded relationship calculate the extra addon amount */

		SET @AddonAdult = (@ExtraAdult + @OAdultMembers) * @PremiumAdult;
		SET @AddonChild = (@ExtraChild + @OChildMembers) * @PremiumChild;

		SET @Contribution += @AddonAdult + @AddonChild;
		
		--Line below was a mistake, All adults and children are already included in GeneralAssembly and Registration
		--SET @GeneralAssembly += (@OAdultMembers + @OChildMembers + @ExtraAdult + @ExtraChild) * @GeneralAssemblyFee;
		
		--IF @PolicyStage = N'N'
		--	SET @Registration += (@OAdultMembers + @OChildMembers + @ExtraAdult + @ExtraChild) * @RegistrationFee;


	SET @PolicyValue = @Contribution + @GeneralAssembly + @Registration;


	--The total policy value is calculated, So if the enroldate is earlier than the discount period then apply discount
	DECLARE @HasCycle BIT
	DECLARE @tblPeriod TABLE(StartDate DATE, ExpiryDate DATE, HasCycle BIT)
	INSERT INTO @tblPeriod(StartDate, ExpiryDate, HasCycle)
	EXEC uspGetPolicyPeriod @ProdId, @EnrollDate, @HasCycle OUTPUT, @PolicyStage;

	DECLARE @StartDate DATE =(SELECT StartDate FROM @tblPeriod);


	DECLARE @MinDiscountDateR DATE,
			@MinDiscountDateN DATE

	IF @PolicyStage = N'N'
	BEGIN
		SET @MinDiscountDateN = DATEADD(MONTH,-(@DiscountPeriodN),@StartDate);
		IF @EnrollDate <= @MinDiscountDateN AND @HasCycle = 1
			SET @PolicyValue -=  (@PolicyValue * 0.01 * @DiscountPercentN);
	END
	ELSE IF @PolicyStage  = N'R'
	BEGIN
		DECLARE @PreviousExpiryDate DATE = NULL

	
		BEGIN
			SET @PreviousExpiryDate = @StartDate;
		END

		SET @MinDiscountDateR = DATEADD(MONTH,-(@DiscountPeriodR),@PreviousExpiryDate);
		IF @EnrollDate <= @MinDiscountDateR
			SET @PolicyValue -=  (@PolicyValue * 0.01 * @DiscountPercentR);
	END

	SELECT @PolicyValue PolicyValue;

	

	
	RETURN @PolicyValue;

END





GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[uspConsumeEnrollments](
	--@File NVARCHAR(300),
	@XML XML,
	@FamilySent INT = 0 OUTPUT ,
	@FamilyImported INT = 0 OUTPUT,
	@FamiliesUpd INT =  0 OUTPUT,  
	@FamilyRejected INT = 0 OUTPUT,
	@InsureeSent INT = 0 OUTPUT,
	@InsureeUpd INT =0 OUTPUT,
	@InsureeImported INT = 0 OUTPUT ,
	@PolicySent INT = 0 OUTPUT,
	@PolicyImported INT = 0 OUTPUT,
	@PolicyRejected INT = 0 OUTPUT,
	@PolicyChanged INT = 0 OUTPUT,
	@PremiumSent INT = 0 OUTPUT,
	@PremiumImported INT = 0 OUTPUT,
	@PremiumRejected INT =0 OUTPUT
	
	)
	AS
	BEGIN

	/*=========ERROR CODES==========
	-400	:Uncaught exception
	0	:	All okay
	-1	:	Given family has no HOF
	-2	:	Insurance number of the HOF already exists
	-3	:	Duplicate Insurance number found
	-4	:	Duplicate receipt found
	
	*/


	DECLARE @Query NVARCHAR(500)
	--DECLARE @XML XML
	DECLARE @tblFamilies TABLE(FamilyId INT,InsureeId INT, CHFID NVARCHAR(12),  LocationId INT,Poverty NVARCHAR(1),FamilyType NVARCHAR(2),FamilyAddress NVARCHAR(200), Ethnicity NVARCHAR(1), ConfirmationNo NVARCHAR(12), ConfirmationType NVARCHAR(3), isOffline BIT, NewFamilyId INT)
	DECLARE @tblInsuree TABLE(InsureeId INT,FamilyId INT,CHFID NVARCHAR(12),LastName NVARCHAR(100),OtherNames NVARCHAR(100),DOB DATE,Gender CHAR(1),Marital CHAR(1),IsHead BIT,Passport NVARCHAR(25),Phone NVARCHAR(50),CardIssued BIT,Relationship SMALLINT,Profession SMALLINT,Education SMALLINT,Email NVARCHAR(100), TypeOfId NVARCHAR(1), HFID INT,CurrentAddress NVARCHAR(200),GeoLocation NVARCHAR(200),CurVillage INT,isOffline BIT,PhotoPath NVARCHAR(100), NewFamilyId INT, NewInsureeId INT)
	DECLARE @tblPolicy TABLE(PolicyId INT,FamilyId INT,EnrollDate DATE,StartDate DATE,EffectiveDate DATE,ExpiryDate DATE,PolicyStatus TINYINT,PolicyValue DECIMAL(18,2),ProdId INT,OfficerId INT,PolicyStage CHAR(1), isOffline BIT, NewFamilyId INT, NewPolicyId INT)
	DECLARE @tblInureePolicy TABLE(PolicyId INT,InsureeId INT,EffectiveDate DATE, NewInsureeId INT, NewPolicyId INT)
	DECLARE @tblPremium TABLE(PremiumId INT,PolicyId INT,PayerId INT,Amount DECIMAL(18,2),Receipt NVARCHAR(50),PayDate DATE,PayType CHAR(1),isPhotoFee BIT, NewPolicyId INT)
	DECLARE @tblRejectedFamily TABLE(FamilyID INT)
	DECLARE @tblRejectedInsuree TABLE(InsureeID INT)
	DECLARE @tblRejectedPolicy TABLE(PolicyId INT)
	DECLARE @tblRejectedPremium TABLE(PremiumId INT)


	DECLARE @tblResult TABLE(Result NVARCHAR(Max))
	DECLARE @tblIds TABLE(OldId INT, [NewId] INT)

	BEGIN TRY

		--SET @Query = (N'SELECT @XML = CAST(X as XML) FROM OPENROWSET(BULK  '''+ @File +''' ,SINGLE_BLOB) AS T(X)')

		--EXECUTE SP_EXECUTESQL @Query,N'@XML XML OUTPUT',@XML OUTPUT


		--GET ALL THE FAMILY FROM THE XML
		INSERT INTO @tblFamilies(FamilyId,InsureeId,CHFID, LocationId,Poverty,FamilyType,FamilyAddress,Ethnicity, ConfirmationNo,ConfirmationType, isOffline)
		SELECT 
		T.F.value('(FamilyId)[1]','INT'),
		T.F.value('(InsureeId)[1]','INT'),
		T.F.value('(HOFCHFID)[1]','NVARCHAR(12)'),
		T.F.value('(LocationId)[1]','INT'),
		T.F.value('(Poverty)[1]','BIT'),
		NULLIF(T.F.value('(FamilyType)[1]','NVARCHAR(2)'),''),
		T.F.value('(FamilyAddress)[1]','NVARCHAR(200)'),
		T.F.value('(Ethnicity)[1]','NVARCHAR(1)'),
		T.F.value('(ConfirmationNo)[1]','NVARCHAR(12)'),
		NULLIF(T.F.value('(ConfirmationType)[1]','NVARCHAR(3)'),''),
		T.F.value('(isOffline)[1]','BIT')
		FROM @XML.nodes('Enrolment/Families/Family') AS T(F)


		--Get total number of families sent via XML
		SELECT @FamilySent = COUNT(1) FROM @tblFamilies

		--GET ALL THE INSUREES FROM XML
		INSERT INTO @tblInsuree(InsureeId,FamilyId,CHFID,LastName,OtherNames,DOB,Gender,Marital,IsHead,Passport,Phone,CardIssued,Relationship,Profession,Education,Email, TypeOfId, HFID, CurrentAddress, GeoLocation, CurVillage, isOffline,PhotoPath)
		SELECT
		T.I.value('(InsureeId)[1]','INT'),
		T.I.value('(FamilyId)[1]','INT'),
		T.I.value('(CHFID)[1]','NVARCHAR(12)'),
		T.I.value('(LastName)[1]','NVARCHAR(100)'),
		T.I.value('(OtherNames)[1]','NVARCHAR(100)'),
		T.I.value('(DOB)[1]','DATE'),
		T.I.value('(Gender)[1]','CHAR(1)'),
		T.I.value('(Marital)[1]','CHAR(1)'),
		T.I.value('(isHead)[1]','BIT'),
		T.I.value('(IdentificationNumber)[1]','NVARCHAR(25)'),
		T.I.value('(Phone)[1]','NVARCHAR(50)'),
		T.I.value('(CardIssued)[1]','BIT'),
		NULLIF(T.I.value('(Relationship)[1]','SMALLINT'),''),
		NULLIF(T.I.value('(Profession)[1]','SMALLINT'),''),
		NULLIF(T.I.value('(Education)[1]','SMALLINT'),''),
		T.I.value('(Email)[1]','NVARCHAR(100)'),
		NULLIF(T.I.value('(TypeOfId)[1]','NVARCHAR(1)'),''),
		NULLIF(T.I.value('(HFID)[1]','INT'),''),
		T.I.value('(CurrentAddress)[1]','NVARCHAR(200)'),
		T.I.value('(GeoLocation)[1]','NVARCHAR(200)'),
		NULLIF(T.I.value('(CurVillage)[1]','INT'),''),
		T.I.value('(isOffline)[1]','BIT'),
		T.I.value('(PhotoPath)[1]','NVARCHAR(100)')
		FROM @XML.nodes('Enrolment/Insurees/Insuree') AS T(I)

		--Get total number of Insurees sent via XML
		SELECT @InsureeSent = COUNT(1) FROM @tblInsuree

		--GET ALL THE POLICIES FROM XML
		INSERT INTO @tblPolicy(PolicyId,FamilyId,EnrollDate,StartDate,EffectiveDate,ExpiryDate,PolicyStatus,PolicyValue,ProdId,OfficerId,PolicyStage,isOffline)
		SELECT 
		T.P.value('(PolicyId)[1]','INT'),
		T.P.value('(FamilyId)[1]','INT'),
		T.P.value('(EnrollDate)[1]','DATE'),
		T.P.value('(StartDate)[1]','DATE'),
		T.P.value('(EffectiveDate)[1]','DATE'),
		T.P.value('(ExpiryDate)[1]','DATE'),
		T.P.value('(PolicyStatus)[1]','TINYINT'),
		T.P.value('(PolicyValue)[1]','DECIMAL(18,2)'),
		T.P.value('(ProdId)[1]','INT'),
		T.P.value('(OfficerId)[1]','INT'),
		T.P.value('(PolicyStage)[1]','CHAR(1)'),
		T.P.value('(isOffline)[1]','BIT')
		FROM @XML.nodes('Enrolment/Policies/Policy') AS T(P)

		--Get total number of Policies sent via XML
		SELECT @PolicySent = COUNT(1) FROM @tblPolicy
			
		--GET INSUREEPOLICY
		INSERT INTO @tblInureePolicy(PolicyId,InsureeId,EffectiveDate)
		SELECT 
		T.P.value('(PolicyId)[1]','INT'),
		T.P.value('(InsureeId)[1]','INT'),
		NULLIF(T.P.value('(EffectiveDate)[1]','DATE'),'')
		FROM @XML.nodes('Enrolment/InsureePolicies/InsureePolicy') AS T(P)

		--GET ALL THE PREMIUMS FROM XML
		INSERT INTO @tblPremium(PremiumId,PolicyId,PayerId,Amount,Receipt,PayDate,PayType,isPhotoFee)
		SELECT
		T.PR.value('(PremiumId)[1]','INT'),
		T.PR.value('(PolicyId)[1]','INT'),
		NULLIF(T.PR.value('(PayerId)[1]','INT'),0),
		T.PR.value('(Amount)[1]','DECIMAL(18,2)'),
		T.PR.value('(Receipt)[1]','NVARCHAR(50)'),
		T.PR.value('(PayDate)[1]','DATE'),
		T.PR.value('(PayType)[1]','CHAR(1)'),
		T.PR.value('(isPhotoFee)[1]','BIT')
		FROM @XML.nodes('Enrolment/Premiums/Premium') AS T(PR)

		--Get total number of premium sent via XML
		SELECT @PremiumSent = COUNT(1) FROM @tblPremium;

		IF ( @XML.exist('(Enrolment/FileInfo)') = 0)
			BEGIN
				INSERT INTO @tblResult VALUES
				(N'<h4 style="color:red;">Wrong format of the extract found. <br />Please contact your IT manager for further assistant.</h4>')
				RAISERROR (N'<h4 style="color:red;">Wrong format of the extract found. <br />Please contact your IT manager for further assistant.</h4>', 16, 1);
			END

			DECLARE @AuditUserId INT =-2,@AssociatedPhotoFolder NVARCHAR(255), @OfficerID INT
			IF ( @XML.exist('(Enrolment/FileInfo)')=1 )
				SET	@AuditUserId= (SELECT T.PR.value('(UserId)[1]','INT') FROM @XML.nodes('Enrolment/FileInfo') AS T(PR))
			
			IF ( @XML.exist('(Enrolment/FileInfo)')=1 )
				SET	@OfficerID= (SELECT T.PR.value('(OfficerId)[1]','INT') FROM @XML.nodes('Enrolment/FileInfo') AS T(PR))
				SET @AssociatedPhotoFolder=(SELECT AssociatedPhotoFolder FROM tblIMISDefaults)

		/********************************************************************************************************
										VALIDATING FILE				
		********************************************************************************************************/
		UPDATE @tblPolicy SET OfficerId = 9
			SELECT @@ROWCOUNT
		

		IF EXISTS(
		--Online Insuree in Offline family
		SELECT 1 FROM @tblInsuree TI
		INNER JOIN @tblFamilies TF ON TI.FamilyId = TF.FamilyId
		WHERE TF.isOffline = 1 AND TI.isOffline= 0
		
		--online Policy in offline family
		UNION ALL
		SELECT 1 FROM @tblPolicy TP 
		INNER JOIN @tblFamilies TF ON TP.FamilyId = TF.FamilyId
		WHERE TF.isOffline = 1 AND TP.isOffline =0

		UNION ALL
		--Insuree without family
		SELECT 1 
		FROM @tblInsuree I LEFT OUTER JOIN @tblFamilies F ON I.FamilyId = F.FamilyID
		WHERE F.FamilyID IS NULL

		UNION ALL

		--Policy without family
		SELECT 1 FROM
		@tblPolicy PL LEFT OUTER JOIN @tblFamilies F ON PL.FamilyId = F.FamilyId
		WHERE F.FamilyId IS NULL

		UNION ALL

		--Premium without policy
		SELECT 1
		FROM @tblPremium PR LEFT OUTER JOIN @tblPolicy P ON PR.PolicyId = P.PolicyId
		WHERE P.PolicyId  IS NULL

		UNION ALL

		---Invalid Family type field
		SELECT 1 FROM @tblFamilies F 
		LEFT OUTER JOIN tblFamilyTypes FT ON F.FamilyType=FT.FamilyTypeCode
		WHERE FT.FamilyType IS NULL AND F.FamilyType IS NOT NULL

		UNION ALL

		---Invalid IdentificationType
		SELECT 1 FROM @tblInsuree I
		LEFT OUTER JOIN tblIdentificationTypes IT ON I.TypeOfId = IT.IdentificationCode
		WHERE IT.IdentificationCode IS NULL AND I.TypeOfId IS NOT NULL

		UNION ALL
		SELECT 1 FROM @tblInureePolicy TIP 
		LEFT OUTER JOIN @tblPolicy TP ON TP.PolicyId = TIP.PolicyId
		WHERE TP.PolicyId IS NULL
		)
		BEGIN
			INSERT INTO @tblResult VALUES
			(N'<h4 style="color:red;">Wrong format of the extract found. <br />Please contact your IT manager for further assistant.</h4>')
		
			RAISERROR (N'<h4 style="color:red;">Wrong format of the extract found. <br />Please contact your IT manager for further assistant.</h4>', 16, 1);
		END

		--SELECT * INTO tempFamilies FROM @tblFamilies
		--SELECT * INTO tempInsuree FROM @tblInsuree
		--SELECT * INTO tempPolicy FROM @tblPolicy
		--SELECT * INTO tempInsureePolicy FROM @tblInureePolicy
		--SELECT * INTO tempPolicy FROM @tblPolicy
		--RETURN

		BEGIN TRAN ENROLL;

		/********************************************************************************************************
										VALIDATING FAMILY				
		********************************************************************************************************/
			--*****************************NEW FAMILY********************************
			INSERT INTO @tblResult (Result)
			SELECT  N'Insuree information is missing for Family with Insurance Number ' + QUOTENAME(F.CHFID) 
			FROM @tblFamilies F
			LEFT OUTER JOIN @tblInsuree I ON F.CHFID = I.CHFID
			WHERE I.InsureeId IS NULL AND F.isOffline =1 ;

			INSERT INTO @tblRejectedFamily (FamilyID)
			SELECT F.FamilyId
			FROM @tblFamilies F
			LEFT OUTER JOIN @tblInsuree I ON F.CHFID = I.CHFID
			WHERE I.InsureeId IS NULL AND F.isOffline =1 ;





			INSERT INTO @tblResult(Result)
			SELECT  N'Family with Insurance Number : ' + QUOTENAME(I.CHFID) + ' already exists'  
			FROM @tblFamilies TF 
			INNER JOIN tblInsuree I ON TF.CHFID = I.CHFID
			WHERE I.ValidityTo IS NULL AND TF.isOffline = 1

			INSERT INTO @tblRejectedFamily(FamilyID)
			SELECT  TF.FamilyId
			FROM @tblFamilies TF 
			INNER JOIN tblInsuree I ON TF.CHFID = I.CHFID
			WHERE I.ValidityTo IS NULL AND TF.isOffline = 1

			--For Phone only
			IF @AuditUserId > 0
				BEGIN
					DECLARE @CHFIDExists INT =0
					SELECT @CHFIDExists = COUNT(1) FROM @tblRejectedFamily GROUP BY FamilyID
					IF @CHFIDExists > 0
					RETURN -2
				END


			--*****************************EXISTING FAMILY********************************
			INSERT INTO @tblResult (Result)
			SELECT  N'Insuree information is missing for Family with Insurance Number ' + QUOTENAME(F.CHFID) 
			FROM @tblFamilies F
			LEFT OUTER JOIN tblInsuree I ON F.CHFID = I.CHFID
			WHERE i.ValidityTo IS NULL AND I.IsHead= 1 AND I.InsureeId IS NULL AND F.isOffline = 0 ;

			INSERT INTO @tblRejectedFamily (FamilyID)
			SELECT F.FamilyId
			FROM @tblFamilies F
			LEFT OUTER JOIN @tblInsuree I ON F.CHFID = I.CHFID
			WHERE I.InsureeId IS NULL AND F.isOffline =1 ;

			--For Phone only
			IF @AuditUserId > 0
				BEGIN
					DECLARE @familiesWithoutHOF INT =0
					SELECT @familiesWithoutHOF = COUNT(1) FROM @tblRejectedFamily GROUP BY FamilyID
					IF @familiesWithoutHOF > 0
					RETURN -1
				END
			
			INSERT INTO @tblResult(Result)
			SELECT N'Family with Insurance Number : ' + QUOTENAME(TF.CHFID) + ' does not exists' 
			FROM @tblFamilies TF 
			LEFT OUTER JOIN tblInsuree I ON TF.CHFID = I.CHFID
			WHERE I.ValidityTo IS NULL 
			AND TF.isOffline = 0 
			AND I.CHFID IS NULL
			AND I.IsHead = 1;

			INSERT INTO @tblRejectedFamily (FamilyID)
			SELECT TF.FamilyId
			FROM @tblFamilies TF 
			LEFT OUTER JOIN tblInsuree I ON TF.CHFID = I.CHFID
			WHERE I.ValidityTo IS NULL 
			AND TF.isOffline = 0 
			AND I.CHFID IS NULL
			AND I.IsHead = 1;

			

			INSERT INTO @tblResult (Result)
			SELECT N'Changing the Location of the Family with Insurance Number : ' + QUOTENAME(I.CHFID) + ' is not allowed' 
			FROM @tblFamilies TF 
			INNER JOIN tblInsuree I ON TF.CHFID = I.CHFID
			INNER JOIN tblFamilies F ON F.FamilyID = ABS(I.FamilyID)
			WHERE I.ValidityTo IS NULL 
			AND F.ValidityTo IS NULL
			AND TF.isOffline = 0 
			AND F.LocationId <> TF.LocationId

			INSERT INTO @tblRejectedFamily
			SELECT DISTINCT TF.FamilyId
			FROM @tblFamilies TF 
			INNER JOIN tblInsuree I ON TF.CHFID = I.CHFID
			INNER JOIN tblFamilies F ON F.FamilyID = ABS(I.FamilyID)
			WHERE I.ValidityTo IS NULL 
			AND F.ValidityTo IS NULL
			AND TF.isOffline = 0 
			AND F.LocationId <> TF.LocationId

			INSERT INTO @tblResult (Result)
			SELECT N'Changing the family of the Insuree with Insurance Number : ' + QUOTENAME(I.CHFID) + ' is not allowed' 
			FROM @tblInsuree TI
			INNER JOIN tblInsuree I ON I.CHFID = TI.CHFID
			INNER JOIN @tblFamilies TF ON TF.FamilyId = TI.FamilyId
			WHERE
			I.ValidityTo IS NULL
			AND TI.isOffline = 0
			AND I.FamilyID <> ABS(TI.FamilyId)

			INSERT INTO @tblRejectedFamily
			SELECT DISTINCT TF.FamilyId
			FROM @tblInsuree TI
			INNER JOIN tblInsuree I ON I.CHFID = TI.CHFID
			INNER JOIN @tblFamilies TF ON TF.FamilyId = TI.FamilyId
			WHERE
			I.ValidityTo IS NULL
			AND TI.isOffline = 0
			AND I.FamilyID <> ABS(TI.FamilyId)

			
			/********************************************************************************************************
										VALIDATING INSUREE				
			********************************************************************************************************/
			----**************NEW INSUREE*********************-----

			INSERT INTO @tblResult(Result)
			SELECT N'Insurance Number : ' + QUOTENAME(TI.CHFID) + ' already exists' 
			FROM @tblInsuree TI
			INNER JOIN tblInsuree I ON TI.CHFID = I.CHFID
			WHERE I.ValidityTo IS NULL AND TI.isOffline = 1
			
			INSERT INTO @tblRejectedInsuree(InsureeID)
			SELECT TI.InsureeId
			FROM @tblInsuree TI
			INNER JOIN tblInsuree I ON TI.CHFID = I.CHFID
			WHERE I.ValidityTo IS NULL AND TI.isOffline = 1

			--Reject Family of the duplicate CHFID
			INSERT INTO @tblRejectedFamily(FamilyID)
			SELECT TI.FamilyId
			FROM @tblInsuree TI
			INNER JOIN tblInsuree I ON TI.CHFID = I.CHFID
			WHERE I.ValidityTo IS NULL AND TI.isOffline = 1


			--For Phone only
			IF @AuditUserId > 0
				BEGIN
					DECLARE @InsureeAlreadyExists INT =0
					SELECT @InsureeAlreadyExists = COUNT(1) FROM @tblRejectedInsuree GROUP BY InsureeID
					IF @InsureeAlreadyExists > 0
					RETURN -3
				END

			----**************EXISTING INSUREE*********************-----
			INSERT INTO @tblResult(Result)
			SELECT N'Insurance Number : ' + QUOTENAME(TI.CHFID) + ' does not exists' 
			FROM @tblInsuree TI
			LEFT OUTER JOIN tblInsuree I ON TI.CHFID = I.CHFID
			WHERE 
			I.ValidityTo IS NULL 
			AND I.CHFID IS NULL
			AND TI.isOffline = 0
			
			INSERT INTO @tblRejectedInsuree(InsureeID)
			SELECT TI.InsureeId
			FROM @tblInsuree TI
			LEFT OUTER JOIN tblInsuree I ON TI.CHFID = I.CHFID
			WHERE 
			I.ValidityTo IS NULL 
			AND I.CHFID IS NULL
			AND TI.isOffline = 0


			

			/********************************************************************************************************
										VALIDATING POLICIES				
			********************************************************************************************************/


			/********************************************************************************************************
										VALIDATING PREMIUMS				
			********************************************************************************************************/
			INSERT INTO @tblResult(Result)
			SELECT N'Receipt number : ' + QUOTENAME(PR.Receipt) + ' is duplicateed in a file ' 
			FROM @tblPremium PR
			INNER JOIN @tblPolicy PL ON PL.PolicyId =PR.PolicyId
			GROUP BY PR.Receipt HAVING COUNT(PR.PolicyId) > 1

			INSERT INTO @tblRejectedPremium(PremiumId)
			SELECT TPR.PremiumId
			FROM tblPremium PR
			INNER JOIN @tblPremium TPR ON PR.Amount = TPR.Amount
			INNER JOIN @tblPolicy TP ON TP.PolicyId = TPR.PolicyId 
			AND PR.Receipt = TPR.Receipt 
			AND PR.PolicyID = TPR.NewPolicyId
			WHERE PR.ValidityTo IS NULL
			AND TP.isOffline = 0
			

			--For Phone only
			IF @AuditUserId > 0
				BEGIN
					DECLARE @duplicateReceipt INT =0
					SELECT @duplicateReceipt = COUNT(1) FROM @tblRejectedPremium GROUP BY PremiumId
					IF @duplicateReceipt > 0
					RETURN -4
				END

			/********************************************************************************************************
										DELETE REJECTED RECORDS		
			********************************************************************************************************/

			SELECT @FamilyRejected =ISNULL(COUNT(DISTINCT FamilyID),0) FROM
			@tblRejectedFamily --GROUP BY FamilyID

			SELECT @PolicyRejected =ISNULL(COUNT(DISTINCT PolicyId),0) FROM
			@tblRejectedPolicy --GROUP BY PolicyId

			SELECT @PolicyRejected= ISNULL(COUNT(DISTINCT TP.PolicyId),0)+ ISNULL(@PolicyRejected ,0)
			FROM @tblPolicy TP 
			INNER JOIN @tblFamilies TF ON TF.FamilyId = TP.FamilyId
			INNER JOIN @tblRejectedFamily RF ON RF.FamilyID = TP.FamilyId
			GROUP BY TP.PolicyId

			SELECT @PremiumRejected =ISNULL(COUNT(DISTINCT PremiumId),0) FROM
			@tblRejectedPremium --GROUP BY PremiumId


			--Rejected Families
			DELETE TF FROM @tblFamilies TF
			INNER JOIN @tblRejectedFamily RF ON TF.FamilyId =RF.FamilyId
			
			DELETE TF FROM @tblFamilies TF
			INNER JOIN @tblInsuree TI ON TI.FamilyId = TF.FamilyId
			INNER JOIN @tblRejectedInsuree RI ON RI.InsureeID = TI.InsureeId

			DELETE TI FROM @tblInsuree TI
			INNER JOIN @tblRejectedFamily RF ON TI.FamilyId =RF.FamilyId

			DELETE TP FROM @tblPolicy TP
			INNER JOIN @tblRejectedFamily RF ON TP.FamilyId =RF.FamilyId

			DELETE TP FROM @tblPolicy TP
			LEFT OUTER JOIN @tblFamilies TF ON TP.FamilyId =TP.FamilyId WHERE TF.FamilyId IS NULL

			--Rejected Insuree
			DELETE TI FROM @tblInsuree TI
			INNER JOIN @tblRejectedInsuree RI ON TI.InsureeId =RI.InsureeID

			DELETE TIP FROM @tblInureePolicy TIP
			INNER JOIN @tblRejectedInsuree RI ON TIP.InsureeId =RI.InsureeID
			
			--Rejected Premium
			DELETE TPR FROM @tblPremium TPR
			INNER JOIN @tblRejectedPremium RP ON RP.PremiumId = TPR.PremiumId


			

			--Making the first insuree to be head for the offline families which miss head of family ONLY for the new family
			IF NOT EXISTS(SELECT 1 FROM @tblFamilies TF INNER JOIN @tblInsuree TI ON TI.CHFID = TF.CHFID WHERE TI.IsHead = 1 AND TF.isOffline = 1)
			BEGIN
				UPDATE TI SET TI.IsHead =1 
				FROM @tblInsuree TI 
				INNER JOIN @tblFamilies TF ON TF.FamilyId = TI.FamilyId 
				WHERE TF.isOffline = 1 
				AND TI.InsureeId=(SELECT TOP 1 InsureeId FROM @tblInsuree WHERE isOffline = 1 ORDER BY InsureeId ASC)
			END
			
			--Updating FamilyId, PolicyId and InsureeId for the existing records
			UPDATE @tblFamilies SET NewFamilyId = ABS(FamilyId) WHERE isOffline = 0
			UPDATE @tblInsuree SET NewFamilyId =ABS(FamilyId)  
			UPDATE @tblPolicy SET NewPolicyId = PolicyId WHERE isOffline = 0

			UPDATE TP SET TP.NewFamilyId = TF.FamilyId FROM @tblPolicy TP 
			INNER JOIN @tblFamilies TF ON TF.FamilyId = TP.FamilyId 
			WHERE TF.isOffline = 0
			

			--updating existing families
			IF EXISTS(SELECT 1 FROM @tblFamilies WHERE isOffline = 0 AND FamilyId < 0)
			BEGIN
				INSERT INTO tblFamilies ([insureeid],[Poverty],[ConfirmationType],isOffline,[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID],FamilyType, FamilyAddress,Ethnicity,ConfirmationNo, LocationId) 
				SELECT F.[insureeid],F.[Poverty],F.[ConfirmationType],F.isOffline,F.[ValidityFrom],getdate() ValidityTo,F.FamilyID, @AuditUserID,F.FamilyType, F.FamilyAddress,F.Ethnicity,F.ConfirmationNo,F.LocationId FROM @tblFamilies TF
				INNER JOIN tblFamilies F ON ABS(TF.FamilyId) = F.FamilyID
				WHERE 
				F.ValidityTo IS NULL
				AND TF.isOffline = 0 AND TF.FamilyId < 0

				
				UPDATE dst SET dst.[Poverty] = src.Poverty,  dst.[ConfirmationType] = src.ConfirmationType, isOffline=0, dst.[ValidityFrom]=GETDATE(), dst.[AuditUserID] = @AuditUserID, dst.FamilyType = src.FamilyType,  dst.FamilyAddress = src.FamilyAddress,
							   dst.Ethnicity = src.Ethnicity,  dst.ConfirmationNo = src.ConfirmationNo
						 FROM tblFamilies dst
						 INNER JOIN @tblFamilies src ON ABS(src.FamilyID)= dst.FamilyID WHERE src.isOffline = 0 AND src.FamilyId < 0
				SELECT @FamiliesUpd = ISNULL(@@ROWCOUNT	,0)

			END

			--new family
				--IF EXISTS(SELECT 1 FROM @tblFamilies WHERE isOffline = 1) 
				--	BEGIN
				--		DECLARE @CurFamilyId INT
				--		DECLARE CurFamily CURSOR FOR SELECT FamilyId FROM @tblFamilies WHERE  isOffline = 1
				--			OPEN CurFamily
				--				FETCH NEXT FROM CurFamily INTO @CurFamilyId;
				--				WHILE @@FETCH_STATUS = 0
				--				BEGIN
				--				INSERT INTO tblFamilies(InsureeId, LocationId, Poverty, ValidityFrom, AuditUserId, FamilyType, FamilyAddress, Ethnicity, ConfirmationNo, ConfirmationType, isOffline) 
				--				SELECT 0 , TF.LocationId, TF.Poverty, GETDATE() , @AuditUserId , TF.FamilyType, TF.FamilyAddress, TF.Ethnicity, TF.ConfirmationNo, ConfirmationType,1 isOffline FROM @tblFamilies TF WHERE FamilyId = @CurFamilyId
				--				DECLARE @NewFamilyId  INT  =0
				--				SELECT @NewFamilyId= SCOPE_IDENTITY();
				--				IF @@ROWCOUNT > 0
				--					BEGIN
				--						SET @FamilyImported = ISNULL(@FamilyImported,0) + 1
				--						UPDATE @tblFamilies SET NewFamilyId = @NewFamilyId WHERE FamilyId = @CurFamilyId
				--						UPDATE @tblInsuree SET NewFamilyId = @NewFamilyId WHERE FamilyId = @CurFamilyId
				--						UPDATE @tblPolicy SET NewFamilyId = @NewFamilyId WHERE FamilyId = @CurFamilyId
				--					END
								
				--				FETCH NEXT FROM CurFamily INTO @CurFamilyId;
				--			END
				--			CLOSE CurFamily
				--			DEALLOCATE CurFamily;
				--		END

				DELETE FROM @tblIds;
				MERGE tblFamilies
			USING @tblFamilies TF ON TF.isOffline = 0
			WHEN NOT MATCHED THEN
				INSERT (InsureeId, LocationId, Poverty, ValidityFrom, AuditUserId, FamilyType, FamilyAddress, Ethnicity, ConfirmationNo, ConfirmationType, isOffline) 
				VALUES(0 , TF.LocationId, TF.Poverty, GETDATE() , @AuditUserId , TF.FamilyType, TF.FamilyAddress, TF.Ethnicity, TF.ConfirmationNo, ConfirmationType,1)
				OUTPUT TF.FamilyId, inserted.FamilyID INTO @tblIds;
			SELECT @FamilyImported = ISNULL(@@ROWCOUNT,0);
			
			UPDATE TF SET TF.NewFamilyId = ID.NewId FROM @tblIds ID
			INNER JOIN @tblFamilies TF ON TF.FamilyId = ID.OldId
			
			UPDATE TI SET TI.NewFamilyId = ID.NewId FROM @tblIds ID
			INNER JOIN @tblInsuree TI ON TI.FamilyId = ID.OldId

			UPDATE TP SET TP.NewFamilyId = ID.NewId FROM @tblIds ID
			INNER JOIN @tblPolicy TP ON TP.FamilyId = ID.OldId




			--Delete duplicate policies
			DELETE TP
			OUTPUT 'Policy for the family : ' + QUOTENAME(I.CHFID) + ' with Product Code:' + QUOTENAME(Prod.ProductCode) + ' already exists' INTO @tblResult
			FROM tblPolicy PL 
			INNER JOIN @tblPolicy TP ON PL.FamilyID = ABS(TP.NewFamilyId )
									AND PL.EnrollDate = TP.EnrollDate 
									AND PL.StartDate = TP.StartDate 
									AND PL.ProdID = TP.ProdId 
			INNER JOIN tblProduct Prod ON PL.ProdId = Prod.ProdId
			INNER JOIN tblInsuree I ON PL.FamilyId = I.FamilyId
			WHERE PL.ValidityTo IS NULL
			AND I.IsHead = 1;

			--Delete Premiums without polices
			DELETE TPR FROM @tblPremium TPR
			LEFT OUTER JOIN @tblPolicy TP ON TP.PolicyId = TPR.PolicyId
			WHERE TPR.PolicyId IS NULL


			--updating existing insuree
			IF EXISTS(SELECT 1 FROM @tblInsuree WHERE isOffline = 0)
				BEGIN
					--Insert Insuree History
					INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,[ValidityTo],legacyId,[Relationship],[Profession],[Education],[Email],[TypeOfId],[HFID], [CurrentAddress], [GeoLocation], [CurrentVillage]) 
					SELECT	I.[FamilyID],I.[CHFID],I.[LastName],I.[OtherNames],I.[DOB],I.[Gender],I.[Marital],I.[IsHead],I.[passport],I.[Phone],I.[PhotoID],I.[PhotoDate],I.[CardIssued],I.isOffline,I.[AuditUserID],I.[ValidityFrom] ,GETDATE() ValidityTo,I.InsureeID,I.[Relationship],I.[Profession],I.[Education],I.[Email] ,I.[TypeOfId],I.[HFID], I.[CurrentAddress], I.[GeoLocation], [CurrentVillage] FROM @tblInsuree TI
					INNER JOIN tblInsuree I ON TI.CHFID = I.CHFID
					WHERE I.ValidityTo IS NULL AND
					TI.isOffline = 0

					UPDATE dst SET  dst.[LastName] = src.LastName,dst.[OtherNames] = src.OtherNames,dst.[DOB] = src.DOB,dst.[Gender] = src.Gender,dst.[Marital] = src.Marital,dst.[passport] = src.passport,dst.[Phone] = src.Phone,dst.[PhotoDate] = GETDATE(),dst.[CardIssued] = src.CardIssued,dst.isOffline=0,dst.[ValidityFrom] = GetDate(),dst.[AuditUserID] = @AuditUserID ,dst.[Relationship] = src.Relationship, dst.[Profession] = src.Profession, dst.[Education] = src.Education,dst.[Email] = src.Email ,dst.TypeOfId = src.TypeOfId,dst.HFID = src.HFID, dst.CurrentAddress = src.CurrentAddress, dst.CurrentVillage = src.CurVillage, dst.GeoLocation = src.GeoLocation 
					FROM tblInsuree dst
					INNER JOIN @tblInsuree src ON src.CHFID = dst.CHFID
					WHERE dst.ValidityTo IS NULL AND src.isOffline = 0;

					SELECT @InsureeUpd= ISNULL(COUNT(1),0) FROM @tblInsuree WHERE isOffline = 0


					--Insert Photo  History
					INSERT INTO tblPhotos(InsureeID,CHFID,PhotoFolder,PhotoFileName,PhotoDate,OfficerID,ValidityFrom,ValidityTo,AuditUserID) 
					SELECT P.InsureeID,P.CHFID,P.PhotoFolder,P.PhotoFileName,P.PhotoDate,P.OfficerID,P.ValidityFrom,GETDATE() ValidityTo,P.AuditUserID 
					FROM tblPhotos P
					INNER JOIN tblInsuree I ON I.PhotoID =P.PhotoID
					INNER JOIN @tblInsuree TI ON TI.CHFID = I.CHFID
					WHERE 
					P.ValidityTo IS NULL AND I.ValidityTo IS NULL
					AND TI.isOffline = 0

					--Update Photo
					UPDATE P SET PhotoFolder = @AssociatedPhotoFolder+'/', PhotoFileName = TI.PhotoPath, OfficerID = @OfficerID, ValidityFrom = GETDATE(), AuditUserID = @AuditUserID 
					FROM tblPhotos P
					INNER JOIN tblInsuree I ON I.PhotoID =P.PhotoID
					INNER JOIN @tblInsuree TI ON TI.CHFID = I.CHFID
					WHERE 
					P.ValidityTo IS NULL AND I.ValidityTo IS NULL
					AND TI.isOffline = 0
				END

				--new insuree
			--	IF EXISTS(SELECT 1 FROM @tblInsuree WHERE isOffline = 1) 
			--		BEGIN
			--			DECLARE @CurInsureeId INT
			--			DECLARE CurInsuree CURSOR FOR SELECT InsureeId FROM @tblInsuree WHERE  isOffline = 1
			--				OPEN CurInsuree
			--					FETCH NEXT FROM CurInsuree INTO @CurInsureeId;
			--					WHILE @@FETCH_STATUS = 0
			--					BEGIN
			--					INSERT INTO tblInsuree(FamilyId, CHFID, LastName, OtherNames, DOB, Gender, Marital, IsHead, passport, Phone, CardIssued, ValidityFrom,
			--					AuditUserId, Relationship, Profession, Education, Email, TypeOfId, HFID, CurrentAddress, GeoLocation, CurrentVillage, isOffline)
			--					SELECT NewFamilyId, CHFID, LastName, OtherNames, DOB, Gender, Marital, IsHead, passport, Phone, CardIssued, GETDATE() ValidityFrom,
			--					@AuditUserId AuditUserId, Relationship, Profession, Education, Email, TypeOfId, HFID, CurrentAddress, GeoLocation, CurVillage, 1 isOffLine
			--					FROM @tblInsuree WHERE InsureeId = @CurInsureeId;
			--					DECLARE @NewInsureeId  INT  =0
			--					SELECT @NewInsureeId= SCOPE_IDENTITY();
			--					IF @@ROWCOUNT > 0
			--						BEGIN
			--							SET @InsureeImported = ISNULL(@InsureeImported,0) + 1
			--							--updating insureeID
			--							UPDATE @tblInsuree SET NewInsureeId = @NewInsureeId WHERE InsureeId = @CurInsureeId
			--							UPDATE @tblInureePolicy SET NewInsureeId = @NewInsureeId WHERE InsureeId = @CurInsureeId
			--							UPDATE F SET InsureeId = TI.NewInsureeId
			--							FROM @tblInsuree TI 
			--							INNER JOIN tblInsuree I ON TI.NewInsureeId = I.InsureeId
			--							INNER JOIN tblFamilies F ON TI.NewFamilyId = F.FamilyID
			--							WHERE TI.IsHead = 1 AND TI.InsureeId = @NewInsureeId
			--						END

			--					--Now we will insert new insuree in the table tblInsureePolicy for only existing policies
			--					IF EXISTS(SELECT 1 FROM tblPolicy P 
			--					INNER JOIN tblFamilies F ON F.FamilyID = P.FamilyID
			--					INNER JOIN tblInsuree I ON I.FamilyID = I.FamilyID
			--					WHERE I.ValidityTo IS NULL AND P.ValidityTo IS NULL AND F.ValidityTo IS NULL AND I.InsureeID = @NewInsureeId)
			--						EXEC uspAddInsureePolicy @NewInsureeId	

			--						INSERT INTO tblPhotos(InsureeID,CHFID,PhotoFolder,PhotoFileName,OfficerID,PhotoDate,ValidityFrom,AuditUserID)
			--						SELECT @NewInsureeId InsureeId, CHFID, @AssociatedPhotoFolder +'/' photoFolder, PhotoPath photoFileName, @OfficerID OfficerID, getdate() photoDate, getdate() ValidityFrom,@AuditUserId AuditUserId
			--						FROM @tblInsuree WHERE InsureeId = @CurInsureeId 

			--					--Update photoId in Insuree
			--					UPDATE I SET PhotoId = PH.PhotoId, I.PhotoDate = PH.PhotoDate
			--					FROM tblInsuree I
			--					INNER JOIN tblPhotos PH ON PH.InsureeId = I.InsureeId

								

			--					FETCH NEXT FROM CurInsuree INTO @CurInsureeId;
			--					END
			--			CLOSE CurInsuree
			--				DEALLOCATE CurInsuree;
			--		END
				
			--	--updating family with the new insureeId of the head
			--	UPDATE F SET InsureeID = I.InsureeID FROM tblInsuree I
			--	INNER JOIN @tblInsuree TI ON I.CHFID = TI.CHFID
			--	INNER JOIN @tblFamilies TF ON I.FamilyID = TF.NewFamilyId
			--	INNER JOIN tblFamilies F ON F.FamilyID = I.FamilyID
			--	WHERE I.ValidityTo IS NULL AND I.ValidityTo IS NULL AND I.IsHead = 1



			DELETE FROM @tblIds;
				MERGE tblInsuree
			USING @tblInsuree TI ON TI.isOffline = 0
			WHEN NOT MATCHED THEN
				INSERT (FamilyId, CHFID, LastName, OtherNames, DOB, Gender, Marital, IsHead, passport, Phone, CardIssued, ValidityFrom,
								AuditUserId, Relationship, Profession, Education, Email, TypeOfId, HFID, CurrentAddress, GeoLocation, CurrentVillage, isOffline)
				VALUES(NewFamilyId, CHFID, LastName, OtherNames, DOB, Gender, Marital, IsHead, passport, Phone, CardIssued, GETDATE(),
								@AuditUserId, Relationship, Profession, Education, Email, TypeOfId, HFID, CurrentAddress, GeoLocation, CurVillage, 1)
				OUTPUT TI.InsureeId, inserted.InsureeId INTO @tblIds;
			SELECT @InsureeImported = ISNULL(@@ROWCOUNT,0);
			
			--updating insureeID
			UPDATE TI SET TI.NewInsureeId = ID.NewId FROM @tblInsuree TI 
			INNER JOIN @tblIds ID ON ID.OldId= TI.InsureeId

			UPDATE TIP SET TIP.NewInsureeId = ID.NewId FROM @tblInureePolicy TIP 
			INNER JOIN @tblIds ID ON ID.OldId= TIP.InsureeId

			UPDATE F SET InsureeId = I.InsureeID
			FROM @tblIds ID 
			INNER JOIN tblInsuree I ON ID.NewId = I.InsureeId
			INNER JOIN tblFamilies F ON I.FamilyID = F.FamilyID
			WHERE I.IsHead = 1 AND 
			I.ValidityTo IS NULL
			AND F.ValidityTo IS NULL

			

			--Now we will insert new insuree in the table tblInsureePolicy for only existing policies
						DECLARE @CurInsureeId INT
						DECLARE CurInsuree CURSOR FOR SELECT I.InsureeID FROM @tblIds ID
											INNER JOIN tblInsuree I ON I.InsureeID = ID.NewId
											INNER JOIN tblFamilies F ON F.FamilyID = I.FamilyID
											INNER JOIN tblPolicy P ON P.FamilyID = F.FamilyID
											WHERE I.ValidityTo IS NULL AND P.ValidityTo IS NULL AND F.ValidityTo IS NULL 
							OPEN CurInsuree
								FETCH NEXT FROM CurInsuree INTO @CurInsureeId;
								WHILE @@FETCH_STATUS = 0
								BEGIN
									EXEC uspAddInsureePolicy @CurInsureeId	
								FETCH NEXT FROM CurInsuree INTO @CurInsureeId;
								END
						CLOSE CurInsuree
							DEALLOCATE CurInsuree;


					
				INSERT INTO tblPhotos(InsureeID,CHFID,PhotoFolder,PhotoFileName,OfficerID,PhotoDate,ValidityFrom,AuditUserID)
				SELECT ID.NewId InsureeId, CHFID, @AssociatedPhotoFolder +'/' photoFolder, PhotoPath photoFileName, @OfficerID OfficerID, getdate() photoDate, getdate() ValidityFrom,@AuditUserId AuditUserId
				FROM @tblInsuree TI
				INNER JOIN @tblIds ID ON ID.OldId = TI.InsureeId

			--Update photoId in Insuree
			UPDATE I SET PhotoId = PH.PhotoId, I.PhotoDate = PH.PhotoDate
			FROM tblInsuree I
			INNER JOIN tblPhotos PH ON PH.InsureeId = I.InsureeId

			DELETE FROM @tblIds;



				---**************INSERTING POLICIES-----------
				DECLARE @FamilyId INT = 0,
				@HOFId INT = 0,
				@PolicyValue DECIMAL(18, 4),
				@ProdId INT,
				@PolicyStage CHAR(1),
				@StartDate DATE,
				@ExpiryDate DATE,
				@EnrollDate DATE,
				@EffectiveDate DATE,
				@ErrorCode INT,
				@PolicyStatus INT,
				@PolicyId TINYINT,
				@PolicyValueFromPhone DECIMAL(18, 4),
				@ContributionAmount DECIMAL(18, 4),
				@Active TINYINT=2,
				@Idle TINYINT=1,
				@NewPolicyId INT,
				@OldPolicyStatus INT,
				@NewPolicyStatus INT


			
			--New policies
		IF EXISTS(SELECT 1 FROM @tblPolicy WHERE isOffline =1)
			BEGIN
			DECLARE CurPolicies CURSOR FOR SELECT PolicyId, ProdId, ISNULL(PolicyStage, N'N') PolicyStage, StartDate, EnrollDate,ExpiryDate, PolicyStatus, PolicyValue, NewFamilyId FROM @tblPolicy WHERE isOffline = 1
			OPEN CurPolicies;
			FETCH NEXT FROM CurPolicies INTO @PolicyId, @ProdId, @PolicyStage, @StartDate, @EnrollDate, @ExpiryDate,  @PolicyStatus, @PolicyValueFromPhone, @FamilyId;
			WHILE @@FETCH_STATUS = 0
			BEGIN			
							SET @EffectiveDate= NULL;
							SET @PolicyStatus = @Idle;

							EXEC @PolicyValue = uspPolicyValue @FamilyId, @ProdId, 0, @PolicyStage, @EnrollDate, 0, @ErrorCode OUTPUT;

							INSERT INTO tblPolicy(FamilyID,EnrollDate,StartDate,EffectiveDate,ExpiryDate,PolicyStatus,PolicyValue,ProdID,OfficerID,PolicyStage,ValidityFrom,AuditUserID, isOffline)
							SELECT	 ABS(NewFamilyID),EnrollDate,StartDate,@EffectiveDate,ExpiryDate,@PolicyStatus,@PolicyValue,ProdID,@OfficerID,PolicyStage,GETDATE(),@AuditUserId, 1 isOffline FROM @tblPolicy WHERE PolicyId=@PolicyId
							SELECT @NewPolicyId = SCOPE_IDENTITY()
							INSERT INTO @tblIds(OldId, [NewId]) VALUES(@PolicyId, @NewPolicyId)
							
							IF @@ROWCOUNT > 0
								BEGIN
									SET @PolicyImported = ISNULL(@PolicyImported,0) +1
									UPDATE @tblInureePolicy SET NewPolicyId = @NewPolicyId WHERE PolicyId=@PolicyId
									UPDATE @tblPremium SET NewPolicyId =@NewPolicyId  WHERE PolicyId = @PolicyId
									INSERT INTO tblPremium(PolicyID,PayerID,Amount,Receipt,PayDate,PayType,ValidityFrom,AuditUserID,isPhotoFee,isOffline)
									SELECT NewPolicyId,PayerID,Amount,Receipt,PayDate,PayType,GETDATE(),@AuditUserId,isPhotoFee, 1 isOffline
									FROM @tblPremium WHERE NewPolicyId = @NewPolicyId
									SELECT @PremiumImported = ISNULL(@PremiumImported,0) +1
								END
							
				
				SELECT @ContributionAmount = ISNULL(SUM(Amount),0) FROM tblPremium WHERE PolicyId = @NewPolicyId
					IF ((@PolicyValueFromPhone = @PolicyValue))
						BEGIN
							SELECT @PolicyStatus = PolicyStatus FROM @tblPolicy WHERE PolicyId=@PolicyId
							SELECT @EffectiveDate = EffectiveDate FROM @tblPolicy WHERE PolicyId=@PolicyId
							
							UPDATE tblPolicy SET PolicyStatus = @PolicyStatus, EffectiveDate = @EffectiveDate WHERE PolicyID = @NewPolicyId

							INSERT INTO tblInsureePolicy
								([InsureeId],[PolicyId],[EnrollmentDate],[StartDate],[EffectiveDate],[ExpiryDate],[ValidityFrom],[AuditUserId], isOffline) 
							SELECT
								 ISNULL(NewInsureeId, IP.InsureeId),IP.NewPolicyId,@EnrollDate,@StartDate,IP.[EffectiveDate],@ExpiryDate,GETDATE(),@AuditUserId, 1 isOffline FROM @tblInureePolicy IP
							     WHERE IP.PolicyId=@PolicyId
						END
					ELSE
						BEGIN
							IF @ContributionAmount >= @PolicyValue
									SELECT @PolicyStatus = @Active
								ELSE
									SELECT @PolicyStatus =@Idle
							
									--Checking the Effectice Date
										DECLARE @Amount DECIMAL(10,0), @TotalAmount DECIMAL(10,0), @PaymentDate DATE 
										DECLARE CurPremiumPayment CURSOR FOR SELECT PayDate, Amount FROM @tblPremium WHERE PolicyId = @PolicyId;
										OPEN CurPremiumPayment;
										FETCH NEXT FROM CurPremiumPayment INTO @PaymentDate,@Amount;
										WHILE @@FETCH_STATUS = 0
										BEGIN
											SELECT @TotalAmount = ISNULL(@TotalAmount,0) + @Amount;
												IF(@TotalAmount >= @PolicyValue)
													BEGIN
														SELECT @EffectiveDate = @PaymentDate
														BREAK;
													END
												ELSE
														SELECT @EffectiveDate = NULL
											FETCH NEXT FROM CurPremiumPayment INTO @PaymentDate,@Amount;
										END
										CLOSE CurPremiumPayment;
										DEALLOCATE CurPremiumPayment; 
							
								UPDATE tblPolicy SET PolicyStatus = @PolicyStatus, EffectiveDate = @EffectiveDate WHERE PolicyID = @NewPolicyId
							

							DECLARE @InsureeId INT
							DECLARE CurNewPolicy CURSOR FOR SELECT I.InsureeID FROM tblInsuree I 
													INNER JOIN tblFamilies F ON I.FamilyID = F.FamilyID 
													INNER JOIN tblPolicy P ON P.FamilyID = F.FamilyID 
													WHERE P.PolicyId = @NewPolicyId 
													AND I.ValidityTo IS NULL 
													AND F.ValidityTo IS NULL
													AND P.ValidityTo IS NULL
							OPEN CurNewPolicy;
							FETCH NEXT FROM CurNewPolicy INTO @InsureeId;
							WHILE @@FETCH_STATUS = 0
							BEGIN
								EXEC uspAddInsureePolicy @InsureeId;
								FETCH NEXT FROM CurNewPolicy INTO @InsureeId;
							END
							CLOSE CurNewPolicy;
							DEALLOCATE CurNewPolicy; 

						END

				

				FETCH NEXT FROM CurPolicies INTO @PolicyId, @ProdId, @PolicyStage, @StartDate, @EnrollDate, @ExpiryDate,  @PolicyStatus, @PolicyValueFromPhone, @FamilyId;
			END
			CLOSE CurPolicies;
			DEALLOCATE CurPolicies; 
		END

		SELECT @PolicyImported = ISNULL(COUNT(1),0) FROM @tblPolicy WHERE isOffline = 1
			
			

		

	
	IF EXISTS(SELECT COUNT(1) 
			FROM tblInsuree 
			WHERE ValidityTo IS NULL
			AND IsHead = 1
			GROUP BY FamilyID
			HAVING COUNT(1) > 1)
			
			--Added by Amani
			BEGIN
					DELETE FROM @tblResult;
					SET @FamilyImported = 0;
					SET @FamilyRejected =0;
					SET @FamiliesUpd =0;
					SET @InsureeImported  = 0;
					SET @InsureeUpd =0;
					SET @PolicyImported  = 0;
					SET @PolicyImported  = 0;
					SET @PolicyRejected  = 0;
					SET @PremiumImported  = 0 
					INSERT INTO @tblResult VALUES
						(N'<h3 style="color:red;">Double HOF Found. <br />Please contact your IT manager for further assistant.</h3>')
						--GOTO EndOfTheProcess;

						RAISERROR(N'Double HOF Found',16,1)	
					END


		COMMIT TRAN ENROLL;

	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE();
		IF @@TRANCOUNT > 0 ROLLBACK TRAN ENROLL;
		
		SELECT * FROM @tblResult;
		RETURN -400
	END CATCH

	SELECT Result FROM @tblResult;
	
	RETURN 0 --ALL OK
	END



GO



