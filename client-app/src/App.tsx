import React, { useState, useEffect } from 'react';
import './App.css';
import axios from 'axios';
import { Header, List } from 'semantic-ui-react';


function App() {
const [activites, setActivities] = useState([]);//set to inital state

useEffect(() => {
  axios.get('http://localhost:5000/api/activities').then(res=>{
    console.log(res);
    setActivities(res.data);
  })
}, [])

  return (
    <div>
      <Header as='h2' icon='users' content='Reactivities' />
      <List>
        {activites.map((activity: any) =>(
          <li key={activity.id}>
            {activity.title}
          </li>
        ))}
      </List>
            {/* curly braces allow you to directly write js code */}
    </div>
  );
}

export default App;
