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

Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
Public Class GeneralBL


    Public Function getMessage(ByVal MessageID As String) As String
        Return System.Web.HttpContext.GetGlobalResourceObject("Resource", MessageID)
    End Function

    Public Function GetMaritalStatus() As DataTable

        Dim dtbl As New DataTable
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("Status")
        Dim dr As DataRow = dtbl.NewRow
        dr("Code") = ""
        dr("Status") = getMessage("T_SELECTSTATUS")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("Code") = "M"
        dr("Status") = getMessage("T_MARRIED")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("Code") = "S"
        dr("Status") = getMessage("T_SINGLE")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("Code") = "D"
        dr("Status") = getMessage("T_DIVORCED")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("Code") = "W"
        dr("Status") = getMessage("T_WIDOWED")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("Code") = "N"
        dr("Status") = getMessage("T_NOTSPECIFIED")
        dtbl.Rows.Add(dr)

        Return dtbl
    End Function
    Public Function GetYesNo() As DataTable

        Dim dtbl As New DataTable
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("Status")
        Dim dr As DataRow = dtbl.NewRow
        dr("Code") = ""
        dr("Status") = getMessage("T_SELECTYESNO")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("Code") = "1"
        dr("Status") = getMessage("T_YES")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("Code") = "0"
        dr("Status") = getMessage("T_NO")
        dtbl.Rows.Add(dr)
        Return dtbl
    End Function
    Public Function GetYesN(Optional ByVal showSelect As Boolean = False) As DataTable

        Dim dtbl As New DataTable
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("Status")
        Dim dr As DataRow

        If showSelect = True Then
            dr = dtbl.NewRow
            dr("Code") = ""
            dr("Status") = getMessage("T_SELECTYESNO")
            dtbl.Rows.Add(dr)
        End If
        
        dr = dtbl.NewRow
        dr("Code") = 1
        dr("Status") = getMessage("T_YES")
        dtbl.Rows.Add(dr)
        dr = dtbl.NewRow
        dr("Code") = 0
        dr("Status") = getMessage("T_NO")
        dtbl.Rows.Add(dr)
        Return dtbl
    End Function
     Public Function GetLanguage() As DataTable
        Dim DAL As New IMIS_DAL.LanguagesDAL
        Dim dt As DataTable = DAL.GetLanguages
        Dim dr As DataRow = dt.NewRow
        dr("LanguageCode") = -1
        dr("LanguageName") = getMessage("T_SELECTLANGUAGE")
        dt.Rows.InsertAt(dr, 0)
        Return dt
    End Function
    Public Function GetPatient(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim dtbl As New DataTable
        dtbl.Columns.Add("ID")
        dtbl.Columns.Add("Patient")
        Dim dr As DataRow
        If showSelect = True Then
            dr = dtbl.NewRow
            dr("ID") = "0"
            dr("Patient") = getMessage("T_SELECTPATIENT")
            dtbl.Rows.Add(dr)
        End If

        dr = dtbl.NewRow
        dr("ID") = "1"
        dr("Patient") = getMessage("T_MAN")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("ID") = "2"
        dr("Patient") = getMessage("T_WOMAN")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("ID") = "3"
        dr("Patient") = getMessage("T_ADULT")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("ID") = "4"
        dr("Patient") = getMessage("T_CHILD")
        dtbl.Rows.Add(dr)

        Return dtbl

    End Function

    'Public Function GetPayerType(Optional ByVal showSelect As Boolean = False) As DataTable
    '    Dim dtbl As New DataTable
    '    Dim dr As DataRow
    '    dtbl.Columns.Add("Code")
    '    dtbl.Columns.Add("PayerType")
    '    If showSelect = True Then
    '        dr = dtbl.NewRow
    '        dr("Code") = ""
    '        dr("PayerType") = getMessage("T_SELECTPAYERTYPE")
    '        dtbl.Rows.Add(dr)
    '    End If
    '    dr = dtbl.NewRow
    '    dr("Code") = "G"
    '    dr("PayerType") = getMessage("T_GOVERNMENT")
    '    dtbl.Rows.Add(dr)

    '    dr = dtbl.NewRow
    '    dr("Code") = "L"
    '    dr("PayerType") = getMessage("T_LOCALAUTHORITY")
    '    dtbl.Rows.Add(dr)

    '    dr = dtbl.NewRow
    '    dr("Code") = "C"
    '    dr("PayerType") = getMessage("T_COOPERATIVE")
    '    dtbl.Rows.Add(dr)

    '    dr = dtbl.NewRow
    '    dr("Code") = "P"
    '    dr("PayerType") = getMessage("T_PRIVATEORGANIZATION")
    '    dtbl.Rows.Add(dr)

    '    dr = dtbl.NewRow
    '    dr("Code") = "D"
    '    dr("PayerType") = getMessage("T_DONOR")
    '    dtbl.Rows.Add(dr)

    '    dr = dtbl.NewRow
    '    dr("Code") = "O"
    '    dr("PayerType") = getMessage("T_OTHER")
    '    dtbl.Rows.Add(dr)

    '    Return dtbl
    'End Function

    'Public Function GetNumberTypeStatus(ByVal min As Integer, ByVal max As Integer) As DataTable

    '    Dim dtbl As New DataTable
    '    dtbl.Columns.Add("value")
    '    dtbl.Columns.Add("name")
    '    Dim dr As DataRow
    '    dr = dtbl.NewRow
    '    dr("value") = 0
    '    dr("name") = getMessage("T_STATUS")
    '    dtbl.Rows.Add(dr)
    '    If min = 0 Then
    '        min = 1
    '    End If
    '    For i As Integer = min To max

    '        If min > max Then
    '            Exit For
    '        End If

    '        dr = dtbl.NewRow
    '        dr = dtbl.NewRow
    '        dr("value") = i
    '        dr("name") = min
    '        min += 1
    '        dtbl.Rows.Add(dr)
    '    Next
    '    Return dtbl
    'End Function

    'Public Function GetReviewStatus(Optional ByVal showSelect As Boolean = False) As DataTable
    '    Dim dtbl As New DataTable
    '    Dim dr As DataRow
    '    dtbl.Columns.Add("Code")
    '    dtbl.Columns.Add("Status")
    '    If showSelect = True Then
    '        dr = dtbl.NewRow
    '        dr("Code") = 7
    '        dr("Status") = getMessage("T_SELECTSTATUS")
    '        dtbl.Rows.Add(dr)
    '    End If
    '    dr = dtbl.NewRow
    '    dr("Code") = 0
    '    dr("Status") = getMessage("T_IDLE")
    '    dtbl.Rows.Add(dr)

    '    dr = dtbl.NewRow
    '    dr("Code") = 1
    '    dr("Status") = getMessage("T_NOTSELECTED")
    '    dtbl.Rows.Add(dr)

    '    dr = dtbl.NewRow
    '    dr("Code") = 2
    '    dr("Status") = getMessage("T_SELECTED")
    '    dtbl.Rows.Add(dr)

    '    dr = dtbl.NewRow
    '    dr("Code") = 4
    '    dr("Status") = getMessage("T_REVIEWED")
    '    dtbl.Rows.Add(dr)

    '    dr = dtbl.NewRow
    '    dr("Code") = 8
    '    dr("Status") = getMessage("T_BYPASSED")
    '    dtbl.Rows.Add(dr)


    '    Return dtbl
    'End Function

    Public Function GetItemServiceStatus(Optional ByVal showSelect As Boolean = False) As DataTable
        Dim dtbl As New DataTable
        Dim dr As DataRow
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("Status")
        If showSelect = True Then
            dr = dtbl.NewRow
            dr("Code") = 3
            dr("Status") = getMessage("T_SELECTSTATUS")
            dtbl.Rows.Add(dr)
        End If
        dr = dtbl.NewRow
        dr("Code") = 1
        dr("Status") = getMessage("T_PASSED")
        dtbl.Rows.Add(dr)

        dr = dtbl.NewRow
        dr("Code") = 2
        dr("Status") = getMessage("T_REJECTED")
        dtbl.Rows.Add(dr)

        Return dtbl
    End Function

    Public Function GetMonths(ByVal start As Integer, ByVal ending As Integer) As DataTable
        Dim dtmonth As New DataTable
        Dim gen As New GeneralBL
        dtmonth.Columns.Add("MonthNum")
        dtmonth.Columns.Add("MonthName")
        Dim month() As String = {gen.getMessage("T_JANUARY"), gen.getMessage("T_FEBRUARY"), gen.getMessage("T_MARCH"), gen.getMessage("T_APRIL"), gen.getMessage("T_MAY"), gen.getMessage("T_JUNE"), gen.getMessage("T_JULY"), gen.getMessage("T_AUGUST"), gen.getMessage("T_SEPTEMBER"), gen.getMessage("T_OCTOBER"), gen.getMessage("T_NOVEMBER"), gen.getMessage("T_DECEMBER")}
        Dim dr As DataRow

        dr = dtmonth.NewRow
        dr("monthNum") = 0
        dr("monthName") = getMessage("T_MONTH")
        dtmonth.Rows.Add(dr)


        If ending > 12 Then
            ending = 12
        End If

        If start < 0 Then
            start = 1
        End If


        For i As Integer = start To ending
            dr = dtmonth.NewRow
            dr("MonthNum") = i
            dr("MonthName") = month(i - 1)
            dtmonth.Rows.Add(dr)
        Next

        Return dtmonth
    End Function

    Public Function GetYears(ByVal start As Integer, ByVal ending As Integer) As DataTable
        Dim dtYear As New DataTable
        dtYear.Columns.Add("YearId", GetType(Integer))
        dtYear.Columns.Add("Year")

        Dim dr As DataRow

        dr = dtYear.NewRow

        dr("YearId") = 0
        dr("Year") = getMessage("T_YEAR")
        dtYear.Rows.Add(dr)

        For i As Integer = start To ending
            dr = dtYear.NewRow
            dr("YearId") = i
            dr("Year") = i
            dtYear.Rows.Add(dr)
        Next

        Return dtYear
    End Function
    Public Function Decrypt(ByVal key As String, ByVal inFile As String) As String
        Dim DESalg As New DESCryptoServiceProvider
        Dim fs As New FileStream(inFile, FileMode.Open)
        Dim objEncod As Encoding = Encoding.ASCII
        DESalg.IV = objEncod.GetBytes("11110000")

        DESalg.Key = objEncod.GetBytes(key)
        Dim cryp As New CryptoStream(fs, DESalg.CreateDecryptor(DESalg.Key, DESalg.IV), CryptoStreamMode.Read)
        Dim fileInf As New IO.FileInfo(inFile)
        Dim strReader As New StreamReader(cryp)

        Dim str As String = strReader.ReadToEnd
        strReader.Close()
        cryp.Close()
        fs.Close()

        Return str
    End Function
    Public Function GetSMSStatus() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("Value")
        dt.Columns.Add("Status")
        Dim dr As DataRow
        dr = dt.NewRow
        dr("Value") = -1
        dr("Status") = getMessage("T_SELECTSTATUS")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("Value") = 0
        dr("Status") = getMessage("T_RECEIVED")
        dt.Rows.Add(dr)

        dr = dt.NewRow
        dr("Value") = 1
        dr("Status") = getMessage("T_FAILED")
        dt.Rows.Add(dr)
        Return dt
    End Function
    Public Function GetPeriodNo(ByVal Type As Char) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("Period")
        dt.Columns.Add("Value")
        'Dim count As Integer
        Dim dr As DataRow
        'If Type = "M" Then
        '    count = 12
        'ElseIf Type = "Q" Then
        '    count = 4
        'ElseIf Type = "Y" Then
        '    count = 1
        'End If
        'For i As Integer = 1 To count
        '    dr = dt.NewRow
        '    dr("Period") = i
        '    dr("Value") = i
        '    dt.Rows.Add(dr)
        'Next

        If Type = "Y" Then
            dr = dt.NewRow
            dr("Period") = "--" & getMessage("L_PERIOD") & "--"
            dr("Value") = 0
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("L_YEAR")
            dr("Value") = 1
            dt.Rows.Add(dr)
        ElseIf Type = "Q" Then
            dr = dt.NewRow
            dr("Period") = "--" & getMessage("L_PERIOD") & "--"
            dr("Value") = 0
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("M_Q1")
            dr("Value") = 1
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("M_Q2")
            dr("Value") = 2
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("M_Q3")
            dr("Value") = 3
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("M_Q4")
            dr("Value") = 4
            dt.Rows.Add(dr)
        ElseIf Type = "M" Then
            dr = dt.NewRow
            dr("Period") = "--" & getMessage("L_PERIOD") & "--"
            dr("Value") = 0
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_JANUARY")
            dr("Value") = 1
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_FEBRUARY")
            dr("Value") = 2
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_MARCH")
            dr("Value") = 3
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_APRIL")
            dr("Value") = 4
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_MAY")
            dr("Value") = 5
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_JUNE")
            dr("Value") = 6
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_JULY")
            dr("Value") = 7
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_AUGUST")
            dr("Value") = 8
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_SEPTEMBER")
            dr("Value") = 9
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_OCTOBER")
            dr("Value") = 10
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_NOVEMBER")
            dr("Value") = 11
            dt.Rows.Add(dr)

            dr = dt.NewRow
            dr("Period") = getMessage("T_DECEMBER")
            dr("Value") = 12
            dt.Rows.Add(dr)
        End If
        Return dt
    End Function
    Public Function sendSMS(ByVal PhoneNumber As String, ByVal Message As String) As Boolean
        Dim WebRequest As Net.WebRequest 'object for WebRequest
        Dim WebResponse As Net.WebResponse 'object for WebResponse



        Dim serv As String = "http://smsplus3.routesms.com:8080/bulksms/bulksms?username=paul1&password=gd7heq&type=0&dlr=0&destination="
        Dim Numb As String = PhoneNumber
        Dim Mess As String = "&source=BEPHA%20IMIS&message="
        Mess += Message

        Dim URL As String = serv & Numb & Mess
        Dim WebResponseString As String = ""


        WebRequest = Net.HttpWebRequest.Create(URL) 'Hit URL Link
        WebRequest.Timeout = 25000
        Try
            WebResponse = WebRequest.GetResponse 'Get Response
            Dim reader As IO.StreamReader = New IO.StreamReader(WebResponse.GetResponseStream)
            'Read Response and store in variable
            WebResponseString = reader.ReadToEnd()
            WebResponse.Close()
            Return True
            ' Response.Write(WebResponseString) 'Display Response.
        Catch ex As Exception
            WebResponseString = "Request Timeout" 'If any exception occur.
            System.Web.HttpContext.Current.Response.Write(WebResponseString)
            Return False
        End Try
    End Function
    Public Function ReadSMSDatatable(ByVal dt As DataTable) As String


        Dim strReturn As String = ""
        For Each row As DataRow In dt.Rows
            Dim Num As String = row("PhoneNumber").ToString
            If Num.StartsWith("255") And Num.Length = 12 Then
                If sendSMS(Num, row("SMSMessage")) = True Then
                    strReturn += Num & " SMS sent OK" & "<br/>"
                Else
                    strReturn += Num & " SMS Failed" & "<br/>"
                End If
            Else
                strReturn += Num & "Invalid Phone Number" & "<br/>"
            End If
        Next
        If strReturn.Length = 0 Then Return "Nothing to send for selected period"
        Return strReturn
    End Function


    Public Function GetUploadStrategies(includeDelete As Boolean) As DataTable

        '1  :   Insert
        '2  :   Update
        '4  :   Delete

        Dim dtStrategy As New DataTable
        dtStrategy.Columns.Add("StrategyId", GetType(Integer))
        dtStrategy.Columns.Add("StrategyName", GetType(String))
        dtStrategy.Rows.Add(0, getMessage("L_SELECTSTRATEGY"))
        dtStrategy.Rows.Add(1, getMessage("L_INSERTONLY"))
        dtStrategy.Rows.Add(2, getMessage("L_UPDATEONLY"))
        dtStrategy.Rows.Add(3, getMessage("L_INSERTANDUPDATE"))
        If includeDelete Then dtStrategy.Rows.Add(7, getMessage("L_INSERTUPDATEDELETE"))
        Return dtStrategy
    End Function


    Public Function CreateUploadRegisterLog(dt As DataTable, registerType As String, StratergyId As Int16, UploadFileName As String, UserName As String, Optional regType As String = "") As String
        Dim path As String = String.Empty
        Dim fileName As String = String.Empty
        Dim strategyType As String = String.Empty

        Select Case StratergyId
            Case 1
                strategyType = "Insert Only"
            Case 2
                strategyType = "Update Only"
            Case 3
                strategyType = "Insert and Update"
            Case 7
                strategyType = "Insert, Update and Delete"
        End Select




        path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings("ExportLogFolder"))
        fileName = String.Format("{0}_{1}_{2}_{3}.txt", "UploadSummary", registerType, Format(DateTime.Now, "yyyyMMddHHmm"), System.IO.Path.GetFileNameWithoutExtension(UploadFileName))

        Dim dv As DataView = dt.DefaultView


        dv.RowFilter = "ResultType='IR'"
        Dim dtInsertedRegion As DataTable = dv.ToTable
        dv.RowFilter = "ResultType='UR'"
        Dim dtUpdatedRegion As DataTable = dv.ToTable
        dv.RowFilter = "ResultType='FR'"
        Dim dtFailedRegion As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='ID'"
        Dim dtInsertedDistrict As DataTable = dv.ToTable
        dv.RowFilter = "ResultType='UD'"
        Dim dtUpdatedDistrict As DataTable = dv.ToTable
        dv.RowFilter = "ResultType='FD'"
        Dim dtFailedDistrict As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='IM'"
        Dim dtInsertedMunicipality As DataTable = dv.ToTable
        dv.RowFilter = "ResultType='UM'"
        Dim dtUpdatedMunicipality As DataTable = dv.ToTable
        dv.RowFilter = "ResultType='FM'"
        Dim dtFaileddMunicipality As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='IV'"
        Dim dtInsertedVillage As DataTable = dv.ToTable
        dv.RowFilter = "ResultType='UV'"
        Dim dtUpdatedVillage As DataTable = dv.ToTable
        dv.RowFilter = "ResultType='FV'"
        Dim dtFailedVillage As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='I'"
        Dim dtInserted As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='U'"
        Dim dtUpdated As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='D'"
        Dim dtDeleted As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='FH' or ResultType='FI'"
        Dim dtFailed As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='IC'"
        Dim dtinsertedCatchment As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='UC'"
        Dim dtUpdatedCatchment As DataTable = dv.ToTable


        dv.RowFilter = "ResultType='E'"
        Dim dtError As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='C'"
        Dim dtConflict As DataTable = dv.ToTable

        dv.RowFilter = "ResultType='FE'"
        Dim dtFatalError As DataTable = dv.ToTable

        Dim fileContent As String = String.Empty

        fileContent = "File Name : " & UploadFileName & vbNewLine
        fileContent += "Register : " & registerType & vbNewLine
        fileContent += "User : " & UserName & vbNewLine
        fileContent += "Date : " & Format(DateTime.Now, "dd/MM/yyyy HH:mm") & vbNewLine
        fileContent += "Strategy : " & strategyType & vbNewLine & vbNewLine & vbNewLine

        If regType = "Locations" Then

            fileContent += dtInsertedRegion.Rows.Count.ToString() & " Region(s) Inserted" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtInsertedRegion.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtInsertedRegion.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine


            fileContent += dtUpdatedRegion.Rows.Count.ToString() & " Region(s) Updated" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtUpdatedRegion.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtUpdatedRegion.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine

            If (dtFailedRegion.Rows.Count > 0) Then
                fileContent += dtFailedRegion.Rows.Count.ToString() & " Region(s) Failed" & vbNewLine
                fileContent += "---------------------------------------------------------------------------------" & vbNewLine

                For i As Integer = 0 To dtFailedRegion.Rows.Count - 1
                    fileContent += i + 1 & "." & vbTab & dtFailedRegion.Rows(i)("Result").ToString() & vbNewLine
                Next
                fileContent += vbNewLine & vbNewLine
            End If

            fileContent += dtInsertedDistrict.Rows.Count.ToString() & " District(s) Inserted" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtInsertedDistrict.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtInsertedDistrict.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine

            fileContent += dtUpdatedDistrict.Rows.Count.ToString() & " District(s) Updated" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtUpdatedDistrict.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtUpdatedDistrict.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine

            If (dtFailedDistrict.Rows.Count > 0) Then
                fileContent += dtFailedDistrict.Rows.Count.ToString() & " District(s) Failed" & vbNewLine
                fileContent += "---------------------------------------------------------------------------------" & vbNewLine

                For i As Integer = 0 To dtFailedDistrict.Rows.Count - 1
                    fileContent += i + 1 & "." & vbTab & dtFailedDistrict.Rows(i)("Result").ToString() & vbNewLine
                Next
                fileContent += vbNewLine & vbNewLine
            End If

            fileContent += dtInsertedMunicipality.Rows.Count.ToString() & " Municipality(s) Inserted" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtInsertedMunicipality.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtInsertedMunicipality.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine

            fileContent += dtUpdatedMunicipality.Rows.Count.ToString() & " Municipality(s) Updated" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            If (dtFaileddMunicipality.Rows.Count > 0) Then
                fileContent += dtFaileddMunicipality.Rows.Count.ToString() & " Municipality(s) Failed" & vbNewLine
                fileContent += "---------------------------------------------------------------------------------" & vbNewLine

                For i As Integer = 0 To dtFaileddMunicipality.Rows.Count - 1
                    fileContent += i + 1 & "." & vbTab & dtFaileddMunicipality.Rows(i)("Result").ToString() & vbNewLine
                Next
                fileContent += vbNewLine & vbNewLine
            End If

            For i As Integer = 0 To dtUpdatedMunicipality.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtUpdatedMunicipality.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine

            fileContent += dtInsertedVillage.Rows.Count.ToString() & " Village(s) Inserted" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtInsertedVillage.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtInsertedVillage.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine

            fileContent += dtUpdatedVillage.Rows.Count.ToString() & " Village(s) Updated" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtUpdatedVillage.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtUpdatedVillage.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine

            If (dtFailedVillage.Rows.Count > 0) Then
                fileContent += dtFailedVillage.Rows.Count.ToString() & " Village(s) Failed" & vbNewLine
                fileContent += "---------------------------------------------------------------------------------" & vbNewLine

                For i As Integer = 0 To dtFailedVillage.Rows.Count - 1
                    fileContent += i + 1 & "." & vbTab & dtFailedVillage.Rows(i)("Result").ToString() & vbNewLine
                Next
                fileContent += vbNewLine & vbNewLine
            End If

        Else
            fileContent += dtInserted.Rows.Count.ToString() & " Inserted" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtInserted.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtInserted.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine



            fileContent += dtUpdated.Rows.Count.ToString() & " Updated" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtUpdated.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtUpdated.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine
        End If


        If regType = "Diagnosis" Then
            fileContent += dtDeleted.Rows.Count.ToString() & " Deleted" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtDeleted.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtDeleted.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine

        End If

        If regType = "HF" Then
            If dtinsertedCatchment.Rows.Count > 0 Then
                fileContent += dtinsertedCatchment.Rows(0)("Result").ToString() & " Catchment(s) inserted" & vbNewLine
                fileContent += vbNewLine
            End If

            If dtUpdatedCatchment.Rows.Count > 0 Then
                fileContent += dtUpdatedCatchment.Rows(0)("Result").ToString() & " Catchment(s) updated" & vbNewLine
                fileContent += vbNewLine
            End If

        End If


        If dtError.Rows.Count > 0 Then
            fileContent += dtError.Rows.Count.ToString() & " Error(s)" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtError.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtError.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine
        End If

        If dtConflict.Rows.Count > 0 Then
            fileContent += dtConflict.Rows.Count.ToString() & " Conflict(s)" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtConflict.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtConflict.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine
        End If

        If dtFailed.Rows.Count > 0 Then
            fileContent += dtFailed.Rows.Count.ToString() & " Failed" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtFailed.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtFailed.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine
        End If

        If dtFatalError.Rows.Count > 0 Then
            fileContent += dtFatalError.Rows.Count.ToString() & " Fatal Error" & vbNewLine
            fileContent += "---------------------------------------------------------------------------------" & vbNewLine

            For i As Integer = 0 To dtFatalError.Rows.Count - 1
                fileContent += i + 1 & "." & vbTab & dtFatalError.Rows(i)("Result").ToString() & vbNewLine
            Next
            fileContent += vbNewLine & vbNewLine
        End If

        My.Computer.FileSystem.WriteAllText(path & fileName, fileContent, False)



        Return path & fileName

    End Function

    Public Function getLoginName(ByVal session As Object) As String
        Try
            Dim dt As New DataTable
            dt = DirectCast(session, DataTable)
            Return dt.Rows(0)("LoginName")
        Catch
            Return ""
        End Try
    End Function
    Public Function PrivateKey(ByVal maxSize As Integer) As String
        Dim chars As Char() = New Char(61) {}
        chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray()
        Dim data As Byte() = New Byte(0) {}

        Using crypto As RNGCryptoServiceProvider = New RNGCryptoServiceProvider()
            crypto.GetNonZeroBytes(data)
            data = New Byte(maxSize - 1) {}
            crypto.GetNonZeroBytes(data)
        End Using

        Dim result As StringBuilder = New StringBuilder(maxSize)

        For Each b As Byte In data
            result.Append(chars(b Mod (chars.Length)))
        Next

        Return result.ToString()
    End Function
    Public Function GenerateSHA256String(ByVal inputString) As String
        Dim sha256 As SHA256 = SHA256Managed.Create()
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(inputString)
        Dim hash As Byte() = sha256.ComputeHash(bytes)
        Dim stringBuilder As New StringBuilder()

        For i As Integer = 0 To hash.Length - 1
            stringBuilder.Append(hash(i).ToString("X2"))
        Next

        Return stringBuilder.ToString()
    End Function
    Public Function GetMode() As DataTable
        Dim dtMode As New DataTable
        dtMode.Columns.Add("ModeID", GetType(Integer))
        dtMode.Columns.Add("ModeName", GetType(String))
        dtMode.Rows.Add(-1, getMessage("T_SELECTMODE"))
        dtMode.Rows.Add(0, getMessage("T_PRESCRIBEDCONTRIBUTIONS"))
        dtMode.Rows.Add(1, getMessage("T_ACTUALPAIDCONTRIBUTIONS"))
        Return dtMode
    End Function
    Public Function GetRejectedReasons(ByVal ReasonId As Integer) As String
        Select Case ReasonId
            Case -1 : Return getMessage("T_REJECTEDBYMEDICALOFFICER")
            Case 0 : Return getMessage("T_ACCEPTED")
            Case 1 : Return getMessage("T_ITEMSERVICENOTINTHEREGISTER")
            Case 2 : Return getMessage("T_ITEMSERVICENOTINTHEPRICELIST")
            Case 3 : Return getMessage("T_ITEMSERVICENOTCOVEREDBYPOLICY")
            Case 4 : Return getMessage("T_ITEMSERVICEDOESNTCOMPLYWITHLIMITATION")
            Case 5 : Return getMessage("T_ITEMSERVICEDOESNTCOMPLYWITHFREQUENCY")
            Case 6 : Return getMessage("T_ITEMSERVICEDUPLICATED")
            Case 7 : Return getMessage("T_ITEMSERVICENOTVALIDINSURANCENUMBER")
            Case 8 : Return getMessage("T_DIAGNOSISCODENOTINTHECURRENTLIST")
            Case 9 : Return getMessage("T_TARGETDATEOFPROVISIONHEALTHCAREINVALID")
            Case 10 : Return getMessage("T_ITEMSERVICEDOESNOTCOMPLYWITHCARECONSTRAINT")
            Case 11 : Return getMessage("T_MAXIMUMNUMBEROFINPATIENTEXCEEDED")
            Case 12 : Return getMessage("T_MAXIMUMNUMBEROFOUTPATIENTEXCEEDED")
            Case 13 : Return getMessage("T_MAXIMUMNUMBEROFCONSULTATIONSEXCEEDED")
            Case 14 : Return getMessage("T_MAXIMUMNUMBEROFSURGERIESEXCEEDED")
            Case 15 : Return getMessage("T_MAXIMUMNUMBEROFDELIVERIESEXCEEDED")
            Case 16 : Return getMessage("T_MAXIMUMNUMBEROFPROVISIONSEXCEEDED")
            Case 17 : Return getMessage("T_ITEMSERVICECANNOTBECOVEREDWITHWAITINGPERIOD")
            Case 18 : Return getMessage("T_NA")
            Case 19 : Return getMessage("T_MAXIMUMNUMBEROFANTENETALEXCEEDED")
            Case Else : Return ""
        End Select
    End Function

    Public Function GetAllRejectedReasons() As DataTable
        Dim dtRejReasons As New DataTable
        dtRejReasons.Columns.Add("ID", GetType(Integer))
        'Dim col = New DataColumn()
        'col.ColumnName = "Name"
        'col.DataType =GetType()
        dtRejReasons.Columns.Add("Name", GetType(String)).MaxLength = 100
        dtRejReasons.Rows.Add(-1, getMessage("T_REJECTEDBYMEDICALOFFICER"))
        dtRejReasons.Rows.Add(0, getMessage("T_ACCEPTED"))
        dtRejReasons.Rows.Add(1, getMessage("T_ITEMSERVICENOTINTHEREGISTER"))
        dtRejReasons.Rows.Add(2, getMessage("T_ITEMSERVICENOTINTHEPRICELIST"))
        dtRejReasons.Rows.Add(3, getMessage("T_ITEMSERVICENOTCOVEREDBYPOLICY"))
        dtRejReasons.Rows.Add(4, getMessage("T_ITEMSERVICEDOESNTCOMPLYWITHLIMITATION"))
        dtRejReasons.Rows.Add(5, getMessage("T_ITEMSERVICEDOESNTCOMPLYWITHFREQUENCY"))
        dtRejReasons.Rows.Add(6, getMessage("T_ITEMSERVICEDUPLICATED"))
        dtRejReasons.Rows.Add(7, getMessage("T_ITEMSERVICENOTVALIDINSURANCENUMBER"))
        dtRejReasons.Rows.Add(8, getMessage("T_DIAGNOSISCODENOTINTHECURRENTLIST"))
        dtRejReasons.Rows.Add(9, getMessage("T_TARGETDATEOFPROVISIONHEALTHCAREINVALID"))
        dtRejReasons.Rows.Add(10, getMessage("T_ITEMSERVICEDOESNOTCOMPLYWITHCARECONSTRAINT"))
        dtRejReasons.Rows.Add(11, getMessage("T_MAXIMUMNUMBEROFINPATIENTEXCEEDED"))
        dtRejReasons.Rows.Add(12, getMessage("T_MAXIMUMNUMBEROFOUTPATIENTEXCEEDED"))
        dtRejReasons.Rows.Add(13, getMessage("T_MAXIMUMNUMBEROFCONSULTATIONSEXCEEDED"))
        dtRejReasons.Rows.Add(14, getMessage("T_MAXIMUMNUMBEROFSURGERIESEXCEEDED"))
        dtRejReasons.Rows.Add(15, getMessage("T_MAXIMUMNUMBEROFDELIVERIESEXCEEDED"))
        dtRejReasons.Rows.Add(16, getMessage("T_MAXIMUMNUMBEROFPROVISIONSEXCEEDED"))
        dtRejReasons.Rows.Add(17, getMessage("T_ITEMSERVICECANNOTBECOVEREDWITHWAITINGPERIOD"))
        dtRejReasons.Rows.Add(18, getMessage("T_NA"))
        dtRejReasons.Rows.Add(19, getMessage("T_MAXIMUMNUMBEROFANTENETALEXCEEDED"))

        Return dtRejReasons
    End Function

    Public Function GetPaymentStatusNames() As DataTable

        Dim dtStatus As New DataTable
        dtStatus.Columns.Add("StatusID", GetType(Integer))
        dtStatus.Columns.Add("PaymenyStatusName", GetType(String))

        dtStatus.Rows.Add(-1, getMessage("T_REJECTEDPOSTED"))
        dtStatus.Rows.Add(-3, getMessage("T_REJECTEDPOSTED"))
        dtStatus.Rows.Add(-2, getMessage("T_REJECTEDPOSTED"))
        dtStatus.Rows.Add(2, getMessage("T_POSTED"))
        dtStatus.Rows.Add(1, getMessage("T_NOTYETCONFIRMED"))
        dtStatus.Rows.Add(3, getMessage("T_ASSIGNED"))
        dtStatus.Rows.Add(4, getMessage("T_UNMATCHED"))
        dtStatus.Rows.Add(5, getMessage("T_PAYMENTMATCHED"))

        Return dtStatus
    End Function

    Public Function GetPaymentStatus(Optional Include As Boolean = False, Optional ForReport As Boolean = False) As DataTable

        Dim dtStatus As New DataTable
        dtStatus.Columns.Add("StatusID", GetType(Integer))
        dtStatus.Columns.Add("PaymenyStatusName", GetType(String))
        If Include = True Then
            dtStatus.Rows.Add(-1, getMessage("L_SELECTPAYMENTSTATUS"))
        End If
        If Not ForReport Then
            If Include = False Then
                'dtStatus.Rows.Add(-1, getMessage("T_FAILEDSTATUS"))
                'dtStatus.Rows.Add(-2, getMessage("T_FAILEDSTATUS"))
                'dtStatus.Rows.Add(-3, getMessage("T_FAILEDSTATUS"))
                'dtStatus.Rows.Add(1, getMessage("T_PAYMENTREQUESTED"))
                'dtStatus.Rows.Add(2, getMessage("T_PAYMENTREQUESTED"))
                'dtStatus.Rows.Add(3, getMessage("T_PAYMENTREQUESTED"))
                'dtStatus.Rows.Add(4, getMessage("T_PAYMENTRPAID"))
                'dtStatus.Rows.Add(5, getMessage("T_PAYMENTMATCHED"))

                dtStatus.Rows.Add(5, getMessage("T_PAYMENTMATCHED"))
                dtStatus.Rows.Add(4, getMessage("T_UNMATCHED"))
            Else
                'dtStatus.Rows.Add(-3, getMessage("T_FAILEDSTATUS"))
                'dtStatus.Rows.Add(1, getMessage("T_PAYMENTREQUESTED"))
                'dtStatus.Rows.Add(4, getMessage("T_PAYMENTRPAID"))
                'dtStatus.Rows.Add(5, getMessage("T_PAYMENTMATCHED"))

                dtStatus.Rows.Add(5, getMessage("T_PAYMENTMATCHED"))
                dtStatus.Rows.Add(4, getMessage("T_UNMATCHED"))
            End If

        Else
            dtStatus.Rows.Add(5, getMessage("T_PAYMENTMATCHED"))
            dtStatus.Rows.Add(1, getMessage("T_UNMATCHED"))
        End If

        Return dtStatus
    End Function

    Public Function GetPostingStatus(Optional Include As Boolean = False) As DataTable
        Dim dtStatus As New DataTable
        dtStatus.Columns.Add("PostingID", GetType(Integer))
        dtStatus.Columns.Add("PostingStatusName", GetType(String))
        If Include = True Then
            dtStatus.Rows.Add(-1, getMessage("T_SELECTPOSTINGSTATUS"))
        End If

        dtStatus.Rows.Add(4, getMessage("T_POSTED"))
        dtStatus.Rows.Add(2, getMessage("T_REJECTEDPOSTED"))
        dtStatus.Rows.Add(0, getMessage("T_NOTYETCONFIRMED"))
        Return dtStatus
    End Function
    Public Function GetAssignmentStatus(Optional Include As Boolean = False) As DataTable
        Dim dtStatus As New DataTable
        dtStatus.Columns.Add("AssignedID", GetType(Integer))
        dtStatus.Columns.Add("AssignedStatusName", GetType(String))
        If Include = True Then
            dtStatus.Rows.Add(-1, getMessage("T_SELECTASSIGNMENTSTATUS"))
        End If

        dtStatus.Rows.Add(0, getMessage("T_ASSIGNED"))
        dtStatus.Rows.Add(1, getMessage("T_REJECTEDASSIGNMENTS"))
        dtStatus.Rows.Add(2, getMessage("T_NOTYETASSIGNED"))
        Return dtStatus
    End Function
    Public Function GetScope() As DataTable
        Dim dtScope As New DataTable
        dtScope.Columns.Add("ScopeID", GetType(Integer))
        dtScope.Columns.Add("ScopeName", GetType(String))
        dtScope.Rows.Add(-1, getMessage("T_SELECTSCOPE"))
        dtScope.Rows.Add(0, getMessage("T_CLAIMSONLY"))
        dtScope.Rows.Add(1, getMessage("T_CLAIMSANDREJECTIONDETAILS"))
        dtScope.Rows.Add(2, getMessage("T_CLAIMSANDALLDETAILS"))

        Return dtScope
    End Function
End Class
