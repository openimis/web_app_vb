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


Partial Public Class UploadICD
    Inherits System.Web.UI.Page

    Private ICD As New IMIS_BI.RegistersBI
    Protected imisgen As New IMIS_Gen


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        RunPageSecurity()

        If Request.Form("__EVENTTARGET") = btnUpload.ClientID Then
            btnUpload_Click(sender, New System.EventArgs)
        End If

        If Request.Form("__EVENTTARGET") = btnUploadLocations.ClientID Then
            btnUploadLocations_Click(sender, New System.EventArgs)
        End If

        If Request.Form("__EVENTTARGET") = btnUploadHF.ClientID Then
            btnUploadHF_Click(sender, New System.EventArgs)
        End If

        If Not Page.IsPostBack Then
            FillCombo()
        End If
        If Not (Page.IsPostBack) Then
            PnlUploadLog.Visible = False
        End If

    End Sub

    Private Sub RunPageSecurity()
        Dim RefUrl = Request.Headers("Referer")
        Dim UserID As Integer = imisgen.getUserId(Session("User"))
        If Not ICD.RunPageSecurity(IMIS_EN.Enums.Pages.UploadICD, Page) Then


            Server.Transfer("Redirect.aspx?perm=0&page=" & IMIS_EN.Enums.Pages.UploadICD.ToString & "&retUrl=" & RefUrl)
        Else
            pnlUploadDiagnoses.Enabled = ICD.checkRights(IMIS_EN.Enums.Rights.DiagnosesUpload, UserID)
            FileUploadDiagnosis.Enabled = pnlUploadDiagnoses.Enabled
            pnlDownLoadICD.Enabled = ICD.checkRights(IMIS_EN.Enums.Rights.DiagnosesDownload, UserID)
            PnlUploadHF.Enabled = ICD.checkRights(IMIS_EN.Enums.Rights.HealthFacilitiesUpload, UserID)
            FileUploadHF.Enabled = PnlUploadHF.Enabled
            pnlDownLoadHF.Enabled = ICD.checkRights(IMIS_EN.Enums.Rights.HealthFacilitiesDownload, UserID)
            pnlUploadLocations.Enabled = ICD.checkRights(IMIS_EN.Enums.Rights.LocationUpload, UserID)
            FileUploadLocations.Enabled = pnlUploadLocations.Enabled
            pnlDownLoadLocations.Enabled = ICD.checkRights(IMIS_EN.Enums.Rights.LocationDonwload, UserID)

        End If
    End Sub

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpload.Click
        If FileUploadDiagnosis.HasFile Then
            Try
                Dim Output As New Dictionary(Of String, Integer)
                Dim FileName As String = Server.MapPath("WorkSpace") & "\" & FileUploadDiagnosis.PostedFile.FileName
                FileUploadDiagnosis.SaveAs(FileName)
                Dim StratergyId = ddlUploadStrategy.SelectedValue
                Dim dtResult As New DataTable
                Dim LogFile As String = String.Empty
                Dim registerName As String
                registerName = imisgen.getMessage("L_DIAGNOSIS")
                Output = ICD.UploadICD(FileName, StratergyId, imisgen.getUserId(Session("User")), dtResult, chkDryRun.Checked, registerName, LogFile)
                'If chkDryRun.Checked = False Then lblMsg.Text = imisgen.getMessage("M_ICDUPLOADED")


                Dim dtError As New DataTable
                Dim dtConflict As New DataTable
                Dim dtFatalError As New DataTable
                Dim dv As DataView = dtResult.DefaultView

                dv.RowFilter = "ResultType='E' OR ResultType='FH' OR  ResultType='FR' OR  ResultType='FD' OR  ResultType='FM' OR  ResultType='FV' OR  ResultType='FI'"
                dtError = dv.ToTable
                dv.RowFilter = "ResultType='C'"
                dtConflict = dv.ToTable
                dv.RowFilter = "ResultType='FE'"
                dtFatalError = dv.ToTable





                Dim ErrMsg As String = "<h4><u>" & dtError.Rows.Count.ToString() & " " & If(dtError.Rows.Count = 1, imisgen.getMessage("L_ERROR"), imisgen.getMessage("L_ERRORS")) & "</u></h4>" & "<br>"
                For i As Int16 = 0 To dtError.Rows.Count - 1
                    ErrMsg += dtError.Rows(i)(0).ToString() & "<br>"
                Next


                Dim ConflictMsg As String = "<h4><u>" & dtConflict.Rows.Count.ToString() & "  " & If(dtConflict.Rows.Count = 1, imisgen.getMessage("L_CONFILCT"), imisgen.getMessage("L_CONFILCTS")) & "</u></h4>" & "<br>"
                For i As Int16 = 0 To dtConflict.Rows.Count - 1
                    ConflictMsg += dtConflict.Rows(i)(0).ToString() & "<br>"
                Next

                Dim FatalErrorMsg As String = "<h4><u>" & dtFatalError.Rows.Count.ToString() & " " & imisgen.getMessage("L_FATALERROR") & "</u></h4>" & "<br>"
                For i As Int16 = 0 To dtFatalError.Rows.Count - 1
                    FatalErrorMsg += dtFatalError.Rows(i)(0).ToString() & "<br>"
                Next

                Dim result As Integer = Output("returnValue")
                Dim str As String = ""

                Select Case result
                    Case 0
                        str = imisgen.getMessage("M_DIAGNOSESUPLOADED")
                    Case -1
                        str = imisgen.getMessage("M_UNEXPECTEDERRORONUPLOADICD")
                End Select
                If chkDryRun.Checked = False And dtFatalError.Rows.Count = 0 Then imisgen.Alert(str, Panel1, alertPopupTitle:="IMIS", Queue:=True)

                Dim Msg As String = "<h4><u>" & imisgen.getMessage("M_UPLOADEDDIAGNOSES") & "</u></h4>" & "<br>" &
                imisgen.getMessage("L_SENT") & ": " & Output("DiagnosisSent") & "<br>" &
                imisgen.getMessage("L_INSERTED") & ": " & Output("Inserts") & "<br>" &
                imisgen.getMessage("L_UPDATED") & ": " & Output("Updates") & "<br> " &
                imisgen.getMessage("L_DELETED") & ": " & Output("Deletes") & "</br>"

                imisgen.Alert(Msg, Panel1, alertPopupTitle:="IMIS", Queue:=True)
                If dtFatalError.Rows.Count > 0 Then
                    imisgen.Alert(HttpUtility.JavaScriptStringEncode(FatalErrorMsg), Panel1, alertPopupTitle:="IMIS", Queue:=True)
                End If
                If dtError.Rows.Count > 0 Then
                    imisgen.Alert(HttpUtility.JavaScriptStringEncode(ErrMsg), Panel1, alertPopupTitle:="IMIS", Queue:=True)
                End If
                If dtConflict.Rows.Count > 0 Then
                    imisgen.Alert(HttpUtility.JavaScriptStringEncode(ConflictMsg), Panel1, alertPopupTitle:="IMIS", Queue:=True)
                End If



                showLastUploadLog(LogFile, registerName, System.IO.Path.GetFileName(FileName), ddlUploadStrategy.SelectedItem.Text, Format(DateTime.Now, "dd/MM/yyyy HH:mm"))


            Catch ex As Exception
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE", True), Panel1, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            End Try
        End If
    End Sub



    Protected Sub B_CANCEL_Click(ByVal sender As Object, ByVal e As EventArgs) Handles B_CANCEL.Click
        Response.Redirect("Home.aspx")
    End Sub

    Protected Sub btnUploadLocations_Click(sender As Object, ByVal e As EventArgs) Handles btnUploadLocations.Click
        If FileUploadLocations.HasFile Then
            Dim Output As New Dictionary(Of String, Integer)
            Try
                Dim FileName As String = Server.MapPath("WorkSpace") & "\" & FileUploadLocations.PostedFile.FileName
                FileUploadLocations.SaveAs(FileName)
                Dim dtResult As New DataTable
                Dim StrategyId As Integer = ddlUploadStrategyLocation.SelectedValue
                Dim LogFile As String = String.Empty
                Dim registerName As String
                registerName = imisgen.getMessage("L_Locations")
                Output = ICD.UploadLocationsXML(FileName, StrategyId, imisgen.getUserId(Session("User")), dtResult, chkDryRunLocations.Checked, registerName, LogFile)
                Dim dtError As New DataTable
                Dim dtConflict As New DataTable
                Dim dtFatalError As New DataTable
                Dim dv As DataView = dtResult.DefaultView

                dv.RowFilter = "ResultType='E' OR ResultType='FH' OR  ResultType='FR' OR  ResultType='FD' OR  ResultType='FM' OR  ResultType='FV' OR  ResultType='FI'"
                dtError = dv.ToTable
                dv.RowFilter = "ResultType='C'"
                dtConflict = dv.ToTable
                dv.RowFilter = "ResultType='FE'"
                dtFatalError = dv.ToTable

                Dim ErrMsg As String = "<h4><u>" & dtError.Rows.Count.ToString() & " " & If(dtError.Rows.Count = 1, imisgen.getMessage("L_ERROR"), imisgen.getMessage("L_ERRORS")) & "</u></h4>" & "<br>"
                For i As Int16 = 0 To dtError.Rows.Count - 1
                    ErrMsg += dtError.Rows(i)(0).ToString() & "<br>"
                Next


                Dim ConflictMsg As String = "<h4><u>" & dtConflict.Rows.Count.ToString() & "  " & If(dtConflict.Rows.Count = 1, imisgen.getMessage("L_CONFILCT"), imisgen.getMessage("L_CONFILCTS")) & "</u></h4>" & "<br>"
                For i As Int16 = 0 To dtConflict.Rows.Count - 1
                    ConflictMsg += dtConflict.Rows(i)(0).ToString() & "<br>"
                Next

                Dim FatalErrorMsg As String = "<h4><u>" & dtFatalError.Rows.Count.ToString() & " " & imisgen.getMessage("L_FATALERROR") & "</u></h4>" & "<br>"
                For i As Int16 = 0 To dtFatalError.Rows.Count - 1
                    FatalErrorMsg += dtFatalError.Rows(i)(0).ToString() & "<br>"
                Next

                Dim result As Integer = Output("returnValue")
                Dim str As String = ""
                Select Case result
                    Case 0
                        str = imisgen.getMessage("M_LOCATIONUPLOADED")
                    Case -1
                        str = imisgen.getMessage("M_UNEXPECTEDERRORONIMPORTLOCATIONS")
                End Select
                If chkDryRunLocations.Checked = False And dtFatalError.Rows.Count = 0 Then imisgen.Alert(str, Panel1, alertPopupTitle:="IMIS", Queue:=True)

                Dim Msg As String = "<h4><u>" & imisgen.getMessage("M_UPLOADEDLOCATIONS") & "</u></h4>" & "<br>" &
                                 "<b>" & imisgen.getMessage("L_REGION") & "</b><br>" &
                                 imisgen.getMessage("L_SENT") & ": " & Output("RegionSent") & "<br>" &
                                 imisgen.getMessage("L_INSERTED") & ": " & Output("RegionInsert") & "<br>" &
                                 imisgen.getMessage("L_UPDATED") & ": " & Output("RegionUpdate") & "</br><br>" &
                                 "<b>" & imisgen.getMessage("L_DISTRICT") & "</b><br>" &
                                 imisgen.getMessage("L_SENT") & ": " & Output("DistrictSent") & "</br>" &
                                 imisgen.getMessage("L_INSERTED") & ": " & Output("DistrictInsert") & "</br>" &
                                 imisgen.getMessage("L_UPDATED") & ": " & Output("DistrictUpdate") & "</br>" &
                                  "<b>" & imisgen.getMessage("L_WARD") & "</b><br>" &
                                 imisgen.getMessage("L_SENT") & ": " & Output("WardSent") & "</br>" &
                                 imisgen.getMessage("L_INSERTED") & ": " & Output("WardInsert") & "</br>" &
                                 imisgen.getMessage("L_UPDATED") & ": " & Output("WardUpdate") & "</br>" &
                                  "<b>" & imisgen.getMessage("L_VILLAGE") & "</b><br>" &
                                 imisgen.getMessage("L_SENT") & ": " & Output("VillageSent") & "</br>" &
                                 imisgen.getMessage("L_INSERTED") & ": " & Output("VillageInsert") & "</br>" &
                                 imisgen.getMessage("L_UPDATED") & ": " & Output("VillageUpdate") & "</br>"


                imisgen.Alert(Msg, Panel1, alertPopupTitle:="IMIS", Queue:=True)
                If dtFatalError.Rows.Count > 0 Then
                    imisgen.Alert(HttpUtility.JavaScriptStringEncode(FatalErrorMsg), Panel1, alertPopupTitle:="IMIS", Queue:=True)
                End If
                If dtError.Rows.Count > 0 Then
                    imisgen.Alert(HttpUtility.JavaScriptStringEncode(ErrMsg), Panel1, alertPopupTitle:="IMIS", Queue:=True)
                End If
                If dtConflict.Rows.Count > 0 Then
                    imisgen.Alert(HttpUtility.JavaScriptStringEncode(ConflictMsg), Panel1, alertPopupTitle:="IMIS", Queue:=True)
                End If



                showLastUploadLog(LogFile, registerName, System.IO.Path.GetFileName(FileName), ddlUploadStrategyLocation.SelectedItem.Text, Format(DateTime.Now, "dd/MM/yyyy HH:mm"))


            Catch ex As Exception
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE", True), Panel1, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            End Try
        End If
    End Sub

    Protected Sub btnDownLoadICD_Click(sender As Object, e As EventArgs)
        Dim path As String = ICD.DownLoadDiagnosis()
        DownloadFile(path, "application/xml")
    End Sub

    Private Sub FillCombo()
        ddlUploadStrategy.DataSource = ICD.GetICDUploadStrategies()
        ddlUploadStrategy.DataValueField = "StrategyId"
        ddlUploadStrategy.DataTextField = "StrategyName"
        ddlUploadStrategy.DataBind()

        ddlUploadStrategyLocation.DataSource = ICD.GetLocationUploadStrategies()
        ddlUploadStrategyLocation.DataValueField = "StrategyId"
        ddlUploadStrategyLocation.DataTextField = "StrategyName"
        ddlUploadStrategyLocation.DataBind()

        ddlUploadStrategyHF.DataSource = ICD.GetUploadStrategiesHF()
        ddlUploadStrategyHF.DataValueField = "StrategyId"
        ddlUploadStrategyHF.DataTextField = "StrategyName"
        ddlUploadStrategyHF.DataBind()

    End Sub

    Protected Sub btnUploadHF_Click(sender As Object, e As EventArgs) Handles btnUploadHF.Click
        If FileUploadHF.HasFile Then
            Dim Output As New Dictionary(Of String, Integer)
            Try
                Dim FileName As String = Server.MapPath("WorkSpace") & "\" & FileUploadHF.PostedFile.FileName
                Dim StrategyId As Integer = ddlUploadStrategyHF.SelectedValue
                Dim LogFile As String = String.Empty
                Dim dtResult As New DataTable
                FileUploadHF.SaveAs(FileName)
                Dim registerName As String
                registerName = imisgen.getMessage("L_HF")
                Output = ICD.UploadHF(FileName, StrategyId, imisgen.getUserId(Session("User")), dtResult, chkDryRunHF.Checked, registerName, LogFile)
                Dim dtError As New DataTable
                Dim dtConflict As New DataTable
                Dim dtFatalError As New DataTable
                Dim dv As DataView = dtResult.DefaultView

                dv.RowFilter = "ResultType='E' OR ResultType='FH' OR  ResultType='FR' OR  ResultType='FD' OR  ResultType='FM' OR  ResultType='FV' OR  ResultType='FI'"
                dtError = dv.ToTable
                dv.RowFilter = "ResultType='C'"
                dtConflict = dv.ToTable
                dv.RowFilter = "ResultType='FE'"
                dtFatalError = dv.ToTable

                Dim ErrMsg As String = "<h4><u>" & dtError.Rows.Count.ToString() & " " & If(dtError.Rows.Count = 1, imisgen.getMessage("L_ERROR"), imisgen.getMessage("L_ERRORS")) & "</u></h4>" & "<br>"
                For i As Int16 = 0 To dtError.Rows.Count - 1
                    ErrMsg += dtError.Rows(i)(0).ToString() & "<br>"
                Next


                Dim ConflictMsg As String = "<h4><u>" & dtConflict.Rows.Count.ToString() & "  " & If(dtConflict.Rows.Count = 1, imisgen.getMessage("L_CONFILCT"), imisgen.getMessage("L_CONFILCTS")) & "</u></h4>" & "<br>"
                For i As Int16 = 0 To dtConflict.Rows.Count - 1
                    ConflictMsg += dtConflict.Rows(i)(0).ToString() & "<br>"
                Next

                Dim FatalErrorMsg As String = "<h4><u>" & dtFatalError.Rows.Count.ToString() & " " & imisgen.getMessage("L_FATALERROR") & "</u></h4>" & "<br>"
                For i As Int16 = 0 To dtFatalError.Rows.Count - 1
                    FatalErrorMsg += dtFatalError.Rows(i)(0).ToString() & "<br>"
                Next

                Dim result As Integer = Output("returnValue")
                Dim str As String = ""
                Select Case result
                    Case 0
                        str = imisgen.getMessage("M_HFUPLOADED")
                    Case -1
                        str = imisgen.getMessage("M_UNEXPECTEDERRORONUPLOADHF")
                End Select
                If chkDryRunHF.Checked = False And dtFatalError.Rows.Count = 0 Then imisgen.Alert(str, Panel1, alertPopupTitle:="IMIS", Queue:=True)

                Dim Msg As String = "<h4><u>" & imisgen.getMessage("M_UPLOADEDHF") & "</u></h4>" & "<br>" &
                                 "<b>" & imisgen.getMessage("L_HF") & "</b><br>" &
                                 imisgen.getMessage("L_SENT") & ": " & Output("SentHF") & "<br>" &
                                 imisgen.getMessage("L_INSERTED") & ": " & Output("Inserts") & "<br>" &
                                 imisgen.getMessage("L_UPDATED") & ": " & Output("Updates") & "</br><br>" &
                                 "<b>" & imisgen.getMessage("L_HFCATCHMENT") & "</b><br>" &
                                 imisgen.getMessage("L_SENT") & ": " & Output("sentCatchment") & "</br>" &
                                 imisgen.getMessage("L_INSERTED") & ": " & Output("InsertCatchment") & "</br>" &
                                 imisgen.getMessage("L_UPDATED") & ": " & Output("UpdateCatchment") & "</br>"

                imisgen.Alert(Msg, Panel1, alertPopupTitle:="IMIS", Queue:=True)
                If dtFatalError.Rows.Count > 0 Then
                    imisgen.Alert(HttpUtility.JavaScriptStringEncode(FatalErrorMsg), Panel1, alertPopupTitle:="IMIS", Queue:=True)
                End If
                If dtError.Rows.Count > 0 Then
                    imisgen.Alert(HttpUtility.JavaScriptStringEncode(ErrMsg), Panel1, alertPopupTitle:="IMIS", Queue:=True)
                End If
                If dtConflict.Rows.Count > 0 Then
                    imisgen.Alert(HttpUtility.JavaScriptStringEncode(ConflictMsg), Panel1, alertPopupTitle:="IMIS", Queue:=True)
                End If

                showLastUploadLog(LogFile, registerName, System.IO.Path.GetFileName(FileName), ddlUploadStrategyHF.SelectedItem.Text, Format(DateTime.Now, "dd/MM/yyyy HH:mm"))

            Catch ex As Exception
                imisgen.Alert(imisgen.getMessage("M_ERRORMESSAGE", True), Panel1, alertPopupTitle:="IMIS")
                EventLog.WriteEntry("IMIS", Page.Title & " : " & imisgen.getLoginName(Session("User")) & " : " & ex.Message, EventLogEntryType.Error, 999)
            End Try
        End If
    End Sub

    Protected Sub btnDownloadHF_Click(sender As Object, e As EventArgs) Handles btnDownloadHF.Click
        Dim path As String = ICD.downLoadHFXML()
        DownloadFile(path, "application/xml")
    End Sub

    Protected Sub btnDownLoadLocation_Click(sender As Object, e As EventArgs) Handles btnDownLoadLocation.Click
        Dim path As String = ICD.DownLoadLocationsXML()
        DownloadFile(path, "application/xml")
    End Sub


    Private Sub DownloadFile(path As String, contentType As String)
        Dim strCommand As String = ""
        Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
        If file.Exists Then
            strCommand = "attachment;filename=" & System.IO.Path.GetFileName(path)
            Response.AppendHeader("Content-Disposition", strCommand)
            Response.ContentType = contentType
            Response.TransmitFile(path)
            Response.End()
            Response.Flush()
        End If
    End Sub

    Private Sub btnDiagnosisLog_Click(sender As Object, e As EventArgs) Handles btnDownloadLog.Click
        Try
            If hfDownloadLog.Value.Length > 0 Then
                DownloadFile(hfDownloadLog.Value, "application/text")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub showLastUploadLog(DownLoadPath As String, RegisterName As String, UploadedFileName As String, StrategyName As String, LogDate As String)
        If (DownLoadPath.Length > 0 And (chkDryRun.Checked = False Or chkDryRunHF.Checked = False Or chkDryRunLocations.Checked = False)) Then
            btnDownloadLog.Visible = True
            hfDownloadLog.Value = DownLoadPath
            LblFileName.Text = UploadedFileName
            LblRegister.Text = RegisterName
            LblStrategy.Text = StrategyName
            LblDate.Text = LogDate
            PnlUploadLog.Visible = True
        Else
            btnDownloadLog.Visible = False
            hfDownloadLog.Value = ""

        End If
    End Sub
End Class
