Public Class tblRole

    Private _RoleID As Int32
    Private _RoleName As String
    Private _IsSystem As Integer?
    Private _IsBlocked As Boolean?
    Private _ValidityFrom As DateTime
    Private _ValidityTo As DateTime?
    Private _AuditUserID As Int32?
    Private _LegacyID As Int32?


    Public Property RoleID As Int32
        Get
            RoleID = _RoleID
        End Get
        Set(Value As Int32)
            _RoleID = Value
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


End Class