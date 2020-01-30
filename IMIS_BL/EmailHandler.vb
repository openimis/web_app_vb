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

Imports System.IO
Imports System.Net.Mail
Imports System.ComponentModel
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient


Public Class EmailHandler

    Private _CompanyName As String = "IMIS"
    


    Private Function getEmailSettings() As DataTable

        Dim ES As New IMIS_BL.EmailSettingsBL
        Return ES.getEmailSettings

    End Function

    Public Function sendEmail(ByVal TemplatePath As String, ByVal Recipient As String, ByVal Subject As String, ByVal Parameters As Dictionary(Of String, String), ByVal Attachment As MemoryStream, ByVal AttachmentName As String, ByVal Bcc As String, ByVal Cc As String) As Boolean
        Try


            Dim dtEmailSettings As DataTable = getEmailSettings()
            If dtEmailSettings.Rows.Count = 0 Then Return False

            Dim Message As String = String.Empty
            'Dim rtf As New RichTextboxEx


            'Check if template exists
            If TemplatePath.Trim.Length > 0 Then
                If Not File.Exists(TemplatePath) Then
                    Dim ex As New Exception("Could not find the template :" & TemplatePath)
                    Throw ex
                End If
                Message = My.Computer.FileSystem.ReadAllText(TemplatePath)

                'rtf.LoadFile(TemplatePath)
            Else
                Message = ""
            End If



            'Replace the template with provided parameters
            If Not Parameters Is Nothing Then
                For Each key As String In Parameters.Keys
                    Dim param As String = key
                    Dim Value As String = Parameters(key)
                    'rtf.Rtf = rtf.Rtf.Replace(param, Value)
                    Message = Message.Replace(param, Value)
                Next
            End If

            'If rtf.Text.Trim.Length > 0 Then
            '    'Convert the rtf string into HTML
            '    Message = RtfToHtml(rtf.Rtf)
            'Else
            '    Message = ""
            'End If

            Dim smtpServer As New SmtpClient
            Dim eMail As New MailMessage

            Dim smtpHost As String = dtEmailSettings(0)("SMTPHost")
            Dim port As Integer = dtEmailSettings(0)("Port")
            Dim FromEmail As String = dtEmailSettings(0)("EmailId")
            Dim EmailPassword As String = dtEmailSettings(0)("EmailPassword")
            Dim EnableSSL As Boolean = dtEmailSettings(0)("EnableSSL")

            'eMail = ToMailMessage(Message)

            With smtpServer
                .UseDefaultCredentials = False
                .Credentials = New Net.NetworkCredential(FromEmail, EmailPassword)
                .Port = port
                .Host = smtpHost
                .EnableSsl = EnableSSL
            End With

            'Always send one copy to sender
            'eMail.CC.Add(dtEmailSettings(0)("EmailId").ToString)

            With eMail
                .From = New MailAddress(FromEmail, _CompanyName)
                .To.Add(Recipient)
                If Bcc.Trim.Length > 0 Then .Bcc.Add(Bcc)
                If Cc.Trim.Length > 0 Then .CC.Add(Cc)
                .Subject = Subject
                If Attachment IsNot Nothing Then .Attachments.Add(New Attachment(Attachment, AttachmentName))
                .IsBodyHtml = True
                .Body = Message
            End With
            Dim r As New Random
            Dim userState As String = "ContactId" & r.Next(1, 1000000)
            AddHandler smtpServer.SendCompleted, AddressOf SendCompletedCallBack
            smtpServer.SendAsync(eMail, userState)
            'smtpServer.Send(eMail)

            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Private Sub SendCompletedCallBack(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
        'Get the unique identifier for this async operation
        Dim Token As String = e.UserState
        If e.Cancelled Then
            Dim ex As New Exception("[{0}] send cancelled" & Token)
            Throw ex
        End If
        If e.Error IsNot Nothing Then
            Dim LogFile As String = HttpContext.Current.Server.MapPath("\Extracts\Phone") & "EmailLog.txt"
            My.Computer.FileSystem.WriteAllText(LogFile, "Error: " & e.Error.ToString & "" & vbNewLine, True)
        End If
    End Sub
    'This function converts rtf to HTML
    
    Private Function ToMailMessage(ByVal Message As String) As MailMessage
        Dim html As String = Message
        If html IsNot Nothing Then
            Return LinkImages(html)
        End If
        Dim msg = New MailMessage
        msg.IsBodyHtml = True
        Return msg
    End Function
    Private Function LinkImages(ByVal html As String) As MailMessage
        Dim msg As New MailMessage
        msg.IsBodyHtml = True

        Dim matches = Regex.Matches(html, "img[^>]*?src\s*=\s*([""']?[^'""]+?['""])[^>]*?>", RegexOptions.IgnoreCase Or RegexOptions.IgnorePatternWhitespace Or RegexOptions.Multiline)

        Dim imgList = New List(Of LinkedResource)()
        Dim cid As Integer = 1

        For Each Match As Match In matches
            Dim src As String = Match.Groups(1).Value
            src = src.Trim(""""c)

            Dim myFile As String = Mid(src, 9, Len(src))
            If File.Exists(myFile) Then
                Dim ext As String = Path.GetExtension(src)
                If ext.Length > 0 Then
                    ext = ext.Substring(1)

                    Dim res = New LinkedResource(myFile)
                    With res
                        .ContentId = String.Format("img{0}.{1}", System.Math.Max(System.Threading.Interlocked.Increment(cid), cid - 1), ext)
                        .TransferEncoding = Net.Mime.TransferEncoding.Base64
                        .ContentType.MediaType = String.Format("image/{0}", ext)
                        .ContentType.Name = res.ContentId
                        imgList.Add(res)
                        src = String.Format("""cid:{0}""", res.ContentId)
                        html = html.Replace(Match.Groups(1).Value, src)
                    End With
                End If
            End If
        Next

        Dim view = AlternateView.CreateAlternateViewFromString(html, Nothing, "text/html")

        For Each img As LinkedResource In imgList
            view.LinkedResources.Add(img)
        Next
        msg.AlternateViews.Add(view)

        Return msg

    End Function
End Class
