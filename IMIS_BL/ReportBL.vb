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

Public Class ReportBL
    Public Function getMessage(ByVal MessageID As String) As String
        Return System.Web.HttpContext.GetGlobalResourceObject("Resource", MessageID)
    End Function
    Public Function GetPremiumCollection(ByVal District As Integer, ByVal Product As Integer, ByVal PaymentType As String, ByVal FromDate As Date, ByVal ToDate As Date) As DataTable
        Dim Rpt As New IMIS_DAL.ReportDAL
        Dim PremiumBL As New PremiumBL
        Dim dtPaymentType As DataTable = PremiumBL.GetPayType(True)
        dtPaymentType.Columns.Add("AltLanguage")

        Dim dt As DataTable = Rpt.GetPremiumCollection(District, Product, PaymentType, FromDate, ToDate, dtPaymentType)
        If dt.Rows.Count = 0 Then
            Throw New Exception(getMessage("M_NODATAFORREPORT"))
        End If
        Return dt
    End Function
    Public Function GetPolicySold(ByVal District As Integer, ByVal Product As Integer, ByVal FromDate As Date, ByVal ToDate As Date) As DataTable
        Dim Rpt As New IMIS_DAL.ReportDAL
        Dim dt As DataTable = Rpt.GetPolicySold(District, Product, FromDate, ToDate)
        If dt.Rows.Count = 0 Then
            Throw New Exception(getMessage("M_NODATAFORREPORT"))
        End If
        Return dt
    End Function
    Public Function GetPremiumDistribution(ByVal District As Integer, ByVal Product As Integer, ByVal Month As Integer, ByVal Year As Integer) As DataTable
        Dim Rpt As New IMIS_DAL.ReportDAL
        Dim dt As DataTable = Rpt.GetPremiumDistribution(District, Product, Month, Year)
        If dt.Rows.Count = 0 Then
            Throw New Exception(getMessage("M_NODATAFORREPORT"))
        End If
        Return dt

    End Function
    Public Function GetFeedbackPrompt(ByVal SMSStatus As Integer, ByVal DistrictId As Integer, ByVal WardId As Integer, ByVal VillageID As Integer, ByVal OfficerID As Integer, ByVal RangeFrom As Date, ByVal RangeTo As Date) As DataTable
        Dim Rpt As New IMIS_DAL.ReportDAL
        Return Rpt.GetFeedbackPrompt(SMSStatus, DistrictId, WardId, VillageID, OfficerID, RangeFrom, RangeTo)
    End Function
    Public Function GetProcessBatch(ByVal DistrictID As Integer, ByVal ProductId As Integer, ByVal RunID As Integer, ByVal HFID As Integer, ByVal HFLevel As String, ByVal DateFrom As Nullable(Of Date), ByVal DateTo As Nullable(Of Date), ByVal MinRemunerated As Decimal) As DataTable
        Dim Rpt As New IMIS_DAL.ReportDAL
        Return Rpt.GetProcessBatch(DistrictID, ProductId, RunID, HFID, HFLevel, DateFrom, DateTo, MinRemunerated)
    End Function
    Public Function GetPrimaryIndicators1(ByVal DistrictId As Integer, ByVal ProductId As Integer, ByVal Month As Integer, ByVal Year As Integer, ByVal Mode As Int16, Optional ByVal MonthTo As Integer = 0) As DataTable
        Dim Rpt As New IMIS_DAL.ReportDAL
        Dim dt As New DataTable
        If MonthTo = 0 Then MonthTo = Month
        dt = Rpt.GetPrimaryIndicators1(DistrictId, ProductId, Month, Year, Mode, MonthTo)
        If dt.Rows.Count = 0 Then
            Throw New Exception(getMessage("M_NODATAFORREPORT"))
        End If

        Return dt
    End Function
    Public Function GetPrimaryIndicators2(ByVal DistrictId As Integer, ByVal ProductId As Integer, ByVal HFID As Integer, ByVal Year As Integer, ByVal MonthFrom As Integer, ByVal MonthTo As Integer) As DataTable
        Dim Rpt As New IMIS_DAL.ReportDAL
        Dim dt As New DataTable
        dt = Rpt.GetPrimaryIndicators2(DistrictId, ProductId, HFID, Year, MonthFrom, MonthTo)
        If dt.Rows.Count = 0 Then
            Throw New Exception(getMessage("M_NODATAFORREPORT"))
        End If

        Return dt
    End Function
    Public Function GetDerivedIndicators(ByVal DistrictId As Integer, ByVal ProductId As Integer, ByVal HFID As Integer, ByVal Month As Integer, ByVal Year As Integer) As DataSet
        Dim Rpt As New IMIS_DAL.ReportDAL
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim ds As New DataSet

        dt1 = Rpt.GetDerivedIndicators1(DistrictId, ProductId, Month, Year)
        dt2 = Rpt.GetDerivedIndicators2(DistrictId, ProductId, HFID, Month, Year)
        If dt1.Rows.Count = 0 And dt2.Rows.Count = 0 Then
            Throw New Exception(getMessage("M_NODATAFORREPORT"))
        End If
        ds.Tables.Add(dt1)
        ds.Tables.Add(dt2)

        Return ds

    End Function
    Public Function GetReportTypes(ByVal UserId As Integer) As DataTable
        Dim dt As New DataTable
        Dim roles As New UsersBL

        dt.Columns.Add("Id")
        dt.Columns.Add("Name")

        Dim dict As New Dictionary(Of String, IMIS_EN.Enums.Rights)
        dict.Add(getMessage("T_PRIMARYOPERATIONALINDICATORS-POLICIES"), IMIS_EN.Enums.Rights.ReportsPrimaryOperationalIndicatorPolicies) 'id = 1
        dict.Add(getMessage("T_PRIMARYOPERATIONALINDICATORS-CLAIMS"), IMIS_EN.Enums.Rights.ReportsPrimaryOperationalIndicatorsClaims) 'id = 2
        dict.Add(getMessage("T_DERIVEDOPERATIONALINDICATORS"), IMIS_EN.Enums.Rights.ReportsDerivedOperationalIndicators) 'id = 3
        dict.Add(getMessage("T_PREMIUMCOLLECTION"), IMIS_EN.Enums.Rights.ReportsContributionCollection) 'id = 4
        dict.Add(getMessage("T_PRODUCTSALES"), IMIS_EN.Enums.Rights.ReportsProductSales) 'id = 5
        dict.Add(getMessage("T_PREMIUMDISTRIBUTION"), IMIS_EN.Enums.Rights.ReportsContributionDistribution) 'id = 6
        dict.Add(getMessage("T_USERACTIVITYREPORT"), IMIS_EN.Enums.Rights.ReportsUserActivity) 'id = 7
        dict.Add(getMessage("T_ENROLMENTPERFORMANCEINDICATORS"), IMIS_EN.Enums.Rights.ReportsEnrolmentPerformanceIndicators) 'id = 8
        dict.Add(getMessage("T_STATUSOFREGISTERS"), IMIS_EN.Enums.Rights.ReportsStatusOfRegister) 'id = 9
        dict.Add(getMessage("T_INSUREESWITHOUTPHOTOS"), IMIS_EN.Enums.Rights.ReportsInsureeWithoutPhotos) 'id = 10
        dict.Add(getMessage("T_PAYMENTCATEGORYOVERVIEW"), IMIS_EN.Enums.Rights.ReportsPaymentCategoryOverview) 'id = 11
        dict.Add(getMessage("T_MATCHINGFUNDS"), IMIS_EN.Enums.Rights.ReportsMatchingFunds) 'id = 12
        dict.Add(getMessage("T_CLAIMOVERVIEW"), IMIS_EN.Enums.Rights.ReportsClaimOverviewReport) 'id = 13
        dict.Add(getMessage("T_PERCENTAGEOFREFERRALS"), IMIS_EN.Enums.Rights.ReportsPercentageReferrals) 'id = 14
        dict.Add(getMessage("T_FAMILIESINSUREESOVERVIEW"), IMIS_EN.Enums.Rights.ReportsFamiliesInsureesOverview) 'id = 15
        dict.Add(getMessage("T_PENDINGINSUREES"), IMIS_EN.Enums.Rights.ReportsPendingInsurees) 'id = 16
        dict.Add(getMessage("T_RENEWALS"), IMIS_EN.Enums.Rights.ReportsRenewals)    'id = 17
        dict.Add(getMessage("T_CAPITATIONPAYMENT"), IMIS_EN.Enums.Rights.ReportsCapitationPayment)  'Id = 18
        dict.Add(getMessage("T_REJECTEDPHOTOS"), IMIS_EN.Enums.Rights.ReportRejectedPhoto)  'Id = 19
        dict.Add(getMessage("L_CONTRIBUTIONPAYMENT"), IMIS_EN.Enums.Rights.ReportsContributionPayment) 'Id =20 
        dict.Add(getMessage("L_CONTROLNUMBERASSIGNMENT"), IMIS_EN.Enums.Rights.ReportsControlNumberAssignment) 'Id = 21
        dict.Add(getMessage("L_OVERVIEWOFCOMMISSIONS"), IMIS_EN.Enums.Rights.ReportsOverviewOfCommissions) 'Id = 22
        dict.Add(getMessage("L_CLAIMHISTORYREPORT"), IMIS_EN.Enums.Rights.ReportsClaimHistoryReport) 'Id = 23
        Dim dr As DataRow
        Dim index As Integer = 1
        For Each Rtype As String In dict.Keys
            If roles.CheckRights(dict(Rtype), UserId) Then
                dr = dt.NewRow()
                dr("Id") = index
                dr("Name") = Rtype
                dt.Rows.Add(dr)
            End If
            index = index + 1
        Next
        Return dt
    End Function
    Public Function GetUserActivityData(ByVal UserID As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal Action As String, ByVal Entity As String) As DataTable
        Dim BL As New IMIS_DAL.ReportDAL
        Return BL.GetUserActivityData(UserID, StartDate, EndDate, Action, Entity)
    End Function
    Public Function GetStatusofRegisters(ByVal DistrictID As Integer) As DataTable
        Dim DAL As New IMIS_DAL.ReportDAL
        Return DAL.GetStatusofRegisters(DistrictID)
    End Function
    Public Function GetInsureesWithoutPhotos(ByVal OfficerId As Integer, ByVal DistrictID As Integer) As DataTable
        Dim DAL As New IMIS_DAL.ReportDAL
        Dim dt As DataTable = DAL.GetInsureesWithoutPhotos(OfficerId, DistrictID)
        If dt.Rows.Count = 0 Then
            Throw New Exception(getMessage("M_NODATAFORREPORT"))
        End If
        Return dt
    End Function
    Public Function GetPaymentCategoryOverview(ByVal DateFrom As DateTime, ByVal DateTo As DateTime, ByVal DistrictId As Integer, ByVal ProductId As Integer) As DataTable
        Dim DAL As New IMIS_DAL.ReportDAL
        Dim dt As DataTable = DAL.GetPaymentCategoryOverview(DateFrom, DateTo, DistrictId, ProductId)
        If dt.Rows.Count = 0 Then
            Throw New Exception(getMessage("M_NODATAFORREPORT"))
        End If
        Return dt
    End Function
    Public Function GetMatchingFunds(ByVal DistrictID As Integer?, ByVal ProdID As Integer?, ByVal PayerID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ReportingID As Integer?, ByRef ErrorMessage As String, ByRef oReturn As Integer) As DataTable
        Dim DAL As New IMIS_DAL.ReportDAL
        Return DAL.GetMatchingFunds(DistrictID, ProdID, PayerID, StartDate, EndDate, ReportingID, ErrorMessage, oReturn)
    End Function
    Public Function GetClaimOverview(ByVal DistrictID As Integer?, ByVal ProdID As Integer?, ByVal HfID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ClaimStatus As Integer?, ByVal Scope As Integer?, ByRef oReturn As Integer) As DataTable
        Dim DAL As New IMIS_DAL.ReportDAL
        Dim imisgen As New GeneralBL
        Dim dtRejReason = New DataTable
        dtRejReason = imisgen.GetAllRejectedReasons()
        Return DAL.GetClaimOverview(DistrictID, ProdID, HfID, StartDate, EndDate, ClaimStatus, Scope, dtRejReason, oReturn)
    End Function
    Public Function GetPercentageReferral(RegionId As Integer, DistrictId As Integer, StartDate As Date, EndDate As Date) As DataTable
        Dim Report As New IMIS_DAL.ReportDAL
        Return Report.GetPercentageReferral(RegionId, DistrictId, StartDate, EndDate)
    End Function
    Public Function GetProcessBatchWithClaims(ByVal DistrictID As Integer, ByVal ProductId As Integer, ByVal RunID As Integer, ByVal HFID As Integer, ByVal HFLevel As String, ByVal DateFrom As Nullable(Of Date), ByVal DateTo As Nullable(Of Date), ByVal MinRemunerated As Decimal) As DataTable
        Dim Rpt As New IMIS_DAL.ReportDAL
        Return Rpt.GetProcessBatchWithClaims(DistrictID, ProductId, RunID, HFID, HFLevel, DateFrom, DateTo, MinRemunerated)
    End Function
    Public Function GetEnroledFamilies(ByVal LocationId As Integer?, ByVal StartDate As Date, ByVal EndDate As Date, ByVal PolicyStatus As Integer?) As DataTable
        Dim Rep As New IMIS_DAL.ReportDAL
        Dim dtPolicyStatus As DataTable = getPolicyStatusForEnroledFamilies()
        Return Rep.GetEnroledFamilies(LocationId, StartDate, EndDate, PolicyStatus, dtPolicyStatus)
    End Function
    Private Function getPolicyStatusForEnroledFamilies() As DataTable
        Dim imisgen As New GeneralBL
        Dim dt As New DataTable
        dt.Columns.Add("Code", GetType(Integer))
        dt.Columns.Add("Status")

        dt.Rows.Add(New Object() {0, imisgen.getMessage("T_NOPOLICY")})
        dt.Rows.Add(New Object() {1, imisgen.getMessage("T_IDLE")})
        dt.Rows.Add(New Object() {2, imisgen.getMessage("T_ACTIVE")})
        dt.Rows.Add(New Object() {4, imisgen.getMessage("T_SUSPENDED")})
        dt.Rows.Add(New Object() {8, imisgen.getMessage("T_EXPIRED")})

        Return dt
    End Function

    Public Function getRejectedPhoto(startDate As Date, endDate As Date) As DataTable
        Dim Rep As New IMIS_DAL.ReportDAL
        Return Rep.getRejectedPhoto(startDate, endDate)
    End Function

    Public Function GetPendingInsurees(ByVal DistrictId As Integer, ByVal OfficerId As Integer?, ByVal StartDate As Date, ByVal EndDate As Date)
        Dim Rep As New IMIS_DAL.ReportDAL
        Return Rep.GetPendingInsurees(DistrictId, OfficerId, StartDate, EndDate)
    End Function
    Public Function GetActions() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("ActionId")
        dt.Columns.Add("Action")

        dt.Rows.Add(New Object() {"", getMessage("R_ALLACTION")})
        dt.Rows.Add(New Object() {"Logged In", getMessage("R_LOGIN")})
        dt.Rows.Add(New Object() {"Logged Out", getMessage("L_LOGOUT")})
        dt.Rows.Add(New Object() {"Inserted", getMessage("R_INSERT")})
        dt.Rows.Add(New Object() {"Modified", getMessage("R_MODIFY")})
        dt.Rows.Add(New Object() {"Deleted", getMessage("B_DELETE")})
        Return dt
    End Function
    Public Function GetEntities() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("EntityId")
        dt.Columns.Add("Entity")

        dt.Rows.Add(New Object() {"", getMessage("R_SELECTENTITY")})
        dt.Rows.Add(New Object() {"F", getMessage("R_FAMILY")})
        dt.Rows.Add(New Object() {"Ins", getMessage("L_INSUREES")})
        dt.Rows.Add(New Object() {"PL", getMessage("L_POLICY")})
        dt.Rows.Add(New Object() {"PR", getMessage("L_PREMIUM")})
        dt.Rows.Add(New Object() {"C", getMessage("L_CLAIM")})
        dt.Rows.Add(New Object() {"Prd", getMessage("R_INSURANCEPRODUCT")})
        dt.Rows.Add(New Object() {"HF", getMessage("L_OFFLINEHFID")})
        dt.Rows.Add(New Object() {"O", getMessage("R_ENROLLMENTOFFICER")})
        dt.Rows.Add(New Object() {"PLI", getMessage("R_PRICELISTITEM")})
        dt.Rows.Add(New Object() {"PLS", getMessage("R_PRICELISTSERVICE")})
        dt.Rows.Add(New Object() {"S", getMessage("L_MEDICALSERVICES")})
        dt.Rows.Add(New Object() {"I", getMessage("R_MEDICALITEM")})
        dt.Rows.Add(New Object() {"CA", getMessage("L_CLAIMADMIN")})
        dt.Rows.Add(New Object() {"D", getMessage("L_DISTRICT")})
        dt.Rows.Add(New Object() {"W", getMessage("L_WARD")})
        dt.Rows.Add(New Object() {"V", getMessage("L_VILLAGE")})
        dt.Rows.Add(New Object() {"U", getMessage("R_USER")})
        dt.Rows.Add(New Object() {"P", getMessage("L_PAYER")})
        dt.Rows.Add(New Object() {"BR", getMessage("L_BATCHRUN")})
        dt.Rows.Add(New Object() {"E", getMessage("R_EXTRACT")})
        dt.Rows.Add(New Object() {"FB", getMessage("B_FEEDBACK")})
        dt.Rows.Add(New Object() {"ICD", getMessage("L_ICDCODE")})
        dt.Rows.Add(New Object() {"Ph", getMessage("L_PHOTOS")})
        Return dt
    End Function
    Public Function GetRenewals(DistrictId As Integer, ProductId As Integer, OfficerId As Integer?, FromDate As Date, ToDate As Date, Sort As String) As DataTable
        Dim Report As New IMIS_DAL.ReportDAL
        Return Report.GetRenewals(DistrictId, ProductId, OfficerId, FromDate, ToDate, Sort)
    End Function
    Public Function GetSorting() As DataTable
        Dim imisgen As New GeneralBL
        Dim dt As New DataTable
        dt.Columns.Add("Code")
        dt.Columns.Add("Sorting")

        dt.Rows.Add(New Object() {"D", imisgen.getMessage("L_RENEWALDATE")}) 'Renewal date
        dt.Rows.Add(New Object() {"R", imisgen.getMessage("L_RECEIPTNUMBER")}) 'Receipt number
        dt.Rows.Add(New Object() {"O", imisgen.getMessage("R_ENROLLMENTOFFICER")}) 'Enrollment officer

        Return dt
    End Function
    Public Function getCatchmentArea(RegionId As Integer, DistrictId As Integer, ByVal ProductId As Integer, ByVal Year As Integer, ByVal Month As Integer) As DataTable
        Dim DAL As New IMIS_DAL.ReportDAL
        Dim BL As New IMIS_BL.HealthFacilityBL
        Dim dt As DataTable = BL.GetHFLevel(False)
        Return DAL.getCatchmentArea(RegionId, DistrictId, ProductId, Year, Month, dt)
    End Function
    Public Function GetPaymentContribution(startDate As Date?, endDate As Date?, controlNumber As String, productCode As String, paymentStutus As Integer)
        Dim DAL As New IMIS_DAL.ReportDAL
        Return DAL.GetPaymentContribution(startDate, endDate, controlNumber, productCode, paymentStutus)
        Return True
    End Function
    Public Function GetControlNumberAssignment(startDate As Date, endDate As Date, PostingStatus As String, AssignmentStatus As String, RegionId As Integer, DistrictId As Integer)
        Dim DAL As New IMIS_DAL.ReportDAL
        Dim gen As New GeneralBL
        Dim dt As New DataTable
        dt = gen.GetPaymentStatusNames()
        Return DAL.GetControlNumberAssignment(startDate, endDate, PostingStatus, AssignmentStatus, RegionId, DistrictId, dt)
    End Function

    Public Function GetOverviewOfCommissions(ByVal LocationId As Integer?, ByVal ProductId As Integer?, ByVal Month As Integer?, ByVal Year As Integer?, ByVal PayerId As Integer?, ByVal OfficerId As Integer?, ByVal Mode As Integer, ByVal CommissionRate As Decimal?, ByVal ReportingID As Integer?, ByRef ErrorMessage As String, ByRef oReturn As Integer) As DataTable
        Dim DAL As New IMIS_DAL.ReportDAL
        Return DAL.GetOverviewOfCommissions(LocationId, ProductId, Month, Year, PayerId, OfficerId, Mode, CommissionRate, ReportingID, ErrorMessage, oReturn)
    End Function
    Public Function GetClaimHistoryReport(ByVal LocationId As Integer?, ByVal ProdID As Integer?, ByVal HfID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ClaimStatus As Integer?, ByVal InsuranceNumber As String, ByVal Scope As Integer, ByRef oReturn As Integer) As DataTable
        Dim DAL As New IMIS_DAL.ReportDAL
        Dim imisgen As New GeneralBL
        Dim dtRejReasons = New DataTable
        dtRejReasons = imisgen.GetAllRejectedReasons()
        Return DAL.GetClaimHistoryReport(LocationId, ProdID, HfID, StartDate, EndDate, ClaimStatus, InsuranceNumber, Scope, dtRejReasons, oReturn)
    End Function
End Class
