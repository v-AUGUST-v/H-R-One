
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.1.486' 
BEGIN
	
	DROP TABLE PayrollGroupLeaveCodeSetupOverride
	
	CREATE TABLE PayrollGroupLeaveCodeSetupOverride
	(
		PayrollGroupLeaveCodeSetupOverrideID INT NOT NULL IDENTITY (1, 1),
		PayGroupID INT NULL,
		LeaveCodeID INT NULL,
		PayrollGroupLeaveCodeSetupLeaveAllowPaymentCodeID INT NULL,
		PayrollGroupLeaveCodeSetupLeaveAllowFormula INT NULL,
		PayrollGroupLeaveCodeSetupLeaveDeductPaymentCodeID INT NULL,
		PayrollGroupLeaveCodeSetupLeaveDeductFormula INT NULL,
		CONSTRAINT PK_PayrollGroupLeaveCodeSetupOverride PRIMARY KEY CLUSTERED 
		(
			PayrollGroupLeaveCodeSetupOverrideID
		) 
	)
	           
	UPDATE PayrollGroup
	SET PayGroupDefaultNextStartDay = PayGroupDefaultEndDay + 1
	WHERE PayGroupDefaultEndDay>=1 AND PayGroupDefaultEndDay<=31
	AND PayGroupDefaultNextStartDay IS NULL
	
	UPDATE PayrollGroup
	SET PayGroupDefaultNextStartDay = PayGroupDefaultNextStartDay - 31
	WHERE PayGroupDefaultNextStartDay>31
	
	UPDATE PayrollGroup
	SET PayGroupLeaveDefaultCutOffDay = PayGroupDefaultStartDay-1
	WHERE PayGroupLeaveDefaultCutOffDay IS NULL

	UPDATE PayrollGroup
	SET PayGroupLeaveDefaultCutOffDay = PayGroupLeaveDefaultCutOffDay + 31
	WHERE PayGroupLeaveDefaultCutOffDay<=0

	UPDATE PayrollGroup
	SET PayGroupLeaveDefaultNextCutOffDay = PayGroupDefaultNextStartDay-1
	WHERE PayGroupLeaveDefaultNextCutOffDay IS NULL

	UPDATE PayrollGroup
	SET PayGroupLeaveDefaultNextCutOffDay = PayGroupLeaveDefaultNextCutOffDay + 31
	WHERE PayGroupLeaveDefaultNextCutOffDay<=0

   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.1.487'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





