using System;
using System.Text;

namespace CommonHelper
{
    public static class HelperMsg
    {
        public static string GetAllMessages(Exception ex, string message = null)
        {
            if (ex == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            int level = 0;

            do
            {
                if (level > 0)
                {
                    sb.Append("  ");
                }

                if (!string.IsNullOrEmpty(ex.Message))
                {
                    sb.Append($"[Level-{level++}] ");
                    sb.Append(ex.Message);
                }

                ex = ex.InnerException;
            } while (ex != null);

            if (message != null)
            {
                if (level > 0)
                {
                    sb.Append("  ");
                }

                sb.Append($"[Level-{level}] ");
                sb.Append(message);
            }

            string strMessage = sb.ToString();
            if (strMessage.Length > 0)
            {
                return $"{strMessage}";
            }

            return string.Empty;
        }
    }
}