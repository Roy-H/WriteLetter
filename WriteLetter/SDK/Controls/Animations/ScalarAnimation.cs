using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace AppCore.SDK.Controls.Animations
{
    /// <summary>
    /// Animation that animates a value of type float
    /// </summary>
    public class ScalarAnimation : TypedAnimationBase<ScalarKeyFrame, double>
    {
        /// <inheritdoc/>
        protected override KeyFrameAnimation GetTypedAnimationFromCompositor(Compositor compositor)
        {
            return compositor.CreateScalarKeyFrameAnimation();
        }

        /// <inheritdoc/>
        protected override void InsertKeyFrameToTypedAnimation(KeyFrameAnimation animation, ScalarKeyFrame keyFrame)
        {
            (animation as ScalarKeyFrameAnimation).InsertKeyFrame((float)keyFrame.Key, (float)keyFrame.Value);
        }
    }
}
