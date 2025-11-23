using AutoMapper;
using HoL.Aplication.DTOs.EntitiDtos;
using HoL.Aplication.DTOs.ValueObjectDtos;
using HoL.Aplication.DTOs.AnatomiDtos;
using HoL.Aplication.DTOs.StatDtos;
using HoL.Domain.Entities;
using HoL.Domain.ValueObjects;
using HoL.Domain.ValueObjects.Anatomi;
using HoL.Domain.ValueObjects.Stat;
using HoL.Domain.Helpers;
using HoL.Domain.Helpers.AnatomiHelpers;
using HoL.Domain.Interfaces.Read_Interfaces;
using HoL.Domain.Interfaces.Write_Interaces;

namespace HoL.Aplication.MyMapper
{
    /// <summary>
    /// AutoMapper profil pro mapování mezi Domain objekty a DTOs.
    /// Enumy z Domain se propagují přímo do Application vrstvy.
    /// </summary>
    public class Mapper : Profile
    {
        public Mapper()
        {
            // Entities
            Map_Race();
            
            // Value Objects
            Map_Currency();
            Map_SpecialAbilities();
            Map_Weapon();
            Map_FightingSpirit();
            Map_VulnerabilityProfil();
            
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
        }

        // === Entities Mapping ===
        
        private void Map_Race()
        {
            CreateMap<Race, RaceDto>();
            CreateMap<RaceDto, Race>();
        }

        // === Value Objects Mapping ===
        
        private void Map_Currency()
        {
            CreateMap<Currency, CurrencyDto>();
            CreateMap<CurrencyDto, Currency>();
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

        private void Map_FightingSpirit()
        {
            CreateMap<FightingSpirit, FightingSpiritDto>();
            CreateMap<FightingSpiritDto, FightingSpirit>();
        }

        private void Map_VulnerabilityProfil()
        {
            CreateMap<VulnerabilityProfil, VulnerabilityProfilDto>();
            CreateMap<VulnerabilityProfilDto, VulnerabilityProfil>();
        }

        // === Anatomy Mapping ===
        
        private void Map_AnatomyProfile()
        {
            CreateMap<AnatomyProfile, AnatomyProfileDto>()
                .ForMember(dest => dest.HeightMin, opt => opt.MapFrom(src => src.HeihtMin))
                .ForMember(dest => dest.HeightMax, opt => opt.MapFrom(src => src.HeihtMax));
            
            CreateMap<AnatomyProfileDto, AnatomyProfile>()
                .ForMember(dest => dest.HeihtMin, opt => opt.MapFrom(src => src.HeightMin))
                .ForMember(dest => dest.HeihtMax, opt => opt.MapFrom(src => src.HeightMax));
        }

        private void Map_BodyPart()
        {
            CreateMap<BodyPart, BodyPartDto>();
            CreateMap<BodyPartDto, BodyPart>();
        }

        private void Map_BodyPartAttack()
        {
            CreateMap<BodyPartAttack, BodyPartAttackDto>();
            CreateMap<BodyPartAttackDto, BodyPartAttack>();
        }

        private void Map_BodyPartDefense()
        {
            CreateMap<BodyPartDefense, BodyPartDefenseDto>();
            CreateMap<BodyPartDefenseDto, BodyPartDefense>();
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
    }
}
