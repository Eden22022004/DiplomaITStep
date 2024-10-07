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

namespace SpaceRythm.Services
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public UserService(MyDbContext context, IOptions<JwtSettings> jwtOptions)
        {
            _context = context;
            _jwtSettings = jwtOptions.Value;
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

            return new CreateUserResponse(user, token); // Pass the user and token to the response
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest req)
        {
            Console.WriteLine($"Attempting to authenticate user with username: {req.Username}");

            // Step 1: Find user by email or username
            //var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == req.Email || u.Username == req.Username);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == req.Username);
            if (user == null)
            {
                Console.WriteLine($"User with username {req.Username} not found.");
                // Return null or throw an error if the user is not found
                throw new Exception("User not found");
            }

            // Step 2: Verify the password
            if (!PasswordHash.Verify(req.Password, user.Password))
            {
                Console.WriteLine($"Invalid password for user {req.Username}.");
                // Password is incorrect
                throw new Exception("Invalid password");
            }
            Console.WriteLine($"Password verification succeeded for user {req.Username}. Generating JWT token...");
            // Step 3: Generate JWT token
            string token = Jwt.GenerateToken(_jwtSettings, user);
            Console.WriteLine($"JWT token generated successfully for user {req.Username}.");
            // Step 4: Return AuthenticateResponse with user data and token
            return new AuthenticateResponse(user, token);
        }

        public async Task<UpdateUserResponse> Update(string id, UpdateUserRequest req)
        {
            // Step 1: Find user by ID
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                // Throw an error if the user is not found
                throw new Exception("User not found");
            }

            // Step 2: Update user properties with new values
            user.Email = req.Email ?? user.Email; // If a new email is provided, update it
            user.Username = req.Username ?? user.Username; // Same for username
            user.ProfileImage = req.ProfileImage ?? user.ProfileImage;
            user.Biography = req.Biography ?? user.Biography;

            // You might want to hash the new password if it’s updated
            if (!string.IsNullOrEmpty(req.Password))
            {
                user.Password = PasswordHash.Hash(req.Password);
            }

            // Step 3: Save changes to the database
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            // Step 4: Return the updated user information
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

            return user.ProfileImage;  // Возвращаем путь к аватару
        }

        // 2. Follow User
        public async Task FollowUser(int followerId, int followedUserId)
        {
            var follower = await _context.Users.FindAsync(followerId);
            var followedUser = await _context.Users.FindAsync(followedUserId);

            if (follower == null || followedUser == null)
                throw new KeyNotFoundException("User not found");

            // Перевірка, чи підписник вже підписаний на користувача
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
            // Отримуємо всіх підписників для конкретного користувача і додаткову інформацію про них
            var followers = await _context.Followers
                .Where(f => f.FollowedUserId == followedUserId)
                .Select(f => new FollowerDto
                {
                    Id = f.UserId,                     // ID підписника
                    Username = f.User.Username,        // Ім'я користувача підписника
                    Avatar = f.User.ProfileImage,      // URL аватара підписника
                    FollowDate = f.FollowDate          // Дата підписки
                })
                .ToListAsync();

            return followers;
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

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

        }
    }
   
}
