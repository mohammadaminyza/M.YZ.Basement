﻿namespace M.YZ.Infra.Auth.ControllerDetectors.Models;
public class ApplicationData
{
    public Guid Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public List<ControllerData> ControllerDatas { get; set; } = new List<ControllerData>();
}
