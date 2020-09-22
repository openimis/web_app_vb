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
Imports System.Net
Imports System.IO
Imports System.Xml
Imports IMIS_DAL

Public Class EscapeBL
    Public Function isValidInsuranceNumber(ByVal InsuranceNumber As String) As Boolean
        Return True
    End Function
    Public Function SendSMS(ByVal Message As String) As String
        Dim Response As String = ""

        Dim Req As HttpWebRequest = Nothing
        Dim res As HttpWebResponse

        Dim URL As String = IMIS_EN.AppConfiguration.SMSGateway
        Dim UserName As String = IMIS_EN.AppConfiguration.SMSGatewayUserName
        Dim UserPassword As String = IMIS_EN.AppConfiguration.SMSGatewayPassword

        Dim Byt As Byte() = Encoding.UTF8.GetBytes(String.Format("{0}:{1}", UserName, UserPassword))
        Dim xmlBytes As Byte() = Encoding.UTF8.GetBytes(Message.ToString)

        Dim base64 As String = Convert.ToBase64String(Byt)
        Req = HttpWebRequest.Create(URL)
        With Req
            .ContentType = "application/xml"
            .Headers.Add(HttpRequestHeader.Authorization, "Basic " & base64)
            .Method = "POST"
            .ContentLength = xmlBytes.Length
        End With

        Dim DataStream As Stream = Req.GetRequestStream
        DataStream.Write(xmlBytes, 0, xmlBytes.Length)
        DataStream.Close()

        res = Req.GetResponse
        Response = String.Format("{0} {1}", res.StatusCode, res.StatusDescription)

        DataStream = res.GetResponseStream
        Dim Reader As New StreamReader(DataStream)
        Dim ResponseFromServer As String = Reader.ReadToEnd

        Dim BulkSMSId As String = ""

        Using r As XmlReader = XmlReader.Create(New StringReader(ResponseFromServer))
            r.ReadToFollowing("bulk_id")
            BulkSMSId = r.ReadElementContentAsString()
        End Using

        Response += " Bulk Id : " & BulkSMSId

        Reader.Close()
        DataStream.Close()
        res.Close()

        Return BulkSMSId

    End Function

    Public Function getActivationOption() As Integer
        Dim IMISDefaults As New IMISDefaultsDAL

        Return IMISDefaults.getActivationOption()
    End Function

    Public Function MatchPayments(DomainUrl As String) As Boolean
        Return True
    End Function

End Class
