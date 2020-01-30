Public Class BgService
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Response.Headers("ServiceResponse") IsNot Nothing Then Response.Write(Response.Headers("ServiceResponse"))
        Catch ex As Exception

            'LOIS_EN.GeneralEN.SetMessage(ex.Message, Page, LOIS_EN.GeneralEN.MessageModes.PopupAlert, IsException:=True)
        End Try
    End Sub
End Class