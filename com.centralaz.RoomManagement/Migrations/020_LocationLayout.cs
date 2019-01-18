// <copyright>
// Copyright by the Central Christian Church
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
using System;
using System.Data.Entity;
using System.Linq;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Plugin;

namespace com.centralaz.RoomManagement.Migrations
{
    [MigrationNumber( 20, "1.6.0" )]
    public class LocationLayout : Migration
    {
        public override void Up()
        {
            Sql( @"
                CREATE TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout](
	                [Id] [int] IDENTITY(1,1) NOT NULL,
                    [IsSystem] [bit] NOT NULL,
                    [LocationId] [int] NOT NULL,
                    [Name] [nvarchar](50) NULL,
	                [Description] [nvarchar](max) NULL,
	                [IsActive] [bit] NOT NULL,
	                [IsDefault] [bit] NOT NULL,
                    [LayoutPhotoId] [int] NULL,
	                [Guid] [uniqueidentifier] NOT NULL,
	                [CreatedDateTime] [datetime] NULL,
	                [ModifiedDateTime] [datetime] NULL,
	                [CreatedByPersonAliasId] [int] NULL,
	                [ModifiedByPersonAliasId] [int] NULL,
	                [ForeignKey] [nvarchar](50) NULL,
                    [ForeignGuid] [uniqueidentifier] NULL,
                    [ForeignId] [int] NULL,
                 CONSTRAINT [PK__com_centralaz_RoomManagement_LocationLayout] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                ) ON [PRIMARY]

                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout]  WITH CHECK ADD  CONSTRAINT [FK__com_centralaz_RoomManagement_LocationLayout_LocationId] FOREIGN KEY([LocationId])
                REFERENCES [dbo].[Location] ([Id])

                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout] CHECK CONSTRAINT [FK__com_centralaz_RoomManagement_LocationLayout_LocationId]

                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout]  WITH CHECK ADD  CONSTRAINT [FK__com_centralaz_RoomManagement_LocationLayout_CreatedByPersonAliasId] FOREIGN KEY([CreatedByPersonAliasId])
                REFERENCES [dbo].[PersonAlias] ([Id])

                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout] CHECK CONSTRAINT [FK__com_centralaz_RoomManagement_LocationLayout_CreatedByPersonAliasId]

                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout]  WITH CHECK ADD  CONSTRAINT [FK__com_centralaz_RoomManagement_LocationLayout_ModifiedByPersonAliasId] FOREIGN KEY([ModifiedByPersonAliasId])
                REFERENCES [dbo].[PersonAlias] ([Id])

                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout] CHECK CONSTRAINT [FK__com_centralaz_RoomManagement_LocationLayout_ModifiedByPersonAliasId]
" );
            Sql( @"
                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_ReservationLocation] ADD [LocationLayoutId] INT NULL

                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_ReservationLocation] WITH CHECK ADD CONSTRAINT [FK__com_centralaz_RoomManagement_ReservationLocation_LocationLayoutId] FOREIGN KEY([LocationLayoutId])
                REFERENCES [dbo].[_com_centralaz_RoomManagement_LocationLayout] ([Id])
" );
        }

        public override void Down()
        {
            Sql( @"
                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_ReservationLocation] DROP CONSTRAINT [FK__com_centralaz_RoomManagement_ReservationLocation_LocationLayoutId]
                ALTER TABLE[dbo].[_com_centralaz_RoomManagement_ReservationLocation] DROP COLUMN[LocationLayoutId]" );

            Sql( @"
                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout] DROP CONSTRAINT [FK__com_centralaz_RoomManagement_LocationLayout_LocationId]
                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout] DROP CONSTRAINT [FK__com_centralaz_RoomManagement_LocationLayout_CreatedByPersonAliasId]
                ALTER TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout] DROP CONSTRAINT [FK__com_centralaz_RoomManagement_LocationLayout_ModifiedByPersonAliasId]
                DROP TABLE [dbo].[_com_centralaz_RoomManagement_LocationLayout]" );
        }
    }
}