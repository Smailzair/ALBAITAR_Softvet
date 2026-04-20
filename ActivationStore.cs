using System;
using System.Security.Cryptography;
using System.Text;
namespace ALBAITAR_Softvet
{
    public class ActivationData
    {
        public decimal PrevRunningDelay { get; set; }
        public DateTime PrevSavedRunningDelayDate { get; set; }
        public bool IsFinanceDone { get; set; }
        public DateTime ServerVerifyDate { get; set; }
    }

    public static class ActivationStore
    {
        private const string SettingsKey = "AlBaitarActivationCodified";

        // Toggle to true if you want DPAPI encryption of stored payload
        private static readonly bool UseDpapi = false;

        public static ActivationData Load()
        {
            var stored = Properties.Settings.Default[SettingsKey] as string;
            if (string.IsNullOrEmpty(stored))
            {
                // nothing saved
                return new ActivationData
                {
                    PrevRunningDelay = 0m,
                    PrevSavedRunningDelayDate = new DateTime(2000, 1, 1),
                    IsFinanceDone = false,
                    ServerVerifyDate = DateTime.Now
                };
            }
            try
            {
                string payload = UseDpapi ? DecryptFromDpapi(stored) : stored;
                // expected format: part0|part1|part2|part3
                var parts = payload.Split(new[] { '|' }, 4);
                var ad = new ActivationData();
                
                if (parts.Length > 0) { decimal.TryParse(parts[0], out decimal d0); ad.PrevRunningDelay = d0; }
                if (parts.Length > 1)
                {
                    if (!string.IsNullOrEmpty(parts[1]))
                    {
                        DateTime.TryParse(PreConnection.Traduct_Codified_txt(parts[1]), out var dt1);
                        ad.PrevSavedRunningDelayDate = dt1;
                    }
                }
                if (parts.Length > 2)
                {
                    ad.IsFinanceDone = PreConnection.Traduct_Codified_txt(parts[2]) == "Yes_is_done";
                }
                if (parts.Length > 3)
                {
                    if (!string.IsNullOrEmpty(parts[3]))
                    {
                        DateTime.TryParse(PreConnection.Traduct_Codified_txt(parts[3]), out var dt3);
                        ad.ServerVerifyDate = dt3;
                    }
                }
                return ad;
            }
            catch
            {
                // fallback to defaults on corrupt data
                return new ActivationData
                {
                    PrevRunningDelay = 0m,
                    PrevSavedRunningDelayDate = new DateTime(2000, 1, 1),
                    IsFinanceDone = false,
                    ServerVerifyDate = DateTime.Now
                };
            }
        }

        public static void Save(ActivationData data)
        {
            string part0 = data.PrevRunningDelay.ToString("N2");
            string part1 = string.IsNullOrEmpty(data.PrevSavedRunningDelayDate.ToString()) ? "" : PreConnection.Codify_txt(data.PrevSavedRunningDelayDate.ToString());
            string part2 = PreConnection.Codify_txt(data.IsFinanceDone ? "Yes_is_done" : "Not_yet");
            string part3 = PreConnection.Codify_txt(data.ServerVerifyDate.ToString());

            string payload = string.Join("|", part0, part1, part2, part3);
            string stored = UseDpapi ? EncryptToDpapi(payload) : payload;
            Properties.Settings.Default[SettingsKey] = stored;
            Properties.Settings.Default.Save();
        }

        // Optional DPAPI helpers (CurrentUser scope)
        private static string EncryptToDpapi(string plain)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plain);
            var enc = ProtectedData.Protect(plainBytes, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(enc);
        }

        private static string DecryptFromDpapi(string base64)
        {
            var enc = Convert.FromBase64String(base64);
            var plainBytes = ProtectedData.Unprotect(enc, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(plainBytes);
        }

        // Migration helper: if old file exists, read and migrate to settings
        public static bool MigrateFromFileIfExists(string folderPath = "C:\\ProgramData\\BAITAR_CTRL", string fileName = "Al_Baitar_Activation.txt")
        {
            try
            {
                string filePath = System.IO.Path.Combine(folderPath, fileName);
                if (!System.IO.File.Exists(filePath)) return false;
                // read and attempt to parse exactly like original code did
                var fileLines = System.IO.File.ReadAllLines(filePath);
                var ad = new ActivationData
                {
                    PrevRunningDelay = 0m,
                    PrevSavedRunningDelayDate = new DateTime(2000, 1, 1),
                    IsFinanceDone = false,
                    ServerVerifyDate = DateTime.Now
                };
                if (fileLines.Length > 0 && !string.IsNullOrWhiteSpace(fileLines[0])) { decimal.TryParse(fileLines[0].Trim(), out decimal t0); ad.PrevRunningDelay = t0; }
                if (fileLines.Length > 1 && !string.IsNullOrWhiteSpace(fileLines[1]))
                {
                    DateTime.TryParse(PreConnection.Traduct_Codified_txt(fileLines[1]), out var dt1);
                    ad.PrevSavedRunningDelayDate = dt1;
                }
                if (fileLines.Length > 2 && !string.IsNullOrWhiteSpace(fileLines[2]))
                {
                    ad.IsFinanceDone = PreConnection.Traduct_Codified_txt(fileLines[2]) == "Yes_is_done";
                }
                if (fileLines.Length > 3 && !string.IsNullOrWhiteSpace(fileLines[3]))
                {
                    DateTime.TryParse(PreConnection.Traduct_Codified_txt(fileLines[3]), out var dt3);
                    ad.ServerVerifyDate = dt3;
                }
                Save(ad);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}