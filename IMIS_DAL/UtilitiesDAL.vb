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

Public Class UtilitiesDAL

    Public Sub CreateDatabaseBackup(ByVal Path As String, Optional ByVal Save As Boolean = False)
        Dim data As New ExactSQL
        Dim sSQL As String = "uspBackupDatabase"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        data.params("@Path", SqlDbType.NVarChar, 255, Path)
        data.params("@Save", SqlDbType.Bit, Save)

        data.ExecuteCommand()


    End Sub
    Public Function getDatabasePath() As DataTable
        Dim sSQL As String
        Dim data As New ExactSQL

        sSQL = "SELECT top 1 DatabaseBackupFolder,AppVersionBackEnd,ISNULL(OffLineHF,0) OffLineHF FROM tblIMISDefaults"

        data.setSQLCommand(sSQL, CommandType.Text)

        Return data.Filldata



    End Function

    Public Sub RestoreDatabases(ByVal Path As String, ByVal DBName As String)

        Dim sSQL As String = ""

        sSQL = "ALTER DATABASE [" & DBName & "] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE;" & _
               " RESTORE DATABASE [" & DBName & "] FROM DISK = @Path With Replace;" & _
               " ALTER DATABASE [" & DBName & "] SET MULTI_USER;" & _
               " EXEC [" & DBName & "].dbo.[SETUP-IMIS];"

        Dim data As New ExactSQL

        data.setSQLCommand(sSQL, CommandType.Text, "MasterConnectionString")

        data.params("@Path", SqlDbType.NVarChar, 255, Path)

        data.ExecuteCommand()


    End Sub
    Public Function GetCurrentDB() As String
        Dim sSQL As String = "SELECT DB_NAME()"
        Dim data As New ExactSQL
        data.setSQLCommand(sSQL, CommandType.Text)
        Return data.Filldata.Rows(0)(0).ToString
    End Function
    Public Sub SetupIMIS()
        Dim sSQL As String = "SETUP-IMIS"
        Dim data As New ExactSQL

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        data.ExecuteCommand()

    End Sub

    Public Function isValidScriptVersion(ByVal Version As String) As Boolean
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        Dim dt As New DataTable
        sSQL = "SELECT AppVersionBackEnd FROM tblIMISDefaults"

        data.setSQLCommand(sSQL, CommandType.Text)

        dt = data.Filldata

        If Convert.ToDecimal(Version) > Convert.ToDecimal(dt.Rows(0)("AppVersionBackEnd")) Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Sub ExecuteScript(ByVal Script As String)
        Dim data As New ExactSQL

        data.setSQLCommand(Script, CommandType.Text)

        data.ExecuteCommand()


    End Sub

    Public Sub UpdateBackEndVersion(ByVal version As String)
        Dim data As New ExactSQL
        Dim sSQL As String = ""

        sSQL = "UPDATE tblIMISDefaults SET AppVersionBackEnd = @Version"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@Version", version)

        data.ExecuteCommand()

    End Sub
End Class
