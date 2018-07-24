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

Public Class ProductsBL
    Private imisgen As New GeneralBL
    Public Function GetProducts(ByVal eProducts As IMIS_EN.tblProduct, Optional ByVal All As Boolean = True) As DataTable
        Dim getDataTable As New IMIS_DAL.ProductsDAL
        eProducts.ProductCode += "%"
        eProducts.ProductName += "%"
        If eProducts.DateFrom = Nothing Then eProducts.DateFrom = System.Data.SqlTypes.SqlDateTime.MinValue.Value
        If eProducts.DateTo = Nothing Then eProducts.DateTo = System.Data.SqlTypes.SqlDateTime.MaxValue.Value
        
        Return getDataTable.GetProducts(eProducts, All)

    End Function
    Public Function GetProducts(ByVal UserId As Integer, ByVal ShowSelect As Boolean, ByVal RegionId As Integer, ByVal DistrictID As Integer, Optional ByVal ByDate As Date? = Nothing) As DataTable
        Dim getDataTable As New IMIS_DAL.ProductsDAL
        Dim dt As DataTable = getDataTable.GetProducts(UserId, RegionId, DistrictID, ByDate)
        If ShowSelect = True Then
            'If dt.Rows.Count > 1 Then
            If ShowSelect = True Then
                Dim dr As DataRow = dt.NewRow
                dr("ProdId") = 0
                dr("ProductCode") = imisgen.getMessage("T_SELECTPRODUCT")
                dt.Rows.InsertAt(dr, 0)
            End If
            'End If
        End If
        Return dt
    End Function
    Public Function GetProductsStict(LocationId As Integer, ByVal UserId As Integer, ByVal ShowSelect As Boolean) As DataTable
        Dim getDataTable As New IMIS_DAL.ProductsDAL
        Dim dt As DataTable = getDataTable.GetProductsStict(LocationId, UserId)
        If ShowSelect = True Then
            If dt.Rows.Count > 1 Then
                If ShowSelect = True Then
                    Dim dr As DataRow = dt.NewRow
                    dr("ProdId") = 0
                    dr("ProductCode") = imisgen.getMessage("T_SELECTPRODUCT")
                    dt.Rows.InsertAt(dr, 0)
                End If
            End If
        End If
        Return dt
    End Function
    Public Function SaveProducts(ByRef eProducts As IMIS_EN.tblProduct) As Integer
        Dim SaveData As New IMIS_DAL.ProductsDAL
        Dim dt As DataTable = SaveData.CheckIfProductExists(eProducts)
        If dt.Rows.Count > 0 Then Return 2
        If eProducts.ProdID = 0 Then
            SaveData.InsertProduct(eProducts)
            Return 0
        Else
            SaveData.UpdateProduct(eProducts)
            Return 1
        End If
    End Function
    Public Function GetPeriod(Optional ByVal showSelect As Boolean = False) As DataTable

        Dim dtbl As New DataTable
        Dim dr As DataRow
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("DistType")
        If showSelect = True Then
            dr = dtbl.NewRow
            dr("Code") = 0
            dr("DistType") = imisgen.getMessage("L_NONE")
            dtbl.Rows.Add(dr)
        End If
        dr = dtbl.NewRow
        dr("Code") = "M"
        dr("DistType") = imisgen.getMessage("L_MONTHLY")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = "Q"
        dr("DistType") = imisgen.getMessage("L_QUARTERLY")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = "Y"
        dr("DistType") = imisgen.getMessage("L_YEARLY")
        dtbl.Rows.Add(dr)




        Return dtbl
    End Function
    Public Sub LoadProduct(ByRef eProducts As IMIS_EN.tblProduct)
        Dim load As New IMIS_DAL.ProductsDAL
        load.LoadProduct(eProducts)
    End Sub
    Public Function GetDistribution(ByVal ProdId As Integer, ByVal CareType As String, ByVal DistType As String) As DataTable
        Dim getDatatable As New IMIS_DAL.ProductsDAL
        Dim distInt As Integer
        Select Case DistType
            Case "M" : distInt = 12
            Case "Q" : distInt = 4
            Case "Y" : distInt = 1
        End Select
        Return getDatatable.GetDistribution(ProdId, CareType, distInt)
    End Function
    Public Function DeleteProduct(ByRef eProduct As IMIS_EN.tblProduct) As Boolean

        Dim ProductCheck As New IMIS_DAL.ProductsDAL

        If ProductCheck.CheckIfCanDelete(eProduct) = True Then
            ProductCheck.DeleteProduct(eProduct)
            Return True
        Else
            Return False
        End If
    End Function
    Public Function GetPeriodForPolicy(ByVal ProdId As Integer, ByVal EnrolDate As Date, Optional ByRef HasCycle As Boolean = False, Optional PolicyStage As String = "N") As DataTable
        Dim Product As New IMIS_DAL.ProductsDAL
        Return Product.GetPeriodForPolicy(ProdId, EnrolDate, HasCycle, PolicyStage)
    End Function
    Public Function GetRegistrationFee(ByVal PolicyId As Integer) As Double
        Dim Prod As New IMIS_DAL.ProductsDAL
        Return Prod.GetRegistrationFee(PolicyId)
    End Function
    Public Function GetDays(Optional ByVal ShowLabel As Boolean = False) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("DayD", GetType(String))
        dt.Columns.Add("DayT", GetType(String))
        Dim dr As DataRow = Nothing
        If ShowLabel Then
            dr = dt.NewRow
            dr("DayD") = imisgen.getMessage("T_DAY")
            dr("DayT") = 0
            dt.Rows.Add(dr)
        End If
        For i = 1 To 31
            dr = dt.NewRow
            dr("DayD") = i
            dr("DayT") = if(i < 10, "0" & i, i)
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function
    Public Function GetMonths(Optional ByVal ShowLabel As Boolean = False) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("MonthN", GetType(String))
        dt.Columns.Add("MonthT", GetType(String))
        Dim dr As DataRow = Nothing
        If ShowLabel Then
            dr = dt.NewRow
            dr("MonthN") = 0
            dr("MonthT") = imisgen.getMessage("T_MONTHS")
            dt.Rows.Add(dr)
        End If
        For i = 1 To 12
            dr = dt.NewRow
            dr("MonthN") = if(i < 10, "0" & i, i)
            dr("MonthT") = MonthName(i, True)
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function
    Public Function GetProductAdministrationPeriod(ByVal ProdId As Integer) As Integer
        Dim DAL As New IMIS_DAL.ProductsDAL
        Return DAL.GetProductAdministrationPeriod(ProdId)
    End Function
    Public Function GetCeilingInterpritation() As DataTable
        Dim Gen As New GeneralBL
        Dim DAL As New IMIS_DAL.CeilingInterpretationDAL
        Dim dt As DataTable = DAL.GetCeilingInterpretation
        Dim dr As DataRow
        dr = dt.NewRow
        dr("CeilingIntCode") = ""
        dr("CeilingIntDesc") = Gen.getMessage("M_SELECTCEILINGINT")
        dr("AltLanguage") = Gen.getMessage("M_SELECTCEILINGINT")

        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function GetProductName_Account(ProdId As Integer) As DataTable
        Dim Prod As New IMIS_DAL.ProductsDAL
        Return Prod.GetProductName_Account(ProdId)
    End Function
    Public Function GetAllProducts() As DataTable
        Dim DAL As New IMIS_DAL.ProductsDAL
        Dim dt As DataTable = DAL.GetAllProducts
        Dim dr As DataRow = dt.NewRow
        dr("ProdId") = 0
        dr("ProductCode") = imisgen.getMessage("T_SELECTPRODUCT")
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function getProductDetailMin(ProdId As Integer) As IMIS_EN.tblProduct
        Dim Product As New IMIS_DAL.ProductsDAL
        Return Product.getProductDetailMin(ProdId)
    End Function
    Public Function GetProductForRenewal(ProdId As Integer, EnrollDate As Date) As IMIS_EN.tblProduct
        Dim Prod As New IMIS_DAL.ProductsDAL
        Return Prod.GetProductForRenewal(ProdId, EnrollDate)
    End Function
    Public Function getProductCapitationDetails(ByVal ProductId As Integer) As DataTable
        Dim DAL As New IMIS_DAL.ProductsDAL
        Dim HF As New IMIS_BL.HealthFacilityBL
        Dim dt As DataTable = HF.GetHFLevel
        Return DAL.getProductCapitationDetails(ProductId, dt)
    End Function
End Class
