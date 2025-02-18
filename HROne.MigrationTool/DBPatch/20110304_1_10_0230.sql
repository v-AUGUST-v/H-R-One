
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.10.0229' 
BEGIN

	ALTER TABLE EmpFinalPayment ADD
		EmpFinalPayIsRestDayPayment int NULL

	ALTER TABLE EmpPersonalInfo ADD
		EmpIsOverrideMinimumWage int NULL,
		EmpOverrideMinimumHourlyRate REAL NULL

	DELETE FROM PaymentCode
		WHERE PaymentCode = 'ADDITIONAL_REMUNERATION'
		
	Insert into PaymentCode 
		(PaymentCode, PaymentCodeDesc, PaymentTypeID, PaymentCodeIsProrata, PaymentCodeIsProrataLeave, PaymentCodeIsMPF, PaymentCodeIsTopUp, PaymentCodeIsWages, PaymentCodeIsORSO, PaymentCodeDecimalPlace, PaymentCodeRoundingRule, PaymentCodeHideInPaySlip, PaymentCodeDisplaySeqNo, PaymentCodeNotRemoveContributionFromTopUp)
	Select 'ADD_REMUNERATION','Additional Remuneration', PaymentTypeID, 0, 0, 1, 0, 1, 0, 2, 'ROUNDTO', 0, 70, 0
		From PaymentType where PaymentTypeCode='OTHERS'

	UPDATE PayrollGroup
	SET PayGroupAdditionalRemunerationPayCodeID=(Select PaymentCodeID FROM PaymentCode where PaymentCode='ADD_REMUNERATION')

	Insert INTO MinimumWage (MinimumWageEffectiveDate,MinimumWageHourlyRate)
	Values ('2011-05-01', 28)
           
	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('PAY014','Statutory Minimum Wage Setup', 'Payroll', 0)

   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.10.0230'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





