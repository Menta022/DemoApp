// See https://aka.ms/new-console-template for more information
using DemoApp.Client;

Console.WriteLine("Cliente Linea de comandos");


do 
{
    
    Console.WriteLine("escriba comando a realizar.");
    var comando = Console.ReadLine()?.Trim();
    if (comando is not { Length: > 0 }) // pattern matching, clausula de guardia, guard clause.
    {
        Console.WriteLine("por favor ingrese un comando");
        continue;
    }

    switch (comando)
    {
        case "exit":
            return;
        case "clima":
            await ObtenerClima();
            // Llamar al servicio de clima
            break;  
        default:
            Console.WriteLine("comando no reconocido intente nuevamente");
            break;
    }

} while (true);

static async Task ObtenerClima()
{
    Console.WriteLine("obteniendo clima...");
    using var httpClient = new HttpClient();
    var response = await httpClient.GetAsync("http://localhost:5088/weatherforecast");/*get obtener contenido*/
    /*protocolos de red corren sobre otro protocolo tcp/ip corren sobre socket tcp, tipo de socket tcp y udp.
     http para contenido paginas web o api web.
    ftp para transferencia de archivos (archivos grandes), carga y descarga, algunos archivos se sirven de http como imagen, audio, html,
    no se puede usar desde navegado web, desde un cliente ftp, linea de comando o escritorio.
    smtp (simple message transfer protocol) para envio de correo electronico.
    tipos de socket: tcp conectado, udp desconectado
    socket son fichero especial.
    */
    response.EnsureSuccessStatusCode();
    var jsonResponse = await response.Content.ReadAsStringAsync();
    //Console.WriteLine(jsonResponse);
    var listadoClima = HistoricoClima.FromJson(jsonResponse);
    foreach (var clima in listadoClima)
    {
        //Console.WriteLine($"{clima.Date:dd/MM/yyyy}: {clima.TemperatureC}C - {clima.TemperatureF}F - {clima.Summary}");
        Console.WriteLine(clima.Summary);
    }

}