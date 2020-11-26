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


Partial Public Class FindPriceListMS
    Inherits System.Web.UI.Page
    Dim Pricelist As New IMIS_BI.FindPriceListMSBI
    Dim ePL As New IMIS_EN.tblPLServices
    Private imisGen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private priceListBI As New IMIS_BI.PricelistMSBI

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvPriceLists)
        If chkLegacy.Checked Then Page.ClientScript.RegisterForEventValidation(B_EDIT.UniqueID)
        MyBase.Render(writer)
    End Sub


    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(6).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
            'row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub
 
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RunPageSecurity()
        Try
            lblMsg.Text = ""
            If IsPostBack = True Then
                If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                    B_DELETE_Click(sender, e)
                End If
            Else
                Dim dtRegions As DataTable = Pricelist.GetRegions(imisGen.getUserId(Session("User")), True, True)
                ddlRegion.DataSource = dtRegions
                ddlRegion.DataValueField = "RegionId"
                ddlRegion.DataTextField = "RegionName"
                ddlRegion.DataBind()
                If dtRegions.Rows.Count = 1 Then
                    FillDistrict()
                End If
                LoadGrid()
            End If


        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisGen.Log(Page.Title & " : " & imisGen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub
    Private Sub FillDistrict()
        Dim LocationId As Integer = -1
        If Val(ddlRegion.SelectedValue) > 0 Then
            LocationId = ddlRegion.SelectedValue
        Else
            LocationId = -1
        End If
        ddlDistrict.DataSource = Pricelist.GetDistricts(imisGen.getUserId(Session("User")), True, LocationId)
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False)
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = imisGen.getUserId(Session("User"))
        If Not ondelete Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.PriceListMS, Page) Then
                B_ADD.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.AddPriceListMedicalServices, UserID)
                B_EDIT.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.EditPriceListMedicalServices, UserID)
                B_DELETE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.DeletePriceListMedicalServices, UserID)
                B_DUPLICATE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.DuplicatePriceListMedicalServices, UserID)
                B_SEARCH.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.FindPriceListMedicalServices, UserID)

                If Not B_EDIT.Visible And Not B_DELETE.Visible And Not B_DUPLICATE.Visible Then
                    pnlGrid.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindPriceListMS.ToString & "&retUrl=" & RefUrl)
            End If
        Else

            If Not userBI.checkRights(IMIS_EN.Enums.Rights.DeletePriceListMedicalServices, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindPriceListMS.ToString & "&retUrl=" & RefUrl)
            End If

        End If
    End Sub
    Private Sub LoadGrid() Handles B_SEARCH.Click, chkLegacy.CheckedChanged, gvPriceLists.PageIndexChanged
        Try
            lblMsg.Text = ""
            Dim eLocations As New IMIS_EN.tblLocations
            Dim LocationId As Integer = -1
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

            ePL.tblLocations = eLocations
            ePL.PLServName = txtName.Text
            ePL.AuditUserID = imisGen.getUserId(Session("User"))
            If Len(Trim(txtDate.Text)) > 0 Then ePL.DatePL = Date.Parse(txtDate.Text)
            getGridData()
        Catch ex As Exception
            'lblMsg.Text = ex.Message
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisGen.Log(Page.Title & " : " & imisGen.getLoginName(Session("User")), ex, 5, EventLogEntryType.Information)
            'EventLog.WriteEntry("IMIS", imisGen.getUserId(Session("User")) & " : " & ex.Message, EventLogEntryType.Information, 5, 3)
        End Try

    End Sub


    Private Sub getGridData()
        Dim dtUsers As DataTable = Pricelist.GetPriceListMS(ePL, chkLegacy.Checked)
        Dim sindex As Integer = 0
        Dim dv As DataView = dtUsers.DefaultView
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("ps") Is Nothing Then
                ePL.PLServName = HttpContext.Current.Request.QueryString("ps")
            Else
                ePL.PLServName = hfPLServName.Value
            End If
            If Not ePL.PLServName = "" Then
                dv.Sort = "PLServName"
                Dim x As Integer = dv.Find(ePL.PLServName)
                If x >= 0 Then
                    gvPriceLists.PageIndex = Int(x / 15)
                    Math.DivRem(x, 15, sindex)
                End If
            End If
        End If

        L_FOUNDPRICELISTS.Text = If(dv.ToTable.Rows.Count = 0, imisGen.getMessage("L_NO"), Format(dv.ToTable.Rows.Count, "#,###")) & " " & imisGen.getMessage("L_FOUNDPRICELISTS")

        gvPriceLists.DataSource = dv
        gvPriceLists.SelectedIndex = sindex
        gvPriceLists.DataBind()
        EnableButtons(gvPriceLists.Rows.Count)
    End Sub

    Private Sub B_DUPLICATE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DUPLICATE.Click
        Response.Redirect("PriceListMS.aspx?ps=" & hfPLServId.Value & "&r=0" & "&Action=duplicate")
    End Sub


    Private Sub EnableButtons(ByVal rows As Integer)
        If rows = 0 Then

            B_DELETE.Visible = False
            B_EDIT.Visible = False
            'B_VIEW.Visible = False
            B_ADD.Visible = True
            B_DUPLICATE.Visible = False
        Else
            If chkLegacy.Checked = True Then
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                'B_VIEW.Visible = True
                B_ADD.Visible = B_ADD.Visible
                B_DUPLICATE.Visible = B_DUPLICATE.Visible
            Else
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                'B_VIEW.Visible = False
                B_DUPLICATE.Visible = B_DUPLICATE.Visible
                B_ADD.Visible = B_ADD.Visible
            End If

        End If
    End Sub
    Protected Sub B_ADD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_ADD.Click

        Response.Redirect("PriceListMS.aspx")


    End Sub

    Protected Sub B_EDIT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_EDIT.Click
        Response.Redirect("PriceListMS.aspx?ps=" & hfPLServId.Value)

       

    End Sub



    Private Sub B_DELETE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DELETE.Click

        lblMsg.Text = ""
        RunPageSecurity(True)
        Try
            ePL.PLServiceID = priceListBI.GetPLServiceIdByUUID(Guid.Parse(hfPLServId.Value))

            ePL.AuditUserID = imisGen.getUserId(Session("User"))
            Pricelist.DeletePriceListMS(ePL)
            Dim FPLMS As String = hfPLServName.Value
            LoadGrid()
            Session("msg") = FPLMS & " " & imisGen.getMessage("M_DELETED")
        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisGen.Log(Page.Title & " : " & imisGen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub



    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub

    
    Private Sub gvPriceLists_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPriceLists.PageIndexChanging
        gvPriceLists.PageIndex = e.NewPageIndex
    End Sub

    'Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click
    '    Response.Redirect("PriceListMS.aspx?ps=" & hfPLServId.Value & "&r=1")
    'End Sub
    'Private Sub gvPriceLists_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPriceLists.RowDataBound
    '    If e.Row.RowType = DataControlRowType.DataRow Then
    '        e.Row.Attributes.Add("onmouseover", "this.previous_color=this.className;this.className='Hover'")
    '        e.Row.Attributes.Add("onmouseout", "this.className=this.previous_color;")
    '        e.Row.Attributes.Add("onclick", "javascript:ChangeClass('" & e.Row.ClientID & "'," & e.Row.RowIndex & ");this.previous_color=this.className")
    '    End If
    'End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistrict()
    End Sub
End Class
