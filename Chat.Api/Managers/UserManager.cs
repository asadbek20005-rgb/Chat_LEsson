using Chat.Api.ConvertExtensions;
using Chat.Api.Dtos;
using Chat.Api.Entities;
using Chat.Api.Exceptions;
using Chat.Api.Helper;
using Chat.Api.Jwt;
using Chat.Api.MemoryCache;
using Chat.Api.Models;
using Chat.Api.Models.UserModel;
using Chat.Api.Models.UserModels;
using Chat.Api.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Chat.Api.Managers
{
    public class UserManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtManager _jwtManager;
        private readonly MemoryCacheManager _memoryCacheManager;
        public UserManager(IUnitOfWork unitOfWork, JwtManager jwtManager, MemoryCacheManager memoryCache)
        {
            _unitOfWork = unitOfWork;
            _jwtManager = jwtManager;
            _memoryCacheManager = memoryCache;
        }




        public async Task<UserDto> RegisterUser(CreateUserModel model)
        {
            await CheckUserExist(model.Username);
            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Role = Constants.User
            };
            var checking = model.Gender != Constants.Male || model.Gender != Constants.Female;
            if (checking)
                newUser.Gender = Constants.Male;
            string hashedPassword = await PasswordHashing(newUser, model.Password);
            newUser.PasswordHash = hashedPassword;


            await _unitOfWork.UserRepository.AddUser(newUser);
            return newUser.ParseToDto();
        }
        public async Task<string> LoginUser(LoginUserModel model)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsername(model.Username);
            if (user != null && user.Username == Constants.AdminUsername)
            {
                var validationPassword = await PasswordVerification(user, Constants.AdminPassword);
                return _jwtManager.GenerateToken(user);
            }

            if (user != null && user.Username == model.Username)
            {
                var validationPassword = await PasswordVerification(user, model.Password);
                return _jwtManager.GenerateToken(user);
            }
            throw new Exception("Login was not done successfully");
        }

        public async Task<UserDto> GetUserById(Guid userId)
        {
            var userDtos = _memoryCacheManager.GetDtos(Constants.CacheKeyUsers);
            if (userDtos is not null)
            {
                List<UserDto> users = (List<UserDto>)userDtos;
                var userDto = users.SingleOrDefault(u => u.Id == userId);
                if (userDto == null)
                    throw new UserNotFound();
                return userDto;
            }

            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if (user == null)
                throw new UserNotFound();

            await Set();
            return user.ParseToDto();

        }
        public async Task<List<UserDto>> GetAllUsers()
        {

            var userss = _memoryCacheManager.GetDtos(Constants.CacheKeyUsers);
            if (userss is not null)
            {
                List<UserDto> userDtos = (List<UserDto>)userss;
                return userDtos;
            }

            var users = await _unitOfWork.UserRepository.GetAllUsers();
            await Set();
            return users.ParseToDtos();

        }



        private async Task CheckUserExist(string username)
        {
            var checkingForUsernameExist = await _unitOfWork.UserRepository.GetUserByUsername(username);
            if (checkingForUsernameExist != null)
                throw new UserExist($"User with {username} is already exist!");
        }
        private async Task<string> PasswordHashing(User user, string password)
        {
            var hashingPassword = new PasswordHasher<User>().HashPassword(user, password);
            if (hashingPassword != null)
                return hashingPassword;
            throw new Exception($"The password with {password} is not hashed!");
        }
        private async Task<PasswordVerificationResult> PasswordVerification(User user, string passoword)
        {
            var validationPassword = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, passoword);
            if (validationPassword == PasswordVerificationResult.Failed)
                throw new Exception($"The validation password is failed!");
            return validationPassword;
        }



        public async Task<byte[]> AddOrUpdatePhoto(Guid userId, IFormFile file)
        {

            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            StaticHelper.IsFile(file);
            var data = await StaticHelper.GetBytes(file);
            user.PhotoData = data;
            await _unitOfWork.UserRepository.UpdateUser(user);
            await Set();
            return data;
        }


        public async Task<string> DeleteUser(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if (user == null)
                throw new UserNotFound();
            await _unitOfWork.UserRepository.DeleteUser(userId);
            await Set();
            return "User was deleted successfully";
        }

        public async Task<UserDto> UpdateGeneralUserData(Guid userId, Models.UserModels.UpdateUserGeneralDataModel updateUser)
        {
            var check = false;
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if (user == null)
            { throw new UserNotFound(); }

            if (!string.IsNullOrEmpty(updateUser.FirstName))
            {
                user.FirstName = updateUser.FirstName;
                check = true;
            }
            if (!string.IsNullOrEmpty(updateUser.LastName))
            {
                user.LastName = updateUser.LastName;
                check = true;
            }
            if (!string.IsNullOrEmpty(updateUser.Age))
            {
                try
                {
                    byte age = byte.Parse(updateUser.Age);

                    user.Age = age;
                    check = true;

                }
                catch (Exception e)
                {
                    throw new Exception("Age must be number");
                }
            }
            if (!string.IsNullOrEmpty(updateUser.Gender))
            {
                user.Gender = updateUser.Gender;
                check = true;
            }
            if (check)
                await _unitOfWork.UserRepository.UpdateUser(user);

            return user.ParseToDto();
        }

        public async Task<UserDto> UpdateUserUsername(UpdateUsernameModel usernameModel)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsername(usernameModel.Username);

            if (user == null)
                throw new UserExist($" The User with {user.Username} is not exist");

            user.Username = usernameModel.Username;
            await _unitOfWork.UserRepository.UpdateUser(user);
            return user.ParseToDto();
        }

        public async Task<UserDto> UpdateUserBio(Guid userId, UpdateUserBioModel userBioModel)
        {
            var user = await _unitOfWork.UserRepository.GetUserById(userId);
            if (user == null)
                throw new UserExist($"The User with{user.Username} is not exist");

            user.Bio = userBioModel.Bio;
            await _unitOfWork.UserRepository.UpdateUser(user);
            return user.ParseToDto();
        }


        private async Task Set()
        {
            var users = await _unitOfWork.UserRepository.GetAllUsers();
            var userDtos = users.ParseToDtos();
            _memoryCacheManager.UpdateValue(Constants.CacheKeyUsers, userDtos);
        }

    }
}