using FluentMigrator;

namespace Pos.Migration
{
    [Migration(3)]
    public class CreatePromotionsTable: FluentMigrator.Migration
    {
        public override void Up()
        {
            Create.Table("promotions")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("name").AsString(64).NotNullable()
                .WithColumn("barcode").AsString(64).NotNullable();

        }

        public override void Down()
        {
            Delete.Table("promotions");
        }
    }
}