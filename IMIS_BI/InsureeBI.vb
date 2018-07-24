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

Imports System.Web

Public Class InsureeBI
    Public Sub GetFamilyHeadInfo(ByVal eFamily As IMIS_EN.tblFamilies)
        Dim getHeadInfo As New IMIS_BL.FamilyBL
        getHeadInfo.GetFamilyHeadInfo(eFamily)
    End Sub
    Public Sub LoadInsuree(ByRef eInsuree As IMIS_EN.tblInsuree)
        Dim Insuree As New IMIS_BL.InsureeBL
        Insuree.LoadInsuree(eInsuree)
    End Sub
    Public Function GetGender() As DataTable
        Dim Gender As New IMIS_BL.GeneralBL
        Return Gender.GetGender()
    End Function
    Public Function GetYesNO() As DataTable
        Dim Marital As New IMIS_BL.GeneralBL
        Return Marital.GetYesNo
    End Function
    Public Function GetMaritalStatus() As DataTable
        Dim Marital As New IMIS_BL.GeneralBL
        Return Marital.GetMaritalStatus()
    End Function
    Public Function SaveInsuree(ByVal eInsuree As IMIS_EN.tblInsuree, Activate As Boolean) As Integer
        Dim SaveData As New IMIS_BL.InsureeBL
        Return SaveData.SaveInsuree(eInsuree, Activate)

    End Function
    Public Function FetchNewImages(ByVal ImagePath As String, ByVal CHFID As String) As DataTable
        Dim Images As New IMIS_BL.InsureeBL
        Return Images.FetchNewImages(ImagePath, CHFID)
    End Function
    Public Sub UpdateImage(ByRef ePhotos As IMIS_EN.tblPhotos, Optional ByVal UpdateInDatabase As Boolean = True)
        Dim Images As New IMIS_BL.InsureeBL
        Images.UpdateImage(ePhotos, UpdateInDatabase)
    End Sub
    Public Function GetInsureeByCHFID(ByVal CHFID As String) As DataTable
        Dim Insuree As New IMIS_BL.InsureeBL
        Return Insuree.FindInsureeByCHFID(CHFID)
    End Function
    Public Function GetInsureeByCHFIDGrid(ByVal CHFID As String) As DataTable
        Dim Policy As New IMIS_BL.PolicyBL
        Return Policy.FindInsureeByCHFIDGrid(CHFID)
    End Function

    Public Function InsureeExists(ByVal eInsuree As IMIS_EN.tblInsuree) As Boolean
        Dim insuree As New IMIS_BL.InsureeBL
        Return insuree.InsureeExists(eInsuree)
    End Function
    Public Function CheckCHFID(ByVal CHFID As String) As Boolean
        Dim insuree As New IMIS_BL.EscapeBL
        Return insuree.isValidInsuranceNumber(CHFID)
    End Function
    Public Function GetRelations() As DataTable
        Dim Ins As New IMIS_BL.InsureeBL
        Return Ins.GetRelations
    End Function
    Public Function GetProfession() As DataTable
        Dim Ins As New IMIS_BL.InsureeBL
        Return Ins.GetProfession
    End Function
    Public Function GetEducation() As DataTable
        Dim Ins As New IMIS_BL.InsureeBL
        Return Ins.GetEducation
    End Function
    Public Function GetMaxMemberCount(ByVal FamilyId As Integer) As DataTable
        Dim Ins As New IMIS_BL.InsureeBL
        Return Ins.GetMaxMemberCount(FamilyId)
    End Function

    Public Function GetTypeOfIdentity() As DataTable
        Dim I As New IMIS_BL.InsureeBL
        Return I.GetTypeOfIdentity
    End Function

    Public Function GetDistricts(ByVal userId As Integer, Optional ByVal showSelect As Boolean = False, Optional ByVal RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userId, True, RegionId)
    End Function
    Public Function GetWards(DistrictId As Integer) As DataTable
        Dim W As New IMIS_BL.LocationsBL
        Return W.GetWards(DistrictId, True)
    End Function
    Public Function GetVillages(WardId As Integer) As DataTable
        Dim V As New IMIS_BL.LocationsBL
        Return V.GetVillages(WardId, True)
    End Function
    Public Function GetHFLevel() As DataTable
        Dim HF As New IMIS_BL.HealthFacilityBL
        Return HF.GetHFLevel(True)
    End Function

    Public Function GetFSPHF(DistrictId As Integer, HFLevel As String) As DataTable
        Dim HF As New IMIS_BL.HealthFacilityBL
        Return HF.GetFSPHF(DistrictId, HFLevel)
    End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect)
    End Function
    Public Function GetRegionsAll(UserId As Integer, Optional ShowSelect As Boolean = True) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetAllRegions(UserId, ShowSelect)
    End Function
    Public Function GetDistrictsAll(ByVal userID As Integer, Optional RegionId As Integer = 0, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistrictsAll(userID, RegionId, True)
    End Function
End Class
