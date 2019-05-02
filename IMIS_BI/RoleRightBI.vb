Public Class RoleRightBI
    Public UserRights As New IMIS_BL.UsersBL
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Return UserRights.CheckRights(Right, UserID)
    End Function
    Public Function IsRoleNameUnique(ByVal roleName As String) As Boolean
        Dim RoleRightBL As New IMIS_BL.RoleRightBL
        Return RoleRightBL.IsRoleNameUnique(roleName)
    End Function
    Public Function GetRoleRights(RoleID As Integer) As DataSet

        Dim RoleRightBL As New IMIS_BL.RoleRightBL

        Return RoleRightBL.GetRoleRights(RoleID)

    End Function
    Public Function SaveRights(dtRights As DataTable, eRole As IMIS_EN.tblRole) As Integer
        Dim RoleRightBL As New IMIS_BL.RoleRightBL
        Return RoleRightBL.SaveRights(dtRights, eRole)

    End Function
    Public Function RunPageSecurity(ByVal PageName As IMIS_EN.Enums.Pages, ByRef PageObj As System.Web.UI.Page) As Boolean
        Dim user As New IMIS_BL.UsersBL
        Return user.RunPageSecurity(PageName, PageObj)
    End Function
    'Public Function CheckRoles(ByVal Right As IMIS_EN.Enums.Rights, ByVal RoleId As Integer) As Boolean
    '    Dim user As New IMIS_BL.UsersBL
    '    Return user.CheckRoles(Right, RoleId)
    'End Function
End Class
