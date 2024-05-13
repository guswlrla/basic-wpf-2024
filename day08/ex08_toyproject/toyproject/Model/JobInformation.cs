namespace toyproject.Model
{
    public class JobInformation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string RecruitAgencyName { get; set; }
        public string RecruitAgencyType { get; set; }
        public string MngDept { get; set; }
        public string MngName { get; set; }
        public string Field { get; set; }
        public string WorkDate_Type { get; set; }
        public string WorkDate_Nm { get; set; }
        public string WorkRegiontxt { get; set; }
        public string ReqDate_s { get; set; }
        public string ReqDate_e { get; set; }
        public string ReqType { get; set; }
        public string ReqType_Nm { get; set; }
        public string RegDate { get; set; }
        public string ModDate { get; set; }

        public static readonly string INSERT_QUERY = @"INSERT INTO [dbo].[JobInformation]
                                                                  ([Title]
                                                                  ,[RecruitAgencyName]
                                                                  ,[RecruitAgencyType]
                                                                  ,[MngDept]
                                                                  ,[MngName]
                                                                  ,[Field]
                                                                  ,[WorkDate_Type]
                                                                  ,[WorkDate_Nm]
                                                                  ,[WorkRegiontxt]
                                                                  ,[ReqDate_s]
                                                                  ,[ReqDate_e]
                                                                  ,[ReqType]
                                                                  ,[ReqType_Nm]
                                                                  ,[RegDate]
                                                                  ,[ModDate])
                                                             VALUES
                                                                  (@Title
                                                                  ,@RecruitAgencyName
                                                                  ,@RecruitAgencyType
                                                                  ,@MngDept
                                                                  ,@MngName
                                                                  ,@Field
                                                                  ,@WorkDate_Type
                                                                  ,@WorkDate_Nm
                                                                  ,@WorkRegiontxt
                                                                  ,@ReqDate_s
                                                                  ,@ReqDate_e
                                                                  ,@ReqType
                                                                  ,@ReqType_Nm
                                                                  ,@RegDate
                                                                  ,@ModDate)";
        public static readonly string SELECT_QUERY = @"SELECT [Id]
                                                             ,[Title]
                                                             ,[RecruitAgencyName]
                                                             ,[RecruitAgencyType]
                                                             ,[MngDept]
                                                             ,[MngName]
                                                             ,[Field]
                                                             ,[WorkDate_Type]
                                                             ,[WorkDate_Nm]
                                                             ,[WorkRegiontxt]
                                                             ,[ReqDate_s]
                                                             ,[ReqDate_e]
                                                             ,[ReqType]
                                                             ,[ReqType_Nm]
                                                             ,[RegDate]
                                                             ,[ModDate]
                                                         FROM [dbo].[JobInformation]
                                                          WHERE CONVERT(CHAR(10), RegDate, 23) = @RegDate";
        public static readonly string GETDATE_QUERY = @"SELECT CONVERT(CHAR(10), RegDate, 23) AS Save_Date
                                                          FROM [dbo].[JobInformation]
                                                         GROUP BY CONVERT(CHAR(10), RegDate, 23)";
    }
}
