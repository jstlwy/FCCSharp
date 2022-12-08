using Microsoft.AspNetCore.Mvc;
using FCCSharp.Models;

namespace FCCSharp.Controllers;

[ApiController]
[Route("[controller]")]
public class ExerciseController : ControllerBase
{
	[HttpPost("/users")]
	public ActionResult CreateUser()
	{
		
	}

	[HttpGet("/users")]
	public IEnumerable<ExerciseUser> GetAll()
	{

	}

	[HttpPost("/users/{id}/exercises")]
	public ActionResult AddExercise(int id)
	{

	}

	[HttpGet("/users/{id}/logs")]
	public IEnumerable<> GetLogs(int id)
	{
		
	}
}
