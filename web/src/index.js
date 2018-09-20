import React from 'react';
import ReactDOM from 'react-dom';

import MasterPage from "./MasterPage";
import MasterPageLogin from "./MasterPageLogin";

var page;
var loggedIn = localStorage.getItem("token");
var contribuinte = localStorage.getItem("contribuinte");

if(loggedIn && contribuinte)
{
	page = <MasterPage />
}
else {
	page = <MasterPageLogin />
}

ReactDOM.render(page, document.getElementById("root"));
