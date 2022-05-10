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




Partial Public Class FindPayment
    Inherits System.Web.UI.Page


    Public payment_id As String
    Private ePayment As New IMIS_EN.tblPayment
    Private Payment As New IMIS_BI.PaymentBI
    Protected imisgen As New IMIS_Gen
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvPayments)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
            If Not row.Cells(12).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If

        Next
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RunPageSecurity()
        Try

            If Not IsPostBack = True Then

                Dim dtRegions As DataTable = Payment.GetRegions(imisgen.getUserId(Session("User")), True)
                ddlRegion.DataSource = dtRegions
                ddlRegion.DataValueField = "RegionId"
                ddlRegion.DataTextField = "RegionName"
                ddlRegion.DataBind()
                If dtRegions.Rows.Count = 1 Then
                    FillDistrict()
                End If

                FillPaymentStatus()
                Session("ParentUrl") = "FindPayment.aspx"
            End If

        Catch ex As Exception
            lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub FillPaymentStatus()
        ddlPaymentStatus.DataSource = Payment.GetPayementStatus(True)
        ddlPaymentStatus.DataTextField = "PaymenyStatusName"
        ddlPaymentStatus.DataValueField = "StatusID"
        ddlPaymentStatus.DataBind()

    End Sub

    Private Sub FillDistrict()
        Dim dtDistricts As DataTable = Payment.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataSource = dtDistricts
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub

    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If Payment.RunPageSecurity(IMIS_EN.Enums.Pages.FindPayment, Page) Then
            B_VIEW.Enabled = Payment.checkRights(IMIS_EN.Enums.Rights.PaymentSearch, UserID)
        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindPayment.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub loadSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim AllowEdit As Boolean = Payment.RunPageSecurity(IMIS_EN.Enums.Pages.OverviewFamily, Page)

    End Sub
    Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click

        If gvPayments.SelectedDataKey Is Nothing Then
            Response.Redirect("Premium.aspx?f=0&p=0")
        End If

        Response.Redirect("Premium.aspx?f=" & gvPayments.SelectedDataKey.Values("FamilyID") & "&p=" & gvPayments.SelectedDataKey.Values("PremiumID") & "&po=" & gvPayments.SelectedDataKey.Values("PolicyID"))
    End Sub
    Protected Sub gvProducts_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPayments.PageIndexChanging
        gvPayments.PageIndex = e.NewPageIndex
    End Sub
    Private Sub LoadGrid() Handles btnSearch.Click, chkLegacy.CheckedChanged, gvPayments.PageIndexChanged
        loadSecurity()
        Dim eFamily As New IMIS_EN.tblFamilies
        Dim eInsuree As New IMIS_EN.tblInsuree
        Dim PaymentStatus As Integer?

        If Val(ddlPaymentStatus.SelectedIndex.ToString()) > 0 Then
            PaymentStatus = Val(ddlPaymentStatus.SelectedValue)

        End If

        With ePayment
            .InsuranceNumber = If(txtInsuranceNumber.Text.Trim = "", Nothing, txtInsuranceNumber.Text.Trim)
            .PhoneNumber = If(txtPhoneNumber.Text.Trim = "", Nothing, txtPhoneNumber.Text.Trim)
            .OfficerCode = If(txtOfficeCode.Text.Trim = "", Nothing, txtOfficeCode.Text.Trim)
            .ReceiptNo = If(txtReceiptNo.Text.Trim = "", Nothing, txtReceiptNo.Text.Trim)
            .PaymentStatus = PaymentStatus
            .ProductCode = If(txtProductCode.Text.Trim = "", Nothing, txtProductCode.Text.Trim)
            .Legacy = chkLegacy.Checked
            .dtPaymentStatus = Payment.GetPayementStatus(False)
            .ControlNumber = If(txtControlNumber.Text.Trim = "", Nothing, txtControlNumber.Text.Trim)
            .TransactionNumber = If(txtTransactionNumber.Text.Trim = "", Nothing, txtTransactionNumber.Text.Trim)
            .PaymentOrigin = If(txtPaymentOrigin.Text.Trim = "", Nothing, txtPaymentOrigin.Text.Trim)

        End With
        If IsNumeric(txtReceivedAmountFrom.Text.Trim) Then
            ePayment.ReceivedAmountFrom = Val(txtReceivedAmountFrom.Text.Trim)
        End If
        If IsNumeric(txtReceivedAmountTo.Text.Trim) Then
            ePayment.ReceivedAmountTO = Val(txtReceivedAmountTo.Text.Trim)
        End If
        ePayment.AuditUserID = imisgen.getUserId(Session("User"))
        If Trim(txtDateOfPaymentFrom.Text.Trim).Length > 0 Then
            If IsDate(txtDateOfPaymentFrom.Text.Trim) Then
                ePayment.DateFrom = Date.Parse(txtDateOfPaymentFrom.Text.Trim)
            Else
                imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If

        If Trim(txtDateOfPaymentTo.Text.Trim).Length > 0 Then
            If IsDate(txtDateOfPaymentTo.Text.Trim) Then
                ePayment.DateTo = Date.Parse(txtDateOfPaymentTo.Text.Trim)
            Else
                imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If
        If Trim(txtReceivingDateFrom.Text.Trim).Length > 0 Then
            If IsDate(txtReceivingDateFrom.Text.Trim) Then
                ePayment.ReceivingDateFrom = Date.Parse(txtReceivingDateFrom.Text.Trim)
            Else
                imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If
        If Trim(txtReceivingDateTo.Text.Trim).Length > 0 Then
            If IsDate(txtReceivingDateTo.Text.Trim) Then
                ePayment.ReceivingDateTo = Date.Parse(txtReceivingDateTo.Text.Trim)
            Else
                imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If

        If Trim(txtMatchingDateFrom.Text.Trim).Length > 0 Then
            If IsDate(txtMatchingDateFrom.Text.Trim) Then
                ePayment.MatchDateFrom = Date.Parse(txtMatchingDateFrom.Text.Trim)
            Else
                imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If
        If Trim(txtMatchingDateTo.Text.Trim).Length > 0 Then
            If IsDate(txtMatchingDateTo.Text.Trim) Then
                ePayment.MatchedDateTo = Date.Parse(txtMatchingDateTo.Text.Trim)
            Else
                imisgen.Alert(imisgen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If
        End If

        ePayment.RegionId = Val(ddlRegion.SelectedValue)

        If chkReconciled.Checked Then
            ePayment.SpReconcReqId = "1"
        Else
            ePayment.SpReconcReqId = ""
        End If

        If Val(ddlDistrict.SelectedValue) > 0 Then ePayment.DistrictId = ddlDistrict.SelectedValue




        Dim dtPayment As DataTable = Payment.getPayment(ePayment)
        L_FOUNDPAYMENTS.Text = If(dtPayment.Rows.Count = 0, imisgen.getMessage("L_NO"), Format(dtPayment.Rows.Count, "#,###")) & " " & imisgen.getMessage("L_FOUNDPAYMENTS")
        gvPayments.DataSource = dtPayment
        gvPayments.SelectedIndex = -1
        gvPayments.DataBind()
        DisableButtonsOnEmptyRows(gvPayments)
    End Sub
    Private Sub DisableButtonsOnEmptyRows(ByRef gv As GridView)
        If gv.Rows.Count = 0 Then
            B_VIEW.Enabled = False
        End If
    End Sub
    Protected Sub chkLegacy_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLegacy.CheckedChanged
        If chkLegacy.Checked = True Then
            B_VIEW.Visible = False
        Else
            B_VIEW.Visible = False
        End If
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub

    Protected Sub gvPayments_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvPayments.SelectedIndexChanged
        If gvPayments.SelectedDataKey Is Nothing Then
            Response.Redirect("PaymentOverview.aspx?p=0")
        End If
        payment_id = gvPayments.SelectedDataKey.Values("PaymentUUID").ToString()
        Response.Redirect("PaymentOverview.aspx?p=" & payment_id)
    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        If Val(ddlRegion.SelectedValue) > 0 Then
            FillDistrict()
        Else
            ddlDistrict.Items.Clear()
        End If

    End Sub
End Class