using GameAPI.Dtos;

namespace GameAPI.Common.Payloads.Responses;

public class AddNewRecordResponse
{
    public LeaderBoardModel TopScore { get; set; }
}