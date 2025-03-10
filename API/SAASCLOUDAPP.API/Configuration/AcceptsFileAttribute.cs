using System;

namespace SAASCLOUDAPP.API.Configuration
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class AcceptsFileAttribute : Attribute
    {
        public AcceptsFileAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; }
    }
}