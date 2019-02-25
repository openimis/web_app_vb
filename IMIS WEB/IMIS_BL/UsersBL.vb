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
    '2b) RunPageSecurity [ inside USERBL class ] ---- calls CheckRoles[ inside USERBL class ]
    Public Function CheckRights(ByVal Right As IMIS_EN.Enums.Rights, ByVal UserID As Integer) As Boolean

        Select Case Right
            'FAMILY OR GROUP
            Case IMIS_EN.Enums.Rights.Family : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindFamily : Return CheckUserRights(UserID, Right)''(Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And UserID)
            Case IMIS_EN.Enums.Rights.AddFamily : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.EditFamily : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.DeleteFamily : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)

            'INSUREE
            Case IMIS_EN.Enums.Rights.Insuree : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindInsuree : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And UserID)
            Case IMIS_EN.Enums.Rights.AddInsuree : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.EditInsuree : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.DeleteInsuree : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.EnquireInsuree : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)

            'POLICY
            Case IMIS_EN.Enums.Rights.FindPolicy : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And UserID)
            Case IMIS_EN.Enums.Rights.AddPolicy : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.EditPolicy : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.DeletePolicy : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
                '
            Case IMIS_EN.Enums.Rights.RenewPolicy : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)   'NOT PART OF THE NEW DOCUMENT

            'CONTRIBUTION
            Case IMIS_EN.Enums.Rights.FindContribution : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk And UserID) 
            Case IMIS_EN.Enums.Rights.AddContribution : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.EditContribution : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.DeleteContribution : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)

            'PAYMENT
            Case IMIS_EN.Enums.Rights.FindPayment : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk And UserID) 
            Case IMIS_EN.Enums.Rights.AddPayment : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.EditPayment : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.DeletePayment : Return CheckUserRights(UserID, Right)

            'CLAIMS
            'Case IMIS_EN.Enums.Rights.Claim : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindClaim : Return CheckUserRights(UserID, Right)'(Roles.ClaimAdministrator + Roles.ClaimContributor + Roles.CHFMedicalOfficer And UserID) 
            Case IMIS_EN.Enums.Rights.EnterClaim : Return CheckUserRights(UserID, Right)'(Roles.ClaimAdministrator + Roles.CHFClerk + Roles.ClaimContributor And UserID)
            Case IMIS_EN.Enums.Rights.EditClaim : Return CheckUserRights(UserID, Right)'NO MAPPING TO ANY USER
            Case IMIS_EN.Enums.Rights.DeleteClaim : Return CheckUserRights(UserID, Right)'(Roles.ClaimAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.LoadClaim : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant + Roles.CHFClerk + Roles.ClaimAdministrator + Roles.ClaimContributor And UserID)
            Case IMIS_EN.Enums.Rights.PrintClaim : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.SubmitClaim : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.ReviewClaim : Return CheckUserRights(UserID, Right)'(Roles.CHFMedicalOfficer And UserID)
            Case IMIS_EN.Enums.Rights.EnterFeedback : Return CheckUserRights(UserID, Right)'(Roles.CHFMedicalOfficer And UserID)                 
            Case IMIS_EN.Enums.Rights.SelectClaimForFeedback : Return CheckUserRights(UserID, Right)'(Roles.CHFMedicalOfficer And UserID)
            Case IMIS_EN.Enums.Rights.UpdateClaims : Return CheckUserRights(UserID, Right)'(Roles.CHFMedicalOfficer And UserID)
            Case IMIS_EN.Enums.Rights.ProcessClaims : Return CheckUserRights(UserID, Right)'(Roles.CHFMedicalOfficer And UserID)  

            'BATCH
            Case IMIS_EN.Enums.Rights.Batch : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.ValuateClaim : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)    
            Case IMIS_EN.Enums.Rights.BatchProcess : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)  Changed from no mapping to CHFAccount because ParentNode ValueClaim mapped to that user, so child also follows
            Case IMIS_EN.Enums.Rights.BatchFilter : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)   Changed from no mapping to CHFAccount because ParentNode ValueClaim mapped to that user, so child also follows
            Case IMIS_EN.Enums.Rights.BatchPreview : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)  Changed from no mapping to CHFAccount because ParentNode ValueClaim mapped to that user, so child also follows

            'ADMINISTRATION 
            Case IMIS_EN.Enums.Rights.Product : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindProduct : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddProduct : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditProduct : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteProduct : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID) 
            Case IMIS_EN.Enums.Rights.DuplicateProduct : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.HealthFacility : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindHealthFacility : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddHealthFacility : Return CheckUserRights(UserID, Right)' (Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditHealthFacility : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteHealthFacility : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID) 

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
            Case IMIS_EN.Enums.Rights.FindUser : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddUser : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditUser : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteUser : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.Payer : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindPayer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.AddPayer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditPayer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeletePayer : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID) 

            Case IMIS_EN.Enums.Rights.Locations : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindLocations : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)  

            Case IMIS_EN.Enums.Rights.userProfiles : Return CheckUserRights(UserID, Right, 1)
            Case IMIS_EN.Enums.Rights.FindUserProfile : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.AddUserProfile : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.EditUserProfile : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.DeleteUserProfile : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.DuplicateUserProfile : Return CheckUserRights(UserID, Right)
           'TOOLS
            Case IMIS_EN.Enums.Rights.UploadICD : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator And RoleId) 'PAUL, THIS NEEDS YOUR ATTENTION (LEGACY BUT NOT FIND IN A NEW DOCUMENT)*******************<<<
           '--

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
            Case IMIS_EN.Enums.Rights.MasterDataDownload : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.PhoneExtractsCreate : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.OfflineExtractCreate : Return CheckUserRights(UserID, Right)'(Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ClaimXMLUpload : Return CheckUserRights(UserID, Right)'(Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.EnrolmentsUpload : Return CheckUserRights(UserID, Right)
            Case IMIS_EN.Enums.Rights.FeedbackUpload : Return CheckUserRights(UserID, Right)

            'REPORTS 
            Case IMIS_EN.Enums.Rights.Reports : Return CheckUserRights(UserID, Right, 0)'(Roles.CHFManager + Role.CHFClerk + Roles.CHFAccountant + Roles.IMISAdministrator + Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.PrimaryOperationalIndicatorPolicies : Return CheckUserRights(UserID, Right)'(Roles.CHManager And UserID)
            Case IMIS_EN.Enums.Rights.PrimaryOperationalIndicatorsClaims : Return CheckUserRights(UserID, Right)'(Roles.CHManager And UserID)
            Case IMIS_EN.Enums.Rights.DerivedOperationalIndicators : Return CheckUserRights(UserID, Right)'(Roles.CHManager And UserID)
            Case IMIS_EN.Enums.Rights.ContributionCollection : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ProductSales : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ContributionDistribution : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.UserActivity : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EnrolmentPerformanceIndicators : Return CheckUserRights(UserID, Right)'(Roles.CHFManager And UserID)
            Case IMIS_EN.Enums.Rights.StatusOfRegister : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.InsureeWithoutPhotos : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.PaymentCategoryOverview : Return CheckUserRights(UserID, Right)'(Roles.CHFManager And UserID)
            Case IMIS_EN.Enums.Rights.MatchingFunds : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ClaimOverviewReport : Return CheckUserRights(UserID, Right)'(Roles.ClaimAdministrator + Roles.CHFAccountant + Roles.CHFMedical Officer And UserID)
            Case IMIS_EN.Enums.Rights.PercentageReferrals : Return CheckUserRights(UserID, Right)'(Roles.ClaimAdministrator + Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.FamiliesInsureesOverview : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.PendingInsurees : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.Renewals : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.CapitationPayment : Return CheckUserRights(UserID, Right) '(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.RejectedPhoto : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ContributionPayment : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)
            Case IMIS_EN.Enums.Rights.ControlNumberAssignment : Return CheckUserRights(UserID, Right) '(Roles.CHFAccountant + Roles.CHFClerk And UserID)
            Case IMIS_EN.Enums.Rights.ClaimHistoryReport : Return CheckUserRights(UserID, Right) '(Roles.CHFAccountant  )


            'UTILITIES
            Case IMIS_EN.Enums.Rights.Utilities : Return CheckUserRights(UserID, Right, 1) '(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DatabaseBackup : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DatabaseRestore : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.ExecuteScripts : Return CheckUserRights(UserID, Right) '(Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EmailSettings : Return CheckUserRights(UserID, Right)'(Roles.IMISAdministrator And UserID)

            'FUNDING
            Case IMIS_EN.Enums.Rights.AddFund : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)

            'LEGACY ROLE, NEW NOT FIND IN NEW DOCUMENT
            Case IMIS_EN.Enums.Rights.FeedbackPrompt : Return CheckUserRights(UserID, Right)'(Roles.CHFMedicalOfficer And UserID)


        '***************************************************************************************************The rest are not in part of the new Legacy roles****************************************
            ''THE REST ARE NOT INCLUDED IN THE DOCUMENT  
            'Case IMIS_EN.Enums.Rights.FamiliesInsureesOverview : Return CheckUserRights(UserID, Right)'(Roles.CHFAccountant And UserID)  

            Case IMIS_EN.Enums.Rights.AddDistrict : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditDistrict : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteDistrict : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.AddWard : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditWard : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteWard : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

            Case IMIS_EN.Enums.Rights.AddVillage : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.EditVillage : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)
            Case IMIS_EN.Enums.Rights.DeleteVillage : Return CheckUserRights(UserID, Right)'(Roles.CHFAdministrator And UserID)

                'Tools
           'Case IMIS_EN.Enums.Rights.UploadICD : Return (Roles.IMISAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.OfflineExtracts : Return CheckUserRights(UserID, Right)'(Roles.HFAdministrator + Roles.OfflineCHFAdministrator And UserID) 




            Case IMIS_EN.Enums.Rights.SelectClaimForReview : Return CheckUserRights(UserID, Right) '(Roles.CHFMedicalOfficer And UserID)                                                        '--> NOT INCLUDED IN A NEW DOCUMENT (Commented by Emmanuel)                                                                   '---> NOT INCLUDED IN A NEW DOCUMENT (Comment by Emmanuel)
                '?????  Case IMIS_EN.Enums.Rights.DownloadClaimForPayment : Return (Roles.CHFAccountant And RoleId)



                '????? Case IMIS_EN.Enums.Rights.CheckClaimPlausibility : Return (Roles.CHFMedicalOfficer And RoleId)  

                '***************************************************************************************************Not in part of Legacy roles****************************************
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
    'Public Function RunPageSecurity(ByVal PageName As IMIS_EN.Enums.Pages, ByRef PageObj As System.Web.UI.Page) As Boolean

    '    Dim RoleId As Integer = getRoleId(PageObj.Session("User"))
    '    Dim HasAccess As Boolean = False

    '    Dim PageRights As List(Of Rights) = GetPageRights(PageName)

    '    For i As Integer = 0 To PageRights.Count - 1
    '        HasAccess = CheckRoles(PageRights.ElementAt(i), RoleId)
    '        If HasAccess Then
    '            Return True   'security is not enforced on page, user has access, can be full access or partial access.
    '        End If

    '    Next

    '    Return HasAccess  'security is enforced on page, user has no access

    'End Function
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

        'Dim RoleId As Integer = getRoleId(PageObj.Session("User"))
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
                PageRights.Add(Rights.FindFamily)
                PageRights.Add(Rights.AddFamily)
                PageRights.Add(Rights.EditFamily)
                PageRights.Add(Rights.DeleteFamily)

            Case IMIS_EN.Enums.Pages.Family
                PageRights.Add(Rights.FindFamily)
                PageRights.Add(Rights.AddFamily)
                PageRights.Add(Rights.EditFamily)
                PageRights.Add(Rights.DeleteFamily)

            Case IMIS_EN.Enums.Pages.ChangeFamily
                PageRights.Add(Rights.EditFamily)

            Case IMIS_EN.Enums.Pages.OverviewFamily
                PageRights.Add(Rights.FindFamily)

            Case IMIS_EN.Enums.Pages.FindInsuree
                PageRights.Add(Rights.FindInsuree)
                PageRights.Add(Rights.AddInsuree)
                PageRights.Add(Rights.EditInsuree)
                PageRights.Add(Rights.DeleteInsuree)
                PageRights.Add(Rights.EnquireInsuree)

            Case IMIS_EN.Enums.Pages.Insuree
                PageRights.Add(Rights.EditInsuree)
                PageRights.Add(Rights.AddInsuree)
                PageRights.Add(Rights.FindInsuree)

            Case IMIS_EN.Enums.Pages.FindPolicy
                PageRights.Add(Rights.FindPolicy)

            Case IMIS_EN.Enums.Pages.Policy
                PageRights.Add(Rights.AddPolicy)
                PageRights.Add(Rights.EditPolicy)
                PageRights.Add(Rights.FindPolicy)

            Case IMIS_EN.Enums.Pages.FindPremium
                PageRights.Add(Rights.FindContribution)

            Case IMIS_EN.Enums.Pages.Premium
                PageRights.Add(Rights.EditContribution)
                PageRights.Add(Rights.AddContribution)
                PageRights.Add(Rights.ViewContribution)

            Case IMIS_EN.Enums.Pages.FindClaim     'claims
                PageRights.Add(Rights.FindClaim)
                PageRights.Add(Rights.EnterClaim)
                PageRights.Add(Rights.EditClaim)
                PageRights.Add(Rights.DeleteClaim)
                PageRights.Add(Rights.LoadClaim)
                PageRights.Add(Rights.PrintClaim)
                PageRights.Add(Rights.SubmitClaim)
                PageRights.Add(Rights.ReviewClaim)
                PageRights.Add(Rights.EnterFeedback)
                PageRights.Add(Rights.SelectClaimForFeedback)
                PageRights.Add(Rights.UpdateClaims)
                PageRights.Add(Rights.ProcessClaims)


            Case IMIS_EN.Enums.Pages.ClaimOverview
                PageRights.Add(Rights.ProcessClaims)
                PageRights.Add(Rights.UpdateClaims)
               'PageRights.Add(Rights.ClaimOverview)

            Case IMIS_EN.Enums.Pages.Claim
                PageRights.Add(Rights.EnterClaim)
                PageRights.Add(Rights.LoadClaim)

            Case IMIS_EN.Enums.Pages.ClaimFeedback
                PageRights.Add(Rights.EnterFeedback)

            Case IMIS_EN.Enums.Pages.ClaimReview
                PageRights.Add(Rights.ReviewClaim)

            Case IMIS_EN.Enums.Pages.ProcessBatches
                PageRights.Add(Rights.ValuateClaim)
                PageRights.Add(Rights.BatchProcess)
                PageRights.Add(Rights.BatchFilter)
                PageRights.Add(Rights.BatchPreview)

            Case IMIS_EN.Enums.Pages.FindProduct                 'administration
                PageRights.Add(Rights.FindProduct)
                PageRights.Add(Rights.AddProduct)
                PageRights.Add(Rights.EditProduct)
                PageRights.Add(Rights.DeleteProduct)
                PageRights.Add(Rights.DuplicateProduct)

            Case IMIS_EN.Enums.Pages.Product
                PageRights.Add(Rights.AddProduct)
                PageRights.Add(Rights.EditProduct)
                PageRights.Add(Rights.FindProduct)
                PageRights.Add(Rights.FindProduct)

            Case IMIS_EN.Enums.Pages.FindHealthFacility
                PageRights.Add(Rights.FindHealthFacility)
                PageRights.Add(Rights.AddHealthFacility)
                PageRights.Add(Rights.EditHealthFacility)
                PageRights.Add(Rights.DeleteHealthFacility)

            Case IMIS_EN.Enums.Pages.HealthFacility
                PageRights.Add(Rights.AddHealthFacility)
                PageRights.Add(Rights.EditHealthFacility)
                PageRights.Add(Rights.FindHealthFacility)

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
                PageRights.Add(Rights.FindUser)
                PageRights.Add(Rights.AddUser)
                PageRights.Add(Rights.EditUser)
                PageRights.Add(Rights.DeleteUser)


            Case IMIS_EN.Enums.Pages.User
                PageRights.Add(Rights.AddUser)
                PageRights.Add(Rights.EditUser)
                PageRights.Add(Rights.FindUser)

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
                PageRights.Add(Rights.FindLocations)


            Case IMIS_EN.Enums.Pages.UploadICD                     'Tools
                PageRights.Add(Rights.UploadICD)

            Case IMIS_EN.Enums.Pages.PolicyRenewal
                PageRights.Add(Rights.RenewPolicy)

            Case IMIS_EN.Enums.Pages.IMISExtracts
                PageRights.Add(Rights.MasterDataDownload)
                PageRights.Add(Rights.PhoneExtractsCreate)
                PageRights.Add(Rights.OfflineExtracts)
                PageRights.Add(Rights.UpdateClaims)
                PageRights.Add(Rights.EnrolmentsUpload)
                PageRights.Add(Rights.FeedbackUpload)

            Case IMIS_EN.Enums.Pages.Report
                PageRights.Add(Rights.Reports)

            Case IMIS_EN.Enums.Pages.Reports
                PageRights.Add(Rights.Reports)
                PageRights.Add(Rights.PrimaryOperationalIndicatorPolicies)
                PageRights.Add(Rights.PrimaryOperationalIndicatorsClaims)
                PageRights.Add(Rights.DerivedOperationalIndicators)
                PageRights.Add(Rights.ContributionCollection)
                PageRights.Add(Rights.ProductSales)
                PageRights.Add(Rights.ContributionDistribution)
                PageRights.Add(Rights.UserActivity)
                PageRights.Add(Rights.EnrolmentPerformanceIndicators)
                PageRights.Add(Rights.StatusOfRegister)
                PageRights.Add(Rights.InsureeWithoutPhotos)
                PageRights.Add(Rights.PaymentCategoryOverview)
                PageRights.Add(Rights.MatchingFunds)
                PageRights.Add(Rights.PercentageReferrals)
                PageRights.Add(Rights.PendingInsurees)
                PageRights.Add(Rights.Renewals)
                PageRights.Add(Rights.CapitationPayment)
                PageRights.Add(Rights.RejectedPhoto)


            Case IMIS_EN.Enums.Pages.Utilities
                PageRights.Add(Rights.Utilities)
                PageRights.Add(Rights.DatabaseBackup)
                PageRights.Add(Rights.DatabaseRestore)
                PageRights.Add(Rights.ExecuteScripts)


            Case IMIS_EN.Enums.Pages.FeedbackPrompt
                PageRights.Add(Rights.FeedbackPrompt)

            Case IMIS_EN.Enums.Pages.EmailSettings
                PageRights.Add(Rights.EmailSettings)
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
    Public Function getRolesForUser(ByVal UserId As Integer, offline As Boolean) As DataTable
        Dim UsersDal As New IMIS_DAL.UsersDAL
        Dim dtRoles As DataTable = UsersDal.getRolesForUser(UserId, offline)
        For Each row As DataRow In dtRoles.Rows
            If row("IsSystem") > 0 Then
                row("RoleName") = ReturnRole(row("IsSystem"))
            End If
        Next
        Return dtRoles
    End Function
    'Public Function getUserRoles(ByVal RoleId As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
    '    Dim imisgen As New GeneralBL
    '    Dim dtbl As New DataTable
    '    Dim dr As DataRow
    '    dtbl.Columns.Add("Code")
    '    dtbl.Columns.Add("Role")
    '    If showSelect = True Then
    '        dr = dtbl.NewRow
    '        dr("Code") = 0
    '        dr("Role") = imisgen.getMessage("T_SELECTROLE")
    '        dtbl.Rows.Add(dr)
    '    End If
    '    Dim i As Integer = 1
    '    Do Until i > 1048576 '1024
    '        If (i And RoleId) > 0 Then
    '            dr = dtbl.NewRow
    '            dr("Code") = i
    '            dr("Role") = ReturnRole(i)
    '            dtbl.Rows.Add(dr)
    '        End If
    '        i += i
    '    Loop
    '    Return dtbl
    'End Function

    Public Function GetUsers(ByVal eUser As IMIS_EN.tblUsers, Optional ByVal Legacy As Boolean = False, Optional ByVal DistrictId As Integer = 0) As DataTable
        Dim getDataTable As New IMIS_DAL.UsersDAL
        eUser.LastName += "%"
        eUser.OtherNames += "%"
        eUser.Phone += "%"
        eUser.LoginName += "%"
        eUser.EmailId = "%" & eUser.EmailId & "%"


        Return getDataTable.GetUsers(eUser, Legacy, DistrictId)


    End Function

    Public Function SaveUser(ByRef eUser As IMIS_EN.tblUsers, Optional dtRoles As DataTable = Nothing) As Integer '0 - Insert, 1 - Exists, 2 - Update
        Dim users As New IMIS_DAL.UsersDAL
        Dim dt As DataTable = users.CheckIfUserExists(eUser)


        If dt.Rows.Count > 0 Then Return 1
        If eUser.RoleID > 0 And dtRoles Is Nothing Then
            Dim DALRole As New IMIS_DAL.RoleDAL
            dtRoles = DALRole.GetSystemRoles(eUser.RoleID)
        End If
        If eUser.UserID = 0 Then
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
            If isDirtyUser(eUser, UserOrg) Then
                users.UpdateUser(eUser)
            End If

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

End Class