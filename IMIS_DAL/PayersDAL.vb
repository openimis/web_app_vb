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

Public Class PayersDAL
    
    'Corrected + Rogers
    Public Sub LoadPayer(ByRef ePayers As IMIS_EN.tblPayer)
        Dim eLocations As New IMIS_EN.tblLocations
        Dim data As New ExactSQL
        Dim dr As DataRow
        Dim sSQL As String = ""
        sSQL = "SELECT ISNULL(L.ParentLocationId ,L.LocationId) RegionId,"
        sSQL += " CASE WHEN L.ParentLocationId IS NULL THEN NULL ELSE L.LocationId END DistrictId,  P.*"
        sSQL += " FROM tblPayer P"
        sSQL += " LEFT OUTER JOIN tblLocations L ON L.LocationId = P.LocationId"
        sSQL += " WHERE PayerID =@PayerId"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PayerId", SqlDbType.Int, ePayers.PayerID)
        dr = data.Filldata()(0)
        If Not dr Is Nothing Then
            ePayers.PayerType = dr("PayerType")
            ePayers.PayerName = dr("PayerName")
            ePayers.PayerAddress = dr("PayerAddress")
            'If (Not dr("DistrictID") Is DBNull.Value) Then
            '    eDistrict.DistrictID = dr("DistrictID")
            'End If
            eLocations.LocationId = if(dr.IsNull("DistrictId"), Nothing, dr("DistrictId"))
            eLocations.RegionId = if(dr.IsNull("RegionId"), -1, dr("RegionId"))
            ePayers.tblLocations = eLocations
            ePayers.Phone = dr("Phone")
            ePayers.Fax = dr("Fax")
            ePayers.eMail = dr("eMail")
            If Not dr("ValidityTo") Is DBNull.Value Then
                ePayers.ValidityTo = dr("ValidityTo").ToString
            End If
        End If
    End Sub
    Public Function CheckIfPayerExists(ByVal ePayer As IMIS_EN.tblPayer) As Boolean
        Dim data As New ExactSQL

        Dim str As String = "Select Count(*) from tblPayer where PayerName = @PayerName AND PayerAddress = @PayerAddress AND LegacyID <> @PayerID"
        If Not ePayer.PayerID = 0 Then
            str += " AND tblPayer.PayerID <> @PayerID "
        End If
        data.setSQLCommand(str, CommandType.Text)
        data.params("@PayerName", SqlDbType.NVarChar, 100, ePayer.PayerName)
        data.params("@PayerAddress", SqlDbType.NVarChar, 100, ePayer.PayerAddress)
        data.params("@PayerID", SqlDbType.Int, ePayer.PayerID)

        Return data.ExecuteScalar()
    End Function

    'Corrected + Rogers
    Public Function GetPayers(ByVal ePayer As IMIS_EN.tblPayer, ByVal dtPayerType As DataTable, ByVal All As Boolean) As DataTable
        Dim data As New ExactSQL
        Dim sSql As String = ""

        sSql += " SELECT P.PayerId, P.PayerUUID, P.PayerUUID, P.PayerName, P.PayerAddress, P.Phone, P.ValidityFrom, P.ValidityTo,"
        sSql += " L.RegionName , L.DistrictName,"
        sSql += " P.PayerType, PayerType.AltLanguage, L.RegionId, L.DistrictId"
        sSql += " FROM tblPayer P"
        sSql += " INNER JOIN @dtPayerType Payertype on Payertype.code = P.Payertype"
        sSql += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(P.LocationId, 0)"

        sSql += " INNER JOIN ( SELECT L.DistrictId, L.RegionId FROM tblUsersDistricts UD"
        sSql += " INNER JOIN uvwLocations L ON L.DistrictId = UD.LocationId WHERE UD.ValidityTo IS NULL AND (UD.UserId = @UserId OR @UserId = 0)"
        sSql += " GROUP BY L.DistrictId, L.RegionId )UD ON UD.DistrictId = P.LocationId  OR UD.RegionId = P.LocationId OR P.LocationId IS NULL"


        sSql += " WHERE (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId = 0)"
        sSql += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL)"
        sSql += " AND P.PayerName LIKE @PayerName"
        sSql += " AND P.PayerType like @PayerType"
        sSql += " AND P.eMail LIKE @Email"
        sSql += " AND P.Phone  like @Phone"
       
        If Not All Then sSql += " AND P.ValidityTo is NULL"

        sSql += " GROUP BY P.PayerId, P.PayerName, P.PayerAddress, P.Phone, P.ValidityFrom, P.ValidityTo, L.RegionName , L.DistrictName, P.PayerType,  L.RegionId, L.DistrictId, PayerType.AltLanguage,L.ParentLocationId, P.PayerUUID"

        sSql += " ORDER BY  L.ParentLocationId,PayerName, P.ValidityTo"

        data.setSQLCommand(sSql, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, ePayer.AuditUserID)
        data.params("@PayerType", SqlDbType.Char, 1, ePayer.PayerType)
        data.params("@PayerName", SqlDbType.NVarChar, 100, ePayer.PayerName)
        data.params("@Phone", SqlDbType.NVarChar, 50, ePayer.Phone)
        data.params("@eMail", SqlDbType.NVarChar, 50, ePayer.eMail)
        data.params("@RegionId", SqlDbType.Int, ePayer.tblLocations.RegionId)
        data.params("@DistrictId", SqlDbType.Int, ePayer.tblLocations.DistrictID)

        data.params("@dtPayerType", dtPayerType, "xCareType")
        Return data.Filldata

    End Function

    'Corrected
    Public Function GetPayers(ByVal RegionId As Integer, ByVal DistrictId As Integer, ByVal UserId As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " SELECT  P.PayerId, P.PayerUUID, P.PayerName ,L.LocationId,L.RegionName,L.DistrictName"
        sSQL += " FROM tbLpayer P"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(P.LocationId, 0)"
        sSQL += " INNER JOIN ( SELECT L.DistrictId, L.RegionId FROM tblUsersDistricts UD"
        sSQL += " INNER JOIN uvwLocations L ON L.DistrictId = UD.LocationId"
        sSQL += " WHERE UD.ValidityTo IS NULL AND (UD.UserId = @UserId OR @UserId = 0)"
        sSQL += " GROUP BY L.DistrictId, L.RegionId )UD ON UD.DistrictId = P.LocationId  OR UD.RegionId = P.LocationId OR P.LocationId IS NULL"
        sSQL += " WHERE (L.Regionid = @RegionId OR L.LocationId = 0) "
        sSQL += " AND (L.DistrictId = @DistrictId OR L.DistrictId IS NULL OR  @DistrictId = 0 )" '
        sSQL += " AND P.ValidityTo IS NULL"
        sSQL += " GROUP BY P.PayerId, P.PayerName ,L.LocationId,L.RegionName,L.DistrictName, L.ParentLocationId"
        sSQL += " ORDER BY L.ParentLocationId"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@RegionId", SqlDbType.Int, RegionId)
        data.params("@DistrictId", SqlDbType.Int, DistrictId)
        data.params("@UserId", SqlDbType.Int, UserId)
        Return data.Filldata
    End Function

    'Corrected
    Public Sub DeletePayer(ByRef ePayers As IMIS_EN.tblPayer)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPayer ([PayerType],[PayerName],[PayerAddress],[LocationId],[Phone],[Fax],[eMail],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID])" _
      & " select [PayerType],[PayerName],[PayerAddress],[LocationId],[Phone],[Fax],[eMail],[ValidityFrom],getdate(),@PayerID,[AuditUserID] from tblPayer where PayerID = @PayerID;" _
      & "UPDATE [tblPayer] SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate(),[AuditUserID] = @AuditUserID  WHERE PayerID = @PayerID", CommandType.Text)
        data.params("@PayerID", SqlDbType.Int, ePayers.PayerID)
        data.params("@AuditUserID", SqlDbType.Int, ePayers.AuditUserID)
        data.ExecuteCommand()

    End Sub

    'Corrected
    Public Sub InsertPayer(ByRef ePayers As IMIS_EN.tblPayer)
        Dim data As New ExactSQL
        data.setSQLCommand("Insert into tblPayer([PayerType],[PayerName],[PayerAddress],[LocationId],[Phone],[Fax],[eMail],[AuditUserID])" _
        & "VALUES(@PayerType, @PayerName,@PayerAddress,case when @LocationId = 0 then null else @LocationId end,@Phone,@Fax,@Email,@AuditUserID)", CommandType.Text)
        data.params("@PayerType", SqlDbType.Char, 1, ePayers.PayerType)
        data.params("@PayerName", SqlDbType.NVarChar, 100, ePayers.PayerName)
        data.params("@PayerAddress", SqlDbType.NVarChar, 100, ePayers.PayerAddress)
        data.params("@LocationId", SqlDbType.Int, if(ePayers.tblLocations.LocationId = -1, Nothing, ePayers.tblLocations.LocationId))
        data.params("@Phone", SqlDbType.NVarChar, 50, ePayers.Phone)
        data.params("@Fax", SqlDbType.NVarChar, 50, ePayers.Fax)
        data.params("@eMail", SqlDbType.NVarChar, 50, ePayers.eMail)
        data.params("@AuditUserID", SqlDbType.Int, ePayers.AuditUserID)
        data.ExecuteCommand()
    End Sub

    'Corrected
    Public Sub UpdatePayer(ByRef ePayers As IMIS_EN.tblPayer)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblPayer ([PayerType],[PayerName],[PayerAddress],[LocationId],[Phone],[Fax],[eMail],[ValidityTo],[LegacyID],[AuditUserID])" _
      & " select [PayerType],[PayerName],[PayerAddress],[LocationId],[Phone],[Fax],[eMail],getdate(),@PayerID,[AuditUserID] from tblPayer where PayerID = @PayerID;" _
      & "UPDATE [tblPayer] SET [PayerType] = @PayerType,[PayerName] = @PayerName,[PayerAddress] = @PayerAddress,[LocationId] = case when @LocationId = 0 then null else @LocationId end,[Phone] = @Phone ,[Fax] = @Fax" _
      & ",[eMail] = @eMail,[ValidityFrom] = GetDate()" _
      & ",LegacyID = @LegacyID,[AuditUserID] = @AuditUserID  WHERE PayerID = @PayerID", CommandType.Text)
        data.params("@PayerID", SqlDbType.Int, ePayers.PayerID)
        data.params("@PayerType", SqlDbType.Char, 1, ePayers.PayerType)
        data.params("@PayerName", SqlDbType.NVarChar, 100, ePayers.PayerName)
        data.params("@PayerAddress", SqlDbType.NVarChar, 100, ePayers.PayerAddress)
        data.params("@LocationId", SqlDbType.Int, If(ePayers.tblLocations.LocationId = -1, Nothing, ePayers.tblLocations.LocationId))
        data.params("@Phone", SqlDbType.NVarChar, 50, ePayers.Phone)
        data.params("@Fax", SqlDbType.NVarChar, 50, ePayers.Fax)
        data.params("@eMail", SqlDbType.NVarChar, 50, ePayers.eMail)
        data.params("@LegacyID", SqlDbType.Int, 1, ParameterDirection.Output)
        data.params("@AuditUserID", SqlDbType.Int, ePayers.AuditUserID)
        data.ExecuteCommand()

    End Sub

    Public Function GetPayerIdByUUID(ByVal uuid As Guid) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select PayerID from tblPayer where PayerUUID = @PayerUUID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PayerUUID", SqlDbType.UniqueIdentifier, uuid)

        Return data.Filldata
    End Function
End Class
