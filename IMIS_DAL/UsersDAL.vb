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

Public Class UsersDAL
    'Corrected
    Public Function getUsersDistricts(ByVal UserId As Integer)
        Dim data As New ExactSQL
        data.setSQLCommand("select DistrictName from tblUsers inner join tblUsersDistricts on tblUsers.UserID = " & _
        " tblUsersDistricts.UserID and tblUsers.ValidityTo is null and tblUsersDistricts.ValidityTo is null" & _
        " inner join tblDistricts on tblDistricts.DistrictID = tblUsersDistricts.LocationID and tbldistricts.validityto is null" & _
        " where(tblUsers.UserID = @UserId)", CommandType.Text)
        data.params("@UserId", SqlDbType.Int, UserId)
        Return data.Filldata
    End Function

    'Corrected
    Public Function GetUsers(ByVal eUser As IMIS_EN.tblUsers, ByVal All As Boolean, ByVal LocationId As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        'Dim strsql As String = "select distinct tblusers.* from tblUsers inner join tblUsersDistricts on  ISNULL(tblUsers.LegacyID,tblUsers.UserID) = tblUsersDistricts.UserID and tblUsersDistricts.ValidityTo is null inner join (select LocationId from tblUsersDistricts where UserID = @userId and ValidityTo is null) userDistricts on userdistricts.LocationId = tblUsersDistricts.LocationId WHERE LastName LIKE @LastName AND OtherNames LIKE @OtherNames AND LoginName LIKE @LoginName AND CASE WHEN @RoleID = 0 THEN 0 ELSE RoleID & @RoleId END = @RoleID AND  CASE WHEN @LanguageID = '-1' THEN '-1' ELSE LanguageID END = @LanguageID AND isnull(Phone,'')  like @Phone  AND ISNULL(EmailId,'') LIKE @EmailId"
        sSQL = " SELECT U.UserId, U.LanguageID, U.LastName, U.OtherNames, U.Phone, U.LoginName, U.RoleId, U.HFID, U.ValidityFrom, U.ValidityTo, U.LegacyId, U.AuditUserId, U.EmailId"
        sSQL += " FROM tblUsers U"
        sSQL += " INNER JOIN tblUsersDistricts UD ON UD.UserId = U.UserId"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(UD.LocationId, 0)"
        sSQL += " WHERE (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId = 0)"
        sSQL += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL)"

     
        sSQL += " AND UD.ValidityTo IS NULL"
        sSQL += " AND U.LastName LIKE @lastName"
        sSQL += " AND U.OtherNames LIKE @OtherNames"
        sSQL += " AND U.LoginName LIKE @LoginName"
        sSQL += " AND (U.RoleID & @RoleId = @RoleId OR @RoleId = 0)"
        sSQL += " AND (U.LanguageID = @languageId OR @languageId = '-1')"
        sSQL += " AND ISNULL(U.Phone, '') LIKE @Phone"
        sSQL += " AND ISNULL(U.EmailId, '') LIKE @EmailId"
        sSQL += " AND (U.HFID = @HFID OR @HFID = 0)"
   
        If All = False Then
            sSQL += " AND U.ValidityTo is null"
        End If

        sSQL += " GROUP BY U.UserId, U.LanguageID, U.LastName, U.OtherNames, U.Phone, U.LoginName, U.RoleId, U.HFID, U.ValidityFrom, U.ValidityTo, U.LegacyId,"
        sSQL += " U.AuditUserId, U.EmailId"
        sSQL += " ORDER BY U.LoginName, U.ValidityFrom DESC"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, eUser.AuditUserID)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@LastName", SqlDbType.NVarChar, 100, eUser.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eUser.OtherNames)
        data.params("@Phone", SqlDbType.NVarChar, 50, eUser.Phone)
        data.params("@RoleID", SqlDbType.Int, eUser.RoleID)
        data.params("@LoginName", SqlDbType.NVarChar, 100, eUser.LoginName)
        data.params("@LanguageID", SqlDbType.NVarChar, 2, eUser.LanguageID)
        data.params("@RegionId", SqlDbType.Int, eUser.tblLocations.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eUser.tblLocations.DistrictID)
        data.params("@HFID", SqlDbType.Int, If(eUser.HFID Is Nothing, 0, eUser.HFID))
        data.params("@EmailId", SqlDbType.NVarChar, 200, eUser.EmailId)
        Return data.Filldata
    End Function


    Public Function CheckIfUserExists(ByVal eUser As IMIS_EN.tblUsers) As DataTable
        Dim data As New ExactSQL
        Dim strSQL As String = "Select Top 1 * from tblUsers where LoginName = @LoginName and ValidityTo is null"

        If Not eUser.UserID = 0 Then
            strSQL += " AND tblUsers.UserId <> @UserId"
        End If

        data.setSQLCommand(strSQL, CommandType.Text)
        data.params("@LoginName", SqlDbType.NVarChar, 25, eUser.LoginName)
        If Not eUser.UserID = 0 Then
            data.params("@UserId", SqlDbType.Int, eUser.UserID)
        End If
        Return data.Filldata()
    End Function
    Public Sub DeleteUser(ByVal eUser As IMIS_EN.tblUsers)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblUsers ([LanguageID],[LastName],[OtherNames],[Phone],[LoginName],[RoleID],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID], [EmailId])" _
      & "select [LanguageID],[LastName],[OtherNames],[Phone],[LoginName],[RoleID],[ValidityFrom],getdate(),@UserID,[AuditUserID], [EmailId] from tblUsers where UserID = @UserID;" _
      & "UPDATE [tblUsers]   SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate(),[AuditUserID] = @AuditUserID  WHERE UserID = @UserID", CommandType.Text)
        data.params("@UserID", SqlDbType.Int, eUser.UserID)
        data.params("@AuditUserID", SqlDbType.Int, eUser.AuditUserID)
        data.ExecuteCommand()
    End Sub

    'Corrected
    Public Sub InsertUserDistricts(ByRef eUserDistricts As IMIS_EN.tblUsersDistricts)
        Dim data As New ExactSQL
        data.setSQLCommand("Insert into tblUsersDistricts ([UserId],[LocationId],[AuditUserID])" _
        & "VALUES(@UserId, @LocationId, @AuditUserID)", CommandType.Text)
        data.params("@UserId", SqlDbType.Int, eUserDistricts.tblUsers.UserID)
        data.params("@LocationId", SqlDbType.Int, eUserDistricts.tblLocations.LocationId)
        data.params("@AuditUserID", SqlDbType.Int, eUserDistricts.AuditUserID)



        data.ExecuteCommand()

    End Sub

    'Corrected
    Public Sub DeleteUserDistricts(ByRef eUserDistricts As IMIS_EN.tblUsersDistricts)
        Dim data As New ExactSQL
        data.setSQLCommand("Insert into tblUsersDistricts ([UserId],[LocationId],[ValidityTo],[ValidityFrom],[AuditUserID],[LegacyID])" _
      & "select [UserId],[LocationId],GETDATE(),[ValidityFrom],[AuditUserID],UserDistrictID from tblUsersDistricts where UserDistrictID = @UserDistrictID;" _
      & "UPDATE [tblUsersDistricts] set [ValidityFrom] = getdate() ,[ValidityTo] = getdate() ,[AuditUserID] = @AuditUserID  WHERE UserDistrictID = @UserDistrictID", CommandType.Text)
        data.params("@UserDistrictID", SqlDbType.Int, eUserDistricts.UserDistrictID)
        data.params("@AuditUserID", SqlDbType.Int, eUserDistricts.AuditUserID)
        data.ExecuteCommand()
    End Sub
    Public Sub InsertUser(ByRef eUser As IMIS_EN.tblUsers)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL =
               " Insert into tblUsers ([LastName],[OtherNames],[Phone],[LoginName],[RoleID],[LanguageID],[HFID],[AuditUserID],[EmailId],[StoredPassword],[PrivateKey])" &
               " VALUES(@LastName, @OtherNames, @Phone, @LoginName, @RoleId,@LanguageID,@HFID,@AuditUserID, @EmailId,@StoredPassword,@PrivateKey);select @UserId = scope_identity()"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, eUser.UserID, ParameterDirection.Output)
        data.params("@LastName", SqlDbType.NVarChar, 100, eUser.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eUser.OtherNames)
        data.params("@Phone", SqlDbType.NVarChar, 50, eUser.Phone)
        data.params("@LoginName", SqlDbType.NVarChar, 100, eUser.LoginName)
        data.params("@LanguageID", SqlDbType.NVarChar, 2, eUser.LanguageID)
        data.params("@RoleId", SqlDbType.Int, eUser.RoleID)
        data.params("@AuditUserID", SqlDbType.Int, eUser.AuditUserID)
        data.params("@HFID", SqlDbType.Int, eUser.HFID)
        data.params("@EmailId", SqlDbType.NVarChar, 200, eUser.EmailId)
        data.params("@StoredPassword", SqlDbType.NVarChar, 256, eUser.StoredPassword)
        data.params("@PrivateKey", SqlDbType.NVarChar, 256, eUser.PrivateKey)
        data.ExecuteCommand()
        eUser.UserID = data.sqlParameters("@UserId")
    End Sub
    Public Sub UpdateUser(ByRef eUser As IMIS_EN.tblUsers)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL =
                "INSERT INTO tblUsers ([LanguageID],[LastName],[OtherNames],[Phone],[LoginName],[RoleID],[HFID],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID],[EmailId],[StoredPassword],[PrivateKey])" _
                & " select [LanguageID],[LastName],[OtherNames],[Phone],[LoginName],[RoleID],[HFID],[ValidityFrom],getdate(),[UserID],[AuditUserID],[EmailId],[StoredPassword],[PrivateKey] from tblUsers where UserID = @UserID;" _
                & "UPDATE [tblUsers] SET [LanguageID] = @LanguageID,[LastName] = @LastName,[OtherNames] = @OtherNames,[Phone] = @Phone,[LoginName] = @LoginName,[RoleID] = @RoleID,[HFID] = @HFID, [EmailId] = @EmailId" _
                & ",[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID"
        If eUser.DummyPwd <> String.Empty Then
            sSQL += ",[StoredPassword] =@StoredPassword,[PrivateKey]=@PrivateKey"
        End If

        sSQL += " WHERE UserID = @UserID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserID", SqlDbType.Int, eUser.UserID)
        data.params("@LanguageID", SqlDbType.NVarChar, 2, eUser.LanguageID)
        data.params("@LastName", SqlDbType.NVarChar, 100, eUser.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eUser.OtherNames)
        data.params("@Phone", SqlDbType.NVarChar, 50, eUser.Phone)
        data.params("@LoginName", SqlDbType.NVarChar, 25, eUser.LoginName)
        data.params("@RoleID", SqlDbType.Int, eUser.RoleID)
        data.params("@HFID", SqlDbType.Int, eUser.HFID)
        data.params("@AuditUserID", SqlDbType.Int, eUser.AuditUserID)
        data.params("@EmailId", SqlDbType.NVarChar, 200, eUser.EmailId)
        data.params("@StoredPassword", SqlDbType.NVarChar, 256, eUser.StoredPassword)
        data.params("@PrivateKey", SqlDbType.NVarChar, 256, eUser.PrivateKey)
        data.ExecuteCommand()
    End Sub
    Public Sub LoadUsers(ByRef eUser As IMIS_EN.tblUsers)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = " select UserID,LanguageID,LastName,OtherNames,Phone,LoginName,RoleID,HFID,ValidityTo, EmailId,StoredPassword,PrivateKey,PasswordValidity from tblUsers"
        sSQL += " where (UserID= @UserID OR EmailId = @EmailId OR LoginName = @LoginName) AND ValidityTo IS NULL"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserID", SqlDbType.Int, eUser.UserID)
        data.params("@EmailId", SqlDbType.NVarChar, 300, eUser.EmailId)
        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            eUser.LanguageID = dr("LanguageID")
            eUser.LastName = dr("LastName")
            eUser.OtherNames = dr("OtherNames")
            eUser.Phone = If(dr.IsNull("Phone"), "", dr("Phone"))
            eUser.LoginName = dr("LoginName")
            eUser.DummyPwd = ""
            eUser.RoleID = dr("RoleID")
            If Not dr("HFID") Is DBNull.Value Then
                eUser.HFID = dr("HFID")
            End If
            If Not dr("ValidityTo") Is DBNull.Value Then
                eUser.ValidityTo = dr("ValidityTo").ToString
            End If
            eUser.EmailId = dr("EmailId").ToString
            eUser.PrivateKey = dr("PrivateKey").ToString
            eUser.StoredPassword = dr("StoredPassword").ToString
            If Not dr("PasswordValidity") Is DBNull.Value Then
                eUser.PasswordValidity = dr("PasswordValidity")
            End If
        End If

    End Sub
    Public Sub TestTable()

        Dim data As New ExactSQL

        data.setSQLCommand("uspTest", CommandType.StoredProcedure)


        Dim DT As New DataTable
        DT.Columns.Add("ItemID")
        DT.Columns.Add("OverrulePrice")

        Dim dr As DataRow = DT.NewRow
        dr("ItemID") = 1
        dr("OverRulePrice") = 10
        DT.Rows.Add(dr)

        dr = DT.NewRow()
        dr("ItemID") = 2
        dr("OverRulePrice") = 20
        DT.Rows.Add(dr)


        data.params("@tbl", DT)

        data.ExecuteCommand()

    End Sub
    Public Function GetUsers() As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select UserID,LoginName UserName from tblUsers where ValidityTo is null"

        data.setSQLCommand(sSQL, CommandType.Text)

        Return data.Filldata
    End Function
    Public Sub ChangePassword(ByVal eUser As IMIS_EN.tblUsers)

        Dim data As New ExactSQL

        Dim sSQL As String = ""
        sSQL = "INSERT INTO tblUsers ([LanguageID],[LastName],[OtherNames],[Phone],[LoginName],[RoleID],[HFID],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID],[EmailId],StoredPassword,PrivateKey)" _
                & " select [LanguageID],[LastName],[OtherNames],[Phone],[LoginName],[RoleID],[HFID],[ValidityFrom],getdate(),[UserID],[AuditUserID],[EmailId],StoredPassword,PrivateKey from tblUsers where UserID = @UserID;" _
                & "UPDATE [tblUsers] SET [PrivateKey] = @PrivateKey,StoredPassword=@StoredPassword,password = NULL" _
                & ",[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID  WHERE UserID = @UserID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserID", SqlDbType.Int, eUser.UserID)
        data.params("@StoredPassword", SqlDbType.NVarChar, 256, eUser.StoredPassword)
        data.params("@PrivateKey", SqlDbType.NVarChar, 256, eUser.PrivateKey)
        data.params("@AuditUserID", SqlDbType.Int, eUser.AuditUserID)
        data.ExecuteCommand()
    End Sub
    Public Sub UpdatePasswordValidity(ByVal eUser As IMIS_EN.tblUsers)

        Dim data As New ExactSQL

        Dim sSQL As String = ""
        sSQL = "INSERT INTO tblUsers ([LanguageID],[LastName],[OtherNames],[Phone],[LoginName],[RoleID],[HFID],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID],[EmailId],StoredPassword,PrivateKey,PasswordValidity)" _
                & " select [LanguageID],[LastName],[OtherNames],[Phone],[LoginName],[RoleID],[HFID],[ValidityFrom],getdate(),[UserID],[AuditUserID],[EmailId],StoredPassword,PrivateKey,PasswordValidity from tblUsers where UserID = @UserID;" _
                & "UPDATE [tblUsers] SET [PasswordValidity] = @PasswordValidity" _
                & ",[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID  WHERE UserID = @UserID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserID", SqlDbType.Int, eUser.UserID)
        data.params("@PasswordValidity", SqlDbType.DateTime, eUser.PasswordValidity)
        data.params("@AuditUserID", SqlDbType.Int, eUser.AuditUserID)
        data.ExecuteCommand()
    End Sub
End Class
