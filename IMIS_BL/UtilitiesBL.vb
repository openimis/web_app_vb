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

Imports System.IO
Imports System.Web

Public Class UtilitiesBL

    Public Sub CreateDatabaseBackup(ByVal Path As String, Optional ByVal Save As Boolean = False)
        Dim Utility As New IMIS_DAL.UtilitiesDAL
        Utility.CreateDatabaseBackup(Path, Save)
    End Sub

    Public Function getDatabasePath() As DataTable
        Dim Utility As New IMIS_DAL.UtilitiesDAL
        Return Utility.getDatabasePath()
    End Function

    Public Sub RestoreDatabases(ByVal Path As String)
        Dim Utility As New IMIS_DAL.UtilitiesDAL
        Utility.RestoreDatabases(Path, GetCurrentDB)
    End Sub
    Public Function GetCurrentDB() As String
        Dim Util As New IMIS_DAL.UtilitiesDAL
        Return Util.GetCurrentDB
    End Function
    Public Sub SetupIMIS()
        Dim Util As New IMIS_DAL.UtilitiesDAL
        Util.SetupIMIS()
    End Sub
    Public Function Execute(ByVal FilePath As String) As String
        Dim Gen As New GeneralBL
        Try
            Dim Utility As New IMIS_DAL.UtilitiesDAL
            Dim sScript As String = ""
            sScript = DecryptFile(FilePath)

            Dim Version As String = ExtractVersion(sScript)
            Dim Script As String = sScript.Substring(Version.Length, sScript.Length - Version.Length)

            If isValidScriptVersion(Version) Then

                Utility.ExecuteScript(Script)
                Utility.UpdateBackEndVersion(Version)
                DeleteScript(FilePath)
            Else
                DeleteScript(FilePath)

                Return Gen.getMessage("M_VERSION") '"Your backend version is higher than the script Gen."
            End If

            Return Gen.getMessage("M_SCRIPTEXECUTED") '"Script executed successfully."

        Catch ex As Exception
            Return Gen.getMessage("M_ERROROCCURED") '"Error occured while executing the script."
        End Try

    End Function

    Private Sub DeleteScript(ByVal FilePath As String)
        File.Delete(FilePath)
    End Sub

    Private Function ExtractVersion(ByVal Script As String) As String
        Return Script.Substring(0, Script.IndexOf(vbCrLf))
    End Function


    Public Function isValidScriptVersion(ByVal Version As String) As Boolean
        Dim Utility As New IMIS_DAL.UtilitiesDAL
        Return Utility.isValidScriptVersion(Version)

    End Function

    Public Function DecryptFile(ByVal FilePath As String) As String
        Dim Utitlity As New IMIS_BL.GeneralBL

        Return Utitlity.Decrypt(":-+A7V@=", FilePath)

    End Function



End Class
