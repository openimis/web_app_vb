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

Public Class PolicyBI

    Public Sub GetFamilyHeadInfo(ByVal eFamily As IMIS_EN.tblFamilies)
        Dim getHeadInfo As New IMIS_BL.FamilyBL
        getHeadInfo.GetFamilyHeadInfo(eFamily)
    End Sub
    Public Sub getPolicyValue(ByRef ePolicy As IMIS_EN.tblPolicy, PreviousPolicyId As Integer)
        Dim Policy As New IMIS_BL.PolicyBL
        Policy.getPolicyValue(ePolicy, PreviousPolicyId)
    End Sub
    Public Function GetOfficers(ByVal DistrictId As Integer, Optional ByVal showselect As Boolean = False, Optional ByVal VillageId As Integer = 0, Optional EnrolmentDate As Date? = Nothing) As DataTable
        Dim Officer As New IMIS_BL.OfficersBL
        Return Officer.GetOfficers(DistrictId, showselect, VillageId, EnrolmentDate)
    End Function
    Public Function GetProducts(ByVal UserId As Integer, Optional ByVal ShowSelect As Boolean = False, Optional ByVal RegionId As Integer = 0, Optional ByVal DistrictID As Integer = 0, Optional ByVal ByDate As Date? = Nothing) As DataTable
        Dim Products As New IMIS_BL.ProductsBL
        Return Products.GetProducts(UserId, ShowSelect, RegionId, DistrictID, ByDate)
    End Function
    Public Sub LoadPolicy(ByRef ePolicy As IMIS_EN.tblPolicy, ByRef PremiumPaid As Decimal)
        Dim Policy As New IMIS_BL.PolicyBL
        Policy.LoadPolicy(ePolicy, PremiumPaid)
    End Sub


    Public Function SavePolicy(ByRef ePolicy As IMIS_EN.tblPolicy, Optional ByVal IsOffLine As Boolean = False) As Integer
        Dim Policy As New IMIS_BL.PolicyBL
        Return Policy.SavePolicy(ePolicy, IsOffLine)
    End Function

    Public Sub LoadPolicyDedRem(ByRef eclaimDedRem As IMIS_EN.tblClaimDedRem)
        Dim policy As New IMIS_BL.ClaimDedRemBL
        policy.LoadPolicyDedRem(eclaimDedRem)
    End Sub
    Public Function ReturnPolicyStatus(ByVal PolicyStatus As Integer) As String
        Dim Policy As New IMIS_BL.PolicyBL
        Return Policy.ReturnPolicyStatus(PolicyStatus)
    End Function
 
    Public Function isExceededAdherents(ByVal ProdId As Integer, ByVal FamilyId As Integer) As Boolean
        Dim Pol As New IMIS_BL.PolicyBL
        Return Pol.isExceededAdherents(ProdId, FamilyId)
    End Function
    Public Function IsRenewalLate(ByVal PolicyID As Integer, ByVal EnrolDate As Date) As Boolean
        Dim BLPolicy As New IMIS_BL.PolicyBL
        Return BLPolicy.IsRenewalLate(PolicyID, EnrolDate)
    End Function
    Public Function GetProductAdministrationPeriod(ByVal ProdId As Integer) As Integer
        Dim Prod As New IMIS_BL.ProductsBL
        Return Prod.GetProductAdministrationPeriod(ProdId)
    End Function
    Public Sub GetProductDetails(eProducts As IMIS_EN.tblProduct)
        Dim Prod As New IMIS_BL.ProductsBL
        Prod.LoadProduct(eProducts)
    End Sub
    Public Function getProductDetailMin(ProdId As Integer) As IMIS_EN.tblProduct
        Dim prod As New IMIS_BL.ProductsBL
        Return prod.getProductDetailMin(ProdId)
    End Function
    Public Function getEnrollmentOfficerMoved(ByVal OfficerID As Integer) As IMIS_EN.tblOfficer
        Dim BL As New IMIS_BL.OfficersBL
        Return BL.getEnrollmentOfficerMoved(OfficerID)
    End Function
    Public Function GetPeriodForPolicy(ByVal ProdId As Integer, ByVal EnrolDate As Date, Optional ByRef HasCycle As Boolean = False, Optional PolicyStage As String = "N") As DataTable
        Dim BL As New IMIS_BL.ProductsBL
        Return BL.GetPeriodForPolicy(ProdId, EnrolDate, HasCycle, PolicyStage)
    End Function
    Public Function GetProductForRenewal(ProdId As Integer, EnrollDate As Date) As IMIS_EN.tblProduct
        Dim Prod As New IMIS_BL.ProductsBL
        Return Prod.GetProductForRenewal(ProdId, EnrollDate)
    End Function
    Public Function GetPolicyIdByUUID(ByVal uuid As Guid) As Integer
        Dim Policy As New IMIS_BL.PolicyBL
        Return Policy.GetPolicyIdByUUID(uuid)
    End Function
    Public Function GetPolicyUUIDByID(ByVal id As Integer) As Guid
        Dim Policy As New IMIS_BL.PolicyBL
        Return Policy.GetPolicyUUIDByID(id)
    End Function
    Public Function GetRenewalCount(ByVal ProdID As Integer, ByVal FamilyID As Integer) As Integer
        Dim PolicyBL As New IMIS_BL.PolicyBL
        Return PolicyBL.GetRenewalCount(ProdID, FamilyID)
    End Function
End Class
