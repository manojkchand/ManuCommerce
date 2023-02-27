﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SignalRQueryStringAuthMiddleware
{
    private readonly RequestDelegate _next;

    public SignalRQueryStringAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Convert incomming qs auth token to a Authorization header so the rest of the chain
    // can authorize the request correctly
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers["Connection"] == "Upgrade" &&
            context.Request.Query.TryGetValue("authToken", out var token))
        {
            context.Request.Headers.Add("Authorization", "Bearer " + token.First());
        }
        await _next.Invoke(context);
    }
}

public static class SignalRQueryStringAuthExtensions
{
    public static IApplicationBuilder UseSignalRQueryStringAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SignalRQueryStringAuthMiddleware>();
    }
}
