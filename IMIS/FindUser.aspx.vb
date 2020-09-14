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



Partial Public Class FindUser
    Inherits System.Web.UI.Page
    Private users As New IMIS_BI.FindUsersBI
    Dim eUser As New IMIS_EN.tblUsers
    Private imisGen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvUsers)
        If chkLegacy.Checked Then Page.ClientScript.RegisterForEventValidation(B_EDIT.UniqueID)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(6).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
        Next
    End Sub
   
    Private Sub Users_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RunPageSecurity()

        Try


            If IsPostBack = True Then
                If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                    B_DELETE_Click(sender, e)
                End If
            End If

            If Not IsPostBack = True Then

                'HIREN: Line below checks for the userid which I think is wrong, should check for the roleId instead


                ddlRole.DataSource = users.GetRoles(IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF)
                ddlRole.DataValueField = "RoleID"
                ddlRole.DataTextField = "RoleName"
                ddlRole.DataBind()
                'here 000
                Dim dtRegions As DataTable = users.GetRegions(imisGen.getUserId(Session("User")), True)
                ddlRegion.DataSource = dtRegions
                ddlRegion.DataValueField = "RegionId"
                ddlRegion.DataTextField = "RegionName"
                ddlRegion.DataBind()
                If dtRegions.Rows.Count = 1 Then
                    FillDistricts()
                End If
                ddlLanguage.DataSource = users.GetLanguage
                ddlLanguage.DataValueField = "LanguageCode"
                ddlLanguage.DataTextField = "LanguageName"
                ddlLanguage.DataBind()
                ddlHFName.DataSource = users.GetHFCodes(imisGen.getUserId(Session("User")), Val(ddlDistrict.SelectedValue))
                ddlHFName.DataValueField = "Hfid"
                ddlHFName.DataTextField = "HFCode"
                ddlHFName.DataBind()
                loadgrid()
            End If

        Catch ex As Exception
            Session("Msg") = imisGen.getMessage("M_ERRORMESSAGE")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

        End Try

    End Sub
    Private Sub FillDistricts()
        ddlDistrict.DataSource = users.GetDistricts(imisGen.getUserId(Session("User")), True, Val(ddlRegion.SelectedValue))
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False)
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisGen.getRoleId(Session("User"))
        Dim UserID As Integer = imisGen.getUserId(Session("User"))
        If Not ondelete Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.FindUser, Page) Then
                B_ADD.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.UsersAdd, UserID)
                B_EDIT.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.UsersEdit, UserID)
                B_DELETE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.UsersDelete, UserID)
                B_SEARCH.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.UsersSearch, UserID)
                If Not B_EDIT.Visible And Not B_DELETE.Visible Then
                    pnlGrid.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindUser.ToString & "&retUrl=" & RefUrl)
            End If

        Else

            If Not userBI.checkRights(IMIS_EN.Enums.Rights.UsersDelete, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindUser.ToString & "&retUrl=" & RefUrl)
            End If
        End If

    End Sub

    Private Sub loadgrid() Handles B_SEARCH.Click, chkLegacy.CheckedChanged, gvUsers.PageIndexChanged
        Try
            lblMsg.Text = ""
            eUser.AuditUserID = imisGen.getUserId(Session("User"))
            eUser.LastName = txtLastName.Text
            eUser.OtherNames = txtOtherNames.Text
            If Not ddlRole.SelectedValue = 0 Then
                eUser.RoleID = ddlRole.SelectedValue
            End If
            eUser.LoginName = txtUsername.Text
            eUser.Phone = txtPhone.Text
            If ddlHFName.SelectedIndex >= 0 Then
                eUser.HFID = ddlHFName.SelectedValue
            End If



            '  If Not ddlLanguage.SelectedValue = -1 Then
            eUser.LanguageID = ddlLanguage.SelectedValue
            '   End If
            eUser.EmailId = txtEmail.Text

            getGridData()

        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub getGridData()
        Dim eLocations As New IMIS_EN.tblLocations
        Dim LocationId As Integer
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
        eLocations.DistrictId = DistrictId
        eUser.tblLocations = eLocations
        Dim Authority As Integer = imisGen.getUserId(Session("User"))
        Dim dtUsers As DataTable = users.GetUsers(eUser, chkLegacy.Checked, LocationId, Authority)
        Dim sindex As Integer = 0
        Dim dv As DataView = dtUsers.DefaultView
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("u") Is Nothing Then
                eUser.LoginName = HttpContext.Current.Request.QueryString("u")
            End If
            If Not eUser.LoginName = "" Then
                dv.Sort = "LoginName"
                Dim x As Integer = dv.Find(eUser.LoginName)
                If x >= 0 Then
                    gvUsers.PageIndex = Int(x / 15)
                    Math.DivRem(x, 15, sindex)
                End If
            End If
        End If
        L_FOUNDUSERS.Text = If(dv.ToTable.Rows.Count = 0, imisGen.getMessage("L_NO"), Format(dv.ToTable.Rows.Count, "#,###")) & " " & imisGen.getMessage("L_FOUNDUSERS")
        gvUsers.DataSource = dv
        gvUsers.SelectedIndex = sindex
        gvUsers.DataBind()
        EnableButtons(gvUsers.Rows.Count)

    End Sub
    Public Function CheckDifferenceandSave(ByVal grid As GridView, ByVal RowIndex As Integer) As Boolean

        Dim chkSelect As CheckBox = CType(grid.Rows(RowIndex).Cells(0).Controls(1), CheckBox)
        If chkSelect.Checked <> CBool(grid.DataKeys(grid.Rows(RowIndex).RowIndex).Value) Then
            Return True
        Else
            Return False
        End If


    End Function
    Protected Sub B_ADD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_ADD.Click
        Response.Redirect("User.aspx")
    End Sub
    Protected Sub B_EDIT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_EDIT.Click
        Response.Redirect("User.aspx?u=" & hfUserId.Value & "&r=0")
    End Sub
    Private Sub B_DELETE_Click(sender As Object, e As EventArgs) Handles B_DELETE.Click, gvUsers.SelectedIndexChanged
        RunPageSecurity(True)
        Dim UserUUID As Guid = Guid.Parse(hfUserId.Value)
        Dim UserId As Integer = userBI.GetUserIdByUUID(UserUUID)
        Dim User As String = hfUserName.Value
        Dim IsAssoc As Boolean = userBI.IsUserExists(UserId)

        If IsAssoc = True Then
            imisGen.Alert(User & " " & imisGen.getMessage("M_NOTDELETEASSOCIATEDUSER"), pnlButtons, alertPopupTitle:="IMIS")
            Return
        End If
        Dim result = users.GetUserDistricts(imisGen.getUserId(Session("User")), UserId)
        If result = 1 Then
            imisGen.Alert(imisGen.getMessage("M_USERCANNOTBEDELETED"), pnlButtons, alertPopupTitle:="IMIS")
            Return
        End If
        Try
            eUser.UserID = UserId

            lblMsg.Text = ""
            eUser.AuditUserID = imisGen.getUserId(Session("User"))
            users.DeleteUser(eUser)
            loadgrid()
            Session("msg") = User & " " & imisGen.getMessage("M_DELETED")
        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Protected Sub gvUsers_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvUsers.PageIndexChanging
        gvUsers.PageIndex = e.NewPageIndex

    End Sub
    Private Sub EnableButtons(ByVal rows As Integer)
        If rows = 0 Then

            B_DELETE.Visible = False
            B_EDIT.Visible = False
            B_ADD.Visible = B_ADD.Visible
        Else
            If chkLegacy.Checked = True Then
                B_DELETE.Visible = False
                B_EDIT.Visible = False
                B_ADD.Visible = False
            Else
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                B_ADD.Visible = B_ADD.Visible
            End If

        End If
    End Sub

    'Protected Sub chkLegacy_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLegacy.CheckedChanged
    '    If chkLegacy.Checked = True Then
    '        B_DELETE.Visible = False
    '        B_EDIT.Visible = False
    '        B_VIEW.Visible = True
    '        B_ADD.Visible = False
    '    Else
    '        B_DELETE.Visible = True
    '        B_EDIT.Visible = True
    '        B_VIEW.Visible = False
    '        B_ADD.Visible = True
    '    End If

    'End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx?")
    End Sub


    'Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click
    '    Response.Redirect("User.aspx?u=" & hfUserId.Value & "&r=1")

    'End Sub

    'Private Sub gvUsers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUsers.RowDataBound
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
