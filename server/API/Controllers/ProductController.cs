using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using Service.TransferModels.Responses;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IDMShopService service, IOptionsMonitor<AppOptions> options) : ControllerBase
{

    [HttpGet]
    [Route("basic")]
    public ActionResult<List<ProductDto>> GetAllPapers()
    {
        var papers = service.GetAllPapers();
        return Ok(papers);
    }
    
    [HttpGet]
    [Route("with-properties")]
    public ActionResult<List<ProductDto>> GetAllPapersWithProperties()
    {
        var papers = service.GetAllPapersWithProperties();
            
        if (papers == null || !papers.Any())
        {
            return NotFound("No papers found.");
        }
            
        return Ok(papers);
    }

    [HttpPost]
    [Route("")]
    public ActionResult<ProductDto> CreatePaper([FromBody] ProductDto productDto)
    {
        if (productDto == null)
        {
            return BadRequest("Invalid product");
        }

        var createdProduct = service.CreatePaper(productDto);
        return CreatedAtAction(nameof(GetAllPapers), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult<ProductDto> DeletePaper(int id, [FromBody] ProductDto productDto)
    {
        try
        {
            var deletedProduct = service.DeletePaper(id, productDto); // Pass the productDto
            return Ok(deletedProduct);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }


    [HttpPut]
    [Route("{id}")]
    public ActionResult<ProductDto> UpdatePaper(int id, [FromBody] ProductDto productDto)
    {
        if (productDto == null)
        {
            return BadRequest("Product data cannot be null.");
        }

        // Ensure the ID in the DTO matches the ID in the route
        if (productDto.Id != id)
        {
            return BadRequest("Product ID mismatch.");
        }

        // Extract property IDs from the productDto
        var propertyIds = productDto.Properties?.Select(p => p.Id).ToList() ?? new List<int>();
        
        var updatedProduct = service.UpdatePaper(id, productDto, propertyIds);

        return Ok(updatedProduct);
    }
    
    [HttpGet]
    [Route("{id}")]
    public ActionResult<ProductDto> GetPaperById(int id)
    {
        var paper = service.GetPaperById(id);
        if (paper == null)
        {
            return NotFound("Paper not found.");
        }
        return Ok(paper);
    }

    [HttpGet]
    [Route("filter")]
    public IActionResult GetPapersByProperties([FromQuery] List<int> propertyIds)
    {
        var papers = service.GetPapersByProperties(propertyIds);
        return Ok(papers);
    }
    
}