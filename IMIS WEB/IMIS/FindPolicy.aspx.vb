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

Partial Public Class FindPolicy
    Inherits System.Web.UI.Page
    Private ePolicy As New IMIS_EN.tblPolicy
    Private Policy As New IMIS_BI.FindPolicyBI
    Protected imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvPolicies)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(13).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
            'row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub
    Private Sub loadGrid() Handles btnSearch.Click, chkLegacy.CheckedChanged, gvPolicies.PageIndexChanged, chkOffline.CheckedChanged
        Try
            loadSecurity()
            Dim eProduct As New IMIS_EN.tblProduct
            If txtEnrolmentDateFrom.Text.Length > 0 Then
                If IsDate(txtEnrolmentDateFrom.Text.Trim) Then
                    ePolicy.EnrollDateFrom = Date.ParseExact(txtEnrolmentDateFrom.Text, "dd/MM/yyyy", Nothing)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If

            End If
            If txtEnrolmentDateTo.Text.Length > 0 Then
                If IsDate(txtEnrolmentDateTo.Text.Trim) Then
                    ePolicy.EnrollDateTo = Date.ParseExact(txtEnrolmentDateTo.Text, "dd/MM/yyyy", Nothing)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If

            End If


            If txtExpiryDateFrom.Text.Length > 0 Then
                If IsDate(txtExpiryDateFrom.Text.Trim) Then
                    ePolicy.ExpiryDateFrom = Date.ParseExact(txtExpiryDateFrom.Text, "dd/MM/yyyy", Nothing)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If

            End If
            If txtExpiryDateTo.Text.Length > 0 Then
                If IsDate(txtExpiryDateTo.Text.Trim) Then
                    ePolicy.ExpiryDateTo = Date.ParseExact(txtExpiryDateTo.Text, "dd/MM/yyyy", Nothing)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If

            End If



            If txtStartDateFrom.Text.Length > 0 Then
                If IsDate(txtStartDateFrom.Text.Trim) Then
                    ePolicy.StartDateFrom = Date.ParseExact(txtStartDateFrom.Text, "dd/MM/yyyy", Nothing)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If

            End If
            If txtStartDateTo.Text.Length > 0 Then
                If IsDate(txtStartDateTo.Text.Trim) Then
                    ePolicy.StartDateTo = Date.ParseExact(txtStartDateTo.Text, "dd/MM/yyyy", Nothing)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If

            End If



            If txtEffectiveDateFrom.Text.Length > 0 Then
                If IsDate(txtEffectiveDateFrom.Text.Trim) Then
                    ePolicy.EffectiveDateFrom = Date.ParseExact(txtEffectiveDateFrom.Text, "dd/MM/yyyy", Nothing)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If

            End If
            If txtEffectiveDateTo.Text.Length > 0 Then
                If IsDate(txtEffectiveDateTo.Text.Trim) Then
                    ePolicy.EffectiveDateTo = Date.ParseExact(txtEffectiveDateTo.Text, "dd/MM/yyyy", Nothing)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If

            End If


            If txtBalance.Text.Length > 0 Then
                ePolicy.PolicyValue = txtBalance.Text
            Else
                ePolicy.PolicyValue = Nothing
            End If
            If ddlPolicyStatus.SelectedValue.Length > 0 Then
                ePolicy.PolicyStatus = ddlPolicyStatus.SelectedValue
            End If
            If ddlType.SelectedValue <> String.Empty Then
                ePolicy.PolicyStage = ddlType.SelectedValue
            End If
            ePolicy.isOffline = chkOffline.Checked
            Dim eFamilies As New IMIS_EN.tblFamilies
            eFamilies.RegionId = Val(ddlRegion.SelectedValue)
            If Val(ddlDistrict.SelectedValue) > 0 Then eFamilies.DistrictId = Val(ddlDistrict.SelectedValue) Else eFamilies.DistrictId = 0
            ePolicy.tblFamilies = eFamilies
            ePolicy.AuditUserID = imisgen.getUserId(Session("User"))
            Dim eOfficer As New IMIS_EN.tblOfficer
            If Val(ddProduct.SelectedValue) > 0 Then eProduct.ProdID = Val(ddProduct.SelectedValue)
            ePolicy.tblProduct = eProduct
            If Val(ddlEnrolmentOfficers.SelectedValue) > 0 Then eOfficer.OfficerID = Val(ddlEnrolmentOfficers.SelectedValue)
            ePolicy.tblOfficer = eOfficer
            getGridData()
            DisableButtonsOnEmptyRows(gvPolicies)
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
    Private Sub DisableButtonsOnEmptyRows(ByRef gv As GridView)
        If gv.Rows.Count = 0 Then
            B_VIEW.Enabled = False
        End If
    End Sub
    Private Sub loadSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim AllowEdit As Boolean = userBI.RunPageSecurity(IMIS_EN.Enums.Pages.OverviewFamily, Page)

        Dim hlink As HyperLinkField = gvPolicies.Columns(1)
        If chkLegacy.Checked Then
            hlink.DataNavigateUrlFormatString = "Policy.aspx?f={0}&po={1}"
        Else
            If AllowEdit Then
                hlink.DataNavigateUrlFormatString = "OverViewFamily.aspx?f={0}&po={1}"
            Else
                hlink.DataNavigateUrlFormatString = "Policy.aspx?f={0}&po={1}"
            End If
        End If

    End Sub
    Private Sub getGridData()

        Dim dtPoliocies As DataTable = Policy.GetPolicy(ePolicy, chkLegacy.Checked, chkDeactivatedPolicies.Checked)

        L_FOUNDPOLICY.Text = If(dtPoliocies.Rows.Count = 0, imisgen.getMessage("L_NO"), Format(dtPoliocies.Rows.Count, "#,###")) & " " & imisgen.getMessage("L_POLICIESFOUND")


        'Dim sindex As Integer = 0
        'Dim dv As DataView = dtUsers.DefaultView
        'If Not IsPostBack = True Then
        '    If Not HttpContext.Current.Request.QueryString("p") Is Nothing Then
        '        ePolicy.PolicyID = HttpContext.Current.Request.QueryString("p")
        '    End If
        '    If Not ePolicy.PolicyID = "" Then
        '        dv.Sort = "PolicyID"
        '        Dim x As Integer = dv.Find(ePolicy.PolicyID)
        '        If x >= 0 Then
        '            gvPolicies.PageIndex = Int(x / 15)
        '            Math.DivRem(x, 15, sindex)
        '        End If
        '    End If
        'End If

        gvPolicies.DataSource = dtPoliocies
        gvPolicies.SelectedIndex = -1
        gvPolicies.DataBind()
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub
    Protected Sub gvPolicies_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPolicies.PageIndexChanging
        gvPolicies.PageIndex = e.NewPageIndex
        loadGrid()
    End Sub
    Private Sub FindPolicy_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()
        Try
            chkOffline.Visible = IMIS_Gen.OfflineCHF
            lblMsg.Text = ""
            If IsPostBack = True Then Return

            Dim dtRegions As DataTable = Policy.GetRegions(imisgen.getUserId(Session("User")), True, False)
            ddlRegion.DataSource = dtRegions
            ddlRegion.DataValueField = "RegionId"
            ddlRegion.DataTextField = "RegionName"
            ddlRegion.DataBind()
            If dtRegions.Rows.Count = 1 Then
                FillDistrict()
                FillProducts()
            End If
            ddlPolicyStatus.DataSource = Policy.GetPolicyStatus(True)
            ddlPolicyStatus.DataValueField = "Code"
            ddlPolicyStatus.DataTextField = "Status"
            ddlPolicyStatus.DataBind()


           

            ddlType.DataSource = Policy.GetPolicyType()
            ddlType.DataValueField = "TypeId"
            ddlType.DataTextField = "Type"
            ddlType.DataBind()
           ' ddlRegion.SelectedValue = -1
            LoadOfficers()
            FillProducts()
            Session("ParentUrl") = "FindPolicy.aspx"
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub
    Private Sub FillDistrict()
        Dim dtDistricts As DataTable = Policy.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataSource = dtDistricts
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.FindPolicy, Page) Then
            B_VIEW.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.FindPolicy, UserID)
        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindPolicy.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Protected Sub chkLegacy_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLegacy.CheckedChanged
        If chkLegacy.Checked = True Then
            B_VIEW.Visible = False
        Else
            B_VIEW.Visible = False
        End If

    End Sub
    Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click
        Response.Redirect("Policy.aspx?p=" & gvPolicies.SelectedDataKey.Values.Item("PolicyID"))
    End Sub
    Private Sub ddlDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged
        LoadOfficers()
        FillProducts()
    End Sub
    Private Sub LoadOfficers()
        Dim LocationId As Integer = -1
        If Val(ddlDistrict.SelectedValue) = 0 Then
            LocationId = Val(ddlRegion.SelectedValue)
        Else
            LocationId = Val(ddlDistrict.SelectedValue)
        End If

        'If LocationId > 0 Then
        ddlEnrolmentOfficers.DataSource = Policy.GetOfficers(LocationId, True)
        ddlEnrolmentOfficers.DataValueField = "OfficerID"
        ddlEnrolmentOfficers.DataTextField = "Code"
        ddlEnrolmentOfficers.DataBind()

        'End If
    End Sub
    Private Sub FillProducts()

        'AMANI 26/09

        'If(Val(ddlRegion.SelectedValue)=0, 0, Val(ddlRegion.SelectedValue))


        ddProduct.DataSource = Policy.GetProducts(imisgen.getUserId(Session("User")), True, Val(ddlRegion.SelectedValue), Val(ddlDistrict.SelectedValue))
        ddProduct.DataValueField = "ProdId"
        ddProduct.DataTextField = "ProductCode"
        ddProduct.DataBind()
    End Sub
    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistrict()
        FillProducts()
        LoadOfficers()
    End Sub
End Class
