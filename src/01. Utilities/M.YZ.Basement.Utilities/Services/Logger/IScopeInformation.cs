namespace M.YZ.Basement.Utilities.Services.Logger;
public interface IScopeInformation
{
    Dictionary<string, string> HostScopeInfo { get; }
    Dictionary<string, string> RequestScopeInfo { get; }
}

