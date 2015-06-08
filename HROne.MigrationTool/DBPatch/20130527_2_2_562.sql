
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.2.561' 
BEGIN

	ALTER TABLE PaymentCode ADD 
		PaymentCodeIsProrataStatutoryHoliday INT NULL
		
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.2.562'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





