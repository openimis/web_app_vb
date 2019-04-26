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

Partial Public Class MedicalItem
    Inherits System.Web.UI.Page

    Dim eItem As New IMIS_EN.tblItems
    Dim Item As New IMIS_BI.MedicalItemBI
    Private imisGen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisGen.getRoleId(Session("User"))
        Dim UserID As Integer = imisGen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.MedicalItem, Page) Then
            Dim Add As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.AddMedicalItem, UserID)
            Dim Edit As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.EditMedicalItem, UserID)

            If Not Add And Not Edit Then
                B_SAVE.Visible = False
                Panel2.Enabled = False
            End If
        Else
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.MedicalItem.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RunPageSecurity()
        Try
            lblMsg.Text = ""
            eItem.ItemID = HttpContext.Current.Request.QueryString("i")


            If IsPostBack = True Then Return
            hfMI.Value = 2
            If Not eItem.ItemID = 0 Then
                Item.LoadItem(eItem)
                txtCode.Text = eItem.ItemCode
                txtName.Text = eItem.ItemName
                txtPackage.Text = eItem.ItemPackage
                txtPrice.Text = FormatNumber(eItem.ItemPrice, 0)
                txtFrequency.Text = eItem.ItemFrequency
                setItemCare()
                setItemPatCat()
                setItemType()
            Else
                txtFrequency.Text = 0
                chkAdult.Checked = True
                chkChild.Checked = True
                chkMan.Checked = True
                chkWoman.Checked = True
            End If

            If HttpContext.Current.Request.QueryString("r") = 1 Or eItem.ValidityTo.HasValue Then
                Panel2.Enabled = False
                B_SAVE.Visible = False
            End If

        Catch ex As Exception
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try


    End Sub
    Protected Sub B_SAVE_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_SAVE.Click
        If CInt(hfMI.Value) = 1 Then
            GoTo lblDirty
        End If
        If CType(Me.Master.FindControl("hfDirty"), HiddenField).Value = True Then
lblDirty:   Dim chk As Integer = 0
            Try
                Dim dt As New DataTable
                dt = DirectCast(Session("User"), DataTable)
                eItem.ItemCode = txtCode.Text
                eItem.ItemName = txtName.Text
                eItem.ItemType = GetItemType()
                eItem.ItemPackage = txtPackage.Text
                eItem.ItemPrice = txtPrice.Text
                eItem.ItemCareType = GetItemCare()
                eItem.ItemFrequency = If(txtFrequency.Text.Trim.Length = 0, 0, Val(txtFrequency.Text))
                eItem.ItemPatCat = GetItemPatCat()
                eItem.AuditUserID = imisGen.getUserId(Session("User"))
                chk = Item.SaveMedicalItem(eItem)
                If chk = 0 Then
                    Session("msg") = eItem.ItemCode & "  " & eItem.ItemName & imisGen.getMessage("M_Inserted")
                ElseIf chk = 1 Then
                    imisGen.Alert(eItem.ItemCode & imisGen.getMessage("M_Exists"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                Else
                    Session("msg") = eItem.ItemCode & "  " & eItem.ItemName & imisGen.getMessage("M_Updated")
                End If
                hfMI.Value = 0
            Catch ex As Exception
                'lblMsg.Text = ex.Message
                imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
                hfMI.Value = 1
                Return
            End Try
           
        End If
        Response.Redirect("FindMedicalItem.aspx?i=" & txtCode.Text)
    End Sub





    Private Sub setItemType()
        Dim itemtype As Char = eItem.ItemType
        If itemtype = "D" Then
            rbDrug.Checked = True
        Else
            rbMedicalProstheses.Checked = True
        End If

    End Sub
    Private Function GetItemType() As String
        Dim itemTypeValue As String = ""
        If rbDrug.Checked = True Then
            itemTypeValue = "D"
        End If
        If rbMedicalProstheses.Checked = True Then
            itemTypeValue = "M"
        End If
        If String.IsNullOrEmpty(itemTypeValue) Then
            Throw New Exception("Please select Type")
        End If
        Return itemTypeValue
    End Function
    Private Function GetItemCare() As String
        Dim CareValue As String = ""
        If rbOutPatient.Checked = True Then
            CareValue = "O" 'CareValue + 1
        End If

        If rbInPatient.Checked = True Then
            CareValue = "I" 'CareValue + 2
        End If

        If rbBoth.Checked = True Then
            CareValue = "B" 'CareValue + 4
        End If
        If String.IsNullOrEmpty(CareValue) Then
            Throw New Exception("Please select atleast one Care Type")
        End If
        Return CareValue

    End Function
    Private Sub setItemCare()

        Dim CareValue As String = eItem.ItemCareType
        If CareValue = "O" Then
            rbOutPatient.Checked = True
        ElseIf CareValue = "I" Then
            rbInPatient.Checked = True
        ElseIf CareValue = "B" Then
            rbBoth.Checked = True
        End If
        'chkDispensary.Checked = (O And CareValue)
        'chkHealthCentre.Checked = (2 And CareValue)
        'chKHospital.Checked = (4 And CareValue)

    End Sub

    Private Function GetItemPatCat() As Integer
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
    Private Sub setItemPatCat()
        Dim PatCat As Integer = eItem.ItemPatCat
        chkMan.Checked = (1 And PatCat)
        chkWoman.Checked = (2 And PatCat)
        chkAdult.Checked = (4 And PatCat)
        chkChild.Checked = (8 And PatCat)

    End Sub

   
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("FindMedicalItem.aspx?i=" & txtCode.Text)
    End Sub
End Class
