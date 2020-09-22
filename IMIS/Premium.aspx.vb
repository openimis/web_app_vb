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

Partial Public Class Premium
    Inherits System.Web.UI.Page
    Dim eFamily As New IMIS_EN.tblFamilies
    Dim ePremium As New IMIS_EN.tblPremium
    Dim ePolicy As New IMIS_EN.tblPolicy
    Dim ePayer As New IMIS_EN.tblPayer
    Dim eProduct As New IMIS_EN.tblProduct
    Dim Premium As New IMIS_BI.PremiumBI
    Protected imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private PremiumContribution As Decimal
    Private familyBI As New IMIS_BI.FamilyBI
    Private policyBI As New IMIS_BI.PolicyBI
    Private ActivationOption As New IMIS_BL.EscapeBL
    Private Sub FormatForm()

        Dim Adjustibility As String = ""

       
        'Confirmation
        Adjustibility = General.getControlSetting("Confirmation")
        L_CONFIRMATIONNO0.Visible = Not (Adjustibility = "N")
        txtConfirmationType.Visible = Not (Adjustibility = "N")

        Adjustibility = General.getControlSetting("ConfirmationNo")
        L_CONFIRMATIONNO.Visible = Not (Adjustibility = "N")
        txtConfirmationNo1.Visible = Not (Adjustibility = "N")

      
        'Permanent Address
        Adjustibility = General.getControlSetting("PermanentAddress")
        L_ADDRESS0.Visible = Not (Adjustibility = "N")
        txtPermanentAddress.Visible = Not (Adjustibility = "N")

        'Poverty
        Adjustibility = General.getControlSetting("Poverty")
        lblPoverty.Visible = Not (Adjustibility = "N")
        txtPoverty.Visible = Not (Adjustibility = "N")

        'Contribution Category
        Adjustibility = General.getControlSetting("ContributionCategory")
        trContributionCategory.Visible = Not (Adjustibility = "N")
        RequiredFieldTypeOfPayment0.Enabled = (Adjustibility = "M")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()
        Try

            If HttpContext.Current.Request.QueryString("po") IsNot Nothing Then
                ePolicy.PolicyUUID = Guid.Parse(HttpContext.Current.Request.QueryString("po"))
                ePolicy.PolicyID = policyBI.GetPolicyIdByUUID(ePolicy.PolicyUUID)
                hfPolicyID.Value = ePolicy.PolicyID
            End If

            If HttpContext.Current.Request.QueryString("f") IsNot Nothing Then
                eFamily.FamilyUUID = Guid.Parse(HttpContext.Current.Request.QueryString("f"))
                eFamily.FamilyID = familyBI.GetFamilyIdByUUID(eFamily.FamilyUUID)
            End If

            If HttpContext.Current.Request.QueryString("p") IsNot Nothing Then
                ePremium.PremiumUUID = Guid.Parse(HttpContext.Current.Request.QueryString("p"))
                ePremium.PremiumId = Premium.GetPremiumIdByUUID(ePremium.PremiumUUID)
            End If

            If Request.Form("__EVENTTARGET") = B_SAVE.ClientID Then
                B_SAVE_Click(sender, New System.EventArgs)
            End If

            If IsPostBack = True Then Return

            FormatForm()

            'If imisgen.RunPageSecurity(IMIS_EN.Enums.Pages.Premium, Page) Then
            '    Dim RefUrl = Request.Headers("Referer")
            '    Dim reg As New Regex("Find", RegexOptions.IgnoreCase)
            '    If Not reg.IsMatch(RefUrl) Then
            '        Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Premium.ToString & "&retUrl=" & RefUrl)
            '    End If
            '    B_CANCEL.Enabled = True
            '    pnlButtons.Enabled = True
            'End If
            ddlTypeOfPayment.DataSource = Premium.GetTypeOfPayment(True)
            ddlTypeOfPayment.DataTextField = "PayType"
            ddlTypeOfPayment.DataValueField = "Code"
            ddlTypeOfPayment.DataBind()

            ddlCategory.DataSource = Premium.GetCategory
            ddlCategory.DataTextField = "Category"
            ddlCategory.DataValueField = "CategoryId"
            ddlCategory.DataBind()

            Premium.GetFamilyHeadInfo(eFamily)
            txtRegion.Text = eFamily.RegionName
            txtDistrict.Text = eFamily.DistrictName
            txtVillage.Text = eFamily.VillageName
            txtWard.Text = eFamily.WardName
            txtPoverty.Text = If(eFamily.Poverty Is Nothing, "", If(eFamily.Poverty = True, "Yes", "No"))
            txtHeadCHFID.Text = eFamily.tblInsuree.CHFID
            txtHeadLastName.Text = eFamily.tblInsuree.LastName
            txtHeadOtherNames.Text = eFamily.tblInsuree.OtherNames
            txtConfirmationType.Text = eFamily.ConfirmationType
            txtConfirmationNo1.Text = eFamily.ConfirmationNo
            'txtHeadPhone.Text = eFamily.tblInsuree.Phone
            FillCombobox()
            hfLastDate.Value = Premium.GetLastDateForPayment(ePolicy.PolicyID)
            Dim dt As DataTable = Premium.GetInstallmentsInfo(ePolicy.PolicyID)
            If dt.Rows.Count > 0 Then
                hfTotalInstallments.Value = dt.Rows(0)("TotalInstallments")

                If dt.Rows(0)("MaxInstallments") Is DBNull.Value OrElse dt.Rows(0)("MaxInstallments") = 0 Then
                    hfMaxInstallments.Value = Integer.MaxValue
                Else
                    hfMaxInstallments.Value = dt.Rows(0)("MaxInstallments")
                End If

                'hfMaxInstallments.Value = If(dt.Rows(0)("MaxInstallments") = 0, Integer.MaxValue, dt.Rows(0)("MaxInstallments"))
            End If
            ePremium.tblPolicy = ePolicy
            If Not ePremium.PremiumId = 0 Then
                Premium.LoadPremium(ePremium, PremiumContribution)
                ddlPayer.SelectedValue = ePremium.tblPayer.PayerID
                txtPremiumPaid.Text = FormatNumber(ePremium.Amount)
                txtReceiptNumber.Text = ePremium.Receipt

                txtPaymentDate.Text = FormatDateTime(ePremium.PayDate.ToString, DateFormat.ShortDate)
                ddlTypeOfPayment.SelectedValue = ePremium.PayType
                ddlPayer.SelectedValue = ePremium.tblPayer.PayerID.ToString
                hfPremiumContribution.Value = PremiumContribution - ePremium.Amount

                If ePremium.isPhotoFee = True Then
                    ddlCategory.SelectedValue = "P"
                Else
                    ddlCategory.SelectedValue = "C"
                End If

                If Not ePremium.tblPolicy.PolicyValue Is Nothing Then
                    hfPolicyValue.Value = ePremium.tblPolicy.PolicyValue
                End If

                If Not ePremium.tblPolicy.PolicyStatus Is Nothing Then
                    hfPolicyStatus.Value = ePremium.tblPolicy.PolicyStatus
                End If
                hfPolicyIsOffline.Value = if(ePremium.tblPolicy.isOffline Is Nothing, False, ePremium.tblPolicy.isOffline)
                hfPremiumIsOffline.Value = if(ePremium.isOffline Is Nothing, False, ePremium.isOffline)

                If ePremium.ValidityTo.HasValue Or ((IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF) And Not if(ePremium.isOffline Is Nothing, False, ePremium.isOffline)) Then
                    pnlBody.Enabled = False
                    B_SAVE.Visible = False
                End If

            Else
                Premium.GetPremiumContribution(ePremium, PremiumContribution)
                hfPolicyIsOffline.Value = Premium.GetPolicyOfflineValue(ePolicy.PolicyID)
            End If

            hfPremiumContribution.Value = PremiumContribution - ePremium.Amount
            txtPremiumContribution.Text = FormatNumber(PremiumContribution)


            If Not ePremium.tblPolicy.PolicyValue Is Nothing Then
                hfPolicyValue.Value = ePremium.tblPolicy.PolicyValue
                txtPolicyValue.Text = FormatNumber(ePremium.tblPolicy.PolicyValue)
            End If

            If Not ePremium.tblPolicy.PolicyStatus Is Nothing Then
                hfPolicyStatus.Value = ePremium.tblPolicy.PolicyStatus
                txtPolicyStatus.Text = Premium.ReturnPolicyStatus(ePremium.tblPolicy.PolicyStatus)
            End If

            If Not ePremium.tblPolicy.EffectiveDate Is Nothing Then
                hfPolicyEffectiveDate.Value = ePremium.tblPolicy.EffectiveDate
            End If

            hfPolicyStartDate.Value = ePremium.tblPolicy.StartDate

            txtBalance.Text = FormatNumber(ePremium.tblPolicy.PolicyValue - PremiumContribution)

            If Not ePremium.tblPolicy.tblProduct Is Nothing Then
                hfInsurancePeriod.Value = ePremium.tblPolicy.tblProduct.InsurancePeriod
            End If

            getGridData()


        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 1)
            Return
        End Try
    End Sub
    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Premium, Page) Then
            B_SAVE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ContributionEdit, UserID) Or userBI.checkRights(IMIS_EN.Enums.Rights.ContributionAdd, UserID)
            If Not B_SAVE.Visible Then
                pnlBody.Enabled = False
                L_FAMILYPANEL.Enabled = False
            End If
        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Premium.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub

    Private Sub FillCombobox()
        FillPayers()
    End Sub

    Private Sub getGridData()

        ePremium.PolicyID = hfPolicyID.Value

        gvPremium.DataSource = Premium.GetPremium(ePremium)
        gvPremium.DataBind()

    End Sub

    Private Sub FillPayers()
        ddlPayer.DataSource = Premium.GetPayers(eFamily.RegionId, eFamily.DistrictID, imisgen.getUserId(Session("User")), True)
        ddlPayer.DataValueField = "PayerID"
        ddlPayer.DataTextField = "PayerName"
        ddlPayer.DataBind()
    End Sub
    Private Sub B_SAVE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_SAVE.Click
        If CType(Me.Master.FindControl("hfDirty"), HiddenField).Value = True Then
            Try
                ePolicy.PolicyID = CInt(hfPolicyID.Value)
                ePolicy.isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
                ePolicy.StartDate = hfPolicyStartDate.Value

                ePayer.PayerID = ddlPayer.SelectedValue
                ' EMIS: 15223 (ePremium.Amount = val(txtPremiumPaid.Text)
                ePremium.Amount = txtPremiumPaid.Text
                ePremium.Receipt = txtReceiptNumber.Text
                ePremium.PayDate = Date.ParseExact(txtPaymentDate.Text, "dd/MM/yyyy", Nothing)
                ePremium.PayType = ddlTypeOfPayment.SelectedValue
                ePremium.isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
                ePremium.AuditUserID = imisgen.getUserId(Session("User"))
                ' EMIS: 15329 (To Capture AuditUserId in Policy update )
                ePolicy.AuditUserID = ePremium.AuditUserID
                ePremium.tblPolicy = ePolicy
                ePremium.tblPayer = ePayer
                If ddlCategory.SelectedValue = "P" Then
                    ePremium.isPhotoFee = 1
                Else
                    ePremium.isPhotoFee = 0
                End If

                Dim PayDate As Date = Date.ParseExact(txtPaymentDate.Text, "dd/MM/yyyy", Nothing)
                Dim StartDate As Date = Date.ParseExact(hfPolicyStartDate.Value, "dd/MM/yyyy", Nothing)
                Dim EffectiveDate As Date = if(PayDate < StartDate, StartDate, PayDate)
                If ePremium.PayDate > System.DateTime.Now Then
                    lblMsg.Text = imisgen.getMessage("M_PAYDATETOEXCEEDCURRENDATE")
                    Return
                End If

                Dim ActivationValue As Integer = ActivationOption.getActivationOption()

                If ddlCategory.SelectedValue = "C" Or ddlCategory.SelectedValue = "" Then
                    Select Case Request.Form("__EVENTARGUMENT_PREMIUM") 'Coming from js save button click function
                        Case 4
                            Select Case Request.Form("__EVENTARGUMENT")
                                Case imisgen.getMessage("L_ENFORCE")
                                    If ActivationValue = 3 Then
                                        ePolicy.PolicyStatus = 16
                                    Else
                                        ePolicy.PolicyStatus = 2
                                    End If
                                    ePolicy.EffectiveDate = EffectiveDate
                            End Select

                        Case 5
                            Select Case Request.Form("__EVENTARGUMENT")
                                Case imisgen.getMessage("L_SUSPEND")
                                    ePolicy.PolicyStatus = 4
                                Case imisgen.getMessage("L_ENFORCE")
                                    If ActivationValue = 3 Then
                                        ePolicy.PolicyStatus = 16
                                    Else
                                        ePolicy.PolicyStatus = 2
                                    End If
                                    If hfPolicyEffectiveDate.Value.Trim = String.Empty Then
                                        ePolicy.EffectiveDate = EffectiveDate
                                    Else
                                        ePolicy.EffectiveDate = Date.ParseExact(hfPolicyEffectiveDate.Value.Trim, "dd/MM/yyyy", Nothing)
                                    End If
                            End Select
                        Case 6
                            If Request.Form("__EVENTARGUMENT") = "ActivateInsuree" Then
                                If txtPolicyStatus.Text.Trim <> imisgen.getMessage("T_IDLE") Then
                                    ePolicy.EffectiveDate = EffectiveDate
                                ElseIf txtPolicyStatus.Text.Trim = imisgen.getMessage("T_IDLE") Then
                                    ePolicy.EffectiveDate = EffectiveDate
                                    If ActivationValue = 3 Then
                                        ePolicy.PolicyStatus = 16
                                    Else
                                        ePolicy.PolicyStatus = 2
                                    End If
                                End If
                            End If
                    End Select
                End If

                ePolicy.isOffline = CBool(hfPolicyIsOffline.Value)

                ePremium.tblPolicy = ePolicy


                If Premium.isUniqueReceipt(ePremium) = False Then
                    imisgen.Alert(imisgen.getMessage("M_DUPLICATERECEIPT"), pnlBody, alertPopupTitle:="IMIS")
                    Return
                End If

                Dim chk As Integer = Premium.SavePremium(ePremium, IMIS_Gen.offlineHF)
                If chk = 0 Then
                    Session("msg") = imisgen.getMessage("L_PREMIUM") & " " & imisgen.getMessage("M_Inserted")
                ElseIf chk = 1 Then
                    Session("msg") = imisgen.getMessage("L_PREMIUM") & " " & imisgen.getMessage("M_Updated")
                ElseIf chk = 2 Then
                    lblMsg.Text = imisgen.getMessage("M_POLICYNOTADDED")
                    Return
                End If

            Catch ex As Exception
                'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 2)
                Return
            End Try
        End If
        Dim PremiumUUID As Guid
        PremiumUUID = Premium.GetPremiumnUUIDByID(ePremium.PremiumId)
        Response.Redirect("OverviewFamily.aspx?f=" & eFamily.FamilyUUID.ToString() & "&p=" & PremiumUUID.ToString() & "&po=" & ePolicy.PolicyUUID.ToString())
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        If B_SAVE.Visible = False Then 'HttpContext.Current.Request.QueryString("po") Is Nothing
            Response.Redirect("FindPremium.aspx?p=" & ePremium.PremiumUUID.ToString())
        Else
            Response.Redirect("OverviewFamily.aspx?f=" & eFamily.FamilyUUID.ToString() & "&p=" & ePremium.PremiumUUID.ToString() & "&po=" & ePolicy.PolicyUUID.ToString())
        End If
    End Sub
End Class
