using HoL.Aplication.DTOs.AnatomiDtos;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.DTOs.StatDtos;
using HoL.Aplication.DTOs.ValueObjectDtos;
using HoL.Domain.Entities;
using HoL.Domain.Helpers;
using HoL.Domain.Helpers.AnatomiHelpers;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Anatomi.Stat;

namespace HoL.Aplication.MyMapper
{
    /// <summary>
    /// AutoMapper profil pro mapování mezi Domain objekty a DTOs.
    /// Enumy z Domain se propagují přímo do Application vrstvy.
    /// </summary>
    public class GlobalMapper : Profile
    {
        public GlobalMapper()
        {
            // Entities
            Map_Race();

            // Value Objects
            Map_Treasure();
            Map_SpecialAbilities();
            Map_Weapon();


            // Anatomy
            Map_AnatomyProfile();
            Map_BodyPart();
            Map_BodyPartAttack();
            Map_BodyPartDefense();

            // Stats
            Map_Stat();
            Map_ValueRange();

            // Helpers
            Map_Dice();
            Map_Currency();
        }

        // === Entities Mapping ===

        private void Map_Race()
        {
            CreateMap<Race, RaceDto>();
            CreateMap<RaceDto, Race>();
        }

        // === Value Objects Mapping ===

        private void Map_Treasure()
        {
            CreateMap<Treasure, TreasureDto>();
            CreateMap<TreasureDto, Treasure>();
        }

        private void Map_SpecialAbilities()
        {
            CreateMap<SpecialAbilities, SpecialAbilitiesDto>();
            CreateMap<SpecialAbilitiesDto, SpecialAbilities>();
        }

        private void Map_Weapon()
        {
            CreateMap<Weapon, WeaponDto>();
            CreateMap<WeaponDto, Weapon>();
        }


        // === Anatomy Mapping ===

        private void Map_AnatomyProfile()
        {
            // Mapování mezi BodyDimension a AnatomyProfileDto
            CreateMap<BodyDimension, AnatomyProfileDto>();
            CreateMap<AnatomyProfileDto, BodyDimension>();
        }

        private void Map_BodyPart()
        {
            CreateMap<BodyPart, BodyPartDto>();
            CreateMap<BodyPartDto, BodyPart>();
        }

        private void Map_BodyPartAttack()
        {
            // DTO nemá vlastnost CanBeUsedWithOtherAttacks -> ignorujeme při mapování z Domain na DTO
            CreateMap<BodyPartAttack, BodyPartAttackDto>();
            // Při mapování z DTO do Domain ignorujeme CanBeUsedWithOtherAttacks (zůstane default)
            CreateMap<BodyPartAttackDto, BodyPartAttack>()
                .ForMember(d => d.CanBeUsedWithOtherAttacks, opt => opt.Ignore());
        }

        private void Map_BodyPartDefense()
        {
            // V Domain je IsProtected, v DTO je IsMagical -> namapujeme explicitně
            CreateMap<BodyPartDefense, BodyPartDefenseDto>()
                .ForMember(d => d.IsMagical, opt => opt.MapFrom(src => src.IsProtected));

            CreateMap<BodyPartDefenseDto, BodyPartDefense>()
                .ForMember(d => d.IsProtected, opt => opt.MapFrom(src => src.IsMagical));
        }

        // === Stats Mapping ===

        private void Map_Stat()
        {
            CreateMap<Stat, StatDto>();
            CreateMap<StatDto, Stat>();
        }

        private void Map_ValueRange()
        {
            CreateMap<ValueRange, ValueRangeDto>();
            CreateMap<ValueRangeDto, ValueRange>();
        }

        // === Helpers Mapping ===

        private void Map_Dice()
        {
            CreateMap<Dice, DiceDto>();
            CreateMap<DiceDto, Dice>();
        }

        private void Map_Currency()
        {
            CreateMap<CurrencyGroup, CurrencyDto>();
            CreateMap<CurrencyDto, CurrencyGroup>();
        }
    }
}
