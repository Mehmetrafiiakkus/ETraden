using ETrade.Data.Models.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ETrade.UI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoriesController : Controller
    {
        private HttpClient httpClient;
        public CategoriesController(HttpClient httpClient)
        {
            this.httpClient = httpClient;   
        }
        public async Task<IActionResult> Index()
        {
            var responseMessage = await httpClient.GetAsync("http://localhost:7049/api/Categories");//web apı haliii
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<Category>>(jsonString);
                return View(values);
            }
            return NotFound("Kategori Listesi alınamadı");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var responseMessage = await httpClient.GetAsync("http://localhost:7049/api/Categories/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<Category>(jsonString);
                return View(value);
            }
            return NotFound("Categori bulunamadı");
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin,Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category category)
        {
            var jsonCategory = JsonConvert.SerializeObject(category);
            var stringContent = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PostAsync("http://localhost:7049/api/Categories", stringContent);
            if (responseMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin,Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            var responseMessage = await httpClient.GetAsync("http://localhost:7049/api/Categories/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<Category>(jsonString);
                return View(value);
            }
            return NotFound("Categori bulunamadı");
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            var jsonCategory = JsonConvert.SerializeObject(category);
            var stringContent = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var responseMessage = await httpClient.PutAsync("http://localhost:7049/api/Categories", stringContent);
            if (responseMessage.IsSuccessStatusCode)
                return RedirectToAction("Index");
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin,Delete")]
        public async Task<IActionResult> Delete(int? id)
        {

            var responseMessage = await httpClient.GetAsync("http://localhost:7049/api/Categories/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<Category>(jsonString);
                return View(value);
            }
            return NotFound("Categori bulunamadı");
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var responseMessage = await httpClient.DeleteAsync("http://localhost:7049/api/Categories?id=" + id);
            if (responseMessage.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            return NotFound("Categori bulunamadı");
        }
    }
}
