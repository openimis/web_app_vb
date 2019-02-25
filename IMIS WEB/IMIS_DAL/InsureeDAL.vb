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

Public Class InsureeDAL
    Dim data As New ExactSQL
    Public Function GetInsureesByFamily(ByVal FamilyId As Integer, Optional Language As String = "en") As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += "SELECT FamilyID, InsureeId, CHFID, LastName, OtherNames, DOB,"
        sSQL += "" & IIf(Language = "en", "GE.Gender", "ISNULL(GE.AltLanguage,GE.Gender) Gender")
        sSQL += ", Marital,cardIssued, isOffline, validityFrom, ValidityTo,RowID from tblInsuree TB"
        sSQL += " LEFT JOIN tblGender GE On GE.Code = TB.Gender"


        sSQL += " WHERE TB.familyid = @FamilyId"
        sSQL += " AND TB.LegacyID is null and TB.ValidityTo is null AND RowID > 0  ORDER BY CHFID DESC "
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@FamilyId", SqlDbType.Int, FamilyId)
        Return data.Filldata
    End Function
    Public Sub GetInsureesByCHFID(ByRef einsuree As IMIS_EN.tblInsuree)
        'HVH CHANGED relationhip !!
        data.setSQLCommand("select FamilyID,InsureeId,CHFID,LastName,OtherNames,ishead,isOffline,Relationship,Profession,Education,Email  from tblInsuree where  CHFID = @CHFID and validityto is null", CommandType.Text)
        data.params("@CHFID", SqlDbType.NVarChar, 12, einsuree.CHFID)
        'HVH CHANGED relationhip !!
        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            Dim efamily As New IMIS_EN.tblFamilies
            efamily.FamilyID = dr("FamilyID")
            einsuree.tblFamilies1 = efamily
            einsuree.InsureeID = dr("InsureeID")
            einsuree.CHFID = dr("CHFID")
            einsuree.LastName = dr("LastName")
            einsuree.IsHead = dr("IsHead")
            einsuree.OtherNames = dr("OtherNames")
            'HVH CHANGE  need to antociptae on null as it is optional
            einsuree.Relationship = Integer.Parse((if(dr("Relationship") Is DBNull.Value, 0, dr("Relationship"))))
            einsuree.Profession = Integer.Parse(if(dr("Profession") Is DBNull.Value, 0, dr("Profession")))
            einsuree.Education = Integer.Parse(if(dr("Education") Is DBNull.Value, 0, dr("Education")))
            'HVH CHANGE
            einsuree.Email = dr("Email").ToString
            If dr("isOffline") IsNot DBNull.Value Then
                einsuree.isOffline = dr("isOffline")
            End If
        Else
            einsuree.CHFID = String.Empty
        End If
    End Sub
    Public Function GetCHFNumbers() As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = "SELECT tblinsuree.insureeid, chfid,othernames,lastname from  tblinsuree where validityto is null"
        data.setSQLCommand(sSQL, CommandType.Text)
        Return data.Filldata
    End Function

    'Corrected

    Public Function GetInsureeFullSearch(ByVal eInsuree As IMIS_EN.tblInsuree, Optional ByVal All As Boolean = False, Optional ByVal PhotoAssigned As Int16 = 1, Optional Language As String = "en", Optional dtMarital As DataTable = Nothing) As DataTable
        Dim sSQL As String = ""
        'sSQL += " SELECT I.isOffline,I.FamilyID,I.InsureeID, RegionName,DistrictName,WardName,VillageName,LastName,Othernames, I.CHFID,Gender,Marital,phone,DOB,  I.validityfrom,I.validityTo"
        'sSQL += " FROM tblInsuree I"
        'sSQL += " INNER JOIN tblFamilies F ON I.FamilyID = F.FamilyID"
        'sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId,0) = ISNULL(F.LocationId,0)"
        'sSQL += " INNER JOIN (SELECT L.DistrictId, L.RegionId FROM tblUsersDistricts UD"
        'sSQL += " INNER JOIN uvwLocations L ON L.DistrictId = UD.LocationId WHERE UD.ValidityTo IS NULL AND (UD.UserId = @UserId OR @UserId = 0)"
        'sSQL += " GROUP BY L.DistrictId, L.RegionId )UD ON UD.DistrictId = L.DistrictId  OR UD.RegionId = L.RegionId OR F.LocationId IS NULL"
        'sSQL += " LEFT JOIN tblPhotos on I.PhotoID = tblPhotos.PhotoID AND tblPhotos.ValidityTo is null"

        sSQL += " ;WITH UD AS( "
        sSQL += " SELECT L.DistrictId, L.Region FROM tblUsersDistricts UD  "
        sSQL += " INNER JOIN tblDistricts L ON L.DistrictId = UD.LocationId  "
        sSQL += " WHERE UD.ValidityTo IS NULL AND (UD.UserId = @UserId OR @UserId = 0)  "
        sSQL += " GROUP BY L.DistrictId, L.Region ) "
        sSQL += " SELECT I.isOffline,I.FamilyID,I.InsureeID, RegionName,DistrictName,WardName,VillageName,LastName,Othernames, I.CHFID,"
        sSQL += "" & IIf(Language = "en", "GE.Gender", "ISNULL(GE.AltLanguage,GE.Gender)") & " Gender"
        sSQL += ",dtMarital.Name Marital, phone, DOB, I.validityfrom, I.validityTo  "
        sSQL += " FROM tblInsuree I  "
        sSQL += " INNER JOIN tblFamilies F On I.FamilyID = F.FamilyID  "
        sSQL += " INNER JOIN uvwLocations L On ISNULL(L.LocationId, 0) = ISNULL(F.LocationId, 0)  "
        sSQL += " LEFT JOIN tblPhotos On I.PhotoID = tblPhotos.PhotoID And tblPhotos.ValidityTo Is null  "
        sSQL += " LEFT JOIN tblGender GE On GE.Code = I.Gender"
        sSQL += " LEFT JOIN @dtMarital dtMarital ON I.Marital = dtMarital.Code"
        'If (Request.Cookies("CultureInfo").Value = "en", "Education", "AltLanguage") Then

        'End If

        Dim strWhere As String = ""
        strWhere += " WHERE  ((L.RegionId In (Select Region FROM UD)) Or (L.DistrictId In (Select DistrictId FROM UD))) "
        If All = False Then
            strWhere += " And I.ValidityTo Is NULL"
        End If
        If Not eInsuree.tblFamilies1.RegionId = 0 Then
            strWhere += " And L.RegionId= @RegionId"
        End If
        If Not eInsuree.LastName = Nothing Then
            eInsuree.LastName += "%"
            strWhere += " And lastname Like @Lastname"
        End If
        If Not eInsuree.OtherNames = Nothing Then
            eInsuree.OtherNames += "%"
            strWhere += " And othernames Like @OtherNames"
        End If
        If Not eInsuree.CHFID = Nothing Then
            eInsuree.CHFID += "%"
            strWhere += " And I.CHFID Like @CHFID"
        End If
        If Not eInsuree.Gender = Nothing Then
            strWhere += " And I.Gender = @Gender"
        End If
        If Not eInsuree.Marital = Nothing Then
            strWhere += " And I.Marital = @Marital"
        End If
        If Not eInsuree.Phone = Nothing Then
            eInsuree.Phone += "%"
            strWhere += " And isnull(Phone,'') like @Phone"
        End If
        If Not eInsuree.tblFamilies1.DistrictId Is Nothing Then
            strWhere += " AND L.DistrictID = @DistrictID"
        End If
        If Not eInsuree.tblFamilies1.WardId Is Nothing Then
            strWhere += " AND L.WardID = @WardID"
        End If
        If Not eInsuree.tblFamilies1.LocationId = Nothing Then
            strWhere += " AND L.VillageID = @VillageID"
        End If
        If Not eInsuree.DOBFrom = Nothing Then
            strWhere += " AND DOB >= @DOBFrom"
        End If
        If Not eInsuree.DOBTo = Nothing Then
            strWhere += " AND DOB <= @DOBTo"
        End If
        If PhotoAssigned = 2 Then
            strWhere += " AND PhotoFileName <> ''"
        End If
        If PhotoAssigned = 3 Then
            strWhere += " AND PhotoFileName = ''"
        End If
        If eInsuree.isOffline IsNot Nothing Then
            If eInsuree.isOffline Then
                strWhere += " AND I.isOffline = 1"
            End If
        End If
        If eInsuree.Email.ToString.Trim.Length > 0 Then
            strWhere += " AND I.Email like @Email"
        End If
        'If Not strWhere = String.Empty Then
        '    strWhere = " WHERE" & strWhere.Remove(1, 4)
        'End If

        sSQL += strWhere

        sSQL += " GROUP BY  I.isOffline,I.FamilyID,I.InsureeID, RegionName,DistrictName,WardName,VillageName,LastName,Othernames, I.CHFID,I.Gender,"

        sSQL += "" & IIf(Language = "en", "GE.Gender", "ISNULL(GE.AltLanguage,GE.Gender)")
        sSQL += ",dtMarital.Name,phone,DOB,  I.validityfrom,I.validityTo"
        strWhere += " order by LastName,I.ValidityFrom desc ,I.ValidityTo desc"
        Dim data As New ExactSQL
        'changed by amani added timeout:=0 12/12/2017
        data.setSQLCommand(sSQL, CommandType.Text, timeout:=0)
        data.params("@UserId", SqlDbType.Int, eInsuree.AuditUserID)
        data.params("@DOBFrom", SqlDbType.Date, eInsuree.DOBFrom)
        data.params("@DOBTo", SqlDbType.Date, eInsuree.DOBTo)
        data.params("@LastName", SqlDbType.NVarChar, 100, eInsuree.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eInsuree.OtherNames)
        data.params("@CHFID", SqlDbType.NVarChar, 12, eInsuree.CHFID)
        data.params("@Gender", SqlDbType.Char, 1, eInsuree.Gender)
        data.params("@Marital", SqlDbType.Char, 1, eInsuree.Marital)
        data.params("@Phone", SqlDbType.NVarChar, 50, eInsuree.Phone)
        data.params("@RegionId", SqlDbType.Int, eInsuree.tblFamilies1.RegionId)
        data.params("@DistrictID", SqlDbType.Int, eInsuree.tblFamilies1.DistrictId)
        data.params("@VillageID", SqlDbType.Int, eInsuree.tblFamilies1.LocationId)
        data.params("@WardID", SqlDbType.Int, eInsuree.tblFamilies1.WardId)
        data.params("@Email", SqlDbType.NVarChar, 100, "%" & eInsuree.Email & "%")
        data.params("@dtMarital", dtMarital, "xAttributeV")
        Return data.Filldata



    End Function


    'Corrected
    Public Sub LoadInsuree(ByRef eInsuree As IMIS_EN.tblInsuree)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "SELECT I.FamilyId, I.ValidityTo, I.CHFID, I.LastName, I.OtherNames, I.DOB, I.Gender, I.Marital, I.isHead, I.Passport, I.PhotoId,"
        sSQL += " I.PhotoDate, I.CardIssued, PH.PhotoFolder, PH.PhotoFileName, ISNULL(I.Relationship, 0)Relationship, ISNULL(I.Profession, 0) Profession,"
        sSQL += " ISNULL(I.Education, 0) Education, I.Email,I.Phone,  I.isOffline, I.TypeOfId, I.HFID, HF.HFLevel, HFR.RegionId FSPRegionId, HFD.DistrictId,"
        sSQL += " I.CurrentAddress, R.RegionId CurrentRegion, D.DistrictId CurrentDistrict, W.WardId CurrentWard, I.CurrentVillage"
        sSQL += " FROM tblInsuree I"
        sSQL += " LEFT OUTER JOIN tblPhotos PH ON PH.InsureeId = I.InsureeID"
        sSQL += " LEFT OUTER JOIN tblHF HF ON HF.HFID = I.HFID"
        sSQL += " LEFT OUTER JOIN tblDistricts HFD ON HFD.DistrictId = HF.LocationId"
        sSQL += " LEFT OUTER JOIN tblRegions HFR ON HFR.RegionId = HFD.Region"
        sSQL += " LEFT OUTER JOIN tblVillages V ON V.VillageId = I.CurrentVillage"
        sSQL += " LEFT OUTER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " LEFT OUTER JOIN tblDistricts D ON D.DistrictId = W.DistrictId"
        sSQL += " LEFT OUTER JOIN tblRegions R ON R.RegionId = D.Region"
        sSQL += " WHERE I.InsureeId = @InsureeId"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@InsureeID", SqlDbType.Int, eInsuree.InsureeID)
        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            eInsuree.CHFID = dr("CHFID")
            eInsuree.CardIssued = dr("CardIssued")
            eInsuree.DOB = dr("DOB")
            eInsuree.Gender = dr("Gender").ToString
            eInsuree.LastName = dr("LastName")
            eInsuree.Marital = If(dr("Marital") Is DBNull.Value, Nothing, dr("Marital"))
            eInsuree.OtherNames = dr("OtherNames")
            eInsuree.passport = if(dr.IsNull("passport"), "", dr("passport"))
            eInsuree.Phone = if(dr.IsNull("Phone"), "", dr("Phone"))
            eInsuree.PhotoDate = if(dr.IsNull("PhotoDate"), Nothing, dr("PhotoDate"))
            eInsuree.IsHead = dr("IsHead")

            Dim efamily As New IMIS_EN.tblFamilies
            efamily.FamilyID = dr("FamilyID")
            eInsuree.tblFamilies1 = efamily
            Dim ePhotos As New IMIS_EN.tblPhotos
            ePhotos.PhotoID = if(dr.IsNull("PhotoID"), Nothing, dr("PhotoID"))
            ePhotos.PhotoFolder = dr("PhotoFolder").ToString
            ePhotos.PhotoFileName = dr("PhotoFileName").ToString
            eInsuree.tblPhotos = ePhotos

            If Not dr("ValidityTo") Is DBNull.Value Then
                eInsuree.ValidityTo = dr("ValidityTo")
            End If
            If dr("isOffline") IsNot DBNull.Value Then
                eInsuree.isOffline = dr("isOffline")
            End If

            eInsuree.TypeOfId = dr("TypeOfId").ToString

            Dim eHF As New IMIS_EN.tblHF
            eHF.HfID = if(dr.IsNull("HFID"), 0, dr("HFID"))
            eInsuree.tblHF = eHF

            eInsuree.FSPRegion = If(dr.IsNull("FSPRegionId"), 0, dr("FSPRegionId"))
            eInsuree.FSPDistrict = If(dr.IsNull("DistrictId"), 0, dr("DistrictId"))
            eInsuree.FSPCategory = If(dr.IsNull("HFLevel"), 0, dr("HFLevel"))

            eInsuree.Relationship = dr("Relationship")
            eInsuree.Profession = dr("Profession")
            eInsuree.Education = dr("Education")
            eInsuree.Email = dr("Email").ToString

            eInsuree.CurrentAddress = dr("CurrentAddress").ToString
            eInsuree.CurrentRegion = if(dr.IsNull("CurrentRegion"), 0, dr("CurrentRegion"))
            eInsuree.CurDistrict = if(dr.IsNull("CurrentDistrict"), 0, dr("CurrentDistrict"))
            eInsuree.CurWard = if(dr.IsNull("CurrentWard"), 0, dr("CurrentWard"))
            eInsuree.CurrentVillage = if(dr.IsNull("CurrentVillage"), 0, dr("CurrentVillage"))

        End If


    End Sub
    Public Sub InsertInsuree(ByRef eInsuree As IMIS_EN.tblInsuree)
        Dim data As New ExactSQL         '"DECLARE @InsureeID INT;" & _
        Dim sSQL As String = ""
        sSQL = " INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[Relationship],[Profession],[Education],[Email],[TypeOfId],[HFID],[CurrentAddress],[CurrentVillage],[GeoLocation])" & _
               " VALUES (@FamilyID,@CHFID,@LastName,@OtherNames,@DOB,@Gender,@Marital,@IsHead,@passport,@Phone,@PhotoID, @PhotoDate,@CardIssued,@isOffline,@AuditUserID,@Relationship,@Profession,@Education,@Email, @TypeOfId, @HFID, @CurrentAddress, @CurrentVillage, @GeoLocation);" & _
               " SET @InsureeID = (SELECT SCOPE_IDENTITY());" & _
               " INSERT INTO tblPhotos(InsureeID,CHFID,PhotoFolder,PhotoFileName,OfficerID,PhotoDate,ValidityFrom,AuditUserID)" & _
               " SELECT InsureeID,CHFID,'','',0,GETDATE(),ValidityFrom,AuditUserID from tblInsuree WHERE InsureeID = @InsureeID; " & _
               " UPDATE tblInsuree SET PhotoID = (SELECT IDENT_CURRENT('tblPhotos')) WHERE InsureeID = @InsureeID;"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@FamilyId", SqlDbType.Int, eInsuree.tblFamilies1.FamilyID)
        data.params("@CHFID", SqlDbType.NVarChar, 12, eInsuree.CHFID)
        data.params("@LastName", SqlDbType.NVarChar, 100, eInsuree.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eInsuree.OtherNames)
        data.params("@DOB", SqlDbType.SmallDateTime, eInsuree.DOB)
        data.params("@Marital", SqlDbType.NVarChar, 1, eInsuree.Marital)
        data.params("@Gender", SqlDbType.NVarChar, 1, eInsuree.Gender)
        data.params("@IsHead", SqlDbType.Bit, eInsuree.IsHead)
        data.params("@Passport", SqlDbType.NVarChar, 50, eInsuree.passport)
        data.params("@Phone", SqlDbType.NVarChar, 50, eInsuree.Phone)
        data.params("@PhotoID", SqlDbType.Int, if(eInsuree.tblPhotos.PhotoID = 0, DBNull.Value, eInsuree.tblPhotos.PhotoID))
        data.params("@PhotoDate", SqlDbType.SmallDateTime, eInsuree.PhotoDate)
        data.params("@CardIssued", SqlDbType.Bit, eInsuree.CardIssued)
        data.params("@isOffline", SqlDbType.Bit, eInsuree.isOffline)
        data.params("@AuditUserID", SqlDbType.Int, eInsuree.AuditUserID)
        data.params("@InsureeID", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@Relationship", SqlDbType.SmallInt, eInsuree.Relationship)
        data.params("@Profession", SqlDbType.SmallInt, eInsuree.Profession)
        data.params("@Education", SqlDbType.SmallInt, eInsuree.Education)
        data.params("@Email", SqlDbType.NVarChar, 100, eInsuree.Email)
        data.params("@TypeOfId", SqlDbType.NVarChar, 1, eInsuree.TypeOfId)
        data.params("@HFID", SqlDbType.Int, If(eInsuree.tblHF.HfID = 0, DBNull.Value, eInsuree.tblHF.HfID))
        data.params("@CurrentAddress", SqlDbType.NVarChar, 50, eInsuree.CurrentAddress)
        data.params("@CurrentVillage", SqlDbType.Int, eInsuree.CurrentVillage, ParameterDirection.Input)
        data.params("@GeoLocation", SqlDbType.NVarChar, 50, eInsuree.GeoLocation)



        data.ExecuteCommand()
        eInsuree.InsureeID = data.sqlParameters("@InsureeID")

    End Sub
    Public Sub ModifyInsuree(ByVal eInsuree As IMIS_EN.tblInsuree)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,[ValidityTo],legacyId,[Relationship],[Profession],[Education],[Email],[TypeOfId],[HFID], [CurrentAddress], [GeoLocation], [CurrentVillage]) " & _
                           " select	[FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,getdate(),@insureeId,[Relationship],[Profession],[Education],[Email] ,[TypeOfId],[HFID], [CurrentAddress], [GeoLocation], [CurrentVillage]" & _
                           " from tblInsuree where InsureeID = @InsureeID;" & _
                           " UPDATE [tblInsuree] SET [CHFID] = @CHFID,[LastName] = @LastName,[OtherNames] = @OtherNames,[DOB] = @DOB,[Gender] = @Gender ,[Marital] = @Marital,[passport] = @passport,[Phone] = @Phone,[PhotoDate] = @PhotoDate,[CardIssued] = @CardIssued,isOffline=@isOffline,[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID ,[Relationship] = @Relationship, [Profession] = @Profession, [Education] = @Education,[Email] = @Email ,TypeOfId = @TypeOfId,HFID = @HFID, CurrentAddress = @CurrentAddress, CurrentVillage = @CurrentVillage, GeoLocation = @GeoLocation" & _
                           " WHERE InsureeId = @InsureeId", CommandType.Text)
        data.params("@InsureeID", SqlDbType.Int, eInsuree.InsureeID)
        data.params("@CHFID", SqlDbType.NVarChar, 12, eInsuree.CHFID)
        data.params("@LastName", SqlDbType.NVarChar, 100, eInsuree.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eInsuree.OtherNames)
        data.params("@DOB", SqlDbType.SmallDateTime, eInsuree.DOB)
        data.params("@Marital", SqlDbType.NVarChar, 1, eInsuree.Marital)
        data.params("@Gender", SqlDbType.NVarChar, 1, eInsuree.Gender)
        'data.params("@IsHead", SqlDbType.Bit, eInsuree.IsHead)
        data.params("@Passport", SqlDbType.NVarChar, 50, eInsuree.passport)
        data.params("@Phone", SqlDbType.NVarChar, 50, eInsuree.Phone)
        'data.params("@PhotoID", SqlDbType.Int, eInsuree.PhotoID)
        data.params("@PhotoDate", SqlDbType.SmallDateTime, eInsuree.PhotoDate)
        data.params("@CardIssued", SqlDbType.Bit, eInsuree.CardIssued)
        data.params("@isOffline", SqlDbType.Bit, eInsuree.isOffline)
        data.params("@AuditUserID", SqlDbType.Int, eInsuree.AuditUserID)
        'data.params("@LegacyID", SqlDbType.Int, 1, ParameterDirection.Output)
        data.params("@Relationship", SqlDbType.SmallInt, eInsuree.Relationship)
        data.params("@Profession", SqlDbType.SmallInt, eInsuree.Profession)
        data.params("@Education", SqlDbType.SmallInt, eInsuree.Education)
        data.params("@Email", SqlDbType.NVarChar, 100, eInsuree.Email)
        data.params("@TypeOfId", SqlDbType.NVarChar, 1, eInsuree.TypeOfId)
        data.params("@HFID", SqlDbType.Int, If(eInsuree.tblHF.HfID = 0, DBNull.Value, eInsuree.tblHF.HfID))

        data.params("@CurrentAddress", SqlDbType.NVarChar, 50, eInsuree.CurrentAddress)
        data.params("@Currentvillage", SqlDbType.Int, eInsuree.CurrentVillage, ParameterDirection.Input)
        data.params("@GeoLocation", SqlDbType.NVarChar, 50, eInsuree.GeoLocation)

        data.ExecuteCommand()

    End Sub
    Public Function MoveInsuree(ByVal eInsuree As IMIS_EN.tblInsuree) As Boolean
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,[ValidityTo],legacyId, TypeOfId, HFID, [CurrentAddress], [GeoLocation], [CurrentVillage]) " & _
                           " select					[FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,getdate(),@insureeId, TypeOfId, HFID, [CurrentAddress], [GeoLocation], [CurrentVillage] " & _
                           " from tblInsuree where InsureeID = @InsureeID;" & _
                           " UPDATE [tblInsuree] SET [FamilyID]=@FamilyID,[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID " & _
                           " WHERE InsureeId = @InsureeId", CommandType.Text)
        data.params("@FamilyID", SqlDbType.Int, eInsuree.tblFamilies1.FamilyID)
        data.params("@InsureeId", SqlDbType.Int, eInsuree.InsureeID)
        data.params("@AuditUserID", SqlDbType.Int, eInsuree.AuditUserID)

        Return data.ExecuteCommand

    End Function
    Public Function FindInsuree(ByVal eInsuree As IMIS_EN.tblInsuree)
        Dim data As New ExactSQL
        Dim sSQL As String = "SELECT FamilyID,InsureeID,CHFId,LastName,OtherNames,validityFrom,ValidityTo FROM tblInsuree"

        data.setSQLCommand(sSQL, CommandType.Text)

        Return data.Filldata

    End Function
    Public Sub UpdateImage(ByRef ePhotos As IMIS_EN.tblPhotos)
        Try

            Dim data As New ExactSQL

            Dim sSQL As String = ""

            If ePhotos.InsureeID > 0 Then
                sSQL = "DECLARE @PhotoId INT =  (SELECT PhotoID from tblInsuree where CHFID = @CHFID AND LegacyID is NULL and ValidityTo is NULL)" & _
                        " INSERT INTO tblPhotos(InsureeID,CHFID,PhotoFolder,PhotoFileName,PhotoDate,OfficerID,ValidityFrom,ValidityTo,AuditUserID)" & _
                        " SELECT InsureeID,CHFID,PhotoFolder,PhotoFileName,PhotoDate,OfficerID,ValidityFrom,GETDATE(),AuditUserID FROM tblPhotos WHERE PhotoID = @PhotoID;" & _
                        " UPDATE tblPhotos SET PhotoFolder = @PhotoFolder,PhotoFileName = @PhotoFileName, OfficerID = @OfficerID, ValidityFrom = GETDATE(), AuditUserID = @AuditUserID WHERE PhotoID = @PhotoID"
            Else
                sSQL = "DECLARE @PhotoId INT =  (SELECT PhotoID from tblInsuree where CHFID = @CHFID AND LegacyID is NULL and ValidityTo is NULL)" & _
                        " UPDATE tblPhotos SET PhotoFolder = @PhotoFolder,PhotoFileName = @PhotoFileName, OfficerID = @OfficerID, ValidityFrom = GETDATE(), AuditUserID = @AuditUserID WHERE PhotoID = @PhotoID"
            End If

            data.setSQLCommand(sSQL, CommandType.Text)

            data.params("@InsureeID", SqlDbType.Int, ePhotos.InsureeID)
            data.params("@CHFID", SqlDbType.NVarChar, 12, ePhotos.CHFID)
            data.params("@PhotoFolder", SqlDbType.NVarChar, 255, ePhotos.PhotoFolder)
            data.params("@PhotoFileName", SqlDbType.NVarChar, 255, ePhotos.PhotoFileName)
            data.params("@OfficerID", SqlDbType.Int, ePhotos.OfficerID)
            data.params("@ValidityFrom", SqlDbType.SmallDateTime, Date.Now)
            data.params("@AuditUserID", SqlDbType.Int, ePhotos.AuditUserID)

            data.ExecuteCommand()

        Catch ex As Exception

        End Try
    End Sub
    Public Function InsureeExists(ByVal eInsuree As IMIS_EN.tblInsuree) As DataTable
        Dim data As New ExactSQL
        Dim strSQL As String = "select * from tblInsuree where CHFID = @CHFId AND ValidityTo IS NULL"

        If Not eInsuree.InsureeID = 0 Then
            strSQL += " AND tblInsuree.InsureeId <> @InsureeId"
        End If

        data.setSQLCommand(strSQL, CommandType.Text)
        data.params("@CHFId", SqlDbType.NVarChar, 12, eInsuree.CHFID)
        If Not eInsuree.InsureeID = 0 Then
            data.params("@InsureeId", SqlDbType.Int, eInsuree.InsureeID)
        End If
        Return data.Filldata()
    End Function

    Public Function FindInsureeByCHFID(ByVal CHFID As String, Optional Language As String = "en") As DataTable
        Dim data As New ExactSQL
        Dim UpdatedFolder As String
        UpdatedFolder = System.Web.Configuration.WebConfigurationManager.AppSettings("UpdatedFolder").ToString()
        Dim sSQL As String = " SELECT HF.HFCode+' ' +HF.HFName AS FirstServicePoint, CASE HF.HFLevel WHEN 'D' THEN 'Dispensary' WHEN 'C' THEN 'Health Centre' WHEN 'H' THEN 'Hospital' END HFLevel, R.LocationName RegionOfFSP,D.LocationName DistrictOfFSP, I.CHFID,I.LastName,I.OtherNames,CONVERT(VARCHAR,I.DOB,103)DOB, (YEAR(GETDATE()) - YEAR(I.DOB)) AS Age,"
        sSQL += "" & IIf(Language = "en", "GE.Gender", "ISNULL(GE.AltLanguage,GE.Gender) Gender")
        sSQL += ", P.PhotoFileName AS PhotoPath FROM tblInsuree I"
        sSQL += " INNER JOIN tblFamilies F On I.FamilyID = F.FamilyID"
        sSQL += " LEFT OUTER JOIN tblPhotos P On I.PhotoID = P.PhotoID"
        sSQL += " LEFT OUTER JOIN tblHF HF On HF.HfID = I.HFID"
        sSQL += " LEFT OUTER JOIN tblLocations D On D.LocationId = HF.LocationId"
        sSQL += " LEFT OUTER JOIN tblLocations R On R.LocationId = D.ParentLocationId"
        sSQL += " LEFT OUTER JOIN tblGender GE On GE.Code = I.Gender"
        sSQL += " WHERE I.CHFID = @CHFID And I.ValidityTo Is NULL"
        sSQL += " And I.ValidityTo Is NULL"
        sSQL += " And D.ValidityTo Is NULL"
        sSQL += " And R.ValidityTo Is NULL"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@CHFID", SqlDbType.NVarChar, 12, CHFID)
        data.params("@UpdatedFolder", SqlDbType.NVarChar, 100, UpdatedFolder)
        Return data.Filldata

    End Function

    'Corrected
    Public Function ChangeHead(ByVal eInsureeOLD As IMIS_EN.tblInsuree, ByVal eInsureeNew As IMIS_EN.tblInsuree) As Boolean

        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,[ValidityTo],legacyId,TypeOfId, HFID, CurrentAddress, GeoLocation, CurrentVillage ) " & _
                           " Select					[FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,getdate(),@insureeIdOLD ,TypeOfId, HFID, CurrentAddress, GeoLocation, CurrentVillage " & _
                           " from tblInsuree where InsureeID = @InsureeIDOLD;" & _
                           " UPDATE [tblInsuree] Set [isHead] = 0,[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID " & _
                           " WHERE InsureeId = @InsureeIDOLD;" & _
                            "INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,[ValidityTo],legacyId,TypeOfId, HFID ) " & _
                           " Select					[FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,getdate(),@insureeIdnew ,TypeOfId, HFID " & _
                           " from tblInsuree where InsureeID = @InsureeIDNEW;" & _
                           " UPDATE [tblInsuree] Set [isHead] = 1,[FamilyID] = @FamilyID,[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID " & _
                           " WHERE InsureeId = @InsureeIDNEW;" & _
                           "insert into tblFamilies ([insureeid],LocationId,[Poverty],isOffline,[ValidityFrom],[ValidityTo]," & _
                            "[LegacyID],[AuditUserID])Select [insureeid],LocationId,[Poverty],isOffline,[ValidityFrom],getdate()," & _
                            " @FamilyID, [AuditUserID] from tblFamilies where FamilyID = @FamilyID;" & _
                            " Update tblFamilies Set InsureeId = @InsureeidNew,validityfrom = getdate(),AudituserId =@AuditUserId where FamilyId = @FamilyId;", CommandType.Text)
        data.params("@InsureeIDOLD", SqlDbType.Int, eInsureeOLD.InsureeID)
        data.params("@InsureeIDNew", SqlDbType.Int, eInsureeNew.InsureeID)
        data.params("@FamilyID", SqlDbType.Int, eInsureeOLD.tblFamilies1.FamilyID)
        data.params("@AuditUserID", SqlDbType.Int, eInsureeNew.AuditUserID)

        Return data.ExecuteCommand

    End Function
    Public Function verifyCHFIDandReturnName(ByVal CHFID As String, ByRef insureeid As Integer) As String
        Dim data As New ExactSQL
        data.setSQLCommand("Select Insureeid,  OtherNames + ' ' + Lastname Name from tblInsuree where CHFID = @CHFId and validityto is null", CommandType.Text)
        data.params("@CHFId", SqlDbType.NVarChar, 12, CHFID)
        '  data.params("Name", SqlDbType.NVarChar, 150, "", ParameterDirection.Output)
        Dim dr As DataRow = data.Filldata()(0)
        If dr Is Nothing Then
            Return ""
        Else
            insureeid = dr("Insureeid")
            Return dr("Name")
        End If

    End Function
    Public Function CheckCanBeDeleted(ByVal InsureeID As Integer) As DataTable
        Dim str As String = "SELECT * FROM tblInsuree WHERE InsureeID=@InsureeID AND IsHead=@IsHead AND ValidityTo IS NULL AND LegacyID IS NULL"

        data.setSQLCommand(str, CommandType.Text)
        data.params("@InsureeID", SqlDbType.Int, InsureeID)
        data.params("@IsHead", SqlDbType.Bit, True)
        Return data.Filldata()
    End Function
    Public Function DeleteInsuree(ByVal eInsuree As IMIS_EN.tblInsuree) As Boolean
        Dim str As String = "INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,[ValidityTo],legacyId,TypeOfId, HFID, CurrentAddress, CurrentVillage,GeoLocation ) " & _
                           " select	[FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[ValidityFrom] ,getdate(),@insureeId ,TypeOfId, HFID, CurrentAddress, CurrentVillage, GeoLocation " & _
                           " from tblInsuree where InsureeID = @InsureeID AND ValidityTo IS NULL;" & _
                           " UPDATE [tblInsuree] SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate(),[AuditUserID] = @AuditUserID " & _
                           " WHERE InsureeId = @InsureeID AND ValidityTo IS NULL"

        data.setSQLCommand(str, CommandType.Text)
        data.params("@InsureeID", SqlDbType.Int, eInsuree.InsureeID)
        data.params("@AuditUserID", SqlDbType.Int, eInsuree.AuditUserID)
        data.ExecuteCommand()
        Return True
    End Function
    Public Function GetInsureeOfflineValue(ByVal InsureeID As Integer) As Boolean
        Dim Query As String = "SELECT isnull(isOffline,0) isOffline FROM tblInsuree where InsureeId=@InsureeId"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@InsureeId", SqlDbType.Int, InsureeID)
        Return CBool(data.Filldata().Rows(0)("isOffline"))
    End Function
    Public Function GetInsureeProductDetails(ByVal oDict As Dictionary(Of String, Object), ByVal CHFID As String, ByVal ItemCode As String, ByVal ServiceCode As String) As DataTable
        Dim Query As String = "uspServiceItemEnquiry"
        data.setSQLCommand(Query, CommandType.StoredProcedure)
        data.params("@CHFID", SqlDbType.NVarChar, 12, CHFID)
        data.params("@ItemCode", SqlDbType.NVarChar, 6, ItemCode)
        data.params("@ServiceCode", SqlDbType.NVarChar, 6, ServiceCode)
        data.params("@MinDateService", SqlDbType.Date, Nothing, ParameterDirection.Output)
        data.params("@ServiceLeft", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@MinDateItem", SqlDbType.Date, Nothing, ParameterDirection.Output)
        data.params("@ItemLeft", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@IsItemOk", SqlDbType.Bit, 0, ParameterDirection.Output)
        data.params("@IsServiceOk", SqlDbType.Bit, 0, ParameterDirection.Output)
        Dim dt As DataTable = data.Filldata()
        If oDict IsNot Nothing Then
            oDict.Add("ItemLeft", data.sqlParameters("@ItemLeft"))
            oDict.Add("MinDateItem", data.sqlParameters("@MinDateItem"))
            oDict.Add("ServiceLeft", data.sqlParameters("@ServiceLeft"))
            oDict.Add("MinDateService", data.sqlParameters("@MinDateService"))
            oDict.Add("IsItemOk", data.sqlParameters("@IsItemOk"))
            oDict.Add("IsServiceOk", data.sqlParameters("@IsServiceOk"))
        End If
        Return dt
    End Function
    Public Function GetMaxMemberCount(ByVal FamilyId As Integer) As DataTable
        Dim sSQL As String = ""
        sSQL = "SELECT Prod.MemberCount, Prod.Threshold, PL.EnrollDate,COUNT(I.InsureeId) TotalInsurees FROM tblPolicy PL INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID INNER JOIN tblInsuree I ON I.FamilyID = PL.FamilyID WHERE(PL.ValidityTo Is NULL And Prod.ValidityTo Is NULL And I.ValidityTo Is NULL) AND PL.FamilyID = @FamilyId GROUP BY Prod.MemberCount,Prod.Threshold,PL.EnrollDate HAVING MemberCount <= COUNT(I.InsureeId) OR Threshold <= COUNT(I.InsureeId)"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@FamilyId", SqlDbType.Int, FamilyId)

        Return data.Filldata

    End Function

    Public Function InsureePhotoExists(photoFileName As String) As Boolean
        Dim sSQL As String = ""
        sSQL = "SELECT 1 FROM tblPhotos WHERE PhotoFileName=@photoFileName AND ValidityTo IS NULL"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@photoFileName", SqlDbType.NVarChar, 100, photoFileName)
        Return data.Filldata.Rows.Count > 0
    End Function
End Class
