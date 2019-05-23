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



Partial Public Class FindPriceListMI
    Inherits System.Web.UI.Page
    Private Pricelists As New IMIS_BI.FindPricelistMIBI
    Private ePL As New IMIS_EN.tblPLItems
    Private imisGen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private PriceListBI As New IMIS_BI.PricelisMIBI

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

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RunPageSecurity()

        Try
            lblMsg.Text = ""

            If IsPostBack = True Then
                If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                    B_DELETE_Click(sender, e)
                End If
            End If

            If Not IsPostBack = True Then
                Dim dtRegions As DataTable = Pricelists.GetRegions(imisGen.getUserId(Session("User")), True, True)
                ddlRegion.DataSource = dtRegions
                ddlRegion.DataValueField = "RegionId"
                ddlRegion.DataTextField = "RegionName"
                ddlRegion.DataBind()
                If dtRegions.Rows.Count = 1 Then
                    FillDistricts()
                End If
                loadGrid()
            End If


        Catch ex As Exception
            Session("Msg") = imisGen.getMessage("M_ERRORMESSAGE")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

        End Try

    End Sub
    Private Sub FillDistricts()
        Dim LocationId As Integer = -1
        If Val(ddlRegion.SelectedValue) > 0 Then
            LocationId = ddlRegion.SelectedValue
        Else
            LocationId = -1
        End If
        ddlDistrict.DataSource = Pricelists.GetDistricts(imisGen.getUserId(Session("User")), True, LocationId)
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub

    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False)
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = imisGen.getUserId(Session("User"))
        If Not ondelete Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.PriceListMI, Page) Then
                B_ADD.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.AddPriceListMedicalItems, UserID)
                B_EDIT.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.EditPriceListMedicalItems, UserID)
                B_DELETE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.DeletePriceListMedicalItems, UserID)
                B_DUPLICATE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.DuplicatePriceListMedicalItems, UserID)
                B_SEARCH.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.FindPriceListMedicalItems, UserID)

                If Not B_EDIT.Visible And Not B_DELETE.Visible And Not B_DUPLICATE.Visible Then
                    pnlGrid.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindPriceListMI.ToString & "&retUrl=" & RefUrl)
            End If
        Else

            If Not userBI.checkRights(IMIS_EN.Enums.Rights.DeletePriceListMedicalItems, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindPriceListMI.ToString & "&retUrl=" & RefUrl)
            End If

        End If
    End Sub

    Private Sub loadGrid() Handles B_SEARCH.Click, chkLegacy.CheckedChanged, gvPriceLists.PageIndexChanged
        Try
            ePL.PLItemName = txtName.Text
            If Len(Trim(txtDate.Text)) > 0 Then ePL.DatePL = Date.Parse(txtDate.Text)
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
            ePL.tblLocations = eLocations
            ePL.AuditUserID = imisGen.getUserId(Session("User"))
            getGridData()
        Catch ex As Exception
            Session("msg") = ex.Message
        End Try

    End Sub


    Private Sub getGridData()
        Dim dtUsers As DataTable = Pricelists.GetPriceListMI(ePL, chkLegacy.Checked)
        Dim sindex As Integer = 0
        Dim dv As DataView = dtUsers.DefaultView
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("pi") Is Nothing Then
                ePL.PLItemName = HttpContext.Current.Request.QueryString("pi")
            Else
                ePL.PLItemName = hfPLItemName.Value
            End If
            If Not ePL.PLItemName = "" Then
                dv.Sort = "PLItemName"
                Dim x As Integer = dv.Find(ePL.PLItemName)
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

        Response.Redirect("PriceListMI.aspx")




    End Sub

    Protected Sub B_EDIT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_EDIT.Click

        Response.Redirect("PriceListMI.aspx?pi=" & hfPLItemId.Value)

    End Sub


    Private Sub B_DELETE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DELETE.Click
        RunPageSecurity(True)
        Try
            ePL.PLItemID = PriceListBI.GetPLItemIdByUUID(Guid.Parse(hfPLItemId.Value))

            ePL.AuditUserID = imisGen.getUserId(Session("User"))
            Pricelists.DeletePriceListMI(ePL)
            Dim FPLMI As String = hfPLItemName.Value
            loadGrid()
            Session("msg") = FPLMI & " " & imisGen.getMessage("M_DELETED")
        Catch ex As Exception
            Session("Msg") = imisGen.getMessage("M_ERRORMESSAGE")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

        End Try
    End Sub



    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub


    Protected Sub gvPriceLists_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPriceLists.PageIndexChanging
        gvPriceLists.PageIndex = e.NewPageIndex

    End Sub


    'Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click
    '    Response.Redirect("PriceListMI.aspx?pi=" & hfPLItemId.Value & "&r=1")

    'End Sub
    Private Sub B_DUPLICATE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DUPLICATE.Click
        Response.Redirect("PriceListMI.aspx?pi=" & hfPLItemId.Value & "&r=0" & "&Action=duplicate")
    End Sub

    'Private Sub gvPriceLists_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPriceLists.RowDataBound
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
