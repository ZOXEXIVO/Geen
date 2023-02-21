using System;
using Geen.Web.Application.Formatter.Bindings;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Geen.Web.Application.Attributes;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class)]
public class FromJsonUriAttribute : Attribute, IModelNameProvider, IBinderTypeProviderMetadata
{
    public FromJsonUriAttribute()
    {
        BinderType = typeof(JsonModelBinder);
        BindingSource = BindingSource.Custom;
    }

    public Type BinderType { get; set; }

    public BindingSource BindingSource { get; set; }

    public string Name { get; set; }
}