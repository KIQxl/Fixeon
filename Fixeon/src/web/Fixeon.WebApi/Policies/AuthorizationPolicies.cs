using Microsoft.AspNetCore.Authorization;

public static class AuthorizationPolicies
{
    public const string MasterAdminPolicy = "MasterAdminPolicy";
    public const string AdminPolicy = "AdminPolicy";
    public const string AnalystPolicy = "AnalystPolicy";
    public const string CommonUserPolicy = "CommonUserPolicy";

    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(MasterAdminPolicy, policy =>
            policy.RequireRole("MasterAdmin"));

        options.AddPolicy(AdminPolicy, policy =>
            policy.RequireRole("Admin", "MasterAdmin"));

        options.AddPolicy(AnalystPolicy, policy =>
            policy.RequireRole("Analista", "Admin", "MasterAdmin"));

        options.AddPolicy(CommonUserPolicy, policy =>
            policy.RequireRole("Usuario", "Analista", "Admin", "MasterAdmin"));
    }
}
