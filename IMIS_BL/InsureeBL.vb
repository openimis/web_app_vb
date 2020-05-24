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

Imports System.IO
Imports System.Text
Imports System.Web

Public Class InsureeBL
    Private Insuree As New IMIS_DAL.InsureeDAL
    Private imisgen As New GeneralBL
    Public Function GetInsureesByFamily(ByVal FamilyId As Integer, Optional Language As String = "en") As DataTable
        Return Insuree.GetInsureesByFamily(FamilyId, Language)
    End Function
    Public Function SaveInsuree(ByVal eInsuree As IMIS_EN.tblInsuree, ByVal Activate As Boolean) As Integer
        If InsureeExists(eInsuree) Then Return 1
        If eInsuree.InsureeID = 0 Then
            Insuree.InsertInsuree(eInsuree)
            AddInsuree(eInsuree.InsureeID, Activate)
            Dim Policy As New IMIS_DAL.PolicyDAL

            Return 0
        Else
            Insuree.ModifyInsuree(eInsuree)
            Return 2
        End If
    End Function
    Public Sub AddInsuree(ByVal InsureeId As Integer, Optional ByVal Activate As Boolean = False)
        Dim IP As New IMIS_BL.InsureePolicyBL
        IP.AddInsuree(InsureeId, Activate)
    End Sub
    
    Public Function MoveInsuree(ByVal eInsureeNew As IMIS_EN.tblInsuree, ByVal Activate As Boolean) As Boolean
        Dim InsureeDAL As New IMIS_DAL.InsureeDAL

        If InsureeDAL.MoveInsuree(eInsureeNew) = True Then
            DeletePolicyInsuree(eInsureeNew.InsureeID)
            AddInsuree(eInsureeNew.InsureeID, Activate)
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub LoadInsuree(ByRef eInsuree As IMIS_EN.tblInsuree)
        Insuree.LoadInsuree(eInsuree)
    End Sub
    Public Sub GetInsureesByCHFID(ByRef einsuree As IMIS_EN.tblInsuree)
        Insuree.GetInsureesByCHFID(einsuree)

    End Sub
    Public Function GetCHFNumbers() As DataTable
        Return Insuree.GetCHFNumbers()
    End Function
    Public Function FindInsuree(ByRef eInsuree As IMIS_EN.tblInsuree, Optional ByVal All As Boolean = False, Optional ByVal PhotoAssigned As Int16 = 1, Optional Language As String = "en")
        Dim dtMarital As New DataTable
        Dim BLGen As New GeneralBL
        dtMarital = BLGen.GetMaritalStatus
        Return Insuree.GetInsureeFullSearch(eInsuree, All, PhotoAssigned, Language, dtMarital)
    End Function
    Public Function InsureeExists(ByVal eInsuree As IMIS_EN.tblInsuree) As Boolean
        Dim dt As DataTable = Insuree.InsureeExists(eInsuree)
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function FetchNewImages(ByVal ImagePath As String, ByVal CHFID As String) As DataTable
        Dim Images As FileInfo()

        Dim dt As New DataTable
        dt.Columns.Add("ImagePath")
        dt.Columns.Add("TakenDate", System.Type.GetType("System.DateTime"))

        Dim dr As DataRow = dt.NewRow

        If Not CHFID Is Nothing Then

            Dim DirInfo As New DirectoryInfo(ImagePath)
            Images = DirInfo.GetFiles(CHFID & "*")
            'Images = Directory.GetFiles(ImagePath, CHFID & "*")

            For i As Integer = 0 To Images.Count - 1

                Dim Family As New IMIS_BL.FamilyBL
                If Family.getOfficerID(Images(i).Name) = -1 Then Continue For

                dr("ImagePath") = Images(i).Name

                dr("TakenDate") = Family.ExtractDate(Images(i).Name)
                dt.Rows.Add(dr)
                dr = dt.NewRow
            Next


        End If

        Return dt

    End Function
    Public Sub MoveImageToFolder(ByVal SrcPath As String, ByVal DestPath As String, ByVal FileName As String)
        Directory.Move(SrcPath & FileName, DestPath & FileName)
    End Sub
    Public Sub UpdateImage(ByRef ePhotos As IMIS_EN.tblPhotos, Optional ByVal UpdateInDatabase As Boolean = True)
        MoveImageToFolder(HttpContext.Current.Server.MapPath(IMIS_EN.AppConfiguration.SubmittedFolder), HttpContext.Current.Server.MapPath(IMIS_EN.AppConfiguration.UpdatedFolder), ePhotos.PhotoFileName)
        If UpdateInDatabase = True Then Insuree.UpdateImage(ePhotos)
    End Sub
    Public Function FindInsureeByCHFID(ByVal CHFID As String, Optional Language As String = "en")
        Return Insuree.FindInsureeByCHFID(CHFID, Language)
    End Function
    Public Function verifyCHFIDandReturnName(ByVal CHFID As String, ByRef insureeid As Integer) As String
        Return Insuree.verifyCHFIDandReturnName(CHFID, insureeid)
    End Function
    Public Function ChangeHead(ByVal eInsureeOLD As IMIS_EN.tblInsuree, ByVal eInsureeNew As IMIS_EN.tblInsuree) As Boolean
        Dim InsureeDAL As New IMIS_DAL.InsureeDAL
        Return InsureeDAL.ChangeHead(eInsureeOLD, eInsureeNew)
    End Function
    Public Function DeleteInsuree(ByVal eInsuree As IMIS_EN.tblInsuree) As Integer
        Dim insuree As New IMIS_DAL.InsureeDAL
        Dim dt As DataTable = insuree.CheckCanBeDeleted(eInsuree.InsureeID)
        If dt.Rows.Count > 0 Then Return 2

        If insuree.DeleteInsuree(eInsuree) Then
            DeletePolicyInsuree(eInsuree.InsureeID)
            Return 1
        Else
            Return 0
        End If
    End Function
    Public Sub DeletePolicyInsuree(ByVal InsureeId As Integer)
        Dim IP As New IMIS_BL.InsureePolicyBL
        IP.DeletePolicyInsuree(InsureeId, 0)
    End Sub
    Public Function GetPhotoAssigned() As DataTable
        Dim dt As New DataTable
        Dim dr As DataRow

        dt.Columns.Add("PhotoAssignedValue")
        dt.Columns.Add("PhotoAssignedText")

        dr = dt.NewRow
        dr("PhotoAssignedValue") = 1
        dr("PhotoAssignedText") = imisgen.getMessage("T_ALL")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("PhotoAssignedValue") = 2
        dr("PhotoAssignedText") = imisgen.getMessage("T_YES")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("PhotoAssignedValue") = 3
        dr("PhotoAssignedText") = imisgen.getMessage("T_NO")
        dt.Rows.Add(dr)

        Return dt
    End Function
    Public Function GetInsureeOfflineValue(ByVal InsureeID As Integer) As Boolean
        Return Insuree.GetInsureeOfflineValue(InsureeID)
    End Function
    Public Function GetRelations() As DataTable
        Dim Gen As New GeneralBL
        Dim DAL As New IMIS_DAL.RelationsDAL
        Dim dt As DataTable = DAL.GetRelations
        Dim dr As DataRow
        dr = dt.NewRow
        dr("RelationId") = 0
        dr("Relation") = Gen.getMessage("M_SELECTRELATION")
        dr("AltLanguage") = Gen.getMessage("M_SELECTRELATION")
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function GetProfession() As DataTable
        Dim Gen As New GeneralBL

        Dim DAL As New IMIS_DAL.ProfessionsDAL
        Dim dt As DataTable = DAL.GetProfessions
        Dim dr As DataRow
        dr = dt.NewRow
        dr("ProfessionId") = 0
        dr("Profession") = Gen.getMessage("M_SELECTPROFESSION")
        dr("AltLanguage") = Gen.getMessage("M_SELECTPROFESSION")

        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function GetEducation() As DataTable
        Dim Gen As New GeneralBL
        Dim DAL As New IMIS_DAL.EducationsDAL
        Dim dt As DataTable = DAL.GetEducations
        Dim dr As DataRow
        dr = dt.NewRow
        dr("EducationId") = 0
        dr("Education") = Gen.getMessage("M_SELECTEDUCATION")
        dr("AltLanguage") = Gen.getMessage("M_SELECTEDUCATION")
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function GetInsureeProductDetails(ByVal oDict As Dictionary(Of String, Object), ByVal CHFID As String, ByVal ItemCode As String, ByVal ServiceCode As String) As DataTable
        Return Insuree.GetInsureeProductDetails(oDict, CHFID, ItemCode, ServiceCode)
    End Function
    Public Function GetMaxMemberCount(ByVal FamilyId As Integer) As DataTable
        Dim Insuree As New IMIS_DAL.InsureeDAL
        Return Insuree.GetMaxMemberCount(FamilyId)
    End Function

    Public Function GetTypeOfIdentity() As DataTable
        Dim Gen As New GeneralBL
        Dim DAL As New IMIS_DAL.IdentificationTypesDAL
        Dim dt As DataTable = DAL.GetIdentificationTypes
        Dim dr As DataRow
        dr = dt.NewRow
        dr("IdentificationCode") = ""
        dr("IdentificationTypes") = Gen.getMessage("M_SELECTIDENTITY")
        dr("AltLanguage") = Gen.getMessage("M_SELECTIDENTITY")
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function

    Public Function GetInsureeIdByUUID(ByVal uuid As Guid) As Integer
        Dim Insuree As New IMIS_DAL.InsureeDAL
        Return Insuree.GetInsureeIdByUUID(uuid).Rows(0).Item(0)
    End Function
    Public Function GetInsureeUUIDByID(ByVal id As Integer) As Guid
        Dim Insuree As New IMIS_DAL.InsureeDAL
        Return Insuree.GetInsureeUUIDByID(id).Rows(0).Item(0)
    End Function
    Public Function getLastVisitDays(ByVal CHFID As String, ByVal hfid As Integer) As String
        Return Insuree.getLastVisitDays(CHFID, hfid)
    End Function
    Public Function getLastVisitDaysForReview(ByVal CHFID As String, ByVal claimid As Integer, ByVal VISITDATETO As Date) As DataTable
        Return Insuree.getLastVisitDaysForReview(CHFID, claimid, VISITDATETO)
    End Function
    Public Function GetFamilyDetails(ByVal CHFID As String, Optional Language As String = "en")
        Return Insuree.GetFamilyDetails(CHFID, Language)
    End Function
    Public Function GetClaimList(ByVal CHFID As String, Optional Language As String = "en")
        Return Insuree.GetClaimList(CHFID, Language)
    End Function

End Class
