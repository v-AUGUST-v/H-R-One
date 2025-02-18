
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='1.14.377' 
BEGIN

	DELETE FROM EmpHierarchy
	WHERE EmpPosID not in (SELECT EmpPosID FROM EmpPositionInfo)

	DELETE FROM EmpCostCenterDetail
	WHERE EmpCostCenterID not in (SELECT EmpCostCenterID FROM EmpCostCenter)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('SYS010','Employment Type Code Setup', 'System', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('SYS011','Cessation Reason Code Setup', 'System', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('SYS012','Position Code Setup', 'System', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('SYS013','Qualification Code Setup', 'System', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('SYS014', 'Skill Code Setup', 'System', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('SYS015', 'Skill Level Code Setup', 'System', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('SYS016', 'Rank Code Setup', 'System', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('SYS017', 'Staff Type Code Setup', 'System', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('SYS018', 'Permit Type Code Setup', 'System', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('SYS019', 'Document Type Code Setup', 'System', 0)

	INSERT INTO SystemFunction
    (	FunctionCode
       ,Description
       ,FunctionCategory
       ,FunctionIsHidden)
     VALUES
           ('CST000', 'Cost Center Code Setup', 'Cost Center', 0)
	
	UPDATE SystemFunction 
	SET FunctionIsHidden=-1
	WHERE FunctionCode='SYS005'
	
	INSERT INTO UserGroupFunction (UserGroupID, FunctionID, FunctionAllowRead, FunctionAllowWrite)
	SELECT ugf.UserGroupID, sf.FunctionID, ugf.FunctionAllowRead, ugf.FunctionAllowWrite
	FROM UserGroupFunction ugf, SystemFunction sf
	WHERE ugf.FunctionID=
	(
		SELECT FunctionID 
		FROM SystemFunction 
		WHERE FunctionCode='SYS005'
	)
	AND sf.FunctionID IN
	(
		SELECT FunctionID 
		FROM SystemFunction 
		WHERE FunctionCode 
		IN ('SYS010', 'SYS011', 'SYS012', 'SYS013', 'SYS014', 'SYS015', 'SYS016', 'SYS017', 'SYS018', 'SYS019', 'CST000')
	)
   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='1.14.378'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





