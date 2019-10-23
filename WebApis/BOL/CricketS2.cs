using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    public class CricketS2  
    {
        EsLayer oLayer = new EsLayer();
        private static List<SearchCricketResultData> MapcricketS1datascopy(List<SearchCricketResultTempData> _objs1Data, MatchDetail _objmatchdetail)
        {
            List<SearchCricketResultData> Final_result = new List<SearchCricketResultData>();
            string[] Id = _objmatchdetail.LanguageId.Split(',');
            switch (_objmatchdetail.IsAsset)
            {
                case true:
                    Final_result = MapcricketS1data(_objs1Data, "4");
                    break;
                case false:
                    if (string.IsNullOrEmpty(_objmatchdetail.LanguageId))
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (i == 1)
                            {
                                List<SearchCricketResultData> FinalTempResult = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId != "").ToList();
                                FinalTempResult = MapcricketS1data(temp, "1");
                                Final_result = FinalTempResult;
                                FinalTempResult = null;
                            }
                            else if (i == 2)
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId2 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "2");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (i == 3)
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId3 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "3");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (i == 4)
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId4 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "6");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (i == 5)
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId5 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "5");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (i == 6)
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId6 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "8");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                        }

                    }
                    else
                    {

                        for (int i = 0; i < Id.Count(); i++)
                        {
                            if (Id[i] == "1")
                            {
                                List<SearchCricketResultData> FinalTempResult = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId != "").ToList();
                                FinalTempResult = MapcricketS1data(temp, "1");
                                Final_result = FinalTempResult;
                                FinalTempResult = null;
                            }
                            else if (Id[i] == "2")
                            {
                                List<SearchCricketResultData> FinalTempResult = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId2 != "").ToList();
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                FinalTempResult1 = MapcricketS1data(temp, "2");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;

                            }
                            else if (Id[i] == "3")
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId3 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "3");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (Id[i] == "6")
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId4 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "6");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (Id[i] == "5")
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId5 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "5");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (Id[i] == "8")
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId6 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "8");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                        }
                    }
                    break;
            }

            return Final_result;
        }
        private static List<SearchCricketResultData> MapcricketS1data(List<SearchCricketResultTempData> _objs1Data, string cases)
        {
            List<SearchCricketResultData> _objresult = new List<SearchCricketResultData>();
            try
            {



                switch (cases)
                {

                    case "1":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId,
                                MediaId = str.MediaId,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DismissalId = str.DismissalId,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "1"

                            };
                            _objresult.Add(result);
                        }
                        break;
                    case "2":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId2,
                                MediaId = str.MediaId2,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "2"
                            };
                            _objresult.Add(result);
                        }
                        break;
                    case "3":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId3,
                                MediaId = str.MediaId3,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "3"
                            };
                            _objresult.Add(result);
                        }

                        break;
                    case "6":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId4,
                                MediaId = str.MediaId4,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "6"
                            };
                            _objresult.Add(result);
                        }

                        break;
                    case "5":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId5,
                                MediaId = str.MediaId5,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "5"
                            };
                            _objresult.Add(result);
                        }
                        break;
                    case "8":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId6,
                                MediaId = str.MediaId6,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "8"
                            };
                            _objresult.Add(result);
                        }
                        break;
                    default:
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId,
                                MediaId = str.MediaId,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = str.LanguageId

                            };
                            _objresult.Add(result);
                        }
                        break;

                }


            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return _objresult;
        }

        public  IEnumerable<SearchCricketResultData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName)
        {
            List<SearchCricketResultData> Final_result = new List<SearchCricketResultData>();
            EsClient = oLayer.CreateConnection();
            searchcricket sc = new searchcricket();
          
            var result = EsClient.Search<SearchCricketData>(s => s.Index(IndexName).Query(q => _objNestedQuery).Size(409846));
            //Final_result = SearchResultFilterDataMap(result);
            return Final_result;
        }

        public  List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchCricketData> result)
        {
            List<SearchResultFilterData> ListObj = new List<SearchResultFilterData>();
            foreach (var hit in result.Hits)
            {
                ListObj.Add(new SearchResultFilterData
                {
                    ClearId = hit.Source.ClearId.ToString(),
                    Description = hit.Source.Description.ToString(),
                    MarkIn = hit.Source.MarkIn.ToString(),
                    MarkOut = hit.Source.MarkOut.ToString(),
                    ShotType = hit.Source.MarkOut.ToString(),
                    Duration = hit.Source.Duration.ToString(),
                    DeliveryType = hit.Source.DeliveryType.ToString(),
                    DeliveryTypeId = hit.Source.DeliveryTypeId.ToString(),
                    EventId = hit.Source.EventId.ToString(),
                    EventName = hit.Source.EventText.ToString(),
                    Id = hit.Source.Id.ToString(),
                    MatchId = hit.Source.MatchId.ToString(),
                    MediaId = hit.Source.MediaId.ToString(),
                    Title = hit.Source.Title.ToString(),
                    ShotTypeId = hit.Source.ShotTypeId.ToString(),
                });
            }
            return ListObj;
        }
    }
}
