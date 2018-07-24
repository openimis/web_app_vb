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

Public Class IMISExtractsBI

    'Public Function GetHealthFacility(Optional ByVal All As Boolean = False) As DataTable
    '    Dim HealthFacility As New IMIS_BL.HealthFacilityBL
    '    Return HealthFacility.GetHealthFacility(All)
    'End Function

    Public Function GetDistricts(ByVal userID As Integer, Optional ByVal showSelect As Boolean = False, Optional RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userID, showSelect, RegionId)
    End Function
    Public Function CreatePhoneExtracts(ByRef eExtractInfo As IMIS_EN.eExtractInfo, ByVal WithInsuree As Boolean) As String
        Dim Extract As New IMIS_BL.IMISExtractsBL
        Extract.CreatePhoneExtracts(eExtractInfo, WithInsuree)
        Return ""
    End Function
    Public Function CreateOffLineExtracts(ByRef eExtractInfo As IMIS_EN.eExtractInfo) As String
        Dim Extract As New IMIS_BL.IMISExtractsBL
        Extract.CreateOffLineExtracts(eExtractInfo)
        Return ""
    End Function
    Public Function ImportOffLineExtracts(ByRef eExtractInfo As IMIS_EN.eExtractInfo) As String
        Dim Extract As New IMIS_BL.IMISExtractsBL
        Extract.ImportOffLineExtracts(eExtractInfo)
        Return ""
    End Function
    Public Sub GetDefaults(ByRef eDefaults As IMIS_EN.tblIMISDefaults)
        Dim Defaults As New IMIS_BL.IMISDefaultsBL
        Defaults.GetDefaults(eDefaults)
    End Sub
    Public Function GetExtractList(ByVal LocationId As Integer, ByVal ExtractDirection As Integer, ByVal ExtractType As Integer) As DataTable
        Dim Extracts As New IMIS_BL.IMISExtractsBL
        Return Extracts.GetExtractList(LocationId, ExtractDirection, ExtractType)
    End Function
    Public Function GetDownLoadExtractInfo(Optional ByVal ExtractID As Integer = 0, Optional ByVal DistrictID As Integer = 0, Optional ByVal PhotoExtract As Boolean = False, Optional ByVal ExtractFileName As String = "") As String
        Dim Extract As New IMIS_BL.IMISExtractsBL
        Return Extract.GetDownLoadExtractInfo(ExtractID, DistrictID, PhotoExtract, ExtractFileName)
    End Function
    Public Function ImportOffLinePhotos(ByRef eExtractInfo As IMIS_EN.eExtractInfo) As Boolean
        Dim Extract As New IMIS_BL.IMISExtractsBL
        Extract.ImportOffLinePhotos(eExtractInfo)
        Return True
    End Function
    Public Function GetLastCreateExtractInfo(ByVal LocationId As Integer, ByVal ExtractType As Integer, Optional ByVal ExtractDirection As Integer = 0) As IMIS_EN.tblExtracts
        Dim Extract As New IMIS_BL.IMISExtractsBL
        Return Extract.GetLastCreateExtractInfo(LocationId, ExtractType, ExtractDirection)
    End Function
    Public Sub SubmitClaimFromXML(ByVal FileName As String)
        Dim Extracts As New IMIS_BL.IMISExtractsBL
        Extracts.SubmitClaimFromXML(FileName)
    End Sub
    Public Function checkRoles(ByVal Role As IMIS_EN.Enums.Rights, ByVal roleid As Integer) As Boolean
        Dim roles As New IMIS_BL.UsersBL
        Return (roles.CheckRoles(Role, roleid))
    End Function
    Public Sub CreateEnrolmentXML(ByVal Output As Dictionary(Of String, Integer), Optional ByVal isBackp As Boolean = False)
        Dim Ext As New IMIS_BL.IMISExtractsBL
        Ext.CreateEnrolmentXML(Output, isBackp)
    End Sub
    Public Function UploadEnrolments(ByVal FileName As String, ByVal Output As Dictionary(Of String, Integer)) As DataTable
        Dim Ext As New IMIS_BL.IMISExtractsBL
        Return Ext.UploadEnrolments(FileName, Output)
    End Function
    Public Sub UpdateOfflineUserDistrict(HFID As Integer)
        Dim UD As New IMIS_BL.UsersDistrictsBL
        UD.UpdateOfflineUserDistrict(HFID)
    End Sub
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True, Optional ByVal IncludeNational As Boolean = False) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect, IncludeNational)
    End Function
    Public Function NewSequenceNumber(ByVal LocationId As Integer) As Integer
        Dim Ext As New IMIS_BL.IMISExtractsBL
        Return Ext.NewSequenceNumber(LocationId)
    End Function
End Class
