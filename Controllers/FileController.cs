using Microsoft.AspNetCore.Mvc;

namespace FCCSharp.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    [HttpPost("analyze")]
    public ActionResult OnPostUpload(IFormFile upfile)
    {
        if (upfile.Length <= 0) 
            return BadRequest("The file you uploaded was empty.");

        return Ok(new {
            name = upfile.FileName,
            type = upfile.ContentType,
            size = upfile.Length
        });
    }
}
