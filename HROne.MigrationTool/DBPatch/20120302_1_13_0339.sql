
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.13.0336' 
BEGIN

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('RPT112','New Join Employee List', 'Personnel Reports', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('RPT113','Employee Probation List', 'Personnel Reports', 0)


	ALTER TABLE LeaveCode ADD
		LeaveCodeIsCNDProrata INT NULL
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.13.0339'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





