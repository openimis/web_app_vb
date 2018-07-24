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

Public Class eReports
    Private _DateFrom As Global.System.Nullable(Of Date)
    Private _DateTo As Global.System.Nullable(Of Date)
    Private _ProdId As Integer
    Private _HfId As Integer
    Private _CareType As Integer
    Private Shared _SubTitle As String


    Public Shared Property SubTitle() As String
        Get
            SubTitle = _SubTitle
        End Get
        Set(ByVal value As String)
            _SubTitle = value
        End Set
    End Property

    Public Property DateFrom() As Global.System.Nullable(Of Date)
        Get
            Return _DateFrom
        End Get
        Set(ByVal value As Global.System.Nullable(Of Date))
            _DateFrom = value
        End Set
    End Property
    Public Property DateTo() As Global.System.Nullable(Of Date)
        Get
            Return _DateTo
        End Get
        Set(ByVal value As Global.System.Nullable(Of Date))
            _DateTo = value
        End Set
    End Property
    Public Property ProdId() As Integer
        Get
            Return _ProdId
        End Get
        Set(ByVal value As Integer)
            _ProdId = value
        End Set
    End Property
    Public Property HFId() As Integer
        Get
            Return _HfId
        End Get
        Set(ByVal value As Integer)
            _HfId = value
        End Set
    End Property
    Public Property CareType() As String
        Get
            Return _HfId
        End Get
        Set(ByVal value As String)
            _HfId = value
        End Set
    End Property

    Public Shared Property Level1 As String
    Public Shared Property Sublevel1 As String
    Public Shared Property Level2 As String
    Public Shared Property Sublevel2 As String
    Public Shared Property Level3 As String
    Public Shared Property Sublevel3 As String
    Public Shared Property Level4 As String
    Public Shared Property Sublevel4 As String
    Public Shared Property ShareContribution As Double
    Public Shared Property WeightPopulation As Double
    Public Shared Property WeightNumberFamilies As Double
    Public Shared Property WeightInsuredPopulation As Double
    Public Shared Property WeightNumberInsuredFamilies As Double
    Public Shared Property WeightAdjustedAmount As Double
    Public Shared Property WeightNumberVisits As Double
End Class
