
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.3.581' 
BEGIN
	
	ALTER TABLE LeaveType ADD 
		LeaveTypeIsDisabled INT NULL

	CREATE TABLE TextTransformation
	(
		TextTransformationID INT NOT NULL IDENTITY (1, 1),
		TextTransformationOriginalString NVARCHAR(255) NULL,
		TextTransformationReplacedTo NVARCHAR(255) NULL,
		CONSTRAINT PK_TextTransformation PRIMARY KEY CLUSTERED 
		(
			TextTransformationID
		) 		
	)

   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.3.582'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





