using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AppCore.Helper
{
    public class ControlsHelper
    {
        public static ICollection<TextBlock> ConvertText2TextBlocks(string text)
        {
            ICollection<TextBlock> textBlocks = new List<TextBlock>();
            if (string.IsNullOrEmpty(text))
                return null;
            foreach (var i in text.ToCharArray())
            {
                textBlocks.Add(new TextBlock() { Text = i.ToString() });
            }
            return textBlocks;
        }
    }
}
