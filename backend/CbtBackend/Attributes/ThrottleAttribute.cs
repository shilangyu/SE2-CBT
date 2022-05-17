using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CbtBackend.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class Throttle : ActionFilterAttribute {
    public static bool Bypass = false;

    private readonly int limit;
    private readonly double durationMinutes;

    public bool ByAction { get; set; } = true;
    public bool ByIpAddress { get; set; } = true;
    public bool BypassLocalHost { get; set; } = true;

    private static readonly ConcurrentDictionary<string, List<DateTime>> History = new();

    public Throttle(int limit, double durationMinutes) {
        this.limit = limit;
        this.durationMinutes = durationMinutes;
    }

    public override void OnActionExecuting(ActionExecutingContext context) {
        base.OnActionExecuting(context);

        if (Bypass) {
            return;
        }

        if (BypassLocalHost && context.HttpContext.Connection.RemoteIpAddress is {
                AddressFamily: AddressFamily.InterNetwork or AddressFamily.InterNetworkV6
            }) {
            return;
        }

        var key = BuildKey(context);
        if (key is null) {
            // failed to build the key, no throttle will be applied
            // TODO: log it (warning)
            return;
        }

        if (!History.ContainsKey(key)) {
            History.TryAdd(key, new List<DateTime>());
        }

        var list = History[key];
        lock (list) {
            var now = DateTime.Now;
            var validSince = now - TimeSpan.FromMinutes(durationMinutes);
            var bs = list.BinarySearch(validSince);

            if (bs < 0) {
                bs = ~bs;
            }

            if (bs > 0) {
                list.RemoveRange(0, bs);
            }

            if (list.Count < limit) {
                list.Add(now);
            } else {
                ResultRateLimit(context);
            }
        }
    }

    private string? BuildKey(ActionExecutingContext context) {
        var sb = new StringBuilder();

        if (ByAction) {
            sb.Append(context.ActionDescriptor.Id);
            sb.Append(';');
        }

        if (ByIpAddress) {
            if (context.HttpContext.Connection.RemoteIpAddress is null) {
                return null;
            }

            sb.Append(context.HttpContext.Connection.RemoteIpAddress);
            sb.Append(';');
        }

        return sb.ToString();
    }

    private static void ResultRateLimit(ActionExecutingContext context) {
        context.Result = new StatusCodeResult((int)HttpStatusCode.TooManyRequests);
    }
}
