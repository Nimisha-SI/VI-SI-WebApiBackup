using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using WebApis.BOL;
using static WebApis.Model.ELModels;

namespace WebApis.Model
{
    public class GetSearchS1DataForCricket
    {
        string con;
        public AppConfig _settings;
       //  GetSearchS1DataForCricket() {
           

       // }
       //private GetSearchS1DataForCricket(IOptions<AppConfig> setting)
       // {
       //     _settings = setting.Value;

       // }

        public string  GetConnectionString(string conString, string subCon)
        {
            var configuration = GetConfiguration();
            // con= configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            con = configuration.GetSection(conString).GetSection(subCon).Value;
            return con;
        }



        public string GetKeyValueForSport(string s1, string s2)
        {
            var configuration = GetConfiguration();
            con = configuration.GetSection(s1).GetSection(s2).Value;
            return con;
        }


        public List<string> GetKeyValueForSportTemp(string s1, string s2)
        {
            var configuration = GetConfiguration();
            //var cons = configuration.GetSection(s1).GetSection(s2).Value;
            //IServiceCollection services;
            //services.AddOptions();
            //var settings = configuration.GetSection("PlayerDetails");
            //var settings2 = configuration.GetSection("CRICKET").GetSection("PlayerDetails");
            //var set = configuration.GetSection("CRICKET:PlayerDetails").Get<AppConfig>();
            //_settings = settings.Value;
            //var obj = services.Configure<AppConfig>(configuration);
            //IConfigurationSection myArraySection = configuration.GetSection("CRICKET:PlayerDetails");
            //var itemArray = myArraySection.AsEnumerable();
            // var section = configuration.GetSection("CRICKET:PlayerDetails:OR").Get<List<string>>();
            List<string> playerdetails = configuration.GetSection(s2+":"+s1+"_PlayerDetails").Get<List<string>>();
            //con = configuration.GetSection(s1).Value;
            return playerdetails;
        }


        public IConfiguration GetConfiguration() {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public   List<SearchCricketData> getCricketData(string DBConnectionString, bool isFullDownload = true, int FullBatchCounter = 1, int FullRowCount = 0)
        {
            // public static string DBConnectionString { get { return ConfigurationManager.ConnectionStrings[GlobalInfo.DBSchemeName].ConnectionString; } }

            //string DBConnectionString;
            int FullDownloadFlag = isFullDownload ? 1 : 0;
            List<SearchCricketData> _objLstData = new List<SearchCricketData>();
            try
            {
                DataSet _objDsSearch = new DataSet();
                switch (isFullDownload)
                {
                    case true:
                        _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_s1_data_search_full", 1, FullDownloadFlag, FullBatchCounter, FullRowCount);
                        break;
                    case false:
                        _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_s1_data_search", 1, FullDownloadFlag);
                        break;
                }


                if (_objDsSearch.Tables != null && _objDsSearch.Tables.Count > 0)
                {
                    SearchCricketData _objSearchData;
                    foreach (DataRow rowitem in _objDsSearch.Tables[0].Rows)
                    {
                        _objSearchData = new SearchCricketData();
                        _objSearchData.Id = Convert.ToString(rowitem["s_id"]);
                        _objSearchData.RId = Convert.ToString(rowitem["Row_Id"]);
                        _objSearchData.MatchId = Convert.ToString(rowitem["match_id"]);
                        _objSearchData.MarkIn = Convert.ToString(rowitem["markin_timecode"]);
                        _objSearchData.MarkOut = Convert.ToString(rowitem["markout_timecode"]);
                        _objSearchData.ShortMarkIn = Convert.ToString(rowitem["markin_short"]);
                        _objSearchData.ShortMarkOut = Convert.ToString(rowitem["markout_short"]);
                        _objSearchData.ClearId = Convert.ToString(rowitem["clear_id"]).Trim();
                        _objSearchData.QClearId = Convert.ToString(rowitem["clear_id"]).Replace("-", string.Empty);
                        _objSearchData.MediaId = Convert.ToString(rowitem["media_id"]);
                        _objSearchData.CompType = Convert.ToString(rowitem["comp_type"]);
                        _objSearchData.VenueId = Convert.ToString(rowitem["venue_id"]);
                        _objSearchData.Venue = Convert.ToString(rowitem["venue_name"]);
                        _objSearchData.SeriesId = Convert.ToString(rowitem["series_id"]);
                        _objSearchData.Series = Convert.ToString(rowitem["series_name"]);
                        //_objSearchData.MatchDate = Convert.ToString(rowitem["matchdate"]) != string.Empty ? Convert.ToDateTime(rowitem["matchdate"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty;
                        _objSearchData.MatchDate = Convert.ToString(rowitem["event_date"]) != "" ? int.Parse(Convert.ToDateTime(rowitem["event_date"]).ToString("yyyyMMdd", CultureInfo.InvariantCulture)) : 0;
                        _objSearchData.Team1Id = Convert.ToString(rowitem["team1_id"]);
                        _objSearchData.Team1 = Convert.ToString(rowitem["team1"]);
                        _objSearchData.Team2Id = Convert.ToString(rowitem["team2_id"]);
                        _objSearchData.Team2 = Convert.ToString(rowitem["team2"]);
                        _objSearchData.BatsmanId = Convert.ToString(rowitem["batsman_id"]);
                        _objSearchData.Batsman = Convert.ToString(rowitem["batsman"]);
                        _objSearchData.BowlerId = Convert.ToString(rowitem["bowler_id"]);
                        _objSearchData.Bowler = Convert.ToString(rowitem["bowler"]);
                        _objSearchData.FielderId = Convert.ToString(rowitem["fielder_id"]);
                        _objSearchData.Fielder = Convert.ToString(rowitem["fielder"]);
                        _objSearchData.ShotTypeId = Convert.ToString(rowitem["shot_typeid"]);
                        _objSearchData.ShotType = Convert.ToString(rowitem["shot_type"]);
                        _objSearchData.ShotZoneId = Convert.ToString(rowitem["zone_id"]);
                        _objSearchData.ShotZone = Convert.ToString(rowitem["zone_name"]);
                        _objSearchData.DeliveryTypeId = Convert.ToString(rowitem["DeliveryTypeID"]);
                        _objSearchData.DeliveryType = Convert.ToString(rowitem["DeliveryTypeName"]);
                        _objSearchData.DismissalId = Convert.ToString(rowitem["dismissal_id"]);
                        _objSearchData.Dismissal = Convert.ToString(rowitem["dismissal"]);
                        _objSearchData.RunsSaved = Convert.ToString(rowitem["runssaved"]);
                        _objSearchData.RunsConceeded = Convert.ToString(rowitem["runsconceded"]);
                        _objSearchData.BowlingLineId = Convert.ToString(rowitem["line_id"]);
                        _objSearchData.BowlingLine = Convert.ToString(rowitem["pitching_line"]);
                        _objSearchData.BowlingLengthId = Convert.ToString(rowitem["length_id"]);
                        _objSearchData.BowlingLength = Convert.ToString(rowitem["pitching_length"]);
                        _objSearchData.OverNo = Convert.ToString(rowitem["OverNumber"]);
                        _objSearchData.BallNo = Convert.ToString(rowitem["BallNumber"]);
                        _objSearchData.Runs = Convert.ToString(rowitem["RunsScored"]);
                        _objSearchData.IsFour = Convert.ToString(rowitem["is_four"]);
                        _objSearchData.IsSix = Convert.ToString(rowitem["is_six"]);
                        _objSearchData.IsDot = Convert.ToString(rowitem["is_dot"]);
                        _objSearchData.IsBeaten = Convert.ToString(rowitem["is_beaten"]);
                        _objSearchData.IsEdged = Convert.ToString(rowitem["Edge"]);
                        _objSearchData.IsWide = Convert.ToString(rowitem["is_wide"]);
                        _objSearchData.IsNoBall = Convert.ToString(rowitem["is_NoBall"]);
                        _objSearchData.IsOverTheWicket = Convert.ToString(rowitem["is_over_tw"]);
                        _objSearchData.IsRoundTheWicket = Convert.ToString(rowitem["is_round_tw"]);
                        _objSearchData.Skill = Convert.ToString(rowitem["skill"]);
                        _objSearchData.IsCatch = Convert.ToString(rowitem["is_catch"]);
                        _objSearchData.IsRunOut = Convert.ToString(rowitem["Is_runout"]);
                        _objSearchData.IsDropped = Convert.ToString(rowitem["Is_Dropped"]);
                        _objSearchData.FielderPositionId = Convert.ToString(rowitem["position_id"]);
                        _objSearchData.FielderPosition = Convert.ToString(rowitem["FielderPosition"]);
                        _objSearchData.SType = Convert.ToString(rowitem["s_type"]);
                        _objSearchData.Title = Convert.ToString(rowitem["title"]);
                        _objSearchData.Description = Convert.ToString(rowitem["description"]);
                        _objSearchData.Duration = Convert.ToString(rowitem["duration"]);
                        _objSearchData.RunsSaved = Convert.ToString(rowitem["runssaved"]);

                        _objSearchData.Innings = Convert.ToString(rowitem["InningsNumber"]);
                        _objSearchData.MatchInstance = string.Empty;
                        _objSearchData.BatsmanRuns = Convert.ToString(rowitem["batsmanRuns"]) != string.Empty ? Convert.ToInt32(rowitem["batsmanRuns"]) : 0;
                        _objSearchData.BatsmanBallsFaced = Convert.ToString(rowitem["batsmanBalls"]) != string.Empty ? Convert.ToInt32(rowitem["batsmanBalls"]) : 0;
                        _objSearchData.MatchResult = Convert.ToString(rowitem["result"]);
                        _objSearchData.Winner = Convert.ToString(rowitem["Winner"]);
                        _objSearchData.Loser = Convert.ToString(rowitem["Loser"]);
                        _objSearchData.BowlerBallsBowled = Convert.ToString(rowitem["BallsBowledByBowler"]) != string.Empty ? Convert.ToInt32(rowitem["BallsBowledByBowler"]) : 0;
                        _objSearchData.BowlerRunsConceeded = Convert.ToString(rowitem["RunsConcededByBowler"]) != string.Empty ? Convert.ToInt32(rowitem["RunsConcededByBowler"]) : 0;
                        _objSearchData.BowlerWickets = Convert.ToString(rowitem["wicketsofbowler"]) != string.Empty ? Convert.ToInt32(rowitem["wicketsofbowler"]) : 0;
                        _objSearchData.TeamScore = Convert.ToString(rowitem["TeamScore"]) != string.Empty ? Convert.ToInt32(rowitem["TeamScore"]) : 0;
                        _objSearchData.TeamOver = Convert.ToString(rowitem["TeamOver"]) != string.Empty ? Convert.ToInt32(rowitem["TeamOver"]) : 0;

                        _objSearchData.WinnerPlayer = Convert.ToString(rowitem["WinnerPlayer"]);
                        _objSearchData.LoserPlayer = Convert.ToString(rowitem["LoserPlayer"]);
                        _objSearchData.WinningFielder = Convert.ToString(rowitem["WinningFielder"]);

                        _objSearchData.IsPowerPlayOver = Convert.ToString(rowitem["IsPowerPlayOver"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["IsPowerPlayOver"])) : "0";
                        _objSearchData.is_middle = Convert.ToString(rowitem["is_middle"]);
                        _objSearchData.is_death = Convert.ToString(rowitem["is_death"]);
                        _objSearchData.is_lastover = Convert.ToString(rowitem["is_lastover"]);
                        _objSearchData.is_lastBall = Convert.ToString(rowitem["is_lastBall"]);

                        _objSearchData.ParentSeriesId = Convert.ToString(rowitem["parent_series_id"]);
                        _objSearchData.IsAsset = Convert.ToString(rowitem["is_asset"]);

                        _objSearchData.IsWicket = Convert.ToString(rowitem["is_wicket"]);
                        _objSearchData.IsAppeal = Convert.ToString(rowitem["is_appeal"]);

                        _objSearchData.HasShortClip = Convert.ToString(rowitem["has_short_clip"]);
                        _objSearchData.EventId = Convert.ToString(rowitem["event_id"]);
                        _objSearchData.EventText = Convert.ToString(rowitem["event_text"]);

                        _objSearchData.IsTagged = Convert.ToString(rowitem["is_tagged"]);
                        _objSearchData.Match = Convert.ToString(rowitem["game_event_name"]);
                        _objSearchData.ParentSeriesName = Convert.ToString(rowitem["parent_series_name"]);
                        _objSearchData.IsMisField = Convert.ToString(rowitem["is_misfield"]);
                        _objSearchData.AssetTypeId = Convert.ToString(rowitem["asset_type_id"]);
                        _objSearchData.ClearId2 = Convert.ToString(rowitem["clear_id_2"]);
                        _objSearchData.QClearId2 = Convert.ToString(rowitem["clear_id_2"]).Replace("-", string.Empty);
                        _objSearchData.ClearId3 = Convert.ToString(rowitem["clear_id_3"]);
                        _objSearchData.QClearId3 = Convert.ToString(rowitem["clear_id_3"]).Replace("-", string.Empty);
                        _objSearchData.ClearId4 = Convert.ToString(rowitem["clear_id_4"]);
                        _objSearchData.QClearId4 = Convert.ToString(rowitem["clear_id_4"]).Replace("-", string.Empty);
                        _objSearchData.ClearId5 = Convert.ToString(rowitem["clear_id_5"]);
                        _objSearchData.QClearId5 = Convert.ToString(rowitem["clear_id_5"]).Replace("-", string.Empty);
                        _objSearchData.ClearId6 = Convert.ToString(rowitem["clear_id_6"]);
                        _objSearchData.QClearId6 = Convert.ToString(rowitem["clear_id_6"]).Replace("-", string.Empty);

                        _objSearchData.MediaId2 = Convert.ToString(rowitem["media_id_2"]);
                        _objSearchData.MediaId3 = Convert.ToString(rowitem["media_id_3"]);
                        _objSearchData.MediaId4 = Convert.ToString(rowitem["media_id_4"]);
                        _objSearchData.MediaId5 = Convert.ToString(rowitem["media_id_5"]);
                        _objSearchData.MediaId6 = Convert.ToString(rowitem["media_id_6"]);
                        _objSearchData.LanguageId = Convert.ToString(rowitem["language_id"]);
                        _objSearchData.Language = Convert.ToString(rowitem["language"]);
                        _objSearchData.BattingOrder = Convert.ToString(rowitem["batting_order_no"]);
                        _objSearchData.BowlingArm = Convert.ToString(rowitem["Bowling_arm"]);

                        _objSearchData.CF_Batsman = new CompletionField
                        {
                            Input = new[] { Convert.ToString(rowitem["batsman"]) }
                        };
                        _objSearchData.CF_Bowler = new CompletionField
                        {
                            Input = new[] { Convert.ToString(rowitem["bowler"]) }
                        };
                        _objSearchData.batsmanddl = Convert.ToString(rowitem["batsman"]) + "|" + Convert.ToString(rowitem["batsman_id"]);
                        _objSearchData.bowlerddl = Convert.ToString(rowitem["bowler"]) + "|" + Convert.ToString(rowitem["bowler_id"]);

                        _objLstData.Add(_objSearchData);
                        _objSearchData = null;
                    }
                }
            }

            catch (Exception ex)
            {
                //ErrorLog.LogServiceError("SearchDAO.cs", "GetSearchS1DataForCricket", ex.ToString());
            }
            return _objLstData;
        }

        public  List<SearchS2Data> GetAllSearchS2Data(string DBConnectionString, int SportId, bool isFullDownload = true)
        {
            int FullDownloadFlag = isFullDownload ? 1 : 0;
            List<SearchS2Data> _objLstData = new List<SearchS2Data>();
            try
            {
                DataSet _objDsSearch = new DataSet();

                _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_s2_data_search_enhanced", SportId, FullDownloadFlag);
                //  _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_s2_data_search", SportId, FullDownloadFlag);
                if (_objDsSearch.Tables != null && _objDsSearch.Tables.Count > 0)
                {
                    foreach (DataRow rowitem in _objDsSearch.Tables[0].Rows)
                    {
                        SearchS2Data _objSearchData = new SearchS2Data();
                        _objSearchData.Id = Convert.ToString(rowitem["s_id"]);
                        _objSearchData.RId = Convert.ToInt64(rowitem["s_id"]);
                        _objSearchData.MatchId = Convert.ToString(rowitem["match_id"]);
                        //_objSearchData.Match = Convert.ToString(rowitem["match_name"]); 
                        _objSearchData.MarkIn = Convert.ToString(rowitem["mark_in"]);
                        _objSearchData.MarkOut = Convert.ToString(rowitem["mark_out"]);
                        _objSearchData.ClearId = Convert.ToString(rowitem["clear_id"]).Trim();
                        _objSearchData.QClearId = Convert.ToString(rowitem["clear_id"]).Replace("-", string.Empty);
                        _objSearchData.MediaId = Convert.ToString(rowitem["media_id"]);
                        _objSearchData.CompType = Convert.ToString(rowitem["comp_type"]);
                        _objSearchData.VenueId = Convert.ToString(rowitem["venue_id"]);
                        _objSearchData.Venue = Convert.ToString(rowitem["venue_name"]);
                        _objSearchData.SeriesId = Convert.ToString(rowitem["series_id"]);
                        _objSearchData.Series = Convert.ToString(rowitem["series_name"]);
                        //_objSearchData.MatchDate = Convert.ToString(rowitem["matchdate"]) != string.Empty ? Convert.ToDateTime(rowitem["matchdate"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty;
                        _objSearchData.MatchDate = Convert.ToString(rowitem["event_date"]) != "" ? int.Parse(Convert.ToDateTime(rowitem["event_date"]).ToString("yyyyMMdd", CultureInfo.InvariantCulture)) : 0;
                        _objSearchData.MatchStageId = Convert.ToString(rowitem["match_stage_id"]);
                        _objSearchData.MatchStage = Convert.ToString(rowitem["match_stage"]);
                        _objSearchData.Team1Id = Convert.ToString(rowitem["team1_id"]);
                        _objSearchData.Team1 = Convert.ToString(rowitem["team1"]);
                        _objSearchData.Team2Id = Convert.ToString(rowitem["team2_id"]);
                        _objSearchData.Team2 = Convert.ToString(rowitem["team2"]);
                        _objSearchData.Attribute_Id_Level1 = Convert.ToString(rowitem["attribute_id_level_1"]);
                        _objSearchData.Attribute_Name_Level1 = Convert.ToString(rowitem["attribute_name_level_1"]);
                        _objSearchData.Attribute_Id_Level2 = Convert.ToString(rowitem["attribute_id_level_2"]);
                        _objSearchData.Attribute_Name_Level2 = Convert.ToString(rowitem["attribute_name_level_2"]);
                        _objSearchData.Attribute_Id_Level3 = Convert.ToString(rowitem["attribute_id_level_3"]);
                        _objSearchData.Attribute_Name_Level3 = Convert.ToString(rowitem["attribute_name_level_3"]);
                        _objSearchData.Attribute_Id_Level4 = Convert.ToString(rowitem["attribute_id_level_4"]);
                        _objSearchData.Attribute_Name_Level4 = Convert.ToString(rowitem["attribute_name_level_4"]);
                        _objSearchData.EmotionId = Convert.ToString(rowitem["emotion_id"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["emotion_id"])) : "0";
                        _objSearchData.EmotionName = Convert.ToString(rowitem["emotion_name"]);
                        //_objSearchData.EntityName = Convert.ToString(rowitem["entity_name"]);
                        _objSearchData.EntityName_1 = Convert.ToString(rowitem["entity_1"]);
                        _objSearchData.EntityName_2 = Convert.ToString(rowitem["entity_2"]);
                        _objSearchData.EntityName_3 = Convert.ToString(rowitem["entity_3"]);
                        _objSearchData.EntityName_4 = Convert.ToString(rowitem["entity_4"]);
                        _objSearchData.EntityName_5 = Convert.ToString(rowitem["entity_5"]);
                        _objSearchData.EntityId_1 = Convert.ToString(rowitem["entity_1_id"]);
                        _objSearchData.EntityId_2 = Convert.ToString(rowitem["entity_2_id"]);
                        _objSearchData.EntityId_3 = Convert.ToString(rowitem["entity_3_id"]);
                        _objSearchData.EntityId_4 = Convert.ToString(rowitem["entity_4_id"]);
                        _objSearchData.EntityId_5 = Convert.ToString(rowitem["entity_5_id"]);
                        _objSearchData.ParentSeriesId = Convert.ToString(rowitem["parent_series_id"]);
                        _objSearchData.IsAsset = Convert.ToString(rowitem["is_asset"]);
                        _objSearchData.IsBigMoment = Convert.ToString(rowitem["is_big_moment"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_big_moment"])) : "0";
                        _objSearchData.IsFunnyMoment = Convert.ToString(rowitem["is_funny_moment"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_funny_moment"])) : "0";
                        _objSearchData.IsAudioPiece = Convert.ToString(rowitem["is_audio_piece"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_audio_piece"])) : " 0";
                        _objSearchData.Title = Convert.ToString(rowitem["title"]);
                        _objSearchData.Description = Convert.ToString(rowitem["description"]);
                        _objSearchData.Duration = Convert.ToString(rowitem["duration"]);
                        _objSearchData.ClearId2 = Convert.ToString(rowitem["clear_id_2"]);
                        _objSearchData.MediaId2 = Convert.ToString(rowitem["media_id_2"]);
                        _objSearchData.IsTagged = Convert.ToString(Convert.ToInt32(rowitem["is_tagged"]));
                        _objSearchData.LanguageId = Convert.ToString(rowitem["language_id"]);
                        _objSearchData.Language = Convert.ToString(rowitem["language"]); ;
                        _objSearchData.SType = Convert.ToString(rowitem["s_type"]);//new
                        _objSearchData.EventId = _objSearchData.IsBigMoment == "1" ? "5" : _objSearchData.IsFunnyMoment == "1" ? "6" : _objSearchData.IsAudioPiece == "1" ? "12" : "0";
                        _objSearchData.EventText = _objSearchData.IsBigMoment == "1" ? "BM" : _objSearchData.IsFunnyMoment == "1" ? "FM" : _objSearchData.IsAudioPiece == "1" ? "AP" : "";
                        _objSearchData.SportId = SportId;
                        _objLstData.Add(_objSearchData);
                    }
                }
            }

            catch (Exception ex)
            {
                //ErrorLog.LogServiceError("SearchDAO.cs", "GetAllSearchS2Data", ex.ToString());
            }
            return _objLstData;
        }

        public string GetSavedSearches(string DBConnectionString,SaveSearchesRequestData objSavedSearchData)
        {
            string response = string.Empty;
            try
            {
                DataSet _objDsSearch = new DataSet();
                _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_cl_get_search", objSavedSearchData.SportId, objSavedSearchData.UserId == 0 ? null : objSavedSearchData.UserId, objSavedSearchData.SearchTypeId);
                if (_objDsSearch.Tables != null && _objDsSearch.Tables.Count > 0)
                {
                    response = JSONHelper.FromDataTable(_objDsSearch.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                response = ex.ToString();
            }
            return response;
        }



    }
}
