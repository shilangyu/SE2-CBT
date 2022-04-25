namespace CbtBackend.Contracts;

public static class ApiRoutes {

    // api endpoints are given in pg 55. of the spec
    public static class User {
        public const string Register = "user";              // POST
        public const string GetAll = "user";                // GET
        public const string Login = "user/login";           // POST
        public const string Logout = "user/logout";         // POST
        public const string GetByEmail = "user/{login}";      // GET
        public const string UpdateByEmail = "user/{login}";   // PUT
        public const string DeleteByEmail = "user/{login}";   // DELETE
    }

}
