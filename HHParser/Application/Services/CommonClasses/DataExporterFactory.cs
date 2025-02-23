using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                // ExportFormat.Json => new JsonDataExporter(),
                // ExportFormat.Xml  => new XmlDataExporter(),
                _ => throw new NotSupportedException($"Формат {format} не поддерживается"),
            };
        }
    }

}
