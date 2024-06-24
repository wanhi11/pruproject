using AutoMapper;
using GameAPI.Common;
using GameAPI.Common.Payloads.Requests;
using GameAPI.Common.Payloads.Responses;
using GameAPI.Dtos;
using GameAPI.Exceptions;
using GameAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GameAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class LeaderBoardController : Controller
{
    private readonly LeaderBoardService _leaderBoardService;
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public LeaderBoardController(LeaderBoardService leaderBoardService, UserService userService,IMapper mapper)
    {
        _leaderBoardService = leaderBoardService;
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    [Route("{username}")]
    public IActionResult GetTopAsync([FromRoute]string username)
    {
        var result =  _leaderBoardService.GetTopScore(username);
        return Ok(ApiResult<TopScoreResponse>.Succeed(new TopScoreResponse()
        {
            TopLeaderBoard = result!
        }));
    }

    [HttpPost]
    public async Task<IActionResult> SaveNewRecord([FromBody] AddNewRecordRequest req)
    {
        var result = await _leaderBoardService.AddRecord(req.UserName, req.Time);
        if (!result)
        {
            throw new BadRequestException("It is not the new record");
        }

        return Ok(ApiResult<AddNewRecordResponse>.Succeed(new AddNewRecordResponse()
        {
            TopScore = new LeaderBoardModel()
            {
                Time = req.Time
            }
        }));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTopPlayer()
    {
        var scoreResult = await _leaderBoardService.GetTopPlayer();
        var playerInfor = await _userService.FindById(scoreResult.UserId);
        return Ok(ApiResult<TopPLayerDetailsResponse>.Succeed(new TopPLayerDetailsResponse()
        {
            PlayerName = playerInfor.Username,
            PlayerScore = scoreResult.Time.ToString()
        }));
    }
}