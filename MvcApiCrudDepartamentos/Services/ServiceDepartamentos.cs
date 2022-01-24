using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using MvcApiCrudDepartamentos.Models;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Specialized;
using System.Web;

namespace MvcApiCrudDepartamentos.Services
{
    public class ServiceDepartamentos
    {
        private string URL;
        private MediaTypeWithQualityHeaderValue Header;
        private NameValueCollection queryString;

        public ServiceDepartamentos(string url)
        {
            this.URL = url;
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.queryString = HttpUtility.ParseQueryString(string.Empty);
        }

        //LOS METODOS GET SI UTILIZAN GENERICOS
        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                request = request + "?" + this.queryString;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.CacheControl =
                    CacheControlHeaderValue.Parse("no-cache");
                //AÑADIMOS NUESTRA CLAVE DE SUBSCRIPCION
                client.DefaultRequestHeaders.Add
                    ("Ocp-Apim-Subscription-Key", "c50da623b8ad4321b118d8074523774a");
                HttpResponseMessage response =
                    await client.GetAsync(this.URL + request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            string request = "/api/departamentos";
            List<Departamento> departamentos =
                await this.CallApiAsync<List<Departamento>>(request);
            return departamentos;
        }

        public async Task<Departamento> FindDepartamentoAsync(int id)
        {
            string request = "/api/departamentos/" + id;
            Departamento dept = await this.CallApiAsync<Departamento>(request);
            return dept;
        }

        public async Task DeleteDepartamentoAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/departamentos/" + id;
                client.BaseAddress = new Uri(this.URL);
                client.DefaultRequestHeaders.Clear();
                await client.DeleteAsync(request);
            }
        }

        public async Task InsertDepartamentoAsync(int id, string nombre, string localidad)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/departamentos";
                client.BaseAddress = new Uri(this.URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Departamento dept = new Departamento();
                dept.IdDepartamento = id;
                dept.Nombre = nombre;
                dept.Localidad = localidad;
                //DEBEMOS CONVERTIR A JSON EL OBJETO
                String json =
                    JsonConvert.SerializeObject(dept);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(request, content);
            }
        }

        public async Task UpdateDepartamentoAsync(int id, string nombre, string localidad)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/departamentos";
                client.BaseAddress = new Uri(this.URL);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Departamento dept = new Departamento();
                dept.IdDepartamento = id;
                dept.Nombre = nombre;
                dept.Localidad = localidad;
                String json =
                    JsonConvert.SerializeObject(dept);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
        }
    }
}
