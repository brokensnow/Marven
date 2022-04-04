#region Using
using REAccess.Mobile.Common.Interfaces;
using REAccess.Mobile.Common.Utils;
using REAccess.Mobile.Common.ViewModel;
using REAccess.Mobile.Database.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using static REAccess.Mobile.Common.Constants;
#endregion

namespace REAccess.Mobile.Common.Services
{
    public class RankService : BaseService, IRankService
    {
        private readonly IUtilService _utilService;
        public RankService(IUtilService utilService)
        {
            _utilService = utilService;
        }

        ///<summary>
        ///清理所有正在使用的资源
        ///</summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 单指标排名，根据用户选择的指标获取该指标的真实数据进行排名，取排名前N的数据
        /// </summary>
        //public SingleIndexModel GetSingleIndexRank(string selectIndex, string selectYear,int dataCount)
        //{
        //    SingleIndexModel model = new SingleIndexModel();
        //    List<RankModel> singleIndexList = new List<RankModel>();
        //    //获取数据库所有指标的真实数据
        //    var indicatorOriginalScore = StaticCache.IndicatorOriginalScore;
        //    //获取数据库城市列表
        //    var districtList = StaticCache.Districts;
        //    if (string.IsNullOrEmpty(selectYear))
        //    {
        //        //若年份为空，则取数据库数据的最新年份
        //        selectYear = _utilService.GetYearListByIndexName(selectIndex).FirstOrDefault();
        //    }
        //    model.Year = selectYear;

        //    //默认取排名前五的数据
        //    dataCount = dataCount == 0 ? 5 : dataCount;
        //    //获取单位
        //    var dbIndicator = StaticCache.Indicators.FirstOrDefault(x => x.Name == selectIndex);
        //    var dataUnit = string.Empty;
        //    if(dbIndicator != null)
        //    {
        //        dataUnit = StaticCache.Indicators.FirstOrDefault(x => x.Name == selectIndex).DisplayDataUnit;
        //    }
        //    model.Unit = dataUnit;

        //    foreach (var district in districtList)
        //    {
        //        var indicator = indicatorOriginalScore.FirstOrDefault(x => x.DistrictSk == district.DistrictSk && x.SnapshotPeriod == selectYear);
        //        if (indicator != null)
        //        {
        //            RankModel singleIndex = new RankModel()
        //            {
        //                DistrictSk = district.DistrictSk,
        //                CityName = district.CityName,
        //                ProvinceName = district.ProvName,
        //                RankValue = ((decimal)indicator.GetOriginalValueByName(selectIndex)).ToString("N1"),//通过指标中文名映射数据库字段名，获取该数据的真实值
        //                Unit = dataUnit
        //            };
        //            singleIndexList.Add(singleIndex);
        //        }
        //    }

        //    //根据所有指标真实数据进行倒叙排名，并取出排名前五的数据
        //    singleIndexList = singleIndexList.OrderByDescending(x => float.Parse(x.RankValue)).Take(dataCount).ToList();

        //    //根据指标数据排名--数据相同则名次相同
        //    for (var i = 0; i < singleIndexList.Count(); i++)
        //    {
        //        if (i == 0)
        //        {
        //            singleIndexList[i].RankPlace = 1;
        //        }
        //        else
        //        {
        //            if (singleIndexList[i].RankValue == singleIndexList[i - 1].RankValue)
        //            {
        //                singleIndexList[i].RankPlace = singleIndexList[i - 1].RankPlace;
        //            }
        //            else
        //            {
        //                singleIndexList[i].RankPlace = i + 1;
        //            }
        //        }
        //    }
        //    model.SingleIndexList = singleIndexList;

        //    return model;
        //}
        public SingleIndexModel GetSingleIndexRank(string selectIndex, string selectYear, int dataCount)
        {
            SingleIndexModel model = _utilService.GetSingleRankData(selectIndex);

            return model;
        }

        /// <summary>
        /// 城市排名,根据用户选择的城市，获取该城市的每一个指标在所有城市中的排名，并取出排名前N的数据
        /// </summary>
        //public CityRankModel GetCityRank(string selectCity, string selectYear,int dataCount)
        //{
        //    CityRankModel model = new CityRankModel();
        //    List<CityRank> cityRankList = new List<CityRank>();
        //    //筛选时间默认为最新时间
        //    if (string.IsNullOrEmpty(selectYear))
        //    {
        //        selectYear = StaticCache.IndicatorOriginalScore.Max(x => x.SnapshotPeriod);
        //    }
        //    //默认取排名前五的数据
        //    dataCount = dataCount == 0 ? 5 : dataCount;

        //    model.Year = selectYear;
        //    //获取指标列表
        //    var indicatorList = _utilService.GetAllIndex();
        //    //获取已选城市ID
        //    var districtSk = StaticCache.Districts.FirstOrDefault(x => x.CityName == selectCity && x.CityName == x.DistrictName).DistrictSk;
        //    //获取指标得分表
        //    var indicatorScore = StaticCache.IndicatorScore.FirstOrDefault(x => x.DistrictSk == districtSk && x.SnapshotPeriod == selectYear);
        //    foreach(var indicator in indicatorList)
        //    {
        //        var singleIndexRank = GetSingleIndexRank(indicator, "", StaticCache.Districts.Count(x => x.CityName == x.DistrictName));
        //        var currentRank = singleIndexRank.SingleIndexList.FirstOrDefault(x => x.DistrictSk == districtSk);
        //        CityRank cityRank = new CityRank()
        //        {
        //            IndexName = indicator,
        //            RankPlace = currentRank.RankPlace,
        //            IndexValue = currentRank.RankValue,
        //            Unit = singleIndexRank.Unit,
        //            Year = singleIndexRank.Year,
        //        };
        //        cityRankList.Add(cityRank);
        //    }

        //    cityRankList = cityRankList.Where(x => x.IndexValue != NoData.NotPublished).OrderBy(x => x.RankPlace).Take(dataCount).ToList();

        //    model.CityRankList = cityRankList;

        //    return model;
        //}
        public CityRankModel GetCityRank(string selectCity, string selectYear, int dataCount)
        {
            CityRankModel model = new CityRankModel();
            //默认取排名前五的数据
            dataCount = dataCount == 0 ? 5 : dataCount;
            var cityRankList = _utilService.GetCityRankData(selectCity);
            cityRankList = cityRankList.Where(x => x.IndexValue != NoData.NotPublished).OrderBy(x => x.RankPlace).Take(dataCount).ToList();
            model.CityRankList = cityRankList;

            return model;
        }

        /// <summary>
        /// 获取所有咨讯信息
        /// </summary>
        public List<NewsModel> GetNewsList(int currentPage,int pageSize)
        {
            //List<NewsModel> model = StaticCache.DdsNews.Select(x => new NewsModel()
            //{
            //    Id = x.Id,
            //    NewsTitle = WebUtility.HtmlEncode(x.Title),
            //    NewsContent = x.Content,
            //    NewsImage = Path.Combine("RealTimeInfoImgs",x.Img).Replace(".png", ".jpg"),
            //    //NewsImage = FileEncode.GetImageBytes($"{Directory.GetCurrentDirectory()}\\RealTimeInfoImgs\\{x.Img}"),
            //    NewsReleaseDate = ToolFunc.DatetimeFormatter(x.ReleaseTime),
            //    NewTags = x.Tags.Split(';').Where(x => x != "").ToList()
            //}).ToList();
            List<NewsModel> model = _utilService.GetNewsData();
            currentPage = currentPage == 0 ? 1 : currentPage;
            pageSize = pageSize == 0 ? 5 : pageSize;

            model = PageHelper.PageUtil<NewsModel>(currentPage, pageSize, model);

            return model;
        }
        /// <summary>
        /// 获取资讯详细信息
        /// </summary>
        public NewsModel GetNewsById(int newsId)
        {
            NewsModel model = new NewsModel();

            List<NewsModel> newsData = _utilService.GetNewsData();
            model = newsData.FirstOrDefault(x => x.Id == newsId);
            if(model != null)
            {
                model.NewsTitle = WebUtility.HtmlDecode(model.NewsTitle).Replace("<span>", "").Replace("</span>", "");
            }

            return model;
        }
    }
}
