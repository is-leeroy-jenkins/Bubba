using System.Text;
using System.Security.Cryptography;

public static class Hasher
{
    /// <summary>
    /// Supported hash algorithms
    /// </summary>
    public enum HashType
    {
        HMAC, 
        HMACMD5, 
        HMACSHA1, 
        HMACSHA256, 
        HMACSHA384, 
        HMACSHA512,
        MACTripleDES, 
        MD5, 
        RIPEMD160, 
        SHA1, 
        SHA256, 
        SHA384, 
        SHA512
    }

    private static byte[] GetHash( string input, HashType hash )
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);

        switch (hash)
        {
            case HashType.HMAC:
                return HMAC.Create().ComputeHash(inputBytes);

            case HashType.HMACMD5:
                return HMACMD5.Create().ComputeHash(inputBytes);

            case HashType.HMACSHA1:
                return HMACSHA1.Create().ComputeHash(inputBytes);

            case HashType.HMACSHA256:
                return HMACSHA256.Create().ComputeHash(inputBytes);

            case HashType.HMACSHA384:
                return HMACSHA384.Create().ComputeHash(inputBytes);

            case HashType.HMACSHA512:
                return HMACSHA512.Create().ComputeHash(inputBytes);

            case HashType.MACTripleDES:
                return MACTripleDES.Create().ComputeHash(inputBytes);

            case HashType.MD5:
                return MD5.Create().ComputeHash(inputBytes);

            case HashType.RIPEMD160:
                return RIPEMD160.Create().ComputeHash(inputBytes);

            case HashType.SHA1:
                return SHA1.Create().ComputeHash(inputBytes);

            case HashType.SHA256:
                return SHA256.Create().ComputeHash(inputBytes);

            case HashType.SHA384:
                return SHA384.Create().ComputeHash(inputBytes);

            case HashType.SHA512:
                return SHA512.Create().ComputeHash(inputBytes);

            default:
                return inputBytes;
        }
    }

    /// <summary>
    /// Computes the hash of the string using a specified hash algorithm
    /// </summary>
    /// <param name="input">The string to hash</param>
    /// <param name="hashType">The hash algorithm to use</param>
    /// <returns>The resulting hash or an empty string on error</returns>
    public static string ComputeHash(this string input, HashType hashType)
    {
        try
        {
            byte[] hash = GetHash(input, hashType);
            StringBuilder ret = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
                ret.Append(hash[i].ToString("x2"));

            return ret.ToString();
        }
        catch
        {
            return string.Empty;
        }
    }
}