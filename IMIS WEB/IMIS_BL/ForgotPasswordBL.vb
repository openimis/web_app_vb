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

Public Class ForgotPasswordBL
    Public Function SendPassword(LoginName As String, Password As String) As Integer
        Dim EmailHandler As New IMIS_BL.EmailHandler
        Try
            'Check if given email exists in database
            Dim Users As New IMIS_BL.UsersBL
            Dim eUsers As New IMIS_EN.tblUsers
            eUsers.LoginName = LoginName
            Users.LoadUsers(eUsers)
            'Check if the Login name exists
            If eUsers.UserID = Nothing Then
                Return -1
            End If

            'Send email
            Dim ValidityDate As Date = Now.AddHours(24)
            eUsers.PasswordValidity = ValidityDate
            eUsers.AuditUserID = eUsers.UserID
            eUsers.DummyPwd = Password
            Dim EmailHash As String = Users.CreateEmailHash(eUsers)
            Dim Template As String = HttpContext.Current.Server.MapPath("\") & "Templates\ForgotPassword.html"
            Dim StrPathAndQuery As String = HttpContext.Current.Request.Url.PathAndQuery
            Dim NewPasswordPage As String = HttpContext.Current.Request.Url.AbsoluteUri.Replace(StrPathAndQuery, "/") & "EnterNewPassword.aspx?h=" & EmailHash
            'HttpContext.Current.Server.MapPath("\") & "EnterNewPassword.aspx?h=" & EmailHash

            Dim Dict As New Dictionary(Of String, String)
            Dict.Add("@@Name", eUsers.OtherNames & " " & eUsers.LastName)
            Dict.Add("@@NewLink", NewPasswordPage)
            Dict.Add("@@ValidityDate", ValidityDate)
            Dim DAL As New IMIS_DAL.UsersDAL
            DAL.UpdatePasswordValidity(eUsers)
            Dim emailed As Boolean = EmailHandler.sendEmail(Template, eUsers.EmailId, "IMIS Password Request", Dict, Nothing, "", "", "")



        Catch ex As Exception
            Throw ex
        End Try
        Return 1
    End Function
    Public Function UpdatePassword(LoginName As String, Password As String, EmailHash As String) As Integer
        Dim EmailHandler As New IMIS_BL.EmailHandler
        Try
            'Check if given email exists in database
            Dim Users As New IMIS_BL.UsersBL
            Dim eUsers As New IMIS_EN.tblUsers
            eUsers.LoginName = LoginName
            Users.LoadUsers(eUsers)
            eUsers.DummyPwd = Password
            eUsers.AuditUserID = eUsers.UserID
            'Check if the email exists
            If eUsers.UserID = Nothing Then
                Return -1
            End If
            'Check if request is still valid
            If eUsers.PasswordValidity IsNot Nothing AndAlso Date.Now > eUsers.PasswordValidity Then
                Return -2
            End If
            'Check if password can be validated with the request
            If Users.ValidateEmailHash(eUsers, EmailHash) = False Then
                Return -3
            End If
            Users.CreatePassword(eUsers)

            Dim DAL As New IMIS_DAL.UsersDAL
            DAL.ChangePassword(eUsers)

        Catch ex As Exception
            Throw ex
        End Try
        Return 1
    End Function
End Class
