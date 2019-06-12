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

Public Class FamilyDAL
    Private data As New ExactSQL

    'Corrected
    Public Sub LoadFamily(ByRef eFamily As IMIS_EN.tblFamilies)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " SELECT I.InsureeId, I.CHFID, I.LastName, I.OtherNames, I.DOB, I.Phone, I.isOffline InsureeIsOffline, I.Education, I.Profession,"
        sSQL += " F.Poverty, F.ConfirmationType, R.RegionId, R.RegionName, D.DistrictName, D.DistrictId, V.VillageId, V.VillageName, W.wardId, W.WardName, I.TypeOfId,"
        sSQL += " I.CurrentAddress, I.CurrentVillage , I.HFID, HF.LocationId FSPDistrictId, HF.HFCareType, F.FamilyType,"
        sSQL += " F.FamilyAddress, F.Ethnicity,F.ConfirmationNo, F.ValidityTo, F.isOffline"
        sSQL += " from tblFamilies F"
        sSQL += " INNER JOIN tblVillages V ON V.VillageId = F.LocationId"
        sSQL += " INNER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId"
        sSQL += " INNER JOIN tblRegions R ON R.RegionId = D.RegionId"
        sSQL += " INNER JOIN tblInsuree i ON f.FamilyID = i.FamilyID"
        sSQL += " LEFT OUTER JOIN tblHF HF ON HF.HFID = I.HFID"
        sSQL += " WHERE F.FamilyId = @FamilyId"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@FamilyId", SqlDbType.Int, eFamily.FamilyID)
        Dim dr As DataRow = data.Filldata()(0)
        Dim eInsurees As New IMIS_EN.tblInsuree
        
        If Not dr Is Nothing Then
            eInsurees.InsureeID = dr("InsureeID")
            eInsurees.CHFID = dr("CHFID")
            eInsurees.LastName = dr("LastName")
            eInsurees.OtherNames = dr("Othernames")
            eInsurees.DOB = dr("DOB")
            If Not dr("Phone") Is DBNull.Value Then
                eInsurees.Phone = dr("Phone")
            End If
            If dr("InsureeIsOffline") IsNot DBNull.Value Then
                eInsurees.isOffline = dr("InsureeIsOffline")
            End If
            If dr("Education") IsNot DBNull.Value Then eInsurees.Education = dr("Education")
            If dr("Profession") IsNot DBNull.Value Then eInsurees.Profession = dr("Profession")
            eFamily.Poverty = dr("Poverty")
            eFamily.ConfirmationType = dr("ConfirmationType").ToString
            eFamily.RegionId = dr("RegionId")
            eFamily.RegionName = dr("RegionName")
            eFamily.DistrictName = dr("DistrictName")
            eFamily.DistrictID = dr("DistrictID")
            eFamily.LocationId = dr("VillageID")
            eFamily.VillageName = dr("VillageName")
            eFamily.WardID = dr("WardID")
            eFamily.WardName = dr("WardName")

            'Addition for Nepal >> Start
            eInsurees.TypeOfId = dr("TypeOfId").ToString
            eInsurees.CurrentAddress = dr("CurrentAddress").ToString
            'eInsurees.CurDistrict = dr("ICurDistrict").ToString
            'eInsurees.CurWard = dr("ICurVDC").ToString
            eInsurees.CurrentVillage = dr("CurrentVillage").ToString


            Dim eHF As New IMIS_EN.tblHF
            eHF.HfID = if(dr.IsNull("HFID"), 0, dr("HFID"))
            eInsurees.tblHF = eHF

            eInsurees.FSPDistrict = if(dr.IsNull("FSPDistrictId"), 0, dr("FSPDistrictId"))
            eInsurees.FSPCategory = dr("HFCareType").ToString

            'Addition for Nepal >> End

            eFamily.tblInsuree = eInsurees
            eFamily.FamilyType = dr("FamilyType")
            eFamily.FamilyAddress = dr("FamilyAddress").ToString
            eFamily.Ethnicity = dr("Ethinicity").ToString
            eFamily.ConfirmationNo = dr("ConfirmationNo").ToString

            If Not dr("ValidityTo") Is DBNull.Value Then
                eFamily.ValidityTo = dr("ValidityTo")
            End If

            If dr("isOffline") IsNot DBNull.Value Then
                eFamily.isOffline = dr("isOffline")
            End If

            'If Not dr("CurDistrict") IsNot DBNull.Value Then eFamily.CurDistrict = dr("CurDistrict")
            'If Not dr("CurVDC") IsNot DBNull.Value Then eFamily.CurVDC = dr("CurVDC")
            'If Not dr("CurWard") IsNot DBNull.Value Then eFamily.CurWard = dr("CurWard")


        End If

    End Sub
    'Corrected
    Public Sub InsertInsuredFamily(ByRef eFamily As IMIS_EN.tblFamilies)
        Dim data As New ExactSQL
        data.setSQLCommand("Insert into tblFamilies (LocationId,Poverty,ConfirmationType,isOffline,AuditUserID,FamilyType,FamilyAddress,Ethnicity,ConfirmationNo) VALUES (@LocationId,@Poverty,@ConfirmationType, @isOffline,@AuditUserID,@FamilyType,@FamilyAddress,@Ethnicity,@ConfirmationNo ); select @FamilyId = scope_identity();" _
                           & "INSERT INTO tblInsuree ([FamilyID],[CHFID],[LastName],[OtherNames],[DOB],[Gender],[Marital],[IsHead],[passport],[Phone],[PhotoID],[PhotoDate],[CardIssued],isOffline,[AuditUserID],[Email],Education,Profession, TypeOfId, HFID,CurrentAddress,CurrentVillage, GeoLocation)" _
     & " VALUES (@FamilyID,@CHFID,@LastName,@OtherNames,@DOB,@Gender,@Marital,@IsHead,@passport,@Phone,@PhotoID, @PhotoDate,@CardIssued,@isOffline,@AuditUserID,@Email,@Education,@Profession, @TypeOfId, @HFID,@CurrentAddress, @CurrentVillage, @GeoLocation);select @InsureeID = scope_identity()" _
     & "Update tblFamilies set InsureeId = @Insureeid where FamilyId = @FamilyId;" & _
     " INSERT INTO tblPhotos (InsureeID,CHFID,PhotoDate,PhotoFolder,PhotoFileName,OfficerID,ValidityFrom,AuditUserID)" & _
     " VALUES(@InsureeID,@CHFID,@PhotoDate,@PhotoFolder,@PhotoFileName,@OfficerID,@ValidityFrom,@AuditUserID);" & _
     " update tblInsuree SET PhotoID = (SELECT PhotoID from TblPhotos where InsureeID = @InsureeID)" & _
     " WHERE InsureeID = @InsureeID", CommandType.Text)
        data.params("@FamilyId", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@InsureeId", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@LocationId", SqlDbType.Int, If(eFamily.LocationId = 0, Nothing, eFamily.LocationId))
        data.params("@Poverty", SqlDbType.Bit, eFamily.Poverty, ParameterDirection.Input)
        data.params("@ConfirmationType", SqlDbType.NVarChar, 3, eFamily.ConfirmationType)
        data.params("@Ethnicity", SqlDbType.NVarChar, 1, eFamily.Ethnicity)
        data.params("@ConfirmationNo", SqlDbType.NVarChar, 12, eFamily.ConfirmationNo)


        data.params("@CHFID", SqlDbType.NVarChar, 12, eFamily.tblInsuree.CHFID)
        data.params("@LastName", SqlDbType.NVarChar, 100, eFamily.tblInsuree.LastName)
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eFamily.tblInsuree.OtherNames)
        data.params("@DOB", SqlDbType.SmallDateTime, eFamily.tblInsuree.DOB)
        data.params("@Marital", SqlDbType.NVarChar, 1, eFamily.tblInsuree.Marital)
        data.params("@Gender", SqlDbType.NVarChar, 1, eFamily.tblInsuree.Gender)
        data.params("@IsHead", SqlDbType.Bit, 1)
        data.params("@Passport", SqlDbType.NVarChar, 50, eFamily.tblInsuree.passport)
        data.params("@Phone", SqlDbType.NVarChar, 50, eFamily.tblInsuree.Phone)
        data.params("@PhotoID", SqlDbType.Int, if(eFamily.tblInsuree.tblPhotos.PhotoID = 0, DBNull.Value, eFamily.tblInsuree.tblPhotos.PhotoID))
        data.params("@PhotoDate", SqlDbType.SmallDateTime, eFamily.tblInsuree.PhotoDate)
        data.params("@CardIssued", SqlDbType.Bit, eFamily.tblInsuree.CardIssued)
        data.params("@AuditUserID", SqlDbType.Int, eFamily.AuditUserID)
        data.params("@PhotoFolder", SqlDbType.NVarChar, 255, eFamily.tblInsuree.tblPhotos.PhotoFolder)
        data.params("@PhotoFileName", SqlDbType.NVarChar, 255, eFamily.tblInsuree.tblPhotos.PhotoFileName)
        data.params("@OfficerID", SqlDbType.Int, eFamily.tblInsuree.tblPhotos.OfficerID)
        data.params("@isOffline", SqlDbType.Bit, eFamily.isOffline)
        data.params("@ValidityFrom", SqlDbType.SmallDateTime, Date.Now)
        data.params("@FamilyType", SqlDbType.NVarChar, 2, If(eFamily.FamilyType Is Nothing, Nothing, eFamily.FamilyType))
        data.params("@FamilyAddress", SqlDbType.NVarChar, 200, eFamily.FamilyAddress)
        data.params("@Email", SqlDbType.NChar, 100, eFamily.tblInsuree.Email)
        data.params("@Education", SqlDbType.SmallInt, eFamily.tblInsuree.Education)
        data.params("@Profession", SqlDbType.SmallInt, eFamily.tblInsuree.Profession)
        'Addition for Nepal >> Start

        'data.params("@CurDistrictF", SqlDbType.Int, eFamily.CurDistrict, ParameterDirection.Input)
        'data.params("@CurVDCF", SqlDbType.Int, eFamily.CurVDC, ParameterDirection.Input)
        'data.params("@CurWardF", SqlDbType.Int, eFamily.CurWard, ParameterDirection.Input)

        data.params("@TypeOfId", SqlDbType.NVarChar, 1, eFamily.tblInsuree.TypeOfId)
        data.params("@HFID", SqlDbType.Int, If(eFamily.tblInsuree.tblHF.HfID = 0, DBNull.Value, eFamily.tblInsuree.tblHF.HfID))
        data.params("@CurrentAddress", SqlDbType.NVarChar, 50, eFamily.tblInsuree.CurrentAddress)
        'data.params("@CurDistrict", SqlDbType.Int, eFamily.tblInsuree.CurDistrict, ParameterDirection.Input)
        data.params("@CurrentVillage", SqlDbType.Int, eFamily.tblInsuree.CurrentVillage, ParameterDirection.Input)
        'data.params("@CurWard", SqlDbType.Int, eFamily.tblInsuree.CurrentVillage, ParameterDirection.Input)
        data.params("@GeoLocation", SqlDbType.NVarChar, 50, eFamily.tblInsuree.GeoLocation)
        'Addition for Nepal >> End
        data.ExecuteCommand()
        eFamily.FamilyID = data.sqlParameters("@FamilyID")

    End Sub
    'Corrected
    Public Sub UpdateFamily(ByRef eFamily As IMIS_EN.tblFamilies)
        Dim data As New ExactSQL
        Dim str As String = "insert into tblFamilies ([insureeid],[Poverty],[ConfirmationType],isOffline" & _
              ",[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID],FamilyType, FamilyAddress,Ethnicity,ConfirmationNo, LocationId) select [insureeid]" & _
              ",[Poverty],[ConfirmationType],isOffline,[ValidityFrom],getdate(),@FamilyID, @AuditUserID,FamilyType, FamilyAddress,Ethnicity,ConfirmationNo,LocationId" & _
              " from tblFamilies where FamilyID = @FamilyID; update [tblFamilies] set " & _
              "[Poverty] = @Poverty, [ConfirmationType] = @ConfirmationType, isOffline=@isOffline,[ValidityFrom]=GETDATE()" & _
              ",[AuditUserID] = @AuditUserID,FamilyType = @FamilyType, FamilyAddress = @FamilyAddress, Ethnicity = @Ethnicity, ConfirmationNo = @ConfirmationNo, LocationId = @LocationId where FamilyID = @FamilyID"
        data.setSQLCommand(str, CommandType.Text)
        data.params("@FamilyID", SqlDbType.Int, eFamily.FamilyID)
        data.params("@Poverty", SqlDbType.Bit, eFamily.Poverty, ParameterDirection.Input)
        data.params("@ConfirmationType", SqlDbType.NVarChar, 3, eFamily.ConfirmationType)
        data.params("@Ethnicity", SqlDbType.NVarChar, 1, eFamily.Ethnicity, ParameterDirection.Input)
        data.params("@AuditUserID", SqlDbType.Int, eFamily.AuditUserID)
        data.params("@isOffline", SqlDbType.Bit, eFamily.isOffline)
        data.params("@FamilyType", SqlDbType.NVarChar, 2, If(eFamily.FamilyType.Length = 0, Nothing, eFamily.FamilyType))
        data.params("@FamilyAddress", SqlDbType.NVarChar, 200, eFamily.FamilyAddress)
        data.params("@ConfirmationNo", SqlDbType.NVarChar, 12, eFamily.ConfirmationNo)
        data.params("@LocationId", SqlDbType.Int, eFamily.LocationId)

        data.ExecuteCommand()
    End Sub

    'Corrected
    Public Function GetFamilyFiltered(ByVal eFamily As IMIS_EN.tblFamilies, ByVal All As Boolean, ByVal Allpoverty As Boolean, ByVal GetYesNo As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        'sSQL = "select f.isOffline,f.FamilyId,i.CHFID,i.LastName,i.Othernames,D.DistrictName,W.WardName, V.VillageName,F.Poverty as Poverty,"
        'sSQL += " CASE WHEN F.Poverty = 1 THEN 'Yes' ELSE 'No' END PovertyDisplay, F.ConfirmationType,F.Ethnicity,  RegionName,f.validityfrom,f.validityTo"
        'sSQL += " from tblfamilies f"
        'sSQL += " INNER JOIN tblInsuree i ON f.InsureeID = i.insureeid"
        'sSQL += " inner join tblVillages V on V.VillageId = f.LocationId"
        'sSQL += " inner join tblWards W ON W.WardId = V.WardId"
        'sSQL += " inner join tblDistricts D ON D.DistrictId = W.DistrictId"
        'sSQL += " left outer join tblRegions R ON R.RegionId = D.Region"
        'sSQL += " inner join (SELECT LocationId, UserId, ValidityTo FROM tblUsersDistricts UNION SELECT DistrictId, @UserId UserId, NULL FROM tblDistricts WHERE Region IS NULL) UD on UD.LocationId = D.DistrictID and UD.userid = @userid and UD.ValidityTo is null"
        'sSQL += " where lastname like @Lastname"
        'sSQL += " And othernames like @OtherNames"
        'sSQL += " And CHFID like @CHFID"
        'sSQL += " And  (Gender like @Gender  OR Gender is null)"
        'sSQL += " And isnull(Marital,'')  like @Marital"
        'sSQL += " And isnull(Phone,'') like @Phone"

        'sSQL += " SELECT F.isOffline,F.FamilyId,I.CHFID,I.LastName,I.Othernames,L.DistrictName,L.WardName, L.VillageName,F.Poverty AS Poverty,"
        'sSQL += " CASE WHEN F.Poverty = 1 THEN 'Yes' ELSE 'No' END PovertyDisplay, F.ConfirmationType,F.Ethnicity,  RegionName,F.validityfrom,"
        'sSQL += " F.validityTo"
        'sSQL += " FROM tblfamilies F"
        'sSQL += " INNER JOIN tblInsuree I ON F.InsureeID = I.insureeid  AND   I.ValidityTo IS NULL"
        'sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId,0) = ISNULL(F.LocationId,0)"
        'sSQL += " INNER JOIN (SELECT L.DistrictId, L.RegionId FROM tblUsersDistricts UD"
        'sSQL += " INNER JOIN uvwLocations L ON L.DistrictId = UD.LocationId"
        'sSQL += " WHERE UD.ValidityTo IS NULL AND (UD.UserId = @UserId OR @UserId = 0)"
        'sSQL += " GROUP BY L.DistrictId, L.RegionId )UD ON UD.DistrictId = L.DistrictId  OR UD.RegionId = L.RegionId OR F.LocationId IS NULL"
        'sSQL += " WHERE LastName LIKE @Lastname"
        'sSQL += " AND OtherNames LIKE @OtherNames"
        'sSQL += " AND CHFID LIKE @CHFID"
        'sSQL += " AND  (Gender LIKE @Gender  OR Gender IS NULL)"
        'sSQL += " AND ISNULL(Marital,'')  LIKE @Marital And ISNULL(Phone,'') LIKE @Phone"

        sSQL += " ;WITH UD AS ("
        sSQL += " SELECT L.DistrictId, L.Region FROM tblUsersDistricts UD "
        sSQL += " INNER JOIN tblDistricts L ON L.DistrictId = UD.LocationId "
        sSQL += " WHERE UD.ValidityTo IS NULL AND (UD.UserId = @UserId OR @UserId = 0)  "
        sSQL += " GROUP BY L.DistrictId, L.Region ) "
        sSQL += " SELECT F.isOffline, F.FamilyID, F.FamilyUUID, I.CHFID, I.LastName, I.OtherNames, L.DistrictName, L.WardName, L.VillageName, F.Poverty,CASE WHEN F.Poverty = 1 THEN 'Yes' ELSE 'No' END PovertyDisplay,F.ConfirmationType,F.Ethnicity,  RegionName,F.validityfrom, F.validityTo  "
        sSQL += " FROM tblFamilies F INNER JOIN tblInsuree I ON I.InsureeId = F.InsureeID INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(F.LocationId, 0) "
        sSQL += " WHERE (L.RegionId IN (SELECT Region FROM UD) OR (L.DistrictId IN (SELECT DistrictId FROM UD)) OR F.LocationId IS NULL) "
        sSQL += " AND LastName LIKE @Lastname "
        sSQL += " AND OtherNames LIKE @OtherNames "
        sSQL += " AND CHFID LIKE @CHFID "
        sSQL += " AND (Gender LIKE @Gender  OR Gender IS NULL) "
        sSQL += " AND ISNULL(Marital,'')  LIKE @Marital "
        sSQL += " And ISNULL(Phone,'') LIKE @Phone  "
        'sSQL += " AND F.ValidityTo is null "
        sSQL += " AND I.ValidityTo IS NULL "
        ' sSQL += " GROUP BY F.isOffline, F.FamilyID, I.CHFID, I.LastName, I.OtherNames, L.DistrictName, L.WardName, L.VillageName, F.Poverty,F.ConfirmationType,F.Ethnicity,  RegionName,F.validityfrom, F.validityTo "



        If Not eFamily.RegionId = 0 Then
            sSQL += " And  L.RegionId =  @RegionId "
        End If
        If Not eFamily.DistrictID = 0 Then
            sSQL += " And  L.DistrictID =  @DistrictID "
        End If
        If Not eFamily.WardID = 0 Then
            sSQL += " And  L.WardID =  @WardID "
        End If
        If Not eFamily.LocationId = 0 Then
            sSQL += " And  L.VillageId =  @VillageID "
        End If
        If CDbl(eFamily.tblInsuree.DOBFrom.ToOADate) > 2 Then
            sSQL += " And  DOB >= @DOBFrom "
        End If
        If CDbl(eFamily.tblInsuree.DOBTo.ToOADate) > 2 Then
            sSQL += " And  DOB <= @DOBTo "
        End If
        If Not Allpoverty = True Then
            sSQL += " AND  F.Poverty  = @Poverty"
        End If
        If All = False Then
            sSQL = sSQL & " and F.ValidityTo is null"
        End If
        If eFamily.isOffline IsNot Nothing Then
            If eFamily.isOffline Then
                sSQL += " AND F.isOffline = 1"
            End If
        End If
        If eFamily.ConfirmationNo.Trim.Length > 0 Then
            sSQL += " AND F.ConfirmationNo like @ConfirmationNo"
        End If
        If eFamily.tblInsuree.Email.Trim.Length > 0 Then
            sSQL += " AND I.Email like @Email"
        End If

        'sSQL += " GROUP BY  F.isOffline,F.FamilyId,I.CHFID,I.LastName,I.Othernames,L.DistrictName,L.WardName, L.VillageName,F.Poverty ,"
        sSQL += " GROUP BY F.isOffline, F.FamilyID, I.CHFID, I.LastName, I.OtherNames, L.DistrictName, L.WardName, L.VillageName, F.Poverty,F.ConfirmationType,F.Ethnicity,  RegionName,F.validityfrom, F.validityTo, F.FamilyUUID "
        'sSQL += " CASE WHEN F.Poverty = 1 THEN 'Yes' ELSE 'No' END, F.ConfirmationType,F.Ethnicity,  RegionName,F.validityfrom,"
        sSQL += " ,F.validityTo,I.ValidityTo "
        sSQL = sSQL & " ORDER BY Familyid DESC, validityto"
        'changed by amani added timeout:=0 12/12/2017
        data.setSQLCommand(sSQL, CommandType.Text, timeout:=0)
        data.params("@UserId", SqlDbType.Int, eFamily.AuditUserID)
        data.params("@DOBFrom", SqlDbType.Date, eFamily.tblInsuree.DOBFrom)
        data.params("@DOBTo", SqlDbType.Date, eFamily.tblInsuree.DOBTo)
        data.params("@LastName", SqlDbType.NVarChar, 100, eFamily.tblInsuree.LastName & "%")
        data.params("@OtherNames", SqlDbType.NVarChar, 100, eFamily.tblInsuree.OtherNames & "%")
        data.params("@CHFID", SqlDbType.NVarChar, 12, eFamily.tblInsuree.CHFID & "%")

        data.params("@Gender", SqlDbType.Char, 1, eFamily.tblInsuree.Gender & "%")
        data.params("@Marital", SqlDbType.Char, 1, eFamily.tblInsuree.Marital & "%")
        data.params("@Phone", SqlDbType.NVarChar, 50, eFamily.tblInsuree.Phone & "%")
        If Not Allpoverty = True Then
            data.params("@Poverty", SqlDbType.Bit, eFamily.Poverty, ParameterDirection.Input)
        End If
        data.params("@WardID", SqlDbType.Int, eFamily.WardID)
        data.params("@VillageID", SqlDbType.Int, eFamily.LocationId)
        data.params("@DistrictID", SqlDbType.Int, eFamily.DistrictID)
        data.params("@RegionId", SqlDbType.Int, eFamily.RegionId)
        data.params("@dtYesNo", GetYesNo, "xAttributeV")
        data.params("@ConfirmationNo", SqlDbType.NVarChar, 12, eFamily.ConfirmationNo & "%")
        data.params("@Email", SqlDbType.NVarChar, 50, eFamily.tblInsuree.Email & "%")


        Return data.Filldata
    End Function
    
   
    'Corrected Rogers
    Public Sub GetFamilyHeadInfo(ByVal eFamily As IMIS_EN.tblFamilies)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " SELECT TOP 1 F.isOffline,F.ValidityTo,F.FamilyId, ISNULL(L.VillageID,0) VillageID,  ISNULL(L.WardID,0) WardID,L.RegionId, L.RegionName, ISNULL(L.DistrictID,0) DistrictID, DistrictName,WardName,VillageName"
        sSQL += ",FT.FamilyType, ISNULL(FT.AltLanguage, FT.FamilyType) AltLanguage,"
        sSQL += " F.FamilyAddress,F.Poverty, CT.ConfirmationType, CT.ConfirmationTypeCode, LastName,Othernames,Phone, CHFID,I.InsureeID,I.isOffline As InsureeIsOffline,"
        sSQL += " F.Ethnicity, ConfirmationNo FROM tblInsuree I"
        sSQL += " INNER JOIN tblFamilies F On ishead = 1 And I.ValidityTo Is NULL And I.FamilyId = isnull(F.LegacyID,F.FamilyID)"
        sSQL += " INNER JOIN uvwLocations L On ISNULL(F.LocationId,0) = ISNULL(L.LocationId,0)"
        sSQL += " LEFT JOIN tblFamilyTypes FT  On FT.FamilyTypeCode = F.FamilyType"
        sSQL += " LEFT JOIN tblConfirmationTypes CT On CT.ConfirmationTypeCode=F.ConfirmationType WHERE F.FamilyID = @FamilyId"



        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@FamilyId", SqlDbType.Int, eFamily.FamilyID)
        Dim dr As DataRow = data.Filldata()(0)
        Dim eInsurees As New IMIS_EN.tblInsuree

        If Not dr Is Nothing Then
            eInsurees.InsureeID = dr("InsureeID")
            eInsurees.CHFID = dr("CHFID")
            eInsurees.LastName = dr("LastName")
            eInsurees.OtherNames = dr("Othernames")
            If Not dr("Phone") Is DBNull.Value Then
                eInsurees.Phone = dr("Phone")
            End If
            If dr("InsureeIsOffline") IsNot DBNull.Value Then
                eInsurees.isOffline = dr("InsureeIsOffline")
            End If
            eFamily.Poverty = If(dr("Poverty") Is DBNull.Value, Nothing, dr("Poverty"))
            If dr("isOffline") IsNot DBNull.Value Then
                eFamily.isOffline = dr("isOffline")
            End If
            eFamily.ConfirmationType = dr("ConfirmationType").ToString
            eFamily.ConfirmationTypeCode = dr("ConfirmationTypeCode").ToString
            eFamily.Ethnicity = dr("Ethnicity").ToString
            eFamily.RegionId = If(dr("RegionId") Is DBNull.Value, 0, dr("RegionId"))
            eFamily.RegionName = dr("RegionName").ToString
            eFamily.DistrictName = dr("DistrictName").ToString
            eFamily.DistrictId = dr("DistrictID")
            eFamily.LocationId = dr("VillageID") 'dr("VillageID")
            eFamily.VillageName = dr("VillageName").ToString
            eFamily.WardId = dr("WardID")
            eFamily.WardName = dr("WardName").ToString
            eFamily.tblInsuree = eInsurees
            Dim eFamilyType As New IMIS_EN.tblFamilyTypes
            eFamilyType.FamilyType = dr("FamilyType").ToString
            eFamilyType.AltLanguage = dr("AltLanguage").ToString
            eFamily.tblFamilyTypes = eFamilyType
            eFamily.FamilyAddress = dr("FamilyAddress").ToString
            eFamily.ConfirmationNo = dr("ConfirmationNo").ToString

            If Not dr("ValidityTo") Is DBNull.Value Then
                eFamily.ValidityTo = dr("ValidityTo")
            End If
        End If

    End Sub
    Public Function FamilyExists(ByVal CHFID As String) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        Dim dt As New DataTable
        sSQL = "Select * FROM tblInsuree WHERE CHFID = '" & CHFID & "' AND ValidityTo IS NULL"
        data.setSQLCommand(sSQL, CommandType.Text)
        Return data.Filldata
    End Function
    Public Function CheckCanBeDeleted(ByVal FamilyID As Integer) As DataTable
        'Dim str As String = "SELECT f.* FROM tblFamilies f INNER JOIN tblInsuree i ON f.FamilyID=i.FamilyID LEFT JOIN tblPolicy po" & _
        '                    " ON po.FamilyID=f.FamilyID WHERE f.FamilyID=@FamilyID AND i.IsHead=@isHead AND i.ValidityTo IS NULL AND i.LegacyID IS NULL"



        Dim sSQL As String = ""
        sSQL = "SELECT 1"
        sSQL += " FROM tblFamilies f INNER JOIN tblInsuree i ON f.FamilyID=i.FamilyID"
        sSQL += " WHERE F.ValidityTo IS NULL"
        sSQL += " AND I.ValidityTo IS NULL"
        sSQL += " AND I.IsHead = 0"
        sSQL += " AND F.FamilyId  = @FamilyID"
        sSQL += " UNION ALL"
        sSQL += " SELECT 1 RecordFound"
        sSQL += " FROM tblFamilies f INNER JOIN tblInsuree i ON f.FamilyID=i.FamilyID"
        sSQL += " INNER JOIN tblPolicy po ON po.FamilyID=f.FamilyID AND po.ValidityTo IS NULL"
        sSQL += " LEFT OUTER JOIN tblPremium PR ON PR.PolicyId = po.PolicyID AND PR.ValidityTo IS NULL"
        sSQL += " WHERE f.FamilyID=@FamilyID"
        sSQL += " AND i.ValidityTo IS NULL"
        sSQL += " AND i.ValidityTo IS NULL"
        sSQL += " AND i.isHead = 1"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@FamilyID", SqlDbType.Int, FamilyID)
        data.params("@isHead", SqlDbType.Int, False)
        Return data.Filldata()
    End Function
    'Corrected
    Public Function DeleteFamily(ByVal eFamily As IMIS_EN.tblFamilies) As Boolean
        Dim str As String = "insert into tblFamilies ([insureeid],LocationId, [Poverty], [ConfirmationType],isOffline,[ValidityFrom],[ValidityTo]," & _
                            "[LegacyID],[AuditUserID],[Ethnicity], [ConfirmationNo])select [insureeid],LocationId,[Poverty], [ConfirmationType],isOffline,[ValidityFrom],getdate()," & _
                            " @FamilyID, [AuditUserID],Ethnicity, [ConfirmationNo] from tblFamilies where FamilyID = @FamilyID AND ValidityTo IS NULL;" & _
                            " update [tblFamilies] set [ValidityFrom]=GETDATE(),[ValidityTo]=GETDATE(),[AuditUserID] = @AuditUserID where FamilyID = @FamilyID AND ValidityTo IS NULL"

        data.setSQLCommand(str, CommandType.Text)
        data.params("@FamilyID", SqlDbType.Int, eFamily.FamilyID)
        data.params("@AuditUserID", SqlDbType.Int, eFamily.AuditUserID)
        data.ExecuteCommand()
        Return True
    End Function
    Public Function GetFamilyOfflineValue(ByVal FamilyID As Integer) As Boolean
        Dim Query As String = "SELECT isnull(isOffline,0) isOffline FROM tblFamilies where FamilyID=@FamilyID"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@FamilyID", SqlDbType.Int, FamilyID)
        Return CBool(data.Filldata().Rows(0)("isOffline"))
    End Function
    Public Function GetFamilyIdByUUID(ByVal uuid As Guid) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select FamilyId from tblFamilies where FamilyUUID = @FamilyUUID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@FamilyUUID", SqlDbType.UniqueIdentifier, uuid)

        Return data.Filldata
    End Function
    Public Function GetFamilyUUIDByID(ByVal id As Integer) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select FamilyUUID from tblFamilies where FamilyId = @FamilyId"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@FamilyId", SqlDbType.Int, id)

        Return data.Filldata
    End Function
End Class
