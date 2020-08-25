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

Public Class MedicalServicesDAL
    Public Sub DeleteMedicalServices(ByVal eServices As IMIS_EN.tblServices)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblServices ([ServCode],[ServName],[ServType],[ServLevel],[ServPrice],[ServCareType],[ServFrequency],[ServPatCat],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID])" _
      & " select [ServCode],[ServName],[ServType],[ServLevel],[ServPrice],[ServCareType],[ServFrequency],[ServPatCat],[ValidityFrom],getdate(),@ServiceID,[AuditUserID] from tblServices where ServiceID = @ServiceID;" _
      & "UPDATE [tblServices] SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate(),[AuditUserID] = @AuditUserID  WHERE ServiceID = @ServiceID", CommandType.Text)
        data.params("@ServiceID", SqlDbType.Int, eServices.ServiceID)
        data.params("@AuditUserID", SqlDbType.Int, eServices.AuditUserID)
        data.ExecuteCommand()
    End Sub
    Public Sub LoadMedicalServices(ByRef eServices As IMIS_EN.tblServices)
        Dim data As New ExactSQL
        data.setSQLCommand("select *  from tblServices where ServiceID = @ServiceID", CommandType.Text)
        data.params("@ServiceID", SqlDbType.Int, eServices.ServiceID)
        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            eServices.ServCode = dr("ServCode")
            eServices.ServName = dr("ServName")
            eServices.ServType = dr("ServType")
            eServices.ServLevel = dr("ServLevel")
            eServices.ServPrice = dr("ServPrice")
            eServices.ServCareType = dr("ServCareType")
            eServices.ServFrequency = dr("ServFrequency")
            eServices.ServPatCat = dr("ServPatCat")
            eServices.ServCategory = dr("ServCategory").ToString

            eServices.ValidityTo = if(dr("ValidityTo").ToString = String.Empty, Nothing, dr("ValidityTo"))
        End If

    End Sub
    Public Function GetMedicalServices(Optional ByVal All As Boolean = False) As DataTable
        Dim data As New ExactSQL
        If All = True Then
            data.setSQLCommand("SELECT ServiceID,ServiceUUID,ServCode,ServName,CASE ServType WHEN 'P' THEN 'Preventive' ELSE 'Curative' END AS ServType,CASE ServLevel WHEN 'S' THEN 'Simple Service' WHEN 'V' THEN 'Visit' WHEN 'D' THEN 'Day of stay' ELSE 'Hospital case' END AS ServLevel ,ServPrice, ServCareType, ServFrequency, ServPatCat,validityfrom,validityto FROM tblServices order by ServCode", CommandType.Text)
        Else
            data.setSQLCommand("SELECT ServiceID,ServiceUUID,ServCode,ServName,CASE ServType WHEN 'P' THEN 'Preventive' ELSE 'Curative' END AS ServType, CASE ServLevel WHEN 'S' THEN 'Simple Service' WHEN 'V' THEN 'Visit' WHEN 'D' THEN 'Day of stay' ELSE 'Hospital case' END AS ServLevel ,ServPrice,ServCareType, ServFrequency, ServPatCat,validityfrom,validityto FROM tblServices where ValidityTo is NULL order by ServCode", CommandType.Text)
        End If
        Return data.Filldata
    End Function
    Public Function GetMS(ByVal eService As IMIS_EN.tblServices, dtSType As DataTable, dtServType As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " select ServiceID,ServiceUUID,ServCode,ServName,dt.Name ServType ,SL.Name ServLevel,ServPrice,validityfrom,validityto"
        sSQL += " from tblServices S"
        sSQL += " LEFT OUTER JOIN @dtIType dt ON dt.Code = S.ServType"
        sSQL += " LEFT OUTER JOIN @dtServLevel Sl ON Sl.Code = S.ServLevel"
        sSQL += " where ServCode LIKE @ServiceCode"
        sSQL += " AND ServName LIKE @ServiceName"
        sSQL += " AND ServType LIKE @ServiceType"
        sSQL += " AND ValidityTo is NULL"
        sSQL += " order by ServCode,ValidityTo"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ServiceCode", SqlDbType.NVarChar, 25, eService.ServCode)
        data.params("@ServiceName", SqlDbType.NVarChar, 100, eService.ServName)
        data.params("@ServiceType", SqlDbType.NVarChar, 1, eService.ServType)
        data.params("@dtIType", dtSType, "xCareType")
        data.params("@dtServLevel", dtServType, "xCareType")


        Return data.Filldata
    End Function
    'Corrected By Rogers
    Public Function GetMSLegacy(ByVal eService As IMIS_EN.tblServices, dtSType As DataTable, dtServType As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        'data.setSQLCommand("select ServiceID,ServCode,ServName,CASE ServType WHEN 'P' THEN 'Preventive' ELSE 'Curative' END AS ServType ,CASE ServLevel WHEN 'S' THEN 'Simple Service' WHEN 'V' THEN 'Visit' WHEN 'D' THEN 'Day Of Stay' ELSE 'Hospital Case' END as ServLevel,ServPrice,validityfrom,validityto from tblServices where ServCode LIKE @ServiceCode AND ServName LIKE @ServiceName AND ServType LIKE @ServiceType order by ServCode,ValidityTo", CommandType.Text)
        sSQL += " SELECT ServiceID,ServiceUUID,ServCode,ServName,dt.Name  ServType,SL.Name ServLevel,ServPrice,validityfrom,validityto"
        sSQL += " FROM tblServices S"
        sSQL += " LEFT OUTER JOIN @dtIType DT ON dt.Code = S.ServType"
        sSQL += " LEFT OUTER JOIN @dtServLevel SL ON Sl.Code = S.ServLevel"
        sSQL += " WHERE ServCode LIKE @ServiceCode AND ServName LIKE @ServiceName AND ServType LIKE @ServiceType order by ServCode,ValidityTo"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ServiceCode", SqlDbType.NVarChar, 25, eService.ServCode)
        data.params("@ServiceName", SqlDbType.NVarChar, 100, eService.ServName)
        data.params("@ServiceType", SqlDbType.NVarChar, 1, eService.ServType)
        data.params("@dtIType", dtSType, "xCareType")
        data.params("@dtServLevel", dtServType, "xCareType")
        Return data.Filldata
    End Function
    Public Sub InsertMedicalServices(ByRef eServices As IMIS_EN.tblServices)
        Dim data As New ExactSQL
        Dim sSQL As String = "INSERT INTO tblServices(ServCode, ServName, ServType, ServLevel, ServPrice, ServCareType, ServFrequency, ServPatCat,AuditUserID,ServCategory)" & _
                " VALUES (@ServCode, @ServName, @ServType, @ServLevel, @ServPrice, @ServCareType, @ServFrequency, @ServPatCat,@AuditUserID,@ServCategory)"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ServCode", SqlDbType.NVarChar, 25, eServices.ServCode)
        data.params("@ServName", SqlDbType.NVarChar, 100, eServices.ServName)
        data.params("@ServType", SqlDbType.Char, 1, eServices.ServType)
        data.params("@ServLevel", SqlDbType.Char, 1, eServices.ServLevel)
        data.params("@ServPrice", SqlDbType.Decimal, eServices.ServPrice)
        data.params("@ServCareType", SqlDbType.Char, 1, eServices.ServCareType)
        data.params("@ServFrequency", SqlDbType.SmallInt, eServices.ServFrequency)
        data.params("@ServPatCat", SqlDbType.TinyInt, eServices.ServPatCat)
        data.params("@AuditUserID", SqlDbType.Int, eServices.AuditUserID)
        data.params("@ServCategory", SqlDbType.Char, 1, eServices.ServCategory)
        data.ExecuteCommand()
    End Sub
    Public Sub UpdateMedicalServices(ByRef eServices As IMIS_EN.tblServices)
        Dim data As New ExactSQL
        data.setSQLCommand("INSERT INTO tblServices ([ServCode],[ServName],[ServType],[ServLevel],[ServPrice],[ServCareType],[ServFrequency],[ServPatCat],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID],ServCategory)" _
      & " select [ServCode],[ServName],[ServType],[ServLevel],[ServPrice],[ServCareType],[ServFrequency],[ServPatCat],[ValidityFrom],getdate(),@ServiceID,[AuditUserID],ServCategory from tblServices where ServiceID = @ServiceID;" _
      & "UPDATE [tblServices] SET [ServCode] = @ServCode,[ServName] = @ServName,[ServType] = @ServType,[ServLevel] = @ServLevel,[ServPrice] = @ServPrice,[ServCareType] = @ServCareType,[ServFrequency] = @ServFrequency, [ServPatCat] = @ServPatCat" _
      & ",[ValidityFrom] = GetDate(),[LegacyID] = @LegacyID,[AuditUserID] = @AuditUserID,ServCategory = @ServCategory  WHERE ServiceID = @ServiceID", CommandType.Text)
        data.params("@ServiceID", SqlDbType.Int, eServices.ServiceID)
        data.params("@ServCode", SqlDbType.NVarChar, 25, eServices.ServCode)
        data.params("@ServName", SqlDbType.NVarChar, 100, eServices.ServName)
        data.params("@ServType", SqlDbType.Char, 1, eServices.ServType)
        data.params("@ServLevel", SqlDbType.Char, 1, eServices.ServLevel)
        data.params("@ServPrice", SqlDbType.Decimal, eServices.ServPrice)
        data.params("@ServCareType", SqlDbType.Char, 1, eServices.ServCareType)
        data.params("@ServFrequency", SqlDbType.SmallInt, eServices.ServFrequency)
        data.params("@ServPatCat", SqlDbType.TinyInt, eServices.ServPatCat)
        data.params("@LegacyID", SqlDbType.Int, 1, ParameterDirection.Output)
        data.params("@AuditUserID", SqlDbType.Int, eServices.AuditUserID)
        data.params("@ServCategory", SqlDbType.Char, 1, eServices.ServCategory)
        data.ExecuteCommand()
    End Sub
    Public Function CheckIfServiceExists(ByVal eService As IMIS_EN.tblServices) As DataTable
        Dim data As New ExactSQL
        Dim str As String = "Select top 1 tblservices.ServiceID from tblServices where ServCode = @ServCode AND ValidityTo is null"
        If Not eService.ServiceID = 0 Then
            str += " AND tblServices.ServiceID <> @ServiceID"
        End If
        data.setSQLCommand(str, CommandType.Text)
        data.params("@ServCode", SqlDbType.NVarChar, 6, eService.ServCode)
        data.params("@ServiceID", SqlDbType.Int, eService.ServiceID)

        Return data.Filldata()
    End Function
    'Corrected by Rogers
    Public Function GetMedicalServices(ByVal PLServiceID As Integer, dtSType As DataTable, dtServType As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        'data.setSQLCommand("select PLServiceDetailID,tblServices.ServiceID ,cast (case when tblPLServicesDetail.ServiceID is null then 0 else 1 end as bit)  checked ,ServName,ServCode,CASE ServType WHEN 'P' THEN 'Preventive' ELSE 'Curative' END AS ServType,CASE ServLevel WHEN 'S' THEN 'Simple Service' WHEN 'V' THEN 'Visit' WHEN 'D' THEN 'Day of stay' ELSE 'Hospital case' END AS ServLevel ,ServPrice,tblPLServicesDetail.PriceOverule from tblServices left join (select * from tblPLServicesDetail where tblPLServicesDetail.plserviceid =@PLServiceID and tblPLServicesDetail.ValidityTo is null) tblPLServicesDetail on tblServices.ServiceID = tblPLServicesDetail.ServiceID " & _
        '                   " where tblServices.ValidityTo is null Order by case when tblPLServicesDetail.ServiceID is null then 0 else 1 end DESC,ServCode", CommandType.Text)
        sSQL = " SELECT PLServiceDetailID,S.ServiceID ,cast(case when tblPLServicesDetail.ServiceID IS NULL THEN 0 ELSE 1 END AS BIT)  checked ,ServName,ServCode,"
        sSQL += " dt.Name ServType, SL.Name ServLevel,ServPrice,tblPLServicesDetail.PriceOverule"
        sSQL += " FROM tblServices S"
        sSQL += " LEFT JOIN (SELECT * FROM tblPLServicesDetail WHERE tblPLServicesDetail.plserviceid =@PLServiceID AND tblPLServicesDetail.ValidityTo IS NULL) tblPLServicesDetail ON S.ServiceID = tblPLServicesDetail.ServiceID"
        sSQL += " LEFT OUTER JOIN @dtIType DT ON dt.Code = S.ServType"
        sSQL += " LEFT OUTER JOIN @dtServLevel SL ON Sl.Code = S.ServLevel"
        sSQL += " WHERE S.ValidityTo IS NULL ORDER BY CASE WHEn tblPLServicesDetail.ServiceID is null THEN 0 ELSE 1 END DESC,ServCode"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PLServiceID", SqlDbType.Int, PLServiceID)
        data.params("@dtIType", dtSType, "xCareType")
        data.params("@dtServLevel", dtServType, "xCareType")
        Return data.Filldata
    End Function
    Public Function CheckIfDelete(ByVal eServices As IMIS_EN.tblServices) As DataTable
        Dim data As New ExactSQL
        Dim str As String = "select top 1 tblservices.ServiceID from tblServices left join tblProductServices on tblservices.ServiceID = tblProductServices.ServiceID and tblProductServices.ValidityTo is null" & _
                          " left join tblPLServicesDetail on tblPLServicesDetail.ServiceID = tblservices.ServiceID  and tblPLServicesDetail.ValidityTo is null" & _
                          " where (tblProductServices.ServiceID = @ServiceID or tblPLServicesDetail.ServiceID = @ServiceID)"
        data.setSQLCommand(str, CommandType.Text)
        data.params("@ServiceID", SqlDbType.Int, eServices.ServiceID)
        Return data.Filldata
    End Function
    Public Function GetServiceIdByUUID(ByVal uuid As Guid) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select ServiceID from tblServices where ServiceUUID = @ServiceUUID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ServiceUUID", SqlDbType.UniqueIdentifier, uuid)

        Return data.Filldata
    End Function
End Class
