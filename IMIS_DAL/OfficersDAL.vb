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

Public Class OfficersDAL
    Public Function CheckIfOfficerExists(ByVal eOfficer As IMIS_EN.tblOfficer) As DataTable
        Dim data As New ExactSQL
        Dim strSQL As String = "Select Top 1 * from tblOfficer where Code = @Code and ValidityTo is null"
        If Not eOfficer.OfficerID = 0 Then
            strSQL += " AND tblOfficer.OfficerID <> @OfficerID"
        End If
        data.setSQLCommand(strSQL, CommandType.Text)
        data.params("@Code", SqlDbType.NVarChar, 8, eOfficer.Code)
        data.params("@OfficerID", SqlDbType.Int, eOfficer.OfficerID)

        Return data.Filldata()
    End Function

    'CorrectedN + Rogers
    Public Sub LoadOfficer(ByRef eOfficers As IMIS_EN.tblOfficer)


        Dim data As New ExactSQL
        Dim dr As DataRow
        Dim sSQL As String = ""
        sSQL += " SELECT 	O.Code,O.LastName,O.OtherNames,O.DOB ,O.Phone,O.LocationId,O.OfficerIDSubst,O.WorksTo,O.VEOCode,O.VEOLastName"
        sSQL += " ,O.VEOOtherNames, O.VEODOB, O.VEOPhone ,O.ValidityTo, O.EmailId ,O.PhoneCommunication, O.PermanentAddress, R.RegionId, O.HasLogin  FROM tblOfficer  O"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId = O.LocationId"
        sSQL += " INNER JOIN tblRegions R ON R.RegionId=D.Region"
        sSQL += " WHERE OfficerID=@OfficerId"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@OfficerId", SqlDbType.Int, eOfficers.OfficerID)
        dr = data.Filldata()(0)
        If Not dr Is Nothing Then

            Dim eLocations As New IMIS_EN.tblLocations
            eLocations.LocationId = dr("LocationId")
            eOfficers.LocationId1 = dr("RegionId")
            eOfficers.tblLocations = eLocations
            eOfficers.Code = dr("Code")
            eOfficers.LastName = dr("LastName")
            eOfficers.OtherNames = dr("OtherNames")
            eOfficers.DOB = if(dr("DOB") Is DBNull.Value, Nothing, dr("DOB"))
            eOfficers.Phone = if(dr("Phone") Is DBNull.Value, Nothing, dr("Phone"))
            eOfficers.WorksTo = if(dr("WorksTo") Is DBNull.Value, Nothing, dr("WorksTo"))
            Dim eofficer2 As New IMIS_EN.tblOfficer
            eofficer2.OfficerID = if(dr("OfficerIDSubst") Is DBNull.Value, 0, dr("OfficerIDSubst"))
            eOfficers.tblOfficer2 = eofficer2

            eOfficers.VEOCode = if(dr("VEOCode") Is DBNull.Value, Nothing, dr("VEOCode"))
            eOfficers.VEOPhone = if(dr("VEOPhone") Is DBNull.Value, Nothing, dr("VEOPhone"))
            eOfficers.VEOLastName = if(dr("VEOLastName") Is DBNull.Value, Nothing, dr("VEOLastName"))
            eOfficers.VEOOtherNames = if(dr("VEOOtherNames") Is DBNull.Value, Nothing, dr("VEOOtherNames"))
            eOfficers.VEODOB = if(dr("VEODOB") Is DBNull.Value, Nothing, dr("VEODOB"))
            If Not dr("ValidityTo") Is DBNull.Value Then
                eOfficers.ValidityTo = dr("ValidityTo").ToString
            End If
            eOfficers.EmailId = dr("EmailId").ToString
            eOfficers.PhoneCommunication = If(dr("PhoneCommunication") Is DBNull.Value, False, dr("PhoneCommunication"))
            eOfficers.PermanentAddress = If(dr("PermanentAddress") Is DBNull.Value, "", dr("PermanentAddress"))
            eOfficers.HasLogin = If(dr("HasLogin") Is DBNull.Value, Nothing, dr("HasLogin"))
        End If


    End Sub

    Public Function GetOfficers(ByVal LocationId As Integer, ByVal VillageId As Integer, Optional WorksTo As Date? = Nothing) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String


        'Do not delete the comment


        'sSQL += " SELECT OfficerId,Code + ' - ' +  Lastname + ' ' + OtherNames AS Code, 1 [Level] FROM tblOfficer O"
        'sSQL += " INNER JOIN uvwLocations L ON L.LocationId = O.LocationId"
        'sSQL += " WHERE O.ValidityTo IS NULL AND (@EnrolmentDate  <= O.WorksTo OR O.WorksTo IS NULL OR @EnrolmentDate IS NULL)"
        'sSQL += " AND CASE WHEN @VillageId > 0 THEN -1 ELSE ISNULL(L.ParentLocationId, -1) END = ISNULL(L.ParentLocationId, -1)"
        'sSQL += " AND (L.DistrictId = @LocationId OR @LocationId = 0 )"
        'sSQL += " UNION"
        'sSQL += " SELECT DISTINCT O.OfficerId,Code + ' - ' +  Lastname + ' ' + OtherNames AS Code, 2 [Level] FROM tblOfficer O"
        'sSQL += " INNER JOIN tblDistricts D ON O.LocationID = D.DistrictID"
        'sSQL += " INNER JOIN tblOfficerVillages OV ON OV.OfficerId = O.OfficerID"
        'sSQL += " WHERE O.ValidityTo is NULL"
        'sSQL += " AND OV.LocationId=@VillageId AND OV.ValidityTo IS NULL AND D.ValidityTo IS NULL"
        'sSQL += " AND  (OV.LocationId  = @VillageId OR @VillageId = 0) AND (@EnrolmentDate  <= O.WorksTo OR O.WorksTo IS NULL"
        'sSQL += " OR @EnrolmentDate IS NULL)"


        sSQL = ";WITH Locations AS"
        sSQL += " ("
        sSQL += " SELECT * FROM"
        sSQL += " (SELECT 0 LocationId, N'National' LocationName, NULL ParentLocationId"
        sSQL += " UNION"
        sSQL += " SELECT LocationId, LocationName, ISNULL(ParentLocationId, 0)ParentLocationId FROM tblLocations WHERE ValidityTO IS NULL"
        sSQL += " )X WHERE LocationId = @LocationId OR @LocationId = 0"
        sSQL += " UNION ALL"
        sSQL += " SELECT L.LocationId, L.LocationName, L.ParentLocationId"
        sSQL += " FROM tblLocations L"
        sSQL += " INNER JOIN Locations ON Locations.ParentLocationId = L.LocationId"
        sSQL += " AND L.ValidityTo IS NULL"
        sSQL += " ), Officers AS"
        sSQL += " ("
        sSQL += " SELECT OfficerId,Code + ' - ' +  Lastname + ' ' + OtherNames AS Code, 0 [Level]"
        sSQL += " FROM tblOfficer"
        sSQL += " WHERE ValidityTo IS NULL"
        sSQL += " AND LocationId IS NULL"
        sSQL += " AND (@WorksTo  <= WorksTo OR WorksTo IS NULL OR @WorksTo IS NULL)"
        sSQL += " UNION"
        sSQL += " SELECT OfficerId,Code + ' - ' +  Lastname + ' ' + OtherNames AS Code, 1 [Level]"
        sSQL += " FROM tblOfficer O"
        'CHANGED FROM Locations to tblLocations  BY AMANI 15/11/2017
        sSQL += " INNER JOIN tblLocations L ON L.LocationId = O.LocationId"
        'END CHANGE
        sSQL += " WHERE O.ValidityTo IS NULL"
        sSQL += " AND (@WorksTo  <= O.WorksTo OR O.WorksTo IS NULL OR @WorksTo IS NULL)"
        sSQL += " AND CASE WHEN @VillageId > 0 THEN -1 ELSE ISNULL(L.ParentLocationId, -1) END = ISNULL(L.ParentLocationId, -1)"
        'ADDED BY AMANI TO DISPAL REGIONAL OFFICERS 15/11/2017
        sSQL += "AND (L.LocationId = @LocationId OR L.ParentLocationId=@LocationId OR @LocationId=0 )"
        'END ADDED
        sSQL += " UNION"
        sSQL += " SELECT DISTINCT O.OfficerId,Code + ' - ' +  Lastname + ' ' + OtherNames AS Code, 2 [Level]"
        sSQL += " FROM tblOfficer O"
        sSQL += " INNER JOIN tblDistricts D ON O.LocationID = D.DistrictID"
        sSQL += " INNER JOIN tblOfficerVillages OV ON OV.OfficerId = O.OfficerID"
        sSQL += " WHERE O.ValidityTo is NULL"
        sSQL += " AND OV.LocationId=@VillageId"
        sSQL += " AND OV.ValidityTo IS NULL"
        sSQL += " AND D.ValidityTo IS NULL"
        sSQL += " AND  (OV.LocationId  = @VillageId OR @VillageId = 0)"
        sSQL += " AND (@WorksTo  <= O.WorksTo OR O.WorksTo IS NULL OR @WorksTo IS NULL)"
        sSQL += " )"
        sSQL += " SELECT OfficerId, Code"
        sSQL += " FROM Officers"
        sSQL += " ORDER BY [Level]"



        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@VillageId", SqlDbType.Int, VillageId)
        data.params("@WorksTo", SqlDbType.Date, WorksTo)
        Return data.Filldata

    End Function

    'Corrected
    Public Function GetOfficers(ByVal eOfficer As IMIS_EN.tblOfficer, ByVal All As Boolean) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""

        'sSQL = "select tblOfficer.*,Districtname from tblOfficer left outer join tblDistricts on tblOfficer.LocationId = tblDistricts.DistrictID inner join tblUsersDistricts UD on UD.LocationId = tblOfficer.LocationId and UD.userid = @userid and UD.ValidityTo is null WHERE code like @Code AND  LastName LIKE @LastName AND OtherNames LIKE @OtherNames  AND Phone  like @Phone AND ISNULL(EmailId, '') LIKE @EmailId"

        sSQL = " SELECT DISTINCT O.OfficerID,O.OfficerUUID,O.Code,O.OtherNames,O.LastName,O.Phone,O.DOB,O.ValidityFrom, O.ValidityTo,"
        sSQL += " L.RegionName , L.DistrictName, ISNULL(HasLogin,0) HasLogin"
        sSQL += " FROM tblOfficer O"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(O.LocationId, 0) "
        sSQL += " INNER JOIN (SELECT UD.UserId, L.DistrictId, L.RegionId FROM tblUsersDistricts UD"
        sSQL += " INNER JOIN uvwLocations L ON L.DistrictId = UD.LocationId"
        sSQL += " WHERE UD.ValidityTo IS NULL AND (UD.UserId = @UserId OR @UserId = 0)"
        sSQL += " GROUP BY UD.UserId, L.DistrictId, L.RegionId )UD ON UD.DistrictId = O.LocationId"
        sSQL += " INNER JOIN tblUsers U ON U.UserId = UD.UserId"

        sSQL += " WHERE (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId = 0)"
        sSQL += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL)"
        sSQL += " AND code like @Code"
        sSQL += " AND  O.LastName LIKE @LastName"
        sSQL += " AND O.OtherNames LIKE @OtherNames"
        sSQL += " AND O.Phone  like @Phone"
        sSQL += " AND ISNULL(O.EmailId, '') LIKE @EmailId"
        sSQL += " AND U.ValidityTo IS NULL"

        If Not eOfficer.DOBFrom Is Nothing Then
            sSQL += " AND DOB >= @DOBFROM"
        End If
        If Not eOfficer.DOBTo Is Nothing Then
            sSQL += " AND DOB <= @DOBTO"
        End If
        'If Not eOfficer.tblLocations.LocationId = 0 Then
        '    sSQL += " AND tblOfficer.LocationId = @LocationId"
        'End If
        If All = False Then
            sSQL += " AND O.ValidityTo is NULL"
        End If

        sSQL += " order by Code,O.validityfrom desc"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, eOfficer.AuditUserID)
        data.params("@Code", SqlDbType.NVarChar, 8, eOfficer.Code)
        data.params("@LastName", SqlDbType.NVarChar, 100, eOfficer.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eOfficer.OtherNames)
        data.params("@DOBFROM", SqlDbType.Date, eOfficer.DOBFrom)
        data.params("@DOBTO", SqlDbType.Date, eOfficer.DOBTo)
        data.params("@Phone", SqlDbType.NVarChar, 50, eOfficer.Phone)
        data.params("@LocationId", SqlDbType.Int, eOfficer.tblLocations.LocationId)
        data.params("@RegionId", SqlDbType.Int, eOfficer.tblLocations.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eOfficer.tblLocations.DistrictID)
        data.params("@Emailid", SqlDbType.NVarChar, 200, eOfficer.EmailId)
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetOfficersFullSearchLegacy(ByVal eOfficer As IMIS_EN.tblOfficer) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("select tblOfficer.*,Districtname from tblOfficer inner join tblDistricts on tblOfficer.LocationId = tblDistricts.DistrictID WHERE code like @Code AND  LastName LIKE @LastName AND OtherNames LIKE @OtherNames AND CASE WHEN @LocationId = 0 THEN 0 ELSE tblOfficer.LocationId END = @LocationId AND  DOB >= @DOB AND Phone  like @Phone AND ISNULL(EmailId, '') LIKE @EmailId order by Code,tblOfficer.ValidityTo", CommandType.Text)
        data.params("@Code", SqlDbType.NVarChar, 8, eOfficer.Code)
        data.params("@LastName", SqlDbType.NVarChar, 100, eOfficer.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eOfficer.OtherNames)
        data.params("@DOB", SqlDbType.Date, eOfficer.DOB)
        data.params("@Phone", SqlDbType.NVarChar, 50, eOfficer.Phone)
        data.params("@LocationId", SqlDbType.Int, eOfficer.tblLocations.LocationId)
        data.params("@Emailid", SqlDbType.NVarChar, 200, eOfficer.EmailId)
        Return data.Filldata
    End Function
    Public Sub DeleteOfficer(ByRef eOfficers As IMIS_EN.tblOfficer)
        Dim data As New ExactSQL

        data.setSQLCommand("INSERT INTO tblOfficer ([Code],[LastName],[OtherNames],[DOB],[Phone],[LocationId],[OfficerIDSubst],[WorksTo],[VEOCode],[VEOLastName],[VEOOtherNames],[VEODOB],[VEOPhone],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID], [EmailId], [PhoneCommunication],[PermanentAddress],[HasLogin])" _
      & " select [Code],[LastName],[OtherNames],[DOB],[Phone],[LocationId],[OfficerIDSubst],[WorksTo],[VEOCode],[VEOLastName],[VEOOtherNames],[VEODOB],[VEOPhone],[ValidityFrom],getdate(),@OfficerID,[AuditUserID], [EmailId],PhoneCommunication,PermanentAddress,HasLogin from tblOfficer where OfficerID = @OfficerID;" _
      & "UPDATE [tblOfficer] SET [ValidityTo]=Getdate(),[AuditUserID] = @AuditUserID WHERE OfficerID = @OfficerID", CommandType.Text)
        data.params("@OfficerID", SqlDbType.Int, eOfficers.OfficerID)
        data.params("@AuditUserID", SqlDbType.Int, eOfficers.AuditUserID)
        data.ExecuteCommand()
    End Sub

    'Corrected
    Public Sub InsertOfficer(ByRef eOfficers As IMIS_EN.tblOfficer)
        Dim data As New ExactSQL
        data.setSQLCommand("Insert into tblOfficer([Code],[LastName],[OtherNames],[DOB],[Phone],[LocationId],[OfficerIDSubst],[WorksTo],[VEOCode],[VEOLastName],[VEOOtherNames],[VEODOB],[VEOPhone],[AuditUserID], [EmailId],PhoneCommunication,PermanentAddress,HasLogin)" _
        & "VALUES(@OfficerCode, @LastName,@OtherNames,@DOB,@Phone,@LocationId,@OfficerIDSubst,@WorksTo,@VEOCode,@VEOLastName,@VEOOtherNames,@VEODOB,@VEOPhone,@AuditUserID, @EmailId,@PhoneCommunication, @PermanentAddress,@HasLogin);SELECT @OfficerId = SCOPE_IDENTITY();", CommandType.Text)
        data.params("@OfficerId", SqlDbType.Int, 0, direction:=ParameterDirection.Output)
        data.params("@OfficerCode", SqlDbType.NVarChar, 8, eOfficers.Code)
        data.params("@LastName", SqlDbType.NVarChar, 100, eOfficers.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eOfficers.OtherNames)
        data.params("@DOB", SqlDbType.DateTime, if(eOfficers.DOB Is Nothing, SqlTypes.SqlDateTime.Null, eOfficers.DOB))
        data.params("@Phone", SqlDbType.NVarChar, 50, eOfficers.Phone)
        data.params("@LocationId", SqlDbType.Int, If(eOfficers.tblLocations.LocationId = -1, DBNull.Value, eOfficers.tblLocations.LocationId))
        data.params("@OfficerIDSubst", SqlDbType.Int, if(eOfficers.tblOfficer2.OfficerID = 0, DBNull.Value, eOfficers.tblOfficer2.OfficerID))

        data.params("@WorksTo", SqlDbType.DateTime, if(eOfficers.WorksTo Is Nothing, SqlTypes.SqlDateTime.Null, eOfficers.WorksTo))
        data.params("@VEOCode", SqlDbType.NVarChar, 25, eOfficers.VEOCode)
        data.params("@VEOLastName", SqlDbType.NVarChar, 100, eOfficers.VEOLastName)
        data.params("@VEOOtherNames", SqlDbType.NVarChar, 100, eOfficers.VEOOtherNames)
        data.params("@VEOPhone", SqlDbType.NVarChar, 25, eOfficers.VEOPhone)
        data.params("@VEODOB", SqlDbType.Date, if(eOfficers.VEODOB Is Nothing, SqlTypes.SqlDateTime.Null, eOfficers.VEODOB))
        data.params("@AuditUserID", SqlDbType.Int, eOfficers.AuditUserID)
        data.params("@Emailid", SqlDbType.NVarChar, 200, eOfficers.EmailId)
        data.params("@PermanentAddress", SqlDbType.NVarChar, 100, eOfficers.PermanentAddress)
        data.params("@PhoneCommunication", SqlDbType.Bit, eOfficers.PhoneCommunication)
        data.params("@HasLogin", SqlDbType.Bit, eOfficers.HasLogin)

        data.ExecuteCommand()
        eOfficers.OfficerID = data.sqlParameters("@OfficerId")
    End Sub

    'Corrected
    Public Sub UpdateOfficer(ByRef eOfficers As IMIS_EN.tblOfficer)

        Dim data As New ExactSQL

        data.setSQLCommand("INSERT INTO tblOfficer ([Code],[LastName],[OtherNames],[DOB],[Phone],[LocationId],[OfficerIDSubst],[WorksTo],[VEOCode],[VEOLastName],[VEOOtherNames],[VEODOB],[VEOPhone],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID], [EmailId],PhoneCommunication,PermanentAddress, HasLogin)" _
      & " select [Code],[LastName],[OtherNames],[DOB],[Phone],[LocationId],[OfficerIDSubst],[WorksTo],[VEOCode],[VEOLastName],[VEOOtherNames],[VEODOB],[VEOPhone],[ValidityFrom],getdate(),@OfficerID,[AuditUserID], [EmailId],PhoneCommunication,PermanentAddress, HasLogin from tblOfficer where OfficerID = @OfficerID;" _
      & "UPDATE [tblOfficer] SET [Code] = @Code,[LastName] = @LastName,[OtherNames] = @OtherNames,[DOB] = @DOB,[Phone] = @Phone,[LocationId] = @LocationId,[OfficerIDSubst] = @OfficerIDSubst,[WorksTo] = @WorksTo" _
      & ",[VEOCode]=@VEOCode,[VEOLastName]=@VEOLastName,[VEOOtherNames]=@VEOOtherNames,[VEODOB]=@VEODOB,[VEOPhone]=@VEOPhone,[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID, EmailId = @EmailId,PhoneCommunication=@PhoneCommunication, PermanentAddress=@PermanentAddress, HasLogin = @HasLogin  WHERE OfficerID = @OfficerID", CommandType.Text)
        data.params("@OfficerID", SqlDbType.Int, eOfficers.OfficerID)
        data.params("@Code", SqlDbType.NVarChar, 8, eOfficers.Code)
        data.params("@LastName", SqlDbType.NVarChar, 100, eOfficers.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eOfficers.OtherNames)
        data.params("@DOB", SqlDbType.SmallDateTime, if(eOfficers.DOB Is Nothing, SqlTypes.SqlDateTime.Null, eOfficers.DOB))
        data.params("@Phone", SqlDbType.NVarChar, 50, eOfficers.Phone)
        data.params("@LocationId", SqlDbType.Int, eOfficers.tblLocations.LocationId)
        data.params("@OfficerIDSubst", SqlDbType.Int, if(eOfficers.tblOfficer2.OfficerID = 0, DBNull.Value, eOfficers.tblOfficer2.OfficerID))

        data.params("@WorksTo", SqlDbType.DateTime, if(eOfficers.WorksTo Is Nothing, SqlTypes.SqlDateTime.Null, eOfficers.WorksTo))

        data.params("@VEOCode", SqlDbType.NVarChar, 25, eOfficers.VEOCode)
        data.params("@VEOLastName", SqlDbType.NVarChar, 100, eOfficers.VEOLastName)
        data.params("@VEOOtherNames", SqlDbType.NVarChar, 100, eOfficers.VEOOtherNames)
        data.params("@VEOPhone", SqlDbType.NVarChar, 25, eOfficers.VEOPhone)
        data.params("@VEODOB", SqlDbType.Date, if(eOfficers.VEODOB Is Nothing, SqlTypes.SqlDateTime.Null, eOfficers.VEODOB))
        data.params("@AuditUserID", SqlDbType.Int, eOfficers.AuditUserID)
        data.params("@Emailid", SqlDbType.NVarChar, 200, eOfficers.EmailId)
        data.params("@PhoneCommunication", SqlDbType.Bit, eOfficers.PhoneCommunication)
        data.params("@PermanentAddress", SqlDbType.NVarChar, 100, eOfficers.PermanentAddress)
        data.params("@HasLogin", SqlDbType.Bit, eOfficers.HasLogin)
        data.ExecuteCommand()

    End Sub
    Public Function ValidOfficerCode(ByVal OfficerCode As String) As Integer
        Dim data As New ExactSQL
        Dim sSQL As String = String.Empty
        sSQL = "SELECT OfficerID FROM tblOfficer WHERE Code = @OfficerCode"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@OfficerCode", SqlDbType.NVarChar, 8, OfficerCode)

        Dim dt As New DataTable
        dt = data.Filldata
        If dt.Rows.Count > 0 Then
            Return data.Filldata.Rows(0)("OfficerID")
        Else
            Return -1
        End If
    End Function
    Public Function getEnrollmentOfficerMoved(ByVal OfficerID As Integer) As IMIS_EN.tblOfficer
        Dim data As New ExactSQL
        Dim sSQL As String = String.Empty
        Dim eOfficer As New IMIS_EN.tblOfficer
        sSQL = "SELECT TOP 1 OfficerId, code FROM tblOfficer where OfficerId=@OfficerId"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@OfficerId", SqlDbType.Int, OfficerID)

        Dim dt As DataTable
        dt = data.Filldata

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            eOfficer.OfficerID = dt(0)("OfficerId")
            eOfficer.Code = dt(0)("Code")
        End If
        Return eOfficer
    End Function

    'Corrected
    Public Function GetSubstitutionOfficer(ByVal OfficerId As Integer) As DataTable

        Dim data As New ExactSQL
        Dim sSQL As String = String.Empty

        sSQL = ";WITH Villages AS"
        sSQL += " ("
        sSQL += " SELECT OV.LocationId"
        sSQL += " FROM tblOfficer OMain"
        sSQL += " INNER JOIN tblOfficerVillages OV ON OMain.OfficerId = OV.OfficerId AND OV.OfficerId = @OfficerId"
        sSQL += " WHERE OV.ValidityTo IS NULL"
        sSQL += " )"
        sSQL += " SELECT OfficerId, Code, OtherNames, LastName"
        sSQL += " FROM tblOfficer"
        sSQL += " WHERE LocationId IS NULL"
        sSQL += " AND OfficerId <> @OfficerId"
        sSQL += " UNION"
        sSQL += " SELECT DISTINCT RO.OfficerId, RO.Code, RO.OtherNames, RO.LastName"
        sSQL += " FROM tblOfficer O"
        sSQL += " INNER JOIN tblLocations L1 ON L1.LocationId = O.LocationId"
        sSQL += " INNER JOIN tblOfficer RO ON RO.LocationId = ISNULL(L1.ParentLocationId, L1.LocationId) AND RO.OfficerId <> @OfficerId"
        sSQL += " WHERE O.ValidityTo IS NULL"
        sSQL += " AND L1.ValidityTo IS NULL"
        sSQL += " UNION"
        sSQL += " SELECT O.OfficerId, O.Code, O.OtherNames, O.LastName"
        sSQL += " FROM tblOfficer O"
        sSQL += " INNER JOIN tblOfficerVillages OV ON O.OfficerId  = OV.OfficerId"
        sSQL += " INNER JOIN Villages ON Villages.LocationId = OV.LocationId AND O.OfficerId <> @OfficerId"
        sSQL += " GROUP BY O.OfficerId, O.Code, O.OtherNames, O.LastName"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@OfficerId", SqlDbType.Int, OfficerId)
        Return data.Filldata
    End Function

    Public Function OfficerCodeExists(ByVal OfficerCode As String) As Boolean
        Dim data As New ExactSQL
        Dim sSQL As String = String.Empty
        sSQL = "SELECT OfficerID FROM tblOfficer WHERE Code = @OfficerCode AND ValidityTo IS NULL"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@OfficerCode", SqlDbType.NVarChar, 8, OfficerCode)
        Dim dt As New DataTable
        dt = data.Filldata
        Return dt.Rows.Count > 0
    End Function

    Public Function GetOfficerIdByUUID(ByVal uuid As Guid) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select OfficerID from tblOfficer where OfficerUUID = @OfficerUUID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@OfficerUUID", SqlDbType.UniqueIdentifier, uuid)

        Return data.Filldata
    End Function
End Class
