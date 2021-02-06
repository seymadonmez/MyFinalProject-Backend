using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class Category:IEntity
    {
        // Çıplak Class Kalmasın-> kural. bu kural sebebi ile Ientity interfaceini tanımladık.
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
