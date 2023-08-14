import { observer } from 'mobx-react-lite';
import { useEffect, useState } from 'react';
import { useParams, Link, useNavigate } from 'react-router-dom';
import { Button, Header, Segment } from 'semantic-ui-react';
import LoadingComponent from '../../../app/layout/LoadingComponent';
import { useStore } from '../../../app/stores/store';
import { Formik, Form } from 'formik';
import * as Yup from 'yup';
import MyTextInput from '../../../app/common/form/MyTextInput';
import MyTextArea from '../../../app/common/form/MyTextArea';
import { categoryOptions } from '../../../app/common/options/categoryOptions';
import MySelectInput from '../../../app/common/form/MySelectInput';
import MyDateInput from '../../../app/common/form/MyDateInput';
import { ActivityFormValues } from '../../../app/models/activity';
import {v4 as uuid} from 'uuid';

const ActivityForm = () => {
    const navigate = useNavigate();
    const {activityStore} = useStore();
    const {createActivity, updateActivity, loadActivity, loadingInitial} = activityStore;
    const {id} = useParams<{id: string}>();

    const [activity, setActivity] = useState<ActivityFormValues>( new ActivityFormValues());

    const validationSchema = Yup.object({
        title: Yup.string().required('This field is required'),
        description: Yup.string().required('This field is required'),
        category: Yup.string().required('This field is required'),
        date: Yup.string().required('Date is required').nullable(),
        venue: Yup.string().required('This field is required'),
        city: Yup.string().required('This field is required'),
    })

    useEffect(() => {
        if (id) loadActivity(id).then(activity => setActivity(new ActivityFormValues(activity)))
    }, [id, loadActivity]);

    function handleFormSubmit(activity : ActivityFormValues){
        if (!activity.id){
            let newActivity = {
                ...activity,
                id: uuid()
            };
            createActivity(newActivity).then(() => navigate(`/activities/${newActivity.id}`))
        } else{
            updateActivity(activity).then(() => navigate(`/activities/${activity.id}`))
        }
    }

    if (loadingInitial) return <LoadingComponent content='loading activity ...' />
    return (
        <Segment clearing>
            <Header content='Activity details' sub color='teal'/>
            <Formik 
            validationSchema = {validationSchema} 
            enableReinitialize initialValues={activity} onSubmit={values => handleFormSubmit(values)}
            >
                {({ handleSubmit, isValid, isSubmitting, dirty })=> (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput name="title" placeholder="Title" />
                        <MyTextArea rows={3} placeholder='Description' 
                        name='description'/>
                        <MySelectInput options={categoryOptions} placeholder='Category' 
                        name='category'/>
                        <MyDateInput placeholderText = 'Date'
                        name='date'
                        showTimeSelect
                        timeCaption='time'
                        dateFormat='MMMM d, yyyy h:mm aa'
                        />
                        <Header content='Location details' sub color='teal'/>
                        <MyTextInput placeholder='City'
                        name='city'/>
                        <MyTextInput placeholder='Venue'
                        name='venue'/>
                        <Button 
                            loading={isSubmitting} floated='right' positive type='submit' content='Submit'
                            disabled={isSubmitting || !dirty || !isValid}
                        />
                        <Button as={Link} to='/activities' floated='right' type='button' content='Cancel'/>
                    </Form>
                )}
            </Formik>
        </Segment>
    )
}

export default observer(ActivityForm)
