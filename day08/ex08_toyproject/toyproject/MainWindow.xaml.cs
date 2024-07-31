using System.Data;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using toyproject.Model;

namespace toyproject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitComboDataFromDB();
        }

        private void InitComboDataFromDB()
        {
           using (SqlConnection conn = new SqlConnection(Helper.Common.CONNSTRING))
           {
                conn.Open();
                SqlCommand cmd = new SqlCommand(Model.JobInformation.GETDATE_QUERY, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                List<string> saveDates = new List<string>();

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    saveDates.Add(Convert.ToString(row["Save_Date"]));
                }

                CboReqDate.ItemsSource = saveDates;
            }
        }

        // 조회버튼
        private async void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            string openApiUri = "https://apis.data.go.kr/6260000/BusanJobOpnngInfoService/getJobOpnngInfo?serviceKey=nC6FFppZywZlFw%2B2XlRYzqZukBFja5Hd1UCzNjLKF3iMA94%2BF13honnETdW5iO%2Be7aHyL%2B12yma4aKnWhXqm6g%3D%3D&pageNo=1&numOfRows=100&resultType=json";
            string result = string.Empty;

            WebRequest request = null;
            WebResponse response = null;
            StreamReader reader = null;

            try
            {
                request = WebRequest.Create(openApiUri);
                response = await request.GetResponseAsync();
                reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("오류", $"OpenAPI 조회오류 {ex.Message}");
            }

            var jsonResult = JObject.Parse(result); 
            var header = jsonResult["getJobOpnngInfo"]["header"];
            var resultCode = Convert.ToInt32(header["resultCode"]);

            if (resultCode == 00)
            {
                var data = jsonResult["getJobOpnngInfo"]["body"]["items"]["item"];
                var jsonArray = data as JArray;

                var jobInfo = new List<JobInformation>();
                foreach (var item in jsonArray)
                {
                    jobInfo.Add(new JobInformation()
                    {
                        Id = 0,
                        Title = Convert.ToString(item["title"]),
                        RecruitAgencyName = Convert.ToString(item["recruitAgencyName"]),
                        RecruitAgencyType = Convert.ToString(item["recruitAgencyType"]),
                        MngDept = Convert.ToString(item["mngDept"]),
                        MngName = Convert.ToString(item["mngName"]),
                        Field = Convert.ToString(item["bunya"]),
                        WorkDate_Type = Convert.ToString(item["workDate_type"]),
                        WorkDate_Nm = Convert.ToString(item["workDate_nm"]),
                        WorkRegiontxt = Convert.ToString(item["workregiontxt"]),
                        ReqDate_s = Convert.ToString(item["reqDate_s"]),
                        ReqDate_e = Convert.ToString(item["reqDate_e"]),
                        ReqType = Convert.ToString(item["reqType"]),
                        ReqType_Nm = Convert.ToString(item["reqType_nm"]),
                        RegDate = Convert.ToString(item["regDate"]),
                        ModDate = Convert.ToString(item["modDate"]),
                    });
                }

                this.DataContext = jobInfo;
                StsResult.Content = $"OpenAPI {jobInfo.Count}건 조회완료!";
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (GrdResult.Items.Count == 0)
            {
                await this.ShowMessageAsync("저장오류", "실시간 조회후 저장하십시오.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(Helper.Common.CONNSTRING))
                {
                    conn.Open();

                    var insRes = 0;
                    foreach (JobInformation item in GrdResult.SelectedItems)
                    {
                        SqlCommand cmd = new SqlCommand(Model.JobInformation.INSERT_QUERY, conn);
                        cmd.Parameters.AddWithValue("@Title", item.Title);
                        cmd.Parameters.AddWithValue("@RecruitAgencyName", item.RecruitAgencyName);
                        cmd.Parameters.AddWithValue("@RecruitAgencyType", item.RecruitAgencyType);
                        cmd.Parameters.AddWithValue("@MngDept", item.MngDept);
                        cmd.Parameters.AddWithValue("@MngName", item.MngName);
                        cmd.Parameters.AddWithValue("@Field", item.Field);
                        cmd.Parameters.AddWithValue("@WorkDate_Type", item.WorkDate_Type);
                        cmd.Parameters.AddWithValue("@WorkDate_Nm", item.WorkDate_Nm);
                        cmd.Parameters.AddWithValue("@WorkRegiontxt", item.WorkRegiontxt);
                        cmd.Parameters.AddWithValue("@ReqDate_s", item.ReqDate_s);
                        cmd.Parameters.AddWithValue("@ReqDate_e", item.ReqDate_e);
                        cmd.Parameters.AddWithValue("@ReqType", item.ReqType);
                        cmd.Parameters.AddWithValue("@ReqType_Nm", item.ReqType_Nm);
                        cmd.Parameters.AddWithValue("@RegDate", item.RegDate);
                        cmd.Parameters.AddWithValue("@ModDate", item.ModDate);

                        insRes += cmd.ExecuteNonQuery();
                    }

                    if (insRes > 0)
                    {
                        await this.ShowMessageAsync("저장", "DB저장성공!");
                        StsResult.Content = $"DB저장 {insRes}건 성공!";
                    }
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("저장오류", $"저장오류 {ex.Message}");
            }

            InitComboDataFromDB();
        }

        private void CboReqDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CboReqDate.SelectedValue != null)
            {
                using (SqlConnection conn = new SqlConnection(Helper.Common.CONNSTRING))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(Model.JobInformation.SELECT_QUERY, conn);
                    cmd.Parameters.AddWithValue("@RegDate", CboReqDate.SelectedValue.ToString());
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "JobInformation");
                    var jobInfo = new List<JobInformation>();

                    foreach (DataRow row in dataSet.Tables["JobInformation"].Rows)
                    {
                        jobInfo.Add(new JobInformation
                        {
                            Id = Convert.ToInt32(row["Id"]),
                            Title = Convert.ToString(row["Title"]),
                            RecruitAgencyName = Convert.ToString(row["RecruitAgencyName"]),
                            RecruitAgencyType = Convert.ToString(row["RecruitAgencyType"]),
                            MngDept = Convert.ToString(row["MngDept"]),
                            MngName = Convert.ToString(row["MngName"]),
                            Field = Convert.ToString(row["Field"]),
                            WorkDate_Type = Convert.ToString(row["WorkDate_type"]),
                            WorkDate_Nm = Convert.ToString(row["WorkDate_nm"]),
                            WorkRegiontxt = Convert.ToString(row["Workregiontxt"]),
                            ReqDate_s = Convert.ToString(row["ReqDate_s"]),
                            ReqDate_e = Convert.ToString(row["ReqDate_e"]),
                            ReqType = Convert.ToString(row["ReqType"]),
                            ReqType_Nm = Convert.ToString(row["ReqType_nm"]),
                            RegDate = Convert.ToString(row["RegDate"]),
                            ModDate = Convert.ToString(row["ModDate"]),
                        });
                    }

                    this.DataContext = jobInfo;
                    StsResult.Content = $"DB {jobInfo.Count}건 조회완료";
                }
            }
            else
            {
                this.DataContext = null;
                StsResult.Content = $"DB 조회클리어";
            }
        }
    }
}