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

Public Partial Class FindClaimAdministrator
    Inherits System.Web.UI.Page

#Region "Members"
    Protected ImisGen As New IMIS_Gen
    Private eClaimAdmin As New IMIS_EN.tblClaimAdmin
    Private eHF As IMIS_EN.tblHF
    Private BIFindClaimAdmin As New IMIS_BI.FindClaimAdministratorBI
    Private BIClaimAdmin As New IMIS_BI.ClaimAdministratorBI
#End Region
#Region "Events"
#Region "Page"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()
        lblMsg.Text = ""
        Try
            If IsPostBack = True Then
                If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                    RunPageSecurity(True)
                    If Not DeleteClaimAdministrator() Then Exit Sub
                End If
            End If
            If IsPostBack = False Then
                FillHFCodes()
                SetEntity()
                loadGrid()
            End If
        Catch ex As Exception
            ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & ImisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvClaimAdministrators)
        If chkLegacy.Checked Then Page.ClientScript.RegisterForEventValidation(B_EDIT.UniqueID)
        MyBase.Render(writer)
    End Sub
#End Region
#Region "Buttons"
    Protected Sub B_SEARCH_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_SEARCH.Click
        Try
            SetEntity()
            loadGrid()
        Catch ex As Exception
            ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & ImisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Protected Sub B_ADD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_ADD.Click
        Response.Redirect("ClaimAdministrator.aspx?a=0")
    End Sub
    Protected Sub B_EDIT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_EDIT.Click
        Response.Redirect("ClaimAdministrator.aspx?a=" & hfClaimAdministratorId.Value)
    End Sub
    Private Sub B_DELETE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DELETE.Click
        'This event does not fire automatically on delete button click, since the click is further handled by javascript ( delete confirmation popup window )
        RunPageSecurity(True)
        Try
            DeleteClaimAdministrator()
        Catch ex As Exception
            ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & ImisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub
#End Region
#Region "CheckBox"
    Protected Sub chkLegacy_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkLegacy.CheckedChanged
        Try
            SetEntity()
            loadGrid()
        Catch ex As Exception
            ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & ImisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
#End Region
#Region "GridView"
    'Private Sub gvofficers_rowdatabound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvClaimAdministrators.RowDataBound
    '    If e.row.rowtype = datacontrolrowtype.datarow Then
    '        e.row.attributes.add("onmouseover", "this.previous_color=this.classname;this.classname='hover'")
    '        e.row.attributes.add("onmouseout", "this.classname=this.previous_color;")
    '        e.row.attributes.add("onclick", "javascript:changeclass('" & e.row.clientid & "'," & e.row.rowindex & ");this.previous_color=this.classname")
    '    End If
    'End Sub
    Private Sub gvClaimAdministrators_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvClaimAdministrators.PageIndexChanging
        Try
            gvClaimAdministrators.PageIndex = e.NewPageIndex
            SetEntity()
            loadGrid()
        Catch ex As Exception
            ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & ImisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
#End Region
#End Region
#Region "Functions & Procedures"
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(9).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
        Next
    End Sub
    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False)
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = ImisGen.getUserId(Session("User"))
        If Not ondelete Then
            If BIFindClaimAdmin.RunPageSecurity(IMIS_EN.Enums.Pages.ClaimAdministrator, Page) Then
                B_ADD.Visible = BIFindClaimAdmin.checkRights(IMIS_EN.Enums.Rights.AddClaimAdministrator, UserID)
                B_EDIT.Visible = BIFindClaimAdmin.checkRights(IMIS_EN.Enums.Rights.EditClaimAdministrator, UserID)
                B_DELETE.Visible = BIFindClaimAdmin.checkRights(IMIS_EN.Enums.Rights.DeleteClaimAdministrator, UserID)
                B_SEARCH.Visible = BIFindClaimAdmin.checkRights(IMIS_EN.Enums.Rights.FindClaimAdministrator, UserID)
                If Not B_EDIT.Visible And Not B_DELETE.Visible Then
                    pnlGrid.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindClaimAdministrator.ToString & "&retUrl=" & RefUrl)
            End If
        Else
            If Not BIFindClaimAdmin.checkRights(IMIS_EN.Enums.Rights.DeleteClaimAdministrator, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindClaimAdministrator.ToString & "&retUrl=" & RefUrl)
            End If
        End If
    End Sub
    Private Sub FillHFCodes()
        Dim UserID As Integer = ImisGen.getUserId(Session("User"))
        ddlHFCode.DataSource = BIFindClaimAdmin.GetHFCodes(UserID, 0)
        ddlHFCode.DataValueField = "HfID"
        ddlHFCode.DataTextField = "HFCODE"
        ddlHFCode.DataBind()
    End Sub
    Private Sub loadGrid()
        Dim dtUsers As DataTable = BIFindClaimAdmin.GetClaimAdmins(eClaimAdmin, chkLegacy.Checked)
        Dim sindex As Integer = 0
        Dim dv As DataView = dtUsers.DefaultView
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("a") Is Nothing Then
                eClaimAdmin.ClaimAdminCode = HttpContext.Current.Request.QueryString("a")
            Else
                eClaimAdmin.ClaimAdminCode = hfClaimAdministratorCode.Value
            End If
            If eClaimAdmin.ClaimAdminCode.Trim <> String.Empty Then
                dv.Sort = "ClaimAdminCode"
                Dim x As Integer = dv.Find(eClaimAdmin.ClaimAdminCode)
                If x >= 0 Then
                    gvClaimAdministrators.PageIndex = Int(x / 15)
                    Math.DivRem(x, 15, sindex)
                End If
            End If
        End If
        lblFoundClaimAdministrators.Text = If(dv.ToTable.Rows.Count = 0, ImisGen.getMessage("L_NO"), Format(dv.ToTable.Rows.Count, "#,###")) & " " & ImisGen.getMessage("L_FOUNDCLAIMADMINISTRATORS")
        gvClaimAdministrators.DataSource = dv
        gvClaimAdministrators.SelectedIndex = sindex
        gvClaimAdministrators.DataBind()
        EnableButtons(gvClaimAdministrators.Rows.Count)
    End Sub
    Private Sub EnableButtons(ByVal rows As Integer)
        If rows = 0 Then
            B_DELETE.Visible = False
            B_EDIT.Visible = False
            B_ADD.Visible = True
        Else
            If chkLegacy.Checked = True Then
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                B_ADD.Visible = B_ADD.Visible
            Else
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                B_ADD.Visible = B_ADD.Visible
            End If
        End If
    End Sub
    Private Sub SetEntity()
        eClaimAdmin.ClaimAdminCode = txtCode.Text.Trim
        eClaimAdmin.LastName = txtLastName.Text.Trim
        If txtDOBFrom.Text.Length > 0 Then
            If IsDate(txtDOBFrom.Text.Trim) Then
                eClaimAdmin.DOBFrom = Date.Parse(txtDOBFrom.Text.Trim)
            Else
                ImisGen.Alert(ImisGen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If
       

        If txtDOBTo.Text.Length > 0 Then
            If IsDate(txtDOBTo.Text.Trim) Then
                eClaimAdmin.DOBTo = Date.Parse(txtDOBTo.Text.Trim)
            Else
                ImisGen.Alert(ImisGen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If
 

        eClaimAdmin.Phone = txtPhone.Text
        eHF = New IMIS_EN.tblHF
        If ddlHFCode.SelectedIndex > -1 Then eHF.HfID = ddlHFCode.SelectedValue
        eClaimAdmin.tblHF = eHF
        eClaimAdmin.OtherNames = txtOtherNames.Text.Trim
        eClaimAdmin.AuditUserId = ImisGen.getUserId(Session("User"))
        eClaimAdmin.EmailId = txtEmail.Text
    End Sub
    Private Function DeleteClaimAdministrator() As Boolean
        lblMsg.Text = ""
        eClaimAdmin.ClaimAdminId = hfClaimAdministratorId.Value
        eClaimAdmin.ClaimAdminCode = hfClaimAdministratorCode.Value
        eClaimAdmin.AuditUserId = ImisGen.getUserId(Session("User"))
        If hfHasLogin.Value = "True" Then
            DeleteAssociatedUser()
        End If
        Dim Chk As Integer = BIFindClaimAdmin.DeleteClaimAdmin(eClaimAdmin)
        Dim AdminCode As String = hfClaimAdministratorCode.Value
        SetEntity()
        loadGrid()
        If Chk = 1 Then
            lblMsg.Text = AdminCode & " " & ImisGen.getMessage("M_DELETED")
        ElseIf Chk = -1 Then
            ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            Return False
        End If
        Return True
    End Function

    Private Sub DeleteAssociatedUser()
        Try
            eClaimAdmin.eUsers = New IMIS_EN.tblUsers
            eClaimAdmin.eUsers.LoginName = eClaimAdmin.ClaimAdminCode
            BIClaimAdmin.LoadUsers(eClaimAdmin.eUsers)
            BIClaimAdmin.DeleteUser(eClaimAdmin.eUsers)
        Catch ex As Exception
            ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & ImisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
#End Region
End Class
