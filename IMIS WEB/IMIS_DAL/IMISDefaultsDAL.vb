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

Public Class IMISDefaultsDAL
    Dim data As New ExactSQL
    Public Sub GetDefaults(ByRef eDefaults As IMIS_EN.tblIMISDefaults)


        data.setSQLCommand("SELECT [DefaultID],[PolicyRenewalInterval],[FTPHost],[FTPUser],[FTPPassword],[FTPPort],[FTPEnrollmentFolder],[FTPClaimFolder],[FTPFeedbackFolder],[FTPPolicyRenewalFolder],[FTPPhoneExtractFolder],[FTPOffLineExtractFolder],[AppVersionBackEnd],[AppVersionEnquire],[AppVersionEnroll],[AppVersionRenewal],[AppVersionFeedback],[AppVersionClaim],[OffLineHF],[OfflineCHF],[WinRarFolder],[DatabaseBackupFolder],[APIKey] FROM tblIMISDefaults", CommandType.Text)
        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            eDefaults.DefaultID = dr("DefaultID")
            eDefaults.PolicyRenewalInterval = dr("PolicyRenewalInterval")
            eDefaults.FTPHost = dr("FTPHost")
            eDefaults.FTPUser = dr("FTPUser")
            eDefaults.FTPPassword = dr("FTPPassword")
            eDefaults.FTPPort = dr("FTPPort")
            eDefaults.FTPEnrollmentFolder = dr("FTPEnrollmentFolder")
            eDefaults.FTPClaimFolder = dr("FTPClaimFolder")
            eDefaults.FTPFeedbackFolder = dr("FTPFeedbackFolder")
            eDefaults.FTPPolicyRenewalFolder = dr("FTPPolicyRenewalFolder")
            eDefaults.FTPPhoneExtractFolder = dr("FTPPhoneExtractFolder")
            eDefaults.FTPOffLineExtractFolder = dr("FTPOffLineExtractFolder")
            eDefaults.AppVersionBackEnd = dr("AppVersionBackEnd")
            eDefaults.AppVersionEnquire = dr("AppVersionEnquire")
            eDefaults.AppVersionEnroll = dr("AppVersionEnroll")
            eDefaults.AppVersionRenewal = dr("AppVersionRenewal")
            eDefaults.AppVersionFeedback = dr("AppVersionFeedback")
            eDefaults.AppVersionClaim = dr("AppVersionClaim")
            eDefaults.WinRarFolder = dr("WinRarFolder")
            eDefaults.OffLineHF = If(dr("OffLineHF") Is DBNull.Value, 0, dr("OffLineHF"))
            eDefaults.OfflineCHF = If(dr("OfflineCHF") Is DBNull.Value, 0, dr("OfflineCHF"))
            eDefaults.DatabaseBackupFolder = dr("DatabaseBackupFolder")
            eDefaults.APIKey = IIf(dr("APIKey") Is DBNull.Value, "", dr("APIKey"))
        End If

    End Sub

    Public Function SaveOfflineHFID(ByVal eDefaults As IMIS_EN.tblIMISDefaults) As Boolean
        Dim str As String = "UPDATE tblIMISDefaults SET [OfflineHF]=@OfflineHF, [OfflineCHF] = @OfflineCHF"
        data.setSQLCommand(str, CommandType.Text)
        data.params("@OfflineHF", SqlDbType.Int, eDefaults.OffLineHF)
        data.params("@OfflineCHF", SqlDbType.Int, eDefaults.OfflineCHF)
        data.ExecuteCommand()
        Return True
    End Function

End Class
