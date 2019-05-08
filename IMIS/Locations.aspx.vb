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

Imports System.Web.Script.Serialization

Partial Public Class Locations
    Inherits System.Web.UI.Page
    Protected imisgen As New IMIS_Gen
    Private FamilyId As Integer = 0
    Private eInsuree As New IMIS_EN.tblInsuree
    Private eFamily As New IMIS_EN.tblFamilies
    Private Locations As New IMIS_BI.LocationsBI
    Private eDistricts As New IMIS_EN.tblLocations
    Private eWards As New IMIS_EN.tblLocations
    Private eVillages As New IMIS_EN.tblLocations
    Private _bClick As Boolean = True
    Private userBI As New IMIS_BI.UserBI
    Private eRegions As New IMIS_EN.tblLocations
    Private Gender As New IMIS_BI.GenderBI
    Public isOtherGenderUsed As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RunPageSecurity()
        isOtherGenderUsed = Gender.IsOtherGenderUSed()
        OtherGenderUsed()
        Try
            If IsPostBack = True Then
                If Request.Form("__EVENTARGUMENT").ToString = "Delete" Then

                    Select Case hfLocationType.Value
                        Case imisgen.getMessage("L_REGION")
                            DeleteRegion()
                        Case imisgen.getMessage("L_DISTRICT")
                            DeleteDistrict()
                        Case imisgen.getMessage("L_WARD")
                            DeleteWard()
                        Case imisgen.getMessage("L_VILLAGE")
                            DeleteVillage()
                    End Select


                End If
            End If
            FamilyId = HttpContext.Current.Request.QueryString("f")

            If IsPostBack = True Then Return

            LoadGrids(0)


        Catch ex As Exception
            Session("Msg") = imisgen.getMessage("M_ERRORMESSAGE")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

        End Try
    End Sub

    Private Sub hideOtherField()
        isOtherGenderUsed = Gender.IsOtherGenderUSed()
        txtOthers.Visible = isOtherGenderUsed
        lblOthers.Visible = isOtherGenderUsed
        gvVillages.Columns(4).Visible = isOtherGenderUsed
    End Sub

    Private Sub RunPageSecurity(Optional ByVal OnButtonEvent As Boolean = False, Optional ByVal cmd As String = "none")
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If Not OnButtonEvent Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Locations, Page) Then
                Dim AddLocations As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.AddLocations, UserID)
                Dim EditLocations As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.EditLocations, UserID)
                Dim DeleteLocations As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.DeleteLocations, UserID)
                Dim MoveLocations As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.MoveLocations, UserID)




                If Not DeleteLocations And Not EditLocations Then
                    pnlDistricts.Enabled = False
                    pnlWards.Enabled = False
                    pnlVillages.Enabled = False
                End If

                If Not AddLocations Then
                    btnAdd.Visible = False
                End If
                If Not EditLocations Then
                    btnEdit.Visible = False
                End If
                If Not DeleteLocations Then
                    btnDelete.Visible = False
                End If
                If Not MoveLocations Then
                    btnMoveLocation.Visible = False
                End If

                If btnAdd.Visible And pnlDistricts.Enabled And Not pnlWards.Enabled And Not pnlVillages.Enabled Then
                    btnSave.Visible = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Locations.ToString & "&retUrl=" & RefUrl)
            End If
        Else
            If cmd = "deletedistrict" Then
                If Not userBI.checkRights(IMIS_EN.Enums.Rights.DeleteLocations, UserID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Locations.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf cmd = "deletevillage" Then
                If Not userBI.checkRights(IMIS_EN.Enums.Rights.DeleteLocations, UserID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Locations.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf cmd = "deleteward" Then
                If Not userBI.checkRights(IMIS_EN.Enums.Rights.DeleteLocations, UserID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Locations.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf cmd = "EditDistrict" Then
                If Not userBI.checkRights(IMIS_EN.Enums.Rights.EditLocations, UserID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Locations.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf cmd = "EditWard" Then
                If Not userBI.checkRights(IMIS_EN.Enums.Rights.EditLocations, UserID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Locations.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf cmd = "EditVillage" Then
                If Not userBI.checkRights(IMIS_EN.Enums.Rights.EditLocations, UserID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Locations.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf cmd = "AddDistrict" Then
                If Not userBI.checkRights(IMIS_EN.Enums.Rights.AddLocations, UserID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Locations.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf cmd = "AddWard" Then
                If Not userBI.checkRights(IMIS_EN.Enums.Rights.AddLocations, UserID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Locations.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf cmd = "AddVillage" Then
                If Not userBI.checkRights(IMIS_EN.Enums.Rights.AddLocations, UserID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Locations.ToString & "&retUrl=" & RefUrl)
                End If
            End If
        End If
    End Sub

    Private Sub LoadGrids(ByVal LoadOption As Integer)



        Select Case LoadOption
            Case 0
                Dim dtRegions As DataTable = Locations.GetRegions(imisgen.getUserId(Session("User")))
                ViewState("dtRegions") = dtRegions
                ViewState("SelectedRegion") = gvRegions.SelectedValue
                gvRegions.DataSource = dtRegions
                gvRegions.SelectedIndex = 0
                gvRegions.DataBind()

                If dtRegions.Rows.Count > 0 Then
                    L_REGION.Text = Format(dtRegions.Rows.Count, "#,###") & " " & imisgen.getMessage("L_REGION") & "(s)"
                    Dim dtDistricts As DataTable = Locations.GetDistricts(imisgen.getUserId(Session("User")), RegionId:=gvRegions.SelectedValue)
                    ViewState("dtDistricts") = dtDistricts

                    L_DISTRICT.Text = Format(dtDistricts.Rows.Count, "#,###") & " " & imisgen.getMessage("L_DISTRICT") & "(s)"

                    gvDistricts.DataSource = dtDistricts
                    gvDistricts.SelectedIndex = 0
                    gvDistricts.DataBind()
                    If dtDistricts.Rows.Count > 0 Then
                        LoadGrids(2)
                    End If
                End If


            Case 1
                Dim dtDistricts As DataTable = Locations.GetDistricts(imisgen.getUserId(Session("User")), RegionId:=ViewState("SelectedRegion"))
                ViewState("dtDistricts") = dtDistricts

                gvDistricts.DataSource = dtDistricts
                gvDistricts.SelectedIndex = 0
                gvDistricts.DataBind()

                If dtDistricts.Rows.Count > 0 Then

                    L_DISTRICT.Text = Format(dtDistricts.Rows.Count, "#,###") & " " & imisgen.getMessage("L_DISTRICT") & "(s)"

                    Dim dtWards As DataTable = Locations.GetWards(gvDistricts.SelectedDataKey.Value, False)
                    ViewState("dtWards") = dtWards

                    L_WARD.Text = Format(dtWards.Rows.Count, "#,###") & " " & imisgen.getMessage("L_WARD") & "(s)"

                    gvWards.DataSource = dtWards
                    gvWards.SelectedIndex = 0
                    gvWards.DataBind()

                    If dtWards.Rows.Count > 0 Then
                        Dim dtVillages As DataTable = Locations.GetVillages(gvWards.SelectedDataKey.Value, False)
                        ViewState("dtVillages") = dtVillages

                        L_VILLAGE.Text = Format(dtVillages.Rows.Count, "#,###") & " " & imisgen.getMessage("L_VILLAGE") & "(s)"

                        gvVillages.DataSource = dtVillages
                        gvVillages.SelectedIndex = 0
                        gvVillages.DataBind()
                    Else
                        gvVillages.DataSource = Nothing
                        gvVillages.DataBind()
                    End If
                End If

            Case 2
                ViewState("SelectedDistrict") = gvDistricts.SelectedDataKey.Value
                Dim dtWards As DataTable = Locations.GetWards(ViewState("SelectedDistrict"), False) '(gvDistricts.SelectedDataKey.Value, False)
                ViewState("dtWards") = dtWards

                L_WARD.Text = Format(dtWards.Rows.Count, "#,###") & " " & imisgen.getMessage("L_WARD") & "(s)"

                gvWards.DataSource = dtWards
                gvWards.SelectedIndex = 0
                gvWards.DataBind()
                If dtWards.Rows.Count > 0 Then
                    LoadGrids(3)
                End If
            Case 3
                If gvWards.Rows.Count > 0 Then ViewState("SelectedWard") = gvWards.SelectedDataKey.Value
                Dim dtVillages As DataTable = Locations.GetVillages(ViewState("SelectedWard"), False) '(gvWards.SelectedDataKey.Value, False)
                ViewState("dtVillages") = dtVillages

                L_VILLAGE.Text = Format(dtVillages.Rows.Count, "#,###") & " " & imisgen.getMessage("L_VILLAGE") & "(s)"

                gvVillages.DataSource = dtVillages
                gvVillages.SelectedIndex = 0
                gvVillages.DataBind()
        End Select

    End Sub

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

    Private Sub gvRegions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRegions.SelectedIndexChanged

        ViewState("SelectedRegion") = gvRegions.SelectedDataKey.Value
        'If Not ViewState("dtWards") Is Nothing Then CType(ViewState("dtWards"), DataTable).Rows.Clear()
        'If Not ViewState("dtVillages") Is Nothing Then CType(ViewState("dtVillages"), DataTable).Rows.Clear()
        LoadGrids(1)
        If gvDistricts.Rows.Count > 0 Then
            ViewState("SelecteDistrict") = gvDistricts.DataKeys(0).Value
            LoadGrids(2)
        Else
            gvWards.DataSource = Nothing
            gvWards.DataBind()
            L_DISTRICT.Text = imisgen.getMessage("L_DISTRICT") & "(s)"
            L_WARD.Text = imisgen.getMessage("L_WARD") & "(s)"
            L_VILLAGE.Text = imisgen.getMessage("L_VILLAGE") & "(s)"
        End If
        If gvWards.Rows.Count > 0 Then
            ViewState("SelectedWard") = gvWards.DataKeys(0).Value
            LoadGrids(3)
        Else
            gvVillages.DataSource = Nothing
            gvVillages.DataBind()
        End If


        hfLocationType.Value = imisgen.getMessage("L_REGION")
    End Sub
    Private Sub gvDistricts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDistricts.SelectedIndexChanged
        ViewState("SelectedDistrict") = gvDistricts.SelectedDataKey.Value
        'If Not ViewState("dtWards") Is Nothing Then CType(ViewState("dtWards"), DataTable).Rows.Clear()
        'If Not ViewState("dtVillages") Is Nothing Then CType(ViewState("dtVillages"), DataTable).Rows.Clear()
        LoadGrids(2)
        If gvWards.Rows.Count > 0 Then
            ViewState("SelectedWard") = gvWards.DataKeys(0).Value
            LoadGrids(3)
        Else

            gvVillages.DataSource = Nothing
            L_WARD.Text = imisgen.getMessage("L_WARD") & "(s)"
            L_VILLAGE.Text = imisgen.getMessage("L_VILLAGE") & "(s)"
            gvVillages.DataBind()
        End If

        hfLocationType.Value = imisgen.getMessage("L_DISTRICT")

        'gvWards.DataSource = Locations.GetWards(gvDistricts.SelectedDataKey.Value, False)
        'gvWards.SelectedIndex = 0
        'gvWards.DataBind()
    End Sub
    Private Sub gvWards_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWards.SelectedIndexChanged
        ViewState("SelectedWard") = gvWards.SelectedDataKey.Value
        'If Not ViewState("dtVillages") Is Nothing Then CType(ViewState("dtVillages"), DataTable).Rows.Clear()
        LoadGrids(3)
        hfLocationType.Value = imisgen.getMessage("L_WARD")
        'gvVillages.DataSource = Locations.GetVillages(gvWards.SelectedDataKey.Value, False)
        'gvVillages.SelectedIndex = 0
        'gvVillages.DataBind()
    End Sub
    Private Sub gvVillage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvVillages.SelectedIndexChanged
        hfLocationType.Value = imisgen.getMessage("L_VILLAGE")
    End Sub

    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub

    Public Sub SaveRegion(ByVal LocationName As String, ByVal LocationCode As String)
        Dim Mode = hfMode.Value 'B_ADD
        If Mode = imisgen.getMessage("B_EDIT") Then
            RunPageSecurity(True, cmd:="EditRegion")
            If Not gvRegions.SelectedDataKey Is Nothing Then
                eRegions.LocationId = gvRegions.SelectedDataKey.Value
            End If
        ElseIf Mode = imisgen.getMessage("B_ADD") Then
            RunPageSecurity(True, cmd:="AddRegion")
            eRegions.LocationId = 0
        End If
        eRegions.LocationName = LocationName
        eRegions.LocationCode = LocationCode
        eRegions.LocationType = "R"
        eRegions.AuditUserId = imisgen.getUserId(Session("User"))


        Locations.SaveLocation(eRegions)
        lblMsg.Text = imisgen.getMessage("M_REGIONSAVED")
        gvRegions.DataSource = Nothing
        gvRegions.DataBind()

        LoadGrids(0)

        _bClick = False

    End Sub


    Public Sub SaveDistrict(ByVal LocationName As String, ByVal LocationCode As String)

        Dim Mode = hfMode.Value 'B_ADD

        If Mode = imisgen.getMessage("B_EDIT") Then
            RunPageSecurity(True, cmd:="EditDistrict")
            If Not gvDistricts.SelectedDataKey Is Nothing Then
                eDistricts.LocationId = gvDistricts.SelectedDataKey.Value
            End If
        ElseIf Mode = imisgen.getMessage("B_ADD") Then
            RunPageSecurity(True, cmd:="AddDistrict")
            eDistricts.LocationId = 0
        End If
        eDistricts.LocationName = LocationName
        eDistricts.LocationCode = LocationCode
        eDistricts.AuditUserId = imisgen.getUserId(Session("User"))
        If Not gvRegions.SelectedDataKey Is Nothing Then
            eDistricts.ParentLocationId = gvRegions.SelectedDataKey.Value
        Else
            imisgen.Alert(imisgen.getMessage("NOREGIONSPECIFIED"), pnlButtons, alertPopupTitle:="IMIS")
        End If
        eDistricts.LocationType = "D"
        Locations.SaveLocation(eDistricts)
        lblMsg.Text = imisgen.getMessage("M_DISTRICTSAVED")
        gvDistricts.DataSource = Nothing
        gvDistricts.DataBind()

        LoadGrids(1)

        _bClick = False

    End Sub

    Public Sub SaveWard(ByVal LocationName As String, ByVal LocationCode As String)

        Dim Mode = hfMode.Value

        If Mode = imisgen.getMessage("B_EDIT") Then
            RunPageSecurity(True, cmd:="EditWard")
            If Not gvWards.SelectedDataKey Is Nothing Then
                eWards.LocationId = gvWards.SelectedDataKey.Value
            End If
        ElseIf Mode = imisgen.getMessage("B_ADD") Then
            RunPageSecurity(True, cmd:="AddWard")
            eWards.LocationId = 0
        End If

        eWards.LocationName = LocationName
        eWards.LocationCode = LocationCode
        eWards.AuditUserId = imisgen.getUserId(Session("User"))
        If Not gvDistricts.SelectedDataKey Is Nothing Then
            eWards.ParentLocationId = gvDistricts.SelectedDataKey.Value
        Else
            imisgen.Alert(imisgen.getMessage("NOVILLAGESPECIFIED"), pnlButtons, alertPopupTitle:="IMIS")
        End If
        eWards.LocationType = "W"

        Locations.SaveLocation(eWards)
        lblMsg.Text = imisgen.getMessage("M_WARDSAVED")
        gvWards.DataSource = Nothing
        gvWards.DataBind()

        LoadGrids(2)
        LoadGrids(3)
        _bClick = False

    End Sub

    Public Sub SaveVillage(ByVal LocationName As String, ByVal LocationCode As String)

        Dim Mode = hfMode.Value

        If Mode = imisgen.getMessage("B_EDIT") Then
            RunPageSecurity(True, cmd:="EditVillage")
            If Not gvVillages.SelectedDataKey Is Nothing Then
                eVillages.LocationId = gvVillages.SelectedDataKey.Value
            End If
        ElseIf Mode = imisgen.getMessage("B_ADD") Then
            RunPageSecurity(True, cmd:="AddVillage")
            eVillages.LocationId = 0
        End If

        eVillages.LocationName = LocationName
        eVillages.LocationCode = LocationCode
        If txtMale.Text.Trim.Length > 0 Then eVillages.MalePopulation = txtMale.Text
        If txtFemale.Text.Trim.Length > 0 Then eVillages.FemalePopulation = txtFemale.Text
        If txtFamily.Text.Trim.Length > 0 Then eVillages.Families = txtFamily.Text
        If txtOthers.Text.Trim.Length > 0 Then eVillages.OtherPopulation = txtOthers.Text
        eVillages.AuditUserId = imisgen.getUserId(Session("User"))
        If Not gvWards.SelectedDataKey Is Nothing Then
            eVillages.ParentLocationId = gvWards.SelectedDataKey.Value
        Else
            imisgen.Alert(imisgen.getMessage("M_NOWARDSPECIFIED"), pnlButtons, alertPopupTitle:="IMIS")
            Return
        End If

        eVillages.LocationType = "V"

        Locations.SaveLocation(eVillages)
        lblMsg.Text = imisgen.getMessage("M_VILLAGESAVED")
        gvVillages.DataSource = Nothing
        gvVillages.DataBind()

        LoadGrids(3)

        _bClick = False

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim LocationType = hfLocationType.Value
        Dim LocationName = txtLocationName.Text
        Dim LocationCode = txtLocationCode.Text
        Dim GridLocCode = hfLocationCode.Value
        If GridLocCode <> LocationCode Then
            If Locations.IsLocCodeUnique(LocationCode) Then
                imisgen.Alert(imisgen.getMessage("M_UNIQUECODELOCATION"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If


        If (LocationCode = String.Empty) Then
            imisgen.Alert(imisgen.getMessage("M_EMPTYLOCATIONCODE"), pnlButtons, alertPopupTitle:="IMIS")
            Return
        End If
        If (LocationName = String.Empty) Then
            imisgen.Alert(imisgen.getMessage("M_EMPTYLOCATIONNAME"), pnlButtons, alertPopupTitle:="IMIS")
            Return
        End If

        If (LocationType = imisgen.getMessage("L_REGION")) Then
            SaveRegion(LocationName, LocationCode)
        ElseIf (LocationType = imisgen.getMessage("L_DISTRICT")) Then
            SaveDistrict(LocationName, LocationCode)
        ElseIf (LocationType = imisgen.getMessage("L_WARD")) Then
            SaveWard(LocationName, LocationCode)
        ElseIf (LocationType = imisgen.getMessage("L_VILLAGE")) Then

            SaveVillage(LocationName, LocationCode)
        End If

    End Sub

    Private Sub DeleteRegion()
        RunPageSecurity(True, "deletedRegion")
        Try

            If Not gvRegions.SelectedDataKey Is Nothing Then
                eRegions.LocationId = gvRegions.SelectedDataKey.Value
            Else
                lblMsg.Text = imisgen.getMessage("M_NORECORDSELECTED")
                Return
            End If
            If gvDistricts.Rows.Count > 0 Then
                lblMsg.Text = imisgen.getMessage("M_DISTRICTINREGION")
                Return
            End If
            eRegions.AuditUserId = imisgen.getUserId(Session("User"))


            Dim chk As Integer = Locations.DeleteLocation(eRegions)

            If chk = 3 Then
                Throw New Exception()
            ElseIf chk = 2 Then
                lblMsg.Text = imisgen.getMessage("L_REGION") & " " & imisgen.getMessage("M_DELETED")
            ElseIf chk = 1 Then
                lblMsg.Text = imisgen.getMessage("L_REGION") & " " & imisgen.getMessage("M_INUSE")
                Return
            End If

            gvRegions.DataSource = Nothing
            gvRegions.DataBind()

            LoadGrids(0)

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 2)
            Throw New Exception(ex.Message)
        End Try

    End Sub
    Private Sub DeleteDistrict()
        RunPageSecurity(True, "deletedistrict")
        Try

            If Not gvDistricts.SelectedDataKey Is Nothing Then
                eDistricts.LocationId = gvDistricts.SelectedDataKey.Value
            Else
                lblMsg.Text = imisgen.getMessage("M_NORECORDSELECTED")
                Return
            End If
            If gvWards.Rows.Count > 0 Then
                lblMsg.Text = imisgen.getMessage("M_WARDSINDISTRICT")
                Return
            End If
            eDistricts.AuditUserId = imisgen.getUserId(Session("User"))


            Dim chk As Integer = Locations.DeleteLocation(eDistricts)

            If chk = 3 Then
                Throw New Exception()
            ElseIf chk = 2 Then
                lblMsg.Text = imisgen.getMessage("L_DISTRICT") & " " & imisgen.getMessage("M_DELETED")
            ElseIf chk = 1 Then
                lblMsg.Text = imisgen.getMessage("L_DISTRICT") & " " & imisgen.getMessage("M_INUSE")
                Return
            End If

            gvDistricts.DataSource = Nothing
            gvDistricts.DataBind()

            LoadGrids(0)
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 2)
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub DeleteWard()


        RunPageSecurity(True, "deleteward")

        Try
            If Not gvWards.SelectedDataKey Is Nothing Then
                eWards.LocationId = gvWards.SelectedDataKey.Value
            Else
                eWards.LocationId = 0
            End If
            If gvVillages.Rows.Count > 0 Then
                lblMsg.Text = imisgen.getMessage("M_VILLAGESINWARD")
                Return
            End If
            eWards.AuditUserId = imisgen.getUserId(Session("User"))


            Dim chk As Integer = Locations.DeleteLocation(eWards)
            If chk = 3 Then
                Throw New Exception()
            ElseIf chk = 2 Then
                lblMsg.Text = imisgen.getMessage("L_WARD") & " " & imisgen.getMessage("M_DELETED")
            ElseIf chk = 1 Then
                lblMsg.Text = imisgen.getMessage("L_WARD") & " " & imisgen.getMessage("M_INUSE")
                Return
            End If

            gvWards.DataSource = Nothing
            gvWards.DataBind()

            LoadGrids(0)

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 2)
            Throw New Exception(ex.Message)
            Return
        End Try
    End Sub

    Private Sub DeleteVillage()

        RunPageSecurity(True, "deletevillage")

        Try

            If Not gvVillages.SelectedDataKey Is Nothing Then
                eVillages.LocationId = gvVillages.SelectedDataKey.Value
            Else
                eVillages.LocationId = 0
            End If

            eVillages.AuditUserId = imisgen.getUserId(Session("User"))

            Dim chk As Integer = Locations.DeleteLocation(eVillages)
            If chk = 3 Then
                Throw New Exception()
            ElseIf chk = 2 Then
                lblMsg.Text = imisgen.getMessage("L_VILLAGE") & " " & imisgen.getMessage("M_DELETED")
            ElseIf chk = 1 Then
                lblMsg.Text = imisgen.getMessage("L_VILLAGE") & " " & imisgen.getMessage("M_INUSE")
                Return
            End If

            gvVillages.DataSource = Nothing
            gvVillages.DataBind()

            LoadGrids(3)

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            'Return
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub btnMoveLocation_Click(sender As Object, e As EventArgs) Handles btnMoveLocation.Click
        Response.Redirect("MoveLocation.aspx")
    End Sub

    Protected Function OtherGenderUsed() As Boolean
        gvVillages.Columns(4).Visible = isOtherGenderUsed
        Dim jSerializer As New JavaScriptSerializer
        Return jSerializer.Serialize(isOtherGenderUsed)
    End Function

End Class
