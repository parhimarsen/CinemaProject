namespace CinemaProject.BLL.Models
{
    public class RegistrationResponse
    {
        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }

        public string Status { get; set; }

        public RegistrationResponse(string jwtToken, string refreshToken, string status)
        {
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            Status = status;
        }
    }
}
