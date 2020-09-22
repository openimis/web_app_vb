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

Public Class RoleRightDAL

    Public Function GetRoleRights(RoleID As Integer) As DataSet

        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL
        sSQL = "SELECT RoleId,RoleName,isSystem,isBlocked,AltLanguage,ValidityTo From tblRole WHERE RoleID = @RoleID"
        sSQL += " SELECT [RoleRightID],[RoleID],[RightID],[ValidityFrom],[ValidityTo],[AuditUserId],[LegacyID]"
        sSQL += " FROM tblRoleRight WHERE RoleID = @RoleID and left(RightID,2) = 10 AND ValidityTo IS NULL"
        sSQL += " SELECT [RoleRightID],[RoleID],[RightID],[ValidityFrom],[ValidityTo],[AuditUserId],[LegacyID]"
        sSQL += " FROM tblRoleRight WHERE RoleID = @RoleID and left(RightID,2) = 11 AND ValidityTo IS NULL"
        sSQL += " SELECT [RoleRightID],[RoleID],[RightID],[ValidityFrom],[ValidityTo],[AuditUserId],[LegacyID]"
        sSQL += " FROM tblRoleRight WHERE RoleID = @RoleID and left(RightID,2) = 12 AND ValidityTo IS NULL"
        sSQL += " SELECT [RoleRightID],[RoleID],[RightID],[ValidityFrom],[ValidityTo],[AuditUserId],[LegacyID]"
        sSQL += " FROM tblRoleRight WHERE RoleID = @RoleID and left(RightID,2) > 12 AND ValidityTo IS NULL"


        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@RoleID", SqlDbType.Int, RoleID)
        Return data.FilldataSet

    End Function
    Public Function SaveRights(dtRights As DataTable, eRole As IMIS_EN.tblRole)
        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL

        sSQL = "INSERT INTO tblRoleright ([RoleID],[RightID],[ValidityFrom],[ValidityTo],[AuditUserId],[LegacyID])"
        sSQL += " SELECT [RoleID],[RightID],[ValidityFrom],GETDATE(),[AuditUserId],[RoleRightID] from tblRoleRight"
        sSQL += " WHERE RoleID = @RoleID AND RightID NOT IN (SELECT ID from @Rights) AND ValidityTo IS NULL"

        sSQL += " UPDATE tblRoleRight set ValidityTo = GETDATE(),AuditUserId =@AuditUserID"
        sSQL += " WHERE RoleID = @RoleID AND RightID NOT IN (SELECT ID from @Rights) AND ValidityTo IS NULL"

        sSQL += " INSERT INTO tblRoleright ([RoleID],[RightID],[ValidityFrom],[AuditUserId])"
        sSQL += " SELECT @RoleID,ID,GETDATE(),@AuditUserID FROM @Rights"
        sSQL += " WHERE ID NOT IN (SELECT RightID from tblRoleRight WHERE RoleID = @RoleID  AND ValidityTo IS NULL)"



        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@RoleID", SqlDbType.Int, eRole.RoleID)
        data.params("@AuditUserID", SqlDbType.Int, eRole.AuditUserID)
        data.params("@Rights", dtRights, "xAttribute")
        Return data.Filldata
    End Function
    Public Function GetRoleRight(ByRef eRoleRight As IMIS_EN.tblRoleRight) As DataTable

        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL

        sSQL = "SELECT [RoleRightID],[RoleID],[RightID],[ValidityFrom],[ValidityTo],[AuditUserId],[LegacyID] FROM tblRoleRight WHERE RoleRightID = @RoleRightID"


        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@RoleRightID", SqlDbType.Int, eRoleRight.RoleRightID)


        Dim dt As New DataTable
        dt = data.Filldata

        If dt.Rows.Count > 0 Then
            eRoleRight.RoleRightID = IIf(dt.Rows(0)("RoleRightID") Is DBNull.Value, Nothing, dt.Rows(0)("RoleRightID"))
            eRoleRight.RoleID = IIf(dt.Rows(0)("RoleID") Is DBNull.Value, Nothing, dt.Rows(0)("RoleID"))
            eRoleRight.RightID = IIf(dt.Rows(0)("RightID") Is DBNull.Value, Nothing, dt.Rows(0)("RightID"))
            eRoleRight.ValidityFrom = IIf(dt.Rows(0)("ValidityFrom") Is DBNull.Value, Nothing, dt.Rows(0)("ValidityFrom"))
            eRoleRight.ValidityTo = IIf(dt.Rows(0)("ValidityTo") Is DBNull.Value, Nothing, dt.Rows(0)("ValidityTo"))
            eRoleRight.AuditUserId = IIf(dt.Rows(0)("AuditUserId") Is DBNull.Value, Nothing, dt.Rows(0)("AuditUserId"))
            eRoleRight.LegacyID = IIf(dt.Rows(0)("LegacyID") Is DBNull.Value, Nothing, dt.Rows(0)("LegacyID"))
        End If

        Return dt
    End Function


    Public Sub InsertRoleRight(ByVal eRoleRight As IMIS_EN.tblRoleRight)

        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL

        sSQL = "INSERT INTO tblRoleRight(RoleID,RightID,ValidityFrom,ValidityTo,AuditUserId,LegacyID)VALUES(@RoleID,@RightID,@ValidityFrom,@ValidityTo,@AuditUserId,@LegacyID)"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@RoleID", SqlDbType.Int, eRoleRight.RoleID)
        data.params("@RightID", SqlDbType.Int, eRoleRight.RightID)
        data.params("@ValidityFrom", SqlDbType.DateTime, eRoleRight.ValidityFrom)
        data.params("@ValidityTo", SqlDbType.DateTime, eRoleRight.ValidityTo)
        data.params("@AuditUserId", SqlDbType.Int, eRoleRight.AuditUserId)
        data.params("@LegacyID", SqlDbType.Int, eRoleRight.LegacyID)


        data.ExecuteCommand()

    End Sub

    Public Sub UpdateRoleRight(ByVal eRoleRight As IMIS_EN.tblRoleRight)

        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL

        sSQL = "UPDATE tblRoleRight SET RoleID = @RoleID ,RightID = @RightID ,ValidityFrom = @ValidityFrom ,ValidityTo = @ValidityTo ,AuditUserId = @AuditUserId ,LegacyID = @LegacyID  WHERE RoleRightID = @RoleRightID"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@RoleRightID", SqlDbType.Int, eRoleRight.RoleRightID)
        data.params("@RoleID", SqlDbType.Int, eRoleRight.RoleID)
        data.params("@RightID", SqlDbType.Int, eRoleRight.RightID)
        data.params("@ValidityFrom", SqlDbType.DateTime, eRoleRight.ValidityFrom)
        data.params("@ValidityTo", SqlDbType.DateTime, eRoleRight.ValidityTo)
        data.params("@AuditUserId", SqlDbType.Int, eRoleRight.AuditUserId)
        data.params("@LegacyID", SqlDbType.Int, eRoleRight.LegacyID)


        data.ExecuteCommand()

    End Sub

    Public Sub DeleteRoleRight(ByVal eRoleRight As IMIS_EN.tblRoleRight)

        Dim sSQL As String = String.Empty
        Dim data As New ExactSQL

        sSQL = "DELETE FROM tblRoleRight  WHERE RoleRightID = @RoleRightID"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@RoleRightID", SqlDbType.Int, eRoleRight.RoleRightID)


        data.ExecuteCommand()

    End Sub

    '    Private Function isDirty() As Boolean
    'isDirty = True

    'If eRoleRight.RoleRightID.ToString() <> eRoleRightOrg.RoleRightID.ToString() Then Exit Function
    'If IIF(eRoleRight.RoleID Is Nothing, DbNull.value,eRoleRight.RoleID).ToString() <> IIF(eRoleRightOrg.RoleID Is Nothing, DbNull.value,eRoleRightOrg.RoleID).ToString() Then Exit Function
    'If IIF(eRoleRight.RightID Is Nothing, DbNull.value,eRoleRight.RightID).ToString() <> IIF(eRoleRightOrg.RightID Is Nothing, DbNull.value,eRoleRightOrg.RightID).ToString() Then Exit Function
    'If IIF(eRoleRight.ValidityFrom Is Nothing, DbNull.value,eRoleRight.ValidityFrom).ToString() <> IIF(eRoleRightOrg.ValidityFrom Is Nothing, DbNull.value,eRoleRightOrg.ValidityFrom).ToString() Then Exit Function
    'If IIF(eRoleRight.ValidityTo Is Nothing, DbNull.value,eRoleRight.ValidityTo).ToString() <> IIF(eRoleRightOrg.ValidityTo Is Nothing, DbNull.value,eRoleRightOrg.ValidityTo).ToString() Then Exit Function
    'If IIF(eRoleRight.AuditUserId Is Nothing, DbNull.value,eRoleRight.AuditUserId).ToString() <> IIF(eRoleRightOrg.AuditUserId Is Nothing, DbNull.value,eRoleRightOrg.AuditUserId).ToString() Then Exit Function
    'If IIF(eRoleRight.LegacyID Is Nothing, DbNull.value,eRoleRight.LegacyID).ToString() <> IIF(eRoleRightOrg.LegacyID Is Nothing, DbNull.value,eRoleRightOrg.LegacyID).ToString() Then Exit Function

    'isDirty = False
    'End Function

End Class
