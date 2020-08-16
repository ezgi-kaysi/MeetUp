import React from "react";
import ReactDOM from "react-dom";
import { Provider } from 'react-redux';
import 'react-redux-toastr/lib/css/react-redux-toastr.min.css'
import "./index.css";
import App from "./app/layout/App";
import * as serviceWorker from "./serviceWorker";
import { BrowserRouter } from "react-router-dom";
import ScrollToTop from "./app/common/util/ScrollToTop";
import { configureStore } from './app/store/configureStore';
import ReduxToastr from 'react-redux-toastr';

const store = configureStore();

const rootEl = document.getElementById('root');

ReactDOM.render(
  <Provider store={store}>
    <BrowserRouter>
      <ScrollToTop>
          <ReduxToastr 
            position='bottom-right'
            transitionIn='fadeIn'
            transitionOut='fadeOut'
          />
        <App />
      </ScrollToTop>
    </BrowserRouter>,
    </Provider>,
    rootEl
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
