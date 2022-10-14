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

Public Class PricelistMSBL
    Private imisgen As New GeneralBL
    Public Function GetPriceListMS(ByVal userid As Integer, Optional ByVal All As Boolean = True) As DataTable
        Dim getDataTable As New IMIS_DAL.PricelistMSDAL
        Return getDataTable.GetPriceListMS(userid, All)
    End Function
    Public Sub DeletePriceListMS(ByRef ePLServices As IMIS_EN.tblPLServices)
        Dim PLMS As New IMIS_DAL.PricelistMSDAL
        PLMS.DeletePriceListMS(ePLServices)
    End Sub
    Public Function GetPriceListMS(ByVal ePL As IMIS_EN.tblPLServices, Optional ByVal All As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_DAL.PricelistMSDAL
        ePL.PLServName += "%"
        Return getDataTable.GetPLServices(ePL, All)
    End Function
    Public Function SavePriceListMS(ByRef ePLServices As IMIS_EN.tblPLServices) As Integer
        Dim SaveData As New IMIS_DAL.PricelistMSDAL
        If SaveData.CheckIfPLServiceExists(ePLServices) = True Then Return 1
        If ePLServices.PLServiceID = 0 Then
            SaveData.InsertPriceListMS(ePLServices)
            Return 0
        Else
            SaveData.UpdatePriceListMS(ePLServices)
            Return 2
        End If

    End Function

    Public Sub LoadPriceListMS(ByRef ePLServices As IMIS_EN.tblPLServices)
        Dim load As New IMIS_DAL.PricelistMSDAL
        load.LoadPriceListMS(ePLServices)
    End Sub

    Public Function GetPriceListMS(ByVal UserId As Integer, ByVal RegionId As Integer, ByVal DistrictId As Integer, ByVal ShowSelectRow As Boolean) As DataTable
        Dim GetDataTable As New IMIS_DAL.PricelistMSDAL
        Dim dtbl As DataTable = GetDataTable.GetPriceListMS(UserId, RegionId, DistrictId)
        If ShowSelectRow = True Then
            Dim dr As DataRow = dtbl.NewRow
            dr("PLServiceId") = 0
            dr("PLServName") = imisgen.getMessage("T_SELECTPRICELIST")
            dtbl.Rows.InsertAt(dr, 0)
        End If
        Return dtbl

    End Function

    Public Function GetPriceListDistrictMS(ByVal UserId As Integer, ByVal DistrictName As String, ByVal ShowSelectRow As Boolean) As DataTable
        Dim GetDataTable As New IMIS_DAL.PricelistMSDAL
        Dim dtbl As DataTable = GetDataTable.GetPriceListDistrictMS(UserId, DistrictName)
        If ShowSelectRow = True Then
            Dim dr As DataRow = dtbl.NewRow
            dr("PLServiceId") = 0
            dr("PLServName") = imisgen.getMessage("T_SELECTPRICELIST")
            dtbl.Rows.InsertAt(dr, 0)
        End If
        Return dtbl
    End Function

    Public Sub DetachPriceListDistrictMS(ByVal UserId As Integer, ByVal DistrictName As String)
        Dim PLMS As New IMIS_DAL.PricelistMSDAL
        PLMS.DetachLocalServicePriceListFromHF(UserId, DistrictName)
    End Sub
    Public Sub SavePLServicesDetail(ByVal ePLServicesDetail As IMIS_EN.tblPLServicesDetail, ByVal action As Integer)
        Dim Item As New IMIS_DAL.PricelistMSDAL
        If action = 0 Then
            Item.InsertGrid(ePLServicesDetail)
        ElseIf action = 1 Then
            Item.DeleteGrid(ePLServicesDetail)
        Else
            Item.UpdateGrid(ePLServicesDetail)
        End If

    End Sub

    Public Function GetPLServiceIdByUUID(ByVal uuid As Guid) As Integer
        Dim PLService As New IMIS_DAL.PricelistMSDAL
        Return PLService.GetPLServiceIdByUUID(uuid).Rows(0).Item(0)
    End Function

End Class
