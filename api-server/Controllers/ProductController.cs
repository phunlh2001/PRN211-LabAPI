
using System.Linq;
using System.Threading.Tasks;
using api_server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_server.Controllers
{
  [ApiController]
  [Route("api")]
  public class ProductController : ControllerBase
  {
    private readonly ApplicationDbContext _db;
    public ProductController(ApplicationDbContext db) => _db = db;

    // [GET] api/getall
    [HttpGet("getall")]
    public IActionResult GetAll()
    {
      var list = _db.Products.Select(p => new
      {
        p.ID,
        p.Name,
        p.Price,
        p.Stock,
        p.CateID
      }).ToList();

      if (list == null)
      {
        return NotFound("Khong co san pham nao.");
      }
      return Ok(list);
    }

    // [GET] api/categories
    [HttpGet("categories")]
    public IActionResult GetCategories()
    {
      var list = _db.Categories.Select(c => new
      {
        c.ID,
        c.Name
      }).ToList();

      if (list == null)
      {
        return NotFound("Khong co loai san pham nao.");
      }
      return Ok(list);
    }

    // [GET] api/product/2
    [HttpGet("product/{id}", Name = "product")]
    public async Task<IActionResult> GetOne(int id)
    {
      var prod = await _db.Products.FirstOrDefaultAsync(p => p.ID == id);

      if (prod == null)
      {
        return NotFound("San pham khong ton tai.");
      }
      return Ok(prod);
    }

    // [POST] api/add
    [HttpPost("add", Name = "add")]
    public async Task<IActionResult> AddProduct(Product prod)
    {
      if (prod == null)
      {
        return BadRequest("Thong tin san pham khong duoc null.");
      }
      if (ModelState.IsValid)
      {
        await _db.Products.AddAsync(prod);
        _db.SaveChanges();
      }
      return CreatedAtRoute("product", new { id = prod.ID }, new
      {
        prod.ID,
        prod.Name,
        prod.Price,
        prod.Stock,
        prod.Category
      });
    }

    // [DELETE] api/delete/3
    [HttpDelete("delete/{id}", Name = "delete")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
      var prod = await _db.Products.FindAsync(id);
      if (prod == null)
      {
        return BadRequest("san pham khong ton tai.");
      }
      try
      {
        _db.Products.Remove(prod);
        _db.SaveChanges();
        return Ok(prod);
      }
      catch
      {
        return NotFound("co loi");
      }
    }

    // [PUT] api/edit
    [HttpPut("edit/{id}", Name = "edit")]
    public IActionResult UpdateProduct(int id, Product prod)
    {
      if (prod == null)
      {
        return BadRequest("Thong tin san pham khong duoc null.");
      }
      Product obj = this._db.Products.AsNoTracking().FirstOrDefault(x => x.ID == id);
      if (obj == null)
      {
        return NotFound("San pham khong ton tai!!");
      }
      this._db.Products.Update(prod);
      this._db.SaveChanges();
      return Ok(prod);
    }
  }
}