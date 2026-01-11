using AutoMapper;
using HoL.Domain.Entities.CurencyEntities;
using HoL.Infrastructure.Data.Models;

namespace HoL.Infrastructure.Data.MapModels
{
    /// <summary>
    /// Converter pro mapování CurrencyGroupDbModel → CurrencyGroup
    /// </summary>
    public class CurrencyGroupDbModelToCurrencyGroupConverter : ITypeConverter<CurrencyGroupDbModel, CurrencyGroup>
    {
        public CurrencyGroup Convert(CurrencyGroupDbModel source, CurrencyGroup destination, ResolutionContext context)
        {
            return source.MapToCurrencyGroup();
        }
    }

    /// <summary>
    /// Extension metody pro mapování CurrencyGroupDbModel → CurrencyGroup
    /// </summary>
    public static class CurrencyGroupConverterExtensions
    {
        /// <summary>
        /// Mapuje CurrencyGroupDbModel na CurrencyGroup domain model.
        /// </summary>
        /// <param name="source">Databázový model skupiny měn</param>
        /// <returns>Domain model CurrencyGroup s naplněnými daty</returns>
        public static CurrencyGroup MapToCurrencyGroup(this CurrencyGroupDbModel source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "CurrencyGroupDbModel nesmí být null.");

            // Mapování SingleCurrencyDbModel → SingleCurrency
            var singleCurrencies = source.Currencies?
                .Select(sc => sc.MapToSingleCurrency())
                .ToList() ?? new List<SingleCurrency>();

            // Vytvoření CurrencyGroup s mapovanými měnami
            var currencyGroup = new CurrencyGroup(source.GroupName, singleCurrencies);
            
            // Nastavení ID
            currencyGroup.SetId(source.Id);

            return currencyGroup;
        }
    }
}
