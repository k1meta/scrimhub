namespace ScrimHub.Infrastructure;

public static class LocalEnv
{
    public const string SqlConnectionVariable = "SQL_CONNECTION";

    public static string? TryReadSqlConnection(string? startDirectory = null)
    {
        var fromProcess = Environment.GetEnvironmentVariable(SqlConnectionVariable);
        if (!string.IsNullOrWhiteSpace(fromProcess))
        {
            return fromProcess;
        }

        var envFile = FindEnvFile(startDirectory ?? Directory.GetCurrentDirectory());
        if (envFile is null)
        {
            return null;
        }

        foreach (var line in File.ReadLines(envFile))
        {
            var trimmed = line.Trim();
            if (trimmed.Length == 0 || trimmed.StartsWith('#'))
            {
                continue;
            }

            var splitAt = trimmed.IndexOf('=');
            if (splitAt <= 0)
            {
                continue;
            }

            var key = trimmed[..splitAt].Trim();
            if (!string.Equals(key, SqlConnectionVariable, StringComparison.Ordinal))
            {
                continue;
            }

            var value = trimmed[(splitAt + 1)..].Trim().Trim('"');
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        return null;
    }

    private static string? FindEnvFile(string startDirectory)
    {
        var directory = new DirectoryInfo(startDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, ".env");
            if (File.Exists(candidate))
            {
                return candidate;
            }

            directory = directory.Parent;
        }

        return null;
    }
}