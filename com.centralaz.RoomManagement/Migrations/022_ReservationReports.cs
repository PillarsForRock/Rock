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
using System.Text;
using com.centralaz.RoomManagement.ReportTemplates;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Plugin;
using Rock.Web.Cache;

namespace com.centralaz.RoomManagement.Migrations
{
    [MigrationNumber( 22, "1.6.0" )]
    public class ReservationReports : Migration
    {
        public override void Up()
        {
            //throw new NotImplementedException();
            RockMigrationHelper.UpdateCategory( Rock.SystemGuid.EntityType.DEFINED_TYPE, "Room Management", "", "", "731C5F16-62EA-4DE0-A1FC-6EE2263BF816" );
            RockMigrationHelper.AddDefinedType( "Room Management", "Reservation Reports", "Printable Reports used by the Room Management System", "13B169EA-A090-45FF-8B11-A9E02776E35E", @"" );
            RockMigrationHelper.AddDefinedTypeAttribute( "13B169EA-A090-45FF-8B11-A9E02776E35E", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "Lava", "Lava", "If the Lava Template is selected, this is the lava that will be used in the report", 3, "", "2F0BEBBA-B890-46B1-8C36-A3F7CE9A36B9" );
            RockMigrationHelper.AddDefinedTypeAttribute( "13B169EA-A090-45FF-8B11-A9E02776E35E", "6B88A513-4B4C-403B-ADFA-82C3A2B1C3B8", "Report Template", "ReportTemplate", "", 0, "", "1C2F3975-B1E2-4F8A-B2A2-FEF8D1A37E6C" );
            RockMigrationHelper.AddDefinedTypeAttribute( "13B169EA-A090-45FF-8B11-A9E02776E35E", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Report Font", "ReportFont", "", 1, "", "98F113C0-8497-48BC-9DA3-C51D163206CB" );
            RockMigrationHelper.AddDefinedTypeAttribute( "13B169EA-A090-45FF-8B11-A9E02776E35E", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Report Logo", "ReportLogo", "URL to the logo (PNG) to display in the printed report.", 2, "", "E907AB6D-642C-4079-AD08-0641B4C84B16" );
            RockMigrationHelper.AddAttributeQualifier( "2F0BEBBA-B890-46B1-8C36-A3F7CE9A36B9", "editorHeight", "", "4178EED6-EA71-41AF-ABD5-29D58E4626DD" );
            RockMigrationHelper.AddAttributeQualifier( "2F0BEBBA-B890-46B1-8C36-A3F7CE9A36B9", "editorMode", "3", "C1723802-3281-47CA-B8C6-B64573782A23" );
            RockMigrationHelper.AddAttributeQualifier( "2F0BEBBA-B890-46B1-8C36-A3F7CE9A36B9", "editorTheme", "0", "9B2F9E78-24F1-49CE-A1DF-AECC61678CDB" );
            RockMigrationHelper.AddAttributeQualifier( "98F113C0-8497-48BC-9DA3-C51D163206CB", "ispassword", "False", "FB3955AE-16D1-4051-8A18-E53028F70958" );
            RockMigrationHelper.AddAttributeQualifier( "98F113C0-8497-48BC-9DA3-C51D163206CB", "maxcharacters", "", "FFF9CA66-0867-4843-B82E-051A58A26F0D" );
            RockMigrationHelper.AddAttributeQualifier( "98F113C0-8497-48BC-9DA3-C51D163206CB", "showcountdown", "False", "E59E009B-372D-4143-A655-4F1BA4F54D1C" );
            RockMigrationHelper.AddAttributeQualifier( "E907AB6D-642C-4079-AD08-0641B4C84B16", "ispassword", "False", "E103657B-F3A5-41D2-A940-D9D44A6FD70A" );
            RockMigrationHelper.AddAttributeQualifier( "E907AB6D-642C-4079-AD08-0641B4C84B16", "maxcharacters", "", "C7A4FF45-C785-4D54-B345-9A3B01D38141" );
            RockMigrationHelper.AddAttributeQualifier( "E907AB6D-642C-4079-AD08-0641B4C84B16", "showcountdown", "False", "21FA831C-483A-478B-AFAC-88926843C0D5" );

            var blockIdSql = SqlScalar( @"
                Select Id
                From Block
                Where [Guid] = 'F71B7715-EBF5-4CDF-867E-B1018B2AECD5'
                " );
            int? blockId = blockIdSql.ToString().AsIntegerOrNull();

            RockMigrationHelper.UpdateBlockTypeAttribute( "D0EC5F69-5BB1-4BCA-B0F0-3FE2B9267635", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Report Font", "ReportFont", "", "", 11, @"Gotham", "B9DA1FF2-10EA-4466-BD67-A4D62E03D703" );
            RockMigrationHelper.UpdateBlockTypeAttribute( "D0EC5F69-5BB1-4BCA-B0F0-3FE2B9267635", "9C204CD0-1233-41C5-818A-C5DA439445AA", "Report Logo", "ReportLogo", "", "URL to the logo (PNG) to display in the printed report.", 12, @"~/Plugins/com_centralaz/RoomManagement/Assets/Icons/Central_Logo_Black_rgb_165_90.png", "C05E0362-6C49-4D59-8DB7-7DCADAF19FDD" );
            RockMigrationHelper.UpdateBlockTypeAttribute( "D0EC5F69-5BB1-4BCA-B0F0-3FE2B9267635", "6B88A513-4B4C-403B-ADFA-82C3A2B1C3B8", "Report Template", "ReportTemplate", "", "The template for the printed report. The Default and Advanced Templates will generate a printed report based on the templates' hardcoded layout. The Lava Template will generate a report based on the lava provided below in the Report Lava Setting. Any other custom templates will format based on their developer's documentation.", 13, @"9b74314a-37e0-40f2-906c-2862c93f8888", "8E2EE6F2-54FC-4C3A-9C9A-54CEA34544F7" );
            RockMigrationHelper.UpdateBlockTypeAttribute( "D0EC5F69-5BB1-4BCA-B0F0-3FE2B9267635", "1D0D3794-C210-48A8-8C68-3FBEC08A6BA5", "Report Lava", "ReportLava", "", "If the Lava Template is selected, this is the lava that will be used in the report", 14, @"{% include '~/Plugins/com_centralaz/RoomManagement/Assets/Lava/ReservationReport.lava' %}", "69131013-4E48-468E-B2C2-CF19CEA26590" );

            var defaultFont = GetAttributeValueFromBlock( blockId, "B9DA1FF2-10EA-4466-BD67-A4D62E03D703".AsGuid() );
            var defaultLogo = GetAttributeValueFromBlock( blockId, "C05E0362-6C49-4D59-8DB7-7DCADAF19FDD".AsGuid() );
            var selectedReportTemplateGuid = GetAttributeValueFromBlock( blockId, "8E2EE6F2-54FC-4C3A-9C9A-54CEA34544F7".AsGuid() );
            var defaultLava = GetAttributeValueFromBlock( blockId, "69131013-4E48-468E-B2C2-CF19CEA26590".AsGuid() );

            var selectedDefinedValueGuid = "";
            var allReportTemplates = ReportTemplateContainer.Instance.Components.Values
                .Where( v => v.Value.IsActive == true )
                .Select( v => v.Value.EntityType );

            var reportTemplateList = allReportTemplates
                .ToList();

            if ( reportTemplateList.Any() )
            {
                foreach ( EntityTypeCache reportTemplate in reportTemplateList )
                {
                    var definedValueGuidString = Guid.NewGuid().ToString();
                    var reportTemplateGuidString = reportTemplate.Guid.ToString();
                    var valueName = "";
                    var description = "";
                    var lavaCode = "";

                    if ( selectedReportTemplateGuid == reportTemplateGuidString )
                    {
                        selectedDefinedValueGuid = definedValueGuidString;
                    }

                    switch ( reportTemplateGuidString )
                    {
                        case "9b74314a-37e0-40f2-906c-2862c93f8888": // Event Based
                            valueName = "Event-Based Report";
                            description = "This is default report that can be printed out.";
                            definedValueGuidString = "5D53E2F0-BA82-4154-B996-085C979FACB0";
                            break;
                        case "97a7ffda-1b75-473f-a680-c9a7602b5c60": // Location Based
                            valueName = "Location-Based Report";
                            description = "Meant primarily for facilities teams, this report has a line item for each reservation location containing information about requested layouts.";
                            definedValueGuidString = "46C855B0-E50E-49E7-8B99-74561AFB3DD2";
                            break;
                        case "7ef82cca-7874-4b8d-adb7-896f05095354": // Lava
                            valueName = "Lava Report";
                            description = "This is a generic Lava Report.";
                            lavaCode = defaultLava;
                            definedValueGuidString = "71CEBC9E-D9BA-432D-B1C9-9B3D5CB8E7ED";
                            break;
                        default:
                            valueName = reportTemplate.FriendlyName;
                            break;

                    }

                    RockMigrationHelper.UpdateDefinedValue( "13B169EA-A090-45FF-8B11-A9E02776E35E", valueName, description, definedValueGuidString, true );
                    RockMigrationHelper.AddDefinedValueAttributeValue( definedValueGuidString, "1C2F3975-B1E2-4F8A-B2A2-FEF8D1A37E6C", reportTemplateGuidString );
                    RockMigrationHelper.AddDefinedValueAttributeValue( definedValueGuidString, "2F0BEBBA-B890-46B1-8C36-A3F7CE9A36B9", lavaCode );
                    RockMigrationHelper.AddDefinedValueAttributeValue( definedValueGuidString, "98F113C0-8497-48BC-9DA3-C51D163206CB", defaultFont );
                    RockMigrationHelper.AddDefinedValueAttributeValue( definedValueGuidString, "E907AB6D-642C-4079-AD08-0641B4C84B16", defaultLogo );
                }
            }

            RockMigrationHelper.UpdateBlockTypeAttribute( "D0EC5F69-5BB1-4BCA-B0F0-3FE2B9267635", "59D5A94C-94A0-4630-B80A-BB25697D74C7", "Visible Report Options", "VisibleReportOptions", "", "The Reservation Reports that the user is able to select", 21, @"", "BB36C64E-E379-4B34-BC91-BD65FCEEBBF7" );

            StringBuilder sb = new StringBuilder();
            sb.Append( "97a7ffda-1b75-473f-a680-c9a7602b5c60," );
            sb.Append( selectedDefinedValueGuid );
            RockMigrationHelper.AddBlockAttributeValue( "F71B7715-EBF5-4CDF-867E-B1018B2AECD5", "BB36C64E-E379-4B34-BC91-BD65FCEEBBF7", sb.ToString() );
        }

        public override void Down()
        {

        }

        private string GetAttributeValueFromBlock( int? blockId, Guid attributeGuid )
        {
            object valueSql = null;
            if ( blockId.HasValue )
            {
                valueSql = SqlScalar( String.Format( @"
                Select av.Value
                From AttributeValue av
                Join Attribute a on av.AttributeId = a.Id
                Where av.EntityId = {0}
                And a.Guid = '{1}'
                ",
                blockId.Value,
                attributeGuid.ToString() ) );
            }

            if ( valueSql == null )
            {
                valueSql = SqlScalar( String.Format( @"
                Select a.DefaultValue
                From Attribute a
                Where a.Guid = '{0}'
                ",
                attributeGuid.ToString() ) );
            }

            if ( valueSql != null )
            {
                return valueSql.ToString();
            }
            else
            {
                return string.Empty;
            }

        }
    }
}