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

Public Class ChangePassword
    Inherits System.Web.UI.Page
    Private ChangePasswordBI As New IMIS_BI.ChangePasswordBI
    Private eUsers As New IMIS_EN.tblUsers
    Private imisgen As New IMIS_Gen
    Private Sub B_SAVE_Click(sender As Object, e As EventArgs) Handles B_SAVE.Click
        Try

            If Not General.isValidPassword(txtNewPassword.Text) Then
                lblMsg.Text = General.getInvalidPasswordMessage()
                Exit Sub
            End If
            If txtNewPassword.Text <> txtConfirmNewPassword.Text Then
                lblMsg.Text = imisgen.getMessage("V_CONFIRMPASSWORD")
                Exit Sub
            End If
            eUsers.UserID = imisgen.getUserId(Session("User"))

            eUsers.DummyPwd = txtCurrentPassword.Text.ToString
            eUsers.AuditUserID = eUsers.UserID
            If ChangePasswordBI.ChangePassword(eUsers, txtNewPassword.Text) = 1 Then
                lblMsg.Text = imisgen.getMessage("L_PASSWORD") & imisgen.getMessage("M_Updated")
            Else
                lblMsg.Text = imisgen.getMessage("M_INCORRECTCURRENTPASSWORD")
            End If


        Catch ex As Exception
            lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Public Function IsValidyCurrentPassword() As Boolean
        eUsers.UserID = imisgen.getUserId(Session("User"))
        ChangePasswordBI.LoadUsers(eUsers)
        If eUsers.DummyPwd = txtCurrentPassword.Text.ToString Then Return True
        Return False
    End Function
    Private Sub B_CANCEL_Click(sender As Object, e As EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub
End Class
