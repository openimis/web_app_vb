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
Public Class RoleDAL



    Public Function GetRoles(erole As IMIS_EN.tblRole) As DataTable

        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL

        sSQL = "SELECT [RoleID],[RoleName],CASE WHEN [IsSystem] > 0 THEN 'True' Else 'False' END System,"
        sSQL += " CASE WHEN [IsBlocked] = 1 THEN 'True' ELSE 'False' END Blocked,[ValidityFrom],[ValidityTo],[AuditUserID],[LegacyID] FROM tblRole"
        sSQL += " WHERE Rolename like '%" & erole.RoleName & "%'  AND (isBlocked = @isBlocked OR @isBlocked IS NULL)"
        If erole.IsSystem IsNot Nothing Then
            If erole.IsSystem = 1 Then
                sSQL += " AND isSystem > 0"
            Else
                sSQL += " AND isSystem = 0"
            End If
        End If
        If erole.LegacyID Is Nothing Then
            sSQL += " AND ValidityTo IS NULL"
        End If

        data.setSQLCommand(sSQL, CommandType.Text)
        'data.params("@RoleName", SqlDbType.NVarChar, 50, erole.RoleName)
        data.params("@IsSystem", SqlDbType.Bit, erole.IsSystem)
        data.params("@IsBlocked", SqlDbType.Bit, erole.IsBlocked)


        Return data.Filldata
    End Function
    Public Function GetRoles(offline As Boolean) As DataTable

        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL

        sSQL = "SELECT [RoleID],[RoleName],IsSystem From tblRole"
        sSQL += " WHERE ValidityTo IS NULL AND  isBlocked = 0"
        If offline = True Then
            sSQL += " AND (IsSystem = 0 OR IsSystem IN (524288, 525184, 1048584))"
        Else
            sSQL += " AND IsSystem NOT IN(524288, 525184, 1048584)"
        End If


        data.setSQLCommand(sSQL, CommandType.Text)



        Return data.Filldata
    End Function

    Public Function GetRole(ByRef eRole As IMIS_EN.tblRole) As Datatable

    Dim sSQL As String = String.Empty
    Dim data As new ExactSQL

    sSQL = "SELECT [RoleID],[RoleName],[IsSystem],[IsBlocked],[ValidityFrom],[ValidityTo],[AuditUserID],[LegacyID] FROM tblRole WHERE RoleID = @RoleID"


    data.setSQLCommand(sSQL, CommandType.Text)

    data.params("@RoleID",SqlDbType.int, eRole.RoleID)


    Dim dt as New DataTable
    dt = data.Filldata

    If dt.rows.count > 0 Then
    eRole.RoleID = IIF(dt.rows(0)("RoleID") IS DbNull.Value, Nothing,dt.rows(0)("RoleID"))
    eRole.RoleName =  dt.rows(0)("RoleName").ToString
    eRole.IsSystem = IIF(dt.rows(0)("IsSystem") IS DbNull.Value, Nothing,dt.rows(0)("IsSystem"))
    eRole.IsBlocked = IIF(dt.rows(0)("IsBlocked") IS DbNull.Value, Nothing,dt.rows(0)("IsBlocked"))
    eRole.ValidityFrom = IIF(dt.rows(0)("ValidityFrom") IS DbNull.Value, Nothing,dt.rows(0)("ValidityFrom"))
    eRole.ValidityTo = IIF(dt.rows(0)("ValidityTo") IS DbNull.Value, Nothing,dt.rows(0)("ValidityTo"))
    eRole.AuditUserID = IIF(dt.rows(0)("AuditUserID") IS DbNull.Value, Nothing,dt.rows(0)("AuditUserID"))
    eRole.LegacyID = IIF(dt.rows(0)("LegacyID") IS DbNull.Value, Nothing,dt.rows(0)("LegacyID"))
    End If

    Return dt
    End Function


    Public Sub InsertRole(ByVal eRole As IMIS_EN.tblRole)

    Dim sSQL As String = String.Empty
    Dim data As new ExactSQL

        sSQL = "INSERT INTO tblRole(RoleName,IsSystem,IsBlocked,ValidityFrom,AuditUserID)"
        sSQL += " VALUES(@RoleName,@IsSystem,@IsBlocked,GetDate(),@AuditUserID)"
        sSQL += " SELECT @RoleId = SCOPE_IDENTITY()"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@RoleID", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@RoleName", SqlDbType.NVarChar, 50, eRole.RoleName)
        data.params("@IsSystem", SqlDbType.Bit, eRole.IsSystem)
        data.params("@IsBlocked", SqlDbType.Bit, eRole.IsBlocked)
        data.params("@AuditUserID", SqlDbType.Int, eRole.AuditUserID)



        data.ExecuteCommand
        eRole.RoleID = data.sqlParameters("@RoleID")
    End Sub

    Public Sub UpdateRole(ByVal eRole As IMIS_EN.tblRole)

    Dim sSQL As String = String.Empty
        Dim data As New ExactSQL

        sSQL = "INSERT INTO tblRole(RoleName,IsSystem,IsBlocked,ValidityFrom,ValidityTo,AuditUserID,LegacyID)"
        sSQL += " SELECT RoleName,IsSystem,IsBlocked,ValidityFrom,GETDATE(),AuditUserID,RoleID FROM tblRole WHERE RoleID=@RoleID"
        sSQL += " UPDATE tblRole SET RoleName = @RoleName ,IsSystem = @IsSystem ,IsBlocked = @IsBlocked ,ValidityFrom ="
        sSQL += " GETDATE() ,AuditUserID = @AuditUserID WHERE RoleID = @RoleID"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@RoleID", SqlDbType.Int, eRole.RoleID)
        data.params("@RoleName", SqlDbType.NVarChar, 50, eRole.RoleName)
        data.params("@IsSystem",SqlDbType.bit, eRole.IsSystem)
        data.params("@IsBlocked", SqlDbType.Bit, eRole.IsBlocked)
        data.params("@AuditUserID",SqlDbType.int, eRole.AuditUserID)



        data.ExecuteCommand

    End Sub

    Public Sub DeleteRole(ByVal eRole As IMIS_EN.tblRole)

        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL
        'data.ExecuteCommand()
        sSQL = "INSERT INTO tblrole ([RoleName],[IsSystem],[IsBlocked],[ValidityFrom],[ValidityTo],[AuditUserID],[LegacyID])" _
             & " Select [RoleName],[IsSystem],[IsBlocked],[ValidityFrom],GetDate(),@RoleID,[LegacyID] from tblRole where RoleID = @RoleID;" _
             & " UPDATE [tblRole] SET [ValidityTo] = GetDate(),[AuditUserID] = @AuditUserID  WHERE RoleID = @RoleID"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@RoleID", SqlDbType.Int, eRole.RoleID)
        data.params("@AuditUserID", SqlDbType.Int, eRole.AuditUserID)
        data.ExecuteCommand()
    End Sub
    Public Function IsRoleInUse(ByVal RoleID As Integer) As Boolean
        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL
        sSQL = "SELECT  * FROM tblUserRole UR WHERE UR.RoleID = @RoleID and  UR.ValidityTo IS NULL"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@RoleID", SqlDbType.Int, RoleID)
        Dim dt As New DataTable
        dt = data.Filldata()
        If dt.Rows.Count > 0 Then
            Return True
        End If
        Return False
    End Function
    Public Function GetSystemRoles(RoleID) As DataTable
        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL
        sSQL = "SELECT RoleId,RoleName FROM tblRole"
        sSQL += " WHERE (IsSystem & @isSystem) > 0 AND ValidityTo IS NULL"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@isSystem", SqlDbType.Int, RoleID)
        Return data.Filldata()
    End Function

End Class