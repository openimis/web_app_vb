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


Public Class tblPaymentDetail

    Private _PaymentDetailID As Global.System.Int32?
    Private _Amount As Double?
    Private _ProductCode As String
    Private _InsuranceNumber As String
    Private _MatchedDate As Date
    Private _ValidityFrom As Date
    Private _ValidityTo As Date
    Private _PolicyStage As String
    Private _AuditUserID As Integer?
    Private _Legacy As Boolean

    Public Property PaymentDetailID() As Global.System.Int32
        Get
            Return _PaymentDetailID
        End Get
        Set(ByVal value As Integer)
            _PaymentDetailID = value
        End Set
    End Property
    Private _DetailsID As Integer
    Public Property DetailsID() As Integer
        Get
            Return _DetailsID
        End Get
        Set(ByVal value As Integer)
            _DetailsID = value
        End Set
    End Property

    Public Property Amount As Double?
        Get
            Return _Amount
        End Get
        Set(value As Double?)
            _Amount = value
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
    Public Property MatchedDate As Date?
        Get
            Return _MatchedDate
        End Get
        Set(value As Date?)
            _MatchedDate = value
        End Set
    End Property
    Public Property ValidityFrom As Date?
        Get
            Return _ValidityFrom
        End Get
        Set(value As Date?)
            _ValidityFrom = value
        End Set
    End Property
    Public Property ValidityTo As Date?
        Get
            Return _ValidityTo
        End Get
        Set(value As Date?)
            _ValidityTo = value
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
    Public Property PolicyStage As String
        Get
            Return _PolicyStage
        End Get
        Set(value As String)
            _PolicyStage = value
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

    Public Property Legacy As Boolean
        Get
            Return _Legacy
        End Get
        Set(value As Boolean)
            _Legacy = value
        End Set
    End Property

End Class
