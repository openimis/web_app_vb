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

Public Class PayersBL
    Private imisgen As New GeneralBL
    
    Public Sub DeletePayer(ByRef ePayers As IMIS_EN.tblPayer)
        Dim Payer As New IMIS_DAL.PayersDAL
        Payer.DeletePayer(ePayers)
    End Sub
    Public Function GetPayers(ByVal ePayer As IMIS_EN.tblPayer, Optional ByVal Legacy As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_DAL.PayersDAL
        ePayer.PayerName += "%"
        ePayer.eMail = "%" & ePayer.eMail & "%"
        ePayer.Phone += "%"
        ePayer.PayerType += "%"
        Dim dtPayerType As DataTable = GetPayerType()
        Return getDataTable.GetPayers(ePayer, dtPayerType, Legacy)
      
    End Function
    Public Function GetPayers(ByVal RegionId As Integer, ByVal Districtid As Integer, ByVal userId As Integer, ByVal showSelect As Boolean) As DataTable
        Dim Premium As New IMIS_DAL.PayersDAL
        Dim dt As DataTable = Premium.GetPayers(RegionId, Districtid, userId)
        If showSelect = True Then
            Dim dr As DataRow = dt.NewRow
            dr("PayerID") = 0
            dr("PayerName") = imisgen.getMessage("T_SELECTAPAYER")
            dt.Rows.InsertAt(dr, 0)
        End If
        Return dt
    End Function
    Public Function SavePayer(ByRef ePayers As IMIS_EN.tblPayer) As Integer
        Dim SaveData As New IMIS_DAL.PayersDAL
        If SaveData.CheckIfPayerExists(ePayers) = True Then Return 1
        If ePayers.PayerID = 0 Then
            SaveData.InsertPayer(ePayers)
            Return 0
        Else
            Dim ePayersOrg As New IMIS_EN.tblPayer
            ePayersOrg.PayerID = ePayers.PayerID
            SaveData.LoadPayer(ePayersOrg)
            If (isDirtyPayers(ePayers, ePayersOrg)) Then
                SaveData.UpdatePayer(ePayers)
                Return 2
            End If
        End If
    End Function

    Private Function isDirtyPayers(ePayers As IMIS_EN.tblPayer, ePayersOrg As IMIS_EN.tblPayer) As Boolean
        isDirtyPayers = True

        If IIf(ePayers.LocationId Is Nothing, DBNull.Value, ePayers.LocationId).ToString() <> IIf(ePayersOrg.LocationId Is Nothing, DBNull.Value, ePayersOrg.LocationId).ToString() Then Exit Function
        If IIf(ePayers.PayerName Is Nothing, DBNull.Value, ePayers.PayerName).ToString() <> IIf(ePayersOrg.PayerName Is Nothing, DBNull.Value, ePayersOrg.PayerName).ToString() Then Exit Function
        If IIf(ePayers.Phone Is Nothing, DBNull.Value, ePayers.Phone).ToString() <> IIf(ePayersOrg.Phone Is Nothing, DBNull.Value, ePayersOrg.Phone).ToString() Then Exit Function
        If IIf(ePayers.PayerType Is Nothing, DBNull.Value, ePayers.PayerType).ToString() <> IIf(ePayersOrg.PayerType Is Nothing, DBNull.Value, ePayersOrg.PayerType).ToString() Then Exit Function
        If ePayers.PayerID <> ePayersOrg.PayerID Then Exit Function
        If ePayers.ValidityFrom <> ePayersOrg.ValidityFrom Then Exit Function
        If IIf(ePayers.PayerAddress Is Nothing, DBNull.Value, ePayers.PayerAddress).ToString() <> IIf(ePayersOrg.PayerAddress Is Nothing, DBNull.Value, ePayersOrg.PayerAddress).ToString() Then Exit Function
        If IIf(ePayers.Fax Is Nothing, DBNull.Value, ePayers.Fax).ToString() <> IIf(ePayersOrg.Fax Is Nothing, DBNull.Value, ePayersOrg.Fax).ToString() Then Exit Function
        If IIf(ePayers.eMail Is Nothing, DBNull.Value, ePayers.eMail).ToString() <> IIf(ePayersOrg.eMail Is Nothing, DBNull.Value, ePayersOrg.eMail).ToString() Then Exit Function
        If ePayers.AuditUserID <> ePayersOrg.AuditUserID Then Exit Function
        isDirtyPayers = False
    End Function



    Public Sub LoadPayer(ByRef ePayers As IMIS_EN.tblPayer)
        Dim load As New IMIS_DAL.PayersDAL
        load.LoadPayer(ePayers)
    End Sub

    Public Function GetPayerType(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim DAL As New IMIS_DAL.PayerTypeDAL
        Dim dtbl As DataTable = DAL.GetPayerType
        Dim dr As DataRow
        If showSelect = True Then
            dr = dtbl.NewRow
            dr("Code") = ""
            dr("PayerType") = imisgen.getMessage("T_SELECTPAYERTYPE")
            dr("AltLanguage") = imisgen.getMessage("T_SELECTPAYERTYPE")
            dtbl.Rows.InsertAt(dr, 0)
        End If
        'dr = dtbl.NewRow
        'dr("Code") = "G"
        'dr("PayerType") = imisgen.getMessage("T_GOVERNMENT")
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = "L"
        'dr("PayerType") = imisgen.getMessage("T_LOCALAUTHORITY")
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = "C"
        'dr("PayerType") = imisgen.getMessage("T_COOPERATIVE")
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = "P"
        'dr("PayerType") = imisgen.getMessage("T_PRIVATEORGANIZATION")
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = "D"
        'dr("PayerType") = imisgen.getMessage("T_DONOR")
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = "O"
        'dr("PayerType") = imisgen.getMessage("T_OTHER")
        'dtbl.Rows.Add(dr)

        Return dtbl
    End Function

    Public Function GetPayerIdByUUID(ByVal uuid As Guid) As Integer
        Dim Payer As New IMIS_DAL.PayersDAL
        Return Payer.GetPayerIdByUUID(uuid).Rows(0).Item(0)
    End Function
End Class
