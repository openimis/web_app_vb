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

Partial Public Class ProcessRelIndex
    Inherits System.Web.UI.Page
    Private process As New IMIS_BI.ProcessBatchesBI
    Private modularBatchProcess As New IMIS_BI.ModularBatchProcess
    Private eRelIndex As New IMIS_EN.tblRelIndex
    Protected imisgen As New IMIS_Gen
    Private ProcessControls As New Dictionary(Of String, String)
    Private FilterControls As New Dictionary(Of String, String)
    Private PreviewControls As New Dictionary(Of String, String)
    Private userBI As New IMIS_BI.UserBI


    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvRelIndex)
        MyBase.Render(writer)
    End Sub
    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.ProcessBatches, Page) Then
            btnProcess.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.BatchProcess, UserID)
            btnFilter.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.BatchFilter, UserID)
            btnPreview.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.BatchPreview, UserID)

            If Not btnProcess.Enabled And Not btnFilter.Enabled Then
                pnlBody.Enabled = False
            End If
            If Not btnProcess.Enabled Then
                pnlTop.Enabled = False
            End If
            If Not btnFilter.Enabled Then
                pnlMiddle.Enabled = False
            End If

        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.ProcessBatches.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub
    Private Sub RegisterControlsForCaching()
        ProcessControls.Add("ddlDistrictsBatch", ddlDistrictsBatch.SelectedValue)
        ProcessControls.Add("ddlMonthProcess", ddlMonthProcess.SelectedValue)
        ProcessControls.Add("ddlYearProcess", ddlYearProcess.SelectedValue)

        FilterControls.Add("ddlPeriod", ddlPeriod.SelectedValue)
        FilterControls.Add("ddlYearFilter", ddlYearFilter.SelectedValue)
        FilterControls.Add("ddlMonthFilter", ddlMonthFilter.SelectedValue)
        FilterControls.Add("ddlDistrictFilter", ddlDistrictFilter.SelectedValue)
        FilterControls.Add("ddlProductFilter", ddlProductFilter.SelectedValue)
        FilterControls.Add("ddlHFLevelFilter", ddlHFLevelFilter.SelectedValue)

        PreviewControls.Add("rbHF", rbHF.Checked.ToString())
        PreviewControls.Add("rbCapitation", rbCapitation.Checked.ToString())
        PreviewControls.Add("rbProduct", rbProduct.Checked.ToString())
        PreviewControls.Add("ddlDistrictACC", ddlDistrictACC.SelectedValue)
        PreviewControls.Add("ddlProductAAC", ddlProductAAC.SelectedValue)
        PreviewControls.Add("ddlHF", ddlHF.SelectedValue)
        PreviewControls.Add("ddlHFLevel", ddlHFLevel.SelectedValue)
        PreviewControls.Add("ddlBatchAAC", ddlBatchAAC.SelectedValue)
        PreviewControls.Add("txtSTARTData", txtSTARTData.Text)
        PreviewControls.Add("txtENDData", txtENDData.Text)
        PreviewControls.Add("chkClaims", chkClaims.Checked)

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.Form("__EVENTTARGET") = btnProcess.ClientID Then
            btnProcess_Click(sender, New System.EventArgs)
        End If
        RegisterControlsForCaching()
        If IsPostBack Then Return
        RunPageSecurity()
        Try

            FillDropdown()
            loadNextMonth()
            ReselectCachedCriteria()
            MonthFilterBind(ddlPeriod.SelectedValue)
        Catch ex As Exception
            Session("Msg") = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub ddlDistrictsBatch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrictsBatch.SelectedIndexChanged
        loadNextMonth()
    End Sub
    Private Sub loadNextMonth()
        If Not Val(ddlDistrictsBatch.SelectedValue) = 0 Then
            Dim dt As DataTable = process.GetBatchRunDate(Val(ddlDistrictsBatch.SelectedValue))
            If Not dt.Rows.Count = 0 Then
                Dim dr As DataRow = dt.Rows(0)
                If dr("Month") Is DBNull.Value Then
                    ddlMonthProcess.SelectedValue = 0
                Else
                    ddlMonthProcess.SelectedValue = dr("Month")
                End If
                If dr("Year") Is DBNull.Value Then

                    ddlYearProcess.SelectedValue = CInt(Date.Today.Year)
                Else
                    ddlYearProcess.SelectedValue = dr("Year")
                End If
            Else
                ddlMonthProcess.SelectedValue = 0
                ddlYearProcess.SelectedValue = 0
            End If
        End If
    End Sub
    Private Sub ReselectCachedCriteria()
        Dim ddl As DropDownList = Nothing
        Dim txtbox As TextBox = Nothing
        Dim rb As RadioButton = Nothing
        Dim chk As CheckBox = Nothing
        Dim ctrl As Control

        If Not Session("ProcessCriteria") Is Nothing Then
            ProcessControls = CType(Session("ProcessCriteria"), Dictionary(Of String, String))
            For Each id As String In ProcessControls.Keys
                ctrl = pnlTop.FindControl(id)
                If (TypeOf ctrl Is DropDownList) Then
                    ddl = CType(ctrl, DropDownList)
                    ddl.SelectedValue = ProcessControls(id)
                End If
            Next
        End If

        If Not Session("FilterCriteria") Is Nothing Then
            FilterControls = CType(Session("FilterCriteria"), Dictionary(Of String, String))
            For Each id As String In FilterControls.Keys
                ctrl = pnlMiddle.FindControl(id)
                If (TypeOf ctrl Is DropDownList) Then
                    ddl = CType(ctrl, DropDownList)
                    ddl.SelectedValue = FilterControls(id)
                    If id.Contains("District") Then
                        FillProductFilter()
                    End If
                End If
            Next
        End If

        If Not Session("PreviewCriteria") Is Nothing Then
            PreviewControls = CType(Session("PreviewCriteria"), Dictionary(Of String, String))
            For Each id As String In PreviewControls.Keys
                ctrl = Panel1.FindControl(id)
                If (TypeOf ctrl Is DropDownList) Then
                    ddl = CType(ctrl, DropDownList)
                    ddl.SelectedValue = PreviewControls(id)
                    If id.Contains("District") Then
                        FillProductAAC()
                        FillBatches()
                        FillHealthFacility()
                    End If

                ElseIf (TypeOf ctrl Is TextBox) Then
                    txtbox = CType(ctrl, TextBox)
                    txtbox.Text = PreviewControls(id)

                ElseIf (TypeOf ctrl Is RadioButton) Then
                    rb = CType(ctrl, RadioButton)
                    rb.Checked = PreviewControls(id)

                ElseIf (TypeOf ctrl Is CheckBox) Then
                    chk = CType(ctrl, CheckBox)
                    chk.Checked = PreviewControls(id)
                End If
            Next
        End If
    End Sub

    Private Sub FillDropdown()
        FillRegionsBatch()
        FillRegionsFilter()
        FillRegionACC()
        FillMonth()
        FillYear()
        FillCareType()
        ' FillProduct()
        FillPeriod()
        FillHealthFacility()
        FillBatches()
    End Sub
    Private Sub FillRegionsBatch()
        Dim dtRegions As DataTable = process.GetRegions(imisgen.getUserId(Session("User")), True, True)
        ddlRegionBatch.DataSource = dtRegions
        ddlRegionBatch.DataValueField = "RegionId"
        ddlRegionBatch.DataTextField = "RegionName"
        ddlRegionBatch.DataBind()
        If dtRegions.Rows.Count = 1 Then
            FillDistrictsBatch()
        End If
    End Sub
    Private Sub FillDistrictsBatch()
        Dim dt As New DataTable
        dt = process.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegionBatch.SelectedValue, True)

        ddlDistrictsBatch.DataSource = dt
        ddlDistrictsBatch.DataValueField = "DistrictId"
        ddlDistrictsBatch.DataTextField = "DistrictName"
        ddlDistrictsBatch.DataBind()
    End Sub
    Private Sub FillRegionsFilter()
        Dim dtRegions As DataTable = process.GetRegions(imisgen.getUserId(Session("User")), True, True)
        ddlRegionFilter.DataSource = dtRegions
        ddlRegionFilter.DataValueField = "RegionId"
        ddlRegionFilter.DataTextField = "RegionName"
        ddlRegionFilter.DataBind()
        If dtRegions.Rows.Count = 1 Then
            FillDistrictFilter()
        End If
    End Sub
    Private Sub FillDistrictFilter()
        Dim dt As New DataTable
        dt = process.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegionFilter.SelectedValue, True)
        ddlDistrictFilter.DataSource = dt
        ddlDistrictFilter.DataValueField = "DistrictId"
        ddlDistrictFilter.DataTextField = "DistrictName"
        ddlDistrictFilter.DataBind()
    End Sub
    Private Sub FillRegionACC()
        Dim dtRegions As DataTable = process.GetRegions(imisgen.getUserId(Session("User")), True, True)
        ddlRegionACC.DataSource = dtRegions
        ddlRegionACC.DataValueField = "RegionId"
        ddlRegionACC.DataTextField = "RegionName"
        ddlRegionACC.DataBind()
        If dtRegions.Rows.Count = 1 Then
            FillDistrictAcc()
        End If

    End Sub
    Private Sub FillDistrictAcc()

        Dim dt As New DataTable
        dt = process.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegionACC.SelectedValue, True)
        ddlDistrictACC.DataSource = dt
        ddlDistrictACC.DataValueField = "DistrictId"
        ddlDistrictACC.DataTextField = "DistrictName"
        ddlDistrictACC.DataBind()
        If ddlRegionACC.SelectedValue = -1 And dt.Rows.Count = 0 Then
            FillBatches()
        End If
    End Sub
    Private Sub FillMonth()
        Dim dtMonth As New DataTable
        Dim dt As Date = Today

        dtMonth = process.GetMonths(1, 12)

        ddlMonthProcess.DataSource = dtMonth
        ddlMonthProcess.DataValueField = "MonthNum"
        ddlMonthProcess.DataTextField = "MonthName"
        ddlMonthProcess.DataBind()
        '    ddlMonthProcess.SelectedValue = Month(dt.AddMonths(-1))



    End Sub

    Private Sub FillYear()
        Dim dtYear As DataTable

        dtYear = process.GetYears(2012, Year(Today))

        ddlYearProcess.DataSource = dtYear
        ddlYearProcess.DataValueField = "YearId"
        ddlYearProcess.DataTextField = "Year"
        ddlYearProcess.DataBind()
        ' ddlYearProcess.SelectedValue = Year(Now())

        ddlYearFilter.DataSource = dtYear
        ddlYearFilter.DataValueField = "Year"
        ddlYearFilter.DataTextField = "Year"
        ddlYearFilter.DataBind()


    End Sub

    Private Sub FillCareType()
        ddlHFLevelFilter.DataSource = process.GetHFLevelType(True)
        ddlHFLevelFilter.DataValueField = "Code"
        ddlHFLevelFilter.DataTextField = "HFLevel"
        ddlHFLevelFilter.DataBind()

        ddlHFLevel.DataSource = process.GetHFLevel(True)
        ddlHFLevel.DataValueField = "Code"
        ddlHFLevel.DataTextField = "HFLevel"
        ddlHFLevel.DataBind()

    End Sub

    Private Sub FillProduct()
        FillProductFilter()
        FillProductAAC()
    End Sub

    Private Sub FillProductFilter()
        Dim LocationId As Integer
        If Val(ddlDistrictFilter.SelectedValue) > 0 Then
            LocationId = Val(ddlDistrictFilter.SelectedValue)
        Else
            LocationId = Val(ddlRegionFilter.SelectedValue)
        End If

        ddlProductFilter.DataSource = process.GetProductsStict(LocationId, imisgen.getUserId(Session("User")), True)
        ddlProductFilter.DataValueField = "ProdID"
        ddlProductFilter.DataTextField = "ProductCode"
        ddlProductFilter.DataBind()
    End Sub

    Private Sub FillProductAAC()
        Dim LocationId As Integer
        If Val(ddlDistrictACC.SelectedValue) > 0 Then
            LocationId = Val(ddlDistrictACC.SelectedValue)
        Else
            LocationId = Val(ddlRegionACC.SelectedValue)
        End If
        ddlProductAAC.DataSource = process.GetProductsStict(LocationId, imisgen.getUserId(Session("User")), True)
        ddlProductAAC.DataValueField = "ProdID"
        ddlProductAAC.DataTextField = "ProductCode"
        ddlProductAAC.DataBind()
    End Sub

    Private Sub FillPeriod()
        ddlPeriod.DataSource = process.GetPeriod
        ddlPeriod.DataValueField = "Code"
        ddlPeriod.DataTextField = "DistType"
        ddlPeriod.DataBind()
    End Sub

    Private Sub FillBatches()
        Dim LocationId As Integer
        If Val(ddlDistrictACC.SelectedValue) = 0 Then
            LocationId = Val(ddlRegionACC.SelectedValue)
        Else
            LocationId = Val(ddlDistrictACC.SelectedValue)
        End If
        ddlBatchAAC.DataSource = process.GetBatches(LocationId)
        ddlBatchAAC.DataValueField = "RunID"
        ddlBatchAAC.DataTextField = "Batch"
        ddlBatchAAC.DataBind()
    End Sub

    Private Sub FillHealthFacility()
        Dim LocationId As Integer
        If Val(ddlDistrictACC.SelectedValue) = 0 Then
            LocationId = Val(ddlRegionACC.SelectedValue)
        Else
            LocationId = Val(ddlDistrictACC.SelectedValue)
        End If

        ddlHF.DataSource = process.GetHealthFacility(imisgen.getUserId(Session("User")), LocationId)
        ddlHF.DataValueField = "HFID"
        ddlHF.DataTextField = "HFCode"
        ddlHF.DataBind()
    End Sub

    Private Sub CacheCriteria(ByVal Button As String, Optional ByVal redirection As Boolean = False)

        If Button = "Process" Then
            Session("ProcessCriteria") = ProcessControls
        ElseIf Button = "Filter" Then
            Session("FilterCriteria") = FilterControls
        ElseIf Button = "Preview" Then
            Session("PreviewCriteria") = PreviewControls
        End If

    End Sub

    Private Sub btnProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess.Click
        Try
            CacheCriteria("Process", True)


            Dim eProduct As New IMIS_EN.tblProduct
            Dim eLocations As New IMIS_EN.tblLocations
            eRelIndex.AuditUserID = imisgen.getUserId(Session("User"))

            If Val(ddlDistrictsBatch.SelectedValue) = 0 Then
                eLocations.LocationId = Val(ddlRegionBatch.SelectedValue)
            Else
                eLocations.LocationId = Val(ddlDistrictsBatch.SelectedValue)
            End If

            eProduct.tblLocation = eLocations
            ' eProduct.ProdID = ddlProductFilter.SelectedValue

            Dim RelType As Integer = 0

            Select Case ddlPeriod.SelectedValue
                Case "Y"
                    RelType = 1
                Case "M"
                    RelType = 12
                Case "Q"
                    RelType = 4
                Case Else
                    RelType = 0
            End Select

            Dim modularURL As String = System.Web.Configuration.WebConfigurationManager.AppSettings("ModularGQLSource").ToString()
            Dim modularExecution As Boolean = Not (modularURL = "")

            eRelIndex.RelType = RelType
            eRelIndex.tblProduct = eProduct
            eRelIndex.RelYear = If(IsNumeric(ddlYearProcess.SelectedValue), ddlYearProcess.SelectedValue, 0)
            eRelIndex.RelPeriod = ddlMonthProcess.SelectedValue
            eRelIndex.RelCareType = ddlHFLevelFilter.SelectedValue

            Dim Proc = -1
            If modularExecution Then
                Dim result = modularBatchProcess.sendBatchRunGQLRequest(
                    If(IsNumeric(ddlYearProcess.SelectedValue), ddlYearProcess.SelectedValue, 0), ' year
                    If(IsNumeric(ddlMonthProcess.SelectedValue), ddlMonthProcess.SelectedValue, 0), ' month
                    eLocations.LocationId, ' location
                    imisgen.getLoginName(Session("User")) ' GQL endpoint
                )
                If result.error Is Nothing Then
                    Proc = 0
                Else
                    Proc = IIf(result.error.Contains("2"), 2, 1)
                End If
            Else
                Proc = process.ProcessBatch(eRelIndex)
            End If



            Dim msg As String = ""
            Select Case Proc
                Case 0
                    Dim LocationName As String = ""

                    If Val(ddlDistrictsBatch.SelectedValue) = 0 Then
                        LocationName = ddlRegionBatch.SelectedItem.Text
                    Else
                        LocationName = ddlDistrictsBatch.SelectedItem.Text
                    End If

                    msg = imisgen.getMessage("L_DISTRICT") & ": " & LocationName & "<BR />"
                    msg = msg & imisgen.getMessage("L_YEAR") & ": " & eRelIndex.RelYear & "<BR />"
                    msg = msg & imisgen.getMessage("L_MONTH") & ": " & eRelIndex.RelPeriod & "<BR />"
                    msg = msg & imisgen.getMessage("M_PROCESSEDSUCCESSFULLY")
                Case 2
                    msg = imisgen.getMessage("M_BATCHPROCESSRUNDONETHISSELECTION")
                Case Else
                    msg = imisgen.getMessage("M_ERRORWHILEPROCESSING")
            End Select

            If Proc = 0 Then
                gvRelIndex.DataSource = process.GetRelIndexes(eRelIndex, True)
                gvRelIndex.DataBind()
                FillBatches()
            End If

            imisgen.Alert(msg, divPopupScript)

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub btnFilter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFilter.Click
        Dim period As Integer = hfPeriod.Value
        hfPeriod.Value = ""
        Try
            CacheCriteria("Filter", True)

            Dim eProduct As New IMIS_EN.tblProduct
            Dim eLocations As New IMIS_EN.tblLocations
            Dim iRelType As Int16

            If ddlPeriod.SelectedValue = "M" Then
                iRelType = 12
            ElseIf ddlPeriod.SelectedValue = "Q" Then
                iRelType = 4
            Else
                iRelType = 1
            End If

            If Val(ddlDistrictsBatch.SelectedValue) = 0 Then
                eLocations.LocationId = Val(ddlRegionFilter.SelectedValue)
            Else
                eLocations.LocationId = Val(ddlDistrictFilter.SelectedValue)
            End If
            'eLocations.LocationId = ddlDistrictFilter.SelectedValue
            eProduct.tblLocation = eLocations
            eProduct.ProdID = Val(ddlProductFilter.SelectedValue)

            eRelIndex.RelType = iRelType
            eRelIndex.tblProduct = eProduct
            eRelIndex.RelYear = If(IsNumeric(ddlYearFilter.SelectedValue), ddlYearFilter.SelectedValue, 0)
            eRelIndex.RelPeriod = period
            eRelIndex.RelCareType = ddlHFLevelFilter.SelectedValue


            gvRelIndex.DataSource = process.GetRelIndexes(eRelIndex, False)
            gvRelIndex.DataBind()

            lblMsg.Text = imisgen.getMessage("M_RESULTFILTEREDSUCCESSFULLY")

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub ddlDistrictFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrictFilter.SelectedIndexChanged
        FillProductFilter()
    End Sub

    Private Sub ddlDistrictACC_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrictACC.SelectedIndexChanged
        FillProductAAC()
        FillBatches()
        FillHealthFacility()
    End Sub

    Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim dt As New DataTable
        Dim sSubTitle As String = ""
        Try
            CacheCriteria("Preview", True)


            Dim RangeFrom As Nullable(Of Date)
            Dim RangeTo As Nullable(Of Date)
            Dim DistrictID As Integer
            If Val(ddlDistrictACC.SelectedValue) > 0 Then
                DistrictID = Val(ddlDistrictACC.SelectedValue)
            Else
                DistrictID = Val(ddlRegionACC.SelectedValue)
            End If

            Dim ProductID As Integer = Val(ddlProductAAC.SelectedValue)
            Dim RunID As Integer = If(IsNumeric(ddlBatchAAC.SelectedValue), ddlBatchAAC.SelectedValue, 0)
            Dim HFID As Integer = If(IsNumeric(ddlHF.SelectedValue), ddlHF.SelectedValue, 0)
            Dim HFLevel As String = If(ddlHFLevel.SelectedIndex > 0, ddlHFLevel.SelectedValue, "")

            If txtENDData.Text = "" Then txtENDData.Text = txtSTARTData.Text

            If Not txtSTARTData.Text = "" Then
                If IsDate(Date.ParseExact(txtSTARTData.Text, "dd/MM/yyyy", Nothing)) Then
                    RangeFrom = Date.ParseExact(txtSTARTData.Text, "dd/MM/yyyy", Nothing)
                End If
            Else
                ' RangeFrom = Nothing
            End If

            If Not txtENDData.Text = "" Then
                If IsDate(Date.ParseExact(txtENDData.Text, "dd/MM/yyyy", Nothing)) Then
                    RangeTo = Date.ParseExact(txtENDData.Text, "dd/MM/yyyy", Nothing)
                End If
            Else
                'RangeTo = Nothing
            End If

            If rbCapitation.Checked Then
                dt = getCapitationPayment(Val(ddlRegionACC.SelectedValue), Val(ddlDistrictACC.SelectedValue),
                                          Val(ddlProductAAC.SelectedValue), Val(ddlBatchAAC.SelectedValue))
                If dt Is Nothing Then Exit Sub
                Session("Report") = dt
                Response.Redirect("Report.aspx?r=ca&tid=18")
            End If

            If RunID > 0 Then sSubTitle = sSubTitle & " " & imisgen.getMessage("L_RUNID") & ": " & ddlBatchAAC.SelectedItem.Text
            If Not txtSTARTData.Text = "" Then
                If sSubTitle.Length > 0 Then sSubTitle += " | "
                sSubTitle += imisgen.getMessage("L_DATEFROM") & " " & txtSTARTData.Text & " " & imisgen.getMessage("L_TO") & " " & txtENDData.Text
            End If

            If Val(ddlRegionACC.SelectedValue) > 0 OrElse ddlRegionACC.SelectedValue = -1 Then
                If sSubTitle.Length > 0 Then sSubTitle += " | "
                sSubTitle += imisgen.getMessage("L_REGION") & " : " & ddlRegionACC.SelectedItem.Text
            End If

            If Val(ddlDistrictACC.SelectedValue) > 0 Then
                If sSubTitle.Length > 0 Then sSubTitle += " | "
                sSubTitle += imisgen.getMessage("L_DISTRICT") & " : " & ddlDistrictACC.SelectedItem.Text
            End If

            '  If DistrictID > 0 Or ProductID > 0 Or RunID >= 0 Or HFID > 0 Or HFLevel.Length > 0 Then
            'sSubTitle = sSubTitle & " Filter("
            '  sSubTitle = sSubTitle & if(DistrictID = "0", "", "District: " & ddlDistrictACC.SelectedItem.Text)

            If Not ProductID = "0" Then
                If sSubTitle.Length > 0 Then sSubTitle += " | "
                sSubTitle += imisgen.getMessage("L_PRODUCT") & ": " & ddlProductAAC.SelectedItem.Text
            End If
            If ddlHFLevel.SelectedIndex > 0 Then
                If sSubTitle.Length > 0 Then sSubTitle += " | "
                sSubTitle += imisgen.getMessage("L_LEVEL") & ": " & ddlHFLevel.SelectedItem.Text
            End If

            '  If ddlHF.SelectedIndex > 0 Then sSubTitle += ", Health Facility: " & ddlHF.SelectedItem.Text
            'If ddlHFLevel.SelectedIndex > 0 Then sSubTitle += "Level: " & ddlHFLevel.SelectedItem.Text
            ' sSubTitle += ")"
            '  End If

            If sSubTitle.Length = 0 Then sSubTitle = imisgen.getMessage("R_ALLCLAIMS")
            IMIS_EN.eReports.SubTitle = sSubTitle
            Dim showall As Integer = chkShowAll.Checked

            If chkClaims.Checked = False Then
                dt = process.GetProcessBatch(DistrictID, ProductID, RunID, HFID, HFLevel, RangeFrom, RangeTo, showall)
            Else
                dt = process.GetProcessBatchWithClaims(DistrictID, ProductID, RunID, HFID, HFLevel, RangeFrom, RangeTo, showall)
            End If

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
        IMIS_EN.eReports.SubTitle = sSubTitle
        Session("Report") = dt
        If dt.Rows.Count > 0 Then
            If rbHF.Checked Then
                If chkClaims.Checked = False Then Response.Redirect("Report.aspx?r=pbh")
                If chkClaims.Checked = True Then Response.Redirect("Report.aspx?r=pbc&group=H")
            ElseIf rbProduct.Checked Then
                If chkClaims.Checked = False Then Response.Redirect("Report.aspx?r=pbp")
                If chkClaims.Checked = True Then Response.Redirect("Report.aspx?r=pbc&group=P")
            ElseIf rbCapitation.Checked Then
                dt = getCapitationPayment(Val(ddlRegionACC.SelectedValue), Val(ddlDistrictACC.SelectedValue),
                                          Val(ddlProductAAC.SelectedValue), Val(ddlBatchAAC.SelectedValue))
                If dt Is Nothing Then Exit Sub
                Session("Report") = dt

                Response.Redirect("Report.aspx?r=ca&tid=18")
            End If
            imisgen.Alert(imisgen.getMessage("M_NODATAFORREPORT"), pnlButtons, alertPopupTitle:="IMIS")
        End If



    End Sub

    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub

    Private Sub ddlPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriod.SelectedIndexChanged
        If ddlPeriod.SelectedValue = "Y" Then
            MonthFilterBind("Y")
        ElseIf ddlPeriod.SelectedValue = "Q" Then
            MonthFilterBind("Q")
        ElseIf ddlPeriod.SelectedValue = "M" Then
            MonthFilterBind("M")
        End If
    End Sub
    Private Sub MonthFilterBind(ByVal Type As Char)
        ddlMonthFilter.DataSource = process.GetPeriodNo(Type)
        ddlMonthFilter.DataTextField = "Period"
        ddlMonthFilter.DataValueField = "Value"
        ddlMonthFilter.DataBind()
    End Sub

    Private Sub ddlRegionBatch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegionBatch.SelectedIndexChanged
        FillDistrictsBatch()
    End Sub

    Private Sub ddlRegionFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegionFilter.SelectedIndexChanged
        FillDistrictFilter()
        FillProductFilter()
    End Sub

    Private Sub ddlRegionACC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegionACC.SelectedIndexChanged
        FillDistrictAcc()
        FillProductAAC()
        FillBatches()
        FillHealthFacility()
    End Sub

    Private Function getCapitationPayment(RegionId, DistrictID, ProductID, RunID) As DataTable
        Dim sSubTitle As String = ""
        Dim year As Integer
        Dim month As Integer
        Dim batchRun As DataRow
        Dim dtCapitation As DataTable
        Dim reports As New IMIS_BI.ReportsBI
        Dim batchRunBL As New IMIS_BL.BatchRunBL
        Dim locationName As String = ""

        Dim dt As New DataTable
        ' Dim dtHFLevel As New DataTable
        '  \dtHFLevel.Columns.Add("Code")
        'dtHFLevel.Columns.Add("Name")

        ' Run year and date depending on selected batch run
        batchRun = batchRunBL.GetBatchRunById(RunID)
        year = batchRun("RunYear")
        month = batchRun("RunMonth")

        ' LocationName 
        If (Val(RegionId) > 0 Or Val(RegionId) = -1) Then locationName = imisgen.getMessage("L_REGION") & ": " & ddlRegionACC.SelectedItem.Text
        If Val(DistrictID) > 0 Then locationName += " | " & imisgen.getMessage("L_DISTRICT") & ": " & ddlDistrictACC.SelectedItem.Text


        If Val(ProductID) > 0 Then
            dtCapitation = reports.getProductCapitationDetails(ProductID)

            If dtCapitation.Rows.Count Then
                IMIS_EN.eReports.Level1 = getHFName(dtCapitation.Rows(0)("Level1").ToString)
                IMIS_EN.eReports.Sublevel1 = dtCapitation.Rows(0)("HFSublevel1").ToString
                IMIS_EN.eReports.Level2 = getHFName(dtCapitation.Rows(0)("Level2").ToString)
                IMIS_EN.eReports.Sublevel2 = dtCapitation.Rows(0)("HFSublevel2").ToString
                IMIS_EN.eReports.Level3 = getHFName(dtCapitation.Rows(0)("Level3").ToString)
                IMIS_EN.eReports.Sublevel3 = dtCapitation.Rows(0)("HFSublevel3").ToString
                IMIS_EN.eReports.Level4 = getHFName(dtCapitation.Rows(0)("Level4").ToString)
                IMIS_EN.eReports.Sublevel4 = dtCapitation.Rows(0)("HFSublevel4").ToString
                IMIS_EN.eReports.ShareContribution = If(dtCapitation.Rows(0)("ShareContribution") Is DBNull.Value, 0, dtCapitation.Rows(0)("ShareContribution"))
                IMIS_EN.eReports.WeightPopulation = If(dtCapitation.Rows(0)("WeightPopulation") Is DBNull.Value, 0, dtCapitation.Rows(0)("WeightPopulation"))
                IMIS_EN.eReports.WeightNumberFamilies = If(dtCapitation.Rows(0)("WeightNumberFamilies") Is DBNull.Value, 0, dtCapitation.Rows(0)("WeightNumberFamilies"))
                IMIS_EN.eReports.WeightInsuredPopulation = If(dtCapitation.Rows(0)("WeightInsuredPopulation") Is DBNull.Value, 0, dtCapitation.Rows(0)("WeightInsuredPopulation"))
                IMIS_EN.eReports.WeightNumberInsuredFamilies = If(dtCapitation.Rows(0)("WeightNumberInsuredFamilies") Is DBNull.Value, 0, dtCapitation.Rows(0)("WeightNumberInsuredFamilies"))
                IMIS_EN.eReports.WeightNumberVisits = If(dtCapitation.Rows(0)("WeightNumberVisits") Is DBNull.Value, 0, dtCapitation.Rows(0)("WeightNumberVisits"))
                IMIS_EN.eReports.WeightAdjustedAmount = If(dtCapitation.Rows(0)("WeightAdjustedAmount") Is DBNull.Value, 0, dtCapitation.Rows(0)("WeightAdjustedAmount"))

            End If

            '   lblCatchmentArea.Text = dt.Rows(0)("Catchment")
        End If

        dt = reports.getCatchmentArea(Val(RegionId), Val(DistrictID), Val(ProductID), year, month)

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            Dim locationId = If(DistrictID > 0, DistrictID, RegionId)
            If Not batchRunBL.WasAlreadyRun(locationId, year, month) Then
                lblMsg.Text = imisgen.getMessage("M_CAPITATION_PAYMENT_NO_BATCH_RUN")
                Return Nothing
            End If
            lblMsg.Text = imisgen.getMessage("M_NODATAFORREPORT")
            Return Nothing
        End If
        If Val(RegionId) > 0 Or Val(RegionId) = -1 Or ProductID > 0 Then
            If Not Val(RegionId) = 0 Then
                'If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += locationName
            End If
        End If
        If Not ProductID = 0 Then
            If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
            Dim dtProdDetails As DataTable = reports.GetProductName_Account(Val(ProductID))
            Dim ProductName As String = ""

            If Not dtProdDetails Is Nothing AndAlso dtProdDetails.Rows.Count > 0 Then
                ProductName = dtProdDetails(0)("ProductName").ToString
            End If
            sSubTitle += imisgen.getMessage("L_PRODUCT") & ": " & ddlProductAAC.SelectedItem.Text & " - " & ProductName & ", " '& imisgen.getMessage("L_PRODUCTCODE") & ": " & AccountCode
            'sSubTitle += imisgen.getMessage("L_CODE") & ": " & ddlProductStrict.SelectedItem.Text
        End If

        ' Batch dropdown is in format year-month
        Dim batchDateText() As String = Split(ddlBatchAAC.SelectedItem.Text, "-")
        If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
        sSubTitle += imisgen.getMessage("L_MONTH") & ": " & batchDateText(1)

        If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
        sSubTitle += imisgen.getMessage("L_YEAR") & ": " & batchDateText(0)



        IMIS_EN.eReports.SubTitle = sSubTitle
        Return dt
    End Function

    Private Function getHFName(ByVal Code As String)
        Select Case Code
            Case "H"
                Return imisgen.getMessage("T_HOSPITAL")
            Case "C"
                Return imisgen.getMessage("T_HEALTHCENTRE")
            Case "D"
                Return imisgen.getMessage("T_DISPENSARY")
            Case Else
                Return ""
        End Select
    End Function

End Class

