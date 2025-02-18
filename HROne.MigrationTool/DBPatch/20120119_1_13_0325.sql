
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.13.0324' 
BEGIN

	ALTER TABLE UserGroup ALTER COLUMN 
		UserGroupName NVARCHAR(100) NULL

	ALTER TABLE UserGroup ALTER COLUMN 
		UserGroupDesc NVARCHAR(255) NULL

	ALTER TABLE EmpPayroll ADD 
		EmpPayRemark NTEXT NULL

   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.13.0325'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





