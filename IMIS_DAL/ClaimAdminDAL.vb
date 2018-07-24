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

Public Class ClaimAdminDAL
    Private Query As String = String.Empty
    Private Data As New ExactSQL
    Public Sub LoadClaimAdmin(ByRef eClaimAdmin As IMIS_EN.tblClaimAdmin)
        Query = "SELECT ClaimAdminId,ClaimAdminCode,LastName,OtherNames,DOB,Phone,EmailId,HFId" & _
           ",ValidityFrom,ValidityTo,LegacyId,AuditUserId from tblClaimAdmin where ClaimAdminId=@ClaimAdminId"
        Data.setSQLCommand(Query, CommandType.Text)
        Data.params("@ClaimAdminId", SqlDbType.Int, eClaimAdmin.ClaimAdminId)
        Dim dr As DataRow = Data.Filldata()(0)
        If Not dr Is Nothing Then
            Dim eHF As New IMIS_EN.tblHF
            If dr("HFId") IsNot DBNull.Value Then eHF.HfID = dr("HFId")
            eClaimAdmin.tblHF = eHF
            eClaimAdmin.ClaimAdminId = dr("ClaimAdminId")
            eClaimAdmin.ClaimAdminCode = if(dr("ClaimAdminCode") Is DBNull.Value, Nothing, dr("ClaimAdminCode"))
            eClaimAdmin.LastName = if(dr("LastName") Is DBNull.Value, Nothing, dr("LastName"))
            eClaimAdmin.OtherNames = if(dr("OtherNames") Is DBNull.Value, Nothing, dr("OtherNames"))
            eClaimAdmin.DOB = if(dr("DOB") Is DBNull.Value, Nothing, dr("DOB"))
            eClaimAdmin.Phone = if(dr("Phone") Is DBNull.Value, Nothing, dr("Phone"))
            eClaimAdmin.EmailId = dr("EmailId").ToString
            If Not dr("ValidityTo") Is DBNull.Value Then
                eClaimAdmin.ValidityTo = dr("ValidityTo").ToString
            End If
        End If
    End Sub

    'Corrected
    Public Function GetClaimAdmins(ByVal eClaimAdmin As IMIS_EN.tblClaimAdmin, ByVal All As Boolean) As DataTable
        Query = "select ClaimAdminId,ClaimAdminCode,ClaimAdminCode +' - '+ ca.LastName Description,ca.LastName,ca.OtherNames,ca.DOB,ca.Phone,ca.ValidityFrom" & _
           ",ca.ValidityTo,ca.LegacyId,ca.AuditUserId,tblHF.HfID,tblHF.HFCode, ca.EmailId from tblClaimAdmin ca" & _
           " inner join tblHF on ca.HFId = tblHF.HfID" & _
           " inner join tblUsersDistricts ud on tblHF.LocationId = ud.LocationId And ud.UserID = @UserID" & _
           " WHERE ca.ClaimAdminCode like @Code AND " & _
           " LastName LIKE @LastName AND OtherNames LIKE @OtherNames  AND ISNULL(ca.Phone,'') LIKE @Phone" & _
           " AND ISNULL(ca.EmailId,'') LIKE @EmailId"

        If Not eClaimAdmin.DOBFrom Is Nothing Then
            Query += " AND ca.DOB >= @DOBFrom"
        End If
        If Not eClaimAdmin.DOBTo Is Nothing Then
            Query += " AND ca.DOB <= @DOBTo"
        End If
        If eClaimAdmin.tblHF IsNot Nothing Then
            If eClaimAdmin.tblHF.HfID <> 0 Then
                Query += " AND ca.HFId = @HFId"
            End If
        End If
        If All = False Then
            Query += " AND ca.ValidityTo is NULL and tblHf.validityto is null And ud.validityto is null"
        End If

        Query += " order by ClaimAdminCode,ca.validityfrom desc"
        Data.setSQLCommand(Query, CommandType.Text)
        Data.params("@UserID", SqlDbType.Int, eClaimAdmin.AuditUserId)
        Data.params("@Code", SqlDbType.NVarChar, 8, eClaimAdmin.ClaimAdminCode & "%")
        Data.params("@LastName", SqlDbType.NVarChar, 100, eClaimAdmin.LastName & "%")
        Data.params("@OtherNames", SqlDbType.NVarChar, 100, eClaimAdmin.OtherNames & "%")
        If Not eClaimAdmin.DOBFrom Is Nothing Then
            Data.params("@DOBFrom", SqlDbType.Date, eClaimAdmin.DOBFrom)
        End If
        If Not eClaimAdmin.DOBTo Is Nothing Then
            Data.params("@DOBTo", SqlDbType.Date, eClaimAdmin.DOBTo)
        End If
        Data.params("@Phone", SqlDbType.NVarChar, 50, eClaimAdmin.Phone & "%")
        If eClaimAdmin.tblHF IsNot Nothing Then
            If eClaimAdmin.tblHF.HfID <> 0 Then
                Data.params("@HFId", SqlDbType.Int, eClaimAdmin.tblHF.HfID)
            End If
        End If
        Data.params("@EmailId", SqlDbType.NVarChar, 200, "%" & eClaimAdmin.EmailId & "%")
        Return Data.Filldata
    End Function
    Public Function GetHFClaimAdminCodes(ByVal HFID As Integer)
        Query = "select ClaimAdminId,ClaimAdminCode,ClaimAdminCode +' - '+ ca.LastName Description,ca.LastName,ca.OtherNames,ca.DOB,ca.Phone,ca.ValidityFrom" & _
           ",ca.ValidityTo,ca.LegacyId,ca.AuditUserId,HfID, ca.EmailId from tblClaimAdmin ca where HfID=@HfID And ValidityTo IS NULL"
        Data.setSQLCommand(Query, CommandType.Text)
        Data.params("@HfID", SqlDbType.Int, HFID)
        Return Data.Filldata()
    End Function
    Public Function DeleteClaimAdmin(ByRef eClaimAdmin As IMIS_EN.tblClaimAdmin) As Boolean
        Query = "INSERT INTO tblClaimAdmin(ClaimAdminCode,LastName,OtherNames,DOB,Phone,HFId" & _
           ",ValidityFrom,ValidityTo,LegacyId,AuditUserId, EmailId)" & _
        " select ClaimAdminCode,LastName,OtherNames,DOB,Phone,HFId" & _
        ",ValidityFrom,getdate(),@ClaimAdminId,AuditUserId, EmailId from tblClaimAdmin where ClaimAdminId = @ClaimAdminId;" & _
        " UPDATE tblClaimAdmin SET [ValidityTo]=Getdate(),[AuditUserID] = @AuditUserID WHERE ClaimAdminId = @ClaimAdminId"
        Data.setSQLCommand(Query, CommandType.Text)
        Data.params("@ClaimAdminId", SqlDbType.Int, eClaimAdmin.ClaimAdminId)
        Data.params("@AuditUserID", SqlDbType.Int, eClaimAdmin.AuditUserId)
        Data.ExecuteCommand()
        Return True
    End Function
    Public Function InsertClaimAdmin(ByRef eClaimAdmin As IMIS_EN.tblClaimAdmin) As Boolean
        Query = "INSERT INTO tblClaimAdmin(ClaimAdminCode,LastName,OtherNames,DOB,Phone,HFId" & _
           ",ValidityFrom,AuditUserId, EmailId)" & _
        " VALUES(@ClaimAdminCode,@LastName,@OtherNames,@DOB,@Phone,@HFId" & _
        ",getdate(),@AuditUserId, @EmailId)"
        Data.setSQLCommand(Query, CommandType.Text)
        Data.params("@ClaimAdminCode", SqlDbType.NVarChar, 8, eClaimAdmin.ClaimAdminCode)
        Data.params("@LastName", SqlDbType.NVarChar, 100, eClaimAdmin.LastName)
        Data.params("@OtherNames", SqlDbType.NVarChar, 100, eClaimAdmin.OtherNames)
        Data.params("@DOB", SqlDbType.DateTime, if(eClaimAdmin.DOB Is Nothing, SqlTypes.SqlDateTime.Null, eClaimAdmin.DOB))
        Data.params("@Phone", SqlDbType.NVarChar, 50, eClaimAdmin.Phone)
        Data.params("@HFId", SqlDbType.Int, if(eClaimAdmin.tblHF.HfID = 0, Nothing, eClaimAdmin.tblHF.HfID))
        Data.params("@AuditUserID", SqlDbType.Int, eClaimAdmin.AuditUserId)
        Data.params("@EmailId", SqlDbType.NVarChar, 200, eClaimAdmin.EmailId)
        Data.ExecuteCommand()
        Return True
    End Function
    Public Function UpdateClaimAdmin(ByRef eClaimAdmin As IMIS_EN.tblClaimAdmin) As Boolean
        Query = "INSERT INTO tblClaimAdmin(ClaimAdminCode,LastName,OtherNames,DOB,Phone,HFId" & _
           ",ValidityFrom,ValidityTo,LegacyId,AuditUserId, EmailId)" & _
           " select ClaimAdminCode,LastName,OtherNames,DOB,Phone,HFId" & _
           ",ValidityFrom,getdate(),@ClaimAdminId,AuditUserId, EmailId from tblClaimAdmin where ClaimAdminId = @ClaimAdminId;" & _
           " UPDATE tblClaimAdmin SET ClaimAdminCode=@ClaimAdminCode,LastName=@LastName,OtherNames=@OtherNames" & _
           ",DOB=@DOB,Phone=@Phone,HFId=@HFId" & _
           ",ValidityFrom = GetDate(),[AuditUserID] = @AuditUserID, EmailId = @EmailId  WHERE ClaimAdminId = @ClaimAdminId"
        Data.setSQLCommand(Query, CommandType.Text)
        Data.params("@ClaimAdminId", SqlDbType.Int, eClaimAdmin.ClaimAdminId)
        Data.params("@ClaimAdminCode", SqlDbType.NVarChar, 8, eClaimAdmin.ClaimAdminCode)
        Data.params("@LastName", SqlDbType.NVarChar, 100, eClaimAdmin.LastName)
        Data.params("@OtherNames", SqlDbType.NVarChar, 100, eClaimAdmin.OtherNames)
        Data.params("@DOB", SqlDbType.SmallDateTime, if(eClaimAdmin.DOB Is Nothing, SqlTypes.SqlDateTime.Null, eClaimAdmin.DOB))
        Data.params("@Phone", SqlDbType.NVarChar, 50, eClaimAdmin.Phone)
        Data.params("@HFId", SqlDbType.Int, if(eClaimAdmin.tblHF.HfID = 0, Nothing, eClaimAdmin.tblHF.HfID))
        Data.params("@AuditUserID", SqlDbType.Int, eClaimAdmin.AuditUserId)
        Data.params("@EmailId", SqlDbType.NVarChar, 200, eClaimAdmin.EmailId)
        Data.ExecuteCommand()
        Return True
    End Function
    Public Function ClaimAdminCodeExists(ByVal eClaimAdmin As IMIS_EN.tblClaimAdmin) As Boolean
        Query = "Select * from tblClaimAdmin where ClaimAdminCode = @ClaimAdminCode and ValidityTo is null" & _
                " AND ClaimAdminId <> @ClaimAdminId"
        Data.setSQLCommand(Query, CommandType.Text)
        Data.params("@ClaimAdminCode", SqlDbType.NVarChar, 8, eClaimAdmin.ClaimAdminCode)
        Data.params("@ClaimAdminId", SqlDbType.Int, eClaimAdmin.ClaimAdminId)
        If Data.Filldata().Rows.Count > 0 Then Return True
        Return False
    End Function
End Class
