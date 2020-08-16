import React, { Component } from "react";
import { Grid } from "semantic-ui-react";
import { connect } from "react-redux";
import { compose } from "redux";
import EventDetailedHeader from "./EventDetailedHeader";
import EventDetailedInfo from "./EventDetailedInfo";
import EventDetailedChat from "./EventDetailedChat";
import EventDetailedSidebar from "./EventDetailedSidebar";
import { fetchEvent } from '../../../app/store/actions/eventActions';
import { objectToArray, createDataTree } from '../../../app/common/util/helpers';

// class EventDetailedPage extends Component {
    
//   // async componentDidMount() {
//   //   const { id } = this.props.match.params;
//   //   await this.props.fetchEvent(id);
//   // }


//   render() {
//     const { event } = this.props;
    
//     if (!this.props.event) {
//         return <div>Loading...</div>;
//     }
//     const attendees = event && event.attendees && objectToArray(event.attendees);
//     return (
//       <Grid>
//         <Grid.Column width={10}>
//         <EventDetailedHeader event={event} />
//           <EventDetailedInfo event={event} />
//           <EventDetailedChat />
//         </Grid.Column>
//         <Grid.Column width={6}>
//         <EventDetailedSidebar attendees={attendees} eventId={event.id} />
         
//         </Grid.Column>
//       </Grid>
//     );
//   }
// }

const EventDetailedPage = ({event}) => {
  return (
    <Grid>
      <Grid.Column width={10}>
        <EventDetailedHeader event={event} />
        <EventDetailedInfo event={event} />
        <EventDetailedChat />
      </Grid.Column>
      <Grid.Column width={6}>
       <EventDetailedSidebar attendees={event.attendees} />
      </Grid.Column>
    </Grid>
  );
};

const mapStateToProps = (state, ownProps) => {
  const eventId = ownProps.match.params.id;

  let event = {};

  if (eventId && state.events.length > 0) {
    event = state.events.filter(event => event.id === eventId)[0];
  }

  return {
    event
  };
};

// const mapDispatchToProps = {
//     fetchEvent
// };

export default connect(mapStateToProps)(EventDetailedPage);
