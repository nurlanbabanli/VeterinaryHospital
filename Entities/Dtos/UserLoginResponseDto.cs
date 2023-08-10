namespace Entities.Dtos
{
    public class UserLoginResponseDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        public List<string> Roles { get; set; }
    }
}
