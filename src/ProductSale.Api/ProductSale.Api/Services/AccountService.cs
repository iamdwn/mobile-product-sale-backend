using AutoMapper;
using ProductSale.Api.Services.Interfaces;
using ProductSale.Api.Services.Mapper;
using ProductSale.Data.Base;
using ProductSale.Data.DTO.RequestModel;
using ProductSale.Data.DTO.ResponseModel;
using ProductSale.Data.Models;
using System.Net;

namespace ProductSale.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> getUserByUserName(string userName)
        {
            if (userName == null)
            {
                return ResponseUtils.Error("Request fails", "Username is required", HttpStatusCode.BadRequest);
            }

            User user = await Task.FromResult(_unitOfWork.UserRepository.Get(c => c.Username.Equals(userName)).SingleOrDefault());
            AccountDTO result = _mapper.Map<AccountDTO>(user);


            return ResponseUtils.GetObject(result, "Account retrieve successfully", HttpStatusCode.OK, null);

        }

        public async Task<ResponseDTO> Login(string userName, string password)
        {
            string hashedPassword = PasswordEncoder.HashPassword(password);

            User user = _unitOfWork.UserRepository.Get(c => c.Username.Equals(userName) && c.PasswordHash.Equals(hashedPassword)).SingleOrDefault();

            if (user == null)
            {
                return ResponseUtils.Error("Request fails", "Username or password is incorrect", HttpStatusCode.BadRequest);
            }

            return ResponseUtils.GetObject("Request accept", "Successfully login", HttpStatusCode.OK, null);
        }

        public async Task<ResponseDTO> register(Register register)
        {
            if (isAccountExisted(register.Username, register.Email))
            {
                return ResponseUtils.Error("Request fails", "Username or email is already existed", HttpStatusCode.BadRequest);
            }

            User user = _mapper.Map<User>(register);
            user.PasswordHash = PasswordEncoder.HashPassword(user.PasswordHash);
            user.Role = "USER";
            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();

            return ResponseUtils.GetObject("Request accept", "Successfully register", HttpStatusCode.OK, null);
        }

        private bool isAccountExisted(string username, string email)
        {
            User user = _unitOfWork.UserRepository.Get(c => c.Username.Equals(username) || c.Email.Equals(email)).SingleOrDefault();

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}
