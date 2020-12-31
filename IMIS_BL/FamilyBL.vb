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

Public Class FamilyBL


    Public Function GetFamilyFiltered(ByVal eFamily As IMIS_EN.tblFamilies, ByVal All As Boolean, ByVal Allpoverty As Boolean) As DataTable
        Dim Family As New IMIS_DAL.FamilyDAL
        Dim gen As New GeneralBL
        Return Family.GetFamilyFiltered(eFamily, All, Allpoverty, gen.GetYesN)
    End Function
    Public Sub LoadFamily(ByRef eFamily As IMIS_EN.tblFamilies)
        Dim Family As New IMIS_DAL.FamilyDAL
        Family.LoadFamily(eFamily)
    End Sub
    Public Sub SaveFamily(ByVal eFamily As IMIS_EN.tblFamilies)
        Dim Family As New IMIS_DAL.FamilyDAL
        If eFamily.FamilyID = 0 Then
            Family.InsertInsuredFamily(eFamily)
            'assign photo
            'save insuree
            'set dependents to 0
            'Load(family And Insuree)
        End If
        
    End Sub
    Public Function UpdateFamily(ByVal eFamily As IMIS_EN.tblFamilies) As Boolean
        Dim Family As New IMIS_DAL.FamilyDAL
        Family.UpdateFamily(eFamily)
        Return True
    End Function
    Public Sub GetFamilyHeadInfo(ByVal eFamily As IMIS_EN.tblFamilies)
        Dim getHeadInfo As New IMIS_DAL.FamilyDAL
        getHeadInfo.GetFamilyHeadInfo(eFamily)
    End Sub
    Public Function FamilyExists(ByVal CHFID As String) As Boolean
        Dim Family As New IMIS_DAL.FamilyDAL
        Dim dt As DataTable = Family.FamilyExists(CHFID)
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function getOfficerID(ByVal ImageName As String) As Integer
        Dim Officer As New IMIS_BL.OfficersBL
        Return Officer.ValidOfficerCode(ExtractOfficerCode(ImageName))
    End Function
    Public Function ExtractCHFID(ByVal ImageName As String) As String
        Return Split(ImageName, IMIS_EN.AppConfiguration.InsureeImageDelimiter)(0)
    End Function
    Public Function ExtractOfficerCode(ByVal ImageName As String) As String
        Return Split(ImageName, IMIS_EN.AppConfiguration.InsureeImageDelimiter)(1)
    End Function
    Public Function ExtractDate(ByVal ImageName As String) As Date
        Return Date.ParseExact(Left(Split(ImageName, IMIS_EN.AppConfiguration.InsureeImageDelimiter)(2), 8), "yyyyMMdd", Nothing)
    End Function
    Public Function ExtractLatitude(ByVal ImageName As String) As String
        Dim ImageNameArray = Split(ImageName, IMIS_EN.AppConfiguration.InsureeImageDelimiter)

        If ImageNameArray.Length > 3 Then
            Return Split(ImageName, IMIS_EN.AppConfiguration.InsureeImageDelimiter)(3)
        End If

        Return ""
    End Function
    Public Function ExtractLongitude(ByVal ImageName As String) As String
        Dim ImageNameArray = Split(ImageName, IMIS_EN.AppConfiguration.InsureeImageDelimiter)

        If ImageNameArray.Length > 4 Then
            Return Split(ImageName, IMIS_EN.AppConfiguration.InsureeImageDelimiter)(4).Replace(".jpg", "")
        End If

        Return ""

    End Function
    Public Function DeleteFamily(ByVal eFamily As IMIS_EN.tblFamilies) As Integer '0 not deleted, 1 deleted, 2 in use
        Dim family As New IMIS_DAL.FamilyDAL
        Dim insuree As New IMIS_DAL.InsureeDAL

        Dim dt As DataTable = family.CheckCanBeDeleted(eFamily.FamilyID)
        If dt.Rows.Count > 0 Then Return 2

        If family.DeleteFamily(eFamily) Then
            If insuree.DeleteInsuree(eFamily.tblInsuree) Then 'delete head of family too
                Return 1
            Else
                Return 0
            End If
        Else
            Return 0
        End If
    End Function
    Public Function GetFamilyOfflineValue(ByVal FamilyID As Integer) As Boolean
        Dim Family As New IMIS_DAL.FamilyDAL
        Return Family.GetFamilyOfflineValue(FamilyID)
    End Function
      Public Function GetTypes() As DataTable
        Dim Gen As New GeneralBL
        Dim DAL As New IMIS_DAL.FamilyTypesDAL
        Dim dt As DataTable = DAL.GetFamilyTypes
        Dim dr As DataRow
        dr = dt.NewRow
        dr("FamilyTypeCode") = ""
        dr("FamilyType") = Gen.getMessage("T_SELECTTYPE")
        dr("AltLanguage") = Gen.getMessage("T_SELECTTYPE")
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function

    Public Function GetSubsidy() As DataTable
        Dim DAL As New IMIS_DAL.ConfirmationTypeDAL
        Dim gen As New GeneralBL
        Dim dt As DataTable = DAL.GetConfirmationTypes
        Dim dr As DataRow
        dr = dt.NewRow
        dr("ConfirmationTypeCode") = ""
        dr("ConfirmationType") = gen.getMessage("T_SELECTTYPE")
        dr("AltLanguage") = gen.getMessage("T_SELECTTYPE")
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function

    Public Function GetEthnicity() As DataTable
        Dim dt As New DataTable
        Dim gen As New GeneralBL
        dt.Columns.Add("Code")
        dt.Columns.Add("Ethnicity")

        dt.Rows.Add(New Object() {"", gen.getMessage("M_ETHNICITY")})
        dt.Rows.Add(New Object() {"1", "1"})
        dt.Rows.Add(New Object() {"2", "2"})
        dt.Rows.Add(New Object() {"3", "3"})
        dt.Rows.Add(New Object() {"4", "4"})
        dt.Rows.Add(New Object() {"5", "5"})
        dt.Rows.Add(New Object() {"6", "6"})

        Return dt
    End Function
    Public Function GetFamilyIdByUUID(ByVal uuid As Guid) As Integer
        Dim Family As New IMIS_DAL.FamilyDAL
        Return Family.GetFamilyIdByUUID(uuid).Rows(0).Item(0)
    End Function
    Public Function GetFamilyUUIDByID(ByVal id As Integer) As Guid
        Dim Family As New IMIS_DAL.FamilyDAL
        Return Family.GetFamilyUUIDByID(id).Rows(0).Item(0)
    End Function
End Class
