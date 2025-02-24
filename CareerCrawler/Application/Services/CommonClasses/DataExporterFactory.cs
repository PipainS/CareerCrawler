using HHParser.Application.Interfaces;
using HHParser.Domain.Enums;

namespace HHParser.Application.Services.CommonClasses
{
    public static class DataExporterFactory
    {
        public static IDataExporter GetExporter(ExportFormat format)
        {
            return format switch
            {
                ExportFormat.Csv => new CsvDataExporter(),
                _ => throw new NotSupportedException($"Формат {format} не поддерживается"),
            };
        }
    }
}
