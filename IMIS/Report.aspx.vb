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


Imports Microsoft.Reporting.WebForms
Partial Public Class Report
    Inherits System.Web.UI.Page
    Dim imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private Gender As New IMIS_BI.GenderBI


    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        If Not userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Report, Page) Then
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Report.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            RunPageSecurity()
            Try
                Dim refUrl As String = Request.ServerVariables("HTTP_REFERER")
                If Not Request.QueryString("tid") Is Nothing Then
                    Back.HRef = "Reports.aspx?tid=" & Request.QueryString("tid")
                Else
                    Back.HRef = refUrl
                End If
                Dim ds As New ReportDataSource()
                Dim ds1 As New DataSet

                Dim rpt As LocalReport = rptViewer.LocalReport

                Select Case Request.QueryString("r")
                    Case "ps"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptPolicyRenewalStatus.rdlc" ' this is the report this should also be passed to this page in a generic scenario
                        ds.Name = "ds_PolicyRenewalStatus"
                        Page.Title = imisgen.getMessage("L_POLICYSTATUSOVERV")

                        Dim Param(14) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPRSMainTitle", imisgen.getMessage("L_POLICYSTATUSOVERV", False))
                        Param(2) = New ReportParameter("paramPRSCHFID", imisgen.getMessage("L_CHFID", False))
                        Param(3) = New ReportParameter("paramPRSLastName", imisgen.getMessage("L_LASTNAME", False))
                        Param(4) = New ReportParameter("paramPRSOtherNam", imisgen.getMessage("L_OTHERNAMES", False))
                        Param(5) = New ReportParameter("paramPRSProdCod", imisgen.getMessage("L_PRODUCTCODE", False))
                        Param(6) = New ReportParameter("paramPRSProdNam", imisgen.getMessage("L_PRODUCTNAME", False))
                        Param(7) = New ReportParameter("paramPRSRenewDate", imisgen.getMessage("L_RENEWALDATE", False))
                        Param(8) = New ReportParameter("paramPRSProdVal", imisgen.getMessage("L_PRODUCTVALUE", False))
                        Param(9) = New ReportParameter("paramPRSTotalVal", imisgen.getMessage("L_TOTALVALUE", False))
                        Param(10) = New ReportParameter("paramPRSPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        '  Param(11) = New ReportParameter("prmAgent", imisgen.getMessage("R_AGENT"))
                        Param(11) = New ReportParameter("prmWard", imisgen.getMessage("L_WARD", False))
                        Param(12) = New ReportParameter("prmVillage", imisgen.getMessage("L_VILLAGE", False))
                        Param(13) = New ReportParameter("prmOfficer", imisgen.getMessage("L_OFFICER", False))
                        Param(14) = New ReportParameter("prmRegion", imisgen.getMessage("L_REGION", False))
                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "psj"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptPolicyRenewalJournal.rdlc"
                        ds.Name = "ds_uspSSRSPolicyRenewalPromptJournal"
                        Page.Title = imisgen.getMessage("L_POLICYRENEWALPROMPTJ")

                        Dim Param(11) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPRJMainTitle", imisgen.getMessage("L_POLICYRENEWALPROMPTJ", False))
                        Param(2) = New ReportParameter("paramPRJRenewID", imisgen.getMessage("L_RENEWALID", False))
                        Param(3) = New ReportParameter("paramPRJPromptDate", imisgen.getMessage("L_PROMPTDATE", False))
                        Param(4) = New ReportParameter("paramPRJRenewDate", imisgen.getMessage("L_RENEWALDATE", False))
                        Param(5) = New ReportParameter("paramPRJLastNam", imisgen.getMessage("L_LASTNAME", False))
                        Param(6) = New ReportParameter("paramPRJOtherNam", imisgen.getMessage("L_OTHERNAMES", False))
                        Param(7) = New ReportParameter("paramPRJProdCod", imisgen.getMessage("L_PRODUCTCODE", False))
                        Param(8) = New ReportParameter("paramPRJProdNam", imisgen.getMessage("L_PRODUCTNAME", False))
                        Param(9) = New ReportParameter("paramPRJSMSStatus", imisgen.getMessage("L_SMSSTATUS", False))
                        Param(10) = New ReportParameter("paramPRJCHFID", imisgen.getMessage("L_CHFID", False))
                        Param(11) = New ReportParameter("paramPRJPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "pc"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptPremiumCollection.rdlc"
                        ds.Name = "ds_uspSSRSPremiumCollection"
                        Page.Title = imisgen.getMessage("L_PREMIUMCOLLECTIONREPORT")

                        Dim Param(8) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPCMainTitle", imisgen.getMessage("L_PREMIUMCOLLECTIONREPORT", False))
                        Param(2) = New ReportParameter("paramPCPayDate", imisgen.getMessage("L_PAYDATE", False))
                        Param(3) = New ReportParameter("paramPCPayType", imisgen.getMessage("L_PAYMENTTYPE", False))
                        Param(4) = New ReportParameter("paramPCAmount", imisgen.getMessage("L_AMOUNT", False))
                        Param(5) = New ReportParameter("paramPCPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(6) = New ReportParameter("paramPCTotal", imisgen.getMessage("P_TOTALCOLLECTION", False)) '
                        Param(7) = New ReportParameter("paramPCCollection", imisgen.getMessage("P_COLLECTION", False))
                        Param(8) = New ReportParameter("paramPCAccCode", imisgen.getMessage("P_ACCCODE", False)) ''''
                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "p"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptProductSales.rdlc"
                        ds.Name = "ds_uspSSRSProductSales"
                        Page.Title = imisgen.getMessage("L_PRODUCTSALES")

                        Dim Param(7) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPSMainTitle", imisgen.getMessage("L_PRODUCTSALES", False))
                        Param(2) = New ReportParameter("paramPSEffectDate", imisgen.getMessage("L_EFFECTIVEDATE", False))
                        Param(3) = New ReportParameter("paramPSAmount", imisgen.getMessage("L_AMOUNT", False))
                        Param(4) = New ReportParameter("paramPSTotalSales", imisgen.getMessage("L_TOTALSALES", False))
                        Param(5) = New ReportParameter("paramPSPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(6) = New ReportParameter("paramPCTotal", imisgen.getMessage("P_TOTALCOLLECTION", False)) '
                        Param(7) = New ReportParameter("paramPCCollection", imisgen.getMessage("P_COLLECTION", False))
                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "pd"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptPremiumDistribution.rdlc"
                        ds.Name = "ds_uspSSRSPremiumDistribution"
                        Page.Title = imisgen.getMessage("T_PREMIUMDISTRIBUTION")

                        Dim Param(11) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPDMainTitle", imisgen.getMessage("T_PREMIUMDISTRIBUTION", False))
                        Param(2) = New ReportParameter("paramPDProdCod", imisgen.getMessage("L_PRODUCTCODE", False))
                        Param(3) = New ReportParameter("paramPDProdNam", imisgen.getMessage("L_PRODUCTNAME", False))
                        Param(4) = New ReportParameter("paramPDCollection", imisgen.getMessage("L_COLLECTION", False))
                        Param(5) = New ReportParameter("paramPDAllocated", imisgen.getMessage("L_ALLOCATED", False))
                        Param(6) = New ReportParameter("paramPDNotAllocated", imisgen.getMessage("L_NOTALLOCATED", False))
                        Param(7) = New ReportParameter("paramPDOverallTotal", imisgen.getMessage("L_OVERALLTOTAL", False))
                        Param(8) = New ReportParameter("paramPDPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(9) = New ReportParameter("paramPDMonth", imisgen.getMessage("L_MONTH", False))
                        Param(10) = New ReportParameter("ParameterPCDistrict", imisgen.getMessage("L_DISTRICT", False))
                        Param(11) = New ReportParameter("ParameterPCTotalIn", imisgen.getMessage("P_TOTALIN", False))
                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "fpj"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptFeedbackPrompt.rdlc"
                        ds.Name = "DataSet1_uspSSRSFeedbackPrompt"
                        Page.Title = imisgen.getMessage("L_FEEDBACKPROMPTJ")

                        Dim Param(19) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramFPMainTitle", imisgen.getMessage("L_FEEDBACKPROMPTJ", False))
                        Param(2) = New ReportParameter("paramFPDate", imisgen.getMessage("L_DATE", False))
                        Param(3) = New ReportParameter("paramFPClaimID", imisgen.getMessage("L_CLAIMID", False))
                        Param(4) = New ReportParameter("paramFPClaimCode", imisgen.getMessage("L_CLAIMCODE", False))
                        Param(5) = New ReportParameter("paramFPHf", imisgen.getMessage("L_HFNAME", False))
                        Param(6) = New ReportParameter("paramFPCHFID", imisgen.getMessage("L_CHFID", False))
                        Param(7) = New ReportParameter("paramFPLastNam", imisgen.getMessage("L_LASTNAME", False))
                        Param(8) = New ReportParameter("paramFPOtherNam", imisgen.getMessage("L_OTHERNAMES", False))
                        Param(9) = New ReportParameter("paramFPDiagnosis", imisgen.getMessage("L_DIAGNOSIS", False))
                        Param(10) = New ReportParameter("paramFPFrom", imisgen.getMessage("L_FROM", False))
                        Param(11) = New ReportParameter("paramFPTo", imisgen.getMessage("L_TO", False))
                        Param(12) = New ReportParameter("paramFPClaimVal", imisgen.getMessage("L_CLAIMVALUE", False))
                        Param(13) = New ReportParameter("paramFPSMS", "SMS")
                        Param(14) = New ReportParameter("paramFPPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(15) = New ReportParameter("paramDistrict", imisgen.getMessage("L_DISTRICT", False))
                        Param(16) = New ReportParameter("paramOfficer", imisgen.getMessage("L_ENROLMENTOFFICERS", False))
                        Param(17) = New ReportParameter("paramPhone", imisgen.getMessage("L_PHONE", False))
                        Param(18) = New ReportParameter("paramWard", imisgen.getMessage("L_WARD", False))
                        Param(19) = New ReportParameter("paramVillage", imisgen.getMessage("L_VILLAGE", False))

                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "pbh"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptHFProcessBatch.rdlc"
                        ds.Name = "DataSet1_uspSSRSProcessBatch"
                        Page.Title = imisgen.getMessage("L_HFWISEPROCESSBATCH")

                        Dim Param(7) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramHFProduct", imisgen.getMessage("L_PRODUCT", False))
                        Param(2) = New ReportParameter("paramHFRemun", imisgen.getMessage("L_REMUNERATED", False))
                        Param(3) = New ReportParameter("paramHFTotal", imisgen.getMessage("L_TOTAL", False))
                        Param(4) = New ReportParameter("paramHFMainTitle", imisgen.getMessage("L_HFWISEPROCESSBATCH", False))
                        Param(5) = New ReportParameter("paramHFPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(6) = New ReportParameter("paramAccountCode", imisgen.getMessage("L_ACCOUNTCODE", False))
                        Param(7) = New ReportParameter("paramTotalFor", imisgen.getMessage("L_TOTALFOR", False))

                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "pbp"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptProdProcessBatch.rdlc"
                        ds.Name = "DataSet1_uspSSRSProcessBatch"
                        Page.Title = imisgen.getMessage("L_PRODUCTWISEPROCESSBATCH")

                        Dim Param(5) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPPBMainTitle", imisgen.getMessage("L_PRODUCTWISEPROCESSBATCH", False))
                        Param(2) = New ReportParameter("paramPPBHF", imisgen.getMessage("L_HFNAME", False))
                        Param(3) = New ReportParameter("paramPPBRenum", imisgen.getMessage("L_REMUNERATED", False))
                        Param(4) = New ReportParameter("paramPPBTotal", imisgen.getMessage("L_TOTAL", False))
                        Param(5) = New ReportParameter("paramPPBPrintedOn", imisgen.getMessage("L_PRINTEDON", False))

                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "pip"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)

                        rpt.ReportPath = "Reports\rptPrimaryIndicatorsPolicies.rdlc"
                        ds.Name = "DataSet1_uspSSRSPrimaryIndicators1"
                        Page.Title = imisgen.getMessage("T_PRIMARYOPERATIONALINDICATORS-POLICIES")

                        Dim Param(22) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPIMainTitle", imisgen.getMessage("T_PRIMARYOPERATIONALINDICATORS-POLICIES", False))
                        Param(2) = New ReportParameter("paramPIProdCode", imisgen.getMessage("L_PRODUCTCODE", False))
                        Param(3) = New ReportParameter("paramPIProdNam", imisgen.getMessage("L_PRODUCTNAME", False))
                        Param(4) = New ReportParameter("paramPIPolicy", imisgen.getMessage("L_POLICY", False))
                        Param(5) = New ReportParameter("paramPINewPolicy", imisgen.getMessage("L_NEWPOLICY", False))
                        Param(6) = New ReportParameter("paramPISuspendedPly", imisgen.getMessage("L_SUSPENDEDPOLICY", False))
                        Param(7) = New ReportParameter("paramPIExpiredPly", imisgen.getMessage("L_EXPIREDPOLICY", False))
                        Param(8) = New ReportParameter("paramPIPolicyRenew", imisgen.getMessage("L_POLICYRENEWAL", False))
                        Param(9) = New ReportParameter("paramPIInsuree", imisgen.getMessage("L_INSUREE", False))
                        Param(10) = New ReportParameter("paramPINewInsuree", imisgen.getMessage("L_NEWINSUREE", False))
                        Param(11) = New ReportParameter("paramPIPremiumColted", imisgen.getMessage("L_PREMIUMCOLLECTED", False))
                        Param(12) = New ReportParameter("paramPIAvailablePrem", imisgen.getMessage("L_AVAILABLEPREMIUM", False))
                        Param(13) = New ReportParameter("paramPIOverallTotal", imisgen.getMessage("L_OVERALLTOTAL", False))
                        Param(14) = New ReportParameter("paramPITotalClaim", imisgen.getMessage("L_TOTALCLAIMS", False))
                        Param(15) = New ReportParameter("paramPIRemun", imisgen.getMessage("L_REMUNERATED", False))
                        Param(16) = New ReportParameter("paramPIRejectedClm", imisgen.getMessage("L_REJECTEDCLAIMS", False))
                        Param(17) = New ReportParameter("paramPIRTotalFor", imisgen.getMessage("L_TOTALFOR", False))
                        Param(18) = New ReportParameter("paramPIPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(19) = New ReportParameter("paramIsOtherGenderUsed", Gender.IsOtherGenderUSed())
                        Param(20) = New ReportParameter("paramPIMale", imisgen.getMessage("T_MALE", False))
                        Param(21) = New ReportParameter("paramPIFemale", imisgen.getMessage("T_FEMALE", False))
                        Param(22) = New ReportParameter("paramPIOther", imisgen.getMessage("T_OTHER", False))
                        rpt.SetParameters(Param)
                        ds.Value = dt

                        rpt.DataSources.Add(ds)

                    Case "pic"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)

                        rpt.ReportPath = "Reports\rptPrimaryIndicatorsClaims.rdlc"
                        ds.Name = "DataSet1_uspSSRSPrimaryIndicators2"
                        Page.Title = imisgen.getMessage("T_PRIMARYOPERATIONALINDICATORS-CLAIMS")

                        Dim Param(18) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPIMainTitle", imisgen.getMessage("T_PRIMARYOPERATIONALINDICATORS-CLAIMS", False))
                        Param(2) = New ReportParameter("paramPIProdCode", imisgen.getMessage("L_PRODUCTCODE", False))
                        Param(3) = New ReportParameter("paramPIProdNam", imisgen.getMessage("L_PRODUCTNAME", False))
                        Param(4) = New ReportParameter("paramPIPolicy", imisgen.getMessage("L_POLICY", False))
                        Param(5) = New ReportParameter("paramPINewPolicy", imisgen.getMessage("L_NEWPOLICY", False))
                        Param(6) = New ReportParameter("paramPISuspendedPly", imisgen.getMessage("L_SUSPENDEDPOLICY", False))
                        Param(7) = New ReportParameter("paramPIExpiredPly", imisgen.getMessage("L_EXPIREDPOLICY", False))
                        Param(8) = New ReportParameter("paramPIPolicyRenew", imisgen.getMessage("L_POLICYRENEWAL", False))
                        Param(9) = New ReportParameter("paramPIInsuree", imisgen.getMessage("L_INSUREE", False))
                        Param(10) = New ReportParameter("paramPINewInsuree", imisgen.getMessage("L_NEWINSUREE", False))
                        Param(11) = New ReportParameter("paramPIPremiumColted", imisgen.getMessage("L_PREMIUMCOLLECTED", False))
                        Param(12) = New ReportParameter("paramPIAvailablePrem", imisgen.getMessage("L_AVAILABLEPREMIUM", False))
                        Param(13) = New ReportParameter("paramPIOverallTotal", imisgen.getMessage("L_OVERALLTOTAL", False))
                        Param(14) = New ReportParameter("paramPITotalClaim", imisgen.getMessage("L_TOTALCLAIMS", False))
                        Param(15) = New ReportParameter("paramPIRemun", imisgen.getMessage("L_PAID", False))
                        Param(16) = New ReportParameter("paramPIRejectedClm", imisgen.getMessage("L_REJECTEDCLAIMS", False))
                        Param(17) = New ReportParameter("paramPITotalFor", imisgen.getMessage("L_TOTALFOR", False))
                        Param(18) = New ReportParameter("paramPIPrintedOn", imisgen.getMessage("L_PRINTEDON", False))

                        rpt.SetParameters(Param)
                        ds.Value = dt

                        rpt.DataSources.Add(ds)

                    Case "epi"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)

                        rpt.ReportPath = "Reports\rptPrimaryIndicatorsPolicies.rdlc"
                        ds.Name = "DataSet1_uspSSRSPrimaryIndicators1"
                        Page.Title = imisgen.getMessage("T_ENROLMENTPERFORMANCEINDICATORS")

                        Dim Param(22) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPIMainTitle", imisgen.getMessage("T_ENROLMENTPERFORMANCEINDICATORS", False))
                        Param(2) = New ReportParameter("paramPIProdCode", imisgen.getMessage("L_PRODUCTCODE", False))
                        Param(3) = New ReportParameter("paramPIProdNam", imisgen.getMessage("L_PRODUCTNAME", False))
                        Param(4) = New ReportParameter("paramPIPolicy", imisgen.getMessage("L_POLICY", False))
                        Param(5) = New ReportParameter("paramPINewPolicy", imisgen.getMessage("L_NEWPOLICY", False))
                        Param(6) = New ReportParameter("paramPISuspendedPly", imisgen.getMessage("L_SUSPENDEDPOLICY", False))
                        Param(7) = New ReportParameter("paramPIExpiredPly", imisgen.getMessage("L_EXPIREDPOLICY", False))
                        Param(8) = New ReportParameter("paramPIPolicyRenew", imisgen.getMessage("L_POLICYRENEWAL", False))
                        Param(9) = New ReportParameter("paramPIInsuree", imisgen.getMessage("L_INSUREE", False))
                        Param(10) = New ReportParameter("paramPINewInsuree", imisgen.getMessage("L_NEWINSUREE", False))
                        Param(11) = New ReportParameter("paramPIPremiumColted", imisgen.getMessage("L_PREMIUMCOLLECTED", False))
                        Param(12) = New ReportParameter("paramPIAvailablePrem", imisgen.getMessage("L_AVAILABLEPREMIUM", False))
                        Param(13) = New ReportParameter("paramPIOverallTotal", imisgen.getMessage("L_OVERALLTOTAL", False))
                        Param(14) = New ReportParameter("paramPITotalClaim", imisgen.getMessage("L_TOTALCLAIMS", False))
                        Param(15) = New ReportParameter("paramPIRemun", imisgen.getMessage("L_REMUNERATED", False))
                        Param(16) = New ReportParameter("paramPIRejectedClm", imisgen.getMessage("L_REJECTEDCLAIMS", False))
                        Param(17) = New ReportParameter("paramPIOfficerCode", imisgen.getMessage("L_OFFICERCODE", False))
                        Param(18) = New ReportParameter("paramPIPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(19) = New ReportParameter("paramIsOtherGenderUsed", Gender.IsOtherGenderUSed())
                        Param(20) = New ReportParameter("paramPIMale", imisgen.getMessage("T_MALE", False))
                        Param(21) = New ReportParameter("paramPIFemale", imisgen.getMessage("T_FEMALE", False))
                        Param(22) = New ReportParameter("paramPIOther", imisgen.getMessage("T_OTHER", False))

                        rpt.SetParameters(Param)
                        ds.Value = dt

                        rpt.DataSources.Add(ds)
                    Case "di"
                        ds1 = DirectCast(Session("report"), DataSet)

                        rpt.ReportPath = "Reports\rptDerivedIndicators.rdlc"
                        ds.Name = "DataSet1_uspSSRSDerivedIndicators1"
                        Page.Title = imisgen.getMessage("T_DERIVEDOPERATIONALINDICATORS")

                        Dim Param(15) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramDIMainTitle", imisgen.getMessage("T_DERIVEDOPERATIONALINDICATORS", False))
                        Param(2) = New ReportParameter("paramDIProdCode", imisgen.getMessage("L_PRODUCTCODE", False))
                        Param(3) = New ReportParameter("paramDIProdNam", imisgen.getMessage("L_PRODUCTNAME", False))
                        Param(4) = New ReportParameter("paramDIIncuredClmRatio", imisgen.getMessage("L_INCURREDCLAIMRATIO", False))
                        Param(5) = New ReportParameter("paramDIRenewRatio", imisgen.getMessage("L_RENEWALRATIO", False))
                        Param(6) = New ReportParameter("paramDIGrowRatio", imisgen.getMessage("L_GROWTHRATIO", False))
                        Param(7) = New ReportParameter("paramDIPrompOfClmSet", imisgen.getMessage("L_PROMPTNESSOFCLMSETTLEMENT", False))
                        Param(8) = New ReportParameter("paramDIClmPerInsure", imisgen.getMessage("L_CLAIMSPERINSUREE", False))
                        Param(9) = New ReportParameter("paramDIClmSetRatio", imisgen.getMessage("L_CLAIMSETTLEMENTRATIO", False))
                        Param(10) = New ReportParameter("paramDIAverageCostPerClm", imisgen.getMessage("L_AVERAGECOSTPERCLAIM", False))
                        Param(11) = New ReportParameter("paramDISatisfyLevel", imisgen.getMessage("L_SATISFACTIONLEVEL", False))
                        Param(12) = New ReportParameter("paramDIFeedBckResRatio", imisgen.getMessage("L_FEEDBACKRESPONSERATIO", False))
                        Param(13) = New ReportParameter("paramDIOverallTotal", imisgen.getMessage("L_OVERALLTOTAL", False))
                        Param(14) = New ReportParameter("paramDIPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(15) = New ReportParameter("paramTotalFor", imisgen.getMessage("L_TOTALFOR", False))
                        rpt.SetParameters(Param)
                        ds.Value = ds1.Tables(0)

                        rpt.DataSources.Add(ds)

                        ds = New ReportDataSource
                        ds.Name = "DataSet1_uspSSRSDerivedIndicators2"
                        ds.Value = ds1.Tables(1)

                        rpt.DataSources.Add(ds)

                    Case "ua"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptUserActivityReport.rdlc"
                        ds.Name = "DataSet_UserActivity_UserActivityData"
                        Page.Title = imisgen.getMessage("T_USERACTIVITYREPORT")

                        Dim Param(7) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramUAMainTitle", imisgen.getMessage("T_USERACTIVITYREPORT", False))
                        Param(2) = New ReportParameter("paramUAUserName", imisgen.getMessage("T_USERNAME", False))
                        Param(3) = New ReportParameter("paramUARecordType", imisgen.getMessage("L_RECORDTYPE", False))
                        Param(4) = New ReportParameter("paramUAActionType", imisgen.getMessage("L_ACTIONTYPE", False))
                        Param(5) = New ReportParameter("paramUARecordIdentity", imisgen.getMessage("L_RECORDIDENTITY", False))
                        Param(6) = New ReportParameter("paramUA_ActionTime", imisgen.getMessage("L_ACTIONTIME", False))
                        Param(7) = New ReportParameter("paramUAPrintedOn", imisgen.getMessage("L_PRINTEDON", False))

                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)
                    Case "sr"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptStatusofRegisters.rdlc"
                        ds.Name = "DataSet_StatusofRegister"
                        Page.Title = imisgen.getMessage("T_STATUSOFREGISTERS")

                        Dim Param(13) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramSRMainTitle", imisgen.getMessage("T_STATUSOFREGISTERS", False))
                        Param(2) = New ReportParameter("paramSRUNoofEnrolmentOfficers", imisgen.getMessage("L_NOOFENROLMENTOFFICERS", False))
                        Param(3) = New ReportParameter("paramSRNoofUsers", imisgen.getMessage("L_NOOFUSERS", False))
                        Param(4) = New ReportParameter("paramSRNoofInsuranceProducts", imisgen.getMessage("L_NOOFINSURANCEPRODUCTS", False))
                        Param(5) = New ReportParameter("paramSRNoofHealthFacilities", imisgen.getMessage("L_NOOFHEALTHFACILITIES", False))
                        Param(6) = New ReportParameter("paramSRNoofServicePricelists", imisgen.getMessage("L_NOOFSERVICEPRICELIST", False))
                        Param(7) = New ReportParameter("paramSRNoofItemPricelists", imisgen.getMessage("L_NOOFITEMPRICELIST", False))
                        Param(8) = New ReportParameter("paramSRNoofMedicalItems", imisgen.getMessage("L_NOOFMEDICALITEMS", False))
                        Param(9) = New ReportParameter("paramSRNoofServices", imisgen.getMessage("L_NOOFSERVICES", False))
                        Param(10) = New ReportParameter("paramSRNoofPayers", imisgen.getMessage("L_NOOFPAYERS", False))
                        Param(11) = New ReportParameter("paramSRDistrictName", imisgen.getMessage("L_DistrictName", False))
                        Param(12) = New ReportParameter("paramSRTotal", imisgen.getMessage("L_TOTAL", False))
                        Param(13) = New ReportParameter("paramSRPrintedOn", imisgen.getMessage("L_PRINTEDON", False))

                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "iwp"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptInsureesWithoutPhotos.rdlc"
                        ds.Name = "ds_InsureesWithoutPhotos"
                        Page.Title = imisgen.getMessage("T_INSUREESWITHOUTPHOTOS")

                        Dim Param(18) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPRSMainTitle", imisgen.getMessage("T_INSUREESWITHOUTPHOTOS", False))
                        Param(2) = New ReportParameter("paramPRSCHFID", imisgen.getMessage("L_CHFID", False))
                        Param(3) = New ReportParameter("paramPRSLastName", imisgen.getMessage("L_LASTNAME", False))
                        Param(4) = New ReportParameter("paramPRSOtherNam", imisgen.getMessage("L_OTHERNAMES", False))
                        Param(5) = New ReportParameter("paramPRSGender", imisgen.getMessage("L_GENDER", False))
                        Param(6) = New ReportParameter("paramPRSIsHead", imisgen.getMessage("L_ISHEAD", False))
                        Param(7) = New ReportParameter("paramPRSOfficerCode", imisgen.getMessage("L_OFFICERCODE", False))
                        Param(8) = New ReportParameter("paramPRSOfficerLastName", imisgen.getMessage("L_OFFICERLASTNAME", False))
                        Param(9) = New ReportParameter("paramPRSOfficerOtherName", imisgen.getMessage("L_OFFICEROTHERNAMES", False))
                        Param(10) = New ReportParameter("paramPRSOfficer", imisgen.getMessage("L_OFFICER", False))
                        Param(11) = New ReportParameter("paramPRSDistrict", imisgen.getMessage("L_DISTRICT", False))
                        Param(12) = New ReportParameter("paramPRSVillage", imisgen.getMessage("L_VILLAGE", False))
                        Param(13) = New ReportParameter("paramPRSWard", imisgen.getMessage("L_WARD", False))
                        Param(14) = New ReportParameter("paramYes", imisgen.getMessage("L_YES", False))
                        Param(15) = New ReportParameter("paramNo", imisgen.getMessage("L_NO", False))
                        Param(16) = New ReportParameter("paramPRSPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(17) = New ReportParameter("paramSRTotal", imisgen.getMessage("L_TOTAL", False))
                        Param(18) = New ReportParameter("paramOverallTotal", imisgen.getMessage("L_OVERALLTOTAL", False))
                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)
                    Case "pco"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptPaymentCategoryOverview.rdlc"
                        ds.Name = "ds_uspSSRSPaymentCategoryOverview"
                        Page.Title = imisgen.getMessage("T_PAYMENTCATEGORYOVERVIEW")

                        Dim Param(11) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPCMainTitle", imisgen.getMessage("T_PAYMENTCATEGORYOVERVIEW", False))
                        Param(2) = New ReportParameter("paramProdcode", imisgen.getMessage("L_PRODUCTCODE", False))
                        Param(3) = New ReportParameter("paramProdName", imisgen.getMessage("L_PRODUCTNAME", False))
                        Param(4) = New ReportParameter("paramRegFee", imisgen.getMessage("L_REGISTRATIONFEE", False))
                        Param(5) = New ReportParameter("paramGenFee", imisgen.getMessage("L_GENERALASSEMBLYFEE", False))
                        Param(6) = New ReportParameter("paramContribution", imisgen.getMessage("L_PREMIUM", False))
                        Param(7) = New ReportParameter("paramPhoto", imisgen.getMessage("T_PHOTOFEE", False))
                        Param(8) = New ReportParameter("paramDistrictName", imisgen.getMessage("L_DistrictName", False))
                        Param(9) = New ReportParameter("paramPCPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(10) = New ReportParameter("paramPCTotalFor", imisgen.getMessage("L_TOTALFOR", False))
                        Param(11) = New ReportParameter("paramPCTotal", imisgen.getMessage("L_TOTAL", False))
                        '
                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "mf"
                        Dim dt As DataTable = CType(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptMatchingFunds.rdlc"
                        ds.Name = "ds_dtGetMatchingFunds"
                        Page.Title = imisgen.getMessage("T_MATCHINGFUNDS")
                        Dim Param(14) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramUAMainTitle", imisgen.getMessage("T_MATCHINGFUNDS", False))
                        Param(2) = New ReportParameter("paramUAPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(3) = New ReportParameter("paramUAUserName", imisgen.getMessage("T_USERNAME", False))
                        Param(4) = New ReportParameter("paramWard", imisgen.getMessage("L_WARD", False))
                        Param(5) = New ReportParameter("paramVillage", imisgen.getMessage("L_VILLAGE", False))
                        Param(6) = New ReportParameter("paramCHFNumber", imisgen.getMessage("L_CHFID", False))
                        Param(7) = New ReportParameter("paramFullName", imisgen.getMessage("L_FULLNAME", False))
                        Param(8) = New ReportParameter("paramBirthDate", imisgen.getMessage("L_BIRTHDATE", False))
                        Param(9) = New ReportParameter("paramPaymentDate", imisgen.getMessage("L_PAYMENTDATE", False))
                        Param(10) = New ReportParameter("paramPaymentReceiptCode", imisgen.getMessage("L_RECEIPT", False))
                        Param(11) = New ReportParameter("paramPaymentAmount", imisgen.getMessage("L_PAYMENTAMOUNT", False))
                        Param(12) = New ReportParameter("paramPayer", imisgen.getMessage("L_PAYERS", False))
                        Param(13) = New ReportParameter("paramEnrollmentDate", imisgen.getMessage("L_ENROLDATE", False))
                        Param(14) = New ReportParameter("paramTotal", imisgen.getMessage("L_TOTAL", False))

                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "co"
                        Dim dt As DataTable = CType(Session("report"), DataTable)
                        Dim dtDistinct As New DataTable
                        Dim ServiceQTY As Object = Nothing
                        Dim ItemQTY As Object = Nothing
                        Dim TotalClaimed As Object = Nothing
                        Dim TotalAdjAmount As Object = Nothing
                        Dim TotalApproved As Object = Nothing
                        Dim TotalPaid As Object = Nothing


                        If Session("Scope") = 0 Then
                            rpt.ReportPath = "Reports\rptClaimOverviewClaimsOnly.rdlc"
                            Dim dtView As DataView = dt.DefaultView
                            dtDistinct = dtView.ToTable(True, New String() {"ClaimID", "Claimed", "Adjusted", "Approved", "Paid", "ClaimStatus"})

                            TotalClaimed = dtDistinct.Compute("SUM(Claimed)", "1=1")

                            TotalAdjAmount = dtDistinct.Compute("SUM(Adjusted)", "1=1")

                            TotalApproved = dtDistinct.Compute("SUM(Approved)", "1=1")

                            TotalPaid = dtDistinct.Compute("SUM(Paid)", "1=1")
                        ElseIf Session("Scope") = 1 Then
                            rpt.ReportPath = "Reports\rptClaimOverviewRejecteServItem.rdlc"

                            Dim dtViewDetail As DataView = dt.DefaultView
                            Dim dtDistinct1 As DataTable = dtViewDetail.ToTable(True, New String() {"ServiceID", "ItemID"})
                            ServiceQTY = dtDistinct1.Compute("SUM(ServiceID)", "1=1")

                            ItemQTY = dtDistinct1.Compute("SUM(ItemID)", "1=1")

                            Dim dtView1 As DataView = dt.DefaultView
                            dtDistinct = dtView1.ToTable(True, New String() {"ClaimID", "Claimed", "Adjusted", "Approved", "Paid"})

                            TotalClaimed = dtDistinct.Compute("SUM(Claimed)", "1=1")

                            TotalAdjAmount = dtDistinct.Compute("SUM(Adjusted)", "1=1")

                            TotalApproved = dtDistinct.Compute("SUM(Approved)", "1=1")

                            TotalPaid = dtDistinct.Compute("SUM(Paid)", "1=1")

                        Else
                            rpt.ReportPath = "Reports\rptClaimOverviewAllDetails.rdlc"
                            Dim dtView As DataView = dt.DefaultView
                            dtDistinct = dtView.ToTable(True, New String() {"ClaimID", "Claimed", "Adjusted", "Approved", "Paid", "ClaimStatus"})

                            TotalClaimed = dtDistinct.Compute("SUM(Claimed)", "1=1")

                            TotalAdjAmount = dtDistinct.Compute("SUM(Adjusted)", "1=1")

                            TotalApproved = dtDistinct.Compute("SUM(Approved)", "1=1")

                            TotalPaid = dtDistinct.Compute("SUM(Paid)", "1=1")

                        End If

                        ds.Name = "ds_dtGetClaimOverview"
                        Page.Title = imisgen.getMessage("T_CLAIMOVERVIEW")
                        Dim Param(47) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramUAMainTitle", imisgen.getMessage("T_CLAIMOVERVIEW", False))
                        Param(2) = New ReportParameter("paramUAPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(3) = New ReportParameter("paramUAUserName", imisgen.getMessage("T_USERNAME", False))

                        Param(4) = New ReportParameter("paramClaimCode", imisgen.getMessage("R_CLAIMCODE", False))
                        Param(5) = New ReportParameter("paramClaimDate", imisgen.getMessage("R_CLAIMDATE", False))
                        Param(6) = New ReportParameter("paramClaimAdminName", imisgen.getMessage("R_CLAIMADMINNAME", False))
                        Param(7) = New ReportParameter("paramVisitFrom", imisgen.getMessage("R_VISITFROM", False))
                        Param(8) = New ReportParameter("paramVisitTo", imisgen.getMessage("R_VISITTO", False))
                        Param(9) = New ReportParameter("paramCHFNumber", imisgen.getMessage("R_CHFID", False))
                        Param(10) = New ReportParameter("paramInsureeName", imisgen.getMessage("R_INSUREENAME", False))
                        Param(11) = New ReportParameter("paramClaimStatus", imisgen.getMessage("R_CLAIMSTATUS", False))
                        Param(12) = New ReportParameter("paramRejectionCode", imisgen.getMessage("R_CLAIMREJECTIONREASONCODE", False))
                        Param(13) = New ReportParameter("paramClaimTotalInitial", imisgen.getMessage("L_CLAIMED", False))
                        Param(14) = New ReportParameter("paramClaimTotalFinal", imisgen.getMessage("L_ADJUSTED", False))
                        Param(15) = New ReportParameter("paramRejectedServiceCode", imisgen.getMessage("R_REJECTEDSERVICECODE", False))
                        Param(16) = New ReportParameter("paramServiceRejectionReasonCode", imisgen.getMessage("R_SERVICEREJECTIONREASONCODE", False))
                        Param(17) = New ReportParameter("paramRejectedItemCode", imisgen.getMessage("R_REJECTEDITEMCODE", False))
                        Param(18) = New ReportParameter("paramItemRejectionReasonCode", imisgen.getMessage("R_ITEMREJECTIONREASONCODE", False))
                        Param(19) = New ReportParameter("paramServiceCode", imisgen.getMessage("R_SERVICECODE", False))
                        Param(20) = New ReportParameter("paramOriginalServices", imisgen.getMessage("R_ORIGINALSERVICES", False))
                        Param(21) = New ReportParameter("paramAdjustedServices", imisgen.getMessage("R_ADJUSTEDSERVICES", False))
                        Param(22) = New ReportParameter("paramItemCode", imisgen.getMessage("R_ITEMCODE", False))
                        Param(23) = New ReportParameter("paramOriginalItems", imisgen.getMessage("R_ORIGINALITEMS", False))
                        Param(24) = New ReportParameter("paramAdjustedItems", imisgen.getMessage("R_ADJUSTEDITEMS", False))
                        Param(25) = New ReportParameter("paramTotalClaimedT", imisgen.getMessage("R_TOTALCLAIMED", False))
                        Param(26) = New ReportParameter("paramTotalClaimsT", imisgen.getMessage("R_TOTALCLAIMS", False))

                        If ItemQTY Is Nothing Then
                            ItemQTY = 0
                        End If
                        If ServiceQTY Is Nothing Then
                            ServiceQTY = 0
                        End If
                        If TotalClaimed Is Nothing Then
                            TotalClaimed = 0
                        End If
                        If (TotalAdjAmount Is DBNull.Value Or TotalAdjAmount Is Nothing) Then
                            TotalAdjAmount = 0
                        End If
                        If TotalApproved Is Nothing Then
                            TotalApproved = 0
                        End If
                        If (TotalPaid Is DBNull.Value Or TotalPaid Is Nothing) Then
                            TotalPaid = 0
                        End If


                        Param(27) = New ReportParameter("paramTotalClaims", dtDistinct.Rows.Count)
                        Param(28) = New ReportParameter("paramTotalClaimed", TotalClaimed.ToString)
                        Param(29) = New ReportParameter("paramAdjustedAmount", imisgen.getMessage("L_PAID", False))
                        Param(30) = New ReportParameter("paramTotalAdjustedAmount", TotalAdjAmount.ToString)
                        'Param(31) = New ReportParameter("paramTotalAdjAmountT", imisgen.getMessage("R_TOTALADJUSTEDAMOUNT"))
                        Param(31) = New ReportParameter("paramTotalApproved", TotalApproved.ToString)
                        Param(32) = New ReportParameter("paramApproved", imisgen.getMessage("L_APPROVED", False))
                        Param(33) = New ReportParameter("paramTotalPaid", TotalPaid.ToString)
                        Param(34) = New ReportParameter("prmService", imisgen.getMessage("L_SERVICE", False))
                        Param(35) = New ReportParameter("prmQty", imisgen.getMessage("L_QTY", False))
                        Param(36) = New ReportParameter("paramServiceQty", ServiceQTY.ToString())
                        Param(37) = New ReportParameter("paramItemQty", ItemQTY.ToString())
                        Param(38) = New ReportParameter("prmAppQty", imisgen.getMessage("L_APPQTY", False))
                        Param(39) = New ReportParameter("prmPrice", imisgen.getMessage("L_PRICE", False))
                        Param(40) = New ReportParameter("prmAppValue", imisgen.getMessage("L_APPVALUE", False))
                        Param(41) = New ReportParameter("prmClaimServices", imisgen.getMessage("L_SERVICES", False))
                        Param(42) = New ReportParameter("prmClaimItems", imisgen.getMessage("L_ITEM", False))
                        Param(43) = New ReportParameter("prmJustification", imisgen.getMessage("L_JUSTIFICATION", False))
                        Param(44) = New ReportParameter("prmValuated", imisgen.getMessage("L_PRICEVALUATED", False))
                        Param(45) = New ReportParameter("prmTotalApproved", imisgen.getMessage("R_TOTALAPPROVED", False))
                        Param(46) = New ReportParameter("prmTotalAdjustedAmount", imisgen.getMessage("R_TOTALADJUSTEDAMOUINT", False))
                        Param(47) = New ReportParameter("prmTotalPaid", imisgen.getMessage("R_TOTALPAIDAMOUNT", False))
                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)
                    Case "c"
                        Dim _ds As DataSet = Session("report")
                        Back.HRef = "Claim.aspx?c=" & _ds.Tables("Claim").Rows(0)("ClaimUUID").ToString()
                        rpt.ReportPath = "Reports\rptClaim.rdlc"
                        Page.Title = imisgen.getMessage("T_CLAIM")

                        Dim Param(39) As ReportParameter '28
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramUAMainTitle", imisgen.getMessage("L_CLAIM", False))
                        Param(2) = New ReportParameter("paramUAPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(3) = New ReportParameter("paramUAUserName", imisgen.getMessage("T_USERNAME", False))
                        Param(4) = New ReportParameter("prmHFCode", imisgen.getMessage("L_HFCODE", False))
                        Param(5) = New ReportParameter("prmHFName", imisgen.getMessage("L_HFNAME", False))
                        Param(6) = New ReportParameter("prmVisitFrom", imisgen.getMessage("L_VISITDATEFROM", False))
                        Param(7) = New ReportParameter("prmVisitTo", imisgen.getMessage("L_VISITDATETO", False))
                        Param(8) = New ReportParameter("prmCHFID", imisgen.getMessage("L_CHFID", False))
                        Param(9) = New ReportParameter("prmInsureeName", imisgen.getMessage("L_INSUREE", False))
                        Param(10) = New ReportParameter("prmICD", imisgen.getMessage("L_ICD", False))
                        Param(11) = New ReportParameter("prmClaimCode", imisgen.getMessage("L_CLAIMCODE", False))
                        Param(12) = New ReportParameter("prmClaimDate", imisgen.getMessage("L_CLAIMDATE", False))
                        Param(13) = New ReportParameter("prmClaimTotal", imisgen.getMessage("L_CLAIMTOTAL", False))
                        Param(14) = New ReportParameter("prmICD1", imisgen.getMessage("L_SECONDARYDG1", False))
                        Param(15) = New ReportParameter("prmICD2", imisgen.getMessage("L_SECONDARYDG2", False))
                        Param(16) = New ReportParameter("prmICD3", imisgen.getMessage("L_SECONDARYDG3", False))
                        Param(17) = New ReportParameter("prmICD4", imisgen.getMessage("L_SECONDARYDG4", False))
                        Param(18) = New ReportParameter("prmClaimAdmin", imisgen.getMessage("L_CLAIMADMIN", False))
                        Param(19) = New ReportParameter("prmGuaranteeId", imisgen.getMessage("L_GUARANTEE", False))
                        Param(20) = New ReportParameter("prmVisitType", imisgen.getMessage("L_VISITTYPE", False))
                        Param(21) = New ReportParameter("prmCode", imisgen.getMessage("L_CODE", False))
                        Param(22) = New ReportParameter("prmQuantity", imisgen.getMessage("L_QUANTITY", False))
                        Param(23) = New ReportParameter("prmValue", imisgen.getMessage("L_PRICE", False))
                        Param(24) = New ReportParameter("prmExplanation", imisgen.getMessage("L_EXPLANATION", False))
                        Param(25) = New ReportParameter("prmName", imisgen.getMessage("L_NAME", False))


                        Dim ClaimServicesTotal As Double
                        For Each dr As DataRow In _ds.Tables("ClaimedServices").Rows
                            ClaimServicesTotal += dr("QtyProvided") * dr("PriceAsked")
                        Next
                        Dim ClaimItemsTotal As Double
                        For Each dr As DataRow In _ds.Tables("ClaimedItems").Rows
                            ClaimItemsTotal += dr("QtyProvided") * dr("PriceAsked")
                        Next
                        Param(26) = New ReportParameter("prmClaimPriceTotal", (ClaimServicesTotal + ClaimItemsTotal).ToString)
                        Param(27) = New ReportParameter("prmServicesExist", If(_ds.Tables("ClaimedServices").Rows.Count > 0, 1, 0).ToString)
                        Param(28) = New ReportParameter("prmItemsExist", If(_ds.Tables("ClaimedItems").Rows.Count > 0, 1, 0).ToString)


                        Param(29) = New ReportParameter("prmHospitalDeduction", imisgen.getMessage("L_HDEDUCTION", False))
                        Param(30) = New ReportParameter("prmNonHospitalDeduction", imisgen.getMessage("L_NHDEDUCTION", False))
                        Param(31) = New ReportParameter("prmHospitalCeiling", imisgen.getMessage("L_HCEILING", False))
                        Param(32) = New ReportParameter("prmNonHospitalCeiling", imisgen.getMessage("L_NHCEILING", False))

                        Param(33) = New ReportParameter("prmDed1", _ds.Tables("Policy").Rows(0)("Ded1").ToString)
                        Param(34) = New ReportParameter("prmDed2", _ds.Tables("Policy").Rows(0)("Ded2").ToString)
                        Param(35) = New ReportParameter("prmCeiling1", _ds.Tables("Policy").Rows(0)("Ceiling1").ToString)
                        Param(36) = New ReportParameter("prmCeiling2", _ds.Tables("Policy").Rows(0)("Ceiling2").ToString)

                        Param(37) = New ReportParameter("prmFreeServicePrice", imisgen.getMessage("L_FREESERVICEPRICE", False))
                        Param(38) = New ReportParameter("prmFreeItemPrice", imisgen.getMessage("L_FREEITEMPRICE", False))
                        Param(39) = New ReportParameter("prmTotal", imisgen.getMessage("L_TOTAL", False))
                        'Param(40) = New ReportParameter("prmClaimServices", imisgen.getMessage("L_SERVICES", False))
                        'Param(41) = New ReportParameter("prmClaimItems", imisgen.getMessage("L_ITEMS", False))

                        rpt.SetParameters(Param)
                        ds.Name = "ds_dtClaim"
                        ds.Value = _ds.Tables("Claim")
                        rpt.DataSources.Add(ds)
                        ds = New ReportDataSource("ds_dtClaimServices", _ds.Tables("ClaimedServices"))
                        rpt.DataSources.Add(ds)
                        ds = New ReportDataSource("ds_dtClaimItems", _ds.Tables("ClaimedItems"))
                        rpt.DataSources.Add(ds)

                    Case "pr"
                        Dim dt As DataTable = CType(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptPercentageReferral.rdlc"
                        ds.Name = "ds_Perc"
                        Page.Title = imisgen.getMessage("T_PERCENTAGEOFREFERRALS")

                        Dim Param(6) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPRSMainTitle", imisgen.getMessage("T_PERCENTAGEOFREFERRALS", False))
                        Param(2) = New ReportParameter("prmHF", imisgen.getMessage("L_HFACILITY", False))
                        Param(3) = New ReportParameter("prmTotalClaims", imisgen.getMessage("L_TOTALCLAIMS", False))
                        Param(4) = New ReportParameter("prmTotalOP", imisgen.getMessage("L_REFOP", False))
                        Param(5) = New ReportParameter("prmTotalIP", imisgen.getMessage("L_REFIP", False))
                        Param(6) = New ReportParameter("paramPRSPrintedOn", imisgen.getMessage("L_PRINTEDON", False))

                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "pbc"
                        Dim dt As DataTable
                        dt = DirectCast(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptHFProcessBatchWithClaims.rdlc"
                        ds.Name = "dtProcessBatchHFWithClaim"
                        Page.Title = imisgen.getMessage("L_HFWISEPROCESSBATCH")

                        Dim Param(19) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramHFMainTitle", imisgen.getMessage("L_HFWISEPROCESSBATCH", False))
                        Param(2) = New ReportParameter("paramHFPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(3) = New ReportParameter("paramGroupBy", Request.QueryString("group"))
                        Param(4) = New ReportParameter("paramClaimCode", imisgen.getMessage("L_CLAIMCODE", False))
                        Param(5) = New ReportParameter("paramDate", imisgen.getMessage("L_DATE", False))
                        Param(6) = New ReportParameter("paramClaimAdmin", imisgen.getMessage("R_CLAIMADMINNAME", False))
                        Param(7) = New ReportParameter("paramDateFrom", imisgen.getMessage("L_DATEFROM", False))
                        Param(8) = New ReportParameter("paramDateTo", imisgen.getMessage("L_DATETO", False))
                        Param(9) = New ReportParameter("paramCHFID", imisgen.getMessage("L_CHFID", False))
                        Param(10) = New ReportParameter("paramInsuree", imisgen.getMessage("R_INSUREENAME", False))
                        Param(11) = New ReportParameter("paramProduct", imisgen.getMessage("L_PRODUCT", False))
                        Param(12) = New ReportParameter("paramHF", imisgen.getMessage("L_HF", False))
                        Param(13) = New ReportParameter("paramClaimed", imisgen.getMessage("L_CLAIMED", False))
                        Param(14) = New ReportParameter("paramApproved", imisgen.getMessage("L_APPROVED", False))
                        Param(15) = New ReportParameter("paramAdjusted", imisgen.getMessage("L_ADJUSTED", False))
                        Param(16) = New ReportParameter("paramPaid", imisgen.getMessage("L_PAID", False))
                        Param(17) = New ReportParameter("paramAccountCode", imisgen.getMessage("L_ACCOUNTCODE", False))
                        Param(18) = New ReportParameter("paramTotalFor", imisgen.getMessage("L_TOTALFOR", False))
                        Param(19) = New ReportParameter("paramOverallTotal", imisgen.getMessage("L_OVERALLTOTAL", False))


                        rpt.SetParameters(Param)

                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "fio"
                        Dim dt As DataTable = CType(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptFamilesInsureesOverview.rdlc"

                        ds.Name = "ds_dtFamiliesInsureesOverview"
                        Page.Title = imisgen.getMessage("T_FAMILIESINSUREESOVERVIEW")
                        Dim Param(13) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPSMainTitle", imisgen.getMessage("T_FAMILIESINSUREESOVERVIEW", False))
                        Param(2) = New ReportParameter("paramPSPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(3) = New ReportParameter("paramPSUserName", imisgen.getMessage("T_USERNAME", False))

                        Param(4) = New ReportParameter("paramWard", imisgen.getMessage("L_WARD", False))
                        Param(5) = New ReportParameter("paramVillage", imisgen.getMessage("L_VILLAGE", False))
                        Param(6) = New ReportParameter("paramTotalFamily", imisgen.getMessage("L_TOTALFAMILY", False))
                        Param(7) = New ReportParameter("paramTotalInsuree", imisgen.getMessage("L_TOTALINSUREE", False))
                        Param(8) = New ReportParameter("paramCHFID", imisgen.getMessage("L_CHFID", False))
                        Param(9) = New ReportParameter("paramName", imisgen.getMessage("L_NAME", False))
                        Param(10) = New ReportParameter("paramStatus", imisgen.getMessage("L_STATUS", False))
                        Param(11) = New ReportParameter("paramEnrolDate", imisgen.getMessage("L_ENROLDATE", False))
                        Param(12) = New ReportParameter("paramRegion", imisgen.getMessage("L_REGION", False))
                        Param(13) = New ReportParameter("paramDistrict", imisgen.getMessage("L_DISTRICT", False))
                        rpt.SetParameters(Param)

                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "pi"
                        Dim dt As DataTable = CType(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptPendingInsurees.rdlc"

                        ds.Name = "ds_dtPendingInsurees"
                        Page.Title = imisgen.getMessage("T_PENDINGINSUREES")
                        Dim Param(7) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramPSMainTitle", imisgen.getMessage("T_PENDINGINSUREES", False))
                        Param(2) = New ReportParameter("paramPSPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(3) = New ReportParameter("paramPSUserName", imisgen.getMessage("T_USERNAME", False))
                        Param(4) = New ReportParameter("paramPhotoDate", imisgen.getMessage("R_PHOTODATE", False))
                        Param(5) = New ReportParameter("paramInsuranceNo", imisgen.getMessage("L_CHFID", False))
                        Param(6) = New ReportParameter("paramOfficer", imisgen.getMessage("R_ENROLLMENTOFFICER", False))
                        Param(7) = New ReportParameter("paramTotalInsurees", imisgen.getMessage("R_TOTALINSUREES", False))
                        rpt.SetParameters(Param)

                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "rnw"
                        Dim dt As DataTable = CType(Session("Report"), DataTable)
                        rpt.ReportPath = "Reports\rptRenewals.rdlc"

                        ds.Name = "ds_dtRenewals"
                        Page.Title = imisgen.getMessage("T_RENEWALS")
                        Dim param(12) As ReportParameter
                        param(0) = New ReportParameter("paramMainTitle", imisgen.getMessage("T_RENEWALS", False))
                        param(1) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        param(2) = New ReportParameter("paramPCPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        param(3) = New ReportParameter("paramOfficerCode", imisgen.getMessage("L_OFFICERCODE", False))
                        param(4) = New ReportParameter("paramOfficerName", imisgen.getMessage("L_ENROLMENTOFFICERS", False))
                        param(5) = New ReportParameter("paramWardName", imisgen.getMessage("L_WARD", False))
                        param(6) = New ReportParameter("paramVillageName", imisgen.getMessage("L_VILLAGE", False))
                        param(7) = New ReportParameter("paramCHFID", imisgen.getMessage("L_CHFID", False))
                        param(8) = New ReportParameter("paramInsureeName", imisgen.getMessage("L_INSUREE", False))
                        param(9) = New ReportParameter("paramEnrollDate", imisgen.getMessage("L_RENEWALDATE", False))
                        param(10) = New ReportParameter("paramReceipt", imisgen.getMessage("L_RECEIPT", False))
                        param(11) = New ReportParameter("paramAmount", imisgen.getMessage("L_AMOUNT", False))
                        param(12) = New ReportParameter("paramPayerName", imisgen.getMessage("L_PAYERS", False))

                        rpt.SetParameters(param)

                        ds.Value = dt
                        rpt.DataSources.Add(ds)
                    Case "ca"
                        Dim dt As DataTable = CType(Session("Report"), DataTable)
                        rpt.ReportPath = "Reports\rptCapitationPayment.rdlc"

                        ds.Name = "ds_CapitationPayment"
                        Page.Title = imisgen.getMessage("T_CAPITATIONPAYMENT")
                        Dim param(46) As ReportParameter


                        param(0) = New ReportParameter("paramTitle", imisgen.getMessage("T_CAPITATIONPAYMENT", False))
                        param(1) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        param(2) = New ReportParameter("paramLblLevel1", imisgen.getMessage("L_LEVEL1", False))
                        param(3) = New ReportParameter("paramLblSublevel1", imisgen.getMessage("L_SUBLEVEL1", False))
                        param(4) = New ReportParameter("paramLblLevel2", imisgen.getMessage("L_LEVEL2", False))
                        param(5) = New ReportParameter("paramLblSublevel2", imisgen.getMessage("L_SUBLEVEL2", False))
                        param(6) = New ReportParameter("paramLblLevel3", imisgen.getMessage("L_LEVEL3", False))
                        param(7) = New ReportParameter("paramLblSublevel3", imisgen.getMessage("L_SUBLEVEL3", False))
                        param(8) = New ReportParameter("paramLblLevel4", imisgen.getMessage("L_LEVEL4", False))
                        param(9) = New ReportParameter("paramLblSublevel4", imisgen.getMessage("L_SUBLEVEL4", False))
                        param(10) = New ReportParameter("paramLblShareContribution", imisgen.getMessage("L_SHAREOFCONTRIBUTION", False))
                        param(11) = New ReportParameter("paramLblWeightPopulation", imisgen.getMessage("L_WEIGHTOFPOPUATION", False))
                        param(12) = New ReportParameter("paramLblWeightNumberFamilies", imisgen.getMessage("L_WEGHTOFNUMBERFAMILIES", False))
                        param(13) = New ReportParameter("paramLblWeightInsuredPopulation", imisgen.getMessage("L_WEIGHTOFINSUREDPOPULATION", False))
                        param(14) = New ReportParameter("paramLblWeightNumberInsuredFamilies", imisgen.getMessage("L_WEIGHTOFNUMBERINSUREDFAMILIES", False))
                        param(15) = New ReportParameter("paramLblWeightAdjustedAmounts", imisgen.getMessage("L_WEIGHTOFADJUSTEDAMOUT", False))
                        param(16) = New ReportParameter("paramLblWeightNumberVisits", imisgen.getMessage("L_WEIGHTOFNUMBERVISITS", False))

                        param(17) = New ReportParameter("paramLevel1", IMIS_EN.eReports.Level1)
                        param(18) = New ReportParameter("paramSublevel1", IMIS_EN.eReports.Sublevel1)
                        param(19) = New ReportParameter("paramLevel2", IMIS_EN.eReports.Level2)
                        param(20) = New ReportParameter("paramSublevel2", IMIS_EN.eReports.Sublevel2)
                        param(21) = New ReportParameter("paramLevel3", IMIS_EN.eReports.Level3)
                        param(22) = New ReportParameter("paramSublevel3", IMIS_EN.eReports.Sublevel3)
                        param(23) = New ReportParameter("paramLevel4", IMIS_EN.eReports.Level4)
                        param(24) = New ReportParameter("paramSublevel4", IMIS_EN.eReports.Sublevel4)
                        param(25) = New ReportParameter("paramShareContribution", IMIS_EN.eReports.ShareContribution)
                        param(26) = New ReportParameter("paramWeightPopulation", IMIS_EN.eReports.WeightPopulation)
                        param(27) = New ReportParameter("paramWeightNumberFamilies", IMIS_EN.eReports.WeightNumberFamilies)
                        param(28) = New ReportParameter("paramWeightInsuredPopulation", IMIS_EN.eReports.WeightInsuredPopulation)
                        param(29) = New ReportParameter("paramWeightNumberInsuredFamilies", IMIS_EN.eReports.WeightNumberInsuredFamilies)
                        param(30) = New ReportParameter("paramWeightAdjustedAmount", IMIS_EN.eReports.WeightAdjustedAmount)
                        param(31) = New ReportParameter("paramWeightNumberVisits", IMIS_EN.eReports.WeightNumberVisits)

                        'Header Labels
                        param(32) = New ReportParameter("paramHFCode", imisgen.getMessage("L_HFCODE", False))
                        param(33) = New ReportParameter("paramHFName", imisgen.getMessage("L_HFNAME", False))
                        param(34) = New ReportParameter("paramAccountCode", imisgen.getMessage("L_ACCCODE", False))
                        param(35) = New ReportParameter("paramPopulation", imisgen.getMessage("L_POPULATION", False))
                        param(36) = New ReportParameter("paramNoFamilies", imisgen.getMessage("L_NOOFFAMILIES", False))
                        param(37) = New ReportParameter("paramNoInsuredPopulation", imisgen.getMessage("L_NOOFINSUREDPOPULATION", False))
                        param(38) = New ReportParameter("paramNoInsuedFamilies", imisgen.getMessage("L_NOOFINSURESFAMILY", False))
                        param(39) = New ReportParameter("paramNoClaims", imisgen.getMessage("L_NOOFCLAIMS", False))
                        param(40) = New ReportParameter("paramAmountAdjusted", imisgen.getMessage("R_CLAIMTOTALFINAL", False))
                        param(41) = New ReportParameter("paramCapitationPayment", imisgen.getMessage("L_CAPITATIONPAYMENT", False))


                        param(42) = New ReportParameter("paramUnitPrice", imisgen.getMessage("L_UNITPRICE", False))
                        param(43) = New ReportParameter("paramAllocatedContribution", imisgen.getMessage("L_ALLOCATEDCONTRIBUTION", False))
                        param(44) = New ReportParameter("paramDistrictTotal", imisgen.getMessage("L_DISTRICTTOTAL", False))
                        param(45) = New ReportParameter("paramRegionTotal", imisgen.getMessage("L_REGIONTOTAL", False))
                        param(46) = New ReportParameter("paramOverallTotal", imisgen.getMessage("L_OVERALLTOTAL", False))


                        rpt.SetParameters(param)

                        ds.Value = dt
                        rpt.DataSources.Add(ds)
                    Case "rp"
                        Dim dt As DataTable = CType(Session("Report"), DataTable)
                        rpt.ReportPath = "Reports\rptRejectedPhotos.rdlc"

                        ds.Name = "ds_RejectedPhotos"
                        Page.Title = imisgen.getMessage("T_REJECTEDPHOTOS")
                        Dim param(7) As ReportParameter
                        param(0) = New ReportParameter("paramPSMainTitle", imisgen.getMessage("T_REJECTEDPHOTOS", False))
                        param(1) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        param(2) = New ReportParameter("paramOfficer", imisgen.getMessage("R_ENROLLMENTOFFICER", False))
                        param(3) = New ReportParameter("paramPSPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        param(4) = New ReportParameter("paramInsuranceNo", imisgen.getMessage("L_CHFID", False))
                        param(5) = New ReportParameter("paramRejectedDate", imisgen.getMessage("T_REJECTEDDATE", False))
                        param(6) = New ReportParameter("paramTotalRejectedPhotos", imisgen.getMessage("T_TOTALREJECTEDPHOTOS", False))
                        param(7) = New ReportParameter("paramTotal", imisgen.getMessage("L_TOTAL", False))

                        rpt.SetParameters(param)

                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "cp"
                        Dim dt As DataTable = CType(Session("Report"), DataTable)
                        rpt.ReportPath = "Reports\rptContributionPayment.rdlc"


                        ds.Name = "ds_ContributionPayment"
                        Page.Title = imisgen.getMessage("T_CONTRIBUTIONPAYMENT")
                        Dim param(5) As ReportParameter
                        param(0) = New ReportParameter("paramPSMainTitle", imisgen.getMessage("T_CONTRIBUTIONPAYMENT", False))
                        param(1) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        param(2) = New ReportParameter("paramOfficer", imisgen.getMessage("R_ENROLLMENTOFFICER", False))
                        param(3) = New ReportParameter("paramPSPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        param(4) = New ReportParameter("paramControlNumber", imisgen.getMessage("L_CONTROLNUMBER", False))
                        param(5) = New ReportParameter("paramProductCode", imisgen.getMessage("L_PRODUCT", False))


                        rpt.SetParameters(param)

                        ds.Value = dt
                        rpt.DataSources.Add(ds)


                    Case "cna"
                        Dim dt As DataTable = CType(Session("Report"), DataTable)
                        rpt.ReportPath = "Reports\rptControlNumberAssignment.rdlc"


                        ds.Name = "ds_ControlNumberAssignment"
                        Page.Title = imisgen.getMessage("T_CONTROLNUMBERASSIGNMENT")
                        Dim param(2) As ReportParameter
                        param(0) = New ReportParameter("paramUAMainTitle", imisgen.getMessage("T_CONTROLNUMBERASSIGNMENT", False))
                        param(1) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        'param(2) = New ReportParameter("paramOfficer", imisgen.getMessage("R_ENROLLMENTOFFICER", False))
                        param(2) = New ReportParameter("paramPSPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        ' param(3) = New ReportParameter("paramControlNumber", imisgen.getMessage("L_CONTROLNUMBER", False))



                        rpt.SetParameters(param)

                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "oc"
                        Dim dt As DataTable = CType(Session("report"), DataTable)
                        rpt.ReportPath = "Reports\rptOverviewOfCommissions.rdlc"
                        ds.Name = "ds_uspSSRSGetOverviewCommissions"
                        Page.Title = imisgen.getMessage("T_OVERVIEWOFCOMMISSIONS")
                        Dim Param(26) As ReportParameter
                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramUAMainTitle", imisgen.getMessage("T_OVERVIEWOFCOMMISSIONS", False))
                        Param(2) = New ReportParameter("paramUAPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(3) = New ReportParameter("paramUAUserName", imisgen.getMessage("T_USERNAME", False))
                        Param(4) = New ReportParameter("paramOfficer", imisgen.getMessage("L_OFFICER", False))
                        Param(5) = New ReportParameter("paramProduct", imisgen.getMessage("L_PRODUCT", False))
                        Param(6) = New ReportParameter("paramCHFNumber", imisgen.getMessage("L_CHFID", False))
                        Param(7) = New ReportParameter("paramFullName", imisgen.getMessage("L_FULLNAME", False))
                        Param(8) = New ReportParameter("paramBirthDate", imisgen.getMessage("L_BIRTHDATE", False))
                        Param(9) = New ReportParameter("paramPaymentDate", imisgen.getMessage("L_PAYMENTDATE", False))
                        Param(10) = New ReportParameter("paramPaymentReceiptCode", imisgen.getMessage("L_RECEIPT", False))
                        Param(11) = New ReportParameter("paramPaymentAmount", imisgen.getMessage("L_PAYMENTAMOUNT", False))
                        Param(12) = New ReportParameter("paramPayer", imisgen.getMessage("L_PAYERS", False))
                        Param(13) = New ReportParameter("paramEnrollmentDate", imisgen.getMessage("L_ENROLDATE", False))
                        Param(14) = New ReportParameter("paramTotal", imisgen.getMessage("L_TOTAL", False))
                        Param(15) = New ReportParameter("Grouping", IMIS_EN.eReports.Grouping)
                        Param(16) = New ReportParameter("prmWard", imisgen.getMessage("L_WARD", False))
                        Param(17) = New ReportParameter("prmVillage", imisgen.getMessage("L_VILLAGE", False))
                        Param(18) = New ReportParameter("paramTotalCommissionRate", imisgen.getMessage("L_TOTALCOMMISSIONRATE", False))
                        Param(19) = New ReportParameter("prmTotalNumberOfPolicies", imisgen.getMessage("L_TOTALNUMBEROFPOLICIES", False))
                        Param(20) = New ReportParameter("prmTotalPrescribedContribution", imisgen.getMessage("R_TOTALOFALLPRESCRIBEDCONTRIBUTION", False))
                        Param(21) = New ReportParameter("prmTotalActualPayment", imisgen.getMessage("R_TOTALOFALLACTUALPAYMENTS", False))
                        Param(22) = New ReportParameter("prmCalculatedCommission", imisgen.getMessage("R_CALCULATEDCOMMISSION", False))
                        Param(23) = New ReportParameter("prmTotalNumberOfPoliciesFor", imisgen.getMessage("L_TOTALNUMBEROFPOLICIESFOR", False))
                        Param(24) = New ReportParameter("prmTotalPrescribedContributionFor", imisgen.getMessage("R_TOTALPRESCRIBEDCONTRIBUTIONFOR", False))
                        Param(25) = New ReportParameter("prmTotalActualPaymentsFor", imisgen.getMessage("R_TOTALACTUALPAYMENTSFOR", False))
                        Param(26) = New ReportParameter("prmCalculatedCommissionFor", imisgen.getMessage("R_CALCULATEDCOMMISSIONFOR", False))
                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)

                    Case "chr"
                        Dim dtDistinct As New DataTable
                        Dim ServiceQTY As Object = Nothing
                        Dim ItemQTY As Object = Nothing
                        Dim TotalClaimed As Object = Nothing
                        Dim TotalAdjAmount As Object = Nothing
                        Dim TotalApproved As Object = Nothing
                        Dim TotalPaid As Object = Nothing
                        Dim dt As DataTable = CType(Session("report"), DataTable)
                        'rpt.ReportPath = "Reports\rptClaimHistoryReport.rdlc"
                        ds.Name = "ds_dtGetClaimOverview"
                        Page.Title = imisgen.getMessage("T_CLAIMHISTORYREPORT")
                        Dim Param(48) As ReportParameter

                        Param(0) = New ReportParameter("paramSubtitle", IMIS_EN.eReports.SubTitle)
                        Param(1) = New ReportParameter("paramUAMainTitle", imisgen.getMessage("T_CLAIMHISTORYREPORT", False))
                        Param(2) = New ReportParameter("paramUAPrintedOn", imisgen.getMessage("L_PRINTEDON", False))
                        Param(3) = New ReportParameter("paramUAUserName", imisgen.getMessage("T_USERNAME", False))

                        Param(4) = New ReportParameter("paramClaimCode", imisgen.getMessage("L_CLAIMCODE", False))
                        Param(5) = New ReportParameter("paramClaimDate", imisgen.getMessage("L_CLAIMDATE", False))
                        Param(6) = New ReportParameter("paramClaimAdminName", imisgen.getMessage("R_CLAIMADMINNAME", False))
                        Param(7) = New ReportParameter("paramVisitFrom", imisgen.getMessage("R_VISITFROM", False))
                        Param(8) = New ReportParameter("paramVisitTo", imisgen.getMessage("R_VISITTO", False))
                        Param(9) = New ReportParameter("paramCHFNumber", imisgen.getMessage("R_CHFID", False))
                        Param(10) = New ReportParameter("paramInsureeName", imisgen.getMessage("L_PATIENTNAME", False))
                        Param(11) = New ReportParameter("paramClaimStatus", imisgen.getMessage("R_CLAIMSTATUS", False))
                        Param(12) = New ReportParameter("paramRejectionCode", imisgen.getMessage("R_CLAIMREJECTIONREASONCODE", False))
                        Param(13) = New ReportParameter("paramClaimTotalInitial", imisgen.getMessage("L_CLAIMED", False))
                        Param(14) = New ReportParameter("paramClaimTotalFinal", imisgen.getMessage("L_ADJUSTED", False))
                        Param(15) = New ReportParameter("paramRejectedServiceCode", imisgen.getMessage("R_REJECTEDSERVICECODE", False))
                        Param(16) = New ReportParameter("paramServiceRejectionReasonCode", imisgen.getMessage("R_SERVICEREJECTIONREASONCODE", False))
                        Param(17) = New ReportParameter("paramRejectedItemCode", imisgen.getMessage("R_REJECTEDITEMCODE", False))
                        Param(18) = New ReportParameter("paramItemRejectionReasonCode", imisgen.getMessage("R_ITEMREJECTIONREASONCODE", False))
                        Param(19) = New ReportParameter("paramServiceCode", imisgen.getMessage("R_SERVICECODE", False))
                        Param(20) = New ReportParameter("paramOriginalServices", imisgen.getMessage("R_ORIGINALSERVICES", False))
                        Param(21) = New ReportParameter("paramAdjustedServices", imisgen.getMessage("R_ADJUSTEDSERVICES", False))
                        Param(22) = New ReportParameter("paramItemCode", imisgen.getMessage("R_ITEMCODE", False))
                        Param(23) = New ReportParameter("paramOriginalItems", imisgen.getMessage("R_ORIGINALITEMS", False))
                        Param(24) = New ReportParameter("paramAdjustedItems", imisgen.getMessage("R_ADJUSTEDITEMS", False))
                        Param(25) = New ReportParameter("paramTotalClaimedT", imisgen.getMessage("R_TOTALCLAIMED", False))
                        Param(26) = New ReportParameter("paramTotalClaimsT", imisgen.getMessage("R_TOTALCLAIMS", False))

                        If ItemQTY Is Nothing Then
                            ItemQTY = 0
                        End If
                        If ServiceQTY Is Nothing Then
                            ServiceQTY = 0
                        End If
                        If TotalClaimed Is Nothing Then
                            TotalClaimed = 0
                        End If
                        If (TotalAdjAmount Is DBNull.Value Or TotalAdjAmount Is Nothing) Then
                            TotalAdjAmount = 0
                        End If
                        If TotalApproved Is Nothing Then
                            TotalApproved = 0
                        End If
                        If (TotalPaid Is DBNull.Value Or TotalPaid Is Nothing) Then
                            TotalPaid = 0
                        End If



                        Dim dtView As DataView = dt.DefaultView
                        If (Session("Scope") = 2 Or Session("Scope") = -1) Then
                            rpt.ReportPath = "Reports\rptClaimOverviewHistoryAllDetails.rdlc"

                            dtDistinct = dtView.ToTable(True, New String() {"ClaimID", "Claimed", "Adjusted", "Approved", "Paid", "ClaimStatus"})

                            TotalClaimed = dtDistinct.Compute("SUM(Claimed)", "1=1")
                            If (TotalClaimed Is DBNull.Value Or TotalClaimed Is Nothing) Then
                                TotalClaimed = 0
                            End If

                            TotalAdjAmount = dtDistinct.Compute("SUM(Adjusted)", "1=1")
                            If (TotalAdjAmount Is DBNull.Value Or TotalAdjAmount Is Nothing) Then
                                TotalAdjAmount = 0
                            End If
                            TotalApproved = dtDistinct.Compute("SUM(Approved)", "1=1")
                            If (TotalApproved Is DBNull.Value Or TotalApproved Is Nothing) Then
                                TotalApproved = 0
                            End If


                            TotalPaid = dtDistinct.Compute("SUM(Paid)", "1=1")
                            If (TotalPaid Is DBNull.Value Or TotalPaid Is Nothing) Then
                                TotalPaid = 0
                            End If
                        ElseIf Session("Scope") = 0 Then
                            rpt.ReportPath = "Reports\rptClaimOverviewHistoryClaimsOnly.rdlc"
                            dtDistinct = dtView.ToTable(True, New String() {"ClaimID", "Claimed", "Adjusted", "Approved", "Paid", "ClaimStatus"})

                            TotalClaimed = dtDistinct.Compute("SUM(Claimed)", "1=1")
                            If (TotalClaimed Is DBNull.Value Or TotalClaimed Is Nothing) Then
                                TotalClaimed = 0
                            End If

                            TotalAdjAmount = dtDistinct.Compute("SUM(Adjusted)", "1=1")
                            If (TotalAdjAmount Is DBNull.Value Or TotalAdjAmount Is Nothing) Then
                                TotalAdjAmount = 0
                            End If
                            TotalApproved = dtDistinct.Compute("SUM(Approved)", "1=1")
                            If (TotalApproved Is DBNull.Value Or TotalApproved Is Nothing) Then
                                TotalApproved = 0
                            End If


                            TotalPaid = dtDistinct.Compute("SUM(Paid)", "1=1")
                            If (TotalPaid Is DBNull.Value Or TotalPaid Is Nothing) Then
                                TotalPaid = 0
                            End If
                        Else
                            rpt.ReportPath = "Reports\rptClaimOverviewHistoryRejecteServItem.rdlc"
                            Dim dtView1 As DataView = dt.DefaultView
                            Dim dtDistinct1 As DataTable = dtView1.ToTable(True, New String() {"ServiceID", "ItemID"})
                            ServiceQTY = dtDistinct1.Compute("SUM(ServiceID)", "1=1")

                            ItemQTY = dtDistinct1.Compute("SUM(ItemID)", "1=1")


                            dtDistinct = dtView1.ToTable(True, New String() {"ClaimID", "Claimed", "Adjusted", "Approved", "Paid"})

                            TotalClaimed = dtDistinct.Compute("SUM(Claimed)", "1=1")
                            If (TotalClaimed Is DBNull.Value Or TotalClaimed Is Nothing) Then
                                TotalClaimed = 0
                            End If

                            TotalAdjAmount = dtDistinct.Compute("SUM(Adjusted)", "1=1")
                            If (TotalAdjAmount Is DBNull.Value Or TotalAdjAmount Is Nothing) Then
                                TotalAdjAmount = 0
                            End If
                            TotalApproved = dtDistinct.Compute("SUM(Approved)", "1=1")
                            If (TotalApproved Is DBNull.Value Or TotalApproved Is Nothing) Then
                                TotalApproved = 0
                            End If


                            TotalPaid = dtDistinct.Compute("SUM(Paid)", "1=1")
                            If (TotalPaid Is DBNull.Value Or TotalPaid Is Nothing) Then
                                TotalPaid = 0
                            End If
                        End If
                        Param(27) = New ReportParameter("paramTotalClaims", dtDistinct.Rows.Count)
                        Param(28) = New ReportParameter("paramTotalClaimed", TotalClaimed.ToString())
                        Param(29) = New ReportParameter("paramAdjustedAmount", imisgen.getMessage("L_PAID", False))
                        Param(30) = New ReportParameter("paramTotalAdjustedAmount", TotalAdjAmount.ToString())
                        Param(31) = New ReportParameter("paramTotalApproved", TotalApproved.ToString())
                        Param(32) = New ReportParameter("paramApproved", imisgen.getMessage("L_APPROVED", False))
                        Param(33) = New ReportParameter("paramTotalPaid", TotalPaid.ToString())
                        Param(34) = New ReportParameter("prmService", imisgen.getMessage("L_SERVICE", False))
                        Param(35) = New ReportParameter("prmQty", imisgen.getMessage("L_QTY", False))
                        Param(36) = New ReportParameter("paramServiceQty", ServiceQTY.ToString())
                        Param(37) = New ReportParameter("paramItemQty", ItemQTY.ToString())
                        Param(38) = New ReportParameter("prmAppQty", imisgen.getMessage("L_APPQTY", False))
                        Param(39) = New ReportParameter("prmPrice", imisgen.getMessage("L_PRICE", False))
                        Param(40) = New ReportParameter("prmAppValue", imisgen.getMessage("L_APPVALUE", False))
                        Param(41) = New ReportParameter("prmClaimServices", imisgen.getMessage("L_SERVICES", False))
                        Param(42) = New ReportParameter("prmClaimItems", imisgen.getMessage("L_ITEM", False))
                        Param(43) = New ReportParameter("prmJustification", imisgen.getMessage("L_JUSTIFICATION", False))
                        Param(44) = New ReportParameter("prmValuated", imisgen.getMessage("L_PRICEVALUATED", False))
                        Param(45) = New ReportParameter("Grouping", IMIS_EN.eReports.Grouping)
                        Param(46) = New ReportParameter("prmTotalApproved", imisgen.getMessage("R_TOTALAPPROVED", False))
                        Param(47) = New ReportParameter("prmTotalAdjustedAmount", imisgen.getMessage("R_TOTALADJUSTEDAMOUINT", False))
                        Param(48) = New ReportParameter("prmTotalPaid", imisgen.getMessage("R_TOTALPAIDAMOUNT", False))



                        rpt.SetParameters(Param)
                        ds.Value = dt
                        rpt.DataSources.Add(ds)
                End Select

            Catch ex As Exception
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), divJsScript, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
                Return
            End Try
        End If
    End Sub
End Class
