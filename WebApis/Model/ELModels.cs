using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApis.Model
{
    public class ELModels
    {
        public class SearchResultFilterData
        {

            public string Id { get; set; }
            public string ClearId { get; set; }
            public string MatchId { get; set; }
            public int MatchDate { get; set; }
            public string MarkIn { get; set; }
            public string MarkOut { get; set; }
            public string ShortMarkIn { get; set; }
            public string ShortMarkOut { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Duration { get; set; }
            public int IsAsset { get; set; }
            public string EventId { get; set; }
            public string EventName { get; set; }
            public string EventText { get; set; }
            public string MediaId { get; set; }
            public string ShotTypeId { get; set; }
            public string ShotType { get; set; }
            public string SaveTypeId { get; set; }
            public string SaveType { get; set; }
            public string ShotResultId { get; set; }
            public string ShotResult { get; set; }
            public string EventReasonId { get; set; }
            public string EventReason { get; set; }
            public string GoalZoneId { get; set; }
            public string GoalZone { get; set; }
            public string Innings { get; set; }
            public string DeliveryTypeId { get; set; }
            public string DeliveryType { get; set; }
            public string ErrorTypeId { get; set; }
            public string ErrorType { get; set; }
            public string SType { get; set; }
        }
        public class FilteredEntityForCricket
        {
            public string CompType { get; set; }
            public string MatchId { get; set; }
            public string ClearId { get; set; }
            public string BatsmanId { get; set; }
            public string Batsman { get; set; }
            public string Team1Id { get; set; }
            public string Team1 { get; set; }
            public string Team2Id { get; set; }
            public string Team2 { get; set; }
            public string SeriesId { get; set; }
            public string Series { get; set; }
            public string VenueId { get; set; }
            public string Venue { get; set; }
            public string Match { get; set; }
            public string BowlerId { get; set; }
            public string Bowler { get; set; }
            public string FielderId { get; set; }
            public string Fielder { get; set; }
            public string ParentSeriesId { get; set; }
            public string ParentSeriesName { get; set; }
            public string entityid { get; set; }
            public string entityname { get; set; }
            public string entityplayername { get; set; }
            public int MatchDate { get; set; }
        }
        public class SearchCricketExtendedResultData
        {
            public IEnumerable<SearchCricketData> ResultData { get; set; }
            public List<SearchS1CricketMasterData> ResultMasterData { get; set; }
            public Dictionary<string, Int64> ResultCount { get; set; }
            public CricketDerivedData ResultDerivedData { get; set; }
        }
        public class SearchS1CricketMasterData
        {
            public string ShotTypeId { get; set; }
            public string ShotType { get; set; }
            public string ShotZoneId { get; set; }
            public string ShotZone { get; set; }
            public string Dismissal { get; set; }
            public string DismissalId { get; set; }
            public string DeliveryTypeId { get; set; }
            public string DeliveryType { get; set; }
            public string BowlingLengthId { get; set; }
            public string BowlingLength { get; set; }
            public string BowlingLineId { get; set; }
            public string BowlingLine { get; set; }
            public string FielderPositionId { get; set; }
            public string FielderPosition { get; set; }
            public string BattingOrder { get; set; }
            public string BowlingArm { get; set; }
            public int MatchDate { get; set; }
        }
        public class CricketDerivedData
        {
            public Dictionary<string, Int64> RangeData { get; set; }
            public Dictionary<string, object> MasterData { get; set; }
        }
        public class ExtendedSearchResultFilterData
        {
            public IEnumerable<SearchResultFilterData> ResultData { get; set; }
            public MasterDatas Master { get; set; }
        }
        public class MasterDatas
        {
            public Dictionary<string, object> MasterData { get; set; }
        }
        public class STFilterRequestData
        {
            public MatchDetail MatchDetail { get; set; }
            public dynamic S1Data { get; set; }
            public MatchSituation MatchSituation { get; set; }
            public S2ActionData S2Data { get; set; }
            public Moments Moments { get; set; }
            public string EntityText { get; set; }
        }
        public class MatchSituation
        {
            public string MatchInstance { get; set; }
            public string Result { get; set; }
            public string Innings { get; set; }
            public string BatsmanRunsRange { get; set; }
            public string BatsmanBallsFacedRange { get; set; }
            public string BowlerBallsBowledRange { get; set; }
            public string BowlerWicketsRange { get; set; }
            public string BowlerRunsRange { get; set; }
            public string TeamOversRange { get; set; }
            public string TeamScoreRange { get; set; }
        }
        public class Moments
        {
            public string Entities { get; set; }
            public bool IsBigMoment { get; set; }
            public bool IsFunnyMoment { get; set; }
            public bool IsAudioPiece { get; set; }
            public bool IsMoments { get; set; }
        }
        public class S2ActionData
        {
            public string ID { get; set; }
            public string AttributeId_Level1 { get; set; }
            public string AttributeId_Level2 { get; set; }
            public string AttributeId_Level3 { get; set; }
            public string AttributeId_Level4 { get; set; }
            public string Emotion { get; set; }
            public string Entities { get; set; }
        }
        public class MatchDetail
        {
            public string MatchFormat { get; set; }
            public string MatchDate { get; set; }
            public string SeriesId { get; set; }
            public string VenueId { get; set; }
            public string Date { get; set; }
            public string Team1Id { get; set; }
            public string Team2Id { get; set; }
            public string MatchId { get; set; }
            public bool IsParentSeries { get; set; }
            public bool IsAsset { get; set; }
            public string ClearId { get; set; }
            public bool HasShortClip { get; set; }
            public bool IsAssetSearch { get; set; }
            public string LanguageId { get; set; }
            public string MatchStageId { get; set; }
            public string MatchStage { get; set; }
            public string CompTypeId { get; set; }
            public string GameTypeId { get; set; } //
            public bool MasterData { get; set; }

            [DefaultValue(1)]
            public int SportID { get; set; }

            public int RequestCount { get; set; }


        }
        public class FilteredEntityData
        {
            //public int Id { get; set; }
            public string EntityId { get; set; }
            public string EntityName { get; set; }
            public string EntityType { get; set; }
            //public int RoleId { get; set; }
            //public int LookupId { get; set; }
            //public int IsParentSeries { get; set; }
            public int IsSelectedEntity { get; set; }
        }
        public class SearchEntityRequestData
        {
            public int SportId { get; set; }
            public int EntityId { get; set; }
            public int EntityTypeId { get; set; }
            public int EntityRoleId { get; set; }
            public string EntityText { get; set; }
            public MatchDetail MatchDetails { get; set; }
            public dynamic playerDetails { get; set; }


        }
        public class SearchQueryModel
        {
            public string FieldName { get; set; }
            public string FieldType { get; set; }
            public string SearchText { get; set; }
            public string Operator { get; set; }
            public bool IsNested { get; set; }
            public Dictionary<string, string> NestedFields { get; set; }
            public bool UseWildcard { get; set; }
        }
        public class EntityList {
            public List<string> EntityIds { get; set; }
            public List<string> EntityNames { get; set; }
        }
        public class SaveSearchesRequestData
        {
            public int SportId { get; set; }
            public int? UserId { get; set; }
            public int SearchTypeId { get; set; }
        }
        public class SearchRequestData
        {
            public List<MatchDetail> MatchDetails { get; set; }
            public List<PlayerDetail> PlayerDetails { get; set; }
            public List<MatchSituation> MatchSituations { get; set; }
        }
        public class PlayerDetail
        {
            public string BatsmanID { get; set; }
            public bool BatsmanFours { get; set; }
            public bool BatsmanSixes { get; set; }
            public bool BastsmanBeaten { get; set; }
            public bool BatsmanEdged { get; set; }
            public bool BatsmanDots { get; set; }
            public bool BatsmanDismissal { get; set; }
            public bool BatsmanAppeal { get; set; }
            public string ShotType { get; set; }
            public string ShotZone { get; set; }
            public string DismissalType { get; set; }
            public string BowlerID { get; set; }
            public bool BowlerWides { get; set; }
            public bool BowlerNoBalls { get; set; }
            public bool BowlerBeaten { get; set; }
            public bool BowlerEdged { get; set; }
            public bool BowlerDots { get; set; }
            public string DeliveryType { get; set; }
            public string BowlingLength { get; set; }
            public string BowlingLine { get; set; }
            public bool BowlingRound { get; set; }
            public bool BowlingOver { get; set; }
            public bool BowlerDismissal { get; set; }
            public bool BowlerAppeal { get; set; }
            public bool BowlerFours { get; set; }
            public bool BowlerSixes { get; set; }
            public string FielderID { get; set; }
            public bool FielderCatch { get; set; }
            public bool FielderRunOut { get; set; }
            public bool FielderDrops { get; set; }
            public bool FielderStumping { get; set; }
            public string FieldingPosition { get; set; }
            public string RunsSaved { get; set; }
            public bool IsDefault { get; set; }
            public bool FielderMisFields { get; set; }
            public bool IsMasterData { get; set; }
            public string CurrentSelector { get; set; }
            public string BattingOrder { get; set; }
            public string BowlingArm { get; set; }
            public bool BowlerSpin { get; set; }
            public bool BowlerPace { get; set; }
        }

        public class SearchCricketResultTempData
        {
            public Int64 Id { get; set; }
            public string ClearId { get; set; }
            public string ClearId2 { get; set; }
            public string ClearId3 { get; set; }
            public string ClearId4 { get; set; }
            public string ClearId5 { get; set; }
            public string ClearId6 { get; set; }
            public string MediaId { get; set; }
            public string MediaId2 { get; set; }
            public string MediaId3 { get; set; }
            public string MediaId4 { get; set; }
            public string MediaId5 { get; set; }
            public string MediaId6 { get; set; }
            public string MatchId { get; set; }
            public string MarkIn { get; set; }
            public string MarkOut { get; set; }
            public string ShortMarkIn { get; set; }
            public string ShortMarkOut { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Duration { get; set; }
            public int BatsmanRuns { get; set; }
            public int BatsmanBallsFaced { get; set; }
            public int BowlerBallsBowled { get; set; }
            public int BowlerWickets { get; set; }
            public int BowlerRunsConceeded { get; set; }
            public int TeamOver { get; set; }
            public int TeamScore { get; set; }
            public string IsAsset { get; set; }
            public int MatchDate { get; set; }
            //
            public string ShotTypeId { get; set; }
            public string ShotType { get; set; }
            public string ShotZoneId { get; set; }
            public string ShotZone { get; set; }
            public string Dismissal { get; set; }
            public string DismissalId { get; set; }
            public string DeliveryTypeId { get; set; }
            public string DeliveryType { get; set; }
            public string BowlingLengthId { get; set; }
            public string BowlingLength { get; set; }
            public string BowlingLineId { get; set; }
            public string BowlingLine { get; set; }
            public string FielderPositionId { get; set; }
            public string FielderPosition { get; set; }
            public string BattingOrder { get; set; }
            public string BowlingArm { get; set; }
            public string LanguageId { get; set; }
        }

        public class SearchCricketResultData
        {
            public Int64 Id { get; set; }
            public string ClearId { get; set; }
            public string MediaId { get; set; }
            public string MatchId { get; set; }
            public string MarkIn { get; set; }
            public string MarkOut { get; set; }
            public string ShortMarkIn { get; set; }
            public string ShortMarkOut { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Duration { get; set; }
            public int BatsmanRuns { get; set; }
            public int BatsmanBallsFaced { get; set; }
            public int BowlerBallsBowled { get; set; }
            public int BowlerWickets { get; set; }
            public int BowlerRunsConceeded { get; set; }
            public int TeamOver { get; set; }
            public int TeamScore { get; set; }
            public string IsAsset { get; set; }
            public int MatchDate { get; set; }
            public string SType { get; set; }
            public string ShotTypeId { get; set; }
            public string ShotType { get; set; }
            public string ShotZoneId { get; set; }
            public string ShotZone { get; set; }
            public string Dismissal { get; set; }
            public string DismissalId { get; set; }
            public string DeliveryTypeId { get; set; }
            public string DeliveryType { get; set; }
            public string BowlingLengthId { get; set; }
            public string BowlingLength { get; set; }
            public string BowlingLineId { get; set; }
            public string BowlingLine { get; set; }
            public string FielderPositionId { get; set; }
            public string FielderPosition { get; set; }
            public string BattingOrder { get; set; }
            public string BowlingArm { get; set; }
            public string LanguageId { get; set; }
        }

    }

}


