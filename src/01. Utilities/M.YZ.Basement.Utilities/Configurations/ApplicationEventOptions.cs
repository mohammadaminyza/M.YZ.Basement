namespace M.YZ.Basement.Utilities.Configurations;

public class ApplicationEventOptions
{
    public bool TransactionalEventsEnabled { get; set; } = true;
    public bool RaiseInmemoryEvents { get; set; } = false;
}

