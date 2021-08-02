namespace CinemaProject.BLL.Models
{
    public class AuthenticateResponse
    {
        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }

        public User User { get; set; }

        public AuthenticateResponse(string jwtToken, string refreshToken, User user)
        {
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            User = user;
        }
    }
}
