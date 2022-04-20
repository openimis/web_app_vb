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

Public Class MedicalServicesBL
    Private imisgen As New GeneralBL
    Public Function GetMedicalServices(ByVal PLServiceID As Integer) As DataTable
        Dim GetDataTable As New IMIS_DAL.MedicalServicesDAL

        Dim dtSType As DataTable = GetServiceType(False)
        Dim dtServLevel As DataTable = GetServiceLevel(False)
      
        If Not dtSType.Columns.Contains("AltLanguage") Then dtSType.Columns.Add("AltLanguage")
        If Not dtServLevel.Columns.Contains("AltLanguage") Then dtServLevel.Columns.Add("AltLanguage")

        Return GetDataTable.GetMedicalServices(PLServiceID, dtSType, dtServLevel)
    End Function
    Public Function GetMedicalServices() As DataTable
        Dim GetDataTable As New IMIS_DAL.MedicalServicesDAL
        Return GetDataTable.GetMedicalServices()
    End Function
    Public Function GetMedicalServices(ByVal eService As IMIS_EN.tblServices, Optional ByVal All As Boolean = True) As DataTable
        Dim getDataTable As New IMIS_DAL.MedicalServicesDAL

        Dim dtSType As DataTable = GetServiceType(False)
        Dim dtServLevel As DataTable = GetServiceLevel(False)
        
        If Not dtSType.Columns.Contains("AltLanguage") Then dtSType.Columns.Add("AltLanguage")
        If Not dtServLevel.Columns.Contains("AltLanguage") Then dtServLevel.Columns.Add("AltLanguage")

        eService.ServCode = "%" & eService.ServCode & "%"
        eService.ServName = "%" & eService.ServName & "%"
        eService.ServType = "%" & eService.ServType & "%"

        If All = True Then
            Return getDataTable.GetMSLegacy(eService, dtSType, dtServLevel)
        Else
            Return getDataTable.GetMS(eService, dtSType, dtServLevel)
        End If
    End Function
    Public Function SaveMedicalServices(ByRef eServices As IMIS_EN.tblServices) As Integer
        Dim SaveData As New IMIS_DAL.MedicalServicesDAL
        Dim dt As DataTable = SaveData.CheckIfServiceExists(eServices)
        If dt.Rows.Count > 0 Then Return 1
        If eServices.ServiceID = 0 Then
            SaveData.InsertMedicalServices(eServices)
            Return 0
        Else
            SaveData.UpdateMedicalServices(eServices)
            Return 2
        End If
    End Function
    Public Function DeleteMedicalServices(ByVal eServices As IMIS_EN.tblServices) As Boolean
        Dim MS As New IMIS_DAL.MedicalServicesDAL
        Dim dt As DataTable = MS.CheckIfDelete(eServices)
        If dt.Rows.Count > 0 Then
            Return False
        Else
            MS.DeleteMedicalServices(eServices)
            Return True
        End If
    End Function
    Public Sub LoadMedicalService(ByRef eServices As IMIS_EN.tblServices)
        Dim load As New IMIS_DAL.MedicalServicesDAL
        load.LoadMedicalServices(eServices)
    End Sub
    Public Function GetServiceType(Optional ByVal showSelect As Boolean = True) As DataTable
        Dim dtbl As New DataTable
        dtbl.Columns.Add("ServiceID")
        dtbl.Columns.Add("ServCode")

        Dim dr As DataRow = dtbl.NewRow
        If showSelect = True Then
            dr("ServiceID") = ""
            dr("ServCode") = imisgen.getMessage("T_SELECTSERVICETYPE")
            dtbl.Rows.Add(dr)
        End If


        dr = dtbl.NewRow
        dr("ServiceID") = "P"
        dr("ServCode") = imisgen.getMessage("T_PREVENTIVE")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("ServiceID") = "C"
        dr("ServCode") = imisgen.getMessage("T_CURATIVE")
        dtbl.Rows.Add(dr)

        Return dtbl

    End Function
    Public Function GetServiceLevel(Optional ByVal ShowSelect As Boolean = True) As DataTable
        Dim dtbl As New DataTable
        dtbl.Columns.Add("ServiceID")
        dtbl.Columns.Add("Service")

        Dim dr As DataRow = dtbl.NewRow
        If ShowSelect Then
            dr("ServiceID") = ""
            dr("Service") = imisgen.getMessage("T_SELECTLEVEL")
            dtbl.Rows.Add(dr)
        End If
        dr = dtbl.NewRow
        dr("ServiceID") = "S"
        dr("Service") = imisgen.getMessage("T_SIMPLESERVICE")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("ServiceID") = "V"
        dr("Service") = imisgen.getMessage("T_VISIT")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("ServiceID") = "D"
        dr("Service") = imisgen.getMessage("T_DAYOFSTAY")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("ServiceID") = "H"
        dr("Service") = imisgen.getMessage("T_HOSPITALCASE")
        dtbl.Rows.Add(dr)
        Return dtbl
    End Function
    Public Function GetServiceCategory() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("CategoryId")
        dt.Columns.Add("Category")
        Dim dr As DataRow

        dr = dt.NewRow
        dr("CategoryId") = ""
        dr("Category") = imisgen.getMessage("L_SELECTCATEGORY")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("CategoryId") = "S"
        dr("Category") = imisgen.getMessage("T_SURGERY")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("CategoryId") = "C"
        dr("Category") = imisgen.getMessage("L_CONSULTATIONS")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("CategoryId") = "D"
        dr("Category") = imisgen.getMessage("T_DELIVERY")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("CategoryId") = "A"
        dr("Category") = imisgen.getMessage("T_ANTENATAL")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("CategoryId") = "O"
        dr("Category") = imisgen.getMessage("T_OTHER")
        dt.Rows.Add(dr)

        Return dt

    End Function
    Public Function GetServiceIdByUUID(ByVal uuid As Guid) As Integer
        Dim Service As New IMIS_DAL.MedicalServicesDAL
        Return Service.GetServiceIdByUUID(uuid).Rows(0).Item(0)
    End Function
End Class
