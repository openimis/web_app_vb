Public Class RoleBL

    Public Function GetRoles(erole As IMIS_EN.tblRole) As DataTable

        Dim DAL As New IMIS_DAL.RoleDAL
        Return DAL.GetRoles(erole)
    End Function
    Public Sub DeleteRole(ByVal eRole As IMIS_EN.tblRole)
        Dim Role As New IMIS_DAL.RoleDAL
        Role.DeleteRole(eRole)
    End Sub
    Public Function IsRoleInUse(ByVal RoleID As Integer) As Boolean
        Dim Role As New IMIS_DAL.RoleDAL
        Return Role.IsRoleInUse(RoleID)
    End Function
End Class
