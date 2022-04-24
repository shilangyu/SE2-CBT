namespace CbtBackend;

public static class Utilities {
    public static bool IsDocker() {
        return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    }
}
