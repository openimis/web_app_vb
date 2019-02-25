Public Class RoleBL

    Public Function GetRoles(erole As IMIS_EN.tblRole) As DataTable

        Dim DAL As New IMIS_DAL.RoleDAL
        Dim BL As New UsersBL
        Dim dtRoles As DataTable = DAL.GetRoles(erole)
        For Each row As DataRow In dtRoles.Rows
            If row("IsSystem") > 0 Then

                row("RoleName") = BL.ReturnRole(row("IsSystem"))
            End If
        Next
        Return dtRoles
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
