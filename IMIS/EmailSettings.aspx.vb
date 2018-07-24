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

Public Class EmailSettings
    Inherits System.Web.UI.Page

    Private SettingsBI As New IMIS_BI.EmailSettingsBI
    Private imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private eEmailSettings As New IMIS_EN.tblEmailSettings

    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim RefUrl = Request.Headers("Referer")
        If Not userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Payer, Page) Then
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.EmailSettings.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub LoadEmailSettings()
        Dim dt As DataTable = SettingsBI.getEmailSettings
        If dt.Rows.Count > 0 Then
            txtEmail.Text = dt(0)("EmailId")
            txtPassword.Attributes.Add("value", dt(0)("EmailPassword"))
            txtSMTPHost.Text = dt(0)("SMTPHost")
            txtPort.Text = dt(0)("Port")
            chkEnableSSL.Checked = dt(0)("EnableSSL")
        End If
    End Sub
    Private Sub SetEntity()
        eEmailSettings = New IMIS_EN.tblEmailSettings
        With eEmailSettings
            .EmailId = txtEmail.Text
            .EmailPassword = txtPassword.Text
            .SMTPHost = txtSMTPHost.Text
            .EnableSSL = chkEnableSSL.Checked
            If txtPort.Text.Trim.Length > 0 Then .Port = Val(txtPort.Text)
        End With
    End Sub
    Private Sub Save()
        SetEntity()
        SettingsBI.SaveEmailSettings(eEmailSettings)

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            RunPageSecurity()
        If IsPostBack = False Then
            LoadEmailSettings()
        End If

    End Sub

    Private Sub B_SAVE_Click(sender As Object, e As EventArgs) Handles B_SAVE.Click
        Save()
        Session("msg") = imisgen.getMessage("M_EMAILUPDATED")
        Response.Redirect("Home.aspx")
    End Sub

    Private Sub B_CANCEL_Click(sender As Object, e As EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub
End Class
