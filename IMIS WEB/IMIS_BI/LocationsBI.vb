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

Public Class LocationsBI
    Public UserRights As New IMIS_BL.UsersBL
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Return UserRights.CheckRights(Right, UserID)
    End Function
    Public Function GetDistricts(ByVal userId As Integer, Optional ByVal showSelect As Boolean = False, Optional RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userId, showSelect, RegionId:=RegionId)
    End Function
    Public Function GetWards(ByVal DistrictID As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Wards As New IMIS_BL.LocationsBL
        Return Wards.GetWards(DistrictID, showSelect)
    End Function
    Public Function GetVillages(ByVal WardId As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Villages As New IMIS_BL.LocationsBL
        Return Villages.GetVillages(WardId, showSelect)
    End Function
    'Public Function DeleteDistrict(ByVal DistrictId As Integer) As Boolean
    '    Return True
    'End Function
    'Public Function DeleteVillage(ByVal VillageId As Integer) As Boolean
    '    Return True
    'End Function
    'Public Function DeleteWard(ByVal WardId As Integer) As Boolean
    '    Return True
    'End Function
    Public Sub SaveLocation(ByVal eDistract As IMIS_EN.tblLocations)
        Dim Locations As New IMIS_BL.LocationsBL
        Locations.SaveLocation(eDistract)
    End Sub


    Public Function DeleteLocation(ByRef eDistrict As IMIS_EN.tblLocations) As Integer
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.DeleteLocation(eDistrict)
    End Function
    'Public Sub SaveVillage(ByRef eVillage As IMIS_EN.tblLocations)
    '    Dim Village As New IMIS_BL.LocationsBL
    '    Village.SaveVillage(eVillage)
    'End Sub
    'Public Sub SaveWard(ByRef eWard As IMIS_EN.tblLocations)
    '    Dim Wards As New IMIS_BL.LocationsBL
    '    Wards.SaveWards(eWard)
    'End Sub


    'Public Function DeleteWard(ByRef eWard As IMIS_EN.tblLocations) As Integer
    '    Dim Ward As New IMIS_BL.LocationsBL
    '    Return Ward.DeleteWards(eWard)
    'End Function

    'Public Function DeleteVillage(ByRef eVillage As IMIS_EN.tblLocations) As Integer
    '    Dim Village As New IMIS_BL.LocationsBL
    '    Return Village.DeleteVillage(eVillage)
    'End Function

    Public Function UploadLocations(ByVal DistrictsFile As String, ByVal WardsFile As String, ByVal VillagesFile As String) As Integer
        Dim Locations As New IMIS_BL.LocationsBL
        Return Locations.UploadLocations(DistrictsFile, WardsFile, VillagesFile)
    End Function
    'Public Function checkRoles(ByVal Role As IMIS_EN.Enums.Rights, ByVal roleid As Integer) As Boolean
    '    Dim roles As New IMIS_BL.UsersBL
    '    Return (roles.CheckRoles(Role, roleid))
    'End Function

    Public Function GetRegions(ByVal UserId As Integer) As DataTable
        Dim Regions As New IMIS_BL.LocationsBL
        Return Regions.GetRegions(UserId, False)
    End Function
    'Public Function SaveRegion(eRegions As IMIS_EN.tblLocations) As Boolean
    '    Dim Regions As New IMIS_BL.RegionsBL
    '    Return Regions.SaveRegion(eRegions)
    'End Function
    'Public Function DeleteRegions(ByRef eRegion As IMIS_EN.tblLocations) As Integer
    '    Dim Regions As New IMIS_BL.RegionsBL
    '    Return Regions.DeleteRegions(eRegion)
    'End Function
    Public Function IsLocCodeUnique(ByVal LocCode As String) As Boolean
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.IsLocCodeUnique(LocCode)
    End Function
End Class
