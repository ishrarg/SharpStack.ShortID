using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace SharpStack
{
    public class ShortID
    {


        static HashSet<string> previousNumbers = new HashSet<string>();
        static Random rx = new Random();

        /// <summary>
        /// Generates a unique number of length 12 to 32 based on timestamp be used as a unique id
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string LongUniqueNumber(int length = 12)
        {

            while (true)
            {
                // Get the current timestamp
                long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                int l = length - 4;
                //get right 10 characters from string

                string strTimestamp = timestamp.ToString();
                int startIndex = (strTimestamp.Length > l ? strTimestamp.Length - l : 0);
                int endIndex = strTimestamp.Length - startIndex;
                strTimestamp = strTimestamp.Substring(startIndex, endIndex);

                if (previousNumbers.Count > 99999)
                {
                    previousNumbers.Clear();
                }
                var prePattern = "00";
                var postPattern = "";
                if (strTimestamp.Length < (length - 2))
                {
                    for (int i = 0; i < length - strTimestamp.Length - 2; i++)
                    {
                        postPattern += "0";
                    }

                }

                int postMax = Convert.ToInt32(postPattern.Replace("0", "9"));
                string post = rx.Next(00, postMax).ToString(postPattern);
                //if (post.Length > postPattern.Length)
                //{
                //    post = post.Substring(post.Length - postPattern.Length, postPattern.Length);
                //}
                string pre = rx.Next(0, 99).ToString(prePattern);
                string shortUniqueId = $"{pre}{strTimestamp}{post}";
                if (previousNumbers.Contains(shortUniqueId))
                {
                    //seed++;
                    continue;
                }
                else
                {
                    previousNumbers.Add(shortUniqueId);
                    return shortUniqueId;
                }
            }

        }



        /// <summary>
        /// Generates 8 Digit Unique Number based on Timestamp to be used as a unique id
        /// </summary>
        /// <returns></returns>
        public static string ShortUniqueNumber()
        {
            if (previousNumbers.Count > 9999999)
            {
                previousNumbers.Clear();
            }


            while (true)
            {
                // Get the current timestamp
                long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                //get right 10 characters from string
                string strTimestamp = timestamp.ToString().Substring(timestamp.ToString().Length - 6, 6);



                string post = rx.Next(00, 9).ToString("0");
                string pre = rx.Next(0, 9).ToString("0");
                string shortUniqueId = $"{pre}{strTimestamp}{post}";
                if (previousNumbers.Contains(shortUniqueId))
                {
                    //seed++;
                    continue;
                }
                else
                {
                    previousNumbers.Add(shortUniqueId);
                    return shortUniqueId;
                }
            }

        }

        /// <summary>
        /// Generate Unique Alphanumeric String between length 6 to 22 to be used as a unique id
        /// </summary>
        /// <param name="length">Between 6 to 22</param>
        /// <returns></returns>
        public static string UniqueID(int length = 22)
        {
            if (length < 6)
            {
                length = 6;
            }
            else if (length > 22)
            {
                length = 22;
            }

            if (previousNumbers.Count > 9999999)
            {
                previousNumbers.Clear();
            }
            while (true)
            {
                string uniqueId;
                if (length < 15)
                    uniqueId = GenerateRandomString(length);
                else
                    uniqueId = UniqueFromBase64(length);
                if (previousNumbers.Contains(uniqueId))
                {
                    continue;
                }
                else
                {
                    previousNumbers.Add(uniqueId);
                    return uniqueId;
                }

            }
        }
        private static string UniqueFromBase64(int length)
        {
            var g = Guid.NewGuid();
            var ba = g.ToByteArray();
            var base64 = Convert.ToBase64String(ba);
            var firsttwentytwo = base64.Substring(0, length);
            var uniqueId = firsttwentytwo.Replace("/", "_").Replace("+", "-");

            return uniqueId;

        }

        static string GetChecksum(long unixTimestamp)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            // Calculate the remainder when dividing the timestamp by the number of letters
            int remainder = (int)(unixTimestamp % letters.Length);

            // Use the remainder as an index to get a character from the letters string
            char checksum = letters[remainder];

            return checksum.ToString();
        }
        static string GenerateRandomString(int length)
        {
            long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            // get year month day from timestamp
            //string strTimestamp = timestamp.ToString().Substring(timestamp.ToString().Length - 10, 10);
            string checksum = GetChecksum(timestamp);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            char[] randomString = new char[length - 1];
            for (int i = 0; i < length - 1; i++)
            {
                randomString[i] = chars[rx.Next(chars.Length)];
            }

            return checksum + (new string(randomString));
        }
    }
}
