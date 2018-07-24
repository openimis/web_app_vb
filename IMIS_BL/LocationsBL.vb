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

Public Class LocationsBL
    Private imisgen As New GeneralBL
    Public Function GetDistricts(ByVal userID As Integer, ByVal showSelect As Boolean, ByVal RegionId As Integer, Optional EnforceSelect As Boolean = False) As DataTable
        Dim Districts As New IMIS_DAL.LocationsDAL
        Dim dt As DataTable = Districts.GetDistricts(userID, RegionId:=RegionId)

        If dt.Rows.Count > 1 Or EnforceSelect Then
            If showSelect = True Then
                Dim dr As DataRow = dt.NewRow
                dr("DistrictId") = 0
                dr("DistrictName") = imisgen.getMessage("T_SELECTDISTRICT")
                dt.Rows.InsertAt(dr, 0)
            End If
        End If
        Return dt
    End Function
    Public Function GetDistrictsAll(ByVal userID As Integer, Optional RegionId As Integer = 0, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Districts As New IMIS_DAL.LocationsDAL
        Dim dt As DataTable = Districts.GetDistrictsALL(userID, RegionId)
        If dt.Rows.Count > 1 Then
            If showSelect = True Then
                Dim dr As DataRow = dt.NewRow
                dr("DistrictId") = 0
                dr("DistrictName") = imisgen.getMessage("T_SELECTDISTRICT")
                dt.Rows.InsertAt(dr, 0)
            End If
        End If
        Return dt
    End Function
    Public Function GetDistrictForHF(ByVal HFID As Integer, ByVal UserId As Integer) As DataTable
        Dim Districts As New IMIS_DAL.LocationsDAL
        Return Districts.GetDistrictForHF(HFID, UserId)
    End Function
    Public Function GetVillages(ByVal WardID As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Villages As New IMIS_DAL.LocationsDAL
        Dim dt As DataTable = Villages.GetVillages(WardID)
        If dt.Rows.Count > 1 Then
            If showSelect = True Then
                Dim dr As DataRow = dt.NewRow
                dr("VillageId") = 0
                dr("VillageName") = imisgen.getMessage("T_SELECTAVILLAGE")
                dt.Rows.InsertAt(dr, 0)
            End If
        End If
        Return dt
    End Function
    Public Function GetWards(ByVal DistrictID As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim Wards As New IMIS_DAL.LocationsDAL
        Dim dt As DataTable = Wards.GetWards(DistrictID)
        If dt.Rows.Count > 1 Then
            If showSelect = True Then
                Dim dr As DataRow = dt.NewRow
                dr("WardId") = 0
                dr("WardName") = imisgen.getMessage("T_SELECTAWARD")
                dt.Rows.InsertAt(dr, 0)

            End If
        End If

        Return dt
    End Function

    Public Sub SaveLocation(ByVal eLocations As IMIS_EN.tblLocations)
        Dim Locations As New IMIS_DAL.LocationsDAL
        If eLocations.LocationId = 0 Then
            Locations.SaveLocation(eLocations)
        Else
            Locations.UpdateLocation(eLocations)
        End If
    End Sub

    Public Function DeleteLocation(ByRef eLocations As IMIS_EN.tblLocations) As Integer
        Dim Districts As New IMIS_DAL.LocationsDAL
        Dim dt As DataTable = Districts.CheckCanBeDeleted(eLocations.LocationId)

        If dt.Rows.Count > 0 Then Return 1

        If Districts.DeleteLocation(eLocations) Then Return 2

        Return 3
    End Function

    Public Function UploadLocationsXML(ByVal File As String, ByVal StratergyId As Integer, ByVal AuditUserID As Integer, ByRef dtresult As DataTable, ByVal dryRun As Boolean, registerName As String, ByRef LogFile As String) As Dictionary(Of String, Integer)
        Dim Locations As New IMIS_DAL.LocationsDAL
        Dim dict As Dictionary(Of String, Integer) = Locations.UploadLocationsXML(File, StratergyId, AuditUserID, dtresult, dryRun)

        If dryRun = False Then
            LogFile = imisgen.CreateUploadRegisterLog(dtresult, registerName, StratergyId, System.IO.Path.GetFileName(File), imisgen.getLoginName(HttpContext.Current.Session("User")), "Locations")
        Else
            LogFile = String.Empty
        End If

        Return dict
    End Function

    'Public Sub SaveWards(ByRef eWards As IMIS_EN.tblLocations)
    '    Dim Wards As New IMIS_DAL.LocationsDAL
    '    If eWards.LocationId = 0 Then
    '        Wards.SaveWards(eWards)
    '    Else
    '        Wards.UpdateWards(eWards)
    '    End If
    'End Sub

    'Public Function DeleteWards(ByRef eWards As IMIS_EN.tblLocations) As Integer
    '    Dim Wards As New IMIS_DAL.LocationsDAL

    '    ' Dim dt As DataTable = Wards.CheckIfCanDeleteWard(eWards)
    '    ' If dt.Rows.Count > 0 Then Return 1

    '    If Wards.DeleteWards(eWards) Then
    '        Return 2
    '    End If
    '    Return 3
    'End Function
    'Public Function DeleteVillage(ByVal eVillage As IMIS_EN.tblLocations) As Integer
    '    Dim Village As New IMIS_DAL.LocationsDAL
    '    Dim dt As DataTable = Village.CheckIfCanDeleteVillage(eVillage)
    '    If dt.Rows.Count > 0 Then Return 1

    '    If Village.DeleteVillage(eVillage) Then
    '        Return 2
    '    End If
    '    Return 3
    'End Function
    'Public Sub SaveVillage(ByRef eVillage As IMIS_EN.tblLocations)
    '    Dim Village As New IMIS_DAL.LocationsDAL
    '    If eVillage.LocationId = 0 Then
    '        Village.SaveVillage(eVillage)
    '    Else
    '        Village.UpdateVillage(eVillage)
    '    End If
    'End Sub

    Public Function UploadLocations(ByVal DistrictsFile As String, ByVal WardsFile As String, ByVal VillagesFile As String) As Integer
        Dim Locations As New IMIS_DAL.LocationsDAL
        Return Locations.UploadLocations(DistrictsFile, WardsFile, VillagesFile)
    End Function

    Public Function GetDistricts(Optional ShowSelect As Boolean = True) As DataTable
        Dim D As New IMIS_DAL.LocationsDAL
        Dim dt As DataTable = D.GetDistricts

        Dim dr As DataRow = dt.NewRow
        dr("DistrictId") = 0
        dr("DistrictName") = imisgen.getMessage("T_SELECTDISTRICT")

        dt.Rows.InsertAt(dr, 0)

        Return dt
    End Function
    Public Function GetWardsForOfficers(DistrictId As Integer, OfficerId As Integer) As DataTable
        Dim Wards As New IMIS_DAL.LocationsDAL
        Return Wards.GetWardsForOfficers(DistrictId, OfficerId)
    End Function
    Public Function GetVillagesForOfficers(DistrictId As Integer, OfficerId As Integer)
        Dim Villages As New IMIS_DAL.LocationsDAL
        Return Villages.GetVillagesForOfficers(DistrictId, OfficerId)
    End Function
    Public Sub MoveLocation(ByVal SourceId As Integer, ByVal DestinationId As Integer, ByVal LocationType As String, ByRef AffectedFamilies As Integer, ByRef ErrorMessage As Integer, ByVal AuditUserId As Integer)
        Dim DAL As New IMIS_DAL.LocationsDAL
        DAL.MoveLocation(SourceId, DestinationId, LocationType, AffectedFamilies, ErrorMessage, AuditUserId)
    End Sub
    Public Function IsLocCodeUnique(ByVal LocCode As String) As Boolean
        Dim DAL As New IMIS_DAL.LocationsDAL
        Return DAL.IsLocCodeUnique(LocCode)
    End Function

    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True, Optional IncludeNational As Boolean = False) As DataTable
        Dim Region As New IMIS_DAL.LocationsDAL
        Dim imisgen As New GeneralBL
        Dim dt As DataTable = Region.GetRegions(UserId)
        If IncludeNational Then
            Dim dr As DataRow = dt.NewRow
            dr("RegionId") = -1
            dr("RegionName") = imisgen.getMessage("M_NATIONAL")
            dt.Rows.InsertAt(dr, 0)
        End If
        If ShowSelect = True And dt.Rows.Count > 1 Then
            Dim dr As DataRow
            dr = dt.NewRow
            dr("RegionId") = 0
            dr("RegionName") = imisgen.getMessage("T_SELECREGION")

            dt.Rows.InsertAt(dr, 0)
        End If
        Return dt
    End Function
    Public Function GetAllRegions(UserId As Integer, Optional ShowSelect As Boolean = True, Optional IncludeNational As Boolean = False) As DataTable
        Dim Region As New IMIS_DAL.LocationsDAL
        Dim imisgen As New GeneralBL
        Dim dt As DataTable = Region.getAllRegions(UserId)
        If IncludeNational Then
            Dim dr As DataRow = dt.NewRow
            dr("RegionId") = -1
            dr("RegionName") = imisgen.getMessage("M_NATIONAL")
            dt.Rows.InsertAt(dr, 0)
        End If
        If ShowSelect = True And dt.Rows.Count > 1 Then
            Dim dr As DataRow
            dr = dt.NewRow
            dr("RegionId") = 0
            dr("RegionName") = imisgen.getMessage("T_SELECREGION")

            dt.Rows.InsertAt(dr, 0)
        End If
        Return dt
    End Function
    Public Function GetUserRegions(UserId As Integer) As DataTable
        Dim Region As New IMIS_DAL.LocationsDAL
        Dim dt As DataTable = Region.GetUserRegions(UserId)
        Return dt
    End Function
    Public Function DownLoadLocationsXML() As String
        Dim DAL As New IMIS_DAL.LocationsDAL
        Dim dtLocations As New DataTable
        Dim ExportFolder As String
        Dim sXML As String = ""
        dtLocations = DAL.DownLoadLocationSXML
        For Each row As DataRow In dtLocations.Rows
            sXML += row(0).ToString
        Next
        Dim ICDXML As System.Xml.XmlDocument = New System.Xml.XmlDocument
        ICDXML.LoadXml(sXML)
        ExportFolder = Web.Configuration.WebConfigurationManager.AppSettings("ExportFolder").ToString()
        Dim FileName As String = "Locations" & Format(Now, "yyyyMMddHHmm") & ".xml"
        Dim path As String = HttpContext.Current.Server.MapPath(ExportFolder) & "\" & FileName
        ICDXML.Save(path)
        Return path
    End Function

    'Public Function SaveRegion(eRegions As IMIS_EN.tblLocations) As Boolean
    '    Dim Region As New IMIS_DAL.RegionsDAL
    '    If eRegions.LocationId = 0 Then
    '        Return Region.InsertRegion(eRegions)
    '    Else
    '        Return Region.UpdateRegion(eRegions)
    '    End If
    'End Function
    'Public Function DeleteRegions(ByRef eRegion As IMIS_EN.tblLocations) As Integer
    '    Dim Regions As New IMIS_DAL.RegionsDAL
    '    ' Dim dt As DataTable = Regions.CheckCanBeDeleted(eRegion.RegionId)
    '    '  If dt.Rows.Count > 0 Then Return 2

    '    If Regions.DeleteRegions(eRegion) Then Return 1

    '    Return 0
    'End Function
End Class
