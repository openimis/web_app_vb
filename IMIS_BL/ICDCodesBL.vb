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

Public Class ICDCodesBL
    Dim icd As New IMIS_DAL.ICDCodesDAL
    Private imisgen As New GeneralBL

    Public Function GetICDCodes(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim dt As DataTable
        dt = icd.getICDCodes

        Dim dr As DataRow
        dr = dt.NewRow
        dr("ICDID") = 0
        dr("ICDCode") = imisgen.getMessage("T_ICDCODE")
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function getAutoCompleteICDCodes(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim dt As DataTable
        dt = icd.getAutoCompleteICDCodes
        Return dt
    End Function
    'Public Function getICDIDFromCode(ByVal ICDCODE As String) As Integer

    '    Return icd.getICDIDFromCode(ICDCODE)

    'End Function
    Public Function DownLoadDiagnosisXML() As String
        Dim dtICD As New DataTable
        Dim ExportFolder As String
        Dim sXML As String = ""
        dtICD = icd.DownLoadDiagnosisXML()

        For Each row As DataRow In dtICD.Rows
            sXML += row(0).ToString
        Next
        Dim ICDXML As System.Xml.XmlDocument = New System.Xml.XmlDocument
        ICDXML.LoadXml(sXML)
        ExportFolder = Web.Configuration.WebConfigurationManager.AppSettings("ExportFolder").ToString()
        Dim FileName As String = "Diagnosis" & Format(Now, "yyyyMMddHHmm") & ".xml"
        Dim path As String = HttpContext.Current.Server.MapPath(ExportFolder) & "\" & FileName
        ICDXML.Save(path)
        Return path
    End Function

    Public Function UploadICDXML(ByVal FilePath As String, ByVal StratergyId As Integer, ByVal AuditUserID As Integer, ByRef dtResult As DataTable, ByVal dryRun As Boolean, registerName As String, ByRef LogFile As String) As Dictionary(Of String, Integer)
        Dim ICD As New IMIS_DAL.ICDCodesDAL

        Dim XMLfile As New XmlDocument
        XMLfile.XmlResolver = Nothing
        XMLfile.Load(FilePath)

        For Each node As XmlNode In XMLfile
            If node.NodeType = XmlNodeType.XmlDeclaration Then
                XMLfile.RemoveChild(node)
            End If
        Next


        Dim dict As Dictionary(Of String, Integer) = ICD.UploadICDXML(XMLfile, StratergyId, AuditUserID, dtResult, dryRun)

        If dryRun = False Then
            LogFile = imisgen.CreateUploadRegisterLog(dtResult, registerName, StratergyId, System.IO.Path.GetFileName(FilePath), imisgen.getLoginName(HttpContext.Current.Session("User")), "Diagnosis")
        Else
            LogFile = String.Empty
        End If


        Return dict
    End Function



End Class
