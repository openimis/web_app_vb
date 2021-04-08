Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Script.Serialization

Public Class ModularBatchProcess
    Dim client As HttpClient
    Dim modularUrl As String
    Dim jsonSerializer As JavaScriptSerializer

    Public Sub New()
        client = New HttpClient()
        client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        modularUrl = System.Web.Configuration.WebConfigurationManager.AppSettings("ModularGQLSource").ToString()
        jsonSerializer = New JavaScriptSerializer()
    End Sub

    Public Function sendBatchRunGQLRequest(ByVal year As Integer, ByVal month As Integer, ByVal locationId As Integer, ByVal senderLogin As String) As MutationNode
        Dim manager = ServicePointManager.ServerCertificateValidationCallback
        Dim output As MutationNode = Nothing
        Try
            ServicePointManager.ServerCertificateValidationCallback = Function(s, c, h, e) True
            Dim clientMutationUUID As String = Guid.NewGuid().ToString()

            Dim requestContent As String = buildRequestContent(year, month, locationId, clientMutationUUID)
            Dim result = sendRequest(requestContent, senderLogin)

            Dim data = jsonSerializer.Deserialize(Of BatchRunMutationResponse)(result.Content.ReadAsStringAsync().Result)
            Dim internal_client_uuid As String = data.data.processBatch.internalId

            requestContent = buildCheckMutationContent(internal_client_uuid)
            result = sendRequest(requestContent, senderLogin)

            Dim batchProcessResult = jsonSerializer.Deserialize(Of MutationLogResponseData)(result.Content.ReadAsStringAsync().Result)

            If batchProcessResult.data.mutationLogs.edges.Count > 0 Then
                output = batchProcessResult.data.mutationLogs.edges(0).node
            End If
        Finally
            ServicePointManager.ServerCertificateValidationCallback = manager
        End Try
        Return output
    End Function

    Private Function buildCheckMutationContent(internalClientUUID As String) As String
        Return String.Format("
            query  {{
                mutationLogs(id: ""{0}""){{
                pageInfo {{ hasNextPage, hasPreviousPage, startCursor, endCursor}}
                edges {{
                    node {{
                        id,status,error,
                        clientMutationId,clientMutationLabel,
                        clientMutationDetails,requestDateTime
                    }}
                }}
                }}
            }}", internalClientUUID)
    End Function

    Private Function buildBatchMuatitonFrame(ByVal frameContent As String) As HttpRequestMessage
        Dim requestFrame As HttpRequestMessage = New HttpRequestMessage()
        requestFrame.Method = HttpMethod.Post
        requestFrame.RequestUri = New Uri(modularUrl)

        Dim content = New StringContent(frameContent)
        requestFrame.Content = content
        requestFrame.Content.Headers.ContentType = New MediaTypeHeaderValue("application/json")
        Return requestFrame
    End Function

    Private Function sendRequest(requestContent As String, senderLogin As String) As HttpResponseMessage
        Dim requestBody = String.Format("{{""query"": {0}}}", HttpUtility.JavaScriptStringEncode(requestContent, True))
        Dim requestFrame As HttpRequestMessage = buildBatchMuatitonFrame(requestBody)
        authenticateFrame(requestFrame, senderLogin)
        Dim result = client.SendAsync(requestFrame).Result
        Return result
    End Function
    Protected Function authenticateFrame(ByRef requestFrame As HttpRequestMessage, ByVal userName As String)
        requestFrame.Headers.Add("remote-user", userName)
    End Function

    Private Function buildRequestContent(ByVal year As Integer, ByVal month As Integer, ByVal locationId As Integer, ByVal clientMutationUUID As String) As String
        Dim inputData = String.Format("
                clientMutationId: ""{0}""          
                clientMutationLabel: ""Batch Process - {1}, {2} {3}""" + vbLf, clientMutationUUID, locationId, month, year)

        If year > 0 Then
            inputData += String.Format("year: {0}" + vbLf, year)
        End If
        If month > 0 Then
            inputData += String.Format("month: {0}" + vbLf, month)
        End If
        If locationId > 0 Then
            inputData += String.Format("locationId: {0}" + vbLf, locationId)
        End If

        Dim requestContent As String = String.Format("
        mutation {{      
                    processBatch(        
                        input: {{                  
                            {0}        
                        }}      
                    ) 
                    {{        
                        clientMutationId        
                        internalId      
                    }}
            }}", inputData)

        Return requestContent
    End Function

    Private Class BatchRunMutationResponse
        Public Property data As MutationReponseData
    End Class
    Private Class MutationReponseData
        Public Property processBatch As ProcessBatchResponseData
    End Class
    Private Class ProcessBatchResponseData
        Public Property clientMutationId As String
        Public Property internalId As String
    End Class

    Private Class MutationLogResponseData
        Public Property data As MutationLogs
    End Class
    Private Class MutationLogs
        Public Property mutationLogs As MutationLog
    End Class
    Private Class MutationLog
        Public Property edges As List(Of MutationEdge)
    End Class
    Private Class MutationEdge
        Public Property node As MutationNode
    End Class
    Public Class MutationNode
        Public Property id As String
        Public Property status As Integer
        Public Property [error] As String
        Public Property clientMutationId As String
        Public Property clientMutationLabel As String
        Public Property clientMutationDetails As String
        Public Property requestDateTime As String
    End Class
End Class
