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

Imports System.Net

Public Class PaymentBL

    Public Function GetPayment(ByRef ePayment As IMIS_EN.tblPayment) As DataTable
        Dim Payment As New IMIS_DAL.PaymentDAL
        Return Payment.GetPayment(ePayment)
    End Function
    Public Function getPayment(PaymentId As Integer) As IMIS_EN.tblPayment
        Dim PaymentDAL As New IMIS_DAL.PaymentDAL
        Dim dtStatus As DataTable
        Dim gen As New GeneralBL
        dtStatus = gen.GetPaymentStatus(False)
        Dim dt As DataTable = PaymentDAL.getPayment(PaymentId, dtStatus)
        If dt.Rows.Count > 0 Then
            Return SetPaymentEntity(dt.Rows(0))
        End If

    End Function
    Public Function SetPaymentEntity(dr As DataRow) As IMIS_EN.tblPayment
        Dim ePayment As New IMIS_EN.tblPayment

        ePayment.ControlNumber = If(dr.Table.Columns.Contains("ControlNumber") AndAlso Not dr.IsNull("ControlNumber"), dr("ControlNumber"), Nothing)
        ePayment.PaymentID = If(dr.Table.Columns.Contains("PaymentID") AndAlso Not dr.IsNull("PaymentID"), dr("PaymentID"), Nothing)
        ePayment.PaymentStatusName = If(dr.Table.Columns.Contains("PaymenyStatusName") AndAlso Not dr.IsNull("PaymenyStatusName"), dr("PaymenyStatusName"), Nothing)
        ePayment.PaymentOrigin = If(dr.Table.Columns.Contains("PaymentOrigin") AndAlso Not dr.IsNull("PaymentOrigin"), dr("PaymentOrigin"), Nothing)
        ePayment.ReceivedAmount = If(dr.Table.Columns.Contains("ReceivedAmount") AndAlso Not dr.IsNull("ReceivedAmount"), dr("ReceivedAmount"), Nothing)
        ePayment.ReceiptNo = If(dr.Table.Columns.Contains("ReceiptNo") AndAlso Not dr.IsNull("ReceiptNo"), dr("ReceiptNo"), Nothing)
        ePayment.PhoneNumber = If(dr.Table.Columns.Contains("PhoneNumber") AndAlso Not dr.IsNull("PhoneNumber"), dr("PhoneNumber"), Nothing)
        ePayment.MatchedDate = If(dr.Table.Columns.Contains("MatchedDate") AndAlso Not dr.IsNull("MatchedDate"), dr("MatchedDate"), Nothing)
        ePayment.ExpectedAmount = If(dr.Table.Columns.Contains("ExpectedAmount") AndAlso Not dr.IsNull("ExpectedAmount"), dr("ExpectedAmount"), Nothing)
        ePayment.OfficerCode = If(dr.Table.Columns.Contains("OfficerCode") AndAlso Not dr.IsNull("OfficerCode"), dr("OfficerCode"), Nothing)
        ePayment.PaymentDate = If(dr.Table.Columns.Contains("PaymentDate") AndAlso Not dr.IsNull("PaymentDate"), dr("PaymentDate"), Nothing)
        ePayment.ControlNumber = If(dr.Table.Columns.Contains("ControlNumber") AndAlso Not dr.IsNull("ControlNumber"), dr("ControlNumber"), Nothing)
        ePayment.TransactionNumber = If(dr.Table.Columns.Contains("TransactionNo") AndAlso Not dr.IsNull("TransactionNo"), dr("TransactionNo"), Nothing)
        ePayment.ReceivedDate = If(dr.Table.Columns.Contains("ReceivedDate") AndAlso Not dr.IsNull("ReceivedDate"), dr("ReceivedDate"), Nothing)
        ePayment.PaymentStatus = If(dr.Table.Columns.Contains("StatusID") AndAlso Not dr.IsNull("StatusID"), dr("StatusID"), Nothing)

        Return ePayment
    End Function


    Public Function LoadPayment(ByRef ePayment As IMIS_EN.tblPayment) As DataTable
        Dim Payment As New IMIS_DAL.PaymentDAL
        Return Payment.LoadPayment(ePayment)
    End Function
    Public Function LoadPaymentDetails(ePayment As IMIS_EN.tblPayment, ByVal PaymentDetails As Integer)
        Dim Payment As New IMIS_DAL.PaymentDAL
        Return Payment.LoadPaymentDetails(ePayment, PaymentDetails)
    End Function

    Public Function MatchPaymentAPI(ByVal PaymentID As Integer, ByVal AuditUserID As Integer) As Boolean
        Dim Gen As New GeneralBL
        Dim IMISDefault As New IMISDefaultsBL
        Dim eDefaults As New IMIS_EN.tblIMISDefaults
        Dim APIKey, Json As String
        Dim isMatched As Boolean
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim reqString() As Byte
        Dim APIObject = New With {.paymentId = PaymentID, .auditUserId = AuditUserID}
        Dim APIresponse As New Object
        Dim urlToPost As String = ConfigurationManager.AppSettings("RestfullURL")
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Json = serializer.Serialize(APIObject)
        IMISDefault.GetDefaults(eDefaults)
        APIKey = eDefaults.APIKey

        webClient.Headers("content-type") = "application/json"
        reqString = Encoding.Default.GetBytes(Json)
        resByte = webClient.UploadData(urlToPost + "MatchPayment?APIKey=" + APIKey, "post", reqString)
        resString = Encoding.Default.GetString(resByte)
        APIresponse = serializer.Deserialize(Of Object)(resString)
        isMatched = Boolean.Parse(APIresponse.item("isMatched"))
        webClient.Dispose()
        Return isMatched

    End Function
    Public Sub SaveEditedPaymentDetails(ePaymentDetails As IMIS_EN.tblPaymentDetail)
        Dim Payment As New IMIS_DAL.PaymentDAL

        Payment.SaveEditedPaymentDetails(ePaymentDetails)

    End Sub
End Class

