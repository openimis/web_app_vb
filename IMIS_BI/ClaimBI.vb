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

Public Class ClaimBI
    Dim Claim As New IMIS_BL.ClaimsBL
    Public Sub LoadClaim(ByRef eClaim As IMIS_EN.tblClaim)
        Claim.LoadClaim(eClaim)
    End Sub
    Public Function getClaimServiceAndItems(ByVal ClaimID As Integer) As DataSet
        Return Claim.ReviewClaim(ClaimID)
    End Function
    Public Function GetItemServiceStatus(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim General As New IMIS_BL.GeneralBL
        Return General.GetItemServiceStatus
    End Function
    Public Function GetServiceCode(ByVal HFID As Integer) As DataTable
        Dim ServCode As New IMIS_BL.ServicesBL
        Return ServCode.GetServiceCode(HFID)
    End Function
    Public Function GetItemCode(ByVal HFID As Integer) As DataTable
        Dim ItemCode As New IMIS_BL.ItemsBL
        Return ItemCode.GetItemCode(HFID)
    End Function
    Public Function verifyCHFIDandReturnName(ByVal chfid As String, ByRef insureeid As Integer) As String
        Dim insuree As New IMIS_BL.InsureeBL
        Return insuree.verifyCHFIDandReturnName(chfid, insureeid)
    End Function
    Public Function GetHFType(Optional ByVal showSelect As Boolean = False, Optional ByVal InOutOnly As Boolean = False) As DataTable
        Dim HF As New IMIS_BL.HealthFacilityBL
        Return HF.GetHFType(showSelect, InOutOnly)
    End Function
    Public Function SaveClaim(ByRef eClaim As IMIS_EN.tblClaim) As Integer
        Dim claim As New IMIS_BL.ClaimsBL
        Return claim.SaveClaim(eClaim)
    End Function
    Public Sub UpdateClaimTotalValue(ByRef eClaim As IMIS_EN.tblClaim)
        Dim claim As New IMIS_BL.ClaimsBL
        claim.UpdateClaimTotalValue(eClaim)
    End Sub
    Public Function checkClaimCode(ByVal eClaim As IMIS_EN.tblClaim) As Boolean
        Return Claim.checkClaimCode(eClaim)
    End Function
    Public Function GetHFCodes(ByVal UserId As Integer, ByVal LocationId As Integer) As DataTable
        Dim hf As New IMIS_BL.HealthFacilityBL
        Return hf.GetHFCodes(UserId, LocationId)
    End Function
    Public Function GetICDCodes(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim ICDCodes As New IMIS_BL.ICDCodesBL
        Return ICDCodes.GetICDCodes(showSelect)
    End Function
    Public Function SaveClaimServices(ByRef eClaimServices As IMIS_EN.tblClaimServices) As Integer
        Dim claimServices As New IMIS_BL.ClaimServicesBL
        Return claimServices.SaveClaimServices(eClaimServices)
    End Function
    Public Function SaveClaimItems(ByRef eClaimItems As IMIS_EN.tblClaimItems) As Integer
        Dim claimItems As New IMIS_BL.ClaimItemsBL
        Return claimItems.SaveClaimItems(eClaimItems)
    End Function
    Public Sub DeleteClaimService(ByRef eClaimService As IMIS_EN.tblClaimServices)
        Dim claimServices As New IMIS_BL.ClaimServicesBL

        claimServices.DeleteClaimService(eClaimService)
    End Sub
    Public Sub DeleteClaimItems(ByRef eClaimItem As IMIS_EN.tblClaimItems)
        Dim claimItems As New IMIS_BL.ClaimItemsBL

        claimItems.DeleteClaimItems(eClaimItem)
    End Sub
    Public Function IsClaimStatusChanged(ByRef eClaim As IMIS_EN.tblClaim) As Boolean
        Return Claim.IsClaimStatusChanged(eClaim)
    End Function
    Public Sub getHFCodeAndName(ByRef eHF As IMIS_EN.tblHF)
        Dim hf As New IMIS_BL.HealthFacilityBL
        hf.getHFCodeAndName(eHF)
    End Sub
    Public Sub GetClaimAdminDetails(ByRef eClaimAdmin As IMIS_EN.tblClaimAdmin)
        Dim BLClaimAdmin As New IMIS_BL.ClaimAdminBL
        BLClaimAdmin.LoadClaimAdmin(eClaimAdmin)
    End Sub
    Public Function GetVisitTypes(Optional ByVal ShowSelect As Boolean = False) As DataTable
        Dim Clm As New IMIS_BL.ClaimsBL
        Return Clm.GetVisitTypes(ShowSelect)
    End Function
    Public Function GetClaim(ByVal ClaimID As Integer) As DataTable
        Return Claim.GetClaim(ClaimID)
    End Function
    Public Function GetInsureeByCHFIDGrid(ByVal CHFID As String) As DataTable
        Dim Policy As New IMIS_BL.PolicyBL
        Return Policy.FindInsureeByCHFIDGrid(CHFID)
    End Function
    Public Function GetClaimIdByUUID(ByVal uuid As Guid) As Integer
        Dim Claim As New IMIS_BL.ClaimsBL
        Return Claim.GetClaimIdByUUID(uuid)
    End Function
    Public Function GetClaimUUIDByID(ByVal id As Integer) As Guid
        Dim Claim As New IMIS_BL.ClaimsBL
        Return Claim.GetClaimUUIDByID(id)
    End Function
    Public Function getLastVisitDays(ByVal chfid As String, ByVal hfid As Integer) As String
        Dim insuree As New IMIS_BL.InsureeBL
        Return insuree.getLastVisitDays(chfid, hfid)
    End Function
End Class
