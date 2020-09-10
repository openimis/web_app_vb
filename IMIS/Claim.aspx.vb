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

Partial Public Class Claim
    Inherits System.Web.UI.Page
    Private claim As New IMIS_BI.ClaimBI
    Private eClaim As New IMIS_EN.tblClaim
    Protected imisgen As New IMIS_Gen
    Private eHF As New IMIS_EN.tblHF
    Private eClaimService As New IMIS_EN.tblClaimServices
    Private eClaimItem As New IMIS_EN.tblClaimItems
    Private eClaimAdmin As New IMIS_EN.tblClaimAdmin
    Private userBI As New IMIS_BI.UserBI
    Protected canClearRow As Boolean = True
    Private hfBI As New IMIS_BI.HealthFacilityBI
    Private claimBI As New IMIS_BI.ClaimBI
    Private claimAdminBI As New IMIS_BI.ClaimAdministratorBI
    Protected RestoreMode As Boolean = True

    Private Sub FormatForm()

        Dim Adjustibility As String = ""


        'ClaimAdministrator
        Adjustibility = General.getControlSetting("ClaimAdministrator")
        lblClaimAdminCode.Visible = Not (Adjustibility = "N")
        txtClaimAdminCode.Visible = Not (Adjustibility = "N")


        'GuranteeNo
        Adjustibility = General.getControlSetting("GuaranteeNo")
        lblGurantee.Visible = Not (Adjustibility = "N")
        txtGuaranteeId.Visible = Not (Adjustibility = "N")
        rfGuranteeId.Enabled = (Adjustibility = "M")

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""

        '  ddlHFCode.Attributes.Add("oncontextmenu", "RightClickJSFunction(this.id);")
        If IsPostBack = True Then Return
        RunPageSecurity()
        FormatForm()
        Try
            If Request.QueryString("c") IsNot Nothing Then
                Dim hfClaimUUID As Guid = Guid.Parse(HttpContext.Current.Request.QueryString("c"))
                hfClaimID.Value = If(hfClaimUUID.Equals(Guid.Empty), 0, claimBI.GetClaimIdByUUID(hfClaimUUID))
            Else
                hfClaimID.Value = 0
            End If

            eClaim.ClaimID = hfClaimID.Value
            If eClaim.ClaimID = 0 Then
                hfHFID.Value = hfBI.GetHfIdByUUID(Guid.Parse(Request.QueryString("h")))
                eHF.HfID = hfHFID.Value
                eHF.HfUUID = Guid.Parse(Request.QueryString("h"))
                claim.getHFCodeAndName(eHF)

                eClaim.tblHF = eHF
                hfClaimID.Value = 0
                txtHFCode.Text = eHF.HFCode
                txtHFName.Text = eHF.HFName
                If Request.QueryString("a") IsNot Nothing Then
                    Dim ClaimAdminId As Integer = claimAdminBI.GetClaimAdminIdByUUID(Guid.Parse(Request.QueryString("a")))

                    If IsNumeric(ClaimAdminId) Then
                        hfClaimAdminId.Value = ClaimAdminId
                        eClaimAdmin.ClaimAdminId = CInt(hfClaimAdminId.Value)
                        eClaimAdmin.ClaimAdminUUID = Guid.Parse(Request.QueryString("a"))
                        claim.GetClaimAdminDetails(eClaimAdmin)
                        If eClaimAdmin.ClaimAdminCode IsNot Nothing Then txtClaimAdminCode.Text = eClaimAdmin.ClaimAdminCode.ToString.Trim
                    End If
                End If
            Else
                claim.LoadClaim(eClaim)
                hfHFID.Value = eClaim.tblHF.HfID
                txtHFCode.Text = eClaim.tblHF.HFCode
                txtHFName.Text = eClaim.tblHF.HFName
                If eClaim.tblClaimAdmin.ClaimAdminCode IsNot Nothing Then
                    txtClaimAdminCode.Text = eClaim.tblClaimAdmin.ClaimAdminCode.ToString.Trim
                End If
                hfClaimAdminId.Value = eClaim.tblClaimAdmin.ClaimAdminId
                txtGuaranteeId.Text = eClaim.GuaranteeId
                hfGuaranteeId.Value = txtGuaranteeId.Text
            End If
            'Addition for Nepal >> Stat
            FillICDCodes()
            FillVisitTypes()
            'Addition for Nepal >> Stat

            Dim dt As DataTable = claim.GetHFCodes(imisgen.getUserId(Session("User")), 0)
            Dim dr() As DataRow = dt.Select("hfid =" & eClaim.tblHF.HfID)
            If dr.Length = 0 Then
                pnlBodyCLM.Attributes.Add("Class", "disabled")
                pnlServiceDetails.Enabled = False
                pnlItemsDetails.Enabled = False
                B_ADD.Visible = False
                B_SAVE.Visible = False
            End If

            If Not eClaim.tblInsuree Is Nothing Then
                hfInsureeId.Value = eClaim.tblInsuree.InsureeID
                txtCHFIDData.Text = eClaim.tblInsuree.CHFID
                txtNAMEData.Text = eClaim.tblInsuree.OtherNames & " " & eClaim.tblInsuree.LastName
            End If

            If Not eClaim.tblICDCodes Is Nothing Then
                ddlICDData.SelectedValue = eClaim.tblICDCodes.ICDID
                txtICDCode0.Text = ddlICDData.SelectedItem.Text
                hfICDID0.Value = eClaim.tblICDCodes.ICDID
            End If
            If Not eClaim.ClaimCode Is Nothing Then
                txtCLAIMCODEData.Text = eClaim.ClaimCode
            End If
            'Addition for Nepal >> Start
            If Not eClaim.ICDID1 Is Nothing Then
                ddlICDData1.SelectedValue = eClaim.ICDID1
                txtICDCode1.Text = ddlICDData1.SelectedItem.Text
                hfICDID1.Value = eClaim.ICDID1
            End If
            If Not eClaim.ICDID2 Is Nothing Then
                ddlICDData2.SelectedValue = eClaim.ICDID2
                txtICDCode2.Text = ddlICDData2.SelectedItem.Text
                hfICDID2.Value = eClaim.ICDID2
            End If
            If Not eClaim.ICDID3 Is Nothing Then
                ddlICDData3.SelectedValue = eClaim.ICDID3
                txtICDCode3.Text = ddlICDData3.SelectedItem.Text
                hfICDID3.Value = eClaim.ICDID3
            End If
            If Not eClaim.ICDID4 Is Nothing Then
                ddlICDData4.SelectedValue = eClaim.ICDID4
                txtICDCode4.Text = ddlICDData4.SelectedItem.Text
                hfICDID4.Value = eClaim.ICDID4
            End If
            If Not eClaim.VisitType Is Nothing Then
                ddlVisitType.SelectedValue = eClaim.VisitType
            End If
            'Addition for Nepal >> End
            txtCLAIMCODEData.Attributes.Add("ClaimCodetag", txtCLAIMCODEData.Text)

            If Not eClaim.DateClaimed = Nothing Then
                txtClaimDate.Text = Left(eClaim.DateClaimed.ToString, 10)
            End If
            'txtClaimDate.Text = Left(eClaim.DateClaimed.ToString, 10)
            txtSTARTData.Text = If(eClaim.DateFrom = Nothing, "", eClaim.DateFrom)
            txtENDData.Text = If(eClaim.DateTo Is Nothing, "", eClaim.DateTo)
            txtEXPLANATION.Text = eClaim.Explanation

            If Not eClaim.ClaimID = 0 Then
                StoreClaimDetails()
            End If

            txtAddItemRows.Text = IMIS_EN.AppConfiguration.DefaultClaimRows
            txtAddServiceRows.Text = txtAddItemRows.Text
            ServiceItemGridBinding()


            hfPrevItemRows.Value = CInt(txtAddItemRows.Text)
            hfPrevServiceRows.Value = CInt(txtAddServiceRows.Text)

            Dim dzt As DataTable = claim.GetServiceCode(eClaim.tblHF.HfID)

            gvHiddenServiceCodes.DataSource = claim.GetServiceCode(eClaim.tblHF.HfID)
            gvHiddenServiceCodes.DataBind()

            gvHiddenItemCodes.DataSource = claim.GetItemCode(eClaim.tblHF.HfID)
            gvHiddenItemCodes.DataBind()

            hfClaimItemID.Value = 0
            hfClaimServiceID.Value = 0
            If Not eClaim.ClaimID = 0 Then
                If Not eClaim.ClaimStatus = 2 Then
                    pnlBodyCLM.Attributes.Add("Class", "disabled")
                    pnlBodyCLM.Enabled = False
                    pnlServiceDetails.Enabled = False
                    pnlItemsDetails.Enabled = False
                    B_ADD.Visible = False
                    B_SAVE.Visible = False
                    canClearRow = False
                End If
            End If
            tdPrintW.Visible = eClaim.ClaimID > 0
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
    Private Sub FillICDCodes()
        Dim dtICD As DataTable = claim.GetICDCodes
        ddlICDData.DataSource = dtICD
        ddlICDData.DataTextField = "ICDCODE"
        ddlICDData.DataValueField = "ICDID"
        ddlICDData.DataBind()
        ddlICDData1.DataSource = dtICD
        ddlICDData1.DataTextField = "ICDCODE"
        ddlICDData1.DataValueField = "ICDID"
        ddlICDData1.DataBind()
        ddlICDData2.DataSource = dtICD
        ddlICDData2.DataTextField = "ICDCODE"
        ddlICDData2.DataValueField = "ICDID"
        ddlICDData2.DataBind()
        ddlICDData3.DataSource = dtICD
        ddlICDData3.DataTextField = "ICDCODE"
        ddlICDData3.DataValueField = "ICDID"
        ddlICDData3.DataBind()
        ddlICDData4.DataSource = dtICD
        ddlICDData4.DataTextField = "ICDCODE"
        ddlICDData4.DataValueField = "ICDID"
        ddlICDData4.DataBind()
    End Sub
    Private Sub FillVisitTypes()
        ddlVisitType.DataSource = claim.GetVisitTypes(True)
        ddlVisitType.DataValueField = "Code"
        ddlVisitType.DataTextField = "Visit"
        ddlVisitType.DataBind()
    End Sub
    Private Sub StoreClaimDetails()
        Session("LoadedHFID") = hfHFID.Value
        Session("LoadedICD") = txtICDCode0.Text
        Session("LoadedICD") = ddlICDData.SelectedValue
        Session("LoadedCHFID") = txtCHFIDData.Text ' CInt(txtCHFIDData.Text)
        Session("LoadedClaimCode") = txtCLAIMCODEData.Text
        Session("LoadedClaimedDate") = txtClaimDate.Text 'Date.Parse(txtClaimDate.Text)
        Session("LoadedVisitDateFrom") = txtSTARTData.Text 'Date.Parse(txtSTARTData.Text)
        Session("LoadedVisitDateTo") = If(eClaim.DateTo Is Nothing, txtSTARTData.Text, eClaim.DateTo)
        Session("LoadedExplanation") = eClaim.Explanation
        'Addition for Nepal >> Start

        Session("LoadedICD1") = ddlICDData1.SelectedValue
        Session("LoadedICD2") = ddlICDData2.SelectedValue
        Session("LoadedICD3") = ddlICDData3.SelectedValue
        Session("LoadedICD4") = ddlICDData4.SelectedValue

        Session("LoadedICD1") = txtICDCode1.Text
        Session("LoadedICD2") = txtICDCode2.Text
        Session("LoadedICD3") = txtICDCode3.Text
        Session("LoadedICD4") = txtICDCode4.Text
        Session("LoadedVisitType") = ddlVisitType.SelectedValue
        'Addition for Nepal >> End
    End Sub
    Private Sub RunPageSecurity()

        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Claim, Page) Then
            B_SAVE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ClaimAdd, UserID)
            btnPrint.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ClaimPrint, UserID)
            btnRestore.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ClaimRestore, UserID)    'RFC 111 06/09/2019

            If Not B_SAVE.Visible Then
                pnlBodyCLM.Attributes.Add("Class", "disabled")
                pnlBodyCLM.Enabled = False
                pnlServiceDetails.Enabled = False
                pnlItemsDetails.Enabled = False
                B_ADD.Visible = False
                canClearRow = False
            End If

        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Claim.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub ServiceItemGridBinding()
        Dim ds As New DataSet
        Dim dt As DataTable

        ds = claim.getClaimServiceAndItems(eClaim.ClaimID)
        dt = ds.Tables("ClaimedServices")

        Dim ItemRows, ServiceRows As Integer

        If Not txtAddServiceRows.Text = 5 Then
            ServiceRows = txtAddServiceRows.Text
        Else
            ServiceRows = IMIS_EN.AppConfiguration.DefaultClaimRows
        End If

        If dt.Rows.Count < ServiceRows Then
            Dim y As Integer = ServiceRows - dt.Rows.Count
            For c As Integer = 1 To y
                dt.Rows.Add(dt.NewRow())
            Next
        Else
            txtAddServiceRows.Text = dt.Rows.Count
        End If

        gvService.DataSource = dt
        Session("vsServices") = dt
        gvService.DataBind()

        dt = ds.Tables("ClaimedItems")

        If Not txtAddItemRows.Text = 5 Then
            ItemRows = txtAddItemRows.Text
        Else
            ItemRows = IMIS_EN.AppConfiguration.DefaultClaimRows
        End If
        If dt.Rows.Count < ItemRows Then
            Dim y As Integer = ItemRows - dt.Rows.Count
            For c As Integer = 1 To y
                dt.Rows.Add(dt.NewRow())
            Next
        Else
            txtAddItemRows.Text = dt.Rows.Count
        End If

        gvItems.DataSource = dt
        Session("vsItems") = dt
        gvItems.DataBind()
    End Sub
    Protected Sub txtCHFIDData_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCHFIDData.TextChanged
        Try
            'LoadGridOnTextChange()

            Dim insureeid As Integer = 0
            txtNAMEData.Text = claim.verifyCHFIDandReturnName(sender.text, insureeid).ToString

            If txtNAMEData.Text = "" Then
                imisgen.Alert(imisgen.getMessage("M_CHFIDNOTFOUND"), pnlClaimDetails, alertPopupTitle:="IMIS")
                pnlServiceDetails.Enabled = False
                pnlItemsDetails.Enabled = False
                txtAddServiceRows.Enabled = False
                txtAddItemRows.Enabled = False
            Else
                hfInsureeId.Value = insureeid
                lblMsg.Text = ""
                EnablingServiceAndItemPanel()
            End If
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

    End Sub
    Private Sub EnablingServiceAndItemPanel()
        pnlServiceDetails.Enabled = True
        pnlItemsDetails.Enabled = True
        txtAddServiceRows.Enabled = True
        txtAddItemRows.Enabled = True
    End Sub
    Private Sub B_ADD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_ADD.Click
        Try
            EnablingServiceAndItemPanel()
            txtCLAIMCODEData.Text = ""
            txtCHFIDData.Text = ""
            txtENDData.Text = ""
            txtSTARTData.Text = ""
            txtClaimDate.Text = ""
            txtEXPLANATION.Text = ""
            txtICDCode0.Text = ""
            txtICDCode1.Text = ""
            txtICDCode2.Text = ""
            txtICDCode3.Text = ""
            txtICDCode4.Text = ""
            txtCLAIMTOTALData.Text = 0
            hfClaimID.Value = 0
            txtNAMEData.Text = ""
            txtAddItemRows.Text = 5
            txtAddServiceRows.Text = 5
            hfPrevItemRows.Value = 5
            hfPrevServiceRows.Value = 5
            ddlVisitType.SelectedValue = ""
            txtGuaranteeId.Text = ""
            'hfClaimAdminId.Value = 0
            'txtClaimAdminCode.Text = String.Empty

            Dim dtS As DataTable = CType(Session("vsServices"), DataTable)
            Dim dtI As DataTable = CType(Session("vsItems"), DataTable)
            Dim counter As Integer

            dtS.Clear()
            dtI.Clear()
            For counter = 1 To IMIS_EN.AppConfiguration.DefaultClaimRows
                dtS.Rows.Add(dtS.NewRow())
                dtI.Rows.Add(dtI.NewRow())
            Next

            gvService.DataSource = dtS
            gvService.DataBind()

            gvItems.DataSource = dtI
            gvItems.DataBind()
            txtCHFIDData.Focus()
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Session("RestoreMode") = Nothing
        Response.Redirect("FindClaims.aspx")
    End Sub
    Private Function IsClaimChanged(ByRef ClaimTotalValueFlag As Boolean) As Boolean
        Dim EndDate As Date = CType(Session("LoadedVisitDateTo"), Date)

        If Not Session("LoadedHFID") = hfHFID.Value Then
        ElseIf Not Session("LoadedCHFID") = txtCHFIDData.Text Then
        ElseIf Not Session("LoadedClaimCode") = txtCLAIMCODEData.Text Then
        ElseIf Not Session("LoadedClaimedDate") = txtClaimDate.Text Then
        ElseIf Not Session("LoadedVisitDateFrom") = txtSTARTData.Text Then
        ElseIf Not If(EndDate = Nothing, "", Format(EndDate, "dd/MM/yyyy")) = txtENDData.Text Then
        ElseIf Not Session("LoadedICD") = txtICDCode0.Text Then
        ElseIf hfGuaranteeId.Value <> txtGuaranteeId.Text.Trim Then
        ElseIf Not Session("LoadedExplanation") = txtEXPLANATION.Text.Trim Then
            'Addition for Nepal >> Start
        ElseIf Not Session("LoadedICD1") = txtICDCode1.Text Then
        ElseIf Not Session("LoadedICD2") = txtICDCode2.Text Then
        ElseIf Not Session("LoadedICD3") = txtICDCode3.Text Then
        ElseIf Not Session("LoadedICD4") = txtICDCode4.Text Then
        ElseIf Not CType(Session("LoadedVisitType"), String) = ddlVisitType.SelectedValue Then
            'Addition for Nepal >> End
        Else
            If Not hfClaimTotalValue.Value = hfInitialCLMTotalValue.Value Then
                ClaimTotalValueFlag = True
            End If
            Return False
        End If
        Return True
    End Function
    Private Function ClaimValidation() As String
        Try
            If eClaim.ClaimID = 0 Then
                Dim ServiceExists As Boolean
                Dim ItemExists As Boolean
                For Each Row As GridViewRow In gvService.Rows
                    If CType(Row.Cells(0).Controls(1), TextBox).Text.Trim <> "" Then
                        ServiceExists = True
                        Exit For
                    End If
                Next
                For Each Row As GridViewRow In gvItems.Rows
                    If CType(Row.Cells(0).Controls(1), TextBox).Text.Trim <> "" Then
                        ItemExists = True
                        Exit For
                    End If
                Next
                If Not ServiceExists And Not ItemExists Then
                    imisgen.Alert(imisgen.getMessage("M_ADDATLEASTONEITEMORSERVICE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return "Exit"
                End If
            End If

            'If ddlHFCode.SelectedValue = 0 Then
            '    lblMsg.Text = imisgen.getMessage("M_SELECTHFCODE")
            '    Return "Exit"
            'End If

            'If txtICDCode.Text = "" Then
            '    lblMsg.Text = imisgen.getMessage("M_SELECTICDCODE")
            '    Return "Exit"
            'Else
            '    Dim i As Integer = claim.getICDIDFromCode(txtICDCode.Text)
            '    If i = 0 Then
            '        lblMsg.Text = imisgen.getMessage("M_INVALIDICDCODE")
            '        Return "Exit"
            '    Else
            '        Dim eICD As New IMIS_EN.tblICDCodes
            '        eICD.ICDID = i
            '        eClaim.tblICDCodes = eICD

            '    End If

            'End If
            If txtICDCode0.Text = "" Then
                imisgen.Alert(imisgen.getMessage("M_PLEASEENTERANMDGCODE"), pnlButtons, alertPopupTitle:="IMIS")
                Return "Exit"
            End If
            If txtNAMEData.Text = "" Then
                imisgen.Alert(imisgen.getMessage("M_CHFIDNOTFOUND"), pnlButtons, alertPopupTitle:="IMIS")
                Return "Exit"
            End If

            eHF.HfID = hfHFID.Value
            eClaim.tblHF = eHF
            eClaim.ClaimCode = txtCLAIMCODEData.Text

            If eClaim.ClaimID > 0 Then
                If Not txtCLAIMCODEData.Attributes.Item("ClaimCodetag") = eClaim.ClaimCode Then
                    If claim.checkClaimCode(eClaim) Then
                        imisgen.Alert(imisgen.getMessage("M_CLAIMCODEEXISTS"), pnlButtons, alertPopupTitle:="IMIS")
                        Return "Exit"
                    End If
                End If
            Else
                If claim.checkClaimCode(eClaim) Then
                    imisgen.Alert(imisgen.getMessage("M_CLAIMCODEEXISTS"), pnlButtons, alertPopupTitle:="IMIS")
                    Return "Exit"
                End If
            End If

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            'imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            'Return "Exit"
            Throw New Exception(ex.Message)
        End Try
        Return "Continue"
    End Function
    Private Sub B_SAVE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_SAVE.Click
        Dim chkSaveClaimItems, chkSaveClaim, chkSaveClaimServices As Integer
        eClaim.ClaimID = hfClaimID.Value
        If CType(Me.Master.FindControl("hfDirty"), HiddenField).Value = True Then

            Try
                If ClaimValidation() = "Exit" Then
                    Return
                End If

                If Not eClaim.ClaimID = 0 Then
                    'Added by Salumu 05092019 setting claim status to entered, review and feedback to idle for rejected claims
                    'Starts
                    If CInt(Session("RestoreMode")) = True Then
                        If Not eClaim.ClaimStatus = 1 Then
                            eClaim.ClaimStatus = 2
                            eClaim.ReviewStatus = 1
                            eClaim.FeedbackStatus = 1
                            eClaim.ClaimCode = txtCLAIMCODEData.Text
                        End If

                        'Ends
                    Else
                        If claim.IsClaimStatusChanged(eClaim) Then
                            lblMsg.Text = imisgen.getMessage("M_CLAIMSTATUSCHANGEDFROMENTERED")
                            Return
                        End If
                    End If
                End If

                eClaim.AuditUserID = imisgen.getUserId(Session("User"))
                Dim CLMTotalValueFlag As Boolean

                If eClaim.ClaimID = 0 Or IsClaimChanged(CLMTotalValueFlag) = True Or CLMTotalValueFlag = True Or Session("RestoreMode") = True Then
                    If CLMTotalValueFlag = True Then
                        eClaim.Claimed = hfClaimTotalValue.Value
                        claim.UpdateClaimTotalValue(eClaim)
                    Else
                        Dim eInsuree As New IMIS_EN.tblInsuree
                        eInsuree.InsureeID = hfInsureeId.Value
                        eClaim.tblInsuree = eInsuree
                        eClaim.DateClaimed = Date.ParseExact(txtClaimDate.Text, "dd/MM/yyyy", Nothing)
                        eClaim.DateFrom = Date.ParseExact(txtSTARTData.Text, "dd/MM/yyyy", Nothing)
                        If txtENDData.Text.Length > 0 Then
                            eClaim.DateTo = Date.ParseExact(txtENDData.Text, "dd/MM/yyyy", Nothing)
                        Else
                            eClaim.DateTo = Date.ParseExact(txtSTARTData.Text, "dd/MM/yyyy", Nothing)
                        End If
                        eClaim.Claimed = hfClaimTotalValue.Value
                        Dim eICDCodes As New IMIS_EN.tblICDCodes
                        eICDCodes.ICDID = If(hfICDID0.Value = "", 0, CInt(Int(hfICDID0.Value)))

                        'Addition for Nepal >> Start
                        If Not txtICDCode1.Text = "" Then eClaim.ICDID1 = If(hfICDID1.Value = "", 0, CInt(Int(hfICDID1.Value)))
                        If Not txtICDCode2.Text = "" Then eClaim.ICDID2 = If(hfICDID2.Value = "", 0, CInt(Int(hfICDID2.Value)))
                        If Not txtICDCode3.Text = "" Then eClaim.ICDID3 = If(hfICDID3.Value = "", 0, CInt(Int(hfICDID3.Value)))
                        If Not txtICDCode4.Text = "" Then eClaim.ICDID4 = If(hfICDID4.Value = "", 0, CInt(Int(hfICDID4.Value)))
                        If ddlVisitType.SelectedValue.Trim <> "" Then eClaim.VisitType = ddlVisitType.SelectedValue
                        'Addition for Nepal >> End

                        eClaim.tblICDCodes = eICDCodes
                        eClaim.Explanation = txtEXPLANATION.Text
                        If hfClaimAdminId.Value.Trim <> String.Empty Then
                            eClaimAdmin.ClaimAdminId = CInt(hfClaimAdminId.Value)
                        End If
                        eClaim.tblClaimAdmin = eClaimAdmin
                        eClaim.GuaranteeId = txtGuaranteeId.Text
                        chkSaveClaim = claim.SaveClaim(eClaim)
                        hfClaimID.Value = eClaim.ClaimID
                        txtCLAIMCODEData.Attributes.Add("ClaimCodetag", eClaim.ClaimCode)
                        If txtENDData.Text = "" Then txtENDData.Text = txtSTARTData.Text
                        StoreClaimDetails()
                    End If

                End If

            Catch ex As Exception
                'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
                Return
            End Try
        End If
        Try
            Dim eItem As New IMIS_EN.tblItems
            Dim eService As New IMIS_EN.tblServices
            Dim eClaimItems As New IMIS_EN.tblClaimItems
            Dim eClaimServices As New IMIS_EN.tblClaimServices


            For Each row As GridViewRow In gvItems.Rows
                If Not gvItems.DataKeys.Item(row.RowIndex).Values("ItemCode") Is DBNull.Value Then
                    If Not gvItems.DataKeys.Item(row.RowIndex).Values("ItemCode") = CType(gvItems.Rows(row.RowIndex).Cells(0).Controls(1), TextBox).Text Then
                    ElseIf Not gvItems.DataKeys.Item(row.RowIndex).Values("QtyProvided") = CType(gvItems.Rows(row.RowIndex).Cells(1).Controls(1), TextBox).Text Then
                    ElseIf Not gvItems.DataKeys.Item(row.RowIndex).Values("PriceAsked") = CType(gvItems.Rows(row.RowIndex).Cells(2).Controls(1), TextBox).Text Then
                    ElseIf Not gvItems.DataKeys.Item(row.RowIndex).Values("Explanation").ToString = CType(gvItems.Rows(row.RowIndex).Cells(3).Controls(1), TextBox).Text Then
                    Else
                        Continue For
                    End If
                End If
                If Not gvItems.DataKeys.Item(row.RowIndex).Values("ClaimItemId") Is DBNull.Value Then
                    'eItem.ItemID = gvItems.DataKeys.Item(row.RowIndex).Values("ItemID")
                    eClaimItems.ClaimItemID = gvItems.DataKeys.Item(row.RowIndex).Values("ClaimItemId")
                    'Ruzo:start:Added on 2 July 13 - On update of items, new item id should be ready
                    Dim ItemCod As String = CType(row.Cells(0).Controls(1), TextBox).Text
                    If ItemCod.Trim <> String.Empty Then
                        For Each r As GridViewRow In gvHiddenItemCodes.Rows
                            If ItemCod = r.Cells(0).Text & "  " & HttpUtility.HtmlDecode(r.Cells(1).Text.ToString) Then
                                eItem.ItemID = gvHiddenItemCodes.DataKeys(r.RowIndex).Values("ItemID")
                                Exit For
                            End If
                        Next
                        If eItem.ItemID = 0 Then
                            imisgen.Alert(imisgen.getMessage("M_WRONGITEMCODENAME"), pnlButtons, alertPopupTitle:="IMIS")
                            Return
                        End If
                    Else
                        eItem.ItemID = gvItems.DataKeys.Item(row.RowIndex).Values("ItemID")
                    End If
                    'Ruzo:end
                Else
                    If Not CType(gvItems.Rows(row.RowIndex).Cells(0).Controls(1), TextBox).Text = "" Then
                        eClaimItems.tblClaim = eClaim
                        Dim ItemCod As String = CType(row.Cells(0).Controls(1), TextBox).Text
                        For Each r As GridViewRow In gvHiddenItemCodes.Rows
                            If ItemCod = r.Cells(0).Text & "  " & HttpUtility.HtmlDecode(r.Cells(1).Text.ToString) Then
                                eItem.ItemID = gvHiddenItemCodes.DataKeys(r.RowIndex).Values("ItemID")
                                eClaimItems.ClaimItemID = 0
                                Exit For
                            End If
                        Next
                        If eItem.ItemID = 0 Then
                            imisgen.Alert(imisgen.getMessage("M_WRONGITEMCODENAME"), pnlButtons, alertPopupTitle:="IMIS")
                            Return
                        End If
                    End If
                End If
                If Not CType(gvItems.Rows(row.RowIndex).Cells(0).Controls(1), TextBox).Text = "" Or Not gvItems.DataKeys.Item(row.RowIndex).Values("ClaimItemId") Is DBNull.Value Then
                    eClaimItems.tblItems = eItem
                    eClaimItems.AuditUserID = eClaim.AuditUserID
                    If CType(gvItems.Rows(row.RowIndex).Cells(0).Controls(1), TextBox).Text = "" Then

                        If Not hfClaimItemID.Value = gvItems.DataKeys.Item(row.RowIndex).Values("ClaimItemId") Then
                            claim.DeleteClaimItems(eClaimItems)
                            chkSaveClaimItems = 3
                            hfClaimItemID.Value = gvItems.DataKeys.Item(row.RowIndex).Values("ClaimItemId")
                            'lblMsg.Text = "The Item were deleted successfully"
                        End If

                    Else
                        eClaimItems.QtyProvided = CType(row.Cells(1).Controls(1), TextBox).Text
                        eClaimItems.PriceAsked = CType(row.Cells(2).Controls(1), TextBox).Text
                        eClaimItems.Explanation = CType(row.Cells(3).Controls(1), TextBox).Text
                        chkSaveClaimItems = claim.SaveClaimItems(eClaimItems)
                    End If

                End If
            Next

            For Each row In gvService.Rows
                If Not gvService.DataKeys.Item(row.RowIndex).Values("ServCode") Is DBNull.Value Then
                    If Not gvService.DataKeys.Item(row.RowIndex).Values("ServCode") = CType(gvService.Rows(row.RowIndex).Cells(0).Controls(1), TextBox).Text Then
                    ElseIf Not gvService.DataKeys.Item(row.RowIndex).Values("QtyProvided") = CType(gvService.Rows(row.RowIndex).Cells(1).Controls(1), TextBox).Text Then
                    ElseIf Not gvService.DataKeys.Item(row.RowIndex).Values("PriceAsked") = CType(gvService.Rows(row.RowIndex).Cells(2).Controls(1), TextBox).Text Then
                    ElseIf Not gvService.DataKeys.Item(row.RowIndex).Values("Explanation").ToString = CType(gvService.Rows(row.RowIndex).Cells(3).Controls(1), TextBox).Text Then
                    Else
                        Continue For
                    End If
                End If
                If Not gvService.DataKeys.Item(row.RowIndex).Values("ClaimServiceId") Is DBNull.Value Then
                    'eService.ServiceID = gvService.DataKeys.Item(row.RowIndex).Values("ServiceID")
                    eClaimServices.ClaimServiceID = gvService.DataKeys.Item(row.RowIndex).Values("ClaimServiceId")
                    'Ruzo:start:Added on 2 July 13 - On update of services, new service id should be ready
                    Dim ServiceCod As String = CType(row.cells(0).controls(1), TextBox).Text
                    If ServiceCod.Trim <> String.Empty Then
                        For Each r As GridViewRow In gvHiddenServiceCodes.Rows
                            If ServiceCod = r.Cells(0).Text & "  " & HttpUtility.HtmlDecode(r.Cells(1).Text.ToString) Then
                                eService.ServiceID = gvHiddenServiceCodes.DataKeys(r.RowIndex).Values("ServiceID")
                                Exit For
                            End If
                        Next
                        If eService.ServiceID = 0 Then
                            imisgen.Alert(imisgen.getMessage("M_WRONGSERVICECODENAME"), pnlButtons, alertPopupTitle:="IMIS")
                            Return
                        End If
                    Else
                        eService.ServiceID = gvService.DataKeys.Item(row.RowIndex).Values("ServiceID")
                    End If
                    'Ruzo:end
                Else
                    If Not CType(gvService.Rows(row.RowIndex).Cells(0).Controls(1), TextBox).Text = "" Then
                        eClaimServices.tblClaim = eClaim
                        Dim ServiceCod As String = CType(row.cells(0).controls(1), TextBox).Text
                        For Each r As GridViewRow In gvHiddenServiceCodes.Rows
                            Dim str As String = (r.Cells(1).Text.ToString)
                            If ServiceCod = r.Cells(0).Text & "  " & HttpUtility.HtmlDecode(r.Cells(1).Text.ToString) Then
                                eService.ServiceID = gvHiddenServiceCodes.DataKeys(r.RowIndex).Values("ServiceID")
                                eClaimServices.ClaimServiceID = 0
                                Exit For
                            End If
                        Next
                        If eService.ServiceID = 0 Then
                            imisgen.Alert(imisgen.getMessage("M_WRONGSERVICECODENAME"), pnlButtons, alertPopupTitle:="IMIS")
                            Return
                        End If
                    End If
                End If
                If Not CType(gvService.Rows(row.RowIndex).Cells(0).Controls(1), TextBox).Text = "" Or Not gvService.DataKeys.Item(row.RowIndex).Values("ClaimServiceId") Is DBNull.Value Then
                    eClaimServices.tblServices = eService
                    eClaimServices.AuditUserID = eClaim.AuditUserID
                    If CType(gvService.Rows(row.RowIndex).Cells(0).Controls(1), TextBox).Text = "" Then
                        If Not hfClaimServiceID.Value = gvService.DataKeys.Item(row.RowIndex).Values("ClaimServiceId") Then
                            claim.DeleteClaimService(eClaimServices)
                            chkSaveClaimServices = 3
                            hfClaimServiceID.Value = gvService.DataKeys.Item(row.RowIndex).Values("ClaimServiceId")
                            'lblMsg.Text = "The Services were deleted successfully"
                        End If

                    Else
                        eClaimServices.QtyProvided = CType(row.cells(1).controls(1), TextBox).Text
                        eClaimServices.PriceAsked = CType(row.cells(2).controls(1), TextBox).Text
                        eClaimServices.Explanation = CType(row.cells(3).controls(1), TextBox).Text
                        chkSaveClaimServices = claim.SaveClaimServices(eClaimServices)
                    End If
                End If
            Next

            ServiceItemGridBinding()
            AfterSaveMessages(chkSaveClaim, chkSaveClaimItems, chkSaveClaimServices)
            tdPrintW.Visible = eClaim.ClaimID > 0
            'Added by Salumu to kill the session
            Session("RestoreMode") = Nothing
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
    Private Sub AfterSaveMessages(ByVal chkSaveClaim As Integer, ByVal chkSaveClaimItems As Integer, ByVal chkSaveClaimServices As Integer)
        'UPDATING OF A CLAIM AND SERVICE OR ITEM
        If chkSaveClaim = 0 And chkSaveClaimItems = 0 And chkSaveClaimServices = 0 Then
            lblMsg.Text = imisgen.getMessage("M_NOCHANGEMADETOCLAIM")
        Else
            lblMsg.Text = imisgen.getMessage("M_CLAIMSAVED")
        End If
        'ElseIf chkSaveClaim = 0 And chkSaveClaimItems = 0 And chkSaveClaimServices = 2 Then
        'lblMsg.Text = imisgen.getMessage("M_CLAIMSERVICESUPDATED")
        'ElseIf chkSaveClaim = 0 And chkSaveClaimItems = 2 And chkSaveClaimServices = 0 Then
        'lblMsg.Text = imisgen.getMessage("M_CLAIMITEMUPDATED")
        'ElseIf chkSaveClaim = 0 And chkSaveClaimItems = 2 And chkSaveClaimServices = 2 Then
        'lblMsg.Text = imisgen.getMessage("M_CLMSERVICESITEMSUPDATED")
        'ElseIf chkSaveClaim = 2 And chkSaveClaimItems = 0 And chkSaveClaimServices = 0 Then
        'lblMsg.Text = imisgen.getMessage("M_CLAIMDETAILSUPDATED")
        'ElseIf chkSaveClaim = 2 And chkSaveClaimItems = 0 And chkSaveClaimServices = 2 Then
        'lblMsg.Text = imisgen.getMessage("M_CLMSERVICESUPDATED")
        'ElseIf chkSaveClaim = 2 And chkSaveClaimItems = 2 And chkSaveClaimServices = 0 Then
        'lblMsg.Text = imisgen.getMessage("M_CLMITEMUPDATED")
        'ElseIf chkSaveClaim = 2 And chkSaveClaimItems = 2 And chkSaveClaimServices = 2 Then
        'lblMsg.Text = imisgen.getMessage("M_CLMDETAILSERVICEITEM")
        ''INSERTING OF NEW CLAIM AND SERVICE OR ITEM
        'ElseIf chkSaveClaim = 1 And chkSaveClaimItems = 0 And chkSaveClaimServices = 1 Then
        'lblMsg.Text = imisgen.getMessage("M_CLMDSERVICESSAVED")
        'ElseIf chkSaveClaim = 1 And chkSaveClaimItems = 1 And chkSaveClaimServices = 0 Then
        'lblMsg.Text = imisgen.getMessage("M_CLMDITEMSAVED")
        'ElseIf chkSaveClaim = 1 And chkSaveClaimItems = 1 And chkSaveClaimServices = 1 Then
        'lblMsg.Text = imisgen.getMessage("M_CLMDSERVICEITEMSAVED")
        ''UPDATES OF CLAIM BY INSERTING A NEW SERVICE OR ITEM
        'ElseIf chkSaveClaim = 0 And chkSaveClaimItems = 0 And chkSaveClaimServices = 1 Then
        'lblMsg.Text = imisgen.getMessage("M_SERVICESSAVED")
        'ElseIf chkSaveClaim = 0 And chkSaveClaimItems = 1 And chkSaveClaimServices = 0 Then
        'lblMsg.Text = imisgen.getMessage("M_ITEMSSAVED")
        'ElseIf chkSaveClaim = 0 And chkSaveClaimItems = 1 And chkSaveClaimServices = 1 Then
        'lblMsg.Text = imisgen.getMessage("M_SERVICESITEMSSAVED")
        ''DELETE OF SERVICES OR ITEMS
        'ElseIf chkSaveClaimItems = 0 And chkSaveClaimServices = 3 Then
        'lblMsg.Text = imisgen.getMessage("M_SERVICESDELETED")
        'ElseIf chkSaveClaimItems = 3 And chkSaveClaimServices = 0 Then
        'lblMsg.Text = imisgen.getMessage("M_ITEMSDELETED")
        'ElseIf chkSaveClaimItems = 3 And chkSaveClaimServices = 3 Then
        'lblMsg.Text = imisgen.getMessage("M_SERVICESITEMDELETED")
        'End If
    End Sub
    Private Sub AddNewRow(ByRef gv As GridView, ByVal dt As DataTable, ByVal RowNo As Integer)

        Try
            Dim dtNew As New DataTable
            dtNew = dt

            For x As Integer = 1 To RowNo
                dtNew.Rows.Add(dtNew.NewRow())
            Next

            gv.DataSource = dtNew
            gv.DataBind()

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

    End Sub
    Private Sub LoadGridOnTextChange()
        Dim dtS As DataTable = CType(Session("vsServices"), DataTable)
        Dim dtI As DataTable = CType(Session("vsItems"), DataTable)
        Dim ds As New DataSet
        dtS.Clear()
        dtI.Clear()
        Dim countItemRow, countServRow As Integer
        Dim dr As DataRow

        For Each r As GridViewRow In gvService.Rows
            If Not CType(r.Cells(0).Controls(1), TextBox).Text = "" Then
                dr = dtS.NewRow
                dr.Item("ServCode") = CType(r.Cells(0).Controls(1), TextBox).Text
                dr.Item("QtyProvided") = CType(r.Cells(1).Controls(1), TextBox).Text
                dr.Item("PriceAsked") = CType(r.Cells(2).Controls(1), TextBox).Text
                dr.Item("Explanation") = CType(r.Cells(3).Controls(1), TextBox).Text
                dtS.Rows.Add(dr)

            End If
        Next

        For Each r As GridViewRow In gvItems.Rows
            If Not CType(r.Cells(0).Controls(1), TextBox).Text = "" Then
                dr = dtI.NewRow
                dr.Item("ItemCode") = CType(r.Cells(0).Controls(1), TextBox).Text
                dr.Item("QtyProvided") = CType(r.Cells(1).Controls(1), TextBox).Text
                dr.Item("PriceAsked") = CType(r.Cells(2).Controls(1), TextBox).Text
                dr.Item("Explanation") = CType(r.Cells(3).Controls(1), TextBox).Text
                dtI.Rows.Add(dr)
            End If
        Next

        If gvItems.Rows.Count > dtI.Rows.Count Then
            countItemRow = gvItems.Rows.Count - dtI.Rows.Count
            For x As Integer = 1 To countItemRow
                dtI.Rows.Add(dtI.NewRow)
            Next
        End If
        If gvService.Rows.Count > dtS.Rows.Count Then
            countServRow = gvService.Rows.Count - dtS.Rows.Count
            For x As Integer = 1 To countServRow
                dtS.Rows.Add(dtS.NewRow)
            Next
        End If
        gvService.DataSource = dtS
        gvService.DataBind()
        gvItems.DataSource = dtI
        gvItems.DataBind()

        Session("vsServices") = dtS
        Session("vsItems") = dtI
    End Sub
    Private Sub txtAddServiceRows_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddServiceRows.TextChanged
        LoadGridOnTextChange()
        If CInt(txtAddServiceRows.Text) > hfPrevServiceRows.Value Then
            Dim c As Integer = CInt(txtAddServiceRows.Text) - hfPrevServiceRows.Value
            AddNewRow(gvService, CType(Session("vsServices"), DataTable), c)
            hfPrevServiceRows.Value = CInt(txtAddServiceRows.Text)
        Else
            txtAddServiceRows.Text = hfPrevServiceRows.Value
        End If
    End Sub
    Private Sub txtAddItemRows_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAddItemRows.TextChanged
        LoadGridOnTextChange()
        If CInt(txtAddItemRows.Text) > hfPrevItemRows.Value Then
            Dim c As Integer = CInt(txtAddItemRows.Text) - hfPrevItemRows.Value
            AddNewRow(gvItems, CType(Session("vsItems"), DataTable), c)
            hfPrevItemRows.Value = CInt(txtAddItemRows.Text)
        Else
            txtAddItemRows.Text = hfPrevItemRows.Value
        End If
    End Sub
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        Dim Url As String
        Try
            If Not IsNumeric(hfClaimID.Value) OrElse hfClaimID.Value <= 0 Then Exit Sub
            Dim ClaimId As Integer = hfClaimID.Value
            Dim dt As DataTable = claim.GetClaim(ClaimId)
            If dt.Rows.Count = 0 Then 'Check if claim is present
                lblMsg.Text = imisgen.getMessage("M_NODATAFORREPORT")
                Exit Sub
            End If
            Dim ds As DataSet = claim.getClaimServiceAndItems(ClaimId)
            dt.TableName = "Claim"
            ds.Tables.Add(dt)

            'get the current position of the deductino and ceilings
            Dim dtInq As DataTable = claim.GetInsureeByCHFIDGrid(txtCHFIDData.Text)
            dtInq.TableName = "Policy"
            ds.Tables.Add(dtInq)

            Session("Report") = ds
            IMIS_EN.eReports.SubTitle = "Claim" 'imisgen.getMessage("L_HFACILITY") 
            Url = "Report.aspx?r=c"
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Exit Sub
        End Try
        Response.Redirect(Url)
    End Sub
    Protected Sub btnRestore_Click(sender As Object, e As EventArgs) Handles btnRestore.Click
        Try
            Dim hfClaimUUID As Guid = Guid.Parse(HttpContext.Current.Request.QueryString("c"))
            hfClaimID.Value = If(hfClaimUUID.Equals(Guid.Empty), 0, claimBI.GetClaimIdByUUID(hfClaimUUID))

            eClaim.ClaimID = hfClaimID.Value
            claim.LoadClaim(eClaim)

            If (eClaim.ClaimItems.RejectionReason = 0 And eClaim.ClaimServices.RejectionReason = 0) Then
                txtCHFIDData.Text = ""
                txtNAMEData.Text = ""
                txtNAMEData.Enabled = True
            End If


            pnlBodyCLM.Attributes.Add("Class", "enabled")
            pnlBodyCLM.Enabled = True
            pnlServiceDetails.Enabled = True
            pnlItemsDetails.Enabled = True
            B_ADD.Visible = True
            B_SAVE.Visible = True
            canClearRow = True
            RestoreMode = True
            btnPrint.Visible = False


            Session("RestoreMode") = RestoreMode
            If Session("RestoreMode") = True Then
                Dim claimCodePrefix As String = IMIS_EN.AppConfiguration.ClaimCodePrefix
                If Not claimCodePrefix Is Nothing Then
                    txtCLAIMCODEData.Text = claimCodePrefix + eClaim.ClaimCode
                Else
                    txtCLAIMCODEData.Text = "@" + eClaim.ClaimCode
                End If
                btnRestore.Visible = False
            End If
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
End Class
