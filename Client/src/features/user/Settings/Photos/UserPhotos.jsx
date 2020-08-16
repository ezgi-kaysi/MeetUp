import React, { Fragment } from 'react';
import { Header, Card, Image, Button } from 'semantic-ui-react';

const UserPhotos = ({
  photos,
  profile,
  deletePhoto,
  setMainPhoto,
  loading
}) => {
  let filteredPhotos;
  if (photos) {
    filteredPhotos = photos.filter(photo => {
      return photo.Url !== profile.Image;
    });
  }
  return (
    <Fragment>
      <Header sub color='teal' content='All Photos' />

      <Card.Group itemsPerRow={5}>
        <Card>
          <Image src={profile.Image || '/assets/user.png'} />
          <Button positive>Main Photo</Button>
        </Card>
        {photos &&
          filteredPhotos.map(photo => (
            <Card key={photo.Id}>
              <Image src={photo.Url} />
              <div className='ui two buttons'>
                <Button
                  loading={loading}
                  onClick={() => setMainPhoto(photo)}
                  basic
                  color='green'
                >
                  Main
                </Button>
                <Button
                  onClick={() => deletePhoto(photo)}
                  basic
                  icon='trash'
                  color='red'
                />
              </div>
            </Card>
          ))}
      </Card.Group>
    </Fragment>
  );
};

export default UserPhotos;
