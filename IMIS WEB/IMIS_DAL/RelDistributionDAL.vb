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

Public Class RelDistributionDAL
    Private data As New ExactSQL
    Public Function GetDistribution(ByVal ProdId As Integer, ByVal DistrCareType As String, ByVal DistrType As Integer) As DataTable
        data.setSQLCommand("select Distrid,DistrCareType,period,Prodid,DistrPerc from tblrelDistr WHERE ProdID = @ProdID AND DistrCareType = @DistrCareType AND DistrType = @DistrType AND validityto is null order by Period", CommandType.Text)
        data.params("@ProdID", SqlDbType.NVarChar, ProdId)
        data.params("@DistrCareType", SqlDbType.NVarChar, 1, DistrCareType)
        data.params("@DistrType", SqlDbType.TinyInt, DistrType)
        Return (data.Filldata)
    End Function

    Function GetAllDistributionData(ByVal eRelDistr As IMIS_EN.tblRelDistr) As DataTable
        Dim str As String = "SELECT *,'o' AS RowV FROM tblRelDistr WHERE [ProdID]=@ProdID AND [ValidityTo] IS NULL ORDER BY DistrType,Period"
        data.setSQLCommand(str, CommandType.Text)
        data.params("@ProdID", SqlDbType.Int, eRelDistr.tblProduct.ProdID)
        Return data.Filldata()
    End Function




    Function InsertRelDistributionRecord(ByVal eRelDistr As IMIS_EN.tblRelDistr) As Boolean
        Dim str As String = "INSERT INTO tblRelDistr( [DistrType],[DistrCareType],[ProdID],[Period],[DistrPerc],[AuditUserID])" & _
                            " VALUES(@DistrType,@DistrCareType,@ProdID,@Period,@DistrPerc,@AuditUserID)"

        data.setSQLCommand(str, CommandType.Text)

        data.params("@DistrType", SqlDbType.TinyInt, eRelDistr.DistrType)
        data.params("@DistrCareType", SqlDbType.Char, 1, eRelDistr.DistrCareType)
        data.params("@ProdID", SqlDbType.Int, eRelDistr.tblProduct.ProdID)
        data.params("@Period", SqlDbType.TinyInt, eRelDistr.Period)
        data.params("@DistrPerc", SqlDbType.Decimal, eRelDistr.DistrPerc)
        data.params("@AuditUserID", SqlDbType.Int, eRelDistr.AuditUserID)

        Return data.ExecuteCommand()
    End Function

    Function UpdateRelDistributionRecord(ByVal eRelDistr As IMIS_EN.tblRelDistr) As Boolean
        Dim str As String = "INSERT INTO tblRelDistr( [DistrType],[DistrCareType],[ProdID],[Period],[DistrPerc],[ValidityFrom],[ValidityTo],[LegacyID],[AuditUserID] )" & _
                            " SELECT [DistrType],[DistrCareType],[ProdID],[Period],[DistrPerc],[ValidityFrom],GETDATE(),[DistrID],[AuditUserID] FROM tblRelDistr WHERE [DistrID] = @DistrID ;" & _
                            " UPDATE tblRelDistr SET [DistrPerc]=@DistrPerc,[ValidityFrom]=GETDATE(),[AuditUserID]=@AuditUserID WHERE [DistrID] = @DistrID"

        data.setSQLCommand(str, CommandType.Text)

        data.params("@DistrID", SqlDbType.Int, eRelDistr.DistrID)
        data.params("@DistrPerc", SqlDbType.Decimal, eRelDistr.DistrPerc)
        'data.params("@ValidityFrom", SqlDbType.DateTime, eRelDistr.ValidityFrom)
        data.params("@AuditUserID", SqlDbType.Int, eRelDistr.AuditUserID)

        Return data.ExecuteCommand()
    End Function

End Class
