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

Public Class MedicalItemsDAL
    Public Sub LoadItems(ByRef eItems As IMIS_EN.tblItems)
        Dim data As New ExactSQL
        data.setSQLCommand("select *  from tblItems where ItemID = @ItemID", CommandType.Text)
        data.params("@ItemID", SqlDbType.Int, eItems.ItemID)
        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            eItems.ItemCode = dr("ItemCode")
            eItems.ItemName = dr("ItemName")
            eItems.ItemType = dr("ItemType")
            eItems.ItemPackage = dr("ItemPackage")
            eItems.ItemPrice = dr("ItemPrice")
            eItems.ItemCareType = dr("ItemCareType")
            eItems.ItemFrequency = dr("ItemFrequency")
            eItems.ItemPatCat = dr("ItemPatCat")

            eItems.ValidityTo = if(dr("ValidityTo").ToString = String.Empty, Nothing, dr("ValidityTo"))
        End If

    End Sub
    Public Sub DeleteMedicalItems(ByRef eItems As IMIS_EN.tblItems)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblItems ([ItemCode],[ItemName],[ItemType],[ItemPackage],[ItemPrice],[ItemCareType],[ItemFrequency],[ItemPatCat],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID])" _
      & " select [ItemCode],[ItemName],[ItemType],[ItemPackage],[ItemPrice],[ItemCareType],[ItemFrequency],[ItemPatCat],[ValidityFrom],getdate(),@ItemID,[AuditUserID] from tblItems where ItemID = @ItemID;" _
      & "UPDATE [tblItems] SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate(),[AuditUserID] = @AuditUserID  WHERE ItemID = @ItemID", CommandType.Text)
        data.params("@ItemID", SqlDbType.Int, eItems.ItemID)
        data.params("@AuditUserID", SqlDbType.Int, eItems.AuditUserID)
        data.ExecuteCommand()
    End Sub
    Public Function GetMI(ByVal eItems As IMIS_EN.tblItems, ByVal ALL As Boolean, ByVal dtIType As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim strSQL As String = "select ItemId,ItemCode,ItemName,IType.Name AS ItemType ,ItemPackage,ItemPrice,validityfrom,validityto from tblItems inner join @dtIType IType on Itype.code = tblItems.itemType where ItemCode LIKE @ItemCode AND ItemName LIKE @ItemName AND ItemType LIKE @ItemType and isnull(ItemPackage,'') like @ItemPackage"
        If ALL = False Then
            strSQL += " AND ValidityTo is NULL"
        End If
        strSQL += " order by ItemCode,ValidityTo,ValidityFrom"
        data.setSQLCommand(strSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, eItems.AuditUserID)
        data.params("@ItemCode", SqlDbType.NVarChar, 25, eItems.ItemCode)
        data.params("@ItemName", SqlDbType.NVarChar, 100, eItems.ItemName)
        data.params("@ItemType", SqlDbType.NVarChar, 1, eItems.ItemType)
        data.params("@ItemPackage", SqlDbType.NVarChar, 25, eItems.ItemPackage)
        data.params("@dtIType", dtIType, "xCareType")
        Return data.Filldata
    End Function
   
    Public Sub InsertMedicalItems(ByRef eItems As IMIS_EN.tblItems)
        Dim data As New ExactSQL
        Dim sSQL As String = "INSERT INTO tblItems(ItemCode,ItemName,ItemType,ItemPackage,ItemPrice,ItemCareType,ItemFrequency,ItemPatCat,AuditUserID)" & _
            " VALUES(@ItemCode,@ItemName,@ItemType,@ItemPackage,@ItemPrice,@ItemCareType,@ItemFrequency,@ItemPatCat,@AuditUserID)"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ItemCode", SqlDbType.NVarChar, 25, eItems.ItemCode)
        data.params("@ItemName", SqlDbType.NVarChar, 100, eItems.ItemName)
        data.params("@itemType", SqlDbType.NVarChar, 1, eItems.ItemType)
        data.params("@ItemPackage", SqlDbType.NVarChar, 255, eItems.ItemPackage)
        data.params("@ItemPrice", SqlDbType.Int, eItems.ItemPrice)
        data.params("@ItemCareType", SqlDbType.Char, 1, eItems.ItemCareType)
        data.params("@ItemFrequency", SqlDbType.SmallInt, eItems.ItemFrequency)
        data.params("@ItemPatCat", SqlDbType.TinyInt, eItems.ItemPatCat)
        data.params("@AuditUserID", SqlDbType.Int, eItems.AuditUserID)
        data.ExecuteCommand()
    End Sub
    Public Sub UpdateMedicalItems(ByRef eItems As IMIS_EN.tblItems)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblItems ([ItemCode],[ItemName],[ItemType],[ItemPackage],[ItemPrice],[ItemCareType],[ItemFrequency],[ItemPatCat],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID])" _
      & " select [ItemCode],[ItemName],[ItemType],[ItemPackage],[ItemPrice],[ItemCareType],[ItemFrequency],[ItemPatCat],[ValidityFrom],getdate(),@ItemID,[AuditUserID] from tblItems where ItemID = @ItemID;" _
      & "UPDATE [tblItems] SET [ItemCode] = @ItemCode,[ItemName] = @ItemName,[ItemType] = @ItemType,[ItemPackage] = @ItemPackage,[ItemPrice] = @ItemPrice,[ItemCareType] = @ItemCareType,[ItemFrequency] = @ItemFrequency, [ItemPatCat] = @ItemPatCat" _
      & ",[ValidityFrom] = GetDate(),[LegacyID] = @LegacyID,[AuditUserID] = @AuditUserID  WHERE ItemID = @ItemID", CommandType.Text)
        data.params("@ItemID", SqlDbType.Int, eItems.ItemID)
        data.params("@ItemCode", SqlDbType.NVarChar, 25, eItems.ItemCode)
        data.params("@ItemName", SqlDbType.NVarChar, 100, eItems.ItemName)
        data.params("@ItemType", SqlDbType.Char, 1, eItems.ItemType)
        data.params("@ItemPackage", SqlDbType.NVarChar, 255, eItems.ItemPackage)
        data.params("@ItemPrice", SqlDbType.Decimal, eItems.ItemPrice)
        data.params("@ItemCareType", SqlDbType.Char, 1, eItems.ItemCareType)
        data.params("@ItemFrequency", SqlDbType.SmallInt, eItems.ItemFrequency)
        data.params("@ItemPatCat", SqlDbType.TinyInt, eItems.ItemPatCat)
        data.params("@LegacyID", SqlDbType.Int, 1, ParameterDirection.Output)
        data.params("@AuditUserID", SqlDbType.Int, eItems.AuditUserID)
        data.ExecuteCommand()
    End Sub
    Public Function CheckIfItemExists(ByVal eItem As IMIS_EN.tblItems) As DataTable
        Dim data As New ExactSQL
        Dim str As String = "Select top 1 itemid from tblItems where ItemCode = @ItemCode AND ValidityTo is null"
        If Not eItem.ItemID = 0 Then
            str += " AND tblItems.ItemID <> @ItemID"
        End If
        data.setSQLCommand(str, CommandType.Text)
        data.params("@ItemCode", SqlDbType.NVarChar, 6, eItem.ItemCode)
        data.params("@ItemID", SqlDbType.Int, eItem.ItemID)
        Return data.Filldata()
    End Function
    Public Function CheckIfDelete(ByVal eItem As IMIS_EN.tblItems) As DataTable
        Dim data As New ExactSQL
        Dim str As String = "select top 1 tblItems.ItemID from tblItems left join tblProductitems on tblItems.ItemID = tblProductitems.itemid and tblProductitems.ValidityTo is null" & _
                            " left join tblPLItemsDetail on tblPLitemsdetail.ItemID = tblItems.itemid  and tblPLItemsDetail.ValidityTo is null" & _
                            " where (tblProductItems.ItemID = @ItemID or tblPLItemsDetail.itemid = @ItemID)"

        data.setSQLCommand(str, CommandType.Text)
        data.params("@ItemID", SqlDbType.Int, eItem.ItemID)
        Return data.Filldata()
    End Function
End Class
