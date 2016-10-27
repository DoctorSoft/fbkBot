﻿using System.Net;

namespace RequestsHelpers
{
    public static class RequestsHelper
    {
        public static string Get(string url, string cookie)
        {
            var client = new WebClient();

            client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.143 Safari/537.36";
            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            client.Headers[HttpRequestHeader.Accept] = "*/*";
            client.Headers[HttpRequestHeader.AcceptLanguage] = "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4";
            client.Headers.Add(HttpRequestHeader.Cookie, cookie);
            var answer = client.DownloadString(url);
            return answer;
        }

        public static string Post(string url, string parameters, string cookie)
        {
            throw new System.NotImplementedException();
        }
    }
}