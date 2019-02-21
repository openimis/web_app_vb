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

Public Class FindPriceListMSBI
    Public UserRights As New IMIS_BL.UsersBL
    Public Function checkRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean
        Return UserRights.CheckRights(Right, UserID)
    End Function
    Public Function GetPriceListMS(ByVal ePL As IMIS_EN.tblPLServices, Optional ByVal All As Boolean = False) As DataTable
        Dim getDataTable As New IMIS_BL.PricelistMSBL
        Return getDataTable.GetPriceListMS(ePL, All)
    End Function
    Public Function GetDistricts(ByVal userId As Integer, Optional ByVal showSelect As Boolean = False, Optional ByVal RegionId As Integer = 0) As DataTable
        Dim Districts As New IMIS_BL.LocationsBL
        Return Districts.GetDistricts(userId, showSelect, RegionId)
    End Function


    Public Sub SavePricelist(ByRef ePLServices As IMIS_EN.tblPLServices)
        Dim SaveData As New IMIS_BL.PricelistMSBL
        SaveData.SavePriceListMS(ePLServices)
    End Sub
    Public Function DuplicatePriceListMI(ByVal PLServiceId As Integer) As Integer
        Dim getDataTable As New IMIS_BL.PricelistMSBL
        ' Return getDataTable.GetProducts
        Return 0
    End Function
    Public Sub DeletePriceListMS(ByRef ePLServices As IMIS_EN.tblPLServices)
        Dim PLMS As New IMIS_BL.PricelistMSBL
        PLMS.DeletePriceListMS(ePLServices)
    End Sub

    'Public Function checkRoles(ByVal Role As IMIS_EN.Enums.Rights, ByVal roleid As Integer) As Boolean
    '    Dim roles As New IMIS_BL.UsersBL
    '    Return (roles.CheckRoles(Role, roleid))
    'End Function
    Public Function GetRegions(UserId As Integer, Optional ShowSelect As Boolean = True, Optional ByVal IncludeNational As Boolean = False) As DataTable
        Dim BL As New IMIS_BL.LocationsBL
        Return BL.GetRegions(UserId, ShowSelect:=ShowSelect, IncludeNational:=IncludeNational)
    End Function
End Class
