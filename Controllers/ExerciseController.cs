using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FCCSharp.Models;
using FCCSharp.Data;

namespace FCCSharp.Controllers;

[ApiController]
[Route("[controller]")]
public class ExerciseController : ControllerBase
{
	private readonly ExerciseDbContext _context;

	public ExerciseController(ExerciseDbContext context)
	{
		_context = context;
	}

	[HttpPost("/users")]
	public ActionResult CreateUser([FromForm] string username)
	{
		if (username == null)
			return BadRequest("Username cannot be empty");

		if (!_context.Users.Any(u => u.Username == username))
			return BadRequest("User already exists.");

		ExerciseUser newUser = new ExerciseUser
		{
			Username = username
		};
		_context.Users.Add(newUser);

		try
		{
			_context.SaveChanges();
		}
		catch (DbUpdateException e)
		{
			Console.WriteLine($"Error when attempting to save to database: {e.Message}");
			return BadRequest("Error when attempting to save to database. Please try again.");
		}

		return Ok(newUser);
	}

	[HttpGet("/users")]
	public IEnumerable<ExerciseUser> GetAll()
	{
		return _context.Users;
	}

	[HttpPost("/users/{uuid}/exercises")]
	public ActionResult AddExercise(
		string uuid,
		[FromForm] string description,
		[FromForm] string duration,
		[FromForm] string date)
	{
		ExerciseUser? user = _context.Users.Where(u => u.UserId == uuid).FirstOrDefault();
		if (user == null)
			return BadRequest("Invalid user ID.");

		uint exerciseDuration;
		try
		{
			exerciseDuration = Convert.ToUInt32(duration);
		}
		catch (FormatException)
		{
			return BadRequest("Invalid exercise duration.");
		}

		DateOnly exerciseDate;
		if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", out exerciseDate))
			return BadRequest("Invalid date.");

		Exercise ex = new Exercise
		{
			UserId = user.Id,
			Description = description,
			Duration = exerciseDuration,
			Date = exerciseDate
		};
		_context.Exercises.Add(ex);

		try
		{
			_context.SaveChanges();
		}
		catch (DbUpdateException e)
		{
			Console.WriteLine($"Error when attempting to save to database: {e.Message}");
			return BadRequest("Error when attempting to save to database. Please try again.");
		}

		return Ok();
	}

	[HttpGet("/users/{uuid}/logs")]
	public ActionResult GetLogs(string uuid)
	{
		if (String.IsNullOrEmpty(uuid))
			return NotFound();

		var queryResult = (
			from user in _context.Users
			join exercise in _context.Exercises on user.Id equals exercise.UserId
			where user.UserId == uuid
			select user.Username, exercise.Description, exercise.Duration, exercise.Date
		);

		if (!queryResult?.Any())
			return NotFound();

		ExerciseUserLog log = new ExerciseUserLog();
		log.UserId = id;
		log.Username = queryResult.First().Username;
		List<Exercise> exercises = new List<Exercise>();
		foreach (var result in queryResult)
		{
			exercises.Add(result.Exercise);
		}
		log.Exercises = exercises;

		return Ok(log);
	}
}
