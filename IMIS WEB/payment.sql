 SELECT * FROM ( 
 SELECT
  ROW_NUMBER() OVER(PARTITION BY PY.PaymentID ORDER BY PY.PaymentID DESC ) RN, PD.Amount MatchedAmount, PY.PaymentID, CN.ControlNumber, PY.TransactionNo,  PY.MatchedDate MatchingDate, CASE PY.PaymentStatus WHEN 5 THEN PY.ReceiptNo ELSE NULL END AS ReceiptNo, CASE PY.PaymentStatus WHEN 5 THEN PY.ReceivedAmount ELSE NULL END AS ReceivedAmount, PY.PaymentDate,PY.ReceivedDate, PY.PaymentOrigin, PY.OfficerCode ,PR.Receipt,PD.InsuranceNumber,PD.ProductCode 
 FROM tblPayment PY
 -- LEFT OUTER JOIN tblPaymentDetails PD ON PD.PaymentID = PY.PaymentID 
 LEFT OUTER JOIN tblControlNumber CN ON CN.PaymentID = PY.PaymentID 
 LEFT OUTER JOIN tblpremium PR ON PD.PremiumID = PR.PremiumID AND PR.ValidityTo IS NULL 
 WHERE PY.ValidityTo IS NULL AND PD.ValidityTo IS NULL AND CN.ValidityTo IS NULL 
 --AND (PD.ProductCode = @ProductCode OR @ProductCode IS NULL) AND (PY.PaymentDate BETWEEN CAST(@FromDate AS DATE) AND CAST(@ToDate AS --DATE)) 
 ) XX 
 
 WHERE XX.RN = 1