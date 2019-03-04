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
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Rock.CheckIn
{
    /// <summary>
    /// Object for maintaining the state of a check-in kiosk and workflow
    /// </summary>
    [DataContract]
    public class CheckInState 
    {
        /// <summary>
        /// Gets or sets the device id
        /// </summary>
        /// <value>
        /// The device id.
        /// </value>
        [DataMember]
        public int DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the checkin type identifier.
        /// </summary>
        /// <value>
        /// The checkin type identifier.
        /// </value>
        [DataMember]
        public int? CheckinTypeId
        {
            get { return _checkinTypeId; }
            set
            {
                _checkinTypeId = value;
                _checkinType = null;
            }
        }
        private int? _checkinTypeId;

        /// <summary>
        /// Gets the type of the current check in.
        /// </summary>
        /// <value>
        /// The type of the current check in.
        /// </value>
        public CheckinType CheckInType
        {
            get
            {
                if ( _checkinType != null )
                {
                    return _checkinType;
                }

                if ( CheckinTypeId.HasValue )
                {
                    _checkinType = new CheckinType( CheckinTypeId.Value );
                    return _checkinType;
                }

                return null;
            }

            set
            {
                _checkinType = value;
            }
        }

        private CheckinType _checkinType;

        /// <summary>
        /// Gets or sets a value indicating whether [manager logged in].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [manager logged in]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ManagerLoggedIn { get; set; }

        /// <summary>
        /// Gets or sets the configured group types.
        /// </summary>
        /// <value>
        /// The configured group types.
        /// </value>
        [DataMember]
        public List<int> ConfiguredGroupTypes { get; set; }

        /// <summary>
        /// Gets the kiosk.
        /// </summary>
        /// <value>
        /// The kiosk.
        /// </value>
        public KioskDevice Kiosk
        {
            get
            {
                return KioskDevice.Get( DeviceId, ConfiguredGroupTypes );
            }
        }

        /// <summary>
        /// Gets or sets the check-in status
        /// </summary>
        /// <value>
        /// The check-in.
        /// </value>
        [DataMember]
        public CheckInStatus CheckIn { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckInState" /> class.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="checkinTypeId">The checkin type identifier.</param>
        /// <param name="configuredGroupTypes">The configured group types.</param>
        public CheckInState( int deviceId, int? checkinTypeId, List<int> configuredGroupTypes )
        {
            DeviceId = deviceId;
            CheckinTypeId = checkinTypeId;
            ConfiguredGroupTypes = configuredGroupTypes;
            CheckIn = new CheckInStatus();
        }

        /// <summary>
        /// Creates a new CheckInState object Froms a json string.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static CheckInState FromJson( string json )
        {
            return JsonConvert.DeserializeObject( json, typeof( CheckInState ) ) as CheckInState;
          
        }

        /// <summary>
        /// Logs a message to an App_Data\Logs\checkin_yyyymmdd.csv file.
        /// </summary>
        public static void WriteToCheckInLog( CheckInState checkInState, string sourceType, string sourceName, string sourceState, string message )
        {
            if ( checkInState == null || checkInState.CheckInType == null || !checkInState.CheckInType.EnableLogging )
            {
                return;
            }

            try
            {
                string directory = AppDomain.CurrentDomain.BaseDirectory;
                directory = System.IO.Path.Combine( directory, "App_Data", "Logs" );
                if ( !System.IO.Directory.Exists( directory ) )
                {
                    System.IO.Directory.CreateDirectory( directory );
                }

                var when = RockDateTime.Now;
                string fileName = $"checkin_{when:yyyyMMdd}.csv";
                string filePath = System.IO.Path.Combine( directory, fileName );
                if ( !System.IO.File.Exists( filePath ) )
                {
                    System.IO.File.AppendAllText( filePath, $"Time,Device,Family,Source,SourceName,SourceState,Message,CurrentSelection\r\n" );
                }

                var selection = new StringBuilder();
                selection.Append( "Selection:" );

                foreach ( var family in checkInState.CheckIn?.GetFamilies( true ) )
                {
                    selection.Append( $"Family:{family.Group.Name}[{family.Group.Id}] " );
                    if ( family.CurrentPerson != null )
                    {
                        selection.Append( $"Current Person:{family.CurrentPerson.Person.FullName}[{family.CurrentPerson.Person.Id}] " );
                    }

                    foreach ( var person in family.GetPeople( true ) )
                    {
                        selection.Append( $"Person:{person.Person.FullName}[{person.Person.Id}] " );
                        foreach ( var groupType in person.GetGroupTypes( true ) )
                        {
                            selection.Append( $"Group Type:{groupType.GroupType.Name}[{groupType.GroupType.Id}] " );
                            foreach ( var group in groupType.GetGroups( true ) )
                            {
                                selection.Append( $"Group:{group.Group.Name}[{group.Group.Id}] " );
                                foreach ( var location in group.GetLocations( true ) )
                                {
                                    selection.Append( $"Location:{location.Location.Name}[{location.Location.Id}] " );
                                    foreach ( var schedule in location.GetSchedules( true ) )
                                    {
                                        selection.Append( $"Schedule:{schedule.Schedule.Name}[{schedule.Schedule.Id}] " );
                                    }
                                }
                            }
                        }
                    }

                    string timeStr = when.ToString( "HH:mm:ss.fff" );
                    System.IO.File.AppendAllText( filePath, $"{timeStr},{checkInState?.Kiosk?.Device?.Name}[{checkInState?.Kiosk?.Device?.Id}],FamilyId:{family.Group.Id},{sourceType},{sourceName},{sourceState},{message},{selection}\r\n" );

                }
            }
            catch { }
        }
    }
}