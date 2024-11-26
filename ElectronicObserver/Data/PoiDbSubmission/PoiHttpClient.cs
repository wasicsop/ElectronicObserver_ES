using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbQuestSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

namespace ElectronicObserver.Data.PoiDbSubmission;

public class PoiHttpClient
{
	private static HttpClient MakeHttpClient() => new()
	{
		BaseAddress = new("http://report2.kcwiki.org:17027/api/report/"),
	};

	private static async Task EnsureSuccessStatusCode(HttpResponseMessage response)
	{
#if DEBUG
		if (response.RequestMessage?.Content is HttpContent content)
		{
			string requestBody = await content.ReadAsStringAsync();
			Utility.Logger.Add(3, requestBody);
		}
#endif

		if (response.IsSuccessStatusCode) return;

		string responseMessage = await response.Content.ReadAsStringAsync();
		string errorMessage = $"{response.ReasonPhrase} {responseMessage}";

		throw new HttpRequestException(errorMessage);
	}

	public async Task Quest(PoiDbQuestSubmissionData submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("quest", submission);
		await EnsureSuccessStatusCode(response);
	}

	public async Task Battle(PoiDbBattleSubmissionData submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("battle", submission);
		await EnsureSuccessStatusCode(response);
	}

	public async Task FriendFleet(Dictionary<string, Dictionary<string, JsonNode?>> submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("friendly_info", submission);
		await EnsureSuccessStatusCode(response);
	}

	public async Task AirDefense(Dictionary<string, JsonNode?> submission)
	{
		using HttpClient client = MakeHttpClient();

		MultipartFormDataContent formData = new()
		{
			JsonContent.Create(submission),
		};

		HttpResponseMessage response = await client.PostAsync("air_base_attack", formData);
		await EnsureSuccessStatusCode(response);
	}

	public async Task Route(PoiDbRouteSubmissionData submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("next_way_v2", submission);
		await EnsureSuccessStatusCode(response);
	}
}
