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

Partial Public Class FindHealthFacility
    Inherits System.Web.UI.Page
    Dim hf As New IMIS_BI.FindHealthFacilityBI
    Dim eHF As New IMIS_EN.tblHF
    Private imisGen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Dim hfBI As New IMIS_BI.HealthFacilityBI
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvHF)
        If chkLegacy.Checked Then Page.ClientScript.RegisterForEventValidation(B_EDIT.UniqueID)
        MyBase.Render(writer)
    End Sub
   
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(11).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
            'row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub
    Private Sub loadSecurity()
     
    End Sub

    Private Sub HealthFacility_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()
        Try


            Dim RefUrl = Request.Headers("Referer")
            Dim reg As New Regex("FindHealthFacility", RegexOptions.IgnoreCase) '
            If IsPostBack = True Then

            End If

            If Not IsPostBack = True Then

                FillRegions()
                

                ddlType.DataSource = hf.GetHFType(True)
                ddlType.DataValueField = "Code"
                ddlType.DataTextField = "HFCareType"
                ddlType.DataBind()

                ddlLevel.DataSource = hf.GetHFLevel(True)
                ddlLevel.DataValueField = "Code"
                ddlLevel.DataTextField = "HFLevel"
                ddlLevel.DataBind()

                ddlLegal.DataSource = hf.GetHFLegal(True)
                ddlLegal.DataValueField = "LegalFormCode"
                ddlLegal.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "LegalForms", "AltLanguage")
                ddlLegal.DataBind()
                LoadGrid()
            Else
                If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                    B_DELETE_Click(sender, e)
                End If

            End If

        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisGen.Log(Page.Title & " : " & imisGen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub

    Private Sub FillRegions()
        Dim dtRegions As DataTable = hf.GetRegions(imisGen.getUserId(Session("User")), True, False)
        ddlRegion.DataSource = dtRegions
        ddlRegion.DataValueField = "RegionId"
        ddlRegion.DataTextField = "RegionName"
        ddlRegion.DataBind()
        If dtRegions.Rows.Count = 1 Then
            FillDistricts()
        End If
    End Sub
    Private Sub FillDistricts()
        ddlDistrict.DataSource = hf.GetDistricts(imisGen.getUserId(Session("User")), True, Val(ddlRegion.SelectedValue))
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False, Optional ByVal cmd As String = "delete")
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = imisGen.getUserId(Session("User"))
        If Not ondelete Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.HealthFacility, Page) Then
                B_ADD.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.HealthFacilityAdd, UserID)
                B_EDIT.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.HealthFacilityEdit, UserID)
                B_DELETE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.HealthFacilityDelete, UserID)
                B_SEARCH.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.HealthFacilitySearch, UserID)

                If Not B_EDIT.Visible And Not B_DELETE.Visible Then
                    pnlGrid.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindHealthFacility.ToString & "&retUrl=" & RefUrl)
            End If
        Else

            If Not userBI.checkRights(IMIS_EN.Enums.Rights.HealthFacilityDelete, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindHealthFacility.ToString & "&retUrl=" & RefUrl)
            End If

        End If
    End Sub
    Private Sub LoadGrid() Handles chkLegacy.CheckedChanged, B_SEARCH.Click, gvHF.PageIndexChanged
        Try
            Dim eLocations As New IMIS_EN.tblLocations
            lblMsg.Text = ""
            loadSecurity()
            eHF.AuditUserID = imisGen.getUserId(Session("User"))
            eHF.HFName = txtName.Text.Trim
            eHF.HFCode = txtHFCode.Text.Trim
            eHF.Phone = txtPhone.Text.Trim

            Dim RegionId As Integer?
            Dim DistrictId As Integer?

            If ddlRegion.SelectedValue = "" OrElse ddlRegion.SelectedValue = 0 Then
                RegionId = 0
            ElseIf ddlRegion.SelectedValue = -1 Then
                RegionId = Nothing
            Else
                RegionId = ddlRegion.SelectedValue
            End If

            If ddlDistrict.SelectedValue = "" OrElse ddlDistrict.SelectedValue = 0 Then
                DistrictId = 0
            Else
                DistrictId = ddlDistrict.SelectedValue
            End If

            eLocations.RegionId = RegionId
            eLocations.DistrictID = DistrictId
            eHF.tblLocations = eLocations
            eHF.Fax = txtFax.Text.Trim
            eHF.eMail = txtEmail.Text.Trim
            eHF.HFCareType = ddlType.SelectedValue
            eHF.HFLevel = ddlLevel.SelectedValue
            eHF.LegalForm = ddlLegal.SelectedValue
            getGridData()

        Catch ex As Exception
            Session("Msg") = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Log(Page.Title & " : " & imisGen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

        End Try

    End Sub

    Private Sub getGridData()
        Dim dtHF As DataTable = hf.GetHealthFacility(eHF, chkLegacy.Checked)
        Dim sindex As Integer = 0
        Dim dv As DataView = dtHF.DefaultView
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("h") Is Nothing Then
                eHF.HFCode = HttpContext.Current.Request.QueryString("h")
            Else
                eHF.HFCode = hfHFCode.Value
            End If
            If Not eHF.HFCode = "" Then
                dv.Sort = "HFCode"
                Dim x As Integer = dv.Find(eHF.HFCode)
                If x >= 0 Then
                    gvHF.PageIndex = Int(x / gvHF.PageSize)
                    Math.DivRem(x, gvHF.PageSize, sindex)
                End If
            End If
        End If
        L_FOUNDHFACILITIES.Text = If(dv.ToTable.Rows.Count = 0, imisGen.getMessage("L_NO"), Format(dv.ToTable.Rows.Count, "#,###")) & " " & imisGen.getMessage("L_FOUNDHFACILITIES")
        gvHF.DataSource = dv
        gvHF.SelectedIndex = sindex
        gvHF.DataBind()
        EnableButtons(gvHF.Rows.Count)
    End Sub

    Protected Sub B_ADD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_ADD.Click

        Response.Redirect("HealthFacility.aspx")
    End Sub

    Protected Sub B_EDIT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_EDIT.Click
        Response.Redirect("HealthFacility.aspx?h=" & hfHFId.Value)
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub
    Private Sub EnableButtons(ByVal rows As Integer)
        If rows = 0 Then

            B_DELETE.Visible = False
            B_EDIT.Visible = False
            'B_VIEW.Visible = False
            B_ADD.Visible = True

        Else
            If chkLegacy.Checked = True Then
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                'B_VIEW.Visible = True
                B_ADD.Visible = B_ADD.Visible

            Else
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                'B_VIEW.Visible = False

                B_ADD.Visible = B_ADD.Visible
            End If

        End If
    End Sub
    Private Sub B_DELETE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DELETE.Click
        RunPageSecurity(True)
        Try
            lblMsg.Text = ""

            Dim HfUUID As Guid = Guid.Parse(hfHFId.Value)
            Dim HfId As Integer = hfBI.GetHfIdByUUID(HfUUID)
            eHF.HfID = HfId

            eHF.AuditUserID = imisGen.getUserId(Session("User"))
            hf.DeleteHealthFacility(eHF)
            Dim FHF As String = hfHFCode.Value
            LoadGrid()
            Session("msg") = FHF & " " & imisGen.getMessage("M_DELETED")
        Catch ex As Exception
            Session("Msg") = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Log(Page.Title & " : " & imisGen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

        End Try
    End Sub

    Protected Sub gvHF_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvHF.PageIndexChanging
        gvHF.PageIndex = e.NewPageIndex
    End Sub

    'Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click

    '    Response.Redirect("HealthFacility.aspx?h=" & hfHFId.Value & "&r=1")
    'End Sub

    'Private Sub gvHF_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvHF.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Attributes.Add("onmouseover", "this.previous_color=this.className;this.className='Hover'")
    '        e.Row.Attributes.Add("onmouseout", "this.className=this.previous_color;")
    '        e.Row.Attributes.Add("onclick", "javascript:ChangeClass('" & e.Row.ClientID & "'," & e.Row.RowIndex & ");this.previous_color=this.className")
    '    End If
    'End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
    End Sub
End Class
