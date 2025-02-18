
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.09.0226' 
BEGIN

	ALTER TABLE PayrollGroup ADD
		PayGroupDefaultProrataFormula int NULL

	INSERT INTO PayrollProrataFormula (PayFormCode, PayFormDesc, PayFormIsSys)
	VALUES ('<DEFAULT>', '<Use Default Formula>', 'Y')
	
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.09.0227'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





