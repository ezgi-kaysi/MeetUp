import React from 'react';
import FacebookLogin from 'react-facebook-login/dist/facebook-login-render-props';
import { Button, Icon } from 'semantic-ui-react';

const SocialLogin = ({socialLogin}) => {
    return (
        <div>
            <FacebookLogin 
                appId="2642172902739022"
                fields="name,email,picture"
                callback={socialLogin}
                render={(renderProps) => {
                    return (
                        <Button onClick={renderProps.onClick} type="button" fluid color="facebook">
                            <Icon name="facebook" />
                            Login with Facebook
                        </Button>
                    )
                } }
            />
        </div>
    )
}


export default SocialLogin;
