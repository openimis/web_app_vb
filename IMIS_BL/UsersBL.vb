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

Imports IMIS_EN.Enums

Public Class UsersBL
    Private imisgen As New GeneralBL
    Private dtUserRights As DataTable = Nothing
    Private Function CheckUserRights(UserID As Integer, rightID As Integer, Optional level As Integer = 2) As Boolean
        If dtUserRights Is Nothing Then
            Dim DAL As New IMIS_DAL.UsersDAL
            dtUserRights = DAL.GetUserRights(UserID)
        End If
        If level = 0 Then
            For Each row As DataRow In dtUserRights.Rows
                Dim intRight As Integer = row("RightID")
                If Left(intRight, 2) = Left(rightID, 2) Then
                    Return True
                End If
            Next
        End If
        If level = 1 Then
            For Each row As DataRow In dtUserRights.Rows
                Dim intRight As Integer = row("RightID")
                If Left(intRight, 4) = Left(rightID, 4) Then
                    Return True
                End If
            Next
        End If
        If dtUserRights.Rows.Count > 0 Then
            Dim dr() As DataRow = dtUserRights.Select("RightID=" & rightID, "")
            If dr.Count > 0 Then Return True
        End If


        Return False
    End Function



#Region "Security"
    'Page is an Enum containing name identifiers of all pages in the IMIS.
    'Each Page in IMIS has a set of Rights called PageRights.
    'Access to any IMIS page, is provided only when one of the rights in the set PageRights is valid for the user accessing that page.
    'Checking of whether access is to the IMIS page is granted or not, is done in the [ CheckRoles ] function, which maps the right and role on 'AND' operation.
    'When access is granted, still the function RunPageSecurity [ inside IMIS page ] will have to check which exactly level of access has been granted to the user.
    'The Security Function call stack is as mentioned below:
    '1. RunPageSecurity [ inside IMIS page( code behind file ) ] ---- calls RunPageSecurity [ inside USERBL class ] 
    '2a) RunPageSecurity [ inside USERBL class ]  ---- calls GetPageRights [ inside USERBL class ]
    '2b) RunPageSecurity [ inside USERBL class ] ---- calls CheckRights[ inside USERBL class ]
    Public Function CheckRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean

        Select Case Right
            'FAMILY OR GROUP
            Case IMIS_EN.Enums.Rights.Family : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FamilySearch : Return CheckUserRights(UserID, Right)''(Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And UserID)
            Case IMIS_EN.Enums.Rights.FamilyAdd : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.FamilyEdit : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.FamilyDelete : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)

            'INSUREE
            Case IMIS_EN.Enums.Rights.Insuree : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.InsureeSearch : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And UserID)
            Case IMIS_EN.Enums.Rights.InsureeAdd : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.InsureeEdit : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.InsureeDelete : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.InsureeEnquire : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)

            'POLICY
            Case IMIS_EN.Enums.Rights.PolicySearch : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And UserID)
            Case IMIS_EN.Enums.Rights.PolicyAdd : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.PolicyEdit : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.PolicyDelete : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
                '
            Case IMIS_EN.Enums.Rights.PolicyRenew : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)   'NOT PART OF THE NEW DOCUMENT

            'CONTRIBUTION
            Case IMIS_EN.Enums.Rights.ContributionSearch : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk And UserID) 
            Case IMIS_EN.Enums.Rights.ContributionAdd : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.ContributionEdit : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.ContributionDelete : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)

            'PAYMENT
            Case IMIS_EN.Enums.Rights.PaymentSearch : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk And UserID) 
            Case IMIS_EN.Enums.Rights.PaymentAdd : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.PaymentEdit : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.PaymentDelete : Return CheckUserRights(UserID, Right)

            'CLAIMS
            Case IMIS_EN.Enums.Rights.Claims : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.ClaimSearch : Return CheckUserRights(UserID, Right)'(Roles.ClaimAdministrator + Roles.ClaimContributor + Roles.CHFMedicalOfficer And UserID) 
            Case IMIS_EN.Enums.Rights.ClaimAdd : Return CheckUserRights(UserID, Right)'(Roles.ClaimAdministrator + Roles.CHFClerk + Roles.ClaimContributor And UserID)

            Case IMIS_EN.Enums.Rights.ClaimDelete : Return CheckUserRights(UserID, Right)'(Roles.ClaimAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ClaimLoad : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk + Roles.ClaimAdministrator + Roles.ClaimContributor And UserID)
            Case IMIS_EN.Enums.Rights.ClaimPrint : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.ClaimSubmit : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.ClaimReview : Return CheckUserRights(UserID, Right)'(Roles.CHFMedicalOfficer And UserID)
            Case IMIS_EN.Enums.Rights.ClaimFeedback : Return CheckUserRights(UserID, Right)'(Roles.CHFMedicalOfficer And UserID)                

            Case IMIS_EN.Enums.Rights.ClaimUpdate : Return CheckUserRights(UserID, Right)'(Roles.CHFMedicalOfficer And UserID)
            Case IMIS_EN.Enums.Rights.ClaimProcess : Return CheckUserRights(UserID, Right)'(Roles.CHFMedicalOfficer And UserID)  
            Case IMIS_EN.Enums.Rights.ClaimRestore : Return CheckUserRights(UserID, Right)

            'BATCH
            Case IMIS_EN.Enums.Rights.Batch : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.BatchProcess : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)  Changed from no mapping to CHFAccount because ParentNode ValueClaim mapped to that user, so child also follows
            Case IMIS_EN.Enums.Rights.BatchFilter : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)   Changed from no mapping to CHFAccount because ParentNode ValueClaim mapped to that user, so child also follows
            Case IMIS_EN.Enums.Rights.BatchPreview : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)  Changed from no mapping to CHFAccount because ParentNode ValueClaim mapped to that user, so child also follows

            'ADMINISTRATION 
            Case IMIS_EN.Enums.Rights.Product : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.ProductSearch : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ProductAdd : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ProductEdit : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ProductDelete : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID) 
            Case IMIS_EN.Enums.Rights.ProductDuplicate : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.HealthFacility : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.HealthFacilitySearch : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.HealthFacilityAdd : Return CheckUserRights(UserID, Right)' (Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.HealthFacilityEdit : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.HealthFacilityDelete : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID) 

            Case IMIS_EN.Enums.Rights.PriceListMedicalServices : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindPriceListMedicalServices : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddPriceListMedicalServices : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditPriceListMedicalServices : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeletePriceListMedicalServices : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DuplicatePriceListMedicalServices : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.PriceListMedicalItems : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindPriceListMedicalItems : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddPriceListMedicalItems : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditPriceListMedicalItems : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeletePriceListMedicalItems : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DuplicatePriceListMedicalItems : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.MedicalService : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindMedicalService : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddMedicalService : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditMedicalService : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteMedicalService : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.MedicalItem : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindMedicalItem : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddMedicalItem : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditMedicalItem : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteMedicalItem : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.Officer : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindOfficer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddOfficer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditOfficer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteOfficer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.ClaimAdministrator : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindClaimAdministrator : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddClaimAdministrator : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditClaimAdministrator : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteClaimAdministrator : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.Users : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.UsersSearch : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.UsersAdd : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.UsersEdit : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.UsersDelete : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.Payer : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindPayer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddPayer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditPayer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeletePayer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID) 

            Case IMIS_EN.Enums.Rights.Locations : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindLocations : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID) 
            Case IMIS_EN.Enums.Rights.AddLocations : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditLocations : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteLocations : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.MoveLocations : Return CheckUserRights(UserID, Right)

            Case IMIS_EN.Enums.Rights.userProfiles : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindUserProfile : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.AddUserProfile : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.EditUserProfile : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.DeleteUserProfile : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.DuplicateUserProfile : Return CheckUserRights(UserID, Right)
           'TOOLS


           'REGISTERS
            Case IMIS_EN.Enums.Rights.Registers : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.DiagnosesUpload : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.DiagnosesDownload : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.HealthFacilitiesUpload : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.HealthFacilitiesDownload : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.LocationUpload : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.LocationDonwload : Return CheckUserRights(UserID, Right)

            'EXTRACT
            Case IMIS_EN.Enums.Rights.Extracts : Return CheckUserRights(UserID, Right, 1)'(Roles.HFAdministrator + Roles.CHFAdministrator + Roles.OfflineCHFAdministrator And RoleId)    NOT IN THE NEW DOCUMENT
            Case IMIS_EN.Enums.Rights.ExtractMasterDataDownload : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.ExtractPhoneExtractsCreate : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.ExtractOfflineExtractCreate : Return CheckUserRights(UserID, Right)'(Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ExtractClaimUpload : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.ExtractEnrolmentsUpload : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.ExtractFeedbackUpload : Return CheckUserRights(UserID, Right)

            'REPORTS 
            Case IMIS_EN.Enums.Rights.Reports : Return CheckUserRights(UserID, Right, 1)'(Roles.CHFManager + Role.CHFClerk + Roles.CHFAccountant + Roles.IMISAdministrator + Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ReportsPrimaryOperationalIndicatorPolicies : Return CheckUserRights(UserID, Right)'(Roles.CHManager And UserID)
            Case IMIS_EN.Enums.Rights.ReportsPrimaryOperationalIndicatorsClaims : Return CheckUserRights(UserID, Right)'(Roles.CHManager And UserID)
            Case IMIS_EN.Enums.Rights.ReportsDerivedOperationalIndicators : Return CheckUserRights(UserID, Right)'(Roles.CHManager And UserID)
            Case IMIS_EN.Enums.Rights.ReportsContributionCollection : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsProductSales : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsContributionDistribution : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsUserActivity : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ReportsEnrolmentPerformanceIndicators : Return CheckUserRights(UserID, Right)'(Roles.CHFManager And UserID)
            Case IMIS_EN.Enums.Rights.ReportsStatusOfRegister : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ReportsInsureeWithoutPhotos : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsPaymentCategoryOverview : Return CheckUserRights(UserID, Right)'(Roles.CHFManager And UserID)
            Case IMIS_EN.Enums.Rights.ReportsMatchingFunds : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsClaimOverviewReport : Return CheckUserRights(UserID, Right)'(Roles.ClaimAdministrator + Roles.CHFAccountant + Roles.CHFMedical Officer And UserID)
            Case IMIS_EN.Enums.Rights.ReportsPercentageReferrals : Return CheckUserRights(UserID, Right)'(Roles.ClaimAdministrator + Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsFamiliesInsureesOverview : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsPendingInsurees : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsRenewals : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsCapitationPayment : Return CheckUserRights(UserID, Right) '(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportRejectedPhoto : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsContributionPayment : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ReportsControlNumberAssignment : Return CheckUserRights(UserID, Right) '(Roles.CHFAccountant + Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.ReportsOverviewOfCommissions : Return CheckUserRights(UserID, Right) '(Roles.CHFAccountant  )
            Case IMIS_EN.Enums.Rights.ReportsClaimHistoryReport : Return CheckUserRights(UserID, Right) '(Roles.CHFAccountant  )

            'UTILITIES
            Case IMIS_EN.Enums.Rights.Utilities : Return CheckUserRights(UserID, Right, 1) '(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DatabaseBackup : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DatabaseRestore : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ExecuteScripts : Return CheckUserRights(UserID, Right) '(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EmailSettings : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator And UserID)

            'FUNDING
            Case IMIS_EN.Enums.Rights.FundingSave : Return CheckUserRights(UserID, Right) '(Roles.CHFAccountant And UserID)


            Case Else
                Return False
        End Select
    End Function

#Region "CheckRole not in use"
    'Public Function CheckRoles(ByVal Right As IMIS_EN.Enums.Rights, ByVal RoleId As Integer) As Boolean

    '    Select Case Right

    '        Case IMIS_EN.Enums.Rights.FindFamily : Return (Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And RoleId)
    '        Case IMIS_EN.Enums.Rights.AddFamily : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditFamily : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteFamily : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.ViewFamily : Return (Roles.CHFAccountant + Roles.Receptionist And RoleId)
    '        Case IMIS_EN.Enums.Rights.MovingInsuree : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.ChangeFamilyHead : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindInsuree : Return (Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And RoleId)
    '        Case IMIS_EN.Enums.Rights.AddInsuree : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteInsuree : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditInsuree : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.EnquireInsuree : Return (Roles.Receptionist And RoleId)   'Changed from ViewInsuree to EnquireInsuree because ViewInsure is the same to FindInsuree
    '        Case IMIS_EN.Enums.Rights.FindPolicy : Return (Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeletePolicy : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditPolicy : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.AddPolicy : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.ViewPolicy : Return (Roles.Receptionist And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindContribution : Return (Roles.CHFAccountant + Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindPayment : Return (Roles.CHFAccountant + Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteContribution : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.AddContribution : Return (Roles.CHFAccountant + Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditContribution : Return (Roles.CHFAccountant + Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindContribution : Return (Roles.Receptionist And RoleId)
    '        Case IMIS_EN.Enums.Rights.OverviewFamily : Return (Roles.CHFAccountant + Roles.CHFClerk And RoleId)
    '            'claims
    '        Case IMIS_EN.Enums.Rights.FindClaim : Return (Roles.ClaimAdministrator + Roles.ClaimContributor And RoleId)

    '        Case IMIS_EN.Enums.Rights.EnterClaim : Return (Roles.ClaimAdministrator + Roles.CHFClerk + Roles.ClaimContributor And RoleId)
    '        Case IMIS_EN.Enums.Rights.SelectClaimForFeedback : Return (Roles.CHFMedicalOfficer And RoleId)
    '       '????? Case IMIS_EN.Enums.Rights.PromptingFeedbackCollection : Return (Roles.CHFMedicalOfficer And RoleId)
    '        Case IMIS_EN.Enums.Rights.EnterFeedback : Return (Roles.CHFMedicalOfficer And RoleId)
    '       '????? Case IMIS_EN.Enums.Rights.CheckClaimPlausibility : Return (Roles.CHFMedicalOfficer And RoleId)
    '        Case IMIS_EN.Enums.Rights.SelectClaimForReview : Return (Roles.CHFMedicalOfficer And RoleId)
    '        Case IMIS_EN.Enums.Rights.ValuateClaim : Return (Roles.CHFAccountant And RoleId)
    '      '?????  Case IMIS_EN.Enums.Rights.DownloadClaimForPayment : Return (Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteClaim : Return (Roles.ClaimAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.UpdateClaims : Return (Roles.CHFMedicalOfficer And RoleId)
    '        Case IMIS_EN.Enums.Rights.ProcessClaims : Return (Roles.CHFMedicalOfficer And RoleId)
    '        'Case IMIS_EN.Enums.Rights.ClaimsBatchClosure : Return (Roles.ClaimAdministrator + Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.ReviewClaim : Return (Roles.CHFMedicalOfficer And RoleId)
    '        'Case IMIS_EN.Enums.Rights.ClaimOverview : Return (Roles.CHFMedicalOfficer And RoleId)
    '        Case IMIS_EN.Enums.Rights.LoadClaim : Return (Roles.CHFAccountant + Roles.CHFClerk + Roles.ClaimAdministrator + Roles.ClaimContributor And RoleId)
    '            'Administration
    '        Case IMIS_EN.Enums.Rights.AddUser : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteUser : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindUser : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditUser : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.FindOfficer : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.AddOfficer : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditOfficer : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteOfficer : Return (Roles.CHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.FindClaimAdministrator : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.AddClaimAdministrator : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditClaimAdministrator : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteClaimAdministrator : Return (Roles.CHFAdministrator And RoleId)

    '            '?????Case IMIS_EN.Enums.Rights.InquirePolicy : Return (Roles.Receptionist And RoleId)
    '       '???? Case IMIS_EN.Enums.Rights.DownloadDataOnPremiumsToAccounting : Return (Roles.CHFAccountant And RoleId)
    '       '???? Case IMIS_EN.Enums.Rights.UploadBatchXMLFile : Return (Roles.CHFClerk And RoleId)
    '       '???? Case IMIS_EN.Enums.Rights.DownloadBatchForPayment : Return (Roles.CHFAccountant And RoleId)
    '        '??? Case IMIS_EN.Enums.Rights.UploadAccountingDataOnBatchPayment : Return (Roles.CHFAccountant And RoleId)

    '        Case IMIS_EN.Enums.Rights.AddProduct : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindProduct : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditProduct : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteProduct : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.ViewProduct : Return (Roles.CHFAdministrator + Roles.CHFClerk + Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.DuplicateProduct : Return (Roles.CHFAdministrator And RoleId)


    '            '   ??? Case IMIS_EN.Enums.Rights.DownloadToOfflineClient : Return (Roles.CHFAdministrator And RoleId)
    '          '  ???? Case IMIS_EN.Enums.Rights.UploadDataFromOfflineClient : Return (Roles.CHFAdministrator And RoleId)
    '       '???? Case IMIS_EN.Enums.Rights.CreateDataForAccounting : Return (Roles.CHFAccountant And RoleId)
    '       '???? Case IMIS_EN.Enums.Rights.CreateDataForReinsurance : Return (Roles.CHFAccountant And RoleId)
    '       '???? Case IMIS_EN.Enums.Rights.CreateManagerialStatistics : Return (Roles.CHFManager And RoleId)

    '        Case IMIS_EN.Enums.Rights.EditMedicalItem : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteMedicalItem : Return (Roles.CHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.AddPriceListMedicalServices : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindPriceListMedicalServices : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditPriceListMedicalServices : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeletePriceListMedicalServices : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DuplicatePriceListMedicalServices : Return (Roles.CHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.AddPriceListMedicalItems : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindPriceListMedicalItems : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditPriceListMedicalItems : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeletePriceListMedicalItems : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DuplicatePriceListMedicalItems : Return (Roles.CHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.AddPayer : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindPayer : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditPayer : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeletePayer : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.ViewPayer : Return (Roles.CHFAdministrator + Roles.CHFClerk + Roles.CHFAccountant And RoleId)

    '        Case IMIS_EN.Enums.Rights.AddHealthFacility : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindHealthFacility : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditHealthFacility : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteHealthFacility : Return (Roles.CHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.AddMedicalService : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindMedicalService : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditMedicalService : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteMedicalService : Return (Roles.CHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.AddMedicalItem : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.FindMedicalItem : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EmailSettings : Return (Roles.IMISAdministrator And RoleId)

    '            'locations
    '        Case IMIS_EN.Enums.Rights.FindLocations : Return (Roles.CHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.AddDistrict : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditDistrict : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteDistrict : Return (Roles.CHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.AddWard : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditWard : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteWard : Return (Roles.CHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.AddVillage : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.EditVillage : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DeleteVillage : Return (Roles.CHFAdministrator And RoleId)

    '            'Tools
    '       ' Case IMIS_EN.Enums.Rights.UploadICD : Return (Roles.IMISAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.RenewPolicy : Return (Roles.CHFClerk And RoleId)
    '        Case IMIS_EN.Enums.Rights.FeedbackPrompt : Return (Roles.CHFMedicalOfficer And RoleId)
    '       ' Case IMIS_EN.Enums.Rights.IMISExtracts : Return (Roles.HFAdministrator + Roles.CHFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.OfflineExtracts : Return (Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.ClaimXMLUpload : Return (Roles.CHFClerk And RoleId)
    '      '  Case IMIS_EN.Enums.Rights.OfflineClaims : Return (Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)

    '        Case IMIS_EN.Enums.Rights.Utilities : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DatabaseBackup : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.DatabaseRestore : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.ExecuteScripts : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)

    '            'Reports
    '        Case IMIS_EN.Enums.Rights.Reports : Return (Roles.CHFManager + Roles.CHFAccountant + Roles.IMISAdministrator + Roles.CHFAdministrator And RoleId)
    '        'Case IMIS_EN.Enums.Rights.PremiumDistribution : Return (Roles.CHFAccountant And RoleId)
    '        'Case IMIS_EN.Enums.Rights.PremiumCollection : Return (Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.ProductSales : Return (Roles.CHFAccountant And RoleId)
    '       ' Case IMIS_EN.Enums.Rights.Indicators : Return (Roles.CHFManager And RoleId)
    '        Case IMIS_EN.Enums.Rights.UserActivity : Return (Roles.IMISAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.InsureeWithoutPhotos : Return (Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.MatchingFunds : Return (Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.ClaimOverviewReport : Return (Roles.ClaimAdministrator + Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.FamiliesInsureesOverview : Return (Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.StatusOfRegister : Return (Roles.CHFAdministrator And RoleId)
    '        Case IMIS_EN.Enums.Rights.PercentageReferrals : Return (Roles.ClaimAdministrator + Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.FamiliesInsureesOverview : Return (Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.PendingInsurees : Return (Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.RejectedPhoto : Return (Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.AddFund : Return (Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.Renewals : Return (Roles.CHFAccountant And RoleId)
    '        Case IMIS_EN.Enums.Rights.Reports : Return (Roles.CHFManager + Roles.CHFAccountant + Roles.CHFClerk + Roles.CHFMedicalOfficer And RoleId)

    '        Case IMIS_EN.Enums.Rights.CapitationPayment : Return (Roles.CHFAccountant And RoleId)



    '        Case Else
    '            Return False
    '    End Select

    'End Function
#End Region


    Private Enum Roles
        EnrolementOfficer = 1
        CHFManager = 2
        CHFAccountant = 4
        CHFClerk = 8
        CHFMedicalOfficer = 16
        CHFAdministrator = 32
        IMISAdministrator = 64
        Receptionist = 128
        ClaimAdministrator = 256
        ClaimContributor = 512

        HFAdministrator = 524288
        OfflineCHFAdministrator = 1048576
    End Enum

    Private Function getUserId(ByVal session As Object) As Integer
        Try
            Dim dt As New DataTable
            dt = DirectCast(session, DataTable)
            Return dt.Rows(0)("UserID")
        Catch
            Return Nothing
        End Try
    End Function
    Public Function RunPageSecurity(ByVal PageName As IMIS_EN.Enums.Pages, ByRef PageObj As System.Web.UI.Page) As Boolean

        Dim UserId As Integer = getUserId(PageObj.Session("User"))
        Dim HasAccess As Boolean = False

        Dim PageRights As List(Of Rights) = GetPageRights(PageName)

        For i As Integer = 0 To PageRights.Count - 1
            HasAccess = CheckRights(PageRights.ElementAt(i), UserId)
            If HasAccess Then
                Return True   'security is not enforced on page, user has access, can be full access or partial access.
            End If

        Next

        Return HasAccess  'security is enforced on page, user has no access

    End Function

    Public Function GetPageRights(ByVal PageName As IMIS_EN.Enums.Pages) As List(Of Rights)
        Dim PageRights As New List(Of Rights)
        Select Case PageName

            Case IMIS_EN.Enums.Pages.Home

            Case IMIS_EN.Enums.Pages.FindFamily
                PageRights.Add(Rights.FamilySearch)
                PageRights.Add(Rights.FamilyAdd)
                PageRights.Add(Rights.FamilyEdit)
                PageRights.Add(Rights.FamilyDelete)

            Case IMIS_EN.Enums.Pages.Family
                PageRights.Add(Rights.FamilySearch)
                PageRights.Add(Rights.FamilyAdd)
                PageRights.Add(Rights.FamilyEdit)
                PageRights.Add(Rights.FamilyDelete)

            Case IMIS_EN.Enums.Pages.ChangeFamily
                PageRights.Add(Rights.FamilyEdit)

            Case IMIS_EN.Enums.Pages.OverviewFamily
                PageRights.Add(Rights.FamilySearch)

            Case IMIS_EN.Enums.Pages.FindInsuree
                PageRights.Add(Rights.InsureeSearch)
                PageRights.Add(Rights.InsureeAdd)
                PageRights.Add(Rights.InsureeEdit)
                PageRights.Add(Rights.InsureeDelete)
                PageRights.Add(Rights.InsureeEnquire)

            Case IMIS_EN.Enums.Pages.Insuree
                PageRights.Add(Rights.InsureeEdit)
                PageRights.Add(Rights.InsureeAdd)
                PageRights.Add(Rights.InsureeSearch)
            Case IMIS_EN.Enums.Pages.FindPolicy
                PageRights.Add(Rights.PolicySearch)
            Case IMIS_EN.Enums.Pages.Policy
                PageRights.Add(Rights.PolicyAdd)
                PageRights.Add(Rights.PolicyEdit)
                PageRights.Add(Rights.PolicySearch)

            Case IMIS_EN.Enums.Pages.FindPremium
                PageRights.Add(Rights.ContributionSearch)
            Case IMIS_EN.Enums.Pages.Premium
                PageRights.Add(Rights.ContributionEdit)
                PageRights.Add(Rights.ContributionAdd)
                PageRights.Add(Rights.ContributionSearch)
            Case IMIS_EN.Enums.Pages.FindClaim     'claims
                PageRights.Add(Rights.ClaimSearch)
                PageRights.Add(Rights.ClaimAdd)
                PageRights.Add(Rights.ClaimDelete)
                PageRights.Add(Rights.ClaimLoad)
                PageRights.Add(Rights.ClaimPrint)
                PageRights.Add(Rights.ClaimSubmit)
                PageRights.Add(Rights.ClaimReview)
                PageRights.Add(Rights.ClaimFeedback)
                PageRights.Add(Rights.ClaimUpdate)
                PageRights.Add(Rights.ClaimProcess)
            Case IMIS_EN.Enums.Pages.ClaimOverview
                PageRights.Add(Rights.ClaimProcess)
                PageRights.Add(Rights.ClaimUpdate)
                PageRights.Add(Rights.ClaimReview)
                PageRights.Add(Rights.ClaimFeedback)
                PageRights.Add(Rights.ClaimSearch)

            Case IMIS_EN.Enums.Pages.Claim
                PageRights.Add(Rights.ClaimAdd)
                PageRights.Add(Rights.ClaimPrint)
                PageRights.Add(Rights.ClaimLoad)
                PageRights.Add(Rights.ClaimSearch)
            Case IMIS_EN.Enums.Pages.ClaimFeedback
                PageRights.Add(Rights.ClaimFeedback)
            Case IMIS_EN.Enums.Pages.ClaimReview
                PageRights.Add(Rights.ClaimSearch)
                PageRights.Add(Rights.ClaimReview)
            Case IMIS_EN.Enums.Pages.ProcessBatches
                PageRights.Add(Rights.BatchProcess)
                PageRights.Add(Rights.BatchFilter)
                PageRights.Add(Rights.BatchPreview)
            Case IMIS_EN.Enums.Pages.FindProduct
                PageRights.Add(Rights.ProductSearch)
                PageRights.Add(Rights.ProductAdd)
                PageRights.Add(Rights.ProductEdit)
                PageRights.Add(Rights.ProductDelete)
                PageRights.Add(Rights.ProductDuplicate)
            Case IMIS_EN.Enums.Pages.Product
                PageRights.Add(Rights.ProductAdd)
                PageRights.Add(Rights.ProductEdit)
                PageRights.Add(Rights.ProductSearch)
                PageRights.Add(Rights.ProductSearch)
            Case IMIS_EN.Enums.Pages.FindHealthFacility
                PageRights.Add(Rights.HealthFacilitySearch)
                PageRights.Add(Rights.HealthFacilityAdd)
                PageRights.Add(Rights.HealthFacilityEdit)
                PageRights.Add(Rights.HealthFacilityDelete)
            Case IMIS_EN.Enums.Pages.HealthFacility
                PageRights.Add(Rights.HealthFacilityAdd)
                PageRights.Add(Rights.HealthFacilityEdit)
                PageRights.Add(Rights.HealthFacilitySearch)
            Case IMIS_EN.Enums.Pages.FindPriceListMI
                PageRights.Add(Rights.FindPriceListMedicalItems)
                PageRights.Add(Rights.EditPriceListMedicalItems)
                PageRights.Add(Rights.AddPriceListMedicalItems)
                PageRights.Add(Rights.DeletePriceListMedicalItems)
                PageRights.Add(Rights.DuplicatePriceListMedicalItems)
            Case IMIS_EN.Enums.Pages.PriceListMI
                PageRights.Add(Rights.EditPriceListMedicalItems)
                PageRights.Add(Rights.AddPriceListMedicalItems)
                PageRights.Add(Rights.DeletePriceListMedicalItems)
                PageRights.Add(Rights.DuplicatePriceListMedicalItems)
            Case IMIS_EN.Enums.Pages.FindPriceListMS
                PageRights.Add(Rights.FindPriceListMedicalServices)
                PageRights.Add(Rights.EditPriceListMedicalServices)
                PageRights.Add(Rights.AddPriceListMedicalServices)
                PageRights.Add(Rights.DeletePriceListMedicalServices)
                PageRights.Add(Rights.DuplicatePriceListMedicalServices)
            Case IMIS_EN.Enums.Pages.PriceListMS
                PageRights.Add(Rights.EditPriceListMedicalServices)
                PageRights.Add(Rights.AddPriceListMedicalServices)
                PageRights.Add(Rights.FindPriceListMedicalServices)
            Case IMIS_EN.Enums.Pages.FindMedicalService
                PageRights.Add(Rights.FindMedicalService)
                PageRights.Add(Rights.EditMedicalService)
                PageRights.Add(Rights.AddMedicalService)
                PageRights.Add(Rights.DeleteMedicalService)
            Case IMIS_EN.Enums.Pages.MedicalService
                PageRights.Add(Rights.EditMedicalService)
                PageRights.Add(Rights.AddMedicalService)
                PageRights.Add(Rights.FindMedicalService)
            Case IMIS_EN.Enums.Pages.FindMedicalItem
                PageRights.Add(Rights.FindMedicalItem)
                PageRights.Add(Rights.EditMedicalItem)
                PageRights.Add(Rights.AddMedicalItem)
                PageRights.Add(Rights.DeleteMedicalItem)
            Case IMIS_EN.Enums.Pages.MedicalItem
                PageRights.Add(Rights.EditMedicalItem)
                PageRights.Add(Rights.AddMedicalItem)
                PageRights.Add(Rights.FindMedicalItem)
            Case IMIS_EN.Enums.Pages.FindUser
                PageRights.Add(Rights.UsersSearch)
                PageRights.Add(Rights.UsersAdd)
                PageRights.Add(Rights.UsersEdit)
                PageRights.Add(Rights.UsersDelete)
            Case IMIS_EN.Enums.Pages.User
                PageRights.Add(Rights.UsersAdd)
                PageRights.Add(Rights.UsersEdit)
                PageRights.Add(Rights.UsersSearch)
            Case IMIS_EN.Enums.Pages.UserProfiles
                PageRights.Add(Rights.FindUserProfile)
                PageRights.Add(Rights.AddUserProfile)
                PageRights.Add(Rights.EditUserProfile)
                PageRights.Add(Rights.DeleteUserProfile)
                PageRights.Add(Rights.DuplicateUserProfile)
            Case IMIS_EN.Enums.Pages.Role
                PageRights.Add(Rights.AddUserProfile)
                PageRights.Add(Rights.EditUserProfile)
                PageRights.Add(Rights.FindUserProfile)

            Case IMIS_EN.Enums.Pages.FindOfficer
                PageRights.Add(Rights.FindOfficer)
                PageRights.Add(Rights.AddOfficer)
                PageRights.Add(Rights.EditOfficer)
                PageRights.Add(Rights.DeleteOfficer)
            Case IMIS_EN.Enums.Pages.Officer
                PageRights.Add(Rights.AddOfficer)
                PageRights.Add(Rights.EditOfficer)
                PageRights.Add(Rights.FindOfficer)
            Case IMIS_EN.Enums.Pages.FindClaimAdministrator
                PageRights.Add(Rights.FindClaimAdministrator)
                PageRights.Add(Rights.AddClaimAdministrator)
                PageRights.Add(Rights.EditClaimAdministrator)
                PageRights.Add(Rights.DeleteClaimAdministrator)
            Case IMIS_EN.Enums.Pages.ClaimAdministrator
                PageRights.Add(Rights.AddClaimAdministrator)
                PageRights.Add(Rights.EditClaimAdministrator)
                PageRights.Add(Rights.FindClaimAdministrator)
            Case IMIS_EN.Enums.Pages.FindPayer
                PageRights.Add(Rights.FindPayer)
                PageRights.Add(Rights.AddPayer)
                PageRights.Add(Rights.EditPayer)
                PageRights.Add(Rights.DeletePayer)
            Case IMIS_EN.Enums.Pages.Payer
                PageRights.Add(Rights.AddPayer)
                PageRights.Add(Rights.EditPayer)
                PageRights.Add(Rights.FindPayer)
                PageRights.Add(Rights.FindPayer)
            Case IMIS_EN.Enums.Pages.Locations
                PageRights.Add(Rights.Locations)
            Case IMIS_EN.Enums.Pages.UploadICD
                PageRights.Add(Rights.Registers)
            Case IMIS_EN.Enums.Pages.PolicyRenewal
                PageRights.Add(Rights.PolicyRenew)
            Case IMIS_EN.Enums.Pages.IMISExtracts
                PageRights.Add(Rights.Extracts)
            Case IMIS_EN.Enums.Pages.Report
                PageRights.Add(Rights.Reports)
                PageRights.Add(Rights.ClaimPrint)
                PageRights.Add(Rights.BatchPreview)
                PageRights.Add(Rights.PolicyRenew)
            Case IMIS_EN.Enums.Pages.Reports
                PageRights.Add(Rights.Reports)
            Case IMIS_EN.Enums.Pages.Utilities
                PageRights.Add(Rights.Utilities)
                PageRights.Add(Rights.DatabaseBackup)
                PageRights.Add(Rights.DatabaseRestore)
                PageRights.Add(Rights.ExecuteScripts)
            Case IMIS_EN.Enums.Pages.FeedbackPrompt
                PageRights.Add(Rights.ClaimReview)
            Case IMIS_EN.Enums.Pages.EmailSettings
                PageRights.Add(Rights.EmailSettings)
            Case IMIS_EN.Enums.Pages.FindPayment
                PageRights.Add(Rights.PaymentSearch)
                PageRights.Add(Rights.PaymentEdit)
        End Select

        Return PageRights
    End Function


#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="RoleId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function ReturnRole(ByVal RoleId As Integer) As String
        Select Case RoleId
            Case 1 : Return imisgen.getMessage("L_ENROLMENTOFFICERS")
            Case 2 : Return imisgen.getMessage("M_MANAGER")
            Case 4 : Return imisgen.getMessage("M_ACCOUNTANT")
            Case 8 : Return imisgen.getMessage("M_CLERK")
            Case 16 : Return imisgen.getMessage("M_MEDICALOFFICER")
            Case 32 : Return imisgen.getMessage("M_SCHEMEADMINISTRATOR")
            Case 64 : Return imisgen.getMessage("M_IMISADMINISTRATOR")
            Case 128 : Return imisgen.getMessage("M_RECEPTIONIST")
            Case 256 : Return imisgen.getMessage("L_CLAIMADMIN")
            Case 512 : Return imisgen.getMessage("M_CLAIMCONTRIBUTOR")

            Case 524288 : Return imisgen.getMessage("M_HFADMINISTRATOR")
            Case 1048576 : Return imisgen.getMessage("M_OFFLINESCHEMEADMIN")
            Case Else : Return ""
        End Select
    End Function
    Public Function getRoleId(ByVal session As Object) As Integer
        Try
            Dim dt As New DataTable
            dt = DirectCast(session, DataTable)
            Return dt.Rows(0)("RoleID")
        Catch
            Return Nothing
        End Try
    End Function
    Public Function GetRoles(offline As Boolean) As DataTable
        Dim RolesDal As New IMIS_DAL.RoleDAL

        Dim dtRoles As DataTable = RolesDal.GetRoles(offline)
        For Each row As DataRow In dtRoles.Rows
            If row("IsSystem") > 0 Then

                row("RoleName") = ReturnRole(row("IsSystem"))
            End If
        Next
        Dim dr As DataRow
        dr = dtRoles.NewRow
        dr("RoleID") = 0
        dr("RoleName") = imisgen.getMessage("T_SELECTROLE")
        dtRoles.Rows.InsertAt(dr, 0)
        Return dtRoles
    End Function
    Public Function getUserRoles(ByVal UserId As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim UsersDal As New IMIS_DAL.UsersDAL
        Dim dtRoles As DataTable = UsersDal.getUserRoles(UserId)

        For Each row As DataRow In dtRoles.Rows
            If row("Code") > 0 Then
                row("Role") = ReturnRole(row("Code"))
            Else
                row("Code") = 10000000
            End If
        Next
        If showSelect = True Then
            Dim dr As DataRow
            dr = dtRoles.NewRow
            dr("Code") = 0
            dr("Role") = imisgen.getMessage("T_SELECTROLE")
            dtRoles.Rows.InsertAt(dr, 0)
        End If
        Return dtRoles
    End Function

    Public Function getRolesForUser(ByVal UserId As Integer, offline As Boolean, Authority As Integer) As DataTable
        Dim UsersDal As New IMIS_DAL.UsersDAL
        Dim dtRoles As DataTable = UsersDal.getRolesForUser(UserId, offline, Authority)
        For Each row As DataRow In dtRoles.Rows
            If row("IsSystem") > 0 Then
                row("RoleName") = ReturnRole(row("IsSystem"))
            End If
        Next
        Return dtRoles
    End Function

    Public Function GetUsers(ByVal eUser As IMIS_EN.tblUsers, Optional ByVal Legacy As Boolean = False, Optional ByVal DistrictId As Integer = 0, Optional Authority As Integer = 0) As DataTable
        Dim getDataTable As New IMIS_DAL.UsersDAL
        eUser.LastName += "%"
        eUser.OtherNames += "%"
        eUser.Phone += "%"
        eUser.LoginName += "%"
        eUser.EmailId = "%" & eUser.EmailId & "%"


        Return getDataTable.GetUsers(eUser, Legacy, DistrictId, Authority)


    End Function

    Public Function SaveUser(ByRef eUser As IMIS_EN.tblUsers, Optional dtRoles As DataTable = Nothing) As Integer '0 - Insert, 1 - Exists, 2 - Update
        Dim users As New IMIS_DAL.UsersDAL
        Dim dt As DataTable = users.CheckIfUserExists(eUser)


        If dt.Rows.Count > 0 Then Return 1
        If eUser.RoleID > 0 And dtRoles Is Nothing Then
            Dim DALRole As New IMIS_DAL.RoleDAL
            dtRoles = DALRole.GetSystemRoles(eUser.RoleID)
        End If
        If eUser.UserID = 0 Or eUser.ValidityTo IsNot Nothing Then
            eUser.UserID = 0
            CreatePassword(eUser)
            users.InsertUser(eUser)
            users.SaveUserRoles(dtRoles, eUser)
            Return 0
        Else
            If eUser.DummyPwd <> String.Empty Then
                CreatePassword(eUser)
            End If

            'Check if no changes made not to update.
            Dim UserOrg As New IMIS_EN.tblUsers
            UserOrg.UserID = eUser.UserID
            users.LoadUsers(UserOrg)
            'If isDirtyUser(eUser, UserOrg) Then
            users.UpdateUser(eUser)
            'End If

            users.SaveUserRoles(dtRoles, eUser)
            Return 2
        End If
    End Function

    Private Function isDirtyUser(eUser As IMIS_EN.tblUsers, eUserOrg As IMIS_EN.tblUsers) As Boolean
        isDirtyUser = True
        If eUser.DummyPwd <> "" Then Exit Function
        If IIf(eUser.LoginName Is Nothing, DBNull.Value, eUser.LoginName).ToString() <> IIf(eUserOrg.LoginName Is Nothing, DBNull.Value, eUserOrg.LoginName).ToString() Then Exit Function
        If IIf(eUser.LastName Is Nothing, DBNull.Value, eUser.LastName).ToString() <> IIf(eUserOrg.LastName Is Nothing, DBNull.Value, eUserOrg.LastName).ToString() Then Exit Function
        If IIf(eUser.OtherNames Is Nothing, DBNull.Value, eUser.OtherNames).ToString() <> IIf(eUserOrg.OtherNames Is Nothing, DBNull.Value, eUserOrg.OtherNames).ToString() Then Exit Function
        If IIf(eUser.Phone Is Nothing, DBNull.Value, eUser.Phone).ToString() <> IIf(eUserOrg.Phone Is Nothing, DBNull.Value, eUserOrg.Phone).ToString() Then Exit Function
        If IIf(eUser.EmailId Is Nothing, DBNull.Value, eUser.EmailId).ToString() <> IIf(eUserOrg.EmailId Is Nothing, DBNull.Value, eUserOrg.EmailId).ToString() Then Exit Function
        If eUser.UserID <> eUserOrg.UserID Then Exit Function
        If IIf(eUser.HFID Is Nothing, DBNull.Value, eUser.HFID).ToString() <> IIf(eUserOrg.HFID Is Nothing, DBNull.Value, eUserOrg.HFID).ToString() Then Exit Function
        If IIf(eUser.LanguageID Is Nothing, DBNull.Value, eUser.LanguageID).ToString() <> IIf(eUserOrg.LanguageID Is Nothing, DBNull.Value, eUserOrg.LanguageID).ToString() Then Exit Function

        isDirtyUser = False
    End Function
    Public Function getUsersDistricts(ByVal UserId As Integer)
        Dim users As New IMIS_DAL.UsersDAL
        Return users.getUsersDistricts(UserId)
    End Function
    Public Function SaveUserDistricts(ByVal eUserDistricts As IMIS_EN.tblUsersDistricts) As Integer
        Dim userDistricts As New IMIS_DAL.UsersDAL
        If eUserDistricts.UserDistrictID = 0 Then
            userDistricts.InsertUserDistricts(eUserDistricts)
        Else
            userDistricts.DeleteUserDistricts(eUserDistricts)
        End If
        Return 0
    End Function
    Public Sub LoadUsers(ByRef eUser As IMIS_EN.tblUsers)
        Dim User As New IMIS_DAL.UsersDAL
        User.LoadUsers(eUser)
    End Sub
    Public Sub DeleteUser(ByRef eUser As IMIS_EN.tblUsers)
        Dim User As New IMIS_DAL.UsersDAL
        User.DeleteUser(eUser)
    End Sub

    Public Sub TestTable()
        Dim test As New IMIS_DAL.UsersDAL
        test.TestTable()
    End Sub
    Public Function GetUsers() As DataTable
        Dim DAL As New IMIS_DAL.UsersDAL
        Dim dt As DataTable = DAL.GetUsers
        Dim dr As DataRow

        dr = dt.NewRow
        dr("UserID") = 0
        dr("UserName") = imisgen.getMessage("T_SELECTUSERNAME")
        dt.Rows.InsertAt(dr, 0)

        Return dt
    End Function
    Public Function ChangePassword(ByVal euser As IMIS_EN.tblUsers, NewPassword As String) As Integer
        Dim users As New IMIS_DAL.UsersDAL
        Dim Gen As New GeneralBL
        '-1 = Wrong Old Password
        ' 1 = Saved OK
        Dim OldPassword As String = euser.DummyPwd
        LoadUsers(euser)
        Dim LegacyPassword As String = euser.DummyPwd
        euser.DummyPwd = OldPassword
        If ValidateLogin(euser) Or LegacyPassword = OldPassword Then
            euser.DummyPwd = NewPassword
            CreatePassword(euser)
            users.ChangePassword(euser)
            Return 1
        End If
        Return -1
    End Function
    Public Function CreateEmailHash(ByRef eUser As IMIS_EN.tblUsers) As String

        Dim Gen As New GeneralBL
        Return Gen.GenerateSHA256String(eUser.DummyPwd + eUser.EmailId + eUser.PasswordValidity)
    End Function
    Public Function ValidateEmailHash(ByRef eUser As IMIS_EN.tblUsers, EmailHash As String) As Boolean

        Dim Gen As New GeneralBL
        If Gen.GenerateSHA256String(eUser.DummyPwd + eUser.EmailId + eUser.PasswordValidity) = EmailHash Then
            Return True
        End If
        Return False

    End Function
    Public Sub CreatePassword(ByRef eUser As IMIS_EN.tblUsers)

        Dim Gen As New GeneralBL
        eUser.PrivateKey = Gen.PrivateKey(256)
        eUser.StoredPassword = Gen.GenerateSHA256String(eUser.DummyPwd + eUser.PrivateKey)
    End Sub
    Public Function ValidateLogin(ByRef eUser As IMIS_EN.tblUsers) As Boolean

        Dim Gen As New GeneralBL
        If Gen.GenerateSHA256String(eUser.DummyPwd + eUser.PrivateKey) = eUser.StoredPassword Then
            Return True
        End If
        Return False
    End Function
    Public Function IsUserExists(ByVal UserID As Integer) As Boolean
        Dim User As New IMIS_DAL.UsersDAL
        Return User.IsUserExists(UserID)
    End Function
    Public Function GetUserIdByUUID(ByVal uuid As Guid) As Integer
        Dim User As New IMIS_DAL.UsersDAL
        Return User.GetUserIdByUUID(uuid).Rows(0).Item(0)
    End Function
    Function GetUserDistricts(ByVal CurrenctUserID As Integer, ByVal SelectedUserID As Integer) As Integer
        Dim User As New IMIS_DAL.UsersDAL
        Dim ds As New DataSet
        ds = User.GetUserDistricts(CurrenctUserID, SelectedUserID)
        Dim dtSelectedUserDistricts As DataTable = ds.Tables("SelectedUserDistricts")
        Dim dtCurrentUserDistricts As DataTable = ds.Tables("CurrentUserDistricts")

        Dim dtSelectedUserRegions As DataTable = ds.Tables("SelectedUserRegions")
        Dim dtCurrentUserRegions As DataTable = ds.Tables("CurrentUserRegions")

        Dim Users As New IMIS_DAL.UsersDAL

        If dtCurrentUserRegions.Rows.Count = 1 Then
            If dtSelectedUserRegions.Rows.Count = 1 Then
                If dtCurrentUserDistricts.Rows.Count = 1 AndAlso dtSelectedUserDistricts.Rows.Count > 1 Then
                    Return 1  ' The selected user from the gridview should not be edited
                End If
            Else
                Return 1  ' The selected user from the gridview should not be edited
            End If
        ElseIf dtCurrentUserDistricts.Rows.Count = 1 Then
            Return 1  ' The selected user from the gridview should not be edited
        End If
        Return 0
    End Function
End Class
