using Application.Activities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
  public class List
  {
    public class Query : IRequest<Result<List<UserActivityDto>>>
    {
      public string Predicate { get; set; }
      public string Username { get; set; }
    }
    public class Hnadler : IRequestHandler<Query, Result<List<UserActivityDto>>>
    {
      private readonly DataContext _context;
      private readonly IMapper _mapper;
      public Hnadler(DataContext context, IMapper mapper)
      {
        _mapper = mapper;
        _context = context;
      }

      public async Task<Result<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
      {
        var query = _context.ActivityAttendess
          .Where(u=> u.AppUser.UserName ==request.Username)
          .OrderBy(a=>a.Activity.Date)
          .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
          .AsQueryable();

          query = request.Predicate switch
          {
            "past" => query.Where(a=>a.Date<=DateTime.Now),
            "hosting" => query.Where(a=>a.HostUsername == request.Username),
            _ =>query.Where(a=>a.Date>= DateTime.Now)
          };

          var activities = await query.ToListAsync();

          return Result<List<UserActivityDto>>.Success(activities);
      }
    }
  }
}