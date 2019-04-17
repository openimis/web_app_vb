Imports System.Web
Imports System.Web.Services
Imports Newtonsoft.Json

Public Class AutoCompleteHandler
    Implements System.Web.IHttpHandler
    Private FindClaimsB As New IMIS_BI.FindClaimsBI
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        Dim dt As DataTable = FindClaimsB.getAutoCompleteICDCodes(True)
        Dim d As String = JsonConvert.SerializeObject(dt)

        context.Response.ContentType = "text/json"
        context.Response.Write(d)

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class