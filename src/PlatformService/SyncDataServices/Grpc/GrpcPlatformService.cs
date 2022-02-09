using AutoMapper;
using Grpc.Core;
using PlatformService.Data.Repositories;
using PlatformService.Models;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IRepository<Platform> _platformRepository;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IRepository<Platform> platformRepository, IMapper mapper)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();
            var platforms = _platformRepository.GetAll();

            foreach(var platform in platforms)
            {
                response.Platform.Add(_mapper.Map<Platform, GrpcPlatformModel>(platform));
            }

            return Task.FromResult(response);
        }
    }
}