using UnityEngine;
using System;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute
{
    public string Name { get; }
    
    public CommandAttribute(string name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class CommandArgsAttribute : Attribute
{
    public string Name;
    public List<Type> ParamTypes = new List<Type>();

    public CommandArgsAttribute(string name, params Type[] paramsTypes)
    {
        Name = name;
        if (paramsTypes != null)
            ParamTypes.AddRange(paramsTypes);
    }
}
