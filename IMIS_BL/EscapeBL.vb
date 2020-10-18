Imports System.Net
Imports System.IO
Imports System.Xml
Imports System.Web
Imports System.Web.Services
Imports Newtonsoft.Json
Imports IMIS_DAL

#Const CHF = True
Public Class EscapeBL
    Public Function isValidInsuranceNumber(ByVal InsuranceNumber As String) As Boolean
#If CHF Then
        If Not InsuranceNumber.ToString.Length = 9 Then Return False
        Dim n As String = Left(InsuranceNumber.ToString, 8)
        Dim Checksum As String = Right(InsuranceNumber.ToString, 1)
        If CInt(n) = Checksum And Checksum = 0 Then Return False
        If Checksum = n - (Int(n / 7) * 7) Then Return True
        Return False
#Else
        Return True
#End If
    End Function
    Public Function getActivationOption() As Integer
        Dim IMISDefaults As New IMISDefaultsDAL
        Return IMISDefaults.getActivationOption()
    End Function
    Public Function MatchPayments(DomainUrl As String) As Boolean
#If CHF Then
        Dim webClient As New WebClient()
        Dim resByte As Byte()
        Dim resString As String
        Dim reqString() As Byte
        Dim MatchApi As String = DomainUrl & "restapi/api/webmatchpayment" 'HttpContext.Current.Request.Url.AbsolutePath & "restapi/api/matchpayment"

        'MatchApi = "http://localhost:63401/api/webmatchpayment"
        Dim dt As New Dictionary(Of String, Object)
        dt.Add("internal_identifier", DBNull.Value)
        dt.Add("audit_user_id", "0")
        dt.Add("api_key", "Xdfg8796021ff89Df4654jfjHeHidas987vsdg97e54ggdfHjdt")

        Try
            webClient.Headers("content-type") = "application/json"
            webClient.Headers("User-Agent") = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; EasyBits GO v1.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; Tablet PC 2.0; InfoPath.3)"
            webClient.Proxy = WebRequest.DefaultWebProxy
            reqString = Encoding.Default.GetBytes(JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented))
            resByte = webClient.UploadData(MatchApi, "post", reqString)
            resString = Encoding.Default.GetString(resByte)


            'Return resString
            Return True
        Catch ex As WebException
            Dim res As WebResponse = ex.Response
            Dim sr As New IO.StreamReader(res.GetResponseStream)
            Dim geterror As String = sr.ReadToEnd()
            Debug.Print(geterror)
            Throw ex
        Finally
            webClient.Dispose()
        End Try
        Return False
#Else
        Return True
#End If
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

    Public Function CheckConfiguration() As String
        Dim configInfo As String = ""
#If CHF Then
        configInfo += checkAccCodePremiumsConfig()
#End If
        Return configInfo
    End Function

    Private Function checkAccCodePremiumsConfig() As String
        Dim getDataTable As New IMIS_BL.ProductsBL
        Dim productDAL As New IMIS_DAL.ProductsDAL
        Dim eLocations As New IMIS_EN.tblLocations
        Dim EpRODUCTS As New IMIS_EN.tblProduct

        eLocations.RegionId = 0
        eLocations.DistrictId = 0
        EpRODUCTS.tblLocation = eLocations

        Dim dtProducts As DataTable = getDataTable.GetProducts(EpRODUCTS, False)
        Dim dv As DataView = dtProducts.DefaultView

        dv.RowFilter = "DateTo > #" + DateTime.Today.ToShortTimeString + "#" ' and AccCodePremiums=''"
        Dim productsNoAccCode = ""
        For Each row As DataRow In dv.ToTable.Rows
            Dim accCode As String = productDAL.GetProductName_Account(row.Item("ProdID")).Rows(0).Item("AccCodePremiums")
            If accCode Is "" Or accCode Is Nothing Then
                productsNoAccCode = productsNoAccCode + " " + row.Item("ProductName") + "(" + row.Item("ProductCode") + ") " + Environment.NewLine
            End If
        Next row

        Return If(productsNoAccCode.Equals(""), "", "Products without AccCodePremium: " + Environment.NewLine & productsNoAccCode)
    End Function

End Class