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

Imports System.Net
Imports System.IO
Imports System.Xml
Imports System.Web
Imports System.Web.Services
Imports Newtonsoft.Json
Imports IMIS_DAL


Public Class EscapeBL
    Public Function isValidInsuranceNumber(ByVal InsuranceNumber As String) As Boolean
#If CHF Then
        If Not InsuranceNumber.ToString.Length = 9 Then Return False
        Dim n As String = Left(InsuranceNumber.ToString, 8)
        Dim Checksum As String = Right(InsuranceNumber.ToString, 1)
        If CInt(n) = Checksum And Checksum = 0 Then Return False
        If Checksum = n - (Int(n / 7) * 7) Then Return True
        Return False
#ElseIf BEPHA Then
        Return InsuranceNumber.Length.Equals(11)
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
        dt.Add("internal_identifier", "0")
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

    Public Function CheckConfiguration(ByRef configDict As Dictionary(Of String, Object)) As String
        Dim configInfo As String = ""
#If CHF Then
        configInfo += checkAccCodePremiumsConfig(CType(configDict.Item("eUser"), IMIS_EN.tblUsers))
#End If
        Return configInfo
    End Function

    Private Function checkAccCodePremiumsConfig(eUser As IMIS_EN.tblUsers) As String
        If (Not haveProductRights(eUser)) Then
            Return ""
        End If
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

    Private Function haveProductRights(eUser As IMIS_EN.tblUsers)
        Dim UserRights As New IMIS_BL.UsersBL
        Dim requiredRights = {
            IMIS_EN.Enums.Rights.ProductAdd,
            IMIS_EN.Enums.Rights.ProductEdit,
            IMIS_EN.Enums.Rights.ProductDuplicate,
            IMIS_EN.Enums.Rights.ProductDelete
        }
        Dim Right As Integer

        For Each Right In requiredRights
            If Not UserRights.CheckRights(Right, eUser.UserID) Then
                Return False
            End If
        Next
        Return True
    End Function
End Class