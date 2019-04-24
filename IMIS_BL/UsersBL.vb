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

    Public Function CheckRoles(ByVal Right As IMIS_EN.Enums.Rights, ByVal RoleId As Integer) As Boolean

        Select Case Right

            Case IMIS_EN.Enums.Rights.FindFamily : Return (Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And RoleId)
            Case IMIS_EN.Enums.Rights.AddFamily : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.EditFamily : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteFamily : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.ViewFamily : Return (Roles.CHFAccountant + Roles.Receptionist And RoleId)
            Case IMIS_EN.Enums.Rights.MovingInsuree : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.ChangeFamilyHead : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.FindInsuree : Return (Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And RoleId)
            Case IMIS_EN.Enums.Rights.AddInsuree : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteInsuree : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.EditInsuree : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.ViewInsuree : Return (Roles.Receptionist And RoleId)
            Case IMIS_EN.Enums.Rights.FindPolicy : Return (Roles.CHFAccountant + Roles.CHFClerk + Roles.Receptionist And RoleId)
            Case IMIS_EN.Enums.Rights.DeletePolicy : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.EditPolicy : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.AddPolicy : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.ViewPolicy : Return (Roles.Receptionist And RoleId)
            Case IMIS_EN.Enums.Rights.FindPremium : Return (Roles.CHFAccountant + Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.DeletePremium : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.AddPremium : Return (Roles.CHFAccountant + Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.EditPremium : Return (Roles.CHFAccountant + Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.ViewPremium : Return (Roles.Receptionist And RoleId)
            Case IMIS_EN.Enums.Rights.OverviewFamily : Return (Roles.CHFAccountant + Roles.CHFClerk And RoleId)
                'claims
            Case IMIS_EN.Enums.Rights.FindClaim : Return (Roles.ClaimAdministrator + Roles.ClaimContributor And RoleId)
            Case IMIS_EN.Enums.Rights.ClaimXMLUpload : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.EnterClaim : Return (Roles.ClaimAdministrator + Roles.CHFClerk + Roles.ClaimContributor And RoleId)
            Case IMIS_EN.Enums.Rights.SelectClaimForFeedback : Return (Roles.CHFMedicalOfficer And RoleId)
            Case IMIS_EN.Enums.Rights.PromptingFeedbackCollection : Return (Roles.CHFMedicalOfficer And RoleId)
            Case IMIS_EN.Enums.Rights.EnterFeedback : Return (Roles.CHFMedicalOfficer And RoleId)
            Case IMIS_EN.Enums.Rights.CheckClaimPlausibility : Return (Roles.CHFMedicalOfficer And RoleId)
            Case IMIS_EN.Enums.Rights.SelectClaimForReview : Return (Roles.CHFMedicalOfficer And RoleId)
            Case IMIS_EN.Enums.Rights.ValuateClaim : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.DownloadClaimForPayment : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteClaim : Return (Roles.ClaimAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.UpdateClaims : Return (Roles.CHFMedicalOfficer And RoleId)
            Case IMIS_EN.Enums.Rights.ProcessClaims : Return (Roles.CHFMedicalOfficer And RoleId)
            Case IMIS_EN.Enums.Rights.ClaimsBatchClosure : Return (Roles.ClaimAdministrator + Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.ReviewClaim : Return (Roles.CHFMedicalOfficer And RoleId)
            Case IMIS_EN.Enums.Rights.ClaimOverview : Return (Roles.CHFMedicalOfficer And RoleId)
            Case IMIS_EN.Enums.Rights.LoadClaim : Return (Roles.CHFAccountant + Roles.CHFClerk + Roles.ClaimAdministrator + Roles.ClaimContributor And RoleId)
                'Administration
            Case IMIS_EN.Enums.Rights.AddUser : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteUser : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.FindUser : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditUser : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.FindOfficer : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.AddOfficer : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditOfficer : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteOfficer : Return (Roles.CHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.FindClaimAdministrator : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.AddClaimAdministrator : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditClaimAdministrator : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteClaimAdministrator : Return (Roles.CHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.InquirePolicy : Return (Roles.Receptionist And RoleId)
            Case IMIS_EN.Enums.Rights.DownloadDataOnPremiumsToAccounting : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.UploadBatchXMLFile : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.DownloadBatchForPayment : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.UploadAccountingDataOnBatchPayment : Return (Roles.CHFAccountant And RoleId)

            Case IMIS_EN.Enums.Rights.AddProduct : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.FindProduct : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditProduct : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteProduct : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.ViewProduct : Return (Roles.CHFAdministrator + Roles.CHFClerk + Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.DuplicateProduct : Return (Roles.CHFAdministrator And RoleId)


            Case IMIS_EN.Enums.Rights.DownloadToOfflineClient : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.UploadDataFromOfflineClient : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.CreateDataForAccounting : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.CreateDataForReinsurance : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.CreateManagerialStatistics : Return (Roles.CHFManager And RoleId)

            Case IMIS_EN.Enums.Rights.EditMedicalItem : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteMedicalItem : Return (Roles.CHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.AddPriceListMedicalServices : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.FindPriceListMedicalServices : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditPriceListMedicalServices : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeletePriceListMedicalServices : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DuplicatePriceListMedicalServices : Return (Roles.CHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.AddPriceListMedicalItems : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.FindPriceListMedicalItems : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditPriceListMedicalItems : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeletePriceListMedicalItems : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DuplicatePriceListMedicalItems : Return (Roles.CHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.AddPayer : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.FindPayer : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditPayer : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeletePayer : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.ViewPayer : Return (Roles.CHFAdministrator + Roles.CHFClerk + Roles.CHFAccountant And RoleId)

            Case IMIS_EN.Enums.Rights.AddHealthFacility : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.FindHealthFacility : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditHealthFacility : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteHealthFacility : Return (Roles.CHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.AddMedicalService : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.FindMedicalService : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditMedicalService : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteMedicalService : Return (Roles.CHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.AddMedicalItem : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.FindMedicalItem : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EmailSettings : Return (Roles.IMISAdministrator And RoleId)

                'locations
            Case IMIS_EN.Enums.Rights.FindLocations : Return (Roles.CHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.AddDistrict : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditDistrict : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteDistrict : Return (Roles.CHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.AddWard : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditWard : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteWard : Return (Roles.CHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.AddVillage : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.EditVillage : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DeleteVillage : Return (Roles.CHFAdministrator And RoleId)

                'Tools
            Case IMIS_EN.Enums.Rights.UploadICD : Return (Roles.IMISAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.RenewPolicy : Return (Roles.CHFClerk And RoleId)
            Case IMIS_EN.Enums.Rights.FeedbackPrompt : Return (Roles.CHFMedicalOfficer And RoleId)
            Case IMIS_EN.Enums.Rights.IMISExtracts : Return (Roles.HFAdministrator + Roles.CHFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.OfflineExtracts : Return (Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.OfflineClaims : Return (Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)

            Case IMIS_EN.Enums.Rights.Utilities : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DatabaseBackup : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.DatabaseRestore : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.ExecuteScripts : Return (Roles.IMISAdministrator + Roles.HFAdministrator + Roles.OfflineCHFAdministrator And RoleId)

                'Reports
            Case IMIS_EN.Enums.Rights.Reports : Return (Roles.CHFManager + Roles.CHFAccountant + Roles.IMISAdministrator + Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.PremiumDistribution : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.PremiumCollection : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.ProductSales : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.Indicators : Return (Roles.CHFManager And RoleId)
            Case IMIS_EN.Enums.Rights.UserActivity : Return (Roles.IMISAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.InsureeWithoutPhotos : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.MatchingFunds : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.ClaimOverviewReport : Return (Roles.ClaimAdministrator + Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.FamiliesInsureesOverview : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.StatusOfRegister : Return (Roles.CHFAdministrator And RoleId)
            Case IMIS_EN.Enums.Rights.PercentageReferrals : Return (Roles.ClaimAdministrator + Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.FamiliesInsureesOverview : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.PendingInsurees : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.RejectedPhoto : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.AddFund : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.Renewals : Return (Roles.CHFAccountant And RoleId)
            Case IMIS_EN.Enums.Rights.ViewReport : Return (Roles.CHFManager + Roles.CHFAccountant + Roles.CHFClerk + Roles.CHFMedicalOfficer And RoleId)

            Case IMIS_EN.Enums.Rights.CapitationPayment : Return (Roles.CHFAccountant And RoleId)



            Case Else
                Return False
        End Select
    End Function
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
    Public Function RunPageSecurity(ByVal PageName As IMIS_EN.Enums.Pages, ByRef PageObj As System.Web.UI.Page) As Boolean

        Dim RoleId As Integer = getRoleId(PageObj.Session("User"))
        Dim HasAccess As Boolean = False

        Dim PageRights As List(Of Rights) = GetPageRights(PageName)

        For i As Integer = 0 To PageRights.Count - 1
            HasAccess = CheckRoles(PageRights.ElementAt(i), RoleId)
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

            Case IMIS_EN.Enums.Pages.FindFamily                   'policy & insurance
                PageRights.Add(Rights.FindFamily)

            Case IMIS_EN.Enums.Pages.Family
                PageRights.Add(Rights.AddFamily)

            Case IMIS_EN.Enums.Pages.ChangeFamily
                PageRights.Add(Rights.EditFamily)
                PageRights.Add(Rights.ChangeFamilyHead)
                PageRights.Add(Rights.MovingInsuree)
                PageRights.Add(Rights.ViewFamily)

            Case IMIS_EN.Enums.Pages.OverviewFamily
                PageRights.Add(Rights.OverviewFamily)

            Case IMIS_EN.Enums.Pages.FindInsuree
                PageRights.Add(Rights.FindInsuree)

            Case IMIS_EN.Enums.Pages.Insuree
                PageRights.Add(Rights.EditInsuree)
                PageRights.Add(Rights.AddInsuree)
                PageRights.Add(Rights.ViewInsuree)

            Case IMIS_EN.Enums.Pages.FindPolicy
                PageRights.Add(Rights.FindPolicy)

            Case IMIS_EN.Enums.Pages.Policy
                PageRights.Add(Rights.AddPolicy)
                PageRights.Add(Rights.EditPolicy)
                PageRights.Add(Rights.ViewPolicy)

            Case IMIS_EN.Enums.Pages.FindPremium
                PageRights.Add(Rights.FindPremium)

            Case IMIS_EN.Enums.Pages.Premium
                PageRights.Add(Rights.EditPremium)
                PageRights.Add(Rights.AddPremium)
                PageRights.Add(Rights.ViewPremium)

            Case IMIS_EN.Enums.Pages.FindClaim     'claims
                PageRights.Add(Rights.FindClaim)

            Case IMIS_EN.Enums.Pages.ClaimOverview
                PageRights.Add(Rights.ProcessClaims)
                PageRights.Add(Rights.UpdateClaims)

            Case IMIS_EN.Enums.Pages.Claim
                PageRights.Add(Rights.EnterClaim)
                PageRights.Add(Rights.LoadClaim)

            Case IMIS_EN.Enums.Pages.ClaimFeedback
                PageRights.Add(Rights.EnterFeedback)

            Case IMIS_EN.Enums.Pages.ClaimReview
                PageRights.Add(Rights.ReviewClaim)

            Case IMIS_EN.Enums.Pages.ProcessBatches
                PageRights.Add(Rights.ValuateClaim)

            Case IMIS_EN.Enums.Pages.FindProduct                 'administration
                PageRights.Add(Rights.FindProduct)

            Case IMIS_EN.Enums.Pages.Product
                PageRights.Add(Rights.AddProduct)
                PageRights.Add(Rights.EditProduct)
                PageRights.Add(Rights.ViewProduct)
                PageRights.Add(Rights.FindProduct)

            Case IMIS_EN.Enums.Pages.FindHealthFacility
                PageRights.Add(Rights.FindHealthFacility)

            Case IMIS_EN.Enums.Pages.HealthFacility
                PageRights.Add(Rights.AddHealthFacility)
                PageRights.Add(Rights.EditHealthFacility)
                PageRights.Add(Rights.FindHealthFacility)

            Case IMIS_EN.Enums.Pages.FindPriceListMI
                PageRights.Add(Rights.FindPriceListMedicalItems)
                'Functionalities.Add(Roles.EditPriceListMedicalItems)
                'Functionalities.Add(Roles.AddPriceListMedicalItems)
                'Functionalities.Add(Roles.DeletePriceListMedicalItems)

            Case IMIS_EN.Enums.Pages.PriceListMI
                PageRights.Add(Rights.EditPriceListMedicalItems)
                PageRights.Add(Rights.AddPriceListMedicalItems)
                PageRights.Add(Rights.FindPriceListMedicalItems)

            Case IMIS_EN.Enums.Pages.FindPriceListMS
                PageRights.Add(Rights.FindPriceListMedicalServices)
                'Functionalities.Add(Roles.EditPriceListMedicalServices)
                'Functionalities.Add(Roles.AddPriceListMedicalServices)
                'Functionalities.Add(Roles.DeletePriceListMedicalServices)

            Case IMIS_EN.Enums.Pages.PriceListMS
                PageRights.Add(Rights.EditPriceListMedicalServices)
                PageRights.Add(Rights.AddPriceListMedicalServices)
                PageRights.Add(Rights.FindPriceListMedicalServices)

            Case IMIS_EN.Enums.Pages.FindMedicalService
                PageRights.Add(Rights.FindMedicalService)
                'Functionalities.Add(Roles.EditMedicalService)
                'Functionalities.Add(Roles.AddMedicalService)
                'Functionalities.Add(Roles.DeleteMedicalService)

            Case IMIS_EN.Enums.Pages.MedicalService
                PageRights.Add(Rights.EditMedicalService)
                PageRights.Add(Rights.AddMedicalService)
                PageRights.Add(Rights.FindMedicalService)

            Case IMIS_EN.Enums.Pages.FindMedicalItem
                PageRights.Add(Rights.FindMedicalItem)
                'Functionalities.Add(Roles.EditMedicalItem)
                'Functionalities.Add(Roles.AddMedicalItem)
                'Functionalities.Add(Roles.DeleteMedicalItem)
            Case IMIS_EN.Enums.Pages.MedicalItem
                PageRights.Add(Rights.EditMedicalItem)
                PageRights.Add(Rights.AddMedicalItem)
                PageRights.Add(Rights.FindMedicalItem)

            Case IMIS_EN.Enums.Pages.FindUser
                PageRights.Add(Rights.FindUser)
                'Functionalities.Add(Roles.EditUser)
                'Functionalities.Add(Roles.AddUser)
                'Functionalities.Add(Roles.DeleteUser)

            Case IMIS_EN.Enums.Pages.User
                PageRights.Add(Rights.AddUser)
                PageRights.Add(Rights.EditUser)
                PageRights.Add(Rights.FindUser)

            Case IMIS_EN.Enums.Pages.FindOfficer
                PageRights.Add(Rights.FindOfficer)
                'Functionalities.Add(Roles.AddOfficer)
                'Functionalities.Add(Roles.EditOfficer)
                'Functionalities.Add(Roles.DeleteOfficer)

            Case IMIS_EN.Enums.Pages.Officer
                PageRights.Add(Rights.AddOfficer)
                PageRights.Add(Rights.EditOfficer)
                PageRights.Add(Rights.FindOfficer)

            Case IMIS_EN.Enums.Pages.FindClaimAdministrator
                PageRights.Add(Rights.FindClaimAdministrator)
                'Functionalities.Add(Roles.AddOfficer)
                'Functionalities.Add(Roles.EditOfficer)
                'Functionalities.Add(Roles.DeleteOfficer)

            Case IMIS_EN.Enums.Pages.ClaimAdministrator
                PageRights.Add(Rights.AddClaimAdministrator)
                PageRights.Add(Rights.EditClaimAdministrator)
                PageRights.Add(Rights.FindClaimAdministrator)

            Case IMIS_EN.Enums.Pages.FindPayer
                PageRights.Add(Rights.FindPayer)
                'Functionalities.Add(Roles.AddPayer)
                'Functionalities.Add(Roles.EditPayer)
                'Functionalities.Add(Roles.DeletePayer)

            Case IMIS_EN.Enums.Pages.Payer
                PageRights.Add(Rights.AddPayer)
                PageRights.Add(Rights.EditPayer)
                PageRights.Add(Rights.ViewPayer)
                PageRights.Add(Rights.FindPayer)


            Case IMIS_EN.Enums.Pages.Locations
                PageRights.Add(Rights.FindLocations)


            Case IMIS_EN.Enums.Pages.UploadICD                     'Tools
                PageRights.Add(Rights.UploadICD)

            Case IMIS_EN.Enums.Pages.PolicyRenewal
                PageRights.Add(Rights.RenewPolicy)

            Case IMIS_EN.Enums.Pages.IMISExtracts
                PageRights.Add(Rights.IMISExtracts)

            Case IMIS_EN.Enums.Pages.Report
                PageRights.Add(Rights.ViewReport)

            Case IMIS_EN.Enums.Pages.Reports
                PageRights.Add(Rights.Reports)

            Case IMIS_EN.Enums.Pages.Utilities
                PageRights.Add(Rights.Utilities)

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
    Public Function getUserRoles(ByVal RoleId As Integer, Optional ByVal showSelect As Boolean = False) As DataTable
        Dim imisgen As New GeneralBL
        Dim dtbl As New DataTable
        Dim dr As DataRow
        dtbl.Columns.Add("Code")
        dtbl.Columns.Add("Role")
        If showSelect = True Then
            dr = dtbl.NewRow
            dr("Code") = 0
            dr("Role") = imisgen.getMessage("T_SELECTROLE")
            dtbl.Rows.Add(dr)
        End If
        Dim i As Integer = 1
        Do Until i > 1048576 '1024
            If (i And RoleId) > 0 Then
                dr = dtbl.NewRow
                dr("Code") = i
                dr("Role") = ReturnRole(i)
                dtbl.Rows.Add(dr)
            End If
            i += i
        Loop
        Return dtbl
    End Function

    Public Function GetUsers(ByVal eUser As IMIS_EN.tblUsers, Optional ByVal Legacy As Boolean = False, Optional ByVal DistrictId As Integer = 0) As DataTable
        Dim getDataTable As New IMIS_DAL.UsersDAL
        eUser.LastName += "%"
        eUser.OtherNames += "%"
        eUser.Phone += "%"
        eUser.LoginName += "%"
        eUser.EmailId = "%" & eUser.EmailId & "%"


        Return getDataTable.GetUsers(eUser, Legacy, DistrictId)


    End Function
    Public Function SaveUser(ByRef eUser As IMIS_EN.tblUsers) As Integer '0 - Insert, 1 - Exists, 2 - Update
        Dim users As New IMIS_DAL.UsersDAL
        Dim dt As DataTable = users.CheckIfUserExists(eUser)
        If dt.Rows.Count > 0 Then Return 1
        If eUser.UserID = 0 Then
            users.InsertUser(eUser)
            Return 0
        Else
            users.UpdateUser(eUser)
            Return 2
        End If
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
    Public Sub ChangePassword(ByVal euser As IMIS_EN.tblUsers)
        Dim users As New IMIS_DAL.UsersDAL
        users.ChangePassword(euser)
    End Sub

    Public Function IsUserExists(ByVal UserID As Integer) As Boolean
        Dim User As New IMIS_DAL.UsersDAL
        Return User.IsUserExists(UserID)
    End Function
End Class
