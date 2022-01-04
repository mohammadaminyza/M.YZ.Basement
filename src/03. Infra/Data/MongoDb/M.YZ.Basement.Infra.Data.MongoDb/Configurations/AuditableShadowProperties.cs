namespace M.YZ.Basement.Infra.Data.MongoDb.Configurations;

public static class AuditableShadowProperties
{
    public static readonly string CreatedByUserId = nameof(CreatedByUserId);

    public static readonly string ModifiedByUserId = nameof(ModifiedByUserId);

    public static readonly string CreatedDateTime = nameof(CreatedDateTime);

    public static readonly string ModifiedDateTime = nameof(ModifiedDateTime);

}
