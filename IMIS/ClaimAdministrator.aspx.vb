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


Partial Public Class ClaimAdministrator
    Inherits System.Web.UI.Page

#Region "Members"
    Protected ImisGen As New IMIS_Gen
    Private eClaimAdmin As New IMIS_EN.tblClaimAdmin
    Private BIClaimAdmin As New IMIS_BI.ClaimAdministratorBI
#End Region
#Region "Events"
#Region "Page"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()
        lblmsg.Text = ""

        If HttpContext.Current.Request.QueryString("a") IsNot Nothing Then
            eClaimAdmin.ClaimAdminUUID = Guid.Parse(HttpContext.Current.Request.QueryString("a"))
            eClaimAdmin.ClaimAdminId = BIClaimAdmin.GetClaimAdminIdByUUID(eClaimAdmin.ClaimAdminUUID)
        End If

        If IsPostBack = True Then
            If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                DeleteLogin()
            End If
            Return
        End If
        Try
            FillHFCodes()
            FillLanguage()
            If Not eClaimAdmin.ClaimAdminId = 0 Then
                BIClaimAdmin.LoadClaimAdmin(eClaimAdmin)

                If eClaimAdmin.ClaimAdminCode IsNot Nothing Then
                    txtCode.Text = eClaimAdmin.ClaimAdminCode
                End If
                If eClaimAdmin.LastName IsNot Nothing Then
                    txtLastName.Text = eClaimAdmin.LastName
                End If
                If eClaimAdmin.OtherNames IsNot Nothing Then
                    txtOtherNames.Text = eClaimAdmin.OtherNames
                End If
                If eClaimAdmin.DOB IsNot Nothing Then
                    txtDOB.Text = If(eClaimAdmin.DOB Is Nothing, "", eClaimAdmin.DOB)
                End If
                If eClaimAdmin.Phone IsNot Nothing Then
                    txtPhone.Text = If(eClaimAdmin.Phone Is Nothing, "", eClaimAdmin.Phone)
                End If
                If eClaimAdmin.tblHF IsNot Nothing Then
                    ddlHFCode.SelectedValue = eClaimAdmin.tblHF.HfID
                End If
                If eClaimAdmin.EmailId IsNot Nothing Then
                    txtEmail.Text = eClaimAdmin.EmailId
                End If
                hfUserID.Value = ""
                If eClaimAdmin.HasLogin = True Then
                    eClaimAdmin.eUsers.LoginName = eClaimAdmin.ClaimAdminCode
                    hfUserID.Value = eClaimAdmin.eUsers.UserID
                    BIClaimAdmin.LoadUsers(eClaimAdmin.eUsers)
                    chkIncludeLogin.Checked = True
                    ddlLanguage.SelectedValue = eClaimAdmin.eUsers.LanguageID
                End If
            Else
                hfUserID.Value = ""
            End If
            If eClaimAdmin.ValidityTo.HasValue Then
                pnlDetails.Enabled = False
                B_SAVE.Visible = False
            End If
        Catch ex As Exception
            ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlDetails, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & ImisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
#End Region
#Region "Buttons"
    Protected Sub B_SAVE_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_SAVE.Click
        If CType(Me.Master.FindControl("hfDirty"), HiddenField).Value = True Then
            Try
                If SetEntity() = False Then Return
                If SaveClaimAdmin() = False Then Return
            Catch ex As Exception
                ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlDetails, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & ImisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
                Return
            End Try
        End If
        Response.Redirect("FindClaimAdministrator.aspx?a=" & txtCode.Text.Trim)
    End Sub

    Protected Sub chkIncludeLogin_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkIncludeLogin.Checked Then
            hfUserID.Value = ""
            changeEnabledValidatorFieldsInIncludeLogin(True)
        Else
            changeEnabledValidatorFieldsInIncludeLogin(False)
        End If
    End Sub

    Private Sub changeEnabledValidatorFieldsInIncludeLogin(ByVal enabled As Boolean)
        RequiredFieldLanguage.Enabled = enabled
        RequiredFieldPassword.Enabled = enabled
        RequiredFieldConfirmPassword.Enabled = enabled
        ComparePassword.Enabled = enabled
    End Sub

    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("FindClaimAdministrator.aspx?a=" & txtCode.Text.Trim)
    End Sub
#End Region
#End Region
#Region "Functions & Procedures"
    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False)
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = ImisGen.getUserId(Session("User"))

        If BIClaimAdmin.RunPageSecurity(IMIS_EN.Enums.Pages.ClaimAdministrator, Page) Then
            Dim Add As Boolean = BIClaimAdmin.checkRights(IMIS_EN.Enums.Rights.AddClaimAdministrator, UserID)
            Dim Edit As Boolean = BIClaimAdmin.checkRights(IMIS_EN.Enums.Rights.EditClaimAdministrator, UserID)

            If Not Add And Not Edit Then
                B_SAVE.Visible = False
                pnlDetails.Enabled = False
            End If
        Else
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.ClaimAdministrator.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub FillHFCodes()
        Dim UserID As Integer = ImisGen.getUserId(Session("User"))
        ddlHFCode.DataSource = BIClaimAdmin.GetHFCodes(UserID, 0)
        ddlHFCode.DataValueField = "HfID"
        ddlHFCode.DataTextField = "HFCODE"
        ddlHFCode.DataBind()
    End Sub
    Private Sub FillLanguage()
        ddlLanguage.DataSource = BIClaimAdmin.GetLanguage
        ddlLanguage.DataValueField = "LanguageCode"
        ddlLanguage.DataTextField = "LanguageName"
        ddlLanguage.DataBind()
    End Sub
    Private Function SetEntity()
        eClaimAdmin = New IMIS_EN.tblClaimAdmin
        eClaimAdmin.eUsers = New IMIS_EN.tblUsers
        If HttpContext.Current.Request.QueryString("a") IsNot Nothing Then
            eClaimAdmin.ClaimAdminUUID = Guid.Parse(HttpContext.Current.Request.QueryString("a"))
            eClaimAdmin.ClaimAdminId = BIClaimAdmin.GetClaimAdminIdByUUID(eClaimAdmin.ClaimAdminUUID)
        End If
        Dim eHF As New IMIS_EN.tblHF
        If ddlHFCode.SelectedIndex > 0 Then eHF.HfID = ddlHFCode.SelectedValue
        eClaimAdmin.tblHF = eHF
        If txtCode.Text.Trim <> String.Empty Then
            eClaimAdmin.ClaimAdminCode = txtCode.Text.Trim
        End If
        If txtLastName.Text.Trim <> String.Empty Then
            eClaimAdmin.LastName = txtLastName.Text.Trim
        End If
        If txtOtherNames.Text.Trim <> String.Empty Then
            eClaimAdmin.OtherNames = txtOtherNames.Text.Trim
        End If
        If IsDate(txtDOB.Text.Trim) Then
            eClaimAdmin.DOB = Date.Parse(txtDOB.Text.Trim)
        End If
        If txtPhone.Text.Trim <> String.Empty Then
            eClaimAdmin.Phone = txtPhone.Text.Trim
        End If
        eClaimAdmin.AuditUserId = ImisGen.getUserId(Session("User"))

        If txtEmail.Text.Trim <> String.Empty Then
            eClaimAdmin.EmailId = txtEmail.Text.Trim
        End If
        eClaimAdmin.HasLogin = 0
        If chkIncludeLogin.Checked = True Then
            If SetLoginDetails() = False Then
                Return False
            End If
        End If
        Return True
    End Function
    Private Function AdminExists()
        Dim eUser = New IMIS_EN.tblUsers
        eUser.LoginName = txtCode.Text
        Dim dt As DataTable = BIClaimAdmin.CheckIfUserExists(eUser)
        If dt.Rows.Count > 0 Then
            Dim loginName = If(dt.Rows(0)("LoginName") = "", "", dt.Rows(0)("LoginName"))
            If loginName <> "" And loginName = txtCode.Text Then
                ImisGen.Alert(eClaimAdmin.ClaimAdminCode & " " & ImisGen.getMessage("M_OFFICEREXISTS"), pnlButtons, alertPopupTitle:="IMIS")
                Return True
            End If
        End If

        Return False
    End Function
    Private Function SetLoginDetails() As Boolean
        eClaimAdmin.eUsers = New IMIS_EN.tblUsers
        If hfUserID.Value <> "" Then
            eClaimAdmin.eUsers.UserID = hfUserID.Value
        End If
        If eClaimAdmin.eUsers.UserID > 0 Then
            BIClaimAdmin.LoadUsers(eClaimAdmin.eUsers)
            eClaimAdmin.eUsers.LoginName = txtCode.Text
        Else
            eClaimAdmin.eUsers.LoginName = txtCode.Text
            BIClaimAdmin.LoadUsers(eClaimAdmin.eUsers)
        End If
        eClaimAdmin.eUsers.AuditUserID = eClaimAdmin.AuditUserId
        If eClaimAdmin.eUsers.UserID = 0 Then
            If Not General.isValidPassword(txtPassword.Text) Then
                lblmsg.Text = General.getInvalidPasswordMessage()
                Return False
            End If
        End If
        If txtPassword.Text <> txtConfirmPassword.Text Then
            lblmsg.Text = ImisGen.getMessage("V_CONFIRMPASSWORD")
            Return False
        End If
        eClaimAdmin.eUsers.LastName = txtLastName.Text
        eClaimAdmin.eUsers.OtherNames = txtOtherNames.Text
        If txtPassword.Text.Length > 0 Then
            eClaimAdmin.eUsers.DummyPwd = txtPassword.Text
        End If
        eClaimAdmin.eUsers.Phone = txtPhone.Text
        eClaimAdmin.eUsers.EmailId = txtEmail.Text
        eClaimAdmin.eUsers.LanguageID = ddlLanguage.SelectedValue
        eClaimAdmin.eUsers.RoleID = 256
        eClaimAdmin.eUsers.AuditUserID = ImisGen.getUserId(Session("User"))
        If ddlHFCode.SelectedIndex >= 0 Then
            eClaimAdmin.eUsers.HFID = ddlHFCode.SelectedValue
        End If
        eClaimAdmin.eUsers.IsAssociated = True
        eClaimAdmin.HasLogin = 1
        Return True
    End Function
    Private Function SaveClaimAdmin() As Boolean
        Dim chk As Integer = BIClaimAdmin.SaveClaimAdmin(eClaimAdmin)
        If chk = 1 Then
            Session("msg") = eClaimAdmin.ClaimAdminCode & " " & eClaimAdmin.LastName & ImisGen.getMessage("M_Inserted")
        ElseIf chk = 2 Then
            Session("msg") = eClaimAdmin.ClaimAdminCode & " " & eClaimAdmin.LastName & ImisGen.getMessage("M_Updated")
        ElseIf chk = 3 Then
            Dim Msg As String = eClaimAdmin.ClaimAdminCode & " " & ImisGen.getMessage("M_Exists")
            ImisGen.Alert(Msg, pnlButtons, alertPopupTitle:="IMIS")
            Return False
        ElseIf chk = -1 Then
            ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            Return False
        End If
        Return True
    End Function
    Public Sub DeleteLogin()
        Try
            If SetEntity() = False Then
                Exit Sub
            End If
            eClaimAdmin.eUsers = New IMIS_EN.tblUsers
            If hfUserID.Value <> "" Then
                eClaimAdmin.eUsers.UserID = hfUserID.Value
                BIClaimAdmin.LoadUsers(eClaimAdmin.eUsers)
                BIClaimAdmin.DeleteUser(eClaimAdmin.eUsers)
                BIClaimAdmin.SaveClaimAdmin(eClaimAdmin)
            End If
            Session("msg") = eClaimAdmin.ClaimAdminCode & " " & ImisGen.getMessage("M_DELETED")
        Catch ex As Exception
            ImisGen.Alert(ImisGen.getMessage("M_ERRORMESSAGE"), pnlClaimAdmiLogin, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & ImisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
#End Region
End Class
