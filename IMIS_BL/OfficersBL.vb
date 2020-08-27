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

Imports System

Public Class OfficersBL
    Private imisgen As New GeneralBL
    Public Sub DeleteOfficer(ByRef eOfficers As IMIS_EN.tblOfficer)
        Dim Officer As New IMIS_DAL.OfficersDAL
        Officer.DeleteOfficer(eOfficers)
    End Sub
    Public Function GetOfficers(ByVal DistrictId As Integer, ByVal showselect As Boolean, Optional ByVal VillageId As Integer = 0, Optional WorksTo As Date? = Nothing) As DataTable
        Dim getDataTable As New IMIS_DAL.OfficersDAL
        Dim dtOfficer As DataTable = getDataTable.GetOfficers(DistrictId, VillageId, WorksTo)

        If showselect = True And dtOfficer.Rows.Count > 0 Then
            Dim dr As DataRow = dtOfficer.NewRow
            dr("OfficerID") = 0
            dr("Code") = imisgen.getMessage("T_SELECTOFFICER")
            dtOfficer.Rows.InsertAt(dr, 0)
        End If
        Return dtOfficer
    End Function

    Public Function GetOfficers(ByVal eOfficer As IMIS_EN.tblOfficer, Optional ByVal Legacy As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_DAL.OfficersDAL
        eOfficer.Code += "%"
        eOfficer.LastName += "%"
        eOfficer.OtherNames += "%"
        eOfficer.Phone += "%"
        eOfficer.EmailId = "%" & eOfficer.EmailId & "%"
        Return getDataTable.GetOfficers(eOfficer, Legacy)


    End Function
    Public Function SaveOfficers(ByRef eOfficers As IMIS_EN.tblOfficer, dtData As DataTable) As Integer
        Dim SaveData As New IMIS_DAL.OfficersDAL
        Dim dt As DataTable = SaveData.CheckIfOfficerExists(eOfficers)
        If dt.Rows.Count > 0 Then Return 1
        If eOfficers.OfficerID = 0 Then
            SaveData.InsertOfficer(eOfficers)
            SaveOfficersVillages(dtData, eOfficers.OfficerID)
            If eOfficers.HasLogin = True Then
                SaveOfficerUser(eOfficers)
            End If
            Return 0
        Else
            Dim eOfficerOrgr As New IMIS_EN.tblOfficer
            eOfficerOrgr.OfficerID = eOfficers.OfficerID
            SaveData.LoadOfficer(eOfficerOrgr)
            SaveData.UpdateOfficer(eOfficers)
            SaveOfficersVillages(dtData, eOfficers.OfficerID)
            If eOfficers.HasLogin = True Then
                SaveOfficerUser(eOfficers)
            End If
            Return 2
        End If
    End Function
    Public Function SaveOfficerUser(eOfficer As IMIS_EN.tblOfficer) As Integer
        Dim BLUsers As New UsersBL
        Dim iReturn As Integer = -5
        If BLUsers.SaveUser(eOfficer.eUsers) >= 0 Then

            Dim eUsersDistricts As New IMIS_EN.tblUsersDistricts
            Dim eLocations As New IMIS_EN.tblLocations
            Dim DALUserDistricts As New IMIS_DAL.UsersDistrictsDAL

            eUsersDistricts.AuditUserID = eOfficer.eUsers.AuditUserID

            eLocations.LocationId = eOfficer.tblLocations.LocationId
            eUsersDistricts.tblUsers = eOfficer.eUsers
            eUsersDistricts.UserDistrictID = 0
            eUsersDistricts.tblLocations = eLocations
            BLUsers.SaveUserDistricts(eUsersDistricts)
        Else
            Return -4
        End If
        Return 0

    End Function
    Private Function isDirtyOfficer(eOfficer As IMIS_EN.tblOfficer, eOfficerOrg As IMIS_EN.tblOfficer) As Boolean
        isDirtyOfficer = True

        If IIf(eOfficer.LastName Is Nothing, DBNull.Value, eOfficer.LastName).ToString() <> IIf(eOfficerOrg.LastName Is Nothing, DBNull.Value, eOfficerOrg.LastName).ToString() Then Exit Function
        If IIf(eOfficer.OtherNames Is Nothing, DBNull.Value, eOfficer.OtherNames).ToString() <> IIf(eOfficerOrg.OtherNames Is Nothing, DBNull.Value, eOfficerOrg.OtherNames).ToString() Then Exit Function
        If IIf(eOfficer.Phone Is Nothing, DBNull.Value, eOfficer.Phone).ToString() <> IIf(eOfficerOrg.Phone Is Nothing, DBNull.Value, eOfficerOrg.Phone).ToString() Then Exit Function
        If IIf(eOfficer.EmailId Is Nothing, DBNull.Value, eOfficer.EmailId).ToString() <> IIf(eOfficerOrg.EmailId Is Nothing, DBNull.Value, eOfficerOrg.EmailId).ToString() Then Exit Function
        If eOfficer.OfficerID <> eOfficerOrg.OfficerID Then Exit Function
        If IIf(eOfficer.PermanentAddress Is Nothing, DBNull.Value, eOfficer.PermanentAddress).ToString() <> IIf(eOfficerOrg.PermanentAddress Is Nothing, DBNull.Value, eOfficerOrg.PermanentAddress).ToString() Then Exit Function
        If eOfficer.HasLogin <> eOfficerOrg.HasLogin Then Exit Function
        isDirtyOfficer = False
    End Function

    Private Sub SaveOfficersVillages(dtData As DataTable, OfficerId As Integer)
        If dtData Is Nothing OrElse dtData.Rows.Count = 0 Then Exit Sub
        Dim OV As New IMIS_DAL.OfficerVillagesDAL
        OV.SaveOfficerVillages(dtData, OfficerId)
    End Sub
    Public Sub LoadOfficer(ByRef eOfficers As IMIS_EN.tblOfficer)
        Dim load As New IMIS_DAL.OfficersDAL
        load.LoadOfficer(eOfficers)
    End Sub

    Public Function ValidOfficerCode(ByVal OfficerCode As String) As Integer
        Dim Officer As New IMIS_DAL.OfficersDAL
        Return Officer.ValidOfficerCode(OfficerCode)
    End Function
    Public Function getEnrollmentOfficerMoved(ByVal OfficerID As Integer) As IMIS_EN.tblOfficer
        Dim DAL As New IMIS_DAL.OfficersDAL
        Return DAL.getEnrollmentOfficerMoved(OfficerID)
    End Function
    Public Function GetSubstitutionOfficer(ByVal OfficerId As Integer) As DataTable
        Dim Officer As New IMIS_DAL.OfficersDAL
        Dim dtOfficer As DataTable = Officer.GetSubstitutionOfficer(OfficerId)
        If dtOfficer.Rows.Count > 0 Then
            Dim dr As DataRow = dtOfficer.NewRow
            dr("OfficerID") = 0
            dr("Code") = imisgen.getMessage("T_SELECTOFFICER")
            dtOfficer.Rows.InsertAt(dr, 0)
        End If

        Return dtOfficer
    End Function
    Public Function GetOfficerIdByUUID(ByVal uuid As Guid) As Integer
        Dim Officer As New IMIS_DAL.OfficersDAL
        Return Officer.GetOfficerIdByUUID(uuid).Rows(0).Item(0)
    End Function

    Public Function CheckIfUserExists(ByVal eUser As IMIS_EN.tblUsers) As DataTable
        Dim UserDAL As New IMIS_DAL.UsersDAL
        Return UserDAL.CheckIfUserExists(eUser)
    End Function
End Class
