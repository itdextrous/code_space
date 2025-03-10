using System;
using Workfacta.Models;

namespace SAASCLOUDAPP.BackgroundProcessing.Library
{
    internal static class NumberExtensions
    {
        public static string LogInfo(this Number number) => $"{number.GlobalId} '{number.Description}'";
    }
}
