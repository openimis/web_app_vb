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

Public Class LoginBI

    Public Sub GetDefaults(ByRef eDefaults As IMIS_EN.tblIMISDefaults)
        Dim def As New IMIS_BL.IMISDefaultsBL
        def.GetDefaults(eDefaults)
    End Sub

    Public Function GetLogin(ByVal eLogin As IMIS_EN.tblUsers) As DataTable
        Dim Login As New IMIS_BL.LoginBL
        Return Login.GetLogin(eLogin)
    End Function

    Public Function SaveOfflineHFID(ByVal eDefaults As IMIS_EN.tblIMISDefaults) As Integer
        Dim defaults As New IMIS_BL.IMISDefaultsBL
        Return defaults.SaveOfflineHFID(eDefaults)
    End Function

    Public Sub InsertLogTime(ByVal UserID As Integer, ByVal LogAction As Integer)
        Dim Login As New IMIS_BL.LoginBL
        Login.InsertLogTime(UserID, LogAction)
    End Sub
    Public Sub getHFCodeAndName(ByRef eHF As IMIS_EN.tblHF)
        Dim HF As New IMIS_BL.HealthFacilityBL
        HF.getHFCodeAndName(eHF)
    End Sub
    Public Function getControlsSettings() As DataTable
        Dim Components As New IMIS_BL.ComponentsBL
        Return Components.getControlsSettings
    End Function
    Public Function GetLanguages() As DataTable
        Dim lan As New IMIS_BL.LanguagesBL
        Return lan.GetLanguages()
    End Function
End Class
