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

Public Class ClaimAdministratorBI
    Public UserRights As New IMIS_BL.UsersBL
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Return UserRights.CheckRights(Right, UserID)
    End Function
    Public Function RunPageSecurity(ByVal PageName As IMIS_EN.Enums.Pages, ByRef PageObj As System.Web.UI.Page) As Boolean
        Dim user As New IMIS_BL.UsersBL
        Return user.RunPageSecurity(PageName, PageObj)
    End Function
    Public Function GetHFCodes(ByVal UserId As Integer, ByVal LocationId As Integer) As DataTable
        Dim hf As New IMIS_BL.HealthFacilityBL
        Return hf.GetHFCodes(UserId, LocationId)
    End Function
    Public Sub LoadClaimAdmin(ByRef eClaimAdmin As IMIS_EN.tblClaimAdmin)
        Dim BLClaimAdmin As New IMIS_BL.ClaimAdminBL
        BLClaimAdmin.LoadClaimAdmin(eClaimAdmin)
    End Sub
    Public Function SaveClaimAdmin(ByRef eClaimAdmin As IMIS_EN.tblClaimAdmin) As Integer
        Dim BLClaimAdmin As New IMIS_BL.ClaimAdminBL
        Return BLClaimAdmin.SaveClaimAdmin(eClaimAdmin)
    End Function
    Public Function SaveUserDistricts(ByVal eUserDistricts As IMIS_EN.tblUsersDistricts) As Integer
        Dim users As New IMIS_BL.UsersBL
        Return users.SaveUserDistricts(eUserDistricts)
    End Function
    Public Function SaveUser(ByRef eUser As IMIS_EN.tblUsers) As Integer
        Dim users As New IMIS_BL.UsersBL
        Return users.SaveUser(eUser)
    End Function
    Public Sub LoadUsers(ByRef eUser As IMIS_EN.tblUsers)
        Dim User As New IMIS_BL.UsersBL
        User.LoadUsers(eUser)
    End Sub
    Public Function GetLanguage() As DataTable
        Dim users As New IMIS_BL.GeneralBL
        Return users.GetLanguage
    End Function
    Public Function DeleteUser(ByVal eUser As IMIS_EN.tblUsers) As Boolean
        Dim Del As New IMIS_BL.UsersBL
        Del.DeleteUser(eUser)
        Return True
    End Function
    Public Function GetClaimAdminIdByUUID(ByVal uuid As Guid) As Integer
        Dim ClaimAdmin As New IMIS_BL.ClaimAdminBL
        Return ClaimAdmin.GetClaimAdminIdByUUID(uuid)
    End Function
    Public Function GetClaimAdminUUIDByID(ByVal id As Integer) As Guid
        Dim ClaimAdmin As New IMIS_BL.ClaimAdminBL
        Return ClaimAdmin.GetClaimAdminUUIDByID(id)
    End Function
    Public Function getHFUserLocation(ByVal UserId As Integer, ByVal Hfid As Integer) As DataTable
        Dim BL As New IMIS_BL.HealthFacilityBL
        Return BL.getHFUserLocation(UserId, Hfid)
    End Function
    Public Function CheckIfUserExists(ByVal eUser As IMIS_EN.tblUsers) As DataTable
        Dim BL As New IMIS_BL.ClaimAdminBL
        Return BL.CheckIfUserExists(eUser)
    End Function
End Class
