Imports System
Imports System.Data
Imports System.Data.SqlClient

Partial Public Class CappedItemService
    Inherits System.Web.UI.Page

    Private Insuree As New IMIS_BI.FindInsureeBI
    Private eInsuree As New IMIS_EN.tblInsuree
    Private imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Return
        'RunPageSecurity()
        Dim NSHID As String
        NSHID = Request.QueryString("nshid")
        If String.IsNullOrEmpty(NSHID) Then
            Return
        End If
        Try
            Dim str As String = Web.Configuration.WebConfigurationManager.ConnectionStrings("IMISConnectionString").ConnectionString

            Dim con As New SqlConnection(str)

            Dim com As String = "select i.CHFID,concat(i.OtherNames,' ',i.LastName) MemberName, c.ClaimCode, s.ServName, s.ServFrequency, c.DateTo, hf.HFName  " &
                "from tblInsuree i inner join tblClaim c On i.InsureeID=c.InsureeID inner join tblClaimServices cs on cs.ClaimID=c.ClaimID  " &
                "inner join tblServices s On cs.ServiceID=s.ServiceID inner join tblHF hf on hf.HfID=c.HFID where s.ServFrequency >0 and  " &
                "c.ValidityTo is null And i.ValidityTo Is null and cs.ValidityTo is null and hf.ValidityTo is null And s.ValidityTo Is null and  " &
                "c.ClaimStatus in(4,8,16) And cs.RejectionReason =0 and DATEDIFF(day, c.DateTo,GETDATE()) < s.ServFrequency And  " &
                "CHFID='" & NSHID & "' order by c.DateTo desc "

            Dim Adpt As New SqlDataAdapter(com, con)
            Dim ds As New DataSet()

            Adpt.Fill(ds, "Capped")


            If ds.Tables(0).Rows.Count > 0 Then
                grdCappedDetails.DataSource = ds.Tables(0)
                grdCappedDetails.DataBind()
            End If
        Catch ex As Exception

            Return

        End Try


    End Sub
    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Home, Page) Then

        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindInsuree.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub


End Class