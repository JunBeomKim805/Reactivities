using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
  public class List
  {
    public class Query : IRequest<Result<PagedList<ActivityDto>>>
    {
      public ActivityParams Params { get; set; }
    }
    public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
    {
      private readonly DataContext _context;
      private readonly IMapper _mapper;
      private readonly IUserAccessor _userAccessor;
      public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)

      {
        _userAccessor = userAccessor;
        _mapper = mapper;
        // _logger = logger;
        _context = context;
      }

      public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
      {
        // try
        // {
        //   for (var i = 0; i < 10; i++)
        //   {
        //     cancellationToken.ThrowIfCancellationRequested();
        //     await Task.Delay(1000, cancellationToken);
        //     _logger.LogInformation($"Task {i} has completed");
        //   }
        // }
        // catch (Exception ex) when (ex is TaskCanceledException)
        // {
        //   _logger.LogInformation("Task was cancelled");
        // }
        var query =  _context.Activities
          .Where(d=>d.Date>= request.Params.StartDate)
          .OrderBy(d=>d.Date)
          // .Include(a => a.Attendees)
          // .ThenInclude(u => u.AppUser)
          .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new {currentUsername = _userAccessor.GetUsername()})
          .AsQueryable();
        if(request.Params.IsGoing &&!request.Params.IsHost)
        {
          query = query.Where(x=>x.Attendees.Any(a=>a.Username ==_userAccessor.GetUsername()));
        }

        if(request.Params.IsHost && !request.Params.IsGoing)
        {
           query = query.Where(x=>x.HostUsername ==_userAccessor.GetUsername());
        } 
        // var activitiesToReturn = _mapper.Map<List<ActivityDto>>(activities);

        return Result<PagedList<ActivityDto>>.Success(
          await PagedList<ActivityDto>.CreateAsync(query,request.Params.PageNumber,request.Params.PageSize)
        );
      }
    }
  }
}