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


Partial Public Class FindClaims
    Inherits System.Web.UI.Page
    Private eClaim As New IMIS_EN.tblClaim
    Protected imisgen As New IMIS_Gen
    Private FindClaimsB As New IMIS_BI.FindClaimsBI
    Private eUsers As New IMIS_EN.tblUsers
    Dim eHF As New IMIS_EN.tblHF
    Private eClaimAdmin As New IMIS_EN.tblClaimAdmin
    Private userBI As New IMIS_BI.UserBI
    Private hfBI As New IMIS_BI.HealthFacilityBI
    Private claimBI As New IMIS_BI.ClaimBI
    Private claimAdminBI As New IMIS_BI.ClaimAdministratorBI

    Private Sub FormatForm()

        Dim Adjustibility As String = ""


        'ClaimAdministrator
        Adjustibility = General.getControlSetting("ClaimAdministrator")
        lblClaimAdmin0.Visible = Not (Adjustibility = "N")
        ddlClaimAdmin.Visible = Not (Adjustibility = "N")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load, txtICDCode.TextChanged
        chkboxSubmitAll.Checked = False
        'ddlBatchRun.Attributes.Add("oncontextmenu", "RightClickJSFunction(this.id);")
        'ddlClaimStatus.Attributes.Add("oncontextmenu", "RightClickJSFunction(this.id,31);")
        'ddlFBStatus.Attributes.Add("oncontextmenu", "RightClickJSFunction(this.id,31);")
        'ddlHFCode.Attributes.Add("oncontextmenu", "RightClickJSFunction(this.id);")
        'ddlReviewStatus.Attributes.Add("oncontextmenu", "RightClickJSFunction(this.id,31);")
        'ddlDistrict.Attributes.Add("oncontextmenu", "RightClickJSFunction(this.id);")

        If Request.Form("__EVENTTARGET") = B_DELETE.ClientID Then
            B_DELETE_Click(sender, New System.EventArgs)
        End If
        If Request.Form("__EVENTTARGET") = B_SUBMIT.ClientID Then
            B_SUBMIT_Click(sender, New System.EventArgs)
        End If
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("i") Is Nothing Then
                txtCHFID.Text = HttpContext.Current.Request.QueryString("i")
            Else
                txtCHFID.Text = ""
            End If

        End If

        If IsPostBack = False Then RunPageSecurity()
        FormatForm()

        Try

            If IsPostBack = True Then Return

            Dim UserID As Integer
            UserID = imisgen.getUserId(Session("User"))
            FillRegion()
            If Request.QueryString("c") = "c" Then
                ddlRegion.SelectedValue = CType(Session("RegionSelected"), Integer)
                FillDistricts()
                'ddlDistrict.SelectedValue = CType(Session("DistrictIDFindClaims"), Integer)
                ddlDistrict.SelectedValue = 0
            End If
            ddlFBStatus.DataSource = FindClaimsB.GetFeedbackStatus()
            ddlFBStatus.DataTextField = "Status"
            ddlFBStatus.DataValueField = "Code"
            ddlFBStatus.DataBind()

            ddlReviewStatus.DataSource = FindClaimsB.GetReviewStatus()
            ddlReviewStatus.DataTextField = "Status"
            ddlReviewStatus.DataValueField = "Code"
            ddlReviewStatus.DataBind()

            ddlClaimStatus.DataSource = FindClaimsB.GetClaimStatus(63)
            ddlClaimStatus.DataTextField = "Status"
            ddlClaimStatus.DataValueField = "Code"
            ddlClaimStatus.DataBind()
            ddlClaimStatus.SelectedValue = 2

            'ddlICD.DataSource = FindClaimsB.GetICDCodes(True)
            'ddlICD.DataTextField = "ICDNames"
            'ddlICD.DataValueField = "ICDID"
            'ddlICD.DataBind()

            'gvHiddenICDCodes.DataSource = FindClaimsB.GetICDCodes(True)
            'gvHiddenICDCodes.DataBind()



            FillVisitTypes()
            HFCodeAndBatchRunBinding(UserID)
            If ddlHFCode.Items.Count = 1 Then
                txtHFName.Enabled = False
            End If
            '  AddButtonControl()
            ' ClaimCodeTxtControl()

            ButtonDisplayControl(0)
            If eHF.HfID = 0 And Request.QueryString("c") = Nothing Then
                Exit Sub
            End If
            loadgrid()
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

    End Sub

    Private Sub FillRegion()
        Dim dtRegions As DataTable = FindClaimsB.GetRegions(imisgen.getUserId(Session("User")), True)
        ddlRegion.DataSource = dtRegions
        ddlRegion.DataValueField = "RegionId"
        ddlRegion.DataTextField = "RegionName"
        ddlRegion.DataBind()

        If dtRegions.Rows.Count = 1 Then
            FillDistricts()

        End If
    End Sub
    Private Sub FillVisitTypes()
        ddlVisitType.DataSource = FindClaimsB.GetVisitTypes(True)
        ddlVisitType.DataValueField = "Code"
        ddlVisitType.DataTextField = "Visit"
        ddlVisitType.DataBind()
    End Sub
    Private Sub RunPageSecurity(Optional ByVal which As Integer = 0)
        Dim RefUrl = Request.Headers("Referer")

        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If which = 0 Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.FindClaim, Page) Then
                B_ADD.Visible = FindClaimsB.checkRights(IMIS_EN.Enums.Rights.ClaimAdd, UserID)
                B_LOAD.Visible = FindClaimsB.checkRights(IMIS_EN.Enums.Rights.ClaimLoad, UserID)
                B_DELETE.Visible = FindClaimsB.checkRights(IMIS_EN.Enums.Rights.ClaimDelete, UserID)
                B_SUBMIT.Visible = FindClaimsB.checkRights(IMIS_EN.Enums.Rights.ClaimSubmit, UserID)
                btnSearch.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ClaimSearch, UserID)
                If Not B_LOAD.Visible And Not B_DELETE.Visible And Not B_SUBMIT.Visible Then
                    ' pnlBody.Enabled = False
                    'pnlTop.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindClaim.ToString & "&retUrl=" & RefUrl)
            End If
        ElseIf which = 1 Then
            If Not FindClaimsB.checkRights(IMIS_EN.Enums.Rights.ClaimDelete, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindClaim.ToString & "&retUrl=" & RefUrl)
            End If
        ElseIf which = 2 Then
            If Not FindClaimsB.checkRights(IMIS_EN.Enums.Rights.ClaimReview, UserID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindClaim.ToString & "&retUrl=" & RefUrl)
            End If
        End If
    End Sub
    Private Sub AddButtonControl()
        If Not ddlDistrict.SelectedValue = 0 Then
            If Not ddlHFCode.SelectedValue = 0 Then
                B_ADD.Visible = True
            Else
                B_ADD.Visible = False
            End If
        Else
            B_ADD.Visible = False
        End If
        Session("DistrictIDFindClaims") = ddlDistrict.SelectedValue
        Session("HFID") = ddlHFCode.SelectedValue
        Session("RegionSelected") = ddlRegion.SelectedValue
    End Sub

    Private Sub FillHF(ByVal UserID As Integer)
        Dim LocationId As Integer = 0
        If Val(ddlDistrict.SelectedValue) > 0 Then
            LocationId = Val(ddlDistrict.SelectedValue)
        ElseIf Val(ddlRegion.SelectedValue) > 0 Then
            LocationId = Val(ddlRegion.SelectedValue)
        End If
        ddlHFCode.DataSource = FindClaimsB.GetHFCodes(UserID, LocationId)
        ddlHFCode.DataValueField = "HfID"
        ddlHFCode.DataTextField = "HFCODE"
        ddlHFCode.DataBind()
        If Request.QueryString("c") = "c" Then
            If IsPostBack = False Then
                ddlHFCode.SelectedValue = CType(Session("HFID"), Integer)
                ''''Clear HFID session
                Session.Remove("HFID")
            End If
        End If

    End Sub

    Private Sub HFCodeAndBatchRunBinding(ByVal UserID As Integer)

        FillHF(UserID)

        If Not Val(ddlDistrict.SelectedValue) = 0 Then
            ddlBatchRun.Enabled = True
            ddlBatchRun.DataSource = FindClaimsB.GetBatchRun(ddlDistrict.SelectedValue)
            ddlBatchRun.DataTextField = "Batch"
            ddlBatchRun.DataValueField = "RunID"
            ddlBatchRun.DataBind()
        Else
            ddlBatchRun.Enabled = False
        End If

        FillClaimAdminCodes()

    End Sub
    Private Sub FillClaimAdminCodes()
        Dim HFID As Integer = 0
        If ddlHFCode.SelectedIndex > -1 Then HFID = ddlHFCode.SelectedValue
        ddlClaimAdmin.DataSource = FindClaimsB.GetHFClaimAdminCodes(HFID, True)
        ddlClaimAdmin.DataTextField = "Description"
        ddlClaimAdmin.DataValueField = "ClaimAdminID"
        ddlClaimAdmin.DataBind()
    End Sub
    Private Sub loadgrid() Handles btnSearch.Click, gvClaims.PageIndexChanged

        Try
            Dim eBatchRun As New IMIS_EN.tblBatchRun
            Dim eICDCodes As New IMIS_EN.tblICDCodes
            Dim eInsuree As New IMIS_EN.tblInsuree

            If (Not Request.QueryString("c") = Nothing) And (ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack() = False) Then

                Dim dic As New Dictionary(Of String, String)
                If Not Session("FindClaimsFilter") Is Nothing Then
                    dic = CType(Session("FindClaimsFilter"), Dictionary(Of String, String))
                End If
                eClaim.LegacyID = Val(dic("LocationId")) 'Used as a carrier for DistrictID
                eHF.HfID = dic("HFID")
                eClaim.FeedbackStatus = dic("FeedbackStatus")
                eClaim.ReviewStatus = dic("ReviewStatus")
                eClaim.ClaimStatus = dic("ClaimStatus")
                eICDCodes.ICDID = dic("ICDID")

                If Not dic("CHFNo") = "" Then
                    eInsuree.CHFID = dic("CHFNo")
                End If
                If Not dic("VisitDateTo") = "" Then
                    eClaim.DateTo = Date.Parse(dic("VisitDateTo"))
                End If

                If Not dic("VisitDateFrom") = "" Then
                    eClaim.DateFrom = Date.Parse(dic("VisitDateFrom"))
                End If
                If Not dic("ClaimedDateFrom") = "" Then
                    eClaim.DateClaimed = Date.Parse(dic("ClaimedDateFrom"))
                End If

                If Not dic("ClaimedDateTo") = "" Then
                    eClaim.DateProcessed = Date.Parse(dic("ClaimedDateTo")) 'Used as a carrier for ClaimedDate to range 
                End If
                If Not dic("HFName") = "" Then
                    eHF.HFName = dic("HFName")
                End If
                If Not dic("BatchRunID") = "" Then
                    eBatchRun.RunID = dic("BatchRunID")
                End If
                If Not dic("ClaimCode") = "" Then
                    eClaim.ClaimCode = dic("ClaimCode")
                End If
                If dic("ClaimAdminID") IsNot Nothing Then
                    If dic("ClaimAdminID").Trim <> String.Empty Then
                        eClaimAdmin.ClaimAdminId = dic("ClaimAdminID")
                    End If
                End If
                eClaim.VisitType = dic("VisitType")
                eClaim.tblClaimAdmin = eClaimAdmin
                ddlRegion.SelectedValue = Val(dic("RegionId"))
                FillDistricts()
                ddlDistrict.SelectedValue = eClaim.LegacyID
                ddlHFCode.SelectedValue = eHF.HfID
                FillClaimAdminCodes()
                ddlFBStatus.SelectedValue = eClaim.FeedbackStatus
                ddlReviewStatus.SelectedValue = eClaim.ReviewStatus
                ddlClaimStatus.SelectedValue = eClaim.ClaimStatus
                'txtICDCode.SelectedValue = eICDCodes.ICDID
                hfICDCode.Value = eICDCodes.ICDID
                txtClaimCode.Text = If(eClaim.ClaimCode Is Nothing, "", eClaim.ClaimCode)
                txtHFName.Text = eHF.HFName
                txtCHFID.Text = eInsuree.CHFID
                txtVisitDateTo.Text = If(eClaim.DateTo Is Nothing, "", eClaim.DateTo)
                txtVisitDateFrom.Text = If(eClaim.DateFrom = Nothing, "", eClaim.DateFrom)
                txtClaimedDateFrom.Text = If(eClaim.DateClaimed = Nothing, "", eClaim.DateClaimed)
                txtClaimedDateTo.Text = If(eClaim.DateProcessed Is Nothing, "", eClaim.DateProcessed) 'Used as a carrier for ClaimedDate to range 
                ddlBatchRun.SelectedValue = If(eBatchRun.RunID = Nothing, Nothing, eBatchRun.RunID)
                ddlClaimAdmin.SelectedValue = eClaim.tblClaimAdmin.ClaimAdminId
                ddlVisitType.SelectedValue = eClaim.VisitType

                '''''clear Session("FindClaimsFilter")....
                Session.Remove("FindClaimsFilter")
            Else
                If Not ddlBatchRun.SelectedValue = "" Then
                    eBatchRun.RunID = ddlBatchRun.SelectedValue
                End If
                If ddlHFCode.SelectedIndex >= 0 Then
                    eHF.HfID = ddlHFCode.SelectedValue
                End If
                If Not txtHFName.Text = "" Then
                    eHF.HFName = txtHFName.Text
                End If

                eHF.RegionId = Val(ddlRegion.SelectedValue)
                eHF.DistrictId = Val(ddlDistrict.SelectedValue)

                eClaim.FeedbackStatus = ddlFBStatus.SelectedValue
                eClaim.ReviewStatus = ddlReviewStatus.SelectedValue
                eClaim.ClaimStatus = ddlClaimStatus.SelectedValue
                If Not hfICDID.Value = "" Then
                    eICDCodes.ICDID = CInt(Int(hfICDID.Value))
                Else
                    eICDCodes.ICDID = 0
                End If

                If Not txtClaimCode.Text = "" Then
                    eClaim.ClaimCode = txtClaimCode.Text
                End If

                If Not txtVisitDateTo.Text = "" Then
                    eClaim.DateTo = Date.Parse(txtVisitDateTo.Text)
                End If

                If Not txtVisitDateFrom.Text = "" Then
                    eClaim.DateFrom = Date.Parse(txtVisitDateFrom.Text)
                End If
                If Not txtClaimedDateFrom.Text = "" Then
                    eClaim.DateClaimed = Date.Parse(txtClaimedDateFrom.Text)
                End If

                If Not txtClaimedDateTo.Text = "" Then
                    eClaim.DateProcessed = Date.Parse(txtClaimedDateTo.Text) 'Used as a carrier for ClaimedDate to range 
                End If
                If Not txtCHFID.Text = "" Then
                    eInsuree.CHFID = txtCHFID.Text
                End If
                If ddlClaimAdmin.SelectedIndex > -1 Then
                    eClaimAdmin.ClaimAdminId = ddlClaimAdmin.SelectedValue
                End If
                eClaim.VisitType = ddlVisitType.SelectedValue
            End If

            eClaim.tblBatchRun = eBatchRun
            eClaim.tblHF = eHF
            eClaim.tblInsuree = eInsuree
            eClaim.tblICDCodes = eICDCodes
            eClaim.tblClaimAdmin = eClaimAdmin

            If FindClaimsB.GetClaimsCount(eClaim, imisgen.getUserId(Session("User"))) = 1 Then
                imisgen.Alert(imisgen.getMessage("M_CLAIMSEXCEEDLIMIT"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If

            Dim dtClaims As DataTable = FindClaimsB.GetClaims(eClaim, imisgen.getUserId(Session("User")))
            L_CLAIMSFOUND.Text = If(dtClaims.Rows.Count = 0, imisgen.getMessage("L_NO"), Format(dtClaims.Rows.Count, "#,###")) & " " & imisgen.getMessage("L_CLAIMSFOUND")
            gvClaims.DataSource = dtClaims
            gvClaims.SelectedIndex = 0
            gvClaims.DataBind()

            ButtonDisplayControl(gvClaims.Rows.Count)
            GetFilterCriteria()
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
    Private Sub ButtonDisplayControl(ByVal GridCount As Integer)

        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If GridCount > 0 Then
            If B_LOAD.Visible Or FindClaimsB.checkRights(IMIS_EN.Enums.Rights.ClaimLoad, UserID) Then
                B_LOAD.Visible = True
            End If
            If B_DELETE.Visible Or FindClaimsB.checkRights(IMIS_EN.Enums.Rights.ClaimDelete, UserID) Then
                B_DELETE.Visible = True
            End If
            If B_SUBMIT.Visible Or FindClaimsB.checkRights(IMIS_EN.Enums.Rights.ClaimSubmit, UserID) Then
                B_SUBMIT.Visible = True
                lblSelectToSubmit.Visible = True
                chkboxSubmitAll.Visible = True
            End If
        Else
            B_LOAD.Visible = False
            B_DELETE.Visible = False
            B_SUBMIT.Visible = False
            lblSelectToSubmit.Visible = False
            chkboxSubmitAll.Visible = False
        End If

    End Sub
    Protected Sub gvClaims_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvClaims.PageIndexChanging
        gvClaims.PageIndex = e.NewPageIndex
    End Sub
    Private Sub GetFilterCriteria()
        Dim dic As New Dictionary(Of String, String)
        dic.Add("RegionId", ddlRegion.SelectedValue)
        dic.Add("LocationId", ddlDistrict.SelectedValue)
        dic.Add("HFID", ddlHFCode.SelectedValue)
        dic.Add("CHFNo", txtCHFID.Text)
        dic.Add("ClaimCode", txtClaimCode.Text)
        dic.Add("HFName", txtHFName.Text)
        dic.Add("ReviewStatus", ddlReviewStatus.SelectedValue)
        dic.Add("FeedbackStatus", ddlFBStatus.SelectedValue)
        dic.Add("ClaimStatus", ddlClaimStatus.SelectedValue)
        ' dic.Add("ICDID", ddlICD.SelectedValue)
        dic.Add("ICDID", txtICDCode.Text)
        dic.Add("BatchRunID", ddlBatchRun.SelectedValue)
        dic.Add("VisitDateFrom", If(txtVisitDateFrom.Text = "", "", txtVisitDateFrom.Text))
        dic.Add("VisitDateTo", If(txtVisitDateTo.Text = "", "", txtVisitDateTo.Text))
        dic.Add("ClaimedDateFrom", If(txtClaimedDateFrom.Text = "", "", txtClaimedDateFrom.Text))
        dic.Add("ClaimedDateTo", If(txtClaimedDateTo.Text = "", "", txtClaimedDateTo.Text))
        dic.Add("ClaimAdminID", ddlClaimAdmin.SelectedValue)
        dic.Add("VisitType", ddlVisitType.SelectedValue)

        Session("FindClaimsFilter") = dic
    End Sub
    Private Sub B_ADD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_ADD.Click
        GetFilterCriteria()
        Session("DistrictIDFindClaims") = ddlDistrict.SelectedValue
        Session("HFID") = ddlHFCode.SelectedValue
        Session("RegionSelected") = ddlRegion.SelectedValue

        Dim HfUUID As Guid = hfBI.GetHfUUIDByID(ddlHFCode.SelectedValue)
        Dim ClaimAdminUUID As Guid = claimAdminBI.GetClaimAdminUUIDByID(ddlClaimAdmin.SelectedValue)

        Response.Redirect("Claim.aspx?h=" & HfUUID.ToString() & "&a=" & ClaimAdminUUID.ToString())
    End Sub
    Private Sub B_LOAD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_LOAD.Click
        GetFilterCriteria()
        Dim ClaimUUID As Guid = claimBI.GetClaimUUIDByID(hfClaimID.Value)
        Response.Redirect("Claim.aspx?c=" & ClaimUUID.ToString())
    End Sub
    Private Sub B_DELETE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DELETE.Click



        RunPageSecurity(1)
        Try
            eClaim.ClaimID = hfClaimID.Value
            ' eHF.HfID = hfHFID.Value
            ' eClaim.tblHF = eHF
            If FindClaimsB.IsClaimStatusChanged(eClaim) Then
                imisgen.Alert(imisgen.getMessage("M_CLAIMNOTENTEREDNODELETE", True), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
            eClaim.AuditUserID = imisgen.getUserId(Session("User"))

            If FindClaimsB.DeleteClaim(eClaim) Then
                loadgrid()
                lblMsg.Text = imisgen.getMessage("M_CLAIMDELETED")
                hfdeleteClaim.Value = 1
            End If
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        If Not HttpContext.Current.Request.QueryString("i") Is Nothing Then
            Response.Redirect("FindInsuree.aspx")
        Else
            Response.Redirect("Home.aspx")
        End If

    End Sub

    Private Sub B_SUBMIT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_SUBMIT.Click

        RunPageSecurity(2)
        Try
            Dim chkbox As CheckBox
            Dim dt As New DataTable
            Dim dr As DataRow
            Dim Submitflag As Boolean
            dt.Columns.Add("ClaimID", System.Type.GetType("System.Int32"))
            dt.Columns.Add("RowID", System.Type.GetType("System.Byte[]"))

            For Each r As GridViewRow In gvClaims.Rows
                chkbox = CType(r.Cells(8).FindControl("chkbgridSubmit"), CheckBox)
                If chkbox.Checked = True Then
                    dr = dt.NewRow
                    dr("ClaimID") = gvClaims.DataKeys(r.RowIndex).Values("ClaimID")
                    dr("RowID") = gvClaims.DataKeys(r.RowIndex).Values("RowID")
                    dt.Rows.Add(dr)
                    Submitflag = True
                End If
            Next
            Dim Submitted, Checked, Rejected, Changed, Failed, ItemsPassed, ServicesPassed, ItemsRejected, ServicesRejected As Integer
            If Submitflag = False Then Exit Sub
            FindClaimsB.SubmitClaims(dt, imisgen.getUserId(Session("User")), Submitted, Checked, Rejected, Changed, Failed, ItemsPassed, ServicesPassed, ItemsRejected, ServicesRejected)
            hfSubmitClaims.Value = "<h4><u>" & imisgen.getMessage("M_CLAIMSUBMITTED_") & "</u></h4>" & "<br>" &
                                    "<table><tr><td>" & imisgen.getMessage("M_SUBMITTED") & "</td><td>" & Submitted & "</td></tr><tr><td>" &
                                    imisgen.getMessage("M_CHECKED") & "</td><td>" & Checked & "</td></tr><tr><td>" & imisgen.getMessage("M_REJECTED") &
                                    "</td><td>" & Rejected & "</td></tr><tr><td>" & imisgen.getMessage("M_CHANGED") & "</td><td>" & Changed &
                                    "</td></tr><tr><td>" & imisgen.getMessage("M_FAILED") & "</td><td>" & Failed & "</td></tr><tr><td>" &
                                    imisgen.getMessage("M_ITEMSPASSED") & "</td><td>" & ItemsPassed & "</td></tr>" &
                                    "<tr><td>" & imisgen.getMessage("M_SERVICESPASSED") & "</td><td>" & ServicesPassed & "</td></tr><tr><td>" &
                                    imisgen.getMessage("M_ITEMSREJECTED") & "</td><td>" & ItemsRejected & "</td></tr><tr><td>" &
                                    imisgen.getMessage("M_SERVICESREJECTED") & "</td><td>" & ServicesRejected & "</td></tr></table>"

            If IMIS_Gen.offlineHF Then
                WriteToXml(dt)
            End If
            loadgrid()
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

    End Sub
    Private Sub WriteToXml(ByVal dt As DataTable)
        Try
            For Each row As DataRow In dt.Rows
                FindClaimsB.WriteToXml(row("ClaimID"))
            Next

            FindClaimsB.ZipXMLs()
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            'Return
            Throw New Exception(ex.Message)
        End Try

    End Sub

    Private Sub ddlDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged, ddlRegion.SelectedIndexChanged
        'Amani 04/12
        Session("DistrictIDFindClaims") = ddlDistrict.SelectedValue.ToString()
        HFCodeAndBatchRunBinding(imisgen.getUserId(Session("User")))
    End Sub

    Private Sub ddlHFCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHFCode.SelectedIndexChanged
        FillClaimAdminCodes()
    End Sub
    'Private Sub ClaimCodeTxtControl()
    '    If ddlHFCode.SelectedValue = 0 Then
    '        txtClaimCode.Enabled = False
    '    Else
    '        txtClaimCode.Enabled = True
    '    End If
    'End Sub
    'Private Sub ddlHFCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlHFCode.SelectedIndexChanged
    '    ClaimCodeTxtControl()
    '    AddButtonControl()
    'End Sub
    Private Sub FillDistricts()
        ddlDistrict.DataSource = FindClaimsB.GetDistricts(imisgen.getUserId(Session("User")), True, Val(ddlRegion.SelectedValue))
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        ''amani 04/12
        Session("RegionSelected") = ddlRegion.SelectedValue.ToString
        FillDistricts()

    End Sub

End Class
