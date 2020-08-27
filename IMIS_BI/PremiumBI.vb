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

Public Class PremiumBI

    Public Sub LoadPremium(ByRef ePremium As IMIS_EN.tblPremium, ByRef PremiumContribution As Decimal)
        Dim Premium As New IMIS_BL.PremiumBL
        Premium.LoadPremium(ePremium, PremiumContribution)
    End Sub
    Public Sub GetFamilyHeadInfo(ByVal eFamily As IMIS_EN.tblFamilies)
        Dim getHeadInfo As New IMIS_BL.FamilyBL
        getHeadInfo.GetFamilyHeadInfo(eFamily)
    End Sub
    Public Function GetPayers(ByVal RegionId As Integer, ByVal DistrictId As Integer, ByVal Userid As Integer, ByVal showSelect As Boolean) As DataTable
        Dim Premimum As New IMIS_BL.PayersBL
        Return Premimum.GetPayers(RegionId, DistrictId, Userid, True)
    End Function
    Public Function GetTypeOfPayment(Optional ByVal showSelect As Boolean = True) As DataTable
        Dim getDataTable As New IMIS_BL.PremiumBL
        Return getDataTable.GetPayType(showSelect)
    End Function
    Public Function SavePremium(ByRef ePremium As IMIS_EN.tblPremium, ByVal IsOffline As Boolean) As Integer

        Dim Premium As New IMIS_BL.PremiumBL
        Return Premium.SavePremium(ePremium, IsOffline)

    End Function
    Public Sub GetPremiumContribution(ByRef ePremium As IMIS_EN.tblPremium, ByRef PremiumContribution As Decimal)
        Dim premium As New IMIS_BL.PremiumBL
        premium.GetPremiumContribution(ePremium, PremiumContribution)
    End Sub
    Public Function ReturnPolicyStatus(ByVal PolicyStatus As Integer) As String
        Dim Policy As New IMIS_BL.PolicyBL
        Return Policy.ReturnPolicyStatus(PolicyStatus)
    End Function
    Public Function GetPolicyOfflineValue(ByVal PolicyID As Integer) As Boolean
        Dim Policy As New IMIS_BL.PolicyBL
        Return Policy.GetPolicyOfflineValue(PolicyID)
    End Function
    Public Function GetCategory() As DataTable
        Dim Pre As New IMIS_BL.PremiumBL
        Return Pre.GetCategory
    End Function
    Public Function GetLastDateForPayment(ByVal PolicyId As Integer) As Date
        Dim Premium As New IMIS_BL.PremiumBL
        Return Premium.GetLastDateForPayment(PolicyId)
    End Function
    Public Function GetInstallmentsInfo(ByVal PolicyId As Integer) As DataTable
        Dim Premium As New IMIS_BL.PremiumBL
        Return Premium.GetInstallmentsInfo(PolicyId)
    End Function
    Public Function isUniqueReceipt(ByVal ePremium As IMIS_EN.tblPremium) As Boolean
        Dim Pr As New IMIS_BL.PremiumBL
        Return Pr.isUniqueReceipt(ePremium)
    End Function
    Public Function GetPremiumIdByUUID(ByVal uuid As Guid) As Integer
        Dim Premium As New IMIS_BL.PremiumBL
        Return Premium.GetPremiumIdByUUID(uuid)
    End Function
    Public Function GetPremiumnUUIDByID(ByVal id As Integer) As Guid
        Dim Premium As New IMIS_BL.PremiumBL
        Return Premium.GetPremiumUUIDByID(id)
    End Function
    Public Function GetPremium(ByVal ePremium As IMIS_EN.tblPremium) As DataTable
        Dim premium As New IMIS_BL.PremiumBL
        Return premium.GetPremium(ePremium)
    End Function
End Class
