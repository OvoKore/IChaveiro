using System;
namespace IChaveiro.DTO
{
    public class TokenDTO : MsgDTO
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public DateTime fresh { get; set; }
        public DateTime expires_access { get; set; }
        public DateTime expires_refresh { get; set; }

        public TokenDTO()
        {
        }
    }
}