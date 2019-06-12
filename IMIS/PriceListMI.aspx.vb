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

Partial Public Class PriceListsMI
    Inherits System.Web.UI.Page
    Private imisgen As New IMIS_Gen
    Private PriceList As New IMIS_BI.PricelisMIBI
    Private ePLItems As New IMIS_EN.tblPLItems

    Private userBI As New IMIS_BI.UserBI


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RunPageSecurity()
        Try
            lblMsg.Text = ""

            If Not String.IsNullOrEmpty(HttpContext.Current.Request.QueryString("pi")) Then
                ePLItems.PLItemUUID = Guid.Parse(HttpContext.Current.Request.QueryString("pi"))
                ePLItems.PLItemID = If(ePLItems.PLItemUUID.Equals(Guid.Empty), 0, PriceList.GetPLItemIdByUUID(ePLItems.PLItemUUID))
            Else
                ePLItems.PLItemID = 0
            End If

            If IsPostBack Then Return

            Dim dtRegion As DataTable = PriceList.GetRegions(imisgen.getUserId(Session("User")), True, True)
            ddlRegion.DataSource = dtRegion
            ddlRegion.DataValueField = "RegionId"
            ddlRegion.DataTextField = "RegionName"
            ddlRegion.DataBind()
            If dtRegion.Rows.Count = 1 Then
                FillDistricts()
            End If
            If Not ePLItems.PLItemID = 0 Then
                PriceList.LoadPriceListMI(ePLItems)
                txtName.Text = ePLItems.PLItemName
                hfCancel.Value = txtName.Text
                Dim datevalue As Date = ePLItems.DatePL
                txtDate.Text = datevalue.Date
                If ePLItems.RegionId IsNot Nothing Then
                    ddlRegion.SelectedValue = ePLItems.RegionId
                End If

                FillDistricts()
                If ePLItems.tblLocations IsNot Nothing Then
                    ddlDistrict.SelectedValue = ePLItems.tblLocations.LocationId
                End If

            End If

            If HttpContext.Current.Request.QueryString("r") = 1 Or ePLItems.ValidityTo.HasValue Then
                pnlPriceLists.Enabled = False
                B_SAVE.Visible = False
            End If

            gvMedicalItems.DataSource = PriceList.GetPLMedicalItems(ePLItems.PLItemID)
            gvMedicalItems.DataBind()
            AssignOriginalValue()
            If Request.QueryString("Action") = "duplicate" Then
                ePLItems.PLItemID = 0
                ePLItems.PLItemName = ""
                txtName.Text = ""
            End If
            txtName.Attributes.Add("Nametag", txtName.Text)
            txtDate.Attributes.Add("Datetag", txtDate.Text)
            ddlDistrict.Attributes.Add("Districttag", Val(ddlDistrict.SelectedValue))
            ddlRegion.Attributes.Add("RegionTag", Val(ddlRegion.SelectedValue))
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlMedicalItems, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub FillDistricts()
        Dim LocationId As Integer
        If Val(ddlRegion.SelectedValue) > 0 Then
            LocationId = ddlRegion.SelectedValue
        Else
            LocationId = -1
        End If
        ddlDistrict.DataSource = PriceList.GetDistricts(imisgen.getUserId(Session("User")), True, LocationId)
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.PriceListMI, Page) Then
            Dim Add As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.AddPriceListMedicalItems, UserID)
            Dim Edit As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.EditPriceListMedicalItems, UserID)

            If Not Add And Not Edit Then
                B_SAVE.Visible = False
                pnlPriceLists.Enabled = False
            End If
        Else
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.PriceListMI.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub

    Private Sub AssignOriginalValue()
        Dim _ichecked As Boolean = True
        For Each r In gvMedicalItems.Rows

            Dim chkSelect As CheckBox = CType(r.Cells(0).Controls(1), CheckBox)
            chkSelect.Checked = gvMedicalItems.DataKeys(r.RowIndex).Value
            If _ichecked = True Then
                If chkSelect.Checked = False Then
                    _ichecked = False
                End If
            End If
        Next
        CheckBox1.Checked = _ichecked
    End Sub

    Private Function CheckDifference(ByVal RowIndex As Integer, ByRef Action As Integer) As Boolean
        'Action 0 = inserted, 1 = deleted, 2 = updated
       
        Dim chkSelect As CheckBox = CType(gvMedicalItems.Rows(RowIndex).Cells(0).Controls(1), CheckBox)
        If Request.QueryString("Action") = "duplicate" Then
            Action = 0
            Return chkSelect.Checked
        End If
        Dim dNewValue As Decimal?
        Dim dOldValue As Decimal?

        If Not CType(gvMedicalItems.Rows(RowIndex).Cells(5).Controls(3), TextBox).Text = "" Then
            dNewValue = CType(gvMedicalItems.Rows(RowIndex).Cells(5).Controls(3), TextBox).Text
            'Else
            '    dNewValue = 0
        End If

        If Not gvMedicalItems.DataKeys.Item(RowIndex).Values("PriceOverule").ToString = "" Then
            dOldValue = CDec(gvMedicalItems.DataKeys.Item(RowIndex).Values("PriceOverule"))
            'Else
            '   dOldValue = 0
        End If


        If chkSelect.Checked <> gvMedicalItems.DataKeys(RowIndex).Value Then
            If chkSelect.Checked = True Then
                Action = 0
            Else
                Action = 1
            End If
            Return True
        End If
        If Not dNewValue.Equals(dOldValue) Then
            Action = 2
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub B_SAVE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_SAVE.Click
        Dim chk As Integer = 1
        ePLItems.PLItemName = txtName.Text
        If Request.QueryString("Action") = "duplicate" Then
            ePLItems.PLItemID = 0
        End If
        Dim Changed As Boolean = checkOrgValues()
        If Changed = True Then
            Try


                Dim Pricedate As Date

                If Not String.IsNullOrEmpty(txtDate.Text) Then
                    If Not IsDate(FormatDateTime(txtDate.Text, DateFormat.GeneralDate)) Then
                        Return
                    Else
                        Pricedate = FormatDateTime(txtDate.Text, DateFormat.GeneralDate)
                    End If
                Else
                    imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If

                Dim eLocations As New IMIS_EN.tblLocations

                Dim LocationId As Integer
                If Val(ddlDistrict.SelectedValue) > 0 Then
                    LocationId = ddlDistrict.SelectedValue
                ElseIf Val(ddlRegion.SelectedValue) > 0 Then
                    LocationId = ddlRegion.SelectedValue
                Else
                    LocationId = -1
                End If

                eLocations.LocationId = LocationId

                ePLItems.DatePL = Pricedate

                ePLItems.tblLocations = eLocations
                chk = PriceList.SavePriceListMI(ePLItems)

                If chk = 0 Then
                    Session("msg") = ePLItems.PLItemName & imisgen.getMessage("M_Inserted")

                ElseIf chk = 1 Then
                    imisgen.Alert(ePLItems.PLItemName & imisgen.getMessage("M_Exists"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                Else
                    Session("msg") = ePLItems.PLItemName & imisgen.getMessage("M_Updated")
                End If

            Catch ex As Exception
                'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlMedicalItems, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
                Return
            End Try
        End If
        Try
            Dim action As Integer
          
            For Each r In gvMedicalItems.Rows
                If CheckDifference(r.RowIndex, action) = True Then
                    'insert and update grid here
                    Dim eItemDetails As New IMIS_EN.tblPLItemsDetail
                    Dim Item As New IMIS_EN.tblItems
                    If Not gvMedicalItems.DataKeys.Item(r.RowIndex).Values("PriceOverule") Is DBNull.Value Or Not CType(gvMedicalItems.Rows(r.RowIndex).Cells(5).Controls(3), TextBox).Text = "" Then
                        eItemDetails.PriceOverule = CType(gvMedicalItems.Rows(r.RowIndex).Cells(5).Controls(3), TextBox).Text
                    End If
                    Item.ItemID = gvMedicalItems.DataKeys.Item(r.RowIndex).Values("ItemID")
                    If action > 0 Then

                        If Not gvMedicalItems.DataKeys.Item(r.RowIndex).Values("PLItemDetailID") Is DBNull.Value Then
                            eItemDetails.PLItemDetailID = gvMedicalItems.DataKeys.Item(r.RowIndex).Values("PLItemDetailID")
                        End If

                    End If

                    eItemDetails.tblItems = Item
                    eItemDetails.tblPLItems = ePLItems
                    eItemDetails.AuditUserID = imisgen.getUserId(Session("User"))
                    PriceList.SavePLItemDetails(eItemDetails, action)
                    If Changed = False Then Session("msg") = ePLItems.PLItemName & imisgen.getMessage("M_Updated")
                End If
            Next


        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlMedicalItems, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

        Response.Redirect("FindPriceListMI.aspx?pi=" & ePLItems.PLItemName)

    End Sub

    Protected Sub gvMedicalItems_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMedicalItems.PageIndexChanging
        gvMedicalItems.PageIndex = e.NewPageIndex

    End Sub

    Private Function checkOrgValues() As Boolean
        If txtName.Attributes.Item("Nametag") <> txtName.Text Then
        ElseIf txtDate.Attributes.Item("datetag") <> txtDate.Text Then
        ElseIf ddlDistrict.Attributes.Item("Districttag") <> ddlDistrict.SelectedValue Then
        ElseIf ddlRegion.Attributes.Item("RegionTag") <> ddlRegion.SelectedValue Then
        Else
            Return False
        End If
        Return True

    End Function

    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("FindPriceListMI.aspx?pi=" & txtName.Text.Trim)
    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
    End Sub
End Class
