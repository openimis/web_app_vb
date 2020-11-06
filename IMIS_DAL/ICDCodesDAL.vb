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

Public Class ICDCodesDAL
    Dim data As New ExactSQL


    Public Function getICDCodes() As DataTable
        Dim str As String
        str = "SELECT ICDID,ICDCode,ICDName, ICDCode+ ' ' + ICDName ICDNames FROM tblICDCodes WHERE ValidityTo IS NULL"

        data.setSQLCommand(str, CommandType.Text)

        Return data.Filldata
    End Function
    Public Function getAutoCompleteICDCodes() As DataTable
        Dim str As String
        str = "SELECT DISTINCT ICDID, ISNULL(ICDCode,'')+ ' ' + ISNULL(ICDName,'') ICDNames FROM tblICDCodes WHERE ValidityTo IS NULL"

        data.setSQLCommand(str, CommandType.Text)

        Return data.Filldata
    End Function
    'Public Function getICDIDFromCode(ByVal ICDCODE As String) As Integer
    '    Dim str As String
    '    str = "SELECT @ICDID = ICDID FROM tblICDCodes where icdcode = @ICDCode"

    '    data.setSQLCommand(str, CommandType.Text)
    '    data.params("@ICDCode", SqlDbType.Int, ICDCODE, ParameterDirection.Input)
    '    data.params("@ICDID", SqlDbType.Int, 0, ParameterDirection.Output)
    '    data.ExecuteScalar()
    '    str = data.sqlParameters("@ICDID").ToString

    '    Return if(IsNumeric(str), str, 0)
    'End Function
    Public Function DownLoadDiagnosisXML() As DataTable
        Dim sSQL As String
        sSQL = "SELECT ICDCode DiagnosisCode,ICDName DiagnosisName  FROM tblICDCodes WHERE ValidityTo IS NULL"
        sSQL += " FOR XML PATH('Diagnosis'),ROOT('Diagnoses'),TYPE"
        data.setSQLCommand(sSQL, CommandType.Text)
        Return data.Filldata
    End Function

    Public Function UploadICDXML(ByVal Xml As System.Xml.XmlDocument, ByVal StrategyId As Integer, ByVal AuditUserID As Integer, ByRef dtResult As DataTable, Optional ByVal dryRun As Boolean = 0) As Dictionary(Of String, Integer)
        Dim data As New ExactSQL
        Dim sSQL As String = "uspUploadDiagnosisXML"

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@XML", Xml.InnerXml)
        data.params("@StrategyId", StrategyId)
        data.params("@AuditUserID", AuditUserID)
        data.params("@DryRun", dryRun)

        data.params("@DiagnosisSent", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@Inserts", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@Updates", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@Deletes", SqlDbType.Int, Nothing, ParameterDirection.Output)
        data.params("@returnValue", SqlDbType.Int, Nothing, ParameterDirection.ReturnValue)
        dtResult = data.Filldata()


        Dim Output As New Dictionary(Of String, Integer)
        Output.Add("DiagnosisSent", 0)
        Output.Add("Inserts", 0)
        Output.Add("Updates", 0)
        Output.Add("Deletes", 0)
        Output.Add("returnValue", 0)

        Output("DiagnosisSent") = If(data.sqlParameters("@DiagnosisSent") Is DBNull.Value, 0, data.sqlParameters("@DiagnosisSent"))
        Output("Inserts") = If(data.sqlParameters("@Inserts") Is DBNull.Value, 0, data.sqlParameters("@Inserts"))
        Output("Updates") = If(data.sqlParameters("@Updates") Is DBNull.Value, 0, data.sqlParameters("@Updates"))
        Output("Deletes") = If(data.sqlParameters("@Deletes") Is DBNull.Value, 0, data.sqlParameters("@Deletes"))

        Output("returnValue") = data.sqlParameters("@returnValue")
        Return Output
    End Function

End Class
