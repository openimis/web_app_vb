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

Public Class LocationsDAL
    'Corrected
    Public Function GetDistricts(ByVal UserID As Integer, Optional RegionId As Integer = 0) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "Select distinct tbldistricts.DistrictID,DistrictName,DistrictCode, Region"
        sSQL += " from tblDistricts"
        sSQL += " left outer join tblUsersDistricts on tbldistricts.districtID = [tblUsersDistricts].[LocationID]"
        sSQL += " and tblUsersDistricts.validityto is null"
        sSQL += " inner join tblRegions on tblDistricts.Region = tblRegions.RegionId"
        sSQL += " where case when @userid = 0 then 0 else userid end =  @UserID"
        sSQL += " and case when @RegionId = 0 then 0 else RegionId end = @RegionId"
        sSQL += " and  tblDistricts.ValidityTo is NULL"
        sSQL += " and tblRegions.ValidityTo is null"
        sSQL += " order by DistrictName"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserID", SqlDbType.Int, UserID)
        data.params("@RegionId", SqlDbType.Int, RegionId)

        Return data.Filldata
    End Function
    'Corrected + Rogers addition of option regionId
    Public Function GetDistrictsALL(ByVal UserID As Integer, Optional RegionId As Integer = 0, Optional Authority As Integer = 0) As DataTable
        Dim data As New ExactSQL
        If Authority = 0 Then Authority = UserID
        Dim sSQL As String = ""
        sSQL += " SELECT CASE WHEN UserID IS NULL THEN 0 ELSE 1 END AS checked,userDistrictId, tblDistricts.DistrictID, tblDistricts.DistrictName, DistrictCode,tblDistricts.Region FROM tblDistricts"
        sSQL += " LEFT JOIN tblUsersDistricts ON tblDistricts.DistrictID = tblUsersDistricts.LocationId And  tblUsersDistricts.UserID = @UserID And tblUsersDistricts.ValidityTo Is NULL "
        sSQL += " RIGHT JOIN (SELECT LocationID FROM tblUsersDistricts UD"
        sSQL += " WHERE UserID = @Authority And UD.ValidityTo Is NULL) AR ON Ar.LocationID = tblDistricts.DistrictID"
        sSQL += " WHERE tbldistricts.validityto Is null And tblDistricts.DistrictName <> N'Funding'"

        If RegionId <> 0 Then sSQL += "   AND Region = @RegionId "
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserID", SqlDbType.Int, UserID)
        data.params("@Authority", SqlDbType.Int, Authority)
        data.params("@RegionId", SqlDbType.Int, RegionId)
        Return data.Filldata
    End Function
    Public Function GetDistrictForHF(ByVal HFID As Integer, ByVal UserId As Integer) As DataTable
        Dim data As New ExactSQL
        Dim Query As String = "SELECT CAST(CASE WHEN UD.LocationId IS NULL THEN 0 ELSE 1 END AS BIT) Checked,D.DistrictId, DistrictName, DistrictCode,UD.UserDistrictID, D.Region " & _
                " FROM tblDistricts D INNER JOIN tbLHF HF ON D.districtId = HF.LocationId" & _
                " LEFT OUTER JOIN tblUsersDistricts UD ON UD.LocationId = D.DistrictId AND UD.UserId = @UserId" & _
                " WHERE HF.HFId = @HFID AND D.DistrictName <> N'Funding'" & _
                " AND HF.ValidityTo IS NULL AND UD.ValidityTo IS NULL AND D.ValidityTo IS NULL"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@HFID", SqlDbType.Int, HFID)
        data.params("@UserId", SqlDbType.Int, UserId)
        Return data.Filldata
    End Function
    Public Function GetDistricts() As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = "SELECT DistrictId, DistrictName FROM tblDistricts WHERE ValidityTo IS NULL"
        data.setSQLCommand(sSQL, CommandType.Text)
        Return data.Filldata
    End Function
    'Corrected
    Public Sub SaveLocation(ByVal eLocation As IMIS_EN.tblLocations)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "INSERT INTO tblLocations(LocationName,LocationCode,ParentLocationId, LocationType,AuditUserID, MalePopulation, FemalePopulation, OtherPopulation, Families)VALUES(@DistrictName,@DistrictCode,@Region,@LocationType,@AuditUserID, @MalePopulation, @FemalePopulation, @OtherPopulation, @Families);  select @LocationID = scope_identity();"

        If eLocation.LocationType = "D" Then
            sSQL += "Insert into tblUsersDistricts ([UserId],[LocationId],[AuditUserID])"
            sSQL += "VALUES(@AuditUserID, @LocationID, @AuditUserID)"
        End If

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@DistrictName", SqlDbType.NVarChar, 50, eLocation.LocationName)
        data.params("@DistrictCode", SqlDbType.NVarChar, 8, eLocation.LocationCode)
        data.params("@Region", SqlDbType.Int, eLocation.ParentLocationId) ')
        data.params("@AuditUserID", SqlDbType.Int, eLocation.AuditUserId)
        data.params("@LocationType", SqlDbType.NVarChar, 1, eLocation.LocationType)
        data.params("@LocationID", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@MalePopulation", SqlDbType.Int, eLocation.MalePopulation)
        data.params("@FemalePopulation", SqlDbType.Int, eLocation.FemalePopulation)
        data.params("@OtherPopulation", SqlDbType.Int, eLocation.OtherPopulation)
        data.params("@Families", SqlDbType.Int, eLocation.Families)

        data.ExecuteCommand()

    End Sub
    'Corrected
    Public Sub UpdateLocation(ByVal eLocations As IMIS_EN.tblLocations)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "INSERT INTO tblLocations(LocationName,LocationCode,ParentLocationId, LocationType,ValidityFrom,ValidityTo,LegacyID,AuditUserID, MalePopulation, FemalePopulation, OtherPopulation, Families)" & _
               " SELECT LocationName,@DistrictCode,ParentLocationId, LocationType,ValidityFrom,GETDATE(),LocationID,AuditUserID, MalePopulation, FemalePopulation, OtherPopulation, Families FROM tblLocations WHERE LocationID = @LocationID;" & _
               " UPDATE tblLocations SET LocationName = @DistrictName, LocationCode=@DistrictCode, LocationType = @LocationType,ValidityFrom=GETDATE(),AuditUserID=@AuditUserID, MalePopulation = @MalePopulation, FemalePopulation = @FemalePopulation, OtherPopulation = @OtherPopulation, Families = @Families WHERE LocationID = @LocationID"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@LocationID", SqlDbType.Int, eLocations.LocationId)
        data.params("@DistrictName", SqlDbType.NVarChar, 50, eLocations.LocationName)
        data.params("@DistrictCode", SqlDbType.NVarChar, 8, eLocations.LocationCode)
        data.params("@LocationType", SqlDbType.NVarChar, 1, eLocations.LocationType)
        data.params("@AuditUserID", SqlDbType.Int, eLocations.AuditUserId)
        data.params("@MalePopulation", SqlDbType.Int, eLocations.MalePopulation)
        data.params("@FemalePopulation", SqlDbType.Int, eLocations.FemalePopulation)
        data.params("@OtherPopulation", SqlDbType.Int, eLocations.OtherPopulation)
        data.params("@Families", SqlDbType.Int, eLocations.Families)

        data.ExecuteCommand()

    End Sub
    Public Function GetVillages(ByVal WardID As Integer) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("select VillageID, VillageName,VillageCode,WardId, MalePopulation, FemalePopulation, OtherPopulation, Families from tblVillages where case when @WardID = 0 then 0 else WardId end = @WardID AND tblVillages.LegacyID is NULL AND tblVillages.ValidityTo is NULL  order by VillageName", CommandType.Text)
        data.params("@WardID", SqlDbType.Int, WardID)
        Return data.Filldata
    End Function
    Public Function GetWards(ByVal DistrictID As Integer) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("select WardId,WardName,WardCode,DistrictId from tblWards where case when @DistrictID = 0 then 0 else DistrictID end = @DistrictID AND tblWards.LegacyID is NULL AND tblWards.ValidityTo is NULL    order by WardName", CommandType.Text)
        data.params("@DistrictID", SqlDbType.Int, DistrictID)
        Return data.Filldata
    End Function
    'Corrected
    'Public Sub SaveWards(ByRef eWards As IMIS_EN.tblLocations)
    '    Dim data As New ExactSQL
    '    Dim sSQL As String = ""
    '    sSQL = "INSERT INTO tblLocations(ParentLocationId, LocationName,LocationCode, LocationType,AuditUserID)VALUES(@DistrictId,@WardName,@WardCode, @LocationType,@AuditUserID);SELECT @WardID = SCOPE_IDENTITY()"

    '    data.setSQLCommand(sSQL, CommandType.Text)

    '    data.params("@WardID", SqlDbType.Int, 0, ParameterDirection.Output)
    '    data.params("@DistrictId", SqlDbType.Int, eWards.ParentLocationId)
    '    data.params("@WardName", SqlDbType.NVarChar, 50, eWards.LocationName) 'eDistricts.Region)
    '    data.params("@WardCode", SqlDbType.NVarChar, 8, eWards.LocationCode)
    '    data.params("@LocationType", SqlDbType.NVarChar, 1, eWards.LocationType)
    '    data.params("@AuditUserID", SqlDbType.Int, eWards.AuditUserId)



    '    data.ExecuteCommand()

    '    eWards.LocationId = data.sqlParameters("@WardID")

    'End Sub
    'Corrected
    'Public Sub UpdateWards(ByVal eWards As IMIS_EN.tblLocations)
    '    Dim data As New ExactSQL
    '    Dim sSQL As String = ""
    '    sSQL = "INSERT INTO tblLocations(ParentLocationId,LocationName,LocationCode, LocationType,ValidityFrom,ValidityTo,LegacyID,AuditUserID)" & _
    '           " SELECT ParentLocationID,LocationName,LocationCode, LocationType,ValidityFrom,GETDATE(),LocationID,AuditUserID FROM tblLocations WHERE LocationID = @WardID;" & _
    '           " UPDATE tblLocations SET LocationName=@WardName,LocationCode=@WardCode,ValidityFrom=GETDATE(), LocationType = @LocationType,AuditUserID=@AuditUserID WHERE LocationID = @WardID;"

    '    data.setSQLCommand(sSQL, CommandType.Text)

    '    data.params("@WardID", SqlDbType.Int, eWards.LocationId)
    '    data.params("@WardName", SqlDbType.NVarChar, 50, eWards.LocationName)
    '    data.params("@WardCode", SqlDbType.NVarChar, 8, eWards.LocationCode)
    '    data.params("@LocationType", SqlDbType.NVarChar, 1, eWards.LocationType)
    '    data.params("@AuditUserID", SqlDbType.Int, eWards.AuditUserId)

    '    data.ExecuteCommand()

    'End Sub
    'Corrected
    'Public Sub SaveVillage(ByRef eVillage As IMIS_EN.tblLocations)
    '    Dim data As New ExactSQL
    '    Dim sSQL As String = ""
    '    sSQL = "INSERT INTO tblLocations(ParentLocationID,LocationName,LocationCode, LocationType,AuditUserID)VALUES(@WardID,@VillageName,@VillageCode, @LocationType,@AuditUserID);SELECT @VillageID = SCOPE_IDENTITY()"

    '    data.setSQLCommand(sSQL, CommandType.Text)

    '    data.params("@VillageID", SqlDbType.Int, 0, ParameterDirection.Output)
    '    data.params("@WardId", SqlDbType.Int, eVillage.ParentLocationId)
    '    data.params("@VillageName", SqlDbType.NVarChar, 50, eVillage.LocationName)
    '    data.params("@VillageCode", SqlDbType.NVarChar, 8, eVillage.LocationCode)
    '    data.params("@LocationType", SqlDbType.NVarChar, 1, eVillage.LocationType)
    '    data.params("@AuditUserID", SqlDbType.Int, eVillage.AuditUserId)

    '    data.ExecuteCommand()

    '    eVillage.LocationId = data.sqlParameters("@VillageID")

    'End Sub
    'Corrected
    'Public Sub UpdateVillage(ByRef eVillage As IMIS_EN.tblLocations)
    '    Dim data As New ExactSQL
    '    Dim sSQL As String = ""

    '    sSQL = "INSERT INTO tblLocations(ParentLocationID,LocationName,LocationCode, LocationType,ValidityFrom,ValidityTo,LegacyID,AuditUserID)" & _
    '           " SELECT ParentLocationId,LocationName,LocationCode, LocationType,ValidityFrom,GETDATE(),LocationID,AuditUserID FROM tblLocations WHERE LocationID = @VillageID;" & _
    '           " UPDATE tblLocations SET LocationName=@VillageName,LocationCode=@VillageCode, LocationType = @LocationType,ValidityFrom=GETDATE(),AuditUserID = @AuditUserID  WHERE LocationID = @VillageID;" & _
    '           " SELECT @VillageID = SCOPE_IDENTITY()"

    '    data.setSQLCommand(sSQL, CommandType.Text)

    '    data.params("@VillageID", SqlDbType.Int, eVillage.LocationId)
    '    data.params("@VillageName", SqlDbType.NVarChar, 50, eVillage.LocationName)
    '    data.params("@VillageCode", SqlDbType.NVarChar, 8, eVillage.LocationCode)
    '    data.params("@LocationType", SqlDbType.NVarChar, 1, eVillage.LocationType)
    '    data.params("@AuditUserID", SqlDbType.Int, eVillage.AuditUserId)

    '    data.ExecuteCommand()

    'End Sub
    'Corrected
    Public Function DeleteLocation(ByVal eLocations As IMIS_EN.tblLocations) As Boolean
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "INSERT INTO tblLocations(LocationName,ParentLocationId, LocationType,ValidityFrom,ValidityTo,LegacyID,AuditUserID, MalePopulation, FemalePopulation, OtherPopulation, Families)" & _
               " SELECT LocationName,ParentLocationId, LocationType,ValidityFrom,GETDATE(),LocationID,AuditUserID, MalePopulation, FemalePopulation, OtherPopulation, Families FROM tblLocations WHERE LocationID = @LocationId;" & _
               " UPDATE tblLocations SET ValidityTo=GETDATE(),AuditUserID=@AuditUserID WHERE LocationID = @LocationId"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@LocationId", SqlDbType.Int, eLocations.LocationId)
        data.params("@AuditUserID", SqlDbType.Int, eLocations.AuditUserId)

        data.ExecuteCommand()

        Return True
    End Function

    'Corrected
    Public Function CheckCanBeDeleted(ByVal DistrictID As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "SELECT D.DistrictId"
        sSQL += " FROM tblFamilies F"
        sSQL += " INNER JOIN tblVillages V ON F.LocationId = V.VillageId"
        sSQL += " INNER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictID = W.DistrictId"
        sSQL += " WHERE F.ValidityTo IS NULL"
        'changed by Salumu 14 Aug 2019 From D.DistrictID to V.VillageId
        sSQL += " AND V.VillageId = @DistrictId"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@DistrictID", SqlDbType.Int, DistrictID)

        Return data.Filldata()
    End Function

    'Corrected
    Public Function CheckIfCanDeleteWard(ByVal eWards As IMIS_EN.tblLocations) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "SELECT W.WardId"
        sSQL += " FROM tblFamilies F"
        sSQL += " INNER JOIN tblVillages V ON F.LocationId = V.VillageId"
        sSQL += " INNER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " WHERE F.ValidityTo IS NULL"
        sSQL += " AND W.WardId = @WardID"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@WardID", SqlDbType.Int, eWards.LocationId)
        Return data.Filldata()
    End Function

    'Corrected
    'Public Function DeleteWards(ByVal eWards As IMIS_EN.tblLocations) As Boolean
    '    Dim data As New ExactSQL
    '    Dim sSQL As String = ""
    '    sSQL = "INSERT INTO tblLocations(ParentLocationId,LocationName, LocationCode, LocationType,ValidityFrom,ValidityTo,LegacyID,AuditUserID)" & _
    '           " SELECT ParentLocationID,LocationName, LocationCode, LocationType,ValidityFrom,GETDATE(),LocationID,AuditUserID FROM tblLocations WHERE LocationID = @WardID;" & _
    '           " UPDATE tblLocations SET ValidityTo=GETDATE(),AuditUserID=@AuditUserID WHERE LocationID = @WardID;"

    '    data.setSQLCommand(sSQL, CommandType.Text)

    '    data.params("@WardID", SqlDbType.Int, eWards.LocationId)
    '    data.params("@AuditUserID", SqlDbType.Int, eWards.AuditUserId)

    '    data.ExecuteCommand()
    '    Return True
    'End Function

    'Corrected
    Public Function CheckIfCanDeleteVillage(ByVal eVillage As IMIS_EN.tblLocations) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""

        sSQL = "SELECT V.VillageId"
        sSQL += " FROM tblFamilies F"
        sSQL += " INNER JOIN tblVillages V ON F.LocationId = V.VillageId"
        sSQL += " WHERE F.ValidityTo IS NULL"
        sSQL += " AND V.VillageId = @VillageId"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@VillageID", SqlDbType.Int, eVillage.LocationId)
        Return data.Filldata()
    End Function

    'Corrected
    'Public Function DeleteVillage(ByRef eVillage As IMIS_EN.tblLocations) As Boolean
    '    Dim data As New ExactSQL
    '    Dim sSQL As String = ""

    '    sSQL = "INSERT INTO tblLocations(ParentLocationID,LocationName,LocationCode, LocationType,ValidityFrom,ValidityTo,LegacyID,AuditUserID)" & _
    '           " SELECT ParentLocationID,LocationName,LocationCode, LocationType,ValidityFrom,GETDATE(),LocationID,AuditUserID FROM tblLocations WHERE LocationId = @VillageID;" & _
    '           " UPDATE tblLocations SET ValidityTo=GETDATE(),AuditUserID = @AuditUserID  WHERE LocationID = @VillageID;"

    '    data.setSQLCommand(sSQL, CommandType.Text)

    '    data.params("@VillageID", SqlDbType.Int, eVillage.LocationId)

    '    data.params("@AuditUserID", SqlDbType.Int, eVillage.AuditUserId)

    '    data.ExecuteCommand()
    '    Return True
    'End Function

    Public Function UploadLocations(ByVal DistrictsFile As String, ByVal WardsFile As String, ByVal VillagesFile As String) As Integer
        Dim data As New ExactSQL
        Dim sSQL As String = "uspImportLocations"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@DistrictsFile", SqlDbType.NVarChar, 255, DistrictsFile)
        data.params("@WardsFile", SqlDbType.NVarChar, 255, WardsFile)
        data.params("@VillagesFile", SqlDbType.NVarChar, 255, VillagesFile)
        data.params("@Output", SqlDbType.Int, 0, ParameterDirection.ReturnValue)

        data.ExecuteCommand()

        Return data.sqlParameters("@Output")

    End Function

    'Corrected
    Public Function GetWardsForOfficers(DistrictId As Integer, OfficerId As Integer) As DataTable

        Dim sSQL As String = ""
        'sSQL = ";WITH Wards AS"
        'sSQL += " ("
        sSQL = " SELECT W.WardID WardId, W.WardName WardName, CASE WHEN @OfficerId= 0 THEN 1 ELSE CAST(IIF(OV.WardID IS NULL,0,1) AS BIT) END Checked FROM tblWards W"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictID=w.DistrictID"
        sSQL += " LEFT JOIN ("
        sSQL += " SELECT DISTINCT V.WardID  FROM tblOfficerVillages OV"
        sSQL += " INNER JOIN tblVillages V ON V.VillageID = OV.LocationId"
        sSQL += " INNER JOIN tblWards W ON W.WardID = v.WardID"
        sSQL += " WHERE OV.ValidityTo IS NULL"
        sSQL += " AND V.ValidityTo IS NULL"
        sSQL += " AND (OV.OfficerId = @OfficerId "
        If OfficerId = 0 Then sSQL += " OR @OfficerId = 0 "
        sSQL += " )"
        sSQL += " ) OV ON OV.WardID = W.WardID"
        sSQL += " WHERE W.ValidityTo  IS NULL"
        sSQL += " AND D.ValidityTo IS NULL"
        sSQL += " AND D.DistrictID=@DistrictId"
        'sSQL += " )SELECT Checked, WardId, WardName FROM Wards WHERE RNo = 1"

        Dim data As New ExactSQL
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@DistrictId", SqlDbType.Int, DistrictId)
        data.params("@OfficerId", SqlDbType.Int, OfficerId)
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetVillagesForOfficers(DistrictId As Integer, OfficerId As Integer)
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "SELECT  CASE WHEN @OfficerId =0 THEN 1 ELSE IIF(OV.OfficerId IS NULL, 0, 1) END Checked,W.WardId, V.VillageID, V.VillageName"
        sSQL += " FROM tblDistricts D"
        sSQL += " LEFT OUTER JOIN tblWards W ON D.DistrictID = W.DistrictID"
        sSQL += " LEFT OUTER JOIN tblVillages V ON W.WardID = V.WardID"
        sSQL += " LEFT OUTER JOIN tblOfficerVillages OV ON OV.LocationId = V.VillageID AND OV.OfficerId = @OfficerId AND OV.ValidityTo IS NULL"
        sSQL += " WHERE D.DistrictID = @DistrictId"
        sSQL += " AND V.VillageId IS NOT NULL"
        sSQL += " AND V.VillageName <> 'Funding'"
        sSQL += " AND D.ValidityTo IS NULL"
        sSQL += " AND OV.ValidityTo IS NULL"
        sSQL += " AND W.ValidityTo IS NULL"
        sSQL += " and V.ValidityTo is null"
        sSQL += " GROUP BY OV.OfficerId, W.WardId, V.VillageID, V.VillageName"
        sSQL += " ORDER BY W.WardId, Checked DESC"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@DistrictId", SqlDbType.Int, DistrictId)
        data.params("@OfficerId", SqlDbType.Int, OfficerId)
        Return data.Filldata

    End Function
    Public Sub MoveLocation(ByVal SourceId As Integer, ByVal DestinationId As Integer, ByVal LocationType As String, ByRef AffectedFamilies As Integer, ByRef ErrorMessage As Integer, ByVal AuditUserId As Integer)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "uspMoveLocation"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        data.params("@SourceId", SqlDbType.Int, SourceId)
        data.params("@DestinationId", SqlDbType.Int, DestinationId)
        data.params("@LocationType", SqlDbType.Char, 1, LocationType)
        data.params("@AuditUserId", SqlDbType.Int, AuditUserId)
        data.params("@ErrorMessage", SqlDbType.Int, 0, ParameterDirection.Output)
        data.ExecuteCommand()
        ErrorMessage = data.sqlParameters("@ErrorMessage").ToString
    End Sub

    'Corrected
    Public Function IsLocCodeUnique(ByVal LocCode As String) As Boolean
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "SELECT * FROM tblLocations WHERE LocationCode = @Code AND ValidityTo IS NULL"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@Code", SqlDbType.NVarChar, 30, LocCode)
        Dim dt As DataTable = data.Filldata()
        If dt.Rows.Count = 0 Then Return False
        Return True
    End Function

    'Corrected This return
    Public Function GetRegions(UserId As Integer) As DataTable
        Dim sSQL As String = ""
        sSQL = ";WITH Regions AS"
        sSQL += " ("
        sSQL += " SELECT ROW_NUMBER() OVER(PARTITION BY R.RegionId ORDER BY UD.UserId DESC)RNo,IIF(UD.UserID IS NULL, 0, 1)Checked, R.RegionId, R.RegionName,RegionCode, R.ValidityFrom, R.ValidityTo, R.LegacyId, R.AuditUserId"
        sSQL += " FROM tblRegions R"
        sSQL += " INNER JOIN tblDistricts D ON R.RegionId = D.Region AND D.ValidityTo IS NULL"
        sSQL += " INNER JOIN tblUsersDistricts UD ON D.DistrictID = UD.LocationId AND UD.UserId = @UserId AND UD.ValidityTo IS NULL"
        sSQL += " WHERE R.ValidityTo IS NULL"
        sSQL += " GROUP BY UD.UserID, R.RegionId, R.RegionName, R.ValidityFrom, R.ValidityTo, R.LegacyId, R.AuditUserId,RegionCode"
        sSQL += " )"
        sSQL += " SELECT Checked, RegionId, RegionName,RegionCode, ValidityFrom, ValidityTo, LegacyId, AuditUserId"
        sSQL += " FROM Regions"
        sSQL += " WHERE RNo = 1"

        Dim data As New ExactSQL
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, UserId)
        Return data.Filldata
    End Function


    Public Function getAllRegions(UserId As Integer, Optional Authority As Integer = 0)
        Dim sSQL As String = ";WITH Regions AS ( SELECT ROW_NUMBER() OVER(PARTITION BY R.RegionId ORDER BY UD.UserId DESC)RNo,"
        sSQL += " IIf(UD.UserID Is NULL, 0, 1) Checked, R.RegionId, R.RegionName,RegionCode, R.ValidityFrom, R.ValidityTo,"
        sSQL += " R.LegacyId, R.AuditUserId FROM tblRegions R "
        sSQL += " left JOIN tblDistricts D ON R.RegionId = D.Region And D.ValidityTo Is NULL And R.ValidityTo Is NULL"
        sSQL += " left JOIN tblUsersDistricts UD ON D.DistrictID = UD.LocationId "

        sSQL += " And UD.UserId = @UserId "

        sSQL += " And UD.ValidityTo Is NULL "
        If Authority > 0 Then
            sSQL += " RIGHT JOIN (SELECT DISTINCT RegionId FROM tblRegions R"
            sSQL += " INNER JOIN tblDistricts D ON D.Region = R.RegionId"
            sSQL += "  INNER JOIN tblUsersDistricts UD ON UD.LocationId = D.DistrictId AND UD.ValidityTo IS NULL"

            sSQL += "  where UserID = @Authority And UD.ValidityTo IS NULL) AR ON Ar.RegionId = R.RegionId"
        End If
        sSQL += " WHERE R.ValidityTo IS NULL"



        sSQL += "   Group BY UD.UserID, R.RegionId, R.RegionName, R.ValidityFrom, R.ValidityTo, R.LegacyId, R.AuditUserId,RegionCode ) "
        sSQL += " SELECT Checked, RegionId, RegionName,RegionCode, ValidityFrom, ValidityTo, LegacyId, AuditUserId FROM Regions "

        sSQL += " WHERE RNo = 1"


        Dim data As New ExactSQL
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, UserId)
        data.params("@Authority", SqlDbType.Int, Authority)
        Dim dt As New DataTable
        dt = data.Filldata

        Return dt
    End Function
    'Corrected
    Public Function GetUserRegions(UserId As Integer) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL
        sSQL = " SELECT RegionName FROM tblRegions R"
        sSQL += " INNER JOIN tblDistricts D ON R.RegionId=D.Region"
        sSQL += " INNER JOIN tblUsersDistricts UD ON UD.LocationId = D.DistrictID"
        sSQL += " INNER JOIN tblUsers U ON U.UserID = UD.UserID"
        sSQL += " WHERE R.ValidityTo IS NULL AND D.ValidityTo IS NULL AND UD.ValidityTo IS NULL AND UD.UserID= @UserId"
        sSQL += " GROUP BY RegionName"


        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, UserId)
        Return data.Filldata
    End Function

    Public Function UploadLocationsXML(ByVal Xml As System.Xml.XmlDocument, ByVal StrategyId As Integer, ByVal AuditUserID As Integer, ByRef dtresult As DataTable, Optional ByVal dryRun As Boolean = 0) As Dictionary(Of String, Integer)
        Dim sSQL As String = ""
        Dim data As New ExactSQL
        sSQL = "uspUploadLocationsXML"
        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@XML", Xml.InnerXml)
        data.params("@StrategyId", SqlDbType.Int, StrategyId)
        data.params("@dryRun", SqlDbType.Bit, dryRun)
        data.params("@AuditUserID", SqlDbType.Int, AuditUserID)
        data.params("@SentRegion", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@InsertRegion", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@UpdateRegion", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@SentDistrict", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@InsertDistrict", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@UpdateDistrict", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@SentWard", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@InsertWard", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@UpdateWard", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@SentVillage", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@InsertVillage", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@UpdateVillage", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@returnValue", SqlDbType.Int, Nothing, ParameterDirection.ReturnValue)
        dtresult = data.Filldata()

        Dim Output As New Dictionary(Of String, Integer)
        Output.Add("RegionSent", 0)
        Output.Add("RegionInsert", 0)
        Output.Add("RegionUpdate", 0)
        Output.Add("DistrictSent", 0)
        Output.Add("DistrictInsert", 0)
        Output.Add("DistrictUpdate", 0)
        Output.Add("WardSent", 0)
        Output.Add("WardInsert", 0)
        Output.Add("WardUpdate", 0)
        Output.Add("VillageSent", 0)
        Output.Add("VillageInsert", 0)
        Output.Add("VillageUpdate", 0)
        Output.Add("returnValue", 0)

        Output("RegionSent") = If(data.sqlParameters("@SentRegion") Is DBNull.Value, 0, data.sqlParameters("@SentRegion"))
        Output("RegionInsert") = If(data.sqlParameters("@InsertRegion") Is DBNull.Value, 0, data.sqlParameters("@InsertRegion"))
        Output("RegionUpdate") = If(data.sqlParameters("@UpdateRegion") Is DBNull.Value, 0, data.sqlParameters("@UpdateRegion"))
        Output("DistrictSent") = If(data.sqlParameters("@SentDistrict") Is DBNull.Value, 0, data.sqlParameters("@SentDistrict"))
        Output("DistrictInsert") = If(data.sqlParameters("@InsertDistrict") Is DBNull.Value, 0, data.sqlParameters("@InsertDistrict"))
        Output("DistrictUpdate") = If(data.sqlParameters("@UpdateDistrict") Is DBNull.Value, 0, data.sqlParameters("@UpdateDistrict"))
        Output("WardSent") = If(data.sqlParameters("@SentWard") Is DBNull.Value, 0, data.sqlParameters("@SentWard"))
        Output("WardInsert") = If(data.sqlParameters("@InsertWard") Is DBNull.Value, 0, data.sqlParameters("@InsertWard"))
        Output("WardUpdate") = If(data.sqlParameters("@UpdateWard") Is DBNull.Value, 0, data.sqlParameters("@UpdateWard"))
        Output("VillageSent") = If(data.sqlParameters("@SentVillage") Is DBNull.Value, 0, data.sqlParameters("@SentVillage"))
        Output("VillageInsert") = If(data.sqlParameters("@InsertVillage") Is DBNull.Value, 0, data.sqlParameters("@InsertVillage"))
        Output("VillageUpdate") = If(data.sqlParameters("@UpdateVillage") Is DBNull.Value, 0, data.sqlParameters("@UpdateVillage"))
        Output("returnValue") = data.sqlParameters("@returnValue")
        Return Output
    End Function

    Public Function DownLoadLocationSXML() As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL
        sSQL = "  SELECT "
        sSQL += " (SELECT RegionCode, RegionName "
        sSQL += "  From tblRegions"
        sSQL += "  WHERE ValidityTo IS NULL AND  NOT (RegionName='Funding' OR RegionCode='FR')"
        sSQL += " FOR XML PATH('Region'), ROOT('Regions'), TYPE),"
        sSQL += " (SELECT R.RegionCode, D.DistrictCode, D.DistrictName"
        sSQL += "  From tblDistricts D"
        sSQL += "  INNER Join tblRegions R ON D.Region = R.RegionId"
        sSQL += "  WHERE D.ValidityTo IS NULL AND NOT( DistrictName='Funding' OR DistrictCode ='FD' OR RegionName ='Funding' OR RegionCode= 'FR')"
        sSQL += " And R.ValidityTo Is NULL"
        sSQL += "  For XML PATH('District'), ROOT('Districts'), TYPE),"
        sSQL += "  (SELECT D.DistrictCode, W.WardCode MunicipalityCode, W.WardName MunicipalityName"
        sSQL += "  From tblWards W"
        sSQL += "  INNER Join tblDistricts D ON W.DistrictId = D.DistrictId"
        sSQL += "  WHERE D.ValidityTo Is NULL"
        sSQL += "  AND W.ValidityTo IS NULL AND NOT( WardName ='Funding' OR WardCode='FW' OR DistrictName ='Funding' OR DistrictCode= 'FD')"
        sSQL += "  For XML PATH('Municipality'), ROOT('Municipalities'), TYPE),"
        sSQL += "  (SELECT W.WardCode MunicipalityCode, V.VillageCode, V.VillageName, ISNULL(V.MalePopulation,0)MalePopulation, ISNULL(V.FemalePopulation,0)FemalePopulation, ISNULL(V.OtherPopulation,0)OtherPopulation, ISNULL(V.Families,0)Families"
        sSQL += "  From tblVillages V"
        sSQL += "  INNER Join tblWards W ON V.WardId = W.WardId"
        sSQL += "  WHERE V.ValidityTo Is NULL"
        sSQL += "  AND NOT(VillageName='Funding' OR VillageCode='FV' OR  WardName ='Funding' OR WardCode='FW')"
        sSQL += "  For XML PATH('Village'), ROOT('Villages'), TYPE)"
        sSQL += "  For XML PATH(''),ROOT('Locations')"
        data.setSQLCommand(sSQL, CommandType.Text)
        Return data.Filldata()
    End Function

    'Corrected
    'Public Function InsertRegion(eRegions As IMIS_EN.tblLocations) As Boolean
    '    Dim sSQL As String = "INSERT INTO tblLocations(LocationName, LocationCode,LocationType, AuditUserId) SELECT @RegionName,@RegionCode,@LocationType, @AuditUserId; SELECT @RegionId = SCOPE_IDENTITY();"
    '    Dim data As New ExactSQL
    '    data.setSQLCommand(sSQL, CommandType.Text)

    '    data.params("@RegionId", SqlDbType.Int, 0, direction:=ParameterDirection.Output)
    '    data.params("@RegionName", SqlDbType.NVarChar, 50, eRegions.LocationName)
    '    data.params("@RegionCode", SqlDbType.NVarChar, 8, eRegions.LocationCode)
    '    data.params("@LocationType", SqlDbType.NVarChar, 1, eRegions.LocationType)
    '    data.params("@AuditUserId", SqlDbType.Int, eRegions.AuditUserId)

    '    Return data.ExecuteCommand
    'End Function
    'Corrected
    'Public Function UpdateRegion(eRegions As IMIS_EN.tblLocations) As Boolean
    '    Dim sSQL As String
    '    Dim data As New ExactSQL

    '    sSQL = "INSERT INTO tblLocations(RegionName,RegionCode, ValidityFrom, ValidityTo, LegacyId, AuditUserId, LocationType)"
    '    sSQL += " SELECT RegionName, RegionCode, ValidityFrom, GETDATE() ValidityTo, RegionId LegacyId, AuditUserId, LocationType FROM tblLocations WHERE LocationIdId = @RegionId;"
    '    sSQL += " UPDATE tblLocations SET LocationName = @RegionName,LocationCode=@RegionCode,LocationType= @LocationType, ValidityFrom = GETDATE(), AuditUserId = @AuditUserId WHERE LocationId = @RegionId;"
    '    data.setSQLCommand(sSQL, CommandType.Text)
    '    data.params("@RegionId", SqlDbType.Int, eRegions.LocationId)
    '    data.params("@RegionName", SqlDbType.NVarChar, 50, eRegions.LocationName)
    '    data.params("@RegionCode", SqlDbType.NVarChar, 8, eRegions.LocationCode)
    '    data.params("@LocationType", SqlDbType.NVarChar, 1, eRegions.LocationType)
    '    data.params("@AuditUserId", SqlDbType.Int, eRegions.AuditUserId)

    '    Return data.ExecuteCommand
    'End Function
    'Public Function CheckCanBeDeleted(ByVal RegionId As Integer) As DataTable
    '    Dim data As New ExactSQL
    '    Dim sSQL As String
    '    sSQL = " select R.RegionId from tblRegions R"
    '    sSQL += " INNER JOIN tblDistricts D    ON R.RegionId = D.Region"
    '    sSQL += " WHERE R.RegionId = @RegionId and R.ValidityTo is null and D.ValidityTo is null"

    '    data.setSQLCommand(sSQL, CommandType.Text)
    '    data.params("@RegionId", SqlDbType.Int, RegionId)

    '    Return data.Filldata()
    'End Function
    'Corrected
    'Public Function DeleteRegions(ByVal eRegion As IMIS_EN.tblLocations) As Boolean
    '    Dim data As New ExactSQL
    '    Dim sSQL As String = ""
    '    sSQL += " INSERT INTO tblLocations(RegionName,RegionCode,ValidityFrom,ValidityTo,LegacyID,AuditUserID, LocationType)"
    '    sSQL += " SELECT RegionName,RegionCode,ValidityFrom,GETDATE(),RegionId,AuditUserID, LocationType FROM tblLocations WHERE LocationId = @RegionId;"
    '    sSQL += " UPDATE tblLocations SET ValidityTo=GETDATE(),AuditUserID=@AuditUserID WHERE RegionId = @RegionId"
    '    data.setSQLCommand(sSQL, CommandType.Text)

    '    data.params("@RegionId", SqlDbType.Int, eRegion.LocationId)
    '    data.params("@RegionCode", SqlDbType.NVarChar, 8, eRegion.LocationCode)
    '    data.params("@AuditUserID", SqlDbType.Int, eRegion.AuditUserId)

    '    data.ExecuteCommand()

    '    Return True
    'End Function
End Class
