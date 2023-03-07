using AutoMapper;
using Messages.IntegrationEvents;
using Order.Application.Features.Commands;

namespace Order.Application.Mappings
{
    public sealed class ConsumerMappingProfile : Profile
    {
        public ConsumerMappingProfile()
        {
            CreateMap<StockFailedIE, StockFailed.Command>().ReverseMap();
            CreateMap<PaymentFailedIE, PaymentFailed.Command>().ReverseMap();
            CreateMap<PaymentSucceededIE, PaymentSucceeded.Command>().ReverseMap();
        }
    }
}
