Imports System.Net
Imports System.IO
Imports System.Xml

Public Class EscapeBL
    Public Function isValidInsuranceNumber(ByVal InsuranceNumber As String) As Boolean
        If Not InsuranceNumber.ToString.Length = 9 Then Return False
        Dim n As String = Left(InsuranceNumber.ToString, 8)
        Dim Checksum As String = Right(InsuranceNumber.ToString, 1)
        If CInt(n) = Checksum And Checksum = 0 Then Return False
        If Checksum = n - (Int(n / 7) * 7) Then Return True
        Return False
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
End Class
