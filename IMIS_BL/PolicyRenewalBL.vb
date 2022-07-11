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

Public Class PolicyRenewalBL
    Dim imisgen As New GeneralBL
    Public Sub UpdatePolicyRenewal(RemindingInterval As Integer?, RegionId As Integer?, DistrictId As Integer?, WardId As Integer?, VillageId As Integer?, OfficerId As Integer?, DateFrom As Date?, DateTo As Date?)
        Dim Update As New IMIS_DAL.PolicyRenewalDAL
        Update.UpdatePolicyRenewal(RemindingInterval, RegionId, DistrictId, WardId, VillageId, OfficerId, DateFrom, DateTo)
    End Sub
    Public Function GetPolicyRenewal(ByVal RangeFrom As DateTime, ByVal RangeTo As DateTime, ByVal OfficerID As Integer, ByVal RegionId As Integer, ByVal District As Integer, ByVal Village As Integer, ByVal Ward As Integer, ByVal PolicyStatus As Integer) As DataTable
        Dim Policy As New IMIS_DAL.PolicyRenewalDAL
        Return Policy.GetPolicyStatus(RangeFrom, RangeTo, OfficerID, RegionId, District, Village, Ward, PolicyStatus)
    End Function
    

    Public Function GetPolicyPromptJournal(ByVal RangeFrom As DateTime, ByVal RangeTo As DateTime, ByVal OfficerID As Integer, ByVal District As Integer, ByVal Village As Integer, ByVal Ward As Integer, ByVal SMSStatus As Integer, ByVal IntervalType As Integer) As DataTable
        Dim Policy As New IMIS_DAL.PolicyRenewalDAL
        Return Policy.GetPolicyPromptJournal(RangeFrom, RangeTo, OfficerID, District, Village, Ward, SMSStatus, IntervalType)
    End Function
    Public Function GetSMSStatus() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("Status")
        dt.Columns.Add("Value")
        Dim dr As DataRow

        dr = dt.NewRow
        dr("Status") = imisgen.getMessage("T_SELECTSTATUS")
        dr("Value") = -1
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("Status") = imisgen.getMessage("T_DELIVERED")
        dr("Value") = 0
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("Status") = imisgen.getMessage("T_FAILED")
        dr("Value") = 1
        dt.Rows.Add(dr)

        Return dt
    End Function

    Public Function GetJournalOn() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("State")
        dt.Columns.Add("Value")
        Dim dr As DataRow

        dr = dt.NewRow
        dr("State") = imisgen.getMessage("T_PROMPT")
        dr("Value") = 1
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("State") = imisgen.getMessage("T_EXPIRY")
        dr("Value") = 2
        dt.Rows.Add(dr)

        Return dt
    End Function
    Public Function getPolicyRenewalSMSData(ByVal DateFrom As Date, ByVal DateTo As Date) As DataTable
        Dim Policy As New IMIS_DAL.PolicyRenewalDAL
        Return Policy.getPolicyRenewalSMSData(DateFrom, DateTo)

    End Function
    Public Function sendSMS(ByVal DateFrom As Date, ByVal DateTo As Date) As String
        Dim Policy As New IMIS_DAL.PolicyRenewalDAL

        'Dim BLGeneral As New GeneralBL
        'Return BLGeneral.ReadSMSDatatable(Policy.getPolicyRenewalSMSData(DateFrom, DateTo))

        Dim Esc As New EscapeBL
        Dim Message As String = Policy.getPolicyRenewalSMSData(DateFrom, DateTo).Rows(0)(0).ToString
        Return Esc.SendSMS(Message)

    End Function
End Class
