using System;
using IChaveiro.DTO;
using Realms;

namespace IChaveiro.ModelRealm
{
    public class TokenRealm : RealmObject
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset Fresh { get; set; }
        public DateTimeOffset ExpiresAccess { get; set; }
        public DateTimeOffset ExpiresRefresh { get; set; }

        public TokenRealm()
        {
        }

        public TokenRealm(TokenDTO _token)
        {
            AccessToken = _token.access_token;
            RefreshToken = _token.refresh_token;
            Fresh = _token.fresh;
            ExpiresAccess = _token.expires_access;
            ExpiresRefresh = _token.expires_refresh;
        }
    }
}