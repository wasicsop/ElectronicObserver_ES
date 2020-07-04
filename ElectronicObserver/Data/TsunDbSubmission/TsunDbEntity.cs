using ElectronicObserver.Utility;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
	public abstract class TsunDbEntity
	{
		protected abstract string Url { get; }

		public void SendData()
		{
			Task sendData = new Task(async () =>
			{
				try
				{
					var result = await this.HttpSendData();
					string response = result.Content.ReadAsStringAsync().Result;

					if (!result.IsSuccessStatusCode)
					{
						throw new Exception("Error " + result.StatusCode + ": " + response);
					}
				}
				catch (Exception ex)
				{
					Utility.ErrorReporter.SendErrorReport(ex, "TsunDb Submission module");
				}
			});

			sendData.Start();
		}

		private async Task<HttpResponseMessage> HttpSendData()
		{			
			using (var httpClient = new HttpClient())
			{
				JsonSerializerSettings jsonSerializer = new JsonSerializerSettings()
				{
					CheckAdditionalContent = true,
					NullValueHandling = NullValueHandling.Ignore
				};

				string contentSerialized = JsonConvert.SerializeObject(this, jsonSerializer);
				StringContent content = new StringContent(contentSerialized);

				content.Headers.Add("tsun-ver", "Kasumi Kai");
				content.Headers.Add("dataorigin", "eo");
				content.Headers.Add("version", SoftwareInformation.VersionEnglish);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				if (Configuration.Config.Debug.EnableDebugMenu) File.WriteAllText($"tsundb_{this.Url}.json", contentSerialized);

				return await httpClient.PutAsync($"https://tsundb.kc3.moe/api/{this.Url}", content);
			}
		}
	}
}