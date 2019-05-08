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

Public Class FindClaimsBI
    Public UserRights As New IMIS_BL.UsersBL
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Return UserRights.CheckRights(Right, UserID)
    End Function
    Dim Claims_BL As New IMIS_BL.ClaimsBL
    Public Function GetClaims(ByRef eClaims As IMIS_EN.tblClaim, ByVal UserID As Integer) As DataTable
        Return Claims_BL.GetClaims(eClaims, UserID)
    End Function
    Public Function GetClaimsCount(ByRef eClaims As IMIS_EN.tblClaim, ByVal UserID As Integer) As Integer
        Return Claims_BL.GetClaimsCount(eClaims, UserID)
    End Function
    Public Function GetDistricts(ByVal userId As Integer, Optional ByVal showSelect As Boolean = False, Optional ByVal RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userId, True, RegionId)
    End Function
    Public Function GetHFCodes(ByVal UserId As Integer, ByVal LocationId As Integer) As DataTable
        Dim hf As New IMIS_BL.HealthFacilityBL
        Return hf.GetHFCodes(UserId, LocationId)
    End Function
    Public Function GetICDCodes(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim GetDataTable As New IMIS_BL.ICDCodesBL
        Return GetDataTable.GetICDCodes
    End Function
    Public Function GetHFClaimAdminCodes(ByVal HFID As Integer, ByVal ShowSelect As Boolean) As DataTable
        Dim BLClaimAdmin As New IMIS_BL.ClaimAdminBL
        Return BLClaimAdmin.GetHFClaimAdminCodes(HFID, ShowSelect)
    End Function
    Public Function DeleteClaim(ByRef eClaim As IMIS_EN.tblClaim) As Boolean
        Return Claims_BL.DeleteClaim(eClaim)
    End Function

    Public Sub SubmitClaims(ByVal dt As DataTable, ByVal UserID As Integer, ByRef Submitted As Integer, ByRef Checked As Integer, ByRef Rejected As Integer, ByRef Changed As Integer, ByRef Failed As Integer, ByRef ItemsPassed As Integer, ByRef ServicesPassed As Integer, ByRef ItemsRejected As Integer, ByRef ServicesRejected As Integer)
        Claims_BL.SubmitClaims(dt, UserID, Submitted, Checked, Rejected, Changed, Failed, ItemsPassed, ServicesPassed, ItemsRejected, ServicesRejected)
    End Sub
    Public Function GetBatchRun(ByVal DistrictID As Integer) As DataTable
        Dim br As New IMIS_BL.BatchRunBL
        Return br.GetBatchRun(DistrictID)
    End Function

    Public Function GetFeedbackStatus(Optional ByVal RetrievalValue As Integer = 0) As DataTable
        Return Claims_BL.GetFeedbackStatus(RetrievalValue)
    End Function
    Public Function GetReviewStatus(Optional ByVal RetrievalValue As Integer = 0) As DataTable
        Return Claims_BL.GetReviewStatus(RetrievalValue)
    End Function

    Public Function GetClaimStatus(Optional ByVal RetrievalValue As Integer = 0) As DataTable
        Return Claims_BL.GetClaimStatus(RetrievalValue)
    End Function
    
    Public Function IsClaimStatusChanged(ByRef eClaim As IMIS_EN.tblClaim) As Boolean
        Return Claims_BL.IsClaimStatusChanged(eClaim)
    End Function
    Public Sub WriteToXml(ByVal ClaimID As Integer)
        Claims_BL.WriteToXml(ClaimID)
    End Sub

    Public Sub ZipXMLs()
        Claims_BL.ZipXMLs()
    End Sub
    Public Function GetVisitTypes(Optional ByVal ShowSelect As Boolean = False) As DataTable
        Dim Clm As New IMIS_BL.ClaimsBL
        Return Clm.GetVisitTypes(ShowSelect)
    End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect)
    End Function
    Public Function getAutoCompleteICDCodes(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim GetDataTable As New IMIS_BL.ICDCodesBL
        Return GetDataTable.getAutoCompleteICDCodes
    End Function
End Class
