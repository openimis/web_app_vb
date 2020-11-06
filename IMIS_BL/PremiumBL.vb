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

Public Class PremiumBL
    Private imisgen As New GeneralBL

    Public Sub LoadPremium(ByRef ePremium As IMIS_EN.tblPremium, ByRef PremiumContribution As Decimal)
        Dim Premium As New IMIS_DAL.PremiumDAL
        Premium.LoadPremium(ePremium, PremiumContribution)
    End Sub
    Public Function GetPremium(ByVal ePremium As IMIS_EN.tblPremium) As DataTable
        Dim getDataTable As New IMIS_DAL.PremiumDAL
        Return getDataTable.GetPremium(ePremium)


    End Function
    Public Function GetPremiumsByPolicy(ByVal PolicyId As Integer) As DataTable
        Dim premiums As New IMIS_DAL.PremiumDAL
        Return premiums.GetPremiumsByPolicy(PolicyId)
    End Function
    Public Function GetPayType(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim dtbl As New DataTable
        Dim dr As DataRow
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("PayType")
        If showSelect = True Then
            dr = dtbl.NewRow
            dr("Code") = ""
            dr("PayType") = imisgen.getMessage("T_PAYMENTTYPE")
            dtbl.Rows.Add(dr)
        End If
        dr = dtbl.NewRow
        dr("Code") = "C"
        dr("PayType") = imisgen.getMessage("T_CASH")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = "B"
        dr("PayType") = imisgen.getMessage("T_BANKTRANSFER")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = "M"
        dr("PayType") = imisgen.getMessage("T_MOBILEPHONE")
        dtbl.Rows.Add(dr)

        Return dtbl
    End Function
    Public Function SavePremium(ByRef ePremium As IMIS_EN.tblPremium, ByVal IsOffline As Boolean) As Integer '1 updated 0 inserted 2 policyId not existing
        Dim Premium As New IMIS_DAL.PremiumDAL
        Dim policy As New IMIS_DAL.PolicyDAL
        Dim res As Integer

        Dim dt = policy.CheckIfPolicyIDExists(ePremium.tblPolicy.PolicyID)
        If Not dt.Rows.Count > 0 Then
            Return 2
        End If


        If ePremium.PremiumId = 0 Then
            If Premium.InsertPremium(ePremium) Then
                res = 0
            End If
        Else
            If Premium.UpdatePremium(ePremium) Then
                res = 1
            End If
        End If

        'Amani & Hiren 11/05

        If ePremium.tblPolicy.PolicyStatus = 4 Then
            policy.UpdatePolicy(ePremium.tblPolicy)
            Return res
        End If




        If Not ePremium.tblPolicy.PolicyStatus Is Nothing And ((ePremium.tblPolicy.EffectiveDate = ePremium.PayDate) Or (ePremium.tblPolicy.EffectiveDate = ePremium.tblPolicy.StartDate)) Then 'should execute only in inforcing the policy
            If Not ePremium.tblPolicy.isOffline Then
                If Not IsOffline Then
                    policy.UpdatePolicy(ePremium.tblPolicy)
                End If
            Else
                policy.UpdatePolicy(ePremium.tblPolicy)
            End If
            If ePremium.tblPolicy.PolicyStatus = 2 Then UpdatePolicyInsuree(ePremium.tblPolicy.PolicyID)
        ElseIf ePremium.tblPolicy.EffectiveDate IsNot Nothing Then 'when activating the insuree
            ActivateInsuree(ePremium.tblPolicy.PolicyID, ePremium.PayDate)
        End If
        Return res
    End Function
    Public Sub UpdatePolicyInsuree(ByVal PolicyId As Integer)
        Dim PI As New IMIS_BL.InsureePolicyBL
        PI.UpdatePolicyInsuree(PolicyId)
    End Sub
    Public Sub ActivateInsuree(ByVal PolicyId As Integer, ByVal EffectiveDate As Date)
        Dim IP As New IMIS_BL.InsureePolicyBL
        IP.ActivateInsuree(PolicyId, EffectiveDate)
    End Sub
    Public Function DeletePremium(ByVal epremium As IMIS_EN.tblPremium) As Integer
        Dim premium As New IMIS_DAL.PremiumDAL

        'Dim dt As DataTable = premium.CheckCanBeDeleted(epremium.PremiumId)
        'If dt.Rows.Count > 0 Then Return 2

        If premium.DeletePremium(epremium) Then
            Return 1
        Else
            Return 0
        End If
    End Function
    Public Function FindPremium(ByRef ePremium As IMIS_EN.tblPremium, Optional ByVal All As Boolean = False)

        Dim getDataTable As New IMIS_DAL.PremiumDAL
        Dim dtPaymentType As DataTable = GetPayType()
        Dim dtPayCategory As DataTable = GetCategory()

        If Not dtPaymentType.Columns.Contains("AltLanguage") Then dtPaymentType.Columns.Add("AltLanguage")
        If Not dtPayCategory.Columns.Contains("AltLanguage") Then dtPayCategory.Columns.Add("AltLanguage")

        Return getDataTable.GetPremiums(ePremium, All, dtPaymentType, dtPayCategory)


    End Function
    Public Sub GetPremiumContribution(ByRef ePremium As IMIS_EN.tblPremium, ByRef PremiumContribution As Decimal)
        Dim premium As New IMIS_DAL.PremiumDAL

        premium.GetPremiumContribution(ePremium, PremiumContribution)

    End Sub
    Public Function GetCategory() As DataTable
        Dim Gen As New GeneralBL
        Dim dt As New DataTable
        dt.Columns.Add("CategoryId")
        dt.Columns.Add("Category")
        Dim dr As DataRow

        dr = dt.NewRow
        dr("CategoryId") = ""
        dr("Category") = Gen.getMessage("L_SELECTCATEGORY")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("CategoryId") = "C"
        dr("Category") = Gen.getMessage("T_CONTRIBUTION")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("CategoryId") = "P"
        dr("Category") = Gen.getMessage("T_PHOTOFEE")
        dt.Rows.Add(dr)

        Return dt
    End Function
    Public Function GetLastDateForPayment(ByVal PolicyId As Integer) As Date
        Dim Premium As New IMIS_DAL.PremiumDAL
        Return Premium.GetLastDateForPayment(PolicyId)
    End Function
    Public Function GetInstallmentsInfo(ByVal PolicyId As Integer) As DataTable
        Dim Premium As New IMIS_DAL.PremiumDAL
        Return Premium.GetInstallmentsInfo(PolicyId)
    End Function
    Public Function isUniqueReceipt(ByVal ePremium As IMIS_EN.tblPremium) As Boolean
        Dim Pr As New IMIS_DAL.PremiumDAL
        Return Pr.isUniqueReceipt(ePremium)
    End Function
    Public Function AddFund(ePremium As IMIS_EN.tblPremium, ProdId As Integer) As Integer
        Dim Fund As New IMIS_DAL.PremiumDAL
        Return Fund.AddFund(ePremium, ProdId)
    End Function
    Public Function GetPremiumIdByUUID(ByVal uuid As Guid) As Integer
        Dim Premium As New IMIS_DAL.PremiumDAL
        Return Premium.GetPremiumIdByUUID(uuid).Rows(0).Item(0)
    End Function
    Public Function GetPremiumUUIDByID(ByVal id As Integer) As Guid
        Dim Premium As New IMIS_DAL.PremiumDAL
        Return Premium.GetPremiumUUIDByID(id).Rows(0).Item(0)
    End Function
End Class
