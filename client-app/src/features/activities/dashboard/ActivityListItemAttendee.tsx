import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Image, List, Popup } from "semantic-ui-react";
import { Activity } from "../../../app/models/activity";
import { Profile } from "../../../app/models/profile";
import ProfileCard from "../../profiles/ProfileCard";

interface Props {
  attendess: Profile[];
  activity: Activity;
}

export default observer(function ActivityListItemAttendee({
  attendess,
  activity: { host },
}: Props) {
  return (
    <List horizontal>
      {attendess.map(
        (attendee) =>
          attendee.username === host?.username && (
            <Popup
              hoverable
              key={attendee.username}
              trigger={
                <List.Item
                  key={attendee.username}
                  as={Link}
                  to={`/profiles/${attendee.username}`}
                >
                  <Image
                    size="mini"
                    circular
                    src={attendee.image || "/assets/user.png"}
                  />
                </List.Item>
              }
            >
              <Popup.Content>
                <ProfileCard profile={attendee} />
              </Popup.Content>
            </Popup>
          )
      )}
      {attendess.map(
        (attendee) =>
          attendee.username !== host?.username && (
            <Popup
              hoverable
              key={attendee.username}
              trigger={
                <List.Item
                  key={attendee.username}
                  as={Link}
                  to={`/profiles/${attendee.username}`}
                >
                  <Image
                    size="mini"
                    circular
                    src={attendee.image || "/assets/user.png"}
                  />
                </List.Item>
              }
            >
              <Popup.Content>
                <ProfileCard profile={attendee} />
              </Popup.Content>
            </Popup>
          )
      )}
    </List>
  );
});
