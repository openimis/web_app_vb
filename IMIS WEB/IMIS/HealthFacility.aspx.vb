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

Partial Public Class HealthFacility
    Inherits System.Web.UI.Page
    Private eHF As New IMIS_EN.tblHF
    Dim eLocation As New IMIS_EN.tblLocations
    Dim ePLServices As New IMIS_EN.tblPLServices
    Dim ePLItems As New IMIS_EN.tblPLItems
    Dim HF As New IMIS_BI.HealthFacilityBI
    Public imisgen As New IMIS_Gen
    Private IsCheckBoxChecked As Boolean = False
    Private userBI As New IMIS_BI.UserBI
    Dim dtcopy As DataTable
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        AddRowSelectToGridView(gvRegion)
        AddRowSelectToGridView(gvDistrict)
        AddRowSelectToGridView(gvWards)
        'AddRowSelectToGridView(gvVillage)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'ViewState("Districts") = gvDistrict.DataSource
        lblMsg.Text = ""
        eHF.HfID = HttpContext.Current.Request.QueryString("h")
        If HttpContext.Current.Request.QueryString("r") = 1 Or eHF.ValidityTo.HasValue Then
            Panel2.Enabled = False
            B_SAVE.Visible = False
        End If
        If IsPostBack = True Then Return
      
        RunPageSecurity()
        Try

            LoadCatchmentArea(eHF.HfID)


            Dim dtRegions As DataTable = HF.GetRegions(imisgen.getUserId(Session("User")), True)

            ddlRegion.DataSource = dtRegions
            ddlRegion.DataValueField = "RegionId"
            ddlRegion.DataTextField = "RegionName"
            ddlRegion.DataBind()
            If dtRegions.Rows.Count = 1 Then
                FillDistricts()
            End If

            ddlLegalForm.DataSource = HF.GetHFLegal(True)
            ddlLegalForm.DataValueField = "LegalFormCode"
            ddlLegalForm.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "LegalForms", "AltLanguage")
            ddlLegalForm.DataBind()

            ddlHFLevel.DataSource = HF.GetHFLevel(True)
            ddlHFLevel.DataValueField = "Code"
            ddlHFLevel.DataTextField = "HFLevel"
            ddlHFLevel.DataBind()

            ddlType.DataSource = HF.GetHFType(True)
            ddlType.DataValueField = "Code"
            ddlType.DataTextField = "HFCareType"
            ddlType.DataBind()

            ddlSublevel.DataSource = HF.GetSublevel
            ddlSublevel.DataValueField = "HFSublevel"
            ddlSublevel.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "HFSublevelDesc", "AltLanguage")
            ddlSublevel.DataBind()


            If Not eHF.HfID = 0 Then
                HF.LoadHF(eHF)
                ddlLegalForm.SelectedValue = eHF.LegalForm
                ddlHFLevel.SelectedValue = eHF.HFLevel
                ddlSublevel.SelectedValue = eHF.HFSublevel
                txtHFCode.Text = eHF.HFCode
                txtFacilityName.Text = eHF.HFName
                txtAddress.Text = eHF.HFAddress
                ddlRegion.SelectedValue = If(eHF.RegionId IsNot Nothing, eHF.RegionId, 0)
                FillDistricts()
                ddlDistrict.SelectedValue = If(eHF.tblLocations.LocationId, eHF.tblLocations.LocationId, 0)
                txtPhone.Text = eHF.Phone
                txtFax.Text = eHF.Fax
                txtEmail.Text = eHF.eMail
                ddlType.SelectedValue = eHF.HFCareType
                ddOwnPricerListService.SelectedValue = eHF.tblPLServices.PLServiceID
                ddOwnPricerListItem.SelectedValue = eHF.tblPLItems.PLItemID
                txtAccCode.Text = eHF.AccCode
            End If

            ddOwnPricerListService.DataSource = HF.GetPLServices(imisgen.getUserId(Session("User")), Val(ddlRegion.SelectedValue), Val(ddlDistrict.SelectedValue), True)
            ddOwnPricerListService.DataValueField = "PLServiceID"
            ddOwnPricerListService.DataTextField = "PLServName"
            ddOwnPricerListService.DataBind()

            ddOwnPricerListItem.DataSource = HF.GetPLItems(imisgen.getUserId(Session("User")), Val(ddlRegion.SelectedValue), Val(ddlDistrict.SelectedValue), True)
            ddOwnPricerListItem.DataValueField = "PLItemID"
            ddOwnPricerListItem.DataTextField = "PLItemName"
            ddOwnPricerListItem.DataBind()

            If HttpContext.Current.Request.QueryString("r") = 1 Or eHF.ValidityTo.HasValue Then
                Panel2.Enabled = False
                B_SAVE.Visible = False
            End If
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try


    End Sub
    Private Sub FillDistricts()
        Dim dt As DataTable = HF.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataSource = dt
        ddlDistrict.DataValueField = "DistrictID"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
        If dt.Rows.Count = 1 Then
            FillPriceList()
        End If
    End Sub

    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.HealthFacility, Page) Then
            Dim Add As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.HealthFacilityAdd, UserID)
            Dim Edit As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.HealthFacilityEdit, UserID)

            If Not Add And Not Edit Then
                B_SAVE.Visible = False
                Panel2.Enabled = False
            End If
        Else
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.HealthFacility.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub

    Protected Sub B_SAVE_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_SAVE.Click
        If CType(Me.Master.FindControl("hfDirty"), HiddenField).Value = True Then
            Try

                Dim dt As New DataTable
                dt = DirectCast(Session("User"), DataTable)
                Dim ehfNew As New IMIS_EN.tblHF

                eHF.HFName = txtFacilityName.Text
                eHF.LegalForm = ddlLegalForm.SelectedValue
                eHF.HFLevel = ddlHFLevel.SelectedValue
                eHF.HFSublevel = ddlSublevel.SelectedValue
                eHF.HFCode = txtHFCode.Text
                eHF.HFAddress = txtAddress.Text
                eLocation.LocationId = ddlDistrict.SelectedValue
                eHF.tblLocations = eLocation
                eHF.Phone = if(txtPhone.Text Is Nothing, SqlTypes.SqlString.Null, txtPhone.Text)
                eHF.Fax = if(txtFax.Text Is Nothing, SqlTypes.SqlString.Null, txtFax.Text)
                eHF.eMail = if(txtEmail.Text Is Nothing, SqlTypes.SqlString.Null, txtEmail.Text)
                eHF.HFCareType = ddlType.SelectedValue
                ePLServices.PLServiceID = if(ddOwnPricerListService.SelectedValue = "", SqlTypes.SqlInt32.Null, ddOwnPricerListService.SelectedValue)
                eHF.tblPLServices = ePLServices
                ePLItems.PLItemID = if(ddOwnPricerListItem.SelectedValue = "", SqlTypes.SqlInt32.Null, ddOwnPricerListItem.SelectedValue)
                eHF.tblPLItems = ePLItems
                eHF.AccCode = if(txtAccCode.Text Is Nothing, SqlTypes.SqlString.Null, txtAccCode.Text)
                eHF.AuditUserID = imisgen.getUserId(Session("User"))

                Dim chk As Integer = HF.SaveHealthFacilities(eHF, getCatchmentDt)
                If chk = 0 Then
                    Session("msg") = eHF.HFCode & " " & eHF.HFName & imisgen.getMessage("M_Inserted")

                ElseIf chk = 1 Then
                    imisgen.Alert(eHF.HFCode & " " & eHF.HFName & imisgen.getMessage("M_Exists"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                Else
                    Session("msg") = eHF.HFCode & " " & eHF.HFName & imisgen.getMessage("M_Updated")
                End If

            Catch ex As Exception
                'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
                Return
            End Try
        End If
        Response.Redirect("FindHealthFacility.aspx?h=" & txtHFCode.Text)
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("FindHealthFacility.aspx?h=" & txtHFCode.Text)
    End Sub
    Private Sub ddlDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged
        FillPriceList()
    End Sub
    Private Sub FillPriceList()
        ddOwnPricerListService.DataSource = HF.GetPLServices(imisgen.getUserId(Session("User")), Val(ddlRegion.SelectedValue), Val(ddlDistrict.SelectedValue), True)
        ddOwnPricerListService.DataValueField = "PLServiceID"
        ddOwnPricerListService.DataTextField = "PLServName"
        ddOwnPricerListService.DataBind()
        ddOwnPricerListItem.DataSource = HF.GetPLItems(imisgen.getUserId(Session("User")), Val(ddlRegion.SelectedValue), Val(ddlDistrict.SelectedValue), True)
        ddOwnPricerListItem.DataValueField = "PLItemID"
        ddOwnPricerListItem.DataTextField = "PLItemName"
        ddOwnPricerListItem.DataBind()
    End Sub
    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
    End Sub

    

    Private Sub FillWards()
        If Not Val(ddlDistrict.SelectedValue) > 0 Then Exit Sub
        Dim dtWards As DataTable = HF.GetWards(Val(ddlDistrict.SelectedValue), 0)
        gvWards.DataSource = dtWards
        gvWards.DataBind()
        'SetCheckboxes(gvWards)
        fillVillages()
    End Sub
    Private Sub fillVillages()
        Dim dtVillages As DataTable = HF.GetVillages(Val(ddlDistrict.SelectedValue), 0)
        gvVillage.datasource = dtVillages
        gvVillage.DataBind()
        ' SetCheckboxes(gvVillage)
    End Sub
    Private Sub LoadCatchmentArea(ByVal HFId As Integer)
        Dim dtRegion As DataTable = HF.LoadCathmentRegions(HFId)
        ViewState("Regions") = dtRegion
        gvRegion.DataSource = dtRegion
        gvRegion.DataBind()
        ' SetCheckboxes(gvRegion)

        Dim dtDistricts As DataTable = HF.LoadCatchmentDistricts(HFId)
        ViewState("Districts") = dtDistricts
        gvDistrict.DataSource = dtDistricts
        gvDistrict.DataBind()
        'SetCheckboxes(gvDistrict)

        Dim dtWard As DataTable = HF.LoadCatchmentWard(HFId)
        ViewState("Wards") = dtWard
        gvWards.DataSource = dtWard
        gvWards.DataBind()
        'SetCheckboxes(gvWards)

        Dim dtVillage As DataTable = HF.LoadCatchmentVillage(HFId)
        ViewState("Villages") = dtVillage
        gvVillage.DataSource = dtVillage
        gvVillage.DataBind()
        ' SetCheckboxes(gvVillage)
    End Sub

    Private Sub gvRegion_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRegion.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowIndex <= 0 Then Return
            Dim chk As CheckBox = DirectCast(gvRegion.Rows(e.Row.RowIndex - 1).FindControl("chkRegionSelect"), CheckBox)
            If Not chk Is Nothing Then
                chk.Attributes.Add("OnCheckedChanged", "__dopostback('" & gvRegion.UniqueID & "', 'Select$" & e.Row.RowIndex - 1 & "')")
            End If
        End If
    End Sub
    Private Sub gvDistrict_DataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDistrict.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowIndex <= 0 Then Return
            Dim chk As CheckBox = DirectCast(gvDistrict.Rows(e.Row.RowIndex - 1).FindControl("chkDistrictSelect"), CheckBox)
            If Not chk Is Nothing Then
                chk.Attributes.Add("OnCheckedChanged", "__dopostback('" & gvDistrict.UniqueID & "', 'Select$" & e.Row.RowIndex - 1 & "')")
            End If
        End If
    End Sub
    Private Sub gvWards_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvWards.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowIndex <= 0 Then Return
            Dim chk As CheckBox = DirectCast(gvWards.Rows(e.Row.RowIndex - 1).FindControl("chkWardSelect"), CheckBox)
            If Not chk Is Nothing Then
                chk.Attributes.Add("OnCheckedChanged", "__dopostback('" & gvWards.UniqueID & "', 'Select$" & e.Row.RowIndex - 1 & "')")
            End If
        End If
    End Sub
    Protected Sub gvRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRegion.SelectedIndexChanged
        If IsCheckBoxChecked Then
            Dim chkSelect As CheckBox = CType(gvRegion.Rows(gvRegion.SelectedIndex).Cells(0).Controls(1), CheckBox)
            FillDistrictGrid(gvRegion.SelectedDataKey("RegionId"), CBool(chkSelect.Checked))
        End If
        IsCheckBoxChecked = False
    End Sub

  
    Private Sub gvDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvDistrict.SelectedIndexChanged
        If IsCheckBoxChecked Then
            Dim chkSelect As CheckBox = CType(gvDistrict.Rows(gvDistrict.SelectedIndex).Cells(0).Controls(1), CheckBox)
            FillWardGrid(gvDistrict.SelectedDataKey("DistrictId"), CBool(chkSelect.Checked))
        End If
        IsCheckBoxChecked = False
    End Sub

   
    Private Sub gvWards_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvWards.SelectedIndexChanged
        If IsCheckBoxChecked Then
            Dim chkSelect As CheckBox = CType(gvWards.Rows(gvWards.SelectedIndex).Cells(0).Controls(1), CheckBox)
            FillVillageGrid(gvWards.SelectedDataKey("WardId"), CBool(chkSelect.Checked))
        End If
        IsCheckBoxChecked = False
    End Sub
    Private Sub FillDistrictGrid(ByVal RegionId As Integer, ByVal checked As Boolean)

        Dim dtDistricts As DataTable = HF.GetDistricts(imisgen.getUserId(Session("User")), False, RegionId)
        Dim dtSelectedDistricts As DataTable
        dtSelectedDistricts = DirectCast(ViewState("Districts"), DataTable)

        For Each dr As GridViewRow In gvDistrict.Rows
            Dim chk As Boolean = DirectCast(gvDistrict.Rows(dr.RowIndex).Cells(0).Controls(1), CheckBox).Checked
            Dim DistrictId As Integer = gvDistrict.DataKeys(dr.RowIndex).Values("DistrictId")
            Dim row As DataRow = dtSelectedDistricts.Select("DistrictId = " & DistrictId & "")(0)
            row("Checked") = chk
        Next
        ViewState("Districts") = dtSelectedDistricts

        If checked Then
            For Each row As DataRow In dtDistricts.Rows
                Dim newrow As DataRow = dtSelectedDistricts.NewRow
                newrow("Checked") = False
                newrow("DistrictId") = row("DistrictId")
                newrow("DistrictName") = row("DistrictName")
                newrow("Region") = row("Region")
                dtSelectedDistricts.Rows.Add(newrow)
            Next
        Else

            Dim dr() As DataRow = dtSelectedDistricts.Select("Region='" & RegionId & "'")
            Dim row As DataRow = dtDistricts.NewRow
            row("Region") = If(dr.Length = 0, 0, dr(0)("Region"))
RemoveRow:
            For Each selectedRow In dtSelectedDistricts.Rows
                If selectedRow("Region") = row("Region") Then
                    RemoveWard(selectedRow("DistrictId"))
                    dtSelectedDistricts.Rows.Remove(selectedRow)
                    GoTo RemoveRow
                End If
            Next
        End If

        ViewState("Districts") = dtSelectedDistricts
        gvDistrict.DataSource = dtSelectedDistricts
        gvDistrict.DataBind()

    End Sub
    Private Sub FillWardGrid(ByVal DistrictId As Integer, ByVal checked As Boolean)
        Dim dtWard As DataTable = HF.GetAllWards(DistrictId, False)
        Dim dtSelectedWard As DataTable = DirectCast(ViewState("Wards"), DataTable)

        For Each dr As GridViewRow In gvWards.Rows
            Dim chk As Boolean = DirectCast(gvWards.Rows(dr.RowIndex).Cells(0).Controls(1), CheckBox).Checked
            Dim WardId As Integer = gvWards.DataKeys(dr.RowIndex).Values("WardId")
            Dim row As DataRow = dtSelectedWard.Select("WardId = " & WardId & "")(0)
            row("Checked") = chk
        Next

        ViewState("Wards") = dtSelectedWard


        If checked Then
            For Each row As DataRow In dtWard.Rows
                Dim newrow As DataRow = dtSelectedWard.NewRow
                newrow("Checked") = False
                newrow("WardId") = row("WardId")
                newrow("WardName") = row("WardName")
                newrow("DistrictId") = row("DistrictId")
                dtSelectedWard.Rows.Add(newrow)
            Next
        Else

            Dim dr() As DataRow = dtSelectedWard.Select("DistrictId='" & DistrictId & "'")
            Dim row As DataRow = dtWard.NewRow
            row("DistrictId") = If(dr.Length = 0, 0, dr(0)("DistrictId"))

RemoveRow:
            For Each selectedRow In dtSelectedWard.Rows
                If selectedRow("DistrictId") = row("DistrictId") Then
                    RemoveVillage(selectedRow("WardId"))
                    dtSelectedWard.Rows.Remove(selectedRow)

                    GoTo RemoveRow
                End If
            Next
        End If
        ViewState("Wards") = dtSelectedWard
        gvWards.DataSource = dtSelectedWard
        gvWards.DataBind()

    End Sub
    Private Sub FillVillageGrid(ByVal WardId As Integer, ByVal checked As Boolean, Optional ByVal OnUncheck As Boolean = False)
        Dim dtVillages As DataTable = HF.GetAllVillage(WardId, False)
        Dim dtSelectedVillages As DataTable = DirectCast(ViewState("Villages"), DataTable)

        For Each dr As GridViewRow In gvVillage.Rows
            Dim chk As Boolean = DirectCast(gvVillage.Rows(dr.RowIndex).Cells(0).Controls(1), CheckBox).Checked
            Dim Catchment As Integer
            
            Dim VillageId As Integer = gvVillage.DataKeys(dr.RowIndex).Values("VillageID")
            Dim row As DataRow = dtSelectedVillages.Select("VillageId = " & VillageId & "")(0)
            row("Checked") = chk
            If DirectCast(gvVillage.Rows(dr.RowIndex).Cells(3).Controls(1), TextBox).Text <> "" Then
                Catchment = DirectCast(gvVillage.Rows(dr.RowIndex).Cells(3).Controls(1), TextBox).Text
                row("Catchment") = Catchment
            End If
        Next
        ViewState("Villages") = dtSelectedVillages
        Dim dt As DataTable = TryCast(gvWards.DataSource, DataTable)
        If checked Then
            For Each row As DataRow In dtVillages.Rows
                Dim newrow As DataRow = dtSelectedVillages.NewRow
                newrow("Checked") = False
                newrow("WardId") = row("WardId")
                newrow("VillageName") = row("VillageName")
                newrow("VillageId") = row("VillageId")
                dtSelectedVillages.Rows.Add(newrow)
            Next
        Else
            Dim dr() As DataRow = dtSelectedVillages.Select("WardId='" & WardId & "'")
            Dim row As DataRow = dtVillages.NewRow
            row("WardId") = If(dr.Length = 0, 0, dr(0)("WardId"))

RemoveRow:
            For Each selectedRow In dtSelectedVillages.Rows
                If selectedRow("WardId") = row("WardId") Then
                    dtSelectedVillages.Rows.Remove(selectedRow)
                    GoTo RemoveRow
                End If
            Next
        End If

        ViewState("Villages") = dtSelectedVillages
        gvVillage.DataSource = dtSelectedVillages
        gvVillage.DataBind()
    End Sub
    Private Sub RemoveVillage(ByVal WardId As Integer)
        Dim dtSelectedVillages As DataTable = DirectCast(ViewState("Villages"), DataTable)
        Dim dr() As DataRow = dtSelectedVillages.Select("WardId='" & WardId & "'")
        Dim row As DataRow = dtSelectedVillages.NewRow

        row("WardId") = If(dr.Length = 0, 0, dr(0)("WardId"))
      
RemoveRow:
        For Each selectedRow In dtSelectedVillages.Rows
            If selectedRow("WardId") = row("WardId") Then
                dtSelectedVillages.Rows.Remove(selectedRow)
                GoTo RemoveRow
            End If
        Next
        ViewState("Villages") = dtSelectedVillages
        gvVillage.DataSource = dtSelectedVillages
        gvVillage.DataBind()
    End Sub
    Private Sub RemoveWard(ByVal DistrictId As Integer)
        Dim dtSelectedWard As DataTable = DirectCast(ViewState("Wards"), DataTable)
        Dim dr() As DataRow = dtSelectedWard.Select("DistrictId='" & DistrictId & "'")
        Dim row As DataRow = dtSelectedWard.NewRow
        row("DistrictId") = If(dr.Length = 0, 0, dr(0)("DistrictId"))
RemoveRow:
        For Each selectedRow In dtSelectedWard.Rows
            If selectedRow("DistrictId") = row("DistrictId") Then
                RemoveVillage(selectedRow("WardId"))
                dtSelectedWard.Rows.Remove(selectedRow)
                GoTo RemoveRow
            End If
        Next
        ViewState("Wards") = dtSelectedWard
        gvWards.DataSource = dtSelectedWard
        gvWards.DataBind()
    End Sub
    Private Function getCatchmentDt() As DataTable
        Dim dtData As New DataTable
        dtData.Columns.Add("HFCatchmentId", GetType(Integer))
        dtData.Columns.Add("HFID", GetType(Integer))
        dtData.Columns.Add("LocationId", GetType(Integer))
        dtData.Columns.Add("Catchment", GetType(Integer))

        For Each row As GridViewRow In gvVillage.Rows
            Dim chkSelect As CheckBox = CType(gvVillage.Rows(row.RowIndex).Cells(0).Controls(1), CheckBox)
            If CBool(chkSelect.Checked) Then
                Dim HFCatchmentId As Integer? = If(gvVillage.DataKeys(gvVillage.Rows(row.RowIndex).RowIndex)("HFCatchmentId") Is DBNull.Value, Nothing, gvVillage.DataKeys(gvVillage.Rows(row.RowIndex).RowIndex)("HFCatchmentId"))
                Dim LocationId As Integer = gvVillage.DataKeys(gvVillage.Rows(row.RowIndex).RowIndex)("VillageID")
                Dim Catchment As Integer = 0
                If Not CType(row.Cells(3).Controls(1), TextBox).Text.Trim = String.Empty Then
                    Catchment = CType(row.Cells(3).Controls(1), TextBox).Text
                End If
                dtData.Rows.Add(New Object() {HFCatchmentId, eHF.HfID, LocationId, Catchment})
            End If
        Next
        Return dtData
    End Function
    Protected Sub FirstCellClicked(sender As Object, e As EventArgs)
        IsCheckBoxChecked = True
    End Sub

 
End Class
