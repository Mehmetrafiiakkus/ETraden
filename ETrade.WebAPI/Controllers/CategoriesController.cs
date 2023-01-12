using ETrade.Dal.Abstract;
using ETrade.Data.Models.Entites;
using Microsoft.AspNetCore.Mvc;

namespace ETrade.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        ICategoryDAl categoryDAl;
        public CategoriesController(ICategoryDAl categoryDAl)
        {
            this.categoryDAl = categoryDAl;
        }
        [HttpGet]
        public IActionResult Get()
        {

            return Ok(categoryDAl.GetAll());
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id==null||categoryDAl.GetAll()==null)
            {
                return BadRequest();//404
            }
            var category = categoryDAl.Get(Convert.ToInt32(id));
            if (category==null)
            {
                return NotFound("Categori Bullunamadı...");
            }

            return Ok(category);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                categoryDAl.Add(category);
                return CreatedAtAction("Get", new { id = category.Id },category);

            }
            return Ok();    
        }
        [HttpPut]
        public IActionResult Put([FromBody] Category category) {
            if (ModelState.IsValid)
            {
                categoryDAl.Update(category);
                return Ok(category);

            }
            return BadRequest();
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var category=categoryDAl.Get(x=>x.Id==id);
            if (category==null)
            {
                return BadRequest();
                
            }
            categoryDAl.Delete(id);
            return Ok();

        }

    }
}
