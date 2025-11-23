using HoL.Aplication.DTOs.EntitiDtos;
using MediatR;                                                     

namespace HoL.Aplication.Handlers.Commands.RaceCommand.ExampleCommandAndHandler

// 1.a  Example of Command wirh record - return Dto
{
    protected saeled record class XXXCommand(Dto dto) : IRequest<Dto>;

}

// 1.b Example of Command wirh class - return Dto
{
    protected sealed class XXXCommand : IRequest<Dto>
    {
        public Dto Dto { get; }

        public XXXCommand(Dto dto)
        {
            Dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }
    }
}

--------------------------------------------------------------

// 2.a Example of Command wirh record - return void
{
    protected sealed record class XXXCommand(Dto dto) : IRequest;
}

// 2.b Example of Command wirh class - return void
{
    protected sealed class XXXCommand :IRequest
    {
        public Dto Dto { get; }
        public XXXCommand(Dto dto)
        {
            Dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }
    }
}

-------------------------------------------------------------

 // 3.a Example of Command wirh record - return primitive type
{
    protected sealed record class XXXCommand(Dto dto) : IRequest<int>;
}

// 3.b Example of Command wirh class - return primitive type
{
    protected sealed class XXXCommand : IRequest<int>
    {
        public Dto Dto { get; }
        public XXXCommand(Dto dto)
        {
            Dto = dto ?? throw new ArgumentNullException(nameof(dto));
        }
    }
}