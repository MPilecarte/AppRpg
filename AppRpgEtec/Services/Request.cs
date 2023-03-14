using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;


namespace AppRpgEtec.Services
{
    public class Request
    {
        public async Task<int> PostReturnIntAsync<TResult>(String uri, TResult data)
        {
            HttpClient httpClient= new HttpClient();    

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            string serialized = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return int.Parse(serialized);
            }
            else
                return 0;    
        }

        public async Task<TResult> PostAsync<TResult>(string uri, TResult data, string token)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject((TResult)data));    
            content.Headers.ContentType=new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = data;

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
                result = await Task.Run(()=> JsonConvert.DeserializeObject<TResult>(serialized));   

            return result;



        }

    }
}
