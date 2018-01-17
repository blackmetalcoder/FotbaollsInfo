using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using PortableRest;
using System.Net;
using System.IO;

namespace FotbaollsInfo
{
    public sealed class DataSource
    {
       
        const string apiKey = "e9ecf8f219364df0ab6f292c105584ff";
        private static async Task<dynamic> getDataFromService(string queryString)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(queryString);
                var response = await request.GetResponseAsync().ConfigureAwait(false);
                var stream = response.GetResponseStream();
                var streamReader = new StreamReader(stream);
                string responseText = streamReader.ReadToEnd();
                dynamic data = JsonConvert.DeserializeObject(responseText);
                return data;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async static Task<string> GetLigor()
        {
            try
            {
                var client = new HttpClient();
                var uri = new Uri("http://resultatservice.azurewebsites.net/api/Ligors");
                var Response = await client.GetAsync(uri);
                var statusCode = Response.StatusCode;
                Response.EnsureSuccessStatusCode();
                var ResponseText = await Response.Content.ReadAsStringAsync();
                return ResponseText;
            }

            catch (Exception ex)
            {
                return "fel";
            }
        }
        public async static Task<string> GetLive(string Datum)
        {
            try
            {
                var client = new HttpClient();
                var uri = new Uri("http://resultatservice.azurewebsites.net/api/Fixtures?StartDatum=" + Datum);
                var Response = await client.GetAsync(uri);
                var statusCode = Response.StatusCode;
                Response.EnsureSuccessStatusCode();
                var ResponseText = await Response.Content.ReadAsStringAsync();
                return ResponseText;
            }

            catch (Exception ex)
            {
                return "fel";
            }
        }

       public async static Task<string> getTeam(int id)
        {
            try
            {
                var client = new HttpClient();
                var uri = new Uri("http://resultatservice.azurewebsites.net/api/Teams?id=" + id);
                var Response = await client.GetAsync(uri);
                var statusCode = Response.StatusCode;
                Response.EnsureSuccessStatusCode();
                var ResponseText = await Response.Content.ReadAsStringAsync();
                return ResponseText;
            }

            catch (Exception ex)
            {
                return "fel";
            }
        }
    }
}
