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

Partial Public Class FeedbackPrompt
    Inherits System.Web.UI.Page

    Private Feedback As New IMIS_BI.FeedbackPromptBI
    Private imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Return
        RunPageSecurity()

        Dim dtRegions As DataTable = Feedback.GetRegions(imisgen.getUserId(Session("User")), True)
        ddlRegion.DataSource = dtRegions
        ddlRegion.DataValueField = "RegionId"
        ddlRegion.DataTextField = "RegionName"
        ddlRegion.DataBind()
        If dtRegions.Rows.Count = 1 Then
            FillDistricts()
        End If
        'ddlOfficer.DataSource = Feedback.GetOfficers(ddlDistrict.SelectedValue, True)
        'ddlOfficer.DataValueField = "OfficerID"
        'ddlOfficer.DataTextField = "Code"
        'ddlOfficer.DataBind()

        ddlSMSStatus.DataSource = Feedback.GetSMSStatus
        ddlSMSStatus.DataTextField = "Status"
        ddlSMSStatus.DataValueField = "Value"
        ddlSMSStatus.DataBind()
        If Not Session("RptFilterCriteria") Is Nothing Then
            Dim RprtDic As Dictionary(Of String, String) = CType(Session("RptFilterCriteria"), Dictionary(Of String, String))
            If Not RprtDic("SMSStatus") = -1 Then
                ddlSMSStatus.SelectedValue = CInt(RprtDic("SMSStatus"))
            End If
            If Not Val(RprtDic("DistrictID")) = 0 Then
                ddlDistrict.SelectedValue = CInt(RprtDic("DistrictID"))
                GetWards()
                If Not RprtDic("WardID") = 0 Then
                    ddlWard.SelectedValue = CInt(RprtDic("WardID"))
                    getVillages()
                    If Not RprtDic("VillageID") = 0 Then
                        ddlVillage.SelectedValue = CInt(RprtDic("VillageID"))
                    End If
                End If
                If Not RprtDic("OfficerID").ToString = "" AndAlso Not RprtDic("OfficerID") = 0 Then
                    ddlOfficer.SelectedValue = CInt(RprtDic("OfficerID"))
                End If
            End If
            If Not RprtDic("DateFrom") = "" Then
                txtFromDate.Text = RprtDic("DateFrom")
            End If
            If Not RprtDic("DateTo") = "" Then
                txtToDate.Text = RprtDic("DateTo")
            End If
        End If
    End Sub
    Private Sub FillDistricts()
        Dim dtDistricts As DataTable = Feedback.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataSource = dtDistricts
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
        If dtDistricts.Rows.Count = 1 Then
            GetWards()
        End If
    End Sub

    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        If Not userBI.RunPageSecurity(IMIS_EN.Enums.Pages.FeedbackPrompt, Page) Then
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FeedbackPrompt.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub

    Private Sub GetWards()
        Dim dtWards As DataTable = Feedback.GetWards(Val(ddlDistrict.SelectedValue), True)
        Dim wards As Integer = dtWards.Rows.Count
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
    Private Sub getVillages(Optional ByVal Wards As Integer = 1)
        If ddlWard.SelectedIndex < 0 Then Exit Sub
        If Wards > 0 And Not ddlWard.SelectedValue = 0 Then
            ddlVillage.DataSource = Feedback.GetVillages(ddlWard.SelectedValue, True)
            ddlVillage.DataValueField = "VillageId"
            ddlVillage.DataTextField = "VillageName"
            ddlVillage.DataBind()

            ddlOfficer.DataSource = Feedback.GetOfficers(Val(ddlDistrict.SelectedValue), True, Val(ddlVillage.SelectedValue))
            ddlOfficer.DataValueField = "OfficerID"
            ddlOfficer.DataTextField = "Code"
            ddlOfficer.DataBind()
        Else
            ddlVillage.Items.Clear()
        End If

    End Sub

    Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click

        Dim dt As New DataTable


        Dim RangeFrom As DateTime
        Dim RangeTo As DateTime
        Dim OfficerID As Integer
        Dim District As Integer

        If Val(ddlOfficer.SelectedValue) > 0 Then OfficerID = ddlOfficer.SelectedValue
        If Val(ddlDistrict.SelectedValue) > 0 Then District = ddlDistrict.SelectedValue

        Dim Village As Integer = Val(ddlVillage.SelectedValue)
        Dim Ward As Integer = Val(ddlWard.SelectedValue)
        Dim SMSStatus As Integer = if(ddlSMSStatus.SelectedValue = "-1", 0, ddlSMSStatus.SelectedValue)

        If txtFromDate.Text = "" Then txtFromDate.Text = Format(Date.Now, "dd/MM/yyyy")
        'If ddlOfficer.SelectedValue > 0 Then
        If IsDate(Date.ParseExact(txtFromDate.Text, "dd/MM/yyyy", Nothing)) Then
            RangeFrom = Date.ParseExact(txtFromDate.Text, "dd/MM/yyyy", Nothing)
        End If


        If txtToDate.Text = "" Then txtToDate.Text = Format(Date.Now, "dd/MM/yyyy")

        If IsDate(Date.ParseExact(txtToDate.Text, "dd/MM/yyyy", Nothing)) Then
            RangeTo = Date.ParseExact(txtToDate.Text, "dd/MM/yyyy", Nothing)
        End If





        Dim sSubTitle As String = imisgen.getMessage("L_DATEFROM") & " " & txtFromDate.Text & " " & imisgen.getMessage("L_TO") & " " & txtToDate.Text

        If ddlVillage.SelectedValue = "" Then ddlVillage.SelectedValue = 0

        'If ddlDistrict.SelectedValue > 0 Or Val(ddlWard.SelectedValue) > 0 Or ddlOfficer.SelectedValue > 0 Or Val(ddlVillage.SelectedValue) > 0 Then
        'sSubTitle = sSubTitle & " Filter("]
        If Not ddlRegion.SelectedValue = "0" AndAlso ddlRegion.SelectedValue <> "" Then
            If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
            sSubTitle += imisgen.getMessage("L_REGION") & ": " & ddlRegion.SelectedItem.Text
        End If
        If Not ddlDistrict.SelectedValue = "0" AndAlso ddlDistrict.SelectedValue <> "" Then
            If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
            sSubTitle += imisgen.getMessage("L_DISTRICT") & ": " & ddlDistrict.SelectedItem.Text
        End If
        If Not ddlWard.SelectedValue = "0" And Not ddlWard.SelectedValue = "" Then
            If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
            sSubTitle += imisgen.getMessage("L_WARD") & ": " & ddlWard.SelectedItem.Text
        End If

        If ddlVillage.SelectedIndex > 0 Then
            If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
            sSubTitle += imisgen.getMessage("L_VILLAGE") & ": " & ddlVillage.SelectedItem.Text
        End If
        If Not Val(ddlOfficer.SelectedValue) = "0" Then
            If Not sSubTitle.EndsWith(" ") Then sSubTitle += ", "
            sSubTitle += imisgen.getMessage("L_OFFICER") & ": " & ddlOfficer.SelectedItem.Text
        End If
        'sSubTitle = sSubTitle & ")"
        'End If

        IMIS_EN.eReports.SubTitle = sSubTitle

        dt = Feedback.GetFeedbackPrompt(SMSStatus, District, Ward, Village, OfficerID, RangeFrom, RangeTo)

        If dt.Rows.Count > 0 Then
            StoreReportCriteria()
            Session("Report") = dt

            Response.Redirect("Report.aspx?r=fpj")
        Else
            imisgen.Alert(imisgen.getMessage("M_NODATAFORREPORT"), pnlButtons, alertPopupTitle:="IMIS")
        End If
        'IMIS_EN.eReports.SubTitle = sSubTitle

        'Session("Report") = dt
    End Sub
    Private Sub StoreReportCriteria()
        Dim RptDictionary As New Dictionary(Of String, String)
        RptDictionary.Add("SMSStatus", ddlSMSStatus.SelectedValue)
        RptDictionary.Add("DistrictID", ddlDistrict.SelectedValue)
        RptDictionary.Add("WardID", ddlWard.SelectedValue)
        RptDictionary.Add("VillageID", ddlVillage.SelectedValue)
        RptDictionary.Add("OfficerID", ddlOfficer.SelectedValue)
        RptDictionary.Add("DateFrom", txtFromDate.Text)
        RptDictionary.Add("DateTo", txtToDate.Text)
        Session("RptFilterCriteria") = RptDictionary
    End Sub
    Private Sub ddlDistricts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged
        GetWards()



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

            Dim str As String = Feedback.sendSMS(RangeFrom, RangeTo)
            imisgen.Alert(str, pnlBody, , , "IMIS")
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub

    Private Sub ddlVillage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVillage.SelectedIndexChanged

        ddlOfficer.DataSource = Feedback.GetOfficers(Val(ddlDistrict.SelectedValue), True, Val(ddlVillage.SelectedValue))
        ddlOfficer.DataValueField = "OfficerID"
        ddlOfficer.DataTextField = "Code"
        ddlOfficer.DataBind()
    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
    End Sub
End Class
