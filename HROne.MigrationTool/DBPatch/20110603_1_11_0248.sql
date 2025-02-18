
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.11.0247' 
BEGIN

		
	ALTER Table EmpPayroll ADD
		EmpPayIsAdditionalRemuneration NVARCHAR(1) NULL
		
	ALTER Table PayrollGroup ADD
		PayGroupTerminatedALCompensationByEEPaymentCodeID  REAL NULL

	UPDATE PayrollProrataFormula
		SET PayFormDesc='Payment within Payroll Cycle / (Working days within Payroll Cycle)'
		WHERE PayFormCode='SYS004'
		
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.11.0248'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





