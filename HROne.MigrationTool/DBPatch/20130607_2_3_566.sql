
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.2.563' OR @DBVERSION='2.2.565.3'
BEGIN

	UPDATE LeavePlanBroughtForward
	SET LeavePlanBroughtForwardNumOfMonthExpired=9999
	WHERE LeavePlanBroughtForwardNumOfMonthExpired IS NULL

	ALTER TABLE EmpPositionInfo ADD
		EmpPosIsLeavePlanResetEffectiveDate INT NULL


   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.3.566'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





