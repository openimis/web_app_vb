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

Public Class PriceListMIBL
    Public Function GetPriceListMI(ByVal UserId As Integer, ByVal RegionId As Integer, ByVal DistrictId As Integer, Optional ByVal ShowSelectedRow As Boolean = False) As DataTable
        Dim Gen As New GeneralBL
        Dim getDataTable As New IMIS_DAL.PriceListMIDAL
        Dim dtbl As DataTable = getDataTable.GetPriceListMI(UserId, RegionId, DistrictId)
        If ShowSelectedRow = True Then
            Dim dr As DataRow = dtbl.NewRow
            dr("PLItemId") = 0
            dr("PLItemName") = Gen.getMessage("M_ITEMNAME") '"-- Select Price List --"
            dtbl.Rows.InsertAt(dr, 0)
        End If
        Return dtbl

    End Function
    Public Function GetPriceListMI(ByVal ePL As IMIS_EN.tblPLItems, Optional ByVal All As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_DAL.PriceListMIDAL
        ePL.PLItemName += "%"
        Return getDataTable.GetPLItems(ePL, All)


    End Function
    Public Function GetPLMedicalItems(ByVal PLItemID As Integer) As DataTable
        Dim getDataTable As New IMIS_DAL.PriceListMIDAL
        Dim MI As New IMIS_BL.MedicalItemsBL
        Dim dtItype As DataTable = MI.GetItemType(False)
        Return getDataTable.GetPLMedicalItems(PLItemID, dtItype)
    End Function
    Public Function SavePriceListMI(ByRef ePLItems As IMIS_EN.tblPLItems) As Integer
        Dim SaveData As New IMIS_DAL.PriceListMIDAL
        Dim dt As DataTable = SaveData.CheckIfPriceListItemExists(ePLItems)
        If dt.Rows.Count > 0 Then Return 1
        If ePLItems.PLItemID = 0 Then
            SaveData.InsertPriceListMI(ePLItems)
            Return 0
        Else
            SaveData.UpdatePriceListMI(ePLItems)
            Return 2
        End If
    End Function


    Public Sub LoadPriceListMI(ByRef ePLItems As IMIS_EN.tblPLItems)
        Dim load As New IMIS_DAL.PriceListMIDAL
        load.LoadPriceListMI(ePLItems)
    End Sub
    Public Sub DeletePriceListMI(ByRef ePLItems As IMIS_EN.tblPLItems)
        Dim PLMI As New IMIS_DAL.PriceListMIDAL
        PLMI.DeletePriceListMI(ePLItems)
    End Sub
    Public Sub SavePLItemDetails(ByVal ePLItemDetails As IMIS_EN.tblPLItemsDetail, ByVal action As Integer)
        Dim Item As New IMIS_DAL.PriceListMIDAL
        If action = 0 Then
            Item.InsertGrid(ePLItemDetails)
        ElseIf action = 1 Then
            Item.DeleteGrid(ePLItemDetails)
        Else
            Item.UpdateGrid(ePLItemDetails)
        End If

    End Sub
    Public Function GetPLItemIdByUUID(ByVal uuid As Guid) As Integer
        Dim PLItem As New IMIS_DAL.PriceListMIDAL
        Return PLItem.GetPLItemIdByUUID(uuid).Rows(0).Item(0)
    End Function
End Class
