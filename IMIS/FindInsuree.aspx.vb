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

Partial Public Class FindInsuree
    Inherits System.Web.UI.Page

    Private Insuree As New IMIS_BI.FindInsureeBI
    Private eInsuree As New IMIS_EN.tblInsuree
    Protected imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvInsuree)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        Try

            For Each row As GridViewRow In gv.Rows
                If Not row.Cells(11).Text = "&nbsp;" Then
                    row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
                End If
                '  row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
            Next

        Catch ex As Exception

        End Try
    End Sub
    Private Sub FormatForm()
        Dim Adjustibility As String = ""
        Adjustibility = General.getControlSetting("MaritalStatus")
        L_MARITAL.Visible = Not (Adjustibility = "N")
        ddlMarital.Visible = Not (Adjustibility = "N")

        For i As Integer = 0 To gvInsuree.Columns.Count
            If gvInsuree.Columns(i).HeaderText.Equals(imisgen.getMessage("L_MARITAL")) Then gvInsuree.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
        Next
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Return
        RunPageSecurity()
        Try
            FormatForm()
            chkOffline.Visible = IMIS_Gen.OfflineCHF
            Dim dtRegions As DataTable = Insuree.GetRegions(imisgen.getUserId(Session("User")), True)
            ddlRegion.DataSource = dtRegions
            ddlRegion.DataValueField = "RegionId"
            ddlRegion.DataTextField = "RegionName"
            ddlRegion.DataBind()
            If dtRegions.Rows.Count = 1 Then
                FillDistrict()
            End If

            ddlGender.DataSource = Insuree.GetGender
            ddlGender.DataValueField = "Code"
            ddlGender.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "Gender", "AltLanguage")
            ddlGender.DataBind()
            ddlMarital.DataSource = Insuree.GetMaritalStatus
            ddlMarital.DataValueField = "Code"
            ddlMarital.DataTextField = "Status"
            ddlMarital.DataBind()


            ddlPhotoAssigned.DataSource = Insuree.GetPhotoAssigned
            ddlPhotoAssigned.DataValueField = "PhotoAssignedValue"
            ddlPhotoAssigned.DataTextField = "PhotoAssignedText"
            ddlPhotoAssigned.DataBind()


            Session("ParentUrl") = "FindInsuree.aspx"

        Catch ex As Exception
            Session("Msg") = imisgen.getMessage("M_ERRORMESSAGE")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

        End Try


    End Sub
    Private Sub FillDistrict()
        Dim dtDistricts As DataTable = Insuree.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataSource = dtDistricts
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
        If dtDistricts.Rows.Count = 1 Then
            GetWards()
        End If
    End Sub
    Private Sub RunPageSecurity()
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Insuree, Page) Then
            B_VIEW.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.InsureeSearch, UserID)
            B_CLAIM.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ClaimSearch, UserID)
            B_CLAIMSREVIEWS.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ClaimSearch, UserID)
            btnSearch.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.InsureeSearch, UserID)

        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindInsuree.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub

    Private Sub loadgrid() Handles btnSearch.Click, chkLegacy.CheckedChanged, gvInsuree.PageIndexChanged, chkOffline.CheckedChanged
        Try
            loadSecurity()
            Dim eFamily As New IMIS_EN.tblFamilies
            Dim eInsuree As New IMIS_EN.tblInsuree

            'Dim bdate As Nullable(Of DateTime) = New DateTime(1900, 1, 1)
            'If IsDate(txtBirthDate.Text) Then bdate = FormatDateTime(txtBirthDate.Text, "dd/MM/yyyy")


            eInsuree.AuditUserID = imisgen.getUserId(Session("User"))
            eInsuree.LastName = txtLastName.Text
            eInsuree.OtherNames = txtOtherNames.Text
            eInsuree.CHFID = txtCHFID.Text
            eInsuree.Email = txtEmail.Text
            If Trim(txtBirthDateFrom.Text).Length > 0 Then
                If IsDate(txtBirthDateFrom.Text) Then
                    eInsuree.DOBFrom = Date.Parse(txtBirthDateFrom.Text)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If
            End If
            If Trim(txtBirthDateto.Text).Length > 0 Then
                If IsDate(txtBirthDateTo.Text) Then
                    eInsuree.DOBTo = Date.Parse(txtBirthDateTo.Text)
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If
            End If
            eInsuree.Phone = txtPhone.Text
            eInsuree.Gender = ddlGender.SelectedValue
            eInsuree.Marital = ddlMarital.SelectedValue
            eInsuree.isOffline = chkOffline.Checked

            eFamily.RegionId = Val(ddlRegion.SelectedValue)
            If Val(ddlDistrict.SelectedValue) > 0 Then eFamily.DistrictID = ddlDistrict.SelectedValue
            If Not Val(ddlVillage.SelectedValue) = 0 Then
                eFamily.LocationId = ddlVillage.SelectedValue
            End If
            If Not Val(ddlWard.SelectedValue) = 0 Then
                eFamily.WardID = ddlWard.SelectedValue
            End If
           
            eInsuree.tblFamilies1 = eFamily


            Dim dtInsuree As DataTable = Insuree.FindInsuree(eInsuree, chkLegacy.Checked, ddlPhotoAssigned.SelectedValue, Request.Cookies("CultureInfo").Value)
            L_FOUNDINSUREE.Text = If(dtInsuree.Rows.Count = 0, imisgen.getMessage("L_NO"), Format(dtInsuree.Rows.Count, "#,###")) & " " & imisgen.getMessage("L_FOUNDINSUREE")
            gvInsuree.DataSource = dtInsuree
            gvInsuree.SelectedIndex = -1
            gvInsuree.DataBind()
            DisableButtonsOnEmptyRows(gvInsuree)
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
    Private Sub GetWards()
        Dim dtWards As DataTable = Insuree.GetWards(ddlDistrict.SelectedValue, True)
        Dim wards As Integer = dtWards.Rows.Count
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
        If ddlWard.SelectedIndex = -1 Then Exit Sub
        If Wards > 0 And Not ddlWard.SelectedValue = 0 Then
            ddlVillage.DataSource = Insuree.GetVillages(ddlWard.SelectedValue, True)
            ddlVillage.DataValueField = "VillageId"
            ddlVillage.DataTextField = "VillageName"
            ddlVillage.DataBind()
        Else
            ddlVillage.Items.Clear()
        End If

    End Sub
    Private Sub ddlDistricts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged
        If Val(ddlDistrict.SelectedValue) > 0 Then
            GetWards()
        Else
            ddlWard.Items.Clear()
            ddlVillage.Items.Clear()
        End If

    End Sub
    Private Sub gvWards_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWard.SelectedIndexChanged
        If Val(ddlWard.SelectedValue) > 0 Then
            getVillages()
        Else
            ddlVillage.Items.Clear()
        End If

    End Sub

    Protected Sub gvInsuree_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvInsuree.PageIndexChanging
        gvInsuree.PageIndex = e.NewPageIndex

    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub
    Private Sub loadSecurity()

        Dim AllowEdit As Boolean = userBI.RunPageSecurity(IMIS_EN.Enums.Pages.OverviewFamily, Page)

        Dim hlink As HyperLinkField = gvInsuree.Columns(0)
        If chkLegacy.Checked Then
            hlink.DataNavigateUrlFormatString = "Insuree.aspx?f={0}&i={0}"
        Else
            If AllowEdit Then
                hlink.DataNavigateUrlFormatString = "OverviewFamily.aspx?f={0}&i={1}"
            Else
                hlink.DataNavigateUrlFormatString = "Insuree.aspx?f={0}&i={0}"
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
    Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click
        If Not gvInsuree.SelectedDataKey.Item("FamilyID") Is Nothing Then
            Response.Redirect("Insuree.aspx?f=" & gvInsuree.SelectedDataKey.Item("FamilyID") & "&i=" & gvInsuree.SelectedDataKey.Item("InsureeID"))
        Else
            Response.Redirect("Insuree.aspx?f=0")
        End If
    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        If Val(ddlRegion.SelectedValue) > 0 Then
            FillDistrict()
        Else
            ddlDistrict.Items.Clear()
            ddlWard.Items.Clear()
            ddlVillage.Items.Clear()

        End If

    End Sub
    Protected Sub B_CLAIM_Click(sender As Object, e As EventArgs) Handles B_CLAIM.Click
        If gvInsuree.Rows.Count > 0 Then
            Dim InsuranceNumber = hfInsuranceNumber.Value
            Response.Redirect("FindClaims.aspx?i=" & InsuranceNumber)
        End If


    End Sub

    Protected Sub B_CLAIMSREVIEWS_Click(sender As Object, e As EventArgs) Handles B_CLAIMSREVIEWS.Click
        If gvInsuree.Rows.Count > 0 And Not hfInsuranceNumber.Value Is Nothing Then
            Dim InsuranceNumber = hfInsuranceNumber.Value
            Response.Redirect("ClaimOverview.aspx?i=" & InsuranceNumber)
        End If
    End Sub
End Class
