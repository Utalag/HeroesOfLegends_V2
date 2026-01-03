using AutoMapper;
using HoL.Aplication.MyMapper;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AplicationTest.MapperTest
{
    public abstract class SetupMapperTests
    {
        protected readonly IMapper _mapper;

        protected SetupMapperTests()
        {
            _mapper = CreateMapper();
        }

        private static IMapper CreateMapper()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                // Volitelné: builder.AddConsole(); builder.SetMinimumLevel(LogLevel.Debug);
            });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GlobalMapper>();
            }, loggerFactory);

            config.AssertConfigurationIsValid();
            return config.CreateMapper();
        }


        [Fact]
        public void ConfigurationMapper_Is_Valid()
        {
            var loggerFactory = LoggerFactory.Create(_ => { });
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(GlobalMapper).Assembly);
            }, loggerFactory);

            try
            {
                config.AssertConfigurationIsValid();
            }
            catch (AutoMapperConfigurationException ex)
            {
                // Užitečný výpis chyb mapování
                var msg = string.Join(Environment.NewLine, ex.Errors.Select(e =>
                    $"{e.TypeMap?.SourceType?.Name} -> {e.TypeMap?.DestinationType?.Name}"));
                Assert.Fail(msg);
            }

            var mapper = config.CreateMapper();
            Assert.NotNull(mapper);
        }
    }
}


