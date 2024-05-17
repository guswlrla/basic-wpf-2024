CREATE TABLE [dbo].[JobInformation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NULL,
	[RecruitAgencyName] [nvarchar](100) NULL,
	[RecruitAgencyType] [nvarchar](100) NULL,
	[MngDept] [nvarchar](100) NULL,
	[MngName] [nvarchar](100) NULL,
	[Field] [nvarchar](100) NULL,
	[WorkDate_Type] [nvarchar](100) NULL,
	[WorkDate_Nm] [nvarchar](100) NULL,
	[WorkRegiontxt] [nvarchar](100) NULL,
	[ReqDate_s] [nvarchar](100) NULL,
	[ReqDate_e] [nvarchar](100) NULL,
	[ReqType] [nvarchar](100) NULL,
	[ReqType_Nm] [nvarchar](100) NULL,
	[RegDate] [nvarchar](100) NULL,
	[ModDate] [nvarchar](100) NULL,
 CONSTRAINT [PK_JobInformation] PRIMARY KEY
 ( 	[Id] ASC ) ON [PRIMARY]
) ON [PRIMARY]
GO