Public Class RoleRightBL
    Public Function GetRoleRights(RoleID As Integer) As DataSet

        Dim RoleRightDAL As New IMIS_DAL.RoleRightDAL

        Return RoleRightDAL.GetRoleRights(RoleID)

    End Function
    Public Function SaveRights(dtRights As DataTable, eRole As IMIS_EN.tblRole) As Integer
        Dim RoleRightDAL As New IMIS_DAL.RoleRightDAL
        Dim RoleDAL As New IMIS_DAL.RoleDAL
        If eRole.RoleID = 0 Then
            RoleDAL.InsertRole(eRole)
        Else
            Dim eRoleOrg As New IMIS_EN.tblRole
            eRoleOrg.RoleID = eRole.RoleID
            RoleDAL.GetRole(eRoleOrg)
            If isDirty(eRole, eRoleOrg) Then
                RoleDAL.UpdateRole(eRole)
            End If

        End If
        RoleRightDAL.SaveRights(dtRights, eRole)
        Return 0
    End Function
    Private Function isDirty(eRole As IMIS_EN.tblRole, eRoleOrg As IMIS_EN.tblRole) As Boolean
        isDirty = True

        If eRole.RoleID.ToString() <> eRoleOrg.RoleID.ToString() Then Exit Function
        If IIf(eRole.RoleName Is Nothing, DBNull.Value, eRole.RoleName).ToString() <> IIf(eRoleOrg.RoleName Is Nothing, DBNull.Value, eRoleOrg.RoleName).ToString() Then Exit Function
        If IIf(eRole.IsSystem Is Nothing, False, eRole.IsSystem).ToString() <> IIf(eRoleOrg.IsSystem Is Nothing, False, eRoleOrg.IsSystem).ToString() Then Exit Function
        If IIf(eRole.IsBlocked Is Nothing, False, eRole.IsBlocked).ToString() <> IIf(eRoleOrg.IsBlocked Is Nothing, False, eRoleOrg.IsBlocked).ToString() Then Exit Function

        isDirty = False
    End Function
End Class
