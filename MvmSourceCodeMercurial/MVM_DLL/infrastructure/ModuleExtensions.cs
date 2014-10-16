using System;
using System.Collections.Generic;
using System.Text;

namespace MVM
{
    public static class ModuleExtensions
    {
        public static string ReadOrDefault(this IReadString parsedSyntax, ModuleContext mc, string defaultValue)
        {
            if (parsedSyntax != null) return parsedSyntax.Read(mc);
            return defaultValue;
        }

        public static string ReadOrNull(this IReadString parsedSyntax, ModuleContext mc)
        {
            if (parsedSyntax != null) return parsedSyntax.Read(mc);
            return null;
        }

        public static string ReadOrNullString(this IReadString parsedSyntax, ModuleContext mc)
        {
            if (parsedSyntax != null) return parsedSyntax.Read(mc);
            return "";
        }
    }
}
