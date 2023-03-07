using AutoMapper;
using Messages.IntegrationEvents;
using Payment.Application.Features.Commands;

namespace Payment.Application.Mappings
{
    public sealed class ConsumerMappingProfile : Profile
    {
        public ConsumerMappingProfile()
        {
            CreateMap<StockDecreasedIE, DoPayment.Command>().ReverseMap();
        }
    }
}
