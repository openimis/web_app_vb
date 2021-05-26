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

Partial Public Class MedicalService
    Inherits System.Web.UI.Page

    Dim eService As New IMIS_EN.tblServices
    Dim Service As New IMIS_BI.MedicalServiceBI
    Private imisGen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI


    

    Protected Sub B_SAVE_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_SAVE.Click
        If CInt(hfMS.Value) = 1 Then
            GoTo lblDirty
        End If
        If CType(Me.Master.FindControl("hfDirty"), HiddenField).Value = True Then
lblDirty:   Try

                Dim dt As New DataTable
                dt = DirectCast(Session("User"), DataTable)

                eService.ServCode = txtCode.Text.Trim
                eService.ServName = txtName.Text.Trim
                'eService.ServType = if(rbPreventive.Checked, "P", "C")
                eService.ServType = GetServType()
                eService.ServLevel = ddServiceLevel.SelectedValue
                eService.ServPrice = txtPrice.Text.Trim
                eService.ServCareType = GetServiceCare()
                eService.ServFrequency = txtFrequency.Text.Trim
                eService.ServPatCat = GetServicePatCat()
                eService.ServCategory = ddlCategory.SelectedValue
                eService.AuditUserID = imisGen.getUserId(Session("User"))

                Dim chk As Integer = Service.SaveMedicalService(eService)

                If chk = 0 Then
                    Session("msg") = eService.ServCode & " " & eService.ServName & imisGen.getMessage("M_Inserted")
                ElseIf chk = 1 Then
                    imisGen.Alert(eService.ServCode & imisGen.getMessage("M_Exists"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                Else
                    Session("msg") = eService.ServCode & " " & eService.ServName & imisGen.getMessage("M_Updated")
                End If
                'If chk = 0 Then
                '    Session("msg") = eService.ServName & imisGen.getMessage("M_Inserted")

                'ElseIf chk = 1 Then
                '    Session("msg") = eService.ServName & imisGen.getMessage("M_Exists")
                '    Return

                'Else
                '    Session("msg") = eService.ServName & imisGen.getMessage("M_Updated")


                'End If

                hfMS.Value = 0
            Catch ex As Exception
                'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
                imisGen.Alert(ex.Message, pnlButtons, alertPopupTitle:="IMIS")
                imisGen.Log(Page.Title & " : " & imisGen.getLoginName(Session("User")), ex)
                'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
                'txtCode.Text = ""
                'txtCode.Focus()
                hfMS.Value = 1
                Return
            End Try
        End If
        Response.Redirect("FindMedicalService.aspx?s=" & txtCode.Text)
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RunPageSecurity()

        Try
            lblMsg.Text = ""

            If HttpContext.Current.Request.QueryString("s") IsNot Nothing Then
                eService.ServiceUUID = Guid.Parse(HttpContext.Current.Request.QueryString("s"))
                eService.ServiceID = Service.GetServiceIdByUUID(eService.ServiceUUID)
            End If

            If IsPostBack = False Then
                Dim Serv As New IMIS_BI.MedicalServiceBI
                ddServiceLevel.DataSource = Serv.GetServiceLevel
                ddServiceLevel.DataValueField = "ServiceID"
                ddServiceLevel.DataTextField = "Service"
                ddServiceLevel.DataBind()

                ddlCategory.DataSource = Serv.GetServiceCategory
                ddlCategory.DataValueField = "CategoryId"
                ddlCategory.DataTextField = "Category"
                ddlCategory.DataBind()

            End If
            If IsPostBack = True Then Return
            hfMS.Value = 2
            If Not eService.ServiceID = 0 Then
                Service.LoadMedicalServices(eService)
                txtCode.Text = eService.ServCode
                txtName.Text = eService.ServName
                ddServiceLevel.SelectedValue = eService.ServLevel
                txtPrice.Text = FormatNumber(eService.ServPrice, 0)
                txtFrequency.Text = eService.ServFrequency
                ddlCategory.SelectedValue = eService.ServCategory

                setServType()
                setServCare()
                setServPatCat()
            Else
                txtFrequency.Text = 0
                chkAdult.Checked = True
                chkChild.Checked = True
                chkMan.Checked = True
                chkWoman.Checked = True
            End If

            If HttpContext.Current.Request.QueryString("r") = 1 Or eService.ValidityTo.HasValue Then
                Panel2.Enabled = False
                B_SAVE.Visible = False
            End If

        Catch ex As Exception
            'lblMsg.Text = ex.Message
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisGen.Log(Page.Title & " : " & imisGen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", imisGen.getUserId(Session("User")) & " : " & ex.Message, EventLogEntryType.Information, 5, 3)
        End Try

    End Sub

    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisGen.getRoleId(Session("User"))
        Dim UserID As Integer = imisGen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.MedicalService, Page) Then
            Dim Add As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.AddMedicalService, UserID)
            Dim Edit As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.EditMedicalService, UserID)

            If Not Add And Not Edit Then
                B_SAVE.Visible = False
                Panel2.Enabled = False
            End If
        Else
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.MedicalService.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub

    Private Sub setServType()
        Dim servType As String = eService.ServType
        If servType = "P" Then
            rbPreventive.Checked = True
        ElseIf servType = "C" Then
            rbCurative.Checked = True
        End If
    End Sub
    Private Function GetServType() As String
        Dim servtypeValue As String = ""
        If rbPreventive.Checked = True Then
            servtypeValue = "P"
        End If

        If rbCurative.Checked = True Then
            servtypeValue = "C"
        End If
        If String.IsNullOrEmpty(servtypeValue) Then
            Throw New Exception("Please select Type")
        End If
        Return servtypeValue
    End Function
    Private Sub setServCare()
        Dim CareValue As String = eService.ServCareType

        If CareValue = "O" Then
            rbOutPatient.Checked = True
        ElseIf CareValue = "I" Then
            rbInPatient.Checked = True
        ElseIf CareValue = "B" Then
            rbBoth.Checked = True


        End If
        'chkDispensary.Checked = (1 And CareValue)
        'chkHealthCentre.Checked = (2 And CareValue)
        'chKHospital.Checked = (4 And CareValue)
    End Sub
    Private Function GetServiceCare() As String
        Dim CareValue As String = ""
        If rbOutPatient.Checked = True Then
            CareValue = "O" 'CareValue + 1
        End If

        If rbInPatient.Checked Then
            CareValue = "I" 'CareValue + 2
        End If

        If rbBoth.Checked Then
            CareValue = "B" 'CareValue + 4
        End If
        If String.IsNullOrEmpty(CareValue) = True Then
            Throw New Exception("Please select atleast one care type")
        End If

        Return CareValue

    End Function
    Private Sub setServPatCat()
        Dim PatCat As Integer = eService.ServPatCat
        chkMan.Checked = (1 And PatCat)
        chkWoman.Checked = (2 And PatCat)
        chkAdult.Checked = (4 And PatCat)
        chkChild.Checked = (8 And PatCat)




    End Sub
    Private Function GetServicePatCat() As Integer
        Dim PatCat As Integer = 0

        If chkMan.Checked Then
            PatCat = PatCat + 1
        End If

        If chkWoman.Checked Then
            PatCat = PatCat + 2
        End If

        If chkAdult.Checked Then
            PatCat = PatCat + 4
        End If

        If chkChild.Checked Then
            PatCat = PatCat + 8
        End If
        If PatCat = 0 Then
            Throw New Exception("Please select atleast one patient")
        End If
        Return PatCat

    End Function
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("FindMedicalService.aspx?s=" & txtCode.Text.Trim)
    End Sub
End Class
