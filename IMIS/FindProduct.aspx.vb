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

Partial Public Class FindProduct
    Inherits System.Web.UI.Page
    Dim products As New IMIS_BI.FindProductsBI
    Private EpRODUCTS As New IMIS_EN.tblProduct
    Public imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private productBI As New IMIS_BI.ProductBI
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvProducts)
        If chkLegacy.Checked Then Page.ClientScript.RegisterForEventValidation(B_EDIT.UniqueID)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(14).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
        Next
    End Sub
    Private Sub Product_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""
        If Request.QueryString("m") = "True" Then
            Dim str As String = imisgen.getMessage("M_NODISTRIBUTION")
            imisgen.Alert(str, pnlTop, , , "IMIS")
        End If
        Dim RefUrl = Request.Headers("Referer")
        Dim reg As New Regex("FindProduct", RegexOptions.IgnoreCase) '
        RunPageSecurity()
        Try
            If Not IsPostBack = True Then
                FillRegions()
                LoadGrid()
            Else
                If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                    B_DELETE_Click(sender, e)
                End If
            End If
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub FillRegions()
        Dim dtRegion As DataTable = products.GetRegions(imisgen.getUserId(Session("User")), True, True)
        ddlRegion.DataSource = dtRegion
        ddlRegion.DataValueField = "RegionId"
        ddlRegion.DataTextField = "RegionName"
        ddlRegion.DataBind()
        If dtRegion.Rows.Count = 1 Then
            FillDistricts()
        End If
    End Sub
    Private Sub FillDistricts()
        ddlDistrict.DataSource = products.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False, Optional ByVal cmd As String = "delete")
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If Not ondelete Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.FindProduct, Page) Then
                B_ADD.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ProductAdd, UserID)
                B_EDIT.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ProductEdit, UserID)
                B_DUPLICATE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ProductDuplicate, UserID)
                B_DELETE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ProductDelete, UserID)
                B_SEARCH.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ProductSearch, UserID)


                If Not B_EDIT.Visible And Not B_DUPLICATE.Visible And Not B_DELETE.Visible Then
                    pnlGrid.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindProduct.ToString & "&retUrl=" & RefUrl)
            End If
        Else
            If Not products.checkRights(IMIS_EN.Enums.Rights.ProductDelete, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindProduct.ToString & "&retUrl=" & RefUrl)
            End If
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadGrid() Handles B_SEARCH.Click, chkLegacy.CheckedChanged, gvProducts.PageIndexChanged
        Try
            lblMsg.Text = ""
            Dim eLocations As New IMIS_EN.tblLocations
            'If ddlDistrict.SelectedValue <> "" Then
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
            'eLocations.LocationId = If(Val(ddlDistrict.SelectedValue) = 0, Nothing, Val(ddlDistrict.SelectedValue))
            'End If
            EpRODUCTS.tblLocation = eLocations
            EpRODUCTS.ProductCode = txtProductCode.Text
            EpRODUCTS.ProductName = txtProductName.Text
            EpRODUCTS.AuditUserID = imisgen.getUserId(Session("User"))
            If Len(Trim(txtDateFrom.Text)) > 0 AndAlso txtDateFrom.Text <> "__/__/____" Then EpRODUCTS.DateFrom = Date.Parse(txtDateFrom.Text)

            If Len(Trim(txtDateTo.Text)) > 0 AndAlso txtDateTo.Text <> "__/__/____" Then EpRODUCTS.DateTo = Date.Parse(txtDateTo.Text)
            getGridData()
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
    Private Sub getGridData()
        Dim dtProduct As DataTable = products.GetProducts(EpRODUCTS, chkLegacy.Checked)
        Dim sindex As Integer = 0
        Dim dv As DataView = dtProduct.DefaultView
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("p") Is Nothing Then
                EpRODUCTS.ProductCode = HttpContext.Current.Request.QueryString("p")
            Else
                EpRODUCTS.ProductCode = hfProdCode.Value
            End If

            If Not EpRODUCTS.ProductCode = "" Then
                dv.Sort = "ProductCode"
                Dim x As Integer = dv.Find(EpRODUCTS.ProductCode)
                If x >= 0 Then
                    gvProducts.PageIndex = Int(x / 15)
                    Math.DivRem(x, 15, sindex)
                End If
            End If
        End If
        L_FOUNDPRODUCTS.Text = If(dv.ToTable.Rows.Count = 0, imisgen.getMessage("L_NO"), Format(dv.ToTable.Rows.Count, "#,###")) & " " & imisgen.getMessage("L_FOUNDPRODUCTS", False)
        gvProducts.DataSource = dv
        gvProducts.SelectedIndex = sindex
        gvProducts.DataBind()
        EnableButtons(gvProducts.Rows.Count)
    End Sub
    Protected Sub gvProducts_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvProducts.PageIndexChanging
        gvProducts.PageIndex = e.NewPageIndex
    End Sub
    Private Sub EnableButtons(ByVal rows As Integer)
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If rows = 0 Then
            B_DELETE.Visible = False
            B_EDIT.Visible = False
            B_ADD.Visible = B_ADD.Visible
            B_DUPLICATE.Visible = False
        Else
            If chkLegacy.Checked = True Then
                B_DELETE.Visible = False
                B_EDIT.Visible = False
                B_ADD.Visible = False
                B_DUPLICATE.Visible = False
            Else
                B_DELETE.Visible = B_DELETE.Visible
                B_EDIT.Visible = B_EDIT.Visible
                B_DUPLICATE.Visible = B_DUPLICATE.Visible
                B_ADD.Visible = B_ADD.Visible
            End If

        End If
    End Sub
    Protected Sub B_ADD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_ADD.Click
        Try
            Response.Redirect("Product.aspx")
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Protected Sub B_EDIT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_EDIT.Click
        Response.Redirect("Product.aspx?p=" & hfProdId.Value)
    End Sub
    Private Sub B_DELETE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DELETE.Click
        RunPageSecurity(True)
        Try
            lblMsg.Text = ""
            Dim ProdUUID As Guid = Guid.Parse(hfProdId.Value)
            Dim ProdId As Integer = productBI.GetProdIdByUUID(ProdUUID)
            EpRODUCTS.ProdID = ProdId
            EpRODUCTS.AuditUserID = imisgen.getUserId(Session("User"))
            Dim productCode As String = hfProdCode.Value
            Dim Msg As String = ""
            If products.DeleteProduct(EpRODUCTS) = True Then
                Msg = productCode & " " & imisgen.getMessage("M_DELETED")
            Else
                Msg = productCode & " " & "Can not be deleted"
            End If
            LoadGrid()
            lblMsg.Text = Msg
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Server.Transfer("Home.aspx")
    End Sub
    Private Sub B_DUPLICATE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DUPLICATE.Click
        Response.Redirect("Product.aspx?p=" & hfProdId.Value & "&r=0&action=duplicate")
    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
    End Sub
End Class
