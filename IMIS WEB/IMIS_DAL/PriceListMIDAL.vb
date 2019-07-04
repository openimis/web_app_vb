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

Public Class PriceListMIDAL

    'Corrected
    Public Sub DeletePriceListMI(ByRef ePLItems As IMIS_EN.tblPLItems)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPLItems ([PLItemName],[DatePL],[LocationId],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID])" _
      & " select [PLItemName],[DatePL],[LocationId],[ValidityFrom],getdate(),@PLItemID,[AuditUserID] from tblPLItems where PLItemID = @PLItemID;" _
      & "UPDATE [tblPLItems] SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate(),[AuditUserID] = @AuditUserID  WHERE PLItemID = @PLItemID", CommandType.Text)
        data.params("@PLItemID", SqlDbType.Int, ePLItems.PLItemID)
        data.params("@AuditUserID", SqlDbType.Int, ePLItems.AuditUserID)
        data.ExecuteCommand()
    End Sub

    'Corrected
    Public Sub InsertPriceListMI(ByRef ePLItem As IMIS_EN.tblPLItems)
        Dim data As New ExactSQL
        Dim sSQL As String = "INSERT INTO tblPLItems(PLItemName,DatePL,LocationId,AuditUserID)" &
            " VALUES(@PLItemName,@DatePL,@LocationId,@AuditUserID); select @PLItemID = scope_identity()"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@PLItemID", SqlDbType.Int, ePLItem.PLItemID, ParameterDirection.Output)
        data.params("@PLItemName", SqlDbType.NVarChar, 100, ePLItem.PLItemName)
        data.params("@DatePL", SqlDbType.SmallDateTime, ePLItem.DatePL)
        data.params("@LocationId", SqlDbType.Int, IIf(ePLItem.tblLocations.LocationId = -1, Nothing, ePLItem.tblLocations.LocationId))
        data.params("@AuditUserID", SqlDbType.Int, ePLItem.AuditUserID)
        data.ExecuteCommand()
        ePLItem.PLItemID = data.sqlParameters("@PLItemID")
    End Sub

    'Corrected
    Public Function GetPriceListMI(ByVal userid As Integer, ByVal RegionId As Integer, ByVal DistrictId As Integer) As DataTable
        Dim data As New ExactSQL

        'data.setSQLCommand("select PLItemID,PLItemName from tblPLItems left join  tblUsersDistricts on tblUsersDistricts.LocationId = tblPLItems.Locationid and @UserId = tblUsersDistricts.userid where (tblplitems.LocationId = @LocationId or tblplitems.LocationId is null) and tblusersdistricts.validityto is null and tblplitems.validityto is null order by PLItemName", CommandType.Text)
        Dim sSQL As String = String.Empty
        sSQL += " SELECT PLItemID,PLItemName FROM tblPLItems  PL"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(PL.LocationId, 0)"
        sSQL += " LEFT OUTER JOIN tblUsersDistricts UD ON PL.LocationId = UD.LocationId AND UD.UserId = @UserId AND UD.ValidityTo IS NULL"
        sSQL += " WHERE PL.ValidityTo IS NULL"
        sSQL += " AND UD.ValidityTo IS NULL"
        sSQL += " AND (L.Regionid = @RegionId  OR L.LocationId = 0 )"
        sSQL += " AND (L.DistrictId = @DistrictId  OR L.DistrictId IS NULL OR @DistrictId = 0)"
        sSQL += " ORDER BY L.ParentLocationId"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@UserID", SqlDbType.Int, userid)
        data.params("@RegionId", SqlDbType.Int, RegionId)
        data.params("@DistrictId", SqlDbType.Int, DistrictId)
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetPLItems(ByVal ePL As IMIS_EN.tblPLItems, ByVal All As Boolean) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        'Dim strSQL As String = "select tblPLItems.*,'Region' RegionName,Districtname from tblPLItems left outer join tblDistricts on tblPLItems.LocationId = tblDistricts.DistrictId left outer join tblUsersDistricts UD on UD.LocationId = tblPLItems.LocationId and UD.userid = @userid and UD.ValidityTo is NULL "
        sSQL = "SELECT PL.PLItemID,PL.DatePL, PLItemName,PL.ValidityFrom,PL.ValidityTo, L.RegionName , L.DistrictName"
        sSQL += " FROM tblPLItems  PL"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(PL.LocationId, 0)"
        sSQL += " INNER JOIN"
        sSQL += " (SELECT UD.UserId, L.DistrictId, L.RegionId"
        sSQL += " FROM tblUsersDistricts UD"
        sSQL += " INNER JOIN uvwLocations L ON L.DistrictId = UD.LocationId"
        sSQL += " WHERE UD.ValidityTo IS NULL"
        sSQL += " AND (UD.UserId = @UserId OR @UserId = 0)"
        sSQL += " GROUP BY UD.UserId, L.DistrictId, L.RegionId"
        sSQL += " )UD ON UD.DistrictId = PL.LocationId OR UD.RegionId = PL.LocationId OR PL.LocationId IS NULL"
        sSQL += " INNER JOIN tblUsers U ON U.UserID=@userId"
        sSQL += " WHERE (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId = 0)"
        sSQL += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL)"
        sSQL += " AND PLItemName like @PLItemName"

        If All = False Then
            sSQL += " AND PL.ValidityTo is NULL"
        End If
        If Not ePL.DatePL = Nothing Then
            sSQL += " AND DatePL = @DatePL"
        End If
        sSQL += " GROUP BY PL.PLItemID,PL.DatePL, PLItemName,PL.ValidityFrom,PL.ValidityTo, L.RegionName , L.DistrictName"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, ePL.AuditUserID)
        data.params("@PLItemName", SqlDbType.NVarChar, 100, ePL.PLItemName)
        If Not ePL.DatePL = Nothing Then
            data.params("@DatePL", SqlDbType.SmallDateTime, ePL.DatePL)
        End If
        data.params("@LocationId", SqlDbType.Int, ePL.tblLocations.LocationId)
        data.params("@RegionId", SqlDbType.Int, ePL.tblLocations.RegionId)
        data.params("@DistrictId", SqlDbType.Int, ePL.tblLocations.DistrictID)
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetPLItemsNoDate(ByVal ePL As IMIS_EN.tblPLItems) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("select tblPLItems.*,Districtname from tblPLItems inner join tblDistricts on tblPLItems.LocationId = tblDistricts.LocationId WHERE PLItemName like @PLItemName AND CASE WHEN @LocationId = 0 THEN 0 ELSE tblPLItems.LocationId END = @LocationId AND tblPLItems.ValidityTo is NULL order by PLItemName", CommandType.Text)
        data.params("@PLItemName", SqlDbType.NVarChar, 100, ePL.PLItemName)
        data.params("@LocationId", SqlDbType.Int, ePL.tblLocations.LocationId)

        Return data.Filldata
    End Function

    'Corrected
    Public Function GetPLItems(ByVal LocationId As Integer) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("select tblPLItems.*,Districtname from tblPLItems inner join tblDistricts on tblPLItems.LocationId = tblDistricts.LocationId where tblPLItems.ValidityTo is NULL AND  tblOfficer.LocationId  = @LocationId ORDER BY PLItemName", CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetPLItemsFullSearchLegacy(ByVal ePL As IMIS_EN.tblPLItems) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("select tblPLItems.*,Districtname from tblPLItems inner join tblDistricts on tblPLItems.LocationId = tblDistricts.LocationId WHERE PLItemName like @PLItemName AND DatePL = @DatePL AND CASE WHEN @LocationId = 0 THEN 0 ELSE tblPLItems.LocationId END = @LocationId order by PLItemName", CommandType.Text)
        data.params("@PLItemName", SqlDbType.NVarChar, 100, ePL.PLItemName)
        data.params("@DatePL", SqlDbType.SmallDateTime, ePL.DatePL)
        data.params("@LocationId", SqlDbType.Int, ePL.tblLocations.LocationId)
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetPLItemsFullSearch(ByVal ePL As IMIS_EN.tblPLItems) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("select tblPLItems.*,Districtname from tblPLItems inner join tblDistricts on tblPLItems.LocationId = tblDistricts.LocationId WHERE PLItemName like @PLItemName AND DatePL = @DatePL AND CASE WHEN @LocationId = 0 THEN 0 ELSE tblPLItems.LocationId END = @LocationId AND tblPLItems.ValidityTo is NULL order by PLItemName", CommandType.Text)
        data.params("@PLItemName", SqlDbType.NVarChar, 100, ePL.PLItemName)
        data.params("@DatePL", SqlDbType.SmallDateTime, ePL.DatePL)
        data.params("@LocationId", SqlDbType.Int, ePL.tblLocations.LocationId)
        Return data.Filldata
    End Function
  
    'Corrected
    Public Sub UpdatePriceListMI(ByRef ePLItems As IMIS_EN.tblPLItems)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPLItems ([PLItemName],[DatePL],[LocationId],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID])" _
      & " select [PLItemName],[DatePL],[LocationId],[ValidityFrom],getdate(),@PLItemID,[AuditUserID] from tblPLItems where PLItemID = @PLItemID;" _
      & "UPDATE [tblPLItems] SET [PLItemName] = @PLItemName,[DatePL] = @DatePL,[LocationId] = @LocationId" _
      & ",[ValidityFrom] = GetDate(),[LegacyID] = @LegacyID,[AuditUserID] = @AuditUserID  WHERE PLItemID = @PLItemID", CommandType.Text)
        data.params("@PLItemID", SqlDbType.Int, ePLItems.PLItemID)
        data.params("@PLItemName", SqlDbType.NVarChar, 100, ePLItems.PLItemName)
        data.params("@DatePL", SqlDbType.Date, ePLItems.DatePL)
        data.params("@LocationId", SqlDbType.Int, if(ePLItems.tblLocations.LocationId = -1, Nothing, ePLItems.tblLocations.LocationId))
        data.params("@LegacyID", SqlDbType.Int, 1, ParameterDirection.Output)
        data.params("@AuditUserID", SqlDbType.Int, ePLItems.AuditUserID)
        data.ExecuteCommand()
    End Sub
    Public Sub UpdateGrid(ByVal eItem As IMIS_EN.tblPLItemsDetail)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPLItemsDetail ([PLItemID],[ItemID],[PriceOverule],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID]) " _
        & " Select [PLItemID],[ItemID],[PriceOverule],[ValidityFrom],getdate(),[PLItemDetailID],[AuditUserID] from tblPLItemsDetail where PLItemDetailID = @PLItemDetailID ; UPDATE [tblPLItemsDetail] SET  [PriceOverule]= @PriceOverule ,[ValidityFrom] = getdate(), [AuditUserID] = @AuditUserID where PLItemDetailID = @PLItemDetailID", CommandType.Text)
        data.params("@PLItemDetailID", SqlDbType.Int, eItem.PLItemDetailID)
        data.params("@PLItemID", SqlDbType.Int, eItem.tblPLItems.PLItemID)
        data.params("@ItemID", SqlDbType.Int, eItem.tblItems.ItemID)
        data.params("@PriceOverule", SqlDbType.Decimal, if(eItem.PriceOverule Is Nothing, DBNull.Value, eItem.PriceOverule))
        data.params("@AuditUserID", SqlDbType.Int, eItem.AuditUserID)

        data.ExecuteCommand()
    End Sub
    Public Sub DeleteGrid(ByVal eItem As IMIS_EN.tblPLItemsDetail)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPLItemsDetail ([PLItemID],[ItemID],[PriceOverule],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID]) " _
        & " Select [PLItemID],[ItemID],[PriceOverule],[ValidityFrom],getdate(),[PLItemDetailID],[AuditUserID] from tblPLItemsDetail where PLItemDetailID = @PLItemDetailID ; UPDATE [tblPLItemsDetail] SET  [ValidityFrom] = getdate(),[ValidityTo] = getdate(), [AuditUserID] = @AuditUserID where PLItemDetailID = @PLItemDetailID", CommandType.Text)
        data.params("@PLItemDetailID", SqlDbType.Int, eItem.PLItemDetailID)
        data.params("@AuditUserID", SqlDbType.Int, eItem.AuditUserID)

        data.ExecuteCommand()
    End Sub
    Public Sub InsertGrid(ByVal eItem As IMIS_EN.tblPLItemsDetail)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPLItemsDetail ([PLItemID],[ItemID],[PriceOverule],[AuditUserID]) " _
        & " VALUES (@PLItemID,@ItemID,@PriceOverule,@AuditUserID)", CommandType.Text)

        data.params("@PLItemID", SqlDbType.Int, eItem.tblPLItems.PLItemID)
        data.params("@ItemID", SqlDbType.Int, eItem.tblItems.ItemID)
        data.params("@PriceOverule", SqlDbType.Decimal, if(eItem.PriceOverule Is Nothing, DBNull.Value, eItem.PriceOverule))
        data.params("@AuditUserID", SqlDbType.Int, eItem.AuditUserID)

        data.ExecuteCommand()
    End Sub

    'Corrected
    Public Sub LoadPriceListMI(ByRef ePLItems As IMIS_EN.tblPLItems)
        Dim data As New ExactSQL
        Dim dr As DataRow
        Dim sSQL As String = ""
        sSQL += " SELECT PL.* , ISNULL(R.RegionId, PL.LocationId) RegionId FROM tblPLItems   PL"
        sSQL += " LEFT JOIN tblDistricts D ON D.DistrictId= PL.LocationId"
        sSQL += " LEFT JOIN tblRegions R ON R.RegionId=D.Region"
        sSQL += " WHERE PLItemID=@PLItemID"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PLItemID", SqlDbType.Int, ePLItems.PLItemID)
        dr = data.Filldata()(0)
        If Not dr Is Nothing Then
            Dim eLocation As New IMIS_EN.tblLocations
            ePLItems.RegionId = if(dr.IsNull("RegionId"), -1, dr("RegionId"))
            eLocation.LocationId = if(dr.IsNull("LocationId"), -1, dr("LocationId"))
            ePLItems.PLItemID = dr("PLItemID")
            ePLItems.PLItemName = dr("PLItemName")
            ePLItems.DatePL = dr("DatePL")
            ePLItems.ValidityTo = if(dr("ValidityTo").ToString = String.Empty, Nothing, dr("ValidityTo"))
            ePLItems.tblLocations = eLocation
        End If
    End Sub
    'Corrected by Rogers
    Public Function GetPLMedicalItems(ByVal PLItemID As Integer, DtIType As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " SELECT  	PLItemDetailID,I.ItemID ,CAST(CASE WHEN tblPLItemsDetail.ItemID IS NULL THEN 0 ELSE 1 END AS BIT)  checked,"
        sSQL += " ItemCode,ItemName,dt.Name ItemType ,ItemPackage,ItemPrice,"
        sSQL += " tblPLItemsDetail.PriceOverule FROM tblitems I"
        sSQL += " LEFT JOIN ( SELECT * FROM tblPLItemsDetail"
        sSQL += " WHERE tblPLItemsDetail.plitemid =@PLItemID AND tblPLItemsDetail.ValidityTo IS NULL ) tblPLItemsDetail ON I.ItemID = tblPLItemsDetail.ItemID"
        sSQL += " LEFT OUTER JOIN @dtIType DT ON dt.Code = I.ItemType"
        sSQL += " WHERE I.ValidityTo IS NULL ORDER BY CASE WHEN tblPLItemsDetail.ItemID IS NULL THEN 0 ELSE 1 END DESC,ItemCode"
        data.setSQLCommand(sSQL, CommandType.Text)

        'where (CASE WHEN @ItemCode = '' THEN '' ELSE ItemCode END) = @ItemCode AND (CASE WHEN @ItemName = '' THEN '' ELSE ItemName END) LIKE @ItemName + '%' AND ItemType = @ItemType AND ValidityTo is NULL order by ItemCode
        data.params("@PLItemID", SqlDbType.Int, PLItemID)
        data.params("@dtIType", dtIType, "xCareType")
        Return data.Filldata
    End Function
    Public Function CheckIfPriceListItemExists(ByVal ePLItems As IMIS_EN.tblPLItems) As DataTable
        Dim data As New ExactSQL
        Dim strSQL As String = "Select top 1 plitemID from tblPLItems where PLItemName = @PLItemName and ValidityTo is null"

        If Not ePLItems.PLItemID = 0 Then
            strSQL += " AND PLItemID <> @PLItemID"
        End If
        data.setSQLCommand(strSQL, CommandType.Text)
        data.params("@PLItemID", SqlDbType.Int, ePLItems.PLItemID)
        data.params("@PLItemName", SqlDbType.NVarChar, 100, ePLItems.PLItemName)

        Return data.Filldata()
    End Function
End Class
