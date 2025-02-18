
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.08.0207' 
BEGIN

	Update EmpPayroll
	Set EmpPayIsYEB='Y'
	where EmpPayrollID in (Select Distinct EmpPayrollID from PaymentRecord where PayRecType='Y')

	Update EmpPayroll
	Set EmpPayIsCND='Y'
	where EmpPayrollID in (Select Distinct EmpPayrollID from PaymentRecord where PayRecType='C')

	Update EmpPayroll
	Set EmpPayIsCND='N'
	where EmpPayrollID NOT IN (Select Distinct EmpPayrollID from PaymentRecord where PayRecType='C')

	Alter Table LeaveType ADD
		LeaveTypeIsUseWorkHourPattern int NULL,
		LeaveTypeIsSkipStatutoryHolidayChecking int NULL,
		LeaveTypeIsSkipPublicHolidayChecking int NULL

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('PAY012','Stop Payment', 'Payroll', 0)

	

	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.08.0210'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





