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

Public Class RelIndexDAL
    Private data As New ExactSQL
    'Corrected By Rogers
    Public Function GetRelIndexes(ByVal eRelIndex As IMIS_EN.tblRelIndex, ByVal process As Boolean, ByVal RelCareTypedt As DataTable) As DataTable
        Dim sSQL As String = ""

        sSQL += " SELECT R.RelYear,CASE RelType WHEN 12 THEN DATENAME(MONTH,DATEADD(MONTH, R.RelPeriod  - 1, 0)) WHEN 4"
        sSQL += " THEN 'Quarter ' + CAST(R.RelPeriod AS VARCHAR(2))ELSE 'Year' END RelPeriod, P.ProductName,"
        sSQL += " RelCareTypeSt.Name AS RelCareType,R.CalcDate,R.RelIndex * 100 RelIndex FROM tblRelIndex R"
        sSQL += " INNER JOIN @RelCareTypeSt RelCareTypeSt on RelCareTypeSt.Code = R.RelCareType"
        sSQL += " INNER JOIN tblProduct P ON R.ProdID = P.ProdID WHERE R.ValidityTo Is NULL And P.ValidityTo Is NULL AND"
        sSQL += " CASE @RelType WHEN 0 THEN 0 ELSE R.RelType END  = @RelType AND"
        sSQL += " CASE @LocationId WHEN 0 THEN 0 ELSE ISNULL(R.LocationId,'') END  = ISNULL(@LocationId,'') AND"
        sSQL += " CASE @RelYear WHEN 0 THEN 0 ELSE R.RelYear END = @RelYear AND"
        sSQL += " CASE @RelPeriod WHEN 0 THEN 0 ELSE R.RelPeriod END = @RelPeriod AND"
        sSQL += " CASE @ProductID WHEN 0 THEN 0 ELSE P.ProdID END = @ProductID"

        If Not eRelIndex.RelCareType = Nothing Then
            sSQL += " AND RelCareType = @RelCareType"
        End If
        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@RelType", eRelIndex.RelType)
        data.params("@LocationId", If(eRelIndex.tblProduct.tblLocation.LocationId = -1, DBNull.Value, eRelIndex.tblProduct.tblLocation.LocationId))
        data.params("@ProductID", SqlDbType.Int, eRelIndex.tblProduct.ProdID)
        data.params("@RelCareType", SqlDbType.Char, 1, eRelIndex.RelCareType)
        data.params("@RelCareTypeSt", RelCareTypedt, "xCareType")

        ' End If



        data.params("@RelPeriod", SqlDbType.TinyInt, eRelIndex.RelPeriod)
        data.params("@RelYear", SqlDbType.Int, eRelIndex.RelYear)

        Return data.Filldata
    End Function


    Public Function ProcessBatch(ByVal eRelIndex As IMIS_EN.tblRelIndex) As Integer
        Dim data As New ExactSQL
        Dim sSQL As String = "uspBatchProcess"

        With data
            .setSQLCommand(sSQL, CommandType.StoredProcedure, timeout:=0)

            .params("AuditUser", SqlDbType.Int, eRelIndex.AuditUserID)
            .params("LocationId", SqlDbType.Int, eRelIndex.tblProduct.tblLocation.LocationId)
            .params("Year", SqlDbType.Int, eRelIndex.RelYear)
            .params("Period", SqlDbType.Int, eRelIndex.RelPeriod)
            .params("ReturnValue", 0, 0, 0, ParameterDirection.ReturnValue)

            .ExecuteCommand()

            Return .sqlParameters("ReturnValue")
        End With

    End Function

  
End Class
