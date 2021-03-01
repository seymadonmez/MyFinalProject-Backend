using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        //hash oluşturma
        public static void CreatePasswordHash(string password, out byte[] passwordHash,out byte[] passwordSalt) //out dışarıya verilecek değer
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        //hash doğrulama, saltını kullanarak doğruluyoruz
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) //out olmamalı çünkü bu değerleri biz vereceğiz
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            
        }
    }
}
