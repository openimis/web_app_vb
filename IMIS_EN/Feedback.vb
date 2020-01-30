Public Class Feedback
    Private _ClaimId As Integer
    Private _OfficerId As Integer
    Private _OfficerCode As String
    Private _CHFID As String
    Private _LastName As String
    Private _OtherNames As String
    Private _HFCode As String
    Private _HFName As String
    Private _ClaimCode As String
    Private _DateFrom As String
    Private _DateTo As String
    Private _IMEI As String
    Private _Phone As String
    Private _FeedbackPromptDate As String

    Public Property ClaimId() As Integer
        Get
            ClaimId = _ClaimId
        End Get
        Set(ByVal value As Integer)
            _ClaimId = value
        End Set
    End Property

    Public Property OfficerId() As Integer
        Get
            OfficerId = _OfficerId
        End Get
        Set(ByVal value As Integer)
            _OfficerId = value
        End Set
    End Property

    Public Property OfficerCode() As String
        Get
            OfficerCode = _OfficerCode
        End Get
        Set(ByVal value As String)
            _OfficerCode = value
        End Set
    End Property

    Public Property CHFID() As String
        Get
            CHFID = _CHFID
        End Get
        Set(ByVal value As String)
            _CHFID = value
        End Set
    End Property

    Public Property LastName() As String
        Get
            LastName = _LastName
        End Get
        Set(ByVal value As String)
            _LastName = value
        End Set
    End Property

    Public Property OtherNames() As String
        Get
            OtherNames = _OtherNames
        End Get
        Set(ByVal value As String)
            _OtherNames = value
        End Set
    End Property

    Public Property HFCode() As String
        Get
            HFCode = _HFCode
        End Get
        Set(ByVal value As String)
            _HFCode = value
        End Set
    End Property

    Public Property HFName() As String
        Get
            HFName = _HFName
        End Get
        Set(ByVal value As String)
            _HFName = value
        End Set
    End Property

    Public Property ClaimCode() As String
        Get
            ClaimCode = _ClaimCode
        End Get
        Set(ByVal value As String)
            _ClaimCode = value
        End Set
    End Property

    Public Property DateFrom() As String
        Get
            DateFrom = _DateFrom
        End Get
        Set(ByVal value As String)
            _DateFrom = value
        End Set
    End Property

    Public Property DateTo() As String
        Get
            DateTo = _DateTo
        End Get
        Set(ByVal value As String)
            _DateTo = value
        End Set
    End Property

    Public Property IMEI() As String
        Get
            IMEI = _IMEI
        End Get
        Set(ByVal value As String)
            _IMEI = value
        End Set
    End Property

    Public Property Phone As String
        Get
            Phone = _Phone
        End Get
        Set(value As String)
            _Phone = value
        End Set
    End Property

    Public Property FeedbackPromptDate() As String
        Get
            FeedbackPromptDate = _FeedbackPromptDate
        End Get
        Set(ByVal value As String)
            _FeedbackPromptDate = value
        End Set
    End Property


End Class