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

Public Partial Class Utilities
    Inherits System.Web.UI.Page
    Private Utility As New IMIS_BI.UtilitiesBI
    Private imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Return
        RunPageSecurity()

        Dim dt As New DataTable
        dt = Utility.getDatabasePath

        txtPath.Text = dt(0)("DatabaseBackupFolder")
        ltlBackendVersion.Text = dt(0)("AppVersionBackEnd")

    End Sub

    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If Not userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Utilities, Page) Then
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Utilities.ToString & "&retUrl=" & RefUrl)
        End If
        'Added by Emmanuel, the security level upon rights
        btnBackup.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.DatabaseBackup, UserID)
        txtPath.Enabled = btnBackup.Enabled
        chkSavePath.Enabled = btnBackup.Enabled
        btnRestore.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.DatabaseRestore, UserID)
        txtRestore.Enabled = btnRestore.Enabled
        btnExecute.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.ExecuteScripts, UserID)
        FileUpload1.Enabled = btnExecute.Enabled
    End Sub

    Private Sub btnBackup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackup.Click
        If Len(Trim(txtPath.Text)) = 0 Then Return

        If Not My.Computer.FileSystem.DirectoryExists(txtPath.Text.Trim) Then
            imisgen.Alert(imisgen.getMessage("M_INVALIDBACKPATH"), pnlButtons, alertPopupTitle:="IMIS")

            Return
        End If

        Utility.CreateDatabaseBackup(txtPath.Text.Trim, chkSavePath.Checked)
    End Sub

    Protected Sub btnRestore_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRestore.Click
        If Len(Trim(txtRestore.Text.Trim)) = 0 Then Return
        'Restore database
        Utility.RestoreDatabases(txtRestore.Text.Trim)

        'Run the SETUP_IMIS Stored Procedure
        'Utility.SetupIMIS()

        'Log out the user 
        Response.Redirect("Logout.aspx")

    End Sub

    Protected Sub btnExecute_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExecute.Click



        'If Not FileUpload1.HasFile Then Return
        'FileUpload1.SaveAs(Server.MapPath("/Workspace/" & FileUpload1.FileName & ""))
        'Dim Result As String = ""
        'Result = Utility.Execute(Server.MapPath("/Workspace/" & FileUpload1.FileName & ""))


        'Dim Popup As String = "<script type=""text/javascript"">$(document).ready(function(){popup.alert('" & Result & "')});</script>"
        'Dim ltl As New Literal
        'ltl.Text = Popup

        'Panel1.Controls.Add(ltl)


    End Sub

    Protected Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_CANCEL.Click
         Response.Redirect("Home.aspx")
    End Sub
End Class
