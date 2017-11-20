

// *******************************************
// *** Auto-generated code. Do not modify! ***
// *******************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace AppCore
{
	/// <summary>
	/// Provide code access to string ressources.
	/// </summary>
	public static class Strings
	{
        #region String accessor properties

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static String IDS_APP_NAME { get { return AppCore.SDK.Helper.StringLoader.Instance.Load(StringId.IDS_APP_NAME); } }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static String IDS_BACK_EXIT_WARNING { get { return AppCore.SDK.Helper.StringLoader.Instance.Load(StringId.IDS_BACK_EXIT_WARNING); } }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static String IDS_TEST_STRING { get { return AppCore.SDK.Helper.StringLoader.Instance.Load(StringId.IDS_TEST_STRING); } }

        #endregion
    }

    /// <summary>
    /// IDs of all available strings.
    /// </summary>
    public static class StringId
    {
        public const String IDS_APP_NAME = "IDS_APP_NAME";
        public const String IDS_BACK_EXIT_WARNING = "IDS_BACK_EXIT_WARNING";
        public const String IDS_TEST_STRING = "IDS_TEST_STRING";
    }
}