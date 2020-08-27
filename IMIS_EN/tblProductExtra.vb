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

Partial Public Class tblProduct
    Public Property tblLocation As tblLocations
    Public Property RegionId As Integer?
    Public Property DistrictId As Integer?
    Public Property Level1 As String 
    Public Property Sublevel1 As String
    Public Property Level2 As String
    Public Property Sublevel2 As String
    Public Property Level3 As String
    Public Property Sublevel3 As String
    Public Property Level4 As String
    Public Property Sublevel4 As String
    Public Property ShareContribution As Double
    Public Property WeightPopulation As Double
    Public Property WeightNumberFamilies As Double
    Public Property WeightInsuredPopulation As Double
    Public Property WeightNumberInsuredFamilies As Double
    Public Property WeightNumberVisits As Double
    Public Property WeightAdjustedAmount As Double

    Private _Recurrence As Integer?
    Public Property Recurrence() As Integer?
        Get
            Return _Recurrence
        End Get
        Set(ByVal value As Integer?)
            _Recurrence = value
        End Set
    End Property

End Class
