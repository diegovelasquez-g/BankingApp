using BankingApp.Shared.Dtos.Responses;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace BankingApp.Application.Helpers;

public class GeneratePDF
{
    public Stream GenerateCreditCardStatus(CreditCardAccountStatusResponse creditCardAccountStatusResponse)
    {
        var data = Document.Create(document =>
        {
            document.Page(page =>
            {

                page.Margin(30);

                page.Header().ShowOnce().Row(row =>
                {
                    //var ruta = "https://i.pinimg.com/originals/06/2f/46/062f46285497838b2b87273a41cfeb80.png";
                    //byte[] imageData = System.IO.File.ReadAllBytes(ruta);
                    //row.ConstantItem(150).Image(imageData);

                    row.ConstantItem(140).Height(60).Placeholder();


                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignCenter().Text("Mi Banco").Bold().FontSize(14);
                        col.Item().AlignCenter().Text("San Salvador - El Salvador").FontSize(9);
                        col.Item().AlignCenter().Text("2254-5875 / 2236-4897").FontSize(9);
                        col.Item().AlignCenter().Text("atencioncliente@mibanco.sv").FontSize(9);

                    });

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Border(1).BorderColor("#257272")
                        .AlignCenter().Text("RUC 21312312312");

                        col.Item().Background("#257272").Border(1)
                        .BorderColor("#257272").AlignCenter()
                        .Text("Estado de Cuenta").FontColor("#fff");

                        col.Item().Border(1).BorderColor("#257272").
                        AlignCenter().Text("B0001 - 234");


                    });
                });

                page.Content().PaddingVertical(10).Column(col1 =>
                {
                    col1.Item().Column(col2 =>
                    {
                        col2.Item().Text("Datos del cliente").Underline().Bold();

                        col2.Item().Text(txt =>
                        {
                            txt.Span("Nombre: ").SemiBold().FontSize(10);
                            txt.Span(creditCardAccountStatusResponse.TitularTarjeta).FontSize(10);
                        });

                        col2.Item().Text(txt =>
                        {
                            txt.Span("# de Tarjeta: ").SemiBold().FontSize(10);
                            txt.Span(creditCardAccountStatusResponse.NumeroTarjeta).FontSize(10);
                        });
                    });

                    col1.Item().LineHorizontal(0.5f);
                    col1.Item().Text("Compras del mes").Bold().FontSize(15);
                    col1.Item().Table(tabla =>
                    {
                        tabla.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3);
                            columns.RelativeColumn();
                            columns.RelativeColumn();

                        });

                        tabla.Header(header =>
                        {
                            header.Cell().Background("#257272")
                            .Padding(2).Text("Descripción").FontColor("#fff");

                            header.Cell().Background("#257272")
                           .Padding(2).Text("Fecha de compra").FontColor("#fff");

                            header.Cell().Background("#257272")
                           .Padding(2).Text("Monto").FontColor("#fff");
                        });

                        foreach (var item in creditCardAccountStatusResponse.Purchases)
                        {
                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            .Padding(2).Text(item.Description).FontSize(10);

                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            .Padding(2).Text(item.Date.ToShortDateString()).FontSize(10);

                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                            .Padding(2).Text($"${item.Amount}").FontSize(10);

                        }

                    });

                    col1.Item().Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text($"Total mes anterior: ${creditCardAccountStatusResponse.TotalComprasMesAnterior}").FontSize(12);

                        row.RelativeItem().AlignRight().Text($"Total mes actual: ${creditCardAccountStatusResponse.TotalComprasMesActual}").FontSize(12);
                    });

                    if (1 == 1)
                    {
                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
                            .Column(column =>
                            {
                                column.Item().Text("Montos Totales").FontSize(12).Bold();
                                column.Item().Text($"Monto total sin intereses: {creditCardAccountStatusResponse.MontoTotalSinIntereses}").FontSize(10);
                                column.Item().Text($"Monto total con intereses: {creditCardAccountStatusResponse.MontoTotalContadoIntereses}").FontSize(10);
                                column.Spacing(1);
                            });

                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
                            .Column(column =>
                            {
                                column.Item().Text("Intereses y Cuotas").FontSize(12).Bold();
                                column.Item().Text($"Cuota mínima: {creditCardAccountStatusResponse.CuotaMinimaAPagar}").FontSize(10);
                                column.Item().Text($"Intereses: {creditCardAccountStatusResponse.InteresBonificable}").FontSize(10);
                                column.Spacing(1);
                            });

                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
                            .Column(column =>
                            {
                                column.Item().Text("Detalles de la Tarjeta").FontSize(12).Bold();
                                column.Item().Text($"Límite: ${creditCardAccountStatusResponse.LimiteCredito}").FontSize(10);
                                column.Item().Text($"Saldo disponible: ${creditCardAccountStatusResponse.SaldoDisponible}").FontSize(10);
                                column.Spacing(1);
                            });
                    }

                    col1.Spacing(10);
                });


                page.Footer()
                .AlignRight()
                .Text(txt =>
                {
                    txt.Span("Pagina ").FontSize(10);
                    txt.CurrentPageNumber().FontSize(10);
                    txt.Span(" de ").FontSize(10);
                    txt.TotalPages().FontSize(10);
                });
            });
        }).GeneratePdf();
        Stream stream = new MemoryStream(data);
        return stream;
    }
}
