using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using ServicioAPI.Data.Entities;
using ServicioAPI.Data.Repositories;

namespace ServicioAPI
{
    public static class Extensions
    {
        public static string AbsoluteContent(this IUrlHelper url, string contentPath)
        {
            var request = url.ActionContext.HttpContext.Request;
            return $"{request.Scheme}://{request.Host.Value}{url.Content(contentPath)}";
        }

        public static bool TryGetToken(this HttpRequest request, out string token)
        {
            bool hasToken = request.Headers.TryGetValue("Authorization", out StringValues values);

            token = hasToken ? values[0].Replace("Bearer ", "") : null;

            return hasToken;
        }

        public static bool IsAuthorized(this HttpRequest request, UserRepository userRepository, out User user)
        {
            bool isAuthorized = false;
            user = null;

            if (request.TryGetToken(out string token))
            {
                user = userRepository.GetUserByTokenAsync(token).Result;
                isAuthorized = user != null;
            }

            return isAuthorized;
        }
    }
}
