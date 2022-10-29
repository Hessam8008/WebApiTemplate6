using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebApi.Configuration;

internal class RoutePrefixConvention : IControllerModelConvention
{
    private readonly AttributeRouteModel _routePrefix;

    public RoutePrefixConvention(string template = "api/[controller]")
    {
        _routePrefix = new AttributeRouteModel(new RouteAttribute(template));
    }

    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
            selector.AttributeRouteModel =
                selector.AttributeRouteModel is null
                    ? _routePrefix
                    : AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
    }
}