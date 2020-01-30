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

Public Partial Class PremiumDistribution
    Inherits System.Web.UI.Page
    Private pd As New IMIS_BI.PremiumDistributionBI
    Private imisgen As New IMIS_Gen

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Return

        Dim dtDistricts As DataTable = pd.GetDistricts(imisgen.getUserId(Session("User")), True)
        ddlDistrict.DataSource = dtDistricts
        ddlDistrict.DataValueField = "DistrictId"
        ddlDistrict.DataTextField = "DistrictName"
        ddlDistrict.DataBind()


        Dim dtMonth, dtYear As DataTable
        Dim dt As Date = Today

        dtMonth = pd.GetMonths(1, 12)
        dtYear = pd.GetYears(2010, Year(Now()) + 5)

        ddlMonth.DataSource = dtMonth
        ddlMonth.DataValueField = "MonthNum"
        ddlMonth.DataTextField = "MonthName"
        ddlMonth.DataBind()
        ddlMonth.SelectedValue = Month(Now())

        ddlYear.DataSource = dtYear
        ddlYear.DataValueField = "Year"
        ddlYear.DataTextField = "Year"
        ddlYear.DataBind()
        ddlYear.SelectedValue = Year(Now())


        FillProducts()
    End Sub


    Private Sub FillProducts()
        ddlProduct.DataSource = pd.GetProducts(imisgen.getUserId(Session("User")), True, ddlDistrict.SelectedValue)
        ddlProduct.DataValueField = "ProdId"
        ddlProduct.DataTextField = "ProductCode"
        ddlProduct.DataBind()
    End Sub



    Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click

        Dim sSubTitle As String = "Month: " & ddlMonth.SelectedItem.Text & " And Year: " & ddlYear.SelectedItem.Text

        If ddlDistrict.SelectedValue > 0 Or ddlProduct.SelectedValue > 0 Then
            sSubTitle = sSubTitle & " Filter("
            sSubTitle = sSubTitle & if(ddlDistrict.SelectedValue = 0, "", " District:" & ddlDistrict.SelectedItem.Text)
            sSubTitle = sSubTitle & if(ddlProduct.SelectedValue = 0, "", " Product:" & ddlProduct.SelectedItem.Text)
            sSubTitle = sSubTitle & ")"
        End If

        IMIS_EN.eReports.SubTitle = sSubTitle

        Dim dt As New DataTable

        dt = pd.GetPremiumDistribution(ddlDistrict.SelectedValue, ddlProduct.SelectedValue, ddlMonth.SelectedValue, ddlYear.SelectedValue)

        Session("Report") = dt

        Response.Redirect("Report.aspx?r=pd")


    End Sub

    Protected Sub ddlDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlDistrict.SelectedIndexChanged
        FillProducts()
    End Sub

End Class
