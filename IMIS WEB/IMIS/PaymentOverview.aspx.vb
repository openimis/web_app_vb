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




Public Class PaymentOverview
    Inherits System.Web.UI.Page
    Private PaymentId As Integer = 0

    Dim BI As New IMIS_BI.PaymentBI
    Dim ePaymentDetails As New IMIS_EN.tblPaymentDetail
    Dim ePayment As New IMIS_EN.tblPayment
    Dim dt As New DataTable
    Protected imisgen As New IMIS_Gen
    Private Sub FormatForm(Optional ByVal status As Integer = 0, Optional ByVal history As Boolean = False)

        If status <> 4 Then
            BtnMatchPayment.Enabled = False
            BtnMatchPayment.Visible = False
            btnSaveEditedPaymentDetails.Enabled = False
            btnSaveEditedPaymentDetails.Visible = False
        Else
            BtnMatchPayment.Enabled = True
            BtnMatchPayment.Visible = True
            btnSaveEditedPaymentDetails.Enabled = True
            btnSaveEditedPaymentDetails.Visible = True
        End If
        If history = True Then
            btnSaveEditedPaymentDetails.Enabled = False
            btnSaveEditedPaymentDetails.Visible = False

        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load, chkLegacy.CheckedChanged
        If Session("User") Is Nothing Then Response.Redirect("Default.aspx")


        PaymentId = HttpContext.Current.Request.QueryString("p")
        With ePayment
            .PaymentID = PaymentId
            .InsuranceNumber = Nothing
            .OfficerCode = Nothing
            .PaymentStatus = -1
            .ProductCode = Nothing
            .Legacy = chkLegacy.Checked
            .LocationID = 0
            .dtPaymentStatus = BI.GetPayementStatus()
        End With

        If Page.IsPostBack = True Then Return
        Try

            FillHeader()
            Dim dtStage As New DataTable()
            dtStage = BI.GetPolicyType()
            ddlPolicyStage.DataSource = dtStage
            ddlPolicyStage.DataValueField = "TypeId"
            ddlPolicyStage.DataTextField = "Type"
            ddlPolicyStage.DataBind()

            loadGrid()

        Catch ex As Exception
            lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

            Return
        End Try

    End Sub
    Private Sub loadGrid() Handles chkLegacy.CheckedChanged, gvPaymentDetails.PageIndexChanged
        Dim dt As New DataTable
        dt = BI.LoadPayment(ePayment)
        gvPaymentDetails.DataSource = dt
        gvPaymentDetails.DataBind()
        L_PAYMENTDETAILS.Text = If(dt.Rows.Count = 0, imisgen.getMessage("L_NO"), Format(dt.Rows.Count, "#,###")) & " " & imisgen.getMessage("L_PAYMENTDETAILS")
        If dt.Rows.Count > 0 Then
            gvPaymentDetails.SelectedIndex = 0
            LoadDetails()

        End If
    End Sub
    Private Sub FillProducts()
        Dim dt As New DataTable
        dt = BI.GetProducts(imisgen.getUserId(Session("User")), True, Val(ddlRegion.SelectedValue), Val(ddlDistrict.SelectedValue))

        ddlProduct.DataSource = dt
        ddlProduct.DataValueField = "ProdId"
        ddlProduct.DataTextField = "ProductCode"

        ddlProduct.DataBind()
    End Sub
    Private Sub FillDistricts()
        Dim dtDistricts As DataTable = BI.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataSource = dtDistricts
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
        FillProducts()
    End Sub
    Private Sub FillHeader()


        Dim payment_id As Integer = HttpContext.Current.Request.QueryString("p")
        Dim entities = BI.getPayment(payment_id)

        FormatForm(If(entities.PaymentStatus IsNot Nothing, entities.PaymentStatus, 0))
        txtPhoneNo.Text = entities.PhoneNumber
        txtMatchedDate.Text = If(entities.MatchedDate Is Nothing, "", entities.MatchedDate)
        txtStatus.Text = If(entities.PaymentStatusName IsNot Nothing, entities.PaymentStatusName, "")
        txtInternalIdentifier.Text = payment_id
        txtPaymentOrigin.Text = If(entities.PaymentOrigin IsNot Nothing, entities.PaymentOrigin, "")
        txtExpectedAmounts.Text = If(entities.ExpectedAmount IsNot Nothing, entities.ExpectedAmount, "")
        txtReceivedAmount.Text = If(entities.ReceivedAmount IsNot Nothing, entities.ReceivedAmount, "")
        txtOfficerCode.Text = If(entities.OfficerCode IsNot Nothing, entities.OfficerCode, "")
        txtPaymentDate.Text = If(entities.PaymentDate IsNot Nothing, entities.PaymentDate, "")
        txtReceivedDate.Text = If(entities.ReceivedDate IsNot Nothing, entities.ReceivedDate, "")
        txtControlNo.Text = If(entities.ControlNumber IsNot Nothing, entities.ControlNumber, "")
        txtTransactionNo.Text = If(entities.TransactionNumber IsNot Nothing, entities.TransactionNumber, "")
        txtReceiptNo.Text = If(entities.ReceiptNo IsNot Nothing, entities.ReceiptNo, "")
    End Sub

    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click

        Response.Redirect(Session("parentUrl"))

    End Sub

    Private Sub BtnMatchPayment_Click(sender As Object, e As EventArgs) Handles BtnMatchPayment.Click

        Try
            Dim isMatched As Boolean
            ePayment.PaymentID = HttpContext.Current.Request.QueryString("p")
            Dim dtUser As New DataTable
            Dim AuditUserID As Integer
            dtUser = Session("User")
            AuditUserID = dtUser.Rows(0)("UserID").ToString
            isMatched = BI.MatchPayment(PaymentId, AuditUserID)
            lblMsg.Text = ""
            If isMatched Then
                lblMsg.Text = imisgen.getMessage("M_PAYMENTMATCHED")
            Else
                lblMsg.Text = imisgen.getMessage("M_NOTPAYMENTMATCHED")
            End If
            FillHeader()
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 2)
            Return
        End Try
    End Sub

    Protected Sub gvPaymentDetails_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvPaymentDetails.SelectedIndexChanged
        LoadDetails()
        FillHeader()
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)

        AddRowSelectToGridView(gvPaymentDetails)

        MyBase.Render(writer)
    End Sub

    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(8).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
            row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub

    Private Sub btnSaveEditedPaymentDetails_Click(sender As Object, e As EventArgs) Handles btnSaveEditedPaymentDetails.Click
        Try
            lblMsg.Text = ""
            If txtEditInsuranceNumber.Text = "" Or Val(ddlProduct.SelectedValue) = 0 Or ddlPolicyStage.SelectedValue = "" Then
                lblMsg.Text = imisgen.getMessage("V_SUMMARY", True)
                Return
            ElseIf Not BI.CheckCHFID(txtEditInsuranceNumber.Text) = True Then
                lblMsg.Text = imisgen.getMessage("M_NOTVALIDCHFNUMBER", True)
                Return
            End If
            Dim dt As DataTable

            ePaymentDetails.InsuranceNumber = txtEditInsuranceNumber.Text

            If ddlProduct.SelectedIndex > 0 Then
                ePaymentDetails.ProductCode = ddlProduct.SelectedItem.Text
            Else
                ePaymentDetails.ProductCode = Nothing
            End If
            If ddlPolicyStage.SelectedIndex > 0 Then
                ePaymentDetails.PolicyStage = ddlPolicyStage.SelectedValue
            Else
                ePaymentDetails.PolicyStage = Nothing
            End If
            ePaymentDetails.PaymentDetailID = Integer.Parse(gvPaymentDetails.SelectedDataKey.Values("PaymentDetailsID"))
            dt = Session("User")
            ePaymentDetails.AuditUserID = dt.Rows(0)("UserID").ToString
            BI.SaveEditedPaymentDetails(ePaymentDetails)
            dt = BI.LoadPayment(ePayment)

            gvPaymentDetails.DataSource = dt
            gvPaymentDetails.SelectedIndex = -1
            gvPaymentDetails.DataBind()

        Catch ex As Exception
            lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            ' imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Throw New ArgumentException(ex.Message)
            Return
        End Try
    End Sub
    Public Function ValidateEntry()
        Dim flag As Boolean = True


        If txtEditInsuranceNumber.Text = "" Or Nothing Then
            flag = False

        End If
        Return flag
    End Function

    Protected Sub gvPaymentDetails_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gvPaymentDetails.SelectedIndexChanging
        gvPaymentDetails.PageIndex = e.NewSelectedIndex
    End Sub

    Private Sub FillSelectedProduct(ByVal ProdID As Integer?, ByVal LocationType As String, ByVal LocationID As Integer?, ByVal ParentLocationID As Integer?)
        FillRegions()
        FillDistricts()

        If LocationType = "R" Then
            ddlRegion.SelectedValue = LocationID
            FillDistricts()
            ddlDistrict.SelectedIndex = 0
            FillProducts()
            ddlProduct.SelectedValue = ProdID
        ElseIf LocationType = "D" Then
            ddlRegion.SelectedValue = ParentLocationID
            ddlDistrict.SelectedValue = LocationID
            FillDistricts()
            FillProducts()
            ddlProduct.SelectedValue = ProdID
        Else

            FillProducts()
            ddlProduct.SelectedValue = ProdID

        End If

    End Sub

    Private Sub FillRegions()
        Dim dtRegions As DataTable = BI.GetRegions(imisgen.getUserId(Session("User")), True, False)
        ddlRegion.DataSource = dtRegions
        ddlRegion.DataValueField = "RegionId"
        ddlRegion.DataTextField = "RegionName"
        ddlRegion.DataBind()
        If dtRegions.Rows.Count = 1 Then
            FillDistricts()
            FillProducts()
        End If
    End Sub

    Protected Sub chkLegacy_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLegacy.CheckedChanged
        If chkLegacy.Checked = True Then
            B_VIEW.Visible = False
        Else
            B_VIEW.Visible = False
        End If

    End Sub

    Private Sub LoadDetails()
        Dim ePaymentDetail As New IMIS_EN.tblPaymentDetail

        Dim dt As DataTable
        Dim status As Int16

        With ePaymentDetail
            .PaymentDetailID = Integer.Parse(gvPaymentDetails.SelectedDataKey.Values("PaymentDetailsID"))

        End With

        dt = BI.LoadPaymentDetails(ePayment, ePaymentDetail.PaymentDetailID)


        status = dt.Rows(0)("PaymentStatus").ToString()
        Dim history As Boolean = False
        history = IIf(dt.Rows(0)("ValidityTo").ToString <> "", True, False)

        FormatForm(status, history)
        ePaymentDetail.Amount.ToString()

        If gvPaymentDetails.SelectedIndex >= 0 Then

            txtEditInsuranceNumber.Text = dt.Rows(0)("InsuranceNumber").ToString()

            ddlPolicyStage.SelectedValue = dt.Rows(0)("PolicyStage").ToString()

            If dt.Rows.Count > 0 Then
                Dim ProdId, LocationId, ParentLocationId As Integer?
                Dim LocationType As String
                ProdId = If(dt.Rows(0)("ProdID") Is DBNull.Value, 0, dt.Rows(0)("ProdID"))
                LocationId = If(dt.Rows(0)("LocationID") Is DBNull.Value, Nothing, dt.Rows(0)("LocationID"))
                ParentLocationId = If(dt.Rows(0)("ParentLocationID") Is DBNull.Value, Nothing, dt.Rows(0)("ParentLocationID"))
                LocationType = dt.Rows(0)("LocationType").ToString()
                FillSelectedProduct(ProdId, LocationType, LocationId, ParentLocationId)
                ddlProduct.SelectedValue = Val(ProdId)
            End If

        End If
    End Sub
End Class