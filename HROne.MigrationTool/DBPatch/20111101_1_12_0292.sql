
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.12.0289' 
BEGIN

	ALTER TABLE Users ADD 
		UsersCannotCreateUsersWithMorePermission INT NULL 

	ALTER TABLE LeaveCode Add
		LeaveCodeUseAllowancePaymentCodeIfSameAmount INT NULL
				
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.12.0292'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





