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

Public Class ClaimReviewBI
   
    Public Function GetClaimStatus(Optional ByVal RetrievalValues As Integer = 15) As DataTable
        Dim claim As New IMIS_BL.ClaimsBL
        Return claim.GetClaimStatus(RetrievalValues)
    End Function
    Public Sub LoadClaim(ByRef eClaim As IMIS_EN.tblClaim, Optional ByRef eExtra As Dictionary(Of String, Object) = Nothing)
        Dim claim As New IMIS_BL.ClaimsBL
        claim.LoadClaim(eClaim, eExtra)
    End Sub
    Public Function GetItemServiceStatus(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim General As New IMIS_BL.GeneralBL
        Return General.GetItemServiceStatus
    End Function
    Public Function getClaimServiceAndItems(ByVal ClaimID As Integer) As DataSet
        Dim claim As New IMIS_BL.ClaimsBL
        Return claim.ReviewClaim(ClaimID)
    End Function
    Public Function IsItSystemRejectedItem(ByVal eClaimReviewItem As IMIS_EN.tblClaimItems) As Boolean
        Dim claimItems As New IMIS_BL.ClaimItemsBL
        Return claimItems.IsItSystemRejected(eClaimReviewItem)
    End Function
    Public Function IsItSystemRejectedService(ByVal eClaimReviewServices As IMIS_EN.tblClaimServices) As Boolean
        Dim claimItems As New IMIS_BL.ClaimServicesBL
        Return claimItems.IsItSystemRejected(eClaimReviewServices)
    End Function
    
    Public Function SaveClaimServicesforReview(ByRef eClaimReviewServices As IMIS_EN.tblClaimServices) As Boolean
        Dim claimServices As New IMIS_BL.ClaimServicesBL
        Return claimServices.SaveClaimServicesforReview(eClaimReviewServices)
    End Function
    Public Function SaveClaimReview(ByRef eClaim As IMIS_EN.tblClaim) As Boolean
        Dim claim As New IMIS_BL.ClaimsBL
        Return claim.UpdateClaimReview(eClaim)
    End Function
    Public Sub UpdateClaimApprovedValue(ByRef eClaim As IMIS_EN.tblClaim)
        Dim claim As New IMIS_BL.ClaimsBL
        claim.UpdateClaimApprovedValue(eClaim)
    End Sub
    Public Function IsClaimReviewStatusChanged(ByVal eClaim As IMIS_EN.tblClaim) As Boolean
        Dim claim As New IMIS_BL.ClaimsBL
        Return claim.IsClaimReviewStatusChanged(eClaim)
    End Function
    Public Function IsClaimStatusChecked(ByVal eClaim As IMIS_EN.tblClaim) As Boolean
        Dim claim As New IMIS_BL.ClaimsBL
        Return claim.IsClaimStatusChecked(eClaim)
    End Function
    Public Function SaveClaimItemsforReview(ByRef eClaimReviewItem As IMIS_EN.tblClaimItems) As Boolean
        Dim claimItems As New IMIS_BL.ClaimItemsBL
        Return claimItems.SaveClaimItemsforReview(eClaimReviewItem)
    End Function
    Public Function ReturnClaimStatus(ByVal claimstatus As Integer) As String
        Dim status As New IMIS_BL.ClaimsBL
        Return status.ReturnClaimStatus(claimstatus)
    End Function
    Public Function GetVisitTypeText(ByVal VisitTypeCode As Char) As String
        Dim Clm As New IMIS_BL.ClaimsBL
        Return Clm.GetVisitTypeText(VisitTypeCode)
    End Function
    Public Function GetServiceRejectedReason(ByVal ReasonId As Integer) As String
        Dim Clm As New IMIS_BL.ClaimsBL
        Return Clm.GetServiceRejectedReason(ReasonId)
    End Function
    Public Function GetItemRejectedReason(ByVal ReasonId As Integer) As String
        Dim Clm As New IMIS_BL.ClaimsBL
        Return Clm.GetItemRejectedReason(ReasonId)
    End Function
    ' Get Last Visit Days Added By Purushottam
    Public Function getLastVisitDaysForReview(ByVal chfid As String, ByVal claimid As Integer, ByVal VISITDATETO As Date) As DataTable
        Dim insuree As New IMIS_BL.InsureeBL
        Return insuree.getLastVisitDaysForReview(chfid, claimid, VISITDATETO)
    End Function
    ' Get Last Visit Days Added By Purushottam
End Class
