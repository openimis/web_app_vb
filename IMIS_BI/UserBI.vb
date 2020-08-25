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

Public Class UserBI

    Public UserRights As New IMIS_BL.UsersBL
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Return UserRights.CheckRights(Right, UserID)
    End Function
    Public Function GetLanguage() As DataTable
        Dim users As New IMIS_BL.GeneralBL
        Return users.GetLanguage
    End Function
    Public Function GetDistricts(ByVal userID As Integer, Authority As Integer) As DataTable
        Dim districts As New IMIS_BL.LocationsBL
        Return districts.GetDistrictsAll(userID, Authority:=Authority)
    End Function
    Public Function GetDistrictForHF(ByVal HFID As Integer, ByVal UserId As Integer) As DataTable
        Dim Loc As New IMIS_BL.LocationsBL
        Return Loc.GetDistrictForHF(HFID, UserId)
    End Function
    Public Function SaveUser(ByRef eUser As IMIS_EN.tblUsers, dtRoles As DataTable) As Integer
        Dim users As New IMIS_BL.UsersBL
        Return users.SaveUser(eUser, dtRoles)
    End Function
    Public Function SaveUserDistricts(ByVal eUserDistricts As IMIS_EN.tblUsersDistricts) As Integer
        Dim users As New IMIS_BL.UsersBL
        Return users.SaveUserDistricts(eUserDistricts)
    End Function

    Public Sub LoadUsers(ByRef eUser As IMIS_EN.tblUsers)
        Dim User As New IMIS_BL.UsersBL
        User.LoadUsers(eUser)
    End Sub
    Public Function GetHFCodes(ByVal UserId As Integer, ByVal LocationId As Integer) As DataTable
        Dim hf As New IMIS_BL.HealthFacilityBL
        Return hf.GetHFCodes(UserId, LocationId)
    End Function
    Public Sub TestTable()
        Dim test As New IMIS_BL.UsersBL
        test.TestTable()
    End Sub
    Public Function RunPageSecurity(ByVal PageName As IMIS_EN.Enums.Pages, ByRef PageObj As System.Web.UI.Page) As Boolean
        Dim user As New IMIS_BL.UsersBL
        Return user.RunPageSecurity(PageName, PageObj)
    End Function
    Public Function getRegions(UserId As Integer, Authority As Integer) As DataTable
        Dim Region As New IMIS_BL.LocationsBL
        Return Region.GetAllRegions(UserId, False, Authority:=Authority)
    End Function
    Public Function getRolesForUser(ByVal UserId As Integer, offline As Boolean, Authority As Integer) As DataTable
        Dim user As New IMIS_BL.UsersBL
        Return user.getRolesForUser(UserId, offline, Authority)
    End Function
    Public Function IsUserExists(ByVal UserID As Integer) As Boolean
        Dim User As New IMIS_BL.UsersBL
        Return User.IsUserExists(UserID)
    End Function
    Public Function GetUserIdByUUID(ByVal uuid As Guid) As Integer
        Dim User As New IMIS_BL.UsersBL
        Return User.GetUserIdByUUID(uuid)
    End Function
    Function GetUserDistricts(ByVal CurrenctUserID As Integer, ByVal SelectedUserID As Integer) As Integer
        Dim User As New IMIS_BL.UsersBL
        Return User.GetUserDistricts(CurrenctUserID, SelectedUserID)
    End Function
End Class
