using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using mvc_client.Models;

namespace mvc_client.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly HttpClient client;
        private string api;

        public ProductController()
        {
            client = new HttpClient();
            var contenType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contenType);
            api = "http://localhost:5001/api";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            dynamic model = new ExpandoObject();

            var apiGetAll = $"{api}/getall";
            HttpResponseMessage res = await client.GetAsync(apiGetAll);
            var data = await res.Content.ReadAsStringAsync();

            var opt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            // var list = JsonSerializer.Deserialize<List<Product>>(data, opt);
            model.products = JsonSerializer.Deserialize<List<Product>>(data, opt);
            model.cates = await GetCategories();

            return View(model);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var apiCates = $"{api}/categories";
            HttpResponseMessage res = await client.GetAsync(apiCates);
            var data = await res.Content.ReadAsStringAsync();

            var opt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var list = JsonSerializer.Deserialize<IEnumerable<Category>>(data, opt);
            return list;
        }

        [HttpGet("add", Name = "add")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Category> categories = await GetCategories();
            SelectList cateList = new SelectList(categories, "ID", "Name"); //list, value, text view
            ViewBag.cate = cateList;

            return View();
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                var apiAdd = $"{api}/add";

                string data = JsonSerializer.Serialize(obj);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await client.PostAsync(apiAdd, content);
                if (res.StatusCode == HttpStatusCode.Created)
                {
                    return Redirect("/product");
                }
            }
            return View(obj);
        }

        [HttpGet("edit/{id}", Name = "edit")]
        public async Task<IActionResult> Update(int id)
        {
            var apiProd = $"{api}/product/{id}";
            HttpResponseMessage res = await client.GetAsync(apiProd);
            var data = await res.Content.ReadAsStringAsync();

            var opt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var prod = JsonSerializer.Deserialize<Product>(data, opt);

            IEnumerable<Category> categories = await GetCategories();
            SelectList cateList = new SelectList(categories, "ID", "Name");
            ViewBag.cate = cateList;

            return View(prod);
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Update(int id, Product obj)
        {
            if (ModelState.IsValid)
            {
                var apiEdit = $"{api}/edit/{id}";

                string data = JsonSerializer.Serialize(obj);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await client.PutAsync(apiEdit, content);

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    return Redirect("/product");
                }
            }
            return View();
        }

        [HttpGet("remove/{id}", Name = "remove")]
        public async Task<IActionResult> Delete(Product p)
        {
            var apiProd = $"{api}/product/{p.ID}";
            HttpResponseMessage res = await client.GetAsync(apiProd);
            var data = await res.Content.ReadAsStringAsync();

            var opt = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var prod = JsonSerializer.Deserialize<Product>(data, opt);

            IEnumerable<Category> categories = await GetCategories();
            SelectList cateList = new SelectList(categories, "ID", "Name");
            ViewBag.cate = cateList;

            return View(prod);
        }

        [HttpPost("remove/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var apiDelete = $"{api}/delete/{id}";

            HttpResponseMessage res = await client.DeleteAsync(apiDelete);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                return Redirect("/product");
            }
            return View();
        }
    }
}