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

Imports System
Imports System.Configuration.ConfigurationManager

Public Class AppConfiguration
    Public Shared ReadOnly Property SubmittedFolder() As String
        Get
            Dim result As String = Configuration.ConfigurationManager.AppSettings.Get("SubmittedFolder")
            If Not String.IsNullOrEmpty(result) Then
                Return result
            End If
            Throw New Exception("Appsetting SubmittedFolder not found in web.config file.")
        End Get
    End Property

    Public Shared ReadOnly Property UpdatedFolder() As String
        Get
            Dim result As String = Configuration.ConfigurationManager.AppSettings("UpdatedFolder")
            If Not String.IsNullOrEmpty(result) Then
                Return result
            End If
            Throw New Exception("AppSetting UpdatedFolder not found in web.config file.")
        End Get
    End Property

    Public Shared ReadOnly Property InsureeImageDelimiter() As String
        Get
            Dim result As String = Configuration.ConfigurationManager.AppSettings("InsureeImageDelimiter")
            If Not String.IsNullOrEmpty(result) Then
                Return result
            End If
            Throw New Exception("AppSetting InsureeImageDelimiter not found in web.config file.")
        End Get
    End Property
    Public Shared ReadOnly Property DefaultLimitation() As String
        Get
            Dim result As String = Configuration.ConfigurationManager.AppSettings("DefaultLimitation")
            If Not String.IsNullOrEmpty(result) Then
                Return result
            End If
            Throw New Exception("AppSetting InsureeImageDelimiter not found in web.config file.")
        End Get
    End Property
    Public Shared ReadOnly Property DefaultPriceOrigin() As String
        Get
            Dim result As String = Configuration.ConfigurationManager.AppSettings("DefaultPriceOrigin")
            If Not String.IsNullOrEmpty(result) Then
                Return result
            End If
            Throw New Exception("AppSetting InsureeImageDelimiter not found in web.config file.")
        End Get
    End Property

    Public Shared ReadOnly Property DefaultClaimRows() As Integer
        Get
            Dim result As Integer = Configuration.ConfigurationManager.AppSettings("DefaultClaimRows")
            If Not String.IsNullOrEmpty(result) Then
                Return result
            End If
            Throw New Exception("AppSetting DefaultClaimRows not found in web.config file.")
        End Get
    End Property

    Public Shared ReadOnly Property SMSGateway As String
        Get
            Dim Result As String = ConfigurationManager.AppSettings("SMSGateway")
            If Not String.IsNullOrEmpty(Result) Then
                Return Result
            End If
            Throw New Exception("AppSetting SMSGateway not found in web.config file.")
        End Get
    End Property

    Public Shared ReadOnly Property SMSGatewayUserName As String
        Get
            Dim Result As String = ConfigurationManager.AppSettings("SMSGatewayUserName")
            If Not String.IsNullOrEmpty(Result) Then
                Return Result
            End If
            Throw New Exception("AppSetting SMSGatewayUserName not found in web.config file.")
        End Get
    End Property

    Public Shared ReadOnly Property SMSGatewayPassword As String
        Get
            Dim Result As String = ConfigurationManager.AppSettings("SMSGatewayPassword")
            If Not String.IsNullOrEmpty(Result) Then
                Return Result
            End If
            Throw New Exception("AppSetting SMSGatewayPassword not found in web.config file.")
        End Get
    End Property

    Public Shared ReadOnly Property Host As String
        Get
            Dim Result As String = ConfigurationManager.AppSettings("Host")
            If Not String.IsNullOrEmpty(Result) Then
                Return Result
            End If
            Throw New Exception("AppSetting Host not found in web.config file.")
        End Get
    End Property

    Public Shared ReadOnly Property PasswordValidationMinLength As Integer
        Get
            Dim defaultValue = 1
            Dim value As Integer = defaultValue
            Try
                Dim textValue As String = ConfigurationManager.AppSettings("Password:MinLength")
                If Not String.IsNullOrEmpty(textValue) Then
                    value = Integer.Parse(textValue)
                    If value < 1 Then
                        value = defaultValue
                        EventLog.WriteEntry("IMIS",
                            "Error during processing the AppSetting 'Password:MinLength' occurred. 
                            Value need to be greater than or equal to 1 (Default value will be used).",
                            EventLogEntryType.Warning, 1)
                    End If
                End If
            Catch ex As Exception
                EventLog.WriteEntry("IMIS",
                    "Error during processing the AppSetting 'Password:MinLength' occurred (Default value will be used): " & ex.Message,
                    EventLogEntryType.Warning, 1)
            End Try
            Return value
        End Get
    End Property

    Public Shared ReadOnly Property PasswordValidationLowerCaseLetter As Integer
        Get
            Dim defaultValue = 0
            Dim value As Integer = defaultValue
            Try
                Dim textValue As String = ConfigurationManager.AppSettings("Password:LowerCaseLetter")
                If Not String.IsNullOrEmpty(textValue) Then
                    value = Integer.Parse(textValue)
                    If value <> 0 AndAlso value <> 1 Then
                        value = defaultValue
                        EventLog.WriteEntry("IMIS",
                            "Error during processing the AppSetting 'Password:LowerCaseLetter' occurred. 
                            Acceptable values: 0, 1 (Default value will be used).",
                            EventLogEntryType.Warning, 1)
                    End If
                End If
            Catch ex As Exception
                EventLog.WriteEntry("IMIS",
                    "Error during processing the AppSetting 'Password:LowerCaseLetter' occurred (Default value will be used): " & ex.Message,
                    EventLogEntryType.Warning, 1)
            End Try
            Return value
        End Get
    End Property

    Public Shared ReadOnly Property PasswordValidationUpperCaseLetter As Integer
        Get
            Dim defaultValue = 0
            Dim value As Integer = defaultValue
            Try
                Dim textValue As String = ConfigurationManager.AppSettings("Password:UpperCaseLetter")
                If Not String.IsNullOrEmpty(textValue) Then
                    value = Integer.Parse(textValue)
                    If value <> 0 AndAlso value <> 1 Then
                        value = defaultValue
                        EventLog.WriteEntry("IMIS",
                            "Error during processing the AppSetting 'Password:UpperCaseLetter' occurred. 
                            Acceptable values: 0, 1 (Default value will be used).",
                            EventLogEntryType.Warning, 1)
                    End If
                End If
            Catch ex As Exception
                EventLog.WriteEntry("IMIS",
                    "Error during processing the AppSetting 'Password:UpperCaseLetter' occurred (Default value will be used): " & ex.Message,
                    EventLogEntryType.Warning, 1)
            End Try
            Return value
        End Get
    End Property

    Public Shared ReadOnly Property PasswordValidationNumber As Integer
        Get
            Dim defaultValue = 0
            Dim value As Integer = defaultValue
            Try
                Dim textValue As String = ConfigurationManager.AppSettings("Password:Number")
                If Not String.IsNullOrEmpty(textValue) Then
                    value = Integer.Parse(textValue)
                    If value <> 0 AndAlso value <> 1 Then
                        value = defaultValue
                        EventLog.WriteEntry("IMIS",
                            "Error during processing the AppSetting 'Password:Number' occurred. 
                            Acceptable values: 0, 1 (Default value will be used).",
                            EventLogEntryType.Warning, 1)
                    End If
                End If
            Catch ex As Exception
                EventLog.WriteEntry("IMIS",
                    "Error during processing the AppSetting 'Password:Number' occurred (Default value will be used): " & ex.Message,
                    EventLogEntryType.Warning, 1)
            End Try
            Return value
        End Get
    End Property

    Public Shared ReadOnly Property PasswordValidationSpecialSymbol As Integer
        Get
            Dim defaultValue = 0
            Dim value As Integer = defaultValue
            Try
                Dim textValue As String = ConfigurationManager.AppSettings("Password:SpecialSymbol")
                If Not String.IsNullOrEmpty(textValue) Then
                    value = Integer.Parse(textValue)
                    If value <> 0 AndAlso value <> 1 Then
                        value = defaultValue
                        EventLog.WriteEntry("IMIS",
                            "Error during processing the AppSetting 'Password:SpecialSymbol' occurred. 
                            Acceptable values: 0, 1 (Default value will be used).",
                            EventLogEntryType.Warning, 1)
                    End If
                End If
            Catch ex As Exception
                EventLog.WriteEntry("IMIS",
                    "Error during processing the AppSetting 'Password:SpecialSymbol' occurred (Default value will be used): " & ex.Message,
                    EventLogEntryType.Warning, 1)
            End Try
            Return value
        End Get
    End Property

    Public Shared ReadOnly Property CommandTimeout As String
        Get
            Dim timeoutConfigValue As String = AppSettings("CommandTimeout")
            Dim parsedValue As Integer
            If Not String.IsNullOrEmpty(timeoutConfigValue) Then
                Try
                    parsedValue = Integer.Parse(timeoutConfigValue)
                    Return parsedValue
                Catch ex As Exception
                    Throw New Exception("Cannot parse string value to integer")
                End Try
            End If
            Throw New Exception("AppSetting CommandTimeout not found in web.config file.")
        End Get
    End Property

    Public Shared ReadOnly Property ClaimCodePrefix As String
        Get
            Dim claimCodePrefixValue As String = AppSettings("ClaimCodePrefix")
            If Not String.IsNullOrEmpty(claimCodePrefixValue) Then
                Return claimCodePrefixValue
            End If
            Throw New Exception("AppSetting ClaimCodePrefix not found in web.config file.")
        End Get
    End Property

End Class
