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

Public Class PaymentDAL
    Dim data As New ExactSQL
    Public Sub LoadPremium(ByRef ePremium As IMIS_EN.tblPremium, ByRef PremiumContribution As Decimal)
        Dim ePolicy As New IMIS_EN.tblPolicy
        Dim ePayer As New IMIS_EN.tblPayer
        Dim eProduct As New IMIS_EN.tblProduct

        Dim data As New ExactSQL
        data.setSQLCommand("select pr.*,( SELECT SUM(Amount) FROM tblPremium WHERE PolicyID=@PolicyID AND tblPremium.ValidityTo IS NULL AND isPhotoFee = 0  ) AS PremiumContribution" &
                           ",ISNULL(po.PolicyValue,0) PolicyValue,po.PolicyStatus,po.StartDate,po.EffectiveDate,po.InsurancePeriod,po.isOffline as PolicyIsOffline FROM tblPremium pr" &
                           " INNER JOIN ( SELECT PolicyID,PolicyValue,startDate" &
                           ",PolicyStatus" &
                           ",EffectiveDate,pd.InsurancePeriod,isOffline  FROM tblPolicy" &
                           " INNER JOIN tblProduct pd ON pd.ProdID=tblPolicy.ProdID" &
                           " WHERE PolicyID=@PolicyID AND  tblPolicy.Validityto is null ) AS po" &
                           " ON po.PolicyID = pr.PolicyID where PremiumId = @PremiumID and pr.validityto is null", CommandType.Text)
        data.params("@PremiumID", SqlDbType.Int, ePremium.PremiumId)
        data.params("@PolicyID", SqlDbType.Int, ePremium.tblPolicy.PolicyID)

        Dim dr As DataRow = data.Filldata()(0)
        If Not dr Is Nothing Then
            ePolicy.PolicyID = dr("PolicyID")
            ePayer.PayerID = If(dr.IsNull("PayerID"), Nothing, dr("PayerID"))
            ePremium.tblPayer = ePayer
            ePremium.Amount = dr("Amount")
            ePremium.Receipt = dr("Receipt")
            ePremium.PayDate = dr("PayDate")
            ePremium.PayType = dr("PayType")
            eProduct.InsurancePeriod = dr("InsurancePeriod")
            ePolicy.StartDate = dr("StartDate")
            If Not dr("isPhotoFee") Is DBNull.Value Then
                ePremium.isPhotoFee = dr("isPhotoFee")
            End If

            If Not dr("PolicyValue") Is DBNull.Value Then
                ePolicy.PolicyValue = dr("PolicyValue")
            End If
            If Not dr("PolicyStatus") Is DBNull.Value Then
                ePolicy.PolicyStatus = dr("PolicyStatus")
            End If
            If Not dr("EffectiveDate") Is DBNull.Value Then
                ePolicy.EffectiveDate = dr("EffectiveDate")
            End If

            If Not dr("PremiumContribution") Is DBNull.Value Then
                PremiumContribution = dr("PremiumContribution")
            End If
            If dr("isOffline") IsNot DBNull.Value Then
                ePremium.isOffline = dr("isOffline")
            End If
            If dr("PolicyIsOffline") IsNot DBNull.Value Then
                ePolicy.isOffline = dr("PolicyIsOffline")
            End If

            If Not dr("ValidityTo") Is DBNull.Value Then
                ePremium.ValidityTo = dr("ValidityTo")
            End If
        End If


        ePremium.tblPolicy = ePolicy
        ePremium.tblPolicy.tblProduct = eProduct
    End Sub
    Public Function getPayment(PaymentId As String, dtPaymentStatus As DataTable) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = " SELECT py.PaymentID,PD.InsuranceNumber, PY.OfficerCode, PY.ExpectedAmount,PS.StatusID, PY.ReceiptNo, PY.RejectedReason, CN.ControlNumber, PY.TransactionNo, PY.PhoneNumber,  PY.PaymentDate, CONVERT(DATE, PY.ReceivedDate,103) ReceivedDate, CONVERT(DATE, PY.MatchedDate,103) MatchedDate,     PY.ReceivedAmount, PD.ProductCode, CASE  WHEN PY.PaymentStatus < 0 THEN 'Failed' ELSE PS.PaymenyStatusName END AS PaymenyStatusName, PY.PaymentOrigin, PY.ValidityFrom, PY.ValidityTo FROM tblPaymentDetails PD"
        sSQL += " INNER Join tblPayment PY ON PY.PaymentID = PD.PaymentID"
        sSQL += " Left OUTER JOIN tblControlNumber CN ON CN.PaymentID = PY.PaymentID"
        sSQL += " Left OUTER JOIN @dtPaymentStatus PS ON PS.StatusID = PY.PaymentStatus"
        sSQL += " WHERE"
        sSQL += "  py.PaymentUUID = @PaymentUUID"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PaymentUUID", PaymentId)
        data.params("@dtPaymentStatus", dtPaymentStatus, "xPayementStatus")
        Return data.Filldata
    End Function
    Public Function GetPayment(ByVal ePayment As IMIS_EN.tblPayment) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "  ;WITH UD AS(  SELECT L.DistrictId, L.Region FROM tblUsersDistricts UD"
        sSQL += " INNER JOIN tblDistricts L ON L.DistrictId = UD.LocationId"
        sSQL += "  WHERE UD.ValidityTo IS NULL AND (UD.UserId = @UserId OR @UserId = 0)"
        sSQL += " GROUP BY L.DistrictId, L.Region )"
        sSQL = " SELECT " + UtilitiesDAL.GetEnvMaxRows()
        sSQL += " py.PaymentID, py.PaymentUUID, PY.OfficerCode, PY.ExpectedAmount, PY.ReceiptNo, PY.RejectedReason, CN.ControlNumber, PY.TransactionNo, PY.PhoneNumber, PY.PaymentDate, PY.ReceivedDate,  PY.MatchedDate MatchingDate, ISNULL(PY.ReceivedAmount,PY.ExpectedAmount) ReceivedAmount,  PY.PaymentOrigin,PS.PaymenyStatusName, PY.ValidityFrom, PY.ValidityTo  FROM tblPaymentDetails PD "
        sSQL += " INNER Join tblPayment PY ON PY.PaymentID = PD.PaymentID"
        sSQL += "  INNER JOIN tblInsuree I ON I.CHFID = PD.InsuranceNumber"
        sSQL += "  INNER JOIN tblFamilies F ON F.FamilyID =  I.FamilyID"
        sSQL += " INNER JOIN uvwLocations L ON ISNULL(L.LocationId,0) = ISNULL(F.LocationId,0)"
        sSQL += " Left OUTER JOIN tblControlNumber CN ON CN.PaymentID = PY.PaymentID"
        sSQL += " INNER JOIN @dtPaymentStatus PS ON PS.StatusID = PY.PaymentStatus"
        sSQL += " WHERE"
        'sSQL += " PD.ValidityTo IS NULL"
        sSQL += " CN.ValidityTo IS NULL"

        If ePayment.Legacy = False Then
            sSQL += " AND PD.ValidityTo IS NULL"
        End If
        If ePayment.InsuranceNumber IsNot Nothing Then
            sSQL += " AND PD.InsuranceNumber LIKE @InsuranceNumber"
        End If
        If ePayment.OfficerCode IsNot Nothing Then
            sSQL += " AND PY.OfficerCode LIKE @OfficerCode"
        End If
        If ePayment.ReceiptNo IsNot Nothing Then
            sSQL += " AND PY.ReceiptNo LIKE @ReceiptNo"
        End If
        If ePayment.TransactionNumber IsNot Nothing Then
            sSQL += " AND PY.TransactionNo LIKE @TransactionNo"
        End If
        If ePayment.PaymentOrigin IsNot Nothing Then
            sSQL += " AND PY.PaymentOrigin LIKE @PaymentOrigin"
        End If
        If ePayment.ControlNumber IsNot Nothing Then
            sSQL += " AND CN.ControlNumber LIKE @ControlNumber"
        End If
        If ePayment.PhoneNumber IsNot Nothing Then
            sSQL += " AND PY.PhoneNumber LIKE @PhoneNumber"
        End If
        If ePayment.ProductCode IsNot Nothing Then
            sSQL += " AND PD.ProductCode Like @ProductCode"
        End If
        If ePayment.DateFrom IsNot Nothing Then
            sSQL += " AND PY.PaymentDate >= @DateFrom"
        End If
        If ePayment.DateTo IsNot Nothing Then
            sSQL += " AND PY.PaymentDate <= @DateTo"
        End If
        If ePayment.ReceivingDateFrom IsNot Nothing Then
            sSQL += " AND PY.ReceivedDate >= @ReceivingDateFrom"
        End If
        If ePayment.ReceivingDateTo IsNot Nothing Then
            sSQL += " AND PY.ReceivedDate <= @ReceivingDateTo"
        End If
        If ePayment.MatchDateFrom IsNot Nothing Then
            sSQL += " AND PY.MatchedDate >= @MatchDateFrom"
        End If
        If ePayment.MatchedDateTo IsNot Nothing Then
            sSQL += " AND PY.MatchedDate <= @MatchedDateTo"
        End If
        If ePayment.ReceivedAmountFrom IsNot Nothing Then
            sSQL += " AND PY.ReceivedAmount >= @ReceivedAmountFrom"
        End If
        If ePayment.ReceivedAmountTO IsNot Nothing Then
            sSQL += " AND PY.ReceivedAmount <= @ReceivedAmountTO"
        End If
        If Not ePayment.RegionId = 0 Then
            sSQL += " AND L.RegionId= @RegionId"
        End If
        If Not ePayment.DistrictId = 0 Then
            sSQL += " AND L.DistrictID = @DistrictID"
        End If

        If ePayment.PaymentStatus IsNot Nothing Then
            If ePayment.PaymentStatus < 0 Then
                sSQL += " AND (PY.PaymentStatus = -1 OR PY.PaymentStatus = -2 OR PY.PaymentStatus = -3 OR PY.PaymentStatus = -4 OR PY.PaymentStatus = -5)"
            Else
                sSQL += " AND PY.PaymentStatus = @PaymentStatus"
            End If

        End If
        sSQL += " GROUP BY  py.PaymentID, py.PaymentUUID, PY.OfficerCode, PY.ExpectedAmount, PY.ReceiptNo, CN.ControlNumber, PY.TransactionNo, PY.PhoneNumber, PY.PaymentDate, PY.ReceivedDate,  PY.MatchedDate , PY.ReceivedAmount,PY.ExpectedAmount,  PaymenyStatusName, PY.PaymentOrigin, PY.ValidityFrom, PY.ValidityTo, PY.RejectedReason"
        sSQL += " ORDER BY PaymentID DESC "
        'If ePayment.LocationID IsNot Nothing Then
        'sSQL += " AND PD.LocationID = @LocationID"
        'End If
        'sSQL += " ORDER BY py.PaymentID,PY.OfficerCode, PY.ExpectedAmount, PY.ReceiptNo, CN.ControlNumber, PY.TransactionNo, PY.PhoneNumber, PY.PaymentDate, PY.ReceivedDate,  PY.MatchedDate,  PY.PaymentOrigin"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@InsuranceNumber", SqlDbType.NVarChar, 15, ePayment.InsuranceNumber + "%")
        data.params("@TransactionNo", SqlDbType.NVarChar, 50, ePayment.TransactionNumber + "%")
        data.params("@ControlNumber", SqlDbType.NVarChar, 50, ePayment.ControlNumber + "%")
        data.params("@PhoneNumber", SqlDbType.NVarChar, 25, ePayment.PhoneNumber + "%")
        data.params("@OfficerCode", SqlDbType.NVarChar, 15, ePayment.OfficerCode + "%")
        data.params("@ProductCode", SqlDbType.NVarChar, 8, ePayment.ProductCode + "%")
        data.params("@ReceiptNo", SqlDbType.NVarChar, 100, ePayment.ReceiptNo + "%")
        data.params("@PaymentOrigin", SqlDbType.NVarChar, 50, ePayment.PaymentOrigin + "%")
        data.params("@PaymentStatus", SqlDbType.Int, ePayment.PaymentStatus)
        data.params("@UserId", SqlDbType.Int, ePayment.AuditUserID)
        data.params("@DateFrom", SqlDbType.SmallDateTime, ePayment.DateFrom)
        If ePayment.DateTo IsNot Nothing AndAlso ePayment.DateTo = ePayment.DateTo.Value.Date Then
            ePayment.DateTo = ePayment.DateTo.Value.AddDays(1)
        End If
        data.params("@DateTo", SqlDbType.SmallDateTime, ePayment.DateTo)
        data.params("@ReceivingDateFrom", SqlDbType.SmallDateTime, ePayment.ReceivingDateFrom)
        If ePayment.ReceivingDateTo IsNot Nothing AndAlso ePayment.ReceivingDateTo = ePayment.ReceivingDateTo.Value.Date Then
            ePayment.ReceivingDateTo = ePayment.ReceivingDateTo.Value.AddDays(1)
        End If
        data.params("@ReceivingDateTo", SqlDbType.SmallDateTime, ePayment.ReceivingDateTo)
        data.params("@MatchDateFrom", SqlDbType.SmallDateTime, ePayment.MatchDateFrom)
        If ePayment.MatchedDateTo IsNot Nothing AndAlso ePayment.MatchedDateTo = ePayment.MatchedDateTo.Value.Date Then
            ePayment.MatchedDateTo = ePayment.MatchedDateTo.Value.AddDays(1)
        End If
        data.params("@MatchedDateTo", SqlDbType.SmallDateTime, ePayment.MatchedDateTo)
        data.params("@ReceivedAmountFrom", SqlDbType.Decimal, ePayment.ReceivedAmountFrom)
        data.params("@ReceivedAmountTO", SqlDbType.Decimal, ePayment.ReceivedAmountTO)
        data.params("@RegionId", SqlDbType.Int, ePayment.RegionId)
        data.params("@DistrictID", SqlDbType.Int, ePayment.DistrictId)
        data.params("@dtPaymentStatus", ePayment.dtPaymentStatus, "xPayementStatus")

        Return data.Filldata
    End Function
    Public Function LoadPayment(ByVal ePayment As IMIS_EN.tblPayment) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = " SELECT PaymentDetailsID, PD.PaymentID, PY.PhoneNumber,PD.InsuranceNumber, PD.ProductCode, PD.PolicyStage, PD.Amount,  CONVERT(nvarchar(12),PY.MatchedDate,103) MatchedDate, PD.ValidityFrom, PD.ValidityTo, PY.OfficerCode, PY.ReceivedAmount,  CONVERT(nvarchar(12),PY.ReceivedDate,103) ReceivedDate, PY.ReceiptNo, PY.TransactionNo, CN.ControlNumber,PY.PaymentOrigin,  CONVERT(nvarchar(12),PY.PaymentDate,103) PaymentDate, ISNULL(PY.ReceivedAmount,0) ReceivedAmount, PS.PaymenyStatusName, PY.PaymentStatus, PD.ExpectedAmount FROM tblPaymentDetails PD"
        sSQL += " INNER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID"
        sSQL += " LEFT OUTER JOIN @dtPaymentStatus PS ON PY.PaymentStatus = PS.StatusID"
        sSQL += " LEFT OUTER JOIN tblControlNumber CN ON CN.PaymentID = PY.PaymentID  AND CN.ValidityTo IS NULL "
        sSQL += " WHERE PY.PaymentUUID = @PaymentUUID"
        If ePayment.Legacy = False Then
            sSQL += " AND PD.ValidityTo IS NULL"
        End If

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PaymentUUID", SqlDbType.UniqueIdentifier, Guid.Parse(ePayment.PaymentUUID.ToString()))
        data.params("@dtPaymentStatus", ePayment.dtPaymentStatus, "xPayementStatus")
        Return data.Filldata
    End Function
    Public Function LoadPaymentDetails(ByVal ePayment As IMIS_EN.tblPayment, ByVal PaymentDetailsID As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = " SELECT PD.PaymentDetailsID, PD.PaymentID, PD.InsuranceNumber, PD.ProductCode, PD.PolicyStage, PD.Amount, PY.MatchedDate, PD.ValidityFrom, PD.ValidityTo, PY.OfficerCode, PY.ReceivedAmount, PY.ReceivedDate, PS.PaymenyStatusName, L.LocationId , L.LocationType, L.ParentLocationId, PR.ProdID,PY.PaymentStatus FROM tblPaymentDetails PD "
        sSQL += " INNER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID"
        sSQL += " LEFT OUTER JOIN (SELECT ProdID, LocationId,ProductCode FROM tblProduct WHERE  ValidityTo IS NULL)  PR ON PD.ProductCode = PR.ProductCode  "
        sSQL += " LEFT OUTER JOIN tblLocations L ON L.LocationId = PR.LocationId"
        sSQL += " LEFT OUTER JOIN @dtPaymentStatus PS ON PY.PaymentStatus = PS.StatusID"
        sSQL += " WHERE PD.PaymentDetailsID = @PaymentDetailsID"
        sSQL += " AND L.ValidityTo Is NULL "
        If ePayment.Legacy = False Then
            sSQL += " AND PD.ValidityTo IS NULL"
        End If

        ' sSQL += " AND PD.ValidityTo IS NULL	"
        sSQL += " And PY.ValidityTo Is NULL"
        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PaymentDetailsID", SqlDbType.Int, PaymentDetailsID)
        data.params("@dtPaymentStatus", ePayment.dtPaymentStatus, "xPayementStatus")

        Return data.Filldata
    End Function
    Public Function MatchedPayment(ByVal PaymentID As Integer) As DataTable
        Dim data As New ExactSQL
        Dim sSQL As String = ""
        sSQL = "uspMatchPayment"
        data.setSQLCommand(sSQL, CommandType.StoredProcedure)
        data.params("@PaymentID", SqlDbType.Int, PaymentID)
        Dim ds As New DataSet
        Dim dt As New DataTable
        ds = data.FilldataSet()
        dt = ds.Tables(ds.Tables.Count - 1)
        Return dt
    End Function
    Public Sub SaveEditedPaymentDetails(ePaymentDetails As IMIS_EN.tblPaymentDetail)
        Dim SSQL As String = "INSERT INTO [dbo].[tblPaymentDetails] ([PaymentID],[ProductCode] ,[InsuranceNumber],[PolicyStage] ,[Amount],[LegacyID],[ValidityFrom],[ValidityTo] ,[PremiumID],[AuditedUserId])"
        SSQL += " SELECT [PaymentID],[ProductCode] ,[InsuranceNumber],[PolicyStage] ,[Amount],[PaymentDetailsID],[ValidityFrom],getdate() ,[PremiumID],[AuditedUserId]"
        SSQL += " FROM  [tblPaymentDetails] WHERE PaymentDetailsID = @PaymentDetailsID"
        SSQL += " UPDATE [dbo].[tblPaymentDetails]"
        SSQL += " SET [ProductCode] = @ProductCode ,[InsuranceNumber]= @InsuranceNumber ,[PolicyStage] = @PolicyStage ,[ValidityFrom] = GETDATE(),[AuditedUserId] = @AuditedUserId"
        SSQL += " WHERE [PaymentDetailsID] = @PaymentDetailsID"
        data.setSQLCommand(SSQL, CommandType.Text)

        data.params("@InsuranceNumber", SqlDbType.NVarChar, 12, ePaymentDetails.InsuranceNumber)
        data.params("@ProductCode", SqlDbType.NVarChar, 8, ePaymentDetails.ProductCode)
        data.params("@PolicyStage", SqlDbType.NVarChar, 1, ePaymentDetails.PolicyStage)
        data.params("@PaymentDetailsID", SqlDbType.Int, ePaymentDetails.PaymentDetailID)
        data.params("@AuditedUserId", SqlDbType.Int, ePaymentDetails.AuditUserID)
        data.ExecuteCommand()

    End Sub
    Public Function GetPaymentIdByUUID(ByVal uuid As Guid) As DataTable
        Dim sSQL As String = ""
        Dim data As New ExactSQL

        sSQL = "select PaymentID from tblPayment where PaymentUUID = @PaymentUUID"

        data.setSQLCommand(sSQL, CommandType.Text)
        data.params("@PaymentUUID", SqlDbType.UniqueIdentifier, uuid)

        Return data.Filldata
    End Function
End Class

