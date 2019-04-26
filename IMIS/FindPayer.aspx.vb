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

Partial Public Class FindPayer
    Inherits System.Web.UI.Page
    Dim payers As New IMIS_BI.FindPayersBI
    Dim epayer As New IMIS_EN.tblPayer
     Private imisGen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Protected Language As String
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvPayers)
        If chkLegacy.Checked Then Page.ClientScript.RegisterForEventValidation(B_EDIT.UniqueID)
        MyBase.Render(writer)
    End Sub

    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(8).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
            'row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub


    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False)
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = imisGen.getUserId(Session("User"))
        If Not ondelete Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Payer, Page) Then
                B_ADD.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.AddPayer, UserID)
                B_EDIT.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.EditPayer, UserID)
                B_DELETE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.DeletePayer, UserID)
                B_SEARCH.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.FindPayer, UserID)

                If Not B_EDIT.Visible And Not B_DELETE.Visible Then
                    pnlGrid.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindPayer.ToString & "&retUrl=" & RefUrl)
            End If
        Else
            If Not userBI.checkRights(IMIS_EN.Enums.Rights.DeletePayer, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindPayer.ToString & "&retUrl=" & RefUrl)
            End If

        End If
    End Sub
   
    Private Sub Payer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RunPageSecurity()

        Try

            If IsPostBack = True Then
                If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                    B_DELETE_Click(sender, e)
                End If
            End If

            If Not IsPostBack = True Then
                Dim dtRegions As DataTable = payers.GetRegions(imisGen.getUserId(Session("User")), True, True)
                ddlRegion.DataSource = dtRegions
                ddlRegion.DataValueField = "RegionId"
                ddlRegion.DataTextField = "RegionName"
                ddlRegion.DataBind()
                If dtRegions.Rows.Count = 1 Then
                    FillDistricts()
                End If

                ddlPayerType.DataSource = payers.GetPayerType(True)
                ddlPayerType.DataValueField = "Code"
                ddlPayerType.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "PayerType", "AltLanguage")
                ddlPayerType.DataBind()
                LoadGrid()

            End If


        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub FillDistricts()
        ddlDistrict.DataSource = payers.GetDistricts(imisGen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub LoadGrid() Handles B_SEARCH.Click, chkLegacy.CheckedChanged, gvPayers.PageIndexChanged
        lblMsg.Text = ""
        Dim eLocations As New IMIS_EN.tblLocations

        Dim RegionId As Integer?
        Dim DistrictId As Integer?

        If ddlRegion.SelectedValue = "" OrElse ddlRegion.SelectedValue = 0 Then
            RegionId = 0
        ElseIf ddlRegion.SelectedValue = -1 Then
            RegionId = Nothing
        Else
            RegionId = ddlRegion.SelectedValue
        End If

        If ddlDistrict.SelectedValue = "" OrElse ddlDistrict.SelectedValue = 0 Then
            DistrictId = 0
        Else
            DistrictId = ddlDistrict.SelectedValue
        End If

        eLocations.RegionId = RegionId
        eLocations.DistrictID = DistrictId
        epayer.tblLocations = eLocations
        epayer.PayerName = txtName.Text
        epayer.Phone = txtPhone.Text
        epayer.eMail = txtEmail.Text
        epayer.PayerType = ddlPayerType.SelectedValue
        epayer.AuditUserID = imisGen.getUserId(Session("User"))

        getGridData()
    End Sub
    Private Sub getGridData()
       
        Dim dtPayers As DataTable = payers.GetPayers(epayer, chkLegacy.Checked)
        Dim sindex As Integer = 0
        Dim dv As DataView = dtPayers.DefaultView
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("p") Is Nothing Then
                epayer.PayerName = HttpContext.Current.Request.QueryString("p")
            Else
                epayer.PayerName = hfPayerName.Value
            End If
            If Not epayer.PayerName = "" Then
                dv.Sort = "PayerName"
                Dim x As Integer = dv.Find(epayer.PayerName)
                If x >= 0 Then
                    gvPayers.PageIndex = Int(x / 15)
                    Math.DivRem(x, 15, sindex)
                End If
            End If
        End If
        L_FOUNDUSERS.Text = If(dv.ToTable.Rows.Count = 0, imisGen.getMessage("L_NO"), Format(dv.ToTable.Rows.Count, "#,###")) & " " & imisGen.getMessage("L_FOUNDPAYERS")
        gvPayers.DataSource = dv
        gvPayers.SelectedIndex = sindex
        gvPayers.DataBind()
        EnableButtons(gvPayers.Rows.Count)
    End Sub
    Protected Sub B_ADD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_ADD.Click

        Response.Redirect("Payer.aspx")
      


    End Sub

    Protected Sub B_EDIT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_EDIT.Click

        Response.Redirect("Payer.aspx?p=" & hfPayerId.Value & "&r=0")

        

    End Sub



    Private Sub B_DELETE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DELETE.Click

        RunPageSecurity(True)
        Try
            lblMsg.Text = ""
            epayer.PayerID = hfPayerId.Value
            epayer.AuditUserID = imisGen.getUserId(Session("User"))
            payers.DeletePayer(epayer)
            LoadGrid()
            Dim payer As String = hfPayerName.Value
            Session("msg") = payer & " " & imisGen.getMessage("M_DELETED")
        Catch ex As Exception
            Session("Msg") = imisGen.getMessage("M_ERRORMESSAGE")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

        End Try
    End Sub
    Private Sub EnableButtons(ByVal rows As Integer)
        If rows = 0 Then

            B_DELETE.Visible = False
            B_EDIT.Visible = False
            'B_VIEW.Visible = False
            B_ADD.Visible = True

        Else
            If chkLegacy.Checked = True Then
                B_DELETE.Visible = False
                B_EDIT.Visible = False
                'B_VIEW.Visible = True
                B_ADD.Visible = False

            Else
                B_DELETE.Visible = True
                B_EDIT.Visible = True
                'B_VIEW.Visible = False

                B_ADD.Visible = True
            End If

        End If
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub
    
    
    Private Sub gvPayers_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPayers.PageIndexChanging
        gvPayers.PageIndex = e.NewPageIndex
    End Sub

    'Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click

    '    Response.Redirect("Payer.aspx?p=" & hfPayerId.Value & "&r=1")

    'End Sub

    'Private Sub gvPayers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPayers.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Attributes.Add("onmouseover", "this.previous_color=this.className;this.className='Hover'")
    '        e.Row.Attributes.Add("onmouseout", "this.className=this.previous_color;")
    '        e.Row.Attributes.Add("onclick", "javascript:ChangeClass('" & e.Row.ClientID & "'," & e.Row.RowIndex & ");this.previous_color=this.className")
    '    End If
    'End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
    End Sub
End Class
