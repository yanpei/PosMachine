using System;
using FluentNHibernate.Mapping;

namespace PosApp.Services
{
    public class Promotion
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string barcode { get; set; }
    }

    public class PromotionMap : ClassMap<Promotion>
    {
        public PromotionMap()
        {
            Table("promotions");
            Id(p => p.Id);
            Map(p => p.Name);
            Map(p => p.barcode);

        }

    }
}