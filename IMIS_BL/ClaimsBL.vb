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

Imports System.Web
Imports System.IO

Public Class ClaimsBL
    Dim clm As New IMIS_DAL.ClaimsDAL
    Private imisgen As New GeneralBL
    Private Const MaxGridRow As Integer = 2000
    Public Function getMessage(ByVal MessageID As String) As String
        Return System.Web.HttpContext.GetGlobalResourceObject("Resource", MessageID)
    End Function


    Public Sub LoadClaim(ByRef eClaim As IMIS_EN.tblClaim, Optional ByRef eExtra As Dictionary(Of String, Object) = Nothing)
        Dim Claim As New IMIS_DAL.ClaimsDAL
        Claim.LoadClaim(eClaim, eExtra)
    End Sub
    Public Function ReviewClaim(ByVal ClaimID As Integer) As DataSet
        Dim claim As New IMIS_DAL.ClaimsDAL
        Return claim.ReviewClaim(ClaimID)
    End Function

    Public Function ReturnClaimStatus(ByVal claimstatus As Integer) As String
        Select Case claimstatus
            Case 1 : Return getMessage("T_REJECTED")
            Case 2 : Return getMessage("T_ENTERED")
            Case 4 : Return getMessage("T_CHECKED")
            Case 8 : Return getMessage("T_PROCESSED")
            Case 16 : Return getMessage("T_VALUATED")
            Case Else : Return ""
        End Select
    End Function

    Public Function ReturnReviewStatus(ByVal reviewstatus As Integer) As String
        Select Case reviewstatus
            Case 1 : Return getMessage("T_IDLE")
            Case 2 : Return getMessage("T_NOTSELECTED")
            Case 4 : Return getMessage("T_SELECTEDFORREVIEW")
            Case 8 : Return getMessage("T_REVIEWED")
            Case 16 : Return getMessage("T_BYPASSED")
            Case Else : Return getMessage("T_SELECTSTATUS")
        End Select
    End Function

    Public Function ReturnFeedbackStatus(ByVal feedbackstatus As Integer) As String
        Select Case feedbackstatus
            Case 1 : Return getMessage("T_IDLE")
            Case 2 : Return getMessage("T_NOTSELECTED")
            Case 4 : Return getMessage("T_SELECTEDFORFEEDBACK")
            Case 8 : Return getMessage("T_DELIVERED")
            Case 16 : Return getMessage("T_BYPASSED")
            Case Else : Return getMessage("T_SELECTSTATUS")
        End Select
    End Function
    Public Function GetReviewStatus(Optional ByVal RetrievalValue As Integer = 0) As DataTable
        Dim dtbl As New DataTable
        Dim dr As DataRow
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("Status")
        If RetrievalValue = 0 Then RetrievalValue = 63
        Dim max As Integer = 32
        Dim i As Integer = 1
        Do Until i > max

            If (i And RetrievalValue) > 0 Then
                If i = max Then
                    dr = dtbl.NewRow
                    dr("Code") = max - 1
                    dr("Status") = ReturnReviewStatus(i)

                    dtbl.Rows.InsertAt((dr), 0)
                Else
                    dr = dtbl.NewRow
                    dr("Code") = i
                    dr("Status") = ReturnReviewStatus(i)
                    dtbl.Rows.Add(dr)
                End If


            End If
            i += i
        Loop

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
        '    dr("Status") = getMessage("T_SELECTEDFORREVIEW")
        '    dtbl.Rows.Add(dr)

        '    dr = dtbl.NewRow
        '    dr("Code") = 4
        '    dr("Status") = getMessage("T_REVIEWED")
        '    dtbl.Rows.Add(dr)

        '    dr = dtbl.NewRow
        '    dr("Code") = 8
        '    dr("Status") = getMessage("T_BYPASSED")
        '    dtbl.Rows.Add(dr)


        Return dtbl
    End Function
    Public Function GetClaimStatus(Optional ByVal RetrievalValue As Integer = 0) As DataTable
        Dim dtbl As New DataTable
        Dim dr As DataRow
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("Status")
        If RetrievalValue = 0 Then RetrievalValue = 63
        Dim i As Integer = 0
        If (2 And RetrievalValue) > 0 Then
            dr = dtbl.NewRow
            dr("Code") = 2
            dr("Status") = ReturnClaimStatus(2)
            dtbl.Rows.Add(dr)
            i += 2
        End If
        If (4 And RetrievalValue) > 0 Then
            dr = dtbl.NewRow
            dr("Code") = 4
            dr("Status") = ReturnClaimStatus(4)
            dtbl.Rows.Add(dr)
            i += 4
        End If
        If (8 And RetrievalValue) > 0 Then
            dr = dtbl.NewRow
            dr("Code") = 8
            dr("Status") = ReturnClaimStatus(8)
            dtbl.Rows.Add(dr)
            i += 8
        End If
        If (16 And RetrievalValue) > 0 Then
            dr = dtbl.NewRow
            dr("Code") = 16
            dr("Status") = ReturnClaimStatus(16)
            dtbl.Rows.Add(dr)
            i += 16
        End If
        If (1 And RetrievalValue) > 0 Then
            dr = dtbl.NewRow
            dr("Code") = 1
            dr("Status") = ReturnClaimStatus(1)
            dtbl.Rows.Add(dr)
            i += 1
        End If
        If (32 And RetrievalValue) > 0 Then
            dr = dtbl.NewRow
            dr("Code") = i
            dr("Status") = getMessage("T_SELECTSTATUS")
            dtbl.Rows.InsertAt((dr), 0)
        End If
        Return dtbl
    End Function

    Public Function GetFeedbackStatus(Optional ByVal RetrievalValue As Integer = 0) As DataTable
        Dim dtbl As New DataTable
        Dim dr As DataRow
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("Status")
        If RetrievalValue = 0 Then RetrievalValue = 63
        Dim max As Integer = 32
        Dim i As Integer = 1
        Do Until i > max

            If (i And RetrievalValue) > 0 Then
                If i = max Then
                    dr = dtbl.NewRow
                    dr("Code") = max - 1
                    dr("Status") = ReturnFeedbackStatus(i)
                    dtbl.Rows.InsertAt((dr), 0)
                Else
                    dr = dtbl.NewRow
                    dr("Code") = i
                    dr("Status") = ReturnFeedbackStatus(i)
                    dtbl.Rows.Add(dr)
                End If


            End If
            i += i
        Loop
        'If showSelect = True Then
        '    dr = dtbl.NewRow
        '    dr("Code") = 7
        '    dr("Status") = "-- Select Status --"
        '    dtbl.Rows.Add(dr)
        'End If
        'dr = dtbl.NewRow
        'dr("Code") = 0
        'dr("Status") = ReturnFeedbackStatus(0)
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = 1
        'dr("Status") = ReturnFeedbackStatus(1)
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = 2
        'dr("Status") = ReturnFeedbackStatus(2)
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = 4
        'dr("Status") = ReturnFeedbackStatus(4)
        'dtbl.Rows.Add(dr)

        'dr = dtbl.NewRow
        'dr("Code") = 8
        'dr("Status") = ReturnFeedbackStatus(8)
        'dtbl.Rows.Add(dr)

        Return dtbl
    End Function
    Public Sub UpdateClaimApprovedValue(ByRef eClaim As IMIS_EN.tblClaim)
        Dim claim As New IMIS_DAL.ClaimsDAL
        claim.UpdateClaimApprovedValue(eClaim)
    End Sub
    Public Function SaveClaim(ByRef eclaim As IMIS_EN.tblClaim) As Integer
        Dim claim As New IMIS_DAL.ClaimsDAL
        If eclaim.ClaimID = 0 Then
            claim.InsertClaim(eclaim)
            Return 1
        Else
            claim.UpdateClaim(eclaim)
            Return 2
        End If

    End Function
    Public Sub UpdateClaimTotalValue(ByRef eClaim As IMIS_EN.tblClaim)
        Dim claim As New IMIS_DAL.ClaimsDAL
        claim.UpdateClaimTotalValue(eClaim)
    End Sub
    Public Function UpdateClaimReview(ByRef eClaim As IMIS_EN.tblClaim) As Boolean
        clm.UpdateClaimReview(eClaim)
        Return True
    End Function
    Public Function IsClaimReviewStatusChanged(ByVal eClaim As IMIS_EN.tblClaim) As Boolean
        Dim dt As DataTable = clm.IsClaimReviewStatusChanged(eClaim)
        If Not dt.Rows.Count = 0 Then
            If dt.Rows(0).Item(0) = 8 Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Function checkClaimCode(ByVal eClaim As IMIS_EN.tblClaim) As Boolean
        Dim dt As DataTable = clm.checkClaimCode(eClaim)
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function


    Public Function GetReviewClaims(ByRef eClaims As IMIS_EN.tblClaim, ByVal UserID As Integer) As DataTable
        Return clm.GetReviewClaims(eClaims, GetClaimStatus, UserID)
    End Function
    Public Function GetReviewClaimsCount(ByRef eClaims As IMIS_EN.tblClaim, ByVal UserID As Integer) As Integer
        Dim dt As DataTable = clm.GetReviewClaimsCount(eClaims, GetClaimStatus, UserID)
        If Not dt.Rows.Count = 0 Then
            If dt.Rows(0).Item(0) > MaxGridRow Then
                Return 1
            Else
                Return 2
            End If
        Else
            Return 2
        End If
    End Function
    Public Function GetClaims(ByRef eClaims As IMIS_EN.tblClaim, ByVal UserID As Integer) As DataTable
        Return clm.GetClaims(eClaims, GetClaimStatus, GetFeedbackStatus, GetReviewStatus, UserID)
    End Function
    Public Function GetClaimsCount(ByRef eClaims As IMIS_EN.tblClaim, ByVal UserID As Integer) As Integer
        Dim dt As DataTable = clm.GetClaimsCount(eClaims, GetClaimStatus, GetFeedbackStatus, GetReviewStatus, UserID)
        If Not dt.Rows.Count = 0 Then
            If dt.Rows(0).Item(0) > MaxGridRow Then
                Return 1
            Else
                Return 2
            End If
        Else
            Return 2
        End If
    End Function

  
    Public Function GetReviewSelection(Optional ByVal showselect As Boolean = False) As DataTable

        Dim dt As New DataTable
        dt.Columns.Add("ReviewCode")
        dt.Columns.Add("ReviewText")
        Dim dr As DataRow
        If showselect = True Then
            dr = dt.NewRow
            dr("ReviewCode") = 0
            dr("ReviewText") = imisgen.getMessage("T_SELECT")
            dt.Rows.Add(dr)
        End If
        dr = dt.NewRow
        dr("ReviewCode") = 1
        dr("ReviewText") = imisgen.getMessage("T_REVIEWSELECT")
        dt.Rows.Add(dr)
        dr = dt.NewRow
        dr("ReviewCode") = 2
        dr("ReviewText") = imisgen.getMessage("T_FEEDBACKSELECT")
        dt.Rows.Add(dr)

        Return dt
    End Function
    Public Sub ReviewFeedbackSelection(ByVal dt As DataTable, ByVal Value As Decimal, ByVal ReviewType As Int16, ByVal SelectionType As Int16, ByVal SelectionValue As Decimal, ByRef Submitted As Integer, ByRef Selected As Integer, ByRef NotSelected As Integer)
        clm.ReviewFeedbackSelection(dt, Value, ReviewType, SelectionType, SelectionValue, Submitted, Selected, NotSelected)
    End Sub

    Public Function ManualSelectionUpdate(ByVal eClaim As IMIS_EN.tblClaim, ByVal StatusField As String) As Integer
        If StatusField = "FeedBack" Then
            clm.ManualSelectionUpdate(eClaim)
            Return 1
        ElseIf StatusField = "Review" Then
            clm.ManualSelectionUpdate(eClaim)
            Return 2
        Else
            Return -1
        End If

    End Function

   
    Public Function IsClaimStatusChanged(ByRef eClaim As IMIS_EN.tblClaim) As Boolean
        Dim dt As DataTable = clm.IsClaimStatusChanged(eClaim)
        If dt.Rows.Count > 0 Then
            If Not dt.Rows(0).Item("ClaimStatus") = 2 Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function DeleteClaim(ByRef eClaim As IMIS_EN.tblClaim) As Boolean
        clm.DeleteClaim(eClaim)
        Return True
    End Function
    Public Function SaveFeedback(ByVal efeedback As IMIS_EN.tblFeedback) As Integer

        If efeedback.FeedbackID = 0 Then
            clm.InsertFeedback(efeedback)
            Return 1
        Else
            clm.UpdateFeedback(efeedback)
            Return 2
        End If
    End Function

    Public Sub ProcessClaims(ByVal dt As DataTable, ByVal UserID As Integer, ByRef Submitted As Integer, ByRef Processed As Integer, ByRef Valuated As Integer, ByRef Changed As Integer, ByRef Rejected As Integer, ByRef Failed As Integer, ByRef ReturnValue As Integer)
        clm.ProcessClaims(dt, UserID, Submitted, Processed, Valuated, Changed, Rejected, Failed, ReturnValue)
    End Sub
    Public Sub SubmitClaims(ByVal dt As DataTable, ByVal UserID As Integer, ByRef Submitted As Integer, ByRef Checked As Integer, ByRef Rejected As Integer, ByRef Changed As Integer, ByRef Failed As Integer, ByRef ItemsPassed As Integer, ByRef ServicesPassed As Integer, ByRef ItemsRejected As Integer, ByRef ServicesRejected As Integer)
        clm.SubmitClaims(dt, UserID, Submitted, Checked, Rejected, Changed, Failed, ItemsPassed, ServicesPassed, ItemsRejected, ServicesRejected)
    End Sub
    Public Sub WriteToXml(ByVal ClaimID As Integer)
        Dim dtXML As New DataTable
        dtXML = clm.GetXML(ClaimID)

        Dim ClaimXml As System.Xml.XmlDocument = New System.Xml.XmlDocument
        ClaimXml.LoadXml(dtXML.Rows(0)(0))

        Dim path As String = HttpContext.Current.Server.MapPath("WorkSpace") & "\Claim_" & ClaimID & ".xml"

        ClaimXml.Save(path)

    End Sub

   

    Public Sub ZipXMLs()
        Dim Extracts As New IMIS_BL.IMISExtractsBL
        Dim defaults As New IMIS_EN.tblIMISDefaults
        Dim def As New IMIS_BL.IMISDefaultsBL
        def.GetDefaults(defaults)
        Dim hf As New IMIS_BL.HealthFacilityBL
        Dim hfcode As String = hf.getHFCodeFromID(defaults.OffLineHF)
        Dim rarFolder As String = defaults.WinRarFolder
        Extracts.Zip(HttpContext.Current.Server.MapPath("~/WorkSpace/"), "Claims-" & hfcode & "-" & Format(Now, "dd-MM-yyyy") & ".rar", HttpContext.Current.Server.MapPath("~/WorkSpace/"), "Claim_*.xml")

        MoveXMLsToArhive()


    End Sub

    Private Sub MoveXMLsToArhive()
        Dim XMLs As String()
        XMLs = Directory.GetFiles(HttpContext.Current.Server.MapPath("Workspace"), "Claim_*.xml")

        For i As Integer = 0 To XMLs.Length - 1
            If File.Exists(XMLs(i)) Then
                File.Move(XMLs(i), HttpContext.Current.Server.MapPath("Archive\") & Mid(XMLs(i), XMLs(i).LastIndexOf("\") + 2, XMLs(i).Length))
                'File.Delete(XMLs(i))
                'Continue For
            End If


        Next

    End Sub

    Public Function IsClaimStatusChecked(ByVal eClaim As IMIS_EN.tblClaim) As Boolean
        Dim dt As DataTable = clm.IsClaimStatusChecked(eClaim)
        If Not dt.Rows.Count = 0 Then
            If dt.Rows(0).Item(0) = 4 Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function
    Public Function GetBatchRunDate(ByVal DistrictID As Integer) As DataTable
        Return clm.GetBatchRunDate(DistrictID)
    End Function

    Public Function sendSMS(ByVal DateFrom As Date, ByVal DateTo As Date) As String

        'Dim BLGeneral As New GeneralBL
        'Return BLGeneral.ReadSMSDatatable(clm.getFeedbackSMSData(DateFrom, DateTo))

        Dim Esc As New EscapeBL
        Dim Message As String = clm.getFeedbackSMSData(DateFrom, DateTo).Rows(0)(0).ToString
        Return Esc.SendSMS(Message)

    End Function
    Public Function GetVisitTypes(Optional ByVal ShowSelect As Boolean = False) As DataTable
        Dim dt As New DataTable
        dt.Columns.Add("Code")
        dt.Columns.Add("Visit")
        Dim dr As DataRow = Nothing
        If ShowSelect Then
            dr = dt.NewRow
            dr("Code") = ""
            dr("Visit") = getMessage("V_SELECTTYPE")
            dt.Rows.Add(dr)
        End If
        dr = dt.NewRow
        dr("Code") = "E"
        dr("Visit") = getMessage("M_EMERGENCY")
        dt.Rows.Add(dr)
        dr = dt.NewRow
        dr("Code") = "R"
        dr("Visit") = getMessage("L_REFERRALS")
        dt.Rows.Add(dr)
        dr = dt.NewRow
        dr("Code") = "O"
        dr("Visit") = getMessage("M_OTHER")
        dt.Rows.Add(dr)
        Return dt
    End Function
    Public Function GetVisitTypeText(ByVal VisitTypeCode As Char) As String
        Dim dt As DataTable = GetVisitTypes()
        Dim drs() As DataRow = dt.Select("Code = '" & VisitTypeCode & "'")
        If drs.Count > 0 Then Return drs(0)("Visit")
        Return ""
    End Function
    Public Function GetClaim(ByVal ClaimID As Integer) As DataTable
        Return clm.GetClaim(ClaimID)
    End Function
    Public Function GetServiceRejectedReason(ByVal ReasonId As Integer) As String
        Return imisgen.GetRejectedReasons(ReasonId)
    End Function
    Public Function GetItemRejectedReason(ByVal ReasonId As Integer) As String
        Return imisgen.GetRejectedReasons(ReasonId)
    End Function
    Public Function GetClaimIdByUUID(ByVal uuid As Guid) As Integer
        Dim Claim As New IMIS_DAL.ClaimsDAL
        Dim Result As DataTable = Claim.GetClaimIdByUUID(uuid)
        If Result IsNot Nothing And Result.Rows.Count > 0 Then
            Return Result.Rows(0).Item(0)
        Else
            Return 0
        End If
    End Function
    Public Function GetClaimUUIDByID(ByVal id As Integer) As Guid
        Dim Claim As New IMIS_DAL.ClaimsDAL
        Dim Result As DataTable = Claim.GetClaimUUIDByID(id)
        If Result IsNot Nothing And Result.Rows.Count > 0 Then
            Return Result.Rows(0).Item(0)
        Else
            Return Guid.Empty
        End If
    End Function
    Public Function GetClaimUUIDByClaimCode(ByVal ClaimCode As String) As Guid
        Dim Claim As New IMIS_DAL.ClaimsDAL
        Dim Result As DataTable = Claim.GetClaimUUIDByClaimCode(ClaimCode)
        If Result IsNot Nothing And Result.Rows.Count > 0 Then
            Return Result.Rows(0).Item(0)
        Else
            Return Guid.Empty
        End If
    End Function
End Class
