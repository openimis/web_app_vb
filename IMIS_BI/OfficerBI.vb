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

Public Class OfficerBI
    Public UserRights As New IMIS_BL.UsersBL
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Return UserRights.CheckRights(Right, UserID)
    End Function
    Public Function SaveOfficer(ByRef eOfficer As IMIS_EN.tblOfficer, dtData As DataTable) As Integer
        Dim saveData As New IMIS_BL.OfficersBL
        Return saveData.SaveOfficers(eOfficer, dtData)
    End Function
    Public Function GetDistricts(ByVal userId As Integer, Optional ByVal showSelect As Boolean = False, Optional RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userId, showSelect, RegionId, True)
    End Function
    Public Function GetOfficers(ByVal DistrictId As Integer, ByVal showSelect As Boolean) As DataTable
        Dim getDataTable As New IMIS_BL.OfficersBL
        Return getDataTable.GetOfficers(DistrictId, showSelect)
    End Function
    Public Sub LoadOfficer(ByRef eOfficers As IMIS_EN.tblOfficer)
        Dim loadEntity As New IMIS_BL.OfficersBL
        loadEntity.LoadOfficer(eOfficers)
    End Sub
    Public Function GetWards(DistrictId As Integer, OfficerId As Integer) As DataTable
        Dim Ward As New IMIS_BL.LocationsBL
        Return Ward.GetWardsForOfficers(DistrictId, OfficerId)
    End Function
    Public Function GetVillages(DistrictId As Integer, OfficerId As Integer) As DataTable
        Dim Ward As New IMIS_BL.LocationsBL
        Return Ward.GetVillagesForOfficers(DistrictId, OfficerId)
    End Function
    Public Function GetSubstitutionOfficer(ByVal OfficerId As Integer) As DataTable
        Dim Officer As New IMIS_BL.OfficersBL
        Return Officer.GetSubstitutionOfficer(OfficerId)
    End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect, False)
    End Function
    Public Function GetLanguage() As DataTable
        Dim users As New IMIS_BL.GeneralBL
        Return users.GetLanguage
    End Function

    Public Function GetDistrictForHF(ByVal HFID As Integer, ByVal UserId As Integer) As DataTable
        Dim Loc As New IMIS_BL.LocationsBL
        Return Loc.GetDistrictForHF(HFID, UserId)
    End Function
    Public Function SaveUser(ByRef eUser As IMIS_EN.tblUsers) As Integer
        Dim users As New IMIS_BL.UsersBL
        Return users.SaveUser(eUser)
    End Function
    Public Function SaveUserDistricts(ByVal eUserDistricts As IMIS_EN.tblUsersDistricts) As Integer
        Dim users As New IMIS_BL.UsersBL
        Return users.SaveUserDistricts(eUserDistricts)
    End Function
    Public Function GetRoles(ByVal RoleId As Integer) As DataTable
        Dim getDataTable As New IMIS_BL.UsersBL
        Return getDataTable.getUserRoles(RoleId)
    End Function
    Public Sub LoadUsers(ByRef eUser As IMIS_EN.tblUsers)
        Dim User As New IMIS_BL.UsersBL
        User.LoadUsers(eUser)
    End Sub
    Public Function GetHFCodes(ByVal UserId As Integer, ByVal LocationId As Integer) As DataTable
        Dim hf As New IMIS_BL.HealthFacilityBL
        Return hf.GetHFCodes(UserId, LocationId)
    End Function

    Public Function RunPageSecurity(ByVal PageName As IMIS_EN.Enums.Pages, ByRef PageObj As System.Web.UI.Page) As Boolean
        Dim user As New IMIS_BL.UsersBL
        Return user.RunPageSecurity(PageName, PageObj)
    End Function
    Public Function getRoleId(ByVal session As Object) As Integer
        Dim user As New IMIS_BL.UsersBL
        Return user.getRoleId(session)
    End Function
    Public Function getRegions(UserId As Integer) As DataTable
        Dim Region As New IMIS_BL.LocationsBL
        Return Region.GetAllRegions(UserId, False)
    End Function
    Public Function DeleteUser(ByVal eUser As IMIS_EN.tblUsers) As Boolean
        Dim Del As New IMIS_BL.UsersBL
        Del.DeleteUser(eUser)
        Return True
    End Function
    Public Function GetOfficerIdByUUID(ByVal uuid As Guid) As Integer
        Dim Officer As New IMIS_BL.OfficersBL
        Return Officer.GetOfficerIdByUUID(uuid)
    End Function
    Public Function CheckIfUserExists(ByVal eUser As IMIS_EN.tblUsers) As DataTable
        Dim BL As New IMIS_BL.OfficersBL
        Return BL.CheckIfUserExists(eUser)
    End Function

End Class


