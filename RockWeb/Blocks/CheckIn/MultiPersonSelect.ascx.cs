﻿// <copyright>
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Rock;
using Rock.CheckIn;
using Rock.Model;

namespace RockWeb.Blocks.CheckIn
{
    [DisplayName("Person Select (Family Check-in)")]
    [Category("Check-in")]
    [Description("Lists people who match the selected family and provides option of selecting multiple.")]
    public partial class MultiPersonSelect : CheckInBlock
    {
        bool _hidePhotos = false;
        bool _autoCheckin = false;

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            rSelection.ItemDataBound += rSelection_ItemDataBound;
            rSelection.ItemCommand += RSelection_ItemCommand;

            string script = string.Format( @"
        function GetPersonSelection() {{
            var ids = '';
            $('div.checkin-person-list').find('i.fa-check-square').each( function() {{
                ids += $(this).closest('a').attr('person-id') + ',';
            }});
            if (ids == '') {{
                bootbox.alert('Please select at least one person');
                return false;
            }}
            else
            {{
                $('#{0}').button('loading')
                $('#{1}').val(ids);
                return true;
            }}
        }}

        $('a.btn-checkin-select').click( function() {{
            //$(this).toggleClass('btn-dimmed');
            $(this).find('i').toggleClass('fa-check-square').toggleClass('fa-square-o');
        }});

", lbSelect.ClientID, hfPeople.ClientID );
            ScriptManager.RegisterStartupScript( Page, Page.GetType(), "SelectPerson", script, true );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            RockPage.AddScriptLink( "~/Scripts/iscroll.js" );
            RockPage.AddScriptLink( "~/Scripts/CheckinClient/checkin-core.js" );

            var bodyTag = this.Page.Master.FindControl( "bodyTag" ) as HtmlGenericControl;
            if ( bodyTag != null )
            {
                bodyTag.AddCssClass( "checkin-multipersonselect-bg" );
            }

            if ( CurrentWorkflow == null || CurrentCheckInState == null )
            {
                NavigateToHomePage(); 
            }
            else
            {
                _autoCheckin = CurrentCheckInState.CheckInType.AutoSelectOptions.HasValue && CurrentCheckInState.CheckInType.AutoSelectOptions.Value == 1;
                _hidePhotos = CurrentCheckInState.CheckInType.HidePhotos;

                if ( !Page.IsPostBack )
                {
                    ClearSelection();

                    var family = CurrentCheckInState.CheckIn.CurrentFamily;
                    if ( family == null )
                    {
                        GoBack();
                    }

                    lFamilyName.Text = family.ToString();

                    BindData();
                }
            }
        }

        /// <summary>
        /// Handles the ItemDataBound event of the rSelection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterItemEventArgs"/> instance containing the event data.</param>
        private void rSelection_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            if ( e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem )
            {
                var pnlPhoto = e.Item.FindControl( "pnlPhoto" ) as Panel;
                pnlPhoto.Visible = !_hidePhotos;

                var pnlPerson = e.Item.FindControl( "pnlPerson" ) as Panel;
                pnlPerson.CssClass = ( _hidePhotos ? "col-md-10 col-sm-10 col-xs-8" : "col-md-10 col-sm-8 col-xs-6" ) + " family-personselect";

                if ( _autoCheckin )
                {
                    var pnlPersonButton = e.Item.FindControl( "pnlPersonButton" ) as Panel;
                    var pnlChangeButton = e.Item.FindControl( "pnlChangeButton" ) as Panel;
                    if ( pnlPersonButton != null && pnlChangeButton != null )
                    {
                        pnlPersonButton.CssClass = "col-xs-12 col-sm-9 col-md-10";
                        pnlChangeButton.Visible = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the ItemCommand event of the RSelection control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="RepeaterCommandEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void RSelection_ItemCommand( object source, RepeaterCommandEventArgs e )
        {
            if ( e.CommandName == "Change" )
            {
                int personId = e.CommandArgument.ToString().AsInteger();
            }
        }

        /// <summary>
        /// Handles the Click event of the lbSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbSelect_Click( object sender, EventArgs e )
        {
            if ( KioskCurrentlyActive )
            {
                var selectedPersonIds = hfPeople.Value.SplitDelimitedValues().AsIntegerList();

                var family = CurrentCheckInState.CheckIn.CurrentFamily;
                if ( family != null )
                {
                    foreach ( var person in family.People )
                    {
                        person.Selected = selectedPersonIds.Contains( person.Person.Id );
                        person.PreSelected = person.Selected;
                    }

                    ProcessSelection( maWarning );
                }
            }
        }

        protected void lbBack_Click( object sender, EventArgs e )
        {
            GoBack();
        }

        protected void lbCancel_Click( object sender, EventArgs e )
        {
            CancelCheckin();
        }

        /// <summary>
        /// Clear any previously selected people.
        /// </summary>
        private void ClearSelection()
        {
            foreach ( var family in CurrentCheckInState.CheckIn.Families )
            {
                foreach ( var person in family.People )
                {
                    person.ClearFilteredExclusions();
                    person.PossibleSchedules = new List<CheckInSchedule>();
                    person.Selected = false;
                    person.Processed = false;
                }
            }
        }

        private void BindData()
        {
            var family = CurrentCheckInState.CheckIn.CurrentFamily;
            if ( family != null )
            {
                rSelection.DataSource = family.People
                .OrderByDescending( p => p.FamilyMember )
                .ThenBy( p => p.Person.BirthYear )
                .ThenBy( p => p.Person.BirthMonth )
                .ThenBy( p => p.Person.BirthDay )
                .ToList();

                rSelection.DataBind();
            }
        }

        protected void ProcessSelection()
        {
            ProcessSelection( 
                maWarning, 
                () => CurrentCheckInState.CheckIn.CurrentFamily.GetPeople( true )
                    .SelectMany( p => p.GroupTypes.Where( t => !t.ExcludedByFilter ) ) 
                    .Count() <= 0,
                "<p>Sorry, there are currently not any available areas that the selected people can check into.</p>" );
        }

        protected string GetCheckboxClass( bool selected )
        {
            return selected ? "fa fa-check-square fa-3x" : "fa fa-square-o fa-3x";
        }

        protected string GetPersonImageTag( object dataitem )
        {
            var person = dataitem as Person;
            if ( person != null )
            {
                return Person.GetPersonPhotoUrl( person, 200, 200 );
            }
            return string.Empty;
        }

    }
}