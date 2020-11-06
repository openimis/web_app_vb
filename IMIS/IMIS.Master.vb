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

Imports IMIS_BI
Imports IMIS_EN

Public Class IMIS
    Inherits System.Web.UI.MasterPage
    Private MasterBI As New IMIS_BI.MasterBI
    Protected Friend imisgen As New IMIS_Gen
    Private Sub FormatForm()
        Dim Adjustibility As String = ""
        For i As Integer = 0 To gvPolicy.Columns.Count
            If gvPolicy.Columns(i).HeaderText.Equals(imisgen.getMessage("L_PRODUCTCODE")) Then gvPolicy.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvPolicy.Columns(i).HeaderText.Equals(imisgen.getMessage("L_EXPIREDATE")) Then gvPolicy.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvPolicy.Columns(i).HeaderText.Equals(imisgen.getMessage("L_STATUS")) Then gvPolicy.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvPolicy.Columns(i).HeaderText.Equals(imisgen.getMessage("L_HDEDUCTION")) Then gvPolicy.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvPolicy.Columns(i).HeaderText.Equals(imisgen.getMessage("L_NHDEDUCTION")) Then gvPolicy.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvPolicy.Columns(i).HeaderText.Equals(imisgen.getMessage("L_HCEILING")) Then gvPolicy.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvPolicy.Columns(i).HeaderText.Equals(imisgen.getMessage("L_NHCEILING")) Then gvPolicy.Columns(i).Visible = Not (Adjustibility = "N") : Exit For

        Next
        For i As Integer = 0 To gvProduct.Columns.Count
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_TOTALADMISSIONSLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_TOTALVISITSLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_TOTALCONSULTATIONSLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_TOTALSURGERIESLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_TOTALDELIVERIESLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_TOTALANTENATALLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_HOSPITALIZATIONAMOUNTLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_CONSULTATIONAMOUNTLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_SURGERYAMOUNTLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_DELIVERYAMOUNTLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For
            If gvProduct.Columns(i).HeaderText.Equals(imisgen.getMessage("L_ANTENATALAMOUNTLEFT")) Then gvProduct.Columns(i).Visible = Not (Adjustibility = "N") : Exit For

        Next

        'ItemCode
        Adjustibility = General.getControlSetting("lblItemCode")
        lblItemCode.Visible = Not (Adjustibility = "N")

        'Item Left
        Adjustibility = General.getControlSetting("lblItemLeft")
        lblItemLeft.Visible = Not (Adjustibility = "N")

        'Service Left
        Adjustibility = General.getControlSetting("lblServiceLeft")
        lblServiceLeft.Visible = Not (Adjustibility = "N")

        'Service MidDate
        Adjustibility = General.getControlSetting("lblServiceMinDate")
        lblServiceMinDate.Visible = Not (Adjustibility = "N")

        'Item MidDate
        Adjustibility = General.getControlSetting("lblItemMinDate")
        lblItemMinDate.Visible = Not (Adjustibility = "N")
    End Sub



    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load, rptInsuree.ItemDataBound
        FormatForm()

        Dim Referer As String = Request.ServerVariables("HTTP_REFERER")
        If String.IsNullOrEmpty(Referer) Then
            If Not Request.QueryString("x") = 1 Then
                Server.Transfer("Redirected.htm")
            End If
        End If

        Dim lbl As Label = FindControl("FooterID").FindControl("Footer").FindControl("lblMsg")
        If lbl Is Nothing Then
            lbl = New Label
            lbl.ID = "lblMsg"
            FindControl("FooterID").FindControl("Footer").Controls.Add(lbl)
        End If

        If Session("User") Is Nothing Then Response.Redirect("Default.aspx")
        If Not Session("Msg") Is Nothing Then
            If Session("Msg").ToString.Length > 0 Then

                If Trim(lbl.Text).Length > 0 Then
                    lbl.Text = lbl.Text & Session("Msg")
                Else
                    lbl.Text = Session("Msg") & "<br/>"
                End If

                Session("Msg") = ""
            Else
                If lbl.Text.Length = 0 Then
                    Session("Msg") = ""
                    lbl.Text = ""
                End If
            End If

        Else
            If lbl.Text.Length = 0 Then
                Session("Msg") = ""
                lbl.Text = ""
            End If
        End If


        If Not IsPostBack = True Then
            Loadsecurity()
        End If

        If ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack() Then
            Dim js As String = "<script type='text/javascript'> $(document).ready(function(){ if( typeof(BindEventsonRowAdd) == 'function' )BindEventsonRowAdd();  }); </script>"
            ScriptManager.RegisterStartupScript(btnSearch, Me.GetType(), "insureePopup", js, False)
        End If
        Page.Header.DataBind()

        If IsPostBack Then Exit Sub

        'Dim eDefaults As New IMIS_EN.tblIMISDefaults
        'If IMIS_Gen.OfflineCHF = True Or IMIS_Gen.offlineHF = True Then
        '    MasterBI.GetDefaults(eDefaults)
        'End If

        If IMIS_Gen.offlineHF = True Then
            If IMIS_Gen.HFCode Is Nothing Then
                Dim eHF As New IMIS_EN.tblHF
                eHF.HfID = IMIS_Gen.HFID
                MasterBI.getHFCodeAndName(eHF)
                IMIS_Gen.HFCode = eHF.HFCode
                IMIS_Gen.HFName = eHF.HFName
            End If
            lblinfo.Text = "OFF-LINE HF " & IMIS_Gen.HFCode & ":" & IMIS_Gen.HFName
            FooterID.Style.Add("background", "#FFCFC4")
            lblinfo.Style.Add("color", "Red")
            'lblinfo.ForeColor = Drawing.Color.Red
        ElseIf IMIS_Gen.OfflineCHF = True Then
            lblinfo.Text = "OFF-LINE | Scheme: " & IMIS_Gen.CHFID 'eDefaults.OfflineCHF
            FooterID.Style.Add("background", "#DBFFB7")
            lblinfo.Style.Add("color", "Red")
        Else
            lblinfo.Text = ""
            'lblinfo.ForeColor = Drawing.Color.Green
        End If

    End Sub

    Private Sub Loadsecurity()
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        SubAddFamily.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.FamilyAdd, UserID)
        SubFindfamily.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.FamilySearch, UserID)
        SubFindInsuree.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.InsureeSearch, UserID)
        SubFindPolicy.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.PolicySearch, UserID)
        SubFindPremium.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.ContributionSearch, UserID)
        SubFindPayment.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.PaymentSearch, UserID)

        ''Claims 
        SubClaimOverview.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Claims, UserID)
        SubReview.Enabled = (MasterBI.checkRights(IMIS_EN.Enums.Rights.ClaimReview, UserID) Or MasterBI.checkRights(IMIS_EN.Enums.Rights.ClaimFeedback, UserID) Or MasterBI.checkRights(IMIS_EN.Enums.Rights.ClaimProcess, UserID) Or MasterBI.checkRights(IMIS_EN.Enums.Rights.ClaimUpdate, UserID))
        SubBatchRun.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Batch, UserID)

        '' Administration
        SubProducts.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Product, UserID)
        SubHF.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.HealthFacility, UserID)
        SubPriceList.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.PriceListMedicalItems, UserID)
        SubPLMS.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.PriceListMedicalServices, UserID)
        SubPLMI.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.PriceListMedicalItems, UserID)
        SubMS.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.MedicalService, UserID)
        SUBMI.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.MedicalItem, UserID)
        SubUser.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Users, UserID)
        SubUserProfile.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.userProfiles, UserID) And Not (IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF)
        subOfficer.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Officer, UserID)
        SubClaimAdministrator.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.ClaimAdministrator, UserID)
        subPayer.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Payer, UserID)
        subLocation.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Locations, UserID)


        '' Tools
        UploadICD.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Registers, UserID)
        subPolicyRenewal.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.PolicyRenew, UserID)
        subFeedbackPrompt.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.ClaimReview, UserID)
        If UserID = 8 And (IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF) Then
            subIMISExtracts.Enabled = True
        Else
            subIMISExtracts.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Extracts, UserID)
        End If
        subReports.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Reports, UserID)
        subUtilities.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.Utilities, UserID)
        subFunding.Enabled = MasterBI.checkRights(Enums.Rights.FundingSave, UserID)
        subEmailSetting.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.EmailSettings, UserID)


        btnSearch.Enabled = MasterBI.checkRights(IMIS_EN.Enums.Rights.InsureeEnquire, UserID)
        txtSearch.Enabled = btnSearch.Enabled




    End Sub
    Private Sub BindData()
        FillRepeater()
        FillPolicyGrid()
        FillProductDetails()
    End Sub

    'Private Sub FillRepeater()
    '    If Not IsNumeric(txtSearch.Text) Then Return
    '    Dim dt As New DataTable
    '    Dim Insuree As New IMIS_BI.InsureeBI
    '    dt = Insuree.GetInsureeByCHFID(txtSearch.Text)
    '    rptInsuree.DataSource = dt
    '    rptInsuree.DataBind()

    'End Sub
    Private Sub FillRepeater()
        If Not IsNumeric(txtSearch.Text) Then Return
        Dim dt As New DataTable
        Dim Insuree As New IMIS_BI.InsureeBI

        dt = Insuree.GetInsureeByCHFID(txtSearch.Text, Request.Cookies("CultureInfo").Value)
        rptInsuree.DataSource = dt
        rptInsuree.DataBind()

        Dim hf As HiddenField = CType(upDL.FindControl("hfPanelHasData"), HiddenField)
        If dt.Rows.Count > 0 Then
            hf.Value = "Yes"
        Else
            hf.Value = "No"
        End If

    End Sub
    Private Sub FillPolicyGrid()
        If Not IsNumeric(txtSearch.Text) Then Return
        Dim dt As New DataTable
        Dim Insuree As New IMIS_BI.InsureeBI
        dt = Insuree.GetInsureeByCHFIDGrid(txtSearch.Text)
        gvPolicy.DataSource = dt
        gvPolicy.DataBind()

    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        If Not IsNumeric(txtSearch.Text) Then Return
        Dim Insuree As New IMIS_BI.MasterBI
        If Not Insuree.CheckCHFID(txtSearch.Text) = True Then

            Return
        End If
        BindData()
        'Dim js As String = "<script type='javascript'>$(document).ready(function(){ $('#btnSearch').trigger('click'); });</script>"
        'Dim ltl As New Literal()
        'ltl.Text = js
        'pnlModalPopup.Controls.Add(ltl)
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsNumeric(txtSearch.Text) Then Return
        BindData()
    End Sub
    Private Sub FillProductDetails()
        If IsNumeric(txtSearch.Text) Then
            Dim oDict As New Dictionary(Of String, Object)
            Dim dt As DataTable = MasterBI.GetInsureeProductDetails(oDict, txtSearch.Text, txtItemCode.Value, txtServiceCode.Value)
            lblItemCode.Visible = (txtItemCode.Value.Trim <> String.Empty)
            lblItemCodeL.Visible = (txtItemCode.Value.Trim <> String.Empty)
            lblItemLeft.Visible = (txtItemCode.Value.Trim <> String.Empty)
            lblItemLeftL.Visible = (txtItemCode.Value.Trim <> String.Empty)
            lblItemMinDate.Visible = (txtItemCode.Value.Trim <> String.Empty)
            lblItemMinDateL.Visible = (txtItemCode.Value.Trim <> String.Empty)
            imgItemIsOk.Visible = (Not if(oDict("IsItemOk").ToString = String.Empty, False, oDict("IsItemOk")) And txtItemCode.Value.Trim <> String.Empty)

            lblItemCode.Text = txtItemCode.Value
            lblItemLeft.Text = if(oDict("ItemLeft").ToString.Trim = String.Empty, ".....", oDict("ItemLeft").ToString)
            If oDict("MinDateItem").ToString.Trim <> String.Empty Then
                lblItemMinDate.Text = Format(CDate(oDict("MinDateItem")), "dd/MM/yyyy")
            Else
                lblItemMinDate.Text = "....."
            End If

            lblServiceCode.Visible = (txtServiceCode.Value.Trim <> String.Empty)
            lblServiceCodeL.Visible = (txtServiceCode.Value.Trim <> String.Empty)
            lblServiceLeft.Visible = (txtServiceCode.Value.Trim <> String.Empty)
            lblServiceLeftL.Visible = (txtServiceCode.Value.Trim <> String.Empty)
            lblServiceMinDate.Visible = (txtServiceCode.Value.Trim <> String.Empty)
            lblServiceMinDateL.Visible = (txtServiceCode.Value.Trim <> String.Empty)
            imgServiceIsOk.Visible = (Not if(oDict("IsServiceOk").ToString = String.Empty, False, oDict("IsServiceOk")) And txtServiceCode.Value.Trim <> String.Empty)

            lblServiceCode.Text = txtServiceCode.Value
            lblServiceLeft.Text = if(oDict("ServiceLeft").ToString.Trim = String.Empty, ".....", oDict("ServiceLeft").ToString)
            If oDict("MinDateService").ToString.Trim <> String.Empty Then
                lblServiceMinDate.Text = Format(CDate(oDict("MinDateService")), "dd/MM/yyyy")
            Else
                lblServiceMinDate.Text = "....."
            End If

            gvProduct.DataSource = dt
            gvProduct.DataBind()

            Dim hf As HiddenField = CType(upDL.FindControl("hfPanelHasData"), HiddenField)
        End If
    End Sub
    Protected Sub rptInsuree_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptInsuree.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            'Reference the Repeater Item.
            Dim item As RepeaterItem = e.Item
            Dim Adjustibility = String.Empty
            Adjustibility = General.getControlSetting("CHFID")
            'Reference the Controls.
            Dim InsuranceNumber As Label = TryCast(item.FindControl("chf_ID"), Label)
            InsuranceNumber.Visible = Not (Adjustibility = "N")

            'Last Name
            Adjustibility = General.getControlSetting("LastName")
            Dim LastName As Label = TryCast(item.FindControl("Last_Name"), Label)
            LastName.Visible = Not (Adjustibility = "N")


            ' Region Of FSP
            Adjustibility = General.getControlSetting("RegionOfFSP")
            Dim RegionOFSP As Label = (TryCast(item.FindControl("regionalFSP"), Label))
            Dim L_FSPREGION As Label = (TryCast(item.FindControl("L_FSPREGION"), Label))
            RegionOFSP.Visible = Not (Adjustibility = "N")
            L_FSPREGION.Visible = Not (Adjustibility = "N")

            'Other Names
            Adjustibility = General.getControlSetting("OtherNames")
            Dim OtherName As Label = TryCast(item.FindControl("other_names"), Label)
            OtherName.Visible = Not (Adjustibility = "N")

            'District OF FSP
            Adjustibility = General.getControlSetting("DistrictOfFSP")
            Dim DistrictOfFSP As Label = TryCast(item.FindControl("district_of_fsp"), Label)
            Dim L_FSPDISTRICT As Label = TryCast(item.FindControl("L_FSPDISTRICT"), Label)
            DistrictOfFSP.Visible = Not (Adjustibility = "N")
            L_FSPDISTRICT.Visible = Not (Adjustibility = "N")


            'Date Of Birth
            Adjustibility = General.getControlSetting("DOB")
            Dim DOB As Label = TryCast(item.FindControl("dob"), Label)
            DOB.Visible = Not (Adjustibility = "N")

            'Age
            Adjustibility = General.getControlSetting("Age")
            Dim Age As Label = TryCast(item.FindControl("age"), Label)
            Age.Visible = Not (Adjustibility = "N")

            'HF Level
            Adjustibility = General.getControlSetting("HFLevel")
            Dim HFLevel As Label = TryCast(item.FindControl("hfLevel"), Label)
            Dim L_FSPLEVEL As Label = TryCast(item.FindControl("L_FSPLEVEL"), Label)
            HFLevel.Visible = Not (Adjustibility = "N")
            L_FSPLEVEL.Visible = Not (Adjustibility = "N")

            'Gender
            Adjustibility = General.getControlSetting("Gender")
            Dim Gender As Label = TryCast(item.FindControl("gender"), Label)
            Gender.Visible = Not (Adjustibility = "N")

            'FSP
            Adjustibility = General.getControlSetting("FirstServicePoint")
            Dim FSP As Label = TryCast(item.FindControl("fsp"), Label)
            Dim L_FSP As Label = TryCast(item.FindControl("L_FSP"), Label)
            FSP.Visible = Not (Adjustibility = "N")
            L_FSP.Visible = Not (Adjustibility = "N")


        End If
    End Sub
End Class
