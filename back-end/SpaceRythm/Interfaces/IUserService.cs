using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using SpaceRythm.DTOs;
using SpaceRythm.Entities;
using SpaceRythm.Models.User;
using System.Security.Claims;
using ResetPasswordRequest = SpaceRythm.Models.User.ResetPasswordRequest;



namespace SpaceRythm.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(int id);

    Task<User> GetByUsername(string username);

    Task<User> GetByEmail(string email);

    Task<CreateUserResponse> Create(CreateUserRequest req);

    Task<AuthenticateResponse> Authenticate(AuthenticateRequest req);
    Task<AuthenticateResponse> AuthenticateWithOAuth(ClaimsPrincipal claimsPrincipal);
    Task<UpdateUserResponse> Update(string id, UpdateUserRequest req);

    Task<string> UploadAvatar(int userId, string avatarPath);
    Task<string?> GetUserAvatarNameAsync(int userId);
    Task FollowUser(int followerId, int followeeId);
    Task<IEnumerable<FollowerDto>> GetFollowers(int userId);
    Task ChangePassword(int userId, ChangePasswordRequest request);
    //Task<bool> Delete(string id);
    Task <bool> Delete(int id);
    //Task<bool> VerifyFacebookRequest(string accessToken);
    Task<string> VerifyFacebookRequest(string accessToken);
    Task<IdentityResult> ConfirmEmailAsync(User user, string token);
    Task<bool> ResetPassword(ResetPasswordRequest request);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    Task<Playlist> CreatePlaylist(int userId, string name, string description);
    Task<Playlist> CreatePlaylist(int userId, string name, string description, List<int> trackIds);
    Task<IEnumerable<Playlist>> GetPlaylists(int userId);
    Task AddTrackToPlaylist(int playlistId, int trackId);
    Task RemoveTrackFromPlaylist(int playlistId, int trackId);

}

