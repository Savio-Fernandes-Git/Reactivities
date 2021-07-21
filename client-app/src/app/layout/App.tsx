import React, { useState, useEffect } from 'react';
import { Container } from 'semantic-ui-react';
import { Activity } from '../models/acitivity';
import NavBar from './NavBar';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import {v4 as uuid} from 'uuid';
import agent from '../api/agent';
import LoadingComponent from './LoadingComponent';

function App() {
const [activities, setActivities] = useState<Activity[]>([]);//set to inital state
const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);//set to inital state
const [editMode, setEditMode] = useState(false);
const [loading, setLoading] = useState(true);
const [submitting, setSubmitting] = useState(false);

useEffect(() => {
  agent.Activities.list().then(res=>{
    let activities: Activity [] = [];
    res.forEach(activity => {
      activity.date = activity.date.split('T')[0];
      activities.push(activity);
    })
    // console.log(res);
    setActivities(activities);
    setLoading(false);
  })
}, [])

function handleSelectActivity(id:string) {
  setSelectedActivity(activities.find(x => x.id === id));
}
function handleCancelSelectActivity() {
  setSelectedActivity(undefined);
}

function handleFormOpen(id?: string){
  id ? handleSelectActivity(id) : handleCancelSelectActivity();
  setEditMode(true);
}

function handleFormClose(){
  setEditMode(false);
}

function handleCreateOrEditActivity(activity: Activity){
    setSubmitting(true);
    if(activity.id) {
      agent.Activities.update(activity).then(() => {
        setActivities([...activities.filter(x=> x.id !== activity.id), activity]);
        setSelectedActivity(activity);
        setEditMode(false);
        setSubmitting(false);
      })
    } else {
      activity.id = uuid();
      agent.Activities.create(activity).then(() => {
        setActivities([...activities,activity])
        setSelectedActivity(activity);
        setEditMode(false);
        setSubmitting(false);
      })
    }
}

function handleDeleteActivity(id: string){
  setSubmitting(true);
  agent.Activities.delete(id).then(() => {
    setActivities([...activities.filter(x => x.id !== id)])
    setSubmitting(false);
  })
}

if(loading) return <LoadingComponent content='Loading app' />
  return (
    <>
      <NavBar openForm={handleFormOpen} />
      <Container style={{marginTop: '7em'}}>
        <ActivityDashboard 
          activities={activities}  
          selectedActivity={selectedActivity}
          selectActivity={handleSelectActivity}
          cancelSelectActivity={handleCancelSelectActivity}
          editMode={editMode}
          openForm={handleFormOpen}
          closeForm={handleFormClose}
          createOrEdit={handleCreateOrEditActivity}
          deleteActivity={handleDeleteActivity}
          submitting={submitting}
        />
      </Container>
      {/* curly braces allow you to directly write js code */}
    </>
  );
}

export default App;
