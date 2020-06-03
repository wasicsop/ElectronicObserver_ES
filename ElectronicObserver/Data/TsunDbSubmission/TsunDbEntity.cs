using Codeplex.Data;
using ElectronicObserver.Utility;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

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

				return await httpClient.PutAsync($"https://tsundb.kc3.moe/api/{this.Url}", content);
			}


			/*sendData: function(payload, type) {
				//console.debug(JSON.stringify(payload));
				$.ajax({
					url: `https://tsundb.kc3.moe/api/${type}`,
					method: 'PUT',
					headers:
						{
							'content-type': 'application/json',
						'tsun-ver': 'Kasumi Kai',
						'dataorigin': 'kc3',
						'version': this.kc3version
					},
					data: JSON.stringify(payload)
				}).done(function() {
						console.log(`Tsun DB Submission to /${ type}
						done.`);
					}).fail(function(jqXHR, textStatus, error) {
						const statusCode = jqXHR.status;
						if (statusCode === 400)
						{
							// Server-side defines: '400 Bad Request' = status can be ignored
							console.log(`Tsun DB Submission to /${ type} ${ textStatus}`, statusCode, error);
						}
						else
						{
							console.warn(`Tsun DB Submission to /${ type} ${ textStatus}`, statusCode, error);
						}
					});
					return;
				}*/
		}
	}
}