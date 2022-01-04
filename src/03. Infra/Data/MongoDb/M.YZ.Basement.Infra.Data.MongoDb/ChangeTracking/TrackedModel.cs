namespace M.YZ.Basement.Infra.Data.MongoDb.ChangeTracking;

public enum TrackedModelState
{
    Added,
    Deleted,
    Update
}

internal class TrackedModel<T>
{
    public static TrackedModel<T> Add(T model) => new TrackedModel<T>(model, TrackedModelState.Added);
    public static TrackedModel<T> Update(T model) => new TrackedModel<T>(model, TrackedModelState.Update);

    public TrackedModel(T model, TrackedModelState state)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
        State = state;
    }

    public TrackedModelState State { get; }
    public T Model { get; }

    public TrackedModel<T> WithNewState(TrackedModelState newState)
    {
        return new TrackedModel<T>(Model, newState);
    }
}