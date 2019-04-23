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

Partial Public Class OverviewFamily
    Inherits System.Web.UI.Page
    Private FamilyId As Integer = 0
    Private eInsuree As New IMIS_EN.tblInsuree
    Private ePolicy As New IMIS_EN.tblPolicy
    Private ePremium As New IMIS_EN.tblPremium
    Private eFamily As New IMIS_EN.tblFamilies
    Dim OverviewFamily As New IMIS_BI.OverviewFamilyBI
    Protected imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private Sub FormatForm()
        Dim Adjustibility As String = ""
     

        'Confirmation
        Adjustibility = General.getControlSetting("Confirmation")
        lblConfirmation.Visible = Not (Adjustibility = "N")
        txtConfirmationType.Visible = Not (Adjustibility = "N")

        Adjustibility = General.getControlSetting("ConfirmationNo")
        lblConfirmationNo.Visible = Not (Adjustibility = "N")
        txtConfirmationNo.Visible = Not (Adjustibility = "N")

        'Poverty
        Adjustibility = General.getControlSetting("Poverty")
        lblPoverty.Visible = Not (Adjustibility = "N")
        txtPoverty.Visible = Not (Adjustibility = "N")


        'Permanent Address
        Adjustibility = General.getControlSetting("PermanentAddress")
        L_ADDRESS0.Visible = Not (Adjustibility = "N")
        txtPermanentAddress.Visible = Not (Adjustibility = "N")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FormatForm()

        RunPageSecurity()

        If Session("User") Is Nothing Then Response.Redirect("Default.aspx")
        FamilyId = HttpContext.Current.Request.QueryString("f")

        If Not HttpContext.Current.Request.QueryString("i") Is Nothing Then
            eInsuree.InsureeID = CInt(HttpContext.Current.Request.QueryString("i"))

        End If
        If Not HttpContext.Current.Request.QueryString("po") Is Nothing Then
            ePolicy.PolicyID = CInt(HttpContext.Current.Request.QueryString("po"))

        End If
        If Not HttpContext.Current.Request.QueryString("p") Is Nothing Then
            ePremium.PremiumId = CInt(HttpContext.Current.Request.QueryString("p"))

        End If

        If Request.Form("__EVENTTARGET") = AddPremium.ClientID Then
            AddPremium_Click(sender, New System.Web.UI.ImageClickEventArgs(0, 0))

        ElseIf Request.Form("__EVENTTARGET") = DeleteFamily.ClientID Then
            DeleteFamily_Click(sender, New System.Web.UI.ImageClickEventArgs(0, 0))

        ElseIf Request.Form("__EVENTTARGET") = DeleteInsuree.ClientID Then
            DeleteInsuree_Click(sender, New System.Web.UI.ImageClickEventArgs(0, 0))

        ElseIf Request.Form("__EVENTTARGET") = DeletePolicy.ClientID Then
            DeletePolicy_Click(sender, New System.Web.UI.ImageClickEventArgs(0, 0))

        ElseIf Request.Form("__EVENTTARGET") = DeletePremium.ClientID Then
            DeletePremium_Click(sender, New System.Web.UI.ImageClickEventArgs(0, 0))
        ElseIf Request.Form("__EVENTTARGET") = btnRenewPolicy.ClientID Then
            btnRenewPolicy_Click(sender, New System.Web.UI.ImageClickEventArgs(0, 0))
        End If

        Dim RefUrl = Request.Headers("Referer")
        Dim reg As New Regex("OverviewFamily", RegexOptions.IgnoreCase) '

        If IsPostBack = True Then Return

        Try

            loadSecurity()
            Dim load As New IMIS_BI.OverviewFamilyBI
            Dim dt As DataTable

            dt = load.GetInsureesByFamilyFiltered(FamilyId)
            loadGrid(gvInsurees, dt)

            dt = load.GetPolicybyFamily(FamilyId)
            CheckPolicyValueAgainstInsuree(dt)
            loadGrid(gvPolicies, dt)

            If Not gvPolicies.Rows.Count = 0 Then
                dt = load.GetPremiumsByPolicy(gvPolicies.SelectedDataKey.Values("PolicyID"))
                loadGrid(gvPremiums, dt)
                If gvPolicies.SelectedIndex >= 0 Then
                    hfPolicyValue.Value = gvPolicies.Rows(gvPolicies.SelectedIndex).Cells(7).Text
                End If
            Else
                gvPremiums.DataSource = New DataTable()
                gvPremiums.DataBind()
            End If

            DisableEmptyGridEditDeleteButtons(gvPremiums)
            DisableEmptyGridEditDeleteButtons(gvPolicies)

            eFamily.FamilyID = FamilyId
            OverviewFamily.GetFamilyHeadInfo(eFamily)
            txtRegion.Text = eFamily.RegionName
            txtDistrict.Text = eFamily.DistrictName
            txtVillage.Text = eFamily.VillageName
            txtWard.Text = eFamily.WardName
            If eFamily.tblInsuree IsNot Nothing Then
                txtHeadCHFID.Text = eFamily.tblInsuree.CHFID
                txtHeadLastName.Text = eFamily.tblInsuree.LastName
                txtHeadOtherNames.Text = eFamily.tblInsuree.OtherNames
            End If
            txtPoverty.Text = If(eFamily.Poverty Is Nothing, "", If(eFamily.Poverty = True, "Yes", "No"))
            txtConfirmationType.Text = eFamily.ConfirmationType
          

            ''txtHeadPhone.Text = eFamily.tblInsuree.Phone
            '' txtEthnicity.Text = eFamily.Ethnicity
            txtHeadGroupType.Text = If(Request.Cookies("CultureInfo").Value = "en", eFamily.tblFamilyTypes.FamilyType, eFamily.tblFamilyTypes.AltLanguage)
            txtConfirmationNo.Text = eFamily.ConfirmationNo
            txtPermanentAddress.Text = eFamily.FamilyAddress
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlPremiums, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message & " : " & ex.StackTrace, EventLogEntryType.Error, 999)
            Return
        End Try
    End Sub
    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False, Optional ByVal comm As String = "delete")
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        If Not ondelete Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.OverviewFamily, Page) Then
                AddFamily.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.AddFamily, RoleID)
                EditFamily.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.EditFamily, RoleID)
                DeleteFamily.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.DeleteFamily, RoleID)
                AddInsuree.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.AddInsuree, RoleID)
                EditInsuree.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.EditInsuree, RoleID)
                DeleteInsuree.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.DeleteInsuree, RoleID)
                AddPolicy.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.AddPolicy, RoleID)
                EditPolicy.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.EditPolicy, RoleID)
                DeletePolicy.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.DeletePolicy, RoleID)
                AddPremium.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.AddPremium, RoleID)
                EditPremium.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.EditPremium, RoleID)
                DeletePremium.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.DeletePremium, RoleID)
               
                If Not (AddFamily.Visible Or EditFamily.Visible Or DeleteFamily.Visible) Then
                    L_FAMILYPANEL.Enabled = False
                End If

                If Not (AddInsuree.Visible Or EditInsuree.Visible Or DeleteInsuree.Visible) Then
                    Panel1.Enabled = False
                End If

                If Not (AddPolicy.Visible Or EditPolicy.Visible Or DeletePolicy.Visible) Then
                    Panel2.Enabled = False
                End If

                If Not (AddPremium.Visible Or EditPremium.Visible Or DeletePremium.Visible) Then
                    pnlPremiums.Enabled = False
                End If

            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.OverviewFamily.ToString & "&retUrl=" & RefUrl)
            End If
        Else
            If comm = "deletefamily" Then
                If Not OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.DeleteFamily, RoleID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.OverviewFamily.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf comm = "deleteinsuree" Then
                If Not OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.DeleteInsuree, RoleID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.OverviewFamily.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf comm = "deletepolicy" Then
                If Not OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.DeletePolicy, RoleID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.OverviewFamily.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf comm = "deletepremium" Then
                If Not OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.DeletePremium, RoleID) Then
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.OverviewFamily.ToString & "&retUrl=" & RefUrl)
                End If
            End If
        End If
    End Sub
    Private Sub CheckPolicyValueAgainstInsuree(ByRef dt As DataTable)
        Try
            Dim efamily As IMIS_EN.tblFamilies
            Dim eproduct As IMIS_EN.tblProduct
            Dim epolicy As IMIS_EN.tblPolicy
            Dim policy As New IMIS_BI.PolicyBI
            Dim chk As Integer = 0
            Dim msg As String = ""
            Dim policyValueCurrent As Decimal

            For Each dr As DataRow In dt.Rows
                efamily = New IMIS_EN.tblFamilies
                eproduct = New IMIS_EN.tblProduct
                epolicy = New IMIS_EN.tblPolicy
                policyValueCurrent = 0

                If Not dr("PolicyValue") Is Nothing Then
                    policyValueCurrent = dr("PolicyValue")
                End If
                If Not dr("PolicyId") Is Nothing Then
                    epolicy.PolicyID = dr("PolicyId")
                End If
                If Not dr("EnrollDate") Is Nothing Then
                    epolicy.EnrollDate = dr("EnrollDate")
                End If
                If Not dr("PolicyStage") Is Nothing Then
                    epolicy.PolicyStage = dr("PolicyStage").ToString
                End If
                If Not dr("FamilyId") Is Nothing Then
                    efamily.FamilyID = dr("FamilyId")
                End If
                If Not dr("ProdID") Is Nothing Then
                    eproduct.ProdID = dr("ProdID")
                End If

                epolicy.tblFamilies = efamily
                epolicy.tblProduct = eproduct

                OverviewFamily.GetPolicyValue(epolicy, 0)

                If policyValueCurrent <> epolicy.PolicyValue Then
                    epolicy.AuditUserID = imisgen.getUserId(Session("user"))
                    ' epolicy.PolicyStatus = 1
                    chk = 0
                    chk = OverviewFamily.SavePolicy(epolicy)
                    If chk = 1 Then
                        msg += imisgen.getMessage("M_POLICYVALUECHANGE") & epolicy.EnrollDate & " " & imisgen.getMessage("M_CHANGE") & "<br/>"

                        dr("PolicyValue") = epolicy.PolicyValue
                        'dr("PolicyStatus") = OverviewFamily.ReturnPolicyStatus(epolicy.PolicyStatus)

                    End If
                End If
            Next
            If msg.Trim <> String.Empty Then imisgen.Alert(msg, pnlMsgHolder)
            'If Trim(lblMsg.Text).Length > 0 Then
            '    lblMsg.Text = lblMsg.Text & "<br/>" & msg
            'Else
            '    lblMsg.Text = msg
            'End If
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlPremiums, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Exit Sub
        End Try
    End Sub
    Private Sub DisableEmptyGridEditDeleteButtons(ByVal gv As GridView)
        Dim count As Integer = gv.Rows.Count
        Select Case gv.ID
            Case gvPremiums.ID
                If count = 0 Then
                    EditPremium.Visible = False
                    DeletePremium.Visible = False

                End If
            Case gvPolicies.ID
                If count = 0 Then
                    EditPolicy.Visible = False
                    DeletePolicy.Visible = False
                    btnRenewPolicy.Visible = False
                    AddPremium.Visible = False
                End If
        End Select

    End Sub
    Private Sub loadSecurity()
        Dim RoleId As Integer = imisgen.getRoleId(Session("User"))
        AddFamily.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.AddFamily, RoleId)
        AddInsuree.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.AddInsuree, RoleId)
        AddPolicy.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.AddPolicy, RoleId)
        AddPremium.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.AddPremium, RoleId)
        EditFamily.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.EditFamily, RoleId)
        EditInsuree.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.EditInsuree, RoleId)
        EditPolicy.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.EditPolicy, RoleId)
        EditPremium.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.EditPremium, RoleId)
        DeleteFamily.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.DeleteFamily, RoleId)
        DeleteInsuree.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.DeleteInsuree, RoleId)
        DeletePolicy.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.DeletePolicy, RoleId)
        DeletePremium.Visible = OverviewFamily.checkRoles(IMIS_EN.Enums.Rights.DeletePremium, RoleId)
        If Not EditInsuree.Visible Then
            Dim hlink As HyperLinkField = gvInsurees.Columns(1)
            hlink.DataNavigateUrlFormatString = "#"
        End If
        If Not EditPolicy.Visible Then
            Dim hlink As HyperLinkField = gvPolicies.Columns(0)
            hlink.DataNavigateUrlFormatString = "#"
        End If
        If Not EditPremium.Visible Then
            Dim hlink As HyperLinkField = gvPremiums.Columns(0)
            hlink.DataNavigateUrlFormatString = "#"
        End If
    End Sub
    Private Sub loadGrid(ByRef gv As GridView, ByRef dt As DataTable)
        Dim x As Integer = 0
        Dim Rows() As DataRow
        Dim dv As DataView = dt.DefaultView
        Try
            If Not IsPostBack = True Then

                If gv.ID = "gvInsurees" Then

                    Rows = dt.Select("InsureeID=" & eInsuree.InsureeID)
                    dv.Sort = "CHFID"
                    If Rows.Length > 0 Then
                        x = dv.Find(Rows(0).Item("CHFID"))
                    End If

                ElseIf gv.ID = "gvPolicies" Then

                    Rows = dt.Select("PolicyID=" & ePolicy.PolicyID)
                    dv.Sort = "EnrollDate DESC"
                    If Rows.Length > 0 Then
                        x = dv.Find(Rows(0).Item("EnrollDate"))
                    End If

                ElseIf gv.ID = "gvPremiums" Then

                    Rows = dt.Select("PremiumId=" & ePremium.PremiumId)
                    dv.Sort = "PayerName,PremiumId"
                    If Rows.Length > 0 Then
                        Dim a() As Object = {Rows(0).Item("PayerName"), Rows(0).Item("PremiumId")}
                        x = dv.Find(a)
                    End If

                Else
                    Return
                End If



            End If
            gv.DataSource = dv
            gv.SelectedIndex = x
            gv.DataBind()
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlPremiums, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvInsurees)
        AddRowSelectToGridView(gvPolicies)
        AddRowSelectToGridView(gvPremiums)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub

    Private Sub gv_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvInsurees.PageIndexChanging, gvPolicies.PageIndexChanging, gvPremiums.PageIndexChanging
        sender.PageIndex = e.NewPageIndex
    End Sub
    Private Sub gv_PageIndexChanged(sender As Object, e As EventArgs) Handles gvInsurees.PageIndexChanged, gvPolicies.PageIndexChanged, gvPremiums.PageIndexChanged
        Try
            Dim load As New IMIS_BI.OverviewFamilyBI
            FamilyId = HttpContext.Current.Request.QueryString("f")
            Dim dt As DataTable
            Dim gv As GridView = sender
            If gv.ID = gvInsurees.ID Then
                dt = load.GetInsureesByFamilyFiltered(FamilyId)
                loadGrid(gvInsurees, dt)
            ElseIf gv.ID = gvPolicies.ID Then
                dt = load.GetPolicybyFamily(FamilyId)
                loadGrid(gvPolicies, dt)

                If Not gvPolicies.Rows.Count = 0 Then
                    dt = load.GetPremiumsByPolicy(gvPolicies.SelectedDataKey.Values("PolicyID"))
                    loadGrid(gvPremiums, dt)
                    If gvPolicies.SelectedIndex >= 0 Then
                        hfPolicyValue.Value = gvPolicies.Rows(gvPolicies.SelectedIndex).Cells(7).Text
                    End If
                Else
                    gvPremiums.DataSource = New DataTable()
                    gvPremiums.DataBind()
                End If
            ElseIf gv.ID = gvPremiums.ID Then
                dt = load.GetPremiumsByPolicy(gvPolicies.SelectedDataKey.Values("PolicyID"))
                loadGrid(gvPremiums, dt)
            End If
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlPremiums, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub


    Private Function SelectedInsuree() As Integer
        Return gvInsurees.SelectedDataKey(0)
    End Function
    Private Sub gvPolicies_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPolicies.SelectedIndexChanged
        Dim reload As New IMIS_BI.OverviewFamilyBI
        gvPremiums.DataSource = reload.GetPremiumsByPolicy(gvPolicies.SelectedDataKey.Values("PolicyID"))
        gvPremiums.SelectedIndex = 0
        gvPremiums.DataBind()
        DisableEmptyGridEditDeleteButtons(gvPremiums)
        If gvPolicies.SelectedIndex >= 0 Then
            hfPolicyValue.Value = gvPolicies.Rows(gvPolicies.SelectedIndex).Cells(7).Text
        End If
    End Sub
    Private Sub AddFamily_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AddFamily.Click
        Response.Redirect("Family.aspx")
    End Sub
    Private Sub EditFamily_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EditFamily.Click
        Response.Redirect("ChangeFamily.aspx?f=" & FamilyId)
    End Sub
    Private Sub DeleteFamily_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles DeleteFamily.Click
        RunPageSecurity(True, "deletefamily")
        Try
            Dim eFamily As New IMIS_EN.tblFamilies
            Dim eInsuree As New IMIS_EN.tblInsuree
            Dim chk As Integer = 0

            Dim dt As DataTable = DirectCast(Session("User"), DataTable)

            eFamily.FamilyID = FamilyId
            eFamily.AuditUserID = dt.Rows(0)("UserID")

            eInsuree.AuditUserID = dt.Rows(0)("UserID")
            If Not gvInsurees.SelectedDataKey Is Nothing Then
                eInsuree.InsureeID = CInt(gvInsurees.SelectedDataKey.Values("InsureeID"))
            End If

            eFamily.tblInsuree = eInsuree

            chk = OverviewFamily.DeleteFamily(eFamily)

            If chk = 0 Then
                imisgen.Alert(imisgen.getMessage("M_FAMILYNOTDELETED"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            ElseIf chk = 1 And eFamily.FamilyID > 0 Then
                Session("msg") = imisgen.getMessage("M_FAMILYANDHEADOFFLY_") & " " & imisgen.getMessage("M_DELETED")
                Response.Redirect("FindFamily.aspx?f=" & FamilyId)
            ElseIf chk = 2 Then
                Session("msg") = imisgen.getMessage("M_FAMILYSTILLHASDEPENDANTS")
            End If


        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlPremiums, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
        Response.Redirect("OverviewFamily.aspx?f=" & FamilyId & "&i=" & eInsuree.InsureeID & "&po=" & ePolicy.PolicyID & "&p=" & ePremium.PremiumId)
    End Sub
    Private Sub AddInsuree_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddInsuree.Click
        Response.Redirect("Insuree.aspx?f=" & FamilyId)
    End Sub
    Private Sub EditInsuree_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EditInsuree.Click
        Response.Redirect("Insuree.aspx?f=" & FamilyId & "&i=" & gvInsurees.SelectedDataKey.Value)
    End Sub
    Private Sub DeleteInsuree_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles DeleteInsuree.Click
        RunPageSecurity(True, "deleteinsuree")

        Try
            Dim eInsuree As New IMIS_EN.tblInsuree
            Dim chk As Integer = 0

            Dim dt As DataTable = DirectCast(Session("User"), DataTable)
            If Not gvInsurees.SelectedDataKey Is Nothing Then
                eInsuree.InsureeID = CInt(gvInsurees.SelectedDataKey.Values("InsureeID"))
            End If

            eInsuree.AuditUserID = dt.Rows(0)("UserID")

            chk = OverviewFamily.DeleteInsuree(eInsuree)

            If chk = 0 Then
                imisgen.Alert(imisgen.getMessage("M_INSUREENOTDELETED"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            ElseIf chk = 1 And eInsuree.InsureeID > 0 Then
                Session("msg") = imisgen.getMessage("L_INSUREE") & " " & imisgen.getMessage("M_DELETED")
            ElseIf chk = 2 Then
                Session("msg") = imisgen.getMessage("M_INSUREEHEADFMLYDELETENOT")
            End If


        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlPremiums, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
        Response.Redirect("OverviewFamily.aspx?f=" & FamilyId & "&i=" & eInsuree.InsureeID & "&po=" & ePolicy.PolicyID & "&p=" & ePremium.PremiumId)
    End Sub
    Private Sub AddPolicy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddPolicy.Click
        Response.Redirect("Policy.aspx?f=" & FamilyId & "&po=0&stage=N")
    End Sub

    Private Sub btnRenewPolicy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRenewPolicy.Click
        Dim pd As Integer = 0
        Dim ed As String = ""
        Dim rpo As Integer = 0
        If Not gvPolicies.SelectedDataKey Is Nothing Then
            pd = gvPolicies.SelectedDataKey.Values("ProdID")
            ed = gvPolicies.SelectedDataKey.Values("ExpiryDate")
            rpo = gvPolicies.SelectedDataKey.Values("PolicyID")
        End If

        Response.Redirect("Policy.aspx?f=" & FamilyId & "&po=0&stage=R&pd=" & pd & "&ed=" & ed & "&rpo=" & rpo)
    End Sub

    Private Sub EditPolicy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EditPolicy.Click
        Dim po As Integer = 0
        If Not gvPolicies.SelectedDataKey Is Nothing Then
            po = gvPolicies.SelectedDataKey.Values("PolicyID")
        End If

        Response.Redirect("Policy.aspx?f=" & FamilyId & "&po=" & po & "&stage=")
    End Sub
    Private Sub DeletePolicy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles DeletePolicy.Click

        RunPageSecurity(True, "deletepolicy")
        Try
            Dim epolicy As New IMIS_EN.tblPolicy
            Dim chk As Integer = 0

            Dim dt As DataTable = DirectCast(Session("User"), DataTable)
            If Not gvPolicies.SelectedDataKey Is Nothing Then
                epolicy.PolicyID = CInt(gvPolicies.SelectedDataKey.Values("PolicyID"))
            End If

            epolicy.AuditUserID = dt.Rows(0)("UserID")

            chk = OverviewFamily.DeletePolicy(epolicy)

            If chk = 0 Then
                imisgen.Alert(imisgen.getMessage("M_POLICYNOTDELETED"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            ElseIf chk = 1 And epolicy.PolicyID > 0 Then
                Session("msg") = imisgen.getMessage("L_POLICY") & " " & imisgen.getMessage("M_DELETED")
            ElseIf chk = 2 Then
                Session("msg") = imisgen.getMessage("M_POLICYHASPREMIUMNODELETE")
            End If


        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlPremiums, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
        Response.Redirect("OverviewFamily.aspx?f=" & FamilyId & "&i=" & eInsuree.InsureeID & "&po=" & ePolicy.PolicyID & "&p=" & ePremium.PremiumId)
    End Sub
    Private Sub AddPremium_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles AddPremium.Click
        Dim po As Integer = 0
        If Not gvPolicies.SelectedDataKey Is Nothing Then
            po = gvPolicies.SelectedDataKey.Values("PolicyID")
        End If
        Response.Redirect("Premium.aspx?f=" & FamilyId & "&po=" & po)
    End Sub
    Private Sub EditPremium_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles EditPremium.Click
        Dim po As Integer = 0
        If Not gvPolicies.SelectedDataKey Is Nothing Then
            po = gvPolicies.SelectedDataKey.Values("PolicyID")
        End If
        Dim p As Integer = 0
        If Not gvPremiums.SelectedDataKey Is Nothing Then
            p = gvPremiums.SelectedDataKey.Value
        End If
        Response.Redirect("Premium.aspx?f=" & FamilyId & "&p=" & p & "&po=" & po)
    End Sub
    Private Sub DeletePremium_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles DeletePremium.Click
        RunPageSecurity(True, "deletepremium")
        Try
            Dim epremium As New IMIS_EN.tblPremium
            Dim chk As Integer = 0

            Dim dt As DataTable = DirectCast(Session("User"), DataTable)
            If Not gvPremiums.SelectedDataKey Is Nothing Then
                epremium.PremiumId = CInt(gvPremiums.SelectedDataKey.Values("PremiumID"))
            End If

            epremium.AuditUserID = dt.Rows(0)("UserID")

            chk = OverviewFamily.DeletePremium(epremium)

            If chk = 0 Then
                imisgen.Alert(imisgen.getMessage("M_PREMIUMNOTDELETED"), pnlButtons, alertPopupTitle:="IMIS")
                Return
            ElseIf chk = 1 And epremium.PremiumId > 0 Then
                Session("msg") = imisgen.getMessage("L_PREMIUM") & " " & imisgen.getMessage("M_DELETED")
            ElseIf chk = 2 Then
                Session("msg") = imisgen.getMessage("M_PREMIUMISINUSE")
            End If


        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlPremiums, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            Return
        End Try
        Response.Redirect("OverviewFamily.aspx?f=" & FamilyId & "&i=" & eInsuree.InsureeID & "&po=" & ePolicy.PolicyID & "&p=" & ePremium.PremiumId)
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click

        
        Response.Redirect(Session("parentUrl"))

    End Sub
End Class
