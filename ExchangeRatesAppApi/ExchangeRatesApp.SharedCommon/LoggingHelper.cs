using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRatesApp.SharedCommon
{
    public static class LoggingHelper
    {
        public static readonly string ScopedLogMessageForHandler = "Handler name: " + ScopedLogPlaceholders.HandlerName
                                                                        + " | Logged user: " + ScopedLogPlaceholders.LoggedUser
                                                                        + " | Input data: " + ScopedLogPlaceholders.InputData
                                                                        + " | Execution Context Id: " + ScopedLogPlaceholders.ExecutionContextId;

        public static readonly string ScopedLogMessageForRequest = "Path: " + ScopedLogPlaceholders.HandlerName
                                                                        + " | Logged user: " + ScopedLogPlaceholders.LoggedUser
                                                                        + " | POST body: " + ScopedLogPlaceholders.InputData
                                                                        + " | Execution Context Id: " + ScopedLogPlaceholders.ExecutionContextId;

        public static Dictionary<string, object> GetScopedDictionary(string? executionContextId, string? handlerName, string? currentUser, string? inputData)
        {
            var dict = new Dictionary<string, object>
            {
                [ScopedLogPlaceholders.ExecutionContextId.LabelValue] = executionContextId ?? "N/A",
                [ScopedLogPlaceholders.HandlerName.LabelValue] = handlerName ?? "N/A",
                [ScopedLogPlaceholders.LoggedUser.LabelValue] = currentUser ?? "N/A",
                [ScopedLogPlaceholders.InputData.LabelValue] = inputData ?? "N/A"
            };

            return dict;
        }

        public static async Task<(string executionContextId, string handlerName, string currentUser, string inputData)> FetchLoggingDataFromRequest(this HttpContext? context)
        {
            var handlerName = context?.Request?.Path.ToString() ?? "N/A";
            handlerName += $" ({(context?.Request?.Method?.ToUpper() ?? "N/A")})";

            var currentUser = context?.User.FindFirst(ClaimTypes.Name)?.Value ?? "N/A";
            var inputData = await GetPostBody(context!) ?? "N/A";

            return (context?.TraceIdentifier ?? "N/A", handlerName, currentUser, inputData);
        }

        public static void SetScopedLoggingData(this HttpContext? context, string handlerName, string currentUser, string inputData)
        {
            context!.Items[ScopedLogPlaceholders.HandlerName.LabelValue] = handlerName;
            context.Items[ScopedLogPlaceholders.LoggedUser.LabelValue] = currentUser;
            context.Items[ScopedLogPlaceholders.InputData.LabelValue] = inputData;
        }

        /// <summary>
        /// Use for logging purposes only.
        /// (Method ignores exceptions)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToJsonForLog<T>(this T obj) where T : class?
        {
            try
            {
                if (obj == null)
                {
                    return "[]";
                }


                var data = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                if (data == null)
                {
                    return "[]";
                }

                return JsonConvert.SerializeObject(data,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                return $"[{(ex.Message == null ? "ERROR during creating the log" : $"ERROR during creating the log: {ex.Message}")}]";
            }
        }

        private static async Task<string?> GetPostBody(HttpContext context)
        {
            string? inputData = null;

            var request = context?.Request;
            if (request != null && request.Method == HttpMethods.Post && request.ContentLength > 0)
            {
                request.EnableBuffering();
                request.Body.Position = 0;
                using var ms = new MemoryStream();
                byte[] buffer = new byte[1024];
                int readCount;

                while ((readCount = await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length), default)) > 0)
                {
                    await ms.WriteAsync(buffer.AsMemory(0, readCount));
                }

                inputData = Encoding.UTF8.GetString(ms.ToArray()) ?? "N/A";

                request.Body.Position = 0;
            }

            return inputData;
        }

    }

    public static class ScopedLogPlaceholders
    {
        public static ScopedLogPlaceholder HandlerName => new("HandlerName");
        public static ScopedLogPlaceholder LoggedUser => new("LoggedUser");
        public static ScopedLogPlaceholder InputData => new("InputData");
        public static ScopedLogPlaceholder ExecutionContextId => new("ExecutionContextId");
    }

    public class ScopedLogPlaceholder
    {
        public string LabelValue { get; private set; }

        public ScopedLogPlaceholder(string value)
        {
            LabelValue = value;
        }

        public string Value => "{" + LabelValue + "}";

        public override string ToString()
        {
            return Value;
        }
    }
}
