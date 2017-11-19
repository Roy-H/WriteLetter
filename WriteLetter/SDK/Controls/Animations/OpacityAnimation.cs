using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace AppCore.SDK.Controls.Animations
{
    /// <summary>
    /// ScalarAnimation that animates the <see cref="Visual.Opacity"/> property
    /// </summary>
    public class OpacityAnimation : ScalarAnimation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpacityAnimation"/> class.
        /// </summary>
        public OpacityAnimation()
        {
            Target = nameof(Visual.Opacity);
        }
    }
}
