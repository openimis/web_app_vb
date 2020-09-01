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
Partial Public Class User
    Inherits System.Web.UI.Page
    Private Users As New IMIS_BI.UserBI
    Private eUsers As New IMIS_EN.tblUsers
    Private imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()

        Try
            lblMsg.Text = ""

            If HttpContext.Current.Request.QueryString("u") IsNot Nothing Then
                eUsers.UserUUID = Guid.Parse(HttpContext.Current.Request.QueryString("u"))
                eUsers.UserID = Users.GetUserIdByUUID(eUsers.UserUUID)
            End If

            If HttpContext.Current.Request.QueryString("r") = 1 Then
                Panel2.Enabled = False
                B_SAVE.Visible = False
            End If

            If IsPostBack = True Then Return
            Dim load As New IMIS_BI.UserBI
            ddlLanguage.DataSource = Users.GetLanguage
            ddlLanguage.DataValueField = "LanguageCode"
            ddlLanguage.DataTextField = "LanguageName"
            ddlLanguage.DataBind()
            ddlHFNAME.DataSource = Users.GetHFCodes(imisgen.getUserId(Session("User")), 0)
            ddlHFNAME.DataValueField = "Hfid"
            ddlHFNAME.DataTextField = "HFCode"
            ddlHFNAME.DataBind()

            Dim dtRegion As DataTable = Users.getRegions(eUsers.UserID, imisgen.getUserId(Session("User")))
            gvRegion.DataSource = dtRegion
            gvRegion.DataBind()

            Assign(gvRegion)

            If IMIS_Gen.offlineHF Then
                gvDistrict.DataSource = Users.GetDistrictForHF(IMIS_Gen.HFID, eUsers.UserID)
            Else
                gvDistrict.DataSource = Users.GetDistricts(eUsers.UserID, imisgen.getUserId(Session("User")))
            End If
            gvDistrict.DataBind()
            Assign(gvDistrict)
            Dim LoggedInUser As Integer = imisgen.getUserId(Session("User"))
            If Not eUsers.UserID = 0 Then

                Users.LoadUsers(eUsers)

                ddlLanguage.SelectedValue = eUsers.LanguageID
                txtLastName.Text = eUsers.LastName
                txtOtherNames.Text = eUsers.OtherNames
                txtPhone.Text = eUsers.Phone
                txtEmail.Text = eUsers.EmailId
                txtLoginName.Text = eUsers.LoginName
                ' txtPassword.Attributes.Add("value", eUsers.DummyPwd)
                ' txtConfirmPassword.Attributes.Add("value", eUsers.DummyPwd)
                If HttpContext.Current.Request.QueryString("r") = 1 Or eUsers.ValidityTo.HasValue Then
                    Panel2.Enabled = False
                    B_SAVE.Visible = False
                End If

                Dim CurrentUserID As Integer = imisgen.getUserId(Session("User"))
                Dim result = Users.GetUserDistricts(LoggedInUser, eUsers.UserID)
                If result = 1 Then
                    imisgen.Alert(imisgen.getMessage("M_USERCANNOTBEEDITED"), pnlDistrict, alertPopupTitle:="IMIS")
                    Panel2.Enabled = False
                    B_SAVE.Visible = False
                End If

                ddlHFNAME.SelectedValue = eUsers.HFID.ToString
                RequiredFieldPassword.Visible = False

                RequiredFieldConfirmPassword.Visible = False
            End If 'Added

            Dim RoleId As Integer = imisgen.getRoleId(Session("User"))
            Dim dtRoles As New DataTable
            dtRoles = Users.getRolesForUser(eUsers.UserID, IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF, LoggedInUser)
            gvRoles.DataSource = dtRoles
            gvRoles.DataBind()
            If eUsers.IsAssociated IsNot Nothing AndAlso eUsers.IsAssociated = True Then
                toggleModifingIfUsersClaimOrEnrolment(False)
                B_SAVE.Enabled = False
            End If

            If IMIS_Gen.offlineHF Then
                ddlHFNAME.SelectedValue = IMIS_Gen.HFID
                ddlHFNAME.Enabled = False
            End If
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlDistrict, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub toggleModifingIfUsersClaimOrEnrolment(enabled As Boolean)
        txtOtherNames.Enabled = enabled
        txtLastName.Enabled = enabled
        txtEmail.Enabled = enabled
        txtLoginName.Enabled = enabled
        ddlHFNAME.Enabled = enabled
        pnlRole.Enabled = enabled
        Checkbox1.Enabled = enabled
        chkCheckAllR.Enabled = enabled
        pnlRegion.Enabled = enabled
        CheckBox2.Enabled = enabled
        pnlDistrict.Enabled = enabled
        txtPhone.Enabled = enabled
    End Sub

    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.User, Page) Then
            Dim Add As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.UsersAdd, UserID)
            Dim Edit As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.UsersEdit, UserID)

            If Not Add And Not Edit Then
                B_SAVE.Visible = False
                Panel2.Enabled = False
            End If
        Else
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.User.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub


    Public Sub Assign(ByVal grid As GridView)
        Dim _checkedRole As Boolean = True
        Dim _checkedDistrict As Boolean = True
        Dim _checkedRegion As Boolean = True

        For Each row In grid.Rows
            Dim chkSelect As CheckBox = CType(row.Cells(0).Controls(1), CheckBox)
            If grid.ID = gvRoles.ID Then
                chkSelect.Checked = (eUsers.RoleID And CInt(grid.DataKeys(row.RowIndex)("Code")))
                If chkSelect.Checked <> True Then
                    _checkedRole = False
                End If
            ElseIf grid.ID = gvDistrict.ID Then
                chkSelect.Checked = gvDistrict.DataKeys(row.RowIndex).Value
                If chkSelect.Checked <> True Then
                    _checkedDistrict = False
                End If
            ElseIf grid.ID = gvRegion.ID Then
                chkSelect.Checked = gvRegion.DataKeys(row.RowIndex).Value
                If chkSelect.Checked <> True Then
                    _checkedRegion = False
                End If
            End If
        Next

        If grid.ID = gvRoles.ID Then
            Checkbox1.Checked = _checkedRole
        ElseIf grid.ID = gvDistrict.ID Then
            CheckBox2.Checked = _checkedDistrict
        ElseIf grid.ID = gvRegion.ID Then
            chkCheckAllR.Checked = _checkedRegion
        End If

    End Sub
    Public Function CheckDifferenceandSave(ByVal grid As GridView, ByVal RowIndex As Integer) As Boolean

        Dim chkSelect As CheckBox = CType(grid.Rows(RowIndex).Cells(0).Controls(1), CheckBox)
        If chkSelect.Checked <> CBool(grid.DataKeys(grid.Rows(RowIndex).RowIndex).Value) Then
            Return True
        Else
            Return False
        End If


    End Function

    Private Function checkChecked(ByVal gv As GridView) As Boolean
        Dim checked As Boolean = False
        If gv.ID = gvRoles.ID Then
            If txtLoginName.Text = "Admin" Then Return True
        End If
        For Each row In gv.Rows
            Dim chkSelect As CheckBox = CType(row.Cells(0).Controls(1), CheckBox)
            If chkSelect.Checked Then
                checked = True
                Exit For
            End If
        Next
        Return checked
    End Function

    Private Sub B_SAVE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_SAVE.Click
        If txtPassword.Text <> String.Empty Then
            If Not General.isValidPassword(txtPassword.Text) Then
                lblMsg.Text = General.getInvalidPasswordMessage()
                Exit Sub
            End If
            If txtPassword.Text <> txtConfirmPassword.Text Then
                lblMsg.Text = imisgen.getMessage("V_CONFIRMPASSWORD")
                Exit Sub
            End If
            eUsers.DummyPwd = txtPassword.Text
        End If

        If CType(Me.Master.FindControl("hfDirty"), HiddenField).Value = True Then
            Try
                If Not checkChecked(gvDistrict) Then
                    lblMsg.Text = imisgen.getMessage("V_SELECTDISTRICT")
                    Return
                End If
                If Not checkChecked(gvRoles) Then
                    lblMsg.Text = imisgen.getMessage("V_SELECTROLE")
                    Return
                End If
                eUsers.LastName = txtLastName.Text
                eUsers.OtherNames = txtOtherNames.Text
                eUsers.DummyPwd = txtPassword.Text
                eUsers.Phone = txtPhone.Text
                eUsers.EmailId = txtEmail.Text
                eUsers.LoginName = txtLoginName.Text
                eUsers.LanguageID = ddlLanguage.SelectedValue
                'eUsers.RoleID = GetRoles(gvRoles)
                eUsers.AuditUserID = imisgen.getUserId(Session("User"))
                If ddlHFNAME.SelectedIndex >= 0 Then
                    eUsers.HFID = ddlHFNAME.SelectedValue
                End If

                Dim dt As New DataTable
                dt.Columns.Add("UserRoleID", GetType(Integer))
                dt.Columns.Add("UserID", GetType(Integer))
                dt.Columns.Add("RoleID", GetType(Integer))
                dt.Columns.Add("ValidityFrom", GetType(Date))
                dt.Columns.Add("ValidityTo", GetType(Date))
                dt.Columns.Add("AuditUserID", GetType(Integer))
                dt.Columns.Add("LegacyID", GetType(Integer))
                dt.Columns.Add("Assign", GetType(Integer))




                Dim dr As DataRow
                Dim UserRoleID As New Object
                For Each row As GridViewRow In gvRoles.Rows
                    dr = dt.NewRow
                    UserRoleID = CType(row.Cells(4).Controls(1), HiddenField).Value

                    If UserRoleID = "" Then UserRoleID = 0
                    dr("UserID") = eUsers.UserID
                    dr("UserRoleID") = UserRoleID
                    dr("Assign") = 0
                    If CType(row.Cells(0).Controls(1), CheckBox).Checked = True Then
                        dr("RoleID") = gvRoles.DataKeys(row.RowIndex).Value
                        dr("Assign") = 1
                    End If
                    If CType(row.Cells(2).Controls(1), CheckBox).Checked = True Then
                        dr("RoleID") = gvRoles.DataKeys(row.RowIndex).Value
                        dr("Assign") = dr("Assign") + 2
                    End If
                    If dr("RoleID") IsNot DBNull.Value Then
                        dt.Rows.Add(dr)
                    End If
                Next
                Dim chk As Integer = Users.SaveUser(eUsers, dt)
                If Not chk = 1 Then
                    For Each row In gvDistrict.Rows
                        If CheckDifferenceandSave(gvDistrict, row.RowIndex) = True Then
                            Dim eUsersDistricts As New IMIS_EN.tblUsersDistricts
                            Dim eLocations As New IMIS_EN.tblLocations
                            eLocations.LocationId = gvDistrict.DataKeys(row.RowIndex)("DistrictId")
                            eUsersDistricts.tblUsers = eUsers
                            eUsersDistricts.UserDistrictID = If(gvDistrict.DataKeys(row.RowIndex)("UserDistrictId") Is System.DBNull.Value, 0, gvDistrict.DataKeys(row.RowIndex)("UserDistrictId"))
                            eUsersDistricts.AuditUserID = imisgen.getUserId(Session("User"))
                            eUsersDistricts.tblLocations = eLocations
                            Users.SaveUserDistricts(eUsersDistricts)
                        End If
                    Next
                End If

                If chk = 0 Then
                    Session("msg") = eUsers.LoginName & imisgen.getMessage("M_Inserted")
                ElseIf chk = 1 Then
                    imisgen.Alert(eUsers.LoginName & imisgen.getMessage("M_Exists"), pnlButtons, alertPopupTitle:="IMIS")
                    txtLoginName.Text = ""
                    Return
                Else
                    Session("msg") = eUsers.LoginName & imisgen.getMessage("M_Updated")
                End If
            Catch ex As Exception
                'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlDistrict, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
                Return
            End Try
        End If


        Response.Redirect("FindUser.aspx?u=" & txtLoginName.Text)
    End Sub

    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("FindUser.aspx?u=" & txtLoginName.Text)
    End Sub
End Class
