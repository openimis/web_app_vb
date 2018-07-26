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
        FindFamily
        AddFamily
        EditFamily
        ChangeFamilyHead
        MovingInsuree
        DeleteFamily
        ViewFamily

        'Insuree
        FindInsuree
        AddInsuree
        EditInsuree
        DeleteInsuree
        ViewInsuree

        'Policy
        FindPolicy
        AddPolicy
        EditPolicy
        DeletePolicy
        ViewPolicy

        'Premium
        FindPremium
        AddPremium
        EditPremium
        DeletePremium
        ViewPremium

        'OverviewFamily
        OverviewFamily

        '**************Claims
        'FindClaim
        FindClaim
        DeleteClaim
        ClaimsBatchClosure

        'Claim
        EnterClaim
        LoadClaim

        'ClaimOverview
        ClaimOverview
        SelectClaimForFeedback
        SelectClaimForReview
        ProcessClaims
        UpdateClaims

        'ClaimReview
        ReviewClaim

        'ClaimFeedback
        EnterFeedback

        'ProcessBatch
        ValuateClaim

        '**undefined***
        PromptingFeedbackCollection
        CheckClaimPlausibility
        ClaimXMLUpload
        DownloadClaimForPayment

        '*************administration
        AddUser
        FindUser
        EditUser
        DeleteUser
        AddOfficer
        FindOfficer
        EditOfficer
        DeleteOfficer
        AddPayer
        DeletePayer
        AddHealthFacility
        FindHealthFacility
        EditHealthFacility
        DeleteHealthFacility
        FindPayer
        EditPayer
        ViewPayer
        AddMedicalService
        FindMedicalService
        EditMedicalService
        DeleteMedicalService
        AddMedicalItem
        FindMedicalItem
        EditMedicalItem
        DeleteMedicalItem
        AddPriceListMedicalServices
        FindPriceListMedicalServices
        EditPriceListMedicalServices
        DeletePriceListMedicalServices
        DuplicatePriceListMedicalServices
        AddPriceListMedicalItems
        FindPriceListMedicalItems
        EditPriceListMedicalItems
        DeletePriceListMedicalItems
        DuplicatePriceListMedicalItems
        'claim administrator
        AddClaimAdministrator
        FindClaimAdministrator
        EditClaimAdministrator
        DeleteClaimAdministrator
        'locations
        FindLocations
        AddDistrict
        EditDistrict
        DeleteDistrict
        AddWard
        EditWard
        DeleteWard
        AddVillage
        EditVillage
        DeleteVillage

        'product
        ViewProduct
        AddProduct
        FindProduct
        EditProduct
        DeleteProduct
        DuplicateProduct

        'Email
        EmailSettings

        '**************************************************Tools
        RenewPolicy
        InquirePolicy
        FeedbackPrompt
        UploadICD
        DownloadDataOnPremiumsToAccounting
        UploadBatchXMLFile
        DownloadBatchForPayment
        UploadAccountingDataOnBatchPayment
        DownloadToOfflineClient
        UploadDataFromOfflineClient
        CreateDataForAccounting
        CreateDataForReinsurance
        CreateManagerialStatistics

        'ImisExtracts
        IMISExtracts
        OfflineExtracts
        OfflineClaims
        OfflineEnrolments
        '/** added, not yet reviewed **/
        'Utilities
        Utilities
        DatabaseBackup
        DatabaseRestore
        ExecuteScripts

        'Reports
        Reports
        PremiumCollection
        ProductSales
        Indicators
        PremiumDistribution
        UserActivity
        InsureeWithoutPhotos
        MatchingFunds
        ClaimOverviewReport
        PercentageReferrals
        FamiliesInsureesOverview
        StatusOfRegister
        PendingInsurees
        Renewals
        CapitationPayment
        RejectedPhoto
        'Report
        ViewReport

        AddFund
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
