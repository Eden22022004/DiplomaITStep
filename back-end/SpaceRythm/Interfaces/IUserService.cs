using SpaceRythm.DTOs;
using SpaceRythm.Entities;
using SpaceRythm.Models.User;



namespace SpaceRythm.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(string id);

    Task<User> GetByUsername(string username);

    Task<User> GetByEmail(string email);

    Task<CreateUserResponse> Create(CreateUserRequest req);

    Task<AuthenticateResponse> Authenticate(AuthenticateRequest req);

    Task<UpdateUserResponse> Update(string id, UpdateUserRequest req);

    Task<string> UploadAvatar(int userId, string avatarPath);
    Task FollowUser(int followerId, int followeeId);
    Task<IEnumerable<FollowerDto>> GetFollowers(int userId);
    Task ChangePassword(int userId, ChangePasswordRequest request);

    Task Delete(int id);
}

