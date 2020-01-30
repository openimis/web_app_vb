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

Partial Public Class FindFamily
    Inherits System.Web.UI.Page
    Private Family As New IMIS_BI.FindFamilyBI
    Protected imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private Sub FormatForm()
        Dim Adjustibility As String = ""
        'Confirmation Number
        Adjustibility = General.getControlSetting("ConfirmationNo")
        tdConfirmationNoLBL.Visible = Not (Adjustibility = "N")
        tdConfirmationNoTXT.Visible = Not (Adjustibility = "N")

        'Email
        Adjustibility = General.getControlSetting("InsureeEmail")
        tdEmailLBL.Visible = Not (Adjustibility = "N")
        tdEmailTXT.Visible = Not (Adjustibility = "N")

        'Poverty
        Adjustibility = General.getControlSetting("Poverty")
        tdPovertyLBL.Visible = Not (Adjustibility = "N")
        tdPovertyDD.Visible = Not (Adjustibility = "N")
        If tdPovertyDD.Visible = False And tdPovertyLBL.Visible = False Then
            tdTogglePosition.Visible = True
        Else
            tdTogglePosition.Visible = False
        End If
        For i As Integer = 0 To gvFamilies.Columns.Count
            If gvFamilies.Columns(i).HeaderText.Equals(imisgen.getMessage("L_Poverty_")) Then gvFamilies.Columns(i).Visible = Not (Adjustibility = "N") : Exit For

        Next



    End Sub
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvFamilies)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(10).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
            ' row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            FormatForm()
            chkOffline.Visible = IMIS_Gen.OfflineCHF
            If IsPostBack = True Then Return
            txtLastName.Attributes.Add("oncontextmenu", "RightClickJSFunction(this.id,'');")
            RunPageSecurity()
            Dim dtRegions As DataTable = Family.GetRegions(imisgen.getUserId(Session("User")), True)
            ddlRegion.DataSource = dtRegions
            ddlRegion.DataValueField = "RegionId"
            ddlRegion.DataTextField = "RegionName"
            ddlRegion.DataBind()
            If dtRegions.Rows.Count = 1 Then
                FillDistricts()
            End If
            ddlGender.DataSource = Family.GetGender
            ddlGender.DataValueField = "Code"
            ddlGender.DataTextField = "Gender"
            ddlGender.DataBind()
            ddlMarital.DataSource = Family.GetMaritalStatus
            ddlMarital.DataValueField = "Code"
            ddlMarital.DataTextField = "Status"
            ddlMarital.DataBind()

            ddlPoverty.DataSource = Family.GetPoverty
            ddlPoverty.DataValueField = "Code"
            ddlPoverty.DataTextField = "Status"
            ddlPoverty.DataBind()

            Session("ParentUrl") = "FindFamily.aspx"
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
    Private Sub FillDistricts()
        Dim dtDistricts As DataTable = Family.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataSource = dtDistricts
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
        GetWards()

    End Sub
    Private Sub RunPageSecurity()
        'Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        If Not userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Family, Page) Then

            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindFamily.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub ddlDistricts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged
        Try
            If Val(ddlDistrict.SelectedValue) > 0 Then
                GetWards()
            Else
                ddlWard.Items.Clear()
                ddlVillage.Items.Clear()
            End If
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody)
        End Try
    End Sub
    Private Sub gvWards_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWard.SelectedIndexChanged
        getVillages()
    End Sub
    Private Sub GetWards()
        Dim dtWards As DataTable = Family.GetWards(ddlDistrict.SelectedValue, True)
        Dim wards As Integer = ddlDistrict.SelectedValue
        If wards > 0 Then
            ddlWard.DataSource = dtWards
            ddlWard.DataValueField = "WardId"
            ddlWard.DataTextField = "WardName"
            ddlWard.DataBind()
        Else
            ddlWard.Items.Clear()
        End If
        getVillages(wards)
    End Sub
    Private Sub getVillages(Optional ByVal Wards As Integer = 1)
        If ddlWard.SelectedIndex < 0 Then
            ddlVillage.Items.Clear()
            Exit Sub
        End If

        If Wards > 0 And Not ddlWard.SelectedValue = 0 Then
            ddlVillage.DataSource = Family.GetVillages(ddlWard.SelectedValue, True)
            ddlVillage.DataValueField = "VillageId"
            ddlVillage.DataTextField = "VillageName"
            ddlVillage.DataBind()
        Else
            ddlVillage.Items.Clear()
        End If

    End Sub
    Private Sub loadgrid() Handles btnSearch.Click, chkLegacy.CheckedChanged, gvFamilies.PageIndexChanged, chkOffline.CheckedChanged
        Try
            loadSecurity()
            Dim eFamily As New IMIS_EN.tblFamilies
            Dim eInsuree As New IMIS_EN.tblInsuree

            'Dim bdate As Nullable(Of DateTime) = New DateTime(1900, 1, 1)
            'If IsDate(txtBirthDate.Text) Then bdate = FormatDateTime(txtBirthDate.Text, "dd/MM/yyyy")
            If Trim(txtBirthDateFrom.Text).Length > 0 Then
                If IsDate(txtBirthDateFrom.Text) Then
                    eInsuree.DOBFrom = Date.Parse(txtBirthDateFrom.Text)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If
            End If
            If Trim(txtBirthDateTo.Text).Length > 0 Then
                If IsDate(txtBirthDateTo.Text) Then
                    eInsuree.DOBTo = Date.Parse(txtBirthDateTo.Text)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If
            End If
            eFamily.AuditUserID = imisgen.getUserId(Session("User"))
            eFamily.isOffline = chkOffline.Checked
            eInsuree.LastName = txtLastName.Text
            eInsuree.OtherNames = txtOtherNames.Text
            eInsuree.CHFID = txtCHFID.Text
            eInsuree.Phone = txtPhone.Text
            eInsuree.Email = txtEmail.Text


            If Trim(ddlGender.SelectedValue).Length > 0 Then
                eInsuree.Gender = ddlGender.SelectedValue
            End If

            If Trim(ddlMarital.SelectedValue).Length > 0 Then
                eInsuree.Marital = ddlMarital.SelectedValue
            End If

            eFamily.ConfirmationNo = txtConfirmationNo.Text
            eFamily.RegionId = Val(ddlRegion.SelectedValue)
            eFamily.WardId = CInt(If(ddlWard.SelectedValue = String.Empty, 0, ddlWard.SelectedValue))
            eFamily.LocationId = CInt(If(ddlVillage.SelectedValue = String.Empty, 0, ddlVillage.SelectedValue))
            If Val(ddlDistrict.SelectedValue) > 0 Then eFamily.DistrictId = CInt(ddlDistrict.SelectedValue)
            Dim allPoverty As Boolean = True
            If Trim(ddlPoverty.SelectedValue).Length > 0 Then
                allPoverty = False
                eFamily.Poverty = ddlPoverty.SelectedValue
            End If

            eFamily.tblInsuree = eInsuree

            Dim dtFamilies As DataTable = Family.GetFamily(eFamily, chkLegacy.Checked, allPoverty)

            L_FOUNDFAMILY.Text = If(dtFamilies.Rows.Count = 0, imisgen.getMessage("L_NO"), Format(dtFamilies.Rows.Count, "#,###")) & " " & imisgen.getMessage("L_FOUNDFAMILY")
            gvFamilies.DataSource = dtFamilies
            gvFamilies.SelectedIndex = -1
            gvFamilies.DataBind()
            DisableButtonsOnEmptyRows(gvFamilies)

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody)
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

    End Sub
    Private Sub DisableButtonsOnEmptyRows(ByRef gv As GridView)
        If gv.Rows.Count = 0 Then
            B_VIEW.Enabled = False
        End If
    End Sub
    Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click
        If Not gvFamilies.SelectedDataKey.Item("FamilyID") Is Nothing Then
            Response.Redirect("ChangeFamily.aspx?f=" & gvFamilies.SelectedDataKey.Value)
        Else
            Response.Redirect("ChangeFamily.aspx?f=0")
        End If
    End Sub
    Protected Sub gvFamilies_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvFamilies.PageIndexChanging
        gvFamilies.PageIndex = e.NewPageIndex
        loadgrid()
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub
    Private Sub loadSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim AllowEdit As Boolean = userBI.RunPageSecurity(IMIS_EN.Enums.Pages.OverviewFamily, Page)

        Dim hlink As HyperLinkField = gvFamilies.Columns(1)
        If chkLegacy.Checked Then
            hlink.DataNavigateUrlFormatString = "ChangeFamily.aspx?f={0}"
        Else
            If AllowEdit Then
                hlink.DataNavigateUrlFormatString = "OverviewFamily.aspx?f={0}"
            Else
                'hlink.DataNavigateUrlFormatString = "ChangeFamily.aspx?f={0}"
                hlink.DataNavigateUrlFormatString = "#"
            End If
        End If
    End Sub
    Protected Sub chkLegacy_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLegacy.CheckedChanged
        If chkLegacy.Checked = True Then
            B_VIEW.Visible = False
        Else
            B_VIEW.Visible = False
        End If

    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        If Val(ddlRegion.SelectedValue) > 0 Then
            FillDistricts()
        Else
            ddlDistrict.Items.Clear()
            ddlWard.Items.Clear()
            ddlVillage.Items.Clear()
        End If

    End Sub
End Class
