import React from 'react';
import { Form, Segment, Button, Label, Divider } from 'semantic-ui-react';
import { reduxForm, Field } from 'redux-form';
import TextInput from '../../../app/common/form/TextInput';
import { login, socialLogin } from '../../../app/store/actions/authActions';
import { connect } from 'react-redux';
import ErrorMessage from '../../../app/common/form/ErrorMessage';
import SocialLogin from '../SocialLogin/SocialLogin';

const actions = {
  login,
  socialLogin
};

const LoginForm = ({ login,handleSubmit, error, submitFailed, socialLogin }) => {
  return (
    <Form size='large' onSubmit={handleSubmit(login)} autoComplete='off'  error={submitFailed}>
      <Segment>
        <Field
          name='email'
          component={TextInput}
          type='text'
          placeholder='Email Address'
        />
        <Field
          name='password'
          component={TextInput}
          type='password'
          placeholder='password'
        />
         {error && <ErrorMessage error={error}/>}
        <Button fluid size='large' color='teal'>
          Login
        </Button>
        <Divider horizontal>
          Or
        </Divider>
        <SocialLogin socialLogin={socialLogin} />
        
      </Segment>
    </Form>
  );
};

export default connect(
  null,
  actions
)(reduxForm({ form: 'loginForm' })(LoginForm));
