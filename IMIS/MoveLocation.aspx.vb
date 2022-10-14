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

Public Class MoveLocation
    Inherits System.Web.UI.Page
    Protected imisgen As New IMIS_Gen
    Private PricelistsItems As New IMIS_BL.PriceListMIBL
    Private PricelistsServices As New IMIS_BL.PricelistMSBL
    Private MoveLocationBI As New IMIS_BI.MoveLocationBI
    Private AffectedFamilies As Integer = 0
    Private DistrictId As Integer
    Private RegionId As Integer
    Private AuditUserId As Integer

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvRegions)
        AddRowSelectToGridView(gvDistricts)
        AddRowSelectToGridView(gvWards)
        AddRowSelectToGridView(gvVillages)
        MyBase.Render(writer)
    End Sub

    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If row.Cells(0).Controls.Count > 1 Then
                If CType(row.Cells(0).Controls(3), TextBox).Visible = False Then
                    row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
                End If
            Else
                row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
            End If

        Next

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblMsg.Text = ""
            AuditUserId = imisgen.getUserId(Session("User"))
            If IsPostBack = True Then
                If Request.Params.Get("__EVENTARGUMENT").ToString = "MoveDelete" Then
                    MoveDistrict()
                End If
                Return
            End If
            'If IsPostBack = True Then Return
            GetRegions()
            LoadGrids(0)
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub B_CANCEL_Click(sender As Object, e As EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Locations.aspx")
    End Sub
    'Index Changed on gridview
    Private Sub gvRegions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRegions.SelectedIndexChanged
        ViewState("SelectedRegion") = gvRegions.SelectedDataKey.Values("RegionId")
        ViewState("SelectedRegionName") = gvRegions.SelectedDataKey.Values("RegionName")
    End Sub
    Private Sub gvDistricts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDistricts.SelectedIndexChanged
        ViewState("SelectedDistrict") = gvDistricts.SelectedDataKey.Values("DistrictId")
        ViewState("SelectedDistrictName") = gvDistricts.SelectedDataKey.Values("DistrictName")
    End Sub
    Private Sub gvWards_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWards.SelectedIndexChanged
        ViewState("SelectedWard") = gvWards.SelectedDataKey.Values("WardId")
        ViewState("SelectedWardName") = gvWards.SelectedDataKey.Values("WardName")
    End Sub
    Private Sub gvVillage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvVillages.SelectedIndexChanged
        ViewState("SelectedVillage") = gvVillages.SelectedDataKey.Values("VillageId")
        ViewState("SelectedVillageName") = gvVillages.SelectedDataKey.Values("VillageName")
    End Sub

    '---- Fill Ward grid from dropdown----------'
    Private Sub ddlVRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVRegion.SelectedIndexChanged
        Try
            If ddlVRegion.SelectedValue > 0 Then
                GetVDistrict(ddlVRegion.SelectedValue)
                DistrictId = -1

                GetWard()
            End If
            gvVillages.DataSource = Nothing
            gvVillages.DataBind()

        Catch ex As Exception
            lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE").ToString
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub ddlVDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVDistrict.SelectedIndexChanged
        Try
            If ddlVDistrict.SelectedValue > 0 Then GetWard()

            gvVillages.DataSource = Nothing
            gvVillages.DataBind()
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub ddlVWard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVWard.SelectedIndexChanged
        Try
            If ddlVWard.SelectedValue > 0 Then LoadGrids(3)
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    '-----Fill Municipality  grid from dropdown-
    Private Sub ddlWRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWRegion.SelectedIndexChanged
        Try
            If ddlWRegion.SelectedValue > 0 Then GetWDistrict(ddlWRegion.SelectedValue)
            gvWards.DataSource = Nothing
            gvWards.DataBind()
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub ddlWDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlWDistrict.SelectedIndexChanged
        Try
            If ddlWDistrict.SelectedValue > 0 Then LoadGrids(2)
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    '---Fill District grid from Drop down

    Private Sub ddlDRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDRegion.SelectedIndexChanged
        Try
            If ddlDRegion.SelectedValue Then LoadGrids(1)
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub


    '--- Move Locations Button
    Private Sub ImgBtnMoveVillage_Click(sender As Object, e As ImageClickEventArgs) Handles ImgBtnMoveVillage.Click
        Dim ErrorMessage As Integer = 0
        Dim Msg As String = ""
        If ViewState("SelectedWard") Is Nothing AndAlso ViewState("SelectedVillage") Is Nothing Then
            Msg = imisgen.getMessage("M_PLEASESELECT") & " " & imisgen.getMessage("L_VILLAGE") & " " & imisgen.getMessage("m_AND") & " " & imisgen.getMessage("L_WARD")
            lblMsg.Text = Msg
            Exit Sub
        ElseIf ViewState("SelectedWard") = ddlVWard.SelectedValue Then
            Msg = imisgen.getMessage("L_VILLAGE") & " " & ViewState("SelectedVillageName") & " " & imisgen.getMessage("M_BELONGSTO") & " " & imisgen.getMessage("L_WARD") & " " & ViewState("SelectedWardName")
            lblMsg.Text = Msg
            Exit Sub
        ElseIf ViewState("SelectedWard") Is Nothing Then
            Msg = imisgen.getMessage("V_WARD")
            lblMsg.Text = Msg
            Exit Sub
        ElseIf ViewState("SelectedVillage") Is Nothing Then
            Msg = imisgen.getMessage("V_VILLAGE")
            lblMsg.Text = Msg
            Exit Sub
        End If
        MoveLocationBI.MoveLocation(ViewState("SelectedVillage"), ViewState("SelectedWard"), "V", AffectedFamilies, ErrorMessage, AuditUserId)

        Select Case ErrorMessage
            Case 0  'Success 
                Msg = imisgen.getMessage("L_VILLAGE") & " " & ViewState("SelectedVillageName") & " " & imisgen.getMessage("M_SUCCESSFULLYMOVED") & " " & imisgen.getMessage("L_WARD") & " " & ViewState("SelectedWardName")
            Case 1  'Invalid Location type
                Msg = imisgen.getMessage("M_INVALIDLOCATIONTYPE")
            Case 2  'Already belongs to Destination location
                Msg = imisgen.getMessage("L_VILLAGE") & " " & ViewState("SelectedVillageName") & " " & imisgen.getMessage("M_BELONGSTO") & " " & imisgen.getMessage("L_WARD") & " " & ViewState("SelectedWard")
            Case -1 'Unknown Error
                Msg = imisgen.getMessage("M_ERRORMESSAGE")
        End Select
        LoadGrids(2)
        LoadGrids(3)
        lblMsg.Text = Msg
    End Sub
    Private Sub ImgbtnMoveWard_Click(sender As Object, e As ImageClickEventArgs) Handles ImgbtnMoveWard.Click
        Dim ErrorMessage As Integer = 0
        Dim Msg As String = ""
        If ViewState("SelectedWard") Is Nothing AndAlso ViewState("SelectedDistrict") Is Nothing Then
            Msg = imisgen.getMessage("M_PLEASESELECT") & " " & imisgen.getMessage("L_WARD") & " " & imisgen.getMessage("m_AND") & " " & imisgen.getMessage("L_DISTRICT")
            lblMsg.Text = Msg
            Exit Sub
        ElseIf ViewState("SelectedDistrict") = ddlWDistrict.SelectedValue Then
            Msg = imisgen.getMessage("L_WARD") & " " & ViewState("SelectedWardName") & " " & imisgen.getMessage("M_BELONGSTO") & " " & imisgen.getMessage("L_DISTRICT") & " " & ViewState("SelectedDistrictName")
            lblMsg.Text = Msg
            Exit Sub
        ElseIf ViewState("SelectedWard") Is Nothing Then
            Msg = imisgen.getMessage("V_WARD")
            lblMsg.Text = Msg
            Exit Sub
        ElseIf ViewState("SelectedDistrict") Is Nothing Then
            Msg = imisgen.getMessage("M_PLEASESELECTADISTRICT")
            lblMsg.Text = Msg
            Exit Sub
        End If
        MoveLocationBI.MoveLocation(ViewState("SelectedWard"), ViewState("SelectedDistrict"), "W", AffectedFamilies, ErrorMessage, AuditUserId)

        Select Case ErrorMessage
            Case 0  'Success 
                Msg = imisgen.getMessage("L_WARD") & " " & ViewState("SelectedWardName") & " " & imisgen.getMessage("M_SUCCESSFULLYMOVED") & " " & imisgen.getMessage("L_DISTRICT") & " " & ViewState("SelectedDistrictName")
            Case 1  'Invalid Location type
                Msg = imisgen.getMessage("M_INVALIDLOCATIONTYPE")
            Case 2  'Already belongs to Destination location
                Msg = imisgen.getMessage("L_WARD") & " " & ViewState("SelectedWardName") & " " & imisgen.getMessage("M_BELONGSTO") & " " & imisgen.getMessage("L_DISTRICT") & " " & ViewState("SelectedDistrictName")
            Case -1 'Unknown Error
                Msg = imisgen.getMessage("M_ERRORMESSAGE")
        End Select
        lblMsg.Text = Msg
        LoadGrids(1)
        LoadGrids(2)
    End Sub

    Private Function isDistrictsPricelistConflict() As Boolean
        Dim temp = gvDistricts.SelectedDataKey.Values("DistrictName")
        Dim dtPricelistsItems As DataTable = PricelistsItems.GetPriceListDistrictMI(imisgen.getUserId(Session("User")), gvDistricts.SelectedDataKey.Values("DistrictName"), False)
        Dim dtPricelistsServices As DataTable = PricelistsServices.GetPriceListDistrictMS(imisgen.getUserId(Session("User")), gvDistricts.SelectedDataKey.Values("DistrictName"), False)

        If dtPricelistsItems.Rows.Count > 0 Or dtPricelistsServices.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function

    Private Function removeDistrictPricelist() As Boolean
        If isDistrictsPricelistConflict() = True Then
            PricelistsServices.DetachPriceListDistrictMS(imisgen.getUserId(Session("User")), gvDistricts.SelectedDataKey.Values("DistrictName"))
            PricelistsItems.DetachPriceListDistrictMI(imisgen.getUserId(Session("User")), gvDistricts.SelectedDataKey.Values("DistrictName"))
        End If
    End Function
    Private Sub MoveDistrict() Handles ImgBtnMoveDistrict.Click
        Dim ErrorMessage As Integer = 0
        Dim Msg As String = ""
        removeDistrictPricelist()

        If ViewState("SelectedRegion") Is Nothing AndAlso ViewState("SelectedDistrict") Is Nothing Then
            Msg = imisgen.getMessage("M_PLEASESELECT") & " " & imisgen.getMessage("L_DISTRICT") & " " & imisgen.getMessage("m_AND") & " " & imisgen.getMessage("L_REGION")
            lblMsg.Text = Msg
            Exit Sub
        ElseIf ViewState("SelectedRegion") = ddlDRegion.SelectedValue Then
            Msg = imisgen.getMessage("L_DISTRICT") & " " & ViewState("SelectedDistrictName") & " " & imisgen.getMessage("M_BELONGSTO") & " " & imisgen.getMessage("L_REGION") & " " & ViewState("SelectedRegionName")
            lblMsg.Text = Msg
            Exit Sub
        ElseIf ViewState("SelectedRegion") Is Nothing Then
            Msg = imisgen.getMessage("M_PLEASESELECTAREGION")
            lblMsg.Text = Msg
            Exit Sub
        ElseIf ViewState("SelectedDistrict") Is Nothing Then
            Msg = imisgen.getMessage("M_PLEASESELECTADISTRICT")
            lblMsg.Text = Msg
        End If

        MoveLocationBI.MoveLocation(ViewState("SelectedDistrict"), ViewState("SelectedRegion"), "D", AffectedFamilies, ErrorMessage, AuditUserId)

        Select Case ErrorMessage
            Case 0  'Success 
                Msg = imisgen.getMessage("L_DISTRICT") & " " & ViewState("SelectedDistrictName") & " " & imisgen.getMessage("M_SUCCESSFULLYMOVED") & " " & imisgen.getMessage("L_REGION") & " " & ViewState("SelectedRegionName")
            Case 1  'Invalid Location type
                Msg = imisgen.getMessage("M_INVALIDLOCATIONTYPE")
            Case 2  'Already belongs to Destination location
                Msg = imisgen.getMessage("L_DISTRICT") & " " & ViewState("SelectedDistrictName") & imisgen.getMessage("M_BELONGSTO") & " " & imisgen.getMessage("L_REGION") & " " & ViewState("SelectedRegionName")
            Case -1 'Unknown Error
                Msg = imisgen.getMessage("M_ERRORMESSAGE")
        End Select
        lblMsg.Text = Msg
        LoadGrids(1)
        LoadGrids(0)
    End Sub
    'Fill Location Dropdowns
    Private Sub GetRegions()
        Dim dtRegions As DataTable = MoveLocationBI.GetRegions(imisgen.getUserId(Session("User")), ShowSelect:=True)
        'Select Region to get Village
        ddlVRegion.DataSource = dtRegions
        ddlVRegion.DataValueField = "RegionId"
        ddlVRegion.DataTextField = "RegionName"
        ddlVRegion.DataBind()

        'Select Region to get Ward
        ddlWRegion.DataSource = dtRegions
        ddlWRegion.DataValueField = "RegionId"
        ddlWRegion.DataTextField = "RegionName"
        ddlWRegion.DataBind()

        'Select Region to get District
        ddlDRegion.DataSource = dtRegions
        ddlDRegion.DataValueField = "RegionId"
        ddlDRegion.DataTextField = "RegionName"
        ddlDRegion.DataBind()
        If dtRegions.Rows.Count = 1 Then
            GetDistrict()
            GetVDistrict(Val(ddlVRegion.SelectedValue))
            GetWDistrict(Val(ddlWRegion.SelectedValue))
            LoadGrids(1)
        End If
    End Sub
    Private Sub GetDistrict()
        'Select District to get Ward
        Dim dtDistricts As DataTable = MoveLocationBI.GetDistricts(imisgen.getUserId(Session("User")), True)
        If dtDistricts.Rows.Count = 1 Then
            Dim dr As DataRow = dtDistricts.NewRow
            dr("DistrictId") = 0
            dr("DistrictName") = imisgen.getMessage("T_SELECTDISTRICT")
            dtDistricts.Rows.InsertAt(dr, 0)
        End If

        ddlWDistrict.DataSource = dtDistricts
        ddlWDistrict.DataValueField = "DistrictId"
        ddlWDistrict.DataTextField = "DistrictName"
        ddlWDistrict.DataBind()

    End Sub
    Private Sub GetVDistrict(RegionId As Integer)
        'Select District to get Village
        Dim dtDistricts As DataTable = MoveLocationBI.GetDistricts(imisgen.getUserId(Session("User")), True, RegionId)
        If dtDistricts.Rows.Count = 1 Then
            Dim dr As DataRow = dtDistricts.NewRow
            dr("DistrictId") = 0
            dr("DistrictName") = imisgen.getMessage("T_SELECTDISTRICT")
            dtDistricts.Rows.InsertAt(dr, 0)
        End If

        ddlVDistrict.DataSource = dtDistricts
        ddlVDistrict.DataValueField = "DistrictId"
        ddlVDistrict.DataTextField = "DistrictName"
        ddlVDistrict.DataBind()
    End Sub
    Private Sub GetWDistrict(RegionId As Integer)
        Dim dtDistricts As DataTable = MoveLocationBI.GetDistricts(imisgen.getUserId(Session("User")), True, RegionId:=RegionId)
        'Select District to get Ward
        If dtDistricts.Rows.Count = 1 Then
            Dim dr As DataRow = dtDistricts.NewRow
            dr("DistrictId") = 0
            dr("DistrictName") = imisgen.getMessage("T_SELECTDISTRICT")
            dtDistricts.Rows.InsertAt(dr, 0)
        End If
        ddlWDistrict.DataSource = dtDistricts
        ddlWDistrict.DataValueField = "DistrictId"
        ddlWDistrict.DataTextField = "DistrictName"
        ddlWDistrict.DataBind()
    End Sub
    Private Sub GetWard()
        If ddlVDistrict.SelectedValue <> "" AndAlso Val(ddlVDistrict.SelectedValue) > 0 Then DistrictId = ddlVDistrict.SelectedValue
        Dim dtWards As DataTable = MoveLocationBI.GetWards(DistrictId, True)
        If dtWards.Rows.Count = 1 Then
            Dim dr As DataRow = dtWards.NewRow
            dr("WardId") = 0
            dr("WardName") = imisgen.getMessage("T_SELECTAWARD")
            dtWards.Rows.InsertAt(dr, 0)
        End If
        ddlVWard.DataSource = dtWards
        ddlVWard.DataValueField = "WardId"
        ddlVWard.DataTextField = "WardName"
        ddlVWard.DataBind()
    End Sub

    Private Sub LoadGrids(ByVal LoadOption As Integer)
        Select Case LoadOption
            Case 0
                Dim dtRegions As DataTable = MoveLocationBI.GetRegions(imisgen.getUserId(Session("User")), False)
                gvRegions.DataSource = dtRegions
                gvRegions.SelectedIndex = 0
                gvRegions.DataBind()

            Case 1
                Dim dtDistricts As DataTable = MoveLocationBI.GetDistricts(imisgen.getUserId(Session("User")), showSelect:=False, RegionId:=ddlDRegion.SelectedValue)


                gvDistricts.DataSource = dtDistricts
                gvDistricts.SelectedIndex = 0
                gvDistricts.DataBind()

            Case 2

                Dim dtWards As DataTable = MoveLocationBI.GetWards(ddlWDistrict.SelectedValue, False) '(gvDistricts.SelectedDataKey.Value, False)

                gvWards.DataSource = dtWards
                gvWards.SelectedIndex = 0
                gvWards.DataBind()
            Case 3
                Dim dt As DataTable = MoveLocationBI.GetVillages(ddlVWard.SelectedValue, False) '(gvWards.SelectedDataKey.Value, False)
                Dim dtVillages As DataTable = MoveLocationBI.GetVillages(ddlVWard.SelectedValue, False) '(gvWards.SelectedDataKey.Value, False)

                gvVillages.DataSource = dtVillages
                gvVillages.SelectedIndex = 0
                gvVillages.DataBind()
        End Select

    End Sub



End Class
