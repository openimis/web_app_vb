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

Public Class ProcessBatchesBI
    Public Function ProcessRelativeIndex(ByVal monthid As Integer, ByVal Yearid As Integer) As Boolean
        Return True
    End Function
    Public Function GetMonths(ByVal start As Integer, ByVal ending As Integer) As DataTable
        Dim month As New IMIS_BL.GeneralBL
        Return month.GetMonths(start, ending)
    End Function

    Public Function GetYears(ByVal start As Integer, ByVal ending As Integer) As DataTable
        Dim year As New IMIS_BL.GeneralBL
        Return year.GetYears(start, ending)
    End Function

    Public Function GetHFLevelType(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim leveltype As New IMIS_BL.HealthFacilityBL

        Return leveltype.GetHFLevelType(showSelect)
    End Function

    Public Function GetProductsStict(LocationId As Integer, ByVal userID As Integer, ByVal showSelect As Boolean) As DataTable
        Dim product As New IMIS_BL.ProductsBL
        Return product.GetProductsStict(LocationId, userID, showSelect)
    End Function
    Public Function GetPeriod(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim product As New IMIS_BL.ProductsBL

        Return product.GetPeriod(showSelect)
    End Function
    Public Function GetRelIndexes(ByVal eRelIndex As IMIS_EN.tblRelIndex, ByVal process As Boolean) As DataTable
        Dim RelIndex As New IMIS_BL.RelIndexBL

        Return RelIndex.GetRelIndexes(eRelIndex, process)
    End Function
    Public Function GetHealthFacility(ByVal UserID As Integer, ByVal LocationId As Integer) As DataTable
        Dim HealthFacility As New IMIS_BL.HealthFacilityBL
        Return HealthFacility.GetHFCodes(UserID, LocationId)
    End Function

    Public Function GetDistricts(ByVal userID As Integer, Optional ByVal showSelect As Boolean = False, Optional ByVal RegionId As Integer = 0, Optional IncludeNational As Boolean = False) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userID, showSelect, RegionId:=RegionId)
    End Function
    Public Function ProcessRelativeIndexes(ByVal userID As Integer, Optional ByVal showSelect As Boolean = False) As Boolean
        Dim Districts As New IMIS_BL.LocationsBL
        Return True
    End Function
    Public Function GetAccounts(ByVal eReports As IMIS_EN.eReports) As DataTable
        Dim dt As New DataTable
        Return dt
    End Function
    Public Function PreviewAccounts(ByVal eReports As IMIS_EN.eReports) As DataTable
        Dim dt As New DataTable
        Return dt
    End Function

    Public Function GetHFLevel(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_BL.HealthFacilityBL
        Return getDataTable.GetHFLevel(showSelect)
    End Function
    Public Function GetHFCodes(ByVal UserId As Integer, ByVal LocationId As Integer) As DataTable
        Dim hf As New IMIS_BL.HealthFacilityBL
        Return hf.GetHFCodes(UserId, LocationId)
    End Function

    Public Function ProcessBatch(ByVal eRelIndex As IMIS_EN.tblRelIndex) As Integer
        Dim Process As New IMIS_BL.RelIndexBL
        Return Process.ProcessBatch(eRelIndex)
    End Function

    Public Function GetBatches(ByVal DistrictID As Integer) As DataTable
        Dim Process As New IMIS_BL.BatchRunBL
        Return Process.GetBatchRun(DistrictID)
    End Function

    Public Function GetProcessBatch(ByVal DistrictID As Integer, ByVal ProductId As Integer, ByVal RunID As Integer, ByVal HFID As Integer, ByVal HFLevel As String, ByVal DateFrom As Nullable(Of Date), ByVal DateTo As Nullable(Of Date), ByVal MinRemunerated As Decimal) As DataTable
        Dim Rpt As New IMIS_BL.ReportBL
        Return Rpt.GetProcessBatch(DistrictID, ProductId, RunID, HFID, HFLevel, DateFrom, DateTo, MinRemunerated)
    End Function
    Public Function GetProcessBatchWithClaims(ByVal DistrictID As Integer, ByVal ProductId As Integer, ByVal RunID As Integer, ByVal HFID As Integer, ByVal HFLevel As String, ByVal DateFrom As Nullable(Of Date), ByVal DateTo As Nullable(Of Date), ByVal MinRemunerated As Decimal) As DataTable
        Dim Rpt As New IMIS_BL.ReportBL
        Return Rpt.GetProcessBatchWithClaims(DistrictID, ProductId, RunID, HFID, HFLevel, DateFrom, DateTo, MinRemunerated)
    End Function
    Public Function GetBatchRunDate(ByVal DistrictID As Integer) As DataTable
        Dim Clm As New IMIS_BL.ClaimsBL
        Return Clm.GetBatchRunDate(DistrictID)
    End Function
    Public Function GetPeriodNo(ByVal Type As Char) As DataTable
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetPeriodNo(Type)
    End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True, Optional ByVal IncludeNational As Boolean = False) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect, IncludeNational)
    End Function
End Class
