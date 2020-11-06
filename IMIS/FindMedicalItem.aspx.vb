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

Partial Public Class FindMedicalItem
    Inherits System.Web.UI.Page
    Dim MedicalItems As New IMIS_BI.FindMedicalItemsBI
    Dim eItems As New IMIS_EN.tblItems
    Private imisGen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Dim MedicalItemBI As New IMIS_BI.MedicalItemBI

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvMedicalItems)
        If chkLegacy.Checked Then Page.ClientScript.RegisterForEventValidation(B_EDIT.UniqueID)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(7).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
            'row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub
   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RunPageSecurity()

        Try
            If IsPostBack = True Then
                If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                    B_DELETE_Click(sender, e)
                End If
            End If

            If Not IsPostBack = True Then
                ddlType.DataSource = MedicalItems.GetItemType(True)
                ddlType.DataValueField = "ItemID"
                ddlType.DataTextField = "ItemType"
                ddlType.DataBind()
                loadGrid()
            End If


        Catch ex As Exception
            imisGen.Log(Page.Title & " : " & imisGen.getLoginName(Session("User")), ex, 5, EventLogEntryType.Information)
            'EventLog.WriteEntry("IMIS", imisGen.getUserId(Session("User")) & " : " & ex.Message, EventLogEntryType.Information, 5, 3)
            'lblMsg.Text = ex.Message
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
        End Try

    End Sub

    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False)
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = imisGen.getUserId(Session("User"))
        If Not ondelete Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.MedicalItem, Page) Then
                B_ADD.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.AddMedicalItem, UserID)
                B_EDIT.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.EditMedicalItem, UserID)
                B_DELETE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.DeleteMedicalItem, UserID)
                B_SEARCH.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.FindMedicalItem, UserID)

                If Not B_EDIT.Visible And Not B_DELETE.Visible Then
                    pnlGrid.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindMedicalItem.ToString & "&retUrl=" & RefUrl)
            End If
        Else

            If Not userBI.checkRights(IMIS_EN.Enums.Rights.DeleteMedicalItem, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindMedicalItem.ToString & "&retUrl=" & RefUrl)
            End If

        End If
    End Sub
    Private Sub loadGrid() Handles B_SEARCH.Click, chkLegacy.CheckedChanged, gvMedicalItems.PageIndexChanged
        lblMsg.Text = ""
        eItems.ItemCode = txtItemCode.Text
        eItems.ItemName = txtItemName.Text
        eItems.ItemName = txtItemName.Text
        eItems.ItemPackage = txtPackage.Text
        eItems.ItemType = ddlType.SelectedValue
        eItems.AuditUserID = imisGen.getUserId(Session("User"))
        getGridData()


    End Sub
    Private Sub getGridData()
        Dim dtUsers As DataTable = MedicalItems.GetMedicalItems(eItems, chkLegacy.Checked)
        Dim sindex As Integer = 0
        Dim dv As DataView = dtUsers.DefaultView
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("i") Is Nothing Then
                eItems.ItemCode = HttpContext.Current.Request.QueryString("i")
            Else
                eItems.ItemCode = hfItemCode.Value
            End If
            If Not eItems.ItemCode = "" Then
                dv.Sort = "ItemCode"
                Dim x As Integer = dv.Find(eItems.ItemCode)
                If x >= 0 Then
                    gvMedicalItems.PageIndex = Int(x / 15)
                    Math.DivRem(x, 15, sindex)
                End If
            End If
        End If
        L_FOUNDITEMS.Text = If(dv.ToTable.Rows.Count = 0, imisGen.getMessage("L_NO"), Format(dv.ToTable.Rows.Count, "#,###")) & " " & imisGen.getMessage("L_FOUNDITEMS")
        gvMedicalItems.DataSource = dv
        gvMedicalItems.SelectedIndex = sindex
        gvMedicalItems.DataBind()
        EnableButtons(gvMedicalItems.Rows.Count)
    End Sub
    Protected Sub B_ADD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_ADD.Click
        Response.Redirect("MedicalItem.aspx")

    End Sub
    Protected Sub B_EDIT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_EDIT.Click

        Response.Redirect("MedicalItem.aspx?i=" & hfItemId.Value)


    End Sub
    Private Sub B_DELETE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DELETE.Click
        RunPageSecurity(True)
        Try
            lblMsg.Text = ""
            Dim ItemUUID As Guid = Guid.Parse(hfItemId.Value)
            Dim ItemId As Integer = MedicalItemBI.GetItemIdByUUID(ItemUUID)
            eItems.ItemID = ItemId

            eItems.AuditUserID = imisGen.getUserId(Session("User"))
            Dim chk As Boolean = MedicalItems.DeleteItem(eItems)
            Dim FMI As String = hfItemCode.Value
            loadGrid()
            If chk = True Then

                Session("Msg") = FMI & " " & imisGen.getMessage("M_DELETED")
            Else
                lblMsg.Text = FMI & " " & imisGen.getMessage("M_INUSE")
            End If

        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisGen.Log(Page.Title & " : " & imisGen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Server.Transfer("Home.aspx")
    End Sub
    Private Sub EnableButtons(ByVal rows As Integer)
        If rows = 0 Then

            B_DELETE.Visible = False
            B_EDIT.Visible = False
            B_ADD.Visible = True

        Else
            If chkLegacy.Checked = True Then
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                B_ADD.Visible = False

            Else
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                B_ADD.Visible = B_ADD.Visible
            End If

        End If
    End Sub
    Private Sub gvMedicalItems_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMedicalItems.PageIndexChanging
        gvMedicalItems.PageIndex = e.NewPageIndex
    End Sub
    'Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click

    '    Response.Redirect("MedicalItem.aspx?i=" & hfItemId.Value & "&r=1")
    'End Sub
    'Private Sub gvMedicalItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMedicalItems.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Attributes.Add("onmouseover", "this.previous_color=this.className;this.className='Hover'")
    '        e.Row.Attributes.Add("onmouseout", "this.className=this.previous_color;")
    '        e.Row.Attributes.Add("onclick", "javascript:ChangeClass('" & e.Row.ClientID & "'," & e.Row.RowIndex & ");this.previous_color=this.className")
    '    End If
    'End Sub
End Class
