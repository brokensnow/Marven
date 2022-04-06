#region Using
using REAccess.Mobile.Common.Interfaces;
using REAccess.Mobile.Common.Utils;
using REAccess.Mobile.Common.ViewModel;
using REAccess.Mobile.Database.LogModels;
using REAccess.Mobile.Database.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static REAccess.Mobile.Common.Constants;
#endregion

namespace REAccess.Mobile.Common.Services
{
    public class UtilService : BaseService, IUtilService
    {

        ///<summary>
        ///清理所有正在使用的资源
        ///</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 获取当前所有三级指标
        /// </summary>
        public List<string> GetAllIndex()
        {
            List<string> indexList = StaticCache.Indicators.Where(x => x.Class == 3 && x.Valid == DbYesOrNo.Yes).Select(x => x.Name).Distinct().ToList();

            return indexList;
        }
        /// <summary>
        /// 单项指标-指标下拉列表
        /// </summary>
        public List<string> GetMobileAllIndex()
        {
            //List<string> indexList = MobileData.AllIndexList;
            List<string> indexList = new List<string>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "指标下拉列表");
            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    indexList.Add(dataTable.Rows[i][1].ToString());
                }
            }
            indexList = indexList.Where(x => x != "新增常住人口").ToList();

            return indexList;
        }

        /// <summary>
        /// 获取城市列表
        /// </summary>
        public List<string> GetCityList()
        {
            List<string> cityList = StaticCache.Districts.OrderBy(x => x.RankingOfProvinces).Select(x => x.CityName).Distinct().ToList();

            return cityList;
        }
        
        /// <summary>
        /// 城市排名-城市下拉列表
        /// </summary>
        public List<string> GetMobileCityList()
        {
            //List<string> cityList = MobileData.AllCityList;
            List<string> cityList = new List<string>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "城市下拉列表");
            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    cityList.Add(dataTable.Rows[i][1].ToString() + "市");
                }
            }

            return cityList;
        }

        /// <summary>
        /// 获取政策类别列表
        /// </summary>
        public List<PolicyListModel> GetPolicyList()
        {
            //List<PolicyListModel> policyList = _db.DsaIndustryFieldTag.Where(x => x.Category == 1 && x.IsValid == DbYesOrNo.Yes).OrderBy(x => x.OrderNum).Select(x => new PolicyListModel() 
            //{
            //    PolicyId = x.Id,
            //    PolicyName = x.Name
            //}).ToList();

            //return policyList;
            var model = new List<PolicyListModel>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "政策下拉菜单");
            var dbTag = _db.DsaIndustryFieldTag.Where(x => x.Category == 1 && x.IsValid == DbYesOrNo.Yes).ToList();
            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    PolicyListModel policy = new PolicyListModel()
                    {
                        PolicyId = dbTag.FirstOrDefault(x => x.Name == dataTable.Rows[i][1].ToString()).Id,
                        PolicyName = dataTable.Rows[i][1].ToString(),
                        PolicyCount = dataTable.Rows[i][2].ToString().Split('条').FirstOrDefault()
                    };
                    model.Add(policy);
                }
            }

            return model;
        }

        /// <summary>
        /// 记录系统日志
        /// </summary>
        public string SysLog(string pageName, string previousPage)
        {
            string result = ResponseStatusMessage.Success;
            if (string.IsNullOrEmpty(previousPage) && string.IsNullOrEmpty(pageName))
            {
                result = ResponseStatusMessage.Failed;
            }
            

            return result;
        }

        /// <summary>
        /// 根据指标名称，获取数据库有数据的年份
        /// </summary>
        public List<string> GetYearListByIndexName(string indicator)
        {
            List<string> yearList = new List<string>();
            var indicatorRankList = StaticCache.IndicatorScore.ToList();
            string indexName = IndicatorMappingExtension.GetIndexName(indicator);
            if (!string.IsNullOrEmpty(indexName))
            {
                yearList = indicatorRankList.Where(x => x.GetType().GetProperty(indexName).GetValue(x, null) != null)
                                                      .GroupBy(x => x.SnapshotPeriod)
                                                      .Select(x => x.FirstOrDefault())
                                                      .OrderByDescending(x => x.SnapshotPeriod)
                                                      .Select(x => x.SnapshotPeriod)
                                                      .ToList();
            }
            if (!yearList.Any())
            {
                yearList = indicatorRankList.GroupBy(x => x.SnapshotPeriod).Select(x => x.First()).OrderByDescending(x => x.SnapshotPeriod).Select(x => x.SnapshotPeriod).ToList();
            }

            return yearList;

        }
        /// <summary>
        /// 读取excel数据包城市排名数据
        /// </summary>
        public List<CityRank> GetCityRankData(string cityName)
        {
            List<CityRank> cityRankList = new List<CityRank>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "城市排名data");
            if(dataTable != null)
            {
                for(var i = 0; i < dataTable.Rows.Count; i++)
                {
                    CityRank cityRank = new CityRank()
                    {
                        CityName = dataTable.Rows[i][0].ToString() + "市",
                        RankPlace = int.Parse(dataTable.Rows[i][1].ToString().Split("#").Last()),
                        IndexName = dataTable.Rows[i][2].ToString(),
                        IndexValue = ToolFunc.ThousandFormatter(dataTable.Rows[i][3].ToString()),
                        Unit = dataTable.Rows[i][4].ToString() == "-" ? "" : dataTable.Rows[i][4].ToString(),
                        Year = dataTable.Rows[i][5].ToString()
                    };
                    cityRankList.Add(cityRank);
                }
            }
            cityRankList = cityRankList.Where(x => x.CityName == cityName).ToList();

            return cityRankList;
        }
        /// <summary>
        /// 读取excel数据包政策数据
        /// </summary>
        public List<IndustrialPolicy> GetPolicyData()
        {
            List<IndustrialPolicy> policyList = new List<IndustrialPolicy>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "政策data");
            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    IndustrialPolicy policy = new IndustrialPolicy()
                    {
                        IndustryName = dataTable.Rows[i][0].ToString(),
                        FileName = dataTable.Rows[i][1].ToString(),
                        Province = dataTable.Rows[i][2].ToString(),
                        City = dataTable.Rows[i][3].ToString(),
                    };
                    policyList.Add(policy);
                }
            }

            return policyList;
        }

        /// <summary>
        /// 读取excel数据包政策数据
        /// </summary>
        public PolicyDetail GetPolicyData(string policyName)
        {
            PolicyDetail policyList = new PolicyDetail();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "政策data");
            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][1].ToString().Equals(policyName))
                    {
                        policyList = new PolicyDetail()
                        {
                            FileCategory = dataTable.Rows[i][0].ToString(),
                            FileName = dataTable.Rows[i][1].ToString(),
                            FileLevel = dataTable.Rows[i][4].ToString(),
                            IssuingAgency = dataTable.Rows[i][5].ToString(),
                            SupportAreas = dataTable.Rows[i][6].ToString(),
                            ReleaseDate = ToolFunc.DatetimeFormatter(Convert.ToDateTime(dataTable.Rows[i][7].ToString())),
                            ClosingDate = dataTable.Rows[i][8].ToString() == "未公布" ? "未公布" : ToolFunc.DatetimeFormatter(Convert.ToDateTime(dataTable.Rows[i][8].ToString()))
                        };
                    }
                }
            }

            return policyList;
        }
        /// <summary>
        /// 读取excel数据包政策数据
        /// </summary>
        public List<RankModel> GetIndusrtyRegionData()
        {
            var regionData = new List<RankModel>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "产业拿地by city,2021");
            if(dataTable != null)
            {
                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    RankModel rank = new RankModel()
                    {
                        RankPlace = int.Parse(dataTable.Rows[i][0].ToString().Split('#').Last()),
                        CityName = dataTable.Rows[i][1].ToString(),
                        ProvinceName = dataTable.Rows[i][2].ToString(),
                        RankValue = ToolFunc.ThousandFormatter(dataTable.Rows[i][3].ToString()),
                        Unit = dataTable.Rows[i][4].ToString()
                    };
                    regionData.Add(rank);
                }
            }

            return regionData;
        }
        /// <summary>
        /// 读取excel数据包产业用地-热点产业数据
        /// </summary>
        public List<CityRank> GetLandHotIndustryData()
        {
            List<CityRank> data = new List<CityRank>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "产业拿地by industry,2021");
            if(dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    CityRank rank = new CityRank()
                    {
                        RankPlace = int.Parse(dataTable.Rows[i][0].ToString().Split('#').Last()),
                        IndexName = dataTable.Rows[i][1].ToString(),
                        IndexValue = ToolFunc.ThousandFormatter(dataTable.Rows[i][2].ToString()),
                        Unit = dataTable.Rows[i][3].ToString(),
                        IndexAmount = ToolFunc.ThousandFormatter(dataTable.Rows[i][4].ToString(),1),
                        IndexAmountUnit = dataTable.Rows[i][5].ToString()
                    };
                    data.Add(rank);
                }
            }

            return data;
        }
        /// <summary>
        /// 读取excel数据包单项排名数据
        /// </summary>
        public SingleIndexModel GetSingleRankData(string selectIndex)
        {
            SingleIndexModel model = new SingleIndexModel();
            var data = new List<RankModel>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "指标data");
            if (dataTable != null)
            {
               
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if(dataTable.Rows[i][0].ToString() == selectIndex)
                    {
                        model.IndexName = dataTable.Rows[i][0].ToString();
                        model.Unit = dataTable.Rows[i][5].ToString();
                        model.Year = dataTable.Rows[i][6].ToString();
                        model.Formula = string.IsNullOrEmpty(dataTable.Rows[i][7].ToString()) ? "" : $"{selectIndex} = {dataTable.Rows[i][7]}" ;
                        RankModel rank = new RankModel()
                        {
                            RankPlace = int.Parse(dataTable.Rows[i][1].ToString().Split('#').Last()),
                            CityName = dataTable.Rows[i][2].ToString(),
                            ProvinceName = dataTable.Rows[i][3].ToString(),
                            RankValue = ToolFunc.ThousandFormatter(dataTable.Rows[i][4].ToString()),
                            Unit = dataTable.Rows[i][5].ToString() == "-" ? "" : dataTable.Rows[i][5].ToString(),
                        };
                        data.Add(rank);
                    }
                }
            }
            model.SingleIndexList = data;

            return model;
        }
        /// <summary>
        /// 读取excel数据包资讯数据
        /// </summary>
        public List<NewsModel> GetNewsData()
        {
            List<NewsModel> model = new List<NewsModel>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "资讯");
            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    NewsModel news = new NewsModel()
                    {
                        Id = int.Parse(dataTable.Rows[i][0].ToString()),
                        NewsTitle = WebUtility.HtmlEncode(dataTable.Rows[i][1].ToString()),
                        NewsImage = Path.Combine("RealTimeInfoImgs", dataTable.Rows[i][2].ToString()).Replace(".png", ".jpg") ,
                        NewsContent = dataTable.Rows[i][3].ToString(),
                        NewsReleaseDate = ToolFunc.DatetimeFormatter(Convert.ToDateTime(dataTable.Rows[i][6].ToString()))
                    };
                    var tagList = new List<string>();
                    if (!string.IsNullOrEmpty(dataTable.Rows[i][10].ToString()))
                    {
                        tagList.Add(dataTable.Rows[i][10].ToString());
                    }
                    if (!string.IsNullOrEmpty(dataTable.Rows[i][11].ToString()))
                    {
                        tagList.Add(dataTable.Rows[i][11].ToString());
                    }
                    if (!string.IsNullOrEmpty(dataTable.Rows[i][12].ToString()))
                    {
                        tagList.Add(dataTable.Rows[i][12].ToString());
                    }
                    news.NewTags = tagList;
                    model.Add(news);
                }
                model = model.OrderByDescending(x => Convert.ToDateTime(x.NewsReleaseDate)).ToList();
            }

            return model;
        }
        /// <summary>
        /// 读取excel数据包产业投资-区域数据
        /// </summary>
        public List<RankModel> GetActiveAreaData()
        {
            List<RankModel> model = new List<RankModel>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "上市企业投资by city，2021");
            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    RankModel rank = new RankModel()
                    {
                        RankPlace = int.Parse(dataTable.Rows[i][0].ToString().Split('#').Last()),
                        CityName = dataTable.Rows[i][1].ToString(),
                        ProvinceName = dataTable.Rows[i][2].ToString(),
                        RankValue = dataTable.Rows[i][3].ToString(),
                        Unit = dataTable.Rows[i][4].ToString(),
                        RankAmount = ToolFunc.ThousandFormatter(dataTable.Rows[i][5].ToString(),1),
                        RankAmountUnit = dataTable.Rows[i][6].ToString()
                    };
                    model.Add(rank);
                }
            }

            return model;
        }
        /// <summary>
        /// 产业投资-区域-下沉页数据
        /// </summary>
        public List<ProjectDetail> GetCityInvestProjectData(string cityName)
        {
            var model = new List<ProjectDetail>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "按投资额最大 by city, 2021");
            if(dataTable != null)
            {
                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][2].ToString().Contains(cityName))
                    {
                        ProjectDetail project = new ProjectDetail()
                        {
                            ProjectName = dataTable.Rows[i][0].ToString(),
                            InvestmentCompany = dataTable.Rows[i][1].ToString(),
                            ProjectLocation = cityName,
                            ProjectIndustry = dataTable.Rows[i][3].ToString(),
                            ProjectCapacity = dataTable.Rows[i][4].ToString(),
                            AreaCovered = dataTable.Rows[i][5].ToString() == IndustryRankUnit.NullVaule ? IndustryRankUnit.NullVaule : ToolFunc.ThousandFormatter(dataTable.Rows[i][5].ToString()) + IndustryRankUnit.ByLandSize,
                            TransactionAmount = dataTable.Rows[i][6].ToString() == IndustryRankUnit.NullVaule ? IndustryRankUnit.NullVaule : ToolFunc.BillionConversionFunc(dataTable.Rows[i][6].ToString()),
                            AnnualOutput = dataTable.Rows[i][7].ToString() == IndustryRankUnit.NullVaule ? IndustryRankUnit.NullVaule : ToolFunc.BillionConversionFunc(dataTable.Rows[i][7].ToString()),
                            AnnualTax = dataTable.Rows[i][8].ToString() == IndustryRankUnit.NullVaule ? IndustryRankUnit.NullVaule : ToolFunc.BillionConversionFunc(dataTable.Rows[i][8].ToString())
                        };
                        model.Add(project);
                    }
                }
            }

            return model;
        }
        /// <summary>
        /// 产业投资-行业-下沉页数据
        /// </summary>
        public List<ProjectDetail> GetIndustryInvestProjectData(string industryName)
        {
            var model = new List<ProjectDetail>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "按投资额最大 by industry, 2021");
            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][3].ToString().Equals(industryName))
                    {
                        ProjectDetail project = new ProjectDetail()
                        {
                            ProjectName = dataTable.Rows[i][0].ToString(),
                            InvestmentCompany = dataTable.Rows[i][1].ToString(),
                            ProjectLocation = dataTable.Rows[i][2].ToString(),
                            ProjectIndustry = industryName,
                            ProjectCapacity = dataTable.Rows[i][4].ToString(),
                            AreaCovered = dataTable.Rows[i][5].ToString() == IndustryRankUnit.NullVaule ? IndustryRankUnit.NullVaule : ToolFunc.ThousandFormatter(dataTable.Rows[i][5].ToString()) + IndustryRankUnit.ByLandSize,
                            TransactionAmount = dataTable.Rows[i][6].ToString() == IndustryRankUnit.NullVaule ? IndustryRankUnit.NullVaule : ToolFunc.BillionConversionFunc(dataTable.Rows[i][6].ToString()),
                            AnnualOutput = dataTable.Rows[i][7].ToString() == IndustryRankUnit.NullVaule ? IndustryRankUnit.NullVaule : ToolFunc.BillionConversionFunc(dataTable.Rows[i][7].ToString()),
                            AnnualTax = dataTable.Rows[i][8].ToString() == IndustryRankUnit.NullVaule ? IndustryRankUnit.NullVaule : ToolFunc.BillionConversionFunc(dataTable.Rows[i][8].ToString())
                        };
                        model.Add(project);
                    }
                }
            }

            return model;
        }
        /// <summary>
        /// 读取excel数据包产业投资-热点产业数据
        /// </summary>
        public List<CityRank> GetHotIndustryData()
        {
            var model = new List<CityRank>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "上市企业投资by industry,2021");
            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    CityRank rank = new CityRank()
                    {
                        RankPlace = int.Parse(dataTable.Rows[i][0].ToString().Split('#').Last()),
                        IndexName = dataTable.Rows[i][1].ToString(),
                        IndexValue = dataTable.Rows[i][2].ToString(),
                        Unit = dataTable.Rows[i][3].ToString(),
                        IndexAmount = ToolFunc.ThousandFormatter(dataTable.Rows[i][4].ToString(),1),
                        IndexAmountUnit = dataTable.Rows[i][5].ToString()
                    };
                    model.Add(rank);
                }
            }

            return model;
        }
        /// <summary>
        /// 读取excel数据包产业拿地-活跃区域数据
        /// </summary>
        /// <returns></returns>
        public List<RankModel> GetIndusrtyActiveRegionData()
        {
            var model = new List<RankModel>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "产业拿地by city,2021");
            if(dataTable != null)
            {
                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    RankModel rank = new RankModel()
                    {
                        RankPlace = int.Parse(dataTable.Rows[i][0].ToString().Split('#').Last()),
                        CityName = dataTable.Rows[i][1].ToString(),
                        ProvinceName = dataTable.Rows[i][2].ToString(),
                        RankValue = dataTable.Rows[i][3].ToString(),
                        Unit = dataTable.Rows[i][4].ToString(),
                        RankAmount = dataTable.Rows[i][5].ToString(),
                        RankAmountUnit = dataTable.Rows[i][6].ToString()
                    };
                    model.Add(rank);
                }
            }

            return model;
        }
        /// <summary>
        /// 产业用地-活跃区域-下沉页数据
        /// </summary>
        public List<ProjectDetail> GetCityIndustryProjectData(string cityName)
        {
            var model = new List<ProjectDetail>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "按成交金额最大交易 by city, 2021");
            if (dataTable != null)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i][1].ToString().Equals(cityName))
                    {
                        ProjectDetail project = new ProjectDetail()
                        {
                            ProjectLocation = $"{dataTable.Rows[i][1]}{dataTable.Rows[i][2]}",
                            AreaCovered = ToolFunc.ThousandFormatter(dataTable.Rows[i][3].ToString(), 0) + "㎡",
                            ProjectName = dataTable.Rows[i][6].ToString(),
                            TransactionAmount = ToolFunc.ThousandFormatter(dataTable.Rows[i][7].ToString(),0) + IndustryRankUnit.AmountUnit,
                            ProjectIndustry = dataTable.Rows[i][10].ToString(),
                            TransferDate = ToolFunc.DatetimeFormatter(Convert.ToDateTime(dataTable.Rows[i][5].ToString()))
                        };
                        model.Add(project);
                    }
                }
            }

            return model;
        }
        /// <summary>
        /// 产业用地-热点产业-下沉页数据
        /// </summary>
        public List<ProjectDetail> GetIndustryProjectData(string industryName)
        {
            var model = new List<ProjectDetail>();
            var filePath = Path.Combine("DataFiles", DataFile.FileName);
            Stream stream = new FileInfo(filePath).OpenRead();
            var dataTable = FileHelper.ReadDataFromFile(stream, true, "按成交金额最大交易 by industry, 2021");
            if(dataTable != null)
            {
                for(int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if(industryName.Replace(" ", "").Equals(dataTable.Rows[i][10].ToString()))
                    {
                        ProjectDetail project = new ProjectDetail()
                        {
                            ProjectLocation = $"{dataTable.Rows[i][1]}{dataTable.Rows[i][2]}",
                            AreaCovered = ToolFunc.ThousandFormatter(dataTable.Rows[i][3].ToString(), 0) + "㎡",
                            ProjectName = dataTable.Rows[i][6].ToString(),
                            ProjectIndustry = industryName,
                            TransactionAmount = ToolFunc.ThousandFormatter(dataTable.Rows[i][7].ToString(), 0) + IndustryRankUnit.AmountUnit,
                            TransferDate = ToolFunc.DatetimeFormatter(Convert.ToDateTime(dataTable.Rows[i][5]))
                        };
                        model.Add(project);
                    }
                }
            }

            return model;
        }
    }
}
