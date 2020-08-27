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


Partial Public Class ChangeFamily
    Inherits System.Web.UI.Page
    Private eFamily As New IMIS_EN.tblFamilies
    Private ChangeFamily As New IMIS_BI.ChangeFamilyBI
    Private imisgen As New IMIS_Gen
    Private eLocations As New IMIS_EN.tblLocations
    Private eIinsureeNEW As New IMIS_EN.tblInsuree
    Private userBI As New IMIS_BI.UserBI
    Private familyBI As New IMIS_BI.FamilyBI

    Private Sub FormatForm()
        Dim Adjustibility As String = ""


        'Confirmation
        Adjustibility = General.getControlSetting("Confirmation")
        lblConfirmation.Visible = Not (Adjustibility = "N")
        txtConfirmationType.Visible = Not (Adjustibility = "N")
        tfConfirmationType.Visible = Not (Adjustibility = "N")
        rfConfirmationType.Enabled = (Adjustibility = "M")

        Adjustibility = General.getControlSetting("ConfirmationNo")
        L_CONFIRMATIONNO0.Visible = Not (Adjustibility = "N")
        txtConfirmationNo1.Visible = Not (Adjustibility = "N")
        lblConfirmationNo.Visible = Not (Adjustibility = "N")
        txtConfirmationNo.Visible = Not (Adjustibility = "N")
        trConfirmationNo.Visible = Not (Adjustibility = "N")
        rfConfirmationNo.Enabled = (Adjustibility = "M")

        'FamilyType
        Adjustibility = General.getControlSetting("FamilyType")
        trType.Visible = Not (Adjustibility = "N")
        rfType.Enabled = (Adjustibility = "M")

        'Permanent Address
        Adjustibility = General.getControlSetting("PermanentAddress")
        L_ADDRESS0.Visible = Not (Adjustibility = "N")
        txtPermanentAddress.Visible = Not (Adjustibility = "N")
        trAddress.visible = Not (Adjustibility = "N")
        rfAddress.Enabled = (Adjustibility = "M")

        'Poverty
        Adjustibility = General.getControlSetting("Poverty")
        trPoverty.Visible = Not (Adjustibility = "N")
        rfPoverty.Enabled = (Adjustibility = "M")
        lblPoverty3.Visible = Not (Adjustibility = "N")
        txtPoverty.Visible = Not (Adjustibility = "N")




    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblCHFIDToChange.Text = ""
        lblMsg.Text = ""

        If HttpContext.Current.Request.QueryString("f") IsNot Nothing Then
            eFamily.FamilyUUID = Guid.Parse(HttpContext.Current.Request.QueryString("f"))
            eFamily.FamilyID = familyBI.GetFamilyIdByUUID(eFamily.FamilyUUID)
        End If

        If IsPostBack = True Then Return
        FormatForm()
        RunPageSecurity()
        Try
            txtCHFIDToChange.Attributes.Add("NewInsureeID", 0)
            hfFamilyIDValue.Value = eFamily.FamilyID
            ChangeFamily.GetFamilyHeadInfo(eFamily)
            hfInsureeIDValue.Value = eFamily.tblInsuree.InsureeID
        
            Dim dtRegions As DataTable = ChangeFamily.GetRegions(imisgen.getUserId(Session("User")), True)
            ddlRegion.DataSource = dtRegions
            ddlRegion.DataValueField = "RegionId"
            ddlRegion.DataTextField = "RegionName"
            ddlRegion.DataBind()

            ddlType.DataSource = ChangeFamily.GetTypes
            ddlType.DataValueField = "FamilyTypeCode"
            ddlType.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "FamilyType", "AltLanguage")
            ddlType.DataBind()

            ddlEthnicity.DataSource = ChangeFamily.GetEthnicity
            ddlEthnicity.DataValueField = "Code"
            ddlEthnicity.DataTextField = "Ethnicity"
            ddlEthnicity.DataBind()

            If Not eFamily.FamilyID = 0 Then
                txtRegion.Text = eFamily.RegionName
                txtDistrict.Text = eFamily.DistrictName
                txtVillage.Text = eFamily.VillageName
                txtWard.Text = eFamily.WardName

                txtPoverty.Text = If(eFamily.Poverty Is Nothing, "", If(eFamily.Poverty = True, "Yes", "No"))

                txtHeadCHFID.Text = eFamily.tblInsuree.CHFID
                txtHeadLastName.Text = eFamily.tblInsuree.LastName
                txtHeadOtherNames.Text = eFamily.tblInsuree.OtherNames
                ''  txtHeadPhone.Text = eFamily.tblInsuree.Phone

                ddlType.SelectedValue = eFamily.FamilyType
                txtAddress.Text = eFamily.FamilyAddress
                txtPermanentAddress.Text = eFamily.FamilyAddress
                txtConfirmationNo1.Text = eFamily.ConfirmationNo
                txtConfirmationType.Text = eFamily.ConfirmationType
                ddlRegion.SelectedValue = eFamily.RegionId
                ddlDistrict.SelectedValue = eFamily.DistrictID
                If dtRegions.Rows.Count > 0 Then
                    FillDistrict()
                End If
                ddlWard.SelectedValue = eFamily.WardID
                ddlVillage.SelectedValue = eFamily.LocationId

                ddlPoverty.DataSource = ChangeFamily.GetYesNO
                ddlPoverty.DataValueField = "Code"
                ddlPoverty.DataTextField = "Status"
                ddlPoverty.DataBind()
                ddlPoverty.SelectedValue = If(eFamily.Poverty, 1, 0)

                ddlConfirmationType.DataSource = ChangeFamily.GetSubsidy
                ddlConfirmationType.DataValueField = "ConfirmationTypeCode"
                ddlConfirmationType.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "ConfirmationType", "AltLanguage")
                ddlConfirmationType.DataBind()
                ddlConfirmationType.SelectedValue = eFamily.ConfirmationType
                ddlEthnicity.SelectedValue = eFamily.Ethnicity
                txtConfirmationNo.Text = eFamily.ConfirmationNo
                ddlConfirmationType.SelectedValue = eFamily.ConfirmationTypeCode

                hfFamilyIsOffline.Value = If(eFamily.isOffline Is Nothing, False, eFamily.isOffline)
                hfInsureeIsOffline.Value = If(eFamily.tblInsuree.isOffline Is Nothing, False, eFamily.tblInsuree.isOffline)

                If eFamily.ValidityTo.HasValue Or ((IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF) And Not If(eFamily.isOffline Is Nothing, False, eFamily.isOffline)) Then
                    pnlChangeFamily.Enabled = False
                    pnlChangeHeadOfFamily.Enabled = False
                    pnlMoveInsuree.Enabled = False
                    B_Change.Visible = False
                    B_CHECK.Visible = False
                    B_CHECKMOVE.Visible = False
                    B_Move.Visible = False
                    btnSave.Visible = False
                End If

            End If


        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlChangeFamily, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
    Private Sub FillDistrict()
        Dim dtDistricts As DataTable = ChangeFamily.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataSource = dtDistricts
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
        If dtDistricts.Rows.Count > 0 Then
            GetWards()
        End If
    End Sub
    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.ChangeFamily, Page) Then
            btnSave.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.FamilyEdit, UserID)
            pnlChangeFamily.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.FamilyEdit, UserID)
            pnlChangeHeadOfFamily.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.FamilyEdit, UserID)
            pnlMoveInsuree.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.InsureeEdit, UserID)
            L_FAMILYPANEL.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.FamilyEdit, UserID)
        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.ChangeFamily.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub ddlDistricts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrict.SelectedIndexChanged
        GetWards()
    End Sub
    Private Sub gvWards_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWard.SelectedIndexChanged
        getVillages()
    End Sub
    Private Sub GetWards()
        Dim dtWards As DataTable = ChangeFamily.GetWards(ddlDistrict.SelectedValue, True)
        Dim wards As Integer = dtWards.Rows.Count
        If wards > 0 Then
            ddlWard.DataSource = dtWards
            ddlWard.DataValueField = "WardId"
            ddlWard.DataTextField = "WardName"
            ddlWard.DataBind()
            'If Not eFamily.tblWards Is Nothing Then
            If eFamily.WardID > 0 Then ddlWard.SelectedValue = eFamily.WardID
            'End If

        Else
            ddlWard.Items.Clear()
        End If
        getVillages(wards)
    End Sub
    Private Sub getVillages(Optional ByVal Wards As Integer = 1)

        If ddlWard.SelectedIndex < 0 Then
            ddlVillage.Items.Clear()
            Exit Sub
        End If

        If Wards > 0 Then
            ddlVillage.DataSource = ChangeFamily.GetVillages(ddlWard.SelectedValue, True)
            ddlVillage.DataValueField = "VillageId"
            ddlVillage.DataTextField = "VillageName"
            ddlVillage.DataBind()
        Else
            ddlVillage.Items.Clear()
        End If

    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        If Not userBI.RunPageSecurity(IMIS_EN.Enums.Pages.OverviewFamily, Page) Then
            Response.Redirect("FindFamily.aspx")
        ElseIf btnSave.Visible Then
            Response.Redirect("OverviewFamily.aspx?f=" & eFamily.FamilyUUID.ToString())
        Else
            Response.Redirect("FindFamily.aspx")
        End If

    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Try
            eFamily.FamilyID = hfFamilyIDValue.Value
            ' eFamily.DistrictID = ddlDistrict.SelectedValue
            'eFamily.WardID = ddlWard.SelectedValue
            eFamily.LocationId = ddlVillage.SelectedValue
            If ddlPoverty.SelectedValue.Length > 0 Then eFamily.Poverty = ddlPoverty.SelectedValue
            If ddlConfirmationType.SelectedValue.Length > 0 Then eFamily.ConfirmationType = ddlConfirmationType.SelectedValue
            eFamily.isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
            eFamily.FamilyType = ddlType.SelectedValue
            'If ddlType.SelectedValue.Length > 0 Then eFamily.FamilyType = ddlType.SelectedValue
            eFamily.FamilyAddress = txtAddress.Text
            If ddlEthnicity.SelectedValue.Length > 0 Then eFamily.Ethnicity = ddlEthnicity.SelectedValue
            eFamily.ConfirmationNo = txtConfirmationNo.Text

            Dim chk As Boolean = ChangeFamily.UpdateChangeFamily(eFamily)
            If chk = True Then
                Session("Msg") = imisgen.getMessage("L_POLICYHOLDER") & " " & txtHeadLastName.Text & imisgen.getMessage("M_Updated")
            End If
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlChangeFamily, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
        Response.Redirect("OverviewFamily.aspx?f=" & eFamily.FamilyUUID.ToString())
    End Sub
    Private Sub txtCHFIDToChange_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CHECK.Click

        B_Change.Enabled = False
        Try
            'HVH CHANGE
            'If txtCHFIDToChange.Text.Length < 9 Then
            '    imisgen.Alert(imisgen.getMessage("M_INVALIDCHFNUMBER"), pnlChangeFamily, alertPopupTitle:="IMIS")
            '    Return
            'End If
            'HVH CHANGE
            eIinsureeNEW.CHFID = txtCHFIDToChange.Text
            ChangeFamily.GetInsureesByCHFID(eIinsureeNEW)
            If eIinsureeNEW.CHFID = String.Empty Then
                imisgen.Alert(imisgen.getMessage("M_CHFNUMBERNOTEXISTS", True), pnlChangeFamily, alertPopupTitle:="IMIS")
                Return
            ElseIf eIinsureeNEW.IsHead = True Then
                imisgen.Alert(imisgen.getMessage("M_CANNOTCHANGEFAMILYHEAD", True), pnlChangeFamily, alertPopupTitle:="IMIS")
                lblCHFIDToChange.Text = eIinsureeNEW.LastName & " " & eIinsureeNEW.OtherNames
                Return
            End If
            txtCHFIDToChange.Attributes.Clear()
            txtCHFIDToChange.Attributes.Item("NewInsureeID") = eIinsureeNEW.InsureeID
            lblCHFIDToChange.Text = eIinsureeNEW.LastName & " " & eIinsureeNEW.OtherNames
            B_Change.Enabled = True
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlChangeFamily, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

    End Sub
    Private Sub B_Change_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_Change.Click
        Try

            Dim eInsureeold As New IMIS_EN.tblInsuree
            eInsureeold.InsureeID = hfInsureeIDValue.Value
            eInsureeold.tblFamilies1 = eFamily
            eInsureeold.isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
            eIinsureeNEW.InsureeID = txtCHFIDToChange.Attributes("NewInsureeID")
            eIinsureeNEW.AuditUserID = imisgen.getUserId(Session("User"))
            eIinsureeNEW.isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
            Dim CHFNumber As String = eIinsureeNEW.CHFID
            If ChangeFamily.ChangeHead(eInsureeold, eIinsureeNEW) = True Then
                Session("Msg") = CHFNumber & imisgen.getMessage("M_CHANGEHEAD")
            End If
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlChangeFamily, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
        Response.Redirect("OverviewFamily.aspx?f=" & eFamily.FamilyUUID.ToString())

    End Sub

    Private Sub B_Move_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_Move.Click
        Try

            'HIREN: Old code is changed
            'If hfCheckMaxInsureeCount.Value = 1 Then
            '    Dim dt As New DataTable
            '    dt = ChangeFamily.GetMaxMemberCount(eFamily.FamilyID)
            '    If dt.Rows.Count > 0 Then
            '        'Give the popup with YES and NO 
            '        'IF the ans is YES then continue with saving else Return to the Overview page
            '        Dim Msg As String = imisgen.getMessage("M_MAXMEMBERCOUNTREACHED") & "<br/>"
            '        For i As Integer = 0 To dt.Rows.Count - 1
            '            If i = dt.Rows.Count - 1 Then
            '                If dt.Rows.Count = 1 Then
            '                    Msg += dt.Rows(i)("EnrollDate")
            '                Else
            '                    Msg += " and " & dt.Rows(i)("EnrollDate")
            '                End If
            '            Else
            '                Msg += dt.Rows(i)("EnrollDate") & ", "
            '            End If
            '        Next
            '        Msg += "<br/>" & imisgen.getMessage("M_CONTINUEPROMPT")
            '        imisgen.Confirm(Msg, pnlButtons, "promptInsureeAdd", "", AcceptButtonText:=imisgen.getMessage("L_YES"), RejectButtonText:=imisgen.getMessage("L_NO"))
            '        Exit Sub
            '    End If
            'End If

            Dim dt As New DataTable
            dt = ChangeFamily.GetMaxMemberCount(eFamily.FamilyID)

            'Display all the policies which already had exceeded the max member count 
            If hfOK.Value = 1 Then
                If dt.Rows.Count > 0 Then
                    If dt.Select("MemberCount <= TotalInsurees").Count > 0 Then
                        If dt.Rows(0)("MemberCount") <> 0 Then
                            Dim Msg As String = imisgen.getMessage("M_MAXMEMBERCOUNTREACHED", True) & "<br/>"
                            For i As Integer = 0 To dt.Rows.Count - 1
                                If dt.Rows(i)("MemberCount") <= dt.Rows(i)("TotalInsurees") Then
                                    If i = dt.Rows.Count - 1 Then
                                        If dt.Rows.Count = 1 Then
                                            Msg += dt.Rows(i)("EnrollDate")
                                        Else
                                            Msg += " and " & dt.Rows(i)("EnrollDate")
                                        End If
                                    Else
                                        Msg += dt.Rows(i)("EnrollDate") & ", "
                                    End If
                                End If
                            Next
                            'imisgen.Confirm(Msg, pnlButtons, "msgOkay", "", AcceptButtonText:=imisgen.getMessage("L_OK"))
                            imisgen.Alert(Msg, pnlButtons, "msgOkay")
                            Exit Sub
                        End If
                    End If
                End If
            End If

            'Check if any of the policies exceed the max number of adherents
            If hfCheckMaxInsureeCount.Value = 1 Then
                If dt.Rows.Count > 0 Then
                    'Give the popup with YES and NO 
                    'IF the ans is YES then continue with saving else Return to the Overview page
                    If dt.Select("MemberCount > TotalInsurees").Count > 0 Then
                        If dt.Rows(0)("MemberCount") <> 0 Or dt.Rows(0)("Threshold") <> 0 Then
                            Dim Msg As String = imisgen.getMessage("M_MAXTHRESHOLDCOUNTREACHED", True) & "<br/>"
                            For i As Integer = 0 To dt.Rows.Count - 1
                                If dt.Rows(i)("Threshold") <= dt.Rows(i)("TotalInsurees") And dt.Rows(i)("MemberCount") >= dt.Rows(i)("TotalInsurees") Then
                                    If i = dt.Rows.Count - 1 Then
                                        If dt.Rows.Count = 1 Then
                                            Msg += dt.Rows(i)("EnrollDate")
                                        Else
                                            Msg += " and " & dt.Rows(i)("EnrollDate")
                                        End If
                                    Else
                                        Msg += dt.Rows(i)("EnrollDate") & ", "
                                    End If
                                End If
                            Next
                            'Msg += "<br/>" & imisgen.getMessage("M_CONTINUEPROMPT")
                            imisgen.Confirm(Msg, pnlButtons, "promptInsureeAdd", "", AcceptButtonText:=imisgen.getMessage("L_YES", True), RejectButtonText:=imisgen.getMessage("L_NO", True))
                            Exit Sub
                        End If
                    End If
                End If
            End If

            hfCheckMaxInsureeCount.Value = 0
            hfOK.Value = 0

            eIinsureeNEW.tblFamilies1 = eFamily
            eIinsureeNEW.InsureeID = txtCHFIDToMove.Attributes("NewInsureeID")
            eIinsureeNEW.AuditUserID = imisgen.getUserId(Session("User"))
            eIinsureeNEW.isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
            Dim CHFNumber As String = eIinsureeNEW.CHFID

            Dim Activate As Boolean = If(hfActivate.Value = 0, False, True)

            If ChangeFamily.MoveInsuree(eIinsureeNEW, Activate) = True Then
                Session("Msg") = CHFNumber & imisgen.getMessage("M_MOVEINSUREE", True) & " " & txtHeadLastName.Text
            End If
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE", True), pnlChangeFamily, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try

        Dim FamilyUUID As Guid
        If HttpContext.Current.Request.QueryString("f") IsNot Nothing Then
            FamilyUUID = familyBI.GetFamilyUUIDByID(hfFamilyIDValue.Value)
        End If

        Response.Redirect("OverviewFamily.aspx?f=" & FamilyUUID.ToString())

    End Sub
    Private Sub B_CHECKMOVE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CHECKMOVE.Click
        B_Move.Enabled = False
        Try
            'HVH CHANGED 
            'If txtCHFIDToMove.Text.Length < 9 Then
            '    imisgen.Alert(imisgen.getMessage("M_SELECTEDCHFNONOTEXIT"), pnlChangeFamily, alertPopupTitle:="IMIS")
            '    Return
            'End If
            'HVH CHANGED 
            eIinsureeNEW.CHFID = txtCHFIDToMove.Text
            eIinsureeNEW.isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
            ChangeFamily.GetInsureesByCHFID(eIinsureeNEW)
            If eIinsureeNEW.CHFID = String.Empty Then
                imisgen.Alert(imisgen.getMessage("M_SELECTEDCHFNONOTEXIT", True), pnlChangeFamily, alertPopupTitle:="IMIS")
                Return
            ElseIf eIinsureeNEW.IsHead = True Then
                imisgen.Alert(imisgen.getMessage("M_CANNOTCHANGECHFNOISHEADFMLY", True), pnlChangeFamily, alertPopupTitle:="IMIS")
                Return
            ElseIf eIinsureeNEW.tblFamilies1.FamilyID = hfFamilyIDValue.Value Then
                imisgen.Alert(imisgen.getMessage("M_INSUREEALREADYINFAMILY", True), pnlChangeFamily, alertPopupTitle:="IMIS")
                Return
            End If
            txtCHFIDToMove.Attributes.Clear()
            txtCHFIDToMove.Attributes.Item("NewInsureeID") = eIinsureeNEW.InsureeID
            lblCHFIDToMove.Text = eIinsureeNEW.LastName & " " & eIinsureeNEW.OtherNames
            B_Move.Enabled = True
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlChangeFamily, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub
    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistrict()
    End Sub
End Class
