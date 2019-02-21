Public Class tblRoleRight

    Private _RoleRightID As Int32
    Private _RoleID As Int32
    Private _RightID As Int32
    Private _ValidityFrom As DateTime
    Private _ValidityTo As DateTime?
    Private _AuditUserId As Int32
    Private _LegacyID As Int32?


    Public Property RoleRightID As Int32
        Get
            RoleRightID = _RoleRightID
        End Get
        Set(Value As Int32)
            _RoleRightID = Value
        End Set
    End Property
    Public Property RoleID As Int32
        Get
            RoleID = _RoleID
        End Get
        Set(Value As Int32)
            _RoleID = Value
        End Set
    End Property
    Public Property RightID As Int32
        Get
            RightID = _RightID
        End Get
        Set(Value As Int32)
            _RightID = Value
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
    Public Property AuditUserId As Int32
        Get
            AuditUserId = _AuditUserId
        End Get
        Set(Value As Int32)
            _AuditUserId = Value
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