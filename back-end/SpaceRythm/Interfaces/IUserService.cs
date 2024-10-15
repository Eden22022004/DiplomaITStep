using SpaceRythm.DTOs;
using SpaceRythm.Entities;
using SpaceRythm.Models.User;
using System.Security.Claims;



namespace SpaceRythm.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(string id);

    Task<User> GetByUsername(string username);

    Task<User> GetByEmail(string email);

    Task<CreateUserResponse> Create(CreateUserRequest req);

    Task<AuthenticateResponse> Authenticate(AuthenticateRequest req);
    Task<AuthenticateResponse> AuthenticateWithOAuth(ClaimsPrincipal claimsPrincipal);

    Task<UpdateUserResponse> Update(string id, UpdateUserRequest req);

    Task<string> UploadAvatar(int userId, string avatarPath);
    Task FollowUser(int followerId, int followeeId);
    Task<IEnumerable<FollowerDto>> GetFollowers(int userId);
    Task ChangePassword(int userId, ChangePasswordRequest request);

    //Task<bool> Delete(string id);
    Task <bool> Delete(int id);
    //Task<bool> VerifyFacebookRequest(string accessToken);
    Task<string> VerifyFacebookRequest(string accessToken);
}

