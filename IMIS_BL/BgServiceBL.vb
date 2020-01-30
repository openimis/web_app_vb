Imports System.Net

Public Class BgServiceBL
    Public Function DoBackgroundTasks(DomainUrl As String) As Boolean
        Try
            Dim Escape As New EscapeBL
            Escape.MatchPayments(DomainUrl)
        Catch Ex As Exception

            Throw Ex
        End Try
    End Function

End Class
