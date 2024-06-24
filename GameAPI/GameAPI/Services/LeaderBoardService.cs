using AutoMapper;
using GameAPI.Dtos;
using GameAPI.Entities;
using GameAPI.Exceptions;
using GameAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameAPI.Services;

public class LeaderBoardService
{
    private readonly IRepository<LeaderBoard, int> _leaderBoardRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<User, int> _userRepositoty;

    public LeaderBoardService(IRepository<LeaderBoard, int> leaderBoardRepository, IRepository<User,int> useRepository, IMapper mapper)
    {
        _leaderBoardRepository = leaderBoardRepository;
        _mapper = mapper;
        _userRepositoty = useRepository;
    }

    public LeaderBoardModel? GetTopScore(string username)
    {
        var player = _userRepositoty.FindByCondition(u => u.Username.Equals(username)).FirstOrDefault();
        if (player is null)
        {
            throw new BadRequestException("Player does not existed");
        }

        var top = _leaderBoardRepository.FindByCondition(t => t.Userid.Equals(player.Userid))
            .OrderBy(x => x.Time)
            .FirstOrDefault();
        if (top is null) throw new BadRequestException("This username have not played before");
        return _mapper.Map<LeaderBoardModel>(top);
    }

    public async Task<bool> AddRecord(string username, double time)
    {
        var player = _userRepositoty.FindByCondition(u => u.Username.Equals(username)).FirstOrDefault();
        if (player is null)
        {
            throw new BadRequestException("Player does not existed");
        }
        var top = _leaderBoardRepository.FindByCondition(t => t.Userid.Equals(player.Userid)).OrderBy(x => x.Time).FirstOrDefault();
        if (top is null || top.Time > time)
        {
            await _leaderBoardRepository.AddAsync(_mapper.Map<LeaderBoard>(new LeaderBoardModel()
            {
                Time = time,
                UserId = player.Userid
            }));
            await _leaderBoardRepository.Commit();  
            
            return true;
        }

        return false;
    }

    public async Task<LeaderBoardModel> GetTopPlayer()
    {
        var topLeaderBoard = await _leaderBoardRepository.GetAll().OrderBy(x => x.Time).FirstOrDefaultAsync();
        if (topLeaderBoard is null)
        {
            throw new BadRequestException("There is no one played before");
        }

        return _mapper.Map<LeaderBoardModel>(topLeaderBoard);
    }
}