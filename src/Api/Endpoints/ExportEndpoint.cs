using Api.Extensions;
using Application.Services;

namespace Api.Endpoints
{
    public static class ExportEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        {
            app.MapGet("Export/{tableName}", async (
                ExcelExportService excelExportService,
                string tableName,
                CancellationToken cancellationToken) =>
            {
                var file = await excelExportService.Export(tableName, cancellationToken);
                return Results.File(file.Content, file.ContentType, file.Name);
            })
            .WithApiName("Export")
            .WithOpenApi();
        }
    }
}
