import { observer } from "mobx-react-lite";
import { ChangeEvent, useState } from "react";
import { Button, Form, Segment } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";

export default observer(function ActivityForm() {
  const { activityStore } = useStore();
  const {
    selectedActivity,
    closeForm,
    createActivity,
    updateActivity,
    loading,
  } = activityStore;

  const initialState = selectedActivity ?? {
    id: "",
    title: "",
    category: "",
    description: "",
    date: "",
    city: "",
    venue: "",
  };

  const [activity, setActivity] = useState(initialState);

  function handleSubmit() {
    activity.id ? updateActivity(activity) : createActivity(activity);
  }

  function handleInputChange(
    event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) {
    const { name, value } = event.target;
    setActivity({ ...activity, [name]: value });
  }

  return (
    <Segment clearing>
      <Form onSubmit={handleSubmit} autoComplete="off">
        <Form.Input
          placeholder="Title"
          onChange={handleInputChange}
          value={activity.title}
          name="title"
        />
        <Form.TextArea
          onChange={handleInputChange}
          value={activity.description}
          name="description"
          placeholder="Description"
        />
        <Form.Input
          onChange={handleInputChange}
          value={activity.category}
          name="category"
          placeholder="Category"
        />
        <Form.Input
          onChange={handleInputChange}
          value={activity.date}
          type="date"
          name="date"
          placeholder="Date"
        />
        <Form.Input
          onChange={handleInputChange}
          value={activity.city}
          name="city"
          placeholder="City"
        />
        <Form.Input
          onChange={handleInputChange}
          value={activity.venue}
          name="venue"
          placeholder="Venue"
        />
        <Button
          loading={loading}
          floated="right"
          positive
          type="submit"
          content="Submit"
        />
        <Button
          onClick={closeForm}
          floated="right"
          type="button"
          content="Cancel"
        />
      </Form>
    </Segment>
  );
});
