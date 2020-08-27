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
Public Class tblRole

    Private _RoleID As Int32
    Private _RoleUUID As Guid
    Private _RoleName As String
    Private _AltLanguage As String
    Private _IsSystem As Integer?
    Private _IsBlocked As Boolean?
    Private _ValidityFrom As DateTime
    Private _ValidityTo As DateTime?
    Private _AuditUserID As Int32?
    Private _LegacyID As Int32?
    Private _Assign As Int32?

    Public Property RoleID As Int32
        Get
            RoleID = _RoleID
        End Get
        Set(Value As Int32)
            _RoleID = Value
        End Set
    End Property
    Public Property RoleUUID As Guid
        Get
            RoleUUID = _RoleUUID
        End Get
        Set(Value As Guid)
            _RoleUUID = Value
        End Set
    End Property
    Public Property RoleName As String
        Get
            RoleName = _RoleName
        End Get
        Set(Value As String)
            _RoleName = Value
        End Set
    End Property
    Public Property AltLanguage As String
        Get
            AltLanguage = _AltLanguage
        End Get
        Set(Value As String)
            _AltLanguage = Value
        End Set
    End Property
    Public Property IsSystem As Integer?
        Get
            IsSystem = _IsSystem
        End Get
        Set(Value As Integer?)
            _IsSystem = Value
        End Set
    End Property
    Public Property IsBlocked As Boolean?
        Get
            IsBlocked = _IsBlocked
        End Get
        Set(Value As Boolean?)
            _IsBlocked = Value
        End Set
    End Property
    Public Property ValidityFrom As DateTime
        Get
            ValidityFrom = _ValidityFrom
        End Get
        Set(Value As DateTime)
            _ValidityFrom = Value
        End Set
    End Property
    Public Property ValidityTo As DateTime?
        Get
            ValidityTo = _ValidityTo
        End Get
        Set(Value As DateTime?)
            _ValidityTo = Value
        End Set
    End Property
    Public Property AuditUserID As Int32
        Get
            AuditUserID = _AuditUserID
        End Get
        Set(Value As Int32)
            _AuditUserID = Value
        End Set
    End Property
    Public Property LegacyID As Int32?
        Get
            LegacyID = _LegacyID
        End Get
        Set(Value As Int32?)
            _LegacyID = Value
        End Set
    End Property
    Public Property Assign As Int32?
        Get
            Assign = _Assign
        End Get
        Set(Value As Int32?)
            _Assign = Value
        End Set
    End Property

End Class