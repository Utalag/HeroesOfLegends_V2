using MediatR;

namespace Application.Features.TempFeatures.Queries.GetAllTemp;
public class GetAllTempQueryHandler() : IRequestHandler<GetAllTempQuery,GetAllTempQueryResponse>
{
    public async Task<GetAllTempQueryResponse> Handle(GetAllTempQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
