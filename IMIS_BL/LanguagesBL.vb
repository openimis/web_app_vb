Imports IMIS_DAL

Public Class LanguagesBL
    Public Function GetLanguages() As DataTable
        Dim dal As New LanguagesDAL
        Return dal.GetLanguages()
    End Function
End Class
