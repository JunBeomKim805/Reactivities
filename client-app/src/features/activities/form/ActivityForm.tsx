import { ChangeEvent, useState } from "react";
import { Button, Form, Segment } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";

interface Props {
  activity: Activity | undefined;
  closeForm: () => void;
  createOrEdit: (activity: Activity) => void;
  submitting: boolean;
}

export default function ActivityForm({
  activity: selectedActivity,
  closeForm,
  createOrEdit,
  submitting,
}: Props) {
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
    createOrEdit(activity);
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
          loading={submitting}
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
}
