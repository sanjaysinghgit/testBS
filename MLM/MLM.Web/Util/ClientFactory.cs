using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json.Linq;

namespace MLM.Util
{
    public class ClientFactory : ClientFactoryBase
    {
        private const bool _useCookies = false;
        private const string AuthorizationScheme = "Bearer";
        private readonly IBazingaConfig _bazingaConfig;

        public ClientFactory(IBazingaConfig bazingaConfig)
        {
            _bazingaConfig = bazingaConfig;
        }

        public HttpClient CreateWithHeader(HttpContextBase context)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_bazingaConfig.GetSetting("ApiUrl"));
            client.DefaultRequestHeaders.Add("UserAgent", "Web." + context.Request.UserAgent);

            //Add Authentication token
            var authCookieToken = ClientFactoryBase.GetHeaderFromClientRequest(context, "AuthorizationToken");
            if (!string.IsNullOrWhiteSpace(authCookieToken))
            {
                var authorization = AuthorizationScheme + " " + authCookieToken;
                client.DefaultRequestHeaders.Add(_bazingaConfig.GetSetting("AuthCookieName"), authorization);
            }

            //Add X-Bazinga-DevelopmentId to request if exists
            var bazingaDevelopmentId = ClientFactoryBase.GetHeaderFromClientRequest(context, "X-Bazinga-DevelopmentId");
            if (!string.IsNullOrWhiteSpace(bazingaDevelopmentId))
            {
                client.DefaultRequestHeaders.Add("X-Bazinga-DevelopmentId", bazingaDevelopmentId);
            }
            return client;
        }

        public HttpClient CreateWithCookie(HttpContextBase context)
        {
            var authToken = context.Request.Cookies.Get(_bazingaConfig.GetSetting("AuthCookieName")) == null ? "" : context.Request.Cookies.Get(_bazingaConfig.GetSetting("AuthCookieName")).Value;

            var client = new HttpClient();
            client.BaseAddress = new Uri(_bazingaConfig.GetSetting("ApiUrl"));
            client.DefaultRequestHeaders.Add("UserAgent", "Web." + context.Request.UserAgent);
            
            if (!string.IsNullOrEmpty(authToken))
            {
                var authorization = AuthorizationScheme + " " + authToken;
                client.DefaultRequestHeaders.Add(_bazingaConfig.GetSetting("AuthorizationHeaderName"), authorization);
            }
            
            return client;
        }

        public override HttpResponseMessage Login(string username, string password)
        {
            var idsrvEndpoint = _bazingaConfig.GetSetting("AuthUrl");
            var realm = _bazingaConfig.GetSetting("OauthRealm");
            var client = new HttpClient { BaseAddress = new Uri(idsrvEndpoint) };

            client.DefaultRequestHeaders.Authorization =
                new BasicAuthenticationHeaderValue(_bazingaConfig.GetSetting("ClientId"),
                    _bazingaConfig.GetSetting("ClientSecret"));

            var values = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.Password },
                { OAuth2Constants.UserName, username },
                { OAuth2Constants.Password, password },
                { OAuth2Constants.Scope, realm }
            };

            var form = new FormUrlEncodedContent(values);

            Logger.Debug(new LogEntry
            {
                Message = "Getting authorization token.",
                Object = new { Username = username, Scope = realm }
            });

            return client.PostAsync("", form).Result;
        }

        public bool Login(string username, string password, HttpContextBase context)
        {
            var idsrvEndpoint = _bazingaConfig.GetSetting("AuthUrl");
            var realm = _bazingaConfig.GetSetting("OauthRealm");
            var client = new HttpClient { BaseAddress = new Uri(idsrvEndpoint) };

            client.DefaultRequestHeaders.Authorization =
                new BasicAuthenticationHeaderValue(_bazingaConfig.GetSetting("ClientId"),
                    _bazingaConfig.GetSetting("ClientSecret"));

            var values = new Dictionary<string, string>
            {
                { OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.Password },
                { OAuth2Constants.UserName, username },
                { OAuth2Constants.Password, password },
                { OAuth2Constants.Scope, realm }
            };

            var form = new FormUrlEncodedContent(values);

            Logger.Debug(new LogEntry
            {
                Message = "Getting authorization token.",
                Object = new { username = username, scope = realm }
            });

            var response = client.PostAsync("", form).Result;
            if (!response.IsSuccessStatusCode)
            {

                // Log failed token request
                Logger.Error(new LogEntry
                {
                    Message = "Get authorization token failed.",
                    Object = new { username = username, statusCode = response.StatusCode, reasonPhrase = response.ReasonPhrase }
                });

                return false;
            }

            var tokenResponse = response.Content.ReadAsStringAsync().Result;

            var json = JObject.Parse(tokenResponse);
            var authToken = json["access_token"].ToString();
            var refreshToken = json["refresh_token"].ToString();
            var expireTime = json["expires_in"].ToString();
            int expireSeconds;
            if (!int.TryParse(expireTime, out expireSeconds))
            {
                expireSeconds = 3600;
            }

            Logger.Debug(new LogEntry
            {
                Message = "Get authorization token succeeded.",
                Object = new
                {
                    refreshToken = refreshToken,
                    authorizationToken = authToken,
                    expiresApproxAtUtc = DateTime.UtcNow.AddSeconds(expireSeconds)
                }
            });

            SetCookies(authToken, refreshToken, expireSeconds, context);

            return true;
        }
        
        private void SetCookies(string authToken, string refreshToken, int expireTime, HttpContextBase context)
        {
            var authcookie = new HttpCookie(_bazingaConfig.GetSetting("AuthCookieName"), authToken);
            var refreshcookie = new HttpCookie(_bazingaConfig.GetSetting("RefreshTokenCookieName"), refreshToken);
            var expiretimecookie = new HttpCookie(_bazingaConfig.GetSetting("ExpireTimeCookieName"), expireTime.ToString());

            authcookie.HttpOnly = false;
            refreshcookie.HttpOnly = false;
            expiretimecookie.HttpOnly = false;
            context.Response.SetCookie(authcookie);
            context.Response.SetCookie(refreshcookie);
            context.Response.SetCookie(expiretimecookie);
        }

        private bool NearExpiry(HttpContextBase context)
        {
            var httpCookie = context.Request.Cookies.Get(_bazingaConfig.GetSetting("ExpireTimeCookieName"));
            if (httpCookie != null)
            {
                long ms;
                if (long.TryParse(httpCookie.Value, out ms))
                {
                    var now = DateTime.Now;
                    var expires = new DateTime(ms * TimeSpan.TicksPerMillisecond);
                    var timeDiff = expires - now;
                    return timeDiff.TotalMinutes < 10;
                }
            }

            return false;
        }

        public HttpResponseMessage RefreshWithHeader(string refreshToken)
        {
            var idsrvEndpoint = _bazingaConfig.GetSetting("AuthUrl");
            var client = new HttpClient { BaseAddress = new Uri(idsrvEndpoint) };

            client.DefaultRequestHeaders.Authorization =
                new BasicAuthenticationHeaderValue(_bazingaConfig.GetSetting("ClientId"),
                    _bazingaConfig.GetSetting("ClientSecret"));


            Logger.Debug(new LogEntry
            {
                Message = "Refreshing authorization token.",
                Object = new { refreshToken = refreshToken }
            });

            var values = new Dictionary<string, string>
                {
                    { OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.RefreshToken },
                    { OAuth2Constants.GrantTypes.RefreshToken, refreshToken }
                };

            var form = new FormUrlEncodedContent(values);
            return client.PostAsync("", form).Result;
        }

        public string RefreshWithCookie(HttpContextBase context)
        {
            var idsrvEndpoint = _bazingaConfig.GetSetting("AuthUrl");
            var client = new HttpClient { BaseAddress = new Uri(idsrvEndpoint) };

            client.DefaultRequestHeaders.Authorization =
                new BasicAuthenticationHeaderValue(_bazingaConfig.GetSetting("ClientId"),
                    _bazingaConfig.GetSetting("ClientSecret"));
            var httpCookie = context.Request.Cookies.Get(_bazingaConfig.GetSetting("RefreshTokenCookieName"));
            if (httpCookie != null)
            {
                var refreshToken = httpCookie.Value;

                Logger.Debug(new LogEntry
                {
                    Message = "Refreshing authorization token.",
                    Object = new { refreshToken = refreshToken }
                });

                var values = new Dictionary<string, string>
                    {
                        { OAuth2Constants.GrantType, OAuth2Constants.GrantTypes.RefreshToken },
                        { OAuth2Constants.GrantTypes.RefreshToken, refreshToken }
                    };

                var form = new FormUrlEncodedContent(values);

                var response = client.PostAsync("", form).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Logger.Warn(new LogEntry
                    {
                        Message = "Refresh authorization token failed.",
                        Object = new { refreshToken = refreshToken, statusCode = response.StatusCode, reasonPhrase = response.ReasonPhrase }
                    });

                    return null;
                }

                var tokenResponse = response.Content.ReadAsStringAsync().Result;

                var json = JObject.Parse(tokenResponse);
                var authToken = json["access_token"].ToString();
                var newRefreshToken = json["refresh_token"].ToString();
                var expireTime = json["expires_in"].ToString();
                int expireSeconds;
                int.TryParse(expireTime, out expireSeconds);

                Logger.Debug(new LogEntry
                {
                    Message = "Refresh authorization token succeeded.",
                    Object = new
                    {
                        oldRefreshToken = refreshToken,
                        newRefreshToken = newRefreshToken,
                        newAuthorizationToken = authToken,
                        expiresApproxAtUtc = DateTime.UtcNow.AddSeconds(expireSeconds)
                    }
                });

                SetCookies(authToken, newRefreshToken, expireSeconds, context);

                return authToken;
            }

            return string.Empty;
        }


        public override HttpClient Create(HttpContextBase context)
        {
            return CreateWithHeader(context);
        }

        public override HttpResponseMessage Refresh(string refreshToken)
        {
            return RefreshWithHeader(refreshToken);
        }
    }
}
