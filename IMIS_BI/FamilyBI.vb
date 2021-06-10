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



Public Class FamilyBI


    Public Sub LoadFamily(ByRef eFamily As IMIS_EN.tblFamilies)
        Dim Family As New IMIS_BL.FamilyBL
        Family.LoadFamily(eFamily)
    End Sub
    Public Function GetDistricts(ByVal userId As Integer, Optional ByVal showSelect As Boolean = False, Optional ByVal RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userId, showSelect, RegionId)
    End Function
    Public Function GetDistrictsAll(ByVal userID As Integer, Optional RegionId As Integer = 0, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistrictsAll(userID, RegionId, showSelect)
    End Function
    Public Function GetWards(ByVal DistrictID As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Wards As New IMIS_BL.LocationsBL
        Return Wards.GetWards(DistrictID, showSelect)
    End Function
    Public Function GetVillages(ByVal WardId As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Villages As New IMIS_BL.LocationsBL
        Return Villages.GetVillages(WardId, showSelect)
    End Function
    Public Function GetGender() As DataTable
        Dim Gender As New IMIS_BL.GenderBL
        Return Gender.GetGenders()
    End Function
    Public Function GetYesNO() As DataTable
        Dim Marital As New IMIS_BL.GeneralBL
        Return Marital.GetYesNo
    End Function
    Public Function GetMaritalStatus() As DataTable
        Dim Marital As New IMIS_BL.GeneralBL
        Return Marital.GetMaritalStatus()
    End Function
    Public Sub SaveFamily(ByRef eFamily As IMIS_EN.tblFamilies)
        Dim Family As New IMIS_BL.FamilyBL
        Family.SaveFamily(eFamily)
    End Sub
    Public Function FamilyExists(ByVal CHFID As String) As Boolean
        Dim Family As New IMIS_BL.FamilyBL
        Return Family.FamilyExists(CHFID)
    End Function


    Public Function getOfficerID(ByVal ImageName As String) As Integer
        Dim Officer As New IMIS_BL.OfficersBL
        Return Officer.ValidOfficerCode(ExtractOfficerCode(ImageName))
    End Function

    Public Function ExtractCHFID(ByVal ImageName As String) As String
        Dim Family As New IMIS_BL.FamilyBL
        Return Family.ExtractCHFID(ImageName)
    End Function

    Public Function ExtractOfficerCode(ByVal ImageName As String) As String
        Dim Family As New IMIS_BL.FamilyBL
        Return Family.ExtractOfficerCode(ImageName)
    End Function

    Public Function ExtractDate(ByVal ImageName As String) As String
        Dim Family As New IMIS_BL.FamilyBL
        Return Family.ExtractDate(ImageName)
    End Function
    Public Function CheckCHFID(ByVal CHFID As String) As Boolean
        Dim insuree As New IMIS_BL.EscapeBL
        Return insuree.isValidInsuranceNumber(CHFID)
    End Function
    Public Function GetTypes() As DataTable
        Dim F As New IMIS_BL.FamilyBL
        Return F.GetTypes
    End Function
    Public Function GetProfession() As DataTable
        Dim Ins As New IMIS_BL.InsureeBL
        Return Ins.GetProfession
    End Function
    Public Function GetEducation() As DataTable
        Dim Ins As New IMIS_BL.InsureeBL
        Return Ins.GetEducation
    End Function

    Public Function GetSubsidy() As DataTable
        Dim F As New IMIS_BL.FamilyBL
        Return F.GetSubsidy
    End Function
    Public Function GetEthnicity() As DataTable
        Dim F As New IMIS_BL.FamilyBL
        Return F.GetEthnicity
    End Function
    Public Function GetTypeOfIdentity() As DataTable
        Dim I As New IMIS_BL.InsureeBL
        Return I.GetTypeOfIdentity
    End Function
    Public Function GetDistricts() As DataTable
        Dim D As New IMIS_BL.LocationsBL
        Return D.GetDistricts()
    End Function
    Public Function GetHFLevel() As DataTable
        Dim HF As New IMIS_BL.HealthFacilityBL
        Return HF.GetHFLevel(True)
    End Function
    Public Function GetFSPHF(DistrictId As Integer, HFLevel As String) As DataTable
        Dim HF As New IMIS_BL.HealthFacilityBL
        Return HF.GetFSPHF(DistrictId, HFLevel)
    End Function
    Public Function ExtractLatitude(ByVal ImageName As String) As String
        Dim Family As New IMIS_BL.FamilyBL
        Return Family.ExtractLatitude(ImageName)
    End Function
    Public Function ExtractLongitude(ByVal ImageName As String) As String
        Dim Family As New IMIS_BL.FamilyBL
        Return Family.ExtractLongitude(ImageName)
    End Function


    Public Function GetCurDistricts() As DataTable
        Dim D As New IMIS_BL.LocationsBL
        Return D.GetDistricts()
    End Function
    Public Function GetCurWards(DistrictId As Integer) As DataTable
        Dim W As New IMIS_BL.LocationsBL
        Return W.GetWards(DistrictId, True)
    End Function
    Public Function GetCurVillages(WardId As Integer) As DataTable
        Dim V As New IMIS_BL.LocationsBL
        Return V.GetVillages(WardId, True)
    End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect)
    End Function
    Public Function GetRegionsAll(UserId As Integer, Optional ShowSelect As Boolean = True) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetAllRegions(UserId, ShowSelect)
    End Function
    Public Function GetFamilyIdByUUID(ByVal uuid As Guid) As Integer
        Dim Family As New IMIS_BL.FamilyBL
        Return Family.GetFamilyIdByUUID(uuid)
    End Function
    Public Function GetFamilyUUIDByID(ByVal id As Integer) As Guid
        Dim Family As New IMIS_BL.FamilyBL
        Return Family.GetFamilyUUIDByID(id)
    End Function
End Class
