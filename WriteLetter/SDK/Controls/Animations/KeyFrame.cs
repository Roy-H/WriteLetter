﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AppCore.SDK.Controls.Animations
{
    /// <summary>
    /// Provides common Dependency properties for KeyFrames
    /// </summary>
    public abstract class KeyFrame : DependencyObject
    {
        /// <summary>
        /// Identifies the <see cref="Key"/> dependency property
        /// </summary>
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register(nameof(Key), typeof(double), typeof(KeyFrame), new PropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the key of the key frame
        /// Value should be between 0.0 and 1.0
        /// </summary>
        public double Key
        {
            get { return (double)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }
    }
}
