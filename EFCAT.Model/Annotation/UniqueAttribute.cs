using EFCAT.Model.Configuration;
using EFCAT.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class UniqueAttribute : Attribute {

    public string? ErrorMessage { get; set; } = null;

    public UniqueAttribute() {

    }
}