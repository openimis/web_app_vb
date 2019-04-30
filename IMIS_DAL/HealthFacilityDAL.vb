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

Public Class HealthFacilityDAL
    Public Function CheckIfHFExists(ByVal eHF As IMIS_EN.tblHF) As Boolean
        Dim data As New ExactSQL
        Dim str As String = "Select Count(*) from tblHF where HFName = @HFName AND HFAddress = @HFAddress AND LegacyID <> @HfID AND ValidityTo IS NULL"
        If Not eHF.HfID = 0 Then
            str += " AND tblHF.HfID <> @HfID "
        End If
        data.setSQLCommand(str, CommandType.Text)
        data.params("@HFName", SqlDbType.NVarChar, 100, eHF.HFName)
        data.params("@HFAddress", SqlDbType.NVarChar, 100, eHF.HFAddress)
        data.params("@HfID", SqlDbType.Int, eHF.HfID)

        Return data.ExecuteScalar()
    End Function
    Public Sub DeleteHealthFacility(ByRef eHF As IMIS_EN.tblHF)

        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " INSERT INTO tblHF ([HFName],[LegalForm],[HFLevel],[HFCode],[HFAddress],[LocationId],[Phone],[Fax],[eMail],[HFCareType],"
        sSQL += " [PLServiceID],[PLItemID],[AccCode],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID])"
        sSQL += " SELECT [HFName],[LegalForm],[HFLevel],[HFCode],[HFAddress],[LocationId],[Phone],[Fax],[eMail],[HFCareType],[PLServiceID],"
        sSQL += " [PLItemID],[AccCode],[ValidityFrom],GETDATE(),@HfID,[AuditUserID]"
        sSQL += " FROM tblHF WHERE HfID = @HfID;"
        sSQL += " UPDATE [tblHF] SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate(), [AuditUserID] = @AuditUserID  WHERE HfID= @HfID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@HfID", SqlDbType.Int, eHF.HfID)
        data.params("@AuditUserID", SqlDbType.Int, eHF.AuditUserID)
        data.ExecuteCommand()

    End Sub
    Public Sub getHFCodeAndName(ByRef eHF As IMIS_EN.tblHF)
        Dim data As New ExactSQL

        data.setSQLCommand("select top 1 HFCode,HFName from tblHF where HFID = @HFID;", CommandType.Text)
        data.params("@HfID", SqlDbType.Int, eHF.HfID)
        Dim dt As DataTable = data.Filldata()
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)

            eHF.HFCode = dr("HFCode")
            eHF.HFName = dr("HFName")
        End If
    End Sub
    Public Function GetHF(ByVal eHF As IMIS_EN.tblHF, ByVal dtHFLEVEL As DataTable, ByVal dtLegal As DataTable, ByVal All As Boolean, ByVal dtHFCareType As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        'Dim strSQL As String = "select tblHF.*,'Region' RegionName,DistrictName,HFCareTypes.Name as HFCareType2, LEGAL.LegalForms as Legal, HFLEVEL.Name as HFLevelName from tblHF inner join tblDistricts on tblHF.LocationId = tblDistricts.DistrictID inner join tblUsersDistricts UD on UD.LocationId = tblHF.LocationId and UD.userid = @userid and UD.ValidityTo is null inner join @dtHFLevel HFLEVEL on tblHF.HFLevel = HFLEVEL.CODE inner join @dtHFCareType HFCareTypes on HFCareTypes.Code = tblHF.HFCareType  inner join tblLegalforms LEGAL on tblHF.LegalForm = LEGAL.LegalFormCode WHERE CASE WHEN @DistrictID = 0 THEN 0 ELSE tblHF.LocationId END = @DistrictID "

        sSQL = " SELECT HF.HfID,HF.HFCode, HF.HFName,   CASE U.LanguageID WHEN 'EN' THEN LEGAL.LegalForms   ELSE LEGAL.AltLanguage END Legal  , HFLEVEL.Name  HFLevelName, HF.Phone,HFCareTypes.Name  HFCareType2, HF.ValidityFrom, HF.ValidityTo, L.RegionName , L.DistrictName "
        sSQL += " FROM tblHF HF"
        sSQL += " INNER JOIN @dtHFLevel HFLEVEL on HF.HFLevel = HFLEVEL.CODE INNER JOIN @dtHFCareType HFCareTypes on HFCareTypes.Code = HF.HFCareType"
        sSQL += " INNER JOIN tblLegalforms LEGAL on HF.LegalForm = LEGAL.LegalFormCode"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(HF.LocationId, 0)"
        sSQL += " INNER JOIN"
        sSQL += " (SELECT UD.UserId, L.DistrictId, L.RegionId"
        sSQL += " FROM tblUsersDistricts UD"
        sSQL += " INNER JOIN uvwLocations L ON L.DistrictId = UD.LocationId"
        sSQL += " WHERE UD.ValidityTo IS NULL"
        sSQL += " AND (UD.UserId = @UserId OR @UserId = 0)"
        sSQL += " GROUP BY UD.UserId, L.DistrictId, L.RegionId"
        sSQL += " )UD ON UD.DistrictId = HF.LocationId"
        sSQL += " INNER JOIN tblUsers U ON U.UserId = UD.UserId"

        sSQL += " WHERE (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId = 0)"
        sSQL += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL)"


        If Not All = True Then
            sSQL += " AND HF.ValidityTo is NULL"
        End If

        If Not eHF.HFCode = Nothing Then
            sSQL += "  AND  HF.HFCode LIKE @HFCode + '%'"
        End If
        If Not eHF.HFName = Nothing Then
            sSQL += " AND  HF.HFName LIKE @HFName + '%'"
        End If
        If Not eHF.Phone = Nothing Then
            sSQL += " AND  HF.Phone LIKE @Phone + '%'"
        End If
        If Not eHF.Fax = Nothing Then
            sSQL += " AND  HF.Fax LIKE @Fax + '%'"
        End If
        If Not eHF.eMail = Nothing Then
            sSQL += " AND  HF.eMail LIKE @Email + '%'"
        End If
        If Not eHF.HFCareType = Nothing Then
            sSQL += " AND  HF.HFCareType LIKE @HFCareType + '%'"
        End If
        If Not eHF.HFLevel = Nothing Then
            sSQL += " AND HF.HFLevel LIKE @HFLevel + '%'"
        End If
        If Not eHF.LegalForm = Nothing Then
            sSQL += " AND LegalForm LIKE @HFLegal + '%'"
        End If

        sSQL += " GROUP BY HF.HfID,HF.HFCode, HF.HFName, CASE U.LanguageID WHEN 'EN' THEN LEGAL.LegalForms ELSE LEGAL.AltLanguage END, HFLEVEL.Name, HF.Phone,HFCareTypes.Name , HF.ValidityFrom, HF.ValidityTo, L.RegionName , L.DistrictName "

        sSQL += " ORDER BY  HF.HFCode,HF.ValidityFrom DESC"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, eHF.AuditUserID)
        data.params("@Phone", SqlDbType.NVarChar, 50, eHF.Phone)
        data.params("@RegionId", SqlDbType.Int, eHF.tblLocations.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eHF.tblLocations.DistrictID)
        data.params("@Fax", SqlDbType.NVarChar, 50, eHF.Fax)
        data.params("@eMail", SqlDbType.NVarChar, 50, eHF.eMail)
        data.params("@dtHFLevel", dtHFLEVEL, "xAttributeV")
        'data.params("@dtLegal", dtLegal, "xAttributeV")
        data.params("@dtHFCareType", dtHFCareType, "xAttributeV")

        If Not eHF.HFCode = Nothing Then
            data.params("@HFCode", SqlDbType.NVarChar, 8, eHF.HFCode)
        End If
        If Not eHF.HFName = Nothing Then
            data.params("@HFName", SqlDbType.NVarChar, 100, eHF.HFName)
        End If
        If Not eHF.HFCareType Is Nothing Then
            data.params("@HFCareType", SqlDbType.NVarChar, 1, eHF.HFCareType)
        End If
        If Not eHF.HFLevel = Nothing Then
            data.params("@HFLevel", SqlDbType.NVarChar, 1, eHF.HFLevel)
        End If
        If Not eHF.LegalForm = Nothing Then
            data.params("@HFLegal", SqlDbType.NVarChar, 1, eHF.LegalForm)
        End If

        Return data.Filldata

    End Function
    'Corrected
    Public Function GetFSPHF(DistrictId As Integer, HFLevel As String) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = "SELECT HFID, HFCode + '-' + HFName HFCode FROM tblHF WHERE ValidityTo Is NULL AND LocationID = @LocationId AND HFLevel = @HFLevel"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, DistrictId)
        data.params("@HFLevel", SqlDbType.NVarChar, 1, HFLevel)
        Return data.Filldata
    End Function
    Public Function GetHFLegacy(ByVal eHF As IMIS_EN.tblHF) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("select tblHF.*,DistrictName, CASE HFCareType when '0' then 'Out-Patient' when 'I' then 'IN-Patient' else 'Both' end as HFCareType2, " _
                                & " CASE  LegalForm when 'G' then 'Government' when 'D' then 'District Org.' when 'P' then 'Private Org.' when 'C' Then 'Charity' else '' end as Legalform, " _
                                 & " CASE  HFLevel when 'D' then 'Dispensary' when 'C' then 'Health Centre' when 'H' then 'Hospital' else '' end as HFLevel from tblHF " _
                                 & "inner join tblDistricts on tblHF.DistrictID = tblDistricts.DistrictID  WHERE HFName LIKE @HFName AND Phone LIKE @Phone AND CASE WHEN @DistrictID = 0 THEN 0 ELSE tblHF.DistrictID END = @DistrictID AND eMail LIKE @Email AND Fax LIKE @Fax AND HFCareType LIKE @HFCareType AND HFLevel LIKE @HFLevel AND LegalForm LIKE @HFLegal order by HFCode", CommandType.Text)
        data.params("@HFName", SqlDbType.NVarChar, 100, eHF.HFName)
        data.params("@Phone", SqlDbType.NVarChar, 50, eHF.Phone)
        data.params("@DistrictID", SqlDbType.Int, eHF.tblLocations.LocationId)
        data.params("@Fax", SqlDbType.NVarChar, 50, eHF.Fax)
        data.params("@eMail", SqlDbType.NVarChar, 50, eHF.eMail)
        data.params("@HFCareType", SqlDbType.NVarChar, 1, eHF.HFCareType)
        data.params("@HFLevel", SqlDbType.NVarChar, 1, eHF.HFLevel)
        data.params("@HFLegal", SqlDbType.NVarChar, 1, eHF.LegalForm)
        Return data.Filldata
    End Function
    'Corrected by Rogers
    Public Sub InsertHealthFacility(ByRef eHF As IMIS_EN.tblHF)
        Dim data As New ExactSQL
        data.setSQLCommand("Insert into tblHF([HFName],[LegalForm],[HFLevel],[HFSublevel],[HFCode],[HFAddress],[LocationId],[Phone],[Fax],[eMail],[HFCareType],[PLServiceID],[PLItemID],[AccCode],[AuditUserID])" _
        & "VALUES(@HFName, @LegalForm, @HFLevel, @HFSublevel,@HFCode, @HFAddress, @LocationId, @Phone, @Fax, @eMail, @HFCareType, @PLServiceID, @PLItemID,@AccCode,@AuditUserID); SELECT @HfId = SCOPE_IDENTITY()", CommandType.Text)
        data.params("@HFName", SqlDbType.NVarChar, 100, eHF.HFName)
        data.params("@LegalForm", SqlDbType.Char, 1, eHF.LegalForm)
        data.params("@HFLevel", SqlDbType.Char, 1, eHF.HFLevel)
        data.params("@HFSubLevel", SqlDbType.Char, 1, If(eHF.HFSublevel.Trim.Length = 0, Nothing, eHF.HFSublevel))
        data.params("@HFCode", SqlDbType.NVarChar, 8, eHF.HFCode)
        data.params("@HFAddress", SqlDbType.NVarChar, 100, eHF.HFAddress)
        data.params("@LocationId", SqlDbType.Int, eHF.tblLocations.LocationId)
        data.params("@Phone", SqlDbType.NVarChar, 50, eHF.Phone)
        data.params("@Fax", SqlDbType.NVarChar, 50, eHF.Fax)
        data.params("@eMail", SqlDbType.NVarChar, 50, eHF.eMail)
        data.params("@HFCareType", SqlDbType.Char, 1, eHF.HFCareType)
        data.params("@PLServiceID", SqlDbType.Int, if(eHF.tblPLServices.PLServiceID = 0, DBNull.Value, eHF.tblPLServices.PLServiceID))
        data.params("@PLItemID", SqlDbType.Int, if(eHF.tblPLItems.PLItemID = 0, DBNull.Value, eHF.tblPLItems.PLItemID))
        data.params("@AccCode", SqlDbType.NVarChar, 25, eHF.AccCode)
        data.params("@AuditUserID", SqlDbType.Int, eHF.AuditUserID)
        data.params("@HfId", SqlDbType.Int, eHF.HfID, ParameterDirection.Output)
        data.ExecuteCommand()
        eHF.HfID = data.sqlParameters("@HfId")
    End Sub
    'Corrected by Rogers
    Public Sub UpdateHealthFacility(ByRef eHF As IMIS_EN.tblHF)

        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblHF ([HFName],[LegalForm],[HFLevel],[HFSublevel],[HFCode],[HFAddress],[LocationId],[Phone],[Fax],[eMail],[HFCareType],[PLServiceID],[PLItemID],[AccCode],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID])" _
      & " select [HFName],[LegalForm],[HFLevel],[HFSublevel],[HFCode],[HFAddress],[LocationId],[Phone],[Fax],[eMail],[HFCareType],[PLServiceID],[PLItemID],[AccCode],[ValidityFrom],getdate(),[HfID],[AuditUserID] from tblHF where HfID = @HfID;" _
      & "UPDATE [tblHF] SET [HFName] = @HFName,[LegalForm] = @LegalForm,[HFLevel] = @HFLevel,HFSublevel = @HFSublevel,[HFCode]=@HFCode,[HFAddress] = @HFAddress,[LocationId] = @LocationId,[Phone] = @Phone ,[Fax] = @Fax" _
      & ",[eMail] = @eMail,[HFCareType] = @HFCareType,[PLServiceID] = @PLServiceID,[PLItemID] = @PLItemID,[AccCode] = @AccCode, [ValidityFrom] = GetDate()" _
      & ",[LegacyID] = @LegacyID,[AuditUserID] = @AuditUserID  WHERE HfID= @HfID", CommandType.Text)
        data.params("@HfID", SqlDbType.Int, eHF.HfID)
        data.params("@HFName", SqlDbType.NVarChar, 100, eHF.HFName)
        data.params("@LegalForm", SqlDbType.Char, 1, eHF.LegalForm)
        data.params("@HFLevel", SqlDbType.Char, 1, eHF.HFLevel)
        data.params("@HFSubLevel", SqlDbType.Char, 1, If(eHF.HFSublevel.Trim.Length = 0, Nothing, eHF.HFSublevel))
        data.params("@HFCode", SqlDbType.NVarChar, 8, eHF.HFCode)
        data.params("@HFAddress", SqlDbType.NVarChar, 100, eHF.HFAddress)
        data.params("@LocationId", SqlDbType.Int, eHF.tblLocations.LocationId)
        data.params("@Phone", SqlDbType.NVarChar, 50, eHF.Phone)
        data.params("@Fax", SqlDbType.NVarChar, 50, eHF.Fax)
        data.params("@eMail", SqlDbType.NVarChar, 50, eHF.eMail)
        data.params("@HFCareType", SqlDbType.Char, 1, eHF.HFCareType)
        data.params("@PLServiceID", SqlDbType.Int, if(eHF.tblPLServices.PLServiceID = 0, DBNull.Value, eHF.tblPLServices.PLServiceID))
        data.params("@PLItemID", SqlDbType.Int, if(eHF.tblPLItems.PLItemID = 0, DBNull.Value, eHF.tblPLItems.PLItemID))
        data.params("@AccCode", SqlDbType.NVarChar, 25, eHF.AccCode)
        data.params("@LegacyID", SqlDbType.Int, eHF.HfID)
        data.params("@AuditUserID", SqlDbType.Int, eHF.AuditUserID)
        data.ExecuteCommand()

    End Sub
    Public Sub LoadHF(ByRef eHF As IMIS_EN.tblHF)
        Dim eLocations As New IMIS_EN.tblLocations
        Dim ePLService As New IMIS_EN.tblPLServices
        Dim ePLItem As New IMIS_EN.tblPLItems
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " SELECT [HFName],[LegalForm],[HFLevel],[HFSublevel],[HFCode],[HFAddress],RegionId,[LocationId] DistrictId,[Phone],[Fax],[eMail],"
        sSQL += " [HFCareType],isnull(PLServiceID,0) PLServiceID, isnull(PLItemID,0) PLItemID,[AccCode],HF.ValidityTo,HF.[AuditUserID]"
        sSQL += " FROM tblHF HF"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId= HF.LocationId"
        sSQL += " INNER JOIN tblRegions R ON R.RegionId=D.Region"
        sSQL += " WHERE  HfID = @HFID"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@HFID", SqlDbType.Int, eHF.HfID)
        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            eHF.HFName = dr("HFName")
            eHF.LegalForm = dr("LegalForm")
            eHF.HFLevel = dr("HFLevel")
            eHF.HFSublevel = dr("HFSublevel").ToString
            eHF.HFCode = dr("HFCode")
            eHF.HFAddress = dr("HFAddress")
            eHF.RegionId = dr("RegionId")
            eLocations.LocationId = dr("DistrictId")
            eHF.tblLocations = eLocations
            eHF.Phone = dr("Phone")
            eHF.Fax = dr("Fax")
            eHF.eMail = dr("eMail")
            eHF.HFCareType = dr("HFCareType")
            ePLService.PLServiceID = dr("PLServiceID")
            eHF.tblPLServices = ePLService
            ePLItem.PLItemID = dr("PLItemID")
            eHF.AccCode = dr("AccCode")
            eHF.tblPLItems = ePLItem

            eHF.ValidityTo = if(dr("ValidityTo").ToString = String.Empty, Nothing, dr("ValidityTo"))
        End If

    End Sub
    'Corrected
    Public Function GetHFCodes(ByVal UserId As Integer, ByVal LocationId As Integer, Optional ByRef Hfid As Integer = 0) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "SELECT @hfid = isnull(hfid,0)"
        sSQL += " FROM tblUsers"
        sSQL += " WHERE UserID = @userid;"
        sSQL += " SELECT tblhf.HfID,HFCode + ' - ' + HFNAME HFCODE"
        sSQL += " FROM tblHF"
        sSQL += " INNER JOIN tblusersdistricts on tblhf.LocationId = tblusersdistricts.LocationId"
        sSQL += " AND tblusersdistricts.validityto is null"
        sSQL += " AND tblusersdistricts.userid = @UserId"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId = tblusersdistricts.LocationId"
        sSQL += " INNER JOIN tblRegions R ON R.RegionId = D.Region"
        sSQL += " where tblHF.validityto IS NULL"
        sSQL += " AND (D.DistrictId = @LocationId OR R.RegionId = @LocationId OR @LocationId  = 0)"
        sSQL += " AND CASE WHEN  @hfid = 0 THEN 0 ELSE tblhf.hfid END = @hfid"
        sSQL += " ORDER BY hfCode"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, UserId)
        data.params("@LocationId", SqlDbType.Int, If(LocationId = -1, 0, LocationId))
        data.params("@hfId", SqlDbType.Int, Hfid, ParameterDirection.Output)

        Dim dt As DataTable = data.Filldata
        Hfid = data.sqlParameters("@hfid")
        Return dt
    End Function
    Public Function getHFCodeFromID(ByVal HFID As Integer) As String
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "SELECT HFCode FROM tblHF WHERE HFID = @HFID"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@HFID", HFID)
        Return data.Filldata.Rows(0)("hfcode").ToString
    End Function

    Public Function UploadHF(ByVal Xml As System.Xml.XmlDocument, ByVal StrategyId As Integer, ByVal AuditUserID As Integer, ByRef dtresult As DataTable, Optional ByVal dryRun As Boolean = 0) As Dictionary(Of String, Integer)
        Dim data As New ExactSQL
        Dim sSQL As String = "uspUploadHFXML"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@XML", Xml.InnerXml)
        data.params("@StrategyId", StrategyId)
        data.params("@AuditUserID", AuditUserID)
        data.params("@DryRun", dryRun)

        data.params("@SentHF", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@Inserts", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@Updates", SqlDbType.Int, Nothing, ParameterDirection.Output)
        'data.params("@sentCatchment", SqlDbType.Int, Nothing, ParameterDirection.Output)
        'data.params("@InsertCatchment", SqlDbType.Int, Nothing, ParameterDirection.Output)
        'data.params("@UpdateCatchment", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@returnValue", SqlDbType.Int, Nothing, ParameterDirection.ReturnValue)
        dtresult = data.Filldata()


        Dim Output As New Dictionary(Of String, Integer)
        Output.Add("SentHF", 0)
        Output.Add("Inserts", 0)
        Output.Add("Updates", 0)
        'Output.Add("sentCatchment", 0)
        'Output.Add("InsertCatchment", 0)
        'Output.Add("UpdateCatchment", 0)

        Output.Add("returnValue", 0)

        Output("SentHF") = If(data.sqlParameters("@SentHF") Is DBNull.Value, 0, data.sqlParameters("@SentHF"))
        Output("Inserts") = If(data.sqlParameters("@Inserts") Is DBNull.Value, 0, data.sqlParameters("@Inserts"))
        Output("Updates") = If(data.sqlParameters("@Updates") Is DBNull.Value, 0, data.sqlParameters("@Updates"))
        'Output("sentCatchment") = If(data.sqlParameters("@sentCatchment") Is DBNull.Value, 0, data.sqlParameters("@sentCatchment"))
        'Output("InsertCatchment") = If(data.sqlParameters("@InsertCatchment") Is DBNull.Value, 0, data.sqlParameters("@InsertCatchment"))
        'Output("UpdateCatchment") = If(data.sqlParameters("@UpdateCatchment") Is DBNull.Value, 0, data.sqlParameters("@UpdateCatchment"))

        Output("returnValue") = data.sqlParameters("@returnValue")
        Return Output
    End Function

    Public Function downLoadHFXML() As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = " SELECT(SELECT ISNULL([LegalForm], '') LegalForm, ISNULL([HFLevel], '') [Level],ISNULL([HFSublevel], '') SubLevel, [HFCode] Code, [HFName] Name, "
        sSQL += " ISNULL([HFAddress], '') [Address], D.DistrictCode, D.DistrictName, ISNULL(Phone, '')Phone, ISNULL(Fax, '')Fax, ISNULL(eMail, '') Email, ISNULL(HFCareType, '') CareType, "
        sSQL += " ISNULL(AccCode, '') AccountCode, PLI.PLItemName ItemPriceListName,  PLS.PLServName ServicePriceListName"
        sSQL += " FROM tblHF HF"
        sSQL += " INNER JOIN tblDistricts D ON HF.LocationId = D.DistrictId"
        sSQL += " LEFT OUTER JOIN tblPLItems PLI ON PLI.PLItemID = HF.PLItemID"
        sSQL += " LEFT OUTER JOIN tblPLServices PLS ON PLS.PLServiceID = HF.PLServiceID"
        sSQL += " WHERE HF.ValidityTo IS NULL"
        sSQL += " AND  D.ValidityTo IS NULL"
        sSQL += " AND PLI.ValidityTo IS NULL AND PLS.ValidityTo IS NULL"
        sSQL += " FOR XML PATH('HealthFacility'),ROOT('HealthFacilityDetails'),TYPE),"
        sSQL += " (SELECT HF2.HFCode, ISNULL(L.LocationCode,'') VillageCode, L.LocationName VillageName, ISNULL(C.Catchment,'')Percentage FROM  tblHFCatchment C"
        sSQL += " INNER JOIN tblHF HF2 ON C.HFID=HF2.HfID"
        sSQL += " INNER JOIN tblLocations L ON L.LocationId=C.LocationId"
        sSQL += " WHERE HF2.ValidityTo IS NULL"
        sSQL += " AND C.ValidityTo IS NULL"
        sSQL += " And L.ValidityTo IS NULL"
        sSQL += " FOR XML PATH('Catchment'),ROOT('CatchmentDetails'),TYPE)"
        sSQL += " FOR XML PATH(''),ROOT('HealthFacilities') "

        data.setSQLCommand(sSQL, CommandType.Text)
        Return data.Filldata()
    End Function
    Public Function getHFUserLocation(ByVal UserId As Integer, ByVal Hfid As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "SELECT  D.DistrictId, UD.UserDistrictID,UD.LocationId FROM tblHF HF "
        sSQL += " INNER JOIN tblUsersDistricts UD ON UD.LocationId = HF.LocationId "
        sSQL += " AND UD.ValidityTo IS NULL"
        sSQL += " AND UD.UserID = @UserID"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId = UD.LocationId"
        sSQL += " INNER JOIN tblRegions R ON R.RegionId = D.Region"
        sSQL += " WHERE HF.ValidityTo IS NULL"
        sSQL += " AND CASE WHEN  @HfID = 0 THEN 0 ELSE HF.HfID END = @HfID"


        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, UserId)
        data.params("@HfID", SqlDbType.Int, Hfid)

        Return data.Filldata
    End Function
End Class
