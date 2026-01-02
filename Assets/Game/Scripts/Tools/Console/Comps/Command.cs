using System.Collections.Generic;
using System.Reflection;
using System;

public struct Command
{
    public Command(string name, MethodInfo method, object target, List<Type> paramTypes)
    {
        Name = name; 
        Method = method;
        Target = target;
        ParamTypes = paramTypes;
    }

    public string Name;
    public MethodInfo Method;
    public object Target;
    public List<Type> ParamTypes;
}
