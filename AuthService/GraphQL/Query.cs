namespace AuthService.GraphQL
{
    public class Query
    {
        public UserType GetUser([Service] IHttpContextAccessor httpContextAccessor)
        {
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            return new UserType { Username = username };
        }
    }
}