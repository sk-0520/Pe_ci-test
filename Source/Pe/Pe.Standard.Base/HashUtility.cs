using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base
{
    public static class HashUtility
    {
        #region function

        /// <summary>
        /// .NET7 で使えなくなった <see cref="HashAlgorithm.Create(string)"/> のラッパー。
        /// </summary>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"><c>System.Security.*</c>系を指定。</exception>
        /// <exception cref="NotImplementedException"></exception>
        public static HashAlgorithm Create(string algorithmName)
        {
            switch(algorithmName.ToUpperInvariant()) {
                case "SHA":
                case "SHA1":
                    return SHA1.Create();

                case "SHA256":
                case "SHA-256":
                    return SHA256.Create();

                case "SHA384":
                case "SHA-384":
                    return SHA384.Create();

                case "SHA512":
                case "SHA-512":
                    return SHA512.Create();

                case "MD5":
                    return MD5.Create();

                case "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA1":
                case "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA256":
                case "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA384":
                case "SYSTEM.SECURITY.CRYPTOGRAPHY.SHA512":
                case "SYSTEM.SECURITY.CRYPTOGRAPHY.MD5":
                case "SYSTEM.SECURITY.CRYPTOGRAPHY.HASHALGORITHM":
                    throw new NotSupportedException(algorithmName);

                default:
                    throw new NotImplementedException(algorithmName);
            }
        }

        #endregion
    }
}
