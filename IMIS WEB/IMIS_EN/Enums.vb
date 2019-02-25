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

        '****************************************Insuree and Policy  10 00 00
        'Family
        Family = 101000
        FindFamily = 101001
        AddFamily = 101002
        EditFamily = 101003
        DeleteFamily = 101004
        ViewFamily = 101001

        'Insuree
        Insuree = 101100
        FindInsuree = 101101
        AddInsuree = 101102
        EditInsuree = 101103
        DeleteInsuree = 101104
        EnquireInsuree = 101105             'NEW

        'Policy
        FindPolicy = 101201
        AddPolicy = 101202
        EditPolicy = 101203
        DeletePolicy = 101204
        RenewPolicy = 101205
        ViewPolicy = 101201

        'Contribution
        FindContribution = 101301           'NEW
        AddContribution = 101302            'NEW
        EditContribution = 101303           'NEW    
        DeleteContribution = 101304         'NEW
        ViewContribution = 101301           'NEW 

        'Payment
        FindPayment = 101401
        AddPayment = 101402
        EditPayment = 101403
        DeletePayment = 101404

        '***************************************Claims     11 00 00

        'Claims
        Claims = 111001
        FindClaim = 111001
        EnterClaim = 111002
        EditClaim = 111003
        DeleteClaim = 111004
        LoadClaim = 111005
        PrintClaim = 111006                  'NEW
        SubmitClaim = 111007                 'NEW
        ReviewClaim = 111008
        Feedback = 111009
        UpdateClaims = 111010
        ProcessClaims = 111011
        Filter = 111012
        Preview = 111013
        'ClaimsBatchClosure = 111014
        'ClaimOverview = 111015

        'Batch
        Batch = 111100
        BatchProcess = 111101
        BatchFilter = 111102
        BatchPreview = 111103

        '***************************************Administrations 12 00 00

        'Product
        Product = 121000
        FindProduct = 121001
        AddProduct = 121002
        EditProduct = 121003
        DeleteProduct = 121004
        DuplicateProduct = 121005
        ViewProduct = 121001

        'Health Facilities
        HealthFacility = 121100
        FindHealthFacility = 121101
        AddHealthFacility = 121102
        EditHealthFacility = 121103
        DeleteHealthFacility = 121104

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
        FindUser = 121701
        AddUser = 121702
        EditUser = 121703
        DeleteUser = 121704

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
        '--
        '--

        '****************************************Registers 13 00 00
        Tools = 130000
        Registers = 131000
        DiagnosesUpload = 131001                'NEW
        DiagnosesDownload = 131002              'NEW

        HealthFacilitiesUpload = 131003        'NEW
        HealthFacilitiesDownload = 131004       'NEW

        LocationUpload = 131005                'NEW
        LocationDonwload = 131006               'NEW

        '****************************************Extracts 15 00 00
        Extracts = 131100
        'Master Data
        MasterDataDownload = 131101             'NEW

        'Phone Extract
        PhoneExtractsCreate = 131102            'NEW

        'Offline Extract
        OfflineExtractCreate = 131103           'NEW

        'Claims
        ClaimXMLUpload = 131104

        'Enrolment
        EnrolmentsUpload = 131105               'NEW

        'Feedback
        FeedbackUpload = 131106                 'NEW

        '****************************************Reports 1312 00
        'Reports
        Reports = 131200                              ' Add to include all reports                
        PrimaryOperationalIndicatorPolicies = 131201  'NEW 
        PrimaryOperationalIndicatorsClaims = 131202   'NEW
        DerivedOperationalIndicators = 131203         'NEW
        ContributionCollection = 131204               'NEW
        ProductSales = 131205                         'NEW
        ContributionDistribution = 131206             'NEW
        UserActivity = 131207                         'NEW
        EnrolmentPerformanceIndicators = 131208       'NEW
        StatusOfRegister = 131209                     'NEW
        InsureeWithoutPhotos = 131210
        PaymentCategoryOverview = 131211              'NEW
        MatchingFunds = 131212
        ClaimOverviewReport = 131213
        PercentageReferrals = 131214
        FamiliesInsureesOverview = 131215
        PendingInsurees = 131216
        Renewals = 131217
        CapitationPayment = 131218
        RejectedPhoto = 131219
        ContributionPayment = 131220                  'NEW
        ControlNumberAssignment = 131221              'NEW 
        OverviewOfCommissions = 131222
        ClaimHistoryReport = 131223
        '-ViewReport = 160007

        '****************************************Utilities/Email Setting 131300 
        Utilities = 131300
        DatabaseBackup = 131301
        DatabaseRestore = 131302
        ExecuteScripts = 131303
        EmailSettings = 131304

        '****************************************Funding 13 14 00  
        AddFund = 131401

        '**********************************************************************************END***************************************************


        'IN LEGACY START 
        SelectClaimForReview = 111016
        EnterFeedback = 111009
        'ValuateClaim = 111017 Amended on top
        ' NEW starts 
        SelectClaimForFeedback = 111009
        OverviewFamily = 101005
        ChangeFamilyHead = 101005
        MovingInsuree = 101006
        'Ward



        FeedbackPrompt = 191001                 'NOT IN THE NEW DOCUMENT 
        UploadICD = 191002                      'NOT IN THE NEW DOCUMENT  

        OfflineExtracts
        OfflineClaims
        OfflineEnrolments

        'IN LEGACY END


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
        HealthFacility
        FindHealthFacility
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
