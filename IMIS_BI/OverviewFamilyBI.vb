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

Public Class OverviewFamilyBI
    Public UserRights As New IMIS_BL.UsersBL
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Return UserRights.CheckRights(Right, UserID)
    End Function
    Public Function GetInsureesByFamilyFiltered(ByVal FamilyId As Integer, Optional Language As String = "en") As DataTable
        Dim Insurees As New IMIS_BL.InsureeBL
        Return Insurees.GetInsureesByFamily(FamilyId, Language)
    End Function
    Public Function GetPolicybyFamily(ByVal FamilyId As Integer) As DataTable
        Dim Policy As New IMIS_BL.PolicyBL
        Return Policy.GetPolicybyFamily(FamilyId)
    End Function
    Public Function GetPremiumsByPolicy(ByVal PolicyId As Integer) As DataTable
        Dim premiums As New IMIS_BL.PremiumBL
        Return premiums.GetPremiumsByPolicy(PolicyId)
    End Function
    Public Sub GetFamilyHeadInfo(ByVal eFamily As IMIS_EN.tblFamilies)
        Dim getHeadInfo As New IMIS_BL.FamilyBL
        getHeadInfo.GetFamilyHeadInfo(eFamily)
    End Sub
    Public Function DeleteInsuree(ByVal eInsuree As IMIS_EN.tblInsuree) As Integer
        Dim insuree As New IMIS_BL.InsureeBL
        Return insuree.DeleteInsuree(eInsuree)
    End Function
    Public Function DeletePolicy(ByVal ePolicy As IMIS_EN.tblPolicy) As Integer
        Dim policy As New IMIS_BL.PolicyBL
        Return policy.DeletePolicy(ePolicy)
    End Function

    Public Function DeletePremium(ByVal epremium As IMIS_EN.tblPremium) As Integer
        Dim premium As New IMIS_BL.PremiumBL
        Return premium.DeletePremium(epremium)
    End Function
    Public Function DeleteFamily(ByVal eFamily As IMIS_EN.tblFamilies) As Integer
        Dim family As New IMIS_BL.FamilyBL
        Return family.DeleteFamily(eFamily)
    End Function

    Public Sub GetPolicyValue(ByRef ePolicy As IMIS_EN.tblPolicy, PreviousPolicyId As Integer)
        Dim Policy As New IMIS_BL.PolicyBL
        Policy.getPolicyValue(ePolicy, PreviousPolicyId)
    End Sub
    Public Function SavePolicy(ByRef ePolicy As IMIS_EN.tblPolicy) As Integer
        Dim Policy As New IMIS_BL.PolicyBL
        Return Policy.SavePolicy(ePolicy)
    End Function
    Public Function ReturnPolicyStatus(ByVal PolicyStatus As Integer) As String
        Dim BLPolicy As New IMIS_BL.PolicyBL
        Return BLPolicy.ReturnPolicyStatus(PolicyStatus)
    End Function
    Public Function GetRenewalCount(ByVal ProdID As Integer, ByVal FamilyID As Integer) As Integer
        Dim PolicyBL As New IMIS_BL.PolicyBL
        Return PolicyBL.GetRenewalCount(ProdID, FamilyID)
    End Function
End Class

