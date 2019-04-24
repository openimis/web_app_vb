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

Partial Public Class FindOfficer
    Inherits System.Web.UI.Page
    Private eofficer As New IMIS_EN.tblOfficer
    Private Officer As New IMIS_BI.FindOfficersBI
    Protected imisGen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI
    Private BIClaimAdmin As New IMIS_BI.ClaimAdministratorBI

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddRowSelectToGridView(gvOfficers)
        If chkLegacy.Checked Then Page.ClientScript.RegisterForEventValidation(B_EDIT.UniqueID)
        MyBase.Render(writer)
    End Sub
    Private Sub AddRowSelectToGridView(ByVal gv As GridView)
        For Each row As GridViewRow In gv.Rows
            If Not row.Cells(10).Text = "&nbsp;" Then
                row.Style.Value = "color:#000080;font-style:italic;text-decoration:line-through"
            End If
            'row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(gv, "Select$" + row.RowIndex.ToString(), True))
        Next
    End Sub

    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RunPageSecurity()
        Try
            If IsPostBack = True Then
                If Request.Params.Get("__EVENTARGUMENT").ToString = "Delete" Then
                    B_DELETE_Click(sender, e)
                End If
            End If

            If Not IsPostBack = True Then
                Dim dtRegions As DataTable = Officer.GetRegions(imisGen.getUserId(Session("User")), True)
                ddlRegion.DataSource = dtRegions
                ddlRegion.DataValueField = "RegionId"
                ddlRegion.DataTextField = "RegionName"
                ddlRegion.DataBind()
                If dtRegions.Rows.Count = 1 Then
                    FillDistrict()
                End If
                loadGrid()
            End If


        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub
    Private Sub FillDistrict()
        ddlDistrict.DataSource = Officer.GetDistricts(imisGen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub

    Private Sub RunPageSecurity(Optional ByVal ondelete As Boolean = False)
        Dim RefUrl = Request.Headers("Referer")
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        If Not ondelete Then
            If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.FindOfficer, Page) Then
                B_ADD.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.AddOfficer, RoleID)
                B_EDIT.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.EditOfficer, RoleID)
                B_DELETE.Visible = userBI.CheckRoles(IMIS_EN.Enums.Rights.DeleteOfficer, RoleID)

                If Not B_EDIT.Visible And Not B_DELETE.Visible Then
                    pnlGrid.Enabled = False
                End If
            Else
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindOfficer.ToString & "&retUrl=" & RefUrl)
            End If
        Else
            If Not userBI.CheckRoles(IMIS_EN.Enums.Rights.DeleteOfficer, RoleID) Then
                Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.FindOfficer.ToString & "&retUrl=" & RefUrl)
            End If

        End If
    End Sub
   
    Private Sub loadGrid() Handles B_SEARCH.Click, chkLegacy.CheckedChanged, gvOfficers.PageIndexChanged
        Try
            lblMsg.Text = ""
            eofficer.Code = txtCode.Text
            eofficer.LastName = txtLastName.Text
            If txtDOBFROM.Text.Length > 0 Then
                If IsDate(txtDOBFROM.Text) Then
                    eofficer.DOBFrom = Date.Parse(txtDOBFROM.Text)
                Else
                    imisGen.Alert(imisGen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If
            End If
           
            If txtDOBTo.Text.Length > 0 Then
                If IsDate(txtDOBTo.Text) Then
                    eofficer.DOBTo = Date.Parse(txtDOBTo.Text)
                Else
                    imisGen.Alert(imisGen.getMessage("M_INVALIDDATE"), pnlButtons, alertPopupTitle:="IMIS")
                    Return
                End If
            End If
          

            eofficer.Phone = txtPhone.Text

            Dim eLocations As New IMIS_EN.tblLocations
            Dim RegionId As Integer?
            Dim DistrictId As Integer?

            If ddlRegion.SelectedValue = "" OrElse ddlRegion.SelectedValue = 0 Then
                RegionId = 0
            ElseIf ddlRegion.SelectedValue = -1 Then
                RegionId = Nothing
            Else
                RegionId = ddlRegion.SelectedValue
            End If

            If ddlDistrict.SelectedValue = "" OrElse ddlDistrict.SelectedValue = 0 Then
                DistrictId = 0
            Else
                DistrictId = ddlDistrict.SelectedValue
            End If

            eLocations.RegionId = RegionId
            eLocations.DistrictID = DistrictId

            eofficer.tblLocations = eLocations
            eofficer.OtherNames = txtOtherNames.Text
            eofficer.AuditUserID = imisGen.getUserId(Session("User"))
            eofficer.EmailId = txtEmail.Text
            getGridData()

        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub
    Private Sub getGridData()
        Dim dtOfficers As DataTable = Officer.GetOfficers(eofficer, chkLegacy.Checked)
        Dim sindex As Integer = 0
        Dim dv As DataView = dtOfficers.DefaultView
        If Not IsPostBack = True Then
            If Not HttpContext.Current.Request.QueryString("o") Is Nothing Then
                eofficer.Code = HttpContext.Current.Request.QueryString("o")
            Else
                eofficer.Code = hfOfficerCode.Value
            End If
            If Not eofficer.Code = "" Then
                dv.Sort = "Code"
                Dim x As Integer = dv.Find(eofficer.Code)
                If x >= 0 Then
                    gvOfficers.PageIndex = Int(x / 15)
                    Math.DivRem(x, 15, sindex)
                End If
            End If
        End If
        L_FOUNDUSERS.Text = If(dv.ToTable.Rows.Count = 0, imisGen.getMessage("L_NO"), Format(dv.ToTable.Rows.Count, "#,###")) & " " & imisGen.getMessage("L_FOUNDOFFICERS", False)
        gvOfficers.DataSource = dv
        gvOfficers.SelectedIndex = sindex
        gvOfficers.DataBind()
        EnableButtons(gvOfficers.Rows.Count)
    End Sub

    Protected Sub B_ADD_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_ADD.Click

        Response.Redirect("Officer.aspx?o=0")
       



    End Sub
  
    Protected Sub B_EDIT_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_EDIT.Click
        Response.Redirect("Officer.aspx?o=" & hfOfficerId.Value)

        

    End Sub


    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub

    Private Sub B_DELETE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_DELETE.Click
        RunPageSecurity(True)
        Try
            lblMsg.Text = ""
            eofficer.OfficerID = hfOfficerId.Value
            eofficer.Code = hfOfficerCode.Value
            eofficer.AuditUserID = imisGen.getUserId(Session("User"))
            If hfHasLogin.Value = "True" Then
                DeleteAssociatedUser()
            End If
            Officer.DeleteOfficer(eofficer)
            Dim FOfficer As String = hfOfficerCode.Value
            loadGrid()
            lblMsg.Text = FOfficer & " " & imisGen.getMessage("M_DELETED")
        Catch ex As Exception
            'lblMsg.Text = imisGen.getMessage("M_ERRORMESSAGE")
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub DeleteAssociatedUser()
        Try
            eofficer.eUsers = New IMIS_EN.tblUsers
            eofficer.eUsers.LoginName = eofficer.Code
            BIClaimAdmin.LoadUsers(eofficer.eUsers)
            BIClaimAdmin.DeleteUser(eofficer.eUsers)
        Catch ex As Exception
            imisGen.Alert(imisGen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisGen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try
    End Sub
    Private Sub EnableButtons(ByVal rows As Integer)
        If rows = 0 Then

            B_DELETE.Visible = False
            B_EDIT.Visible = False
            'B_VIEW.Visible = False
            B_ADD.Visible = True
        Else
            If chkLegacy.Checked = True Then
                B_DELETE.Visible = False
                B_EDIT.Visible = False
                'B_VIEW.Visible = True
                B_ADD.Visible = False
            Else
                B_DELETE.Visible = True
                B_EDIT.Visible = True
                'B_VIEW.Visible = False
                B_ADD.Visible = True
            End If

        End If
    End Sub

    Private Sub gvOfficers_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvOfficers.PageIndexChanging
        gvOfficers.PageIndex = e.NewPageIndex
    End Sub

    'Private Sub B_VIEW_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_VIEW.Click
    '    Response.Redirect("Officer.aspx?o=" & hfOfficerId.Value & "&r=1")

    'End Sub

    'private sub gvofficers_rowdatabound(byval sender as object, byval e as system.web.ui.webcontrols.gridviewroweventargs) handles gvofficers.rowdatabound
    '    if e.row.rowtype = datacontrolrowtype.datarow then
    '        e.row.attributes.add("onmouseover", "this.previous_color=this.classname;this.classname='hover'")
    '        e.row.attributes.add("onmouseout", "this.classname=this.previous_color;")
    '        e.row.attributes.add("onclick", "javascript:changeclass('" & e.row.clientid & "'," & e.row.rowindex & ");this.previous_color=this.classname")
    '    end if
    'end sub

   
    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistrict()
    End Sub
End Class
