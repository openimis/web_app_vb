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

Public Class MedicalItemsBL
    Private imisgen As New GeneralBL
    Public Function GetMedicalItems(ByVal eItems As IMIS_EN.tblItems, Optional ByVal All As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_DAL.MedicalItemsDAL
        eItems.ItemCode += "%"
        eItems.ItemName += "%"
        eItems.ItemType += "%"
        eItems.ItemPackage += "%"
        Dim dtIType As DataTable = GetItemType()
        Return getDataTable.GetMI(eItems, All, dtIType)

    End Function
    Public Function GetItemType(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim dtbl As New DataTable
        dtbl.Columns.Add("ItemID")
        dtbl.Columns.Add("ItemType")
        dtbl.Columns.Add("AltLanguage")
        Dim dr As DataRow
        If showSelect = True Then
            dr = dtbl.NewRow
            dr("ItemID") = ""
            dr("ItemType") = imisgen.getMessage("T_SELECTITEMTYPE")
            dtbl.Rows.Add(dr)
        End If
        dr = dtbl.NewRow
        dr("ItemID") = "M"
        dr("ItemType") = imisgen.getMessage("T_MEDICALPROSTHESES")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("ItemID") = "D"
        dr("ItemType") = imisgen.getMessage("T_DRUG")
        dtbl.Rows.Add(dr)

        Return dtbl

    End Function
    Public Function SaveMedicalItems(ByRef eItem As IMIS_EN.tblItems) As Integer
        Dim Item As New IMIS_DAL.MedicalItemsDAL
        Dim dt As DataTable = Item.CheckIfItemExists(eItem)
        If dt.Rows.Count > 0 Then Return 1
        If eItem.ItemID = 0 Then
            Item.InsertMedicalItems(eItem)
            Return 0
        Else
            Item.UpdateMedicalItems(eItem)
            Return 2
        End If
    End Function
    Public Function DeleteMedicalItems(ByVal eItems As IMIS_EN.tblItems) As Boolean
        Dim MI As New IMIS_DAL.MedicalItemsDAL
        Dim dt As DataTable = MI.CheckIfDelete(eItems)
        If dt.Rows.Count > 0 Then
            Return False
        Else
            MI.DeleteMedicalItems(eItems)
            Return True
        End If
    End Function
    Public Sub LoadItems(ByRef eItems As IMIS_EN.tblItems)
        Dim Items As New IMIS_DAL.MedicalItemsDAL
        Items.LoadItems(eItems)
    End Sub
    Public Function GetItemIdByUUID(ByVal uuid As Guid) As Integer
        Dim Item As New IMIS_DAL.MedicalItemsDAL
        Return Item.GetItemIdByUUID(uuid).Rows(0).Item(0)
    End Function
End Class
