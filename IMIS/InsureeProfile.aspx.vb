Imports System
Imports System.Data
Imports System.Data.SqlClient

Partial Public Class InsureeProfile
    Inherits System.Web.UI.Page
    Private Insuree As New IMIS_BI.FindInsureeBI
    Private eInsuree As New IMIS_EN.tblInsuree
    Private imisgen As New IMIS_Gen
    Private FindClaimsB As New IMIS_BI.FindClaimsBI
    Private userBI As New IMIS_BI.UserBI



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Return
        'RunPageSecurity()
        Dim NSHI As String
        NSHI = Request.QueryString("nshid")
        If String.IsNullOrEmpty(NSHI) Then
            Return
        End If
        BindData(NSHI)
        Dim UserID As Integer
        UserID = imisgen.getUserId(Session("User"))
        FillRegion()
        FillHF(UserID)

    End Sub
    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Home, Page) Then

        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindInsuree.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub BindData(ByVal NSHI As String)
        FillRepeater(NSHI)
        FillPolicyGrid(NSHI)
    End Sub
    Private Sub FillPolicyGrid(ByVal NSHI As String)
        If Not IsNumeric(NSHI) Then Return
        Dim dt As New DataTable
        Dim Insuree As New IMIS_BI.InsureeBI
        dt = Insuree.GetInsureeByCHFIDGrid(NSHI)
        gvPolicy.DataSource = dt
        gvPolicy.DataBind()

    End Sub
    Private Sub FillRepeater(ByVal NSHI As String)
        If Not IsNumeric(NSHI) Then Return
        Dim dt As New DataTable
        Dim Insuree As New IMIS_BI.InsureeBI
        dt = Insuree.GetInsureeByCHFID(NSHI)
        Dim myDOB As DateTime
        If (dt.Rows.Count > 0) Then
            Select Case dt.Rows(0)("Gender").ToString()
                Case "M"
                    dt.Rows(0)("Gender") = "Male"
                Case "F"
                    dt.Rows(0)("Gender") = "Female"
                Case "O"
                    dt.Rows(0)("Gender") = "Other"
                Case Else
                    dt.Rows(0)("Gender") = "Other"
            End Select
            myDOB = DateTime.Parse(dt.Rows(0)("DOB"))

        End If
        rptInsuree.DataSource = dt
        rptInsuree.DataBind()
        Dim intAge As Int16 = Math.Floor(DateDiff(DateInterval.Month, DateValue(myDOB), Now()) / 12)
        For i As Integer = 0 To rptInsuree.Items.Count - 1
            Dim lblInsureeAge As Label = DirectCast(rptInsuree.Items(i).FindControl("lblInsureeAge"), Label)
            lblInsureeAge.Text = " (" + intAge.ToString() + " Years)"
        Next
        dt = Insuree.GetFamilyDetails(NSHI, Request.Cookies("CultureInfo").Value)

        dt = Insuree.GetFamilyDetails(NSHI, Request.Cookies("CultureInfo").Value)
        grdFamilyDetail.DataSource = dt
        grdFamilyDetail.DataBind()

        dt = Insuree.GetClaimList(NSHI, Request.Cookies("CultureInfo").Value)
        grdClaimDetail.DataSource = dt
        grdClaimDetail.DataBind()


    End Sub

    Protected Sub gvPolicy_RowDataBound(sender As Object, e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim statusCell As TableCell = e.Row.Cells(3)
            If statusCell.Text = imisgen.getMessage("X_NP_ACTIVE") Then
                statusCell.ForeColor = Drawing.Color.Green
            Else
                statusCell.ForeColor = Drawing.Color.Red
            End If

        End If
    End Sub
    Private Sub B_ADD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_ADD.Click

        'GetFilterCriteria()
        Session("DistrictIDFindClaims") = ddlDistrict.SelectedValue
        Session("HFID") = ddlHFCode.SelectedValue
        Session("RegionSelected") = ddlRegion.SelectedValue
        Response.Redirect("Claim.aspx?c=0&h=" & ddlHFCode.SelectedValue & "&a=" & ddlClaimAdmin.SelectedValue)
    End Sub
    Private Sub FillRegion()
        Dim dtRegions As DataTable = FindClaimsB.GetRegions(imisgen.getUserId(Session("User")), True)
        ddlRegion.DataSource = dtRegions
        ddlRegion.DataValueField = "RegionId"
        ddlRegion.DataTextField = "RegionName"
        ddlRegion.DataBind()

        If dtRegions.Rows.Count = 1 Then
            FillDistricts()

        End If
    End Sub
    Private Sub FillDistricts()
        ddlDistrict.DataSource = FindClaimsB.GetDistricts(imisgen.getUserId(Session("User")), True, Val(ddlRegion.SelectedValue))
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub FillHF(ByVal UserID As Integer)
        Dim LocationId As Integer = 0
        If Val(ddlDistrict.SelectedValue) > 0 Then
            LocationId = Val(ddlDistrict.SelectedValue)
        ElseIf Val(ddlRegion.SelectedValue) > 0 Then
            LocationId = Val(ddlRegion.SelectedValue)
        End If

        ddlHFCode.DataSource = FindClaimsB.GetHFCodes(UserID, LocationId)
        ddlHFCode.DataValueField = "HfID"
        ddlHFCode.DataTextField = "HFCODE"
        ddlHFCode.DataBind()
        If Request.QueryString("c") = "c" Then
            If IsPostBack = False Then
                ddlHFCode.SelectedValue = CType(Session("HFID"), Integer)
                ''''Clear HFID session
                Session.Remove("HFID")
            End If
        End If
        FillClaimAdminCodes()
    End Sub
    Private Sub FillClaimAdminCodes()
        Dim HFID As Integer = 0
        If ddlHFCode.SelectedIndex > -1 Then HFID = ddlHFCode.SelectedValue
        ddlClaimAdmin.DataSource = FindClaimsB.GetHFClaimAdminCodes(HFID, True)
        ddlClaimAdmin.DataTextField = "Description"
        ddlClaimAdmin.DataValueField = "ClaimAdminID"
        ddlClaimAdmin.DataBind()
    End Sub
    Private Sub ddlDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged, ddlRegion.SelectedIndexChanged
        'Amani 04/12
        FillClaimAdminCodes()
        FillHF(imisgen.getUserId(Session("User")))
    End Sub

    Private Sub ddlHFCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHFCode.SelectedIndexChanged
        FillClaimAdminCodes()
    End Sub
    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        ''amani 04/12
        Session("RegionSelected") = ddlRegion.SelectedValue.ToString
        FillDistricts()

    End Sub
End Class