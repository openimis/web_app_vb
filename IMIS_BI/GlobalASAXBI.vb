Public Class GlobalAsaxBI
    Public Shared Sub DoBackgroundTasks(DomainUrl As String)
        Dim BL As New IMIS_BL.BgServiceBL
        BL.DoBackgroundTasks(DomainUrl)
    End Sub
End Class
