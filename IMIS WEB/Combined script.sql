IF OBJECT_ID('uspSSRSCapitationPayment') IS NOT NULL
DROP PROCEDURE [dbo].[uspSSRSCapitationPayment]
GO

CREATE PROCEDURE [dbo].[uspSSRSCapitationPayment]

(
	@RegionId INT = NULL,
	@DistrictId INT = NULL,
	@ProdId INT,
	@Year INT,
	@Month INT,
	@HFLevel xAttributeV READONLY
)
AS
BEGIN
	
	DECLARE @Level1 CHAR(1) = NULL,
			@Sublevel1 CHAR(1) = NULL,
			@Level2 CHAR(1) = NULL,
			@Sublevel2 CHAR(1) = NULL,
			@Level3 CHAR(1) = NULL,
			@Sublevel3 CHAR(1) = NULL,
			@Level4 CHAR(1) = NULL,
			@Sublevel4 CHAR(1) = NULL,
			@ShareContribution DECIMAL(5, 2),
			@WeightPopulation DECIMAL(5, 2),
			@WeightNumberFamilies DECIMAL(5, 2),
			@WeightInsuredPopulation DECIMAL(5, 2),
			@WeightNumberInsuredFamilies DECIMAL(5, 2),
			@WeightNumberVisits DECIMAL(5, 2),
			@WeightAdjustedAmount DECIMAL(5, 2)

	DECLARE @FirstDay DATE = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01'; 
	DECLARE @LastDay DATE = EOMONTH(CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01', 0)
	DECLARE @DaysInMonth INT = DATEDIFF(DAY,@FirstDay,DATEADD(MONTH,1,@FirstDay));

	SELECT @Level1 = Level1, @Sublevel1 = Sublevel1, @Level2 = Level2, @Sublevel2 = Sublevel2, @Level3 = Level3, @Sublevel3 = Sublevel3, 
	@Level4 = Level4, @Sublevel4 = Sublevel4, @ShareContribution = ISNULL(ShareContribution, 0), @WeightPopulation = ISNULL(WeightPopulation, 0), 
	@WeightNumberFamilies = ISNULL(WeightNumberFamilies, 0), @WeightInsuredPopulation = ISNULL(WeightInsuredPopulation, 0), @WeightNumberInsuredFamilies = ISNULL(WeightNumberInsuredFamilies, 0), 
	@WeightNumberVisits = ISNULL(WeightNumberVisits, 0), @WeightAdjustedAmount = ISNULL(WeightAdjustedAmount, 0)
	FROM tblProduct Prod 
	WHERE ProdId = @ProdId;


	PRINT @ShareContribution
	PRINT @WeightPopulation
	PRINT @WeightNumberFamilies 
	PRINT @WeightInsuredPopulation 
	PRINT @WeightNumberInsuredFamilies 
	PRINT @WeightNumberVisits 
	PRINT @WeightAdjustedAmount


	;WITH TotalPopFam AS
	(
		SELECT C.HFID  ,
		CASE WHEN ISNULL(@DistrictId, @RegionId) IN (R.RegionId, D.DistrictId) THEN 1 ELSE 0 END * SUM((ISNULL(L.MalePopulation, 0) + ISNULL(L.FemalePopulation, 0) + ISNULL(L.OtherPopulation, 0)) *(0.01* Catchment))[Population], 
		CASE WHEN ISNULL(@DistrictId, @RegionId) IN (R.RegionId, D.DistrictId) THEN 1 ELSE 0 END * SUM(ISNULL(((L.Families)*(0.01* Catchment)), 0))TotalFamilies
		FROM tblHFCatchment C
		LEFT JOIN tblLocations L ON L.LocationId = C.LocationId
		INNER JOIN tblHF HF ON C.HFID = HF.HfID
		INNER JOIN tblDistricts D ON HF.LocationId = D.DistrictId
		INNER JOIN tblRegions R ON D.Region = R.RegionId
		WHERE C.ValidityTo IS NULL
		AND L.ValidityTo IS NULL
		AND HF.ValidityTo IS NULL
		GROUP BY C.HFID, D.DistrictId, R.RegionId
	), InsuredInsuree AS
	(
		SELECT HC.HFID, @ProdId ProdId, COUNT(DISTINCT IP.InsureeId)*(0.01 * Catchment) TotalInsuredInsuree
		FROM tblInsureePolicy IP
		INNER JOIN tblInsuree I ON I.InsureeId = IP.InsureeId
		INNER JOIN tblFamilies F ON F.FamilyId = I.FamilyId
		INNER JOIN tblHFCatchment HC ON HC.LocationId = F.LocationId
		--INNER JOIN uvwLocations L ON L.LocationId = HC.LocationId
		INNER JOIN tblPolicy PL ON PL.PolicyID = IP.PolicyId
		WHERE HC.ValidityTo IS NULL 
		AND I.ValidityTo IS NULL
		AND IP.ValidityTo IS NULL
		AND F.ValidityTo IS NULL
		AND PL.ValidityTo IS NULL
		AND IP.EffectiveDate <= @LastDay 
		AND IP.ExpiryDate > @LastDay
		AND PL.ProdID = @ProdId
		GROUP BY HC.HFID, Catchment--, L.LocationId
	), InsuredFamilies AS
	(
		SELECT HC.HFID, COUNT(DISTINCT F.FamilyID)*(0.01 * Catchment) TotalInsuredFamilies
		FROM tblInsureePolicy IP
		INNER JOIN tblInsuree I ON I.InsureeId = IP.InsureeId
		INNER JOIN tblFamilies F ON F.InsureeID = I.InsureeID
		INNER JOIN tblHFCatchment HC ON HC.LocationId = F.LocationId
		--INNER JOIN uvwLocations L ON L.LocationId = HC.LocationId
		INNER JOIN tblPolicy PL ON PL.PolicyID = IP.PolicyId
		WHERE HC.ValidityTo IS NULL 
		AND I.ValidityTo IS NULL
		AND IP.ValidityTo IS NULL
		AND F.ValidityTo IS NULL
		AND PL.ValidityTo IS NULL
		AND IP.EffectiveDate <= @LastDay 
		AND IP.ExpiryDate > @LastDay
		AND PL.ProdID = @ProdId
		GROUP BY HC.HFID, Catchment--, L.LocationId
	), Claims AS
	(
		SELECT C.HFID,  COUNT(C.ClaimId)TotalClaims
		FROM tblClaim C
		INNER JOIN (
			SELECT ClaimId FROM tblClaimItems WHERE ProdId = @ProdId AND ValidityTo IS NULL
			UNION
			SELECT ClaimId FROM tblClaimServices WHERE ProdId = @ProdId AND ValidityTo IS NULL
			) CProd ON CProd.ClaimID = C.ClaimID
		WHERE C.ValidityTo IS NULL
		AND C.ClaimStatus >= 8
		AND YEAR(C.DateProcessed) = @Year
		AND MONTH(C.DateProcessed) = @Month
		GROUP BY C.HFID
	), ClaimValues AS
	(
		SELECT HFID, @ProdId ProdId, SUM(PriceValuated)TotalAdjusted
		FROM(
		SELECT C.HFID, CValue.PriceValuated
		FROM tblClaim C
		INNER JOIN (
			SELECT ClaimId, PriceValuated FROM tblClaimItems WHERE ValidityTo IS NULL AND ProdId = @ProdId
			UNION ALL
			SELECT ClaimId, PriceValuated FROM tblClaimServices WHERE ValidityTo IS NULL AND ProdId = @ProdId
			) CValue ON CValue.ClaimID = C.ClaimID
		WHERE C.ValidityTo IS NULL
		AND C.ClaimStatus >= 8
		AND YEAR(C.DateProcessed) = @Year
		AND MONTH(C.DateProcessed) = @Month
		)CValue
		GROUP BY HFID
	),Locations AS
	(
		SELECT 0 LocationId, N'National' LocationName, NULL ParentLocationId
		UNION
		SELECT LocationId,LocationName, ISNULL(ParentLocationId, 0) FROM tblLocations WHERE ValidityTo IS NULL AND LocationId = ISNULL(@DistrictId, @RegionId)
		UNION ALL
		SELECT L.LocationId, L.LocationName, L.ParentLocationId 
		FROM tblLocations L 
		INNER JOIN Locations ON Locations.LocationId = L.ParentLocationId
		WHERE L.validityTo IS NULL
		AND L.LocationType IN ('R', 'D')
	), Allocation AS
	(
		SELECT ProdId, CAST(SUM(ISNULL(Allocated, 0)) AS DECIMAL(18, 6))Allocated
		FROM
		(SELECT PL.ProdID,
		CASE 
		WHEN MONTH(DATEADD(D,-1,PL.ExpiryDate)) = @Month AND YEAR(DATEADD(D,-1,PL.ExpiryDate)) = @Year AND (DAY(PL.ExpiryDate)) > 1
			THEN CASE WHEN DATEDIFF(D,CASE WHEN PR.PayDate < @FirstDay THEN @FirstDay ELSE PR.PayDate END,PL.ExpiryDate) = 0 THEN 1 ELSE DATEDIFF(D,CASE WHEN PR.PayDate < @FirstDay THEN @FirstDay ELSE PR.PayDate END,PL.ExpiryDate) END  * ((SUM(PR.Amount))/(CASE WHEN (DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate)) <= 0 THEN 1 ELSE DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) END))
		WHEN MONTH(CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END) = @Month AND YEAR(CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END) = @Year
			THEN ((@DaysInMonth + 1 - DAY(CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END)) * ((SUM(PR.Amount))/CASE WHEN DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) <= 0 THEN 1 ELSE DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) END)) 
		WHEN PL.EffectiveDate < @FirstDay AND PL.ExpiryDate > @LastDay AND PR.PayDate < @FirstDay
			THEN @DaysInMonth * (SUM(PR.Amount)/CASE WHEN (DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,DATEADD(D,-1,PL.ExpiryDate))) <= 0 THEN 1 ELSE DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) END)
		END Allocated
		FROM tblPremium PR 
		INNER JOIN tblPolicy PL ON PR.PolicyID = PL.PolicyID
		INNER JOIN tblProduct Prod ON Prod.ProdId = PL.ProdID
		INNER JOIN Locations L ON ISNULL(Prod.LocationId, 0) = L.LocationId
		WHERE PR.ValidityTo IS NULL
		AND PL.ValidityTo IS NULL
		AND PL.ProdID = @ProdId
		AND PL.PolicyStatus <> 1
		AND PR.PayDate <= PL.ExpiryDate
		GROUP BY PL.ProdID, PL.ExpiryDate, PR.PayDate,PL.EffectiveDate)Alc
		GROUP BY ProdId
	)



	,ReportData AS
	(
		SELECT L.RegionCode, L.RegionName, L.DistrictCode, L.DistrictName, HF.HFCode, HF.HFName, Hf.AccCode, HL.Name HFLevel, SL.HFSublevelDesc HFSublevel,
		PF.[Population] [Population], PF.TotalFamilies TotalFamilies, II.TotalInsuredInsuree, IFam.TotalInsuredFamilies, C.TotalClaims, CV.TotalAdjusted
		,(
			  ISNULL(ISNULL(PF.[Population], 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightPopulation)) /  NULLIF(SUM(PF.[Population])OVER(),0),0)  
			+ ISNULL(ISNULL(PF.TotalFamilies, 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightNumberFamilies)) /NULLIF(SUM(PF.[TotalFamilies])OVER(),0),0) 
			+ ISNULL(ISNULL(II.TotalInsuredInsuree, 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightInsuredPopulation)) /NULLIF(SUM(II.TotalInsuredInsuree)OVER(),0),0) 
			+ ISNULL(ISNULL(IFam.TotalInsuredFamilies, 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightNumberInsuredFamilies)) /NULLIF(SUM(IFam.TotalInsuredFamilies)OVER(),0),0) 
			+ ISNULL(ISNULL(C.TotalClaims, 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightNumberVisits)) /NULLIF(SUM(C.TotalClaims)OVER() ,0),0) 
			+ ISNULL(ISNULL(CV.TotalAdjusted, 0) * (Allocation.Allocated * (0.01 * @ShareContribution) * (0.01 * @WeightAdjustedAmount)) /NULLIF(SUM(CV.TotalAdjusted)OVER(),0),0)

		) PaymentCathment

		, Allocation.Allocated * (0.01 * @WeightPopulation) * (0.01 * @ShareContribution) AlcContriPopulation
		, Allocation.Allocated * (0.01 * @WeightNumberFamilies) * (0.01 * @ShareContribution) AlcContriNumFamilies
		, Allocation.Allocated * (0.01 * @WeightInsuredPopulation) * (0.01 * @ShareContribution) AlcContriInsPopulation
		, Allocation.Allocated * (0.01 * @WeightNumberInsuredFamilies) * (0.01 * @ShareContribution) AlcContriInsFamilies
		, Allocation.Allocated * (0.01 * @WeightNumberVisits) * (0.01 * @ShareContribution) AlcContriVisits
		, Allocation.Allocated * (0.01 * @WeightAdjustedAmount) * (0.01 * @ShareContribution) AlcContriAdjustedAmount

		,  ISNULL((Allocation.Allocated * (0.01 * @WeightPopulation) * (0.01 * @ShareContribution))/ NULLIF(SUM(PF.[Population]) OVER(),0),0) UPPopulation
		,  ISNULL((Allocation.Allocated * (0.01 * @WeightNumberFamilies) * (0.01 * @ShareContribution))/NULLIF(SUM(PF.TotalFamilies) OVER(),0),0) UPNumFamilies
		,  ISNULL((Allocation.Allocated * (0.01 * @WeightInsuredPopulation) * (0.01 * @ShareContribution))/NULLIF(SUM(II.TotalInsuredInsuree) OVER(),0),0) UPInsPopulation
		,  ISNULL((Allocation.Allocated * (0.01 * @WeightNumberInsuredFamilies) * (0.01 * @ShareContribution))/ NULLIF(SUM(IFam.TotalInsuredFamilies) OVER(),0),0) UPInsFamilies
		,  ISNULL((Allocation.Allocated * (0.01 * @WeightNumberVisits) * (0.01 * @ShareContribution)) / NULLIF(SUM(C.TotalClaims) OVER(),0),0) UPVisits
		,  ISNULL((Allocation.Allocated * (0.01 * @WeightAdjustedAmount) * (0.01 * @ShareContribution))/ NULLIF(SUM(CV.TotalAdjusted) OVER(),0),0) UPAdjustedAmount




		FROM tblHF HF
		INNER JOIN @HFLevel HL ON HL.Code = HF.HFLevel
		LEFT OUTER JOIN tblHFSublevel SL ON SL.HFSublevel = HF.HFSublevel
		LEFT JOIN uvwLocations L ON L.LocationId = HF.LocationId
		LEFT OUTER JOIN TotalPopFam PF ON PF.HFID = HF.HfID
		LEFT OUTER JOIN InsuredInsuree II ON II.HFID = HF.HfID
		LEFT OUTER JOIN InsuredFamilies IFam ON IFam.HFID = HF.HfID
		LEFT OUTER JOIN Claims C ON C.HFID = HF.HfID
		LEFT OUTER JOIN ClaimValues CV ON CV.HFID = HF.HfID
		LEFT OUTER JOIN Allocation ON Allocation.ProdID = @ProdId

		WHERE HF.ValidityTo IS NULL
		AND (((L.RegionId = @RegionId OR @RegionId IS NULL) AND (L.DistrictId = @DistrictId OR @DistrictId IS NULL)) OR CV.ProdID IS NOT NULL OR II.ProdId IS NOT NULL)
		AND (HF.HFLevel IN (@Level1, @Level2, @Level3, @Level4) OR (@Level1 IS NULL AND @Level2 IS NULL AND @Level3 IS NULL AND @Level4 IS NULL))
		AND(
			((HF.HFLevel = @Level1 OR @Level1 IS NULL) AND (HF.HFSublevel = @Sublevel1 OR @Sublevel1 IS NULL))
			OR ((HF.HFLevel = @Level2 ) AND (HF.HFSublevel = @Sublevel2 OR @Sublevel2 IS NULL))
			OR ((HF.HFLevel = @Level3) AND (HF.HFSublevel = @Sublevel3 OR @Sublevel3 IS NULL))
			OR ((HF.HFLevel = @Level4) AND (HF.HFSublevel = @Sublevel4 OR @Sublevel4 IS NULL))
		  )

	)



	SELECT  MAX (RegionCode)RegionCode, 
			MAX(RegionName)RegionName,
			MAX(DistrictCode)DistrictCode,
			MAX(DistrictName)DistrictName,
			HFCode, 
			MAX(HFName)HFName,
			MAX(AccCode)AccCode, 
			MAX(HFLevel)HFLevel, 
			MAX(HFSublevel)HFSublevel,
			ISNULL(SUM([Population]),0)[Population],
			ISNULL(SUM(TotalFamilies),0)TotalFamilies,
			ISNULL(SUM(TotalInsuredInsuree),0)TotalInsuredInsuree,
			ISNULL(SUM(TotalInsuredFamilies),0)TotalInsuredFamilies,
			ISNULL(MAX(TotalClaims), 0)TotalClaims,
			ISNULL(SUM(AlcContriPopulation),0)AlcContriPopulation,
			ISNULL(SUM(AlcContriNumFamilies),0)AlcContriNumFamilies,
			ISNULL(SUM(AlcContriInsPopulation),0)AlcContriInsPopulation,
			ISNULL(SUM(AlcContriInsFamilies),0)AlcContriInsFamilies,
			ISNULL(SUM(AlcContriVisits),0)AlcContriVisits,
			ISNULL(SUM(AlcContriAdjustedAmount),0)AlcContriAdjustedAmount,
			ISNULL(SUM(UPPopulation),0)UPPopulation,
			ISNULL(SUM(UPNumFamilies),0)UPNumFamilies,
			ISNULL(SUM(UPInsPopulation),0)UPInsPopulation,
			ISNULL(SUM(UPInsFamilies),0)UPInsFamilies,
			ISNULL(SUM(UPVisits),0)UPVisits,
			ISNULL(SUM(UPAdjustedAmount),0)UPAdjustedAmount,
			ISNULL(SUM(PaymentCathment),0)PaymentCathment,
			ISNULL(SUM(TotalAdjusted),0)TotalAdjusted
	
	 FROM ReportData

	 GROUP BY HFCode



END
GO

ALTER VIEW [dw].[uvwNumberOfInsuredHouseholds]
AS
	WITH RowData AS
	(
		SELECT F.FamilyID, DATEADD(MONTH,MonthCount.Numbers, EOMONTH(PL.EffectiveDate, 0)) ActiveDate, 
		R.RegionName Region, D.DistrictName, W.WardName, V.VillageName
		FROM tblPolicy PL 
		INNER JOIN tblFamilies F ON PL.FamilyId = F.FamilyID
		INNER JOIN tblVillages V ON V.VillageId = F.LocationId
		INNER JOIN tblWards W ON W.WardId = V.WardId
		INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
		INNER JOIN tblRegions R ON D.Region = R.RegionId 
		CROSS APPLY(VALUES (0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11))MonthCount(Numbers)
		WHERE PL.ValidityTo IS NULL
		AND F.ValidityTo IS NULL
		AND R.ValidityTo IS NULL
		AND D.ValidityTo IS NULL
		AND W.ValidityTo IS NULL
		AND V.ValidityTo IS NULL
		AND PL.EffectiveDate IS NOT NULL
		GROUP BY F.FamilyID,R.RegionName,D.DistrictName,W.WardName,v.VillageName,DATEADD(MONTH,MonthCount.Numbers, EOMONTH(PL.EffectiveDate, 0)) 
	), RowData2 AS
	(
		SELECT FamilyId, ActiveDate, Region, DistrictName, WardName, VillageName
		FROM RowData
		GROUP BY FamilyId, ActiveDate, Region, DistrictName, WardName, VillageName
	)
	SELECT COUNT(FamilyId) InsuredHouseholds, MONTH(ActiveDate)MonthTime, DATENAME(Q, ActiveDate)QuarterTime, YEAR(ActiveDate)YearTime, Region, DistrictName, WardName, VillageName
	FROM RowData2
	GROUP BY ActiveDate, Region, DistrictName, WardName, VillageName

GO

IF NOT OBJECT_ID('uspSSRSGetClaimOverView') IS NULL
DROP PROCEDURE uspSSRSGetClaimOverView

GO

CREATE PROCEDURE [dbo].[uspSSRSGetClaimOverView]
(
	@HFID INT,
	@LocationId INT,
	@ProdId INT, 
	@StartDate DATE, 
	@EndDate DATE,
	@ClaimStatus INT = NULL
)
AS
BEGIN
	;WITH TotalForItems AS
	(
		SELECT C.ClaimId, SUM(CI.PriceAsked * CI.QtyProvided)Claimed,
		SUM(ISNULL(CI.PriceApproved, CI.PriceAsked) * ISNULL(CI.QtyApproved, CI.QtyProvided)) Approved,
		SUM(CI.PriceValuated)Adjusted,
		SUM(CI.RemuneratedAmount)Remunerated
		FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
		WHERE C.ValidityTo IS NULL
		AND CI.ValidityTo IS NULL
		GROUP BY C.ClaimID
	), TotalForServices AS
	(
		SELECT C.ClaimId, SUM(CS.PriceAsked * CS.QtyProvided)Claimed,
		SUM(ISNULL(CS.PriceApproved, CS.PriceAsked) * ISNULL(CS.QtyApproved, CS.QtyProvided)) Approved,
		SUM(CS.PriceValuated)Adjusted,
		SUM(CS.RemuneratedAmount)Remunerated
		FROM tblClaim C 
		LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
		WHERE C.ValidityTo IS NULL
		AND CS.ValidityTo IS NULL
		GROUP BY C.ClaimID
	)

	SELECT C.DateClaimed, C.ClaimID, I.ItemId, S.ServiceID, HF.HFCode, HF.HFName, C.ClaimCode, C.DateClaimed, CA.LastName + ' ' + CA.OtherNames ClaimAdminName,
	C.DateFrom, C.DateTo, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsureeName,
	CASE C.ClaimStatus WHEN 1 THEN N'Rejected' WHEN 2 THEN N'Entered' WHEN 4 THEN N'Checked' WHEN 8 THEN N'Processed' WHEN 16 THEN N'Valuated' END ClaimStatus,
	C.RejectionReason, COALESCE(TFI.Claimed + TFS.Claimed, TFI.Claimed, TFS.Claimed) Claimed, 
	COALESCE(TFI.Approved + TFS.Approved, TFI.Approved, TFS.Approved) Approved,
	COALESCE(TFI.Adjusted + TFS.Adjusted, TFI.Adjusted, TFS.Adjusted) Adjusted,
	COALESCE(TFI.Remunerated + TFS.Remunerated, TFI.Remunerated, TFS.Remunerated)Paid,
	CASE WHEN CI.RejectionReason <> 0 THEN I.ItemCode ELSE NULL END RejectedItem, CI.RejectionReason ItemRejectionCode,
	CASE WHEN CS.RejectionReason > 0 THEN S.ServCode ELSE NULL END RejectedService, CS.RejectionReason ServiceRejectionCode,
	CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN I.ItemCode ELSE NULL END AdjustedItem,
	CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN CI.QtyProvided ELSE NULL END OrgQtyItem,
	CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved ,CI.QtyProvided)  THEN CI.QtyApproved ELSE NULL END AdjQtyItem,
	CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)  THEN S.ServCode ELSE NULL END AdjustedService,
	CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)   THEN CS.QtyProvided ELSE NULL END OrgQtyService,
	CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved ,CS.QtyProvided)   THEN CS.QtyApproved ELSE NULL END AdjQtyService,
	C.Explanation


	FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
	LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
	LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID
	LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID
	INNER JOIN tblHF HF ON C.HFID = HF.HfID
	LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
	INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
	LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
	LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId

	WHERE C.ValidityTo IS NULL
	AND ISNULL(C.DateTo,C.DateFrom) BETWEEN @StartDate AND @EndDate
	AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
	AND HF.LocationId = @LocationId OR @LocationId = 0
	AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) 
	--OR  (CI.ProdID IS NOT NULL OR CS.ProdID IS NOT NULL)	-- --Added by Rogers
	AND HF.HFID = @HFID
END

GO


IF NOT OBJECT_ID('uspCleanTablesNew') IS NULL
DROP PROCEDURE uspCleanTablesNew
GO

CREATE PROCEDURE [dbo].[uspCleanTablesNew]
	@OffLine as int = 0		--0: For Online, 1: HF Offline, 2: CHF Offline
AS
BEGIN
	
	DECLARE @LocationId INT
	DECLARE @ParentLocationId INT

	--SELECT @ParentLocationId = ParentLocationId, @LocationId =LocationId  FROM tblLocations WHERE LocationName='Dummy' AND ValidityTo IS NULL AND LocationType = N'D'
	 
	--Phase 2
	DELETE FROM tblFeedbackPrompt;
	EXEC [UspS_ReseedTable] 'tblFeedbackPrompt';
	DELETE FROM tblReporting;
	EXEC [UspS_ReseedTable] 'tblReporting';
	DELETE FROM tblSubmittedPhotos;
	EXEC [UspS_ReseedTable] 'tblSubmittedPhotos';
	DELETE FROM dbo.tblFeedback;
	EXEC [UspS_ReseedTable] 'tblFeedback';
	DELETE FROM dbo.tblClaimServices;
	EXEC [UspS_ReseedTable] 'tblClaimServices';
	DELETE FROM dbo.tblClaimItems ;
	EXEC [UspS_ReseedTable] 'tblClaimItems';
	DELETE FROM dbo.tblClaimDedRem;
	EXEC [UspS_ReseedTable] 'tblClaimDedRem';
	DELETE FROM dbo.tblClaim;
	EXEC [UspS_ReseedTable] 'tblClaim';
	DELETE FROM dbo.tblClaimAdmin
	EXEC [USPS_ReseedTable] 'tblClaimAdmin'
	DELETE FROM dbo.tblICDCodes;
	EXEC [UspS_ReseedTable] 'tblICDCodes'
	
	
	DELETE FROM dbo.tblRelDistr;
	EXEC [UspS_ReseedTable] 'tblRelDistr';
	DELETE FROM dbo.tblRelIndex ;
	EXEC [UspS_ReseedTable] 'tblRelIndex';
	DELETE FROM dbo.tblBatchRun;
	EXEC [UspS_ReseedTable] 'tblBatchRun';
	DELETE FROM dbo.tblExtracts;
	EXEC [UspS_ReseedTable] 'tblExtracts';
	TRUNCATE TABLE tblPremium;
	
	--Phase 1
	EXEC [UspS_ReseedTable] 'tblPremium';
	DELETE FROM tblPayer;
	EXEC [UspS_ReseedTable] 'tblPayer';

	DELETE FROM dbo.tblPolicyRenewalDetails;
	EXEC [UspS_ReseedTable] 'tblPolicyRenewalDetails';
	DELETE FROM dbo.tblPolicyRenewals;
	EXEC [UspS_ReseedTable] 'tblPolicyRenewals';


	DELETE FROM tblInsureePolicy;
	EXEC [UspS_ReseedTable] 'tblInsureePolicy';
	DELETE FROM tblPolicy;
	EXEC [UspS_ReseedTable] 'tblPolicy';
	DELETE FROM tblProductItems;
	EXEC [UspS_ReseedTable] 'tblProductItems';
	DELETE FROM tblProductServices;
	EXEC [UspS_ReseedTable] 'tblProductServices';

	DELETE FROM dbo.tblRelDistr;
	EXEC [UspS_ReseedTable] 'tblRelDistr';


	DELETE FROM tblProduct;
	EXEC [UspS_ReseedTable] 'tblProduct';
	UPDATE tblInsuree set PhotoID = NULL ;
	DELETE FROM tblPhotos;
	EXEC [UspS_ReseedTable] 'tblPhotos';
	DELETE FROM tblInsuree;
	EXEC [UspS_ReseedTable] 'tblInsuree';
	DELETE FROM tblGender;
	DELETE FROM tblFamilies;
	EXEC [UspS_ReseedTable] 'tblFamilies';
	DELETE FROM tblOfficerVillages;
	EXEC [UspS_ReseedTable] 'tblOfficerVillages';
	DELETE FROM dbo.tblOfficer;
	EXEC [UspS_ReseedTable] 'tblOfficer';
	DELETE FROM dbo.tblHFCatchment;
	EXEC [UspS_ReseedTable] 'tblHFCatchment';
	DELETE FROM dbo.tblHF;
	EXEC [UspS_ReseedTable] 'tblHF';
	DELETe FROM dbo.tblPLItemsDetail;
	EXEC [UspS_ReseedTable] 'tblPLItemsDetail';
	DELETE FROM dbo.tblPLItems;
	EXEC [UspS_ReseedTable] 'tblPLItems';
	DELETE FROM dbo.tblItems;
	EXEC [UspS_ReseedTable] 'tblItems';
	DELETE FROM dbo.tblPLServicesDetail;
	EXEC [UspS_ReseedTable] 'tblPLServicesDetail';
	DELETE FROM dbo.tblPLServices;
	EXEC [UspS_ReseedTable] 'tblPLServices';
	DELETE FROM dbo.tblServices;
	EXEC [UspS_ReseedTable] 'tblServices';
	DELETE FROM dbo.tblUsersDistricts;
	EXEC [UspS_ReseedTable] 'tblUsersDistricts';


	DELETE FROM tblLocations;
	EXEC [UspS_ReseedTable] 'tblLocations';
	DELETE FROM dbo.tblLogins ;
	EXEC [UspS_ReseedTable] 'tblLogins';

	DELETE FROM dbo.tblUsers;
	EXEC [UspS_ReseedTable] 'tblUsers';

	TRUNCATE TABLE tblFromPhone;
	EXEC [UspS_ReseedTable] 'tblFromPhone';

	TRUNCATE TABLE tblEmailSettings;

	DBCC SHRINKDATABASE (0);
	
	--Drop the encryption set
	IF EXISTS(SELECT * FROM sys.symmetric_keys WHERE name = N'EncryptionKey')
	DROP SYMMETRIC KEY EncryptionKey;
		
	IF EXISTS(SELECT * FROM sys.certificates WHERE name = N'EncryptData')
	DROP CERTIFICATE EncryptData;
	
	IF EXISTS(SELECT * FROM sys.symmetric_keys WHERE symmetric_key_id = 101)
	DROP MASTER KEY;
	
	
	
	
	
	--insert new user Admin-Admin
	IF @OffLine = 2  --CHF offline
	BEGIN
		
        INSERT INTO tblUsers ([LastName],[OtherNames],[Phone],[LoginName],[RoleID],[LanguageID],[HFID],[AuditUserID],StoredPassword,PrivateKey)
        VALUES('Admin', 'Admin', '', 'Admin', 1048576,'en',0,0
		--storedPassword
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CONCAT(CAST('Admin' AS VARCHAR(MAX)),CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2))),2)
		 -- PrivateKey
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2)
		)
       
        UPDATE tblIMISDefaults SET OffLineHF = 0,OfflineCHF = 0, FTPHost = '',FTPUser = '', FTPPassword = '',FTPPort = 0,FTPClaimFolder = '',FtpFeedbackFolder = '',FTPPolicyRenewalFolder = '',FTPPhoneExtractFolder = '',FTPOfflineExtractFolder = '',AppVersionEnquire = 0,AppVersionEnroll = 0,AppVersionRenewal = 0,AppVersionFeedback = 0,AppVersionClaim = 0, AppVersionImis = 0, DatabaseBackupFolder = ''
        
	END
	
	
	IF @OffLine = 1 --HF offline
	BEGIN
		
        INSERT INTO tblUsers ([LastName],[OtherNames],[Phone],[LoginName],[RoleID],[LanguageID],[HFID],[AuditUserID],StoredPassword,PrivateKey)
        VALUES('Admin', 'Admin', '', 'Admin', 524288,'en',0,0
		--storedPassword
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CONCAT(CAST('Admin' AS VARCHAR(MAX)),CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2))),2)
		 -- PrivateKey
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2)
		)
        
        UPDATE tblIMISDefaults SET OffLineHF = 0,OfflineCHF = 0,FTPHost = '',FTPUser = '', FTPPassword = '',FTPPort = 0,FTPClaimFolder = '',FtpFeedbackFolder = '',FTPPolicyRenewalFolder = '',FTPPhoneExtractFolder = '',FTPOfflineExtractFolder = '',AppVersionEnquire = 0,AppVersionEnroll = 0,AppVersionRenewal = 0,AppVersionFeedback = 0,AppVersionClaim = 0, AppVersionImis = 0, DatabaseBackupFolder = ''
        
	END
	IF @OffLine = 0 --ONLINE CREATION NEW COUNTRY NO DEFAULTS KEPT
	BEGIN
		
        INSERT INTO tblUsers ([LastName],[OtherNames],[Phone],[LoginName],[RoleID],[LanguageID],[HFID],[AuditUserID],StoredPassword,PrivateKey)
        VALUES('Admin', 'Admin', '', 'Admin',  1023,'en',0,0
		--storedPassword
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CONCAT(CAST('Admin' AS VARCHAR(MAX)),CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2))),2)
		 -- PrivateKey
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2)
		)
		
       	UPDATE tblIMISDefaults SET OffLineHF = 0,OfflineCHF = 0,FTPHost = '',FTPUser = '', FTPPassword = '',FTPPort = 0,FTPClaimFolder = '',FtpFeedbackFolder = '',FTPPolicyRenewalFolder = '',FTPPhoneExtractFolder = '',FTPOfflineExtractFolder = '',AppVersionEnquire = 0,AppVersionEnroll = 0,AppVersionRenewal = 0,AppVersionFeedback = 0,AppVersionClaim = 0, AppVersionImis = 0,				DatabaseBackupFolder = ''
    END
	
	IF @OffLine = -1 --ONLINE CREATION WITH DEFAULTS KEPT AS PREVIOUS CONTENTS
	BEGIN
		
        INSERT INTO tblUsers ([LastName],[OtherNames],[Phone],[LoginName],[RoleID],[LanguageID],[HFID],[AuditUserID],StoredPassword,PrivateKey)
        VALUES('Admin', 'Admin', '', 'Admin', 1023,'en',0,0
		--storedPassword
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CONCAT(CAST('Admin' AS VARCHAR(MAX)),CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2))),2)
		 -- PrivateKey
		,CONVERT(varchar(max),HASHBYTES('SHA2_256',CAST('Admin' AS VARCHAR(MAX))),2)
		)
        UPDATE tblIMISDefaults SET OffLineHF = 0,OfflineCHF = 0
    END


	SET IDENTITY_INSERT tblLocations ON
	INSERT INTO tblLocations(LocationId, LocationCode, Locationname, LocationType, AuditUserId, ParentLocationId) VALUES
	(1, N'R0001', N'Region', N'R', -1, NULL),
	(2, N'D0001', N'Dummy', N'D', -1, 1)
	SET IDENTITY_INSERT tblLocations OFF
		
	INSERT INTO tblUsersDistricts ([UserID],[LocationId],[AuditUserID]) VALUES (1,2,-1)
END

