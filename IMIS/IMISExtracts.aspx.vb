Imports System.IO

Partial Public Class IMISExtracts
    Inherits System.Web.UI.Page
    Private Extracts As New IMIS_BI.IMISExtractsBI
    Protected imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Protected IMIS_Gen As New IMIS_Gen
    'AMANI 27/09
    Private eExtractInfo As New IMIS_EN.eExtractInfo

    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If UserID = 8 And (IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF) Then
            pnlOfflineClaims.Visible = False
            Exit Sub
        End If
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.IMISExtracts, Page) Then
            MasterData.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ExtractMasterDataDownload, UserID)

            pnlCreatePhoneExtracts.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ExtractPhoneExtractsCreate, UserID)


            pnlCreateOfflineExtracts.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ExtractOfflineExtractCreate, UserID)

            pnlUploadClaims.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ExtractClaimUpload, UserID) And Not IMIS_Gen.offlineHF
            pnlOnlineClaims.Visible = pnlUploadClaims.Visible

            pnlExtractEntrolment.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ExtractEnrolmentsUpload, UserID) And IMIS_Gen.OfflineCHF

            pnlUploadEnrolments.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ExtractEnrolmentsUpload, UserID) And Not IMIS_Gen.OfflineCHF
            'pnlUploadEnrolmentXML.Visible = pnlExtractEntrolment.Visible



            'pnlOfflineClaims.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.OfflineClaims, UserID) And IMIS_Gen.offlineHF
            'pnlOfflineClaims.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.OfflineClaims, UserID) And IMIS_Gen.offlineHF

            pnlFeedbackUpload.Enabled = userBI.checkRights(IMIS_EN.Enums.Rights.ExtractFeedbackUpload, UserID)
            pnlFeedbackUpload.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.ExtractFeedbackUpload, UserID)
            PnlFeedBackUploadHeader.Visible = pnlFeedbackUpload.Visible

        Else
            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.IMISExtracts.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()

        Dim eDefaults As New IMIS_EN.tblIMISDefaults
        Dim eExtract As New IMIS_EN.tblExtracts
        Dim ExtractsBI As New IMIS_BI.IMISExtractsBI

        If Request.Form("__EVENTTARGET") = btnUploadExtract.ClientID Then
            btnUploadExtract_Click(Me, New System.EventArgs)
        End If
        If Request.Form("__EVENTTARGET") = btnOffLineExtract.ClientID Then
            btnOffLineExtract_Click(Me, New System.EventArgs)
        End If

        If IsPostBack Then Return

        Try
            Extracts.GetDefaults(eDefaults)
            If eDefaults.OffLineHF <> 0 Or eDefaults.OfflineCHF <> 0 Then
                'we are Offline
                pnlOffline.Visible = True
                pnlOnline.Visible = False
                'get the latest upload sequence in case we are off-line 
                eExtract = ExtractsBI.GetLastCreateExtractInfo(0, 0, 1)
                If eExtract.ExtractSequence = Nothing Then
                    lblmsg.Text = imisgen.getMessage("M_EXTR_NOEXTRACTSFOUND")
                Else
                    If eExtract.ExtractSequence = 0 Then
                        lblmsg.Text = imisgen.getMessage("M_EXTR_NOEXTRACTSFOUND")
                    Else
                        lblmsg.Text = imisgen.getMessage("M_EXTR_EXTRACTSEQUENCE") & eExtract.ExtractSequence.ToString
                        hfExtractFound.Value = 1
                    End If
                End If
                'pnlExtractEntrolment.Visible = True
                'pnlDownloadEntrolment.Visible = True
            Else
                Dim dtRegions As DataTable = Extracts.GetRegions(imisgen.getUserId(Session("User")), True, True)
                pnlOffline.Visible = False
                pnlOnline.Visible = True
                ddlRegionPhone.DataSource = dtRegions
                ddlRegionPhone.DataValueField = "RegionId"
                ddlRegionPhone.DataTextField = "RegionName"
                ddlRegionPhone.DataBind()

                ddlRegionOffLine.DataSource = dtRegions
                ddlRegionOffLine.DataValueField = "RegionId"
                ddlRegionOffLine.DataTextField = "RegionName"
                ddlRegionOffLine.DataBind()


                'AMANI 02/10/1017
                'Dim LocationId As Integer = If(Val(ddlDistrictsOffLine.SelectedValue) = 0, Val(ddlRegionOffLine.SelectedValue), Val(ddlDistrictsOffLine.SelectedValue))
                'ddlExtracts.DataSource = Extracts.GetExtractList(LocationId, 0, 4)
                'ddlExtracts.DataValueField = "ExtractId"
                'ddlExtracts.DataTextField = "ExtractInfo"
                'ddlExtracts.DataBind()
                ''END

                If dtRegions.Rows.Count = 1 Then
                    FillDistrictsOffLine()
                    FillDistrictsPhone()
                End If
                'If ddlDistrictsOffLine.SelectedValue <> 0 Then
                'populate the extracts 

                'End If

            End If

            If Request.QueryString("dl") = "1" Then
                DownloadEnrolments()
            End If

        Catch ex As Exception
            Session("msg") = ex.Message
        End Try

    End Sub
    Private Sub FillDistrictsPhone()
        Dim dtDistricts As DataTable = Extracts.GetDistricts(imisgen.getUserId(Session("User")), True, Val(ddlRegionPhone.SelectedValue))
        pnlOffline.Visible = False
        pnlOnline.Visible = True
        ddlDistrictsPhone.DataSource = dtDistricts
        ddlDistrictsPhone.DataValueField = "DistrictID"
        ddlDistrictsPhone.DataTextField = "DistrictName"
        ddlDistrictsPhone.DataBind()

    End Sub
    Private Sub FillDistrictsOffLine()
        Dim dtDistricts As DataTable = Extracts.GetDistricts(imisgen.getUserId(Session("User")), True, If(Val(ddlRegionOffLine.SelectedValue) = -1, 0, Val(ddlRegionOffLine.SelectedValue)))
        pnlOffline.Visible = False
        pnlOnline.Visible = True
        ddlDistrictsOffLine.DataSource = dtDistricts
        ddlDistrictsOffLine.DataValueField = "DistrictID"
        ddlDistrictsOffLine.DataTextField = "DistrictName"
        ddlDistrictsOffLine.DataBind()

        ' If dtDistricts.Rows.Count Then
        Dim LocationId As Integer = If(Val(ddlDistrictsOffLine.SelectedValue) = 0, If(Val(ddlRegionOffLine.SelectedValue) = -1, 0, Val(ddlRegionOffLine.SelectedValue)), Val(ddlDistrictsOffLine.SelectedValue))
        Dim dtExtracts As DataTable = Extracts.GetExtractList(LocationId, 0, 4)
        ddlExtracts.DataSource = dtExtracts
        ddlExtracts.DataValueField = ("ExtractID")
        ddlExtracts.DataTextField = ("ExtractInfo")
        ddlExtracts.DataBind()
        ' End If
    End Sub
    Private Sub ddlDistrictsOffLine_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDistrictsOffLine.SelectedIndexChanged
        Dim LocationId As Integer = If(Val(ddlDistrictsOffLine.SelectedValue) = 0, If(Val(ddlRegionOffLine.SelectedValue) = -1, 0, Val(ddlRegionOffLine.SelectedValue)), Val(ddlDistrictsOffLine.SelectedValue))
        ddlExtracts.DataSource = Extracts.GetExtractList(LocationId, 0, 4)
        ddlExtracts.DataValueField = "ExtractId"
        ddlExtracts.DataTextField = "ExtractInfo"
        ddlExtracts.DataBind()
    End Sub
    Private Sub btnPhoneExtract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPhoneExtract.Click
        If chkInBackground.Checked = False Then
            CreatePhoneExtract()
        Else
            CreatePhoneExtractInBackground()
        End If
    End Sub

    Private Sub CreatePhoneExtract()

        Try

            Dim sp As New Stopwatch
            sp.Start()
            Dim str As String
            Dim eExtractInfo As New IMIS_EN.eExtractInfo
            If Len(ddlDistrictsPhone.SelectedValue) = 0 Then
            End If

            eExtractInfo.LocationId = Val(ddlDistrictsPhone.SelectedValue)
            eExtractInfo.AuditUserID = imisgen.getUserId(Session("User"))
            eExtractInfo.ExtractType = 1

            Extracts.CreatePhoneExtracts(eExtractInfo, chkWithInsuree.Checked)
            sp.Stop()
            Dim ts As TimeSpan = sp.Elapsed
            Dim TimeElapsed As String = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds)

            If eExtractInfo.ExtractStatus = 0 Then
                str = imisgen.getMessage("M_EXTR_PHONEOK") & "<br />" & imisgen.getMessage("M_EXTR_TASKCOMPLETED") & TimeElapsed & imisgen.getMessage("M_EXTR_HOURS")
                'DivMsg.InnerHtml = str
                imisgen.Alert(str, pnlButtons, alertPopupTitle:=imisgen.getMessage("L_ALERTPOPUPTITLE"))
                'PhoneExtractLink.NavigateUrl = "~/Extracts/Phone/ImisData.db3" 'eExtractInfo.ExtractFileName
                PhoneExtractLink.Visible = True
            Else
                str = imisgen.getMessage("M_EXTR_PHONENOK") & "<br />" & imisgen.getMessage("M_EXTR_TASKCOMPLETED") & TimeElapsed & imisgen.getMessage("M_EXTR_HOURS")
                imisgen.Alert(str, pnlButtons, alertPopupTitle:=imisgen.getMessage("L_ALERTPOPUPTITLE"))
                'imisgen.Alert(str, pnlMiddle)
            End If
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:=imisgen.getMessage("L_ALERTPOPUPTITLE"))
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.ToString(), EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub CreatePhoneExtractInBackground()


        Dim Service As New IMISService.Service1
        Service.AllowAutoRedirect = True
        Service.CreatePhoneExtracts(Val(ddlDistrictsPhone.SelectedValue), DirectCast(Session("User"), DataTable)(0)("UserId"), chkWithInsuree.Checked)


        Dim Str As String = IMIS_Gen.getMessage("M_BGEXTRACTCOMPLETED")
        imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")


    End Sub


    Private Function ExtractStatsMsgExport(ByVal eExtractInfo As IMIS_EN.eExtractInfo) As String
        Dim str As String

        str = "<br/>" & "<br/>"

        '1
        str = str & imisgen.getMessage("L_REGION") & ": " & eExtractInfo.RegionCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_DISTRICT") & ": " & eExtractInfo.DistrictsCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_WARD") & ": " & eExtractInfo.WardsCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_VILLAGE") & ": " & eExtractInfo.VillagesCS.ToString & "<br/>"
        '2
        str = str & imisgen.getMessage("L_ITEMS") & ": " & eExtractInfo.ItemsCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_SERVICES") & ": " & eExtractInfo.ServicesCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRICELISTS") & " " & imisgen.getMessage("L_ITEMS") & ": " & eExtractInfo.PLItemsCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRICELISTS") & " " & imisgen.getMessage("L_SERVICES") & ": " & eExtractInfo.PLServicesCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRICELISTS") & " " & imisgen.getMessage("L_ITEMS") & " " & imisgen.getMessage("L_DETAILS") & ": " & eExtractInfo.PLItemsDetailsCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRICELISTS") & " " & imisgen.getMessage("L_SERVICES") & " " & imisgen.getMessage("L_DETAILS") & ": " & eExtractInfo.PLServicesDetailsCS.ToString & "<br/>"
        '3
        str = str & imisgen.getMessage("L_ICD") & ": " & eExtractInfo.ICDCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_HFACILITIES") & ": " & eExtractInfo.HFCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PAYER") & ": " & eExtractInfo.PayerCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_OFFICERS") & ": " & eExtractInfo.OfficerCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRODUCTS") & ": " & eExtractInfo.ProductCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRODUCT") & imisgen.getMessage("L_ITEMS") & ": " & eExtractInfo.ProductItemsCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRODUCT") & imisgen.getMessage("L_SERVICES") & ": " & eExtractInfo.ProductServicesCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRODUCT") & imisgen.getMessage("L_DISTRIBUTION") & ": " & eExtractInfo.RelDistrCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_CLAIMADMIN") & ": " & eExtractInfo.ClaimAdminCS.ToString & "<br/>"

        '4
        str = str & imisgen.getMessage("L_FAMILY") & ": " & eExtractInfo.FamiliesCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_INSUREES") & ": " & eExtractInfo.InsureeCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PHOTOS") & ": " & eExtractInfo.PhotoCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_POLICIES") & ": " & eExtractInfo.PolicyCS.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PREMIUMS") & ": " & eExtractInfo.PremiumCS.ToString & "<br/>"

        str = str & imisgen.getMessage("L_EXTR_ZIPPEDPHOTOS") & ": " & eExtractInfo.ZippedPhotosCS.ToString & "<br/>"

        ExtractStatsMsgExport = str

    End Function
    Private Function ExtractStatsMsgImport(ByVal eExtractInfo As IMIS_EN.eExtractInfo) As String
        Dim str As String

        str = "<br/>" & "<br/>"

        '1
        str = str & imisgen.getMessage("L_LOCATIONS") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.LocationsIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.LocationsUp.ToString & "<br/>"
        'str = str & imisgen.getMessage("L_WARD") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.WardsIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.WardsUpd.ToString & "<br/>"
        'str = str & imisgen.getMessage("L_VILLAGE") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.VillagesIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.VillagesUpd.ToString & "<br/>"
        '2
        str = str & imisgen.getMessage("L_ITEMS") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.ItemsIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.ItemsUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_SERVICES") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.ServicesIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.ServicesUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRICELISTS") & " " & imisgen.getMessage("L_ITEMS") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.PLItemsIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.PLItemsUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRICELISTS") & " " & imisgen.getMessage("L_SERVICES") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.PLServicesIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.PLServicesUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRICELISTS") & " " & imisgen.getMessage("L_ITEMS") & " " & imisgen.getMessage("L_DETAILS") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.PLItemsDetailsIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.PLItemsDetailsUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRICELISTS") & " " & imisgen.getMessage("L_SERVICES") & " " & imisgen.getMessage("L_DETAILS") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.PLServicesDetailsIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.PLServicesDetailsUpd.ToString & "<br/>"
        '3
        str = str & imisgen.getMessage("L_ICD") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.ICDIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.ICDUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_HFACILITIES") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.HFIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.HFUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PAYER") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.PayerIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.PayerUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_OFFICERS") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.OfficerIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.OfficerUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRODUCTS") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.ProductIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.ProductUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRODUCT") & imisgen.getMessage("L_ITEMS") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.ProductItemsIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.ProductItemsUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRODUCT") & imisgen.getMessage("L_SERVICES") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.ProductServicesIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.ProductServicesUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PRODUCT") & imisgen.getMessage("L_DISTRIBUTION") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.RelDistrIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.RelDistrUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_CLAIMADMIN") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.ClaimAdminIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.ClaimAdminUpd.ToString & "<br/>"
        '4
        str = str & imisgen.getMessage("L_FAMILY") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.FamiliesIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.FamiliesUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_INSUREES") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.InsureeIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.InsureeUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PHOTOS") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.PhotoIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.PhotoUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_POLICIES") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.PolicyIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.PolicyUpd.ToString & "<br/>"
        str = str & imisgen.getMessage("L_PREMIUMS") & "  " & imisgen.getMessage("L_INSERTED") & ": " & eExtractInfo.PremiumIns.ToString & "  " & imisgen.getMessage("L_UPDATED") & ": " & eExtractInfo.PremiumUpd.ToString & "<br/>"

        ExtractStatsMsgImport = str


    End Function
    Private Sub btnOffLineExtract_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOffLineExtract.Click
        'Changed by AMANI 28/09
        Dim eExtractInfo As New IMIS_EN.eExtractInfo

        ' If Val(ddlDistrictsOffLine.SelectedValue) > 0 Then
        eExtractInfo.RegionId = If(Val(ddlRegionOffLine.SelectedValue) = -1, 0, Val(ddlRegionOffLine.SelectedValue))
        eExtractInfo.DistrictId = Val(ddlDistrictsOffLine.SelectedValue)
        eExtractInfo.AuditUserID = imisgen.getUserId(Session("User"))

        'Else

        ' End If

        'Added by Amani 22/09/2017
        If chkWithInsureeExport.Checked = True Then
            eExtractInfo.WithInsuree = 1
        Else
            eExtractInfo.WithInsuree = 0
        End If

        If chkInBackgroundExport.Checked = True Then
            Dim Service As New IMISService.Service1
            Service.AllowAutoRedirect = True
            Service.CreateOfflineExtract(eExtractInfo.RegionId, eExtractInfo.DistrictId, eExtractInfo.AuditUserID, chkWithInsureeExport.Checked, chkFULL.Checked)


            Dim StrBg As String = IMIS_Gen.getMessage("M_BGEXTRACTCOMPLETED")
            imisgen.Alert(StrBg, pnlButtons, alertPopupTitle:="IMIS")
        Else
            OfflineExtractInBrowser(eExtractInfo.RegionId, eExtractInfo.DistrictId, chkWithInsureeExport.Checked, chkFULL.Checked)
        End If

    End Sub
    'subs added by AMANI 28/09
    Private Sub OfflineExtractInBrowser(ByVal RegionId As Integer, ByVal DistrictId As Integer, ByVal WithInsuree As Boolean, ByVal IsFullExtract As Boolean)
        Try
            Dim eExtractInfo As New IMIS_EN.eExtractInfo
            Dim str As String
            ' If Val(ddlDistrictsOffLine.SelectedValue) > 0 Then
            eExtractInfo.RegionId = If(Val(ddlRegionOffLine.SelectedValue) = -1, 0, Val(ddlRegionOffLine.SelectedValue))
            eExtractInfo.DistrictId = Val(ddlDistrictsOffLine.SelectedValue)

            If eExtractInfo.DistrictId = 0 Then
                eExtractInfo.ExtractSequence = Extracts.NewSequenceNumber(eExtractInfo.RegionId)
            Else
                eExtractInfo.ExtractSequence = Extracts.NewSequenceNumber(eExtractInfo.DistrictId)
            End If



            'Else

            ' End If

            'Added by Amani 22/09/2017
            If WithInsuree = True Then
                eExtractInfo.WithInsuree = 1
            Else
                eExtractInfo.WithInsuree = 0
            End If



            eExtractInfo.AuditUserID = imisgen.getUserId(Session("User"))
            eExtractInfo.ExtractType = 4

            Extracts.CreateOffLineExtracts(eExtractInfo)
            If eExtractInfo.ExtractStatus = 0 Then
                str = imisgen.getMessage("M_EXTR_OFFLINEOK") & ExtractStatsMsgExport(eExtractInfo)
                If Not IsFullExtract Then imisgen.Alert(str, pnlButtons, alertPopupTitle:="IMIS", Queue:=True)

                'OffLineExtractLinkD.NavigateUrl = eExtractInfo.ExtractFileName
                OffLineExtractLinkD.Visible = True
                '
                If IsFullExtract = True Then

                    str = imisgen.getMessage("M_EXTR_OFFLINEFULL")
                    imisgen.Alert(str, pnlButtons, alertPopupTitle:="IMIS", Queue:=True)

                    eExtractInfo.LocationId = Val(ddlDistrictsOffLine.SelectedValue)
                    eExtractInfo.AuditUserID = imisgen.getUserId(Session("User"))
                    eExtractInfo.ExtractType = 2


                    'ADDED BY AMANI 26/09/2017
                    If WithInsuree = False Then
                        eExtractInfo.WithInsuree = False
                    Else
                        eExtractInfo.WithInsuree = True
                    End If
                    'ADDED END

                    Extracts.CreateOffLineExtracts(eExtractInfo)
                    If eExtractInfo.ExtractStatus = 0 Then
                        str = imisgen.getMessage("M_EXTR_OFFLINEOK") & ExtractStatsMsgExport(eExtractInfo)
                        'DivMsg.InnerHtml = str
                        imisgen.Alert(str, pnlButtons, alertPopupTitle:="IMIS", Queue:=True)
                        'OffLineExtractLinkF.NavigateUrl = eExtractInfo.ExtractFileName
                        OffLineExtractLinkF.Visible = True
                    Else
                        Select Case eExtractInfo.ExtractStatus
                            Case 1
                                str = imisgen.getMessage("M_EXTR_BUSY")
                            Case Else
                                str = imisgen.getMessage("M_EXTR_OFFLINENOK")
                        End Select
                        'DivMsg.InnerHtml = str
                        imisgen.Alert(str, pnlButtons, alertPopupTitle:="IMIS", Queue:=True)
                    End If

                End If
            Else
                Select Case eExtractInfo.ExtractStatus
                    Case 1
                        str = imisgen.getMessage("M_EXTR_BUSY")
                    Case Else
                        str = imisgen.getMessage("M_EXTR_OFFLINENOK")
                End Select
                'DivMsg.InnerHtml = str
                imisgen.Alert(str, pnlButtons, alertPopupTitle:="IMIS")
            End If

            'refresh the extracts list 
            Dim LocationId As Integer = If(Val(ddlDistrictsOffLine.SelectedValue) = 0, If(Val(ddlRegionOffLine.SelectedValue) = -1, 0, Val(ddlRegionOffLine.SelectedValue)), Val(ddlDistrictsOffLine.SelectedValue))
            ddlExtracts.DataSource = Extracts.GetExtractList(LocationId, 0, 4)
            ddlExtracts.DataValueField = "ExtractId"
            ddlExtracts.DataTextField = "ExtractInfo"
            ddlExtracts.DataBind()

        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    'Protected Sub btnUploadExtract_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUploadExtract.Click
    '    Try
    '        Dim Extract As New IMIS_BI.IMISExtractsBI
    '        Dim eDefaults As New IMIS_EN.tblIMISDefaults

    '        Extract.GetDefaults(eDefaults)

    '        'HVH Get the correct file to Import

    '        Extracts.ImportOffLineExtracts("ToBeReplaced", eDefaults.OffLineHF, imisgen.getUserId(Session("User")))
    '    Catch ex As Exception
    '        lblMsg.Text = ex.Message
    '    End Try
    'End Sub
    Protected Sub btnUploadExtract_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUploadExtract.Click
        Dim str As String = ""
        Try
            Dim eExtract As New IMIS_EN.tblExtracts
            Dim ExtractsBI As New IMIS_BI.IMISExtractsBI



            Extracts.CreateEnrolmentXML(Nothing, True)

            If txtFileUpload.HasFile Then
                If Right(txtFileUpload.FileName, 3) <> "RAR" Then
                    lblmsg.Text = imisgen.getMessage("M_EXTR_NOUPLOADLOADFILE")
                    Exit Sub
                End If
                If Left(txtFileUpload.FileName, 3) <> "OE_" Then
                    lblmsg.Text = imisgen.getMessage("M_EXTR_NOUPLOADLOADFILE")
                    Exit Sub
                End If

                txtFileUpload.SaveAs(Server.MapPath("Extracts\Offline\") & txtFileUpload.FileName)

            Else
                lblmsg.Text = imisgen.getMessage("M_EXTR_NOUPLOADLOADFILE")
                Exit Sub
            End If

            Dim eExtractInfo As New IMIS_EN.eExtractInfo
            'eExtractInfo.DistrictID = ddlDistrictsOffLine.SelectedValue
            eExtractInfo.AuditUserID = imisgen.getUserId(Session("User"))
            'eExtractInfo.ExtractType = 4
            eExtractInfo.ExtractFileName = txtFileUpload.FileName
            Extracts.ImportOffLineExtracts(eExtractInfo)

            Select Case eExtractInfo.ExtractStatus
                Case 0
                    str = imisgen.getMessage("M_EXTR_IMPOK")

                    If IMIS_Gen.offlineHF AndAlso hfExtractFound.Value = 0 Then
                        ExtractsBI.UpdateOfflineUserDistrict(IMIS_Gen.HFID)
                    End If

                Case 1
                    str = imisgen.getMessage("M_EXTR_BUSY")
                Case -1
                    str = imisgen.getMessage("M_EXTR_IMPERR1")
                Case -2
                    str = imisgen.getMessage("M_EXTR_IMPERR2")
                Case -3
                    str = imisgen.getMessage("M_EXTR_IMPERR3")
                Case -4
                    str = imisgen.getMessage("M_EXTR_IMPERR4")
                Case -5
                    str = imisgen.getMessage("M_EXTR_IMPERR5")
                Case -6
                    str = imisgen.getMessage("M_EXTR_IMPERR6")
                Case -7
                    str = imisgen.getMessage("M_EXTR_IMPERR7")
                Case -8
                    str = imisgen.getMessage("M_EXTR_IMPERR8")
            End Select
            If eExtractInfo.ExtractStatus = 0 Then
                str = str & ExtractStatsMsgImport(eExtractInfo)
            End If
            imisgen.Alert(str, pnlButtons, alertPopupTitle:="IMIS")

            eExtract = ExtractsBI.GetLastCreateExtractInfo(0, 0, 1)
            If eExtract.ExtractSequence = Nothing Then
                lblmsg.Text = imisgen.getMessage("M_EXTR_NOEXTRACTSFOUND")
            Else
                If eExtract.ExtractSequence = 0 Then
                    lblmsg.Text = imisgen.getMessage("M_EXTR_NOEXTRACTSFOUND")
                Else
                    lblmsg.Text = imisgen.getMessage("M_EXTR_EXTRACTSEQUENCE") & eExtract.ExtractSequence.ToString
                End If
            End If
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub
    Protected Sub btnUploadPhotos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUploadPhotos.Click
        Dim str As String = ""

        If txtFileUploadPhotos.HasFile Then
            If Right(txtFileUploadPhotos.FileName, 10) <> "Photos.RAR" Then
                lblmsg.Text = imisgen.getMessage("M_EXTR_NOUPLOADLOADFILE")
                Exit Sub
            End If

            txtFileUploadPhotos.SaveAs(Server.MapPath("Extracts\Offline\") & txtFileUploadPhotos.FileName)
        Else
            lblmsg.Text = imisgen.getMessage("M_EXTR_NOUPLOADLOADFILE")
            Exit Sub
        End If

        Dim eExtractInfo As New IMIS_EN.eExtractInfo
        'eExtractInfo.DistrictID = ddlDistrictsOffLine.SelectedValue
        eExtractInfo.AuditUserID = imisgen.getUserId(Session("User"))
        eExtractInfo.ExtractType = 8   'photos
        eExtractInfo.ExtractFileName = txtFileUploadPhotos.FileName
        Extracts.ImportOffLinePhotos(eExtractInfo)

        Select Case eExtractInfo.ExtractStatus
            Case 0
                str = imisgen.getMessage("M_EXTR_PHOTOIMPOK")
            Case 1
                str = imisgen.getMessage("M_EXTR_BUSY")
            Case -1
                str = imisgen.getMessage("M_EXTR_IMPERR1")
            Case -2
                str = imisgen.getMessage("M_EXTR_IMPERR2")
            Case -3
                str = imisgen.getMessage("M_EXTR_IMPERR3")
            Case -4
                str = imisgen.getMessage("M_EXTR_IMPERR4")
            Case -5
                str = imisgen.getMessage("M_EXTR_IMPERR5")
            Case -6
                str = imisgen.getMessage("M_EXTR_IMPERR6")
            Case -7
                str = imisgen.getMessage("M_EXTR_IMPERR7")

        End Select

        imisgen.Alert(str, pnlButtons, alertPopupTitle:="IMIS")

    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub
    Protected Sub PhoneExtractLink_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PhoneExtractLink.Click

        Response.AddHeader("Content-Disposition", "attachment;filename=ImisData.db3")
        Response.ContentType = "application/octet-stream"
        Response.TransmitFile(Server.MapPath("Extracts\Phone\ImisData" & ddlDistrictsPhone.SelectedValue & ".db3"))
        Response.End()
        Response.Flush()

    End Sub
    Protected Sub OffLineExtractLinkF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OffLineExtractLinkF.Click

        'ADDED BY AMANI 27/09
        Dim ExtractFileName As String = "OE_F%"

        Dim StrCommand As String
        Dim strFile As String
        '1st
        Dim location As Integer

        'Two changes Edited by Amani 22/09/2017.....If condition added to check if there is selected district

        '2nd
        If Val(ddlDistrictsOffLine.SelectedValue) = 0 Then location = If(Val(ddlRegionOffLine.SelectedValue) = -1, 0, Val(ddlRegionOffLine.SelectedValue)) Else location = Val(ddlDistrictsOffLine.SelectedValue)

        'FIRST THE actual EXTRACT
        strFile = Extracts.GetDownLoadExtractInfo(0, location, False, ExtractFileName)
        If strFile = "" Then
            Dim Str = imisgen.getMessage("M_EXTR_NODOWNLOADFILE")
            'DivMsg.InnerHtml = str
            imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If

        StrCommand = "attachment;filename=" & strFile
        'Response.AddHeader("Content-Disposition", "attachment;filename=ImisData.db3")
        Response.AddHeader("Content-Disposition", StrCommand)

        Response.ContentType = "application/octet-stream"

        'Response.TransmitFile(Server.MapPath("Extracts\OffLine\" & ddlDistrictsPhone.SelectedValue & ".db3"))
        Response.TransmitFile(Server.MapPath("Extracts\OffLine\" & strFile))

        Response.End()

        Response.Flush()

    End Sub


    'AMANI 27/09/2017
    Protected Sub OffLineExtractLinkE_Click(ByVal sender As Object, e As EventArgs) Handles OffLineExtractLinkE.Click

        'ADDED BY AMANI 27/09

        Dim ExtractFileName As String = "OE_E_%"

        Dim StrCommand As String
        Dim strFile As String
        '1st
        Dim location As Integer

        'Two changes Edited by Amani 22/09/2017.....If condition added to check if there is selected district

        '2nd
        If Val(ddlDistrictsOffLine.SelectedValue) = 0 Then location = Val(ddlRegionOffLine.SelectedValue) Else location = Val(ddlDistrictsOffLine.SelectedValue)

        'FIRST THE actual EXTRACT
        strFile = Extracts.GetDownLoadExtractInfo(0, location, False, ExtractFileName)
        If strFile = "" Then
            Dim Str = imisgen.getMessage("M_EXTR_NODOWNLOADFILE")
            'DivMsg.InnerHtml = str
            imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If

        StrCommand = "attachment;filename=" & strFile
        'Response.AddHeader("Content-Disposition", "attachment;filename=ImisData.db3")
        Response.AddHeader("Content-Disposition", StrCommand)

        Response.ContentType = "application/octet-stream"

        'Response.TransmitFile(Server.MapPath("Extracts\OffLine\" & ddlDistrictsPhone.SelectedValue & ".db3"))
        Response.TransmitFile(Server.MapPath("Extracts\OffLine\" & strFile))

        Response.End()

        Response.Flush()

    End Sub
    Protected Sub OffLinePhotoLinkF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OffLinePhotoLinkF.Click

        Dim StrCommand As String
        Dim strFile As String

        'FIRST THE actual EXTRACT
        strFile = Extracts.GetDownLoadExtractInfo(0, Val(ddlDistrictsOffLine.SelectedValue), False)
        If strFile = "" Then
            Dim Str = imisgen.getMessage("M_EXTR_NODOWNLOADFILE")
            'DivMsg.InnerHtml = str
            imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If

        'check now for photo zipped file
        'SECONDLY THE PHOTOS
        strFile = Extracts.GetDownLoadExtractInfo(0, Val(ddlDistrictsOffLine.SelectedValue), True)
        If strFile = "" Then
            Dim Str = imisgen.getMessage("M_EXTR_NOPHOTOFILE")
            'DivMsg.InnerHtml = str
            imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If

        StrCommand = "attachment;filename=" & strFile
        'Response.AddHeader("Content-Disposition", "attachment;filename=ImisData.db3")
        Response.AddHeader("Content-Disposition", StrCommand)

        Response.ContentType = "application/octet-stream"

        'Response.TransmitFile(Server.MapPath("Extracts\OffLine\" & ddlDistrictsPhone.SelectedValue & ".db3"))
        Response.TransmitFile(Server.MapPath("Extracts\OffLine\" & strFile))

        Response.End()

        Response.Flush()

    End Sub
    Protected Sub OffLinePhotoLinkD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OffLinePhotoLinkD.Click

        Dim StrCommand As String
        Dim strFile As String
        Dim strFilePhoto As String
        If ddlExtracts.SelectedValue = "" Then
            Dim Str = imisgen.getMessage("M_EXTR_NODOWNLOADFILE")
            'DivMsg.InnerHtml = str
            imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If

        'FIRST THE actual EXTRACT
        strFile = Extracts.GetDownLoadExtractInfo(ddlExtracts.SelectedValue, 0, False)
        If strFile = "" Then
            Dim Str = imisgen.getMessage("M_EXTR_NODOWNLOADFILE")
            'DivMsg.InnerHtml = str
            imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If

        strFilePhoto = Extracts.GetDownLoadExtractInfo(ddlExtracts.SelectedValue, 0, True)
        If strFilePhoto = "" Then
            Dim Str = imisgen.getMessage("M_EXTR_NOPHOTOFILE")
            'DivMsg.InnerHtml = str
            imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If

        StrCommand = "attachment;filename=" & strFilePhoto
        'Response.AddHeader("Content-Disposition", "attachment;filename=ImisData.db3")
        Response.AddHeader("Content-Disposition", StrCommand)
        Response.ContentType = "application/octet-stream"
        Response.TransmitFile(Server.MapPath("Extracts\OffLine\" & strFilePhoto))

        Response.End()

        Response.Flush()

    End Sub
    Protected Sub OffLineExtractLinkD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OffLineExtractLinkD.Click

        Dim StrCommand As String
        Dim strFile As String
        Dim strFilePhoto As String
        If ddlExtracts.SelectedValue = "" Then
            Dim Str = imisgen.getMessage("M_EXTR_NODOWNLOADFILE")
            'DivMsg.InnerHtml = str
            imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If

        'FIRST THE actual EXTRACT
        strFile = Extracts.GetDownLoadExtractInfo(ddlExtracts.SelectedValue, 0, False)
        If strFile = "" Then
            Dim Str = imisgen.getMessage("M_EXTR_NODOWNLOADFILE")
            'DivMsg.InnerHtml = str
            imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")
            Exit Sub
        End If

        StrCommand = "attachment;filename=" & strFile
        'Response.AddHeader("Content-Disposition", "attachment;filename=ImisData.db3")
        Response.AddHeader("Content-Disposition", StrCommand)
        Response.ContentType = "application/octet-stream"

        'Response.TransmitFile(Server.MapPath("Extracts\OffLine\" & ddlDistrictsPhone.SelectedValue & ".db3"))
        Response.TransmitFile(Server.MapPath("Extracts\OffLine\" & strFile))

        Response.End()

        Response.Flush()



        'SECONDLY THE PHOTOS
        strFilePhoto = Extracts.GetDownLoadExtractInfo(ddlExtracts.SelectedValue, 0, True)
        If strFilePhoto = "" Then
            Dim Str = imisgen.getMessage("M_EXTR_NOPHOTOFILE")
            'DivMsg.InnerHtml = str
            imisgen.Alert(Str, pnlButtons, alertPopupTitle:="IMIS")
        End If


        If strFilePhoto <> "" Then
            StrCommand = "attachment;filename=" & strFilePhoto
            Response.AppendHeader("Content-Disposition", StrCommand)
            Response.ContentType = "application/octet-stream"
            Response.TransmitFile(Server.MapPath("Extracts\OffLine\" & strFilePhoto))
        End If



    End Sub
    Protected Sub btnDownloadClaim_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDownloadClaim.Click
        Try
            Dim eDef As New IMIS_EN.tblIMISDefaults
            Extracts.GetDefaults(eDef)

            Dim strCommand As String = ""
            Dim rar As String() = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("Workspace"), "Claim*.rar")
            If rar.Count > 0 Then
                Dim FileName As String = Mid(rar(0), rar(0).LastIndexOf("\") + 2, rar(0).Length)

                strCommand = "attachment;filename=" & FileName
                Response.AppendHeader("Content-Disposition", strCommand)
                Response.ContentType = "application/octet-stream"
                Response.WriteFile(Server.MapPath("Workspace\" & FileName))
                Response.Flush()
                IO.File.Delete(Server.MapPath("Workspace\") & FileName)
                Response.End()
            Else
                lblmsg.Text = "No claims found."
            End If
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            lblmsg.Text = ""
            If FileUpload1.HasFile Then
                'Amani added random folder 30/11/2017
                Dim WorkFolder As String = HttpContext.Current.Server.MapPath("Workspace\")
                Dim RandomFolderName As String = Path.GetRandomFileName
                My.Computer.FileSystem.CreateDirectory(WorkFolder & RandomFolderName)
                Dim FileName As String = Server.MapPath("Workspace\") & RandomFolderName & "\" & FileUpload1.PostedFile.FileName
                FileUpload1.SaveAs(FileName)
                Extracts.SubmitClaimFromXML(FileName)
                lblmsg.Text = imisgen.getMessage("M_CLAIMUPLOADED")
            End If
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub

    Private Sub btnDownloadEnrolment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownloadEnrolment.Click
        Try
            Dim Output As New Dictionary(Of String, Integer)
            Output.Add("FamilyExported", 0)
            Output.Add("InsureeExported", 0)
            Output.Add("PolicyExported", 0)
            Output.Add("PremiumExported", 0)
            Extracts.CreateEnrolmentXML(Output, False)
            Dim Msg As String = "<h4><u>" & imisgen.getMessage("M_ENROLMENTEXTRACTED") & "</u></h4>" & "<br>" & _
                                    "<table class=""tblPopupMsg""><tr><td class=""str"">" & imisgen.getMessage("L_FAMILY") & ":</td><td class=""no"">" & Output("FamilyExported") & "</td></tr><tr><td class=""str"">" & _
                                    imisgen.getMessage("L_INSUREE") & ":</td><td class=""no"">" & Output("InsureeExported") & "</td></tr><tr><td class=""str"">" & imisgen.getMessage("L_POLICY") & _
                                    ":</td><td class=""no"">" & Output("PolicyExported") & "</td></tr><tr><td class=""str"">" & imisgen.getMessage("L_PREMIUM") & ":</td><td class=""no"">" & Output("PremiumExported") & _
                                    "</td></tr></table>"
            imisgen.Alert(Msg, pnlButtons, alertPopupTitle:="IMIS")
        Catch ex As Exception
            imisgen.Alert(ex.Message, pnlButtons, alertPopupTitle:="IMIS")
            Return
        End Try
        iFrame.Attributes.Add("src", "IMISExtracts.aspx?dl=1")
    End Sub
    Private Sub DownloadEnrolments()
        Dim eDef As New IMIS_EN.tblIMISDefaults
        Extracts.GetDefaults(eDef)

        Dim strCommand As String = ""
        Dim rar As String() = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("Workspace"), "Enrolment*.rar")
        If rar.Count > 0 Then
            Dim FileName As String = Mid(rar(0), rar(0).LastIndexOf("\") + 2, rar(0).Length)

            strCommand = "attachment;filename=" & FileName
            Response.AppendHeader("Content-Disposition", strCommand)
            Response.ContentType = "application/octet-stream"
            Response.WriteFile(Server.MapPath("Workspace\" & FileName))
            Response.Flush()
            IO.File.Delete(Server.MapPath("Workspace\") & FileName)
            Response.End()
        Else
            lblmsg.Text = imisgen.getMessage("M_NOENROLMENTSFOUND")
        End If
    End Sub
    Private Sub btnUploadEnrolments_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadEnrolments.Click
        If fuEnrolments.HasFile Then
            Dim FileName As String = Server.MapPath("Workspace\") & fuEnrolments.PostedFile.FileName
            fuEnrolments.SaveAs(FileName)
            Dim Output As New Dictionary(Of String, Integer)
            Dim ResultType As Integer

            Output.Add("FamilySent", 0)
            Output.Add("FamilyImported", 0)
            Output.Add("FamiliesUpd", 0)
            Output.Add("FamilyRejected", 0)
            Output.Add("InsureeSent", 0)
            Output.Add("InsureeUpd", 0)
            Output.Add("InsureeImported", 0)
            Output.Add("PolicySent", 0)
            Output.Add("PolicyImported", 0)
            Output.Add("PolicyChanged", 0)
            Output.Add("PolicyRejected", 0)
            Output.Add("PremiumSent", 0)
            Output.Add("PremiumImported", 0)
            Output.Add("PremiumRejected", 0)
            Output.Add("ResultTyple", 0)
            Output.Add("PhotoSent", 0)
            Output.Add("PhotoAccepted", 0)
            Output.Add("PhotoRejected", 0)


            Dim dt As DataTable = Extracts.UploadEnrolments(FileName, Output)
            ResultType = Output("ResultTyple")
            Dim Msg As String = "<h4><u>" & imisgen.getMessage("M_ENROLMENTUPLOADED") & "</u></h4>" & "<br>"
            If ResultType = 1 Then
                Msg += "<table class=""tblPopupMsg"">" &
                                "<tr><th></th><th>" & imisgen.getMessage("L_SENT") & "</th><th>" & imisgen.getMessage("L_UPLOADED") & "</th></tr>" &
                                "<tr><td class=""str"">" & imisgen.getMessage("L_FAMILY") & "</td><td class=""no"">" & Output("FamilySent") & "</td><td class=""no"">" & Output("FamilyImported") & "</td></tr>" &
                                "<tr><td class=""str"">" & imisgen.getMessage("L_INSUREE") & "</td><td class=""no"">" & Output("InsureeSent") & "</td><td class=""no"">" & Output("InsureeImported") & "</td></tr>" &
                                "<tr><td class=""str"">" & imisgen.getMessage("L_POLICY") & "</td><td class=""no"">" & Output("PolicySent") & "</td><td class=""no"">" & Output("PolicyImported") & "</td></tr>" &
                                "<tr><td class=""str"">" & imisgen.getMessage("L_PREMIUM") & "</td><td class=""no"">" & Output("PremiumSent") & "</td><td class=""no"">" & Output("PremiumImported") & "</td></tr>" &
                                "</table>"
            ElseIf ResultType = 2 Then
                Msg += "<br>" & imisgen.getMessage("L_FAMILY") & "<br>" &
                        imisgen.getMessage("L_SENT") & ": " & Output("FamilySent") & "<br>" &
                        imisgen.getMessage("L_ACCEPTED") & ": " & Output("FamilyImported") & "<br>" &
                        imisgen.getMessage("L_UPDATED") & ": " & Output("FamiliesUpd") & "<br>" &
                        imisgen.getMessage("L_REJECTED") & ": " & Output("FamilyRejected") & "</br>" &
                        "<br>" & imisgen.getMessage("L_INSUREE") & "<br>" &
                        imisgen.getMessage("L_SENT") & ": " & Output("InsureeSent") & "<br>" &
                        imisgen.getMessage("L_ACCEPTED") & ": " & Output("InsureeImported") & "<br>" &
                        imisgen.getMessage("L_UPDATED") & ": " & Output("InsureeUpd") & "</br>" &
                         "<br>" & imisgen.getMessage("L_POLICY") & "<br>" &
                        imisgen.getMessage("L_SENT") & ": " & Output("PolicySent") & "<br>" &
                        imisgen.getMessage("L_ACCEPTED") & ": " & Output("PolicyImported") & "<br>" &
                        imisgen.getMessage("L_UPDATED") & ": " & Output("PolicyChanged") & "<br>" &
                         "<br>" & imisgen.getMessage("L_PREMIUM") & "<br>" &
                        imisgen.getMessage("L_SENT") & ": " & Output("PremiumSent") & "<br>" &
                        imisgen.getMessage("L_ACCEPTED") & ": " & Output("PremiumImported") & "<br>" &
                        "<br>" & imisgen.getMessage("L_PHOTOS") & "<br>" &
                        imisgen.getMessage("L_SENT") & ": " & Output("PhotoSent") & "<br>" &
                        imisgen.getMessage("L_ACCEPTED") & ": " & Output("PhotoAccepted") & "<br>" &
                        imisgen.getMessage("L_REJECTED") & ": " & Output("PhotoRejected") & "<br>"
            Else
                Msg += imisgen.getMessage("M_NOENROLLMENTFOUND")
            End If


            imisgen.Alert(Msg, pnlButtons, alertPopupTitle:="IMIS", Queue:=True)

            If dt.Rows.Count > 0 Then
                Msg = "<h4><u>" & imisgen.getMessage("M_ENROLLOG") & "</u></h4>" & "<br>" & _
                                   "<div style=""height:500px;overflow:auto;""><table class=""tblPopupMsg"" style=""width:100%;"">"

                For i As Integer = 0 To dt.Rows.Count - 1
                    Msg += "<tr><td style=""border-right:1px solid #000000;padding:0px;padding-left:10px;padding-right:10px;"">" & i + 1 & "</td><td style=""padding:2px 0px;""><span style=""display:block;padding-top:3px;padding-bottom:3px;text-align:left;padding-left:3px;"">" & dt.Rows(i)("Result").ToString & "</span></td></tr>"
                Next
                Msg += "</table></div>"

                imisgen.Alert(Msg, pnlButtons, alertPopupTitle:="IMIS", Queue:=True)
            End If
        End If
    End Sub

    Private Sub ddlRegionOffLine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegionOffLine.SelectedIndexChanged
        FillDistrictsOffLine()
    End Sub

    Private Sub ddlRegionPhone_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegionPhone.SelectedIndexChanged
        FillDistrictsPhone()
    End Sub


    Protected Sub btnDownLoadMasterData_Click(sender As Object, e As EventArgs) Handles btnDownLoadMasterData.Click

        Dim Extracts As New IMIS_BI.IMISExtractsBI
        Dim strCommand As String = "MasterData.RAR"
        Try
            Dim FileName As String = Extracts.DownloadMasterData()
            Dim Path As String = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings("ExportFolder"))
            If FileName.Length > 0 Then

                strCommand = "attachment;filename=" & FileName
                Response.AppendHeader("Content-Disposition", strCommand)
                Response.ContentType = "application/octet-stream"
                Response.WriteFile(Path & FileName)
                Response.Flush()
                IO.File.Delete(Path & FileName)
                Response.End()
            Else
                lblmsg.Text = imisgen.getMessage("M_NODATAFOUND")
            End If
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.ToString(), EventLogEntryType.Error, 999)
        End Try
    End Sub

    'Protected Sub BtnUploadPhotosFromPhone_Click(sender As Object, e As EventArgs) Handles BtnUploadPhotosFromPhone.Click
    '    Dim str As String = ""
    '    If fuUploadPhotos.HasFile Then
    '        If Right(fuUploadPhotos.FileName, 4).ToLower() <> ".rar" Then
    '            lblmsg.Text = imisgen.getMessage("M_EXTR_NOUPLOADLOADFILE")
    '            Exit Sub
    '        End If

    '        fuUploadPhotos.SaveAs(Server.MapPath("Extracts\Offline\") & fuUploadPhotos.FileName)
    '    Else
    '        lblmsg.Text = imisgen.getMessage("M_EXTR_NOUPLOADLOADFILE")
    '        Exit Sub
    '    End If

    '    Dim eExtractInfo As New IMIS_EN.eExtractInfo
    '    'eExtractInfo.DistrictID = ddlDistrictsOffLine.SelectedValue
    '    eExtractInfo.AuditUserID = imisgen.getUserId(Session("User"))
    '    eExtractInfo.ExtractType = 8   'photos
    '    eExtractInfo.ExtractFileName = fuUploadPhotos.FileName
    '    Extracts.ImportOffLinePhotosFromPhone(eExtractInfo)

    '    Select Case eExtractInfo.ExtractStatus
    '        Case 0
    '            str = imisgen.getMessage("M_EXTR_PHOTOIMPOK")
    '        Case 1
    '            str = imisgen.getMessage("M_EXTR_BUSY")
    '        Case -1
    '            str = imisgen.getMessage("M_EXTR_IMPERR1")
    '        Case -2
    '            str = imisgen.getMessage("M_EXTR_IMPERR2")
    '        Case -3
    '            str = imisgen.getMessage("M_EXTR_IMPERR3")
    '        Case -4
    '            str = imisgen.getMessage("M_EXTR_IMPERR4")
    '        Case -5
    '            str = imisgen.getMessage("M_EXTR_IMPERR5")
    '        Case -6
    '            str = imisgen.getMessage("M_EXTR_IMPERR6")
    '        Case -7
    '            str = imisgen.getMessage("M_EXTR_IMPERR7")

    '    End Select

    '    imisgen.Alert(str, pnlButtons, alertPopupTitle:="IMIS")
    'End Sub

    Protected Sub btnDownLoadFeedback_Click(sender As Object, e As EventArgs)
        Dim OfficerCode As String
        OfficerCode = txtOfficerCode.Text
        Dim result As New Dictionary(Of String, String)
        result = Extracts.getFeedback(OfficerCode)
        Dim ResultCode As String = result("ResultCode")
        Dim str As String = ""
        Select Case ResultCode
            Case "0"
                Dim strCommand As String = ""
                Dim FileName As String = result("result")
                Dim Path As String = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings("ExportFolder"))
                If FileName.Length > 0 Then
                    strCommand = "attachment;filename=" & FileName
                    Response.AppendHeader("Content-Disposition", strCommand)
                    Response.ContentType = "application/octet-stream"
                    Response.WriteFile(Path & FileName)
                    Response.Flush()
                    IO.File.Delete(Path & FileName)
                    Response.End()
                End If
            Case "1"
                imisgen.Alert(imisgen.getMessage("M_OFFICERNOTFOUND"), pnlButtons, alertPopupTitle:="IMIS", Queue:=True)
            Case "2"
                imisgen.Alert(imisgen.getMessage("M_NODATAFOUND"), pnlButtons, alertPopupTitle:="IMIS", Queue:=True)
        End Select
    End Sub

    Protected Sub btnDownLoadRenewal_Click(sender As Object, e As EventArgs)
        Dim OfficerCode As String
        OfficerCode = txtOfficerCode.Text
        Dim result As New Dictionary(Of String, String)
        result = Extracts.getRenewals(OfficerCode)
        Dim ResultCode As String = result("ResultCode")
        Dim str As String = ""
        Select Case ResultCode
            Case "0"
                Dim strCommand As String = ""
                Dim FileName As String = result("result")
                Dim Path As String = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings("ExportFolder"))
                If FileName.Length > 0 Then
                    strCommand = "attachment;filename=" & FileName
                    Response.AppendHeader("Content-Disposition", strCommand)
                    Response.ContentType = "application/octet-stream"
                    Response.WriteFile(Path & FileName)
                    Response.Flush()
                    IO.File.Delete(Path & FileName)
                    Response.End()
                End If
            Case "1"
                imisgen.Alert(imisgen.getMessage("M_OFFICERNOTFOUND"), pnlButtons, alertPopupTitle:="IMIS", Queue:=True)
            Case "2"
                imisgen.Alert(imisgen.getMessage("M_NODATAFOUND"), pnlButtons, alertPopupTitle:="IMIS", Queue:=True)
        End Select
    End Sub

    Protected Sub BtnUploadFeeBack_Click(sender As Object, e As EventArgs)
        Dim str As String = ""
        If FileUploadFeedBack.HasFile Then
            If Right(FileUploadFeedBack.FileName, 4).ToLower() <> ".rar" Then
                lblmsg.Text = imisgen.getMessage("M_EXTR_NOUPLOADLOADFILE")
                Exit Sub
            End If
            Dim filename As String = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings("FromPhone_Feedback")) & FileUploadFeedBack.FileName
            FileUploadFeedBack.SaveAs(filename)
            Dim Output As New Dictionary(Of String, Integer)
            Output = Extracts.UploadFeedBackFromPhone(filename)


            Dim Msg As String = "<h4><u>" & imisgen.getMessage("M_FEEDFILEACCEPTED") & "</u></h4>" & "<br>" &
                                 imisgen.getMessage("L_SENT") & ": " & Output("Sent") & "<br>" &
                                 imisgen.getMessage("L_ACCEPTED") & ": " & Output("Accepted") & "<br>" &
                                 imisgen.getMessage("L_REJECTED") & ": " & Output("Rejected") & "<br>" &
                                 imisgen.getMessage("L_FAILED") & ": " & Output("Failed") & "<br>" &
                                 imisgen.getMessage("L_EXISTS") & ": " & Output("Exists") & "</br><br>"

            imisgen.Alert(Msg, Panel1, alertPopupTitle:="IMIS", Queue:=True)
        Else
            Exit Sub
        End If
        imisgen.Alert(str, pnlButtons, alertPopupTitle:="IMIS")
    End Sub

    Protected Sub BtnUploadRenewal_Click(sender As Object, e As EventArgs)
        Dim str As String = ""
        If FileUploadRenewal.HasFile Then
            If Right(FileUploadRenewal.FileName, 4).ToLower() <> ".rar" Then
                lblmsg.Text = imisgen.getMessage("M_EXTR_NOUPLOADLOADFILE")
                Exit Sub
            End If
            Dim filename As String = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings("FromPhone_Renewal")) & FileUploadRenewal.FileName
            FileUploadRenewal.SaveAs(filename)
            Dim Output As New Dictionary(Of String, Integer)
            Output = Extracts.UploadRenewalFromPhone(filename)


            Dim Msg As String = "<h4><u>" & imisgen.getMessage("M_RENEWALUPLOADED") & "</u></h4>" & "<br>" &
                                 imisgen.getMessage("L_SENT") & ": " & Output("Sent") & "<br>" &
                                 imisgen.getMessage("L_ACCEPTED") & ": " & Output("Accepted") & "<br>" &
                                 imisgen.getMessage("L_REJECTED") & ": " & Output("Rejected") & "<br>" &
                                 imisgen.getMessage("L_FAILED") & ": " & Output("Failed") & "<br>" &
                                 imisgen.getMessage("L_EXISTS") & ": " & Output("Exists") & "</br><br>"

            imisgen.Alert(Msg, Panel1, alertPopupTitle:="IMIS", Queue:=True)
        Else
            Exit Sub
        End If
        imisgen.Alert(str, pnlButtons, alertPopupTitle:="IMIS")
    End Sub


End Class