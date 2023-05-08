using MvcSGCubos.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace MvcSGCubos.Services
{
    public class ServiceApiCubos
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApiCubos;

        public ServiceApiCubos(IConfiguration configuration)
        {
            this.UrlApiCubos =
                configuration.GetValue<string>("ApiUrls:ApiOAuthCubos");
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<string> GetTokenAsync
            (string nombre, string pass)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/auth/login";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    Nombre = nombre,
                    Pass = pass
                };
                string jsonModel = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(jsonModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data =
                        await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(data);
                    string token =
                        jsonObject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
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

        private async Task<T> CallApiAsync<T>
            (string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
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

        //METODO LIBRE

        public async Task<List<Cubo>> GetCubosAsync()
        {
            string request = "/api/cubos";
            List<Cubo> cubos =
                await this.CallApiAsync<List<Cubo>>(request);
            return cubos;
        }

        //METODO LIBRE
        public async Task<List<Cubo>> FindCubosMarcaAsync(string marca)
        {
            string request = "/api/cubos/" + marca;
   
               
            return await this.CallApiAsync<List<Cubo>>(request);
        }

        public async Task InsertUsuarioASync
          (int Id_Usuario, string Nombre, string Email, string Pass, string Imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/cubos/usuario";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Usuario usuario = new Usuario();
                usuario.Id_Usuario = Id_Usuario;
                usuario.Nombre = Nombre;
                usuario.Pass = Pass;
                usuario.Email = Email;
                usuario.Imagen = Imagen;

                string json = JsonConvert.SerializeObject(usuario);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        public async Task InsertCuboAsync
       (int Id_Cubo, string Nombre, string Marca, string Imagen, int Precio)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/cubos/cubo";
                client.BaseAddress = new Uri(this.UrlApiCubos);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Cubo cubo = new Cubo();
                cubo.Id_Cubo = Id_Cubo;
                cubo.Nombre = Nombre;
                cubo.Marca = Marca;
                cubo.Imagen = Imagen;
                cubo.Precio = Precio;

                string json = JsonConvert.SerializeObject(cubo);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        public async Task<Usuario> PerfilUsuarioAsync(string token)
        {
            string request = "/api/cubos/perfilusuario";
            Usuario usuario = 
                await this.CallApiAsync<Usuario>(request, token);
            if (usuario != null)
            {
                return usuario;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<CompraCubos>> GetPedidosUsuariosAsync(string token)
        {
            string request = "/api/cubos/perfilusuario";
            Usuario usuario =
                  await this.CallApiAsync<Usuario>(request, token);
            if (usuario != null)
            {
                string request2 = "/api/cubos/pedidosusuario";
                return await this.CallApiAsync<List<CompraCubos>>(request2, token);
            }
            else
            {
                return null;
            }
        
        }


        public async Task InsertarPedidoAsync(int Id_Pedido, int Id_Cubo, DateTime FechaPedido, string token)
        {
            string request = "/api/cubos/perfilusuario";
            Usuario usuario =
                  await this.CallApiAsync<Usuario>(request, token);

            if (usuario != null)
            {
                using (HttpClient client = new HttpClient())
                {
                    string request2 = "api/cubos/createpedido/" + Id_Pedido + "/" + Id_Cubo + "/" + FechaPedido;
                    client.BaseAddress = new Uri(this.UrlApiCubos);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(this.Header);

                

                    //string json = JsonConvert.SerializeObject(personaje);

                    //StringContent content =
                    //    new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage response =
                        await client.PostAsync(request);
                }
            }
            
        }

    }
}
