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

Partial Public Class Payer
    Inherits System.Web.UI.Page
    Dim Payer As New IMIS_BI.PayerBI
    Dim epayer As New IMIS_EN.tblPayer
    Private imisgen As New IMIS_Gen
    Private userBI As New IMIS_BI.UserBI

    Private Sub RunPageSecurity()
        'Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        Dim RefUrl = if(Request.Headers("Referer") Is Nothing, String.Empty, Request.Headers("Referer"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Payer, Page) Then
            Dim Add As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.AddPayer, UserID)
            Dim Edit As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.EditPayer, UserID)
            Dim View As Boolean = userBI.checkRights(IMIS_EN.Enums.Rights.FindPayer, UserID)

            Dim reg As New Regex("OverviewFamily", RegexOptions.IgnoreCase)
            If reg.IsMatch(RefUrl) Then
                If View Or Add Or Edit Then
                    Panel2.Enabled = False
                    B_SAVE.Visible = False
                Else
                    Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Payer.ToString & "&retUrl=" & RefUrl)
                End If
            ElseIf Not Add And Not Edit Then
                Panel2.Enabled = False
                B_SAVE.Visible = False
            End If
        Else

            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.Payer.ToString & "&retUrl=" & RefUrl)
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()
        Try
            lblMsg.Text = ""
            epayer.PayerID = HttpContext.Current.Request.QueryString("p")

            If IsPostBack = True Then Return

            Dim dtRegions As DataTable = Payer.GetRegions(imisgen.getUserId(Session("User")), True)
            ddlRegion.DataSource = dtRegions
            ddlRegion.DataValueField = "RegionId"
            ddlRegion.DataTextField = "RegionName"
            ddlRegion.DataBind()

            If dtRegions.Rows.Count = 1 Then
                FillDistricts()
            End If

            ddlTypeOfPayer.DataSource = Payer.GetPayerType(True)
            ddlTypeOfPayer.DataValueField = "Code"
            ddlTypeOfPayer.DataTextField = "PayerType"
            ddlTypeOfPayer.DataBind()

            If Not epayer.PayerID = 0 Then
                Payer.LoadPayer(epayer)
                ddlTypeOfPayer.SelectedValue = epayer.PayerType
                txtNameOfPayer.Text = epayer.PayerName
                txtAddress.Text = epayer.PayerAddress

                If Not epayer.tblLocations Is Nothing Then
                    ddlRegion.SelectedValue = epayer.tblLocations.RegionId
                    FillDistricts()
                    ddlDistrict.SelectedValue = epayer.tblLocations.LocationId

                End If

                txtPhone.Text = epayer.Phone
                txtFax.Text = epayer.Fax
                txtEmail.Text = epayer.eMail

            End If
            If HttpContext.Current.Request.QueryString("r") = 1 Or epayer.ValidityTo.HasValue Then
                Panel2.Enabled = False
                B_SAVE.Visible = False
            End If

            'Dim RefUrl = Request.Headers("Referer")
            'Dim reg As New Regex("OverViewFamily", RegexOptions.IgnoreCase)
            If Request.QueryString("x") = 1 Then
                Panel2.Enabled = False
                B_SAVE.Visible = False
                B_CANCEL.Visible = False
                CType(Me.Master.FindControl("pnlDisablePage"), Panel).Attributes.Add("style", "display:block")
            End If
        Catch ex As Exception
            'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
            EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
        End Try

    End Sub
    Private Sub FillDistricts()
        ddlDistrict.DataSource = Payer.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataValueField = "DistrictID"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub B_SAVE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_SAVE.Click
        If CType(Me.Master.FindControl("hfDirty"), HiddenField).Value = True Then
            Try

                Dim eLocations As New IMIS_EN.tblLocations
                If Val(ddlDistrict.SelectedValue) <> 0 Then
                    eLocations.LocationId = Val(ddlDistrict.SelectedValue)
                Else
                    eLocations.LocationId = Val(ddlRegion.SelectedValue)
                End If

                epayer.eMail = txtEmail.Text
                epayer.Fax = txtFax.Text
                epayer.PayerAddress = txtAddress.Text
                epayer.PayerName = txtNameOfPayer.Text
                epayer.Phone = txtPhone.Text
                epayer.PayerType = ddlTypeOfPayer.Text
                epayer.tblLocations = eLocations
                epayer.AuditUserID = imisgen.getUserId(Session("User"))

                Dim chk As Integer = Payer.SavePayer(epayer)
                If chk = 0 Then
                    Session("msg") = epayer.PayerName & imisgen.getMessage("M_Inserted")

                ElseIf chk = 1 Then
                    imisgen.Alert(epayer.PayerName & imisgen.getMessage("M_Exists"), pnlButtons, alertPopupTitle:="IMIS")
                    Return

                Else
                    Session("msg") = epayer.PayerName & imisgen.getMessage("M_Updated")



                End If

            Catch ex As Exception
                'lblMsg.Text = imisgen.getMessage("M_ERRORMESSAGE")
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlButtons, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
                Return
            End Try
        End If
        Response.Redirect("FindPayer.aspx?p=" & txtNameOfPayer.Text)
    End Sub
    Private Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles B_CANCEL.Click
        Response.Redirect("FindPayer.aspx?p=" & txtNameOfPayer.Text)
    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
    End Sub
End Class
