﻿using System;
using System.Text;
using System.Text.Json;
using SS.WebApp.MVC.Extensions;

namespace SS.WebApp.MVC.Services
{
	public abstract class Service
	{
		protected StringContent GetContent(object data)
		{
			return new StringContent(
				JsonSerializer.Serialize(data),
				Encoding.UTF8,
				"application/json");
		}

		protected async Task<T> DeserializeResponseObject<T>(HttpResponseMessage responseMessage)
		{
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};

			return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
		}

		protected bool ProcessResponseErrors(HttpResponseMessage response)
		{
			switch ((int)response.StatusCode)
			{
				case 401:
				case 403:
				case 404:
				case 500:
					throw new CustomHttpRequestException(response.StatusCode);
                case 400:
					return false;
			}

			response.EnsureSuccessStatusCode();
			return true;
		}
	}
}

