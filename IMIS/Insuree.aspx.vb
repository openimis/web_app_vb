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

Imports System.IO

Partial Public Class Insuree
    Inherits System.Web.UI.Page
    Private efamily As New IMIS_EN.tblFamilies
    Private eInsuree As New IMIS_EN.tblInsuree
    Private Insuree As New IMIS_BI.InsureeBI
    Private dtImage As New DataTable
    Private imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private Family As New IMIS_BI.FamilyBI
    Private InsureeUUID As Guid

    Private Sub FormatForm()
        Dim Adjustibility As String = ""
        'Beneficiary card
        Adjustibility = General.getControlSetting("BeneficiaryCard")
        trBeneficiary.Visible = Not (Adjustibility = "N")
        rfBeneficiary.Enabled = (Adjustibility = "M")

        'Confirmation
        Adjustibility = General.getControlSetting("Confirmation")
        L_CONFIRMATIONNO0.Visible = Not (Adjustibility = "N")
        txtConfirmationType.Visible = Not (Adjustibility = "N")

        Adjustibility = General.getControlSetting("ConfirmationNo")
        L_CONFIRMATIONNO.Visible = Not (Adjustibility = "N")
        txtConfirmationNo1.Visible = Not (Adjustibility = "N")

        'CurrentAddress
        Adjustibility = General.getControlSetting("CurrentAddress")
        trCurrentAddress.Visible = Not (Adjustibility = "N")
        rfCurrentAddress.Enabled = (Adjustibility = "M")

        'Current District
        Adjustibility = General.getControlSetting("CurrentDistrict")
        trCurrentDistrict.Visible = Not (Adjustibility = "N")
        trCurrentRegion.visible = Not (Adjustibility = "N")
        rfCurrentDistrict.Enabled = (Adjustibility = "M")
        rfCurrentRegion.Enabled = (Adjustibility = "M")

        'Current Municipality
        Adjustibility = General.getControlSetting("CurrentMunicipality")
        trCurrentMunicipality.Visible = Not (Adjustibility = "N")
        rfCurrentVDC.Enabled = (Adjustibility = "M")

        'Current Village
        Adjustibility = General.getControlSetting("CurrentVillage")
        trCurrentVillage.Visible = Not (Adjustibility = "N")
        rfCurrentVillage.Enabled = (Adjustibility = "M")

        'Education
        Adjustibility = General.getControlSetting("Education")
        rfEducation.Enabled = (Adjustibility = "M")
        trEducation.Visible = Not (Adjustibility = "N")

        'FSP District
        Adjustibility = General.getControlSetting("FSPDistrict")
        trFSPDistrict.Visible = Not (Adjustibility = "N")
        trFSPRegion.Visible = Not (Adjustibility = "N")
        rfDistrictFSP.Enabled = (Adjustibility = "M")
        rfRegionFSP.Enabled = (Adjustibility = "M")

        'FSP Category
        Adjustibility = General.getControlSetting("FSPCategory")
        trFSPCategory.Visible = Not (Adjustibility = "N")
        rfFSPCategory.Enabled = (Adjustibility = "M")

        'FSP
        Adjustibility = General.getControlSetting("FSP")
        trFSP.Visible = Not (Adjustibility = "N")
        rfFSP.Enabled = (Adjustibility = "M")

        'Identification Type
        Adjustibility = General.getControlSetting("IdentificationType")
        trIdentificationType.Visible = Not (Adjustibility = "N")
        rfIdType.Enabled = (Adjustibility = "M")

        'Identification Number
        Adjustibility = General.getControlSetting("IdentificationNumber")
        trIdentificationNo.Visible = Not (Adjustibility = "N")
        rfIdNo.Enabled = (Adjustibility = "M")

        'Email
        Adjustibility = General.getControlSetting("InsureeEmail")
        trEmail.Visible = Not (Adjustibility = "N")
        rfEmail.Enabled = (Adjustibility = "M")

        'Marital Status
        Adjustibility = General.getControlSetting("MaritalStatus")
        trMaritalStatus.Visible = Not (Adjustibility = "N")
        L_MARITAL.Visible = Not (Adjustibility = "N")
        ddlMarital.Visible = Not (Adjustibility = "N")
        rfMaritalStatus.Enabled = (Adjustibility = "M")

        'Permanent Address
        Adjustibility = General.getControlSetting("PermanentAddress")
        L_ADDRESS0.Visible = Not (Adjustibility = "N")
        txtPermanentAddress.Visible = Not (Adjustibility = "N")

        'Poverty
        Adjustibility = General.getControlSetting("Poverty")
        lblPoverty.Visible = Not (Adjustibility = "N")
        txtPoverty.Visible = Not (Adjustibility = "N")

        'Profession
        Adjustibility = General.getControlSetting("Profession")
        rfProfession.Enabled = (Adjustibility = "M")
        trProfession.Visible = Not (Adjustibility = "N")

        'Relationship
        Adjustibility = General.getControlSetting("Relationship")
        trRelation.Visible = Not (Adjustibility = "N")
        'ddlRelation.Visible = Not (Adjustibility = "N")
        rfRelation.Enabled = (Adjustibility = "M")

        'Vulnerability
        Adjustibility = General.getControlSetting("Vulnerability")
        trVulnerability.Visible = Not (Adjustibility = "N")
        rfVulnerability.Enabled = (Adjustibility = "M")



    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If HttpContext.Current.Request.QueryString("i") IsNot Nothing Then
            eInsuree.InsureeUUID = Guid.Parse(HttpContext.Current.Request.QueryString("i"))
            eInsuree.InsureeID = Insuree.GetInsureeIdByUUID(eInsuree.InsureeUUID)
        End If

        If HttpContext.Current.Request.QueryString("f") IsNot Nothing Then
            efamily.FamilyUUID = Guid.Parse(HttpContext.Current.Request.QueryString("f"))
            efamily.FamilyID = Family.GetFamilyIdByUUID(efamily.FamilyUUID)
        End If

        lblMsg.Text = ""
        If IsPostBack = True Then Return

        FormatForm()
        RunPageSecurity()

        Try
            ddlGender.DataSource = Insuree.GetGender
            ddlGender.DataValueField = "Code"
            ddlGender.DataTextField = "Gender"
            ddlGender.DataBind()
            ddlMarital.DataSource = Insuree.GetMaritalStatus
            ddlMarital.DataValueField = "Code"
            ddlMarital.DataTextField = "Status"
            ddlMarital.DataBind()
            ddlCardIssued.DataSource = Insuree.GetYesNO
            ddlCardIssued.DataValueField = "Code"
            ddlCardIssued.DataTextField = "Status"
            ddlCardIssued.DataBind()
            ddlRelation.DataSource = Insuree.GetRelations
            ddlRelation.DataValueField = "RelationId"
            ddlRelation.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "Relation", "AltLanguage")
            ddlRelation.DataBind()
            ddlProfession.DataSource = Insuree.GetProfession
            ddlProfession.DataValueField = "ProfessionId"
            ddlProfession.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "Profession", "AltLanguage")
            ddlProfession.DataBind()
            ddlEducation.DataSource = Insuree.GetEducation
            ddlEducation.DataValueField = "EducationId"
            ddlEducation.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "Education", "AltLanguage")
            ddlEducation.DataBind()
            ddlVulnerability.DataSource = Insuree.GetYesNO
            ddlVulnerability.DataValueField = "Code"
            ddlVulnerability.DataTextField = "Status"
            ddlVulnerability.DataBind()

            ddlIdType.DataSource = Insuree.GetTypeOfIdentity
            ddlIdType.DataValueField = "IdentificationCode"
            ddlIdType.DataTextField = If(Request.Cookies("CultureInfo").Value = "en", "IdentificationTypes", "AltLanguage")
            ddlIdType.DataBind()

            Dim dtFSPRegion As DataTable = Insuree.GetRegionsAll(imisgen.getUserId(Session("User")), True)
            ddlFSPRegion.DataSource = dtFSPRegion
            ddlFSPRegion.DataValueField = "RegionId"
            ddlFSPRegion.DataTextField = "RegionName"
            ddlFSPRegion.DataBind()

            If dtFSPRegion.Rows.Count = 1 Then
                FillFSPDistricts()
            End If

            ddlFSPCateogory.DataSource = Insuree.GetHFLevel
            ddlFSPCateogory.DataValueField = "Code"
            ddlFSPCateogory.DataTextField = "HFLevel"
            ddlFSPCateogory.DataBind()

            FillHF()

            Dim UpdatedFolder As String
            UpdatedFolder = System.Web.Configuration.WebConfigurationManager.AppSettings("UpdatedFolder").ToString()
            'Get Current Region, District, Wards and Villages

            Dim dtCurrentRegion As DataTable = Insuree.GetRegionsAll(imisgen.getUserId(Session("User")), True)
            ddlCurrentRegion.DataSource = dtCurrentRegion
            ddlCurrentRegion.DataValueField = "RegionId"
            ddlCurrentRegion.DataTextField = "RegionName"
            ddlCurrentRegion.DataBind()

            If dtCurrentRegion.Rows.Count = 1 Then
                FillCurrentDistricts()
            End If

            If Not eInsuree.InsureeID = 0 Then
                Insuree.LoadInsuree(eInsuree)
                txtCHFID.Text = eInsuree.CHFID.Trim
                txtLastName.Text = eInsuree.LastName
                txtOtherNames.Text = eInsuree.OtherNames
                txtBirthDate.Text = eInsuree.DOB
                ddlGender.SelectedValue = eInsuree.Gender
                ddlMarital.SelectedValue = eInsuree.Marital
                ddlCardIssued.SelectedValue = If(eInsuree.CardIssued = True, "1", "0")
                txtPassport.Text = eInsuree.passport
                txtPhone.Text = eInsuree.Phone
                Image1.ImageUrl = UpdatedFolder & eInsuree.tblPhotos.PhotoFileName.ToString 'if(eInsuree.tblPhotos.PhotoFileName.ToString <> String.Empty, eInsuree.tblPhotos.PhotoFolder & eInsuree.tblPhotos.PhotoFileName.ToString, "")
                efamily.FamilyID = eInsuree.tblFamilies1.FamilyID
                ddlRelation.SelectedValue = eInsuree.Relationship
                ddlProfession.SelectedValue = eInsuree.Profession
                ddlEducation.SelectedValue = eInsuree.Education
                txtEmail.Text = eInsuree.Email
                If eInsuree.ValidityTo.HasValue Or ((IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF) And Not If(eInsuree.isOffline Is Nothing, False, eInsuree.isOffline)) Then
                    L_FAMILYPANEL.Enabled = False
                    Panel2.Enabled = False
                    pnlImages.Enabled = False
                    B_SAVE.Visible = False
                    btnBrowse.Enabled = False

                End If
                ddlIdType.SelectedValue = eInsuree.TypeOfId

                ddlFSPRegion.SelectedValue = eInsuree.FSPRegion
                FillFSPDistricts()
                ddlFSPDistrict.SelectedValue = eInsuree.FSPDistrict
                ddlFSPCateogory.SelectedValue = eInsuree.FSPCategory
                FillHF()
                ddlFSP.SelectedValue = eInsuree.tblHF.HfID


                ddlCurrentRegion.SelectedValue = eInsuree.CurrentRegion
                FillCurrentDistricts()
                ddlCurrentDistrict.SelectedValue = eInsuree.CurDistrict

                getWards()

                ddlCurentWard.SelectedValue = eInsuree.CurWard
                getVillages()

                ddlCurrentVillage.SelectedValue = eInsuree.CurrentVillage

                ddlVulnerability.SelectedValue = If(eInsuree.Vulnerability = True, "1", "0")

            End If
            hfFamilyId.Value = efamily.FamilyID
            Insuree.GetFamilyHeadInfo(efamily)
            txtRegion.Text = efamily.RegionName
            txtDistrict.Text = efamily.DistrictName
            txtVillage.Text = efamily.VillageName
            txtWard.Text = efamily.WardName
            If efamily.Poverty IsNot Nothing Then txtPoverty.Text = If(efamily.Poverty = True, "Yes", "No")
            txtConfirmationType.Text = efamily.ConfirmationType
            txtHeadCHFID.Text = efamily.tblInsuree.CHFID.Trim
            txtHeadLastName.Text = efamily.tblInsuree.LastName
            txtHeadOtherNames.Text = efamily.tblInsuree.OtherNames
            'txtHeadPhone.Text = efamily.tblInsuree.Phone

            txtCurrentAddress.Text = eInsuree.CurrentAddress


            txtConfirmationNo1.Text = efamily.ConfirmationNo
            txtPermanentAddress.Text = efamily.FamilyAddress

            'Hide the ralationship if it is the head of the family
            L_Relation.Visible = Not eInsuree.IsHead And (Not General.getControlSetting("Relationship") = "N")
            ddlRelation.Visible = Not eInsuree.IsHead And (Not General.getControlSetting("Relationship") = "N")
            rfRelation.Enabled = Not eInsuree.IsHead And (General.getControlSetting("Relationship") = "M")
            ddlRelation.CausesValidation = Not eInsuree.IsHead And (Not General.getControlSetting("Relationship") = "N")


            FillImageDL()

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 1)
            Return
        End Try
    End Sub

    Private Sub FillCurrentDistricts()
        Dim dtCurDistrict As DataTable = Insuree.GetDistrictsAll(imisgen.getUserId(Session("User")), Val(ddlCurrentRegion.SelectedValue), True)
        ddlCurrentDistrict.DataSource = dtCurDistrict
        ddlCurrentDistrict.DataValueField = "DistrictId"
        ddlCurrentDistrict.DataTextField = "DistrictName"
        ddlCurrentDistrict.DataBind()

        If dtCurDistrict.Rows.Count = 1 Then
            getWards()
        End If

    End Sub
    Private Sub FillFSPDistricts()
        ddlFSPDistrict.DataSource = Insuree.GetDistrictsAll(imisgen.getUserId(Session("User")), ddlFSPRegion.SelectedValue, True)
        ddlFSPDistrict.DataValueField = "DistrictId"
        ddlFSPDistrict.DataTextField = "DistrictName"
        ddlFSPDistrict.DataBind()
    End Sub
    Private Sub getWards()
        Dim dtWards As DataTable = Insuree.GetWards(ddlCurrentDistrict.SelectedValue)
        Dim wards As Integer = dtWards.Rows.Count
        If wards > 0 Then
            ddlCurentWard.DataSource = dtWards
            ddlCurentWard.DataValueField = "WardId"
            ddlCurentWard.DataTextField = "WardName"
            ddlCurentWard.DataBind()
        Else
            ddlCurentWard.Items.Clear()
        End If
        getVillages(wards)
    End Sub
    Private Sub getVillages(Optional ByVal Wards As Integer = 1)
        If ddlCurentWard.SelectedIndex < 0 Then Exit Sub
        If Wards > 0 And Not ddlCurentWard.SelectedValue = 0 Then
            ddlCurrentVillage.DataSource = Insuree.GetVillages(ddlCurentWard.SelectedValue)
            ddlCurrentVillage.DataValueField = "VillageId"
            ddlCurrentVillage.DataTextField = "VillageName"
            ddlCurrentVillage.DataBind()
        Else
            ddlCurrentVillage.Items.Clear()
        End If

    End Sub


    Private Sub FillHF()
        'If ddlFSPDistrict.SelectedValue = 0 Or ddlFSPCateogory.SelectedValue = "" Then Exit Sub

        ddlFSP.DataSource = Insuree.GetFSPHF(Val(ddlFSPDistrict.SelectedValue), ddlFSPCateogory.SelectedValue)
        ddlFSP.DataValueField = "HFID"
        ddlFSP.DataTextField = "HFCode"
        ddlFSP.DataBind()

    End Sub
    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Insuree, Page) Then
            B_SAVE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.InsureeEdit, UserID) Or userBI.checkRights(IMIS_EN.Enums.Rights.InsureeAdd, UserID)
            If Not B_SAVE.Visible Then
                pnlImages.Enabled = False
                Panel2.Enabled = False
                L_FAMILYPANEL.Enabled = False
            End If
        Else
            Dim RefUrl = Request.Headers("Referer")
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Insuree.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Private Sub FillImageDL()
        Dim dt As New DataTable
        dt = Insuree.FetchNewImages(Server.MapPath(IMIS_EN.AppConfiguration.SubmittedFolder), eInsuree.CHFID)
        dlImages.DataSource = dt
        dlImages.DataBind()
    End Sub
    Private Sub FetchNewImage()
        If Len(Trim(eInsuree.CHFID)) > 0 Then
            dtImage = Insuree.FetchNewImages(Server.MapPath(IMIS_EN.AppConfiguration.SubmittedFolder), eInsuree.CHFID)
            If dtImage.Rows.Count > 0 Then
                Image1.ImageUrl = IMIS_EN.AppConfiguration.SubmittedFolder & dtImage.Rows(0)("ImagePath")
            Else
                Image1.ImageUrl = ""
            End If

        Else
            Image1.ImageUrl = ""

        End If
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_SAVE.Click
        Try

            If Not Insuree.CheckCHFID(txtCHFID.Text.Trim) = True Then
                imisgen.Alert(txtCHFID.Text.Trim & imisgen.getMessage("M_NOTVALIDCHFNUMBER", True), pnlButtons, alertPopupTitle:="IMIS")
                Return
            End If

            eInsuree.CHFID = txtCHFID.Text.Trim
            eInsuree.LastName = txtLastName.Text.Trim
            eInsuree.OtherNames = txtOtherNames.Text.Trim
            eInsuree.DOB = Date.ParseExact(txtBirthDate.Text.Trim, "dd/MM/yyyy", Nothing)
            eInsuree.Gender = ddlGender.SelectedValue
            If ddlMarital.SelectedValue <> "" Then eInsuree.Marital = ddlMarital.SelectedValue
            If ddlCardIssued.SelectedValue <> "" Then eInsuree.CardIssued = ddlCardIssued.SelectedValue
            eInsuree.passport = txtPassport.Text.Trim
            eInsuree.Phone = txtPhone.Text.Trim
            eInsuree.isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
            If ddlRelation.SelectedValue > 0 Then eInsuree.Relationship = ddlRelation.SelectedValue
            If ddlProfession.SelectedValue > 0 Then eInsuree.Profession = ddlProfession.SelectedValue
            If ddlEducation.SelectedValue > 0 Then eInsuree.Education = ddlEducation.SelectedValue
            eInsuree.Email = txtEmail.Text.Trim
            eInsuree.tblFamilies1 = efamily
            If ddlVulnerability.SelectedValue <> "" Then eInsuree.Vulnerability = ddlVulnerability.SelectedValue

            If ddlIdType.SelectedValue <> "" Then eInsuree.TypeOfId = ddlIdType.SelectedValue

            Dim eHF As New IMIS_EN.tblHF
            If ddlFSP.Items.Count > 0 AndAlso ddlFSP.SelectedValue > 0 Then
                eHF.HfID = ddlFSP.SelectedValue
            End If

            eInsuree.CurrentAddress = txtCurrentAddress.Text.Trim

            If Val(ddlCurrentVillage.SelectedValue) > 0 Then
                eInsuree.CurrentVillage = Val(ddlCurrentVillage.SelectedValue)
            End If


            eInsuree.tblHF = eHF

            Dim ePhotos As New IMIS_EN.tblPhotos
            ePhotos.PhotoID = 0
            ePhotos.InsureeID = eInsuree.InsureeID
            ePhotos.CHFID = eInsuree.CHFID.Trim
            ePhotos.PhotoFolder = IMIS_EN.AppConfiguration.UpdatedFolder
            Dim ImageName As String = Mid(Image1.ImageUrl, Image1.ImageUrl.LastIndexOf("\") + 2, Image1.ImageUrl.Length)
            ePhotos.PhotoFileName = ImageName
            Dim _UpdateImage As Boolean
            _UpdateImage = Image1.ImageUrl.Contains("Submitted")


            Dim Family As New IMIS_BI.FamilyBI
            If ImageName.Length > 0 Then
                ePhotos.OfficerID = Family.getOfficerID(ImageName)
            End If
            Dim PhotoDate As Date = System.Data.SqlTypes.SqlDateTime.Null
            If ImageName.Length > 0 Then
                PhotoDate = Family.ExtractDate(ImageName)
            End If
            If ImageName.Length > 0 Then
                eInsuree.GeoLocation = Family.ExtractLatitude(ImageName) & " " & Family.ExtractLongitude(ImageName)
            End If
            ePhotos.ValidityFrom = PhotoDate
            eInsuree.PhotoDate = PhotoDate
            ePhotos.AuditUserID = imisgen.getUserId(Session("User"))
            eInsuree.AuditUserID = ePhotos.AuditUserID
            eInsuree.tblPhotos = ePhotos

            'If Trim(txtCHFID.Text).Length < 9 Then
            '    imisgen.Alert(imisgen.getMessage("M_CHFNUMBERFEWCHARACTERS"), pnlButtons, alertPopupTitle:="IMIS")
            '    Return
            'End If

            Dim dt As New DataTable
            dt = Insuree.GetMaxMemberCount(eInsuree.tblFamilies1.FamilyID)

            'Display all the policies which already had exceeded the max member count 
            If hfOK.Value = 1 And eInsuree.InsureeID = 0 Then

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

            'Check if any of the policies exceed the max number of threshold
            If hfCheckMaxInsureeCount.Value = 1 And eInsuree.InsureeID = 0 Then
                If dt.Rows.Count > 0 Then
                    'Give the popup with YES and NO 
                    'IF the ans is YES then activate the insuree, make it idle otherwise
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

            Dim Activate As Boolean = If(hfActivate.Value = 0, False, True)

            Dim chk As Integer = Insuree.SaveInsuree(eInsuree, Activate)
            If Not chk = 1 Then
                If Image1.ImageUrl.Length > 0 Then
                    If _UpdateImage Then
                        UpdateImage(ePhotos)
                    End If
                End If
            End If
            If chk = 0 Then
                Session("msg") = imisgen.getMessage("M_INSUREECHF") & " " & eInsuree.CHFID & " " & imisgen.getMessage("M_INSERTED")
            ElseIf chk = 1 Then

                imisgen.Alert(imisgen.getMessage("M_INSUREECHF") & " " & eInsuree.CHFID & " " & imisgen.getMessage("M_EXISTS"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            Else
                Session("msg") = imisgen.getMessage("M_InsureeCHF") & " " & eInsuree.CHFID & " " & imisgen.getMessage("M_Updated")
            End If

        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex, 2)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 2)
            Return
        End Try

        InsureeUUID = Insuree.GetInsureeUUIDByID(eInsuree.InsureeID)

        Response.Redirect("OverviewFamily.aspx?f=" & efamily.FamilyUUID.ToString() & "&i=" & InsureeUUID.ToString())

    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        If B_SAVE.Visible = True Then
            If eInsuree.InsureeID > 0 Then
                InsureeUUID = Insuree.GetInsureeUUIDByID(eInsuree.InsureeID)
                Response.Redirect("OverviewFamily.aspx?f=" & efamily.FamilyUUID.ToString() & "&i=" & InsureeUUID.ToString())
            Else
                Response.Redirect("OverviewFamily.aspx?f=" & efamily.FamilyUUID.ToString())
            End If
        Else
            Response.Redirect("FindInsuree.aspx")
        End If
    End Sub
    Protected Sub dlImages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlImages.SelectedIndexChanged
        Image1.ImageUrl = IMIS_EN.AppConfiguration.SubmittedFolder & (dlImages.SelectedValue)
    End Sub
    Private Sub UpdateImage(ByRef ePhotos As IMIS_EN.tblPhotos)
        Insuree.UpdateImage(ePhotos)
        Image1.ImageUrl = IMIS_EN.AppConfiguration.UpdatedFolder & Mid(Image1.ImageUrl, Image1.ImageUrl.LastIndexOf("\") + 2, Image1.ImageUrl.Length)
    End Sub
    Protected Sub txtCHFID_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCHFID.TextChanged
        eInsuree.CHFID = txtCHFID.Text.Trim
        ' If txtCHFID.Text.Length = 9 Then
        If Insuree.CheckCHFID(txtCHFID.Text.Trim) = True Then
            FetchNewImage()
            FillImageDL()
            Return
        Else

        End If
        '  End If
        imisgen.Alert(eInsuree.CHFID & imisgen.getMessage("M_NOTVALIDCHFNUMBER"), pnlButtons, alertPopupTitle:="IMIS")
    End Sub

    Private Sub ddlFSPDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFSPDistrict.SelectedIndexChanged
        Try
            FillHF()
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub ddlFSPCateogory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFSPCateogory.SelectedIndexChanged
        Try
            FillHF()
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub ddlCurDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCurrentDistrict.SelectedIndexChanged
        Try
            getWards()
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub ddlCurVDC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCurentWard.SelectedIndexChanged
        getVillages()
    End Sub
    Private Sub ddlFSPRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFSPRegion.SelectedIndexChanged
        FillFSPDistricts()
    End Sub

    Private Sub ddlCurrentRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCurrentRegion.SelectedIndexChanged
        FillCurrentDistricts()
    End Sub
End Class
