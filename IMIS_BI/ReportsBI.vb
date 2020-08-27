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

Public Class ReportsBI
    Public UserRights As New IMIS_BL.UsersBL
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Return UserRights.CheckRights(Right, UserID)
    End Function
    Public Function GetProducts(ByVal UserId, Optional ByVal ShowSelect = False, Optional ByVal RegionId = 0, Optional ByVal DistrictId = 0) As DataTable
        Dim product As New IMIS_BL.ProductsBL
        Return product.GetProducts(UserId, ShowSelect, RegionId, DistrictId)
    End Function
    Public Function GetProductsStict(LocationId As Integer, ByVal UserId As Integer, ByVal showSelect As Boolean) As DataTable
        Dim product As New IMIS_BL.ProductsBL
        Return product.GetProductsStict(LocationId, UserId, showSelect)
    End Function
    Public Function GetHFCodes(ByVal UserId As Integer, ByVal LocationId As Integer) As DataTable
        Dim HealthFacility As New IMIS_BL.HealthFacilityBL
        Return HealthFacility.GetHFCodes(UserId, LocationId)
    End Function
    Public Function GetDistricts(ByVal userID As Integer, Optional ByVal showSelect As Boolean = False, Optional ByVal RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userID, showSelect, RegionId)
    End Function
    Public Function GetWards(ByVal DistrictID As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Wards As New IMIS_BL.LocationsBL
        Return Wards.GetWards(DistrictID, True)
    End Function
    Public Function GetVillages(ByVal WardId As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Villages As New IMIS_BL.LocationsBL
        Return Villages.GetVillages(WardId, True)
    End Function
    Public Function GetPrimaryIndicators1(ByVal DistrictId As Integer, ByVal ProductId As Integer, ByVal Month As Integer, ByVal Year As Integer, ByVal Mode As Int16, Optional ByVal MonthTo As Integer = 0) As DataTable
        Dim Rpt As New IMIS_BL.ReportBL
        Return Rpt.GetPrimaryIndicators1(DistrictId, ProductId, Month, Year, Mode, MonthTo)
    End Function
    Public Function GetPrimaryIndicators2(ByVal DistrictId As Integer, ByVal ProductId As Integer, ByVal HFID As Integer, ByVal Year As Integer, ByVal MonthFrom As Integer, ByVal MonthTo As Integer) As DataTable
        Dim Rpt As New IMIS_BL.ReportBL
        Return Rpt.GetPrimaryIndicators2(DistrictId, ProductId, HFID, Year, MonthFrom, MonthTo)
    End Function
    Public Function GetDerivedIndicators(ByVal DistrictId As Integer, ByVal ProductId As Integer, ByVal HFID As Integer, ByVal Month As Integer, ByVal Year As Integer) As DataSet
        Dim Rpt As New IMIS_BL.ReportBL
        Return Rpt.GetDerivedIndicators(DistrictId, ProductId, HFID, Month, Year)
    End Function
    Public Function GetMonths(ByVal start As Integer, ByVal ending As Integer) As DataTable
        Dim month As New IMIS_BL.GeneralBL
        Return month.GetMonths(start, ending)
    End Function
    Public Function GetYears(ByVal start As Integer, ByVal ending As Integer) As DataTable
        Dim year As New IMIS_BL.GeneralBL
        Return year.GetYears(start, ending)
    End Function
    Public Function GetReportTypes(ByVal UserID As Integer) As DataTable
        Dim report As New IMIS_BL.ReportBL
        Return report.GetReportTypes(UserID)
    End Function
    Public Function GetTypeOfPayment(Optional ByVal showSelect As Boolean = True) As DataTable
        Dim getDataTable As New IMIS_BL.PremiumBL
        Return getDataTable.GetPayType(showSelect)
    End Function
    Public Function GetPremiumCollection(ByVal District As Integer, ByVal Product As Integer, ByVal PaymentType As String, ByVal FromDate As Date, ByVal ToDate As Date) As DataTable
        Dim Rpt As New IMIS_BL.ReportBL
        Return Rpt.GetPremiumCollection(District, Product, PaymentType, FromDate, ToDate)
    End Function
    Public Function GetPolicySold(ByVal District As Integer, ByVal Product As Integer, ByVal FromDate As Date, ByVal ToDate As Date) As DataTable
        Dim Rpt As New IMIS_BL.ReportBL
        Return Rpt.GetPolicySold(District, Product, FromDate, ToDate)
    End Function
    Public Function GetPremiumDistribution(ByVal District As Integer, ByVal Product As Integer, ByVal Month As Integer, ByVal Year As Integer)
        Dim Rpt As New IMIS_BL.ReportBL
        Return Rpt.GetPremiumDistribution(District, Product, Month, Year)
    End Function
    Public Function GetPeriodNo(ByVal Type As Char) As DataTable
        Dim BL As New IMIS_BL.GeneralBL
        Return BL.GetPeriodNo(Type)
    End Function
    Public Function GetUserActivityData(ByVal UserID As Integer, ByVal StartDate As DateTime, ByVal EndDate As DateTime, ByVal Action As String, ByVal Entity As String) As DataTable
        Dim BI As New IMIS_BL.ReportBL
        Return BI.GetUserActivityData(UserID, StartDate, EndDate, Action, Entity)
    End Function
    Public Function GetUsers() As DataTable
        Dim BL As New IMIS_BL.UsersBL
        Return BL.GetUsers
    End Function
    Public Function GetStatusofRegisters(ByVal DistrictID As Integer) As DataTable
        Dim BL As New IMIS_BL.ReportBL
        Return BL.GetStatusofRegisters(DistrictID)
    End Function
    Public Function GetInsureesWithoutPhotos(ByVal OfficerId As Integer, ByVal DistrictID As Integer) As DataTable
        Dim BL As New IMIS_BL.ReportBL
        Return BL.GetInsureesWithoutPhotos(OfficerId, DistrictID)
    End Function
    Public Function GetOfficers(ByVal DistrictId As Integer, ByVal showselect As Boolean, Optional ByVal VillageId As Integer = 0) As DataTable
        Dim BL As New IMIS_BL.OfficersBL
        Return BL.GetOfficers(DistrictId, showselect, VillageId)
    End Function
    Public Function GetPaymentCategoryOverview(ByVal DateFrom As DateTime, ByVal DateTo As DateTime, ByVal DistrictId As Integer, ByVal ProductId As Integer) As DataTable
        Dim BL As New IMIS_BL.ReportBL
        Return BL.GetPaymentCategoryOverview(DateFrom, DateTo, DistrictId, ProductId)
    End Function
    Public Function GetPayers(ByVal RegionId As Integer, ByVal Districtid As Integer, ByVal userId As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Premimum As New IMIS_BL.PayersBL
        Return Premimum.GetPayers(RegionId, Districtid, userId, True)
    End Function
    Public Function GetPreviousMatchingFundsReportDates(ByVal UserID As Integer, ByVal DistrictID As Integer, ByVal ReportingID As Integer?) As DataTable
        Dim Rep As New IMIS_BL.ReportingBL
        Return Rep.GetPreviousMatchingFundsReportDates(UserID, DistrictID, ReportingID)
    End Function
    Public Function GetClaimStatus(Optional ByVal RetrievalValue As Integer = 0) As DataTable
        Dim Claims_BL As New IMIS_BL.ClaimsBL
        Return Claims_BL.GetClaimStatus(RetrievalValue)
    End Function
    Public Function GetMatchingFunds(ByVal DistrictID As Integer?, ByVal ProdID As Integer?, ByVal PayerID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ReportingID As Integer?, ByRef ErrorMessage As String, ByRef oReturn As Integer) As DataTable
        Dim BL As New IMIS_BL.ReportBL
        Return BL.GetMatchingFunds(DistrictID, ProdID, PayerID, StartDate, EndDate, ReportingID, ErrorMessage, oReturn)
    End Function
    Public Function GetClaimOverview(ByVal DistrictID As Integer?, ByVal ProdID As Integer?, ByVal HfID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ClaimStatus As Integer?, ByVal Scope As Integer?, ByRef oReturn As Integer) As DataTable
        Dim BL As New IMIS_BL.ReportBL
        Return BL.GetClaimOverview(DistrictID, ProdID, HfID, StartDate, EndDate, ClaimStatus, Scope, oReturn)
    End Function
    Public Function GetPercentageReferral(RegionId As Integer, DistrictId As Integer, StartDate As Date, EndDate As Date) As DataTable
        Dim Report As New IMIS_BL.ReportBL
        Return Report.GetPercentageReferral(RegionId, DistrictId, StartDate, EndDate)
    End Function
    Public Function GetEnroledFamilies(ByVal LocationId As Integer?, ByVal StartDate As Date, ByVal EndDate As Date, ByVal PolicyStatus As Integer?) As DataTable
        Dim Rep As New IMIS_BL.ReportBL
        Return Rep.GetEnroledFamilies(LocationId, StartDate, EndDate, PolicyStatus)
    End Function
    Public Function GetPendingInsurees(ByVal DistrictId As Integer, ByVal OfficerId As Integer?, ByVal StartDate As Date, ByVal EndDate As Date)
        Dim Rep As New IMIS_BL.ReportBL
        Return Rep.GetPendingInsurees(DistrictId, OfficerId, StartDate, EndDate)
    End Function
    Public Function GetPolicyStatus(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim _GetPolicyStatus As New IMIS_BL.PolicyBL
        Return _GetPolicyStatus.GetPolicyStatus(showSelect)
    End Function
    Public Function GetActions() As DataTable
        Dim BL As New IMIS_BL.ReportBL
        Return BL.GetActions
    End Function
    Public Function GetEntities() As DataTable
        Dim BL As New IMIS_BL.ReportBL
        Return BL.GetEntities
    End Function
    Public Function GetProductName_Account(ProdId As Integer) As DataTable
        Dim ProdBL As New IMIS_BL.ProductsBL
        Return ProdBL.GetProductName_Account(ProdId)
    End Function
    Public Function GetAllProducts() As DataTable
        Dim BL As New IMIS_BL.ProductsBL
        Return BL.GetAllProducts
    End Function
    Public Function GetRenewals(DistrictId As Integer, ProductId As Integer, OfficerId As Integer?, FromDate As Date, ToDate As Date, Sort As String) As DataTable
        Dim Report As New IMIS_BL.ReportBL
        Return Report.GetRenewals(DistrictId, ProductId, OfficerId, FromDate, ToDate, Sort)
    End Function
    Public Function GetSorting() As DataTable
        Dim Report As New IMIS_BL.ReportBL
        Return Report.GetSorting
    End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True, Optional IncludeNational As Boolean = False) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect, IncludeNational)
    End Function
    Public Function getProductCapitationDetails(ByVal ProductId As Integer) As DataTable
        Dim BL As New IMIS_BL.ProductsBL
        Return BL.getProductCapitationDetails(ProductId)
    End Function
    Public Function getCatchmentArea(RegionId As Integer, DistrictId As Integer, ByVal ProductId As Integer, ByVal Year As Integer, ByVal Month As Integer) As DataTable
        Dim BL As New IMIS_BL.ReportBL
        Return BL.getCatchmentArea(RegionId, DistrictId, ProductId, Year, Month)
    End Function

    Public Function getRejectedPhoto(startDate As Date, endDate As Date) As DataTable
        Dim Rep As New IMIS_BL.ReportBL
        Return Rep.getRejectedPhoto(startDate, endDate)
    End Function

    Public Function GetPaymentContribution(startDate As Date?, endDate As Date?, controlNumber As String, productCode As String, paymentStutus As Integer)
        Dim Rep As New IMIS_BL.ReportBL
        Return Rep.GetPaymentContribution(startDate, endDate, controlNumber, productCode, paymentStutus)
        Return True
    End Function
    Public Function GetControlNumberAssignment(startDate As Date, endDate As Date, PostingStatus As String, AssignmentStatus As String, RegionId As Integer, DistrictId As Integer)
        Dim Rep As New IMIS_BL.ReportBL
        Return Rep.GetControlNumberAssignment(startDate, endDate, PostingStatus, AssignmentStatus, RegionId, DistrictId)
    End Function
    Public Function GetPayementStatus(Optional Include As Boolean = False, Optional ForReport As Boolean = False) As DataTable
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetPaymentStatus(Include, ForReport)
    End Function
    Public Function GetPostingStatus(Optional Include As Boolean = False) As DataTable
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetPostingStatus(Include)
    End Function
    Public Function GetAssignmentStatus(Optional Include As Boolean = False) As DataTable
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetAssignmentStatus(Include)
    End Function
    Public Function FillMode() As DataTable
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetMode()
    End Function
    Public Function GetPreviousOverviewOfCommissionsReportDates(ByVal UserID As Integer, ByVal DistrictID As Integer, ByVal ReportingID As Integer?, Year As Integer, Month As Integer) As DataTable
        Dim Rep As New IMIS_BL.ReportingBL
        Return Rep.GetPreviousOverviewOfCommissionsReportDates(UserID, DistrictID, ReportingID, Year, Month)
    End Function

    Public Function GetOverviewOfCommissions(ByVal LocationId As Integer?, ByVal ProductId As Integer?, ByVal Month As Integer?, ByVal Year As Integer?, ByVal PayerId As Integer?, ByVal OfficerId As Integer?, ByVal Mode As Integer, ByVal CommissionRate As Decimal?, ByVal ReportingID As Integer?, ByRef ErrorMessage As String, ByRef oReturn As Integer) As DataTable
        Dim Rep As New IMIS_BL.ReportBL
        Return Rep.GetOverviewOfCommissions(LocationId, ProductId, Month, Year, PayerId, OfficerId, Mode, CommissionRate, ReportingID, ErrorMessage, oReturn)
    End Function
    Public Function GetClaimHistoryReport(ByVal LocationId As Integer?, ByVal ProdID As Integer?, ByVal HfID As Integer?, ByVal StartDate As Date?, ByVal EndDate As Date?, ByVal ClaimStatus As Integer?, ByVal InsuranceNumber As String, ByVal Scope As Integer, ByRef oReturn As Integer) As DataTable
        Dim BL As New IMIS_BL.ReportBL
        Return BL.GetClaimHistoryReport(LocationId, ProdID, HfID, StartDate, EndDate, ClaimStatus, InsuranceNumber, Scope, oReturn)
    End Function
    Public Function GetScope() As DataTable
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetScope()
    End Function
End Class
