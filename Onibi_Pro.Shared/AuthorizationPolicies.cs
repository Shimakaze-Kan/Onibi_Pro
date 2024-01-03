namespace Onibi_Pro.Shared;
public static class AuthorizationPolicies
{
    public const string RegionalManagerAccess = "REGIONAL_MANAGER_ACCESS";
    public const string GlobalManagerAccess = "GLOBAL_MANAGER_ACCESS";
    public const string ManagerAccess = "MANAGER_ACCESS";
    public const string CourierAccess = "COURIER_ACCESS";
    public const string GlobalOrRegionalManagerAccess = "GLOBAL_OR_REGIONAL_MANAGER_ACCESS";
    public const string RegionalManagerOrManagerAccess = "REGIONAL_OR_MANAGER_ACCESS";
    public const string RegionalOrCourierOrManagerAccess = "REGIONAL_OR_COURIER_OR_MANAGER_ACCESS";
}
