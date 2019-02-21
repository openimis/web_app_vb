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

Public Class eExtractInfo

    Private _DistrictName As String
    ' Private _DistrictID As Integer
    Private _ExtractDate As Global.System.Nullable(Of Date)
    Private _ExtractSequence As Integer
    Private _ExtractFileName As String
    Private _ExtractStatus As Integer
    Private _ExtractType As Integer
    Private _AudituserID As Integer
    Private _HFID As Integer

    Private _ZippedPhotosCS As Integer

    Private _DistrictsCS As Integer
    Private _DistrictsIns As Integer
    Private _DistrictsUpd As Integer

    Private _WardsCS As Integer
    Private _WardsIns As Integer
    Private _WardsUpd As Integer

    Private _VillagesCS As Integer
    Private _VillagesIns As Integer
    Private _VillagesUpd As Integer

    Private _ItemsCS As Integer
    Private _ItemsIns As Integer
    Private _ItemsUpd As Integer

    Private _ServicesCS As Integer
    Private _ServicesIns As Integer
    Private _ServicesUpd As Integer

    Private _PLItemsCS As Integer
    Private _PLItemsIns As Integer
    Private _PLItemsUpd As Integer

    Private _PLServicesCS As Integer
    Private _PLServicesIns As Integer
    Private _PLServicesUpd As Integer

    Private _PLItemsDetailsCS As Integer
    Private _PLItemsDetailsIns As Integer
    Private _PLItemsDetailsUpd As Integer

    Private _PLServicesDetailsCS As Integer
    Private _PLServicesDetailsIns As Integer
    Private _PLServicesDetailsUpd As Integer

    Private _ICDCS As Integer
    Private _ICDIns As Integer
    Private _ICDUpd As Integer

    Private _HFCS As Integer
    Private _HFIns As Integer
    Private _HFUpd As Integer

    Private _ClaimAdminCS As Integer
    Private _ClaimAdminIns As Integer
    Private _ClaimAdminUpd As Integer

    Private _PayerCS As Integer
    Private _PayerIns As Integer
    Private _payerUpd As Integer

    Private _OfficerCS As Integer
    Private _OfficerIns As Integer
    Private _OfficerUpd As Integer

    Private _ProductCS As Integer
    Private _ProductIns As Integer
    Private _ProductUpd As Integer

    Private _ProductItemsCS As Integer
    Private _ProductItemsIns As Integer
    Private _ProductItemsUpd As Integer

    Private _ProductServicesCS As Integer
    Private _ProductServicesIns As Integer
    Private _ProductServicesUpd As Integer

    Private _RelDistrCS As Integer
    Private _RelDistrIns As Integer
    Private _RelDistrUpd As Integer

    Private _FamiliesCS As Integer
    Private _FamiliesIns As Integer
    Private _FamiliesUpd As Integer

    Private _InsureeCS As Integer
    Private _InsureeIns As Integer
    Private _InsureeUpd As Integer

    Private _PhotoCS As Integer
    Private _PhotoIns As Integer
    Private _PhotoUpd As Integer

    Private _PolicyCS As Integer
    Private _PolicyIns As Integer
    Private _PolicyUpd As Integer

    Private _PremiumCS As Integer
    Private _PremiumIns As Integer
    Private _PremiumUpd As Integer


    Public Property DistrictName() As String
        Get
            Return _DistrictName
        End Get
        Set(ByVal value As String)
            _DistrictName = value
        End Set
    End Property



    Public Property ZippedPhotosCS() As Integer
        Get
            Return _ZippedPhotosCS
        End Get
        Set(ByVal value As Integer)
            _ZippedPhotosCS = value
        End Set
    End Property

    Public Property AuditUserID() As Integer
        Get
            Return _AudituserID
        End Get
        Set(ByVal value As Integer)
            _AudituserID = value
        End Set
    End Property

    Public Property ExtractDate() As Global.System.Nullable(Of Date)
        Get
            Return _ExtractDate
        End Get
        Set(ByVal value As Global.System.Nullable(Of Date))
            _ExtractDate = value
        End Set
    End Property

    Public Property ExtractSequence() As Integer
        Get
            Return _ExtractSequence
        End Get
        Set(ByVal value As Integer)
            _ExtractSequence = value
        End Set
    End Property

    Public Property ExtractStatus() As Integer
        Get
            Return _ExtractStatus
        End Get
        Set(ByVal value As Integer)
            _ExtractStatus = value
        End Set
    End Property

    Public Property HFID() As Integer
        Get
            Return _HFID
        End Get
        Set(ByVal value As Integer)
            _HFID = value
        End Set
    End Property

    Public Property ExtractType() As Integer
        Get
            Return _ExtractType
        End Get
        Set(ByVal value As Integer)
            _ExtractType = value
        End Set
    End Property

    Public Property ExtractFileName() As String
        Get
            Return _ExtractFileName
        End Get
        Set(ByVal value As String)
            _ExtractFileName = value
        End Set
    End Property
    Public Property RegionId As Integer
    Public Property DistrictId As Integer

    Public Property RegionCS() As Integer
    Public Property LocationsIns As Integer
    Public Property LocationsUp As Integer
    Public Property LocationsCS As Integer
    Public Property LocationId As Integer
    Public Property WithInsuree As Boolean



    Public Property DistrictsCS() As Integer
        Get
            Return _DistrictsCS
        End Get
        Set(ByVal value As Integer)
            _DistrictsCS = value
        End Set
    End Property

    Public Property DistrictsIns() As Integer
        Get
            Return _DistrictsIns
        End Get
        Set(ByVal value As Integer)
            _DistrictsIns = value
        End Set
    End Property

    Public Property DistrictsUpd() As Integer
        Get
            Return _DistrictsUpd
        End Get
        Set(ByVal value As Integer)
            _DistrictsUpd = value
        End Set
    End Property

    Public Property WardsCS() As Integer
        Get
            Return _WardsCS
        End Get
        Set(ByVal value As Integer)
            _WardsCS = value
        End Set
    End Property

    Public Property WardsIns() As Integer
        Get
            Return _WardsIns
        End Get
        Set(ByVal value As Integer)
            _WardsIns = value
        End Set
    End Property

    Public Property WardsUpd() As Integer
        Get
            Return _WardsUpd
        End Get
        Set(ByVal value As Integer)
            _WardsUpd = value
        End Set
    End Property


    Public Property VillagesCS() As Integer
        Get
            Return _VillagesCS
        End Get
        Set(ByVal value As Integer)
            _VillagesCS = value
        End Set
    End Property

    Public Property VillagesIns() As Integer
        Get
            Return _VillagesIns
        End Get
        Set(ByVal value As Integer)
            _VillagesIns = value
        End Set
    End Property

    Public Property VillagesUpd() As Integer
        Get
            Return _VillagesUpd
        End Get
        Set(ByVal value As Integer)
            _VillagesUpd = value
        End Set
    End Property

    Public Property ItemsCS() As Integer
        Get
            Return _ItemsCS
        End Get
        Set(ByVal value As Integer)
            _ItemsCS = value
        End Set
    End Property

    Public Property ItemsIns() As Integer
        Get
            Return _ItemsIns
        End Get
        Set(ByVal value As Integer)
            _ItemsIns = value
        End Set
    End Property

    Public Property ItemsUpd() As Integer
        Get
            Return _ItemsUpd
        End Get
        Set(ByVal value As Integer)
            _ItemsUpd = value
        End Set
    End Property

    Public Property ServicesCS() As Integer
        Get
            Return _ServicesCS
        End Get
        Set(ByVal value As Integer)
            _ServicesCS = value
        End Set
    End Property

    Public Property ServicesIns() As Integer
        Get
            Return _ServicesIns
        End Get
        Set(ByVal value As Integer)
            _ServicesIns = value
        End Set
    End Property

    Public Property ServicesUpd() As Integer
        Get
            Return _ServicesUpd
        End Get
        Set(ByVal value As Integer)
            _ServicesUpd = value
        End Set
    End Property

    Public Property PLItemsCS() As Integer
        Get
            Return _PLItemsCS
        End Get
        Set(ByVal value As Integer)
            _PLItemsCS = value
        End Set
    End Property

    Public Property PLItemsIns() As Integer
        Get
            Return _PLItemsIns
        End Get
        Set(ByVal value As Integer)
            _PLItemsIns = value
        End Set
    End Property

    Public Property PLItemsUpd() As Integer
        Get
            Return _PLItemsUpd
        End Get
        Set(ByVal value As Integer)
            _PLItemsUpd = value
        End Set
    End Property

    Public Property PLItemsDetailsCS() As Integer
        Get
            Return _PLItemsDetailsCS
        End Get
        Set(ByVal value As Integer)
            _PLItemsDetailsCS = value
        End Set
    End Property

    Public Property PLItemsDetailsIns() As Integer
        Get
            Return _PLItemsDetailsIns
        End Get
        Set(ByVal value As Integer)
            _PLItemsDetailsIns = value
        End Set
    End Property

    Public Property PLItemsDetailsUpd() As Integer
        Get
            Return _PLItemsDetailsUpd
        End Get
        Set(ByVal value As Integer)
            _PLItemsDetailsUpd = value
        End Set
    End Property

    Public Property PLServicesCS() As Integer
        Get
            Return _PLServicesCS
        End Get
        Set(ByVal value As Integer)
            _PLServicesCS = value
        End Set
    End Property

    Public Property PLServicesIns() As Integer
        Get
            Return _PLServicesIns
        End Get
        Set(ByVal value As Integer)
            _PLServicesIns = value
        End Set
    End Property

    Public Property PLServicesUpd() As Integer
        Get
            Return _PLServicesUpd
        End Get
        Set(ByVal value As Integer)
            _PLServicesUpd = value
        End Set
    End Property

    Public Property PLServicesDetailsCS() As Integer
        Get
            Return _PLServicesDetailsCS
        End Get
        Set(ByVal value As Integer)
            _PLServicesDetailsCS = value
        End Set
    End Property

    Public Property PLServicesDetailsIns() As Integer
        Get
            Return _PLServicesDetailsIns
        End Get
        Set(ByVal value As Integer)
            _PLServicesDetailsIns = value
        End Set
    End Property

    Public Property PLServicesDetailsUpd() As Integer
        Get
            Return _PLServicesDetailsUpd
        End Get
        Set(ByVal value As Integer)
            _PLServicesDetailsUpd = value
        End Set
    End Property

    Public Property ICDCS() As Integer
        Get
            Return _ICDCS
        End Get
        Set(ByVal value As Integer)
            _ICDCS = value
        End Set
    End Property

    Public Property ICDIns() As Integer
        Get
            Return _ICDIns
        End Get
        Set(ByVal value As Integer)
            _ICDIns = value
        End Set
    End Property

    Public Property ICDUpd() As Integer
        Get
            Return _ICDUpd
        End Get
        Set(ByVal value As Integer)
            _ICDUpd = value
        End Set
    End Property

    Public Property HFCS() As Integer
        Get
            Return _HFCS
        End Get
        Set(ByVal value As Integer)
            _HFCS = value
        End Set
    End Property

    Public Property HFIns() As Integer
        Get
            Return _HFIns
        End Get
        Set(ByVal value As Integer)
            _HFIns = value
        End Set
    End Property

    Public Property HFUpd() As Integer
        Get
            Return _HFUpd
        End Get
        Set(ByVal value As Integer)
            _HFUpd = value
        End Set
    End Property

    Public Property ClaimAdminCS() As Integer
        Get
            Return _ClaimAdminCS
        End Get
        Set(ByVal value As Integer)
            _ClaimAdminCS = value
        End Set
    End Property

    Public Property ClaimAdminIns() As Integer
        Get
            Return _ClaimAdminIns
        End Get
        Set(ByVal value As Integer)
            _ClaimAdminIns = value
        End Set
    End Property

    Public Property ClaimAdminUpd() As Integer
        Get
            Return _ClaimAdminUpd
        End Get
        Set(ByVal value As Integer)
            _ClaimAdminUpd = value
        End Set
    End Property

    Public Property PayerCS() As Integer
        Get
            Return _PayerCS
        End Get
        Set(ByVal value As Integer)
            _PayerCS = value
        End Set
    End Property

    Public Property PayerIns() As Integer
        Get
            Return _PayerIns
        End Get
        Set(ByVal value As Integer)
            _PayerIns = value
        End Set
    End Property

    Public Property PayerUpd() As Integer
        Get
            Return _payerUpd
        End Get
        Set(ByVal value As Integer)
            _payerUpd = value
        End Set
    End Property

    Public Property OfficerCS() As Integer
        Get
            Return _OfficerCS
        End Get
        Set(ByVal value As Integer)
            _OfficerCS = value
        End Set
    End Property

    Public Property OfficerIns() As Integer
        Get
            Return _OfficerIns
        End Get
        Set(ByVal value As Integer)
            _OfficerIns = value
        End Set
    End Property

    Public Property OfficerUpd() As Integer
        Get
            Return _OfficerUpd
        End Get
        Set(ByVal value As Integer)
            _OfficerUpd = value
        End Set
    End Property

    Public Property ProductCS() As Integer
        Get
            Return _ProductCS
        End Get
        Set(ByVal value As Integer)
            _ProductCS = value
        End Set
    End Property

    Public Property ProductIns() As Integer
        Get
            Return _ProductIns
        End Get
        Set(ByVal value As Integer)
            _ProductIns = value
        End Set
    End Property

    Public Property ProductUpd() As Integer
        Get
            Return _ProductUpd
        End Get
        Set(ByVal value As Integer)
            _ProductUpd = value
        End Set
    End Property

    Public Property ProductItemsCS() As Integer
        Get
            Return _ProductItemsCS
        End Get
        Set(ByVal value As Integer)
            _ProductItemsCS = value
        End Set
    End Property

    Public Property ProductItemsIns() As Integer
        Get
            Return _ProductItemsIns
        End Get
        Set(ByVal value As Integer)
            _ProductItemsIns = value
        End Set
    End Property

    Public Property ProductItemsUpd() As Integer
        Get
            Return _ProductItemsUpd
        End Get
        Set(ByVal value As Integer)
            _ProductItemsUpd = value
        End Set
    End Property

    Public Property ProductServicesCS() As Integer
        Get
            Return _ProductServicesCS
        End Get
        Set(ByVal value As Integer)
            _ProductServicesCS = value
        End Set
    End Property

    Public Property ProductServicesIns() As Integer
        Get
            Return _ProductServicesIns
        End Get
        Set(ByVal value As Integer)
            _ProductServicesIns = value
        End Set
    End Property

    Public Property ProductServicesUpd() As Integer
        Get
            Return _ProductServicesUpd
        End Get
        Set(ByVal value As Integer)
            _ProductServicesUpd = value
        End Set
    End Property

    Public Property RelDistrCS() As Integer
        Get
            Return _RelDistrCS
        End Get
        Set(ByVal value As Integer)
            _RelDistrCS = value
        End Set
    End Property

    Public Property RelDistrIns() As Integer
        Get
            Return _RelDistrIns
        End Get
        Set(ByVal value As Integer)
            _RelDistrIns = value
        End Set
    End Property

    Public Property RelDistrUpd() As Integer
        Get
            Return _RelDistrUpd
        End Get
        Set(ByVal value As Integer)
            _RelDistrUpd = value
        End Set
    End Property

    Public Property FamiliesCS() As Integer
        Get
            Return _FamiliesCS
        End Get
        Set(ByVal value As Integer)
            _FamiliesCS = value
        End Set
    End Property

    Public Property FamiliesIns() As Integer
        Get
            Return _FamiliesIns
        End Get
        Set(ByVal value As Integer)
            _FamiliesIns = value
        End Set
    End Property

    Public Property FamiliesUpd() As Integer
        Get
            Return _FamiliesUpd
        End Get
        Set(ByVal value As Integer)
            _FamiliesUpd = value
        End Set
    End Property

    Public Property InsureeCS() As Integer
        Get
            Return _InsureeCS
        End Get
        Set(ByVal value As Integer)
            _InsureeCS = value
        End Set
    End Property

    Public Property InsureeIns() As Integer
        Get
            Return _InsureeIns
        End Get
        Set(ByVal value As Integer)
            _InsureeIns = value
        End Set
    End Property

    Public Property InsureeUpd() As Integer
        Get
            Return _InsureeUpd
        End Get
        Set(ByVal value As Integer)
            _InsureeUpd = value
        End Set
    End Property

    Public Property PhotoCS() As Integer
        Get
            Return _PhotoCS
        End Get
        Set(ByVal value As Integer)
            _PhotoCS = value
        End Set
    End Property

    Public Property PhotoIns() As Integer
        Get
            Return _PhotoIns
        End Get
        Set(ByVal value As Integer)
            _PhotoIns = value
        End Set
    End Property

    Public Property PhotoUpd() As Integer
        Get
            Return _PhotoUpd
        End Get
        Set(ByVal value As Integer)
            _PhotoUpd = value
        End Set
    End Property

    Public Property PolicyCS() As Integer
        Get
            Return _PolicyCS
        End Get
        Set(ByVal value As Integer)
            _PolicyCS = value
        End Set
    End Property

    Public Property PolicyIns() As Integer
        Get
            Return _PolicyIns
        End Get
        Set(ByVal value As Integer)
            _PolicyIns = value
        End Set
    End Property

    Public Property PolicyUpd() As Integer
        Get
            Return _PolicyUpd
        End Get
        Set(ByVal value As Integer)
            _PolicyUpd = value
        End Set
    End Property

    Public Property PremiumCS() As Integer
        Get
            Return _PremiumCS
        End Get
        Set(ByVal value As Integer)
            _PremiumCS = value
        End Set
    End Property

    Public Property PremiumIns() As Integer
        Get
            Return _PremiumIns
        End Get
        Set(ByVal value As Integer)
            _PremiumIns = value
        End Set
    End Property

    Public Property PremiumUpd() As Integer
        Get
            Return _PremiumUpd
        End Get
        Set(ByVal value As Integer)
            _PremiumUpd = value
        End Set
    End Property


End Class
