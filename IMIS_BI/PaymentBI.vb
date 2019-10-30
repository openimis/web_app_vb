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

Public Class PaymentBI
    Public Function getPayment(PaymentId As String) As IMIS_EN.tblPayment
        Dim PaymentBL As New IMIS_BL.PaymentBL
        Return PaymentBL.getPayment(PaymentId)
    End Function
    Public Function getPayment(ePayment As IMIS_EN.tblPayment) As DataTable
        Dim PaymentBL As New IMIS_BL.PaymentBL
        Return PaymentBL.GetPayment(ePayment)
    End Function

    Public Function LoadPayment(ByRef ePayment As IMIS_EN.tblPayment) As DataTable
        Dim PaymentBL As New IMIS_BL.PaymentBL
        Return PaymentBL.LoadPayment(ePayment)
    End Function

    Public Function GetPayementStatus(Optional Include As Boolean = False) As DataTable
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetPaymentStatus(Include)
    End Function
    Public Function GetPaymentStatusNames()
        Dim gen As New IMIS_BL.GeneralBL
        Return gen.GetPaymentStatusNames()
    End Function

    Public Function GetDistricts(ByVal userId As Integer, Optional ByVal showSelect As Boolean = False, Optional ByVal RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userId, True, RegionId)
    End Function

    Public Function GetRegions(UserId As Integer, ShowSelect As Boolean, IncludeNational As Boolean) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect, IncludeNational)
    End Function
    Public Function LoadPaymentDetails(ePayment As IMIS_EN.tblPayment, ByVal PaymentDetails As Integer)
        Dim BL As New IMIS_BL.PaymentBL
        Return BL.LoadPaymentDetails(ePayment, PaymentDetails)
    End Function
    Public Function MatchPayment(ByVal PaymentID As Integer, ByVal AuditUserID As Integer) As Boolean
        Dim Payment As New IMIS_BL.PaymentBL
        Return Payment.MatchPaymentAPI(PaymentID, AuditUserID)
    End Function
    Public Sub SaveEditedPaymentDetails(ePaymentDetails As IMIS_EN.tblPaymentDetail)
        Dim Payment As New IMIS_BL.PaymentBL
        Payment.SaveEditedPaymentDetails(ePaymentDetails)
    End Sub
    Public Function CheckCHFID(ByVal CHFID As String) As Boolean
        Dim insuree As New IMIS_BL.EscapeBL
        Return insuree.isValidInsuranceNumber(CHFID)
    End Function
    Public Function GetProducts(ByVal UserId As Integer, Optional ByVal ShowSelect As Boolean = False, Optional ByVal RegionId As Integer = 0, Optional ByVal DistrictID As Integer = 0) As DataTable
        Dim getDataTable As New IMIS_BL.ProductsBL
        Return getDataTable.GetProducts(UserId, ShowSelect, RegionId, DistrictID)
    End Function
    Public Function GetPolicyType() As DataTable
        Dim BLPolicy As New IMIS_BL.PolicyBL
        Return BLPolicy.GetPolicyType()
    End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect)
    End Function
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Dim UserRights As New IMIS_BL.UsersBL
        Return UserRights.CheckRights(Right, UserID)
    End Function

    Public Function RunPageSecurity(ByVal PageName As IMIS_EN.Enums.Pages, ByRef PageObj As System.Web.UI.Page) As Boolean
        Dim user As New IMIS_BL.UsersBL
        Return user.RunPageSecurity(PageName, PageObj)
    End Function
End Class
