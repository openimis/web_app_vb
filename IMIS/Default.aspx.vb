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

Partial Public Class Login
    Inherits System.Web.UI.Page
    Private Login As New IMIS_BI.LoginBI
    Private eLogin As New IMIS_EN.tblUsers
    Protected imisgen As New IMIS_Gen
    Private dt As New DataTable


    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Dim OK As Boolean
        Try

            'Dim dt As New DataTable

            eLogin.LoginName = txtUserName.Text
            eLogin.DummyPwd = txtPassword.Text

            dt = Login.GetLogin(eLogin)

            If dt.Rows.Count > 0 Then

                'Get component settings
                Dim dtControls As DataTable = Login.getControlsSettings

                Session("Controls") = dtControls

                Dim eDefaults As New IMIS_EN.tblIMISDefaults
                Login.GetDefaults(eDefaults)

                Dim cookie As HttpCookie
                cookie = Request.Cookies.Get("CultureInfo")
                If cookie Is Nothing Then
                    cookie = New HttpCookie("CultureInfo")
                End If
              

                cookie.Value = dt.Rows(0)("LanguageID").ToString

                Response.Cookies.Add(cookie)
                Session("User") = dt
                IMIS_Gen.offlineHF = False
                IMIS_Gen.OfflineCHF = False
                'check chfid for offline
                'If offlineHF > 0 Then

                Select Case dt.Rows(0)("RoleID")
                    Case 524288
                        IMIS_Gen.HFID = eDefaults.OffLineHF
                        IMIS_Gen.offlineHF = True
                        If IMIS_Gen.HFID = 0 Then
                            hfOfflineHFIDFlag.Value = 1
                            Return
                        End If
                    Case 1048576
                        IMIS_Gen.OfflineCHF = True
                        IMIS_Gen.CHFID = eDefaults.OfflineCHF
                        If IMIS_Gen.CHFID = 0 Then
                            hfOfflineHFIDFlag.Value = 1
                            Return
                        End If
                End Select

                If eDefaults.OffLineHF > 0 Then
                    IMIS_Gen.offlineHF = True
                    IMIS_Gen.OfflineCHF = False
                ElseIf eDefaults.OfflineCHF > 0 Then
                    IMIS_Gen.offlineHF = False
                    IMIS_Gen.OfflineCHF = True
                Else
                    IMIS_Gen.offlineHF = False
                    IMIS_Gen.OfflineCHF = False
                End If

                'End If

                If IMIS_Gen.offlineHF Then
                    Dim eHF As New IMIS_EN.tblHF
                    eHF.HfID = eDefaults.OffLineHF
                    Login.getHFCodeAndName(eHF)
                    IMIS_Gen.HFCode = eHF.HFCode
                    IMIS_Gen.HFName = eHF.HFName
                End If

                'check chfid for offline
                OK = True

            Else
                lblMessage.Text = imisgen.getMessage("M_PASSWOERDMATCH")
                EventLog.WriteEntry("IMIS", eLogin.LoginName & " failed to login. Due to Username/Password mismatch", EventLogEntryType.Information, 1)
                Return
            End If


        Catch ex As Exception
            If OK = False Then
                lblMessage.Text = imisgen.getMessage("M_PASSWORDERROR")
                EventLog.WriteEntry("IMIS", eLogin.LoginName & " : " & ex.Message & "\n" & ex.StackTrace, EventLogEntryType.Error, 999)
                Return
            End If
        End Try
        Dim UserID As Integer = dt.Rows(0)("UserID")
        Login.InsertLogTime(UserID, 1)
        Session("LogInFlag") = True

        EventLog.WriteEntry("IMIS", eLogin.LoginName & " has logged in.", EventLogEntryType.Information, 1)
        Response.Redirect("Home.aspx")
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtUserName.Focus()
        If Request.Form("__EVENTARGUMENT") = "SaveOfflineHFID" Then
            Dim eDefaults As New IMIS_EN.tblIMISDefaults
            Dim txtOfflineHF As String = Request.Form("txtOfflineHF")
            If String.IsNullOrEmpty(txtOfflineHF) Or Not (IsNumeric(txtOfflineHF)) Then
                hfOfflineHFIDFlag.Value = 1
                Return
            End If

            If txtOfflineHF = 0 Then
                hfOfflineHFIDFlag.Value = 1
                Return
            End If

            If IMIS_Gen.offlineHF = True Then
                eDefaults.OffLineHF = CInt(txtOfflineHF)
                IMIS_Gen.HFID = eDefaults.OffLineHF
            ElseIf IMIS_Gen.OfflineCHF = True Then
                eDefaults.OfflineCHF = CInt(txtOfflineHF)
                IMIS_Gen.CHFID = eDefaults.OfflineCHF
            End If

            hfOfflineHFIDFlag.Value = 0

            Login.SaveOfflineHFID(eDefaults)

            Response.Redirect("Home.aspx")
        End If
    End Sub
End Class
