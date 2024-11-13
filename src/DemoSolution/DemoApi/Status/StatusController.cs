

namespace HtTemplate.Status;

public class StatusController : ControllerBase
{

    // "Document" resource. 

    [HttpGet("/status")]
    public async Task<ActionResult> GetTheStatus()
    {
        return Ok("This is the status for right now.");
    }

    [HttpGet("/status/{year:int}/{month:int:max(12):min(1)}/{day:int}")]
    public async Task<ActionResult> GetTheStatus(int year, int month, int day)
    {
        return Ok($"The Status for {year}/{month}/{day}");
    }

    [HttpGet("/employees")]
    public async Task<ActionResult> GetEmployees(
        [FromQuery]
        string department = "All")
    {

        // you are returning to the user whatever they need to give us more money.
        return Ok($"Here are {department} the employees...");
    }


}
