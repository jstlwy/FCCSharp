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

	[HttpPost("users")]
	public ActionResult CreateUser([FromForm] string username)
	{
		if (username == null)
			return BadRequest("Username cannot be empty");

		if (_context.Users.Any(u => u.Username == username))
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
			Console.WriteLine($"Error when attempting to save username {username} to database: {e.Message}");
			return BadRequest("There was an error when attempting to save your username to the database. Please try again.");
		}

		return Ok(new
		{
			username,
			_id = newUser.Id
		});
	}

	[HttpGet("users")]
	public IEnumerable<ExerciseUser> GetAll()
	{
		return _context.Users;
	}

	[HttpPost("users/{username}/exercises")]
	public ActionResult AddExercise(
		string username,
		[FromForm] string description,
		[FromForm] string duration,
		[FromForm] string? date)
	{
		var user = _context.Users.First(u => u.Username == username);
		if (user == null)
			return BadRequest($"The username you provided, {username}, could not be found.");

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
		if (String.IsNullOrEmpty(date))
			exerciseDate = DateOnly.FromDateTime(DateTime.Now);
		else if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", out exerciseDate))
			return BadRequest("Invalid date.");

		Exercise ex = new()
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

		return Ok(new
		{
			username,
			description,
			duration,
			date = exerciseDate,
			_id = user.Id
		});
	}

	[HttpGet("users/{username}/logs")]
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

		if (queryResult == null || !queryResult.Any())
			return NotFound($"The username you provided, {username}, could not be found.");

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
