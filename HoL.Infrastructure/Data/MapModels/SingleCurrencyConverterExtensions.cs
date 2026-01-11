using AutoMapper;
using HoL.Domain.Entities.CurencyEntities;
using HoL.Infrastructure.Data.Models;

namespace HoL.Infrastructure.Data.MapModels
{
    /// <summary>
    /// Converter pro mapování SingleCurrencyDbModel → SingleCurrency
    /// </summary>
    public class SingleCurrencyDbModelToSingleCurrencyConverter : ITypeConverter<SingleCurrencyDbModel, SingleCurrency>
    {
        public SingleCurrency Convert(SingleCurrencyDbModel source, SingleCurrency destination, ResolutionContext context)
        {
            return source.MapToSingleCurrency();
        }
    }

    /// <summary>
    /// Extension metody pro mapování SingleCurrencyDbModel → SingleCurrency
    /// </summary>
    public static class SingleCurrencyConverterExtensions
    {
        /// <summary>
        /// Mapuje SingleCurrencyDbModel na SingleCurrency domain model.
        /// </summary>
        /// <param name="source">Databázový model měny</param>
        /// <returns>Domain model SingleCurrency s naplněnými daty</returns>
        public static SingleCurrency MapToSingleCurrency(this SingleCurrencyDbModel source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "SingleCurrencyDbModel nesmí být null.");

            var singleCurrency = new SingleCurrency(
                source.Name,
                source.ShotName,
                source.HierarchyLevel,
                source.ExchangeRate);

            // Nastavení ID
            singleCurrency.SetId(source.Id);

            return singleCurrency;
        }
    }
}
