import React, { Component } from "react";
import { Grid, Button, Loader } from "semantic-ui-react";
import { connect } from "react-redux";
import EventList from "../EventList/EventList";
import {createEvent, deleteEvent, updateEvent, loadEvents} from "../../../app/store/actions/eventActions";
import EventActivity from '../EventActivity/EventActivity';
import LoadingComponent from '../../../app/layout/LoadingComponent';

class EventDashboard extends Component {
 
  async componentDidMount() {
    let next = await this.props.loadEvents();
    
  }

  
  handleDeleteEvent = id => {
    this.props.deleteEvent(id);
  };

  render() {
    const {events, loading} = this.props;
    if (loading) return <LoadingComponent  />
    return (
      <Grid>
        <Grid.Column width={10}>
          <EventList
            events={events}
            deleteEvent={this.handleDeleteEvent}
          />
        </Grid.Column>
        <Grid.Column width={6}>
          <EventActivity  />
        </Grid.Column>
      </Grid>
    );
  }
}

const mapStateToProps = state => ({
  events: state.events,
  loading: state.async.loading
});

const mapStateToDispatch = {
  createEvent, 
  deleteEvent,
  updateEvent,
  loadEvents
};

export default connect(mapStateToProps, mapStateToDispatch)(EventDashboard);
