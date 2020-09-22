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

Public Class PolicyBL
    Private imisgen As New GeneralBL
    Public Function GetPolicybyFamily(ByVal FamilyId As Integer) As DataTable
        Dim Policy As New IMIS_DAL.PolicyDAL
        Return Policy.GetPolicybyFamily(FamilyId, GetPolicyStatus)
    End Function
    Public Function GetOfficers() As DataTable
        Dim Officer As New IMIS_DAL.PolicyDAL
        Return Officer.GetOfficers
    End Function
    Public Sub LoadPolicy(ByRef ePolicy As IMIS_EN.tblPolicy, ByRef PremiumPaid As Decimal)
        Dim Policy As New IMIS_DAL.PolicyDAL
        Policy.LoadPolicy(ePolicy, PremiumPaid)
    End Sub

    Public Function SavePolicy(ByRef ePolicy As IMIS_EN.tblPolicy, Optional ByVal IsOffLine As Boolean = False) As Integer
        Dim Policy As New IMIS_DAL.PolicyDAL
        If ePolicy.PolicyID = 0 Then
            Policy.InsertPolicy(ePolicy)
            InsertInsureePolicy(ePolicy.PolicyID, IsOffLine)
            Return 0
        Else
            Policy.UpdatePolicy(ePolicy)
            Return 1
        End If
    End Function

    Public Sub InsertInsureePolicy(ByVal PolicyId As Integer, Optional ByVal IsOffLine As Boolean = False)
        Dim PI As New IMIS_BL.InsureePolicyBL
        PI.InsertInsureePolicy(PolicyId, IsOffLine)
    End Sub
    Public Sub InsertInsureePolicy(ByVal PolicyId As Integer)
        Dim PI As New IMIS_BL.InsureePolicyBL
        PI.InsertInsureePolicy(PolicyId)
    End Sub
    Public Sub getPolicyValue(ByRef ePolicy As IMIS_EN.tblPolicy, PreviousPolicyId As Integer)
        Dim Policy As New IMIS_DAL.PolicyDAL
        Policy.getPolicyValue(ePolicy, PreviousPolicyId)
        'If ePolicy.PolicyStage = "R" Then
        'ePolicy.PolicyValue = ePolicy.PolicyValue - GetRegistrationFee(ePolicy.PolicyID)
        'End If
    End Sub
    Public Function GetPolicy(ByRef ePolicy As IMIS_EN.tblPolicy, ByVal All As Boolean, Optional ByVal DeactivatedPolicies As Boolean = False) As DataTable
        Dim Policy As New IMIS_DAL.PolicyDAL
        'If ePolicy.StartDateFrom = Nothing Then ePolicy.StartDateFrom = System.Data.SqlTypes.SqlDateTime.MinValue.Value
        'If ePolicy.ExpiryDateFrom Is Nothing Then ePolicy.ExpiryDateFrom = System.Data.SqlTypes.SqlDateTime.MinValue.Value
        'If ePolicy.EnrollDateFrom = Nothing Then ePolicy.EnrollDateFrom = System.Data.SqlTypes.SqlDateTime.MinValue.Value
        'If ePolicy.EffectiveDateFrom Is Nothing Then ePolicy.EffectiveDateFrom = System.Data.SqlTypes.SqlDateTime.MinValue.Value

        'If ePolicy.StartDateTo = Nothing Then ePolicy.StartDateTo = System.Data.SqlTypes.SqlDateTime.MaxValue.Value
        'If ePolicy.ExpiryDateTo Is Nothing Then ePolicy.ExpiryDateTo = System.Data.SqlTypes.SqlDateTime.MaxValue.Value
        'If ePolicy.EnrollDateTo = Nothing Then ePolicy.EnrollDateTo = System.Data.SqlTypes.SqlDateTime.MaxValue.Value
        'If ePolicy.EffectiveDateTo Is Nothing Then ePolicy.EffectiveDateTo = System.Data.SqlTypes.SqlDateTime.MaxValue.Value
        '   If ePolicy.PolicyValue Is Nothing Then ePolicy.PolicyValue = 0 'System.Data.SqlTypes.SqlDecimal.MaxValue.Value

        Return Policy.GetPolicy(ePolicy, All, GetPolicyStatus, DeactivatedPolicies)
    End Function
    Public Function FindInsureeByCHFIDGrid(ByVal CHFId As String)
        Dim Policy As New IMIS_DAL.PolicyDAL
        Dim dt As DataTable = Policy.FindInsureeByCHFIDGrid(CHFId)
        dt.Columns.Add("PolicyStatus", GetType(String)).AllowDBNull = True
        Dim s As Integer
        For s = 0 To dt.Rows.Count - 1
            dt.Rows(s)("PolicyStatus") = ReturnPolicyStatusALPHA(dt.Rows(s)("Status"))
        Next
        Return dt
    End Function
    Public Function ReturnPolicyStatusALPHA(ByVal PolicyStatus As String) As String
        Select Case PolicyStatus
            Case "I" : Return imisgen.getMessage("T_IDLE")
            Case "R" : Return imisgen.getMessage("T_READY")
            Case "A" : Return imisgen.getMessage("T_ACTIVE")
            Case "S" : Return imisgen.getMessage("T_SUSPENDED")
            Case "E" : Return imisgen.getMessage("T_EXPIRED")
            Case Else : Return ""
        End Select
    End Function
    Public Function ReturnPolicyStatus(ByVal PolicyStatus As Integer) As String
        Select Case PolicyStatus
            Case 1 : Return imisgen.getMessage("T_IDLE")
            Case 16 : Return imisgen.getMessage("T_READY")
            Case 2 : Return imisgen.getMessage("T_ACTIVE")
            Case 4 : Return imisgen.getMessage("T_SUSPENDED")
            Case 8 : Return imisgen.getMessage("T_EXPIRED")
            Case Else : Return ""
        End Select
    End Function
    Public Function GetPolicyStatus(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim dtbl As New DataTable
        Dim dr As DataRow
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("Status")
        If showSelect = True Then
            dr = dtbl.NewRow
            dr("Code") = 0
            dr("Status") = imisgen.getMessage("T_SELECTSTATUS")
            dtbl.Rows.Add(dr)
        End If

        dr = dtbl.NewRow
        dr("Code") = 1
        dr("Status") = ReturnPolicyStatus(1)
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = 16
        dr("Status") = ReturnPolicyStatus(16)
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = 2
        dr("Status") = ReturnPolicyStatus(2)
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = 4
        dr("Status") = ReturnPolicyStatus(4)
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = 8
        dr("Status") = ReturnPolicyStatus(8)
        dtbl.Rows.Add(dr)

        Return dtbl
    End Function
    Public Function DeletePolicy(ByVal ePolicy As IMIS_EN.tblPolicy) As Integer
        Dim policy As New IMIS_DAL.PolicyDAL

        Dim dt As DataTable = policy.CheckCanBeDeleted(ePolicy.PolicyID)
        If dt.Rows.Count > 0 Then Return 2

        If policy.DeletePolicy(ePolicy) Then
            DeletePolicyInsuree(ePolicy.PolicyID)
            Return 1
        Else
            Return 0
        End If
    End Function
    Public Sub DeletePolicyInsuree(ByVal PolicyId As Integer)
        Dim PI As New IMIS_BL.InsureePolicyBL
        PI.DeletePolicyInsuree(0, PolicyId)
    End Sub
    Public Function GetPolicyOfflineValue(ByVal PolicyId As Integer) As Boolean
        Dim Policy As New IMIS_DAL.PolicyDAL
        Return Policy.GetPolicyOfflineValue(PolicyId)
    End Function
    Public Function GetRegistrationFee(ByVal PolicyId As Integer) As Double
        Dim Prod As New IMIS_BL.ProductsBL
        Return Prod.GetRegistrationFee(PolicyId)
    End Function
    Public Function GetPolicyType() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("TypeId")
        dt.Columns.Add("Type")
        Dim dr As DataRow

        dr = dt.NewRow
        dr("TypeId") = ""
        dr("Type") = imisgen.getMessage("T_SELECTTYPE")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("TypeId") = "N"
        dr("Type") = imisgen.getMessage("T_NEWPOLICIES")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("TypeId") = "R"
        dr("Type") = imisgen.getMessage("T_RENEWAL")
        dt.Rows.Add(dr)

        Return dt

    End Function
    Public Function isExceededAdherents(ByVal ProdId As Integer, ByVal FamilyId As Integer) As Boolean
        Dim Pol As New IMIS_DAL.PolicyDAL
        Return Pol.isExceededAdherents(ProdId, FamilyId)
    End Function
    Public Function IsRenewalLate(ByVal PolicyID As Integer, ByVal EnrolDate As Date) As Boolean
        Dim DALPolicy As New IMIS_DAL.PolicyDAL
        Dim _date As Nullable(Of Date) = DALPolicy.GetLastRenewalDate(PolicyID)
        If _date Is Nothing Then Return False
        If _date < EnrolDate Then Return True
        Return False
    End Function
    Public Function GetPolicyIdByUUID(ByVal uuid As Guid) As Integer
        Dim Policy As New IMIS_DAL.PolicyDAL
        Return Policy.GetPolicyIdByUUID(uuid).Rows(0).Item(0)
    End Function
    Public Function GetPolicyUUIDByID(ByVal id As Integer) As Guid
        Dim Policy As New IMIS_DAL.PolicyDAL
        Return Policy.GetPolicyUUIDByID(id).Rows(0).Item(0)
    End Function

    Public Function GetRenewalCount(ByVal ProdID As Integer, ByVal FamilyID As Integer) As Integer
        Dim PolicyDAL As New IMIS_DAL.PolicyDAL
        Return PolicyDAL.GetRenewalCount(ProdID, FamilyID)
    End Function
End Class
