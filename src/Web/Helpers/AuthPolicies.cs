namespace Web.Helpers;

public static class AuthPolicies
{
    /// <summary>
    /// Policy to ensure that the caller (client) is allowed to call this API. Should be set as default in startup.
    /// </summary>
    public const string RequireApiScope = "RequireApiScope";
}