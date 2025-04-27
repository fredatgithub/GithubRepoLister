using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace GithubRepoLister
{
  class Program
  {
    static void Main()
    {
      try
      {
        // Entrez votre token GitHub (PERSONAL ACCESS TOKEN)
        const string token = "";
        GetRepositoriesAsync(token).GetAwaiter().GetResult();
      }
      catch (Exception exception)
      {
        Console.WriteLine($"Erreur: {exception.Message}");
      }

      Console.WriteLine("Appuyez sur une touche pour quitter...");
      Console.ReadKey();
    }

    static async Task GetRepositoriesAsync(string token)
    {
      if (string.IsNullOrEmpty(token))
      {
        Console.WriteLine("Token invalide. Fin de l'application.");
        return;
      }

      using (var client = new HttpClient())
      {
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("ConsoleApp", "1.0"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);

        const string url = "https://api.github.com/user/repos";
        var response = await client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
          Console.WriteLine($"Requête échouée : {response.StatusCode}");
          return;
        }

        var json = await response.Content.ReadAsStringAsync();
        JArray repos = JArray.Parse(json);

        Console.WriteLine($"Vous avez {repos.Count} dépôt(s) :\n");
        foreach (var repo in repos)
        {
          string name = repo["name"]?.ToString();
          string description = repo["description"]?.ToString();
          string htmlUrl = repo["html_url"]?.ToString();

          Console.WriteLine($"Nom       : {name}");
          Console.WriteLine($"Description: {description}");
          Console.WriteLine($"URL       : {htmlUrl}\n");
        }
      }
    }
  }
}
