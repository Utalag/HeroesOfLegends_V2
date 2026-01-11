using System.Text.Json;
using AutoMapper;
using HoL.Domain.Entities;
using HoL.Domain.Entities.CurencyEntities;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi.Body;
using HoL.Infrastructure.Data.Models;

namespace HoL.Infrastructure.Data.MapModels
{
    public class DomainInfrastructureMapper : Profile
    {
        private readonly JsonSerializerOptions jsonOptions;

        public DomainInfrastructureMapper()
        {
            jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            RaceMapping();
            TreasureMapping();
            SingleCurrencyMapping();
            BodyDimensionMapping();
            CurrencyGroupMapping();
        }

        private void RaceMapping()
        {
            // Domain → Database mapping
            CreateMap<Race, RaceDbModel>()
                // Scalar properties - přímé mapování
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.RaceName, opt => opt.MapFrom(src => src.RaceName))
                .ForMember(dest => dest.RaceCategory, opt => opt.MapFrom(src => src.RaceCategory))
                .ForMember(dest => dest.BaseInitiative, opt => opt.MapFrom(src => src.BaseInitiative))
                .ForMember(dest => dest.RaceDescription, opt => opt.MapFrom(src => src.RaceDescription))
                .ForMember(dest => dest.RaceHistory, opt => opt.MapFrom(src => src.RaceHistory))
                .ForMember(dest => dest.Conviction, opt => opt.MapFrom(src => src.Conviction))
                .ForMember(dest => dest.BaseXP, opt => opt.MapFrom(src => src.BaseXP))
                .ForMember(dest => dest.FightingSpiritNumber, opt => opt.MapFrom(src => src.FightingSpiritNumber))
                .ForMember(dest => dest.ZSM, opt => opt.MapFrom(src => src.ZSM))
                .ForMember(dest => dest.DomesticationValue, opt => opt.MapFrom(src => src.DomesticationValue))

                // Owned Value Objects
                .ForMember(dest => dest.BodyDimensins, opt => opt.MapFrom(src => src.BodyDimensins))
                .ForMember(dest => dest.Treasure, opt => opt.MapFrom(src => src.Treasure))

                .ForMember(dest => dest.JsonBodyParts, opt => opt.MapFrom(src =>
                    src.BodyParts != null ? JsonSerializer.Serialize(src.BodyParts, jsonOptions) : string.Empty))
                .ForMember(dest => dest.JsonBodyStats, opt => opt.MapFrom(src =>
                    JsonSerializer.Serialize(src.StatsPrimar, jsonOptions)))
                .ForMember(dest => dest.JsonVulnerabilities, opt => opt.MapFrom(src =>
                    JsonSerializer.Serialize(src.Vulnerabilities, jsonOptions)))
                .ForMember(dest => dest.JsonMobility, opt => opt.MapFrom(src =>
                    JsonSerializer.Serialize(src.Mobility, jsonOptions)))
                .ForMember(dest => dest.JsonHierarchySystem, opt => opt.MapFrom(src =>
                    src.RaceHierarchySystem != null ? JsonSerializer.Serialize(src.RaceHierarchySystem, jsonOptions) : string.Empty))
                .ForMember(dest => dest.JsonSpeciualAbilities, opt => opt.MapFrom(src =>
                    src.SpecialAbilities != null ? JsonSerializer.Serialize(src.SpecialAbilities, jsonOptions) : string.Empty));

            // Database → Domain mapping - POMOCÍ CONVERTERU Z SAMOSTATNÉHO SOUBORU
            CreateMap<RaceDbModel, Race>()
                .ConvertUsing<RaceDbModelToRaceConverter>();
        }

        private void BodyDimensionMapping()
        {
            // BodyDimension mappings
            CreateMap<BodyDimension, BodyDimensionDbModel>()
                .ForMember(dest => dest.RaceSize, opt => opt.MapFrom(src => src.RaceSize.ToString()))
                .ForMember(dest => dest.WeightMin, opt => opt.MapFrom(src => src.WeightMin))
                .ForMember(dest => dest.WeightMax, opt => opt.MapFrom(src => src.WeightMax))
                .ForMember(dest => dest.LengthMin, opt => opt.MapFrom(src => src.LengthMin))
                .ForMember(dest => dest.LengthMax, opt => opt.MapFrom(src => src.LengthMax))
                .ForMember(dest => dest.HeightMin, opt => opt.MapFrom(src => src.HeihtMin))
                .ForMember(dest => dest.HeightMax, opt => opt.MapFrom(src => src.HeihtMax))
                .ForMember(dest => dest.MaxAge, opt => opt.MapFrom(src => src.MaxAge));

            CreateMap<BodyDimensionDbModel, BodyDimension>()
                .ForAllMembers(opts => opts.Ignore());
        }

        private void TreasureMapping()
        {
            // Treasure mappings - Domain → Database
            CreateMap<Treasure, TreasureDbModel>()
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.CurrencyGroupId))
                .ForMember(dest => dest.CoinQuantitiesJson, opt => opt.MapFrom(src => JsonSerializer.Serialize(src.CoinQuantities, jsonOptions)))
                .ForMember(dest => dest.CurrencyGroup, opt=>opt.MapFrom(src=>src.CurrencyGroup));
            
            // Database → Domain mapping - POMOCÍ CONVERTERU Z SAMOSTATNÉHO SOUBORU
            CreateMap<TreasureDbModel, Treasure>()
                .ConvertUsing<TreasureDbModelToTreasureConverter>();
        }

        private void SingleCurrencyMapping()
        {
            // Domain → Database mapping
            CreateMap<SingleCurrency, SingleCurrencyDbModel>();
            
            // Database → Domain mapping - POMOCÍ CONVERTERU Z SAMOSTATNÉHO SOUBORU
            CreateMap<SingleCurrencyDbModel, SingleCurrency>()
                .ConvertUsing<SingleCurrencyDbModelToSingleCurrencyConverter>();
        }

        private void CurrencyGroupMapping()
        {
            // Domain → Database mapping
            CreateMap<CurrencyGroup, CurrencyGroupDbModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GroupName))
                .ForMember(dest => dest.Currencies, opt => opt.MapFrom(src => src.Currencies));

            // Database → Domain mapping - POMOCÍ CONVERTERU Z SAMOSTATNÉHO SOUBORU
            CreateMap<CurrencyGroupDbModel, CurrencyGroup>()
                .ConvertUsing<CurrencyGroupDbModelToCurrencyGroupConverter>();
        }
    }
}
