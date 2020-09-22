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

Partial Public Class Product
    Inherits System.Web.UI.Page
    Private product As New IMIS_BI.ProductBI
    Dim eProduct As New IMIS_EN.tblProduct
    Public imisgen As New IMIS_Gen
    Dim eRelDistr As New IMIS_EN.tblRelDistr
    Private userBI As New IMIS_BI.UserBI

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvMedicalItems)
        AddRowSelectToGridView(gvMedicalServices)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
           
            CType(row.Cells(6).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMITATION") & "'")
            CType(row.Cells(6).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")
            CType(row.Cells(7).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMITATIONR") & "'")
            CType(row.Cells(7).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")
            CType(row.Cells(8).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMITATIONE") & "'")
            CType(row.Cells(8).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")
            CType(row.Cells(9).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_PRICEORIGIN") & "'")
            CType(row.Cells(9).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")

            CType(row.Cells(20).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_CEILING") & "'")
            CType(row.Cells(20).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")
            CType(row.Cells(21).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_CEILING") & "'")
            CType(row.Cells(21).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")

            CType(row.Cells(10).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMIT") & " " & imisgen.getMessage("M_SERVICE") & " " & imisgen.getMessage("M_FOR") & " " & imisgen.getMessage("M_ADULT") & " " & imisgen.getMessage("M_OTHER") & "'")
            CType(row.Cells(10).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")
            CType(row.Cells(11).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMIT") & " " & imisgen.getMessage("M_SERVICE") & " " & imisgen.getMessage("M_FOR") & " " & imisgen.getMessage("M_ADULT") & " " & imisgen.getMessage("M_REFERAL") & "'")
            CType(row.Cells(11).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")
            CType(row.Cells(12).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMIT") & " " & imisgen.getMessage("M_SERVICE") & " " & imisgen.getMessage("M_FOR") & " " & imisgen.getMessage("M_ADULT") & " " & imisgen.getMessage("M_EMERGENCY") & "'")
            CType(row.Cells(12).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")

            CType(row.Cells(13).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMIT") & " " & imisgen.getMessage("M_SERVICE") & " " & imisgen.getMessage("M_FOR") & " " & imisgen.getMessage("M_CHILD") & " " & imisgen.getMessage("M_OTHER") & "'")
            CType(row.Cells(13).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")
            CType(row.Cells(14).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMIT") & " " & imisgen.getMessage("M_SERVICE") & " " & imisgen.getMessage("M_FOR") & " " & imisgen.getMessage("M_CHILD") & " " & imisgen.getMessage("M_REFERAL") & "'")
            CType(row.Cells(14).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")
            CType(row.Cells(15).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMIT") & " " & imisgen.getMessage("M_SERVICE") & " " & imisgen.getMessage("M_FOR") & " " & imisgen.getMessage("M_CHILD") & " " & imisgen.getMessage("M_EMERGENCY") & "'")
            CType(row.Cells(15).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")

            CType(row.Cells(16).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMIT") & " " & imisgen.getMessage("M_NUMBER") & " " & imisgen.getMessage("M_FOR") & " " & imisgen.getMessage("M_ADULT") & "'")
            CType(row.Cells(16).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")
            CType(row.Cells(17).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_LIMIT") & " " & imisgen.getMessage("M_NUMBER") & " " & imisgen.getMessage("M_FOR") & " " & imisgen.getMessage("M_CHILD") & "'")
            CType(row.Cells(17).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")

            CType(row.Cells(18).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_WAITINGPERIOD") & " " & imisgen.getMessage("M_FOR") & " " & imisgen.getMessage("M_ADULT") & "'")
            CType(row.Cells(18).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")
            CType(row.Cells(19).Controls(1), TextBox).Attributes.Add("onfocus", "document.getElementById('footer_validationSummary').innerHTML = '';document.getElementById('footer_lblMsg').innerHTML = '" & imisgen.getMessage("M_WAITINGPERIOD") & " " & imisgen.getMessage("M_FOR") & " " & imisgen.getMessage("M_CHILD") & "'")
            CType(row.Cells(19).Controls(1), TextBox).Attributes.Add("onblur", "document.getElementById('footer_lblMsg').innerHTML = ''")

        Next
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()
        Try
            lblMsg.Text = ""

            If HttpContext.Current.Request.QueryString("p") IsNot Nothing Then
                eProduct.ProdUUID = Guid.Parse(HttpContext.Current.Request.QueryString("p"))
                eProduct.ProdID = product.GetProdIdByUUID(eProduct.ProdUUID)
            End If

            If IsPostBack = True Then Return

            FillRegion()
            getLevels()
            getSubLevels()
            'gvMedicalItems.DataSource = product.GetProductItems(eProduct.ProdID)
            'gvMedicalItems.DataBind()

            'gvMedicalServices.DataSource = product.GetMedicalServices(eProduct.ProdID)
            'gvMedicalServices.DataBind()

            Dim dt As DataTable = product.GetPeriod(True)

            ddlDistribution.DataSource = dt
            ddlDistribution.DataValueField = "Code"
            ddlDistribution.DataTextField = "DistType"
            ddlDistribution.DataBind()
            ddlDistributionIP.DataSource = dt
            ddlDistributionIP.DataValueField = "Code"
            ddlDistributionIP.DataTextField = "DistType"
            ddlDistributionIP.DataBind()
            ddlDistributionOP.DataSource = dt
            ddlDistributionOP.DataValueField = "Code"
            ddlDistributionOP.DataTextField = "DistType"
            ddlDistributionOP.DataBind()

            Dim dtDays As DataTable = product.GetDays(True)
            Dim dtMonths As DataTable = product.GetMonths(True)
            ddlCycle1Day.DataSource = dtDays
            ddlCycle1Day.DataValueField = "DayT"
            ddlCycle1Day.DataTextField = "DayD"
            ddlCycle1Day.DataBind()

            ddlCycle2Day.DataSource = dtDays
            ddlCycle2Day.DataValueField = "DayT"
            ddlCycle2Day.DataTextField = "DayD"
            ddlCycle2Day.DataBind()

            ddlCycle3Day.DataSource = dtDays
            ddlCycle3Day.DataValueField = "DayT"
            ddlCycle3Day.DataTextField = "DayD"
            ddlCycle3Day.DataBind()

            ddlCycle4Day.DataSource = dtDays
            ddlCycle4Day.DataValueField = "DayT"
            ddlCycle4Day.DataTextField = "DayD"
            ddlCycle4Day.DataBind()

            ddlCycle1Month.DataSource = dtMonths
            ddlCycle1Month.DataValueField = "MonthN"
            ddlCycle1Month.DataTextField = "MonthT"
            ddlCycle1Month.DataBind()

            ddlCycle2Month.DataSource = dtMonths
            ddlCycle2Month.DataValueField = "MonthN"
            ddlCycle2Month.DataTextField = "MonthT"
            ddlCycle2Month.DataBind()

            ddlCycle3Month.DataSource = dtMonths
            ddlCycle3Month.DataValueField = "MonthN"
            ddlCycle3Month.DataTextField = "MonthT"
            ddlCycle3Month.DataBind()

            ddlCycle4Month.DataSource = dtMonths
            ddlCycle4Month.DataValueField = "MonthN"
            ddlCycle4Month.DataTextField = "MonthT"
            ddlCycle4Month.DataBind()

            ddlCeilingInterpretation.DataSource = product.GetCeilingInterpritation
            ddlCeilingInterpretation.DataValueField = "CeilingIntCode"
            ddlCeilingInterpretation.DataTextField = "CeilingIntDesc"
            ddlCeilingInterpretation.DataBind()

            'txtShareContribution.Text = 100.0
            'txtNoOfInsuredPopulation.Text = 100.0
            If Not eProduct.ProdID = 0 Then
                product.LoadProduct(eProduct)

                If eProduct.RegionId IsNot Nothing Then
                    ddlRegion.SelectedValue = eProduct.RegionId
                    FillDistricts()
                End If
                If eProduct.DistrictId IsNot Nothing Then
                    ddlDistrict.SelectedValue = eProduct.DistrictId
                End If
                If Not eProduct.ProductCode Is Nothing Then
                    txtProductCode.Text = eProduct.ProductCode
                End If

                If Not eProduct.ProductName Is Nothing Then
                    txtProductName.Text = eProduct.ProductName
                End If

                'If Not eProduct.tblLocation Is Nothing Then
                '    ddlDistrict.SelectedValue = eProduct.tblLocation.LocationId
                'End If
                'If Not eProduct.tblLocation Is Nothing Then
                '    ddlRegion.SelectedValue = eProduct.tblLocation.LocationId
                'End If
                Dim datevalue As Date
                If Not eProduct.DateFrom = Nothing Then
                    datevalue = eProduct.DateFrom
                    txtDateFrom.Text = datevalue
                End If

                If Not eProduct.DateTo = Nothing Then
                    datevalue = eProduct.DateTo
                    txtDateTo.Text = datevalue
                End If


                txtLumpSum.Text = FormatNumber(eProduct.LumpSum)



                txtMaxNoOfMembers.Text = FormatNumber(eProduct.MemberCount, 0)





                If Not eProduct.PremiumAdult Is Nothing Then
                    txtAdultPremium.Text = FormatNumber(eProduct.PremiumAdult)
                End If


                If Not eProduct.PremiumChild Is Nothing Then
                    txtChildPremium.Text = FormatNumber(eProduct.PremiumChild)
                End If
                'Additon for BEPHA: start
                If Not eProduct.GracePeriodRenewal Is Nothing Then
                    txtGracePeriodRenewal.Text = FormatNumber(eProduct.GracePeriodRenewal, 0)
                End If

                If Not eProduct.WaitingPeriod Is Nothing Then
                    txtWaitingPeriod.Text = FormatNumber(eProduct.WaitingPeriod, 0)
                End If
                If Not eProduct.MaxInstallments Is Nothing Then
                    txtMaxInstallments.Text = FormatNumber(eProduct.MaxInstallments, 0)
                End If

                If Not eProduct.RegistrationLumpSum Is Nothing Then
                    txtRegLumpSum.Text = FormatNumber(eProduct.RegistrationLumpSum)
                End If
                If Not eProduct.RegistrationFee Is Nothing Then
                    txtRegFee.Text = FormatNumber(eProduct.RegistrationFee)
                End If
                If Not eProduct.GeneralAssemblyLumpSum Is Nothing Then
                    txtGenAssemblyLumpsum.Text = FormatNumber(eProduct.GeneralAssemblyLumpSum)
                End If
                If Not eProduct.GeneralAssemblyFee Is Nothing Then
                    txtGenAssemblyFee.Text = FormatNumber(eProduct.GeneralAssemblyFee)
                End If
                If Not eProduct.StartCycle1 Is Nothing Then
                    Dim p() As String = Split(eProduct.StartCycle1, "-")
                    If p.Length > 1 Then
                        ddlCycle1Day.SelectedValue = p(0)
                        ddlCycle1Month.SelectedValue = p(1)
                    End If
                End If
                If Not eProduct.StartCycle2 Is Nothing Then
                    Dim p() As String = Split(eProduct.StartCycle2, "-")
                    If p.Length > 1 Then
                        ddlCycle2Day.SelectedValue = p(0)
                        ddlCycle2Month.SelectedValue = p(1)
                    End If
                End If
                'Addition for BEPHA: end

                If Not eProduct.DedTreatment Is Nothing Then
                    txtDedutibleForTreatment.Text = FormatNumber(eProduct.DedTreatment)
                End If


                If Not eProduct.DedOPTreatment Is Nothing Then
                    txtDedOPTreatment.Text = FormatNumber(eProduct.DedOPTreatment)
                End If


                If Not eProduct.DedIPTreatment Is Nothing Then
                    txtDedIPTreatment.Text = FormatNumber(eProduct.DedIPTreatment)
                End If


                If Not eProduct.MaxTreatment Is Nothing Then
                    txtMaxTreatment.Text = FormatNumber(eProduct.MaxTreatment)
                End If


                If Not eProduct.MaxOPTreatment Is Nothing Then
                    txtMaxOPTreatment.Text = FormatNumber(eProduct.MaxOPTreatment)
                End If


                If Not eProduct.MaxIPTreatment Is Nothing Then
                    txtMaxIPTreatment.Text = FormatNumber(eProduct.MaxIPTreatment)
                End If


                If Not eProduct.DedPolicy Is Nothing Then
                    txtDedPolicy.Text = FormatNumber(eProduct.DedPolicy)
                End If


                If Not eProduct.DedOPPolicy Is Nothing Then
                    txtDedOPPolicy.Text = FormatNumber(eProduct.DedOPPolicy)
                End If


                If Not eProduct.DedIPPolicy Is Nothing Then
                    txtDedIPPolicy.Text = FormatNumber(eProduct.DedIPPolicy)
                End If


                If Not eProduct.MaxPolicy Is Nothing Then
                    txtMaxPolicy.Text = FormatNumber(eProduct.MaxPolicy)
                End If


                If Not eProduct.MaxOPPolicy Is Nothing Then
                    txtMaxOPPolicy.Text = FormatNumber(eProduct.MaxOPPolicy)
                End If


                If Not eProduct.MaxIPPolicy Is Nothing Then
                    txtMaxIPPOlicy.Text = FormatNumber(eProduct.MaxIPPolicy)
                End If


                If Not eProduct.DedInsuree Is Nothing Then
                    txtDedInsuree.Text = FormatNumber(eProduct.DedInsuree)
                End If


                If Not eProduct.DedOPInsuree Is Nothing Then
                    txtDedOPInsuree.Text = FormatNumber(eProduct.DedOPInsuree)
                End If


                If Not eProduct.DedIPInsuree Is Nothing Then
                    txtDedIPInsuree.Text = FormatNumber(eProduct.DedIPInsuree)
                End If


                If Not eProduct.MaxInsuree Is Nothing Then
                    txtMaxInsuree.Text = FormatNumber(eProduct.MaxInsuree)
                End If


                If Not eProduct.MaxOPInsuree Is Nothing Then
                    txtMaxOPInsuree.Text = FormatNumber(eProduct.MaxOPInsuree)
                End If


                If Not eProduct.MaxIPInsuree Is Nothing Then
                    txtMaxIPInsuree.Text = FormatNumber(eProduct.MaxIPInsuree)
                End If


                If Not eProduct.PeriodRelPrices Is Nothing Then
                    ddlDistribution.SelectedValue = eProduct.PeriodRelPrices
                End If


                If Not eProduct.PeriodRelPricesOP Is Nothing Then
                    ddlDistributionOP.SelectedValue = eProduct.PeriodRelPricesOP
                End If


                If Not eProduct.PeriodRelPricesIP Is Nothing Then
                    ddlDistributionIP.SelectedValue = eProduct.PeriodRelPricesIP
                End If

                'Addition for BEPHA: start:
                If Not eProduct.MaxNoConsultation Is Nothing Then
                    txtNumConsultations.Text = FormatNumber(eProduct.MaxNoConsultation, 0)
                End If
                If Not eProduct.MaxAmountConsultation Is Nothing Then
                    txtMaxAmountConsultations.Text = FormatNumber(eProduct.MaxAmountConsultation)
                End If
                If Not eProduct.MaxNoSurgery Is Nothing Then
                    txtNumSurgeries.Text = FormatNumber(eProduct.MaxNoSurgery, 0)
                End If
                If Not eProduct.MaxAmountSurgery Is Nothing Then
                    txtMaxAmountSurgeries.Text = FormatNumber(eProduct.MaxAmountSurgery)
                End If
                If Not eProduct.MaxNoDelivery Is Nothing Then
                    txtNumDeliveries.Text = FormatNumber(eProduct.MaxNoDelivery, 0)
                End If
                If Not eProduct.MaxAmountDelivery Is Nothing Then
                    txtMaxAmountDeliveries.Text = FormatNumber(eProduct.MaxAmountDelivery)
                End If
                If Not eProduct.MaxNoHospitalizaion Is Nothing Then
                    txtNumHospitalizations.Text = FormatNumber(eProduct.MaxNoHospitalizaion, 0)
                End If
                If Not eProduct.MaxAmountHospitalization Is Nothing Then
                    txtMaxAmountHospitalizations.Text = FormatNumber(eProduct.MaxAmountHospitalization)
                End If
                If Not eProduct.MaxNoVisits Is Nothing Then
                    txtNumVisits.Text = FormatNumber(eProduct.MaxNoVisits, 0)
                End If
                If Not eProduct.MaxAmountAntenatal Is Nothing Then
                    txtMaxAmountAntenatal.Text = FormatNumber(eProduct.MaxAmountAntenatal)
                End If
                If Not eProduct.MaxNoAntenatal Is Nothing Then
                    txtNumAntenatal.Text = FormatNumber(eProduct.MaxNoAntenatal, 0)
                End If
                'Addition for BEPHA: end:
                'Addition for Nepal >> Start
                If eProduct.MaxPolicyExtraMember IsNot Nothing Then
                    txtMaxPolicyExtraMember.Text = eProduct.MaxPolicyExtraMember
                End If
                If eProduct.MaxPolicyExtraMemberIP IsNot Nothing Then
                    txtMaxIPPolicyExtraMember.Text = eProduct.MaxPolicyExtraMemberIP
                End If
                If eProduct.MaxPolicyExtraMemberOP IsNot Nothing Then
                    txtMaxOPPolicyExtraMember.Text = eProduct.MaxPolicyExtraMemberOP
                End If
                If eProduct.MaxCeilingPolicy IsNot Nothing Then
                    txtMaxPolicyMC.Text = eProduct.MaxCeilingPolicy
                End If
                If eProduct.MaxCeilingPolicyIP IsNot Nothing Then
                    txtMaxIPPolicyMC.Text = eProduct.MaxCeilingPolicyIP
                End If
                If eProduct.MaxCeilingPolicyOP IsNot Nothing Then
                    txtMaxOPPolicyMC.Text = eProduct.MaxCeilingPolicyOP
                End If
                'Addition for Nepal >> End

                If Not eProduct.AccCodePremiums Is Nothing Then
                    txtAccCodePremiums.Text = eProduct.AccCodePremiums
                End If


                If Not eProduct.AccCodeRemuneration Is Nothing Then
                    txtAccCodeRemuneration.Text = eProduct.AccCodeRemuneration
                End If

                If Not eProduct.CeilingInterpretation Is Nothing Then
                    ddlCeilingInterpretation.SelectedValue = eProduct.CeilingInterpretation
                End If
                If Not eProduct.Recurrence Is Nothing Then
                    txtRecurrence.Text = eProduct.Recurrence
                End If
                txtInsurancrePeriod.Text = FormatNumber(eProduct.InsurancePeriod, 0)



                txtGracePeriod.Text = FormatNumber(eProduct.GracePeriod, 0)

                'Addition for Nepal >> Start
                If eProduct.Threshold IsNot Nothing Then txtThresholdMembers.Text = eProduct.Threshold
                If eProduct.RenewalDiscountPerc IsNot Nothing Then txtRenewalDiscountPercentage.Text = eProduct.RenewalDiscountPerc
                If eProduct.RenewalDiscountPeriod IsNot Nothing Then txtRenewalDiscountPeriod.Text = eProduct.RenewalDiscountPeriod
                If eProduct.AdministrationPeriod IsNot Nothing Then txtAdministrationPeriod.Text = eProduct.AdministrationPeriod
                If eProduct.StartCycle3 IsNot Nothing Then
                    Dim p() As String = Split(eProduct.StartCycle3, "-")
                    If p.Length > 1 Then
                        ddlCycle3Day.SelectedValue = p(0)
                        ddlCycle3Month.SelectedValue = p(1)
                    End If
                End If
                If eProduct.StartCycle4 IsNot Nothing Then
                    Dim p() As String = Split(eProduct.StartCycle4, "-")
                    If p.Length > 1 Then
                        ddlCycle4Day.SelectedValue = p(0)
                        ddlCycle4Month.SelectedValue = p(1)
                    End If
                End If
                If eProduct.EnrolmentDiscountPerc IsNot Nothing Then txtEnrolmentDiscountPerc.Text = eProduct.EnrolmentDiscountPerc
                If eProduct.EnrolmentDiscountPeriod IsNot Nothing Then txtEnrolmentDiscountPeriod.Text = eProduct.EnrolmentDiscountPeriod
                'Addition for Nepal >> End

                'AssignOriginalValue(gvMedicalItems)
                'AssignOriginalValue(gvMedicalServices)
            End If
            ddlDistrict.Attributes.Add("LoadValue", ddlDistrict.SelectedValue)
            'ddlconversion.datasource = product.getproducts(imisgen.getuserid(session("user")), true, ddldistrict.selectedvalue)
            'ddlconversion.datavaluefield = "prodid"
            'ddlconversion.datatextfield = "productcode"
            'ddlconversion.databind()
            If Not eProduct.tblProduct2 Is Nothing Then
                btnConversion_Click(Me, New System.EventArgs)
                ddlConversion.SelectedValue = eProduct.tblProduct2.ProdID
            End If
            If HttpContext.Current.Request.QueryString("r") = 1 Or eProduct.ValidityTo.HasValue Then
                btnLoadMedicalServices_ServerClick(sender, e)
                btnLoadMedicalItems_ServerClick(sender, e)
                pnlProduct.Enabled = False
                B_SAVE.Visible = False
            End If

            'Dim RefUrl = Request.Headers("Referer")
            'Dim reg As New Regex("OverViewFamily", RegexOptions.IgnoreCase)
            'If reg.IsMatch(RefUrl) Then
            '    pnlProduct.Enabled = False
            '    B_SAVE.Visible = False
            '    B_CANCEL.Visible = False
            '    CType(Me.Master.FindControl("pnlDisablePage"), Panel).Attributes.Add("style", "display:block")

            'End If
            If Request.QueryString("x") = 1 Then
                Panel2.Enabled = False
                B_SAVE.Visible = False
                B_CANCEL.Visible = False
                CType(Me.Master.FindControl("pnlDisablePage"), Panel).Attributes.Add("style", "display:block")
            End If
            Dim dtB As DataTable = product.GetDistribution(eProduct.ProdID, "B", getTimeNumber(eProduct.PeriodRelPrices))
            If dtB.Rows.Count = 0 Then
                Dim dr As DataRow = dtB.NewRow
                dtB.Rows.Add(dr)
            End If
            gvDistrB.DataSource = dtB
            gvDistrB.DataBind()

            Dim dtI As DataTable = product.GetDistribution(eProduct.ProdID, "I", getTimeNumber(eProduct.PeriodRelPricesIP))
            If dtI.Rows.Count = 0 Then
                Dim dr As DataRow = dtI.NewRow
                dtI.Rows.Add(dr)
            End If
            gvDistrI.DataSource = dtI
            gvDistrI.DataBind()



            Dim dtO As DataTable = product.GetDistribution(eProduct.ProdID, "O", getTimeNumber(eProduct.PeriodRelPricesOP))
            If dtO.Rows.Count = 0 Then
                Dim dr As DataRow = dtO.NewRow
                ' dr("DistrPerc") = "0.00"
                dtO.Rows.Add(dr)
            End If
            gvDistrO.DataSource = dtO
            gvDistrO.DataBind()
            FillHiddenDistributionGridView()

            If Request.QueryString("action") = "duplicate" Then
                btnLoadMedicalServices_ServerClick(sender, e)
                btnLoadMedicalItems_ServerClick(sender, e)
                eProduct.ProdID = 0
                txtProductCode.Text = ""
                eProduct.ProductCode = ""
            End If

            Dim SumWeight As Double = eProduct.WeightNumberFamilies + eProduct.WeightPopulation + eProduct.WeightNumberFamilies + eProduct.WeightInsuredPopulation + eProduct.WeightNumberInsuredFamilies + eProduct.WeightNumberVisits + eProduct.WeightAdjustedAmount

            If eProduct.Level1 IsNot Nothing Then ddlLevel1.SelectedValue = eProduct.Level1
            If eProduct.Sublevel1 IsNot Nothing Then ddlSubLevel1.SelectedValue = eProduct.Sublevel1
            If eProduct.Level2 IsNot Nothing Then ddlLevel2.SelectedValue = eProduct.Level2
            If eProduct.Sublevel2 IsNot Nothing Then ddlSubLevel2.SelectedValue = eProduct.Sublevel2
            If eProduct.Level3 IsNot Nothing Then ddlLevel3.SelectedValue = eProduct.Level3
            If eProduct.Sublevel3 IsNot Nothing Then ddlSubLevel3.SelectedValue = eProduct.Sublevel3
            If eProduct.Level4 IsNot Nothing Then ddlLevel4.SelectedValue = eProduct.Level4
            If eProduct.Sublevel4 IsNot Nothing Then ddlSubLevel4.SelectedValue = eProduct.Sublevel4

            txtShareContribution.Text = eProduct.ShareContribution 'If(eProduct.ShareContribution = 0, 100.0, eProduct.ShareContribution)
            txtWeightOfPopulation.Text = eProduct.WeightPopulation
            txtNumberOfFamilies.Text = eProduct.WeightNumberFamilies
            txtNoOfInsuredPopulation.Text = eProduct.WeightInsuredPopulation ' If(SumWeight = 0, 100.0, eProduct.WeightInsuredPopulation)
            txtNoOfInseredFamilies.Text = eProduct.WeightNumberInsuredFamilies
            txtNumberOfClaims.Text = eProduct.WeightNumberVisits
            txtAdjustedAmount.Text = eProduct.WeightAdjustedAmount



        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons)
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)

            'EventLog.WriteEntry("IMIS", imisgen.getUserId(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 2, 3)
        End Try
    End Sub
    Private Sub FillRegion()
        Dim dtRegion As DataTable = product.GetRegions(imisgen.getUserId(Session("User")), True, True)
        ddlRegion.DataSource = dtRegion
        ddlRegion.DataValueField = "RegionId"
        ddlRegion.DataTextField = "RegionName"
        ddlRegion.DataBind()
        If dtRegion.Rows.Count = 1 Then
            FillDistricts()
        End If
    End Sub
    Private Sub getLevels()
        Dim dt As DataTable = product.getLevels()
        ddlLevel1.DataSource = dt
        ddlLevel1.DataValueField = "Code"
        ddlLevel1.DataTextField = "HFLevel"
        ddlLevel1.DataBind()
        ddlLevel1.SelectedValue = "D"

        ddlLevel2.DataSource = dt
        ddlLevel2.DataValueField = "Code"
        ddlLevel2.DataTextField = "HFLevel"
        ddlLevel2.DataBind()
        ddlLevel2.SelectedValue = "C"

        ddlLevel3.DataSource = dt
        ddlLevel3.DataValueField = "Code"
        ddlLevel3.DataTextField = "HFLevel"
        ddlLevel3.DataBind()


        ddlLevel4.DataSource = dt
        ddlLevel4.DataValueField = "Code"
        ddlLevel4.DataTextField = "HFLevel"
        ddlLevel4.DataBind()
    End Sub
    Private Sub getSubLevels()
        Dim dt As DataTable = product.getSubLevels()
        ddlSubLevel1.DataSource = dt
        ddlSubLevel1.DataValueField = "HFSubLevel"
        ddlSubLevel1.DataTextField = "HFsubLevelDesc"
        ddlSubLevel1.DataBind()

        ddlSubLevel2.DataSource = dt
        ddlSubLevel2.DataValueField = "HFSubLevel"
        ddlSubLevel2.DataTextField = "HFsubLevelDesc"
        ddlSubLevel2.DataBind()

        ddlSubLevel3.DataSource = dt
        ddlSubLevel3.DataValueField = "HFSubLevel"
        ddlSubLevel3.DataTextField = "HFsubLevelDesc"
        ddlSubLevel3.DataBind()

        ddlSubLevel4.DataSource = dt
        ddlSubLevel4.DataValueField = "HFSubLevel"
        ddlSubLevel4.DataTextField = "HFsubLevelDesc"
        ddlSubLevel4.DataBind()
    End Sub
    Private Sub FillDistricts()
        ddlDistrict.DataSource = product.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataValueField = "DistrictID"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub LoadMedicalServices()
        gvMedicalServices.DataSource = product.GetMedicalServices(eProduct.ProdID)
        gvMedicalServices.DataBind()
        AssignOriginalValue(gvMedicalServices)
    End Sub
    Private Sub LoadMedicalItems()
        gvMedicalItems.DataSource = product.GetProductItems(eProduct.ProdID)
        gvMedicalItems.DataBind()
        AssignOriginalValue(gvMedicalItems)
    End Sub
    Private Sub RunPageSecurity()
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        Dim RefUrl = If(Request.Headers("Referer") Is Nothing, String.Empty, Request.Headers("Referer"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Product, Page) Then
            Dim Add As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.ProductAdd, UserID)
            Dim Edit As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.ProductEdit, UserID)
            Dim View As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.ProductSearch, UserID)

            Dim reg As New Regex("OverviewFamily", RegexOptions.IgnoreCase)
            If reg.IsMatch(RefUrl) Then
                If View Or Add Or Edit Then
                    B_SAVE.Visible = False
                    pnlProduct.Enabled = False
                Else
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Product.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf Not Add And Not Edit Then
                B_SAVE.Visible = False
                pnlProduct.Enabled = False
            End If
        Else

            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Product.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub AssignOriginalValue(ByVal gv As GridView)

        Dim _checkedMI As Boolean = True
        For Each r In gv.Rows
            Dim chkSelect As CheckBox = CType(r.Cells(0).Controls(1), CheckBox)
            chkSelect.Checked = If(gv.DataKeys(r.RowIndex).Value Is System.DBNull.Value, 0, 1)
            If chkSelect.Checked <> True Then
                _checkedMI = False
            End If
        Next
        If gv.ID = "gvmedicalservices" Then
            Checkbox2.Checked = _checkedMI
        Else
            Checkbox1.Checked = _checkedMI
        End If
    End Sub
    Public Function CheckDifference(ByVal grid As GridView, ByVal RowIndex As Integer) As Boolean

        Dim chkSelect As CheckBox = CType(grid.Rows(RowIndex).Cells(0).Controls(1), CheckBox)

        If Request.QueryString("Action") = "duplicate" Then
            Return chkSelect.Checked
        End If

        If chkSelect.Checked <> CBool(If(grid.DataKeys(grid.Rows(RowIndex).RowIndex).Value Is System.DBNull.Value, 0, grid.DataKeys(grid.Rows(RowIndex).RowIndex).Value)) Then
            Return True
        Else
            Return False
        End If
    End Function
    Protected Function ValidateDeductableCeilingValueCombination() As Boolean

        If Not ddlDistribution.SelectedValue = "0" Then
            If Not ddlDistributionIP.SelectedValue = "0" Or Not ddlDistributionOP.SelectedValue = "0" Then Return False
        End If
        Dim Deductible As New Dictionary(Of String, Boolean)

        Deductible.Add("DedTreatment", If(eProduct.DedTreatment Is Nothing, 0, 1))
        Deductible.Add("MaxTreatment", If(eProduct.MaxTreatment Is Nothing, 0, 1))
        Deductible.Add("DedIPTreatment", If(eProduct.DedIPTreatment Is Nothing, 0, 1))
        Deductible.Add("MaxIPTreatment", If(eProduct.MaxIPTreatment Is Nothing, 0, 1))
        Deductible.Add("DedOPTreatment", If(eProduct.DedOPTreatment Is Nothing, 0, 1))
        Deductible.Add("MaxOPTreatment", If(eProduct.MaxOPTreatment Is Nothing, 0, 1))

        Deductible.Add("DedInsuree", If(eProduct.DedInsuree Is Nothing, 0, 1))
        Deductible.Add("MaxInsuree", If(eProduct.MaxInsuree Is Nothing, 0, 1))
        Deductible.Add("DedIPInsuree", If(eProduct.DedIPInsuree Is Nothing, 0, 1))
        Deductible.Add("MaxIPInsuree", If(eProduct.MaxIPInsuree Is Nothing, 0, 1))
        Deductible.Add("DedOPInsuree", If(eProduct.DedOPInsuree Is Nothing, 0, 1))
        Deductible.Add("MaxOPInsuree", If(eProduct.MaxOPInsuree Is Nothing, 0, 1))

        Deductible.Add("DedPolicy", If(eProduct.DedPolicy Is Nothing, 0, 1))
        Deductible.Add("MaxPolicy", If(eProduct.MaxPolicy Is Nothing, 0, 1))
        Deductible.Add("DedIPPolicy", If(eProduct.DedIPPolicy Is Nothing, 0, 1))
        Deductible.Add("MaxIPPolicy", If(eProduct.MaxIPPolicy Is Nothing, 0, 1))
        Deductible.Add("DedOPPolicy", If(eProduct.DedOPPolicy Is Nothing, 0, 1))
        Deductible.Add("MaxOPPolicy", If(eProduct.MaxOPPolicy Is Nothing, 0, 1))

        'Deductible.Add("MaxExtraMember", if(eProduct.MaxPolicyExtraMember Is Nothing, 0, 1))
        'Deductible.Add("MaxIPExtraMember", if(eProduct.MaxPolicyExtraMemberIP Is Nothing, 0, 1))
        'Deductible.Add("MaxOPExtraMember", if(eProduct.MaxPolicyExtraMemberOP Is Nothing, 0, 1))

        'Deductible.Add("MaxCeilingPolicy", if(eProduct.MaxCeilingPolicy Is Nothing, 0, 1))
        'Deductible.Add("MaxIPCeilingPolicy", if(eProduct.MaxCeilingPolicyIP Is Nothing, 0, 1))
        'Deductible.Add("MaxIPCeilingPolicy", if(eProduct.MaxCeilingPolicyOP Is Nothing, 0, 1))

        'Addition for Nepal >> Start
        If eProduct.MaxPolicyExtraMember IsNot Nothing Then
            If eProduct.MaxPolicyExtraMemberIP IsNot Nothing Or eProduct.MaxPolicyExtraMemberOP IsNot Nothing Then
                Return False
            End If
        End If
        If eProduct.MaxCeilingPolicy IsNot Nothing Then
            If eProduct.MaxCeilingPolicyIP IsNot Nothing Or eProduct.MaxCeilingPolicyOP IsNot Nothing Then
                Return False
            End If
        End If
        'Addition for Nepal >> End

        Dim flag As Boolean = False

        '>> TEST FIRST COLUMN  >> START
        If Deductible("DedTreatment") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("Ded") And Not key = "DedTreatment" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("DedInsuree") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("Ded") And Not key = "DedInsuree" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("DedPolicy") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("Ded") And Not key = "DedPolicy" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("MaxTreatment") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("Max") And Not key = "MaxTreatment" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("MaxInsuree") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("Max") And Not key = "MaxInsuree" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("MaxPolicy") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("Max") And Not key = "MaxPolicy" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        ''>> Addition for Nepal >> Start
        'If Deductible("MaxExtraMember") = True Then
        '    For Each key As String In Deductible.Keys
        '        If (key.StartsWith("Max") And Not key = "MaxExtraMember") Or key = "MaxIPExtraMember" Or key = "MaxOPExtraMember" Then
        '            flag = Deductible(key)
        '            If flag = True Then Return False
        '        End If
        '    Next
        'End If
        'If Deductible("MaxCeilingPolicy") = True Then
        '    For Each key As String In Deductible.Keys
        '        If (key.StartsWith("Max") And Not key = "MaxCeilingPolicy") Or key = "MaxIPCeilingPolicy" Or key = "MaxOPCeilingPolicy" Then
        '            flag = Deductible(key)
        '            If flag = True Then Return False
        '        End If
        '    Next
        'End If
        ''>> Addition for Nepal >> End

        '>> TEST FIRST COLUMN  >> END

        '>> TEST IN-PATIENT COLUMN  >> START
        'in-patient insuree
        If Deductible("DedIPInsuree") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("DedIP") And Not key = "DedIPInsuree" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("MaxIPInsuree") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("MaxIP") And Not key = "MaxIPInsuree" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        'in-patient treatment
        If Deductible("DedIPTreatment") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("DedIP") And Not key = "DedIPTreatment" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("MaxIPTreatment") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("MaxIP") And Not key = "MaxIPTreatment" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        'in-patient policy
        If Deductible("DedIPPolicy") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("DedIP") And Not key = "DedIPPolicy" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("MaxIPPolicy") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("MaxIP") And Not key = "MaxIPPolicy" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        ''>> Addition for Nepal >> Start
        ''in-patient max member
        'If Deductible("MaxIPExtraMember") = True Then
        '    'For Each key As String In Deductible.Keys
        '    '    If key.StartsWith("MaxIP") And Not key = "MaxIPExtraMember" Or key = "MaxExtraMember" Then
        '    '        flag = Deductible(key)
        '    '        If flag = True Then Return False
        '    '    End If
        '    'Next
        'End If
        ''in-patient ceiling policy
        'If Deductible("MaxIPCeilingPolicy") = True Then
        '    For Each key As String In Deductible.Keys
        '        If key.StartsWith("MaxIP") And Not key = "MaxIPCeilingPolicy" Then
        '            flag = Deductible(key)
        '            If flag = True Then Return False
        '        End If
        '    Next
        'End If
        ''>> Addition for Nepal >> End
        '>> TEST IN-PATIENT COLUMN  >> END

        '>> TEST OUT-PATIENT COLUMN  >> START
        'out-patient insuree
        If Deductible("DedOPInsuree") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("DedOP") And Not key = "DedOPInsuree" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("MaxOPInsuree") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("MaxOP") And Not key = "MaxOPInsuree" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        'out-patient treatment
        If Deductible("DedOPTreatment") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("DedOP") And Not key = "DedOPTreatment" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("MaxOPTreatment") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("MaxOP") And Not key = "MaxOPTreatment" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        'out-patient policy
        If Deductible("DedOPPolicy") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("DedOP") And Not key = "DedOPPolicy" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        If Deductible("MaxOPPolicy") = True Then
            For Each key As String In Deductible.Keys
                If key.StartsWith("MaxOP") And Not key = "MaxOPPolicy" Then
                    flag = Deductible(key)
                    If flag = True Then Return False
                End If
            Next
        End If
        ''>> Addition for Nepal >> Start
        ''in-patient max member
        'If Deductible("MaxOPExtraMember") = True Then
        '    For Each key As String In Deductible.Keys
        '        If key.StartsWith("MaxOP") And Not key = "MaxOPExtraMember" Then
        '            flag = Deductible(key)
        '            If flag = True Then Return False
        '        End If
        '    Next
        'End If
        ''in-patient ceiling policy
        'If Deductible("MaxOPCeilingPolicy") = True Then
        '    For Each key As String In Deductible.Keys
        '        If key.StartsWith("MaxOP") And Not key = "MaxOPCeilingPolicy" Then
        '            flag = Deductible(key)
        '            If flag = True Then Return False
        '        End If
        '    Next
        'End If
        ''>> Addition for Nepal >> End
        '>> TEST OUT-PATIENT COLUMN  >> START

        Return True
    End Function
    Protected Sub B_SAVE_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_SAVE.Click
        Dim chk As Integer = 1
        eProduct.ProductCode = txtProductCode.Text
        Dim bRelative As Boolean = False ' This flag will check if Relative has been used
        Dim sumWeigth As Double = 0
        sumWeigth += (If(txtWeightOfPopulation.Text Is Nothing, 0, Convert.ToDouble(Val(txtWeightOfPopulation.Text))) + (If(txtNumberOfFamilies.Text Is Nothing, 0, Convert.ToDouble(Val(txtNumberOfFamilies.Text)))) + (If(txtNoOfInsuredPopulation.Text Is Nothing, 0, Convert.ToDouble(Val(txtNoOfInsuredPopulation.Text)))))
        sumWeigth += (If(txtNoOfInseredFamilies.Text Is Nothing, 0, Convert.ToDouble(Val(txtNoOfInseredFamilies.Text))) + (If(txtNumberOfClaims.Text Is Nothing, 0, Convert.ToDouble(Val(txtNumberOfClaims.Text)))) + (If(txtAdjustedAmount.Text Is Nothing, 0, Convert.ToDouble(Val(txtAdjustedAmount.Text)))))
        'Changed by Salumu 10-12-2018 Start

        'Edited by Emmanuel 11/12/2018 
        If sumWeigth = 0 Then
            eProduct.WeightInsuredPopulation = 100
            sumWeigth += (If(txtWeightOfPopulation.Text Is Nothing, 0, Convert.ToDouble(Val(txtWeightOfPopulation.Text))) + (If(txtNumberOfFamilies.Text Is Nothing, 0, Convert.ToDouble(Val(txtNumberOfFamilies.Text)))) + (If(txtNoOfInsuredPopulation.Text Is Nothing, 0, Convert.ToDouble(Val(eProduct.WeightInsuredPopulation)))))
            sumWeigth += (If(txtNoOfInseredFamilies.Text Is Nothing, 0, Convert.ToDouble(Val(txtNoOfInseredFamilies.Text))) + (If(txtNumberOfClaims.Text Is Nothing, 0, Convert.ToDouble(Val(txtNumberOfClaims.Text)))) + (If(txtAdjustedAmount.Text Is Nothing, 0, Convert.ToDouble(Val(txtAdjustedAmount.Text)))))

        Else
            eProduct.WeightInsuredPopulation = Val(txtNoOfInsuredPopulation.Text)
        End If
        If sumWeigth > 100 Then
            Dim msg As String = imisgen.getMessage("M_WEIGHTMUSTBE100")
            imisgen.Alert(msg, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If
        If sumWeigth < 100 Then
            Dim msg As String = imisgen.getMessage("M_WEIGHTMUSTBE100")
            imisgen.Alert(msg, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
            'Edited by Emmanuel End 

        ElseIf Val(txtShareContribution.Text) > 100 Then
            imisgen.Alert(imisgen.getMessage("M_SHAREDCONTRIBUTIONPARCENT"), pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If
        Dim Changed As Boolean = CType(Me.Master.FindControl("hfDirty"), HiddenField).Value
        If Changed = False Then Changed = If(ddlDistrict.Attributes.Item("LoadValue") = ddlDistrict.SelectedValue, False, True)
        If Request.QueryString("action") = "duplicate" Then
            eProduct.ProdID = 0
        End If

        If Changed = True Then
            Try

                Dim LocationId As Integer = -1
                If ddlDistrict.SelectedValue > 0 Then
                    LocationId = ddlDistrict.SelectedValue
                ElseIf ddlRegion.SelectedValue > 0 Then
                    LocationId = ddlRegion.SelectedValue
                End If

                Dim eLocations As New IMIS_EN.tblLocations
                eLocations.LocationId = LocationId

                'If IsNumeric(ddlDistrict.SelectedValue) Then
                'If ddlDistrict.SelectedValue > 0 Then
                'If Val(ddlDistrict.SelectedValue) > 0 Then
                '    eLocations.LocationId = ddlDistrict.SelectedValue
                'ElseIf Val(ddlRegion.SelectedValue) > 0 Then
                '    eLocations.LocationId = ddlRegion.SelectedValue
                'Else
                '    eLocations.LocationId = -1
                'End If

                'End If
                'Else

                'End If

                eProduct.ProductName = txtProductName.Text
                eProduct.tblLocation = eLocations
                ''eProduct.DateFrom = if(Len(Trim(txtDateFrom.Text)) > 0, Date.Parse(txtDateFrom.Text), DBNull.Value)
                ''eProduct.DateTo = if(Len(Trim(txtDateTo.Text)) > 0, Date.Parse(txtDateTo.Text), DBNull.Value)

                If Not String.IsNullOrEmpty(txtDateFrom.Text) Then
                    eProduct.DateFrom = txtDateFrom.Text
                    'Else
                    '    lblMsg.Text = eProduct.DateFrom & imisgen.getMessage("M_WRONGDATEFORMAT")
                    '    Return
                End If

                If Not String.IsNullOrEmpty(txtDateTo.Text) Then
                    eProduct.DateTo = txtDateTo.Text
                    'Else
                    '    lblMsg.Text = eProduct.DateTo & imisgen.getMessage("M_WRONGDATEFORMAT")
                    '    Return
                End If

                Dim eproduct2 As New IMIS_EN.tblProduct
                eproduct2.ProdID = Val(ddlConversion.SelectedValue)
                eProduct.tblProduct2 = eproduct2


                If Not eProduct.ProdID = 0 Then
                    If ddlConversion.SelectedValue = eProduct.ProdID Then
                        Dim msg As String = imisgen.getMessage("M_WRONGCONVERSIONPRODUCTCODE")
                        Throw New Exception(msg)
                    End If
                End If

                If Not String.IsNullOrEmpty(txtLumpSum.Text) Then
                    eProduct.LumpSum = txtLumpSum.Text
                End If

                Dim MaxNumberMember As Integer = txtMaxNoOfMembers.Text
                If Not String.IsNullOrEmpty(MaxNumberMember) Then
                    eProduct.MemberCount = MaxNumberMember
                End If

                If Not String.IsNullOrEmpty(txtAdultPremium.Text) Then
                    eProduct.PremiumAdult = txtAdultPremium.Text
                End If

                If Not String.IsNullOrEmpty(txtChildPremium.Text) Then
                    eProduct.PremiumChild = txtChildPremium.Text
                End If


                If Not String.IsNullOrEmpty(txtInsurancrePeriod.Text) Then
                    eProduct.InsurancePeriod = txtInsurancrePeriod.Text
                End If
                'Additon for BEPHA: start
                If IsNumeric(txtGracePeriodRenewal.Text.Trim) Then
                    eProduct.GracePeriodRenewal = CInt(txtGracePeriodRenewal.Text.Trim)
                End If

                If IsNumeric(txtWaitingPeriod.Text.Trim) Then
                    eProduct.WaitingPeriod = CInt(txtWaitingPeriod.Text.Trim)
                End If
                If IsNumeric(txtMaxInstallments.Text.Trim) Then
                    eProduct.MaxInstallments = CInt(txtMaxInstallments.Text.Trim)
                End If

                If IsNumeric(txtRegLumpSum.Text.Trim) Then
                    eProduct.RegistrationLumpSum = CDec(txtRegLumpSum.Text.Trim)
                End If
                If IsNumeric(txtRegFee.Text.Trim) Then
                    eProduct.RegistrationFee = CDec(txtRegFee.Text.Trim)
                End If
                If IsNumeric(txtGenAssemblyLumpsum.Text.Trim) Then
                    eProduct.GeneralAssemblyLumpSum = CDec(txtGenAssemblyLumpsum.Text.Trim)
                End If
                If IsNumeric(txtGenAssemblyFee.Text.Trim) Then
                    eProduct.GeneralAssemblyFee = CDec(txtGenAssemblyFee.Text.Trim)
                End If
                If ddlCycle1Day.SelectedIndex > 0 And ddlCycle1Month.SelectedIndex > 0 Then
                    eProduct.StartCycle1 = ddlCycle1Day.SelectedValue & "-" & ddlCycle1Month.SelectedValue
                End If
                If ddlCycle2Day.SelectedIndex > 0 And ddlCycle2Month.SelectedIndex > 0 Then
                    eProduct.StartCycle2 = ddlCycle2Day.SelectedValue & "-" & ddlCycle2Month.SelectedValue
                End If
                If IsNumeric(txtMaxAmountAntenatal.Text.Trim) Then
                    eProduct.MaxAmountAntenatal = CDec(txtMaxAmountAntenatal.Text.Trim)
                End If
                If IsNumeric(txtNumAntenatal.Text.Trim) Then
                    eProduct.MaxNoAntenatal = CInt(txtNumAntenatal.Text.Trim)
                End If
                'Additon for BEPHA: end

                If Not String.IsNullOrEmpty(txtDedutibleForTreatment.Text) Then
                    eProduct.DedTreatment = txtDedutibleForTreatment.Text
                End If

                If Not String.IsNullOrEmpty(txtDedOPTreatment.Text) Then
                    eProduct.DedOPTreatment = txtDedOPTreatment.Text
                End If

                If Not String.IsNullOrEmpty(txtDedIPTreatment.Text) Then
                    eProduct.DedIPTreatment = txtDedIPTreatment.Text
                End If

                If Not String.IsNullOrEmpty(txtDedIPTreatment.Text) Then
                    eProduct.DedIPTreatment = txtDedIPTreatment.Text
                End If

                If Not String.IsNullOrEmpty(txtMaxTreatment.Text) Then
                    eProduct.MaxTreatment = txtMaxTreatment.Text
                End If

                If Not String.IsNullOrEmpty(txtMaxOPTreatment.Text) Then
                    eProduct.MaxOPTreatment = txtMaxOPTreatment.Text
                End If

                If Not String.IsNullOrEmpty(txtMaxIPTreatment.Text) Then
                    eProduct.MaxIPTreatment = txtMaxIPTreatment.Text
                End If

                If Not String.IsNullOrEmpty(txtDedPolicy.Text) Then
                    eProduct.DedPolicy = txtDedPolicy.Text
                End If

                If Not String.IsNullOrEmpty(txtDedOPPolicy.Text) Then
                    eProduct.DedOPPolicy = txtDedOPPolicy.Text
                End If

                If Not String.IsNullOrEmpty(txtDedIPPolicy.Text) Then
                    eProduct.DedIPPolicy = txtDedIPPolicy.Text
                End If

                If Not String.IsNullOrEmpty(txtDedIPPolicy.Text) Then
                    eProduct.DedIPPolicy = txtDedIPPolicy.Text
                End If

                If Not String.IsNullOrEmpty(txtMaxPolicy.Text) Then
                    eProduct.MaxPolicy = txtMaxPolicy.Text
                End If

                If Not String.IsNullOrEmpty(txtMaxOPPolicy.Text) Then
                    eProduct.MaxOPPolicy = txtMaxOPPolicy.Text
                End If

                If Not String.IsNullOrEmpty(txtMaxIPPOlicy.Text) Then
                    eProduct.MaxIPPolicy = txtMaxIPPOlicy.Text
                End If

                If Not String.IsNullOrEmpty(txtDedInsuree.Text) Then
                    eProduct.DedInsuree = txtDedInsuree.Text
                End If

                If Not String.IsNullOrEmpty(txtDedOPInsuree.Text) Then
                    eProduct.DedOPInsuree = txtDedOPInsuree.Text
                End If

                If Not String.IsNullOrEmpty(txtDedIPInsuree.Text) Then
                    eProduct.DedIPInsuree = txtDedIPInsuree.Text
                End If

                If Not String.IsNullOrEmpty(txtMaxInsuree.Text) Then
                    eProduct.MaxInsuree = txtMaxInsuree.Text
                End If

                If Not String.IsNullOrEmpty(txtMaxOPInsuree.Text) Then
                    eProduct.MaxOPInsuree = txtMaxOPInsuree.Text
                End If

                If Not String.IsNullOrEmpty(txtMaxIPInsuree.Text) Then
                    eProduct.MaxIPInsuree = txtMaxIPInsuree.Text
                End If

                If Not String.IsNullOrEmpty(txtGracePeriod.Text) Then
                    eProduct.GracePeriod = txtGracePeriod.Text
                End If

                'Additon for BEPHA: start
                If IsNumeric(txtNumConsultations.Text.Trim) Then
                    eProduct.MaxNoConsultation = CInt(txtNumConsultations.Text.Trim)
                End If
                If IsNumeric(txtMaxAmountConsultations.Text.Trim) Then
                    eProduct.MaxAmountConsultation = CDec(txtMaxAmountConsultations.Text.Trim)
                End If
                If IsNumeric(txtNumSurgeries.Text.Trim) Then
                    eProduct.MaxNoSurgery = CInt(txtNumSurgeries.Text.Trim)
                End If
                If IsNumeric(txtMaxAmountSurgeries.Text.Trim) Then
                    eProduct.MaxAmountSurgery = CDec(txtMaxAmountSurgeries.Text.Trim)
                End If
                If IsNumeric(txtNumDeliveries.Text.Trim) Then
                    eProduct.MaxNoDelivery = CInt(txtNumDeliveries.Text.Trim)
                End If
                If IsNumeric(txtMaxAmountDeliveries.Text.Trim) Then
                    eProduct.MaxAmountDelivery = CDec(txtMaxAmountDeliveries.Text.Trim)
                End If
                If IsNumeric(txtNumHospitalizations.Text.Trim) Then
                    eProduct.MaxNoHospitalizaion = CInt(txtNumHospitalizations.Text.Trim)
                End If
                If IsNumeric(txtMaxAmountHospitalizations.Text.Trim) Then
                    eProduct.MaxAmountHospitalization = CDec(txtMaxAmountHospitalizations.Text.Trim)
                End If
                If IsNumeric(txtNumVisits.Text.Trim) Then
                    eProduct.MaxNoVisits = CInt(txtNumVisits.Text.Trim)
                End If
                'Additon for BEPHA: end
                'Addition for Nepal >> Start
                If IsNumeric(txtMaxPolicyExtraMember.Text.Trim) Then
                    eProduct.MaxPolicyExtraMember = CDec(txtMaxPolicyExtraMember.Text.Trim)
                End If
                If IsNumeric(txtMaxIPPolicyExtraMember.Text.Trim) Then
                    eProduct.MaxPolicyExtraMemberIP = CDec(txtMaxIPPolicyExtraMember.Text.Trim)
                End If
                If IsNumeric(txtMaxOPPolicyExtraMember.Text.Trim) Then
                    eProduct.MaxPolicyExtraMemberOP = CDec(txtMaxOPPolicyExtraMember.Text.Trim)
                End If
                If IsNumeric(txtMaxPolicyMC.Text.Trim) Then
                    eProduct.MaxCeilingPolicy = CDec(txtMaxPolicyMC.Text.Trim)
                End If
                If IsNumeric(txtMaxIPPolicyMC.Text.Trim) Then
                    eProduct.MaxCeilingPolicyIP = CDec(txtMaxIPPolicyMC.Text.Trim)
                End If
                If IsNumeric(txtMaxOPPolicyMC.Text.Trim) Then
                    eProduct.MaxCeilingPolicyOP = CDec(txtMaxOPPolicyMC.Text.Trim)
                End If
                'Addition for Nepal >> End
                If IsNumeric(txtRecurrence.Text.Trim) Then
                    eProduct.Recurrence = CDec(txtRecurrence.Text.Trim)
                End If
                eProduct.PeriodRelPrices = If(ddlDistribution.SelectedValue = "0", Nothing, ddlDistribution.SelectedValue)
                eProduct.PeriodRelPricesOP = If(ddlDistributionOP.SelectedValue = "0", Nothing, ddlDistributionOP.SelectedValue)
                eProduct.PeriodRelPricesIP = If(ddlDistributionIP.SelectedValue = "0", Nothing, ddlDistributionIP.SelectedValue)
                eProduct.AccCodePremiums = txtAccCodePremiums.Text
                eProduct.AccCodeRemuneration = txtAccCodeRemuneration.Text

                If Not ValidateDeductableCeilingValueCombination() Then
                    imisgen.Alert(imisgen.getMessage("M_WRONGDEDUCTABLECEILINGCOMBINATION"), pnlButtons, alertPopupTitle:="IMIS")
                    Exit Sub
                End If

                eProduct.AuditUserID = imisgen.getUserId(Session("User"))

                'Addition for Nepal >> Start
                If IsNumeric(txtThresholdMembers.Text.Trim) Then eProduct.Threshold = txtThresholdMembers.Text.Trim
                If IsNumeric(txtRenewalDiscountPercentage.Text.Trim) Then eProduct.RenewalDiscountPerc = txtRenewalDiscountPercentage.Text.Trim
                If IsNumeric(txtRenewalDiscountPeriod.Text.Trim) Then eProduct.RenewalDiscountPeriod = txtRenewalDiscountPeriod.Text.Trim
                If IsNumeric(txtAdministrationPeriod.Text.Trim) Then eProduct.AdministrationPeriod = txtAdministrationPeriod.Text.Trim
                If ddlCycle3Day.SelectedIndex > 0 And ddlCycle3Month.SelectedIndex > 0 Then
                    eProduct.StartCycle3 = ddlCycle3Day.SelectedValue & "-" & ddlCycle3Month.SelectedValue
                End If
                If ddlCycle4Day.SelectedIndex > 0 And ddlCycle4Month.SelectedIndex > 0 Then
                    eProduct.StartCycle4 = ddlCycle4Day.SelectedValue & "-" & ddlCycle4Month.SelectedValue
                End If
                If IsNumeric(txtEnrolmentDiscountPerc.Text.Trim) Then eProduct.EnrolmentDiscountPerc = txtEnrolmentDiscountPerc.Text
                If IsNumeric(txtEnrolmentDiscountPeriod.Text.Trim) Then eProduct.EnrolmentDiscountPeriod = txtEnrolmentDiscountPeriod.Text
                'Addition for Nepal >> End

                If ddlCeilingInterpretation.SelectedValue.Length > 0 Then eProduct.CeilingInterpretation = ddlCeilingInterpretation.SelectedValue

                '===>Capitation Changes<===
                If ddlLevel1.SelectedValue <> "" Then
                    eProduct.Level1 = ddlLevel1.SelectedValue
                End If
                If ddlLevel2.SelectedValue <> "" Then
                    eProduct.Level2 = ddlLevel2.SelectedValue
                End If
                If ddlLevel3.SelectedValue <> "" Then
                    eProduct.Level3 = ddlLevel3.SelectedValue
                End If
                If ddlLevel4.SelectedValue <> "" Then
                    eProduct.Level4 = ddlLevel4.SelectedValue
                End If


                If ddlSubLevel1.SelectedValue <> "" Then
                    eProduct.Sublevel1 = ddlSubLevel1.SelectedValue
                End If
                If ddlSubLevel2.SelectedValue <> "" Then
                    eProduct.Sublevel2 = ddlSubLevel2.SelectedValue
                End If
                If ddlSubLevel3.SelectedValue <> "" Then
                    eProduct.Sublevel3 = ddlSubLevel3.SelectedValue
                End If
                If ddlSubLevel4.SelectedValue <> "" Then
                    eProduct.Sublevel4 = ddlSubLevel4.SelectedValue
                End If

                eProduct.ShareContribution = Val(txtShareContribution.Text)
                eProduct.WeightPopulation = Val(txtWeightOfPopulation.Text)
                eProduct.WeightNumberFamilies = Val(txtNumberOfFamilies.Text)


                eProduct.WeightNumberInsuredFamilies = Val(txtNoOfInseredFamilies.Text)
                eProduct.WeightNumberVisits = Val(txtNumberOfClaims.Text)
                eProduct.WeightAdjustedAmount = Val(txtAdjustedAmount.Text)



                chk = product.SaveProduct(eProduct)


                If chk = 0 Then
                    Session("msg") = eProduct.ProductCode & imisgen.getMessage("M_Inserted")
                ElseIf chk = 1 Then
                    Session("msg") = eProduct.ProductCode & imisgen.getMessage("M_Updated")
                ElseIf chk = 2 Then
                    imisgen.Alert(eProduct.ProductCode & imisgen.getMessage("M_Exists"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If
            Catch ex As Exception
                'lblMsg.Text = ex.Message
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlMedicalItems, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
                Return
            End Try
        End If

        Try
            Dim eProductItem As New IMIS_EN.tblProductItems
            Dim eProductServices As New IMIS_EN.tblProductServices

            For Each row As GridViewRow In gvMedicalItems.Rows
                If CheckDifference(gvMedicalItems, row.RowIndex) = True Then
                    Dim eItems As New IMIS_EN.tblItems

                    If Not Request.QueryString("action") = "duplicate" Then
                        If Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("ProdItemID").ToString = "" Then
                            eProductItem.ProdItemID = gvMedicalItems.DataKeys.Item(row.RowIndex).Values("ProdItemID")
                        Else
                            eProductItem.ProdItemID = 0
                        End If
                    End If
                    'eProductItem.ProdItemID = 0
                    eItems.ItemID = gvMedicalItems.DataKeys.Item(row.RowIndex).Values("ItemId")
                    eProductItem.tblItems = eItems

                    eProductItem.LimitationType = if(String.IsNullOrEmpty(CType(row.Cells(6).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(6).Controls(1), TextBox).Text))
                    'Changes for Nepal >> Start
                    eProductItem.LimitationTypeR = if(String.IsNullOrEmpty(CType(row.Cells(7).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(7).Controls(1), TextBox).Text))
                    eProductItem.LimitationTypeE = if(String.IsNullOrEmpty(CType(row.Cells(8).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(8).Controls(1), TextBox).Text))
                    'Changes for Nepal >> End
                    eProductItem.PriceOrigin = if(String.IsNullOrEmpty(CType(row.Cells(9).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(9).Controls(1), TextBox).Text))
                    If eProductItem.PriceOrigin = "R" Then bRelative = True
                    If Not CType(row.Cells(10).Controls(1), TextBox).Text = "" Then
                        eProductItem.LimitAdult = CType(row.Cells(10).Controls(1), TextBox).Text
                    End If
                    'Changes for Nepal >> Start
                    If Not CType(row.Cells(11).Controls(1), TextBox).Text = "" Then
                        eProductItem.LimitAdultR = CType(row.Cells(11).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(12).Controls(1), TextBox).Text = "" Then
                        eProductItem.LimitAdultE = CType(row.Cells(12).Controls(1), TextBox).Text
                    End If
                    'Changes for Nepal >> End
                    If Not CType(row.Cells(13).Controls(1), TextBox).Text = "" Then
                        eProductItem.LimitChild = CType(row.Cells(13).Controls(1), TextBox).Text
                    End If
                    'Changes for Nepal >> Start
                    If Not CType(row.Cells(14).Controls(1), TextBox).Text = "" Then
                        eProductItem.LimitChildR = CType(row.Cells(14).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(15).Controls(1), TextBox).Text = "" Then
                        eProductItem.LimitChildE = CType(row.Cells(15).Controls(1), TextBox).Text
                    End If
                    'Changes for Nepal >> End
                    'Extra for Beptha >>> Start
                    If Not CType(row.Cells(16).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductItem.LimitNoAdult = CType(row.Cells(16).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(17).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductItem.LimitNoChild = CType(row.Cells(17).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(18).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductItem.WaitingPeriodAdult = CType(row.Cells(18).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(19).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductItem.WaitingPeriodChild = CType(row.Cells(19).Controls(1), TextBox).Text
                    End If
                    'Extra for Bepha >>> End
                    'Changes for Nepal >> Start
                    If Not CType(row.Cells(20).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductItem.CeilingExclusionAdult = CType(row.Cells(20).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(21).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductItem.CeilingExclusionChild = CType(row.Cells(21).Controls(1), TextBox).Text
                    End If
                    'Changes for Nepal >> End


                   

                    '===>END Capitation Changes<===
                    eProductItem.tblProduct = eProduct
                    eProductItem.AuditUserID = eProduct.AuditUserID

                    product.ChangeProductItems(eProductItem)
                    If Changed = False Then Session("msg") = eProduct.ProductName & imisgen.getMessage("M_Updated")
                    Changed = True
                Else
                    'If checked value = true then see if there were any changes else do nothing

                    Dim chkSelect As CheckBox = CType(gvMedicalItems.Rows(row.RowIndex).Cells(0).Controls(1), CheckBox)
                    If chkSelect.Checked = True Then

                        If Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitationType").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(6).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("PriceOrigin").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(9).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitAdult").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(10).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitChild").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(13).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitNoAdult").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(16).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitNoChild").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(17).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("WaitingPeriodAdult").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(18).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("WaitingPeriodChild").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(19).Controls(1), TextBox).Text Then
                            'Changes for Nepal >> Start
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitationTypeR").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(7).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitationTypeE").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(8).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitAdultR").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(11).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitAdultE").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(12).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitChildR").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(14).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("LimitChildE").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(15).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("CeilingExclusionAdult").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(20).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalItems.DataKeys.Item(row.RowIndex).Values("CeilingExclusionChild").ToString = CType(gvMedicalItems.Rows(row.RowIndex).Cells(21).Controls(1), TextBox).Text Then
                            'Changes for Nepal >> End
                        Else
                            If gvMedicalItems.DataKeys.Item(row.RowIndex).Values("PriceOrigin") = "R" Then bRelative = True
                            Continue For
                        End If

                        If Not Request.QueryString("action") = "duplicate" Then
                            eProductItem.ProdItemID = gvMedicalItems.DataKeys.Item(row.RowIndex).Values("ProdItemID")
                        End If

                        eProductItem.LimitationType = if(String.IsNullOrEmpty(CType(row.Cells(6).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(6).Controls(1), TextBox).Text))
                        'Changes for Nepal >> Start
                        eProductItem.LimitationTypeR = if(String.IsNullOrEmpty(CType(row.Cells(7).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(7).Controls(1), TextBox).Text))
                        eProductItem.LimitationTypeE = if(String.IsNullOrEmpty(CType(row.Cells(8).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(8).Controls(1), TextBox).Text))
                        'Changes for Nepal >> End
                        eProductItem.PriceOrigin = if(String.IsNullOrEmpty(CType(row.Cells(9).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(9).Controls(1), TextBox).Text))
                        If eProductItem.PriceOrigin = "R" Then bRelative = True
                        If Not CType(row.Cells(10).Controls(1), TextBox).Text = "" Then
                            eProductItem.LimitAdult = CType(row.Cells(10).Controls(1), TextBox).Text
                        End If
                        'Changes for Nepal >> Start
                        If Not CType(row.Cells(11).Controls(1), TextBox).Text = "" Then
                            eProductItem.LimitAdultR = CType(row.Cells(11).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(12).Controls(1), TextBox).Text = "" Then
                            eProductItem.LimitAdultE = CType(row.Cells(12).Controls(1), TextBox).Text
                        End If
                        'Changes for Nepal >> End
                        If Not CType(row.Cells(13).Controls(1), TextBox).Text = "" Then
                            eProductItem.LimitChild = CType(row.Cells(13).Controls(1), TextBox).Text
                        End If
                        'Changes for Nepal >> Start
                        If Not CType(row.Cells(14).Controls(1), TextBox).Text = "" Then
                            eProductItem.LimitChildR = CType(row.Cells(14).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(15).Controls(1), TextBox).Text = "" Then
                            eProductItem.LimitChildE = CType(row.Cells(15).Controls(1), TextBox).Text
                        End If
                        'Changes for Nepal >> End
                        'Extra for Beptha >>> Start
                        If Not CType(row.Cells(16).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductItem.LimitNoAdult = CType(row.Cells(16).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(17).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductItem.LimitNoChild = CType(row.Cells(17).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(18).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductItem.WaitingPeriodAdult = CType(row.Cells(18).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(19).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductItem.WaitingPeriodChild = CType(row.Cells(19).Controls(1), TextBox).Text
                        End If
                        'Extra for Bepha >>> End
                        'Changes for Nepal >> Start
                        If Not CType(row.Cells(20).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductItem.CeilingExclusionAdult = CType(row.Cells(20).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(21).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductItem.CeilingExclusionChild = CType(row.Cells(21).Controls(1), TextBox).Text
                        End If
                        'Changes for Nepal >> End

                        eProductItem.AuditUserID = eProduct.AuditUserID
                        product.UpdateProductItems(eProductItem)
                        If Changed = False Then Session("msg") = eProduct.ProductName & imisgen.getMessage("M_Updated")
                        Changed = True
                    End If
                End If
            Next
            For Each row As GridViewRow In gvMedicalServices.Rows
                If CheckDifference(gvMedicalServices, row.RowIndex) = True Then
                    Dim eServices As New IMIS_EN.tblServices
                    If Not Request.QueryString("action") = "duplicate" Then
                        If Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("ProdServiceId").ToString = "" Then
                            eProductServices.ProdServiceID = gvMedicalServices.DataKeys.Item(row.RowIndex).Values("ProdServiceId")
                        Else
                            eProductServices.ProdServiceID = 0
                        End If
                    End If


                    eServices.ServiceID = gvMedicalServices.DataKeys.Item(row.RowIndex).Values("ServiceID")
                    eProductServices.tblServices = eServices
                    eProductServices.tblProduct = eProduct
                    eProductServices.LimitationType = if(String.IsNullOrEmpty(CType(row.Cells(6).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(6).Controls(1), TextBox).Text))
                    'Changes for Nepal >> Start
                    eProductServices.LimitationTypeR = if(String.IsNullOrEmpty(CType(row.Cells(7).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(7).Controls(1), TextBox).Text))
                    eProductServices.LimitationTypeE = if(String.IsNullOrEmpty(CType(row.Cells(8).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(8).Controls(1), TextBox).Text))
                    'Changes for Nepal >> End
                    eProductServices.PriceOrigin = if(String.IsNullOrEmpty(CType(row.Cells(9).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(9).Controls(1), TextBox).Text))
                    If eProductServices.PriceOrigin = "R" Then bRelative = True
                    If Not CType(row.Cells(10).Controls(1), TextBox).Text = "" Then
                        eProductServices.LimitAdult = CType(row.Cells(10).Controls(1), TextBox).Text
                    End If
                    'Changes for Nepal >> Start
                    If Not CType(row.Cells(11).Controls(1), TextBox).Text = "" Then
                        eProductServices.LimitAdultR = CType(row.Cells(11).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(12).Controls(1), TextBox).Text = "" Then
                        eProductServices.LimitAdultE = CType(row.Cells(12).Controls(1), TextBox).Text
                    End If
                    'Changes for Nepal >> End
                    If Not CType(row.Cells(13).Controls(1), TextBox).Text = "" Then
                        eProductServices.LimitChild = CType(row.Cells(13).Controls(1), TextBox).Text
                    End If
                    'Changes for Nepal >> Start
                    If Not CType(row.Cells(14).Controls(1), TextBox).Text = "" Then
                        eProductServices.LimitChildR = CType(row.Cells(14).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(15).Controls(1), TextBox).Text = "" Then
                        eProductServices.LimitChildE = CType(row.Cells(15).Controls(1), TextBox).Text
                    End If
                    'Changes for Nepal >> End
                    'Extra for Beptha >>> Start
                    If Not CType(row.Cells(16).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductServices.LimitNoAdult = CType(row.Cells(16).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(17).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductServices.LimitNoChild = CType(row.Cells(17).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(18).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductServices.WaitingPeriodAdult = CType(row.Cells(18).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(19).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductServices.WaitingPeriodChild = CType(row.Cells(19).Controls(1), TextBox).Text
                    End If
                    'Extra for Bepha >>> End
                    'Changes for Nepal >> Start
                    If Not CType(row.Cells(20).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductServices.CeilingExclusionAdult = CType(row.Cells(20).Controls(1), TextBox).Text
                    End If
                    If Not CType(row.Cells(21).Controls(1), TextBox).Text.Trim = String.Empty Then
                        eProductServices.CeilingExclusionChild = CType(row.Cells(21).Controls(1), TextBox).Text
                    End If
                    'Changes for Nepal >> End
                    eProductServices.AuditUserID = eProduct.AuditUserID
                    product.ChangeProductServices(eProductServices)
                    If Changed = False Then Session("msg") = eProduct.ProductName & imisgen.getMessage("M_Updated")
                    Changed = True
                Else

                    Dim chkSelect As CheckBox = CType(gvMedicalServices.Rows(row.RowIndex).Cells(0).Controls(1), CheckBox)
                    If chkSelect.Checked = True Then


                        If Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitationType").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(6).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("PriceOrigin").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(9).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitAdult").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(10).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitChild").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(13).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitNoAdult").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(16).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitNoChild").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(17).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("WaitingPeriodAdult").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(18).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("WaitingPeriodChild").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(19).Controls(1), TextBox).Text Then
                            'Changes for Nepal >> Start
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitationTypeR").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(7).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitationTypeE").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(8).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitAdultR").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(11).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitAdultE").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(12).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitChildR").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(14).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("LimitChildE").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(15).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("CeilingExclusionAdult").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(20).Controls(1), TextBox).Text Then
                        ElseIf Not gvMedicalServices.DataKeys.Item(row.RowIndex).Values("CeilingExclusionChild").ToString = CType(gvMedicalServices.Rows(row.RowIndex).Cells(21).Controls(1), TextBox).Text Then
                            'Changes for Nepal >> End
                        Else
                            If gvMedicalServices.DataKeys.Item(row.RowIndex).Values("PriceOrigin").ToString = "R" Then bRelative = True
                            Continue For

                        End If

                        If Not Request.QueryString("action") = "duplicate" Then
                            eProductServices.ProdServiceID = gvMedicalServices.DataKeys.Item(row.RowIndex).Values("ProdServiceId")
                        End If

                        eProductServices.LimitationType = if(String.IsNullOrEmpty(CType(row.Cells(6).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(6).Controls(1), TextBox).Text))
                        'Changes for Nepal >> Start
                        eProductServices.LimitationTypeR = if(String.IsNullOrEmpty(CType(row.Cells(7).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(7).Controls(1), TextBox).Text))
                        eProductServices.LimitationTypeE = if(String.IsNullOrEmpty(CType(row.Cells(8).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(8).Controls(1), TextBox).Text))
                        'Changes for Nepal >> End
                        eProductServices.PriceOrigin = if(String.IsNullOrEmpty(CType(row.Cells(9).Controls(1), TextBox).Text), "-", UCase(CType(row.Cells(9).Controls(1), TextBox).Text))
                        If eProductServices.PriceOrigin = "R" Then bRelative = True
                        If Not CType(row.Cells(10).Controls(1), TextBox).Text = "" Then
                            eProductServices.LimitAdult = CType(row.Cells(10).Controls(1), TextBox).Text
                        End If
                        'Changes for Nepal >> Start
                        If Not CType(row.Cells(11).Controls(1), TextBox).Text = "" Then
                            eProductServices.LimitAdultR = CType(row.Cells(11).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(12).Controls(1), TextBox).Text = "" Then
                            eProductServices.LimitAdultE = CType(row.Cells(12).Controls(1), TextBox).Text
                        End If
                        'Changes for Nepal >> End
                        If Not CType(row.Cells(13).Controls(1), TextBox).Text = "" Then
                            eProductServices.LimitChild = CType(row.Cells(13).Controls(1), TextBox).Text
                        End If
                        'Changes for Nepal >> Start
                        If Not CType(row.Cells(14).Controls(1), TextBox).Text = "" Then
                            eProductServices.LimitChildR = CType(row.Cells(14).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(15).Controls(1), TextBox).Text = "" Then
                            eProductServices.LimitChildE = CType(row.Cells(15).Controls(1), TextBox).Text
                        End If
                        'Changes for Nepal >> End
                        'Extra for Beptha >>> Start
                        If Not CType(row.Cells(16).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductServices.LimitNoAdult = CType(row.Cells(16).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(17).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductServices.LimitNoChild = CType(row.Cells(17).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(18).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductServices.WaitingPeriodAdult = CType(row.Cells(18).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(19).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductServices.WaitingPeriodChild = CType(row.Cells(19).Controls(1), TextBox).Text
                        End If
                        'Extra for Bepha >>> End
                        'Changes for Nepal >> Start
                        If Not CType(row.Cells(20).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductServices.CeilingExclusionAdult = CType(row.Cells(20).Controls(1), TextBox).Text
                        End If
                        If Not CType(row.Cells(21).Controls(1), TextBox).Text.Trim = String.Empty Then
                            eProductServices.CeilingExclusionChild = CType(row.Cells(21).Controls(1), TextBox).Text
                        End If
                        'Changes for Nepal >> End

                        eProductServices.AuditUserID = eProduct.AuditUserID
                        product.UpdateProductServices(eProductServices)
                        If Changed = False Then Session("msg") = eProduct.ProductName & imisgen.getMessage("M_Updated")
                        Changed = True
                    End If
                End If

            Next


            eRelDistr.tblProduct = eProduct
            eRelDistr.AuditUserID = imisgen.getUserId(Session("User"))
            Dim txtbox As TextBox
            Dim BeforeDistrPerc As Decimal
            Dim AfterDistrPerc As Decimal
            Dim RowV As String
            For Each row As GridViewRow In gvHiddenDistribution.Rows
                txtbox = gvHiddenDistribution.Rows(row.RowIndex).Cells(4).Controls(1)
                BeforeDistrPerc = CType(gvHiddenDistribution.Rows(row.RowIndex).Cells(5).Text, Decimal)
                AfterDistrPerc = CType(txtbox.Text, Decimal)

                If Not Request.QueryString("action") = "duplicate" Then
                    eRelDistr.DistrID = CType(gvHiddenDistribution.Rows(row.RowIndex).Cells(0).Text, Integer)
                End If


                eRelDistr.DistrType = CType(gvHiddenDistribution.Rows(row.RowIndex).Cells(1).Text, Integer)
                eRelDistr.DistrCareType = CType(gvHiddenDistribution.Rows(row.RowIndex).Cells(2).Text, Char)
                eRelDistr.Period = CType(gvHiddenDistribution.Rows(row.RowIndex).Cells(3).Text, Integer)
                RowV = CType(gvHiddenDistribution.Rows(row.RowIndex).Cells(6).Text, String)
                eRelDistr.DistrPerc = AfterDistrPerc

                If Request.QueryString("Action") = "duplicate" Then
                    product.SaveRelDistributionPercentageChanges(eRelDistr)
                    If Changed = False Then Session("msg") = eProduct.ProductName & imisgen.getMessage("M_Updated")
                    Changed = True
                    Continue For
                End If
                If Not BeforeDistrPerc = AfterDistrPerc Or RowV = "n" Then
                    product.SaveRelDistributionPercentageChanges(eRelDistr)
                    If Changed = False Then Session("msg") = eProduct.ProductName & imisgen.getMessage("M_Updated")
                    Changed = True
                End If


            Next
            If ddlDistribution.SelectedValue = "0" And ddlDistributionIP.SelectedValue = "0" And ddlDistributionOP.SelectedValue = "0" Then
            Else
                bRelative = False
            End If
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlProduct, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
        Response.Redirect("FindProduct.aspx?p=" & eProduct.ProductCode & "&m=" & bRelative)
    End Sub
    Protected Function getTimeNumber(ByVal x As String) As Integer
        Select Case x
            Case "Y"
                Return 1
            Case "M"
                Return 12
            Case "Q"
                Return 4
            Case Else
                Return 0
        End Select
    End Function
    Protected Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_CANCEL.Click
        If String.IsNullOrEmpty(eProduct.ProductCode) Then
            eProduct.ProductCode = txtProductCode.Text
        End If
        Response.Redirect("FindProduct.aspx?p=" & eProduct.ProductCode)

    End Sub
    Protected Sub FillHiddenDistributionGridView()

        eRelDistr.tblProduct = eProduct
        Dim dt As DataTable = product.GetAllDistributionData(eRelDistr)
        Dim Rows() As DataRow
        Dim DistrCareTypeRowCount As Integer
        Dim DistrTypes(1) As Integer

        Rows = dt.Select("DistrCareType='B'")
        DistrCareTypeRowCount = Rows.Length

        Select Case DistrCareTypeRowCount
            Case 0
                ReDim DistrTypes(2)
                DistrTypes(0) = 1
                DistrTypes(1) = 4
                DistrTypes(2) = 12
                AddRowsToDistributionDataTable(dt, 17, "B", DistrTypes)
            Case 1
                DistrTypes(0) = 4
                DistrTypes(1) = 12
                AddRowsToDistributionDataTable(dt, 16, "B", DistrTypes)
            Case 4
                DistrTypes(0) = 1
                DistrTypes(1) = 12
                AddRowsToDistributionDataTable(dt, 13, "B", DistrTypes)
            Case 5
                ReDim DistrTypes(0)
                DistrTypes(0) = 12
                AddRowsToDistributionDataTable(dt, 12, "B", DistrTypes)
            Case 12
                DistrTypes(0) = 1
                DistrTypes(1) = 4
                AddRowsToDistributionDataTable(dt, 5, "B", DistrTypes)
            Case 13
                ReDim DistrTypes(0)
                DistrTypes(0) = 4
                AddRowsToDistributionDataTable(dt, 4, "B", DistrTypes)
            Case 16
                ReDim DistrTypes(0)
                DistrTypes(0) = 1
                AddRowsToDistributionDataTable(dt, 1, "B", DistrTypes)
        End Select

        Rows = dt.Select("DistrCareType='I'")
        DistrCareTypeRowCount = Rows.Length

        Select Case DistrCareTypeRowCount
            Case 0
                ReDim DistrTypes(2)
                DistrTypes(0) = 1
                DistrTypes(1) = 4
                DistrTypes(2) = 12
                AddRowsToDistributionDataTable(dt, 17, "I", DistrTypes)
            Case 1
                DistrTypes(0) = 4
                DistrTypes(1) = 12
                AddRowsToDistributionDataTable(dt, 16, "I", DistrTypes)
            Case 4
                DistrTypes(0) = 1
                DistrTypes(1) = 12
                AddRowsToDistributionDataTable(dt, 13, "I", DistrTypes)
            Case 5
                ReDim DistrTypes(0)
                DistrTypes(0) = 12
                AddRowsToDistributionDataTable(dt, 12, "I", DistrTypes)
            Case 12
                DistrTypes(0) = 1
                DistrTypes(1) = 4
                AddRowsToDistributionDataTable(dt, 5, "I", DistrTypes)
            Case 13
                ReDim DistrTypes(0)
                DistrTypes(0) = 4
                AddRowsToDistributionDataTable(dt, 4, "I", DistrTypes)
            Case 16
                ReDim DistrTypes(0)
                DistrTypes(0) = 1
                AddRowsToDistributionDataTable(dt, 1, "I", DistrTypes)
        End Select

        Rows = dt.Select("DistrCareType='O'")
        DistrCareTypeRowCount = Rows.Length

        Select Case DistrCareTypeRowCount
            Case 0
                ReDim DistrTypes(2)
                DistrTypes(0) = 1
                DistrTypes(1) = 4
                DistrTypes(2) = 12
                AddRowsToDistributionDataTable(dt, 17, "O", DistrTypes)
            Case 1
                DistrTypes(0) = 4
                DistrTypes(1) = 12
                AddRowsToDistributionDataTable(dt, 16, "O", DistrTypes)
            Case 4
                DistrTypes(0) = 1
                DistrTypes(1) = 12
                AddRowsToDistributionDataTable(dt, 13, "O", DistrTypes)
            Case 5
                ReDim DistrTypes(0)
                DistrTypes(0) = 12
                AddRowsToDistributionDataTable(dt, 12, "O", DistrTypes)
            Case 12
                DistrTypes(0) = 1
                DistrTypes(1) = 4
                AddRowsToDistributionDataTable(dt, 5, "O", DistrTypes)
            Case 13
                ReDim DistrTypes(0)
                DistrTypes(0) = 4
                AddRowsToDistributionDataTable(dt, 4, "O", DistrTypes)
            Case 16
                ReDim DistrTypes(0)
                DistrTypes(0) = 1
                AddRowsToDistributionDataTable(dt, 1, "O", DistrTypes)
        End Select


        gvHiddenDistribution.DataSource = dt
        gvHiddenDistribution.DataBind()

    End Sub
    Protected Sub AddRowsToDistributionDataTable(ByRef dt As DataTable, ByVal RowsToAdd As Integer, ByVal DistrCareType As Char, ByVal DistrTypes() As Integer)
        Try

            Dim dr As DataRow
            Dim Period As Integer = 1
            Dim DistrType As Integer = DistrTypes(0)
            Dim j As Integer
            Dim TypesLookupStep As Integer = 0

            For i As Integer = 1 To RowsToAdd
                dr = dt.NewRow
                dr("DistrID") = 0
                dr("Period") = Period
                dr("DistrCareType") = DistrCareType
                dr("DistrType") = DistrType
                dr("DistrPerc") = "0.00"
                dr("RowV") = "n"
                dt.Rows.Add(dr)

                j = TypesLookupStep
                While j <= DistrTypes.Length - 1
                    If Period = DistrTypes(j) Then
                        Period = 1
                        TypesLookupStep += 1

                        If DistrTypes.Length - 1 >= TypesLookupStep Then
                            DistrType = DistrTypes(TypesLookupStep)
                        End If

                        Exit While
                    End If
                    j += 1
                End While

                If j > DistrTypes.Length - 1 Then
                    Period += 1
                End If
            Next
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlProduct, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
    Private Sub btnConversion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConversion.Click
        ddlConversion.DataSource = product.GetProducts(imisgen.getUserId(Session("User")), True, Val(ddlRegion.SelectedValue), Val(ddlDistrict.SelectedValue))
        ddlConversion.DataValueField = "ProdId"
        ddlConversion.DataTextField = "ProductCode"
        ddlConversion.DataBind()
    End Sub
    Private Sub btnLoadMedicalServices_ServerClick(sender As Object, e As EventArgs) Handles btnLoadMedicalServices.Click
        Try
            LoadMedicalServices()
            btnLoadMedicalServices.Visible = False
            Checkbox1.Visible = True
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons)
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub btnLoadMedicalItems_ServerClick(sender As Object, e As EventArgs) Handles btnLoadMedicalItems.Click
        Try
            LoadMedicalItems()
            btnLoadMedicalItems.Visible = False
            Checkbox2.Visible = True
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons)
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
    End Sub
End Class
