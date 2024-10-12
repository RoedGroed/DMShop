using Microsoft.AspNetCore.Mvc;
using Service;
using Service.TransferModels.Responses.Products;


namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController(IDmShopService service) : ControllerBase
{

    [HttpGet]
    [Route("")]
    public ActionResult<List<PropertyDto>> GetAllProperties()
    {
        var properties = service.GetAllProperties();
        if (!properties.Any())
        {
            return NotFound("No properties found");
        }
        return Ok(properties);
    }

    [HttpPost]
    [Route("")]
    public ActionResult<PropertyDto> CreateProduct([FromBody] PropertyDto propertyDto)
    {
        if (propertyDto == null)
        {
            return BadRequest("Invalid property");
        }

        var createdProperty = service.CreateProperty(propertyDto);
        return CreatedAtAction(nameof(GetAllProperties), new { id = createdProperty.Id}, createdProperty);
    }
    

    [HttpDelete]
    [Route("{id}")]
    public ActionResult<PropertyDto> DeleteProperty(int id)
    {
        try
        {
            var deletedProperty = service.DeleteProperty(id);
            return Ok(deletedProperty);
        }
        catch (ArgumentException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut]
    [Route("{id}")]
    public ActionResult<PropertyDto> UpdateProperty(int id, [FromBody] PropertyDto propertyDto)
    {
        if (propertyDto == null)
        {
            return BadRequest("Invalid property data");
        }

        if (propertyDto.Id != id)
        {
            return BadRequest("Property Id mismatch");
        }
        var updatedProperty = service.UpdateProperty(propertyDto);
        return Ok(updatedProperty); 
        
    }

}