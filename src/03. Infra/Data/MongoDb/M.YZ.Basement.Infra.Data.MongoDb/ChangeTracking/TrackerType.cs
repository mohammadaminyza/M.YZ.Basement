namespace M.YZ.Basement.Infra.Data.MongoDb.ChangeTracking;

public record TrackerType(string Name, Type Type, TrackedModelState TrackerState)
{
}