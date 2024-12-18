﻿using SpaceRythm.Entities; 
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
using Microsoft.AspNetCore.Identity.Data;
using ResetPasswordRequest = SpaceRythm.Models.User.ResetPasswordRequest;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Org.BouncyCastle.Ocsp;

namespace SpaceRythm.Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _context;
        private readonly JwtSettings _jwtSettings;
        //private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(MyDbContext context, IOptions<JwtSettings> jwtOptions, HttpClient httpClient, TokenService tokenService, IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _jwtSettings = jwtOptions.Value;
            //_httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<CreateUserResponse> Create(CreateUserRequest req)
        {
            Console.WriteLine("req.Email" + req.Email);
            if (!IsValidEmail(req.Email))
            {
                throw new ArgumentException("Invalid email format.");
            }
            var user = new User(req);
            user.Password = _passwordHasher.HashPassword(user, req.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            Console.WriteLine("!!!new user created" + req);

            if (user.Id > 0)
            {
                Console.WriteLine("!!!if (user.Id > 0)");
                //string token = Jwt.GenerateToken(_jwtSettings, user);
                string token = _tokenService.GenerateToken(_jwtSettings, user);

                // Генерація токену підтвердження електронної пошти
                string emailConfirmationToken = await _tokenService.GenerateEmailConfirmationToken(user.Email);

                // Повертаємо результат без відправлення листа тут
                return new CreateUserResponse(user, token, false, emailConfirmationToken)
                {
                    IsEmailConfirmed = false,
                    EmailConfirmationToken = emailConfirmationToken // Можливо, вам знадобиться зберегти токен
                };
            }
            throw new Exception("User creation failed.");
        }
 
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            var userFromDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userFromDb == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Verify the token (you can use JwtSecurityTokenHandler to validate it)
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                // If the token is valid, confirm the email
                userFromDb.IsEmailConfirmed = true;
                _context.Users.Update(userFromDb);
                await _context.SaveChangesAsync();

                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid token." });
            }
        }
      
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest req)
        {
            Console.WriteLine($"Attempting to authenticate user with username: {req.Username}, password: {req.Password}");
            
            // Step 1: Find user by username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == req.Username);
            Console.WriteLine($"_context username: {user.Username}, password: {user.Password}");
            if (user == null)
            {
                Console.WriteLine($"User with username {req.Username} not found.");
                throw new Exception("Username or password incorrect");
            }

            // Step 2: Check if the user's email is confirmed
            if (!user.IsEmailConfirmed)
            {
                Console.WriteLine($"Email not confirmed for user {req.Username}.");
                throw new Exception("Email not confirmed. Please check your inbox and confirm your email.");
            }

            // Step 3: Verify password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, req.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                Console.WriteLine($"Invalid password for user {req.Username}.");
                throw new Exception("Username or password incorrect");
            }
            _httpContextAccessor.HttpContext.Items["User"] = user;

            // Step 4: Generate JWT token after successful verification
            Console.WriteLine($"Password verification succeeded for user {req.Username}. Generating JWT token...");
            string token = _tokenService.GenerateToken(_jwtSettings, user);
            Console.WriteLine($"JWT token generated successfully for user {req.Username}.");
            Console.WriteLine($"JWT token  {token}.");

            // Step 5: Return the authentication response
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

            var token = _tokenService.GenerateToken(_jwtSettings, user);

            return new AuthenticateResponse(user, token);
            
        }
        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            Console.WriteLine($"UserService ResetPasswordAsync");
            var user = await GetByEmail(email);
            if (user == null)
                return false;

            // Верифікація токена скидання паролю
            var isTokenValid = await _tokenService.VerifyPasswordResetToken(email, token);
            if (!isTokenValid)
                return false;

            // Хешування нового паролю та оновлення
            user.Password = _passwordHasher.HashPassword(user, newPassword);
         
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UpdateUserResponse> Update(string id, UpdateUserRequest req)
        {
            // Спробуйте конвертувати id у int
            if (!int.TryParse(id, out int userId))
            {
                throw new Exception("Invalid user ID");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                // Throw an error if the user is not found
                throw new Exception("User not found");
            }

            // Оновлення властивостей користувача
            user.Email = req.Email ?? user.Email;
            user.Username = req.Username ?? user.Username;
            user.ProfileImage = req.ProfileImage ?? user.ProfileImage;
            user.Biography = req.Biography ?? user.Biography;

            // You might want to hash the new password if it’s updated
            if (!string.IsNullOrEmpty(req.Password))
            {
                user.Password = PasswordHash.Hash(req.Password);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Step 4: Return the updated user information
            return new UpdateUserResponse
            {
                // Повернення оновленого користувача або іншої інформації
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

        public async Task<string> GetUserAvatarNameAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user?.ProfileImage;
        }


        // 4. Change Password
        public async Task ChangePassword(int userId, ChangePasswordRequest request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new KeyNotFoundException("User not found");

            // Assuming you have a method to validate the old password and hash the new one
            if (!VerifyPassword(request.OldPassword, user.Password))
            {
                throw new AppException("Old password is incorrect");
            }

            user.Password = HashPassword(request.NewPassword);
            await _context.SaveChangesAsync();
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            // Logic for verifying password
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

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
                //var response = await _httpClient.GetAsync(url);
                //if (response.IsSuccessStatusCode)
                //{
                //    var userData = await response.Content.ReadFromJsonAsync<FacebookUser>();
                //    return userData.Id; 
                //}
            }
            catch (HttpRequestException e)
            {
                // Log exception
                Console.WriteLine($"Request error: {e.Message}");
            }

            return null;
        }

        // 2. Скидання паролю
        public async Task<bool> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.UtcNow)
            {
                throw new Exception("Invalid or expired reset token");
            }

            // Зберігаємо новий пароль
            user.Password = HashPassword(request.NewPassword);
            user.PasswordResetToken = null; // Скидаємо токен після використання
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return true;
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
        public async Task<Playlist> CreatePlaylist(int userId, string name, string description)
        {
            // Створюємо новий плейлист з необхідними властивостями
            var playlist = new Playlist
            {
                UserId = userId,
                Title = name,
                Description = description,
                CreatedDate = DateTime.Now,
                IsPublic = true,
                PlaylistTracks = new List<PlaylistTracks>()
            };

            // Додаємо плейлист до контексту бази даних
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return playlist;
        }

        public async Task<Playlist> CreatePlaylist(int userId, string name, string description, List<int> trackIds)
        {
            var playlist = new Playlist
            {
                UserId = userId,
                Title = name,
                Description = description,
                CreatedDate = DateTime.Now,
                IsPublic = true,
                PlaylistTracks = trackIds.Select(trackId => new PlaylistTracks
                {
                    TrackId = trackId,
                    AddedDate = DateTime.Now
                }).ToList()
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return playlist;
        }

        public async Task<IEnumerable<Playlist>> GetPlaylists(int userId)
        {
            return await _context.Playlists
                .Where(p => p.UserId == userId)
                .Include(p => p.PlaylistTracks) // Включаємо пов’язані треки, якщо потрібно
                .ToListAsync();
        }

        public async Task AddTrackToPlaylist(int playlistId, int trackId)
        {
            // Перевіряємо, чи існує плейлист
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId);

            if (playlist == null)
                throw new Exception("Playlist not found");

            // Перевіряємо, чи трек вже додано до плейлисту
            var trackExists = playlist.PlaylistTracks.Any(pt => pt.TrackId == trackId);
            if (trackExists)
                throw new Exception("Track already exists in the playlist");

            // Додаємо трек до плейлиста
            var playlistTrack = new PlaylistTracks
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                AddedDate = DateTime.Now
            };

            playlist.PlaylistTracks.Add(playlistTrack);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTrackFromPlaylist(int playlistId, int trackId)
        {
            // Знаходимо запис плейлиста і треку
            var playlistTrack = await _context.PlaylistTracks
                .FirstOrDefaultAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);

            if (playlistTrack == null)
                throw new Exception("Track not found in the playlist");

            // Видаляємо трек з плейлиста
            _context.PlaylistTracks.Remove(playlistTrack);
            await _context.SaveChangesAsync();
        }
    }

    public class FacebookUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    
}
   

