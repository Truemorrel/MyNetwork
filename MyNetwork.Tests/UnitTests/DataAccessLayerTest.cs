using Microsoft.EntityFrameworkCore;
using MyNetwork.Data;
using MyNetwork.Models.Users;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using MyNetwork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using AutoFixture;

namespace RazorPagesTestSample.Tests.UnitTests
{
	public class DataAccessLayerTest
    {
		private IMapper _mapper;
		private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public DataAccessLayerTest(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = signInManager;
			_mapper = mapper;

		}
    }
}
