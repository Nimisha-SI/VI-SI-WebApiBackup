using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApis.Model
{
    public class SearchCricketData
    {
        public string Id { get; set; }
        public string RId { get; set; }
        public string MarkIn { get; set; }
        public string MarkOut { get; set; }
        public string ShortMarkIn { get; set; }
        public string ShortMarkOut { get; set; }
        public string MediaId { get; set; }
        public string ClearId { get; set; }
        public string QClearId { get; set; }
        public string ClearId2 { get; set; }
        public string QClearId2 { get; set; }
        public string ClearId3 { get; set; }
        public string QClearId3 { get; set; }
        public string ClearId4 { get; set; }
        public string QClearId4 { get; set; }
        public string ClearId5 { get; set; }
        public string QClearId5 { get; set; }
        public string ClearId6 { get; set; }
        public string QClearId6 { get; set; }
        public string MediaId2 { get; set; }
        public string MediaId3 { get; set; }
        public string MediaId4 { get; set; }
        public string MediaId5 { get; set; }
        public string MediaId6 { get; set; }
        public string MatchId { get; set; }
        public string CompType { get; set; }
        public string VenueId { get; set; }
        public string Venue { get; set; }
        public string ParentSeriesId { get; set; }
        public string SeriesId { get; set; }
        public string Series { get; set; }
        public int MatchDate { get; set; }
        public string Team1Id { get; set; }
        public string Team1 { get; set; }
        public string Team2Id { get; set; }
        public string Team2 { get; set; }
        public string BatsmanId { get; set; }
        public string Batsman { get; set; }
        public string BowlerId { get; set; }
        public string Bowler { get; set; }
        public string FielderId { get; set; }
        public string Fielder { get; set; }
        public string ShotTypeId { get; set; }
        public string ShotType { get; set; }
        public string ShotZoneId { get; set; }
        public string ShotZone { get; set; }
        public string DeliveryTypeId { get; set; }
        public string DeliveryType { get; set; }
        public string DismissalId { get; set; }
        public string Dismissal { get; set; }
        public string BowlingLineId { get; set; }
        public string BowlingLine { get; set; }
        public string BowlingLengthId { get; set; }
        public string BowlingLength { get; set; }
        public string OverNo { get; set; }
        public string BallNo { get; set; }
        public string Runs { get; set; }
        public string IsFour { get; set; }
        public string IsSix { get; set; }
        public string IsDot { get; set; }
        public string IsBeaten { get; set; }
        public string IsEdged { get; set; }
        public string IsWide { get; set; }
        public string IsNoBall { get; set; }
        public string IsOverTheWicket { get; set; }
        public string IsRoundTheWicket { get; set; }
        public string IsCatch { get; set; }
        public string IsRunOut { get; set; }
        public string IsDropped { get; set; }
        public string FielderPositionId { get; set; }
        public string FielderPosition { get; set; }
        public string SType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RunsSaved { get; set; }
        public string RunsConceeded { get; set; }
        public string Duration { get; set; }
        public string Innings { get; set; }
        public string Balls { get; set; }
        public string Skill { get; set; }
        public string MatchInstance { get; set; }
        public int BatsmanRuns { get; set; }
        public int BatsmanBallsFaced { get; set; }

        public string MatchResult { get; set; }
        public string Winner { get; set; }
        public string Loser { get; set; }
        public int BowlerBallsBowled { get; set; }
        public int BowlerRunsConceeded { get; set; }
        public int BowlerWickets { get; set; }

        public int TeamScore { get; set; }
        public int TeamOver { get; set; }

        public string WinnerPlayer { get; set; }
        public string LoserPlayer { get; set; }
        public string WinningFielder { get; set; }

        public string IsPowerPlayOver { get; set; }
        public string is_middle { get; set; }
        public string is_death { get; set; }
        public string is_lastover { get; set; }
        public string is_lastBall { get; set; }

        public string IsAsset { get; set; }
        public string IsWicket { get; set; }
        public string IsAppeal { get; set; }
        public string HasShortClip { get; set; }
        public string EventId { get; set; }
        public string EventText { get; set; }
        public string IsTagged { get; set; }
        public string Match { get; set; }
        public string ParentSeriesName { get; set; }
        public string IsMisField { get; set; }
        public string AssetTypeId { get; set; }
        public string LanguageId { get; set; }
        public string Language { get; set; }
        public string BattingOrder { get; set; }
        public string BowlingArm { get; set; }
        public CompletionField CF_Batsman { get; set; }
        public CompletionField CF_Bowler { get; set; }
        public string batsmanddl { get; set; }
        public string bowlerddl { get; set; }


    }
    public class SearchS2Data
    {
        public string Id { get; set; }
        public Int64 RId { get; set; }
        public string MatchId { get; set; }
        public string Match { get; set; }
        public string MarkIn { get; set; }
        public string MarkOut { get; set; }
        public string ClearId { get; set; }
        public string QClearId { get; set; }
        public string MediaId { get; set; }
        public string CompType { get; set; }
        public string VenueId { get; set; }
        public string Venue { get; set; }
        public string SeriesId { get; set; }
        public string Series { get; set; }
        public int MatchDate { get; set; }
        public string Team1Id { get; set; }
        public string Team1 { get; set; }
        public string Team2Id { get; set; }
        public string Team2 { get; set; }
        public string MatchStage { get; set; }
        public string MatchStageId { get; set; }
        public string Attribute_Id_Level1 { get; set; }
        public string Attribute_Name_Level1 { get; set; }
        public string Attribute_Id_Level2 { get; set; }
        public string Attribute_Name_Level2 { get; set; }
        public string Attribute_Id_Level3 { get; set; }
        public string Attribute_Name_Level3 { get; set; }
        public string Attribute_Id_Level4 { get; set; }
        public string Attribute_Name_Level4 { get; set; }
        public string ParentAttributeId { get; set; }
        public string ParentAttributeName { get; set; }
        public string EmotionId { get; set; }
        public string EmotionName { get; set; }
        public string TagtypeId { get; set; }
        public string TagType { get; set; }
        public string EntityName_1 { get; set; }
        public string EntityName_2 { get; set; }
        public string EntityName_3 { get; set; }
        public string EntityName_4 { get; set; }
        public string EntityName_5 { get; set; }
        public string EntityId_1 { get; set; }
        public string EntityId_2 { get; set; }
        public string EntityId_3 { get; set; }
        public string EntityId_4 { get; set; }
        public string EntityId_5 { get; set; }
        public string ParentSeriesId { get; set; }
        public string IsAsset { get; set; }
        public string IsBigMoment { get; set; }
        public string IsFunnyMoment { get; set; }
        public string IsAudioPiece { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string TaggedEntityIds { get; set; }
        public string ClearId2 { get; set; }
        public string MediaId2 { get; set; }
        public string IsTagged { get; set; }
        public string EventId { get; set; }
        public string EventText { get; set; }
        public string LanguageId { get; set; }
        public string Language { get; set; }
        [DefaultValue(1)]
        public int SportId { get; set; }
        public string SType { get; set; }

    }

    public class ddlValue
    {
        public string Id { get; set; }
        public string Key { get; set; }
    }
    public class ResponseResult
    {
      // public string Source { get; set; }
       // public string Score { get; set; }
        public string Text { get; set; }
       // public int Id { get; set; }
    }
}
