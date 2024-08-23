using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {

        string apiKey = "cfa4f226cae254405aa9f747b84aa98f";
        string url = $"https://api.exchangeratesapi.io/v1/latest?access_key={apiKey}&symbols=USD,BRL";

        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Faz a requisição para a API
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Faz o parsing do JSON retornado
                JObject json = JObject.Parse(responseBody);
                decimal cambioUSD = json["rates"]["USD"].Value<decimal>(); // rates: chave | USD: valor
                decimal cambioBRL = json["rates"]["BRL"].Value<decimal>();

                // Pegando a data da taxa de cambio

                string dataTaxaDeCambio = json["date"].ToString();

                Console.WriteLine($"A taxa de câmbio de conversão EUR -> USD: {cambioUSD}");
                Console.WriteLine($"A taxa de câmbio de conversão EUR -> BRL: {cambioBRL}");
                Console.WriteLine("--------------------------------------------------");

                Console.Write("Insira o valor em BRL para que seja convertido: R$");
                decimal valorBRL = Convert.ToDecimal(Console.ReadLine());

                // Variáveis dos valores convertidos

                decimal valorEUR = valorBRL/cambioBRL; // cambioBRL = EUR * BRL
                decimal valorUSD = valorEUR * cambioUSD; // pega o valor em BRL que foi convertido para EUR e converte para USD

                Console.WriteLine($"O valor R${valorBRL} convertido em USD é igual a: {Math.Round(valorUSD, 2)}");
                Console.WriteLine($"O valor R${valorBRL} convertido em EUR é igual a: {Math.Round(valorEUR, 2)}");
                Console.WriteLine($"Data da taxa de câmbio: {dataTaxaDeCambio}");



            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro ao acessar a API: {e.Message}");
            }
        }

    }
}
