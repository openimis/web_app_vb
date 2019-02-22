
   IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'CHFID')
	   UPDATE tblControls SET FieldName='CHFID',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'CHFID'
	ELSE
	  INSERT INTO tblControls(FieldName,Adjustibility,Usage)
	  VALUES('CHFID','M','Search Insurance Number/Enquiry')

    IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'LastName')
	   UPDATE tblControls SET FieldName='LastName',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'LastName'
	ELSE
	  INSERT INTO tblControls(FieldName,Adjustibility,Usage)
	  VALUES('LastName','M','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'OtherNames')
	   UPDATE tblControls SET FieldName='OtherNames',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'OtherNames'
	ELSE
	  INSERT INTO tblControls(FieldName,Adjustibility,Usage)
	  VALUES('OtherNames','M','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Age')
	   UPDATE tblControls SET FieldName='Age',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Age'
	ELSE
	  INSERT INTO tblControls(FieldName,Adjustibility,Usage)
	  VALUES('Age','M','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'DOB')
		UPDATE tblControls SET FieldName='DOB',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'DOB' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('DOB','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Gender')
		UPDATE tblControls SET FieldName='Gender',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Gender' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('Gender','O','Search Insurance Number/Enquiry')

	
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'RegionOfFSP')
		UPDATE tblControls SET FieldName='RegionOfFSP',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'RegionOfFSP' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('RegionOfFSP','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'DistrictOfFSP')
		UPDATE tblControls SET FieldName='DistrictOfFSP',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'DistrictOfFSP' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('DistrictOfFSP','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'HFLevel')
		UPDATE tblControls SET FieldName='HFLevel',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'HFLevel' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('HFLevel','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'FirstServicePoint')
		UPDATE tblControls SET FieldName='FirstServicePoint',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'FirstServicePoint' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('FirstServicePoint','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'ProductCode')
		UPDATE tblControls SET FieldName='ProductCode',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'ProductCode' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('ProductCode','M','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'ExpiryDate')
		UPDATE tblControls SET FieldName='ExpiryDate',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'ExpiryDate' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('ExpiryDate','M','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'PolicyStatus')
		UPDATE tblControls SET FieldName='PolicyStatus',Adjustibility='M', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'PolicyStatus' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('PolicyStatus','M','Search Insurance Number/Enquiry')

		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Ded1')
		UPDATE tblControls SET FieldName='Ded1',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Ded1' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('Ded1','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Ded2')
		UPDATE tblControls SET FieldName='Ded2',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Ded2' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('Ded2','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Ceiling1')
		UPDATE tblControls SET FieldName='Ceiling1',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Ceiling1' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('Ceiling1','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'Ceiling2')
		UPDATE tblControls SET FieldName='Ceiling2',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'Ceiling2' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('Ceiling2','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalAdmissionsLeft')
		UPDATE tblControls SET FieldName='TotalAdmissionsLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalAdmissionsLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalAdmissionsLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalVisitsLeft')
		UPDATE tblControls SET FieldName='TotalVisitsLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalVisitsLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalVisitsLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalConsultationsLeft')
		UPDATE tblControls SET FieldName='TotalConsultationsLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalConsultationsLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalConsultationsLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalSurgeriesLeft')
		UPDATE tblControls SET FieldName='TotalSurgeriesLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalSurgeriesLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalSurgeriesLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalDelivieriesLeft')
		UPDATE tblControls SET FieldName='TotalDelivieriesLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalDelivieriesLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalDelivieriesLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'TotalAntenatalLeft')
		UPDATE tblControls SET FieldName='TotalAntenatalLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'TotalAntenatalLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('TotalAntenatalLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'HospitalizationAmountLeft')
		UPDATE tblControls SET FieldName='HospitalizationAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'HospitalizationAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('HospitalizationAmountLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'ConsultationAmountLeft')
		UPDATE tblControls SET FieldName='ConsultationAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'ConsultationAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('ConsultationAmountLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'SurgeryAmountLeft')
		UPDATE tblControls SET FieldName='SurgeryAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'SurgeryAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('SurgeryAmountLeft','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'AntenatalAmountLeft')
		UPDATE tblControls SET FieldName='AntenatalAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'AntenatalAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('AntenatalAmountLeft','O','Search Insurance Number/Enquiry')

		IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'DeliveryAmountLeft')
		UPDATE tblControls SET FieldName='DeliveryAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'DeliveryAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('DeliveryAmountLeft','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'AntenatalAmountLeft')
		UPDATE tblControls SET FieldName='AntenatalAmountLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'AntenatalAmountLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('AntenatalAmountLeft','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblItemCodeL')
		UPDATE tblControls SET FieldName='lblItemCodeL',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblItemCodeL' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblItemCodeL','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblItemCode')
		UPDATE tblControls SET FieldName='lblItemCode',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblItemCode' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblItemCode','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblItemLeftL')
		UPDATE tblControls SET FieldName='lblItemLeftL',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblItemLeftL' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblItemLeftL','O','Search Insurance Number/Enquiry')
		
	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblServiceMinDate')
		UPDATE tblControls SET FieldName='lblServiceMinDate',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblServiceMinDate' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblServiceMinDate','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblItemMinDate')
		UPDATE tblControls SET FieldName='lblItemMinDate',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblItemMinDate' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblItemMinDate','O','Search Insurance Number/Enquiry')

	IF EXISTS(SELECT * FROM tblControls WHERE FieldName = 'lblServiceLeft')
		UPDATE tblControls SET FieldName='lblServiceLeft',Adjustibility='O', Usage='Search Insurance Number/Enquiry' WHERE FieldName = 'lblServiceLeft' 
	ELSE
		INSERT INTO tblControls(FieldName,Adjustibility,Usage)
		VALUES('lblServiceLeft','O','Search Insurance Number/Enquiry')