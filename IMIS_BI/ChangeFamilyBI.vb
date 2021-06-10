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

Public Class ChangeFamilyBI
    Private Sub SaveFamily(ByVal eFamily As IMIS_EN.tblFamilies)

    End Sub
    Public Function ChangeHead(ByVal eInsureeOLD As IMIS_EN.tblInsuree, ByVal eInsureeNew As IMIS_EN.tblInsuree) As Boolean
        Dim InsureeBL As New IMIS_BL.InsureeBL
        Return InsureeBL.ChangeHead(eInsureeOLD, eInsureeNew)
    End Function
    Public Function MoveInsuree(ByVal eInsureeNew As IMIS_EN.tblInsuree, ByVal Activate As Boolean) As Boolean
        Dim InsureeBL As New IMIS_BL.InsureeBL
        Return InsureeBL.MoveInsuree(eInsureeNew, Activate)
    End Function
    Public Function GetYesNO() As DataTable
        Dim Marital As New IMIS_BL.GeneralBL
        Return Marital.GetYesNo
    End Function
    Public Function GetSubsidy() As DataTable
        Dim Poverty As New IMIS_BL.FamilyBL
        Return Poverty.GetSubsidy
    End Function
    Public Sub GetFamilyHeadInfo(ByVal eFamily As IMIS_EN.tblFamilies)
        Dim getHeadInfo As New IMIS_BL.FamilyBL
        getHeadInfo.GetFamilyHeadInfo(eFamily)
    End Sub
    Public Function GetDistricts(ByVal userId As Integer, Optional ByVal showSelect As Boolean = False, Optional ByVal RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userId, showSelect, RegionId)
    End Function
    Public Function GetWards(ByVal DistrictID As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Wards As New IMIS_BL.LocationsBL
        Return Wards.GetWards(DistrictID, showSelect)
    End Function
    Public Function GetVillages(ByVal WardId As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Villages As New IMIS_BL.LocationsBL
        Return Villages.GetVillages(WardId, showSelect)
    End Function
    Public Sub LoadFamily(ByRef eFamily As IMIS_EN.tblFamilies)
        Dim Family As New IMIS_BL.FamilyBL
        Family.LoadFamily(eFamily)
    End Sub

    Public Function UpdateChangeFamily(ByVal eFamily As IMIS_EN.tblFamilies) As Boolean
        Dim updateFamily As New IMIS_BL.FamilyBL
        Return updateFamily.UpdateFamily(eFamily)
    End Function


    Public Sub GetInsureesByCHFID(ByRef einsuree As IMIS_EN.tblInsuree)
        Dim insuree As New IMIS_BL.InsureeBL
        insuree.GetInsureesByCHFID(einsuree)
    End Sub
    Public Function GetTypes() As DataTable
        Dim F As New IMIS_BL.FamilyBL
        Return F.GetTypes
    End Function
    Public Function GetMaxMemberCount(ByVal FamilyId As Integer) As DataTable
        Dim Ins As New IMIS_BL.InsureeBL
        Return Ins.GetMaxMemberCount(FamilyId)
    End Function
    Public Function GetEthnicity() As DataTable
        Dim F As New IMIS_BL.FamilyBL
        Return F.GetEthnicity
    End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect)
    End Function
End Class
