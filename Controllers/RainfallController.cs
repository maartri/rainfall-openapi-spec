using Microsoft.AspNetCore.Mvc;
using rainfall_openapi_spec.Models;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace rainfall_openapi_spec.Controllers;

[ApiController]
[Route("rainfall")]
[Produces("application/json")]
public class RainfallController : ControllerBase
{
    private readonly string UK_URI = "http://environment.data.gov.uk/flood-monitoring";
    private readonly HttpClient _httpClient;
    
    public RainfallController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri(UK_URI);
    }

    /// <summary>
    ///     Get rainfall readings by station Id
    /// </summary>
    /// <remarks>
    ///     Retrieve the latest readings for the specified stationId
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [Route("id/{stationId}/readings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(string stationId, int count = 10)
    {
        if(count > 0 && count < 101)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{_httpClient.BaseAddress}/id/stations/{stationId}/readings");
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(new ErrorMessage() { Description = "Invalid Request", Message = response.ReasonPhrase });
                }

                var rainReadingResponse = await JsonSerializer.DeserializeAsync<RainReading>(await response.Content.ReadAsStreamAsync());
                if (rainReadingResponse is not null && rainReadingResponse.Items.Any())
                {
                    return Ok(rainReadingResponse.Items); 
                }
                return NotFound(new ErrorMessage() { Description = "No readings found for the specified stationId", Message = "No readings found for the specified stationId" });
            } catch (Exception ex)
            {
                return StatusCode(500, new ErrorMessage() { Description = "Internal Server Error", Message = ex.ToString() });
            }
        }
        return BadRequest(new ErrorMessage() { Description = "Invalid Request", Message = "Results should number in between or equal to 1 and 100" });
    }
}
