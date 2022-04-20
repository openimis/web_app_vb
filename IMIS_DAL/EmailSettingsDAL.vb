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

Public Class EmailSettingsDAL
    Public Function getEmailSettings() As DataTable
        Dim sSQL As String = "SELECT TOP 1 EmailId, CONVERT(NVARCHAR(200),DECRYPTBYPASSPHRASE(N'EmailSettings', EmailPassword)) EmailPassword, SMTPHost, Port ,EnableSSL, SenderDisplayName FROM tblEmailSettings"
        Dim data As New ExactSQL

        data.setSQLCommand(sSQL, CommandType.Text)

        Dim dt As DataTable = data.Filldata

        Return dt

    End Function

    Public Sub SaveEmailSettings(ByVal eEmailSettings As IMIS_EN.tblEmailSettings)
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "IF NOT EXISTS(SELECT 1 FROM tblEmailSettings)"
        sSQL += " INSERT INTO tblEmailSettings(EmailId, EmailPassword, SMTPHost, Port,EnableSSL)"
        sSQL += " VALUES(@EmailId, ENCRYPTBYPASSPHRASE(N'EmailSettings', @EmailPassword), @SMTPHost, @Port,@EnableSSL)"
        sSQL += " UPDATE tblEmailSettings SET EmailId = @EmailId, EmailPassword = ENCRYPTBYPASSPHRASE(N'EmailSettings', @EmailPassword), SMTPHost = @SMTPHost,  Port= @Port,EnableSSL=@EnableSSL;"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@EmailId", SqlDbType.NVarChar, 200, eEmailSettings.EmailId)
        data.params("@EmailPassword", SqlDbType.NVarChar, 200, eEmailSettings.EmailPassword)
        data.params("@SMTPHost", SqlDbType.NVarChar, 200, eEmailSettings.SMTPHost)
        data.params("@Port", SqlDbType.Int, eEmailSettings.Port)
        data.params("@EnableSSL", SqlDbType.Bit, eEmailSettings.EnableSSL)

        data.ExecuteCommand()
    End Sub

End Class
