using proj_tt.Debugging;

namespace proj_tt
{
    public class proj_ttConsts
    {
        public const string LocalizationSourceName = "proj_tt";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "318c2be3a53c45cea4872597cb2ec290";
    }
}
