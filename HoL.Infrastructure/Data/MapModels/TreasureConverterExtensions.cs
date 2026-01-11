using System.Text.Json;
using AutoMapper;
using HoL.Domain.Entities.CurencyEntities;
using HoL.Domain.ValueObjects;
using HoL.Infrastructure.Data.Models;

namespace HoL.Infrastructure.Data.MapModels
{
    /// <summary>
    /// Converter pro mapování TreasureDbModel → Treasure
    /// Vyžaduje CurrencyGroup pro inicializaci - slouží jen pro kontext v Race
    /// </summary>
    public class TreasureDbModelToTreasureConverter : ITypeConverter<TreasureDbModel, Treasure>
    {


        public Treasure Convert(TreasureDbModel source, Treasure destination, ResolutionContext context)
        {

            return source.MapToTreasure();

        }
    }

    /// <summary>
    /// Extension metody pro mapování TreasureDbModel → Treasure
    /// </summary>
    public static class TreasureConverterExtensions
    {
        private static readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Mapuje TreasureDbModel na Treasure domain model.
        /// Vyžaduje CurrencyGroup pro inicializaci.
        /// </summary>
        /// <param name="source">Databázový model pokladu</param>
        /// <param name="currencyGroup">Měnová skupina pro inicializaci</param>
        /// <returns>Domain value object Treasure s naplněnými daty</returns>
        /// public static Treasure? MapToTreasure(this TreasureDbModel source,CurrencyGroup currencyGroup)
        /// <summary>
        /// Mapuje TreasureDbModel na Treasure domain model.
        /// PŘETÍŽENÍ 1: CurrencyGroup je již načteno v source.CurrencyGroup (Eager Loading)
        /// </summary>
        public static Treasure? MapToTreasure(this TreasureDbModel source)
        {
            if (source == null)
                return null;

            if (source.CurrencyGroup == null)
                throw new ArgumentNullException(nameof(source.CurrencyGroup), "CurrencyGroup nesmí být null.");

            try
            {
                var currencyGroup = source.CurrencyGroup.MapToCurrencyGroup();

                // Vytvoření Treasure s měnovou skupinou
                var treasure = new Treasure(currencyGroup);

                // Deserializace JSON pro množství mincí
                if (!string.IsNullOrEmpty(source.CoinQuantitiesJson))
                {
                    var coinQuantities = DeserializeCoinQuantities(source.CoinQuantitiesJson);
                    if (coinQuantities != null && coinQuantities.Count > 0)
                    {
                        treasure.SetCoinQuantities(coinQuantities);
                    }
                }

                return treasure;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Chyba při mapování TreasureDbModel na Treasure: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Mapuje TreasureDbModel na Treasure domain model.
        /// PŘETÍŽENÍ 2: CurrencyGroup je poskytnut jako parametr (pro TreasureDbRepository)
        /// </summary>
        public static Treasure? MapToTreasure(this TreasureDbModel source, CurrencyGroup currencyGroup)
        {
            if (source == null)
                return null;

            if (currencyGroup == null)
                throw new ArgumentNullException(nameof(currencyGroup), "CurrencyGroup nesmí být null.");

            try
            {
                // Vytvoření Treasure s měnovou skupinou
                var treasure = new Treasure(currencyGroup);

                // Deserializace JSON pro množství mincí
                if (!string.IsNullOrEmpty(source.CoinQuantitiesJson))
                {
                    var coinQuantities = DeserializeCoinQuantities(source.CoinQuantitiesJson);
                    if (coinQuantities != null && coinQuantities.Count > 0)
                    {
                        treasure.SetCoinQuantities(coinQuantities);
                    }
                }

                return treasure;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Chyba při mapování TreasureDbModel na Treasure: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deserializuje JSON slovník množství mincí.
        /// </summary>
        private static Dictionary<int, int>? DeserializeCoinQuantities(string json)
        {
            try
            {
                if (string.IsNullOrEmpty(json))
                    return null;

                return JsonSerializer.Deserialize<Dictionary<int, int>>(json, jsonOptions);
            }
            catch
            {
                return null;
            }
        }
    }
}
