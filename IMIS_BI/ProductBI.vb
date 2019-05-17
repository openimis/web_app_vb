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

Public Class ProductBI

    Public Function SaveProduct(ByRef eProduct As IMIS_EN.tblProduct) As Integer
        Dim saveData As New IMIS_BL.ProductsBL
        Return saveData.SaveProducts(eProduct)
    End Function
   
    Public Sub ChangeProductItems(ByVal eProductItems As IMIS_EN.tblProductItems)
        Dim Insert As New IMIS_BL.ProductItemsBL
        Insert.ChangeProductItems(eProductItems)
    End Sub
    Public Sub ChangeProductServices(ByVal eProductServices As IMIS_EN.tblProductServices)
        Dim Insert As New IMIS_BL.ProductServicesBL
        Insert.ChangeProductServices(eProductServices)
    End Sub
    Public Function GetDistricts(ByVal userId As Integer, Optional ByVal showSelect As Boolean = False, Optional RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userId, showSelect, RegionId:=RegionId, EnforceSelect:=True)
    End Function
    Public Function GetProductItems(ByVal ProdId) As DataTable
        Dim getDataTable As New IMIS_BL.ProductItemsBL
        Return getDataTable.GetProductItems(ProdId)
    End Function
    Public Function GetMedicalServices(ByVal ProdID As Integer) As DataTable
        Dim getDataTable As New IMIS_BL.ProductServicesBL
        Return getDataTable.GetProductServices(ProdID)
    End Function
    Public Sub LoadProduct(ByRef eProducts As IMIS_EN.tblProduct)
        Dim loadEntity As New IMIS_BL.ProductsBL
        loadEntity.LoadProduct(eProducts)
    End Sub
    Public Function GetPeriod(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim getDatatable As New IMIS_BL.ProductsBL
        Return getDatatable.GetPeriod(showSelect)
    End Function
    Public Function GetProducts(ByVal UserId As Integer, Optional ByVal ShowSelect As Boolean = False, Optional ByVal RegionId As Integer = 0, Optional ByVal DistrictID As Integer = 0) As DataTable
        Dim getDatatable As New IMIS_BL.ProductsBL
        Return getDatatable.GetProducts(UserId, ShowSelect, RegionId, DistrictID)
    End Function
    Public Sub UpdateProductItems(ByVal eProductItems As IMIS_EN.tblProductItems)
        Dim updates As New IMIS_BL.ProductItemsBL

        updates.UpdateProductItems(eProductItems)

    End Sub
    Public Sub UpdateProductServices(ByVal eProductServices As IMIS_EN.tblProductServices)
        Dim update As New IMIS_BL.ProductServicesBL

        update.UpdateProductServices(eProductServices)

    End Sub
    Public Function GetDistribution(ByVal ProdId As Integer, ByVal DistrCareType As String, ByVal DistrType As Integer) As DataTable
        Dim getDatatable As New IMIS_BL.RelDistributionBL
        Return getDatatable.GetDistribution(ProdId, DistrCareType, DistrType)
    End Function
    Function GetAllDistributionData(ByVal eRelDistr As IMIS_EN.tblRelDistr) As DataTable
        Dim distr As New IMIS_BL.RelDistributionBL
        Return distr.GetAllDistributionData(eRelDistr)
    End Function
    Function SaveRelDistributionPercentageChanges(ByVal eRelDistr As IMIS_EN.tblRelDistr) As Boolean
        Dim distr As New IMIS_BL.RelDistributionBL
        Return distr.SaveRelDistributionPercentageChanges(eRelDistr)
    End Function
    Public Function GetDays(Optional ByVal ShowLabel As Boolean = False) As DataTable
        Dim BLPrd As New IMIS_BL.ProductsBL
        Return BLPrd.GetDays(ShowLabel)
    End Function
    Public Function GetMonths(Optional ByVal ShowLabel As Boolean = False) As DataTable
        Dim BLPrd As New IMIS_BL.ProductsBL
        Return BLPrd.GetMonths(ShowLabel)
    End Function
    Public Function GetCeilingInterpritation() As DataTable
        Dim BL As New IMIS_BL.ProductsBL
        Return BL.GetCeilingInterpritation
    End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True, Optional ByVal IncludeNational As Boolean = False) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect, IncludeNational)
    End Function
    Public Function getLevels() As DataTable
        Dim BL As New IMIS_BL.HealthFacilityBL
        Return BL.GetHFLevel(True)
    End Function
    Public Function getSubLevels() As DataTable
        Dim BL As New IMIS_BL.HealthFacilityBL
        Return BL.GetSublevel()
    End Function
    Public Function GetProdIdByUUID(ByVal uuid As Guid) As Integer
        Dim Prod As New IMIS_BL.ProductsBL
        Return Prod.GetProdIdByUUID(uuid)
    End Function
End Class
