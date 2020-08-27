Imports System.IO
Imports System.Web
Imports System.Web.Services
Imports Newtonsoft.Json

Public Class AutoCompleteHandler
    Implements System.Web.IHttpHandler
    Private FindClaimsB As New IMIS_BI.FindClaimsBI
    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim FindClaimsB As New IMIS_BI.FindClaimsBI
        Dim dt As DataTable = FindClaimsB.getAutoCompleteICDCodes(True)

        Dim prefix = String.Empty
        Dim d = String.Empty
        prefix = context.Request("ICDCode")

        If prefix = " " Or prefix = "" Then
            Dim items = (From p In dt.AsEnumerable()
                         Select New With {.ICDID = p.Field(Of Integer)("ICDID"),
                                    .ICDNames = p.Field(Of String)("ICDNames")}).Take(10)
            d = JsonConvert.SerializeObject(items)
        Else
            Dim items = (From p In dt.AsEnumerable()
                         Select New With {.ICDID = p.Field(Of Integer)("ICDID"),
                                    .ICDNames = p.Field(Of String)("ICDNames")}).Where(Function(x) x.ICDNames.ToLower.Contains(prefix.ToLower())).Take(10)

            d = JsonConvert.SerializeObject(items)
        End If

        context.Response.ContentType = "text/json"
        context.Response.Write(d)

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class