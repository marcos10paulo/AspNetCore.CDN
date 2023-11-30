using System.ComponentModel;

namespace AspNetCore.CDN.Enums
{
    public enum Extensions
    {
        [Description(".jpg")]
        jpg = 0,
        [Description(".tiff")]
        tiff = 1,
        pdf = 2
    }
}
