using MediatR;

namespace Application.Features.TempFeatures.Queries.GetTempById;
public class GetTempByIdQueryHandler() : IRequestHandler<GetTempByIdQuery,GetTempByIdQueryResponse>
{
    public async Task<GetTempByIdQueryResponse> Handle(GetTempByIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
