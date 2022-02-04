using M.YZ.Basement.Utilities.Configurations;
using M.YZ.Infra.Auth.ControllerDetectors.Models;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace M.YZ.Infra.Auth.ControllerDetectors.ASPServices;

public class ApplicationPartDetector
{
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
    private readonly BasementConfigurationOptions _basementConfigurationOptions;

    public ApplicationPartDetector(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
        BasementConfigurationOptions basementConfigurationOptions)
    {
        _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        _basementConfigurationOptions = basementConfigurationOptions;
    }
    public ApplicationData? Detect()
    {

        var ControllerActionDescriptors = _actionDescriptorCollectionProvider.ActionDescriptors.Items.OfType<ControllerActionDescriptor>().ToList();

        List<ControllerData> controllersDatas = ControllerActionDescriptors.Select(c => c.ControllerName).Distinct().Select(c => new ControllerData { Name = c }).ToList();

        controllersDatas = controllersDatas.GroupJoin(ControllerActionDescriptors, c => c.Name, a => a.ControllerName, (c, a) => new ControllerData
        {
            Name = c.Name,
            ActionDatas = a.Select(b => new ActionData
            {
                Name = b.ActionName
            }).ToList()
        }).ToList();

        return controllersDatas != null && controllersDatas.Count > 0 ? new ApplicationData
        {
            ServiceName = _basementConfigurationOptions.ServiceName,
            ControllerDatas = controllersDatas
        } : null;


    }
}
