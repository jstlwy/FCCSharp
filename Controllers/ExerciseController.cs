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

	[HttpPost("/users/{username}/exercises")]
	public ActionResult AddExercise(
		string username,
		[FromForm] string description,
		[FromForm] string duration,
		[FromForm] string date)
	{
		ExerciseUser? user = _context.Users.Where(u => u.Username == username).FirstOrDefault();
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

	[HttpGet("/users/{username}/logs")]
	public ActionResult GetLogs(string username)
	{
		if (String.IsNullOrEmpty(username))
			return NotFound();

		var queryResult = (
			from user in _context.Users
			join exercise in _context.Exercises on user.Id equals exercise.UserId
			where user.Username == username
			select new
			{
				user.Id,
				user.Username,
				exercise.Description,
				exercise.Duration,
				exercise.Date
			}
		);

		if (queryResult == null)
			return NotFound();

		ExerciseUserLog log = new ExerciseUserLog();
		var firstResult = queryResult.First();
		log.UserId = firstResult.Id;
		log.Username = queryResult.First().Username;
		List<ExerciseLog> exercises = new List<ExerciseLog>();
		foreach (var result in queryResult)
		{
			ExerciseLog e = new ExerciseLog
			{
				Description = result.Description,
				Duration = result.Duration,
				Date = result.Date
			};
			exercises.Add(e);
		}
		log.Exercises = exercises;
		log.Count = exercises.Count;

		return Ok(log);
	}
}
