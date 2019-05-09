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

Public Class General
    Public Shared Function getControlSetting(ControlName As String) As String
        Dim dtControls As DataTable = HttpContext.Current.Session("Controls")

        If dtControls Is Nothing Then Return "O"
        Dim dr As DataRow() = dtControls.Select("FieldName = '" & ControlName & "'")

        If dr Is Nothing Or dr.Count = 0 Then Return "O"

        Return dr(0)("Adjustibility").ToString.ToUpper

    End Function

    Public Shared Function isValidPassword(password As String) As Boolean
        Dim minLength As Integer = IMIS_EN.AppConfiguration.PasswordValidationMinLength
        Dim lowerCaseLetterCheck As Integer = IMIS_EN.AppConfiguration.PasswordValidationLowerCaseLetter
        Dim upperCaseLetterCheck As Integer = IMIS_EN.AppConfiguration.PasswordValidationUpperCaseLetter
        Dim numberCheck As Integer = IMIS_EN.AppConfiguration.PasswordValidationNumber
        Dim specialSymbolCheck As Integer = IMIS_EN.AppConfiguration.PasswordValidationSpecialSymbol

        Dim expression As String = "^"
        If minLength Then
            expression += "(?=.{" & minLength.ToString & "})"
        End If
        If lowerCaseLetterCheck Then
            expression += "(?=[^a-z]*[a-z])"
        End If
        If upperCaseLetterCheck Then
            expression += "(?=[^A-Z]*[A-Z])"
        End If
        If numberCheck Then
            expression += "(?=[^\d]*[\d])"
        End If
        If specialSymbolCheck Then
            expression += "(?=[^\W]*[\W])"
        End If
        expression += "[A-Za-z0-9\W]+$"

        Return Regex.IsMatch(password, expression)
    End Function

    Public Shared Function getInvalidPasswordMessage() As String
        Dim imisgen As New IMIS_Gen
        Dim listOfCondition As List(Of String) = New List(Of String)
        Dim minLength As Integer = IMIS_EN.AppConfiguration.PasswordValidationMinLength
        Dim lowerCaseLetterCheck As Integer = IMIS_EN.AppConfiguration.PasswordValidationLowerCaseLetter
        Dim upperCaseLetterCheck As Integer = IMIS_EN.AppConfiguration.PasswordValidationUpperCaseLetter
        Dim numberCheck As Integer = IMIS_EN.AppConfiguration.PasswordValidationNumber
        Dim specialSymbolCheck As Integer = IMIS_EN.AppConfiguration.PasswordValidationSpecialSymbol

        If minLength Then
            listOfCondition.Add(imisgen.getMessage("V_PASSWORD_MIN_LENGTH_PREFIX") & " " &
                                minLength.ToString & " " & imisgen.getMessage("V_PASSWORD_MIN_LENGTH_SUFFIX"))
        End If
        If lowerCaseLetterCheck Then
            listOfCondition.Add(imisgen.getMessage("V_PASSWORD_LOWER_CASE"))
        End If
        If upperCaseLetterCheck Then
            listOfCondition.Add(imisgen.getMessage("V_PASSWORD_UPPER_CASE"))
        End If
        If numberCheck Then
            listOfCondition.Add(imisgen.getMessage("V_PASSWORD_NUMBER"))
        End If
        If specialSymbolCheck Then
            listOfCondition.Add(imisgen.getMessage("V_PASSWORD_SPECIAL_SYMBOL"))
        End If
        If listOfCondition.Count = 0 Then
            listOfCondition.Add(imisgen.getMessage("V_PASSWORD_MIN_LENGTH_PREFIX") &
                                " 1 " & imisgen.getMessage("V_PASSWORD_MIN_LENGTH_SUFFIX"))
        End If

        Dim result As String = imisgen.getMessage("V_PASSWORD_PREFIX") & " "
        result += String.Join(", ", listOfCondition)

        Return result
    End Function
End Class
