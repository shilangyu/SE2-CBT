namespace CbtBackend;

public static class UserRoles {
    // modify session in your account
    public const string SessionWrite = "write:session";

    // read your session
    public const string SessionRead = "read:session";

    // login to an account
    public const string UserRead = "read:user";

    // register new account
    public const string UserWrite = "write:user";

    // modify your evaluation
    public const string EvaluationWrite = "write:evaluation";

    // read your evaluation
    public const string EvaluationRead = "read:evaluation";

    // modify your statistics
    public const string StatisticsWrite = "write:statistics";

    // read your statistics
    public const string StatisticsRead = "read:statistics";
}
