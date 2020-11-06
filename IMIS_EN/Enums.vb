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

Public Class Enums


    Enum Rights

        '************Insuree and Policy
        'Family
        Family = 101000
        FamilySearch = 101001
        FamilyAdd = 101002
        FamilyEdit = 101003
        FamilyDelete = 101004
        'Insuree
        Insuree = 101100
        InsureeSearch = 101101
        InsureeAdd = 101102
        InsureeEdit = 101103
        InsureeDelete = 101104
        InsureeEnquire = 101105
        'Policy
        PolicySearch = 101201
        PolicyAdd = 101202
        PolicyEdit = 101203
        PolicyDelete = 101204
        PolicyRenew = 101205
        'Contribution
        ContributionSearch = 101301
        ContributionAdd = 101302
        ContributionEdit = 101303
        ContributionDelete = 101304
        'Payment
        PaymentSearch = 101401
        PaymentAdd = 101402
        PaymentEdit = 101403
        PaymentDelete = 101404

        '***************************************Claims     11 00 00

        'Claims
        Claims = 111000
        ClaimSearch = 111001
        ClaimAdd = 111002
        ClaimDelete = 111004
        ClaimLoad = 111005
        ClaimPrint = 111006
        ClaimSubmit = 111007
        ClaimReview = 111008
        ClaimFeedback = 111009
        ClaimUpdate = 111010
        ClaimProcess = 111011
        ClaimRestore = 111012         'RFC 111 06/09/2019

        'Batch
        Batch = 111100
        BatchProcess = 111101
        BatchFilter = 111102
        BatchPreview = 111103

        '***************************************Administrations 12 00 00

        'Product
        Product = 121000
        ProductSearch = 121001
        ProductAdd = 121002
        ProductEdit = 121003
        ProductDelete = 121004
        ProductDuplicate = 121005


        'Health Facilities
        HealthFacility = 121100
        HealthFacilitySearch = 121101
        HealthFacilityAdd = 121102
        HealthFacilityEdit = 121103
        HealthFacilityDelete = 121104

        'PriceLists Medical Services
        PriceListMedicalServices = 121200
        FindPriceListMedicalServices = 121201
        AddPriceListMedicalServices = 121202
        EditPriceListMedicalServices = 121203
        DeletePriceListMedicalServices = 121204
        DuplicatePriceListMedicalServices = 121205

        'Pricelists Medical Items
        PriceListMedicalItems = 121300
        FindPriceListMedicalItems = 121301
        AddPriceListMedicalItems = 121302
        EditPriceListMedicalItems = 121303
        DeletePriceListMedicalItems = 121304
        DuplicatePriceListMedicalItems = 121305

        'Medical Services
        MedicalService = 121400
        FindMedicalService = 121401
        AddMedicalService = 121402
        EditMedicalService = 121403
        DeleteMedicalService = 121404

        'Medical Item
        MedicalItem = 122100
        FindMedicalItem = 122101
        AddMedicalItem = 122102
        EditMedicalItem = 122103
        DeleteMedicalItem = 122104

        'Enrolment Officers
        Officer = 121500
        FindOfficer = 121501
        AddOfficer = 121502
        EditOfficer = 121503
        DeleteOfficer = 121504

        'claim administrator
        ClaimAdministrator = 121600
        FindClaimAdministrator = 121601
        AddClaimAdministrator = 121602
        EditClaimAdministrator = 121603
        DeleteClaimAdministrator = 121604

        'Users
        Users = 121700
        UsersSearch = 121701
        UsersAdd = 121702
        UsersEdit = 121703
        UsersDelete = 121704

        'Payers
        Payer = 121800
        FindPayer = 121801
        AddPayer = 121802
        EditPayer = 121803
        DeletePayer = 121804
        ViewPayer = 121801

        'locations
        Locations = 121900
        FindLocations = 121901
        AddLocations = 121902
        EditLocations = 121903
        DeleteLocations = 121904
        MoveLocations = 121905

        'UserProfile
        userProfiles = 122000
        FindUserProfile = 122001
        AddUserProfile = 122002
        DeleteUserProfile = 122004
        EditUserProfile = 122003
        DuplicateUserProfile = 122005

        '****************************************Tools 13 00 00
        Tools = 130000
        Registers = 131000
        DiagnosesUpload = 131001
        DiagnosesDownload = 131002

        HealthFacilitiesUpload = 131003
        HealthFacilitiesDownload = 131004

        LocationUpload = 131005
        LocationDonwload = 131006


        Extracts = 131100
        ExtractMasterDataDownload = 131101
        ExtractPhoneExtractsCreate = 131102
        ExtractOfflineExtractCreate = 131103
        ExtractClaimUpload = 131104
        ExtractEnrolmentsUpload = 131105
        ExtractFeedbackUpload = 131106

        '****************************************Reports 1312 00
        'Reports
        Reports = 131200
        ReportsPrimaryOperationalIndicatorPolicies = 131201
        ReportsPrimaryOperationalIndicatorsClaims = 131202
        ReportsDerivedOperationalIndicators = 131203
        ReportsContributionCollection = 131204
        ReportsProductSales = 131205
        ReportsContributionDistribution = 131206
        ReportsUserActivity = 131207
        ReportsEnrolmentPerformanceIndicators = 131208
        ReportsStatusOfRegister = 131209
        ReportsInsureeWithoutPhotos = 131210
        ReportsPaymentCategoryOverview = 131211
        ReportsMatchingFunds = 131212
        ReportsClaimOverviewReport = 131213
        ReportsPercentageReferrals = 131214
        ReportsFamiliesInsureesOverview = 131215
        ReportsPendingInsurees = 131216
        ReportsRenewals = 131217
        ReportsCapitationPayment = 131218
        ReportRejectedPhoto = 131219
        ReportsContributionPayment = 131220
        ReportsControlNumberAssignment = 131221
        ReportsOverviewOfCommissions = 131222
        ReportsClaimHistoryReport = 131223


        '****************************************Utilities/Email Setting 131300 
        Utilities = 131300
        DatabaseBackup = 131301
        DatabaseRestore = 131302
        ExecuteScripts = 131303
        EmailSettings = 131304

        '****************************************Funding 13 14 00  
        FundingSave = 131401









    End Enum

    Enum Pages
        Home

        'policy & insurance
        FindFamily
        Family
        ChangeFamily
        OverviewFamily
        FindInsuree
        Insuree
        FindPolicy
        Policy
        FindPremium
        Premium
        FindPayment

        'claims
        FindClaim
        ClaimOverview
        Claim
        ClaimFeedback
        ClaimReview
        ProcessBatches

        'administration
        FindProduct
        Product
        FindHealthFacility
        HealthFacility
        FindPriceListMI
        PriceListMI
        FindPriceListMS
        PriceListMS
        FindMedicalItem
        MedicalItem
        FindMedicalService
        MedicalService
        FindUser
        User
        UserProfiles
        Role
        FindOfficer
        Officer
        FindClaimAdministrator
        ClaimAdministrator
        FindPayer
        Payer
        Locations
        EmailSettings

        'tools
        UploadICD
        PolicyRenewal
        IMISExtracts
        Report
        Reports
        FeedbackPrompt
        Utilities
        Funding
    End Enum
End Class
