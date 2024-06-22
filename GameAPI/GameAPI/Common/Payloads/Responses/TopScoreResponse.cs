using GameAPI.Dtos;

namespace GameAPI.Common.Payloads.Responses;

public class TopScoreResponse
{
    public LeaderBoardModel TopLeaderBoard { get; set; }
}