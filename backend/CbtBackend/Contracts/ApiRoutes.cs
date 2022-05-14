namespace CbtBackend.Contracts;

public static class ApiRoutes {

    // api endpoints are given in pg 55. of the spec
    public static class User {
        public const string Register = "user";              // POST
        public const string GetAll = "user";                // GET
        public const string Login = "user/login";           // POST
        public const string Logout = "user/logout";         // POST
        public const string GetByUserId = "user/{userId}";      // GET
        public const string UpdateByUserId = "user/{userId}";   // PUT
        public const string DeleteByUserId = "user/{userId}";   // DELETE
    }

    public static class Evaluation {
        // evaluations (mood tests)
        public const string GetEvaluations = "moodtest";
        public const string GetEvaluation = "moodtest/{id}";

        public const string PostEvaluationResponse = "evaluation";  // POST
        public const string GetEvaluationResponse = "evaluation/{id}";
        public const string PutEvaluationResponse = "evaluation/{id}";
        public const string DeleteEvaluationResponse = "evaluation/{id}";
        public const string GetEvaluationResponseById = "evaluation/findByUserId";
        public const string GetEvaluationResponseByLogin = "evaluation/findByUserLogin";
    }

}
