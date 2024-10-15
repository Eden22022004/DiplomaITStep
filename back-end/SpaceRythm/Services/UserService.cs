using SpaceRythm.Entities; 
using SpaceRythm.Interfaces;
using SpaceRythm.Models.User;
using SpaceRythm.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpaceRythm.Models;
using SpaceRythm.Util;
using Microsoft.Extensions.Options;
using SpaceRythm.Exceptions;
using SpaceRythm.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;

namespace SpaceRythm.Services
{

    public class UserService : IUserService
    {
        private readonly MyDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly HttpClient _httpClient;

        public UserService(MyDbContext context, IOptions<JwtSettings> jwtOptions, HttpClient httpClient)
        {
            _context = context;
            _jwtSettings = jwtOptions.Value;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<CreateUserResponse> Create(CreateUserRequest req)
        {
            var user = new User(req);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            string token = Jwt.GenerateToken(_jwtSettings, user);

            return new CreateUserResponse(user, token); 
           
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest req)
        {
            Console.WriteLine($"Attempting to authenticate user with username: {req.Username}");


            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == req.Username);
            if (user == null)
            {
                Console.WriteLine($"User with username {req.Username} not found.");
                throw new Exception("User not found");
            }

            if (!PasswordHash.Verify(req.Password, user.Password))
            {
                Console.WriteLine($"Invalid password for user {req.Username}.");
                throw new Exception("Invalid password");
            }
            Console.WriteLine($"Password verification succeeded for user {req.Username}. Generating JWT token...");
            string token = Jwt.GenerateToken(_jwtSettings, user);
            Console.WriteLine($"JWT token generated successfully for user {req.Username}.");
            return new AuthenticateResponse(user, token);
        }
        public async Task<AuthenticateResponse> AuthenticateWithOAuth(ClaimsPrincipal claimsPrincipal)
        {
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            var username = claimsPrincipal.FindFirstValue(ClaimTypes.Name);

            var user = await GetByEmail(email);

            if (user == null)
            {
                var createUserRequest = new CreateUserRequest
                {
                    Email = email,
                    Username = username,
                };

                var createUserResponse = await Create(createUserRequest);

                user = new User
                {
                    Id = createUserResponse.Id,
                    Email = createUserResponse.Email,
                    Username = createUserResponse.Username,
                    ProfileImage = createUserResponse.ProfileImage,
                    Biography = createUserResponse.Biography,
                    DateJoined = createUserResponse.DateJoined,
                    IsEmailConfirmed = createUserResponse.IsEmailConfirmed,
                    SongsLiked = new List<SongLiked>(),
                    ArtistsLiked = new List<ArtistLiked>(),
                    CategoriesLiked = new List<CategoryLiked>()
                };
            }

            var token  = Jwt.GenerateToken(_jwtSettings, user);

            return new AuthenticateResponse(user, token);
            
        }

        //public async Task<AuthenticateResponse> AuthenticateWithGoogle(ClaimsPrincipal claimsPrincipal)
        //{
        //    var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        //    var username = claimsPrincipal.FindFirstValue(ClaimTypes.Name);

        //    // Step 1: Check if the user exists by email
        //    var user = await GetByEmail(email);
        //    if (user == null)
        //    {
        //        // If the user doesn't exist, create a new one
        //        var createUserRequest = new CreateUserRequest
        //        {
        //            Email = email,
        //            Username = username
        //            // Add other necessary fields as required
        //        };

        //        // CreateUserResponse will be returned instead of User
        //        var createUserResponse = await Create(createUserRequest);

        //        // Create a User object for the AuthenticateResponse
        //        user = new User
        //        {
        //            Id = createUserResponse.Id,
        //            Email = createUserResponse.Email,
        //            Username = createUserResponse.Username,
        //            ProfileImage = createUserResponse.ProfileImage,
        //            Biography = createUserResponse.Biography,
        //            DateJoined = createUserResponse.DateJoined,
        //            IsEmailConfirmed = createUserResponse.IsEmailConfirmed,
        //            SongsLiked = new List<SongLiked>(), 
        //            ArtistsLiked = new List<ArtistLiked>(), 
        //            CategoriesLiked = new List<CategoryLiked>() 
        //        };
        //    }

        //    // Step 2: Generate JWT token for the user
        //    string token = Jwt.GenerateToken(_jwtSettings, user);

        //    // Step 3: Return AuthenticateResponse with user data and token
        //    return new AuthenticateResponse(user, token);
        //}

        public async Task<UpdateUserResponse> Update(string id, UpdateUserRequest req)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            user.Email = req.Email ?? user.Email; 
            user.Username = req.Username ?? user.Username; 
            user.ProfileImage = req.ProfileImage ?? user.ProfileImage;
            user.Biography = req.Biography ?? user.Biography;

            if (!string.IsNullOrEmpty(req.Password))
            {
                user.Password = PasswordHash.Hash(req.Password);
            }


            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UpdateUserResponse
            {
                //Id = user.Id.ToString(),
                //Email = user.Email,
                //Username = user.Username,
                //ProfileImage = user.ProfileImage,
                //Biography = user.Biography,
                //DateJoined = user.DateJoined,
                //IsEmailConfirmed = user.IsEmailConfirmed
            };
        }

        public async Task<string> UploadAvatar(int userId, string avatarPath)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.ProfileImage = avatarPath;
            await _context.SaveChangesAsync();

            return user.ProfileImage; 
        }

        // 2. Follow User
        public async Task FollowUser(int followerId, int followedUserId)
        {
            var follower = await _context.Users.FindAsync(followerId);
            var followedUser = await _context.Users.FindAsync(followedUserId);

            if (follower == null || followedUser == null)
                throw new KeyNotFoundException("User not found");


            var alreadyFollowing = await _context.Followers
                .AnyAsync(f => f.UserId == followerId && f.FollowedUserId == followedUserId);

            if (!alreadyFollowing)
            {
                var follow = new Follower
                {
                    UserId = followerId,
                    FollowedUserId = followedUserId,
                    FollowDate = DateTime.UtcNow 
                };

                _context.Followers.Add(follow);
                await _context.SaveChangesAsync();
            }
        }

        // 3. Get Followers
        public async Task<IEnumerable<FollowerDto>> GetFollowers(int followedUserId)
        {
            var followers = await _context.Followers
                .Where(f => f.FollowedUserId == followedUserId)
                .Select(f => new FollowerDto
                {
                    Id = f.UserId,       
                    Username = f.User.Username,   
                    Avatar = f.User.ProfileImage,  
                    FollowDate = f.FollowDate       
                })
                .ToListAsync();

            return followers;
        }

        // 4. Change Password
        public async Task ChangePassword(int userId, ChangePasswordRequest request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            if (!VerifyPassword(request.OldPassword, user.Password))
            {
                throw new AppException("Old password is incorrect");
            }

            user.Password = HashPassword(request.NewPassword);
            await _context.SaveChangesAsync();
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        //public async Task Delete(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    if (user != null)
        //    {
        //        _context.Users.Remove(user);
        //        await _context.SaveChangesAsync();
        //    }

        //}

        public async Task<bool> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> VerifyFacebookRequest(string accessToken)
        {
            string appId = "526202996837238";
            string url = $"https://graph.facebook.com/me?access_token={accessToken}&fields=id,name,email";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var userData = await response.Content.ReadFromJsonAsync<FacebookUser>();
                    return userData.Id; 
                }
            }
            catch (HttpRequestException e)
            {
                // Log exception
                Console.WriteLine($"Request error: {e.Message}");
            }

            return null;
        }
    }

    public class FacebookUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
   

