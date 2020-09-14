''Copyright (c) 2016-2017 Swiss Agency for Development and Cooperation (SDC)
''
''The program users must agree to the following terms:
''
''Copyright notices
''This program is free software: you can redistribute it and/or modify it under the terms of the GNU AGPL v3 License as published by the 
''Free Software Foundation, version 3 of the License.
''This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of 
''MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU AGPL v3 License for more details www.gnu.org.
''
''Disclaimer of Warranty
''There is no warranty for the program, to the extent permitted by applicable law; except when otherwise stated in writing the copyright 
''holders and/or other parties provide the program "as is" without warranty of any kind, either expressed or implied, including, but not 
''limited to, the implied warranties of merchantability and fitness for a particular purpose. The entire risk as to the quality and 
''performance of the program is with you. Should the program prove defective, you assume the cost of all necessary servicing, repair or correction.
''
''Limitation of Liability 
''In no event unless required by applicable law or agreed to in writing will any copyright holder, or any other party who modifies and/or 
''conveys the program as permitted above, be liable to you for damages, including any general, special, incidental or consequential damages 
''arising out of the use or inability to use the program (including but not limited to loss of data or data being rendered inaccurate or losses 
''sustained by you or third parties or a failure of the program to operate with any other programs), even if such holder or other party has been 
''advised of the possibility of such damages.
''
''In case of dispute arising out or in relation to the use of the program, it is subject to the public law of Switzerland. The place of jurisdiction is Berne.
'
' 
'

Public Class ReportDAL
    'Corrected
    Public Function GetPremiumCollection(ByVal LocationId As Integer, ByVal Product As Integer, ByVal PaymentType As String, ByVal FromDate As Date, ByVal ToDate As Date, ByVal dtPaymentType As DataTable) As DataTable
        Dim data As New ExactSQL
        ' Dim sSQL As String = "uspSSRSPremiumCollection"
        Dim sSQL As String = "IF @LocationId=-1
	                            SET @LocationId = 0

	        SELECT LF.RegionName, LF.DistrictName
	        ,Prod.ProductCode,Prod.ProductName,SUM(Pr.Amount) Amount, 
	        PT.Name PayType,Pr.PayDate,Prod.AccCodePremiums 

	        FROM tblPremium PR 
	        INNER JOIN tblPolicy PL ON PR.PolicyID = PL.PolicyID
	        INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
	        INNER JOIN tblFamilies F ON F.FamilyId = PL.FamilyID
	        INNER JOIN uvwLocations LF ON LF.VillageId = F.LocationId
	        INNER JOIN @dtPaymentType PT ON PT.Code = PR.PayType

	        WHERE Prod.ValidityTo IS NULL 
	        AND PR.ValidityTo IS NULL 
	        AND F.ValidityTo  IS NULL
	
	        AND (Prod.ProdId = @Product OR @Product = 0)
	        AND (Pr.PayType = @PaymentType OR @PaymentType = '')
	        AND Pr.PayDate BETWEEN @FromDate AND @ToDate
	        AND (LF.RegionId = @LocationId OR LF.DistrictId = @LocationId OR    @LocationId =0 ) --OR ISNULL(Prod.LocationId, 0) = ISNULL(@LocationId, 0) BY Rogers
	
	        GROUP BY LF.RegionName, LF.DistrictName, Prod.ProductCode,Prod.ProductName,Pr.PayDate,Pr.PayType,Prod.AccCodePremiums, PT.Name"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@Product", SqlDbType.Int, Product)
        data.params("@PaymentType", SqlDbType.VarChar, 2, PaymentType)
        data.params("@FromDate", SqlDbType.Date, FromDate)
        data.params("@ToDate", SqlDbType.Date, ToDate)
        data.params("@dtPaymentType", dtPaymentType, "xCareType")

        Return data.Filldata


    End Function

    'Corrected
    Public Function GetPolicySold(ByVal LocationId As Integer, ByVal Product As Integer, ByVal FromDate As Date, ByVal ToDate As Date) As DataTable
        Dim data As New ExactSQL
        'Dim sSQL As String = "uspSSRSProductSales"
        Dim sSQL As String = " IF @LocationId = -1
		                          SET @LocationId=NULL
	        SELECT L.DistrictName,Prod.ProductCode,Prod.ProductName,PL.EffectiveDate, SUM(PL.PolicyValue) PolicyValue
	        FROM tblPolicy PL 
	        INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
	        INNER JOIN tblFamilies F ON PL.FamilyId = F.FamilyID
	        INNER JOIN uvwLocations L ON L.VillageId = F.LocationId
	        WHERE PL.ValidityTo IS NULL 
	        AND Prod.ValidityTo IS NULL 
	        AND F.validityTo IS NULL
	        AND (L.RegionId = @LocationId OR L.DistrictId = @LocationId OR ISNULL(@LocationId, 0) = 0)
	        AND (Prod.ProdID = @Product OR @Product = 0)
	        AND PL.EffectiveDate BETWEEN @FromDate AND @ToDate
	        GROUP BY L.DistrictName,Prod.ProductCode,Prod.ProductName,PL.EffectiveDate"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@Product", SqlDbType.Int, Product)
        data.params("@FromDate", SqlDbType.Date, FromDate)
        data.params("@ToDate", SqlDbType.Date, ToDate)

        Return data.Filldata


    End Function

    'Corrected
    Public Function GetPremiumDistribution(ByVal LocationId As Integer, ByVal Product As Integer, ByVal Month As Integer, ByVal Year As Integer) As DataTable
        Dim data As New ExactSQL
        'Dim sSQL As String = "uspSSRSPremiumDistribution"
        Dim sSQL As String = " IF NOT OBJECT_ID('tempdb..#tmpResult') IS NULL DROP TABLE #tmpResult
	
	CREATE TABLE #tmpResult(
		MonthID INT,
		DistrictName NVARCHAR(50),
		ProductCode NVARCHAR(8),
		ProductName NVARCHAR(100),
		TotalCollected DECIMAL(18,4),
		NotAllocated DECIMAL(18,4),
		Allocated DECIMAL(18,4)
	)

	DECLARE @Date DATE,
			@DaysInMonth INT,
			@Counter INT = 1,
			@MaxCount INT = 12,
			@EndDate DATE

	IF @Month > 0
	BEGIN
		SET @Counter = @Month
		SET @MaxCount = @Month
	END

	IF @LocationId = -1
	SET @LocationId = NULL

	WHILE @Counter <> @MaxCount + 1
	BEGIN	
		SELECT @Date = CAST(CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Counter AS VARCHAR(2)) + '-' + '01' AS DATE)
		SELECT @DaysInMonth = DATEDIFF(DAY,@Date,DATEADD(MONTH,1,@Date))
		SELECT @EndDate = CAST(CONVERT(VARCHAR(4),@Year) + '-' + CONVERT(VARCHAR(2),@Counter) + '-' + CONVERT(VARCHAR(2),@DaysInMonth) AS DATE)
			
		
		;WITH Locations AS
		(
			SELECT 0 LocationId, N'National' LocationName, NULL ParentLocationId
			UNION
			SELECT LocationId,LocationName, ISNULL(ParentLocationId, 0) FROM tblLocations WHERE ValidityTo IS NULL AND LocationId = @LocationId
			UNION ALL
			SELECT L.LocationId, L.LocationName, L.ParentLocationId 
			FROM tblLocations L 
			INNER JOIN Locations ON Locations.LocationId = L.ParentLocationId
			WHERE L.validityTo IS NULL
			AND L.LocationType IN ('R', 'D')
		)
		INSERT INTO #tmpResult
		SELECT MonthId,DistrictName,ProductCode,ProductName,SUM(ISNULL(TotalCollected,0))TotalCollected,SUM(ISNULL(NotAllocated,0))NotAllocated,SUM(ISNULL(Allocated,0))Allocated
		FROM 
		(
		SELECT @Counter MonthId,L.LocationName DistrictName,Prod.ProductCode,Prod.ProductName,
		SUM(PR.Amount) TotalCollected,
		0 NotAllocated,
		0 Allocated
		FROM tblPremium PR 
		RIGHT OUTER JOIN tblPolicy PL ON PR.PolicyID = PL.PolicyID
		INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID 
		INNER JOIN Locations L ON  ISNULL(Prod.LocationId, 0) = L.LocationId
		WHERE PR.ValidityTo IS NULL
		AND PL.ValidityTo IS NULL
		AND Prod.ValidityTo IS NULL
		AND PL.PolicyStatus <> 1
		AND (Prod.ProdId = @ProductId OR @ProductId IS NULL)
		AND MONTH(PR.PayDate) = @Counter
		AND YEAR(PR.PayDate) = @Year
		GROUP BY L.LocationName,Prod.ProductCode,Prod.ProductName,PR.Amount,PR.PayDate,PL.ExpiryDate

		UNION ALL

		SELECT @Counter MonthId,L.LocationName DistrictName,Prod.ProductCode,Prod.ProductName,
		0 TotalCollected,
		SUM(PR.Amount) NotAllocated,
		0 Allocated
		FROM tblPremium AS PR INNER JOIN tblPolicy AS PL ON PR.PolicyID = PL.PolicyID
		INNER JOIN tblProduct AS Prod ON PL.ProdID = Prod.ProdID 
		INNER JOIN Locations AS L ON ISNULL(Prod.LocationId, 0) = L.LocationId
		WHERE PR.ValidityTo IS NULL AND PL.ValidityTo IS NULL AND Prod.ValidityTo IS NULL
		AND (MONTH(PR.PayDate ) = @Counter) 
		AND (YEAR(PR.PayDate) = @Year) 
		AND (Prod.ProdId = @ProductId OR @ProductId IS NULL) 
		AND (PL.PolicyStatus = 1)
		GROUP BY L.LocationName,Prod.ProductCode,Prod.ProductName,PR.Amount,PR.PayDate,PL.ExpiryDate

		UNION ALL

		SELECT @Counter MonthId,L.LocationName DistrictName,Prod.ProductCode,Prod.ProductName,
		0 TotalCollected,
		SUM(PR.Amount) NotAllocated,
		0 Allocated
		FROM tblPremium AS PR INNER JOIN tblPolicy AS PL ON PR.PolicyID = PL.PolicyID
		INNER JOIN tblProduct AS Prod ON PL.ProdID = Prod.ProdID 
		INNER JOIN Locations AS L ON ISNULL(Prod.LocationId, 0) = L.LocationId
		WHERE PR.ValidityTo IS NULL AND PL.ValidityTo IS NULL AND Prod.ValidityTo IS NULL
		AND (MONTH(PR.PayDate ) = @Counter) 
		AND (YEAR(PR.PayDate) = @Year) 
		AND (Prod.ProdId = @ProductId OR @ProductId IS NULL) 
		AND (PR.PayDate > PL.ExpiryDate)
		GROUP BY L.LocationName,Prod.ProductCode,Prod.ProductName,PR.Amount,PR.PayDate,PL.ExpiryDate

		UNION ALL

		SELECT @Counter MonthId,L.LocationName DistrictName,Prod.ProductCode,Prod.ProductName,
		0 TotalCollected,
		0 NotAllocated,
		CASE 
		WHEN MONTH(DATEADD(D,-1,PL.ExpiryDate)) = @Counter AND YEAR(DATEADD(D,-1,PL.ExpiryDate)) = @Year AND (DAY(PL.ExpiryDate)) > 1
		THEN CASE WHEN DATEDIFF(D,CASE WHEN PR.PayDate < @Date THEN @Date ELSE PR.PayDate END,PL.ExpiryDate) = 0 THEN 1 ELSE DATEDIFF(D,CASE WHEN PR.PayDate < @Date THEN @Date ELSE PR.PayDate END,PL.ExpiryDate) END  * ((SUM(PR.Amount))/(CASE WHEN (DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate)) <= 0 THEN 1 ELSE DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) END))
		WHEN MONTH(CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END) = @Counter AND YEAR(CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END) = @Year
		THEN ((@DaysInMonth + 1 - DAY(CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END)) * ((SUM(PR.Amount))/CASE WHEN DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) <= 0 THEN 1 ELSE DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) END)) 
		WHEN PL.EffectiveDate < @Date AND PL.ExpiryDate > @EndDate AND PR.PayDate < @Date
		THEN @DaysInMonth * (SUM(PR.Amount)/CASE WHEN (DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,DATEADD(D,-1,PL.ExpiryDate))) <= 0 THEN 1 ELSE DATEDIFF(DAY,CASE WHEN PR.PayDate < PL.EffectiveDate THEN PL.EffectiveDate ELSE PR.PayDate END,PL.ExpiryDate) END)
		END Allocated
		FROM tblPremium PR 
		INNER JOIN tblPolicy PL ON PR.PolicyID = PL.PolicyID
		INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID 
		INNER JOIN Locations L ON ISNULL(Prod.LocationId, 0) = L.LocationId
		WHERE PR.ValidityTo IS NULL
		AND PL.ValidityTo IS NULL
		AND Prod.ValidityTo IS  NULL
		AND Prod.ProdID = @ProductID
		AND PL.PolicyStatus <> 1
		AND PR.PayDate <= PL.ExpiryDate
		GROUP BY L.LocationName,Prod.ProductCode,Prod.ProductName,PR.Amount,PR.PayDate,PL.ExpiryDate,PL.EffectiveDate
		)PremiumDistribution
		GROUP BY MonthId,DistrictName,ProductCode,ProductName

		
		SET @Counter = @Counter + 1
		
	END

	 SELECT MonthId, DistrictName,ProductCode,ProductName,TotalCollected,NotAllocated,Allocated FROM #tmpResult "
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@ProductID", SqlDbType.Int, Product)
        data.params("@Month", Month)
        data.params("@Year", Year)

        Return data.Filldata

    End Function
    Public Function GetFeedbackPrompt(ByVal SMSStatus As Integer, ByVal LocationId As Integer, ByVal WardId As Integer, ByVal VillageID As Integer, ByVal OfficerID As Integer, ByVal RangeFrom As Date, ByVal RangeTo As Date) As DataTable
        Dim data As New ExactSQL
        'Dim sSQL As String = "uspSSRSFeedbackPrompt"
        Dim sSQL As String = " IF @RangeFrom = '' SET @RangeFrom = GETDATE()
	    IF @RangeTo = '' SET @RangeTo = GETDATE()


	    SELECT D.DistrictName,W.WardName, V.VillageName,ISNULL(NULLIF(O.VEOLastName, ''), O.LastName) + ' ' + ISNULL(NULLIF(O.VEOOtherNames, ''), O.OtherNames) AS Officer, ISNULL(NULLIF(O.VEOPhone, ''), O.Phone)VEOPhone,
	    FP.FeedbackPromptDate,FP.ClaimID,C.ClaimCode, HF.HFCode, HF.HFName, I.CHFID, I.OtherNames, I.LastName, ICD.ICDName, C.DateFrom, ISNULL(C.DateTo,C.DateFrom) DateTo,FP.SMSStatus,C.Claimed
	    FROM tblFeedbackPrompt FP INNER JOIN tblClaim C ON FP.ClaimID = C.ClaimID
	    INNER JOIN tblHF HF ON C.HFID = HF.HfID
	    INNER JOIN tblICDCodes ICD ON C.ICDID = ICD.ICDID
	    INNER JOIN tblInsuree I ON C.InsureeID = I.InsureeID
	    INNER JOIN tblFamilies F ON I.FamilyID = F.FamilyID
	    INNER JOIN tblVillages V ON V.VillageID = F.LocationId
	    INNER JOIN tblWards W ON W.WardID = V.WardID
	    INNER JOIN tblDistricts D ON D.DistrictID = W.DistrictID
	    LEFT OUTER JOIN tblPolicy PL ON F.FamilyID = PL.FamilyId
	    LEFT OUTER JOIN tblOfficer O ON PL.OfficerID = O.OfficerID
	    WHERE FP.ValidityTo IS NULL 
	    AND C.ValidityTo IS NULL 
	    AND HF.ValidityTo IS NULL 
	    AND I.ValidityTo IS NULL 
	    AND F.ValidityTo IS NULL 
	    AND D.ValidityTo IS NULL 
	    AND W.ValidityTo IS NULL 
	    AND V.ValidityTo IS NULL 
	    AND PL.ValidityTo IS NULL 
	    AND O.ValidityTo IS NULL 
	    AND ICD.ValidityTo IS NULL
	    AND C.FeedbackStatus = 4
	    AND (FP.SMSStatus = @SMSStatus OR @SMSStatus = 0)
	    AND (D.DistrictID  = @LocationId OR @LocationId = 0)
	    AND (W.WardID = @WardID OR @WardID = 0)
	    AND (V.VillageID = @VillageID OR @VillageId = 0)
	    AND (O.OfficerID = @OfficerID OR @OfficerId = 0)
	    AND FP.FeedbackPromptDate BETWEEN @RangeFrom AND @RangeTo
	    GROUP BY D.DistrictName,W.WardName, V.VillageName,O.VEOLastName, O.LastName, O.VEOOtherNames, O.OtherNames, O.VEOPhone, O.Phone,
	    FP.FeedbackPromptDate,FP.ClaimID,C.ClaimCode, HF.HFCode, HF.HFName, I.CHFID, I.OtherNames, I.LastName, ICD.ICDName, C.DateFrom, C.DateTo,FP.SMSStatus,C.Claimed"
        With data
            .setSQLCommand(sSQL, CommandType.Text)

            .params("SMSStatus", SMSStatus)
            .params("LocationId", LocationId)
            .params("WardID", WardId)
            .params("VillageId", VillageID)
            .params("OfficerID", OfficerID)
            .params("RangeFrom", RangeFrom)
            .params("RangeTo", RangeTo)

            Return .Filldata
        End With


    End Function
    Public Function GetProcessBatch(ByVal LocationId As Integer, ByVal ProductId As Integer, ByVal RunID As Integer, ByVal HFID As Integer, ByVal HFLevel As String, ByVal DateFrom As Nullable(Of Date), ByVal DateTo As Nullable(Of Date), ByVal MinRemunerated As Decimal) As DataTable
        Dim data As New ExactSQL
        'Dim sSQL As String = "uspSSRSProcessBatch"
        Dim sSQL As String = " IF @LocationId=-1
        BEGIN
        SET @LocationId = NULL
        END

        IF @DateFrom = '' OR @DateFrom IS NULL OR @DateTo = '' OR @DateTo IS NULL
        BEGIN
	        SET @DateFrom = N'1900-01-01'
	        SET @DateTo = N'3000-12-31'
        END

        ;WITH CDetails AS
	        (
		        SELECT CI.ClaimId, CI.ProdId,
		        SUM(ISNULL(CI.PriceApproved, CI.PriceAsked) * ISNULL(CI.QtyApproved, CI.QtyProvided)) PriceApproved,
		        SUM(CI.PriceValuated) PriceAdjusted, SUM(CI.RemuneratedAmount)RemuneratedAmount
		        FROM tblClaimItems CI
		        WHERE CI.ValidityTo IS NULL
		        AND CI.ClaimItemStatus = 1
		        GROUP BY CI.ClaimId, CI.ProdId
		        UNION ALL

		        SELECT CS.ClaimId, CS.ProdId,
		        SUM(ISNULL(CS.PriceApproved, CS.PriceAsked) * ISNULL(CS.QtyApproved, CS.QtyProvided)) PriceApproved,
		        SUM(CS.PriceValuated) PriceValuated, SUM(CS.RemuneratedAmount) RemuneratedAmount

		        FROM tblClaimServices CS
		        WHERE CS.ValidityTo IS NULL
		        AND CS.ClaimServiceStatus = 1
		        GROUP BY CS.CLaimId, CS.ProdId
	        )
	        SELECT R.RegionName, D.DistrictName, HF.HFCode, HF.HFName, Prod.ProductCode, Prod.ProductName, SUM(CDetails.RemuneratedAmount)Remunerated, Prod.AccCodeRemuneration, HF.AccCode
	
	        FROM tblClaim C
	        INNER JOIN tblInsuree I ON I.InsureeId = C.InsureeID
	        INNER JOIN tblHF HF ON HF.HFID = C.HFID
	        INNER JOIN CDetails ON CDetails.ClaimId = C.ClaimID
	        INNER JOIN tblProduct Prod ON Prod.ProdId = CDetails.ProdID
	        INNER JOIN tblFamilies F ON F.FamilyId = I.FamilyID
	        INNER JOIN tblVillages V ON V.VillageID = F.LocationId
	        INNER JOIN tblWards W ON W.WardId = V.WardId
	        INNER JOIN tblDistricts D ON D.DistrictID = W.DistrictId
	        INNER JOIN tblRegions R ON R.RegionId = D.Region

	        WHERE C.ValidityTo IS NULL
	        AND (Prod.LocationId = @LocationId OR @LocationId = 0 OR Prod.LocationId IS NULL)
	        AND(Prod.ProdId = @ProdId OR @ProdId = 0)
	        AND (C.RunId = @RunId OR @RunId = 0)
	        AND (HF.HFId = @HFID OR @HFId = 0)
	        AND (HF.HFLevel = @HFLevel OR @HFLevel = N'')
	        AND (C.DateTo BETWEEN @DateFrom AND @DateTo)

	        GROUP BY  R.RegionName,D.DistrictName, HF.HFCode, HF.HFName, Prod.ProductCode, Prod.ProductName, Prod.AccCodeRemuneration, HF.AccCode

	        HAVING SUM(CDetails.RemuneratedAmount) > @MinRemunerated"

        With data
            .setSQLCommand(sSQL, CommandType.Text)

            .params("@LocationId", SqlDbType.Int, LocationId)
            .params("@ProdId", SqlDbType.Int, ProductId)
            .params("@RunID", SqlDbType.Int, RunID)
            .params("@HFID", SqlDbType.Int, HFID)
            .params("@HFLevel", SqlDbType.Char, 1, HFLevel)
            .params("@DateFrom", SqlDbType.Date, DateFrom)
            .params("@DateTo", SqlDbType.Date, DateTo)
            .params("@MinRemunerated", SqlDbType.Float, MinRemunerated)

            Return .Filldata
        End With

    End Function

    'Corrected
    Public Function GetPrimaryIndicators1(ByVal LocationId As Integer, ByVal ProductId As Integer, ByVal Month As Integer, ByVal Year As Integer, ByVal Mode As Int16, Optional ByVal MonthTo As Integer = 0) As DataTable
        Dim Data As New ExactSQL
        'Dim sSQL As String = "uspSSRSPrimaryIndicators1"
        Dim sSQL As String = "DECLARE @LastDay DATE
	
	    IF @LocationId=-1
	    SET @LocationId=NULL
	    IF NOT OBJECT_ID('tempdb..#tmpResult') IS NULL DROP TABLE #tmpResult
	
	    CREATE TABLE #tmpResult(
		    [Quarter] INT,
		    NameOfTheMonth VARCHAR(15),
		    OfficerCode VARCHAR(8),
		    LastName NVARCHAR(50),
		    OtherNames NVARCHAR(50),
		    ProductCode NVARCHAR(8),
		    ProductName NVARCHAR(100),
		    NoOfPolicyMale INT,
		    NoOfPolicyFemale INT,
		    NoOfPolicyOther INT, -- bY Ruzo
		    NoOfNewPolicyMale INT,
		    NoOfNewPolicyFemale INT,
		    NoOfNewPolicyOther INT, -- bY Ruzo
		    NoOfSuspendedPolicy INT,
		    NoOfExpiredPolicy INT,
		    NoOfRenewPolicy INT,
		    NoOfInsureeMale INT,
		    NoOfInsureeFemale INT,
		    NoOfInsureeOther INT, -- bY Ruzo
		    NoOfNewInsureeMale INT,
		    NoOfNewInsureeFemale INT,
		    NoOfNewInsureeOther INT, -- bY Ruzo
		    PremiumCollected DECIMAL(18,2),
		    PremiumAvailable DECIMAL(18,2),
		    MonthId INT,
		    OfficerStatus CHAR(1)
	    )	
	
	
    DECLARE @Counter INT = 1
    DECLARE @MaxCount INT = 12

    IF @MonthFrom > 0
	    BEGIN
		    SET @Counter = @MonthFrom
		    SET @MaxCount = @MonthTo
	    END
	
    WHILE @Counter <> @MaxCount + 1
    BEGIN
	
	    SET @LastDay = DATEADD(DAY,-1,DATEADD(MONTH,1,CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Counter AS VARCHAR(2)) + '-01'))
	    IF @Mode = 1
		    INSERT INTO #tmpResult
		    SELECT DATEPART(QQ,@LastDay) [Quarter],
		    CAST(YEAR(@LastDay) AS VARCHAR(4)) + ' ' + DATENAME(MONTH,@LastDay)NameOfTheMonth,NULL,NULL,NULL,MainInfo.ProductCode,MainInfo.ProductName,
		    TP.Male AS NoOfPolicyMale,
		    TP.Female AS NoOfPolicyFemale,
		    TP.Other AS NoOfPolicyOther,
		    NP.Male AS NoOfNewPolicyMale,
		    NP.Female AS NoOfNewPolicyFemale,
		    NP.Other AS NoOfNewPolicyOther,
		    SP.SuspendedPolicies NoOfSuspendedPolicy,
		    EP.ExpiredPolicies NoOfExpiredPolicy,
		    PR.Renewals NoOfRenewPolicy,
		    PIn.Male NoOfInsureeMale,Pin.Female NoOfInsureeFemale, PIn.Other NoOfInsureeOther,
		    NPI.Male NoOfNewInsureeMale, NPI.Female NoOfNewInsureeFemale, NPI.Other NoOfNewInsureeOther,
		    NPC.PremiumCollection PremiumCollected,
		    AP.Allocated PremiumAvailable,
		    @Counter MonthId,
		    NULL OfficerStatus

		    FROM 
		    (SELECT PR.ProdID,PR.ProductCode,PR.ProductName
		    FROM tblProduct PR 
		    --INNER JOIN uvwLocations L ON L.LocationId = ISNULL(PR.LocationId, 0) OR L.RegionId = PR.LocationId OR L.DistrictId= PR.LocationId
		    WHERE PR.ValidityTo IS NULL
		    --AND (PR.LocationId = @LocationId OR @LocationId = 0 OR PR.LocationId IS NULL)
		    AND (PR.ProdID = @ProductID OR @ProductID = 0)
		    --AND (L.LocationId = ISNULL(@LocationId, 0) OR ISNULL(@LocationId, 0) = 0)
		    )MainInfo LEFT OUTER JOIN
		    dbo.udfTotalPolicies(@ProductID,@LocationId,@LastDay,@Mode) TP ON MainInfo.ProdID = TP.ProdID LEFT OUTER JOIN
		    dbo.udfNewPolicies(@ProductID,@LocationId,@Counter,@Year,@Mode) NP ON MainInfo.ProdID = NP.ProdID LEFT OUTER JOIN
		    dbo.udfSuspendedPolicies(@ProductID,@LocationId,@Counter,@Year,@Mode)SP ON MainInfo.ProdID = SP.ProdID LEFT OUTER JOIN
		    dbo.udfExpiredPolicies(@ProductID,@LocationId,@Counter,@Year,@Mode)EP ON MainInfo.ProdID = EP.ProdID LEFT OUTER JOIN
		    dbo.udfPolicyRenewal(@ProductID,@LocationId,@Counter,@Year,@Mode) PR ON MainInfo.ProdID = PR.ProdID LEFT OUTER JOIN
		    dbo.udfPolicyInsuree(@ProductID,@LocationId,@lastDay,@Mode)PIn ON MainInfo.ProdID = PIn.ProdID LEFT OUTER JOIN
		    dbo.udfNewPolicyInsuree(@ProductID,@LocationId,@Counter,@Year,@Mode)NPI ON MainInfo.ProdID = NPI.ProdID LEFT OUTER JOIN
		    dbo.udfNewlyPremiumCollected(@ProductID,@LocationId,@Counter,@Year,@Mode)NPC ON MainInfo.ProdID = NPC.ProdID  LEFT OUTER JOIN
		    dbo.udfAvailablePremium(@ProductID,@LocationId,@Counter,@Year,@Mode)AP ON MainInfo.ProdID = AP.ProdID 
	    ELSE
		    INSERT INTO #tmpResult
	
		    SELECT DATEPART(QQ,@LastDay) [Quarter],
		    CAST(YEAR(@LastDay) AS VARCHAR(4)) + ' ' + DATENAME(MONTH,@LastDay)NameOfTheMonth,MainInfo.Code,MainInfo.LastName,MainInfo.OtherNames,MainInfo.ProductCode,MainInfo.ProductName,
		    TP.Male AS NoOfPolicyMale,
		    TP.Female AS NoOfPolicyFemale,
		    TP.Other AS NoOfPolicyOther,
		    NP.Male AS NoOfNewPolicyMale,
		    NP.Female AS NoOfNewPolicyFemale,
		    NP.Other AS NoOfNewPolicyOther,
		    SP.SuspendedPolicies NoOfSuspendedPolicy,
		    EP.ExpiredPolicies NoOfExpiredPolicy,
		    PR.Renewals NoOfRenewPolicy,
		    PIn.Male NoOfInsureeMale,Pin.Female NoOfInsureeFemale, PIn.Other NoOfInsureeOther,
		    NPI.Male NoOfNewInsureeMale, NPI.Female NoOfNewInsureeFemale, NPI.Other NoOfNewInsureeOther,
		    NPC.PremiumCollection PremiumCollected,
		    AP.Allocated PremiumAvailable,
		    @Counter MonthId,
		    IIF(ISNULL(CAST(WorksTo AS DATE) , DATEADD(DAY, 1, GETDATE())) <= CAST(GETDATE() AS DATE), 'N', 'A')OfficerStatus

		    FROM 
		    (SELECT PR.ProdID,PR.ProductCode,PR.ProductName, o.code,O.LastName,O.OtherNames, O.WorksTo
		    FROM tblProduct PR 
		    INNER JOIN tblPolicy PL ON PR.ProdID = PL.ProdID
		    INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
		    INNER JOIN tblVillages V ON V.VillageId = F.LocationId
		    INNER JOIN tblWards W ON W.WardId = V.WardId
		    INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
		    INNER JOIN (select OfficerID,code,LastName,OtherNames,LocationId,ValidityTo, WorksTo from tblOfficer) O on PL.OfficerID = O.OfficerID
		    WHERE pr.ValidityTo is null and o.ValidityTo is null
		    --AND (PR.LocationId = @LocationId OR @LocationId = 0 OR PR.LocationId IS NULL)
		    --AND (D.DistrictID = @LocationId OR @LocationId IS NULL)
		    AND (PR.ProdID = @ProductID OR @ProductID = 0)
		    AND PL.ValidityTo IS NULL --AND F.ValidityTo IS NULL
		    AND V.ValidityTO IS NULL
		    AND W.ValidityTo IS NULL
		    AND D.ValidityTo IS NULL
		    AND (D.Region = @LocationId OR D.DistrictId = @LocationId OR @LocationId = 0)
		    )MainInfo LEFT OUTER JOIN
		    dbo.udfTotalPolicies(@ProductID,@LocationId,@LastDay,@Mode) TP ON MainInfo.ProdID = TP.ProdID and (maininfo.Code = tp.Officer OR maininfo.Code = ISNULL(TP.Officer,0))  LEFT OUTER JOIN
		    dbo.udfNewPolicies(@ProductID,@LocationId,@Counter,@Year,@Mode) NP ON MainInfo.ProdID = NP.ProdID  and (maininfo.Code = np.Officer OR maininfo.Code = ISNULL(NP.Officer,0)) LEFT OUTER JOIN
		    dbo.udfSuspendedPolicies(@ProductID,@LocationId,@Counter,@Year,@Mode)SP ON MainInfo.ProdID = SP.ProdID  and (maininfo.Code = sp.Officer OR maininfo.Code = ISNULL(SP.Officer,0))LEFT OUTER JOIN
		    dbo.udfExpiredPolicies(@ProductID,@LocationId,@Counter,@Year,@Mode)EP ON MainInfo.ProdID = EP.ProdID and (maininfo.Code = ep.Officer OR maininfo.Code = ISNULL(EP.Officer,0)) LEFT OUTER JOIN
		    dbo.udfPolicyRenewal(@ProductID,@LocationId,@Counter,@Year,@Mode) PR ON MainInfo.ProdID = PR.ProdID and (maininfo.Code = pr.Officer OR maininfo.Code = ISNULL(PR.Officer,0)) LEFT OUTER JOIN
		    dbo.udfPolicyInsuree(@ProductID,@LocationId,@lastDay,@Mode)PIn ON MainInfo.ProdID = PIn.ProdID and (maininfo.Code = pin.Officer OR maininfo.Code = ISNULL(PIn.Officer,0)) LEFT OUTER JOIN
		    dbo.udfNewPolicyInsuree(@ProductID,@LocationId,@Counter,@Year,@Mode)NPI ON MainInfo.ProdID = NPI.ProdID and (maininfo.Code = npi.Officer OR maininfo.Code = ISNULL(NPI.Officer,0))LEFT OUTER JOIN
		    dbo.udfNewlyPremiumCollected(@ProductID,@LocationId,@Counter,@Year,@Mode)NPC ON MainInfo.ProdID = NPC.ProdID and (maininfo.Code = npc.Officer OR maininfo.Code = ISNULL(NPC.Officer,0)) LEFT OUTER JOIN
		    dbo.udfAvailablePremium(@ProductID,@LocationId,@Counter,@Year,@Mode)AP ON MainInfo.ProdID = AP.ProdID and (maininfo.Code = ap.Officer OR maininfo.Code = ISNULL(AP.Officer,0))

	    SET @Counter = @Counter + 1

    END

	    SELECT * FROM #tmpResult
	    GROUP BY [Quarter], NameOfTheMonth, OfficerCode, LastName, OtherNames,ProductCode, ProductName, NoOfPolicyMale, NoOfPolicyFemale,NoOfPolicyOther, NoOfNewPolicyMale,
	    NoOfNewPolicyFemale,NoOfNewPolicyOther, NoOfSuspendedPolicy, NoOfExpiredPolicy, NoOfRenewPolicy, NoOfInsureeMale, NoOfInsureeFemale,NoOfInsureeOther, NoOfNewInsureeMale,
	    NoOfNewInsureeFemale,NoOfNewInsureeOther, PremiumCollected, PremiumAvailable, MonthId, OfficerStatus
      ORDER BY MonthId"
        Data.setSQLCommand(sSQL, CommandType.Text, timeout:=0)

        Data.params("@LocationId", LocationId)
        Data.params("@ProductID", ProductId)
        Data.params("@MonthFrom", Month)
        Data.params("@MonthTo", MonthTo)
        Data.params("@Year", Year)
        Data.params("@Mode", Mode)

        Return Data.Filldata

    End Function

    'Corrected
    Public Function GetPrimaryIndicators2(ByVal LocationId As Integer, ByVal ProductId As Integer, ByVal HFID As Integer, ByVal Year As Integer, ByVal MonthFrom As Integer, ByVal MonthTo As Integer) As DataTable
        Dim Data As New ExactSQL
        'Dim sSQL As String = "uspSSRSPrimaryIndicators2"
        Dim sSQL As String = "IF NOT OBJECT_ID('tempdb..#tmpResult') IS NULL DROP TABLE #tmpResult
	
	        CREATE TABLE #tmpResult(
		        NameOfTheMonth VARCHAR(20),
		        DistrictName NVARCHAR(50),
		        HFCode NVARCHAR(8),
		        HFName NVARCHAR(100),
		        ProductCode NVARCHAR(8), 
		        ProductName NVARCHAR(100), 
		        TotalClaims INT,
		        Remunerated DECIMAL(18,2),
		        RejectedClaims INT,
		        MonthNo INT
		
	        )	

        DECLARE @Counter INT = 1
        DECLARE @MaxCount INT = 12

        IF @MonthFrom > 0
	        BEGIN
		        SET @Counter = @MonthFrom
		        SET @MaxCount = @MonthTo
	        END
	
        IF @LocationId = -1
        SET @LocationId = NULL
        WHILE @Counter <> @MaxCount + 1
        BEGIN
		        DECLARE @LastDay DATE = DATEADD(DAY,-1,DATEADD(MONTH,1,CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Counter AS VARCHAR(2)) + '-01'))
			
		        INSERT INTO #tmpResult
		        SELECT CAST(YEAR(@LastDay) AS VARCHAR(4)) + ' ' + DATENAME(MONTH,@LastDay),MainInfo.DistrictName,
		        MainInfo.HFCode,MainInfo.HFName ,MainInfo.ProductCode , MainInfo.ProductName , 
		        TC.TotalClaims TotalClaims,
		        R.Remunerated Remunerated,
		        RC.RejectedClaims RejectedClaims,
		        DATEPART(MM,@LastDay) MonthNo --Added by Rogers On 19092017
	        FROM
	        (SELECT  DistrictName DistrictName,HF.HFID,HF.HFCode,HF.HFName,Prod.ProdID,Prod.ProductCode,Prod.ProductName
	        FROM tblClaim C 
	        INNER JOIN tblInsuree I ON C.InsureeID = I.InsureeID
	        INNER JOIN tblHF HF ON C.HFID = HF.HFID 
	        INNER JOIN tblDistricts D ON D.DistrictId = HF.LocationId
	        LEFT JOIN tblLocations L ON HF.LocationId = L.LocationId
	        LEFT OUTER JOIN 
	        (SELECT ClaimId,ProdId FROM tblClaimItems WHERE ValidityTo IS NULL
	        UNION 
	        SELECT ClaimId, ProdId FROM tblClaimServices WHERE ValidityTo IS NULL
	        )CProd ON CProd.ClaimId = C.ClaimID
	        LEFT OUTER JOIN tblProduct Prod ON Prod.ProdId = CProd.ProdID
	        WHERE C.ValidityTo IS NULL 
	        AND I.ValidityTo IS NULL 
	        AND D.ValidityTo IS NULL 
	        AND HF.ValidityTo IS NULL 
	        AND Prod.ValidityTo IS NULL
	        AND  (HF.LocationId  = @LocationId OR L.ParentLocationId = @LocationId) --Changed From LocationId to HFLocationId	On 29062017
	        AND (Prod.ProdID = @ProductId OR @ProductId = 0)
	        AND (HF.HfID = @HFID OR @HFID = 0)
	        GROUP BY DistrictName,HF.HFID,HF.HFCode,HF.HFName,Prod.ProdID,Prod.ProductCode,Prod.ProductName
	        ) MainInfo 
	        LEFT OUTER JOIN dbo.udfTotalClaims(@ProductID,@HFID,@LocationId,@Counter,@Year) TC ON ISNULL(MainInfo.ProdID, 0) = ISNULL(TC.ProdID, 0) AND MainInfo.HfID = TC.HFID 
	        LEFT OUTER JOIN dbo.udfRemunerated(@HFID,@ProductID,@LocationId,@Counter,@Year) R ON ISNULL(MainInfo.ProdID, 0) = ISNULL(R.ProdID, 0) AND MainInfo.HfID = R.HFID 
	        LEFT OUTER JOIN dbo.udfRejectedClaims(@ProductID,@HFID,@LocationId,@Counter,@Year) RC ON ISNULL(MainInfo.ProdID, 0) = ISNULL(RC.ProdID, 0) AND MainInfo.HfID = RC.HFID

	        SET @Counter = @Counter + 1
	
        END
	
	        SELECT NameOfTheMonth,MonthNo,DistrictName,HFCode ,HFName,ProductCode,ProductName ,ISNULL(TotalClaims,0) TotalClaims ,ISNULL(Remunerated,0) Remunerated ,ISNULL(RejectedClaims,0) RejectedClaims FROM #tmpResult
	        ORDER BY MonthNo"
        Data.setSQLCommand(sSQL, CommandType.Text, 0)

        Data.params("@LocationId", LocationId)
        Data.params("@ProductID", ProductId)
        Data.params("@HFID", HFID)
        Data.params("@MonthFrom", MonthFrom)
        Data.params("@MonthTo", MonthTo)
        Data.params("@Year", Year)

        Return Data.Filldata

    End Function

    'Corrected
    Public Function GetDerivedIndicators1(ByVal LocationId As Integer, ByVal ProductId As Integer, ByVal Month As Integer, ByVal Year As Integer) As DataTable
        Dim Data As New ExactSQL
        'Dim sSQL As String = "uspSSRSDerivedIndicators1"
        Dim sSQL As String = "IF NOT OBJECT_ID('tempdb..#tmpResult') IS NULL DROP TABLE #tmpResult
	
	        CREATE TABLE #tmpResult(
			        NameOfTheMonth VARCHAR(15),
			        DistrictName NVARCHAR(50),
			        ProductCode NVARCHAR(8),
			        ProductName NVARCHAR(100),
			        IncurredClaimRatio DECIMAL(18,2),
			        RenewalRatio DECIMAL(18,2),
			        GrowthRatio DECIMAL(18,2),
			        Promptness DECIMAL(18,2),
			        InsureePerClaim DECIMAL(18,2)
		        )

	        DECLARE @LastDay DATE
	        DECLARE @PreMonth INT
	        DECLARE @PreYear INT 
	
	        DECLARE @Counter INT = 1
	        DECLARE @MaxCount INT = 12

        IF @Month > 0
	        BEGIN
		        SET @Counter = @Month
		        SET @MaxCount = @Month
	        END
	
        WHILE @Counter <> @MaxCount + 1
        BEGIN
	        SET @LastDay = DATEADD(DAY,-1,DATEADD(MONTH,1,CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Counter AS VARCHAR(2)) + '-01'))
	        SET @PreMonth = MONTH(DATEADD(MONTH,-1,@LastDay))
	        SET @PreYear = YEAR(DATEADD(MONTH,-1,@LastDay))

        INSERT INTO #tmpResult
	        SELECT CAST(YEAR(@LastDay) AS VARCHAR(4)) + ' ' + DATENAME(MONTH,@LastDay)NameOfTheMonth,Promptness.DistrictName,MainInfo.ProductCode,MainInfo.ProductName
	        ,CAST(SUM(ISNULL(R.Remunerated,0))AS FLOAT)/ISNULL(AP.Allocated,1) IncurredClaimRatio
	        ,CAST(ISNULL(PR.Renewals,0) AS FLOAT)/ISNULL(EP.ExpiredPolicies,1)RenewalRatio
	        ,CAST((ISNULL(NP.Male,0) + ISNULL(NP.Female,0)) AS FLOAT)/ISNULL(TP.Male + TP.Female,1)GrowthRatio
	        ,Promptness.AverageDays AS Promptness --Still to come
	        ,SUM(TC.TotalClaims)/ISNULL(PIn.Male + PIn.Female,1)InsureePerClaim
	        FROM
	        (SELECT PR.ProdID,PR.ProductCode,PR.ProductName
	        FROM tblProduct PR 
	        WHERE PR.ValidityTo IS NULL	
	        AND (PR.ProdID = @ProductID OR @ProductID = 0)
	        )MainInfo INNER JOIN
	        dbo.udfRemunerated(0,@ProductID,@LocationId,@Counter,@Year) R ON MainInfo.ProdID = R.ProdID LEFT OUTER JOIN
	        dbo.udfAvailablePremium(@ProductID,@LocationId,@Counter,@Year,1)AP ON MainInfo.ProdID = AP.ProdID LEFT OUTER JOIN
	        dbo.udfPolicyRenewal(@ProductID,@LocationId,@Counter,@Year,1) PR ON MainInfo.ProdID = PR.ProdID LEFT OUTER JOIN
	        dbo.udfExpiredPolicies(@ProductID,@LocationId,@Counter,@Year,1)EP ON MainInfo.ProdID = EP.ProdID LEFT OUTER JOIN
	        dbo.udfNewPolicies(@ProductID,@LocationId,@PreMonth,@PreYear,1)NP ON MainInfo.ProdID = NP.ProdID LEFT OUTER JOIN
	        dbo.udfTotalPolicies(@ProductID,@LocationId,DATEADD(MONTH,-1,@LastDay),1)TP ON MainInfo.ProdID = TP.ProdID LEFT OUTER JOIN
	        --dbo.udfRejectedClaims(@ProductID,@LocationId,0,@Counter,@Year)RC ON MainInfo.ProdID = RC.ProdID LEFT OUTER JOIN
	        dbo.udfTotalClaims(@ProductId,0,@LocationId,@Counter,@Year) TC ON MainInfo.ProdID = TC.ProdID LEFT OUTER JOIN
	        dbo.udfPolicyInsuree(@ProductID,@LocationId,@LastDay,1) PIn ON MainInfo.ProdID = PIn.ProdID LEFT OUTER JOIN
	        (SELECT Base.ProdID,AVG(DATEDIFF(dd,Base.DateClaimed,Base.RunDate))AverageDays,Base.DistrictName
		        FROM
		        (SELECT C.ClaimID,C.DateClaimed,CI.ProdID,B.RunDate,D.DistrictName
		        FROM tblClaim C INNER JOIN tblClaimItems CI ON C.ClaimID = CI.ClaimID
		        INNER JOIN tblInsuree I ON C.InsureeId = I.InsureeId 
		        INNER JOIN tblFamilies F ON I.familyId = F.FamilyId
		        INNER JOIN tblVillages V ON V.VillageId = F.LocationId
		        INNER JOIN tblWards W ON W.WardId = V.WardId
		        INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
		        INNER JOIN tblBatchRun B ON C.RunID = B.RunID
		        WHERE C.ValidityTo IS NULL AND CI.ValidityTo IS NULL AND I.ValidityTo IS NULL AND F.ValidityTo IS NULL
		        AND (CI.ProdID = @ProductID OR @ProductID = 0)
		        AND (D.DistrictId = @LocationId OR @LocationId = 0)
		        AND C.RunID IN (SELECT  RunID FROM tblBatchRun WHERE ValidityTo IS NULL AND MONTH(RunDate) =@Counter AND YEAR(RunDate) = @Year)
		        GROUP BY C.ClaimID,C.DateClaimed,CI.ProdID,B.RunDate,D.DistrictName
		        UNION 
		        SELECT C.ClaimID,C.DateClaimed,CS.ProdID,B.RunDate, D.DistrictName
		        FROM tblClaim C INNER JOIN tblClaimItems CS ON C.ClaimID = CS.ClaimID
		        INNER JOIN tblInsuree I ON C.InsureeId = I.InsureeId 
		        INNER JOIN tblFamilies F ON I.familyId = F.FamilyId
		        INNER JOIN tblVillages V ON V.VillageId = F.LocationId
		        INNER JOIN tblWards W ON W.WardId = V.WardId
		        INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
		        INNER JOIN tblBatchRun B ON C.RunID = B.RunID
		        WHERE C.ValidityTo IS NULL AND CS.ValidityTo IS NULL AND I.ValidityTo IS NULL AND F.ValidityTo IS NULL
		        AND (CS.ProdID = @ProductID OR @ProductID = 0)
		        AND (D.DistrictId = @LocationId OR @LocationId = 0)
		        AND C.RunID IN (SELECT  RunDate FROM tblBatchRun WHERE ValidityTo IS NULL AND MONTH(RunDate) =@Counter AND YEAR(RunDate) = @Year)
		        GROUP BY C.ClaimID,C.DateClaimed,CS.ProdID,B.RunDate, D.DistrictName)Base
		        GROUP BY Base.ProdID,Base.DistrictName)Promptness ON MainInfo.ProdID = Promptness.ProdID
	
	        GROUP BY Promptness.DistrictName,MainInfo.ProductCode,MainInfo.ProductName,AP.Allocated,PR.Renewals,EP.ExpiredPolicies,NP.Male,NP.Female,TP.Male,TP.Female,Promptness.AverageDays,PIn.Male,Pin.Female
	
	        SET @Counter = @Counter + 1
		
        END
	        SELECT * FROM #tmpResult"
        Data.setSQLCommand(sSQL, CommandType.Text)

        Data.params("@LocationId", LocationId)
        Data.params("@ProductID", ProductId)
        Data.params("@Month", Month)
        Data.params("@Year", Year)

        Return Data.Filldata

    End Function

    'Corrected
    Public Function GetDerivedIndicators2(ByVal LocationId As Integer, ByVal ProductId As Integer, ByVal HFID As Integer, ByVal Month As Integer, ByVal Year As Integer) As DataTable
        Dim Data As New ExactSQL
        'Dim sSQL As String = "uspSSRSDerivedIndicators2"
        Dim sSQL As String = "DECLARE @LastDay DATE
	
	        IF NOT OBJECT_ID('tempdb..#tmpResult') IS NULL DROP TABLE #tmpResult
	
	        CREATE TABLE #tmpResult(
		        NameOfTheMonth VARCHAR(15),
		        DistrictName NVARCHAR(50),
		        HFCode NVARCHAR(8),
		        HFName NVARCHAR(100) ,
		        ProductCode NVARCHAR(8), 
		        ProductName NVARCHAR(100),
		        SettlementRatio DECIMAL(18,2),
		        AverageCostPerClaim DECIMAL(18,2),
		        Asessment DECIMAL(18,2),
		        FeedbackResponseRatio DECIMAL(18,2)
		
	        )

        DECLARE @Counter INT = 1
        DECLARE @MaxCount INT = 12

        IF @Month > 0
	        BEGIN
		        SET @Counter = @Month
		        SET @MaxCount = @Month
	        END
	
        WHILE @Counter <> @MaxCount + 1
        BEGIN

	        SET @LastDay = DATEADD(DAY,-1,DATEADD(MONTH,1,CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Counter AS VARCHAR(2)) + '-01'))
	
	        INSERT INTO #tmpResult
	        SELECT CAST(YEAR(@LastDay) AS VARCHAR(4)) + ' ' + DATENAME(MONTH,@LastDay)NameOfTheMonth,MainInfo.DistrictName,MainInfo.HFCode,MainInfo.HFName ,MainInfo.ProductCode , MainInfo.ProductName
	        ,(TC.TotalClaims - ISNULL(RC.RejectedClaims,0))/TC.TotalClaims SettlementRatio
	        --,CAST(SUM(ISNULL(R.Remunerated,0))/CAST(ISNULL(NULLIF(COUNT(TC.TotalClaims),0),1) AS NUMERIC) AS FLOAT)AverageCostPerClaim
	        ,CAST(SUM(ISNULL(R.Remunerated,0))/TC.TotalClaims AS FLOAT)AverageCostPerClaim
	        ,Satisfaction.Asessment
	        ,FeedbackResponse.FeedbackResponseRatio
	        FROM

	        (SELECT tblDistricts.DistrictName,tblHF.HfID  ,tblHF.HFCode ,tblHF.HFName ,tblProduct.ProdID , tblProduct.ProductCode ,tblProduct.ProductName FROM tblDistricts INNER JOIN tblHF ON tblDistricts.DistrictID = tblHF.LocationId 
	        INNER JOIN tblProduct ON tblProduct.LocationId = tblDistricts.DistrictID 
	        WHERE tblDistricts.ValidityTo IS NULL AND tblHF.ValidityTo IS NULL AND tblproduct.ValidityTo IS NULL 
				        AND (tblDistricts.DistrictID = @LocationId OR @LocationId = 0) 
				        AND (tblProduct.ProdID = @ProductID OR @ProductID = 0)
				        AND (tblHF.HFID = @HFID OR @HFID = 0)
	        ) MainInfo LEFT OUTER JOIN
	        dbo.udfRejectedClaims(@ProductID,@LocationId,0,@Counter,@Year)RC ON MainInfo.ProdID = RC.ProdID AND MainInfo.HfID = RC.HFID LEFT OUTER JOIN
	        dbo.udfTotalClaims(@ProductID,@HFID,@LocationId,@Counter,@Year) TC ON MainInfo.ProdID = TC.ProdID AND MainInfo.hfid = TC.HFID LEFT OUTER JOIN
	        dbo.udfRemunerated(@HFID,@ProductID,@LocationId,@Counter,@Year) R ON MainInfo.ProdID = R.ProdID AND MainInfo.HfID = R.HFID LEFT OUTER JOIN
	        (SELECT C.LocationId,C.HFID,C.ProdID,AVG(CAST(F.Asessment AS DECIMAL(3, 1)))Asessment 
	        FROM tblFeedback F INNER JOIN
	        (SELECT CI.ClaimID,CI.ProdID,C.HFID,PR.LocationId
	        FROM tblClaim C INNER JOIN tblClaimItems CI ON C.ClaimID = CI.ClaimID
	        INNER JOIN tblProduct PR ON CI.ProdID = PR.ProdID
	        WHERE C.ValidityTo IS NULL AND CI.ValidityTo IS NULL AND PR.ValidityTo IS NULL
	        GROUP BY CI.ClaimID,CI.ProdID,C.HFID,PR.LocationId
	        UNION 
	        SELECT CS.ClaimID,CS.ProdID,C.HFID,PR.LocationId
	        FROM tblClaim C INNER JOIN tblClaimServices CS ON C.ClaimID = CS.ClaimID
	        INNER JOIN tblProduct PR ON CS.ProdID = PR.ProdID
	        WHERE C.ValidityTo IS NULL AND CS.ValidityTo IS NULL AND PR.ValidityTo IS NULL
	        GROUP BY CS.ClaimID,CS.ProdID,C.HFID,PR.LocationId
	        )C ON F.ClaimID = C.ClaimID
	        WHERE MONTH(F.FeedbackDate) = @Counter AND YEAR(F.FeedbackDate) = @Year
	        GROUP BY C.LocationId,C.HFID,C.ProdID)Satisfaction ON MainInfo.ProdID = Satisfaction.ProdID AND MainInfo.HfID = Satisfaction.HFID
	        LEFT OUTER JOIN
	        (SELECT PR.LocationId, C.HFID, PR.ProdId, COUNT(F.FeedbackID) / COUNT(C.ClaimID) FeedbackResponseRatio
	        FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
	        LEFT OUTER JOIN tblClaimServices CS ON CS.ClaimID = C.ClaimID
	        LEFT OUTER JOIN tblFeedback F ON C.ClaimId = F.ClaimID
	        LEFT OUTER JOIN tblFeedbackPrompt FP ON FP.ClaimID =C.ClaimID
	        INNER JOIN tblProduct PR ON PR.ProdId = CI.ProdID OR PR.ProdID = CS.ProdID
	        WHERE C.ValidityTo IS NULL
	        AND C.FeedbackStatus >= 4
	        AND F.ValidityTo IS NULL
	        AND MONTH(FP.FeedbackPromptDate) = @Counter
	        AND YEAR(FP.FeedbackPromptDate) = @Year
	        GROUP BY PR.LocationId, C.HFID, PR.ProdId)FeedbackResponse ON MainInfo.ProdID = FeedbackResponse.ProdID AND MainInfo.HfID = FeedbackResponse.HFID
	
	        GROUP BY MainInfo.DistrictName,MainInfo.HFCode,MainInfo.HFName,MainInfo.ProductCode,MainInfo.ProductName,RC.RejectedClaims,Satisfaction.Asessment,FeedbackResponse.FeedbackResponseRatio, TC.TotalClaims
	        SET @Counter = @Counter + 1

        END

	        SELECT * FROM #tmpResult"
        Data.setSQLCommand(sSQL, CommandType.Text)

        Data.params("@LocationId", LocationId)
        Data.params("@ProductID", ProductId)
        Data.params("@HFID", HFID)
        Data.params("@Month", Month)
        Data.params("@Year", Year)

        Return Data.Filldata

    End Function

    'Corrected
    Public Function GetUserActivityData(ByVal UserID As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal Action As String, ByVal Entity As String) As DataTable
        'Dim sSQL As String = "uspSSRSUserLogReport"
        Dim data As New ExactSQL
        Dim sSQL As String = " SET @UserId = NULLIF(@UserId, 0);

	SET @ToDate = DATEADD(SECOND,-1,DATEADD(DAY,1,@ToDate))

	DECLARE @tblLogs TABLE(UserId INT,UserName NVARCHAR(20),EntityId NVARCHAR(5),RecordType NVARCHAR(50),ActionType NVARCHAR(50),RecordIdentity NVARCHAR(500),ValidityFrom DATETIME,ValidityTo DATETIME, LegacyId INT, VF DATETIME,HistoryLegacyId INT)
	--DECLARE @UserId INT = 149
	
	--Line below is commented because UserId is made optional now
	DECLARE @UserName NVARCHAR(50) --= (SELECT LoginName FROM tblUsers WHERE (UserID = @UserId OR @Userid IS NULL))
	
	--DECLARE @FromDate DATETIME = '2013-04-29'
	--DECLARE @ToDate DATETIME = '2013-10-29'

	SET @ToDate = DATEADD(S,-1,DATEADD(D,1,@ToDate))

	INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
	--LOGIN INFORMATION
	SELECT L.UserId UserId,NULL UserName,CASE LogAction WHEN 1 THEN N'LI' ELSE N'LO' END,'Login' RecordType ,CASE LogAction WHEN 1 THEN N'Logged In' ELSE N'Logged Out' END ActionType,CAST(LogAction as NVARCHAR(10)) RecordIdentity,LogTime,NULL,NULL,NULL VF,NULL HistoryLegacyId
	FROM tblLogins L
	WHERE (L.UserId = @UserId OR @UserId IS NULL)
	AND LogTime BETWEEN @FromDate AND @ToDate

	--BATCH RUN INFORMATION
	--UNION ALL
	IF @EntityId = N'BR' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT B.AuditUserID UserId, NULL UserName, N'BR' EntityId,'Batch Run' RecordType,'Executed Batch' ActionType,
		'Batch Run For the District:' + D.DistrictName + ' For the month of ' + DATENAME(MONTH,'2000-' + CAST(B.RunMonth AS NVARCHAR(2)) + '-01') RecordIdentity,B.ValidityFrom,B.ValidityTo,B.LegacyID, NULL VF,NULL HistoryLegacyId
		FROM tblBatchRun B INNER JOIN tblDistricts D ON B.LocationId = D.DistrictID
		WHERE (B.AuditUserID = @UserId OR @UserId IS NULL)
		AND B.ValidityFrom BETWEEN @FromDate AND @ToDate

	--CLAIM INFORMATION
	--UNION ALL

	IF @EntityId = N'C' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT C.AuditUserID UserId, NULL UserName,N'C' EntityId, 'Claim' RecordType,
		NULL,'Claim Code: '+ ClaimCode + ' For Health Facility:' + HF.HFCode RecordIdentity,
		C.ValidityFrom,C.ValidityTo,C.LegacyID,VF,Hist.LegacyID
		FROM tblClaim C INNER JOIN tblHF HF ON C.HFID = HF.HfID
		LEFT OUTER JOIN
		(SELECT MIN(ValidityFrom) VF FROM tblClaim WHERE LegacyID IS NOT NULL GROUP BY LegacyID) Ins ON Ins.VF = C.ValidityFrom
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblClaim WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON C.ClaimID = Hist.LegacyID
		WHERE (C.AuditUserID = @UserId OR @UserId IS NULL)
		AND C.ValidityFrom BETWEEN @FromDate AND @ToDate

	--CLAIM ADMINISTRATOR INFORMATION
	--UNION ALL
	IF @EntityId = N'CA' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT A.AuditUserID UserId, NULL UserName, N'CA' EntityId,'Claim Administrator' RecordType,NULL ActionType,
		'Name:' + A.OtherNames + ' ' + A.LastName + ' in the Health Facility:' + HF.HFName RecordIdentity, 
		A.ValidityFrom, A.ValidityTo,A.LegacyID,VF,Hist.LegacyId
		FROM tblClaimAdmin A INNER JOIN tblHF HF ON A.HFID = HF.HFID
		LEFT OUTER JOIN
		(SELECT MIN(ValidityFrom) VF FROM tblClaimAdmin WHERE LegacyId IS NOT NULL GROUP BY LegacyId) Ins ON Ins.VF = A.ValidityFrom
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblClaimAdmin WHERE LegacyId IS NOT NULL GROUP BY LegacyId) Hist ON A.ClaimAdminId = Hist.LegacyId
		WHERE (A.AuditUserID = @UserId AND @UserId IS NULL)
		AND A.ValidityFrom BETWEEN @FromDate AND @ToDate

	--DISTRICT INFORMATION
	--UNION ALL
	IF @EntityId = N'D' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT D.AuditUserID UserId, NULL UserName, N'D' EntityId,'District' RecordType,NULL ActionType,
		DistrictName RecordIdentity, D.ValidityFrom, D.ValidityTo,D.LegacyID, VF,Hist.LegacyID
		FROM tblDistricts D 
		LEFT OUTER JOIN
		(SELECT MIN(ValidityFrom) VF FROM tblDistricts WHERE LegacyID IS NOT NULL GROUP BY LegacyID) Ins ON D.ValidityFrom = Ins.VF
		LEFT OUTER JOIN
		(SELECT LegacyID FROM tblDistricts WHERE LegacyID IS NOT NULL GROUP BY LegacyID) Hist ON D.DistrictID = Hist.LegacyID
		WHERE (D.AuditUserID = @UserId OR @UserId IS  NULL)
		AND D.ValidityFrom BETWEEN @FromDate AND @ToDate

	--EXTRACT INFORMATION
	--UNION ALL
	IF @EntityId  = N'E' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT E.AuditUserID UserId, NULL UserName, N'E' EntityId,'Extracts' RecordType,NULL ActionType,
		'For the District:' + D.DistrictName + ' File:' + E.ExtractFileName RecordIdentity, E.ValidityFrom, E.ValidityTo,E.LegacyID,VF,Hist.LegacyID
		FROM tblExtracts E INNER JOIN tblDistricts D ON E.LocationId = D.DistrictID
		LEFT OUTER JOIN
		(SELECT MIN(ValidityFrom) VF FROM tblExtracts WHERE LegacyID IS NOT NULL GROUP BY LegacyID) Ins ON E.ValidityFrom = Ins.VF
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblExtracts WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON E.ExtractID = Hist.LegacyID
		WHERE (E.AuditUserID = @UserId OR @UserId IS NULL)
		AND E.ValidityFrom BETWEEN @FromDate AND @ToDate

	--FAMILY INFORMATION
	--UNION ALL
	IF @EntityId = N'F' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT F.AuditUserID UserId, NULL UserName, N'F' EntityId,'Family/Group' RecordType,NULL ActionType,
		'Insurance No.:' + I.CHFID + ' In District:' + D.DistrictName  RecordIdentity, 
		F.ValidityFrom, F.ValidityTo,F.LegacyID,VF,Hist.LegacyID
		FROM tblFamilies F INNER JOIN tblDistricts D ON F.LocationId = D.DistrictID
		INNER JOIN tblInsuree I ON F.InsureeID = I.InsureeID
		LEFT OUTER JOIN(
		SELECT MIN(ValidityFrom) VF from tblFamilies WHERE LegacyID is not null group by LegacyID) Ins ON F.ValidityFrom = Ins.VF
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblFamilies WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON F.FamilyID = Hist.LegacyID
		WHERE (F.AuditUserID = @UserId OR @UserId IS NULL)
		AND f.ValidityFrom BETWEEN @FromDate AND @ToDate

	--FEEDBACK INFORMATION
	--UNION ALL
	IF @EntityId = N'FB' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT F.AuditUserID UserId, NULL UserName, N'FB' EntityId,'Feedback' RecordType,NULL ActionType,
		'Feedback For the claim:' + C.ClaimCode  RecordIdentity, 
		F.ValidityFrom, F.ValidityTo,F.LegacyID,VF,Hist.LegacyID
		FROM tblFeedback F INNER JOIN tblClaim C ON F.ClaimID = C.ClaimID
		LEFT OUTER JOIN(
		  SELECT MIN(ValidityFrom) VF FROM tblFeedback WHERE LegacyID is not null group by LegacyID) Ins On F.ValidityFrom = Ins.VF
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblFeedback WHERE LegacyID IS NOT NULL GROUP BY LegacyID) Hist ON F.FeedbackID = Hist.LegacyID
		WHERE (F.AuditUserID = @UserId OR @UserId IS NULL)
		AND F.ValidityFrom BETWEEN @FromDate AND @ToDate

	--HEALTH FACILITY INFORMATION
	--UNION ALL
	IF @EntityId = N'HF' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT HF.AuditUserID UserId, NULL UserName, N'HF' EntityId,'Health Facility' RecordType,NULL ActionType,
		'Code:' + HF.HFCode + ' Name:' + HF.HFName RecordIdentity, 
		HF.ValidityFrom, HF.ValidityTo,HF.LegacyID,VF,Hist.LegacyId
		FROM tblHF HF 
		LEFT OUTER JOIN(
		SELECT MIN(ValidityFrom) VF FROM tblHF WHERE LegacyID is not null group by LegacyID) Ins ON HF.ValidityFrom = Ins.VF
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblHF WHERE LegacyID IS NOT NULL GROUP BY LegacyID) Hist ON HF.HfID = Hist.LegacyID
		WHERE (HF.AuditUserID = @UserId OR @UserId IS NULL)
		AND HF.ValidityFrom BETWEEN @FromDate AND @ToDate

	--ICD CODE INFORMATION
	--UNION ALL
	IF @EntityId = N'ICD' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT ICD.AuditUserID UserId, NULL UserName, N'ICD' EntityId,'Main Dg.' RecordType,NULL ActionType,
		'Code:' + ICD.ICDCode + ' Name:' + ICD.ICDName RecordIdentity,
		ICD.ValidityFrom, ICD.ValidityTo,ICD.LegacyID,VF, Hist.LegacyId
		FROM tblICDCodes ICD 
		LEFT OUTER JOIN(
		SELECT MIN(ValidityFrom) VF FROM tblICDCodes WHERE LegacyID IS NOT NULL GROUP BY LegacyID) Ins ON ICD.ValidityFrom = Ins.VF
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblICDCodes WHERE LegacyID IS NOT NULL GROUP BY LegacyId)Hist ON ICD.ICDID = Hist.LegacyId
		WHERE (ICD.AuditUserID = @UserId OR @UserId IS NULL)
		AND ICD.ValidityFrom BETWEEN @FromDate AND @ToDate

	--INSUREE INFORMATION
	--UNION ALL
	IF @EntityId = N'Ins' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT I.AuditUserID UserId, @UserName UserName, N'Ins' EntityId,'Insuree' RecordType,NULL ActionType,
		'Insurance No.:' + I.CHFID RecordIdentity, 
		I.ValidityFrom, I.ValidityTo,I.LegacyID,vf,Hist.LegacyID
		FROM tblInsuree I
		LEFT OUTER JOIN(
		SELECT MIN(validityfrom) vf from tblInsuree where LegacyID is not null group by LegacyID) Ins ON I.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblInsuree WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON I.InsureeID = Hist.LegacyID
		WHERE (I.AuditUserID = @UserId OR @UserId IS NULL)
		AND I.ValidityFrom BETWEEN @FromDate AND @ToDate

	--MEDICAL ITEM INFORMATION
	--UNION ALL
	IF @EntityId = N'I' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT I.AuditUserID UserId, @UserName UserName, N'I' EntityId,'Medical Items' RecordType,NULL ActionType,
		'Code:' + I.ItemCode + ' Name:' + I.ItemName RecordIdentity, 
		I.ValidityFrom, I.ValidityTo,I.LegacyID,vf,Hist.LegacyID
		FROM tblItems I
		LEFT OUTER JOIN(
		SELECT MIN(ValidityFrom) vf from tblItems WHERE LegacyID IS NOT NULL GROUP BY LegacyID) Ins on I.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblItems WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON I.ItemID = Hist.LegacyID
		WHERE (I.AuditUserID = @UserId OR @UserId IS NULL)
		AND I.ValidityFrom BETWEEN @FromDate AND @ToDate

	--OFFICER INFORMATION
	--UNION ALL
	IF @EntityId = N'O' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT O.AuditUserID UserId, @UserName UserName, N'O' EntityId,'Enrolment Officer' RecordType,NULL ActionType,
		'Code:' + O.Code + ' Name:' + O.OtherNames RecordIdentity, 
		O.ValidityFrom, O.ValidityTo,O.LegacyID,vf,Hist.LegacyID
		FROM tblOfficer O
		left outer join(
		select MIN(ValidityFrom) vf from tblOfficer where LegacyID is not null group by LegacyID) Ins ON O.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblOfficer WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON O.OfficerID = Hist.LegacyID
		WHERE (O.AuditUserID = @UserId OR @UserId IS NULL)
		AND O.ValidityFrom BETWEEN @FromDate AND @ToDate

	--PAYER INFORMATION
	--UNION ALL
	IF @EntityId = N'P' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT P.AuditUserID UserId, @UserName UserName, N'P' EntityId,'Payer' RecordType,NULL ActionType,
		'Name:' + P.PayerName RecordIdentity, 
		P.ValidityFrom, P.ValidityTo,P.LegacyID,VF,Hist.LegacyID
		FROM tblPayer P
		left outer join(
		select MIN(ValidityFrom) VF from tblPayer where LegacyID is not null group by LegacyID) Ins ON P.ValidityFrom = Ins.VF
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblPayer WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON P.PayerID = Hist.LegacyID
		WHERE (P.AuditUserID = @UserId OR @UserId IS NULL)
		AND P.ValidityFrom BETWEEN @FromDate AND @ToDate

	--PHOTO INFORMATION
	--UNION ALL
	IF @EntityId = N'Ph' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT P.AuditUserID UserId, @UserName UserName, N'Ph' EntityId,'Photo' RecordType,NULL ActionType,
		'Assign to Insurance No.:' + I.CHFID RecordIdentity, 
		P.ValidityFrom, P.ValidityTo,NULL LegacyID,NULL VF,NULL HistoryLegacyId
		FROM tblPhotos P INNER JOIN tblInsuree I ON P.InsureeID = I.InsureeID
		WHERE (P.AuditUserID = @UserId OR @UserId IS NULL)
		AND ISNULL(P.PhotoFileName,'') <> ''
		AND P.ValidityFrom BETWEEN @FromDate AND @ToDate

	--PRICE LIST ITEM INFORMATION
	--UNION ALL
	IF @EntityId = N'PLI' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT I.AuditUserID UserId, @UserName UserName, N'PLI' EntityId,'Price List Items' RecordType,NULL ActionType,
		'Name:' + I.PLItemName + ' In the District:' + D.DistrictName RecordIdentity, 
		I.ValidityFrom, I.ValidityTo,I.LegacyID,VF,Hist.LegacyID
		FROM tblPLItems I INNER JOIN tblDistricts D ON I.LocationId = D.DistrictID
		left outer join(
		select MIN(validityFrom) VF From tblPLItems where LegacyID is not null group by LegacyID) Ins On I.ValidityFrom = Ins.VF
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblPLItems WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON I.PLItemID = Hist.LegacyID
		WHERE (I.AuditUserID = @UserId OR @UserId IS NULL)
		AND I.ValidityFrom BETWEEN @FromDate AND @ToDate

	--PRICE LIST ITEM DETAILS INFORMATION
	--UNION ALL
	IF @EntityId = N'PLID' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT I.AuditUserID UserId, @UserName UserName, N'PLID' EntityId,'Price List Items Details' RecordType,NULL ActionType,
		'Item:' + I.ItemName + ' In the Price List:' + PL.PLItemName RecordIdentity, 
		D.ValidityFrom, D.ValidityTo,D.LegacyID,vf,Hist.LegacyID
		FROM tblPLItemsDetail D INNER JOIN tblPLItems PL ON D.PLItemID = PL.PLItemID
		INNER JOIN tblItems I ON D.ItemID = I.ItemID
		left outer join(
		select MIN(validityfrom) vf from tblPLItemsDetail where LegacyID is not null group by LegacyID) Ins On D.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblPLItemsDetail WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON D.PLItemDetailID = Hist.LegacyID
		WHERE (I.AuditUserID = @UserId OR @UserId IS NULL)
		AND D.ValidityFrom BETWEEN @FromDate AND @ToDate

	--PRICE LIST SERVICE INFORMATION
	--UNION ALL
	IF @EntityId = N'PLS' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT S.AuditUserID UserId, @UserName UserName, N'PLS' EntityId,'Price List Service' RecordType,NULL ActionType,
		'Name:' + S.PLServName + ' In the District:' + D.DistrictName RecordIdentity, 
		S.ValidityFrom, S.ValidityTo,S.LegacyID,vf,Hist.LegacyID
		FROM tblPLServices S INNER JOIN tblDistricts D ON S.LocationId = D.DistrictID
		left outer join(
		select MIN(validityfrom) vf from tblPLServices where LegacyID is not null group by LegacyID) Ins On S.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblPLServices WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON S.PLServiceID = Hist.LegacyID
		WHERE (S.AuditUserID = @UserId OR @UserId IS NULL)
		AND S.ValidityFrom BETWEEN @FromDate AND @ToDate

	--PRICE LIST SERVICE DETAILS INFORMATION
	--UNION ALL
	IF @EntityId = N'PLSD' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT D.AuditUserID UserId, @UserName UserName, N'PLSD' EntityId,'Price List Service Details' RecordType,NULL ActionType,
		'Service:' + S.ServName + ' In the Price List:' + PL.PLServName RecordIdentity, 
		D.ValidityFrom, D.ValidityTo,D.LegacyID,vf,Hist.LegacyID
		FROM tblPLServicesDetail D INNER JOIN tblPLServices PL ON D.PLServiceID = PL.PLServiceID
		INNER JOIN tblServices S ON D.ServiceID = S.ServiceID
		left outer join(
		select MIN(validityfrom) vf from tblPLServicesDetail where LegacyID is not null group by LegacyID) Ins ON D.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblPLServicesDetail WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON D.PLServiceID = Hist.LegacyID
		WHERE (D.AuditUserID = @UserId OR @UserId IS NULL)
		AND D.ValidityFrom BETWEEN @FromDate AND @ToDate

	--POLICY INFORMATION
	--UNION ALL
	IF @EntityId =N'PL' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT P.AuditUserID UserId, @UserName UserName, N'PL' EntityId,'Policy' RecordType,NULL ActionType,
		'To the Family/Group Head:' + I.CHFID RecordIdentity, 
		P.ValidityFrom, P.ValidityTo,P.LegacyID,vf,Hist.LegacyID
		FROM tblPolicy P INNER JOIN tblFamilies F ON P.FamilyID = F.FamilyID
		INNER JOIN tblInsuree I ON F.InsureeID = I.InsureeID
		left outer join(
		select MIN(validityfrom) vf from tblPolicy where LegacyID is not null group by LegacyID) Ins on P.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblPolicy WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON P.PolicyID = Hist.LegacyID
		WHERE (P.AuditUserID = @UserId OR @UserId IS NULL)
		AND P.ValidityFrom BETWEEN @FromDate AND @ToDate

	--PREMIUM INFORMATION
	--UNION ALL
	IF @EntityId = N'PR' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT PR.AuditUserID UserId, @UserName UserName, N'PR' EntityId,'Contribution' RecordType,NULL ActionType,
		CAST(PR.Amount AS NVARCHAR(20)) + ' Paid for the policy started on ' + CONVERT(NVARCHAR(10),P.StartDate,103) + ' For the Family/Group Head:' + I.CHFID RecordIdentity, 
		PR.ValidityFrom, PR.ValidityTo,PR.LegacyID,vf,Hist.LegacyID
		FROM tblPremium PR INNER JOIN tblPolicy P ON PR.PolicyID = P.PolicyID
		INNER JOIN tblFamilies F ON P.FamilyID = F.FamilyID
		INNER JOIN tblInsuree I ON F.InsureeID = I.InsureeID
		left outer join(
		select MIN(validityfrom) vf from tblPremium where LegacyID is not null group by LegacyID) Ins on PR.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyID FROM tblPremium WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON PR.PremiumId = Hist.LegacyID
		WHERE (PR.AuditUserID = @UserId OR @UserId IS NULL)
		AND PR.ValidityFrom BETWEEN @FromDate AND @ToDate

	--PRODUCT INFORMATION
	--UNION ALL
	IF @EntityId = N'PRD' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT PR.AuditUserID UserId, @UserName UserName, N'PRD' EntityId,'Product' RecordType,NULL ActionType,
		'Code:' + PR.ProductCode + ' Name:' + PR.ProductName RecordIdentity, 
		PR.ValidityFrom, PR.ValidityTo,PR.LegacyID,vf,Hist.LegacyID
		FROM tblProduct PR
		left outer join(
		select MIN(validityfrom) vf from tblProduct where LegacyID is not null group by LegacyID) Ins ON PR.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT legacyId FROM tblProduct WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON PR.ProdId = Hist.LegacyID
		WHERE (PR.AuditUserID = @UserId OR @UserId IS NULL)
		AND PR.ValidityFrom BETWEEN @FromDate AND @ToDate

	--PRODUCT ITEM INFORMATION
	--UNION ALL
	IF @EntityId = N'PRDI' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT ProdI.AuditUserID UserId, @UserName UserName, N'PRDI' EntityId,'Product Item' RecordType,NULL ActionType,
		'Item:' + I.ItemCode + ' in the product: ' + P.ProductCode RecordIdentity, 
		ProdI.ValidityFrom, ProdI.ValidityTo,ProdI.LegacyID,vf,Hist.LegacyID
		FROM tblProductItems ProdI INNER JOIN tblItems I ON ProdI.ItemID = I.ItemID
		INNER JOIN tblProduct P ON ProdI.ProdID = P.ProdID
		left outer join(
		select MIN(validityfrom) vf from tblProductItems where LegacyID is not null group by LegacyID) Ins ON ProdI.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblProductItems WHERE LegacyID IS NOT NULL GROUP BY LegacyID) Hist ON Prodi.ProdItemID = Hist.LegacyID
		WHERE (ProdI.AuditUserID = @UserId OR @UserId IS NULL)
		AND ProdI.ValidityFrom BETWEEN @FromDate AND @ToDate

	--PRODUCT SERVICE INFORMATION
	--UNION ALL
	IF @EntityId = N'PRDS' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT ProdS.AuditUserID UserId, @UserName UserName, N'PRDS' EntityId,'Product Service' RecordType,NULL ActionType,
		'Service:' + S.ServCode + ' in the product: ' + P.ProductCode RecordIdentity, 
		ProdS.ValidityFrom, ProdS.ValidityTo,ProdS.LegacyID,vf,Hist.LegacyID
		FROM tblProductServices ProdS INNER JOIN tblServices S ON ProdS.ServiceID = S.ServiceID
		INNER JOIN tblProduct P ON ProdS.ProdID = P.ProdID
		left outer join(
		select MIN(validityfrom) vf from tblProductServices where LegacyID is not null group by LegacyID) Ins ON ProdS.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblProductServices WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON ProdS.ProdServiceID = Hist.LegacyID
		WHERE (ProdS.AuditUserID = @UserId OR @UserId IS NULL)
		AND ProdS.ValidityFrom BETWEEN @FromDate AND @ToDate

	--RELATIVE DISTRIBUTION INFROMATION
	--UNION ALL
	IF @EntityId = N'RD' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT RD.AuditUserID UserId, @UserName UserName, N'RD' EntityId,'Relative Distribution' RecordType,NULL ActionType,
		'In the Product:' + Prod.ProductCode RecordIdentity, 
		RD.ValidityFrom, RD.ValidityTo,RD.LegacyID,vf,Hist.LegacyID
		FROM tblRelDistr RD INNER JOIN tblProduct Prod ON RD.ProdId = Prod.ProdId
		left outer join(
		select MIN(validityfrom) vf from tblRelDistr where LegacyID is not null group by LegacyID) Ins ON RD.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblRelDistr WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON RD.DistrID = Hist.LegacyID
		WHERE (RD.AuditUserID = @UserId OR @UserId IS NULL)
		AND RD.ValidityFrom BETWEEN @FromDate AND @ToDate

	--MEDICAL SERVICE INFORMATION 
	--UNION ALL
	IF @EntityId = N'S' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT S.AuditUserID UserId, @UserName UserName, N'S' EntityId,'Medical Services' RecordType,NULL ActionType,
		'Code:' + S.ServCode + ' Name:' + S.ServName RecordIdentity, 
		S.ValidityFrom, S.ValidityTo,S.LegacyID,vf,Hist.LegacyID
		FROM tblServices S
		left outer join(
		select MIN(validityfrom) vf from tblServices where LegacyID is not null group by LegacyID) Ins ON S.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblServices WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON S.ServiceID = Hist.LegacyID
		WHERE (S.AuditUserID = @UserId OR @UserId IS NULL)
		AND S.ValidityFrom BETWEEN @FromDate AND @ToDate

	--USERS INFORMATION
	--UNION ALL
	IF @EntityId = N'U' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT U.AuditUserID UserId, @UserName UserName, N'U' EntityId,'Users' RecordType,NULL ActionType,
		'Login:' + U.LoginName RecordIdentity, 
		U.ValidityFrom, U.ValidityTo,U.LegacyID,vf,Hist.LegacyID
		FROM tblUsers U
		left outer join(
		select MIN(validityfrom) vf from tblUsers where LegacyID is not null group by LegacyID) Ins ON U.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblUsers WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON U.UserID = Hist.LegacyID
		WHERE (U.AuditUserID = @UserId OR @UserId IS NULL)
		AND U.ValidityFrom BETWEEN @FromDate AND @ToDate

	--USER DISTRICTS INFORMATION
	--UNION ALL
	IF @EntityId = N'UD' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT UD.AuditUserID UserId, @UserName UserName, N'UD' EntityId,'User Districts' RecordType,NULL ActionType,
		'User:' + U.LoginName + ' Assigned to the District:' + D.DistrictName RecordIdentity, 
		UD.ValidityFrom, UD.ValidityTo,UD.LegacyID,vf,Hist.LegacyID
		FROM tblUsersDistricts UD INNER JOIN tblUsers U ON UD.UserID = U.UserID
		INNER JOIN tblDistricts D ON D.DistrictID = UD.LocationId
		left outer join(
		select MIN(validityfrom) vf from tblUsersDistricts where LegacyID is not null group by LegacyID) Ins ON UD.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyID FROM tblUsersDistricts WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON UD.UserDistrictID = Hist.LegacyID
		WHERE (UD.AuditUserID = @UserId OR @UserId IS NULL)
		AND UD.ValidityFrom BETWEEN @FromDate AND @ToDate

	--VILLAGE INFORMATION
	--UNION ALL
	IF @EntityId = N'V' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT V.AuditUserID UserId, @UserName UserName, N'V' EntityId,'Village' RecordType,NULL ActionType,
		'Village:' + V.VillageName + ' in Municipality:' + W.WardName + ' in District:' + D.DistrictName RecordIdentity, 
		V.ValidityFrom, V.ValidityTo,V.LegacyID,vf,Hist.LegacyID
		FROM tblVillages V INNER JOIN tblWards W ON V.WardID = W.WardID
		INNER JOIN tblDistricts D ON W.DistrictID = D.DistrictID
		left outer join(
		select MIN(validityfrom) vf from tblVillages where LegacyID is not null group by LegacyID) Ins ON V.ValidityFrom = Ins.vf
		LEFT OUTER JOIN
		(SELECT LegacyId FROM tblVillages WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON V.VillageID = Hist.LegacyID
		WHERE (V.AuditUserID = @UserId OR @UserId IS NULL)
		AND V.ValidityFrom BETWEEN @FromDate AND @ToDate

	--WARD INFORMATION
	--UNION ALL
	IF @EntityId = N'W' OR @EntityId = N''
		INSERT INTO @tblLogs(UserId,UserName,EntityId,RecordType,ActionType,RecordIdentity,ValidityFrom,ValidityTo,LegacyId,VF,HistoryLegacyId)
		SELECT W.AuditUserID UserId, @UserName UserName, N'W' EntityId,'Municipality' RecordType,NULL ActionType,
		'Municipality:' + W.WardName + ' in District:' + D.DistrictName RecordIdentity, 
		W.ValidityFrom, W.ValidityTo,W.LegacyID,vf,Hist.LegacyID
		FROM tblWards W INNER JOIN tblDistricts D ON W.DistrictID = D.DistrictID
		left outer join(
		select MIN(validityfrom) vf from tblWards where LegacyID is not null group by LegacyID) Ins ON W.ValidityFrom = Ins.vf
		LEFT OUTER JOIN 
		(SELECT LegacyId FROM tblWards WHERE LegacyID IS NOT NULL GROUP BY LegacyID)Hist ON W.WardID = Hist.LegacyID
		WHERE (W.AuditUserID = @UserId OR @UserId IS NULL)
		AND W.ValidityFrom BETWEEN @FromDate AND @ToDate

	;WITH Result AS
	(
		SELECT UserId,UserName,EntityId,RecordType,
		CASE WHEN ActionType IS NULL AND ( (VF IS NOT NULL OR ((ValidityTo IS  NULL) AND LegacyId IS NULL AND VF IS NULL AND HistoryLegacyId IS NULL))) THEN N'Inserted'      --Inserts (new and updated inserts) 
			 WHEN ((ValidityTo IS NOT NULL) AND LegacyId IS NOT NULL AND VF IS NULL AND HistoryLegacyId IS NULL) THEN N'Modified'
			 WHEN ((ValidityTo IS  NULL) AND LegacyId IS  NULL AND VF IS NULL AND HistoryLegacyId IS NOT NULL) THEN N'Modified'
			 WHEN ((ValidityTo IS NOT NULL) AND LegacyId IS NULL AND VF IS NULL) Then 'Deleted'
			 ELSE ActionType
		END ActionType , RecordIdentity, 
		CASE WHEN ValidityTo IS NOT NULL AND LegacyId IS NULL AND VF IS NULL THEN ValidityTo ELSE ValidityFrom END ActionTime
		FROM @tblLogs
	)SELECT Result.UserId, ISNULL(CASE WHEN Result.UserId <> -1 THEN  U.LoginName ELSE N'Mobile/Offline System' END,N'Unknown') UserName, EntityId, RecordType, ActionType, RecordIdentity, ActionTime 
	FROM Result	LEFT OUTER JOIN tblUsers U ON Result.userId = U.UserID
	WHERE (EntityId = @EntityId OR @EntityId = N'')
	AND (ActionType = @Action OR @Action = N'')
	ORDER BY ActionTime"
        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@UserId", SqlDbType.Int, UserID)
        data.params("@FromDate", SqlDbType.DateTime, StartDate)
        data.params("@ToDate", SqlDbType.DateTime, EndDate)
        data.params("@Action", SqlDbType.NVarChar, 50, Action)
        data.params("@EntityId", SqlDbType.NVarChar, 5, Entity)
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetStatusofRegisters(ByVal LocationId As Integer) As DataTable
        Dim sSQL As String = "SET ARITHABORT OFF;

	    IF @LocationId = -1
		    SET @LocationId = NULL;

	    DECLARE @tblResult TABLE(
		    LocationId INT,
		    ParentLocationId INT,
		    LocationType NVARCHAR(1),
		    LocationName NVARCHAR(100),
		    TotalActiveOfficers INT,
		    TotalNonActiveOfficers INT,
		    TotalUsers INT,
		    TotalProducts INT,
		    TotalHealthFacilities INT,
		    TotalItemPriceLists INT,
		    TotalServicePriceLists INT,
		    TotalItems INT,
		    TotalServices INT,
		    TotalPayers INT
	    );

	    ;WITH LocationsAll AS
		    (
		    SELECT -1 LocationId, N'National' LocationName, NULL ParentLocationId, NULL LocationType
		    UNION
		    SELECT LocationId,LocationName, ISNULL(ParentLocationId, -1)ParentLocationId, LocationType FROM tblLocations WHERE LocationType IN ('D', 'R') AND ValidityTo IS NULL AND (LocationId = @LocationId OR CASE WHEN @LocationId IS NULL THEN ISNULL(ParentLocationId, 0) ELSE 0 END = ISNULL(@LocationId, 0))
		    UNION ALL
		    SELECT L.LocationId, L.LocationName, L.ParentLocationId, L.LocationType
		    FROM tblLocations L 
		    INNER JOIN LocationsAll ON LocationsAll.LocationId = L.ParentLocationId
		    WHERE L.ValidityTo IS NULL
		    AND L.LocationType = N'D'
		    ),Locations AS(
			    SELECT Locationid, LocationName, ParentLocationId, LocationType
			    FROM LocationsAll
			    GROUP BY LocationID, LocationName, ParentLocationId, LocationType
		    )


		    INSERT INTO @tblResult(LocationId, ParentLocationId, LocationType, LocationName, TotalActiveOfficers, TotalNonActiveOfficers, TotalUsers, TotalProducts, TotalHealthFacilities, TotalItemPriceLists, TotalServicePriceLists, TotalItems, TotalServices, TotalPayers)
	
		    SELECT Locations.LocationId, NULLIF(Locations.ParentLocationId, -1)ParentLocationId, Locations.LocationType ,Locations.LocationName,ActiveOfficers.TotalEnrollmentOfficers TotalActiveOfficers
		    , NonActiveOfficers.TotalEnrollmentOfficers TotalNonActiveOfficers 
		    ,Users.TotalUsers,TotalProducts ,HF.TotalHealthFacilities ,PLItems.TotalItemPriceLists,PLServices.TotalServicePriceLists ,
		    PLItemDetails.TotalItems,PLServiceDetails.TotalServices,Payers.TotalPayers
		    FROM
		    (SELECT COUNT(O.OfficerId)TotalEnrollmentOfficers,ISNULL(L.LocationId, -1)LocationId 
		    FROM Locations L
		    LEFT OUTER JOIN tblOfficer O ON ISNULL(O.LocationId, -1) = L.LocationId AND O.ValidityTo IS NULL
		    WHERE ISNULL(CAST(WorksTo AS DATE) , DATEADD(DAY, 1, GETDATE())) > CAST(GETDATE() AS DATE) 
		    GROUP BY L.LocationId) ActiveOfficers INNER JOIN Locations ON Locations.LocationId = ActiveOfficers.LocationId 

		    LEFT OUTER JOIN
		    (SELECT COUNT(O.OfficerId)TotalEnrollmentOfficers,ISNULL(L.LocationId, -1)LocationId 
		    FROM Locations L
		    LEFT OUTER JOIN tblOfficer O ON ISNULL(O.LocationId, -1) = L.LocationId AND O.ValidityTo IS NULL
		    WHERE CAST(WorksTo AS DATE) <= CAST(GETDATE() AS DATE) 
		    GROUP BY L.LocationId
		    ) NonActiveOfficers ON Locations.LocationId = NonActiveOfficers.LocationId

		    LEFT OUTER JOIN
		    (SELECT COUNT(U.UserID) TotalUsers,ISNULL(L.LocationId, -1)LocationId 
		    FROM tblUsers U 
		    INNER JOIN tblUsersDistricts UD ON U.UserID = UD.UserID AND U.ValidityTo IS NULL AND UD.ValidityTo IS NULL
		    RIGHT OUTER JOIN Locations L ON L.LocationId = UD.LocationId
		    GROUP BY L.LocationId)Users ON Locations.LocationId = Users.LocationId

		    LEFT OUTER JOIN 
		    (SELECT COUNT(Prod.ProdId)TotalProducts, ISNULL(L.LocationId, -1)LocationId 
		    FROM Locations L
		    LEFT OUTER JOIN tblProduct Prod ON ISNULL(Prod.Locationid, -1) = L.LocationId AND Prod.ValidityTo IS NULL 
		    GROUP BY L.LocationId) Products ON Locations.LocationId = Products.LocationId

		    LEFT OUTER JOIN 
		    (SELECT COUNT(HF.HfID)TotalHealthFacilities, ISNULL(L.LocationId, -1)LocationId 
		    FROM Locations L
		    LEFT OUTER JOIN tblHF HF ON ISNULL(HF.LocationId, -1) = L.LocationId AND HF.ValidityTo IS NULL
		    GROUP BY L.LocationId) HF ON Locations.LocationId = HF.LocationId

		    LEFT OUTER JOIN 
		    (SELECT COUNT(PLI.PLItemID) TotalItemPriceLists, ISNULL(L.LocationId, -1)LocationId 
		    FROM Locations L
		    LEFT OUTER JOIN tblPLItems PLI ON ISNULL(PLI.LocationId, -1) = L.LocationId AND PLI.ValidityTo IS NULL
		    GROUP BY L.LocationId) PLItems ON Locations.LocationId = PLItems.LocationId

		    LEFT OUTER JOIN
		    (SELECT COUNT(PLS.PLServiceID) TotalServicePriceLists,ISNULL(L.LocationId, -1)LocationId 
		    FROM Locations L
		    LEFT OUTER JOIN tblPLServices PLS ON ISNULL(PLS.LocationId, -1) = L.LocationId AND PLS.ValidityTo IS NULL 
		    GROUP BY L.LocationId) PLServices ON Locations.LocationId = PLServices.LocationId

		    LEFT OUTER JOIN
		    (SELECT COUNT(ItemId)TotalItems, LocationId
		    FROM (
			    SELECT I.ItemID, ISNULL(L.LocationId, -1)LocationId
			    FROM Locations L
			    LEFT OUTER JOIN tblPLItems PL ON ISNULL(PL.LocationId, -1) = L.LocationId AND PL.ValidityTo IS NULL
			    LEFT OUTER JOIN tblPLItemsDetail I ON I.PLItemID = PL.PLItemID
			    GROUP BY I.ItemId, L.LocationId
		    )x
		    GROUP BY LocationId)PLItemDetails ON Locations.LocationId = PLItemDetails.LocationId

		    LEFT OUTER JOIN
		    (SELECT COUNT(ServiceID)TotalServices, LocationId
		    FROM (
			    SELECT S.ServiceId, ISNULL(L.LocationId, -1)LocationId
			    FROM Locations L
			    LEFT OUTER JOIN tblPLServices PL ON ISNULL(PL.LocationId, -1) = L.LocationId AND PL.ValidityTo IS NULL
			    LEFT OUTER JOIN tblPLServicesDetail S ON S.PLServiceID = PL.PLServiceID 
			    GROUP BY S.ServiceID, L.LocationId
		    )x
		    GROUP BY LocationId)PLServiceDetails ON Locations.LocationId = PLServiceDetails.LocationId

		    LEFT OUTER JOIN
		    (SELECT COUNT(P.PayerId)TotalPayers,ISNULL(L.LocationId, -1)LocationId 
		    FROM Locations L 
		    LEFT OUTER JOIN tblPayer P ON ISNULL(P.LocationId, -1) = L.LocationId AND P.ValidityTo IS NULL 
		    GROUP BY L.LocationId)Payers ON Locations.LocationId = Payers.LocationId

	    IF @LocationId = 0
	    BEGIN
		    ;WITH Results AS
		    (
			    SELECT 0 [Level],LocationId, ParentLocationId, Locationname, LocationType,
			    TotalActiveOfficers, TotalNonActiveOfficers, TotalUsers, TotalProducts, TotalHealthFacilities, TotalItemPriceLists, TotalServicePriceLists, TotalItems, TotalServices, TotalPayers
			    FROM @tblResult 
			    UNION ALL
			    SELECT Results.[Level] + 1, R.LocationId, R.ParentLocationId, R.LocationName, R.LocationType,
			    Results.TotalActiveOfficers, Results.TotalNonActiveOfficers, Results.TotalUsers, Results.TotalProducts, Results.TotalHealthFacilities, Results.TotalItemPriceLists, Results.TotalServicePriceLists, Results.TotalItems, Results.TotalServices, Results.TotalPayers
			    FROM @tblResult R
			    INNER JOIN Results ON R.LocationId = Results.ParentLocationId
		    )
		    SELECT LocationId, LocationName
		    , NULLIF(SUM(TotalActiveOfficers), 0) TotalActiveOfficers
		    , NULLIF(SUM(TotalNonActiveOfficers), 0)TotalNonActiveOfficers
		    , NULLIF(SUM(TotalUsers), 0)TotalUsers
		    , NULLIF(SUM(TotalProducts), 0)TotalProducts
		    , NULLIF(SUM(TotalHealthFacilities), 0) TotalHealthFacilities
		    , NULLIF(SUM(TotalItemPriceLists) , 0)TotalItemPriceLists
		    , NULLIF(SUM(TotalServicePriceLists), 0) TotalServicePriceLists
		    , NULLIF(SUM(TotalItems), 0)TotalItems
		    , NULLIF(SUM(TotalServices), 0) TotalServices
		    , NULLIF(SUM(TotalPayers), 0)TotalPayers

		    FROM Results
		    WHERE LocationType = 'R' OR LocationType IS NULL
		    GROUP BY LocationId, LocationName
		    ORDER BY LocationId
	    END
	    ELSE
	    BEGIN
		    SELECT LocationId, LocationName, NULLIF(TotalActiveOfficers, 0)TotalActiveOfficers, NULLIF(TotalNonActiveOfficers, 0)TotalNonActiveOfficers, NULLIF(TotalUsers, 0)TotalUsers, NULLIF(TotalProducts, 0)TotalProducts, NULLIF(TotalHealthFacilities, 0)TotalHealthFacilities, NULLIF(TotalItemPriceLists, 0)TotalItemPriceLists, NULLIF(TotalServicePriceLists, 0)TotalServicePriceLists, NULLIF(TotalItems, 0)TotalItems, NULLIF(TotalServices, 0)TotalServices, NULLIF(TotalPayers, 0)TotalPayers  
		    FROM @tblResult
		    WHERE LocationId <> -1;
	    END"
        'Dim sSQL As String = "uspSSRSStatusRegister"
        Dim data As New ExactSQL

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@LocationID", SqlDbType.Int, LocationId)

        Return data.Filldata()
    End Function

    'Corrected
    Public Function GetInsureesWithoutPhotos(ByVal OfficerId As Integer, ByVal LocationId As Integer) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = ";WITH Locations AS("
        sSQL += " SELECT LocationId, ParentLocationId FROM tblLocations WHERE ValidityTo IS NULL AND (LocationId = @LocationId OR CASE WHEN @LocationId IS NULL THEN ISNULL(ParentLocationId, 0) ELSE 0 END = ISNULL(@LocationId, 0))"
        sSQL += " UNION ALL"
        sSQL += " SELECT L.LocationId, L.ParentLocationId"
        sSQL += " FROM tblLocations L"
        sSQL += " INNER JOIN Locations ON Locations.LocationId = L.ParentLocationId"
        sSQL += " WHERE L.ValidityTo IS NULL"
        sSQL += " )"
        sSQL += " SELECT I.CHFID,I.LastName, I.OtherNames,I.Gender,I.IsHead,D.DistrictName,W.WardName,V.VillageName,O.Code,O.LastName OfficerLastName,"
        sSQL += " O.OtherNames OfficerOtherNames , IIF(ISNULL(CAST(WorksTo AS DATE) , DATEADD(DAY, 1, GETDATE())) <= CAST(GETDATE() AS DATE), 'N', 'A') OfficerStatus"
        sSQL += " FROM tblFamilies F"
        sSQL += " INNER JOIN Locations ON Locations.LocationId = F.LocationId"
        sSQL += " INNER JOIN tblInsuree I ON F.FamilyID = I.FamilyID"
        sSQL += " INNER JOIN tblPhotos PH ON I.InsureeID = PH.InsureeID"
        sSQL += " LEFT OUTER JOIN tblPolicy P ON F.FamilyID = P.FamilyID"
        sSQL += " LEFT OUTER JOIN tblOfficer O ON P.OfficerID = O.OfficerID"
        sSQL += " INNER JOIN tblVillages V ON V.VillageId = F.LocationId"
        sSQL += " INNER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId"
        sSQL += " INNER JOIN tblRegions R ON R.RegionId = D.Region"
        sSQL += " WHERE F.ValidityTo Is NULL"
        sSQL += " And I.ValidityTo Is NULL"
        sSQL += " And P.ValidityTo Is NULL"
        sSQL += " And PH.ValidityTo Is NULL"
        sSQL += " And D.ValidityTo Is NULL"
        sSQL += " And W.ValidityTo Is NULL"
        sSQL += " And V.ValidityTo Is NULL"
        sSQL += " AND R.ValidityTo IS NULL"
        sSQL += " AND LEN(RTRIM(LTRIM(PH.PhotoFileName))) = 0"
        sSQL += " AND(P.OfficerID = @OfficerId OR @OfficerId = 0)"
        sSQL += " GROUP BY I.CHFID, I.LastName,I.OtherNames,I.Gender,I.IsHead,D.DistrictName,W.WardName,V.VillageName,O.Code,O.LastName,O.OtherNames, O.WorksTo"

        data.setSQLCommand(sSQL, CommandType.Text, timeout:=0)

        data.params("@OfficerId", SqlDbType.Int, OfficerId)
        data.params("@LocationID", SqlDbType.Int, If(LocationId = -1, Nothing, LocationId))

        Return data.Filldata()
    End Function

    'Corrected
    Public Function GetPaymentCategoryOverview(ByVal DateFrom As DateTime, ByVal DateTo As DateTime, ByVal LocationId As Integer, ByVal ProductId As Integer) As DataTable
        Dim Data As New ExactSQL
        'Dim sSQL As String = "uspSSRSPaymentCategoryOverview"
        Dim sSQL As String = ";WITH InsureePolicy AS
	    (
		    SELECT COUNT(IP.InsureeId) TotalMembers, IP.PolicyId
		    FROM tblInsureePolicy IP
		    WHERE IP.ValidityTo IS NULL
		    GROUP BY IP.PolicyId
	    ), [Main] AS
	    (
		    SELECT PL.PolicyId, Prod.ProdID, PL.FamilyId, SUM(CASE WHEN PR.isPhotoFee = 0 THEN PR.Amount ELSE 0 END)TotalPaid,
		    SUM(CASE WHEN PR.isPhotoFee = 1 THEN PR.Amount ELSE 0 END)PhotoFee,
		    COALESCE(Prod.RegistrationLumpsum, IP.TotalMembers * Prod.RegistrationFee, 0)[Registration],
		    COALESCE(Prod.GeneralAssemblyLumpsum, IP.TotalMembers * Prod.GeneralAssemblyFee, 0)[Assembly]

		    FROM tblPremium PR
		    INNER JOIN tblPolicy PL ON PL.PolicyId = PR.PolicyID
		    INNER JOIN InsureePolicy IP ON IP.PolicyId = PL.PolicyID
		    INNER JOIN tblProduct Prod ON Prod.ProdId = PL.ProdID
	
		    WHERE PR.ValidityTo IS NULL
		    AND PL.ValidityTo IS NULL
		    AND Prod.ValidityTo IS NULL
		    AND PR.PayTYpe <> 'F'
		    AND PR.PayDate BETWEEN @DateFrom AND @DateTo
		    AND (Prod.ProdID = @ProductId OR @ProductId = 0)
	

		    GROUP BY PL.PolicyId, Prod.ProdID, PL.FamilyId, IP.TotalMembers, Prod.GeneralAssemblyLumpsum, Prod.GeneralAssemblyFee, Prod.RegistrationLumpsum, Prod.RegistrationFee
	    ), RegistrationAndAssembly AS
	    (
		    SELECT PolicyId, 
		    CASE WHEN TotalPaid - Registration >= 0 THEN Registration ELSE TotalPaid END R,
		    CASE WHEN TotalPaid - Registration > 0 THEN CASE WHEN TotalPaid - Registration - [Assembly] >= 0 THEN [Assembly] ELSE TotalPaid - Registration END ELSE 0 END A
		    FROM [Main]
	    ), Overview AS
	    (
		    SELECT Main.ProdId, Main.PolicyId, Main.FamilyId, RA.R, RA.A,
		    CASE WHEN TotalPaid - RA.R - Main.[Assembly] >= 0 THEN TotalPaid - RA.R - Main.[Assembly] ELSE Main.TotalPaid - RA.R - RA.A END C,
		    Main.PhotoFee
		    FROM [Main] 
		    INNER JOIN RegistrationAndAssembly RA ON Main.PolicyId = RA.PolicyID
	    )

	    SELECT Prod.ProdId, Prod.ProductCode, Prod.ProductName, D.DistrictName, SUM(O.R) R, SUM(O.A)A, SUM(O.C)C, SUM(PhotoFee)P
	    FROM Overview O
	    INNER JOIN tblProduct Prod ON Prod.ProdID = O.ProdId
	    INNER JOIN tblFamilies F ON F.FamilyId = O.FamilyID
	    INNER JOIN tblVillages V ON V.VillageId = F.LocationId
	    INNER JOIN tblWards W ON W.WardId = V.WardId
	    INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId

	    WHERE Prod.ValidityTo IS NULL
	    AND F.ValidityTo IS NULL
	    AND (D.Region = @LocationId OR D.DistrictId = @LocationId OR @LocationId = 0)

	    GROUP BY Prod.ProdId, Prod.ProductCode, Prod.ProductName, D.DistrictName"
        Data.setSQLCommand(sSQL, CommandType.Text)

        Data.params("@DateFrom", DateFrom)
        Data.params("@DateTo", DateTo)
        Data.params("@LocationId", LocationId)
        Data.params("@ProductId", ProductId)

        Return Data.Filldata
    End Function

    'Corrected
    Public Function GetMatchingFunds(ByVal LocationId As Integer?, ByVal ProdID As Integer?, ByVal PayerID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ReportingID As Integer?, ByRef ErrorMessage As String, ByRef oReturn As Integer) As DataTable
        Dim Data As New ExactSQL
        Dim sSQL As String = "DECLARE @RecordFound INT = 0

	    --Create new entries only if reportingId is not provided

	    IF @ReportingId IS NULL
	    BEGIN

		    IF @LocationId IS NULL RETURN 1;
		    IF @ProdId IS NULL RETURN 2;
		    IF @StartDate IS NULL RETURN 3;
		    IF @EndDate IS NULL RETURN 4;
		
		    BEGIN TRY
			    BEGIN TRAN
				    --Insert the entry into the reporting table
				    INSERT INTO tblReporting(ReportingDate,LocationId, ProdId, PayerId, StartDate, EndDate, RecordFound,OfficerID,ReportType)
				    SELECT GETDATE(),@LocationId, @ProdId, @PayerId, @StartDate, @EndDate, 0,null,1;

				    --Get the last inserted reporting Id
				    SELECT @ReportingId =  SCOPE_IDENTITY();

	
				    --Update the premium table with the new reportingid

				    UPDATE tblPremium SET ReportingId = @ReportingId
				    WHERE PremiumId IN (
				    SELECT Pr.PremiumId--,Prod.ProductCode, Prod.ProductName, D.DistrictName, W.WardName, V.VillageName, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName, 
				    --Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount, Payer.PayerName
				    FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID
				    INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
				    INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
				    INNER JOIN tblVillages V ON V.VillageId = F.LocationId
				    INNER JOIN tblWards W ON W.WardId = V.WardId
				    INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
				    LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
				    left join tblReporting ON PR.ReportingId =tblReporting.ReportingId AND tblReporting.ReportType=1
				    WHERE Pr.ValidityTo IS NULL 
				    AND PL.ValidityTo IS NULL
				    AND Prod.ValidityTo IS NULL
				    AND F.ValidityTo IS NULL
				    AND D.ValidityTo IS NULL
				    AND W.ValidityTo IS NULL
				    AND V.ValidityTo IS NULL
				    AND Payer.ValidityTo IS NULL

				    AND D.DistrictID = @LocationId
				    AND PayDate BETWEEN @StartDate AND @EndDate
				    AND Prod.ProdID = @ProdId
				    AND (ISNULL(Payer.PayerID,0) = ISNULL(@PayerId,0) OR @PayerId IS NULL)
				    AND Pr.ReportingId IS NULL
				    AND PR.PayType <> N'F'
				    )

				    SELECT @RecordFound = @@ROWCOUNT;

				    UPDATE tblReporting SET RecordFound = @RecordFound WHERE ReportingId = @ReportingId;

			    COMMIT TRAN;
		    END TRY
		    BEGIN CATCH
			    --SELECT @ErrorMessage = ERROR_MESSAGE(); ERROR MESSAGE WAS COMMENTED BY SALUMU ON 12-11-2019
			    ROLLBACK;
			    --RETURN -1 RETURN WAS COMMENTED BY SALUMU ON 12-11-2019
		    END CATCH
	    END
	
	    SELECT Pr.PremiumId,Prod.ProductCode, Prod.ProductName,F.FamilyID, D.DistrictName, W.WardName, V.VillageName, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName, 
	    Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount, Payer.PayerName
	    FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID
	    INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
	    INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
	    INNER JOIN tblVillages V ON V.VillageId = F.LocationId
	    INNER JOIN tblWards W ON W.WardId = V.WardId
	    INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
	    INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
	    LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
	    WHERE Pr.ReportingId = @ReportingId
	    ORDER BY PremiumId DESC, IsHead DESC;

	    SET @ErrorMessage = N''

	    --RETURN 0; RETURN WAS COMMENTED BY SALUMU ON 12-11-2019"
        Data.setSQLCommand("uspSSRSGetMatchingFunds", CommandType.Text)
        Data.params("@LocationId", SqlDbType.Int, LocationId)
        Data.params("@ProdID", SqlDbType.Int, ProdID)
        Data.params("@PayerID", SqlDbType.Int, PayerID)
        Data.params("@StartDate", SqlDbType.Date, StartDate)
        Data.params("@EndDate", SqlDbType.Date, EndDate)
        Data.params("@ReportingID", SqlDbType.Int, If(ReportingID = 0, DBNull.Value, ReportingID))
        Data.params("@ErrorMessage", SqlDbType.NVarChar, 200, "", ParameterDirection.Output)
        Data.params("@RV", SqlDbType.Int, 0, ParameterDirection.ReturnValue)
        Dim dt As DataTable = Data.Filldata()
        oReturn = Data.sqlParameters("@RV")
        ErrorMessage = Data.sqlParameters("@ErrorMessage").ToString
        Return dt
    End Function

    Public Function getRejectedPhoto(startDate As Date, endDate As Date) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = " ;WITH RejectedPhotos AS( "
        sSQL += "SELECT CHFID, OfficerCode, "
        sSQL += " Convert(VARCHAR(11), Convert(Date, SUBSTRING(SUBSTRING(SUBSTRING(DocName, CHARINDEX('_', DocName, 1) + 1,  LEN(DocName)-1), CHARINDEX('_', SUBSTRING(DocName, CHARINDEX('_', DocName, 1) + 1,  LEN(DocName)-1), 1) + 1,  LEN(SUBSTRING(DocName, CHARINDEX('_', DocName, 1) + 1,  LEN(DocName)-1))-1), 0, 9)),101) RejectedDate  FROM tblFromPhone WHERE DocType='E' AND DocStatus='R') "
        sSQL += "SELECT CHFID, OfficerCode, RejectedDate FROM RejectedPhotos WHERE 1=1 "
        If startDate.ToString().Length > 0 Then
            sSQL += " AND RejectedDate >=@StartDate"
        End If
        If endDate.ToString().Length > 0 Then
            sSQL += " AND RejectedDate <=@EndDate"
        End If
        sSQL += " ORDER BY OfficerCode ASC"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@StartDate", SqlDbType.Date, startDate)
        data.params("@EndDate", SqlDbType.Date, endDate)
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetClaimOverview(ByVal LocationId As Integer?, ByVal ProdID As Integer?, ByVal HfID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ClaimStatus As Integer?, ByVal Scope As Integer?, ByVal dtRejReasons As DataTable, ByRef oReturn As Integer) As DataTable

        Dim Data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = ""
        If Scope = 0 Then
            sSQL = ";WITH TotalForItems AS
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
	        --INNER JOIN tblProduct PROD ON PROD.ProdID = CS.ProdID AND PROD.ProdID = CI.ProdID
	        INNER JOIN tblHF HF ON C.HFID = HF.HfID
	        LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
	        INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
	        LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
	        LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId

	        WHERE C.ValidityTo IS NULL
	        AND ISNULL(C.DateTo,C.DateFrom) BETWEEN @StartDate AND @EndDate
	        AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
	        AND (HF.LocationId = @LocationId OR @LocationId = 0)
	        AND (HF.HFID = @HFID OR @HFID = 0)
	        AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) "



            Data.setSQLCommand(sSQL, CommandType.Text)
        ElseIf Scope = 1 Then
            sSQL = ";WITH TotalForItems AS
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
	        CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN I.ItemCode+' '+I.ItemName ELSE NULL END AdjustedItem,
	        CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN CI.QtyProvided ELSE NULL END OrgQtyItem,
	        CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved ,CI.QtyProvided)  THEN CI.QtyApproved ELSE NULL END AdjQtyItem,
	        CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)  THEN S.ServCode+' ' + S.ServName ELSE NULL END AdjustedService,
	        CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)   THEN CS.QtyProvided ELSE NULL END OrgQtyService,
	        CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved ,CS.QtyProvided)   THEN CS.QtyApproved ELSE NULL END AdjQtyService,
	        C.Explanation
	        ,CS.QtyApproved ServiceQtyApproved, CI.QtyApproved ItemQtyApproved,cs.PriceAsked ServicePrice, CI.PriceAsked ItemPrice
	        ,ISNULL(cs.PriceApproved,0) ServicePriceApproved,ISNULL(ci.PriceApproved,0) ItemPriceApproved, ISNULL(cs.Justification,NULL) ServiceJustification,
	        ISNULL(CI.Justification,NULL) ItemJustification,cs.ClaimServiceID,CI.ClaimItemID--,cs.PriceApproved ServicePriceApproved,ci.PriceApproved ItemPriceApproved--,
	        ,CONCAT(CS.RejectionReason,' - ', XCS.Name) ServiceRejectionReason,CONCAT(CI.RejectionReason, ' - ', XCI.Name) ItemRejectionReason


	        FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
	        LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID 
	        LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID
	        LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID 
	        --INNER JOIN tblProduct PROD ON PROD.ProdID = CS.ProdID AND PROD.ProdID = CI.ProdID
	        INNER JOIN tblHF HF ON C.HFID = HF.HfID
	        LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
	        INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
	        LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
	        LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId
	        INNER JOIN @ClaimRejReason XCI ON XCI.ID = CI.RejectionReason
	        INNER JOIN @ClaimRejReason XCS ON XCS.ID = CS.RejectionReason
	        WHERE C.ValidityTo IS NULL
	        AND ISNULL(C.DateFrom,C.DateTo) BETWEEN @StartDate AND @EndDate
	        AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
	        AND (HF.LocationId = @LocationId OR @LocationId = 0)
	        AND HF.HFID = @HFID
	        AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) "
            Data.setSQLCommand(sSQL, CommandType.Text)
        Else
            sSQL = ";WITH TotalForItems AS
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
	        CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN I.ItemCode+' '+I.ItemName ELSE NULL END AdjustedItem,
	        CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN CI.QtyProvided ELSE NULL END OrgQtyItem,
	        CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved ,CI.QtyProvided)  THEN CI.QtyApproved ELSE NULL END AdjQtyItem,
	        CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)  THEN S.ServCode+' '+S.ServName ELSE NULL END AdjustedService,
	        CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)   THEN CS.QtyProvided ELSE NULL END OrgQtyService,
	        CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved ,CS.QtyProvided)   THEN CS.QtyApproved ELSE NULL END AdjQtyService,
	        C.Explanation
	        ,CS.QtyApproved ServiceQtyApproved, ISNULL(CI.QtyApproved,0) ItemQtyApproved,ISNULL(cs.PriceAsked,0) ServicePrice, ISNULL(CI.PriceAsked,0) ItemPrice
	        ,ISNULL(cs.PriceApproved,0) ServicePriceApproved,ISNULL(ci.PriceApproved,0) ItemPriceApproved, cs.Justification ServiceJustification,
	        CI.Justification ItemJustification,cs.ClaimServiceID,CI.ClaimItemID,ISNULL(CI.PriceValuated,0) ItemPriceValuated,ISNULL(CS.PriceValuated,0) ServicePriceValuated
	        ,CONCAT(CS.RejectionReason,' - ', XCS.Name) ServiceRejectionReason,CONCAT(CI.RejectionReason, ' - ', XCI.Name) ItemRejectionReason


	        FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
	        LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
	        LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID
	        LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID
	        INNER JOIN tblHF HF ON C.HFID = HF.HfID
	        LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
	        INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
	        LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
	        LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId
	        LEFT OUTER JOIN @ClaimRejReason XCI ON XCI.ID = CI.RejectionReason
	        LEFT OUTER JOIN @ClaimRejReason XCS ON XCS.ID = CS.RejectionReason
	        WHERE C.ValidityTo IS NULL
	        AND ISNULL(C.DateTo,C.DateFrom) BETWEEN @StartDate AND @EndDate
	        AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
	        AND (HF.LocationId = @LocationId OR @LocationId = 0)
	        AND (HF.HFID = @HFID OR @HFID = 0)
	        AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) "
            Data.setSQLCommand(sSQL, CommandType.Text)
        End If

        Data.params("@HfID", SqlDbType.Int, HfID)
        Data.params("@LocationId", SqlDbType.Int, LocationId)
        Data.params("@ProdID", SqlDbType.Int, ProdID)
        Data.params("@StartDate", SqlDbType.Date, StartDate)
        Data.params("@EndDate", SqlDbType.Date, EndDate)
        Data.params("@ClaimStatus", SqlDbType.Int, ClaimStatus)
        If Not Scope = 0 Then
            Data.params("@ClaimRejReason", dtRejReasons, "xClaimRejReasons")
        End If
        Data.params("@RV", SqlDbType.Int, 0, ParameterDirection.ReturnValue)
        Dim dt As DataTable = Data.Filldata()
        oReturn = Data.sqlParameters("@RV")
        Return dt
    End Function

    'Corrected
    Public Function GetPercentageReferral(RegionId As Integer, DistrictId As Integer, StartDate As Date, EndDate As Date) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String
        'sSQL = "SELECT CONCAT(HF.HFCode,' - ', HF.HFName)HF, TotalClaim.TotalClaims, RefOP.TotalOP, RefIP.TotalIP FROM" & _
        '       " (SELECT HF.HFID, HF.HFCode, HF.HFName FROM tblHF HF WHERE HF.ValidityTo Is NULL AND HF.HFLevel IN ('D','C') AND HF.LocationId = @LocationId)HF" & _
        '       " LEFT OUTER JOIN (SELECT COUNT(1) TotalClaims, HFID FROM tblClaim WHERE ValidityTo Is NULL AND DateClaimed BETWEEN @StartDate AND @EndDate GROUP BY HFID )TotalClaim ON HF.HfID = TotalClaim.HFID" & _
        '       " LEFT OUTER JOIN (SELECT HF.HfID,COUNT(C.ClaimID)TotalOP FROM tblCLaim C INNER JOIN tblInsuree I ON C.InsureeId = I.InsureeID" & _
        '       " INNER JOIN tblHF HF ON C.HFID = HF.HfID WHERE C.ValidityTo Is NULL AND I.ValidityTo IS NULL AND HF.ValidityTo IS NULL AND C.DateFrom = C.DateTo AND HF.HFId <> I.HFID AND C.VisitType = N'R' AND LocationId = @LocationId AND C.DateClaimed BETWEEN @StartDate AND @EndDate GROUP BY HF.HfID )RefOP ON HF.HFId = RefOP.HFID" & _
        '       " LEFT OUTER JOIN (SELECT HF.HfID,COUNT(C.ClaimID)TotalIP FROM tblCLaim C INNER JOIN tblInsuree I ON C.InsureeId = I.InsureeID INNER JOIN tblHF HF ON C.HFID = HF.HfID" & _
        '       " WHERE C.ValidityTo Is NULL AND I.ValidityTo IS NULL AND HF.ValidityTo IS NULL AND C.DateFrom <> C.DateTo AND HF.HFId <> I.HFID AND C.VisitType = N'R' AND LocationId = @LocationId AND C.DateClaimed BETWEEN @StartDate AND @EndDate GROUP BY HF.HfID )RefIP ON HF.HFId = RefIP.HFID"

        sSQL = " SELECT CONCAT(HF.HFCode,' - ', HF.HFName)HF, TotalClaim.TotalClaims, RefOP.TotalOP, RefIP.TotalIP"
        sSQL += " FROM ("
        sSQL += " SELECT HF.HFID, HF.HFCode, HF.HFName FROM tblHF HF"
        sSQL += " INNER JOIN uvwLocations L ON L.LocationId =HF.LocationId"
        sSQL += " WHERE HF.ValidityTo Is NULL AND HF.HFLevel IN ('D','C')"
        sSQL += " AND (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId IS NULL )"
        sSQL += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL))HF"
        sSQL += " LEFT OUTER JOIN ("
        sSQL += " SELECT COUNT(1) TotalClaims, HFID FROM tblClaim WHERE ValidityTo Is NULL AND DateClaimed"
        sSQL += " BETWEEN @StartDate AND @EndDate GROUP BY HFID"
        sSQL += " )TotalClaim ON HF.HfID = TotalClaim.HFID"
        sSQL += " LEFT OUTER JOIN (SELECT HF.HfID,COUNT(C.ClaimID)TotalOP FROM tblCLaim C"
        sSQL += " INNER JOIN tblInsuree I ON C.InsureeId = I.InsureeID"
        sSQL += " INNER JOIN tblHF HF ON C.HFID = HF.HfID"
        sSQL += " INNER JOIN uvwLocations L ON L.LocationId =HF.LocationId"
        sSQL += " WHERE C.ValidityTo Is NULL"
        sSQL += " AND I.ValidityTo IS NULL"
        sSQL += " AND HF.ValidityTo IS NULL AND C.DateFrom = C.DateTo AND HF.HFId <> I.HFID"
        sSQL += " AND C.VisitType = N'R'"
        sSQL += " AND (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId IS NULL )"
        sSQL += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL)"
        sSQL += " AND C.DateClaimed BETWEEN @StartDate"
        sSQL += " AND @EndDate GROUP BY HF.HfID )RefOP ON HF.HFId = RefOP.HFID"
        sSQL += " LEFT OUTER JOIN (SELECT HF.HfID,COUNT(C.ClaimID)TotalIP FROM tblCLaim C"
        sSQL += " INNER JOIN tblInsuree I ON C.InsureeId = I.InsureeID"
        sSQL += " INNER JOIN tblHF HF ON C.HFID = HF.HfID"
        sSQL += " INNER JOIN uvwLocations L ON L.LocationId =HF.LocationId"
        sSQL += " WHERE C.ValidityTo Is NULL"
        sSQL += " AND I.ValidityTo IS NULL"
        sSQL += " AND HF.ValidityTo IS NULL"
        sSQL += " AND C.DateFrom <> C.DateTo"
        sSQL += " AND HF.HFId <> I.HFID AND C.VisitType = N'R'"
        sSQL += " AND (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId IS NULL )"
        sSQL += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL)"
        sSQL += " AND C.DateClaimed BETWEEN @StartDate"
        sSQL += " AND @EndDate GROUP BY HF.HfID )RefIP ON HF.HFId = RefIP.HFID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@RegionId", SqlDbType.Int, RegionId)
        data.params("@DistrictId", SqlDbType.Int, DistrictId)
        data.params("@StartDate", SqlDbType.Date, StartDate)
        data.params("@EndDate", SqlDbType.Date, EndDate)

        Return data.Filldata
    End Function
    'Corrected by Rogers
    Public Function GetProcessBatchWithClaims(ByVal LocationId As Integer, ByVal ProductId As Integer, ByVal RunID As Integer, ByVal HFID As Integer, ByVal HFLevel As String, ByVal DateFrom As Nullable(Of Date), ByVal DateTo As Nullable(Of Date), ByVal MinRemunerated As Decimal) As DataTable
        Dim data As New ExactSQL
        'Dim sSQL As String = "uspSSRSProcessBatchWithClaim"
        Dim sSQL As String = "IF @DateFrom = '' OR @DateFrom IS NULL OR @DateTo = '' OR @DateTo IS NULL
	    BEGIN
		    SET @DateFrom = N'1900-01-01'
		    SET @DateTo = N'3000-12-31'
	    END

	    ;WITH CDetails AS
	    (
		    SELECT CI.ClaimId, CI.ProdId,
		    SUM(ISNULL(CI.PriceApproved, CI.PriceAsked) * ISNULL(CI.QtyApproved, CI.QtyProvided)) PriceApproved,
		    SUM(CI.PriceValuated) PriceAdjusted, SUM(CI.RemuneratedAmount)RemuneratedAmount
		    FROM tblClaimItems CI
		    WHERE CI.ValidityTo IS NULL
		    AND CI.ClaimItemStatus = 1
		    GROUP BY CI.ClaimId, CI.ProdId
		    UNION ALL

		    SELECT CS.ClaimId, CS.ProdId,
		    SUM(ISNULL(CS.PriceApproved, CS.PriceAsked) * ISNULL(CS.QtyApproved, CS.QtyProvided)) PriceApproved,
		    SUM(CS.PriceValuated) PriceValuated, SUM(CS.RemuneratedAmount) RemuneratedAmount

		    FROM tblClaimServices CS
		    WHERE CS.ValidityTo IS NULL
		    AND CS.ClaimServiceStatus = 1
		    GROUP BY CS.CLaimId, CS.ProdId
	    )
	    SELECT C.ClaimCode, C.DateClaimed, CA.OtherNames OtherNamesAdmin, CA.LastName LastNameAdmin, C.DateFrom, C.DateTo, I.CHFID, I.OtherNames,
	    I.LastName, C.HFID, HF.HFCode, HF.HFName, HF.AccCode, Prod.ProdID, Prod.ProductCode, Prod.ProductName, 
	    C.Claimed PriceAsked, SUM(CDetails.PriceApproved)PriceApproved, SUM(CDetails.PriceAdjusted)PriceAdjusted, SUM(CDetails.RemuneratedAmount)RemuneratedAmount,
	    D.DistrictID, D.DistrictName, R.RegionId, R.RegionName

	    FROM tblClaim C
	    LEFT OUTER JOIN tblClaimAdmin CA ON CA.ClaimAdminId = C.ClaimAdminId
	    INNER JOIN tblInsuree I ON I.InsureeId = C.InsureeID
	    INNER JOIN tblHF HF ON HF.HFID = C.HFID
	    INNER JOIN CDetails ON CDetails.ClaimId = C.ClaimID
	    INNER JOIN tblProduct Prod ON Prod.ProdId = CDetails.ProdID
	    INNER JOIN tblFamilies F ON F.FamilyId = I.FamilyID
	    INNER JOIN tblVillages V ON V.VillageID = F.LocationId
	    INNER JOIN tblWards W ON W.WardId = V.WardId
	    INNER JOIN tblDistricts D ON D.DistrictID = W.DistrictId
	    INNER JOIN tblRegions R ON R.RegionId = D.Region

	    WHERE C.ValidityTo IS NULL
	    AND (Prod.LocationId = @LocationId OR @LocationId = 0 OR Prod.LocationId IS NULL)
	    AND(Prod.ProdId = @ProdId OR @ProdId = 0)
	    AND (C.RunId = @RunId OR @RunId = 0)
	    AND (HF.HFId = @HFID OR @HFId = 0)
	    AND (HF.HFLevel = @HFLevel OR @HFLevel = N'')
	    AND (C.DateTo BETWEEN @DateFrom AND @DateTo)

	    GROUP BY C.ClaimCode, C.DateClaimed, CA.OtherNames, CA.LastName , C.DateFrom, C.DateTo, I.CHFID, I.OtherNames,
	    I.LastName, C.HFID, HF.HFCode, HF.HFName, HF.AccCode, Prod.ProdID, Prod.ProductCode, Prod.ProductName, C.Claimed,
	    D.DistrictId, D.DistrictName, R.RegionId, R.RegionName"

        With data
            .setSQLCommand(sSQL, CommandType.Text)

            .params("@LocationId", SqlDbType.Int, LocationId)
            .params("@ProdId", SqlDbType.Int, ProductId)
            .params("@RunID", SqlDbType.Int, RunID)
            .params("@HFID", SqlDbType.Int, HFID)
            .params("@HFLevel", SqlDbType.Char, 1, HFLevel)
            .params("@DateFrom", SqlDbType.Date, DateFrom)
            .params("@DateTo", SqlDbType.Date, DateTo)
            '.params("@MinRemunerated", SqlDbType.Float, MinRemunerated)

            Return .Filldata
        End With

    End Function

    'Corrected
    Public Function GetEnroledFamilies(ByVal LocationId As Integer?, ByVal StartDate As Date, ByVal EndDate As Date, ByVal PolicyStatus As Integer?, ByVal dtPolicyStatus As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ";WITH MainDetails AS
	(
		SELECT F.FamilyID, F.LocationId,R.RegionName, D.DistrictName,W.WardName,V.VillageName,I.IsHead,I.CHFID, I.LastName, I.OtherNames, CONVERT(DATE,I.ValidityFrom) EnrolDate
		FROM tblFamilies F 
		INNER JOIN tblInsuree I ON F.FamilyID = I.FamilyID
		INNER JOIN tblVillages V ON V.VillageId = F.LocationId
		INNER JOIN tblWards W ON W.WardId = V.WardId
		INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
		INNER JOIN tblRegions R ON R.RegionId = D.Region
		WHERE F.ValidityTo IS NULL
		AND I.ValidityTo IS NULL
		AND R.ValidityTo IS NULL
		AND D.ValidityTo IS  NULL
		AND W.ValidityTo IS NULL
		AND V.ValidityTo IS NULL
		AND CAST(I.ValidityFrom AS DATE) BETWEEN @StartDate AND @EndDate
		
	),Locations AS(
		SELECT LocationId, ParentLocationId FROM tblLocations WHERE ValidityTo IS NULL AND (LocationId = @LocationId OR CASE WHEN @LocationId IS NULL THEN ISNULL(ParentLocationId, 0) ELSE 0 END = ISNULL(@LocationId, 0))
		UNION ALL
		SELECT L.LocationId, L.ParentLocationId
		FROM tblLocations L 
		INNER JOIN Locations ON Locations.LocationId = L.ParentLocationId
		WHERE L.ValidityTo IS NULL
	),Policies AS
	(
		SELECT ROW_NUMBER() OVER(PARTITION BY PL.FamilyId ORDER BY PL.FamilyId, PL.PolicyStatus)RNo,PL.FamilyId,PL.PolicyStatus
		FROM tblPolicy PL
		WHERE PL.ValidityTo IS NULL
		--AND (PL.PolicyStatus = @PolicyStatus OR @PolicyStatus IS NULL)
		GROUP BY PL.FamilyId, PL.PolicyStatus
	) 
	SELECT MainDetails.*, Policies.PolicyStatus, 
	--CASE Policies.PolicyStatus WHEN 1 THEN N'Idle' WHEN 2 THEN N'Active' WHEN 4 THEN N'Suspended' WHEN 8 THEN N'Expired' ELSE N'No Policy' END 
	PS.Name PolicyStatusDesc
	FROM  MainDetails 
	INNER JOIN Locations ON Locations.LocationId = MainDetails.LocationId
	LEFT OUTER JOIN Policies ON MainDetails.FamilyID = Policies.FamilyID
	LEFT OUTER JOIN @dtPolicyStatus PS ON PS.ID = ISNULL(Policies.PolicyStatus, 0)
	WHERE (Policies.RNo = 1 OR Policies.PolicyStatus IS NULL) 
	AND (Policies.PolicyStatus = @PolicyStatus OR @PolicyStatus IS NULL)
	ORDER BY MainDetails.LocationId;"
        ' Dim sSQL As String = "uspSSRSEnroledFamilies"
        data.setSQLCommand(sSQL, CommandType.Text, timeout:=0)

        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@StartDate", SqlDbType.Date, StartDate)
        data.params("@EndDate", SqlDbType.Date, EndDate)
        data.params("@PolicyStatus", SqlDbType.Int, PolicyStatus)
        data.params("@dtPolicyStatus", dtPolicyStatus, "xAttribute")
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetPendingInsurees(ByVal LocationId As Integer, ByVal OfficerId As Integer?, ByVal StartDate As Date, ByVal EndDate As Date)
        Dim data As New ExactSQL
        Dim sSQL As String = ""

        sSQL = ";WITH PendingInsurees AS"
        sSQL += " ("
        sSQL += " SELECT O.OfficerID,O.Code, O.OtherNames, O.LastName,P.CHFID, MAX(P.PhotoDate)PhotoDate,"
        sSQL += " ROW_NUMBER() OVER(PARTITION BY P.CHFID ORDER BY O.OfficerId) RNo,"
        sSQL += " IIF(CAST(O.WorksTo AS DATE) <= CAST(GETDATE() AS DATE), 'N', 'A')OfficerStatus"
        sSQL += " FROM tblSubmittedPhotos P"
        sSQL += " LEFT OUTER JOIN tblInsuree I ON P.CHFID = I.CHFID"
        sSQL += " INNER JOIN tblOfficer O ON P.OfficerCode = O.Code"
        sSQL += " INNER JOIN tblDistricts L ON L.DistrictId = O.Locationid OR L.Region = O.LocationId"
        sSQL += " WHERE I.ValidityTo Is NULL"
        sSQL += " AND I.InsureeID IS NULL"
        sSQL += " AND O.ValidityTo IS NULL"
        sSQL += " AND (L.Region = @LocationId OR L.DistrictId = @LocationId OR @LocationId =0)"
        sSQL += " AND (O.OfficerID = @OfficerId OR @OfficerId = 0)"
        sSQL += " AND P.PhotoDate BETWEEN @StartDate AND @EndDate"
        sSQL += " GROUP BY O.OfficerID,O.Code, O.OtherNames, O.LastName,P.CHFID, O.WorksTo"
        sSQL += " )"
        sSQL += " SELECT OfficerId, Code, OtherNames, LastName, CHFID, PhotoDate,IIF(RNo = 1, '', 'Duplicated')Duplicated, CASE WHEN RNo=1 THEN 1 ELSE 0 END RNo, OfficerStatus"
        sSQL += " FROM PendingInsurees"


        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@OfficerId", SqlDbType.Int, OfficerId)
        data.params("@StartDate", SqlDbType.Date, StartDate)
        data.params("@EndDate", SqlDbType.Date, EndDate)

        Return data.Filldata

    End Function

    'Corrected
    Public Function GetRenewals(LocationId As Integer, ProductId As Integer, OfficerId As Integer?, FromDate As Date, ToDate As Date, Sort As String) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "SELECT O.Code OfficerCode, CONCAT(O.OtherNames,  ' ', O.LastName)OfficerName, W.WardName, V.VillageName, I.CHFID,"
        sSQL += " CONCAT(I.OtherNames,' ', O.LastName)InsureeName, PL.EnrollDate, PR.Receipt, PR.Amount, Pay.PayerName, D.Region RegionID,D.DistrictId DistrictID,CONCAT(R.RegionCode,'-', R.RegionName) RegionName, CONCAT(D.DistrictCode,'-',D.DistrictName) DistrictName "
        sSQL += " FROM tblPolicy PL"
        sSQL += " INNER JOIN tblOfficer O ON PL.OfficerId = O.OfficerID"
        sSQL += " INNER JOIN tblFamilies F ON PL.FamilyId = F.FamilyId"
        sSQL += " INNER JOIN tblVillages V ON V.VillageId = F.LocationId"
        sSQL += " INNER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId"
        sSQL += " LEFT OUTER JOIN tblRegions R ON R.RegionId = D.Region"
        sSQL += " INNER JOIN tblInsuree I ON F.InsureeId = I.InsureeId"
        sSQL += " INNER JOIN tblPremium PR ON PL.PolicyId = PR.PolicyID"
        sSQL += " LEFT OUTER JOIN tblPayer Pay ON PR.PayerId = Pay.PayerID"
        sSQL += " WHERE PL.ValidityTo IS NULL"
        sSQL += " AND F.ValidityTo IS NULL"
        sSQL += " AND I.ValidityTo IS NULL"
        sSQL += " AND O.ValidityTo IS NULL"
        sSQL += " AND PR.ValidityTo IS NULL"
        sSQL += " AND PolicyStage = N'R'"
        sSQL += " AND (D.Region = @LocationId OR D.DistrictID = @LocationId OR @LocationId = 0)"
        sSQL += " AND (PL.ProdID = @ProdId OR @ProdId = 0)"
        sSQL += " AND (O.OfficerId = @OfficerID  OR @OFficerID IS NULL)"
        sSQL += " AND (PL.EnrollDate BETWEEN @FromDate AND @ToDate)"

        sSQL += " GROUP BY O.Code , CONCAT(O.OtherNames,  ' ', O.LastName), W.WardName, V.VillageName, I.CHFID, CONCAT(I.OtherNames,' ', O.LastName), PL.EnrollDate, PR.Receipt, PR.Amount, Pay.PayerName,D.Region,D.DistrictId,CONCAT(R.RegionCode,'-', R.RegionName) , CONCAT(D.DistrictCode,'-',D.DistrictName) "
        sSQL += " ORDER BY CASE @Sort WHEN 'D' THEN CONVERT(VARCHAR, PL.EnrollDate, 103) WHEN 'R' THEN PR.Receipt WHEN 'O' THEN O.Code END"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@ProdId", SqlDbType.Int, ProductId)
        data.params("@OfficerId", SqlDbType.Int, OfficerId)
        data.params("@FromDate", SqlDbType.Date, FromDate)
        data.params("@ToDate", SqlDbType.Date, ToDate)
        data.params("@Sort", SqlDbType.Char, 1, Sort)

        Return data.Filldata

    End Function

    Public Function getCatchmentArea(RegionId As Integer, DistrictId As Integer, ByVal ProductId As Integer, ByVal Year As Integer, ByVal Month As Integer, ByVal dt As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = "uspSSRSCapitationPayment"
        Dim sSQLfull As String = "DECLARE @Level1 CHAR(1) = NULL,
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

"
        'data.setSQLCommand(sSQLfull, CommandType.Text, timeout:=0)
        data.setSQLCommand(sSQL, CommandType.StoredProcedure, timeout:=0)
        data.params("@RegionId", SqlDbType.Int, If(RegionId = -1, DBNull.Value, RegionId))
        data.params("@DistrictId", SqlDbType.Int, If(DistrictId = 0, DBNull.Value, DistrictId)) ' DistrictId)
        data.params("@ProdId", SqlDbType.Int, ProductId)
        data.params("@Year", SqlDbType.Int, Year)
        data.params("@Month", SqlDbType.Int, Month)
        data.params("@HFLevel", dt, "xAttributeV")
        Return data.Filldata
    End Function
    Public Function GetPaymentContribution(startDate As Date?, endDate As Date?, controlNumber As String, productCode As String, PaymentStatus As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        Dim myDate = New DateTime()

        sSQL = " SELECT PY.TypeOfPayment,CASE row_number() OVER (Partition BY PY.PaymentID ORDER BY PY.PaymentID) WHEN 1 THEN PY.TransferFee ELSE NULL END TransferFee, PD.Amount MatchedAmount, PY.PaymentID, CN.ControlNumber, PY.TransactionNo,PY.ReceiptNo,PY.MatchedDate MatchingDate, CASE row_number() OVER (Partition BY PY.PaymentID ORDER BY PY.PaymentID) WHEN 1 THEN PY.ReceivedAmount ELSE NULL END ReceivedAmount, PY.PaymentDate,PY.ReceivedDate, PY.PaymentOrigin, PY.OfficerCode "
        sSQL += " ,CASE WHEN Paymentstatus = 5 THEN PR.Receipt ELSE NULL END Receipt,CASE WHEN Paymentstatus = 5 THEN PD.InsuranceNumber ELSE NULL END InsuranceNumber,CASE WHEN Paymentstatus = 5 THEN PD.ProductCode ELSE NULL END ProductCode"
        sSQL += " FROM tblPayment PY"
        sSQL += " INNER JOIN tblPaymentDetails PD ON PD.PaymentID = PY.PaymentID AND PD.ValidityTo IS NULL"
        sSQL += " INNER JOIN tblControlNumber CN ON CN.PaymentID = PY.PaymentID AND CN.ValidityTo IS NULL"
        sSQL += " LEFT JOIN tblpremium PR ON PD.PremiumID = PR.PremiumID AND PR.ValidityTo IS NULL"


        sSQL += " WHERE PY.PaymentDate IS NOT NULL"
        sSQL += " AND PY.ValidityTo IS NULL"
        sSQL += " AND (PD.ProductCode = @ProductCode OR @ProductCode IS NULL)"
        sSQL += " AND ((PY.PaymentDate >= @FromDate OR @FromDate IS NULL) AND (PY.PaymentDate <= @ToDate OR @ToDate IS NULL))"

        If PaymentStatus > 0 Then
            If PaymentStatus = 1 Then
                sSQL += " AND PY.PaymentStatus < 5"
            Else
                sSQL += " AND PY.PaymentStatus = @PaymentStatus"
            End If

        End If


        If controlNumber <> "" Then
            sSQL += " AND CN.ControlNumber LIKE @ControlNumber"
        End If


        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProductCode", SqlDbType.NVarChar, 8, productCode)
        data.params("@FromDate", SqlDbType.Date, startDate)
        data.params("@ToDate", SqlDbType.Date, endDate)
        data.params("@PaymentStatus", SqlDbType.Int, PaymentStatus)
        data.params("@ControlNumber", SqlDbType.NVarChar, 50, controlNumber + "%")

        Return data.Filldata
    End Function
    Public Function GetControlNumberAssignment(startDate As Date, endDate As Date, PostingStatus As String, AssignmentStatus As String, RegionId As Integer, DistrictId As Integer, dtpaymentStatus As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = " SELECT CONVERT(NVARCHAR(12), CN.RequestedDate) RequestedDate,CASE PD.PolicyStage WHEN 'N' THEN 'Yes' ELSE 'No' END AS PolicyStage, PY.OfficerCode,O.LastName, PY.PhoneNumber,  "
        sSQL += " CASE PY.PaymentStatus WHEN 1 THEN 'Not yet Confirmed' WHEN 2 THEN 'Posted' WHEN 3 THEN 'Posted' WHEN 4 THEN  'Posted' WHEN 5 THEN 'Posted' WHEN -1  THEN 'Rejected'  WHEN -2 THEN 'Rejected' WHEN -3 THEN 'Rejected' END PostingStatus,   "
        sSQL += " PY.RejectedReason PostingRejectedReason, CASE PY.PaymentStatus WHEN 3 THEN 'Assigned' WHEN 4 THEN 'Assigned' WHEN 5 THEN 'Assigned' WHEN 1 THEN 'Not yet assigned' WHEN 2 THEN 'Not yet assigned' WHEN -1  THEN 'Rejected'  WHEN -2 THEN 'Rejected' WHEN -3  THEN 'Rejected' END AssigmentStatus,"
        sSQL += " PY.ExpectedAmount,PY.TransferFee,PY.TypeOfPayment, "
        sSQL += " CN.Comment CAssignmentRejectedReason, cn.ControlNumber,NULL PaymenyStatusName FROM tblControlNumber CN "
        sSQL += " INNER JOIN tblPayment PY ON PY.PaymentID = CN.PaymentID"
        sSQL += "  LEFT OUTER JOIN tblPaymentDetails PD ON PD.PaymentID = PY.PaymentID"
        sSQL += " LEFT OUTER JOIN tblOfficer O ON O.Code = PY.OfficerCode"
        sSQL += " LEFT OUTER JOIN tblLocations D ON D.LocationId = O.LocationId"
        sSQL += " LEFT OUTER JOIN tblLocations R ON R.LocationId = D.ParentLocationId"
        sSQL += " LEFT OUTER JOIN @dtpaymentStatus PS ON PS.StatusID= PY.PaymentStatus"
        sSQL += " WHERE"
        sSQL += " PY.ValidityTo IS NULL"
        sSQL += " AND CN.ValidityTo IS NULL"
        sSQL += " AND O.ValidityTo IS NULL"
        sSQL += " AND D.ValidityTo IS NULL"
        sSQL += " AND R.ValidityTo IS NULL"
        sSQL += " AND CONVERT(DATE,CN.RequestedDate) >= @FromDate AND CONVERT(DATE,CN.RequestedDate) <= @ToDate "
        If AssignmentStatus = "Assigned" Then
            sSQL += " AND  PY.PaymentStatus >=3"
        End If
        'Rejected Assignment Status is -3
        If AssignmentStatus = "Rejected" Then
            sSQL += " AND  PY.PaymentStatus= -3"
        End If
        If AssignmentStatus = "Not yet assigned" Then
            sSQL += " AND PY.PaymentStatus <= 2 AND PY.PaymentStatus > 0"
        End If
        If RegionId <> Nothing Or DistrictId <> Nothing Then
            sSQL += " AND D.LocationId = @DistrictId"
            sSQL += " AND R.LocationId = @RegionId"
        End If
        If PostingStatus = "Posted" Then
            sSQL += " AND  PY.PaymentStatus >= 2"
        End If
        'Rejected posting status are -1,-2,-3
        If PostingStatus = "Rejected" Then
            sSQL += " AND  PY.PaymentStatus = -1 OR  PY.PaymentStatus = -2"
        End If
        If PostingStatus = "Not yet confirmed" Then
            sSQL += " AND  PY.PaymentStatus =1 "
        End If
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@FromDate", SqlDbType.Date, startDate)
        data.params("@ToDate", SqlDbType.Date, endDate)
        data.params("@RegionId", SqlDbType.Int, RegionId)
        data.params("@DistrictId", SqlDbType.Int, DistrictId)
        data.params("@dtpaymentStatus", dtpaymentStatus, "xPayementStatus")

        Return data.Filldata
    End Function
    Public Function GetOverviewOfCommissions(ByVal LocationId As Integer?, ByVal ProductId As Integer?, ByVal Month As Integer?, ByVal Year As Integer?, ByVal PayerId As Integer?, ByVal OfficerId As Integer?, ByVal Mode As Integer, ByVal CommissionRate As Decimal?, ByVal ReportingID As Integer?, ByRef ErrorMessage As String, ByRef oReturn As Integer) As DataTable

        Dim Data As New ExactSQL
        'Dim sSQL As String = "uspSSRSOverviewOfCommissions"
        Dim sSQL As String = " IF @ReportingId IS NULL

        BEGIN
			DECLARE @RecordFound INT = 0
			DECLARE @Rate DECIMAL(18,2)
          
		
			IF @CommissionRate IS NOT NULL
				BEGIN
					 SET @Rate = @CommissionRate / 100
				END
		ELSE

	  		DECLARE @FirstDay DATE = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01'; 
			DECLARE @LastDay DATE = EOMONTH(CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01', 0)
			
	
	
		
			BEGIN TRY
					BEGIN TRAN

			  
				
					INSERT INTO tblReporting(ReportingDate,LocationId, ProdId, PayerId, StartDate, EndDate, RecordFound,OfficerID,ReportType,CommissionRate,ReportMode)
			
					SELECT GETDATE(),@LocationId,ISNULL(@ProdId,0), @PayerId, @FirstDay, @LastDay, 0,@OfficerId,2,@Rate,@Mode; 
					--Get the last inserted reporting Id
					SELECT @ReportingId =  SCOPE_IDENTITY();
				
					IF @Mode = 1 
						UPDATE tblPremium SET ReportingCommissionID = @ReportingId
						WHERE PremiumId IN (
							SELECT  Pr.PremiumId
					FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID AND (PL.PolicyStatus=1 OR PL.PolicyStatus=2)
					LEFT JOIN tblPaymentDetails PD ON PD.PremiumID = Pr.PremiumId
					LEFT JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID 
					INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
					INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
					INNER JOIN tblVillages V ON V.VillageId = F.LocationId
					INNER JOIN tblWards W ON W.WardId = V.WardId
					INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
					INNER JOIN tblOfficer O ON O.LocationId = D.DistrictId
					INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
					LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
					WHERE PD.Amount >=  0
					AND Year(Pr.PayDate) = @Year AND Month(Pr.paydate) = @Month
					AND D.DistrictId = @LocationId					
					AND (ISNULL(Prod.ProdID,0) = ISNULL(@ProdId,0) OR @ProdId is null)
					AND (ISNULL(O.OfficerID,0) = ISNULL(@OfficerId,0) OR @OfficerId IS NULL)
					AND (ISNULL(Payer.PayerID,0) = ISNULL(@PayerId,0) OR @PayerId IS NULL)
					AND Pr.ReportingId IS NULL
					AND PR.PayType <> N'F'
					AND Pr.ReportingCommissionID IS NULL
					
					GROUP BY Pr.PremiumId
						)
					ELSE --@Mode = 0
						UPDATE tblPremium SET ReportingCommissionID = @ReportingId
						WHERE PremiumId IN (
							SELECT  Pr.PremiumId
								FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID AND (PL.PolicyStatus=1 OR PL.PolicyStatus=2)
								INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID AND Prod.ValidityTo IS NULL
								INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID AND F.ValidityTo IS NULL
								INNER JOIN tblVillages V ON V.VillageId = F.LocationId AND V.ValidityTo IS NULL
								INNER JOIN tblWards W ON W.WardId = V.WardId AND W.ValidityTo IS NULL
								INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
								INNER JOIN tblOfficer O ON O.Officerid = PL.OfficerID AND  O.LocationId = D.DistrictId AND O.ValidityTo IS NULL
								INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
								LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
								WHERE 
								 Year(Pr.PayDate) = @Year AND Month(Pr.paydate) = @Month
								AND D.DistrictId = @LocationId					
								AND (ISNULL(Prod.ProdID,0) = ISNULL(@ProdId,0) OR @ProdId is null)
								AND (ISNULL(O.OfficerID,0) = ISNULL(@OfficerId,0) OR @OfficerId IS NULL)
								AND (ISNULL(Payer.PayerID,0) = ISNULL(@PayerId,0) OR @PayerId IS NULL)
								AND Pr.ReportingId IS NULL
								AND PR.PayType <> N'F'
								AND Pr.ReportingCommissionID IS NULL					
								GROUP BY Pr.PremiumId
						)
					SELECT @RecordFound = @@ROWCOUNT;
					IF @RecordFound = 0 
						BEGIN
							SELECT @ErrorMessage = 'No Data'
							ROLLBACK;
							 
						END
					UPDATE tblReporting SET RecordFound = @RecordFound WHERE ReportingId = @ReportingId;

				COMMIT TRAN;
			END TRY
			BEGIN CATCH
				--SELECT @ErrorMessage = ERROR_MESSAGE(); ERROR MESSAGE WAS COMMENTED BY SALUMU ON 12-11-2019
				ROLLBACK;
				--RETURN -2 RETURN WAS COMMENTED BY SALUMU ON 12-11-2019
			END CATCH
		  END
	      
					    
				 
		SELECT  Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName Product,PL.PolicyID,F.FamilyID,D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName,O.Code + ' ' + O.LastName Officer,
		Ins.DOB, Ins.IsHead, PL.EnrollDate,REP.ReportMode,Month(REP.StartDate)  [Month], Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN ISNULL(Pr.Amount,0) ELSE NULL END Amount,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE NULL END  PrescribedContribution, CASE WHEN Ins.IsHead = 1 THEN ISNULL(PD.Amount,0) ELSE NULL END ActualPayment, Payer.PayerName,PY.PaymentDate,CASE WHEN IsHead = 1 THEN SUM(ISNULL(Pr.Amount,0.00)) * ISNULL(rep.CommissionRate,0.00) ELSE NULL END  CommissionRate,PY.ExpectedAmount PaymentAmount,OfficerCode,VillageName,WardName,PL.PolicyStage,TransactionNo
		FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID AND (PL.PolicyStatus=1 OR PL.PolicyStatus=2) AND PL.ValidityTo IS NULL
		LEFT JOIN tblPaymentDetails PD ON PD.PremiumID = Pr.PremiumId AND PD.ValidityTo IS NULl AND PR.ValidityTo IS NULL
		LEFT JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID AND PY.ValidityTo IS NULL
		INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID AND Prod.ValidityTo IS NULL
		INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID AND F.ValidityTo IS NULL
		INNER JOIN tblVillages V ON V.VillageId = F.LocationId AND V.ValidityTo IS NULL
		INNER JOIN tblWards W ON W.WardId = V.WardId AND W.ValidityTo IS NULL
		INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
		INNER JOIN tblOfficer O ON O.Officerid = PL.OfficerID AND  O.LocationId = D.DistrictId AND O.ValidityTo IS NULL
		INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
		INNER JOIN tblReporting REP ON REP.ReportingId = @ReportingId
		LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID
		WHERE Pr.ReportingCommissionID = @ReportingId
		
		GROUP BY Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName , PL.PolicyID ,  F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames ,O.Code + ' ' + O.LastName ,
		Ins.DOB, Ins.IsHead, PL.EnrollDate,REP.ReportMode,Month(REP.StartDate), Pr.Paydate, Pr.Receipt,Pr.Amount,Pr.Amount, PD.Amount , Payer.PayerName,PY.PaymentDate, PY.ExpectedAmount,OfficerCode,VillageName,WardName,PL.PolicyStage,TransactionNo,CommissionRate
		ORDER BY PremiumId, O.OfficerID,F.FamilyID,IsHead DESC;"
        Data.setSQLCommand(sSQL, CommandType.Text)

        Data.params("@Month", SqlDbType.Int, Month)
        Data.params("@Year", SqlDbType.Int, Year)
        Data.params("@Mode", SqlDbType.Int, Mode)
        Data.params("@OfficerId", SqlDbType.Int, OfficerId)
        Data.params("@LocationId", SqlDbType.Int, LocationId)
        Data.params("@ProdId", SqlDbType.Int, ProductId)
        Data.params("@PayerId", SqlDbType.Int, PayerId)
        Data.params("@ReportingId", SqlDbType.Int, ReportingID)
        Data.params("@CommissionRate", SqlDbType.Decimal, CommissionRate)
        Data.params("@ErrorMessage", SqlDbType.NVarChar, 200, "", ParameterDirection.Output)
        Data.params("@RV", SqlDbType.Int, 0, ParameterDirection.ReturnValue)
        Dim dt As DataTable = Data.Filldata()
        oReturn = Data.sqlParameters("@RV")
        ErrorMessage = Data.sqlParameters("@ErrorMessage").ToString
        Return dt

    End Function
    Public Function GetClaimHistoryReport(ByVal LocationId As Integer?, ByVal ProdID As Integer?, ByVal HfID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ClaimStatus As Integer?, ByVal InsuranceNumber As String, ByVal Scope As Integer, ByVal dtRejReasons As DataTable, ByRef oReturn As Integer) As DataTable
        Dim Data As New ExactSQL
        Dim sSQL As String = ""
        If Scope = 2 Or Scope = -1 Then

            sSQL = "
     /*DECLARE @HFID INT,
	    @LocationId INT ,
	    @ProdId INT, 
	    @StartDate DATE, 
	    @EndDate DATE,
	    @ClaimStatus INT = NULL,
	    @InsuranceNumber NVARCHAR(12),
	    @Scope INT= NULL
     */
    --@ClaimRejReason xClaimRejReasons READONLY

        /*
		RESPONSE CODES
			1 - Insurance number not found
			0 - Success 
			-1 Unknown Error
		*/
		

       -- Check Insurance number if exsists

    --IF NOT EXISTS(SELECT 1 FROM tblInsuree WHERE CHFID=@InsuranceNumber AND ValidityTo IS NULL) 
		-- RETURN 1 

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

	    SELECT  HF.HFCode+' ' + HF.HFName HFCodeName, L.ParentLocationId AS RegionId,l.LocationId as DistrictID, R.RegionName,D.DistrictName,  C.DateClaimed,PROD.ProductCode +' ' + PROD.ProductName Product, C.ClaimID, I.ItemId, S.ServiceID, HF.HFCode, HF.HFName, C.ClaimCode, C.DateClaimed, CA.LastName + ' ' + CA.OtherNames ClaimAdminName,
			    C.DateFrom, C.DateTo, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsureeName,Ins.DOB DateOfBirth,
	    CASE C.ClaimStatus WHEN 1 THEN N'Rejected' WHEN 2 THEN N'Entered' WHEN 4 THEN N'Checked' WHEN 8 THEN N'Processed' WHEN 16 THEN N'Valuated' END ClaimStatus,
	    C.RejectionReason, COALESCE(TFI.Claimed + TFS.Claimed, TFI.Claimed, TFS.Claimed) Claimed, 
	    COALESCE(TFI.Approved + TFS.Approved, TFI.Approved, TFS.Approved) Approved,
	    COALESCE(TFI.Adjusted + TFS.Adjusted, TFI.Adjusted, TFS.Adjusted) Adjusted,
	    COALESCE(TFI.Remunerated + TFS.Remunerated, TFI.Remunerated, TFS.Remunerated)Paid,
	    CASE WHEN CI.RejectionReason <> 0 THEN I.ItemCode ELSE NULL END RejectedItem, CI.RejectionReason ItemRejectionCode,
	    CASE WHEN CS.RejectionReason > 0 THEN S.ServCode ELSE NULL END RejectedService, CS.RejectionReason ServiceRejectionCode,
	    CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN I.ItemCode+' '+I.ItemName ELSE NULL END AdjustedItem,
	    CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN CI.QtyProvided ELSE NULL END OrgQtyItem,
	    CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved ,CI.QtyProvided)  THEN CI.QtyApproved ELSE NULL END AdjQtyItem,
	    CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)  THEN S.ServCode+'		'+S.ServName ELSE NULL END AdjustedService,
	    CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)   THEN CS.QtyProvided ELSE NULL END OrgQtyService,
	    CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved ,CS.QtyProvided)   THEN CS.QtyApproved ELSE NULL END AdjQtyService,
	    C.Explanation
	    ,CS.QtyApproved ServiceQtyApproved, CI.QtyApproved ItemQtyApproved,cs.PriceAsked ServicePrice, CI.PriceAsked ItemPrice
	    ,cs.PriceApproved ServiceAppPrice,ci.PriceApproved ItemAppPrice, cs.Justification ServiceJustification,
	    CI.Justification ItemJustification,cs.ClaimServiceID,CI.ClaimItemID,
	     XCS.Name ServiceRejectionReason, XCI.Name ItemRejectionReason,CS.RejectionReason [Services] ,ci.RejectionReason Items

	    FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
	    LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
	    LEFT OUTER JOIN tblProduct PROD ON PROD.ProdID =@ProdId
	    LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID
	    LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID
	    INNER JOIN tblHF HF ON C.HFID = HF.HfID
	    INNER JOIN tblLocations L ON L.LocationId = HF.LocationId
	    INNER JOIN tblRegions R ON R.RegionId = L.ParentLocationId
	    INNER JOIN tblDistricts D ON D.DistrictId = L.LocationId
	    LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
	    INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
	    LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
	    LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId
	    LEFT OUTER JOIN @ClaimRejReason XCI ON XCI.ID = CI.RejectionReason
	    LEFT OUTER JOIN @ClaimRejReason XCS ON XCS.ID = CS.RejectionReason
	    WHERE C.ValidityTo IS NULL
	    AND ISNULL(C.DateTo,C.DateFrom) BETWEEN @StartDate AND @EndDate
	    AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
	    AND (HF.LocationId = @LocationId OR @LocationId = 0)
	    AND (Ins.CHFID = @InsuranceNumber)
	    AND (HF.HFID = @HFID OR @HFID = 0)
	    AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) "
            Data.setSQLCommand(sSQL, CommandType.Text)
        ElseIf Scope = 0 Then
            sSQL = ";WITH TotalForItems AS
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

			SELECT --R.RegionId,R.RegionName,D.DistrictId,D.DistrictName,
			HF.HFCode+' ' + HF.HFName HFCodeName, L.ParentLocationId AS RegionId,l.LocationId as DistrictID, R.RegionName,D.DistrictName, C.DateClaimed,PROD.ProductCode +' ' + PROD.ProductName Product, C.ClaimID, I.ItemId, S.ServiceID, HF.HFCode, HF.HFName, C.ClaimCode, C.DateClaimed, CA.LastName + ' ' + CA.OtherNames ClaimAdminName,
			C.DateFrom, C.DateTo, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsureeName,Ins.DOB DateOfBirth,
			CASE C.ClaimStatus WHEN 1 THEN N'Rejected' WHEN 2 THEN N'Entered' WHEN 4 THEN N'Checked' WHEN 8 THEN N'Processed' WHEN 16 THEN N'Valuated' END ClaimStatus,
			COALESCE(TFI.Claimed + TFS.Claimed, TFI.Claimed, TFS.Claimed) Claimed, 
			COALESCE(TFI.Approved + TFS.Approved, TFI.Approved, TFS.Approved) Approved,
			COALESCE(TFI.Adjusted + TFS.Adjusted, TFI.Adjusted, TFS.Adjusted) Adjusted,
			COALESCE(TFI.Remunerated + TFS.Remunerated, TFI.Remunerated, TFS.Remunerated)Paid

			FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
			LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID
			LEFT OUTER JOIN tblProduct PROD ON PROD.ProdID =@ProdId
			LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID
			LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID
			INNER JOIN tblHF HF ON C.HFID = HF.HfID
			INNER JOIN tblLocations L ON L.LocationId = HF.LocationId
			INNER JOIN tblRegions R ON R.RegionId = L.ParentLocationId
	        INNER JOIN tblDistricts D ON D.DistrictId = L.LocationId
			LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
			INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
			LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
			LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId
			WHERE C.ValidityTo IS NULL
			AND ISNULL(C.DateTo,C.DateFrom) BETWEEN @StartDate AND @EndDate
			AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
			AND (HF.LocationId = @LocationId OR @LocationId = 0)
			AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) 
			AND (Ins.CHFID = @InsuranceNumber)
			AND (HF.HFID = @HFID OR @HFID = 0)"
            Data.setSQLCommand(sSQL, CommandType.Text)
        Else
            sSQL = ";WITH TotalForItems AS
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

		SELECT  HF.HFCode+' ' + HF.HFName HFCodeName, L.ParentLocationId AS RegionId,l.LocationId as DistrictID, R.RegionName,D.DistrictName, C.DateClaimed,PROD.ProductCode +' ' + PROD.ProductName Product, C.ClaimID, I.ItemId, S.ServiceID, HF.HFCode, HF.HFName, C.ClaimCode, C.DateClaimed, CA.LastName + ' ' + CA.OtherNames ClaimAdminName,
			C.DateFrom, C.DateTo, Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsureeName,Ins.DOB DateOfBirth,
		CASE C.ClaimStatus WHEN 1 THEN N'Rejected' WHEN 2 THEN N'Entered' WHEN 4 THEN N'Checked' WHEN 8 THEN N'Processed' WHEN 16 THEN N'Valuated' END ClaimStatus,
		C.RejectionReason, COALESCE(TFI.Claimed + TFS.Claimed, TFI.Claimed, TFS.Claimed) Claimed, 
		COALESCE(TFI.Approved + TFS.Approved, TFI.Approved, TFS.Approved) Approved,
		COALESCE(TFI.Adjusted + TFS.Adjusted, TFI.Adjusted, TFS.Adjusted) Adjusted,
		COALESCE(TFI.Remunerated + TFS.Remunerated, TFI.Remunerated, TFS.Remunerated)Paid,
		CASE WHEN CI.RejectionReason <> 0 THEN I.ItemCode ELSE NULL END RejectedItem, CI.RejectionReason ItemRejectionCode,
		CASE WHEN CS.RejectionReason > 0 THEN S.ServCode ELSE NULL END RejectedService, CS.RejectionReason ServiceRejectionCode,
		CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN I.ItemCode+' '+I.ItemName ELSE NULL END AdjustedItem,
		CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved,CI.QtyProvided) THEN CI.QtyProvided ELSE NULL END OrgQtyItem,
		CASE WHEN CI.QtyProvided <> COALESCE(CI.QtyApproved ,CI.QtyProvided)  THEN CI.QtyApproved ELSE NULL END AdjQtyItem,
		CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)  THEN S.ServCode+' '+S.ServName ELSE NULL END AdjustedService,
		CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved,CS.QtyProvided)   THEN CS.QtyProvided ELSE NULL END OrgQtyService,
		CASE WHEN CS.QtyProvided <> COALESCE(CS.QtyApproved ,CS.QtyProvided)   THEN CS.QtyApproved ELSE NULL END AdjQtyService,
		C.Explanation
		,CS.QtyApproved ServiceQtyApproved, CI.QtyApproved ItemQtyApproved,cs.PriceAsked ServicePrice, CI.PriceAsked ItemPrice
		,ISNULL(cs.PriceApproved,0) ServicePriceApproved,ISNULL(ci.PriceApproved,0) ItemPriceApproved, ISNULL(cs.Justification,NULL) ServiceJustification,
		ISNULL(CI.Justification,NULL) ItemJustification,cs.ClaimServiceID,CI.ClaimItemID
		,CONCAT(CS.RejectionReason,' - ', XCS.Name) ServiceRejectionReason,CONCAT(CI.RejectionReason, ' - ', XCI.Name) ItemRejectionReason


		FROM tblClaim C LEFT OUTER JOIN tblClaimItems CI ON C.ClaimId = CI.ClaimID
		LEFT OUTER JOIN tblClaimServices CS ON C.ClaimId = CS.ClaimID 
		LEFT OUTER JOIN tblProduct PROD ON PROD.ProdID =@ProdId
		LEFT OUTER JOIN tblItems I ON CI.ItemId = I.ItemID 
		LEFT OUTER JOIN tblServices S ON CS.ServiceID = S.ServiceID 
		INNER JOIN tblHF HF ON C.HFID = HF.HfID
		INNER JOIN tblLocations L ON L.LocationId = HF.LocationId
		INNER JOIN tblRegions R ON R.RegionId = L.ParentLocationId
	    INNER JOIN tblDistricts D ON D.DistrictId = L.LocationId
		LEFT OUTER JOIN tblClaimAdmin CA ON C.ClaimAdminId = CA.ClaimAdminId
		INNER JOIN tblInsuree Ins ON C.InsureeId = Ins.InsureeId
		LEFT OUTER JOIN TotalForItems TFI ON C.ClaimId = TFI.ClaimID
		LEFT OUTER JOIN TotalForServices TFS ON C.ClaimId = TFS.ClaimId
		INNER JOIN @ClaimRejReason XCI ON XCI.ID = CI.RejectionReason
		INNER JOIN @ClaimRejReason XCS ON XCS.ID = CS.RejectionReason
		WHERE C.ValidityTo IS NULL
		AND CI.RejectionReason > 0
		AND CS.RejectionReason > 0
		AND ISNULL(C.DateFrom,C.DateTo) BETWEEN @StartDate AND @EndDate
		AND (C.ClaimStatus = @ClaimStatus OR @ClaimStatus IS NULL)
		AND (HF.LocationId = @LocationId OR @LocationId = 0)
		AND (Ins.CHFID = @InsuranceNumber)
		AND (HF.HFID = @HFID OR @HFID = 0)
		AND (CI.ProdID = @ProdId OR CS.ProdID = @ProdId  OR COALESCE(CS.ProdID, CI.ProdId) IS NULL OR @ProdId = 0) "
            Data.setSQLCommand(sSQL, CommandType.Text)
        End If
        Data.params("@LocationId", SqlDbType.Int, LocationId)
        Data.params("@ProdID", SqlDbType.Int, ProdID)
        Data.params("@HfID", SqlDbType.Int, HfID)
        Data.params("@StartDate", SqlDbType.Date, StartDate)
        Data.params("@EndDate", SqlDbType.Date, EndDate)
        Data.params("@ClaimStatus", SqlDbType.Int, ClaimStatus)
        Data.params("@InsuranceNumber", SqlDbType.NVarChar, 12, InsuranceNumber)
        Data.params("@Scope", SqlDbType.Int, Scope)
        If Not Scope = 0 Then
            Data.params("@ClaimRejReason", dtRejReasons, "xClaimRejReasons")
        End If
        Data.params("@RV", SqlDbType.Int, 0, ParameterDirection.ReturnValue)
        Dim dt As DataTable = Data.Filldata()
        oReturn = Data.sqlParameters("@RV")
        Return dt
    End Function
End Class
