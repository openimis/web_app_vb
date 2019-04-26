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

Partial Public Class ClaimFeedback
    Inherits System.Web.UI.Page
    Private efeedback As New IMIS_EN.tblFeedback
    Private eClaim As New IMIS_EN.tblClaim
    Private feedback As New IMIS_BI.ClaimFeedbackBI
    Protected imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Private Sub FormatForm()

        Dim Adjustibility As String = ""


        'ClaimAdministrator
        Adjustibility = General.getControlSetting("ClaimAdministrator")
        lblClaimAdminCode.Visible = Not (Adjustibility = "N")
        txtClaimAdminCode.Visible = Not (Adjustibility = "N")

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        eClaim.ClaimID = CType(Request.QueryString("c"), Integer)
        If IsPostBack Then Return
        RunPageSecurity()
        FormatForm()

        Try

            feedback.LoadClaim(eClaim)

            If Not eClaim.tblHF Is Nothing Then
                lblHFCodeData.Text = eClaim.tblHF.HFCode
            End If

            If Not eClaim.tblInsuree Is Nothing Then
                lblCHFIDData.Text = eClaim.tblInsuree.CHFID
                lblNameData.Text = eClaim.tblInsuree.LastName
            End If

            lblClaimIDData.Text = eClaim.ClaimCode
            lblClaimDateData.Text = Format(eClaim.DateClaimed, "dd/MM/yyyy")
            lblStartDateData.Text = If(eClaim.DateFrom = Nothing, String.Empty, eClaim.DateFrom)
            lblEndDateData.Text = If(eClaim.DateTo Is Nothing, String.Empty, eClaim.DateTo)

            If Not eClaim.ReviewStatus Is Nothing Then
                lblReviewStatusData.Text = feedback.ReturnReviewStatus(eClaim.ReviewStatus)
            End If

            If Not eClaim.FeedbackStatus Is Nothing Then
                lblFeedbackStatusData.Text = feedback.ReturnFeedbackStatus(eClaim.FeedbackStatus)
            End If

            If Not eClaim.ClaimStatus = Nothing Then
                lblClaimStatusData.Text = feedback.ReturnClaimStatus(eClaim.ClaimStatus)
            End If

            If Not eClaim.tblClaimAdmin.ClaimAdminCode Is Nothing Then
                txtClaimAdminCode.Text = eClaim.tblClaimAdmin.ClaimAdminCode.ToString.Trim
            End If

            If Not eClaim.tblInsuree.OtherNames Is Nothing Then
                lblOtherNamesData.Text = eClaim.tblInsuree.OtherNames
            End If

            If Not eClaim.tblHF.HFName Is Nothing Then
                lblHFNameData.text = eClaim.tblHF.HFName
            End If

            hfClaimAdminId.Value = eClaim.tblClaimAdmin.ClaimAdminId

            Dim feedback_ddl_source As New DataTable
            feedback_ddl_source = feedback.GetYesNo()

            ddlCareRendered.DataSource = feedback_ddl_source
            ddlCareRendered.DataValueField = "Code"
            ddlCareRendered.DataTextField = "Status"
            ddlCareRendered.DataBind()

            ddlDrugsPrescribed.DataSource = feedback_ddl_source
            ddlDrugsPrescribed.DataValueField = "Code"
            ddlDrugsPrescribed.DataTextField = "Status"
            ddlDrugsPrescribed.DataBind()

            ddlDrugsReceived.DataSource = feedback_ddl_source
            ddlDrugsReceived.DataValueField = "Code"
            ddlDrugsReceived.DataTextField = "Status"
            ddlDrugsReceived.DataBind()

            ddlPaymentAsked.DataSource = feedback_ddl_source
            ddlPaymentAsked.DataValueField = "Code"
            ddlPaymentAsked.DataTextField = "Status"
            ddlPaymentAsked.DataBind()

            Dim dic As Dictionary(Of String, String)
            dic = CType(Session("ClaimOverviewCriteria"), Dictionary(Of String, String))

            ddlEnrolmentOfficer.DataSource = feedback.GetOfficers(Val(dic("LocationId")), True)
            ddlEnrolmentOfficer.DataTextField = "Code"
            ddlEnrolmentOfficer.DataValueField = "OfficerId"
            ddlEnrolmentOfficer.DataBind()


            If Not eClaim.tblFeedback Is Nothing Then

                If Not eClaim.tblFeedback.CareRendered Is Nothing Then ddlCareRendered.SelectedValue = If(eClaim.tblFeedback.CareRendered, 1, 0)
                If Not eClaim.tblFeedback.DrugPrescribed Is Nothing Then ddlDrugsPrescribed.SelectedValue = If(eClaim.tblFeedback.DrugPrescribed, 1, 0)
                If Not eClaim.tblFeedback.DrugReceived Is Nothing Then ddlDrugsReceived.SelectedValue = If(eClaim.tblFeedback.DrugReceived, 1, 0)
                If Not eClaim.tblFeedback.PaymentAsked Is Nothing Then ddlPaymentAsked.SelectedValue = If(eClaim.tblFeedback.PaymentAsked, 1, 0)
                If Not eClaim.tblFeedback.FeedbackDate Is Nothing Then txtFeedbackDate.Text = eClaim.tblFeedback.FeedbackDate
                If Not eClaim.tblFeedback.CHFOfficerCode Is Nothing Then ddlEnrolmentOfficer.SelectedValue = eClaim.tblFeedback.CHFOfficerCode

                If Not eClaim.tblFeedback.Asessment Is Nothing Then rbOverallAsessmentLevels.SelectedValue = eClaim.tblFeedback.Asessment


                hdnFeedbackID.Value = eClaim.tblFeedback.FeedbackID
            End If

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

    End Sub

    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.ClaimFeedback, Page) Then
            pnlBody.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.ClaimFeedback, UserID)

            If Not pnlBody.Enabled Then
                B_SAVE.Visible = False
            End If
        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.ClaimFeedback.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub

    Protected Sub B_SAVE_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_SAVE.Click

        Try

            efeedback.AuditUserID = imisgen.getUserId(Session("user"))
            efeedback.FeedbackID = CInt(hdnFeedbackID.Value)
            efeedback.tblClaim1 = eClaim
            efeedback.CHFOfficerCode = ddlEnrolmentOfficer.SelectedValue
            efeedback.CareRendered = CType(ddlCareRendered.SelectedValue, Boolean)
            efeedback.PaymentAsked = CType(ddlPaymentAsked.SelectedValue, Boolean)
            efeedback.DrugPrescribed = CType(ddlDrugsPrescribed.SelectedValue, Boolean)
            efeedback.DrugReceived = CType(ddlDrugsReceived.SelectedValue, Boolean)
            efeedback.FeedbackDate = Date.Parse(txtFeedbackDate.Text)
            efeedback.Asessment = rbOverallAsessmentLevels.SelectedValue
          

            If feedback.SaveFeedback(efeedback) = 1 Then
                Session("Msg") = imisgen.getMessage("M_FEEDBACKSAVEDSUCCESSFULLY")
            Else
                Session("Msg") = imisgen.getMessage("M_FEEDBACKUPDATEDSUCCESSFULLY")
            End If


        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
        Response.Redirect("ClaimOverview.aspx?c=" & eClaim.ClaimID)
    End Sub

    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("ClaimOverview.aspx?c=" & eClaim.ClaimID)
    End Sub
End Class
