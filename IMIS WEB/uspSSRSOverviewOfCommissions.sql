IF NOT OBJECT_ID('uspSSRSOverviewOfCommissionsTEST') IS NULL
DROP PROCEDURE uspSSRSOverviewOfCommissionsTEST

GO
CREATE PROCEDURE uspSSRSOverviewOfCommissionsTEST
(
    @Month INT,
    @Year INT, 
	@Mode INT=NULL,
	@OfficerId INT =NULL,
    @LocationId INT, 
	@ProdId INT = NULL,
	@PayerId INT = NULL,
	@ReportingId INT = NULL,
	@CommissionRate DECIMAL(18,2) = NULL,
	@ErrorMessage NVARCHAR(200) = N'' OUTPUT
)
AS
BEGIN

   --   ReportType
	  --1 = OverviewCommissions report
	  --2 = MatchingFund report
	  


      DECLARE @RecordFound INT = 0

	--Create new entries only if reportingId is not provided

	  IF @ReportingId IS NULL

        BEGIN

	    IF @LocationId IS NULL RETURN 1;
		IF @ProdId IS NULL RETURN 2;
		--IF @StartDate IS NULL RETURN 3;
		--IF @EndDate IS NULL RETURN 4;
	
		
		BEGIN TRY
				BEGIN TRAN

			  
				DECLARE @FirstDay DATE = CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01'; 
	            DECLARE @LastDay DATE = EOMONTH(CAST(@Year AS VARCHAR(4)) + '-' + CAST(@Month AS VARCHAR(2)) + '-01', 0)
			    INSERT INTO tblReporting(ReportingDate,LocationId, ProdId, PayerId, StartDate, EndDate, RecordFound,OfficerID,ReportType)
			
				SELECT GETDATE(),@LocationId, @ProdId, @PayerId, @FirstDay, @LastDay, 0,@OfficerId,2;

				--Get the last inserted reporting Id
				SELECT @ReportingId =  SCOPE_IDENTITY();

	
				

				UPDATE tblPremium SET ReportingCommisionID = @ReportingId
				WHERE PremiumId IN (
                SELECT Pr.PremiumId
				FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID
				INNER JOIN tblOfficer O ON O.OfficerID = PL.OfficerID
				INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
				INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
				INNER JOIN tblVillages V ON V.VillageId = F.LocationId
				INNER JOIN tblWards W ON W.WardId = V.WardId
				INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
				LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
				left join tblReporting ON PR.ReportingCommisionID =tblReporting.ReportingId AND tblReporting.ReportType=2

				WHERE Pr.ValidityTo IS NULL 
				AND PL.ValidityTo IS NULL
				AND Prod.ValidityTo IS NULL
				AND F.ValidityTo IS NULL
				AND D.ValidityTo IS NULL
				AND W.ValidityTo IS NULL
				AND V.ValidityTo IS NULL
				AND Payer.ValidityTo IS NULL
				
				AND D.DistrictID = 33
				AND PayDate BETWEEN '2012-01-01' AND '2019-02-12'
				AND Prod.ProdID = 6
				AND (ISNULL(O.OfficerID,0) = ISNULL(1,0) OR 1 IS NULL)
				AND (ISNULL(Payer.PayerID,0) = ISNULL(1,0) OR 1 IS NULL)
				AND tblReporting.ReportingId IS NULL


				AND PR.PayType <> N'F'
				)

				SELECT @RecordFound = @@ROWCOUNT;

				UPDATE tblReporting SET RecordFound = @RecordFound WHERE ReportingId = @ReportingId;

			COMMIT TRAN;
		END TRY
		BEGIN CATCH
			SELECT @ErrorMessage = ERROR_MESSAGE();
			ROLLBACK;
			RETURN -1
		END CATCH
	  END
	       
		
				--IF @Mode =0
				--BEGIN
		        
				--	SELECT Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName Product,  PL.PolicyID, F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName,O.Code + ' ' + O.LastName Officer,
				--	Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount,Pr.Amount as TotlaPrescribedContribution, Payer.PayerName,PY.PaymentDate
				--	FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID 
				--	INNER JOIN tblPaymentDetails PD ON PD.PremiumID = Pr.PremiumId
				--	INNER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID
				--	INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
				--	INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
				--	INNER JOIN tblVillages V ON V.VillageId = F.LocationId
				--	INNER JOIN tblWards W ON W.WardId = V.WardId
				--	INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
				--	INNER JOIN tblOfficer O ON O.LocationId = D.DistrictId
				--	INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
				--	LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
				--	WHERE Pr.PayDate BETWEEN @FirstDay AND @LastDay
				--	AND PL.PolicyStatus=1 OR PL.PolicyStatus=2
				--	--WHERE   (SELECT TOP 1 Amount FROM tblPremium) =

				--	GROUP BY Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName , PL.PolicyID ,  F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames ,O.Code + ' ' + O.LastName ,
				--	Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,Pr.Amount,Pr.Amount, Payer.PayerName,PY.PaymentDate

				--END
				--IF @Mode = 1

				--BEGIN
				--	SELECT TOP 1 Amount FROM tblPremium WHERE PolicyID = 1 ORDER BY PremiumId DESC
				--	SELECT Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName Product,  PL.PolicyID, F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName,O.Code + ' ' + O.LastName Officer,
				--	Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount,Pr.Amount as TotlaPrescribedContribution, Payer.PayerName,PY.PaymentDate
				--	FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID 
				--	INNER JOIN tblPaymentDetails PD ON PD.PremiumID = Pr.PremiumId
				--	INNER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID
				--	INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
				--	INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
				--	INNER JOIN tblVillages V ON V.VillageId = F.LocationId
				--	INNER JOIN tblWards W ON W.WardId = V.WardId
				--	INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
				--	INNER JOIN tblOfficer O ON O.LocationId = D.DistrictId
				--	INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
				--	LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
				--	WHERE PD.Amount >=  (SELECT TOP 1 Amount FROM tblPaymentDetails)
				--	AND Pr.PayDate BETWEEN @FirstDay AND @LastDay
				--	AND PL.PolicyStatus=1 OR PL.PolicyStatus=2
				--	GROUP BY Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName , PL.PolicyID ,  F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames ,O.Code + ' ' + O.LastName ,
				--	Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,Pr.Amount,Pr.Amount, Payer.PayerName,PY.PaymentDate
				--END
			    
				 
				--IF @Mode = NULL

				--BEGIN
				
					SELECT Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName Product,  PL.PolicyID, F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames InsName,O.Code + ' ' + O.LastName Officer,
					Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,CASE WHEN Ins.IsHead = 1 THEN Pr.Amount ELSE 0 END Amount,Pr.Amount as TotlaPrescribedContribution, Payer.PayerName,PY.PaymentDate
					FROM tblPremium Pr INNER JOIN tblPolicy PL ON Pr.PolicyID = PL.PolicyID AND PL.PolicyStatus=1 OR PL.PolicyStatus=2
					LEFT OUTER JOIN tblPaymentDetails PD ON PD.PremiumID = Pr.PremiumId
					LEFT OUTER JOIN tblPayment PY ON PY.PaymentID = PD.PaymentID
					INNER JOIN tblProduct Prod ON PL.ProdID = Prod.ProdID
					INNER JOIN tblFamilies F ON PL.FamilyID = F.FamilyID
					INNER JOIN tblVillages V ON V.VillageId = F.LocationId
					INNER JOIN tblWards W ON W.WardId = V.WardId
					INNER JOIN tblDistricts D ON D.DistrictId = W.DistrictId
					INNER JOIN tblOfficer O ON O.LocationId = D.DistrictId
					INNER JOIN tblInsuree Ins ON F.FamilyID = Ins.FamilyID  AND Ins.ValidityTo IS NULL
					LEFT OUTER JOIN tblPayer Payer ON Pr.PayerId = Payer.PayerID 
			        WHERE Pr.ReportingCommisionID = @ReportingId
	                
					GROUP BY Pr.PremiumId,Prod.ProductCode,Prod.ProdID,Prod.ProductName,prod.ProductCode +' ' + prod.ProductName , PL.PolicyID ,  F.FamilyID, D.DistrictName,o.OfficerID , Ins.CHFID, Ins.LastName + ' ' + Ins.OtherNames ,O.Code + ' ' + O.LastName ,
					Ins.DOB, Ins.IsHead, PL.EnrollDate, Pr.Paydate, Pr.Receipt,Pr.Amount,Pr.Amount, Payer.PayerName,PY.PaymentDate
					--ORDER BY PremiumId DESC, IsHead DESC;
				--END

			    
           
     RETURN 0
END

GO