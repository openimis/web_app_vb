USE [IMIS]
GO

/****** Object:  View [dw].[uvwNumberOfInsuredHouseholds]    Script Date: 10/12/2018 11:58:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

