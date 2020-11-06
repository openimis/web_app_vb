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

Public Class AddFunding
    Inherits System.Web.UI.Page
    Private userBI As New IMIS_BI.UserBI
    Protected imisgen As New IMIS_Gen
    Private FundBI As New IMIS_BI.AddFundingBI
    Private ePremium As New IMIS_EN.tblPremium
    Private ePayer As New IMIS_EN.tblPayer


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If IsPostBack Then Return

            FillDropDowns()
        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex, 1)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.ToString(), EventLogEntryType.Error, 1)
            Return
        End Try
    End Sub


    Private Sub FillDropDowns()
        FillRegions()
        FillDistricts()
        FillProducts()
        FillPayers()
    End Sub
    Private Sub FillRegions()
        Dim dtRegions As DataTable = FundBI.GetRegions(imisgen.getUserId(Session("User")), True)
        ddlRegion.DataSource = dtRegions
        ddlRegion.DataValueField = "RegionId"
        ddlRegion.DataTextField = "RegionName"
        ddlRegion.DataBind()
        If dtRegions.Rows.Count = 1 Then
            FillDistricts()
            FillProducts()
        End If
    End Sub
    Private Sub FillDistricts()
        ddlDistrict.DataSource = FundBI.GetDistricts(imisgen.getUserId(Session("User")), True, ddlRegion.SelectedValue)
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()
    End Sub
    Private Sub FillProducts()
        Dim LocationId As Integer
        If Val(ddlDistrict.SelectedValue = 0) Then
            LocationId = If(Val(ddlRegion.SelectedValue) = 0, -1, Val(ddlRegion.SelectedValue))
        Else
            LocationId = Val(ddlDistrict.SelectedValue)
        End If
        'imisgen.getUserId(Session("User"))
        ddlProduct.DataSource = FundBI.GetProducts(LocationId, imisgen.getUserId(Session("User")), True)
        ddlProduct.DataValueField = "ProdId"
        ddlProduct.DataTextField = "ProductCode"
        ddlProduct.DataBind()
    End Sub
    Private Sub FillPayers()
        Dim LocationId As Integer
        If Val(ddlDistrict.SelectedValue = 0) Then
            LocationId = If(Val(ddlRegion.SelectedValue) = 0, -1, Val(ddlRegion.SelectedValue))
        Else
            LocationId = Val(ddlDistrict.SelectedValue)
        End If
        ddlPayer.DataSource = FundBI.GetPayers(Val(ddlRegion.SelectedValue), Val(ddlDistrict.SelectedValue), imisgen.getUserId(Session("User")), True)
        ddlPayer.DataValueField = "PayerID"
        ddlPayer.DataTextField = "PayerName"
        ddlPayer.DataBind()
    End Sub
    Private Sub RunPageSecurity()
        Dim RoleID As Integer = imisgen.getRoleId(Session("User"))
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If userBI.RunPageSecurity(IMIS_EN.Enums.Pages.Funding, Page) Then
            B_SAVE.Visible = userBI.checkRights(IMIS_EN.Enums.Rights.FundingSave, UserID)
            If Not B_SAVE.Visible Then
                pnlBody.Enabled = False
            End If
        Else
            Server.Transfer("Home.aspx")
        End If
    End Sub

    Private Sub ddlDistrict_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDistrict.SelectedIndexChanged
        FillProducts()
        FillPayers()
    End Sub
    Private Sub SetEntity()
        ePremium = New IMIS_EN.tblPremium
        With ePremium
            .PremiumId = 0
            .isOffline = IMIS_Gen.offlineHF Or IMIS_Gen.OfflineCHF
            If ddlPayer.SelectedValue > 0 Then ePayer.PayerID = ddlPayer.SelectedValue
            .Amount = txtPremiumPaid.Text
            .Receipt = txtReceiptNumber.Text
            .PayDate = Date.ParseExact(txtPaymentDate.Text, "dd/MM/yyyy", Nothing)
            .PayType = "F"
            .AuditUserID = imisgen.getUserId(Session("User"))


            .tblPayer = ePayer
        End With
    End Sub
    Private Sub B_SAVE_Click(sender As Object, e As EventArgs) Handles B_SAVE.Click
        Try
            Dim msg As String = String.Empty
            If Not String.IsNullOrWhiteSpace(ddlProduct.Text) Then

                SetEntity()
                Dim Result As Integer = FundBI.AddFund(ePremium, ddlProduct.SelectedValue)
                Select Case Result
                    Case 0
                        msg = imisgen.getMessage("M_FUNDADDED")
                    Case 99
                        msg = imisgen.getMessage("M_AJAXERROR")
                End Select

                Session("msg") = msg
                Response.Redirect("Home.aspx")
                'Added by Emmanuel
            Else
                imisgen.Alert(imisgen.getMessage("M_MUSTFILLPRODUCT"), pnlBody, alertPopupTitle:="IMIS")
            End If

            'imisgen.Alert(msg, pnlBody, alertPopupTitle:="IMIS-Funding"    Commeted by developer initially

        Catch ex As Exception
            imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE"), pnlBody, alertPopupTitle:="IMIS")
            imisgen.Log(Page.Title & " : " & imisgen.getLoginName(Session("User")), ex, 1)
            'EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.ToString(), EventLogEntryType.Error, 1)
            Return
        End Try
    End Sub

    Private Sub B_CANCEL_Click(sender As Object, e As EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        FillDistricts()
        FillProducts()
        FillPayers()
    End Sub
End Class
