							
--Declare variable
DECLARE @RoleID as INT		
DECLARE @Rights AS TABLE(RightID INT) 

INSERT INTO @Rights VALUES(101105)--InsureeEnquire 
INSERT INTO @Rights VALUES(121001)--ProductSearch  
INSERT INTO @Rights VALUES(121002)--ProductAdd     
INSERT INTO @Rights VALUES(121003)--ProductEdit    
INSERT INTO @Rights VALUES(121004)--ProductDelete  
INSERT INTO @Rights VALUES(121005)--ProductDuplicate  
INSERT INTO @Rights VALUES(121101)--HealthFacilitiesSearch  
INSERT INTO @Rights VALUES(121102)--HealthFacilitiesAdd   
INSERT INTO @Rights VALUES(121103)--HealthFacilitiesEdit    
INSERT INTO @Rights VALUES(121104)--HealthFacilitiesDelete   
INSERT INTO @Rights VALUES(121201)--PriceListMedicalServicesSearch   
INSERT INTO @Rights VALUES(121202)--PriceListMedicalServicesAdd      
INSERT INTO @Rights VALUES(121203)--PriceListMedicalServicesEdit     
INSERT INTO @Rights VALUES(121204)--PriceListMedicalServicesDelete   
INSERT INTO @Rights VALUES(121205)--PriceListMedicalServicesDuplicate   
INSERT INTO @Rights VALUES(121301)--PriceListMedicalItemsSearch   
INSERT INTO @Rights VALUES(121302)--PriceListMedicalItemsAdd      
INSERT INTO @Rights VALUES(121303)--PriceListMedicalItemsEdit     
INSERT INTO @Rights VALUES(121304)--PriceListMedicalItemsDelete   
INSERT INTO @Rights VALUES(121305)--PriceListMedicalItemsDuplicate   
INSERT INTO @Rights VALUES(121401)--MedicalServicesSearch  
INSERT INTO @Rights VALUES(121402)--MedicalServicesAdd     
INSERT INTO @Rights VALUES(121403)--MedicalServicesEdit    
INSERT INTO @Rights VALUES(121404)--MedicalServicesDelete   
INSERT INTO @Rights VALUES(122101)--MedicalItemsSearch   
INSERT INTO @Rights VALUES(122102)--MedicalItemsAdd     
INSERT INTO @Rights VALUES(122103)--MedicalItemsEdit     
INSERT INTO @Rights VALUES(122104)--MedicalItemsDelete   
INSERT INTO @Rights VALUES(121501)--OfficerSearch        
INSERT INTO @Rights VALUES(121502)--OfficerAdd           
INSERT INTO @Rights VALUES(121503)--OfficerEdit          
INSERT INTO @Rights VALUES(121504)--OfficerDelete   
INSERT INTO @Rights VALUES(121601)--ClaimAdministratorSearch    
INSERT INTO @Rights VALUES(121602)--ClaimAdministratorAdd       
INSERT INTO @Rights VALUES(121603)--ClaimAdministratorEdit      
INSERT INTO @Rights VALUES(121604)--ClaimAdministratorDelete   
INSERT INTO @Rights VALUES(121801)--PayersSearch      
INSERT INTO @Rights VALUES(121802)--PayersAdd         
INSERT INTO @Rights VALUES(121803)--PayersEdit        
INSERT INTO @Rights VALUES(121804)--PayersDelete    
INSERT INTO @Rights VALUES(121901)--LocationsSearch   
INSERT INTO @Rights VALUES(121902)--LocationsAdd      
INSERT INTO @Rights VALUES(121903)--LocationsEdit     
INSERT INTO @Rights VALUES(121904)--LocationsDelete  
INSERT INTO @Rights VALUES(121905)--LocationsMove  
INSERT INTO @Rights VALUES(131001)--DiagnosesUpload          
INSERT INTO @Rights VALUES(131002)--DiagnosesDownload        
INSERT INTO @Rights VALUES(131003)--HealthFacilitiesUpload   
INSERT INTO @Rights VALUES(131004)--HealthFacilitiesDownload 
INSERT INTO @Rights VALUES(131005)--LocationsUpload          
INSERT INTO @Rights VALUES(131006)--LocationsDownload   
INSERT INTO @Rights VALUES(131101)--ExtractsMasterDataDownload      
INSERT INTO @Rights VALUES(131102)--ExtractsPhoneExtractsCreate     
INSERT INTO @Rights VALUES(131103)--ExtractsOfflineExtractCreate    
INSERT INTO @Rights VALUES(131104)--ExtractsClaimsUpload            
INSERT INTO @Rights VALUES(131105)--ExtractsEnrolmentsUpload        
INSERT INTO @Rights VALUES(131106)--ExtractsFeedbackUpload  
INSERT INTO @Rights VALUES(131209)--ReportsStatusOfRegister  
SELECT @RoleID = RoleID from tblRole WHERE Rolename ='Scheme Administrator'	 
--Uncheck
DELETE FROM tblRoleRight WHERE RoleID = @RoleID AND RightID NOT IN (SELECT RightID FROM @Rights)

 --Setting value

INSERT INTO tblRoleRight (RoleID,RightID,ValidityFrom) 
SELECT @RoleID,Ro.RightID,GETDATE() FROM @Rights Ro 
		LEFT OUTER JOIN tblRoleRight Rr ON Rr.RoleID =@RoleID 
		AND Rr.RightID = Ro.RightID 
		AND Rr.ValidityTo IS NULL 
		WHERE Rr.RoleRightID IS NULL

