using AutoMapper;
using Messages;
using Messages.IntegrationEvents;
using Report.Application.Features.Commands;

namespace Report.Application.Mappings
{
    public class ConsumerMappingProfile : Profile
    {
        public ConsumerMappingProfile()
        {
            CreateMap<IIntegrationEvent, LogEvent.Command>().ReverseMap();
            CreateMap<OrderPlacedIE, LogEvent.Command>().ReverseMap();
            CreateMap<StockDecreasedIE, LogEvent.Command>().ReverseMap();
            CreateMap<StockFailedIE, LogEvent.Command>().ReverseMap();
            CreateMap<PaymentSucceededIE, LogEvent.Command>().ReverseMap();
            CreateMap<PaymentFailedIE, LogEvent.Command>().ReverseMap();
        }
    }
}
