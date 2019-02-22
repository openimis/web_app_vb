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

Imports System.Web.Script.Serialization

Public Class IMIS_Gen
    Inherits System.Web.UI.Page
    Private Shared _offlineHF As Boolean
    Private Shared _OfflineCHF As Boolean
    Private Shared _HFID As Integer
    Private Shared _HFCode As String
    Private Shared _HFName As String
    Private Shared _CHFID As Integer

    Public Shared Property offlineHF() As Boolean
        Get
            Return _offlineHF
        End Get
        Set(ByVal value As Boolean)
            _offlineHF = value
        End Set
    End Property
    Public Shared Property OfflineCHF() As Boolean
        Get
            Return _OfflineCHF
        End Get
        Set(ByVal value As Boolean)
            _OfflineCHF = value
        End Set
    End Property
    Public Shared Property HFID() As Integer
        Get
            Return _HFID
        End Get
        Set(ByVal value As Integer)
            _HFID = value
        End Set
    End Property
    Public Shared Property HFName() As String
        Get
            Return _HFName
        End Get
        Set(ByVal value As String)
            _HFName = value
        End Set
    End Property
    Public Shared Property HFCode() As String
        Get
            Return _HFCode
        End Get
        Set(ByVal value As String)
            _HFCode = value
        End Set
    End Property
    Public Shared Property CHFID() As Integer
        Get
            Return _CHFID
        End Get
        Set(ByVal value As Integer)
            _CHFID = value
        End Set
    End Property
    Public Function getUserId(ByVal session As Object) As Integer
        Try
            Dim dt As New DataTable
            dt = DirectCast(session, DataTable)
            Return dt.Rows(0)("UserID")
        Catch
            Return Nothing
        End Try
    End Function
    'Public Function getRoleName(ByVal session As Object) As String
    '    Try
    '        Dim dt As New DataTable
    '        dt = DirectCast(session, DataTable)
    '        Return dt.Rows(0)("RoleName")
    '    Catch
    '        Return Nothing
    '    End Try
    'End Function
    Public Function getLoginName(ByVal session As Object) As String
        Try
            Dim dt As New DataTable
            dt = DirectCast(session, DataTable)
            Return dt.Rows(0)("LoginName")
        Catch
            Return ""
        End Try
    End Function
    Public Function getMessage(ByVal MessageID As String, Optional EncodeJS As Boolean = True) As String
        If EncodeJS = True Then
            Return HttpUtility.JavaScriptStringEncode(System.Web.HttpContext.GetGlobalResourceObject("Resource", MessageID))
        Else
            Return System.Web.HttpContext.GetGlobalResourceObject("Resource", MessageID)
        End If
    End Function


    Public Function getRoleId(ByVal session As Object) As Integer
        Try
            Dim dt As New DataTable
            dt = DirectCast(session, DataTable)
            Return dt.Rows(0)("RoleID")
        Catch
            Return Nothing
        End Try
    End Function
    Public Shared Function IsAjaxRequest() As Boolean
        Dim Req As System.Web.HttpRequest = HttpContext.Current.Request
        If Req Is Nothing Then Return False
        If Req.Headers IsNot Nothing Then
            If Req.Headers("X-Requested-With") IsNot Nothing Then
                Return Req.Headers("X-Requested-With") = "XMLHttpRequest"
            End If
        End If
        If Req("X-Requested-With") IsNot Nothing Then
            Return Req("X-Requested-With") = "XMLHttpRequest"
        End If
        Return False
    End Function
    Public Shared Function DataTableToList(ByVal dt As DataTable) As List(Of Dictionary(Of String, Object))
        Dim Lst As New List(Of Dictionary(Of String, Object))
        Dim dict As Dictionary(Of String, Object) = Nothing
        For Each row As DataRow In dt.Rows
            dict = New Dictionary(Of String, Object)
            For Each col As DataColumn In dt.Columns
                dict.Add(col.ColumnName, row(col.ColumnName))
            Next
            Lst.Add(dict)
        Next
        Return Lst
    End Function
    Public Shared Function ToJSON(ByVal Obj As Object) As String
        Dim json As New JavaScriptSerializer
        Return json.Serialize(Obj)
    End Function
    'added on 10 aug 2012 by ruzo
    Public Sub Alert(ByVal msg As String, ByRef controlIDToEmbedScript As Control, Optional ByVal jsCallBackFun As String = "", Optional ByVal jsCallBackFunArgs As String = "", Optional ByVal alertPopupTitle As String = "IMIS", Optional ByVal AcceptButtonText As String = "", Optional ByVal Queue As Boolean = False)
        Dim ltl As New Literal
        msg = "<script class='scriptServerPopup' type='text/javascript'>" & _
              "  popup.alertTitle='" & alertPopupTitle & "';" & _
              "  popup.acceptBTN_Text = '" & if(AcceptButtonText = "", getMessage("L_OK", True), AcceptButtonText) & "';" & _
              "  $(document).ready(function(){" & _
              "  popup.alert('" & msg & "','" & jsCallBackFun & "','" & jsCallBackFunArgs & "'," & if(Queue, "true", "false") & ");});" & _
              " </script>"
        ltl.Text = msg
        If Not controlIDToEmbedScript Is Nothing Then
            controlIDToEmbedScript.Controls.Add(ltl)
        Else
            Page.Controls.Add(ltl)
        End If
    End Sub
    Public Sub Confirm(ByVal msg As String, ByRef controlIDToEmbedScript As Control, Optional ByVal jsCallBackFun As String = "", Optional ByVal jsCallBackFunArgs As String = "", Optional ByVal confirmPopupTitle As String = "IMIS", Optional ByVal AcceptButtonText As String = "", Optional ByVal RejectButtonText As String = "", Optional ByVal Queue As Boolean = False)
        Dim ltl As New Literal
        msg = "<script class='scriptServerPopup' type='text/javascript'>" & _
              "  popup.confirmTitle='" & confirmPopupTitle & "';" & _
              "  popup.acceptBTN_Text = '" & if(AcceptButtonText = "", getMessage("L_OK", True), AcceptButtonText) & "';" & _
              " popup.rejectBTN_Text = '" & if(RejectButtonText = "", getMessage("L_CANCEL", True), RejectButtonText) & "';" & _
              "  $(document).ready(function(){" & _
              "  popup.confirm('" & msg & "','" & jsCallBackFun & "','" & jsCallBackFunArgs & "'," & if(Queue, "true", "false") & ");});" & _
              " </script>"
        ltl.Text = msg
        If Not controlIDToEmbedScript Is Nothing Then
            controlIDToEmbedScript.Controls.Add(ltl)
        Else
            Page.Controls.Add(ltl)
        End If
    End Sub
End Class
