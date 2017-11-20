using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace AppCore.SDK.Controls.Animations
{
    /// <summary>
    /// Animation that animates a value of type <see cref="Vector3"/>
    /// </summary>
    public class Vector3Animation : TypedAnimationBase<Vector3KeyFrame, string>
    {
        /// <inheritdoc/>
        protected override KeyFrameAnimation GetTypedAnimationFromCompositor(Compositor compositor)
        {
            return compositor.CreateVector3KeyFrameAnimation();
        }

        /// <inheritdoc/>
        protected override void InsertKeyFrameToTypedAnimation(KeyFrameAnimation animation, Vector3KeyFrame keyFrame)
        {
            (animation as Vector3KeyFrameAnimation).InsertKeyFrame((float)keyFrame.Key, keyFrame.Value.ToVector3());
        }
    }
}
