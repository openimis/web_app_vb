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

Public Class PricelistMSDAL
  
    'Corrected    
    Public Sub DeletePriceListMS(ByRef ePLServices As IMIS_EN.tblPLServices)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPLServices ([PLServName],[DatePL],[LocationId],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID])" _
      & " select [PLServName],[DatePL],[LocationId],[ValidityFrom],getdate(),@PLServiceID,[AuditUserID] from tblPLServices where PLServiceID = @PLServiceID;" _
      & "UPDATE [tblPLServices] SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate(),[AuditUserID] = @AuditUserID  WHERE PLServiceID = @PLServiceID", CommandType.Text)
        data.params("@PLServiceID", SqlDbType.Int, ePLServices.PLServiceID)
        data.params("@AuditUserID", SqlDbType.Int, ePLServices.AuditUserID)
        data.ExecuteCommand()
    End Sub

    'Corrected + Rogers 
    Public Function GetPriceListMS(ByVal UserId As Integer, ByVal RegionId As Integer, ByVal DistrictId As Integer) As DataTable
        Dim data As New ExactSQL
        Dim SSQL As String = String.Empty
        'data.setSQLCommand("select PLServiceID,PLServName from tblPLServices left join  tblUsersDistricts on tblUsersDistricts.LocationId = tblPLServices.LocationId and @UserId = tblUsersDistricts.userid where (tblplservices.LocationId = @LocationId or tblplservices.LocationId IS NULL) and tblusersdistricts.validityto is null and tblplservices.validityto is null order by PLServName", CommandType.Text)

        SSQL = " SELECT PLServiceID,PLServName FROM tblPLServices PL"
        SSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(PL.LocationId, 0)"
        SSQL += " LEFT OUTER JOIN tblUsersDistricts UD ON PL.LocationId = UD.LocationId AND UD.UserId = @UserId AND UD.ValidityTo IS NULL"
        SSQL += " WHERE PL.ValidityTo IS NULL"
        SSQL += " AND UD.ValidityTo IS NULL"
        SSQL += " AND (L.Regionid = @RegionId  OR L.LocationId = 0)"
        SSQL += " AND (L.DistrictId = @DistrictId  OR L.DistrictId IS NULL)"
        SSQL += " ORDER BY L.ParentLocationId"

        data.setSQLCommand(SSQL, CommandType.Text)
        data.params("@UserID", SqlDbType.Int, UserId)
        data.params("@RegionId", SqlDbType.Int, RegionId)
        data.params("@DistrictId", SqlDbType.Int, DistrictId)
        Return data.Filldata
    End Function

    'Corrected  
    Public Function GetPriceListMS(ByVal UserId As Integer, Optional ByVal All As Boolean = True) As DataTable
        Dim data As New ExactSQL
        If All = True Then
            data.setSQLCommand("select tblPLServices.*,DistrictName from tblPLServices inner join tblDistricts on tblPLServices.LocationId = tblDistricts.DistrictID inner join tblusersdistricts on tblusersdistricts.LocationId = tbldistricts.districtid and tbldistricts.validityto is null and tblusersdistricts.userid = @UserID and tblusersdistricts.validityto is null order by PLServName", CommandType.Text)
        Else
            data.setSQLCommand("select tblPLServices.*,DistrictName from tblPLServices, inner join tblDistricts on tblPLServices.LocationId = tblDistricts.DistrictID AND tblPLServices.ValidityTo is NULL inner join tblusersdistricts on tblusersdistricts.LocationId = tbldistricts.districtid and tbldistricts.validityto is null and tblusersdistricts.userid = @UserID tblusersdistricts.validityto is null order by PLServName", CommandType.Text)
        End If
        data.params("@UserID", SqlDbType.Int, UserId)
        Return data.Filldata
    End Function
    

    'Corrected  
    Public Function GetPLServices(ByVal LocationId As Integer) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("select tblPLServices.*,Districtname from tblPLServices inner join tblDistricts on tblPLServices.LocationId = tblDistricts.DistrictID where tblPLServices.ValidityTo is NULL AND  tblOfficer.LocationId  = @LocationId ORDER BY PLServName", CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        Return data.Filldata
    End Function

    'Corrected  
    Public Function GetPLServices(ByVal ePL As IMIS_EN.tblPLServices, ByVal All As Boolean) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""

        sSQL = "SELECT PL.PLServiceID,PL.PLServName,PL.DatePL,PL.ValidityFrom,PL.ValidityTo, L.RegionName , L.DistrictName"
        sSQL += " FROM tblPLServices  PL"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(PL.LocationId, 0)"
        sSQL += " INNER JOIN"
        sSQL += " (SELECT UD.UserId, L.DistrictId, L.RegionId"
        sSQL += " FROM tblUsersDistricts UD"
        sSQL += " INNER JOIN uvwLocations L ON L.DistrictId = UD.LocationId"
        sSQL += " WHERE UD.ValidityTo IS NULL"
        sSQL += " AND (UD.UserId = @UserId OR @UserId = 0)"
        sSQL += " GROUP BY UD.UserId, L.DistrictId, L.RegionId"
        sSQL += " )UD ON UD.DistrictId = PL.LocationId OR UD.RegionId = PL.LocationId OR PL.LocationId IS NULL"
        sSQL += " INNER JOIN tblUsers U ON U.UserID= UD.UserID"
        sSQL += " WHERE (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId = 0)"
        sSQL += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL)"
        sSQL += " AND PLServName like @PLServName"
       
        If All = False Then
            sSQL += " AND PL.ValidityTo is NULL"
        End If
        If Not ePL.DatePL = Nothing Then
            sSQL += " AND DatePL = @DatePL"
        End If

        sSQL += " GROUP BY PL.PLServiceID,PL.PLServName,PL.DatePL,PL.ValidityFrom,PL.ValidityTo, L.RegionName , L.DistrictName "
        sSQL += " ORDER BY  PL.PLServName,PL.ValidityTo DESC,PL.ValidityFrom DESC"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, ePL.AuditUserID)
        data.params("@PLServName", SqlDbType.NVarChar, 100, ePL.PLServName)
        If Not ePL.DatePL = Nothing Then
            data.params("@DatePL", SqlDbType.SmallDateTime, ePL.DatePL)
        End If
        data.params("@LocationId", SqlDbType.Int, ePL.tblLocations.LocationId)
        data.params("@RegionId", SqlDbType.Int, ePL.tblLocations.RegionId)
        data.params("@DistrictId", SqlDbType.Int, ePL.tblLocations.DistrictID)
        Return data.Filldata
    End Function
   
   

    'Corrected  
    Public Sub UpdatePriceListMS(ByRef ePLServices As IMIS_EN.tblPLServices)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPLServices ([PLServName],[DatePL],[LocationId],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID])" _
      & " select [PLServName],[DatePL],[LocationId],[ValidityFrom],getdate(),@PLServiceID,[AuditUserID] from tblPLServices where PLServiceID = @PLServiceID;" _
      & "UPDATE [tblPLServices] SET [PLServName] = @PLServName,[DatePL] = @DatePL,[LocationId] = @LocationId" _
      & ",[ValidityFrom] = GetDate(),[LegacyID] = @LegacyID,[AuditUserID] = @AuditUserID  WHERE PLServiceID = @PLServiceID", CommandType.Text)
        data.params("@PLServiceID", SqlDbType.Int, ePLServices.PLServiceID)
        data.params("@PLServName", SqlDbType.NVarChar, 100, ePLServices.PLServName)
        data.params("@DatePL", SqlDbType.SmallDateTime, ePLServices.DatePL)
        data.params("@LocationId", SqlDbType.Int, if(ePLServices.tblLocations.LocationId = -1, Nothing, ePLServices.tblLocations.LocationId))
        data.params("@LegacyID", SqlDbType.Int, 1, ParameterDirection.Output)
        data.params("@AuditUserID", SqlDbType.Int, ePLServices.AuditUserID)
        data.ExecuteCommand()
    End Sub
    Public Sub UpdateGrid(ByVal eItem As IMIS_EN.tblPLServicesDetail)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPLServicesDetail ([PLServiceID],[ServiceID],[PriceOverule],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID]) " _
        & " Select [PLServiceID],[ServiceID],[PriceOverule],[ValidityFrom],getdate(),[PLServiceDetailID],[AuditUserID] from tblPLServicesDetail where PLServiceDetailID = @PLServiceDetailID " _
        & " UPDATE [tblPLServicesDetail] SET  [PriceOverule]= @PriceOverule ,[ValidityFrom] = getdate(), [AuditUserID] = @AuditUserID where PLServiceDetailID = @PLServiceDetailID", CommandType.Text)
        data.params("@PLServiceDetailID", SqlDbType.Int, eItem.PLServiceDetailID)
        data.params("@PLServiceID", SqlDbType.Int, eItem.tblPLServices.PLServiceID)
        data.params("@ServiceID", SqlDbType.Int, eItem.tblServices.ServiceID)
        data.params("@PriceOverule", SqlDbType.Decimal, if(eItem.PriceOverule Is Nothing, DBNull.Value, eItem.PriceOverule))
        data.params("@AuditUserID", SqlDbType.Int, eItem.AuditUserID)

        data.ExecuteCommand()
    End Sub




    Public Sub DeleteGrid(ByVal eItem As IMIS_EN.tblPLServicesDetail)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPLServicesDetail ([PLServiceID],[ServiceID],[PriceOverule],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID]) " _
        & " Select [PLServiceID],[ServiceID],[PriceOverule],[ValidityFrom],getdate(),[PLServiceDetailID],[AuditUserID] from tblPLServicesDetail where PLServiceDetailID = @PLServiceDetailID " _
        & " UPDATE [tblPLServicesDetail] SET  [ValidityFrom] = getdate(),[ValidityTo] = getdate(), [AuditUserID] = @AuditUserID where PLServiceDetailID = @PLServiceDetailID", CommandType.Text)
        data.params("@PLServiceDetailID", SqlDbType.Int, eItem.PLServiceDetailID)
        data.params("@AuditUserID", SqlDbType.Int, eItem.AuditUserID)

        data.ExecuteCommand()
    End Sub




    Public Sub InsertGrid(ByVal eItem As IMIS_EN.tblPLServicesDetail)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPLServicesDetail ([PLServiceID],[ServiceID],[PriceOverule],[AuditUserID]) " _
        & " VALUES (@PLServiceID,@ServiceID,@PriceOverule,@AuditUserID)", CommandType.Text)

        data.params("@PLServiceID", SqlDbType.Int, eItem.tblPLServices.PLServiceID)
        data.params("@ServiceID", SqlDbType.Int, eItem.tblServices.ServiceID)
        data.params("@PriceOverule", SqlDbType.Decimal, if(eItem.PriceOverule Is Nothing, DBNull.Value, eItem.PriceOverule))
        data.params("@AuditUserID", SqlDbType.Int, eItem.AuditUserID)

        data.ExecuteCommand()
    End Sub


    'Corrected  + Rogers
    Public Sub LoadPriceListMS(ByRef ePLServices As IMIS_EN.tblPLServices)
        Dim data As New ExactSQL
        Dim dr As DataRow
        Dim sSQL As String = ""
        sSQL += " SELECT PL.*, ISNULL(R.RegionId,PL.LocationId) RegionId FROM tblPLServices  PL"
        sSQL += " LEFT JOIN tblDistricts D ON D.DistrictId= PL.LocationId"
        sSQL += " LEFT JOIN tblRegions R ON R.RegionId=D.Region"
        sSQL += " WHERE  PLServiceID = @PLServiceID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PLServiceID", SqlDbType.Int, ePLServices.PLServiceID)
        dr = data.Filldata()(0)
        If Not dr Is Nothing Then
            Dim eLocations As New IMIS_EN.tblLocations
            ePLServices.RegionId = if(dr.IsNull("RegionId"), -1, dr("RegionId")) ' dr("RegionId")
            eLocations.LocationId = if(dr.IsNull("LocationId"), -1, dr("LocationId"))
            ePLServices.PLServiceID = dr("PLServiceID")
            ePLServices.PLServName = dr("PLServName")
            ePLServices.DatePL = dr("DatePL")
            ePLServices.AuditUserID = dr("AuditUserID")
            ePLServices.ValidityTo = if(dr("ValidityTo").ToString = String.Empty, Nothing, dr("ValidityTo"))
            ePLServices.tblLocations = eLocations
        End If
    End Sub


    'Corrected  
    Public Sub InsertPriceListMS(ByRef ePLService As IMIS_EN.tblPLServices)
        Dim data As New ExactSQL
        Dim sSQL As String = "INSERT INTO tblPLServices(PLServName,DatePL,LocationId,AuditUserID)" &
            " VALUES(@PLServName,@DatePL,@LocationId,@AuditUserID); select @PLServiceID = scope_identity()"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@PLServiceID", SqlDbType.Int, ePLService.PLServiceID, ParameterDirection.Output)
        data.params("@PLServName", SqlDbType.NVarChar, 100, ePLService.PLServName)
        data.params("@DatePL", SqlDbType.SmallDateTime, ePLService.DatePL)
        data.params("@LocationId", SqlDbType.Int, IIf(ePLService.tblLocations.LocationId = -1, Nothing, ePLService.tblLocations.LocationId))
        data.params("@AuditUserID", SqlDbType.Int, ePLService.AuditUserID)
        data.ExecuteCommand()
        ePLService.PLServiceID = data.sqlParameters("@PLServiceID")
    End Sub


    Public Function CheckIfPLServiceExists(ByVal ePLService As IMIS_EN.tblPLServices) As Boolean
        Dim data As New ExactSQL
        Dim strSQL As String = "Select Count(*) from tblPLServices where PLServName = @PLServName AND ValidityTo IS NULL"

        If Not ePLService.PLServiceID = 0 Then
            strSQL += " AND PLServiceID <> @PLServiceID"
        End If
        data.setSQLCommand(strSQL, CommandType.Text)
        data.params("@PLServiceID", SqlDbType.Int, ePLService.PLServiceID)
        data.params("@PLServName", SqlDbType.NVarChar, 100, ePLService.PLServName)

        Return data.ExecuteScalar()
    End Function

End Class
