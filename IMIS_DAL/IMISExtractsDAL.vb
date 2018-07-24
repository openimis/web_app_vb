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
Imports System.Security.Cryptography
Imports System.Data.SqlClient


Public Class IMISExtractsDAL
    Dim data As New ExactSQL


    Public Function getReferences() As DataTable

        Dim sSQL As String = ""
        sSQL = "SELECT ItemCode [Code],ItemName [Name],'I' [Type],ItemPrice [Price] FROM TBLITEMS WHERE ValidityTo IS NULL" & vbCrLf & _
               " UNION ALL" & vbCrLf & _
               " SELECT ServCode,ServName,'S',ServPrice FROM tblServices WHERE ValidityTo IS NULL" & vbCrLf & _
               " UNION ALL" & vbCrLf & _
               " SELECT ICDCode,ICDName, 'D',0 FROM tblICDCodes WHERE ValidityTo IS NULL"


        data.setSQLCommand(sSQL, CommandType.Text)
        Return data.Filldata


    End Function
    'Corrected By Rogers
    Public Function GetExtractList(ByVal LocationId As Integer, ByVal ExtractDirection As Integer, ByVal ExtractType As Integer) As DataTable
        Dim dt As New DataTable
        Dim sSQL As String = ""
        sSQL += " SELECT TOP 100 [ExtractID],RIGHT ('000000' + Cast([ExtractSequence] as nvarchar(8)) , 6 ) + ' - ' + CONVERT(nvarchar(10),"
        sSQL += " ExtractDate ,105) as ExtractInfo FROM [dbo].[tblExtracts] E "
        sSQL += " INNER JOIN uvwLocations L ON L.LocationId = E.LocationId"
        sSQL += " WHERE (L.LocationId = @LocationId   )"
        sSQL += " AND ExtractDirection = @ExtractDirection And ExtractType = @ExtractType ORDER BY ExtractID DESC"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@ExtractType", SqlDbType.Int, ExtractType)
        data.params("@ExtractDirection", SqlDbType.Int, ExtractDirection)

        dt = data.Filldata()
        Return dt

    End Function
    'Corrected by Rogers
    Public Function GetLastCreateExtractInfo(ByVal LocationId As Integer, ByVal ExtractType As Integer, Optional ByVal ExtractDirection As Integer = 0) As IMIS_EN.tblExtracts
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " DECLARE @MaxSeq INT;"
        sSQL += " SELECT @MaxSeq = ISNULL(MAX([ExtractSequence]),0) FROM [dbo].[tblExtracts]"
        sSQL += " WHERE ExtractDirection = @ExtractDirection"
        sSQL += " AND  LocationId = @LocationId "
        sSQL += " AND (CASE @ExtractType WHEN 0 THEN 0 ELSE ExtractType END ) = @ExtractType AND ValidityTo IS NULL;"
        sSQL += " SELECT @MaxSeq AS SEQUENCE, RowID FROM tblExtracts"
        sSQL += " WHERE ExtractDirection = @ExtractDirection"
        sSQL += " AND  LocationId = @LocationId "
        sSQL += " AND (CASE @ExtractType WHEN 0 THEN 0 ELSE ExtractType END ) = @ExtractType"
        sSQL += " AND ValidityTo IS NULL AND ExtractSequence = @MaxSeq;"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        '  data.params("@DistrictId", SqlDbType.Int, DistrictId)
        data.params("@ExtractType", SqlDbType.Int, ExtractType)
        data.params("@ExtractDirection", SqlDbType.Int, ExtractDirection)
        Dim eExtractLog As New IMIS_EN.tblExtracts
        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            eExtractLog.RowID = dr("RowID")
            eExtractLog.ExtractSequence = dr("Sequence")
        Else
            eExtractLog.RowID = 0
            eExtractLog.ExtractSequence = 0
        End If
        Return eExtractLog
    End Function

    Public Function GetNewSeqToImport() As IMIS_EN.tblExtracts
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " DECLARE @MaxSeq INT;"
        sSQL += " SELECT @MaxSeq = ISNULL(MAX([ExtractSequence]),0) FROM [dbo].[tblExtracts]"
        sSQL += " WHERE ExtractDirection = 1 AND ValidityTo IS NULL;"
        sSQL += " SELECT @MaxSeq AS SEQUENCE, RowID FROM tblExtracts"
        sSQL += " WHERE ValidityTo IS NULL AND ExtractSequence = @MaxSeq;"
        data.setSQLCommand(sSQL, CommandType.Text)

        Dim eExtractLog As New IMIS_EN.tblExtracts
        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            eExtractLog.RowID = dr("RowID")
            eExtractLog.ExtractSequence = dr("Sequence")
        Else
            eExtractLog.RowID = 0
            eExtractLog.ExtractSequence = 0
        End If
        Return eExtractLog
    End Function

    Public Function FlagExtractTableasDeleted()
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " UPDATE tblExtracts SET ValidityTo = GETDATE() WHERE ValidityTo IS NULL;"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.ExecuteCommand()
        Return True
    End Function

    'Not Corrected yet Require more information by  Rogers
    Public Function GetExportOfflineExtract1(ByVal RowID As Int64, ByRef dtLocation As DataTable) As Boolean

        Dim dsResult As New DataSet
        data.setSQLCommand("uspExportOffLineExtract1", CommandType.StoredProcedure, timeout:=0)
        '  data.params("@LocationId", SqlDbType.Int, DistrictID)
        data.params("@RowID", SqlDbType.BigInt, RowID)

        ' dsResult = data.FilldataSet()
        dtLocation = data.Filldata
        'dtRegion = data.Filldata(0)
        'dtDistricts = dsResult.Tables(1)
        'dtWards = dsResult.Tables(2)
        'dtVillages = dsResult.Tables(3)

        Return True
    End Function
    Public Function GetExportOfflineExtract2(ByVal DistrictID As Integer, ByVal RowID As Int64, ByRef dtItems As DataTable, ByRef dtServices As DataTable, ByRef dtPLItems As DataTable, ByRef dtPLItemsDetails As DataTable, ByRef dtPLServices As DataTable, ByRef dtPLServicesDetails As DataTable) As Boolean

        Dim dsResult As New DataSet
        data.setSQLCommand("uspExportOffLineExtract2", CommandType.StoredProcedure, timeout:=0)
        data.params("@LocationId", SqlDbType.Int, DistrictID)
        data.params("@RowID", SqlDbType.BigInt, RowID)

        dsResult = data.FilldataSet()

        dtItems = dsResult.Tables(0)
        dtServices = dsResult.Tables(1)
        dtPLItems = dsResult.Tables(2)
        dtPLItemsDetails = dsResult.Tables(3)
        dtPLServices = dsResult.Tables(4)
        dtPLServicesDetails = dsResult.Tables(5)


        Return True
    End Function
    Public Function GetExportOfflineExtract3(ByRef eExtractInfo As IMIS_EN.eExtractInfo, ByVal RowID As Int64, ByRef dtICD As DataTable, ByRef dtHF As DataTable, ByRef dtPayer As DataTable, ByRef dtOfficer As DataTable, ByRef dtProduct As DataTable, ByRef dtProductItems As DataTable, ByRef dtProductServices As DataTable, ByRef dtRelDistr As DataTable, ByRef dtClaimAdmin As DataTable, ByRef dtOfficerVillage As DataTable) As Boolean

        Dim dsResult As New DataSet
        data.setSQLCommand("uspExportOffLineExtract3", CommandType.StoredProcedure, timeout:=0)
        data.params("@RegionId", SqlDbType.Int, eExtractInfo.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eExtractInfo.DistrictId)
        data.params("@RowID", SqlDbType.BigInt, RowID)

        dsResult = data.FilldataSet()

        dtICD = dsResult.Tables(0)
        dtHF = dsResult.Tables(1)
        dtPayer = dsResult.Tables(2)
        dtOfficer = dsResult.Tables(3)
        dtProduct = dsResult.Tables(4)
        dtProductItems = dsResult.Tables(5)
        dtProductServices = dsResult.Tables(6)
        dtRelDistr = dsResult.Tables(7)
        dtClaimAdmin = dsResult.Tables(8)
        dtOfficerVillage = dsResult.Tables(9)

        Return True
    End Function
    Public Function GetExportOfflineExtract4(ByRef eExtractInfo As IMIS_EN.eExtractInfo, ByVal RowID As Int64, ByRef dt As DataTable, ByVal str As Integer) As Boolean

        ' Dim dsResult As New DataSet
        data.setSQLCommand("uspExportOffLineExtract4", CommandType.StoredProcedure, timeout:=0)
        ' data.params("@LocationId", SqlDbType.Int, DistrictID)
        data.params("@RegionId", SqlDbType.Int, eExtractInfo.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eExtractInfo.DistrictId)
        data.params("@RowID", SqlDbType.BigInt, RowID)
        data.params("@WithInsuree", SqlDbType.Bit, eExtractInfo.WithInsuree)
        'get Family
        If str = 1 Then
            dt = data.Filldata()
            Return True
        End If


        'dsResult = data.FilldataSet()

        data.setSQLCommand("uspExportOffLineExtract5", CommandType.StoredProcedure, timeout:=0)
        ' data.params("@LocationId", SqlDbType.Int, DistrictID)
        data.params("@RegionId", SqlDbType.Int, eExtractInfo.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eExtractInfo.DistrictId)
        data.params("@RowID", SqlDbType.BigInt, RowID)
        data.params("@WithInsuree", SqlDbType.Bit, eExtractInfo.WithInsuree)
        If str = 2 Then
            dt = data.Filldata()
            Return True
        End If


        data.setSQLCommand("uspExportOffLineExtract6", CommandType.StoredProcedure, timeout:=0)
        ' data.params("@LocationId", SqlDbType.Int, DistrictID)
        data.params("@RegionId", SqlDbType.Int, eExtractInfo.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eExtractInfo.DistrictId)
        data.params("@RowID", SqlDbType.BigInt, RowID)
        data.params("@WithInsuree", SqlDbType.Bit, eExtractInfo.WithInsuree)
        If str = 3 Then
            dt = data.Filldata()
            Return True
        End If

        data.setSQLCommand("uspExportOffLineExtract7", CommandType.StoredProcedure, timeout:=0)
        ' data.params("@LocationId", SqlDbType.Int, DistrictID)
        data.params("@RegionId", SqlDbType.Int, eExtractInfo.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eExtractInfo.DistrictId)
        data.params("@RowID", SqlDbType.BigInt, RowID)
        data.params("@WithInsuree", SqlDbType.Bit, eExtractInfo.WithInsuree)
        If str = 4 Then
            dt = data.Filldata()
            Return True
        End If

        data.setSQLCommand("uspExportOffLineExtract8", CommandType.StoredProcedure, timeout:=0)
        ' data.params("@LocationId", SqlDbType.Int, DistrictID)
        data.params("@RegionId", SqlDbType.Int, eExtractInfo.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eExtractInfo.DistrictId)
        data.params("@RowID", SqlDbType.BigInt, RowID)
        data.params("@WithInsuree", SqlDbType.Bit, eExtractInfo.WithInsuree)
        If str = 5 Then
            dt = data.Filldata()
            Return True
        End If

        data.setSQLCommand("uspExportOffLineExtract9", CommandType.StoredProcedure, timeout:=0)
        ' data.params("@LocationId", SqlDbType.Int, DistrictID)
        data.params("@RegionId", SqlDbType.Int, eExtractInfo.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eExtractInfo.DistrictId)
        data.params("@RowID", SqlDbType.BigInt, RowID)
        data.params("@WithInsuree", SqlDbType.Bit, eExtractInfo.WithInsuree)
        If str = 6 Then
            dt = data.Filldata()
            Return True
        End If

        Return True
    End Function
    Public Function GetExportOfflineExtract5(ByVal eExtract As IMIS_EN.tblExtracts, ByRef dtExtracts As DataTable) As Boolean

        Dim dt As New DataTable

        data.setSQLCommand("SELECT [ExtractID],[ExtractDirection],[ExtractType],[ExtractSequence],[ExtractDate],[ExtractFileName],[ExtractFolder],[LocationId],[HFID],[AppVersionBackend],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID],[RowID] FROM [dbo].[tblExtracts] WHERE LocationId = @LocationId AND ExtractType = @ExtractType AND ExtractDirection = @ExtractDirection AND ExtractSequence = @ExtractSequence", CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, eExtract.LocationId)
        data.params("@ExtractType", SqlDbType.Int, eExtract.ExtractType)
        data.params("@ExtractDirection", SqlDbType.Int, eExtract.ExtractDirection)
        data.params("@ExtractSequence", SqlDbType.Int, eExtract.ExtractSequence)

        Dim eExtractLog As New IMIS_EN.tblExtracts
        dt = data.Filldata()
        dtExtracts = dt

        Return True
    End Function
    Public Function ImportOfflineExtract1(ByRef eExtractInfo As IMIS_EN.eExtractInfo, ByRef dtLocations As DataTable) As Boolean
        Dim dsResult As New DataSet
        data.setSQLCommand("uspImportOffLineExtract1", CommandType.StoredProcedure, timeout:=0)
        'data.params("@HFID", SqlDbType.Int, eExtractInfo.HFID)
        'data.params("@LocationId", SqlDbType.Int, eExtractInfo.LocationId)
        data.params("@AuditUser", SqlDbType.Int, eExtractInfo.AuditUserID)
        data.params("@LocationsIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@LocationsUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        'data.params("@WardsIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        'data.params("@WardsUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        'data.params("@VillagesIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        'data.params("@VillagesUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@xLocations", dtLocations)
        'data.params("@xtDistricts", dtDistricts)
        'data.params("@xtWards", dtWards)
        'data.params("@xtVillages", dtVillages)

        dsResult = data.FilldataSet()

        eExtractInfo.LocationsIns = data.sqlParameters("@LocationsIns")
        eExtractInfo.LocationsUp = data.sqlParameters("@LocationsUpd")
        'eExtractInfo.WardsIns = data.sqlParameters("@WardsIns")
        'eExtractInfo.WardsUpd = data.sqlParameters("@WardsUpd")
        'eExtractInfo.VillagesIns = data.sqlParameters("@VillagesIns")
        'eExtractInfo.VillagesUpd = data.sqlParameters("@VillagesUpd")

        Return True
    End Function
    Public Function ImportOfflineExtract2(ByRef eExtractInfo As IMIS_EN.eExtractInfo, ByRef dtItems As DataTable, ByRef dtServices As DataTable, ByRef dtPLItems As DataTable, ByRef dtPLItemsDetails As DataTable, ByRef dtPLServices As DataTable, ByRef dtPLServicesDetails As DataTable) As Boolean
        Dim dsResult As New DataSet
        data.setSQLCommand("uspImportOffLineExtract2", CommandType.StoredProcedure, timeout:=0)
        data.params("@HFID", SqlDbType.Int, eExtractInfo.HFID)
        data.params("@LocationId", SqlDbType.Int, eExtractInfo.LocationId)
        data.params("@AuditUser", SqlDbType.Int, eExtractInfo.AuditUserID)
        data.params("@ItemsIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ItemsUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ServicesIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ServicesUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PLItemsIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PLItemsUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PLItemsDetailIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PLItemsDetailUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PLServicesIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PLServicesUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PLServicesDetailIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PLServicesDetailUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@xtItems", dtItems)
        data.params("@xtServices", dtServices)
        data.params("@xtPLItems", dtPLItems)
        data.params("@xtPLServices", dtPLServices)
        data.params("@xtPLItemsDetail", dtPLItemsDetails)
        data.params("@xtPLServicesDetail", dtPLServicesDetails)

        dsResult = data.FilldataSet()

        eExtractInfo.ItemsIns = data.sqlParameters("@ItemsIns")
        eExtractInfo.ItemsUpd = data.sqlParameters("@ItemsUpd")
        eExtractInfo.ServicesIns = data.sqlParameters("@ServicesIns")
        eExtractInfo.ServicesUpd = data.sqlParameters("@ServicesUpd")
        eExtractInfo.PLItemsIns = data.sqlParameters("@PLItemsIns")
        eExtractInfo.PLItemsUpd = data.sqlParameters("@PLItemsUpd")
        eExtractInfo.PLItemsDetailsIns = data.sqlParameters("@PLItemsDetailIns")
        eExtractInfo.PLItemsDetailsUpd = data.sqlParameters("@PLItemsDetailUpd")
        eExtractInfo.PLServicesIns = data.sqlParameters("@PLServicesIns")
        eExtractInfo.PLServicesUpd = data.sqlParameters("@PLServicesUpd")
        eExtractInfo.PLServicesDetailsIns = data.sqlParameters("@PLServicesDetailIns")
        eExtractInfo.PLServicesDetailsUpd = data.sqlParameters("@PLServicesDetailUpd")

        Return True
    End Function
    Public Function ImportOfflineExtract3(ByRef eExtractInfo As IMIS_EN.eExtractInfo, ByRef dtICD As DataTable, ByRef dtHF As DataTable, ByRef dtPayer As DataTable, ByRef dtOfficer As DataTable, ByRef dtProduct As DataTable, ByRef dtProductItems As DataTable, ByRef dtProductServices As DataTable, ByRef dtRelDistr As DataTable, ByRef dtClaimAdmin As DataTable, ByRef dtOfficerVillage As DataTable) As Boolean
        Dim dsResult As New DataSet
        data.setSQLCommand("uspImportOffLineExtract3", CommandType.StoredProcedure, timeout:=0)
        data.params("@HFID", SqlDbType.Int, eExtractInfo.HFID)
        'data.params("@LocationId", SqlDbType.Int, eExtractInfo.LocationId)
        data.params("@AuditUser", SqlDbType.Int, eExtractInfo.AuditUserID)
        data.params("@ICDIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ICDUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@HFIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@HFUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PayersIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PayersUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@OfficersIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@OfficersUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ProductIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ProductUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ProductItemsIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ProductItemsUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ProductServicesIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ProductServicesUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@RelDistrIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@RelDistrUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ClaimAdminIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@ClaimAdminUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)

        data.params("@OfficerVillageIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@OfficerVillageUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)


        data.params("@xtICDCodes", dtICD)
        data.params("@xtHF", dtHF)
        data.params("@xtOfficers", dtOfficer)
        data.params("@xtPayers", dtPayer)
        data.params("@xtProduct", dtProduct)
        data.params("@xtProductItems", dtProductItems)
        data.params("@xtProductServices", dtProductServices)
        data.params("@xtRelDistr", dtRelDistr)
        data.params("@xtClaimAdmin", dtClaimAdmin)
        data.params("@xtVillageOfficer", dtOfficerVillage)

        dsResult = data.FilldataSet()

        eExtractInfo.ICDIns = data.sqlParameters("@ICDIns")
        eExtractInfo.ICDUpd = data.sqlParameters("@ICDUpd")
        eExtractInfo.HFIns = data.sqlParameters("@HFIns")
        eExtractInfo.HFUpd = data.sqlParameters("@HFUpd")
        eExtractInfo.OfficerIns = data.sqlParameters("@OfficersIns")
        eExtractInfo.OfficerUpd = data.sqlParameters("@OfficersUpd")
        eExtractInfo.PayerIns = data.sqlParameters("@PayersIns")
        eExtractInfo.PayerUpd = data.sqlParameters("@PayersUpd")
        eExtractInfo.ProductIns = data.sqlParameters("@ProductIns")
        eExtractInfo.ProductUpd = data.sqlParameters("@ProductUpd")
        eExtractInfo.ProductItemsUpd = data.sqlParameters("@ProductItemsUpd")
        eExtractInfo.ProductItemsIns = data.sqlParameters("@ProductItemsIns")
        eExtractInfo.ProductServicesUpd = data.sqlParameters("@ProductServicesUpd")
        eExtractInfo.ProductServicesIns = data.sqlParameters("@ProductServicesIns")
        eExtractInfo.RelDistrUpd = data.sqlParameters("@relDistrUpd")
        eExtractInfo.RelDistrIns = data.sqlParameters("@RelDistrIns")
        eExtractInfo.ClaimAdminUpd = data.sqlParameters("@ClaimAdminUpd")
        eExtractInfo.ClaimAdminIns = data.sqlParameters("@ClaimAdminIns")

        Return True
    End Function
    Public Function ImportOfflineExtract4(ByRef eExtractInfo As IMIS_EN.eExtractInfo, ByRef dtFamilies As DataTable, ByRef dtInsuree As DataTable, ByRef dtPhoto As DataTable, ByRef dtPolicy As DataTable, ByRef dtPremium As DataTable, ByRef dtinsureePolicy As DataTable) As Boolean
        Dim dsResult As New DataSet
        data.setSQLCommand("uspImportOffLineExtract4", CommandType.StoredProcedure, timeout:=0)
        data.params("@HFID", SqlDbType.Int, eExtractInfo.HFID)
        data.params("@LocationId", SqlDbType.Int, eExtractInfo.LocationId)
        data.params("@AuditUser", SqlDbType.Int, eExtractInfo.AuditUserID)
        data.params("@FamiliesIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@FamiliesUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@InsureeIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@InsureeUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PhotoIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PhotoUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PolicyIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PolicyUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PremiumIns", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@PremiumUpd", SqlDbType.Int, Nothing, ParameterDirection.Output)

        data.params("@xtFamilies", dtFamilies)
        data.params("@xtInsuree", dtInsuree)
        data.params("@xtPhotos", dtPhoto)
        data.params("@xtPolicy", dtPolicy)
        data.params("@xtPremium", dtPremium)
        data.params("@xtInsureePolicy", dtinsureePolicy)

        dsResult = data.FilldataSet()


        eExtractInfo.FamiliesIns = data.sqlParameters("@FamiliesIns")
        eExtractInfo.FamiliesUpd = data.sqlParameters("@FamiliesUpd")
        eExtractInfo.InsureeIns = If(data.sqlParameters("@InsureeIns") Is DBNull.Value, 0, data.sqlParameters("@InsureeIns"))
        eExtractInfo.InsureeUpd = data.sqlParameters("@InsureeUpd")
        eExtractInfo.PhotoIns = data.sqlParameters("@PhotoIns")
        eExtractInfo.PhotoUpd = data.sqlParameters("@PhotoUpd")
        eExtractInfo.PolicyIns = If(data.sqlParameters("@PolicyIns") Is DBNull.Value, 0, data.sqlParameters("@PolicyIns"))
        eExtractInfo.PolicyUpd = data.sqlParameters("@PolicyUpd")
        eExtractInfo.PremiumIns = data.sqlParameters("@PremiumIns")
        eExtractInfo.PremiumUpd = data.sqlParameters("@PremiumUpd")

        Return True
    End Function
    Public Function GetDBLastRowVersion() As Int64
        Dim dsResult As New DataSet
        data.setSQLCommand("uspS_LRV", CommandType.StoredProcedure)
        data.params("@LRV", SqlDbType.BigInt, Nothing, ParameterDirection.Output)

        dsResult = data.FilldataSet()

        GetDBLastRowVersion = data.sqlParameters("@LRV")
    End Function
    Public Sub InsertExtract(ByVal eExtract As IMIS_EN.tblExtracts)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " INSERT INTO"
        sSQL += " tblExtracts (ExtractDirection ,ExtractType,ExtractSequence,ExtractDate,ExtractFileName,ExtractFolder,"
        sSQL += " LocationId,HFID,AppVersionBackend,AuditUserID,RowID)"
        sSQL += " VALUES"
        sSQL += " (@Direction,@ExtractType,@Sequence,@ExtractDate,@ExtractFileName,"
        sSQL += " @ExtractFolder,@LocationId,@HFID,@AppVersionBackend,@AuditUserID,@RowID);"
        sSQL += " SELECT @ExtractID = SCOPE_IDENTITY()"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ExtractID", SqlDbType.Int, eExtract.ExtractID, ParameterDirection.Output)
        data.params("@Direction", SqlDbType.TinyInt, eExtract.ExtractDirection)
        data.params("@ExtractType", SqlDbType.TinyInt, eExtract.ExtractType)
        data.params("@Sequence", SqlDbType.Int, eExtract.ExtractSequence)
        data.params("@ExtractDate", SqlDbType.DateTime, Now())
        data.params("@ExtractFileName", SqlDbType.NVarChar, 255, eExtract.extractFileName)
        data.params("@ExtractFolder", SqlDbType.NVarChar, 255, eExtract.extractFolder)
        data.params("@LocationId", SqlDbType.Int, eExtract.LocationId)
        data.params("@HFID", SqlDbType.Int, eExtract.HFID)
        data.params("@AppVersionBackend", SqlDbType.Decimal, eExtract.AppVersionBackend)
        data.params("@AuditUserID", SqlDbType.Int, eExtract.AuditUserID)
        data.params("@RowID", SqlDbType.BigInt, eExtract.RowID)
        data.ExecuteCommand()

        eExtract.ExtractID = data.sqlParameters("@ExtractID")

    End Sub
    Public Function GetExtract(ByVal ExtractID As Integer) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("Select * FROM tblExtracts WHERE  ExtractID = @ExtractId", CommandType.Text)
        data.params("@ExtractId", SqlDbType.Int, ExtractID)
        Return data.Filldata
    End Function

    '@ExtractFileName ADDED BY AMANI 27/09/2017
    Public Function GetLastFullExtractID(ByVal LocationId As Integer, Optional ByVal ExtractFileName As String = "") As Int64
        Dim data As New ExactSQL
        Dim dt As New DataTable
        Dim sSQL As String = ""
        sSQL = "Select ISNULL(MAX(ExtractID),0) as MaxExtractID FROM tblExtracts WHERE LocationId = @LocationId AND ExtractType = 2   AND ExtractDirection = 0"

        'AMANI 02/10/2017
        If ExtractFileName <> "" Then sSQL += " AND ExtractFileName like @ExtractFileName "
        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@LocationId", SqlDbType.Int, LocationId)
        data.params("@ExtractFileName", SqlDbType.NVarChar, 20, ExtractFileName)

        dt = data.Filldata
        If dt.Rows.Count = 0 Then
            GetLastFullExtractID = 0
        Else
            GetLastFullExtractID = dt.Rows(0)("MaxExtractID")
        End If

    End Function

    'Corrected by Rogers
    Public Function GetPhoneExtractSource(ByVal LocationId As Integer) As DataTable

        Dim data As New ExactSQL
        Dim sSQL As String = ""

        sSQL = "uspPhoneExtract"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure, , 0)
        'data.params("@CHFID", SqlDbType.NVarChar, 12, "")
        data.params("@LocationId", SqlDbType.Int, LocationId)


        Return data.Filldata

    End Function
    Public Sub SubmitClaimFromXML(ByVal FileName As String)
        Dim data As New ExactSQL
        Dim sSQL As String = "uspUpdateClaimFromPhone"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@FileName", SqlDbType.NVarChar, 255, FileName)

        data.ExecuteCommand()
    End Sub
    Public Function GetEnrolmentXML(ByVal Output As Dictionary(Of String, Integer)) As DataTable
        Dim sSQL As String = "uspCreateEnrolmentXML"
        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@FamilyExported", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@InsureeExported", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@PolicyExported", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@PremiumExported", SqlDbType.Int, 0, ParameterDirection.Output)
        Dim dt As DataTable = data.Filldata
        If Not Output Is Nothing Then
            Output("FamilyExported") = data.sqlParameters("@FamilyExported")
            Output("InsureeExported") = data.sqlParameters("@InsureeExported")
            Output("PolicyExported") = data.sqlParameters("@PolicyExported")
            Output("PremiumExported") = data.sqlParameters("@PremiumExported")
        End If
        Return dt
    End Function
    Public Sub DeleteAllLocalRecords()
        Dim sSQL As String = ""
        sSQL = "DELETE FROM  tblInsureePolicy WHERE isOffline = 1;" & _
               "DELETE FROM  tblPremium WHERE isOffline = 1;" & _
               "DELETE FROM  tblPolicy WHERE isOffline = 1;" & _
               "DELETE FROM tblInsuree WHERE isOffline = 1;" & _
               "DELETE FROM tblFamilies WHERE isOffline = 1"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.ExecuteCommand()
    End Sub
    
    Public Function UploadEnrolments(ByVal File As String, ByVal Output As Dictionary(Of String, Integer)) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = "uspUploadEnrolments"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@File", SqlDbType.NVarChar, 300, File)
        data.params("@RV", SqlDbType.Int, 0, ParameterDirection.ReturnValue)
        data.params("@FamilySent", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@InsureeSent", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@PolicySent", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@PremiumSent", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@FamilyImported", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@InsureeImported", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@PolicyImported", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@PremiumImported", SqlDbType.Int, 0, ParameterDirection.Output)
        'get the last datatable which is result table
        Dim ds As DataSet = data.FilldataSet()
        Dim dt As DataTable = ds.Tables(ds.Tables.Count - 1)



        Output("FamilySent") = data.sqlParameters("@FamilySent")
        Output("InsureeSent") = data.sqlParameters("@InsureeSent")
        Output("PolicySent") = data.sqlParameters("@PolicySent")
        Output("PremiumSent") = data.sqlParameters("@PremiumSent")
        Output("FamilyImported") = data.sqlParameters("@FamilyImported")
        Output("InsureeImported") = data.sqlParameters("@InsureeImported")
        Output("PolicyImported") = data.sqlParameters("@PolicyImported")
        Output("PremiumImported") = data.sqlParameters("@PremiumImported")
        Return dt
    End Function

    Public Function isFullExtractExists() As Boolean
        Dim sSQL As String
        Dim data As New ExactSQL
        sSQL = "SELECT 1 FROM tblExtracts "
        sSQL += " WHERE ExtractSequence = (SELECT Max(ExtractSequence) ExtractSequence FROM tblExtracts WHERE ExtractDirection =1 AND (ExtractType = 2 OR ExtractType = 8)) "
        sSQL += " AND ExtractDirection = 1 "
        sSQL += " AND ExtractType = 2 "
        data.setSQLCommand(sSQL, CommandType.Text)
        If data.Filldata.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function NewSequenceNumber(ByVal LocationId As Integer) As Integer
        Dim sSQL As String
        Dim data As New ExactSQL
        sSQL = "SELECT Max(ExtractSequence) ExtractSequence FROM tblExtracts WHERE ExtractDirection = 0 AND  ExtractType = 4 AND LocationId= @LocationId"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, LocationId)
        Dim dt As DataTable = data.Filldata
        Return If(dt.Rows(0)("ExtractSequence") Is DBNull.Value, 1, dt.Rows(0)("ExtractSequence") + 1)
    End Function
End Class
