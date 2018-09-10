import React from "react";
import { BrowserRouter as Router, Route } from "react-router-dom";

import { Login, Cadastro, SelecionarContribuinte } from "./pages";

export default class MasterPageLogin extends React.Component {
	render() {
		return (
			<div className="panel-login middle-box">
                <div className="logo">
                    <img src="./imagens/Intech.png" alt="Intech" />
                </div>

				<Router basename={process.env.PUBLIC_URL}>
					<div>
						<Route exact path="/" component={Login} />
						<Route path="/cadastro" component={Cadastro} />
						<Route path="/selecionarContribuinte" component={SelecionarContribuinte} />
					</div>
				</Router>
                <br/>
                <br/>

			</div>
		)
	}
}