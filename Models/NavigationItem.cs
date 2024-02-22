using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;

namespace JsonDbGui.Models
{
    public class NavigationItem
    {
        public NavigationItem(Type type, string? iconKey = null, string? alias = null)
        {
            ModelType = type;
            Alias = alias is null ? type.Name.Replace("PageViewModel", "") : alias;

            if (string.IsNullOrEmpty(iconKey))
                iconKey = "question_regular";

            Application.Current!.TryFindResource(iconKey, out var res);
            if (res is not null)
                NavigationItemIcon = (StreamGeometry)res;
        }

        public string Alias { get; }
        public Type ModelType { get; }
        public StreamGeometry NavigationItemIcon { get; }
    }
}
