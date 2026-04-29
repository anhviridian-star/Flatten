using System;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceAccess
{
    public static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void Register<T>(T service)
    {
        Type type = typeof(T);

        if (!services.ContainsKey(type))
        {
            services.Add(type, service);
        }
    }

    public static void Unregister<T>() 
    {
        Type type = typeof(T);
        if (services.ContainsKey(type)) 
        { 
            services.Remove(type);
        }
    }

    public static T Get<T>()
    {
        Type type = typeof(T);

        if (services.TryGetValue(type, out object service))
        {
            return (T)service;
        }

        throw new Exception($"Service of type {type} not registered");
    }
}
