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

Public Class ProductsDAL
    'Correct by Rogers & Hiren

    Public Sub LoadProduct(ByRef eProducts As IMIS_EN.tblProduct)
        Dim data As New ExactSQL
        Dim dr As DataRow
        Dim sSQL As String = ""
        '  data.setSQLCommand("SELECT * FROM tblProduct WHERE prodID=@ProdId", CommandType.Text)
        sSQL += " SELECT R.LocationId RegionId, CASE  WHEN L.ParentLocationId IS NULL THEN NULL ELSE L.LocationId END DistrictId, P.*"
        sSQL += " FROM tblProduct P"
        sSQL += " LEFT OUTER JOIN tblLocations L ON P.LocationId = L.LocationId"
        sSQL += " LEFT OUTER JOIN tblLocations R ON R.LocationId = ISNULL(L.parentLocationId , L.LocationId)"
        sSQL += " WHERE P.ProdId = @ProdId"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProdID", SqlDbType.Int, eProducts.ProdID)
        dr = data.Filldata()(0)
        If Not dr Is Nothing Then
            Dim eLocations As New IMIS_EN.tblLocations
            eLocations.LocationId = If(dr.IsNull("LocationId"), -1, dr("LocationId"))
            eProducts.tblLocation = eLocations
            eProducts.ProdID = dr("ProdID")
            If Not dr("ProductCode") Is DBNull.Value Then
                eProducts.ProductCode = dr("ProductCode")
            End If

            If Not dr("ProductName") Is DBNull.Value Then
                eProducts.ProductName = dr("ProductName")
            End If
            If Not dr("Recurrence") Is DBNull.Value Then
                eProducts.Recurrence = CInt(dr("Recurrence"))
            End If
            If Not dr("DateFrom") Is DBNull.Value Then
                eProducts.DateFrom = dr("DateFrom")
            End If

            If Not dr("DateTo") Is DBNull.Value Then
                eProducts.DateTo = dr("DateTo")
            End If

            Dim eproduct2 As New IMIS_EN.tblProduct
            eproduct2.ProdID = If(dr("ConversionProdID") Is System.DBNull.Value, 0, dr("ConversionProdID"))
            eProducts.tblProduct2 = eproduct2

            If Not dr("LumpSum") Is DBNull.Value Then
                eProducts.LumpSum = dr("LumpSum")
            End If

            If Not dr("MemberCount") Is DBNull.Value Then
                eProducts.MemberCount = dr("MemberCount")
            End If

            If Not dr("PremiumAdult") Is DBNull.Value Then
                eProducts.PremiumAdult = dr("PremiumAdult")
            End If
            If Not dr("PremiumChild") Is DBNull.Value Then
                eProducts.PremiumChild = dr("PremiumChild")
            End If
            If Not dr("DedInsuree") Is DBNull.Value Then
                eProducts.DedInsuree = dr("DedInsuree")
            End If
            If Not dr("DedOPInsuree") Is DBNull.Value Then
                eProducts.DedOPInsuree = dr("DedOPInsuree")
            End If
            If Not dr("DedIPInsuree") Is DBNull.Value Then
                eProducts.DedIPInsuree = dr("DedIPInsuree")
            End If
            If Not dr("MaxInsuree") Is DBNull.Value Then
                eProducts.MaxInsuree = dr("MaxInsuree")
            End If
            If Not dr("MaxOPInsuree") Is DBNull.Value Then
                eProducts.MaxOPInsuree = dr("MaxOPInsuree")
            End If
            If Not dr("MaxIPInsuree") Is DBNull.Value Then
                eProducts.MaxIPInsuree = dr("MaxIPInsuree")
            End If
            If Not dr("PeriodRelPrices") Is DBNull.Value Then
                eProducts.PeriodRelPrices = dr("PeriodRelPrices")
            End If
            If Not dr("PeriodRelPricesIP") Is DBNull.Value Then
                eProducts.PeriodRelPricesIP = dr("PeriodRelPricesIP")
            End If
            If Not dr("PeriodRelPricesOP") Is DBNull.Value Then
                eProducts.PeriodRelPricesOP = dr("PeriodRelPricesOP")
            End If

            'If Not dr("DistrRelID") Is DBNull.Value Then
            '    eProducts.DistrRelID = dr("DistrRelID")
            'End If

            'If Not dr("DistrRelIPID") Is DBNull.Value Then
            '    eProducts.DistrRelIPID = dr("DistrRelIPID")
            'End If

            'If Not dr("DistrRelOPID") Is DBNull.Value Then
            '    eProducts.DistrRelOPID = dr("DistrRelOPID")
            'End If

            If Not dr("AccCodePremiums") Is DBNull.Value Then
                eProducts.AccCodePremiums = dr("AccCodePremiums")
            End If

            If Not dr("AccCodeRemuneration") Is DBNull.Value Then
                eProducts.AccCodeRemuneration = dr("AccCodeRemuneration")
            End If
            'Addition for Bepha start:
            If Not dr("GracePeriodRenewal") Is DBNull.Value Then
                eProducts.GracePeriodRenewal = dr("GracePeriodRenewal")
            End If

            If Not dr("MaxInstallments") Is DBNull.Value Then
                eProducts.MaxInstallments = dr("MaxInstallments")
            End If
            If Not dr("WaitingPeriod") Is DBNull.Value Then
                eProducts.WaitingPeriod = dr("WaitingPeriod")
            End If

            If Not dr("RegistrationLumpSum") Is DBNull.Value Then
                eProducts.RegistrationLumpSum = dr("RegistrationLumpSum")
            End If
            If Not dr("RegistrationFee") Is DBNull.Value Then
                eProducts.RegistrationFee = dr("RegistrationFee")
            End If
            If Not dr("GeneralAssemblyLumpSum") Is DBNull.Value Then
                eProducts.GeneralAssemblyLumpSum = dr("GeneralAssemblyLumpSum")
            End If
            If Not dr("GeneralAssemblyFee") Is DBNull.Value Then
                eProducts.GeneralAssemblyFee = dr("GeneralAssemblyFee")
            End If
            If Not dr("StartCycle1") Is DBNull.Value Then
                eProducts.StartCycle1 = dr("StartCycle1")
            End If
            If Not dr("StartCycle2") Is DBNull.Value Then
                eProducts.StartCycle2 = dr("StartCycle2")
            End If
            'Addition for Bepha end:
            If Not dr("DedTreatment") Is DBNull.Value Then
                eProducts.DedTreatment = dr("DedTreatment")
            End If
            If Not dr("DedIPTreatment") Is DBNull.Value Then
                eProducts.DedIPTreatment = dr("DedIPTreatment")
            End If
            If Not dr("DedOPTreatment") Is DBNull.Value Then
                eProducts.DedOPTreatment = dr("DedOPTreatment")
            End If
            If Not dr("MaxTreatment") Is DBNull.Value Then
                eProducts.MaxTreatment = dr("MaxTreatment")
            End If
            If Not dr("MaxOPTreatment") Is DBNull.Value Then
                eProducts.MaxOPTreatment = dr("MaxOPTreatment")
            End If
            If Not dr("MaxIPTreatment") Is DBNull.Value Then
                eProducts.MaxIPTreatment = dr("MaxIPTreatment")
            End If
            If Not dr("DedPolicy") Is DBNull.Value Then
                eProducts.DedPolicy = dr("DedPolicy")
            End If
            If Not dr("DedOPPolicy") Is DBNull.Value Then
                eProducts.DedOPPolicy = dr("DedOPPolicy")
            End If
            If Not dr("DedIPPolicy") Is DBNull.Value Then
                eProducts.DedIPPolicy = dr("DedIPPolicy")
            End If
            If Not dr("MaxOPPolicy") Is DBNull.Value Then
                eProducts.MaxOPPolicy = dr("MaxOPPolicy")
            End If
            If Not dr("MaxIPPolicy") Is DBNull.Value Then
                eProducts.MaxIPPolicy = dr("MaxIPPolicy")
            End If
            If Not dr("MaxPolicy") Is DBNull.Value Then
                eProducts.MaxPolicy = dr("MaxPolicy")
            End If
            If Not dr("GracePeriod") Is DBNull.Value Then
                eProducts.GracePeriod = dr("GracePeriod")
            End If
            If Not dr("InsurancePeriod") Is DBNull.Value Then
                eProducts.InsurancePeriod = dr("InsurancePeriod")
            End If
            'Addition for Bepha start:
            If Not dr("MaxNoConsultation") Is DBNull.Value Then
                eProducts.MaxNoConsultation = dr("MaxNoConsultation")
            End If
            If Not dr("MaxAmountConsultation") Is DBNull.Value Then
                eProducts.MaxAmountConsultation = dr("MaxAmountConsultation")
            End If
            If Not dr("MaxNoSurgery") Is DBNull.Value Then
                eProducts.MaxNoSurgery = dr("MaxNoSurgery")
            End If
            If Not dr("MaxAmountSurgery") Is DBNull.Value Then
                eProducts.MaxAmountSurgery = dr("MaxAmountSurgery")
            End If
            If Not dr("MaxNoDelivery") Is DBNull.Value Then
                eProducts.MaxNoDelivery = dr("MaxNoDelivery")
            End If
            If Not dr("MaxAmountDelivery") Is DBNull.Value Then
                eProducts.MaxAmountDelivery = dr("MaxAmountDelivery")
            End If
            If Not dr("MaxNoHospitalizaion") Is DBNull.Value Then
                eProducts.MaxNoHospitalizaion = dr("MaxNoHospitalizaion")
            End If
            If Not dr("MaxAmountHospitalization") Is DBNull.Value Then
                eProducts.MaxAmountHospitalization = dr("MaxAmountHospitalization")
            End If
            If Not dr("MaxNoVisits") Is DBNull.Value Then
                eProducts.MaxNoVisits = dr("MaxNoVisits")
            End If
            If Not dr("MaxAmountAntenatal") Is DBNull.Value Then
                eProducts.MaxAmountAntenatal = dr("MaxAmountAntenatal")
            End If
            If Not dr("MaxNoAntenatal") Is DBNull.Value Then
                eProducts.MaxNoAntenatal = dr("MaxNoAntenatal")
            End If

            If Not dr("RegionId") Is DBNull.Value Then
                eProducts.RegionId = dr("RegionId")
            Else
                eProducts.RegionId = -1
            End If
            If Not dr("DistrictId") Is DBNull.Value Then
                eProducts.DistrictId = dr("DistrictId")
            End If

            'Addition for Bepha end:
            'Addition for Nepal >> Start
            If dr("MaxPolicyExtraMember") IsNot DBNull.Value Then eProducts.MaxPolicyExtraMember = dr("MaxPolicyExtraMember")
            If dr("MaxPolicyExtraMemberIP") IsNot DBNull.Value Then eProducts.MaxPolicyExtraMemberIP = dr("MaxPolicyExtraMemberIP")
            If dr("MaxPolicyExtraMemberOP") IsNot DBNull.Value Then eProducts.MaxPolicyExtraMemberOP = dr("MaxPolicyExtraMemberOP")

            If dr("MaxCeilingPolicy") IsNot DBNull.Value Then eProducts.MaxCeilingPolicy = dr("MaxCeilingPolicy")
            If dr("MaxCeilingPolicyIP") IsNot DBNull.Value Then eProducts.MaxCeilingPolicyIP = dr("MaxCeilingPolicyIP")
            If dr("MaxCeilingPolicyOP") IsNot DBNull.Value Then eProducts.MaxCeilingPolicyOP = dr("MaxCeilingPolicyOP")

            If dr("Threshold") IsNot DBNull.Value Then eProducts.Threshold = dr("Threshold")
            If dr("RenewalDiscountPerc") IsNot DBNull.Value Then eProducts.RenewalDiscountPerc = dr("RenewalDiscountPerc")
            If dr("RenewalDiscountPeriod") IsNot DBNull.Value Then eProducts.RenewalDiscountPeriod = dr("RenewalDiscountPeriod")
            If dr("StartCycle3") IsNot DBNull.Value Then eProducts.StartCycle3 = dr("StartCycle3")
            If dr("StartCycle4") IsNot DBNull.Value Then eProducts.StartCycle4 = dr("StartCycle4")
            '   If dr("AdministrationPeriod") IsNot DBNull.Value Then eProducts.AdministrationPeriod = dr("AdministrationPeriod")
            eProducts.AdministrationPeriod = If(dr("AdministrationPeriod") IsNot DBNull.Value, dr("AdministrationPeriod"), 0)

            If dr("EnrolmentDiscountPerc") IsNot DBNull.Value Then eProducts.EnrolmentDiscountPerc = dr("EnrolmentDiscountPerc")
            If dr("EnrolmentDiscountPeriod") IsNot DBNull.Value Then eProducts.EnrolmentDiscountPeriod = dr("EnrolmentDiscountPeriod")
            'Addition for Nepal >> End
            eProducts.Level1 = dr("Level1").ToString
            eProducts.Sublevel1 = dr("Sublevel1").ToString
            eProducts.Level2 = dr("Level2").ToString
            eProducts.Sublevel2 = dr("Sublevel2").ToString
            eProducts.Level3 = dr("Level3").ToString
            eProducts.Sublevel3 = dr("Sublevel3").ToString
            eProducts.Level4 = dr("Level4").ToString
            eProducts.Sublevel4 = dr("Sublevel4").ToString


            If dr("ShareContribution") IsNot DBNull.Value Then eProducts.ShareContribution = dr("ShareContribution")
            If dr("WeightPopulation") IsNot DBNull.Value Then eProducts.WeightPopulation = dr("WeightPopulation")
            If dr("WeightNumberFamilies") IsNot DBNull.Value Then eProducts.WeightNumberFamilies = dr("WeightNumberFamilies")
            If dr("WeightInsuredPopulation") IsNot DBNull.Value Then eProducts.WeightInsuredPopulation = dr("WeightInsuredPopulation")
            If dr("WeightNumberInsuredFamilies") IsNot DBNull.Value Then eProducts.WeightNumberInsuredFamilies = dr("WeightNumberInsuredFamilies")
            If dr("WeightNumberVisits") IsNot DBNull.Value Then eProducts.WeightNumberVisits = dr("WeightNumberVisits")
            If dr("WeightAdjustedAmount") IsNot DBNull.Value Then eProducts.WeightAdjustedAmount = dr("WeightAdjustedAmount")


            If dr("CeilingInterpretation") IsNot DBNull.Value Then eProducts.CeilingInterpretation = dr("CeilingInterpretation").ToString

            eProducts.ValidityTo = If(dr("ValidityTo").ToString = String.Empty, Nothing, dr("ValidityTo"))

        End If
    End Sub



    'Corrected + Rogers
    Public Function GetProducts(ByVal UserId As Integer, Optional ByVal RegionId As Integer = 0, Optional ByVal DistrictId As Integer = 0, Optional ByVal ByDate As Date? = Nothing) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""


        sSQL = " SELECT Prod.ProdId, Prod.ProductCode , Prod.LocationId"
        sSQL += " FROM tblProduct Prod"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(Prod.LocationId, 0)"
        sSQL += " LEFT OUTER JOIN tblUsersDistricts UD ON Prod.LocationId = UD.LocationId AND UD.UserId = @UserId AND UD.ValidityTo IS NULL"
        sSQL += " LEFT JOIN (SELECT   ProductCode, MIN(ValidityFrom) ValidityFrom from tblProduct WHERE LegacyID IS NOT NULL GROUP BY ProductCode) HPROD ON HPROD.ProductCode=PROD.ProductCode"
        sSQL += " WHERE Prod.ValidityTo IS NULL "
        sSQL += " AND (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId = 0)"
        sSQL += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL)"
        sSQL += " AND (@EnrollDate BETWEEN Prod.DateFrom AND Prod.DateTo OR @EnrollDate IS NULL)"
        sSQL += " AND (@EnrollDate BETWEEN  ISNULL(CONVERT(DATE,HPROD.ValidityFrom,103) ,CONVERT(DATE,prod.ValidityFrom,103)) AND Prod.DateTo OR @EnrollDate IS NULL)"
        sSQL += " ORDER BY L.ParentLocationId"

        data.setSQLCommand(sSQL, CommandType.Text)
        'If Not DistrictId = 0 Then
        data.params("@RegionId", SqlDbType.Int, If(RegionId = -1, DBNull.Value, RegionId))
        data.params("@DistrictId", SqlDbType.Int, DistrictId)
        'End If
        data.params("@UserID", SqlDbType.Int, UserId)
        data.params("@EnrollDate", SqlDbType.Date, ByDate)
        Return data.Filldata
    End Function

    'Corrected By Rogers & Hiren
    Public Function GetProducts(ByVal eProducts As IMIS_EN.tblProduct, ByVal All As Boolean) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""

        sSQL = "SELECT  Prod.ProdId, Prod.ProdUUID, Prod.ProductCode , Prod.ProductName , L.RegionName , L.DistrictName, Prod.DateFrom, Prod.DateTo, Prod.MemberCount, Prod.LumpSum, Prod.PremiumAdult, Prod.PremiumChild, Prod.InsurancePeriod, Prod.GracePeriod, Prod.ValidityFrom, Prod.ValidityTo"
        sSQL += " FROM tblProduct Prod"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(Prod.LocationId, 0)"

        sSQL += " INNER JOIN ("
        sSQL += " SELECT L.DistrictId, L.RegionId"
        sSQL += " FROM tblUsersDistricts UD"
        sSQL += " INNER JOIN uvwLocations L ON L.DistrictId = UD.LocationId"
        sSQL += " WHERE UD.ValidityTo IS NULL"
        sSQL += " AND (UD.UserId = @UserId OR @UserId = 0)"
        sSQL += " GROUP BY L.DistrictId, L.RegionId"
        sSQL += " )UD ON UD.DistrictId = Prod.LocationId  OR UD.RegionId = Prod.LocationId OR Prod.LocationId IS NULL"

        sSQL += " WHERE (L.Regionid = @RegionId OR @RegionId = 0 OR L.LocationId = 0)"
        sSQL += " AND (L.DistrictId = @DistrictId OR @DistrictId = 0 OR L.DistrictId IS NULL)"
        sSQL += " AND Prod.ProductCode LIKE @ProductCode"
        sSQL += " AND ProductName LIKE @ProductName"
        sSQL += " AND DateFrom >= @DateFrom"
        sSQL += " AND DateTo <= @DateTo"


        If Not All Then
            sSQL += " AND Prod.ValidityTo IS NULL"
        End If


        sSQL += " GROUP BY Prod.ProdId, Prod.ProductCode , Prod.ProductName , L.RegionName , L.DistrictName, Prod.DateFrom, Prod.DateTo, Prod.MemberCount, Prod.LumpSum, Prod.PremiumAdult, Prod.PremiumChild, Prod.InsurancePeriod, Prod.GracePeriod, Prod.ValidityFrom, Prod.ValidityTo, Prod.ProdUUID "
        sSQL += " ORDER BY ProdID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@UserId", SqlDbType.Int, eProducts.AuditUserID)
        data.params("@ProductCode", SqlDbType.NVarChar, 50, eProducts.ProductCode)
        data.params("@ProductName", SqlDbType.NVarChar, 100, eProducts.ProductName)
        data.params("@LocationId", SqlDbType.Int, eProducts.tblLocation.LocationId)
        data.params("@RegionId", SqlDbType.Int, eProducts.tblLocation.RegionId)
        data.params("@DistrictId", SqlDbType.Int, eProducts.tblLocation.DistrictID)
        data.params("@DateFrom", SqlDbType.Date, eProducts.DateFrom)
        data.params("@DateTo", SqlDbType.Date, eProducts.DateTo)
        Return data.Filldata
    End Function


    Public Function CheckIfProductExists(ByVal eProducts As IMIS_EN.tblProduct) As DataTable
        Dim data As New ExactSQL
        Dim strSQL As String = "Select Top 1 * from tblProduct where ProductCode = @ProductCode AND ValidityTo is null"

        If Not eProducts.ProdID = 0 Then
            strSQL += " AND ProdID <> @ProdID"
        End If
        data.setSQLCommand(strSQL, CommandType.Text)

        data.params("@ProdID", SqlDbType.Int, eProducts.ProdID)
        data.params("@ProductCode", SqlDbType.NVarChar, 8, eProducts.ProductCode)
        Return data.Filldata()
    End Function
    Public Sub InsertProduct(ByRef eProducts As IMIS_EN.tblProduct)
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL += " INSERT INTO tblProduct([ProductCode],[ProductName],[ConversionProdID],[LocationId],[DateFrom],[DateTo],[LumpSum],[MemberCount],[PremiumAdult],[PremiumChild],"
        sSQL += " [InsurancePeriod],[DedInsuree],[DedOPInsuree],[DedIPInsuree],[MaxInsuree],[MaxOPInsuree],[MaxIPInsuree],[PeriodRelPrices],[PeriodRelPricesOP],"
        sSQL += " [PeriodRelPricesIP],[AccCodePremiums],[AccCodeRemuneration],[DedTreatment],[DedOPTreatment],[DedIPTreatment],[MaxTreatment],[MaxOPTreatment],"
        sSQL += " [MaxIPTreatment],[DedPolicy],[DedOPPolicy],[DedIPPolicy],[MaxPolicy],[MaxOPPolicy],[MaxIPPolicy],[GracePeriod],[AuditUserID],GracePeriodRenewal,"
        sSQL += " RegistrationLumpSum,RegistrationFee,GeneralAssemblyLumpSum,GeneralAssemblyFee,StartCycle1,StartCycle2,MaxNoConsultation,MaxNoSurgery,MaxNoDelivery,"
        sSQL += " MaxNoHospitalizaion,MaxNoVisits,MaxAmountConsultation,MaxAmountSurgery,MaxAmountDelivery,MaxAmountHospitalization,MaxInstallments,WaitingPeriod,"
        sSQL += " Threshold,RenewalDiscountPerc,RenewalDiscountPeriod,StartCycle3,StartCycle4,AdministrationPeriod,MaxPolicyExtraMember,MaxPolicyExtraMemberIP,"
        sSQL += " MaxPolicyExtraMemberOP,MaxCeilingPolicy,MaxCeilingPolicyIP,MaxCeilingPolicyOP,EnrolmentDiscountPerc,EnrolmentDiscountPeriod, MaxAmountAntenatal,"
        sSQL += " MaxNoAntenatal, CeilingInterpretation,[Level1],[Sublevel1],[Level2],[Sublevel2],[Level3],[Sublevel3],[Level4],[Sublevel4],[ShareContribution],[WeightPopulation],WeightNumberFamilies,[WeightInsuredPopulation],[WeightNumberInsuredFamilies],[WeightNumberVisits],[WeightAdjustedAmount],[Recurrence])"
        sSQL += " VALUES(@ProductCode, @ProductName,@ConversionProdID,@LocationId,@DateFrom,@DateTo,@LumpSum,@MemberCount,@PremiumAdult,@PremiumChild,@InsurancePeriod,"
        sSQL += " @DedInsuree,@DedOPInsuree,@DedIPInsuree,@MaxInsuree,@MaxOPInsuree,@MaxIPInsuree,@PeriodRelPrices,@PeriodRelPricesOP,@PeriodRelPricesIP,@AccCodePremiums,"
        sSQL += " @AccCodeRemuneration,@DedTreatment,@DedOPTreatment,@DedIPTreatment,@MaxTreatment,@MaxOPTreatment,@MaxIPTreatment,@DedPolicy,@DedOPPolicy,"
        sSQL += " @DedIPPolicy,@MaxPolicy,@MaxOPPolicy,@MaxIPPolicy,@GracePeriod,@AuditUserID,@GracePeriodRenewal,@RegistrationLumpSum,@RegistrationFee,"
        sSQL += " @GeneralAssemblyLumpSum,@GeneralAssemblyFee,@StartCycle1,@StartCycle2,@MaxNoConsultation,@MaxNoSurgery,@MaxNoDelivery,@MaxNoHospitalizaion,"
        sSQL += " @MaxNoVisits,@MaxAmountConsultation,@MaxAmountSurgery,@MaxAmountDelivery,@MaxAmountHospitalization,@MaxInstallments,@WaitingPeriod,@Threshold,"
        sSQL += " @RenewalDiscountPerc,@RenewalDiscountPeriod,@StartCycle3,@StartCycle4,@AdministrationPeriod,@MaxPolicyExtraMember,@MaxPolicyExtraMemberIP,"
        sSQL += " @MaxPolicyExtraMemberOP,@MaxCeilingPolicy,@MaxCeilingPolicyIP,@MaxCeilingPolicyOP,@EnrolmentDiscountPerc,@EnrolmentDiscountPeriod, @MaxAmountAntenatal,"
        sSQL += " @MaxNoAntenatal, @CeilingInterpretation, @Level1,@Sublevel1,@Level2,@Sublevel2,@Level3,@Sublevel3,@Level4,@Sublevel4,@ShareContribution,@WeightPopulation,@WeightNumberFamilies,@WeightInsuredPopulation,@WeightNumberInsuredFamilies,@WeightNumberVisits,@WeightAdjustedAmount,@Recurrence);"
        sSQL += " SELECT @ProdId = scope_identity()"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProdID", SqlDbType.Int, 0, ParameterDirection.Output)
        data.params("@ProductCode", SqlDbType.NVarChar, 8, eProducts.ProductCode)
        data.params("@ProductName", SqlDbType.NVarChar, 100, eProducts.ProductName)
        data.params("@LocationId", SqlDbType.Int, If(eProducts.tblLocation.LocationId = -1, DBNull.Value, eProducts.tblLocation.LocationId)) 'Nothing to DBNull.Value was Changed by Salumu 10-12-2018
        data.params("@DateFrom", SqlDbType.DateTime, eProducts.DateFrom)
        data.params("@DateTo", SqlDbType.SmallDateTime, eProducts.DateTo)
        data.params("@ConversionProdID", SqlDbType.Int, if(eProducts.tblProduct2.ProdID = 0, DBNull.Value, eProducts.tblProduct2.ProdID))
        data.params("@LumpSum", SqlDbType.Decimal, if(eProducts.LumpSum = Nothing, 0, eProducts.LumpSum))
        data.params("@MemberCount", SqlDbType.SmallInt, eProducts.MemberCount)
        data.params("@PremiumAdult", SqlDbType.Decimal, eProducts.PremiumAdult)
        data.params("@PremiumChild", SqlDbType.Decimal, eProducts.PremiumChild)
        data.params("@InsurancePeriod", SqlDbType.TinyInt, eProducts.InsurancePeriod)
        data.params("@DedInsuree", SqlDbType.Decimal, eProducts.DedInsuree)
        data.params("@DedOPInsuree", SqlDbType.Decimal, eProducts.DedOPInsuree)
        data.params("@DedIPInsuree", SqlDbType.Decimal, eProducts.DedIPInsuree)
        data.params("@MaxInsuree", SqlDbType.Decimal, eProducts.MaxInsuree)
        data.params("@MaxOPInsuree", SqlDbType.Decimal, eProducts.MaxOPInsuree)
        data.params("@MaxIPInsuree", SqlDbType.Decimal, eProducts.MaxIPInsuree)
        data.params("@PeriodRelPrices", SqlDbType.Char, 1, eProducts.PeriodRelPrices)
        data.params("@PeriodRelPricesOP", SqlDbType.Char, 1, eProducts.PeriodRelPricesOP)
        data.params("@PeriodRelPricesIP", SqlDbType.Char, 1, eProducts.PeriodRelPricesIP)
        data.params("@AccCodePremiums", SqlDbType.NVarChar, 25, eProducts.AccCodePremiums)
        data.params("@AccCodeRemuneration", SqlDbType.NVarChar, 25, eProducts.AccCodeRemuneration)
        data.params("@DedTreatment", SqlDbType.Decimal, eProducts.DedTreatment)
        data.params("@DedOPTreatment", SqlDbType.Decimal, eProducts.DedOPTreatment)
        data.params("@DedIPTreatment", SqlDbType.Decimal, eProducts.DedIPTreatment)
        data.params("@MaxTreatment", SqlDbType.Decimal, eProducts.MaxTreatment)
        data.params("@MaxOPTreatment", SqlDbType.Decimal, eProducts.MaxOPTreatment)
        data.params("@MaxIPTreatment", SqlDbType.Decimal, eProducts.MaxIPTreatment)
        data.params("@DedPolicy", SqlDbType.Decimal, eProducts.DedPolicy)
        data.params("@DedOPPolicy", SqlDbType.Decimal, eProducts.DedOPPolicy)
        data.params("@DedIPPolicy", SqlDbType.Decimal, eProducts.DedIPPolicy)
        data.params("@MaxPolicy", SqlDbType.Decimal, eProducts.MaxPolicy)
        data.params("@MaxOPPolicy", SqlDbType.Decimal, eProducts.MaxOPPolicy)
        data.params("@MaxIPPolicy", SqlDbType.Decimal, eProducts.MaxIPPolicy)
        data.params("@GracePeriod", SqlDbType.Int, eProducts.GracePeriod)
        data.params("@AuditUserID", SqlDbType.Int, eProducts.AuditUserID)
        'Addition for Bepha start:
        data.params("@GracePeriodRenewal", SqlDbType.Int, eProducts.GracePeriodRenewal)
        data.params("@RegistrationLumpSum", SqlDbType.Decimal, eProducts.RegistrationLumpSum)
        data.params("@RegistrationFee", SqlDbType.Decimal, eProducts.RegistrationFee)
        data.params("@GeneralAssemblyLumpSum", SqlDbType.Decimal, eProducts.GeneralAssemblyLumpSum)
        data.params("@GeneralAssemblyFee", SqlDbType.Decimal, eProducts.GeneralAssemblyFee)
        data.params("@StartCycle1", SqlDbType.NVarChar, 5, eProducts.StartCycle1)
        data.params("@StartCycle2", SqlDbType.NVarChar, 5, eProducts.StartCycle2)
        data.params("@MaxNoConsultation", SqlDbType.Int, eProducts.MaxNoConsultation)
        data.params("@MaxAmountConsultation", SqlDbType.Decimal, eProducts.MaxAmountConsultation)
        data.params("@MaxNoSurgery", SqlDbType.Int, eProducts.MaxNoSurgery)
        data.params("@MaxAmountSurgery", SqlDbType.Decimal, eProducts.MaxAmountSurgery)
        data.params("@MaxNoDelivery", SqlDbType.Int, eProducts.MaxNoDelivery)
        data.params("@MaxAmountDelivery", SqlDbType.Decimal, eProducts.MaxAmountDelivery)
        data.params("@MaxNoHospitalizaion", SqlDbType.Int, eProducts.MaxNoHospitalizaion)
        data.params("@MaxAmountHospitalization", SqlDbType.Decimal, eProducts.MaxAmountHospitalization)
        data.params("@MaxNoVisits", SqlDbType.Int, eProducts.MaxNoVisits)
        data.params("@MaxInstallments", SqlDbType.Int, eProducts.MaxInstallments)
        data.params("@WaitingPeriod", SqlDbType.Int, eProducts.WaitingPeriod)
        data.params("@MaxAmountAntenatal", SqlDbType.Decimal, eProducts.MaxAmountAntenatal)
        data.params("@MaxNoAntenatal", SqlDbType.Int, eProducts.MaxNoAntenatal)
        'Addition for Bepha end;
        'Addition for Nepal >> Start
        data.params("@Threshold", SqlDbType.Int, eProducts.Threshold)
        data.params("@RenewalDiscountPerc", SqlDbType.Int, eProducts.RenewalDiscountPerc)
        data.params("@RenewalDiscountPeriod", SqlDbType.Int, eProducts.RenewalDiscountPeriod)
        data.params("@StartCycle3", SqlDbType.NVarChar, 5, eProducts.StartCycle3)
        data.params("@StartCycle4", SqlDbType.NVarChar, 5, eProducts.StartCycle4)
        data.params("@AdministrationPeriod", SqlDbType.Int, eProducts.AdministrationPeriod)
        data.params("@MaxPolicyExtraMember", SqlDbType.Decimal, eProducts.MaxPolicyExtraMember)
        data.params("@MaxPolicyExtraMemberIP", SqlDbType.Decimal, eProducts.MaxPolicyExtraMemberIP)
        data.params("@MaxPolicyExtraMemberOP", SqlDbType.Decimal, eProducts.MaxPolicyExtraMemberOP)
        data.params("@MaxCeilingPolicy", SqlDbType.Decimal, eProducts.MaxCeilingPolicy)
        data.params("@MaxCeilingPolicyIP", SqlDbType.Decimal, eProducts.MaxCeilingPolicyIP)
        data.params("@MaxCeilingPolicyOP", SqlDbType.Decimal, eProducts.MaxCeilingPolicyOP)
        data.params("@EnrolmentDiscountPerc", SqlDbType.Int, eProducts.EnrolmentDiscountPerc)
        data.params("@EnrolmentDiscountPeriod", SqlDbType.Int, eProducts.EnrolmentDiscountPeriod)
        data.params("@CeilingInterpretation", SqlDbType.Char, 1, eProducts.CeilingInterpretation)

        'Addition Capitation payment
        data.params("@Level1 ", SqlDbType.Char, 1, eProducts.Level1)
        data.params("@Sublevel1 ", SqlDbType.Char, 1, eProducts.Sublevel1)
        data.params("@Level2 ", SqlDbType.Char, 1, eProducts.Level2)
        data.params("@Sublevel2 ", SqlDbType.Char, 1, eProducts.Sublevel2)
        data.params("@Level3 ", SqlDbType.Char, 1, eProducts.Level3)
        data.params("@Sublevel3 ", SqlDbType.Char, 1, eProducts.Sublevel3)
        data.params("@Level4 ", SqlDbType.Char, 1, eProducts.Level4)
        data.params("@Sublevel4 ", SqlDbType.Char, 1, eProducts.Sublevel4)
        data.params("@ShareContribution ", SqlDbType.Float, eProducts.ShareContribution)
        data.params("@WeightPopulation ", SqlDbType.Float, eProducts.WeightPopulation)
        data.params("@WeightNumberFamilies ", SqlDbType.Float, eProducts.WeightNumberFamilies)
        data.params("@WeightInsuredPopulation ", SqlDbType.Float, eProducts.WeightInsuredPopulation)
        data.params("@WeightNumberInsuredFamilies", SqlDbType.Float, eProducts.WeightNumberInsuredFamilies)
        data.params("@WeightNumberVisits", SqlDbType.Float, eProducts.WeightNumberVisits)
        data.params("@WeightAdjustedAmount", SqlDbType.Float, eProducts.WeightAdjustedAmount)
        data.params("@Recurrence", SqlDbType.TinyInt, eProducts.Recurrence)
        'Addition for Nepal >> End
        data.ExecuteCommand()
        eProducts.ProdID = data.sqlParameters("@prodID")
    End Sub
    Public Sub UpdateProduct(ByRef eProducts As IMIS_EN.tblProduct)
        Dim data As New ExactSQL
        Dim Query As String = "Insert into tblProduct([ProductCode],[ProductName],[LocationId],[DateFrom],[DateTo]" &
            ",[LumpSum],[MemberCount],[PremiumAdult],[PremiumChild],[InsurancePeriod],[DedInsuree],[DedOPInsuree]" &
            ",[DedIPInsuree],[MaxInsuree],[MaxOPInsuree],[MaxIPInsuree],[PeriodRelPrices],[PeriodRelPricesOP]" &
            ",[PeriodRelPricesIP],[AccCodePremiums],[AccCodeRemuneration],[DedTreatment],[DedOPTreatment]" &
            ",[DedIPTreatment],[MaxTreatment],[MaxOPTreatment],[MaxIPTreatment],[DedPolicy],[DedOPPolicy]" &
            ",[DedIPPolicy],[MaxPolicy],[MaxOPPolicy],[MaxIPPolicy],[GracePeriod],[ValidityFrom],[ValidityTo]" &
            ",[LegacyID],[AuditUserID]" &
              ",GracePeriodRenewal,RegistrationLumpSum,RegistrationFee,GeneralAssemblyLumpSum,GeneralAssemblyFee,StartCycle1" &
              ",StartCycle2,MaxNoConsultation,MaxNoSurgery,MaxNoDelivery,MaxNoHospitalizaion" &
              ",MaxNoVisits,MaxAmountConsultation,MaxAmountSurgery,MaxAmountDelivery,MaxAmountHospitalization,MaxInstallments,WaitingPeriod" &
              ",Threshold,RenewalDiscountPerc,RenewalDiscountPeriod,StartCycle3,StartCycle4,AdministrationPeriod" &
              ",MaxPolicyExtraMember,MaxPolicyExtraMemberIP,MaxPolicyExtraMemberOP" &
              ",MaxCeilingPolicy,MaxCeilingPolicyIP,MaxCeilingPolicyOP" &
              ",EnrolmentDiscountPerc,EnrolmentDiscountPeriod, MaxAmountAntenatal, MaxNoAntenatal, CeilingInterpretation,[Level1],[Sublevel1],[Level2],[Sublevel2],[Level3],[Sublevel3],[Level4],[Sublevel4],[ShareContribution],[WeightPopulation],WeightNumberFamilies,[WeightInsuredPopulation],[WeightNumberInsuredFamilies],[WeightNumberVisits],[WeightAdjustedAmount],[Recurrence]" &
              ")" &
          " SELECT [ProductCode],[ProductName],[LocationId],[DateFrom],[DateTo],[LumpSum],[MemberCount]" &
          ",[PremiumAdult],[PremiumChild],[InsurancePeriod],[DedInsuree],[DedOPInsuree],[DedIPInsuree]" &
          ",[MaxInsuree],[MaxOPInsuree],[MaxIPInsuree],[PeriodRelPrices],[PeriodRelPricesOP],[PeriodRelPricesIP]" &
          ",[AccCodePremiums],[AccCodeRemuneration],[DedTreatment],[DedOPTreatment],[DedIPTreatment],[MaxTreatment]" &
          ",[MaxOPTreatment],[MaxIPTreatment],[DedPolicy],[DedOPPolicy],[DedIPPolicy],[MaxPolicy],[MaxOPPolicy]" &
          ",[MaxIPPolicy],[GracePeriod],[ValidityFrom],getdate(),@ProdID,[AuditUserID]" &
          ",GracePeriodRenewal,RegistrationLumpSum,RegistrationFee,GeneralAssemblyLumpSum,GeneralAssemblyFee,StartCycle1" &
          ",StartCycle2,MaxNoConsultation,MaxNoSurgery,MaxNoDelivery,MaxNoHospitalizaion" &
          ",MaxNoVisits,MaxAmountConsultation,MaxAmountSurgery,MaxAmountDelivery,MaxAmountHospitalization,MaxInstallments,WaitingPeriod" &
          ",Threshold,RenewalDiscountPerc,RenewalDiscountPeriod,StartCycle3,StartCycle4,AdministrationPeriod" &
          ",MaxPolicyExtraMember,MaxPolicyExtraMemberIP,MaxPolicyExtraMemberOP" &
          ",MaxCeilingPolicy,MaxCeilingPolicyIP,MaxCeilingPolicyOP" &
          ",EnrolmentDiscountPerc,EnrolmentDiscountPeriod, MaxAmountAntenatal, MaxNoAntenatal, CeilingInterpretation,[Level1],[Sublevel1],[Level2],[Sublevel2],[Level3],[Sublevel3],[Level4],[Sublevel4],[ShareContribution],[WeightPopulation],WeightNumberFamilies,[WeightInsuredPopulation],[WeightNumberInsuredFamilies],[WeightNumberVisits],[WeightAdjustedAmount],[Recurrence]" &
          " FROM tblProduct WHERE ProdID = @ProdID;" &
          " UPDATE tblProduct SET [ProductCode] = @ProductCode,[ConversionProdID] = @ConversionProdID" &
          ",[ProductName] = @ProductName,[LocationId]=@LocationId,[DateFrom]=@DateFrom,[DateTo]=@DateTo" &
          ",[LumpSum]=@LumpSum,[MemberCount]=@MemberCount,[PremiumAdult]=@PremiumAdult" &
          ",[PremiumChild]=@PremiumChild,[InsurancePeriod]=@InsurancePeriod,[DedInsuree]=@DedInsuree" &
          ",[DedOPInsuree]=@DedOPInsuree,[DedIPInsuree]=@DedIPInsuree,[MaxInsuree]=@MaxInsuree" &
          ",[MaxOPInsuree]=@MaxOPInsuree,[MaxIPInsuree]=@MaxIPInsuree,[PeriodRelPrices]=@PeriodRelPrices" &
          ",[PeriodRelPricesOP]=@PeriodRelPricesOP,[PeriodRelPricesIP]=@PeriodRelPricesIP" &
          ",[AccCodePremiums]=@AccCodePremiums,[AccCodeRemuneration]=@AccCodeRemuneration" &
          ",[DedTreatment]=@DedTreatment,[DedOPTreatment]=@DedOPTreatment,[DedIPTreatment]=@DedIPTreatment" &
          ",[MaxTreatment]=@MaxTreatment,[MaxOPTreatment]=@MaxOPTreatment,[MaxIPTreatment]=@MaxIPTreatment" &
          ",[DedPolicy]=@DedPolicy,[DedOPPolicy]=@DedOPPolicy,[DedIPPolicy]=@DedIPPolicy,[MaxPolicy]=@MaxPolicy" &
          ",[MaxOPPolicy]=@MaxOPPolicy,[MaxIPPolicy]=@MaxIPPolicy " &
          ",[GracePeriod]=@GracePeriod,[ValidityFrom] = GetDate(),[AuditUserID] = @AuditUserID" &
          ",GracePeriodRenewal=@GracePeriodRenewal,RegistrationLumpSum=@RegistrationLumpSum,RegistrationFee=@RegistrationFee" &
          ",GeneralAssemblyLumpSum=@GeneralAssemblyLumpSum,GeneralAssemblyFee=@GeneralAssemblyFee" &
          ",StartCycle1=@StartCycle1,StartCycle2=@StartCycle2" &
          ",MaxNoConsultation=@MaxNoConsultation,MaxNoSurgery=@MaxNoSurgery,MaxNoDelivery=@MaxNoDelivery" &
          ",MaxNoHospitalizaion=@MaxNoHospitalizaion,MaxNoVisits=@MaxNoVisits,MaxAmountConsultation=@MaxAmountConsultation" &
          ",MaxAmountSurgery=@MaxAmountSurgery,MaxAmountDelivery=@MaxAmountDelivery,MaxAmountHospitalization=@MaxAmountHospitalization,MaxInstallments=@MaxInstallments,WaitingPeriod=@WaitingPeriod" &
          ",Threshold=@Threshold,RenewalDiscountPerc=@RenewalDiscountPerc,RenewalDiscountPeriod=@RenewalDiscountPeriod,StartCycle3=@StartCycle3,StartCycle4=@StartCycle4,AdministrationPeriod=@AdministrationPeriod" &
          ",MaxPolicyExtraMember=@MaxPolicyExtraMember,MaxPolicyExtraMemberIP=@MaxPolicyExtraMemberIP,MaxPolicyExtraMemberOP=@MaxPolicyExtraMemberOP" &
          ",MaxCeilingPolicy=@MaxCeilingPolicy,MaxCeilingPolicyIP=@MaxCeilingPolicyIP,MaxCeilingPolicyOP=@MaxCeilingPolicyOP" &
          ",EnrolmentDiscountPerc=@EnrolmentDiscountPerc,EnrolmentDiscountPeriod=@EnrolmentDiscountPeriod, MaxAmountAntenatal = @MaxAmountAntenatal, MaxNoAntenatal = @MaxNoAntenatal, CeilingInterpretation = @CeilingInterpretation," &
          " Level1 = @Level1, Sublevel1 = @Sublevel1, Level2 = @Level2,Sublevel2 = @Sublevel2, Level3 = @Level3, Sublevel3 = @Sublevel3, Level4 = @Level4 , Sublevel4 = @Sublevel4, ShareContribution = @ShareContribution, WeightPopulation = @WeightPopulation,WeightNumberFamilies = @WeightNumberFamilies, WeightInsuredPopulation = @WeightInsuredPopulation, WeightNumberInsuredFamilies = @WeightNumberInsuredFamilies , WeightNumberVisits = @WeightNumberVisits,WeightAdjustedAmount = @WeightAdjustedAmount, Recurrence = @Recurrence " &
          " WHERE ProdID = @ProdID"
        data.setSQLCommand(Query, CommandType.Text)

        data.params("@ProdID", SqlDbType.Int, eProducts.ProdID)
        data.params("@ProductCode", SqlDbType.NVarChar, 8, eProducts.ProductCode)
        data.params("@ProductName", SqlDbType.NVarChar, 100, eProducts.ProductName)
        data.params("@Recurrence", SqlDbType.TinyInt, eProducts.Recurrence)
        If eProducts.tblLocation.LocationId = -1 Then
            data.params("@LocationId", SqlDbType.Int, DBNull.Value)
        Else
            data.params("@LocationId", SqlDbType.Int, eProducts.tblLocation.LocationId)
        End If
        data.params("@DateFrom", SqlDbType.SmallDateTime, eProducts.DateFrom)
        data.params("@DateTo", SqlDbType.SmallDateTime, eProducts.DateTo)
        data.params("@ConversionProdID", SqlDbType.Int, if(eProducts.tblProduct2.ProdID = 0, DBNull.Value, eProducts.tblProduct2.ProdID))
        data.params("@LumpSum", SqlDbType.Decimal, eProducts.LumpSum)
        data.params("@MemberCount", SqlDbType.SmallInt, eProducts.MemberCount)
        data.params("@PremiumAdult", SqlDbType.Decimal, eProducts.PremiumAdult)
        data.params("@PremiumChild", SqlDbType.Decimal, eProducts.PremiumChild)
        data.params("@InsurancePeriod", SqlDbType.TinyInt, eProducts.InsurancePeriod)
        data.params("@DedInsuree", SqlDbType.Decimal, eProducts.DedInsuree)
        data.params("@DedOPInsuree", SqlDbType.Decimal, eProducts.DedOPInsuree)
        data.params("@DedIPInsuree", SqlDbType.Decimal, eProducts.DedIPInsuree)
        data.params("@MaxInsuree", SqlDbType.Decimal, eProducts.MaxInsuree)
        data.params("@MaxOPInsuree", SqlDbType.Decimal, eProducts.MaxOPInsuree)
        data.params("@MaxIPInsuree", SqlDbType.Decimal, eProducts.MaxIPInsuree)
        data.params("@PeriodRelPrices", SqlDbType.Char, 1, eProducts.PeriodRelPrices)
        data.params("@PeriodRelPricesOP", SqlDbType.Char, 1, eProducts.PeriodRelPricesOP)
        data.params("@PeriodRelPricesIP", SqlDbType.Char, 1, eProducts.PeriodRelPricesIP)
        data.params("@AccCodePremiums", SqlDbType.NVarChar, 25, eProducts.AccCodePremiums)
        data.params("@AccCodeRemuneration", SqlDbType.NVarChar, 25, eProducts.AccCodeRemuneration)
        data.params("@DedTreatment", SqlDbType.Decimal, eProducts.DedTreatment)
        data.params("@DedOPTreatment", SqlDbType.Decimal, eProducts.DedOPTreatment)
        data.params("@DedIPTreatment", SqlDbType.Decimal, eProducts.DedIPTreatment)
        data.params("@MaxTreatment", SqlDbType.Decimal, eProducts.MaxTreatment)
        data.params("@MaxOPTreatment", SqlDbType.Decimal, eProducts.MaxOPTreatment)
        data.params("@MaxIPTreatment", SqlDbType.Decimal, eProducts.MaxIPTreatment)
        data.params("@DedPolicy", SqlDbType.Decimal, eProducts.DedPolicy)
        data.params("@DedOPPolicy", SqlDbType.Decimal, eProducts.DedOPPolicy)
        data.params("@DedIPPolicy", SqlDbType.Decimal, eProducts.DedIPPolicy)
        data.params("@MaxPolicy", SqlDbType.Decimal, eProducts.MaxPolicy)
        data.params("@MaxOPPolicy", SqlDbType.Decimal, eProducts.MaxOPPolicy)
        data.params("@MaxIPPolicy", SqlDbType.Decimal, eProducts.MaxIPPolicy)
        data.params("@GracePeriod", SqlDbType.Int, eProducts.GracePeriod)
        data.params("@AuditUserID", SqlDbType.Int, eProducts.AuditUserID)
        'Addition for Bepha start:
        data.params("@GracePeriodRenewal", SqlDbType.Int, eProducts.GracePeriodRenewal)
        data.params("@RegistrationLumpSum", SqlDbType.Decimal, eProducts.RegistrationLumpSum)
        data.params("@RegistrationFee", SqlDbType.Decimal, eProducts.RegistrationFee)
        data.params("@GeneralAssemblyLumpSum", SqlDbType.Decimal, eProducts.GeneralAssemblyLumpSum)
        data.params("@GeneralAssemblyFee", SqlDbType.Decimal, eProducts.GeneralAssemblyFee)
        data.params("@StartCycle1", SqlDbType.NVarChar, 5, eProducts.StartCycle1)
        data.params("@StartCycle2", SqlDbType.NVarChar, 5, eProducts.StartCycle2)
        data.params("@MaxNoConsultation", SqlDbType.Int, eProducts.MaxNoConsultation)
        data.params("@MaxAmountConsultation", SqlDbType.Decimal, eProducts.MaxAmountConsultation)
        data.params("@MaxNoSurgery", SqlDbType.Int, eProducts.MaxNoSurgery)
        data.params("@MaxAmountSurgery", SqlDbType.Decimal, eProducts.MaxAmountSurgery)
        data.params("@MaxNoDelivery", SqlDbType.Int, eProducts.MaxNoDelivery)
        data.params("@MaxAmountDelivery", SqlDbType.Decimal, eProducts.MaxAmountDelivery)
        data.params("@MaxNoHospitalizaion", SqlDbType.Int, eProducts.MaxNoHospitalizaion)
        data.params("@MaxAmountHospitalization", SqlDbType.Decimal, eProducts.MaxAmountHospitalization)
        data.params("@MaxNoVisits", SqlDbType.Int, eProducts.MaxNoVisits)
        data.params("@MaxInstallments", SqlDbType.Int, eProducts.MaxInstallments)
        data.params("@WaitingPeriod", SqlDbType.Int, eProducts.WaitingPeriod)
        data.params("@MaxAmountAntenatal", SqlDbType.Decimal, eProducts.MaxAmountAntenatal)
        data.params("@MaxNoAntenatal", SqlDbType.Int, eProducts.MaxNoAntenatal)
        'Addition for Bepha end;
        'Addition for Nepal >> Start
        data.params("@Threshold", SqlDbType.Int, eProducts.Threshold)
        data.params("@RenewalDiscountPerc", SqlDbType.Int, eProducts.RenewalDiscountPerc)
        data.params("@RenewalDiscountPeriod", SqlDbType.Int, eProducts.RenewalDiscountPeriod)
        data.params("@StartCycle3", SqlDbType.NVarChar, 5, eProducts.StartCycle3)
        data.params("@StartCycle4", SqlDbType.NVarChar, 5, eProducts.StartCycle4)
        data.params("@AdministrationPeriod", SqlDbType.Int, eProducts.AdministrationPeriod)
        data.params("@MaxPolicyExtraMember", SqlDbType.Decimal, eProducts.MaxPolicyExtraMember)
        data.params("@MaxPolicyExtraMemberIP", SqlDbType.Decimal, eProducts.MaxPolicyExtraMemberIP)
        data.params("@MaxPolicyExtraMemberOP", SqlDbType.Decimal, eProducts.MaxPolicyExtraMemberOP)
        data.params("@MaxCeilingPolicy", SqlDbType.Decimal, eProducts.MaxCeilingPolicy)
        data.params("@MaxCeilingPolicyIP", SqlDbType.Decimal, eProducts.MaxCeilingPolicyIP)
        data.params("@MaxCeilingPolicyOP", SqlDbType.Decimal, eProducts.MaxCeilingPolicyOP)
        data.params("@EnrolmentDiscountPerc", SqlDbType.Int, eProducts.EnrolmentDiscountPerc)
        data.params("@EnrolmentDiscountPeriod", SqlDbType.Int, eProducts.EnrolmentDiscountPeriod)
        data.params("@CeilingInterpretation", SqlDbType.Char, 1, eProducts.CeilingInterpretation)
        'Addition for Nepal >> End

        '*********Capitation*******************
        data.params("@Level1 ", SqlDbType.Char, 1, eProducts.Level1)
        data.params("@Sublevel1 ", SqlDbType.Char, 1, eProducts.Sublevel1)
        data.params("@Level2 ", SqlDbType.Char, 1, eProducts.Level2)
        data.params("@Sublevel2 ", SqlDbType.Char, 1, eProducts.Sublevel2)
        data.params("@Level3 ", SqlDbType.Char, 1, eProducts.Level3)
        data.params("@Sublevel3 ", SqlDbType.Char, 1, eProducts.Sublevel3)
        data.params("@Level4 ", SqlDbType.Char, 1, eProducts.Level4)
        data.params("@Sublevel4 ", SqlDbType.Char, 1, eProducts.Sublevel4)
        data.params("@ShareContribution ", SqlDbType.Float, eProducts.ShareContribution)
        data.params("@WeightPopulation ", SqlDbType.Float, eProducts.WeightPopulation)
        data.params("@WeightNumberFamilies ", SqlDbType.Float, eProducts.WeightNumberFamilies)
        data.params("@WeightInsuredPopulation ", SqlDbType.Float, eProducts.WeightInsuredPopulation)
        data.params("@WeightNumberInsuredFamilies", SqlDbType.Float, eProducts.WeightNumberInsuredFamilies)
        data.params("@WeightNumberVisits", SqlDbType.Float, eProducts.WeightNumberVisits)
        data.params("@WeightAdjustedAmount", SqlDbType.Float, eProducts.WeightAdjustedAmount)

        '**************************************
        data.ExecuteCommand()
    End Sub
    Public Sub DeleteProduct(ByRef eProduct As IMIS_EN.tblProduct)
        Dim data As New ExactSQL
        Dim Query As String = "Insert into tblProduct([ProductCode],[ProductName],[LocationId],[DateFrom],[DateTo]" & _
            ",[LumpSum],[MemberCount],[PremiumAdult],[PremiumChild],[InsurancePeriod],[DedInsuree],[DedOPInsuree]" & _
            ",[DedIPInsuree],[MaxInsuree],[MaxOPInsuree],[MaxIPInsuree],[PeriodRelPrices],[PeriodRelPricesOP]" & _
            ",[PeriodRelPricesIP],[AccCodePremiums],[AccCodeRemuneration],[DedTreatment],[DedOPTreatment]" & _
            ",[DedIPTreatment],[MaxTreatment],[MaxOPTreatment],[MaxIPTreatment],[DedPolicy],[DedOPPolicy]" & _
            ",[DedIPPolicy],[MaxPolicy],[MaxOPPolicy],[MaxIPPolicy],[GracePeriod],[ValidityFrom],[ValidityTo]" & _
            ",[LegacyID],[AuditUserID]" & _
              ",GracePeriodRenewal,RegistrationLumpSum,RegistrationFee,GeneralAssemblyLumpSum,GeneralAssemblyFee,StartCycle1" & _
              ",StartCycle2,MaxNoConsultation,MaxNoSurgery,MaxNoDelivery,MaxNoHospitalizaion" & _
              ",MaxNoVisits,MaxAmountConsultation,MaxAmountSurgery,MaxAmountDelivery,MaxAmountHospitalization,MaxInstallments,WaitingPeriod" & _
              ",Threshold,RenewalDiscountPerc,RenewalDiscountPeriod,StartCycle3,StartCycle4,AdministrationPeriod" & _
              ",MaxPolicyExtraMember,MaxPolicyExtraMemberIP,MaxPolicyExtraMemberOP" & _
              ",MaxCeilingPolicy,MaxCeilingPolicyIP,MaxCeilingPolicyOP" & _
              ",EnrolmentDiscountPerc,EnrolmentDiscountPeriod,MaxAmountAntenatal, MaxNoAntenatal, CeilingInterpretation,[Level1],[Sublevel1],[Level2],[Sublevel2],[Level3],[Sublevel3],[Level4],[Sublevel4],[ShareContribution],[WeightPopulation],WeightNumberFamilies,[WeightInsuredPopulation],[WeightNumberInsuredFamilies],[WeightNumberVisits],[WeightAdjustedAmount]" & _
              ")" & _
          " SELECT [ProductCode],[ProductName],[LocationId],[DateFrom],[DateTo],[LumpSum],[MemberCount]" & _
          ",[PremiumAdult],[PremiumChild],[InsurancePeriod],[DedInsuree],[DedOPInsuree],[DedIPInsuree]" & _
          ",[MaxInsuree],[MaxOPInsuree],[MaxIPInsuree],[PeriodRelPrices],[PeriodRelPricesOP],[PeriodRelPricesIP]" & _
          ",[AccCodePremiums],[AccCodeRemuneration],[DedTreatment],[DedOPTreatment],[DedIPTreatment],[MaxTreatment]" & _
          ",[MaxOPTreatment],[MaxIPTreatment],[DedPolicy],[DedOPPolicy],[DedIPPolicy],[MaxPolicy],[MaxOPPolicy]" & _
          ",[MaxIPPolicy],[GracePeriod],[ValidityFrom],getdate(),@ProdID,[AuditUserID]" & _
          ",GracePeriodRenewal,RegistrationLumpSum,RegistrationFee,GeneralAssemblyLumpSum,GeneralAssemblyFee,StartCycle1" & _
          ",StartCycle2,MaxNoConsultation,MaxNoSurgery,MaxNoDelivery,MaxNoHospitalizaion" & _
          ",MaxNoVisits,MaxAmountConsultation,MaxAmountSurgery,MaxAmountDelivery,MaxAmountHospitalization,MaxInstallments,WaitingPeriod" & _
          ",Threshold,RenewalDiscountPerc,RenewalDiscountPeriod,StartCycle3,StartCycle4,AdministrationPeriod" & _
          ",MaxPolicyExtraMember,MaxPolicyExtraMemberIP,MaxPolicyExtraMemberOP" & _
          ",MaxCeilingPolicy,MaxCeilingPolicyIP,MaxCeilingPolicyOP" & _
          ",EnrolmentDiscountPerc,EnrolmentDiscountPeriod,MaxAmountAntenatal, MaxNoAntenatal, CeilingInterpretation,[Level1],[Sublevel1],[Level2],[Sublevel2],[Level3],[Sublevel3],[Level4],[Sublevel4],[ShareContribution],[WeightPopulation],WeightNumberFamilies,[WeightInsuredPopulation],[WeightNumberInsuredFamilies],[WeightNumberVisits],[WeightAdjustedAmount]" & _
          " FROM tblProduct where ProdID = @ProdID;" & _
        " UPDATE [tblProduct]   SET [ValidityFrom] = GetDate(),[ValidityTo] = GetDate()" & _
        ",[AuditUserID] = @AuditUserID  WHERE ProdID = @ProdID"
        data.setSQLCommand(Query, CommandType.Text)
        data.params("@ProdID", SqlDbType.Int, eProduct.ProdID)
        data.params("@AuditUserID", SqlDbType.Int, eProduct.AuditUserID)
        data.ExecuteCommand()
    End Sub
    Public Function GetDistribution(ByVal ProdId As Integer, ByVal CareType As String, ByVal distType As Integer) As DataTable
        Dim data As New ExactSQL
        data.setSQLCommand("SELECT Distrid,DistrCareType,period,Prodid,DistrPerc FROM tblrelDistr WHERE ProdID = @ProdID AND DistrCareType = @CareType order by Period", CommandType.Text)
        data.params("@ProdID", SqlDbType.NVarChar, ProdId)
        data.params("@CareType", SqlDbType.NVarChar, 1, CareType)
        Return (data.Filldata)
    End Function
    Public Function CheckIfCanDelete(ByVal eProduct As IMIS_EN.tblProduct) As Boolean
        Dim data As New ExactSQL
        data.setSQLCommand("SELECT Top 1 PRODID FROM tblPolicy WHERE ProdID = @ProdID and validityto is null", CommandType.Text)
        data.params("@ProdID", SqlDbType.Int, eProduct.ProdID)
        Return Not data.ExecuteScalar()
    End Function
    Public Function GetPeriodForPolicy(ByVal ProdId As Integer, ByVal EnrolDate As Date, Optional ByRef HasCycle As Boolean = False, Optional PolicyStage As String = "N") As DataTable
        Dim sSQL As String = "uspGetPolicyPeriod"
        Dim data As New ExactSQL

        data.setSQLCommand(sSQL, CommandType.StoredProcedure)

        data.params("@ProdId", SqlDbType.Int, ProdId)
        data.params("@EnrolDate", SqlDbType.Date, EnrolDate)
        data.params("@HasCycles", SqlDbType.Bit, HasCycle, ParameterDirection.Output)
        data.params("@PolicyStage", SqlDbType.NVarChar, 1, PolicyStage)
        Dim dt As DataTable = data.Filldata
        HasCycle = data.sqlParameters("@HasCycles")
        Return dt
    End Function
    Public Function GetRegistrationFee(ByVal PolicyId As Integer) As Double
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "SELECT PL.ProdID,CASE WHEN ISNULL(Prod.RegistrationLumpSum,0) > 0 THEN ISNULL(Prod.RegistrationLumpSum ,0) WHEN ISNULL(Prod.RegistrationFee,0) > 0 THEN COUNT(I.InsureeID) * ISNULL(Prod.RegistrationFee,0) ELSE 0 END AS RegistrationFee FROM tblProduct Prod INNER JOIN tblPolicy PL ON Prod.ProdID = PL.ProdID INNER JOIN tblInsuree I ON I.FamilyID = PL.FamilyID WHERE(Prod.ValidityTo Is NULL) AND PL.ValidityTo IS NULL AND I.ValidityTo IS NULL AND PL.PolicyID = @PolicyId GROUP BY PL.ProdID,Prod.RegistrationLumpSum,Prod.RegistrationFee"

        data.setSQLCommand(sSQL, CommandType.Text)

        data.params("@PolicyId", SqlDbType.Int, PolicyId)

        Return data.Filldata(0).Rows(0)("RegistrationFee")

    End Function
    Public Function GetProductAdministrationPeriod(ByVal ProdId As Integer) As Integer
        Dim sSQL As String = "SELECT AdministrationPeriod FROM tblProduct WHERE ProdId = @ProdId"
        Dim data As New ExactSQL
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProdId", SqlDbType.Int, ProdId)
        Dim dt As DataTable = data.Filldata
        If dt.Rows.Count > 0 AndAlso dt.Rows(0)("AdministrationPeriod") IsNot DBNull.Value Then Return dt.Rows(0)("AdministrationPeriod")
        Return 0
    End Function

    Public Function GetProductName_Account(ProdId As Integer) As DataTable
        Dim sSQL As String = "SELECT ProductName, AccCodePremiums FROM tblProduct WHERE ProdId = @ProdId"
        Dim data As New ExactSQL
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProdId", SqlDbType.Int, ProdId)
        Return data.Filldata
    End Function
    Public Function GetAllProducts() As DataTable
        Dim data As New ExactSQL
        Dim strSQL As String = ""
        strSQL = "SELECT ProdId, ProductCode  FROM tblProduct WHERE ValidityTo IS NULL"
        data.setSQLCommand(strSQL, CommandType.Text)
        Return data.Filldata
    End Function
    Public Function getProductDetailMin(ProdId As Integer) As IMIS_EN.tblProduct
        Dim sSQL As String = "SELECT ProdId, ProductCode FROM tblProduct WHERE ProdId = @ProdId"
        Dim Data As New ExactSQL
        Data.setSQLCommand(sSQL, CommandType.Text)
        Data.params("@ProdId", SqlDbType.Int, ProdId)
        Dim dt As DataTable = Data.Filldata

        Dim eProduct As New IMIS_EN.tblProduct

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            With eProduct
                .ProdID = dt(0)("ProdId")
                .ProductCode = dt(0)("ProductCode").ToString
            End With
        End If
        Return eProduct
    End Function

    Public Function GetProductForRenewal(ProdId As Integer, EnrollDate As Date) As IMIS_EN.tblProduct
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        Dim eProd As New IMIS_EN.tblProduct

        sSQL = ";WITH Prod AS"
        sSQL += " ("
        sSQL += " SELECT ProdId, ProductCode, ConversionProdId, ValidityFrom, ValidityTo, DateFrom, DateTo"
        sSQL += " FROM tblProduct"
        sSQL += " WHERE (ProdId = @ProdId OR LegacyId = @ProdId)"
        sSQL += " UNION ALL"
        sSQL += " SELECT CP.ProdId, CP.ProductCode, CP.ConversionProdId, CP.ValidityFrom, CP.ValidityTo, CP.DateFrom, CP.DateTo"
        sSQL += " FROM tblProduct CP INNER JOIN Prod ON CP.ProdId = Prod.ConversionProdID"
        sSQL += " )"
        sSQL += " SELECT TOP 1 Prod.ProdId, Prod.ProductCode"
        sSQL += " FROM Prod"
        sSQL += " WHERE Prod.Validityto IS NULL"
        sSQL += " AND (@EnrollDate BETWEEN Prod.DateFrom AND Prod.DateTo)"
        sSQL += " ORDER BY Prod.DateFrom, Prod.DateTo"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProdId", SqlDbType.Int, ProdId)
        data.params("@EnrollDate", SqlDbType.Date, EnrollDate)

        Dim dt As DataTable = data.Filldata

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            With eProd
                .ProdID = dt(0)("ProdId")
                .ProductCode = dt(0)("ProductCode").ToString
            End With
        End If

        Return eProd

    End Function

    'Get Product Locationwise
    Public Function GetProductsStict(LocationId As Integer, UserId As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""

        'sSQL += " SELECT ProdID, ProductCode, ProductName FROM tblProduct P"
        'sSQL += " WHERE P.ValidityTo IS NULL"
        'sSQL += " AND ISNULL(P.LocationId,-1) = ISNULL(@LocationId,-1)"

        sSQL += " SELECT Prod.ProdId, Prod.ProductCode , Prod.LocationId FROM tblProduct Prod"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId, 0) = ISNULL(Prod.LocationId, 0)"
        sSQL += " LEFT JOIN tblUsersDistricts UD ON Prod.LocationId = UD.LocationId AND UD.UserId = @UserId AND UD.ValidityTo IS NULL"
        sSQL += " LEFT JOIN ("
        sSQL += " SELECT   ProductCode, MIN(ValidityFrom) ValidityFrom"
        sSQL += " FROM tblProduct"
        sSQL += " WHERE LegacyID IS NOT NULL"
        sSQL += " GROUP BY ProductCode) HPROD ON HPROD.ProductCode=PROD.ProductCode"
        sSQL += " WHERE Prod.ValidityTo IS NULL"
        sSQL += " AND ISNULL(L.LocationId,-1) = ISNULL(@LocationId,-1)"
        sSQL += " ORDER BY L.ParentLocationId"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@LocationId", SqlDbType.Int, If(LocationId = -1, 0, LocationId))
        data.params("@UserId", SqlDbType.Int, UserId)
        Return data.Filldata
    End Function
    Public Function getProductCapitationDetails(ByVal ProductId As Integer, ByVal dt As DataTable) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL
        sSQL += " SELECT [Level1],[Sublevel1],[Level2],[Sublevel2],[Level3],[Sublevel3],[Level4],[Sublevel4],[ShareContribution],[WeightPopulation],WeightNumberFamilies"
        sSQL += " ,[WeightInsuredPopulation],[WeightNumberInsuredFamilies],[WeightNumberVisits],[WeightAdjustedAmount],HF1.HFSublevelDesc HFSublevel1, HF2.HFSublevelDesc HFSublevel2 , HF3.HFSublevelDesc HFSublevel3, HF4.HFSublevelDesc HFSublevel4"
        sSQL += " FROM  [tblProduct] P"
        sSQL += " LEFT JOIN    tblHFSublevel HF1 ON HF1.HFSublevel = P.Sublevel1"
        sSQL += " LEFT JOIN    tblHFSublevel HF2 ON HF2.HFSublevel = P.Sublevel2"
        sSQL += " LEFT JOIN    tblHFSublevel HF3 ON HF3.HFSublevel = P.Sublevel3"
        sSQL += " LEFT JOIN    tblHFSublevel HF4 ON HF4.HFSublevel = P.Sublevel4"

        '  sSQL += " LEFT JOIN dtHFlevel HF ON HF.Level1 =  "
        sSQL += " WHERE P.ValidityTo IS NULL AND P.ProdID = @ProductId"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProductId", ProductId)
        data.params("@dtHFlevel", dt, "xAttributeV")
        Return data.Filldata
    End Function
    Public Function GetProdIdByUUID(ByVal uuid As Guid) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select ProdId from tblProduct where ProdUUID = @ProdUUID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProdUUID", SqlDbType.UniqueIdentifier, uuid)

        Return data.Filldata
    End Function
    Public Function GetProductUUIDByID(ByVal id As Integer) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select ProdUUID from tblProduct where ProdId = @ProdId"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@ProdId", SqlDbType.Int, id)

        Return data.Filldata
    End Function
End Class
