
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.3.566' 
BEGIN

	ALTER TABLE LeaveBalanceEntitle ADD
		LeaveBalanceEntitleGrantPeriodFrom DATETIME NULL,
		LeaveBalanceEntitleGrantPeriodTo DATETIME NULL
	
	CREATE TABLE RequestLeaveApplicationCancel
	(
		RequestLeaveAppCancelID INT NOT NULL IDENTITY (1, 1),
		EmpID INT NULL,
		LeaveAppID INT NULL,
		RequestLeaveAppCancelCreateDateTime DATETIME NULL,
		RequestLeaveAppCancelReason NTEXT NULL,
		CONSTRAINT PK_RequestLeaveApplicationCancel PRIMARY KEY CLUSTERED 
		(
			RequestLeaveAppCancelID
		) 
	)

	CREATE TABLE LeaveApplicationCancel
	(
		LeaveAppCancelID INT NOT NULL IDENTITY (1, 1),
		EmpID INT NULL,
		LeaveAppID INT NULL,
		LeaveAppCancelCreateDateTime DATETIME NULL,
		LeaveAppCancelReason NTEXT NULL,
		CONSTRAINT PK_LeaveApplicationCancel PRIMARY KEY CLUSTERED 
		(
			LeaveAppCancelID
		) 
	)
	
	ALTER TABLE LeaveApplication ADD
		LeaveAppCancelID INT NULL
		
		
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.3.569'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





