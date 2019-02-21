Imports System.Net
Imports Newtonsoft.Json
Public Class BgServiceBL
    Public Sub DoBackgroundTasks(DomainUrl As String)
        Debug.Print("Running")
        'Dim eLog As LOIS_EN.tblLogs
        Try
            '
            MatchPayments(DomainUrl)

        Catch Ex As Exception
            '    eLog = New LOIS_EN.tblLogs()
            '    With eLog
            '        .Type = LOIS_EN.GeneralEN.LogTypes.BgService
            '        .Status = LOIS_EN.GeneralEN.LogStatus.Fail
            '        .Description = Ex.Message
            '    End With
            '    Dim BL As New LogBL
            '    BL.InsertLog(eLog)
            Throw Ex
        End Try
    End Sub


    Public Function MatchPayments(DomainUrl As String) As String
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim reqString() As Byte
        Dim MatchApi As String = DomainUrl & "restapi/api/matchpayment" 'HttpContext.Current.Request.Url.AbsolutePath & "restapi/api/matchpayment"
        MatchApi = "http://imis-mv.swisstph-mis.ch/restapi/api/matchpayment"
        Dim dt As New Dictionary(Of String, Object)
        dt.Add("internal_identifier", DBNull.Value)
        dt.Add("audit_user_id", "0")

        Try
            webClient.Headers("content-type") = "application/json"
            webClient.Headers("User-Agent") = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; EasyBits GO v1.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; Tablet PC 2.0; InfoPath.3)"
            webClient.Proxy = WebRequest.DefaultWebProxy
            reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dt, Formatting.Indented))
            resByte = webClient.UploadData(MatchApi, "post", reqString)
            resString = Encoding.Default.GetString(resByte)


            Return resString
        Catch ex As WebException
            Dim res As WebResponse = ex.Response
            Dim sr As New IO.StreamReader(res.GetResponseStream)
            Dim geterror As String = sr.ReadToEnd()
            Debug.Print(geterror)
            Throw ex
        Finally
            webClient.Dispose()
        End Try
        Return False
    End Function
End Class
