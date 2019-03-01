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
        Dim sSQL As String = "uspSSRSPremiumCollection"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
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
        Dim sSQL As String = "uspSSRSProductSales"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@Product", SqlDbType.Int, Product)
        data.params("@FromDate", SqlDbType.Date, FromDate)
        data.params("@ToDate", SqlDbType.Date, ToDate)

        Return data.Filldata


    End Function

    'Corrected
    Public Function GetPremiumDistribution(ByVal LocationId As Integer, ByVal Product As Integer, ByVal Month As Integer, ByVal Year As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = "uspSSRSPremiumDistribution"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@ProductID", SqlDbType.Int, Product)
        data.params("@Month", Month)
        data.params("@Year", Year)

        Return data.Filldata

    End Function
    Public Function GetFeedbackPrompt(ByVal SMSStatus As Integer, ByVal LocationId As Integer, ByVal WardId As Integer, ByVal VillageID As Integer, ByVal OfficerID As Integer, ByVal RangeFrom As Date, ByVal RangeTo As Date) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = "uspSSRSFeedbackPrompt"

        With data
            .setSQLCommand(sSQL, CommandType.StoredProcedure)

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
        Dim sSQL As String = "uspSSRSProcessBatch"


        With data
            .setSQLCommand(sSQL, CommandType.StoredProcedure)

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
        Dim sSQL As String = "uspSSRSPrimaryIndicators1"

        Data.setSQLCommand(sSQL, CommandType.StoredProcedure, timeout:=0)

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
        Dim sSQL As String = "uspSSRSPrimaryIndicators2"

        Data.setSQLCommand(sSQL, CommandType.StoredProcedure, , 0)

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
        Dim sSQL As String = "uspSSRSDerivedIndicators1"

        Data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        Data.params("@LocationId", LocationId)
        Data.params("@ProductID", ProductId)
        Data.params("@Month", Month)
        Data.params("@Year", Year)

        Return Data.Filldata

    End Function

    'Corrected
    Public Function GetDerivedIndicators2(ByVal LocationId As Integer, ByVal ProductId As Integer, ByVal HFID As Integer, ByVal Month As Integer, ByVal Year As Integer) As DataTable
        Dim Data As New ExactSQL
        Dim sSQL As String = "uspSSRSDerivedIndicators2"

        Data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        Data.params("@LocationId", LocationId)
        Data.params("@ProductID", ProductId)
        Data.params("@HFID", HFID)
        Data.params("@Month", Month)
        Data.params("@Year", Year)

        Return Data.Filldata

    End Function

    'Corrected
    Public Function GetUserActivityData(ByVal UserID As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal Action As String, ByVal Entity As String) As DataTable
        Dim sSQL As String = "uspSSRSUserLogReport"
        Dim data As New ExactSQL

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        data.params("@UserId", SqlDbType.Int, UserID)
        data.params("@FromDate", SqlDbType.DateTime, StartDate)
        data.params("@ToDate", SqlDbType.DateTime, EndDate)
        data.params("@Action", SqlDbType.NVarChar, 50, Action)
        data.params("@EntityId", SqlDbType.NVarChar, 5, Entity)
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetStatusofRegisters(ByVal LocationId As Integer) As DataTable
        Dim sSQL As String = "uspSSRSStatusRegister"
        Dim data As New ExactSQL

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

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
        Dim sSQL As String = "uspSSRSPaymentCategoryOverview"

        Data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        Data.params("@DateFrom", DateFrom)
        Data.params("@DateTo", DateTo)
        Data.params("@LocationId", LocationId)
        Data.params("@ProductId", ProductId)

        Return Data.Filldata
    End Function

    'Corrected
    Public Function GetMatchingFunds(ByVal LocationId As Integer?, ByVal ProdID As Integer?, ByVal PayerID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ReportingID As Integer?, ByRef ErrorMessage As String, ByRef oReturn As Integer) As DataTable
        Dim Data As New ExactSQL
        Data.setSQLCommand("uspSSRSGetMatchingFunds", CommandType.StoredProcedure)
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
    Public Function GetClaimOverview(ByVal LocationId As Integer?, ByVal ProdID As Integer?, ByVal HfID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ClaimStatus As Integer?) As DataTable
        Dim Data As New ExactSQL
        Data.setSQLCommand("uspSSRSGetClaimOverView", CommandType.StoredProcedure)
        Data.params("@LocationId", SqlDbType.Int, LocationId)
        Data.params("@ProdID", SqlDbType.Int, ProdID)
        Data.params("@HfID", SqlDbType.Int, HfID)
        Data.params("@StartDate", SqlDbType.Date, StartDate)
        Data.params("@EndDate", SqlDbType.Date, EndDate)
        Data.params("@ClaimStatus", SqlDbType.Int, ClaimStatus)
        Return Data.Filldata()
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
        Dim sSQL As String = "uspSSRSProcessBatchWithClaim"


        With data
            .setSQLCommand(sSQL, CommandType.StoredProcedure)

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
        Dim sSQL As String = "uspSSRSEnroledFamilies"
        data.setSQLCommand(sSQL, CommandType.StoredProcedure, timeout:=0)

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
        data.setSQLCommand(sSQL, CommandType.StoredProcedure, timeout:=0)
        data.params("@RegionId", SqlDbType.Int, If(RegionId = -1, DBNull.Value, RegionId))
        data.params("@DistrictId", SqlDbType.Int, If(DistrictId = 0, DBNull.Value, DistrictId)) ' DistrictId)
        data.params("@ProdId", SqlDbType.Int, ProductId)
        data.params("@Year", SqlDbType.Int, Year)
        data.params("@Month", SqlDbType.Int, Month)
        data.params("@HFLevel", dt, "xAttributeV")
        Return data.Filldata
    End Function
    Public Function GetPaymentContribution(startDate As Date, endDate As Date, controlNumber As String, productCode As String, PaymentStatus As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        Dim myDate = New DateTime()

        sSQL = " SELECT * FROM ( SELECT ROW_NUMBER() OVER(PARTITION BY PY.PaymentID ORDER BY PY.PaymentID ASC ) RN, PD.Amount MatchedAmount, PY.PaymentID, CN.ControlNumber, PY.TransactionNo,  PY.MatchedDate MatchingDate, CASE PY.PaymentStatus WHEN 5 THEN PY.ReceiptNo ELSE NULL END AS ReceiptNo, CASE PY.PaymentStatus WHEN 5 THEN PY.ReceivedAmount ELSE NULL END AS ReceivedAmount, PY.PaymentDate,PY.ReceivedDate, PY.PaymentOrigin, PY.OfficerCode FROM tblPayment PY"
        sSQL += " LEFT OUTER JOIN tblPaymentDetails PD ON PD.PaymentID = PY.PaymentID"
        sSQL += " LEFT OUTER JOIN tblControlNumber CN ON CN.PaymentID = PY.PaymentID"

        sSQL += " WHERE"
        sSQL += " PY.ValidityTo IS NULL"
        sSQL += " AND PD.ValidityTo IS NULL"
        sSQL += " AND CN.ValidityTo IS NULL"
        sSQL += " AND (PD.ProductCode = @ProductCode OR @ProductCode IS NULL)"
        sSQL += " AND (PY.PaymentDate BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS DATE))"
        If PaymentStatus >= 1 Then
            If PaymentStatus = 1 Then
                sSQL += " AND PY.PaymentStatus >=4 AND PY.PaymentStatus < 5"
            Else
                sSQL += " AND PY.PaymentStatus = @PaymentStatus"
            End If
        End If

        If controlNumber <> "" Then
            sSQL += " AND CN.ControlNumber LIKE @ControlNumber"
        End If
        sSQL += " ) XX WHERE XX.RN = 1"

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
        sSQL += " CASE PY.PaymentStatus WHEN 1 THEN 'Not yet Confirmed' WHEN 2 THEN 'Posted' WHEN 3 THEN 'Posted' WHEN 4 THEN  'Posted' WHEN 5 THEN 'Posted' WHEN -1  THEN 'Rejected'  WHEN -2 THEN 'Rejected' END PostingStatus,   "
        sSQL += " PY.RejectedReason PostingRejectedReason, CASE PY.PaymentStatus WHEN 3 THEN 'Assigned' WHEN 4 THEN 'Assigned' WHEN 5 THEN 'Assigned' WHEN 1 THEN 'Not yet assigned' WHEN 2 THEN 'Not yet assigned' WHEN -3 THEN 'Rejected' END AssigmentStatus,"
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
            sSQL += " AND  PY.PaymentStatus = -1 OR  PY.PaymentStatus = -2 OR  PY.PaymentStatus = -3"
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
    Public Function GetOverviewOfCommissions(ByVal LocationId As Integer?, ByVal ProductId As Integer?, ByVal Month As Integer, ByVal Year As Integer, ByVal PayerId As Integer?, ByVal OfficerId As Integer?, ByVal Mode As Integer?, ByVal CommissionRate As Decimal?, ByVal ReportingID As Integer?, ByRef ErrorMessage As String, ByRef oReturn As Integer) As DataTable

        Dim Data As New ExactSQL
        Dim sSQL As String = "uspSSRSOverviewOfCommissions"

        Data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        Data.params("@Month", Month)
        Data.params("@Year", Year)
        Data.params("@Mode", SqlDbType.Int, Mode)
        Data.params("@OfficerId", OfficerId)
        Data.params("@LocationId", SqlDbType.Int, LocationId)
        Data.params("@ProdId", SqlDbType.Int, ProductId)
        Data.params("@PayerId", SqlDbType.Int, PayerId)
        Data.params("@ReportingId", SqlDbType.Int, If(ReportingID = 0, DBNull.Value, ReportingID))
        Data.params("@CommissionRate", SqlDbType.Decimal, CommissionRate)
        Data.params("@ErrorMessage", SqlDbType.NVarChar, 200, "", ParameterDirection.Output)
        Data.params("@RV", SqlDbType.Int, 0, ParameterDirection.ReturnValue)
        Dim dt As DataTable = Data.Filldata()
        oReturn = Data.sqlParameters("@RV")
        ErrorMessage = Data.sqlParameters("@ErrorMessage").ToString
        Return Data.Filldata

    End Function
    Public Function GetClaimHistoryReport(ByVal LocationId As Integer?, ByVal ProdID As Integer?, ByVal HfID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ClaimStatus As Integer?, ByVal InsuranceNumber As String, ByVal Scope As Integer, ByRef oReturn As Integer) As DataTable
        Dim Data As New ExactSQL
        Data.setSQLCommand("uspSSRSGetClaimHistoryReport", CommandType.StoredProcedure)
        Data.params("@LocationId", SqlDbType.Int, LocationId)
        Data.params("@ProdID", SqlDbType.Int, ProdID)
        Data.params("@HfID", SqlDbType.Int, HfID)
        Data.params("@StartDate", SqlDbType.Date, StartDate)
        Data.params("@EndDate", SqlDbType.Date, EndDate)
        Data.params("@ClaimStatus", SqlDbType.Int, ClaimStatus)
        Data.params("@InsuranceNumber", SqlDbType.NVarChar, 12, InsuranceNumber)
        Data.params("@Scope", SqlDbType.Int, Scope)
        Data.params("@RV", SqlDbType.Int, 0, ParameterDirection.ReturnValue)

        Dim dt As DataTable = Data.Filldata()
        oReturn = Data.sqlParameters("@RV")
        Return dt
    End Function
End Class
