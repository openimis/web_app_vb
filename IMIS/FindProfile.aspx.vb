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


Imports IMIS_BI

Partial Public Class FindProfile
    Inherits System.Web.UI.Page

    Dim eRole As New IMIS_EN.tblRole
    Private imisGen As New IMIS_Gen
    Private BIFindRole As New IMIS_BI.FindProfileBI
    Private roleBI As New IMIS_BI.RoleRightBI

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvRole)
        If chkLegacy.Checked Then Page.ClientScript.RegisterForEventValidation(B_EDIT.UniqueID)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(4).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If

        Next
    End Sub

    Private Sub Role_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RunPageSecurity()

        Try


            If IsPostBack = True Then
                If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                    B_DELETE_Click(sender, e)
                End If
            End If

            If Not IsPostBack = True Then

                'HIREN: Line below checks for the userid which I think is wrong, should check for the roleId instead
                'Dim roles As Integer = if(imisGen.getUserId(Session("User")) = 524288, 525184, 1023)

                loadgrid()


                'here 000  

            End If

        Catch ex As Exception
            Session("Msg") = imisGen.getMessage("M_ERRORMESSAGE")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

        End Try

    End Sub
    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False)
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = imisGen.getUserId(Session("User"))

        If Not ondelete Then
            If BIFindRole.RunPageSecurity(IMIS_EN.Enums.Pages.UserProfiles, Page) Then
                B_ADD.Visible = BIFindRole.checkRights(IMIS_EN.Enums.Rights.AddUserProfile, UserID)
                B_EDIT.Visible = BIFindRole.checkRights(IMIS_EN.Enums.Rights.EditUserProfile, UserID)
                B_DELETE.Visible = BIFindRole.checkRights(IMIS_EN.Enums.Rights.DeleteUserProfile, UserID)
                B_SEARCH.Visible = BIFindRole.checkRights(IMIS_EN.Enums.Rights.FindUserProfile, UserID)
                B_DUPLICATE.Visible = BIFindRole.checkRights(IMIS_EN.Enums.Rights.DuplicateUserProfile, UserID)
                If Not B_EDIT.Visible And Not B_DELETE.Visible Then
                    pnlGrid.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.UserProfiles.ToString & "&retUrl=" & RefUrl)
            End If

        Else

            If Not BIFindRole.checkRights(IMIS_EN.Enums.Rights.DeleteUserProfile, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.UserProfiles.ToString & "&retUrl=" & RefUrl)
            End If
        End If

    End Sub

    Private Sub loadgrid() Handles B_SEARCH.Click, chkLegacy.CheckedChanged, gvRole.PageIndexChanged
        Try
            lblMsg.Text = ""

            getGridData()

        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub getGridData()
        eRole.RoleName = txtRolename.Text
        eRole.AltLanguage = txtRolename.Text
        If ddlBlocked.SelectedIndex > 0 Then
            If ddlBlocked.SelectedValue = "True" Then
                eRole.IsBlocked = 1
            Else
                eRole.IsBlocked = 0
            End If

        End If


        If ddlSystem.SelectedIndex > 0 Then

            If ddlSystem.SelectedValue = "True" Then
                eRole.IsSystem = 1
            Else
                eRole.IsSystem = 0
            End If

        End If
        If chkLegacy.Checked Then
            eRole.LegacyID = 1
        End If

        Dim dtRole As DataTable = BIFindRole.GetRoles(eRole)
        Dim sindex As Integer = 0
        Dim dv As DataView = dtRole.DefaultView
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("u") Is Nothing Then
                eRole.RoleID = HttpContext.Current.Request.QueryString("u")
            End If
            If Not eRole.RoleName = "" Then
                dv.Sort = "RoleName"
                Dim x As Integer = dv.Find(eRole.RoleID)
                If x >= 0 Then
                    gvRole.PageIndex = Int(x / 15)
                    Math.DivRem(x, 15, sindex)
                End If
            End If
        End If
        L_ROLEFOUNDS.Text = If(dv.ToTable.Rows.Count = 0, imisGen.getMessage("L_NO"), Format(dv.ToTable.Rows.Count, "#,###")) & " " & imisGen.getMessage("L_ROLEFOUNDS")
        gvRole.DataSource = dv
        gvRole.SelectedIndex = sindex
        gvRole.DataBind()
        EnableButtons(gvRole.Rows.Count)

    End Sub
    Protected Sub B_ADD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_ADD.Click
        Response.Redirect("Role.aspx")
    End Sub
    Protected Sub B_EDIT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_EDIT.Click
        Response.Redirect("Role.aspx?r=" & hfRoleId.Value)
    End Sub

    Private Sub B_DELETE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DELETE.Click, gvRole.SelectedIndexChanged
        RunPageSecurity(True)
        Dim RoleUUID As Guid = Guid.Parse(hfRoleId.Value)
        Dim RoleId As Integer = roleBI.GetRoleIdByUUID(RoleUUID)
        Dim Role As String = hfRoleName.Value
        Dim IsAssoc As Boolean = BIFindRole.IsRoleInUse(RoleId)

        If IsAssoc = True Then
            imisGen.Alert(Role & " " & imisGen.getMessage("M_NOTDELETEASSOCIATEDROLE"), pnlButtons, alertPopupTitle:="IMIS")
            Return
        End If
        Try

            eRole.RoleID = RoleId

            lblMsg.Text = ""
            eRole.AuditUserID = imisGen.getUserId(Session("User"))
            BIFindRole.DeleteRole(eRole)
            loadgrid()
            Session("msg") = Role & " " & imisGen.getMessage("M_DELETED")
        Catch ex As Exception

            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub

    Protected Sub gvRole_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRole.PageIndexChanging
        gvRole.PageIndex = e.NewPageIndex

    End Sub
    Private Sub EnableButtons(ByVal rows As Integer)
        If rows = 0 Then

            B_DELETE.Visible = False
            B_EDIT.Visible = False
            B_ADD.Visible = B_ADD.Visible
            B_DUPLICATE.Visible = False
        Else
            If chkLegacy.Checked = True Then
                B_DELETE.Visible = False
                B_EDIT.Visible = False
                B_ADD.Visible = False
                B_DUPLICATE.Visible = False
            Else
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                B_ADD.Visible = B_ADD.Visible
                B_DUPLICATE.Visible = B_DUPLICATE.Visible
            End If

        End If
    End Sub


    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx?")
    End Sub

    Private Sub B_DUPLICATE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DUPLICATE.Click
        Response.Redirect("role.aspx?r=" & hfRoleId.Value & "&action=duplicate")
    End Sub
End Class
