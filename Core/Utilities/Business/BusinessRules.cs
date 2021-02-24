using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResult Run(params IResult[] logics) //params kullandığımız zaman istediğim kadar IResult tipinde parametre verebiliyorum
        {
            foreach (var logic in logics)
            {
                if (!logic.Success)
                {
                    return logic; // kurala uymayanı döndürür
                }
            }
            // dönüş tipi List<IResult> olsaydı
            //List<IResult> errorResults=new List<IResult>();
            //foreach (var logic in logics)
            //{
            //    if (!logic.Success)
            //    {
            //        return errorResults.Add(logic); // yapıda hata varsa direkt hatayı döndürür
            //    }
            //}
            return null;
        }
    }
}
