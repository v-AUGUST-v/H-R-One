
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.3.580' 
BEGIN
	
	ALTER TABLE LeaveBalanceAdjustment ADD 
		RecordCreatedDateTime DATETIME NULL,
		RecordCreatedBy INT NULL,
		RecordLastModifiedDateTime DATETIME NULL,
		RecordLastModifiedBy INT NULL,
		SynID NVARCHAR(255) NULL

	ALTER TABLE CompensationLeaveEntitle ADD 
		RecordCreatedDateTime DATETIME NULL,
		RecordCreatedBy INT NULL,
		RecordLastModifiedDateTime DATETIME NULL,
		RecordLastModifiedBy INT NULL,
		SynID NVARCHAR(255) NULL

	Insert into SystemFunction
           (FunctionCode, Description, FunctionCategory, FunctionIsHidden)
     	VALUES
           ('LEV006', 'Import Compensaion Leave Entitlement','Leave', 0)

   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.3.581'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





