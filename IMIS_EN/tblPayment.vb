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


Public Class tblPayment

    Private _PaymentID As Integer?
    Private _PaymentUUID As String
    Private _ControlNumber As String
    Private _ExpectedAmount As Decimal?
    Private _ReceivedAmount As Global.System.Decimal?
    Private _OfficerCode As String
    Private _InsuranceNumber As String
    Private _RequestID As Integer
    Private _DateFrom As Date?
    Private _DateTo As Date?
    Private _PayerName As String
    Private _PaymentDesc As String
    Private _PaymentStatus As Global.System.Int32?
    Private _LocationID As Integer
    Private _ProductCode As String
    Private _Legacy As Boolean
    Property _dtPaymentStatus As DataTable
    Private _AuditUserID As Integer?
    Private _ReceivingDateFrom As Date?
    Private _ReceivingDateTo As Date?
    Private _PaymentOrigin As String
    Private _TransactionNumber As String
    Private _ReceiptNo As String
    Private _MatchDateFrom As Date?
    Private _MatchedDateTo As Date?
    Private _MatchedDate As Date?
    Private _PhoneNumber As String
    Private _ReceivedAmountFrom As Global.System.Decimal?
    Private _ReceivedAmountTO As Global.System.Decimal?
    Private _PaymentStatusName As String
    Private _ReceivedDate As Date?
    Private _RegionId As Integer
    Private _RejectedReason As String
    Private _DistrictId As Integer

    Public Property PaymentID() As Integer
        Get
            Return _PaymentID
        End Get
        Set(ByVal value As Integer)
            _PaymentID = value
        End Set
    End Property
    Public Property PaymentUUID() As String
        Get
            Return _PaymentUUID
        End Get
        Set(ByVal value As String)
            _PaymentUUID = value
        End Set
    End Property
    Public Property RegionId() As Integer
        Get
            Return _RegionId
        End Get
        Set(ByVal value As Integer)
            _RegionId = value
        End Set
    End Property
    Public Property DistrictId() As Integer
        Get
            Return _DistrictId
        End Get
        Set(ByVal value As Integer)
            _DistrictId = value
        End Set
    End Property
    Public Property ControlNumber As String
        Get
            Return _ControlNumber
        End Get
        Set(value As String)
            _ControlNumber = value
        End Set
    End Property
    Public Property ExpectedAmount As Global.System.Decimal?
        Get
            Return _ExpectedAmount
        End Get
        Set(value As Decimal?)
            _ExpectedAmount = value
        End Set
    End Property
    Public Property ReceivedAmount() As Global.System.Decimal?
        Get
            Return _ReceivedAmount
        End Get
        Set(value As Decimal?)
            _ReceivedAmount = value
        End Set
    End Property
    Public Property OfficerCode As String
        Get
            Return _OfficerCode
        End Get
        Set(value As String)
            _OfficerCode = value
        End Set
    End Property
    Public Property InsuranceNumber As String
        Get
            Return _InsuranceNumber
        End Get
        Set(value As String)
            _InsuranceNumber = value
        End Set
    End Property
    Public Property DateFrom As Date?
        Get
            Return _DateFrom
        End Get
        Set(ByVal value As Date?)
            _DateFrom = value
        End Set
    End Property
    Public Property DateTo As Date?
        Get
            Return _DateTo
        End Get
        Set(ByVal value As Date?)
            _DateTo = value
        End Set
    End Property
    Public Property PayerName As String
        Get
            Return _PayerName
        End Get
        Set(value As String)
            _PayerName = value
        End Set
    End Property
    Public Property PaymentDesc As String
        Get
            Return _PaymentDesc
        End Get
        Set(value As String)
            _PaymentDesc = value
        End Set
    End Property

    Public Property PaymentStatusName As String
        Get
            Return _PaymentStatusName
        End Get
        Set(value As String)
            _PaymentStatusName = value
        End Set
    End Property
    Public Property PaymentStatus As Global.System.Int32?
        Get
            Return _PaymentStatus
        End Get
        Set(value As Integer?)
            _PaymentStatus = value
        End Set
    End Property
    Public Property LocationID As Integer?
        Get
            Return _LocationID
        End Get
        Set(value As Integer?)
            _LocationID = value
        End Set
    End Property
    Public Property ProductCode As String
        Get
            Return _ProductCode
        End Get
        Set(value As String)
            _ProductCode = value
        End Set
    End Property
    Public Property Legacy As Boolean
        Get
            Return _Legacy
        End Get
        Set(value As Boolean)
            _Legacy = value
        End Set
    End Property
    Public Property dtPaymentStatus As DataTable
        Get
            Return _dtPaymentStatus
        End Get
        Set(value As DataTable)
            _dtPaymentStatus = value
        End Set
    End Property
    Public Property AuditUserID As Integer
        Get
            Return _AuditUserID
        End Get
        Set(value As Integer)
            _AuditUserID = value
        End Set
    End Property

    Public Property TransactionNumber() As String
        Get
            Return _TransactionNumber
        End Get
        Set(ByVal value As String)
            _TransactionNumber = value
        End Set
    End Property

    Public Property PaymentOrigin() As String
        Get
            Return _PaymentOrigin
        End Get
        Set(ByVal value As String)
            _PaymentOrigin = value
        End Set
    End Property

    Public Property ReceivingDateFrom() As Date?
        Get
            Return _ReceivingDateFrom
        End Get
        Set(ByVal value As Date?)
            _ReceivingDateFrom = value
        End Set
    End Property

    Public Property ReceivingDateTo() As Date?
        Get
            Return _ReceivingDateTo
        End Get
        Set(ByVal value As Date?)
            _ReceivingDateTo = value
        End Set
    End Property


    Public Property MatchDateFrom() As Date?
        Get
            Return _MatchDateFrom
        End Get
        Set(ByVal value As Date?)
            _MatchDateFrom = value
        End Set
    End Property


    Public Property MatchedDateTo() As Date?
        Get
            Return _MatchedDateTo
        End Get
        Set(ByVal value As Date?)
            _MatchedDateTo = value
        End Set
    End Property
    Public Property MatchedDate() As Date?
        Get
            Return _MatchedDate
        End Get
        Set(ByVal value As Date?)
            _MatchedDate = value
        End Set
    End Property
    Private _PaymentDate As Date?
    Public Property PaymentDate() As Date?
        Get
            Return _PaymentDate
        End Get
        Set(ByVal value As Date?)
            _PaymentDate = value
        End Set
    End Property

    Public Property ReceivedDate() As Date?
        Get
            Return _ReceivedDate
        End Get
        Set(ByVal value As Date?)
            _ReceivedDate = value
        End Set
    End Property


    Public Property PhoneNumber() As String
        Get
            Return _PhoneNumber
        End Get
        Set(ByVal value As String)
            _PhoneNumber = value
        End Set
    End Property


    Public Property ReceivedAmountFrom() As Global.System.Decimal?
        Get
            Return _ReceivedAmountFrom
        End Get
        Set(ByVal value As Decimal?)
            _ReceivedAmountFrom = value
        End Set
    End Property

    Public Property ReceivedAmountTO() As Global.System.Decimal?
        Get
            Return _ReceivedAmountTO
        End Get
        Set(ByVal value As Decimal?)
            _ReceivedAmountTO = value
        End Set
    End Property
    Public Property ReceiptNo As String
        Get
            Return _ReceiptNo
        End Get
        Set(value As String)
            _ReceiptNo = value
        End Set
    End Property
    Public Property RejectedReason As String
        Get
            Return _RejectedReason
        End Get
        Set(value As String)
            _RejectedReason = value
        End Set
    End Property
End Class
