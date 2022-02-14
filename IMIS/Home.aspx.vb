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

Imports System.IO
Imports System.Net
Imports System.Data
Imports System.Reflection

Partial Public Class Home
    Inherits System.Web.UI.Page
    Private Home As New IMIS_BI.HomeBI
    Private eUsers As New IMIS_EN.tblUsers
    Protected imisgen As New IMIS_Gen
    Protected escapeBL As New IMIS_BL.EscapeBL


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Dim version As Version
            version = Assembly.GetExecutingAssembly().GetName().Version

            compiledVersion.Text = version.ToString()

            eUsers.UserID = imisgen.getUserId(Session("User"))
            If IsPostBack = True Then Return
            Dim load As New IMIS_BI.UserBI

            gvRegions.DataSource = Home.GetUserRegions(eUsers.UserID)
            gvRegions.DataBind()
            gvDistrict.DataSource = Home.getUsersDistricts(eUsers.UserID)
            gvDistrict.DataBind()



            Dim configDict As Dictionary(Of String, Object) = buildConfigDict()
            txtCONFIGISSUE.Text = escapeBL.CheckConfiguration(configDict).Replace(Environment.NewLine, "<br />")
            If Not txtCONFIGISSUE.Text.Equals("") Then
                ConfigContent.Visible = True
            End If

            If Not eUsers.UserID = 0 Then
                Home.LoadUsers(eUsers)
                txtCURRENTUSER.Text = eUsers.OtherNames & " " & eUsers.LastName & " (" & eUsers.LoginName & ")"
                'txtCURRENTUSER.Text = eUsers.OtherNames & " " & eUsers.LastName
            End If

            'Dim currentUserRoles = Home.GetRoles(eUsers.UserID)
            'Dim currRole = getUserRoles(eUsers.UserID)


            gvRoles.DataSource = Home.GetRoles(eUsers.UserID)
            Dim security_warning_message = DefaultPasswordWarning(gvRoles.DataSource)

            gvRoles.DataBind()
            '  Assign(gvRoles)
        Catch ex As Exception
            Session("Msg") = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.ToString(), EventLogEntryType.Error, 999)

        End Try



    End Sub

    Private Function buildConfigDict()
        Dim configDict As New Dictionary(Of String, Object)
        configDict.Add("eUser", eUsers)
        Return configDict
    End Function

    Public Function DefaultPasswordWarning(ByRef currentUserRoles)
        ' This functions checks whether admin changed default secret keys from config files. (ex. Web.debug.config)
        ' If not a security alert is raised when main page is opened.

        ' Check if user is admin
        Dim userRolesList = imisgen.DataTableToList(currentUserRoles)
        Dim isAdmin = False
        For Each role In userRolesList
            If role.Item("Code") = "64" Then
                isAdmin = True
            End If
        Next
        If isAdmin = False Then
            Return False
        End If

        Dim passwordSQL As String = System.Configuration.ConfigurationManager.AppSettings("Offline:SQLite:Password").ToString()
        Dim passwordEncryption As String = System.Configuration.ConfigurationManager.AppSettings("Offline:Encryption:Password").ToString()
        Dim passwordRAR As String = System.Configuration.ConfigurationManager.AppSettings("Offline:RAR:Password").ToString()
        Dim passwordWarningsList As New List(Of String)

        If passwordSQL = "%^Klp)*3" Then
            passwordWarningsList.Add("Offline:SQLite:Password")
        End If
        If passwordEncryption = ":-+A7V@=" Then
            passwordWarningsList.Add("Offline:Encryption:Password")
        End If
        If passwordRAR = ")(#$1HsD" Then
            passwordWarningsList.Add("Offline:RAR:Password")
        End If

        If passwordWarningsList.Count() = 0 Then
            Return False
        Else
            Dim alert_message = imisgen.getMessage("M_DEFAULTPASSWORDWARNING") + "</br>" + String.Join("</br>", passwordWarningsList)
            imisgen.Alert(alert_message, DEFAULTPASSWORDWARNINGFIELD, alertPopupTitle:=imisgen.getMessage("M_DEFAULTPASSWORDWARNINGLABEL"))
            Return True
        End If
    End Function


End Class
