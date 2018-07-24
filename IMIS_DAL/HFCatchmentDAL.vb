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

Public Class HFCatchmentDAL
    Private sSQL As String = ""
    Private data As New ExactSQL
    Public Sub SaveHFCatchment(ByVal dtData As DataTable, ByVal HfId As Integer, ByVal AuditUserId As Integer)


        ' sSQL += " --DELETED"
        sSQL += " INSERT INTO tblHFCatchment(HFID, LocationId, Catchment, ValidityFrom, ValidityTo, LegacyId, AuditUserId)"
        sSQL += " SELECT HFID, LocationId, Catchment, ValidityFrom, ValidityTo, LegacyId, AuditUserId"
        sSQL += " FROM tblHFCatchment"
        sSQL += " WHERE HFID = (SELECT TOP 1 HFID FROM @tblData)"
        sSQL += " AND HFCatchmentId NOT IN (SELECT HFCatchmentId FROM @tblData WHERE HFCatchmentId IS NOT NULL)"
        sSQL += " AND ValidityTo IS NULL"
        sSQL += " UPDATE HC SET ValidityTo = GETDATE()"
        sSQL += " FROM tblHFCatchment HC"
        sSQL += " WHERE HFID = (SELECT TOP 1 HFID FROM @tblData)"
        sSQL += " AND HFCatchmentId NOT IN (SELECT HFCatchmentId FROM @tblData WHERE HFCatchmentId IS NOT NULL)"
        sSQL += " AND ValidityTo IS NULL"


        'sSQL += " --UPDATE"
        sSQL += " INSERT INTO tblHFCatchment(HFID, LocationId, Catchment, ValidityFrom, ValidityTo, LegacyId, AuditUserId)"
        sSQL += " SELECT HC.HFID, HC.LocationId,HC.Catchment, HC.ValidityFrom, GETDATE()ValidityTo, HC.HFCatchmentId LegacyId, @AuditUserId"
        sSQL += " FROM tblHFCatchment HC"
        sSQL += " INNER JOIN @tbldata DT ON DT.HFCatchmentId = HC.HFCatchmentId"
        sSQL += " WHERE HC.ValidityTo IS NULL"
        sSQL += " AND HC.Catchment <> DT.Catchment;"
        sSQL += " UPDATE HC SET Catchment = DT.Catchment"
        sSQL += " FROM tblHFCatchment HC"
        sSQL += " INNER JOIN @tbldata DT ON DT.HFCatchmentId = HC.HFCatchmentId"
        sSQL += " AND HC.ValidityTo IS NULL"
        sSQL += " AND HC.Catchment <> DT.Catchment;"
        ' sSQL += " ----INSERT"
        sSQL += " INSERT INTO tblHFCatchment(HFID, LocationId, Catchment, ValidityFrom, AuditUserId)"
        sSQL += " SELECT @HfId, DT.LocationId, DT.Catchment, GETDATE() ValidityFrom,@AuditUserId"
        sSQL += " FROM tblHFCatchment HC"
        sSQL += " RIGHT OUTER JOIN @tbldata DT ON DT.HFCatchmentId = HC.HFCatchmentId"
        sSQL += " WHERE DT.HFCatchmentId IS NULL;"




        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@tblData", dtData, "xHFCatchment")
        data.params("@HfId", SqlDbType.Int, HfId)
        data.params("@AuditUserId", SqlDbType.Int, AuditUserId)
        data.ExecuteCommand()
    End Sub
    Public Function LoadRegions(HfId As Integer) As DataTable
        sSQL += " ;WITH Regions AS("
        sSQL += " SELECT DISTINCT ROW_NUMBER() OVER (PARTITION BY RW.RegionId ORDER BY IIF(@HFID=HC.HFID,IIF(R.RegionId = RW.RegionId, 1, 0),0) DESC) Rno,  IIF(@HFID=HC.HFID,IIF(R.RegionId = RW.RegionId, 1, 0),0)Checked, RW.RegionName, RW.RegionId"
        sSQL += " FROM tblHFCatchment HC INNER JOIN tblVillages V ON V.VillageId = HC.LocationId"
        sSQL += " INNER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId=W.DistrictId"
        sSQL += " INNER JOIN tblRegions R ON R.RegionId=D.Region"
        sSQL += " RIGHT JOIN tblRegions RW ON 1=1"
        sSQL += " WHERE HC.ValidityTo IS NULL  AND RW.ValidityTo IS NULL"
        sSQL += " GROUP BY IIF(@HFID=HC.HFID,IIF(R.RegionId = RW.RegionId, 1, 0),0), RW.RegionName, RW.RegionId"
        sSQL += " )"
        sSQL += " SELECT * FROM Regions WHERE Rno=1"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@HFID", SqlDbType.Int, HfId)
        Return data.Filldata
    End Function

    Public Function LoadDistricts(HfId As Integer) As DataTable
        sSQL += " SELECT IIF(DW.DistrictId = D.DistrictId, 1, 0)Checked, DW.DistrictName, D.Region, DW.DistrictId"
        sSQL += " FROM tblHFCatchment HC"
        sSQL += " INNER JOIN tblVillages V ON V.VillageId = HC.LocationId"
        sSQL += " INNER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " INNER JOIN tblDistricts D ON D.DistrictId=W.DistrictId"
        sSQL += " INNER JOIN tblDistricts DW ON DW.Region = D.Region"
        sSQL += " WHERE HC.ValidityTo IS NULL AND HC.HFID=@HFID"
        sSQL += " GROUP BY IIF(DW.DistrictId = D.DistrictId, 1, 0), DW.DistrictName, D.Region, DW.DistrictId"
        sSQL += " ORDER BY IIF(DW.DistrictId = D.DistrictId, 1, 0) DESC"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@HFID", SqlDbType.Int, HfId)
        Return data.Filldata
    End Function
    Public Function LoadWards(HfId As Integer) As DataTable
        sSQL += " SELECT IIF(AW.WardId = W.WardId, 1, 0)Checked,AW.WardId, AW.DistrictId, AW.WardName"
        sSQL += " FROM tblHFCatchment HC"
        sSQL += " INNER JOIN tblVillages V ON V.VillageId = HC.LocationId"
        sSQL += " INNER JOIN tblWards W ON W.WardId = V.WardId"
        sSQL += " INNER JOIN tblWards AW ON AW.DistrictId = W.DistrictId"
        sSQL += " WHERE HC.ValidityTo IS NULL"
        sSQL += " AND HC.HFID = @HFID"
        sSQL += " GROUP BY IIF(AW.WardId = W.WardId, 1, 0),AW.WardId, AW.DistrictId, AW.WardName"
        sSQL += " ORDER BY IIF(AW.WardId = W.WardId, 1, 0) DESC"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@HFID", SqlDbType.Int, HfId)
        Return data.Filldata
    End Function
    Public Function LoadVilages(HfId As Integer) As DataTable
        sSQL = " ;WITH Villages AS"
        sSQL += " ("
        sSQL += " SELECT AV.VillageId, AV.WardId, AV.VillageName"
        sSQL += " FROM tblHFCatchment HC"
        sSQL += " INNER JOIN tblVillages V ON V.VillageId = HC.LocationId"
        sSQL += " INNER JOIN tblVillages AV ON AV.WardId = V.WardId"
        sSQL += " WHERE HC.ValidityTo IS NULL"
        sSQL += " AND HC.HFID = @HFID"
        sSQL += " )"
        'Ward Source
        sSQL += " SELECT IIF(HC.HFCatchmentId IS NULL, 0, 1) Checked, Villages.VillageId,Villages.WardId, Villages.VillageName, HC.HFCatchmentId, HC.Catchment"
        sSQL += " FROM Villages"
        sSQL += " LEFT OUTER JOIN tblHFCatchment HC ON HC.LocationId = Villages.VillageId AND HC.HFID = @HFID"
        sSQL += " WHERE HC.ValidityTo IS NULL"
        sSQL += " GROUP BY IIF(HC.HFCatchmentId IS NULL, 0, 1), Villages.VillageId,Villages.WardId, Villages.VillageName, HC.HFCatchmentId, HC.Catchment"
        sSQL += " ORDER BY IIF(HC.HFCatchmentId IS NULL, 0, 1) DESC"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@HFID", SqlDbType.Int, HfId)
        Return data.Filldata
    End Function
End Class
