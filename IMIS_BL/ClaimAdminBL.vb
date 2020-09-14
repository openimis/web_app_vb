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

Public Class ClaimAdminBL
    Private DALClaimAdmin As New IMIS_DAL.ClaimAdminDAL
    Public Function GetClaimAdmins(ByVal eClaimAdmin As IMIS_EN.tblClaimAdmin, ByVal All As Boolean) As DataTable
        Return DALClaimAdmin.GetClaimAdmins(eClaimAdmin, All)
    End Function
    Public Function GetHFClaimAdminCodes(ByVal HFID As Integer, ByVal ShowSelect As Boolean) As DataTable
        Dim dt As DataTable = DALClaimAdmin.GetHFClaimAdminCodes(HFID)
        If dt.Rows.Count > 0 Then
            If ShowSelect = True Then
                Dim imisgen As New GeneralBL
                Dim dr As DataRow = dt.NewRow
                dr("ClaimAdminId") = 0
                dr("Description") = imisgen.getMessage("T_SELECTCLAIMADMIN")
                dt.Rows.InsertAt(dr, 0)
            End If
        End If
        Return dt
    End Function
    Public Function DeleteClaimAdmin(ByRef eClaimAdmin As IMIS_EN.tblClaimAdmin) As Integer
        '1 - deleted, -1 - unidentified problem
        If DALClaimAdmin.DeleteClaimAdmin(eClaimAdmin) Then Return 1
        Return -1
    End Function
    Public Sub LoadClaimAdmin(ByRef eClaimAdmin As IMIS_EN.tblClaimAdmin)
        DALClaimAdmin.LoadClaimAdmin(eClaimAdmin)
    End Sub
    Public Function SaveClaimAdmin(ByRef eClaimAdmin As IMIS_EN.tblClaimAdmin) As Integer
        '1 - inserted,2 - updated, 3 -  code exists, -1 - unidentified problem
        If DALClaimAdmin.ClaimAdminCodeExists(eClaimAdmin) Then Return 3
        If eClaimAdmin.ClaimAdminId = 0 Then
            If DALClaimAdmin.InsertClaimAdmin(eClaimAdmin) Then
                If eClaimAdmin.HasLogin Then
                    SaveClaimAdministratorUser(eClaimAdmin)
                End If
                Return 1
            End If
        Else
            Dim eClaimAdminOrg As New IMIS_EN.tblClaimAdmin
            eClaimAdminOrg.ClaimAdminId = eClaimAdmin.ClaimAdminId
            DALClaimAdmin.LoadClaimAdmin(eClaimAdminOrg)
            If (isDirtyClaimAdmin(eClaimAdmin, eClaimAdminOrg)) Then
                If DALClaimAdmin.UpdateClaimAdmin(eClaimAdmin) Then
                End If

            End If
            If eClaimAdmin.HasLogin Then
                SaveClaimAdministratorUser(eClaimAdmin)
            End If
            Return 2
        End If
        Return 0
    End Function

    Private Function isDirtyClaimAdmin(eClaimAdmin As IMIS_EN.tblClaimAdmin, eClaimAdminOrg As IMIS_EN.tblClaimAdmin) As Boolean
        isDirtyClaimAdmin = True
        If IIf(eClaimAdmin.LastName Is Nothing, DBNull.Value, eClaimAdmin.LastName).ToString() <> IIf(eClaimAdminOrg.LastName Is Nothing, DBNull.Value, eClaimAdminOrg.LastName).ToString() Then Exit Function
        If IIf(eClaimAdmin.OtherNames Is Nothing, DBNull.Value, eClaimAdmin.OtherNames).ToString() <> IIf(eClaimAdminOrg.OtherNames Is Nothing, DBNull.Value, eClaimAdminOrg.OtherNames).ToString() Then Exit Function
        If IIf(eClaimAdmin.Phone Is Nothing, DBNull.Value, eClaimAdmin.Phone).ToString() <> IIf(eClaimAdminOrg.Phone Is Nothing, DBNull.Value, eClaimAdminOrg.Phone).ToString() Then Exit Function
        If IIf(eClaimAdmin.EmailId Is Nothing, DBNull.Value, eClaimAdmin.EmailId).ToString() <> IIf(eClaimAdminOrg.EmailId Is Nothing, DBNull.Value, eClaimAdminOrg.EmailId).ToString() Then Exit Function
        If IIf(eClaimAdmin.ClaimAdminCode Is Nothing, DBNull.Value, eClaimAdmin.ClaimAdminCode).ToString() <> IIf(eClaimAdminOrg.ClaimAdminCode Is Nothing, DBNull.Value, eClaimAdminOrg.ClaimAdminCode).ToString() Then Exit Function
        If IIf(eClaimAdmin.DOB Is Nothing, DBNull.Value, eClaimAdmin.DOB).ToString() <> IIf(eClaimAdminOrg.DOB Is Nothing, DBNull.Value, eClaimAdminOrg.DOB).ToString() Then Exit Function
        If eClaimAdmin.tblHF.HfID <> eClaimAdminOrg.tblHF.HfID Then Exit Function
        If IIf(eClaimAdmin.LegacyId Is Nothing, DBNull.Value, eClaimAdmin.LegacyId).ToString() <> IIf(eClaimAdminOrg.LegacyId Is Nothing, DBNull.Value, eClaimAdminOrg.LegacyId).ToString() Then Exit Function
        If eClaimAdmin.ClaimAdminId <> eClaimAdminOrg.ClaimAdminId Then Exit Function
        If eClaimAdmin.HasLogin <> eClaimAdminOrg.HasLogin Then Exit Function
        If IIf(eClaimAdmin.ValidityFrom Is Nothing, DBNull.Value, eClaimAdmin.ValidityFrom).ToString() <> IIf(eClaimAdminOrg.ValidityFrom Is Nothing, DBNull.Value, eClaimAdminOrg.ValidityFrom).ToString() Then Exit Function

        isDirtyClaimAdmin = False
    End Function
    Public Function CheckIfUserExists(ByVal eUser As IMIS_EN.tblUsers) As DataTable
        Dim UserDAL As New IMIS_DAL.UsersDAL
        Return UserDAL.CheckIfUserExists(eUser)
    End Function
    Public Function SaveClaimAdministratorUser(eclaimAdmin As IMIS_EN.tblClaimAdmin) As Integer
        Dim BLUsers As New UsersBL
        Dim iReturn As Integer = -5
        If BLUsers.SaveUser(eclaimAdmin.eUsers) >= 0 Then

            Dim eUsersDistricts As New IMIS_EN.tblUsersDistricts
            Dim eLocations As New IMIS_EN.tblLocations
            Dim DALUserDistricts As New IMIS_DAL.UsersDistrictsDAL
            Dim DALHealthFacility As New IMIS_DAL.HealthFacilityDAL
            eUsersDistricts.AuditUserID = eclaimAdmin.eUsers.AuditUserID

            Dim dtOnlineAdminDistricts As DataTable = DALHealthFacility.getHFUserLocation(eclaimAdmin.AuditUserId, eclaimAdmin.tblHF.HfID)
            eLocations.LocationId = dtOnlineAdminDistricts.Rows(0)("DistrictId")
            eUsersDistricts.tblUsers = eclaimAdmin.eUsers
            eUsersDistricts.UserDistrictID = 0
            eUsersDistricts.tblLocations = eLocations
            BLUsers.SaveUserDistricts(eUsersDistricts)
        Else
            Return -4
        End If
        Return 0
    End Function
    Public Function GetClaimAdminIdByUUID(ByVal uuid As Guid) As Integer
        Dim ClaimAdmin As New IMIS_DAL.ClaimAdminDAL
        Return ClaimAdmin.GetClaimAdminIdByUUID(uuid).Rows(0).Item(0)
    End Function
    Public Function GetClaimAdminUUIDByID(ByVal id As Integer) As Guid
        Dim ClaimAdmin As New IMIS_DAL.ClaimAdminDAL
        Return ClaimAdmin.GetClaimAdminUUIDByID(id).Rows(0).Item(0)
    End Function
End Class
