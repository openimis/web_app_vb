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

Public Class HealthFacilityBI
    Public Function GetDistricts(ByVal userId As Integer, Optional ByVal showSelect As Boolean = False, Optional ByVal RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userId, showSelect, RegionId)
    End Function

    Public Function GetHFType(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_BL.HealthFacilityBL
        Return getDataTable.GetHFType(showSelect)
    End Function
    Public Function GetHFLevel(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_BL.HealthFacilityBL
        Return getDataTable.GetHFLevel(showSelect)
    End Function
    Public Function GetHFLegal(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_BL.HealthFacilityBL
        Return getDataTable.GetHFLegal(showSelect)
    End Function
    Public Sub LoadHF(ByRef eHF As IMIS_EN.tblHF)
        Dim HF As New IMIS_BL.HealthFacilityBL
        HF.LoadHF(eHF)
    End Sub
    Public Function SaveHealthFacilities(ByVal eHF As IMIS_EN.tblHF, ByVal dtData As DataTable) As Integer
        Dim healthfacilites As New IMIS_BL.HealthFacilityBL
        Return healthfacilites.SaveHealthFacilities(eHF, dtData)
    End Function
    Public Function GetPLItems(ByVal UserId As Integer, ByVal RegionId As Integer, ByVal DistrictId As Integer, Optional ByVal ShowSelectedRow As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_BL.PriceListMIBL
        Return getDataTable.GetPriceListMI(UserId, RegionId, DistrictId, ShowSelectedRow)
    End Function
    Public Function GetPLServices(ByVal UserId As Integer, ByVal RegionId As Integer, ByVal DistrictId As Integer, Optional ByVal All As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_BL.PricelistMSBL
        Return getDataTable.GetPriceListMS(UserId, RegionId:=RegionId, DistrictId:=DistrictId, ShowSelectRow:=All)
    End Function
    Public Function GetSublevel() As DataTable
        Dim HF As New IMIS_BL.HealthFacilityBL
        Return HF.GetSublevel
    End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True, Optional ByVal IncludeNational As Boolean = False) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect, IncludeNational)
    End Function
    Public Function GetWards(DistrictId As Integer, OfficerId As Integer) As DataTable
        Dim Ward As New IMIS_BL.LocationsBL
        Return Ward.GetWardsForOfficers(DistrictId, OfficerId)
    End Function
    Public Function GetVillages(DistrictId As Integer, OfficerId As Integer) As DataTable
        Dim Ward As New IMIS_BL.LocationsBL
        Return Ward.GetVillagesForOfficers(DistrictId, OfficerId)
    End Function
    Public Function LoadCathmentRegions(HfId As Integer) As DataTable
        Dim BL As New IMIS_BL.HFCatchmentBL
        Return BL.LoadRegions(HfId)
    End Function
    Public Function LoadCatchmentDistricts(ByVal HfId As Integer) As DataTable
        Dim districts As New IMIS_BL.HFCatchmentBL
        Return districts.LoadDistricts(HfId, HfId)
    End Function
    Public Function LoadCatchmentWard(ByVal HfId As Integer) As DataTable
        Dim districts As New IMIS_BL.HFCatchmentBL
        Return districts.LoadWards(HfId)
    End Function
    Public Function LoadCatchmentVillage(ByVal HfId As Integer) As DataTable
        Dim districts As New IMIS_BL.HFCatchmentBL
        Return districts.LoaVilage(HfId)
    End Function
    Public Function GetAllWards(DistrictId As Integer, ByVal showSelect As Boolean) As DataTable
        Dim Ward As New IMIS_BL.LocationsBL
        Return Ward.GetWards(DistrictId, showSelect)
    End Function
    Public Function GetAllVillage(WardId As Integer, ByVal showSelect As Boolean) As DataTable
        Dim Ward As New IMIS_BL.LocationsBL
        Return Ward.GetVillages(WardId, showSelect)
    End Function
End Class
