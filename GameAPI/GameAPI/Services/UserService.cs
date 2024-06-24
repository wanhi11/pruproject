using AutoMapper;
using GameAPI.Dtos;
using GameAPI.Entities;
using GameAPI.Exceptions;
using GameAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameAPI.Services;

public class UserService
{
    private readonly IRepository<User, int> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IRepository<User, int> userRepository, IMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public UserModel? Login(string username, string password)
    {
        var user = _userRepository.FindByCondition(u => u.Username.Equals(username) && u.Password.Equals(password))
            .FirstOrDefault();
        if (user is null)
        {
            throw new BadRequestException("Wrong username or password");
        }

        return _mapper.Map<UserModel>(user);
    }

    public async Task<bool> Register(string username, string password)
    {
        var existeduUser = _userRepository.FindByCondition(u => u.Username.Equals(username)).FirstOrDefault();
        if (existeduUser is not null)
        {
            return false;
        }
        else
        {
            await _userRepository.AddAsync(new User()
            {
                Password = password,
                Username = username
            });
            await _userRepository.Commit();
            return true;
        }
    }

    public async Task<UserModel> FindById(int id)
    {
        var user = await _userRepository.FindByCondition(x => x.Userid == id).FirstOrDefaultAsync();
        if (user is null)
        {
            throw new BadRequestException("User does not exist");
        }

        return _mapper.Map<UserModel>(user);
    }
}