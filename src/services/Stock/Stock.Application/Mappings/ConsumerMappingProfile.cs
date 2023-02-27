using AutoMapper;
using Messages.IntegrationEvents;
using Stock.Application.Features.Commands;

namespace Stock.Application.Mappings
{
    public sealed class ConsumerMappingProfile : Profile
    {
        public ConsumerMappingProfile()
        {
            CreateMap<OrderPlacedIE, DecreaseStock.Command>().ReverseMap();
            CreateMap<Messages.IntegrationEvents.OrderItemDto, Stock.Domain.Dtos.OrderItemDto>().ReverseMap();
        }
    }
}
