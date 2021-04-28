// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class GroupSalutation : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.AnalyticsSourceGivingUnit",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GivingId = c.String(maxLength: 20),
                        GivingLeaderPersonId = c.Int(nullable: false),
                        GivingSalutation = c.String(),
                        GivingSalutationFull = c.String(),
                        GivingBin = c.Int(nullable: false),
                        GivingPercentile = c.Int(nullable: false),
                        GiftAmountMedian = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GiftAmountIqr = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GiftFrequencyMean = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GiftFrequencyStandardDeviation = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PercentGiftsScheduled = c.Int(nullable: false),
                        Frequency = c.String(maxLength: 250),
                        PreferredCurrencyValueId = c.Int(nullable: false),
                        PreferredCurrency = c.String(maxLength: 250),
                        PreferredSourceValueId = c.Int(nullable: false),
                        PreferredSource = c.String(maxLength: 250),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.Int(),
                        ForeignGuid = c.Guid(),
                        ForeignKey = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Guid, unique: true);
            
            AddColumn("dbo.Group", "GroupSalutation", c => c.String(maxLength: 250));
            AddColumn("dbo.Group", "GroupSalutationFull", c => c.String(maxLength: 250));
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropIndex("dbo.AnalyticsSourceGivingUnit", new[] { "Guid" });
            DropColumn("dbo.Group", "GroupSalutationFull");
            DropColumn("dbo.Group", "GroupSalutation");
            DropTable("dbo.AnalyticsSourceGivingUnit");
        }
    }
}
