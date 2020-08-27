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

Public Partial Class PolicyRenewal
    Inherits System.Web.UI.Page

    Private Family As New IMIS_BI.FindFamilyBI

    Private Policy As New IMIS_BI.FindPolicyBI
    Private PolicyRenewal As New IMIS_BI.PolicyRenewalBI
    Private imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then Return
        RunPageSecurity()

        rbPreview.Checked = True
        Dim dtRegions As DataTable = PolicyRenewal.GetRegions(imisgen.getUserId(Session("User")), True)
        ddlRegion.DataSource = dtRegions
        ddlRegion.DataValueField = "RegionId"
        ddlRegion.DataTextField = "RegionName"
        ddlRegion.DataBind()
        If dtRegions.Rows.Count = 1 Then
            FillDistricts()
        End If
        'ddlOfficer.DataSource = Policy.GetOfficers(ddlDistrict.SelectedValue, True)
        'ddlOfficer.DataValueField = "OfficerID"
        'ddlOfficer.DataTextField = "Code"
        'ddlOfficer.DataBind()

        ddlPolicyStatus.DataSource = Policy.GetPolicyStatus(True)
        ddlPolicyStatus.DataTextField = "Status"
        ddlPolicyStatus.DataValueField = "Code"
        ddlPolicyStatus.DataBind()

        ddlSMSStatus.DataSource = PolicyRenewal.GetSMSStatus
        ddlSMSStatus.DataTextField = "Status"
        ddlSMSStatus.DataValueField = "Value"
        ddlSMSStatus.DataBind()

        ddlOn.DataSource = PolicyRenewal.GetJournalOn
        ddlOn.DataTextField = "State"
        ddlOn.DataValueField = "Value"
        ddlOn.DataBind()

        If Not Session("RptFilterPolicy") Is Nothing Then
            Dim RprtDic As Dictionary(Of String, String) = CType(Session("RptFilterPolicy"), Dictionary(Of String, String))
            If Not RprtDic("SMSStatus") = -1 Then
                ddlSMSStatus.SelectedValue = CInt(RprtDic("SMSStatus"))
            End If
            If Not RprtDic("PolicyStatus") = 0 Then
                ddlPolicyStatus.SelectedValue = CInt(RprtDic("PolicyStatus"))
            End If

            If Not RprtDic("RegionId") = "0" AndAlso RprtDic("RegionId") <> "" Then
                ddlRegion.SelectedValue = CInt(RprtDic("RegionId"))
                FillDistricts()


                If Not RprtDic("DistrictID") = "0" AndAlso RprtDic("DistrictID") <> "" Then
                    ddlDistrict.SelectedValue = CInt(RprtDic("DistrictID"))
                    GetWards()
                    If Not RprtDic("WardID") = 0 Then
                        ddlWard.SelectedValue = CInt(RprtDic("WardID"))
                        getVillages()
                        If Not Val(RprtDic("VillageID")) = 0 Then
                            ddlVillage.SelectedValue = CInt(RprtDic("VillageID"))
                        End If
                    End If
                    If Not Val(RprtDic("OfficerID")) = 0 Then
                        ddlOfficer.SelectedValue = CInt(RprtDic("OfficerID"))
                    End If
                End If
            End If
            If Not RprtDic("DateFrom") = "" Then
                txtFromDate.Text = RprtDic("DateFrom")
            End If
            If Not RprtDic("DateTo") = "" Then
                txtToDate.Text = RprtDic("DateTo")
            End If
            If Not RprtDic("JournalOn") = 1 Then
                ddlOn.SelectedValue = CInt(RprtDic("JournalOn"))
            End If
        End If
    End Sub
    Private Sub FillDistricts()
        Dim dtDistricts As DataTable = PolicyRenewal.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataSource = dtDistricts
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
            GetWards()

    End Sub

    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        If Not userBI.RunPageSecurity(IMIS_EN.Enums.Pages.PolicyRenewal, Page) Then
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.PolicyRenewal.ToString & "&retUrl=" & RefUrl)
        End If
        If IMIS_Gen.OfflineCHF Or IMIS_Gen.offlineHF Then
            btnSendSMS.Visible = False
            btnUpdateRenewals.Visible = False
        End If
    End Sub
    Private Sub GetWards()
        Dim dtWards As DataTable = Family.GetWards(Val(ddlDistrict.SelectedValue), True)
        Dim wards As Integer = Val(ddlDistrict.SelectedValue)
        If wards > 0 Then
            ddlWard.DataSource = dtWards
            ddlWard.DataValueField = "WardId"
            ddlWard.DataTextField = "WardName"
            ddlWard.DataBind()
        Else
            ddlWard.Items.Clear()
        End If
        getVillages(wards)
    End Sub

    Private Sub FillOfficers()
        '2/7/2019 - Salum - Clears Officers when the region is not selected.
        If (Val(ddlRegion.SelectedValue) > 0) Then
            ddlOfficer.DataSource = Policy.GetOfficers(If(Val(ddlDistrict.SelectedValue) = 0, Val(ddlRegion.SelectedValue), Val(ddlDistrict.SelectedValue)), True, Val(ddlVillage.SelectedValue))
            ddlOfficer.DataValueField = "OfficerID"
            ddlOfficer.DataTextField = "Code"
            ddlOfficer.DataBind()
        Else
            ddlOfficer.Items.Clear()
        End If
    End Sub
    Private Sub getVillages(Optional ByVal Wards As Integer = 1)
        'If ddlWard.SelectedIndex < 0 Then Exit Sub
        If Wards > 0 And Not Val(ddlWard.SelectedValue) = 0 Then
            ddlVillage.DataSource = Family.GetVillages(Val(ddlWard.SelectedValue), True)
            ddlVillage.DataValueField = "VillageId"
            ddlVillage.DataTextField = "VillageName"
            ddlVillage.DataBind()

            'FillOfficers()
        Else
            'ddlOfficer.Items.Clear()
            ddlVillage.Items.Clear()
        End If
        FillOfficers()
    End Sub
    
    Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click

        Dim pol As New IMIS_BI.PolicyRenewalBI
        Dim dt As New DataTable


        Dim RangeFrom As DateTime
        Dim RangeTo As DateTime
        Dim OfficerID As Integer
        Dim RegionId As Integer
        Dim District As Integer
        Dim Village As Integer
        Dim Ward As Integer

        If Val(ddlOfficer.SelectedValue) > 0 Then OfficerID = ddlOfficer.SelectedValue
        If Val(ddlRegion.SelectedValue) > 0 Then RegionId = ddlRegion.SelectedValue
        If Val(ddlDistrict.SelectedValue) > 0 Then District = ddlDistrict.SelectedValue
        If Val(ddlVillage.SelectedValue) > 0 Then Village = ddlVillage.SelectedValue
        If Val(ddlWard.SelectedValue) > 0 Then Ward = ddlWard.SelectedValue


        If txtFromDate.Text = "" Then txtFromDate.Text = Format(Date.Now, "dd/MM/yyyy")
        If IsDate(Date.ParseExact(txtFromDate.Text, "dd/MM/yyyy", Nothing)) Then
            RangeFrom = Date.ParseExact(txtFromDate.Text, "dd/MM/yyyy", Nothing)
        End If


        If txtToDate.Text = "" Then txtToDate.Text = Format(Date.Now, "dd/MM/yyyy")

        If IsDate(Date.ParseExact(txtToDate.Text, "dd/MM/yyyy", Nothing)) Then
            RangeTo = Date.ParseExact(txtToDate.Text, "dd/MM/yyyy", Nothing)
        End If


        If rbPreview.Checked Then
            Dim PolicyStatus As Integer = ddlPolicyStatus.SelectedValue

            Dim sSubTitle As String = imisgen.getMessage("L_DATEFROM") & " " & txtFromDate.Text & " " & imisgen.getMessage("L_TO") & " " & txtToDate.Text

            If ddlVillage.SelectedValue = "" Then ddlVillage.SelectedValue = 0

            'If ddlDistrict.SelectedValue > 0 Or Val(ddlWard.SelectedValue) > 0 Or ddlOfficer.SelectedValue > 0 Or ddlPolicyStatus.SelectedValue > 0 Or Val(ddlVillage.SelectedValue) > 0 Then
            'sSubTitle = sSubTitle & " Filter("

            If Not ddlRegion.SelectedValue = "0" AndAlso Not ddlRegion.SelectedValue = "" Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += imisgen.getMessage("L_REGION") & ": " & If(ddlRegion.SelectedItem.Text = "", "", ddlRegion.SelectedItem.Text)
            End If
            If Not ddlDistrict.SelectedValue = "0" AndAlso Not ddlDistrict.SelectedValue = "" Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += imisgen.getMessage("L_DISTRICT") & ": " & If(ddlDistrict.SelectedItem.Text = "", "", ddlDistrict.SelectedItem.Text)
            End If
            If Not ddlWard.SelectedValue = "0" And Not ddlWard.SelectedValue = "" Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += imisgen.getMessage("L_WARD") & ": " & ddlWard.SelectedItem.Text
            End If
            If ddlVillage.SelectedIndex > 0 Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += imisgen.getMessage("L_VILLAGE") & ": " & ddlVillage.SelectedItem.Text
            End If
            If Not ddlOfficer.SelectedValue = "0" AndAlso ddlOfficer.SelectedValue <> "" Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                If Val(ddlOfficer.SelectedValue) > 0 Then sSubTitle += imisgen.getMessage("L_OFFICER") & ": " & ddlOfficer.SelectedItem.Text
            End If
            If Not ddlPolicyStatus.SelectedValue = 0 Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += imisgen.getMessage("L_POLICYSTATUS") & ": " & ddlPolicyStatus.SelectedItem.Text
            End If

            'sSubTitle = sSubTitle & ")"
            ' End If

            IMIS_EN.eReports.SubTitle = sSubTitle


            dt = pol.GetPolicyStatus(RangeFrom, RangeTo, OfficerID, RegionId, District, Village, Ward, PolicyStatus)
            If dt.Rows.Count > 0 Then
                StoreReportFilter()
                Session("Report") = dt

                Response.Redirect("Report.aspx?r=ps")
            Else
                imisgen.Alert(imisgen.getMessage("M_NODATAFORREPORT"), pnlButtons, alertPopupTitle:="IMIS")
            End If
        Else

            Dim SMSStatus As Integer = if(ddlSMSStatus.SelectedValue = "-1", 0, ddlSMSStatus.SelectedValue)
            Dim IntervalType As Integer = ddlOn.SelectedValue

            Dim sSubTitle As String = imisgen.getMessage("L_DATEFROM") & " " & txtFromDate.Text & " " & imisgen.getMessage("L_TO") & " " & txtToDate.Text & " " & imisgen.getMessage("L_ON") & " " & ddlOn.SelectedItem.Text

            'If ddlDistrict.SelectedValue > 0 Or Val(ddlWard.SelectedValue) > 0 Or ddlOfficer.SelectedValue > 0 Or Val(ddlVillage.SelectedValue) > 0 Or ddlSMSStatus.SelectedValue > -1 Then
            'sSubTitle = sSubTitle & " Filter("
            If Not Val(ddlDistrict.SelectedValue) = "0" Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += imisgen.getMessage("L_DISTRICT") & ": " & ddlDistrict.SelectedItem.Text
            End If
            If Not Val(ddlWard.SelectedValue) = "0" And Not ddlWard.SelectedValue = "" Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += imisgen.getMessage("L_WARD") & ": " & ddlWard.SelectedItem.Text
            End If
            If ddlVillage.SelectedIndex > 0 Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += imisgen.getMessage("L_VILLAGE") & ": " & ddlVillage.SelectedItem.Text
            End If
            If Not Val(ddlOfficer.SelectedValue) = 0 Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += imisgen.getMessage("L_OFFICER") & ": " & ddlOfficer.SelectedItem.Text
            End If
            If Not ddlSMSStatus.SelectedValue = -1 Then
                If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
                sSubTitle += imisgen.getMessage("L_SMSSTATUS") & ": " & ddlSMSStatus.SelectedItem.Text
            End If
            'sSubTitle = sSubTitle & ")"
            ' End If

            IMIS_EN.eReports.SubTitle = sSubTitle

            dt = pol.GetPolicyPromptJournal(RangeFrom, RangeTo, OfficerID, District, Village, Ward, SMSStatus, IntervalType)
            If dt.Rows.Count > 0 Then
                StoreReportFilter()
                Session("Report") = dt

                Response.Redirect("Report.aspx?r=psj")
            Else
                imisgen.Alert(imisgen.getMessage("M_NODATAFORREPORT"), pnlButtons, alertPopupTitle:="IMIS")
            End If
        End If
    End Sub
    Private Sub StoreReportFilter()
        Dim RptDictionary As New Dictionary(Of String, String)
        RptDictionary.Add("PolicyStatus", ddlPolicyStatus.SelectedValue)
        RptDictionary.Add("SMSStatus", ddlSMSStatus.SelectedValue)
        RptDictionary.Add("RegionId", Val(ddlRegion.SelectedValue))
        RptDictionary.Add("DistrictID", Val(ddlDistrict.SelectedValue))
        RptDictionary.Add("WardID", ddlWard.SelectedValue)
        RptDictionary.Add("VillageID", ddlVillage.SelectedValue)
        RptDictionary.Add("OfficerID", ddlOfficer.SelectedValue)
        RptDictionary.Add("DateFrom", txtFromDate.Text)
        RptDictionary.Add("DateTo", txtToDate.Text)
        RptDictionary.Add("JournalOn", ddlOn.SelectedValue)
        Session("RptFilterPolicy") = RptDictionary
    End Sub

    Protected Sub btnUpdateRenewals_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateRenewals.Click
        Try
            Dim Update As New IMIS_BI.PolicyRenewalBI

            Dim RemindingInterval As Integer?,
                RegionId As Integer?,
                DistrictId As Integer?,
                WardId As Integer?,
                VillageId As Integer?,
                OfficerId As Integer?,
                DateFrom As Date?,
                DateTo As Date?

            RemindingInterval = 0
            If Val(ddlRegion.SelectedValue) > 0 Then RegionId = Val(ddlRegion.SelectedValue)
            If Val(ddlDistrict.SelectedValue) > 0 Then DistrictId = Val(ddlDistrict.SelectedValue)
            If Val(ddlWard.SelectedValue) > 0 Then WardId = Val(ddlWard.SelectedValue)
            If Val(ddlVillage.SelectedValue) > 0 Then VillageId = Val(ddlVillage.SelectedValue)
            If Val(ddlOfficer.SelectedValue) > 0 Then OfficerId = Val(ddlOfficer.SelectedValue)

            If IsDate(Date.ParseExact(txtFromDate.Text, "dd/MM/yyyy", Nothing)) Then
                DateFrom = Date.ParseExact(txtFromDate.Text, "dd/MM/yyyy", Nothing)
            End If

            If IsDate(Date.ParseExact(txtToDate.Text, "dd/MM/yyyy", Nothing)) Then
                DateTo = Date.ParseExact(txtToDate.Text, "dd/MM/yyyy", Nothing)
            End If

            Update.UpdatPolicyRenewal(RemindingInterval, RegionId, DistrictId, WardId, VillageId, OfficerId, DateFrom, DateTo)
            lblMsg.Text = imisgen.getMessage("L_POLICYRENEWALS") & " " & imisgen.getMessage("M_Updated")

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
       
    End Sub



    Private Sub ddlDistricts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged
        GetWards()
        FillOfficers()
    End Sub
    Private Sub gvWards_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWard.SelectedIndexChanged
        getVillages()
    End Sub

    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("home.aspx")
    End Sub
    Private Sub btnSendSMS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSendSMS.Click
        Try


            Dim RangeTo As Date
            Dim RangeFrom As Date
            If txtFromDate.Text = "" Then txtFromDate.Text = Format(Date.Now, "dd/MM/yyyy")

            If IsDate(Date.ParseExact(txtFromDate.Text, "dd/MM/yyyy", Nothing)) Then
                RangeFrom = Date.ParseExact(txtFromDate.Text, "dd/MM/yyyy", Nothing)
            End If


            If txtToDate.Text = "" Then txtToDate.Text = Format(Date.Now, "dd/MM/yyyy")

            If IsDate(Date.ParseExact(txtToDate.Text, "dd/MM/yyyy", Nothing)) Then
                RangeTo = Date.ParseExact(txtToDate.Text, "dd/MM/yyyy", Nothing)
            End If

            Dim str As String = PolicyRenewal.sendSMS(RangeFrom, RangeTo)
            imisgen.Alert(str, pnlBody, , , "IMIS")
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub

    Private Sub ddlVillage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVillage.SelectedIndexChanged
        ddlOfficer.DataSource = Policy.GetOfficers(ddlDistrict.SelectedValue, True, Val(ddlVillage.SelectedValue))
        ddlOfficer.DataValueField = "OfficerID"
        ddlOfficer.DataTextField = "Code"
        ddlOfficer.DataBind()
    End Sub
    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
        FillOfficers()
    End Sub
End Class
