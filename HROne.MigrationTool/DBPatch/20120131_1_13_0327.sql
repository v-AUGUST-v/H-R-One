
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.13.0326' 
BEGIN

		
	UPDATE SystemFunction
		SET Description='Import Employee Information'
		WHERE FunctionCode='PER999'
		
	UPDATE SystemFunction
		SET Description='Roster Table Group Setup'
		WHERE FunctionCode='ATT014'
		
	INSERT INTO MPFScheme (MPFSchemeCode, MPFSchemeDesc)
	VALUES ('MT00555', 'HSBC Mandatory Provident Fund - ValueChoice')
		  
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.13.0327'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





