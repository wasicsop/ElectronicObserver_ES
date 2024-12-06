using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbBattleSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbQuestSubmission;
using ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.PoiDbSubmission;

/// <summary>
/// I got the battle and air_base_attack endpoints working
/// couldn't get next_way_v2 working and can't get any info on what's wrong
/// 
/// battle endpoint seems to use json but doesn't work without UnsafeRelaxedJsonEscaping
///
/// air_base_attack endpoint seems to use FormUrlEncodedContent
///
/// next_way_v2 seems to use MultipartFormDataContent
///
/// not sure about the other endpoints, but probably MultipartFormDataContent
/// </summary>
public class PoiHttpClient
{
	private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
	};

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
			Logger.Add(3, requestBody);
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

		HttpResponseMessage response = await client.PostAsJsonAsync("quest", submission, JsonSerializerOptions);
		await EnsureSuccessStatusCode(response);
	}

	public async Task Battle(PoiDbBattleSubmissionData submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("battle", submission, JsonSerializerOptions);
		await EnsureSuccessStatusCode(response);
	}

	public async Task FriendFleet(Dictionary<string, Dictionary<string, JsonNode?>> submission)
	{
		using HttpClient client = MakeHttpClient();

		HttpResponseMessage response = await client.PostAsJsonAsync("friendly_info", submission, JsonSerializerOptions);
		await EnsureSuccessStatusCode(response);
	}

	public async Task AirDefense(Dictionary<string, JsonNode?> submission)
	{
		using HttpClient client = MakeHttpClient();

		FormUrlEncodedContent body = GetFormUrlEncoded(submission, true);

		HttpResponseMessage response = await client.PostAsync("air_base_attack", body);
		await EnsureSuccessStatusCode(response);
	}

	public async Task Route(PoiDbRouteSubmissionData submission)
	{
		using HttpClient client = MakeHttpClient();

		MultipartFormDataContent formData = GetMultipartFormData(submission);

		HttpResponseMessage response = await client.PostAsync("next_way_v2", formData);
		await EnsureSuccessStatusCode(response);
	}

	private static MultipartFormDataContent GetMultipartFormData(object data)
	{
		return new()
		{
			GetFormUrlEncoded(data),
		};
	}

	private static FormUrlEncodedContent GetFormUrlEncoded(object data, bool objectAsString = false)
	{
		Dictionary<string, string> formData = data.ToKeyValue(objectAsString);
		return new(formData);
	}

}
