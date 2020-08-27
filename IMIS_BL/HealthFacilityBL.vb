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

Imports System.Xml

Public Class HealthFacilityBL
    Private imisgen As New GeneralBL
    Public Sub DeleteHealthFacility(ByRef eHF As IMIS_EN.tblHF)
        Dim HF As New IMIS_DAL.HealthFacilityDAL
        HF.DeleteHealthFacility(eHF)
    End Sub
    'Public Function GetHealthFacility(Optional ByVal All As Boolean = True) As DataTable
    '    Dim HealthFacilities As New IMIS_DAL.HealthFacilityDAL
    '    Return HealthFacilities.GetHealthFacility(All)
    'End Function
    Public Function GetHealthFacility(ByVal eHF As IMIS_EN.tblHF, Optional ByVal All As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_DAL.HealthFacilityDAL
        'eHF.HFName += "%"
        'eHF.HFCode += "%"
        'eHF.Phone += "%"
        'eHF.Fax += "%"
        'eHF.eMail += "%"
        'eHF.HFCareType += "%"
        'eHF.HFLevel += "%"
        'eHF.LegalForm += "%"
        Dim dtHFLevel As DataTable = GetHFLevel()
        Dim dtHFLegal As DataTable = GetHFLegal()
        'If All = True Then
        '    Return getDataTable.GetHFLegacy(eHF)
        'Else
        Return getDataTable.GetHF(eHF, dtHFLevel, GetHFLegal, All, GetHFType)
        'End If
    End Function
    Public Function SaveHealthFacilities(ByRef eHF As IMIS_EN.tblHF, ByVal dtData As DataTable) As Integer
        Dim healthfacilities As New IMIS_DAL.HealthFacilityDAL
        If healthfacilities.CheckIfHFExists(eHF) = True Then Return 1
        If eHF.HfID = 0 Then
            healthfacilities.InsertHealthFacility(eHF)
            SaveHFCatchment(dtData, eHF.HfID, eHF.AuditUserID)
            Return 0
        Else
            healthfacilities.UpdateHealthFacility(eHF)
            SaveHFCatchment(dtData, eHF.HfID, eHF.AuditUserID)
            Return 2
        End If

    End Function
    Public Sub LoadHF(ByRef eHF As IMIS_EN.tblHF)
        Dim HF As New IMIS_DAL.HealthFacilityDAL
        HF.LoadHF(eHF)
    End Sub
    Public Function GetHFLevel(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim dtbl As New DataTable
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("HFLevel")
        Dim dr As DataRow = dtbl.NewRow
        If showSelect = True Then
            dr("Code") = ""
            dr("HFLevel") = imisgen.getMessage("T_SELECTLEVEL")
            dtbl.Rows.Add(dr)
        End If

        dr = dtbl.NewRow

        'dr("Code") = "S"
        'dr("HFLevel") = imisgen.getMessage("T_SUBHEALTPOST")
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = "P"
        'dr("HFLevel") = imisgen.getMessage("T_HEALTHPOST")
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = "C"
        'dr("HFLevel") = imisgen.getMessage("T_PRIMARYHEALTHCENTRE")
        'dtbl.Rows.Add(dr)


        dr("Code") = "D"
        dr("HFLevel") = imisgen.getMessage("T_DISPENSARY")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = "C"
        dr("HFLevel") = imisgen.getMessage("T_HEALTHCENTRE")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = "H"
        dr("HFLevel") = imisgen.getMessage("T_HOSPITAL")
        dtbl.Rows.Add(dr)



        Return dtbl
    End Function
    Public Function GetHFLegal(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim DAL As New IMIS_DAL.LegalFormDAL
        Dim dt As DataTable = DAL.GetLegalForm
        Dim dr As DataRow = Nothing
        If showSelect = True Then
            dr = dt.NewRow
            dr("LegalFormCode") = ""
            dr("LegalForms") = imisgen.getMessage("T_SELECTLEGALFORM")
            dr("AltLanguage") = imisgen.getMessage("T_SELECTLEGALFORM")
            dt.Rows.InsertAt(dr, 0)
        End If
        Return dt
    End Function
    Public Function GetHFType(Optional ByVal showSelect As Boolean = False, Optional ByVal InOutOnly As Boolean = False) As DataTable
        Dim dtbl As New DataTable
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("HFCareType")
        Dim dr As DataRow = dtbl.NewRow
        If showSelect = True Then
            dr("Code") = ""
            dr("HFCareType") = imisgen.getMessage("T_SELECTTYPE")
            dtbl.Rows.Add(dr)
        End If

        dr = dtbl.NewRow
        dr("Code") = "I"
        dr("HFCareType") = imisgen.getMessage("T_INPATIENT")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = "O"
        dr("HFCareType") = imisgen.getMessage("T_OUTPATIENT")
        dtbl.Rows.Add(dr)
        If InOutOnly = False Then
            dr = dtbl.NewRow
            dr("Code") = "B"
            dr("HFCareType") = imisgen.getMessage("T_BOTH")
            dtbl.Rows.Add(dr)

        End If

        Return dtbl
    End Function
    Public Function GetHFLevelType(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim dtbl As New DataTable
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("HFLevel")
        dtbl.Columns.Add("AltLanguage")
        Dim dr As DataRow = dtbl.NewRow
        If showSelect = True Then
            dr("Code") = ""
            dr("HFLevel") = imisgen.getMessage("T_SELECTCATEGORY")
            dtbl.Rows.Add(dr)
        End If

        dr = dtbl.NewRow
        dr("Code") = "I"
        dr("HFLevel") = imisgen.getMessage("T_HOSPITAL")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = "O"
        dr("HFLevel") = imisgen.getMessage("T_NONHOSPITAL")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = "B"
        dr("HFLevel") = imisgen.getMessage("L_GENERAL")
        dtbl.Rows.Add(dr)

        Return dtbl
    End Function
    Public Function GetHFCodes(ByVal UserId As Integer, ByVal LocationId As Integer) As DataTable
        Dim hf As New IMIS_DAL.HealthFacilityDAL
        Dim dthf As DataTable
        Dim dtrow As DataRow
        Dim hfId As Integer = 0
        dthf = hf.GetHFCodes(UserId, LocationId, hfId)
        If dthf.Rows.Count = 0 Then Return dthf
        If dthf.Rows.Count > 1 Or hfId = 0 Then
            dtrow = dthf.NewRow
            dtrow("HfID") = 0
            dtrow("HFCode") = imisgen.getMessage("T_SELECTHFCODE")
            dthf.Rows.InsertAt(dtrow, 0)
        End If
        Return dthf

    End Function
    Public Function getHFCodeFromID(ByVal HFID As Integer) As String
        Dim hf As New IMIS_DAL.HealthFacilityDAL
        Return hf.getHFCodeFromID(HFID)
    End Function
    Public Sub getHFCodeAndName(ByRef eHF As IMIS_EN.tblHF)
        Dim hf As New IMIS_DAL.HealthFacilityDAL
        hf.getHFCodeAndName(eHF)
    End Sub
    Public Function GetSublevel() As DataTable

        Dim DAL As New IMIS_DAL.HFSublevelDAL
        Dim dt As DataTable = DAL.GetHFSublevel
        Dim dr As DataRow = Nothing

        dr = dt.NewRow
        dr("HFSublevel") = ""
        dr("HFSublevelDesc") = imisgen.getMessage("T_SELECTSUBLEVEL")
        dr("AltLanguage") = imisgen.getMessage("T_SELECTSUBLEVEL")
        dt.Rows.InsertAt(dr, 0)

        Return dt


    End Function
    Public Function GetFSPHF(DistrictId As Integer, HFLevel As String) As DataTable
        Dim HF As New IMIS_DAL.HealthFacilityDAL
        Dim dt As DataTable = HF.GetFSPHF(DistrictId, HFLevel)
        If dt.Rows.Count > 1 Then
            Dim dr As DataRow = dt.NewRow
            dr("HFId") = 0
            dr("HFCode") = imisgen.getMessage("M_HealthFacility")
            dt.Rows.InsertAt(dr, 0)
        End If
        Return dt
    End Function
    Private Sub SaveHFCatchment(ByVal dtData As DataTable, ByVal HfId As Integer, ByVal AuditUserId As Integer)
        Dim DAL As New IMIS_DAL.HFCatchmentDAL
        DAL.SaveHFCatchment(dtData, HfId, AuditUserId)
    End Sub
    Public Function UploadHF(ByVal FilePath As String, ByVal StratergyId As Integer, ByVal AuditUserID As Integer, ByRef dtresult As DataTable, ByVal dryRun As Boolean, registerName As String, ByRef LogFile As String) As Dictionary(Of String, Integer)
        Dim DAL As New IMIS_DAL.HealthFacilityDAL

        Dim XMLfile As New XmlDocument
        XMLfile.Load(FilePath)

        For Each node As XmlNode In XMLfile
            If node.NodeType = XmlNodeType.XmlDeclaration Then
                XMLfile.RemoveChild(node)
            End If
        Next

        Dim dict As Dictionary(Of String, Integer) = DAL.UploadHF(XMLfile, StratergyId, AuditUserID, dtresult, dryRun)

        If dryRun = False Then
            LogFile = imisgen.CreateUploadRegisterLog(dtresult, registerName, StratergyId, System.IO.Path.GetFileName(FilePath), imisgen.getLoginName(HttpContext.Current.Session("User")))
        Else
            LogFile = String.Empty
        End If

        Return dict
    End Function
    Public Function downLoadHFXML() As String
        Dim DAL As New IMIS_DAL.HealthFacilityDAL
        Dim dtHF As New DataTable
        Dim ExportFolder As String
        Dim sXML As String = ""
        dtHF = DAL.downLoadHFXML
        For Each row As DataRow In dtHF.Rows
            sXML += row(0).ToString
        Next
        Dim ICDXML As System.Xml.XmlDocument = New System.Xml.XmlDocument
        ICDXML.LoadXml(sXML)
        ExportFolder = Web.Configuration.WebConfigurationManager.AppSettings("ExportFolder").ToString()
        Dim FileName As String = "HF" & Format(Now, "yyyyMMddHHmm") & ".xml"
        Dim path As String = HttpContext.Current.Server.MapPath(ExportFolder) & "\" & FileName
        ICDXML.Save(path)
        Return path
    End Function
    Public Function GetHfIdByUUID(ByVal uuid As Guid) As Integer
        Dim Hf As New IMIS_DAL.HealthFacilityDAL
        Return Hf.GetHfIdByUUID(uuid).Rows(0).Item(0)
    End Function
    Public Function GetHfUUIDByID(ByVal id As Integer) As Guid
        Dim Hf As New IMIS_DAL.HealthFacilityDAL
        Return Hf.GetHfUUIDByID(id).Rows(0).Item(0)
    End Function

    Public Function getHFUserLocation(ByVal UserId As Integer, ByVal Hfid As Integer) As DataTable
        Dim DAL As New IMIS_DAL.HealthFacilityDAL
        Return DAL.getHFUserLocation(UserId, Hfid)
    End Function
End Class
