''Copyright (c) 2016-2017 Swiss Agency for Development and Cooperation (SDC)
''
''The program users must agree to the following terms:
''
''Copyright notices
''This program is free software: you can redistribute it and/or modify it under the terms of the GNU AGPL v3 License as published by the 
''Free Software Foundation, version 3 of the License.
''This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of 
''MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU AGPL v3 License for more details www.gnu.org.
''
''Disclaimer of Warranty
''There is no warranty for the program, to the extent permitted by applicable law; except when otherwise stated in writing the copyright 
''holders and/or other parties provide the program "as is" without warranty of any kind, either expressed or implied, including, but not 
''limited to, the implied warranties of merchantability and fitness for a particular purpose. The entire risk as to the quality and 
''performance of the program is with you. Should the program prove defective, you assume the cost of all necessary servicing, repair or correction.
''
''Limitation of Liability 
''In no event unless required by applicable law or agreed to in writing will any copyright holder, or any other party who modifies and/or 
''conveys the program as permitted above, be liable to you for damages, including any general, special, incidental or consequential damages 
''arising out of the use or inability to use the program (including but not limited to loss of data or data being rendered inaccurate or losses 
''sustained by you or third parties or a failure of the program to operate with any other programs), even if such holder or other party has been 
''advised of the possibility of such damages.
''
''In case of dispute arising out or in relation to the use of the program, it is subject to the public law of Switzerland. The place of jurisdiction is Berne.
'
' 
'

Partial Public Class FindPremium
    Inherits System.Web.UI.Page

    Private ePremium As New IMIS_EN.tblPremium
    Private Premium As New IMIS_BI.FindPremiumBI
    Protected imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvPremiums)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(8).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
            'row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RunPageSecurity()
        Try
            chkOffline.Visible = IMIS_Gen.OfflineCHF

            If Not IsPostBack = True Then

                Dim dtRegions As DataTable = Premium.GetRegions(imisgen.getUserId(Session("User")), True)
                ddlRegion.DataSource = dtRegions
                ddlRegion.DataValueField = "RegionId"
                ddlRegion.DataTextField = "RegionName"
                ddlRegion.DataBind()
                If dtRegions.Rows.Count = 1 Then
                    FillDistricts()
                End If
                ddlPayType.DataSource = Premium.GetTypeOfPayment(True)
                ddlPayType.DataTextField = "PayType"
                ddlPayType.DataValueField = "Code"
                ddlPayType.DataBind()


              

                LoadPayer()

                Session("ParentUrl") = "FindPremium.aspx"
            End If

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub FillDistricts()
        Dim dtDistricts As DataTable = Premium.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataSource = dtDistricts
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.FindPremium, Page) Then
            B_VIEW.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.ContributionSearch, UserID)
        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindPremium.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub LoadPayer()
        'If Val(ddlDistrict.SelectedValue) > 0 Then
        ddlPayer.DataSource = Premium.GetPayers(Val(ddlRegion.SelectedValue), Val(ddlDistrict.SelectedValue), imisgen.getUserId(Session("User")), True)
        ddlPayer.DataValueField = "PayerID"
        ddlPayer.DataTextField = "PayerName"
        ddlPayer.DataBind()
        'End If
    End Sub
    Private Sub loadSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim AllowEdit As Boolean = userBI.RunPageSecurity(IMIS_EN.Enums.Pages.OverviewFamily, Page)

        Dim hlink As HyperLinkField = gvPremiums.Columns(1)
        If chkLegacy.Checked Then
            hlink.DataNavigateUrlFormatString = "Premium.aspx?p={1}&f={0}&po={2}"
        Else
            If AllowEdit Then
                hlink.DataNavigateUrlFormatString = "OverViewFamily.aspx?p={1}&f={0}&po={2}"
            Else
                hlink.DataNavigateUrlFormatString = "#"
            End If
        End If

    End Sub
    Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click

        If gvPremiums.SelectedDataKey Is Nothing Then
            Response.Redirect("Premium.aspx?f=0&p=0")
        End If

        Response.Redirect("Premium.aspx?f=" & gvPremiums.SelectedDataKey.Values("FamilyID") & "&p=" & gvPremiums.SelectedDataKey.Values("PremiumID") & "&po=" & gvPremiums.SelectedDataKey.Values("PolicyID"))
    End Sub
    Protected Sub gvProducts_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPremiums.PageIndexChanging
        gvPremiums.PageIndex = e.NewPageIndex
    End Sub
    Private Sub LoadGrid() Handles btnSearch.Click, chkLegacy.CheckedChanged, gvPremiums.PageIndexChanged, chkOffline.CheckedChanged
        loadSecurity()
        Dim ePayer As New IMIS_EN.tblPayer
        Dim eLocations As New IMIS_EN.tblLocations
        eLocations.DistrictID = Val(ddlDistrict.SelectedValue)

        eLocations.RegionId = Val(ddlRegion.SelectedValue)

        ePayer.PayerID = Val(ddlPayer.SelectedValue)
        ePayer.tblLocations = eLocations
        ePremium.tblPayer = ePayer
        ePremium.AuditUserID = imisgen.getUserId(Session("User"))
        If Trim(txtDateOfPaymentFrom.Text).Length > 0 Then
            If IsDate(txtDateOfPaymentFrom.Text) Then
                ePremium.PayDateFrom = Date.Parse(txtDateOfPaymentFrom.Text)
            Else
                imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If

        If Trim(txtDateOfPaymentTo.Text).Length > 0 Then
            If IsDate(txtDateOfPaymentTo.Text) Then
                ePremium.PayDateTo = Date.Parse(txtDateOfPaymentTo.Text)
            Else
                imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
            If ePremium.PayDateTo > System.DateTime.Now Then

                lblMsg.Text = imisgen.getMessage("M_PAYDATETOEXCEEDCURRENDATE")
            End If
        End If
        If Trim(txtMatchedDateFrom.Text).Length > 0 Then
            If IsDate(txtMatchedDateFrom.Text) Then
                ePremium.MatchedDateFrom = Date.Parse(txtMatchedDateFrom.Text)
            Else
                imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If
        If Trim(txtMatchedDateTo.Text).Length > 0 Then
            If IsDate(txtMatchedDateTo.Text) Then
                ePremium.MatchedDateTo = Date.Parse(txtMatchedDateTo.Text)
            Else
                imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If


        ePremium.Amount = if(IsNumeric(txtPremiumPaid.Text), txtPremiumPaid.Text, 0)
        ePremium.PayType = ddlPayType.SelectedValue
        ePremium.Receipt = txtReceiptNo.Text
        ePremium.isOffline = chkOffline.Checked


        Dim dtPremium As DataTable = Premium.GetPremium(ePremium, chkLegacy.Checked)
        L_FOUNDPREMIUM.Text = If(dtPremium.Rows.Count = 0, imisgen.getMessage("L_NO"), Format(dtPremium.Rows.Count, "#,###")) & " " & imisgen.getMessage("L_FOUNDPREMIUMS")
        gvPremiums.DataSource = dtPremium
        gvPremiums.SelectedIndex = -1
        gvPremiums.DataBind()
        DisableButtonsOnEmptyRows(gvPremiums)
    End Sub
    Private Sub DisableButtonsOnEmptyRows(ByRef gv As GridView)
        If gv.Rows.Count = 0 Then
            B_VIEW.Enabled = False
        End If
    End Sub
    Protected Sub chkLegacy_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLegacy.CheckedChanged
        If chkLegacy.Checked = True Then
            B_VIEW.Visible = False
        Else
            B_VIEW.Visible = False
        End If
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub
    Private Sub ddlDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged
        Try
            LoadPayer()
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
        LoadPayer()
    End Sub
End Class
