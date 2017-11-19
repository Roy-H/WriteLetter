using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace AppCore.SDK.Controls.Animations
{
    /// <summary>
    /// Vector3Animation that animates the <see cref="Visual"/> Translation property
    /// <seealso cref="ElementCompositionPreview.SetIsTranslationEnabled"/>
    /// </summary>
    public class TranslationAnimation : Vector3Animation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationAnimation"/> class.
        /// </summary>
        public TranslationAnimation()
        {
            Target = "Translation";
        }

        /// <inheritdoc/>
        public override CompositionAnimation GetCompositionAnimation(Compositor compositor)
        {
            if (ApiInformationHelper.IsCreatorsUpdateOrAbove)
            {
                return base.GetCompositionAnimation(compositor);
            }
            else
            {
                return null;
            }
        }
    }
}
