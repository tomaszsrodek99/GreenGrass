using AutoMapper;
using GreenGrassAPI.Context;
using GreenGrassAPI.Interfaces;
using GreenGrassAPI.Models;

namespace GreenGrassAPI.Repositories
{
    public class FriendRepository : GenericRepository<UserFriends>, IFriendsRepository
    {
        private readonly GreenGrassDbContext _context;
        private readonly IMapper _mapper;
        public FriendRepository(GreenGrassDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
