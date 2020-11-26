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

Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication


    Public Const ServiceCacheItemKey As String = "ASP.NET_Service"
    Private Shared IsServiceOn As Boolean = False
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

        If Not (EventLog.SourceExists("IMIS")) Then

            EventLog.CreateEventSource("IMIS", "IMIS-LOG")

        End If
        IsServiceOn = True
        RegisterCacheEntry("Start")



    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
       
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        Dim cookie As HttpCookie = Request.Cookies.Get("CultureInfo")
        If Not cookie Is Nothing Then
            Dim culture_info As New System.Globalization.CultureInfo(cookie.Value)

            System.Threading.Thread.CurrentThread.CurrentUICulture = culture_info
        Else
            Dim culture_info As New System.Globalization.CultureInfo("en")

            System.Threading.Thread.CurrentThread.CurrentUICulture = culture_info
        End If
        Try
            'If the dummy page is hit, then it means we want to add another item in cache

            If HttpContext.Current.Cache(ServiceCacheItemKey) = "Start" Then
                Dim StrPathAndQuery As String = HttpContext.Current.Request.Url.PathAndQuery
                Dim DomainUrl As String = HttpContext.Current.Request.Url.AbsoluteUri.Replace(StrPathAndQuery, "/")
                'Add the item in cache and when successful, do the work.
                HttpContext.Current.Cache.Remove(ServiceCacheItemKey)
                RegisterCacheEntry(DomainUrl)

            Else
                If HttpContext.Current.Request.Url.LocalPath.ToUpper = "/" & "BgService.aspx".ToUpper Then

                    If HttpContext.Current.Request.QueryString("AX") = 1 Then
                        Debug.Print(Date.Now & " Register")
                        IsServiceOn = True
                        Dim StrPathAndQuery As String = HttpContext.Current.Request.Url.PathAndQuery
                        Dim DomainUrl As String = HttpContext.Current.Request.Url.AbsoluteUri.Replace(StrPathAndQuery, "/")
                        'Add the item in cache and when successful, do the work.
                        RegisterCacheEntry(DomainUrl)

                        HttpContext.Current.Response.AddHeader("ServiceResponse", "<h1 id='ServiceSuccess' style='color:green;' >Service run successfully.</h1>")
                    Else
                        Debug.Print(Date.Now & " Else")
                        IsServiceOn = False
                        If HttpContext.Current.Cache.Remove(ServiceCacheItemKey) IsNot Nothing Then Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            'Write log
        End Try
        ' MyBase.InitializeCulture()
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
       
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
        If Session("LogInFlag") = True Then
            Dim Login As New IMIS_BI.LoginBI
            Dim dt As DataTable = CType(Session("User"), DataTable)
            Dim UserID As Integer = dt.Rows(0)("UserID")
            Login.InsertLogTime(UserID, -1)
            Session("LogInFlag") = False

        End If
    End Sub


    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub
#Region "Procedures"
    Private Sub RegisterCacheEntry(CurrentDomainurl As String)
        If HttpContext.Current.Cache(ServiceCacheItemKey) IsNot Nothing Then
            HttpContext.Current.Cache.Remove(ServiceCacheItemKey)
            ' Exit Sub
        End If

        Dim Interval As Integer = 300

        HttpContext.Current.Cache.Add(ServiceCacheItemKey, CurrentDomainurl,
                     Nothing, DateTime.MaxValue, TimeSpan.FromSeconds(Interval), CacheItemPriority.Normal,
                     New CacheItemRemovedCallback(AddressOf CacheItemRemovedCallback))
    End Sub
    Public Sub CacheItemRemovedCallback(key As String, value As Object, reason As CacheItemRemovedReason)
        Try
                If value = "Start" Then Exit Sub
            ' If Not IsServiceOn Then Exit Sub
            HitPage(value)
            Debug.Print(Date.Now)
            EventLog.WriteEntry("IMIS", "Service background tasks start", EventLogEntryType.Information, 166)

            'Do the service works
            IMIS_BI.GlobalAsaxBI.DoBackgroundTasks(value)
            EventLog.WriteEntry("IMIS", "Service background completed", EventLogEntryType.Information, 167)
        Catch ex As Exception
            EventLog.WriteEntry("IMIS", ex.Message, EventLogEntryType.Error, 900)
        End Try
    End Sub
    Private Sub HitPage(CurrentDomainurl)

        'Dim CurrentDomainurl As String = "http://localhost:57214/"
        Dim Uri As New System.Uri(CurrentDomainurl & "BgService.aspx" & "?AX=1") '&R=" & LOIS_EN.GeneralEN.EncryptData(LOIS_EN.GeneralEN.EWURARoles.Administrator))
        Dim Client As New Net.WebClient()
        Client.DownloadDataAsync(Uri)
    End Sub
#End Region
End Class
