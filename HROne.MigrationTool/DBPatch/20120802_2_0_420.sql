
DECLARE @DBVERSION as varchar(100);
set @DBVERSION = (Select ParameterValue from SystemParameter where ParameterCode='DBVERSION');

IF @DBVERSION='2.0.419' 
BEGIN

	CREATE TABLE InboxAttachment
	(
		InboxAttachmentID INT NOT NULL IDENTITY (1, 1),
		InboxID INT NULL,
		InboxAttachmentOriginalFileName NVARCHAR(250) NULL,
		InboxAttachmentStoredFileName NVARCHAR(250) NULL,
		InboxAttachmentIsCompressed INT NULL,
		InboxAttachmentSize BIGINT NULL
		CONSTRAINT PK_InboxAttachment PRIMARY KEY CLUSTERED 
		(
			InboxAttachmentID
		) 
	)
	
	CREATE INDEX IDX_InboxAttachment_InboxID ON InboxAttachment 
	(
		InboxID
	)

   	-- Insert version of Database --
	Update SystemParameter 
	set ParameterValue='2.0.420'
	where ParameterCode='DBVERSION';
	print ('Upgrade Successfully');

END
ELSE
print ('Incorrect Version: ' + @DBVERSION);





