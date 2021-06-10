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

Public Partial Class PremiumCollection_
    Inherits System.Web.UI.Page

    Private PC As New IMIS_BI.PremiumCollectionBI
    Private imisgen As New IMIS_Gen

    Private Sub PremiumCollection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Return


        ddlDistrict.DataSource = PC.GetDistricts(imisgen.getUserId(Session("User")), True)
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()


        ddlPaymentType.DataSource = PC.GetTypeOfPayment(True)
        ddlPaymentType.DataTextField = "PayType"
        ddlPaymentType.DataValueField = "Code"
        ddlPaymentType.DataBind()


        FillProducts()

    End Sub

    Private Sub FillProducts()
        ddlProduct.DataSource = PC.GetProducts(imisgen.getUserId(Session("User")), True, Val(ddlDistrict.SelectedValue))
        ddlProduct.DataValueField = "ProdId"
        ddlProduct.DataTextField = "ProductCode"
        ddlProduct.DataBind()
    End Sub

    Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim RangeFrom As DateTime
        Dim RangeTo As DateTime

        If IsDate(Date.ParseExact(txtFromDate.Text.Trim, "dd/MM/yyyy", Nothing)) Then
            RangeFrom = Date.ParseExact(txtFromDate.Text.Trim, "dd/MM/yyyy", Nothing)
        End If

        If IsDate(Date.ParseExact(txtToDate.Text.Trim, "dd/MM/yyyy", Nothing)) Then
            RangeTo = Date.ParseExact(txtToDate.Text.Trim, "dd/MM/yyyy", Nothing)
        End If

        Dim sSubTitle As String = "Date from " & txtFromDate.Text.Trim & " To " & txtToDate.Text.Trim

        Dim PaymentType As String = ""
        Select Case ddlPaymentType.SelectedValue
            Case ""
                PaymentType = ""
            Case "C"
                PaymentType = "Cash"
            Case "B"
                PaymentType = "Bank"
            Case "M"
                PaymentType = "Mobile"
        End Select

        If Len(Trim(ddlPaymentType.SelectedValue)) > 0 Or ddlDistrict.SelectedValue > 0 Or ddlProduct.SelectedValue > 0 Then
            sSubTitle = sSubTitle & " Filter("
            sSubTitle = sSubTitle & if(Len(Trim(PaymentType)) = 0, "", "Payment Type:" & PaymentType)
            sSubTitle = sSubTitle & if(ddlDistrict.SelectedValue = 0, "", " District:" & ddlDistrict.SelectedItem.Text)
            sSubTitle = sSubTitle & if(ddlProduct.SelectedValue = 0, "", " Product:" & ddlProduct.SelectedItem.Text)
            sSubTitle = sSubTitle & ")"
        End If

        IMIS_EN.eReports.SubTitle = sSubTitle

        Dim dt As New DataTable

        If Request.QueryString("t") = "pc" Then
            'Premium Collection
            dt = PC.GetPremiumCollection(ddlDistrict.SelectedValue, ddlProduct.SelectedValue, ddlPaymentType.SelectedValue, RangeFrom, RangeTo)
        Else
            'Policy Sold
            dt = PC.GetPolicySold(ddlDistrict.SelectedValue, ddlProduct.SelectedValue, RangeFrom, RangeTo)
        End If

        Session("Report") = dt

        Response.Redirect("Report.aspx?r=" & Request.QueryString("t") & "")



    End Sub

    Protected Sub ddlDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDistrict.SelectedIndexChanged
        FillProducts()
    End Sub
End Class
